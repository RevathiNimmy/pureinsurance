<Serializable()> Public Class AuditTrail

    'adding Properties for GetAuditTrail Request method

    Private dFromDateField As Date
    Private bFromDateFieldSpecified As Boolean
    Private dDateToField As Date
    Private bDateToFieldSpecified As Boolean
    Private iModuleKeyField As Integer
    Private bModuleKeySpecified As Boolean
    Private iUserIdField As Integer
    Private bUserIdSpecified As Boolean

    'adding properties for GetAuditTrailResponseType 
    Private iModuleIdField As Integer
    Private iConfigurationAuditdetailIdField As Integer
    Private sModuleName As String
    Private dEventFromDateField As Date
    Private dEventToDateField As Date

    Private sScreenDescriptionField As String
    Private sFieldDescriptionField As String

    Public iUserId As Integer
    Private sUserNameField As String

    Private sOldValueField As String
    Private sNewValueField As String



    Private dtModifiedOn As Date



    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        Return sbPrint.ToString()

    End Function

    'adding properties for GetAuditTrail Request method in Party
    '''<remarks/>

    Public Property FromDate() As Date
        Get
            Return Me.dFromDateField
        End Get
        Set(ByVal value As Date)
            Me.dFromDateField = value
        End Set
    End Property


    Public Property FromDateSpecified() As Boolean
        Get
            Return Me.bFromDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bFromDateFieldSpecified = value
        End Set
    End Property

    Public Property DateTo() As Date
        Get
            Return Me.dDateToField
        End Get
        Set(ByVal value As Date)
            Me.dDateToField = value
        End Set
    End Property


    Public Property DateToSpecified() As Boolean
        Get
            Return Me.bDateToFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bDateToFieldSpecified = value
        End Set
    End Property

    Public Property UserKey() As Integer
        Get
            Return Me.iUserIdField
        End Get
        Set(ByVal value As Integer)
            Me.iUserIdField = value
        End Set
    End Property

    Public Property UserKeySpecified() As Boolean
        Get
            Return Me.bUserIdSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bUserIdSpecified = value
        End Set
    End Property

    Public Property ModuleKeySpecified() As Boolean
        Get
            Return Me.bModuleKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bModuleKeySpecified = value
        End Set
    End Property

    Public Property ModuleKey() As Integer
        Get
            Return Me.iModuleKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iModuleKeyField = value
        End Set
    End Property
    'adding properties for GetAuditTrail Response method in Party

    Public Property ConfigurationAuditdetailId() As Integer
        Get
            Return Me.iConfigurationAuditdetailIdField
        End Get
        Set(ByVal value As Integer)
            Me.iConfigurationAuditdetailIdField = value
        End Set
    End Property
    Public Property ModuleId() As Integer
        Get
            Return Me.iModuleIdField
        End Get
        Set(ByVal value As Integer)
            Me.iModuleIdField = value
        End Set
    End Property

    Public Property ModuleName() As String
        Get
            Return Me.sModuleName
        End Get
        Set(ByVal value As String)
            Me.sModuleName = value
        End Set
    End Property
    '''<remarks/>
    Public Property EventFromDate() As Date
        Get
            Return Me.dEventFromDateField
        End Get
        Set(ByVal value As Date)
            Me.dEventFromDateField = value
        End Set
    End Property
    '''<remarks/>
    Public Property EventToDate() As Date
        Get
            Return Me.dEventToDateField
        End Get
        Set(ByVal value As Date)
            Me.dEventToDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ScreenDescription() As String
        Get
            Return Me.sScreenDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sScreenDescriptionField = value
        End Set
    End Property
    '''<remarks/>
    Public Property FieldDescription() As String
        Get
            Return Me.sFieldDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sFieldDescriptionField = value
        End Set
    End Property
    '''<remarks/>
    Public Property UserName() As String
        Get
            Return Me.sUserNameField
        End Get
        Set(ByVal value As String)
            Me.sUserNameField = value
        End Set
    End Property
    '''<remarks/>
    Public Property UserId() As Integer
        Get
            Return Me.iUserId
        End Get
        Set(ByVal value As Integer)
            Me.iUserId = value
        End Set
    End Property
    Public Property OldValue() As String
        Get
            Return Me.sOldValueField
        End Get
        Set(ByVal value As String)
            Me.sOldValueField = value
        End Set
    End Property
    Public Property NewValue() As String
        Get
            Return Me.sNewValueField
        End Get
        Set(ByVal value As String)
            Me.sNewValueField = value
        End Set
    End Property
    Public Property ModifiedOn() As DateTime
        Get
            Return Me.dtModifiedOn
        End Get
        Set(ByVal value As DateTime)
            Me.dtModifiedOn = value
        End Set
    End Property


End Class
''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AuditTrailCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(AuditTrail)
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAuditTrail As AuditTrail In List
            sbPrint.AppendLine(oAuditTrail.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oAuditTrail As AuditTrail) As Integer
        Return List.Add(v_oAuditTrail)
    End Function

    Public Sub Remove(ByVal v_oAuditTrail As AuditTrail)
        List.Remove(v_oAuditTrail)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As AuditTrail
        Get
            Return List(i)
        End Get
        Set(ByVal value As AuditTrail)
            List(i) = value
        End Set
    End Property



End Class