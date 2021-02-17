using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class PostDB
    {
        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


        private static string strConLocal = ConfigurationManager.ConnectionStrings["strConLocal"].ConnectionString;
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;


        static PostDB()
        {
            if (local)
                _conStr = strConLocal;
            else
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }




        private static List<Post> ExecReader(string commandString)
        {
            try
            {

                List<Post> listToReturn = new List<Post>();
                if (con.State == ConnectionState.Open)
                    con.Close();
                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var POST_ID = (int)dataReader["POST_ID"];
                    var USER_ID = (int)dataReader["USER_ID"];
                    var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                    var POST_IMAGE = (dataReader["POST_IMAGE"] != DBNull.Value ? dataReader["POST_IMAGE"] : string.Empty).ToString();
                    var TRACK_CODE = (int)(dataReader["TRACK_CODE"] != DBNull.Value ? dataReader["TRACK_CODE"] : -1);
                    var UPLOAD_DATE = dataReader["UPLOAD_DATE"].ToString();

                    listToReturn.Add(new Post(POST_ID, USER_ID, DESCRIPTION, POST_IMAGE, TRACK_CODE, UPLOAD_DATE));
                }

                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }





        //READ ALL POSTS INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedPost> AllPostOfPartners()
        {
            try
            {
                List<ExtendedPost> listToReturn = new List<ExtendedPost>();
                SqlCommand procedureComm = new SqlCommand($"ALL_POST_OF_PARTNERS", con);

                procedureComm.CommandType = CommandType.StoredProcedure;

                con.Open();
                using (SqlDataReader dataReader = procedureComm.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var POST_ID = (int)dataReader["POST_ID"];
                        var USER_ID = (int)dataReader["USER_ID"];
                        var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                        var POST_IMAGE = (dataReader["POST_IMAGE"] != DBNull.Value ? dataReader["POST_IMAGE"] : string.Empty).ToString();
                        var TRACK_CODE = (int)(dataReader["TRACK_CODE"] != DBNull.Value ? dataReader["TRACK_CODE"] : -1);
                        var UPLOAD_DATE = dataReader["UPLOAD_DATE"].ToString();
                        var p = new Post(POST_ID, USER_ID, DESCRIPTION, POST_IMAGE, TRACK_CODE, UPLOAD_DATE);



                        var USER_NAME = dataReader["USER_NAME"].ToString();
                        var FIRST_NAME = dataReader["FIRST_NAME"].ToString();
                        var LAST_NAME = dataReader["LAST_NAME"].ToString();
                        var USER_IMAGE = dataReader["USER_IMAGE"].ToString();
                        var EMAIL = dataReader["EMAIL"].ToString();
                        listToReturn.Add(new ExtendedPost(USER_NAME, FIRST_NAME, LAST_NAME, USER_IMAGE, EMAIL, p));
                    }
                }
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }





        //READ ALL POSTS BY USER ID INCLUDE USER DETAILS -- StoredProcedure
        public static List<ExtendedPost> PostsOfPartnersByUserId(int USER_ID_OUT)
        {
            try
            {
                List<ExtendedPost> listToReturn = new List<ExtendedPost>();
                SqlCommand procedureComm = new SqlCommand($"POST_OF_PARTNERS_BY_USER", con);

                procedureComm.CommandType = CommandType.StoredProcedure;

                procedureComm.Parameters.Add(new SqlParameter("@USER_ID_OUT", USER_ID_OUT));


                con.Open();
                using (SqlDataReader dataReader = procedureComm.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var POST_ID = (int)dataReader["POST_ID"];
                        var USER_ID = (int)dataReader["USER_ID"];
                        var DESCRIPTION = dataReader["DESCRIPTION"].ToString();
                        var POST_IMAGE = (dataReader["POST_IMAGE"] != DBNull.Value ? dataReader["POST_IMAGE"] : string.Empty).ToString();
                        var TRACK_CODE = (int)(dataReader["TRACK_CODE"] != DBNull.Value ? dataReader["TRACK_CODE"] : -1);
                        var UPLOAD_DATE = dataReader["UPLOAD_DATE"].ToString();
                        var p = new Post(POST_ID, USER_ID, DESCRIPTION, POST_IMAGE, TRACK_CODE, UPLOAD_DATE);



                        var USER_NAME = dataReader["USER_NAME"].ToString();
                        var FIRST_NAME = dataReader["FIRST_NAME"].ToString();
                        var LAST_NAME = dataReader["LAST_NAME"].ToString();
                        var USER_IMAGE = dataReader["USER_IMAGE"].ToString();
                        var EMAIL = dataReader["EMAIL"].ToString();
                        listToReturn.Add(new ExtendedPost(USER_NAME, FIRST_NAME, LAST_NAME, USER_IMAGE, EMAIL, p));
                    }
                }
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }



        //READ ALL POSTS
        public static List<Post> GetAllPosts()
        {
            try
            {
                List<Post> listToReturn = ExecReader("SELECT * FROM POSTS");
                return listToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }




        //READ ONE POST BY ID
        public static Post GetPostById(int POST_ID)
        {
            try
            {
                Post postToReturn = ExecReader($"SELECT * FROM POSTS   WHERE POST_ID = {POST_ID} ").FirstOrDefault();
                return postToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }





        //READ ONE POST BY ID
        public static Post GetLastPost(int USER_ID, string DESCRIPTION, string UPLOAD_DATE)
        {
            try
            {
                Post postToReturn = ExecReader($"SELECT * FROM POSTS   WHERE USER_ID = {USER_ID} AND DESCRIPTION='{DESCRIPTION}' AND UPLOAD_DATE = '{UPLOAD_DATE} '").FirstOrDefault();
                return postToReturn;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }


        //INSERT POST
        public static int InsertNewPost(Post newPost)
        {
            try
            {
                command.CommandText = $"INSERT INTO POSTS " +
                                  $" (USER_ID,DESCRIPTION,POST_IMAGE,TRACK_CODE,UPLOAD_DATE) " +
                                  $" VALUES({newPost.UserId}, N'{newPost.Description}' , N'{newPost.PostImage}' , {newPost.TrackCode} , '{newPost.UploadDate}' ) ";

                con.Open();
                command.ExecuteNonQuery();
                int postId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    postId = int.Parse(dataReader["IDENTITY"].ToString());
                return postId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //UPDATE POST
        public static int UpdatePost(Post post)
        {
            try
            {
                command.CommandText = $"UPDATE POSTS SET DESCRIPTION = N'{post.Description}' , " +
                    $" USER_ID = {post.UserId} , POST_IMAGE = N'{post.PostImage}' , " +
                    $" TRACK_CODE = {post.TrackCode}  WHERE POST_ID = {post.PostId}";

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








        //DELETE POST
        public static int DeletePost(int POST_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM POSTS WHERE POST_ID = {POST_ID}";

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