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

        public int AddCourse(int code, string designation, int volume, string categorie)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "INSERT INTO COURS (code, designation, volume,categorie) " +
                        "VALUES (:code, :designation, :volume,:categorie)", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        cmd.Parameters.Add("designation", OracleDbType.Varchar2).Value = designation;
                        cmd.Parameters.Add("volume", OracleDbType.Int32).Value = volume;
                        cmd.Parameters.Add("categorie", OracleDbType.Varchar2).Value = categorie;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1)
                    throw new Exception("Ce Code existe deja.");
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public int UpdateCourse(int code, string designation, int volume, string categorie)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "UPDATE cours SET designation = :designation, volume = :volume  ,categorie = :categorie WHERE matricule = :matricule", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("designation", OracleDbType.Varchar2).Value = designation;
                        cmd.Parameters.Add("volume", OracleDbType.Int32).Value = volume;
                        cmd.Parameters.Add("categorie", OracleDbType.Varchar2).Value = categorie;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }


        public int DeleteCourse(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "DELETE FROM cours WHERE code = :code", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        return cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (OracleException ex)
            {
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }


        public DataTable ShowCourse()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT code, designation, volume, categorie FROM cours ORDER BY code", con))
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

        public DataTable SearchCourse(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT code,designation,volume , categorie FROM cours " +
                        "WHERE code = :code", con))
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
                throw new Exception("Erreur Oracle : " + ex.Message);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}