Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: Interface
	'
	' Date: 25/04/1997
	'
	' Description: Main public class to accompany the interface form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
Private Const ACClass As String = "Interface" 
    'developer guide no.50
    Dim frmInterface As frmInterface
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	' {* USER DEFINED CODE (Begin) *}
	Private m_sNavigatorTitle As String = ""
	Private m_sDecisionTitle As String = ""
	Private m_sDecisionText As String = ""
	' {* USER DEFINED CODE (End) *}
	
	' Stores the exit status of the interface.
	Private m_lStatus As Integer
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	' PRIVATE Data Members (End)
	
	' PUBLIC Property Procedures (Begin)
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public ReadOnly Property Task() As Integer
		Get
			
			' Standard Property.
			
			' Return the task.
			Return m_iTask
			
		End Get
	End Property
	
	Public ReadOnly Property Navigate() As Integer
		Get
			
			' Standard Property.
			
			' Return the navigate flag.
			Return m_lNavigate
			
		End Get
	End Property
	
	Public ReadOnly Property ProcessMode() As Integer
		Get
			
			' Standard Property.
			
			' Return the process mode.
			Return m_lProcessMode
			
		End Get
	End Property
	
	Public ReadOnly Property TransactionType() As String
		Get
			
			' Standard Property.
			
			' Return the type of business.
			Return m_sTransactionType
			
		End Get
	End Property
	
	Public ReadOnly Property EffectiveDate() As Date
		Get
			
			' Standard Property.
			
			' Return the effective date.
			Return m_dtEffectiveDate
			
		End Get
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	
	Public WriteOnly Property DecisionTitle() As String
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sDecisionTitle = Value
			
		End Set
	End Property
	
	Public WriteOnly Property DecisionText() As String
		Set(ByVal Value As String)
			
			' Set the objects parameter value.
			m_sDecisionText = Value
			
		End Set
	End Property
	' {* USER DEFINED CODE (End) *}
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
		Dim LogMessagePopup( ,  ,  ,  ,  ,  ,  ) As Object
		
		Try 
			
			result = GeneralConst.PMTrue
			
			' Initialise the process modes.
			m_iTask = GeneralConst.PMView
			m_lNavigate = GeneralConst.PMNavigateNotRequired
			m_lProcessMode = GeneralConst.PMProcessModeGeneric
			m_sTransactionType = GeneralConst.PMTypeOfBusinessGeneric
			m_dtEffectiveDate = DateTime.Now
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = GeneralConst.PMError
			
			' Log Error.
			Dim tempAuxVar As Object = LogMessagePopup(GeneralConst.PMLogOnError, CInt("Failed to initialise the object"), CInt(ACApp), CInt(ACClass), CInt("Initialise"), Information.Err().Number, CInt(excep.Message))
			
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

	
	' ***************************************************************** '
	' Name: SetKeys (Standard Method)
	'
	' Description: Stores all of the parameter members with the key
	'              array.
	'
	' ***************************************************************** '
Public Function SetKeys(ByRef vKeyArray( , ) As Object ) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = GeneralConst.PMTrue
			
			' Check we have a vaild array.
			If Not Information.IsArray(vKeyArray) Then
				Return GeneralConst.PMFalse
			End If
			
			' Step through the key array.
			For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
				' Assign the parameter member with the
				' correct key array item.
				
				' {* USER DEFINED CODE (Begin) *}
				

				Select Case CStr(vKeyArray(GeneralConst.PMKeyName, lRow)).Trim()
					Case GeneralConst.PMKeyNameDecisionTitle

						m_sDecisionTitle = CStr(vKeyArray(GeneralConst.PMKeyValue, lRow)).Trim()
						
					Case GeneralConst.PMKeyNameDecisionText

						m_sDecisionText = CStr(vKeyArray(GeneralConst.PMKeyValue, lRow)).Trim()
				End Select
				
				' {* USER DEFINED CODE (End) *}
			Next lRow
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = GeneralConst.PMError
			
			' Log Error Message
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetKeys (Standard Method)
	'
	' Description: Stores all of the key array with the parameter
	'              members.
	'
	' ***************************************************************** '
