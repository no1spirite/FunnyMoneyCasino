using System.Collections.Generic;
using System.Runtime.Serialization;

namespace BlackJackSL.Web.BlackJack.JSON
{
    [DataContract]
    public class SampleDataObject
    {
        public SampleDataObject(int NewId, string NewName)
        {
            Id = NewId;
            Name = NewName;
        }
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}