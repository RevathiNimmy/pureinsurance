Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("clsDataGridRows_NET.clsDataGridRows")> _
Public NotInheritable Class clsDataGridRows 

    Private m_colRows As New Collection

    Public Property Rows() As Collection
        Get
            Return m_colRows
        End Get
        Set(ByVal Value As Collection)
            m_colRows = Value
        End Set
    End Property
    Public ReadOnly Property Row(ByVal v_iRowIndex As Integer) As clsDataGridRow
        Get
            Try
                Return m_colRows(gPMFunctions.ToSafeString(v_iRowIndex))
            Catch exc As System.Exception
                Return Nothing
            End Try
        End Get
    End Property

    Protected Overrides Sub Finalize()
        m_colRows = Nothing
    End Sub
End Class
