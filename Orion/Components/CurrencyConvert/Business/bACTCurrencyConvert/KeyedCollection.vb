Imports System.Collections.ObjectModel

Public Class DecimalKeyedCollection
    Inherits KeyedCollection(Of String, Integer)

    Private v_lCurrencyID As String

    Public Sub New(v_lCurrencyID As String)
        Me.v_lCurrencyID = v_lCurrencyID
    End Sub
    Protected Overrides Function GetKeyForItem(item As Integer) As String
        Return v_lCurrencyID
    End Function
End Class
Public Class FormatStringKeyedCollection
    Inherits KeyedCollection(Of String, String)

    Private v_lCurrencyID As String

    Public Sub New(v_lCurrencyID As String)
        Me.v_lCurrencyID = v_lCurrencyID
    End Sub
    Protected Overrides Function GetKeyForItem(item As String) As String
        Return v_lCurrencyID
    End Function
End Class
