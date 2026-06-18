Imports SharedFiles

''' <summary>
''' Batch Quote Deletion - Direct Database Access (No SOAP Dependency)
''' Migrated from SOAP web service to direct stored procedure calls
''' </summary>
Public NotInheritable Class QuoteDeletion
    Const ACClass As String = "QuoteDeletion"

#Region " Private Declarations "
    Private m_lReturn As Long
    Protected m_oDatabase As dPMDAO.Database = Nothing
    Private m_sUserName As String = String.Empty
    Private lInsuranceFileCnt As Long = 0
#End Region

#Region "Main Methods"

    ''' <summary>
    ''' Processes batch quote deletion without SOAP dependency
    ''' </summary>
    ''' <param name="v_sUserName">Username for audit logging</param>
    ''' <param name="v_sPassword">Not used (kept for backward compatibility)</param>
    ''' <param name="r_lQuotesDeletd">Output: Number of successfully deleted quotes</param>
    ''' <returns>PMEReturnCode.PMTrue on success, PMEReturnCode.PMError on failure</returns>
    Public Function QuoteDeletion(ByVal v_sUserName As String, ByVal v_sPassword As String, ByRef r_lQuotesDeletd As Long) As Long

        Dim lResult As Long
        Dim lStart As Long = 0
        Dim lIterations As Long = 0

        Try
            ' Initialize
            Dim sCurrentDate As Date
            Dim vQuotesForAutoDeleteArray(,) As Object = Nothing
            Dim lCnt As Long

            lResult = gPMConstants.PMEReturnCode.PMTrue
            m_sUserName = v_sUserName
            r_lQuotesDeletd = 0

            ' Get current date
            sCurrentDate = Convert.ToDateTime(Now).ToLongDateString

            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine("    Batch Quote Deletion Process")
            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine($"Started at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
            OutputLine($"User: {m_sUserName}")
            OutputLine($"Date: {sCurrentDate}")
            OutputLine("───────────────────────────────────────────────────────────────")
            OutputLine()

            ' Step 1: Get quotes eligible for auto-deletion
            OutputLine("Step 1: Retrieving quotes eligible for auto-deletion...")
            m_lReturn = GetQuotesForAutoDelete(v_dtCurrentDate:=sCurrentDate, r_vResultArray:=vQuotesForAutoDeleteArray)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to retrieve quotes for auto-deletion")
            End If

            If Not IsArray(vQuotesForAutoDeleteArray) Then
                OutputLine("ℹ No Quotes Found for Auto Deletion.")
                OutputLine()
                Return lResult
            End If

            ' Display found quotes
            Dim lTotalQuotes As Long = UBound(vQuotesForAutoDeleteArray, 2) - LBound(vQuotesForAutoDeleteArray, 2) + 1
            OutputLine($"✓ Found {lTotalQuotes} quote(s) to process")
            OutputLine()
            OutputLine("Step 2: Processing quote deletions...")
            OutputLine("───────────────────────────────────────────────────────────────")

ContinueQuoteDeletion:
            ' Calculate start position for retry logic
            lStart = LBound(vQuotesForAutoDeleteArray, 2) + lIterations
            lIterations = lIterations + 1

            ' Step 2: Process each quote for deletion
            For lCnt = lStart To UBound(vQuotesForAutoDeleteArray, 2)

                lInsuranceFileCnt = ToSafeLong(vQuotesForAutoDeleteArray(0, lCnt), 0)

                OutputLine($"  Processing Insurance File Key: {lInsuranceFileCnt}...")

                Try

                    Dim bDeleted As Boolean = ExecuteDeletePolicy(lInsuranceFileCnt)

                    If bDeleted Then
                        ' Success
                        r_lQuotesDeletd = r_lQuotesDeletd + 1
                        lIterations = lIterations + 1

                        OutputLine($"    ✓ Successfully deleted Insurance File Key {lInsuranceFileCnt}")

                        ' Log success to audit table
                        m_lReturn = InsertInsuranceFileDeleteLog(lInsuranceFileCnt, 1, "Successfully deleted via batch process")
                    Else
                        ' Failure (stored procedure returned error)
                        lIterations = lIterations + 1

                        OutputLine($"    ✗ Failed to delete Insurance File Key {lInsuranceFileCnt} - stored procedure validation failed")

                        ' Log failure to audit table
                        m_lReturn = InsertInsuranceFileDeleteLog(lInsuranceFileCnt, 0, "Delete stored procedure returned error or validation failed")
                    End If

                Catch ex As Exception
                    ' Exception during deletion - log and continue with next quote
                    OutputLine($"    ✗ Exception deleting Insurance File Key {lInsuranceFileCnt}: {ex.Message}")

                    m_lReturn = InsertInsuranceFileDeleteLog(lInsuranceFileCnt, 0, $"Exception: {ex.Message}")

                    ' Continue processing remaining quotes
                    GoTo ContinueQuoteDeletion
                End Try

            Next

            OutputLine()
            OutputLine("───────────────────────────────────────────────────────────────")
            OutputLine("✓ Batch processing completed successfully")

        Catch ex As Exception
            lResult = gPMConstants.PMEReturnCode.PMError
            OutputLine()
            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine("✗ CRITICAL ERROR")
            OutputLine("═══════════════════════════════════════════════════════════════")
            OutputLine($"Error: {ex.Message}")
            OutputLine($"Stack Trace: {ex.StackTrace}")
            OutputLine("═══════════════════════════════════════════════════════════════")
        End Try

        ' Final summary
        OutputLine()
        OutputLine("═══════════════════════════════════════════════════════════════")
        OutputLine("    Batch Quote Deletion Summary")
        OutputLine("═══════════════════════════════════════════════════════════════")
        OutputLine($"Total Quotes Deleted: {r_lQuotesDeletd}")
        OutputLine($"Completed at: {DateTime.Now:yyyy-MM-dd HH:mm:ss}")
        OutputLine($"Status: {If(lResult = gPMConstants.PMEReturnCode.PMTrue, "SUCCESS", "FAILED")}")
        OutputLine("═══════════════════════════════════════════════════════════════")

        Return lResult
    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Executes the delete policy stored procedure directly
    ''' The stored procedure (spu_DeletePolicy) handles all validations internally
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt">Insurance file key to delete</param>
    ''' <returns>True if deletion successful, False otherwise</returns>
    Private Function ExecuteDeletePolicy(ByVal v_lInsuranceFileCnt As Long) As Boolean
        Const kMethod As String = "ExecuteDeletePolicy"
        Dim bSuccess As Boolean = False

        Try
            ' Clear previous parameters
            m_oDatabase.Parameters.Clear()

            ' Add insurance file key parameter
            AddParameterLite(m_oDatabase,
                            "nInsuranceFileCnt",
                            v_lInsuranceFileCnt,
                            PMEParameterDirection.PMParamInput,
                            PMEDataType.PMInteger)

            ' Execute the deletion stored procedure
            ' Note: spu_DeletePolicy handles all validations internally:
            ' 1. Branch code validation (via insurance file lookup)
            ' 2. Insurance file key validation
            ' 3. Quote status check (ensures it's a quote, not a policy)
            ' 4. Cascade deletion logic (deletes related records)
            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:="spu_DeletePolicy",
                sSQLName:="DeletePolicy",
                bStoredProcedure:=True,
                lNumberRecords:=gPMConstants.PMAllRecords)

            ' Check if stored procedure execution was successful
            bSuccess = (m_lReturn = PMEReturnCode.PMTrue)

            If Not bSuccess Then
                OutputLine($"    ⚠ Warning: Stored procedure returned error for insurance file {v_lInsuranceFileCnt}")
            End If

        Catch ex As Exception
            OutputLine($"    ✗ Exception in {kMethod} for insurance file {v_lInsuranceFileCnt}: {ex.Message}")
            bSuccess = False
        End Try

        Return bSuccess
    End Function

    ''' <summary>
    ''' Retrieves quotes eligible for auto-deletion based on current date
    ''' </summary>
    ''' <param name="v_dtCurrentDate">Current date for deletion criteria</param>
    ''' <param name="r_vResultArray">Output: Array of quotes to delete</param>
    ''' <returns>PMEReturnCode.PMTrue on success, PMEReturnCode.PMError on failure</returns>
    Private Function GetQuotesForAutoDelete(ByVal v_dtCurrentDate As Date, ByRef r_vResultArray(,) As Object) As Long
        Const kMethod As String = "GetQuotesForAutoDelete"
        Dim lResult As Long = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            ' Add current date parameter
            AddParameterLite(m_oDatabase,
                            "Current_Date",
                            v_dtCurrentDate,
                            PMEParameterDirection.PMParamInput,
                            PMEDataType.PMDate)

            ' Execute stored procedure to get quotes for auto-deletion
            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACGetQuotesForAutoDeleteSQL,
                sSQLName:=ACGetQuotesForAutoDeleteName,
                bStoredProcedure:=ACGetQuotesForAutoDeleteStored,
                lNumberRecords:=gPMConstants.PMAllRecords,
                vResultArray:=r_vResultArray)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute spu_get_quotes_for_auto_delete")
            End If

        Catch ex As Exception
            OutputLine($"✗ Error in {kMethod}: {ex.Message}")
            lResult = PMEReturnCode.PMError
        End Try

        Return lResult
    End Function

    ''' <summary>
    ''' Inserts a record into the insurance file delete log for audit purposes
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt">Insurance file key</param>
    ''' <param name="v_lStatus">1 = Success, 0 = Failure</param>
    ''' <param name="v_sErrorDescription">Description or error message</param>
    ''' <returns>PMEReturnCode.PMTrue on success, PMEReturnCode.PMError on failure</returns>
    Private Function InsertInsuranceFileDeleteLog(ByVal v_lInsuranceFileCnt As Long, ByVal v_lStatus As Long, ByVal v_sErrorDescription As String) As Long
        Const kMethod As String = "InsertInsuranceFileDeleteLog"

        InsertInsuranceFileDeleteLog = PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            ' Normalize status value (1 = Success, 0 = Failure)
            If v_lStatus <> 1 Then
                v_lStatus = 0
            End If

            ' Add parameters for audit log
            AddParameterLite(m_oDatabase, "InsuranceFileCnt", v_lInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "Status", v_lStatus, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "FailureDescription", v_sErrorDescription, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute stored procedure to insert audit log record
            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACAddInsuranceFileDeleteLogSQL,
                sSQLName:=ACAddInsuranceFileDeleteLogName,
                bStoredProcedure:=ACAddInsuranceFileDeleteLogStored,
                lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log warning but don't fail the entire process
                OutputLine($"    ⚠ Warning: Failed to insert audit log for insurance file {v_lInsuranceFileCnt}")
            End If

        Catch ex As Exception
            ' Audit logging failure should not stop the batch process
            OutputLine($"    ⚠ Warning in {kMethod}: {ex.Message}")
            InsertInsuranceFileDeleteLog = PMEReturnCode.PMError
        End Try

        Return InsertInsuranceFileDeleteLog
    End Function

#End Region

#Region "Report Methods"

    ''' <summary>
    ''' Retrieves successful quote version deletions for reporting
    ''' </summary>
    Private Function GetDeletionReport(ByVal v_dtDateFrom As Date, ByVal v_dtDateTo As Date, ByRef r_vResultArray(,) As Object) As Long
        Const kMethod As String = "GetDeletionReport"
        Dim lResult As Long = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            AddParameterLite(m_oDatabase, "DateFrom", v_dtDateFrom, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "DateTo", v_dtDateTo, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACGetDeletionReportSQL,
                sSQLName:=ACGetDeletionReportName,
                bStoredProcedure:=ACGetDeletionReportStored,
                lNumberRecords:=gPMConstants.PMAllRecords,
                vResultArray:=r_vResultArray)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute " & ACGetDeletionReportSQL)
            End If

        Catch ex As Exception
            OutputLine($"✗ Error in {kMethod}: {ex.Message}")
            lResult = PMEReturnCode.PMError
        End Try

        Return lResult
    End Function

    ''' <summary>
    ''' Retrieves failed quote version deletions for reporting
    ''' </summary>
    Private Function GetDeletionFailureReport(ByVal v_dtDateFrom As Date, ByVal v_dtDateTo As Date, ByRef r_vResultArray(,) As Object) As Long
        Const kMethod As String = "GetDeletionFailureReport"
        Dim lResult As Long = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            AddParameterLite(m_oDatabase, "DateFrom", v_dtDateFrom, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "DateTo", v_dtDateTo, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACGetDeletionFailureReportSQL,
                sSQLName:=ACGetDeletionFailureReportName,
                bStoredProcedure:=ACGetDeletionFailureReportStored,
                lNumberRecords:=gPMConstants.PMAllRecords,
                vResultArray:=r_vResultArray)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to execute " & ACGetDeletionFailureReportSQL)
            End If

        Catch ex As Exception
            OutputLine($"✗ Error in {kMethod}: {ex.Message}")
            lResult = PMEReturnCode.PMError
        End Try

        Return lResult
    End Function

    ''' <summary>
    ''' Generates CSV reports for successful and failed deletions
    ''' </summary>
    Public Sub GenerateCsvReports(ByVal v_dtDateFrom As Date, ByVal v_dtDateTo As Date, ByVal v_sOutputPath As String)
        Dim vSuccessArray(,) As Object = Nothing
        Dim vFailureArray(,) As Object = Nothing
        Dim sTimestamp As String = DateTime.Now.ToString("yyyyMMdd_HHmmss")

        ' Create folder structure with error handling
        Dim sSuccessFolder As String = IO.Path.Combine(v_sOutputPath, "Quote History Version Deletion Report")
        Dim sFailureFolder As String = IO.Path.Combine(v_sOutputPath, "Quote History Version Deletion Failure Report")

        Try
            If Not IO.Directory.Exists(sSuccessFolder) Then IO.Directory.CreateDirectory(sSuccessFolder)
            If Not IO.Directory.Exists(sFailureFolder) Then IO.Directory.CreateDirectory(sFailureFolder)
        Catch ex As Exception
            OutputLine($"  ✗ Error creating report directories: {ex.Message}")
            OutputLine("  ✗ CSV report generation skipped.")
            Return
        End Try

        OutputLine()
        OutputLine("Step 3: Generating CSV reports...")
        OutputLine("───────────────────────────────────────────────────────────────")

        ' Success Report
        Try
            m_lReturn = GetDeletionReport(v_dtDateFrom, v_dtDateTo, vSuccessArray)

            Dim sSuccessFile As String = IO.Path.Combine(sSuccessFolder, $"Quote_History_Version_Deletion_Report_{sTimestamp}.csv")

            Using sw As New IO.StreamWriter(sSuccessFile, False, Text.Encoding.UTF8)
                sw.WriteLine("Quote History Version Deletion Report")
                sw.WriteLine($"Run Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                sw.WriteLine()
                sw.WriteLine("Quote Number,Agent Code,Agent Name,Product,Name of Policyholder,Date")

                Dim lCount As Long = 0
                If m_lReturn = PMEReturnCode.PMTrue AndAlso IsArray(vSuccessArray) Then
                    For i As Long = LBound(vSuccessArray, 2) To UBound(vSuccessArray, 2)
                        sw.WriteLine(
                            EscapeCsv(ToSafeString(vSuccessArray(0, i))) & "," &
                            EscapeCsv(ToSafeString(vSuccessArray(1, i))) & "," &
                            EscapeCsv(ToSafeString(vSuccessArray(2, i))) & "," &
                            EscapeCsv(ToSafeString(vSuccessArray(3, i))) & "," &
                            EscapeCsv(ToSafeString(vSuccessArray(4, i))) & "," &
                            EscapeCsv(ToSafeString(vSuccessArray(5, i))))
                        lCount += 1
                    Next
                End If

                sw.WriteLine()
                sw.WriteLine($"Total Quote Versions Deleted:,{lCount}")
            End Using

            OutputLine($"  ✓ Success report: {sSuccessFile}")

        Catch ex As Exception
            OutputLine($"  ✗ Error generating success report: {ex.Message}")
        End Try

        ' Failure Report
        Try
            m_lReturn = GetDeletionFailureReport(v_dtDateFrom, v_dtDateTo, vFailureArray)

            Dim sFailureFile As String = IO.Path.Combine(sFailureFolder, $"Quote_History_Version_Deletion_Failure_Report_{sTimestamp}.csv")

            Using sw As New IO.StreamWriter(sFailureFile, False, Text.Encoding.UTF8)
                sw.WriteLine("Quote History Version Deletion Failure Report")
                sw.WriteLine($"Run Date: {DateTime.Now:dd/MM/yyyy HH:mm:ss}")
                sw.WriteLine()
                sw.WriteLine("Quote Number,Agent Code,Agent Name,Product,Name of Policyholder,Reason for Failure,Date")

                Dim lCount As Long = 0
                If m_lReturn = PMEReturnCode.PMTrue AndAlso IsArray(vFailureArray) Then
                    For i As Long = LBound(vFailureArray, 2) To UBound(vFailureArray, 2)
                        sw.WriteLine(
                            EscapeCsv(ToSafeString(vFailureArray(0, i))) & "," &
                            EscapeCsv(ToSafeString(vFailureArray(1, i))) & "," &
                            EscapeCsv(ToSafeString(vFailureArray(2, i))) & "," &
                            EscapeCsv(ToSafeString(vFailureArray(3, i))) & "," &
                            EscapeCsv(ToSafeString(vFailureArray(4, i))) & "," &
                            EscapeCsv(ToSafeString(vFailureArray(5, i))) & "," &
                            EscapeCsv(ToSafeString(vFailureArray(6, i))))
                        lCount += 1
                    Next
                End If

                sw.WriteLine()
                sw.WriteLine($"Total Quote Versions Failed:,{lCount}")
            End Using

            OutputLine($"  ✓ Failure report: {sFailureFile}")

        Catch ex As Exception
            OutputLine($"  ✗ Error generating failure report: {ex.Message}")
        End Try

        OutputLine("───────────────────────────────────────────────────────────────")
    End Sub

    Private Function EscapeCsv(ByVal v_sValue As String) As String
        If String.IsNullOrEmpty(v_sValue) Then Return v_sValue

        ' Neutralize spreadsheet formula injection (CSV injection prevention)
        If v_sValue.Length > 0 AndAlso (v_sValue(0) = "="c OrElse v_sValue(0) = "+"c OrElse v_sValue(0) = "-"c OrElse v_sValue(0) = "@"c) Then
            v_sValue = "'" & v_sValue
        End If

        ' Standard CSV escaping for commas, quotes, newlines
        If v_sValue.Contains(",") OrElse v_sValue.Contains("""") OrElse v_sValue.Contains(vbCr) OrElse v_sValue.Contains(vbLf) Then
            Return """" & v_sValue.Replace("""", """""") & """"
        End If
        Return v_sValue
    End Function

    Private Function ToSafeString(ByVal v_oValue As Object) As String
        If v_oValue Is Nothing OrElse IsDBNull(v_oValue) Then Return String.Empty
        Return v_oValue.ToString()
    End Function

#End Region

#Region "Methods"

    ''' <summary>
    ''' Closes the database connection
    ''' </summary>
    Public Sub CloseDBConnection()
        Try
            DBDisconnect(m_oDatabase)
        Catch ex As Exception
            OutputLine($"Warning: Error closing database connection: {ex.Message}")
        End Try
    End Sub

    ''' <summary>
    ''' Cleans up COM interop objects
    ''' </summary>
    Public Sub CleanUpInterops()
        Try
            m_oDatabase = Nothing
        Catch ex As Exception
            OutputLine($"Warning: Error cleaning up interops: {ex.Message}")
        End Try
    End Sub

#End Region

#Region "Creator"

    ''' <summary>
    ''' Constructor - initializes database connection
    ''' </summary>
    Public Sub New()
        Try
            ' Initialize database connection
            DBConnect(m_oDatabase)
        Catch ex As Exception
            Throw New Exception($"Failed to initialize database connection: {ex.Message}", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Finalizer
    ''' </summary>
    Protected Overrides Sub Finalize()
        Try
            MyBase.Finalize()
        Catch
            ' Suppress finalization errors
        End Try
    End Sub

#End Region

End Class