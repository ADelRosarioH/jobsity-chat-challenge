using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobsityStocksChat.Core.Entities;
using JobsityStocksChat.Core.Interfaces;
using JobsityStocksChat.WebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JobsityStocksChat.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessagesController : ControllerBase
    {
        private readonly IChatMessageService _service;
        public MessagesController(IChatMessageService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetLast50Messages()
        {
            var messages = await _service.GetLast50MessagesAsync();

            var result = messages.Select(t => new ChatMessageViewModel { 
               Id = t.Id,
               Message = t.Message,
               CreatedAt = t.CreatedAt,
               UserName = t.User.UserName
            });

            return Ok(result);
        }
    }
}
