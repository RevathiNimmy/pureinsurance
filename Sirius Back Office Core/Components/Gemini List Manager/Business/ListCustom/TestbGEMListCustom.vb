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
		Dim ACListCustomFieldArraySize, bGEMListCustom As Object
		Dim ACListCustom_AbiCode, ACListCustom_Command, ACListCustom_ListCustomID, ACListCustom_PositionID, ACListCustom_PropertyID, ACListCustom_Text, ACListCustom_ValueID As Byte
		Dim PMTrue As Integer
		
        Dim vLookupValues, vLookupDetails As Object
        Dim vFieldArray() As Object
		

		Dim oListCustom As bGEMListCustom.Form = New bGEMListCustom.Form()
		
		Dim sText As New FixedLengthString(70)
		Dim sAbiCode As New FixedLengthString(10)
		Dim sCommand As New FixedLengthString(1)
		Dim sPropertyID As New FixedLengthString(10)
		

		Dim lReturn As Integer = CType(oListCustom, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:="basilb", sPassword:="basilb", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test")
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		'    lReturn = oListCustom.GetLookupValues( _
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
		Dim lListCustomID As Integer = 0
		Dim lPositionID As Integer = 1
		Dim lValueID As Integer = 2
		sText.Value = CStr(3)
		sAbiCode.Value = CStr(4)
		sCommand.Value = CStr(5)
		sPropertyID.Value = CStr(6)
		

		ReDim vFieldArray(CInt(ACListCustomFieldArraySize))
		
		' Assign Field variables to array

		vFieldArray(ACListCustom_ListCustomID) = lListCustomID

		vFieldArray(ACListCustom_PositionID) = lPositionID

		vFieldArray(ACListCustom_ValueID) = lValueID

		vFieldArray(ACListCustom_Text) = sText.Value

		vFieldArray(ACListCustom_AbiCode) = sAbiCode.Value

		vFieldArray(ACListCustom_Command) = sCommand.Value

		vFieldArray(ACListCustom_PropertyID) = sPropertyID.Value
		
		' Do a direct Add passing DB fields in an array

		lReturn = oListCustom.DirectAdd(r_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set ID to required value to get details back
		lReturn = oListCustom.GetDetails(v_vListCustomID:=?)
		'UPGRADE_ERROR: (1010) The preceding line couldn't be parsed. More Information: http://www.vbtonet.com/ewis/ewi1010.aspx
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Get Next actually returns the fields in an array

		lReturn = oListCustom.GetNext(r_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oListCustom.EditUpdate(v_lRow:=1, v_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oListCustom.EditAdd(v_lRow:=2, v_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oListCustom.EditDelete(v_lRow:=1)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		

		lReturn = oListCustom.Update
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
	End Sub
End Module
