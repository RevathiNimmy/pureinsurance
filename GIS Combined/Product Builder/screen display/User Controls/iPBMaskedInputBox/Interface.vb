Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 
	' ***************************************************************** '
	' Class:    Interface for iPBMaskedInputBox component
	' Shared:   MultiUse
	' Needs:    frmMaskedInputBox, Standard Sirius Constant modules e.g. gPMConstants
	'
	' Main public class to accompany the interface form. Sets properties in the form
	' set via the calling application. Displays the form, captures data entered and
	' unloads the form.
	'
	' Edit History: CJB 13/11/02 Created
	'
	' ***************************************************************** '
	
	
	Private Const ACClass As String = "Interface"
	
	' TitleBar caption of form
	Private m_sTitleBarCaption As String = ""
	
	' Label on form to indicate what input is required
	Private m_sMaskedInputBoxCaption As String = ""
	
	' Holds the data captured
	Private m_sInputCaptured As String = ""
	
	Public Property TitleBarCaption() As String
		Get
			Return m_sTitleBarCaption
		End Get
		Set(ByVal Value As String)
			m_sTitleBarCaption = Value
		End Set
	End Property
	
	Public Property MaskedInputBoxCaption() As String
		Get
			Return m_sMaskedInputBoxCaption
		End Get
		Set(ByVal Value As String)
			m_sMaskedInputBoxCaption = Value
		End Set
	End Property
	
	Public Property InputCaptured() As String
		Get
			Return m_sInputCaptured
		End Get
		Set(ByVal Value As String)
			m_sInputCaptured = Value
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: DisplayMaskedInputBox (Standard Method)
	'
	' Description: Entry point for the object to start its processing.
	'
	' ***************************************************************** '
	Public Function DisplayMaskedInputBox() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim oForm As frmMaskedInputBox
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oForm = New frmMaskedInputBox()
			
			With oForm
				.TitleBarCaption = m_sTitleBarCaption
				.MaskedInputBoxCaption = m_sMaskedInputBoxCaption
			End With
			
			' Load the form

            'Load(oForm)
			
			' Show the form
			oForm.ShowDialog()
			
			' Get properties back
			With oForm
				InputCaptured = .InputCaptured
			End With
			
			' Unload the form
			oForm.Close()
			
			oForm = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMaskedInputBox failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMaskedInputBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	Shared Sub New()
		MainModule.JustForInvokeMain()
	End Sub
End Class
