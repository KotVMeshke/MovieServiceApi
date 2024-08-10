using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MovieServiceApi.DataBase.Entities;

namespace MovieServiceApi.DataBase.Context;

public partial class MovieServiceContext : DbContext
{
    public MovieServiceContext()
    {
    }

    public MovieServiceContext(DbContextOptions<MovieServiceContext> options)
        : base(options)
    {
    }



    public virtual DbSet<AgeRestriction> AgeRestrictions { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<CrewRole> CrewRoles { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Film> Films { get; set; }

    public virtual DbSet<FilmCrew> FilmCrews { get; set; }

    public virtual DbSet<FilmInfo> FilmInfos { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<History> Histories { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Photo> Photos { get; set; }

    public virtual DbSet<Poster> Posters { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Voiceover> Voiceovers { get; set; }

   

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgeRestriction>(entity =>
        {
            entity.HasKey(e => e.AgeId);

            entity.ToTable("age_restriction");

            entity.HasIndex(e => e.AgeValue, "UNQ_value").IsUnique();

            entity.Property(e => e.AgeId).HasColumnName("age_id");
            entity.Property(e => e.AgeValue)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("age_value");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.CtrId);

            entity.ToTable("country");

            entity.HasIndex(e => e.CtrName, "UNQ_name").IsUnique();

            entity.Property(e => e.CtrId).HasColumnName("ctr_id");
            entity.Property(e => e.CtrName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ctr_name");
        });

        modelBuilder.Entity<CrewRole>(entity =>
        {
            entity.HasKey(e => e.CrId);

            entity.ToTable("crew_role");

            entity.Property(e => e.CrId)
                .ValueGeneratedNever()
                .HasColumnName("cr_id");
            entity.Property(e => e.CrName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cr_name");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => new { e.FbkUser, e.FbkFilm });

            entity.ToTable("feedback");

            entity.HasIndex(e => e.FbkFilm, "IXFK_feedback_film");

            entity.HasIndex(e => e.FbkUser, "IXFK_feedback_user");

            entity.HasIndex(e => e.FbkMark, "IX_mark");

            entity.Property(e => e.FbkUser).HasColumnName("fbk_user");
            entity.Property(e => e.FbkFilm).HasColumnName("fbk_film");
            entity.Property(e => e.FbkMark).HasColumnName("fbk_mark");
            entity.Property(e => e.FbkText)
                .HasColumnType("text")
                .HasColumnName("fbk_text");

            entity.HasOne(d => d.FbkFilmNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.FbkFilm)
                .HasConstraintName("FK_feedback_film");

            entity.HasOne(d => d.FbkUserNavigation).WithMany(p => p.Feedbacks)
                .HasForeignKey(d => d.FbkUser)
                .HasConstraintName("FK_feedback_user");
        });

        modelBuilder.Entity<Film>(entity =>
        {
            entity.HasKey(e => e.FlmId);

            entity.ToTable("film");

            entity.HasIndex(e => e.FlmAgeRestriction, "IXFK_film_age_restriction");

            entity.HasIndex(e => e.FlmCountry, "IXFK_film_country");

            entity.HasIndex(e => e.FlmMedia, "IXFK_film_media");

            entity.HasIndex(e => e.FlmName, "IX_name");

            entity.HasIndex(e => new { e.FlmName, e.FlmReleaseDate }, "UNQ_name_date").IsUnique();

            entity.Property(e => e.FlmId).HasColumnName("flm_id");
            entity.Property(e => e.FlmAgeRestriction).HasColumnName("flm_age_restriction");
            entity.Property(e => e.FlmCountry).HasColumnName("flm_country");
            entity.Property(e => e.FlmDescription)
                .HasColumnType("text")
                .HasColumnName("flm_description");
            entity.Property(e => e.FlmMedia).HasColumnName("flm_media");
            entity.Property(e => e.FlmName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("flm_name");
            entity.Property(e => e.FlmReleaseDate).HasColumnName("flm_release_date");

            entity.HasOne(d => d.FlmAgeRestrictionNavigation).WithMany(p => p.Films)
                .HasForeignKey(d => d.FlmAgeRestriction)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_film_age_restriction");

            entity.HasOne(d => d.FlmCountryNavigation).WithMany(p => p.Films)
                .HasForeignKey(d => d.FlmCountry)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_film_country");

            entity.HasOne(d => d.FlmMediaNavigation).WithMany(p => p.Films)
                .HasForeignKey(d => d.FlmMedia)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_film_media");

            entity.HasMany(d => d.FgGenres).WithMany(p => p.FgFilms)
                .UsingEntity<Dictionary<string, object>>(
                    "FilmGenre",
                    r => r.HasOne<Genre>().WithMany()
                        .HasForeignKey("FgGenre")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_film_genre_genre"),
                    l => l.HasOne<Film>().WithMany()
                        .HasForeignKey("FgFilm")
                        .HasConstraintName("FK_film_genre_film"),
                    j =>
                    {
                        j.HasKey("FgFilm", "FgGenre");
                        j.ToTable("film_genre");
                        j.HasIndex(new[] { "FgFilm" }, "IXFK_film_genre_film");
                        j.HasIndex(new[] { "FgGenre" }, "IXFK_film_genre_genre");
                        j.IndexerProperty<int>("FgFilm").HasColumnName("fg_film");
                        j.IndexerProperty<int>("FgGenre").HasColumnName("fg_genre");
                    });
        });

        modelBuilder.Entity<FilmCrew>(entity =>
        {
            entity.HasKey(e => new { e.FcrId, e.FcrPerson });

            entity.ToTable("film_crew");

            entity.HasIndex(e => e.FcrRole, "IXFK_film_crew_crew_role");

            entity.HasIndex(e => e.FcrFilm, "IXFK_film_crew_film");

            entity.HasIndex(e => e.FcrPerson, "IXFK_film_crew_person");

            entity.Property(e => e.FcrId)
                .ValueGeneratedOnAdd()
                .HasColumnName("fcr_id");
            entity.Property(e => e.FcrPerson).HasColumnName("fcr_person");
            entity.Property(e => e.FcrFilm).HasColumnName("fcr_film");
            entity.Property(e => e.FcrRole).HasColumnName("fcr_role");

            entity.HasOne(d => d.FcrFilmNavigation).WithMany(p => p.FilmCrews)
                .HasForeignKey(d => d.FcrFilm)
                .HasConstraintName("FK_film_crew_film");

            entity.HasOne(d => d.FcrPersonNavigation).WithMany(p => p.FilmCrews)
                .HasForeignKey(d => d.FcrPerson)
                .HasConstraintName("FK_film_crew_person");

            entity.HasOne(d => d.FcrRoleNavigation).WithMany(p => p.FilmCrews)
                .HasForeignKey(d => d.FcrRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_film_crew_crew_role");
        });

        modelBuilder.Entity<FilmInfo>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("film_info");

            entity.Property(e => e.Age)
                .HasMaxLength(5)
                .IsUnicode(false)
                .HasColumnName("age");
            entity.Property(e => e.CountryName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("country_name");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.FilmPath)
                .HasColumnType("text")
                .HasColumnName("film_path");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.PosterFilepath)
                .HasColumnType("text")
                .HasColumnName("poster_filepath");
            entity.Property(e => e.ReleaseDate).HasColumnName("release_date");
            entity.Property(e => e.TrailerPath)
                .HasColumnType("text")
                .HasColumnName("trailer_path");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GnrId);

            entity.ToTable("genre");

            entity.HasIndex(e => e.GnrName, "UNQ_genre").IsUnique();

            entity.Property(e => e.GnrId).HasColumnName("gnr_id");
            entity.Property(e => e.GnrName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("gnr_name");
        });

        modelBuilder.Entity<History>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("history");

            entity.HasIndex(e => e.HFilm, "IXFK_history_film");

            entity.HasIndex(e => e.HUser, "IXFK_history_user");

            entity.Property(e => e.HData)
                .HasColumnType("datetime")
                .HasColumnName("h_data");
            entity.Property(e => e.HFilm).HasColumnName("h_film");
            entity.Property(e => e.HUser).HasColumnName("h_user");

            entity.HasOne(d => d.HFilmNavigation).WithMany()
                .HasForeignKey(d => d.HFilm)
                .HasConstraintName("FK_history_film");

            entity.HasOne(d => d.HUserNavigation).WithMany()
                .HasForeignKey(d => d.HUser)
                .HasConstraintName("FK_history_user");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.HasKey(e => e.MedId);

            entity.ToTable("media");

            entity.Property(e => e.MedId).HasColumnName("med_id");
            entity.Property(e => e.MedFilmPath)
                .HasColumnType("text")
                .HasColumnName("med_film_path");
            entity.Property(e => e.MedTrailerPath)
                .HasColumnType("text")
                .HasColumnName("med_trailer_path");

            entity.HasMany(d => d.MpPosters).WithMany(p => p.MpMedia)
                .UsingEntity<Dictionary<string, object>>(
                    "MediaPoster",
                    r => r.HasOne<Poster>().WithMany()
                        .HasForeignKey("MpPoster")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_media_poster_poster"),
                    l => l.HasOne<Medium>().WithMany()
                        .HasForeignKey("MpMedia")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_media_poster_media"),
                    j =>
                    {
                        j.HasKey("MpMedia", "MpPoster");
                        j.ToTable("media_poster");
                        j.IndexerProperty<int>("MpMedia").HasColumnName("mp_media");
                        j.IndexerProperty<int>("MpPoster").HasColumnName("mp_poster");
                    });
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PerId);

            entity.ToTable("person");

            entity.HasIndex(e => new { e.PerName, e.PerSurname }, "IX_name");

            entity.Property(e => e.PerId).HasColumnName("per_id");
            entity.Property(e => e.PerAge).HasColumnName("per_age");
            entity.Property(e => e.PerBirthDate).HasColumnName("per_birth_date");
            entity.Property(e => e.PerName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("per_name");
            entity.Property(e => e.PerPatronymic)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("per_patronymic");
            entity.Property(e => e.PerSurname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("per_surname");
        });

        modelBuilder.Entity<Photo>(entity =>
        {
            entity.HasKey(e => e.PhId);

            entity.ToTable("photo");

            entity.HasIndex(e => e.PhPerson, "IXFK_photo_person");

            entity.Property(e => e.PhId).HasColumnName("ph_id");
            entity.Property(e => e.PhPath)
                .HasColumnType("text")
                .HasColumnName("ph_path");
            entity.Property(e => e.PhPerson).HasColumnName("ph_person");

            entity.HasOne(d => d.PhPersonNavigation).WithMany(p => p.Photos)
                .HasForeignKey(d => d.PhPerson)
                .HasConstraintName("FK_photo_person");
        });

        modelBuilder.Entity<Poster>(entity =>
        {
            entity.HasKey(e => e.PstId);

            entity.ToTable("poster");

            entity.HasIndex(e => e.PstId, "IX_poster").IsUnique();

            entity.Property(e => e.PstId)
                .ValueGeneratedNever()
                .HasColumnName("pst_id");
            entity.Property(e => e.PstFilepath)
                .HasColumnType("text")
                .HasColumnName("pst_filepath");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UsrId);

            entity.ToTable("user", tb =>
                {
                    tb.HasTrigger("delete_feedback_on_ban_trigger");
                    tb.HasTrigger("preserve_delete_trigger");
                });

            entity.HasIndex(e => e.UserBannedBy, "IXFK_user_user");

            entity.HasIndex(e => e.UserRole, "IXFK_user_user_role");

            entity.HasIndex(e => e.UsrEmail, "IX_email");

            entity.HasIndex(e => e.UsrName, "IX_name");

            entity.HasIndex(e => e.UserRole, "IX_role");

            entity.HasIndex(e => e.UsrEmail, "UNQ_email").IsUnique();

            entity.Property(e => e.UsrId).HasColumnName("usr_id");
            entity.Property(e => e.UserBannedBy).HasColumnName("user_banned_by");
            entity.Property(e => e.UserRole).HasColumnName("user_role");
            entity.Property(e => e.UsrEmail)
                .HasMaxLength(254)
                .IsUnicode(false)
                .HasColumnName("usr_email");
            entity.Property(e => e.UsrName)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("usr_name");
            entity.Property(e => e.UsrPassword)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("usr_password");

            entity.HasOne(d => d.UserBannedByNavigation).WithMany(p => p.InverseUserBannedByNavigation)
                .HasForeignKey(d => d.UserBannedBy)
                .HasConstraintName("FK_user_user");

            entity.HasOne(d => d.UserRoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserRole)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_user_user_role");

            entity.HasMany(d => d.LibFilms).WithMany(p => p.LibUsers)
                .UsingEntity<Dictionary<string, object>>(
                    "Library",
                    r => r.HasOne<Film>().WithMany()
                        .HasForeignKey("LibFilm")
                        .HasConstraintName("FK_library_film"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("LibUser")
                        .HasConstraintName("FK_library_user"),
                    j =>
                    {
                        j.HasKey("LibUser", "LibFilm");
                        j.ToTable("library");
                        j.HasIndex(new[] { "LibFilm" }, "IXFK_library_film");
                        j.HasIndex(new[] { "LibUser" }, "IXFK_library_user");
                        j.IndexerProperty<int>("LibUser").HasColumnName("lib_user");
                        j.IndexerProperty<int>("LibFilm").HasColumnName("lib_film");
                    });
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => e.UrId);

            entity.ToTable("user_role");

            entity.Property(e => e.UrId).HasColumnName("ur_id");
            entity.Property(e => e.UrName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ur_name");
        });

        modelBuilder.Entity<Voiceover>(entity =>
        {
            entity.HasKey(e => e.VoiId);

            entity.ToTable("voiceover");

            entity.HasIndex(e => e.VoiFilm, "IXFK_voiceover_film");

            entity.Property(e => e.VoiId).HasColumnName("voi_id");
            entity.Property(e => e.VoiFilepath)
                .HasColumnType("text")
                .HasColumnName("voi_filepath");
            entity.Property(e => e.VoiFilm).HasColumnName("voi_film");

            entity.HasOne(d => d.VoiFilmNavigation).WithMany(p => p.Voiceovers)
                .HasForeignKey(d => d.VoiFilm)
                .HasConstraintName("FK_voiceover_film");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
