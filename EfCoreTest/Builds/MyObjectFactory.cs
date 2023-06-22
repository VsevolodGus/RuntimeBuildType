using Microsoft.Data.SqlClient;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BuildObjects.Builds;

public class MyObjectFactory
{
    public object BuildObject(Type type, SqlDataReader reader)
    {
        var columns = reader.GetColumnSchema();
        var obj = Activator.CreateInstance(type);

        int indexColumn = 0;
        while (indexColumn < columns.Count)
        {
            var field = type.GetField(columns[indexColumn].ColumnName);

            var value = reader.GetValue(indexColumn);
            value = value == DBNull.Value
                    ? type.GetDefaultValue()
                    : value;

            field.SetValue(obj, value);

            indexColumn++;
        }

        return obj;
    }
}
