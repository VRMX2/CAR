using Oracle.ManagedDataAccess.Client;
using RemotingInterfaces;
using System;
using System.Data;

namespace RemotingServer
{
    public class RemoteOperation : MarshalByRefObject, IRemoteOperation
    {
        private const string oradb =
            "Data Source=127.0.0.1:1521/XE;User Id=system;Password=lahcengrissi123;";

        public override object InitializeLifetimeService()
        {
            return null;
        }


        public string Connect()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    return "OK";
                }
            }
            catch (OracleException ex)
            {
                return "ECHEC : " + ex.Message;
            }
        }

        public int AddFormation(int code, string intitule,int duree, int prix ,string niveau,DateTime date_debut)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "insert into formation (code,intitule,duree,prix,niveau,date_debut) values(:code,:intitule,:duree,:prix,:niveau,:date_debut)",con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        cmd.Parameters.Add("intitule", OracleDbType.Varchar2).Value = intitule;
                        cmd.Parameters.Add("duree", OracleDbType.Int32).Value = duree;
                        cmd.Parameters.Add("prix", OracleDbType.Int32).Value = prix;
                        cmd.Parameters.Add("niveau", OracleDbType.Varchar2).Value = niveau;
                        cmd.Parameters.Add("date_debut",OracleDbType.Date).Value = date_debut;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public int UpdateFormation(int code, string intitule,int duree, int prix ,string niveau,DateTime date_debut)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "update formation set intitule = :intitule, duree = :duree,prix = :prix,niveau = :niveau, date_debut = :date_debut where code = :code",con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("intitule", OracleDbType.Varchar2).Value = intitule;
                        cmd.Parameters.Add("duree", OracleDbType.Int32).Value = duree;
                        cmd.Parameters.Add("prix", OracleDbType.Int32).Value = prix;
                        cmd.Parameters.Add("niveau", OracleDbType.Varchar2).Value = niveau;
                        cmd.Parameters.Add("date_debut",OracleDbType.Date).Value = date_debut;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }



        public int DeleteFormation(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "DELETE FROM formation WHERE code = :code", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public DataTable ShowFormation()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT code,intitule,duree,prix,niveau,date_debut from formation ORDER BY code", con))
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

        public DataTable SearchFormation(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT code,intitule,duree,prix,niveau,date_debut from formation where code = :code ", con))
                    {
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add(
                            "code", OracleDbType.Int32).Value = code;

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

        public int AddStagiaire(int code, string identifiant, string nom, string prenom, string email,
                        string telephone, string ville, DateTime date_inscription, int montant_paye)
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
                            // 1. Insert stagiaire only if he does not exist yet
                            using (OracleCommand cmd = new OracleCommand(
                                "insert into stagiaire (identifiant,nom,prenom,email,telephone,ville) " +
                                "select :identifiant,:nom,:prenom,:email,:telephone,:ville from dual " +
                                "where not exists (select 1 from stagiaire where identifiant = :identifiant)", con))
                            {
                                cmd.Transaction = tx;
                                cmd.BindByName = true;
                                cmd.Parameters.Add("identifiant", OracleDbType.Varchar2).Value = identifiant;
                                cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = nom;
                                cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = prenom;
                                cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                                cmd.Parameters.Add("telephone", OracleDbType.Varchar2).Value = telephone;
                                cmd.Parameters.Add("ville", OracleDbType.Varchar2).Value = ville;
                                cmd.ExecuteNonQuery();
                            }

                            // 2. Insert inscription
                            using (OracleCommand cmd = new OracleCommand(
                                "insert into inscription (code,identifiant,date_inscription,montant_paye) " +
                                "values(:code,:identifiant,:date_inscription,:montant_paye)", con))
                            {
                                cmd.Transaction = tx;
                                cmd.BindByName = true;
                                cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                                cmd.Parameters.Add("identifiant", OracleDbType.Varchar2).Value = identifiant;
                                cmd.Parameters.Add("date_inscription", OracleDbType.Date).Value = date_inscription;
                                cmd.Parameters.Add("montant_paye", OracleDbType.Int32).Value = montant_paye;
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                            return 1;                       // success
                        }
                        catch (OracleException ex)
                        {
                            tx.Rollback();
                            if (ex.Number == 1) return 0;   // inscription already exists
                            throw;
                        }
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }
        public int CountStagiaires(int code)
        {
            using (OracleConnection con = new OracleConnection(oradb))
            {
                con.Open();
                using (OracleCommand cmd = new OracleCommand(
                    "SELECT COUNT(*) FROM inscription WHERE code = :code", con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public int SumMontant(int code)
        {
            using (OracleConnection con = new OracleConnection(oradb))
            {
                con.Open();
                using (OracleCommand cmd = new OracleCommand(
                    "SELECT SUM(montant_paye) FROM inscription WHERE code = :code", con))
                {
                    cmd.BindByName = true;
                    cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                    object r = cmd.ExecuteScalar();
                    return (r == null || r == DBNull.Value) ? 0 : Convert.ToInt32(r);
                }
            }
        }
        public DataTable GetStagiaires(int code)
        {
            using (OracleConnection con = new OracleConnection(oradb))
            {
                con.Open();
                using (OracleDataAdapter adapter = new OracleDataAdapter(
                    "SELECT s.nom, s.prenom, i.date_inscription, i.montant_paye " +
                    "FROM stagiaire s JOIN inscription i ON s.identifiant = i.identifiant " +
                    "WHERE i.code = :code", con))
                {
                    adapter.SelectCommand.BindByName = true;
                    adapter.SelectCommand.Parameters.Add("code", OracleDbType.Int32).Value = code;
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "T");
                    return ds.Tables["T"];
                }
            }
        }


        private static string Translate(OracleException ex)
        {
            switch (ex.Number)
            {
                case 1: return "Ce code de formation existe deja.";
                case 2291: return "Reference inexistante (cle etrangere).";
                case 2292: return "Impossible de supprimer : cette formation a des inscriptions.";
                case 12899: return "Une valeur est trop longue pour sa colonne.";
                case 2290: return "Valeur refusee par une contrainte de la table.";
                default: return "Erreur Oracle : " + ex.Message;
            }
        }
    }
}
