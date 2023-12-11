﻿using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace To_Do_List.API.Models
{
    public class TaskDTO
    {
        [BindNever]
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime Deadline { get; set; }
        public string Priority { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }

    public static class EnumExtensions
    {
        public static string GetEnumDisplayName(this Enum enumValue)
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            var displayAttribute = (DisplayAttribute)fieldInfo!.GetCustomAttribute(typeof(DisplayAttribute))!;

            return displayAttribute?.Name ?? enumValue.ToString();
        }

        public static TEnum ParseFromDisplayName<TEnum>(string displayName, bool ignoreCase = true)
        {
            Type enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new ArgumentException($"{nameof(TEnum)} must be an enumeration type.");
            }

            foreach (FieldInfo field in enumType.GetFields())
            {
                var displayAttribute = field.GetCustomAttribute<DisplayAttribute>(false);

                if (displayAttribute != null && string.Equals(displayAttribute.Name, displayName, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal))
                {
                    return (TEnum)field.GetValue(null)!;
                }
            }

            throw new ArgumentException($"No matching enum value found for display name '{displayName}'.");
        }
    }
}
