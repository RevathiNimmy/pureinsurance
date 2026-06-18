Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Artinsoft.VB6.VB
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Reflection
Partial Friend Class frmParentMDI
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmParentMDI
    '
    ' Date: 14/05/1998
    '
    ' Description: TIF Viewer Child Form
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant to identify which class this is.
    Const ACClass As String = "frmParentMDI"

    ' PRIVATE Data Members (Begin)
    ' Stores the return value for a function call.
    Private m_lReturn As Integer

    Const LOAD_RTF_DOC As Integer = 1
    Const UNLOAD_RTF_DOC As Integer = 2
    Const TERMINATE_VIEWER As Integer = 3

    ' Instance of iDOCSplash
    Private m_oSplash As iDOCSplash.Interface_Renamed

    Private m_iCurrentPrintPage As Integer
    Private m_bLastPage As Boolean
    Private m_iPagesToPrint As Integer
    Private m_bThumbnails As Boolean

    Private m_vFileNames As Object

    'ND 061100
    Private m_bUnloadForm As Boolean

    Public WriteOnly Property UnloadForm() As Boolean
        Set(ByVal Value As Boolean)

            m_bUnloadForm = Value

        End Set
    End Property
    'ND END


    'Private Sub cboPages_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

    '    Try

    '        Dim iPageNum As Integer

    '        
    '        'If ActiveMdiChild.ViewerType = ACViewerTypeTIF Then
    '        If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

    '            If Not bDontUpdate Then
    '                iPageNum = Conversion.Val(Me.cboPages.Text)
    '                
    '                'm_lReturn = ActiveMdiChild.DisplayPage(iPageNum:=iPageNum)
    '                m_lReturn = ReflectionHelper.Invoke(ActiveMdiChild, "DisplayFirstPage", New Object() {iPageNum})

    '                
    '                
    '                Me._StbInfo_Panel1.Text = "Page " & _
    '                                                ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") & " of " & _
    '                                                ReflectionHelper.GetMember(ActiveMdiChild, "PageTotal")
    '            End If

    '        End If

    '    Catch excep As System.Exception



    '        ' Log Error.
    '        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process combo box click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboPages_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

    '        Exit Sub

    '    End Try

    'End Sub

    Private Sub frmParentMDI_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        'if parent only form then disable buttons
        If Me.MdiChildren.Length = 0 Then
            For iLoop1 As Integer = 1 To Toolbar.Items.Count
                Toolbar.Items.Item(iLoop1 - 1).Enabled = Not (Toolbar.Items.Item(iLoop1 - 1).Name <> "ReturnManager")
            Next iLoop1
            ' and the combo box
            Me.cboPages.Enabled = False

            ' and the menu's
            Me.mnuView.Enabled = False
            Me.mnuImage.Enabled = False
            Me.mnuWindow.Enabled = False
            Me.mnuPrint.Enabled = False
            Me.mnuInfo.Enabled = False
            Me.mnuArchive.Enabled = False 'MS 15/05/01


        End If
    End Sub


    Private Sub frmParentMDI_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            m_bUnloadForm = False

            ' RDC 08122004 BriefcaseViewer should not do this
            If g_iUserID <> USER_IS_BRIEFCASE Then
                ' Intialise the doc information object
                If g_oDOCInformation Is Nothing Then

                    'g_oDOCInformation = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCInformation.Interface")
                    g_oDOCInformation = New iDOCInformation.Interface_Renamed

                    'm_lReturn = CType(g_oDOCInformation, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                    m_lReturn = g_oDOCInformation.Initialise()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of iDOCInformation.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="MDIForm_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If


                End If

                ' Create an instance of the Splash object (for printing)
                If m_oSplash Is Nothing Then

                    'm_oSplash = System.Runtime.InteropServices.Marshal.GetActiveObject("iDOCSplash.Interface")
                    m_oSplash = New iDOCSplash.Interface_Renamed

                    'm_lReturn = CType(m_oSplash, SSP.S4I.Interfaces.ILocalInterface).Initialise()
                    m_lReturn = m_oSplash.Initialise()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of iDOCSplash.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="MDIForm_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If

                End If

            End If

            ' Get the Thumbnails registry setting

            Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
            Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
            Dim eProductFamily As gPMConstants.PMEProductFamily
            Const sDOCThumbnails As String = "Thumbnails"
            Dim sTmp As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode


            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster
            m_bThumbnails = False

            lReturn = CType(GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sDOCThumbnails, r_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_bThumbnails = False
            End If

            If sTmp = "Y" Then
                m_bThumbnails = True
                CType(Toolbar.Items.Item("Thumbnails"), ToolStripButton).Checked = True
                mnuShowThumbnails.Checked = True
            End If
            StbInfo.Width = Me.ClientRectangle.Width
            Me._StbInfo_Panel3.Width = StbInfo.Width * (10 / 100)
            Me._StbInfo_Panel4.Width = StbInfo.Width * (10 / 100)
            Me._StbInfo_Panel1.Width = StbInfo.Width * (10 / 100)
            Me._StbInfo_Panel2.Width = StbInfo.Width * (40 / 100)

            _StbInfo_Panel4.Text = (CDate(DateTime.Now.ToString("HH:mm:ss")).ToString("HH:MM")).ToString
            StbInfo.Refresh()
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on loading MDI Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuBold_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmParentMDI_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If Not m_bUnloadForm Then
            'DN 21/05/02 - Close all docs before hiding the form
            mnuFileCloseAll_Click(mnuFileCloseAll, New EventArgs())
            Me.Hide()
            Cancel = 1
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Public Sub mnuArchive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuArchive.Click

        Dim lReturn As Integer
        If MessageBox.Show("Proceed with document archive?", "Archive Document", MessageBoxButtons.YesNo) <> System.Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If


        'Dim sCommandLine As String = "ARCHIVE" & ActiveMdiChild.DocumentKey
        Dim sCommandLine As String = "ARCHIVE" & ReflectionHelper.GetMember(ActiveMdiChild, "DocumentKey")


        ' need to clear current doc
        ReflectionHelper.Invoke(ActiveMdiChild, "Close", New Object() {})

        ' activate Documaster with an Archive request

        lReturn = g_frmManager.FrmManager.Activate(sCommandLine)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' oh no..
            MessageBox.Show("Failed to activate DocuMaster Enterprise.", DOCAppName)

            g_frmManager.FrmManager.Dispose()
            g_frmManager = Nothing
            Exit Sub
        End If

        ' release reference
        g_frmManager = Nothing
    End Sub

    ' **********************************************************************
    '
    ' Function    : mnuHelpAbout
    '
    ' Description : Displays the standard Policy Master about screen modally
    '
    ' **********************************************************************
    Public Sub mnuHelpAbout_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuHelpAbout.Click

        ' Code needed to display standard PM About screen

        Dim oSirAbout As iPMAbout.Interface_Renamed

        Dim sTitle, sVersionNumber, sVersionDate, sComponent As String


        Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_mnuHelpAbout_Click)")

        ' Set the application title
        sTitle = "DocuMaster Enterprise"

        ' Set the version number and date
        sVersionNumber = CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision)

        Try

            sVersionDate = DateTimeHelper.ToString((New FileInfo(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".exe")).LastWriteTime)

        Catch
        End Try



        sComponent = My.Application.Info.AssemblyName

        ' Create the object
        oSirAbout = New iPMAbout.Interface_Renamed()

        ' Initialise it. No parameters
        m_lReturn = CType(oSirAbout, SSP.S4I.Interfaces.ILocalInterface).Initialise()

        ' Display the about screen modally
        m_lReturn = oSirAbout.Show(sTitle:=sTitle, sVersionNumber:=sVersionNumber, sVersionDate:=sVersionDate, sComponent:=sComponent)

        ' Terminate it, and...
        oSirAbout.Dispose()

        ' ...remove it from memory
        oSirAbout = Nothing

        Exit Sub

