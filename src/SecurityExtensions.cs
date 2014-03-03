using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.Security;

namespace Auth0.Nancy.SelfHost
{
    public static class SecurityExtensions
    {
        public static Auth0User ToUserModel(this IUserIdentity identity)
        {
            return new Auth0User
            {
                UserId = identity.Claims.Get("user_id"),
                Name = identity.UserName,
                Nickname = identity.Claims.Get("nickname"),
                GravatarUrl = identity.Claims.Get("picture"),
                Email = identity.Claims.Get("email"),
            };
        }

        public static string Get(this IEnumerable<string> claims, string lookingfor)
        {
            var item = claims.ToList().SingleOrDefault(x => x.Contains(lookingfor));

            return String.IsNullOrEmpty(item)
                ? String.Empty
                : item.Split(':')[1];
        }
    }
}