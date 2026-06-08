using Microsoft.EntityFrameworkCore;
using Warehouse.Infrastructure.Enum;

namespace Warehouse.Infrastructure.Data;

public partial class WmsContext : DbContext
{
    public WmsContext()
    {
    }

    public WmsContext(DbContextOptions<WmsContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<StockBalance> StockBalances { get; set; }

    public virtual DbSet<StockMovement> StockMovements { get; set; }

    public virtual DbSet<StockReservation> StockReservations { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseLocation> WarehouseLocations { get; set; }
    public virtual DbSet<OutboxMessage> OutboxMessages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("movement_type", new[] { "receive", "transfer", "ship", "write_off", "positive_adjustment", "negative_adjustment", "return" })
            .HasPostgresEnum("order_status", new[] { "created", "reserved", "picking", "picked", "packed", "shipped", "cancelled" })
            .HasPostgresEnum("reservation_status", new[] { "active", "cancelled", "consumed", "expired" })
            .HasPostgresEnum("warehouse_locations_status", new[] { "active", "inactive", "blocked" })
            .HasPostgresEnum("warehouse_locations_type", new[] { "storage", "picking", "packing", "damaged", "receiving" })
            .HasPostgresEnum("warehouse_status", new[] { "active", "inactive" });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_order");

            entity.ToTable("orders");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExternalOrderId).HasColumnName("external_order_id");
            entity.Property(e => e.PickedAt).HasColumnName("picked_at");
            entity.Property(e => e.ReservedAt).HasColumnName("reserved_at");
            entity.Property(e => e.ShippedAt).HasColumnName("shipped_at");
            entity.Property(e => e.Status)
                .HasColumnName("status")
                .HasColumnType("order_status");
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_order_item");

            entity.ToTable("order_items");

            entity.HasIndex(e => new { e.OrderId, e.ProductId }, "uq_order_items_order_product").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.OrderedQuantity).HasColumnName("ordered_quantity");
            entity.Property(e => e.PickedQuantity).HasColumnName("picked_quantity");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.ReservedQuantity).HasColumnName("reserved_quantity");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_product");

            entity.ToTable("products");

            entity.HasIndex(e => e.Barcode, "unique_barcode").IsUnique();

            entity.HasIndex(e => e.Sku, "unique_sku").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Barcode).HasColumnName("barcode");
            entity.Property(e => e.HeightMm).HasColumnName("height_mm");
            entity.Property(e => e.IsActive).HasColumnName("is_active");
            entity.Property(e => e.LengthMm).HasColumnName("length_mm");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Sku).HasColumnName("sku");
            entity.Property(e => e.Weight).HasColumnName("weight");
            entity.Property(e => e.WidthMm).HasColumnName("width_mm");
        });

        modelBuilder.Entity<StockBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_stock_balance");

            entity.ToTable("stock_balances");

            entity.HasIndex(e => new { e.WarehouseId, e.LocationId, e.ProductId }, "uq_stock_balance_location_product").IsUnique();

            entity.HasIndex(e => new { e.WarehouseId, e.LocationId, e.ProductId }, "ux_stock_balances_location_product").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.QuantityOnHand).HasColumnName("quantity_on_hand");
            entity.Property(e => e.QuantityReserved).HasColumnName("quantity_reserved");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Product).WithMany(p => p.StockBalances)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_product");
        });

        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_movement_id");

            entity.ToTable("stock_movements");

            entity.HasIndex(e => new { e.ProductId, e.CreatedAt }, "ix_stock_movements_product_created_at").IsDescending(false, true);

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.CreatedBy).HasColumnName("created_by");
            entity.Property(e => e.FromLocationId).HasColumnName("from_location_id");
            entity.Property(e => e.ProductId).HasColumnName("product_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.Reason).HasColumnName("reason");
            entity.Property(e => e.ReferenceId).HasColumnName("reference_id");
            entity.Property(e => e.ReferenceType).HasColumnName("reference_type");
            entity.Property(e => e.ToLocationId).HasColumnName("to_location_id");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.FromLocation).WithMany(p => p.StockMovementFromLocations)
                .HasForeignKey(d => d.FromLocationId)
                .HasConstraintName("fk_from_location");

            entity.HasOne(d => d.Product).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_product");

            entity.HasOne(d => d.ToLocation).WithMany(p => p.StockMovementToLocations)
                .HasForeignKey(d => d.ToLocationId)
                .HasConstraintName("fk_to_location");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockMovements)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_warehouse");
        });

        modelBuilder.Entity<StockReservation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_reservation");

            entity.ToTable("stock_reservations");

            entity.HasIndex(e => e.ConsumedAt, "ix_stock_reservations_active_consumed").HasFilter("(status = 'active'::reservation_status)");

            entity.HasIndex(e => e.ConsumedAt, "ix_stock_reservations_active_expired").HasFilter("(status = 'active'::reservation_status)");

            entity.HasIndex(e => e.ConsumedAt, "ix_stock_reservations_consumed_at");

            entity.HasIndex(e => e.IdempotencyKey, "unique_idempotency_key").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CancelledAt).HasColumnName("cancelled_at");
            entity.Property(e => e.ConsumedAt).HasColumnName("consumed_at");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
            entity.Property(e => e.IdempotencyKey).HasColumnName("idempotency_key");
            entity.Property(e => e.LocationId).HasColumnName("location_id");
            entity.Property(e => e.OrderItemId).HasColumnName("order_item_id");
            entity.Property(e => e.Quantity).HasColumnName("quantity");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Location).WithMany(p => p.StockReservations)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_warehouse_location");

            entity.HasOne(d => d.OrderItem).WithMany(p => p.StockReservations)
                .HasForeignKey(d => d.OrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_order_item");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockReservations)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_warehouse");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_warehouse");

            entity.ToTable("warehouses");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.Name).HasColumnName("name");
        });

        modelBuilder.Entity<WarehouseLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_warehouse_location");

            entity.ToTable("warehouse_locations");

            entity.HasIndex(e => new { e.WarehouseId, e.Code }, "uq_location_code").IsUnique();

            entity.HasIndex(e => new { e.Id, e.WarehouseId }, "uq_location_in_warehouse").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.Capacity).HasColumnName("capacity");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.WarehouseId).HasColumnName("warehouse_id");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseLocations)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_warehouse");
        });

        modelBuilder.Entity<OutboxMessage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pk_outbox_message");

            entity.ToTable("outbox_messages");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Type).HasColumnName("type");
            entity.Property(e => e.OccurredOnUtc).HasColumnName("occurred_on_utc");
            entity.Property(e => e.ExpiresOnUtc).HasColumnName("expires_on_utc");
            entity.Property(e => e.Error).HasColumnName("error");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
