using System;
using System.Collections.Generic;

using CRUD.Models;
using Microsoft.EntityFrameworkCore;
using PassHash;

namespace CRUD.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Carrera> Carreras { get; set; }
    
    public virtual DbSet<Materia> Materias { get; set; }

    public virtual DbSet<CarreraMateria> CarreraMaterias{ get; set; }

    public virtual DbSet<Estudiante> Estudiantes { get; set; }
    
    public virtual DbSet<EstudianteMateria> EstudianteMaterias { get; set; }
    
    
    public virtual DbSet<Docente> Docentes { get; set; }
    
    
    public virtual DbSet<MateriaDocente> MateriaDocentes { get; set; }
    
    public virtual DbSet<Aula> Aulas { get; set; }
    
    
    public virtual DbSet<MateriaAula> MateriaAulas { get; set; }

    
    public virtual DbSet<Seccion> Secciones { get; set; }
    
    public virtual DbSet<MateriaSeccion> SeccionMaterias { get; set; }
    
    
    public virtual DbSet<CuentasPorPagar> CuentasPorPagars { get; set; }
    
    
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(entity =>
        {

            entity.HasOne(d => d.Carreras).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CarreraId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });
        
        modelBuilder.Entity<CarreraMateria>()
            .HasKey(cm => new { cm.CarreraId, cm.CodigoMateria });

        modelBuilder.Entity<CarreraMateria>()
            .HasOne(cm => cm.Carreras)
            .WithMany(c => c.CarreraMaterias)
            .HasForeignKey(cm => cm.CarreraId);

        modelBuilder.Entity<CarreraMateria>()
            .HasOne(cm => cm.Materias)
            .WithMany(m => m.CarreraMaterias)
            .HasForeignKey(cm => cm.CodigoMateria);
        
        
        // Configuración de EstudianteMateria
        modelBuilder.Entity<EstudianteMateria>()
            .HasKey(em => new { em.CodigoMateria, em.CodigoEstudiante });

        modelBuilder.Entity<EstudianteMateria>()
            .HasOne(em => em.Materias)
            .WithMany(m => m.EstudianteMaterias)
            .HasForeignKey(em => em.CodigoMateria);

        modelBuilder.Entity<EstudianteMateria>()
            .HasOne(em => em.Estudiantes)
            .WithMany(e => e.EstudianteMaterias)
            .HasForeignKey(em => em.CodigoEstudiante);
        
        
        modelBuilder.Entity<MateriaDocente>()
            .HasKey(md => new { md.DocenteId, md.CodigoMateria });

        modelBuilder.Entity<MateriaDocente>()
            .HasOne(md => md.Docentes)
            .WithMany(d => d.MateriaDocentes)
            .HasForeignKey(md => md.DocenteId);

        modelBuilder.Entity<MateriaDocente>()
            .HasOne(md => md.Materias)
            .WithMany(m => m.MateriaDocentes)
            .HasForeignKey(md => md.CodigoMateria);
        
        
        modelBuilder.Entity<MateriaAula>()
            .HasKey(ma => new { ma.AulaId, ma.CodigoMateria });

        modelBuilder.Entity<MateriaAula>()
            .HasOne(ma => ma.Aulas)
            .WithMany(a => a.MateriaAulas)
            .HasForeignKey(ma => ma.AulaId);

        modelBuilder.Entity<MateriaAula>()
            .HasOne(ma => ma.Materias)
            .WithMany(m => m.MateriaAulas)
            .HasForeignKey(ma => ma.CodigoMateria);
        
        
        modelBuilder.Entity<MateriaSeccion>()
            .HasKey(ms => new { ms.CodigoMateria, ms.CodigoSeccion });

        modelBuilder.Entity<MateriaSeccion>()
            .HasOne(ms => ms.Materia)
            .WithMany(m => m.MateriaSecciones)
            .HasForeignKey(ms => ms.CodigoMateria);

        modelBuilder.Entity<MateriaSeccion>()
            .HasOne(ms => ms.Seccion)
            .WithMany(s => s.MateriaSecciones)
            .HasForeignKey(ms => ms.CodigoSeccion);
        
        
        modelBuilder.Entity<CuentasPorPagar>()
            .HasOne(c => c.EstudianteMateria)
            .WithMany(em => em.CuentasPorPagars)
            .HasForeignKey(c => new { c.CodigoMateria, c.CodigoEstudiante });
            
        


       
    }
    
}
