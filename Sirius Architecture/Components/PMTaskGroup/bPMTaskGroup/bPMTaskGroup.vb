Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No. 129
Imports SharedFiles
Friend NotInheritable Class PMTaskGroup
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMUserGroup
    '
    ' Date: 24th October 1996
    '
    ' Description: Describes the PMTask attributes.
    '
    ' Edit History:
    ' DAK041099 - Add foriegn key to PMWrk_Task_Group_Category
    ' DAK211299 - Remove PMWrk_Task_Group_Category
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMTaskGroup"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lTaskGroupID As Integer
    Private m_lCaptionID As Integer
    Private m_sTaskGroupCode As String = ""
    Private m_sTaskGroupDescription As String = ""
    Private m_iDeleted As Integer
    Private m_iIsDeleted As gPMConstants.PMEVarTrueFalse
    Private m_dtEffectiveDate As Date
    Private m_lDisplayIcon As Integer
    Private m_iIncluded As Integer
    'DAK041099
    'DAK211299
    'Private m_lTaskGroupCategoryID As Long

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


    Public Property TaskGroupID() As Integer
        Get

            Return m_lTaskGroupID

        End Get
        Set(ByVal Value As Integer)

            m_lTaskGroupID = Value

        End Set
    End Property


    Public Property CaptionID() As Integer
        Get

            Return m_lCaptionID

        End Get
        Set(ByVal Value As Integer)

            m_lCaptionID = Value

        End Set
    End Property


    Public Property TaskGroupCode() As String
        Get

            Return m_sTaskGroupCode

        End Get
        Set(ByVal Value As String)

            m_sTaskGroupCode = Value

        End Set
    End Property

    Public Property TaskGroupDescription() As String
        Get

            Return m_sTaskGroupDescription

        End Get
        Set(ByVal Value As String)

            m_sTaskGroupDescription = Value

        End Set
    End Property


    Public Property Deleted() As Integer
        Get

            Return m_iDeleted

        End Get
        Set(ByVal Value As Integer)

            m_iDeleted = Value

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


    Public Property DisplayIcon() As Integer
        Get

            Return m_lDisplayIcon

        End Get
        Set(ByVal Value As Integer)

            m_lDisplayIcon = Value

        End Set
    End Property


    Public Property Included() As Integer
        Get

            Return m_iIncluded

        End Get
        Set(ByVal Value As Integer)

            m_iIncluded = Value

        End Set
    End Property

    'DAK041099
    'DAK211299
    'Public Property Let TaskGroupCategoryID(lTaskGroupCategoryID As Long)

    '    m_lTaskGroupCategoryID = lTaskGroupCategoryID

    'End Property

    'Public Property Get TaskGroupCategoryID() As Long

    '    TaskGroupCategoryID = m_lTaskGroupCategoryID

    'End Property

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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As PMTaskGroup = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMTaskGroup
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMTaskGroup
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
