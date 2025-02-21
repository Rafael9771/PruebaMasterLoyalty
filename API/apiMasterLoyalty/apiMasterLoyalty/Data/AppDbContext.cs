using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using apiMasterLoyalty.Models;

namespace apiMasterLoyalty.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<Carrito> Carritos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<TiendaArticulo> TiendaArticulos { get; set; }

    public virtual DbSet<Tiendum> Tienda { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-DTMN536\\ROXSQLSERVER;Database=DBMasterLoyalty;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.ArId).HasName("pk_articulo");

            entity.ToTable("articulo");

            entity.Property(e => e.ArId).HasColumnName("arId");
            entity.Property(e => e.ArCodigo)
                .HasDefaultValue("")
                .HasColumnName("ar_codigo");
            entity.Property(e => e.ArDcreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ar_dcreate");
            entity.Property(e => e.ArDescripcion)
                .HasDefaultValue("")
                .HasColumnName("ar_descripcion");
            entity.Property(e => e.ArGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("arGuid");
            entity.Property(e => e.ArImagen)
                .HasDefaultValue("")
                .HasColumnName("ar_imagen");
            entity.Property(e => e.ArNombre)
                .HasDefaultValue("")
                .HasColumnName("ar_nombre");
            entity.Property(e => e.ArPrecio)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("ar_precio");
            entity.Property(e => e.ArStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("ar_status");
            entity.Property(e => e.ArStock).HasColumnName("ar_stock");
        });

        modelBuilder.Entity<Carrito>(entity =>
        {
            entity.HasKey(e => e.CaId).HasName("pk_carrito");

            entity.ToTable("carrito");

            entity.Property(e => e.CaId).HasColumnName("caId");
            entity.Property(e => e.CaArId).HasColumnName("ca_arId");
            entity.Property(e => e.CaCantidad).HasColumnName("ca_cantidad");
            entity.Property(e => e.CaClId).HasColumnName("ca_clId");
            entity.Property(e => e.CaDcreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ca_dcreate");
            entity.Property(e => e.CaGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("caGuid");
            entity.Property(e => e.CaStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("ca_status");
            entity.Property(e => e.CaTiId).HasColumnName("ca_tiId");
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.CiId).HasName("pk_cliente");

            entity.ToTable("cliente");

            entity.Property(e => e.CiId).HasColumnName("ciId");
            entity.Property(e => e.CiCorreo)
                .HasDefaultValue("")
                .HasColumnName("ci_correo");
            entity.Property(e => e.CiDcreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ci_dcreate");
            entity.Property(e => e.CiGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("ciGuid");
            entity.Property(e => e.CiNombre)
                .HasDefaultValue("")
                .HasColumnName("ci_nombre");
            entity.Property(e => e.CiPassword)
                .HasDefaultValue("")
                .HasColumnName("ci_password");
            entity.Property(e => e.CiPrimerApellido)
                .HasDefaultValue("")
                .HasColumnName("ci_primer_apellido");
            entity.Property(e => e.CiRol)
                .HasMaxLength(1)
                .HasDefaultValue("2")
                .HasColumnName("ci_rol");
            entity.Property(e => e.CiSegundoApellido)
                .HasDefaultValue("")
                .HasColumnName("ci_segundo_apellido");
            entity.Property(e => e.CiStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("ci_status");
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.CoId).HasName("pk_compra");

            entity.ToTable("compra");

            entity.Property(e => e.CoId).HasColumnName("coId");
            entity.Property(e => e.CoArId).HasColumnName("co_arId");
            entity.Property(e => e.CoCantidad).HasColumnName("co_cantidad");
            entity.Property(e => e.CoClId).HasColumnName("co_clId");
            entity.Property(e => e.CoDcreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("co_dcreate");
            entity.Property(e => e.CoFolio)
                .HasDefaultValue("")
                .HasColumnName("co_folio");
            entity.Property(e => e.CoGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("coGuid");
            entity.Property(e => e.CoStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("co_status");
            entity.Property(e => e.CoTiId).HasColumnName("co_tiId");
        });

        modelBuilder.Entity<TiendaArticulo>(entity =>
        {
            entity.HasKey(e => e.TiarId).HasName("pk_tienda_articulo");

            entity.ToTable("tienda_articulo");

            entity.Property(e => e.TiarId).HasColumnName("tiarId");
            entity.Property(e => e.TiarArId).HasColumnName("tiar_arId");
            entity.Property(e => e.TiarDcreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("tiar_dcreate");
            entity.Property(e => e.TiarGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("tiarGuid");
            entity.Property(e => e.TiarStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("tiar_status");
            entity.Property(e => e.TiarTiId).HasColumnName("tiar_tiId");
        });

        modelBuilder.Entity<Tiendum>(entity =>
        {
            entity.HasKey(e => e.TiId).HasName("pk_tienda");

            entity.ToTable("tienda");

            entity.Property(e => e.TiId).HasColumnName("tiId");
            entity.Property(e => e.TiDcreate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("ti_dcreate");
            entity.Property(e => e.TiDireccion)
                .HasDefaultValue("")
                .HasColumnName("ti_direccion");
            entity.Property(e => e.TiGuid)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("tiGuid");
            entity.Property(e => e.TiStatus)
                .HasDefaultValue((byte)1)
                .HasColumnName("ti_status");
            entity.Property(e => e.TiSucursal)
                .HasDefaultValue("")
                .HasColumnName("ti_sucursal");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
