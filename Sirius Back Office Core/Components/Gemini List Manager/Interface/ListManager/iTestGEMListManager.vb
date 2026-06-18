Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Module Test
	
	' Return value for a function call.
	Private m_lReturn As Integer
	Private m_lCoverID As Integer
	Private m_sTradingName As String = ""
	
	Public Sub Main()
		Dim Form1 As Object
		
		Dim vKeyArray(1, 6) As Object
		Dim lRow As Integer
		Dim vListData As Object
		

		Form1.Show(FormShowConstants.Modal)
		Exit Sub
		
		' Create a new instance of the interface.
		Dim oInterface As New iGEMListManager.Interface_Renamed
		

		m_lReturn = CInt(CType(oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise())
		

		m_lReturn = CInt(oInterface.CheckListVersions())
		
		Dim sPropertyId As String = "524291"

		m_lReturn = oInterface.GetList(v_spropertyid:=sPropertyId, r_vListData:=vListData)
		
		

        oInterface.Dispose()
		
		oInterface = Nothing
		
		Environment.Exit(0)
		
	End Sub
End Module