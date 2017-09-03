using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JSON;
using CommonClass;
namespace ServerLib
{
    class MatterController : ControllerBasic
    {
        public MatterController() : base()
        {

        }
        public override Dictionary<string, Func<HttpServerContext, string>> GetActions()
        {
            var actions = new Dictionary<string, Func<HttpServerContext, string>>();
            actions["getMatter"] = GetMatter;
            actions["saveMatter"] = SaveMatter;
            actions["getMatters"] = GetMatters;
            actions["getFolders"] = GetFolders;
            actions["deleteMatter"] = DeleteMatter;
            actions["finishMatter"] = FinishMatter;
            actions["addFolder"] = AddFolder;
            actions["deleteFolder"] = DeleteFolder;
            return actions;
        }
        public string GetMatter(HttpServerContext context)
        {
            try
            {

                var matter_model = new MatterModel(context.DBConnection);
                int matter_id = 0;
                Int32.TryParse(context.Arguments["id"] ?? "0", out matter_id);
                if (matter_id <= 0) throw new InvalidRequest(1, "错误的matter_id");
                var matter_info = matter_model.GetMatterByID(matter_id);
                var json = new Json();
                json["matter_info"] = Json.ConvertFrom<MatterBasic>(matter_info);
                matter_info = json["matter_info"].ConvertTo<MatterPeriodicity>();
                context.Write(GenerateResponse(json).Encode());
            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string FinishMatter(HttpServerContext context)
        {
            try
            {
                if (context.session.Data["user_id"] <= 0)
                    throw new InvalidRequest(1, "您还未登录");
                var matter_model = new MatterModel(context.DBConnection);
                var matter_id = (int)Json.Decode(context.Request)["matter_id"];
                var db_matter = matter_model.GetMatterByID(matter_id);
                if (db_matter == null)
                {
                    throw new InvalidRequest(1, "无此matter");
                }
                else
                {
                    if (db_matter.user_id != (int)context.session.Data["user_id"]) throw new InvalidRequest(1, "您没有权限");
                    if (db_matter.OnUserFinish())
                        matter_model.SetMatterStatus(matter_id, 1);
                    else
                        matter_model.SaveMatter(db_matter);


                }

                context.Write(GenerateResponse(new Json()).Encode());
            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string SaveMatter(HttpServerContext context)
        {
            try
            {
                if (context.session.Data["user_id"] <= 0)
                    throw new InvalidRequest(1, "您还未登录");
                var matter_model = new MatterModel(context.DBConnection);
                var matter = MatterBasic.ParseFromJson(Json.Decode(context.Request));
                var db_matter = matter_model.GetMatterByID(matter.matter_id);
                if (db_matter == null)
                {
                    matter.user_id = (int)context.session.Data["user_id"];
                    matter_model.NewMatter(matter);
                }
                else
                {
                    if (matter.user_id != (int)context.session.Data["user_id"]) throw new InvalidRequest(1, "您没有权限");
                    matter_model.SaveMatter(matter);
                }

                context.Write(GenerateResponse(new Json()).Encode());
            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string DeleteMatter(HttpServerContext context)
        {
            try
            {
                if (context.session.Data["user_id"] <= 0)
                    throw new InvalidRequest(1, "您还未登录");
                var matter_model = new MatterModel(context.DBConnection);
                var matter_id = (int)Json.Decode(context.Request)["matter_id"];
                var db_matter = matter_model.GetMatterByID(matter_id);
                if (db_matter == null)
                {
                    throw new InvalidRequest(1, "无此matter");
                }
                else
                {
                    if (db_matter.user_id != (int)context.session.Data["user_id"]) throw new InvalidRequest(1, "您没有权限");
                    matter_model.SetMatterStatus(matter_id, 1);
                }

                context.Write(GenerateResponse(new Json()).Encode());
            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string GetMatters(HttpServerContext context)
        {
            try
            {
                var mattermodel = new MatterModel(Connection);
                var user_id = (int?)context.session.Data["user_id"] ?? 0;

                if (user_id <= 0) throw new InvalidRequest(1, "还未登录");
                var matters = mattermodel.GetUserMatters(user_id);

                var tmp = new Json();
                tmp["matters"] = Json.Import<MatterBasic>(matters);
                context.Write(GenerateResponse(tmp).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string GetFolders(HttpServerContext context)
        {
            try
            {
                var mattermodel = new MatterModel(Connection);
                var user_id = (int?)context.session.Data["user_id"] ?? 0;

                if (user_id <= 0) throw new InvalidRequest(1, "还未登录");
                var folders = mattermodel.GetUserFolders(user_id);

                var tmp = new Json();
                tmp["folders"] = Json.Import<MatterFolder>(folders);
                context.Write(GenerateResponse(tmp).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string AddFolder(HttpServerContext context)
        {
            try
            {
                var mattermodel = new MatterModel(Connection);
                var user_id = (int?)context.session.Data["user_id"] ?? 0;

                if (user_id <= 0) throw new InvalidRequest(1, "还未登录");
                string folder_name = Json.Decode(context.Request)["folder_name"];
                if (folder_name == null || folder_name == "") throw new InvalidRequest(1, "foldername不能为空");
                mattermodel.AddFolder(user_id, folder_name);
                context.Write(GenerateResponse(new Json()).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
        public string DeleteFolder(HttpServerContext context)
        {
            try
            {
                var mattermodel = new MatterModel(Connection);
                var user_id = (int?)context.session.Data["user_id"] ?? 0;

                if (user_id <= 0) throw new InvalidRequest(1, "还未登录");
                int folder_id = (int)Json.Decode(context.Request)["folder_id"];
                var db_folder = mattermodel.GetFolderByID(folder_id);
                if (db_folder == null || db_folder.user_id != user_id) throw new InvalidRequest(1, "您没有权限");
                mattermodel.DeleteFolder(folder_id);
                context.Write(GenerateResponse(new Json()).Encode());

            }
            catch (InvalidRequest a)
            {
                context.Write(GenerateResponse(a.Code, a.Message, null).Encode());
            }
            return null;

        }
    }
}
