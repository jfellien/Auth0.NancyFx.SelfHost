using System;
using Nancy;

namespace Auth0.Nancy.SelfHost
{
    public static class ContextExtensions
    {
        public static bool ContainsCookie(this NancyContext context, string cookieName)
        {
            var session = context.Request.Session;

            return session != null
                   && session[cookieName] != null
                   && !String.IsNullOrEmpty(session[cookieName].ToString());
        }

        public static string ValueOfCookie(this NancyContext context, string cookieName)
        {
            return context.Request.Session[cookieName] as String;
        }
    }
}