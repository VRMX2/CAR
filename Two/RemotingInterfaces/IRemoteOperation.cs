using System.Data;

namespace RemotingInterfaces
{
    public interface IRemoteOperation
    {
        int AddCourse(int code, string designation, int volume , string categorie);
        int UpdateCourse(int code, string designation, int volume, string categorie);
        int DeleteCourse(int code);
        DataTable ShowCourse();
        DataTable SearchCourse(int code);
    }
}