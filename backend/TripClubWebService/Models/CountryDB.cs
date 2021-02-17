using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class CountryDB
    {


        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


        private static string strConLocal = ConfigurationManager.ConnectionStrings["strConLocal"].ConnectionString;
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;



        static CountryDB()
        {
            if (local)
                _conStr = strConLocal;
            else
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }




        private static List<Country> ExecReader(string commandString)
        {
            List<Country> listToReturn = new List<Country>();
            try
            {



                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var COUNTRY_ID = (int)dataReader["COUNTRY_ID"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();

                    listToReturn.Add(new Country(COUNTRY_ID, DESCRIPTION));
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
        public static List<Country> GetAllCountries()
        {
            List<Country> listToReturn = ExecReader("SELECT * FROM COUNTRIES");

            return listToReturn;
        }




        //READ ONE BY ID
        public static Country GetCountryById(int id)
        {
            Country countryToReturn = ExecReader($"SELECT * FROM COUNTRIES WHERE COUNTRY_ID = {id} ").FirstOrDefault();
            return countryToReturn;
        }




        //INSERT COUNTRY
        public static int InsertNewCountry(string DESCRIPTION)
        {
            try
            {

                command.CommandText = $"INSERT INTO COUNTRIES (DESCRIPTION) VALUES(N'{DESCRIPTION}' ) ";

                con.Open();
                command.ExecuteNonQuery();

                int countryId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    countryId = int.Parse(dataReader["IDENTITY"].ToString());
                return countryId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //UPDATE COUNTRY
        public static int UpdateCountry(int COUNTRY_ID, string DESCRIPTION)
        {
            try
            {
                command.CommandText = $@"UPDATE COUNTRIES SET DESCRIPTION = N'{DESCRIPTION}' WHERE COUNTRY_ID = {COUNTRY_ID}";
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








        //DELETE COUNTRY
        public static int DeleteCountry(int COUNTRY_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM COUNTRIES WHERE COUNTRY_ID = {COUNTRY_ID}";
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