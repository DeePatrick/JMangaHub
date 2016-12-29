using JMangaHub.Controllers;
using JMangaHub.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace JMangaHub.Models.ViewModels
{
    public class MangaFormViewModel
    {
        public int Id { get; set; }

        [Required]
        public string MangaName { get; set; }

        [Required]
        public string Vendor { get; set; }

        public string Summary { get; set; }

        [Required]
        [FutureDate]
        public string DueDate { get; set; }

        [Required]
        [ValidTime]
        public string DueTime { get; set; }

        [Required]
        public byte Genre { get; set; }

        public IEnumerable<Genre> Genres { get; set; }

        public DateTime GetDueDateTime()
        {
            return DateTime.Parse(string.Format("{0} {1}", DueDate, DueTime));
        }

        public string Heading { get; set; }

        public string Action
        {
            get
            {
                Expression<Func<MangasController, ActionResult>> update =
                    (c => c.Update(this));

                Expression<Func<MangasController, ActionResult>> create =
                    (c => c.Create(this));

                var action = (Id != 0) ? update : create;
                return (action.Body as MethodCallExpression).Method.Name;
            }
        }

    }
}