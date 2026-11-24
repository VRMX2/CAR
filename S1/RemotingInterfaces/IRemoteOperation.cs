using System;
using System.Data;

namespace RemotingInterfaces
{
    public interface IRemoteOperation
    {
        string Connect();

        int AddFormation(int code, string intitule, int duree, int prix, string niveau, DateTime date_debut);
        int UpdateFormation(int code, string intitule, int duree, int prix, string niveau, DateTime date_debut);
        int DeleteFormation(int code);
        DataTable ShowFormation();
        DataTable SearchFormation(int code);

        int AddStagiaire(int code, string identifiant, string nom, string prenom, string email,
                         string telephone, string ville, DateTime date_inscription, int montant_paye);
        DataTable GetStagiaires(int code);
        int CountStagiaires(int code);
        int SumMontant(int code);
    }
}