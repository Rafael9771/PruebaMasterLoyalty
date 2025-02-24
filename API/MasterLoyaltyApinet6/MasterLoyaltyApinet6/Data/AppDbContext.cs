using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MasterLoyaltyApinet6.Models;

namespace MasterLoyaltyApinet6.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Articulo> Articulos { get; set; } = null!;
        public virtual DbSet<Carrito> Carritos { get; set; } = null!;
        public virtual DbSet<Cliente> Clientes { get; set; } = null!;
        public virtual DbSet<Compra> Compras { get; set; } = null!;
        public virtual DbSet<TiendaArticulo> TiendaArticulos { get; set; } = null!;
        public virtual DbSet<Tiendum> Tienda { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-DTMN536\\ROXSQLSERVER;Database=DBMasterLoyalty;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.HasKey(e => e.ArId)
                    .HasName("pk_articulo");

                entity.ToTable("articulo");

                entity.Property(e => e.ArId).HasColumnName("arId");

                entity.Property(e => e.ArCodigo)
                    .HasColumnName("ar_codigo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ArDcreate)
                    .HasColumnType("datetime")
                    .HasColumnName("ar_dcreate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ArDescripcion)
                    .HasColumnName("ar_descripcion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ArGuid)
                    .HasColumnName("arGuid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArImagen)
                    .HasColumnName("ar_imagen")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ArNombre)
                    .HasColumnName("ar_nombre")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ArPrecio)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("ar_precio")
                    .HasDefaultValueSql("((0.00))");

                entity.Property(e => e.ArStatus)
                    .HasColumnName("ar_status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Carrito>(entity =>
            {
                entity.HasKey(e => e.CaId)
                    .HasName("pk_carrito");

                entity.ToTable("carrito");

                entity.Property(e => e.CaId).HasColumnName("caId");

                entity.Property(e => e.CaArTiId).HasColumnName("ca_arTiId");

                entity.Property(e => e.CaCantidad).HasColumnName("ca_cantidad");

                entity.Property(e => e.CaClId).HasColumnName("ca_clId");

                entity.Property(e => e.CaDcreate)
                    .HasColumnType("datetime")
                    .HasColumnName("ca_dcreate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CaGuid)
                    .HasColumnName("caGuid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CaStatus)
                    .HasColumnName("ca_status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.CiId)
                    .HasName("pk_cliente");

                entity.ToTable("cliente");

                entity.Property(e => e.CiId).HasColumnName("ciId");

                entity.Property(e => e.CiCorreo)
                    .HasColumnName("ci_correo")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CiDcreate)
                    .HasColumnType("datetime")
                    .HasColumnName("ci_dcreate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CiGuid)
                    .HasColumnName("ciGuid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CiNombre)
                    .HasColumnName("ci_nombre")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CiPassword)
                    .HasColumnName("ci_password")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CiPrimerApellido)
                    .HasColumnName("ci_primer_apellido")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CiRol)
                    .HasMaxLength(1)
                    .HasColumnName("ci_rol")
                    .HasDefaultValueSql("('2')");

                entity.Property(e => e.CiSegundoApellido)
                    .HasColumnName("ci_segundo_apellido")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CiStatus)
                    .HasColumnName("ci_status")
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Compra>(entity =>
            {
                entity.HasKey(e => e.CoId)
                    .HasName("pk_compra");

                entity.ToTable("compra");

                entity.Property(e => e.CoId).HasColumnName("coId");

                entity.Property(e => e.CoArId).HasColumnName("co_arId");

                entity.Property(e => e.CoCantidad).HasColumnName("co_cantidad");

                entity.Property(e => e.CoClId).HasColumnName("co_clId");

                entity.Property(e => e.CoDcreate)
                    .HasColumnType("datetime")
                    .HasColumnName("co_dcreate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CoFolio)
                    .HasColumnName("co_folio")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CoGuid)
                    .HasColumnName("coGuid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CoStatus)
                    .HasColumnName("co_status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CoTiId).HasColumnName("co_tiId");
            });

            modelBuilder.Entity<TiendaArticulo>(entity =>
            {
                entity.HasKey(e => e.TiarId)
                    .HasName("pk_tienda_articulo");

                entity.ToTable("tienda_articulo");

                entity.Property(e => e.TiarId).HasColumnName("tiarId");

                entity.Property(e => e.TiarArId).HasColumnName("tiar_arId");

                entity.Property(e => e.TiarDcreate)
                    .HasColumnType("datetime")
                    .HasColumnName("tiar_dcreate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TiarGuid)
                    .HasColumnName("tiarGuid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TiarStatus)
                    .HasColumnName("tiar_status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TiarStockTienda)
                    .HasColumnName("tiar_stockTienda")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TiarTiId).HasColumnName("tiar_tiId");
            });

            modelBuilder.Entity<Tiendum>(entity =>
            {
                entity.HasKey(e => e.TiId)
                    .HasName("pk_tienda");

                entity.ToTable("tienda");

                entity.Property(e => e.TiId).HasColumnName("tiId");

                entity.Property(e => e.TiDcreate)
                    .HasColumnType("datetime")
                    .HasColumnName("ti_dcreate")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.TiDireccion)
                    .HasColumnName("ti_direccion")
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.TiGuid)
                    .HasColumnName("tiGuid")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.TiStatus)
                    .HasColumnName("ti_status")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TiSucursal)
                    .HasColumnName("ti_sucursal")
                    .HasDefaultValueSql("('')");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
