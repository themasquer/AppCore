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
using System.Text.Json;

namespace AppCore.Business.Abstracts.Utils.Security.Identity
{
    public abstract class JwtUtilBase
    {
        private readonly AppSettingsUtilBase _appSettingsUtil;

        public bool ShowException { get; set; } = false;

        protected JwtUtilBase(AppSettingsUtilBase appSettingsUtil)
        {
            _appSettingsUtil = appSettingsUtil;
        }

        public virtual Result<Jwt> CreateJwt(IdentityUserModel user, string appSettingsSectionKey = "JwtOptions")
        {
            try
            {
                var jwtOptions = _appSettingsUtil.BindAppSettings<JwtOptions>(appSettingsSectionKey);
                var securityKeyHelper = new SecurityKeyHelper();
                var securityKey = securityKeyHelper.CreateSecurityKey(jwtOptions.SecurityKey);
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
                var expiration = DateTimeUtil.AddTimeToDate(DateTime.Now, 0, jwtOptions.JwtExpirationMinutes);
                var jwtSecurityToken = new JwtSecurityToken(jwtOptions.Issuer, jwtOptions.Audience, claimList,
                    DateTime.Now, expiration, signingCredentials);
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var token = jwtSecurityTokenHandler.WriteToken(jwtSecurityToken);
                var jwt = new Jwt()
                {
                    Token = token,
                    Expiration = expiration
                };
                return new SuccessResult<Jwt>(JwtUtilConfig.JwtCreatedMessage, jwt);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<Jwt>(exc, ShowException);
            }
        }

        public virtual Result<List<Claim>> GetClaimsFromJwt(string jwt)
        {
            try
            {
                var claims = new List<Claim>();
                var payload = jwt.Split('.')[1];
                switch (payload.Length % 4)
                {
                    case 2:
                        payload += "==";
                        break;
                    case 3:
                        payload += "=";
                        break;
                }
                var jsonBytes = Convert.FromBase64String(payload);
                var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                keyValuePairs.TryGetValue(ClaimTypes.Role, out object roles);
                if (roles != null)
                {
                    if (roles.ToString().Trim().StartsWith("["))
                    {
                        var parsedRoles = JsonSerializer.Deserialize<string[]>(roles.ToString());
                        foreach (var parsedRole in parsedRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, parsedRole));
                        }
                    }
                    else
                    {
                        claims.Add(new Claim(ClaimTypes.Role, roles.ToString()));
                    }
                    keyValuePairs.Remove(ClaimTypes.Role);
                }
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString())));
                return new SuccessResult<List<Claim>>(JwtUtilConfig.ClaimsRetrievedFromJwtMessage, claims);
            }
            catch (Exception exc)
            {
                return new ExceptionResult<List<Claim>>(exc, ShowException);
            }
        }
    }
}
