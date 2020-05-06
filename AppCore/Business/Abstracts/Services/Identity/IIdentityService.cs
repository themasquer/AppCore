using System.Collections.Generic;
using System.Linq;
using AppCore.Business.Concretes.Models.Identity;
using AppCore.Business.Concretes.Models.Results;
using AppCore.Entities.Concretes.Identity;

namespace AppCore.Business.Abstracts.Services.Identity
{
    public interface IIdentityService
    {
        Result<IdentityUserModel> GetUserByUserName(string userName, bool active = true);
        Result<IdentityUserModel> GetUserByUserNameAndPassword(string userName, string password, bool active = true);
        Result<IdentityUserModel> GetUser(int id, bool active = true);
        Result<IdentityUserModel> GetUserByGuid(string guid, bool active = true);
        Result<IdentityUser> GetUserEntityByUserName(string userName, bool active = true);
        Result<IdentityUser> GetUserEntity(int id, bool active = true);
        Result<IdentityUser> GetUserEntityByGuid(string guid, bool active = true);
        Result<List<IdentityUserModel>> GetUsers(bool onlyActive = false);
        IQueryable<IdentityUser> GetUserQuery();
        Result<IdentityUserModel> GetUserModel(IdentityUser user);
        Result<List<IdentityUserModel>> GetUsersModel(List<IdentityUser> users);
        Result<IdentityUserModel> AddUser(IdentityUserModel userModel);
        Result<IdentityUserModel> UpdateUser(IdentityUserModel userModel);
        Result<IdentityUserModel> UpdateUserByGuid(IdentityUserModel userModel, string guid);
        Result<IdentityUserModel> UpdateUserByUserName(IdentityUserModel userModel, string userName);
        Result<IdentityUserModel> UpdateUserPassword(int userId, string password);
        Result<IdentityUserModel> UpdateUserPasswordByGuid(string guid, string password);
        Result<IdentityUserModel> UpdateUserPasswordByUserName(string userName, string password);
        Result DeleteUser(int id);
        Result DeleteUserByGuid(string guid);
        Result DeleteUserByUserName(string userName);
        Result<IdentityRoleModel> GetRole(int id);
        Result<IdentityRoleModel> GetRoleByGuid(string guid);
        Result<IdentityRoleModel> GetRoleByName(string name);
        Result<List<IdentityRoleModel>> GetRoles();
        Result<IdentityRoleModel> GetRoleModel(IdentityRole role);
        Result<List<IdentityRoleModel>> GetRolesModel(List<IdentityRole> roles);
        Result<IdentityRoleModel> AddRole(IdentityRoleModel roleModel);
        Result<IdentityRoleModel> UpdateRole(IdentityRoleModel roleModel);
        Result<IdentityRoleModel> UpdateRoleByGuid(IdentityRoleModel roleModel, string guid);
        Result<IdentityRoleModel> UpdateRoleByName(IdentityRoleModel roleModel, string name);
        Result DeleteRole(int id);
        Result DeleteRoleByGuid(string guid);
        Result DeleteRoleByName(string name);
        Result<IdentityClaimModel> GetClaim(int id);
        Result<IdentityClaimModel> GetClaimByGuid(string guid);
        Result<IdentityClaimModel> GetClaimByType(string type);
        Result<List<IdentityClaimModel>> GetClaims();
        Result<IdentityClaimModel> GetClaimModel(IdentityClaim claim);
        Result<List<IdentityClaimModel>> GetClaimsModel(List<IdentityClaim> claims);
        Result<IdentityClaimModel> AddClaim(IdentityClaimModel claimModel);
        Result<IdentityClaimModel> UpdateClaim(IdentityClaimModel claimModel);
        Result<IdentityClaimModel> UpdateClaimByGuid(IdentityClaimModel claimModel, string guid);
        Result<IdentityClaimModel> UpdateClaimByType(IdentityClaimModel claimModel, string type);
        Result DeleteClaim(int id);
        Result DeleteClaimByGuid(string guid);
        Result DeleteClaimByType(string type);
        Result<List<IdentityUserModel>> GetUsersByRole(int roleId);
        Result<List<IdentityRoleModel>> GetRolesByUser(int userId);
        Result<List<IdentityUserModel>> GetUsersByClaim(int claimId);
        Result<List<IdentityClaimModel>> GetClaimsByUser(int userId);
    }
}
