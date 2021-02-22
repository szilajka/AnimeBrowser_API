using AnimeBrowser.Common.Helpers;
using AnimeBrowser.Common.Models.ErrorModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Helpers
{
    public class TestBase
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .AddJsonFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "serilog.test.json"), optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        public static IServiceProvider SetupDI(Action<IServiceCollection> configure)
        {
            IServiceCollection services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            if (configure != null)
            {
                configure.Invoke(services);
            }

            return services.BuildServiceProvider();
        }

        public static async Task<IServiceProvider> SetupDI(Func<IServiceCollection, Task> configure)
        {
            IServiceCollection services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();

            if (configure != null)
            {
                await configure.Invoke(services);
            }

            return services.BuildServiceProvider();
        }

        protected IList<ErrorModel> CreateErrorList(ErrorCodes errCode, string source)
        {
            var errorCode = errCode.GetIntValueAsString();
            IList<ErrorModel> errors = new List<ErrorModel>();
            ErrorModel errorModel = new ErrorModel(
                code: errorCode,
                description: string.Empty,
                title: EnumHelper.GetDescriptionFromValue(errorCode, typeof(ErrorCodes)),
                source: source
            );
            errors.Add(errorModel);

            return errors;
        }
    }
}
