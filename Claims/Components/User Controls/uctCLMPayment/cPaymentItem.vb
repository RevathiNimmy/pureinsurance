Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no 129. 
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("cPaymentItem_NET.cPaymentItem")> _
Public NotInheritable Class cPaymentItem

	Private Const ACClass As String = "cPaymentItem"
	Private m_lReserveId As Integer
	Private m_lExchangeRateOverrideReasonId As Integer
	Private m_lCurrencyId As Integer
	Private m_lWorkClaimPaymentId As CheckState
	Private m_crThisPayment As Decimal
	Private m_lTaxGroupId As Integer
	Private m_crTaxAmount As Decimal
	Private m_crTaxAmountWHT As Decimal
	Private m_dCurrencyToBaseXRate As Double
	Private m_dtCurrencyToBaseDate As Date
	Private m_dAccountToBaseXRate As Double
	Private m_dtAccountToBaseDate As Date
	Private m_dSystemToBaseXRate As Double
	Private m_dtSystemToBaseDate As Date
	Private m_dPaymentToLossXRate As Double
	Private m_oBusiness As Object
	Private m_sCurrencyDescription As String = ""
	Private m_sTaxGroupDescription As String = ""
	Private m_crTotalReserve As Decimal
	Private m_crPaidToDate As Decimal
	Private m_crBalance As Decimal
	Private m_bIsWithHoldingTax As Boolean
	Private m_sAdvancedTaxScript As String = ""
	Private m_sCurrencyCode As String = ""
	Private m_vTaxBandRateArray As Object
	Private m_sReserveDesc As String = ""
	Private m_crThisRevision As Decimal
	Private m_bIsExcess As Boolean
	Private m_bPostClaimTax As Boolean
	Private m_crPaymentAdjustment As Decimal
	Private m_crThisReserveRevision As Decimal '(RC) QBENZ001
	
	Public WriteOnly Property PostClaimTax() As Boolean
		Set(ByVal Value As Boolean)
			m_bPostClaimTax = Value
		End Set
	End Property
	
	Public Property IsExcess() As Boolean
		Get
			Return m_bIsExcess
		End Get
		Set(ByVal Value As Boolean)
			m_bIsExcess = Value
		End Set
	End Property
	
	Public ReadOnly Property ThisRevisionInLossCurrency() As Decimal
		Get
			GetRevisionAmount()
			Return m_crThisRevision
		End Get
	End Property
	
	
	Public Property ReserveDesc() As String
		Get
			Return m_sReserveDesc
		End Get
		Set(ByVal Value As String)
			m_sReserveDesc = Value
		End Set
	End Property
	
	
	Public Property TaxBandRateArray() As Object
		Get
			Return m_vTaxBandRateArray
		End Get
		Set(ByVal Value As Object)


			m_vTaxBandRateArray = Value
		End Set
	End Property
	
	
	Public Property CurrencyCode() As String
		Get
			Return m_sCurrencyCode
		End Get
		Set(ByVal Value As String)
			m_sCurrencyCode = Value
		End Set
	End Property
	
	
	Public Property AdvancedTaxScript() As String
		Get
			Return m_sAdvancedTaxScript
		End Get
		Set(ByVal Value As String)
			m_sAdvancedTaxScript = Value
		End Set
	End Property
	
	
	Public Property IsWithHoldingTax() As Boolean
		Get
			Return m_bIsWithHoldingTax
		End Get
		Set(ByVal Value As Boolean)
			m_bIsWithHoldingTax = Value
		End Set
	End Property
	
	
	Public Property TotalReserve() As Decimal
		Get
			Return m_crTotalReserve
		End Get
		Set(ByVal Value As Decimal)
			m_crTotalReserve = Value
		End Set
	End Property
	
	'(RC) QBENZ001
	Public Property ThisReserveRevision() As Decimal
		Get
			Return m_crThisReserveRevision
		End Get
		Set(ByVal Value As Decimal)
			m_crThisReserveRevision = Value
		End Set
	End Property
	
	
	Public Property PaidToDate() As Decimal
		Get
			Return m_crPaidToDate
		End Get
		Set(ByVal Value As Decimal)
			m_crPaidToDate = Value
		End Set
	End Property
	
	
	Public Property Balance() As Decimal
		Get
			Return m_crBalance
		End Get
		Set(ByVal Value As Decimal)
			m_crBalance = Value
		End Set
	End Property
	
	Public WriteOnly Property Business() As Object
		Set(ByVal Value As Object)
			Business = Value
		End Set
	End Property
	
	
	Public Property ExchangeRateOverrideReasonId() As Integer
		Get
			Return m_lExchangeRateOverrideReasonId
		End Get
		Set(ByVal Value As Integer)
			m_lExchangeRateOverrideReasonId = Value
		End Set
	End Property
	
	
	Public Property WorkClaimPaymentId() As Integer
		Get
			Return m_lWorkClaimPaymentId
		End Get
		Set(ByVal Value As Integer)
			m_lWorkClaimPaymentId = Value
		End Set
	End Property
	
	
	Public Property ReserveId() As Integer
		Get
			Return m_lReserveId
		End Get
		Set(ByVal Value As Integer)
			m_lReserveId = Value
		End Set
	End Property
	
	
	Public Property CurrencyId() As Integer
		Get
			Return m_lCurrencyId
		End Get
		Set(ByVal Value As Integer)
			m_lCurrencyId = Value
		End Set
	End Property
	
	
	Public Property ThisPayment() As Decimal
		Get
			Return m_crThisPayment
		End Get
		Set(ByVal Value As Decimal)
			m_crThisPayment = Value
		End Set
	End Property
	
	
	Public Property TaxGroupId() As Integer
		Get
			Return m_lTaxGroupId
		End Get
		Set(ByVal Value As Integer)
			m_lTaxGroupId = Value
		End Set
	End Property
	
	
	Public Property TaxAmount() As Decimal
		Get
			Return m_crTaxAmount
		End Get
		Set(ByVal Value As Decimal)
			m_crTaxAmount = Value
		End Set
	End Property
	
	
	Public Property TaxAmountWHT() As Decimal
		Get
			Return m_crTaxAmountWHT
		End Get
		Set(ByVal Value As Decimal)
			m_crTaxAmountWHT = Value
		End Set
	End Property
	
	
	Public Property SystemToBaseXRate() As Double
		Get
			Return m_dSystemToBaseXRate
		End Get
		Set(ByVal Value As Double)
			m_dSystemToBaseXRate = Value
		End Set
	End Property
	
	
	Public Property SystemToBaseDate() As Date
		Get
			Return m_dtSystemToBaseDate
		End Get
		Set(ByVal Value As Date)
			m_dtSystemToBaseDate = Value
		End Set
	End Property
	
	
	Public Property AccountToBaseXRate() As Double
		Get
			Return m_dAccountToBaseXRate
		End Get
		Set(ByVal Value As Double)
			m_dAccountToBaseXRate = Value
		End Set
	End Property
	
	
	Public Property AccountToBaseDate() As Date
		Get
			Return m_dtAccountToBaseDate
		End Get
		Set(ByVal Value As Date)
			m_dtAccountToBaseDate = Value
		End Set
	End Property
	
	
	Public Property CurrencyToBaseXRate() As Double
		Get
			Return m_dCurrencyToBaseXRate
		End Get
		Set(ByVal Value As Double)
			m_dCurrencyToBaseXRate = Value
		End Set
	End Property
	
	
	Public Property CurrencyToBaseDate() As Date
		Get
			Return m_dtCurrencyToBaseDate
		End Get
		Set(ByVal Value As Date)
			m_dtCurrencyToBaseDate = Value
		End Set
	End Property
	
	
	Public Property PaymentToLossXRate() As Double
		Get
			Return m_dPaymentToLossXRate
		End Get
		Set(ByVal Value As Double)
			m_dPaymentToLossXRate = Value
		End Set
	End Property
	
	
	Public Property TaxGroupDescription() As String
		Get
			Return m_sTaxGroupDescription
		End Get
		Set(ByVal Value As String)
			m_sTaxGroupDescription = Value
		End Set
	End Property
	
	
	Public Property CurrencyDescription() As String
		Get
			Return m_sCurrencyDescription
		End Get
		Set(ByVal Value As String)
			m_sCurrencyDescription = Value
		End Set
	End Property
	
	
	Public Property PaymentAdjustment() As Decimal
		Get
			Return m_crPaymentAdjustment
		End Get
		Set(ByVal Value As Decimal)
			m_crPaymentAdjustment = Value
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: RecalculateTaxAmounts
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
	' ***************************************************************** '
	Public Function RecalculateTaxAmounts() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "RecalculateTaxAmounts"
		
		Dim lReturn, llBound, lUBound As Integer
		Dim crTotalTaxAmount As Decimal
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If Information.IsArray(m_vTaxBandRateArray) Then
			

			llBound = m_vTaxBandRateArray.GetLowerBound(1)

			lUBound = m_vTaxBandRateArray.GetUpperBound(1)
			
			For lTaxItem As Integer = llBound To lUBound
				

                crTotalTaxAmount += CDbl(m_vTaxBandRateArray(kTaxArrayValue, lTaxItem))

            Next

        End If

        If IsWithHoldingTax Then
            m_crTaxAmount = 0
            m_crTaxAmountWHT = crTotalTaxAmount
        Else
            m_crTaxAmount = crTotalTaxAmount
            m_crTaxAmountWHT = 0
        End If


		Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

		Finally

