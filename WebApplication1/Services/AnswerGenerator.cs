using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AliseCofeemaker.Models;
using AliseCofeemaker.Modules;
using AliceCofeemaker.Enums;

namespace AliseCofeemaker.Services
{
    public interface IAnswerGenerator
    {
        string Generate();

        void SetTokens(string[] tokens);
    }

    public class AnswerGenerator : IAnswerGenerator
    {
        private IDBProcCaller procCaller;
        private SkillCategory category;
        private string[] words;
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
        public string Generate()
        {
            string reply = "";

            switch (category)
            {
                case SkillCategory.start: //пустое сообщение на входе
                    {
                        reply = "Привет, я голосовой офисный помошник. Можете попросить меня сварить кофе, заказать обед или записать на прием";
                        break;
                    }
                case SkillCategory.cofeeMachine: //свари/сделай/приготовь кофе
                    {
                        reply = CofeeMaker();
                        break;
                    }
                case SkillCategory.unknown: //неизвестный запрос
                    {
                        reply = "Не понимаю о чем вы, попробуйте еще раз";
                        break;
                    }
            }
            
            return reply;
        }

        public void SetTokens(string[] tokens)
        {
            if (//category не пустая)
            {

            }

            if (tokens.Count() == 0)
            {
                category = SkillCategory.start;
            }
            else
            {
                //на входе не пустое сообщение
                if (tokens.Contains("кофе") && tokens.Intersect(new string[] { "свари", "сделай", "приготовь" }).Count() > 0)
                {
                    category = SkillCategory.cofeeMachine;
                }
                else
                {
                    category = SkillCategory.unknown;
                }
            }
            words = tokens;
        }

        private string CofeeMaker()
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
                return "Задача передана кофе-машине";
            }
            else
            {
                //status != ok, кофемашина занята

                //ответ = значение из поля status
                return result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString();
            }
        }
    }
}
