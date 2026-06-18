Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: {17/2/98}
	'
	' Description: Main interface.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmInterface"
	
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	
	'***Insert Form Constants***
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	
	' {* USER DEFINED CODE (Begin) *}
	
	' DME Install Directory
	Private m_sDMEDIR As String = ""
	
	' {* USER DEFINED CODE (End) *}
	
	' Declare an instance of the Business object.
	Private m_oBusiness As Object
	
	' Stores the return value for the a
	' function call.
	Private m_lReturn As Integer
	
	
	' Stores the details from the business object.
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PRIVATE Data Members (End)
	
	
    ' PUBLIC Property Procedures (Begin)
    'developer guide no. 7
    Private Const vbFormCode As Integer = 0
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	
	' {* USER DEFINED CODE (Begin) *}
	' {* USER DEFINED CODE (End) *}
	' PUBLIC Property Procedures (End)
	' PRIVATE Property Procedures (Begin)
	
	'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub Status(ByVal Value As Integer)
		'
		' Standard Property.
		'
		' Set the interface exit status.
		'm_lStatus = Value
		'
	'End Sub
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	' PRIVATE Property Procedures (End)
	
	
	' PUBLIC Methods (Begin)
	
	
	' PRIVATE Methods (Begin)
	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
            iPMFunc.CenterForm(Me)
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			
			' Set any other default values to the interface.
			
			' {* USER DEFINED CODE (Begin) *}
			' {* USER DEFINED CODE (End) *}
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DisplayCaptions
	'
	' Description: Display all language specific captions.
	'
	' ***************************************************************** '
	Private Function DisplayCaptions() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			' Display all language specific captions.
			
			'    Me.Caption = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACInterfaceTitle, _
			''        iDataType:=PMResString)
			'
			'    ' Check for an error.
			'    If (Me.Caption = "") Then
			'        ' Failed to get data from the resource file.
			'        DisplayCaptions = PMFalse
			'
			'        ' Log Error.
			'        iPMFunc.LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
			''            "Please check the file exists and the correct captions are available", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="DisplayCaptions"
			'
			'        Exit Function
			'    End If
			
			'    cmdOK.Caption = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACOKButton, _
			''        iDataType:=PMResString)
			'
			'    cmdCancel.Caption = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACCancelButton, _
			''        iDataType:=PMResString)
			'
			'    cmdHelp.Caption = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACHelpButton, _
			''        iDataType:=PMResString)
			'
			'    cmdNavigate.Caption = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACNavigateButton, _
			''        iDataType:=PMResString)
			'
			'    tabMainTab.TabCaption(0) = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACTabTitle1, _
			''        iDataType:=PMResString)
			
			' {* USER DEFINED CODE (Begin) *}
			
			' ************************************************************
			' Enter your code here to display all language specific
			' captions.
			' The GetResData function will allow you to do this.
			'
			' Example:-
			'
			'    lblDesc.Caption = GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACDesc, _
			''        iDataType:=PMResString)
			'
			' NOTE: Replace this section with your new code.
			' ************************************************************
			
			' {* USER DEFINED CODE (End) *}
			
			'***Insert GetRes Calls***
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			Dim sDefaultAnnotation, sTmp, sScanDirectory As String
			
			
			Try 
				
				' get path of scan folder
				m_lReturn = GetDOCRegSettings(vScanDirectory:=sScanDirectory)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					
					'this is no good at all
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Scan Directory.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Me.Hide()
					
				End If
				

				m_oBusiness.ScanDirectory = sScanDirectory
				
				'Get default annotation and set it in the business
				sDefaultAnnotation = "Batch "
				sDefaultAnnotation = sDefaultAnnotation & DateTime.Now.ToString("dd/MM/yy") & " "
				sDefaultAnnotation = sDefaultAnnotation & StringsHelper.Format(DateTime.Now, "hh:mm:ss am/pm")
				
				sTmp = Interaction.InputBox("Enter Description", "Batch Annotation", sDefaultAnnotation)
				

				m_oBusiness.batchannotation = sTmp
				
				'start the business commiting by setting the property to the start value

				m_oBusiness.runstatus = DOCCommitStarted
				
				'enable the progress timer
				tmrProg.Enabled = True
				
				Exit Sub
			
			Catch excep As System.Exception
				
				
				
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
                Exit Sub

            End Try
        End If
    End Sub

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bDOCCommit.Form", vInstanceManager:=PMGetLocalBusiness)
            m_oBusiness = temp_m_oBusiness


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                'sTitle$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFailTitle, _
                'iDataType:=PMResString)

                'sMessage$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFail, _
                'iDataType:=PMResString)

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' {* USER DEFINED CODE (Begin) *}

            'Get the DME install directory
            If m_sDMEDIR.Trim() = "" Then
                m_lReturn = GetDMEDIR(m_sDMEDIR)
            End If

            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then

                'cant just exit if stil commiting

                If m_oBusiness.runstatus = DOCCommitStarted Then

                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no. 7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If


            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            'let the business know it has been cancelled

            If m_oBusiness.runstatus = DOCCommitStarted Then

                m_oBusiness.runstatus = DOCCommitCancelled
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tmrProg_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrProg.Tick

        Static bTotalSet As Boolean


        Try

            'check if commit has finished

            If m_oBusiness.runstatus = DOCCommitFinished Then

                'final progress update


                proCommit.Value = m_oBusiness.DocsDone + m_oBusiness.DocsFailed


                lblDone.Text = m_oBusiness.DocsDone

                lblFailed.Text = m_oBusiness.DocsFailed

                'Advise user if any left uncommitted

                If m_oBusiness.DocsFailed > 0 Then


                    MessageBox.Show(m_oBusiness.DocsFailed & " Documents failed to commit." & Strings.Chr(10).ToString() & "Please try again later.", "Commit Batch")
                End If

                'Advise user as to commit success


                If m_oBusiness.DocsDone = m_oBusiness.DocsTotal Then

                    If m_oBusiness.DocsTotal > 0 Then
                        MessageBox.Show("All documents were successfully committed.", "Commit Batch")
                    Else
                        MessageBox.Show("Failed to commit any documents.", "Commit Batch")
                    End If
                End If

                Me.Hide()

            End If

            'check if was locked by another commit process

            If m_oBusiness.runstatus = DOCCommitLocked Then

                MessageBox.Show("                SCANSTATION IS CURRENTLY LOCKED." & Strings.Chr(10).ToString() & Strings.Chr(10).ToString() & "If another ScanStation is committing the please try again later." & Strings.Chr(10).ToString() & "Otherwise select the 'Remove Commit Lock' option on the server.", "Commit Batch")

                Me.Hide()

            End If


            'update progress bar

            If (Not bTotalSet) And (m_oBusiness.DocsTotal > 0) Then

                bTotalSet = True

                proCommit.Minimum = 0

                proCommit.Maximum = m_oBusiness.DocsTotal

            End If

            If bTotalSet Then



                proCommit.Value = m_oBusiness.DocsDone + m_oBusiness.DocsFailed


                lblTotal.Text = m_oBusiness.DocsTotal

                lblDone.Text = m_oBusiness.DocsDone

                lblFailed.Text = m_oBusiness.DocsFailed

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tmrProg_Timer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		MemoryHelper.ReleaseMemory()
	End Sub
End Class