using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project_Doctor_Appointment.Areas.Doctor.Models;
using Project_Doctor_Appointment.Areas.SpecializeInDisease.Models;

namespace Project_Doctor_Appointment.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Route("Doctor/{Controller}/{action}")]
    public class DoctorController : Controller
    {
        private IConfiguration Configuration;
        public DoctorController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_Doctor_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);


            return View("DoctorList", dt);
        }


        public IActionResult Delete(int DoctorID)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Doctor_Delete";
            cmd.Parameters.AddWithValue("@DoctorID", DoctorID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(DoctorModel modelDoctor)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelDoctor.DoctorID == null)
            {
                cmd.CommandText = "PR_Doctor_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Doctor_Update";
                cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = modelDoctor.DoctorID;

            }
            cmd.Parameters.Add("@SpecializeInDiseaseID", SqlDbType.Int).Value = modelDoctor.SpecializeInDiseaseID;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = modelDoctor.Name;

            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = modelDoctor.MobileNo;

            cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = modelDoctor.BirthDate;

            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = modelDoctor.Email;




            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = modelDoctor.Address;
            cmd.Parameters.Add("@Degree", SqlDbType.VarChar).Value = modelDoctor.Degree;

            cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = modelDoctor.Gender;

            cmd.Parameters.Add("@Experience", SqlDbType.Int).Value = modelDoctor.Experience;

            cmd.Parameters.Add("@HospitalName", SqlDbType.VarChar).Value = modelDoctor.HospitalName;

            cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = modelDoctor.BloodGroup;

            cmd.Parameters.Add("@JoiningDate", SqlDbType.DateTime).Value = modelDoctor.JoiningDate;



            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelDoctor.DoctorID == null)
                {
                    TempData["DoctorInsertMsg"] = "Doctor Inserted Succesfully";
                }
                else
                {
                    TempData["DoctorInsertMsg"] = "Doctor Updated Succesfully";

                }

            }

            conn.Close();

            return RedirectToAction("Index");
        }

        public IActionResult Add(int? DoctorID)
        {
            string myconnStr1 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection1 = new SqlConnection(myconnStr1);
            DataTable dt1 = new DataTable();
            connection1.Open();
            SqlCommand cmd1 = connection1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_Specializein_DropDown";
            SqlDataReader reader1 = cmd1.ExecuteReader();
            dt1.Load(reader1);

            List<SpecializationDropdown> list = new List<SpecializationDropdown>();
            foreach (DataRow dr in dt1.Rows)
            {
                SpecializationDropdown lstList = new SpecializationDropdown();
                lstList.SpecializeInDiseaseID = Convert.ToInt32(dr["SpecializeInDiseaseID"]);
                lstList.SpecializeIn = dr["SpecializeIn"].ToString();
                list.Add(lstList);
            }
            ViewBag.SpecializationInList = list;

            if (DoctorID != null)
            {
                String str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Doctor_SelectByPK";
                cmd.Parameters.Add("@DoctorID", SqlDbType.Int).Value = DoctorID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);

                DoctorModel modelDoctor = new DoctorModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelDoctor.DoctorID = Convert.ToInt32(dr["DoctorID"]);
                    modelDoctor.SpecializeInDiseaseID = Convert.ToInt32(dr["SpecializeInDiseaseID"]);

                    modelDoctor.Name = dr["Name"].ToString();
                    modelDoctor.MobileNo = dr["MobileNo"].ToString();

                    modelDoctor.BirthDate = Convert.ToDateTime(dr["BirthDate"]);

                    modelDoctor.Email = dr["Email"].ToString();
                    modelDoctor.Address = dr["Address"].ToString();
                    modelDoctor.Degree = dr["Degree"].ToString();
                    modelDoctor.Gender = dr["Gender"].ToString();
                    modelDoctor.Experience = Convert.ToInt32(dr["Experience"]);

                    modelDoctor.HospitalName = dr["HospitalName"].ToString();
                    modelDoctor.BloodGroup = dr["BloodGroup"].ToString();

                    modelDoctor.JoiningDate = Convert.ToDateTime(dr["JoiningDate"]);




                }
                return View("DoctorAddEdit", modelDoctor);

            }


            return View("DoctorAddEdit");
        }
    }
}
