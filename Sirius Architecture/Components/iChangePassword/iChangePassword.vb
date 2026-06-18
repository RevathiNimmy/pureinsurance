Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: Interface
	'
	' Date: 21/01/1997
	'
	' Description: Main public class to accompany the interface form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
Private Const ACClass As String = "Interface" 
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	
	' {* USER DEFINED CODE (Begin) *}
	
	Private m_iLanguageID As Integer
	Private m_sOldPassword As String = ""
	Private m_sNewPassword As String = ""
	
	' {* USER DEFINED CODE (End) *}
	
	' Determines weather or not the interface
	' has been cancelled.
	Private m_bCancelled As Boolean
	
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
	
	Public ReadOnly Property Cancelled() As Boolean
		Get
			
			' Standard Property.
			
			' Return the cancelled status.
			Return m_bCancelled
			
		End Get
	End Property
	
	' {* USER DEFINED CODE (Begin) *}
	Public WriteOnly Property LanguageID() As Integer
		Set(ByVal Value As Integer)
			
			' Set the language ID.
			m_iLanguageID = Value
			
		End Set
	End Property
	
	Public WriteOnly Property OldPassword() As String
		Set(ByVal Value As String)
			
			' Set the old password.
			m_sOldPassword = Value
			
		End Set
	End Property
	
	Public ReadOnly Property NewPassword() As String
		Get
			
			' Return the new password.
			Return m_sNewPassword
			
		End Get
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
			
			
			Return GeneralConst.PMTrue
		
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
		Dim frmInterface As Object
		
		 
			
			result = GeneralConst.PMTrue
			
			' Assign the parameters to the interface properties.
			With frmInterface

				.CallingAppName = m_sCallingAppName
				
				' {* USER DEFINED CODE (Begin) *}
				

				.LanguageID = m_iLanguageID

				.OldPassword = m_sOldPassword
				
				' {* USER DEFINED CODE (End) *}
			End With
			
			' Load the instance of the interface into memory.

            'developer guide no. 68
            'Load(frmInterface)
			
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
		Dim frmInterface As Object
		
		 
			
			result = GeneralConst.PMTrue
			
			' Assign the property members from the interface parameters.
			With frmInterface

				m_bCancelled = .Cancelled
				
				' {* USER DEFINED CODE (Begin) *}
				

				m_sNewPassword = .NewPassword
				
				' {* USER DEFINED CODE (End) *}
			End With
			
			' Unload and destroy the instance of the interface
			' from memory.

            'developer guide no. 68
            'Unload(frmInterface)
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
		Dim frmInterface As Object
		
		 
			
			result = GeneralConst.PMTrue
			
			' Display the interface.

			frmInterface.Show(lDisplayState)
			
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

