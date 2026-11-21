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

        int AddTeacher(int matricule,string nom,string prenom,string telephone,string email,int experience,string grade);

        DataTable GetTeachersByCourse(int code);
        
        int CountTeacher(int code);
    }
}