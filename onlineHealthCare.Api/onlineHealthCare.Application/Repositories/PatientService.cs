using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using onlineHealthCare.Application.Dtos;
using onlineHealthCare.Database;
using onlineHealthCare.Domain.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace onlineHealthCare.Application.Repositories
{
    public class PatientService : IPatientService
    {

        public onlineHealthCareDbContext Context { get; set; }
       
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _IConfiguration;
        public PatientService(UserManager<ApplicationUser> userManager, IConfiguration IConfiguration, onlineHealthCareDbContext context)
        {
            _IConfiguration = IConfiguration;
            _userManager = userManager;
            Context = context;
        }
        public async Task<AuthModel> Register(RegisterModel Data)
        {
            using var transaction = await Context.Database.BeginTransactionAsync();
            try
            {
                
                //TransactionManager.ImplicitDistributedTransactions = true;
                //TransactionInterop.GetTransmitterPropagationToken(Transaction.Current);
                var res = await RegisterAsync(Data);
                if (res.Message != null)
                {
                    throw new Exception(res.Message);
                }
                
                transaction.Dispose();
                return new AuthModel
                {
                    //Username = res.Username,
                    //Email = res.Email,
                    //Roles = res.Roles,
                    IsAuthenticated = res.IsAuthenticated,
                    Id=res.Id,
                    Token = res.Token,
                    ExpiresOn = res.ExpiresOn
                };
            }
            catch (Exception ex)
            {
                transaction.RollbackAsync();
                return new AuthModel { Message = ex.Message };
            }
        }

       

        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            model.Name = model.Name.Replace(" ", "-");
            if (await _userManager.FindByEmailAsync(model.EmailAddress) is not null)
                return new AuthModel { Message = "Email is already registered!" };

            if (await _userManager.FindByNameAsync(model.Name) is not null)
                return new AuthModel { Message = "Username is already registered!" };

            var user = new ApplicationUser
            {
                UserName=model.Name,
                Name = model.Name,
                Email = model.EmailAddress,
                EmailAddress=model.EmailAddress,
                PhoneNumber = model.PhoneNumber,
            };
                
                
                
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return new AuthModel { Message = errors };
            }

            await _userManager.AddToRoleAsync(user, "Patient");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Id = user.Id,
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "Patient" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.Name
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
     new Claim(JwtRegisteredClaimNames.Sub, user.Name),
     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
     new Claim(JwtRegisteredClaimNames.Email, user.Email),
     new Claim("uid", user.Id)
 }
            .Union(userClaims)
            .Union(roleClaims);

            var Key = _IConfiguration["JWT:key"];
            var issuer = _IConfiguration["JWT:issuer"];
            var audience = _IConfiguration["JWT:Audience"];
            var dur = _IConfiguration["JWT:DurationInDays"];
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(double.Parse(dur)),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
    }
}
