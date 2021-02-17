using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{

    public class ExtendedRecommendation : Recommendation
    {
        //fields
        private string _destinationName;
        private string _userEmail;
        private string _userName;
        private string _firstName;
        private string _lastName;
        private string _userImage;

        private string _countryId;
        private string _countryName;

        //props
        public string DestinationName { get => _destinationName; set => _destinationName = value; }
        public string UserEmail { get => _userEmail; set => _userEmail = value; }
        public string UserName { get => _userName; set => _userName = value; }
        public string FirstName { get => _firstName; set => _firstName = value; }
        public string LastName { get => _lastName; set => _lastName = value; }
        public string UserImage { get => _userImage; set => _userImage = value; }
        public string CountryId { get => _countryId; set => _countryId = value; }
        public string CountryName { get => _countryName; set => _countryName = value; }


        //ctor
        public ExtendedRecommendation(string destinationName, string userEmail, string userName, string firstName, string lastName, string userImage, string countryId,
        string countryName, Recommendation r)
            : base(r.RecoId, r.UserId, r.DestinationCode, r.Description, r.PostImage, r.UploadDate)
        {
            DestinationName = destinationName;
            UserEmail = userEmail;
            UserName = userName;
            FirstName = firstName;
            LastName = lastName;
            UserImage = userImage;
            CountryId=countryId;
            CountryName=countryName;
        }

    }
}