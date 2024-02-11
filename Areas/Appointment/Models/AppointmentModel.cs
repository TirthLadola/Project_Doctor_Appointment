namespace Project_Doctor_Appointment.Areas.Appointment.Models
{
    public class AppointmentModel
    {
        public int? AppointmentID { get; set; }

        public int PatientID { get; set; }

        public int DoctorID { get; set; }

        public int AppointmentTypeID { get; set; }

        public DateTime AppointmentDate { get; set; }

        public DateTime AppointmentTime { get; set; }


        public int DiseaseID { get; set; }

        public DateTime Created { get; set; }

        public DateTime Modified { get; set; }
    }
}
