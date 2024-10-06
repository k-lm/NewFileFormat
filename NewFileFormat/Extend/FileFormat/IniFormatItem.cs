using NetFileFormat.Extend.FileFormat.Base;
using NetFileFormat.FAnnotation;
using NetFileFormat.FormatModel.Base;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetFileFormat.Extend.FileFormat
{
    public class IniFormatItem : BaseFileFormatItem
    {
        private StringBuilder mStringBuilder;

        public override object GetTargetObjectContent(string absPath, Type type, TypeCode typeCode)
        {
            object obj = Activator.CreateInstance(type);
            string[] contents = File.ReadAllLines(absPath);
            List<PropertyInfo> propertyInfos = new List<PropertyInfo>(type.GetProperties());
            string lastName = null;
            PropertyInfo lastPropertyInfo = null;

            Dictionary<string, SerializedFile> serializedFileDic = new Dictionary<string, SerializedFile>();

            foreach (var item in propertyInfos)
            {
                SerializedFile serializedFile = GetSerializedFile(item);
                if (serializedFile == null) continue;
                serializedFileDic[item.Name] = serializedFile;
            }

            for (int i = 0; i < contents.Length; i++)
            {
                string value = contents[i].Trim();
                if (string.IsNullOrEmpty(value)) continue;
                string[] infos =  value.Split('=');
                if (infos.Length < 2 && lastName == null) continue;

                if(infos.Length< 2)
                {
                    AddValue(infos[0]);
                    continue;
                }

                if(lastName != null && mStringBuilder != null )
                {
                    SetValue(obj, lastPropertyInfo, lastName, mStringBuilder.ToString());
                    mStringBuilder.Clear();
                }

                string name = infos[0].Trim();
                // 根据字段名称查询对象属性
                PropertyInfo propertyInfo = propertyInfos.Select(info => info)
                    .Where(info => {
                        if (!serializedFileDic.TryGetValue(info.Name, out SerializedFile serialized)) return false;
                        return serialized.Name == name;
                    }).FirstOrDefault();
                if (propertyInfo == null) continue;
                propertyInfos.Remove(propertyInfo);
                // 添加内容
                lastName = name;
                lastPropertyInfo = propertyInfo;
                for (int j = 1; j < infos.Length; j++)
                {
                    AddValue(infos[j]);
                }
            }

            if (lastName != null && mStringBuilder != null)
            {
                SetValue(obj, lastPropertyInfo, lastName, mStringBuilder.ToString());
                mStringBuilder.Clear();
            }
            return obj;
        }

        private void AddValue(string value)
        {
            if (mStringBuilder == null) mStringBuilder = new StringBuilder();
            mStringBuilder.Append(value);
        }

        private void SetValue(object obj, PropertyInfo propertyInfo,  string name,string value)
        {
            object objValue;
            Type valueType = propertyInfo.PropertyType;
            if (typeof(string).IsAssignableFrom(valueType))
            {
                objValue = value;
            }
            else if (typeof(bool).IsAssignableFrom(valueType))
            {
                objValue = bool.Parse(value);
            }
            else if (typeof(float).IsAssignableFrom(valueType))
            {
                objValue = float.Parse(value);
            }
            else if (typeof(double).IsAssignableFrom(valueType))
            {
                objValue = double.Parse(value);
            }
            else if (typeof(int).IsAssignableFrom(valueType))
            {
                objValue = int.Parse(value);
            }
            else if (typeof(long).IsAssignableFrom(valueType))
            {
                objValue = long.Parse(value);
            }
            else
            {
                return;
            }

            propertyInfo.SetValue(obj, objValue, null);
        }


        public override bool IsObjectFile(Type type)
        {
            return true;
        }

        public override void SaveObjectValue(string absPath, Type type, TypeCode typeCode, object value)
        {
           
        }

        public override string GetFilePath(string[] files ,string fileName, object extendObj)
        {
            return GetSelectFilePath(files, fileName, "ini","txt");
        }

        public override string GetFilePath(string filePath, string fileName, object extendObj)
        {
            return GetSelectFilePath(filePath, fileName, "ini", "txt");
        }

        public override string GetSaveFileAbsPath(string folder, string name, object value)
        {
            return GetSaveFilePath(folder, name, "ini");
        }
    }
}
