Option Strict Off
Option Explicit On
Imports SSP.Shared

Public NotInheritable Class cReceiptTaxParameters

    Private Const ACClass As String = "cScriptingItem"

    Private m_vProcessType As Object
    Private m_vPayee As Object
    Private m_vInsuredDomiciled As Object
    Private m_vInsuredPercentage As Object
    Private m_vInsuredTaxNumber As Object
    Private m_vIsTaxExempt As Object
    Private m_vIsSettlement As Object
    Private m_vCurrencyCode As Object
    Private m_vAmount As Object
    Private m_vTaxArray As Object
    Private m_vReceivablePercentage As Object
    Private m_vErrorMessage As Object

    'NOTE: Similar Set of Constants are defined in bGISPMUExtras
    'So Any change here should be made available there as well.
    Private Const kProcessType As Integer = 0
    Private Const kPayee As Integer = 1
    Private Const kPaymentToCode As Integer = 2
    Private Const kInsuredDomiciled As Integer = 3
    Private Const kInsuredPercentage As Integer = 4
    Private Const kInsuredTaxNumber As Integer = 5
    Private Const kIsTaxExempt As Integer = 6
    Private Const kIsSettlement As Integer = 7
    Private Const kCurrencyCode As Integer = 8
    Private Const kAmount As Integer = 9
    Private Const kTaxArray As Integer = 10
    Private Const kReceivablePercentage As Integer = 11
    Private Const kErrorMessage As Integer = 12
    Private Const kRecieptTaxParameters_FieldCount As Integer = kErrorMessage

    Public Property ProcessType() As Object
        Get
            Return m_vProcessType
        End Get
        Set(ByVal Value As Object)

            m_vProcessType = Value
        End Set
    End Property

    Public Property Payee() As Object
        Get
            Return m_vPayee
        End Get
        Set(ByVal Value As Object)

            m_vPayee = Value
        End Set
    End Property
    Public Property InsuredDomiciled() As Object
        Get
            Return m_vInsuredDomiciled
        End Get
        Set(ByVal Value As Object)

            m_vInsuredDomiciled = Value
        End Set
    End Property

    Public Property InsuredPercentage() As Object
        Get
            Return m_vInsuredPercentage
        End Get
        Set(ByVal Value As Object)

            m_vInsuredPercentage = Value
        End Set
    End Property

    Public Property InsuredTaxNumber() As Object
        Get
            Return m_vInsuredTaxNumber
        End Get
        Set(ByVal Value As Object)

            m_vInsuredTaxNumber = Value
        End Set
    End Property

    Public Property IsTaxExempt() As Object
        Get
            Return m_vIsTaxExempt
        End Get
        Set(ByVal Value As Object)

            m_vIsTaxExempt = Value
        End Set
    End Property

    Public Property IsSettlement() As Object
        Get
            Return m_vIsSettlement
        End Get
        Set(ByVal Value As Object)

            m_vIsSettlement = Value
        End Set
    End Property

    Public Property CurrencyCode() As Object
        Get
            Return m_vCurrencyCode
        End Get
        Set(ByVal Value As Object)
            m_vCurrencyCode = Value
        End Set
    End Property

    Public Property Amount() As Object
        Get
            Return m_vAmount
        End Get
        Set(ByVal Value As Object)

            m_vAmount = Value
        End Set
    End Property

    Public Property TaxArray() As Object
        Get
            Return m_vTaxArray
        End Get
        Set(ByVal Value As Object)

            m_vTaxArray = Value
        End Set
    End Property

    Public Property ReceivablePercentage() As Object
        Get
            Return m_vReceivablePercentage
        End Get
        Set(ByVal Value As Object)

            m_vReceivablePercentage = Value
        End Set
    End Property

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
            m_vInsuredDomiciled = v_vDataArray(kInsuredDomiciled)
            m_vInsuredPercentage = v_vDataArray(kInsuredPercentage)
            m_vInsuredTaxNumber = v_vDataArray(kInsuredTaxNumber)
            m_vIsTaxExempt = v_vDataArray(kIsTaxExempt)
            m_vIsSettlement = v_vDataArray(kIsSettlement)
            m_vCurrencyCode = v_vDataArray(kCurrencyCode)
            m_vAmount = v_vDataArray(kAmount)
            ' ARRAY OF TAX BAND RATES
            m_vTaxArray = v_vDataArray(kTaxArray)
            m_vReceivablePercentage = v_vDataArray(kReceivablePercentage)
            m_vErrorMessage = v_vDataArray(kErrorMessage)


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function DataToArray(ByRef v_vDataArray() As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim v_vDataArray(kRecieptTaxParameters_FieldCount)

            v_vDataArray(kProcessType) = m_vProcessType
            v_vDataArray(kPayee) = m_vPayee
            v_vDataArray(kInsuredDomiciled) = m_vInsuredDomiciled
            v_vDataArray(kInsuredPercentage) = m_vInsuredPercentage
            v_vDataArray(kInsuredTaxNumber) = m_vInsuredTaxNumber
            v_vDataArray(kIsTaxExempt) = m_vIsTaxExempt
            v_vDataArray(kIsSettlement) = m_vIsSettlement
            v_vDataArray(kCurrencyCode) = m_vCurrencyCode
            v_vDataArray(kAmount) = m_vAmount

            ' array of tax band rates
            If m_vTaxArray.Length > 0 Then
                If Not String.IsNullOrEmpty(CStr(m_vInsuredPercentage)) AndAlso Not m_vInsuredPercentage Is Nothing Then
                    m_vTaxArray(3, 0) = m_vInsuredPercentage
                ElseIf Not String.IsNullOrEmpty(CStr(m_vReceivablePercentage)) AndAlso Not m_vReceivablePercentage Is Nothing AndAlso m_vInsuredDomiciled = False Then
                    m_vTaxArray(3, 0) = m_vReceivablePercentage
                End If
            End If

            v_vDataArray(kTaxArray) = m_vTaxArray
            v_vDataArray(kReceivablePercentage) = m_vReceivablePercentage
            v_vDataArray(kErrorMessage) = m_vErrorMessage


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
End Class