
using NetFileFormat.FAnnotation;
using NetFileFormat.FileFormat;
using NewFileFormatForm.Bean;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace NewFileFormatForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            /* PropertyInfo[] propertyInfos = typeof(TestBean).GetProperties();
             foreach (var item in propertyInfos)
             {
                 object[] attributes = item.GetCustomAttributes(typeof(SerializedFile), true);
                 if (attributes == null || attributes.Length == 0) continue;
                 foreach (var a in attributes)
                 {
                     Console.WriteLine(":a");
                 }
             }*/


            FileFormatConvert.InitFormat();
            List<TestBean> contentBean = FileFormatConvert.DeserializeObject<List<TestBean>>(Directory.GetCurrentDirectory() + "/内容");
            Console.WriteLine("aaaa");


           /* List<TestBean2> contentBean2 = new List<TestBean2>();
            foreach (var item in contentBean)
            {
                contentBean2.Add(new TestBean2(item));
            }*/

            FileFormatConvert.SerializeObject(Directory.GetCurrentDirectory() + "/内容2", contentBean);
            Console.WriteLine("保存文件成功");

        }

    }
}
