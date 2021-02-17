using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class User
    {
        //fields
        private int _userId;
        private string _userName;
        private string _password;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _userImage;
        private char _gender;
        private int _userType;
        private string _discription;
        
        
        //props
        public int UserId { get => _userId; set => _userId = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string Password { get => _password; set => _password = value; }
        public string Email { get => _email; set => _email = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string UserImage { get => _userImage; set => _userImage = value; }
        public char Gender { get => _gender; set => _gender = value; }
        public int UserType { get => _userType; set => _userType = value; }
        public string Discription { get => _discription; set => _discription = value; }

        
        //ctor
        public User(int userId, string userName, string password, string email, string firstName, string lastName, string userImage, char gender, int userType, string discription)
        {
            UserId = userId;
            UserName = userName;
            Password = password;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            UserImage = userImage;
            Gender = gender;
            UserType = userType;
            Discription = discription;
        }

    }
}