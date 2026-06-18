<Serializable()> Public Class EventDetails
    Private sPriority As String
    Private bStatusKey As Boolean
    Private iEventTypeKey As Integer
    Private iEventLogSubjectKey As Integer
    Private dtEventDate As DateTime
    Private sRtfText As String
    Private iEventkey As Integer
    'adding Properties for GetEventDetails Request method
    Private iBGKeyField As Integer
    Private bBGKeyFieldSpecified As Boolean
    Private iInsuranceFolderKeyField As Integer
    Private bInsuranceFolderKeyFieldSpecified As Boolean
    Private iInsuranceFileKeyField As Integer
    Private bInsuranceFileKeyFieldSpecified As Boolean
    Private iClaimKeyField As Integer
    Private bClaimKeyFieldSpecified As Boolean
    Private iOldPartyTypeKeyField As Integer
    Private bOldPartyTypeKeyFieldSpecified As Boolean
    Private iFSAComplaintFolderKeyField As Integer
    Private bFSAComplaintFolderKeyFieldSpecified As Boolean
    Private iAccountKeyField As Integer
    Private bAccountKeyFieldSpecified As Boolean
    Private dFromDateField As Date
    Private bFromDateFieldSpecified As Boolean
    Private dDateToField As Date
    Private bDateToFieldSpecified As Boolean
    Private iBaseClaimKeyField As Integer
    Private bBaseClaimKeyFieldSpecified As Boolean
    Private iCaseKeyField As Integer
    Private bCaseKeyFieldSpecified As Boolean
    Private iBaseCaseKeyField As Integer
    Private bBaseCaseKeyFieldSpecified As Boolean
    Private iPartyKeyField As Integer
    'adding properties for GetEventDetailsResponseType 
    Private iEventKeyField As Integer
    Private iDocumentKeyField As Integer
    Private dEventFromDateField As Date
    Private dEventToDateField As Date
    Private iTypeKeyField As Integer
    Private sPolicyCodeField As String
    Private sClaimNumberField As String
    Private sDescriptionField As String
    Private sEventDescriptionField As String
    Private sUserNameField As String
    Private sPriorityField As String
    Private sStatusKeyField As Short
    Private sEventNoteExistField As String
    Private sEventTypeField As String
    Public sEventText As String
    Public iEventPublicTextKey As Integer
    Public iUserId As Integer
    Private sCaseNumber As String
    Private sDocument_PathField As String
    Private oNewEvent As EventDetailsCollection


    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        Return sbPrint.ToString()

    End Function

    Public Property EventTypeKey() As Integer
        Get
            Return iEventTypeKey
        End Get
        Set(ByVal value As Integer)
            iEventTypeKey = value
        End Set
    End Property
    Public Property EventLogSubjectKey() As Integer
        Get
            Return iEventLogSubjectKey
        End Get
        Set(ByVal value As Integer)
            iEventLogSubjectKey = value
        End Set
    End Property
    Public Property RtfText() As String
        Get
            Return sRtfText
        End Get
        Set(ByVal value As String)
            sRtfText = value
        End Set
    End Property

    Public Property EventText() As String
        Get
            Return sEventText
        End Get
        Set(ByVal value As String)
            sEventText = value
        End Set
    End Property
    Public Property EventPublicTextKey() As Integer
        Get
            Return iEventPublicTextKey
        End Get
        Set(ByVal value As Integer)
            iEventPublicTextKey = value
        End Set
    End Property
    'adding properties for GetEventDetails Request method in Party
    '''<remarks/>
    Public Property BGKey() As Integer
        Get
            Return Me.iBGKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBGKeyField = value
        End Set
    End Property

    Public Property BGKeySpecified() As Boolean
        Get
            Return Me.bBGKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bBGKeyFieldSpecified = value
        End Set
    End Property

    Public Property InsuranceFolderKey() As Integer
        Get
            Return Me.iInsuranceFolderKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFolderKeyField = value
        End Set
    End Property


    Public Property InsuranceFolderKeySpecified() As Boolean
        Get
            Return Me.bInsuranceFolderKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bInsuranceFolderKeyFieldSpecified = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKeyField = value
        End Set
    End Property
    Public Property InsuranceFileKeySpecified() As Boolean
        Get
            Return Me.bInsuranceFileKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bInsuranceFileKeyFieldSpecified = value
        End Set
    End Property

    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKeyField = value
        End Set
    End Property


    Public Property ClaimKeySpecified() As Boolean
        Get
            Return Me.bClaimKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bClaimKeyFieldSpecified = value
        End Set
    End Property

    Public Property OldPartyTypeKey() As Integer
        Get
            Return Me.iOldPartyTypeKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iOldPartyTypeKeyField = value
        End Set
    End Property

    Public Property OldPartyTypeKeySpecified() As Boolean
        Get
            Return Me.bOldPartyTypeKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bOldPartyTypeKeyFieldSpecified = value
        End Set
    End Property

    Public Property FSAComplaintFolderKey() As Integer
        Get
            Return Me.iFSAComplaintFolderKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iFSAComplaintFolderKeyField = value
        End Set
    End Property


    Public Property FSAComplaintFolderKeySpecified() As Boolean
        Get
            Return Me.bFSAComplaintFolderKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bFSAComplaintFolderKeyFieldSpecified = value
        End Set
    End Property

    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKeyField = value
        End Set
    End Property


    Public Property AccountKeySpecified() As Boolean
        Get
            Return Me.bAccountKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bAccountKeyFieldSpecified = value
        End Set
    End Property

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

    Public Property BaseClaimKey() As Integer
        Get
            Return Me.iBaseClaimKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimKeyField = value
        End Set
    End Property

    Public Property BaseClaimKeySpecified() As Boolean
        Get
            Return Me.bBaseClaimKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bBaseClaimKeyFieldSpecified = value
        End Set
    End Property

    Public Property CaseKey() As Integer
        Get
            Return Me.iCaseKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iCaseKeyField = value
        End Set
    End Property


    Public Property CaseKeySpecified() As Boolean
        Get
            Return Me.bCaseKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCaseKeyFieldSpecified = value
        End Set
    End Property

    Public Property BaseCaseKey() As Integer
        Get
            Return Me.iBaseCaseKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iBaseCaseKeyField = value
        End Set
    End Property


    Public Property BaseCaseKeySpecified() As Boolean
        Get
            Return Me.bBaseCaseKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bBaseCaseKeyFieldSpecified = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKeyField = value
        End Set
    End Property
    'adding properties for GetEventDetails Response method in Party
    '''<remarks/>
    Public Property EventKey() As Integer
        Get
            Return Me.iEventKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iEventKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentKey() As Integer
        Get
            Return Me.iDocumentKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iDocumentKeyField = value
        End Set
    End Property
    '''<remarks/>
    Public Property EventDate() As Date
        Get
            Return Me.dtEventDate
        End Get
        Set(ByVal value As Date)
            Me.dtEventDate = value
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
    Public Property TypeKey() As Integer
        Get
            Return Me.iTypeKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iTypeKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyCode() As String
        Get
            Return Me.sPolicyCodeField
        End Get
        Set(ByVal value As String)
            Me.sPolicyCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumberField
        End Get
        Set(ByVal value As String)
            Me.sClaimNumberField = value
        End Set
    End Property


    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property EventDescription() As String
        Get
            Return Me.sEventDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sEventDescriptionField = value
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

    '''<remarks/>
    Public Property Priority() As String
        Get
            Return Me.sPriorityField
        End Get
        Set(ByVal value As String)
            Me.sPriorityField = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusKey() As Short
        Get
            Return Me.sStatusKeyField
        End Get
        Set(ByVal value As Short)
            Me.sStatusKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property EventNoteExist() As String
        Get
            Return Me.sEventNoteExistField
        End Get
        Set(ByVal value As String)
            Me.sEventNoteExistField = value
        End Set
    End Property

    '''<remarks/>
    Public Property EventType() As String
        Get
            Return Me.sEventTypeField
        End Get
        Set(ByVal value As String)
            Me.sEventTypeField = value
        End Set
    End Property

    Public Property AddEvent() As EventDetailsCollection
        Get
            Return Me.oNewEvent
        End Get
        Set(ByVal value As EventDetailsCollection)
            Me.oNewEvent = value
        End Set
    End Property

    ''' <summary>
    ''' Will be used in Searching and displaying in grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseNumber() As String
        Get
            Return Me.sCaseNumber
        End Get
        Set(ByVal value As String)
            Me.sCaseNumber = value
        End Set
    End Property

    ''' <summary>
    ''' Sharepoint Document Path
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Document_Path() As String
        Get
            Return Me.sDocument_PathField
        End Get
        Set(ByVal value As String)
            Me.sDocument_PathField = value
        End Set
    End Property
End Class
''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class EventDetailsCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(EventDetails)
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oEventDetails As EventDetails In List
            sbPrint.AppendLine(oEventDetails.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oEventDetails As EventDetails) As Integer
        Return List.Add(v_oEventDetails)
    End Function

    Public Sub Remove(ByVal v_oEventDetails As EventDetails)
        List.Remove(v_oEventDetails)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As EventDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As EventDetails)
            List(i) = value
        End Set
    End Property

End Class