using Auth0.Nancy.SelfHost;
using Nancy;

namespace auth0_nancyfx_sample
{
    public class Authentication
        : NancyModule
    {
        public Authentication()
        {
            Get["/login"] = o =>
            {
                if (this.SessionIsAuthenticated())
                    return Response.AsRedirect("securepage");

                return View["login"];
            };

            Get["/login-callback"] = o => this
                .AuthenticateThisSession()
                .ThenRedirectTo("securepage");

            Get["/logout"] = o => this
                .RemoveAuthenticationFromThisSession()
                .ThenRedirectTo("index");
        }
    }
}