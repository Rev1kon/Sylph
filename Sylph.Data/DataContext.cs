using Microsoft.EntityFrameworkCore;
using Sylph.Data.Models;
using System;

namespace Sylph.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Site> Sites { get; set; }
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
            AddMockedData();
            base.SaveChanges();
        }
        private void AddMockedData() //TODO REMOVE LATER
        {
            Sites.AddRange(
                new Site()
                {
                    Url = "bt.rozetka.com",
                    Option = 1,
                    ContentPathOption = "(//div[@class='goods-tile__inner'])",
                    ImagePathOption = "(//a[contains(@class,'picture ng-star-inserted')])",
                    NamePathOption = "(//span[contains(@class,'goods-tile__title')])",
                    PricePathOption = "(//div[contains(@class,'goods-tile__price ng-star-inserted') or contains(@class,'goods-tile__price price--red ng-star-inserted')])",
                    NotAvailableFilter = "Немає в наявності"
                },
                new Site()
                {
                    Url = "hard.rozetka.com",
                    Option = 0,
                    ContentPathOption = "//div[contains(@class,'central-wrapper ng-star-inserted')]",
                    ImagePathOption = "(//img[contains(@class,'picture')])[2]",
                    NamePathOption = "//h1[contains(@class,'title')]",
                    PricePathOption = "(//p[contains(@class,'product-prices__big')])",
                    NotAvailableFilter = "Немає в наявності"
                },
                new Site()
                {
                    Url = "hard.rozetka.com",
                    Option = 1,
                    ContentPathOption = "(//div[@class='goods-tile__inner'])",
                    ImagePathOption = "(//a[contains(@class,'picture ng-star-inserted')])",
                    NamePathOption = "(//span[contains(@class,'goods-tile__title')])",
                    PricePathOption = "(//div[contains(@class,'goods-tile__price ng-star-inserted') or contains(@class,'goods-tile__price price--red ng-star-inserted')])",
                    NotAvailableFilter = "Немає в наявності"
                },
                new Site()
                {
                    Url = "rozetka.com",
                    Option = 1,
                    ContentPathOption = "(//div[@class='goods-tile__inner'])",
                    ImagePathOption = "(//a[contains(@class,'picture ng-star-inserted')])",
                    NamePathOption = "(//span[contains(@class,'goods-tile__title')])",
                    PricePathOption = "(//div[contains(@class,'goods-tile__price ng-star-inserted') or contains(@class,'goods-tile__price price--red ng-star-inserted')])",
                    NotAvailableFilter = "Немає в наявності"
                },
                new Site()
                {
                    Url = "www.foxtrot.com",
                    Option = 1,
                    ContentPathOption = "(//div[@data-has-variabilities='false'])",
                    ImagePathOption = "(//img[contains(@src,'Small.jpg')])",
                    NamePathOption = "(//a[contains(@class,'card__title')])",
                    PricePathOption = "(//div[@class='card-price'])",
                    NotAvailableFilter = "Немає в наявності"
                },
                new Site()
                {
                    Url = "epicentrk",
                    Option = 1,
                    ContentPathOption = "(//div[contains(@class,'card ')])",
                    ImagePathOption = "(//a[contains(@class,'photo')])",
                    NamePathOption = "(//div[contains(@class,'card__name')])",
                    PricePathOption = "(//span[@class='card__price-sum'])",
                    NotAvailableFilter = "Немає в наявності"
                }
                );
        }
    }
}
