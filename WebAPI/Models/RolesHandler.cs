using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPI.Models
{
    public static class RolesHandler
    {
        /// <summary>
        /// A method that gets all the roles from the database
        /// </summary>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <returns>Returns a dictionary where for a specific entry the key is the roleId and the value is of type Role</returns>
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

        /// <summary>
        /// A method that gets a specific Role
        /// </summary>
        /// <param name="roleID">roleID is the id used to find the role</param>
        /// <param name="db">db is the database to be passed to it of type ParknGardenData</param>
        /// <returns>returns either the default role if no role was found with the corresponding id, or the role that has an id match</returns>
        public static Role GetOneRole(int roleID, ParknGardenData db)
        {
            return db.Roles.FirstOrDefault(r => r.ID == roleID);
        }

        /// <summary>
        /// A method for creating new roles in the database
        /// </summary>
        /// <param name="db"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public static Role PostRole(ParknGardenData db, Role role)
        {
            bool checkRole(Role r) => r.Name.ToLower() == role.Name.ToLower();
            bool roleExists = db.Roles.Any(checkRole);

            if (!roleExists)
            {
                Role newRole = db.Roles.Add(role);
                db.SaveChanges();
                return newRole;
            }

            Role returnRole = db.Roles.FirstOrDefault(checkRole);
            return returnRole;
        }

    }
}