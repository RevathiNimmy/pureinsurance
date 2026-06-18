Imports SharedFiles
Public NotInheritable Class RI2007DisabledPortfolioTransferInterface

    ''' <summary>
    ''' ProcessInterface
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessInterface() As Integer
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue
            frmRI2007DisabledPortfolioTransfer.ShowDialog()

            frmRI2007DisabledPortfolioTransfer = Nothing
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            Return nResult
        End Try

    End Function


End Class
