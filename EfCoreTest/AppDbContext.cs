using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Options;

namespace EfCoreTest;


public class PublishingHouse
{
    public int ID { get; set; }

    public string Title { get; set; }

    public string NumberPhone { get; set; }

    public string Email { get; set; }

    public string Address { get; set; }

    public string Description { get; set; }
}

public class AppDbContext : DbContext
{
    public DbSet<PublishingHouse> PublishingHouses { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        Configure(modelBuilder.Entity<PublishingHouse>());
    }

    public void Configure(EntityTypeBuilder<PublishingHouse> builder)
    {
        builder.ToTable("PublishingHouses", "dbo");

        builder.HasKey(c => c.ID).HasName("PK_PublishingHouses");

        builder.Property(c => c.ID).HasColumnName("ID")
            .ValueGeneratedOnAdd();

        builder.Property(dto => dto.Title)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("Title");

        builder.Property(dto => dto.Email)
                .HasMaxLength(100)
                .HasColumnName("Email");

        builder.Property(dto => dto.Address)
                .HasMaxLength(1000)
                .HasColumnName("Address");

        builder.Property(dto => dto.Description)
                .HasMaxLength(1000)
                .HasColumnName("Description");

        builder.Property(dto => dto.NumberPhone)
                .HasMaxLength(15)
                .HasColumnName("NumberPhone");

        builder.HasData(
            new PublishingHouse
            {
                ID = 1,
                Title = "Красная цена ",
                Email = "redPrice@gmail.com",
                NumberPhone = "8937-216-76-11",
                Address = "ООО 'Красная Цена' Самара",
                Description = "Название Красная Цена выбрано не случайно!"
            },
            //db.Makers.Add(new Maker("Красная Цена", "8937-216-76-11", "",
            //    "ООО 'Красная Цена' Самара", "Название Красная Цена выбрано не случайно!"));
            new PublishingHouse
            {
                ID = 2,
                Title = "АЛМА",
                Email = "alma@mail.ru",
                NumberPhone = "8347-827-36-96",
                Address = "ул.Комарова, д.41;",
                Description = "У нас вы найдете экологически чистые продукты по приятным ценам"
            },
            //db.Makers.Add(new Maker("АЛМА", "8347-827-36-96", "",
            //    "ул.Комарова, д.41;", "У нас вы найдете экологически чистые продукты по приятным ценам"));
            new PublishingHouse
            {
                ID = 3,
                Title = "Мясницкий ряд",
                Email = "zakupki@kolbasa.ru",
                NumberPhone = "+7495-411-33-41",
                Address = "Транспортный проезд д.7 г. Одинцово Московская область",
                Description = "Компания «Мясницкий ряд» основана в 2004 году на базе Первого Одинцовского мясокомбината. " +
                              "Наша компания активно растёт и развивается, " +
                              "регулярно расширяя ассортимент и повышая качество выпускаемой продукции."
            },
            //db.Makers.Add(new Maker("Мясницкий ряд", " +7495-411-33-41", " zakupki@kolbasa.ru",
            //    "Транспортный проезд д.7 г. Одинцово Московская область", ""));
            new PublishingHouse
            {
                ID = 4,
                Title = "Черкизово",
                Email = "sk@cherkizovo.com",
                NumberPhone = "+7495-660-24-40",
                Address = "Москва 125047 Лесная улица 5Б, бизнес - центр «Белая площадь», 12 - й этаж",
                Description = "Мясное производство для нас не просто бизнес. " +
                                "Делать лучшие в стране продукты питания — наша страсть и призвание."

            });
        //db.Makers.Add(new Maker("Черкизово", "+7495-660-24-40", " sk@cherkizovo.com",
        //    "Москва 125047 Лесная улица 5Б, бизнес - центр «Белая площадь», 12 - й этаж", ""));
    }
}
