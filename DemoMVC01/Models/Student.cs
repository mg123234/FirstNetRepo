

using DemoMVC01.Constants.Validation.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC01.Models
{
    [Table("Students")]
    public class Student
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        [Required, MaxLength(255)]
        [DataType(DataType.Text)]
        [StringLength(maximumLength: 255, MinimumLength = 3, ErrorMessage = ErrorMessage.Name)]
        public string Name { get; set; }

        [Display(Name = "Phone Number")]
        [Required, MaxLength(20)]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(RegularExpression.Phone, ErrorMessage = ErrorMessage.Phone)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Email Address")]
        [Required, MaxLength(255)]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.Email, ErrorMessage = ErrorMessage.Email)]
        public string EmailAddress { get; set; }

        [Display(Name = "Major Id")]
        [Required]
        public int MajorId { get; set; }

        [Display(Name = "Major Name")]
        [ForeignKey("MajorId")]
        public Major Major { get; set; }
    }
}
