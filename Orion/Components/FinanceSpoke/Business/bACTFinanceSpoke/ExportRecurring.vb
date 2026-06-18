Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportRecurring
    '====================================================================
    '   Class/Module: ExportRecurringPayments
    '   Description : Class implementation of use case:
    'Export for InterfaceCode: "RECURRING"'
    '
    '====================================================================
    '   Maintenance History
    '
    '    28 February 2002    Paul Cunnigham    Created.
    '
    '====================================================================



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportRecurringPayments"

    '#Region " Private fields "
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database
    '#End Region

    ' ************************************************
    ' Added to replace global variables 24/09/2003
    ' Username.
    Private m_sUsername As String = ""
    ' Password.
    Private m_sPassword As String = ""
    ' User ID
    Private m_iUserID As Integer
    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    '#Region " Private Enums "
    'Columns in HeaderData array
    'Private Enum HeaderDataCols
    '    MediaTypeCode = 0
    '    BranchCode
    '    ExtractDate
    'End Enum
    '#End Region

    '#Region " Stored Procedures "
    'Private Const ksSPExportExtractTransSQL = "{call spu_ACT_Spoke_ExportExtractTrans (?,?,?)}"
    'Private Const ksSPExportExtractTransName = "GetExportExtractTrans"
    'Private Const ksSPExportExtractTransStored = True
    '#End Region

    '#Region " Friend Properties "
    Friend WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property

    Friend WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property
    '#End Region

    Friend Function PassThroughLogin(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PassThroughLogin
        ' PURPOSE: Pass through the module level login information to the Class.
        ' This is for COM+. Normally a business class will not require this but the Spoke
        ' design means that Classes are instantiated by the Business class and can
        ' no longer rely on global variables.
        ' AUTHOR: Danny Davis
        ' DATE: 24 September 2003, 11:55 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PassThroughLogin", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    '#Region " Friend Methods "
    Friend Function Start(ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Start
        ' PURPOSE: Start process for use case
        ' AUTHOR: Paul Cunnigham
        ' DATE: 28 February 2003, 11:45:03
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "Start"
        Const kSysOptionCollectINSFailedStatus As Integer = 5208

        Dim vResults, vHeaderArrayValues As Object

        Dim dtDueDateLimit As Date

        Dim dTotalAmount, dTotalAmountGrouped As Double
        Dim lBatchID, lTotalTransactions As Integer

        Dim lPFInstalmentsResultsId, lStatusId As Integer

        Dim sSQLWhere As String = ""
        Dim sSQLInsert As String = ""
        Dim sSelectFields As String = ""
        Dim sSourceList As String = ""
        Dim sTableName As String = ""

        Dim sSQLUpdate As New StringBuilder

        Dim vBatchId As Object
        Dim vDetailData(,) As Object

        Dim sStr() As String
        Dim lCount, lMediaCodeID As Integer
        Dim sMediaCode, sBatchType As String
        Dim lBatchTypeID As Integer

        Dim bTransactionActive As Boolean

        'DD 18/06/2003: Added for 1.8.6 Integration
        Dim bArrayOnly As Boolean

        Dim sSysOptionCollectINSFailedStatus As String = "0"

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            bTransactionActive = False

            'We need valid database and business objects
            If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Business and Database object are not set")
            End If

            'OK do the Export processing...

            If Not Information.IsArray(r_vHeaderData) Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Header data array empty, unable to proceed further.")
            End If

            'Get the values from the header array


            vHeaderArrayValues = r_vHeaderData(conValue)

            'Determine whether Collect further instalments if ‘Failed’ status is switched on/off
            result = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, kSysOptionCollectINSFailedStatus, sSysOptionCollectINSFailedStatus, 1), gPMConstants.PMEReturnCode)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Initialise variables
            lStatusId = 0
            dTotalAmount = 0

            'Prepare SQL Search criteria from supplied header data.

            dtDueDateLimit = DateTime.Today.AddDays(CDbl(vHeaderArrayValues(ehdLeadDays)))


            If Strings.Len(CStr(vHeaderArrayValues(ehdSourceList))) > 0 Then

                sSourceList = CStr(vHeaderArrayValues(ehdSourceList)).Replace(";", "','")
            End If

            'Safety check

            If vHeaderArrayValues.GetUpperBound(0) > 18 Then

                bArrayOnly = CBool(vHeaderArrayValues(ehdArrayOnly))
            Else
                bArrayOnly = False
            End If

            With m_oDatabase

                sSQLWhere = "WHERE [PFI].duedate <= '" & dtDueDateLimit.ToString("yyyy/MM/dd") & "'" & Strings.Chr(13) & Strings.Chr(10)
                'asnd pfinstalments.batch id is null

                'sw 18/03/2003, do not return instalments that have already been exported as part of a batch
                'unless the status is 5 (retrying) or 7 (held)
                sSQLWhere = sSQLWhere & "AND ([PFI].Batch_ID is Null OR [PFI].Status = 5 OR [PFI].Status = 7)" & Strings.Chr(13) & Strings.Chr(10)

                If sSourceList.Length > 0 Then
                    'Must be some source items listed.
                    sSQLWhere = sSQLWhere & "AND [COM1].code IN ('" & sSourceList.Replace(":", "','") & "')" & Strings.Chr(13) & Strings.Chr(10)
                End If


                sSQLWhere = sSQLWhere & "AND UPPER([MTP].code) = '" & CStr(vHeaderArrayValues(ehdMediaTypeCode)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)


                If CInt(vHeaderArrayValues(ehdAccountsFilter)) = 1 Then
                    sSQLWhere = sSQLWhere & "AND [COM1].company_id = [COM2].company_id" & Strings.Chr(13) & Strings.Chr(10)
                ElseIf CInt(vHeaderArrayValues(ehdAccountsFilter)) = 2 Then
                    sSQLWhere = sSQLWhere & "AND [COM1].company_id <> [COM2].company_id" & Strings.Chr(13) & Strings.Chr(10)
                End If

                'sw 04/04/2003, only include instalments that have not been paid

                'DD 18/06/2003: The Instalment needs to be ready to go (New or Retry)

                If CBool(r_vHeaderData(1)(ehdArrayOnly)) Then
                    'DD 16/06/2003: The Plan must be live
                    sSQLWhere = sSQLWhere & " AND ([PLN].StatusInd='040' or [PLN].StatusInd='140')" & Strings.Chr(13) & Strings.Chr(10)

                    'DD 18/06/2003: The Instalment needs to be ready to go (New or Retry)
                    sSQLWhere = sSQLWhere & " AND ([PFI].Status=1 OR [PFI].Status=5 OR [PFI].Status=7)" & Strings.Chr(13) & Strings.Chr(10)
                Else
                    'DD 16/06/2003: The Plan must be live
                    sSQLWhere = sSQLWhere & " AND [PLN].StatusInd='040'" & Strings.Chr(13) & Strings.Chr(10)

                    'DD 18/06/2003: The Instalment needs to be ready to go (New or Retry)
                    sSQLWhere = sSQLWhere & " AND ([PFI].Status=1 OR [PFI].Status=5)" & Strings.Chr(13) & Strings.Chr(10)
                End If

                ' ensure that the plan has actually been posted before exporting any of its instalments
                ' to avoid exporting instalment from plans which have not yet been transacted
                sSQLWhere = sSQLWhere & " AND [PLN].plantransaction_id IS NOT NULL" & Strings.Chr(13) & Strings.Chr(10)

                'DD 19/11/2003: CQ3235 Filter out instalments that have reached their retry limit
                sSQLWhere = sSQLWhere & " AND (ISNULL([PFI].failure_count,0)<=PFRF.retry_limit OR PFRF.retry_limit = 0)" & Strings.Chr(13) & Strings.Chr(10)

                'MKW160204 PN10300.  Exclude Audis Lines (based upon option)
                sSQLWhere = sSQLWhere & " AND ((pfit.code <> '0N' and pfi.instalmentNumber <> 0 and pfpm.excludeaudis=1) or (isnull(pfpm.excludeaudis,0) <> 1))"

                'Exclude failed instalment plan ie particular plan with version and plan ref if Collect further instalments if ‘Failed’ status is switched off
                If sSysOptionCollectINSFailedStatus = "0" Then
                    sSQLWhere = sSQLWhere & " AND Convert(VARCHAR(12), [PFI].pfprem_finance_cnt) +'-'+ CONVERT(VARCHAR(12),[PFI].pfprem_finance_version) NOT IN "
                    sSQLWhere = sSQLWhere & " (SELECT CONVERT(VARCHAR(12),pfprem_finance_cnt)+'-'+ CONVERT(VARCHAR(12),pfprem_finance_version) FROM PFInstalments WITH(NOLOCK) "
                    sSQLWhere = sSQLWhere & " WHERE PFInstalments.Status=6 AND PFInstalments.pfprem_finance_cnt=[PFI].pfprem_finance_cnt  "
                    sSQLWhere = sSQLWhere & " AND PFInstalments.pfprem_finance_version=[PFI].pfprem_finance_version)"
                End If


                If RetrieveRecords(v_sWhereClause:=sSQLWhere, r_vResults:=vResults, v_bGroupRecords:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error(s) during retrieval of records")
                End If

                'Nothing to do if no records found
                If Information.IsArray(vResults) Then
                    'DD 18/06/2003: If we're only returning the array then
                    'Skip the batch generation and just return the results
                    If Not bArrayOnly Then
                        If m_oBusiness.GetIDValueFromCode(v_sTableName:="BatchStatus", v_bGettingCode:=False, r_sCode:=conBICode, r_lID:=lStatusId) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.BatchStatus." & conBICode)
                        End If


                        sMediaCode = CStr(vHeaderArrayValues(ehdMediaTypeCode))

                        lMediaCodeID = 0

                        If m_oBusiness.GetIDValueFromCode(v_sTableName:="mediatype", v_bGettingCode:=False, r_sCode:=sMediaCode, r_lID:=lMediaCodeID) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.mediatype." & sMediaCode)
                        End If


                        sBatchType = CStr(vHeaderArrayValues(ehdBatchTypeCode))

                        If m_oBusiness.GetIDValueFromCode(v_sTableName:="batch_type", v_bGettingCode:=False, r_sCode:=sBatchType, r_lID:=lBatchTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.batch_type." & sBatchType)
                        End If

                        'Begin a transaction
                        If .SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to start transaction")
                        End If
                        bTransactionActive = True



                        m_lReturn = CType(m_oBusiness.CreateBatchRecord(r_lBatchID:=lBatchID, v_lBatchStatusID:=lStatusId, v_lCompanyID:=m_iSourceID, v_lUserID:=m_iUserID, v_sBatchRef:=r_sBatchRef, v_dtCreatedDate:=DateTime.Today, v_sComment:=CStr(vHeaderArrayValues(ehdBatchCode)), v_lBatchTypeID:=lBatchTypeID, v_dtExportDate:=DateTime.Today, v_lMediaTypeID:=lMediaCodeID, v_sInterfaceCode:=r_sInterfaceCode, v_iAutoClose:=Conversion.Val(CStr(vHeaderArrayValues(ehdAutoClose)))), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while inserting new record into Batch table")
                        End If

                        'update the batch record
                        m_lReturn = CType(m_oBusiness.UpdateBatchRef(v_lBatchID:=lBatchID, v_sBatchRef:=CStr(lBatchID)), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error whilst updating the batch table")
                        End If

                        If m_oBusiness.GetIDValueFromCode(v_sTableName:="PFInstalments_Result", v_bGettingCode:=False, r_sCode:=conWaitingForCollectionCode, r_lID:=lPFInstalmentsResultsId) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.PFInstalments_Result." & conWaitingForCollectionCode)
                        End If

                        'Add one array element to r_vDetailData per row in vResults

                        vDetailData = ArraysHelper.RedimPreserve(Of Object(,))(vDetailData, New Integer() {gHUBSpokeConstants.eddPFInstalmentDueDate - gHUBSpokeConstants.eddDetailId + 1, vResults.GetUpperBound(conRows - 1) + 1}, New Integer() {gHUBSpokeConstants.eddDetailId, 0})

                        'loop through the array updating the group_id field

                        For iLoop As Integer = 0 To vResults.GetUpperBound(conRows - 1)

                            'now update the PFInstalments record including the group ID



                            If UpdatePFInstalmentsGroupID(v_lInstalmentID:=CInt(vResults(rcpPFInstalmentsId, iLoop)), v_dtDueDate:=CDate(vResults(rcpPFInstalmentDueDate, iLoop)), v_sShortName:=CStr(vResults(rcpPartyShortName, iLoop)).Trim(), v_sCCNumber:=gPMFunctions.NullToString(CStr(vResults(rcpPFCCNumber, iLoop))).Trim(), v_sSortCode:=gPMFunctions.NullToString(CStr(vResults(rcpPFPremiumFinanceBankSortCode, iLoop))).Trim(), v_sBankAccountNo:=gPMFunctions.NullToString(CStr(vResults(rcpPFPremiumFinanceBankAccountNo, iLoop))).Trim(), v_sAutoGeneratedPlanRef:=gPMFunctions.NullToString(CStr(vResults(rcpPFAutoGeneratedPlanRef, iLoop))).Trim(), v_sSQLWhere:=sSQLWhere) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while updating Instalment Group ID")

                            End If

                        Next iLoop

                        'now update the batch related fields

                        For iLoop As Integer = 0 To vResults.GetUpperBound(conRows - 1)
                            'Link PFInstalment records to the newly created batch record using batch_id column.
                            sSQLUpdate = New StringBuilder("UPDATE PFInstalments" & Strings.Chr(13) & Strings.Chr(10))
                            sSQLUpdate.Append("SET batch_id = " & lBatchID & Strings.Chr(13) & Strings.Chr(10))

                            sSQLUpdate.Append(" WHERE pfinstalments_id = " & CStr(vResults(rcpPFInstalmentsId, iLoop)) & Strings.Chr(13) & Strings.Chr(10))

                            sTableName = "PFInstalments"

                            If .SQLAction(sSQL:=sSQLUpdate.ToString(), sSQLName:="Update " & sTableName, bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error while updating " & sTableName)
                            End If


                            dTotalAmount += CDbl(vResults(rcpPFIInstalmentAmount, iLoop))
                        Next iLoop

                        'now get the records again, this time group them by DueDate/Party/CreditCardNo or DueDate/Party/BankDetails

                        sSQLWhere = "WHERE [PFI].batch_id = " & lBatchID & Strings.Chr(13) & Strings.Chr(10)

                        If RetrieveRecords(v_sWhereClause:=sSQLWhere, r_vResults:=vResults, v_bGroupRecords:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while retrieving grouped transactions")
                        End If


                        lTotalTransactions = vResults.GetUpperBound(conRows - 1) + 1

                        'validate that the total currency amount of the grouped records is the same as that calculated
                        'from the non grouped records, this will confirm that nothing went wrong when inserting the
                        'GroupID's


                        For iLoop As Integer = 0 To vResults.GetUpperBound(conRows - 1)

                            dTotalAmountGrouped += CDbl(vResults(rcpPFIInstalmentAmount, iLoop))
                        Next iLoop

                        'If dTotalAmount <> dTotalAmountGrouped Then
                        If Math.Round(dTotalAmount, 2) <> Math.Round(dTotalAmountGrouped, 2) Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", An Error occured wilst grouping the transactions")
                        End If

                        'Update PFInstalment records setting their status update using batch_id column.
                        sSQLUpdate = New StringBuilder("UPDATE PFInstalments" & Strings.Chr(13) & Strings.Chr(10) &
                                     "SET status = 2, " &
                                     "pfinstalments_result_id = " & CStr(lPFInstalmentsResultsId) &
                                 " WHERE batch_id = " & CStr(lBatchID) & Strings.Chr(13) & Strings.Chr(10))

                        sTableName = "PFInstalments"

                        If .SQLAction(sSQL:=sSQLUpdate.ToString(), sSQLName:="Update " & sTableName, bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error while updating " & sTableName)
                        End If
                    End If

                    'redeclare the detail data array (this will reduce in size dependant on whether records have been grouped)

                    vDetailData = ArraysHelper.RedimPreserve(Of Object(,))(vDetailData, New Integer() {gHUBSpokeConstants.eddPFInstalmentDueDate - gHUBSpokeConstants.eddDetailId + 1, vResults.GetUpperBound(conRows - 1) + 1}, New Integer() {gHUBSpokeConstants.eddDetailId, 0})

                    'populate the detail data to return to the hub

                    For iLoop As Integer = 0 To vResults.GetUpperBound(conRows - 1)


                        If m_oBusiness.PopulateDetailArray(v_sInterfaceCode:=r_sInterfaceCode, r_vDetailData:=vDetailData, v_vResults:=vResults, v_iElementNumber:=iLoop) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error populating detail array")
                        End If

                    Next iLoop

                    'round total to 2 dp
                    dTotalAmount = Math.Round(dTotalAmount, 2)

                    'Update header array with total batch amount
                    r_sBatchRef = CStr(lBatchID)

                    vHeaderArrayValues(ehdBatchAmount) = dTotalAmount

                    vHeaderArrayValues(ehdTotalNoOfTransactions) = lTotalTransactions

                    'DD 18/06/2003: Skip if producing the array only
                    If Not bArrayOnly Then

                        lStatusId = 0

                        If m_oBusiness.GetIDValueFromCode(v_sTableName:="BatchStatus", v_bGettingCode:=False, r_sCode:=conBECode, r_lID:=lStatusId) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error(s) during retrieval of id for db.BatchStatus." & conBECode)
                        End If

                        'Update batch status to 'exported' (BE).
                        'Update Batch Total Amount and Total Transactions fields.
                        sSQLUpdate = New StringBuilder("UPDATE Batch" & Strings.Chr(13) & Strings.Chr(10))
                        sSQLUpdate.Append("SET batchstatus_id = " & lStatusId & conComma & Strings.Chr(13) & Strings.Chr(10))
                        sSQLUpdate.Append("total_amount = " & dTotalAmount & conComma & Strings.Chr(13) & Strings.Chr(10))
                        sSQLUpdate.Append("total_transactions = " & lTotalTransactions & Strings.Chr(13) & Strings.Chr(10))
                        sSQLUpdate.Append("WHERE batch_id = " & lBatchID)

                        If .SQLAction(sSQL:=sSQLUpdate.ToString(), sSQLName:="Update Batch details", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error while updating batch status")
                        End If
                    End If

                    'If we have results then we need to add a new element to the
                    'r_vDetailData array

                    If AddResultArrayToDetailArray(v_vDetailArray:=r_vDetailData, r_vResultArray:=vDetailData, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to add results to Detail Array")
                    End If

                    'assign the header details back to the return variable


                    r_vHeaderData(conValue) = vHeaderArrayValues

                    'Begin a transaction
                    If bTransactionActive Then
                        If .SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to start transaction")
                        End If
                        bTransactionActive = False
                    End If
                End If
            End With

            If Information.IsArray(vResults) Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception


            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

            'Rollback any active transactions
            If bTransactionActive Then
                m_oDatabase.SQLRollbackTrans()
            End If

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: RetrieveRecords
    '
    ' Description: Retrieves all the records associated with a batch
    '
    ' ***************************************************************** '
    Public Function RetrieveRecords(ByVal v_sWhereClause As String, ByRef r_vResults(,) As Object, ByVal v_bGroupRecords As Boolean) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "RetrieveRecords"

        Dim sSQLSearch As String = ""
        Dim vResults(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Need to take the relevant records from the PFInstalments table
            'Criteria to be used.
            '*************************************************************************
            'PFInstalments.DueDate <= (Today + Lead Days)
            'PFInstalments.Source = On Source List
            'PFInstalments.Plan.MediaType.MediaType_Validation = Media Type Validation
            'If Accounts Filter = 1
            'PFInstalments.Plan.Source = Party.Source
            'If Accounts Filter = 2
            'PFInstalments.Plan.Source <> Party.Source
            '*************************************************************************

            'sw issue 3009, return bank details from query
            'DD 11/08/2003: CQ1253, return CC details
            'DD 17/11/2003: CQ3235, returned PFRF record for filtering retry limit

            'sw 04/04/2003
            'PN12360 Modified to group by max duedate for grouped records so that where the 0 record is included
            ' todays date doesn't override the 1st payment date + lead days
            If Not v_bGroupRecords Then
                'DD 19/06/2003: Added zero dummy field to avoid array out of bounds error
                sSQLSearch = "SELECT [PFI].pfinstalments_id, [PFI].duedate, [PTY].shortname, [PLN].clientname, [PLN].companyname, "
                sSQLSearch = sSQLSearch & "[PLN].cc_number,[PFMTH].bankaccountname, [PFMTH].BankAccountNo, [PFMTH].BankSortCode, [PFMTH].BankName, "
                sSQLSearch = sSQLSearch & "[PFI].amount, [PTY].alternative_identifier, [PTY].agent_cnt, 0, PFI.Status, PFIS.Description, PFIT.Code, PLN.AutoGeneratedPlanRef, PFI.InstalmentNumber, "
                sSQLSearch = sSQLSearch & "[PLN].cc_expiry_date, [PLN].cc_start_date, [PLN].cc_issue, [PLN].cc_pin"
            Else
                sSQLSearch = "SELECT Min([PFI].pfinstalments_id), Max([PFI].duedate), [PTY].shortname, [PLN].clientname, [PLN].companyname, "
                sSQLSearch = sSQLSearch & "[PLN].cc_number, [PFMTH].bankaccountname, [PFMTH].BankAccountNo, [PFMTH].BankSortCode, [PFMTH].BankName, "
                sSQLSearch = sSQLSearch & "Sum([PFI].amount), [PTY].alternative_identifier, [PTY].agent_cnt, [PFI].group_id, 0, '', Min(PFIT.Code), PLN.AutoGeneratedPlanRef, 0, "
                sSQLSearch = sSQLSearch & "[PLN].cc_expiry_date, [PLN].cc_start_date, [PLN].cc_issue, [PLN].cc_pin"
            End If

            sSQLSearch = sSQLSearch & " FROM PFInstalments [PFI]"
            sSQLSearch = sSQLSearch & " JOIN PFInstalments_Status [PFIS] ON [PFIS].PFInstalments_Status_id=PFI.Status "
            sSQLSearch = sSQLSearch & " JOIN PFInstalments_Transaction [PFIT] ON [PFIT].PFInstalments_Transaction_id=PFI.TransactionCode "
            sSQLSearch = sSQLSearch & " JOIN PFPremiumFinance[PLN] ON [PLN].pfprem_finance_cnt = [PFI].pfprem_finance_cnt "
            sSQLSearch = sSQLSearch & "    AND [PLN].pfprem_finance_version = [PFI].pfprem_finance_version"
            sSQLSearch = sSQLSearch & " LEFT JOIN Company[COM1] ON [COM1].Company_id = [PLN].CompanyNo"
            sSQLSearch = sSQLSearch & " JOIN PFScheme[SCH] on [SCH].schemeversion = [PLN].schemeversion"
            sSQLSearch = sSQLSearch & "    AND [SCH].schemeno = [PLN].schemeno"
            sSQLSearch = sSQLSearch & "    AND [SCH].companyno = [PLN].companyno"
            sSQLSearch = sSQLSearch & " JOIN PFRF on PFRF.pfrf_id=[PLN].pfrf_id"
            sSQLSearch = sSQLSearch & " JOIN MediaType[MTP] ON [MTP].mediatype_id = [SCH].mediatype_id"
            sSQLSearch = sSQLSearch & " JOIN Party[PTY] ON [PTY].party_cnt = [PLN].clientid"
            sSQLSearch = sSQLSearch & " JOIN Company[COM2] ON [COM2].Company_id = [PTY].source_id "

            'MKW160204 PN10300.  Exclude Audis Lines (based upon option)
            sSQLSearch = sSQLSearch & " left join pfpaymentmethod pfpm on pfpm.PFPaymentMethod_cnt = sch.PaymentMethod_cnt "
            sSQLSearch = sSQLSearch & " left join PFMediaTypeHistory [PFMTH] on PFMTH.pfMediaTypeHistory_id=PFI.pfmediatype_history_id "

            sSQLSearch = sSQLSearch & v_sWhereClause

            If v_bGroupRecords Then
                ' SET 20/08/2004 PN14299 - grouping records so exclude AUDDIS record
                sSQLSearch = sSQLSearch & " AND [PFI].InstalmentNumber <> 0"
                sSQLSearch = sSQLSearch & " Group By [PFI].group_id, [PTY].shortname, [PFMTH].BankAccountNo, [PFMTH].BankSortCode, PLN.AutoGeneratedPlanRef, "
                sSQLSearch = sSQLSearch & "[PLN].cc_number, [PLN].clientname, [PLN].companyname, [PFMTH].bankaccountname, "
                sSQLSearch = sSQLSearch & "[PFMTH].BankName , [PTY].alternative_identifier, [PTY].agent_cnt, "
                sSQLSearch = sSQLSearch & "[PLN].cc_expiry_date, [PLN].cc_start_date, [PLN].cc_issue, [PLN].cc_pin"
                'PN12171 Add Union with Auddis transactions
                sSQLSearch = sSQLSearch & " UNION "
                sSQLSearch = sSQLSearch & "SELECT [PFI].pfinstalments_id, [PFI].duedate, [PTY].shortname, [PLN].clientname, [PLN].companyname, "
                sSQLSearch = sSQLSearch & "[PLN].cc_number,[PFMTH].bankaccountname, [PFMTH].BankAccountNo, [PFMTH].BankSortCode, [PFMTH].BankName, "
                sSQLSearch = sSQLSearch & "[PFI].amount, [PTY].alternative_identifier, [PTY].agent_cnt, 0, PFI.Status, PFIS.Description, PFIT.Code, PLN.AutoGeneratedPlanRef, PFI.InstalmentNumber, "
                sSQLSearch = sSQLSearch & "[PLN].cc_expiry_date, [PLN].cc_start_date, [PLN].cc_issue, [PLN].cc_pin"
                sSQLSearch = sSQLSearch & " FROM PFInstalments [PFI]"
                sSQLSearch = sSQLSearch & " JOIN PFInstalments_Status [PFIS] ON [PFIS].PFInstalments_Status_id=PFI.Status "
                sSQLSearch = sSQLSearch & " JOIN PFInstalments_Transaction [PFIT] ON [PFIT].PFInstalments_Transaction_id=PFI.TransactionCode "
                sSQLSearch = sSQLSearch & " JOIN PFPremiumFinance[PLN] ON [PLN].pfprem_finance_cnt = [PFI].pfprem_finance_cnt "
                sSQLSearch = sSQLSearch & "    AND [PLN].pfprem_finance_version = [PFI].pfprem_finance_version"
                sSQLSearch = sSQLSearch & " LEFT JOIN Company[COM1] ON [COM1].Company_id = [PLN].CompanyNo"
                sSQLSearch = sSQLSearch & " JOIN PFScheme[SCH] on [SCH].schemeversion = [PLN].schemeversion"
                sSQLSearch = sSQLSearch & "    AND [SCH].schemeno = [PLN].schemeno"
                sSQLSearch = sSQLSearch & "    AND [SCH].companyno = [PLN].companyno"
                sSQLSearch = sSQLSearch & " JOIN PFRF on PFRF.pfrf_id=[PLN].pfrf_id"
                sSQLSearch = sSQLSearch & " JOIN MediaType[MTP] ON [MTP].mediatype_id = [SCH].mediatype_id"
                sSQLSearch = sSQLSearch & " JOIN Party[PTY] ON [PTY].party_cnt = [PLN].clientid"
                sSQLSearch = sSQLSearch & " JOIN Company[COM2] ON [COM2].Company_id = [PTY].source_id "
                sSQLSearch = sSQLSearch & " left join pfpaymentmethod pfpm on pfpm.PFPaymentMethod_cnt = sch.PaymentMethod_cnt "
                sSQLSearch = sSQLSearch & " left join PFMediaTypeHistory [PFMTH] on PFMTH.pfMediaTypeHistory_id=PFI.pfmediatype_history_id "
                sSQLSearch = sSQLSearch & v_sWhereClause
                sSQLSearch = sSQLSearch & " AND [PFI].InstalmentNumber = 0"
                'PN12171End
                sSQLSearch = sSQLSearch & " ORDER BY Max([PFI].duedate)"
            Else
                sSQLSearch = sSQLSearch & " ORDER BY [PFI].duedate"
            End If

            If m_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Installment-Payment Search", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error(s) during retrieval of records")
            End If

            'Error if we don't retrive any records?
            '    If Not (IsArray(vResults)) Then
            '        Err.Raise vbObjectError, ACMethod, "Search returned no payment/installment results"
            '    End If

            'Pass back the complete SQL query results set.


            r_vResults = vResults

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally


        End Try
        Return result
    End Function

    'SW 04/04/2003 Added this function to update PFInastalments with the Group ID of a given Instalment
    Private Function UpdatePFInstalmentsGroupID(ByVal v_lInstalmentID As Integer, ByVal v_dtDueDate As Date, ByVal v_sShortName As String, ByVal v_sCCNumber As String, ByVal v_sSortCode As String, ByVal v_sBankAccountNo As String, ByVal v_sAutoGeneratedPlanRef As String, ByVal v_sSQLWhere As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "Declare @temp int" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "Select @temp = Min([PFI].pfinstalments_id)" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "From " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "PFInstalments [PFI] " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & " JOIN PFInstalments_Transaction [PFIT] ON [PFIT].PFInstalments_Transaction_id=PFI.TransactionCode "
        sSQL = sSQL & "LEFT JOIN PFPremiumFinance[PLN] ON [PLN].pfprem_finance_cnt = [PFI].pfprem_finance_cnt AND [PLN].pfprem_finance_version = [PFI].pfprem_finance_version " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "LEFT JOIN Company[COM1] ON [COM1].Company_id = [PLN].CompanyNo " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "LEFT JOIN PFScheme[SCH] on [SCH].schemeversion = [PLN].schemeversion AND [SCH].schemeno = [PLN].schemeno    AND [SCH].companyno = [PLN].companyno " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "LEFT JOIN MediaType[MTP] ON [MTP].mediatype_id = [SCH].mediatype_id " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "LEFT JOIN MediaType_Validation[MTV] ON [MTV].mediatype_validation_id = [MTP].mediatype_validation_id " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "LEFT JOIN Party[PTY] ON [PTY].party_cnt = [PLN].clientid " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "LEFT JOIN Company[COM2] ON [COM2].Company_id = [PTY].source_id " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & " LEFT JOIN pfpaymentmethod pfpm ON pfpm.PFPaymentMethod_cnt = sch.PaymentMethod_cnt "

        ' PW201103 - CQ3235 - join to PFRF
        sSQL = sSQL & "LEFT JOIN PFRF on PFRF.pfrf_id=[PLN].pfrf_id" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & v_sSQLWhere
        sSQL = sSQL & "AND [PTY].shortname = '" & v_sShortName.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
        If v_sCCNumber <> "" Then
            sSQL = sSQL & "AND [PLN].cc_number = '" & v_sCCNumber.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
        Else
            sSQL = sSQL & "AND [PLN].BankAccountNo = '" & v_sBankAccountNo.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "AND [PLN].BankSortCode = '" & v_sSortCode.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
        End If

        If v_sAutoGeneratedPlanRef <> "" Then
            sSQL = sSQL & "AND [PLN].AutoGeneratedPlanRef = '" & v_sAutoGeneratedPlanRef.Trim() & "'" & Strings.Chr(13) & Strings.Chr(10)
        End If

        sSQL = sSQL & " Group By [PFI].group_id, [PTY].shortname, [PLN].BankAccountNo, [PLN].BankSortCode, " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "[PLN].cc_number, [PLN].clientname, [PLN].companyname, [PLN].bankaccountname, " & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "[PLN].BankName , [PTY].alternative_identifier" & Strings.Chr(13) & Strings.Chr(10)
        sSQL = sSQL & "Update PFInstalments set Group_ID = @temp Where PFInstalments_ID = " & CStr(v_lInstalmentID)

        If m_oDatabase.SQLAction(sSQL, "Update PFInstalments Group ID", False) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", UpdatePFInstalmentsGroupID, Error updating the PFInstalments Group ID")
        End If


        Return result

    End Function




    '#End Region

    '#Region " Private Methods "
    Public Sub New()
        MyBase.New()
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Class_Initialize
        ' PURPOSE: Class initialisation
        ' AUTHOR: Paul Cunnigham
        ' DATE: 11 November 2002, 12:08:23
        ' CHANGES:
        ' ---------------------------------------------------------------------------


        Try

            'Class initialisation
            m_oBusiness = Nothing
            m_oDatabase = Nothing


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            End Select

        Finally


        End Try
    End Sub
End Class
