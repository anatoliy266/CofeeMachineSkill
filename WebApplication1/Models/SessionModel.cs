using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace StranzitOnline.Common.Models
{

    /// <summary>
    /// Данные о сессии.
    /// </summary>
    public class SessionModel
    {
        /// <summary>
        /// Признак новой сессии
        /// </summary>
        [JsonProperty("new")]
        public bool New { get; set; }
        /// <summary>
        /// Уникальный идентификатор сессии
        /// </summary>
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
        /// <summary>
        /// Идентификатор сообщения в рамках сессии
        /// </summary>
        [JsonProperty("message_id")]
        public int MessageId { get; set; }
        /// <summary>
        /// Идентификатор вызываемого навыка. Чтобы узнать идентификатор своего навыка, откройте его в личном кабинете — идентификатор будет в адресе страницы
        /// </summary>
        [JsonProperty("skill_id")]
        public string SkillId { get; set; }
        /// <summary>
        /// Идентификатор экземпляра приложения, в котором пользователь общается с Алисой
        /// </summary>
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
    /// <summary>
    /// Данные для ответа пользователю
    /// </summary>
    public class ResponseModel
    {
        /// <summary>
        /// Текст, который следует показать и сказать пользователю. Максимум 1024 символа. Не должен быть пустым
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
        /// <summary>
        /// Ответ в формате TTS (text-to-speech), максимум 1024 символа
        /// </summary>
        [JsonProperty("tts")]
        public string Tts { get; set; }
        /// <summary>
        /// Признак конца разговора
        /// </summary>
        [JsonProperty("end_session")]
        public bool EndSession { get; set; }
        /// <summary>
        /// Кнопки, которые следует показать пользователю.
        /// </summary>
        [JsonProperty("buttons")]
        public ButtonModel[] Buttons { get; set; }
    }
    /// <summary>
    /// Кнопки, которые следует показать пользователю.
    /// </summary>
    public class ButtonModel
    {
        /// <summary>
        /// Текст кнопки, обязателен для каждой кнопки
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }
        /// <summary>
        /// Произвольный JSON, который Яндекс.Диалоги должны отправить обработчику, если данная кнопка будет нажата.
        /// </summary>
        [JsonProperty("payload")]
        public object Payload { get; set; }
        /// <summary>
        /// URL, который должна открывать кнопка, максимум 1024 байта.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        /// <summary>
        /// Признак того, что кнопку нужно убрать после следующей реплики пользователя
        /// </summary>
        [JsonProperty("hide")]
        public bool Hide { get; set; }
    }
    /// <summary>
    /// Информация об устройстве, с помощью которого пользователь разговаривает с Алисой.
    /// </summary>
    public class MetaModel
    {
        /// <summary>
        /// Язык в POSIX-формате, максимум 64 символа.
        /// </summary>
        [JsonProperty("locale")]
        public string Locale { get; set; }
        /// <summary>
        /// Название часового пояса, включая алиасы, максимум 64 символа.
        /// </summary>
        [JsonProperty("timezone")]
        public string Timezone { get; set; }
        /// <summary>
        /// Идентификатор устройства и приложения, в котором идет разговор
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }
    }
    /// <summary>
    /// Запрос Яндекс Диалогов на коллбэк функцию в соответствии с https://tech.yandex.ru/dialogs/alice/doc/protocol-docpage/
    /// </summary>
    public class AliceRequest
    {
        /// <summary>
        /// Информация об устройстве, с помощью которого пользователь разговаривает с Алисой.
        /// </summary>
        [JsonProperty("meta")]
        public MetaModel Meta { get; set; }
        /// <summary>
        /// Данные, полученные от пользователя.
        /// </summary>
        [JsonProperty("request")]
        public RequestModel Request { get; set; }
        /// <summary>
        /// Данные о сессии.
        /// </summary>
        [JsonProperty("session")]
        public SessionModel Session { get; set; }
        /// <summary>
        /// Версия протокола. Текущая версия — 1.0.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }
        /// <summary>
        /// простой текстовый ответ на запрос
        /// </summary>
        /// <param name="text"></param>
        /// <param name="endSession"></param>
        /// <param name="buttons"></param>
        /// <returns></returns>
        public AliceResponse Reply(string text, bool endSession = false, ButtonModel[] buttons = null)
        {
            return new AliceResponse
            {
                Response = new ResponseModel
                {
                    Text = text,
                    Tts = text,
                    EndSession = endSession
                },
                Session = Session
            };
        }

        public AliceResponse Reply(string text, string tts, bool endSession = false, ButtonModel[] buttons = null)
        {
            return new AliceResponse
            {
                Response = new ResponseModel
                {
                    Text = text,
                    Tts = tts,
                    EndSession = endSession
                },
                Session = Session
            };
        }

        public AliceResponse Reply(Dictionary<string, object> replyParams)
        {
            return new AliceResponse
            {
                Response = new ResponseModel
                {
                    Text = (string)replyParams["text"],
                    Tts = (string)replyParams["tts"],
                    EndSession = (bool)replyParams["isEnd"]
                },
                Session = Session
            };
        }
    }

    /// <summary>
    /// Тип ввода 
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        ///  голосовой ввод
        /// </summary>
        SimpleUtterance,
        /// <summary>
        /// нажатие кнопки
        /// </summary>
        ButtonPressed
    }
    /// <summary>
    /// Данные, полученные от пользователя.
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// Служебное поле: запрос пользователя, преобразованный для внутренней обработки Алисы. В ходе преобразования текст, в частности, очищается от знаков препинания, а числительные преобразуются в числа.
        /// </summary>
        [JsonProperty("command")]
        public string Command { get; set; }
        /// <summary>
        /// Тип ввода, обязательное свойство
        /// </summary>
        [JsonProperty("type")]
        public RequestType Type { get; set; }
        /// <summary>
        /// Полный текст пользовательского запроса, максимум 1024 символа.
        /// </summary>
        [JsonProperty("original_utterance")]
        public string OriginalUtterance { get; set; }
        /// <summary>
        /// JSON, полученный с нажатой кнопкой от обработчика навыка (в ответе на предыдущий запрос)
        /// </summary>
        [JsonProperty("payload")]
        public JObject Payload { get; set; }
    }
    /// <summary>
    /// Ответ на запрос Яндекс Диалогов
    /// </summary>
    public class AliceResponse
    {
        /// <summary>
        /// Данные для ответа пользователю.
        /// </summary>
        [JsonProperty("response")]
        public ResponseModel Response { get; set; }
        /// <summary>
        /// Данные о сессии.
        /// </summary>
        [JsonProperty("session")]
        public SessionModel Session { get; set; }
        /// <summary>
        /// Версия протокола. Текущая версия — 1.0.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; } = "1.0";
    }
}
