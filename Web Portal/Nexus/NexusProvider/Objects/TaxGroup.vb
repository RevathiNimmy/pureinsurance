<Serializable()> Public Class TaxGroup

    Private iTaxGroupKey As Integer

    Private sDescription As String

    Private sCode As String

    Private bIsWithHoldingTax As Boolean

    Private sAdvanceTaxScript As String

    '''<remarks/>
    Public Property TaxGroupKey() As Integer
        Get
            Return Me.iTaxGroupKey
        End Get
        Set(ByVal value As Integer)
            Me.iTaxGroupKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsWithHoldingTax() As Boolean
        Get
            Return Me.bIsWithHoldingTax
        End Get
        Set(ByVal value As Boolean)
            Me.bIsWithHoldingTax = value
        End Set
    End Property

    '''<remarks/>
    Public Property AdvanceTaxScript() As String
        Get
            Return Me.sAdvanceTaxScript
        End Get
        Set(ByVal value As String)
            Me.sAdvanceTaxScript = value
        End Set
    End Property

    Public Property IsTaxAmountEditable() As Boolean

End Class
<Serializable()> Public Class TaxGroupCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oTaxGroup As TaxGroup In List
            'sbPrint.AppendLine(oTaxGroup.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oTaxGroup As TaxGroup) As Integer
        Return List.Add(v_oTaxGroup)
    End Function

    Public Sub Remove(ByVal v_oTaxGroup As TaxGroup)
        List.Remove(v_oTaxGroup)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

	Default Public Property Item(ByVal i As Integer) As TaxGroup
		Get
			Return List(i)
		End Get
		Set(ByVal value As TaxGroup)
			List(i) = value
		End Set
	End Property

	Public Function FindCodeByKey(ByVal v_iKey As Integer) As String
		For Each oItem As TaxGroup In List
			If oItem.TaxGroupKey = v_iKey Then
				Return oItem.Code
			End If
		Next

		Return Nothing
	End Function

End Class