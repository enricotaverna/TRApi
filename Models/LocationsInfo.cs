using System.Runtime.Serialization;

namespace TRApi.Models
{
    [DataContract]
    public class LocationsInfo
    {
        [DataMember(Name = "ParentKey")]
        public string ParentKey;

        [DataMember(Name = "LocationKey")]
        public int LocationKey;

        [DataMember(Name = "LocationDescription")]
        public string LocationDescription { get; set; }

    }
}