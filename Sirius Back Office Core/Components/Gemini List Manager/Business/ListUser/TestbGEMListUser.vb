Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Module TestRun
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	'
	
	
	Public Sub Main()
		
		Test()
		
	End Sub
	Sub Test()
		Dim ACListUserFieldArraySize, bGEMListUser As Object
		Dim ACListUser_AbiCode, ACListUser_ListID, ACListUser_ListUserID, ACListUser_Text As Byte
		Dim PMTrue As Integer
		
        Dim vLookupValues, vLookupDetails As Object
        Dim vFieldArray() As Object

		Dim oListUser As bGEMListUser.Form = New bGEMListUser.Form()
		
		Dim sText As New FixedLengthString(70)
		Dim sAbiCode As New FixedLengthString(10)
		

		Dim lReturn As Integer = CType(oListUser, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:="basilb", sPassword:="basilb", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test")
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		'    lReturn = oListUser.GetLookupValues( _
		''                                iLookupType:=PMLookupAllEffective, _
		''                                vTableArray:=vLookupValues, _
		''                                iLanguageID:=1, _
		''                                vResultArray:=vLookupDetails)
		'
		'    If (lReturn <> PMTrue) Then
		'        MsgBox lReturn
		'        Exit Sub
		'    End If
		
		' Field variables nominally given a counter as a value
		' Set to required values
		Dim lListUserID As Integer = 0
		Dim lListID As Integer = 1
		sText.Value = CStr(2)
		sAbiCode.Value = CStr(3)
		

		ReDim vFieldArray(CInt(ACListUserFieldArraySize))
		
		' Assign Field variables to array

		vFieldArray(ACListUser_ListUserID) = lListUserID

		vFieldArray(ACListUser_ListID) = lListID

		vFieldArray(ACListUser_Text) = sText.Value

		vFieldArray(ACListUser_AbiCode) = sAbiCode.Value
		
		' Do a direct Add passing DB fields in an array

		lReturn = oListUser.DirectAdd(r_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set ID to required value to get details back
		lReturn = oListUser.GetDetails(v_vListUserID:=?)
		'UPGRADE_ERROR: (1010) The preceding line couldn't be parsed. More Information: http://www.vbtonet.com/ewis/ewi1010.aspx
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Get Next actually returns the fields in an array

		lReturn = oListUser.GetNext(r_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oListUser.EditUpdate(v_lRow:=1, v_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oListUser.EditAdd(v_lRow:=2, v_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oListUser.EditDelete(v_lRow:=1)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		

		lReturn = oListUser.Update
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
	End Sub
End Module
