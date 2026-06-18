Imports Microsoft.Web.Services3.Security.Tokens
''' <summary>
''' Nexus BasePerilClaim object, containing the common elements between the various perilclaim.
''' </summary>
''' <remarks></remarks>
''' 
<Serializable()> Public Class PerilSummary

    Private iClaimKey As Integer
    Private bIncludeTotals As Boolean
    Private bIncludeTPRecovery As Boolean
    Private bIncludeSalvageRecovery As Boolean
    Private bIncludeReserveTypes As Boolean
    Private dAverage As Decimal
    Private dCurrentReserve As Decimal
    Private sDescription As String
    Private dInitialReserve As Decimal
    Private dPaidAmount As Decimal
    Private dRevisedReserve As Decimal
    Private dSumInsured As Decimal
    'Newly Added property for RunDefaultRulesEdit method
    Private iClaimPerilKey As Integer

    'Newly Added property for GetClaimDetails screen
    Private iBaseClaimPerilKey As Integer
    Private sTypeCodeField As String
    Private sRIBandField As String
    Private sGisScreenCodeField As String
    Private iRiskKey As Integer
    ' Private oReserveType As ReserveTypeCollection
    Private oPeril As PerilCollection
    Private oSalvageRecovery As PerilRecoveryCollection
    Private oTPRecovery As PerilRecoveryCollection
    Private oReserve As ReserveCollection
    Private oRecovery As RecoveryCollection
    Private oClaimPayment As ClaimPaymentCollection
    Private oPayment As ClaimPayment
    Private oRecoveryType As RecoveryTypeCollection
    Dim oReserveType As ReserveTypeCollection
    Private oReceiptColl As ClaimReceiptCollection
    Private oReceipt As ClaimReceipt
    Private oClaimPerilReservePaymentType As ClaimPerilReservePaymentTypeCollection
    Private bPerilEdited As Boolean
    Private dTotalPaidAmount As Decimal

    Public Sub New()
        'initialize the collection
        oPeril = New PerilCollection
        oSalvageRecovery = New PerilRecoveryCollection
        oTPRecovery = New PerilRecoveryCollection
        oReserve = New ReserveCollection
        oRecovery = New RecoveryCollection
        oReserveType = New ReserveTypeCollection
        oClaimPayment = New ClaimPaymentCollection
        oPayment = New ClaimPayment
        oReceipt = New ClaimReceipt
        oReceiptColl = New ClaimReceiptCollection
        oClaimPerilReservePaymentType = New ClaimPerilReservePaymentTypeCollection
    End Sub
    Public Property TotalPaidAmount() As Decimal
        Get
            Return Me.dTotalPaidAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalPaidAmount = value
        End Set
    End Property
    Public Property PerilEdited() As Boolean
        Get
            Return Me.bPerilEdited
        End Get
        Set(ByVal value As Boolean)
            Me.bPerilEdited = value
        End Set
    End Property

    Public Property ClaimReserve() As ClaimPerilReservePaymentTypeCollection
        Get
            Return Me.oClaimPerilReservePaymentType
        End Get
        Set(ByVal value As ClaimPerilReservePaymentTypeCollection)
            Me.oClaimPerilReservePaymentType = value
        End Set
    End Property
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    Public Property IncludeTotals() As Boolean
        Get
            Return Me.bIncludeTotals
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeTotals = value
        End Set
    End Property

    Public Property IncludeTPRecovery() As Boolean
        Get
            Return Me.bIncludeTPRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeTPRecovery = value
        End Set
    End Property

    Public Property IncludeSalvageRecovery() As Boolean
        Get
            Return Me.bIncludeSalvageRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeSalvageRecovery = value
        End Set
    End Property

    Public Property IncludeReserveTypes() As Boolean
        Get
            Return Me.bIncludeReserveTypes
        End Get
        Set(ByVal value As Boolean)
            Me.bIncludeReserveTypes = value
        End Set
    End Property

    Public Property Average() As Decimal
        Get
            Return Me.dAverage
        End Get
        Set(ByVal value As Decimal)
            Me.dAverage = value
        End Set
    End Property

    Public Property CurrentReserve() As Decimal
        Get
            Return Me.dCurrentReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrentReserve = value
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

    Public Property InitialReserve() As Decimal
        Get
            Return Me.dInitialReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dInitialReserve = value
        End Set
    End Property

    Public Property PaidAmount() As Decimal
        Get
            Return Me.dPaidAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidAmount = value
        End Set
    End Property

    Public Property RevisedReserve() As Decimal
        Get
            Return Me.dRevisedReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisedReserve = value
        End Set
    End Property

    Public Property SumInsured() As Decimal
        Get
            Return Me.dSumInsured
        End Get
        Set(ByVal value As Decimal)
            Me.dSumInsured = value
        End Set
    End Property
    'Newly Added property for RunDefaultRulesEdit method

    Public Property ClaimPerilKey() As Integer
        Get
            Return Me.iClaimPerilKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimPerilKey = value
        End Set
    End Property

    'Newly added property for GetClaimDetails screen
    Public Property BaseClaimPerilKey() As Integer
        Get
            Return Me.iBaseClaimPerilKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimPerilKey = value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return Me.sTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sTypeCodeField = value
        End Set
    End Property

    Public Property RIBand() As String
        Get
            Return Me.sRIBandField
        End Get
        Set(ByVal value As String)
            Me.sRIBandField = value
        End Set
    End Property

    Public Property RiskKey() As Integer
        Get
            Return Me.iRiskKey
        End Get
        Set(ByVal value As Integer)
            Me.iRiskKey = value
        End Set
    End Property

    Public Property GisScreenCode() As String
        Get
            Return Me.sGisScreenCodeField
        End Get
        Set(ByVal value As String)
            Me.sGisScreenCodeField = value
        End Set
    End Property

    Public Property ReserveType() As ReserveTypeCollection
        Get
            Return Me.oReserveType
        End Get
        Set(ByVal value As ReserveTypeCollection)
            Me.oReserveType = value
        End Set
    End Property
    Public Property PerilTotals() As PerilCollection
        Get
            Return Me.oPeril
        End Get
        Set(ByVal value As PerilCollection)
            Me.oPeril = value
        End Set
    End Property
    Public Property SalvageRecovery() As PerilRecoveryCollection
        Get
            Return Me.oSalvageRecovery
        End Get
        Set(ByVal value As PerilRecoveryCollection)
            Me.oSalvageRecovery = value
        End Set
    End Property

    Public Property TPRecovery() As PerilRecoveryCollection
        Get
            Return Me.oTPRecovery
        End Get
        Set(ByVal value As PerilRecoveryCollection)
            Me.oTPRecovery = value
        End Set
    End Property
    Public Property Reserve() As ReserveCollection
        Get
            Return Me.oReserve
        End Get
        Set(ByVal value As ReserveCollection)
            Me.oReserve = value
        End Set
    End Property
    Public Property Recovery() As RecoveryCollection
        Get
            Return Me.oRecovery
        End Get
        Set(ByVal value As RecoveryCollection)
            Me.oRecovery = value
        End Set
    End Property

    Public Property RecoveryType() As RecoveryTypeCollection
        Get
            Return Me.oRecoveryType
        End Get
        Set(ByVal value As RecoveryTypeCollection)
            Me.oRecoveryType = value
        End Set
    End Property
    Public Property ClaimPayment() As ClaimPaymentCollection
        Get
            Return Me.oClaimPayment
        End Get
        Set(ByVal value As ClaimPaymentCollection)
            Me.oClaimPayment = value
        End Set
    End Property

    Public Property Payment() As ClaimPayment
        Get
            Return Me.oPayment
        End Get
        Set(ByVal value As ClaimPayment)
            Me.oPayment = value
        End Set
    End Property

    Public Property Receipt() As ClaimReceipt
        Get
            Return Me.oReceipt
        End Get
        Set(ByVal value As ClaimReceipt)
            Me.oReceipt = value
        End Set
    End Property

    Public Property ClaimReceipt() As ClaimReceiptCollection
        Get
            Return Me.oReceiptColl
        End Get
        Set(ByVal value As ClaimReceiptCollection)
            Me.oReceiptColl = value
        End Set
    End Property

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Claim Key : " & iClaimKey.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include Totals : " & bIncludeTotals & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include TPRecovery : " & bIncludeTPRecovery & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include SalvageRecovery : " & bIncludeSalvageRecovery & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include ReserveTypes : " & bIncludeReserveTypes & "<br />")
        Return sbPrint.ToString()

    End Function

End Class
<Serializable()> Public Class PerilCollection : Inherits SortableCollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each v_oPeril As PerilSummary In List
            sbPrint.AppendLine(v_oPeril.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Document object to the collection
    ''' </summary>
    ''' <param name="v_oPeril">The Document object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oPeril As PerilSummary) As Integer
        Return List.Add(v_oPeril)
    End Function

    ''' <summary>
    ''' Remove an Document object from the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oPeril As PerilSummary)
        List.Remove(v_oPeril)
    End Sub

    ''' <summary>
    ''' Remove an Document object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Document object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Document object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement Document object</value>
    ''' <returns>The Document object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As PerilSummary
        Get
            Return List(i)
        End Get
        Set(ByVal value As PerilSummary)
            List(i) = value
        End Set
    End Property

