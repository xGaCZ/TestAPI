using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestNetAPI.Models;
using TestNetAPI.Services;

namespace TestNetAPI.Controllers
{
    [Route("api/contact/{contactId}/details")]
    [ApiController]
    [Authorize]
    public class DetailsController : ControllerBase
    {
        private readonly IDetailService _detailService;
    
        public DetailsController(IDetailService detailService)
        {
            _detailService = detailService;
        }
        [HttpPost]
        public ActionResult Post([FromRoute] int contactId, [FromBody] CreateDetailDto dto)
        {
            var addDetail = _detailService.Create(contactId, dto);
            return Created($"api/contact/{contactId}/detail/{addDetail}", null);
        }
        [HttpGet("{detailId}")]
        public ActionResult<DetailDto> Get([FromRoute] int contactId, [FromRoute] int detailId)
        {
            DetailDto detail = _detailService.GetById(contactId, detailId);
            return Ok(detail);
        }
        [HttpGet]
        public ActionResult<List<DetailDto>> GetAll([FromRoute] int contactId)
        {
            var result = _detailService.GetAll(contactId);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody] DetailDto dto, [FromRoute]int id)
        {
            _detailService.Update(id ,dto);
            return Ok();
        }
    }
}
