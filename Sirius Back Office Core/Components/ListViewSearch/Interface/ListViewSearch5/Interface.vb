Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 
    Public objfrmSearch As frmSearch
	Public Function LvwSearch(ByRef oSearchList As ListView, Optional ByVal v_lShowModal As Integer = 1) As Integer
		Dim result As Integer = 0
        'developer guide no. 50
        objfrmSearch = New frmSearch
		result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 50
        objfrmSearch.SearchList = oSearchList
        'developer guide no. 50
        VB6.ShowForm(objfrmSearch, v_lShowModal)
        'developer guide no. 50
        Return objfrmSearch.Status
		
	End Function
End Class
