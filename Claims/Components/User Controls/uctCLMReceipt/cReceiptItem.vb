Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("cReceiptItem_NET.cReceiptItem")> _
Public NotInheritable Class cReceiptItem 
	
	Private Const ACClass As String = "cReceiptItem"
	
	Private m_lRecoveryId As Integer
	Private m_lExchangeRateOverrideReasonId As Integer
	Private m_lCurrencyId As Integer
	Private m_crThisReceipt As Decimal
	Private m_lTaxGroupId As Integer
	Private m_crTaxAmount As Decimal
	Private m_dCurrencyToBaseXRate As Double
	Private m_dtCurrencyToBaseDate As Date
	Private m_dAccountToBaseXRate As Double
	Private m_dtAccountToBaseDate As Date
	Private m_dSystemToBaseXRate As Double
	Private m_dtSystemToBaseDate As Date
	Private m_dReceiptToLossXRate As Double
	Private m_oBusiness As Object
	Private m_sCurrencyDescription As String = ""
	Private m_sTaxGroupDescription As String = ""
	Private m_crTotalReserve As Decimal
	Private m_crReceivedToDate As Decimal
	Private m_crBalance As Decimal
	Private m_sAdvancedTaxScript As String = ""
	Private m_sCurrencyCode As String = ""
	Private m_vTaxBandRateArray As Object
	Private m_sRecoveryTypeDesc As String = ""
	Private m_crScriptedTaxAmount As Decimal
	Private m_crThisRevision As Decimal
	Private m_lRecoveryTypeId As Integer
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.2.1)
	Private m_lRecoveryPartyTypeId As Integer
	Private m_lRecoveryPartyCnt As Integer
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.2.1)
	Private m_lWorkClaimReceiptItemId As Integer
	'Start - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.2.1)
	
	Public Property RecoveryPartyTypeId() As Integer
		Get
			Return m_lRecoveryPartyTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryPartyTypeId = Value
		End Set
	End Property
	
	
	Public Property RecoveryPartyCnt() As Integer
		Get
			Return m_lRecoveryPartyCnt
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryPartyCnt = Value
		End Set
	End Property
	'End - (Sankar) - (Tech Spec WR34 - Claims Recovery Party Link.doc) - (4.3.2.1)
	''Start(Saurabh Agrawal) Tech Spec CLaim Recovery Reinsurance
	
	
	
	Public Property WorkClaimReceiptItemId() As Integer
		Get
			Return m_lWorkClaimReceiptItemId
		End Get
		Set(ByVal Value As Integer)
			m_lWorkClaimReceiptItemId = Value
		End Set
	End Property
	''End(Saurabh Agrawal) Tech Spec CLaim Recovery Reinsurance
	
	
	Public Property RecoveryTypeId() As Integer
		Get
			Return m_lRecoveryTypeId
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryTypeId = Value
		End Set
	End Property
	
	Public ReadOnly Property ThisNet() As Decimal
		Get
			Return m_crThisReceipt - m_crTaxAmount
		End Get
	End Property
	
	Public ReadOnly Property ThisRevisionInLossCurrency() As Decimal
		Get
			GetRevisionAmount()
			Return m_crThisRevision
		End Get
	End Property
	
	Public ReadOnly Property ScriptedTaxAmount() As Decimal
		Get
			GetScriptedTaxAmount()
			Return m_crScriptedTaxAmount
		End Get
	End Property
	
	
	Public Property RecoveryTypeDesc() As String
		Get
			Return m_sRecoveryTypeDesc
		End Get
		Set(ByVal Value As String)
			m_sRecoveryTypeDesc = Value
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
	
	
	Public Property TotalReserve() As Decimal
		Get
			Return m_crTotalReserve
		End Get
		Set(ByVal Value As Decimal)
			m_crTotalReserve = Value
		End Set
	End Property
	
	
	Public Property ReceivedToDate() As Decimal
		Get
			Return m_crReceivedToDate
		End Get
		Set(ByVal Value As Decimal)
			m_crReceivedToDate = Value
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
	
	
	Public Property RecoveryId() As Integer
		Get
			Return m_lRecoveryId
		End Get
		Set(ByVal Value As Integer)
			m_lRecoveryId = Value
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
	
	
	Public Property ThisReceipt() As Decimal
		Get
			Return m_crThisReceipt
		End Get
		Set(ByVal Value As Decimal)
			m_crThisReceipt = Value
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
	
	
	Public Property AccountToBaseXRate() As Decimal
		Get
			Return m_dAccountToBaseXRate
		End Get
		Set(ByVal Value As Decimal)
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
	
	
	Public Property ReceiptToLossXRate() As Double
		Get
			Return m_dReceiptToLossXRate
		End Get
		Set(ByVal Value As Double)
			m_dReceiptToLossXRate = Value
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

        m_crTaxAmount = crTotalTaxAmount


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

        m_crScriptedTaxAmount = crScriptedTaxAmount


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
		Dim crReceivedToDate, crTotalReserve, crThisRevision, crThisReceipt, crThisExcess As Decimal
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' get claim payment item reserve id
		crReceivedToDate = m_crReceivedToDate
		crTotalReserve = m_crTotalReserve
		crThisReceipt = m_crThisReceipt * m_dReceiptToLossXRate
		m_crThisRevision = 0
		
		' if the Total Payment ( paid to date + this payment - this excess) > total reserve ( initial + revised )
		If (crReceivedToDate + crThisReceipt - crThisExcess) > crTotalReserve Then
			' then adjust the reserve accordingly so that
			' when the payment is applied the reserve amount
			' is reduced to zero...
			m_crThisRevision = crReceivedToDate + crThisReceipt - crThisExcess - crTotalReserve
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
