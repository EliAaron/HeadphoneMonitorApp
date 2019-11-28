using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System;
using System.Reflection;

namespace HeadphoneMonitorApp
{
    public class TextAttribute : Attribute
    {
        public string Text;

        public TextAttribute(string text)
        {
            Text = text;
        }
    }

    public class ShortTextAttribute : Attribute
    {
        public string Text;

        public ShortTextAttribute(string text)
        {
            Text = text;
        }
    }

    public static class EnumExt
    {
        public static string ToText(this Enum enumeration)
        {
            MemberInfo[] memberInfoArr = enumeration.GetType().GetMember(enumeration.ToString());
            if (memberInfoArr.Length <= 0) return enumeration.ToString();

            object[] attributes = memberInfoArr[0].GetCustomAttributes(typeof(TextAttribute), false);
            return attributes.Length > 0 ? ((TextAttribute)attributes[0]).Text : enumeration.ToString();
        }

        public static string ToShortText(this Enum enumeration)
        {
            MemberInfo[] memberInfoArr = enumeration.GetType().GetMember(enumeration.ToString());
            if (memberInfoArr.Length <= 0) return enumeration.ToString();

            object[] attributes = memberInfoArr[0].GetCustomAttributes(typeof(ShortTextAttribute), false);
            return attributes.Length > 0 ? ((ShortTextAttribute)attributes[0]).Text : enumeration.ToString();
        }

        public static string ToDescription(this Enum enumeration)
        {
            return (string)TypeDescriptor.GetConverter(enumeration.GetType()).ConvertTo(enumeration, typeof(string));
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static List<T> GetEnumValueList<T>()
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(T)).Cast<T>().ToList();
        }

        public static T[] GetEnumValueArray<T>()
        {
            if (typeof(T).BaseType != typeof(Enum))
            {
                throw new InvalidCastException();
            }

            return Enum.GetValues(typeof(T)).Cast<T>().ToArray();
        }
    }
}