using Microsoft.Data.SqlClient;
using System.Data;

namespace Sat242516008.Loggers;

public class DbLogger
{
    private readonly string _connectionString;

    public DbLogger(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public void Log(string tableName, string operation, string recordId, string description, string userId = null)
    {
        try
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                var query = @"INSERT INTO Logs_Table (TableName, Operation, RecordID, LogDate, Description, UserID) 
                              VALUES (@TableName, @Operation, @RecordID, @LogDate, @Description, @UserID)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TableName", tableName);
                    cmd.Parameters.AddWithValue("@Operation", operation);
                    cmd.Parameters.AddWithValue("@RecordID", recordId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LogDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Description", description ?? "");
                    cmd.Parameters.AddWithValue("@UserID", userId ?? (object)DBNull.Value);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
        catch (Exception)
        {
            // Loglama hatası akışı bozmasın
        }
    }
}