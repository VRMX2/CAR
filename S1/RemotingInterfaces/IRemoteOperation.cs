using System;
using System.Data;

namespace RemotingInterfaces
{
    public interface IRemoteOperation
    {
        // Formation
        int AddFormation(int code, string intitule, int duree, int prix, string niveau, DateTime date_debut);
        int UpdateFormation(int code, string intitule, int duree, int prix, string niveau, DateTime date_debut);
        int DeleteFormation(int code);
        DataTable ShowFormation();
        DataTable SearchFormation(int code);
    }
}