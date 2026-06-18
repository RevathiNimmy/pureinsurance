Option Strict Off
Option Explicit On
Imports SharedFiles
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 22/03/2011
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMUClonedRIManual"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons
    ' Form
    ' Buttons

    ' Messages
    Public Const ACInvalidStatusTitle As Short = 300
    Public Const ACInvalidStatus As Short = 301
    Public Const ACBusinessFailTitle As Short = 302
    Public Const ACBusinessFail As Short = 303
    Public Const ACStatusSearching As Short = 304
    Public Const ACStatusFound As Short = 305
    Public Const ACRerateNow As Short = 306
    Public Const ACReprintNow As Short = 307
    Public Const ACAmendQuery As Short = 308
    Public Const ACValidationTitle As Short = 309
    Public Const ACMandatoryStartDate As Short = 310
    Public Const ACMandatoryExpiryDate As Short = 311
    Public Const ACMandatoryStartGreaterThanExpiry As Short = 312
    Public Const ACLapseTitle As Short = 313
    Public Const ACLapseComplete As Short = 314
    Public Const ACAmendTitle As Short = 315
    Public Const ACAmendProcessLaunchFail As Short = 316
    Public Const ACAmendSetKeysFail As Short = 317
    Public Const ACAmendSetProcessModesFail As Short = 318
    Public Const ACAmendProcessFail As Short = 319

    Public Const ACDateColumn As Short = 2

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Short = 0
    Public Const ACControlEnd As Short = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' AMB 05-Sep-03: 1.8.6 Deferred Reinsurance development - deferred RI listview columns
    Public Enum enumClonedRIListView
        elBranchDesc = 0
        elDefRIStatusDesc = 1
        elInsuranceRef = 2
        elPartyShortName = 3
        elPartyName = 4
        elProductDesc = 5
    End Enum

    ' AMB 05-Sep-03: 1.8.6 Deferred Reinsurance development - deferred RI data array positions
    Public Enum enumClonedRIData
        edInsFileClonedRIUsageID = 0
        edInsFileCnt = 1
        edInsuranceRef = 2
        edBranchID = 3
        edBranchDesc = 4
        edDefRIStatusID = 5
        edDefRIStatusDesc = 6
        edInsuredCnt = 7
        edPartyShortName = 8
        edPartyName = 9
        edProductID = 10
        edProductDesc = 11
        edInsFolderCnt = 12
        edNewInsFileCnt = 13
    End Enum

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Short
    Public g_iLanguageID As Short
    Public g_lInsuranceFileCnt As Integer

    ' Public instance of the object manager.
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public Const ksPTRIStatusAmend As Integer = 1
    Public Const ksPTRIStatusUpdate As Integer = 2
    ' ***************************************************************** '
    ' Name: ListViewSortByDate
    '
    ' Description: Sorts the list view based on the column passed, and
    '              the order given.
    '
    ' Note : This hasn't been tested on the first column. I suspect
    '        changes might need to be made if sorting on the first
    '        column is needed (CF 060899).
    '
    ' ***************************************************************** '
   Public Function ListViewSortByDate(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder, Optional ByVal v_bMarkSortedColumn As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACClass As Object = Nothing

        Dim lSortColumn As Integer

        Const ACLVTag As String = "SORT_DATE_HIDDEN"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add a dummy sort column and get the index of this new column
            ' -1 because it's a sub item
            v_oListView.Columns.Add(ACLVTag, ACLVTag, CInt(VB6.TwipsToPixelsX(0)))
            lSortColumn = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For Each oListItem As ListViewItem In v_oListView.Items
                ' Process column 0 from the text property else use subitems
                If v_iSourceColumn Then
                    Dim TempDate As Date
                    ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = IIf(DateTime.TryParse(ListViewHelper.GetListViewSubItem(oListItem, v_iSourceColumn).Text, TempDate), TempDate.ToString("yyyyMMddHHMMss"), ListViewHelper.GetListViewSubItem(oListItem, v_iSourceColumn).Text)
                Else
                    If IsDate(oListItem.Text) Then
                        ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = CDate(oListItem.Text).ToString("yyyyMMddHHMMss")
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = (oListItem.Text).ToString
                    End If
                End If
            Next oListItem

            ' Set sort column and direction and sort
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
            ListViewHelper.SetSortKeyProperty(v_oListView, lSortColumn)
            ListViewHelper.SetSortedProperty(v_oListView, True)

            ' Remove the column now
            v_oListView.Columns.RemoveAt(lSortColumn)

            ' Set to original for asc/desc analysis?
            If v_bMarkSortedColumn Then
                ' Note: We must remove the sorted flag first of this will botch everything!
                ListViewHelper.SetSortedProperty(v_oListView, False)
                ListViewHelper.SetSortKeyProperty(v_oListView, v_iSourceColumn)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: Main was upgraded to Main_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Sub Main_Renamed()

    End Sub
End Module