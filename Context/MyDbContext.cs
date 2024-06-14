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

    public virtual DbSet<Estudiante> Estudiantes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Estudiante>(entity =>
        {

            entity.HasOne(d => d.Carreras).WithMany(p => p.Estudiantes)
                .HasForeignKey(d => d.CarreraId)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });



       
    }
    
}
