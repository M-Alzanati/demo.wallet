using Domain.Entities;
using System.Data.Entity.Migrations;
using System.Linq;
using Common;
using Infrastructure.Data;

namespace Infrastructure.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WalletContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(WalletContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var user = User.Create("Test user", "h@gmail.com", PasswordHelper.ComputeSha256Hash("123"));
            context.Users.Add(user);
            context.SaveChanges();
        }
    }
}
