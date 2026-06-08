using NpgsqlTypes;

namespace Warehouse.Infrastructure.Enum;

public enum OrderStatus
{
    [PgName("created")]
    Created,
    
    [PgName("reserved")]
    Reserved,
    
    [PgName("picking")]
    Picking,
    
    [PgName("picked")]
    Picked,
    
    [PgName("packed")]
    Packed,
    
    [PgName("shipped")]
    Shipped,
    
    [PgName("cancelled")]
    Cancelled
}