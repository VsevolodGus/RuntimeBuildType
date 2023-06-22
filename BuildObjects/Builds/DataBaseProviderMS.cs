using Microsoft.Data.SqlClient;

namespace BuildObjects.Builds;

public class DataBaseProviderMS
{
    private readonly SqlConnection _connection = new SqlConnection("Data Source=localhost\\SQLEXPRESS;Initial Catalog=Store;Integrated Security=True;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
    public SqlDataReader GetSqlReader(string query)
    {
        _connection.Open();
        
        var com = new SqlCommand(query, _connection);
        return com.ExecuteReader();
    }

    public void CloseConnection()
    {
        _connection.Close();
    }


}
