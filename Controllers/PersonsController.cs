using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;
using TRApi.Models;
using TRApi.Helpers;
using System.Collections.Generic;

namespace TRApi.Controllers
{

    [RoutePrefix("TRApi/persons")]
    public class PersonsController : ApiController
    {
        // GET: api/Persons

        // GET: api/Persons/xxxx/Projects
        public class ReturnProjectObject
        {
            public IEnumerable<TRApi.Models.ProjectsInfo> data { get; set; }
        }
        public ReturnProjectObject Get(string username)
        {
            string selectString;
            var ret = new ReturnProjectObject();

            // recupera flag progetti forzati
            DataRow dr = Database.GetRow("Select persons_id, forcedaccount from Persons where userid=" + Database.FormatStringDb(username));

            if (dr == null)
                 return null; // errore

            if (dr["forcedaccount"].ToString() == "True")
                selectString = "SELECT DISTINCT v_Projects.Projects_Id, ProjectCode, ProjectCode + ' ' + left(ProjectName, 20) AS ProjectName, TestoObbligatorio, COALESCE(MessaggioDiErrore,'') as MessaggioDiErrore, BloccoCaricoSpese, ActivityOn, WorkflowType, ProjectType_Id, CodiceCliente  FROM ForcedAccounts RIGHT JOIN v_Projects ON ForcedAccounts.Projects_id = v_Projects.Projects_Id WHERE((ForcedAccounts.Persons_id = " + dr["persons_id"].ToString() + " OR v_Projects.Always_available = 1) AND v_Projects.active = 1 )  ORDER BY v_Projects.ProjectCode";
            else
                selectString = "SELECT DISTINCT Projects_Id, ProjectCode, ProjectCode + ' ' + left(ProjectName,20) AS ProjectName, TestoObbligatorio, COALESCE(MessaggioDiErrore,'') as MessaggioDiErrore, BloccoCaricoSpese, ActivityOn, WorkflowType, ProjectType_Id, CodiceCliente  FROM v_Projects WHERE active = 1 ORDER BY ProjectCode";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSql12155"].ConnectionString))
            {
                var lForcedProjects = db.Query<ProjectsInfo>(selectString);

                foreach ( ProjectsInfo i in lForcedProjects )
                {

                    i.LocationFilter = "";
                    // se chargable filtro per cliente, altrimenti per progetto
                    if (i.ProjectType_Id == ConfigurationManager.AppSettings["PROGETTO_CHARGEABLE"])
                         i.LocationFilter = i.CodiceCliente.ToString().TrimEnd();
                    else if (i.ProjectType_Id == ConfigurationManager.AppSettings["PROGETTO_BUSINESS_DEVELOPMENT"] ||
                             i.ProjectType_Id == ConfigurationManager.AppSettings["PROGETTO_INTERNAL_INVESTMENT"] ||
                             i.ProjectType_Id == ConfigurationManager.AppSettings["PROGETTO_INFRASTRUCTURE"])
                        i.LocationFilter = i.Projects_id.ToString();
                }

                ret.data = lForcedProjects;
                return ret;
            }
        }


    }
}
