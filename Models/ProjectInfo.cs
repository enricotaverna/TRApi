using System.Runtime.Serialization;

namespace TRApi.Models
{
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
}