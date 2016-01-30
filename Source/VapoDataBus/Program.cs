using System;
using Autofac;
using Autofac.Extras.Quartz;
using Common.Logging;
using VapoDataBus.Jobs;
using Quartz;
using Quartz.Spi;
using Topshelf;
using Topshelf.Autofac;
using Topshelf.Quartz;
using Topshelf.ServiceConfigurators;
using VapoDataBus.Services;
using System.Configuration;
using VapoDataBus.Helper;

namespace VapoDataBus
{
    class Program
    {
        private static readonly ILog s_log = LogManager.GetLogger(typeof(Program));
        private static IContainer _container;

        static int Main(string[] args)
        {
            s_log.Info("Starting...");
            try
            {
                _container = ConfigureContainer(new ContainerBuilder()).Build();

                HostFactory.Run(conf =>
                {
                    conf.SetServiceName("VapoDataBus");
                    conf.SetDisplayName("Vaporize old DataBus folders");
                    conf.UseLog4Net();
                    conf.UseAutofacContainer(_container);

                    conf.Service<ServiceCore>(svc =>
                    {
                        svc.ConstructUsingAutofacContainer();
                        svc.WhenStarted(o => o.Start());
                        svc.WhenStopped(o =>
                        {
                            o.Stop();
                            _container.Dispose();
                        });
                        ConfigureBackgroundJobs(svc);
                    });
                });

                s_log.Info("Shutting down...");
                log4net.LogManager.Shutdown();
                return 0;
            }

            catch (Exception ex)
            {
                s_log.Fatal("Unhandled exception", ex);
                log4net.LogManager.Shutdown();
                return 1;
            }
        }

        private static void ConfigureBackgroundJobs(ServiceConfigurator<ServiceCore> svc)
        {
            svc.UsingQuartzJobFactory(() => _container.Resolve<IJobFactory>());

            var cron = ConfigurationManager.AppSettings.Get("CRON");

            svc.ScheduleQuartzJob(q =>
            {
                q.WithJob(JobBuilder.Create<VapoJob>()
                    .WithIdentity("Vaporize", "Maintenance")
                    .Build);
                q.AddTrigger(() => TriggerBuilder.Create()
                    .WithCronSchedule(cron).Build());
            });
        }

        internal static ContainerBuilder ConfigureContainer(ContainerBuilder cb)
        {
            cb.RegisterModule(new QuartzAutofacFactoryModule());
            cb.RegisterModule(new QuartzAutofacJobsModule(typeof(VapoJob).Assembly));

            RegisterComponents(cb);
            return cb;
        }

        internal static void RegisterComponents(ContainerBuilder cb)
        {
            // register Service instance
            cb.RegisterType<ServiceCore>().AsSelf();
            // register dependencies
            cb.RegisterType<ConfigHelper>().As<IConfigHelper>();
            cb.RegisterType<VapoService>().As<IVapo>();
        }
    }
}
