using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class RecommendationDB
    {
        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


        private static string strConLocal = ConfigurationManager.ConnectionStrings["strConLocal"].ConnectionString;
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;



        static RecommendationDB()
        {
            if (local)
                _conStr = strConLocal;
            else
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }




        private static List<Recommendation> ExecReader(string commandString)
        {
            try
            {
                List<Recommendation> listToReturn = new List<Recommendation>();

                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var RECO_ID = (int)dataReader["RECO_ID"];
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DESTINATION_CODE = (int)dataReader["DESTINATION_CODE"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var POST_IMAGE = (dataReader["POST_IMAGE"] != DBNull.Value ? dataReader["POST_IMAGE"] : "").ToString();
                    var UPLOAD_DATE = dataReader["UPLOAD_DATE"].ToString();

                    listToReturn.Add(
                        new Recommendation(RECO_ID, USER_ID, DESTINATION_CODE, DESCRIPTION, POST_IMAGE, UPLOAD_DATE)
                        );
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        private static void ExecProcReader(List<ExtendedRecommendation> listToReturn, SqlCommand procedureComm)
        {
            using (SqlDataReader dataReader = procedureComm.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var RECO_ID = (int)dataReader["RECO_ID"];
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DESTINATION_CODE = (int)dataReader["DESTINATION_CODE"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var POST_IMAGE = (dataReader["POST_IMAGE"] != DBNull.Value ? dataReader["POST_IMAGE"] : "").ToString();
                    var UPLOAD_DATE = dataReader["UPLOAD_DATE"].ToString();
                    var r = new Recommendation(RECO_ID, USER_ID, DESTINATION_CODE, DESCRIPTION, POST_IMAGE, UPLOAD_DATE);


                    var USER_NAME = dataReader["USER_NAME"].ToString();
                    var EMAIL = dataReader["EMAIL"].ToString();
                    var FIRST_NAME = dataReader["FIRST_NAME"].ToString();
                    var LAST_NAME = dataReader["LAST_NAME"].ToString();
                    var USER_IMAGE = dataReader["USER_IMAGE"].ToString();
                    var DESTINATION_NAME = dataReader["DESTINATION_NAME"].ToString();
                    var COUNTRY_ID = dataReader["COUNTRY_ID"].ToString();
                    var COUNTRY_NAME = dataReader["COUNTRY_NAME"].ToString();
                    listToReturn.Add(
                                    new ExtendedRecommendation(DESTINATION_NAME, EMAIL, USER_NAME, FIRST_NAME, LAST_NAME, USER_IMAGE, COUNTRY_ID, COUNTRY_NAME, r)
                                    );
                }
            }
        }



        //READ RECOMMENDATIONS INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedRecommendation> GetAllFullRecommendations()
        {
            try
            {
                List<ExtendedRecommendation> listToReturn = new List<ExtendedRecommendation>();
                SqlCommand procedureComm = new SqlCommand($"ALL_RECOMMENDATIONS", con);

                procedureComm.CommandType = CommandType.StoredProcedure;

                con.Open();
                ExecProcReader(listToReturn, procedureComm);
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //READ RECOMMENDATIONS BY DESTINATION CODE INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedRecommendation> GetAllFullRecommendationsById(int DESTINATION_CODE_OUT)
        {
            try
            {
                List<ExtendedRecommendation> listToReturn = new List<ExtendedRecommendation>();
                SqlCommand procedureComm = new SqlCommand($"RECOMMENDATIONS_BY_DESTINATION", con);

                procedureComm.CommandType = CommandType.StoredProcedure;

                procedureComm.Parameters.Add(new SqlParameter("@DESTINATION_CODE_OUT", DESTINATION_CODE_OUT));

                con.Open();
                ExecProcReader(listToReturn, procedureComm);
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }




        //READ ALL RECOMMENDATIONS
        public static List<Recommendation> GetAllRecommendations()
        {
            List<Recommendation> listToReturn = ExecReader("SELECT * FROM RECOMMENDATIONS");

            return listToReturn;
        }




        //READ ONE RECOMMENDATION BY ID
        public static Recommendation GetRecommendationById(int RECO_ID)
        {
            Recommendation countryToReturn = ExecReader($"SELECT * FROM RECOMMENDATIONS   WHERE RECO_ID = {RECO_ID} ").FirstOrDefault();
            return countryToReturn;
        }




        //INSERT RECOMMENDATION
        public static int InsertNewRecommendation(Recommendation newRecommendation)
        {
            try
            {
                command.CommandText = $"INSERT INTO RECOMMENDATIONS " +
                                  $" (USER_ID,DESTINATION_CODE,DESCRIPTION,POST_IMAGE,UPLOAD_DATE) " +
                                  $" VALUES({newRecommendation.UserId} ," +
                                  $" {newRecommendation.DestinationCode} , N'{newRecommendation.Description}' " +
                                  $", N'{newRecommendation.PostImage}' , '{newRecommendation.UploadDate}' ) ";
                con.Open();
                command.ExecuteNonQuery();
                int recommendationId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    recommendationId = int.Parse(dataReader["IDENTITY"].ToString());
                return recommendationId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //UPDATE RECOMMENDATION
        public static int UpdateRecommendation(Recommendation recommendation)
        {
            try
            {
                command.CommandText = $"UPDATE RECOMMENDATIONS " +
                    $" SET USER_ID = {recommendation.UserId} , " +
                    $" DESTINATION_CODE = {recommendation.DestinationCode} ," +
                    $" DESCRIPTION = N'{recommendation.Description}' , " +
                    $" POST_IMAGE = N'{recommendation.PostImage}' ," +
                    $" UPLOAD_DATE = '{recommendation.UploadDate}' " +
                    $" WHERE RECO_ID = {recommendation.RecoId}";
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








        //DELETE RECOMMENDATION
        public static int DeleteRecommendation(int RECO_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM RECOMMENDATIONS WHERE RECO_ID = {RECO_ID}";
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