Imports Microsoft.Web.Services3.Security.Tokens
''' <summary>
''' Nexus BasePerilClaim object, containing the common elements between the various perilclaim.
''' </summary>
''' <remarks></remarks>
''' 
<Serializable()> Public Class PerilRecovery

    Private dCurrentRecovery As Decimal
    Private sDescription As String
    Private dInitialRecovery As Decimal
    Private dReceiptedAmount As Decimal
    Private dRevisedRecovery As Decimal
    Private dRevisionAmount As Decimal
    Private bIsSalvage As Integer
    Private sTypeCode As String
    Private iBaseRecoveryKey As Integer
    Private sCurrencyCode As String
    Private dReceiptedTaxAmount As Decimal
    Private iKey As String
    'Salvage Work
    Private dTotalRecovery As Decimal
    Private dThisReceiptINCLTax, dLossThisReceiptINCLTax As Decimal
    Private dThisReceiptTax, dLossThisReceiptTax As Decimal
    Private dThisNet, dLossThisNet As Decimal
    Private dCostToClaim, dLossAmount, dBaseAmount As Decimal
    Private oReceiptPartyType As ClaimReceiptPartyTypeType
    Private sPartyReceiptCode, sPartyReceiptName, sCurrencyDescription As String
    Private bIsLocked, bIsDeleted, bCanDelete As Boolean
    Private oPayee As Payee
    Private bIsNew As Boolean
    Private iReceiptQueue As Integer
    Private nClaimPerilId As Integer
    Private iRecoveryPartyTypeId As Integer
    Private iRecoveryPartyKey As Integer
    Private sPartyShortName As String
    Private sRecoveryPartyTypeCode As String
    Public Sub New()
        oPayee = New Payee
    End Sub
    Public Property ReceiptQueue() As Integer
        Get
            Return Me.iReceiptQueue
        End Get
        Set(ByVal value As Integer)
            Me.iReceiptQueue = value
        End Set
    End Property
    Public Property IsNew() As Boolean
        Get
            Return Me.bIsNew
        End Get
        Set(ByVal value As Boolean)
            Me.bIsNew = value
        End Set
    End Property
    Public Property CanDelete() As Boolean
        Get
            Return Me.bCanDelete
        End Get
        Set(ByVal value As Boolean)
            Me.bCanDelete = value
        End Set
    End Property
    Public Property IsDeleted() As Boolean
        Get
            Return Me.bIsDeleted
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDeleted = value
        End Set
    End Property
    Public Property Payee() As Payee
        Get
            Return Me.oPayee
        End Get
        Set(ByVal value As Payee)
            Me.oPayee = value
        End Set
    End Property
    Public Property BaseAmount() As Decimal
        Get
            Return Me.dBaseAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dBaseAmount = value
        End Set
    End Property
    Public Property LossAmount() As Decimal
        Get
            Return Me.dLossAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dLossAmount = value
        End Set
    End Property
    Public Property IsLocked() As Boolean
        Get
            Return Me.bIsLocked
        End Get
        Set(ByVal value As Boolean)
            Me.bIsLocked = value
        End Set
    End Property
    Public Property CurrencyDescription() As String
        Get
            Return Me.sCurrencyDescription
        End Get
        Set(ByVal value As String)
            Me.sCurrencyDescription = value
        End Set
    End Property
    Public Property PartyReceiptName() As String
        Get
            Return Me.sPartyReceiptName
        End Get
        Set(ByVal value As String)
            Me.sPartyReceiptName = value
        End Set
    End Property
    Public Property PartyReceiptCode() As String
        Get
            Return Me.sPartyReceiptCode
        End Get
        Set(ByVal value As String)
            Me.sPartyReceiptCode = value
        End Set
    End Property
    Public Property ReceiptPartyType() As ClaimReceiptPartyTypeType
        Get
            Return Me.oReceiptPartyType
        End Get
        Set(ByVal value As ClaimReceiptPartyTypeType)
            Me.oReceiptPartyType = value
        End Set
    End Property
    Public Property LossThisNet() As Decimal
        Get
            Return Me.dLossThisNet
        End Get
        Set(ByVal value As Decimal)
            Me.dLossThisNet = value
        End Set
    End Property
    Public Property ThisNet() As Decimal
        Get
            Return Me.dThisNet
        End Get
        Set(ByVal value As Decimal)
            Me.dThisNet = value
        End Set
    End Property
    Public Property CostToClaim() As Decimal
        Get
            Return Me.dCostToClaim
        End Get
        Set(ByVal value As Decimal)
            Me.dCostToClaim = value
        End Set
    End Property
    Public Property LossThisReceiptTax() As Decimal
        Get
            Return Me.dLossThisReceiptTax
        End Get
        Set(ByVal value As Decimal)
            Me.dLossThisReceiptTax = value
        End Set
    End Property
    Public Property ThisReceiptTax() As Decimal
        Get
            Return Me.dThisReceiptTax
        End Get
        Set(ByVal value As Decimal)
            Me.dThisReceiptTax = value
        End Set
    End Property
    Public Property LossThisReceiptINCLTax() As Decimal
        Get
            Return Me.dLossThisReceiptINCLTax
        End Get
        Set(ByVal value As Decimal)
            Me.dLossThisReceiptINCLTax = value
        End Set
    End Property
    Public Property ThisReceiptINCLTax() As Decimal
        Get
            Return Me.dThisReceiptINCLTax
        End Get
        Set(ByVal value As Decimal)
            Me.dThisReceiptINCLTax = value
        End Set
    End Property
    Public Property TotalRecovery() As Decimal
        Get
            Return Me.dTotalRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalRecovery = value
        End Set
    End Property
    Public Property CurrentRecovery() As Decimal
        Get
            Return Me.dCurrentRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dCurrentRecovery = value
        End Set
    End Property

    Public Property Key() As String
        Get
            Return Me.iKey
        End Get
        Set(ByVal value As String)
            Me.iKey = value
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

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    Public Property InitialRecovery() As Decimal
        Get
            Return Me.dInitialRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dInitialRecovery = value
        End Set
    End Property

    Public Property ReceiptedAmount() As Decimal
        Get
            Return Me.dReceiptedAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptedAmount = value
        End Set
    End Property

    Public Property RevisionAmount() As Decimal
        Get
            Return Me.dRevisionAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisionAmount = value
        End Set
    End Property

    Public Property RevisedRecovery() As Decimal
        Get
            Return Me.dRevisedRecovery
        End Get
        Set(ByVal value As Decimal)
            Me.dRevisedRecovery = value
        End Set
    End Property

    Public Property IsSalvage() As Integer
        Get
            Return Me.bIsSalvage
        End Get
        Set(ByVal value As Integer)
            Me.bIsSalvage = value
        End Set
    End Property

    Public Property BaseRecoveryKey() As Integer
        Get
            Return Me.iBaseRecoveryKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseRecoveryKey = value
        End Set
    End Property
    Public Property CurrencyCode() As String
        Get
            Return Me.sCurrencyCode
        End Get
        Set(ByVal value As String)
            Me.sCurrencyCode = value
        End Set
    End Property
    Public Property ReceiptedTaxAmount() As Decimal
        Get
            Return Me.dReceiptedTaxAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dReceiptedTaxAmount = value
        End Set
    End Property

    Public Property ClaimPerilId() As Integer
        Get
            Return Me.nClaimPerilId
        End Get
        Set(ByVal value As Integer)
            Me.nClaimPerilId = value
        End Set
    End Property

    Public Property RecoveryPartyTypeId() As Integer
        Get
            Return Me.iRecoveryPartyTypeId
        End Get
        Set(ByVal value As Integer)
            Me.iRecoveryPartyTypeId = value
        End Set
    End Property

    Public Property RecoveryPartyKey() As Integer
        Get
            Return Me.iRecoveryPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iRecoveryPartyKey = value
        End Set
    End Property

    Public Property PartyShortName() As String
        Get
            Return Me.sPartyShortName
        End Get
        Set(ByVal value As String)
            Me.sPartyShortName = value
        End Set
    End Property

    Public Property RecoveryPartyTypeCode() As String
        Get
            Return Me.sRecoveryPartyTypeCode
        End Get
        Set(ByVal value As String)
            Me.sRecoveryPartyTypeCode = value
        End Set
    End Property

End Class

<Serializable()> Public Class PerilRecoveryCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(PerilRecovery)
    End Sub
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
    ''' Add a PerilRecovery object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oPerilRecovery As PerilRecovery) As Integer
        v_oPerilRecovery.Key = List.Add(v_oPerilRecovery)
        Return v_oPerilRecovery.Key
    End Function

    ''' <summary>
    ''' Updates PerilRecovery object to the collection
    ''' </summary>
    ''' <param name="v_oPerilRecovery"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oPerilRecovery As PerilRecovery)
        List.Item(v_oPerilRecovery.key) = v_oPerilRecovery
    End Sub

    ''' <summary>
    ''' Remove an PerilRecovery object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oPerilRecovery As PerilRecovery)
        List.Remove(v_oPerilRecovery)
    End Sub

    ''' <summary>
    ''' Remove an PerilRecovery object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the PerilRecovery object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an PerilRecovery object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the PerilRecovery object</param>
    ''' <value>The replacement PerilRecovery object</value>
    ''' <returns>The PerilRecovery object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As PerilRecovery
        Get
            Return List(i)
        End Get
        Set(ByVal value As PerilRecovery)
            List(i) = value
        End Set
    End Property

End Class