Err_mnuHelpAbout_Click:

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuHelpAbout_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Exit Sub

    End Sub

    Public Sub mnuArrangeicons_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuArrangeicons.Click

        ' arrange the icons of the minimised forms
        Me.LayoutMdi(MdiLayout.ArrangeIcons)

    End Sub

    Public Sub mnuBold_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuBold.Click

        Try

            ' if the form is a Text form (not RTF)
            If Convert.ToString(ActiveMdiChild.Tag).Substring(0, 3) = "TXT" Then

                'If ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.bold Then
                If Me.mnuBold.Checked Then
                    ' disable bold
                    Me.mnuBold.Checked = False

                    'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.bold = False
                    ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = VB6.FontChangeBold(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, False)
                Else
                    ' enable bold
                    Me.mnuBold.Checked = True

                    'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.bold = True
                    ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = VB6.FontChangeBold(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, True)
                End If
            End If

            ' update the toolbar
            If mnuBold.Checked Then
                CType(Toolbar.Items.Item("Bold"), ToolStripButton).Checked = True

                'ReflectionHelper.GetMember(ActiveMdiChild, "BoldStatus") = True
            Else
                CType(Toolbar.Items.Item("Bold"), ToolStripButton).Checked = False

                'ActiveMdiChild.BoldStatus = False
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set bold.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuBold_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuCascade_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuCascade.Click

        Me.LayoutMdi(MdiLayout.Cascade)

    End Sub

    Public Sub mnuCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuCopy.Click

        ' Clear the clipboard
        My.Computer.Clipboard.Clear()
        Try  'Error running OLE Command
            If Me.ActiveMdiChild.Name = "frmBrowser" Then
                ' Copy the text to the clipboard



                'ActiveMdiChild.brwWebBrowser.ExecWB(OLECMDID_COPY, OLECMDEXECOPT_DONTPROMPTUSER)
            Else
                ' Copy the text to the clipboard
                If (ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectedText <> "") Then
                    My.Computer.Clipboard.SetText(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectedText, TextDataFormat.Text)
                End If
            End If

        Catch excep As System.Exception

            Debug.WriteLine(VB6.TabLayout(excep.Message, Information.Err().Number))

            ' Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
        End Try
    End Sub

    Public Sub mnuExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExit.Click

        ' exit !
        Me.Close()
    End Sub

    Public Sub mnuFileCloseAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileCloseAll.Click

        Dim iformcount As Integer

        ' Set the Thumbnails registry setting

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Const sDOCThumbnails As String = "Thumbnails"
        Dim sTmp As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            iformcount = Application.OpenForms.Count - 1

            ' Loop through each form currently available
            For Each frm As Form In Me.MdiChildren
                ' Handle if child form is already closed
                Try
                    System.Windows.Forms.Application.DoEvents()

                    frm.Close()
                Catch ex As Exception

                End Try
            Next

            'For iLoop1 As Integer = 1 To iformcount
            '    ' unload it...
            '    System.Windows.Forms.Application.DoEvents()
            '    If (Not System.Windows.Forms.Application.OpenForms.Item(iLoop1).ParentForm Is Nothing) AndAlso (System.Windows.Forms.Application.OpenForms.Item(iLoop1).ParentForm.Name = "frmParentMDI") Then
            '        System.Windows.Forms.Application.OpenForms.Item(iLoop1).Close()
            '    End If

            'Next iLoop1

            ' Save the registry setting here for the thumbnails
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
            eProductFamily = gPMConstants.PMEProductFamily.pmePFDocumaster

            sTmp = IIf(m_bThumbnails, "Y", "N")

            lReturn = CType(SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sDOCThumbnails, v_sSettingValue:=sTmp, v_sSubKey:=DOCOptionsSection), gPMConstants.PMEReturnCode)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to close all forms.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileCloseAll_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFirstPg_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFirstPg.Click

        Try



            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

                'ActiveMdiChild.DisplayFirstPage()
                ReflectionHelper.Invoke(ActiveMdiChild, "DisplayFirstPage", New Object() {})



                'Me.StbInfo.Items.Item(0).Text = "Page " & _
                '                               ActiveMdiChild.PageDisplayed & " of " & _
                'ActiveMdiChild.PageTotal()
                Me._StbInfo_Panel1.Text = "Page " & _
                                              ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") & " of " & ReflectionHelper.GetMember(ActiveMdiChild, "PageTotal")
                StbInfo.Refresh()
                bDontUpdate = True

                'cboPages.SelectedIndex = CInt(ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") - 1)
                cboPages.SelectedIndex = CInt(ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") - 1)
                bDontUpdate = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move to first page in document.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFirstPg_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFitHeight_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFitHeight.Click

        Try

            If mnuFitHeight.Checked Then
                SetFitStatus(FIT_NONE)
            Else
                SetFitStatus(FIT_HEIGHT)
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fit to height.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFitHeight_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFitScreen_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFitScreen.Click

        Try

            If mnuFitScreen.Checked Then
                SetFitStatus(FIT_NONE)
            Else
                SetFitStatus(FIT_SCREEN)
            End If

        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fit to screen.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFitScreen_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuFitWidth_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFitWidth.Click

        Try

            If mnuFitWidth.Checked Then
                SetFitStatus(FIT_NONE)
            Else
                SetFitStatus(FIT_WIDTH)
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fit to width.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnumnuFitWidth_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Public Sub mnuFont_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFont.Click

        Try

            ' initialise the dialog box with the current font settings

            With DlgFontFont

                .Font = VB6.FontChangeName(.Font, ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Name)

                .Font = VB6.FontChangeSize(.Font, ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Size)

                .Font = VB6.FontChangeItalic(.Font, ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Italic)

                .Font = VB6.FontChangeBold(.Font, ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.bold)
            End With

            ' show the font dialog

            DlgFontFont.ScriptsOnly = True

            DlgFontFont.FontMustExist = True

            'TODO
            'DlgFont.Flags = MSComDlg.FontsConstants.cdlCFScreenFonts

            'TODO
            'DlgFont.CancelError = True
            DlgFontFont.ShowDialog()

            ' set the font

            With ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font()
                VB6.FontChangeName(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, _
                                   DlgFontFont.Font.Name)

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeSize(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, DlgFontFont.Font.Size)

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeItalic(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, DlgFontFont.Font.Italic)

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeUnderline(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, DlgFontFont.Font.Bold)

                '.Name = DlgFontFont.Font.Name

                '.Size = DlgFontFont.Font.Size

                '.Italic = DlgFontFont.Font.Italic

                '.bold = DlgFontFont.Font.Bold

                If DlgFontFont.Font.Bold Then
                    CType(Me.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = True
                    Me.mnuBold.Checked = True
                Else
                    CType(Me.Toolbar.Items.Item("Bold"), ToolStripButton).Checked = False
                    Me.mnuBold.Checked = False
                End If

            End With

        Catch excep As System.Exception




            If Information.Err().Number = DialogResult.Cancel Then

                ' user pressed Cancel. We'll let them do this, and not
                ' change the font
                Exit Sub

            Else

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on set font.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFont_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End If

            Exit Sub

        End Try

    End Sub

    Public Sub mnuIconise_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuIconise.Click
        Try
            ' Loop through each form currently available
            For Each frm As Form In Me.MdiChildren
                frm.WindowState = FormWindowState.Minimized
            Next
        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to iconise all forms.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuIconise_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub
        End Try
    End Sub

    Public Sub mnuInfo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuInfo.Click

        Dim lDocNum As Integer
        Dim sKey, sNewName As String

        Try

            ' Display document information for the active form

            ' Get the key from the form's tag
            sKey = Convert.ToString(ActiveMdiChild.Tag).Substring(Convert.ToString(ActiveMdiChild.Tag).Length - (Strings.Len(Convert.ToString(ActiveMdiChild.Tag)) - 3))

            ' Get the document number from the key
            m_lReturn = ExtractNumFromKey(sKey:=sKey, lNum:=lDocNum)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' failed to get number... somehow
                Exit Sub
            End If

            ' Load up document information
            m_lReturn = g_oDOCInformation.Start(lDocNum:=lDocNum, sNewName:=sNewName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on view document information.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuInfo_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub
            End If

            ' Has the user changed the document name?
            If sNewName <> "" Then
                ' Update caption on MDI Child

                ActiveMdiChild.Text = "'" & sNewName & "'" & ReflectionHelper.GetMember(ActiveMdiChild, "ParentsPath")

                ' Update display in Manager

                m_lReturn = g_frmManager.UpdateDocumentName(sKey:=sKey, sNewName:=sNewName)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on view document information.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuInfo_CLick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Public Sub mnuLastpg_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuLastpg.Click

        Try


            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

                ' Move to the last page

                ' ActiveMdiChild.DisplayLastPage()
                ReflectionHelper.Invoke(ActiveMdiChild, "DisplayLastPage", New Object() {})

                ' Update the status bar


                Me.StbInfo.Items.Item(0).Text = "Page " & _
                                                ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") & " of " & _
                                                ReflectionHelper.GetMember(ActiveMdiChild, "PageTotal")

                ' Update the combo box
                bDontUpdate = True

                cboPages.SelectedIndex = CInt(ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") - 1)
                bDontUpdate = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move to last page in document.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuLastpg_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Public Sub mnuMove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuMove.Click

        Try
            If mnuMove.Checked Then
                SetMouseStatus(MOUSE_NONE)
            Else
                SetMouseStatus(MOUSE_MOVE)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process Move command", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuMove_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuNextpg_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNextpg.Click

        Try


            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

                ' Move to the next page

                'ActiveMdiChild.DisplayNextPage()
                ReflectionHelper.Invoke(ActiveMdiChild, "DisplayNextPage", New Object() {})

                ' Update the status bar


                Me.StbInfo.Items.Item(0).Text = "Page " & _
                                                 ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") & " of " & _
                                                 ReflectionHelper.GetMember(ActiveMdiChild, "PageTotal")

                ' Update the combo box
                bDontUpdate = True

                cboPages.SelectedIndex = CInt(ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") - 1)
                bDontUpdate = False
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move to the next page in the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuNextpg_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuNormalDisplay_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNormalDisplay.Click

        Try


            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then
                SetFitStatus(FIT_NONE)
                SetMouseStatus(MOUSE_NONE)
            Else

                'TODO
                'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Name = "Arial"
                'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Size = 10
                '' switch off any thing that the user may have selected from `Font...`
                'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Italic = False
                'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Underline = False

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeName(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, _
                                   ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Name)

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeSize(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, 10)

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeItalic(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, False)

                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = _
                VB6.FontChangeUnderline(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, False)


                If Me.mnuBold.Checked Then
                    mnuBold_Click(mnuBold, New EventArgs())
                End If

                Me.StbInfo.Items.Item(1).Text = ""

                ' unable the zoom
                mnuZoom.Checked = False
                CType(Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = False

                ' unable the move
                mnuMove.Checked = False
                CType(Toolbar.Items.Item("Move"), ToolStripButton).Checked = False

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set display back to normal view.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuNormalDisplay_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try



    End Sub

    Public Sub mnuPrevpg_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPrevpg.Click

        Try


            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

                ' Move to the previous page

                'ActiveMdiChild.DisplayPreviousPage()
                ReflectionHelper.Invoke(ActiveMdiChild, "DisplayPreviousPage", New Object() {})

                ' Update the status bar


                Me.StbInfo.Items.Item(0).Text = "Page " & _
                                                 ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") & " of " & _
                                                 ReflectionHelper.GetMember(ActiveMdiChild, "PageTotal")

                ' Update the combo box on the toolbar
                bDontUpdate = True

                cboPages.SelectedIndex = CInt(ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") - 1)
                bDontUpdate = False

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move to the previous page in the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPrevpg_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuPrint.Click
        Dim original_print_device As String = ""

        Try

            ' TODO - RTF Printing


            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then


                'm_lReturn = ActiveMdiChild.PrintSetup()
                m_lReturn = ReflectionHelper.Invoke(ActiveMdiChild, "PrintSetup", New Object() {})
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

            Else

                ' disable the range, file and selection option

                'DlgPrint.Flags = MSComDlg.PrinterConstants.cdlPDHidePrintToFile

                DlgPrintPrint.AllowSelection = False

                'DlgPrint.Flags = MSComDlg.PrinterConstants.cdlPDNoPageNums

                ' Detect if the users presses cancel

                'DlgPrint.CancelError = True

                original_print_device = PrinterHelper.Printer.DeviceName

                ' Show the print dialog
                'DlgPrintPrint.ShowDialog()
            End If

            m_lReturn = DoPrint()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Set the system default printer back to original one
            If PrinterHelper.Printer.DeviceName <> original_print_device Then
                m_lReturn = iPMFunc.Set_System_Default_Printer(original_print_device)
            End If


        Catch excep As System.Exception
            Select Case Information.Err().Number
                ' This is generated when the user clicks on the Cancel button
                Case DialogResult.Cancel
                    ' Exit ok, no errors
                    Exit Sub

                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process mnuPrint_Click.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPrint_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select

        End Try

    End Sub

    Private Function DoPrint() As Integer
        Dim Err_OLE As Boolean = False
        Dim Err_DoPrint As Boolean = False

        Dim result As Integer = 0
        Dim lDocNum As Integer

        ' The printer is not configured from here, so it can be
        ' called straight from the toolbar

        Dim sFileName As String = ""

        ' Splash message
        Dim sMessage As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            Err_DoPrint = True
            Err_OLE = False

            ' Tell the user that its printing, and on what printer
            sMessage = "Printing document on '" & DlgPrintPrint.PrinterSettings.PrinterName & "'. Please wait..."

            ' Show the splaaaaaaaaaaaash
            m_lReturn = m_oSplash.Show(iSplashType:=DOCSplash_Message, sMessage:=sMessage)
            m_lReturn = m_oSplash.Hide()
            'MsgBox(sMessage, "DocuMaster Enterprise")
            MsgBox(sMessage, MsgBoxStyle.OkOnly, "DocuMaster Enterprise")

            '*******************
            ' This development been done to make the excel print
            ' PN 45500
            '*******************

            m_lReturn = PrinterFunc.GetFileExtension4Excl(sGetFileCacheName)

            If sGetFileType = "XLS" Then
                m_lReturn = eXcelPrint(sGetFileCacheName)
            Else
                'm_lReturn = PrintDocumentSilent(sNewFilename)


                If Me.ActiveMdiChild.Name = "frmBrowser" Then 'Uses OLE Printing so OLE Client Handles the printing
                    Err_OLE = True
                    Err_DoPrint = False 'Error running OLE Command



                    'Me.ActiveMdiChild.brwWebBrowser.ExecWB(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER)
                    'TODO
                    'ReflectionHelper.Invoke(ReflectionHelper.GetMember(ActiveMdiChild, "brwWebBrowser"), "ExecWB", New Object(OLECMDID_PRINT, OLECMDEXECOPT_DONTPROMPTUSER) {})
                    Err_DoPrint = True
                    Err_OLE = False

                    ' CTAF 20031104 - Ability to print office documents
                ElseIf (Convert.ToString(ActiveMdiChild.Tag).Substring(0, 3) = "IMG") Then

                    'CType(ActiveMdiChild.Controls(0), AxDSOFramer.AxFramerControl).PrintOut(True)

                ElseIf (Convert.ToString(ActiveMdiChild.Tag).Substring(0, 3) <> "TIF") Then

                    ' Set the printer properties now

                    ' TODO - RTF Printing. Need to set properties

                    ' Print the contents of the RichTextBox with a one inch margin

                    PrintRTF(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView"), 1440, 1440, 1440, 1440) ' 1440 Twips = 1 Inch

                Else


                    m_lReturn = ReflectionHelper.Invoke(ActiveMdiChild, "PrintPages", New Object() {})

                End If

            End If

            'Create log of this print

            ExtractNumFromKey(ReflectionHelper.GetMember(ActiveMdiChild, "DocumentKey"), lDocNum)
            m_lReturn = LogPrint(lDocNum)

            ' Hide the message
            'm_lReturn = m_oSplash.Hide()

            Return result

        Catch excep As System.Exception
            If Not Err_OLE And Not Err_DoPrint Then
                Throw excep
            End If
            If Err_OLE Then

                Debug.WriteLine(VB6.TabLayout(Information.Err().Number, excep.Message))

                ' Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
            End If
            If Err_DoPrint Or Err_OLE Then


                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to print.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuPrint_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                ' Hide the message
                m_lReturn = m_oSplash.Hide()

                Return result
            End If
        End Try
    End Function

    '' *******************************************************
    ''
    '' Procedure   : mnuReturnManager_Click
    ''
    '' Description : Switches control back to the manager at
    ''               the point of the current document.
    ''
    '' *******************************************************
    'Private Sub mnuReturnManager_Click()
    '
    '    Dim sKey As String
    '
    '    On Error GoTo Err_ReturnManager
    '
    '    If (Forms.Count > 1) Then
    '        ' Get the key of the current active mdi form
    '        sKey = Right$(ActiveForm.Tag, Len(ActiveForm.Tag) - 3)
    '    Else
    '        'no docs loaded
    '        sKey$ = ""
    '    End If
    '
    '    ' call LocateDocument in the manager to go to the position
    '    m_lReturn = g_frmManager.LocateDocument(sKey:=sKey)
    '    If (m_lReturn <> PMTrue) Then
    '        Exit Sub
    '    End If
    '
    '    Exit Sub
    '
    'Err_ReturnManager:
    '
    '    ' log an error
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to process return to Manager", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="mnuReturnManager_Click", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    Public Sub mnuRotateLeft_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRotateLeft.Click

        Try

            'TODO
            'ActiveMdiChild.RotatePage(v_lDegrees:=90)
            ReflectionHelper.Invoke(ActiveMdiChild, "RotatePage", New Object() {90})


            SetFitStatus(ReflectionHelper.GetMember(ActiveMdiChild, "FitStatus"))

        Catch excep As System.Exception



            ' log an error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to rotate image anti-clockwise.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRotateLeft_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuRotateRight_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuRotateRight.Click

        Try

            'TODO
            'ActiveMdiChild.RotatePage(v_lDegrees:=-90)
            ReflectionHelper.Invoke(ActiveMdiChild, "RotatePage", New Object() {-90})

            'SetFitStatus(ActiveMdiChild.FitStatus)
            SetFitStatus(ReflectionHelper.GetMember(ActiveMdiChild, "FitStatus"))

        Catch excep As System.Exception



            ' log an error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to rotate image clockwise.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuRotateRight_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuShowThumbnails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuShowThumbnails.Click

        If mnuShowThumbnails.Checked Then
            mnuShowThumbnails.Checked = False
            m_bThumbnails = False
            CType(Toolbar.Items.Item("Thumbnails"), ToolStripButton).Checked = False
        Else
            mnuShowThumbnails.Checked = True
            m_bThumbnails = True
            CType(Toolbar.Items.Item("Thumbnails"), ToolStripButton).Checked = True
        End If


        Dim iformcount As Integer = Application.OpenForms.Count - 1

        ' Loop through each form currently available
        For iLoop1 As Integer = 1 To iformcount

            'If Application.OpenForms.Item(iLoop1).ViewerType = ACViewerTypeTIF Then
            If Application.OpenForms.Item(iLoop1).Name = "frmChildTIF" Then
                '    
                '    Application.OpenForms.Item(iLoop1).DisplayThumbs(m_bThumbnails)
                DirectCast(Application.OpenForms.Item(iLoop1), frmChildTIF).DisplayThumbs(m_bThumbnails)
            End If
        Next iLoop1

    End Sub

    Public Sub mnuTileHoriz_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTileHoriz.Click

        ' tile windows horizontally
        Me.LayoutMdi(MdiLayout.TileHorizontal)

    End Sub

    Public Sub mnuTilevert_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuTilevert.Click

        ' tile windows vertically
        Me.LayoutMdi(MdiLayout.TileVertical)

    End Sub

    Public Sub mnuZoom_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuZoom.Click

        Try


            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

                If mnuZoom.Checked Then
                    SetMouseStatus(MOUSE_NONE)
                Else
                    SetMouseStatus(MOUSE_ZOOM)
                End If

            Else
                'ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").Font.Size += 2
                Dim rtb As RichTextBox = ReflectionHelper.GetMember(ActiveMdiChild, "rtbView")
                Dim font As Font
                font = New Font(rtb.SelectionFont.FontFamily, rtb.SelectionFont.Size, rtb.SelectionFont.Style)
                ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont = VB6.FontChangeSize(ReflectionHelper.GetMember(ActiveMdiChild, "rtbView").SelectionFont, font.Size + 2)
                'ReflectionHelper.GetMember(ActiveMdiChild, "MouseStatus") = MOUSE_ZOOM

            End If

        Catch excep As System.Exception



            ' log an error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed in zoom mode.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuZoom_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub Tmrclock_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles Tmrclock.Tick

        ' show the HH:MM, but not the SS of the time
        'StbInfo.Items.Item(3).Text = CDate(DateTime.Now.ToString("HH:mm:ss")).ToString("HH:MM")
        _StbInfo_Panel4.Text = (CDate(DateTime.Now.ToString("HH:mm:ss")).ToString("HH:MM")).ToString
        StbInfo.Refresh()
        'StbInfo.Panels(4).ToolTipText = Format(Date$, "Long Date")
    End Sub


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub Toolbar_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar_Button1.Click, _Toolbar_Button2.Click, _Toolbar_Button3.Click, _Toolbar_Button4.Click, _Toolbar_Button5.Click, _Toolbar_Button6.Click, _Toolbar_Button7.Click, _Toolbar_Button8.Click, _Toolbar_Button9.Click, _Toolbar_Button10.Click, _Toolbar_Button11.Click, _Toolbar_Button12.Click, _Toolbar_Button13.Click, _Toolbar_Button14.Click, _Toolbar_Button15.Click, _Toolbar_Button16.Click, _Toolbar_Button17.Click, _Toolbar_Button18.Click, _Toolbar_Button19.Click, _Toolbar_Button20.Click, _Toolbar_Button21.Click, _Toolbar_Button22.Click, _Toolbar_Button23.Click, _Toolbar_Button24.Click, _Toolbar_Button25.Click, _Toolbar_Button26.Click, _Toolbar_Button27.Click, _Toolbar_Button28.Click, _Toolbar_Button29.Click, _Toolbar_Button30.Click, _Toolbar_Button31.Click, _Toolbar_Button32.Click, _Toolbar_Button33.Click, _Toolbar_Button34.Click
        Dim Err_OLE As Boolean = False
        Dim Err_ToolbarButtonClick As Boolean = False
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Try
            Err_ToolbarButtonClick = True
            Err_OLE = False

            'Select Case Button.Name
            Select Case Button.Tag
                ' Print document
                Case Is = "Print"
                    ' Print with default settings
                    'TODO
                    'If ActiveMdiChild.ViewerType = ACViewerTypeTIF Then
                    '    
                    '    ActiveMdiChild.IkPrint1.PrintFileName = "Default"
                    '    
                    '    ActiveMdiChild.IkPrint1.PrintCreateDC(IMGKIT6Lib.PrintModeConstants.ikPrintFileName)
                    '    
                    '    ActiveMdiChild.IkPrint1.FromPage = 1
                    '    
                    '    
                    '    ActiveMdiChild.IkPrint1.ToPage = ActiveMdiChild.PageTotal
                    'End If
                    m_lReturn = DoPrint()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Toolbar Button click", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Exit Sub
                    End If

                    ' Bold
                Case Is = "Bold"
                    mnuBold_Click(mnuBold, New EventArgs())

                    ' Move
                Case Is = "Move"
                    mnuMove_Click(mnuMove, New EventArgs())

                    ' First Page
                Case Is = "FirstPage"
                    mnuFirstPg_Click(mnuFirstPg, New EventArgs())

                    ' Previous Page
                Case Is = "PreviousPage"
                    mnuPrevpg_Click(mnuPrevpg, New EventArgs())

                    ' Next Page
                Case Is = "NextPage"
                    mnuNextpg_Click(mnuNextpg, New EventArgs())

                    ' Last Page
                Case Is = "LastPage"
                    mnuLastpg_Click(mnuLastpg, New EventArgs())

                    ' Rotate Left
                Case Is = "RotateLeft"
                    mnuRotateLeft_Click(mnuRotateLeft, New EventArgs())

                    ' Rotate Right
                Case Is = "RotateRight"
                    mnuRotateRight_Click(mnuRotateRight, New EventArgs())

                    ' Zoom
                Case Is = "Zoom"
                    mnuZoom_Click(mnuZoom, New EventArgs())

                    ' Normal Display
                Case Is = "Normal"
                    mnuNormalDisplay_Click(mnuNormalDisplay, New EventArgs())

                    ' Fit image to height
                Case Is = "FitHeight"
                    mnuFitHeight_Click(mnuFitHeight, New EventArgs())

                    ' Fit image to width
                Case Is = "FitWidth"
                    mnuFitWidth_Click(mnuFitWidth, New EventArgs())

                    ' Fit image to screen
                Case Is = "FitScreen"
                    mnuFitScreen_Click(mnuFitScreen, New EventArgs())

                    ' Copy
                Case Is = "Copy"
                    mnuCopy_Click(mnuCopy, New EventArgs())

                    ' Document Information
                Case Is = "Information"
                    mnuInfo_Click(mnuInfo, New EventArgs())

                    'Show the ole toolbar
                Case Is = "ToolBar"
                    Err_OLE = True
                    Err_ToolbarButtonClick = False 'Error running OLE Command
                    If Me.ActiveMdiChild.Name = "frmBrowser" Then



                        'TODO
                        'Me.ActiveMdiChild.brwWebBrowser.ExecWB(OLECMDID_HIDETOOLBARS, OLECMDEXECOPT_DONTPROMPTUSER)
                    End If

                    'Archive document in SBO MS 15/05/01
                Case Is = "Archive"
                    mnuArchive_Click(mnuArchive, New EventArgs())

                Case Is = "Thumbnails"
                    mnuShowThumbnails_Click(mnuShowThumbnails, New EventArgs())

                    ' Something else went wrong
                Case Else
                    MessageBox.Show("Unknown Button key", Application.ProductName)
                    Exit Sub

            End Select

        Catch excep As System.Exception
            If Not Err_OLE And Not Err_ToolbarButtonClick Then
                Throw excep
            End If
            If Err_OLE Then

                Debug.WriteLine(VB6.TabLayout(Information.Err().Number, excep.Message))

                Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Next Statement")
                Exit Sub
            End If
            If Err_ToolbarButtonClick Or Err_OLE Then


                ' Error Section.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Toolbar Button click", vApp:=ACApp, vClass:=ACClass, vMethod:="Toolbar_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End If
        End Try

    End Sub

    ' PRIVATE Events (End)

    Public Function EnableCommon() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            mnuFileCloseAll.Enabled = True

            ' copy
            'Toolbar.Items.Item("Copy").Enabled = True
            Toolbar.Items.Item("Copy").Enabled = True
            mnuCopy.Enabled = True

            ' print
            'Toolbar.Items.Item("Print").Enabled = True
            Toolbar.Items.Item("Print").Enabled = True
            mnuPrint.Enabled = True

            ' info
            'Toolbar.Items.Item("Information").Enabled = True
            Toolbar.Items.Item("Information").Enabled = True
            mnuInfo.Enabled = True

            ' MS 15/05/01 Archive doucment
            'Toolbar.Items.Item("Archive").Enabled = True
            Toolbar.Items.Item("Archive").Enabled = True
            mnuArchive.Enabled = True

            mnuView.Enabled = True
            mnuImage.Enabled = True
            mnuWindow.Enabled = True

            For iLoop1 As Integer = 1 To Toolbar.Items.Count
                Toolbar.Items.Item(iLoop1 - 1).Enabled = True
            Next iLoop1

            ' and the combo box
            cboPages.Enabled = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to enable common toolbar and menu options.", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableCommon", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function LogPrint(ByVal lDocNum As Integer) As Integer
        Dim result As Integer = 0
        Dim bDOCManager As Object

        Dim oObjectManager As bObjectManager.ObjectManager

        Dim oBusiness As bDOCManager.Form
        Dim lEventCnt As Integer

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "LogPrint"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get object manager
            oObjectManager = New bObjectManager.ObjectManager()
            lReturn = CType(oObjectManager.Initialise(sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("oObjectManager.Initialise", "Unable to initialise the object manager")
            End If

            '   Find the Business Class
            Dim temp_oBusiness As Object
            lReturn = CType(oObjectManager.GetInstance(temp_oBusiness, "bDOCManager.Form", vInstanceManager:=PMGetViaClientManager), gPMConstants.PMEReturnCode)
            oBusiness = temp_oBusiness
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("oObjectManager.GetInstance", "Unable to get instance of bDOCManager.Form")
            End If

            ' Create the event!!)

            lReturn = oBusiness.CopyEventInSBO(lEventCnt:=lEventCnt, lDocNum:=lDocNum, dtEventDate:=DateTime.Now, sDescriptionPrefix:="Printed:")

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' Clean up
            If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
                oBusiness = Nothing
            End If

            If Not (oObjectManager Is Nothing) Then
                oObjectManager.Dispose()
                oObjectManager = Nothing
            End If

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function SetMouseStatus(ByVal v_iMouseStatus As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetMouseStatus"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TODO
            'ActiveMdiChild.MouseStatus = v_iMouseStatus
            ReflectionHelper.SetMember(ActiveMdiChild, "MouseStatus", v_iMouseStatus)

            If v_iMouseStatus = MOUSE_MOVE Then

                'TODO
                'ActiveMdiChild.IkDisp1.RectDraw = False
                'ActiveMdiChild.IkDisp1.ScrollDrag = True
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "RectDraw", False)
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "ScrollBar", True)

                mnuZoom.Checked = False
                CType(Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = False
                mnuMove.Checked = True
                CType(Toolbar.Items.Item("Move"), ToolStripButton).Checked = True

                StbInfo.Items.Item(1).Text = "Move"

            ElseIf v_iMouseStatus = MOUSE_ZOOM Then

                'ActiveMdiChild.IkCommon1.ImgHandle = ActiveMdiChild.IkDisp1.ImgHandle
                'ActiveMdiChild.IkCommon1.GetImageType()

                'ActiveMdiChild.IkDisp1.RectDrawRatio = ActiveMdiChild.IkCommon1.ImgHeight / ActiveMdiChild.IkCommon1.ImgWidth
                'ActiveMdiChild.IkDisp1.RectDraw = True
                'ActiveMdiChild.IkDisp1.ScrollDrag = False
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "RectDrawRatio", ReflectionHelper.GetMember(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "Image"), "Height") / ReflectionHelper.GetMember(ReflectionHelper.GetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "Image"), "Width"))
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "RectDraw", True)
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "ScrollBar", True)

                mnuZoom.Checked = True
                CType(Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = True
                mnuMove.Checked = False
                CType(Toolbar.Items.Item("Move"), ToolStripButton).Checked = False

                StbInfo.Items.Item(1).Text = "Zoom"

            Else
                'MOUSE_NONE

                'TODO
                'ActiveMdiChild.IkDisp1.RectDraw = False
                'ActiveMdiChild.IkDisp1.ScrollDrag = False
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "RectDraw", False)
                ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "ScrollBar", False)


                mnuZoom.Checked = False
                CType(Toolbar.Items.Item("Zoom"), ToolStripButton).Checked = False
                mnuMove.Checked = False
                CType(Toolbar.Items.Item("Move"), ToolStripButton).Checked = False

                StbInfo.Items.Item(1).Text = "Move"

            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function SetFitStatus(ByVal v_iFitStatus As Integer, Optional ByVal v_bActivate As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFitStatus"

        Dim lTwipsPerPixel As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'ActiveMdiChild.FitStatus = v_iFitStatus

            If v_iFitStatus = FIT_HEIGHT Then

                If Not v_bActivate Then


                    'ActiveMdiChild.IkCommon1.ImgHandle = ActiveMdiChild.IkDisp1.ImgHandle
                    'ActiveMdiChild.IkCommon1.GetImageType()

                    lTwipsPerPixel = CInt(VB6.TwipsPerPixelY())

                    'TODO
                    'ActiveMdiChild.IkDisp1.DispScale = (ActiveMdiChild.IkDisp1.Height / lTwipsPerPixel) / ActiveMdiChild.IkCommon1.ImgHeight
                    'ActiveMdiChild.IkDisp1.Display(IMGKIT6Lib.DispModeConstants.ikActualSize)
                    ReflectionHelper.Invoke(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "Display", New Object() {Newtone.ImageKit.Win.DisplayMode.FitToHeight})

                End If

                mnuFitHeight.Checked = True
                CType(Toolbar.Items.Item("FitHeight"), ToolStripButton).Checked = True
                mnuFitWidth.Checked = False
                CType(Toolbar.Items.Item("FitWidth"), ToolStripButton).Checked = False
                mnuFitScreen.Checked = False
                CType(Toolbar.Items.Item("FitScreen"), ToolStripButton).Checked = False

                'StbInfo.Items.Item(2).Text = "Fit to Height"
                _StbInfo_Panel2.Text = "Fit to Height"


            ElseIf v_iFitStatus = FIT_WIDTH Then

                If Not v_bActivate Then


                    'ActiveMdiChild.IkCommon1.ImgHandle = ActiveMdiChild.IkDisp1.ImgHandle
                    'ActiveMdiChild.IkCommon1.GetImageType()

                    lTwipsPerPixel = CInt(VB6.TwipsPerPixelX())

                    'TODO
                    'ActiveMdiChild.IkDisp1.DispScale = (ActiveMdiChild.IkDisp1.Width / lTwipsPerPixel) / ActiveMdiChild.IkCommon1.ImgWidth
                    'ActiveMdiChild.IkDisp1.Display(IMGKIT6Lib.DispModeConstants.ikActualSize)
                    ReflectionHelper.Invoke(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "Display", New Object() {Newtone.ImageKit.Win.DisplayMode.FitToWidth})
                End If

                mnuFitHeight.Checked = False
                CType(Toolbar.Items.Item("FitHeight"), ToolStripButton).Checked = False
                mnuFitWidth.Checked = True
                CType(Toolbar.Items.Item("FitWidth"), ToolStripButton).Checked = True
                mnuFitScreen.Checked = False
                CType(Toolbar.Items.Item("FitScreen"), ToolStripButton).Checked = False
                _StbInfo_Panel2.Text = "Fit to Width"

            ElseIf v_iFitStatus = FIT_SCREEN Then

                If Not v_bActivate Then

                    'TODO
                    'ActiveMdiChild.IkDisp1.Display(IMGKIT6Lib.DispModeConstants.ikStretch)
                    ReflectionHelper.Invoke(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "Display", New Object() {Newtone.ImageKit.Win.DisplayMode.Stretch})
                End If

                mnuFitHeight.Checked = False
                CType(Toolbar.Items.Item("FitHeight"), ToolStripButton).Checked = False
                mnuFitWidth.Checked = False
                CType(Toolbar.Items.Item("FitWidth"), ToolStripButton).Checked = False
                mnuFitScreen.Checked = True
                CType(Toolbar.Items.Item("FitScreen"), ToolStripButton).Checked = True

                _StbInfo_Panel2.Text = "Fit to Screen"

            Else
                'FIT_NONE

                If Not v_bActivate Then

                    'TODO
                    'ActiveMdiChild.IkDisp1.DispScale = 1
                    'ActiveMdiChild.IkDisp1.Display(IMGKIT6Lib.DispModeConstants.ikActualSize)
                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "DrawScaleX", 1)
                    ReflectionHelper.SetMember(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "DrawScaleY", 1)
                    ReflectionHelper.Invoke(ReflectionHelper.GetMember(ActiveMdiChild, "ImageKit1"), "Display", New Object() {Newtone.ImageKit.Win.DisplayMode.ActualSize})
                End If

                mnuFitHeight.Checked = False
                CType(Toolbar.Items.Item("FitHeight"), ToolStripButton).Checked = False
                mnuFitWidth.Checked = False
                CType(Toolbar.Items.Item("FitWidth"), ToolStripButton).Checked = False
                mnuFitScreen.Checked = False
                CType(Toolbar.Items.Item("FitScreen"), ToolStripButton).Checked = False

                StbInfo.Items.Item(2).Text = ""

            End If


            If ReflectionHelper.GetMember(ActiveMdiChild, "FitStatus") = FIT_SCREEN Then
                SetMouseStatus(MOUSE_NONE)
                Toolbar.Items.Item("Zoom").Enabled = False
                Toolbar.Items.Item("Move").Enabled = False
            Else
                Toolbar.Items.Item("Zoom").Enabled = True
                Toolbar.Items.Item("Move").Enabled = True
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Sub Pages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPages.Click
        Try

            Dim iPageNum As Integer


            'If ActiveMdiChild.ViewerType = ACViewerTypeTIF Then
            If ReflectionHelper.GetMember(ActiveMdiChild, "ViewerType") = ACViewerTypeTIF Then

                If Not bDontUpdate Then
                    iPageNum = Conversion.Val(Me.cboPages.Text)

                    'm_lReturn = ActiveMdiChild.DisplayPage(iPageNum:=iPageNum)
                    m_lReturn = ReflectionHelper.Invoke(ActiveMdiChild, "DisplayFirstPage", New Object() {iPageNum})



                    Me._StbInfo_Panel1.Text = "Page " & _
                                                    ReflectionHelper.GetMember(ActiveMdiChild, "PageDisplayed") & " of " & _
                                                    ReflectionHelper.GetMember(ActiveMdiChild, "PageTotal")
                End If

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process combo box click", vApp:=ACApp, vClass:=ACClass, vMethod:="cboPages_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub


End Class
