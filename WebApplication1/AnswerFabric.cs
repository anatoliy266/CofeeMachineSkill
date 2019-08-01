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
            if (command.ToLower() == "")
            {
                Q = QPart.hello;
                A = APart.hello;
            }


            try
            {
                int statusCode = checker.CheckStatus();
                return CofeeStatusAnswers(statusCode);

            } catch (Exception e)
            {

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
