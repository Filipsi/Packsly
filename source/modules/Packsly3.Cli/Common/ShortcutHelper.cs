using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packsly3.Cli.Common {

    internal static class ShortcutHelper {

        public static FileInfo GetShortcutTarget(FileInfo file) {
            using (FileStream fileStream = file.Open(FileMode.Open, FileAccess.Read))
            using (BinaryReader reader = new BinaryReader(fileStream)) {
                fileStream.Seek(0x14, SeekOrigin.Begin);
                uint flags = reader.ReadUInt32();
                if ((flags & 1) == 1) {
                    fileStream.Seek(0x4c, SeekOrigin.Begin);
                    uint offset = reader.ReadUInt16();
                    fileStream.Seek(offset, SeekOrigin.Current);
                }

                long fileInfoStartsAt = fileStream.Position;
                uint totalStructLength = reader.ReadUInt32();
                fileStream.Seek(0xc, SeekOrigin.Current);
                uint fileOffset = reader.ReadUInt32();

                fileStream.Seek((fileInfoStartsAt + fileOffset), SeekOrigin.Begin);
                long pathLength = (totalStructLength + fileInfoStartsAt) - fileStream.Position - 2;
                char[] linkTarget = reader.ReadChars((int)pathLength);
                string link = new string(linkTarget);

                int begin = link.IndexOf("\0\0");
                if (begin > -1) {
                    int end = link.IndexOf("\\\\", begin + 2) + 2;
                    end = link.IndexOf('\0', end) + 1;

                    string firstPart = link.Substring(0, begin);
                    string secondPart = link.Substring(end);

                    return new FileInfo(firstPart + secondPart);
                } else {
                    return new FileInfo(link);
                }
            }
        }

    }

}
