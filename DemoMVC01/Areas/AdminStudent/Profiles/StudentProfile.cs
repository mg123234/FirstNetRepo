using AutoMapper;

using DemoMVC01.Areas.AdminStudent.ViewModels;
using DemoMVC01.Models;

namespace DemoMVC01.Areas.AdminStudent.Profiles
{
    public class StudentProfile : Profile
    {

        public StudentProfile()
        {
            CreateMap<Student, StudentVM>();
            //.ForMember(vm => vm.Major, m => m.MapFrom(x => x.Major.Name));
            CreateMap<StudentCreateVM, Student>();
            CreateMap<StudentEditVM, Student>();
            CreateMap<Student, StudentEditVM>();
            CreateMap<Student, StudentDetailsVM>();
        }

    }
    

    
    
}
