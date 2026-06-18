Imports Microsoft.Office.Interop
Module UpgradeSupport

    Private _Word_Global_definst As Word.Global = Nothing
    Public ReadOnly Property Word_Global_definst() As Word.Global
        Get
            If _Word_Global_definst Is Nothing Then
                _Word_Global_definst = New Word.Global
            End If
            Return _Word_Global_definst
        End Get
    End Property
End Module