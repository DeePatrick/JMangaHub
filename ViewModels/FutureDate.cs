using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace JMangaHub.ViewModels
{
    public class FutureDate : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            DateTime dueDateTime;
            var isValid = DateTime.TryParseExact(Convert.ToString(value), "dd MMM yyyy", CultureInfo.CurrentCulture, DateTimeStyles.None, out dueDateTime);

            return (isValid && dueDateTime > DateTime.Now);
        }
    }
}