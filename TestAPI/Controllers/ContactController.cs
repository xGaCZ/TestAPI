using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestNetAPI.Entities;
using TestNetAPI.Models;
using TestNetAPI.Services;

namespace TestNetAPI.Controllers
{
    [Route("api/contact")]//ścieżka bazowa
    public class ContactController : ControllerBase
    {

        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult Delete([FromRoute] int id) //Obsługuje żądanie DELETE do usunięcia kontaktu o określonym id
        {
           _contactService.Delete(id);

            return NoContent();
        }
        [HttpPut("{id}")]//Obsługuje żądanie PUT do aktualizacji istniejącego kontaktu o określonym id
        [Authorize]
        public ActionResult Update([FromBody] UpdateContactDto dto, [FromRoute]int id)
        {


        _contactService.Update(id, dto);

            return Ok();
        }


        [HttpPost]
        [Authorize]//Obsługuje żądanie POST do utworzenia nowego kontaktu na podstawie CreateContactDto
        public ActionResult CreateContact([FromBody] CreateContactDto dto)
        {

            var id = _contactService.Create(dto);

            return Created($"/api/contact/{id}", null);
        }

        [HttpGet]// Obsługuje żądanie GET do pobrania wszystkich kontaktów
        public ActionResult<IEnumerable<CreateContactDto>> GetAll()
        {

            var contactsDtos = _contactService.GetAll();


            return Ok(contactsDtos);
        }


        [HttpGet("{id}")]
        public ActionResult<CreateContactDto> Get([FromRoute] int id)
        {
            var contact = _contactService.GetById(id);

            return Ok(contact);

        }
    }
}
