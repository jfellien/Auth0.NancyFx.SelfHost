namespace Auth0.Nancy.SelfHosted
{
    public class AuthenticationConfig
    {
        public string RedirectOnLoginFailed { get; set; }
        public string CookieName { get; set; }
        public string UserIdentifier { get; set; }
    }
}