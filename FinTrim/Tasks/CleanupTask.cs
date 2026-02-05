using FinTrim.Configuration;
using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace FinTrim.Tasks;

public class CleanupTask(ILogger logger, IPluginConfiguration pluginConfiguration, ILibraryManager libraryManager)
    : IScheduledTask
{
    public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        var itemList = libraryManager.GetItemList(new InternalItemsQuery
        {
            IsPlayed = true,
            IncludeItemTypes = pluginConfiguration.GetItemTypesToDelete(),
            ExcludeTags = [pluginConfiguration.ExcludedTag],
            IsVirtualItem = false
        });

        logger.LogInformation("Starting cleanup task.");
        progress.Report(0);

        for (var i = 0; i < itemList.Count; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Cleanup cancellation requested.");
                progress.Report(100);
                return Task.FromCanceled(cancellationToken);
            }

            logger.LogInformation("Deleting item {Name} ({Index} / {Total})", itemList[i].Name, i + 1, itemList.Count);
            libraryManager.DeleteItem(itemList[i], new DeleteOptions { DeleteFileLocation = true }, true);
            progress.Report(i / (double)itemList.Count * 100);
        }

        logger.LogInformation("Cleanup complete.");
        progress.Report(100);
        return Task.CompletedTask;
    }

    public IEnumerable<TaskTriggerInfo> GetDefaultTriggers()
    {
        return
        [
            new TaskTriggerInfo
            {
                Type = TaskTriggerInfoType.IntervalTrigger,
                IntervalTicks = TimeSpan.FromDays(pluginConfiguration.Interval).Ticks
            }
        ];
    }

    public string Name => Resources.CleanupTaskName;
    public string Key => Resources.CleanupTaskKey;
    public string Description => Resources.CleanupTaskDescription;
    public string Category => Resources.PluginName;
}