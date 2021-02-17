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
    public class DestinationController : ApiController
    {
        [EnableCors("*", "*", "*")]
        //http://localhost:59821/GetAllDestinations
        [Route("GetAllDestinations")]
        public IHttpActionResult Get()
        {

            try
            {
                Destination[] temp = DestinationDB.GetAllDestinations().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Destinations cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetDestinationId/5
        [Route("GetDestinationId/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Destination d = DestinationDB.GetDestinationId(id);
                if (d != null)
                    return Ok(d);
                else return Content(HttpStatusCode.NotFound, "Destination dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/InsertNewDestination
        [Route("InsertNewDestination")]
        public IHttpActionResult Post([FromBody] Destination destination)
        {
            try
            {
                int newCode = DestinationDB.InsertNewDestination(destination.CountryId, destination.Description);
                destination.DestinationCode = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetDestinationId/{destination.DestinationCode}"), destination);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:59821/UpdateDestination
        [Route("UpdateDestination")]
        public IHttpActionResult Put([FromBody]Destination destination)
        {
            try
            {
                int val = DestinationDB.UpdateDestination(destination.DestinationCode, destination.CountryId, destination.Description);

                if (val > 0) return Content(HttpStatusCode.OK, destination);
                else return Content(HttpStatusCode.NotFound, $"{destination.Description} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/DeleteDestination/5
        [Route("DeleteDestination/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = DestinationDB.DeleteDestination(id);
            if (val > 0) return Ok($"Destination with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"Destination with id {id}  was not found to delete!!!");
        }

    }
}
