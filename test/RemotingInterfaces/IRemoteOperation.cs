using System;
using System.Data;

namespace RemotingInterfaces
{
    public interface IRemoteOperation
    {
        string Connect();
        int AddChercheur(int idcher, string nomcher, string prenomcher, string grade);
        DataTable ShowChercheur();
        int AddArticle(int idart, string titreart, DateTime datepub, int frais, string revue, int idcher);
        DataTable ShowArticle(int idcher);
        decimal Calculer(int idcher);
    }
}