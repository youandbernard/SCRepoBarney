using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Runtime.Security;
using CaseMix.Attributes;
using CaseMix.Authentication.JwtBearer;
using CaseMix.Authorization;
using CaseMix.Authorization.Users;
using CaseMix.Controllers;
using CaseMix.Models.TokenAuth;
using CaseMix.MultiTenancy;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CaseMix.Web.Host.Controllers
{
    [Route("")]
    [ApiController]
    [ThirdPartyApi]
    public class SecurityController : CaseMixControllerBase
    {
        private readonly LogInManager _logInManager;
        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly TokenAuthConfiguration _configuration;

        public SecurityController(
            LogInManager logInManager,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            TokenAuthConfiguration configuration
            )
        {
            _logInManager = logInManager;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
        }


        [HttpGet("getAuthToken")]
        [ProducesResponseType(typeof(AuthenticateResultModel), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json")]
        public async Task<ActionResult<AuthenticateResultModel>> GetAuthToken([FromQuery][Required] string clientId, [FromQuery][Required] string password)
        {
            var loginResult = await GetLoginResultAsync(
                clientId,
                password,
                null
            );

            var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

            return new AuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                UserId = loginResult.User.Id
            };
        }

        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string usernameOrEmailAddress, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(usernameOrEmailAddress, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, usernameOrEmailAddress, tenancyName);
            }
        }

        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        private static List<Claim> CreateJwtClaims(ClaimsIdentity identity)
        {
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            // Specifically add the jti (random nonce), iat (issued timestamp), and sub (subject/user) claims.
            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            });

            return claims;
        }

        private string GetEncryptedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }
    }
}
