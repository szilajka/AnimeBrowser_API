﻿using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimeBrowser.UnitTests.Helpers
{
    public static class StartupHelper
    {
        public static IServiceProvider SetupDI(Action<IServiceCollection> configure)
        {
            IServiceCollection services = new ServiceCollection();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
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
                .WriteTo.Console()
                .CreateLogger();

            if (configure != null)
            {
                await configure.Invoke(services);
            }

            return services.BuildServiceProvider();
        }
    }
}
