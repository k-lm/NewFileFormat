using NetFileFormat.FAnnotation;
using NetFileFormat.FormatModel.Base;
using NetFileFormat.FType;
using NetFileFormat.Utils;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetFileFormat.FileFormat
{
    class ObjectSaveFormat
    {
        private FormatBuild mFormatBuild;

        public ObjectSaveFormat(FormatBuild formatBuild)
        {
            mFormatBuild = formatBuild;
        }

        /// <summary>
        /// 返回Array数值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public  void SaveArrayValue(object obj, string parentPath, ObjectInfo targetInfo)
        {
            Array array = obj as Array;
            if (array == null || array.Length == 0) return;
            SaveArrayOrListValue(obj, parentPath, targetInfo);
        }

        /// <summary>
        /// 返回List数值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public  void SaveListValue(object obj, string parentPath, ObjectInfo targetInfo)
        {
            System.Collections.IList list = obj as System.Collections.IList;
            if (list == null || list.Count == 0) return;
            SaveArrayOrListValue(obj, parentPath, targetInfo);
        }

        /// <summary>
        /// 返回object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public  void SaveObjetValue(object obj, string parentPath, ObjectInfo targetInfo)
        {
            Type type = obj.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            if (propertyInfos == null || propertyInfos.Length == 0) return ;

            foreach (var property in propertyInfos)
            {
                object attributes = property.GetCustomAttributes(typeof(SerializedFile), false).FirstOrDefault();
                if (attributes == null) continue;
                object value = property.GetValue(obj,null);
                if (value == null) continue;

                SerializedFile serializedFile = attributes as SerializedFile;
                IFileFormatItem fileFormat = mFormatBuild.GetFileFormatItem(serializedFile);
                if (fileFormat == null) continue;
                ObjectInfo  childObjectInfo = ObjectInfo.GetObjetInfo(property, serializedFile, parentPath);
                if (!fileFormat.IsObjectFile(childObjectInfo.TarType))
                {
                    string path = parentPath + Path.AltDirectorySeparatorChar + serializedFile.Name;
                    // 转换为目录保存
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                    SaveTargetValue(property.GetValue(obj,null), path, childObjectInfo);
                    continue;
                }
               
                if (fileFormat == null) continue;

                childObjectInfo.AbsPath  = fileFormat.GetSaveFileAbsPath(serializedFile,parentPath, serializedFile.Name,value);
                if (string.IsNullOrEmpty(childObjectInfo.AbsPath)) continue;
                // 保存
                fileFormat.SaveObjectValue(childObjectInfo.AbsPath, type, ObjectInfo.GetObjectTypeCode(type), value);
            }

        }


        /// <summary>
        /// 保存List或Array数据
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentPath"></param>
        /// <param name="targetInfo"></param>
       private void SaveArrayOrListValue(object obj, string parentPath, ObjectInfo targetInfo)
        {
            Type childType = ObjectTypeUtils.GetListOrArrayType(obj.GetType());
            ObjectType objectType = ObjectType.Object;
            bool isLoadObjeType = false;
            // 创建文件夹
            if (!Directory.Exists(parentPath))
            {
                Directory.CreateDirectory(parentPath);
            }

            System.Collections.IEnumerable objes = obj as System.Collections.IEnumerable;
            TypeCode typeCode = TypeCode.Empty;
            IFileFormatItem fileFormatIteml = null;
            int index = 1;
            foreach (var item in objes)
            {

                if (item == null) continue;
                // 多级List或Array
                if (IsArrOrList(childType))
                {
                    continue;
                }

                if (!isLoadObjeType)
                {
                    objectType = ObjectInfo.GetObjectType(childType);
                    isLoadObjeType = true;
                }
                string folder;
                
                switch (objectType)
                {
                    case ObjectType.Object:
                        folder = parentPath + Path.AltDirectorySeparatorChar + GetNumStr(index, mFormatBuild.FolderNumLength);
                        Directory.CreateDirectory(folder);
                        index++;
                        SaveObjetValue(item, folder + Path.AltDirectorySeparatorChar, null);
                        break;
                    case ObjectType.BasicValue:
                        folder = parentPath ;
                        if (fileFormatIteml == null)
                        {
                            fileFormatIteml = mFormatBuild.GetFileFormatItem(targetInfo.SerializedFile);
                            typeCode = ObjectInfo.GetObjectTypeCode(childType);
                        }
                        // 没有转换器或者文件搜索
                        if (fileFormatIteml == null ) return;
                        string path = fileFormatIteml.GetSaveFileAbsPath(targetInfo.SerializedFile, folder, targetInfo.SerializedFile.Name, item);
                        if (string.IsNullOrEmpty(path)) return;
                        fileFormatIteml.SaveObjectValue(path, childType, typeCode, item);

                        break;
                    default:
                        break;
                }
            }
        }



        private void SaveTargetValue(object obj, string parentPath, ObjectInfo targetInfo)
        {
            switch (targetInfo.ObjType)
            {
                case ObjectType.Object:
                    SaveObjetValue(obj, parentPath, targetInfo);
                    break;
                case ObjectType.Array:
                    SaveArrayValue(obj, parentPath, targetInfo);
                    break;
                case ObjectType.List:
                    SaveListValue(obj, parentPath, targetInfo);
                    break;
            }
        }


        private bool IsArrOrList(Type type)
        {
            return type.IsArray || typeof(System.Collections.IList).IsAssignableFrom(type);
        }

        private StringBuilder mNumBuilder;

        /// <summary>
        /// 根据长度返回对应数字字符串
        /// </summary>
        /// <param name="num"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private string GetNumStr(int num,int length)
        {
            if(mNumBuilder == null)
            {
                mNumBuilder = new StringBuilder();
            }
            mNumBuilder.Append(num);
            // 添加前置0
            if (mNumBuilder.Length < length)
            {
                while (mNumBuilder.Length < length)
                {
                    mNumBuilder.Insert(0, 0);
                }
            }

            string numStr = mNumBuilder.ToString();
            mNumBuilder.Clear();
            return numStr;
        }

    }
}
