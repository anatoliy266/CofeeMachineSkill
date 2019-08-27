using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using AliceCofeemaker.Enums;

namespace AliseCofeemaker.Modules
{
    public interface ILunch
    {
        string Session { get; set; }
        string GetMenu();
        string GetDescription(string item);
        string CheckOrder();
    }

    public class Lunch : ILunch
    {
        private IDBProcCaller db;
        private string session;

        public Lunch(IDBProcCaller caller)
        {
            db = caller;
        }

        public string Session { get => session; set => session = value; }

        public string CheckOrder()
        {
            LunchOrderState state;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters["session"] = session;
            parameters["action"] = "GET";
            var result = db.Call("AliseLunchService", parameters);
            if (result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("lunch_number")] == null)
            {
                state = LunchOrderState.noItem;
            } else if (result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("address")] == null)
            {
                state = LunchOrderState.noAddess;
            } else
            {
                state = LunchOrderState.complete;
            }
            

            return MakeOrder(state);
        }

        private string MakeOrder(LunchOrderState orderState)
        {
            switch (orderState)
            {
                case LunchOrderState.noItem:
                    {
                        break;
                    }
                case LunchOrderState.noAddess:
                    {
                        break;
                    }
                case LunchOrderState.complete:
                    {
                        break;
                    }
            }
            return "";
        }

        public string GetDescription(string item)
        {
            return "";
        }

        public string GetMenu()
        {
            //AliseLunchServise @action = menu
            return "";
        }
    }
}
