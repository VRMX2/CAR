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

        // ======================= COURS =======================

        public int AddCourse(int code, string designation, int volume, string categorie)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "INSERT INTO COURS (code, designation, volume, categorie) " +
                        "VALUES (:code, :designation, :volume, :categorie)", con))
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
                throw new Exception(Translate(ex));
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
                        "UPDATE COURS SET designation = :designation, volume = :volume, " +
                        "categorie = :categorie WHERE code = :code", con))
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
                throw new Exception(Translate(ex));
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
                        "DELETE FROM COURS WHERE code = :code", con))
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

        public DataTable ShowCourse()
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT code, designation, volume, categorie FROM COURS ORDER BY code", con))
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

        public DataTable SearchCourse(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT code, designation, volume, categorie FROM COURS " +
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
                throw new Exception(Translate(ex));
            }
        }

        // ===================== ENSEIGNANTS =====================

        // Inserts the teacher (new matricule = MAX + 1) and links him to the course
        // in ENSEIGNE, in one transaction. Returns the new matricule.
        public int AddTeacher(int code, string nom, string prenom, string telephone,
                              string email, int experience, string grade)
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
                            int matricule;

                            using (OracleCommand cmdId = new OracleCommand(
                                "SELECT NVL(MAX(matricule), 0) + 1 FROM ENSEIGNANT", con))
                            {
                                cmdId.Transaction = tx;
                                matricule = Convert.ToInt32(cmdId.ExecuteScalar());
                            }

                            using (OracleCommand cmd = new OracleCommand(
                                "INSERT INTO ENSEIGNANT " +
                                "(matricule, nom, prenom, telephone, email, experience, grade) " +
                                "VALUES (:matricule, :nom, :prenom, :telephone, :email, :experience, :grade)", con))
                            {
                                cmd.Transaction = tx;
                                cmd.BindByName = true;
                                cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;
                                cmd.Parameters.Add("nom", OracleDbType.Varchar2).Value = nom;
                                cmd.Parameters.Add("prenom", OracleDbType.Varchar2).Value = prenom;
                                cmd.Parameters.Add("telephone", OracleDbType.Varchar2).Value = telephone;
                                cmd.Parameters.Add("email", OracleDbType.Varchar2).Value = email;
                                cmd.Parameters.Add("experience", OracleDbType.Int32).Value = experience;
                                cmd.Parameters.Add("grade", OracleDbType.Varchar2).Value = grade;
                                cmd.ExecuteNonQuery();
                            }

                            using (OracleCommand cmd = new OracleCommand(
                                "INSERT INTO ENSEIGNE (code, matricule) VALUES (:code, :matricule)", con))
                            {
                                cmd.Transaction = tx;
                                cmd.BindByName = true;
                                cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                                cmd.Parameters.Add("matricule", OracleDbType.Int32).Value = matricule;
                                cmd.ExecuteNonQuery();
                            }

                            tx.Commit();
                            return matricule;
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
                throw new Exception(Translate(ex));
            }
        }

        public DataTable GetTeachersByCourse(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleDataAdapter adapter = new OracleDataAdapter(
                        "SELECT e.nom, e.prenom, e.grade " +
                        "FROM ENSEIGNANT e JOIN ENSEIGNE s ON e.matricule = s.matricule " +
                        "WHERE s.code = :code ORDER BY e.nom", con))
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

        public int CountTeacher(int code)
        {
            try
            {
                using (OracleConnection con = new OracleConnection(oradb))
                {
                    con.Open();
                    using (OracleCommand cmd = new OracleCommand(
                        "SELECT COUNT(*) FROM ENSEIGNE WHERE code = :code", con))
                    {
                        cmd.BindByName = true;
                        cmd.Parameters.Add("code", OracleDbType.Int32).Value = code;
                        return Convert.ToInt32(cmd.ExecuteScalar());
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
                case 1:     return "Cette valeur existe deja (cle primaire dupliquee).";
                case 2291:  return "Le cours indique n'existe pas.";
                case 2292:  return "Impossible de supprimer : ce cours a des enseignants associes.";
                case 12899: return "Une valeur est trop longue pour sa colonne (email : 20 caracteres max).";
                case 2290:  return "Valeur refusee par une contrainte (categorie A/B/C, grade...).";
                default:    return "Erreur Oracle : " + ex.Message;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}