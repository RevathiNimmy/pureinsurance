Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend NotInheritable Class General 
	  Implements IDisposable
' ***************************************************************** '
	' Class Name: General
	'
	' Date: 02/07/1998
	'
	' Description: General class to accompany the interface form.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	
	' Constant for the functions to identify
	' which class this is.
Private Const ACClass As String = "General" 
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Private instance of the interface form.
	Private m_frmInterface As Form
	
	' Private instance of the business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	Private m_lClaimID As Integer
	
	' PRIVATE Data Members (End)
	
	' PUBLIC Property Procedures (Begin)
	Public Property ClaimId() As Integer
		Get
			
			Return m_lClaimID
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lClaimID = Value
			
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
	Public Function Initialise(ByRef frmInterface As Form, ByRef oBusiness As Object) As Integer



		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Store the instance of the form into the member.
			m_frmInterface = frmInterface
			
			' Store the instance of the business object
			' into the member.
			m_oBusiness = oBusiness
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
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
				m_oBusiness = Nothing
				m_frmInterface = Nothing
			End If
		End If
		Me.disposedValue = True
	End Sub

	
	' ***************************************************************** '
	' Name: GetInterfaceDetails
	'
	' Description: Gets the interface details and sets the appropriate
	'              sytle.
	'
	' ***************************************************************** '
	Public Function GetInterfaceDetails() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Display all of the lookup details.

            'TODO check at run time 
            'm_lReturn = m_frmInterface.DisplayLookupDetails()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'RWH(09/04/2001) Do this for all tasks to allow edit of  newly added records
			' in add mode.
			' Check the task.
			'    If (m_frmInterface.ClaimMode = PMEdit Or _
			''    m_frmInterface.ClaimMode = PMView) Then
			' Get the interface details from the
			' business object.

            'TODO check at runtime
            'm_lReturn = m_frmInterface.GetBusiness()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get the details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Assign the details from the business object
			' to the interface.

            'TODO check at runtime
            'm_lReturn = m_frmInterface.BusinessToInterface()
			
			' Check for errors
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to assign the details.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'    End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInterfaceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ProcessCommand
	'
	' Description: Determines which action to take on the details
	'              depending upon the task and interface state.
	'
	' ***************************************************************** '
	Public Function ProcessCommand() As Integer
		
		Dim result As Integer = 0
		Dim iMsgResult As DialogResult
		Dim sMessage, sTitle As String
		
		'For Event Description
		Const kEVENT_TYPE_UPDATECLAIM As Integer = 6
		Dim sEventDesc As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			
			' Check the task.

            'Developer Guide No.50
            Dim objfrmInteface As New frmInterface()
            Select Case (objfrmInteface.ClaimMode)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.

                    'If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Dim objfrmInteface As New frmInterface()
                    If objfrmInteface.Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        ' Add the details using the business object.

                        m_lReturn = g_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMView
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.

                    'Developer Guide No.50
                    'If m_frmInterface.Status = gPMConstants.PMEReturnCode.PMCancel Then

                    Dim frmInterface As frmInterface
                    If objfrmInteface.Status = gPMConstants.PMEReturnCode.PMCancel Then



                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        '                End If
                    Else
                        ' Update the details using the business object.

                        m_lReturn = g_oBusiness.Update()
                        If frmInterface.DataChanged Then
                            sEventDesc = Interaction.InputBox("Enter the Event Description", "Event Log", sEventDesc)

                            m_lReturn = g_oBusiness.CreateEvent(kEVENT_TYPE_UPDATECLAIM, sEventDesc, ClaimId)
                            frmInterface.DataChanged = False
                        End If
                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                    End If



            End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
			Return result
			
		End Try
	End Function
	'PUBLIC Methods (End)
	
	
	'PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: DisableForm
	'
	' Description: Sets all of the interface details to the disable
	'              state passed.
	'
	' ***************************************************************** '

	'Private Function DisableForm(ByRef lDisabled As Integer) As Integer
		'
		'Dim result As Integer = 0
		'
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Set all of the forms controls to the disable state.

			'For	Each ctlFormControl As Control In ContainerHelper.Controls(m_frmInterface)
				' Check the type of the control.
				'If TypeOf ctlFormControl Is TextBox Then
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				'ElseIf (TypeOf ctlFormControl Is ComboBox) Then 
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				'ElseIf (TypeOf ctlFormControl Is CheckBox) Then 
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				'ElseIf (TypeOf ctlFormControl Is RadioButton) Then 
					'ControlHelper.SetEnabled(ctlFormControl, Not lDisabled)
				'End If
			'Next ctlFormControl
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			' Error Section.
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			' Log Error.
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface general class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Exit Sub
			'
		'End Try
		
	End Sub
	
	Protected Overrides Sub Finalize()
		Dispose(False)
	End Sub

End Class
