namespace CoreBankerApi.Application.Util
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;

    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly string _validUsername = "admin";  // Replace with your valid username
        private readonly string _validPassword = "password";  // Replace with your valid password

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
                                          UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));
            }

            var authorizationHeader = Request.Headers["Authorization"].ToString();
            var authHeader = authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase)
                                 ? authorizationHeader.Substring(6).Trim()
                                 : null;

            if (authHeader == null)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
            }

            try
            {
                var credentialBytes = Convert.FromBase64String(authHeader);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];

                if (username == _validUsername && password == _validPassword)
                {
                    var claims = new[] {
                    new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, username)
                };

                    var identity = new System.Security.Claims.ClaimsIdentity(claims, "Basic");
                    var principal = new System.Security.Claims.ClaimsPrincipal(identity);

                    var ticket = new AuthenticationTicket(principal, "Basic");

                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Username or Password"));
                }
            }
            catch (FormatException)
            {
                return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header Format"));
            }
        }
    }

}
