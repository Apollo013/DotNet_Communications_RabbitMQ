using System.Runtime.Serialization;

namespace Models.ServiceModels.MessageModels
{
    public class QualityOfService
    {
        [DataMember]
        public ushort PrefetchSize { get; set; }
        [DataMember]
        public ushort PrefetchCount { get; set; }
        [DataMember]
        public bool Global { get; set; }
    }
}
