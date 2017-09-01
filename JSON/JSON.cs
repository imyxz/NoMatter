using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace JSON
{
    public class Json:IDictionary<string,Json>
    {
        private Dictionary<string, Json> childrens = new Dictionary<string, Json>();
        private string single_value;
        private static int max_depth = 512;
        private JSONChildrenType childrenType = new JSONChildrenType();

        public ICollection<string> Keys => ((IDictionary<string, Json>)childrens).Keys;

        public ICollection<Json> Values => ((IDictionary<string, Json>)childrens).Values;

        public int Count => ((IDictionary<string, Json>)childrens).Count;

        public bool IsReadOnly => ((IDictionary<string, Json>)childrens).IsReadOnly;


        #region Json Constructor
        public Json()
        {
            childrenType = JSONChildrenType.NULL;
        }
        public Json(string value)
        {
            single_value = value;
            childrenType = JSONChildrenType.STRING;
        }
        public Json(double value)
        {
            single_value = value.ToString();
            childrenType = JSONChildrenType.DOUBLE;
        }
        public Json(int value)
        {
            single_value = value.ToString();
            childrenType = JSONChildrenType.INT;
        }
        public Json(bool value)
        {
            single_value = value.ToString();
            childrenType = JSONChildrenType.BOOLEAN;
        }
        public Json(object value)
        {
            single_value = value.ToString();
            childrenType = JSONChildrenType.STRING;
        }
        #endregion

#region Json [] operator
        public Json this[string index]
        {
            get
            {
                if(!childrens.ContainsKey(index))
                {
                    childrens[index] = new Json();
                }
                childrenType = JSONChildrenType.DICTIONARY;
                return childrens[index];
            }
            set
            {
                childrens[index] = value;
                single_value = "";
                childrenType = JSONChildrenType.DICTIONARY;
            }
        }
        public Json this[int index]
        {
            get
            {
                return this[index.ToString()];
            }
            set
            {
                this[index.ToString()] = value;
            }
        }
#endregion

        #region Json Importor for Dictionary And List
        public static Json Import<T1,T2>(IDictionary<T1,T2> dic)
        {
            Json ret = new Json();
            
            if (typeof(T2) == typeof(int))
            {
                foreach (KeyValuePair<T1, T2> pair in dic)
                {
                    if (pair.Value is IJsonAble ijsonable)
                        ret[pair.Key.ToString()] = ijsonable.toJson();
                    else
                    {
                        ret[pair.Key.ToString()] = new Json((int)(object)pair.Value);
                    }
                }
            }
            else if (typeof(T2) == typeof(double))
            {
                foreach (KeyValuePair<T1, T2> pair in dic)
                {
                    if (pair.Value is IJsonAble ijsonable)
                        ret[pair.Key.ToString()] = ijsonable.toJson();
                    else
                    {
                        ret[pair.Key.ToString()] = new Json((double)(object)pair.Value);
                    }
                }
            }
            else if (typeof(T2) == typeof(bool))
            {
                foreach (KeyValuePair<T1, T2> pair in dic)
                {
                    if (pair.Value is IJsonAble ijsonable)
                        ret[pair.Key.ToString()] = ijsonable.toJson();
                    else
                    {
                        ret[pair.Key.ToString()] = new Json((bool)(object)pair.Value);
                    }
                }
            }
            else
            {
                foreach (KeyValuePair<T1, T2> pair in dic)
                {
                    if (pair.Value is IJsonAble ijsonable)
                        ret[pair.Key.ToString()] = ijsonable.toJson();
                    else
                    { 
                        ret[pair.Key.ToString()] = new Json(pair.Value);
                    }
                }
            }
                
            

            ret.childrenType = JSONChildrenType.DICTIONARY;
            return ret;
        }

        public static Json Import<T>(IEnumerable<T> list)
        {
            Json ret = new Json();
            int cnt = 0;

            if (typeof(T) == typeof(int))
            {
                foreach (T value in list)
                {
                    if (value is IJsonAble ijsonable)
                        ret[cnt.ToString()] = ijsonable.toJson();
                    else
                        ret[cnt.ToString()] = new Json((int)(object)value);
                    cnt++;
                }
            }
            else if (typeof(T) == typeof(double))
            {
                foreach (T value in list)
                {
                    if (value is IJsonAble ijsonable)
                        ret[cnt.ToString()] = ijsonable.toJson();
                    else
                        ret[cnt.ToString()] = new Json((double)(object)value);
                    cnt++;
                }
            }
            else if (typeof(T) == typeof(bool))
            {
                foreach (T value in list)
                {
                    if (value is IJsonAble ijsonable)
                        ret[cnt.ToString()] = ijsonable.toJson();
                    else
                        ret[cnt.ToString()] = new Json((bool)(object)value);
                    cnt++;
                }
            }
            else
            {
                foreach (T value in list)
                {
                    if (value is IJsonAble ijsonable)
                        ret[cnt.ToString()] = ijsonable.toJson();
                    else
                        ret[cnt.ToString()] = new Json(value);
                    cnt++;
                }
            }



            
            ret.childrenType = JSONChildrenType.ARRAY;
            return ret;
        }
        #endregion
        #region Json Convertor
        public override string ToString()
        {
            return this.Encode();
        }
        
        public int ? ToInt()
        {
            int ret = 0;
            if (!IsReadAble() || !int.TryParse(single_value, out ret)) return null;
            return ret;
        }
        public double ? ToDouble()
        {
            double ret = 0;
            if (!IsReadAble() || !double.TryParse(single_value, out ret)) return null;
            return ret;
        }
        public bool ? ToBool()
        {
            bool ret = false;
            if (!IsReadAble() || !bool.TryParse(single_value, out ret)) return null;
            return ret;
        }
        public static Json ConvertFrom(object obj) 
        {
            return Json.ConvertFrom(obj.GetType(), obj, 0, null);
        }
        
        public static Json ConvertFrom<T>(T obj)//用于多态下的转换
        {
            return Json.ConvertFrom(typeof(T), obj, 0, null);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="Filter">用于过滤字段的函数，第一个参数为类名，第二个参数为属性名,若为非null则不调用IJsonable接口</param>
        /// <returns></returns>
        public static Json ConvertFrom(object obj, Func<string, string, bool> Filter)
        {
            return Json.ConvertFrom(obj.GetType(), obj, 0, Filter);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="Filter">用于过滤字段的函数，第一个参数为类名，第二个参数为属性名,若为非null则不调用IJsonable接口</param>
        /// <returns></returns>
        public static Json ConvertFrom<T>(T obj, Func<string, string, bool> Filter)//用于多态下的转换
        {
            return Json.ConvertFrom(typeof(T), obj,0, Filter);
        }
        private static Json ConvertFrom(Type type,object obj,int depth,Func<string,string,bool> Filter)
        {
            if (depth > max_depth)
                throw new Exception("递归层次超过阈值");
            if (Filter==null && obj is IJsonAble ijsonable)
                return ijsonable.toJson();
            else if(type.Namespace!=null&& !isOverideFrom("ToString",type,typeof(object)))//如果自身重写了ToString方法，则认为可以直接取它的ToString值，避免死循环 如果namespace是null，则为匿名类型
            {
                return new Json(obj.ToString());
            }
            else
            {
                Json ret = new Json();
                Type obj_type = type;
                foreach (MemberInfo member in obj_type.GetMembers(BindingFlags.Public | BindingFlags.Instance))
                {
                    if((Filter != null && Filter(obj_type.Name, member.Name)))//过滤函数
                    {
                        if (member.MemberType == MemberTypes.Field)//public 属性
                        {
                            object value = obj_type.InvokeMember(member.Name, 
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetField
                , null, obj, null);//获取属性值

                            if (value != null)
                            {
                                ret[member.Name] = Json.ConvertFrom(((FieldInfo)member).FieldType, value, depth+1, Filter);
                            }
                            else
                            {
                                //null
                            }
                        }
                        /*else if (member.MemberType == MemberTypes.Property)
                        {
                            object value = obj_type.InvokeMember(member.Name, BindingFlags.DeclaredOnly |
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.GetProperty
                , null, obj, null);//获取属性值

                            if (value != null)
                            {
                                ret[member.Name] = Json.ConvertFrom(((PropertyInfo)member).PropertyType, value, depth + 1, Filter);
                            }
                            else
                            {
                                //null
                                ret[member.Name] = new Json();
                            }
                        }*/
                    }
                    
                }
                return ret;

            }
        }
        private static bool isOverideFrom(string methodName,Type target,Type from)
        {
            bool isExist = false;
            foreach (MethodInfo info in target.GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                if(info.Name==methodName)
                {
                    isExist = true;
                    if (info.DeclaringType != from)
                        return false;
                }
                
            }
            return isExist;
            
        }
        public static implicit operator Json(string a)
        {
            return new Json(a);
        }
        public static implicit operator Json(int a)
        {
            return new Json(a);
        }
        public static implicit operator Json(double a)
        {
            return new Json(a);
        }
        public static implicit operator Json(bool a)
        {
            return new Json(a);
        }
        public static implicit operator int?(Json a)
        {
            return a.ToInt();
        }
        public static implicit operator double?(Json a)
        {
            return a.ToDouble();
        }
        public static implicit operator string(Json a)
        {
            if (!a.IsReadAble()) return null;
            return a.single_value;
        }
        public static implicit operator bool?(Json a)
        {
            return a.ToBool();
        }
        public T ConvertTo<T>() where T : new()
        {
            return ConvertTo<T>(null);
        }
        public T ConvertTo<T>(Func<string, string, bool> Filter) where T : new()
        {
            return (T)ConvertTo(typeof(T), this, 0, Filter);
        }

        private static Object ConvertTo(Type type,Json json, int depth, Func<string,string, bool> Filter)
        {
            
            if (depth > max_depth)
                throw new Exception("递归层次超过阈值");
            
            Object obj = null;
            try {
                //obj = type.GetConstructor(Type.EmptyTypes).Invoke(null);//对结构体无效
                if(typeof(string)==type)//string 类型无默认的构造函数
                    obj = "";
                else
                    obj = Activator.CreateInstance(type);//可对对象与结构体操作
            } catch { }
            if (obj == null) throw new Exception("对象无默认公共构造函数");
            if (json.IsNull())
                return obj;
            if (json.IsReadAble())//为单个值类型
            {
                if(type==typeof(string))//string类型无tryparse方法
                {
                    obj = string.Copy((string)json);
                    return obj;
                }else 
                if (type.BaseType == typeof(Enum))//enum类型要特殊处理
                {
                    obj = Enum.Parse(type, (string)json);
                    return obj;
                }
                else
                {
                    var tryparse = type.GetMethod("TryParse", BindingFlags.Public | BindingFlags.Static, Type.DefaultBinder,
                                    new Type[] { typeof(string), type.MakeByRefType() },
                                    new ParameterModifier[] { new ParameterModifier(2) });

                    var parameters = new object[] { (string)json, Activator.CreateInstance(type) };
                    if (tryparse == null)
                        throw new Exception("值类型" + type.FullName + "无tryparse函数");
                    bool success = (bool)(tryparse.Invoke(null, parameters) ?? false);
                    if (success)
                    {
                        obj = parameters[1];
                        return obj;
                    }
                    else
                        throw new Exception("有tryparse的情况下无法转换");
                }
            }


            if (Filter == null && obj is IJsonAble ijsonable)
            {
                obj=ijsonable.fromJson(json);
                return obj;
            }
            else
            {
                Json ret = new Json();
                Type obj_type = type;
                foreach (MemberInfo member in obj_type.GetMembers(BindingFlags.Public | BindingFlags.Instance))
                {
                    if ((Filter != null && json.ContainsKey(member.Name) && Filter(obj_type.Name, member.Name)))//过滤函数
                    {
                        if (member.MemberType == MemberTypes.Field)//public 属性
                        {
                            object[] value = new object[1];
                            value[0] = ConvertTo(((FieldInfo)member).FieldType, json[member.Name], depth + 1, Filter);
                            obj_type.InvokeMember(member.Name, 
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetField
                , null, obj, value);//设置属性
                        }
                        /*else if (member.MemberType == MemberTypes.Property)
                        {
                            object[] value = new object[1];
                            value[0] = ConvertTo(((PropertyInfo)member).PropertyType, json[member.Name], depth + 1, Filter);
                            obj_type.InvokeMember(member.Name, 
                BindingFlags.Public | BindingFlags.NonPublic |
                BindingFlags.Instance | BindingFlags.SetProperty
                , null, obj, value);//设置属性
                        }*/
                    }

                }
                return obj;

            }
        }
        #endregion
            #region Json Encoder
        public string Encode()
        {
            if (childrenType == JSONChildrenType.STRING)
                return "{" + Json.Filter(single_value,childrenType) + "}";
            else
                return _Encode(0);
        }
        private string _Encode(int depth)
        {
            if (depth > max_depth)
                throw new Exception("递归层次超过阈值");
            string ret = "";
            int cnt = 0;
            switch (childrenType)
            {
                case JSONChildrenType.ARRAY:
                     ret = "[";
                     cnt = 0;
                    foreach (KeyValuePair<string, Json> pair in childrens)
                    {
                        ret += pair.Value._Encode(depth + 1) + ",";
                        cnt++;
                    }
                    if (cnt > 0)
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                    }
                    return ret + "]";
                case JSONChildrenType.DICTIONARY:
                     ret = "{";
                     cnt = 0;
                    foreach (KeyValuePair<string, Json> pair in childrens)
                    {
                        ret += "" + Json.Filter(pair.Key, JSONChildrenType.STRING) + ": " + pair.Value._Encode(depth + 1) + ",";
                        cnt++;
                    }
                    if (cnt > 0)
                    {
                        ret = ret.Substring(0, ret.Length - 1);
                    }
                    return ret + "}";
                case JSONChildrenType.NULL:
                     
                    return "null";
                default:
                    return "" + Json.Filter(single_value, childrenType) + "";

            }
                

        }
        private static string Filter(string a,JSONChildrenType type)
        {
            string ret = a;
            switch(type)
            {
                case JSONChildrenType.STRING:
                    ret = ret.Replace("\\", "\\\\");
                    ret = ret.Replace("\"", "\\\"");
                    ret = "\"" + ret + "\"";
                    ret = Json.EncodeNonAsciiCharacters(ret);

                    break;
                case JSONChildrenType.BOOLEAN:
                    if (Convert.ToBoolean(ret) == true)
                        ret = "true";
                    else
                        ret = "false";
                    break;
               
            }
            return ret;
               
            
        }
        #endregion
        #region Json Decoder        
        public static Json Decode(string json)
        {
            Json ret;
            try
            {
                ret= _Decode(ref json);
            }
            catch
            {
                return null;
            }
            return ret;

        }
        private static Json _Decode(ref string json)
        {
            int tmp = 0;
            if (tmp<json.Length)
            {
                switch(json[tmp])
                {
                    case '{':
                        return _DecodeDictionary(ref json, 0, out tmp,0);
                    case '[':
                        return _DecodeArray(ref json, 0, out tmp,0);
                    case ' ':
                        break;
                    default:
                        return new Json();
                }
                tmp++;
            }
            throw new Exception("json解析错误");
        }
        private static Json _DecodeDictionary(ref string json, int start_pos, out int end_pos,int depth)
        {
            if (depth > max_depth)
                throw new Exception("递归层次超过阈值");
            end_pos = start_pos+1;
            Json ret = new Json();
            bool flag = false;
            string key = "";
            while (end_pos < json.Length)
            {
                if (json[end_pos] == '"')
                {
                    if (flag == false)
                    {
                        key = "";
                        if (!_DecodeGetString(ref json, end_pos, out end_pos, out key)) throw new Exception("json解析错误");
                        end_pos++;

                        while (end_pos < json.Length)
                        {
                            if (json[end_pos] == ':')
                                break;
                            end_pos++;
                        }
                        //找到:，接着循环
                        if (end_pos >= json.Length) throw new Exception("json解析错误");
                        flag = true;//表示接下来应该找值
                    }
                    else
                    {
                        string value = "";
                        if (!_DecodeGetString(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
                        ret[key] = value;
                        if (!_DecodeCheckNextCommo(ref json,'}',end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                        flag = false;
                    }
                }
                else if (json[end_pos] == '{')
                {
                    if (!flag) throw new Exception("json解析错误");
                    ret[key] = _DecodeDictionary(ref json, end_pos, out end_pos, depth+1);
                    if (!_DecodeCheckNextCommo(ref json, '}', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                    flag = false;
                }
                else if (json[end_pos] == '[')
                {
                    if (!flag) throw new Exception("json解析错误");
                    ret[key] = _DecodeArray(ref json, end_pos, out end_pos, depth + 1);
                    if (!_DecodeCheckNextCommo(ref json, '}', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                    flag = false;
                }
                else if(json[end_pos]>='0' && json[end_pos]<='9')
                {
                    if (!flag) throw new Exception("json解析错误");
                    string value = "";
                    if(!_DecodeGetNum(ref json, end_pos, out end_pos,out value)) throw new Exception("json解析错误");
                    ret[key] = value;
                    if (!_DecodeCheckNextCommo(ref json, '}', end_pos+1, out end_pos)) throw new Exception("json解析错误");
                    flag = false;
                }
                else if (json[end_pos]=='t' || json[end_pos] == 'T' || json[end_pos] == 'f' || json[end_pos] == 'F')
                {
                    if (!flag) throw new Exception("json解析错误");
                    bool value = false;
                    if (!_DecodeGetBool(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
                    ret[key] = value;
                    if (!_DecodeCheckNextCommo(ref json, '}', end_pos + 1, out end_pos)) throw new Exception("json解析错误");
                    flag = false;
                }
                else if (json[end_pos] == 'n' || json[end_pos] == 'N')
                {
                    if (!flag) throw new Exception("json解析错误");
                    string value = "";
                    if (!_DecodeGetNull(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
                    ret[key] = value;
                    if (!_DecodeCheckNextCommo(ref json, '}', end_pos + 1, out end_pos)) throw new Exception("json解析错误");
                    flag = false;
                } 
                else if (json[end_pos] == '}')
                {
                    if (flag) throw new Exception("json解析错误");
                    return ret;
                }
                else if (json[end_pos] == ' ')
                {

                }
                else
                    throw new Exception("json解析错误");
                end_pos++;
            }
            throw new Exception("json解析错误");
        }
        private static Json _DecodeArray(ref string json, int start_pos, out int end_pos,int depth)
        {
            if (depth > max_depth)
                throw new Exception("递归层次超过阈值");
            end_pos = start_pos + 1;
            Json ret = new Json();
            int cnt = 0;
            while (end_pos < json.Length)
            {
                if (json[end_pos] == '"')
                {
                    string str = "";
                    if (!_DecodeGetString(ref json, end_pos, out end_pos, out str)) throw new Exception("json解析错误");
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos+1, out end_pos)) throw new Exception("json解析错误");

                    ret[cnt++] = str;
                }
                else if (json[end_pos] == '{')
                {
                    ret[cnt++] = _DecodeDictionary(ref json, end_pos, out end_pos, depth + 1);
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                }
                else if (json[end_pos] == '[')
                {
                    ret[cnt++] = _DecodeArray(ref json, end_pos, out end_pos, depth + 1);
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                }
                else if (json[end_pos] >= '0' && json[end_pos] <= '9')
                {
                    string value = "";
                    if (!_DecodeGetNum(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
                    ret[cnt++] = value;
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                }
                else if (json[end_pos] == 't' || json[end_pos] == 'T' || json[end_pos] == 'f' || json[end_pos] == 'F')
                {
                    bool value = false;
                    if (!_DecodeGetBool(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
                    ret[cnt++] = value;
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");
                }
                else if (json[end_pos] == 'n' || json[end_pos] == 'N')
                {
                    string value = "";
                    if (!_DecodeGetNull(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
                    ret[cnt++] = value;
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");
                }
                else if (json[end_pos] == ']')
                {
                    return ret;
                }
                else if (json[end_pos] == ' ')
                {

                }
                else
                    throw new Exception("json解析错误");
                end_pos++;
            }
            throw new Exception("json解析错误");
        }

        //遍历到字符串结束标志"
        private static bool _DecodeGetString(ref string json,int start_pos,out int end_pos,out string str)
        {
            str = "";
            end_pos = start_pos + 1;
            while(end_pos<json.Length)
            {
                if (json[end_pos] == '\\')
                {
                    if (end_pos < json.Length - 1)
                    {
                        end_pos++;
                        switch(json[end_pos])
                        {
                            case 't':
                                str += '\t';
                                end_pos++;
                                continue;
                            case 'n':
                                str += '\n';
                                end_pos++;
                                continue;
                            case 'u':
                                if (json.Length - end_pos >= 4)
                                {
                                    string hex = json.Substring(end_pos + 1, 4);
                                    bool isSuccess = true;
                                    try
                                    {
                                        char tmp = (char)int.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                                        str += tmp;
                                    }
                                    catch
                                    {
                                        isSuccess = false;
                                    }
                                    if (!isSuccess) return false;

                                    end_pos += 5;
                                    continue;

                                }
                                else
                                    return false;
                        }
                    }
                }
                else if (json[end_pos] == '\"')
                    break;
                str += json[end_pos];
                end_pos++;
            }
            if (end_pos >= json.Length)
                return false;
            return true;

        }
        //遍历到最后一个数字位置
        private static bool _DecodeGetNum(ref string json, int start_pos, out int end_pos, out string str)
        {
            str = "";
            end_pos = start_pos;
            int pot_cnt = 0;
            while (end_pos < json.Length)
            {
                if ((json[end_pos] >= '0' && json[end_pos] <= '9'))
                    str += json[end_pos];
                else if (json[end_pos] == '.')
                {
                    if (pot_cnt == 0)
                    {
                        str += json[end_pos];
                        pot_cnt++;
                    }
                    else
                    {
                        return false;
                    }

                }
                else if (json[end_pos] == ' ' || json[end_pos] == '}' || json[end_pos] == ']' || json[end_pos] == ',')
                {
                    if (str.Length == 0)
                        return false;
                    else
                    {
                        end_pos--;
                        return true;
                    }
                    
                }
                else
                    return false;

                end_pos++;
            }
            return true;
        }
        private static bool _DecodeGetBool(ref string json, int start_pos, out int end_pos, out bool boolean)
        {
            end_pos = start_pos;
            boolean = false;
            if (json.Length - end_pos < 5)//一定至少5个
                return false;
            if(json.Substring(end_pos,4).ToLower()=="true")
            {
                boolean = true;
                end_pos += 3;
                return true;
            }
            else
            if(json.Substring(end_pos, 5).ToLower() == "false")
            {
                boolean = false;
                end_pos += 4;
                return true;
            }
            return false;
        }
        private static bool _DecodeGetNull(ref string json, int start_pos, out int end_pos, out string str)
        {
            end_pos = start_pos;
            str = "";
            if (json.Length - end_pos < 5)//一定至少5个
                return false;
            if (json.Substring(end_pos, 4).ToLower() == "null")
            {
                str = "null";
                end_pos += 3;
                return true;
            }
            return false;
        }

        private static bool _DecodeCheckNextCommo(ref string json,char valid_end, int start_pos, out int end_pos)
        {
            end_pos = start_pos;
            while (end_pos<json.Length)
            {
                if (json[end_pos] == ',' || json[end_pos] == valid_end)
                {
                    if (json[end_pos] == valid_end)
                        end_pos--;
                    return true;
                }
                else if (json[end_pos] != ' ')
                    return false;
                end_pos++;
            }
            return false;
        }
        #endregion
        public bool IsReadAble()
        {
            switch(childrenType)
            {
                case JSONChildrenType.NULL:
                    return false;
                case JSONChildrenType.ARRAY:
                    return false;
                case JSONChildrenType.DICTIONARY:
                    return false;
                default:
                    return true;
            }

        }
        public bool IsNull()
        {
            return childrenType == JSONChildrenType.NULL;
        }
        public static bool TryParse(string a,out Json ret)
        {
            ret = Json.Decode(a)??new Json();
            return true;
        }
        #region Implement for IDictionary
        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, Json>)childrens).ContainsKey(key);
        }

        public void Add(string key, Json value)
        {
            ((IDictionary<string, Json>)childrens).Add(key, value);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, Json>)childrens).Remove(key);
        }

        public bool TryGetValue(string key, out Json value)
        {
            return ((IDictionary<string, Json>)childrens).TryGetValue(key, out value);
        }

        public void Add(KeyValuePair<string, Json> item)
        {
            ((IDictionary<string, Json>)childrens).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<string, Json>)childrens).Clear();
        }

        public bool Contains(KeyValuePair<string, Json> item)
        {
            return ((IDictionary<string, Json>)childrens).Contains(item);
        }

        public void CopyTo(KeyValuePair<string, Json>[] array, int arrayIndex)
        {
            ((IDictionary<string, Json>)childrens).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<string, Json> item)
        {
            return ((IDictionary<string, Json>)childrens).Remove(item);
        }

        public IEnumerator<KeyValuePair<string, Json>> GetEnumerator()
        {
            return ((IDictionary<string, Json>)childrens).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, Json>)childrens).GetEnumerator();
        }
        private static string EncodeNonAsciiCharacters(string value)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in value)
            {
                if (c > 127)
                {
                    // This character is too big for ASCII
                    string encodedValue = "\\u" + ((int)c).ToString("x4");
                    sb.Append(encodedValue);
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }
    }
#endregion

}
