using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql;
using MySql.Data.MySqlClient;
using CommonClass;
using JSON;
namespace ServerLib
{
    class MatterModel:ModelBasic
    {
        public MatterModel(MySqlConnection connect) : base(connect)
        {
        }
        public MatterBasic GetMatterByID(int matter_id)
        {
            var result = QueryStmt("select * from user_matters where matter_id=@0 limit 1", matter_id.ToString());
            if (result.Count > 0)
            {
                var info = result[0];
                return MatterBasic.ParseFromDB(info);

            }
            else
            {
                return null;
            }
        }
        public bool SaveMatter(MatterBasic matter)
        {
            QueryStmt("update user_matters set matter_name=@0,matter_desc=@1,matter_start_time=@2,matter_next_effect_time=@3," +
                "matter_end_time=@4,matter_addion_info=@5 where matter_id=@6",
                matter.matter_name, matter.matter_desc, matter.matter_start_time.ToString(), matter.matter_next_effect_time.ToString(), matter.matter_end_time.ToString(),
                matter.matter_addion_info.Encode(), matter.matter_id.ToString());
            return true;
        }
        public bool NewMatter(MatterBasic matter)
        {
            QueryStmt("insert into user_matters set matter_name=@0,matter_desc=@1,matter_start_time=@2,matter_next_effect_time=@3," +
                "matter_end_time=@4,matter_addion_info=@5 , matter_id=@6,matter_type=@7,user_id=@8",
                matter.matter_name, matter.matter_desc, matter.matter_start_time.ToString(), matter.matter_next_effect_time.ToString(), matter.matter_end_time.ToString(),
                matter.matter_addion_info.Encode(), matter.matter_id.ToString(),matter.matter_type.ToString(),matter.user_id.ToString());
            return true;
        }
        public List<MatterBasic> GetUserMatters(int user_id)
        {
            var result = QueryStmt(@"select * from user_matters where user_id=@0", user_id.ToString());
            var ret = new List<MatterBasic>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(MatterBasic.ParseFromDB(result[i]));
            }
            return ret;

        }
        public List<MatterFolder> GetUserFolders(int user_id)
        {
            var result = QueryStmt(@"select * from matter_folder where user_id=@0", user_id.ToString());
            var ret = new List<MatterFolder>();
            for (int i = 0; i < result.Count; i++)
            {
                ret.Add(Json.Import<string,string>(result[i]).ConvertTo<MatterFolder>());
            }
            return ret;
        }
    }
}
