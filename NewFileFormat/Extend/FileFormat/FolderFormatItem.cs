using NetFileFormat.Extend.FileFormat.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetFileFormat.Extend.FileFormat
{
    public class FolderFormatItem : BaseFileFormatItem
    {
        public override string GetFilePath(string[] files, string fileName, object extendObj)
        {
            string filePath = files.Select(path => path)
              .Where(path =>
              {
                  if (!string.IsNullOrEmpty(fileName))
                  {
                      return GetFileName(path) == fileName;
                  }
                  return false;
              }).FirstOrDefault();
            return filePath;
        }

        public override string GetFilePath(string filePath, string fileName, object extendObj)
        {
            return GetFileName(filePath) == fileName ? fileName : null;
        }

        public override string GetSaveFileAbsPath(string folder, string name, object value)
        {
            return GetSaveFilePath(folder, name, "");
        }

        public override object GetTargetObjectContent(string absPath, Type type, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.SByte:
                    return File.ReadAllBytes(absPath);
            }

            string value = File.ReadAllText(absPath);
            switch (typeCode)
            {
                case TypeCode.Boolean:
                 return   Boolean.Parse(value);
                case TypeCode.Int16:
                    return Int16.Parse(value);
                case TypeCode.UInt16:
                    return UInt16.Parse(value);
                case TypeCode.Int32:
                    return Int32.Parse(value);
                case TypeCode.UInt32:
                    return Int32.Parse(value);
                case TypeCode.Int64:
                    return Int64.Parse(value);
                case TypeCode.UInt64:
                    return UInt64.Parse(value);
                case TypeCode.Single:
                    return Single.Parse(value);
                case TypeCode.Double:
                    return Double.Parse(value);
                case TypeCode.String:
                    return value;
                default:
                    return default;
            }
        }

        public override bool IsObjectFile(Type type)
        {
            return false ;
        }
        public override void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value)
        {
            if (Directory.Exists(absPath)) return;
            Directory.CreateDirectory(absPath);
        }
    }
}
