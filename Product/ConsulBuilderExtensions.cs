using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;

namespace Product
{
    public static class ConsulBuilderExtensions

    {
        // 服务注册

        public static IApplicationBuilder RegisterConsul(this IApplicationBuilder app, IHostApplicationLifetime lifetime, IConfiguration Configuration)
        {
            var ip = Configuration["ip"];
            var port = Convert.ToInt32(Configuration["port"]);
            var name = Configuration["name"];
            var slice = Configuration["slice"];

            var consulurl = Configuration["consulurl"];
            var consulClient = new ConsulClient(x => x.Address = new Uri(consulurl));

            var httpCheck = new AgentServiceCheck()

            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),//服务启动多久后注册

                Interval = TimeSpan.FromSeconds(10),//健康检查时间间隔，或者称为心跳间隔

                HTTP = $"http://{ip}:{port}/api/health",//健康检查地址

                Timeout = TimeSpan.FromSeconds(5)
            };

            // Register service with consul

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },

                ID = $"{name}_{slice}",

                Name = name,

                Address = ip,

                Port = port,

                Tags = new[] { $"urlprefix-/{name}" }//添加 urlprefix-/servicename 格式的 tag 标签，以便 Fabio 识别
            };

            consulClient.Agent.ServiceRegister(registration).Wait();//服务启动时注册，内部实现其实就是使用 Consul API 进行注册（HttpClient发起）

            lifetime.ApplicationStopping.Register(() =>

            {
                consulClient.Agent.ServiceDeregister(registration.ID).Wait();//服务停止时取消注册
            });

            return app;
        }
    }
}