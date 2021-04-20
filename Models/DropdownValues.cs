using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace TRApi.Models
{

    public class ResponseType
    {
        public bool status { set; get; }
        public IEnumerable<object> data { set; get; }
    }

    [DataContract]
    public class LocationsInfo
    {
        [DataMember(Name = "ParentKey")]
        public string ParentKey;

        [DataMember(Name = "LocationKey")]
        public string LocationKey;

        [DataMember(Name = "LocationType")]
        public string LocationType; // P = Project C = Client

        [DataMember(Name = "LocationDescription")]
        public string LocationDescription;

    }

    [DataContract]
    public class ProjectsInfo
    {
        [DataMember(Name = "Projects_id")]
        public int Projects_id;

        [DataMember(Name = "Persons_id")]
        public int Persons_id;

        [DataMember(Name = "ProjectCode")]
        public string ProjectCode { get; set; }

        [DataMember(Name = "ProjectName")]
        public string ProjectName { get; set; }

        [DataMember(Name = "TestoObbligatorio")]
        public string TestoObbligatorio { get; set; }

        [DataMember(Name = "MessaggioDiErrore")]
        public string MessaggioDiErrore { get; set; }

        [DataMember(Name = "BloccoCaricoSpese")]
        public string BloccoCaricoSpese { get; set; }

        [DataMember(Name = "ActivityOn")]
        public string ActivityOn { get; set; }

        [DataMember(Name = "WorkflowType")]
        public string WorkflowType { get; set; }

        [DataMember(Name = "ProjectType_Id")]
        public string ProjectType_Id { get; set; }

        [DataMember(Name = "CodiceCliente")]
        public string CodiceCliente { get; set; }

        [DataMember(Name = "LocationFilter")]
        public string LocationFilter { get; set; }

    }

    [DataContract]
    public class ActivitiesInfo
    {
        [DataMember(Name = "Activity_id")]
        public int Activity_id;

        [DataMember(Name = "ActivityCode")]
        public string ActivityCode;

        [DataMember(Name = "ActivityName")]
        public string ActivityName;

        [DataMember(Name = "Projects_id")]
        public int Projects_id;

    }
}