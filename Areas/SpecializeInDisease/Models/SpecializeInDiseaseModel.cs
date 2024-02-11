namespace Project_Doctor_Appointment.Areas.SpecializeInDisease.Models
{
    public class SpecializeInDiseaseModel
    {
        public int? SpecializeInDiseaseID { get; set; }

        public string SpecializeIn { get; set; }

        public DateTime Created { get; set; }


        public DateTime Modified { get; set; }

    }

    public class SpecializationDropdown
    {

        public int SpecializeInDiseaseID { get; set; }

        public string SpecializeIn { get; set; }
    }
}
