using AppCore.Business.Concretes.Helpers.Security.Identity;
using AppCore.Business.Concretes.Models.Identity;
using AppCore.Business.Concretes.Models.Results;
using AppCore.Business.Concretes.Models.Security.Identity;
using AppCore.Business.Extensions;
using AppCore.Utils;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using AppCore.Business.Configs;

namespace AppCore.Business.Abstracts.Utils.Security.Identity
{
    public abstract class AccessTokenUtilBase
    {
        private readonly AppSettingsUtilBase _appSettingsUtil;

        public bool ShowException { get; set; } = false;

        protected AccessTokenUtilBase(AppSettingsUtilBase appSettingsUtil)
        {
            _appSettingsUtil = appSettingsUtil;
        }

        public virtual Result<AccessToken> CreateAccessToken(IdentityUserModel user, string appSettingsSectionKey = "AccessTokenOptions")
        {
            try
            {
                var accessTokenOptions = _appSettingsUtil.BindAppSettings<AccessTokenOptions>(appSettingsSectionKey);
                var securityKeyHelper = new SecurityKeyHelper();
                var securityKey = securityKeyHelper.CreateSecurityKey(accessTokenOptions.SecurityKey);
                var signingCredentialsHelper = new SigningCredentialsHelper();
                var signingCredentials = signingCredentialsHelper.CreateSigningCredentials(securityKey);
                var claimList = new List<Claim>();
                claimList.AddUserNameIdentifier(user.Guid);
                claimList.AddUserName(user.UserName);
                if (user.IdentityRoles != null && user.IdentityRoles.Count > 0)
                {
                    claimList.AddRoleNames(user.IdentityRoles.Select(e => e.Name).ToArray());
                }
                if (user.IdentityClaims != null && user.IdentityClaims.Count > 0)
                {
                    claimList.AddClaims(user.IdentityClaims);
                }
                var expiration = DateTimeUtil.AddTimeToDate(DateTime.Now, 0, accessTokenOptions.AccessTokenExpirationMinutes);
                var jwtSecurityToken = new JwtSecurityToken(accessTokenOptions.Issuer, accessTokenOptions.Audience, claimList,
                    DateTime.Now, expiration, signingCredentials);
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
                var accessToken = new AccessToken()
                {
                    Token = token,
                    Expiration = expiration
                };
                return new SuccessResult<AccessToken>(AccessTokenUtilConfig.AccessTokenCreatedMessage, accessToken);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<AccessToken>(exc, ShowException);
            }
        }
    }
}
