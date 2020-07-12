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

        public bool IsDirty { private set; get; }

        #endregion

        #region Fields

        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IMinecraftInstance instance;

        #endregion

        public FileManager(IMinecraftInstance instance) {
            this.instance = instance;

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
            // Prevent from adding non-existing files
            if (!file.Exists) {
                throw new FileNotFoundException($"There was error while trying to add file with path '{file.FullName}' to the file manager. Specified file does not exist.");
            }

            // Check if the group we want to add file to exists
            if (!instance.PackslyConfig.ManagedFiles.ContainsKey(group)) {
                instance.PackslyConfig.ManagedFiles.Add(group, new List<string>());
            }

            // Resolve environmental path for newly added file
            string envAddedFilePath = instance.EnvironmentVariables.ToEnviromentalPath(file.FullName);

            // Make sure that the group we want to add file to don't already have it
            if (instance.PackslyConfig.ManagedFiles[group].Any(envExistingFile => envExistingFile == envAddedFilePath)) {
                return;
            }
            
            // Add file to the group
            instance.PackslyConfig.ManagedFiles[group].Add(envAddedFilePath);
            logger.Debug($"Tracked file {file.FullName} was added to minecraft instance {instance.Id} as {envAddedFilePath}.");

            // Mark manager as changed
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
            Download(resource.Url, resource.GetFilePath(instance.EnvironmentVariables), group);
        }

        public void Remove(FileInfo file, GroupType group) {
            if (!instance.PackslyConfig.ManagedFiles.ContainsKey(group)) {
                return;
            }

            string environmentPath = instance.EnvironmentVariables.ToEnviromentalPath(file.FullName);

            if (environmentPath == null) {
                logger.Warn($"Group '{group}' does not contain file with path '{file.FullName}'");
                return;
            }

            instance.PackslyConfig.ManagedFiles[group].Remove(environmentPath);
            logger.Debug($"Tracked file {file.FullName} was removed from minecraft instance {instance.Id}");

            if (instance.PackslyConfig.ManagedFiles[group].Count == 0) {
                logger.Debug($"Removing tracking group {group} from minecraft instance {instance.Id}, because it is empty.");
                instance.PackslyConfig.ManagedFiles.Remove(group);
            }

            if (file.Exists) {
                file.Delete();
            }

            IsDirty = true;
        }

        public void Remove(string path, GroupType group) {
            Remove(new FileInfo(path), group);
        }

        public void Remove(RemoteResource resource, GroupType group) {
            Remove(new FileInfo(resource.GetFilePath(instance.EnvironmentVariables)), group);
        }

        #region Utilities

        public bool GroupContains(GroupType group, RemoteResource resource) {
            return GetGroup(group).Any(m => m.FullName == resource.GetFilePath(instance.EnvironmentVariables));
        }

        public bool GroupContains(GroupType group, FileInfo file) {
            return GetGroup(group).Any(m => m.FullName.GetHashCode() == file.FullName.GetHashCode());
        }

        public FileInfo[] GetGroup(GroupType group) {
            if (instance.PackslyConfig.ManagedFiles.ContainsKey(group)) {
                return instance.PackslyConfig.ManagedFiles[group]
                    .Select(environmentPath => instance.EnvironmentVariables.FromEnviromentalPath(environmentPath))
                    .Select(path => new FileInfo(path))
                    .ToArray();
            }

            return new FileInfo[0];
        }

        private FileInfo[] GetNotExistingFiles(GroupType group) {
            if (instance.PackslyConfig.ManagedFiles.ContainsKey(group)) {
                return instance.PackslyConfig.ManagedFiles[group]
                    .Select(environmentPath => instance.EnvironmentVariables.FromEnviromentalPath(environmentPath))
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
