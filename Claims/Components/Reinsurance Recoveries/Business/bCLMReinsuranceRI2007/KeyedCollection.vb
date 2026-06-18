Imports System.Collections.ObjectModel

Public Class ReinsurancesKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewReinsurance As New bCLMReinsuranceRI2007.Reinsurance
        oNewReinsurance = DirectCast(item, bCLMReinsuranceRI2007.Reinsurance)
        Return If(oNewReinsurance Is Nothing, Nothing, "RI" & oNewReinsurance.RIBand)
    End Function
End Class