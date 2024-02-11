using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project_Doctor_Appointment.Areas.Patient.Models;
using Project_Doctor_Appointment.Areas.Disease.Models;

namespace Project_Doctor_Appointment.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Route("Patient/{Controller}/{action}")]
    public class PatientController : Controller
    {
        private IConfiguration Configuration;
        public PatientController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_Patient_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);


            return View("PatientList", dt);
        }


        public IActionResult Delete(int PatientID)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Patient_Delete";
            cmd.Parameters.AddWithValue("@PatientID", PatientID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(PatientModel modelPatient)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelPatient.PatientID == null)
            {
                cmd.CommandText = "PR_Patient_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Patient_Update";
                cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = modelPatient.PatientID;

            }
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = modelPatient.Name;
            cmd.Parameters.Add("@MobileNo", SqlDbType.VarChar).Value = modelPatient.MobileNo;
            cmd.Parameters.Add("@BirthDate", SqlDbType.DateTime).Value = modelPatient.BirthDate;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar).Value = modelPatient.Email;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar).Value = modelPatient.Address;
            cmd.Parameters.Add("@DiseaseID", SqlDbType.Int).Value = modelPatient.DiseaseID;
            cmd.Parameters.Add("@Gender", SqlDbType.VarChar).Value = modelPatient.Gender;
            cmd.Parameters.Add("@BloodGroup", SqlDbType.VarChar).Value = modelPatient.BloodGroup;
            cmd.Parameters.Add("@Weight", SqlDbType.Decimal).Value = modelPatient.Weight;


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelPatient.PatientID == null)
                {
                    TempData["PatientInsertMsg"] = "Patient Inserted Succesfully";
                }
                else
                {
                    TempData["PatientInsertMsg"] = "Patient Updated Succesfully";

                }

            }

            conn.Close();

            return RedirectToAction("Index");
        }

        public IActionResult Add(int? PatientID)
        {
            string myconnStr1 = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection connection1 = new SqlConnection(myconnStr1);
            DataTable dt1 = new DataTable();
            connection1.Open();
            SqlCommand cmd1 = connection1.CreateCommand();
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.CommandText = "PR_Disease_DropDown";
            SqlDataReader reader1 = cmd1.ExecuteReader();
            dt1.Load(reader1);

            List<DiseaseDropDown> list = new List<DiseaseDropDown>();
            foreach (DataRow dr in dt1.Rows)
            {
                DiseaseDropDown lstList = new DiseaseDropDown();
                lstList.DiseaseID = Convert.ToInt32(dr["DiseaseID"]);
                lstList.DiseaseName = dr["DiseaseName"].ToString();
                list.Add(lstList);
            }
            ViewBag.DiseaseList = list;


            if (PatientID != null)
            {
                String str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Patient_SelectByPK";
                cmd.Parameters.Add("@PatientID", SqlDbType.Int).Value = PatientID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);

                PatientModel modelPatient = new PatientModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelPatient.PatientID = Convert.ToInt32(dr["PatientID"]);
                    modelPatient.Name = dr["Name"].ToString();
                    modelPatient.MobileNo = dr["MobileNo"].ToString();
                    modelPatient.BirthDate = Convert.ToDateTime(dr["BirthDate"]);
                    modelPatient.Email = dr["Email"].ToString();
                    modelPatient.Address = dr["Address"].ToString();
                    modelPatient.DiseaseID = Convert.ToInt32(dr["DiseaseID"]);
                    modelPatient.Gender = dr["Gender"].ToString();
                    modelPatient.BloodGroup = dr["BloodGroup"].ToString();
                    modelPatient.Weight = Convert.ToDecimal(dr["Weight"]);


                }
                return View("PatientAddEdit", modelPatient);

            }


            return View("PatientAddEdit");
        }
    }
}
