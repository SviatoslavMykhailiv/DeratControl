using Application.Common.Interfaces;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services {
  public class AuthOptions : IOptions<AuthOptions> {
    public int LifeTime { get; set; }
    public string SecurityKey { get; set; }
    public string QrCodeSymmetricEncryptionKey { get; set; }
    public AuthOptions Value => this;
  }

  public class AuthService : IAuthService {
    public const string USER_ID_CLAIM = "derat-user-id";
    public const string USER_ROLE_CLAIM = "derat-user-role";
    public const string USER_NAME_CLAIM = "derat-user-name";

    private readonly UserManager<ApplicationUser> userManager;
    private readonly SignInManager<ApplicationUser> signInManager;
    private readonly IOptions<AuthOptions> options;

    public AuthService(
      UserManager<ApplicationUser> userManager, 
      SignInManager<ApplicationUser> signInManager, 
      IOptions<AuthOptions> options) {
      this.userManager = userManager;
      this.signInManager = signInManager;
      this.options = options;
    }

    public async Task<string> SignIn(string userName, string password) {
      var result = await signInManager.PasswordSignInAsync(userName, password, false, false);

      if (result.Succeeded == false)
        return null;

      var user = await userManager.FindByNameAsync(userName);

      var claims = new List<Claim> {
        new Claim(USER_ID_CLAIM, user.Id.ToString()),
        new Claim(USER_NAME_CLAIM, userName) };

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
