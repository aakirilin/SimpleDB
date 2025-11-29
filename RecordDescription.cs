using System.Runtime.Serialization;

namespace SimpleDB
{
    [DataContract]
    public struct RecordDescription
    {
        [DataMember]
        public CString10 Name;
        [DataMember]
        public int Length;
        [DataMember]
        public RecordTypes Type;
    }
}
