using NetFileFormat.FAnnotation;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFileFormat.FormatModel.Base
{
  public  interface IFileFormatItem
    {
        /// <summary>
        /// 是否是文件
        /// </summary>
        /// <returns></returns>
        bool IsObjectFile(Type type);
        /// <summary>
        /// 读取文件返回具体数值
        /// </summary>
        /// <param name="absPath">路径</param>
        /// <param name="type">对象类型</param>
        /// <param name="typeCode">对象类型</param>
        /// <param name="fileContentType">读取内容类型</param>
        /// <returns></returns>
        object GetObjectValue( string absPath, Type type,TypeCode typeCode,FileContentType fileContentType);
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="absPath">路径</param>
        /// <param name="type">对象类型</param>
        /// <param name="typeCode">对象类型</param>
        /// <param name="value">数值</param>
        void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value);


        /// <summary>
        /// 在目录中查找文件或目录
        /// </summary>
        /// <param name="files">目录文件/目录列表</param>
        /// <param name="fileName">文件名</param>
        /// <param name="extendObj">扩展信息</param>
        /// <returns></returns>
        string GetFilePath(string[] files, string fileName, object extendObj);
        /// <summary>
        /// 对比文件
        /// </summary>
        /// <param name="filePath">源文件路径</param>
        /// <param name="fileName">文件名</param>
        /// <param name="extendObj">扩展信息</param>
        /// <returns></returns>
        string GetFilePath(SerializedFile serializedFile, string filePath, string fileName, object extendObj);
        /// <summary>
        /// 返回保存文件绝对路径
        /// </summary>
        /// <param name="folder">目录</param>
        /// <param name="name">文件名</param>
        /// <returns></returns>
        string GetSaveFileAbsPath(SerializedFile serializedFile, string folder, string name, object value);


    }
}
