namespace FoxStevenle.API.Constants;

public static class GeneralConstants
{
    public const string DateOnlyKeyFormat = "yyyy-MM-dd";
    public const int SongCountPerDay = 5;
    public const int HintCountPerSong = 3;
    public const int FirstHintLengthMillis = 500;
    public const int SecondHintLengthMillis = 1000;
    public const int ThirdHintLengthMillis = 5000;
#if DEBUG
    public const string HintsDir = "../../hints";
    public const string SongsDir = "../../songs";
#else
    public const string HintsDir = "/app/fs_data/hints";
    public const string SongsDir = "/app/fs_data/songs";
#endif
}