End Class
<Serializable()> Public Class Reserve
#Region "Private Fields"
    Private sTypeCodeField As String

    Private sDescription As String

    Private dRevisionAmountField As Decimal

    Private iBaseReserveKeyField As Integer

    Private dSumInsuredField As Decimal

    Private dInitialReserveField As Decimal

    Private dRevisedReserveField As Decimal

    Private dPaidAmountField As Decimal

    Private bIsExcessField As Boolean

    Private bIsIndemnityField As Boolean

    Private bIsExpenseField As Boolean
    Private dCurrentReserve As Decimal
    Private dAverage As Decimal
    Private bReserveEdited As Boolean
    Private bIsSalvage As Boolean
    Private dReceiptedAmountField As Decimal
#End Region

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()

    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("TypeCodeField : " & sTypeCodeField & "<br />")
        sbPrint.AppendLine("RevisionAmountField: " & dRevisionAmountField & "<br />")
        Return sbPrint.ToString

    End Function

#Region "Properties"
    Public Property IsSalvage() As Boolean
        Get
            Return Me.bIsSalvage
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSalvage = value
        End Set
    End Property
    Public Property ReserveEdited() As Boolean
        Get
            Return Me.bReserveEdited
        End Get
        Set(ByVal value As Boolean)
            Me.bReserveEdited = value
        End Set
    End Property
    Public Property Average() As Decimal
        Get
            Return Me.dAverage
        End Get
        Set(ByVal value As Decimal)
            Me.dAverage = value
        End Set
    End Property
    Public Property CurrentReserve() As Decimal
        Get
            Return Me.dCurrentReserve
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrentReserve = value
        End Set
    End Property

    '''<remarks/>
    ''' 
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sTypeCodeField = value
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

    '''<remarks/>
    Public Property RevisionAmount() As Decimal
        Get
            Return Me.dRevisionAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisionAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseReserveKey() As Integer
        Get
            Return Me.iBaseReserveKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseReserveKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property SumInsured() As Decimal
        Get
            Return Me.dSumInsuredField
        End Get
        Set(ByVal value As Decimal)
            Me.dSumInsuredField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InitialReserve() As Decimal
        Get
            Return Me.dInitialReserveField
        End Get
        Set(ByVal value As Decimal)
            Me.dInitialReserveField = value
        End Set
    End Property

    '''<remarks/>
    Public Property RevisedReserve() As Decimal
        Get
            Return Me.dRevisedReserveField
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisedReserveField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaidAmount() As Decimal
        Get
            Return Me.dPaidAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dPaidAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsExcess() As Boolean
        Get
            Return Me.bIsExcessField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExcessField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsIndemnity() As Boolean
        Get
            Return Me.bIsIndemnityField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsIndemnityField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsExpense() As Boolean
        Get
            Return Me.bIsExpenseField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExpenseField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReceiptedAmount() As Decimal
        Get
            Return Me.dReceiptedAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptedAmountField = value
        End Set
    End Property

    Public Property TypeDescription As String

    Public Property GrossReserve As Decimal

    Public Property Tax As Decimal

    Public Property RevisedGrossReserve As Decimal

    Public Property RevisedTaxReserve As Decimal

    Public Property PaidToDateTax As Decimal

