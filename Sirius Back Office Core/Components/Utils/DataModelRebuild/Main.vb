Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Module MainModule
	
	Public Const ACApp As String = "RebuildDatamodel"
	Public Const ACClass As String = "RebuildDatamodel"
	
	' constants for user name and password - apparently, these should be available on all systems...
    Public Const m_sUserName As String = "sirius"
    Public Const m_sPassword As String = ""
	
	Public g_sUserName As String
	
	Public Sub Main()
		Try
		
		Dim ofrmMain As frmMain
		Dim sDataModel As String
		
        'If VB.Command() = vbNullString Then
        '	sDataModel = "ALL"
        'Else
        '	sDataModel = VB.Command()
        'End If
		
		If Trim(sDataModel) = "" Then
			sDataModel = "ALL"
		End If
		
		ofrmMain = New frmMain
		
		ofrmMain.DataModel = sDataModel
		
        'System.Windows.Forms.Application.Run(ofrmMain)
        'System.Windows.Forms.Application.DoEvents()
        If VB.Command().ToUpper() <> "PBIE" Then
            ofrmMain.Show()
        End If

		Call ofrmMain.RegenerateAllModels()
        Call ofrmMain.CreateAndSavePartyHistorySchema()
		ofrmMain.Hide()
		
		'UPGRADE_NOTE: Object ofrmMain may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		ofrmMain = Nothing
		
		End
		

		
		Catch ex As Exception
		
		' Log Error.
        SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run datamodel rebuild process", vApp:=ACApp, vClass:=ACClass, vMethod:="Main", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
		Exit Sub
		
		End Try
	End Sub
End Module
