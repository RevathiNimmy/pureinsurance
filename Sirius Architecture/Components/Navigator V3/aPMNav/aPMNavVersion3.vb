Option Strict Off
Option Explicit On
Imports System
Public Interface NavigatorV3
	WriteOnly Property CallingAppName As String
	WriteOnly Property PMAuthorityLevel As Integer
	ReadOnly Property Status As Integer
Function SetKeys(ByRef vKeyArray(,) As Object ) As Integer
	Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
	Function Start() As Integer
Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer
	Function GetSummary(ByRef vSummaryArray As Object) As Integer
End Interface
<System.Runtime.InteropServices.ProgId("NavigatorV3_CoClass_NET.NavigatorV3_CoClass")> _
Public NotInheritable Class NavigatorV3_CoClass 
	Implements NavigatorV3
	' ***************************************************************** '
	' Class Name: NavigatorV3
	'
	' Date: 11/09/1998
	'
	' Description: This Abstract Class does not implement (have any
	'              code for) any of its public properties or methods.
	'
	'              Its purpose is to define the public Properties and
	'              Methods that a Navigator enabled component needs
	'              to Implement so that it can be called via Navigator.
	'
	'              Each compoment which wants to be Navigator Version 3
	'              enabled must add the following line of code to the
	'              top of the class.
	'
	'              Implements aPMNav.NavigatorV3
	'
	'              With this line the component will not compile until
	'              it supports all of the public properties and methods
	'              declared within this class.
	'
	' Edit History:
	'              20/11/1998 RFC Original
	'
	' ***************************************************************** '
	
	' PUBLIC Property Procedures (Begin)
	
	' ***************************************************************** '
	' The following properties are set by Navigator                     '
	' ***************************************************************** '
	
	' The Calling application or component name.
	Public WriteOnly Property CallingAppName() As String Implements NavigatorV3.CallingAppName
		Set(ByVal Value As String)
		End Set
	End Property
	
	' The Authority Level (User, Supervisor or SysAdmin)
	Public WriteOnly Property PMAuthorityLevel() As Integer Implements NavigatorV3.PMAuthorityLevel
		Set(ByVal Value As Integer)
		End Set
	End Property
	
	' ***************************************************************** '
	' The following properties need to be set by the                    '
	' component so that Navigator can tell what happened.               '
	' ***************************************************************** '
	
	' The user status of the form on exit
	' i.e. PMOK, PMCancel or PMNavigate.
	' Business objects will normally just set this to PMOK,
	' unless they want the ability to adjust the route through a process.
	Public ReadOnly Property Status() As Integer Implements NavigatorV3.Status
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
Public Function SetKeys(ByRef vKeyArray(,) As Object ) As Integer Implements NavigatorV3.SetKeys
		
		Exit Function
	End Function
	
	' ***************************************************************** '
	' Name: SetProcessModes (Navigator Standard Method)
	'
	' Description: Sets the mode of operation for the Component.
	'              The properties are described individually above.
	'
	' ***************************************************************** '
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements NavigatorV3.SetProcessModes
		
		Exit Function
	End Function
	
	' ***************************************************************** '
	' Name: Start (Navigator Standard Method)
	'
	' Description: Tells the Component to Start its job.
	'
	' ***************************************************************** '
	Public Function Start() As Integer Implements NavigatorV3.Start
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
Public Function GetKeys(ByRef vKeyArray(,) As Object ) As Integer Implements NavigatorV3.GetKeys
		
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
	Public Function GetSummary(ByRef vSummaryArray As Object) As Integer Implements NavigatorV3.GetSummary
		
		Exit Function
	End Function
	' PUBLIC Methods (End)
End Class
