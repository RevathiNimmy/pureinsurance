Option Strict Off
Option Explicit On
'Imports Artinsoft.VB6.Utils
Imports System.Text
'developer guide no. 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable



    ' ************************************************
    ' Module variables
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"


    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lError As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_oCaseNumbering As Object
    Private m_oEvent As bSIREvent.Business
    Dim m_oLock As bpmlock.User

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' Date :18/06/2007
    '
    ' Edit History :Gaurav Arora
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Set Username and Password
            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav Arora
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :15/07/2000
    '
    ' Edit History :Gaurav Arora
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Sub New()
        MyBase.New()

        Try

            Dim vDatabase As Object = Nothing

            ' Class Initialise
            m_oDatabase = New dPMDAO.Database()


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    'developer guide no. 214
    Private Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtEventDate As Date, ByVal v_lEventTypeId As Integer, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentTypeId As Object = Nothing, Optional ByVal v_vReportTypeId As Object = Nothing, Optional ByVal v_vDescription As Object = Nothing) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oEvent Is Nothing Then
            m_oEvent = New bSIREvent.Business()
            m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oEvent.Initialise", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_vDescription)

        Return result
    End Function



    ' ***************************************************************** '
    ' Name: BeginTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BeginTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oDatabase.SQLBeginTrans

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLBeginTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function




    ' ***************************************************************** '
    ' Name: RollbackTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RollbackTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oDatabase.SQLRollbackTrans

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SQLRollbackTrans Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CommitTrans"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = m_oDatabase.SQLCommitTrans

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    'Developer Guide no. 101
    Public Function GetCashDepositDetails(ByRef r_vCashDepositDetails(,) As Object, ByVal v_sParty_Code As String, Optional ByVal v_sCashDeposit_Ref As String = "", Optional ByVal v_sBankCode As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCashDepositDetails"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Code", v_sParty_Code, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            If gPMFunctions.ToSafeString(v_sCashDeposit_Ref) <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_Ref", v_sCashDeposit_Ref, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If
            If gPMFunctions.ToSafeString(v_sBankCode) <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Bank_Code", v_sBankCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELCashDepositDetailsSQL, sSQLName:=ACSELCashDepositDetailsName, bStoredProcedure:=True, vResultArray:=r_vCashDepositDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELCashDepositDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(r_vCashDepositDetails) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetNextCashDepositNumber(ByVal v_lPartyID As Integer, ByRef r_lCashDepositNumber As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetNextCashDepositNumber"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Party_ID", v_lPartyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "NextNumber", r_lCashDepositNumber, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNextCashDepositNumberSQL, sSQLName:=ACGetNextCashDepositNumberName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetNextCashDepositNumberSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                r_lCashDepositNumber = m_oDatabase.Parameters.Item("NextNumber").Value
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    Public Function CreateCDAccount(ByRef r_lAccountId As Integer, ByVal v_sShortCode As String, ByVal v_sShortName As String, ByVal v_sPartyType As String, ByVal v_lPartyID As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "CreateCDAccount"



        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oLedger As bSirOrionLink.Form
            Dim lSubBranchId As Integer
            Dim oOrionAccount As bACTAccount.Form
            Dim oOrionExplorer As bACTExplorer.Form
            Dim oOrionLedger As bACTLedger.Form
            Dim lAccountID As Integer
            Dim lSalesLedgerID, lPurchaseLedgerID, lInsurerLedgerId, lAgentLedgerID, lFeeLedgerId, lCommissionLedgerID, lDiscountLedgerID, lPRemiumFinanceLedgerId, lSubAgentLedgerId, lNominalLedgerId, lOtherPartyPayLedgerId, lOtherPartyRecLedgerId, lIntroducerLedgerId, lNodeId, lElementId As Integer
            Dim cCurrencyID As Decimal
            Dim lPartyTypeId As Integer


            oOrionAccount = New bACTAccount.Form
            m_lReturn = oOrionAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Create an instance of the Orion Ledger object

            oOrionLedger = New bACTLedger.Form
            m_lReturn = oOrionLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTLedger.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Create an instance of the Orion Ledger object

            oLedger = New bSirOrionLink.Form
            m_lReturn = oLedger.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTLedger.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            ' Create an instance of the Orion Account Explorer object

            oOrionExplorer = New bACTExplorer.Form
            m_lReturn = oOrionExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialise bACTExplorer.Form", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            m_lReturn = oLedger.GetLedgerIDs(r_lSalesLedgerID:=lSalesLedgerID, r_lPurchaseLedgerID:=lPurchaseLedgerID, r_lInsurerLedgerId:=lInsurerLedgerId, r_lAgentLedgerId:=lAgentLedgerID, r_lFeeLedgerId:=lFeeLedgerId, r_lCommissionLedgerId:=lCommissionLedgerID, r_lDiscountLedgerId:=lDiscountLedgerID, r_lPremiumFinanceLedgerId:=lPRemiumFinanceLedgerId, r_lSubAgentLedgerId:=lSubAgentLedgerId, r_lNominalLedgerID:=lNominalLedgerId, r_lOtherPartyPayLedgerID:=lOtherPartyPayLedgerId, r_lOtherPartyRecLedgerID:=lOtherPartyRecLedgerId, r_lIntroducerLedgerId:=lIntroducerLedgerId, v_lSubBranchID:=lSubBranchId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetLedgerIDs Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetPartyDetails(v_lPartyID, lSubBranchId, cCurrencyID, lPartyTypeId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lPartyTypeId = 3 Then
                lSalesLedgerID = lAgentLedgerID
            End If

            m_lReturn = oOrionAccount.DirectAdd(vAccountID:=lAccountID, vAccounttypeID:=ACTAccountTypeLiability, vPurgefrequencyID:=ACTPurgeFreqNever, vCurrencyID:=cCurrencyID, vLedgerId:=lSalesLedgerID, vAccountName:=v_sShortName, vShortCode:=v_sShortCode, vAccountStatusID:=ACTAccountStatusActive)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "oOrionAccount.DirectAdd Failed", gPMConstants.PMELogLevel.PMLogError)

                oOrionAccount.Dispose()
                oOrionAccount = Nothing
                Return result
            End If

            r_lAccountId = lAccountID


            oOrionAccount.Dispose()
            oOrionAccount = Nothing


            oOrionLedger.Dispose()
            oOrionLedger = Nothing


            lElementId = oOrionExplorer.InsertElement(v_sShortCode)


            m_lReturn = oOrionExplorer.GetNodeFromMappingText(v_sMappingText:=kCashDepositTitle, v_lNodeId:=lNodeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetNodeFromMappingText Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If lElementId > 0 Then

                lNodeId = oOrionExplorer.InsertNode(lParentNodeId:=lNodeId, lElementId:=lElementId, vAccountID:=lAccountID)
            Else
                gPMFunctions.RaiseError(kMethodName, "oOrionExplorer.InsertElement Failed ", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If


            oOrionExplorer.Dispose()
            oOrionExplorer = Nothing

            Return result

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result

        Finally

        End Try
        Return result
    End Function



    Private Function GetPartyDetails(ByVal v_lPartyID As Integer, ByRef r_lSubBranchId As Integer, ByRef r_cCurrencyId As Decimal, Optional ByRef r_lPartyType As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const kMethodName As String = "GetPartyDetails"


        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()

        With m_oDatabase


            m_lReturn = .Parameters.Add(sName:="Party_cnt", vValue:=v_lPartyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Adding the parameter (Party_cnt) " & " failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = .SQLSelect(sSQL:=ACGETPartyDetailsSQL, sSQLName:=ACGETPartyDetailsName, bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SP " & ACGETPartyDetailsSQL & " calling failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End With


        If Not Object.Equals(vResultArray, Nothing) And Informations.IsArray(vResultArray) Then
            r_lPartyType = gPMFunctions.ToSafeLong(vResultArray(1, 0))
            r_lSubBranchId = gPMFunctions.ToSafeLong(vResultArray(4, 0))
            r_cCurrencyId = gPMFunctions.ToSafeLong(vResultArray(9, 0))
        Else

        End If






        Return result

    End Function

    Public Function GetAllBranches(Optional ByRef r_vGetAllBranches(,) As Object = Nothing, Optional ByVal v_lCashDepositId As Integer = 0, Optional ByRef r_vGetCashDepositBranches(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAllBranches"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If v_lCashDepositId > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashDepositBranchesQL, sSQLName:="ACGetCashDepositBranchesName", bStoredProcedure:=True, vResultArray:=r_vGetCashDepositBranches)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACGetCashDepositBranchesQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                Return result

            Else


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllBranchesSQL, sSQLName:=ACGetAllBranchesName, bStoredProcedure:=True, vResultArray:=r_vGetAllBranches)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACGetAllBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                Return result

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function GetAllProducts(Optional ByRef r_vGetAllProducts(,) As Object = Nothing, Optional ByVal v_lCashDepositId As Integer = 0, Optional ByRef r_vGetCashDepositProducts(,) As Object = Nothing) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetAllProducts"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            If v_lCashDepositId > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCashDepositProductsSQL, sSQLName:=ACGetCashDepositProductsName, bStoredProcedure:=True, vResultArray:=r_vGetCashDepositProducts)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACGetCashDepositProductsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Return result

            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "Filter_Columns", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllProductsSQL, sSQLName:=ACGetAllProductsName, bStoredProcedure:=True, vResultArray:=r_vGetAllProducts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACGetAllProductsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                Return result
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function AddCashDeposit(ByRef r_lCashDepositId As Integer, ByVal v_sCashDeposit_Ref As Object, ByVal v_lAccount_ID As Object, ByVal v_lParty_ID As Integer, ByVal v_sPartyName As Object, ByVal v_sPartyType As String, ByVal v_iIs_SinglePolicy As Integer, ByVal v_lUser_ID As Object, ByVal v_vBranches(,) As Object, ByVal v_vProducts(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddCashDeposit"
        Dim lEventCnt, m_lAccountId As Integer
        Dim sDescription As String = ""


        Try


            BeginTrans()
            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (Convert.IsDBNull(v_lParty_ID) Or Informations.IsNothing(v_lParty_ID)) Then

                'Create Account
                m_lReturn = CType(CreateCDAccount(r_lAccountId:=m_lAccountId, v_sShortCode:=v_sCashDeposit_Ref, v_sShortName:=v_sPartyName, v_sPartyType:=v_sPartyType, v_lPartyID:=v_lParty_ID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateCDAccount Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                ' Clear Down Database Parameters

                m_oDatabase.Parameters.Clear()

                bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", r_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_Ref", v_sCashDeposit_Ref, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                bPMAddParameter.AddParameterLite(m_oDatabase, "Account_ID", m_lAccountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "Party_ID", v_lParty_ID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "User_ID", v_lUser_ID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "Is_SinglePolicy", v_iIs_SinglePolicy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDCashDepositDetailsSQL, sSQLName:=ACADDCashDepositDetailsName, bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACADDCashDepositDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                Else

                    r_lCashDepositId = m_oDatabase.Parameters.Item("CashDeposit_ID").Value
                End If

                m_lReturn = CType(AddBranches(r_lCashDepositId, v_vBranches), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddBranches Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = CType(AddProducts(r_lCashDepositId, v_vProducts), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddProducts Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                sDescription = "Cash Deposit Details Created - "
                sDescription = sDescription &
                               v_sPartyName & ", " &
                               v_sCashDeposit_Ref & ", " &
                               m_sUsername & ", "

                m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=v_lParty_ID, v_lEventTypeId:=PMBConst.PMBEventNewClient, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Create Event Failed for Cash Deposit Ref " & v_sCashDeposit_Ref, gPMConstants.PMELogLevel.PMLogError)
                End If
                If v_sPartyType = "A" Then
                    ' Generate Event Log
                    m_lReturn = CType(CreateEventLog(v_lPartyCnt:=v_lParty_ID, v_sDescription:=sDescription), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If
            CommitTrans()
            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Public Function UpdateCashDeposit(ByVal v_lCashDepositId As Integer, ByVal v_iIsSinglePolicy As Integer, ByVal v_vBranches(,) As Object, ByVal v_vProducts(,) As Object, ByVal v_lParty_ID As Integer, ByVal v_sCashDepositRef As String, ByVal v_sPartyName As String, ByVal v_sPreviousUserName As String, ByVal v_sPartyType As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCashDeposit"

        Try


            Dim sDescription As String = ""
            Dim lEventCnt As Integer
            BeginTrans()
            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Is_SinglePolicy", v_iIsSinglePolicy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUPDCashDepositDetailsSQL, sSQLName:=ACUPDCashDepositDetailsName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACUPDCashDepositDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(DeleteBranches(v_lCashDepositId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DeleteBranches Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(DeleteProducts(v_lCashDepositId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DeleteProducts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(AddBranches(v_lCashDepositId, v_vBranches), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddBranches Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(AddProducts(v_lCashDepositId, v_vProducts), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "AddProducts Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            sDescription = "Cash Deposit Details Edited from -"
            sDescription = sDescription &
                           v_sPartyName & ", " &
                           v_sCashDepositRef & ", " &
                           v_sPreviousUserName

            sDescription = sDescription & " TO "
            sDescription = sDescription &
                           v_sPartyName & ", " &
                           v_sCashDepositRef & ", " &
                           m_sUsername

            m_lReturn = CType(CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=v_lParty_ID, v_lEventTypeId:=PMBConst.PMBEventClientNotes, v_dtEventDate:=DateTime.Today, v_vDescription:=sDescription), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Create Event Failed for Cash Deposit Ref " & v_sCashDepositRef, gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_sPartyType = "A" Then
                ' Generate Event Log
                m_lReturn = CType(CreateEventLog(v_lPartyCnt:=v_lParty_ID, v_sDescription:=sDescription), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "CreateEventLog Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            CommitTrans()
            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()

        Finally

            '        Return result
            '        Resume


            '        Return result
        End Try
        Return result
    End Function

    Public Function AddBranches(ByVal v_lCashDepositId As Integer, ByVal v_vBranches(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddBranches"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            If Not (v_vBranches Is Nothing) And Informations.IsArray(v_vBranches) Then
                For lCount As Integer = 0 To v_vBranches.GetUpperBound(0)

                    m_oDatabase.Parameters.Clear()
                    bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Branch_ID", gPMFunctions.ToSafeLong(v_vBranches(lCount, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDCashDepositBranchSQL, sSQLName:=ACADDCashDepositBranchName, bStoredProcedure:=True)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACADDCashDepositBranchSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next lCount
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume


            '        Return result
        End Try
        Return result
    End Function
    Public Function AddProducts(ByVal v_lCashDepositId As Integer, ByVal v_vProducts(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddProducts"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            If Not (v_vProducts Is Nothing) And Informations.IsArray(v_vProducts) Then
                For lCount As Integer = 0 To v_vProducts.GetUpperBound(0)

                    m_oDatabase.Parameters.Clear()
                    ' Add Required Stored Procedure Parameters
                    bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Product_ID", gPMFunctions.ToSafeLong(v_vProducts(lCount, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACADDCashDepositProductSQL, sSQLName:=ACADDCashDepositProductName, bStoredProcedure:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, ACADDCashDepositProductSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next lCount
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume



            '        Return result
        End Try
        Return result
    End Function

    Private Function DeleteBranches(ByVal v_lCashDepositId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DeleteBranches"




        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELCashDepositBranchSQL, sSQLName:=ACDELCashDepositBranchName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACDELCashDepositBranchSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function

    Private Function DeleteProducts(ByVal v_lCashDepositId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DeleteProducts"




        result = gPMConstants.PMEReturnCode.PMTrue


        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDELCashDepositProductSQL, sSQLName:=ACDELCashDepositProductName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, ACDELCashDepositProductSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListLoad
    '
    ' Description: Standard method for the PickList control to call
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function PickListLoad(ByVal sPickListType As String, ByVal vFKArray As Object, ByRef vResultArray(,) As Object, Optional ByRef sWhereClause As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PickListLoad"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                'Load the parameters

                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)



                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=vFKArray(1, iParam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=CInt(vFKArray(2, iParam)))
                Next iParam

                'Call the SP

                'developer guide no.39
                m_lReturn = .SQLSelect("spu_CashDeposit_" &
                   sPickListType & "_Sel", sPickListType & " PickList Load", True, , vResultArray)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PickList Selection Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End With

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListParams
    '
    ' Description: Returns a string of question marks for the SP definition
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Private Function PickListParams(ByRef vParams(,) As Object) As String

        Dim sComma As String = ""
        Dim sParam As New StringBuilder

        For iParam As Integer = vParams.GetLowerBound(1) To vParams.GetUpperBound(1)
            sParam.Append(sComma & "?")
            sComma = ","
        Next iParam

        Return sParam.ToString()

    End Function

    ' ***************************************************************** '
    '
    ' Name: PickListSave
    '
    ' Description:
    '
    ' History: 26/09/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PickListSave(ByRef sPickListType As String, ByRef vFKArray(,) As Object, ByRef vKeys As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "PickListSave"
        Try




            result = gPMConstants.PMEReturnCode.PMTrue

            BeginTrans()

            If vFKArray.GetUpperBound(1) > 2 And sPickListType.Trim().ToUpper() = "SOURCE" Then
                ReDim Preserve vFKArray(vFKArray.GetUpperBound(0), vFKArray.GetUpperBound(1) - 1)
            End If

            With m_oDatabase

                'clear the old data

                .Parameters.Clear()

                'Load the FK parameters
                For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                    .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=vFKArray(1, iParam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Next iParam


                'developer guide no. 39
                m_lReturn = .SQLAction("spe_PFScheme_PLD" &
                            sPickListType, sPickListType & " PickList Delete", True)

                'See if there is anything to save
                If Informations.IsArray(vKeys) Then

                    For iKey As Integer = vKeys.GetLowerBound(0) To vKeys.GetUpperBound(0)

                        .Parameters.Clear()

                        'Load the FK parameters
                        For iParam As Integer = vFKArray.GetLowerBound(1) To vFKArray.GetUpperBound(1)


                            .Parameters.Add(sName:=CStr(vFKArray(0, iParam)), vValue:=vFKArray(1, iParam), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Next iParam


                        .Parameters.Add("Key", vKeys(iKey), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        'Call the SP

                        'developer guide no. 39
                        m_lReturn = .SQLAction("spu_CashDeposit_PLS" &
                                    sPickListType, sPickListType & " PickList Load", True)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "PickList Save Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Next iKey
                End If
            End With

            CommitTrans()

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    '************************************************************************************
    'create a lock for specified key and value
    '************************************************************************************
    Public Function LockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer, ByRef r_sLockedBy As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LockKey"
        Try

            m_oLock = New bpmlock.User
            If m_oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Create Object for bPMLock.User Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            result = m_oLock.LockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID, sCurrentlyLockedBy:=r_sLockedBy)

            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally

        End Try
        Return result
    End Function

    '************************************************************************************
    'unlock specified key
    '************************************************************************************
    Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnLockKey"
        Try

            m_oLock = New bpmlock.User
            If m_oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                gPMFunctions.RaiseError(kMethodName, "Create Object for bPMLock.User Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            result = m_oLock.UnLockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID)

            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    Public Function GetLinkedCashDepositAccounts(ByRef r_vGetCashDepositAccounts(,) As Object, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lAccount_ID As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLinkedCashDepositAccounts"



        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Input parameters for both PartyCnt and AccountId
            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            If v_lPartyCnt > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If
            If v_lAccount_ID > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Account_ID", v_lAccount_ID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELinkedCashDepositAccountsSQL, sSQLName:=ACSELinkedCashDepositAccountsName, bStoredProcedure:=True, vResultArray:=r_vGetCashDepositAccounts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELinkedCashDepositAccountsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(r_vGetCashDepositAccounts) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'End - Sankar - (Tech Spec -WPR85_Cash_Deposit_Process Part 1.doc) - (5.1.6.2.9)

    ' ***************************************************************** '
    ' Name: CreateEventLog
    '
    ' Parameters: PartyCnt, Description
    '
    ' Description: To Generate Event Log
    '
    ' History:
    ' ***************************************************************** '
    Private Function CreateEventLog(ByVal v_lPartyCnt As Object, ByVal v_sDescription As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateEventLog"

        Dim r_vResults(,) As Object = Nothing
        Dim vPublicTextId As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters

        If CDbl(v_lPartyCnt) > 0 Then

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetEventLogSQL, sSQLName:=ACGetEventLogName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetEventLogSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(r_vResults) Then

                For lCount As Integer = 0 To r_vResults.GetUpperBound(1)


                    vPublicTextId = r_vResults(1, lCount)
                Next


                vPublicTextId = CDbl(vPublicTextId) + 1
            Else

                vPublicTextId = 1
            End If


            m_oDatabase.Parameters.Clear()
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_public_text_id", vPublicTextId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            'Developer Guide no. 40
            bPMAddParameter.AddParameterLite(m_oDatabase, "text_line", "[SIRIUS " & DateTime.Now & "]" & Strings.ChrW(9) & Strings.ChrW(9), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddEventLogSQL, sSQLName:=ACAddEventLogName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACAddEventLogSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_oDatabase.Parameters.Clear()


            vPublicTextId = CDbl(vPublicTextId) + 1
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "party_public_text_id", vPublicTextId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "text_line", v_sDescription & Strings.ChrW(9) & Strings.ChrW(9), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddEventLogSQL, sSQLName:=ACAddEventLogName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACAddEventLogSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        Return result
    End Function

    'Start - (Renuka) - Tech Spec -UIIC_WPR85_Cash_Deposit_Process-Part 2
    Public Function GetCDsForPolicy(ByVal v_lPartyCnt As Integer, ByVal v_lProductId As Integer, ByVal v_lSourceID As Integer, ByVal v_crTotalPremium As Decimal, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vPrePayment As Object, ByVal v_dtCoverStartDate As Date, ByVal v_dtPolicyIssueDate As Date, ByRef r_vCashDepositDetails(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetCDsForPolicy"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Product_ID", v_lProductId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Branch_ID", v_lSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Total_Premium", v_crTotalPremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Policy_ID", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Is_PrePayment", gPMFunctions.ToSafeInteger(v_vPrePayment), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Cover_Start_Date", v_dtCoverStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Policy_Issue_Date", v_dtPolicyIssueDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELCDsForPolicySQL, sSQLName:=ACSELCDsForPolicyName, bStoredProcedure:=True, vResultArray:=r_vCashDepositDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELCDsForPolicySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetCDPaymentHistoryForPolicy(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vCashDepositDetails(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetCDPaymentHistoryForPolicy"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Party_Cnt", v_lPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_Folder_Cnt", v_lInsuranceFolderCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELCDPaymentHistoryForPolicySQL, sSQLName:=ACSELCDPaymentHistoryForPolicyName, bStoredProcedure:=True, vResultArray:=r_vCashDepositDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELCDPaymentHistoryForPolicySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetPolicyDetailsForCashDeposit(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vPolicyDetails(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyDetailsForCashDeposit"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Policy_ID", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELPolicyDetailsForCashDepositSQL, sSQLName:=ACSELPolicyDetailsForCashDepositName, bStoredProcedure:=True, vResultArray:=r_vPolicyDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELPolicyDetailsForCashDepositSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetCDReceiptsForAllocation(ByVal v_lCashDepositId As Integer, ByVal v_crTotalPremium As Decimal, ByVal v_vPrePayment As Object, ByVal v_dtCoverStartDate As Date, ByVal v_dtPolicyIssueDate As Date, ByRef r_vReceiptDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCDReceiptsForAllocation"
        Dim iValid As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Is_Valid", iValid, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Total_Premium", v_crTotalPremium, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Is_PrePayment", gPMFunctions.ToSafeInteger(v_vPrePayment), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Cover_Start_Date", v_dtCoverStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Policy_Issue_Date", v_dtPolicyIssueDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Retrieve_Reciepts", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELCDReceiptsForAllocationSQL, sSQLName:=ACSELCDReceiptsForAllocationName, bStoredProcedure:=True, vResultArray:=r_vReceiptDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELCDReceiptsForAllocationSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    'Start - Prakash - PN 65554. Code is not needed according to new functionality. It can be removed.
    'Public Function GetCDRecieptsForRefund( _
    ''                                ByVal v_lCashDepositId As Long, _
    ''                                ByVal v_crTotalPremium As Currency, _
    ''                                ByRef r_vReceiptDetails As Variant) As Long
    '
    '
    'Const kMethodName As String = "GetCDRecieptsForRefund"
    '
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    GetCDRecieptsForRefund = PMTrue
    '
    '    m_oDatabase.Parameters.Clear
    '
    '    Call AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, PMParamInput, PMLong)
    '    Call AddParameterLite(m_oDatabase, "Total_Premium", v_crTotalPremium, PMParamInput, PMDouble)
    '
    '    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSELCDReceiptsForRefundSQL, _
    ''                                      sSQLName:=ACSELCDReceiptsForRefundName, _
    ''                                      bStoredProcedure:=True, _
    ''                                      vResultArray:=r_vReceiptDetails)
    '
    '    If m_lReturn <> PMTrue Then
    '        RaiseError kMethodName, ACSELCDReceiptsForRefundSQL & " Failed", PMLogError
    '    End If
    '
    '    GoTo Finally
    '
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sUsername:=m_sUsername, _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=GetCDRecieptsForRefund
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    'End Function
    'End - Prakash - PN 65554

    'End - (Renuka) - Tech Spec -UIIC_WPR85_Cash_Deposit_Process-Part 2

    'Start - Renuka - Changes according to the WPR85 process sheet updation
    Public Function GetBalanceForCD(ByVal v_lCashDepositId As Integer, ByVal v_dtCoverStartDate As Date, ByVal v_dtPolicyIssueDate As Date, ByVal v_vPrePayment As Object, ByRef v_crAvaliableBalance As Decimal, ByRef v_crRunningBalance As Decimal) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetBalanceForCD"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Cover_Start_Date", v_dtCoverStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Policy_Issue_Date", v_dtPolicyIssueDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Is_PrePayment", gPMFunctions.ToSafeInteger(v_vPrePayment), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Available_Balance", v_crAvaliableBalance, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Running_Balance", v_crRunningBalance, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSELBalanceForCDSQL, sSQLName:=ACSELBalanceForCDName, bStoredProcedure:=True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                v_crAvaliableBalance = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("Available_Balance").Value)

                v_crRunningBalance = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("Running_Balance").Value)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSELBalanceForCDSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    'End - Renuka - Changes according to the WPR85 process sheet updation

    ' Start - Sankar - Changes according to the WPR85 process sheet updation
    Public Function CheckCDUsedForMultiPolicies(ByVal v_lCashDepositId As Integer, ByRef r_bIsRepeated As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckCDUsedForMultiPolicies"

        Dim iIsRepeated As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "CashDeposit_ID", v_lCashDepositId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "IsRepeated", iIsRepeated, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckCDUsedForMultiPolicySQL, sSQLName:=ACCheckCDUsedForMultiPolicyName, bStoredProcedure:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACCheckCDUsedForMultiPolicySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else

                iIsRepeated = m_oDatabase.Parameters.Item("IsRepeated").Value
                r_bIsRepeated = iIsRepeated = 1
            End If

            Return result

        Catch ex As Exception



            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    ' End - Sankar - Changes according to the WPR85 process sheet updation

    'Start - Prakash - PN 65557
    Public Function ConvertPolicyAmountToBaseCurrency(ByVal v_lInsuranceFileCnt As Integer, ByVal v_crPolicyAmount As Decimal, ByRef v_crBaseAmount As Decimal, ByVal v_lBaseCurrencyID As Integer, ByVal v_sBaseCurrencyCode As String, ByVal v_lTransactionCurrencyID As Integer, ByVal v_sTransactionCurrencyCode As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ConvertPolicyAmountToBaseCurrency"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Policy_Amount", v_crPolicyAmount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Base_Amount", v_crBaseAmount, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Base_Currency_ID", v_lBaseCurrencyID, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Base_Currency_Code", v_sBaseCurrencyCode, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Transaction_Currency_ID", v_lTransactionCurrencyID, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Transaction_Currency_Code", v_sTransactionCurrencyCode, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACConvertPolicyAmountToBaseCurrencySQL, sSQLName:=ACConvertPolicyAmountToBaseCurrencyName, bStoredProcedure:=True)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                v_crBaseAmount = gPMFunctions.ToSafeCurrency(m_oDatabase.Parameters.Item("Base_Amount").Value)

                v_lBaseCurrencyID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("Base_Currency_ID").Value)

                v_sBaseCurrencyCode = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("Base_Currency_Code").Value)

                v_lTransactionCurrencyID = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("Transaction_Currency_ID").Value)

                v_sTransactionCurrencyCode = gPMFunctions.ToSafeString(m_oDatabase.Parameters.Item("Transaction_Currency_Code").Value)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACConvertPolicyAmountToBaseCurrencySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    'End - Prakash -PN 65557
End Class
