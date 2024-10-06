using NetFileFormat.Extend.FileFormat.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFileFormat.Extend.FileFormat
{
    public class AudioFormatItem : BaseLocalFileFormatItem
    {
        public override string GetFilePath(string[] files,string[] folders, string fileName, object extendObj)
        {
            return GetSelectFilePath(files, fileName, "mp3", "ogg", "wav");
        }

        public override string GetFilePath(string filePath, string fileName, object extendObj)
        {
            return GetSelectFilePath(filePath, fileName, "mp3", "ogg", "wav");
        }

        public override object GetTargetObjectContent(string absPath, Type type, TypeCode typeCode)
        {
            return default;
        }

        public override void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value)
        {
            CopyFile(absPath, type, typeCode, value);
        }
    }
}
