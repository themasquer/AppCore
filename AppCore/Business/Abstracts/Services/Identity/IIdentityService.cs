using System;
using AppCore.Business.Concretes.Models.Identity;
using AppCore.Business.Concretes.Models.Results;
using AppCore.Entities.Concretes.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AppCore.Business.Abstracts.Services.Identity
{
    public interface IIdentityService
    {
        bool ShowException { get; set; }
        Result<IQueryable<IdentityUser>> GetUserQuery();
        Result<IQueryable<IdentityUser>> GetUserQuery(Expression<Func<IdentityUser, bool>> predicate);
        Result<IdentityUserModel> GetUserByUserName(string userName, bool active = true);
        Result<IdentityUserModel> GetUserByUserNameAndPassword(string userName, string password, bool active = true);
        Result<IdentityUserModel> GetUser(int id, bool active = true);
        Result<IdentityUserModel> GetUserByGuid(string guid, bool active = true);
        Result<IdentityUser> GetUserEntityByUserName(string userName, bool active = true);
        Result<IdentityUser> GetUserEntity(int id, bool active = true);
        Result<IdentityUser> GetUserEntityByGuid(string guid, bool active = true);
        Result<List<IdentityUserModel>> GetUsers(bool onlyActive = false);
        Result<IdentityUserModel> GetUserModel(IdentityUser user);
        Result<List<IdentityUserModel>> GetUsersModel(List<IdentityUser> users, bool includeRolesAndCliams = true);
        Result<IdentityUserModel> AddUser(IdentityUserModel userModel);
        Result<IdentityUserModel> UpdateUser(IdentityUserModel userModel);
        Result<IdentityUserModel> UpdateUserByGuid(IdentityUserModel userModel, string guid);
        Result<IdentityUserModel> UpdateUserByUserName(IdentityUserModel userModel, string userName);
        Result<IdentityUserModel> UpdateUserPassword(string password, int userId);
        Result<IdentityUserModel> UpdateUserPassword(string password, int userId, string operationBy);
        Result<IdentityUserModel> UpdateUserPasswordByGuid(string password, string guid);
        Result<IdentityUserModel> UpdateUserPasswordByGuid(string password, string guid, string operationBy);
        Result<IdentityUserModel> UpdateUserPasswordByUserName(string password, string userName);
        Result<IdentityUserModel> UpdateUserPasswordByUserName(string password, string userName, string operationBy);
        Result DeleteUser(int id);
        Result DeleteUser(int id, string operationBy);
        Result DeleteUserByGuid(string guid);
        Result DeleteUserByGuid(string guid, string operationBy);
        Result DeleteUserByUserName(string userName);
        Result DeleteUserByUserName(string userName, string operationBy);
        Result<IdentityRoleModel> GetRole(int id);
        Result<IdentityRoleModel> GetRoleByGuid(string guid);
        Result<IdentityRoleModel> GetRoleByName(string name);
        Result<List<IdentityRoleModel>> GetRoles();
        Result<IdentityRoleModel> AddRole(IdentityRoleModel roleModel);
        Result<IdentityRoleModel> UpdateRole(IdentityRoleModel roleModel);
        Result<IdentityRoleModel> UpdateRoleByGuid(IdentityRoleModel roleModel, string guid);
        Result<IdentityRoleModel> UpdateRoleByName(IdentityRoleModel roleModel, string name);
        Result DeleteRole(int id);
        Result DeleteRoleByGuid(string guid);
        Result DeleteRoleByName(string name);
        Result<IdentityClaimModel> GetClaim(int id);
        Result<IdentityClaimModel> GetClaimByGuid(string guid);
        Result<List<IdentityClaimModel>> GetClaimsByType(string type);
        Result<List<IdentityClaimModel>> GetClaims();
        Result<IdentityClaimModel> AddClaim(IdentityClaimModel claimModel);
        Result<IdentityClaimModel> UpdateClaim(IdentityClaimModel claimModel);
        Result<IdentityClaimModel> UpdateClaimByGuid(IdentityClaimModel claimModel, string guid);
        Result DeleteClaim(int id);
        Result DeleteClaimByGuid(string guid);
        Result DeleteClaimsByType(string type);
    }
}
