namespace Project_Doctor_Appointment.Areas.Photos.Models
{
    public class PhotoModel
    {

        public int? PhotoID { get; set; }

        public string PhotoPath { get; set; }

        public string Name { get; set; }

        public IFormFile? File { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }

    }
}
