namespace Project_Doctor_Appointment.DAL
{
    public class DAL_Helper
    {
        public static string MyConnectionStr = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetConnectionString("myConnectionString");
    }
}
