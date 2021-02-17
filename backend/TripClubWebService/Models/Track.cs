using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class Track
    {
        //fields
        private int _trackCode;
        private int _userId;
        private string _description;
        private bool _active;
        private string _trackLocations;



        //props
        public int TrackCode { get => _trackCode; set => _trackCode = value; }
        public int UserId { get => _userId; set => _userId = value; }
        public string Description { get => _description; set => _description = value; }
        public bool Active { get => _active; set => _active = value; }
        public string TrackLocations { get => _trackLocations; set => _trackLocations = value; }
      
        
        //ctor
        public Track(int trackCode, int userId, string description, bool active, string trackLocations)
        {
            TrackCode = trackCode;
            UserId = userId;
            Description = description;
            Active = active;
            TrackLocations = trackLocations;
        }
    }
}