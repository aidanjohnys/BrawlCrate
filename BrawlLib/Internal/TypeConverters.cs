﻿using BrawlLib.SSBB;
using System;
using System.ComponentModel;
using System.Globalization;

namespace BrawlLib.Internal
{

    internal class HexTypeConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertFrom(context, sourceType);
            }
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                return true;
            }
            else
            {
                return base.CanConvertTo(context, destinationType);
            }
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value.GetType() == typeof(UInt32))
            {
                return string.Format("0x{0:X8}", value);
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(string))
            {
                string input = (string)value;

                if (input.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    input = input.Substring(2);
                }

                return UInt32.Parse(input, System.Globalization.NumberStyles.HexNumber, culture);
            }
            else
            {
                return base.ConvertFrom(context, culture, value);
            }
        }
    }

    internal class UserDataConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destType)
        {
            if (destType == typeof(string) && value is UserDataClass)
            {
                return ((UserDataClass) value).ToString();
            }

            return base.ConvertTo(context, culture, value, destType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                try
                {
                    string s = (string) value;
                    string[] s2 = s.Split(':');
                    string[] s3 = s2[1].Split(',');

                    UserDataClass d = new UserDataClass
                    {
                        Name = s2[0]
                    };
                    foreach (string i in s3)
                    {
                        d._entries.Add(i);
                    }

                    return d;
                }
                catch
                {
                    // ignored
                }
            }

            return base.ConvertFrom(context, culture, value);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destType)
        {
            return destType == typeof(UserDataClass) ? true : base.CanConvertTo(context, destType);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) ? true : base.CanConvertFrom(context, sourceType);
        }
    }

    internal class ExpandableObjectCustomConverter : ExpandableObjectConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value,
                                         Type destType)
        {
            string s = (string) base.ConvertTo(context, culture, value, destType);
            return s.Substring(s.LastIndexOf('.') + 1);
        }
    }
}