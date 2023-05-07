using System.Net;

namespace WebImageProxy
{
    class Program
    {
        static void Main(string[] args)
        {
            int port = 8187;
            string proxy = "";
            if (args.Length > 0)
            {
                int.TryParse(args[0], out port);
            }
            if (args.Length > 1)
            {
                proxy = args[1];
            }

            if (!string.IsNullOrEmpty(proxy))
            {
                WebRequest.DefaultWebProxy = new WebProxy(proxy);
            }

            var host = new WebHostBuilder()
                .UseKestrel()
                .UseUrls($"http://*:{port}")
                .Configure(app =>
                {
                    app.Run(async context =>
                    {
                        /* await context.Response.WriteAsync("123"); */
                        /* return; */
                        string? imageUrl = context.Request.Query["url"];
                        string? referer = context.Request.Query["referer"];

                        if (imageUrl is null)
                        {
                            await context.Response.WriteAsync("usage: /?url=<url>[&referer=<referer>]");
                            return;
                        }
                        var request = WebRequest.Create(imageUrl);
                        request.Method = "GET";
                        request.Headers["Referer"] = referer;
                        using (var response = request.GetResponse())
                        using (var stream = response.GetResponseStream())
                        {
                            context.Response.ContentType = "image/jpeg";
                            stream.CopyTo(context.Response.BodyWriter.AsStream());
                        }
                    });
                })
                .Build();

            host.Run();
        }
    }
}
