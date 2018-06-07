using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using Packsly3.Core.Modpack;

namespace Packsly3.Core.Launcher.Instance {

    public class FileManager : IDisposable {

        public readonly ReadOnlyDictionary<GroupType, List<FileInfo>> FileMap;

        private readonly WebClient _client = new WebClient();
        private readonly IMinecraftInstance _instance;

        public bool IsDirty { private set; get; }

        public FileManager(IMinecraftInstance instance) {
            instance.PackslyConfig.Load();
            FileMap = new ReadOnlyDictionary<GroupType, List<FileInfo>>(instance.PackslyConfig.ManagedFiles);
            _instance = instance;
        }

        public void Add(FileInfo file, GroupType group) {
            if (!file.Exists) {
                throw new FileNotFoundException($"File with path '{file.FullName}' cound not be added to file manager because it does not exist");
            }

            if (FileMap.ContainsKey(group)) {
                if (FileMap[group].Any(f => f.FullName.GetHashCode() == file.FullName.GetHashCode())) {
                    throw new DuplicateNameException($"Group '{group}' allready contains file with path '{file.FullName}'");
                }
            } else {
                _instance.PackslyConfig.ManagedFiles.Add(group, new List<FileInfo>());
            }

            _instance.PackslyConfig.ManagedFiles[group].Add(file);

            IsDirty = true;
        }

        public void Add(string path, GroupType group)
            => Add(new FileInfo(path), group);

        public void Download(string url, string destination, GroupType group) {
            FileInfo destinationFile = new FileInfo(destination);

            if (destinationFile.Directory != null && !destinationFile.Directory.Exists) {
                destinationFile.Directory.Create();
            }

            _client.DownloadFile(url, destination);
            Add(destinationFile, group);
        }

        public void Download(RemoteResource resource, GroupType group) =>
            Download(resource.Url.ToString(), _instance.EnvironmentVariables.Format(Path.Combine(resource.FilePath, resource.FileName)), group);

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

            if (fileGroup.Count == 0) {
                _instance.PackslyConfig.ManagedFiles.Remove(group);
            }

            IsDirty = true;
        }

        public void Remove(string path, GroupType group)
            => Remove(new FileInfo(path), group);

        public FileInfo[] GetMissingFiles(GroupType group) {
            if (!FileMap.ContainsKey(group)) {
                return new FileInfo[0];
            }

            List<FileInfo> fileGroup = FileMap[group];
            return fileGroup.Where(f => !f.Exists).ToArray();
        }

        public void Save() {
            if (!IsDirty)
                return;

            _instance.PackslyConfig.Save();
            IsDirty = false;
        }

        #region IDisposable

        private bool _disposed;

        protected virtual void Dispose(bool disposing) {
            if (!_disposed) {
                if (disposing) {
                    _client.Dispose();
                }
            }

            _disposed = true;
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
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
