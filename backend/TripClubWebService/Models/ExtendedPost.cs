using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class ExtendedPost : Post
    {
        //fields
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _userImage;
        private string _userEmail;



        //props
        public string UserName { get => _userName; set => _userName = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string UserImage { get => _userImage; set => _userImage = value; }
        public string UserEmail { get => _userEmail; set => _userEmail = value; }



        //ctor
        public ExtendedPost(string userName, string firstName, string lastName, string userImage, string userEmail, Post postValues)
            : base(postValues.PostId, postValues.UserId, postValues.Description, postValues.PostImage, postValues.TrackCode, postValues.UploadDate)
        {
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            UserImage = userImage;
            UserEmail = userEmail;
        }

    }
}
