using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestNetAPI.Entities;
using TestNetAPI.Exceptions;
using TestNetAPI.Models;

namespace TestNetAPI.Services
{
    public interface IDetailService
    {
        int Create(int contactId, CreateDetailDto dto);
        DetailDto GetById(int contactId, int detailId);
        List<DetailDto> GetAll(int contactId);
        void Update(int id , DetailDto dto);
    }
    public class DetailService : IDetailService
    {
        private readonly NetDbContext _context;
        private readonly IMapper _mapper;


        public DetailService(NetDbContext context,IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
        }
        public int Create(int contactId, CreateDetailDto dto)//Metoda ta tworzy nowy szczegół kontaktu dla określonego kontaktu o identyfikatorze contactId
        {
         var contact = _context.Contacts.FirstOrDefault(r=>r.Id == contactId);
            if ( contact == null ) 
                throw new NotFoundException("Nie znaleziono kontaktu");

            var detailEntity = _mapper.Map<Detail>(dto);
            detailEntity.ContactID = contactId;

            _context.Details.Add(detailEntity);
            _context.SaveChanges();

            return detailEntity.Id;
        }

        public DetailDto GetById(int contactId, int detailId)//Metoda ta zwraca szczegół o określonym identyfikatorze detailId
        {
            var contact = _context.Contacts.FirstOrDefault(r => r.Id == contactId);
            if (contact == null)
                throw new NotFoundException("Nie znaleziono kontaktu");

            var detail= _context.Details.FirstOrDefault(r => r.Id == detailId); // sprawdzamy czy do tego id przypisane są dane 
            if ( detail is null || detail.ContactID != contactId)
            {
                throw new NotFoundException("Nie znaleziono szczegółów");
            }

            var detailDto = _mapper.Map<DetailDto>(detail);
            return detailDto;
        }

        public List<DetailDto> GetAll(int contactId)
        {
            var contact = _context.Contacts.Include(r=> r.Detail).FirstOrDefault(r=>r.Id==contactId);
            if( contact is null ) throw new NotFoundException("Nie znaleziono kontaktu");

            var detailDtos = _mapper.Map<List<DetailDto>>(contact.Detail);
            return detailDtos;
        }


        public void Update(int id, DetailDto dto)//Metoda ta aktualizuje szczegół o określonym identyfikatorze id
        {
            var change = _context.Details.FirstOrDefault(r => r.Id == id);
            if (change is null) throw new NotFoundException("Nie znaleziono kontaktu");

            change.Email = dto.Email;
            change.Born = dto.Born;
            change.LastName = dto.LastName;
            change.Category = dto.Category;
            change.Subcategory = dto.Subcategory;
            change.Password = dto.Password;
            _context.SaveChanges();
        }
    }
}
