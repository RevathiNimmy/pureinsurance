Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("clsDataGridCell_NET.clsDataGridCell")> _
Public NotInheritable Class clsDataGridCell 

    Private m_sDataDisplayValue As String = ""
    Private m_iOriginatingQuoteNo As Integer = -1
    Private m_iFrequencyID As Integer = -1
    Private m_iMediaTypeID As Integer = -1
    Private m_iRowIndex As Integer
    Private m_iColumnIndex As Integer

    Public Property DataDisplayValue() As String
        Get
            Return m_sDataDisplayValue
        End Get
        Set(ByVal Value As String)
            m_sDataDisplayValue = Value
        End Set
    End Property
    Public Property OriginatingQuoteNo() As Integer
        Get
            Return m_iOriginatingQuoteNo
        End Get
        Set(ByVal Value As Integer)
            m_iOriginatingQuoteNo = Value
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
    Public Property RowIndex() As Integer
        Get
            Return m_iRowIndex
        End Get
        Set(ByVal Value As Integer)
            m_iRowIndex = Value
        End Set
    End Property
    Public Property ColumnIndex() As Integer
        Get
            Return m_iColumnIndex
        End Get
        Set(ByVal Value As Integer)
            m_iColumnIndex = Value
        End Set
    End Property

End Class
