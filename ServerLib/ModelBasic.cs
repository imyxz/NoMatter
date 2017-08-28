using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;
namespace ServerLib
{
    /// <summary>
    /// 提供Model层与mysql的接口
    /// </summary>
    class ModelBasic
    {
        protected MySqlConnection connect;
        public ModelBasic(MySqlConnection connect)
        {
            this.connect = connect;
        }
        public ModelResult Query(string query)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connect;
            cmd.CommandText = query;
            var reader = cmd.ExecuteReader();
            var result = new ModelResult();
            while(reader.Read())
            {
                var sub_result = new Dictionary<string, string>();
                for (int i=0;i<reader.FieldCount;i++)
                {
                    sub_result[reader.GetName(i)] = reader.GetString(i);
                }
                result.Add(sub_result);
            }
            return result;
        }
        public ModelResult QueryStmt(string query,params string [] values)
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = connect;
            cmd.CommandText = query;
            cmd.Prepare();
            for(int i=0;i<values.Length;i++)
            {
                cmd.Parameters.AddWithValue("@"+i.ToString(), values[i]);
            }
            var reader = cmd.ExecuteReader();
            var result = new ModelResult();
            while (reader.Read())
            {
                var sub_result = new Dictionary<string, string>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    sub_result[reader.GetName(i)] = reader.GetString(i);
                }
                result.Add(sub_result);
            }
            return result;
        }
    }
}
