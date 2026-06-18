<Serializable()> Public Class WorkManagerFilter

    Dim sSortDirection As String
    Dim sSortExpression As String
    Dim sPageIndex As String
    Dim sDropDownDateValue As String
    Dim sDropDownTaskStatusValue As String
    Dim sDropDownShowTypeValue As String
    Dim sDropDownUserGroupsValue As String
    Dim sDropDownUsersValue As String
    Dim sDropDownPartyValue As String
    Dim sReferenceNumber As String

    '''<remarks/>
    Public Property SortDirection() As String
        Get
            Return Me.sSortDirection
        End Get
        Set(ByVal value As String)
            Me.sSortDirection = value
        End Set
    End Property

    '''<remarks/>
    Public Property SortExpression() As String
        Get
            Return Me.sSortExpression
        End Get
        Set(ByVal value As String)
            Me.sSortExpression = value
        End Set
    End Property

    '''<remarks/>
    Public Property DropDownDateValue() As String
        Get
            Return Me.sDropDownDateValue
        End Get
        Set(ByVal value As String)
            Me.sDropDownDateValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property DropDownTaskStatusValue() As String
        Get
            Return Me.sDropDownTaskStatusValue
        End Get
        Set(ByVal value As String)
            Me.sDropDownTaskStatusValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property DropDownShowTypeValue() As String
        Get
            Return Me.sDropDownShowTypeValue
        End Get
        Set(ByVal value As String)
            Me.sDropDownShowTypeValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property DropDownUserGroupsValue() As String
        Get
            Return Me.sDropDownUserGroupsValue
        End Get
        Set(ByVal value As String)
            Me.sDropDownUserGroupsValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property DropDownUsersValue() As String
        Get
            Return Me.sDropDownUsersValue
        End Get
        Set(ByVal value As String)
            Me.sDropDownUsersValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property DropDownPartyValue() As String
        Get
            Return Me.sDropDownPartyValue
        End Get
        Set(ByVal value As String)
            Me.sDropDownPartyValue = value
        End Set
    End Property
    '''<remarks/>
    Public Property ReferenceNumber() As String
        Get
            Return Me.sReferenceNumber
        End Get
        Set(ByVal value As String)
            Me.sReferenceNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property PageIndex() As String
        Get
            Return Me.sPageIndex
        End Get
        Set(ByVal value As String)
            Me.sPageIndex = value
        End Set
    End Property
 
End Class