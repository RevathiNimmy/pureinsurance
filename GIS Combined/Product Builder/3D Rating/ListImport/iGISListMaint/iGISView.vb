Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmView
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Module Name: frmView
	'
	' Date: 28/06/2002
	'
	' Description:
	'
	' Edit History:
	'   28/06/2002 SJP  - Tidied up after merge from Carole Nash
	' ***************************************************************** '
	
    Private m_vData(,) As Object
    Private frmView As frmView
	
	' ***************************************************************** '
	'
	' Name: SetData
	'
	' Description:  This will set the private variable
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Public Sub SetData(ByRef vData( ,  ) As Object)
		
		Try 
			
			m_vData = vData
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set data", vApp:=ACApp, vClass:=ACClass, vMethod:="SetData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: cmdOK_Click()
	'
	' Description:  This will unload the form
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Try 
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unload screen", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	'
	' Name: Form_Load()
	'
	' Description:  This will set up the list items when
	'                   it runs
	'
	' History: 28/06/2002 SJP - tidied up
	'
	' ***************************************************************** '

	Private Sub frmView_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Dim oListItem As ListViewItem
		
		Try 
			
			'set up list view columns
			lvwData.Columns.Clear()
			
			With lvwData
				'add standard columns
				.Columns.Add("Code", 94)
				.Columns.Add("Description", 94)
				
				'add non standard columns
				For i As Integer = 2 To m_vData.GetUpperBound(0)
					.Columns.Add("Field" & i - 1, 94)
				Next i
				
			End With
			
			lvwData.View = View.Details
			
			'add data
			
			'cycle records
			For i As Integer = 0 To m_vData.GetUpperBound(1)
				
				'add code
				oListItem = lvwData.Items.Add(CStr(m_vData(0, i)))
				
				'add description
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vData(1, i))
				
				'cycle additional fields
				For j As Integer = 2 To m_vData.GetUpperBound(0)
					ListViewHelper.GetListViewSubItem(oListItem, j).Text = CStr(m_vData(j, i))
				Next j
				
			Next 
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
		End Try
		
	End Sub
End Class