#End Region

End Class
''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class ReserveCollection : Inherits SortableCollectionBase

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
    ''' Add a Document object to the collection
    ''' </summary>

    ''' <returns></returns>
    Public Function Add(ByVal v_oReserve As Reserve) As Integer
        Return List.Add(v_oReserve)
    End Function

    ''' <summary>
    ''' Remove an Document object from the collection
    ''' </summary>
    ''' <param name="v_oDocument">The Document object to be removed</param>
    Public Sub Remove(ByVal v_oReserve As Reserve)
        List.Remove(v_oReserve)
    End Sub

    ''' <summary>
    ''' Remove an Document object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Document object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Document object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement Document object</value>
    ''' <returns>The Document object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Reserve
        Get
            Return List(i)
        End Get
        Set(ByVal value As Reserve)
            List(i) = value
        End Set
    End Property
    
End Class

<Serializable()> Public Class Recovery
#Region "Private Fields"
    Private sTypeCodeField As String

    Private dRevisionAmountField As Decimal

    Private iBaseRecoveryKeyField As Integer

    Private sCurrencyCodeField As String

    Private dInitialRecoveryField As Decimal

    Private dRevisedRecoveryField As Decimal

    Private dReceiptedAmountField As Decimal

    Private dReceiptedTaxAmountField As Decimal

    Private iIsSalvageField As Integer
    
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overridable Function Print() As String
    Public Overridable Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("TypeCodeField : " & sTypeCodeField & "<br />")
        sbPrint.AppendLine("RevisionAmountField: " & dRevisionAmountField & "<br />")
        Return sbPrint.ToString

    End Function

