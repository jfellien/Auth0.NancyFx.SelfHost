using Auth0.Nancy.SelfHost;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace auth0_nancyfx_sample
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Auth0Authentication.Enable(pipelines, new AuthenticationConfig
            {
                RedirectOnLoginFailed = "login",
                CookieName = "_auth0_userid",
                UserIdentifier = "userid"
            });
        }
    }
}