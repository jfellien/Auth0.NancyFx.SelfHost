using System.Collections.Generic;
using System.Linq;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Responses;
using Nancy.Security;
using Nancy.Session;

namespace Auth0.Nancy.SelfHost
{
    public class Auth0Authentication
    {
        private static AuthenticationConfig _config;
        private static Dictionary<string, Auth0User> _users;

        public static void Enable(IPipelines pipelines, AuthenticationConfig config)
        {
            _config = config;
            _users = new Dictionary<string, Auth0User>();

            pipelines.BeforeRequest.AddItemToStartOfPipeline(LoadCurrentUser);
            CookieBasedSessions.Enable(pipelines);
        }

        private static Response LoadCurrentUser(NancyContext context)
        {
            if (!context.ContainsCookie(_config.CookieName)) return null;

            var cookieValue = context.ValueOfCookie(_config.CookieName);
            context.CurrentUser = CurrentUserBy(cookieValue);

            return null;
        }

        private static IUserIdentity CurrentUserBy(string cookieValue)
        {
            return _users.Any(u => u.Key.Equals(cookieValue))
                ? _users[cookieValue]
                : null;
        }

        public static Response AuthenticateSession(NancyContext context)
        {
            return context.CurrentUser == null
                ? new RedirectResponse(_config.RedirectOnLoginFailed)
                : null;
        }

        public static void CreateAuthenticationSessionFor(Auth0User auth0User, ISession session)
        {
            var userId = auth0User.Claims.Get(_config.UserIdentifier);

            session[_config.CookieName] = userId;

            if (_users.ContainsKey(userId)) return;

            _users.Add(userId, auth0User);
        }

        public static void RemoveAuthenticationFor(Auth0User auth0User, ISession session)
        {
            var userId = auth0User.Claims.Get(_config.UserIdentifier);

            if (session.ToList().Any(x => x.Key.Equals(_config.CookieName)))
            {
                session.Delete(_config.CookieName);
            }

            _users.Remove(userId);
        }
    }
}