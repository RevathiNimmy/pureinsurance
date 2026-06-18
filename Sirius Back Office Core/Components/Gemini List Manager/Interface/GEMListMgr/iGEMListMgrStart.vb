Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
Module MainModule
	Private m_lReturn As Integer
	
	Public Const ACApp As String = "iGEMListMgrStart"
	
	
	Public Sub Main()
        Dim PMEdit As Object = Nothing
		Dim PMTrue As Integer
		
		Dim sMessage As String = ""
		
		Dim sTitle As String = "Gemini List Maintenance"
		
		Dim oListMgr As New iGEMListMgr.Interface_Renamed
		If oListMgr Is Nothing Then
			sMessage = "Unable to create instance of List Maintenance"
			MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
			Exit Sub
		End If
		

		m_lReturn = CInt(CType(oListMgr, SSP.S4I.Interfaces.ILocalInterface).Initialise())
		If m_lReturn <> PMTrue Then
			sMessage = "Unable to initialise List Maintenance"
			MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
			oListMgr = Nothing
			Exit Sub
		End If
		

		m_lReturn = CInt(oListMgr.SetProcessModes(vTask:=PMEdit))
		If m_lReturn <> PMTrue Then
			sMessage = "Unable to initialise List Maintenance"
			MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
			oListMgr = Nothing
			Exit Sub
		End If
		

		m_lReturn = CInt(oListMgr.Start())
		If m_lReturn <> PMTrue Then
			sMessage = "Unable to initialise List Maintenance"
			MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK)
			oListMgr = Nothing
			Exit Sub
		End If
		

        oListMgr.Dispose()
        oListMgr = Nothing
		
	End Sub
End Module