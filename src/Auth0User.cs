using System.Collections.Generic;
using Nancy.Security;

namespace Auth0.Nancy.SelfHost
{
    public class Auth0User : IUserIdentity
    {
        private IEnumerable<string> _claims;

        public string UserId { get; set; }
        public string Name { get; set; }
        public string Nickname { get; set; }
        public string GravatarUrl { get; set; }
        public string Email { get; set; }

        public string UserToken { get; set; }

        public string UserName
        {
            get { return Name; }
        }

        public IEnumerable<string> Claims
        {
            get
            {
                if (_claims != null) return _claims;

                _claims = new List<string>
                {
                    "userid:" + UserId,
                    "email:" + Email,
                    "nickname:" + Nickname,
                    "gravatarurl:" + GravatarUrl
                };

                return _claims;
            }
        }

        public string AccessToken { get; set; }
    }
}