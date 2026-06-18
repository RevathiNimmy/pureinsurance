Imports System.Text
<Serializable()> Public Class WorkManager

    'Work Manager
    Private iTaskInstanceKeyField As Integer
    Private sCodeField As String
    Private sDescriptionField As String
    Private bIsDeletedField As Boolean
    Private dtEffectiveDateField As DateTime
    Private bTimeStampField() As Byte
    Private bIsSysAdminField As Boolean
    Private sTaskGroupField As String
    Private sTaskField As String
    Private dtDueDateTimeField As DateTime
    Private sClientField As String
    Private bIsUrgentField As Boolean
    Private bIsCompleteField As Boolean
    Private sAllocationUserGroupField As String
    Private bIsTaskReviewField As Boolean
    Private sAllocationUserField As String
    Private okeyDataField As KeyDataCollection
    Private oWorkManagerField As WorkManagerCollection

    Private iTaskGroupKey As Integer
    Private sTaskGroupCode As String
    Private iTaskKey As Integer
    Private sTaskCode As String
    Private sCustomer As String
    Private sBranch As String
    Private dtDueDate As DateTime
    Private iUserGroupKey As Integer
    Private sUserGroupCode As String
    Private iUserKey As Integer
    Private sUserCode As String
    Private sDescription As String
    Private iTaskStatusKey As Integer
    Private iIsUrgent As Integer
    Private iIsTaskReview As Integer
    Private iCreatedByKey As Integer
    Private dtDateCreated As DateTime
    Private iModifiedByKey As Integer
    Private dtLastModified As DateTime
    Private iIsvisible As Integer
    Private sWorkflowInformation As String
    Private bTimeStamp() As Byte
    Private sCreatedBy As String
    Private sModifiedBy As String

    Private sKeyName As String
    Private sKeyValue As String
    Private sLogText As String
    Private sUserName As String
    Private bUrgent As Boolean
    Private bComplete As Boolean
    Private sType As String
    Private sUserGroupDescription As String
    Private bTaskTimeStamp() As Byte
    Private iCaptionId As Integer
    Private iTaskGroupCategoryKey As Integer
    Private iDisplaySequence As Integer
    Private bDisplaySequenceSpecified As Boolean
    Private bIsSupervisor As Boolean
    Private iInsuranceFileKey As Integer
    Private iPartyKey As Integer
    Private sPartyName As String
    Private dAmount As Double
    Private dPercentage As Double
    Private oSubAgents As SubAgentCollection

    'UpdateUserGroup
    Private iUserGroupKeyField As Integer
    Private oUsersField As BaseUpdateUserGroupUsersCollection
    'Newly added Properties
    Private oTaskStatus As TaskStatus
    Private oDate As DateRange
    Private oShowType As ShowType
    Private oWMActionType As WMActionType
    Private nInsuranceFolderKeyField As Integer
    Private sReferenceNumber As String

    Public Sub New()
        okeyDataField = New KeyDataCollection()
        okeyDataField = New KeyDataCollection
        oWorkManagerField = New WorkManagerCollection
    End Sub

    Public Function Print() As String

        Dim sbPrint As New StringBuilder

        sbPrint.AppendLine("Key : " & iTaskInstanceKeyField.ToString() & "<br />")
        sbPrint.AppendLine("Description : " & sDescriptionField & "<br />")
        sbPrint.AppendLine("Code : " & sCodeField & "<br />")
        sbPrint.AppendLine("Effective Date : " & dtEffectiveDateField.ToString() & "<br />")
        sbPrint.AppendLine("Deleted : " & bIsDeletedField.ToString() & "<br />")
        sbPrint.AppendLine("SysAdmin : " & bIsSysAdminField.ToString() & "<br />")
        sbPrint.AppendLine("Allocation User : " & sAllocationUserField & "<br />")
        sbPrint.AppendLine("Allocation User Group : " & sAllocationUserGroupField & "<br />")
        sbPrint.AppendLine("Client : " & sClientField & "<br />")
        sbPrint.AppendLine("Due Date Time : " & dtDueDateTimeField.ToString() & "<br />")
        sbPrint.AppendLine("Completed : " & bIsCompleteField.ToString() & "<br />")
        sbPrint.AppendLine("Task Review : " & bIsTaskReviewField.ToString() & "<br />")
        sbPrint.AppendLine("Urgent : " & bIsUrgentField.ToString() & "<br />")
        sbPrint.AppendLine("Task : " & sTaskField & "<br />")
        sbPrint.AppendLine("Task Group : " & sTaskGroupField & "<br />")
        sbPrint.AppendLine("WorkManager -----------------><br />")

        If oWorkManagerField IsNot Nothing Then
            sbPrint.AppendLine(oWorkManagerField.Print())
            sbPrint.AppendLine("<br />")
        End If

        Return sbPrint.ToString()

    End Function
    Public Property Percentage() As Double
        Get
            Return dPercentage
        End Get
        Set(ByVal value As Double)
            dPercentage = value
        End Set
    End Property
    Public Property Amount() As Double
        Get
            Return dAmount
        End Get
        Set(ByVal value As Double)
            dAmount = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
        End Set
    End Property
    Public Property PartyName() As String
        Get
            Return sPartyName
        End Get
        Set(ByVal value As String)
            sPartyName = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
        End Set
    End Property
    Public Property SubAgents() As SubAgentCollection

        Get
            Return oSubAgents
        End Get
        Set(ByVal value As SubAgentCollection)
            oSubAgents = value
        End Set
    End Property
    'Newly Added Property for missing integer field  on 5-1-2009 
    'Begin
    Public Property IsUrgentForUpdate() As Integer
        Get
            Return iIsUrgent
        End Get
        Set(ByVal value As Integer)
            iIsUrgent = value
        End Set
    End Property
    'Newly Added Property for missing integer field on 5-1-2009 
    'End
    Public Property IsSupervisor() As Boolean
        Get
            Return bIsSupervisor
        End Get
        Set(ByVal value As Boolean)
            bIsSupervisor = value
        End Set
    End Property
    Public Property DisplaySequenceSpecified() As Boolean
        Get
            Return bDisplaySequenceSpecified
        End Get
        Set(ByVal value As Boolean)
            bDisplaySequenceSpecified = value
        End Set
    End Property
    Public Property DisplaySequence() As Integer
        Get
            Return iDisplaySequence
        End Get
        Set(ByVal value As Integer)
            iDisplaySequence = value
        End Set
    End Property
    Public Property TaskGroupCategoryKey() As Integer
        Get
            Return iTaskGroupCategoryKey
        End Get
        Set(ByVal value As Integer)
            iTaskGroupCategoryKey = value
        End Set
    End Property
    Public Property CaptionId() As Integer
        Get
            Return iCaptionId
        End Get
        Set(ByVal value As Integer)
            iCaptionId = value
        End Set
    End Property
    Public Property TaskTimeStamp() As Byte()
        Get
            Return bTaskTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTaskTimeStamp = value
        End Set
    End Property
    Public Property UserGroupDescription() As String
        Get
            Return sUserGroupDescription
        End Get
        Set(ByVal value As String)
            sUserGroupDescription = value
        End Set
    End Property
    Public Property Type() As String
        Get
            Return sType
        End Get
        Set(ByVal value As String)
            sType = value
        End Set
    End Property

    Public Property Urgent() As Boolean
        Get
            Return bUrgent
        End Get
        Set(ByVal value As Boolean)
            bUrgent = value
        End Set
    End Property

    Public Property Complete() As Boolean
        Get
            Return bComplete
        End Get
        Set(ByVal value As Boolean)
            bComplete = value
        End Set
    End Property
    Public Property UserName() As String
        Get
            Return sUserName
        End Get
        Set(ByVal value As String)
            sUserName = value
        End Set
    End Property
    Public Property KeyValue() As String
        Get
            Return sKeyValue
        End Get
        Set(ByVal value As String)
            sKeyValue = value
        End Set
    End Property

    Public Property KeyName() As String
        Get
            Return sKeyName
        End Get
        Set(ByVal value As String)
            sKeyName = value
        End Set
    End Property

    Public Property WorkflowInformation() As String
        Get
            Return sWorkflowInformation
        End Get
        Set(ByVal value As String)
            sWorkflowInformation = value
        End Set
    End Property
    Public Property Isvisible() As Integer
        Get
            Return iIsvisible
        End Get
        Set(ByVal value As Integer)
            iIsvisible = value
        End Set
    End Property
    Public Property ModifiedByKey() As Integer
        Get
            Return iModifiedByKey
        End Get
        Set(ByVal value As Integer)
            iModifiedByKey = value
        End Set
    End Property
    Public Property LastModified() As DateTime
        Get
            Return dtLastModified
        End Get
        Set(ByVal value As DateTime)
            dtLastModified = value
        End Set
    End Property
    Public Property DateCreated() As DateTime
        Get
            Return dtDateCreated
        End Get
        Set(ByVal value As DateTime)
            dtDateCreated = value
        End Set
    End Property
    Public Property CreatedByKey() As Integer
        Get
            Return iCreatedByKey
        End Get
        Set(ByVal value As Integer)
            iCreatedByKey = value
        End Set
    End Property
    Public Property TaskStatusKey() As Integer
        Get
            Return iTaskStatusKey
        End Get
        Set(ByVal value As Integer)
            iTaskStatusKey = value
        End Set
    End Property
    Public Property LogText() As String
        Get
            Return sLogText
        End Get
        Set(ByVal value As String)
            sLogText = value
        End Set
    End Property
    Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
        End Set
    End Property
    Public Property UserCode() As String
        Get
            Return sUserCode
        End Get
        Set(ByVal value As String)
            sUserCode = value
        End Set
    End Property
    Public Property UserKey() As Integer
        Get
            Return iUserKey
        End Get
        Set(ByVal value As Integer)
            iUserKey = value
        End Set
    End Property
    Public Property UserGroupCode() As String
        Get
            Return sUserGroupCode
        End Get
        Set(ByVal value As String)
            sUserGroupCode = value
        End Set
    End Property
    Public Property UserGroupKey() As Integer
        Get
            Return iUserGroupKey
        End Get
        Set(ByVal value As Integer)
            iUserGroupKey = value
        End Set
    End Property
    Public Property DueDate() As DateTime
        Get
            Return dtDueDate
        End Get
        Set(ByVal value As DateTime)
            dtDueDate = value
        End Set
    End Property


    Public Property Customer() As String
        Get
            Return sCustomer
        End Get
        Set(ByVal value As String)
            sCustomer = value
        End Set
    End Property

    Public Property Branch() As String
        Get
            Return sBranch
        End Get
        Set(ByVal value As String)
            sBranch = value
        End Set
    End Property

    Public Property TaskCode() As String
        Get
            Return sTaskCode
        End Get
        Set(ByVal value As String)
            sTaskCode = value
        End Set
    End Property
    Public Property TaskKey() As Integer
        Get
            Return iTaskKey
        End Get
        Set(ByVal value As Integer)
            iTaskKey = value
        End Set
    End Property
    Public Property TaskGroupCode() As String
        Get
            Return sTaskGroupCode
        End Get
        Set(ByVal value As String)
            sTaskGroupCode = value
        End Set
    End Property
    Public Property TaskGroupKey() As Integer
        Get
            Return iTaskGroupKey
        End Get
        Set(ByVal value As Integer)
            iTaskGroupKey = value
        End Set
    End Property
    Public Property TaskInstanceKey() As Integer
        Get
            Return iTaskInstanceKeyField
        End Get
        Set(ByVal value As Integer)
            iTaskInstanceKeyField = value
        End Set
    End Property
    Public Property InsuranceFolderKey() As Integer
        Get
            Return nInsuranceFolderKeyField
        End Get
        Set(ByVal value As Integer)
            nInsuranceFolderKeyField = value
        End Set
    End Property
    Public Property Code() As String
        Get
            Return Me.sCodeField
        End Get
        Set(ByVal value As String)
            Me.sCodeField = value
        End Set
    End Property

    Public Property IsDeleted() As Boolean
        Get
            Return Me.bIsDeletedField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDeletedField = value
        End Set
    End Property
    Public Property EffectiveDate() As DateTime
        Get
            Return Me.dtEffectiveDateField
        End Get
        Set(ByVal value As DateTime)
            Me.dtEffectiveDateField = value
        End Set
    End Property

    Public Property IsSysAdmin() As Boolean
        Get
            Return Me.bIsSysAdminField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSysAdminField = value
        End Set
    End Property

    Public Property TaskGroup() As String
        Get
            Return Me.sTaskGroupField
        End Get
        Set(ByVal value As String)
            Me.sTaskGroupField = value
        End Set
    End Property

    Public Property Task() As String
        Get
            Return Me.sTaskField
        End Get
        Set(ByVal value As String)
            Me.sTaskField = value
        End Set
    End Property

    Public Property DueDateTime() As DateTime
        Get
            Return Me.dtDueDateTimeField
        End Get
        Set(ByVal value As DateTime)
            Me.dtDueDateTimeField = value
        End Set
    End Property

    Public Property Client() As String
        Get
            Return Me.sClientField
        End Get
        Set(ByVal value As String)
            Me.sClientField = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    Public Property IsUrgent() As Boolean
        Get
            Return Me.bIsUrgentField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsUrgentField = value
        End Set
    End Property

    Public Property IsComplete() As Boolean
        Get
            Return Me.bIsCompleteField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsCompleteField = value
        End Set
    End Property

    Public Property AllocationUserGroup() As String
        Get
            Return Me.sAllocationUserGroupField
        End Get
        Set(ByVal value As String)
            Me.sAllocationUserGroupField = value
        End Set
    End Property

    Public Property IsTaskReview() As Boolean
        Get
            Return Me.bIsTaskReviewField
        End Get
        Set(ByVal value As Boolean)
            Me.bIsTaskReviewField = value
        End Set
    End Property

    Public Property AllocationUser() As String
        Get
            Return Me.sAllocationUserField
        End Get
        Set(ByVal value As String)
            Me.sAllocationUserField = value
        End Set
    End Property

    Public Property KeyData() As KeyDataCollection
        Get
            Return Me.okeyDataField
        End Get
        Set(ByVal value As KeyDataCollection)
            Me.okeyDataField = value
        End Set
    End Property
    Public Property TaskStatus() As TaskStatus
        Get
            Return Me.oTaskStatus
        End Get
        Set(ByVal value As TaskStatus)
            Me.oTaskStatus = value
        End Set
    End Property
    Public Property DateRange() As DateRange
        Get
            Return Me.oDate
        End Get
        Set(ByVal value As DateRange)
            Me.oDate = value
        End Set
    End Property
    Public Property ShowType() As ShowType
        Get
            Return Me.oShowType
        End Get
        Set(ByVal value As ShowType)
            Me.oShowType = value
        End Set
    End Property
    Public Property WmActionType() As WMActionType
        Get
            Return Me.oWMActionType
        End Get
        Set(ByVal value As WMActionType)
            Me.oWMActionType = value
        End Set
    End Property
    Public Property CreatedBy() As String
        Get
            Return sCreatedBy
        End Get
        Set(ByVal value As String)
            sCreatedBy = value
        End Set
    End Property
    Public Property ModifiedBy() As String
        Get
            Return sModifiedBy
        End Get
        Set(ByVal value As String)
            sModifiedBy = value
        End Set
    End Property
    Public Property ReferenceNumber() As String
        Get
            Return sReferenceNumber
        End Get
        Set(value As String)
            sReferenceNumber = value
        End Set
    End Property

    Public Property IsExternalItem() As Boolean

    Public Property ParentTaskId() As Integer

    Public Property ExternalTaskCategoryCode() As String

    Public Property GuidPMExternalItem() As String

    Public Property ActionType() As String

    Public Property LockName() As String

    Public Property LockValue() As Integer

    Public Property ExternalTaskStatus() As Integer

    Public Property IsExternalChildTask() As Boolean
