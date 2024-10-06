using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFileFormat.FType
{
 public   enum FileContentType
    {
        /// <summary>
        /// 路径
        /// </summary>
        Path,
        /// <summary>
        /// 内容
        /// </summary>
        Content,
        /// <summary>
        /// 文件名(不包含扩展名)
        /// </summary>
        Name,
        /// <summary>
        /// 文件名(包含扩展名)
        /// </summary>
       FileName,
        /// <summary>
        /// 所在目录名称
        /// </summary>
        FolderName
    }
}
