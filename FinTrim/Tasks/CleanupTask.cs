using MediaBrowser.Controller.Entities;
using MediaBrowser.Controller.Library;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace FinTrim.Tasks;

public class CleanupTask(ILogger<CleanupTask> logger, ILibraryManager libraryManager, IUserManager userManager)
    : IScheduledTask
{
    public Task ExecuteAsync(IProgress<double> progress, CancellationToken cancellationToken)
    {
        var itemList = userManager.Users.SelectMany(user =>
                libraryManager.GetItemList(
                    FinTrim.PluginConfiguration.IncludeFavorites
                        ? new InternalItemsQuery(user)
                        {
                            IsPlayed = true,
                            IncludeItemTypes = FinTrim.PluginConfiguration.GetItemTypesToDelete(),
                            ExcludeTags = FinTrim.PluginConfiguration.GetExcludedTags(),
                            IsVirtualItem = false
                        }
                        : new InternalItemsQuery(user)
                        {
                            IsFavorite = false,
                            IsPlayed = true,
                            IncludeItemTypes = FinTrim.PluginConfiguration.GetItemTypesToDelete(),
                            ExcludeTags = FinTrim.PluginConfiguration.GetExcludedTags(),
                            IsVirtualItem = false
                        }))
            .DistinctBy(item => item.Id)
            .ToArray();

        logger.LogInformation("Starting cleanup task.");
        progress.Report(0);

        for (var i = 0; i < itemList.Length; i++)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                logger.LogWarning("Cleanup cancellation requested.");
                progress.Report(100);
                return Task.FromCanceled(cancellationToken);
            }

            logger.LogInformation("Deleting item {Name} ({Index} / {Total})", itemList[i].Name, i + 1, itemList.Length);
            libraryManager.DeleteItem(itemList[i], new DeleteOptions { DeleteFileLocation = true }, true);
            progress.Report(i / (double)itemList.Length * 100);
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
                IntervalTicks = TimeSpan.FromDays(FinTrim.PluginConfiguration.Interval).Ticks
            }
        ];
    }

    public string Name => Resources.CleanupTaskName;
    public string Key => Resources.CleanupTaskKey;
    public string Description => Resources.CleanupTaskDescription;
    public string Category => Resources.PluginName;
}