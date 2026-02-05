using FinTrim.Tasks;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Logging;

namespace FinTrim.Services;

public class PlaybackStoppedService
{
    private readonly ILogger<PlaybackStoppedService> _logger;
    private readonly ITaskManager _taskManager;

    public PlaybackStoppedService(
        ILogger<PlaybackStoppedService> logger,
        ISessionManager sessionManager,
        ITaskManager taskManager)
    {
        _logger = logger;
        _taskManager = taskManager;
        sessionManager.PlaybackStopped += SessionManagerOnPlaybackStopped;
    }

    private void SessionManagerOnPlaybackStopped(object? sender, PlaybackStopEventArgs e)
    {
        if (!e.PlayedToCompletion || !FinTrim.PluginConfiguration.DeleteImmediately) return;

        _logger.LogInformation("Playback to completion event triggered, queuing cleanup task.");
        _taskManager.QueueIfNotRunning<CleanupTask>();
    }
}