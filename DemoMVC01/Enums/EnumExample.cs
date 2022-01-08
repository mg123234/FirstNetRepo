using System.ComponentModel.DataAnnotations;

namespace DemoMVC01.Enums
{
    public enum EnumExample
    {
		Unknown,
		[Display(Name = "Not verified yet")]
		NotVerifiedYet,
		[Display(Name = "Verified by e-mail")]
		VerifiedByMail,
		[Display(Name = "Verified by phone")]
		VerifiedByPhone
	}
}
