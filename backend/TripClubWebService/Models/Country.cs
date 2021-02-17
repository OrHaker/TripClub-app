using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TripClubWebService.Models
{
    public class Country
    {
        //fields
        private int _countryId;
        private string _description;



        //props
        public int CountryId { get => _countryId; set => _countryId = value; }
        public string Description { get => _description; set => _description = value; }



        //ctor
        public Country(int countryId, string description)
        {
            CountryId = countryId;
            Description = description;
        }
    }
}