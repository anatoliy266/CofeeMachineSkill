using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker;
using AliseCofeemaker.Controllers;

namespace AliseCofeemaker
{
    

    public interface IAnswerFabric
    {
        Dictionary<string, object> Answer(string command);
    }
    public class AnswerFabric : IAnswerFabric
    {
        private IStatus checker;
        private IReplicStorage replics;

        private QPart Q;
        private APart A;


        public AnswerFabric(IStatus status, IReplicStorage rs)
        {
            checker = status;
            replics = rs;
            //_dPart = DialogPart.noDial;
        }
        public Dictionary<string, object> Answer(string command)
        {
            Dictionary<string, object> responseProperties = new Dictionary<string, object>();
            bool isEnd = false;
            var answers = replics.GetAnswersCollection();

            if (command.ToLower() == "")
            {
                Q = QPart.hello;
                A = APart.hello;
            } else if(replics.GetAnswers(APart.end).Any(s => s.IndexOf(command.ToLower(), StringComparison.CurrentCultureIgnoreCase) > -1))
            {
                A = APart.end;
                Q = QPart.end;
                isEnd = true;
            } else
            {

                A = APart.end;
                Q = QPart.error;
                isEnd = true;
                foreach (var key in answers.Keys)
                {
                    if (answers[key].Any(s => s.IndexOf(command.ToLower(), StringComparison.CurrentCultureIgnoreCase) > -1))
                    {
                        A = key;
                        Q = replics.GetNextQuestion(A);
                        break;
                    }
                }
            }

            if (Q == QPart.status && A == APart.status)
            {
                responseProperties = GetCofeeStatus();
                responseProperties["text"] += GetRandomQuestion(replics.GetQuestions(Q));
                responseProperties["tts"] += (string)responseProperties["text"];

            } else
            {
                responseProperties["text"] = GetRandomQuestion(replics.GetQuestions(Q));
                responseProperties["tts"] = responseProperties["text"];
                responseProperties["isEnd"] = isEnd;
            }
            return responseProperties;
        }

        private string GetRandomQuestion(string[] questions)
        {
            Random r = new Random();
            int pos = r.Next(questions.Count() - 1);
            return questions.ToList()[pos];
        }


        private Dictionary<string, object> GetCofeeStatus()
        {
            try
            {
                int statusCode = checker.CheckStatus();
                return CofeeStatusAnswers(statusCode);
            }
            catch (Exception e)
            {
                var exceptionResult = new Dictionary<string, object>();
                exceptionResult["text"] = "Кажется, кофемашина не работает";
                exceptionResult["tts"] = "Кажется, кофемашина не работает";
                exceptionResult["isEnd"] = true;
                return exceptionResult;
            }
        }

        private Dictionary<string, object> CofeeStatusAnswers(int statusCode)
        {
            string answer = "";
            string answerTts = "";
            bool bIsDialogEnd = false;
            switch ((CofeeStatus)statusCode)
            {
                case CofeeStatus.artWorking:
                    {
                        answer = "Рисую сердечки пенкой";
                        answerTts = "Рис+ую серд+ечки п+енкой";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.boiling:
                    {
                        answer = "Кипячу воду";
                        answerTts = "Кипяч+у вод+у";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.cleaning:
                    {
                        answer = "Вытираю плиту от пролившегося кофе";
                        answerTts = "Вытир+аю плит+у ще прол+ившегося к+офе";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.finished:
                    {
                        answer = "Босс, все готово, можете забирать свой кофе";
                        answerTts = "Босс, вс+е гот+ово, м+ожете забир+ать св+ой к+офе";
                        bIsDialogEnd = true;
                        break;
                    }
                case CofeeStatus.grinding:
                    {
                        answer = "Перемалываю кофейные зерна";
                        answerTts = "Перем+алываю коф+ейные з+ерна";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.outwork:
                    {
                        answer = "Еще ничего не готово, сейчас начну";
                        answerTts = "Еще ничег+о не гот+ово, сейч+ас начн+у";
                        bIsDialogEnd = false;
                        break;
                    }
                case CofeeStatus.pouring:
                    {
                        answer = "Наливаю кофе в кружку";
                        answerTts = "Налив+аю к+офе в кр+ужку";
                        bIsDialogEnd = false;
                        break;
                    }
            }
            var result = new Dictionary<string, object>();
            result["text"] = answer;
            result["tts"] = answerTts;
            result["isEnd"] = bIsDialogEnd;
            return result;
        }
    }
}
