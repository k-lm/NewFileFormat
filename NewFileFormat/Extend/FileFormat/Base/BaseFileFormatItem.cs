using NetFileFormat.FAnnotation;
using NetFileFormat.FormatModel.Base;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetFileFormat.Extend.FileFormat.Base
{
    public abstract class BaseFileFormatItem : IFileFormatItem
    {
        public abstract string GetFilePath(string[] files, string fileName, object extendObj);
        public abstract string GetFilePath(string filePath, string fileName, object extendObj);

        public abstract string GetSaveFileAbsPath(string folder, string name, object value);


        /// <summary>
        /// 返回具体数值
        /// </summary>
        /// <param name="absPath"></param>
        /// <param name="type"></param>
        /// <param name="typeCode"></param>
        /// <param name="fileContentType"></param>
        /// <returns></returns>
        public abstract object GetTargetObjectContent(string absPath, Type type, TypeCode typeCode);
        public abstract void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value);

        /// <summary>
        /// 返回文件名称-不包含文件扩展名
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileName"></param>
        /// <param name="extendObj"></param>
        /// <returns></returns>
        public virtual string GetFileNameWithoutExtension(string absPath)
        {
            return Path.GetFileNameWithoutExtension(absPath);
        }

        /// <summary>
        /// 返回文件名称-包含文件扩展名
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileName"></param>
        /// <param name="extendObj"></param>
        /// <returns></returns>
        public virtual string GetFileFileName(string absPath)
        {
            return Path.GetFileName(absPath);
        }

        /// <summary>
        /// 返回文件所在目录名称
        /// </summary>
        /// <param name="files"></param>
        /// <param name="fileName"></param>
        /// <param name="extendObj"></param>
        /// <returns></returns>
        public virtual string GetFileFolderName(string absPath)
        {
            return Path.GetDirectoryName(absPath);
        }


        /// <summary>
        /// 返回具体数值
        /// </summary>
        /// <param name="absPath"></param>
        /// <param name="type"></param>
        /// <param name="typeCode"></param>
        /// <param name="fileContentType"></param>
        /// <returns></returns>
        public object GetObjectValue(string absPath, Type type, TypeCode typeCode, FileContentType fileContentType)
        {
            switch (fileContentType)
            {
                case FileContentType.Path:
                    return absPath;
                case FileContentType.Name:
                    return GetFileNameWithoutExtension(absPath);
                case FileContentType.FileName:
                    return GetFileFileName(absPath);
                case FileContentType.FolderName:
                    return GetFileFolderName(absPath);
                default:
                    return GetTargetObjectContent(absPath, type, typeCode);
            }
           
        }
     

        /// <summary>
        /// 是否是文件
        /// </summary>
        /// <returns></returns>
        public virtual bool IsObjectFile(Type type)
        {
            if(type.IsArray || typeof(System.Collections.IList).IsAssignableFrom(type))
            return false;

            return true;
        }


        public SerializedFile GetSerializedFile(PropertyInfo property)
        {
            object attributes = property.GetCustomAttributes(typeof(SerializedFile), false).FirstOrDefault();
            return attributes as SerializedFile;

        }


        /// <summary>
        /// 返回查找文件扩展名
        /// </summary>
        /// <param name="files">查找的文件</param>
        /// <param name="fileName">文件名</param>
        /// <param name="extensionNames">查找的扩展名</param>
        /// <returns></returns>
        public string GetSelectFilePath(string[] files, string fileName, params string[] extensionNames)
        {
            string filePath = files.Select(path => path)
               .Where(path =>
               {
                   if (!string.IsNullOrEmpty(fileName))
                   {
                       string name = GetFileNameNoExtension(path);
                       if (name != fileName) return false;
                   }
                   return extensionNames.Contains(GetFileExtensionName(path).Substring(1).ToLower());
               }).FirstOrDefault();
            return filePath;
        }

        /// <summary>
        /// 返回查找文件扩展名
        /// </summary>
        /// <param name="files">查找的文件</param>
        /// <param name="fileName">文件名</param>
        /// <param name="extensionNames">查找的扩展名</param>
        /// <returns></returns>
        public string GetSelectFilePath(string filePath, string fileName, params string[] extensionNames)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string name = GetFileNameNoExtension(filePath);
                if (name.Equals(fileName)) return null;
            }

            return extensionNames.Contains(GetFileExtensionName(filePath).Substring(1).ToLower())? filePath:null;
        }
        /// <summary>
        /// 返回文件扩展名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFileExtensionName(string path)
        {
            return Path.GetExtension(path);
        }
        /// <summary>
        /// 返回文件文件名(不包含扩展名)
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFileNameNoExtension(string path)
        {
            return Path.GetFileNameWithoutExtension(path);
        }

        /// <summary>
        /// 返回文件名
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string GetFileName(string path)
        {
            return Path.GetFileName(path);
        }
        /// <summary>
        /// 返回保存文件路径
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="name">名称</param>
        /// <param name="extendName">扩展名(扩展名要带.)</param>
        /// <returns></returns>
        public string GetSaveFilePath(string folder,string name,string extendName)
        {
            return folder + Path.DirectorySeparatorChar + name + extendName;
        }

        public string GetFilePath(SerializedFile serializedFile, string filePath, string fileName, object extendObj)
        {
            switch (serializedFile.ContentType)
            {
                case FType.FileContentType.Name:
                case FType.FileContentType.FileName:
                case FType.FileContentType.FolderName:
                    return null;
                default:
                   return GetFilePath(filePath, fileName, extendObj);
            }
        }

        public string GetSaveFileAbsPath(SerializedFile serializedFile, string folder, string name, object value)
        {
            switch (serializedFile.ContentType)
            {
                case FType.FileContentType.Name:
                case FType.FileContentType.FileName:
                case FType.FileContentType.FolderName:
                    return null;
                default:
                    return GetSaveFileAbsPath(folder, name, value);
            }
        }

      
    }
}
