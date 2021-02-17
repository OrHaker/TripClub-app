using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class UserDB
    {

        //DB SCHEMA
        //USER_ID
        //USER_NAME
        //PASSWORD
        //EMAIL
        //FIRST_NAME
        //LAST_NAME
        //USER_IMAGE
        //GENDER
        //USER_TYPE
        //DISCRIPTION


        private static bool local = false;
        private static string _conStr = null;


        private static SqlConnection con;
        private static SqlCommand command;


        private static string strConLocal = ConfigurationManager.ConnectionStrings["strConLocal"].ConnectionString;
        private static string strConLIVEDNS = ConfigurationManager.ConnectionStrings["strConLIVEDNS"].ConnectionString;



        static UserDB()
        {
            if (local)
                _conStr = strConLocal;
            else
                _conStr = strConLIVEDNS;

            con = new SqlConnection(_conStr);
            command = new SqlCommand();
            command.Connection = con;
        }




        private static List<User> ExecReader(string commandString)
        {
            try
            {
                List<User> listToReturn = new List<User>();
                con.Open();
                command.CommandText = commandString;
                SqlDataReader dataReader;

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    var USER_ID = (int)dataReader["USER_ID"];
                    var USER_NAME = dataReader["USER_NAME"].ToString();
                    var PASSWORD = dataReader["PASSWORD"].ToString();
                    var EMAIL = dataReader["EMAIL"].ToString();
                    var FIRST_NAME = dataReader["FIRST_NAME"].ToString();
                    var LAST_NAME = dataReader["LAST_NAME"].ToString();
                    var USER_IMAGE = dataReader["USER_IMAGE"].ToString();
                    var GENDER = (char)(dataReader["GENDER"]).ToString()[0];
                    var USER_TYPE = (int)dataReader["USER_TYPE"];
                    var DISCRIPTION = dataReader["DISCRIPTION"].ToString();

                    listToReturn.Add(
                        new User(USER_ID, USER_NAME, PASSWORD, EMAIL, FIRST_NAME, LAST_NAME, USER_IMAGE, GENDER, USER_TYPE, DISCRIPTION)
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


        //READ USER BY EMAIL
        public static User GetUserByEmail(string email)
        {
            User user = ExecReader($"SELECT * FROM USERS WHERE USERS.EMAIL = '{email}'").FirstOrDefault();

            return user;
        }

        //READ USER BY EMAIL AND PASSWORD
        public static User GetUserByEmailAndPassword(string email, string password)
        {
            User user = ExecReader($"SELECT * FROM USERS WHERE USERS.EMAIL = '{email}' AND USERS.PASSWORD = '{password}'").FirstOrDefault();
            return user;
        }
        


        //READ ALL USERS
        public static List<User> GetAllUsers()
        {
            List<User> listToReturn = ExecReader("SELECT * FROM USERS");

            return listToReturn;
        }




        //READ ONE USER BY ID
        public static User GetUserById(int USER_ID)
        {
            User countryToReturn = ExecReader($"SELECT * FROM USERS   WHERE USER_ID = {USER_ID} ").FirstOrDefault();
            return countryToReturn;
        }




        //INSERT USER
        public static int InsertNewUser(User newUser)
        {

            try
            {
                command.CommandText = $"INSERT INTO USERS " +
                                  $" (USER_NAME, PASSWORD , EMAIL , FIRST_NAME ," +
                                  $" LAST_NAME , USER_IMAGE , GENDER , " +
                                  $" USER_TYPE , DISCRIPTION) " +
                                  $" VALUES(N'{newUser.UserName}' , N'{newUser.Password}' ," +
                                  $" '{newUser.Email}' , N'{newUser.FirstName}' ," +
                                  $" N'{newUser.LastName}' , N'{newUser.UserImage}' ," +
                                  $" '{newUser.Gender}' , {newUser.UserType} ," +
                                  $" N'{newUser.Discription}') ";

                con.Open();
                command.ExecuteNonQuery();
                int userId = -1;

                //select the last inserted identity (use for auto identity table)
                command.CommandText = "SELECT SCOPE_IDENTITY() as [IDENTITY]";
                SqlDataReader dataReader;
                dataReader = command.ExecuteReader();
                while (dataReader.Read())
                    userId = int.Parse(dataReader["IDENTITY"].ToString());
                return userId;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }






        //UPDATE USER
        public static int UpdateUser(User user)
        {
            try
            {
                command.CommandText = $"UPDATE USERS " +
                    $" SET USER_NAME = N'{user.UserName}' , " +
                    $" PASSWORD = N'{user.Password}' , " +
                    $" EMAIL = '{user.Email}' ," +
                    $" FIRST_NAME = N'{user.FirstName}' , " +
                    $" LAST_NAME = N'{user.LastName}' ," +
                    $" USER_IMAGE = N'{user.UserImage}' ," +
                    $" GENDER = '{user.Gender}' ," +
                    $" USER_TYPE = {user.UserType} ," +
                    $" DISCRIPTION = N'{user.Discription}' " +
                    $" WHERE USER_ID = {user.UserId}";

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



        public static int UserForgotPassword(string Email, string Password)
        {
            try
            {
                command.CommandText = $"UPDATE USERS " +
                    $" SET  PASSWORD = N'{Password}'  " +
                    $" WHERE EMAIL = '{Email}'";

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




        //DELETE USER
        public static int DeleteUser(int USER_ID)
        {
            try
            {
                command.CommandText = $@"DELETE FROM USERS WHERE USER_ID = {USER_ID}";
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