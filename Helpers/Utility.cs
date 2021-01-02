using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TRApi.Helpers
{
    public class Utility
    {

        /// <summary>
        /// Torna la data di cutoff
        /// </summary>
        /// <param periodType="">end = fine periodo, start = inizio periodo</param>
        /// <returns></returns>        
        public static string GetCutoffDate(string periodType)
        {
            string ret;

            DataRow dr = Database.GetRow("SELECT TOP 1 cutoffPeriod, cutoffMonth, cutoffYear FROM Options"); // contiene un unico record

            // calc the cutoff date based on the input parameter
            if (dr[0].ToString() == "1")
            {
                if (periodType == "end")
                    ret = "15/" + dr[1].ToString() + "/" + dr[2].ToString();
                else
                    ret = "1/" + dr[1].ToString() + "/" + dr[2].ToString();
            }
            else if (periodType == "end")
                ret = (DateTime.DaysInMonth(Convert.ToInt32(dr[2].ToString()), Convert.ToInt32(dr[1].ToString()))).ToString() + "/" + dr[1].ToString() + "/" + dr[2].ToString();
            else
                ret = "16" + "/" + dr[1].ToString() + "/" + dr[2].ToString();

            return ret;
        }

    }
}