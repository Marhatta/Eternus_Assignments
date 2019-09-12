using Authorization.Models;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace Authorization.Providers
{
    public class OAuthCustomTokenProvider : OAuthAuthorizationServerProvider{


        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
           context.Validated();

        }

        public override  async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //OWIN - Open Web interface for .NET
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            ProductsEntities1 pd = new ProductsEntities1();
            var user = pd.Users.ToList();
            var id="";
            var role="";
            var check = 0;

            foreach(var x in user)
            {
                if (x.Name.Equals(context.UserName) && x.Password.Equals(context.Password))
                {
                    id = x.Id.ToString();
                    role = x.Role;
                    check = 1;
                    break;

                }

            }
            if(check==0)
            {
                    context.SetError("Invalid grant", "username or password is  incorrect");
                    return;
            }

           
            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("Id", id));
            identity.AddClaim(new Claim(ClaimTypes.Role,role));

            context.Validated(identity);


        
        }
    }
    
    
}