using System;
using System.Threading.Tasks;
using CommandLine;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AccessToDisposedClosure

namespace TeamCity.Docker
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
            (int)await Parser.Default.ParseArguments<Generate.Options, Build.Options, Push.Options>(args)
                .MapResult(
                    (Generate.Options options) => Run<Generate.IOptions>(options),
                    (Build.Options options) => Run<Build.IOptions>(options),
                    (Push.Options options) => Run<Push.IOptions>(options),
                    error => Task.FromResult(Result.Error));

        private static async Task<Result> Run<TOptions>([NotNull] TOptions options)
            where TOptions: IOptions
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (var container = Container
                .Create()
                .Using<IoCConfiguration>()
                .Bind<TOptions, IOptions>().As(Lifetime.Singleton).To(ctx => options)
                .Container)
            {
                try
                {
                    return await container.Resolve<ICommand<TOptions>>().Run();
                }
                catch (Exception ex)
                {
                    container.Resolve<ILogger>().Log(ex.Message, Result.Error);
                    return Result.Error;
                }
            }
        }
    }
}
