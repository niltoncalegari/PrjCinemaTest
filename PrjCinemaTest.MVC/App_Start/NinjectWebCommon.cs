using PrjCinema.Data.Repositories;
using PrjCinema.Data.Repositories.ContextFactory;
using PrjCinema.Domain.Interfaces.Repository;
using PrjCinema.Domain.Interfaces.Service;
using PrjCinema.Service.Service;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PrjCinema.MVC.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PrjCinema.MVC.App_Start.NinjectWebCommon), "Stop")]

namespace PrjCinema.MVC.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using System.Web.Mvc;
    using Ninject.Web.Mvc;

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
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {

            kernel.Bind<IUsuarioRepository>().To<UsuarioRepository>();
            kernel.Bind<IEnderecoRepository>().To<EnderecoRepository>();
            kernel.Bind<IFilmeRepository>().To<FilmeRepository>();
            kernel.Bind<ISerieRepository>().To<SerieRepository>();
            kernel.Bind<IAtorRepository>().To<AtorRepository>();
            kernel.Bind<IOperacaoRepository>().To<OperacaoRepository>();
            kernel.Bind<IGrupoAcessoRepository>().To<GrupoAcessoRepository>();
            kernel.Bind<IPermissaoRepository>().To<PermissaoRepository>();
            kernel.Bind<ITelaRepository>().To<TelaRepository>();
            kernel.Bind(typeof(IRepositoryBase<>)).To(typeof(RepositoryBase<>));
            kernel.Bind<IContextFactory>().To<ContextFactory>().InSingletonScope();

            kernel.Bind<IUsuarioService>().To<UsuarioService>();
            kernel.Bind<IEnderecoService>().To<EnderecoService>();
            kernel.Bind<IFilmeService>().To<FilmeService>();
            kernel.Bind<IOperacaoService>().To<OperacaoService>();
            kernel.Bind<ISerieService>().To<SerieService>();
            kernel.Bind<IAtorService>().To<AtorService>();
            kernel.Bind<ITelaService>().To<TelaService>();
            kernel.Bind<IGrupoAcessoService>().To<GrupoAcessoService>();
            kernel.Bind<IPermissaoService>().To<PermissaoService>();

            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));
        }
    }
}
