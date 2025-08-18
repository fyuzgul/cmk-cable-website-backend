using CmkCable.Business.Abstract;
using CmkCable.DataAccess.Abstract;
using CmkCable.DataAccess.Concrete;
using CmkCable.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.Business.Concrete
{
    public class RoleManager : IRoleService
    {
        private IRoleRepository _roleRepository;

        public RoleManager()
        {
            _roleRepository = new RoleRepository();
        }

        public Role CreateRole(Role role)
        {
            return _roleRepository.CreateRole(role);
        }

        public Role CreateRoleWithTranslations(Role role, List<RoleTranslation> translations)
        {
            return _roleRepository.CreateRoleWithTranslations(role, translations);
        }

        public void DeleteRole(int id)
        {
            _roleRepository.DeleteRole(id);
        }

        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAllRoles();
        }

        public List<Role> GetActiveRoles()
        {
            return _roleRepository.GetActiveRoles();
        }

        public Role GetRoleById(int id)
        {
            return _roleRepository.GetRoleById(id);
        }

        public Role UpdateRole(Role role)
        {
            return _roleRepository.UpdateRole(role);
        }
    }
}

