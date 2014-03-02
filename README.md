Auth0.NancyFx.SelfHost
======================

Auth0 Authentication Library for NancyFx SelfHosted Applications

Ready to use in NancyFx style: super-duper-happy-path implementation. Install the package by NuGet

    PM> Install-Package Auth0.NancyFx.SelfHost
   
In your Nancy self hosted application add to your BootStrapper:


    protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
    {
      ...
            
      Auth0Authentication.Enable(pipelines, new AuthenticationConfig
      {
        RedirectOnLoginFailed = "login",
        CookieName = "_auth0_userid",
        UserIdentifier = "userid"
      });
            
      ...
    }
    
The `AuthenticationConfig` gives you more control. Use `RedirectOnLoginFailed` to define the view shoud shown an unauthenticated user. The `CookieName` allows you to set your own cookie name. And last but not least you can set the identifier to identify the requests. This value of your user instance will save in the cookie. At this time you can set as identifier all fields of the Auth0 user token witch I implemented.

 * userid,
 * email,
 * nickname,
 * gravatarurl

**Important Hint:** Auth0.Nancy.SelfHost enabled in background the `CookieBasedSessions` setting. If you use in your App this setting too, switch it of now.


###How to use it in my App?

After you enabled the `Auth0Authentication` you are able to block all unauthenticated requests by unsing


    public class SecurePage : NancyModule
    {
        public SecurePage()
        {
            this.RequiresAuthentication(); //New implemetation of default extension
            Get["/securepage"] = o => View["securepage"];
        }
    }
    
    
But this it's not all. You need for Auth0 a callback route, and a way log in and log out too. Take look for my implementation.

    public class Authentication : NancyModule
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
