Imports System.Collections.ObjectModel

Public Class ReinsurancesKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewReinsurance As New bCLMReinsurance.Reinsurance
        oNewReinsurance = DirectCast(item, bCLMReinsurance.Reinsurance)
        Return If(oNewReinsurance Is Nothing, Nothing, "RI" & oNewReinsurance.RIBand)
    End Function
End Class