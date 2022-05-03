using API.TaskManagement.EC;
using Library.TaskManagement.Standard.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.TaskManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AppointmentController : ControllerBase
    {
        private readonly ILogger<AppointmentController> _logger;
        public AppointmentController(ILogger<AppointmentController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<ItemDTO> Get()
        {
            return new AppointmentEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public AppointmentDTO AddOrUpdate([FromBody] AppointmentDTO todo)
        {
            return new AppointmentEC().AddOrUpdate(todo);
        }

        [HttpPost("Delete")]
        public AppointmentDTO Delete([FromBody] DeleteItemDTO deleteItem)
        {
            return new AppointmentEC().Delete(deleteItem.IDToDelete);
        }
    }
}
