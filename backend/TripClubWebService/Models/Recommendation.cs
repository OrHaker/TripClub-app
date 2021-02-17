using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class Recommendation
    {
        //fields
        private int _recoId;
        private int _userId;
        private int _destinationCode;
        private string _description;
        private string _postImage;
        private string _uploadDate;



        //props
        public int RecoId { get => _recoId; set => _recoId = value; }
        public int UserId { get => _userId; set => _userId = value; }
        public int DestinationCode { get => _destinationCode; set => _destinationCode = value; }
        public string Description { get => _description; set => _description = value; }
        public string PostImage { get => _postImage; set => _postImage = value; }
        public string UploadDate { get => _uploadDate; set => _uploadDate = value; }


        //ctor
        public Recommendation(int recoId, int userId, int destinationCode, string description, string postImage, string uploadDate)
        {
            RecoId = recoId;
            UserId = userId;
            DestinationCode = destinationCode;
            Description = description;
            PostImage = postImage;
            UploadDate = uploadDate;
        }

    }
}