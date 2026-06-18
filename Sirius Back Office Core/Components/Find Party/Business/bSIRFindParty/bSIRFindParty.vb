Option Strict Off
Option Explicit On
Imports SSP.Shared
Module MainModule

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date:  27th September 1996
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '



    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bSIRFindPartyCore"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As New StringsHelper.FixedLengthString(12)

    ' Password.
    Public g_sPassword As New StringsHelper.FixedLengthString(30)

    ' User ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iUserID As Integer

    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()>
    Public g_sCallingAppName As String = ""

    ' Source ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iSourceID As Integer

    ' Language ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLanguageID As Integer

    ' Log Level
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iLogLevel As Integer

    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()>
    Public g_iCurrencyID As Integer

    'sj 3/11/99 - start
    ' Constants for the get broking client data array indexes.
    Public Const ACBInvariantKey As Integer = 0
    Public Const ACBClientCode As Integer = 1
    Public Const ACBClientName As Integer = 2
    Public Const ACBAddress1 As Integer = 3
    Public Const ACBAddress2 As Integer = 4
    Public Const ACBAddress3 As Integer = 5
    Public Const ACBAddress4 As Integer = 6
    Public Const ACBPostCode As Integer = 7
    Public Const ACBPortfolio As Integer = 8
    Public Const ACBCustomerID As Integer = 9
    Public Const ACBStatus As Integer = 10 ' "Y" = corporate client

    ' Constants for the search data array indexes.
    Public Const ACIPartyCnt As Integer = 0
    Public Const ACIPartyType As Integer = 1
    Public Const ACIShortName As Integer = 2
    Public Const ACILongName As Integer = 3
    Public Const ACIAddress1 As Integer = 4
    Public Const ACIPostalCode As Integer = 5
    Public Const ACISourceID As Integer = 6
    Public Const ACIPartyID As Integer = 7
    Public Const ACITelAreaCode As Integer = 8
    Public Const ACITelNumber As Integer = 9
    Public Const ACIPartyStatus As Integer = 10
    Public Const ACIInvariantKey As Integer = 11
    Public Const ACISource As Integer = 12
    Public Const ACIMax As Integer = 12

    Public Const PMSearchSirius As Integer = 0
    Public Const PMSearchPMB As Integer = 1
    Public Const PMSearchSiriusPMB As Integer = 2
    'sj 3/11/99 - end

    'sj 10/2/00 - start
    Public Const ACMAClientCode As Integer = 0
    Public Const ACMATelNo As Integer = 10
    Public Const ACMAAltTelNo As Integer = 11
    Public Const ACMADOB As Integer = 16
    Public Const ACMASex As Integer = 17
    Public Const ACMAMarried As Integer = 18
    Public Const ACMAChildren As Integer = 24
    'sj 10/2/00 - end

    Public Const PARTY_TYPE_DELIMITER As String = ","


    Sub Main_Renamed()

        ' Main entry point for the component

        'Call TEST

    End Sub

    'Sub TEST()

    '	'SD 01/08/2002 Scalability changes
    '	Dim oFindParty As bSIRFindPartyCore.Business
    '	Dim vResultArray(,) As Object = Nothing

    '	Dim lerror As gPMConstants.PMEReturnCode = CType(CType(oFindParty, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:="philj", sPassword:="philj", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=1, iLogLevel:=6, sCallingAppName:="Test"), gPMConstants.PMEReturnCode)
    '	Dim sPartyName As String = "H"

    '	If lerror <> gPMConstants.PMEReturnCode.PMTrue Then
    '		MessageBox.Show("Error  in initialisation", Application.ProductName)
    '		Exit Sub
    '	End If

    '	Dim lNumberOfRecords As Integer = 999

    '	'lerror = oFindParty.GetID(vID:=lPartyCnt, vShortName:="Jones P")

    '	'lerror = oFindParty.SearchByQuery(lNumberOfRecords:=lNumberOfRecords, vResultArray:=vResultArray, _
    '	'vName:="Jones", vClientType:=-1, vAddress1:="", vPostalCode:="", vForeName:="", _
    '	'vDateOfBirth:=CDate(-1), vAreaCode:="", vNumber:="" _
    '	')

    '	'lerror = oFindParty.SearchLikeShortName(lNumberOfRecords:=lNumberOfRecords, vResultArray:=vResultArray, _
    '	'sShortName:="BETTERIDGE S")

    '	'lerror = oFindParty.GetLookupValues( _
    '	'iLookupType:=PMLookupAll, _
    '	'vTableArray:=vTableArray, _
    '	'iLanguageID:=1, _
    '	'vResultArray:=vResultArray)


    '	If Informations.IsArray(vResultArray) Then

    '		For iCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

    '			For iCount1 As Integer = vResultArray.GetLowerBound(0) To vResultArray.GetUpperBound(0)

    '                   Debug.WriteLine(CStr(vResultArray(iCount1, iCount)) & " ")
    '			Next iCount1
    '		Next iCount
    '	End If


    'End Sub
End Module