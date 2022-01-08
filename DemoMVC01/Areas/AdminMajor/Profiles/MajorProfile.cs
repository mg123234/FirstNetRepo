using AutoMapper;
using DemoMVC01.Areas.AdminMajor.ViewModels;
using DemoMVC01.Models;

namespace DemoMVC01.Areas.AdminMajor.Profiles
{

    public class MajorProfile : Profile
    {

        public MajorProfile()
        {
            CreateMap<Major, MajorVM>()
                .ForMember(vm => vm.Name, m => m.MapFrom(x => x.Name));

        }

    }
}


