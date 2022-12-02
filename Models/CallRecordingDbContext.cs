using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace CallRecording.Models;

public partial class CallRecordingDbContext : DbContext
{
    public CallRecordingDbContext()
    {
    }

    public CallRecordingDbContext(DbContextOptions<CallRecordingDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Event> Events { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=C:\\Users\\ArciV\\source\\repos\\CallRecording\\CallRecording\\DB\\CallRecordingDB.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasOne(d => d.UserNavigation).WithMany(p => p.Events).OnDelete(DeleteBehavior.ClientSetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
