using API.TaskManagement.EC;
using Library.TaskManagement.Standard.DTO;
using Microsoft.AspNetCore.Http;
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
    public class ItemController : ControllerBase
    {
        private readonly ILogger<ItemController> _logger;

        public ItemController(ILogger<ItemController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<ItemDTO> Get()
        {
            List<ItemDTO> results = new List<ItemDTO>();
            results.AddRange(new ToDoEC().Get().ToList());
            results.AddRange(new AppointmentEC().Get().ToList());
            return results;
        }

        [HttpGet("Search")]
        public IEnumerable<ItemDTO> Search([FromQuery] string key)
        {
            key.Trim();
            List<ItemDTO> results = Get().Where(i => string.IsNullOrEmpty(key)
                                || (i?.Name?.ToUpper()?.Contains(key.ToUpper()) ?? false)
                                || (i?.Description?.ToUpper()?.Contains(key.ToUpper()) ?? false)
                                || ((i as AppointmentDTO)?.Attendees?
                                .Select(t => t.ToUpper())?.Contains(key.ToUpper()) ?? false)).ToList();
            return results;
        }
    }
}
