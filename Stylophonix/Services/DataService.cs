using Stylophonix.Data;
using Stylophonix.Interfaces;

namespace Stylophonix.Services;

public class DataService : IDataService
{
    private IEnumerable<PopupMenuOption> _downloads = new List<PopupMenuOption>();
    private IEnumerable<PopupMenuOption> _gigInfoNav = new List<PopupMenuOption>();
    private IEnumerable<Member> _members = new List<Member>();

    private IDictionary<string, IList<PopupMenuOption>> _music = new Dictionary<string, IList<PopupMenuOption>>();
    private IEnumerable<string> _newsImages = Array.Empty<string>();
    private IEnumerable<IEnumerable<string>> _newsParagraphs = new List<IEnumerable<string>>();

    public DataService()
    {
        LoadData();
    }

    private DateTime? LastLoad { get; set; }

    private static string Wwwroot => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    private static string DataDir => Path.Combine(Wwwroot, "data");
    private static string ImgDir => Path.Combine(Wwwroot, "img");

    public bool LoadData()
    {
        if (LastLoad != null && LastLoad > DateTime.UtcNow.AddSeconds(-30))
        {
            return false;
        }

        _members = LoadMembers();
        _newsImages = LoadNewsImages();
        _newsParagraphs = LoadNewsParagraphs();
        _gigInfoNav = LoadGigInfoNav();
        _music = LoadMusic();
        _downloads = LoadDownloads();

        LastLoad = DateTime.UtcNow;
        return true;
    }

    public IEnumerable<Member> GetMembers()
    {
        return _members;
    }

    public IEnumerable<string> GetNewsImages()
    {
        return _newsImages;
    }

    public IEnumerable<IEnumerable<string>> GetNewsParagraphs()
    {
        return _newsParagraphs;
    }

    public IEnumerable<PopupMenuOption> GetGigInfoForNav()
    {
        return _gigInfoNav;
    }

    public IDictionary<string, IList<PopupMenuOption>> GetMusic()
    {
        return _music;
    }

    public IEnumerable<PopupMenuOption> GetDownloads()
    {
        return _downloads;
    }

    private static IEnumerable<Member> LoadMembers()
    {
        var dataPath = Path.Combine(DataDir, "lineup");
        var infoFiles = Directory.GetFiles(dataPath).Where(x => !Path.GetFileName(x).StartsWith("."));

        var members = new List<Member>();

        foreach (var file in infoFiles)
        {
            var lines = File.ReadLines(file).ToArray();
            var name = lines[0];
            var instruments = lines[1].Split(" ");
            members.Add(new Member(name, GetMemberPhotoPath(Path.GetFileNameWithoutExtension(file)), instruments));
        }

        return members;
    }

    private static IEnumerable<string> LoadNewsImages()
    {
        var imgPath = Path.Combine(ImgDir, "news");
        var imgFiles = Directory.GetFiles(imgPath).Where(x => !Path.GetFileName(x).StartsWith(".")).OrderBy(x => x);
        return imgFiles.Select(GetNewsImagePath).Order().ToList();
    }

    private static IEnumerable<IEnumerable<string>> LoadNewsParagraphs()
    {
        var dataPath = Path.Combine(DataDir, "news");
        var paragraphFiles = Directory.GetFiles(dataPath).Where(x => !Path.GetFileName(x).StartsWith("."))
            .OrderBy(x => x);
        return paragraphFiles.OrderDescending().Select(File.ReadAllLines).ToArray();
    }

    private static IEnumerable<PopupMenuOption> LoadGigInfoNav()
    {
        var gigsPath = Path.Combine(DataDir, "gigs");
        var directories = Directory.GetDirectories(gigsPath);

        var gigs = new Dictionary<int, PopupMenuOption>();

        foreach (var directory in directories)
        {
            var navTxt = Path.Combine(directory, "nav.txt");
            var lines = File.ReadAllLines(navTxt);
            var externalUrl = lines.ElementAtOrDefault(3);
            var url = externalUrl ?? $"gigs/{new DirectoryInfo(directory).Name}";
            var popupMenuOption = new PopupMenuOption(lines[0], true, lines[1], url);
            var order = int.Parse(lines[2]);
            gigs.Add(order, popupMenuOption);
        }

        return gigs.OrderByDescending(x => x.Key).Select(x => x.Value).ToArray();
    }

    private static IDictionary<string, IList<PopupMenuOption>> LoadMusic()
    {
        var musicPath = Path.Combine(DataDir, "music");

        var artists = File.ReadAllLines(Path.Combine(musicPath, "artists.txt")).ToArray();
        var dict = artists.ToDictionary<string, string, IList<KeyValuePair<PopupMenuOption, int>>>(x => x,
            x => new List<KeyValuePair<PopupMenuOption, int>>());

        foreach (var file in Directory.GetFiles(musicPath).Where(x =>
                     !x.StartsWith(".") && x.EndsWith(".txt") && Path.GetFileName(x) != "artists.txt"))
        {
            var lines = File.ReadAllLines(file);
            var thumbnailUrl = Path.Combine("img/music", $"{Path.GetFileNameWithoutExtension(file)}.webp");
            var album = new PopupMenuOption(lines[1], true, lines[2], lines[4], thumbnailUrl);
            dict[lines[0]].Add(new KeyValuePair<PopupMenuOption, int>(album, int.Parse(lines[3])));
        }

        return dict.ToDictionary(x => x.Key,
            x => x.Value.OrderByDescending(y => y.Value).Select(y => y.Key).ToList() as IList<PopupMenuOption>);
    }

    private static IEnumerable<PopupMenuOption> LoadDownloads()
    {
        var downloadsPath = Path.Combine(DataDir, "downloads");

        var fileDict = new Dictionary<PopupMenuOption, int>();

        foreach (var file in Directory.GetFiles(downloadsPath).Where(x => !x.StartsWith(".") && !x.EndsWith(".txt")))
        {
            var lines = File.ReadAllLines(Path.ChangeExtension(file, "txt"));
            var fileUrl = Path.Combine("data/downloads/", $"{Path.GetFileName(file)}");
            var popupMenuOption = new PopupMenuOption(lines[0], false, Url: fileUrl);
            fileDict.Add(popupMenuOption, int.Parse(lines[1]));
        }

        return fileDict.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
    }

    private static string GetMemberPhotoPath(string name)
    {
        return Path.Combine("img/lineup", $"{name.ToLowerInvariant()}.jpg");
    }

    private static string GetNewsImagePath(string filePath)
    {
        return Path.Combine("img/news", Path.GetFileName(filePath));
    }
}