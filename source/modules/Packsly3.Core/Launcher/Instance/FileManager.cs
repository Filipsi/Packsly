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

        public readonly ReadOnlyDictionary<GroupType, List<string>> FileMap;

        public bool IsDirty { private set; get; }

        #endregion

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IMinecraftInstance instance;

        #endregion

        public FileManager(IMinecraftInstance instance) {
            this.instance = instance;

            // Setup file map
            FileMap = new ReadOnlyDictionary<GroupType, List<string>>(
                instance.PackslyConfig.ManagedFiles
            );

            // Remove missing or non-existent files
            foreach (GroupType group in Enum.GetValues(typeof(GroupType))) {
                FileInfo[] missingFiles = GetNotExistingFiles(group);

                if (missingFiles.Length == 0) {
                    continue;
                }

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
                if (FileMap[group].Any(f => f.GetHashCode() == file.FullName.GetHashCode())) {
                   return;
                }
            } else {
                instance.PackslyConfig.ManagedFiles.Add(group, new List<string>());
            }

            string environmentPath = instance.EnvironmentVariables.FromFormatedString(file.FullName);
            instance.PackslyConfig.ManagedFiles[group].Add(environmentPath);

            logger.Debug($"Tracked file {file.FullName} was added to minecraft instance {instance.Id} as {environmentPath}.");
            IsDirty = true;
        }

        public void Add(string path, GroupType group) {
            Add(new FileInfo(path), group);
        }

        public void Download(Uri source, string destination, GroupType group) {
            FileInfo destinationFile = new FileInfo(destination);

            if (destinationFile.Directory?.Exists == false) {
                destinationFile.Directory.Create();
            }

            logger.Debug($"Downloading file from {source}...");
            using (WebClient client = new WebClient()) {
                client.DownloadFile(source, destinationFile.FullName);
            }

            Add(destinationFile, group);
        }

        public void Download(RemoteResource resource, GroupType group) {
            Download(resource.Url, instance.EnvironmentVariables.ToFormatedString(Path.Combine(resource.FilePath, resource.FileName)), group);
        }

        public void Remove(FileInfo file, GroupType group) {
            if (!FileMap.ContainsKey(group)) {
                return;
            }

            List<string> fileGroup = FileMap[group];
            string environmentPath = instance.EnvironmentVariables.FromFormatedString(file.FullName);

            if (!fileGroup.Contains(environmentPath)) {
                environmentPath = fileGroup.Find(f => f.GetHashCode() == file.GetHashCode());
            }

            if (environmentPath == null) {
                throw new FileNotFoundException($"Group '{group}' does not contain file with path '{file.FullName}'");
            }

            fileGroup.Remove(environmentPath);
            logger.Debug($"Tracked file {file.FullName} was removed from minecraft instance {instance.Id}");

            if (fileGroup.Count == 0) {
                logger.Debug($"Removing tracking group {group} from minecraft instance {instance.Id}, because it is empty.");
                instance.PackslyConfig.ManagedFiles.Remove(group);
            }

            IsDirty = true;
        }

        public void Remove(string path, GroupType group) {
            Remove(new FileInfo(path), group);
        }

        public void Remove(RemoteResource resource, GroupType group) {
            Remove(new FileInfo(instance.EnvironmentVariables.ToFormatedString(Path.Combine(resource.FilePath, resource.FileName))), group);
        }

        #region Utilities

        public bool GroupContains(GroupType group, RemoteResource resource) {
            return GetGroup(group).Any(m => m.FullName == instance.EnvironmentVariables.ToFormatedString(Path.Combine(resource.FilePath, resource.FileName)));
        }

        public bool GroupContains(GroupType group, FileInfo file) {
            return GetGroup(group).Any(m => m.FullName.GetHashCode() == file.FullName.GetHashCode());
        }

        public FileInfo[] GetGroup(GroupType group) {
            if (FileMap.ContainsKey(group)) {
                return FileMap[group]
                    .Select(environmentPath => instance.EnvironmentVariables.ToFormatedString(environmentPath))
                    .Select(path => new FileInfo(path))
                    .ToArray();
            }

            return new FileInfo[0];
        }

        private FileInfo[] GetNotExistingFiles(GroupType group) {
            if (FileMap.ContainsKey(group)) {
                return FileMap[group]
                    .Select(environmentPath => instance.EnvironmentVariables.ToFormatedString(environmentPath))
                    .Where(path => !File.Exists(path))
                    .Select(path => new FileInfo(path))
                    .ToArray();
            }

            return new FileInfo[0];
        }

        #endregion

        public enum GroupType {
            Mod = 1,
            Resource = 2
        }

    }

}
