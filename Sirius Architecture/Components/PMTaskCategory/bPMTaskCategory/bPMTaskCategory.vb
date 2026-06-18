Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class PMTaskCategory
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMTaskCategory
    '
    ' Date: 8th October 1999
    '
    ' Description: Describes the PMTaskCategory attributes.
    '
    ' Edit History:
    ' DAK131299 - Add licence key
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMTaskCategory"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' DataBase Attributes
    Private m_lTaskCategoryID As Integer
    Private m_lCaptionID As Integer
    Private m_sCategoryCode As String = ""
    Private m_sCategoryDescription As String = ""
    Private m_iDeleted As Integer
    Private m_iIsDeleted As gPMConstants.PMEVarTrueFalse
    Private m_dtEffectiveDate As Date
    ' LicenceLimit
    Private m_lLicenceLimit As Integer
    'DAK131299
    ' LicenceKey
    Private m_sLicenceKey As String = ""
    ' IsBlockAboveLicenceLimit
    Private m_iIsBlockAboveLicenceLimit As Integer
    ' IsWarnAboveLicenceLimit
    Private m_iIsWarnAboveLicenceLimit As Integer
    ' WarnsSinceLicenceUpgrade
    Private m_lWarnsSinceLicenceUpgrade As Integer

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


    Public Property TaskCategoryID() As Integer
        Get

            Return m_lTaskCategoryID

        End Get
        Set(ByVal Value As Integer)

            m_lTaskCategoryID = Value

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


    Public Property CategoryCode() As String
        Get

            Return m_sCategoryCode.Trim()

        End Get
        Set(ByVal Value As String)

            m_sCategoryCode = Value.Trim()

        End Set
    End Property

    Public Property CategoryDescription() As String
        Get

            Return m_sCategoryDescription.Trim()

        End Get
        Set(ByVal Value As String)

            m_sCategoryDescription = Value.Trim()

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

    Public Property LicenceLimit() As Integer
        Get
            Return m_lLicenceLimit
        End Get
        Set(ByVal Value As Integer)
            m_lLicenceLimit = Value
        End Set
    End Property

    'DAK131299
    Public Property LicenceKey() As String
        Get
            Return m_sLicenceKey.Trim()
        End Get
        Set(ByVal Value As String)
            m_sLicenceKey = Value.Trim()
        End Set
    End Property

    Public Property IsBlockAboveLicenceLimit() As Integer
        Get
            Return m_iIsBlockAboveLicenceLimit
        End Get
        Set(ByVal Value As Integer)
            m_iIsBlockAboveLicenceLimit = Value
        End Set
    End Property

    Public Property IsWarnAboveLicenceLimit() As Integer
        Get
            Return m_iIsWarnAboveLicenceLimit
        End Get
        Set(ByVal Value As Integer)
            m_iIsWarnAboveLicenceLimit = Value
        End Set
    End Property

    Public Property WarnsSinceLicenceUpgrade() As Integer
        Get
            Return m_lWarnsSinceLicenceUpgrade
        End Get
        Set(ByVal Value As Integer)
            m_lWarnsSinceLicenceUpgrade = Value
        End Set
    End Property

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

    Private Shared _DefaultInstance As PMTaskCategory = Nothing
    Public Shared ReadOnly Property DefaultInstance() As PMTaskCategory
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New PMTaskCategory
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
