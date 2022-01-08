
using DemoMVC01.Areas.AdminMajor.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DemoMVC01.Areas.AdminStudent.ViewModels
{
    public class StudentVM 
    {
        public int Id { get; set; }

        [Display(Name = "Name")]   
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        public MajorVM Major { get; set; }

    }


   
}
