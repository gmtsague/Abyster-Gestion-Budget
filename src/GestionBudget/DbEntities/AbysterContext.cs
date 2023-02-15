using System;
using System.Collections.Generic;
using DbEntities.Models;
using Microsoft.EntityFrameworkCore;

namespace DbEntities;

public partial class AbysterContext : DbContext
{
    public AbysterContext()
    {
    }

    public AbysterContext(DbContextOptions<AbysterContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Autorisation> Autorisations { get; set; }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Commande> Commandes { get; set; }

    public virtual DbSet<Groupe> Groupes { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=127.0.0.1;port=5433;Database=abyster;Username=abyster;Password=Abyster120");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Autorisation>(entity =>
        {
            entity.HasKey(e => e.Idautorisation).HasName("pk_autorisation");

            entity.ToTable("autorisation", "budget", tb => tb.HasComment("Autorisation"));

            entity.HasIndex(e => e.Idcmde, "association_3_fk");

            entity.HasIndex(e => e.Groupeid, "association_5_fk");

            entity.HasIndex(e => e.Idautorisation, "autorisation_pk").IsUnique();

            entity.Property(e => e.Idautorisation)
                .HasComment("Idautorisation")
                .HasColumnName("idautorisation");
            entity.Property(e => e.Groupeid)
                .HasComment("GroupeId")
                .HasColumnName("groupeid");
            entity.Property(e => e.Idcmde)
                .HasComment("Idcmde")
                .HasColumnName("idcmde");
            entity.Property(e => e.Isactive)
                .HasComment("Isactive")
                .HasColumnName("isactive");

            entity.HasOne(d => d.Groupe).WithMany(p => p.Autorisations)
                .HasForeignKey(d => d.Groupeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_autorisa_associati_groupe");

            entity.HasOne(d => d.Cmde).WithMany(p => p.Autorisations)
                .HasForeignKey(d => d.Idcmde)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_autorisa_associati_commande");
        });

        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.Idcategorie).HasName("pk_categorie");

            entity.ToTable("categorie", "budget", tb => tb.HasComment("Categorie"));

            entity.HasIndex(e => e.Idcategorie, "categorie_pk").IsUnique();

            entity.HasIndex(e => e.Libelle, "unique_categorie").IsUnique();

            entity.Property(e => e.Idcategorie)
                .HasComment("Idcategorie")
                .HasColumnName("idcategorie");
            entity.Property(e => e.Libelle)
                .HasMaxLength(254)
                .HasComment("Libelle")
                .HasColumnName("libelle");
        });

        modelBuilder.Entity<Commande>(entity =>
        {
            entity.HasKey(e => e.Idcmde).HasName("pk_commande");

            entity.ToTable("commande", "budget", tb => tb.HasComment("Commande"));

            entity.HasIndex(e => e.Idcmde, "commande_pk").IsUnique();

            entity.HasIndex(e => e.Libelle, "unique_commande").IsUnique();

            entity.Property(e => e.Idcmde)
                .HasComment("Idcmde")
                .HasColumnName("idcmde");
            entity.Property(e => e.Code)
                .HasComment("Code")
                .HasColumnName("code");
            entity.Property(e => e.Libelle)
                .HasMaxLength(254)
                .HasComment("Libelle")
                .HasColumnName("libelle");
        });

        modelBuilder.Entity<Groupe>(entity =>
        {
            entity.HasKey(e => e.Groupeid).HasName("pk_groupe");

            entity.ToTable("groupe", "budget", tb => tb.HasComment("Groupe"));

            entity.HasIndex(e => e.Groupeid, "groupe_pk").IsUnique();

            entity.HasIndex(e => e.Libelle, "unique_groupe").IsUnique();

            entity.Property(e => e.Groupeid)
                .ValueGeneratedNever()
                .HasComment("GroupeId")
                .HasColumnName("groupeid");
            entity.Property(e => e.Libelle)
                .HasMaxLength(254)
                .HasComment("Libelle")
                .HasColumnName("libelle");
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => e.Idoperation).HasName("pk_operation");

            entity.ToTable("operation", "budget", tb => tb.HasComment("Operation"));

            entity.HasIndex(e => e.Userid, "association_1_fk");

            entity.HasIndex(e => e.Idcategorie, "association_2_fk");

            entity.HasIndex(e => e.Idoperation, "operation_pk").IsUnique();

            entity.HasIndex(e => new { e.Idcategorie, e.Userid, e.Montant, e.Dateoperation, e.Isrevenu }, "unique_operation").IsUnique();

            entity.Property(e => e.Idoperation)
                .HasComment("Idoperation")
                .HasColumnName("idoperation");
            entity.Property(e => e.Dateoperation)
                .HasComment("Dateoperation")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("dateoperation");
            entity.Property(e => e.Idcategorie)
                .HasComment("Idcategorie")
                .HasColumnName("idcategorie");
            entity.Property(e => e.Isrevenu)
                .HasComment("Isrevenu")
                .HasColumnName("isrevenu");
            entity.Property(e => e.Montant)
                .HasComment("Montant")
                .HasColumnName("montant");
            entity.Property(e => e.Userid)
                .HasComment("Userid")
                .HasColumnName("userid");

            entity.HasOne(d => d.Category).WithMany(p => p.Operations)
                .HasForeignKey(d => d.Idcategorie)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_operatio_associati_categori");

            entity.HasOne(d => d.User).WithMany(p => p.Operations)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_operatio_associati_users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("pk_users");

            entity.ToTable("users", "budget", tb => tb.HasComment("Users"));

            entity.HasIndex(e => e.Groupeid, "association_4_fk");

            entity.HasIndex(e => e.Email, "unique_users").IsUnique();

            entity.HasIndex(e => e.Userid, "user_pk").IsUnique();

            entity.Property(e => e.Userid)
                .HasComment("Userid")
                .HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(254)
                .HasComment("Email")
                .HasColumnName("email");
            entity.Property(e => e.Groupeid)
                .HasComment("GroupeId")
                .HasColumnName("groupeid");
            entity.Property(e => e.Isactive)
                .HasComment("Isactive")
                .HasColumnName("isactive");
            entity.Property(e => e.Lastconnexion)
                .HasComment("LastConnexion")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastconnexion");
            entity.Property(e => e.Nom)
                .HasMaxLength(254)
                .HasComment("Nom")
                .HasColumnName("nom");
            entity.Property(e => e.Password)
                .HasMaxLength(254)
                .HasComment("Password")
                .HasColumnName("password");
            entity.Property(e => e.Prenom)
                .HasMaxLength(254)
                .HasComment("Prenom")
                .HasColumnName("prenom");

            entity.HasOne(d => d.Groupe).WithMany(p => p.Users)
                .HasForeignKey(d => d.Groupeid)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("fk_users_associati_groupe");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
