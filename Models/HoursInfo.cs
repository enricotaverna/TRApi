using System;
using System.Runtime.Serialization;

namespace TRApi.Models
{
    [DataContract]
    public class HoursInfo
    {
        [DataMember(Name = "Hours_id")]
        public int Hours_id;

        [DataMember(Name = "Projects_id")]
        public int Projects_id;

        [DataMember(Name = "Activity_id")]
        public int Activity_id;

        [DataMember(Name = "Persons_id")]
        public int Persons_id;

        [DataMember(Name = "UserName")] // usato nella creazione
        public string UserName;

        [DataMember(Name = "RecordDate")] // renaming del campo Data
        public DateTime Date { get; set; }

        [DataMember(Name = "Hours")]
        public float Hours { get; set; }

        [DataMember(Name = "ProjectCode")]
        public string ProjectCode { get; set; }

        [DataMember(Name = "ProjectName")]
        public string NomeProgetto { get; set; }

        [DataMember(Name = "ProjectType_Id")]
        public string ProjectType_Id { get; set; }

        [DataMember(Name = "LocationDescription")]
        public string LocationDescription { get; set; }

        [DataMember(Name = "LocationKey")]
        public string LocationKey { get; set; }

        [DataMember(Name = "LocationType")]
        public string LocationType { get; set; }

        [DataMember(Name = "Comment")]
        public string Comment { get; set; }

        [DataMember(Name = "IconSrc")]
        public string IconSrc { get; set; }
    }
}