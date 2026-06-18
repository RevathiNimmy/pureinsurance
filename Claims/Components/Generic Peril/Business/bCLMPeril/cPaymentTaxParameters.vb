Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("cPaymentTaxParameters_NET.cPaymentTaxParameters")> _
Public NotInheritable Class cPaymentTaxParameters

    Private Const ACClass As String = "cScriptingItem"

    Private m_vProcessType As Object
    Private m_vPayee As Object
    Private m_vPaymentToCode As Object
    Private m_vSafeHarbourCode As Object
    Private m_vSafeHarbourPercentage As Object
    Private m_vInsuredDomiciled As Object
    Private m_vInsuredPercentage As Object
    Private m_vInsuredTaxNumber As Object
    Private m_vPayeeDomiciled As Object
    Private m_vPayeePercentage As Object
    Private m_vPayeeTaxNumber As Object
    Private m_vIsTaxExempt As Object
    Private m_vIsWHTExempt As Object
    Private m_vIsSettlement As Object
    Private m_vCurrencyCode As Object
    Private m_vAmount As Object
    Private m_vExcessAmount As Object
    Private m_vPaymentAdjustment As Object
    Private m_vTaxArray As Object
    Private m_vErrorMessage As Object

    'NOTE: Similar Set of Constants are defined in bGISPMUExtras
    'So Any change here should be made available there as well.
    Private Const kProcessType As Integer = 0
    Private Const kPayee As Integer = 1
    Private Const kPaymentToCode As Integer = 2
    Private Const kSafeHarbourCode As Integer = 3
    Private Const kSafeHarbourPercentage As Integer = 4
    Private Const kInsuredDomiciled As Integer = 5
    Private Const kInsuredPercentage As Integer = 6
    Private Const kInsuredTaxNumber As Integer = 7
    Private Const kPayeeDomiciled As Integer = 8
    Private Const kPayeePercentage As Integer = 9
    Private Const kPayeeTaxNumber As Integer = 10
    Private Const kIsTaxExempt As Integer = 11
    Private Const kIsWHTExempt As Integer = 12
    Private Const kIsSettlement As Integer = 13
    Private Const kCurrencyCode As Integer = 14
    Private Const kAmount As Integer = 15
    Private Const kExcessAmount As Integer = 16
    Private Const kPaymentAdjustment As Integer = 17
    Private Const kTaxArray As Integer = 18
    Private Const kErrorMessage As Integer = 19
    Private Const kPaymentTaxParameters_FieldCount As Integer = kErrorMessage
    ' ***************************************************************** '
    'kTaxArrayPosTaxGroupCode = 1
    'kTaxArrayPosTaxBandCode = 2
    'kTaxArrayPosCurrencyCode = 3
    'kTaxArrayPosValue = 4
    'kTaxArrayPosPercentage = 5
    'kTaxArrayPosIsValue = 6
    'kTaxArrayPosClassOfBusinessId = 7
    'kTaxArrayPosSequence = 8
    ' ***************************************************************** '
    Public Property ProcessType() As Object
        Get
            Return m_vProcessType
        End Get
        Set(ByVal Value As Object)

            m_vProcessType = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property Payee() As Object
        Get
            Return m_vPayee
        End Get
        Set(ByVal Value As Object)

            m_vPayee = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PaymentToCode() As Object
        Get
            Return m_vPaymentToCode
        End Get
        Set(ByVal Value As Object)

            m_vPaymentToCode = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property SafeHarbourCode() As Object
        Get
            Return m_vSafeHarbourCode
        End Get
        Set(ByVal Value As Object)

            m_vSafeHarbourCode = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property SafeHarbourPercentage() As Object
        Get
            Return m_vSafeHarbourPercentage
        End Get
        Set(ByVal Value As Object)

            m_vSafeHarbourPercentage = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property InsuredDomiciled() As Object
        Get
            Return m_vInsuredDomiciled
        End Get
        Set(ByVal Value As Object)

            m_vInsuredDomiciled = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property InsuredPercentage() As Object
        Get
            Return m_vInsuredPercentage
        End Get
        Set(ByVal Value As Object)

            m_vInsuredPercentage = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property InsuredTaxNumber() As Object
        Get
            Return m_vInsuredTaxNumber
        End Get
        Set(ByVal Value As Object)

            m_vInsuredTaxNumber = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PayeeDomiciled() As Object
        Get
            Return m_vPayeeDomiciled
        End Get
        Set(ByVal Value As Object)

            m_vPayeeDomiciled = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PayeePercentage() As Object
        Get
            Return m_vPayeePercentage
        End Get
        Set(ByVal Value As Object)

            m_vPayeePercentage = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PayeeTaxNumber() As Object
        Get
            Return m_vPayeeTaxNumber
        End Get
        Set(ByVal Value As Object)

            m_vPayeeTaxNumber = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property IsTaxExempt() As Object
        Get
            Return m_vIsTaxExempt
        End Get
        Set(ByVal Value As Object)

            m_vIsTaxExempt = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property IsWHTExempt() As Object
        Get
            Return m_vIsWHTExempt
        End Get
        Set(ByVal Value As Object)

            m_vIsWHTExempt = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property IsSettlement() As Object
        Get
            Return m_vIsSettlement
        End Get
        Set(ByVal Value As Object)

            m_vIsSettlement = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property CurrencyCode() As Object
        Get
            Return m_vCurrencyCode
        End Get
        Set(ByVal Value As Object)

            m_vCurrencyCode = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property Amount() As Object
        Get
            Return m_vAmount
        End Get
        Set(ByVal Value As Object)

            m_vAmount = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property ExcessAmount() As Object
        Get
            Return m_vExcessAmount
        End Get
        Set(ByVal Value As Object)

            m_vExcessAmount = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property PaymentAdjustment() As Object
        Get
            Return m_vPaymentAdjustment
        End Get
        Set(ByVal Value As Object)

            m_vPaymentAdjustment = Value
        End Set
    End Property
    ' ***************************************************************** '
    Public Property TaxArray() As Object
        Get
            Return m_vTaxArray
        End Get
        Set(ByVal Value As Object)

            m_vTaxArray = Value
        End Set
    End Property

    ' Error Message
    Public Property ErrorMessage() As Object
        Get
            Return m_vErrorMessage
        End Get
        Set(ByVal Value As Object)

            m_vErrorMessage = Value
        End Set
    End Property

    Public Function ArrayToData(ByVal v_vDataArray() As Object) As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vProcessType = v_vDataArray(kProcessType)
            m_vPayee = v_vDataArray(kPayee)
            m_vPaymentToCode = v_vDataArray(kPaymentToCode)
            m_vSafeHarbourCode = v_vDataArray(kSafeHarbourCode)
            m_vSafeHarbourPercentage = v_vDataArray(kSafeHarbourPercentage)
            m_vInsuredDomiciled = v_vDataArray(kInsuredDomiciled)
            m_vInsuredPercentage = v_vDataArray(kInsuredPercentage)
            m_vInsuredTaxNumber = v_vDataArray(kInsuredTaxNumber)
            m_vPayeeDomiciled = v_vDataArray(kPayeeDomiciled)
            m_vPayeePercentage = v_vDataArray(kPayeePercentage)
            m_vPayeeTaxNumber = v_vDataArray(kPayeeTaxNumber)
            m_vIsTaxExempt = v_vDataArray(kIsTaxExempt)
            m_vIsWHTExempt = v_vDataArray(kIsWHTExempt)
            m_vIsSettlement = v_vDataArray(kIsSettlement)
            m_vCurrencyCode = v_vDataArray(kCurrencyCode)
            m_vAmount = v_vDataArray(kAmount)
            m_vExcessAmount = v_vDataArray(kExcessAmount)
            m_vPaymentAdjustment = v_vDataArray(kPaymentAdjustment)
            ' ARRAY OF TAX BAND RATES
            m_vTaxArray = v_vDataArray(kTaxArray)
            m_vErrorMessage = v_vDataArray(kErrorMessage)


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function DataToArray(ByRef v_vDataArray() As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim v_vDataArray(kErrorMessage)

            v_vDataArray(kProcessType) = m_vProcessType
            v_vDataArray(kPayee) = m_vPayee
            v_vDataArray(kPaymentToCode) = m_vPaymentToCode
            v_vDataArray(kSafeHarbourCode) = m_vSafeHarbourCode
            v_vDataArray(kSafeHarbourPercentage) = m_vSafeHarbourPercentage
            v_vDataArray(kInsuredDomiciled) = m_vInsuredDomiciled
            v_vDataArray(kInsuredPercentage) = m_vInsuredPercentage
            v_vDataArray(kInsuredTaxNumber) = m_vInsuredTaxNumber
            v_vDataArray(kPayeeDomiciled) = m_vPayeeDomiciled
            v_vDataArray(kPayeePercentage) = m_vPayeePercentage
            v_vDataArray(kPayeeTaxNumber) = m_vPayeeTaxNumber
            v_vDataArray(kIsTaxExempt) = m_vIsTaxExempt
            v_vDataArray(kIsWHTExempt) = m_vIsWHTExempt
            v_vDataArray(kIsSettlement) = m_vIsSettlement
            v_vDataArray(kCurrencyCode) = m_vCurrencyCode
            v_vDataArray(kAmount) = m_vAmount

            If m_vExcessAmount IsNot Nothing Then
                v_vDataArray(kExcessAmount) = m_vExcessAmount
            Else
                v_vDataArray(kExcessAmount) = 0
            End If
            If m_vPaymentAdjustment IsNot Nothing Then
                v_vDataArray(kPaymentAdjustment) = m_vPaymentAdjustment
            Else
                v_vDataArray(kPaymentAdjustment) = 0
            End If

            If m_vTaxArray.Length > 0 Then
                If Not String.IsNullOrEmpty(CStr(m_vInsuredPercentage)) AndAlso Not m_vInsuredPercentage Is Nothing AndAlso m_vPayeeDomiciled = False Then
                    m_vTaxArray(3, 0) = m_vInsuredPercentage
                ElseIf Not String.IsNullOrEmpty(CStr(m_vPayeePercentage)) AndAlso Not m_vPayeePercentage Is Nothing AndAlso m_vInsuredDomiciled = False Then
                    m_vTaxArray(3, 0) = m_vPayeePercentage
                Else
                    m_vTaxArray(3, 0) = m_vInsuredPercentage
                End If
            End If

            ' array of tax band rates
            v_vDataArray(kTaxArray) = m_vTaxArray

            ' error message
            v_vDataArray(kErrorMessage) = m_vErrorMessage


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

        Finally

        End Try
        Return result
    End Function
End Class