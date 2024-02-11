namespace Project_Doctor_Appointment.Areas.Patient.Models
{
    public class PatientModel
    {
        public int? PatientID { get; set; }

        public string Name { get; set; }

        public string MobileNo { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public int DiseaseID { get; set; }
        public string Gender { get; set; }


        public string BloodGroup { get; set; }


        public Decimal Weight { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }



    }
}