#Region "Properties"
    '''<remarks/>
    Public Property TypeCode() As String
        Get
            Return Me.sTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sTypeCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property RevisionAmount() As Decimal
        Get
            Return Me.dRevisionAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisionAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseRecoveryKey() As Integer
        Get
            Return Me.iBaseRecoveryKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseRecoveryKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCodeField
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InitialRecovery() As Decimal
        Get
            Return Me.dInitialRecoveryField
        End Get
        Set(ByVal value As Decimal)
            Me.dInitialRecoveryField = value
        End Set
    End Property

    '''<remarks/>
    Public Property RevisedRecovery() As Decimal
        Get
            Return Me.dRevisedRecoveryField
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisedRecoveryField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReceiptedAmount() As Decimal
        Get
            Return Me.dReceiptedAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptedAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReceiptedTaxAmount() As Decimal
        Get
            Return Me.dReceiptedTaxAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptedTaxAmountField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsSalvage() As Integer
        Get
            Return Me.iIsSalvageField
        End Get
        Set(ByVal value As Integer)
            Me.iIsSalvageField = value
        End Set
    End Property

#End Region
End Class

<Serializable()> Public Class RecoveryCollection : Inherits CollectionBase

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
    ''' Add a Document object to the collection
    ''' </summary>

    ''' <returns></returns>
    Public Function Add(ByVal v_oRecovery As Recovery) As Integer
        Return List.Add(v_oRecovery)
    End Function

    ''' <summary>
    ''' Remove an Document object from the collection
    ''' </summary>

    Public Sub Remove(ByVal v_oRecovery As Recovery)
        List.Remove(v_oRecovery)
    End Sub

    ''' <summary>
    ''' Remove an Document object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Document object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Document object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Document object</param>
    ''' <value>The replacement Document object</value>
    ''' <returns>The Document object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Recovery
        Get
            Return List(i)
        End Get
        Set(ByVal value As Recovery)
            List(i) = value
        End Set
    End Property

End Class