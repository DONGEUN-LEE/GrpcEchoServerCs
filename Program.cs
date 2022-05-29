using System.Net;
using GrpcEchoServer;
using Microsoft.AspNetCore.Server.Kestrel.Core;

Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder =>
    {
        webBuilder.ConfigureKestrel(options =>
        {
            options.Listen(IPAddress.Any, 50051, listenOptions =>
            {
                listenOptions.Protocols = HttpProtocols.Http2;
            });
        });
        webBuilder.UseStartup<Startup>();
    })
    .Build().Run();