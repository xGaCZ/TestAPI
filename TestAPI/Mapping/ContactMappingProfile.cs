using AutoMapper;
using TestNetAPI.Entities;
using TestNetAPI.Models;

namespace TestNetAPI.Mapping
{
    public class ContactMappingProfile : Profile
    {

        public ContactMappingProfile()
        {
            CreateMap<Contact, CreateContactDto>();
            CreateMap<CreateContactDto, Contact>();
            CreateMap<CreateDetailDto, Detail>();
            CreateMap<Detail, CreateDetailDto>();
        }
    }
}
//* użyty został automaper aby automatycznie połączył nam "name" z klasy Contact z "name" z klasy ContactDto
