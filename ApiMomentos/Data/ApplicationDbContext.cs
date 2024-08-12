using System;
using System.Collections.Generic;
using ApiObjetos.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiObjetos.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Articulo> Articulos { get; set; }

    public virtual DbSet<CategoriasHabitaciones> CategoriasHabitaciones { get; set; }

    public virtual DbSet<Consumo> Consumos { get; set; }

    public virtual DbSet<DiasSemana> DiasSemanas { get; set; }

    public virtual DbSet<Habitaciones> Habitaciones { get; set; }

    public virtual DbSet<HabitacionesVirtuales> HabitacionesVirtuales { get; set; }

    public virtual DbSet<InventarioInicial> InventarioInicials { get; set; }

    public virtual DbSet<MediosPago> MediosPagos { get; set; }

    public virtual DbSet<Movimiento> Movimientos { get; set; }

    public virtual DbSet<MovimientosServicio> MovimientosServicios { get; set; }

    public virtual DbSet<MovimientosStock> MovimientosStocks { get; set; }

    public virtual DbSet<MovimientosUsuario> MovimientosUsuarios { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Personal> Personals { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ServiciosAdicionale> ServiciosAdicionales { get; set; }

    public virtual DbSet<Tarifa> Tarifas { get; set; }

    public virtual DbSet<TipoMovimiento> TipoMovimientos { get; set; }

    public virtual DbSet<TipoTarifa> TipoTarifas { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<Visita> Visitas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("SQL_Latin1_General_CP1_CI_AS");

        modelBuilder.Entity<Articulo>(entity =>
        {
            entity.HasKey(e => e.ArticuloId).HasName("PK__Articulo__C0D7258D17E6EF80");

            entity.Property(e => e.ArticuloId).HasColumnName("ArticuloID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NombreArticulo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<CategoriasHabitaciones>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1C5CB370FC8");

            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrecioNormal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<Consumo>(entity =>
        {
            entity.HasKey(e => e.ConsumoId).HasName("PK__Consumo__206D9CC6C6EEA4D5");

            entity.ToTable("Consumo");

            entity.Property(e => e.ConsumoId).HasColumnName("ConsumoID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.ArticuloId).HasColumnName("ArticuloID");
            entity.Property(e => e.MovimientosId).HasColumnName("MovimientosID");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Articulo).WithMany(p => p.Consumos)
                .HasForeignKey(d => d.ArticuloId)
                .HasConstraintName("FK__Consumo__Articul__793DFFAF");

            entity.HasOne(d => d.Movimientos).WithMany(p => p.Consumos)
                .HasForeignKey(d => d.MovimientosId)
                .HasConstraintName("FK__Consumo__Movimie__7849DB76");
        });

        modelBuilder.Entity<DiasSemana>(entity =>
        {
            entity.HasKey(e => e.DiaSemanaId).HasName("PK__DiasSema__C5898FE1E9681C02");

            entity.ToTable("DiasSemana");

            entity.Property(e => e.DiaSemanaId).HasColumnName("DiaSemanaID");
            entity.Property(e => e.NombreDiaSemana)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Habitaciones>(entity =>
        {
            entity.HasKey(e => e.HabitacionId).HasName("PK__Habitaci__11AD4441DE85318D");

            entity.Property(e => e.HabitacionId).HasColumnName("HabitacionID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.Disponible)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NombreHabitacion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ProximaReserva).HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Habitaciones)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK__Habitacio__Categ__2739D489");
        });

        modelBuilder.Entity<HabitacionesVirtuales>(entity =>
        {
            entity.HasKey(e => e.HabitacionVirtualId).HasName("PK__Habitaci__31AD1AAA5FB01D72");

            entity.Property(e => e.HabitacionVirtualId).HasColumnName("HabitacionVirtualID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Habitacion1Id).HasColumnName("Habitacion1ID");
            entity.Property(e => e.Habitacion2Id).HasColumnName("Habitacion2ID");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Habitacion1).WithMany(p => p.HabitacionesVirtualesHabitacion1s)
                .HasForeignKey(d => d.Habitacion1Id)
                .HasConstraintName("FK__Habitacio__Habit__3B40CD36");

            entity.HasOne(d => d.Habitacion2).WithMany(p => p.HabitacionesVirtualesHabitacion2s)
                .HasForeignKey(d => d.Habitacion2Id)
                .HasConstraintName("FK__Habitacio__Habit__3C34F16F");
        });

        modelBuilder.Entity<InventarioInicial>(entity =>
        {
            entity.HasKey(e => e.ArticuloId).HasName("PK__Inventar__C0D7258D7CEAADA9");

            entity.ToTable("InventarioInicial");

            entity.Property(e => e.ArticuloId)
                .ValueGeneratedNever()
                .HasColumnName("ArticuloID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");

            entity.HasOne(d => d.Articulo).WithOne(p => p.InventarioInicial)
                .HasForeignKey<InventarioInicial>(d => d.ArticuloId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventari__Artic__7E02B4CC");
        });

        modelBuilder.Entity<MediosPago>(entity =>
        {
            entity.HasKey(e => e.MedioPagoId).HasName("PK__MediosPa__6D54078ED1D306FC");

            entity.ToTable("MediosPago");

            entity.Property(e => e.MedioPagoId).HasColumnName("MedioPagoID");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Movimiento>(entity =>
        {
            entity.HasKey(e => e.MovimientosId).HasName("PK__Movimien__1B3A75F85AC62449");

            entity.Property(e => e.MovimientosId).HasColumnName("MovimientosID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaInicio).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.HabitacionId).HasColumnName("HabitacionID");
            entity.Property(e => e.TotalFacturado).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.VisitaId).HasColumnName("VisitaID");

            entity.HasOne(d => d.Habitacion).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.HabitacionId)
                .HasConstraintName("FK__Movimient__Habit__30C33EC3");

            entity.HasOne(d => d.Visita).WithMany(p => p.Movimientos)
                .HasForeignKey(d => d.VisitaId)
                .HasConstraintName("FK__Movimient__Visit__2FCF1A8A");

            entity.HasOne(d => d.Pago)
.           WithMany(p => p.Movimientos)
            .HasForeignKey(d => d.PagoId)
            .HasConstraintName("FK_Movimientos_Pago");

        });

        modelBuilder.Entity<MovimientosServicio>(entity =>
        {
            entity.HasKey(e => e.MovimientosServicioId).HasName("PK__Movimien__518225B897E0CDB2");

            entity.Property(e => e.MovimientosServicioId).HasColumnName("MovimientosServicioID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.MovimientosId).HasColumnName("MovimientosID");
            entity.Property(e => e.ServicioId).HasColumnName("ServicioID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Movimientos).WithMany(p => p.MovimientosServicios)
                .HasForeignKey(d => d.MovimientosId)
                .HasConstraintName("FK__Movimient__Movim__42E1EEFE");

            entity.HasOne(d => d.Servicio).WithMany(p => p.MovimientosServicios)
                .HasForeignKey(d => d.ServicioId)
                .HasConstraintName("FK__Movimient__Servi__43D61337");
        });

        modelBuilder.Entity<MovimientosStock>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PK__Movimien__BF923FCC1993AA0F");

            entity.ToTable("Movimientos_Stock");

            entity.Property(e => e.MovimientoId).HasColumnName("MovimientoID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.ArticuloId).HasColumnName("ArticuloID");
            entity.Property(e => e.FechaMovimiento).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.MovimientosId).HasColumnName("MovimientosID");
            entity.Property(e => e.TipoMovimientoId).HasColumnName("TipoMovimientoID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Articulo).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.ArticuloId)
                .HasConstraintName("FK__Movimient__Artic__5CA1C101");

            entity.HasOne(d => d.Movimientos).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.MovimientosId)
                .HasConstraintName("FK__Movimient__Movim__5E8A0973");

            entity.HasOne(d => d.TipoMovimiento).WithMany(p => p.MovimientosStocks)
                .HasForeignKey(d => d.TipoMovimientoId)
                .HasConstraintName("FK__Movimient__TipoM__5D95E53A");
        });

        modelBuilder.Entity<MovimientosUsuario>(entity =>
        {
            entity.HasKey(e => e.MovimientoId).HasName("PK__Movimien__BF923FCC31DCE6C3");

            entity.Property(e => e.MovimientoId).HasColumnName("MovimientoID");
            entity.Property(e => e.Accion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FechaHora).HasColumnType("datetime");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Usuario).WithMany(p => p.MovimientosUsuarios)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MovimientosUsuarios_Usuarios");
        });

        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B615896F7F706");

            entity.Property(e => e.PagoId).HasColumnName("PagoID");
            entity.Property(e => e.MedioPagoId).HasColumnName("MedioPagoID");
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.OrigenId).HasColumnName("OrigenID");
            entity.Property(e => e.TipoOrigen)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.MedioPago).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.MedioPagoId)
                .HasConstraintName("FK_Pagos_MediosPago");
        });



        modelBuilder.Entity<Personal>(entity =>
        {
            entity.HasKey(e => e.PersonalId).HasName("PK__Personal__283437138A5F91D4");

            entity.ToTable("Personal");

            entity.Property(e => e.PersonalId).HasColumnName("PersonalID");
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasColumnName("RolID");

            entity.HasOne(d => d.Rol).WithMany(p => p.Personals)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Personal_Roles");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.ReservaId).HasName("PK__Reservas__C39937031EAE2A3E");

            entity.Property(e => e.ReservaId).HasColumnName("ReservaID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaFin).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.FechaReserva).HasColumnType("datetime");
            entity.Property(e => e.HabitacionId).HasColumnName("HabitacionID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.VisitaId).HasColumnName("VisitaID");

            entity.HasOne(d => d.Habitacion).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.HabitacionId)
                .HasConstraintName("FK__Reservas__Habita__2BFE89A6");

            entity.HasOne(d => d.Visita).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.VisitaId)
                .HasConstraintName("FK__Reservas__Visita__2B0A656D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302D13E9BFBA1");

            entity.Property(e => e.RolId).HasColumnName("RolID");
            entity.Property(e => e.NombreRol)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ServiciosAdicionale>(entity =>
        {
            entity.HasKey(e => e.ServicioId).HasName("PK__Servicio__D5AEEC221E25B13E");

            entity.Property(e => e.ServicioId).HasColumnName("ServicioID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NombreServicio)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<Tarifa>(entity =>
        {
            entity.HasKey(e => e.TarifaId).HasName("PK__Tarifas__E81AC21B585D7CD5");

            entity.Property(e => e.TarifaId).HasColumnName("TarifaID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.CategoriaId).HasColumnName("CategoriaID");
            entity.Property(e => e.DetalleTarifa)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DiaSemanaId).HasColumnName("DiaSemanaID");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.TipoTarifaId).HasColumnName("TipoTarifaID");
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Tarifas)
                .HasForeignKey(d => d.CategoriaId)
                .HasConstraintName("FK__Tarifas__Categor__2180FB33");

            entity.HasOne(d => d.DiaSemana).WithMany(p => p.Tarifas)
                .HasForeignKey(d => d.DiaSemanaId)
                .HasConstraintName("FK__Tarifas__DiaSema__22751F6C");

            entity.HasOne(d => d.TipoTarifa).WithMany(p => p.Tarifas)
                .HasForeignKey(d => d.TipoTarifaId)
                .HasConstraintName("FK__Tarifas__TipoTar__236943A5");
        });

        modelBuilder.Entity<TipoMovimiento>(entity =>
        {
            entity.HasKey(e => e.TipoMovimientoId).HasName("PK__Tipo_Mov__097C73011746A78A");

            entity.ToTable("Tipo_Movimiento");

            entity.Property(e => e.TipoMovimientoId).HasColumnName("TipoMovimientoID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.NombreTipoMovimiento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Tipo)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TipoTarifa>(entity =>
        {
            entity.HasKey(e => e.TipoTarifaId).HasName("PK__TipoTari__92EDBAA1CC6254D0");

            entity.ToTable("TipoTarifa");

            entity.Property(e => e.TipoTarifaId).HasColumnName("TipoTarifaID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NombreTipoTarifa)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE798F38AEFCF");

            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
            entity.Property(e => e.Contraseña)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreUsuario)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RolId).HasColumnName("RolID");

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        modelBuilder.Entity<Visita>(entity =>
        {
            entity.HasKey(e => e.VisitaId).HasName("PK__Visitas__D8D4BC22AFD09745");

            entity.Property(e => e.VisitaId).HasColumnName("VisitaID");
            entity.Property(e => e.Anulado).HasDefaultValueSql("((0))");
            entity.Property(e => e.FechaPrimerIngreso).HasColumnType("datetime");
            entity.Property(e => e.FechaRegistro).HasColumnType("datetime");
            entity.Property(e => e.NumeroTelefono)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PatenteVehiculo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UsuarioId).HasColumnName("UsuarioID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
