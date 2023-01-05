using Metaforge_Marketing.Models;
using Metaforge_Marketing.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace Metaforge_Marketing.Repository
{
    public class QuotationRepository
    {
        // public static void Insert(SqlConnection conn, Item item, Quotation quotation, DataTable table, RMCosting rmCosting)
        // {
        //      int quotationId = InsertQuotation()
        //      InsertRM_V
        //      InsertCC_V
        //      UpdateItemStatus
        // }


        #region Inserting New Quotation/ Version

        public static void Insert(SqlConnection conn, Item item, Quotation quotation, DataTable table, RMCosting rmCosting)
        {
            SqlTransaction transaction = conn.BeginTransaction();
            try
            {
                int quotationId = InsertQuotation(conn, transaction, item, quotation);
                InsertRM_V(conn, transaction, rmCosting, quotationId);
                InsertCC_V(conn, transaction, table, quotationId);
                transaction.Commit();
                MessageBox.Show("Success");
            }
            catch(Exception e1)
            {
                MessageBox.Show(e1.Message);
                try
                {
                    transaction.Rollback();
                }
                catch(Exception e2) { MessageBox.Show(e2.Message); }
            }
            finally { transaction.Dispose(); }
        }
        public static int InsertQuotation(SqlConnection conn, SqlTransaction transaction, Item item, Quotation quotation)
        {
            SqlCommand cmd = new SqlCommand("InsertQuotation", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@date", SqlDbType.DateTime).Value = quotation.Date;
            cmd.Parameters.Add("@v_number", SqlDbType.Int).Value = quotation.V_Number;
            cmd.Parameters.Add("@q_number", SqlDbType.VarChar).Value = quotation.Q_Number;
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        public static void InsertRM_V(SqlConnection conn, SqlTransaction transaction, RMCosting rmCosting, int quotationId)
        {
            SqlCommand cmd = new SqlCommand("InsertRM_V", conn, transaction) 
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@quotedRMRate", SqlDbType.Decimal).Value = rmCosting.QuotedRMRate;
            cmd.Parameters.Add("@currentRMRate", SqlDbType.Decimal).Value = rmCosting.CurrentRMRate;
            cmd.Parameters.Add("@rmAsPerDrawing", SqlDbType.VarChar).Value = rmCosting.RMAsPerDrawing;
            cmd.Parameters.Add("@quotationId", SqlDbType.Int).Value = quotationId;
            cmd.Parameters.Add("@rmMasterId", SqlDbType.Int).Value = rmCosting.RMConsidered.Id;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static void InsertCC_V(SqlConnection conn, SqlTransaction transaction, DataTable table, int quotationId)
        {
            SqlCommand InsertCommand = new SqlCommand("InsertCC_V", conn, transaction)
            {
                CommandType = CommandType.StoredProcedure,
            };
            foreach (DataRow row in table.Rows)
            {
                InsertCommand.Parameters.Add("@operationId", SqlDbType.Int).Value = row["OperationId"];
                InsertCommand.Parameters.Add("@quotationId", SqlDbType.Int).Value = quotationId;
                InsertCommand.Parameters.Add("@stepNo", SqlDbType.Int).Value = row["StepNo"];
                InsertCommand.Parameters.Add("@ccPerPiece", SqlDbType.Decimal).Value = row["CCPerPiece"];
                InsertCommand.Parameters.Add("@isOutsourced", SqlDbType.Int).Value = Convert.ToInt16(row["IsOutsourced"]);

                try
                {
                    InsertCommand.ExecuteNonQuery();
                    InsertCommand.Parameters.Clear();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }   
        }
        #endregion Insert New Version/ Quotation

        #region Upsert Drafts/ MF Costing
        public static void UpdateRM_V(SqlConnection conn, SqlTransaction transaction, RMCosting rmCosting)
        {
            SqlCommand cmd = new SqlCommand("UpdateRM_V", conn, transaction) 
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@id", SqlDbType.Int).Value = rmCosting.Id;
            cmd.Parameters.Add("@currentRMRate", SqlDbType.Decimal).Value = rmCosting.CurrentRMRate;
            cmd.Parameters.Add("@quotedRMRate", SqlDbType.Decimal).Value = rmCosting.QuotedRMRate;
            cmd.Parameters.Add("@rmAsPerDrawing", SqlDbType.VarChar).Value = rmCosting.RMAsPerDrawing;
            cmd.Parameters.Add("@rmmId", SqlDbType.Int).Value = rmCosting.RMConsidered.Id;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        // Summary:
        //      Given a DataTable, Inserts/ updates/ deletes rows that are changed
        // Parameters:
        //      Item- whose Costings are being inserted
        //      category- the Category of the costing
        //      DataTable- The datatable from which the changes are to be taken
        public static void UpsertCC_V(SqlConnection conn, SqlTransaction transaction, DataTable table, int quotationId)
        {
            using (SqlDataAdapter adapter = new SqlDataAdapter())
            {
                SqlCommand InsertCommand = new SqlCommand("InsertCC_V", conn, transaction)
                {
                    CommandType = CommandType.StoredProcedure,
                };

                adapter.InsertCommand = InsertCommand;
                InsertCommand.Parameters.Add("@operationId", SqlDbType.Int, 16, "OperationId");
                InsertCommand.Parameters.Add("@stepNo", SqlDbType.Int, 16, "StepNo");
                InsertCommand.Parameters.Add("@ccPerPiece", SqlDbType.Decimal, 16, "CCPerPiece");
                InsertCommand.Parameters.Add("@isOutsourced", SqlDbType.Int, 16, "IsOutsourced");
                InsertCommand.Parameters.Add("@quotationId", SqlDbType.Int).Value = quotationId;

                adapter.DeleteCommand = new SqlCommand("DELETE FROM ConversionCostings WHERE Id = @id", conn, transaction);
                adapter.DeleteCommand.Parameters.Add("@id", SqlDbType.Int, 32, "Id");

                SqlCommand UpdateCommand = new SqlCommand("UpdateCC_V", conn, transaction)
                {
                    CommandType = CommandType.StoredProcedure
                };
                UpdateCommand.Parameters.Add("@operationId", SqlDbType.Int, 16, "OperationId");
                UpdateCommand.Parameters.Add("@stepNo", SqlDbType.Int, 16, "StepNo");
                UpdateCommand.Parameters.Add("@ccPerPiece", SqlDbType.Decimal, 16, "CCPerPiece");
                UpdateCommand.Parameters.Add("@isOutsourced", SqlDbType.Int, 16, "IsOutsourced");
                UpdateCommand.Parameters.Add("@id", SqlDbType.Int, 32, "Id");

                adapter.UpdateCommand = UpdateCommand;

                try
                {
                    adapter.Update(table);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public static void UpsertRM_V(SqlConnection conn, SqlTransaction transaction, RMCosting rmCosting, int quotationId)
        {
            if (rmCosting.IsRMCostingPresent)
            {
                UpdateRM_V(conn, transaction, rmCosting);
            }
            else
            {
                InsertRM_V(conn, transaction, rmCosting, quotationId);
            }
        }
        public static void Upsert(SqlConnection conn, Item item, Quotation quotation, DataTable table, RMCosting rmCosting)
        {
            SqlTransaction transaction = conn.BeginTransaction();

            try
            {
                if (quotation.Id == 0)
                {
                    quotation.Id = InsertQuotation(conn, transaction, item, quotation);
                }
                UpsertRM_V(conn, transaction, rmCosting, quotation.Id);
                UpsertCC_V(conn, transaction, table, quotation.Id);
                transaction.Commit();
            }
            catch(Exception e1)
            {
                MessageBox.Show(e1.Message);
                try
                {
                    transaction.Rollback();
                }
                catch(Exception e2)
                {
                    MessageBox.Show(e2.Message);
                }
            }
            finally { transaction.Dispose(); }

        }
        #endregion

        #region Fetch RM/ CC/ Quotations, etc
        public static RMCosting FetchRM_V(SqlConnection conn, Quotation quotation)
        {
            RMCosting rmCosting = new RMCosting();
            SqlCommand cmd = new SqlCommand("FetchRM_V", conn)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@quotationId", SqlDbType.Int).Value = quotation.Id;

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                rmCosting.IsRMCostingPresent = true;
                while (reader.Read())
                {
                    rmCosting.Id = Convert.ToInt32(reader["Id"]);
                    rmCosting.RMAsPerDrawing = reader["RMAsPerDrawing"].ToString();
                    rmCosting.CurrentRMRate = (float)Convert.ToDecimal(reader["CurrentRMRate"]);
                    rmCosting.QuotedRMRate = (float)Convert.ToDecimal(reader["QuotedRMRate"]);
                    rmCosting.RMConsidered = new RM()
                    {
                        Id = Convert.ToInt32(reader["RMMasterId"]),
                        Grade = reader["Grade"].ToString(),

                        Category = (RMCategoryEnum)Convert.ToInt16(reader["Category"]),
                        CurrentRate = (float)Convert.ToDecimal(reader["CurrentRate"])
                    };
                }
            }
            reader.Close();
            return rmCosting;
        }

        public static DataTable FetchCC_V(SqlConnection conn, Quotation quotation)
        {
            DataTable table = new DataTable();
            SqlCommand cmd = new SqlCommand("FetchCC_V", conn) 
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@quotationId", SqlDbType.Int).Value = quotation.Id;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            adapter.Fill(table);
            return table;
        }
        public static ConversionCosting FetchConvCosting(SqlConnection conn, Quotation quotation)
        {
            ConversionCosting ConvCosting = new ConversionCosting() {Operations = new ObservableCollection<Operation>() };
            SqlCommand cmd = new SqlCommand("SELECT CC_V.*, Operations.* FROM CC_V JOIN Operations ON OperationId = Operations.Id WHERE QuotationId = @quotationId", conn);
            cmd.Parameters.Add("@quotationId", SqlDbType.Int).Value =quotation.Id;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                ConvCosting.IsConvCostingPresent = true;
                Operation op = new Operation()
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    IsOutsourced = Convert.ToBoolean(reader["IsOutsourced"]),
                    OperationName = reader["Name"].ToString(),
                    StepNo = Convert.ToInt32(reader["StepNo"]), 
                    CostPerPiece = (float)Convert.ToDecimal(reader["CCPerPiece"])
                };
                ConvCosting.Operations.Add(op);
            }
            reader.Close();
            return ConvCosting;
        }
        public static IEnumerable<Quotation> FetchQuotations(SqlConnection conn, Item item)
        {
            List<Quotation> quotations= new List<Quotation>();
            SqlCommand QuotationsCommand = new SqlCommand("SELECT Quotations.* FROM Quotations WHERE ItemId = @itemId", conn);
            QuotationsCommand.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            SqlDataReader reader = QuotationsCommand.ExecuteReader();
            while (reader.Read())
            {
                Quotation quotation = new Quotation()
                {
                    Id = Convert.ToInt32(reader["Id"]), 
                    Date = Convert.ToDateTime(reader["Date"]),
                    V_Number = Convert.ToInt32(reader["V_Number"]), 
                    Q_Number = reader["Q_Number"].ToString()
                };
                quotation.RMCosting = FetchRM_V(conn, quotation);
                quotation.ConvCosting = FetchConvCosting(conn, quotation);
                quotations.Add(quotation);
            }
            reader.Close();
            return quotations;
        }
        public static Quotation FetchQuotation(SqlConnection conn, Item item, int versionNumber)
        {
            
            SqlCommand cmd = new SqlCommand("SELECT * FROM Quotations WHERE ItemId = @itemId AND V_Number = @versionNumber", conn);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            cmd.Parameters.Add("@versionNumber", SqlDbType.Int).Value = versionNumber;

            Quotation quotation = new Quotation();
            SqlDataReader reader = cmd.ExecuteReader();

            int count = 0;
            while (reader.Read())
            {
                count++;
                quotation.Id = Convert.ToInt32(reader["Id"]);
                quotation.Date = Convert.ToDateTime(reader["Date"]);
                quotation.Q_Number = reader["Q_Number"].ToString();
                quotation.V_Number = Convert.ToInt32(reader["V_Number"]);
            }
            if (count > 1) { throw new Exception("More than one rows fetched"); }
            reader.Close();
            return quotation;
        }

        // Summary:
        //      Fetches the most recent version of the Quotation, excluding the Drafts, and MF Costing
        public static int FetchVersionNumber(SqlConnection conn, Item item)
        {
            SqlCommand cmd = new SqlCommand("SELECT MAX(V_Number) FROM Quotations WHERE ItemId = @itemId AND V_Number > 0", conn);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value = item.Id;
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        // Summary:
        //      Fetches all Quotation versions of a given item
        //      Used in Reports Page- When one wants to see version by version Quotation
        public static IEnumerable<int> FetchVersions(SqlConnection conn, Item item)
        {
            List<int> versions = new List<int>();
            SqlCommand cmd = new SqlCommand("SELECT V_Number FROM Quotations WHERE ItemId = @itemId ORDER BY V_Number DESC", conn);
            cmd.Parameters.Add("@itemId", SqlDbType.Int).Value =item.Id;
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                versions.Add(Convert.ToInt32(reader["V_Number"]));
            }
            reader.Close();
            return versions;
        }
        #endregion
    }
}
