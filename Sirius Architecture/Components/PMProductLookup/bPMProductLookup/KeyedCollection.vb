Imports System.Collections.ObjectModel

Public Class PMProductLookupKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewPMProductLookup As New PMProductLookup
        oNewPMProductLookup = DirectCast(item, PMProductLookup)
        Return CStr(oNewPMProductLookup.PMProductID) & oNewPMProductLookup.TableName
    End Function
End Class

