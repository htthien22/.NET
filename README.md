using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OracleClient;

namespace DB_Helper
{
    public class DBHelper
    {
        public string strConect = string.Empty;
        public OracleConnection conn = new OracleConnection();
        public OracleCommand oCmd = new OracleCommand();
        public OracleDataAdapter oAdp = new OracleDataAdapter();
        public DBHelper()
        {

            strConect = "User ID=none;Password=none;Data Source=none;INTEGRATED Security = FALSE";
            conn = new OracleConnection(strConect);
            if (conn.State.ToString().ToUpper() == "CLOSED")
            {
                conn.Open();
            }
        }

        public int Close()
        {
            if (conn.State.ToString().ToUpper() == "OPEN")
            {
                conn.Close();
            }
            return 0;
        }
        public DataTable DoSelectSQL(string strSQL)
        {

            DataTable dt = new DataTable();
            oCmd = conn.CreateCommand();
            oCmd.CommandText = strSQL;
            oAdp.SelectCommand = oCmd;
            oAdp.Fill(dt);
            oCmd.Dispose();
            return (dt);
        }


        public int DoNonQuerySQL(string strSQL)
        {
            oCmd.Connection = conn;
            oCmd.CommandText = strSQL;
            int res = oCmd.ExecuteNonQuery();
            return res;
        }

        public void BeginTrans()
        {
            oCmd.Transaction = conn.BeginTransaction();
        }
        public void Commit()
        {
            oCmd.Transaction.Commit();
        }
        public void RollBack()
        {
            oCmd.Transaction.Rollback();
        }

    }
}
