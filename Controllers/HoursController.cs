using Dapper;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Http.Cors;
using TRApi.Models;
using TRApi.Helpers;
using System.Net;
using System.Net.Http;
using System;
using System.Web.Mvc;

namespace TRApi.Controllers
{

    [System.Web.Http.Authorize]
    [RequireHttps]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HoursController : ApiController
    {

        // GET: api/Hours
        public object Get(string userName)
        {

            // estrare le ore nel mese corrente per l'utente passato dalla API
            DateTime periodEndDate = DateTime.Parse(Utility.GetCutoffDate("start"));

            // recupera lo user id corrispondente allo username passato dalla API
            DataRow rec = Database.GetRow("SELECT Persons_id FROM Persons WHERE userId = " + Database.FormatStringDb(userName));
            if (rec == null) return null;

            using ( IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSql12155"].ConnectionString))
            {
                string SQLcommand = "Select Hours_id, Projects_id, Activity_id, Persons_id, Date, Hours, ProjectCode, NomeProgetto, ProjectType_Id, LocationDescription, LocationKey, LocationType,  Comment " +
                                    " From v_ore " +
                                    " WHERE Persons_id = " + Database.FormatStringDb(rec["Persons_id"].ToString()) +
                                    " AND Date >= " + Database.FormatDateDb(periodEndDate) +
                                    " ORDER BY Date DESC";

                var Hours = db.Query<HoursInfo>(SQLcommand);

                // setta icona
                foreach (HoursInfo i in Hours)
                {
                    switch (i.ProjectType_Id) { 
                    
                        case "5":
                            i.IconSrc = "sap-icon://person-placeholder"; // Sick & Ferie
                            break;

                        default:
                            i.IconSrc = "sap-icon://fa-regular/clock"; // Chargable, internal investment, infrastructure
                        break;
                    }
                }

                return Hours;
            }
        }

        // GET: api/Hours/5
        public object Get(int id, string userName)
        {
            using (IDbConnection db = new SqlConnection(ConfigurationManager.ConnectionStrings["MSSql12155"].ConnectionString))
            {
                string SQLcommand = "Select Hours_id, Projects_id, Activity_id, Persons_id, Date, Hours, ProjectCode, NomeProgetto, LocationDescription, LocationKey, LocationType, Comment " +
                                    " From v_ore " +
                                    " WHERE hours_id = '" + id.ToString() + "'";

                var Hours = db.Query<HoursInfo>(SQLcommand);
                return Hours;
            }

        }

        // POST: api/Hours
        public HoursInfo Post([FromBody]HoursInfo data)
        {

            var resp = new HttpResponseMessage();

            // recupera lo user id corrispondente allo username passato dalla API
            DataRow rec = Database.GetRow("SELECT Persons_id FROM Persons WHERE userId = " + Database.FormatStringDb(data.UserName));

            if (rec == null)
            {
                resp.StatusCode = HttpStatusCode.BadRequest;
                resp.ReasonPhrase = "Persons_id non trovato";
                return (null);
            }

            // recupera società e manager
            DataRow drPerson = Database.GetRow("SELECT company_id FROM Persons WHERE Persons_id = " + Database.FormatStringDb(rec["Persons_id"].ToString()));
            // recupera anagrafica progetto
            DataRow drProject = Database.GetRow("SELECT ClientManager_id, AccountManager_id  FROM Projects WHERE Projects_id = " + Database.FormatNumberDB(data.Projects_id));

            // valida commento
            var sComment = data.Comment == null ? "" : data.Comment.ToString();

            string sCmd = "";
            if (data.Hours_id == 0)
                sCmd = "INSERT INTO Hours (Persons_id, Projects_id, Activity_id, Date, Hours, HourType_Id, LocationDescription, LocationType, LocationKey, Company_id, ClientManager_id, AccountManager_id, Comment, CreationDate, CreatedBy) " +
                                "VALUES (" +
                                 Database.FormatStringDb(rec["Persons_id"].ToString()) + "," +
                                 Database.FormatStringDb(data.Projects_id.ToString()) + "," +
                                 Database.FormatStringDb(data.Activity_id.ToString()) + "," +
                                 Database.FormatDateDb(data.Date) + "," +
                                 Database.FormatStringDb(data.Hours.ToString()) + "," +
                                 "'1'," +
                                 Database.FormatStringDb(data.LocationDescription.ToString()) + "," +
                                 Database.FormatStringDb(data.LocationType.ToString()) + "," +
                                 Database.FormatStringDb(data.LocationKey.ToString()) + "," +
                                 Database.FormatNumberDB(Convert.ToInt32(drPerson["Company_id"])) + "," +
                                 Database.FormatNumberDB(Convert.ToInt32(drProject["AccountManager_id"])) + "," +
                                 Database.FormatNumberDB(Convert.ToInt32(drProject["ClientManager_id"])) + "," +
                                 Database.FormatStringDb(sComment) + "," +
                                 Database.FormatDateDb(DateTime.Now, true) + "," + // true Timestamp
                                 Database.FormatStringDb(data.UserName) +
                                 ")";
            else
                sCmd = "UPDATE Hours SET Projects_id = " + Database.FormatStringDb(data.Projects_id.ToString()) +
                              ", Activity_id = " + Database.FormatStringDb(data.Activity_id.ToString()) +
                              ", Date = " + Database.FormatDateDb(data.Date) +
                              ", Hours = " + Database.FormatStringDb(data.Hours.ToString()) +
                              ", LocationDescription = " + Database.FormatStringDb(data.LocationDescription.ToString()) +
                              ", LocationType = " + Database.FormatStringDb(data.LocationType.ToString()) +
                              ", LocationKey = " + Database.FormatStringDb(data.LocationKey.ToString()) +
                              ", Comment=" + Database.FormatStringDb(sComment) +
                              ", LastModificationDate=" + Database.FormatDateDb(DateTime.Now, true) + // true Timestamp
                              ", LastModifiedBy=" + Database.FormatStringDb(data.UserName) +
                              " WHERE Hours_id=" + Database.FormatStringDb(data.Hours_id.ToString());

            bool ret = Database.ExecuteSQL(sCmd);

            if (ret)
                return data;
            else {
                resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                resp.ReasonPhrase = "Errore in INSERT/UPDATE Hours";
                throw new HttpResponseException(resp);
            }
        }

        // PUT: api/Hours/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Hours/5
        public string Delete(int id)
        {

            var resp = new HttpResponseMessage();
            string sCmd = "";

            if (id != 0)
                sCmd = "DELETE FROM Hours WHERE Hours_id=" + Database.FormatNumberDB(id);

            bool ret = Database.ExecuteSQL(sCmd);

            if (ret)
                return DateTime.Now.ToString("dd/MM/yyyy HH.mm.ss"); // NO CONTENT
            else
            {
                resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                resp.ReasonPhrase = "Errore in DELETE Hours";
                throw new HttpResponseException(resp);
            }


        }
    }
}
