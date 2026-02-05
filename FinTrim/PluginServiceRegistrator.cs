using FinTrim.Services;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FinTrim;

public class PluginServiceRegistrator : IPluginServiceRegistrator
{
    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddSingleton<PlaybackStoppedService>(provider =>
        {
            var logger = provider.GetRequiredService<ILogger<PlaybackStoppedService>>();
            var sessionManager = provider.GetRequiredService<ISessionManager>();
            var taskManager = provider.GetRequiredService<ITaskManager>();

            return new PlaybackStoppedService(logger, sessionManager, taskManager);
        });
    }
}