using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;

namespace ApiAzureExamenAGC.Helpers
{
    public class HelperActionServicesOAuth
    {
        private SecretClient secretclient;
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        //esto es por si lo pide sin key vaults :)))
        //public HelperActionServicesOAuth(IConfiguration configuration)
        //{
        //    this.Issuer = configuration.GetValue<string>("ApiOAuthToken:Issuer");
        //    this.Audience = configuration.GetValue<string>("ApiOAuthToken:Audience");
        //    this.SecretKey = configuration.GetValue<string>("ApiOAuthToken:SecretKey");
        //}
        public HelperActionServicesOAuth(IConfiguration configuration, SecretClient client)
        {
            this.secretclient = client;
            KeyVaultSecret secretIssuer = this.secretclient.GetSecret("Issuer");
            this.Issuer = secretIssuer.Value;
            KeyVaultSecret secretAudience = this.secretclient.GetSecret("Audience");
            this.Audience = secretAudience.Value;
            KeyVaultSecret secretKey = this.secretclient.GetSecret("SecretKey");
            this.SecretKey = secretKey.Value;
        }

        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }

        public Action<JwtBearerOptions> GetJwtBearerOptions()
        {
            Action<JwtBearerOptions> options = new Action<JwtBearerOptions>(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issuer,
                    ValidAudience = this.Audience,
                    IssuerSigningKey = this.GetKeyToken()
                };
            });
            return options;
        }

        public Action<AuthenticationOptions> GetAuthenticationSchema()
        {
            Action<AuthenticationOptions> options = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return options;
        }
    }
}

