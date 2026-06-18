Imports System.Collections.ObjectModel
Public Class PMUserSourcedKeyedCollection
    Inherits KeyedCollection(Of String, PMUserSource)
    Protected Overrides Function GetKeyForItem(oPMUserSource As PMUserSource) As String
        Return If(oPMUserSource Is Nothing, Nothing, "K" & oPMUserSource.SourceID)
    End Function
End Class
