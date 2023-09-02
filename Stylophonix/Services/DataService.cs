using Stylophonix.Data;
using Stylophonix.Interfaces;

namespace Stylophonix.Services;

public class DataService : IDataService
{
    private IEnumerable<Member> _members = new List<Member>();

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
    }

    public IEnumerable<Member> GetMembers()
    {
        return _members;
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

    private static string GetMemberPhotoPath(string name)
    {
        return Path.Combine("img/lineup", $"{name.ToLowerInvariant()}.jpg");
    }
}