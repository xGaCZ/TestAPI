using TestNetAPI.Models;

namespace TestNetAPI.Services
{
    public interface IContactService
    {
        int Create(CreateContactDto dto);
        IEnumerable<CreateContactDto> GetAll();
        CreateContactDto GetById(int id);
        void Delete(int id);
        void Update (int id, UpdateContactDto dto);
    }
}