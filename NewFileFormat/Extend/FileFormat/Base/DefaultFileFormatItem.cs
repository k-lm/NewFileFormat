using NetFileFormat.Extend.FileFormat.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewFileFormat.Extend.FileFormat.Base
{
    public abstract class DefaultFileFormatItem : BaseFileFormatItem
    {
        public override string GetFilePath(string[] files,  string fileName, object extendObj)
        {
            string filePath = files.Select(path => path)
              .Where(path =>
              {
                  if (!string.IsNullOrEmpty(fileName))
                  {
                      return GetFileNameNoExtension(path) == fileName;
                  }
                  return false;
              }).FirstOrDefault();
            return filePath;
        }

        public override string GetFilePath(string filePath, string fileName, object extendObj)
        {
            return GetFileNameNoExtension(filePath) == fileName ? fileName : null;
        }

        public override string GetSaveFileAbsPath(string folder, string name, object value)
        {
            return null;
        }

        public override void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value)
        {
            return;
        }
    }
}
