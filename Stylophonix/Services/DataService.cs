using Stylophonix.Data;
using Stylophonix.Interfaces;

namespace Stylophonix.Services;

public class DataService : IDataService
{
    private IEnumerable<Member> _members = new List<Member>();
    private IEnumerable<string> _newsImages = Array.Empty<string>();
    private IEnumerable<IEnumerable<string>> _newsParagraphs = new List<IEnumerable<string>>();

    public DataService()
    {
        LoadData();
    }

    private static string Wwwroot => Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
    private static string DataDir => Path.Combine(Wwwroot, "data");
    private static string ImgDir => Path.Combine(Wwwroot, "img");

    public void LoadData()
    {
        _members = LoadMembers();
        _newsImages = LoadNewsImages();
        _newsParagraphs = LoadNewsParagraphs();
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

    private static IEnumerable<Member> LoadMembers()
    {
        var dataPath = Path.Combine(DataDir, "lineup");
        var infoFiles = Directory.GetFiles(dataPath);

        var members = new List<Member>();

        foreach (var file in infoFiles)
        {
            var lines = File.ReadLines(file).ToArray();
            var name = lines[0];
            var instruments = lines[1].Split(" ");
            members.Add(new Member(name, GetMemberPhotoPath(name), instruments));
        }

        return members;
    }

    private static IEnumerable<string> LoadNewsImages()
    {
        var imgPath = Path.Combine(ImgDir, "news");
        var imgFiles = Directory.GetFiles(imgPath).OrderBy(x => x);
        return imgFiles.Select(GetNewsImagePath).Order().ToList();
    }

    private static IEnumerable<IEnumerable<string>> LoadNewsParagraphs()
    {
        var dataPath = Path.Combine(DataDir, "news");
        var paragraphFiles = Directory.GetFiles(dataPath).OrderBy(x => x);
        return paragraphFiles.OrderDescending().Select(File.ReadAllLines).ToArray();
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