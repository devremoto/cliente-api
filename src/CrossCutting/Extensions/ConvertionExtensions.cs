using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace CrossCutting.Extensions
{
    public static class ConvertionExtensions
    {
        #region CONVERTIONS

        public static string ConvertEnconding(this string value, Encoding from, Encoding to)
        {
            var b = Encoding.Convert(from, to, to?.GetBytes(value));
            return to.GetString(b);
        }

        public static Stream ToStream(this string contents, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            MemoryStream stream = new MemoryStream(contents.ToBytes(encoding));
            return stream;
        }

        public static Stream ToStream(this byte[] contents)
        {
            MemoryStream stream = new MemoryStream(contents);
            return stream;
        }
        public static string Content(this byte[] bytes)
        {
            return Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }
        public static byte[] ToBytes(this string contents, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            return encoding.GetBytes(contents);
        }

        public static string ToBase64(this string contents, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.Default;
            var bytes = encoding.GetBytes(contents);
            return Convert.ToBase64String(bytes);
        }
        public static DataTable ToDataTable<T>(this List<T> list)
        {
            list ??= new List<T>();
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable
            {
                TableName = typeof(T).Name
            };

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in list)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                {
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                }
                table.Rows.Add(row);
            }
            return table;
        }

        public static DataTable ToDataTable<T>(this T source)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable
            {
                TableName = typeof(T).Name
            };

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            DataRow row = table.NewRow();
            foreach (PropertyDescriptor prop in properties)
            {
                row[prop.Name] = prop.GetValue(source) ?? DBNull.Value;
            }

            table.Rows.Add(row);
            return table;
        }

        public static TResult To<TResult>(this object source)
        {
            if (source != null && source != DBNull.Value && source != System.DBNull.Value && !string.IsNullOrWhiteSpace(source.ToString()))
            {
                var type = typeof(TResult);
                if (type.Name.Contains("Nullable"))
                {
                    try
                    {
                        var typeName = type.FullName.Replace("System.Nullable`1[[", "").Split(',')[0];
                        var result = (TResult)Convert.ChangeType(source, Type.GetType(typeName), CultureInfo.InvariantCulture);
                        return result;

                    }
                    catch
                    {
                        return default;
                    }
                }

                return (TResult)Convert.ChangeType(source, typeof(TResult), CultureInfo.InvariantCulture);
            }

            return default;
        }

        public static TResult To<TResult>(this Guid source)
        {
            if (source != null && !string.IsNullOrWhiteSpace(source.ToString()))
            {
                return (TResult)TypeDescriptor.GetConverter(typeof(TResult)).ConvertFromInvariantString(source.ToString());
            }
            return default;
        }


        #endregion

        public static string ToKeyPair<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, char separator, char op)
        {

            var list = new List<string>();
            var keys = dictionary.Keys;
            foreach (var key in keys)
            {
                list.Add($"{key}{op}{dictionary[key]}");
            }
            return string.Join(separator, list);
        }

        public static string ToKeyPair<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> dictionary, char separator, char op)
        {
            if (dictionary == null)
            {
                return null;
            }
            var list = new List<string>();
            var keys = dictionary.Keys;
            foreach (var key in keys)
            {
                list.Add($"{key}{op}{dictionary[key]}");
            }
            return string.Join(separator, list);
        }

        public static string Join(this IList<string> list, char separator)
        {
            if (list == null || list.Count < 0)
            {
                return string.Empty;
            }

            return string.Join(separator, list);
        }
        public static string Join(this List<string> list, char separator)
        {
            if (list == null || list.Count < 0)
            {
                return string.Empty;
            }

            return string.Join(separator, list);
        }
        public static DateTime? ToDateTime(this long? source)
        {
            if (source != null)
            {
                return ToDateTime((long)source);
            }
            return DateTime.MinValue;
        }
        public static DateTime? ToDateTime(this long source)
        {
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            if (new DateTime(source) > start)
            {
                return new DateTime(source);
            }
            DateTime date = start.AddMilliseconds(source);
            return date;
        }
        public static long ToTimestamp(this DateTime value)
        {
            DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return (long)value.Subtract(Epoch).TotalMilliseconds;
        }
    }
}
