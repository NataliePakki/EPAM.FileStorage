using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Helpers;

namespace ORM {
    public class DbInitializer : CreateDatabaseIfNotExists<EntityModel> {
        protected override void Seed(EntityModel context) {
            IList<Role> defaultRoles = new List<Role>();

            defaultRoles.Add(new Role() { Name = "Administrator" });
            defaultRoles.Add(new Role() { Name = "Moderator" });
            defaultRoles.Add(new Role() { Name = "User" });

            foreach(var role in defaultRoles) {
                context.Set<Role>().Add(role);
            }

            var admin = new User() {
                Email = "admin@gmail.com",
                Name = "Admin",
                Password = Crypto.HashPassword("admin1234")
            };
            admin.Roles.Add(defaultRoles[0]);

            var user = new User() {
                Email = "user@gmail.com",
                Name = "User",
                Password = Crypto.HashPassword("user1234")
            };
            user.Roles.Add(defaultRoles[1]);


            context.Set<User>().Add(admin);
            context.Set<User>().Add(user);

            base.Seed(context);
        }
    }
}
