using System.Net.Http;
using Signatory.Controllers;
using Signatory.Data;
using Signatory.Service;
using SimpleAuthentication.Core;
using SimpleAuthentication.Mvc;
using SimpleAuthentication.Mvc.Caching;


[assembly: WebActivator.PreApplicationStartMethod(typeof(Signatory.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(Signatory.App_Start.NinjectWebCommon), "Stop")]

namespace Signatory.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IAuthenticationCallbackProvider>().To<AuthenticationCallbackController>();
            kernel.Bind<IGitHubService>().To<GitHubService>();
            kernel.Bind<ICache>().To<CookieCache>();
            kernel.Bind<DataContext>().ToSelf().InRequestScope();
            kernel.Bind<HttpClient>().ToMethod(ctx =>
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Signatory");
                return client;
            }).InRequestScope();
        }        
    }
}
