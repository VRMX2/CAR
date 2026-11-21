using System.Data;

namespace RemotingInterfaces
{
    public interface IRemoteOperation
    {
        int Add(int matricule, string nom, string prenom);
        int Update(int matricule, string nom, string prenom);
        int Delete(int matricule);
        DataTable Show();
        DataTable ShowStudent(int matricule);
    }
}