using IData;
using IRepository;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Ninject;
using StudentRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace InternWebApi
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        private IUserRepository _repo;
        public MyAuthorizationServerProvider(IUserRepository repo)
        {
            _repo = repo;
        }
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
               var user = _repo.ValidateUser(context.UserName, context.Password);   // UserDTO user = UserDAL.ValidateUser(username, password);
                if (user != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid,user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.UserName),
                        new Claim(ClaimTypes.Email, user.UserEmail),
                        new Claim(ClaimTypes.Role, user.UserRole)
                    };
                    //foreach (var role in user.roleDto)
                    //    claims.Add(new Claim(ClaimTypes.Role, role.Title));
                    ////   ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
                    var props = new AuthenticationProperties(new Dictionary<string, string>
                    {
                      {"Name",user.UserName},
                      {"Email",user.UserEmail },
                      {"UserId",user.Id.ToString()},
                    });
                    ClaimsIdentity oAutIdentity = new ClaimsIdentity(claims, Startup.OAuthOptions.AuthenticationType);
                    context.Validated(new AuthenticationTicket(oAutIdentity, props));
                }
                else
                {
                    context.SetError("invalid_grant", "Error");
                }
            });
        }
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
            {
                context.Validated();
            }
            return Task.FromResult<object>(null);
        }
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}