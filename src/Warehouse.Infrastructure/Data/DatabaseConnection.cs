namespace Warehouse.Infrastructure.Data;

public class DatabaseConnection
{
    public string Provider { get; set; } = "PostgreSQL";
    public required string Server { get; set; }
    public required string User { get; set; }
    public required string Password { get; set; }
    public required string Database { get; set; }
}