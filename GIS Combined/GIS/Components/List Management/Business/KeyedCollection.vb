Imports System.Collections.ObjectModel

Public Class cFilesKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Public ClientListFilePathDat As String

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Return ClientListFilePathDat
    End Function
End Class

Public Class cIndexKeyedCollection
    Inherits KeyedCollection(Of String, Integer)

    Public PropertyId As String

    Protected Overrides Function GetKeyForItem(item As Integer) As String
        Return PropertyId
    End Function
End Class

