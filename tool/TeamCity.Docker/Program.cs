using System;
using CommandLine;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable AccessToDisposedClosure

namespace TeamCity.Docker
{
    public class Program
    {
        public static int Main(string[] args) =>
            (int)Parser.Default.ParseArguments<Generate.Options, Build.Options, Push.Options>(args)
                .MapResult(
                    (Generate.Options options) => Run<Generate.IOptions>(options),
                    (Build.Options options) => Run<Build.IOptions>(options),
                    (Push.Options options) => Run<Push.IOptions>(options),
                    error => Result.Error);

        private static Result Run<TOptions>([NotNull] TOptions options)
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
                    var runTask = container.Resolve<ICommand<TOptions>>().Run();
                    runTask.Wait();
                    return runTask.Result;
                }
                catch (Exception ex)
                {
                    container.Resolve<ILogger>().Log(ex);
                    return Result.Error;
                }
            }
        }
    }
}
