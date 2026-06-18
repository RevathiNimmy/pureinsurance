Option Strict On

Public NotInheritable Class Security

#Region " Public Functions"

    Public Shared Function ComputeSiriusLegacyHash(ByVal sData As String) As String

        Return SiriusLegacyHash.ComputeHash(sData)

    End Function

#End Region

End Class



