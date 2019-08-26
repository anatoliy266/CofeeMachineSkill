using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using AliseCofeemaker.Controllers;
using AliseCofeemaker.Services;
using AliseCofeemaker.Modules;
using AliseCofeemaker.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AliseCofeemaker.Controllers
{
    [Route("api/[controller]")]
    public class LotController : Controller
    {
        private IAnswerGenerator aGen;
        public LotController(IAnswerGenerator answer)
        {
            aGen = answer;
        }
        // POST api/<controller>
        [HttpPost]
        public AliceResponse Post([FromBody] AliceRequest aliceRequest)
        {
            aGen.SetData(aliceRequest);
            var answer = aGen.Generate();
            return aliceRequest.Reply(answer);
        }
    }
}
