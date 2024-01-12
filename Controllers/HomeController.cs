using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using Collegemanagement.Repository;
using Collegemanagement.Model;
using System;
using Microsoft.AspNetCore.Identity;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection.Metadata.Ecma335;

namespace Collegemanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserRepository user;

        public HomeController(IConfiguration configuration)
        {
            user = new UserRepository(configuration);
        }
        /// <summary>
        /// signin page verfiythe user admin or user and redirect the respected page
        /// </summary>
        /// <param name="registermodel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("singin")]
        public ActionResult Signin(CredentialModel registermodel)
        {
            try
            {
                int id;
                int result;
                string name;

                bool flag = user.VerifySignIn(registermodel, out id, out result, out name);

                if (flag)
                {
                    if (result == 1)
                    {
                        return Ok(new { Id = id, Role = "User", Name = name });
                    }
                    else if (result == 2)
                    {
                        return Ok(new { Id = id, Role = "Admin", Name = name });
                    }
                    else if (result == 0)
                    {
                        return Ok(new { Id = id, Role = "Unknown", Name = name });
                    }
                }
                else
                {
                    return Ok("Invalid Username And Password");
                }

                return BadRequest("Unexpected error occurred");
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);

                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// user can register the form and nect go the signin page 
        /// </summary>
        /// <param name="register"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Signup")]
        public string Signup(RegisterModel register)
        {
            try
            {
                if (user.NewUser(register))
                {
                    return "Signup successfully";
                }
                else
                {
                    return "existing email";
                }
                
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "An error occurred during authentication";
            }
        }
        /// <summary>
        /// users if forgot the password means sending number and email match means cahnge the password
        /// </summary>
        /// <param name="forgot"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Forgotpassword")]
        public string Forgotpassword(ForgotPasswordModel forgot)
        {
            try
            {
                bool i = user.ForgotPassword(forgot);
                if (i)
                {
                    return "success";
                }
                else
                {
                    return "Invaild Email and Phonenumber";
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// contact page for the home page if any queries means send the details for admin 
        /// </summary>
        /// <param name="contact"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Contact")]
        public string Contact(ContactModel contact)
        {
            try
            {
                if (user.Contact(contact))
                {
                    return "feedback send";
                }
                else
                {
                    return "error";
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "exerror";
            }

        }
    }
}
