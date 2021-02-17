using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class Post
    {
        //fields
        private int _postId;
        private int _userId;
        private string _description;
        private string _postImage;
        private int _trackCode;
        private string _uploadDate;

       


        //props
        public int PostId { get => _postId; set => _postId = value; }
        public int UserId { get => _userId; set => _userId = value; }
        public string Description { get => _description; set => _description = value; }
        public string PostImage { get => _postImage; set => _postImage = value; }
        public int TrackCode { get => _trackCode; set => _trackCode = value; }
        public string UploadDate { get => _uploadDate; set => _uploadDate = value; }



        //ctor
        public Post(int postId, int userId, string description, string postImage, int trackCode, string uploadDate)
        {
            PostId = postId;
            UserId = userId;
            Description = description;
            PostImage = postImage;
            TrackCode = trackCode;
            UploadDate = uploadDate;
        }

    }
}