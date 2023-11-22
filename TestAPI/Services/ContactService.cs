using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestNetAPI.Entities;
using TestNetAPI.Exceptions;
using TestNetAPI.Models;

namespace TestNetAPI.Services
{
    public class ContactService : IContactService
    {
        private readonly NetDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ContactService> _logger;

        public ContactService(NetDbContext dbContext, IMapper mapper, ILogger<ContactService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public void Update(int id ,UpdateContactDto dto)
        {
            var update = _dbContext.Contacts.FirstOrDefault(r => r.Id == id);
            if (update is null) throw new NotFoundException("Nie znaleziono kontaktu");

            update.Phone = dto.Phone;
            update.Name = dto.Name;
            
            _dbContext.SaveChanges();
           
        }

        public CreateContactDto GetById(int id)
        {

            var contact = _dbContext.Contacts.FirstOrDefault(x => x.Id == id);
            if (contact is null) throw new NotFoundException("Nie znaleziono kontaktu");
            var result = _mapper.Map<CreateContactDto>(contact);
            return result;
        }
        public IEnumerable<CreateContactDto> GetAll()
        {
            var Contact = _dbContext.Contacts.ToList();
            var contactsDtos = _mapper.Map<List<CreateContactDto>>(Contact);
            return contactsDtos;
        }

        public int Create(CreateContactDto dto)
        {
            var contact = _mapper.Map<Contact>(dto);
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
            return contact.Id;
        }
        public void Delete(int id) //* bool ponieważ jeśli nie ma takiego ID w bazie danych to klient zostanie poinformowany o tym 
        {
            _logger.LogError($"kontakt z id: {id} użyto akcji DELETE");
            var delete = _dbContext.Contacts.FirstOrDefault(r => r.Id == id);
            if (delete is null) throw new NotFoundException("Nie znaleziono kontaktu");

            _dbContext.Contacts.Remove(delete);
            _dbContext.SaveChanges();//* zapisuje operacje w bazie danych 
         
        }
    }
}
