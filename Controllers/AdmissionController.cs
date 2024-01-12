using Collegemanagement.Model;
using Collegemanagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Collegemanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionController : ControllerBase
    {
        private readonly ILogger<AdmissionController> _logger; 
        private readonly AdmissionRepository admission;
        private readonly AdminRepository admin;
        public AdmissionController(ILogger<AdmissionController> logger, IConfiguration configuration)
        {
            _logger = logger;
            admission = new AdmissionRepository(configuration);
            admin= new AdminRepository(configuration);
        }
        /// <summary>
        /// show our profile
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>        
        [HttpGet]
        [Route("UserProfile/{id}")]
        public ActionResult ShowProfile(int id)
        {
            try
            {

                List<UserAdmissionModel> profile = admission.UserProfile(id);
                return Ok(profile);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }

        }
        /// <summary>
        /// show Ug list form apply the course that page 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("UGList")]
        public ActionResult UgProgram()
        {
            try
            {
                List<CourseModel> Courselist = admin.UgList();
                return Ok(Courselist);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// show the PG list and apply button also there 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PGList")]
        public ActionResult PgProgram()
        {
            try
            {
                List<CourseModel> Courselist = admin.PgProgram();
                return Ok(Courselist);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// show the Pc program list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("PCList")]
        public ActionResult PcProgram()
        {
            try
            {
                List<CourseModel> Courselist = admin.PcProgram();
                return Ok(Courselist);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// show the our status confirm or not like this
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Status/{ID}")]
        public ActionResult Status(int ID)
        {
            try
            {
                List<UserAdmissionModel> user = admission.Status(ID);

                return Ok(user);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// get the userdetails by id fetching for the details
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("fetchProfileById/{id}")]
        public ActionResult<ProfileModel> EditProfile(int id)
        {
            try
            {
                var userProfile = admission.EditUserProfile(id);

                if (userProfile != null && userProfile.Any())
                {
                    var user = userProfile.Find(e => e.ID == id);
                    return Ok(user);
                }
                else
                { 
                    return NotFound();
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// if fetched means if you can updated 
        /// </summary>
        /// <param name="admission1"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UpdateProfile/{id}")]
        public string EditProfile(ProfileModel admission1,int id)
        {
            try
            {
                admission.Update(admission1,id);

                return  "profile updated successfully";
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// user can apply the course this URL is used for fetching the details
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Applyuser/{ID}")]
        public ActionResult ApplyUser(int ID)
        {
            try
            {
                var apply = admission.ApplyUser(ID).Find(e => e.ID == ID);
                return Ok(apply);

            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return StatusCode(500, "Internal Server Error");
            }
        }
        /// <summary>
        /// fill the all details and post the value for apply form page 
        /// </summary>
        /// <param name="admissionobject"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Applyuserbyid/{id}")]
        public string ApplyUser(UserAdmissionModel admissionobject,int id)
        {
            try
            {
                if (admission.UpdateUserAdmission(admissionobject,id))
                {

                    return "apply successfull";
                }
                else
                {
                    return "fail";
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }

    }
}
