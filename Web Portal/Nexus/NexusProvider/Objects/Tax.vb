Imports Microsoft.Web.Services3.Security.Tokens
''' <summary>
''' Nexus BaseTax object, containing the common elements between the various perilclaim.
''' </summary>
''' <remarks></remarks>
''' 
<Serializable()> Public Class Tax

    Private sTypeCode As String
    Private sCode As String
    Private sDescription As String
    Private bIsWithHoldingTax As Boolean

    Private sTaxGroup, sTaxBand, sCalculationBasis, sClassOfBusiness, sCountry, sState, sApplyTaxBy As String
    Private iSequence As Integer
    Private dTaxAmount, dRate As Double
    Private bIsNotAppliedToClient, bIncludeinInstallment, bSpreadAcrossInstallment As Boolean
   
    Public Sub New()
        'initialize the collection
    End Sub
    Public Property SpreadAcrossInstallment() As Boolean
        Get
            Return Me.bSpreadAcrossInstallment
        End Get
        Set(ByVal value As Boolean)
            Me.bSpreadAcrossInstallment = value
        End Set
    End Property
    Public Property IncludeinInstallment() As Boolean
        Get
            Return Me.bIncludeinInstallment
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeinInstallment = value
        End Set
    End Property
    Public Property IsNotAppliedToClient() As Boolean
        Get
            Return Me.bIsNotAppliedToClient
        End Get
        Set(ByVal value As Boolean)
            Me.bIsNotAppliedToClient = value
        End Set
    End Property
    Public Property Rate() As Double
        Get
            Return Me.dRate
        End Get
        Set(ByVal value As Double)
            Me.dRate = value
        End Set
    End Property
    Public Property TaxAmount() As Double
        Get
            Return Me.dTaxAmount
        End Get
        Set(ByVal value As Double)
            Me.dTaxAmount = value
        End Set
    End Property
    Public Property Sequence() As Integer
        Get
            Return Me.iSequence
        End Get
        Set(ByVal value As Integer)
            Me.iSequence = value
        End Set
    End Property
    Public Property ApplyTaxBy() As String
        Get
            Return Me.sApplyTaxBy
        End Get
        Set(ByVal value As String)
            Me.sApplyTaxBy = value
        End Set
    End Property
    Public Property State() As String
        Get
            Return Me.sState
        End Get
        Set(ByVal value As String)
            Me.sState = value
        End Set
    End Property
    Public Property Country() As String
        Get
            Return Me.sCountry
        End Get
        Set(ByVal value As String)
            Me.sCountry = value
        End Set
    End Property
    Public Property ClassOfBusiness() As String
        Get
            Return Me.sClassOfBusiness
        End Get
        Set(ByVal value As String)
            Me.sClassOfBusiness = value
        End Set
    End Property
    Public Property CalculationBasis() As String
        Get
            Return Me.sCalculationBasis
        End Get
        Set(ByVal value As String)
            Me.sCalculationBasis = value
        End Set
    End Property
    Public Property TaxBand() As String
        Get
            Return Me.sTaxBand
        End Get
        Set(ByVal value As String)
            Me.sTaxBand = value
        End Set
    End Property
    Public Property TaxGroup() As String
        Get
            Return Me.sTaxGroup
        End Get
        Set(ByVal value As String)
            Me.sTaxGroup = value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return Me.sTypeCode
        End Get
        Set(ByVal value As String)
            Me.sTypeCode = value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    Public Property IsWithHoldingTax() As Boolean
        Get
            Return Me.bIsWithHoldingTax
        End Get
        Set(ByVal value As Boolean)
            Me.bIsWithHoldingTax = value
        End Set
    End Property

    ''' <summary>
    ''' TO identufy is value is % or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsValue() As Boolean

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrencyCode() As String

End Class

<Serializable()> Public Class TaxCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As Document In List
        '    sbPrint.AppendLine(oDocument.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a TaxGroup object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oReceiptTaxGroup As Tax) As Integer
        Return List.Add(v_oReceiptTaxGroup)
    End Function

    ''' <summary>
    ''' Remove an ReserveType object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oReceiptTaxGroup As Tax)
        List.Remove(v_oReceiptTaxGroup)
    End Sub

    ''' <summary>
    ''' Remove an ReceiptTaxGroup object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ReceiptTaxGroup object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ReceiptTaxGroup object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ReceiptTaxGroup object</param>
    ''' <value>The replacement ReceiptTaxGroup object</value>
    ''' <returns>The ReceiptTaxGroup object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Tax
        Get
            Return List(i)
        End Get
        Set(ByVal value As Tax)
            List(i) = value
        End Set
    End Property

End Class
