Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmSelectGIIPolType
	Inherits System.Windows.Forms.Form
	Private Sub frmSelectGIIPolType_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
		End If
	End Sub
	' ***************************************************************** '
	' Form Name: frmSelectGIIPolType
	'
	' Date:
	'
	' Description: allows user to select what kind tranaction they wish to do with
	'              the policy that they have selected
	'
	' Edit History: JSB 06/06/01 - Created
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Const ACClass As String = "frmSelectGIIPolType"
	
	Private m_lStatus As Integer
	Private m_lReturn As Integer
	Private m_lPolicyStatus As Integer
	Private m_lSelectedItemTag As Integer

    Private objCM As MainModule
    Public WriteOnly Property ModuleClass() As MainModule
        Set(ByVal value As MainModule)
            objCM = value
        End Set
    End Property


	Public ReadOnly Property SelectedProcess() As Integer
		Get
			
			Return m_lSelectedItemTag
			
		End Get
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	Public WriteOnly Property PolicyStatus() As Integer
		Set(ByVal Value As Integer)
			
			m_lPolicyStatus = Value
			
		End Set
	End Property
	
	' ***************************************************************** '
	' Name: LoadInterface
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function LoadInterface() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
		
		Catch 
		End Try
		
		
		
		
		result = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadInterface Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	' ***************************************************************** '
	'
	' Name: PopulateListView
	'
	' Description:
	'
	' History: 06/06/2001 JSB - Created.
	'
	' ***************************************************************** '
	Private Function PopulateListView() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Clear the listview box.
			lvwGIIProcessTyes.Items.Clear()
			
			'Depending on the policy status give the user options to select
			Select Case m_lPolicyStatus
				Case GIIPolicyIncomplete
					'NB

                    oListItem = lvwGIIProcessTyes.Items.Add(objcm.GIIProcessTypeNB & "ID", "New Business", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeNB
					
				Case GIIPolicyQuote

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeNB & "ID", "New Business", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeNB
				Case GIIPolicyNBComplete
					'Populate list view

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMaintain & "ID", "Maintain Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMaintain

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMTA & "ID", "Create MTA", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMTA

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeRebroke & "ID", "Rebroke Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeRebroke

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
				Case GIIPolicyRequoteRequired, GIIPolicyExpired, GIIPolicyLapsed, GIIPolicyCoverDefault
					
				Case GIIPolicyRequoted, GIIPolicyPending
					'nb

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeNB & "ID", "New Business", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeNB
				Case GIIPolicyPendingTransmitted

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
				Case GIIPolicyLive
					'Populate list view

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMaintain & "ID", "Maintain Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMaintain

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMTA & "ID", "Create MTA", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMTA

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeRebroke & "ID", "Rebroke Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeRebroke

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
					
				Case GIIPolicyMTAPermanent
					'all but nb
					'Populate list view

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMaintain & "ID", "Maintain Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMaintain

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMTA & "ID", "Create MTA", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMTA

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeRebroke & "ID", "Rebroke Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeRebroke

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
				Case GIIPolicyMTATemporary, GIIPolicyMTACancellation
					'review

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
					
				Case GIIPolicyMTAIncomplete
					'all but nb
					'Populate list view

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMaintain & "ID", "Maintain Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMaintain

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMTA & "ID", "Create MTA", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMTA

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeRebroke & "ID", "Rebroke Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeRebroke

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
					
				Case GIIPolicyMTAReinstatement
					'all but nb
				Case GIIPolicyCancelPending
					'all but nb

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMaintain & "ID", "Maintain Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMaintain

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeMTA & "ID", "Create MTA", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeMTA

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeRebroke & "ID", "Rebroke Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeRebroke

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
				Case GIIPolicyCancelled
					' review

                    oListItem = lvwGIIProcessTyes.Items.Add(objCM.GIIProcessTypeReview & "ID", "Review Policy", "IIcon")
                    oListItem.Tag = objCM.GIIProcessTypeReview
				Case Else
					'shouldn't be anything, but never know
			End Select
			
			If lvwGIIProcessTyes.Items.Count > 0 Then
				' Select the first item.
				lvwGIIProcessTyes.Items.Item(0).Selected = True
				
				cmdOK.Enabled = True
			Else
				cmdOK.Enabled = False
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed listview", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListView", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMCancel
		
		Me.Hide()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		'    If (lvwGIIProcessTyes.SelectedItem = True) Then
		'Set the tag of the selected item

		m_lSelectedItemTag = Convert.ToString(lvwGIIProcessTyes.FocusedItem.Tag)
		'    End If
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		
		Me.Hide()
	End Sub
	

	Private Sub frmSelectGIIPolType_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
            objCM.DisableFormCloseButton(Me.Text)
			
			'    CenterForm (Me)
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			PopulateListView()
			
			
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			'    m_lErrorNumber& = PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub


			
		End Try
		
	End Sub
	
	Private Sub lvwGIIProcessTyes_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwGIIProcessTyes.DoubleClick
		'call the ok click event
		cmdOK_Click(cmdOK, New EventArgs())
	End Sub
End Class
