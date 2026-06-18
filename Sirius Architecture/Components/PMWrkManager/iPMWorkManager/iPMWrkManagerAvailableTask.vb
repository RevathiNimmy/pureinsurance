Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles

Friend NotInheritable Class AvailableTask 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: PMTask
	'
	' Date: 30th November 1998
	'
	' Description: A Task that the user is able to start.
	'
	' Edit History:
	' DAK130999 - Add Favourites index property
	' DAK071099 - More new task columns
	' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
Private Const ACClass As String = "PMAvailableTask" 

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' DataBase Attributes
    Private m_lTaskGroupID As Integer
    Private m_lTaskID As Integer
    Private m_sTaskCaption As String = ""
    Private m_iIsSystemTask As Integer
    Private m_iTypeOfTask As Integer
    Private m_lPMNavProcessId As Integer
    Private m_sComponentObjectName As String = ""
    Private m_sComponentClassName As String = ""
    Private m_lAutoDeleteAfterNumDays As Integer
    Private m_iIsSupervisor As Integer
    Private m_lDisplayIcon As Integer
    'DAK071099
    ' IsViewOnlyTask
    Private m_iIsViewOnlyTask As Integer
    ' LinkedObjectName
    Private m_sLinkedObjectName As String = ""
    ' LinkedClassName
    Private m_sLinkedClassName As String = ""
    ' LinkedCaption
    Private m_sLinkedCaption As String = ""

    Private m_lQuickStartBarPos As Integer
    'DAK130999
    Private m_iFavouritesIndex As Integer
    ' RDC 10012003
    Private m_sNavXMLfile As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property TaskGroupID() As Integer
        Get
            Return m_lTaskGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lTaskGroupID = Value
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

    Public Property TaskCaption() As String
        Get
            Return m_sTaskCaption.Trim()
        End Get
        Set(ByVal Value As String)
            m_sTaskCaption = Value.Trim()
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
            Return m_sComponentObjectName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sComponentObjectName = Value.Trim()
        End Set
    End Property

    Public Property ComponentClassName() As String
        Get
            Return m_sComponentClassName.Trim()
        End Get
        Set(ByVal Value As String)
            m_sComponentClassName = Value.Trim()
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

    Public Property IsSupervisor() As Integer
        Get
            Return m_iIsSupervisor
        End Get
        Set(ByVal Value As Integer)
            m_iIsSupervisor = Value
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

    'DAK071099
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

    Public Property QuickStartBarPos() As Integer
        Get
            Return m_lQuickStartBarPos
        End Get
        Set(ByVal Value As Integer)
            m_lQuickStartBarPos = Value
        End Set
    End Property

    'DAK130999 - Favourites Group index position
    Public Property FavouritesIndex() As Integer
        Get
            Return m_iFavouritesIndex
        End Get
        Set(ByVal Value As Integer)
            m_iFavouritesIndex = Value
        End Set
    End Property

    ' RDC 10012003 Navigator XM file
    Public Property NavXMLfile() As String
        Get
            Return m_sNavXMLfile
        End Get
        Set(ByVal Value As String)
            m_sNavXMLfile = Value
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
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise()", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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

        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

End Class
