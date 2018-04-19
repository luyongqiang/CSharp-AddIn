using System.Data;
using System.Linq;
    using System;
namespace     Syncfusion.Core.Extensions
{


    public static class DataSetExtensions
    {
        public static DataRow LastRow(this DataRowCollection rows)
        {
            if (rows == null || rows.Count == 0)
                return null;
            else
                return rows[rows.Count - 1];
        }

        public static int LastRowID(this DataRowCollection rows)
        {
            DataRow row = rows.LastRow();
            if (row == null)
                return 0;
            else
            {
                if (row.Table.PrimaryKey != null && row.Table.PrimaryKey.Length == 1)
                    return row[row.Table.PrimaryKey.First()].SafeInt();

                return 0;
            }
        }

        public static T Field<T>(this DataRow row, string columnName, T defaultValue)
        {
            try
            {
                return row.Field<T>(columnName);
            }
            catch
            {
                return defaultValue;
            }
        }

        public static T Field<T>(this DataRow row, string columnName)
        {
            if (row[columnName] == null)
                throw new NullReferenceException(columnName + " does not exist in DataRow");

            string value = row[columnName].ToString();

            if (typeof(T) == "".GetType())
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            else if (typeof(T) == 0.GetType())
            {
                return (T)Convert.ChangeType(int.Parse(value), typeof(T));
            }
            else if (typeof(T) == false.GetType())
            {
                return (T)Convert.ChangeType(bool.Parse(value), typeof(T));
            }
            else if (typeof(T) == DateTime.Now.GetType())
            {
                return (T)Convert.ChangeType(DateTime.Parse(value), typeof(T));
            }
            else if (typeof(T) == new byte().GetType())
            {
                return (T)Convert.ChangeType(byte.Parse(value), typeof(T));
            }
            else if (typeof(T) == new float().GetType())
            {
                return (T)Convert.ChangeType(float.Parse(value), typeof(T));
            }
            else
            {
                throw new ArgumentException(string.Format("Cannot cast '{0}' to '{1}'.", value, typeof(T).ToString()));
            }
        }

        public static bool HasRows(this DataSet dataSet)
        {
            return (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0);
        }
    }
}
