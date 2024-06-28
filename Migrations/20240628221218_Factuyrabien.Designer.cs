﻿// <auto-generated />
using System;
using CRUD.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CRUD.Migrations
{
    [DbContext(typeof(MyDbContext))]
    [Migration("20240628221218_Factuyrabien")]
    partial class Factuyrabien
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.5.24306.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CRUD.Models.Aula", b =>
                {
                    b.Property<string>("CodigoAula")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("Capacidad")
                        .HasColumnType("int");

                    b.Property<string>("TipoAula")
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.HasKey("CodigoAula");

                    b.ToTable("Aulas");
                });

            modelBuilder.Entity("CRUD.Models.Carrera", b =>
                {
                    b.Property<int>("CarreraId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CarreraId"));

                    b.Property<string>("Departamento")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DuracionPeriodos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NombreCarrera")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TotalCreditos")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CarreraId");

                    b.ToTable("Carreras");
                });

            modelBuilder.Entity("CRUD.Models.CarreraMateria", b =>
                {
                    b.Property<int>("CarreraId")
                        .HasColumnType("int");

                    b.Property<string>("CodigoMateria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("EstudianteId")
                        .HasColumnType("int");

                    b.HasKey("CarreraId", "CodigoMateria");

                    b.HasIndex("CodigoMateria");

                    b.HasIndex("EstudianteId");

                    b.ToTable("CarreraMaterias");
                });

            modelBuilder.Entity("CRUD.Models.CuentaPorPagar", b =>
                {
                    b.Property<int>("IdCuentaPorPagar")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCuentaPorPagar"));

                    b.Property<int>("CodigoEstudiante")
                        .HasColumnType("int");

                    b.Property<string>("CodigoMateria")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal?>("MontoTotalaPagar")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("IdCuentaPorPagar");

                    b.HasIndex("CodigoMateria", "CodigoEstudiante");

                    b.ToTable("CuentaPorPagars");
                });

            modelBuilder.Entity("CRUD.Models.Docente", b =>
                {
                    b.Property<int>("CodigoDocente")
                        .HasColumnType("int");

                    b.Property<string>("ContraseñaDocente")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CorreoDocente")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NombreDocente")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SexoDocente")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("TelefonoDocente")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("CodigoDocente");

                    b.ToTable("Docentes");
                });

            modelBuilder.Entity("CRUD.Models.EstudianteMateria", b =>
                {
                    b.Property<string>("CodigoMateria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("CodigoEstudiante")
                        .HasColumnType("int");

                    b.Property<string>("Calificacion")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("PeriodoCursado")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("SeccionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("CodigoMateria", "CodigoEstudiante");

                    b.HasIndex("CodigoEstudiante");

                    b.HasIndex("SeccionId");

                    b.ToTable("EstudianteMaterias");
                });

            modelBuilder.Entity("CRUD.Models.Factura", b =>
                {
                    b.Property<int>("IdFactura")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdFactura"));

                    b.Property<int>("CodigoEstudiante")
                        .HasColumnType("int");

                    b.Property<int?>("CuentaPorPagarIdCuentaPorPagar")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaCreacion")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("MontoTotal")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Periodo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdFactura");

                    b.HasIndex("CodigoEstudiante");

                    b.HasIndex("CuentaPorPagarIdCuentaPorPagar");

                    b.ToTable("Facturas");
                });

            modelBuilder.Entity("CRUD.Models.Materia", b =>
                {
                    b.Property<string>("CodigoMateria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("AreaAcademica")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("CreditosMateria")
                        .HasColumnType("int");

                    b.Property<string>("NombreMateria")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("TipoMateria")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CodigoMateria");

                    b.ToTable("Materias");
                });

            modelBuilder.Entity("CRUD.Models.MateriaAula", b =>
                {
                    b.Property<string>("AulaId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodigoMateria")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("AulaId", "CodigoMateria");

                    b.HasIndex("CodigoMateria");

                    b.ToTable("MateriaAulas");
                });

            modelBuilder.Entity("CRUD.Models.MateriaDocente", b =>
                {
                    b.Property<int>("DocenteId")
                        .HasColumnType("int");

                    b.Property<string>("CodigoMateria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("SeccionId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("DocenteId", "CodigoMateria");

                    b.HasIndex("CodigoMateria");

                    b.HasIndex("SeccionId");

                    b.ToTable("MateriaDocentes");
                });

            modelBuilder.Entity("CRUD.Models.Seccion", b =>
                {
                    b.Property<string>("CodigoSeccion")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CodigoMateria")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Cupo")
                        .HasColumnType("int");

                    b.Property<string>("Horario")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CodigoSeccion");

                    b.HasIndex("CodigoMateria");

                    b.ToTable("Secciones");
                });

            modelBuilder.Entity("Estudiante", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int?>("CarreraId")
                        .HasColumnType("int");

                    b.Property<string>("CiudadEstudiante")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CondicionAcademica")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ContraseñaEstudiante")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CorreoEstudiante")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int?>("CreditosAprobados")
                        .HasColumnType("int");

                    b.Property<string>("DireccionEstudiante")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<DateTime>("FechaIngreso")
                        .HasColumnType("datetime2");

                    b.Property<decimal?>("IndiceGeneral")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<decimal?>("IndicePeriodo")
                        .HasColumnType("decimal(18, 2)");

                    b.Property<string>("Nacionalidad")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NombreEstudiante")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PeriodoActual")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PeriodosCursados")
                        .HasColumnType("int");

                    b.Property<string>("SexoEstudiante")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("TelefonoEstudiante")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.HasIndex("CarreraId");

                    b.ToTable("Estudiantes");
                });

            modelBuilder.Entity("CRUD.Models.CarreraMateria", b =>
                {
                    b.HasOne("CRUD.Models.Carrera", "Carreras")
                        .WithMany("CarreraMaterias")
                        .HasForeignKey("CarreraId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.Materia", "Materias")
                        .WithMany("CarreraMaterias")
                        .HasForeignKey("CodigoMateria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Estudiante", null)
                        .WithMany("CarreraMaterias")
                        .HasForeignKey("EstudianteId");

                    b.Navigation("Carreras");

                    b.Navigation("Materias");
                });

            modelBuilder.Entity("CRUD.Models.CuentaPorPagar", b =>
                {
                    b.HasOne("CRUD.Models.EstudianteMateria", "EstudianteMateria")
                        .WithMany("CuentaPorPagars")
                        .HasForeignKey("CodigoMateria", "CodigoEstudiante")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EstudianteMateria");
                });

            modelBuilder.Entity("CRUD.Models.EstudianteMateria", b =>
                {
                    b.HasOne("Estudiante", "Estudiantes")
                        .WithMany("EstudianteMaterias")
                        .HasForeignKey("CodigoEstudiante")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.Materia", "Materias")
                        .WithMany("EstudianteMaterias")
                        .HasForeignKey("CodigoMateria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.Seccion", "Seccions")
                        .WithMany("EstudianteMaterias")
                        .HasForeignKey("SeccionId");

                    b.Navigation("Estudiantes");

                    b.Navigation("Materias");

                    b.Navigation("Seccions");
                });

            modelBuilder.Entity("CRUD.Models.Factura", b =>
                {
                    b.HasOne("Estudiante", "Estudiante")
                        .WithMany("Facturas")
                        .HasForeignKey("CodigoEstudiante")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.CuentaPorPagar", null)
                        .WithMany("Facturas")
                        .HasForeignKey("CuentaPorPagarIdCuentaPorPagar");

                    b.Navigation("Estudiante");
                });

            modelBuilder.Entity("CRUD.Models.MateriaAula", b =>
                {
                    b.HasOne("CRUD.Models.Aula", "Aulas")
                        .WithMany("MateriaAulas")
                        .HasForeignKey("AulaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.Materia", "Materias")
                        .WithMany("MateriaAulas")
                        .HasForeignKey("CodigoMateria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Aulas");

                    b.Navigation("Materias");
                });

            modelBuilder.Entity("CRUD.Models.MateriaDocente", b =>
                {
                    b.HasOne("CRUD.Models.Materia", "Materias")
                        .WithMany("MateriaDocentes")
                        .HasForeignKey("CodigoMateria")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.Docente", "Docentes")
                        .WithMany("MateriaDocentes")
                        .HasForeignKey("DocenteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CRUD.Models.Seccion", "Seccions")
                        .WithMany("MateriaDocentes")
                        .HasForeignKey("SeccionId");

                    b.Navigation("Docentes");

                    b.Navigation("Materias");

                    b.Navigation("Seccions");
                });

            modelBuilder.Entity("CRUD.Models.Seccion", b =>
                {
                    b.HasOne("CRUD.Models.Materia", "Materias")
                        .WithMany("Seccions")
                        .HasForeignKey("CodigoMateria");

                    b.Navigation("Materias");
                });

            modelBuilder.Entity("Estudiante", b =>
                {
                    b.HasOne("CRUD.Models.Carrera", "Carreras")
                        .WithMany("Estudiantes")
                        .HasForeignKey("CarreraId");

                    b.Navigation("Carreras");
                });

            modelBuilder.Entity("CRUD.Models.Aula", b =>
                {
                    b.Navigation("MateriaAulas");
                });

            modelBuilder.Entity("CRUD.Models.Carrera", b =>
                {
                    b.Navigation("CarreraMaterias");

                    b.Navigation("Estudiantes");
                });

            modelBuilder.Entity("CRUD.Models.CuentaPorPagar", b =>
                {
                    b.Navigation("Facturas");
                });

            modelBuilder.Entity("CRUD.Models.Docente", b =>
                {
                    b.Navigation("MateriaDocentes");
                });

            modelBuilder.Entity("CRUD.Models.EstudianteMateria", b =>
                {
                    b.Navigation("CuentaPorPagars");
                });

            modelBuilder.Entity("CRUD.Models.Materia", b =>
                {
                    b.Navigation("CarreraMaterias");

                    b.Navigation("EstudianteMaterias");

                    b.Navigation("MateriaAulas");

                    b.Navigation("MateriaDocentes");

                    b.Navigation("Seccions");
                });

            modelBuilder.Entity("CRUD.Models.Seccion", b =>
                {
                    b.Navigation("EstudianteMaterias");

                    b.Navigation("MateriaDocentes");
                });

            modelBuilder.Entity("Estudiante", b =>
                {
                    b.Navigation("CarreraMaterias");

                    b.Navigation("EstudianteMaterias");

                    b.Navigation("Facturas");
                });
#pragma warning restore 612, 618
        }
    }
}
