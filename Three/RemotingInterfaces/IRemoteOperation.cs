using System;
using System.Data;

namespace RemotingInterfaces
{
    public interface IRemoteOperation
    {
        // Produit
        int AddProduct(int reference, string designation, int prix, int qte, string categorie);
        int UpdateProduct(int reference, string designation, int prix, int qte, string categorie);
        int DeleteProduct(int reference);
        DataTable ShowProduct();
        DataTable SearchProduct(int reference);

        // Client + Commande (fiche d'ajout d'un produit)
        int AddClientOrder(int reference, string id, string nom, string prenom,
                           string adresse, string email, DateTime date, int quantite);
        DataTable GetClientsByProduct(int reference);
        decimal GetAmount(int reference);
    }
}