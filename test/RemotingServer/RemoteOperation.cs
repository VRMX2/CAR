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
                return "ECHEC : " + Translate(ex);
            }
        }

        public int AddChercheur(int idcher, string nomcher, string prenomcher, string grade)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "insert into chercheur (idcher,nomcher,prenomcher,grade) " +
                        "values (:idcher,:nomcher,:prenomcher,:grade)", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("idcher", OracleDbType.Int32).Value = idcher;
                        cmd.Parameters.Add("nomcher", OracleDbType.Varchar2).Value = nomcher;
                        cmd.Parameters.Add("prenomcher", OracleDbType.Varchar2).Value = prenomcher;
                        cmd.Parameters.Add("grade", OracleDbType.Varchar2).Value = grade;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public DataTable ShowChercheur()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT idcher,nomcher,prenomcher,grade FROM chercheur ORDER BY idcher", con))
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

        public int AddArticle(int idart, string titreart, DateTime datepub, int frais, string revue, int idcher)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "insert into article (idart,titreart,datepub,frais,revue,idcher) " +
                        "values (:idart,:titreart,:datepub,:frais,:revue,:idcher)", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("idart", OracleDbType.Int32).Value = idart;
                        cmd.Parameters.Add("titreart", OracleDbType.Varchar2).Value = titreart;
                        cmd.Parameters.Add("datepub", OracleDbType.Date).Value = datepub;
                        cmd.Parameters.Add("frais", OracleDbType.Int32).Value = frais;
                        cmd.Parameters.Add("revue", OracleDbType.Varchar2).Value = revue;
                        cmd.Parameters.Add("idcher", OracleDbType.Int32).Value = idcher;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        public DataTable ShowArticle(int idcher)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT idart,titreart,datepub,frais,revue,idcher FROM article " +
                        "WHERE idcher = :idcher ORDER BY idart", con))
                    {
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add("idcher", OracleDbType.Int32).Value = idcher;

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

        public decimal Calculer(int idcher)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT NVL(SUM(frais), 0) FROM article WHERE idcher = :idcher", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("idcher", OracleDbType.Int32).Value = idcher;
                        return Convert.ToDecimal(cmd.ExecuteScalar());
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception(Translate(ex));
            }
        }

        private static string Translate(OracleException ex)
        {
            switch (ex.Number)
            {
                case 1: return "Cet identifiant existe deja (ORA-00001).";
                case 904: return "Nom de colonne invalide (ORA-00904).";
                case 942: return "Table introuvable (ORA-00942). Creez les tables CHERCHEUR et ARTICLE dans le schema SYSTEM.";
                case 1008: return "Parametre non lie (ORA-01008). Verifiez les noms des parametres.";
                case 1017: return "Utilisateur ou mot de passe Oracle incorrect (ORA-01017).";
                case 2290: return "Valeur refusee par une contrainte (grade, revue ou frais invalide).";
                case 2291: return "Chercheur inexistant (cle etrangere).";
                case 2292: return "Impossible de supprimer : des articles referencent ce chercheur.";
                case 12514:
                case 12541:
                case 12170:
                    return "Impossible de joindre Oracle (ORA-" + ex.Number.ToString("00000") + "). Verifiez que le service XE et le listener sont demarres.";
                case 12899: return "Une valeur est trop longue pour sa colonne.";
                case 50000: return "Delai de connexion Oracle depasse (ORA-50000). Verifiez les verrous (COMMIT dans SQL Developer) et redemarrez le service Oracle.";
                default: return "Erreur Oracle ORA-" + ex.Number.ToString("00000") + " : " + ex.Message;
            }
        }
    }
}