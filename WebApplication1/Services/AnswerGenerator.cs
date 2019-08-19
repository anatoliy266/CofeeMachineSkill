using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AliseCofeemaker.Models;
using AliseCofeemaker.Modules;

namespace AliseCofeemaker.Services
{
    public interface IAnswerGenerator
    {
        string Generate(string[] tokens, EntityModel[] entities = null);
    }

    public class AnswerGenerator : IAnswerGenerator
    {
        private IDBProcCaller procCaller;
        public AnswerGenerator(IDBProcCaller caller)
        {
            procCaller = caller;
        }



        /// <summary>
        /// генерация ответа от навыка на основе запроса пользователя
        /// </summary>
        /// <param name="tokens">массив слов запроса</param>
        /// <param name="entities">массив определенных процессором алисы сущностей</param>
        /// <returns>подготовленный ответ</returns>
        public string Generate(string[] tokens = null, EntityModel[] entities = null)
        {
            string reply = "";
            if (tokens.Count() == 0)
            {
                //на входе пустое сообщение(активация навыка)
                //в ответ приветственное сообщение
                reply = "Приветственное сообщение";
            } else
            {
                //на входе не пустое сообщение
                if (tokens.Contains("кофе") && tokens.Intersect(new string[] { "свари", "сделай", "приготовь" }).Count() > 0)
                {
                    //запрос к базе, возращает DataSet с олонкой status
                    var result = procCaller.Call("CofeeMashineStatus");

                    if (result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString() == "ok")
                    {
                        //status == ok

                        //GET запрос на внешний ресурс
                        WebClient client = new WebClient();
                        client.UseDefaultCredentials = true;
                        client.Headers.Add(HttpRequestHeader.Allow, "GET");
                        client.DownloadData("https://someurl.net");

                        //возвращаемый ответ
                        reply = "Задача передана кофе-машине";
                    }
                    else
                    {
                        //status != ok, кофемашина занята

                        //ответ = значение из поля status
                        reply = result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString();
                    }
                } 
            }
            return reply;
        }
    }
}
