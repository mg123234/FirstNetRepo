using System.ComponentModel.DataAnnotations;

namespace DemoMVC01.Areas.AdminMajor.ViewModels
{
    public class MajorVM
    {
        [Display(Name = "Major Name")]
        public string Name { get; set; }
    }
}