End Class
<Serializable()> Public Class WorkManagerCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(WorkManager)
    End Sub

    Public Function Print() As String

        Dim sbPrint As New StringBuilder
        sbPrint.AppendLine()
        For Each oWorkManager As WorkManager In List
            sbPrint.AppendLine(oWorkManager.Print())
            sbPrint.AppendLine("<Br/>")
        Next
        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oWorkManager As WorkManager) As Integer

        Return List.Add(v_oWorkManager)

    End Function
    Public Sub Remove(ByVal v_oWorkManager As WorkManager)
        List.Remove(v_oWorkManager)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As WorkManager

        Get
            Return List(i)
        End Get
        Set(ByVal value As WorkManager)
            List(i) = value
        End Set

    End Property

End Class


<Serializable()> Public Class BaseUpdateUserGroupUsersCollection : Inherits Collections.CollectionBase
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New StringBuilder
        sbPrint.AppendLine()
        For Each oUpdateUserGroupUsers As WorkManager In List
            sbPrint.AppendLine(oUpdateUserGroupUsers.Print())
            sbPrint.AppendLine("<Br/>")
        Next
        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oUpdateUserGroupUsers As WorkManager) As Integer

        Return List.Add(v_oUpdateUserGroupUsers)

    End Function
    Public Sub Remove(ByVal v_oUpdateUserGroupUsers As WorkManager)
        List.Remove(v_oUpdateUserGroupUsers)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As WorkManager

        Get
            Return List(i)
        End Get
        Set(ByVal value As WorkManager)
            List(i) = value
        End Set

    End Property

