using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NLog;
using StranzitOnline.Common.Models;
using System.Linq;
using System;
using System.Collections.Generic;
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

        private IAnswerFabric answerFabric;
        public AliceController(IAnswerFabric fabric)
        {
            answerFabric = fabric;
        }

        [HttpPost]
        //api/Alice
        public AliceResponse CofeeRequest([FromBody] AliceRequest aliceRequest)
        {
            logger.Debug("Получен запрос от Алисы: " + JsonConvert.SerializeObject(aliceRequest));
            var answerProps = answerFabric.Answer(aliceRequest.Request.OriginalUtterance);
            return aliceRequest.Reply(answerProps);
        }

        
    }
}