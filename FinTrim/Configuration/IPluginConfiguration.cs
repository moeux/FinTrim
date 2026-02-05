using Jellyfin.Data.Enums;

namespace FinTrim.Configuration;

public interface IPluginConfiguration
{
    public uint Interval { get; set; }
    public bool IncludeFavorites { get; set; }
    public bool DeleteAudio { get; set; }
    public bool DeleteAudioBooks { get; set; }
    public bool DeleteBooks { get; set; }
    public bool DeleteEpisodes { get; set; }
    public bool DeleteMovies { get; set; }
    public bool DeletePhotos { get; set; }
    public bool DeleteVideos { get; set; }
    public bool DeleteImmediately { get; set; }
    public string ExcludedTag { get; set; }

    public BaseItemKind[] GetItemTypesToDelete()
    {
        var itemTypes = new[]
        {
            (DeleteAudio, BaseItemKind.Audio),
            (DeleteAudioBooks, BaseItemKind.AudioBook),
            (DeleteBooks, BaseItemKind.Book),
            (DeleteEpisodes, BaseItemKind.Episode),
            (DeleteMovies, BaseItemKind.Movie),
            (DeletePhotos, BaseItemKind.Photo),
            (DeleteVideos, BaseItemKind.Video)
        };

        return itemTypes.Where(pair => pair.Item1).Select(pair => pair.Item2).ToArray();
    }
}