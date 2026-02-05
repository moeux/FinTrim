using FinTrim.Configuration;
using FinTrim.Services;
using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Controller;
using MediaBrowser.Controller.Plugins;
using MediaBrowser.Model.Serialization;
using Microsoft.Extensions.DependencyInjection;

namespace FinTrim;

public class FinTrim : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public override string Name => Resources.PluginName;
    public override Guid Id => Guid.Parse(Resources.PluginId);

    public IEnumerable<PluginPageInfo> GetPages()
    {
        return
        [
            new PluginPageInfo
            {
                Name = Resources.PluginName,
                EmbeddedResourcePath = $"{GetType().Namespace}.Web.configPage.html"
            }
        ];
    }
}