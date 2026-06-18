Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Globalization
Imports System.IO
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Class Name: frmInterface
    '
    ' Date: 4/2/98
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    '
    ' SP070898 - Ensure access level is saved for the doc and do not allow
    ' blank document names
    '
    ' SP180898 - Frig so only works in single mode, yet have single prompt
    '            for more pages. This done till batch gets fixed.
    '
    ' JH021198 - added functions LoadSettings, LoadSetting and
    '            GrabChanges, which sort out the preference settings
    '
    ' ***************************************************************** '

    ' Who am i?
    Private Const ACClass As String = "frmInterface"

    Dim bMovedNextPage As Boolean
    Dim bInitialising As Boolean
    Dim m_lReturn As Integer

    'ND 201000 - SBO task
    Dim m_sCustomer As String = ""
    Dim m_dtTaskDueDate As Date
    Dim m_lPMUserGroup As Integer
    Dim m_lUserID As Integer
    Dim m_iUrgent As CheckState

    'ND 071100 - So scan station can be hidden to choose new folder to scan to
    Private m_bHiddenForFolderSelect As Boolean

    ' CTAF 20030814
    Private m_bMultiDocMode As Boolean
    Private m_bBlankScan As Boolean
    Private m_lBlankSize As Integer
    Private m_sBlankFileName As String = ""
    Private m_bCancelledScan As Boolean
    Private dsInfo() As Newtone.ImageKit.DataSourceInformation = Nothing


    Public Property HiddenForFolderSelect() As Boolean
        Get

            Return m_bHiddenForFolderSelect

        End Get
        Set(ByVal Value As Boolean)

            m_bHiddenForFolderSelect = Value

        End Set
    End Property

    ' *****************************************************************************
    '
    ' Function   : ScanPage
    '
    ' Description: Disables the controls, and starts the scanner to scan a page
    '
    ' *****************************************************************************
    Private Function ScanPage() As Integer

        Dim result As Integer = 0
        Dim Index(10) As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Disable all the command buttons on the interface

            For Each Control As Control In ContainerHelper.Controls(Me)
                If TypeOf Control Is Button Then
                    Control.Enabled = False
                End If
            Next Control

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Default cancelled status to true.
            m_bCancelledScan = True

            ' Activate the scanner
            m_lReturn = IkDisp1.Scan.Execute()

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to scan current page.", vApp:=ACApp, vClass:="frmInterface", vMethod:="ScanPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cboDisplay_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboDisplay.SelectedIndexChanged

        g_AdvOptions.Display.bChanged = True

        g_AdvOptions.Display.sValue = cboDisplay.Text

        If cboDisplay.Text <> PMDOCDISPLAYYESTIMED Then
            txtDelay.Enabled = False
            UpDown1.Enabled = False
            lblSeconds.Enabled = False
        Else
            txtDelay.Enabled = True
            UpDown1.Enabled = True
            lblSeconds.Enabled = True
        End If

        'SP030500 - Changed to save change to registry
        With g_AdvOptions.Display
            .bChanged = True
            m_lReturn = SetRegistryValue(sSubKey:=.sSubKey, sKey:=.sKey, sValue:=.sValue, iLocation:=.iLocation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                .bChanged = False
                Exit Sub
            End If
        End With

    End Sub

    Private Sub cboDocumentName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles cboDocumentName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If KeyAscii >= 32 Then
            If Strings.Len(cboDocumentName.Text) > 49 Then
                KeyAscii = 0
            End If
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub cboPMUserGroupByTask_ClickEvent(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMUserGroupByTask.Click
        'BE 5/9/2003 CQ2479 filter cboUserLookup
        cboPMUserLookup.PMUserGroupID = cboPMUserGroupByTask.UserGroupID
        cboPMUserLookup.RefreshList()
    End Sub


    Private isInitializingComponent As Boolean
    Private Sub cboScanner_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboScanner.TextChanged

        'IkScan1.ScanDsName = cboScanner.Text.Trim()
        IkDisp1.Scan.DataSourceName = cboScanner.Text.Trim()

    End Sub

    Private Sub chkEnableTask_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkEnableTask.CheckStateChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        'DN 10/05/01 - Enable or disable task scheduler
        If chkEnableTask.CheckState = CheckState.Unchecked Then
            FraRecipient.Enabled = False
            Label2.Enabled = False
            cboPMUserLookup.Enabled = False
            Label3.Enabled = False
            cboPMUserGroupByTask.Enabled = False
            Label1.Enabled = False
            cboCustomer.Enabled = False
            Label4.Enabled = False
            txtTaskDD.Enabled = False
            cmdTaskDDReset.Enabled = False
            chkUrgent.Enabled = False
        Else
            FraRecipient.Enabled = True
            Label2.Enabled = True
            cboPMUserLookup.Enabled = True
            Label3.Enabled = True
            cboPMUserGroupByTask.Enabled = True
            Label1.Enabled = True
            cboCustomer.Enabled = True
            Label4.Enabled = True
            txtTaskDD.Enabled = True
            cmdTaskDDReset.Enabled = True
            chkUrgent.Enabled = True
        End If

        With g_SiriusOptions.TaskEnabled
            .bChanged = True
            .sValue = CStr(chkEnableTask.CheckState)
            m_lReturn = SetRegistryValue(sSubKey:=.sSubKey, sKey:=.sKey, sValue:=.sValue, iLocation:=.iLocation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                .bChanged = False
                Exit Sub
            End If
        End With
    End Sub

    Private Sub cmdAnnotations_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAnnotations.Click

        Dim sCurrentAnnotation As String = ""

        Try

            ' Load the annotation form
            Dim frmAnnotation As frmAnnotation = New frmAnnotation

            frmAnnotation.txtAnnotation.Text = ""

            ' Show the annotation form, modally
            VB6.ShowForm(frmAnnotation, FormShowConstants.Modal, Me)

            ' Grab the annotation text
            sCurrentAnnotation = frmAnnotation.txtAnnotation.Text

            If sCurrentAnnotation.Trim() <> "" Then

                lNumberAnnotations += 1

                ReDim Preserve vAnnotations(lNumberAnnotations - 1)


                vAnnotations(lNumberAnnotations) = sCurrentAnnotation

            End If

            ' Unload the annotation form
            frmAnnotation.Close()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to add an annotation.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdAnnotations_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click

        ' Just pass to OK button
        cmdOK_Click()

    End Sub

    Private Sub cmdDocReset_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDocReset.Click

        ' Reset back to proper date
        txtDocumentDate.Text = DateTime.Now.AddDays(iDocDateOffset).ToString("dd MMMM yyyy")

    End Sub

    Private Sub cmdExpiryReset_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExpiryReset.Click

        ' Reset back to proper date
        txtExpiryDate.Text = DateTime.Now.AddDays(iDocExpiryOffset).ToString("dd MMMM yyyy")

    End Sub

    Private Sub cmdKeywords_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdKeywords.Click

        Dim oKeywords As iDOCKeywordAdmin.interface_Renamed

        'RAM20021223 : Changed the early binding to Late Binding

        Try

            ' Set the cursor to busy - hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create an instance of the keyword class
            'RAM20021223 : Changed the early binding to Late Binding
            oKeywords = New iDOCKeywordAdmin.Interface_Renamed()

            ' Initialise it
            m_lReturn = oKeywords.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the cursor back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Attach the keywords to the document
            m_lReturn = oKeywords.AttachKeywords(vKeywordID:=vKeywords, lDocNum:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Terminate the class
            oKeywords.Dispose()
            ' Remove the instance of the class
            oKeywords = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to attach keywords.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdKeywords_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdOK_Click()

        If Not g_bDocumentSaved Then
            SaveDocument()
        End If

        ' Unload the form, exiting
        Me.Close()

    End Sub

    Private Sub cmdPassword_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPassword.Click

        'Dim oPassword As iDOCPassword.interface

        'RAM20021223 : Changed the early binding to Late Binding
        Dim oPassword As iDOCPassword.Interface_Renamed
        Dim sPassword As String = ""

        Try

            ' set the mouse pointer to busy - hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Create an instance of the password class
            'Set oPassword = New iDOCPassword.interface

            'RAM20021223 : Changed the early binding to Late Binding
            oPassword = New iDOCPassword.Interface_Renamed()

            ' Initialise it
            m_lReturn = oPassword.Initialise(bStandAlone:=bIsStandAlone, sUsername:=g_sUsername, sPassword:=g_sPassword.Value, iUserID:=g_iUserID, iSourceID:=g_iSourceID, iLanguageID:=g_iLanguageID, iCurrencyID:=g_iCurrencyID, iLogLevel:=iCurrentLogLevel, sCallingAppName:="iDOCScan")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
                Exit Sub
            End If

            ' set the mouse pointer back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Call the AddPassword function to add a password
            m_lReturn = oPassword.AddPassword(sEncryptedPassword:=sPassword)
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                sCurrentPassword = ""
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to associate a password.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Else
                ' Now store it
                sCurrentPassword = sPassword
            End If

            ' Remove the instance of the password class
            oPassword = Nothing

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to associate a password.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdPassword_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub cmdReScan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReScan.Click


        Dim sMessage As String = "This action will remove all images scanned in the current" & _
                                 Environment.NewLine & _
                                 "document. Are you sure you wish to continue?" & _
                                 Environment.NewLine & Environment.NewLine & _
                                 "Note: Please make sure that the next document page is ready to scan."

        ' Make sure that the user wants to delete the scanned documents
        m_lReturn = MessageBox.Show(sMessage, "Re-Scan Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
            ' Delete the documents
            m_lReturn = ReScanDocument()
        End If

    End Sub

    ' *****************************************************************************
    '
    ' Function   : ReScanDocument
    '
    ' Description: Deletes the scanned files, and starts the scan process again
    '
    ' *****************************************************************************
    Private Function ReScanDocument() As Integer

        Dim result As Integer = 0
        Dim sFilename As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the display
            'TODO:
            'If IkDisp1.ImgHandle <> 0 Then
            '    IkCommon1.ImgHandle = IkDisp1.ImgHandle
            '    IkCommon1.FreeMemory()
            'End If

            ' Clear the page information

            ReDim g_vCurrentPageInfo(0)

            ' delete the files
            If g_lCurrentPage > 1 Then
                For iLoop1 As Integer = 1 To g_lCurrentPage - 1

                    sFilename = g_sCurrentPath & "\" & Conversion.Str(iLoop1).Trim() & DEFAULTTIFEXTENSION

                    ' delete the file if it exists
                    If FileSystem.Dir(sFilename, FileAttribute.Normal) <> "" Then
                        m_lReturn = KillFile(sFilename)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                Next
            End If

            ' reset the counters
            g_lCurrentPage = 1

            ' Scan it again
            cmdScan_Click(cmdScan, New EventArgs())

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to rescan document.", vApp:=ACApp, vClass:="frmInterface", vMethod:="ReScanDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************************
    '
    ' Function   : ReScanPage
    '
    ' Description: Deletes the scanned file, and re scans the last page
    '
    ' *****************************************************************************

    'UPGRADE_NOTE: (7001) The following declaration (ReScanPage) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ReScanPage() As Integer
    '
    'Dim result As Integer = 0
    'Dim sFilename As String = ""
    '
    'Try 
    '
    ' Clear the display
    'If IkDisp1.ImgHandle <> 0 Then
    'IkCommon1.ImgHandle = IkDisp1.ImgHandle
    'IkCommon1.FreeMemory()
    'End If
    '
    '
    'g_lCurrentPage -= 1
    'g_lPagesScanned -= 1
    '
    'sFilename = g_sCurrentPath & "\" & Conversion.Str(g_lCurrentPage).Trim() & DEFAULTTIFEXTENSION
    '
    ' delete the file if it exists
    'If FileSystem.Dir(sFilename, FileAttribute.Normal) <> "" Then
    'm_lReturn = KillFile(sFilename)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '
    'cmdScan_Click(cmdScan, New EventArgs())
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to rescan page.", vApp:=ACApp, vClass:="frmInterface", vMethod:="ReScanPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Function ScanClick() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            g_bDocumentSaved = False

            statMainbar.Items.Item(0).Text = "Scanning. Please wait..."
            statMainbar.Refresh()
            Me.Refresh()

            bScanBatch = False

            ' Scan the page / document
            m_lReturn = Scan()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Return the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Re-Enable the command buttons

            For Each Control As Control In ContainerHelper.Controls(Me)
                If TypeOf Control Is Button Then
                    Control.Enabled = True
                End If
            Next Control

            ' Disable the keywords and password buttons if stand alone
            If bIsStandAlone Then
                cmdKeywords.Enabled = False
                cmdPassword.Enabled = False
            End If

            ' This will be re-enabled if needed later on in the program
            cmdScanNext.Enabled = False
            mnuFileNextDocument.Enabled = False

            ' No view batch, as this will save the empty document
            cmdViewBatch.Enabled = False

            UpdateDocPageStats()

            IkDisp1.Width = picBorder.Width - VB6.TwipsToPixelsX(80)
            IkDisp1.Height = picBorder.Height - VB6.TwipsToPixelsY(80)

            statMainbar.Items.Item(0).Text = "Ready ..."
            statMainbar.Refresh()
            If iScanError = gPMConstants.PMEReturnCode.PMTrue And Not m_bCancelledScan Then

                If Not m_bMultiDocMode Then
                    IkDisp1.File.GetImageFileType()
                    'IkFile1.GetImageFileType()

                    statMainbar.Items.Item(1).Text = "Scanned image to " & _
                                                     g_sCurrentFilename & " [ " & _
                                                     Conversion.Str(IkFile1.FileSize).Trim() & " bytes ]"
                    statMainbar.Refresh()

                    If IkFile1.FileSize < MINIMUM_IMAGE_SIZE Then
                        SSTabHelper.SetSelectedIndex(tabMain, 2)
                        Dim frmBadScan As frmBadScan = New frmBadScan
                        frmBadScan.ShowDialog()

                    End If
                End If

                cmdScanNext.Enabled = True
                mnuFileNextDocument.Enabled = True

            Else

                ' We dont want to save an empty document if the user exits.
                g_bDocumentSaved = True

                If iScanError <> gPMConstants.PMEReturnCode.PMTrue Then
                    statMainbar.Items.Item(1).Text = "An error occured during the last scan."
                Else
                    statMainbar.Items.Item(1).Text = "Scan cancelled."
                End If
                statMainbar.Refresh()
            End If

            cmdViewBatch.Enabled = True
            cmdReScan.Enabled = True
            mnuFileReScan.Enabled = True

            If Not m_bMultiDocMode And iScanError = gPMConstants.PMEReturnCode.PMTrue And Not m_bCancelledScan Then

                m_lReturn = MessageBox.Show("The current page has scanned successfully." & Strings.Chr(13) & Strings.Chr(10) & "Do you wish to continue scanning?", "Scan Complete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                    iScanOption = SCAN_CONTINUE
                Else
                    iScanOption = SCAN_FINISH
                End If
            Else
                iScanOption = SCAN_FINISH
            End If


            bScanLoop = (iScanOption = SCAN_CONTINUE)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bScanLoop = False

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to scan document.", vApp:=ACApp, vClass:="frmInterface", vMethod:="ScanClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdScan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdScan.Click

        Dim lPages As Integer
        Dim sText As String = ""

        Try

            'SP070898 - Ensure doc name supplied
            If cboDocumentName.Text.Trim() = "" And Not optAutomatic.Checked Then
                MessageBox.Show("You must supply a document name", "ScanStation")
                cboDocumentName.Focus()
                Exit Sub
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' KR20021218   : Added code to check the user entered the mandatory
            ' (CMG)             fields. Ref. Sirius Process No. 189
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If cboPMUserLookup.ListIndex = -1 Then
                MessageBox.Show("User details must be entered", "Document Scan")
                cboPMUserLookup.Focus()
                Exit Sub
            End If

            If cboPMUserGroupByTask.ListIndex = -1 Then
                MessageBox.Show("User Group details must be entered", "Document Scan")
                cboPMUserGroupByTask.Focus()
                Exit Sub
            End If

            If txtTaskDD.Text = "" Then
                MessageBox.Show("Task Due Date must be entered", "Document Scan")
                txtTaskDD.Focus()
                Exit Sub
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            g_lPagesScannedBatch = 0

            If m_bMultiDocMode Then
                MessageBox.Show("Please make sure that the first TWO pages are blank, and that your" & Environment.NewLine & "documents are separated by TWO blank pages." & Environment.NewLine & Environment.NewLine & "Click OK to continue.", "Multiple Documents", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_bBlankScan = True
                If Not g_sScanDirectory.EndsWith("\") Then
                    m_sBlankFileName = g_sScanDirectory & "\" & ACBlankFileName
                Else
                    m_sBlankFileName = g_sScanDirectory & ACBlankFileName
                End If
            End If

            'Show the UI with the settings on it.
            'TODO:
            'IkScan1.UiMode = IMGKIT6Lib.ScanUIModeConstants.ikScanUICLOSE


            ' Scan each page, until no more are needed
            Do
                'iScanError = PMTrue

                m_lReturn = ScanClick()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

            Loop While bScanLoop

            If iScanError = gPMConstants.PMEReturnCode.PMTrue And Not m_bCancelledScan Then
                lPages = g_lPagesScannedBatch

                sText = "Batch completed. Scanned " & Conversion.Str(lPages).Trim() & " page"
                If lPages <> 1 Then
                    sText = sText & "s"
                End If
                sText = sText & "."

                statMainbar.Items.Item(1).Text = sText
                statMainbar.Refresh()
            End If

            cmdScan.Enabled = False
            mnuFileScan.Enabled = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process scan.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdScan_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function Scan() As Integer

        Dim result As Integer = 0
        Dim dDocDate, dExpiryDate As Date
        Dim sStatusText As String = ""
        Dim lTimerVal As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Do we want to display the scanned image?
            If cboDisplay.Text <> "No" Then
                SSTabHelper.SetSelectedIndex(tabMain, 2)
                Me.Refresh()
            End If
            IkFile1 = New Newtone.ImageKit.IkFile()
            IkFile1.TiffAppend = (optMultiYes.Checked)

            ' No error scanning
            iScanError = gPMConstants.PMEReturnCode.PMTrue

            ' Save last filename
            g_sLastFilename = g_sCurrentFilename

            ' Work out the filename that will need to be saved
            g_sCurrentFilename = g_sCurrentPath

            If Not g_sCurrentPath.EndsWith("\") Then
                g_sCurrentFilename = g_sCurrentFilename & "\"
            End If

            If optMultiYes.Checked Then
                g_sCurrentFilename = g_sCurrentFilename & "1" & DEFAULTTIFEXTENSION
            Else
                g_sCurrentFilename = g_sCurrentFilename & Conversion.Str(g_lCurrentPage).Trim() & DEFAULTTIFEXTENSION
            End If

            If g_sLastFilename = "" Then
                g_sLastFilename = g_sCurrentFilename
            End If

            If optMultiYes.Checked And Not m_bMultiDocMode Then
                sStatusText = "Scanning to " & g_sLastFilename
            Else
                sStatusText = "Scanning to " & g_sCurrentFilename
            End If

            ' Set the status bar text, and refresh it so it displays it!
            statMainbar.Items.Item(1).Text = sStatusText
            statMainbar.Refresh()

            ' Scan it
            m_lReturn = ScanPage()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If iScanError <> gPMConstants.PMEReturnCode.PMTrue Or m_bCancelledScan Then
                Return result
            End If

            lCurrentFolderNum = 0

            ' Check that the document date, and expiry date are valid
            If Information.IsDate(txtExpiryDate.Text) Then
                dDocDate = CDate(txtDocumentDate.Text)
            End If

            If Information.IsDate(txtExpiryDate.Text) Then
                dExpiryDate = CDate(txtExpiryDate.Text)
            End If

            If cboDisplay.Text <> "No" Then

                ' Start the timer to display the image for the number of seconds required
                If cboDisplay.Text <> "Yes" Then
                    m_lReturn = GetDisplayTimer(lTimeVal:=lTimerVal)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    If lTimerVal = 0 Then
                        tmrDisplay.Enabled = False
                    Else
                        tmrDisplay.Interval = lTimerVal
                        tmrDisplay.Enabled = True
                    End If
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Reset the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


            Select Case Information.Err().Number
                Case 53, 0

                    ' File not found?
                    If Not m_bMultiDocMode Then

                        result = gPMConstants.PMEReturnCode.PMError

                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to scan document.", vApp:=ACApp, vClass:=ACClass, vMethod:="Scan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                    Else

                        ' That's ok in multidoc mode

                        Return gPMConstants.PMEReturnCode.PMTrue

                    End If

                Case Else

                    ' Error Section.
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to scan document.", vApp:=ACApp, vClass:=ACClass, vMethod:="Scan", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            End Select




            Return result
        End Try
    End Function


    ' ************************************************************************
    ''JH021198 copied structure from frmInterface of iDOCOptions
    '
    ' Function: LoadSettings
    '
    ' Desc.   : Loads the settings from either the registry,
    '           setting the defaults if needed.
    '
    ' ************************************************************************

    Private Function LoadSettings() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {{{{{{{ Image Options }}}}}}

            g_ImageOptions.Colours.iLocation = REGISTRY_SYSTEM
            g_ImageOptions.Colours.sKey = DOCScanColoursKey
            g_ImageOptions.Colours.sSubKey = DOCScanOptionsSection
            g_ImageOptions.Colours.sDefault = ""
            g_ImageOptions.Colours.bNumber = False

            g_ImageOptions.Contrast.iLocation = REGISTRY_SYSTEM
            g_ImageOptions.Contrast.sKey = DOCScanContrastKey
            g_ImageOptions.Contrast.sSubKey = DOCScanOptionsSection
            g_ImageOptions.Contrast.sDefault = ""
            g_ImageOptions.Contrast.bNumber = False

            g_ImageOptions.DPI.iLocation = REGISTRY_SYSTEM
            g_ImageOptions.DPI.sKey = DOCScanDPIKey
            g_ImageOptions.DPI.sSubKey = DOCScanOptionsSection
            g_ImageOptions.DPI.sDefault = PMDOCDPI100
            g_ImageOptions.DPI.bNumber = True

            g_ImageOptions.ScanMode.iLocation = REGISTRY_SYSTEM
            g_ImageOptions.ScanMode.sKey = DOCScanModeKey
            g_ImageOptions.ScanMode.sSubKey = DOCScanOptionsSection
            g_ImageOptions.ScanMode.sDefault = ""
            g_ImageOptions.ScanMode.bNumber = False

            g_ImageOptions.ScanSize.iLocation = REGISTRY_SYSTEM
            g_ImageOptions.ScanSize.sKey = DOCScanSizeKey
            g_ImageOptions.ScanSize.sSubKey = DOCScanOptionsSection
            g_ImageOptions.ScanSize.sDefault = "A4"
            g_ImageOptions.ScanSize.bNumber = False

            ' Read from the registry
            m_lReturn = LoadSetting(vSetting:=g_ImageOptions.Colours)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_ImageOptions.Contrast)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_ImageOptions.DPI)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_ImageOptions.ScanSize)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_ImageOptions.ScanMode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {{{{{{{ Scan Options }}}}}}

            g_ScanOptions.Batch.iLocation = REGISTRY_SYSTEM
            g_ScanOptions.Batch.sKey = DOCScanBatchKey
            g_ScanOptions.Batch.sSubKey = DOCScanOptionsSection
            g_ScanOptions.Batch.sDefault = "N"
            g_ScanOptions.Batch.bNumber = False

            g_ScanOptions.Confirm.iLocation = REGISTRY_SYSTEM
            g_ScanOptions.Confirm.sKey = DOCScanConfirmKey
            g_ScanOptions.Confirm.sSubKey = DOCScanOptionsSection
            g_ScanOptions.Confirm.sDefault = "Y"
            g_ScanOptions.Confirm.bNumber = False

            g_ScanOptions.Flatbed.iLocation = REGISTRY_SYSTEM
            g_ScanOptions.Flatbed.sKey = DOCScanFlatbedKey
            g_ScanOptions.Flatbed.sSubKey = DOCScanOptionsSection
            g_ScanOptions.Flatbed.sDefault = "Y"
            g_ScanOptions.Flatbed.bNumber = False

            ' Read from the registry
            m_lReturn = LoadSetting(vSetting:=g_ScanOptions.Batch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_ScanOptions.Confirm)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_ScanOptions.Flatbed)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {{{{{{{ Advanced Options }}}}}}

            g_AdvOptions.AutoDocName.iLocation = REGISTRY_SYSTEM
            g_AdvOptions.AutoDocName.sKey = DOCScanAutoDocNameKey
            g_AdvOptions.AutoDocName.sSubKey = DOCScanOptionsSection
            g_AdvOptions.AutoDocName.sDefault = "Y"
            g_AdvOptions.AutoDocName.bNumber = False

            g_AdvOptions.Display.iLocation = REGISTRY_SYSTEM
            g_AdvOptions.Display.sKey = DOCScanDisplayKey
            g_AdvOptions.Display.sSubKey = DOCScanOptionsSection
            g_AdvOptions.Display.sDefault = PMDOCDISPLAYYESTIMED
            g_AdvOptions.Display.bNumber = False

            g_AdvOptions.DisplayTime.iLocation = REGISTRY_SYSTEM
            g_AdvOptions.DisplayTime.sKey = DOCScanDisplayTimeKey
            g_AdvOptions.DisplayTime.sSubKey = DOCScanOptionsSection
            g_AdvOptions.DisplayTime.sDefault = PMDOCDEFAULTTIME
            g_AdvOptions.DisplayTime.bNumber = True

            g_AdvOptions.MultiTiff.iLocation = REGISTRY_SYSTEM
            g_AdvOptions.MultiTiff.sKey = DOCScanMultiTiffKey
            g_AdvOptions.MultiTiff.sSubKey = DOCScanOptionsSection
            g_AdvOptions.MultiTiff.sDefault = "Y"
            g_AdvOptions.MultiTiff.bNumber = False

            g_AdvOptions.TabSelected.iLocation = REGISTRY_SYSTEM
            g_AdvOptions.TabSelected.sKey = DOCScanTabSelectedKey
            g_AdvOptions.TabSelected.sSubKey = DOCScanOptionsSection
            g_AdvOptions.TabSelected.sDefault = "0"
            g_AdvOptions.TabSelected.bNumber = True

            g_SiriusOptions.TaskEnabled.iLocation = REGISTRY_SYSTEM
            g_SiriusOptions.TaskEnabled.sKey = DOCScanTaskEnabledKey
            g_SiriusOptions.TaskEnabled.sSubKey = DOCScanOptionsSection
            g_SiriusOptions.TaskEnabled.sDefault = "0"
            g_SiriusOptions.TaskEnabled.bNumber = True

            m_lReturn = LoadSetting(vSetting:=g_AdvOptions.AutoDocName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_AdvOptions.Display)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = LoadSetting(vSetting:=g_AdvOptions.DisplayTime)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_AdvOptions.MultiTiff)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = LoadSetting(vSetting:=g_AdvOptions.TabSelected)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DN 15/05/01
            m_lReturn = LoadSetting(vSetting:=g_SiriusOptions.TaskEnabled)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            Stop


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get values from the registry and database.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ************************************************************************
    ''JH021198 copied structure from frmInterface of iDOCOptions
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

            ' Read from the registry
            m_lReturn = GetRegistryValue(sKey:=vSetting.sKey, sSubKey:=vSetting.sSubKey, sValue:=vSetting.sValue, iLocation:=vSetting.iLocation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Apply the default if necessary
            If vSetting.sValue = "" Then

                vSetting.sValue = vSetting.sDefault

                ' Write it back to registry now
                m_lReturn = SetRegistryValue(sSubKey:=vSetting.sSubKey, sKey:=vSetting.sKey, sValue:=vSetting.sValue, iLocation:=vSetting.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception


            Stop


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get value from the registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' ************************************************************************
    'JH021198 copied structure from frmInterface of iDOCOptions
    ' but writes directly to setregistryvalue
    '
    ' Function: GrabChanges
    '
    ' Desc.   : Checks if any of the settings have been changed, and if so,
    '           then it saves them.
    '
    ' ************************************************************************

    'UPGRADE_NOTE: (7001) The following declaration (GrabChanges) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    Private Function GrabChanges() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '{{{{{{{ Image Options }}}}}}

            If g_ImageOptions.Colours.bChanged Then
                ' Write it back to registry now
                m_lReturn = SetRegistryValue(sSubKey:=g_ImageOptions.Colours.sSubKey, sKey:=g_ImageOptions.Colours.sKey, sValue:=g_ImageOptions.Colours.sValue, iLocation:=g_ImageOptions.Colours.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_ImageOptions.Contrast.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_ImageOptions.Contrast.sSubKey, sKey:=g_ImageOptions.Contrast.sKey, sValue:=g_ImageOptions.Contrast.sValue, iLocation:=g_ImageOptions.Contrast.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_ImageOptions.DPI.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_ImageOptions.DPI.sSubKey, sKey:=g_ImageOptions.DPI.sKey, sValue:=g_ImageOptions.DPI.sValue, iLocation:=g_ImageOptions.DPI.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_ImageOptions.ScanMode.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_ImageOptions.ScanMode.sSubKey, sKey:=g_ImageOptions.ScanMode.sKey, sValue:=g_ImageOptions.ScanMode.sValue, iLocation:=g_ImageOptions.ScanMode.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            '{{{{{{{ Scan Options }}}}}}

            If g_ScanOptions.Batch.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_ScanOptions.Batch.sSubKey, sKey:=g_ScanOptions.Batch.sKey, sValue:=g_ScanOptions.Batch.sValue, iLocation:=g_ScanOptions.Batch.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_ScanOptions.Confirm.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_ScanOptions.Confirm.sSubKey, sKey:=g_ScanOptions.Confirm.sKey, sValue:=g_ScanOptions.Confirm.sValue, iLocation:=g_ScanOptions.Confirm.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_ScanOptions.Flatbed.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_ScanOptions.Flatbed.sSubKey, sKey:=g_ScanOptions.Flatbed.sKey, sValue:=g_ScanOptions.Flatbed.sValue, iLocation:=g_ScanOptions.Flatbed.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            '  {{{{{{{ Advanced Options }}}}}}

            If g_AdvOptions.AutoDocName.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_AdvOptions.AutoDocName.sSubKey, sKey:=g_AdvOptions.AutoDocName.sKey, sValue:=g_AdvOptions.AutoDocName.sValue, iLocation:=g_AdvOptions.AutoDocName.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_AdvOptions.Display.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_AdvOptions.Display.sSubKey, sKey:=g_AdvOptions.Display.sKey, sValue:=g_AdvOptions.Display.sValue, iLocation:=g_AdvOptions.Display.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_AdvOptions.DisplayTime.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_AdvOptions.DisplayTime.sSubKey, sKey:=g_AdvOptions.DisplayTime.sKey, sValue:=g_AdvOptions.DisplayTime.sValue, iLocation:=g_AdvOptions.DisplayTime.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_AdvOptions.MultiTiff.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_AdvOptions.MultiTiff.sSubKey, sKey:=g_AdvOptions.MultiTiff.sKey, sValue:=g_AdvOptions.MultiTiff.sValue, iLocation:=g_AdvOptions.MultiTiff.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If g_AdvOptions.TabSelected.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_AdvOptions.TabSelected.sSubKey, sKey:=g_AdvOptions.TabSelected.sKey, sValue:=g_AdvOptions.TabSelected.sValue, iLocation:=g_AdvOptions.TabSelected.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            '  DN(15 / 5 / 1)
            If g_SiriusOptions.TaskEnabled.bChanged Then
                m_lReturn = SetRegistryValue(sSubKey:=g_SiriusOptions.TaskEnabled.sSubKey, sKey:=g_SiriusOptions.TaskEnabled.sKey, sValue:=g_SiriusOptions.TaskEnabled.sValue, iLocation:=g_SiriusOptions.TaskEnabled.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save changed values.", vApp:=ACApp, vClass:=ACClass, vMethod:="GrabChanges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cmdScanNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdScanNext.Click

        m_lReturn = PrepareNextDocument()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' This error will have already been logged
            Exit Sub
        End If

    End Sub

    ' **********************************************************************
    ' Function   : PrepareNextDocument
    '
    ' Description: Prepares the command buttons, and intialises the page/doc
    '              counters for the next document.
    '
    ' **********************************************************************
    Public Function PrepareNextDocument(Optional ByVal v_bLeaveControls As Boolean = True) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not g_bDocumentSaved Then
                m_lReturn = SaveDocument()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Initialise for the next scan
            NextDocument()

            g_lDocumentsScanned += 1

            UpdateDocPageStats()

            ' CTAF 20030814 - Leave the controls alone?
            If v_bLeaveControls Then
                cmdScan.Enabled = True
                cmdScanNext.Enabled = False
                cmdReScan.Enabled = False

                mnuFileScan.Enabled = True
                mnuFileNextDocument.Enabled = False
                mnuFileReScan.Enabled = False

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to initialise save document and move to next number.", vApp:=ACApp, vClass:="frmInterface", vMethod:="PrepareNextDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdSelectDestination_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectDestination.Click

        'nd 071100 - hide so we can select another folder
        m_bHiddenForFolderSelect = True
        Me.Hide()

    End Sub

    Private Sub cmdTaskDDReset_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTaskDDReset.Click

        ' Reset back to proper date
        txtTaskDD.Text = DateTime.Now.ToString("dd MMMM yyyy")

    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdView_2.Click, _cmdView_1.Click, _cmdView_0.Click
        Dim Index As Integer = Array.IndexOf(cmdView, eventSender)

        Dim lTwipsPerPixel As Integer

        Select Case Index
            Case 0
                IkDisp1.ScrollBar = True

                IkFile1.ImageHandle = IkDisp1.Handle
                'TODO
                'IkCommon1.GetImageType()

                lTwipsPerPixel = CInt(VB6.TwipsPerPixelY())

                'IkDisp1.DispScale = (VB6.PixelsToTwipsY(IkDisp1.Height) / lTwipsPerPixel) / IkCommon1.ImgHeight


                IkDisp1.Display(Newtone.ImageKit.Win.DisplayMode.ActualSize)

            Case 1
                IkDisp1.ScrollBar = True

                'IkCommon1.ImgHandle = IkDisp1.ImgHandle
                IkFile1.ImageHandle = IkDisp1.Handle
                'IkCommon1.GetImageType()

                lTwipsPerPixel = CInt(VB6.TwipsPerPixelX())

                'IkDisp1.DispScale = (VB6.PixelsToTwipsX(IkDisp1.Width) / lTwipsPerPixel) / IkCommon1.ImgWidth
                IkDisp1.Display(Newtone.ImageKit.Win.DisplayMode.ActualSize)

            Case 2
                IkDisp1.ScrollBar = False
                IkDisp1.Display(Newtone.ImageKit.Win.DisplayMode.Stretch)
        End Select
    End Sub

    Private Sub cmdViewBatch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdViewBatch.Click

        Try

            If Not g_bDocumentSaved Then

                m_lReturn = PrepareNextDocument()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Display the error message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="An error occured saving the current document.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdViewBatch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

            End If

            ' View the batch of images

            m_lReturn = g_oViewBatch.ViewBatch()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to initialise View Batch object.", vApp:=ACApp, vClass:="frmInterface", vMethod:="cmdViewBatch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Function GetDateOffsets() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the document date offsets

            m_lReturn = m_oDOCScan.GetDocDateOffSets(iDocDateOffset:=iDocDateOffset, iExpiryDateOffset:=iDocExpiryOffset)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="An error occured getting the date offsets.", vApp:=ACApp, vClass:="frmInterface", vMethod:="GetDateOffsets", excep:=excep)

            Return result

        End Try
    End Function

    Private Sub InitialiseForm()

        Dim oDocCommitSrv As bDOCCommitServer.Commit

        Try

            iScanError = gPMConstants.PMEReturnCode.PMFalse

            ' Initialise the variables
            g_lPagesScanned = 0
            g_lDocumentsScanned = 0
            g_lCurrentPage = 1

            g_bDocumentSaved = True

            ' Access level is 9, always by default
            cboAccessLevel.Text = "9"

            If bIsStandAlone Then

                txtScanUser.Text = "DMSSCAN"
                txtParentFolderName.Text = ""
                txtScanFolder.Text = "Scan Folder"

                cmdKeywords.Enabled = False
                cmdPassword.Enabled = False

                iDocDateOffset = 0
                iDocExpiryOffset = 0

                ' ND 201000 - Hide SBO tab
                SSTabHelper.SetTabVisible(tabMain, 3, False)
                'DN 10/05/01
                chkEnableTask.CheckState = CheckState.Unchecked

            Else

                txtScanUser.Text = sCurrentUserName

                ' Get the document name history
                GetDocNameHistory()

                ' Get Date Offsets
                GetDateOffsets()

                ' ND 201000 - Check SA database on server for existance of SBO

                ' get link to server
                'Dim temp_oDocCommitSrv As Object
                'm_lReturn = g_oObjectManager.GetInstance(temp_oDocCommitSrv, "bDocCommitServer.commit", vInstanceManager:=PMGetViaClientManager)
                'oDocCommitSrv = temp_oDocCommitSrv
                oDocCommitSrv = New bDOCCommitServer.Commit

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create instance of oDocCommitSrv.commit", vApp:=ACApp, vClass:=ACClass, vMethod:="InitiliseForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    g_bSBOInstalled = False

                End If

                ' check for existance of SBO in SA database on server

                m_lReturn = oDocCommitSrv.IsSBOInstalled(g_bSBOInstalled)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to determine existance of Sirius Back-office on server." & _
                               Strings.Chr(13) & Strings.Chr(10) & "disabling integration.", vApp:=ACApp, vClass:=ACClass, vMethod:="InitiliseForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    g_bSBOInstalled = False

                End If

                ' if SBO is not installed hide SBO tab
                If (Not g_bSBOInstalled) Then SSTabHelper.SetTabVisible(tabMain, 3, False)

            End If


            SSTabHelper.SetSelectedIndex(tabMain, 0)

            'set the stuff from the registry
            SetInterfaceDefaults()

            'DN 10/05/01 - Enable or disable task scheduler
            If chkEnableTask.CheckState = CheckState.Unchecked Then
                FraRecipient.Enabled = False
                Label2.Enabled = False
                cboPMUserLookup.Enabled = False
                Label3.Enabled = False
                cboPMUserGroupByTask.Enabled = False
                Label1.Enabled = False
                cboCustomer.Enabled = False
                Label4.Enabled = False
                txtTaskDD.Enabled = False
                cmdTaskDDReset.Enabled = False
                chkUrgent.Enabled = False
            Else
                FraRecipient.Enabled = True
                Label2.Enabled = True
                cboPMUserLookup.Enabled = True
                Label3.Enabled = True
                cboPMUserGroupByTask.Enabled = True
                Label1.Enabled = True
                cboCustomer.Enabled = True
                Label4.Enabled = True
                txtTaskDD.Enabled = True
                cmdTaskDDReset.Enabled = True
                chkUrgent.Enabled = True
            End If

            ' Update the display
            UpdateDocPageStats()

            ' Initialise for the next scan
            NextDocument()

            'BE 5/9/2003 CQ2479 filter cboUserLookup
            cboPMUserGroupByTask_ClickEvent(cboPMUserGroupByTask, New EventArgs())

        Catch excep As System.Exception



            iScanError = Information.Err().Number


            Select Case Information.Err().Number
                Case 20391, 20392

                    ' Display the error message
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Please make sure the scanner is switched on and properly connected.", vApp:=ACApp, vClass:="kscan", vMethod:="KSCANSTART", excep:=excep)
                Case Else

                    ' Display the error message
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="An error occured during intialising the form.", vApp:=ACApp, vClass:="frmInterface", vMethod:="IntializeForm", excep:=excep)

            End Select

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' ND 071100 - set so if user exists scanstation do not hide
            m_bHiddenForFolderSelect = False

        End If
    End Sub



    Public Sub frmInterface_Load()

        Try

            bInitialising = True

            InitialiseForm()

            '            m_lReturn = IkScan1.ScanInitialize(Handle.ToInt32(), 1, 0, "ImageKit6 Sample", "NEWTONE Corp.", "ImageKit", "ImageKit6")
            m_lReturn = IkDisp1.Scan.Initialize(Me.Handle, 1, 0, "ImageKit.NET Sample", "NEWTONE Corp.", "ImageKit", "ImageKit.NET")

            IkDisp1.Width = picBorder.Width
            IkDisp1.Height = picBorder.Height
            IkDisp1.Left = 0
            IkDisp1.Top = 0
            'TODO
            'IkDisp1.ImgHandle = 0
            '

            PopulateScanner()

            Me.Refresh()

            bInitialising = False
            statMainbar.Width = Me.Width
            statMainbar.Stretch = True
            _statMainbar_Panel1.Width = Me.Width * (22 / 100)
            _statMainbar_Panel3.Width = Me.Width * (22 / 100)
            _statMainbar_Panel2.Width = Me.Width * (52 / 100)
            Me.cboPMUserGroupByTask.FirstItem = ""
        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Error loading the form.", vApp:=ACApp, vClass:="frmInterface", vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        'ND 071100 do show this if we are just hiding
        If Not m_bHiddenForFolderSelect Then
            m_lReturn = MessageBox.Show("Are you sure you wish to exit ScanStation?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
            On Error Resume Next
            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                Cancel = False
                bAppClosing = True
            Else
                Cancel = True
            End If
        End If

        eventArgs.Cancel = Cancel <> 0
    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        Try

            m_lReturn = IkDisp1.Scan.Terminate()

            'If IkDisp1.ImgHandle <> 0 Then
            '    IkCommon1.ImgHandle = IkDisp1.ImgHandle
            '    IkCommon1.FreeMemory()
            '    IkDisp1.ImgHandle = 0
            'End If

        Catch



            MemoryHelper.ReleaseMemory()
        End Try

    End Sub
    'Private Sub imsTwainScan_PageDone(ByVal PageNumber As Long)
    '
    'Static lFileSize As Long
    'Static lBlankSize As Long
    'Dim lNewSize As Long
    'Static bDeleted As Boolean
    '
    '    On Error GoTo Err_PageDone
    '
    '    ' Don't process the rest of this if we aren't in multidoc mode
    '    If (m_bMultiDocMode = False) Then
    '        Exit Sub
    '    End If
    '
    '    ' We just want the filesize if it's a blank page (the first)
    '    If (m_bBlankScan = True) Then
    '        'm_lBlankSize = FileLen(imsTwainScan.Image)
    '        'imsTwainScan.StopScan
    '        lFileSize = 0
    '        Exit Sub
    '    End If
    '
    '    'lNewSize = FileLen(imsTwainScan.Image)
    '
    '    Debug.Print "Page: " & Format(PageNumber, "000") & " Size: " & Format(lNewSize, "00000000")
    '
    '    If lNewSize < (lFileSize + m_lBlankSize) Then
    '
    '        ' Stop the scanner
    '        'imsTwainScan.StopScan
    '        'imsTwainScan.CloseScanner
    '
    '        ' Delete the last page because it was blank
    '        ' stick this at the top of the loop. if we've done one document then do this code
    '        'ImgAdmin.Image = imsTwainScan.Image
    '        'ImgAdmin.DeletePages ImgAdmin.PageCount, 1
    '
    '
    '        ' Reset the filesize
    '        lFileSize = 0
    '
    '    Else
    '
    '        ' Save the filesize
    '        lFileSize = lNewSize
    '
    '    End If
    '
    '
    '
    '    Exit Sub
    '
    'Err_PageDone:
    '
    '    Exit Sub
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (ScanBlankPage) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ScanBlankPage() As Integer
    '
    'Dim result As Integer = 0
    'Dim Index(10) As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' We're doing a blank page scan
    'm_bBlankScan = True
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
    '
    ' Activate the scanner
    'm_lReturn = IkScan1.ScanExec(Index(0))
    '
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    '
    'm_bBlankScan = False
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Display the error message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:="frmInterface", vMethod:="ScanBlankPage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function



    Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click

        cmdOK_Click()

    End Sub

    Public Sub mnuFileNextDocument_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileNextDocument.Click

        cmdScanNext_Click(cmdScanNext, New EventArgs())

    End Sub

    Public Sub mnuFileReScan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileReScan.Click

        cmdReScan_Click(cmdReScan, New EventArgs())

    End Sub

    Public Sub mnuFileScan_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileScan.Click

        cmdScan_Click(cmdScan, New EventArgs())

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


        Try

            ' Set the application title
            sTitle = "DocuMaster Enterprise"

            ' Set the version number and date
            sVersionNumber = CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision)

            sVersionDate = DateTimeHelper.ToString((New FileInfo(My.Application.Info.DirectoryPath & "\" & My.Application.Info.AssemblyName & ".dll")).LastWriteTime)
            sVersionDate = sVersionDate.Substring(0, 8)

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

        Catch excep As System.Exception



            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="mnuHelpAbout_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuOptionsMultiDoc_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuOptionsMultiDoc.Click

        Try

            ' Prompt if they wish to enable this mode
            ' Also gives information on the blank sheet
            If Not mnuOptionsMultiDoc.Checked Then

                m_lReturn = MessageBox.Show("This mode will scan multiple documents seperated by two blank sheets of paper." & Environment.NewLine & _
                            "You should only use this with a scanner with a sheet feeder attached." & Environment.NewLine & "Do you wish to enable this mode?", "Multiple Document", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                    mnuOptionsMultiDoc.Checked = True

                    optMultiYes.Checked = True
                    optMultiYes.Enabled = False
                    optMultiNo.Enabled = False
                    optAutomatic.Checked = True
                    optAutomatic.Enabled = False
                    optManual.Enabled = False
                End If

            Else

                mnuOptionsMultiDoc.Checked = False
                optMultiYes.Enabled = True
                optMultiNo.Enabled = True
                optAutomatic.Enabled = True
                optManual.Enabled = True

            End If


            m_bMultiDocMode = mnuOptionsMultiDoc.Checked

        Catch



            Exit Sub
        End Try


    End Sub

    Public Sub mnuOptionsSettings_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuOptionsSettings.Click

        Dim sMessage, sDirectory As String

        Try

            sDirectory = g_sScanDirectory

            m_lReturn = ChangeDOCRegSettings(vScanDirectory:=sDirectory, hWndParent:=Me.Handle.ToInt32())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Only assign new directory, if its valid and differnt to old one
            If (sDirectory <> g_sScanDirectory) And (sDirectory <> "") Then

                g_sScanDirectory = sDirectory

                sMessage = "Note that changes will not take effect until the" & _
                           Strings.Chr(13).ToString() & Strings.Chr(10).ToString() & _
                           "ScanStation is restarted."

                MessageBox.Show(sMessage, "Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)

            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to initialise get change settings", vApp:=ACApp, vClass:="frmInterface", vMethod:="mnuToolsSettings_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Sub mnuTab_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuTab_0.Click, _mnuTab_1.Click, _mnuTab_2.Click
        Dim Index As Integer = Array.IndexOf(mnuTab, eventSender)

        ' switch the tab to the selected menu option
        SSTabHelper.SetSelectedIndex(tabMain, Index)

    End Sub

    Private Sub optAutomatic_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optAutomatic.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            Dim sDocName As String = ""

            g_AdvOptions.AutoDocName.bChanged = True

            If optAutomatic.Checked Then
                sDocName = "Document" & Conversion.Str(g_iDocNum).Trim()
                cboDocumentName.Text = sDocName
                cboDocumentName.Enabled = False
                g_AdvOptions.AutoDocName.sValue = "Y"
            Else
                cboDocumentName.Enabled = True
                g_AdvOptions.AutoDocName.sValue = "N"
            End If


            'SP030500 - Changed to save change to registry
            With g_AdvOptions.AutoDocName
                .bChanged = True
                m_lReturn = SetRegistryValue(sSubKey:=.sSubKey, sKey:=.sKey, sValue:=.sValue, iLocation:=.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    .bChanged = False
                    Exit Sub
                End If
            End With

        End If
    End Sub


    Private Sub optManual_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optManual.CheckedChanged


        optAutomatic_CheckedChanged(optManual, New EventArgs())


    End Sub

    Private Sub optMultiNo_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optMultiNo.CheckedChanged

        ' PW190603 - CQ1155
        '    imsTwainScan.PageOption = CreateNewFile

        'UpdateDocPageStats
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            g_AdvOptions.MultiTiff.bChanged = True

            If Not optMultiNo.Checked Then
                g_AdvOptions.MultiTiff.sValue = "Y"
            Else
                g_AdvOptions.MultiTiff.sValue = "N"
            End If

            'SP030500 - Changed to save change to registry
            With g_AdvOptions.MultiTiff
                .bChanged = True
                m_lReturn = SetRegistryValue(sSubKey:=.sSubKey, sKey:=.sKey, sValue:=.sValue, iLocation:=.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    .bChanged = False
                    Exit Sub
                End If
            End With
        End If
    End Sub

    Private Sub optMultiYes_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optMultiYes.CheckedChanged

        Dim sMessage As String = ""

        ' PW190603 - CQ1155
        '    imsTwainScan.PageOption = AppendPages
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            If Not bInitialising Then
                sMessage = "It is not recommended that you use this option if" & _
                           Strings.Chr(10).ToString() & Strings.Chr(13).ToString() & _
                           "you intend scanning more than 50 pages to a document."

                MessageBox.Show(sMessage, "Scan to multi-page TIFF", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            'UpdateDocPageStats

            g_AdvOptions.MultiTiff.bChanged = True

            If optMultiYes.Checked Then
                g_AdvOptions.MultiTiff.sValue = "Y"
            Else
                g_AdvOptions.MultiTiff.sValue = "N"
            End If

            'SP030500 - Changed to save change to registry
            With g_AdvOptions.MultiTiff
                .bChanged = True
                m_lReturn = SetRegistryValue(sSubKey:=.sSubKey, sKey:=.sKey, sValue:=.sValue, iLocation:=.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    .bChanged = False
                    Exit Sub
                End If
            End With

        End If
    End Sub

    ' ***************************************************************
    ' Once this has been called, then time to end the picture display
    ' ***************************************************************
    Private Sub tmrDisplay_Tick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tmrDisplay.Tick

        tmrDisplay.Enabled = False
        SSTabHelper.SetSelectedIndex(tabMain, 0)

    End Sub

    Private Function ClearDocument() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the display
            'TODO:
            'If IkDisp1.ImgHandle <> 0 Then
            '    IkCommon1.ImgHandle = IkDisp1.ImgHandle
            '    IkCommon1.FreeMemory()
            'End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to clear document view.", vApp:=ACApp, vClass:="frmInterface", vMethod:="ClearDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function NextDocument() As Integer

        Dim result As Integer = 0
        Dim sDocName As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            statMainbar.Items.Item(1).Text = ""

            ' Clear the document, ready for the next
            ClearDocument()

            bMovedNextPage = True

            '    g_sLastFilename = ""
            '    g_sCurrentFilename = ""

            sCurrentPassword = ""

            ' CF 050898 - Oops. Fixed keywords problem.

            ' Clear the keyword list
            ReDim vKeywords(0)


            vKeywords(0) = ""

            ' Clear the page information
            ReDim g_vCurrentPageInfo(0)


            g_vCurrentPageInfo(0) = 0

            ReDim vAnnotations(0)

            ' Clear the annotations

            vAnnotations(0) = ""
            lNumberAnnotations = 0


            ' Get the next document number

            m_lReturn = m_oDOCScan.GetNextDocNum(iDocNum:=g_iDocNum)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sob 271099 Move to below to accuratly record current doc No
            ' Get the current document number
            'iCurrentDocument = g_iDocNum

            ' Reset the current page number
            g_lCurrentPage = 1

            ' Reset the number of pages scanned
            g_lPagesScanned = 1

            ' Reset the annotation
            sCurrentAnnotation.Value = ""

            ' Update the screen with the new numbers
            UpdateDocPageStats()

            ' Calculate the new directory for the document
            g_sCurrentPath = g_sScanDirectory

            If Not g_sScanDirectory.EndsWith("\") Then
                g_sCurrentPath = g_sCurrentPath & "\"
            End If

            'SOB271099 Making sure you can save the next file
            g_sCurrentPath = g_sCurrentPath & "Doc"  '& Trim$(Str$(g_iDocNum)) cheak to make sure valid file name
            'Make sure you can save next document
            Do While FileSystem.Dir(g_sCurrentPath & Conversion.Str(g_iDocNum).Trim() & "\*.*", FileAttribute.Normal) <> ""
                g_iDocNum += 1
            Loop
            g_sCurrentPath = g_sCurrentPath & Conversion.Str(g_iDocNum).Trim()

            'SOB 271099 Get the current document number
            iCurrentDocument = g_iDocNum


            g_sLastFilename = g_sCurrentFilename

            ' Work out the filename that will need to be saved
            g_sCurrentFilename = g_sCurrentPath

            If Not g_sCurrentPath.EndsWith("\") Then
                g_sCurrentFilename = g_sCurrentFilename & "\"
            End If

            g_sCurrentFilename = g_sCurrentFilename & Conversion.Str(g_lCurrentPage).Trim() & DEFAULTTIFEXTENSION

            If g_sLastFilename = "" Then
                g_sLastFilename = g_sCurrentFilename
            End If


            ' if the scan directory doesnt exist
            If Not Directory.Exists(g_sScanDirectory) Then
                ' then create it
                Directory.CreateDirectory(g_sScanDirectory)
            End If

            ' If the document directory doesnt exit
            If Not Directory.Exists(g_sCurrentPath) Then
                ' then create it
                Directory.CreateDirectory(g_sCurrentPath)
            End If

            ' If automatic document naming, then work it out
            If optAutomatic.Checked Then
                sDocName = "Document" & Conversion.Str(g_iDocNum).Trim()
                cboDocumentName.Text = sDocName
            Else
                sDocName = ""
                cboDocumentName.Text = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process next document.", vApp:=ACApp, vClass:="interface", vMethod:="NextDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function UpdateDocPageStats() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtPagesScanned.Text = Conversion.Str(g_lPagesScanned).Trim()

            If g_lPagesScanned > 0 Then
                txtDocumentsScanned.Text = Conversion.Str(g_lDocumentsScanned + 1).Trim()
            Else
                txtDocumentsScanned.Text = Conversion.Str(g_lDocumentsScanned).Trim()
            End If

            txtCreateDate.Text = DateTimeHelper.ToString(DateTime.Now)

            ' Add the offsets to the dates, and format them
            txtDocumentDate.Text = DateTime.Now.AddDays(iDocDateOffset).ToString("dd MMMM yyyy")
            txtExpiryDate.Text = DateTime.Now.AddDays(iDocExpiryOffset).ToString("dd MMMM yyyy")

            statMainbar.Items.Item(2).Text = "Current Page :" & Conversion.Str(g_lCurrentPage)

            If optMultiYes.Checked Then
                statMainbar.Items.Item(2).Text = statMainbar.Items.Item(2).Text & " [M]"
            End If
            statMainbar.Refresh()
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to update document and page statistics.", vApp:=ACApp, vClass:="frmInterface", vMethod:="UpdateDocPageStats", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function ValidateValues() As Integer

        Dim result As Integer = 0
        Try


            ' TODO

            ' Validate Dates

            ' Validate Document Name





            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to save document.", vApp:=ACApp, vClass:="interface", vMethod:="ValidateValues", excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************************
    ' Function    : SaveDocument
    ' Description : Saves the current page details and document information to the
    '               local access database, via the business object.
    '
    ' *****************************************************************************

    Private Function SaveDocument() As Integer

        Dim result As Integer = 0
        Dim dDocDate, dExpiryDate As Date
        Dim sDocumentName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            g_bDocumentSaved = True

            dDocDate = CDate(txtDocumentDate.Text)
            dExpiryDate = DateTime.Parse(txtExpiryDate.Text)
            sDocumentName = cboDocumentName.Text.Substring(0, 50)

            ' if SBO installed then get further interface details
            'DN 15/05/01 - Only if enabled
            If g_bSBOInstalled And chkEnableTask.CheckState = CheckState.Checked Then
                m_sCustomer = cboCustomer.Text
                m_dtTaskDueDate = CDate(txtTaskDD.Text)
                m_iUrgent = chkUrgent.CheckState
                m_lPMUserGroup = cboPMUserGroupByTask.UserGroupID
                m_lUserID = cboPMUserLookup.UserID
            End If

            m_lReturn = ValidateValues()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SP070898 - the access level should be saved

            m_lReturn = m_oDOCScan.SaveDocument(iDocNum:=iCurrentDocument, sDocName:=sDocumentName, vPageSize:=g_vCurrentPageInfo, dExpiryDate:=dExpiryDate, dDocDate:=dDocDate, vKeywordID:=vKeywords, vAnnotation:=vAnnotations, sPageType:=SCANNEDPAGETYPE, sDocType:=SCANNEDDOCTYPE, sPassword:=sCurrentPassword, iAccessLevel:=CInt(cboAccessLevel.Text), lFolderNum:=lFolderNumber, sScanUser:=sCurrentUserName, sCustomer:=m_sCustomer, dtTaskDueDate:=m_dtTaskDueDate, lPMUserGroup:=m_lPMUserGroup, lUserID:=m_lUserID, sDescription:=sDocumentName, lTaskStatus:=chkEnableTask.CheckState, iUrgent:=m_iUrgent, dtDateCreated:=DateTime.Now, lCreatedByID:=g_oObjectManager.UserID)

            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                g_bDocumentSaved = False
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to save document.", vApp:=ACApp, vClass:="frmInterface", vMethod:="SaveDocument", excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetDocNameHistory() As Integer

        ' from misc class

        Dim result As Integer = 0
        Dim vDocNames As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the document names

            m_lReturn = m_oDOCScan.GetDocNames(vDocNames:=vDocNames)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' CTAF 20030806
            ' Clear the document names before populating the list
            ' PN 5789
            cboDocumentName.Items.Clear()

            ' Check there are some document names
            If Information.IsArray(vDocNames) Then

                For iLoop1 As Integer = 0 To vDocNames.GetUpperBound(1)

                    cboDocumentName.Items.Add(CStr(vDocNames(0, iLoop1)))
                Next iLoop1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get document name history", vApp:=ACApp, vClass:="frmInterface", vMethod:="GetDocNameHistory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *********************************************************************************
    '
    ' Function: ValidateDate
    '
    ' Desc:     Validates the date in the passed text box.
    '           Fills bValid with either True or False, and displays a message
    '           that 'sField' is invalid if needed.
    '
    ' *********************************************************************************
    Private Function ValidateDate(ByRef ctlText As TextBox, ByRef sField As String, ByRef bValid As Boolean) As Integer

        Dim result As Integer = 0
        Dim sDate, sMessage As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sDate = ctlText.Text

            ' Check the validity of the date
            bValid = Not (Not Information.IsDate(sDate))

            ' If its not valid, then
            If Not bValid Then
                ' Set to the 2nd tab
                SSTabHelper.SetSelectedIndex(tabMain, 1)
                ' Set focus to the control
                ctlText.Focus()
                ' Display a message
                sMessage = "Invalid date for the " & sField & " field"
                MessageBox.Show(sMessage, "Date", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get document name history", vApp:=ACApp, vClass:="frmInterface", vMethod:="ValidateDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    Private Sub txtDelay_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDelay.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Try


            'SP030500 - validate the wait interval
            If KeyAscii = CInt(Keys.Back) Then
                If KeyAscii = 0 Then
                    eventArgs.Handled = True
                End If
                Exit Sub
            End If

            If Strings.Chr(KeyAscii).ToString() = "." Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtDelay.Text & ".", NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    KeyAscii = 0
                End If
                If KeyAscii = 0 Then
                    eventArgs.Handled = True
                End If
                Exit Sub
            End If

            If (KeyAscii < Keys.D0) Or (KeyAscii > Keys.D9) Then
                KeyAscii = 0
                If KeyAscii = 0 Then
                    eventArgs.Handled = True
                End If
                Exit Sub
            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try



        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to validate keypress.", vApp:=ACApp, vClass:="frmInterface", vMethod:="txtDelay_KeyPress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        Exit Sub

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    Private Sub txtDelay_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDelay.Leave

        Try

            If (txtDelay.Text.Trim() = "") Or (CInt(txtDelay.Text.Trim()) = 0) Then
                txtDelay.Text = CStr(3)
            End If

            'SP030500 - Changed to save change to registry
            With g_AdvOptions.DisplayTime
                .bChanged = True
                .sValue = txtDelay.Text
                m_lReturn = SetRegistryValue(sSubKey:=.sSubKey, sKey:=.sKey, sValue:=.sValue, iLocation:=.iLocation)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    .bChanged = False
                    Exit Sub
                End If
            End With

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:="frmInterface", vMethod:="txtDelay_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub


    Private Sub txtDocumentDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDocumentDate.Leave

        Dim bValid As Boolean

        ' Only validate it if the user hasnt pressed "Reset"


        If ActiveControl.Name <> "cmdDocReset" Then
            ' Validate the document date
            m_lReturn = ValidateDate(ctlText:=txtDocumentDate, sField:="'Document Date'", bValid:=bValid)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        End If

    End Sub

    Private Sub txtExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpiryDate.Leave

        Dim bValid As Boolean

        ' Only validate if the user hasnt pressed "Reset"


        If ActiveControl.Name <> "cmdExpiryReset" Then
            ' Validate the expiry date
            m_lReturn = ValidateDate(ctlText:=txtExpiryDate, sField:="'Expiry Date'", bValid:=bValid)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        End If

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (UpDown1_DownClick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub UpDown1_DownClick()
    '
    '
    'Dim iSecDelay As Double = Conversion.Val(txtDelay.Text)
    '
    'If iSecDelay > 0 Then
    'iSecDelay -= 0.5
    'End If
    '
    'txtDelay.Text = StringsHelper.Format(Conversion.Str(iSecDelay).Trim(), "###0.0")
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (UpDown1_UpClick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub UpDown1_UpClick()
    '
    '
    'Dim iSecDelay As Double = Conversion.Val(txtDelay.Text)
    '
    'If iSecDelay < 15 Then
    'iSecDelay += 0.5
    'End If
    '
    'txtDelay.Text = StringsHelper.Format(Conversion.Str(iSecDelay).Trim(), "###0.0")
    '
    'End Sub

    ' ******************************************************************************
    '
    ' Function   : GetDisplayTimer
    '
    ' Description: Returns the number of seconds the user wants to display the
    '              scanned image for.
    '
    ' ******************************************************************************

    Private Function GetDisplayTimer(ByRef lTimeVal As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the time value, and convert from seconds to milliseconds
            lTimeVal = (CInt(txtDelay.Text) * 1000)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get display timer value.", vApp:=ACApp, vClass:="frmInterface", vMethod:="GetDisplayTimer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' JH021198 copied structure from iDOCOptions
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim iList As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Set the default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Load settings from the registry
            m_lReturn = LoadSettings()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the configuration settings
            With Me

                'Advanced options
                If g_AdvOptions.AutoDocName.sValue = "Y" Then
                    .optAutomatic.Checked = True
                    .optManual.Checked = False
                    cboDocumentName.Enabled = False
                    cboDocumentName.Text = "Document" & Conversion.Str(g_iDocNum).Trim()
                    'optAutomatic_CheckedChanged(optAutomatic, New EventArgs())
                Else
                    .optAutomatic.Checked = False
                    .optManual.Checked = True
                    cboDocumentName.Enabled = True
                    'optAutomatic_CheckedChanged(optManual, New EventArgs())
                End If

                cboDisplay.Items.Clear()
                cboDisplay.Items.Add(PMDOCDISPLAYYES)
                cboDisplay.Items.Add(PMDOCDISPLAYYESTIMED)
                cboDisplay.Items.Add(PMDOCDISPLAYNO)

                m_lReturn = SetComboText(cboDisplay, g_AdvOptions.Display.sValue, iList)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'make sure something is selected!
                If iList = -1 Then iList = 0
                .cboDisplay.SelectedIndex = iList

                .txtDelay.Text = g_AdvOptions.DisplayTime.sValue

                bInitialising = True
                If g_AdvOptions.MultiTiff.sValue = "Y" Then
                    .optMultiYes.Checked = True
                    .optMultiNo.Checked = False
                Else
                    .optMultiYes.Checked = False
                    .optMultiNo.Checked = True
                End If
                bInitialising = False

                SSTabHelper.SetSelectedIndex(.tabMain, Math.Floor(CDbl(g_AdvOptions.TabSelected.sValue)))

            End With

            g_AdvOptions.AutoDocName.bChanged = False
            g_AdvOptions.Display.bChanged = False
            g_AdvOptions.DisplayTime.bChanged = False
            g_AdvOptions.MultiTiff.bChanged = False
            g_AdvOptions.TabSelected.bChanged = False

            g_SiriusOptions.TaskEnabled.bChanged = False
            chkEnableTask.CheckState = Math.Floor(CDbl(g_SiriusOptions.TaskEnabled.sValue))

            'Set Task Due Date to today
            txtTaskDD.Text = DateTime.Now.ToString("dd MMMM yyyy")

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function PopulateScanner() As Integer

        Dim result As Integer = 0
        Dim DsList As New FixedLengthString(1024)


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            dsInfo = IkDisp1.Scan.GetDataSources()
            For index As Integer = 0 To dsInfo.Length - 1
                cboScanner.Items.Add(dsInfo(0).ProductName)
            Next

            'm_lReturn = IkScan1.ScanList()
            'IkDisp1.Scan.GetCurrentDataSource()


            'DsList.Value = IkDisp1.Scan.DataSourceName
            'If Not m_lReturn Or DsList.Value.Length = 0 Then
            '    Return result
            'End If

            ''Default Scanner (the default scanner is the scanner listed before the first comma)
            'i = (DsList.Value.IndexOf(","c) + 1)
            'If i <= 1 Then
            '    DefDsName = ""
            'Else
            '    DefDsName = DsList.Value.Substring(0, i - 1)
            '    DsList.Value = Mid(DsList.Value, i + 1)
            'End If

            ''scan device list
            'n = 0
            'Do While True
            '    i = (DsList.Value.IndexOf(","c) + 1)
            '    If i <= 1 Then Exit Do
            '    DsName = DsList.Value.Substring(0, i - 1)
            '    DsList.Value = Mid(DsList.Value, i + 1)
            '    VB6.SetItemString(cboScanner, n, DsName)
            '    n += 1
            'Loop

            ' cboScanner.Text = DefDsName 'default scanner
            cboScanner.SelectedIndex = 0
            IkScan1 = New Newtone.ImageKit.Scan()
            IkScan1.DataSourceName = cboScanner.SelectedItem.ToString()

            Return result

        Catch excep As System.Exception


            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateScanner Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateScanner", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub IkDisp1_AfterScan(ByVal sender As System.Object, ByVal e As Newtone.ImageKit.AfterScanEventArgs) Handles IkDisp1.AfterScan
        Static lFileSize As Integer
        Static lBlankcount As Integer

        'If we are in this event then the scan was not cancelled.
        m_bCancelledScan = False

        If IkDisp1.Handle <> 0 Then
            '   IkCommon1.ImgHandle = IkDisp1.ImgHandle
            '  IkCommon1.FreeMemory()
        End If
        'TODO
        'IkCommon1.ImgHandle = EventArgs.dibHandle
        'IkDisp1.ImgHandle = IkCommon1.CopyImage()

        'IkDisp1.Display (ikScale)
        IkDisp1.Display(Newtone.ImageKit.Win.DisplayMode.Stretch)
        'TODO
        '  IkFile1.ImgHandle = IkDisp1.ImgHandle

        Dim bSaveFile As Boolean = True

        If m_bMultiDocMode Then

            'Get the scan size
            IkFile1.TiffAppend = False
            IkFile1.FileName = m_sBlankFileName
            bSaveFile = IkFile1.SaveImageToFile(Newtone.ImageKit.SaveFileType.SaveTIFFLZW, IkDisp1.Image)
            'IkFile1.GetImageFileType()
            lFileSize = IkFile1.FileSize
            IkFile1.TiffAppend = True

            If m_bBlankScan Then
                bSaveFile = False
                If lBlankcount = 1 Then
                    'Get the size of the blank file. Add a bit to it as this can vary with each blank page.
                    m_lBlankSize = CInt(Math.Floor(lFileSize * 1.05))
                    m_bBlankScan = False
                    lBlankcount = 0
                Else
                    lBlankcount = 1
                End If
            ElseIf lFileSize < m_lBlankSize Then
                bSaveFile = False
                If lBlankcount = 1 Then
                    lBlankcount = 0
                    m_lReturn = PrepareNextDocument(False)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If
                Else
                    lBlankcount = 1
                End If
            ElseIf lBlankcount = 1 Then
                'Single blank documents are ok.
                lBlankcount = 0
            End If
        End If

        If bSaveFile Then

            g_bDocumentSaved = False

            If Not optMultiYes.Checked Then

                g_sCurrentFilename = g_sCurrentPath

                If Not g_sCurrentPath.EndsWith("\") Then
                    g_sCurrentFilename = g_sCurrentFilename & "\"
                End If

                g_sCurrentFilename = g_sCurrentFilename & Conversion.Str(g_lCurrentPage).Trim() & DEFAULTTIFEXTENSION

                g_lCurrentPage += 1
            Else
                g_lCurrentPage = 2
            End If

            IkFile1.FileName = g_sCurrentFilename
            bSaveFile = IkFile1.SaveImageToFile(Newtone.ImageKit.SaveFileType.SaveTIFFLZW, IkDisp1.Image)

            'TODO:
            'IkFile1.GetImageFileType()
            lFileSize = IkFile1.FileSize

            ReDim Preserve g_vCurrentPageInfo(g_lCurrentPage - 1)

            g_vCurrentPageInfo(g_lCurrentPage - 1) = lFileSize

            g_lPagesScanned += 1
            g_lPagesScannedBatch += 1

        End If
    End Sub


End Class