Public Function GetKeys(ByRef vKeyArray( , ) As Object ) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = GeneralConst.PMTrue
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Initialise the key array with the number of
			' keys needed to be returned.
			' Note: Remember arrays are zero based.
			ReDim vKeyArray(1, 1)
			
			' Assign the key array with the parameter members.

			vKeyArray(GeneralConst.PMKeyName, 0) = GeneralConst.PMKeyNameDecisionTitle

			vKeyArray(GeneralConst.PMKeyValue, 0) = m_sDecisionTitle

			vKeyArray(GeneralConst.PMKeyName, 1) = GeneralConst.PMKeyNameDecisionText

			vKeyArray(GeneralConst.PMKeyValue, 1) = m_sDecisionText
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = GeneralConst.PMError
			
			' Log Error Message
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetSummary (Standard Method)
	'
	' Description: Stores all of the summary array with the parameter
	'              members.
	'
	' ***************************************************************** '
	Public Function GetSummary(ByRef vSummaryArray( ,  ) As Object) As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = GeneralConst.PMTrue
			
			' {* USER DEFINED CODE (Begin) *}
			
			' Initialise the summary array with the number of
			' items needed to be returned.
			' Note: Remember arrays are zero based.
			Dim vKeyArray(1, 0) As Object
			
			' Assign the key array with the parameter members.

			vSummaryArray(GeneralConst.PMKeyName, 0) = GeneralConst.PMKeyNameNavigatorTitle1

			vSummaryArray(GeneralConst.PMKeyValue, 0) = m_sNavigatorTitle
			
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = GeneralConst.PMError
			
			' Log Error Message
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: SetProcessModes (Standard Method)
	'
	' Description: Set the optional process modes.
	'
	' ***************************************************************** '
	Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = GeneralConst.PMTrue
			
			' Assign the process modes to the property members.
			

			If Not Information.IsNothing(vTask) Then

				m_iTask = CInt(vTask)
			End If
			

			If Not Information.IsNothing(vNavigate) Then

				m_lNavigate = CInt(vNavigate)
			End If
			

			If Not Information.IsNothing(vProcessMode) Then

				m_lProcessMode = CInt(vProcessMode)
			End If
			

			If Not Information.IsNothing(vTransactionType) Then

				m_sTransactionType = CStr(vTransactionType)
			End If
			

			If Not Information.IsNothing(vEffectiveDate) Then

				m_dtEffectiveDate = CDate(vEffectiveDate)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = GeneralConst.PMError
			
			' Log Error Message
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: Start (Standard Method)
	'
	' Description: Entry point for the object to start its processing.
	'
	' ***************************************************************** '
	Public Function Start() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = GeneralConst.PMTrue
			
			' Starts the interface processing.
			m_lReturn = ProcessInterface()
			
			' Check for errors.
			If m_lReturn <> GeneralConst.PMTrue Then
				' Failed to process the interface.
				result = GeneralConst.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = GeneralConst.PMError
			
			' Log Error.
			iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	' PUBLIC Methods (End)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: ProcessInterface (Standard Method)
	'
	' Description: Calls the appropriate methods to process the
	'              interface.
	'
	' ***************************************************************** '
	Private Function ProcessInterface() As Integer
		
		Dim result As Integer = 0
		 
			
			result = GeneralConst.PMTrue
			
			' Load the interface into memory.
			m_lReturn = LoadInterface()
			
			' Check for errors.
			If m_lReturn <> GeneralConst.PMTrue Then
				' Failed to load the interface.
				Return GeneralConst.PMFalse
			End If
			
			' Display the interface.
			m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)
			
			' Check for errors.
			If m_lReturn <> GeneralConst.PMTrue Then
				' Failed to display the inteface.
				result = GeneralConst.PMFalse
			End If
			
			' Destroy the interface from memory.
			m_lReturn = UnLoadInterface()
			
			' Check for errors.
			If m_lReturn <> GeneralConst.PMTrue Then
				' Failed to unload the interface.
				result = GeneralConst.PMFalse
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: LoadInterface (Standard Method)
	'
	' Description: Loads the instance of the interface into memory and
	'              passes the parameters in.
	'
	' ***************************************************************** '
	Private Function LoadInterface() As Integer
		
		Dim result As Integer = 0
		 
			
			result = GeneralConst.PMTrue
			
			' Assign the parameters to the interface properties.
			With frmInterface
				.CallingAppName = m_sCallingAppName
				.Task = m_iTask
				.Navigate = m_lNavigate
				.ProcessMode = m_lProcessMode
				.TransactionType = m_sTransactionType
				.EffectiveDate = m_dtEffectiveDate
				
				' {* USER DEFINED CODE (Begin) *}
				.DecisionTitle = m_sDecisionTitle
				.DecisionText = m_sDecisionText
				' {* USER DEFINED CODE (End) *}
			End With
			
			' Load the instance of the interface into memory.
			Dim tempLoadForm As frmInterface = frmInterface
			
			' Check if we have had an error so far.
			If frmInterface.ErrorNumber = GeneralConst.PMFalse Then
				' We have already encountered an error,
				' so we MUST return the error.
				result = frmInterface.ErrorNumber
			End If
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: UnLoadInterface (Standard Method)
	'
	' Description: Unloads the instance of the interface from memory.
	'
	' ***************************************************************** '
	Private Function UnLoadInterface() As Integer
		
		Dim result As Integer = 0
		 
			
			result = GeneralConst.PMTrue
			
			' Assign the property members from the interface parameters.
			With frmInterface
				m_lStatus = .Status
				
				' {* USER DEFINED CODE (Begin) *}
				' {* USER DEFINED CODE (End) *}
			End With
			
			' Unload and destroy the instance of the interface
			' from memory.
			frmInterface.Close()
			frmInterface = Nothing
			
			Return result
		
	End Function
	
	' ***************************************************************** '
	' Name: ShowInterface (Standard Method)
	'
	' Description: Displays the instance of the interface using the
	'              display state.
	'
	' ***************************************************************** '
	Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer
		
		Dim result As Integer = 0
		 
			
			result = GeneralConst.PMTrue
			
			' Display the interface.
			VB6.ShowForm(frmInterface, lDisplayState)
			
			If lDisplayState = FormShowConstants.Modal Then
				' Check for any form errors.
				If frmInterface.ErrorNumber <> 0 Then
					result = frmInterface.ErrorNumber
				End If
			End If
			
			Return result
		
	End Function
	'PRIVATE Methods (End)
	
	
	Public Sub New()
		MyBase.New()

		' Class Initialise Event.
		
		'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
		'Try 
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			' Log Error Message
			'Dim tempAuxVar As Object = LogMessagePopup(GeneralConst.PMLogOnError, CInt("Failed to initialise the interface entry class"), CInt(ACApp), CInt(ACClass), CInt("Class_Initialise"), Information.Err().Number, CInt(excep.Message))
			'
			'Exit Sub
			'
		'End Try
		
	End Sub
	
	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

	Shared Sub New()
		MainModule.JustForInvokeMain()
	End Sub
End Class

