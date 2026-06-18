Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
'Module OrionLinkFunc
Partial NotInheritable Class MainModule 
    ' ******************************************************************************
    ' Title: OrionFuncLink
    '
    ' Description: Contains functions to use the OrionLink object.
    '
    ' Edit History: CF 160699 - Created
    '
    ' ******************************************************************************


    ' Return value
    'Private m_lReturn As gPMConstants.PMEReturnCode

    ' Constants
    Public Const ACIGotoAccounts As Integer = 1
    Public Const ACIGoToTransactionDebit As Integer = 2
    Public Const ACIGoToTransactionCredit As Integer = 3
    Public Const ACIGotoTransactionCash As Integer = 4
    Public Const ACIGotoTransactionFee As Integer = 5
    'eck080800
    Public Const ACIGoToTransactionAJ As Integer = 6
    Public Const ACIGoToTransactionAJReversal As Integer = 7
    'Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.3)
    Public Const ACIGotoInsuredAccounts As Integer = 8
    'End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.3)

    '
    ' This class
    'Private Const ACClass As String = "OrionLinkFunc"

    ' Instance of OrionLink
    Private m_oOrionLink As Object
    'EK 22/9/99
    Private m_oTransactions As Object
    'eck240800


    Private m_oAJTransactions As Object


    Private m_oFees As Object

    ' Instance of object manager
    'Private g_oObjectManager As bObjectManager.ObjectManager

    ' ***************************************************************** '
    ' Name: InitialiseOrionLinkFunc
    '
    ' Description: Initialises
    '
    ' ***************************************************************** '
    Public Function InitialiseOrionLinkFunc() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise object manager
            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            '    ' Get instance of OrionLink object
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''                    oObject:=m_oOrionLink, _
            ''                    sClassName:="iPMBOrionLink.Interface", _
            ''                    vInstanceManager:=PMGetLocalInterface)
            '    If (m_lReturn& <> PMTrue) Then
            '        InitialiseOrionLinkFunc = PMFalse
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to get instance of iPMBOrionLink.Interface", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="InitialiseOrionLinkFunc", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            'EK 22/9/99
            '    ' Get instance of Transaction Object
            '    m_lReturn& = g_oObjectManager.GetInstance( _
            ''                    oObject:=m_oTransactions, _
            ''                    sClassName:="iPMBTransactions.Interface", _
            ''                    vInstanceManager:=PMGetLocalInterface)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        InitialiseOrionLinkFunc = PMFalse
            '        ' Log Error Message
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to get instance of iPMBTransactions.Interface", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="InitialiseOrionLinkFunc", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseOrionLinkFunc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseOrionLinkFunc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: TerminateOrionLinkFunc
    '
    ' Description: Terminate
    '
    ' ***************************************************************** '
    Public Function TerminateOrionLinkFunc() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Terminate orion link
            If Not (m_oOrionLink Is Nothing) Then

                m_oOrionLink.Dispose()
                m_oOrionLink = Nothing
            End If

            ' Terminate transactions
            If Not (m_oTransactions Is Nothing) Then

		m_oTransactions.Dispose()
                m_oTransactions = Nothing
            End If

            ' Terminate fees
            If Not (m_oFees Is Nothing) Then

		m_oFees.Dispose()
                m_oFees = Nothing
            End If

            ' Terminate object manager
            If Not (g_oObjectManager Is Nothing) Then
		g_oObjectManager.Dispose()
                g_oObjectManager = Nothing
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TerminateOrionLinkFunc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateOrionLinkFunc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ProcessOrionFunc
    '
    ' Description: Handles calls to orion functionality.
    ' 'EK 22/9/99 Modified to Call Debits and Credits
    '   eck220800 Pass Optional Insurance ref
    ' ***************************************************************** '
    Public Function ProcessOrionFunc(ByVal v_iButton As Integer, Optional ByVal v_sShortName As String = "", Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sInsuranceRef As String = Nothing, Optional ByVal v_bCalledViaClientManager As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessOrionFunc"

        Try

            Dim bBrokerlink As Boolean
            Dim vAgency As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.getUnderwritingOrAgency(vAgency)



            Select Case (v_iButton)
                ' Goto Account
                Case ACIGotoAccounts
                    'eck220800 optional insuranceRef
                    m_lReturn = CType(ShowOutstandingTransactions(v_sShortName:=v_sShortName, v_sInsuranceRef:=v_sInsuranceRef, v_bCalledViaClientManager:=v_bCalledViaClientManager), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show outstanding transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                    ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.3.1)
                Case ACIGotoInsuredAccounts

                    m_lReturn = CType(ShowOutstandingTransactions(v_sShortName:=v_sShortName, v_sInsuranceRef:=v_sInsuranceRef, v_bInsuredAccountView:=True), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        ' DO Not Call any functions before here or the error will be lost
                        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
                        Return result

                    End If
                    ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.3.1)

                    'EK 22/09/99
                    ' Debit Transactions
                Case ACIGoToTransactionDebit
                    If v_lInsuranceFileCnt <> 0 Then
                        m_lReturn = CType(ProcessTransactionDebit(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform a debit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                    Else
                        result = gPMConstants.PMEReturnCode.PMCancel
                    End If

                    ' Credit Transactions
                Case ACIGoToTransactionCredit
                    If v_lInsuranceFileCnt <> 0 Then
                        m_lReturn = CType(ProcessTransactionCredit(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform a credit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                    Else
                        result = gPMConstants.PMEReturnCode.PMCancel
                    End If

                    ' Cash Transactions
                Case ACIGotoTransactionCash
                    m_lReturn = CType(ProcessTransactionCash(v_sShortName:=v_sShortName), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform a manual journal.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    ' Fee Transactions
                Case ACIGotoTransactionFee
                    m_lReturn = CType(ProcessTransactionFee(v_lPartyCnt:=v_lPartyCnt), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform raising a fee.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                    'eck080800
                    ' Manual AJ
                Case ACIGoToTransactionAJ
                    If v_lInsuranceFileCnt <> 0 Then
                        m_lReturn = CType(ProcessTransactionAJ(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform an AJ.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                    Else
                        result = gPMConstants.PMEReturnCode.PMCancel
                    End If

                    ' Credit Manual AJ Reversal
                Case ACIGoToTransactionAJReversal
                    If v_lInsuranceFileCnt <> 0 Then
                        m_lReturn = CType(ProcessTransactionAJReversal(v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform an AJ reversal.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                    Else
                        result = gPMConstants.PMEReturnCode.PMCancel
                    End If

                    ' Error
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid button/menu chosen. Possibly not implemented yet.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result

            End Select

            m_lReturn = CType(UpdatePartyScreen(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessOrionFunc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ShowTransactions
    '
    ' Description: Shows all transactions for the passed short code.
    ' eck 220800 Optional Insurance Ref
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ShowTransactions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ShowTransactions(ByVal v_sShortName As String, Optional ByVal v_sInsuranceRef As String = "") As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Show transactions
    ' Get instance of OrionLink object
    'If m_oOrionLink Is Nothing Then
    'Dim temp_m_oOrionLink As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_m_oOrionLink, sClassName:="iPMBOrionLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'm_oOrionLink = temp_m_oOrionLink
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBOrionLink.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return result
    'End If
    'End If
    'eck220800 optional insurance ref

    'm_lReturn = m_oOrionLink.ShowTransactions(v_sShortName:=v_sShortName, v_sInsuranceFef:=v_sInsuranceRef)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ShowOutstandingTransactions
    '
    ' Description: Shows outstanding transactions for the passed short code.
    ' 'eck220800 Add optional insurance ref
    ' Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc - 5.1.3.2
    ' ***************************************************************** '
    Private Function ShowOutstandingTransactions(ByVal v_sShortName As String, Optional ByVal v_sInsuranceRef As Object = Nothing, Optional ByVal v_bCalledViaClientManager As Boolean = False, Optional ByVal v_bInsuredAccountView As Boolean = False) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show transactions
            ' Get instance of OrionLink object
            If m_oOrionLink Is Nothing Then
                Dim temp_m_oOrionLink As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oOrionLink, sClassName:="iPMBOrionLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oOrionLink = temp_m_oOrionLink

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBOrionLink.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowOutstandingTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_oOrionLink.CalledViaClientManager = v_bCalledViaClientManager

            'eck220800
            ' Start - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.3.2)

            m_lReturn = m_oOrionLink.ShowHistory(v_sShortName:=v_sShortName, v_sInsuranceRef:=v_sInsuranceRef, v_bInsuredAccountView:=v_bInsuredAccountView)
            ' End - (Sankar) - (Tech Spec - QBENZCR001 - Client Manager view Account Transactions.doc) - (5.1.3.2)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show outstanding transactions.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessTransactionCash
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionCash(ByVal v_sShortName As String) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Show transactions
            ' Get instance of OrionLink object
            If m_oOrionLink Is Nothing Then
                Dim temp_m_oOrionLink As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oOrionLink, sClassName:="iPMBOrionLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oOrionLink = temp_m_oOrionLink

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBOrionLink.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionCash", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_lReturn = m_oOrionLink.TransactionCash(v_sShortName:=v_sShortName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show outstanding transactions.", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessTransactionFee
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionFee(ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oOrionLink As Object

        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do Debit Transaction
            If m_oFees Is Nothing Then
                ' Get instance of Transaction Object
                Dim temp_m_oFees As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oFees, sClassName:="iPMBFeeTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oFees = temp_m_oFees

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBTransactions.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionFee", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_oFees.PartyCnt = v_lPartyCnt


            m_lReturn = CType(m_oFees, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise fee transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionFee", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'SD 01/08/2002 Scalability changes

            m_lReturn = m_oFees.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set process modes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionFee", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = m_oFees.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start fee transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionFee", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


		m_oFees.Dispose()

            Return result

    End Function

    'EK 22/9/99
    ' ***************************************************************** '
    ' Name: ProcessTransactionDebit
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionDebit(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do Debit Transaction
            If m_oTransactions Is Nothing Then
                ' Get instance of Transaction Object
                Dim temp_m_oTransactions As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oTransactions, sClassName:="iPMBTransactions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oTransactions = temp_m_oTransactions

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBTransactions.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionDebit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_oTransactions.InsuranceFileCnt = v_lInsuranceFileCnt


            m_oTransactions.FromEvent = False

            m_oTransactions.DebitCredit = "D"
            'AR20050201 - PN18214

            m_oTransactions.FromClientManager = True


            m_lReturn = CType(m_oTransactions, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise debit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionDebit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'SD 01/08/2002 Scalability changes

            m_lReturn = m_oTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set process modes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionDebit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = m_oTransactions.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start debit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionDebit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


		m_oTransactions.Dispose()

            Return result

    End Function
    'EK 22/9/99
    ' ***************************************************************** '
    ' Name: ProcessTransactionCredit
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionCredit(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do Credit Transaction
            If m_oTransactions Is Nothing Then
                ' Get instance of Transaction Object
                Dim temp_m_oTransactions As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oTransactions, sClassName:="iPMBTransactions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oTransactions = temp_m_oTransactions

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBTransactions.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionCredit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_oTransactions.InsuranceFileCnt = v_lInsuranceFileCnt


            m_oTransactions.FromEvent = False

            m_oTransactions.DebitCredit = "C"
            'AR20050201 - PN18214

            m_oTransactions.FromClientManager = True


            m_lReturn = CType(m_oTransactions, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise Credit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionCredit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'SD 01/08/2002 Scalability changes

            m_lReturn = m_oTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set Process Modes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionCredit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = m_oTransactions.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Credit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionCredit", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


		m_oTransactions.Dispose()

            Return result

    End Function
    'eck080800
    ' ***************************************************************** '
    ' Name: ProcessTransactionAJ
    '
    ' Description:
    '
    'eck240800 use different object name for Aj transaction
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionAJ(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do AJ Transaction
            If m_oAJTransactions Is Nothing Then
                ' Get instance of Transaction Object
                Dim temp_m_oAJTransactions As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAJTransactions, sClassName:="iPMBDDTrans.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAJTransactions = temp_m_oAJTransactions

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBDDTrans.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJ", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_oAJTransactions.InsuranceFileCnt = v_lInsuranceFileCnt


            m_oAJTransactions.FromEvent = False

            m_oAJTransactions.DIDDIC = "D"
            m_lReturn = CType(CType(m_oAJTransactions, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise AJ transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJ", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'SD 01/08/2002 Scalability changes

            m_lReturn = m_oAJTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set process modes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJ", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = m_oAJTransactions.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start AJ transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJ", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


		m_oAJTransactions.Dispose()

            Return result

    End Function
    ' ***************************************************************** '
    ' Name: ProcessTransactionAJReversal
    '
    ' Description:
    '
    'eck240800 use different object name for Aj transaction
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionAJReversal(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do AJReversal Transaction
            If m_oAJTransactions Is Nothing Then
                ' Get instance of Transaction Object
                Dim temp_m_oAJTransactions As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAJTransactions, sClassName:="iPMBDDTrans.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAJTransactions = temp_m_oAJTransactions

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBDDTrans.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJReversal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If
            End If


            m_oAJTransactions.InsuranceFileCnt = v_lInsuranceFileCnt


            m_oAJTransactions.FromEvent = False

            m_oAJTransactions.DIDDIC = "C"
            m_lReturn = CType(CType(m_oAJTransactions, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise AJReversal transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJReversal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'SD 01/08/2002 Scalability changes

            m_lReturn = m_oAJTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set process modes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJReversal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            m_lReturn = m_oAJTransactions.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start AJReversal transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionAJReversal", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


		m_oAJTransactions.Dispose()

            Return result

    End Function
End Class

