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
    public class TrackController : ApiController
    {


       
        //http://localhost:59821/GetAllFullTracks
        [Route("GetAllFullTracks")]
        [HttpGet]
        public IHttpActionResult GetAllFullTracks()
        {
            try
            {
                ExtendedTrack[] temp = TrackDB.GetAllFullTracks().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Tracks cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }



        //http://localhost:59821/GetAllFullTracksByUser/1
        [Route("GetAllFullTracksByUser/{id}")]
        [HttpGet]
        public IHttpActionResult GetAllFullTracksByUser(int id)
        {
            try
            {
                ExtendedTrack[] temp = TrackDB.GetAllFullTracksByUser(id).ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Tracks cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetAllFullTracksByDestination/1
        [Route("GetAllFullTracksByDestination/{id}")]
        [HttpGet]
        public IHttpActionResult GetAllFullTracksByDestination(int id)
        {
            try
            {
                ExtendedTrack[] temp = TrackDB.GetAllFullTracksByDestination(id).ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Tracks cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }








        //http://localhost:59821/GetAllTracks
        [Route("GetAllTracks")]
        public IHttpActionResult Get()
        {
            try
            {
                Track[] temp = TrackDB.GetAllTracks().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Tracks cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetAllTracksLocationsData
        [Route("GetAllTracksLocationsData")]
        public IHttpActionResult GetAllTracksLocationsData()
        {
            try
            {
                Object[] temp = TrackDB.GetAllTracksLocationsData().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Locations cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetTrackById/5
        [Route("GetTrackById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Track t = TrackDB.GetTrackById(id);
                if (t != null)
                    return Ok(t);
                else return Content(HttpStatusCode.NotFound, "Track dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/InsertNewTrack
        [Route("InsertNewTrack")]
        public IHttpActionResult Post([FromBody] Track track)
        {
            try
            {
                int newCode = TrackDB.InsertNewTrack(track);
                track.TrackCode = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetTrackById/{track.TrackCode}"), track);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:64896/UpdateTrack
        [Route("UpdateTrack")]
        public IHttpActionResult Put([FromBody]Track track)
        {
            try
            {
                int rowsEffected = TrackDB.UpdateTrack(track);

                if (rowsEffected > 0) return Content(HttpStatusCode.OK, track);
                else return Content(HttpStatusCode.NotFound, $"Track with id {track.TrackCode} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:64896/DeleteTrack/5
        [Route("DeleteTrack/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = TrackDB.DeleteTrack(id);
            if (val > 0) return Ok($"Track with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"Track with id {id}  was not found to delete!!!");
        }






    }
}
