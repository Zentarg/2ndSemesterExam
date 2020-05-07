using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class RolesHandler
    {
        public static Dictionary<int, Role> GetAllRoles(ParknGardenData db)
        {
            Dictionary<int, Role> roles = new Dictionary<int, Role>();

            List<Role> roleDB = db.Roles.ToList();

            foreach (Role role in roleDB)
            {
                roles.Add(role.ID, role);
            }

            return roles;

        }

        public static Role GetOneRole(int roleID, ParknGardenData db)
        {
            return db.Roles.FirstOrDefault(r => r.ID == roleID);
        }

    }
}