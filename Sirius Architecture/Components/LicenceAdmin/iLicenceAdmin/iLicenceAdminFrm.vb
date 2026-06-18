Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Linq
Imports System.Xml.Linq
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic.Compatibility.VB6
'Developer Guide No: 129
Imports SharedFiles

Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 18 July 1996
    '
    ' Description: Main View Form.
    '
    ' Edit History:
    ' Updated 28/04/98
    '
    'changes made to data displayed
    'RFC080399 - Update Licence Limit added.
    'DAK060100 - Add Product Licencing
    ' ***************************************************************** '
    Private Const kLicenceFileError As String = "Licence File Not Foud." & vbCrLf & "Click on ""Update Licence -> Update System Licence"" to locate and store the licence file."

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)

    Public m_vDatArray As Object
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Form cancelled flag.
    Private m_bCancelled As Boolean

    ' Form error number.
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    ' Object parameter member.
    Private m_vObjectParam As Object


    Private m_lReturn As Integer

    ' Declare an instance of the Business object.

    Private m_oBusiness As bPMLicenceAdmin.LicenceAdmin
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    'Declare the column numbers from where the data will sought from
    Const colUserName As Integer = 0
    Const colLogonTime As Integer = 2
    Const colLoggedOnAtClient As Integer = 1
    'DAK060100
    Const colTaskInstance As Integer = 1

    Const colLicenceLimit As Integer = 0



    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)


    ' PRIVATE Property Procedures (Begin)

    Public Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the form.
            Return m_lErrorNumber

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the current form's error number.
            m_lErrorNumber = Value

        End Set
    End Property

    Public Property Cancelled() As Boolean
        Get

            ' Standard Property.

            ' Return if the form has been cancelled.
            Return m_bCancelled

        End Get
        Set(ByVal Value As Boolean)

            ' Standard Property.

            ' Set the form's cancelled flag.
            m_bCancelled = Value

        End Set
    End Property

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    'RFC080399 - Update Licence Limit added.
    ' ***************************************************************** '
    ' Name: UpdateLicenceLimit
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Sub UpdateLicenceLimit()
        Dim fLicenceLimit As frmLicenceLimit
        Dim lReturn, lNewLimit As Integer

        Try

            fLicenceLimit = New frmLicenceLimit()
            fLicenceLimit.Business = m_oBusiness

            VB6.ShowForm(fLicenceLimit, FormShowConstants.Modal, Me)

            If fLicenceLimit.Status = gPMConstants.PMEReturnCode.PMOK Then
                Dim LicenceKeyPath As String = ""
                gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.kRegKeyLicenseKeyPath, r_sSettingValue:=LicenceKeyPath)
                If String.IsNullOrEmpty(LicenceKeyPath) OrElse Not File.Exists(LicenceKeyPath) Then
                    Using New Centered_MessageBox(Me)
                        MessageBox.Show(kLicenceFileError, "Licence File Error", MessageBoxButtons.OK)
                    End Using
                Else

                        Dim xml As New XDocument()
                        xml = XDocument.Load(LicenceKeyPath)
                        Dim licenceLimit As Integer = Convert.ToInt32(xml.Element("License").Element("Quantity").Value)
                        Dim IAttributes As New Generic.List(Of XAttribute)
                        IAttributes = xml.Element("License").Element("LicenseAttributes").Elements("Attribute").Attributes.ToList()
                        Dim blinkToStart As Long = Convert.ToInt32(IIf(String.IsNullOrEmpty(IAttributes(2).Parent.Value), 1, IAttributes(2).Parent.Value))

                        Dim licenceValidity As Date = Convert.ToDateTime(xml.Element("License").Element("Expiration").Value) '= Convert.ToDateTime(xml.Element("License").Element("LicenseAttributes").Element("Attribute("License Validity").Value).Date
                        lNewLimit = licenceLimit
                        lReturn = ShowLicenceDetails(r_lLicenceLimit:=CStr(lNewLimit), licenceValidity, blinkToStart)
                    End If
                End If
            fLicenceLimit.Close()
            fLicenceLimit = Nothing

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLicenceLimitFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLicenceLimit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ReadLicenceFile
    '
    ' Description:
    '
    ' History: 04/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Sub ReadLicenceFile()
        Try
            Dim LicenceKeyPath As String = ""
            gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:="LicenceKeyPath", r_sSettingValue:=LicenceKeyPath)
            If String.IsNullOrEmpty(LicenceKeyPath) OrElse Not File.Exists(LicenceKeyPath) Then
                Using New Centered_MessageBox(Me)
                    MessageBox.Show(kLicenceFileError, "Licence File Error", MessageBoxButtons.OK)
                End Using
            Else
                Dim xml As XElement = XElement.Load(LicenceKeyPath)
                Dim frmLicenceFile As New frmProductLimit()
                frmLicenceFile.XmlFiledata = xml.ToString()
                frmLicenceFile.ShowDialog()
            End If

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReadLicenceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReadLicenceFile", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try

    End Sub

    '*****************************************************************************
    '
    ' Name: GetShowData
    '
    ' Function to call the functions to get the data and populate the listview box
    '
    '*****************************************************************************
    Private Function GetShowData() As Integer
        Dim result As Integer = 0
        Dim vUserdata As Object
        Dim lProductLimit As Integer
        Dim iIsWarnAboveLicenceLimit As Integer
        Dim lWarnsSinceLicenceUpgrade As Integer
        Dim vProductData As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'call the function selectdata from the business

            m_lReturn = m_oBusiness.selectdata(r_vUserdataArray:=vUserdata)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                MessageBox.Show("Unable to Read Data from Sirius Architecture Database" & Strings.Chr(13) & Strings.Chr(10) & "Licence Admin will be shut down.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'call the funtion to display the data

            m_lReturn = DisplayData(r_vUserdataarray:=vUserdata)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the Listview box", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Put the number of items in the listview box in Number allocated box
            'Developer Guide No: 26
            lblNumAllocated.Text = lstInstances.Items.Count

            'Developer Guide No: 170
            ListView6Func.ListViewAutoSize(lstInstances)

            Return result

        Catch excep As System.Exception

            'Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Obtain And display data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetShowData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '***************************************************************************
    '
    ' Name: DisplayData
    '
    ' Function to Display the data in the listview control
    '
    '***************************************************************************

    Private Function DisplayData(ByRef r_vUserdataarray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oItem As ListViewItem
        Dim sKey, sCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop through the data array
            lstInstances.Items.Clear()
            If Information.IsArray(r_vUserdataarray) Then

                For lRow As Integer = r_vUserdataarray.GetLowerBound(1) To r_vUserdataarray.GetUpperBound(1)

                    ' Create key which contains the row number
                    sKey = "L" & lRow

                    ' Get the code from the array

                    sCode = CStr(r_vUserdataarray(colUserName, lRow)).Trim()

                    ' Add a new listitem to the listview
                    oItem = lstInstances.Items.Add(sKey, sCode, "")

                    ' Set the other data into the other columns

                    ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(r_vUserdataarray(colLoggedOnAtClient, lRow)) & ""

                    ListViewHelper.GetListViewSubItem(oItem, 2).Text = CStr(r_vUserdataarray(colLogonTime, lRow)) & ""
                Next lRow
            End If
            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*******************************************************************
    '
    ' Nam showLicencedetails
    '
    ' this function display the nubers of licences available for
    ' this computer
    '*******************************************************************

    Private Function ShowLicenceDetails(ByRef r_lLicenceLimit As String, ByVal LicenceValidity As Date, ByVal BlinkToStart As Long) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No: 26
            lblLicenceLimit1.Text = r_lLicenceLimit
            lblLicenceValidity.Text = LicenceValidity.ToShortDateString()
            If (LicenceValidity.Date < DateTime.Now.Date) Then
                lblLicenceValidity.ForeColor = Color.Red
            ElseIf (Microsoft.VisualBasic.DateDiff(DateInterval.Day, DateTime.Now.Date, Convert.ToDateTime(LicenceValidity.Date))) <= BlinkToStart - 1 Then
                Timer1.Interval = 500
                Timer1.Enabled = True
                Timer1_Tick(Nothing, Nothing) '<- The event takes 2 arguments
                Timer1.Start()
            Else
                lblLicenceValidity.ForeColor = Color.Green
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowLicenceDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        lblLicenceValidity.ForeColor = Color.Red
        lblLicenceValidity.Visible = Not lblLicenceValidity.Visible
    End Sub
    ' ***************************************************************** '
    ' Name: SetFormDefaults
    '
    ' Description: Get and display all of the form's default values.
    '
    ' ***************************************************************** '
    Private Function SetFormDefaults() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get all lookup details

            ' {* USER DEFINED CODE (Begin) *}
            Dim iLanguageID As Integer
            result = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageID)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If iLanguageID = 2 Then
                Me.Text = "Licence Adminstrator"
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the forms defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFormDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub chkAutoRef_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAutoRef.CheckStateChanged

        'if the auto refresh check box is ticked the correct caption is displayed beside
        'it and anything that needs to be enabled, is and anything that needs to be
        'disabled, is.
        If chkAutoRef.CheckState = CheckState.Checked Then
            lblYN.Text = "Yes"
            cmdRefresh.Enabled = False
            'VB6.SetDefault(cmdRefresh, False
            frarefresh.Enabled = True
            lblDsplMins.Enabled = True
            lblDsplMins.Text = CStr(sldRefresh.Value)
            lblMinutes.Enabled = True
            lblSlow.Enabled = True
            lblFast.Enabled = True
            tmrRefreshInstances.Enabled = True

            'else if the auto refresh check box is  not ticked the correct caption is displayed beside
            'it and anything that needs to be enabled, is and anything that needs to be
            'disabled, is.

        ElseIf (chkAutoRef.CheckState = CheckState.Unchecked) Then
            lblYN.Text = "No"
            cmdRefresh.Enabled = True
            VB6.SetDefault(cmdRefresh, True)
            frarefresh.Enabled = False
            lblDsplMins.Enabled = False
            lblDsplMins.Text = ""
            lblMinutes.Enabled = False
            lblSlow.Enabled = False
            lblFast.Enabled = False
            tmrRefreshInstances.Enabled = False
        End If
    End Sub



    Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click

        cmdReset.Enabled = False
        'When the refresh button is clicked the function to populate the listview box
        ' is called
        GetShowData()
    End Sub

    Private Sub cmdReset_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReset.Click
        Dim retval As DialogResult

        'If nothing is selected then prompt the user via a message box to
        'make a selection
        'DAK070100
        If SSTabHelper.GetSelectedIndex(tabMainTab) = 0 Then

            If lstInstances.FocusedItem Is Nothing OrElse lstInstances.SelectedItems.Count = 0 Then
                MessageBox.Show("A User needs to be selected" & Strings.Chr(10).ToString() & "before this command can be executed", "Licence Admin", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            Else
                retval = MessageBox.Show("Reset the selected login?", "Find", MessageBoxButtons.OKCancel, MessageBoxIcon.Question)
                If retval = System.Windows.Forms.DialogResult.OK Then

                    m_sUniqueId = GetUniqueID()
                    m_sScreenHierarchy = $"Licence Administrator({lstInstances.SelectedItems.Item(0).Text.Trim()})"
                    m_oBusiness.UpdatePMUser(lstInstances.SelectedItems.Item(0).Text, m_sUniqueId, m_sScreenHierarchy)
                    GetShowData()
                    'lstInstances.TopItem.Focused = True
                End If
            End If
            If lstInstances.Items.Count = 0 Then
                cmdReset.Enabled = False
            End If

        End If

    End Sub

    ' PRIVATE Methods (End)

    Private Sub Form_Initialize_Renamed()

        ' Forms Initialise Event.
        Dim lErrorValue As Integer

        Try

            ' Default LanguageID, SourceID & CallingAppName
            g_iLanguageID = 1
            g_iSourceID = 1
            g_iCurrencyID = 1
            '   g_sCallingAppName = "LICENCEMANAGER"
            g_iUserID = 1

            ' Create an instance of the busniess
            ' object.

            m_oBusiness = New bPMLicenceAdmin.LicenceAdmin()


            ' Call the initialise method.
            'Developer Guide No: 97
            lErrorValue = m_oBusiness.Initialise()
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                MessageBox.Show("Unable to Connect to Sirius Architecture Database" & Strings.Chr(13) & Strings.Chr(10) & "Licence Admin will be shut down.", "Critical Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Cancelled = True
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the form object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            Dim lLicLimit As Integer

            ' Sets up the form before displaying.

            Static bFormActivated As Boolean

            Try

                ' Check the static flag to see if this
                ' function has been called.
                If bFormActivated Then
                    Exit Sub
                End If

                ' Set the static flag to true to indicate
                ' we have called this function.
                bFormActivated = True

                Dim LicenceKeyPath As String = ""
                gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:="LicenceKeyPath", r_sSettingValue:=LicenceKeyPath)
                If String.IsNullOrEmpty(LicenceKeyPath) OrElse Not File.Exists(LicenceKeyPath) Then
                    Using New Centered_MessageBox(Me)
                        MessageBox.Show(kLicenceFileError, "Licence File Error", MessageBoxButtons.OK)
                    End Using
                Else
                    Try
                        Dim xml As New XDocument()
                        xml = XDocument.Load(LicenceKeyPath)
                        Dim licenceLimit As Integer = Convert.ToInt32(xml.Element("License").Element("Quantity").Value)
                        Dim licenceValidity = Convert.ToDateTime(xml.Element("License").Element("Expiration").Value).Date
                        Dim IAttributes As New Generic.List(Of XAttribute)
                        IAttributes = xml.Element("License").Element("LicenseAttributes").Elements("Attribute").Attributes.ToList()
                        Dim blinkToStart As Long = Convert.ToInt32(IIf(String.IsNullOrEmpty(IAttributes(2).Parent.Value), 1, IAttributes(2).Parent.Value))

                        m_lReturn = ShowLicenceDetails(r_lLicenceLimit:=CStr(licenceLimit), licenceValidity, blinkToStart)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Me.Close()
                            Exit Sub
                        End If
                    Catch
                        Using New Centered_MessageBox(Me)
                            MessageBox.Show("Licence File Is InValid", "Licence File Error", MessageBoxButtons.OK)
                        End Using
                    End Try
                End If

                m_lReturn = GetShowData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Me.Close()
                    Exit Sub
                End If
                m_lReturn = tmrRefreshInstances.Enabled

                'Set some defaults

                lblDsplMins.Text = CStr(sldRefresh.Value)
                cmdReset.Enabled = False

                Exit Sub

            Catch excep As System.Exception

                ' Error Section

                ErrorNumber = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the form ", vApp:=ACApp, vClass:=ACClass, vMethod:="Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Me.Close()
                Exit Sub

            End Try
        End If
    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.Alt And e.KeyCode = Keys.S Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.P Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.R Then
            tabMainTab.SelectedIndex = 2
        End If
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Sets up the forms defaults.


        ' If we failed to Initialise (Cancelled = True)
        If Cancelled Then
            ' Unload Form
            Me.Close()
            Exit Sub
        End If

        ' Set the form's default values
        Dim lErrorValue As Integer = SetFormDefaults()

        ' Check for errors.
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            ErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set the form's defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

            Exit Sub
        End If

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click Event Of The OK Button.
        Try

            ' Unload me.
            Me.Close()

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        FormResize()
    End Sub
    Private Sub FormResize()
        If isInitializingComponent Then
            Exit Sub
        End If

        If VB6.PixelsToTwipsY(Me.ClientRectangle.Height) = 0 And VB6.PixelsToTwipsX(Me.ClientRectangle.Width) = 0 Then
            Exit Sub
        End If

        'Set minimum limits
        If VB6.PixelsToTwipsY(Me.Height) < 5310 Then
            Me.Height = VB6.TwipsToPixelsY(5310)
        End If



        If VB6.PixelsToTwipsX(Me.Width) < 6105 Then
            Me.Width = VB6.TwipsToPixelsX(6105)
        End If


        'Size the buttons
        cmdRefresh.Top = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdRefresh.Height) - 90)
        cmdReset.Top = cmdRefresh.Top
        cmdOK.Top = cmdRefresh.Top
        cmdHelp.Top = cmdRefresh.Top
        cmdHelp.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(cmdHelp.Width) - 90)
        cmdOK.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdHelp.Left) - VB6.PixelsToTwipsX(cmdOK.Width) - 90)

        ''Size the tabs
        'Developer Guide No: 284
        tabMainTab.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - VB6.PixelsToTwipsY(cmdRefresh.Height) - 600)
        tabMainTab.Width = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(Me.ClientRectangle.Width) - VB6.PixelsToTwipsX(tabMainTab.Left) - 90)

        ''Size the lists
        lstInstances.Height = tabMainTab.Height - VB6.TwipsToPixelsY(1300)
        lstInstances.Width = tabMainTab.Width - VB6.TwipsToPixelsX(250)
        'imgSystemIcon.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMainTab.Width) - VB6.PixelsToTwipsX(imgSystemIcon.Width) - 100)

    End Sub

    Private Sub lstInstances_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lstInstances.ColumnClick
        Dim ColumnHeader As ColumnHeader = lstInstances.Columns(eventArgs.Column)

        Static lOrder As SortOrder

        If ColumnHeader.Index + 1 <= 2 Then
            With lstInstances
                If ListViewHelper.GetSortKeyProperty(lstInstances) <> ColumnHeader.Index + 1 - 1 Then
                    ListViewHelper.SetSortKeyProperty(lstInstances, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortOrderProperty(lstInstances, SortOrder.Ascending)
                Else
                    If ListViewHelper.GetSortOrderProperty(lstInstances) = SortOrder.Ascending Then
                        ListViewHelper.SetSortOrderProperty(lstInstances, SortOrder.Descending)
                    Else
                        ListViewHelper.SetSortOrderProperty(lstInstances, SortOrder.Ascending)
                    End If
                End If

                ListViewHelper.SetSortedProperty(lstInstances, True)
            End With
            lOrder = SortOrder.Descending
        Else
            If lOrder = SortOrder.Ascending Then
                lOrder = SortOrder.Descending
            Else
                lOrder = SortOrder.Ascending
            End If

            'Developer Guide No: 170
            ListView6Func.ListViewSortByDate(lstInstances, ColumnHeader.Index + 1 - 1, lOrder)
        End If

    End Sub
    Private Sub lstInstances_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstInstances.Click

        If lstInstances.FocusedItem Is Nothing Then
            Exit Sub
        End If

        cmdReset.Enabled = (lstInstances.FocusedItem.Text <> "")
    End Sub

    Public Sub mnuExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExit.Click
        Me.Close()
    End Sub

    'DAK040100
    Public Sub mnuReadLicenceFile_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuReadLicenceFile.Click
        ReadLicenceFile()
    End Sub

    Public Sub mnuUpdateLicenceLimit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuUpdateLicenceLimit.Click
        ' Update the Licence Limit
        '    UpdateLicenceLimit
    End Sub

    'DAK040100
    Public Sub mnuUpdateSystemLimit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuUpdateSystemLimit.Click
        ' Update the System Licence Limit
        UpdateLicenceLimit()
    End Sub

    Private Sub sldRefresh_Change()

        'When the slider changes set the lbldsplmins caption to equal the value of the slider
        ' and also enable the timer
        lblDsplMins.Text = CStr(sldRefresh.Value)
        tmrRefreshInstances.Enabled = True
    End Sub

    Private Sub lblDsplMins_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblDsplMins.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        If lblDsplMins.Text = "1" Then
            lblMinutes.Text = "Minute"
        Else
            lblMinutes.Text = "Minutes"
        End If

    End Sub

    Private Sub tabMainTab_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles tabMainTab.GotFocus
        lstInstances.TabStop = False
        cmdOK.TabStop = True
        cmdRefresh.TabStop = True
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        cmdReset.Enabled = False

        tabMainTabPreviousTab = tabMainTab.SelectedIndex
    End Sub


    Private Sub tmrRefreshInstances_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrRefreshInstances.Tick

        Dim lErrorResults As Integer
        Static iCounter As Integer

        Try



            tmrRefreshInstances.Enabled = gPMConstants.PMEReturnCode.PMTrue

            'if the counter value is equal to the value lbldsplmins caption then Refresh
            'the details from the business object to the form. if it's not equal then
            'increment the value of the counter by one.
            If iCounter = CDbl(lblDsplMins.Text) - 1 Then
                lErrorResults = GetShowData()
                iCounter = 0
            ElseIf (iCounter <> CDbl(lblDsplMins.Text)) Then
                iCounter += 1
                Exit Sub
            End If

            ' Check for errors
            If lErrorResults <> gPMConstants.PMEReturnCode.PMTrue Then
                tmrRefreshInstances.Enabled = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the form details", vApp:=ACApp, vClass:=ACClass, vMethod:="tmrRefreshInstances_Timer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Developer Guide No: 289
    Private Sub sldRefresh_Scroll(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles sldRefresh.Scroll
        'if the auto refresh check box is ticked the correct caption is displayed beside
        'it and anything that needs to be enabled, is and anything that needs to be
        'disabled, is.
        If chkAutoRef.CheckState = CheckState.Checked Then
            lblYN.Text = "Yes"
            cmdRefresh.Enabled = False
            'VB6.SetDefault(cmdRefresh, False)
            frarefresh.Enabled = True
            lblDsplMins.Enabled = True
            lblDsplMins.Text = CStr(sldRefresh.Value)
            ToolTip1.SetToolTip(sldRefresh, sldRefresh.Value)
            ToolTip1.Show(sldRefresh.Value, sldRefresh, (sldRefresh.Location.X - 29) + (sldRefresh.Value * 4.5), sldRefresh.Location.Y - 63)
            lblMinutes.Enabled = True
            lblSlow.Enabled = True
            lblFast.Enabled = True
            tmrRefreshInstances.Enabled = True

            'else if the auto refresh check box is  not ticked the correct caption is displayed beside
            'it and anything that needs to be enabled, is and anything that needs to be
            'disabled, is.

        ElseIf (chkAutoRef.CheckState = CheckState.Unchecked) Then
            lblYN.Text = "No"
            cmdRefresh.Enabled = True
            VB6.SetDefault(cmdRefresh, True)
            frarefresh.Enabled = False
            lblDsplMins.Enabled = False
            lblDsplMins.Text = ""
            lblMinutes.Enabled = False
            lblSlow.Enabled = False
            lblFast.Enabled = False
            tmrRefreshInstances.Enabled = False
        End If
    End Sub

    Private Sub cmdOK_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdOK.GotFocus
        cmdReset.TabStop = False
        tabMainTab.TabStop = False
        lstInstances.TabStop = True
    End Sub

    Private Sub lstInstances_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lstInstances.GotFocus
        cmdReset.TabStop = True
        cmdOK.TabStop = False
        cmdRefresh.TabStop = False
        tabMainTab.TabStop = True
    End Sub


    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown
        If e.Alt And e.KeyCode = Keys.S Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.P Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.R Then
            tabMainTab.SelectedIndex = 2
        End If
    End Sub

    Private Sub frmInterface_GotFocus(sender As Object, e As EventArgs) Handles Me.GotFocus
        Dim licenseKeyPath As String = ""
        gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:="LicenseKeyPath", r_sSettingValue:=licenseKeyPath)
        If String.IsNullOrEmpty(licenseKeyPath) OrElse Not File.Exists(licenseKeyPath) Then
            Using New Centered_MessageBox(Me)
                MessageBox.Show(kLicenceFileError, "Licence File Error", MessageBoxButtons.OK)
            End Using
            Exit Sub
        End If
        Dim xml As New XDocument()
        xml = XDocument.Load(licenseKeyPath)
        Dim licenceLimit As Integer = Convert.ToInt32(xml.Element("License").Element("Quantity").Value)
        Dim licenceValidity As Date = Convert.ToDateTime(xml.Element("License").Element("LicenseAttributes").Attribute("License Validity").Value).Date
    End Sub
End Class
