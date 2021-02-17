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
    public class UserController : ApiController
    {

        //http://localhost:59821/GetAllUsers
        [Route("GetAllUsers")]
        public IHttpActionResult Get()
        {

            try
            {
                User[] users = UserDB.GetAllUsers().ToArray();
                if (users != null)
                    return Ok(users);
                else return Content(HttpStatusCode.NotFound, "Users cannot be found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetUserById/5
        [Route("GetUserById/{id}")]
        public IHttpActionResult Get(int id)
        {
            try
            {
                User user = UserDB.GetUserById(id);
                if (user != null)
                    return Ok(user);
                else return Content(HttpStatusCode.NotFound, "User dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/GetUserByEmailAndPassword?email=or@or.com&password=123456
        [Route("GetUserByEmailAndPassword")]
        public IHttpActionResult GetUserByEmailAndPassword(string email, string password)
        {
            try
            {
                User user = UserDB.GetUserByEmailAndPassword(email, password);
                if (user != null)
                    return Ok(user);
                else return Content(HttpStatusCode.NotFound, "User dont found!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }




        //http://localhost:59821/InsertNewUser
        [Route("InsertNewUser")]
        public IHttpActionResult Post([FromBody] User user)
        {
            try
            {
                //chek if email already taken
                User userToCheck = UserDB.GetUserByEmail(user.Email);
                if (userToCheck != null)    
                    return Content(HttpStatusCode.Conflict, $"User with Email '{user.Email}' already exists.");

                int newCode = UserDB.InsertNewUser(user);
                user.UserId = newCode;
                return Created(new Uri(Request.RequestUri.AbsoluteUri + $"/GetUserById/{user.UserId}"), user);
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }

        }


        //http://localhost:59821/UpdateUser
        [Route("UpdateUser")]
        public IHttpActionResult Put([FromBody]User user)
        {
            try
            {
                int rowsEffected = UserDB.UpdateUser(user);
                if (rowsEffected > 0) return Content(HttpStatusCode.OK, user);
                else return Content(HttpStatusCode.NotFound, $"User with id {user.UserId} was not found to update!");
            }
            catch (Exception ex)
            {
                return Content(HttpStatusCode.BadRequest, ex);
            }
        }


        //http://localhost:59821/DeleteUser/5
        [Route("DeleteUser/{id}")]
        public IHttpActionResult Delete(int id)
        {
            int val = UserDB.DeleteUser(id);
            if (val > 0) return Ok($"User with id {id} Successfully deleted!");
            else return Content(HttpStatusCode.NotFound, $"User with id {id}  was not found to delete!!!");
        }

    }
}
