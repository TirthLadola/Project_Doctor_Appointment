using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Data.Common;
using System.Data;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace Project_Doctor_Appointment.DAL
{
    public class SEC_User_DALBase : DAL_Helper
    {
        public DataTable PR_SEC_User_SelectBYUserNamePassword(string UserName, string Password)
        {
            try
            {
                SqlDatabase sqlDB = new SqlDatabase(MyConnectionStr);
                DbCommand cmd = sqlDB.GetStoredProcCommand("SelectByPasswordAndUseName");
                sqlDB.AddInParameter(cmd, "@UserName", DbType.String, UserName);
                sqlDB.AddInParameter(cmd, "@Password", DbType.String, Password);
                DataTable dt = new DataTable();
                using (IDataReader reader = sqlDB.ExecuteReader(cmd))
                {
                    dt.Load(reader);
                }
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
                return null;
            }
        }

    }
}
