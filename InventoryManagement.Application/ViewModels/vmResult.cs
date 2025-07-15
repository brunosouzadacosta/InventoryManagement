using System.Runtime.Serialization;

namespace InventoryManagement.Controllers
{
    [Serializable]
    [DataContract]
    public class vmResult
    {
        [DataMember]
        public object Data { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Mensagem { get; set; }
        [DataMember]
        public string FriendlyErrorMessage { get; set; }
        [DataMember]
        public string StackTrace { get; set; }
        public bool Success { get; set; }
        public string MensagemErroTratado { get; set; }
        public string retorno { get; set; }
    }
}