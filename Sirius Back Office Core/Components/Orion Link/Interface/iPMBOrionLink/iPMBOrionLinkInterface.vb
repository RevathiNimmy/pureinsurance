Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    ' Name of this class
    Private Const ACClass As String = "Interface"

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Instance of the business

    Private m_oBusiness As bPMBOrionLink.Business

    ' Navigator roadmap
    'Private Const ACACTNavManualJournal As String = "PMBPCP"
    Private Const ACACTNavManualJournal As String = "CLICASHP1"

    ' Bank account name
    'Private Const ACACTBankAccount As String = "CLIENTBANK"

    ' Object Manager
    Private g_oObjectManager As bObjectManager.ObjectManager

    ' Navigator starter
    Private WithEvents m_oNavStart As iPMNavStart.Interface_Renamed

    Private m_bCalledViaClientManager As Boolean


    Public Property CalledViaClientManager() As Boolean
        Get
            Return m_bCalledViaClientManager
        End Get
        Set(ByVal Value As Boolean)
            m_bCalledViaClientManager = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description: Standard initialise function
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new instance of object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Initialise object manager
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get an instance of the business object
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMBOrionLink.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck PN6169
            With g_oObjectManager
                g_iSourceID = .SourceID
            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description: Standard terminate function.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oNavStart IsNot Nothing Then
                    m_oNavStart.Dispose()
                    m_oNavStart = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                End If
                m_oBusiness = Nothing
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                End If
                g_oObjectManager = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ********************************************************************** '
    ' Name: Start
    '
    ' Description: Standard entry point for programs calling this component.
    '
    ' ********************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountID
    '
    ' Description: Calls the business to get an account id.
    '
    ' ***************************************************************** '
    Private Function GetAccountID(ByVal v_sShortCode As String, ByRef r_lAccountID As Integer, ByRef r_iCompanyID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set the mouse pointer to busy
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'eck130600
        ' Try and get an account id

        m_lReturn = m_oBusiness.GetAccountID(v_sShortCode:=v_sShortCode, r_lAccountID:=r_lAccountID, r_iCompanyID:=r_iCompanyID)

        ' Reset the mouse pointer
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        If m_lReturn = gPMConstants.PMEReturnCode.PMMNoAccess Then
            Return gPMConstants.PMEReturnCode.PMMNoAccess
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowTransDetails
    '
    ' Description: Shows the find transdetails object, with or without
    '              outstanding checked.
    '   eck220800 Pass optional insurance ref
    ' Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc - 5.2.1.2
    ' ***************************************************************** '
    Private Function ShowTransDetails(ByVal v_sShortName As String, ByVal v_bOutstandingOnly As Boolean, Optional ByVal v_sInsuranceRef As String = Nothing, Optional ByVal v_bInsuredAccountView As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim oFindTransaction As iACTFindTransaction.Interface_Renamed
        Dim iCompanyID As Integer
        'eck PN6169
        Dim vMulticompany As Object = Nothing
        Dim vBranchLogon As Object = Nothing
        Dim v_sAgentShortName As String = String.Empty



        result = gPMConstants.PMEReturnCode.PMTrue

        'TN20001119 (Start)
        'is it underwriting

        m_lReturn = m_oBusiness.GetAgentShortName(v_sClientShortName:=v_sShortName, r_sAgentShortName:=v_sAgentShortName)


        'Developer Guide No. 98
        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=1, r_vUnderwriting:=vMulticompany)

        'Developer Guide No. 98
        iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, v_vBranch:=1, r_vUnderwriting:=vBranchLogon)

        If gPMFunctions.NullToString(vBranchLogon) = "1" And gPMFunctions.NullToString(vMulticompany) = "1" Then
            iCompanyID = g_iSourceID
        End If

        ' Get the account id
        m_lReturn = CType(GetAccountID(v_sShortCode:=v_sShortName, r_lAccountID:=lAccountID, r_iCompanyID:=iCompanyID), gPMConstants.PMEReturnCode)
        'eck130600
        If m_lReturn = gPMConstants.PMEReturnCode.PMMNoAccess Then
            MessageBox.Show("Sorry, you do not have access to this Account", Application.ProductName)
            Return result
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Dim temp_oFindTransaction As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_oFindTransaction, sClassName:="iACTFindTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
        oFindTransaction = temp_oFindTransaction
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iACTFindTransaction.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End If

        ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.2.1.2)
        If v_bInsuredAccountView Then


            oFindTransaction.InsuredAccountID = lAccountID

            oFindTransaction.InsuredAccountView = True

        Else

            oFindTransaction.AccountID = lAccountID
        End If
        ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.2.1.2)

        'to set the value if its called from client manager

        oFindTransaction.CalledViaClientManager = m_bCalledViaClientManager

        ' Show outstandingonly or not

        oFindTransaction.OutstandingOnly = v_bOutstandingOnly
        'eck220800 Insurance ref
        If Not String.IsNullOrEmpty(v_sInsuranceRef) Then

            oFindTransaction.InsuranceRef = v_sInsuranceRef
        End If
        'eck130600

        oFindTransaction.CompanyID = iCompanyID

        ' AAB - 01-Aug-2003 - Added this so we can fix an issue related to PN 3169

        oFindTransaction.CallingAppName = ACApp

        ' Start it up

        m_lReturn = oFindTransaction.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start iACTFindTransaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Terminate

        oFindTransaction.Dispose()

        oFindTransaction = Nothing

        Return result

    End Function

    ' ******************************************************************** '
    ' Name: ShowTransactions
    '
    ' Description: Shows all transactions for the account of shortname.
    ' eck220800 Pass optional Insurance File Ref
    ' ******************************************************************** '
    Public Function ShowTransactions(ByVal v_sShortName As String, Optional ByRef v_sInsuranceRef As String = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'eck220800 Pass optional InsuranceRef
            If Not String.IsNullOrEmpty(v_sInsuranceRef) Then
                m_lReturn = CType(ShowTransDetails(v_sShortName:=v_sShortName, v_bOutstandingOnly:=False), gPMConstants.PMEReturnCode)

            Else
                m_lReturn = CType(ShowTransDetails(v_sShortName:=v_sShortName, v_bOutstandingOnly:=False, v_sInsuranceRef:=v_sInsuranceRef), gPMConstants.PMEReturnCode)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show transactions for account : " & v_sShortName, vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************* '
    ' Name: ShowHistory
    '
    ' Description: As above, but for outstanding transactions only.
    ' eck220800 add optiona insurance ref
    ' Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc - (5.2.1.1)
    ' ********************************************************************* '
    Public Function ShowHistory(ByVal v_sShortName As String, Optional ByVal v_sInsuranceRef As String = Nothing, Optional ByVal v_bInsuredAccountView As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'eck220800
            If String.IsNullOrEmpty(v_sInsuranceRef) Then
                ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.2.1.1)
                m_lReturn = CType(ShowTransDetails(v_sShortName:=v_sShortName, v_bOutstandingOnly:=False, v_bInsuredAccountView:=v_bInsuredAccountView), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(ShowTransDetails(v_sShortName:=v_sShortName, v_bOutstandingOnly:=True, v_sInsuranceRef:=v_sInsuranceRef, v_bInsuredAccountView:=v_bInsuredAccountView), gPMConstants.PMEReturnCode)
                ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.2.1.1)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show outstanding transactions for account : " & v_sShortName, vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHistory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowHistory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowHistory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************* '
    ' Name: TransactionCash
    '
    ' Description: Start up the manual journal road map.
    '
    ' ********************************************************************* '
    Public Function TransactionCash(ByVal v_sShortName As String) As Integer

        Dim result As Integer = 0
        Dim lAccountID As Integer
        Dim iCompanyID As Integer
        Dim sProcess As String = ""
        Dim vKeyArray(1, 1) As Object 'MKW110703 PN5344 Resized Array to allow extra key.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the ID for the account
            m_lReturn = CType(GetAccountID(v_sShortCode:=v_sShortName, r_lAccountID:=lAccountID, r_iCompanyID:=iCompanyID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get account_id for '" & v_sShortName & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactionCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Get the account_id for the bank
            '    m_lReturn& = GetAccountID(v_sShortCode:=ACACTBankAccount, _
            ''                              r_lAccountID:=lBankID&)
            '    If (m_lReturn& <> PMTrue) Then
            '        TransactionCash = PMFalse
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to get account_id for '" & CStr(ACACTBankAccount) & "'", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="TransactionCash", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            ' Set the process we want to start.
            sProcess = ACACTNavManualJournal

            ' Set the keys
            ' Account ID

            vKeyArray(1, 0) = lAccountID

            vKeyArray(0, 0) = PMNavKeyConst.ACTKeyNameAccountID

            'MKW110703 PN5344 Added debit_credit key to create roadmap.

            vKeyArray(1, 1) = ""

            vKeyArray(0, 1) = PMNavKeyConst.ACTKeyNameDebitCredit

            ' CTAF 020200 - Removed bank account as this CANNOT be hard coded.
            ' Bank Account ID
            'vKeyArray(1, 1) = lBankID&
            ' Needs to be replaced with the proper constant
            'vKeyArray(0, 1) = "bank_account_id"

            ' Start the navigator process
            m_lReturn = CType(StartNavMap(v_sProcessCode:=sProcess, v_vKeyArray:=vKeyArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start navigator map : " & sProcess, vApp:=ACApp, vClass:=ACClass, vMethod:="TransactionCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactionCash Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactionCash", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************* '
    ' Name: TransactionCredit
    '
    ' Description:
    '
    ' ********************************************************************* '
    Public Function TransactionCredit(ByVal v_sShortName As String) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactionCredit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactionCredit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ********************************************************************* '
    ' Name: TransactionDebit
    '
    ' Description:
    '
    ' ********************************************************************* '
    Public Function TransactionDebit(ByVal v_sShortName As String) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactionDebit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactionDebit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartNavMap
    '
    ' Description: Starts the navigator process
    '
    ' ***************************************************************** '
    Private Function StartNavMap(ByVal v_sProcessCode As String, Optional ByVal v_vKeyArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create a new instance of navigator
        m_oNavStart = New iPMNavStart.Interface_Renamed()

        m_lReturn = CType(m_oNavStart, SSP.S4I.Interfaces.ILocalInterface).Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise iPMNavStart object.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' Set the navigator keys

        If Not Information.IsNothing(v_vKeyArray) Then
            m_lReturn = m_oNavStart.SetKeys(vKeyArray:=v_vKeyArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
        End If

        ' Set its properties
        m_oNavStart.CallingAppName = ACApp

        ' Set the process to start
        m_oNavStart.ProcessCode = v_sProcessCode

        ' Start it
        m_lReturn = m_oNavStart.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start oNavStart", vApp:=ACApp, vClass:=ACClass, vMethod:="StartNavMap", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        Return result

    End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

