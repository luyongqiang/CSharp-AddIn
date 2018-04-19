using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace   Syncfusion.Core.Extensions
{
    /// <summary>
    /// Object扩展
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// 克隆对像
        /// </summary>
        /// <param name="obj">当前对像</param>
        /// <returns>object</returns>
        public static object CloneObject(this object obj)
        {
            using (MemoryStream memStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, obj);
                memStream.Position = 0;
                return binaryFormatter.Deserialize(memStream);
            }
        }

        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj">参数1</param>
        /// <param name="obj1">参数2</param>
        /// <returns></returns>
        public static bool IsBinaryEqualTo(this object obj, object obj1)
        {
            if (obj == null || obj1 == null)
            {
                if (obj == null && obj1 == null)
                    return true;
                else
                    return false;
            }

            using (MemoryStream memStream = new MemoryStream())
            {                
                BinaryFormatter binaryFormatter = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                binaryFormatter.Serialize(memStream, obj);
                byte[] b1 = memStream.ToArray();
                memStream.SetLength(0);

                binaryFormatter.Serialize(memStream, obj1);
                byte[] b2 = memStream.ToArray();

                if (b1.Length != b2.Length)
                    return false;                

                for (int i = 0; i < b1.Length; i++)
                {
                    if (b1[i] != b2[i])
                        return false;
                }

                return true;
            }
        }

        /// <summary>
        /// 是否为空
        /// </summary>
        /// <returns></returns>
        public static bool IsNull(this object obj)
        {
            return obj == null || obj == DBNull.Value;
        }

        /// <summary>
        /// 转换为String
        /// </summary>
        /// <returns></returns>
        public static string SafeString(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return "";
            else            
                return System.Convert.ToString(obj);
        }

        /// <summary>
        /// 转换为Int
        /// </summary>
        /// <returns></returns>
        public static int SafeInt(this object obj)
        {
            return SafeInt(obj, 0);
        }

       /// <summary>
       /// 将参数转换为Int类型
       /// </summary>
       /// <param name="DefaultValue">参数值</param>
       /// <returns></returns>
        public static int SafeInt(this object obj, int DefaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return DefaultValue;
            else if (obj is int)
                return (int)obj;
            else if (obj is byte)
                return (int)(byte)obj;
            else if (obj is sbyte)
                return (int)(sbyte)obj;
            else if (obj is Int16)
                return (int)(Int16)obj;
            else if (obj is UInt16)
                return (int)(UInt16)obj;

            else
            {
                try
                {
                    if (obj is double)
                        return (int)(double)obj;
                    else if (obj is float)
                        return (int)(float)obj;
                    else if (obj is decimal)
                        return (int)(decimal)obj;

                    else if (obj is Int64)
                        return (int)(Int64)obj;
                    
                    else if (obj is UInt32)
                        return (int)(UInt32)obj;
                    else if (obj is UInt64)
                        return (int)(UInt64)obj;

                    else
                    {
                        int retValue;
                        if (int.TryParse(obj.ToString(), out retValue))
                            return retValue;
                        else
                            return DefaultValue;
                    }
                }
                catch (Exception)
                {
                    return DefaultValue;
                }            
            }
        }

        public static long SafeLong(this object obj)
        {
            return SafeLong(obj, 0);
        }

        public static long SafeLong(this object obj, long DefaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return DefaultValue;
            else if (obj is int)
                return (long)(int)obj;
            else if (obj is byte)
                return (long)(byte)obj;
            else if (obj is sbyte)
                return (long)(sbyte)obj;
            else if (obj is Int16)
                return (long)(Int16)obj;
            else if (obj is UInt16)
                return (long)(UInt16)obj;

            else
            {
                try
                {
                    if (obj is double)
                        return (long)(double)obj;
                    else if (obj is float)
                        return (long)(float)obj;
                    else if (obj is decimal)
                        return (long)(decimal)obj;

                    else if (obj is Int64)
                        return (long)(Int64)obj;

                    else if (obj is UInt32)
                        return (long)(UInt32)obj;
                    else if (obj is UInt64)
                        return (long)(UInt64)obj;

                    else
                    {
                        long retValue;
                        if (long.TryParse(obj.ToString(), out retValue))
                            return retValue;
                        else
                            return DefaultValue;
                    }
                }
                catch (Exception)
                {
                    return DefaultValue;
                }
            }
        }

        public static decimal SafeDecimal(this object obj)
        {
            return SafeDecimal(obj, Decimal.Zero);
        }

        public static decimal SafeDecimal(this object obj, decimal DefaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return DefaultValue;
            else if (obj is decimal)
                return (decimal)obj;            
            else if (obj is double)
                return (decimal)(double)obj;            
            else if (obj is float)
                return (decimal)(float)obj;

            else if (obj is byte)
                return (decimal)(byte)obj;
            else if (obj is sbyte)
                return (int)(sbyte)obj;

            else if (obj is Int16)
                return (decimal)(Int16)obj;
            else if (obj is Int32)
                return (decimal)(Int32)obj;
            else if (obj is Int64)
                return (decimal)(Int64)obj;            

            else if (obj is UInt16)
                return (decimal)(UInt16)obj;
            else if (obj is UInt32)
                return (decimal)(UInt32)obj;
            else if (obj is UInt64)
                return (decimal)(UInt64)obj;            
            else
            {
                decimal retValue;
                if (decimal.TryParse(obj.ToString(), out retValue))
                    return retValue;
                else
                    return DefaultValue;
            }
        }

        public static double SafeDouble(this object obj)
        {
            return SafeDouble(obj, 0d);
        }

        public static double SafeDouble(this object obj, double DefaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return DefaultValue;
            else if (obj is double)
                return (double)obj;            
            else if (obj is float)            
                return (double)(float)obj;
            else if (obj is decimal)
                return (double)(decimal)obj;
                        
            else if (obj is byte)
                return (double)(byte)obj;
            else if (obj is sbyte)
                return (double)(sbyte)obj;

            else if (obj is Int16)
                return (double)(Int16)obj;
            else if (obj is Int32)
                return (double)(Int32)obj;
            else if (obj is Int64)
                return (double)(Int64)obj;
            else if (obj is UInt16)
                return (double)(UInt16)obj;
            else if (obj is UInt32)
                return (double)(UInt32)obj;
            else if (obj is UInt64)
                return (double)(UInt64)obj;
            else
            {
                double retValue;
                if (double.TryParse(obj.ToString(), out retValue))
                    return retValue;
                else
                    return DefaultValue;
            }
        }

        public static float SafeFloat(this object obj)
        {
            return SafeFloat(obj, 0f);
        }

        public static float SafeFloat(this object obj, float defaultValue)
        {
            if (obj == null || obj == DBNull.Value)
                return defaultValue;
            else if (obj is float)
                return (float)obj;
            else
            {
                try
                {
                    if (obj is double)
                        return (float)(double)obj;
                    else if (obj is decimal)
                        return (float)(decimal)obj;
                    else if (obj is Int16)
                        return (float)(Int16)obj;
                    else if (obj is Int32)
                        return (float)(Int32)obj;
                    else if (obj is Int64)
                        return (float)(Int64)obj;
                    else if (obj is UInt16)
                        return (float)(UInt16)obj;
                    else if (obj is UInt32)
                        return (float)(UInt32)obj;
                    else if (obj is UInt64)
                        return (float)(UInt64)obj;                    
                    else if (obj is byte)
                        return (float)(byte)obj;
                    else if (obj is sbyte)
                        return (float)(sbyte)obj;
                    else
                    {
                        float retValue;
                        if (float.TryParse(obj.ToString(), out retValue))
                            return retValue;
                        else
                            return defaultValue;
                    }
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }            
        }

        public static DateTime SafeDateTime(this object obj)
        {
            return SafeDateTime(obj, DateTime.MinValue);
        }

        public static DateTime SafeDateTime(this object obj, DateTime DefaultValue)
        {
            if (obj is DateTime)
                return (DateTime)obj;
            else if (obj == null || obj == DBNull.Value)
                return DefaultValue;            
            else
            {
                DateTime retValue;
                if (DateTime.TryParse(obj.ToString(), out retValue))
                    return retValue;
                else
                    return DefaultValue;
            }
        }

        public static bool SafeBool(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
                return false;
            
            if (obj is bool)
                return (bool)obj;

            if (IsNumeric(obj))
                return obj.SafeInt(0) != 0;            

            switch (obj.ToString().Trim().ToUpperInvariant())
            {
                case "0":
                case "F":
                case "FALSE":
                case "FALSCH":
                case "OFF":
                    return false;

                default:
                    return true;
            }
        }

        /// <summary>
        /// 是否日期类型
        /// </summary>
        /// <returns></returns>
        public static bool IsDate(this object value)
        {
            bool returnValue;

            if (value == null || value == DBNull.Value) return false;

            if (value is DateTime)
                return true;

            try
            {
                DateTime d = System.Convert.ToDateTime(value);            
                returnValue = true;
            }
            catch (Exception)
            {
                returnValue = false;
            }
            return returnValue;
        }

        /// <summary>
        /// 是否整数、浮点类型
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNumeric(this object value)
        {
            if (value == null || value == DBNull.Value)
                return false;

            if (value is int || value is double || value is float || value is decimal || value is byte || value is Int16 || value is Int64 || value is UInt16 || value is UInt32 || value is UInt64 || value is sbyte)
                return true;

            double tmp;            
            return Double.TryParse(value.ToString(), out tmp);
        }


    }
}
