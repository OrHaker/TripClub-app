using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class DestinationDB
    {


        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


        private static string strConLocal = ConfigurationManager.ConnectionStrings["strConLocal"].ConnectionString;
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;



        static DestinationDB()
        {
            if (local)
                _conStr = strConLocal;
            else
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }




        private static List<Destination> ExecReader(string commandString)
        {
            try
            {

                List<Destination> listToReturn = new List<Destination>();



                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var DESTINATION_CODE = (int)dataReader["DESTINATION_CODE"];
                    var COUNTRY_ID = (int)dataReader["COUNTRY_ID"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    listToReturn.Add(new Destination(DESTINATION_CODE, COUNTRY_ID, DESCRIPTION));
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }




        //READ ALL
        public static List<Destination> GetAllDestinations()
        {
            List<Destination> listToReturn = ExecReader("SELECT * FROM DESTINATIONS ");

            return listToReturn;
        }




        //READ ONE BY ID
        public static Destination GetDestinationId(int DESTINATION_CODE)
        {
            Destination countryToReturn = ExecReader($"SELECT * FROM DESTINATIONS " +
                          $" WHERE DESTINATION_CODE = {DESTINATION_CODE} ").FirstOrDefault();
            return countryToReturn;
        }




        //INSERT DESTINATION
        public static int InsertNewDestination(int COUNTRY_ID, string DESCRIPTION)
        {
            try
            {

                command.CommandText = $"INSERT INTO DESTINATIONS " +
                                  $" (COUNTRY_ID,DESCRIPTION) VALUES({COUNTRY_ID},N'{DESCRIPTION}' ) ";

                con.Open();
                command.ExecuteNonQuery();
                int destinationId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    destinationId = int.Parse(dataReader["IDENTITY"].ToString());
                return destinationId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //UPDATE DESTINATION
        public static int UpdateDestination(int DESTINATION_CODE, int COUNTRY_ID, string DESCRIPTION)
        {
            try
            {
                command.CommandText = $@"UPDATE DESTINATIONS SET DESCRIPTION = N'{DESCRIPTION}' , COUNTRY_ID = {COUNTRY_ID}  WHERE DESTINATION_CODE = {DESTINATION_CODE}";
                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }








        //DELETE DESTINATION
        public static int DeleteDestination(int DESTINATION_CODE)
        {
            try
            {

                command.CommandText = $@"DELETE FROM DESTINATIONS WHERE DESTINATION_CODE = {DESTINATION_CODE}";

                con.Open();
                int rowsAffected = command.ExecuteNonQuery();

                return rowsAffected;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }



    }
}