using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Abstract
{
    public interface IRoleService
    {
        Role CreateRole(Role role);
        Role CreateRoleWithTranslations(Role role, List<RoleTranslation> translations);
        Role UpdateRole(Role role);
        void DeleteRole(int id);
        Role GetRoleById(int id);
        List<Role> GetAllRoles();
        List<Role> GetActiveRoles();
    }
}

