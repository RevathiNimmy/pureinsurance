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
		Dim ACListsFieldArraySize, bGEMLists As Object
		Dim ACLists_Description, ACLists_ListID, ACLists_PropertyID As Byte
		Dim PMTrue As Integer
		
		Dim vLookupValues, vLookupDetails, vFieldArray As Object
		

		Dim oLists As bGEMLists.Form = New bGEMLists.Form()
		
		Dim sPropertyID As New FixedLengthString(10)
		Dim sDescription As New FixedLengthString(50)
		

		Dim lReturn As Integer = CType(oLists, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:="basilb", sPassword:="basilb", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test")
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		'    lReturn = oLists.GetLookupValues( _
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
		Dim lListID As Integer = 0
		sPropertyID.Value = CStr(1)
		sDescription.Value = CStr(2)
		

		ReDim vFieldArray(CInt(ACListsFieldArraySize))
		
		' Assign Field variables to array

		vFieldArray(ACLists_ListID) = lListID

		vFieldArray(ACLists_PropertyID) = sPropertyID.Value

		vFieldArray(ACLists_Description) = sDescription.Value
		
		' Do a direct Add passing DB fields in an array

		lReturn = oLists.DirectAdd(r_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set ID to required value to get details back
		lReturn = oLists.GetDetails(v_vListsID:=?)
		'UPGRADE_ERROR: (1010) The preceding line couldn't be parsed. More Information: http://www.vbtonet.com/ewis/ewi1010.aspx
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Get Next actually returns the fields in an array

		lReturn = oLists.GetNext(r_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oLists.EditUpdate(v_lRow:=1, v_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oLists.EditAdd(v_lRow:=2, v_vFieldArray:=vFieldArray)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
		' Set Row to required value

		lReturn = oLists.EditDelete(v_lRow:=1)
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		

		lReturn = oLists.Update
		
		If lReturn <> PMTrue Then
			MessageBox.Show(CStr(lReturn), Application.ProductName)
			Exit Sub
		End If
		
	End Sub
End Module
