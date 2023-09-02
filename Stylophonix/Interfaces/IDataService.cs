using Stylophonix.Data;

namespace Stylophonix.Interfaces;

public interface IDataService
{
    public void LoadData();

    public IEnumerable<Member> GetMembers();
}