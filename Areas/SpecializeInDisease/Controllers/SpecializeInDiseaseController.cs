using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project_Doctor_Appointment.Areas.SpecializeInDisease.Models;

namespace Project_Doctor_Appointment.Areas.SpecializeInDisease.Controllers
{
    [Area("SpecializeInDisease")]
    [Route("SpecializeInDisease/{Controller}/{action}")]
    public class SpecializeInDiseaseController : Controller
    {
        private IConfiguration Configuration;
        public SpecializeInDiseaseController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_SpecializeInDisease_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);


            return View("SpecializeInDiseaseList", dt);
        }


        public IActionResult Delete(int SpecializeInDiseaseID)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_SpecializeInDisease_Delete";
            cmd.Parameters.AddWithValue("@SpecializeInDiseaseID", SpecializeInDiseaseID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(SpecializeInDiseaseModel modelSpecializeInDisease)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelSpecializeInDisease.SpecializeInDiseaseID == null)
            {
                cmd.CommandText = "PR_SpecializeInDisease_Insert";
            }
            else
            {
                cmd.CommandText = "PR_SpecializeInDisease_Update";
                cmd.Parameters.Add("@SpecializeInDiseaseID", SqlDbType.Int).Value = modelSpecializeInDisease.SpecializeInDiseaseID;

            }
            cmd.Parameters.Add("@SpecializeIn", SqlDbType.VarChar).Value = modelSpecializeInDisease.SpecializeIn;

            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelSpecializeInDisease.SpecializeInDiseaseID == null)
                {
                    TempData["SpecializeInDiseaseInsertMsg"] = "SpecializeInDisease Inserted Succesfully";
                }
                else
                {
                    TempData["SpecializeInDiseaseInsertMsg"] = "SpecializeInDisease Updated Succesfully";

                }

            }

            conn.Close();

            return RedirectToAction("Index");
        }

        public IActionResult Add(int? SpecializeInDiseaseID)
        {
            if (SpecializeInDiseaseID != null)
            {
                String str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_SpecializeInDisease_SelectByPK";
                cmd.Parameters.Add("@SpecializeInDiseaseID", SqlDbType.Int).Value = SpecializeInDiseaseID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);

                SpecializeInDiseaseModel modelSpecializeInDisease = new SpecializeInDiseaseModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelSpecializeInDisease.SpecializeInDiseaseID = Convert.ToInt32(dr["SpecializeInDiseaseID"]);
                    modelSpecializeInDisease.SpecializeIn = dr["SpecializeIn"].ToString();


                }
                return View("SpecializeInDiseaseAddEdit", modelSpecializeInDisease);

            }


            return View("SpecializeInDiseaseAddEdit");
        }
    }
}
