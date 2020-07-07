using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace MaskService
{
    // Определение методов, которые предоставляет сервис
    [ServiceContract]
    public interface IService1
    {
        // Метод получения списка всех масок в формате JSON
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/usermasks/",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<UserDetails> GetUserMasks();

        // Метод получения списка логов по ID пользователя в формате JSON
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/logs/{UserID}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        List<UserDetails> GetLogs(string UserID);

        // Метод удаления маски по ID юзера
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/delete/{UserID}")]
        string DeleteMask(string UserID);

        // Метод обновления маски юзера по его ID, а также занесения старой маски в лог
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "/update/{UserID}_{MaskUserID}_{newValue}")]
        string UpdateMask(string UserID, string MaskUserID, string newValue);
    }

    // Определение данных, которые содержит сервис
    [DataContract]
    public class UserDetails
    {
        // Определение id пользователя, его имени и маски 
        string userID = string.Empty;
        string userName = string.Empty;
        string maskUserID = string.Empty;

        // Определение геттеров и сеттеров для всех свойств сервиса
        [DataMember]
        public string UserID
        {
            get { return userID; }
            set { userID = value; }
        }
        [DataMember]
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        [DataMember]
        public string MaskUserID
        {
            get { return maskUserID; }
            set { maskUserID = value; }
        }
    }
}