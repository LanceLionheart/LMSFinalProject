using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LMSFinalProject.UI.MVC.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "* Name is required *")]
        public string Name { get; set; }

        [Required(ErrorMessage = "* Email is required *")]
        [DataType(DataType.EmailAddress, ErrorMessage = "**Email is required**")]
        public string Email { get; set; }

        [Required(ErrorMessage = "* Subject is required *")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "* Message is required *")]
        [UIHint("MultilineText")]
        public string Message { get; set; }
    }
}