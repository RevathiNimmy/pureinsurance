Imports System.Text
<Serializable()> Public Class Locks

#Region "Private Fields"
    Private sLockNameField As String
    Private nLockValueField As Integer
    Private sUserName As String
    Private dtLockedTime As DateTime
    Private nLockedById As Integer
    Private nLock2Value As String
    Private bIsSystemLock As Boolean
    Private nLock3Value As String
    Private bIsExclusiveLock As Boolean
#End Region

    Public Property LockName() As String
        Get
            Return Me.sLockNameField
        End Get
        Set(ByVal value As String)
            sLockNameField = value
        End Set
    End Property

    Public Property LockValue() As Integer
        Get
            Return Me.nLockValueField
        End Get
        Set(ByVal value As Integer)
            nLockValueField = value
        End Set
    End Property

    Public Property LockUserName() As String
        Get
            Return Me.sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
        End Set
    End Property

    Public Property LockedTime() As DateTime
        Get
            Return Me.dtLockedTime
        End Get
        Set(ByVal value As DateTime)
            dtLockedTime = value
        End Set
    End Property


    Public Property IsSystemLock() As Boolean
        Get
            Return Me.bIsSystemLock
        End Get
        Set(ByVal value As Boolean)
            bIsSystemLock = value
        End Set
    End Property

    Public Property IsExclusiveLock() As Boolean
        Get
            Return Me.bIsExclusiveLock
        End Get
        Set(ByVal value As Boolean)
            bIsExclusiveLock = value
        End Set
    End Property

    Public Property SessionID() As String
        Get
            Return Me.nLock3Value
        End Get
        Set(ByVal value As String)
            nLock3Value = value
        End Set
    End Property

    Public Property Lock3Value() As Integer
        Get
            Return Me.nLock3Value
        End Get
        Set(ByVal value As Integer)
            nLock3Value = value
        End Set
    End Property

    Public Property LockedById() As Integer
        Get
            Return Me.nLockedById
        End Get
        Set(ByVal value As Integer)
            nLockedById = value
        End Set
    End Property

End Class

'Make changes in this class to Convert it to sortable collection
<Serializable()> Public Class LockCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(Locks)
    End Sub

    ''' <summary>
    ''' Add Finance Plan List
    ''' </summary>
    ''' <param name="oClaimLock"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal oClaimLock As Locks) As Integer
        Return List.Add(oClaimLock)
    End Function

    ''' <summary>
    ''' Remove Claim Lock List
    ''' </summary>
    ''' <param name="oClaimLock"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal oClaimLock As Locks)
        List.Remove(oClaimLock)
    End Sub

    ''' <summary>
    ''' Remove Claim Lock List at a particular Index
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Shadows Sub RemoveAt(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Add Item Claim Lock Item List
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As Locks
        Get
            Return List(i)
        End Get
        Set(ByVal value As Locks)
            List(i) = value
        End Set
    End Property

    Public ReadOnly Property Length() As Integer
        Get
            Return List.Count
        End Get

    End Property

End Class

