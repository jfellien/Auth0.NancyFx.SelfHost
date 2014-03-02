using System.Configuration;
using Nancy;
using Nancy.Security;

namespace Auth0.Nancy.SelfHosted
{
    internal static class ModuleExtensions
    {
        private static readonly Auth0.Client Auth0Client = new Auth0.Client(
            ConfigurationManager.AppSettings["auth0:ClientId"],
            ConfigurationManager.AppSettings["auth0:ClientSecret"],
            ConfigurationManager.AppSettings["auth0:Domain"]);

        public static void RequiresAuthentication(this NancyModule module)
        {
            module.Before.AddItemToEndOfPipeline(Auth0Authentication.AuthenticateSession);
        }

        public static IResponseFormatter AuthenticateThisSession(this NancyModule module)
        {
            var code = (string) module.Request.Query["code"];

            var token = Auth0Client.ExchangeAuthorizationCodePerAccessToken(code,
                ConfigurationManager.AppSettings["auth0:CallbackUrl"]);

            var userInfo = Auth0Client.GetUserInfo(token);

            var user = new Auth0User
            {
                AccessToken = token.AccessToken,
                UserToken = token.IdToken,
                UserId = userInfo.UserId,
                Name = userInfo.Name,
                Nickname = userInfo.Nickname,
                GravatarUrl = userInfo.Picture,
                Email = userInfo.Email
            };

            Auth0Authentication.CreateAuthenticationSessionFor(user, module.Context.Request.Session);

            return module.Response;
        }

        public static IResponseFormatter RemoveAuthenticationFromThisSession(this NancyModule module)
        {
            var userInstance = module.Context.CurrentUser.ToUserModel();
            Auth0Authentication.RemoveAuthenticationFor(userInstance, module.Session);

            return module.Response;
        }

        public static bool SessionIsAuthenticated(this NancyModule module)
        {
            return module.Context.CurrentUser.IsAuthenticated();
        }

        public static Response ThenRedirectTo(this IResponseFormatter response, string viewName)
        {
            return response.AsRedirect(viewName);
        }
    }
}