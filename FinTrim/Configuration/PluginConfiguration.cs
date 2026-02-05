using MediaBrowser.Model.Plugins;

namespace FinTrim.Configuration;

public class PluginConfiguration : BasePluginConfiguration, IPluginConfiguration
{
    public uint Interval { get; set; } = 1;
    public bool IncludeFavorites { get; set; } = false;
    public bool DeleteAudio { get; set; } = false;
    public bool DeleteAudioBooks { get; set; } = false;
    public bool DeleteBooks { get; set; } = false;
    public bool DeleteEpisodes { get; set; } = true;
    public bool DeleteMovies { get; set; } = true;
    public bool DeletePhotos { get; set; } = false;
    public bool DeleteVideos { get; set; } = false;
    public bool DeleteImmediately { get; set; } = false;
    public string ExcludedTags { get; set; } = string.Empty;
}