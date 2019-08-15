using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker.Models;

namespace AliseCofeemaker.Services
{
    public interface IAnswerGenerator
    {
        string Generate(string[] tokens, EntityModel[] entities = null);
    }

    public class AnswerGenerator : IAnswerGenerator
    {
        public AnswerGenerator()
        {
            
        }


        public string Generate(string[] tokens, EntityModel[] entities = null)
        {
            string reply = "";

            if (tokens.Contains())
            

            return reply;
        }

    }
}
