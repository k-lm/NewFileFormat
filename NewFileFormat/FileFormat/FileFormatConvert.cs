using NetFileFormat.Extend;
using NetFileFormat.Extend.FileFormat;
using NetFileFormat.FAnnotation;
using NetFileFormat.FormatModel.Base;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace NetFileFormat.FileFormat
{
    public class FileFormatConvert
    {

        private static FormatBuild mFormatBuild;

        private static ObjectLoadFormat mLoadFormat;

        private static ObjectSaveFormat mSaveFormat;

        private static bool isIni = false;

        public static void InitFormat()
        {
            if (isIni) return;
            isIni = true;

            AddFileFormat(DefaultFileType.IMAGE, new ImageFormatItem());
            AddFileFormat(DefaultFileType.TXT, new TxtFormatItem());
            AddFileFormat(DefaultFileType.VIDEO, new VideoFormatItem());
            AddFileFormat(DefaultFileType.INI, new IniFormatItem());
            AddFileFormat(DefaultFileType.Folder, new FolderFormatItem());
        }

        public static void AddFileFormat(string type, IFileFormatItem fileFormat)
        {
            if (mFormatBuild == null)
            {
                mFormatBuild = new FormatBuild();
            }

            mFormatBuild.AddFileFormat(type, fileFormat);
        }

        public static void RemoveFileFormat(string type)
        {
            if (mFormatBuild == null) return;
            mFormatBuild.RemoveFileFormater(type);
        }

        public static T DeserializeObject<T>(string folder) where T : new()
        {
            if (!Directory.Exists(folder)) return default;

            if (mFormatBuild == null)
            {
                InitFormat();
            }

            if (mLoadFormat == null)
            {
                mLoadFormat = new ObjectLoadFormat(mFormatBuild);
            }
            // 判断范型类型
            Type objType = typeof(T);
            T tarObj;
            if (objType.IsArray)
            {
                // 数组
                tarObj = (T)mLoadFormat.GetArrayValue(objType, folder, null);
            }
            else if (typeof(System.Collections.IList).IsAssignableFrom(objType))
            {
                // 列表
                tarObj = (T)mLoadFormat.GetListValue(objType, folder, null);
            }
            else
            {
                tarObj = (T)mLoadFormat.GetObjetValue(objType, folder, null);
            }

            return tarObj;
        }

        /// <summary>
        /// 保存本地
        /// </summary>
        /// <param name="folder"></param>
        public static void SerializeObject(string folder,object obj)  {
            if (obj == null) return;
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            if (mSaveFormat == null) mSaveFormat = new ObjectSaveFormat(mFormatBuild);
             Type objType = obj.GetType();
            if (objType.IsArray)
            {
                // 数组
                mSaveFormat.SaveArrayValue(obj, folder, null);
            }
            else if (typeof(System.Collections.IList).IsAssignableFrom(objType))
            {
                // 列表
                mSaveFormat.SaveListValue(obj, folder, null);
            }
            else
            {
                mSaveFormat.SaveObjetValue(obj, folder, null);
            }
        }
    }
}
