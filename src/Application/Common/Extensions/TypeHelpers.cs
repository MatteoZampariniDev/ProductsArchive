using System.Reflection;
using Newtonsoft.Json;

namespace ProductsArchive.Application.Common.Extensions;

public static class TypeHelpers
{
    public static bool IsValidProperty<TSource>(string propertyPath, bool throwExceptionIfNotFound = true)
    => IsValidProperty(typeof(TSource), propertyPath, throwExceptionIfNotFound);

    public static bool IsValidProperty(Type type, string propertyPath, bool throwExceptionIfNotFound = true)
    {
        if (propertyPath == null)
        {
            return false;
        }

        Type currentType = type;
        PropertyInfo? currentProperty;

        foreach (string p in propertyPath.Split('.'))
        {
            if (!string.IsNullOrWhiteSpace(p))
            {
                currentProperty = currentType.GetProperty(p, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

                if (currentProperty == null)
                {
                    if (throwExceptionIfNotFound)
                    {
                        throw new NotSupportedException($"ERROR: Property '{p}' in '{propertyPath}' does not exist.");
                    }

                    return false;
                }
                else
                {
                    currentType = currentProperty.PropertyType;
                }
            }
        }

        return true;
    }

    public static LocalizedString[] GetLocalizedProperties(this object? obj)
    {
        if (obj == null)
        {
            return new LocalizedString[0];
        }

        var localizedProperties = new List<LocalizedString>();

        foreach (PropertyInfo prop in obj.GetType().GetProperties())
        {
            var value = prop.GetValue(obj);

            if (value != null && value.GetType() == typeof(LocalizedString))
            {
                localizedProperties.Add((LocalizedString)value);
            }
        }

        return localizedProperties.ToArray();
    }

    public static string ToJson(this object? obj, bool indented = true)
    {
        return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
    }
}