End Class
Public Enum TaskStatus

    '''<remarks/>
    [New]

    '''<remarks/>

    InProgress

    '''<remarks/>
    InComplete

    '''<remarks/>
    Complete

    '''<remarks/>
    All

    '''<remarks/>
    NotComplete
End Enum
Public Enum DateRange

    '''<remarks/>
    <System.Xml.Serialization.XmlEnumAttribute("(All Dates)")> _
    AllDates

    '''<remarks/>
    Today

    '''<remarks/>
    Tomorrow

    '''<remarks/>

    Next2Days

    '''<remarks/>

    Next3Days

    '''<remarks/>

    Next4Days

    '''<remarks/>

    Next5Days

    '''<remarks/>

    Next6Days

    '''<remarks/>

    Next7Days

    '''<remarks/>

    Next14Days

    '''<remarks/>

    Next28Days
End Enum
Public Enum ShowType

    '''<remarks/>
    User

    '''<remarks/>
    Sys

    '''<remarks/>

    All
End Enum
Public Enum WMActionType

    '''<remarks/>
    Update

    '''<remarks/>
    Assign

    '''<remarks/>
    Complete

    '''<remarks/>
    InComplete

    '''<remarks/>
    Run
End Enum

Public Enum WMLockType

    '''<remarks/>
    InsuranceFolderCnt = 1

    '''<remarks/>
    InsuranceFileCnt = 2

    '''<remarks/>
    PartyCnt = 3

    '''<remarks/>
    ClaimId = 4

    '''<remarks/>
    TaskInstanceCnt = 5

    '''<remarks/>
    UserGroupCnt = 6

    '''<remarks/>
    TaskGroupCnt = 7

    '''<remarks/>
    CoverNoteBookId = 8

    '''<remarks/>
    BGId = 9

    '''<remarks/>
    RenewalProcess = 10

    '''<remarks/>
    TransDetailKey = 11

    '''<remarks/>
    CashListItemID = 12

    '''<remarks/>
    ClaimPaymentCnt = 13

    '''<remarks/>
    RiskKey = 14

    '''<remarks/>
    CashDepositKey = 15
