using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Project_Doctor_Appointment.Areas.Photos.Models;

namespace Project_Doctor_Appointment.Areas.Photos.Controllers
{
    [Area("Photo")]
    [Route("Photo/{Controller}/{action}")]
    public class PhotoController : Controller
    {
        private IConfiguration Configuration;
        public PhotoController(IConfiguration _configuration)
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
            cmd.CommandText = "PR_Photo_SelectAll";
            DataTable dt = new DataTable();
            SqlDataReader objSDR = cmd.ExecuteReader();
            dt.Load(objSDR);


            return View("PhotoList", dt);
        }


        public IActionResult Delete(int PhotoID)
        {
            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "PR_Photo_Delete";
            cmd.Parameters.AddWithValue("@PhotoID", PhotoID);
            cmd.ExecuteNonQuery();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Save(PhotoModel modelPhoto)
        {
            if (modelPhoto.File != null)
            {
                string FilePath = "wwwroot\\Images";
                string path = Path.Combine(Directory.GetCurrentDirectory(), FilePath);

                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string fileNameWithPath = Path.Combine(path, modelPhoto.File.FileName);
                modelPhoto.PhotoPath = "~" + FilePath.Replace("wwwroot\\", "/") + "/" + modelPhoto.File.FileName;

                using (var stream = new FileStream(fileNameWithPath, FileMode.Create))
                {
                    modelPhoto.File.CopyTo(stream);
                }
            }

            String str = this.Configuration.GetConnectionString("myConnectionString");
            SqlConnection conn = new SqlConnection(str);
            conn.Open();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;


            if (modelPhoto.PhotoID == null)
            {
                cmd.CommandText = "PR_Photo_Insert";
            }
            else
            {
                cmd.CommandText = "PR_Photo_Update";
                cmd.Parameters.Add("@PhotoID", SqlDbType.Int).Value = modelPhoto.PhotoID;

            }
            cmd.Parameters.Add("@Name", SqlDbType.VarChar).Value = modelPhoto.Name;
            cmd.Parameters.Add("@PhotoPath", SqlDbType.VarChar).Value = modelPhoto.PhotoPath;


            if (Convert.ToBoolean(cmd.ExecuteNonQuery()))
            {
                if (modelPhoto.PhotoID == null)
                {
                    TempData["PhotoInsertMsg"] = "Photo Inserted Succesfully";
                }
                else
                {
                    TempData["PhotoInsertMsg"] = "Photo Updated Succesfully";

                }

            }

            conn.Close();

            return RedirectToAction("Index");
        }

        public IActionResult Add(int? PhotoID)
        {
            if (PhotoID != null)
            {
                String str = this.Configuration.GetConnectionString("myConnectionString");
                SqlConnection conn = new SqlConnection(str);
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "PR_Photo_SelectByPK";
                cmd.Parameters.Add("@PhotoID", SqlDbType.Int).Value = PhotoID;
                DataTable dt = new DataTable();
                SqlDataReader objSDR = cmd.ExecuteReader();
                dt.Load(objSDR);

                PhotoModel modelPhoto = new PhotoModel();

                foreach (DataRow dr in dt.Rows)
                {

                    modelPhoto.PhotoID = Convert.ToInt32(dr["PhotoID"]);
                    modelPhoto.Name = dr["Name"].ToString();
                    modelPhoto.PhotoPath = dr["PhotoPath"].ToString();



                }
                return View("PhotoAddEdit", modelPhoto);

            }


            return View("PhotoAddEdit");
        }
    }
}
