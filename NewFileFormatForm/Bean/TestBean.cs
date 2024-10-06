
using NetFileFormat.Extend;
using NetFileFormat.FAnnotation;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewFileFormatForm.Bean
{
    public class TestBean
    {



        [SerializedFile(name: "标题", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
        public string Title { get; set; }
        [SerializedFile(name: "内容", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
        public string Content { get; set; }

        [SerializedFile(name: "图片", contentType: FileContentType.Path, fileType: DefaultFileType.IMAGE)]
        public string ImagePath { get; set; }

        [SerializedFile(name: "图片", contentType: FileContentType.FileName, fileType: DefaultFileType.IMAGE)]
        public string ImageName { get; set; }

        [SerializedFile(name: "图片", contentType: FileContentType.FolderName, fileType: DefaultFileType.IMAGE)]
        public string FolderName { get; set; }

        [SerializedFile(name: "内容2", contentType: FileContentType.Content, fileType: DefaultFileType.INI)]
        public IniBean IniContent { get; set; }
        [SerializedFile(name: "图片", contentType: FileContentType.Content, fileType: DefaultFileType.Folder)]
        public ImaegBean ImageInfos { get; set; }

        [SerializedFile(name: "内容目录", contentType: FileContentType.Content, fileType: DefaultFileType.Folder)]
        public TestBean[] ContentBeans { get; set; }


        public class IniBean
        {
            [SerializedFile(name: "", contentType: FileContentType.FolderName, fileType: DefaultFileType.TXT)]
            public string FolderName { get; set; }

            [SerializedFile(name: "内容1", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public string Content1 { get; set; }
            [SerializedFile(name: "内容2", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public int Content2 { get; set; }
            [SerializedFile(name: "内容3", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public bool Content3 { get; set; }
        }


        public class ImaegBean
        {

            [SerializedFile(name: "详情", contentType: FileContentType.Path, fileType: DefaultFileType.Folder)]
            public ImageDetailBean DetailInfo  { get; set; }

            [SerializedFile(name: "", contentType: FileContentType.Path, fileType: DefaultFileType.IMAGE)]
            public List<string> ImagePaths { get; set; }
        }


        public class ImageDetailBean
        {
            [SerializedFile(name: "内容1", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public string Content1 { get; set; }
            [SerializedFile(name: "内容2", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public string Content2 { get; set; }
        }


    }
}
