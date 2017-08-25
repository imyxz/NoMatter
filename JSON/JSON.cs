using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON
{
    public class Json
    {
        private Dictionary<string, Json> childrens = new Dictionary<string, Json>();
        private string single_value;
        private JSONChildrenType childrenType = new JSONChildrenType();
        public Json()
        {
            childrenType = JSONChildrenType.ARRAY;
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

        public T ConvertTo<T>() where T : IJsonAble,new()
        {
            T ret = new T();
            ret.fromJson(this);
            return ret;
        }
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
        public int ToInt()
        {
            int ret = 0;
            int.TryParse(single_value, out ret);
            return ret;
        }
        public double ToDouble()
        {
            double ret = 0;
            double.TryParse(single_value, out ret);
            return ret;
        }
        public bool ToBool()
        {
            bool ret = false;
            bool.TryParse(single_value, out ret);
            return ret;
        }
        public static Json ConvertFrom<T>(T obj) where T: IJsonAble
        {
            return obj.toJson();
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
        public static implicit operator int(Json a)
        {
            return a.ToInt();
        }
        public static implicit operator double(Json a)
        {
            return a.ToDouble();
        }
        public static implicit operator string(Json a)
        {
            return a.single_value;
        }
        public static implicit operator bool(Json a)
        {
            return a.ToBool();
        }
        public string Encode()
        {
            if (childrenType == JSONChildrenType.STRING)
                return "{" + Json.Filter(single_value,childrenType) + "}";
            else
                return _Encode();
        }
        private string _Encode()
        {
            if (childrenType == JSONChildrenType.ARRAY)
            {
                string ret = "[";
                int cnt = 0;
                foreach (KeyValuePair<string, Json> pair in childrens)
                {
                    ret += pair.Value._Encode() + ",";
                    cnt++;
                }
                if (cnt > 0)
                {
                    ret = ret.Substring(0, ret.Length - 1);
                }
                return ret + "]";

            }
            else if (childrenType == JSONChildrenType.DICTIONARY)
            {
                string ret = "{";
                int cnt = 0;
                foreach (KeyValuePair<string, Json> pair in childrens)
                {
                    ret += "" + Json.Filter(pair.Key,JSONChildrenType.STRING) + ": " + pair.Value._Encode() +",";
                    cnt++;
                }
                if(cnt>0)
                {
                    ret = ret.Substring(0, ret.Length - 1);
                }
                return ret + "}";

            }
            else
                return "" + Json.Filter(single_value, childrenType) + "";

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
        public static Json Decode(string json)
        {
            return _Decode(ref json);

        }
        private static Json _Decode(ref string json)
        {
            int tmp = 0;
            if (tmp<json.Length)
            {
                switch(json[tmp])
                {
                    case '{':
                        return _DecodeDictionary(ref json, 0, out tmp);
                    case '[':
                        return _DecodeArray(ref json, 0, out tmp);
                    case ' ':
                        break;
                    default:
                        throw new Exception("json解析错误");
                }
                tmp++;
            }
            throw new Exception("json解析错误");
        }
        private static Json _DecodeDictionary(ref string json, int start_pos, out int end_pos)
        {
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
                    ret[key] = _DecodeDictionary(ref json, end_pos, out end_pos);
                    if (!_DecodeCheckNextCommo(ref json, '}', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                    flag = false;
                }
                else if (json[end_pos] == '[')
                {
                    if (!flag) throw new Exception("json解析错误");
                    ret[key] = _DecodeArray(ref json, end_pos, out end_pos);
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
        private static Json _DecodeArray(ref string json, int start_pos, out int end_pos)
        {
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
                    ret[cnt++] = _DecodeDictionary(ref json, end_pos, out end_pos);
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                }
                else if (json[end_pos] == '[')
                {
                    ret[cnt++] = _DecodeArray(ref json, end_pos, out end_pos);
                    if (!_DecodeCheckNextCommo(ref json, ']', end_pos + 1, out end_pos)) throw new Exception("json解析错误");

                }
                else if (json[end_pos] >= '0' && json[end_pos] <= '9')
                {
                    string value = "";
                    if (!_DecodeGetNum(ref json, end_pos, out end_pos, out value)) throw new Exception("json解析错误");
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

    }
}
