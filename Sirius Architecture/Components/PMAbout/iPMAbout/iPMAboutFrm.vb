Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 22/05/1998
	'
	' Description: Policy Master About Box
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
	
	' Reg Key Security Options...
	Const KEY_ALL_ACCESS As Integer = &H2003F
	
	' Reg Key ROOT Types...
	Const HKEY_LOCAL_MACHINE As Integer = &H80000002
	Const ERROR_SUCCESS As Integer = 0
	Const REG_SZ As Integer = 1 ' Unicode nul terminated string
	Const REG_DWORD As Integer = 4 ' 32-bit number
    Const REG_EXPAND_SZ As Integer = 2
	Const gREGKEYSYSINFOLOC As String = "SOFTWARE\Microsoft\Shared Tools Location"
	Const gREGVALSYSINFOLOC As String = "MSINFO"
	Const gREGKEYSYSINFO As String = "SOFTWARE\Microsoft\Shared Tools\MSINFO"
	Const gREGVALSYSINFO As String = "PATH"
	
	Private Declare Function RegOpenKeyEx Lib "advapi32"  Alias "RegOpenKeyExA"(ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer
	Private Declare Function RegQueryValueEx Lib "advapi32"  Alias "RegQueryValueExA"(ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer
	Private Declare Function RegCloseKey Lib "advapi32" (ByVal hKey As Integer) As Integer
	
	' PUBLIC Property Procedures (Begin)
	
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
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Try 
			
			' Check if we have had an error so far.
			' Possibly creating the business object.
			If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
				' We have already encountered an error,
				' so we MUST exit now.
				Exit Sub
			End If
			
			' {* USER DEFINED CODE (Begin) *}
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
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Form_Load process failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: CmdSysInfo
	'
	' Description: Call to StartSysInfo
	'
	' ***************************************************************** '
	
	Private Sub cmdSysInfo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSysInfo.Click
		
		Try 
			
			m_lReturn = StartSysInfo()
		
		Catch excep As System.Exception
			
			
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Call StartSysInfo failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdSysInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Me.Close()
		
	End Sub
	
	' ***************************************************************** '
	' Name: StartSysInfo
	'
	' Description: Display System Info
	'
	' ***************************************************************** '
	
	Public Function StartSysInfo() As Integer
		
		Dim result As Integer = 0
        Dim SysInfoPath As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Try To Get System Info Program Path\Name From Registry...
			If GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFO, gREGVALSYSINFO, SysInfoPath) Then
				' Try To Get System Info Program Path Only From Registry...
			ElseIf GetKeyValue(HKEY_LOCAL_MACHINE, gREGKEYSYSINFOLOC, gREGVALSYSINFOLOC, SysInfoPath) Then 
				' Validate Existance Of Known 32 Bit File Version
				If FileSystem.Dir(SysInfoPath & "\MSINFO32.EXE", FileAttribute.Normal) <> "" Then
					SysInfoPath = SysInfoPath & "\MSINFO32.EXE"
					
					
					' Error - File Can Not Be Found...
				Else
					Throw New Exception()
				End If
				' Error - Registry Entry Can Not Be Found...
			Else
				Throw New Exception()
			End If
			

            'Dim startInfo As ProcessStartInfo = New ProcessStartInfo(SysInfoPath)
            'startInfo.WindowStyle = ProcessWindowStyle.Normal
            'Process.Start(startInfo)
            ShellWait(SysInfoPath)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			MessageBox.Show("System Information Is Unavailable At This Time", Application.ProductName, MessageBoxButtons.OK)
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get System Info failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StartSysInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GetKeyValue
	'
	' Description: Get Key Value
	'
	' This function was written by Microsoft and expects a TRUE or FALSE
	' return value. Should not be changed to PMTrue or PMFalse.
	'
	' ***************************************************************** '
	
	Public Function GetKeyValue(ByRef KeyRoot As Integer, ByRef KeyName As String, ByRef SubKeyRef As String, ByRef KeyVal As String) As Boolean
		
		Dim result As Boolean = False
		' Loop Counter
		Dim rc As Integer ' Return Code
		Dim hKey As Integer ' Handle To An Open Registry Key
        Dim KeyValType As Integer ' Data Type Of A Registry Key
		Dim tmpVal As String = "" ' Tempory Storage For A Registry Key Value
		Dim KeyValSize As Integer ' Size Of Registry Key Variable
		
		Try 
			
			'------------------------------------------------------------
			' Open RegKey Under KeyRoot {HKEY_LOCAL_MACHINE...}
			'------------------------------------------------------------
			rc = RegOpenKeyEx(KeyRoot, KeyName, 0, KEY_READ, hKey) ' Open Registry Key
			
			
			If (rc <> ERROR_SUCCESS) Then Throw New Exception() ' Handle Error...
			
			
			tmpVal = New String(Strings.Chr(0), 1024) ' Allocate Variable Space
			KeyValSize = 1024 ' Mark Variable Size
			
			
			'------------------------------------------------------------
			' Retrieve Registry Key Value...
			'------------------------------------------------------------
			rc = RegQueryValueEx(hKey, SubKeyRef, 0, KeyValType, tmpVal, KeyValSize) ' Get/Create Key Value
			
			
			If (rc <> ERROR_SUCCESS) Then Throw New Exception() ' Handle Errors
			
			
			If Strings.Asc(Mid(tmpVal, KeyValSize, 1)(0)) = 0 Then ' Win95 Adds Null Terminated String...
				tmpVal = tmpVal.Substring(0, KeyValSize - 1) ' Null Found, Extract From String
			Else
				' WinNT Does NOT Null Terminate String...
				tmpVal = tmpVal.Substring(0, KeyValSize) ' Null Not Found, Extract String Only
			End If
			'------------------------------------------------------------
			' Determine Key Value Type For Conversion...
			'------------------------------------------------------------
			Select Case KeyValType ' Search Data Types...
				Case REG_SZ ' String Registry Key Data Type
                    KeyVal = tmpVal ' Copy String Value
                Case REG_EXPAND_SZ
                    KeyVal = tmpVal
                Case REG_DWORD ' Double Word Registry Key Data Type
                    For i As Integer = tmpVal.Length To 1 Step -1 ' Convert Each Bit
                        KeyVal = KeyVal & Strings.Asc(Mid(tmpVal, i, 1)(0)).ToString("X") ' Build Value Char. By Char.
                    Next
                    KeyVal = ("&h" & KeyVal).ToString() ' Convert Double Word To String
            End Select
			
			
			result = True ' Return Success
			rc = RegCloseKey(hKey) ' Close Registry Key
			
			Return result
		
		Catch excep As System.Exception ' Exit
			
			
			
			KeyVal = "" ' Set Return Val To Empty String
			result = False ' Return Failure
			rc = RegCloseKey(hKey) ' Close Registry Key
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get registry key value", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_GetKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function

	
	' ***************************************************************** '
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		
		Dim result As Integer = 0
		Dim sDMEDir As String = ""
		
		
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
			
			'    Me.Caption = iPMFunc.GetResData( _
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
			'        LogMessage _
			''            iType:=PMLogError, _
			''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
			''            "Please check the file exists and the correct captions are available", _
			''            vApp:=ACApp, _
			''            vClass:=ACClass, _
			''            vMethod:="DisplayCaptions"
			'
			'        Exit Function
			'    End If
			
			'    cmdFindNow.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACOKButton, _
			''        iDataType:=PMResString)
			'
			'    cmdCancel.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACCancelButton, _
			''        iDataType:=PMResString)
			'
			'    cmdHelp.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACHelpButton, _
			''        iDataType:=PMResString)
			'
			'    cmdNavigate.Caption = iPMFunc.GetResData( _
			''        iLangID:=g_iLanguageID%, _
			''        lID:=ACNavigateButton, _
			''        iDataType:=PMResString)
			'
			'    tabMainTab.TabCaption(0) = iPMFunc.GetResData( _
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
			'    lblDesc.Caption = iPMFunc.GetResData( _
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

    Private Sub cmdSupport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSupport.Click

        Try
            ' Spawn the default e-mail client so that the user can email PM
            m_lReturn = ShellExecute(hwnd:=Me.Handle.ToInt32(), lpOperation:="open", lpFile:="http://www.sspsupport.com", lpParameters:="", lpDirectory:=".", nShowCmd:=1)

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to launch SSP website.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSupport_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub
End Class
