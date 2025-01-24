using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QJ.Communication.Core.Extension
{
    public static class EnumExtensions
    {
        /// <summary>
        /// 針對Eunm的擴展，該方法可以獲取Enum中的註解文字串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this System.Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            var attribute = field.GetCustomAttribute<DescriptionAttribute>();
            return attribute?.Description ?? value.ToString();
        }
    }
}
