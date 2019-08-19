using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using System.Linq;
using System;
using System.Collections.Generic;
using AliseCofeemaker.Controllers;
using AliseCofeemaker.Services;
using AliseCofeemaker.Modules;
using AliseCofeemaker.Models;
//using StranzitOnline.Common.Models.Settings.Base;
//using System.Web.Http;

namespace AliseCofeemaker.Controllers
{
    public enum DialogStatus
    {
        dialStart = 0,
        dialError,
        dialEnd,
        noDial
    }
    [Route("api/[controller]")]
    [ApiController]
    public class AliceController : ControllerBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private IAnswerGenerator generator;
        public AliceController(IAnswerGenerator gen)
        {
            generator = gen;
        }

        [HttpPost]
        //api/Alice
        public AliceResponse CofeeRequest([FromBody] AliceRequest aliceRequest)
        {
            logger.Debug("Получен запрос от Алисы: " + JsonConvert.SerializeObject(aliceRequest));
            var answer = generator.Generate(aliceRequest.Request.nlu.tokens, aliceRequest.Request.nlu.Entity);
            return aliceRequest.Reply("some");
        }

        
    }
}