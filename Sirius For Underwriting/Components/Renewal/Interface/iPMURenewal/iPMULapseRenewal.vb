Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmLapseRenewal

    Inherits System.Windows.Forms.Form
    ' Form Name: frmLapseRenewal
    '
    ' Date: 26/09/00
    '
    ' Description: form for obtaining renewal lapse reason for Renewals processing.
    '
    ' Edit History: CT 26/09/2000 - Created
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmLapseRenewal"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)


    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_lItemsFound As Integer
    Private m_vLapseReasons(,) As Object

    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer


    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdSelect_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelect.Click
        Try

            ' Check if there are any items available.
            If lvwLapseReasons.Items.Count = 0 Then
                Exit Sub
            End If

            m_lReturn = StoreLapseReason()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSelect_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Me.Hide()
            Me.Close()

        Catch excep As System.Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSelect_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        iPMFunc.ShowFormInTaskBar_Attach()

    End Sub


    Private Sub frmLapseRenewal_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occured setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            m_lReturn = ShowLapseReasons()

            lvwLapseReasons.HideSelection = False

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' {* USER DEFINED CODE (Begin) *}

            'Made full row select on list views
            'Developer Guide No. TODO: commeted not required archana
            'm_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwLapseReasons.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If



            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                'developer guide no. 243
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message
            'developer guide no. 26.
            stbStatus.Text = " " & sMessage
            'stbStatus.Refresh()
            lblStatus.Text = " " & sMessage
            lblStatus.Refresh()
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'developer guide no. 26
            stbStatus.Text = " " & m_lItemsFound & " " & sMessage
            lblStatus.Text = " " & m_lItemsFound & " " & sMessage
            'stbStatus.Refresh()
            lblStatus.Refresh()
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwLapseReasons.Items.Clear()

            m_lItemsFound = 0

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vLapseReasons) Then
                Return result
            End If

            'Assign the details to the interface.
            For lRow As Integer = m_vLapseReasons.GetLowerBound(1) To m_vLapseReasons.GetUpperBound(1)
                ' Assign the details to the first column.
                m_lItemsFound += 1
                ' Column 1 Lapse reason id
                oListItem = lvwLapseReasons.Items.Add(CStr(m_vLapseReasons(ACLapseReasonID, lRow)).Trim())

                ' Assign details to the other columns
                ' Column 2 Lapse reason descrition
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLapseReasons(ACLapseReason, lRow)).Trim()
            Next lRow

            'Refresh the initial results.
            lvwLapseReasons.Refresh()
            lvwLapseReasons.Sort()
            If lvwLapseReasons.Items.Count > 1 Then
                lvwLapseReasons.Items.Item(lvwLapseReasons.Items.Count - 2).Selected = True
                lvwLapseReasons.Items.Item(lvwLapseReasons.Items.Count - 2).Focused = True

            End If
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ShowLapseReasons
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function ShowLapseReasons() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Display a searching message.
            DisplayStatusSearching()


            m_lReturn = g_oRenewal.GetLapseReasons(r_vLapseReasons:=m_vLapseReasons)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = DataToInterface()

            'Display a searching message.
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show lapse reasons", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLapseReasons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub lvwLapseReasons_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwLapseReasons.DoubleClick
        Try

            ' Check if there are any items available.
            If lvwLapseReasons.Items.Count = 0 Then
                Exit Sub
            End If

            m_lReturn = StoreLapseReason()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwLapseReasons_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            Me.Close()

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse the renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwLapseReasons_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: StoreLapseReason
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function StoreLapseReason() As Integer
        Dim result As Integer = 0
        Dim iSelected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iSelected = lvwLapseReasons.FocusedItem.Index + 1
            g_lReasonID = CInt(lvwLapseReasons.Items.Item(iSelected - 1).Text)
            g_sReasonDesc = ListViewHelper.GetListViewSubItem(lvwLapseReasons.Items.Item(iSelected - 1), ACLapseReason).Text


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign data to properties", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreLapseReason", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


End Class
