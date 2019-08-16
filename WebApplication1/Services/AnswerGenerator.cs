using System;
using System.Collections.Generic;
using System.Linq;
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

            
            if (tokens.Contains("кофе") && tokens.Intersect(new string[] { "свари", "сделай", "приготовь" }).Count() > 0)
            {
                var result = procCaller.Call("CofeeMashineStatus");
            }

            return reply;
        }

    }
}
