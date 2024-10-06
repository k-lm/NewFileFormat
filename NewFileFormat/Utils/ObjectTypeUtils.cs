using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetFileFormat.Utils
{
   public class ObjectTypeUtils
    {
        /// <summary>
        /// 返回List或数组中范型的类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type GetListOrArrayType(Type type)
        {
            if (type.IsArray) return type.GetElementType();

            if (!type.IsGenericType) return type;
            type = type.GetGenericArguments()[0];
            return type;
        }
    }
}
