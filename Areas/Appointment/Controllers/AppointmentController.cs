using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project_Doctor_Appointment.Areas.Appointment.Models;

namespace Project_Doctor_Appointment.Areas.Appointment.Controllers
{
    [Area("Appointment")]
    [Route("Appointment/{Controller}/{action}")]
    public class AppointmentController : Controller
    {
        private IConfiguration Configuration;
        public AppointmentController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }
        public IActionResult Index()
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Appointment_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);


            return View("AppointmentList", dt);
        }


        public IActionResult Delete(int AppointmentID)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Appointment_Delete";
            cmd.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(AppointmentModel modelAppointment)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelAppointment.AppointmentID == null)
            {
                cmd.CommandText = "PR_Appointment_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Appointment_Update";
                cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = modelAppointment.AppointmentID;

            }
            cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = modelAppointment.PatientID;
            cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = modelAppointment.DoctorID;
            cmd.Parameters.Add("@AppointmentTypeID", SqlDbType.Int).Value = modelAppointment.AppointmentTypeID;
            cmd.Parameters.Add("@AppointmentDate", SqlDbType.DateTime).Value = modelAppointment.AppointmentDate;
            cmd.Parameters.Add("@AppointmentTime", SqlDbType.DateTime).Value = modelAppointment.AppointmentTime;
            cmd.Parameters.Add("@DiseaseID", SqlDbType.Int).Value = modelAppointment.DiseaseID;



            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelAppointment.AppointmentID == null)
                {
                    TempData["AppointmentInsertMsg"] = "Appointment Inserted Succesfully";
                }
                else
                {
                    TempData["AppointmentInsertMsg"] = "Appointment Updated Succesfully";

                }

            }

            conn.Close();

            return View("AppointmentAddEdit");
        }

        public IActionResult Add(int? AppointmentID)
        {
            if (AppointmentID != null)
            {
                String str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Appointment_SelectByPK";
                cmd.Parameters.Add("@AppointmentID", SqlDbType.Int).Value = AppointmentID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);

                AppointmentModel modelAppointment = new AppointmentModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelAppointment.AppointmentID = Convert.ToInt32(dr["AppointmentID"]);
                    modelAppointment.PatientID = Convert.ToInt32(dr["PatientID"]);
                    modelAppointment.DoctorID = Convert.ToInt32(dr["DoctorID"]);
                    modelAppointment.AppointmentTypeID = Convert.ToInt32(dr["AppointmentTypeID"]);
                    modelAppointment.AppointmentDate = Convert.ToDateTime(dr["AppointmentDate"]);
                    modelAppointment.AppointmentTime = Convert.ToDateTime(dr["AppointmentTime"]);
                    modelAppointment.DiseaseID = Convert.ToInt32(dr["DiseaseID"]);




                }
                return View("AppointmentAddEdit", modelAppointment);

            }


            return View("AppointmentAddEdit");
        }
    }
}
