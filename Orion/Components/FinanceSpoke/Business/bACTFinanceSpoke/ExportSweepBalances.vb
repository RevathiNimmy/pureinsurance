Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportSweepBalances

    '====================================================================
    '   Class/Module: ExportSweepBalances
    '   Description : Class implementation of use case:
    'Export for InterfaceCode: "Sweep_Balances"'
    '
    '====================================================================
    '   Maintenance History
    '
    '   04/02/2003  SW Created
    '
    'return status and message
    '====================================================================

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportSweepBalances"

    '#Region " Private fields "
    Private m_lReturn As Integer
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

    '#Region " Stored Procedures "
    'developer guide no. 39
    'Private Const ACSBGetStatementDetailsSQL As String = "{call spu_ACT_Spoke_Get_Statement_Details (?)}"
    Private Const ACSBGetStatementDetailsSQL As String = "spu_ACT_Spoke_Get_Statement_Details"
    Private Const ACSBGetStatementDetailsName As String = "GetStatementDetails"

    'developer guide no. 39
    Private Const ACSBGetAccountIDFromBankAccountIDSQL As String = "spu_ACT_Spoke_Get_AccountIDFromBankAccountID"
    Private Const ACSBGetAccountIDFromBankAccountIDName As String = "GetAccountIDFromBankAccountID"

    'developer guide no. 39
    Private Const ACSBInsertBankReconcileTransDetailSQL As String = "spu_ACT_Spoke_Insert_Bank_Rec_TD"
    Private Const ACSBInsertBankReconcileTransDetailName As String = "InsertBankReconcileTransDetail"

    'developer guide no. 39
    Private Const ACSBUpdateMatchDateSQL As String = "spu_ACT_Spoke_UpdateMatchDate"
    Private Const ACSBUpdateMatchDateName As String = "UpdateMatchDate"

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


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: Start
    ' PURPOSE: Start process for use case
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        Dim lMainBAAccountID, lMainBAID As Integer
        Dim vStatementDetails(,) As Object
        Dim lRowCount As Integer
        Dim vReconcileTransID As Object
        Dim lDebitTransID, lPostingAccountID As Integer
        Dim bTransStarted As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'first we need to look up the Main Bank Account from systemoptions

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            'look up main bank account from system options
            If GetMainBankAccount(lMainBAID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to establish main bank account")
                Return result
            End If

            'get the statement details

            If GetStatementDetails(lMainBAID, vStatementDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get statement details from the database")
                Return result
            End If

            'DD 27/08/2003: Validate if Array returned
            If Not Information.IsArray(vStatementDetails) Then
                'nothing to process exit without error
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            'get the account id for the main bank account
            If GetAccountIDForBankAccount(lMainBAID, lMainBAAccountID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to establish account ID for main bank account")
                Return result
            End If


            lRowCount = vStatementDetails.GetUpperBound(1)

            'begin a transaction for the import

            If m_oDatabase.SQLBeginTrans() = gPMConstants.PMEReturnCode.PMTrue Then
                bTransStarted = True
            Else
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to create transaction")
                Return result
            End If

            For lRow As Integer = 0 To lRowCount

                'reset this variables
                vReconcileTransID = 0
                lPostingAccountID = 0

                'now find the account id for the clients bank account

                If GetAccountIDForBankAccount(CInt(vStatementDetails(ACSBBankAccountID, lRow)), lPostingAccountID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to establish account ID for main bank account")
                    Return result
                End If


                'process the individual statement entry

                If Conversion.Val(CStr(vStatementDetails(ACSBCreditAmount, lRow))) > 0 Then

                    'post debit to account id for transaction bank account

                    'post credit to then main bank accounts account


                    m_lReturn = m_oBusiness.PostTransaction(v_vCreditAccount:=lMainBAAccountID, v_vDebitAccount:=lPostingAccountID, v_sComment:="", v_dTotalAmount:=CDbl(vStatementDetails(ACSBCreditAmount, lRow)), r_vNewDebitTransDetailId:=vReconcileTransID, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error posting transactions")
                        Return result
                    End If

                Else

                    'post credit to account id for transaction bank account

                    'post debit to then main bank accounts account


                    m_lReturn = m_oBusiness.PostTransaction(v_vCreditAccount:=lPostingAccountID, v_vDebitAccount:=lMainBAAccountID, v_sComment:="", v_dTotalAmount:=CDbl(vStatementDetails(ACSBDebitAmount, lRow)), r_vNewCreditTransDetailId:=vReconcileTransID, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error posting transactions")
                        Return result
                    End If
                End If


                If InsertBankReconcileTransDetail(vReconcileTransID, CInt(vStatementDetails(ACSBBankStatementDetailID, lRow))) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error creating reconcile transaction record")
                    Return result
                End If

                'Insert record into bank_reconcile_transdetail

                If UpdateMatchDate(CInt(vStatementDetails(ACSBBankStatementDetailID, lRow)), DateTime.Today) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error creating reconcile transaction record")
                    Return result
                End If

            Next

            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error commiting transaction")
                Return result
            End If

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

            If bTransStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If

            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    '#End Region



    ' ***************************************************************** '
    ' Name: GetStatementDetails
    '
    ' Description: Get the statement details for the export
    '
    ' ***************************************************************** '
    Private Function GetStatementDetails(ByRef v_lMainBankAccountID As Integer, ByRef r_vStatementDetails(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="mainbankaccountid", vValue:=CStr(v_lMainBankAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACSBGetStatementDetailsSQL, sSQLName:=ACSBGetStatementDetailsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vStatementDetails) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Name: InsertBankReconcileTransDetail
    '
    ' Description: inserts a record in the Bank_reconcile_transDetail table
    '
    ' ***************************************************************** '
    Private Function InsertBankReconcileTransDetail(ByRef v_lTransDetailID As Integer, ByRef v_lStatementDetailID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="transdetailid", vValue:=CStr(v_lTransDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="statementdetailid", vValue:=CStr(v_lStatementDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACSBInsertBankReconcileTransDetailSQL, sSQLName:=ACSBInsertBankReconcileTransDetailName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Name: UpdateMatchDate
    '
    ' Description: updates the match date field
    '
    ' ***************************************************************** '
    Private Function UpdateMatchDate(ByRef v_lStatementDetailID As Integer, ByRef v_dtMatchDate As Date) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="statementdetailid", vValue:=CStr(v_lStatementDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Add mediacode as an input param
        If m_oDatabase.Parameters.Add(sName:="matchdate", vValue:=DateTimeHelper.ToString(v_dtMatchDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACSBUpdateMatchDateSQL, sSQLName:=ACSBUpdateMatchDateName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Name: GetmainBankAccount (Private)
    '
    ' Description: Get the main bank account from system options
    '
    ' ***************************************************************** '
    Private Function GetMainBankAccount(ByRef r_lMainBAID As Integer) As Integer

        Dim result As Integer = 0
        Dim oSystemOption As bSIROptions.Business
        Dim sAccountString As String = ""
        Dim vResultArray As Object

        Const cMainBankAccountOption As Integer = 77




        result = gPMConstants.PMEReturnCode.PMFalse

        'Get Instance of System Option Business

        oSystemOption = New bSIROptions.Business
        m_lReturn = oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oSystemOption.GetOption(iOptionNumber:=cMainBankAccountOption, sValue:=sAccountString)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the account details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMainBankAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If


        oSystemOption.Dispose()

        

        oSystemOption = Nothing

        r_lMainBAID = CInt(sAccountString)


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function



    ' ***************************************************************** '
    ' Name: GetAccountIDForBankAccount (Private)
    '
    ' Description: Get the account ID For a given bank Account
    '
    ' ***************************************************************** '
    Private Function GetAccountIDForBankAccount(ByVal v_lBankAccountID As Integer, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add bankaccountid as an input param
        If m_oDatabase.Parameters.Add(sName:="bankaccountid", vValue:=CStr(v_lBankAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACSBGetAccountIDFromBankAccountIDSQL, sSQLName:=ACSBGetAccountIDFromBankAccountIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        'return the account ID for the main bank account

        If Information.IsArray(vResultArray) Then

            r_lAccountID = CInt(vResultArray(0, 0))
            Return gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountIDForBankAccount failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountIDForBankAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
End Class

