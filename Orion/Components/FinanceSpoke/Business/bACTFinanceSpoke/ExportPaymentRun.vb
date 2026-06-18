Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportPaymentRun

    Private Const ACClass As String = "PaymentRun"

    '#Region " Private fields "
    Private m_lReturn As Integer
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database

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

    Private Const kMediaTypeCode As Integer = 9
    Private Const kPaymentTypeCode As Integer = 10

    Const kName As Integer = 0
    Const kValue As Integer = 1

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

    ' ***************************************************************** '
    ' Name: CheckReportsExist
    '
    ' Description: Checks the required reports exist for our media type
    '                   validation code and payment type
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: PMTrue if all required reports found
    '
    ' Date: 06/10/2003
    '
    ' ***************************************************************** '
    Private Function CheckReportsExist(ByRef v_sMediaTypeValidation As String, ByRef v_sPaymentType As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vResults(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_sMediaTypeValidation.Trim() = "BANK" Or v_sMediaTypeValidation.Trim() = "CC" Then
            'Check for report "PRSUM"

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="Code", vValue:="PRSUM", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                'developer guide no. 39
                m_lReturn = .SQLSelect(sSQL:="spu_report_select", sSQLName:="Report Select", bStoredProcedure:=gPMConstants.PMEReturnCode.PMTrue, vResultArray:=vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckReportsExist failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Not Information.IsArray(vResults) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to find report  for code 'PRSUM'", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Select Case v_sPaymentType
            Case "COMM" 'Check for report "PRCOMM"
                With m_oDatabase

                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="Code", vValue:="PRCOMM", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    'developer guide no. 39
                    m_lReturn = .SQLSelect(sSQL:="spu_report_select", sSQLName:="Report Select", bStoredProcedure:=gPMConstants.PMEReturnCode.PMTrue, vResultArray:=vResults)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckReportsExist failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                If Not Information.IsArray(vResults) Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to find report  for code 'PRCOMM'", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Case "CLP" 'Check for report "PRCLM"
                With m_oDatabase

                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="Code", vValue:="PRCOMM", idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    'developer guide no. 39
                    m_lReturn = .SQLSelect(sSQL:="spu_report_select", sSQLName:="Report Select", bStoredProcedure:=gPMConstants.PMEReturnCode.PMTrue, vResultArray:=vResults)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CheckReportsExist failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                If Not Information.IsArray(vResults) Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to find report  for code 'PRCLM'", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAccountID
    '
    ' Description: Gets the AccountID from a given CashListItemID.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: AccountID as long
    '
    ' Date: 08/10/2003
    '
    ' ***************************************************************** '
    Private Function GetAccountID(ByVal v_lCashListItemID As Integer, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 39
        Const sSQL As String = "spu_ACT_Select_CashListItem"
        Const sSQLName As String = "Get Account ID"
        Const sSQLStored As Boolean = True
        Const ACTAccountID As Integer = 4

        Dim vResults(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue


        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=sSQLStored, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAccountID failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        If Not Information.IsArray(vResults) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get AccountID", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            r_lAccountID = CInt(vResults(ACTAccountID, 0))
        End If


        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetCashListItemID
    '
    ' Description: Gets the CashListItemID from a given TransDetailID.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: CashListItemID as long
    '
    ' Date: 08/10/2003
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetCashListItemID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetCashListItemID(ByVal v_lTransDetailID As Integer, ByRef r_lCashListItemID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const sSQL As String = "{call spu_ACT_Select_CashListItemID_From_TransDetailID(?)}"
    'Const sSQLName As String = "Get Cash List Item ID"
    'Const sSQLStored As Boolean = True
    'Const ACTCashListItemID As Integer = 0
    '
    'Dim vResults(,) As Object
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Try 
    'With m_oDatabase
    '
    '.Parameters.Clear()
    '
    'm_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    '
    'm_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=sSQLStored, vResultArray:=vResults)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCashListItemID failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End With
    '
    'If Not Information.IsArray(vResults) Then
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get CashListItemID", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'Else

    'r_lCashListItemID = CInt(vResults(ACTCashListItemID, 0))
    'End If
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCashListItemID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCashListItemID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: GetAllocatedTransDetailIDs
    '
    ' Description: Gets all allocated transdetail IDs and amounts in a rolling array.
    '                   for a given TransDetail ID
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: An array of TransDetail IDs and amounts separated by a | character
    '           in the format required for allocation
    '
    ' Date: 08/10/2003
    '
    ' ***************************************************************** '
    Private Function GetAllocatedTransDetailIDs(ByVal v_lTransDetailID As Integer, ByRef r_TransDetailArray() As Object) As Integer

        Dim result As Integer = 0


        'developer guide no. 39
        Const sSQL As String = "spu_ACT_Select_Allocated_TransDetail_ID"
        Const sSQLName As String = "Select Allocated TransDetail IDs"
        Const sSQLStored As Boolean = True

        Dim vResultArray(,) As Object
        Dim lCurrentArrayItems As Integer

        result = gPMConstants.PMEReturnCode.PMTrue
        'Run the SP for this TransDetailID
        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="TransDetailID", vValue:=CStr(v_lTransDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=sSQLStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAllocatedTransDetailIDs failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        'Do we already have an array? If so, how many items are in it?
        If Information.IsArray(r_TransDetailArray) Then
            lCurrentArrayItems = r_TransDetailArray.GetUpperBound(0) + 1
        Else
            'If not then create an array with 0 items
            lCurrentArrayItems = 0
            ReDim r_TransDetailArray(0)
        End If

        'Now increase the size of the array to include however many results were returned from the SP

        ReDim Preserve r_TransDetailArray(lCurrentArrayItems + vResultArray.GetUpperBound(1))

        'Now loop though and format it in the manner that Allocation requires

        For lCount As Integer = 0 To vResultArray.GetUpperBound(1)



            r_TransDetailArray(lCount + lCurrentArrayItems) = CStr(vResultArray(0, lCount)) & "|" & CStr(vResultArray(1, lCount))
        Next

        Return result

    End Function

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


    Friend Function Start(ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sPaymentType, sMediaTypeValidation, sMediaTypeCode, sPaymentTypeCode As String

            result = gPMConstants.PMEReturnCode.PMTrue

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            'DD 20/08/2003: Set the Batch Ref from the header if this is being
            'launched from the user interface
            If v_sBatchRef = "" Then

                v_sBatchRef = CStr(r_vHeaderData(kValue)(1))
            End If

            sMediaTypeCode = CStr(r_vHeaderData(kValue)(kMediaTypeCode)).Trim()

            sPaymentTypeCode = CStr(r_vHeaderData(kValue)(kPaymentTypeCode)).Trim()

            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the batch details in the database
            m_lReturn = PaymentRun(v_sBatchRef, sMediaTypeCode, sPaymentTypeCode, r_vDetailData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_vDetailData.GetUpperBound(0) = 0 Then
                'no records to export
                m_lReturn = CommitTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'Get the payment run type for chossing the report to print
            m_lReturn = GetPaymentRunType(v_sBatchRef, sPaymentType, sMediaTypeValidation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SMJB 6/10/03: Check that the required reports exist now, this is to prevent the whole
            'process from failing because the reports aren't there
            m_lReturn = CheckReportsExist(v_sMediaTypeValidation:=sMediaTypeValidation, v_sPaymentType:=sPaymentType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                'SMJB 6/10/03 Cannot have an outstanding transaction when entering PostCashListItems as it causes
                'a deadlock in reverse allocation.  The transaction remains so that we can rollback any batch
                'changes we have made to ensure the batch can be re-selected if the reports were missing before
                CommitTrans()
            End If

            'Post the cash list items to the db

            m_lReturn = PostCashListItems(v_sBatchRef, r_vDetailData(1))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sMediaTypeValidation.Trim() = "BANK" Or sMediaTypeValidation.Trim() = "CC" Then
                'Print the Payment Summary report
                m_lReturn = SpoolReport(v_sBatchRef, "PRSUM")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Select Case sPaymentType
                Case "COMM"
                    m_lReturn = SpoolReport(v_sBatchRef, "PRCOMM")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Case "CLP"
                    m_lReturn = SpoolReport(v_sBatchRef, "PRCLM")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
            End Select

            'SMJB - Shouldn't be any outstanding transactions now as we had to commit it before we entered
            'PostCashListItems but I'm not going to remove it just in case
            m_lReturn = CommitTrans()
            'If commit fails then better to try and rollback, than possibly leave transaction open
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE

            Return result

        Catch excep As System.Exception



            RollbackTrans()
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start method failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PaymentRun(ByVal v_sBatchRef As String, ByVal v_sMediaTypeCode As String, ByVal v_sPaymentTypeCode As String, ByRef r_vDetailData() As Object) As Integer

        Dim result As Integer = 0


        Dim lNumberRecords As Integer
        Dim vDetailDataValues(,) As Object
        Dim bSpool As Boolean
        Dim sSpoolDocumentCode As String = ""
        Dim bExport As Boolean
        Dim oDocManagerWrapper As Object
        Dim lDocumentTemplateID, lDocumentTemplateVersionID, lDocumentTypeID As Integer

        Const ACPrintSilentMode As Integer = 3
        Const ACSpoolDocMode As Integer = 4

        result = gPMConstants.PMEReturnCode.PMTrue

        ' DD 01/10/2003: Get the Media Type settings
        m_lReturn = GetMediaTypeSettings(sMediaTypeCode:=v_sMediaTypeCode, r_bSpool:=bSpool, r_sSpoolDocumentCode:=sSpoolDocumentCode, r_bExport:=bExport)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DD 01/10/2003: If we are going to spool then get the Object
        If bSpool And sSpoolDocumentCode <> "" Then
            'Developer Guide no.108
            oDocManagerWrapper = New bSIRDocManagerWrapper.Interface_Renamed()
            'Developer Guide No 9
            If oDocManagerWrapper.Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If
        End If

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add batch ref as  input param for batch run
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRef", vValue:=v_sBatchRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add media type as  input param for batch run
        m_lReturn = m_oDatabase.Parameters.Add(sName:="MediaTypeCode", vValue:=v_sMediaTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add payment type as  input param for batch run
        m_lReturn = m_oDatabase.Parameters.Add(sName:="PaymentTypeCode", vValue:=v_sPaymentTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        'developer guide no. 39
        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_ProcessPaymentRun", sSQLName:="spu_ACT_ProcessPaymentRun", bStoredProcedure:=True, lNumberRecords:=lNumberRecords, vResultArray:=vDetailDataValues)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DD 01/10/2003: Spool the documents if set
        If bSpool And sSpoolDocumentCode <> "" Then

            For lRow As Integer = 0 To vDetailDataValues.GetUpperBound(1)
                'We must the template for each record as a template code
                'could be duplicated for each branch

                m_lReturn = GetDocumentTemplate(CInt(vDetailDataValues(31, lRow)), sSpoolDocumentCode, lDocumentTemplateID, lDocumentTemplateVersionID, lDocumentTypeID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Did we find one
                If lDocumentTemplateID > 0 Then

                    ' Set the appropriate properties
                    oDocManagerWrapper.DocumentTemplateId = lDocumentTemplateID
                    oDocManagerWrapper.DocumentTypeId = lDocumentTypeID


                    oDocManagerWrapper.CashListItemId = vDetailDataValues(31, lRow)
                    oDocManagerWrapper.SpoolDesc = "Payment Run Letter"

#If CODEBASE = 18 Then
                        '1.8 Platform prints directly
                        oDocManagerWrapper.Mode = ACPrintSilentMode
#Else

                    '1.9 Platform prints to Document Spooler
                    oDocManagerWrapper.Mode = ACSpoolDocMode
#End If

                    ' Call the Start Method
                    m_lReturn = oDocManagerWrapper.Start()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to print the document", vApp:=ACApp, vClass:=ACClass, vMethod:="PaymentRun", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If

            Next lRow

            'Shut down the spooler
            oDocManagerWrapper.Dispose()
            oDocManagerWrapper = Nothing
        End If

        'Now add the extra columns that the HUB requires

        If AddHUBColumnsToDetailArray(v_sStatusCode:=gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS, v_sStatusMsg:=gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS, r_vResultArray:=vDetailDataValues, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DD 01/10/2003: Only export if Media Type says so
        If bExport Then
            'add the result array to the detail array

            If AddResultArrayToDetailArray(v_vDetailArray:=r_vDetailData, r_vResultArray:=vDetailDataValues, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function

    Private Function SpoolReport(ByVal v_sBatchRef As String, ByVal v_sReportCode As String) As Integer

        Dim result As Integer = 0


        Dim oReportPrint As bSIRReportPrint.Business
        Dim sReportName As String = ""
        Dim vParameters, vDefaults As Object

        result = gPMConstants.PMEReturnCode.PMFalse

        ' Create an instance of the Account object
        oReportPrint = New bSIRReportPrint.Business
        If oReportPrint.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'Get the report name from the DB
        m_lReturn = GetReportName(v_sReportCode, sReportName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If


        oReportPrint.ReportName = sReportName

        'get the parameter (Batch ref) from the report template

        m_lReturn = oReportPrint.GetParameters(vParameters, vDefaults)
        'Set the parameter to the reuired value

        vParameters(0, 1) = v_sBatchRef

        'Print only (Don't view)

        oReportPrint.PrintReport = PMNavKeyConst.AC_PRINT_ONLY

        m_lReturn = oReportPrint.SendToPrint(v_vParameters:=vParameters)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oReportPrint = Nothing
            Return result
        End If

        oReportPrint = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function GetReportName(ByVal v_sReportCode As String, ByRef r_sReportName As String) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add batch ref as  input param for batch run
        m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sReportCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        'developer guide no. 39
        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_report_select", sSQLName:="spu_report_select", bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_sReportName = m_oDatabase.Records.Item(1).Fields("report_name")

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function GetPaymentRunType(ByVal v_sBatchRef As String, ByRef r_sPaymentType As String, ByRef r_sMediaTypeValidation As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add batch ref as  input param for batch run
        m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchRef", vValue:=v_sBatchRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        'developer guide no. 39
        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_ACT_PaymentRunType", sSQLName:="spu_ACT_PaymentRunType", bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_oDatabase.Records.Count() > 0 Then
            r_sMediaTypeValidation = m_oDatabase.Records.Item(1).Fields("MediaTypeValidationCode")
            r_sPaymentType = m_oDatabase.Records.Item(1).Fields("PaymentTypeCode")
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    Private Function PostCashListItems(ByVal v_sBatchRef As String, ByVal v_vDetailData(,) As Object) As gPMConstants.PMEReturnCode

        Dim oReversal As bACTDocumentReversal.Business
        Dim oCashListPost As bACTCashListPost.Automated
        Dim vTransDetailArray() As Object
        Dim lTransDetailID As Integer
        Dim cAmount As Decimal
        Dim lCashListItemID, lAccountId As Integer
        Dim bFoundAmalgamatedPayment As Boolean


        oReversal = New bACTDocumentReversal.Business
        m_lReturn = oReversal.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTDocumentReversal.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMError
        End If

        oCashListPost = New bACTCashListPost.Automated
        m_lReturn = oCashListPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCashListPost.Automated", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return gPMConstants.PMEReturnCode.PMError
        End If

        For l As Integer = 0 To v_vDetailData.GetUpperBound(1)

            If CDbl(v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColLevel, l)) = 2 Then
                bFoundAmalgamatedPayment = True
                'Go and get the TransDetail IDs that the payments are currently allocated to before
                'we reverse it, then we can pass this array in for allocation


                m_lReturn = GetAllocatedTransDetailIDs(CInt(v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColTransDetailID, l)), vTransDetailArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get allocated TransDetail IDs", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMError
                End If


                oReversal.DocumentId = 0


                oReversal.TransDetailId = v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColTransDetailID, l)

                m_lReturn = oReversal.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reverse cash list item", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMError
                End If

            Else


                If CDbl(v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColLevel, l)) = 1 And CDbl(v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColAmalgamated, l)) = 1 Then
                    bFoundAmalgamatedPayment = True


                    m_lReturn = oCashListPost.PostUnallocatedCash(v_vCashListID:=v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColCashListID, l), v_vCashListItemID:=v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColCashListItemID, l))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post unallocated cash", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMError
                    End If


                    lTransDetailID = oCashListPost.CashTransId

                    cAmount = CDec(v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColAmount, l))

                    lCashListItemID = CInt(v_vDetailData(gHUBSpokeConstants.knPaymentRunDetailColCashListItemID, l))

                End If
            End If
        Next l
        If bFoundAmalgamatedPayment Then
            m_lReturn = GetAccountID(lCashListItemID, lAccountId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get AccountID", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMError
            End If


            m_lReturn = Allocate(lAccountId, lTransDetailID, cAmount, vTransDetailArray)
        End If
        oReversal = Nothing
        oCashListPost = Nothing


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: Allocate
    '
    ' Description: .
    '
    ' Written By: Simon.Baynes
    '
    ' Returns:
    '
    ' Date: 08/10/2003
    '
    ' ***************************************************************** '
    Private Function Allocate(ByRef v_lAccountId As Integer, ByRef v_lTransDetailID As Integer, ByRef v_cAmount As Decimal, ByRef v_vTransDetailArray() As Object) As Integer


        Dim result As Integer = 0


        Dim oAllocationManual As bACTAllocationManual.Business

        result = gPMConstants.PMEReturnCode.PMFalse

        'Use the bACTAllocationManual component to do the allocation
        oAllocationManual = New bACTAllocationManual.Business
        If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        'Set keys for the AllocationManual component
        Dim vKeyArray As Array = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 3}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lAccountId

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(v_lTransDetailID) & "|" & CStr(v_cAmount * -1)

        vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_vTransDetailArray

        'Perform the allocation
        With oAllocationManual

            If .SetKeys(vKeyArray:=vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If


            If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
        End With

        result = gPMConstants.PMEReturnCode.PMTrue


        oAllocationManual.Dispose()
        oAllocationManual = Nothing

        Return result

    End Function


    Private Function GetMediaTypeSettings(ByVal sMediaTypeCode As String, ByRef r_bSpool As Boolean, ByRef r_sSpoolDocumentCode As String, ByRef r_bExport As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetMediaTypeSettings
        ' PURPOSE: Returns what should happen when a particular Media Type goes
        '          through the Payment Run.
        ' AUTHOR: Danny Davis
        ' DATE: 01 October 2003, 02:00 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lMediaTypeID As Integer
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("mediacode", sMediaTypeCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            'developer guide no. 39
            m_lReturn = .SQLSelect("spu_ACT_Get_MediaID_From_code", "Select Media Type", True, , vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Media Type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypeSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            'Get the returned ID

            lMediaTypeID = CInt(vResultArray(0, 0))

            .Parameters.Clear()
            .Parameters.Add("mediatype_id", CStr(lMediaTypeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no. 39
            m_lReturn = .SQLSelect("spu_ACT_Select_MediaType", "Select Media Type", True, , vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Media Type", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypeSettings", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMError
            End If


            r_bSpool = gPMFunctions.NullToBoolean(CStr(vResultArray(10, 0)))

            r_sSpoolDocumentCode = gPMFunctions.NullToString(CStr(vResultArray(11, 0)))

            r_bExport = gPMFunctions.NullToBoolean(CStr(vResultArray(12, 0)))
        End With



        Return result

    End Function

    Private Function GetDocumentTemplate(ByVal lCashListItemID As Integer, ByVal sDocumentTemplateCode As String, ByRef r_lDocumentTemplateID As Integer, ByRef r_lDocumentTemplateVersionID As Integer, ByRef r_lDocumentTypeID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetDocumentTemplate
        ' PURPOSE: Returns the appropriate Template ID and Type for a Code
        ' associated with a Cash List Item
        ' AUTHOR: Danny Davis
        ' DATE: 01 October 2003, 03:41 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("cashlistitem_id", CStr(lCashListItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("document_template_code", sDocumentTemplateCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            'Developer Guide no.108
            'Start
            .Parameters.Add("document_template_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            .Parameters.Add("document_template_version_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            .Parameters.Add("document_type_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            'developer guide no. 39
            m_lReturn = .SQLSelect("spu_ACT_Get_Document_Template_For_Payment", "Get Document Template for Payment", True)
            'Modification end
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Document Template", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentTemplate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMError
            End If

            r_lDocumentTemplateID = gPMFunctions.NullToLong(.Parameters.Item("document_template_id").Value)
            r_lDocumentTemplateVersionID = gPMFunctions.NullToLong(.Parameters.Item("document_template_version_id").Value)
            r_lDocumentTypeID = gPMFunctions.NullToLong(.Parameters.Item("document_type_id").Value)
        End With


        Return result


    End Function
End Class
