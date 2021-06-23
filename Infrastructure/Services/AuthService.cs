using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthOptions : IOptions<AuthOptions>
    {
        public int LifeTime { get; set; }
        public string SecurityKey { get; set; }
        public string QrCodeSymmetricEncryptionKey { get; set; }
        public AuthOptions Value => this;
    }

    public class AuthService : IAuthService
    {
        public const string USER_ID_CLAIM = "derat-user-id";
        public const string USER_ROLE_CLAIM = "derat-user-role";
        public const string USER_NAME_CLAIM = "derat-user-name";
        public const string FACILITY_ID_CLAIM = "derat-facility-id";
        public const string USER_FIRST_NAME_CLAIM = "derat-user-first-name";
        public const string USER_LAST_NAME_CLAIM = "derat-user-last-name";
        public const string PROVIDER_NAME_CLAIM = "derat-provider-name";
        public const string ENCRYPTION_KEY_CLAIM = "derat-encryption-key";

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IOptions<AuthOptions> options;

        public AuthService(
          UserManager<ApplicationUser> userManager,
          SignInManager<ApplicationUser> signInManager,
          IOptions<AuthOptions> options)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.options = options;
        }

        public async Task<string> SignIn(string userName, string password)
        {
            var result = await signInManager.PasswordSignInAsync(userName, password, false, false);

            if (result.Succeeded == false)
                throw new BadRequestException("Не вдалось знайти користувача.");

            var user = await userManager.FindByNameAsync(userName);
            var userRole = await userManager.GetRolesAsync(user);

            var claims = new List<Claim> {
                new Claim(USER_ID_CLAIM, user.Id.ToString()),
                new Claim(USER_ROLE_CLAIM, userRole.First()),
                new Claim(USER_NAME_CLAIM, userName),
                new Claim(USER_FIRST_NAME_CLAIM, user.FirstName),
                new Claim(USER_LAST_NAME_CLAIM, user.LastName),
                new Claim(ENCRYPTION_KEY_CLAIM, options.Value.QrCodeSymmetricEncryptionKey)};

            if (user.FacilityId.HasValue)
                claims.Add(new Claim(FACILITY_ID_CLAIM, user.FacilityId.Value.ToString()));

            if (string.IsNullOrEmpty(user.ProviderName) == false)
                claims.Add(new Claim(PROVIDER_NAME_CLAIM, user.ProviderName));

            var now = DateTime.Now;
            var expires = DateTime.Now.Add(TimeSpan.FromHours(options.Value.LifeTime));

            var jwt = new JwtSecurityToken(notBefore: now,
                                           claims: claims,
                                           expires: expires,
                                           signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(options.Value.SecurityKey)), SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
