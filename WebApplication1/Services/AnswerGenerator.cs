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

        void SetData(string[] tokens, EntityModel[] ent);
    }

    public class AnswerGenerator : IAnswerGenerator
    {
        private IDBProcCaller procCaller;
        private ILot lot;
        private SkillCategory category;
        private string[] words;
        EntityModel[] entities;
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
                        break;                    }
                case SkillCategory.unknown: //неизвестный запрос
                    {
                        reply = "Не понимаю о чем вы, попробуйте еще раз";
                        break;
                    }
            }
            
            return reply;
        }

        public void SetData(string[] tokens, EntityModel[] ent)
        {
            SetTokens(tokens);
            SetEntities(new EntityModel[] { });
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
                    } else if (tokens.Contains("обед"))
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
            if (words.Contains("все"))
            {
                return lot.GetMember();
            } else
            {
                if (entities.Count() > 0)
                {
                    bool bIsName = false;
                    foreach (var entity in entities)
                    {
                        if (entity.Values.FirstName != null)
                        {
                            lot.AddMember(entity.Values.FirstName);
                            bIsName = true;
                        } 
                    }

                    if (bIsName)
                    {
                        return "Хорошо, еще кто-нибудь?";
                    }
                    else
                    {
                        return "не похоже на имя, попробуйте еще раз";
                    }
                }
                else
                {
                    return "Сперва назовите несколько имен";
                }
            }

            
        }
    }
}
