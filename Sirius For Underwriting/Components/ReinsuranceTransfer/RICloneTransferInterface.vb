Imports SharedFiles
Public NotInheritable Class RICloneTransferInterface 
    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessInterface() As Integer
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue

            frmCloneRITransfer.ShowDialog()

            frmCloneRITransfer = Nothing
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            Return nResult

        End Try

    End Function

End Class
