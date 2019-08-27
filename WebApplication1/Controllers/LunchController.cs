using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using AliseCofeemaker.Models;
using AliseCofeemaker.Services;

namespace AliseCofeemaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LunchController : ControllerBase
    {
        private IAnswerGenerator generator;
        public LunchController(IAnswerGenerator gen)
        {
            generator = gen;
        }

        // POST: api/Lunch
        [HttpPost]
        public AliceResponse Post([FromBody] AliceRequest value)
        {
            generator.SetData(value);
            var result = generator.Generate();
            return value.Reply(result);
        }

        
    }
}
