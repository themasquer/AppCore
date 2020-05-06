using AppCore.Business.Abstracts.Services.Identity;
using AppCore.Business.Concretes.Helpers.Security.Hash;
using AppCore.Business.Concretes.Models.Identity;
using AppCore.Business.Concretes.Models.Results;
using AppCore.Business.Concretes.Models.Security.Hash;
using AppCore.Business.Configs;
using AppCore.DataAccess.Abstracts.EntityFramework.Identity;
using AppCore.Entities.Concretes.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppCore.Business.Concretes.Services.Identity
{
    public sealed class IdentityService : IIdentityService
    {
        private readonly IdentityUserDalBase _userDal;
        private readonly IdentityUserClaimDalBase _userClaimDal;
        private readonly IdentityClaimDalBase _claimDal;
        private readonly IdentityUserRoleDalBase _userRoleDal;
        private readonly IdentityRoleDalBase _roleDal;

        public IdentityService(IdentityUserDalBase userDal, IdentityUserClaimDalBase userClaimDal,
            IdentityClaimDalBase claimDal, IdentityUserRoleDalBase userRoleDal, IdentityRoleDalBase roleDal)
        {
            _userDal = userDal;
            _userClaimDal = userClaimDal;
            _claimDal = claimDal;
            _userRoleDal = userRoleDal;
            _roleDal = roleDal;
        }

        #region IdentityUser
        public Result<IdentityUserModel> GetUserByUserName(string userName, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.UserName == userName && e.Active == active);
                return GetUserModel(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> GetUserByUserNameAndPassword(string userName, string password, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.UserName == userName && e.Active == active);
                if (user != null)
                {
                    var cryptoHelper = new CryptoHelper();
                    if (!cryptoHelper.VerifyHash(password, user.PasswordHash, user.PasswordSalt))
                    {
                        return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.PasswordNotFoundMessage);
                    }
                }
                return GetUserModel(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> GetUser(int id, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.Id == id && e.Active == active);
                return GetUserModel(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> GetUserByGuid(string guid, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.Guid == guid && e.Active == active);
                return GetUserModel(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUser> GetUserEntityByUserName(string userName, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.UserName == userName && e.Active == active);
                if (user == null)
                {
                    return new ErrorResult<IdentityUser>(IdentityServiceConfig.UserNotFoundMessage);
                }
                return new SuccessResult<IdentityUser>(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUser>(exc);
            }
        }

        public Result<IdentityUser> GetUserEntity(int id, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.Id == id && e.Active == active);
                if (user == null)
                {
                    return new ErrorResult<IdentityUser>(IdentityServiceConfig.UserNotFoundMessage);
                }
                return new SuccessResult<IdentityUser>(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUser>(exc);
            }
        }

        public Result<IdentityUser> GetUserEntityByGuid(string guid, bool active = true)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                var user = query.SingleOrDefault(e => e.Guid == guid && e.Active == active);
                if (user == null)
                {
                    return new ErrorResult<IdentityUser>(IdentityServiceConfig.UserNotFoundMessage);
                }
                return new SuccessResult<IdentityUser>(user);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUser>(exc);
            }
        }

        public Result<List<IdentityUserModel>> GetUsers(bool onlyActive = false)
        {
            try
            {
                var query = _userDal.GetEntityQuery("IdentityUserRoles", "IdentityUserClaims");
                if (onlyActive)
                {
                    query = query.Where(e => e.Active == true);
                }
                var users = query.ToList();
                return GetUsersModel(users);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityUserModel>>(exc);
            }
        }

        public Result<IdentityUserModel> AddUser(IdentityUserModel userModel)
        {
            try
            {
                if (!UserExists(userModel.UserName))
                {
                    var cryptoHelper = new CryptoHelper();
                    Crypto crypto = cryptoHelper.CreateHash(userModel.Password);
                    var entity = new IdentityUser()
                    {
                        UserName = userModel.UserName,
                        PasswordHash = crypto.Hash,
                        PasswordSalt = crypto.Salt,
                        Email = userModel.Email,
                        EmailConfirmed = userModel.EmailConfirmed ?? true,
                        PhoneNumber = userModel.PhoneNumber,
                        PhoneNumberConfirmed = userModel.PhoneNumberConfirmed ?? true,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        Active = userModel.Active ?? true,
                        CreatedBy = userModel.CreatedBy ?? IdentityServiceConfig.OperationBy,
                        CreateDate = userModel.CreateDate ?? DateTime.Now
                    };
                    _userDal.AddEntity(entity);
                    UpdateUserModelIds(userModel, entity);
                    var result = AddUserRolesByUser(userModel.IdentityRoles, entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    result = AddUserClaimsByUser(userModel.IdentityClaims, entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    return new SuccessResult<IdentityUserModel>(userModel);
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> UpdateUser(IdentityUserModel userModel)
        {
            try
            {
                var entity = _userDal.GetEntity(userModel.Id);
                if (entity != null)
                {
                    if (!UserExists(userModel.UserName))
                    {
                        entity.UserName = userModel.UserName;
                        entity.Email = userModel.Email;
                        entity.EmailConfirmed = userModel.EmailConfirmed ?? true;
                        entity.PhoneNumber = userModel.PhoneNumber;
                        entity.PhoneNumberConfirmed = userModel.PhoneNumberConfirmed ?? true;
                        entity.FirstName = userModel.FirstName;
                        entity.LastName = userModel.LastName;
                        entity.Active = userModel.Active ?? true;
                        entity.UpdatedBy = userModel.UpdatedBy ?? IdentityServiceConfig.OperationBy;
                        entity.UpdateDate = userModel.UpdateDate ?? DateTime.Now;
                        _userDal.UpdateEntity(entity);
                        UpdateUserModelIds(userModel, entity);
                        var result = UpdateUserRolesByUser(userModel.IdentityRoles, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserRolesErrorMessage,
                                result.Exception);
                        }
                        result = UpdateUserClaimsByUser(userModel.IdentityClaims, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserClaimsErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityUserModel>(userModel);
                    }
                    return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserFoundMessage);
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> UpdateUserByGuid(IdentityUserModel userModel, string guid)
        {
            try
            {
                var entity = _userDal.GetEntity(guid);
                if (entity != null)
                {
                    if (!UserExists(userModel.UserName))
                    {
                        entity.UserName = userModel.UserName;
                        entity.Email = userModel.Email;
                        entity.EmailConfirmed = userModel.EmailConfirmed ?? true;
                        entity.PhoneNumber = userModel.PhoneNumber;
                        entity.PhoneNumberConfirmed = userModel.PhoneNumberConfirmed ?? true;
                        entity.FirstName = userModel.FirstName;
                        entity.LastName = userModel.LastName;
                        entity.Active = userModel.Active ?? true;
                        entity.UpdatedBy = userModel.UpdatedBy ?? IdentityServiceConfig.OperationBy;
                        entity.UpdateDate = userModel.UpdateDate ?? DateTime.Now;
                        _userDal.UpdateEntity(entity);
                        UpdateUserModelIds(userModel, entity);
                        var result = UpdateUserRolesByUser(userModel.IdentityRoles, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserRolesErrorMessage,
                                result.Exception);
                        }
                        result = UpdateUserClaimsByUser(userModel.IdentityClaims, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserClaimsErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityUserModel>(userModel);
                    }
                    return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserFoundMessage);
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> UpdateUserByUserName(IdentityUserModel userModel, string userName)
        {
            try
            {
                var entity = _userDal.GetEntity(e => e.UserName == userName);
                if (entity != null)
                {
                    if (!UserExists(userModel.UserName))
                    {
                        entity.UserName = userModel.UserName;
                        entity.Email = userModel.Email;
                        entity.EmailConfirmed = userModel.EmailConfirmed ?? true;
                        entity.PhoneNumber = userModel.PhoneNumber;
                        entity.PhoneNumberConfirmed = userModel.PhoneNumberConfirmed ?? true;
                        entity.FirstName = userModel.FirstName;
                        entity.LastName = userModel.LastName;
                        entity.Active = userModel.Active ?? true;
                        entity.UpdatedBy = userModel.UpdatedBy ?? IdentityServiceConfig.OperationBy;
                        entity.UpdateDate = userModel.UpdateDate ?? DateTime.Now;
                        _userDal.UpdateEntity(entity);
                        UpdateUserModelIds(userModel, entity);
                        var result = UpdateUserRolesByUser(userModel.IdentityRoles, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserRolesErrorMessage,
                                result.Exception);
                        }
                        result = UpdateUserClaimsByUser(userModel.IdentityClaims, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserClaimsErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityUserModel>(userModel);
                    }
                    return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserFoundMessage);
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> UpdateUserPassword(int userId, string password)
        {
            try
            {
                var entity = _userDal.GetEntity(userId);
                if (entity != null)
                {
                    var cryptoHelper = new CryptoHelper();
                    Crypto crypto = cryptoHelper.CreateHash(password);
                    entity.PasswordHash = crypto.Hash;
                    entity.PasswordSalt = crypto.Salt;
                    entity.UpdatedBy = IdentityServiceConfig.OperationBy;
                    entity.UpdateDate = DateTime.Now;
                    _userDal.UpdateEntity(entity);
                    var result = GetUserModel(entity);
                    if (result.Success)
                    {
                        result.Message = IdentityServiceConfig.PasswordUpdatedMessage;
                    }
                    return result;
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> UpdateUserPasswordByGuid(string guid, string password)
        {
            try
            {
                var entity = _userDal.GetEntity(guid);
                if (entity != null)
                {
                    var cryptoHelper = new CryptoHelper();
                    Crypto crypto = cryptoHelper.CreateHash(password);
                    entity.PasswordHash = crypto.Hash;
                    entity.PasswordSalt = crypto.Salt;
                    entity.UpdatedBy = IdentityServiceConfig.OperationBy;
                    entity.UpdateDate = DateTime.Now;
                    _userDal.UpdateEntity(entity);
                    var result = GetUserModel(entity);
                    if (result.Success)
                    {
                        result.Message = IdentityServiceConfig.PasswordUpdatedMessage;
                    }
                    return result;
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result<IdentityUserModel> UpdateUserPasswordByUserName(string userName, string password)
        {
            try
            {
                var entity = _userDal.GetEntity(e => e.UserName == userName);
                if (entity != null)
                {
                    var cryptoHelper = new CryptoHelper();
                    Crypto crypto = cryptoHelper.CreateHash(password);
                    entity.PasswordHash = crypto.Hash;
                    entity.PasswordSalt = crypto.Salt;
                    entity.UpdatedBy = IdentityServiceConfig.OperationBy;
                    entity.UpdateDate = DateTime.Now;
                    _userDal.UpdateEntity(entity);
                    var result = GetUserModel(entity);
                    if (result.Success)
                    {
                        result.Message = IdentityServiceConfig.PasswordUpdatedMessage;
                    }
                    return result;
                }
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityUserModel>(exc);
            }
        }

        public Result DeleteUser(int id)
        {
            try
            {
                var entity = _userDal.GetEntity(id);
                if (entity != null)
                {
                    var result = DeleteUserRolesByUser(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    result = DeleteUserClaimsByUser(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    entity.UpdatedBy = IdentityServiceConfig.OperationBy;
                    entity.UpdateDate = DateTime.Now;
                    _userDal.DeleteEntity(entity);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        public Result DeleteUserByGuid(string guid)
        {
            try
            {
                var entity = _userDal.GetEntity(guid);
                if (entity != null)
                {
                    var result = DeleteUserRolesByUser(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    result = DeleteUserClaimsByUser(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    entity.UpdatedBy = IdentityServiceConfig.OperationBy;
                    entity.UpdateDate = DateTime.Now;
                    _userDal.DeleteEntity(entity);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        public Result DeleteUserByUserName(string userName)
        {
            try
            {
                var entity = _userDal.GetEntity(e => e.UserName == userName);
                if (entity != null)
                {
                    var result = DeleteUserRolesByUser(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    result = DeleteUserClaimsByUser(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    entity.UpdatedBy = IdentityServiceConfig.OperationBy;
                    entity.UpdateDate = DateTime.Now;
                    _userDal.DeleteEntity(entity);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.UserNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }
        #endregion

        #region IdentityRole
        public Result<IdentityRoleModel> GetRole(int id)
        {
            try
            {
                var role = _roleDal.GetEntity(id, "IdentityUserRoles");
                return GetRoleModel(role);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result<IdentityRoleModel> GetRoleByGuid(string guid)
        {
            try
            {
                var role = _roleDal.GetEntity(guid, "IdentityUserRoles");
                return GetRoleModel(role);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result<IdentityRoleModel> GetRoleByName(string name)
        {
            try
            {
                var role = _roleDal.GetEntity(e => e.Name == name, "IdentityUserRoles");
                return GetRoleModel(role);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result<List<IdentityRoleModel>> GetRoles()
        {
            try
            {
                var roles = _roleDal.GetEntities("IdentityUserRoles");
                return GetRolesModel(roles);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityRoleModel>>(exc);
            }
        }

        public Result<IdentityRoleModel> AddRole(IdentityRoleModel roleModel)
        {
            try
            {
                if (!RoleExists(roleModel.Name))
                {
                    var entity = new IdentityRole()
                    {
                        Name = roleModel.Name,
                        Description = roleModel.Description
                    };
                    _roleDal.AddEntity(entity);
                    UpdateRoleModelIds(roleModel, entity);
                    var result = AddUserRolesByRole(roleModel.IdentityUsers, entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    return new SuccessResult<IdentityRoleModel>(roleModel);
                }
                return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result<IdentityRoleModel> UpdateRole(IdentityRoleModel roleModel)
        {
            try
            {
                var entity = _roleDal.GetEntity(roleModel.Id);
                if (entity != null)
                {
                    if (!RoleExists(roleModel.Name))
                    {
                        entity.Name = roleModel.Name;
                        entity.Description = roleModel.Description;
                        _roleDal.UpdateEntity(entity);
                        UpdateRoleModelIds(roleModel, entity);
                        var result = UpdateUserRolesByRole(roleModel.IdentityUsers, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.UserRolesErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityRoleModel>(roleModel);
                    }
                    return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleFoundMessage);
                }
                return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result<IdentityRoleModel> UpdateRoleByGuid(IdentityRoleModel roleModel, string guid)
        {
            try
            {
                var entity = _roleDal.GetEntity(guid);
                if (entity != null)
                {
                    if (!RoleExists(roleModel.Name))
                    {
                        entity.Name = roleModel.Name;
                        entity.Description = roleModel.Description;
                        _roleDal.UpdateEntity(entity);
                        UpdateRoleModelIds(roleModel, entity);
                        var result = UpdateUserRolesByRole(roleModel.IdentityUsers, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.UserRolesErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityRoleModel>(roleModel);
                    }
                    return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleFoundMessage);
                }
                return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result<IdentityRoleModel> UpdateRoleByName(IdentityRoleModel roleModel, string name)
        {
            try
            {
                var entity = _roleDal.GetEntity(e => e.Name == name);
                if (entity != null)
                {
                    if (!RoleExists(roleModel.Name))
                    {
                        entity.Name = roleModel.Name;
                        entity.Description = roleModel.Description;
                        _roleDal.UpdateEntity(entity);
                        UpdateRoleModelIds(roleModel, entity);
                        var result = UpdateUserRolesByRole(roleModel.IdentityUsers, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.UserRolesErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityRoleModel>(roleModel);
                    }
                    return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleFoundMessage);
                }
                return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityRoleModel>(exc);
            }
        }

        public Result DeleteRole(int id)
        {
            try
            {
                var entity = _roleDal.GetEntity(id);
                if (entity != null)
                {
                    var result = DeleteUserRolesByRole(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    _roleDal.DeleteEntity(id);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.RoleNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        public Result DeleteRoleByGuid(string guid)
        {
            try
            {
                var entity = _roleDal.GetEntity(guid);
                if (entity != null)
                {
                    var result = DeleteUserRolesByRole(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    _roleDal.DeleteEntity(guid);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.RoleNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        public Result DeleteRoleByName(string name)
        {
            try
            {
                var entity = _roleDal.GetEntity(e => e.Name == name);
                if (entity != null)
                {
                    var result = DeleteUserRolesByRole(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserRolesErrorMessage, result.Exception);
                    }
                    _roleDal.DeleteEntity(entity);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.RoleNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }
        #endregion

        #region IdentityClaim
        public Result<IdentityClaimModel> GetClaim(int id)
        {
            try
            {
                var claim = _claimDal.GetEntity(id, "IdentityUserClaims");
                return GetClaimModel(claim);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result<IdentityClaimModel> GetClaimByGuid(string guid)
        {
            try
            {
                var claim = _claimDal.GetEntity(guid, "IdentityUserClaims");
                return GetClaimModel(claim);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result<IdentityClaimModel> GetClaimByType(string type)
        {
            try
            {
                var claim = _claimDal.GetEntity(e => e.Type == type, "IdentityUserClaims");
                return GetClaimModel(claim);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result<List<IdentityClaimModel>> GetClaims()
        {
            try
            {
                var claims = _claimDal.GetEntities("IdentityUserClaims");
                return GetClaimsModel(claims);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityClaimModel>>(exc);
            }
        }

        public Result<IdentityClaimModel> AddClaim(IdentityClaimModel claimModel)
        {
            try
            {
                if (!ClaimExists(claimModel.Type))
                {
                    var entity = new IdentityClaim()
                    {
                        Type = claimModel.Type,
                        Value = claimModel.Value
                    };
                    _claimDal.AddEntity(entity);
                    UpdateClaimModelIds(claimModel, entity);
                    var result = AddUserClaimsByClaim(claimModel.IdentityUsers, entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    return new SuccessResult<IdentityClaimModel>(claimModel);
                }
                return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result<IdentityClaimModel> UpdateClaim(IdentityClaimModel claimModel)
        {
            try
            {
                var entity = _claimDal.GetEntity(claimModel.Id);
                if (entity != null)
                {
                    if (!ClaimExists(claimModel.Type))
                    {
                        entity.Type = claimModel.Type;
                        entity.Value = claimModel.Value;
                        _claimDal.UpdateEntity(entity);
                        UpdateClaimModelIds(claimModel, entity);
                        var result = UpdateUserClaimsByClaim(claimModel.IdentityUsers, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.UserClaimsErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityClaimModel>(claimModel);
                    }
                    return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimFoundMessage);
                }
                return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result<IdentityClaimModel> UpdateClaimByGuid(IdentityClaimModel claimModel, string guid)
        {
            try
            {
                var entity = _claimDal.GetEntity(guid);
                if (entity != null)
                {
                    if (!ClaimExists(claimModel.Type))
                    {
                        entity.Type = claimModel.Type;
                        entity.Value = claimModel.Value;
                        _claimDal.UpdateEntity(entity);
                        UpdateClaimModelIds(claimModel, entity);
                        var result = UpdateUserClaimsByClaim(claimModel.IdentityUsers, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.UserClaimsErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityClaimModel>(claimModel);
                    }
                    return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimFoundMessage);
                }
                return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result<IdentityClaimModel> UpdateClaimByType(IdentityClaimModel claimModel, string type)
        {
            try
            {
                var entity = _claimDal.GetEntity(e => e.Type == type);
                if (entity != null)
                {
                    if (!ClaimExists(claimModel.Type))
                    {
                        entity.Type = claimModel.Type;
                        entity.Value = claimModel.Value;
                        _claimDal.UpdateEntity(entity);
                        UpdateClaimModelIds(claimModel, entity);
                        var result = UpdateUserClaimsByClaim(claimModel.IdentityUsers, entity.Id);
                        if (!result.Success)
                        {
                            return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.UserClaimsErrorMessage,
                                result.Exception);
                        }
                        return new SuccessResult<IdentityClaimModel>(claimModel);
                    }
                    return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimFoundMessage);
                }
                return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult<IdentityClaimModel>(exc);
            }
        }

        public Result DeleteClaim(int id)
        {
            try
            {
                var entity = _claimDal.GetEntity(id);
                if (entity != null)
                {
                    var result = DeleteUserClaimsByClaim(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    _claimDal.DeleteEntity(id);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        public Result DeleteClaimByGuid(string guid)
        {
            try
            {
                var entity = _claimDal.GetEntity(guid);
                if (entity != null)
                {
                    var result = DeleteUserClaimsByClaim(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    _claimDal.DeleteEntity(guid);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        public Result DeleteClaimByType(string type)
        {
            try
            {
                var entity = _claimDal.GetEntity(e => e.Type == type);
                if (entity != null)
                {
                    var result = DeleteUserClaimsByClaim(entity.Id);
                    if (!result.Success)
                    {
                        return new ErrorResult(IdentityServiceConfig.UserClaimsErrorMessage, result.Exception);
                    }
                    _claimDal.DeleteEntity(entity);
                    return new SuccessResult();
                }
                return new ErrorResult(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }
        #endregion

        #region IdentityUser Private Methods
        private Result<IdentityUserModel> GetUserModel(IdentityUser user)
        {
            if (user == null)
            {
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserNotFoundMessage);
            }
            IdentityUserModel model = new IdentityUserModel()
            {
                Id = user.Id,
                Guid = user.Guid,
                UserName = user.UserName,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Active = user.Active,
                CreatedBy = user.CreatedBy,
                CreateDate = user.CreateDate,
                UpdatedBy = user.UpdatedBy,
                UpdateDate = user.UpdateDate
            };
            var rolesResult = GetRolesByUser(model.Id);
            if (rolesResult.Exception)
            {
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserRolesErrorMessage, rolesResult.Exception);
            }
            model.IdentityRoles = rolesResult.Data;
            var claimsResult = GetClaimsByUser(model.Id);
            if (claimsResult.Exception)
            {
                return new ErrorResult<IdentityUserModel>(IdentityServiceConfig.UserClaimsErrorMessage, claimsResult.Exception);
            }
            model.IdentityClaims = claimsResult.Data;
            return new SuccessResult<IdentityUserModel>(model);
        }

        private Result<List<IdentityUserModel>> GetUsersModel(List<IdentityUser> users, bool includeRolesAndCliams = true)
        {
            if (users == null || users.Count == 0)
            {
                return new ErrorResult<List<IdentityUserModel>>(IdentityServiceConfig.UsersNotFoundMessage);
            }
            List<IdentityUserModel> model = users.Select(e => new IdentityUserModel()
            {
                Id = e.Id,
                Guid = e.Guid,
                UserName = e.UserName,
                Email = e.Email,
                EmailConfirmed = e.EmailConfirmed,
                PhoneNumber = e.PhoneNumber,
                PhoneNumberConfirmed = e.PhoneNumberConfirmed,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Active = e.Active,
                CreatedBy = e.CreatedBy,
                CreateDate = e.CreateDate,
                UpdatedBy = e.UpdatedBy,
                UpdateDate = e.UpdateDate
            }).ToList();
            if (includeRolesAndCliams)
            {
                Result<List<IdentityRoleModel>> rolesResult;
                Result<List<IdentityClaimModel>> claimsResult;
                Result<List<IdentityUserModel>> result = null;
                foreach (var user in model)
                {
                    rolesResult = GetRolesByUser(user.Id);
                    if (rolesResult.Exception)
                    {
                        result = new ErrorResult<List<IdentityUserModel>>(IdentityServiceConfig.UserRolesErrorMessage,
                            rolesResult.Exception);
                        break;
                    }
                    user.IdentityRoles = rolesResult.Data;
                    claimsResult = GetClaimsByUser(user.Id);
                    if (claimsResult.Exception)
                    {
                        result = new ErrorResult<List<IdentityUserModel>>(IdentityServiceConfig.UserClaimsErrorMessage,
                            claimsResult.Exception);
                        break;
                    }
                    user.IdentityClaims = claimsResult.Data;
                }
                if (result != null)
                {
                    return result;
                }
            }
            return new SuccessResult<List<IdentityUserModel>>(model);
        }

        private bool UserExists(string userName, bool onlyActive = false)
        {
            if (onlyActive)
            {
                return _userDal.EntityExists(e => e.UserName == userName && e.Active == true);
            }
            return _userDal.EntityExists(e => e.UserName == userName);
        }

        private void UpdateUserModelIds(IdentityUserModel model, IdentityUser entity)
        {
            model.Id = entity.Id;
            model.Guid = entity.Guid;
        }
        #endregion

        #region IdentityRole Private Methods
        private Result<IdentityRoleModel> GetRoleModel(IdentityRole role)
        {
            if (role == null)
            {
                return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.RoleNotFoundMessage);
            }
            IdentityRoleModel model = new IdentityRoleModel()
            {
                Id = role.Id,
                Guid = role.Guid,
                Name = role.Name,
                Description = role.Description
            };
            var usersResult = GetUsersByRole(model.Id);
            if (usersResult.Exception)
            {
                return new ErrorResult<IdentityRoleModel>(IdentityServiceConfig.UserRolesErrorMessage, usersResult.Exception);
            }
            model.IdentityUsers = usersResult.Data;
            return new SuccessResult<IdentityRoleModel>(model);
        }

        private Result<List<IdentityRoleModel>> GetRolesModel(List<IdentityRole> roles, bool includeUsers = true)
        {
            if (roles == null || roles.Count == 0)
            {
                return new ErrorResult<List<IdentityRoleModel>>(IdentityServiceConfig.RolesNotFoundMessage);
            }
            List<IdentityRoleModel> model = roles.Select(e => new IdentityRoleModel()
            {
                Id = e.Id,
                Guid = e.Guid,
                Name = e.Name,
                Description = e.Description
            }).ToList();
            if (includeUsers)
            {
                Result<List<IdentityUserModel>> usersResult = null;
                foreach (var role in model)
                {
                    usersResult = GetUsersByRole(role.Id);
                    if (usersResult.Exception)
                    {
                        break;
                    }
                    role.IdentityUsers = usersResult.Data;
                }
                if (usersResult != null && usersResult.Exception)
                {
                    return new ErrorResult<List<IdentityRoleModel>>(IdentityServiceConfig.UserRolesErrorMessage,
                        usersResult.Exception);
                }
            }
            return new SuccessResult<List<IdentityRoleModel>>(model);
        }

        private bool RoleExists(string name)
        {
            return _roleDal.EntityExists(e => e.Name == name);
        }

        private void UpdateRoleModelIds(IdentityRoleModel model, IdentityRole entity)
        {
            model.Id = entity.Id;
            model.Guid = entity.Guid;
        }
        #endregion

        #region IdentityClaim Private Methods
        private Result<IdentityClaimModel> GetClaimModel(IdentityClaim claim)
        {
            if (claim == null)
            {
                return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.ClaimNotFoundMessage);
            }
            IdentityClaimModel model = new IdentityClaimModel()
            {
                Id = claim.Id,
                Guid = claim.Guid,
                Type = claim.Type,
                Value = claim.Value
            };
            var usersResult = GetUsersByClaim(model.Id);
            if (usersResult.Exception)
            {
                return new ErrorResult<IdentityClaimModel>(IdentityServiceConfig.UserClaimsErrorMessage, usersResult.Exception);
            }
            model.IdentityUsers = usersResult.Data;
            return new SuccessResult<IdentityClaimModel>(model);
        }

        private Result<List<IdentityClaimModel>> GetClaimsModel(List<IdentityClaim> claims, bool includeUsers = true)
        {
            if (claims == null || claims.Count == 0)
            {
                return new ErrorResult<List<IdentityClaimModel>>(IdentityServiceConfig.ClaimsNotFoundMessage);
            }
            List<IdentityClaimModel> model = claims.Select(e => new IdentityClaimModel()
            {
                Id = e.Id,
                Guid = e.Guid,
                Type = e.Type,
                Value = e.Value
            }).ToList();
            if (includeUsers)
            {
                Result<List<IdentityUserModel>> usersResult = null;
                foreach (var claim in model)
                {
                    usersResult = GetUsersByClaim(claim.Id);
                    if (usersResult.Exception)
                    {
                        break;
                    }
                    claim.IdentityUsers = usersResult.Data;
                }
                if (usersResult != null && usersResult.Exception)
                {
                    return new ErrorResult<List<IdentityClaimModel>>(IdentityServiceConfig.UserClaimsErrorMessage,
                        usersResult.Exception);
                }
            }
            return new SuccessResult<List<IdentityClaimModel>>(model);
        }

        private bool ClaimExists(string type)
        {
            return _claimDal.EntityExists(e => e.Type == type);
        }

        private void UpdateClaimModelIds(IdentityClaimModel model, IdentityClaim entity)
        {
            model.Id = entity.Id;
            model.Guid = entity.Guid;
        }
        #endregion

        #region IdentityUserRole Private Methods
        private Result<List<IdentityUserModel>> GetUsersByRole(int roleId)
        {
            try
            {
                var userRoles = _userRoleDal.GetEntities(e => e.IdentityRoleId == roleId, "IdentityUser");
                if (userRoles.Count == 0)
                {
                    return new ErrorResult<List<IdentityUserModel>>(IdentityServiceConfig.UsersNotFoundMessage);
                }
                return GetUsersModel(userRoles.Select(e => e.IdentityUser).ToList(), false);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityUserModel>>(exc);
            }
        }

        private Result<List<IdentityRoleModel>> GetRolesByUser(int userId)
        {
            try
            {
                var userRoles = _userRoleDal.GetEntities(e => e.IdentityUserId == userId, "IdentityRole");
                if (userRoles.Count == 0)
                {
                    return new ErrorResult<List<IdentityRoleModel>>(IdentityServiceConfig.RolesNotFoundMessage);
                }
                return GetRolesModel(userRoles.Select(e => e.IdentityRole).ToList(), false);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityRoleModel>>(exc);
            }
        }

        private Result AddUserRolesByUser(List<IdentityRoleModel> roles, int userId)
        {
            try
            {
                if (roles != null && roles.Count > 0)
                {
                    _userRoleDal.Commit = false;
                    foreach (var role in roles)
                    {
                        var userRole = new IdentityUserRole()
                        {
                            IdentityUserId = userId,
                            IdentityRoleId = role.Id
                        };
                        _userRoleDal.AddEntity(userRole);
                    }
                    _userRoleDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result AddUserRolesByRole(List<IdentityUserModel> users, int roleId)
        {
            try
            {
                if (users != null && users.Count > 0)
                {
                    _userRoleDal.Commit = false;
                    foreach (var user in users)
                    {
                        var userRole = new IdentityUserRole()
                        {
                            IdentityUserId = user.Id,
                            IdentityRoleId = roleId
                        };
                        _userRoleDal.AddEntity(userRole);
                    }
                    _userRoleDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result UpdateUserRolesByUser(List<IdentityRoleModel> roles, int userId)
        {
            try
            {
                var result = DeleteUserRolesByUser(userId);
                if (result.Success)
                {
                    return AddUserRolesByUser(roles, userId);
                }
                return result;
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result UpdateUserRolesByRole(List<IdentityUserModel> users, int roleId)
        {
            try
            {
                var result = DeleteUserRolesByRole(roleId);
                if (result.Success)
                {
                    return AddUserRolesByRole(users, roleId);
                }
                return result;
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result DeleteUserRolesByUser(int userId)
        {
            try
            {
                var userRoles = _userRoleDal.GetEntities(e => e.IdentityUserId == userId);
                if (userRoles != null && userRoles.Count > 0)
                {
                    _userRoleDal.Commit = false;
                    foreach (var userRole in userRoles)
                    {
                        _userRoleDal.DeleteEntity(userRole);
                    }
                    _userRoleDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result DeleteUserRolesByRole(int roleId)
        {
            try
            {
                var userRoles = _userRoleDal.GetEntities(e => e.IdentityRoleId == roleId);
                if (userRoles != null && userRoles.Count > 0)
                {
                    _userRoleDal.Commit = false;
                    foreach (var userRole in userRoles)
                    {
                        _userRoleDal.DeleteEntity(userRole);
                    }
                    _userRoleDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }
        #endregion

        #region IdentityUserClaims Private Methods
        private Result<List<IdentityUserModel>> GetUsersByClaim(int claimId)
        {
            try
            {
                var userClaims = _userClaimDal.GetEntities(e => e.IdentityClaimId == claimId, "IdentityUser");
                if (userClaims.Count == 0)
                {
                    return new ErrorResult<List<IdentityUserModel>>(IdentityServiceConfig.UsersNotFoundMessage);
                }
                return GetUsersModel(userClaims.Select(e => e.IdentityUser).ToList(), false);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityUserModel>>(exc);
            }
        }

        private Result<List<IdentityClaimModel>> GetClaimsByUser(int userId)
        {
            try
            {
                var userClaims = _userClaimDal.GetEntities(e => e.IdentityUserId == userId, "IdentityClaim");
                if (userClaims.Count == 0)
                {
                    return new ErrorResult<List<IdentityClaimModel>>(IdentityServiceConfig.ClaimsNotFoundMessage);
                }
                return GetClaimsModel(userClaims.Select(e => e.IdentityClaim).ToList(), false);
            }
            catch (Exception exc)
            {
                return new ErrorResult<List<IdentityClaimModel>>(exc);
            }
        }

        private Result AddUserClaimsByUser(List<IdentityClaimModel> claims, int userId)
        {
            try
            {
                if (claims != null && claims.Count > 0)
                {
                    _userClaimDal.Commit = false;
                    foreach (var claim in claims)
                    {
                        var userClaim = new IdentityUserClaim()
                        {
                            IdentityUserId = userId,
                            IdentityClaimId = claim.Id
                        };
                        _userClaimDal.AddEntity(userClaim);
                    }
                    _userClaimDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result AddUserClaimsByClaim(List<IdentityUserModel> users, int claimId)
        {
            try
            {
                if (users != null && users.Count > 0)
                {
                    _userClaimDal.Commit = false;
                    foreach (var user in users)
                    {
                        var userClaim = new IdentityUserClaim()
                        {
                            IdentityUserId = user.Id,
                            IdentityClaimId = claimId
                        };
                        _userClaimDal.AddEntity(userClaim);
                    }
                    _userClaimDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result UpdateUserClaimsByUser(List<IdentityClaimModel> claims, int userId)
        {
            try
            {
                var result = DeleteUserClaimsByUser(userId);
                if (result.Success)
                {
                    return AddUserClaimsByUser(claims, userId);
                }
                return result;
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result UpdateUserClaimsByClaim(List<IdentityUserModel> users, int claimId)
        {
            try
            {
                var result = DeleteUserClaimsByClaim(claimId);
                if (result.Success)
                {
                    return AddUserClaimsByClaim(users, claimId);
                }
                return result;
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result DeleteUserClaimsByUser(int userId)
        {
            try
            {
                var userClaims = _userClaimDal.GetEntities(e => e.IdentityUserId == userId);
                if (userClaims != null && userClaims.Count > 0)
                {
                    _userClaimDal.Commit = false;
                    foreach (var userClaim in userClaims)
                    {
                        _userClaimDal.DeleteEntity(userClaim);
                    }
                    _userClaimDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }

        private Result DeleteUserClaimsByClaim(int claimId)
        {
            try
            {
                var userClaims = _userClaimDal.GetEntities(e => e.IdentityClaimId == claimId);
                if (userClaims != null && userClaims.Count > 0)
                {
                    _userClaimDal.Commit = false;
                    foreach (var userClaim in userClaims)
                    {
                        _userClaimDal.DeleteEntity(userClaim);
                    }
                    _userClaimDal.SaveChanges();
                }
                return new SuccessResult();
            }
            catch (Exception exc)
            {
                return new ErrorResult(exc);
            }
        }
        #endregion
    }
}
