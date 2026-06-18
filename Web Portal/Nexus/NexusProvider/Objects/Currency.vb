''' <summary>
''' Nexus currency object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Currency : Inherits CurrencyExchangeRateType

    Private sCurrencyCode As String = Nothing
    Private sDescription As String = Nothing

    'adding Properties for GetCurrencyExchangeRatesRequestType
    Private accountCodeField As String

    Private transactionCurrencyCodeField As String

    Private currencyAmountUnRoundedField As Decimal

    Private currencyAmountUnRoundedFieldSpecified As Boolean

    'adding Properties for GetCurrencyExchangeRatesResponseType
    Private oCurrencyRates As CurrencyExchangeRateType

    Private baseAmountField As Decimal

    Private baseAmountUnroundedField As Decimal

    Private accountAmountField As Decimal

    Private accountAmountUnroundedField As Decimal

    Private systemAmountField As Decimal

    Private systemAmountUnroundedField As Decimal

    Private modeField As String
    Private baseCurrencyCodeField As String

    ''' <summary>
    ''' Desfault constructor
    ''' </summary>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
    Public Property BaseCurrencyCode() As String
        Get
            Return baseCurrencyCodeField
        End Get
        Set(ByVal value As String)
            baseCurrencyCodeField = value
        End Set
    End Property
   
    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value>Currency Code</value>
    ''' <returns>Currency Code</returns>
    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property

    ''' <summary>
    ''' Description
    ''' </summary>
    ''' <value>Description</value>
    ''' <returns>Description</returns>
    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property
    'adding properties for GetCurrencyExchangeRatesRequestType
    Public Property AccountCode() As String
        Get
            Return Me.accountCodeField
        End Get
        Set(ByVal value As String)
            Me.accountCodeField = value
        End Set
    End Property
    Public Property TransactionCurrencyCode() As String
        Get
            Return Me.transactionCurrencyCodeField
        End Get
        Set(ByVal value As String)
            Me.transactionCurrencyCodeField = value
        End Set
    End Property
    Public Property CurrencyAmountUnRounded() As Decimal
        Get
            Return Me.currencyAmountUnRoundedField
        End Get
        Set(ByVal value As Decimal)
            Me.currencyAmountUnRoundedField = value
        End Set
    End Property
    Public Property CurrencyAmountUnRoundedSpecified() As Boolean
        Get
            Return Me.currencyAmountUnRoundedFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.currencyAmountUnRoundedFieldSpecified = value
        End Set
    End Property
    Public Property Mode() As String
        Get
            Return Me.modeField
        End Get
        Set(ByVal value As String)
            Me.modeField = value
        End Set
    End Property
    'adding properties for GetCurrencyExchangeRatesResponseType
    Public Property CurrencyRates() As CurrencyExchangeRateType
        Get
            Return oCurrencyRates
        End Get
        Set(ByVal value As CurrencyExchangeRateType)
            oCurrencyRates = value
        End Set
    End Property
    Public Property BaseAmount() As Decimal
        Get
            Return Me.baseAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.baseAmountField = value
        End Set
    End Property
    Public Property BaseAmountUnrounded() As Decimal
        Get
            Return Me.baseAmountUnroundedField
        End Get
        Set(ByVal value As Decimal)
            Me.baseAmountUnroundedField = value
        End Set
    End Property
    Public Property AccountAmount() As Decimal
        Get
            Return Me.accountAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.accountAmountField = value
        End Set
    End Property
    Public Property AccountAmountUnrounded() As Decimal
        Get
            Return Me.accountAmountUnroundedField
        End Get
        Set(ByVal value As Decimal)
            Me.accountAmountUnroundedField = value
        End Set
    End Property
    Public Property SystemAmount() As Decimal
        Get
            Return Me.systemAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.systemAmountField = value
        End Set
    End Property
    Public Property SystemAmountUnrounded() As Decimal
        Get
            Return Me.systemAmountUnroundedField
        End Get
        Set(ByVal value As Decimal)
            Me.systemAmountUnroundedField = value
        End Set
    End Property
End Class

''' <summary>
''' Collection of currency objects
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CurrencyCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(Currency)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCurrency As Currency In List
            sbPrint.AppendLine(oCurrency.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Currency object to the collection
    ''' </summary>
    ''' <param name="v_oCurrency">The Currency object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oCurrency As Currency) As Integer
        Return List.Add(v_oCurrency)
    End Function

    ''' <summary>
    ''' Remove an Currency object from the collection
    ''' </summary>
    ''' <param name="v_oCurrency">The Currency object to be removed</param>
    Public Sub Remove(ByVal v_oCurrency As Currency)
        List.Remove(v_oCurrency)
    End Sub

    ''' <summary>
    ''' Remove an Currency object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Currency object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Currency object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Currency object</param>
    ''' <value>The replacement Currency object</value>
    ''' <returns>The Currency object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Currency
        Get
            Return List(i)
        End Get
        Set(ByVal value As Currency)
            List(i) = value
        End Set
    End Property

End Class