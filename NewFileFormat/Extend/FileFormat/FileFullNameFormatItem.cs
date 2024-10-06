using NetFileFormat.Extend.FileFormat.Base;
using NewFileFormat.Extend.FileFormat.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetFileFormat.Extend.FileFormat
{
    public class FileFullNameFormatItem : DefaultFileFormatItem
    {

        public override object GetTargetObjectContent(string absPath, Type type, TypeCode typeCode)
        {
            return Path.GetFileName(absPath);
        }

        public override void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value)
        {

        }
    }
}
