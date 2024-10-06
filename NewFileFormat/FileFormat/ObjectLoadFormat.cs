using NetFileFormat.FAnnotation;
using NetFileFormat.FormatModel.Base;
using NetFileFormat.FType;
using NetFileFormat.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetFileFormat.FileFormat
{
    class ObjectLoadFormat
    {

        private FormatBuild mFormatBuild;


        public ObjectLoadFormat(FormatBuild formatBuild)
        {
            mFormatBuild = formatBuild;
        }

        /// <summary>
        /// 返回Array数值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public object GetArrayValue(Type type, string parentPath, ObjectInfo targetInfo)
        {
            Type childType = ObjectTypeUtils.GetListOrArrayType(type);
            object listObject = GetListObjectValue(childType, parentPath, targetInfo);
            IList list = listObject as IList;
            if (list == null) return default;
          
            Array array = Array.CreateInstance(childType, list.Count);
            for (int i = 0; i < array.Length; i++)
            {
                array.SetValue(list[i], i);
            }
            return array;
        }

        /// <summary>
        /// 返回List数值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public object GetListValue(Type type, string parentPath, ObjectInfo targetInfo)
        {
            if (!IsArrOrList(type))
            {
                return default;
            }

            Type childType = ObjectTypeUtils.GetListOrArrayType(type);
            return GetListObjectValue(childType, parentPath,targetInfo);
        }


        /// <summary>
        /// 返回object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        public Object GetObjetValue(Type type, string parentPath, ObjectInfo targetInfo)
        {

            // 获取类型中所有属性
            PropertyInfo[] propertyInfos = type.GetProperties();
           
            if (propertyInfos == null || propertyInfos.Length == 0 || !Directory.Exists(parentPath)) return default;

            string[] filePaths = Directory.GetFiles(parentPath);
            string[] folderPaths = Directory.GetDirectories(parentPath);
            object obj = default;
            Type childType = ObjectTypeUtils.GetListOrArrayType(type);
            foreach (var property in propertyInfos)
            {
                object attributes = property.GetCustomAttributes(typeof(SerializedFile), false).FirstOrDefault();
                if (attributes == null) continue;
                SerializedFile serializedFile = attributes as SerializedFile;

                ObjectInfo objectInfo = GetObjetInfo(property, serializedFile, parentPath);
                IFileFormatItem fileFormat = null;
                if (targetInfo != null)
                {
                    // 获取对对象结构中上一级对象的格式转换器
                    fileFormat = mFormatBuild.GetFileFormatItem(targetInfo.SerializedFile);
                    bool isFile = fileFormat.IsObjectFile(objectInfo.TarType);
                    // 如果上一级对象内容出自文件则用上级的格式转换器 否则使用自身的格式转换器
                    if (!isFile || objectInfo.ObjType != ObjectType.Object)
                    {
                        fileFormat = mFormatBuild.GetFileFormatItem(objectInfo.SerializedFile);
                    }
                }
                if (fileFormat == null)
                {
                    // 使用自身的格式转换器
                    fileFormat = mFormatBuild.GetFileFormatItem(objectInfo.SerializedFile);
                }
                if (fileFormat == null) continue;

                if (fileFormat == null)
                {
                    objectInfo.AbsPath = parentPath + Path.DirectorySeparatorChar + objectInfo.Name;
                }
                else
                {
                    // 目录结构对象使用GetListValue或者GetArrayValue方法获取
                    if (!fileFormat.IsObjectFile(property.PropertyType))
                    {
                        objectInfo.AbsPath = parentPath + Path.DirectorySeparatorChar + objectInfo.Name;
                        if (obj == default) obj = Activator.CreateInstance(childType);
                        SetTargetObjectValue(obj, objectInfo, fileFormat);
                        continue;
                    }


                    objectInfo.AbsPath = fileFormat.GetFilePath(
                        serializedFile.ObjFileType == ObjectFileType.File ? filePaths:folderPaths, 
                        objectInfo.Name,
                        objectInfo.SerializedFile.ExtendObj);
                }

                if (string.IsNullOrEmpty(objectInfo.AbsPath)) continue;

                if (obj == null) obj = Activator.CreateInstance(childType);
                SetTargetObjectValue(obj, objectInfo, fileFormat);
            }

            return obj;
        }

        /// <summary>
        /// 返回列表数据
        /// </summary>
        /// <param name="childType">list中范型类型</param>
        /// <param name="parentPath">目录</param>
        /// <param name="targetInfo"></param>
        /// <returns></returns>
        private object GetListObjectValue(Type childType, string parentPath, ObjectInfo targetInfo)
        {
            TypeCode childTypeCode = ObjectInfo.GetObjectTypeCode(childType);
            IFileFormatItem fileFormatItem;

            object listObj = default;
            string[] childPaths;
            MethodInfo addMethodInfo = typeof(IList).GetMethod("Add");
            object[] values = new object[1];

            // 多级list或Array
            if (IsArrOrList(childType))
            {
                if (!Directory.Exists(parentPath)) return default;
                childPaths = Directory.GetDirectories(parentPath);
                fileFormatItem = mFormatBuild.GetFileFormatItem(targetInfo.SerializedFile);
                foreach (var chilPath in childPaths)
                {
                    object childObj;
                    if (fileFormatItem.IsObjectFile(childType))
                    {
                        childObj = GetObjetValue(childType, chilPath, targetInfo);
                    }
                    else if (childType.IsArray)
                    {
                        childObj = GetArrayValue(childType, chilPath, targetInfo);
                    }
                    else
                    {
                        childObj = GetListValue(childType, chilPath, targetInfo);
                    }
                    if (childObj == default) continue;
                    // 实例化list
                    if (listObj == default) listObj = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { childType }));
                    values[0] = childObj;
                    addMethodInfo.Invoke(listObj, values);
                }
                return listObj;
            }

            switch (childTypeCode)
            {
                case TypeCode.Object:
                    if (!Directory.Exists(parentPath)) break;
                    // 查询文件

                    childPaths =targetInfo.SerializedFile.ChildFileType == ObjectFileType.File ? Directory.GetFiles(parentPath) : Directory.GetDirectories(parentPath);

                    foreach (var chilPath in childPaths)
                    {
                        object childObj = GetObjetValue(childType, chilPath, targetInfo);
                        if (childObj == default) continue;
                        // 实例化list
                        if (listObj == default) listObj = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { childType }));
                        values[0] = childObj;
                        addMethodInfo.Invoke(listObj, values);
                    }
                    break;
                default:
                    // 查询文件
                    if (!Directory.Exists(parentPath)) break;
                    childPaths = Directory.GetFiles(parentPath);
                    fileFormatItem = mFormatBuild.GetFileFormatItem(targetInfo.SerializedFile);

                    foreach (var chilPath in childPaths)
                    {
                        string filePath = fileFormatItem.GetFilePath(targetInfo.SerializedFile, chilPath, "", targetInfo.SerializedFile.ExtendObj);
                        if (string.IsNullOrEmpty(filePath)) continue;
                        // 实例化list
                        if (listObj == default) listObj = Activator.CreateInstance(typeof(List<>).MakeGenericType(new Type[] { childType }));
                        values[0] = fileFormatItem.GetObjectValue(chilPath, childType, childTypeCode, targetInfo.SerializedFile.ContentType);
                        addMethodInfo.Invoke(listObj, values);
                    }

                    break;
            }
            return listObj;
        }

        /// <summary>
        /// 设置对象内容
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <param name="fileFormat"></param>
        private void SetTargetObjectValue(object obj, ObjectInfo objectInfo, IFileFormatItem fileFormat)
        {

            if (fileFormat.IsObjectFile(objectInfo.TarType))
            {
                SetBasicValue(obj, objectInfo, fileFormat);
                return;
            }

            switch (objectInfo.ObjType)
            {
                case ObjectType.Object:
                    SetObjectValue(obj, objectInfo, fileFormat);
                    break;
                case ObjectType.Array:
                    SetArrayValue(obj, objectInfo, fileFormat);
                    break;
                case ObjectType.List:
                    SetListValue(obj, objectInfo, fileFormat);
                    break;
                case ObjectType.BasicValue:
                    SetBasicValue(obj, objectInfo, fileFormat);
                    break;
            }
        }
        /// <summary>
        /// 设置目录结构自定义对象形式
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <param name="fileFormat"></param>
        private void SetObjectValue(object obj, ObjectInfo objectInfo, IFileFormatItem fileFormat)
        {
            objectInfo.Property.SetValue(obj, GetObjetValue(objectInfo.TarType, objectInfo.AbsPath, objectInfo), null);
        }

        /// <summary>
        /// 设置基础类型或单文件对应变量的 对象实例数据
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <param name="fileFormat"></param>
        private void SetBasicValue(object obj, ObjectInfo objectInfo, IFileFormatItem fileFormat)
        {
            objectInfo.Property.SetValue(obj, fileFormat.GetObjectValue(objectInfo.AbsPath, objectInfo.TarType, objectInfo.TCode, objectInfo.SerializedFile.ContentType), null);
        }
        /// <summary>
        /// 设置列表对应数值
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <param name="fileFormatItem"></param>
        private void SetListValue(object obj, ObjectInfo objectInfo, IFileFormatItem fileFormatItem)
        {
            objectInfo.Property.SetValue(obj, GetListValue(objectInfo.TarType, objectInfo.AbsPath, objectInfo), null);

        }

        /// <summary>
        /// 设置数组对应数值
        /// </summary>
        /// <param name="objectInfo"></param>
        /// <param name="fileFormatItem"></param>
        private void SetArrayValue(object obj, ObjectInfo objectInfo, IFileFormatItem fileFormatItem)
        {
            objectInfo.Property.SetValue(obj, GetArrayValue(objectInfo.TarType, objectInfo.AbsPath, objectInfo), null);

        }

        private bool IsArrOrList(Type type)
        {
            return type.IsArray || typeof(System.Collections.IList).IsAssignableFrom(type);
        }

        /// <summary>
        /// 返回文件对象信息
        /// </summary>
        /// <param name="property"></param>
        /// <param name="serializedFile"></param>
        /// <param name="parentPath"></param>
        /// <returns></returns>
        private ObjectInfo GetObjetInfo(PropertyInfo property, SerializedFile serializedFile, string parentPath)
        {
            return ObjectInfo.GetObjetInfo(property, serializedFile, parentPath);
        }


    }
}
