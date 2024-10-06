﻿using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFileFormat.FAnnotation
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class SerializedFile : Attribute
    {
        public SerializedFile(string name, FileContentType contentType, string fileType)
        {
            Name = name;
            this.ContentType = contentType;
            this.FileType = fileType;
        }

        public SerializedFile(string name,string fileType, FileContentType contentType, object extendObj)
        {
            Name = name;
            FileType = fileType;
            ContentType = contentType;
            ExtendObj = extendObj;
        }
        /// <summary>
        /// 文件或文件夹 名称(包括扩展名)
        /// </summary>
        public string Name { get; set; }

        public string FileType { get; set; }

        /// <summary>
        /// 文件类型，用于字符串,用于获取值还是路径 
        /// </summary>
        public FileContentType ContentType { get; set; } = FileContentType.Path;

        public object ExtendObj { get; set; }
    }
}
