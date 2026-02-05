using FinTrim.Configuration;
using FinTrim.Services;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrim;

public class FinTrim(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
    : BasePlugin<PluginConfiguration>(applicationPaths, xmlSerializer), IPluginServiceRegistrator
{
    public override string Name => Resources.PluginName;
    public override Guid Id => Guid.Parse(Resources.PluginId);

    public void RegisterServices(IServiceCollection serviceCollection, IServerApplicationHost applicationHost)
    {
        serviceCollection.AddSingleton<IPluginConfiguration>(_ => Configuration);
        serviceCollection.AddHostedService<PlaybackStoppedService>();
    }
}