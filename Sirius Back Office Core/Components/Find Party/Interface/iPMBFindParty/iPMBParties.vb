Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmParties
	Inherits System.Windows.Forms.Form
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmParties"
    Private Const vbFormControlMenu As Integer = 0
	' {* USER DEFINED CODE (Begin) *}
	Private m_lStatus As Integer
	Private m_sPartyType As String = ""
	Private m_bPartyOther As Boolean
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' {* USER DEFINED CODE (End) *}
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	Public ReadOnly Property PartyType() As String
		Get
			
			Return m_sPartyType
			
		End Get
	End Property
	
	' PRIVATE Events (End)
	'RWH(07/07/2000) RSAIB Process 007
	
	Public Property PartyOther() As Boolean
		Get
			Return m_bPartyOther
		End Get
		Set(ByVal Value As Boolean)
			m_bPartyOther = Value
		End Set
	End Property
	
	' PUBLIC Property Procedures (End)
	
	' PRIVATE Events (Begin)
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Try 
			
			m_sPartyType = ""
			
			m_lStatus = gPMConstants.PMEReturnCode.PMCancel
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Try 
			'ECK 18/5/99 Pass back key instead of description
			'    m_sPartyType$ = lvwParties.SelectedItem.Text
			m_sPartyType = lvwParties.FocusedItem.Name
			
			m_lStatus = gPMConstants.PMEReturnCode.PMOk
			Me.Hide()
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	

	Private Sub frmParties_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim oListItem As ListViewItem
        ' Forms load event.
		
		Try 
			
			iPMFunc.CenterForm(Me)
			'ECK 18/5/99 Modify Key to pass to find component
			
			'RWH(07/07/2000) RSAIB Process 007
			If PartyOther Then
				PopulateWithOtherPartyTypes()
			Else
				'Add personal client

                'developer guide no. 
                oListItem = lvwParties.Items.Insert(0, "PC", gSIRLibrary.SIRPartyTypePersonalClientText, 0)
				
				'Add Corporateclient

                'developer guide no. 
                oListItem = lvwParties.Items.Insert(1, "CC", gSIRLibrary.SIRPartyTypeCorporateClientText, 0)
				
				'Add group client

                'developer guide no. 
                oListItem = lvwParties.Items.Insert(2, "GC", gSIRLibrary.SIRPartyTypeGroupClientText, 0)
				
				'We definitely don't want to do this...
				'Add agent
				'Set oListItem = lvwParties.ListItems.Add(4, "P4", SIRPartyTypeAgentText, , 1)
			End If
			
			' SET 13082002 - Only if there is 1 or more items
			If lvwParties.Items.Count > 0 Then
				' Select the first item.
				lvwParties.Items.Item(0).Selected = True
			End If
			' SET 13082002 - End
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
	Private Sub frmParties_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		


        If UnloadMode = 3 Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            Cancel = True
            cmdCancel.PerformClick()
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub
	Private Sub lvwParties_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwParties.DoubleClick
		
		Try 
			
			cmdOK_Click(cmdOK, New EventArgs())
		
		Catch excep As System.Exception
			
			
			
			' Error Section
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwParties_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
			
		End Try
		
	End Sub
	
	Private Function PopulateWithOtherPartyTypes() As Integer
		Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim oListItem As ListViewItem
		
		Const iDESCRIPTION As Integer = 3
		Const iCODE As Integer = 2
		
		
		Try 
			

			m_lReturn = g_oBusiness.GetOtherPartyTypes(vResultArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'RWH(14/09/2000) Make sure array exists before populating list.
			If Information.IsArray(vResultArray) Then

				For iPartyType As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
					' alway ignore "otherparty" code

                    If CStr(vResultArray(iCODE, iPartyType)).Trim() <> "OTHERPARTY" Then

                        oListItem = lvwParties.Items.Insert(0, CStr(vResultArray(iCODE, iPartyType)), CStr(vResultArray(iDESCRIPTION, iPartyType)), 0)
                    End If
				Next iPartyType
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate Party Types ", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateWithOtherPartyTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
			
		End Try
	End Function
End Class
