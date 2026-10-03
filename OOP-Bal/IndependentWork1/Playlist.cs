class Playlist
{
    private string name = "";
    private int songsCount;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    public int SongsCount
    {
        get { return songsCount; }
        set
        {
            if (value >= 0)
                songsCount = value;
            else
                songsCount = 0;
        }
    }
    public Playlist(string name, int songsCount)
    {
        Name = name;
        SongsCount = songsCount;
    }
    public bool IsLongPlaylist()
    {
        return SongsCount >= 20;
    }
}