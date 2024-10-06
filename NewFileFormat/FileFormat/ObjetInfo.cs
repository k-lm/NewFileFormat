using NetFileFormat.FAnnotation;
using NetFileFormat.FType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NetFileFormat.FileFormat
{
     class ObjectInfo
    {

        /// <summary>
        /// 文件地址
        /// </summary>
        public string AbsPath { get; set; }
        /// <summary>
        /// 父目录地址
        /// </summary>
        public string Path { get; private set; }
        /// <summary>
        /// 文件名称(不包含扩展名)
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 对象中的属性名称
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// 对象类型结构
        /// </summary>
        public ObjectType ObjType { get; private set; }
        /// <summary>
        /// 对象类型
        /// </summary>
        public Type TarType { get; private set; }
        /// <summary>
        /// 包含对象范型类型
        /// </summary>
        public Type ChildType { get; private set; }
        /// <summary>
        /// 对象类型
        /// </summary>

        public TypeCode TCode { get; private set; }
        /// <summary>
        /// 对象特效实例
        /// </summary>
        public SerializedFile SerializedFile { get; private set; }
        /// <summary>
        /// 对象属性实例
        /// </summary>
        public PropertyInfo Property { get; private set; }

        private ObjectInfo()
        {

        }

        private ObjectInfo(PropertyInfo propertyInfo)
        {
            TarType = propertyInfo.PropertyType;
            LoadTypeCode();
        }


        public ObjectInfo(ObjectInfo objectInfo, SerializedFile serializedFile, string parentPath)
        {

        }

        public static ObjectInfo GetObjetInfo(PropertyInfo propertyInfo, SerializedFile serializedFile, string parentPath)
        {
            ObjectInfo objectInfo = new ObjectInfo(propertyInfo)
            {
                Path = parentPath,
                Name = serializedFile.Name,
                SerializedFile = serializedFile,
                PropertyName = propertyInfo.Name,
                Property = propertyInfo,
            };

            return objectInfo;
        }
        /// <summary>
        /// 返回对象类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static TypeCode GetObjectTypeCode(Type type)
        {
            ObjectInfo objectInfo = new ObjectInfo();
            objectInfo.TarType = type;
            objectInfo.LoadTypeCode();
            return objectInfo.TCode;
        }

        public static ObjectType GetObjectType(Type type)
        {
            ObjectInfo objectInfo = new ObjectInfo();
            objectInfo.TarType = type;
            objectInfo.LoadTypeCode();
            return objectInfo.ObjType;
        }

        private void LoadTypeCode()
        {

            Type type = TarType;

            if (typeof(string).IsAssignableFrom(type))
            {
                TCode = TypeCode.String;
            }
            else if (typeof(bool).IsAssignableFrom(type))
            {
                TCode = TypeCode.Boolean;
            }
            else if (typeof(float).IsAssignableFrom(type))
            {
                TCode = TypeCode.Single;
            }
            else if (typeof(double).IsAssignableFrom(type))
            {
                TCode = TypeCode.Double;
            }
            else if (typeof(int).IsAssignableFrom(type))
            {
                TCode = TypeCode.Int32;
            }
            else if (typeof(long).IsAssignableFrom(type))
            {
                TCode = TypeCode.Int64;
            }
            else if (typeof(System.Collections.IList).IsAssignableFrom(type) || type.IsArray)
            {
                ObjType = type.IsArray ? ObjectType.Array : ObjectType.List;
                ChildType = GetRealType(type);
                return;
            }
            else
            {
                TCode = TypeCode.Object;
                ObjType = ObjectType.Object;
                return;
            }

            ObjType = ObjectType.BasicValue;
        }


        /// <summary>
        /// 返回真实type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private Type GetRealType(Type type)
        {
            if (!type.IsGenericType) return type;
            type = type.GetGenericArguments()[0];
            return type;
        }

    }
}
