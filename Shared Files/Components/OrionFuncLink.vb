Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Public Module OrionFuncLink
	' ******************************************************************************
	' Title: OrionFuncLink
	'
	' Description: Contains functions to use the OrionLink object.
	'
	' Edit History: CF 160699 - Created
	'
	' ******************************************************************************
	
	
	' Return value
	Private m_lReturn As Integer
	
	' Constants
	Public Const ACIGotoAccounts As Integer = 1
	Public Const ACIGoToTransactionDebit As Integer = 2
	Public Const ACIGoToTransactionCredit As Integer = 3
	Public Const ACIGotoTransactionCash As Integer = 4
	Public Const ACIGotoTransactionFee As Integer = 5
	
	'
	' This class
	Private Const ACClass As String = "OrionLinkFunc"
	
	' Instance of OrionLink
	Private m_oOrionLink As Object
	'EK 22/9/99
	Private m_oTransactions As Object
	
	Private m_oFees As Object
	
	' Instance of object manager
    Private m_oObjectManager As Object
	
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
            m_oObjectManager = CreateLateBoundObject("bObjectManager.ObjectManager")
			
			m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
			
			'    ' Get instance of OrionLink object
			'    m_lReturn& = m_oObjectManager.GetInstance( _
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
			'    m_lReturn& = m_oObjectManager.GetInstance( _
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
			If Not (m_oObjectManager Is Nothing) Then
                m_oObjectManager.Dispose()
                m_oObjectManager = Nothing
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
	' ***************************************************************** '
	Public Function ProcessOrionFunc(ByVal v_iButton As Integer, Optional ByVal v_sShortName As String = "", Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			Select Case (v_iButton)
				' Goto Account
				Case ACIGotoAccounts
					m_lReturn = ShowOutstandingTransactions(v_sShortName:=v_sShortName)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Log Error Message
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show outstanding transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
						Return result
					End If
					
					'EK 22/09/99
					' Debit Transactions
				Case ACIGoToTransactionDebit
					m_lReturn = ProcessTransactionDebit(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Log Error Message
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform a debit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					End If
					
					' Credit Transactions
				Case ACIGoToTransactionCredit
					m_lReturn = ProcessTransactionCredit(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Log Error Message
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform a credit transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					End If
					
					' Cash Transactions
				Case ACIGotoTransactionCash
					m_lReturn = ProcessTransactionCash(v_sShortName:=v_sShortName)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Log Error Message
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform a manual journal.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					End If
					
					' Fee Transactions
				Case ACIGotoTransactionFee
					m_lReturn = ProcessTransactionFee(v_lPartyCnt:=v_lPartyCnt)
					If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
						result = gPMConstants.PMEReturnCode.PMFalse
						' Log Error Message
						iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to perform raising a fee.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					End If
					
					' Error
				Case Else
					result = gPMConstants.PMEReturnCode.PMFalse
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid button/menu chosen. Possibly not implemented yet.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOrionFunc", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					Return result
					
			End Select
			
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
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (ShowTransactions) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function ShowTransactions(ByVal v_sShortName As String) As Integer
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
				'm_lReturn = m_oObjectManager.GetInstance(temp_m_oOrionLink, sClassName:="iPMBOrionLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				'm_oOrionLink = temp_m_oOrionLink
				'
				'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					'result = gPMConstants.PMEReturnCode.PMFalse
					' Log Error Message
					'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBOrionLink.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					'Return result


    '

    'm_lReturn = m_oOrionLink.ShowTransactions(v_sShortName:=v_sShortName)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

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


	
	' ***************************************************************** '
	' Name: ShowOutstandingTransactions
	'
	' Description: Shows outstanding transactions for the passed short code.
	'
	' ***************************************************************** '
	Private Function ShowOutstandingTransactions(ByVal v_sShortName As String) As Integer
		
		Dim result As Integer = 0
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Show transactions
			' Get instance of OrionLink object
			If m_oOrionLink Is Nothing Then
				Dim temp_m_oOrionLink As Object
				m_lReturn = m_oObjectManager.GetInstance(temp_m_oOrionLink, sClassName:="iPMBOrionLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oOrionLink = temp_m_oOrionLink
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					result = gPMConstants.PMEReturnCode.PMFalse
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iPMBOrionLink.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowOutstandingTransactions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					Return result
				End If
			End If
			

			m_lReturn = m_oOrionLink.ShowHistory(v_sShortName:=v_sShortName)
			
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
                Dim temp_m_oOrionLink As Object = Nothing
				m_lReturn = m_oObjectManager.GetInstance(temp_m_oOrionLink, sClassName:="iPMBOrionLink.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
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
		
		 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Do Debit Transaction
			If m_oFees Is Nothing Then
				' Get instance of Transaction Object
                Dim temp_m_oFees As Object = Nothing
				m_lReturn = m_oObjectManager.GetInstance(temp_m_oFees, sClassName:="iPMBFeeTransaction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
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
			

			m_lReturn = m_oFees.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
			

			m_lReturn = m_oFees.Start()
			

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
                Dim temp_m_oTransactions As Object = Nothing
				m_lReturn = m_oObjectManager.GetInstance(temp_m_oTransactions, sClassName:="iPMBTransactions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
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

			m_lReturn = CType(m_oTransactions, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			

			m_lReturn = m_oTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
			

			m_lReturn = m_oTransactions.Start()
			

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
                Dim temp_m_oTransactions As Object = Nothing
				m_lReturn = m_oObjectManager.GetInstance(temp_m_oTransactions, sClassName:="iPMBTransactions.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
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

			m_lReturn = CType(m_oTransactions, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			

			m_lReturn = m_oTransactions.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)
			

			m_lReturn = m_oTransactions.Start()
			

        m_oTransactions.Dispose()
			
        Return result
		
	End Function
End Module

