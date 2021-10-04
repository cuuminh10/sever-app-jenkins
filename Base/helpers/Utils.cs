using gmc_api.DTO.PP;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static gmc_api.Base.Helpers.Constants;

namespace gmc_api.Base.Helpers
{
    public class Utils
    {
        public static SortedDictionary<string, object> getPropertiesForUpdate(object obj)
        {
            var ListProp = new SortedDictionary<string, object>();
            //double - decimal - DateTime
            foreach (var prop in obj.GetType().GetProperties())
            {
                var typeProp = prop.PropertyType;
                Console.WriteLine(typeProp);
                Console.WriteLine(typeof(DateTime?));
                if (typeProp == typeof(string))
                {
                    if (prop.GetValue(obj) is not null && !prop.GetValue(obj).Equals(DEFAULT_VALUE_STRING))
                    {
                        ListProp.Add(prop.Name, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(int))
                {
                    if (prop.GetValue(obj) is not null && !((int)prop.GetValue(obj) == DEFAULT_VALUE_INT))
                    {
                        ListProp.Add(prop.Name, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(bool?))
                {
                    bool? x = prop.GetValue(obj) as bool?;
                    if (x != null)
                    {
                        ListProp.Add(prop.Name, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(double))
                {
                    if (prop.GetValue(obj) is not null && !((double)prop.GetValue(obj) == DEFAULT_VALUE_DOUBLE))
                    {
                        ListProp.Add(prop.Name, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(decimal))
                {
                    if (prop.GetValue(obj) is not null && !((decimal)prop.GetValue(obj) == DEFAULT_VALUE_DECIMAL))
                    {
                        ListProp.Add(prop.Name, prop.GetValue(obj));
                    }
                }
                else if (typeProp.Equals(typeof(DateTime?))
                    || typeProp.Equals(typeof(DateTime)))
                {
                    if (prop.GetValue(obj) is not null && !((DateTime)prop.GetValue(obj) == DEFAULT_VALUE_DATETIME))
                    {
                        ListProp.Add(prop.Name, prop.GetValue(obj));
                    }
                }
            }
            return ListProp;
        }

        public static object makeDedaultOrEmptyToNull(object obj, string idName = "")
        {
            var objectNew = new ExpandoObject() as IDictionary<string, object>; ;
            foreach (var prop in obj.GetType().GetProperties())
            {
                var typeProp = prop.PropertyType;
                //  var proName = prop.Name;
                var propNameForFE = (JsonPropertyNameAttribute)prop.GetCustomAttributes(typeof(JsonPropertyNameAttribute), false).FirstOrDefault();
                var proName = prop.Name;
                if (proName == idName)
                {
                    proName = "id";
                }
                else if (propNameForFE != null && !string.IsNullOrEmpty(propNameForFE.Name))
                {
                    proName = propNameForFE.Name;
                }

                if (typeProp == typeof(string))
                {
                    if (prop.GetValue(obj) is not null && !prop.GetValue(obj).Equals(DEFAULT_VALUE_STRING))
                    {
                        objectNew.Add(proName, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(int))
                {
                    if (prop.GetValue(obj) is not null && !((int)prop.GetValue(obj) == DEFAULT_VALUE_INT))
                    {
                        objectNew.Add(proName, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(bool?))
                {
                    bool? x = prop.GetValue(obj) as bool?;
                    if (x != null)
                    {
                        objectNew.Add(proName, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(double))
                {
                    if (prop.GetValue(obj) is not null && !((double)prop.GetValue(obj) == DEFAULT_VALUE_DOUBLE))
                    {
                        objectNew.Add(proName, prop.GetValue(obj));
                    }
                }
                else if (typeProp == typeof(decimal))
                {
                    if (prop.GetValue(obj) is not null && decimal.Compare((decimal)prop.GetValue(obj), DEFAULT_VALUE_DECIMAL) != 0)
                    {
                        objectNew.Add(proName, prop.GetValue(obj));
                    }
                }
                else if (typeProp.Equals(typeof(DateTime?))
                    || typeProp.Equals(typeof(DateTime)))
                {
                    if (prop.GetValue(obj) is not null && !((DateTime)prop.GetValue(obj) == DEFAULT_VALUE_DATETIME))
                    {
                        objectNew.Add(proName, prop.GetValue(obj));
                    }
                }
            }

            return objectNew;
        }

        public static string BuildingSelectSQL(SortedDictionary<string, object> mapProp, string tableName)
        {
            string appendSql = "SELECT * FROM " + tableName + " WHERE AAStatus = 'Alive' ";
            foreach (var prop in mapProp)
            {
                string key = prop.Key;
                var value = prop.Value;
                if (value.GetType() == typeof(string))
                {
                    appendSql += " AND " + key + " = '" + value.ToString() + "'";
                }
                else if (value.GetType() == typeof(int))
                {
                    appendSql += " AND " + key + " = " + (int)value;
                }
                else if (value.GetType() == typeof(bool))
                {
                    appendSql += " AND " + key + " = CAST(\'" + (bool)value + "\' AS BIT)";
                }
                else if (value.GetType() == typeof(double))
                {
                    appendSql += " AND " + key + " = CAST(\'" + (double)value + "\' AS FLOAT)";
                }
                else if (value.GetType() == typeof(decimal))
                {
                    appendSql += " AND " + key + " = CAST(\'" + (decimal)value + "\' AS DECIMAL)";
                }
                else if (value.GetType() == typeof(DateTime))
                {
                    appendSql += " AND " + key + " = CAST(\'" + value + "\' AS DATETIME)";
                }
            }
            return appendSql;
        }

        public static string BuildingUpdateSQL(SortedDictionary<string, object> mapProp, string tableName, string idColumns, bool isDeleted = false,
            bool updateDefaultColumn = false, bool whereDefaultColumn = true)
        {
            string appendSql = "UPDATE " + tableName + " SET ";
            bool first = true;
            if (isDeleted)
            {
                appendSql += " AAStatus = 'Delete'";
                first = false;
            }
            foreach (var prop in mapProp)
            {
                string key = prop.Key;
                var value = prop.Value;
                //Console.WriteLine(value.GetType());
                if (key.Equals(idColumns))
                    continue;
                if (value.GetType() == typeof(string))
                {
                    appendSql += first ? "" : ",";
                    first = false;
                    appendSql += key + " = N'" + value.ToString() + "'";
                }
                else if (value.GetType() == typeof(int))
                {
                    appendSql += first ? "" : ",";
                    first = false;
                    appendSql += key + " = " + (int)value;
                }
                else if (value.GetType() == typeof(bool))
                {
                    appendSql += first ? "" : ",";
                    first = false;
                    appendSql += key + " = CAST(\'" + (bool)value + "\' AS BIT)";
                }
                else if (value.GetType() == typeof(double))
                {
                    appendSql += first ? "" : ",";
                    first = false;
                    appendSql += key + " = CAST(\'" + (double)value + "\' AS FLOAT)";
                }
                else if (value.GetType() == typeof(decimal))
                {
                    appendSql += first ? "" : ",";
                    first = false;
                    appendSql += key + " = CAST(\'" + (decimal)value + "\' AS DECIMAL)";
                }
                else if (value.GetType().Equals(typeof(DateTime?))
                    || value.GetType().Equals(typeof(DateTime)))
                {
                    appendSql += first ? "" : ",";
                    first = false;
                    appendSql += key + " = CAST(\'" + value + "\' AS DATETIME)";
                }
            }
            appendSql += " OUTPUT inserted.* WHERE " + idColumns + " = " + mapProp[idColumns];
            appendSql += whereDefaultColumn ? " and AAStatus = 'Alive'" : "";
            return appendSql;
        }

        public static string generateJwtToken(User user, string secretKey)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("userId", user.ADUserID.ToString()), new Claim("userName", user.ADUserName.ToString())
                , new Claim("groupId", user.ADUserGroupID.ToString())}),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string buildConditionFromDateToDate(string columns, DateTime? fromDate, DateTime? toDate)
        {
            var searchCondition = "";
            if (fromDate != null)
            {
                if (toDate != null)
                {
                    searchCondition = string.Format(@" {0} >= '{1}' and {2} < '{3}' ", columns, fromDate, columns, toDate);
                }
                else
                {
                    searchCondition = string.Format(@" {0} >= '{1}' ", columns, fromDate);
                }
            }
            else
            {
                if (toDate != null)
                {
                    searchCondition = string.Format(@" {0} < '{1}' ", columns, toDate);
                }
            }
            return searchCondition;
        }

        public static string SelectStatusApprove(string searchStatus, string statusApprove)
        {
            if (searchStatus == ApproveStatus.INPROCESS)
            {
                statusApprove = "'', 'Create'";
            }
            else if (searchStatus == ApproveStatus.APROVED)
            {
                statusApprove = "'Approved', 'InProgress', 'Approving'";
            }
            else if (searchStatus == ApproveStatus.REJECT)
            {
                statusApprove = "'Rejected'";
            }
            return statusApprove;
        }

        public static string SelectStatusApproveItem(string searchStatus, string statusApprove)
        {
            // Khác
            if (searchStatus == ApproveStatus.INPROCESS)
            {
                statusApprove = "'InProgress', 'Approving'";
            }
            else if (searchStatus == ApproveStatus.APROVED)
            {
                statusApprove = "'Approved'";
            }
            else if (searchStatus == ApproveStatus.REJECT)
            {
                statusApprove = "'Rejected'";
            }
            return statusApprove;
        }

        public static object OveriteProperties(object objFromObject, SortedDictionary<string, object> propDic)
        {
            PropertyInfo[] properties = objFromObject.GetType().GetProperties();
            Type typeFrom = objFromObject.GetType();
            //foreach (PropertyInfo propFrom in properties)
            foreach (var prop in propDic)
            {
                PropertyInfo propFrom = typeFrom.GetProperty(prop.Key);
                if (propFrom != null)
                {
                    propFrom.SetValue(objFromObject, prop.Value, null);
                }
            }
            return objFromObject;
        }

        public static object CopyObject(object objFromObject, object objToObject)
        {
            PropertyInfo[] properties = objToObject.GetType().GetProperties();
            Type typeTo = objToObject.GetType();
            //foreach (PropertyInfo propFrom in properties)
            Parallel.ForEach(properties, propFrom =>
            {
                PropertyInfo propTo = typeTo.GetProperty(propFrom.Name);
                if (propTo != null)
                {
                    object objValue = GetPropertyValue(objFromObject, propFrom.Name);//ExpertLib.DynamicInvoker.DynamicGetValue(objFromObject, propFrom);
                    propTo.SetValue(objToObject, objValue, null);
                }
            }
            );
            return objToObject;
        }

        public static object GetPropertyValue(object obj, string strPropertyName)
        {
            Type objType = obj.GetType();
            PropertyInfo property = objType.GetProperty(strPropertyName);

            if (property != null)
                return property.GetValue(obj, null);

            return null;
        }
        public static bool SetPropertyValue(object obj, string strPropertyName, object value)
        {
            PropertyInfo property = obj.GetType().GetProperty(strPropertyName);
            if (property != null)
            {
                property.SetValue(obj, value, null);
                return true;
            }
            return false;
        }
    }
}
