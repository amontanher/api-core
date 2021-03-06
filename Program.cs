using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;

namespace API.Simple
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog(configureLogger: (context, configuration) =>
             {
                 configuration.Enrich.FromLogContext()
                 .Enrich.WithMachineName()
                 .WriteTo.Console()
                 .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(
                     node: new Uri(context.Configuration["ElasticConfiguration:Uri"]))
                 {
                     IndexFormat = "logstash",
                     AutoRegisterTemplate = true,
                     NumberOfShards = 2,
                     NumberOfReplicas = 1
                 })
                 .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
                 .ReadFrom.Configuration(context.Configuration);
             })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
