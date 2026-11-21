using Oracle.ManagedDataAccess.Client;
using RemotingInterfaces;
using System;
using System.Data;

namespace RemotingServer
{
    public class RemoteOperation : MarshalByRefObject, IRemoteOperation
    {
        private const string oradb =
            "Data Source=127.0.0.1:1521/freepdb1;User Id=VRMX;Password=Vrmx123;";

        // ======================= PRODUIT =======================

        public int AddProduct(int reference, string designation, int prix, int qte, string categorie)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "INSERT INTO PRODUIT (reference, designation, prix, quantite_stock, categorie) " +
                        "VALUES (:reference, :designation, :prix, :quantite_stock, :categorie)", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("reference", OracleDbType.Int32).Value = reference;
                        cmd.Parameters.Add("designation", OracleDbType.Varchar2).Value = designation;
                        cmd.Parameters.Add("prix", OracleDbType.Int32).Value = prix;
                        cmd.Parameters.Add("quantite_stock", OracleDbType.Int32).Value = qte;
                        cmd.Parameters.Add("categorie", OracleDbType.Varchar2).Value = categorie;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public int UpdateProduct(int reference, string designation, int prix, int qte, string categorie)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "UPDATE PRODUIT SET designation = :designation, prix = :prix, " +
                        "quantite_stock = :quantite_stock, categorie = :categorie " +
                        "WHERE reference = :reference", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("designation", OracleDbType.Varchar2).Value = designation;
                        cmd.Parameters.Add("prix", OracleDbType.Int32).Value = prix;
                        cmd.Parameters.Add("quantite_stock", OracleDbType.Int32).Value = qte;
                        cmd.Parameters.Add("categorie", OracleDbType.Varchar2).Value = categorie;
                        cmd.Parameters.Add("reference", OracleDbType.Int32).Value = reference;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public int DeleteProduct(int reference)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "DELETE FROM PRODUIT WHERE reference = :reference", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("reference", OracleDbType.Int32).Value = reference;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public DataTable ShowProduct()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT reference, designation, prix, quantite_stock, categorie " +
                        "FROM PRODUIT ORDER BY reference", con))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "TableOracle");
                        return ds.Tables["TableOracle"];
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public DataTable SearchProduct(int reference)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT reference, designation, prix, quantite_stock, categorie " +
                        "FROM PRODUIT WHERE reference = :reference", con))
                    {
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add(
                            "reference", OracleDbType.Int32).Value = reference;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "TableOracle");
                        return ds.Tables["TableOracle"];
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        // ================= CLIENT + COMMANDE =================

        public int AddClientOrder(int reference, string id, string nom, string prenom,
                                  string adresse, string email, DateTime date, int quantite)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleTransaction tx = con.BeginTransaction())
                    {
                        try
                        {
                            // 1) Create the client only if he does not exist yet
                            int exists;
                            using (OracleCommand cmd = new OracleCommand(
                                "SELECT COUNT(*) FROM CLIENT WHERE identifiant = :identifiant", con))
                            {
                                cmd.Transaction = tx;
                                cmd.BindByName = true;
                                cmd.Parameters.Add("identifiant", OracleDbType.Varchar2).Value = id;
                                exists = Convert.ToInt32(cmd.ExecuteScalar());
                            }

                            if (exists == 0)
                            {
                                using (OracleCommand cmd = new OracleCommand(
                                    "INSERT INTO CLIENT (identifiant, nom, prenom, adresse, email) " +
                                    "VALUES (:identifiant, :nom, :prenom, :adresse, :email)", con))
                                {
                                    cmd.Transaction = tx;
                                    cmd.BindByName = true;
                                    cmd.Parameters.Add("identifiant", OracleDbType.Varchar2).Value = id;
                                    cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = nom;
                                    cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = prenom;
                                    cmd.Parameters.Add("adresse", OracleDbType.Varchar2).Value = adresse;
                                    cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                                    cmd.ExecuteNonQuery();
                                }
                            }

                            // 2) Record the order
                            using (OracleCommand cmd = new OracleCommand(
                                "INSERT INTO COMMANDE (reference, identifiant, date_commande, qauntite_commande) " +
                                "VALUES (:reference, :identifiant, :date_commande, :quantite)", con))
                            {
                                cmd.Transaction = tx;
                                cmd.BindByName = true;
                                cmd.Parameters.Add("reference", OracleDbType.Int32).Value = reference;
                                cmd.Parameters.Add("identifiant", OracleDbType.Varchar2).Value = id;
                                cmd.Parameters.Add("date_commande", OracleDbType.Date).Value = date;
                                cmd.Parameters.Add("quantite", OracleDbType.Int32).Value = quantite;
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                            return 1;
                        }
                        catch
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1)
                    throw new Exception("Ce client a deja commande ce produit.");
                throw new Exception(Translate(ex));
            }
        }

        public DataTable GetClientsByProduct(int reference)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT cl.nom, cl.prenom, c.qauntite_commande AS quantite, c.date_commande " +
                        "FROM COMMANDE c JOIN CLIENT cl ON cl.identifiant = c.identifiant " +
                        "WHERE c.reference = :reference ORDER BY c.date_commande", con))
                    {
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add(
                            "reference", OracleDbType.Int32).Value = reference;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "TableOracle");
                        return ds.Tables["TableOracle"];
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        // Total amount of all orders of one product = SUM(quantite x prix)
        public decimal GetAmount(int reference)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT NVL(SUM(c.qauntite_commande * p.prix), 0) " +
                        "FROM COMMANDE c JOIN PRODUIT p ON p.reference = c.reference " +
                        "WHERE c.reference = :reference", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("reference", OracleDbType.Int32).Value = reference;
                        return Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        // ======================== Helpers ========================

        private static string Translate(OracleException ex)
        {
            switch (ex.Number)
            {
                case 1:     return "Cette valeur existe deja (reference ou identifiant duplique).";
                case 1438:  return "Nombre trop grand pour la colonne (5 chiffres maximum).";
                case 2290:  return "Valeur refusee par une contrainte (categorie non valide).";
                case 2291:  return "Produit ou client inexistant.";
                case 2292:  return "Impossible de supprimer : ce produit est lie a des commandes.";
                case 12899: return "Une valeur est trop longue pour sa colonne.";
                default:    return "Erreur Oracle : " + ex.Message;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}