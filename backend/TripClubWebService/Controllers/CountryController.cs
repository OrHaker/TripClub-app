using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using TripClubWebService.Models;

namespace TripClubWebService.Controllers
{
    [EnableCors("*", "*", "*")]
    public class CountryController : ApiController
    {

        //http://localhost:59821/GetAllCountries
        [Route("GetAllCountries")]
        public IHttpActionResult Get()
        {

            try
            {
                Country[] temp = CountryDB.GetAllCountries().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Countries cannot be found!");

            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetCountryById/5
        [Route("GetCountryById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {

                Country c = CountryDB.GetCountryById(id);
                if (c != null)
                    return Ok(c);
                else return Content(HttpStatusCode.NotFound, "Country dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/InsertNewCountry
        [Route("InsertNewCountry")]
        public IHttpActionResult Post([FromBody] Country country)
        {
            try
            {
                int newCode = CountryDB.InsertNewCountry(country.Description);
                country.CountryId = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetCountryById/{ country.CountryId }"), country);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:64896/UpdateCountry
        [Route("UpdateCountry")]
        public IHttpActionResult Put([FromBody]Country country)
        {
            try
            {
                int val = CountryDB.UpdateCountry(country.CountryId, country.Description);

                if (val > 0) return Content(HttpStatusCode.OK, country);
                else return Content(HttpStatusCode.NotFound, $"{country.Description} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:64896/DeleteCountry/5
        [Route("DeleteCountry/{id}")]
        public IHttpActionResult Delete(int id)
        {

            int val = CountryDB.DeleteCountry(id);
            if (val > 0) return Ok($"Country with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"Country with id {id}  was not found to delete!!!");


        }


    }
}
