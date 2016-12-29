using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JMangaHub.ViewModels
{
    public class ValidTime : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dueDateTime;
            var isValid = DateTime.TryParseExact(Convert.ToString(value), "HH:mm", CultureInfo.CurrentCulture,
                DateTimeStyles.None, out dueDateTime);

            return (isValid);
        }
    }
}