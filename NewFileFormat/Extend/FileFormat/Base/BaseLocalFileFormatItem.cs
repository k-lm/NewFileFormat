using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NetFileFormat.Extend.FileFormat.Base
{
   public abstract class BaseLocalFileFormatItem : BaseFileFormatItem
    {

       protected void CopyFile(string absPath, Type type, TypeCode typeCode, object value)
        {
            if (value == null) return;
            File.Copy(value.ToString(), absPath, true);
        }

        protected void MoveFile(string absPath, Type type, TypeCode typeCode, object value)
        {
            if (value == null) return;
            if (File.Exists(absPath))
            {
                File.Delete(absPath);
            }
            File.Move(value.ToString(), absPath);
        }

        public override string GetSaveFileAbsPath(string folder, string name, object value)
        {
            if (value == null) return null;
            string extendName = Path.GetExtension(value.ToString());
            return folder + Path.DirectorySeparatorChar + name + extendName;
        }

    }
}
