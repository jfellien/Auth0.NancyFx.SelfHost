using Auth0.Nancy.SelfHost;
using Nancy;

namespace auth0_nancyfx_sample
{
    public class SecurePage : NancyModule
    {
        public SecurePage()
        {
            this.RequiresAuthentication();

            Get["/securepage"] = o =>
                View["securepage",
                    Context.CurrentUser.ToUserModel()];
        }
    }
}