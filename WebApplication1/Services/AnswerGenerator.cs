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

        void SetData(AliceRequest request);
    }

    public class AnswerGenerator : IAnswerGenerator
    {
        private IDBProcCaller procCaller;
        private ILot lot;
        private SkillCategory category;
        private string[] words;
        private EntityModel[] entities;
        private string session;
        public AnswerGenerator(IDBProcCaller caller, ILot lotMaker)
        {
            procCaller = caller;
            lot = lotMaker;
            category = SkillCategory.no;
        }



        /// <summary>
        /// генерация ответа от навыка на основе запроса пользователя
        /// </summary>
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
                case SkillCategory.lot: //кто пойдет на обед
                    {
                        reply = LotMaker();
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

        public void SetData(AliceRequest request)
        {
            SetTokens(request.Request.nlu.tokens);
            SetEntities(request.Request.nlu.Entity);
            session = request.Session.SessionId;
        }

        private void SetTokens(string[] tokens)
        {
            if (category == SkillCategory.no)
            {
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
                    } else if (tokens.Contains("запоминай") && tokens.Contains("имена"))
                    {
                        category = SkillCategory.lot;
                    }
                    else
                    {
                        category = SkillCategory.unknown;
                    }
                }
            }
            words = tokens;
        }

        private void SetEntities(EntityModel[] ent)
        {
            entities = ent;
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
                category = SkillCategory.no;
                return "Задача передана кофе-машине";
            }
            else
            {
                //status != ok, кофемашина занята

                //ответ = значение из поля status
                return result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString();
            }
        }

        private string LotMaker()
        {
            ///проверка есть ли записи по сессии в базе

            //записи есть
            //if (записи есть)
            //{
            lot.Session = session;
            if (!(words.Intersect(new string[] { "выбирай", "жребий" }).Count() > 0))
            {
                return lot.AddMember(entities);
            } else
            {
                category = SkillCategory.no;
                return lot.GetMember();
            }
            //} else (записей нет) {
            //
            //}
        }

        
    }
}
