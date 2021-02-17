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
    public class RecommendationController : ApiController
    {


        //http://localhost:59821/GetAllFullRecommendations
        [Route("GetAllFullRecommendations")]
        [HttpGet]
        public IHttpActionResult GetAllFullRecommendations()
        {

            try
            {
                ExtendedRecommendation[] temp = RecommendationDB.GetAllFullRecommendations().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Recommendations cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }



        //http://localhost:59821/GetAllFullRecommendationsById/1
        [Route("GetAllFullRecommendationsById/{id}")]
        [HttpGet]
        public IHttpActionResult GetAllFullRecommendationsById(int id)
        {

            try
            {
                ExtendedRecommendation[] temp = RecommendationDB.GetAllFullRecommendationsById(id).ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Recommendations cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetRecommendationById/5
        [Route("GetRecommendationById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Recommendation r = RecommendationDB.GetRecommendationById(id);
                if (r != null)
                    return Ok(r);
                else return Content(HttpStatusCode.NotFound, "Post dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/InsertNewRecommendation
        [Route("InsertNewRecommendation")]
        public IHttpActionResult Post([FromBody] Recommendation recommendation)
        {
            try
            {
                int newId = RecommendationDB.InsertNewRecommendation(recommendation);
                recommendation.RecoId = newId;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetRecommendationById/{recommendation.RecoId}"), recommendation);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:59821/UpdateRecommendation
        [Route("UpdateRecommendation")]
        public IHttpActionResult Put([FromBody]Recommendation recommendation)
        {
            try
            {
                int val = RecommendationDB.UpdateRecommendation(recommendation);

                if (val > 0) return Content(HttpStatusCode.OK, recommendation);
                else return Content(HttpStatusCode.NotFound, $"Recommendation with id {recommendation.RecoId} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/DeleteRecommendation/5
        [Route("DeleteRecommendation/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = RecommendationDB.DeleteRecommendation(id);
            if (val > 0) return Ok($"Recommendation with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"Recommendation with id {id}  was not found to delete!!!");
        }

    }
}
