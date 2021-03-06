﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ReportManagement.Model.User;
using System;
using System.Threading.Tasks;
using System.Web.Http;

namespace ReportManagement.Controllers
{
    [Authorize(Roles = "Admin")]
    [RoutePrefix("v1/roles")]
    public class RolesController : BaseApiController
    {
        // URL: v1/roles/{id}
        [HttpGet]
        [Route("{id:guid}", Name = "GetRoleById")]
        public async Task<IHttpActionResult> GetRole(string id)
        {
            var role = await this.AppRoleManager.FindByIdAsync(id).ConfigureAwait(true); ;

            if (role != null)
            {
                return Ok(TheModelFactory.Create(role));
            }

            return NotFound();
        }

        // URL: v1/roles/
        [HttpGet]
        [Route("", Name = "GetAllRoles")]
        public IHttpActionResult GetAllRoles()
        {
            var roles = this.AppRoleManager.Roles;
            return Ok(roles);
        }

        // URL: v1/roles/create
        [HttpPost]
        [Route("create")]
        public async Task<IHttpActionResult> Create(CreateRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var role = new IdentityRole { Name = model.Name };

            var result = await this.AppRoleManager.CreateAsync(role).ConfigureAwait(true); ;

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            Uri locationHeader = new Uri(Url.Link("GetRoleById", new { id = role.Id }));

            return Created(locationHeader, TheModelFactory.Create(role));
        }

        // URL: v1/roles/{id}
        [Route("{id:guid}")]
        public async Task<IHttpActionResult> DeleteRole(string Id)
        {
            var role = await this.AppRoleManager.FindByIdAsync(Id).ConfigureAwait(true); ;

            if (role != null)
            {
                IdentityResult result = await this.AppRoleManager.DeleteAsync(role).ConfigureAwait(true); ;

                if (!result.Succeeded)
                {
                    return GetErrorResult(result);
                }
                return Ok();
            }

            return NotFound();
        }

        // URL: v1/roles/ManageUsersInRole
        [Route("ManageUsersInRole")]
        public async Task<IHttpActionResult> ManageUsersInRole(UsersInRoleModel model)
        {
            var role = await this.AppRoleManager.FindByIdAsync(model.Id).ConfigureAwait(true); ;

            if (role == null)
            {
                ModelState.AddModelError("", "Role does not exist");
                return BadRequest();
            }

            foreach (string user in model.EnrolledUsers)
            {
                var appUser = await this.AppUserManager.FindByIdAsync(user).ConfigureAwait(true); ;

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exist", user));
                    continue;
                }

                if (!this.AppUserManager.IsInRole(user, role.Name))
                {
                    IdentityResult result = await this.AppUserManager.AddToRoleAsync(user, role.Name);

                    if (!result.Succeeded)
                    {
                        ModelState.AddModelError("", String.Format("", String.Format("User: {0} could not be added to the specified role", user)));
                    }
                }
            }

            foreach (string user in model.RemovedUsers)
            {
                var appUser = await this.AppUserManager.FindByIdAsync(user).ConfigureAwait(true); ;

                if (appUser == null)
                {
                    ModelState.AddModelError("", String.Format("User: {0} does not exists", user));
                    continue;
                }

                IdentityResult result = await this.AppUserManager.RemoveFromRoleAsync(user, role.Name);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", String.Format("User: {0} could not be removed form the specified role", user));
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok();
        }
    }
}