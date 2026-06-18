Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportStaleCheques

    '====================================================================
    '   Class/Module: ExportStaleCheques
    '   Description : Class implementation of use case:
    'Export for InterfaceCode: "STALE_CHEQUES"
    '
    '====================================================================
    '   Maintenance History
    '
    '   05/02/2003  SW Created
    '
    'return status and message
    '====================================================================

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportSweepBalances"
    'developer guide no. 39
    Private Const ACSCGetStaleChequesSQL As String = "spu_ACT_Spoke_Get_Stale_Cheques"
    Private Const ACSCGetStaleChequesName As String = "GetStaleCheques"

    'developer guide no. 39
    Private Const ACSCGetAccountIDFromBankAccountIDSQL As String = "spu_ACT_Spoke_Get_AccountIDFromBankAccountID"
    Private Const ACSCGetAccountIDFromBankAccountIDName As String = "GetAccountIDFromBankAccountID"

    'developer guide no. 39
    Private Const ACSCUpdateCashListItemStaleSQL As String = "spu_ACT_Spoke_Update_CashListItem_Stale"
    Private Const ACSCUpdateCashListItemStaleName As String = "ACUpdateCashListItemStale"

    'End sw

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
    ' CHANGES: DD 27/08/2003: Tidied up, corrected code and removed transactions
    ' from here as they cause deadlocks.
    ' ---------------------------------------------------------------------------

    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByVal v_vHeaderData() As Object) As Integer

        Dim result As Integer = 0
        Dim vHeaderInfo As Object
        Dim lMonths As Integer
        Dim sMediaTypeCode As String = ""
        Dim dtCutOffDate As Date
        Dim vPaymentDetails(,) As Object
        Dim lRowCount, lStaleChequeAccountID As Integer
        Dim oReversal As Object
        Dim vCreditTransDetail, vNewDebitTransDetID As Object
        Dim oAllocate As bACTAllocate.Business

        Dim r_vTransDetails(2, 1) As Object



        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED



            vHeaderInfo = v_vHeaderData(1)


            lMonths = CInt(vHeaderInfo(ACSCMonths))

            sMediaTypeCode = CStr(vHeaderInfo(ACSCMediaTypeCode))

            'find the cut off date
            dtCutOffDate = DateTime.Today.AddMonths(-lMonths)

            'get the payment details

            m_lReturn = GetPaymentDetails(sMediaTypeCode, dtCutOffDate, vPaymentDetails)

            If Not Information.IsArray(vPaymentDetails) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                'Return codes
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
                Return result
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get payment details")
                Return result
            End If

            'create the Allocation object required for processing

            oAllocate = New bACTAllocate.Business
            m_lReturn = oAllocate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            'get the stalechequeaccountID
            m_lReturn = GetStaleChequeAccount(lStaleChequeAccountID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to get stale cheque details")
                Return result
            End If

            'loop through the payments returned from getpaymentdetails

            lRowCount = vPaymentDetails.GetUpperBound(1)

            For lRow As Integer = 0 To lRowCount


                If CStr(vPaymentDetails(ACSCPaymentTypeCode, lRow)) = "CLP" Then





                    m_lReturn = m_oBusiness.PostTransaction(v_vCreditAccount:=CInt(vPaymentDetails(ACSCCashListBankAccount, lRow)), v_vDebitAccount:=CInt(vPaymentDetails(ACSCAccountID, lRow)), v_sComment:="", v_dTotalAmount:=CDbl(vPaymentDetails(ACSCPaymentAmount, lRow)), r_vNewCreditTransDetailId:=vNewDebitTransDetID, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error posting transactions")
                        Return result
                    End If

                Else




                    m_lReturn = m_oBusiness.PostTransaction(v_vCreditAccount:=CInt(vPaymentDetails(ACSCCashListBankAccount, lRow)), v_vDebitAccount:=lStaleChequeAccountID, v_sComment:="", v_dTotalAmount:=CDbl(vPaymentDetails(ACSCPaymentAmount, lRow)), r_vNewCreditTransDetailId:=vNewDebitTransDetID, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error posting transactions")
                        Return result
                    End If

                End If

                'build up the array to be passed to the allocation object



                r_vTransDetails(0, 0) = CInt(vPaymentDetails(ACSCPaymentTransdetail, lRow))


                r_vTransDetails(1, 0) = vPaymentDetails(ACSCPaymentAmount, lRow)


                r_vTransDetails(2, 0) = vPaymentDetails(ACSCPaymentAmount, lRow)



                r_vTransDetails(0, 1) = CInt(vNewDebitTransDetID)


                r_vTransDetails(1, 1) = CDbl(vPaymentDetails(ACSCPaymentAmount, lRow)) * -1


                r_vTransDetails(2, 1) = CDbl(vPaymentDetails(ACSCPaymentAmount, lRow)) * -1




                If oAllocate.PerformAutoAllocation(r_lAccountID:=CInt(vPaymentDetails(ACSCCashListBankAccount, lRow)), r_lTransDetailId:=CInt(vNewDebitTransDetID), v_vOSTransactions:=r_vTransDetails) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Unable to allocate total")
                End If

                'update the cashlistitem payment status = stale

                If UpdateCashListItemStale(CInt(vPaymentDetails(ACSCPaymentCashListItemID, lRow))) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Unable to update cashlistitem payment status")
                End If
            Next


            oAllocate.Dispose()
            oAllocate = Nothing

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE


            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

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



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPaymentDetails
    ' PURPOSE: gets the payment details where stale cheques are present
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------

    Private Function GetPaymentDetails(ByVal v_sMediaCode As String, ByVal v_dtCutOffDate As String, ByRef r_vResultArray(,) As Object) As gPMConstants.PMEReturnCode

        ' Clear the Database Parameters Collection
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        m_oDatabase.Parameters.Clear()

        ' Add bankaccountid as an input param
        If m_oDatabase.Parameters.Add(sName:="date", vValue:=v_dtCutOffDate, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Add bankaccountid as an input param
        If m_oDatabase.Parameters.Add(sName:="mediacode", vValue:=v_sMediaCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACSCGetStaleChequesSQL, sSQLName:=ACSCGetStaleChequesName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPaymentDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPaymentDetails
    ' PURPOSE: gets the stale cheque account from system options
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetStaleChequeAccount(ByRef r_lStaleChequeAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim oSystemOption As bSIROptions.Business
        Dim sAccountString As String = ""
        Dim vResultArray As Object

        Const cStaleChequeAccountOption As Integer = 78



        result = gPMConstants.PMEReturnCode.PMFalse

        'Get Instance of System Option Business

        oSystemOption = New bSIROptions.Business
        m_lReturn = oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oSystemOption.GetOption(iOptionNumber:=cStaleChequeAccountOption, sValue:=sAccountString)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the account details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStaleChequeAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If


        oSystemOption.Dispose()

        

        oSystemOption = Nothing

        m_lReturn = GetAccountIDForBankAccount(CInt(sAccountString), r_lStaleChequeAccountID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the account id for the stale cheque account", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStaleChequeAccount", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function







    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetPaymentDetails
    ' PURPOSE: gets the account Id for the passed bank account id
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetAccountIDForBankAccount(ByVal v_lBankAccountID As Integer, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add bankaccountid as an input param
        If m_oDatabase.Parameters.Add(sName:="bankaccountid", vValue:=CStr(v_lBankAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ACSCGetAccountIDFromBankAccountIDSQL, sSQLName:=ACSCGetAccountIDFromBankAccountIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

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



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateCashListItemStale
    ' PURPOSE: updates the cashlistitem payment status to stale
    ' AUTHOR: Steve Watton
    ' DATE: 06/02/2003
    ' RETURNS: PMTrue for success
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function UpdateCashListItemStale(ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the eventtypecode parameter
        If m_oDatabase.Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACSCUpdateCashListItemStaleSQL, sSQLName:=ACSCUpdateCashListItemStaleName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Class

