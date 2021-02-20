using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace MemberManagementSystem.Platform.Utilities.Extensions
{
    public static class EnumExtension
    {
        /// <summary>
        /// 為 Enum 添加 Extension，
        /// 取得 Enum 的 Description描述字串
        /// </summary>
        /// <param name="source">source</param>
        /// <returns></returns>
        public static string GetDescriptionText(this Enum source)
        {
            FieldInfo fi = source.GetType().GetField(source.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
             typeof(DescriptionAttribute), false);
            if (attributes.Length > 0) return attributes[0].Description;
            else return source.ToString();
        }
    }
}
