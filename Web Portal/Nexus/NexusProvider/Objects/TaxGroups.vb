<Serializable()> Public Class TaxGroups
    Private iPartyKey As Integer
    Private oPartyType As ClaimReceiptPartyTypeType
    Private oPaymentAdvancedTax As PaymentAdvancedTaxDetails

    Private sCode As String
    Private sDecription As String
    Private bIsWithHoldingTax As Boolean


    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    Public Property PartyType() As ClaimReceiptPartyTypeType
        Get
            Return Me.oPartyType
        End Get
        Set(ByVal value As ClaimReceiptPartyTypeType)
            Me.oPartyType = value
        End Set
    End Property

    Public Property PaymentAdvancedTax() As PaymentAdvancedTaxDetails
        Get
            Return Me.oPaymentAdvancedTax
        End Get
        Set(ByVal value As PaymentAdvancedTaxDetails)
            Me.oPaymentAdvancedTax = value
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
            Return Me.sDecription
        End Get
        Set(ByVal value As String)
            Me.sDecription = value
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

End Class

<Serializable()> Public Class TaxGroupsCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oTaxGroups As TaxGroups In List
        '    sbPrint.AppendLine(oTaxGroups.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a TaxGroups object to the collection
    ''' </summary>
    ''' <param name="v_oTaxGroups">The TaxGroups object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oTaxGroups As TaxGroups) As Integer
        Return List.Add(v_oTaxGroups)
    End Function

    ''' <summary>
    ''' Remove an TaxGroups object from the collection
    ''' </summary>
    ''' <param name="v_oTaxGroups">The TaxGroups object to be removed</param>
    Public Sub Remove(ByVal v_oTaxGroups As TaxGroups)
        List.Remove(v_oTaxGroups)
    End Sub

    ''' <summary>
    ''' Remove an TaxGroups object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the TaxGroups object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an TaxGroups object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the TaxGroups object</param>
    ''' <value>The replacement TaxGroups object</value>
    ''' <returns>The TaxGroups object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As TaxGroups
        Get
            Return List(i)
        End Get
        Set(ByVal value As TaxGroups)
            List(i) = value
        End Set
    End Property

End Class