'        Return result
'        Resume
'        Return result
		End Try
		Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetScriptedTaxAmount
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetScriptedTaxAmount() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetScriptedTaxAmount"

        Dim lReturn, llBound, lUBound As Integer
        Dim crTaxAmount As Decimal
        Dim lIsManuallyChanged As Integer
        Dim crScriptedTaxAmount As Decimal

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(m_vTaxBandRateArray) Then


            llBound = m_vTaxBandRateArray.GetLowerBound(1)

            lUBound = m_vTaxBandRateArray.GetUpperBound(1)

            For lTaxItem As Integer = llBound To lUBound


                crTaxAmount = CDec(m_vTaxBandRateArray(kTaxArrayValue, lTaxItem))

                lIsManuallyChanged = CInt(m_vTaxBandRateArray(kTaxArrayIsManuallyChanged, lTaxItem))

                If lIsManuallyChanged = 2 Then
                    crScriptedTaxAmount += crTaxAmount
                End If

            Next

        End If

        m_crTaxAmount = crScriptedTaxAmount


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

'        Return result
'        Resume
'        Return result
        End Try
        Return result
    End Function
	
	' ***************************************************************** '
	' Name: GetRevisionAmount
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : 13-09-2005 : 360 - Taxes on Claims
	' ***************************************************************** '
	Public Function GetRevisionAmount() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetRevisionAmount"
		
		Dim lReturn As Integer
		Dim crPaidToDate, crTotalReserve, crThisRevision, crThisPayment, crThisTaxAmount, crCurrentReserve As Decimal
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' get revision details
		m_crThisRevision = 0
		
		' already in loss currency
		crPaidToDate = m_crPaidToDate
		crTotalReserve = m_crTotalReserve
		crCurrentReserve = m_crTotalReserve - m_crPaidToDate
		
		' need to be converted to loss currency from payment currency
		crThisPayment = m_crThisPayment * m_dPaymentToLossXRate
		crThisTaxAmount = (m_crTaxAmount + m_crTaxAmountWHT) * m_dPaymentToLossXRate
		
		If Not m_bIsExcess Then
			
			' if the system is posting claim tax seperately
			' then reserve adjustments dont need to include the tax amount
			If m_bPostClaimTax Then
				
				' if the Total Payment ( this payment) > current reserve ( initial + revised - paid to date)
				If crThisPayment > crCurrentReserve + m_crThisReserveRevision Then
					' then adjust the reserve accordingly so that
					' when the payment is applied the reserve amount
					' is reduced to zero...
					m_crThisRevision = crThisPayment - crCurrentReserve
				Else
					m_crThisRevision = m_crThisReserveRevision
				End If
				
			Else
				' if the system is "NOT" posting claim tax seperately
				' then reserve adjustments "DO" need to include the tax amount
				
				' if the Total Payment ( paid to date + this payment) > current reserve ( initial + revised  - paid to date)
				If crThisPayment + crThisTaxAmount > crCurrentReserve + m_crThisReserveRevision Then
					' then adjust the reserve accordingly so that
					' when the payment is applied the reserve amount
					' is reduced to zero...
					m_crThisRevision = crThisPayment + crThisTaxAmount - crCurrentReserve
				Else
					m_crThisRevision = m_crThisReserveRevision
				End If
				
			End If
		Else
			
			' for excess everything is in reverse...
			' note that these figures are negative for excess so what we should see is
			' if -600 + -700 > -1000 then
			'  revision amount = -1300 - - 1000 = -300
			If crThisPayment < crCurrentReserve Then
				' then adjust the reserve accordingly so that
				' when the payment is applied the reserve amount
				' is reduced to zero...
				m_crThisRevision = crThisPayment - crCurrentReserve
			End If
			
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
End Class
