using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{    
    public class ExtendedTrack : Track
    {
        //fields
        private string _userName;
        private string _email;
        private string _firstName;
        private string _lastName;
        private string _userImage;




        //props
        public string UserName { get => _userName; set => _userName = value; }
        public string Email { get => _email; set => _email = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string UserImage { get => _userImage; set => _userImage = value; }

        
        //ctor
        public ExtendedTrack(string userName, string email, string firstName, string lastName, string userImage, Track t)
            : base(t.TrackCode, t.UserId, t.Description, t.Active, t.TrackLocations)
        {
            UserName = userName;
            Email = email;
            FirstName = firstName;
            LastName = lastName;
            UserImage = userImage;
        }
    }
}