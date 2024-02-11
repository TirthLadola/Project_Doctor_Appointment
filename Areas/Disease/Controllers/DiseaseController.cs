using Microsoft.AspNetCore.Mvc;
using Project_Doctor_Appointment.Areas.Disease.Models;
using System.Data;
using System.Data.SqlClient;

namespace Project_Doctor_Appointment.Areas.Disease.Controllers
{
    [Area("Disease")]
    [Route("Disease/{Controller}/{action}")]
    public class DiseaseController : Controller
    {
        private IConfiguration Configuration;
        public DiseaseController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_Disease_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);


            return View("DiseaseList", dt);
        }


        public IActionResult Delete(int DiseaseID)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Disease_Delete";
            cmd.Parameters.AddWithValue("@DiseaseID", DiseaseID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(DiseaseModel modelDisease)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelDisease.DiseaseID == null)
            {
                cmd.CommandText = "PR_Disease_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Disease_Update";
                cmd.Parameters.Add("@DiseaseID", SqlDbType.Int).Value = modelDisease.DiseaseID;

            }
            cmd.Parameters.Add("@DiseaseName", SqlDbType.VarChar).Value = modelDisease.DiseaseName;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelDisease.DiseaseID == null)
                {
                    TempData["DiseaseInsertMsg"] = "Disease Inserted Succesfully";
                }
                else
                {
                    TempData["DiseaseInsertMsg"] = "Disease Updated Succesfully";

                }

            }

            conn.Close();

            return RedirectToAction("Index");
        }

        public IActionResult Add(int? DiseaseID)
        {
            if (DiseaseID != null)
            {
                String str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Disease_SelectByPK";
                cmd.Parameters.Add("@DiseaseID", SqlDbType.Int).Value = DiseaseID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);

                DiseaseModel modelDisease = new DiseaseModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelDisease.DiseaseID = Convert.ToInt32(dr["DiseaseID"]);
                    modelDisease.DiseaseName = dr["DiseaseName"].ToString();


                }
                return View("DiseaseAddEdit", modelDisease);

            }


            return View("DiseaseAddEdit");
        }
    }

}

