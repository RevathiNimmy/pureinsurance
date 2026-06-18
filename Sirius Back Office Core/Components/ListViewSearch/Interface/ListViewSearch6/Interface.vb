Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed 
	
    'developer guide no. 69
    Public m_ofrmSearch As frmSearch
	Public Function LvwSearch(ByRef oSearchList As ListView, Optional ByVal v_lShowSelect As Integer = 1, Optional ByVal v_lShowModal As Integer = FormShowConstants.Modal) As Integer
        Dim result As Integer = 0
        m_ofrmSearch = New frmSearch
		result = gPMConstants.PMEReturnCode.pmtrue
        'developer guide no. 69
        m_ofrmSearch.SearchList = oSearchList
        'developer guide no. 69
        m_ofrmSearch.cmdSelect.Visible = v_lShowSelect
        'developer guide no. 69
        VB6.ShowForm(m_ofrmSearch, v_lShowModal)
        'developer guide no. 69
        Return m_ofrmSearch.Status
		
	End Function
End Class
