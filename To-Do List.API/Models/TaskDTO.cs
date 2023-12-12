using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace To_Do_List.API.Models
{
    public class TaskDTO
    {
        [BindNever]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Обязательное поле"), MaxLength(16, ErrorMessage = "Максимум 16 символов")]
        [Display(Name = "Название")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле"), MaxLength(50, ErrorMessage = "Максимум 50 символов")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Срок")]
        public DateTime Deadline { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Приоритет")]
        public string Priority { get; set; } = string.Empty;
        [Required(ErrorMessage = "Обязательное поле")]
        [Display(Name = "Статус")]
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
