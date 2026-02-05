using FinTrim.Configuration;
using FinTrim.Tasks;
using MediaBrowser.Controller.Library;
using MediaBrowser.Controller.Session;
using MediaBrowser.Model.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FinTrim.Services;

public class PlaybackStoppedService(
    ILogger logger,
    ISessionManager sessionManager,
    ITaskManager taskManager,
    IPluginConfiguration pluginConfiguration)
    : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("PlaybackStoppedService starting.");
        sessionManager.PlaybackStopped += SessionManagerOnPlaybackStopped;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("PlaybackStoppedService stopping.");
        sessionManager.PlaybackStopped -= SessionManagerOnPlaybackStopped;
        return Task.CompletedTask;
    }

    private void SessionManagerOnPlaybackStopped(object? sender, PlaybackStopEventArgs e)
    {
        if (!e.PlayedToCompletion || !pluginConfiguration.DeleteImmediately) return;

        logger.LogInformation("Playback to completion event triggered, queuing cleanup task.");
        taskManager.QueueIfNotRunning<CleanupTask>();
    }
}