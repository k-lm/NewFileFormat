
using NetFileFormat.Extend;
using NetFileFormat.FAnnotation;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NewFileFormatForm.Bean
{
  public  class TestBean2
    {

        public TestBean2()
        {

        }


        public TestBean2(TestBean testBean)
        {
            Title = testBean.Title;
            Content = testBean.Content;
            ImagePath = testBean.ImagePath;
            if(testBean.IniContent != null)
            {
                IniContent = new IniBean()
                {
                    Content1= testBean.IniContent.Content1,
                    Content2 = testBean.IniContent.Content2,
                    Content3= testBean.IniContent.Content3,
                };
            }

            ImagePaths = testBean.ImageInfos == null ? null : testBean.ImageInfos.ImagePaths;

            if(testBean.ContentBeans != null && testBean.ContentBeans.Length > 0)
            {
                ContentBeans = new TestBean2[testBean.ContentBeans.Length];
                for (int i = 0; i < testBean.ContentBeans.Length; i++)
                {
                    ContentBeans[i] = new TestBean2(testBean.ContentBeans[i]);
                }
            }
        }

       [SerializedFile(name:"A标题", contentType : FileContentType.Content,fileType : DefaultFileType.TXT)]
        public string Title { get; set; }
        [SerializedFile(name: "A内容", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
        public string Content { get; set; }

        [SerializedFile(name: "A图片", contentType: FileContentType.Path, fileType: DefaultFileType.IMAGE)]
        public string ImagePath { get; set; }

        [SerializedFile(name: "A图片", contentType: FileContentType.FileName, fileType: DefaultFileType.IMAGE)]
        public string ImageName { get; set; }

        [SerializedFile(name: "A图片", contentType: FileContentType.FolderName, fileType: DefaultFileType.IMAGE)]
        public string FolderName { get; set; }


        [SerializedFile(name: "A内容2", contentType: FileContentType.Content, fileType: DefaultFileType.INI)]
        public IniBean IniContent { get; set; }
        [SerializedFile(name: "A图片", contentType: FileContentType.Path, fileType: DefaultFileType.IMAGE)]
        public List<string> ImagePaths { get; set; }

        [SerializedFile(name: "A内容目录", contentType: FileContentType.Content, fileType: DefaultFileType.Folder)]
        public TestBean2[] ContentBeans { get; set; }


        public class IniBean
        {
            [SerializedFile(name: "", contentType: FileContentType.FolderName, fileType: DefaultFileType.TXT)]
            public string FolderName { get; set; }

            [SerializedFile(name: "A内容1", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public string Content1 { get; set; }
            [SerializedFile(name: "A内容2", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public int Content2 { get; set; }
            [SerializedFile(name: "A内容3", contentType: FileContentType.Content, fileType: DefaultFileType.TXT)]
            public bool Content3 { get; set; }
        }

    }
}
