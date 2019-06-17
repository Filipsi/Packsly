using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using NLog;
using Packsly3.Core.Modpack.Model;

namespace Packsly3.Core.Launcher.Instance {

    public class FileManager {

        #region Properties

        public readonly ReadOnlyDictionary<GroupType, List<FileInfo>> FileMap;

        public bool IsDirty { private set; get; }

        #endregion

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IMinecraftInstance instance;

        #endregion

        public FileManager(IMinecraftInstance instance) {
            FileMap = new ReadOnlyDictionary<GroupType, List<FileInfo>>(instance.PackslyConfig.ManagedFiles);
            this.instance = instance;

            // Remove missing or non-existent files
            foreach (GroupType group in Enum.GetValues(typeof(GroupType))) {
                FileInfo[] missingFiles = GetNotExistingFiles(group);

                if (missingFiles.Length <= 0)
                    continue;

                foreach (FileInfo file in missingFiles) {
                    logger.Debug("Removing non-existing tracked file.");
                    Remove(file, group);
                }
            }
        }

        public void Add(FileInfo file, GroupType group) {
            if (!file.Exists) {
                throw new FileNotFoundException($"There was error while trying to add file with path '{file.FullName}' to the file manager. Specified file does not exist.");
            }

            if (FileMap.ContainsKey(group)) {
                if (FileMap[group].Any(f => f.FullName.GetHashCode() == file.FullName.GetHashCode())) {
                   return;
                }
            } else {
                instance.PackslyConfig.ManagedFiles.Add(group, new List<FileInfo>());
            }

            instance.PackslyConfig.ManagedFiles[group].Add(file);
            logger.Debug($"Tracked file {file.FullName} was added to minecraft instance {instance.Id}");
            IsDirty = true;
        }

        public void Add(string path, GroupType group)
            => Add(new FileInfo(path), group);

        public void Download(string url, string destination, GroupType group) {
            FileInfo destinationFile = new FileInfo(destination);

            if (destinationFile.Directory != null && !destinationFile.Directory.Exists) {
                destinationFile.Directory.Create();
            }

            logger.Debug($"Downloading file from {url}...");
            using (WebClient client = new WebClient()) {
                client.DownloadFile(url, destination);
            }

            Add(destinationFile, group);
        }

        public void Download(RemoteResource resource, GroupType group) =>
            Download(resource.Url.ToString(), instance.EnvironmentVariables.Format(Path.Combine(resource.FilePath, resource.FileName)), group);

        public void Remove(FileInfo file, GroupType group) {
            if (!FileMap.ContainsKey(group)) {
                return;
            }

            List<FileInfo> fileGroup = FileMap[group];
            FileInfo toRemove = file;

            if (!fileGroup.Contains(file)) {
                toRemove = fileGroup.FirstOrDefault(f => f.FullName.GetHashCode() == file.FullName.GetHashCode());
            }

            if (toRemove == null) {
                throw new FileNotFoundException($"Group '{group}' does not contain file with path '{file.FullName}'");
            }

            fileGroup.Remove(toRemove);
            logger.Debug($"Tracked file {file.FullName} was removed from minecraft instance {instance.Id}");

            if (fileGroup.Count == 0) {
                logger.Debug($"Removing tracking group {group} from minecraft instance {instance.Id}, because it is empty.");
                instance.PackslyConfig.ManagedFiles.Remove(group);
            }

            IsDirty = true;
        }

        public void Remove(string path, GroupType group)
            => Remove(new FileInfo(path), group);

        public void Remove(RemoteResource resource, GroupType group)
            => Remove(new FileInfo(instance.EnvironmentVariables.Format(Path.Combine(resource.FilePath, resource.FileName))), group);

        #region Utilities

        public bool GroupContains(GroupType group, RemoteResource resource)
            => GetGroup(group).Any(m => m.FullName == instance.EnvironmentVariables.Format(Path.Combine(resource.FilePath, resource.FileName)));

        public bool GroupContains(GroupType group, FileInfo file)
            => GetGroup(group).Any(m => m.FullName.GetHashCode() == file.FullName.GetHashCode());

        public FileInfo[] GetGroup(GroupType group)
            => !FileMap.ContainsKey(group) ? new FileInfo[0] : FileMap[group].ToArray();

        private FileInfo[] GetNotExistingFiles(GroupType group) {
            return !FileMap.ContainsKey(group)
                ? new FileInfo[0]
                : FileMap[group].Where(f => !f.Exists).ToArray();
        }

        #endregion

        public enum GroupType {
            PackslyInternal,
            Mod,
            ModResource,
            Miscellaneous
        }

    }

}
