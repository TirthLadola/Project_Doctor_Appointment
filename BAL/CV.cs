namespace Project_Doctor_Appointment.BAL
{
    public class CV
    {
        private static IHttpContextAccessor _contextAccessor;
        static CV()
        {
            _contextAccessor = new HttpContextAccessor();
        }

        public static string? UserName()
        {
            string? UserName = null;
            if (_contextAccessor.HttpContext.Session.GetString("UserName") != null)
            {
                UserName = _contextAccessor.HttpContext.Session.GetString("UserName").ToString();
            }
            return UserName;
        }


    }
}
