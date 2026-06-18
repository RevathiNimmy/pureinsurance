Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide no. 129
Imports SharedFiles
Friend NotInheritable Class CLMRecovery 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name:  CLMRecovery
	'
	' Date:        24/08/2000
	'
	' Description: Describes the CLMRecovery attributes.
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
Private Const ACClass As String = "CLMRecovery" 
	
	' Record flags
	Public IsNew As Boolean
	Public IsDeleted As Boolean
	Public IsDirty As Boolean
	'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
	Private m_lRecoveryPartyTypeId As Integer
	Private m_lRecoveryPartyCnt As Integer
	Private m_sRecoveryParty As String = ""
	Private m_sRecoveryPartyDesc As String = ""
	'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
	' Recovery properties
	Public ClaimID As Integer
	Public PerilID As Integer
	Public ReceiptID As Integer
	
	Public UniqueId As String = "" ' Don't assume recovery_id as we won't have one for new records
	Public RecoveryId As Integer
	Public RecoveryTypeID As Integer
	Public RecoveryType As String = ""
	Public RevisionCount As Integer
	
	Public LossCurrencyID As String = ""
	Public LossCurrency As String = ""
	Public ReceiptCurrencyID As Integer
	Public ReceiptCurrency As String = ""
	Public CurrencyRate As Double
	
	Public InitialReserve As Decimal
	Public RevisedReserve As Decimal
	Public ThisReserve As Decimal ' Changes to reserve are ALWAYS in loss currency
	
	Public ReceivedToDate As Decimal
	
	Public TaxTypeID As Integer
	Public TaxType As String = ""
	Public TaxTypeCode As String = ""
	Public TaxBandID As Integer
	Public TaxBand As String = ""
	Public TaxAmount As Decimal
	Public TaxToDate As Decimal
	Public IsPostTaxes As Boolean
	
	' co/reinsurance arrays
	Public Coinsurance( ,  ) As Object
	Public Reinsurance( ,  ) As Object
	
	
	' ************************************************
	' Added to replace global variables 19/12/2003
	Private m_sUsername As String = ""
	
	Private m_sPassword As String = ""
	
	Private m_iUserID As Integer
	
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	' ************************************************
	
	' Database class
	Private m_oDatabase As dPMDAO.Database
	
	' This receipt amount, declared private so we can trigger recalculation in the Public Let
	Private m_cThisReceipt As Decimal
	
	
	' ***************************************************************** '
	'                         PUBLIC PROPERTIES
	' ***************************************************************** '
	
	Public ReadOnly Property Balance() As Decimal
		Get
			' Calculate the total balance
			Return TotalReserve - TotalReceipt
		End Get
	End Property
	
	Public ReadOnly Property BalanceToDate() As Decimal
		Get
			' Calculate the balance to today
			Return (InitialReserve + RevisedReserve) - ReceivedToDate
		End Get
	End Property
	
	Public ReadOnly Property NetReceipt() As Decimal
		Get
			' Calculate receipt total
			Return ThisReceipt - TaxAmount
		End Get
	End Property
	
	Public ReadOnly Property NetReceiptLoss() As Decimal
		Get
			' Calculate receipt total
			Return ThisReceiptLoss - TaxAmountLoss
		End Get
	End Property
	
	Public ReadOnly Property TaxAmountLoss() As Decimal
		Get
			' The tax amount in loss currency
			Return TaxAmount * CurrencyRate
		End Get
	End Property
	
	Public Property ThisReceipt() As Decimal
		Get
			Return m_cThisReceipt
		End Get
		Set(ByVal Value As Decimal)
			m_cThisReceipt = Value
		End Set
	End Property
	
	Public ReadOnly Property ThisReceiptLoss() As Decimal
		Get
			' The receipt in loss currency
			Return ThisReceipt * CurrencyRate
		End Get
	End Property
	
	Public ReadOnly Property TotalReceipt() As Decimal
		Get
			' Calculate to receipt
			Return ReceivedToDate + ThisReceiptLoss
		End Get
	End Property
	
	Public ReadOnly Property TotalReserve() As Decimal
		Get
			' Calculate the total reserve
			Return InitialReserve + RevisedReserve + ThisReserve
		End Get
	End Property
	
	Public ReadOnly Property TotalTax() As Decimal
		Get
			' Calculate the total tax
			Return TaxToDate + TaxAmountLoss
		End Get
	End Property
	
	'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	Public Property lRecoveryPartyTypeId() As Integer
		Get
			Return m_lRecoveryPartyTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryPartyTypeId = Value
		End Set
	End Property
	
	
	Public Property lRecoveryPartyCnt() As Integer
		Get
			Return m_lRecoveryPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryPartyCnt = Value
		End Set
	End Property
	
	Public Property sRecoveryParty() As String
		Get
			Return m_sRecoveryParty
		End Get
		Set(ByVal Value As String)
			m_sRecoveryParty = Value
		End Set
	End Property
	
	Public Property sRecoveryPartyDesc() As String
		Get
			Return m_sRecoveryPartyDesc
		End Get
		Set(ByVal Value As String)
			m_sRecoveryPartyDesc = Value
		End Set
	End Property
	'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
	
	
	' ***************************************************************** '
	'                       PUBLIC FUNCTIONS
	' ***************************************************************** '
	
	Public Function Add() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Add"
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		' Add parameters
		bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", RecoveryId, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMLong, True)
		bPMAddParameter.AddParameterLite(m_oDatabase, "claim_peril_id", PerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_type_id", RecoveryTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		' Recoveries are always in loss currency
		bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", LossCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		bPMAddParameter.AddParameterLite(m_oDatabase, "initial_reserve", InitialReserve, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		' Increment with new reserve
		bPMAddParameter.AddParameterLite(m_oDatabase, "revised_reserve", RevisedReserve + ThisReserve, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		' Increment with new receipt (in loss currency)
		bPMAddParameter.AddParameterLite(m_oDatabase, "received_to_date", ReceivedToDate + ThisReceiptLoss, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		bPMAddParameter.AddParameterLite(m_oDatabase, "revision_count", RevisionCount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		' Increment with new tax (in loss currency)
		bPMAddParameter.AddParameterLite(m_oDatabase, "tax_amount", TaxToDate + TaxAmountLoss, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
		If m_lRecoveryPartyTypeId > 0 Then
			bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery_Party_Type_Id", m_lRecoveryPartyTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
			bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery_Party_cnt", m_lRecoveryPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		End If
		'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
		' Execute sql
		lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRecoverySQL, sSQLName:=ACAddRecoveryName, bStoredProcedure:=ACAddRecoveryStored)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to add recovery record")
		End If
		
		' Get recovery id
		RecoveryId = m_oDatabase.Parameters.Item("recovery_id").Value
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		Finally
'		Return result
		
		' This is for debugging only
'		Resume 
		
'		Return result
		End Try
		Return result
	End Function
	
	
	Public Function Delete() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Delete"
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Add parameters
		bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", RecoveryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
		
		' Execute sql
		lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRecoverySQL, sSQLName:=ACDeleteRecoveryName, bStoredProcedure:=ACDeleteRecoveryStored)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to delete recovery record")
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		Finally
'		Return result
		
		' This is for debugging only
'		Resume 
		
'		Return result
		End Try
		Return result
	End Function
	
	
	' Entry point for any initialisation code for this object.
	Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer
		
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
			m_oDatabase = vDatabase
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' Entry point for any termination code for this object.
 Private disposedValue As Boolean
	Public Sub Dispose() Implements IDisposable.Dispose
		Dispose(True)
		GC.SuppressFinalize(Me)
	End Sub


	Protected Sub Dispose(disposing As Boolean)
		If Not Me.disposedValue Then
			 If disposing Then
			End If
		End If
		Me.disposedValue = True
	End Sub

	
	
	Public Function Update() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Update"
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Add parameters
		bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_id", RecoveryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
		bPMAddParameter.AddParameterLite(m_oDatabase, "claim_peril_id", PerilID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		bPMAddParameter.AddParameterLite(m_oDatabase, "recovery_type_id", RecoveryTypeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		' Recoveries are always in loss currency
		bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", LossCurrencyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		bPMAddParameter.AddParameterLite(m_oDatabase, "initial_reserve", InitialReserve, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		' Increment with new reserve
		bPMAddParameter.AddParameterLite(m_oDatabase, "revised_reserve", RevisedReserve + ThisReserve, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		' Increment with new receipt (in loss currency)
		bPMAddParameter.AddParameterLite(m_oDatabase, "received_to_date", ReceivedToDate + ThisReceiptLoss, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		bPMAddParameter.AddParameterLite(m_oDatabase, "revision_count", RevisionCount, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
		' Increment with new tax (in loss currency)
		bPMAddParameter.AddParameterLite(m_oDatabase, "tax_amount", TaxToDate + TaxAmountLoss, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
		If m_lRecoveryPartyTypeId > 0 Then
			bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery_Party_Type_Id", m_lRecoveryPartyTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
			bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery_Party_cnt", m_lRecoveryPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
		End If
		'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
		' Execute sql
		lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRecoverySQL, sSQLName:=ACUpdateRecoveryName, bStoredProcedure:=ACUpdateRecoveryStored)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Failed to update recovery record")
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		Finally
'		Return result
		
		' This is for debugging only
'		Resume 
		
'		Return result
		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	'                       PRIVATE FUNCTIONS
	' ***************************************************************** '
	
	' Recalculate coinsurance and reinsurance splits
	Public Function RecalcCoReinsurance() As Integer
		
		Dim result As Integer = 0
		Dim cReinsuranceAmount, cReinsuranceTaxAmount, cTotalAmount, cTotalTaxAmount, cThisPayment, cThisTax As Decimal
		
		Const kMethodName As String = "RecalcCoReinsurance"
		Dim lReturn As Integer
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' If we are posting taxes reinsure them seperately, else rollup into receipt
		If IsPostTaxes Then
			cReinsuranceAmount = NetReceipt
			cTotalAmount = NetReceipt
			cReinsuranceTaxAmount = TaxAmount
			cTotalTaxAmount = TaxAmount
		Else
			cReinsuranceAmount = ThisReceipt
			cTotalAmount = ThisReceipt
			cReinsuranceTaxAmount = TaxAmount
			cTotalTaxAmount = TaxAmount
		End If
		
		' Process all coinsurance
		If Information.IsArray(Coinsurance) Then
			For lCount As Integer = Coinsurance.GetLowerBound(1) To Coinsurance.GetUpperBound(1)
				' Calculate payment and adjust amount available for reinsurance
				cThisPayment = cTotalAmount * (CDbl(Coinsurance(ACCISharePercent, lCount)) / 100)
				cReinsuranceAmount -= cThisPayment
				
				' Set payment share and loss equvalent
				Coinsurance(ACCIThisPayment, lCount) = cThisPayment
				Coinsurance(ACCIThisPaymentLoss, lCount) = cThisPayment * CurrencyRate
				
				' Calculate tax and adjust amount available for reinsurance
				cThisTax = cTotalTaxAmount * (CDbl(Coinsurance(ACCISharePercent, lCount)) / 100)
				cReinsuranceTaxAmount -= cThisTax
				
				' Set tax ... only if shared with CI
				If CInt(Coinsurance(ACCIIsTaxShared, lCount)) Or (Not IsPostTaxes) Then
					' Set tax share and loss equivalent
					Coinsurance(ACCITaxAmount, lCount) = cThisTax
					Coinsurance(ACCITaxAmountLoss, lCount) = cThisTax * CurrencyRate
				End If
			Next lCount
		End If
		
		' Process all reinsurance
		If Information.IsArray(Reinsurance) Then
			For lCount As Integer = Reinsurance.GetLowerBound(1) To Reinsurance.GetUpperBound(1)
				' Calculate payment
				cThisPayment = cReinsuranceAmount * (CDbl(Reinsurance(ACRISharePercent, lCount)) / 100)
				
				' Set payment share and loss equvalent
				Reinsurance(ACRIThisPayment, lCount) = cThisPayment
				Reinsurance(ACRIThisPaymentLoss, lCount) = cThisPayment * CurrencyRate
				
				' Calculate tax
				cThisTax = cReinsuranceTaxAmount * (CDbl(Reinsurance(ACRISharePercent, lCount)) / 100)
				
				' Set tax ... only if shared with RI
				If CInt(Reinsurance(ACRIIsTaxShared, lCount)) Or (Not IsPostTaxes) Then
					' Set tax share and loss equivalent
					Reinsurance(ACRITaxAmount, lCount) = cThisTax
					Reinsurance(ACRITaxAmountLoss, lCount) = cThisTax * CurrencyRate
				End If
			Next lCount
		End If
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		Finally
'		Return result
		
		' This is for debugging only
'		Resume 
		
'		Return result
		End Try
		Return result
	End Function
End Class
