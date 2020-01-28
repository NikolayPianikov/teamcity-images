using System;
using System.Collections.Generic;
using Docker.DotNet;
using IoC;
using JetBrains.TeamCity.ServiceMessages.Write;
using JetBrains.TeamCity.ServiceMessages.Write.Special;
using JetBrains.TeamCity.ServiceMessages.Write.Special.Impl.Updater;
using static IoC.Lifetime;
// ReSharper disable InconsistentNaming

namespace TeamCity.Docker
{
    internal class IoCConfiguration: IConfiguration
    {
        public IEnumerable<IToken> Apply(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            yield return container
                .Bind<IFileSystem>().As(Singleton).To<FileSystem>()
                .Bind<IEnvironment>().As(Singleton).To<Environment>()
                .Bind<ILogger>().As(Singleton).Tag("Console").To<ConsoleLogger>()
                .Bind<ILogger>().As(Singleton).Tag("TeamCity").To<TeamCityLogger>()
                .Bind<ILogger>().As(Singleton).Tag("Common").To<Logger>(ctx => new Logger(ctx.Container.Inject<IEnvironment>(), ctx.Container.Inject<ILogger>("Console"), ctx.Container.Inject<ILogger>("TeamCity")))
                .Bind<IDockerClientFactory>().As(Singleton).To<DockerClientFactory>()
                .Bind<IDockerClient>().To(ctx => ctx.Container.Inject<IDockerClientFactory>().Create().Result)
                .Bind<IStreamService>().As(Singleton).To<StreamService>()
                .Bind<IMessageLogger>().As(Singleton).To<MessageLogger>()
                .Bind<IDockerConverter>().As(Singleton).To<DockerConverter>()
                .Bind<IGate<TDisposable>, ILogger, IDisposable>().As(Singleton).To<Gate<TDisposable>>(ctx => new Gate<TDisposable>(ctx.Container.Inject<IOptions>(), ctx.Container.Inject<ILogger>("Common"), ctx.Container.Inject<Func<TDisposable>>()))
                // TeamCity messages
                .Bind<IServiceMessageFormatter>().As(Singleton).To<ServiceMessageFormatter>()
                .Bind<IFlowIdGenerator>().As(Singleton).To<FlowIdGenerator>()
                .Bind<IServiceMessageUpdater>().As(Singleton).To(ctx => new TimestampUpdater(() => DateTime.Now) )
                .Bind<ITeamCityServiceMessages>().As(Singleton).To<TeamCityServiceMessages>()
                .Bind<IPathService>().As(Singleton).To<PathService>()
                .Bind<ITeamCityWriter, ITeamCityMessageWriter>().As(Singleton).To(ctx => ctx.Container.Inject<ITeamCityServiceMessages>().CreateWriter());

            // Generate command
            yield return container
                .Bind<ICommand<Generate.IOptions>>().As(Singleton).To<Generate.Command>()
                .Bind<Generate.IResources>().As(Singleton).To<Generate.Resources>()
                .Bind<Generate.IDockerFileContentParser>().As(Singleton).To<Generate.DockerFileContentParser>()
                .Bind<Generate.IDockerMetadataProvider>().As(Singleton).To<Generate.DockerMetadataProvider>()
                .Bind<Generate.IDockerFileGenerator>().As(Singleton).To<Generate.DockerFileGenerator>()
                .Bind<Generate.IReadmeGenerator>().As(Singleton).To<Generate.ReadmeGenerator>()
                .Bind<Generate.IDockerFileConfigurationExplorer>().As(Singleton).To<Generate.DockerFileConfigurationExplorer>()
                .Bind<Generate.IGenerator>().Tag("").As(Singleton).To<Generate.GeneratorFromConfigurations>();

            // Build command
            yield return container
                .Bind<ICommand<Build.IOptions>>().As(Singleton).To<Build.Command>()
                .Bind<Build.IImageBuilder>().As(Singleton).To<Build.ImageBuilder>()
                .Bind<Build.IContextFactory>().As(Singleton).To<Build.ContextFactory>();

            // Push command
            yield return container
                .Bind<ICommand<Push.IOptions>>().As(Singleton).To<Push.Command>()
                .Bind<Push.IImageFetcher>().As(Singleton).To<Push.ImageFetcher>()
                .Bind<Push.IImagePublisher>().As(Singleton).To<Push.ImagePublisher>()
                .Bind<Push.IImageCleaner>().As(Singleton).To<Push.ImageCleaner>();
        }

        [GenericTypeArgument]
        private abstract class TDisposable: IDisposable
        {
            public abstract void Dispose();
        }
    }
}
