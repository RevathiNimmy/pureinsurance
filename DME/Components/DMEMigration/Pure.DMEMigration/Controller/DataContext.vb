Imports Sirius.Architecture.Data
Imports System.Data.SqlClient

Namespace Controller
    Friend NotInheritable Class DataContext

        Private ReadOnly m_Connection As SiriusConnection

        Public Sub New()
            m_Connection = SiriusConnection.FromSirius()
        End Sub

        Public Sub BeginTransaction()
            m_Connection.BeginTransaction()
        End Sub

        Public Sub CommitTransaction()
            m_Connection.CommitTransaction()
        End Sub

        Public Sub RollBackTransaction()
            m_Connection.RollbackTransaction()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iBatchId"></param>
        ''' <param name="iFolderNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RetrieveTargetDocsForBatch(ByVal iBatchId As Integer, ByVal iFolderNum As Integer) As List(Of Integer)
            Dim iTargetList As List(Of Integer)

            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Upd_Folder_In_Batch")
                cmdDataCommand.Parameters.AddWithValue("@batch_Id", iBatchId)
                cmdDataCommand.Parameters.AddWithValue("@folder_Num", iFolderNum)
                iTargetList = m_Connection.ExecuteList(Of Integer)(cmdDataCommand, New Converter(Of IDataRecord, Integer)(
                                                                                     AddressOf ConvertDataRecordToInteger))
            End Using
            Return iTargetList
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iBatchId"></param>
        ''' <param name="iDocNums"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function RetrieveTargetDocsForBatch(ByVal iBatchId As Integer, ByVal iDocNums() As Integer) As List(Of Integer)
            Dim iTargetList As List(Of Integer)

            For Each iDocNum As Integer In iDocNums
                Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Upd_Doc_In_Batch")
                    cmdDataCommand.Parameters.Add(MakeParameter("@doc_Num", SqlDbType.Int, Nothing, iDocNum))
                    cmdDataCommand.Parameters.Add(MakeParameter("@batch_id", SqlDbType.Int, 20, iBatchId))
                    m_Connection.ExecuteNonQuery(cmdDataCommand)
                End Using
            Next

            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Sel_DME_Batch")
                cmdDataCommand.Parameters.AddWithValue("@batch_Id", iBatchId)
                iTargetList = m_Connection.ExecuteList(Of Integer)(cmdDataCommand, New Converter(Of IDataRecord, Integer)(
                                                                                     AddressOf ConvertDataRecordToInteger))
            End Using

            Return iTargetList
        End Function

        ''' <summary>
        ''' Inserts the batch into the database
        ''' </summary>
        ''' <param name="sBatchReference"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExecuteInsertBatch(ByVal sBatchReference As String) As Integer
            Dim iBatchId As Integer
            Dim iBatchTypeId As Integer
            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Batch_type_id_From_Code")
                cmd.AddInParameter("@code", SqlDbType.VarChar, 255).Value = "DMEMIG"
                dt = m_Connection.ExecuteDataTable(cmd)
            End Using
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                iBatchTypeId = Convert.ToInt32(dr.Item("batch_type_id"))
            End If

            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Add_Batch")
                Dim batchIdParam As SqlParameter = cmdDataCommand.AddOutParameter("@batch_id", SqlDbType.Int)

                cmdDataCommand.Parameters.Add(MakeParameter("@company_id", SqlDbType.SmallInt, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@batchstatus_id", SqlDbType.SmallInt, Nothing, 4))
                ' Ready
                cmdDataCommand.Parameters.Add(MakeParameter("@user_id", SqlDbType.SmallInt, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@batch_ref", SqlDbType.VarChar, 25, String.Empty))
                cmdDataCommand.Parameters.Add(MakeParameter("@created_date", SqlDbType.Date, Nothing, Date.Now()))
                cmdDataCommand.Parameters.Add(MakeParameter("@authorised_date", SqlDbType.Date, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@accounting_date", SqlDbType.Date, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@comment", SqlDbType.VarChar, 60, sBatchReference))
                cmdDataCommand.Parameters.Add(MakeParameter("@batch_type_id", SqlDbType.Int, Nothing, iBatchTypeId))
                cmdDataCommand.Parameters.Add(MakeParameter("@batch_source_id", SqlDbType.Int, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@xml_object", SqlDbType.VarChar, 4000, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@exportdate", SqlDbType.Date, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@reexportdate", SqlDbType.Date, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@mediatypeid", SqlDbType.Int, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@totalamount", SqlDbType.Money, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@totaltransactions", SqlDbType.Int, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@importeddate", SqlDbType.Date, Nothing, DateTime.Now))
                cmdDataCommand.Parameters.Add(MakeParameter("@rejectamount", SqlDbType.Decimal, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@rejecttransactions", SqlDbType.Int, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@closeddate", SqlDbType.Date, Nothing, DBNull.Value))
                cmdDataCommand.Parameters.Add(MakeParameter("@interfacecode", SqlDbType.VarChar, 30, "DME_Migration"))
                cmdDataCommand.Parameters.Add(MakeParameter("@autoclose", SqlDbType.TinyInt, Nothing, DBNull.Value))

                m_Connection.ExecuteNonQuery(cmdDataCommand)
                iBatchId = CInt(batchIdParam.Value)
            End Using

            Return iBatchId
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iDocNum"></param>
        ''' <param name="sArchiveDoc"></param>
        ''' <param name="sTargetDoc"></param>
        ''' <param name="sFullDMEPath"></param>
        ''' <remarks></remarks>
        Public Sub RetreiveDocInfo(ByVal iDocNum As Integer, ByRef sArchiveDoc As String, ByRef sTargetDoc As String, ByRef sFullDMEPath As String, ByRef sCreateBy As String, ByRef sCreateDate As DateTime)

            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Sel_Archived_Doc")
                Dim dmeParam As SqlParameter = cmdDataCommand.AddOutParameter("@dmeDoc", SqlDbType.VarChar, 255)
                Dim sharePointParam As SqlParameter = cmdDataCommand.AddOutParameter("@sharePointDoc", SqlDbType.VarChar, 255)
                Dim fullPathParam As SqlParameter = cmdDataCommand.AddOutParameter("@fullPath", SqlDbType.VarChar, 255)
                Dim CreatedBy As SqlParameter = cmdDataCommand.AddOutParameter("@created_by", SqlDbType.VarChar, 255)
                Dim CreatedDate As SqlParameter = cmdDataCommand.AddOutParameter("@created_date", SqlDbType.DateTime)

                cmdDataCommand.Parameters.Add(MakeParameter("@doc_Num", SqlDbType.Int, Nothing, iDocNum))
                m_Connection.ExecuteNonQuery(cmdDataCommand)
                sArchiveDoc = CStr(dmeParam.Value)
                sTargetDoc = CStr(sharePointParam.Value)
                sFullDMEPath = CStr(fullPathParam.Value)
                sCreateBy = Convert.ToString(CreatedBy.Value)
                sCreateDate = CreatedDate.Value
            End Using

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iFolderNum"></param>
        ''' <param name="sFullDMEPath"></param>
        ''' <remarks></remarks>
        Public Sub RetreiveFolderInfo(ByVal iFolderNum As Integer, ByRef sFullDMEPath As String)

            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Sel_folder_info")
                Dim fullPathParam As SqlParameter = cmdDataCommand.AddOutParameter("@folderPath", SqlDbType.VarChar, 255)

                cmdDataCommand.Parameters.Add(MakeParameter("@folder_Num", SqlDbType.Int, Nothing, iFolderNum))
                m_Connection.ExecuteNonQuery(cmdDataCommand)
                sFullDMEPath = CStr(fullPathParam.Value)
            End Using

        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="iDocNum"></param>
        ''' <param name="sStatusCode"></param>
        ''' <remarks></remarks>
        Public Sub UpdateMigrationStatus(ByVal iDocNum As Integer, ByVal sStatusCode As String)

            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Upd_Doc_Migration_Status")
                cmdDataCommand.Parameters.Add(MakeParameter("@doc_Num", SqlDbType.Int, Nothing, iDocNum))
                cmdDataCommand.Parameters.Add(MakeParameter("@statusCode", SqlDbType.VarChar, 20, sStatusCode))
                m_Connection.ExecuteNonQuery(cmdDataCommand)
            End Using

        End Sub

        ''' <summary>
        ''' Loads a list of insurance folders that must still be executeed
        ''' </summary>
        ''' <param name="iBatchId"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function LoadOutstandingDocsForBatch(iBatchId As Integer) As List(Of Integer)
            Dim iListOfDocs As List(Of Integer)
            Using cmdDataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_Sel_DME_Batch")
                cmdDataCommand.Parameters.Add(MakeParameter("@batch_id", SqlDbType.Int, Nothing, iBatchId))
                iListOfDocs = m_Connection.ExecuteList(Of Integer)(cmdDataCommand,
                                                                             New Converter(Of IDataRecord, Integer)(
                                                                                 AddressOf ConvertDataRecordToInteger))
            End Using

            Return iListOfDocs
        End Function


        ''' <summary>
        ''' Creates a paramater
        ''' </summary>
        ''' <param name="sParameterName">Paramater name as in stored procedure</param>
        ''' <param name="dbType">Data type of paramater</param>
        ''' <param name="iSize">Size of the paramater</param>
        ''' <param name="oValue">value of the paramater</param>
        Private Shared Function MakeParameter(ByVal sParameterName As String, ByVal dbType As SqlDbType, ByVal iSize As Integer?, ByVal oValue As Object) As SqlParameter
            Dim parameter As SqlParameter
            If iSize.HasValue Then
                parameter = New SqlParameter(sParameterName, dbType, iSize.Value)
            Else
                parameter = New SqlParameter(sParameterName, dbType)
            End If
            parameter.Value = oValue
            Return parameter
        End Function

        ''' <summary>
        ''' Converts the a record from a data reader into an integer value
        ''' </summary>
        ''' <param name="record">DataRecord from the datareader</param>
        Private Shared Function ConvertDataRecordToInteger(record As IDataRecord) As Integer
            Return record.GetInt32(0)
        End Function
    End Class
End Namespace
