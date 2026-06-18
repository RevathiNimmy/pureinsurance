Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmLicenceKeyGen
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmLicenceKeyGen"
	
	Private Sub cmdGenerate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGenerate.Click
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sLicenceKey As String = ""
		Dim iIsBlockAboveLicenceLimit As gPMConstants.PMEReturnCode
		Dim iIsWarnAboveLicenceLimit As gPMConstants.PMEReturnCode
		
		
		Dim dbNumericTemp As Double
		If Not Double.TryParse(txtICCS.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
			txtICCS.Text = ""
			txtICCS.Focus()
			Exit Sub
		End If
		
		'DAK140400
		If txtCategory.Visible Then
			
			If txtCategory.Text.Trim() = "" Then
				txtCategory.Text = ""
				txtCategory.Focus()
				Exit Sub
			End If
			
			If txtCategoryDescription.Text.Trim() = "" Then
				txtCategoryDescription.Text = ""
				txtCategoryDescription.Focus()
				Exit Sub
			End If
			
			If optBlock.Checked Then
				iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue
				iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
			ElseIf optWarn.Checked Then 
				iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
				iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue
			Else
				iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
				iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMFalse
			End If
			
		End If
		
		Dim dbNumericTemp2 As Double
		If Not Double.TryParse(txtLicenceLimit.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
			txtLicenceLimit.Text = ""
			txtLicenceLimit.Focus()
			Exit Sub
		End If
		
		'DAK140400 remove system name
		If optSystem.Checked Then
			lReturn = CType(GenLicenceKey(sLicenceKey:=sLicenceKey, sICCS:=txtICCS.Text, iLicenceLimit:=CInt(txtLicenceLimit.Text)), gPMConstants.PMEReturnCode)
		Else
			lReturn = CType(GenProductLicenceKey(sICCS:=txtICCS.Text, sCategoryCode:=txtCategory.Text, iLicenceLimit:=CInt(txtLicenceLimit.Text), sLicenceKey:=sLicenceKey, iIsBlockAboveLicenceLimit:=iIsBlockAboveLicenceLimit, iIsWarnAboveLicenceLimit:=iIsWarnAboveLicenceLimit), gPMConstants.PMEReturnCode)
		End If
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		txtLicenceKey.Text = sLicenceKey
		
		cmdWrite.Enabled = True
		
	End Sub
	
	' ***************************************************************** '
	' Name: GenLicenceKey
	'
	' Description: Encrypts the system name, ICCS code and
	'              licence limit to generate the licence key.
	'
	' ***************************************************************** '
	'DAK140400 - remove system name
	Private Function GenLicenceKey(ByRef sLicenceKey As String, ByRef sICCS As String, ByRef iLicenceLimit As Integer) As Integer
		Dim result As Integer = 0
		Dim lErrorValue As Integer
		Dim sLicence As String = ""
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If iLicenceLimit = 0 Then
				sLicenceKey = ""
				Return result
			End If
			
			'DAK240100
			' Ignore case of system name
			'DAK140400 replace system name with PMProduct
			sLicence = CStr(iLicenceLimit) &  _
			           sICCS &  _
			           gPMConstants.PMProduct &  _
			           Strings.Chr(19).ToString() &  _
			           Strings.Chr(8).ToString() &  _
			           Strings.Chr(63).ToString() &  _
			           iLicenceLimit
			
			'DAK240100
			lErrorValue = iPMFunc.LicenceEncrypt(sLicence:=sLicence, sLicenceKey:=sLicenceKey)
			
			' Check for any errors
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to Encrypt Licence Key.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to generate the licence key", vApp:=ACApp, vClass:=ACClass, vMethod:="GenLicenceKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: GenProductLicenceKey
	'
	' Description: Encrypts the category code, ICCS code, licence limit,
	'              is block above licence limit and is warn above licence
	'              limit to generate the licence key.
	'
	' ***************************************************************** '
	Private Function GenProductLicenceKey(ByRef sICCS As Object, ByRef sCategoryCode As String, ByRef iLicenceLimit As Integer, ByRef sLicenceKey As String, ByRef iIsBlockAboveLicenceLimit As Integer, ByRef iIsWarnAboveLicenceLimit As Integer) As Integer
		Dim result As Integer = 0
		Dim lErrorValue As Integer
		Dim sLicence As String = ""
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue And iLicenceLimit = 0 Then
				
				sLicenceKey = ""
				Return result
				
			End If
			
			If sCategoryCode = gPMConstants.ACTaskCategoryNonLicence Then
				sLicenceKey = ""
				Return result
			End If
			
			'DAK240100
			' Ignore case of Category Code
            sLicence = CStr(iLicenceLimit) & _
              CStr(sICCS) & _
              sCategoryCode.ToUpper() & _
              Strings.Chr(19).ToString() & _
              Strings.Chr(8).ToString() & _
              Strings.Chr(63).ToString() & _
              iLicenceLimit
			
			If iIsBlockAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue Then
				sLicence = sLicence & "B"
			ElseIf iIsWarnAboveLicenceLimit = gPMConstants.PMEReturnCode.PMTrue Then 
				sLicence = sLicence & "W"
			Else
				sLicence = sLicence & "N"
			End If
			
			'DAK240100
			lErrorValue = iPMFunc.LicenceEncrypt(sLicence:=sLicence, sLicenceKey:=sLicenceKey)
			
			' Check for any errors
			If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to Encrypt Licence Key.
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Error Section.
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to generate the licence key", vApp:=ACApp, vClass:=ACClass, vMethod:="GenProductLicenceKey", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: WriteKeyInfo
	'
	' Description:
	'
	' History: 05/01/2000 DAK - Created.
	'
	' ***************************************************************** '
	Private Function WriteKeyInfo() As Integer
        Dim nresult As Integer = 0
		Dim lResult As gPMConstants.PMEReturnCode
		Dim sFileName, sDir, sCurDir As String
		
		
		Try 
			
            nresult = gPMConstants.PMEReturnCode.PMTrue
			
			sFileName = "PMLicence.ini"
            sCurDir = Directory.GetCurrentDirectory()
            If fbdGetFilePath.ShowDialog() = Windows.Forms.DialogResult.OK Then
                sDir = fbdGetFilePath.SelectedPath & "\"
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If
            FileSystem.ChDrive(sDir)
            FileSystem.ChDrive(sCurDir)


            Dim dlgbrowse As New OpenFileDialog
            With dlgbrowse
                'TODO:Cancelerror Property to be checked at runtime
                '.CancelError = True
                .FileName = sDir & sFileName
                .Filter = "Licence Infrormation (*.ini)|*.ini"
                .FilterIndex = 1
                .Title = "Select Licenece File"
                .ShowDialog()
                sFileName = .FileName
            End With

            If optSystem.Checked Then
                lResult = CType(WriteSystemDets(sFileName), gPMConstants.PMEReturnCode)
            Else
                lResult = CType(WriteProductDets(sFileName), gPMConstants.PMEReturnCode)
            End If

            If lResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nresult

        Catch excep As System.Exception



            ' A Drive unavailable so start lookiung in current directory
            If Information.Err().Number = 68 Then
                If sCurDir.EndsWith("\") Then
                    sDir = sCurDir
                Else
                    sDir = sCurDir & "\"
                End If


            End If

            ' Cancel button pressed so
            If Information.Err().Number = DialogResult.Cancel Then
                Return nresult
            End If

            nresult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteKeyInfo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteKeyInfo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nresult

        End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: WriteSystemDets
	'
	' Description:
	'
	' History: 05/01/2000 DAK - Created.
	'
	' ***************************************************************** '
	Private Function WriteSystemDets(ByRef sIpFileName As String) As Integer
		'DAK240100
		Dim result As Integer = 0
		Dim sProfile As String = ""
        Dim auxVar As String = txtLicenceLimit.Text.Trim()
        Dim auxVar_2 As String = txtLicenceKey.Text.Trim()

		Try 

			result = gPMConstants.PMEReturnCode.PMTrue
			
			'DAK240100
			'    sProfile = UCase$(Trim$(txtSystem.Text))
			'DAK140400 replace system name with PMProduct
			sProfile = gPMConstants.PMProduct
			
			'DAK240100
			Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(ACLicenceLimit)
			Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(auxVar)
			Try 

				WritePrivateProfileString(ACSystemPrefix & sProfile, tmpPtr, tmpPtr2, sIpFileName)
			Finally 
				Marshal.FreeHGlobal(tmpPtr)
				Marshal.FreeHGlobal(tmpPtr2)
			End Try
			
			'DAK240100
			Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi(ACLicenceKey)
			Dim tmpPtr4 As IntPtr = Marshal.StringToHGlobalAnsi(auxVar_2)
			Try 
                WritePrivateProfileString(ACSystemPrefix & sProfile, tmpPtr3, tmpPtr4, sIpFileName)
			Finally 
				Marshal.FreeHGlobal(tmpPtr3)
				Marshal.FreeHGlobal(tmpPtr4)
			End Try
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteSystemDets Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteSystemDets", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: WriteProductDets
	'
	' Description:
	'
	' History: 05/01/2000 DAK - Created.
	'
	' ***************************************************************** '
	Private Function WriteProductDets(ByRef sIpFileName As String) As Integer
        Dim result As Integer = 0
		Dim lNoOfChars As Integer
		Dim sDefault As String = ""
		Dim iSub As Integer
		Dim sProductCode, sExceedLimitAction As String
		'DAK240100
		Dim sProfile As String = ""
        Dim auxVar As String = txtCategoryDescription.Text.Trim()
        Dim auxVar_2 As String = txtLicenceLimit.Text.Trim()
        Dim auxVar_3 As String = txtLicenceKey.Text.Trim()

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'DAK240100
			sProfile = txtCategory.Text.Trim().ToUpper()
			
			' Check if product is already in the file
			iSub = 0
			
			Do 
				iSub += 1
				
				sDefault = ""
				sProductCode = New String(" ", 128)
				Dim tmpPtr As IntPtr = Marshal.StringToHGlobalAnsi(CStr(iSub))
				Try 
					lNoOfChars = GetPrivateProfileString(ACInstalledProducts, tmpPtr, sDefault, sProductCode, 128, sIpFileName)
				Finally 
					Marshal.FreeHGlobal(tmpPtr)
				End Try
				
				sProductCode = sProductCode.Substring(0, lNoOfChars)
				sProductCode = sProductCode.Trim()
				
				'DAK240100
				If sProductCode = sProfile Then
					Exit Do
				End If
				
			Loop While sProductCode <> ""
			
			' Add the product to installed products
			If sProductCode = "" Then
				Dim tmpPtr2 As IntPtr = Marshal.StringToHGlobalAnsi(CStr(iSub))
				Dim tmpPtr3 As IntPtr = Marshal.StringToHGlobalAnsi(sProfile)
				Try 
					WritePrivateProfileString(ACInstalledProducts, tmpPtr2, tmpPtr3, sIpFileName)
					sProfile = Marshal.PtrToStringAnsi(tmpPtr3)
				Finally 
					Marshal.FreeHGlobal(tmpPtr2)
					Marshal.FreeHGlobal(tmpPtr3)
				End Try
			End If
			
			' Add the product details
			'DAK240100
			Dim tmpPtr4 As IntPtr = Marshal.StringToHGlobalAnsi(ACProductDescription)
			Dim tmpPtr5 As IntPtr = Marshal.StringToHGlobalAnsi(auxVar)
			Try 

				WritePrivateProfileString(ACProductPrefix & sProfile, tmpPtr4, tmpPtr5, sIpFileName)
			Finally 
				Marshal.FreeHGlobal(tmpPtr4)
				Marshal.FreeHGlobal(tmpPtr5)
			End Try
			
			'DAK240100
			Dim tmpPtr6 As IntPtr = Marshal.StringToHGlobalAnsi(ACLicenceLimit)
			Dim tmpPtr7 As IntPtr = Marshal.StringToHGlobalAnsi(auxVar_2)
			Try 

				WritePrivateProfileString(ACProductPrefix & sProfile, tmpPtr6, tmpPtr7, sIpFileName)
			Finally 
				Marshal.FreeHGlobal(tmpPtr6)
				Marshal.FreeHGlobal(tmpPtr7)
			End Try
			
			'DAK240100
			Dim tmpPtr8 As IntPtr = Marshal.StringToHGlobalAnsi(ACLicenceKey)
			Dim tmpPtr9 As IntPtr = Marshal.StringToHGlobalAnsi(auxVar_3)
			Try 

				WritePrivateProfileString(ACProductPrefix & sProfile, tmpPtr8, tmpPtr9, sIpFileName)
			Finally 
				Marshal.FreeHGlobal(tmpPtr8)
				Marshal.FreeHGlobal(tmpPtr9)
			End Try
			
			If optBlock.Checked Then
				sExceedLimitAction = ACExceedLimitBlock
			ElseIf optWarn.Checked Then 
				sExceedLimitAction = ACExceedLimitWarn
			Else
				sExceedLimitAction = ACExceedLimitNone
			End If
			
			'DAK240100
			Dim tmpPtr10 As IntPtr = Marshal.StringToHGlobalAnsi(ACExceedLimitAction)
			Dim tmpPtr11 As IntPtr = Marshal.StringToHGlobalAnsi(sExceedLimitAction)
			Try 
				WritePrivateProfileString(ACProductPrefix & sProfile, tmpPtr10, tmpPtr11, sIpFileName)
				sExceedLimitAction = Marshal.PtrToStringAnsi(tmpPtr11)
			Finally 
				Marshal.FreeHGlobal(tmpPtr10)
				Marshal.FreeHGlobal(tmpPtr11)
			End Try
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteProductDets Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteProductDets", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdWrite_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdWrite.Click
		
		
		Dim lResult As gPMConstants.PMEReturnCode = CType(WriteKeyInfo(), gPMConstants.PMEReturnCode)
		cmdWrite.Enabled = False
		
	End Sub
	
	Private isInitializingComponent As Boolean
	Private Sub optCategory_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optCategory.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			lblAction.Visible = True
			optBlock.Visible = True
			optBlock.Enabled = True
			optWarn.Visible = True
			optWarn.Enabled = True
			optNone.Visible = True
			optNone.Enabled = True
			lblCategory.Visible = True
			txtCategory.Visible = True
			lblCategoryDescription.Visible = True
			txtCategoryDescription.Visible = True
		End If
	End Sub
	
	Private Sub optSystem_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optSystem.CheckedChanged
		If eventSender.Checked Then
			If isInitializingComponent Then
				Exit Sub
			End If
			lblAction.Visible = False
			optBlock.Visible = False
			optBlock.Enabled = False
			optWarn.Visible = False
			optWarn.Enabled = False
			optNone.Visible = False
			optNone.Enabled = False
			lblCategory.Visible = False
			txtCategory.Visible = False
			lblCategoryDescription.Visible = False
			txtCategoryDescription.Visible = False
		End If
	End Sub
End Class
