Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmSelectGISScreen
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmSelectGISScreen"
	
	Private m_lSelectedScreenID As Integer
	Private m_sSelectedDescription As String = ""
	Private m_vResultArray( ,  ) As Object
	
	
	Property SelectedScreenID() As Integer
		Get
			Return m_lSelectedScreenID
		End Get
		Set(ByVal Value As Integer)
			m_lSelectedScreenID = Value
		End Set
	End Property
	
	ReadOnly Property SelectedDescription() As String
		Get
			Return m_sSelectedDescription
		End Get
	End Property
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		m_lSelectedScreenID = 0
		Me.Close()
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
        Try
            m_lSelectedScreenID = CInt(m_vResultArray(0, lstScreens.SelectedIndex))
            m_sSelectedDescription = CStr(m_vResultArray(1, lstScreens.SelectedIndex))
            Me.Hide()
        Catch
        End Try
	End Sub
	
	Public Function PrepareInterface() As Object
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try 
			
			' Get captions from resource file

			Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACScreenForm, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblRiskType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			lblGISSCreen.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACScreenLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			

			cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
			
			' Fill screen list

			lReturn = g_oBusiness.GetGISScreensList(m_vResultArray)
			If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				If Information.IsArray(m_vResultArray) Then
					For iIndex As Integer = 0 To m_vResultArray.GetUpperBound(1)
						lstScreens.Items.Add(CStr(m_vResultArray(1, iIndex)))
						If m_lSelectedScreenID = CDbl(m_vResultArray(0, iIndex)) Then
							lstScreens.SelectedIndex = iIndex
						End If
					Next iIndex
				End If
			End If
		
		Catch excep As System.Exception
			
			
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Prepare Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="PrepareInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Function
	
    Private Sub lstScreens_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lstScreens.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        KeyAscii = 0
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub frmSelectGISScreen_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'developer guide no.293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub

    Private Sub lstScreens_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstScreens.SelectedIndexChanged
        If lstScreens.Text.Trim() <> "" Then
            cmdOK.Enabled = True
        End If
    End Sub
End Class
