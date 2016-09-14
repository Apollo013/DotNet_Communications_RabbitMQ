using System.Runtime.Serialization;

namespace Models.ServiceModels.MessageModels
{
    [DataContract]
    public class ConsumerRequest
    {
        [DataMember]
        public string QueueName { get; set; }
        [DataMember]
        public bool NoAck { get; set; } = false;
        [DataMember]
        public string ConsumerTag { get; set; }

        private QualityOfService _qualityOfService;
        [DataMember]
        public QualityOfService QualityOfService
        {
            get
            {
                if (_qualityOfService == null)
                {
                    _qualityOfService = new QualityOfService();
                }
                return _qualityOfService;
            }
            set { _qualityOfService = value; }
        }

        public ConsumerRequest()
        { }

        public ConsumerRequest(string queueName)
        {
            this.QueueName = queueName;
        }
    }
}
