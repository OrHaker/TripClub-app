using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class TrackDB
    {
        //DB SCHEMA
        // TRACK_CODE
        // USER_ID
        // DESCRIPTION
        // ACTIVE
        // TRACK_LOACTIONS


        //DB EXTEND SCHEMA
        //TRACK_CODE
        //USER_ID
        //DESCRIPTION
        //ACTIVE
        //TRACK_LOACTIONS
        //USER_NAME
        //EMAIL
        //FIRST_NAME
        //LAST_NAME
        //USER_IMAGE



        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


        private static string strConLocal = ConfigurationManager.ConnectionStrings["strConLocal"].ConnectionString;
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;



        static TrackDB()
        {
            if (local)
                _conStr = strConLocal;
            else
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }




        private static List<Track> ExecReader(string commandString)
        {

            try
            {
                List<Track> listToReturn = new List<Track>();



                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var TRACK_CODE = (int)dataReader["TRACK_CODE"];
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var ACTIVE = (bool)(dataReader["ACTIVE"] != DBNull.Value ? dataReader["ACTIVE"] : false);
                    var TRACK_LOACTIONS = dataReader["TRACK_LOACTIONS"].ToString();

                    listToReturn.Add(new Track(TRACK_CODE, USER_ID, DESCRIPTION, ACTIVE, TRACK_LOACTIONS));
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        private static void ExecProcReader(List<ExtendedTrack> listToReturn, SqlCommand procedureComm)
        {
            using (SqlDataReader dataReader = procedureComm.ExecuteReader())
            {
                while (dataReader.Read())
                {
                    var TRACK_CODE = (int)dataReader["TRACK_CODE"];
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var ACTIVE = (bool)dataReader["ACTIVE"];
                    var TRACK_LOACTIONS = (dataReader["TRACK_LOACTIONS"]).ToString();

                    var t = new Track(TRACK_CODE, USER_ID, DESCRIPTION, ACTIVE, TRACK_LOACTIONS);


                    var USER_NAME = dataReader["USER_NAME"].ToString();
                    var EMAIL = dataReader["EMAIL"].ToString();
                    var FIRST_NAME = dataReader["FIRST_NAME"].ToString();
                    var LAST_NAME = dataReader["LAST_NAME"].ToString();
                    var USER_IMAGE = dataReader["USER_IMAGE"].ToString();

                    listToReturn.Add(new ExtendedTrack(USER_NAME, EMAIL, FIRST_NAME, LAST_NAME, USER_IMAGE, t));
                }
            }
        }




        public static List<Object> GetAllTracksLocationsData()
        {
            try
            {
                List<Object> listToReturn = new List<Object>();
                con.Open();
                command.CommandText = "SELECT * FROM LAT_LANG";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    var DESTINATION_CODE = int.Parse(dataReader["DESTINATION_CODE"].ToString());
                    var LATITUDE = (double)dataReader["LATITUDE"];
                    var LONGITUDE = (double)dataReader["LONGITUDE"];
                    var locationName = dataReader["locationName"].ToString();
                    listToReturn.Add(new { locationName, longitude=LONGITUDE, latitude=LATITUDE, destinationCode= DESTINATION_CODE }); 
                }
                con.Close();
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        //READ TRACKS INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedTrack> GetAllFullTracks()
        {
            try
            {
                List<ExtendedTrack> listToReturn = new List<ExtendedTrack>();
                SqlCommand procedureComm = new SqlCommand($"ALL_TRACKS", con);

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


        //READ TRACKS BY DESTINATION INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedTrack> GetAllFullTracksByDestination(int DESTINATION_OUT)
        {
            try
            {
                List<ExtendedTrack> listToReturn = new List<ExtendedTrack>();
                SqlCommand procedureComm = new SqlCommand("ALL_TRACKS_BY_DESTINATION", con);

                procedureComm.CommandType = CommandType.StoredProcedure;

                procedureComm.Parameters.Add(new SqlParameter("@DESTINATION_OUT", DESTINATION_OUT));
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

        //READ TRACKS BY USER ID INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedTrack> GetAllFullTracksByUser(int USER_ID_OUT)
        {
            try
            {
                List<ExtendedTrack> listToReturn = new List<ExtendedTrack>();
                SqlCommand procedureComm = new SqlCommand("ALL_TRACKS_BY_USER", con);

                procedureComm.CommandType = CommandType.StoredProcedure;

                procedureComm.Parameters.Add(new SqlParameter("@USER_ID_OUT", USER_ID_OUT));
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



        //READ ALL TRACKS
        public static List<Track> GetAllTracks()
        {
            List<Track> listToReturn = ExecReader("SELECT * FROM TRACKS");

            return listToReturn;
        }




        //READ ONE TRACK BY ID
        public static Track GetTrackById(int TRACK_CODE)
        {
            Track countryToReturn = ExecReader($"SELECT * FROM TRACKS   WHERE TRACK_CODE = {TRACK_CODE} ").FirstOrDefault();
            return countryToReturn;
        }




        //INSERT TRACK
        public static int InsertNewTrack(Track newTrack)
        {
            try
            {
                int localActive = newTrack.Active ? 1 : 0;

                command.CommandText = $"INSERT INTO TRACKS " +
                                  $" (USER_ID, DESCRIPTION , ACTIVE ,TRACK_LOACTIONS) " +
                                  $" VALUES({newTrack.UserId} , '{newTrack.Description}' ,{localActive} ,'{newTrack.TrackLocations}' ) ";

                con.Open();
                command.ExecuteNonQuery();
                int trackId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    trackId = int.Parse(dataReader["IDENTITY"].ToString());
                return trackId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //UPDATE TRACK
        public static int UpdateTrack(Track track)
        {
            try
            {

                int localActive = track.Active ? 1 : 0;
                command.CommandText = $"UPDATE TRACKS " +
                    $" SET USER_ID = {track.UserId} , " +
                    $" DESCRIPTION = N'{track.Description}' , " +
                    $" ACTIVE = '{localActive}' ," +
                    $" TRACK_LOACTIONS = N'{track.TrackLocations}' " +
                    $" WHERE TRACK_CODE = {track.TrackCode}";

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








        //DELETE TRACK
        public static int DeleteTrack(int TRACK_CODE)
        {
            try
            {
                command.CommandText = $@"DELETE FROM TRACKS WHERE TRACK_CODE = {TRACK_CODE}";
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