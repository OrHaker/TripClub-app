using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class Destination
    {
        //fields
        private int _destinationCode;
        private int _countryId;
        private string _description;

        


        //props
        public int DestinationCode { get => _destinationCode; set => _destinationCode = value; }
        public int CountryId { get => _countryId; set => _countryId = value; }
        public string Description { get => _description; set => _description = value; }



        //ctor
        public Destination(int destinationCode, int countryId, string description)
        {
            DestinationCode = destinationCode;
            CountryId = countryId;
            Description = description;
        }
    }
}