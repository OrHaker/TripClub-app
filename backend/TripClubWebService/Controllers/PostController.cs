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
    public class PostController : ApiController
    {
        //http://localhost:59821/GetAllPosts
        [Route("GetAllPosts")]
        public IHttpActionResult Get()
        {

            try
            {
                Post[] temp = PostDB.GetAllPosts().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Posts cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetAllPostOfPartners
        [Route("GetAllPostOfPartners")]
        [HttpGet]
        public IHttpActionResult GetAllPostOfPartners()
        {

            try
            {
                ExtendedPost[] temp = PostDB.AllPostOfPartners().ToArray();
                if (temp != null)
                    return Ok(temp);
                else return Content(HttpStatusCode.NotFound, "Posts cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }



        //http://localhost:59821/PostsOfPartnersByUserId/5
        [Route("PostsOfPartnersByUserId/{id}")]
        [HttpGet]
        public IHttpActionResult PostsOfPartnersByUserId(int id)
        {
            try
            {
                ExtendedPost[] p = PostDB.PostsOfPartnersByUserId(id).ToArray();

                if (p != null)
                    return Ok(p);
                else return Content(HttpStatusCode.NotFound, "Posts dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }





        //http://localhost:59821/GetPostById/5
        [Route("GetPostById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                Post p = PostDB.GetPostById(id);

                if (p != null)
                    return Ok(p);
                else return Content(HttpStatusCode.NotFound, "Post dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/InsertNewPost
        [Route("InsertNewPost")]
        public IHttpActionResult Post([FromBody] Post post)
        {
            try
            {
                int newCode = PostDB.InsertNewPost(post);
                post.PostId = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetPostById/{post.PostId}"), post);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:59821/UpdatePost
        [Route("UpdatePost")]
        public IHttpActionResult Put([FromBody]Post post)
        {
            try
            {
                int val = PostDB.UpdatePost(post);

                if (val > 0) return Content(HttpStatusCode.OK, post);
                else return Content(HttpStatusCode.NotFound, $"{post.Description} with id {post.PostId} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/DeletePost/5
        [Route("DeletePost/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = PostDB.DeletePost(id);
            if (val > 0) return Ok($"Post with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"Post with id {id}  was not found to delete!!!");
        }
    }
}
