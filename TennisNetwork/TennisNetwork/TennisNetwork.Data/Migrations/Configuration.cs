namespace TennisNetwork.Data.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TennisNetwork.Models;

    public sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(TennisNetwork.Data.ApplicationDbContext context)
        {
            #region Example
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            // 
            #endregion

            SeedUserLevel(context);
            SeedRoles(context);
        }

        private void SeedRoles(ApplicationDbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            if (!roleManager.RoleExists("Users"))
            {
                roleManager.Create(new IdentityRole("Users"));
            }
            if (!roleManager.RoleExists("Administrators"))
            {
                roleManager.Create(new IdentityRole("Administrators"));
            }

            context.SaveChanges();
        }

        private void SeedUserLevel(ApplicationDbContext context)
        {
            context.UserLevels.AddOrUpdate(
                ul => ul.Level,
                new UserLevel { Level = "2.0" },
                new UserLevel { Level = "2.5" },
                new UserLevel { Level = "3.0" },
                new UserLevel { Level = "3.5" },
                new UserLevel { Level = "4.0" },
                new UserLevel { Level = "4.5" },
                new UserLevel { Level = "5.0" },
                new UserLevel { Level = "5.5" },
                new UserLevel { Level = "6.0" }
                );
        }
    }
}
