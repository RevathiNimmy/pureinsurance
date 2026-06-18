Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("clsDataGridRow_NET.clsDataGridRow")> _
Public NotInheritable Class clsDataGridRow 

    Private m_iRowIndex As Integer = -1
    Private m_iFrequencyID As Integer = -1
    Private m_iMediaTypeID As Integer = -1
    Private m_sFrequencyDesc As String = ""
    Private m_sMediaTypeDesc As String = ""
    Private m_colCells As New Collection

    Public Property RowIndex() As Integer
        Get
            Return m_iRowIndex
        End Get
        Set(ByVal Value As Integer)
            m_iRowIndex = Value
        End Set
    End Property
    Public Property FrequencyID() As Integer
        Get
            Return m_iFrequencyID
        End Get
        Set(ByVal Value As Integer)
            m_iFrequencyID = Value
        End Set
    End Property
    Public Property MediaTypeID() As Integer
        Get
            Return m_iMediaTypeID
        End Get
        Set(ByVal Value As Integer)
            m_iMediaTypeID = Value
        End Set
    End Property
    Public Property FrequencyDesc() As String
        Get
            Return m_sFrequencyDesc
        End Get
        Set(ByVal Value As String)
            m_sFrequencyDesc = Value
        End Set
    End Property
    Public Property MediaTypeDesc() As String
        Get
            Return m_sMediaTypeDesc
        End Get
        Set(ByVal Value As String)
            m_sMediaTypeDesc = Value
        End Set
    End Property
    Public Property Cells() As Collection
        Get
            Return m_colCells
        End Get
        Set(ByVal Value As Collection)
            m_colCells = Value
        End Set
    End Property
    Public ReadOnly Property Cell(ByVal v_iColumnIndex As Integer) As clsDataGridCell
        Get

            Try
                Return m_colCells.Item(gPMFunctions.ToSafeString(v_iColumnIndex))

            Catch exc As System.Exception
                Return Nothing
            End Try
        End Get
    End Property

    Protected Overrides Sub Finalize()
        m_colCells = Nothing
    End Sub
End Class
