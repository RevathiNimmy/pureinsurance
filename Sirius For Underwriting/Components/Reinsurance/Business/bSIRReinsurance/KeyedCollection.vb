Imports System.Collections.ObjectModel

Public Class ReinsurancesKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewReinsurance As New bSIRReinsurance.Reinsurance
        oNewReinsurance = DirectCast(item, bSIRReinsurance.Reinsurance)
        Return CStr("RI" & oNewReinsurance.RIBand)
    End Function
End Class

