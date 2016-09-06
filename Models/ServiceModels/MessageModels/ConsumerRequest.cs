using System.Runtime.Serialization;

namespace Models.ServiceModels.MessageModels
{
    public class ConsumerRequest
    {
        [DataMember]
        public string QueueName { get; set; }
        [DataMember]
        public bool NoAck { get; set; } = true;
        [DataMember]
        public string ConsumerTag { get; set; }
    }
}
