using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace TRApi.Helpers
{

    public class Database
    {
        /// Converte una stringa in formato stringa compatibile con i DB
        public static string FormatStringDb(string InputString)
        {
            string sRet = "'" + InputString.Replace("'", "''") + "'";
            return sRet;
        }

        /// Converte una stringa in formato data compatibile con i DB
        public static string FormatDateDb(DateTime dateToConvert, bool timestamp = false)
        {
            string ret = "";

            if (dateToConvert == null) 
                return null;

            string server = ConfigurationManager.AppSettings["FORMATODATA"];

            switch (server)
            {
                case "US":
                    {
                        ret = "'" + dateToConvert.Month + "-" + dateToConvert.Day + "-" + dateToConvert.Year;
                        break;
                    }

                case "IT":
                    {
                        ret = "'" + dateToConvert.Day + "-" + dateToConvert.Month + "-" + dateToConvert.Year;
                        break;
                    }
            }

            // se richiamato con parametro opzionale timestamp aggiunge ore:min:sec
            if (timestamp)
                ret = ret + " " + dateToConvert.Hour + ":" + dateToConvert.Minute + ":" + dateToConvert.Second + "'";
            else
                ret = ret + "'";

            return ret;
        }

        /// Converte numero in formato data compatibile con i DB
        public static string FormatNumberDB(double InputNumber)
        {
            string sRet = "'" + InputNumber.ToString().Replace(",", ".") + "'";
            return sRet;
        }

        /// Ritorna datatable dopo aver interrogato il DB
        public static DataTable GetData(string cmdText)
        {
            DataTable dt = new DataTable();

            using (SqlConnection lCon = new SqlConnection(Constants.sDefaultConnectionString))

            {
                using (SqlCommand cmd = lCon.CreateCommand())
                {
                    lCon.Open();
                    using (var sda = new SqlDataAdapter(cmd))
                    {
                        try
                        {
                            cmd.CommandText = cmdText;
                            cmd.CommandType = CommandType.Text;
                            sda.Fill(dt);
                        }
                        catch // Exception ex non usato
                        {
                            dt = null; // errore
                        }
                    }
                }
            }

            return dt;
        }

        /// Torna un singolo record di tipo datarow
        public static DataRow GetRow(string cmdText)
        {
            DataTable dtTable = Database.GetData(cmdText);
            DataRow drRet;

            if ((dtTable.Rows.Count > 0))
                drRet = dtTable.Rows[0];
            else
                drRet = null /* TODO Change to default(_) if this is not a reference type */;

            return drRet;
        }

        /// Esegue comando SQL
        public static bool ExecuteSQL(string cmdText)
        {
            using (SqlConnection connection = new SqlConnection(Constants.sDefaultConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(cmdText, connection))
                {
                    try
                    {
                        connection.Open(); // Not necessarily needed In this Case because DataAdapter.Fill does it otherwise 
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

    }
}