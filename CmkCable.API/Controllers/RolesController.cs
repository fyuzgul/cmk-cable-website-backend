using CmkCable.Business.Abstract;
using CmkCable.Business.Concrete;
using CmkCable.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CmkCable.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IRoleService _roleService;

        public RolesController()
        {
            _roleService = new RoleManager();
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = _roleService.GetAllRoles();
            return Ok(roles);
        }

        [HttpGet("active")]
        public IActionResult GetActiveRoles()
        {
            var roles = _roleService.GetActiveRoles();
            return Ok(roles);
        }

        [HttpGet("get/{id}")]
        public IActionResult GetRoleById(int id)
        {
            var role = _roleService.GetRoleById(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpPost("create")]
        [Authorize]
        public IActionResult CreateRole([FromBody] Role role)
        {
            if (role == null)
                return BadRequest();

            var createdRole = _roleService.CreateRole(role);
            return CreatedAtAction(nameof(GetRoleById), new { id = createdRole.Id }, createdRole);
        }

        [HttpPost("create-with-translations")]
        [Authorize]
        public IActionResult CreateRoleWithTranslations([FromBody] DTOs.CreateDTOs.CreateRoleWithTranslationsDTO request)
        {
            if (request == null)
                return BadRequest();

            var role = new Role
            {
                Name = request.Name,
                IsActive = request.IsActive
            };

            var translations = new List<RoleTranslation>();
            if (request.Translations != null)
            {
                foreach (var t in request.Translations)
                {
                    translations.Add(new RoleTranslation
                    {
                        LanguageId = t.LanguageId,
                        Name = t.Name
                    });
                }
            }

            var created = _roleService.CreateRoleWithTranslations(role, translations);
            return CreatedAtAction(nameof(GetRoleById), new { id = created.Id }, created);
        }

        [HttpPut("update")]
        [Authorize]
        public IActionResult UpdateRole([FromBody] Role role)
        {
            if (role == null || role.Id <= 0)
                return BadRequest();

            var updatedRole = _roleService.UpdateRole(role);
            if (updatedRole == null)
                return NotFound();

            return Ok(updatedRole);
        }

        [HttpDelete("delete/{id}")]
        [Authorize]
        public IActionResult DeleteRole(int id)
        {
            if (id <= 0)
                return BadRequest();

            _roleService.DeleteRole(id);
            return NoContent();
        }
    }
}
