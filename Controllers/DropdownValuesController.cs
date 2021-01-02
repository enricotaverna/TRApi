using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TRApi.Helpers;
using TRApi.Models;

namespace TRApi.Controllers
{

    public class DropdownValuesController : ApiController
    {
        // Lista progetti autorizzati.
        public ResponseType GetProjects(string userName)
        {
            string selectString;
            var ret = new ResponseType();

            // recupera flag progetti forzati
            DataRow dr = Database.GetRow("Select persons_id, forcedaccount from Persons where userid=" + Database.FormatStringDb(userName));

            if (dr == null)
                return null; // errore

            if (dr["forcedaccount"].ToString() == "True")
                selectString = "SELECT DISTINCT v_Projects.Projects_Id, ProjectCode, ProjectCode + ' ' + left(ProjectName, 20) AS ProjectName, TestoObbligatorio, COALESCE(MessaggioDiErrore,'') as MessaggioDiErrore, BloccoCaricoSpese, ActivityOn, WorkflowType, ProjectType_Id, CodiceCliente   FROM ForcedAccounts RIGHT JOIN v_Projects ON ForcedAccounts.Projects_id = v_Projects.Projects_Id WHERE((ForcedAccounts.Persons_id = " + dr["persons_id"].ToString() + " OR v_Projects.Always_available = 1) AND v_Projects.active = 1 )  ORDER BY v_Projects.ProjectCode";
            else
                selectString = "SELECT DISTINCT Projects_Id, ProjectCode, ProjectCode + ' ' + left(ProjectName,20) AS ProjectName, TestoObbligatorio, COALESCE(MessaggioDiErrore,'') as MessaggioDiErrore, BloccoCaricoSpese, ActivityOn, WorkflowType, ProjectType_Id, CodiceCliente   FROM v_Projects WHERE active = 1 ORDER BY ProjectCode";

            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSql12155"].ConnectionString))
            {
                var lForcedProjects = db.Query<ProjectsInfo>(selectString);

                foreach (ProjectsInfo i in lForcedProjects)
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

                var oList = lForcedProjects.ToList();
                //oList.Add(oRecordDefault);

                // prepara risposta
                ret.status = true;
                ret.data = oList;
                return ret;
            }
        }

        // Lista Lista locations
        public ResponseType GetLocations(string userName)
        {

            DataTable dt = new DataTable();
            List<LocationsInfo> LocationList = new List<LocationsInfo>();
            var ret = new ResponseType();

            // Carica Location
            dt = Database.GetData("select * from LOC_ClientLocation");

            // carica Dictionary
            foreach (DataRow dr in dt.Rows)
            {
                LocationsInfo item = new LocationsInfo();
                item.ParentKey = dr["CodiceCliente"].ToString().TrimEnd();
                item.LocationKey = dr["ClientLocation_id"].ToString().TrimEnd();
                item.LocationDescription = dr["LocationDescription"].ToString();
                item.LocationType = "C"; // customer, usato su input.aspx.cs
                LocationList.Add(item);
            }

            // Carica Location
            dt = Database.GetData("select * from LOC_ProjectLocation");

            // carica Dictionary
            foreach (DataRow dr in dt.Rows)
            {
                LocationsInfo item = new LocationsInfo();
                item.ParentKey = dr["Projects_id"].ToString();
                item.LocationKey = dr["ProjectLocation_id"].ToString().TrimEnd();
                item.LocationDescription = dr["LocationDescription"].ToString();
                item.LocationType = "P"; // Project, usato su input.aspx.cs
                LocationList.Add(item);
            }

            // prepara risposta
            ret.status = true;
            ret.data = LocationList;
            return ret;

        }

        // Lista Lista locations
        public ResponseType GetActivities(string userName)
        {

            DataTable dt = new DataTable();
            List<ActivitiesInfo> ActivitiesList = new List<ActivitiesInfo>();
            var ret = new ResponseType();

            // Carica Location
            dt = Database.GetData("SELECT Activity_id, ActivityCode, a.Name as ActivityName, a.Projects_id FROM Activity as a INNER JOIN Projects as b ON b.Projects_id = a.Projects_id WHERE a.active= 1 AND b.active = 1");

            // carica Dictionary
            foreach (DataRow dr in dt.Rows)
            {
                ActivitiesInfo item = new ActivitiesInfo();
                item.Activity_id = Convert.ToInt32(dr["Activity_id"].ToString());
                item.ActivityCode = dr["ActivityCode"].ToString();
                item.ActivityName = dr["ActivityName"].ToString();
                item.Projects_id = Convert.ToInt32(dr["Projects_id"].ToString());
                ActivitiesList.Add(item);
            }

            // prepara risposta
            if ( ActivitiesList.Count > 0) { 
                ret.status = true;
                ret.data = ActivitiesList;
            }
            else
            {
                ret.status = false;
                ret.data = null;
            }

            return ret;

        }


    }

}
