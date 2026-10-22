using Forum_Project1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Forum_Project1.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await context.Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            //Dataseedningen har jag tagit av AI, för det skulle ta jättemycket tid annars.

            // 1. CREATE ROLES
            string[] roleNames = { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. CREATE USER (Admin & User)
            var adminEmail = "admin@forum.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    Name = "Admin",
                    ProfileImageUrl = "https://images.unsplash.com/photo-1603112579965-e24332cc453a",
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            var userEmail = "user@forum.com";
            var normalUser = await userManager.FindByEmailAsync(userEmail);

            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    EmailConfirmed = true,
                    Name = "Sven Svensson",
                    ProfileImageUrl = "https://images.unsplash.com/photo-1535713875002-d1d0cf377fde",
                    CreatedAt = DateTime.UtcNow
                };

                var normalUserResult = await userManager.CreateAsync(normalUser, "User123!");
                if (normalUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            // 3. CREATE CATEGORIES OCH SUBCATEGORIES
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "General", Description = "Discuss anything here",
                    SubCategories = new List<SubCategory> { new SubCategory { Name = "General Discussion" }, new SubCategory { Name = "Introductions" }, new SubCategory { Name = "Off-Topic" } } },

                    new Category { Name = "Technology", Description = "Questions and discussion about tech",
                    SubCategories = new List<SubCategory> { new SubCategory { Name = "Hardware" }, new SubCategory { Name = "Software" }, new SubCategory { Name = "Programming & AI" }, new SubCategory { Name = "Mobile & Accessories" } } },

                    new Category { Name = "Sports", Description = "All about sports and fitness",
                    SubCategories = new List<SubCategory> { new SubCategory { Name = "Football" }, new SubCategory { Name = "Fitness" }, new SubCategory { Name = "Ice Hockey" }, new SubCategory { Name = "Combat Sports" } } },

                    new Category { Name = "Gaming & Support", Description = "Video games, movies, TV shows, and entertainment",
                    SubCategories = new List<SubCategory> { new SubCategory { Name = "PC & Console Gaming" }, new SubCategory { Name = "Esports" }, new SubCategory { Name = "Movies & TV" }, new SubCategory { Name = "Music & Podcasts" } } },

                    new Category { Name = "Marketplace", Description = "Buy, sell, and trade with other members",
                    SubCategories = new List<SubCategory> { new SubCategory { Name = "Electronics" }, new SubCategory { Name = "Clothing & Style" }, new SubCategory { Name = "Vehicles & Parts" }, new SubCategory { Name = "Buy & Sell" } } },

                    new Category { Name = "Feedback & Support", Description = "Ideas, feedback, and site questions",
                    SubCategories = new List<SubCategory> { new SubCategory { Name = "Suggestions" }, new SubCategory { Name = "Bug Reports" }, new SubCategory { Name = "Site Rules & FAQ" } } }
                };

                await context.Categories.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            // 4. CREATE TOPICS (BOTH USER AND SUBCATEGORIES)
            if (!await context.Topics.AnyAsync())
            {
                var subCategories = await context.SubCategories.ToListAsync();
                var admin = await userManager.FindByEmailAsync("admin@forum.com");
                var user = await userManager.FindByEmailAsync("user@forum.com");

                var userId = user?.Id ?? admin?.Id;

                if (subCategories.Any() && admin != null && userId != null)
                {
                    var sampleTopics = new List<Topic>();

                    var generalDisc = subCategories.FirstOrDefault(s => s.Name.Equals("General Discussion", StringComparison.OrdinalIgnoreCase));
                    var introSub = subCategories.FirstOrDefault(s => s.Name.Equals("Introductions", StringComparison.OrdinalIgnoreCase));
                    var offTopicSub = subCategories.FirstOrDefault(s => s.Name.Equals("Off-Topic", StringComparison.OrdinalIgnoreCase));

                    if (generalDisc != null && introSub != null && offTopicSub != null)
                    {
                        sampleTopics.AddRange(new List<Topic>
                        {
                            // --- General Discussion ---
                            new Topic { Title = "What is your best tip for getting better sleep on weekdays?", CreatedAt = DateTime.UtcNow.AddDays(-10), SubCategoryId = generalDisc.Id, UserId = userId },
                            new Topic { Title = "How much have your grocery bills increased recently?", CreatedAt = DateTime.UtcNow.AddDays(-6), SubCategoryId = generalDisc.Id, UserId = admin.Id },

                            // --- Introductions ---
                            new Topic { Title = "New to the forum! Hey everyone 👋", CreatedAt = DateTime.UtcNow.AddDays(-15), SubCategoryId = introSub.Id, UserId = userId },
                            new Topic { Title = "Hello from Gothenburg – software dev and coffee enthusiast", CreatedAt = DateTime.UtcNow.AddDays(-8), SubCategoryId = introSub.Id, UserId = admin.Id },

                            // --- Off-Topic ---
                            new Topic { Title = "What music are you listening to right now?", CreatedAt = DateTime.UtcNow.AddDays(-12), SubCategoryId = offTopicSub.Id, UserId = userId },
                            new Topic { Title = "Show your desk setup / workspace (Photos welcome!)", CreatedAt = DateTime.UtcNow.AddDays(-7), SubCategoryId = offTopicSub.Id, UserId = admin.Id },
                        });
                    }

                    var hardwareSub = subCategories.FirstOrDefault(s => s.Name.Equals("Hardware", StringComparison.OrdinalIgnoreCase));
                    var softwareSub = subCategories.FirstOrDefault(s => s.Name.Equals("Software", StringComparison.OrdinalIgnoreCase));
                    var progAiSub = subCategories.FirstOrDefault(s => s.Name.Equals("Programming & AI", StringComparison.OrdinalIgnoreCase));
                    var mobileSub = subCategories.FirstOrDefault(s => s.Name.Equals("Mobile & Accessories", StringComparison.OrdinalIgnoreCase));

                    if (hardwareSub != null && softwareSub != null && progAiSub != null && mobileSub != null)
                    {
                        sampleTopics.AddRange(new List<Topic>
                        {
                            // --- Hardware ---
                            new Topic { Title = "Is 16GB RAM still enough for modern PC gaming in 2026?", CreatedAt = DateTime.UtcNow.AddDays(-11), SubCategoryId = hardwareSub.Id, UserId = userId },
                            new Topic { Title = "Building my first custom PC: Need advice on GPU vs CPU budget split", CreatedAt = DateTime.UtcNow.AddDays(-7), SubCategoryId = hardwareSub.Id, UserId = admin.Id },

                            // --- Software ---
                            new Topic { Title = "What are your must-have desktop apps on a brand new Windows/Mac install?", CreatedAt = DateTime.UtcNow.AddDays(-14), SubCategoryId = softwareSub.Id, UserId = userId },
                            new Topic { Title = "Open-source alternatives to subscription software (Photoshop, Office, etc.)", CreatedAt = DateTime.UtcNow.AddDays(-9), SubCategoryId = softwareSub.Id, UserId = admin.Id },

                            // --- Programming & AI ---
                            new Topic { Title = "How are you using AI coding assistants in your daily workflow?", CreatedAt = DateTime.UtcNow.AddDays(-13), SubCategoryId = progAiSub.Id, UserId = userId },
                            new Topic { Title = "C# / .NET vs Node.js for modern web backend development in 2026", CreatedAt = DateTime.UtcNow.AddDays(-8), SubCategoryId = progAiSub.Id, UserId = admin.Id },

                            // --- Mobile & Accessories ---
                            new Topic { Title = "Battery health preservation: Do you limit charge to 80% or just charge to 100%?", CreatedAt = DateTime.UtcNow.AddDays(-10), SubCategoryId = mobileSub.Id, UserId = userId },
                            new Topic { Title = "Are wireless earbuds getting better or just more expensive?", CreatedAt = DateTime.UtcNow.AddDays(-6), SubCategoryId = mobileSub.Id, UserId = admin.Id },
                        });
                    }

                    // --- Sports ---
                    var footballSub = subCategories.FirstOrDefault(s => s.Name.Equals("Football", StringComparison.OrdinalIgnoreCase));
                    var fitnessSub = subCategories.FirstOrDefault(s => s.Name.Equals("Fitness", StringComparison.OrdinalIgnoreCase));
                    var hockeySub = subCategories.FirstOrDefault(s => s.Name.Equals("Ice Hockey", StringComparison.OrdinalIgnoreCase));
                    var combatSub = subCategories.FirstOrDefault(s => s.Name.Equals("Combat Sports", StringComparison.OrdinalIgnoreCase));

                    if (footballSub != null && fitnessSub != null && hockeySub != null && combatSub != null)
                    {
                        sampleTopics.AddRange(new List<Topic>
                        {
                            // Football
                            new Topic { Title = "Who is winning the Champions League this season?", CreatedAt = DateTime.UtcNow.AddDays(-8), SubCategoryId = footballSub.Id, UserId = userId },
                            new Topic { Title = "Best match you've ever watched live in a stadium?", CreatedAt = DateTime.UtcNow.AddDays(-2), SubCategoryId = footballSub.Id, UserId = admin.Id },

                            // Fitness
                            new Topic { Title = "How do you stay motivated during cold winter workouts?", CreatedAt = DateTime.UtcNow.AddDays(-9), SubCategoryId = fitnessSub.Id, UserId = userId },
                            new Topic { Title = "Strength training vs Cardio: What is your main focus?", CreatedAt = DateTime.UtcNow.AddDays(-3), SubCategoryId = fitnessSub.Id, UserId = admin.Id },

                            // Ice Hockey
                            new Topic { Title = "NHL mid-season predictions and playoff contenders", CreatedAt = DateTime.UtcNow.AddDays(-10), SubCategoryId = hockeySub.Id, UserId = userId },
                            new Topic { Title = "What gear upgrade made the biggest difference in your game?", CreatedAt = DateTime.UtcNow.AddDays(-4), SubCategoryId = hockeySub.Id, UserId = admin.Id },

                            // Combat Sports
                            new Topic { Title = "UFC main event fight predictions for this weekend", CreatedAt = DateTime.UtcNow.AddDays(-6), SubCategoryId = combatSub.Id, UserId = userId },
                            new Topic { Title = "Tips for beginners starting Boxing or BJJ?", CreatedAt = DateTime.UtcNow.AddDays(-1), SubCategoryId = combatSub.Id, UserId = admin.Id }
                        });
                    }

                    // --- Gaming & Support ---
                    var pcConsoleSub = subCategories.FirstOrDefault(s => s.Name.Equals("PC & Console Gaming", StringComparison.OrdinalIgnoreCase));
                    var esportsSub = subCategories.FirstOrDefault(s => s.Name.Equals("Esports", StringComparison.OrdinalIgnoreCase));
                    var moviesSub = subCategories.FirstOrDefault(s => s.Name.Equals("Movies & TV", StringComparison.OrdinalIgnoreCase));
                    var musicSub = subCategories.FirstOrDefault(s => s.Name.Equals("Music & Podcasts", StringComparison.OrdinalIgnoreCase));

                    if (pcConsoleSub != null && esportsSub != null && moviesSub != null && musicSub != null)
                    {
                        sampleTopics.AddRange(new List<Topic>
                        {
                            // PC & Console Gaming
                            new Topic { Title = "What game are you currently sinking all your free time into?", CreatedAt = DateTime.UtcNow.AddDays(-12), SubCategoryId = pcConsoleSub.Id, UserId = userId },
                            new Topic { Title = "Steam Deck / Handheld PCs vs Traditional Consoles", CreatedAt = DateTime.UtcNow.AddDays(-3), SubCategoryId = pcConsoleSub.Id, UserId = admin.Id },

                            // Esports
                            new Topic { Title = "Is Counter-Strike 2 in a good state right now for competitive play?", CreatedAt = DateTime.UtcNow.AddDays(-9), SubCategoryId = esportsSub.Id, UserId = userId },
                            new Topic { Title = "Which team is looking strongest heading into the World Championship?", CreatedAt = DateTime.UtcNow.AddDays(-4), SubCategoryId = esportsSub.Id, UserId = admin.Id },

                            // Movies & TV
                            new Topic { Title = "Best TV series you've binge-watched recently?", CreatedAt = DateTime.UtcNow.AddDays(-7), SubCategoryId = moviesSub.Id, UserId = userId },
                            new Topic { Title = "Unpopular opinion: What highly-rated movie didn't live up to the hype?", CreatedAt = DateTime.UtcNow.AddDays(-2), SubCategoryId = moviesSub.Id, UserId = admin.Id },

                            // Music & Podcasts
                            new Topic { Title = "What podcasts do you listen to during work or commutes?", CreatedAt = DateTime.UtcNow.AddDays(-6), SubCategoryId = musicSub.Id, UserId = userId },
                            new Topic { Title = "Share your favorite album release of the year so far", CreatedAt = DateTime.UtcNow.AddDays(-1), SubCategoryId = musicSub.Id, UserId = admin.Id }
                        });
                    }

                    // --- Marketplace ---
                    var electronicsSub = subCategories.FirstOrDefault(s => s.Name.Equals("Electronics", StringComparison.OrdinalIgnoreCase));
                    var clothingSub = subCategories.FirstOrDefault(s => s.Name.Equals("Clothing & Style", StringComparison.OrdinalIgnoreCase));
                    var vehiclesSub = subCategories.FirstOrDefault(s => s.Name.Equals("Vehicles & Parts", StringComparison.OrdinalIgnoreCase));
                    var buySellSub = subCategories.FirstOrDefault(s => s.Name.Equals("Buy & Sell", StringComparison.OrdinalIgnoreCase));

                    if (electronicsSub != null && clothingSub != null && vehiclesSub != null && buySellSub != null)
                    {
                        sampleTopics.AddRange(new List<Topic>
                        {
                            // Electronics
                            new Topic { Title = "[FS] iPad Air 256GB Wi-Fi - Great condition with original box", CreatedAt = DateTime.UtcNow.AddDays(-8), SubCategoryId = electronicsSub.Id, UserId = userId },
                            new Topic { Title = "[WTB] Looking for a decent 27-inch 144Hz 1440p Monitor", CreatedAt = DateTime.UtcNow.AddDays(-2), SubCategoryId = electronicsSub.Id, UserId = admin.Id },

                            // Clothing & Style
                            new Topic { Title = "[FS] Winter Parka Jacket - Size L (Barely used)", CreatedAt = DateTime.UtcNow.AddDays(-11), SubCategoryId = clothingSub.Id, UserId = userId },
                            new Topic { Title = "Where do you find good quality basics that don't shrink?", CreatedAt = DateTime.UtcNow.AddDays(-4), SubCategoryId = clothingSub.Id, UserId = admin.Id },

                            // Vehicles & Parts
                            new Topic { Title = "[FS] Set of 18-inch Alloy Wheels with Winter Tires", CreatedAt = DateTime.UtcNow.AddDays(-9), SubCategoryId = vehiclesSub.Id, UserId = userId },
                            new Topic { Title = "Electric Car ownership: Is it worth making the switch now?", CreatedAt = DateTime.UtcNow.AddDays(-3), SubCategoryId = vehiclesSub.Id, UserId = admin.Id },

                            // Buy & Sell
                            new Topic { Title = "General safety tips when meeting buyers in person", CreatedAt = DateTime.UtcNow.AddDays(-14), SubCategoryId = buySellSub.Id, UserId = userId },
                            new Topic { Title = "[FS] Standing Desk Frame (Dual Motor) - Local pickup only", CreatedAt = DateTime.UtcNow.AddDays(-1), SubCategoryId = buySellSub.Id, UserId = admin.Id }
                        });
                    }

                    // --- Feedback & Support ---
                    var suggestionsSub = subCategories.FirstOrDefault(s => s.Name.Equals("Suggestions", StringComparison.OrdinalIgnoreCase));
                    var bugReportsSub = subCategories.FirstOrDefault(s => s.Name.Equals("Bug Reports", StringComparison.OrdinalIgnoreCase));
                    var rulesFaqSub = subCategories.FirstOrDefault(s => s.Name.Equals("Site Rules & FAQ", StringComparison.OrdinalIgnoreCase));

                    if (suggestionsSub != null && bugReportsSub != null && rulesFaqSub != null)
                    {
                        sampleTopics.AddRange(new List<Topic>
                        {
                            // Suggestions
                            new Topic { Title = "Feature Request: Add Dark Mode toggle to the navigation bar", CreatedAt = DateTime.UtcNow.AddDays(-15), SubCategoryId = suggestionsSub.Id, UserId = userId },
                            new Topic { Title = "Can we get notification badges for thread replies?", CreatedAt = DateTime.UtcNow.AddDays(-5), SubCategoryId = suggestionsSub.Id, UserId = admin.Id },

                            // Bug Reports
                            new Topic { Title = "Mobile view bug: Profile picture overlaps with title text", CreatedAt = DateTime.UtcNow.AddDays(-7), SubCategoryId = bugReportsSub.Id, UserId = userId },
                            new Topic { Title = "Images fail to upload when file size exceeds 5MB", CreatedAt = DateTime.UtcNow.AddDays(-2), SubCategoryId = bugReportsSub.Id, UserId = admin.Id },

                            // Site Rules & FAQ
                            new Topic { Title = "Community Guidelines & Forum Rules – Please read before posting", CreatedAt = DateTime.UtcNow.AddDays(-30), SubCategoryId = rulesFaqSub.Id, UserId = admin.Id },
                            new Topic { Title = "Frequently Asked Questions (FAQ) & Account Help", CreatedAt = DateTime.UtcNow.AddDays(-28), SubCategoryId = rulesFaqSub.Id, UserId = admin.Id }
                        });
                    }

                    await context.Topics.AddRangeAsync(sampleTopics);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}