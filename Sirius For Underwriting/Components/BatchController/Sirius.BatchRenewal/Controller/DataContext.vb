Imports Sirius.Architecture.Data
Imports System.Data.SqlClient

Namespace Controller
    Friend NotInheritable Class DataContext

        Private ReadOnly mConnection As SiriusConnection

        Public Sub New()
            mConnection = SiriusConnection.FromAny(connectionString:=System.Configuration.ConfigurationManager.AppSettings("ConnectionString").ToString)
        End Sub

        Public Sub BeginTransaction()
            mConnection.BeginTransaction()
        End Sub

        Public Sub CommitTransaction()
            mConnection.CommitTransaction()
        End Sub

        Public Sub RollBackTransaction()
            mConnection.RollbackTransaction()
        End Sub

        ''' <summary>
        ''' Retrieves the renewal job id from a renewal job code
        ''' </summary>
        ''' <param name="batchRenewalJobCode"></param>
        ''' <returns>Job Detail</returns>
        Public Function RetrieveRenewalJobFromJobCode(ByVal batchRenewalJobCode As String) As JobDetail

            Dim renewalJob As JobDetail
            Dim listOfDetail As List(Of JobDetail)
            ' Get the record IDs to process.
            Using dataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_SIRRen_Get_Job_Details")
                dataCommand.Parameters.AddWithValue("@Batch_Renewal_Job_Code", batchRenewalJobCode)
                listOfDetail = mConnection.ExecuteList(Of JobDetail)(dataCommand, New Converter(Of IDataRecord, JobDetail)(
                                                                                     AddressOf ConvertDataRecorToJobDetail))
                renewalJob = listOfDetail.First()
            End Using

            Return renewalJob
        End Function


        ''' <summary>
        ''' Retrieves the policies that matches the jobs processing criteria
        ''' </summary>
        ''' <param name="jobCode">Job to execute</param>
        ''' <param name="jobTypeCode">Type of job to execute</param>
        Public Function RetrieveTargetInsuranceFilesForJob(ByVal jobCode As String, ByVal jobTypeCode As String) As List(Of TargetInsuranceFile)
            Dim targetList As List(Of TargetInsuranceFile)
            Dim procedureName As String = Nothing

            Select Case jobTypeCode
                Case "SEL"
                    procedureName = "spu_SIRRen_Get_Renewal_Selection_Policy_List"
                Case "INV"
                    procedureName = "spu_SIRRen_Get_Renewal_Invitation_Policy_List"
                Case "ACC"
                    procedureName = "spu_SIRRen_Get_Renewal_Acceptance_Policy_List"
            End Select

            If String.IsNullOrEmpty(procedureName) Then
                Return Nothing
            End If

            Using dataCommand As SiriusCommand = SiriusCommand.FromProcedure(procedureName)
                dataCommand.Parameters.AddWithValue("@Batch_Renewal_Job_Code", jobCode)
                targetList = mConnection.ExecuteList(Of TargetInsuranceFile)(dataCommand, New Converter(Of IDataRecord, TargetInsuranceFile)(
                                                                                     AddressOf ConvertDataRecorToTargetInsuranceFile))
            End Using
            Return targetList
        End Function


        ''' <summary>
        ''' Inserts the batch into the database
        ''' </summary>
        ''' <param name="insuranceFolderCount">Number of insurance folders in this batch job</param>
        ''' <param name="batchReference">Batch Reference</param>
        ''' <param name="sInterfaceCode">Batch Reference</param>
        ''' <returns>Batch Id for from the database</returns>
        Public Function ExecuteInsertBatch(ByVal insuranceFolderCount As Integer, ByVal batchReference As String, ByVal sInterfaceCode As String) As Integer?
            Dim batchId As Integer?
            Dim iBatchTypeId As Integer
            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Get_Batch_type_id_From_Code")
                cmd.AddInParameter("@code", SqlDbType.VarChar, 255).Value = "REN"
                dt = mConnection.ExecuteDataTable(cmd)
            End Using
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                iBatchTypeId = Convert.ToInt32(dr.Item("batch_type_id"))

            End If

            Using dataCommand As SiriusCommand = SiriusCommand.FromProcedure("spu_ACT_Add_Batch")
                Dim batchIdParam As SqlParameter = dataCommand.AddOutParameter("@batch_id", SqlDbType.Int)

                dataCommand.Parameters.Add(MakeParameter("@company_id", SqlDbType.SmallInt, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@batchstatus_id", SqlDbType.SmallInt, Nothing, 4))
                ' Ready
                dataCommand.Parameters.Add(MakeParameter("@user_id", SqlDbType.SmallInt, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@batch_ref", SqlDbType.VarChar, 25, String.Empty))
                dataCommand.Parameters.Add(MakeParameter("@created_date", SqlDbType.DateTime, Nothing, Date.Now()))
                dataCommand.Parameters.Add(MakeParameter("@authorised_date", SqlDbType.Date, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@accounting_date", SqlDbType.Date, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@comment", SqlDbType.VarChar, 60, batchReference))
                dataCommand.Parameters.Add(MakeParameter("@batch_type_id", SqlDbType.Int, Nothing, iBatchTypeId))
                dataCommand.Parameters.Add(MakeParameter("@batch_source_id", SqlDbType.Int, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@xml_object", SqlDbType.VarChar, 4000, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@exportdate", SqlDbType.Date, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@reexportdate", SqlDbType.Date, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@mediatypeid", SqlDbType.Int, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@totalamount", SqlDbType.Money, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@totaltransactions", SqlDbType.Int, Nothing,
                                                         insuranceFolderCount))
                dataCommand.Parameters.Add(MakeParameter("@importeddate", SqlDbType.Date, Nothing, DateTime.Now))
                dataCommand.Parameters.Add(MakeParameter("@rejectamount", SqlDbType.Decimal, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@rejecttransactions", SqlDbType.Int, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@closeddate", SqlDbType.Date, Nothing, DBNull.Value))
                dataCommand.Parameters.Add(MakeParameter("@interfacecode", SqlDbType.VarChar, 30, sInterfaceCode))
                dataCommand.Parameters.Add(MakeParameter("@autoclose", SqlDbType.TinyInt, Nothing, DBNull.Value))

                mConnection.ExecuteNonQuery(dataCommand)
                batchId = CInt(batchIdParam.Value)
            End Using
            Return batchId
        End Function

        ''' <summary>
        ''' Adds the job to the dbo.Batch_Renewal_Job_Run_Insurance_Folder table
        ''' </summary>
        ''' <param name="batchId">Batch Job</param>
        ''' <param name="insuranceFolderCnt">Insurance Folder</param>
        ''' <param name="recalculateCommission">Value indicating if the commission should be recalculated</param>
        ''' <param name="recalculateFees">Value indicating if the fees should be recaclulated</param>
        ''' <param name="recalculateTaxes">Value indicating if the taxes should be recalculated</param>
        ''' <param name="insuranceFileCnt">Insurance file that must be processed</param>
        ''' <param name="jobId">The batch renewal job id</param>
        ''' <remarks></remarks>
        Public Sub ExecuteInsertInsuranceFolder(ByVal batchId As Integer,
                                                        ByVal insuranceFolderCnt As Integer,
                                                        ByVal recalculateCommission As Boolean,
                                                        ByVal recalculateFees As Boolean,
                                                        ByVal recalculateTaxes As Boolean,
                                                        ByVal insuranceFileCnt As Integer?,
                                                        ByVal jobId As Integer)


            ' Save the insurance folder details to the DB.

            Using _
                dataCommand As SiriusCommand =
                    SiriusCommand.FromProcedure("spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_add")
                dataCommand.Parameters.Add(MakeParameter("@batch_id", SqlDbType.Int, Nothing, batchId))
                dataCommand.Parameters.Add(MakeParameter("@insurance_folder_cnt", SqlDbType.Int, Nothing,
                                                         insuranceFolderCnt))
                dataCommand.Parameters.Add(MakeParameter("@batch_renewal_job_id", SqlDbType.Int, Nothing, jobId))
                dataCommand.Parameters.Add(MakeParameter("@recalculate_commission", SqlDbType.Bit, Nothing,
                                                         recalculateCommission))
                dataCommand.Parameters.Add(MakeParameter("@recalculate_fees", SqlDbType.Bit, Nothing, recalculateFees))
                dataCommand.Parameters.Add(MakeParameter("@recalculate_taxes", SqlDbType.Bit, Nothing, recalculateTaxes))
                If insuranceFileCnt.HasValue Then
                    dataCommand.Parameters.Add(MakeParameter("@insurance_file_cnt", SqlDbType.Int, Nothing,
                                                             insuranceFileCnt))
                End If
                mConnection.ExecuteNonQuery(dataCommand)
            End Using
        End Sub

        ''' <summary>
        ''' Inserts a risk into the risk processing tables
        ''' </summary>
        ''' <param name="riskFolderCnt">Risk that must be processed</param>
        ''' <param name="rerate">Value indicating whether the risk must be rated</param>
        ''' <param name="recalculateReinsurance">Value indicating whether reinsurance must be recalculated</param>
        ''' <param name="recalculateRiskFees">Value indicating whether risk fees must be recalculated</param>
        ''' <param name="recalculateRiskTaxes">Value indicating whether risk taxes must be recalculated</param>
        ''' <param name="batchId">Batch the this belongs to</param>
        ''' <param name="insuranceFolderCnt">Insurance folder that this risk is on</param>
        Public Sub InsertRisk(ByVal riskFolderCnt As Integer,
                                  ByVal rerate As Boolean,
                                  ByVal recalculateReinsurance As Boolean,
                                  ByVal recalculateRiskFees As Boolean,
                                  ByVal recalculateRiskTaxes As Boolean,
                                  ByVal batchId As Integer,
                                  ByVal insuranceFolderCnt As Integer)

            'Get RISK Attributes


            ' Save the risk details to the DB.
            Using _
                dataCommand As SiriusCommand =
                    SiriusCommand.FromProcedure("spu_SIR_Batch_Renewal_Job_Run_Risk_add")

                dataCommand.Parameters.Add(MakeParameter("@batch_id", SqlDbType.Int, Nothing, batchId))
                dataCommand.Parameters.Add(MakeParameter("@insurance_folder_cnt", SqlDbType.Int, Nothing,
                                                         insuranceFolderCnt))
                dataCommand.Parameters.Add(MakeParameter("@risk_folder_cnt", SqlDbType.Int, Nothing, riskFolderCnt))
                dataCommand.Parameters.Add(MakeParameter("@rerate", SqlDbType.Bit, Nothing, rerate))
                dataCommand.Parameters.Add(MakeParameter("@recalculate_reinsurance", SqlDbType.Bit, Nothing,
                                                         recalculateReinsurance))
                dataCommand.Parameters.Add(MakeParameter("@recalculate_fees", SqlDbType.Bit, Nothing,
                                                         recalculateRiskFees))
                dataCommand.Parameters.Add(MakeParameter("@recalculate_taxes", SqlDbType.Bit, Nothing,
                                                         recalculateRiskTaxes))
                mConnection.ExecuteNonQuery(dataCommand)
            End Using
        End Sub

        ''' <summary>
        ''' Loads a list of insurance folders that must still be executeed
        ''' </summary>
        ''' <param name="batchId">Batch with outstanding items</param>
        Public Function LoadInsuranceFoldersStillOutstandingForBatch(batchId As Integer) As List(Of Integer)
            Dim listOfInsuranceFolders As List(Of Integer)
            Using _
                dataCommand As SiriusCommand =
                    SiriusCommand.FromProcedure("spu_SIR_Batch_Renewal_Job_Run_Insurance_Folder_Outstanding_sel")
                dataCommand.Parameters.Add(MakeParameter("@batch_id", SqlDbType.Int, Nothing, batchId))
                listOfInsuranceFolders = mConnection.ExecuteList(Of Integer)(dataCommand,
                                                                             New Converter(Of IDataRecord, Integer)(
                                                                                 AddressOf ConvertDataRecordToInteger))
            End Using

            Return listOfInsuranceFolders
        End Function


        ''' <summary>
        ''' Creates a paramater
        ''' </summary>
        ''' <param name="parameterName">Paramater name as in stored procedure</param>
        ''' <param name="dbType">Data type of paramater</param>
        ''' <param name="size">Size of the paramater</param>
        ''' <param name="value">value of the paramater</param>
        Private Shared Function MakeParameter(ByVal parameterName As String, ByVal dbType As SqlDbType, ByVal size As Integer?, ByVal value As Object) As SqlParameter
            Dim parameter As SqlParameter
            If size.HasValue Then
                parameter = New SqlParameter(parameterName, dbType, size.Value)
            Else
                parameter = New SqlParameter(parameterName, dbType)
            End If
            parameter.Value = value
            Return parameter
        End Function

        ''' <summary>
        ''' Converts the a data record to a JobDetail object
        ''' </summary>
        ''' <param name="record">DataRecord from the datareader</param>
        Private Shared Function ConvertDataRecorToJobDetail(record As IDataRecord) As JobDetail
            Dim jobDetail As New JobDetail

            jobDetail.JobId = record.GetInt32(0)
            jobDetail.Code = record.GetString(1)
            jobDetail.JobTypeId = record.GetInt32(3)
            jobDetail.JobTypeCode = record.GetString(4)
            jobDetail.JobDescription = record.GetString(8)
            jobDetail.Batch_Job_Type_Description = record.GetString(9)
            jobDetail.Run_Renewal_Extended_Rule = record.GetByte(10)
            Return jobDetail
        End Function

        ''' <summary>
        ''' Sets the internal fields from DataRecord
        ''' </summary>
        ''' <param name="record">Data Row</param>
        Private Shared Function ConvertDataRecorToTargetInsuranceFile(record As IDataRecord) As TargetInsuranceFile
            Dim targetInsuranceFile As New TargetInsuranceFile

            targetInsuranceFile.InsuranceFileCnt = record.GetInt32(0)
            targetInsuranceFile.InsuranceFolderCnt = record.GetInt32(1)

            Return targetInsuranceFile
        End Function

        ''' <summary>
        ''' Converts the a record from a data reader into an integer value
        ''' </summary>
        ''' <param name="record">DataRecord from the datareader</param>
        Private Shared Function ConvertDataRecordToInteger(record As IDataRecord) As Integer
            Return record.GetInt32(0)
        End Function
        ''' <summary>
        ''' UpdateBatchTask
        ''' </summary>
        ''' <param name="sBatchStatusCode"></param>
        ''' <param name="nBatchId"></param>
        ''' <param name="sFileName"></param>
        Public Sub UpdateBatchTask(ByVal sBatchStatusCode As String, ByVal nBatchId As Integer, ByVal sFileName As String, ByVal sBatchDescription As String)
            Dim dt As DataTable = Nothing

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_Update_RenewalBatchTask")
                cmd.AddInParameter("@Batch_Id", SqlDbType.Int).Value = nBatchId
                cmd.AddInParameter("@FileName", SqlDbType.VarChar, 255).Value = sFileName
                cmd.AddInParameter("@batchstatusCode", SqlDbType.VarChar, 255).Value = sBatchStatusCode
                cmd.AddInParameter("@BatchDescription", SqlDbType.VarChar, 255).Value = sBatchDescription
                dt = mConnection.ExecuteDataTable(cmd)
            End Using
        End Sub
    End Class
End Namespace