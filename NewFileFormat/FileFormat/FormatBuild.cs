using NetFileFormat.FAnnotation;
using NetFileFormat.FormatModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFileFormat.FileFormat
{
    class FormatBuild
    {
        /// <summary>
        /// 文件夹排序号长度
        /// </summary>
        public int FolderNumLength { get; set; } = 4;

        public FormatBuild()
        {

        }

        private static Dictionary<string, IFileFormatItem> mFormatDictionary;

        /// <summary>
        /// 添加文件格式
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fileFormat"></param>
        public  void AddFileFormat(string type, IFileFormatItem fileFormat)
        {
            if (mFormatDictionary == null)
            {
                mFormatDictionary = new Dictionary<string, IFileFormatItem>();
            }

            mFormatDictionary.Add(type, fileFormat);
        }
        /// <summary>
        /// 移除文件格式
        /// </summary>
        /// <param name="type"></param>
        public  void RemoveFileFormater(string type)
        {
            if (mFormatDictionary == null) return;
            mFormatDictionary.Remove(type);
        }
       

        public IFileFormatItem GetFileFormatItem(SerializedFile  serializedFile)
        {
            if(mFormatDictionary == null || !mFormatDictionary.TryGetValue(serializedFile.FileType,out IFileFormatItem fileFormat))
            {
                return null;
            }

            return fileFormat;
        }
       


    }
}
