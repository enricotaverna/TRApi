using System.Data;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TRApi.Helpers;

namespace TRApi.Controllers
{
    public class UserController : ApiController
    {

        // GET: User/Details/5
        public HttpResponseMessage Get(string userName, string password)
        {
            var resp = new HttpResponseMessage();

            // utente attivo esiste
            DataRow rec = Database.GetRow("SELECT Persons_id FROM Persons WHERE active=1 AND mail = " + Database.FormatStringDb(userName));

            if (rec == null) 
                return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("Utente non trovato o non attivo") };

            // recupera lo user id corrispondente allo username passato dalla API
            rec = Database.GetRow("SELECT Persons_id FROM Persons WHERE active=1 AND mail = " + Database.FormatStringDb(userName) + " AND password=" + Database.FormatStringDb(password));

            if (rec == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("Password non riconosciuta") };

            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
