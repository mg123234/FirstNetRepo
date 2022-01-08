
using DemoMVC01.Constants.Validation.Common;
using DemoMVC01.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC01.Models 
{
    [Table("Majors")]
    public class Major 

    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        [StringLength(50, MinimumLength = 3, ErrorMessage = ErrorMessage.Name)]
        [Class1]
        public string Name { get; set; }

        public ICollection<Student> Students { get; set; }

 
         
    }
}