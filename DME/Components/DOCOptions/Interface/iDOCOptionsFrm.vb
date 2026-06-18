Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: {17/2/98}
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' SP270898 - Do not reset 00 tree location as thats just pointless
    '
    ' JH021198 - moved getregistryvalue and setregistryvalue to
    '            docGeneralFunc for use by other modules.
    '
    ' JH051198 - new combo box - MaxAutoExpand - folder processing
    '
    ' JH301198 - change validation of path put in for data share
    '            including the check for c:\ type of paths
    '
    ' JH190599 - keep the three entries for data store
    ' separate but make it look like one
    '
    ' MS290600  - SetInterfaceDefaults :  Registry for Briefcase dir
    '
    ' MS070700  - SeparateUNC. Change made in order to save on local hard drive
    '
    ' MS250900  - Automatically fire up the annotations and keywords windows after an
    '             import of a file. RSAIB request
    '
    ' DN270802 - Change embedded SQL to reflect table changes
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'developer guide no.7
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    '***Insert Form Constants***

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}


    ' Declare an instance of the Business object.

    Private m_oBusinessForm As Object

    Private m_oBusinessBusiness As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PW131003 - CQ1413
    Private m_bInitialisingForm As Boolean


    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)


    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            '    m_lreturn& = m_oBusiness.GetDetails()

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            '    If (m_lreturn& <> PMTrue) Then
            '        ' Failed to get details.
            '        GetBusiness = PMFalse
            '
            '        ' Log Error.
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to get details from the business object", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="GetBusiness"
            '
            '        Exit Function
            '    End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' EDIT HISTORY:
    '
    '   MS290600
    '       Read the dir path (BriefcaseDir)as specified in the  registry.
    '       This is where the user specified to store the documents during
    '       Briefcase download mode
    '
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim bPMBenv As Boolean
        Dim sDir As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' PW131003 - CQ1413
            m_bInitialisingForm = True

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            SSTabHelper.SetSelectedIndex(tabOptions, 0)

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Load settings from the registry and database
            m_lReturn = CType(LoadSettings(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the configuration settings
            With Me

                ' Manager options
                If g_ManagerOptions.DisplayFolders.sValue = "Y" Then
                    '.cboDisplayFolders.Text = "Y"
                    .chkDisplayFolders.CheckState = CheckState.Checked
                Else
                    '.cboDisplayFolders.Text = "N"
                    .chkDisplayFolders.CheckState = CheckState.Unchecked
                End If

                If g_ManagerOptions.StartHome.sValue = "Y" Then
                    '.cboStartHome.Text = "Y"
                    .chkHomeFolder.CheckState = CheckState.Checked
                Else
                    '.cboStartHome.Text = "N"
                    .chkHomeFolder.CheckState = CheckState.Unchecked
                End If

                If g_ManagerOptions.MaxFolders.sValue = "0" Then
                    g_ManagerOptions.MaxFolders.sValue = "All"
                End If
                .cboMaxFolders.Text = g_ManagerOptions.MaxFolders.sValue

                If g_ManagerOptions.MaxFilters.sValue = "0" Then
                    g_ManagerOptions.MaxFilters.sValue = "All"
                End If
                .cboMaxFilter.Text = g_ManagerOptions.MaxFilters.sValue

                If StringsHelper.ToDoubleSafe(g_ManagerOptions.MaxAutoExpand.sValue) = 0 Then
                    g_ManagerOptions.MaxAutoExpand.sValue = CStr(DOCDefaultMaxAutoExpand)
                End If
                .cboMaxAuto.Text = g_ManagerOptions.MaxAutoExpand.sValue

                If g_ManagerOptions.WANOptimise.sValue = "Y" Then
                    .chkWANOptimise.CheckState = CheckState.Checked
                Else
                    .chkWANOptimise.CheckState = CheckState.Unchecked
                End If

                ' RDC 23062005
                If g_ViewerOptions.AllowCopyPaste.sValue = "1" Then
                    .chkAllowCopyPaste.CheckState = CheckState.Checked
                Else
                    .chkAllowCopyPaste.CheckState = CheckState.Unchecked
                End If

                ' Config options
                .txtCacheLocation.Text = g_ConfigOptions.CacheLocation.sValue
                '.txtDirectory.Text = g_ConfigOptions.DocuDir.sValue
                '.txtServer.Text = g_ConfigOptions.DocuServer.sValue

                'JH301198 need to set to 'initialise' so the validate routine doesn't run

                .cmdShareBrowse.Tag = "Initialise"
                .txtShare.Text = g_ConfigOptions.DocuShare.sValue
                .cmdShareBrowse.Tag = "Stopped Initialise"

                If g_ConfigOptions.PrintWord.sValue = "Y" Then
                    .chkPrintW.CheckState = CheckState.Checked
                Else
                    .chkPrintW.CheckState = CheckState.Unchecked
                End If

                If g_ConfigOptions.ViewWord.sValue = "Y" Then
                    .chkViewW.CheckState = CheckState.Checked
                Else
                    .chkViewW.CheckState = CheckState.Unchecked
                End If

                ' Auto Keywords/Annotations fire-up                                      MS250900
                If g_ConfigOptions.AutoKeyword.sValue = "Y" Then
                    .chkAutoKeyword.CheckState = CheckState.Checked
                Else
                    .chkAutoKeyword.CheckState = CheckState.Unchecked
                End If

                ' Document options
                .cboDocumentDate.SelectedIndex = CInt(Conversion.Val(g_DocumentOptions.DocumentData.sValue) - 1)
                .txtDocumentExpiry.Text = g_DocumentOptions.DocumentExpiry.sValue

                ' Warning options
                If g_WarningOptions.ScanExternal.sValue = "Y" Then
                    '.cboScanExternal.Text = "Y"
                    .chkScanExternal.CheckState = CheckState.Checked
                Else
                    '.cboScanExternal.Text = "N"
                    .chkScanExternal.CheckState = CheckState.Unchecked
                End If

                If g_WarningOptions.MoveDocument.sValue = "Y" Then
                    '.cboMoveV2.Text = "Y"
                    .chkMoveV2.CheckState = CheckState.Checked
                Else
                    '.cboMoveV2.Text = "N"
                    .chkMoveV2.CheckState = CheckState.Unchecked
                End If

                ' PW131003 - CQ1413 - set image viewer checkbox depending on
                ' value retrieved from registry
                If g_DocumentOptions.ImageViewer.sValue = "Y" Then
                    .chkImageViewer.CheckState = CheckState.Checked
                Else
                    .chkImageViewer.CheckState = CheckState.Unchecked
                End If

            End With

            g_ManagerOptions.MaxFolders.bChanged = False
            g_ManagerOptions.MaxFilters.bChanged = False
            g_ManagerOptions.DisplayFolders.bChanged = False
            g_ManagerOptions.StartHome.bChanged = False
            g_ManagerOptions.WANOptimise.bChanged = False

            g_ConfigOptions.CacheLocation.bChanged = False
            '    g_ConfigOptions.DocuServer.bChanged = False
            g_ConfigOptions.DocuShare.bChanged = False
            '   g_ConfigOptions.DocuDir.bChanged = False
            g_ConfigOptions.PrintWord.bChanged = False
            g_ConfigOptions.ViewWord.bChanged = False

            g_ConfigOptions.AutoKeyword.bChanged = False 'MS250900

            g_DocumentOptions.DocumentData.bChanged = False
            g_DocumentOptions.DocumentExpiry.bChanged = False
            g_DocumentOptions.ImageViewer.bChanged = False

            g_WarningOptions.MoveDocument.bChanged = False
            g_WarningOptions.ScanExternal.bChanged = False

            'disable the command buttons
            cmdOK.Enabled = False
            cmdApply.Enabled = False

            ' Disable admin only boxes if not an admin
            If Not g_bUserIsAdmin Then

                For Each txtAdmin As Control In Me.Controls
                    If TypeOf txtAdmin Is TextBox Then
                        If Convert.ToString(txtAdmin.Tag) = "admin" Then
                            CType(txtAdmin, TextBox).ReadOnly = True
                            txtAdmin.BackColor = SystemColors.Menu
                        End If
                    End If
                Next txtAdmin
                chkViewW.Enabled = False
                chkPrintW.Enabled = False
                chkWANOptimise.Enabled = False
                cmdDefaultManager.Enabled = False
                cmdShareBrowse.Enabled = False
                chkDisplayFolders.Enabled = False
            End If

            ' CTAF 20040420 - Removed checks for PMB Environment as this is no longer supported
            chkMoveV2.CheckState = CheckState.Unchecked
            chkMoveV2.Enabled = False

            chkScanExternal.CheckState = CheckState.Unchecked
            chkScanExternal.Enabled = False

            'JH301198 check if the share has been previously saved
            'and make sure it is either \\ or c:\ type of path
            If (g_ConfigOptions.DocuShare.sValue.Substring(0, 2) <> "\\") And (g_ConfigOptions.DocuShare.sValue.Substring(1, Math.Min(g_ConfigOptions.DocuShare.sValue.Length, 2)) <> ":\") Then

                'JH190599 keep the three separate but make it look like one

                '        g_ConfigOptions.DocuShare.sValue = g_ConfigOptions.DocuServer.sValue & g_ConfigOptions.DocuShare.sValue & g_ConfigOptions.DocuDir.sValue
                '        frmInterface.txtShare.Text = g_ConfigOptions.DocuShare.sValue
                Me.txtShare.Text = g_ConfigOptions.DocuServer.sValue & g_ConfigOptions.DocuShare.sValue & g_ConfigOptions.DocuDir.sValue

            End If

            'MS290600    >
            ' GET Briefcase dir store (BriefcaseDir)from registry if it exists
            ' Note: this setting saves any path (inc. server path with \\) and will the correct one
            m_lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseDir", r_sSettingValue:=sDir), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the textbox to briefcase store dir (if it exists)
            If sDir > "" Then
                Me.txtShare.Text = sDir
            End If

            Dim cloudHostingOptionValue As String = ""

            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableCloudHosting, v_vBranch:=1, r_vUnderwriting:=cloudHostingOptionValue)

            If (gPMFunctions.NullToString(cloudHostingOptionValue) = "1") Then
                txtShare.Enabled = False
                cmdShareBrowse.Enabled = False
            End If
            'MS290600    <

            ' {* USER DEFINED CODE (End) *}

            ' PW131003 - CQ1413
            m_bInitialisingForm = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim lMaxExceeded As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'so what did the user do.

            Select Case Me.Status
                Case gPMConstants.PMEReturnCode.PMCancel

                    'user cancelled
                    g_vDocsFound = Nothing

                Case gPMConstants.PMEReturnCode.PMOK

                    ' validate the information on the form
                    m_lReturn = CType(ValidateData(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' store the changes in the registry or database
                    m_lReturn = GrabChanges()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try


            ' Display all language specific captions.

            '    Me.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACInterfaceTitle, _
            ''        iDataType:=PMResString)
            '
            '    ' Check for an error.
            '    If (Me.Caption = "") Then
            '        ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse
            '
            '        ' Log Error.
            '        iPMFunc.LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            ''            "Please check the file exists and the correct captions are available", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="DisplayCaptions"
            '
            '        Exit Function
            '    End If

            '    cmdFindNow.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACOKButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdCancel.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCancelButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdHelp.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACHelpButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdNavigate.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACNavigateButton, _
            ''        iDataType:=PMResString)
            '
            '    tabMainTab.TabCaption(0) = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACTabTitle1, _
            ''        iDataType:=PMResString)

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            '***Insert GetRes Calls***

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Private Sub cboDisplayFolders_Change()
    '
    '    cboDisplayFolders_Click
    '
    'End Sub
    '
    'Private Sub cboDisplayFolders_Click()
    '
    ''    g_ManagerOptions.DisplayFolders.bChanged = True
    ''    g_ManagerOptions.DisplayFolders.sValue = cboDisplayFolders.Text
    '
    'End Sub

    Private Sub cboDocumentDate_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDocumentDate.SelectedIndexChanged

        g_DocumentOptions.DocumentData.bChanged = True
        g_DocumentOptions.DocumentData.sValue = cboDocumentDate.Text

        EnableButtons()

    End Sub


    Private isInitializingComponent As Boolean
    Private Sub cboMaxAuto_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMaxAuto.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Dim dbNumericTemp As Double
        If Not Double.TryParse(cboMaxAuto.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
            If g_ManagerOptions.WANOptimise.sValue = "Y" Then
                cboMaxAuto.Text = CStr(DOCDefaultMaxAutoExpandWAN)
            Else
                cboMaxAuto.Text = CStr(DOCDefaultMaxAutoExpand)
            End If
        End If

        cboMaxAuto_SelectionChangeCommitted(cboMaxAuto, New EventArgs())

    End Sub

    Private Sub cboMaxAuto_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMaxAuto.SelectionChangeCommitted

        g_ManagerOptions.MaxAutoExpand.bChanged = True
        g_ManagerOptions.MaxAutoExpand.sValue = cboMaxAuto.Text

        EnableButtons()

    End Sub


    Private Sub cboMaxFilter_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMaxFilter.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cboMaxFilter_SelectionChangeCommitted(cboMaxFilter, New EventArgs())

    End Sub

    Private Sub cboMaxFilter_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMaxFilter.SelectionChangeCommitted

        g_ManagerOptions.MaxFilters.bChanged = True
        g_ManagerOptions.MaxFilters.sValue = cboMaxFilter.Text

        EnableButtons()

    End Sub


    Private Sub cboMaxFolders_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMaxFolders.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        cboMaxFolders_SelectionChangeCommitted(cboMaxFolders, New EventArgs())

    End Sub

    Private Sub cboMaxFolders_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboMaxFolders.SelectionChangeCommitted

        g_ManagerOptions.MaxFolders.bChanged = True
        g_ManagerOptions.MaxFolders.sValue = cboMaxFolders.Text

        EnableButtons()

    End Sub
    '
    'Private Sub cboMoveV2_Change()
    '
    '    cboMoveV2_Click
    '
    'End Sub
    '
    'Private Sub cboMoveV2_Click()
    '
    '    'g_WarningOptions.MoveDocument.bChanged = True
    '    'g_WarningOptions.MoveDocument.sValue = cboMoveV2.Text
    '
    'End Sub

    Private Sub chkAllowCopyPaste_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowCopyPaste.CheckStateChanged

        g_ViewerOptions.AllowCopyPaste.bChanged = True

        If chkAllowCopyPaste.CheckState = CheckState.Checked Then
            g_ViewerOptions.AllowCopyPaste.sValue = "1"
        Else
            g_ViewerOptions.AllowCopyPaste.sValue = "0"
        End If

        EnableButtons()

    End Sub

    ' New function to give the user the option to automatically fire up the annotations and
    ' keywords windows after an import of a file
    ' MS250900
    Private Sub chkAutoKeyword_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAutoKeyword.CheckStateChanged

        g_ConfigOptions.AutoKeyword.bChanged = True

        If chkAutoKeyword.CheckState = CheckState.Checked Then
            g_ConfigOptions.AutoKeyword.sValue = "Y"
        Else
            g_ConfigOptions.AutoKeyword.sValue = "N"
        End If

        EnableButtons()


    End Sub

    'Private Sub cboScanExternal_Change()
    '
    '    cboScanExternal_Click
    '
    'End Sub
    '
    'Private Sub cboScanExternal_Click()
    '
    '    'g_WarningOptions.ScanExternal.bChanged = True
    '    'g_WarningOptions.ScanExternal.sValue = cboScanExternal.Text
    '
    'End Sub
    '
    'Private Sub cboStartHome_Change()
    '
    '    cboStartHome_Click
    '
    'End Sub
    '
    'Private Sub cboStartHome_Click()
    '
    '    'g_ManagerOptions.StartHome.bChanged = True
    '    'g_ManagerOptions.StartHome.sValue = cboStartHome.Text
    '
    'End Sub

    Private Sub chkDisplayFolders_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDisplayFolders.CheckStateChanged

        g_ManagerOptions.DisplayFolders.bChanged = True

        If chkDisplayFolders.CheckState = CheckState.Checked Then
            g_ManagerOptions.DisplayFolders.sValue = "Y"
        Else
            g_ManagerOptions.DisplayFolders.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkHomeFolder_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkHomeFolder.CheckStateChanged

        g_ManagerOptions.StartHome.bChanged = True

        If chkHomeFolder.CheckState = CheckState.Checked Then
            g_ManagerOptions.StartHome.sValue = "Y"
        Else
            g_ManagerOptions.StartHome.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkImageViewer_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkImageViewer.CheckStateChanged

        ' PW131003 - CQ1413 - Don't want to do this if initialising
        If m_bInitialisingForm Then
            Exit Sub
        End If

        g_DocumentOptions.ImageViewer.bChanged = True
        If chkImageViewer.CheckState = CheckState.Checked Then
            g_DocumentOptions.ImageViewer.sValue = "Y"
        Else
            g_DocumentOptions.ImageViewer.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkMoveV2_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkMoveV2.CheckStateChanged

        g_WarningOptions.MoveDocument.bChanged = True

        If chkMoveV2.CheckState = CheckState.Checked Then
            g_WarningOptions.MoveDocument.sValue = "Y"
        Else
            g_WarningOptions.MoveDocument.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkPrintW_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPrintW.CheckStateChanged

        g_ConfigOptions.PrintWord.bChanged = True

        If chkPrintW.CheckState = CheckState.Checked Then
            g_ConfigOptions.PrintWord.sValue = "Y"
        Else
            g_ConfigOptions.PrintWord.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkScanExternal_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkScanExternal.CheckStateChanged

        g_WarningOptions.ScanExternal.bChanged = True

        If chkScanExternal.CheckState = CheckState.Checked Then
            g_WarningOptions.ScanExternal.sValue = "Y"
        Else
            g_WarningOptions.ScanExternal.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkViewW_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkViewW.CheckStateChanged

        g_ConfigOptions.ViewWord.bChanged = True

        If chkViewW.CheckState = CheckState.Checked Then
            g_ConfigOptions.ViewWord.sValue = "Y"
        Else
            g_ConfigOptions.ViewWord.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub chkWANOptimise_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkWANOptimise.CheckStateChanged

        g_ManagerOptions.WANOptimise.bChanged = True

        If chkWANOptimise.CheckState = CheckState.Checked Then
            g_ManagerOptions.WANOptimise.sValue = "Y"
        Else
            g_ManagerOptions.WANOptimise.sValue = "N"
        End If

        EnableButtons()

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        ' Click event of the apply button.

        Try

            m_lReturn = CType(ApplyChanges(), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: ApplyChanges
    '
    ' Description: for use by OK and Apply button
    '
    '
    ' ***************************************************************** '
    Private Function ApplyChanges() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' OK pressed
            g_bOKPressed = True

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'right, lets go to work.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
            Else
                ' Everything OK, so we can disable buttons and exit
                cmdOK.Enabled = False
                cmdApply.Enabled = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ApplyChangesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdCacheBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCacheBrowse.Click

        Dim sFolder As String = ""

        Try

            txtCacheLocation.Focus()

            m_lReturn = BrowseFolder(sFolder:=sFolder, sTitle:="Please browse for a directory." & _
                        Strings.Chr(13).ToString() & _
                        "The cache will be created off this.", hWndParent:=Me.Handle.ToInt32())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then

                txtCacheLocation.Text = sFolder

                If Not sFolder.EndsWith("\") Then
                    sFolder = sFolder & "\"
                End If

                ' the cache directory on the end
                sFolder = sFolder & DOCCacheName

                txtCacheLocation.Text = sFolder

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to browse for a cache folder. Please enter manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCacheBrowse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeafaultDates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeafaultDates.Click

        ' Set the defaults for the current tab

        cboDocumentDate.Text = DOC_DEFAULT_DOCDATE
        txtDocumentExpiry.Text = DOC_DEFAULT_EXPIRYDATE

        chkImageViewer.CheckState = CheckState.Checked

    End Sub

    Private Sub cmdDefaultConfig_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefaultConfig.Click

        ' Set the defaults for the current tab

        txtCacheLocation.Text = "C:\" & DOCCacheName
        'SP270898 - Do not reset 00 tree location as thats just pointless
        '    txtServer.Text = "\\dmsnt"
        '    txtShare.Text = "\pmdir"
        '    txtDirectory.Text = "\data"

        'JH281098 default RTF print/view is 0
        chkPrintW.CheckState = CheckState.Unchecked
        chkViewW.CheckState = CheckState.Unchecked
        ' MS250900
        chkAutoKeyword.CheckState = CheckState.Unchecked

    End Sub

    Private Sub cmdDefaultManager_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefaultManager.Click

        ' Sets the defaults for the current tab

        If g_ManagerOptions.WANOptimise.sValue = "Y" Then
            cboMaxAuto.Text = CStr(DOCDefaultMaxAutoExpandWAN)
            cboMaxFolders.Text = CStr(DOCDefaultMaxAutoExpandWAN)
            cboMaxFilter.Text = CStr(DOCDefaultMaxAutoExpandWAN)
        End If

        chkHomeFolder.CheckState = CheckState.Unchecked
        chkDisplayFolders.CheckState = CheckState.Unchecked

        EnableButtons()

    End Sub

    Private Sub cmdDefaultViewer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefaultViewer.Click

        chkAllowCopyPaste.CheckState = CheckState.Unchecked

    End Sub

    Private Sub cmdDefaultWarnings_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDefaultWarnings.Click

        ' set the defaults for the current tab

        chkMoveV2.CheckState = CheckState.Checked
        chkScanExternal.CheckState = CheckState.Checked

        'cboMoveV2.Text = "Y"
        'cboScanExternal.Text = "Y"

    End Sub

    Private Sub cmdShareBrowse_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdShareBrowse.Click

        Dim sFolder As String = ""

        Try

            txtShare.Focus()

            m_lReturn = BrowseFolder(sFolder:=sFolder, sTitle:="Please browse for a directory." & _
                        Strings.Chr(13).ToString() & _
                        "The documents will be retrieved from this.", hWndParent:=Me.Handle.ToInt32())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then

                txtShare.Text = sFolder

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to browse for a document share folder. Please enter manually.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdShareBrowse_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    ' PRIVATE Methods (End)


    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim lRet As gPMConstants.PMEReturnCode
        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusinessForm As Object
            m_lReturn = CType(g_oObjectManager.GetInstance(temp_m_oBusinessForm, "bDOCOptions.Form", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oBusinessForm = temp_m_oBusinessForm

            Dim temp_m_oBusinessBusiness As Object
            lRet = CType(g_oObjectManager.GetInstance(temp_m_oBusinessBusiness, "bDOCOptions.Business", vInstanceManager:="ClientManager"), gPMConstants.PMEReturnCode)
            m_oBusinessBusiness = temp_m_oBusinessBusiness

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or lRet <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.
                'sTitle$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFailTitle, _
                'iDataType:=PMResString)

                'sMessage$ = GetResData( _
                'iLangID:=g_iLanguageID%, _
                'lID:=ACBusinessFail, _
                'iDataType:=PMResString)

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lRet As Integer

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'developer guide no.7
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If


            ' Terminate the business objects

            m_oBusinessForm.Dispose()
            m_oBusinessBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusinessForm = Nothing
            m_oBusinessBusiness = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' OK not pressed
            g_bOKPressed = False

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = CType(ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the ok button.

        Try

            'separate function for this lot
            '    ' OK not pressed
            '    g_bOKPressed = True
            '
            '    ' Set the interface status.
            '    m_lStatus& = PMOK
            '
            '    'right, lets go to work.
            '    iPMFunc.SetMousePointer PMMouseBusy
            '
            '    ' Process the next set of actions depending
            '    ' upon the interface task etc.
            '    m_lReturn& = ProcessCommand()
            '
            '    'right, lets go to work.
            '    iPMFunc.SetMousePointer PMMouseNormal

            m_lReturn = CType(ApplyChanges(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    '
    '' ****************************************************************************
    '''JH021198 - moved to DOCGeneralFunc
    '
    '' Function : GetRegistryValue
    ''
    '' Desc.    : Gets a value from the Documaster part of the database.
    ''            Always has the "Options" subkey
    ''
    '' ****************************************************************************
    'Private Function GetRegistryValue( _
    ''                                sKey As String, _
    ''                                sSubKey As String, _
    ''                                sValue As String, _
    ''                                iLocation As Integer) As Long
    '
    '    Dim eRegSettingRoot As PMERegSettingRoot
    '    Dim eRegSettingLevel As PMERegSettingLevel
    '    Dim eProductFamily As PMEProductFamily
    '
    '    On Error GoTo Err_GetRegistryValue
    '
    '    GetRegistryValue = PMTrue
    '
    '    If (iLocation = REGISTRY_USER) Then
    '        ' hkey\current user
    '        eRegSettingRoot = pmeRSRCurrentUser
    '    Else
    '        ' hkey\local machine
    '        eRegSettingRoot = pmeRSRLocalMachine
    '    End If
    '
    '    ' client registry
    '    eRegSettingLevel = pmeRSLCommon
    '    ' Documaster, wahey
    '    eProductFamily = pmePFDocumaster
    '
    '    m_lReturn = GetPMRegSetting( _
    ''        v_lPMERegSettingRoot:=eRegSettingRoot, _
    ''        v_lPMEProductFamily:=eProductFamily, _
    ''        v_lPMERegsettinglevel:=eRegSettingLevel, _
    ''        v_sSettingName:=sKey, _
    ''        r_sSettingValue:=sValue, _
    ''        v_sSubKey:=sSubKey)
    '    If (m_lReturn& <> PMTrue) Then
    '        ' oh no, error
    '        GetRegistryValue = PMFalse
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_GetRegistryValue:
    '
    '    GetRegistryValue = PMError
    '
    '    ' Log Error.
    '    iPMFunc.LogMessage _
    ''        iType:=PMLogError, _
    ''        sMsg:="Failed to get value from the registry.", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetRegistryValue", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '
    '' ****************************************************************************
    ''
    '' Function : SetRegistryValue
    ''
    '' Desc.    : Sets a value in the Documaster part of the database.
    ''            Always has the "Options" subkey.
    ''
    '' ****************************************************************************
    'Private Function SetRegistryValue( _
    ''                                sKey As String, _
    ''                                sSubKey As String, _
    ''                                sValue As String, _
    ''                                iLocation As Integer) As Long
    '
    '    Dim eRegSettingRoot As PMERegSettingRoot
    '    Dim eRegSettingLevel As PMERegSettingLevel
    '    Dim eProductFamily As PMEProductFamily
    '
    '    On Error GoTo Err_SetRegistryValue
    '
    '    SetRegistryValue = PMTrue
    '
    '    If (iLocation = REGISTRY_USER) Then
    '        ' hkey\current user
    '        eRegSettingRoot = pmeRSRCurrentUser
    '    Else
    '        ' hkey\local machine
    '        eRegSettingRoot = pmeRSRLocalMachine
    '    End If
    '
    '    ' client registry
    '    eRegSettingLevel = pmeRSLCommon
    '    ' Documaster, wahey
    '    eProductFamily = pmePFDocumaster
    '
    '    ' Set the registry
    '    m_lReturn = SetPMRegSetting( _
    ''        v_lPMERegSettingRoot:=eRegSettingRoot, _
    ''        v_lPMEProductFamily:=eProductFamily, _
    ''        v_lPMERegsettinglevel:=eRegSettingLevel, _
    ''        v_sSettingName:=sKey, _
    ''        v_sSettingValue:=sValue, _
    ''        v_sSubKey:=sSubKey)
    '    If (m_lReturn& <> PMTrue) Then
    '        ' oh no, error
    '        SetRegistryValue = PMFalse
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_SetRegistryValue:
    '
    '    SetRegistryValue = PMError
    '
    '    ' Log Error.
    '    iPMFunc.LogMessage _
    ''        iType:=PMLogError, _
    ''        sMsg:="Failed to set value in the registry.", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="SetRegistryValue", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    ' ************************************************************************
    '
    ' Function: SaveSetting
    '
    ' Desc.   : Writes a settings to either the registry, or the database.
    '
    ' ************************************************************************

    Private Function SaveSetting(ByRef vSetting As DOCGeneralFunc.Setting_Type) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' ALL must be converted to 0
            If vSetting.sValue.ToUpper() = "ALL" Then
                vSetting.sValue = "0"
            End If


            Select Case vSetting.sSource
                Case CStr(SOURCE_REGISTRY)

                    ' Write it back to registry now
                    m_lReturn = CType(SetRegistryValue(sSubKey:=vSetting.sSubKey, sKey:=vSetting.sKey, sValue:=vSetting.sValue, iLocation:=vSetting.iLocation), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case CStr(SOURCE_DATABASE)


                    m_lReturn = m_oBusinessForm.SetSetting(sTable:=vSetting.sTable, sColumn:=vSetting.sColumn, sValue:=vSetting.sValue, bNumber:=vSetting.bNumber)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RDC 23062005
                Case CStr(SOURCE_DBASEV2)


                    m_lReturn = m_oBusinessBusiness.SetOption(sOptionName:=vSetting.sOptionName, sOptionValue:=vSetting.sValue)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else

                    ' Log Error.
                    ' CTAF 20030827 - Changed vMethod from LoadSetting
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid setting source. Must be either Registry or Database, not " & vSetting.sSource & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSetting", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select


            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set value in the registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ************************************************************************
    '
    ' Function: LoadSetting
    '
    ' Desc.   : Loads a settings from either the registry, or the database,
    '           setting the defaults if needed.
    '
    ' ************************************************************************

    Private Function LoadSetting(ByRef vSetting As DOCGeneralFunc.Setting_Type) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case vSetting.sSource
                Case CStr(SOURCE_REGISTRY)

                    ' Read from the registry
                    m_lReturn = CType(GetRegistryValue(sKey:=vSetting.sKey, sSubKey:=vSetting.sSubKey, sValue:=vSetting.sValue, iLocation:=vSetting.iLocation), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case CStr(SOURCE_DATABASE)


                    m_lReturn = m_oBusinessForm.GetSetting(sTable:=vSetting.sTable, sColumn:=vSetting.sColumn, sValue:=vSetting.sValue)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' RDC 23062005
                Case CStr(SOURCE_DBASEV2)


                    m_lReturn = m_oBusinessBusiness.GetOption(sOptionName:=vSetting.sOptionName, sOptionValue:=vSetting.sValue)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case Else

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid setting source. Must be either Registry or Database, not " & vSetting.sSource & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSetting", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End Select

            ' Apply the default if necessary
            If vSetting.sValue = "" Then

                vSetting.sValue = vSetting.sDefault

                ' Write it back to registry now
                m_lReturn = CType(SaveSetting(vSetting:=vSetting), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get value from the registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ************************************************************************
    '
    ' Function: LoadSettings
    '
    ' Desc.   : Loads the settings from either the registry, or the database,
    '           setting the defaults if needed.
    '
    ' ************************************************************************

    Private Function LoadSettings() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' {{{{{{{ Manager Tab }}}}}}

            g_ManagerOptions.MaxFolders.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.MaxFolders.iLocation = REGISTRY_USER
            g_ManagerOptions.MaxFolders.sKey = DOCMaxFoldersKey
            g_ManagerOptions.MaxFolders.sSubKey = DOCOptionsSection
            g_ManagerOptions.MaxFolders.sDefault = "All"
            g_ManagerOptions.MaxFilters.bNumber = False

            g_ManagerOptions.MaxFilters.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.MaxFilters.iLocation = REGISTRY_USER
            g_ManagerOptions.MaxFilters.sKey = DOCMaxFilterFoldersKey
            g_ManagerOptions.MaxFilters.sSubKey = DOCOptionsSection
            g_ManagerOptions.MaxFilters.sDefault = "50"
            g_ManagerOptions.MaxFilters.bNumber = False

            g_ManagerOptions.MaxAutoExpand.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.MaxAutoExpand.iLocation = REGISTRY_USER
            g_ManagerOptions.MaxAutoExpand.sKey = DOCMaxAutoExpandKey
            g_ManagerOptions.MaxAutoExpand.sSubKey = DOCOptionsSection
            g_ManagerOptions.MaxAutoExpand.sDefault = CStr(DOCDefaultMaxAutoExpand)
            g_ManagerOptions.MaxAutoExpand.bNumber = True

            g_ManagerOptions.DisplayFolders.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.DisplayFolders.iLocation = REGISTRY_USER
            g_ManagerOptions.DisplayFolders.sKey = DOCDisplayFoldersOnRightKey
            g_ManagerOptions.DisplayFolders.sSubKey = DOCOptionsSection
            g_ManagerOptions.DisplayFolders.sDefault = "N"
            g_ManagerOptions.DisplayFolders.bNumber = False

            g_ManagerOptions.StartHome.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.StartHome.iLocation = REGISTRY_USER
            g_ManagerOptions.StartHome.sKey = DOCStartHome
            g_ManagerOptions.StartHome.sSubKey = DOCOptionsSection
            g_ManagerOptions.StartHome.sDefault = "N"
            g_ManagerOptions.StartHome.bNumber = False

            g_ManagerOptions.WANOptimise.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.WANOptimise.iLocation = REGISTRY_USER
            g_ManagerOptions.WANOptimise.sKey = DOCWANOptimiseKey
            g_ManagerOptions.WANOptimise.sSubKey = DOCOptionsSection
            g_ManagerOptions.WANOptimise.sDefault = "N"
            g_ManagerOptions.WANOptimise.bNumber = False

            g_ManagerOptions.ShowAnnotations.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.ShowAnnotations.iLocation = REGISTRY_USER
            g_ManagerOptions.ShowAnnotations.sKey = DOCExtrasKeywordsKey
            g_ManagerOptions.ShowAnnotations.sSubKey = DOCStartUpSection
            g_ManagerOptions.ShowAnnotations.sDefault = "Y"
            g_ManagerOptions.ShowAnnotations.bNumber = False

            g_ManagerOptions.ShowKeyWords.sSource = CStr(SOURCE_REGISTRY)
            g_ManagerOptions.ShowKeyWords.iLocation = REGISTRY_USER
            g_ManagerOptions.ShowKeyWords.sKey = DOCExtrasAnnotationsKey
            g_ManagerOptions.ShowKeyWords.sSubKey = DOCStartUpSection
            g_ManagerOptions.ShowKeyWords.sDefault = "Y"
            g_ManagerOptions.ShowKeyWords.bNumber = False

            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.DisplayFolders), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.MaxAutoExpand), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.MaxFilters), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.MaxFolders), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.StartHome), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.WANOptimise), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.ShowAnnotations), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ManagerOptions.ShowKeyWords), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result





            ' RDC 23062005
            ' {{{{{{{ Viewer Tab }}}}}}
            g_ViewerOptions.AllowCopyPaste.sSource = CStr(SOURCE_DBASEV2)
            g_ViewerOptions.AllowCopyPaste.sOptionName = OPTIONS_VIEWER_ALLOW_CUT_PASTE
            g_ViewerOptions.AllowCopyPaste.sDefault = "0"
            g_ViewerOptions.AllowCopyPaste.bNumber = True

            m_lReturn = CType(LoadSetting(vSetting:=g_ViewerOptions.AllowCopyPaste), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result


            ' {{{{{{{ Configuration Tab }}}}}}

            g_ConfigOptions.CacheLocation.sSource = CStr(SOURCE_REGISTRY)
            g_ConfigOptions.CacheLocation.iLocation = REGISTRY_SYSTEM
            g_ConfigOptions.CacheLocation.sKey = DOCCacheLocationKey
            g_ConfigOptions.CacheLocation.sSubKey = "Cache"
            g_ConfigOptions.CacheLocation.sDefault = "C:\" & DOCCacheName
            g_ConfigOptions.CacheLocation.bNumber = False

            g_ConfigOptions.DocuServer.sSource = CStr(SOURCE_DATABASE)
            g_ConfigOptions.DocuServer.sColumn = "server_unc"
            g_ConfigOptions.DocuServer.sTable = "DOC_device"
            g_ConfigOptions.DocuServer.sDefault = "\\ntsrv"
            g_ConfigOptions.DocuServer.bNumber = False

            g_ConfigOptions.DocuShare.sSource = CStr(SOURCE_DATABASE)
            g_ConfigOptions.DocuShare.sColumn = "share_name"
            g_ConfigOptions.DocuShare.sTable = "DOC_device"
            g_ConfigOptions.DocuShare.sDefault = "\dme"
            g_ConfigOptions.DocuShare.bNumber = False

            g_ConfigOptions.DocuDir.sSource = CStr(SOURCE_DATABASE)
            g_ConfigOptions.DocuDir.sColumn = "directory"
            g_ConfigOptions.DocuDir.sTable = "DOC_volume"
            g_ConfigOptions.DocuDir.sDefault = "\data"
            g_ConfigOptions.DocuDir.bNumber = False

            g_ConfigOptions.PrintWord.sSource = CStr(SOURCE_REGISTRY)
            g_ConfigOptions.PrintWord.iLocation = REGISTRY_USER
            g_ConfigOptions.PrintWord.sKey = DOCPrintWordKey
            g_ConfigOptions.PrintWord.sSubKey = DOCOptionsSection
            g_ConfigOptions.PrintWord.sDefault = "N"
            g_ConfigOptions.PrintWord.bNumber = False

            g_ConfigOptions.ViewWord.sSource = CStr(SOURCE_REGISTRY)
            g_ConfigOptions.ViewWord.iLocation = REGISTRY_USER
            g_ConfigOptions.ViewWord.sKey = DOCViewWordKey
            g_ConfigOptions.ViewWord.sSubKey = DOCOptionsSection
            g_ConfigOptions.ViewWord.sDefault = "N"
            g_ConfigOptions.ViewWord.bNumber = False

            'MS250900
            g_ConfigOptions.AutoKeyword.sSource = CStr(SOURCE_REGISTRY)
            g_ConfigOptions.AutoKeyword.iLocation = REGISTRY_USER
            g_ConfigOptions.AutoKeyword.sKey = DOCVAutoKeyword
            g_ConfigOptions.AutoKeyword.sSubKey = DOCOptionsSection
            g_ConfigOptions.AutoKeyword.sDefault = "N"
            g_ConfigOptions.AutoKeyword.bNumber = False

            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.CacheLocation), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.DocuDir), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.DocuServer), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.DocuShare), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.ViewWord), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.PrintWord), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            'MS250900
            m_lReturn = CType(LoadSetting(vSetting:=g_ConfigOptions.AutoKeyword), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            ' {{{{{{{ Document Tab }}}}}}

            g_DocumentOptions.DocumentData.sSource = CStr(SOURCE_DATABASE)
            g_DocumentOptions.DocumentData.sTable = "DOC_system"
            g_DocumentOptions.DocumentData.sColumn = "doc_date"
            g_DocumentOptions.DocumentData.sDefault = "2"
            g_DocumentOptions.DocumentData.bNumber = True

            g_DocumentOptions.DocumentExpiry.sSource = CStr(SOURCE_DATABASE)
            g_DocumentOptions.DocumentExpiry.sTable = "DOC_system"
            g_DocumentOptions.DocumentExpiry.sColumn = "expiry_date"
            g_DocumentOptions.DocumentExpiry.sDefault = "365"
            g_DocumentOptions.DocumentExpiry.bNumber = True

            g_DocumentOptions.ImageViewer.sSource = CStr(SOURCE_REGISTRY)
            g_DocumentOptions.ImageViewer.iLocation = REGISTRY_USER
            g_DocumentOptions.ImageViewer.sKey = DOCDefaultImageViewer
            g_DocumentOptions.ImageViewer.sSubKey = DOCOptionsSection
            g_DocumentOptions.ImageViewer.sDefault = "N"
            g_DocumentOptions.ImageViewer.bNumber = False

            m_lReturn = CType(LoadSetting(vSetting:=g_DocumentOptions.DocumentData), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_DocumentOptions.DocumentExpiry), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_DocumentOptions.ImageViewer), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result





            ' {{{{{{{ Warnings Tab }}}}}}
            g_WarningOptions.MoveDocument.sSource = CStr(SOURCE_REGISTRY)
            g_WarningOptions.MoveDocument.iLocation = REGISTRY_USER
            g_WarningOptions.MoveDocument.sKey = DOCScanToExternalWarning
            g_WarningOptions.MoveDocument.sSubKey = DOCOptionsSection
            g_WarningOptions.MoveDocument.sDefault = "Y"
            g_WarningOptions.MoveDocument.bNumber = False

            g_WarningOptions.ScanExternal.sSource = CStr(SOURCE_REGISTRY)
            g_WarningOptions.ScanExternal.iLocation = REGISTRY_USER
            g_WarningOptions.ScanExternal.sKey = DOCMoveToNonFolderWarning
            g_WarningOptions.ScanExternal.sSubKey = DOCOptionsSection
            g_WarningOptions.ScanExternal.sDefault = "Y"
            g_WarningOptions.ScanExternal.bNumber = False

            m_lReturn = CType(LoadSetting(vSetting:=g_WarningOptions.MoveDocument), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            m_lReturn = CType(LoadSetting(vSetting:=g_WarningOptions.ScanExternal), gPMConstants.PMEReturnCode)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result





            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get values from the registry and database.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ************************************************************************
    '
    ' Function: GrabChanges
    '
    ' Desc.   : Checks if any of the settings have been changed, and if so,
    '           then it saves them.
    '
    '
    ' Edit History:
    '
    '       MS290600
    '       Update registry with dir path specified in frmInterface.txtShare.Text
    '       Note: the database table will store only the UNC path
    '       (i.e. \\<server>\<sharename>\<dirpath>)
    '
    '
    ' ************************************************************************

    Private Function GrabChanges() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            'set the WAN Optimise stuff before grabbing changes
            If (g_ManagerOptions.WANOptimise.bChanged) And (g_ManagerOptions.WANOptimise.sValue = "Y") Then

                'change these only if the user hasn't already
                If Not g_ManagerOptions.MaxFolders.bChanged Then
                    cboMaxFolders.Text = CStr(DOCDefaultMaxAutoExpandWAN)
                End If

                If Not g_ManagerOptions.MaxFilters.bChanged Then
                    cboMaxFilter.Text = CStr(DOCDefaultMaxAutoExpandWAN)
                End If

                If Not g_ManagerOptions.MaxAutoExpand.bChanged Then
                    cboMaxAuto.Text = CStr(DOCDefaultMaxAutoExpandWAN)
                End If

                chkDisplayFolders.CheckState = CheckState.Unchecked

                'plus don't show annotations or keywords in main view
                g_ManagerOptions.ShowAnnotations.bChanged = True
                g_ManagerOptions.ShowAnnotations.sValue = "N"
                g_ManagerOptions.ShowKeyWords.bChanged = True
                g_ManagerOptions.ShowKeyWords.sValue = "N"

            End If

            ' Manager options
            If g_ManagerOptions.DisplayFolders.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.DisplayFolders), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ManagerOptions.MaxAutoExpand.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.MaxAutoExpand), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ManagerOptions.MaxFilters.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.MaxFilters), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ManagerOptions.MaxFolders.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.MaxFolders), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ManagerOptions.StartHome.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.StartHome), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ManagerOptions.WANOptimise.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.WANOptimise), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result




            End If

            If g_ManagerOptions.ShowKeyWords.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.ShowKeyWords), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ManagerOptions.ShowAnnotations.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ManagerOptions.ShowAnnotations), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If


            ' RDC 23062005
            If g_ViewerOptions.AllowCopyPaste.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ViewerOptions.AllowCopyPaste), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result
            End If


            ' Config Options
            If g_ConfigOptions.CacheLocation.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.CacheLocation), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ConfigOptions.DocuDir.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.DocuDir), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ConfigOptions.DocuServer.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.DocuServer), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ConfigOptions.DocuShare.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.DocuShare), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            ' MS290600

            ' Update Briefcase dir store in registry with path specified in frmInterface.txtShare.Text
            m_lReturn = CType(SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFDocumaster, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="BriefcaseDir", v_sSettingValue:=Me.txtShare.Text), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If



            ' Word Options
            If g_ConfigOptions.PrintWord.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.PrintWord), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_ConfigOptions.ViewWord.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.ViewWord), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            ' Auto Keywords/Annotations fire-up option      MS250900
            If g_ConfigOptions.AutoKeyword.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_ConfigOptions.AutoKeyword), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            ' Document Options
            If g_DocumentOptions.DocumentData.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_DocumentOptions.DocumentData), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_DocumentOptions.DocumentExpiry.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_DocumentOptions.DocumentExpiry), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_DocumentOptions.ImageViewer.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_DocumentOptions.ImageViewer), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If


            ' Warning options
            If g_WarningOptions.MoveDocument.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_WarningOptions.MoveDocument), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If

            If g_WarningOptions.ScanExternal.bChanged Then
                m_lReturn = CType(SaveSetting(vSetting:=g_WarningOptions.ScanExternal), gPMConstants.PMEReturnCode)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then Return result



            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save changed values.", vApp:=ACApp, vClass:=ACClass, vMethod:="GrabChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub txtCacheLocation_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCacheLocation.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        g_ConfigOptions.CacheLocation.bChanged = True
        g_ConfigOptions.CacheLocation.sValue = txtCacheLocation.Text.Trim()

        EnableButtons()

    End Sub

    'Private Sub txtDirectory_Change()
    '
    '    g_ConfigOptions.DocuDir.bChanged = True
    '    g_ConfigOptions.DocuDir.sValue = Trim(txtDirectory.Text)
    '
    'End Sub

    Private Sub txtDocumentExpiry_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentExpiry.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        g_DocumentOptions.DocumentExpiry.bChanged = True
        g_DocumentOptions.DocumentExpiry.sValue = txtDocumentExpiry.Text

        EnableButtons()

    End Sub



    'Private Sub txtServer_Change()
    '
    '    g_ConfigOptions.DocuServer.bChanged = True
    '    g_ConfigOptions.DocuServer.sValue = Trim(txtServer.Text)
    '
    'End Sub


    ' ***************************************************************** '
    ' Name: ValidatePath
    '
    ' JH301198
    ' Description: to validate the path put in for data share
    '              including the check for c:\ type of paths
    '
    '
    ' ***************************************************************** '
    Private Function ValidatePath(ByRef sText As String) As Integer

        Dim result As Integer = 0
        Dim bvalid As Boolean
        Dim sMsg As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(IsPath(sPath:=sText, bvalid:=bvalid), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'problem
                Return gPMConstants.PMEReturnCode.PMFalse

            End If
            If Not bvalid Then

                If sText.Substring(1, Math.Min(sText.Length, 2)) = ":\" Then

                    sMsg = "The value of 'DocuMaster Share' is invalid." & _
                           Strings.Chr(13).ToString() & Strings.Chr(13).ToString() & _
                           "Client/Server environment paths must begin with \\" & _
                           Strings.Chr(13).ToString() & Strings.Chr(13).ToString() & _
                           "Are you running a server only setup?"


                    Select Case MessageBox.Show(sMsg, "Invalid Value", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error)
                        Case System.Windows.Forms.DialogResult.No, System.Windows.Forms.DialogResult.Cancel
                            Return gPMConstants.PMEReturnCode.PMFalse

                        Case System.Windows.Forms.DialogResult.Yes
                            'ok carry on

                        Case Else
                            'just in case!
                            Return gPMConstants.PMEReturnCode.PMFalse

                    End Select

                Else
                    'anything except c:\ and \\ type paths

                    sMsg = "The value of 'DocuMaster Share' is invalid." & _
                           Strings.Chr(13).ToString() & Strings.Chr(13).ToString() & _
                           "Please check spelling and try again."

                    MessageBox.Show(sMsg, Application.ProductName, MessageBoxButtons.OK)

                    Return gPMConstants.PMEReturnCode.PMFalse

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateData() As Integer

        Dim result As Integer = 0
        Dim bvalid As Boolean
        Dim sText, sMsg As String
        Dim iVal As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' manager tab

            ' max folders returned
            sText = cboMaxFolders.Text
            m_lReturn = CType(IsAlphaAll(sText:=sText, bCheckAll:=True, bAlphaAll:=bvalid), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not bvalid Then
                SSTabHelper.SetSelectedIndex(tabOptions, 0)
                cboMaxFolders.Focus()
                sMsg = "The value of 'Maximum Folders Returned' is invalid." & _
                       Strings.Chr(13).ToString() & Strings.Chr(13).ToString() & _
                       "Possible values are 'All' or a number."
                MessageBox.Show(sMsg, "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' max filters returned
            sText = cboMaxFilter.Text
            m_lReturn = CType(IsAlphaAll(sText:=sText, bCheckAll:=True, bAlphaAll:=bvalid), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not bvalid Then
                SSTabHelper.SetSelectedIndex(tabOptions, 0)
                cboMaxFilter.Focus()
                sMsg = "The value of 'Filter Maximum Folders Returned' is invalid." & _
                       Strings.Chr(13).ToString() & Strings.Chr(13).ToString() & _
                       "Possible values are 'All' or a number."
                MessageBox.Show(sMsg, "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' config tab

            'JH281098 - not using separate server/share/folder path now
            ' txtserver
            '    sText = txtServer.Text
            '    m_lReturn = IsUNCPath(sPath:=sText, bValid:=bValid)
            '    If (m_lReturn <> PMTrue) Then
            '        ValidateData = PMFalse
            '        Exit Function
            '    End If
            '    If (bValid = False) Then
            '        tabOptions.Tab = 1
            '        txtServer.SetFocus
            '        sMsg = "The value of 'DocuMaster Server' is invalid." & _
            ''            Chr$(13) & Chr$(13) & _
            ''            "Server paths must begin with \\"
            '        MsgBox sMsg, vbOKOnly + vbCritical, "Invalid Value"
            '        ValidateData = PMFalse
            '        Exit Function
            '    End If

            ' txtshare

            'put into the change routine

            '    sText = txtShare.Text
            '    m_lReturn = IsPath(sPath:=sText, bValid:=bValid)
            '    If (m_lReturn <> PMTrue) Then
            '        ValidateData = PMFalse
            '        Exit Function
            '    End If
            '    If (bValid = False) Then
            '        tabOptions.Tab = 1
            '        txtShare.SetFocus
            '
            '        sMsg = "The value of 'DocuMaster Share' is invalid." & _
            ''            Chr$(13) & Chr$(13) & _
            ''            "For Client/Server setup paths must begin with \" & _
            ''            Chr$(13) & Chr$(13) & _
            ''            "Are you running a server only setup?"
            '
            '        If MsgBox(sMsg, vbYesNoCancel + vbCritical, "Invalid Value") = vbNo Then
            '            ValidateData = PMFalse
            '            Exit Function
            '        Else
            '            'ok carry on
            '        End If
            '    End If

            ' txtpath
            '    sText = txtDirectory.Text
            '    m_lReturn = IsPath(sPath:=sText, bValid:=bValid)
            '    If (m_lReturn <> PMTrue) Then
            '        ValidateData = PMFalse
            '        Exit Function
            '    End If
            '    If (bValid = False) Then
            '        tabOptions.Tab = 1
            '        txtDirectory.SetFocus
            '        sMsg = "The value of 'DocuMaster Directory' is invalid." & _
            ''            Chr$(13) & Chr$(13) & _
            ''            "Paths must begin with \"
            '        MsgBox sMsg, vbOKOnly + vbCritical, "Invalid Value"
            '        ValidateData = PMFalse
            '        Exit Function
            '    End If


            ' document tab
            iVal = Conversion.Val(txtDocumentExpiry.Text)
            If (iVal < 1) Or (iVal > 365) Then
                SSTabHelper.SetSelectedIndex(tabOptions, 2)
                txtDocumentExpiry.Focus()
                sMsg = "The value of 'Document Expiry Date' is invalid." & _
                       Strings.Chr(13).ToString() & Strings.Chr(13).ToString() & _
                       "Possible range is from 1 to 365."
                MessageBox.Show(sMsg, "Invalid Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' warnings tab



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to validate data.", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ************************************************************************
    '
    ' Function : IsAlphaAll
    '
    ' Desc.    : Checks if a string is either "All" or a number
    '
    ' ************************************************************************

    Private Function IsAlphaAll(ByRef sText As String, ByRef bCheckAll As Boolean, ByRef bAlphaAll As Boolean) As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""
        Dim iLen As Integer
        Dim sChar As New FixedLengthString(1)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' convert to uppercase and remove leading/trailing spaces
            sTemp = sText.Trim().ToUpper()

            ' get the length
            iLen = sTemp.Length

            ' Do we want to check for "All" ?
            If bCheckAll Then
                ' Check if 'ALL'
                If iLen = 3 Then
                    If sTemp = "ALL" Then
                        bAlphaAll = True
                        Return result
                    End If
                End If
            End If

            ' go through each character and decide if number
            For iLoop1 As Integer = 1 To iLen

                ' get the next character to check
                sChar.Value = sTemp.Substring(iLoop1 - 1, 1)

                ' less than a 0?
                If Strings.Asc(sChar.Value(0)) < Strings.Asc("0"c) Then
                    bAlphaAll = False
                    Return result
                End If

                ' greater than a 9?
                If Strings.Asc(sChar.Value(0)) > Strings.Asc("9"c) Then
                    bAlphaAll = False
                    Return result
                End If


            Next iLoop1

            ' all ok!
            bAlphaAll = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to determine if string was alphanumeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAlphaAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function IsPath(ByRef sPath As String, ByRef bvalid As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' must be atleast two characters (eg. \a)
            If sPath.Length < 2 Then
                bvalid = False
                Return result
            End If

            ' must begin with a \
            If sPath.Substring(0, 1) <> "\" Then
                bvalid = False
                Return result
            End If

            bvalid = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to determine if path is valid.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAlphaAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (IsUNCPath) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsUNCPath(ByRef sPath As String, ByRef bvalid As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' has to be more than two characters long for a start
    'If sPath.Length < 2 Then
    'bvalid = False
    'Return result
    'End If
    '
    'If sPath.Substring(0, 2) <> "\\" Then
    'bvalid = False
    'Return result
    'End If
    '
    'bvalid = True
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to determine if string was a valid UNC path.", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUNCPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    Private Sub txtShare_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShare.Enter

        txtShare.Tag = txtShare.Text

    End Sub
    ' ***************************************************************** '
    ' Name: EnableButtons
    '
    ' Description: Just enables the ok and apply buttons
    '              when anything is changed
    '
    '
    ' ***************************************************************** '
    Private Sub EnableButtons()

        Try

            cmdOK.Enabled = True
            cmdApply.Enabled = True

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableButtonsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtShare_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtShare.Leave

        If Convert.ToString(cmdShareBrowse.Tag) <> "Initialise" Then
            m_lReturn = CType(ValidatePath(txtShare.Text), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'user cancelled or there was an error
                txtShare.Text = Convert.ToString(txtShare.Tag)
                Exit Sub
            End If

        End If

        '    g_ConfigOptions.DocuShare.sValue = Trim(txtShare.Text)

        'JH190599 routine to separate the three from one string
        m_lReturn = CType(SeparateUNC(), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'error has occured and don't save changes
            g_ConfigOptions.DocuShare.bChanged = False
            g_ConfigOptions.DocuServer.bChanged = False
            g_ConfigOptions.DocuDir.bChanged = False
            txtShare.Text = Convert.ToString(txtShare.Tag)
        Else
            g_ConfigOptions.DocuShare.bChanged = True
            g_ConfigOptions.DocuServer.bChanged = True
            g_ConfigOptions.DocuDir.bChanged = True
            txtShare.Tag = txtShare.Text
        End If

        EnableButtons()

    End Sub

    ' ***************************************************************** '
    ' Name: SeparateUNC
    '
    ' Description: JH190599 routine to separate the
    '               three database entries from one string
    '
    '
    '   Edit History:
    '
    '   MS070700
    '      Exit function if the path does not start with "\\"
    '      in order to save the local hard drive path dir correctly
    '
    ' ***************************************************************** '

    Function SeparateUNC() As Integer

        Dim result As Integer = 0
        Dim iLen, iSlashPos As Integer
        Dim sShare(2) As String
        Dim sUNC As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sUNC = txtShare.Text.Trim()

            iLen = sUNC.Length

            ' MS070700
            ' If no UNC supplied then exit function as this means local drive was specified as the dir store
            If sUNC.Substring(0, 2) <> "\\" Then
                Return result
            End If

            For iCount As Integer = 0 To 1
                For iSlashPos = iLen To 1 Step -1
                    If (sUNC.Substring(iSlashPos - 1, 1) = "\") Or (sUNC.Substring(iSlashPos - 1, 1) = "/") Then

                        Exit For
                    End If
                Next iSlashPos

                sShare(iCount) = sUNC.Substring(sUNC.Length - (iLen - (iSlashPos - 1))) '"\data"
                sUNC = sUNC.Substring(0, iSlashPos - 1)
                iLen = sUNC.Length
            Next iCount

            sShare(2) = sUNC

            g_ConfigOptions.DocuServer.sValue = sShare(2)
            g_ConfigOptions.DocuShare.sValue = sShare(1)
            g_ConfigOptions.DocuDir.sValue = sShare(0)

            Return result

        Catch ex As Exception



            result = gPMConstants.PMEReturnCode.PMFalse
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed Separate the Path", vApp:=ACApp, vClass:=ACClass, vMethod:="SeparateUNC", excep:=ex)

            Return result
        End Try
    End Function
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub
End Class