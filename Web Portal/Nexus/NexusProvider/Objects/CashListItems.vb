<Serializable()> Public Class CashListItems

    Private iClaimPaymentKey As Integer
    Private iClaimKey As Integer
    Private sClaimNumber As String
    Private sPolicyNumber As String
    Private sClientName As String
    Private dPaymentAmount As Double
    Private dPaymentDate As Date
    Private sCreatedBy As String
    Private sStatus As String
    Private iClientKey As Integer
    Private bReferredForRecommendation As Boolean
    Private sRecommendedBy As String
    Private sCurrencyCode As String
    Private sPayeeName As String
    Private sPartyType As String
    Private sCaseNumber As String
    Private nPaymentAmountBaseCurrency As Double



    Private iCurrencyId As Integer

    '''<remarks/>
    Public Property ClientKey() As Integer
        Get
            Return Me.iClientKey
        End Get
        Set(ByVal value As Integer)
            Me.iClientKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimPaymentKey() As Integer
        Get
            Return Me.iClaimPaymentKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimPaymentKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyNumber() As String
        Get
            Return Me.sPolicyNumber
        End Get
        Set(ByVal value As String)
            Me.sPolicyNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientName() As String
        Get
            Return Me.sClientName
        End Get
        Set(ByVal value As String)
            Me.sClientName = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentAmount() As Double
        Get
            Return Me.dPaymentAmount
        End Get
        Set(ByVal value As Double)
            Me.dPaymentAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentDate() As Date
        Get
            Return Me.dPaymentDate
        End Get
        Set(ByVal value As Date)
            Me.dPaymentDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property CreatedBy() As String
        Get
            Return Me.sCreatedBy
        End Get
        Set(ByVal value As String)
            Me.sCreatedBy = value
        End Set
    End Property

    '''<remarks/>
    Public Property Status() As String
        Get
            Return Me.sStatus
        End Get
        Set(ByVal value As String)
            Me.sStatus = value
        End Set
    End Property

    ''' <summary>
    ''' IsReferredForRecommendation Boolean Property
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsReferredForRecommendation() As Boolean
        Get
            Return Me.bReferredForRecommendation
        End Get
        Set(ByVal value As Boolean)
            Me.bReferredForRecommendation = value
        End Set
    End Property

    ''' <summary>
    ''' RecommendedBy person name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RecommendedBy() As String
        Get
            Return Me.sRecommendedBy
        End Get
        Set(ByVal value As String)
            Me.sRecommendedBy = value
        End Set
    End Property

    ''' <summary>
    ''' Currency Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    ''' <summary>
    ''' will be used for searching and displaying in grid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseNumber() As String
        Get
            Return Me.sCaseNumber
        End Get
        Set(ByVal value As String)
            Me.sCaseNumber = value
        End Set
    End Property
    ''' <summary>
    ''' will be used for searching and displaying in grid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property
    ''' <summary>
    ''' Party Type property is used to displaying in grid.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PartyType() As String
        Get
            Return sPartyType
        End Get
        Set(ByVal value As String)
            Me.sPartyType = value
        End Set
    End Property

    Public Property PaymentAmountBaseCurrency() As Double
        Get
            Return Me.nPaymentAmountBaseCurrency
        End Get
        Set(ByVal value As Double)
            Me.nPaymentAmountBaseCurrency = value
        End Set
    End Property
    Public Property CurrencyId() As Integer
        Get
            Return iCurrencyId
        End Get
        Set(ByVal value As Integer)
            Me.iCurrencyId = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("ClaimPaymentKey : " & iClaimPaymentKey.ToString & "<br />")
        sbPrint.AppendLine("ClaimKey: " & iClaimKey.ToString() & "<br />")
        sbPrint.AppendLine("PolicyNumber : " & sPolicyNumber & "<br />")
        'sbPrint.AppendLine("DateImported : " & dDateImported.ToString & "<br />")
        sbPrint.AppendLine("ClientName : " & sClientName & "<br />")
        sbPrint.AppendLine("PaymentAmount : " & dPaymentAmount.ToString() & "<br />")
        sbPrint.AppendLine("PaymentDate : " & dPaymentDate.ToString() & "<br />")
        sbPrint.AppendLine("CreatedBy : " & sCreatedBy & "<br />")
        sbPrint.AppendLine("Status : " & sStatus & "<br />")
        sbPrint.AppendLine("IsReferredForRecommendation : " & bReferredForRecommendation & "<br />")
        sbPrint.AppendLine("RecommendedBy : " & sRecommendedBy & "<br />")
        sbPrint.AppendLine("CurrencyCode : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("CaseNumber : " & sCaseNumber & "<br />")
        sbPrint.AppendLine("PayeeName : " & sPayeeName & "<br />")
        sbPrint.AppendLine("PartyType : " & sPartyType & "<br />")
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class
''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class CashListItemsCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(CashListItems)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCashListItems As CashListItems In List
            sbPrint.AppendLine(oCashListItems.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCashListItems As CashListItems) As Integer
        Return List.Add(v_oCashListItems)
    End Function

    Public Sub Remove(ByVal v_oCashListItems As CashListItems)
        List.Remove(v_oCashListItems)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CashListItems
        Get
            Return List(i)
        End Get
        Set(ByVal value As CashListItems)
            List(i) = value
        End Set
    End Property
End Class

''' <summary>
''' Will be used for methods named AddCashDeposit, UpdateCashDeposit
''' </summary>
''' <remarks></remarks>
Public Enum ClientAgentType
    '''<remarks/>
    C
    '''<remarks/>
    A
End Enum

''' <summary>
''' Will be used for method named GetCashDepositsForPolicy
''' </summary>
''' <remarks></remarks>
Public Enum CDPartyType
    Client
    Agent
End Enum
