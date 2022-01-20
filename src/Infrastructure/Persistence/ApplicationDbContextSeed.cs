using ProductsArchive.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using ProductsArchive.Domain.Entities.ProductsSection;
using ProductsArchive.Domain.Common.Localization;

namespace ProductsArchive.Infrastructure.Persistence;

public static class ApplicationDbContextSeed
{
    public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        // admin
        var administratorRole = new IdentityRole("Administrator");
        if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await roleManager.CreateAsync(administratorRole);
        }

        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
        if (userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await userManager.CreateAsync(administrator, "Administrator1!");
            await userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }

        // Common User
        var commonUserRole = new IdentityRole("CommonUser");
        if (roleManager.Roles.All(r => r.Name != commonUserRole.Name))
        {
            await roleManager.CreateAsync(commonUserRole);
        }

        var user = new ApplicationUser { UserName = "user@localhost", Email = "user@localhost" };
        if (userManager.Users.All(u => u.UserName != user.UserName))
        {
            await userManager.CreateAsync(user, "CommonUser1!");
            await userManager.AddToRolesAsync(user, new[] { commonUserRole.Name });
        }
    }

    public static async Task SeedSampleDataAsync(ApplicationDbContext context)
    {
        // Seed, if necessary
        if (!context.ProductCategories.Any())
        {
            // categories
            var category_1 = new ProductCategory
            {
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Ortaggi"),
                    ("en", "Vegetables"),
                    ("de", "Gemüse")
                })
            };

            var category_2 = new ProductCategory
            {
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Funghi"),
                    ("en", "Mushrooms"),
                    ("de", "Pilze")
                })
            };

            context.ProductCategories.AddRange(category_1, category_2);
            await context.SaveChangesAsync();

            // sizes
            var size_1 = new ProductSize
            {
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Vaso 1700ml"),
                    ("en", "1700ml Glass Jar"),
                    ("de", "Glas 1700ml")
                })
            };

            var size_2 = new ProductSize
            {
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Sc. 3/1"),
                    ("en", "3/1 Can"),
                    ("de", "Dose 3/1")
                })
            };

            var size_3 = new ProductSize
            {
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Busta 1700"),
                    ("en", "Pouch 1700"),
                    ("de", "Beutel 1700")
                })
            };

            context.ProductSizes.AddRange(size_1, size_2, size_3);
            await context.SaveChangesAsync();

            // product groups
            var group_1 = new ProductGroup
            {
                CategoryId = category_1.Id,
                GroupId = "ARWTQ",
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Carciofi"),
                    ("en", "Artichokes"),
                    ("de", "Artischocken")
                }),
                Description = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Carciofi per la ristorazione."),
                    ("en", "Artichokes for catering."),
                    ("de", "Artischocken für die Gastronomie.")
                }),
                Products = new List<Product>
                {
                    new Product
                    {
                        ProductId = "05683",
                        NetWeight = "1600g",
                        SizeId = size_1.Id
                    },
                    new Product
                    {
                        ProductId = "20356",
                        NetWeight = "2500g",
                        SizeId = size_2.Id
                    },
                    new Product
                    {
                        ProductId = "03266",
                        NetWeight = "1650g",
                        SizeId = size_3.Id
                    }
                }
            };

            var group_2 = new ProductGroup
            {
                CategoryId = category_1.Id,
                GroupId = "DBASL",
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Pomodori"),
                    ("en", "Tomatoes"),
                    ("de", "Tomaten")
                }),
                Description = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Pomodori per la ristorazione."),
                    ("en", "Tomatoes for catering."),
                    ("de", "Tomaten für die Gastronomie.")
                }),
                Products = new List<Product>
                {
                    new Product
                    {
                        ProductId = "92892",
                        NetWeight = "1600g",
                        SizeId = size_1.Id
                    },
                    new Product
                    {
                        ProductId = "84566",
                        NetWeight = "2500g",
                        SizeId = size_2.Id
                    }
                }
            };

            var group_3 = new ProductGroup
            {
                CategoryId = category_2.Id,
                GroupId = "GMLTS",
                Name = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Porcini"),
                    ("en", "Porcini Mushrooms"),
                    ("de", "Steinpilze")
                }),
                Description = LocalizedString.Create(new List<(string, string?)>
                {
                    ("it", "Funghi per la ristorazione."),
                    ("en", "Mushrooms for catering."),
                    ("de", "Pilze für die Gastronomie.")
                }),
                Products = new List<Product>
                {
                    new Product
                    {
                        ProductId = "96325",
                        NetWeight = "1600g",
                        SizeId = size_1.Id
                    },
                    new Product
                    {
                        ProductId = "00014",
                        NetWeight = "2500g",
                        SizeId = size_2.Id
                    },
                    new Product
                    {
                        ProductId = "39501",
                        NetWeight = "1650g",
                        SizeId = size_3.Id
                    }
                }
            };

            context.ProductGroups.AddRange(group_1, group_2, group_3);
            await context.SaveChangesAsync();
        }
    }
}
