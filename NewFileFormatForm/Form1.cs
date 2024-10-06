
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
           


            FileFormatConvert.InitFormat();


            TestBean3 testBean3= FileFormatConvert.DeserializeObject<TestBean3>(Directory.GetCurrentDirectory() + "/内容");





        }

    }
}
