using AutoMapper;
using DBSD_17037_16777_17286.DAL.Models;
using DBSD_17037_16777_17286.Models;



namespace DBSD_17037_16777_17286.DAL.Infrastructure
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.Person.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.Person.LastName));

            CreateMap<EmployeeViewModel, Employee>()
           .ForMember(dest => dest.Photo, opt => opt.MapFrom(src => ConvertToByteArray(src.PhotoFile)));
        }

        private byte[] ConvertToByteArray(IFormFile file)
        {
            if (file != null)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    return ms.ToArray();
                }
            }
            else
            {
                return null;
            }
        }
    }



}
