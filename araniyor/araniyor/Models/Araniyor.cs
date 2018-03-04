namespace araniyor.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Araniyor : DbContext
    {
        public Araniyor()
            : base("name=Araniyor")
        {
        }

        public virtual DbSet<blocked> blocked { get; set; }
        public virtual DbSet<businessCategory> businessCategory { get; set; }
        public virtual DbSet<businessSubategory> businessSubategory { get; set; }
        public virtual DbSet<messages> messages { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<businessCategory>()
                .HasMany(e => e.businessSubategory)
                .WithRequired(e => e.businessCategory)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.gender)
                .IsFixedLength();

            modelBuilder.Entity<Users>()
                .HasMany(e => e.blocked)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.blockerID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.blocked1)
                .WithRequired(e => e.Users1)
                .HasForeignKey(e => e.blockedID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.messages)
                .WithRequired(e => e.Users)
                .HasForeignKey(e => e.senderID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .HasMany(e => e.messages1)
                .WithRequired(e => e.Users1)
                .HasForeignKey(e => e.receiverID)
                .WillCascadeOnDelete(false);
        }
    }
}
