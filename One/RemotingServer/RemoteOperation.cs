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

        public int Add(int matricule, string nom, string prenom)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "INSERT INTO Etudiant (matricule, nom, prenom) " +
                        "VALUES (:matricule, :nom, :prenom)", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;
                        cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = nom;
                        cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = prenom;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1)
                    throw new Exception("Ce matricule existe deja.");
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public int Update(int matricule, string nom, string prenom)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "UPDATE Etudiant SET nom = :nom, prenom = :prenom " +
                        "WHERE matricule = :matricule", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = nom;
                        cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = prenom;
                        cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public int Delete(int matricule)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "DELETE FROM Etudiant WHERE matricule = :matricule", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public DataTable Show()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT matricule, nom, prenom FROM Etudiant ORDER BY matricule", con))
                    {
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "TableOracle");
                        return ds.Tables["TableOracle"];
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public DataTable ShowStudent(int matricule)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT matricule, nom, prenom FROM Etudiant " +
                        "WHERE matricule = :matricule", con))
                    {
                        adapter.SelectCommand.BindByName = true;
                        adapter.SelectCommand.Parameters.Add(
                            "matricule", OracleDbType.Int32).Value = matricule;

                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "TableOracle");
                        return ds.Tables["TableOracle"];
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}