End Enum
<Serializable()> Public Class TaskLog

    Private iTaskInstanceKeyField As Integer

    Private dtDateCreated As Date

    Private sLogText As String

    Private iCreatedbyKey As Integer

    Private sUserName As String
#Region "Properties"
    Public Property TaskInstanceKey() As Integer
        Get
            Return Me.iTaskInstanceKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iTaskInstanceKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property DateCreated() As Date
        Get
            Return Me.dtDateCreated
        End Get
        Set(ByVal value As Date)
            Me.dtDateCreated = value
        End Set
    End Property

    '''<remarks/>
    Public Property LogText() As String
        Get
            Return Me.sLogText
        End Get
        Set(ByVal value As String)
            Me.sLogText = value
        End Set
    End Property

    '''<remarks/>
    Public Property CreatedByKey() As Integer
        Get
            Return Me.iCreatedbyKey
        End Get
        Set(ByVal value As Integer)
            Me.iCreatedbyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserName() As String
        Get
            Return Me.sUserName
        End Get
        Set(ByVal value As String)
            Me.sUserName = value
        End Set
    End Property
#End Region

End Class
<Serializable()> Public Class TaskLogCollection : Inherits Collections.CollectionBase
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>


    Public Function Add(ByVal v_oTaskLog As TaskLog) As Integer

        Return List.Add(v_oTaskLog)

    End Function
    Public Sub Remove(ByVal v_oTaskLog As TaskLog)
        List.Remove(v_oTaskLog)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As TaskLog

        Get
            Return List(i)
        End Get
        Set(ByVal value As TaskLog)
            List(i) = value
        End Set

    End Property

End Class