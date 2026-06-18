Option Strict Off
Option Explicit On
Imports System
Public Interface NavigatorV2
	WriteOnly Property CallingAppName As String
	ReadOnly Property Task As Integer
	ReadOnly Property Navigate As Integer
	ReadOnly Property ProcessMode As Integer
	ReadOnly Property TransactionType As String
	ReadOnly Property EffectiveDate As Date
	ReadOnly Property Status As Integer
	ReadOnly Property StepStatus As String
Function SetKeys(ByRef vKeyArray(,) As Object ) As Integer
	Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
	Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer
	Function Start() As Integer
Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer
	Function GetSummary(ByRef vSummaryArray As Object) As Integer
End Interface
<System.Runtime.InteropServices.ProgId("NavigatorV2_CoClass_NET.NavigatorV2_CoClass")> _
Public NotInheritable Class NavigatorV2_CoClass 
	Implements NavigatorV2
	' ***************************************************************** '
	' Class Name: NavigatorV2
	'
	' Date: 30/10/1997
	'
	' Description: This Abstract Class does not implement (have any
	'              code for) any of its public properties or methods.
	'
	'              Its purpose is to define the public Properties and
	'              Methods that a Navigator enabled component needs
	'              to Implement so that it can be called via Navigator.
	'
	'              Each compoment which wants to be Navigator Version 2
	'              enabled must add the following line of code to the
	'              top of the class.
	'
	'              Implements aPMNav.NavigatorV2
	'
	'              With this line the component will not compile until
	'              it supports all of the public properties and methods
	'              declared within this class.
	'
	' Edit History:
	'              30/10/1997 RFC Original
	'
	' ***************************************************************** '
	
	' PUBLIC Property Procedures (Begin)
	
	' ***************************************************************** '
	' The following property is set by Navigator                        '
	' ***************************************************************** '
	
	' The Calling application or component name.
	Public WriteOnly Property CallingAppName() As String Implements NavigatorV2.CallingAppName
		Set(ByVal Value As String)
		End Set
	End Property
	'End Property
	
	
	' ***************************************************************** '
	' The following properties are set by Navigator via the             '
	' SetProcessModes method and tell the component what                '
	' mode of operation it is in.                                       '
	' ***************************************************************** '
	
	' The Task that the form is to perform
	' i.e. Add, Edit, View, Delete
	Public ReadOnly Property Task() As Integer Implements NavigatorV2.Task
		Get
		End Get
	End Property
	
	' The status of the Navigator button on the form.
	' i.e. Not Required, Enabled, Disabled
	Public ReadOnly Property Navigate() As Integer Implements NavigatorV2.Navigate
		Get
		End Get
	End Property
	
	' The type of process that is being performed
	' i.e. Generic, Enquiry, Quotation, Make Live
	Public ReadOnly Property ProcessMode() As Integer Implements NavigatorV2.ProcessMode
		Get
		End Get
	End Property
	
	' The type of transaction that is being performed
	' i.e. Quotation, New Business, Renewal, MTA
	Public ReadOnly Property TransactionType() As String Implements NavigatorV2.TransactionType
		Get
		End Get
	End Property
	
	' The effective date that we are working to
	Public ReadOnly Property EffectiveDate() As Date Implements NavigatorV2.EffectiveDate
		Get
		End Get
	End Property
	
	' ***************************************************************** '
	' The following properties need to be set by the                    '
	' component so that Navigator can tell what happened.               '
	' ***************************************************************** '
	
	' The user status of the form on exit
	' i.e. PMOK, PMCancel or PMNavigate.
	' Business objects will normally just set this to PMOK,
	' unless they want the ability to adjust the route through a process.
	Public ReadOnly Property Status() As Integer Implements NavigatorV2.Status
		Get
		End Get
	End Property
	
	' The Completion Status of the Step
	' i.e.Complete , Incomplete, Inactive
	Public ReadOnly Property StepStatus() As String Implements NavigatorV2.StepStatus
		Get
		End Get
	End Property
	
	' PUBLIC Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
	' ***************************************************************** '
	' The following methods are called by Navigator BEFORE              '
	' the component is told to Start its job.                           '
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Name: SetKeys (Navigator Standard Method)
	'
	' Description: Accepts an Array in the format KeyName, KeyValue.
	'              The array will contain the key values required by the
	'              component to do its job.
	'
	' ***************************************************************** '
Public Function SetKeys(ByRef vKeyArray(,) As Object ) As Integer Implements NavigatorV2.SetKeys
		
		Exit Function
	End Function
	
	' ***************************************************************** '
	' Name: SetProcessModes (Navigator Standard Method)
	'
	' Description: Sets the mode of operation for the Component.
	'              The properties are described individually above.
	'
	' ***************************************************************** '
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements NavigatorV2.SetProcessModes
		
		Exit Function
	End Function
	
	' ***************************************************************** '
	' Name: SetStatus (Navigator Standard Method)
	'
	' Description: Set the Process, Map and Step Completion Status.
	' Note:        A Property Get is provided for the Step Status only
	'              as this is the only one which this component can
	'              alter directly.
	' ***************************************************************** '
	Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer Implements NavigatorV2.SetStatus
		
		Exit Function
	End Function
	
	' ***************************************************************** '
	' Name: Start (Navigator Standard Method)
	'
	' Description: Tells the Component to Start its job.
	'
	' ***************************************************************** '
	Public Function Start() As Integer Implements NavigatorV2.Start
		Exit Function
	End Function
	
	
	
	
	' ***************************************************************** '
	' The following methods are called by Navigator AFTER               '
	' the component has done its job.                                   '
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Name: GetKeys (Navigator Standard Method)
	'
	' Description: Accepts an Array in the format KeyName, KeyValue.
	'              The component populates the array with
	'              key values. i.e. If the component is
	'              FindParty it will return the PartyCnt of the Party
	'              selected by the user.
	'
	' ***************************************************************** '
Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer Implements NavigatorV2.GetKeys
		
		Exit Function
	End Function
	
	' ***************************************************************** '
	' Name: GetSummary (Navigator Standard Method)
	'
	' Description: Accepts an Array in the format Summary Level, Summary
	'              Heading, Summary Value.
	'
	'              The component populates the array with any
	'              summary information it wants to return to Navigator.
	'
	'              There are three levels of Summary, Process,
	'              Map Instance and Map.
	' ***************************************************************** '
	Public Function GetSummary(ByRef vSummaryArray As Object) As Integer Implements NavigatorV2.GetSummary
		
		Exit Function
	End Function
	' PUBLIC Methods (End)
End Class
