using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Microsoft.Extensions.Configuration;


namespace HelloWorld.Data
{
  public class DataContextDapper
  {
    private IConfiguration _config;
    public DataContextDapper(IConfiguration config)
    {
      _config = config;
    }

    public IEnumerable<T> LoadData<T>(string sql) // .Query
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")); // establish connection
      return dbConnection.Query<T>(sql);
    }

    public T LoadDataSingle<T>(string sql) // .QuerySingle
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")); // establish connection
      return dbConnection.QuerySingle<T>(sql);
    }

    public bool Execute(string sql) //,QuerySingle
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")); // establish connection
      return dbConnection.Execute(sql) > 0;
    }

    public int ExecuteWithRowCount(string sql) //,QuerySingle
    {
      IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString("DefaultConnection")); // establish connection
      return dbConnection.Execute(sql);
    }
  }
}