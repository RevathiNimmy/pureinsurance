Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Friend NotInheritable Class ScheduledTask 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: ScheduledTask
	'
	' Date: 09/11/1998
	'
	' Description: A Scheduled Task.
	'
	' Edit History:
	' DAK071099 - Add new columns to scheduled tasks
	' DAK141299 - Tasks started from Available tasks should not be visisble
	' ***************************************************************** '
	
	
	' Constant for the functions to identify which class this is.
Private Const ACClass As String = "ScheduledTask" 
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Attributes
	Private m_lPMWrkTaskInstanceCnt As Integer
	Private m_sCustomer As String = ""
	Private m_dtTaskDueDate As Date
	Private m_lPmuserGroupID As Integer
	Private m_vUserID As String = ""
	Private m_sDescription As String = ""
	Private m_iTaskStatus As Integer
	Private m_iIsUrgent As Integer
	Private m_iTypeOfTask As Integer
	Private m_iIsSystemTask As Integer
	Private m_sUser As String = ""
	Private m_sUserGroup As String = ""
	Private m_sKey As String = ""
	Private m_lPMNavProcessId As Integer
	Private m_sComponentObjectName As String = ""
	Private m_sComponentClassName As String = ""
	'DAK071099
	' DisplayIcon
	Private m_lDisplayIcon As Integer
	' IsViewOnlyTask
	Private m_iIsViewOnlyTask As Integer
	' LinkedObjectName
	Private m_sLinkedObjectName As String = ""
	' LinkedClassName
	Private m_sLinkedClassName As String = ""
	' LinkedCaption
	Private m_sLinkedCaption As String = ""
	'DAK141299
	' IsVisible
	Private m_iIsVisible As Integer
	
	' Function Return Code
	Private lReturn As Integer
	
	' PRIVATE Data Members (End)
	
	' PUBLIC Property Procedures (Begin)
	
	Public Property PMWrkTaskInstanceCnt() As Integer
		Get
			Return m_lPMWrkTaskInstanceCnt
		End Get
		Set(ByVal Value As Integer)
			m_lPMWrkTaskInstanceCnt = Value
		End Set
	End Property
	
	Public Property Customer() As String
		Get
			Return m_sCustomer.Trim()
		End Get
		Set(ByVal Value As String)
			m_sCustomer = Value.Trim()
		End Set
	End Property
	
	Public Property TaskDueDate() As Date
		Get
			Return m_dtTaskDueDate
		End Get
		Set(ByVal Value As Date)
			m_dtTaskDueDate = Value
		End Set
	End Property
	
	Public Property PmuserGroupID() As Integer
		Get
			Return m_lPmuserGroupID
		End Get
		Set(ByVal Value As Integer)
			m_lPmuserGroupID = Value
		End Set
	End Property
	
	Public Property UserID() As String
		Get
			Return m_vUserID
		End Get
		Set(ByVal Value As String)

			m_vUserID = CStr(Value)
		End Set
	End Property
	
	Public Property Description() As String
		Get
			Return m_sDescription.Trim()
		End Get
		Set(ByVal Value As String)
			m_sDescription = Value.Trim()
		End Set
	End Property
	
	Public Property TaskStatus() As Integer
		Get
			Return m_iTaskStatus
		End Get
		Set(ByVal Value As Integer)
			m_iTaskStatus = Value
		End Set
	End Property
	
	Public Property IsUrgent() As Integer
		Get
			Return m_iIsUrgent
		End Get
		Set(ByVal Value As Integer)
			m_iIsUrgent = Value
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
	
	Public Property IsSystemTask() As Integer
		Get
			Return m_iIsSystemTask
		End Get
		Set(ByVal Value As Integer)
			m_iIsSystemTask = Value
		End Set
	End Property
	
	Public Property User() As String
		Get
			Return m_sUser.Trim()
		End Get
		Set(ByVal Value As String)
			m_sUser = Value.Trim()
		End Set
	End Property
	
	Public Property UserGroup() As String
		Get
			Return m_sUserGroup.Trim()
		End Get
		Set(ByVal Value As String)
			m_sUserGroup = Value.Trim()
		End Set
	End Property
	
	Public Property Key() As String
		Get
			Return m_sKey.Trim()
		End Get
		Set(ByVal Value As String)
			m_sKey = Value.Trim()
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
	
	'DAK071099
	Public Property DisplayIcon() As Integer
		Get
			Return m_lDisplayIcon
		End Get
		Set(ByVal Value As Integer)
			m_lDisplayIcon = Value
		End Set
	End Property
	
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
	
	'DAK141299
	Public Property IsVisible() As Integer
		Get
			Return m_iIsVisible
		End Get
		Set(ByVal Value As Integer)
			m_iIsVisible = Value
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
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
			
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
