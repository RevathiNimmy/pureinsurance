Option Strict Off
Option Explicit On
Imports SharedFiles

Friend Class frmUsers
	Inherits System.Windows.Forms.Form
	' ***************************************************************** '
	' Form Name: frmUsers
	'
	' Date: 12 July 2000
	'
	' Description: Main View Form.
	'
	' Edit History:
	'
	' ***************************************************************** '
	
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "frmUsers"
	
	' PUBLIC Data Members (Begin)
	
	Public m_vDatArray As Object
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Object parameter member.
	Private m_vObjectParam As Object
	
	
	Private m_lReturn As Integer
	
	'Declare the column numbers from where the data will sought from
	Const colUserName As Short = 0
	Const colLogonTime As Short = 2
	Const colLoggedOnAtClient As Short = 1
	'DAK060100
	Const colTaskInstance As Short = 1
	
	Const colLicenceLimit As Short = 0
	
	' ***************************************************************** '
	' Name: SetFormDefaults
	'
	' Description: Get and display all of the form's default values.
	'
	' ***************************************************************** '
	Private Function SetFormDefaults() As Integer
		Dim lErrorValue As Integer
		
		
		Try
		
		SetFormDefaults = gPMConstants.PMEReturnCode.PMTrue
		
		' Get all lookup details
		
		' {* USER DEFINED CODE (Begin) *}
		' {* USER DEFINED CODE (End) *}
		
		Exit Function
		
		Catch ex As Exception
		
		' Error Section.
		
		SetFormDefaults = gPMConstants.PMEReturnCode.PMError
		
		' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the forms defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormDefaults", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

		End Try
    End Function

    '*****************************************************************************
    '
    ' Name: GetShowData
    '
    ' Function to call the functions to get the data and populate the listview box
    '
    '*****************************************************************************
    Private Function GetShowData() As Integer
        Dim vUserdata As Object
        Dim lProductLimit As Integer
        Dim iIsWarnAboveLicenceLimit As Short
        Dim lWarnsSinceLicenceUpgrade As Integer
        Dim vProductData As Object


        Try

        cmdReset.Enabled = False

        GetShowData = gPMConstants.PMEReturnCode.PMTrue

        'call the function selectdata from the business
        'UPGRADE_WARNING: Couldn't resolve default property of object g_oBusiness.Selectdata. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        m_lReturn = g_oBusiness.Selectdata(r_vUserDataArray:=g_vUserData)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            ' Log Error.
            MsgBox("Unable to Read Data from Sirius Architecture Database" & vbCrLf & "UsersLoggedOn will be shut down.", MsgBoxStyle.Critical, "Critical Error")
            GetShowData = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If


        Catch ex As Exception

        'Error Section.

        GetShowData = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Obtain and display data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowData", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)


        End Try
    End Function

    '***************************************************************************
    '
    ' Name: DisplayData
    '
    ' Function to Display the data in the listview control
    '
    '***************************************************************************

    Private Function DisplayData() As Integer

        Dim lRow As Integer
        Dim oItem As System.Windows.Forms.ListViewItem
        Dim sKey As String
        Dim sCode As String
        Dim lReturn As Integer


        Try

        DisplayData = gPMConstants.PMEReturnCode.PMTrue

        ' Loop through the data array
        lstInstances.Items.Clear()

        cmdOK.Enabled = True

        If IsArray(g_vUserData) = True Then
            'RSC
            If UBound(g_vUserData, 2) > 0 Then
                cmdOK.Enabled = False
            End If
            'RSC
            For lRow = LBound(g_vUserData, 2) To UBound(g_vUserData, 2)

                ' Create key which contains the row number
                sKey = "L" & lRow

                ' Get the code from the array
                'UPGRADE_WARNING: Couldn't resolve default property of object g_vUserData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sCode = Trim(g_vUserData(colUserName, lRow))

                ' Add a new listitem to the listview
                'UPGRADE_WARNING: Lower bound of collection lstInstances.ListItems.ImageList has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'UPGRADE_WARNING: Image property was not upgraded Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="D94970AE-02E7-43BF-93EF-DCFCD10D27B5"'
                oItem = lstInstances.Items.Add(sKey, sCode, "User")


                ' Set the other data into the other columns
                'UPGRADE_WARNING: Lower bound of collection oItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'UPGRADE_WARNING: Couldn't resolve default property of object g_vUserData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If oItem.SubItems.Count > 1 Then
                    oItem.SubItems(1).Text = g_vUserData(colLoggedOnAtClient, lRow) & ""
                Else
                    oItem.SubItems.Insert(1, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, g_vUserData(colLoggedOnAtClient, lRow) & ""))
                End If
                'UPGRADE_WARNING: Lower bound of collection oItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
                'UPGRADE_WARNING: Couldn't resolve default property of object g_vUserData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                If oItem.SubItems.Count > 2 Then
                    oItem.SubItems(2).Text = g_vUserData(colLogonTime, lRow) & ""
                Else
                    oItem.SubItems.Insert(2, New System.Windows.Forms.ListViewItem.ListViewSubItem(Nothing, g_vUserData(colLogonTime, lRow) & ""))
                End If
            Next lRow

        End If


        Catch ex As Exception

        ' Error Section.

        DisplayData = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayData", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Function

        End Try
    End Function


    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        ' Click Event Of The OK Button.
        Try

        g_usersLoggedOn = gPMConstants.PMEReturnCode.PMFalse

        ' Unload me.
        Me.Close()



        Catch ex As Exception

        ' Error Section

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try

    End Sub

    Private Sub cmdMessage_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdMessage.Click
        Dim iLoop As Short
        Dim vMachines As Object
        Dim oMsg As frmMessage

        oMsg = New frmMessage

        'UPGRADE_WARNING: Lower bound of array vMachines was changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
        ReDim vMachines(lstInstances.Items.Count)

        For iLoop = 1 To lstInstances.Items.Count
            'UPGRADE_WARNING: Lower bound of collection lstInstances.ListItems has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Lower bound of collection lstInstances.ListItems() has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            'UPGRADE_WARNING: Couldn't resolve default property of object vMachines(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vMachines(iLoop) = lstInstances.Items.Item(iLoop).SubItems(1).Text
        Next

        'UPGRADE_WARNING: Couldn't resolve default property of object vMachines. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        oMsg.Machines = vMachines

        If Not (lstInstances.FocusedItem Is Nothing) Then
            'UPGRADE_WARNING: Lower bound of collection lstInstances.SelectedItem has changed from 1 to 0. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="A3B628A0-A810-4AE2-BFA2-9E7A29EB9AD0"'
            oMsg.Machine = lstInstances.FocusedItem.SubItems(1).Text
        Else
            oMsg.Machine = ""
        End If

        oMsg.ShowDialog()

        'UPGRADE_NOTE: Object oMsg may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        oMsg = Nothing

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOk.Click

        ' Click Event Of The OK Button.
        Try
        If lstInstances.Items.Count < 2 Then
            g_usersLoggedOn = gPMConstants.PMEReturnCode.PMTrue
        Else
            g_usersLoggedOn = gPMConstants.PMEReturnCode.PMFalse
        End If
        'there should only be one user left, so grab their username

        If lstInstances.Items.Count = 0 Then
            Me.Close()
            Exit Sub
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object g_vUserData(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
        g_sUsername = g_vUserData(0, 0)


        ' Unload me.
        Me.Close()



        Catch ex As Exception

        ' Error Section

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Exit Sub

        End Try
    End Sub

    Private Sub cmdRefresh_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdRefresh.Click
        Dim iFilenum As Short


        m_lReturn = GetShowData()
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            iFilenum = FreeFile
            Exit Sub

        End If

        m_lReturn = DisplayData()
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

            iFilenum = FreeFile

            Exit Sub

        End If

        tmrRefreshInstances.Enabled = False
        tmrRefreshInstances.Enabled = True
    End Sub

    Private Sub cmdReset_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReset.Click
        Dim retval As Short

        'If nothing is selected then prompt the user via a message box to
        'make a selection
        If (lstInstances Is Nothing) Then
            MsgBox("A User needs to be selected" & Chr(10) & "before this command can be executed", MsgBoxStyle.OKOnly + MsgBoxStyle.Exclamation, "Licence Admin")
            Exit Sub
        Else
            retval = MsgBox("Reset the selected login?", MsgBoxStyle.Question + MsgBoxStyle.OKCancel, "Find")
            If retval = MsgBoxResult.OK Then
                'UPGRADE_WARNING: Couldn't resolve default property of object g_oBusiness.UpdatePMUser. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                Call g_oBusiness.UpdatePMUser(lstInstances.FocusedItem)
                Call cmdRefresh_Click(cmdRefresh, New System.EventArgs())
            End If
        End If

        Exit Sub
    End Sub

    Private Sub frmUsers_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        ' Sets up the forms defaults.

        Dim lErrorValue As Integer

        ' Set the form's default values
        lErrorValue = SetFormDefaults()

        ' Check for errors.
        If (lErrorValue <> gPMConstants.PMEReturnCode.PMTrue) Then
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set the form's defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

            Exit Sub
        End If

    End Sub

    'UPGRADE_WARNING: Form event frmUsers.Activate has a new behavior. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6BA9B8D2-2A32-4B6E-8D36-44949974A5B4"'
    Private Sub frmUsers_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        Dim lLicLimit As Integer

        ' Sets up the form before displaying.

        Static bFormActivated As Boolean
        Dim lErrorValue As Integer

        Try

        ' Check the static flag to see if this
        ' function has been called.
        If (bFormActivated = True) Then
            Exit Sub
        End If

        ' Set the static flag to true to indicate
        ' we have called this function.
        bFormActivated = True

        m_lReturn = DisplayData()
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Me.Close()
            Exit Sub
        End If

        tmrRefreshInstances.Enabled = True

        'Set some defaults

        cmdReset.Enabled = False



        Catch ex As Exception

        ' Error Section

        m_lReturn = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the form ", vApp:=ACApp, vClass:=ACClass, vMethod:="Activate", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        Me.Close()
        Exit Sub

        End Try
    End Sub
	
	Private Sub tmrRefreshInstances_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles tmrRefreshInstances.Tick
		
		'calls to refresh data
		tmrRefreshInstances.Enabled = False
		Call cmdRefresh_Click(cmdRefresh, New System.EventArgs())
		Call DisplayData()
		tmrRefreshInstances.Enabled = True
		
	End Sub
End Class
