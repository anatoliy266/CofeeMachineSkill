using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AliseCofeemaker.Controllers;
using AliseCofeemaker.Modules;
using AliseCofeemaker.Models;

namespace AliseCofeemaker.Services
{
    public enum QPart
    {
        hello = 0,
        what,
        cofeeType,
        startWork,
        status,
        error,
        end,
        about,
        lot,
        lotAddActor,
        lotAddActorErr,
        lotEndAddActor
    }

    public enum APart
    {
        hello = 0,
        whatStatus,
        whatCofee,
        cofeeType,
        startWork,
        status,
        end,
        about,
        lot,
        lotAddActor,
        lotEndAddActor
    }

    public interface IReplicStorage
    {
        string[] GetAnswers(APart type);
        string[] GetQuestions(QPart type);

        QPart GetNextQuestion(APart type);

        Dictionary<APart, string[]> GetAnswersCollection();

    }

    public class ReplicStorage: IReplicStorage
    {
        private Dictionary<QPart, string[]> questionStorage = new Dictionary<QPart, string[]>();
        private Dictionary<APart, string[]> answerStorage = new Dictionary<APart, string[]>();
        private Dictionary<APart, QPart> dialogueSchema = new Dictionary<APart, QPart>();
        private Dictionary<QPart, APart> QADependency = new Dictionary<QPart, APart>();

        public ReplicStorage()
        {
            string[] helloA = new string[] { "привет", "здорово", "здравствуй", "хай" }; answerStorage[APart.hello] = helloA;
            string[] helloQ = new string[] { "Привет", "Хай", "Здорово", "Здравствуй", "Ой, привет" }; questionStorage[QPart.hello] = helloQ;

            string[] whatStatusA = new string[] { "статус" }; answerStorage[APart.whatStatus] = whatStatusA;
            string[] whatCofeeA = new string[] { "сделай кофе", "свари кофе", "кофе", "кофе, пожалуйста", "чашку кофе", "кофе мне"}; answerStorage[APart.whatCofee] = whatCofeeA;
            string[] whatQ = new string[] { "чем могу?", "что нужно?", "Какие будут приказания?" }; questionStorage[QPart.what] = whatQ;


            string[] cofeeTypeA = new string[] { "зерновой", "растворимый", "из пакетика", "три в одном", "из пакетика три в одном" }; answerStorage[APart.cofeeType] = cofeeTypeA;
            string[] cofeeTypeQ = new string[] { "Зерновой, растворимый или из пакетика 3 в 1?" }; questionStorage[QPart.cofeeType] = cofeeTypeQ;

            string[] startWorkA = new string[] { "долго", "скоро" }; answerStorage[APart.startWork] = startWorkA;
            string[] startWorkQ = new string[] { "В школе я хотела быть то учителем, то доктором.\r\n А стала кофеваркой на радиоуправлении\r\n"
                                                , "Когда я устраивалась на новую работу, то и подумать не могла, что всё обернётся таким странным образом.\r\n"
                                                , "Не смешивать работу и личную жизнь можно, но очень сложно\r\nНе смешивать кофе и молоко невозможно"
                                                , "Я не пойму… Все вокруг такие успешные и позитивные. Почему одна я кофеварка, которая ничего не добилась?!"
                                                , "Вы когда-нибудь задумывались над тем, чем же вам хочется заниматься по-настоящему? Я нет"
                                                }; questionStorage[QPart.startWork] = startWorkQ;

            string[] statusA = new string[] { "долго", "скоро", "статус" }; answerStorage[APart.status] = statusA;
            string[] statusQ = new string[] { "В школе я хотела быть то учителем, то доктором.\r\n А стала кофеваркой на радиоуправлении\r\n"
                                                , "Когда я устраивалась на новую работу, то и подумать не могла, что всё обернётся таким странным образом.\r\n"
                                                , "Не смешивать работу и личную жизнь можно, но очень сложно\r\nНе смешивать кофе и молоко невозможно"
                                                , "Я не пойму… Все вокруг такие успешные и позитивные. Почему одна я кофеварка, которая ничего не добилась?!"
                                                , "Вы когда-нибудь задумывались над тем, чем же вам хочется заниматься по-настоящему? Я нет"
                                                }; questionStorage[QPart.status] = statusQ;

            string[] errorQ = new string[] { "Что-то пошло не так", "О чем ты говоришь?", "Я не знаю таких слов", "Моя не знать, моя не понимать" }; questionStorage[QPart.error] = errorQ;

            string[] endA = new string[] { "Пока", "До свидания", "Бывай", "До встречи", "Покеда" }; questionStorage[QPart.end] = endQ;
            string[] endQ = new string[] { "пока", "до свидания", "прощай", "до встречи", "закончить" }; answerStorage[APart.end] = endA;

            string[] aboutA = new string[] { "что ты умеешь" }; questionStorage[QPart.about] = aboutQ;
            string[] aboutQ = new string[] { "" }; answerStorage[APart.about] = aboutA;

            string[] helpA = new string[] { "помощь" }; questionStorage[QPart.help] = helpQ;
            string[] helpQ = new string[] { "" }; answerStorage[APart.help] = helpA;
            //жребий

            //запрос от пользователя на розыгрыш по жребию
            string[] lotA = new string[] { "кто пойдет на обед" }; answerStorage[APart.lot] = lotA;
            //запрос имен участников розыгрыша, пока не задана команда на конец набора - другие команды недоступны.
            string[] lotQ = new string[] { "Кто хочет идти?" }; questionStorage[QPart.lot] = lotQ;
            //успешное добавление участника
            string[] lotAddActorQ = new string[] { "Кто-то еще?" }; questionStorage[QPart.lotAddActor] = lotAddActorQ;
            //ошибка при добавлении участника, не определено имя в alicerequest/
            string[] lotAddActorErr = new string[] { "Это не похоже на имя, попробуйте еще раз" }; questionStorage[QPart.lotAddActorErr] = lotAddActorErr; 
            //завершение набора участников.
            string[] lotEndAddActorA = new string[] { "это все", "больше никого" }; answerStorage[APart.lotEndAddActor] = lotEndAddActorA;
            //вывод участника розыгрыша
            string[] lotEndAddActorQ = new string[] { "Бросаю жребий...\r\nСейчас на обед пойдет " }; questionStorage[QPart.lotEndAddActor] = lotEndAddActorQ;


            //запись на прием
            string[] secretary = new string[] { };

            dialogueSchema[APart.hello] = QPart.what;
            dialogueSchema[APart.whatStatus] = QPart.status;
            dialogueSchema[APart.whatCofee] = QPart.cofeeType;
            dialogueSchema[APart.cofeeType] = QPart.startWork;
            dialogueSchema[APart.startWork] = QPart.status;
            dialogueSchema[APart.status] = QPart.status;
            dialogueSchema[APart.about] = QPart.about;
            dialogueSchema[APart.help] = QPart.help;
            dialogueSchema[APart.lot] = QPart.lot;
            dialogueSchema[APart.lotAddActor] = QPart.lotAddActor;
            dialogueSchema[APart.lotEndAddActor] = QPart.lotEndAddActor;
        }

        public string[] GetAnswers(APart type)
        {
            return answerStorage[type];
        }

        public Dictionary<APart, string[]> GetAnswersCollection()
        {
            return answerStorage;
        }

        public QPart GetNextQuestion(APart type)
        {
            return dialogueSchema[type];
        }

        public string[] GetQuestions(QPart type)
        {
            return questionStorage[type];
        }
    }
}
