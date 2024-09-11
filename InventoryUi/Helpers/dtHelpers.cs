using System.Data;
using System.Reflection;

namespace InventoryUi.Helpers
{
    public static class dtHelpers
    {
        public static DataTable ListToDt<T>(List<T> items)
        {
            DataTable dt = new DataTable(typeof(T).Name);

            // Get all the properties of the type T
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add a column to the DataTable for each property of T
            foreach (var prop in properties)
            {
                dt.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Populate the DataTable rows with the values from the list
            foreach (var item in items)
            {
                var values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(item, null);
                }
                dt.Rows.Add(values);
            }

            return dt;
        }
        public static DataTable ObjectToDataTable<T>(T obj)
        {
            DataTable table = new DataTable();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                // Add columns for each property in the object
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            // Create a new row and add the property values
            DataRow row = table.NewRow();
            foreach (var prop in properties)
            {
                row[prop.Name] = prop.GetValue(obj) ?? DBNull.Value;
            }
            table.Rows.Add(row);

            return table;
        }


    }
}
