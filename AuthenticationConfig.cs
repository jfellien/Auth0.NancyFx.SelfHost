namespace Auth0.Nancy.SelfHost
{
    public class AuthenticationConfig
    {
        public string RedirectOnLoginFailed { get; set; }
        public string CookieName { get; set; }
        public string UserIdentifier { get; set; }
    }
}