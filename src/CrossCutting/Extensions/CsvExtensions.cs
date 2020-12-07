using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace CrossCutting.Extensions
{
	public static class CsvExtensions
	{
		public static string ToCSV<T>(this List<T> list, bool propNamAsHeader = true, params string[] names)
		{

			return list.ToCSV(";", "\"", propNamAsHeader, names);
		}

		public static string ToCSV<T>(this List<T> list, params string[] names)
		{
			return list.ToCSV(";", "\"", true, names);
		}

		public static string ToCSV<T>(this List<T> list, string[] names, string[] custom)
		{
			return list.ToCSV(";", "\"", true, names, custom);
		}

		public static string ToCSV<T>(this List<T> list, string separator = ";", string enclosedBy = "\"", bool propNamAsHeader = true, string[] names = null, string[] custom = null)
		{
			list ??= new List<T>();
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
			var rows = new List<string>();
			foreach (T item in list)
			{
				var cells = new List<string>();
				foreach (PropertyDescriptor prop in properties)
				{
					object value = "";
					var attrs = prop.Attributes.Cast<Attribute>().ToList();
					var isClass = (prop.PropertyType.Namespace.Contains("Domain") && !prop.PropertyType.IsEnum) || prop.PropertyType.Namespace.Contains("List") || prop.PropertyType.Namespace.Contains("Collections");
					if (!isClass && attrs != null && !attrs.Select(x => x.ToString()).Any(z => z.Contains("NotMapped")))
					{
						if (names != null && names.Length > 0)
						{
							if (names.Contains(prop.Name))
							{
								value = prop.GetValue(item);
								cells.Add(value != null ? $"{enclosedBy}{value}{enclosedBy}" : string.Empty);
							}
						}
						else
						{
							cells.Add(value != null ? $"{enclosedBy}{value}{enclosedBy}" : string.Empty);
						}
					}
				}
				rows.Add(string.Join(separator, cells.Select(x => $"{x.ToArray()}\r\n")));
			}


			if (propNamAsHeader)
			{
				var header = "";
				List<string> props = new List<string>();

				foreach (PropertyDescriptor prop in properties)
				{
					var attrs = prop.Attributes.Cast<Attribute>().ToList();
					bool isClass = (prop.PropertyType.Namespace.Contains("Domain") && !prop.PropertyType.IsEnum)
						|| prop.PropertyType.Namespace.Contains("List")
						|| prop.PropertyType.Namespace.Contains("Collections");
					if (!isClass && attrs != null && !attrs.Select(x => x.ToString()).Any(z => z.Contains("NotMapped")))
					{
						if ((bool)names?.Contains(prop.Name))
						{
							var name = custom != null ? custom[Array.IndexOf(names, prop.Name)] : prop.Name;
							props.Add($"{enclosedBy}{prop.Name}{enclosedBy}");
						}
						else
						{
							props.Add($"{enclosedBy}{prop.Name}{enclosedBy}");
						}

					}


					header = string.Join(separator, props.Select(x => x.ToString(CultureInfo.InvariantCulture)).ToArray()) + "\r\n";
				}
				rows.Insert(0, header);
				return string.Join("", rows);
			}

			return string.Join("", rows);
		}

	}
}
