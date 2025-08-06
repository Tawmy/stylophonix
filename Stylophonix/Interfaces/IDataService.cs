using Stylophonix.Data;

namespace Stylophonix.Interfaces;

public interface IDataService
{
    public bool LoadData();

    public IEnumerable<Member> GetMembers();

    public IEnumerable<string> GetNewsImages();

    public IEnumerable<IEnumerable<string>> GetNewsParagraphs();

    public IEnumerable<GigInfo> GetGigInfo();

    IDictionary<string, IList<PopupMenuOption>> GetMusic();

    IEnumerable<PopupMenuOption> GetDownloads();
}