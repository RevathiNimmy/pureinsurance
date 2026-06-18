Option Strict Off
Option Explicit On
'Developer Guide No. 129
Imports SSP.Shared

Friend NotInheritable Class PMTask
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMTask
    '
    ' Date: 24th October 1996
    '
    ' Description: Describes the PMTask attributes.
    '
    ' Edit History:
    ' DAK200999 - New column, DisplayIcon, on PMWrkTask Table
    ' DAK041099 - New fields for view only task, tasks linked to objects,
    '             and whether task can be run directly from available tasks
    '             bar.
    ' DAK211299 - Add PMWrk_Task_Category
    ' RAW 14/02/2003 : ISS2153 : added NavXMLFile
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMTask"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As gPMConstants.PMEComponentAction

    ' DataBase Attributes
    Private m_lTaskID As Integer
    Private m_lCaptionId As Integer
    Private m_sTaskCode As String = ""
    Private m_sDescription As String = ""
    Private m_iIsDeleted As gPMConstants.PMEVarTrueFalse
    Private m_dtEffectiveDate As Date
    Private m_iIsSystemTask As Integer
    Private m_iTypeOfTask As Integer
    Private m_lPMNavProcessId As Integer
    Private m_sComponentObjectName As String = ""
    Private m_sComponentClassName As String = ""
    Private m_lAutoDeleteAfterNumDays As Integer
    'DAK200999
    Private m_lDisplayIcon As Integer
    'DAK041099
    Private m_iIsViewOnlyTask As Integer
    Private m_sLinkedObjectName As String = ""
    Private m_sLinkedClassName As String = ""
    Private m_sLinkedCaption As String = ""
    Private m_iIsAvailableTask As Integer
    ' TaskCategoryID
    Private m_lTaskCategoryID As Integer
    ' RAW 14/02/2003 : ISS2153 : added
    Private m_sNavXMLFile As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property


    Public Property TaskID() As Integer
        Get

            Return m_lTaskID

        End Get
        Set(ByVal Value As Integer)

            m_lTaskID = Value

        End Set
    End Property


    Public Property CaptionID() As Integer
        Get

            Return m_lCaptionId

        End Get
        Set(ByVal Value As Integer)

            m_lCaptionId = Value

        End Set
    End Property


    Public Property TaskCode() As String
        Get

            Return m_sTaskCode

        End Get
        Set(ByVal Value As String)

            m_sTaskCode = Value

        End Set
    End Property


    Public Property Description() As String
        Get

            Return m_sDescription

        End Get
        Set(ByVal Value As String)

            m_sDescription = Value

        End Set
    End Property


    Public Property IsDeleted() As Integer
        Get

            Return m_iIsDeleted

        End Get
        Set(ByVal Value As Integer)

            m_iIsDeleted = Value

        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
        Set(ByVal Value As Date)

            m_dtEffectiveDate = Value

        End Set
    End Property


    Public Property IsSystemTask() As Integer
        Get

            Return m_iIsSystemTask

        End Get
        Set(ByVal Value As Integer)

            m_iIsSystemTask = Value

        End Set
    End Property


    Public Property TypeOfTask() As Integer
        Get

            Return m_iTypeOfTask

        End Get
        Set(ByVal Value As Integer)

            m_iTypeOfTask = Value

        End Set
    End Property


    Public Property PMNavProcessId() As Integer
        Get

            Return m_lPMNavProcessId

        End Get
        Set(ByVal Value As Integer)

            m_lPMNavProcessId = Value

        End Set
    End Property


    Public Property ComponentObjectName() As String
        Get

            Return m_sComponentObjectName

        End Get
        Set(ByVal Value As String)

            m_sComponentObjectName = Value

        End Set
    End Property


    Public Property ComponentClassName() As String
        Get

            Return m_sComponentClassName

        End Get
        Set(ByVal Value As String)

            m_sComponentClassName = Value

        End Set
    End Property


    Public Property AutoDeleteAfterNumDays() As Integer
        Get

            Return m_lAutoDeleteAfterNumDays

        End Get
        Set(ByVal Value As Integer)

            m_lAutoDeleteAfterNumDays = Value

        End Set
    End Property

    'DAK200999
    Public Property DisplayIcon() As Integer
        Get
            Return m_lDisplayIcon
        End Get
        Set(ByVal Value As Integer)
            m_lDisplayIcon = Value
        End Set
    End Property

    'DAK041099
    Public Property IsViewOnlyTask() As Integer
        Get
            Return m_iIsViewOnlyTask
        End Get
        Set(ByVal Value As Integer)
            m_iIsViewOnlyTask = Value
        End Set
    End Property

    Public Property LinkedObjectName() As String
        Get
            Return m_sLinkedObjectName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedObjectName = Value.Trim()
        End Set
    End Property

    Public Property LinkedClassName() As String
        Get
            Return m_sLinkedClassName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedClassName = Value.Trim()
        End Set
    End Property

    Public Property LinkedCaption() As String
        Get
            Return m_sLinkedCaption.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLinkedCaption = Value.Trim()
        End Set
    End Property

    Public Property IsAvailableTask() As Integer
        Get
            Return m_iIsAvailableTask
        End Get
        Set(ByVal Value As Integer)
            m_iIsAvailableTask = Value
        End Set
    End Property

    Public Property TaskCategoryID() As Integer
        Get
            Return m_lTaskCategoryID
        End Get
        Set(ByVal Value As Integer)
            m_lTaskCategoryID = Value
        End Set
    End Property
    'eck140904
    Public Property NavXMLFile() As String
        Get
            Return m_sNavXMLFile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLFile = Value
        End Set
    End Property
    'eck140904End
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try


            ' Initialisation Code.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMTask = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMTask
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMTask
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
