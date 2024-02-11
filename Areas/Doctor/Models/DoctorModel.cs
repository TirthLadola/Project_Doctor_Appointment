namespace Project_Doctor_Appointment.Areas.Doctor.Models
{
    public class DoctorModel
    {
        public int? DoctorID { get; set; }

        public int SpecializeInDiseaseID { get; set; }

        public string Name { get; set; }

        public string MobileNo { get; set; }

        public DateTime BirthDate { get; set; }

        public string Email { get; set; }

        public string Address { get; set; }

        public string Degree { get; set; }

        public string Gender { get; set; }

        public int Experience { get; set; }

        public string HospitalName { get; set; }

        public string BloodGroup { get; set; }

        public DateTime JoiningDate { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
