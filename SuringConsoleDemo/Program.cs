﻿using Autofac;
using Surging.Core.Caching.Configurations;
using Surging.Core.Codec.MessagePack;
using Surging.Core.Consul;
using Surging.Core.Consul.Configurations;
using Surging.Core.CPlatform;
using Surging.Core.CPlatform.Runtime.Client.Address.Resolvers.Implementation.Selectors.Implementation;
using Surging.Core.CPlatform.Support;
using Surging.Core.CPlatform.Utilities;
using Surging.Core.DotNetty;
using Surging.Core.EventBusRabbitMQ;
using Surging.Core.EventBusRabbitMQ.Configurations;
using Surging.Core.Log4net;
using Surging.Core.ProxyGenerator;
using Surging.Core.ServiceHosting.Internal.Implementation;
using System;

namespace SuringConsoleDemo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var host = new ServiceHostBuilder()
                 .RegisterServices(builder =>
                 {
                     builder.AddMicroService(option =>
                     {
                         option.AddServiceRuntime();//
                         option.AddRelateService();//添加支持服务代理远程调用
                         option.AddConfigurationWatch();//添加同步更新配置文件的监听处理
                                                        // option.UseZooKeeperManager(new ConfigInfo("127.0.0.1:2181")); //使用Zookeeper管理
                         option.UseConsulManager(new ConfigInfo("127.0.0.1:8500"));//使用Consul管理
                         option.UseDotNettyTransport();//使用Netty传输
                         option.UseRabbitMQTransport();//使用rabbitmq 传输
                         option.AddRabbitMQAdapt();//基于rabbitmq的消费的服务适配
                                                   //  option.UseProtoBufferCodec();//基于protobuf序列化
                         option.UseMessagePackCodec();//基于MessagePack序列化
                         builder.Register(p => new CPlatformContainer(ServiceLocator.Current));//初始化注入容器
                     });
                 })
                 .SubscribeAt()     //消息订阅
                                    //.UseServer("127.0.0.1", 98)
                                    //.UseServer("127.0.0.1", 98，“true”) //自动生成Token
                                    //.UseServer("127.0.0.1", 98，“123456789”) //固定密码Token
                 .UseServer(options =>
                 {
                     options.Ip = "127.0.0.1";
                     options.Port = 98;
                     //options.IpEndpoint = new IPEndPoint(IPAddress.Any, 98);
                     //options.Ip = "0.0.0.0";
                     options.ExecutionTimeoutInMilliseconds = 30000; //执行超时时间
                     options.Strategy = (int)StrategyType.Failover; //容错策略使用故障切换
                     options.RequestCacheEnabled = true; //开启缓存（只有通过接口代理远程调用，才能启用缓存）
                     options.Injection = "return null"; //注入方式
                     options.InjectionNamespaces = new string[] { "Surging.IModuleServices.Common" }; //脚本注入使用的命名空间
                     options.BreakeErrorThresholdPercentage = 50;  //错误率达到多少开启熔断保护
                     options.BreakeSleepWindowInMilliseconds = 60000; //熔断多少毫秒后去尝试请求
                     options.BreakerForceClosed = false;   //是否强制关闭熔断
                     options.BreakerRequestVolumeThreshold = 20;//10秒钟内至少多少请求失败，熔断器才发挥起作用
                     options.MaxConcurrentRequests = 100000;//支持最大并发
                     options.ShuntStrategy = AddressSelectorMode.Polling; //使用轮询负载分流策略
                     options.NotRelatedAssemblyFiles = "Centa.Agency.Application.DTO\\w*|StackExchange.Redis\\w*"; //排除无需依赖注册
                 })
               .UseLog4net("Configs/log4net.config") //使用log4net记录日志
                                                     //.UseNLog(LogLevel.Error, "Configs/NLog.config")// 使用NLog 记录日志
                                                     //.UseLog4net(LogLevel.Error) //使用log4net记录日志
                                                     //.UseLog4net()  //使用log4net记录日志
               .Configure(build =>
               build.AddEventBusFile("eventBusSettings.json", optional: false))//使用eventBusSettings.json文件进行配置
               .Configure(build =>
                build.AddCacheFile("cacheSettings.json", optional: false))//使用cacheSettings.json文件进行配置
               .UseProxy() //使用Proxy
                           //.UseStartup<Startup>()
               .Build();

            using (host.Run())
            {
                Console.WriteLine($"服务端启动成功，{DateTime.Now}。");
            }
        }
    }
}