namespace Project_Doctor_Appointment.Areas.SEC_User.Models
{
    public class SEC_UserModel
    {
        public int UserID { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FirstName { get; set; }

        public string? LastName { get; set; }

        public string Email { get; set; }

        public string? PhotoPath { get; set; }


        public DateTime Cretaed { get; set; }

        public DateTime Modified { get; set; }



    }
}
