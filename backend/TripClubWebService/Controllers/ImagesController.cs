using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace TripClubWebService.Controllers
{

   



    public class ImageFromUser
    {
        public string base64string { get; set; }
        public string name { get; set; }
        public string path { get; set; }
    }


    [EnableCors("*", "*", "*")]
    [RoutePrefix("api/Images")]
    public class ImagesController : ApiController
    {

        const string BaseURL = "http://185.60.170.14/plesk-site-preview/ruppinmobile.ac.il/site02";

        ///////    RecommendationImages
        //http://localhost:59821/api/Images/RecommendationImages
        [System.Web.Http.Route("RecommendationImages")]
        [System.Web.Http.HttpPost]
        public IHttpActionResult RecommendationImages([FromBody] ImageFromUser img)
        {

            try
            {
                string fullpath = $"{System.Web.HttpContext.Current.Server.MapPath("../../")}/{img.path}/";
                System.IO.Directory.CreateDirectory(fullpath);
                string filePath = $"{fullpath}/{img.name}.jpg";
                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(img.base64string));
                return Ok($"{BaseURL}//RecommendationImages//{img.name}.jpg");

            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }














        //http://localhost:59821/api/Images/PostsImages
        //////PostsImages
        [Route("PostsImages")]
        [HttpPost]
        public IHttpActionResult PostsImages([FromBody] ImageFromUser img)
        {

            try
            {
                string fullpath = $"{System.Web.HttpContext.Current.Server.MapPath("../../")}/{img.path}/";
                System.IO.Directory.CreateDirectory(fullpath);
                string filePath = $"{fullpath}/{img.name}.jpg";
                System.IO.File.WriteAllBytes(filePath, Convert.FromBase64String(img.base64string));
                return Ok($"{BaseURL}//PostsImages//{img.name}.jpg");
            }
            catch (Exception ex)
            {

                return Ok(ex.Message);
            }
        }

    }
}
