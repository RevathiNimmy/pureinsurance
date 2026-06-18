Imports System.Collections.ObjectModel

Public Class FieldManagerKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewCollection As New Field
        oNewCollection = DirectCast(item, Field)
        Return If(oNewCollection Is Nothing, Nothing, oNewCollection.Key)
    End Function

End Class

Public Class FieldManagerDpmDaoKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewCollection As New bSIRFieldManager.PMDAOInstance()
        oNewCollection = DirectCast(item, bSIRFieldManager.PMDAOInstance)
        Return If(oNewCollection Is Nothing, Nothing, "K" & oNewCollection.Family.ToString().Trim())
    End Function

End Class
Public Class ResultManagerKeyedCollection
    Inherits KeyedCollection(Of String, Object)

    Protected Overrides Function GetKeyForItem(item As Object) As String
        Dim oNewCollection As New Result
        oNewCollection = DirectCast(item, Result)
        Return If(oNewCollection Is Nothing, Nothing, oNewCollection.Key)
    End Function

End Class

