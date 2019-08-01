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
        private DialogStatus dStatus;

        //private IBaseWebServiceSettings Settings { get; set; }
        private IStatus sChecker;
        //public AliceController(IBaseWebServiceSettings settings)
        public AliceController(IStatus statusChecker)
        {
            //Settings = settings;
            sChecker = statusChecker;
        }

        [HttpPost]
        //api/Alice
        public AliceResponse CofeeRequest([FromBody] AliceRequest aliceRequest)
        {
            try
            {
                logger.Debug("Получен запрос от Алисы: " + JsonConvert.SerializeObject(aliceRequest));
                AliceResponse responce = new AliceResponse();
                if (aliceRequest.Request.OriginalUtterance == "")
                {
                    var text = "Привет,Босс";
                    var tts = "Прив+ет, Б+осс";
                    responce = aliceRequest.Reply(text, tts);
                }
                else if (aliceRequest.Request.OriginalUtterance.ToUpper().Contains("СДЕЛАЙ КОФЕ"))
                {
                    var answer = Answer(sChecker.CheckStatus());
                    responce = aliceRequest.Reply(answer);
                }
                else if (aliceRequest.Request.OriginalUtterance.ToUpper().Contains("ДОЛГО ЕЩЕ"))
                {
                    var answer = Answer(sChecker.CheckStatus());
                    responce = aliceRequest.Reply(answer);
                }
                else
                {
                    responce = aliceRequest.Reply("Непонятно, повторите", "Непон+ятно, повтор+ите");
                }
                return responce;
            } catch (Exception e)
            {
                return aliceRequest.Reply(e.Message + ":::" + sChecker.CheckStatus(), e.Message + ":::" + sChecker.CheckStatus(), true);
            } 
        }

        private Dictionary<string, object> Answer(int statusCode)
        {
            string answer = "";
            string answerTts = "";
            bool bIsDialogEnd = false;
            switch ((CofeeStatus)statusCode)
            {
                case CofeeStatus.artWorking:
                    {
                        answer = "Рисую сердечки пенкой";
                        answerTts = "Рис+ую серд+ечки п+енкой";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.boiling:
                    {
                        answer = "Кипячу воду";
                        answerTts = "";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.cleaning:
                    {
                        answer = "Вытираю плиту от пролившегося кофе";
                        answerTts = "";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.finished:
                    {
                        answer = "Босс, все готово, можете забирать кофе";
                        answerTts = "";
                        bIsDialogEnd = true;
                        break;
                    }
                case CofeeStatus.grinding:
                    {
                        answer = "Перемалываю кофейные зерна";
                        answerTts = "";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.outwork:
                    {
                        answer = "Еще ничего не готово, сейчас начну";
                        answerTts = "";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.pouring:
                    {
                        answer = "Наливаю кофе в кружку";
                        answerTts = "";
                        bIsDialogEnd = false;
                        break;
                    }
            }
            var result = new Dictionary<string, object>();
            result["text"] = answer;
            result["tts"] = answerTts;
            result["isEnd"] = bIsDialogEnd;
            return result;
        }
    }
}