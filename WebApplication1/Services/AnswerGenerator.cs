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


        public string Generate(string[] tokens = null, EntityModel[] entities = null)
        {
            string reply = "";
            if (tokens.Count() == 0)
            {
                reply = "Приветственное сообщение";
            } else
            {
                if (tokens.Contains("кофе") && tokens.Intersect(new string[] { "свари", "сделай", "приготовь" }).Count() > 0)
                {
                    var result = procCaller.Call("CofeeMashineStatus");
                    if (result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString() == "ok")
                    {
                        WebClient client = new WebClient();
                        client.Headers.Add(HttpRequestHeader.Accept, "");
                        client.Headers.Add(HttpRequestHeader.Allow, "GET");
                        client.DownloadData("https://someurl.net");
                        reply = "Задача передана кофе-машине";
                    }
                    else
                    {
                        reply = result.Tables[0].Rows[0].ItemArray[result.Tables[0].Columns.IndexOf("status")].ToString();
                    }
                }
            }
            
            
            return reply;
        }
    }
}
