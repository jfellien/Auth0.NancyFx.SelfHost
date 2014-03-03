namespace auth0_nancyfx_sample
{
    using Nancy;

    public class Index
        : NancyModule
    {
        public Index()
        {
            Get["/"] = o => View["index"];

            Get["/index"] = o => View["index"];
        }
    }
}