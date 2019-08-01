using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AliseCofeemaker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CofeeController : ControllerBase
    {
        private ICofee cofeeMaker; 

        public CofeeController(ICofee cofeemaker)
        {
            cofeeMaker = cofeemaker;
        }
        // GET: api/Cofee
        [HttpGet]
        public int Get()
        {
            cofeeMaker.ChangeCofeeStatus();
            return (int)cofeeMaker.status;
        }
    }
}
