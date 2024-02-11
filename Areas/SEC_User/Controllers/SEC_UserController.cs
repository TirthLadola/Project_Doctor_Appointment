using Microsoft.AspNetCore.Mvc;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Project_Doctor_Appointment.Areas.SEC_User.Models;
using Project_Doctor_Appointment.DAL;
using System.Data;

namespace Project_Doctor_Appointment.Areas.SEC_User.Controllers
{
    [Area("SEC_User")]
    [Route("SEC_User/[Controller]/[action]")]
    public class SEC_UserController : Controller
    {
        private IConfiguration Configuration;
        public SEC_UserController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        [HttpPost]
        public IActionResult Login(SEC_UserModel modelSEC_User)
        {
            string ErrorMsg = string.Empty;
            if (string.IsNullOrEmpty(modelSEC_User.UserName))
            {
                ErrorMsg += "User Name is Required";
            }
            if (string.IsNullOrEmpty(modelSEC_User.Password))
            {
                ErrorMsg += "<br/>Password is Required";
            }

            if (!string.IsNullOrEmpty(ErrorMsg))
            {
                TempData["Error"] = ErrorMsg;
                return RedirectToAction("Index");
            }
            else
            {
                SEC_User_DAL dal = new SEC_User_DAL();
                DataTable dt = dal.PR_SEC_User_SelectBYUserNamePassword(modelSEC_User.UserName,modelSEC_User.Password);
                if(dt.Rows.Count > 0)
                {
                    foreach(DataRow dr in dt.Rows)
                    {
                        HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                        HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                        HttpContext.Session.SetString("Password", dr["Password"].ToString());
                        HttpContext.Session.SetString("FirstName", dr["FirstName"].ToString());
                        HttpContext.Session.SetString("LastName", dr["LastName"].ToString());
                        HttpContext.Session.SetString("PhotoPath", dr["PhotoPath"].ToString());
                        HttpContext.Session.SetString("IsAdmin", dr["IsAdmin"].ToString());
                        break;



                    }
                }
                else
                {
                    TempData["Error"] = "User Nmae or Passwod is Invalid!";
                    return RedirectToAction("Index");
                }
                if(HttpContext.Session.GetString("UserName") != null && HttpContext.Session.GetString("Password") != null)
                {
                    string isAdmin = HttpContext.Session.GetString("IsAdmin").ToString();
                    if(isAdmin == "True")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Patient");
                    }
                }
            }
            return RedirectToAction("Index");


        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index","SEC_User");
        }
    }
}
