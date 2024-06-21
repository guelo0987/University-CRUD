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
    [Migration("20240621044819_DeleteKeyAnnotation")]
    partial class DeleteKeyAnnotation
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0-preview.5.24306.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

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

                    b.Property<int>("CodigoMateria")
                        .HasColumnType("int");

                    b.HasKey("CarreraId", "CodigoMateria");

                    b.HasIndex("CodigoMateria");

                    b.ToTable("CarreraMaterias");
                });

            modelBuilder.Entity("CRUD.Models.Materia", b =>
                {
                    b.Property<int>("CodigoMateria")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CodigoMateria"));

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

            modelBuilder.Entity("Estudiante", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CarreraId")
                        .HasColumnType("int");

                    b.Property<string>("CiudadEstudiante")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("CodigoCarrera")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

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

                    b.Navigation("Carreras");

                    b.Navigation("Materias");
                });

            modelBuilder.Entity("Estudiante", b =>
                {
                    b.HasOne("CRUD.Models.Carrera", "Carreras")
                        .WithMany("Estudiantes")
                        .HasForeignKey("CarreraId");

                    b.Navigation("Carreras");
                });

            modelBuilder.Entity("CRUD.Models.Carrera", b =>
                {
                    b.Navigation("CarreraMaterias");

                    b.Navigation("Estudiantes");
                });

            modelBuilder.Entity("CRUD.Models.Materia", b =>
                {
                    b.Navigation("CarreraMaterias");
                });
#pragma warning restore 612, 618
        }
    }
}
