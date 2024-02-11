namespace Project_Doctor_Appointment.Areas.Disease.Models
{
    public class DiseaseModel
    {
        public int? DiseaseID { get; set; }

        public string DiseaseName { get; set; }


        public DateTime Created { get; set; }


        public DateTime Modified { get; set; }


    }

    public class DiseaseDropDown
    {
        public int DiseaseID { get; set; }

        public string DiseaseName { get; set; }

    }
}
