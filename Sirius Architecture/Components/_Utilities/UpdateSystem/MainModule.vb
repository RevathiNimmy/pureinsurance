Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Module MainModule
	' Edit History:
	
	' DAK060100 - Licencing changes
	' ***************************************************************** '
	
	
	Public Const ACApp As String = "UpdateSystem"
	Private Const ACClass As String = "MainModule"
	
	Private m_lResult As Integer
	Private Const m_sSQL As String = "UPDATE PMSystem SET system_name = {system_name}, licence_limit = 0, licence_key = '' WHERE system_id = 1 AND system_name <> {system_name}"
	
	' RDC 21022002 SQL to update licence key
	Private Const m_sLicenceSQL As String = "UPDATE PMSystem SET licence_key = {licence_key}, licence_limit = 5 WHERE system_name = {system_name}"
	
	Public Const ACGetICCSStored As Boolean = True
	Public Const ACGetICCSName As String = "GetICCS"
    Public Const ACGetICCSSQL As String = "spu_pm_iccs"
	
	Public g_iUserID As Short
	Public g_sUsername As String
	Public g_iSourceID As Short
	Public g_iLanguageID As Short
	Public g_sCallingAppName As String
	
	
	
	' end
	
	'UPGRADE_WARNING: Application will terminate when Sub Main() finishes. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="E08DDC71-66BA-424F-A612-80AF11498FF8"'
	Public Sub Main()
		
		' RDC 21022002 no longer required as SetupDB.exe now adds the PMSystem record
		'    m_lResult = UpdateSystem()
		
		'    If (m_lResult <> PMTrue) Then
		'        MsgBox "System Record update in Sirius Architecture Database failed", vbOKOnly + vbInformation
		'        End
		'    End If
		
		' RDC 21022002 add licence to PMSystem for local system name
		m_lResult = AddSystemLicence()
		
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			MsgBox("System Record update in Sirius Architecture Database failed", MsgBoxStyle.OKOnly + MsgBoxStyle.Information)
			End
		End If
		
		m_lResult = AddLicences()
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			MsgBox("Licence update in Sirius Architecture Database failed", MsgBoxStyle.OKOnly + MsgBoxStyle.Information)
			End
		End If
		
		End
		
	End Sub
	
	Function AddSystemLicence() As Integer
		
		Dim iLicenceLimit As Short
		
		Dim sLicenceKey As String
		Dim sSystem As String
		Dim sICCS As String
		
		Dim vResult As Object
		
		Dim oDatabase As dPMDAO.Database
		
		Try
		
		m_lResult = GetSystemNameNoSID(sSystem)
		
		oDatabase = New dPMDAO.Database
		
		' Open the Database
		' RDC 27062002 use Comp Serv to open database
		m_lResult = NewDatabase("", 1, 1, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase)
		
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			'UPGRADE_NOTE: Object oDatabase may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			oDatabase = Nothing
			AddSystemLicence = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		oDatabase.Parameters.Clear()
		
		m_lResult = oDatabase.Parameters.Add(sName:="ICCS", vValue:=sICCS, idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
		
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			oDatabase.CloseDatabase()
			'UPGRADE_NOTE: Object oDatabase may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			oDatabase = Nothing
			
			AddSystemLicence = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		' Execute SQL Statement
		m_lResult = oDatabase.SQLAction(sSQL:=ACGetICCSSQL, sSQLName:=ACGetICCSName, bStoredProcedure:=ACGetICCSStored)
		
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			oDatabase.CloseDatabase()
			'UPGRADE_NOTE: Object oDatabase may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			oDatabase = Nothing
			
			AddSystemLicence = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		sICCS = oDatabase.Parameters.Item("ICCS").Value
		iLicenceLimit = 5
		
		m_lResult = GenLicenceKey(sLicenceKey:=sLicenceKey, sICCS:=sICCS, iLicenceLimit:=iLicenceLimit)
		
		oDatabase.Parameters.Clear()
		
		m_lResult = oDatabase.Parameters.Add(sName:="licence_key", vValue:=CObj(sLicenceKey), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
		
		m_lResult = oDatabase.Parameters.Add(sName:="system_name", vValue:=CObj(sSystem), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
		
		
		m_lResult = oDatabase.SQLAction(sSQL:=m_sLicenceSQL, sSQLName:="Wibble", bStoredProcedure:=False)
		
		AddSystemLicence = m_lResult
		
		m_lResult = oDatabase.CloseDatabase
		
		'UPGRADE_NOTE: Object oDatabase may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		oDatabase = Nothing
		
		 
		
		Catch ex As Exception
		
		AddSystemLicence = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        SharedFiles.iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSystemLicence Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSystemLicence", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
		End Try
		
	End Function
	
	Function UpdateSystem() As Integer
		
		Dim sSystem As String
		Dim oDatabase As dPMDAO.Database
		
		Try
		
		UpdateSystem = gPMConstants.PMEReturnCode.PMTrue
		
		m_lResult = GetSystemNameNoSID(sSystem)
		
		oDatabase = New dPMDAO.Database
		
		' Open the Database
		' RDC 27062002 use Comp Serv to open database
		m_lResult = NewDatabase("", 1, 1, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oDatabase)
		
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			'UPGRADE_NOTE: Object oDatabase may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			oDatabase = Nothing
			UpdateSystem = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		oDatabase.Parameters.Clear()
		
		m_lResult = oDatabase.Parameters.Add(sName:="system_name", vValue:=CObj(sSystem), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
		
		
		m_lResult = oDatabase.SQLAction(sSQL:=m_sSQL, sSQLName:="Wibble", bStoredProcedure:=False)
		
		UpdateSystem = m_lResult
		
		m_lResult = oDatabase.CloseDatabase
		
		'UPGRADE_NOTE: Object oDatabase may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		oDatabase = Nothing
		
		 
		
		Catch ex As Exception 
		
		UpdateSystem = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        SharedFiles.iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSystem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSystem", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
		
		End Try
		
	End Function
	
	' ***************************************************************** '
	'
	' Name: AddLicences
	'
	' Description:
	'
	' History: 07/01/2000 DAK - Created.
	'
	' DAK160500 - Change spelling on CommonDialog box
	' ***************************************************************** '
	Private Function AddLicences() As Integer
		Dim oLicenceAdmin As Object
		Dim sFileName As String
		Dim sDir As String
		Dim sCurDir As String
		Dim vProductArray As Object
		Dim oForm As frmDialog
		'UPGRADE_NOTE: CommonDialog variable has been upgraded to a different type Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="99C3AB28-CC2C-4BA5-9E2F-620E01690355"'
		Dim oDialogOpen As System.Windows.Forms.OpenFileDialog
		Dim sCmdLine As String
		
		On Error GoTo Err_AddLicences
		
		AddLicences = gPMConstants.PMEReturnCode.PMTrue
		
		sCmdLine = VB.Command()
		If FileExists(sCmdLine) Then
			sFileName = sCmdLine
		Else
			
			oForm = New frmDialog
			
			sFileName = "PMLicence.ini"
			sCurDir = CurDir()
			sDir = "C:\"
			ChDrive(sDir)
			ChDrive(sCurDir)
			
			'UPGRADE_ISSUE: MSComDlg.CommonDialog oDialog was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6B85A2A7-FE9F-4FBE-AA0C-CF11AC86A305"'
            'oDialog = oForm.dlgBrowse
            oDialogOpen = oForm.dlgBrowseOpen
			
            'With oDialog
            With oDialogOpen
                'UPGRADE_WARNING: The CommonDialog CancelError property is not supported in Visual Basic .NET. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8B377936-3DF7-4745-AA26-DD00FA5B9BE1"'

                'Not getting alternative
                '.CancelError = True

                .FileName = sDir & sFileName
                'UPGRADE_WARNING: Filter has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
                .Filter = "Licence Information (*.ini)|*.ini"
                .FilterIndex = 1
                .Title = "Select Licence File"
                'UPGRADE_WARNING: FileOpenConstants constant FileOpenConstants.cdlOFNHideReadOnly was upgraded to OpenFileDialog.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
                'UPGRADE_WARNING: MSComDlg.CommonDialog property oDialog.Flags was upgraded to oDialogOpen.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
                'UPGRADE_WARNING: FileOpenConstants constant FileOpenConstants.cdlOFNHideReadOnly was upgraded to OpenFileDialog.ShowReadOnly which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
                .ShowReadOnly = False
                'UPGRADE_WARNING: MSComDlg.CommonDialog property oDialog.Flags was upgraded to oDialogOpen.CheckFileExists which has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="DFCDE711-9694-47D7-9C50-45A99CD8E91E"'
                .CheckFileExists = True
                .CheckPathExists = True
                'UPGRADE_ISSUE: Constant cdlOFNExplorer was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="55B59875-9A95-4B71-9D6A-7C294BF7139D"'
                'UPGRADE_ISSUE: MSComDlg.CommonDialog property oDialog.Flags was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                'Not getting alternative
                '.Flags = MSComDlg.FileOpenConstants.cdlOFNExplorer
                '.Flags = cdlOFNHideReadOnly Or cdlOFNFileMustExist Or cdlOFNExplorer
                'UPGRADE_ISSUE: MSComDlg.CommonDialog property oDialog.MaxFileSize was not upgraded. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="CC4C7EC0-C903-48FC-ACCC-81861D12DA4A"'
                'Not getting alternative
                '.MaxFileSize = 1024

                .ShowDialog()
                sFileName = .FileName
            End With

            'UPGRADE_NOTE: Object oDialog may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            'oDialog = Nothing
            oDialogOpen = Nothing
            'UPGRADE_NOTE: Object oForm may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            oForm = Nothing
			
		End If
		
        oLicenceAdmin = CreateLateBoundObject("bPMLicenceAdmin.LicenceAdmin")
		
		' Call the initialise method.
		'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.Initialise. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		m_lResult = oLicenceAdmin.Initialise()
		If (m_lResult <> gPMConstants.PMEReturnCode.PMTrue) Then
			' Log Error.
            SharedFiles.iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bPMLicenceAdmin.LicenceAdmin", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLicences")
			
			AddLicences = gPMConstants.PMEReturnCode.PMFalse
			If Not oLicenceAdmin Is Nothing Then
				'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oLicenceAdmin.Dispose()
				'UPGRADE_NOTE: Object oLicenceAdmin may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				oLicenceAdmin = Nothing
			End If
			Exit Function
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.GetProducts. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		m_lResult = oLicenceAdmin.GetProducts(vProductArray)
		If m_lResult <> gPMConstants.PMEReturnCode.PMTrue Then
			' Log Error.
            SharedFiles.iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product licence information", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLicences")
			
			AddLicences = gPMConstants.PMEReturnCode.PMFalse
			If Not oLicenceAdmin Is Nothing Then
				'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oLicenceAdmin.Dispose()
				'UPGRADE_NOTE: Object oLicenceAdmin may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				oLicenceAdmin = Nothing
			End If
			Exit Function
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.ReadLicenceFile. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		m_lResult = oLicenceAdmin.ReadLicenceFile(sFileName)
		If m_lResult <> gPMConstants.PMEReturnCode.PMTrue Then
			' Log Error.
            SharedFiles.iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update licence information", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLicences")
			
			AddLicences = gPMConstants.PMEReturnCode.PMFalse
			If Not oLicenceAdmin Is Nothing Then
				'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                oLicenceAdmin.Dispose()
				'UPGRADE_NOTE: Object oLicenceAdmin may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
				oLicenceAdmin = Nothing
			End If
			Exit Function
		End If
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		oLicenceAdmin.Dispose()
		'UPGRADE_NOTE: Object oLicenceAdmin may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		oLicenceAdmin = Nothing
		
		Exit Function
		
Err_AddLicences: 
		
		' A Drive unavailable so start looking in current directory
		If Err.Number = 68 Then
			If Left(sCurDir, 1) = "A" Then
				sCurDir = "C:\"
			End If
			
			If Right(sCurDir, 1) = "\" Then
				sDir = sCurDir
			Else
				sDir = sCurDir & "\"
			End If
			
			Resume Next
		End If
		
		' Cancel button pressed so
		'UPGRADE_WARNING: The CommonDialog CancelError property is not supported in Visual Basic .NET. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="8B377936-3DF7-4745-AA26-DD00FA5B9BE1"'
		If Err.Number = DialogResult.Cancel Then
			'UPGRADE_NOTE: Object oDialog may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
            'oDialog = Nothing
            oDialogOpen = Nothing
			'UPGRADE_NOTE: Object oForm may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			oForm = Nothing
			Exit Function
		End If
		
		AddLicences = gPMConstants.PMEReturnCode.PMError
		
		' Log Error Message
        SharedFiles.iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLicences Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLicences", vErrNo:=Err.Number, vErrDesc:=Err.Description)
		
		If Not oLicenceAdmin Is Nothing Then
			'UPGRADE_WARNING: Couldn't resolve default property of object oLicenceAdmin.Terminate. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            oLicenceAdmin.Dispose()
			'UPGRADE_NOTE: Object oLicenceAdmin may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
			oLicenceAdmin = Nothing
		End If
		
		'UPGRADE_NOTE: Object oDialog may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        'oDialog = Nothing
        oDialogOpen = Nothing
		'UPGRADE_NOTE: Object oForm may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		oForm = Nothing
		
		Exit Function
	End Function
	
	
	' ***************************************************************** '
	' Name: GenLicenceKey
	'
	' Description: Encrypts the system name, ICCS code and
	'              licence limit to generate the licence key.
	'
	' ***************************************************************** '
	'DAK140400 - remove system name
	Private Function GenLicenceKey(ByRef sLicenceKey As String, ByRef sICCS As String, ByRef iLicenceLimit As Short) As Integer
		Dim lErrorValue As Integer
		Dim sLicence As String
		
		
		
		GenLicenceKey = gPMConstants.PMEReturnCode.PMTrue
		
		If iLicenceLimit = 0 Then
			sLicenceKey = ""
			Exit Function
		End If
		
		'DAK240100
		' Ignore case of system name
		'DAK140400 replace system name with PMProduct
        sLicence = CStr(iLicenceLimit) & sICCS & SharedFiles.gPMConstants.PMProduct & Chr(19) & Chr(8) & Chr(63) & CStr(iLicenceLimit)
		
		'DAK240100
        lErrorValue = SharedFiles.iPMFunc.LicenceEncrypt(sLicence:=sLicence, sLicenceKey:=sLicenceKey)
		
		' Check for any errors
		If (lErrorValue <> gPMConstants.PMEReturnCode.PMTrue) Then
			' Failed to Encrypt Licence Key.
			GenLicenceKey = gPMConstants.PMEReturnCode.PMFalse
			Exit Function
		End If
		
		Exit Function
		
	End Function
End Module
