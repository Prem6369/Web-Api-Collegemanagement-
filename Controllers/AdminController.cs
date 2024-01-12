using Collegemanagement.Model;
using Collegemanagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace Collegemanagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly ILogger<HomeController> _logger;
        private readonly AdminRepository admin;

        public AdminController(IConfiguration configuration)
        {

            admin = new AdminRepository(configuration);
        }

        /// <summary>
        /// API endpoint for adding a new admin.
        /// </summary>
        /// <param name="adminObject">AdminModel object containing admin details.</param>
        /// <returns>Success or error message.</returns>
        [HttpPost]
        [Route("Addadmin")]
        public string AddNewAdmin(AdminModel adminObject)
        {
            try
            {
                if (admin.AddAdmin(adminObject))
                {
                    return "new admin added successfully";
                }
                else
                {
                    return "Email Already Taken Please Use Another Email";
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }

        }
        /// <summary>
        /// get the admin list using repository
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("adminList")]
        public ActionResult AdminDetails()
        {
            try
            {
                List<RegisterModel> adminList = admin.AdminList();
                return Ok(adminList);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }

        }
        /// <summary>
        /// detele the admin using session
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteAdmin/{id}")]
        public string AdminDelete(int id)
        {
            try
            {
                if (admin.DeleteAdmin(id))
                {
                    return "delete";
                }
                else
                {
                    return "Internal Server Error";
                }
              
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// user details aplly people show here return the list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Userdetails")]
        public ActionResult UserDetails()
        {
            try
            {
                List<UserAdmissionModel> userList = admin.UserDetails();
                return Ok(userList);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }
        }
        /// <summary>
        /// admin can add the PG course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPG")]
        public string AddPg(CourseModel course)
        {
            try
            {
                if (admin.AddPgCourse(course))
                {
                    return "New PG Course Added successfully";
                }
                else
                {
                    return "Errorcourse";
;                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// admin add the PC course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddPC")]
        public string AddPc(CourseModel course)
        {
            try
            {
                if (admin.AddUgCourse(course))
                {
                    return "CourseCreated successfully";
                }
                else
                {
                    return "Errorcourse";
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// admin add the UG course
        /// </summary>
        /// <param name="course"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddUG")]
        public string AddUg(CourseModel course)
        {
            try
            {
                if (admin.AddUgCourse(course))
                {
                   return "CourseCreated successfully";
                }
                else
                {
                    return "Errorcourse";
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }

        /// <summary>
        /// get the UG program list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ugList")]
        public ActionResult UgProgram()
        {
            try
            {
                List<CourseModel> courseList = admin.UgList();
                return Ok(courseList);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }
        }

        /// <summary>
        /// get the PG course list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pgList")]
        public ActionResult PgProgram()
        {
            try
            {
                List<CourseModel> courseList = admin.PgProgram();
                return Ok(courseList);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }
        }
        /// <summary>
        /// get the PC course list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("pcList")]
        public ActionResult PcProgram()
        {
            try
            {
                List<CourseModel> courseList = admin.PcProgram();
                return Ok(courseList);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }
        }
        /// <summary>
        /// delete ug course form admin
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deleteUg/{courseId}")]
        public string DeleteUg(string courseId)
        {
            try
            {
                if (admin.DeleteData(courseId))
                {
                    return "delete";
                }
                return "id is not here";
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// delete the PG course from courseid
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deletePg/{courseId}")]
        public string DeletePg(string courseId)
        {
            try
            {
                if (admin.DeleteData(courseId))
                {
                    return "delete";
                }
                return "id is not here";
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// delete PC course form courseid
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("deletePc/{courseId}")]
        public string DeletePc(string courseId)
        {
            try
            {
                if (admin.DeleteData(courseId))
                {
                    return "delete";
                }
                else
                {
                    return "id is not here";
                }
               
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// get the coursebyid this is used for fetching the details
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetCourseId/{Courseid}")]
        public ActionResult<CourseModel> GetById(string courseId)
        {
            try
            {
                var courseList = admin.GetCourseById(courseId);

                if (courseList != null && courseList.Any())
                {
                    var course = courseList.Find(e => e.Courseid == courseId);

                    if (course != null)
                    {
                        return Ok(course);
                    }
                    else
                    {
                        return NotFound($"Course with ID {courseId} not found");
                    }
                }
                else
                {
                    return NotFound($"No courses found for ID {courseId}");
                }
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }
        }
        /// <summary>
        /// update the all course 1st get the list and fetch the details and update
        /// </summary>
        /// <param name="course"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateCourse/{id}")]
        public string UpdateCourse(CourseModel course,string id)
        {
            try
            {
                bool temp = admin.Update(course, id);
                if (temp)
                {
                    return "Update Successfully";
                }
                else
                {
                    return "An error occurred while processing Update course";
                }
            }
            catch(Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
        /// <summary>
        /// this contact form details get the list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Contactform")]
        public ActionResult ContactDetails()
        {
            try
            {
                List<ContactModel> contactList = admin.ContactForm();
                return Ok(contactList);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }
        }
        /// <summary>
        /// register people are show here list
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("registerList")]
        public ActionResult Register()
        {
            try
            {
                List<RegisterModel> userlist = admin.RegisterList();
                return Ok(userlist);
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return BadRequest("Internal Server Error");
            }

        }

        /// <summary>
        /// Get the status form admin and process that 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("statusChange/{id}/{Status}")]
        public string Status(int id, int Status)
        {
            try
            {
                bool k = admin.StatusChange(id, Status);
                return "Status updated";
            }
            catch (Exception exception)
            {
                ErrorLog.LogError(exception);
                return "Internal Server Error";
            }
        }
    }


}
