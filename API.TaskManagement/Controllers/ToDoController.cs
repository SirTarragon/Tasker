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
    public class ToDoController : ControllerBase
    {
        private readonly ILogger<ToDoController> _logger;
        public ToDoController(ILogger<ToDoController> logger)
        {
            _logger = logger;
        }

        [HttpGet()]
        public IEnumerable<ItemDTO> Get()
        {
            return new ToDoEC().Get();
        }

        [HttpPost("AddOrUpdate")]
        public ToDoDTO AddOrUpdate([FromBody] ToDoDTO todo)
        {
            return new ToDoEC().AddOrUpdate(todo);
        }

        [HttpPost("Delete")]
        public ToDoDTO Delete([FromBody] DeleteItemDTO deleteItem)
        {
            return new ToDoEC().Delete(deleteItem.IDToDelete);
        }
    }
}
