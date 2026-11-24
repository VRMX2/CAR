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


        private static string Translate(OracleException ex)
        {
            switch (ex.Number)
            {
                case 1: return "Ce code de formation existe deja.";
                case 2291: return "Reference inexistante (cle etrangere).";
                case 2292: return "Impossible de supprimer : cet article est reference ailleurs.";
                case 12899: return "Une valeur est trop longue pour sa colonne.";
                case 2290: return "Valeur refusee par une contrainte de la table.";
                default: return "Erreur Oracle : " + ex.Message;
            }
        }
    }
}
