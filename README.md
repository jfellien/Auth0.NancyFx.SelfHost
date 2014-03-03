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


###How to use it in your project?

After you enabled the `Auth0Authentication` you are able to block all unauthenticated requests by unsing


    public class SecurePage : NancyModule
    {
        public SecurePage()
        {
            this.RequiresAuthentication(); //<- This is a new implemetation of default extension
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
    
###Implementig with Auth0

Look into the sample. The implementation with Auth0 is like any other implementation. 

First insert your Auth0 settings into app.config (like here [ASP.Net Auth0 Documentation](https://github.com/auth0/docs/blob/master/docs/aspnet-tutorial.md))

    <appSettings>
        <!-- Auth0 configuration -->
        <add key="auth0:ClientId" value="YOUR-CLIENTID" />
        <add key="auth0:ClientSecret" value="YOUR-CLINET-SECTRET" />
        <add key="auth0:Domain" value="YOUR-DOMAIN" />
        <add key="auth0:CallbackUrl" value="http://localhost:3579/login-callback" />
    </appSettings>
    
and put the Widget Code into your login website

    <div id="root" style="width: 400px; margin: 40px auto; padding: 10px; border-style: dashed; border-width: 1px;">
        embeded area
    </div>
    <script src="https://cdn.auth0.com/w2/auth0-widget-2.4.6.min.js"></script>
    <script>
        var widget = new Auth0Widget({
            domain:         'YOUR_DOMAIN',
            clientID:       'YOUR-CLIENTID',
            callbackURL:    'http://localhost:3579/login-callback'
            });
            
        widget.signin({ container: 'root', chrome: true });
    </script>
    

That's it
