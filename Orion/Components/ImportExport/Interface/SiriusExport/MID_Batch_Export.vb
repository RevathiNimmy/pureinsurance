Imports System.Collections.ObjectModel

Public NotInheritable Class MID_Batch_Export : Inherits ExportBase

    Private m_nBatchID As Integer = 0

#Region "Public Property"

    ' Interface name
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "MID_Batch_Export"
        End Get
    End Property

    Public Overrides Property BatchId() As Integer
        Get
            Return m_nBatchID
        End Get
        Set(ByVal value As Integer)
            m_nBatchID = value
        End Set
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub DisplayHelp()
        OutputLine("Example call : - SIRIUSEXPORT MID_Batch_Export")
        OutputLine()
    End Sub

    ''' <summary>
    ''' Process commandline argument - if required
    ''' </summary>
    ''' <param name="cArgs"></param>
    ''' <remarks></remarks>
    Public Overrides Sub ProcessCommandLine(ByVal cArgs As Collection(Of String))
        Dim NoofCommandLineArgs As Integer
        Dim lItem As Integer = 0
        Dim sArg As String
        Dim sArgValues() As String

        ' get the number of command line arguments passed
        NoofCommandLineArgs = cArgs.Count - 1

    End Sub

    ' Process the export
    Public Overrides Sub ProcessExport()

        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.SQLAction("spu_MID_Batch_Export", "MID Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_MID_Batch_Export'")
            End If

        Catch ex As Exception
            Throw New Exception("Unable to create new export batch", ex)
        End Try

    End Sub

    Public Overrides Sub CleanUpInterops()

        ' clean up the database interop
        m_oDatabase = Nothing

    End Sub

#End Region

End Class
