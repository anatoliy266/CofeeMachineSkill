using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker.Models;

namespace AliseCofeemaker.Modules
{
    public interface ILot
    {
        string Session { get; set; }
        string AddMember(EntityModel[] entities);
        string GetMember();
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

        public string AddMember(EntityModel[] entities)
        {
            if (entities.Count() > 0)
            {
                int count = 0;
                foreach (var ent in entities)
                {
                    if (ent.Type == "YANDEX.FIO" && ent.Values.FirstName != null)
                    {
                        Dictionary<string, object> parameters = new Dictionary<string, object>();
                        parameters["action"] = "ADD";
                        parameters["session"] = session;
                        parameters["name"] = ent.Values.FirstName;
                        var result = procCaller.Call("AliceLotServise", parameters);

                        if (result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString() == "ok")
                        {
                            count++;
                        }
                        else
                        {
                            return result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString();
                        }
                    }
                }

                if (count > 0)
                {
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
                    return "Имя не распознано, попробуйте еще раз";
                }
            }
            else
            {
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
                    procCaller.Call("AliceLotService", new Dictionary<string, object> { ["action"] = "DEL" });
                    return "я вбираю " + name;
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
    }
}
