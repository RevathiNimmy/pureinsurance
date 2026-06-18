<Serializable()> Public Class BaseClaimTaxItemType
    Private sRecoveryType As String
    Private sTaxGroupCode As String
    Private sTaxBandCode As String
    Private dPercentage As Decimal
    Private dAmount As Decimal

    Private taxBandIdField As Integer

    Private taxGroupIdField As Integer

    Private sequenceField As Integer

    Private isManuallyChangesField As Integer
    '''<remarks/>
    Public Property TaxBandId() As Integer
        Get
            Return Me.taxBandIdField
        End Get
        Set(ByVal value As Integer)
            Me.taxBandIdField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxGroupId() As Integer
        Get
            Return Me.taxGroupIdField
        End Get
        Set(ByVal value As Integer)
            Me.taxGroupIdField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Sequence() As Integer
        Get
            Return Me.sequenceField
        End Get
        Set(ByVal value As Integer)
            Me.sequenceField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsManuallyChanges() As Integer
        Get
            Return Me.isManuallyChangesField
        End Get
        Set(ByVal value As Integer)
            Me.isManuallyChangesField = value
        End Set
    End Property
    Public Property RecoveryType() As String
        Get
            Return Me.sRecoveryType
        End Get
        Set(ByVal value As String)
            Me.sRecoveryType = value
        End Set
    End Property

    Public Property TaxGroupCode() As String
        Get
            Return Me.sTaxGroupCode
        End Get
        Set(ByVal value As String)
            Me.sTaxGroupCode = value
        End Set
    End Property
    Public Property TaxBandCode() As String
        Get
            Return Me.sTaxBandCode
        End Get
        Set(ByVal value As String)
            Me.sTaxBandCode = value
        End Set
    End Property
    Public Property Percentage() As Decimal
        Get
            Return Me.dPercentage
        End Get
        Set(ByVal value As Decimal)
            Me.dPercentage = value
        End Set
    End Property
    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Claim Key : " & sRecoveryType & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include Totals : " & sTaxGroupCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include TPRecovery : " & sTaxBandCode & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include SalvageRecovery : " & dPercentage.ToString() & "<br />")
        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Include ReserveTypes : " & dAmount.ToString() & "<br />")
        Return sbPrint.ToString()

    End Function
End Class

<Serializable()> Public Class TaxItemTypeCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oTaxItemType As TaxItemType In List
        '    sbPrint.AppendLine(oTaxItemType.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a TaxItemType object to the collection
    ''' </summary>
    ''' <param name="v_oTaxItemType">The TaxItemType object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oTaxItemType As BaseClaimTaxItemType) As Integer
        Return List.Add(v_oTaxItemType)
    End Function

    ''' <summary>
    ''' Remove an TaxItemType object from the collection
    ''' </summary>
    ''' <param name="v_oTaxItemType">The TaxItemType object to be removed</param>
    Public Sub Remove(ByVal v_oTaxItemType As BaseClaimTaxItemType)
        List.Remove(v_oTaxItemType)
    End Sub

    ''' <summary>
    ''' Remove an TaxItemType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the TaxItemType object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an TaxItemType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the TaxItemType object</param>
    ''' <value>The replacement TaxItemType object</value>
    ''' <returns>The TaxItemType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As BaseClaimTaxItemType
        Get
            Return List(i)
        End Get
        Set(ByVal value As BaseClaimTaxItemType)
            List(i) = value
        End Set
    End Property

End Class