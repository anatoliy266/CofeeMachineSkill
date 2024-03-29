﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker.Models;

namespace AliseCofeemaker.Modules
{
    public interface ILot
    {
        string Session { get; set; }
        EntityModel[] Entities { get; set; }
        string AddMember();
        string GetMember();
        bool bIsInit();
    }

    public class Lot : ILot
    {
        private IDBProcCaller procCaller;
        private string session;
        public Lot(IDBProcCaller caller)
        {
            procCaller = caller;
        }

        public string Session { get => session; set => session = value; }
        public EntityModel[] Entities { get => entities; set => entities = value; }

        private EntityModel[] entities;

        public string AddMember()
        {
            if (entities.Count() > 0)
            {
                //есть определенные алисой сущности
                int count = 0;
                foreach (var ent in entities)
                {
                    if (ent.Type == "YANDEX.FIO" && ent.Values.FirstName != null)
                    {
                        //сущность - имя
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters["action"] = "ADD";
                        parameters["session"] = session;
                        parameters["name"] = ent.Values.FirstName;
                        
                        var result = procCaller.Call("AliceLotServise", parameters);

                        if (result.Tables["status"].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString() == "ok")
                        {
                            //вставка прошла успешно
                            //status = ok
                            count++;
                        }
                        else
                        {
                            //ошибка при записи имени
                            //status != ok
                            return result.Tables["status"].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString();
                        }
                    }
                }

                if (count > 0)
                {
                    //имена были вставлены
                    switch (count)
                    {
                        case 1:
                            {
                                return "Имя добавлено";
                            }
                        default:
                            {
                                return "добавлено" + count + "имен";
                            }
                    }
                }
                else
                {
                    //ни одного имени не было вставлено
                    return "Имя не распознано, попробуйте еще раз";
                }
            }
            else
            {
                //сущностей в запросе нет
                return "Среди предложенных вариантов имен не обнаружено";
            }
        }

        public string GetMember()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["session"] = session;
            parameters["action"] = "GET";
            var result = procCaller.Call("AliceLotServise", parameters);
            if (result.Tables.Count != 0)
            {
                //были записаны имена по текущей сессии
                if (result.Tables[0].Rows.Count > 1)
                {
                    //несколько имен
                    Random r = new Random();
                    var i = r.Next(result.Tables[0].Rows.Count); //порядковый номер из массива имен
                    var name = result.Tables[0].Rows[i].ItemArray[result.Tables[0].Columns.IndexOf("name")];
                    procCaller.Call("AliceLotService", new Dictionary<string, object> { ["action"] = "DEL", ["session"] = session });
                    return "я выбираю " + name;
                }
                else
                {
                    //всего одно имя
                    return "Недостаточно имен для жребия";
                }
            }
            else
            {
                //ни одного имени по переданной сессии
                return "Недостаточно имен для жребия";
            }
        }

        public bool bIsInit()
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["session"] = session;
            parameters["action"] = "GET";
            var result = procCaller.Call("AliceLotServise", parameters);

            if (result.Tables["status"].Rows[0].ItemArray[result.Tables["status"].Columns.IndexOf("status")] != null )
            {
                return false;
            } else
            {
                Dictionary<string, object> p = new Dictionary<string, object>();
                parameters["session"] = session;
                parameters["action"] = "NEWSESSION";
                procCaller.Call("AliceLotServise", p);
                return true;
            }
        }
    }
}
