Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ********************************************************************************
    '
    ' Name : frmInterface
    '
    ' Desc : Scan Interface form
    '
    ' Edit History:
    '
    ' SP070898 - Remove exit warning
    '
    ' ********************************************************************************


    ' {* Private Constants - Begin *}

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    Private Const sglSplitLimit As Integer = 2300

    ' {* End *}

    ' {* Private Variables - Begin *}

    ' Return value
    Private m_lReturn As Integer

    Private bOverrideUnload As Boolean

    Private bTreeExpanded As Boolean

    ' Properties for the split bar
    Private mbMoving As Boolean
    ' {* End *}

    ' **********************************************************************
    '
    ' Function : GetScanSettings() as Long
    '
    ' Purpose  : Gets the scan directory from registry
    '
    ' **********************************************************************

    Private Function GetScanSettings() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the registry setting, set it if its not there.
            m_lReturn = GetDOCRegSettings(vScanDirectory:=sScanDirectory, hWndParent:=Me.Handle.ToInt32())
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' log an error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get scan settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScanSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' **********************************************************************
    '
    ' Function : ResizeControls() as Long
    '
    ' Purpose  : Resizes the controls when the form is resized.
    '
    ' **********************************************************************

    Private Function ResizeControls() As Integer

        Dim result As Integer = 0
        Dim iTop, iBottom As Integer

        ' temporary variable used so that objects dont need to be
        ' read more than once.
        Dim lTemp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If the form's minimised then exit the resize sub
            If Me.WindowState = FormWindowState.Minimized Then
                Return result
            End If

            lTemp = CInt(VB6.PixelsToTwipsX(Me.Width) - sglSplitLimit)

            If VB6.PixelsToTwipsX(picSplitBar.Left) > lTemp Then
                picSplitBar.Left = VB6.TwipsToPixelsX(lTemp)
            End If

            iTop = VB6.PixelsToTwipsY(tlbMain.Height)
            iBottom = VB6.PixelsToTwipsY(stbMain.Height)

            tvwDocuments.Top = VB6.TwipsToPixelsY(iTop)
            tvwDocuments.Left = 0
            tvwDocuments.Width = picSplitBar.Left
            lTemp = CInt(VB6.PixelsToTwipsY(Me.ClientRectangle.Height) - (iTop + iBottom))
            tvwDocuments.Height = VB6.TwipsToPixelsY(lTemp)

            imgSplitBar.Top = VB6.TwipsToPixelsY(iTop)
            imgSplitBar.Height = VB6.TwipsToPixelsY(lTemp)
            lTemp = CInt(VB6.PixelsToTwipsX(tvwDocuments.Left) + VB6.PixelsToTwipsX(tvwDocuments.Width))
            imgSplitBar.Left = VB6.TwipsToPixelsX(lTemp)

            picSplitBar.Left = VB6.TwipsToPixelsX(lTemp)
            picSplitBar.Top = imgSplitBar.Top
            picSplitBar.Height = imgSplitBar.Height

            '    kvDocument.Top = iTop
            'kvDocument.Left = imgSplitBar.Left + imgSplitBar.Width
            'kvDocument.Height = tvwDocuments.Height

            'kvDocument.Width = Me.Width - imgSplitBar.Width - tvwDocuments.Width

            picBorder.Top = VB6.TwipsToPixelsY(iTop)
            picBorder.Left = imgSplitBar.Left + imgSplitBar.Width
            picBorder.Height = tvwDocuments.Height
            picBorder.Width = Me.Width - imgSplitBar.Width - tvwDocuments.Width

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' log an error
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to resize controls for form.", vApp:=ACApp, vClass:=ACClass, vMethod:="ResizeControls", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ViewDocument
    '
    ' Description: Call the Document Viewer
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ViewDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub ViewDocument()
    '
    'Dim lDocNum As Integer
    'Dim sNodeKeys() As DOCConst.DOCNodes = Nothing
    'Dim vPageArray As Object
    'Dim sFolderName, sTmp As String
    'Dim bZipped As Boolean
    'Dim sParents, sFilename As String
    'Dim oZipper As New bSIRZipper.Zipper
    '
    'Try 
    '
    'call the doc viewer

    'm_lReturn = g_oViewer.ViewDocument(v_sDocumentKey:=sNodeKeys(0).Key, v_sDocumentName:=sNodeKeys(0).Text, v_sParents:=sParents, v_vFileArray:=vPageArray, v_bZipped:=bZipped)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'if no nodes selected, returned, just show the viewer
    'If sNodeKeys(sNodeKeys.GetLowerBound(0)).Key = "" Then
    '
    'call the doc viewer
    'm_lReturn = g_oViewer.ViewDocument(v_sDocumentKey:="", v_sDocumentName:="", v_sParents:="", v_vFileArray:="", v_bZipped:=False, v_bshowonly:=True)
    '
    'Exit Sub
    '
    'End If
    '
    '
    'Go thru all select docs, calling the viewer for each
    'For	Each sNodeKeys_item As DOCConst.DOCNodes In sNodeKeys
    '
    'If sNodeKeys_item.Key.Substring(0, 1) = "D" Then
    '
    'get the doc num
    'm_lReturn = ExtractNumFromKey(sNodeKeys_item.Key, lDocNum)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'get the page file paths
    ' m_lReturn& = g_oBusiness.GetPageList(lDocNum:=lDocNum&, _
    'vPageArray:=vPageArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cannot view '" & sNodeKeys_item.Text &  _
    '           "'. Failed to get pages.", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Else
    ' Get the first filename


    'sFilename = CStr(vPageArray.GetValue(vPageArray.GetLowerBound(0)))
    '
    ' Check to see if zip file...
    ' m_lReturn = m_oZipper.ValidZIPFile(szTestFile:=sFilename, bZipFile:=bZipped)
    '
    'If Not m_lReturn Then
    'error - assume unzipped
    'bZipped = False
    'End If
    '
    'call the doc viewer

    'm_lReturn = g_oViewer.ViewDocument(v_sDocumentKey:=sNodeKeys_item.Key, v_sDocumentName:=sNodeKeys_item.Text, v_sParents:=sParents, v_vFileArray:=vPageArray, v_bZipped:=bZipped)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'End If
    '
    'End If
    '
    'Next sNodeKeys_item
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ViewDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    '
    'End Sub

    Private Sub Form_Initialize_Renamed()

        bTreeExpanded = False

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            ' set the split bar values
            imgSplitBar.Width = picSplitBar.Width

            bOverrideUnload = False

            ' Resize the controls for the size of the form
            m_lReturn = ResizeControls()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Fill the tree data with info from the database
            m_lReturn = FillTreeData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Get the scan settings (document scan folder etc...)
            m_lReturn = GetScanSettings()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            Me.tlbMain.Items.Item(4).Enabled = (Not bIsStandAlone)

            ' if no documents, then no commit button
            If Me.tvwDocuments.Nodes.Count = 0 Then
                Me.tlbMain.Items.Item(4).Enabled = False
            End If

        Catch excep As System.Exception



            ' log an error
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to intialise main form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Try

            'If (bOverrideUnload = False) Then

            'SP070898 - remove warning
            'lVal = MsgBox("Are you sure you wish to exit View Batch?", vbInformation + vbYesNo + vbDefaultButton2, "Exit")

            'If (lVal = vbYes) Then
            '    Cancel = False
            'Else
            '    Cancel = True
            'End If

            'Else

            Cancel = False

            'End If

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to unload form correctly.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            If Me.WindowState <> FormWindowState.Minimized Then

                If VB6.PixelsToTwipsX(Me.Width) < 5500 Then
                    Me.Width = VB6.TwipsToPixelsX(5500)
                    Exit Sub
                End If

                If VB6.PixelsToTwipsY(Me.Height) < 2000 Then
                    Me.Height = VB6.TwipsToPixelsY(2000)
                    Exit Sub
                End If

                ResizeControls()

            End If

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to resize form correctly.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Resize", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub imgSplitBar_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitBar.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Try

            With imgSplitBar
                picSplitBar.SetBounds(.Left, .Top, VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(.Width) \ 2), VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(.Height) - 20))
            End With

            picSplitBar.Visible = True

            mbMoving = True

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process split bar movement.", vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitBar_MouseDown", excep:=excep)

        End Try

    End Sub

    Private Sub imgSplitBar_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitBar.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Dim sglPos As Single

        Try

            If mbMoving Then

                sglPos = X + VB6.PixelsToTwipsX(imgSplitBar.Left)

                If sglPos < sglSplitLimit Then
                    picSplitBar.Left = VB6.TwipsToPixelsX(sglSplitLimit)
                ElseIf sglPos > VB6.PixelsToTwipsX(Me.Width) - sglSplitLimit Then
                    picSplitBar.Left = Me.Width - VB6.TwipsToPixelsX(sglSplitLimit)
                Else
                    picSplitBar.Left = VB6.TwipsToPixelsX(sglPos)
                End If

            End If

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process split bar movement.", vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitBar_MouseMove", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub imgSplitBar_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles imgSplitBar.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim X As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim Y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        Try

            If mbMoving Then

                mbMoving = False
                picSplitBar.Visible = False

                m_lReturn = ResizeControls()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

            End If

        Catch excep As System.Exception



            ' log an error
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process MouseUp", vApp:=ACApp, vClass:=ACClass, vMethod:="imgSplitBar_MouseUp", excep:=excep)

        End Try

    End Sub

    '***********************************************************
    '
    ' Function : Commit() as Long
    '
    ' Purpose : Create an instance of the commit object and
    '           let that commit the scanned docs to the main
    '           database.
    '
    '***********************************************************

    Private Function Commit() As Integer
        Dim result As Integer = 0

        ' Instance of the Commit interface class

        Dim oCommit As iDOCCommit.Interface_Renamed = New iDOCCommit.Interface_Renamed()
        Dim lTemp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object
            'Set oCommit = CreateObject("iDOCCommit.Interface")
            'Set oCommit = New iDOCCommit.Interface

            ' Initialise the object, no parameters needed

            m_lReturn = CType(oCommit, SSP.S4I.Interfaces.ILocalInterface).Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Commit the documents to the server

            m_lReturn = oCommit.Start()


            oCommit.Dispose()
            oCommit = Nothing
            ' Refresh the tree
            m_lReturn = FillTreeData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' update the status bar
            m_lReturn = UpdateStatusBar()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Me.tvwDocuments.Nodes.Count = 0 Then
                bOverrideUnload = True
                mnuExit_Click(mnuExit, New EventArgs())
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' log an error
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to commit documents to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Commit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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
    Public Sub mnuDocumentCommit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentCommit.Click

        m_lReturn = Commit()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' error i suppose
        End If

    End Sub

    Public Sub mnuDocumentDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentDelete.Click

        m_lReturn = DeleteDocument()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    Public Sub mnuDocumentDeleteAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuDocumentDeleteAll.Click

        m_lReturn = DeleteAll()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    Public Sub mnuExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExit.Click

        Try

            Me.Close()

            Me.Finalize()

        Catch excep As System.Exception



            ' log an error
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to unload form", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuExit_Click", excep:=excep)

        End Try

    End Sub

    Public Sub mnuFileExpandAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExpandAll.Click

        Try

            m_lReturn = ExpandAllNodes()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' log an error
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to expand all nodes", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileExpandAll_Click", excep:=excep)

        End Try

    End Sub

    Public Sub mnuFileRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileRefresh.Click

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = FillTreeData()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' log an error
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to fill tree data", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileRefresh_Click", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            m_lReturn = UpdateStatusBar()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' log an error
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to update status bar", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileRefresh_Click", excep:=New Exception(Information.Err().Description))
                Exit Sub
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' log an error
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to refresh tree data.", vApp:=ACApp, vClass:=ACClass, vMethod:="mnuFileRefresh_Click", excep:=excep)

        End Try

    End Sub

    Public Sub mnuViewPage_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewPage.Click

        tvwDocuments_DoubleClick(tvwDocuments, New EventArgs())

    End Sub

    Private Sub tlbMain_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _tlbMain_Button1.Click, _tlbMain_Button2.Click, _tlbMain_Button3.Click, _tlbMain_Button4.Click, _tlbMain_Button5.Click, _tlbMain_Button6.Click, _tlbMain_Button7.Click, _tlbMain_Button8.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Try


            Select Case Button.Name
                Case "expandall"

                    ' Check to see if the nodes need expanding or retracting
                    If Not bTreeExpanded Then
                        ' Expanding
                        m_lReturn = ExpandAllNodes()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                        ' Retract them next time
                        bTreeExpanded = True
                    Else
                        ' Retracting
                        m_lReturn = RetractAllNodes()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Exit Sub
                        End If
                        ' Expand them next time
                        bTreeExpanded = False
                    End If

                Case "clear"

                    ' Delete ALL documents from the scan database
                    m_lReturn = DeleteAll()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case "delete"

                    ' Delete the currently selected document
                    m_lReturn = DeleteDocument()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case "commit"

                    ' Commit the documents to the server
                    m_lReturn = Commit()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                Case "viewpage"

                    ' View the current page
                    tvwDocuments_DoubleClick(tvwDocuments, New EventArgs())

            End Select

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="tlbMain_ButtonClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    '***********************************************************
    '
    ' Function : ExpandAllNodes() As Long
    '
    ' Purpose : Expands all the nodes so that all nodes are
    '           visible
    '
    '***********************************************************

    Private Function ExpandAllNodes() As Integer

        Dim result As Integer = 0
        Dim iNodes As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the node count
            iNodes = tvwDocuments.Nodes.Count

            ' expand each node in the tree
            If iNodes > 0 Then
                For iLoop1 As Integer = 1 To iNodes
                    tvwDocuments.Nodes.Item(iLoop1 - 1).Expand()
                Next iLoop1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to expand all nodes.", vApp:=ACApp, vClass:=ACClass, vMethod:="ExpandAllNodes", excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************
    '
    ' Function : RetractAllNodes() As Long
    '
    ' Purpose : Retracts all the nodes so that the nodes are not
    '           visible. Opposite function of ExpandAllNodes
    '
    '***********************************************************

    Private Function RetractAllNodes() As Integer

        Dim result As Integer = 0
        Dim iNodes As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the node count
            iNodes = tvwDocuments.Nodes.Count

            ' expand each node in the tree
            If iNodes > 0 Then
                For iLoop1 As Integer = 1 To iNodes
                    tvwDocuments.Nodes.Item(iLoop1 - 1).Collapse()
                Next iLoop1
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retract all nodes.", vApp:=ACApp, vClass:=ACClass, vMethod:="RetractAllNodes", excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************
    '
    ' Function : FillTreeData() as Long
    '
    ' Purpose : Creates the tree (left pane) from the information
    '           in the local database (.mdb)
    '
    '***********************************************************

    Private Function FillTreeData() As Integer

        ' declare a few variables for use
        '    Dim oDOCScan As New bDOCScan.Form

        Dim result As Integer = 0
        Dim nodX As TreeNode

        Dim lFolderNum, lCurrentDocument As Integer

        Dim lPages As Integer

        Dim sName, sText, sRel, sPage, sDoc, sParent, sKey As String

        Dim bFakeScanFolder As Boolean

        Dim vFolders, vDocNames As Object

        Dim bExists, bNeedRoot As Boolean

        Dim lDocOffset As Integer

        result = gPMConstants.PMEReturnCode.PMTrue




        bNeedRoot = False

        ' Clear the tree, ready for use
        tvwDocuments.Nodes.Clear()


        m_lReturn = g_oViewBatch.OpenCloseDatabase()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the number of documents

        m_lReturn = g_oViewBatch.GetNextDocument(lDocNum:=lNumberDocuments)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Document Names

        m_lReturn = g_oViewBatch.GetDocumentNames(vDocNames:=vDocNames)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add scanned root node
        nodX = tvwDocuments.Nodes.Add("SCANNEDROOT", DOCDefaultScanFolder)
        nodX.Collapse()
        nodX.ImageKey = "closed"
        nodX.Tag = "ROOT"

        lCurrentDocument = 0

        lDocOffset = 0

        ' Now add the document + folder nodes
        'For lLoop1 As Integer = 1 To lNumberDocuments
        For lLoop1 As Integer = 0 To lNumberDocuments


            m_lReturn = g_oViewBatch.DoesDocumentExist(lDocNum:=lLoop1, bExists:=bExists)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bExists Then

                sDoc = CStr(lLoop1)
                sName = "DOC" & sDoc

                sText = CStr(vDocNames(0, lCurrentDocument))

                ' Get the folder number for the current document

                m_lReturn = g_oViewBatch.GetDocFolderNumber(lDocNum:=lLoop1, lFolderNum:=lFolderNum)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If no foldernum, then add to scanned root
                If lFolderNum = 0 Then

                    Try

                        ' add it
                        nodX = tvwDocuments.Nodes.Find("SCANNEDROOT", True)(0).Nodes.Add(sName, sText, "document")

                    Catch
                    End Try




                    nodX.Tag = sName

                    bNeedRoot = True

                Else

                    ' Only add if not running standalone
                    If Not bIsStandAlone Then

                        ' Else, get the folder tree for the current document

                        m_lReturn = g_oViewBatch.GetFolderTree(lFolderNum:=lFolderNum, vFolderArray:=vFolders)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Get the text and create a unique key


                        sText = CStr(vFolders(1, vFolders.GetUpperBound(1)))


                        sKey = "F" & CStr(vFolders(0, vFolders.GetUpperBound(1)))

                        Try

                            nodX = tvwDocuments.Nodes.Add(sKey, sText, "closed")

                        Catch
                        End Try




                        ' Build

                        If vFolders.GetUpperBound(1) > 0 Then


                            For iLoop2 As Integer = vFolders.GetUpperBound(1) - 1 To 0 Step -1

                                'sKey = "DOC" & CStr(iLoop1)

                                sParent = "F" & CStr(vFolders(0, iLoop2 + 1))

                                sKey = "F" & CStr(vFolders(0, iLoop2))

                                sText = CStr(vFolders(1, iLoop2))

                                ' this is ok! :)
                                Try

                                    nodX = tvwDocuments.Nodes.Find(sParent, True)(0).Nodes.Add(sKey, sText, "closed")

                                Catch
                                End Try

                                ' ok, normal now



                            Next iLoop2


                            sKey = "F" & CStr(vFolders(0, 0))

                            ' Now add the document folder
                            sName = sKey

                            sText = CStr(vDocNames(0, lLoop1 - 1 - lDocOffset))
                            sKey = "DOC" & lLoop1

                            'On Error Resume Next

                            nodX = tvwDocuments.Nodes.Find(sName, True)(0).Nodes.Add(sKey, sText, "document")

                            ' ok, back to normal now



                            nodX.Tag = "DOC" & lLoop1

                        Else

                            ' Now add the document folder
                            sName = sKey

                            sText = CStr(vDocNames(0, lLoop1 - 1 - lDocOffset))
                            sKey = "DOC" & lLoop1

                            '                        sKey = "F" & CStr(vFolders(0, 0))
                            '                        sText = vFolders(1, 0)
                            '
                            If Not bFakeScanFolder Then
                                nodX = tvwDocuments.Nodes.Find(sName, True)(0).Nodes.Add(sKey, sText, "document")
                            Else
                                nodX = tvwDocuments.Nodes.Find("SCANNEDROOT", True)(0).Nodes.Add(sKey, sText, "document")
                            End If

                            nodX.Tag = nodX.Name

                        End If

                        nodX.Name = "DOC" & lLoop1

                    End If

                End If

                lCurrentDocument += 1

            Else

                lDocOffset += 1

                'nodX.Tag = "DOC" & CStr(lLoop1)
                'nodX.Image = "document"

            End If

        Next lLoop1

        ' Create nodes for pages
        ' select max page_num from page where doc_num = lLoop1
        ' For lLoop1 As Integer = 1 To lNumberDocuments
        For lLoop1 As Integer = 0 To lNumberDocuments

            ' Get the number of pages for the current document

            m_lReturn = g_oViewBatch.GetMaxPages(lDocNum:=lLoop1, lDocPages:=lPages)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lPages > 0 Then

                sDoc = CStr(lLoop1)
                sRel = "DOC" & sDoc

                ' Create the nodes for each page
                For iLoop2 As Integer = 1 To lPages

                    sPage = CStr(iLoop2)
                    sName = "PAGE" & sPage & ":" & sDoc
                    sText = "Page " & sPage

                    Try

                        ' Add the pages, to the relevant document
                        nodX = tvwDocuments.Nodes.Find(sRel, True)(0).Nodes.Add(sName, sText, "page")

                    Catch
                    End Try




                    ' Set the node's tag
                    nodX.Tag = "PAG" & iLoop2

                Next iLoop2

            End If

        Next lLoop1

        ' If the 0 folder wasnt used, then remove it
        If Not bNeedRoot Then
            tvwDocuments.Nodes.RemoveAt(ROOT_NODE - 1)
        End If

        ' if no documents, then disable commit button, and delete buttons
        If tvwDocuments.Nodes.Count = 0 Then

            tlbMain.Items.Item(DOC_VB_TOOLBAR_EXPANDALL).Enabled = False
            tlbMain.Items.Item(DOC_VB_TOOLBAR_DELETEALL).Enabled = False
            tlbMain.Items.Item(DOC_VB_TOOLBAR_VIEWPAGE).Enabled = False
            tlbMain.Items.Item(DOC_VB_TOOLBAR_DELETE).Enabled = False
            tlbMain.Items.Item(DOC_VB_TOOLBAR_COMMIT).Enabled = False

            ' Now sort the menu out
            mnuFileExpandAll.Enabled = False
            mnuViewPage.Enabled = False
            mnuDocumentDelete.Enabled = False
            mnuDocumentDeleteAll.Enabled = False
            mnuDocumentCommit.Enabled = False
        Else

            tlbMain.Items.Item(DOC_VB_TOOLBAR_EXPANDALL).Enabled = True
            tlbMain.Items.Item(DOC_VB_TOOLBAR_DELETEALL).Enabled = True
            tlbMain.Items.Item(DOC_VB_TOOLBAR_VIEWPAGE).Enabled = True
            tlbMain.Items.Item(DOC_VB_TOOLBAR_DELETE).Enabled = True
            tlbMain.Items.Item(DOC_VB_TOOLBAR_COMMIT).Enabled = True

            ' Now sort the menu out
            mnuFileExpandAll.Enabled = True
            mnuViewPage.Enabled = True
            mnuDocumentDelete.Enabled = True
            mnuDocumentDeleteAll.Enabled = True
            mnuDocumentCommit.Enabled = True
        End If

        If Not tlbMain.Items.Item(DOC_VB_TOOLBAR_PAGELEFT) Is Nothing Then
            tlbMain.Items.Item(DOC_VB_TOOLBAR_PAGELEFT).Enabled = False
            tlbMain.Items.Item(DOC_VB_TOOLBAR_PAGELEFT).Visible = False
        End If
        If Not tlbMain.Items.Item(DOC_VB_TOOLBAR_PAGERIGHT) Is Nothing Then
            tlbMain.Items.Item(DOC_VB_TOOLBAR_PAGERIGHT).Enabled = False
            tlbMain.Items.Item(DOC_VB_TOOLBAR_PAGERIGHT).Visible = False
        End If

        m_lReturn = UpdateStatusBar()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

Err_FillTreeData:

        result = gPMConstants.PMEReturnCode.PMError

        ' Display the error message
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get document data and fill tree view.", vApp:=ACApp, vClass:=ACClass, vMethod:="FillTreeData", excep:=New Exception(Information.Err().Description))

        Return result

    End Function

    Private Function UpdateStatusBar() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update Total Pages
            stbMain.Items.Item(0).Text = Conversion.Str(lNumberDocuments).Trim() & " document"
            If lNumberDocuments <> 1 Then
                stbMain.Items.Item(0).Text = stbMain.Items.Item(0).Text & "s"
            End If

            ' Update Page of

            ' Update Pages

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to update the status bar.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStatusBar", excep:=excep)

            Return result

        End Try
    End Function

    Private Sub tvwDocuments_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwDocuments.Click

        Dim sTemp As String = ""

        Try

            If tvwDocuments.SelectedNode Is Nothing Then
                Exit Sub
            End If

            ' Get the key
            sTemp = tvwDocuments.SelectedNode.Name

            ' Disable the view page menu option
            mnuViewPage.Enabled = False

            ' if the node is a page, then re-enable the view page menu option
            If sTemp.Length > 4 Then
                If sTemp.Substring(0, 4) = "PAGE" Then
                    mnuViewPage.Enabled = True
                End If
            End If

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get document data and fill tree view.", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwDocuments_Click", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tvwDocuments_AfterCollapse(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwDocuments.AfterCollapse
        Dim node As TreeNode = eventArgs.Node

        Try

            ' If its a root node, then change the icon to a closed folder
            Select Case node.Name
                Case "ROOT"
                    tvwDocuments.Nodes.Item(ROOT_NODE - 1).ImageKey = "closed"
            End Select

        Catch excep As System.Exception



            ' Display the error message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to collapse tree.", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwDocuments_Collapse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tvwDocuments_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwDocuments.DoubleClick

        ' get the page number from the node

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim node As New TreeNode

        Try

            If tvwDocuments.SelectedNode Is Nothing Then
                Exit Sub
            End If

            node = tvwDocuments.SelectedNode

            lReturn = CType(NodeClick(node), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get document data and fill tree view.", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwDocuments_DblClick", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub tvwDocuments_AfterExpand(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwDocuments.AfterExpand
        Dim node As TreeNode = eventArgs.Node

        Try

            ' if its the root node then set the image to an open folder
            Select Case node.Name
                Case "ROOT"
                    tvwDocuments.Nodes.Item(ROOT_NODE - 1).ImageKey = "open"
            End Select

        Catch excep As System.Exception



            ' Display the error message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to expand tree.", vApp:=ACApp, vClass:=ACClass, vMethod:="tvwDocuments_Expand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function NodeClick(ByVal node As TreeNode) As Integer

        Dim result As Integer = 0
        Dim iLoop1 As Integer
        Dim lPages As Integer

        Dim lReturn As Integer

        Dim sDocument, sText, sDirectory, sFilename As String
        Dim sPage As New StringBuilder

        Dim sByte_Renamed As New FixedLengthString(1)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sText = node.Name

            If sText.Substring(0, 1) <> "P" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sPage = New StringBuilder("")

            iLoop1 = 5

            ' Key format for a page :
            ' PAGEX:Y
            ' X = Page
            ' Y = Document

            ' Get the page number from the key
            Do
                sByte_Renamed.Value = sText.Substring(iLoop1 - 1, 1)
                If sByte_Renamed.Value = ":" Then
                    Exit Do
                End If
                sPage.Append(sByte_Renamed.Value)
                iLoop1 += 1
            Loop While iLoop1 <= sText.Length

            sDocument = ""

            iLoop1 += 1

            ' Get the document number from the key
            Do
                sByte_Renamed.Value = sText.Substring(iLoop1 - 1, 1)
                sDocument = sDocument & sByte_Renamed.Value
                iLoop1 += 1
            Loop While iLoop1 <= sText.Length

            sFilename = sPage.ToString() & ".tif"

            sDirectory = sScanDirectory & "Doc" & sDocument & "\" & sFilename

            'call the doc viewer
            'Use the one viewer for all viewing this handles all types of TIF's
            'SOB 09/06/1999 This means viewing is seperate from the tree
            Dim vFileArray As Object
            ReDim vFileArray(0)

            vFileArray(0) = sDirectory


            m_lReturn = g_oViewer.ViewTIFDocument(v_sDocumentKey:=node.Name, v_sDocumentName:=node.Text, v_sParents:="", v_vFileArray:=vFileArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            sText = "Document " & sDocument & ", Page " & sPage.ToString()
            stbMain.Items.Item(1).Text = sText


            lReturn = g_oViewBatch.GetMaxPages(lDocNum:=Conversion.Val(sDocument), lDocPages:=lPages)

            sText = "Page " & sPage.ToString() & " of " & CStr(lPages)

            stbMain.Items.Item(2).Text = sText

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to process double click on node.", vApp:=ACApp, vClass:=ACClass, vMethod:="NodeClick", excep:=excep)

            Return result

        End Try
    End Function

    ' **********************************************************************
    '
    ' Function : DeleteDoc(iDocNum as Integer) as Long
    '
    ' Purpose  : Removes a single document from the local database and
    '            removes the files scanned
    '
    ' **********************************************************************

    Private Function DeleteDoc(ByRef lDocNum As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' call business

            m_lReturn = g_oViewBatch.DeleteDocument(lDocNum:=lDocNum, sScanDirectory:=sScanDirectory)

            ' Dont update the tree, that is the responsibility of the calling
            ' function, incase we are deleting many documents

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to delete the document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDoc", excep:=excep)

            Return result

        End Try
    End Function

    ' **********************************************************************
    '
    ' Function : DeleteDocument() as Long
    '
    ' Purpose  : Removes the currently selected document from the local
    '            scan database, and the files from the drive.
    '
    ' **********************************************************************

    Private Function DeleteDocument() As Integer

        Dim result As Integer = 0
        Dim lDocNum As Integer
        Dim sTag, sQuery As String

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Get current document Node


            sTag = Convert.ToString(tvwDocuments.SelectedNode.Tag)


            Select Case sTag.Substring(0, 3)
                Case "DOC"
                    ' right place

                Case "PAG"
                    ' we want the parent
                    tvwDocuments.SelectedNode = tvwDocuments.SelectedNode.Parent

                    sTag = Convert.ToString(tvwDocuments.SelectedNode.Tag)

                Case Else
                    ' no, not this one :)
                    ' Its the root , or some other folder. exit
                    sQuery = "Unable to delete this folder."
                    MessageBox.Show(sQuery, "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result

            End Select

            ' Now get the document number.
            lDocNum = CInt(sTag.Substring(sTag.Length - (sTag.Length - 3)))

            sQuery = "Are you sure you wish to delete """ & tvwDocuments.SelectedNode.Text & """ ?"
            m_lReturn = MessageBox.Show(sQuery, "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then

                ' Call DeleteDoc with current doc number
                m_lReturn = DeleteDoc(lDocNum:=lDocNum)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Refresh the tree
                m_lReturn = FillTreeData()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Display the error message
                    LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to refresh the tree view.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", excep:=New Exception(Information.Err().Description))

                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to delete the selected document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", excep:=excep)

            Return result

        End Try
    End Function

    ' **********************************************************************
    '
    ' Function : DeleteAllDocuments() as Long
    '
    ' Purpose  : Removes all documents from the local
    '            scan database, and the files from the drive.
    '
    ' **********************************************************************

    Private Function DeleteAllDocuments() As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'm_lReturn = g_oViewBatch.GetNextDocument(lDocNum:=lNumDocs)

            ' delete each document
            For lLoop1 As Integer = 1 To lNumberDocuments
                m_lReturn = DeleteDoc(lDocNum:=lLoop1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                End If
            Next lLoop1

            ' Update the tree
            m_lReturn = FillTreeData()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to delete the selected document.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllDocuments", excep:=excep)

            Return result

        End Try
    End Function

    ' **********************************************************************
    '
    ' Function : DeleteAll() as Long
    '
    ' Purpose  : Asks for confirmation before calling DeleteAllDocuments
    '
    ' **********************************************************************

    Private Function DeleteAll() As Integer

        Dim sMessage As String = ""

        Try

            sMessage = "Are you sure you wish to delete all documents?" & _
                       Environment.NewLine & Environment.NewLine & _
                       "NOTE: YOU WILL NOT BE ABLE TO UN-DELETE THESE DOCUMENTS."
            m_lReturn = MessageBox.Show(sMessage, "Delete All", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
                m_lReturn = DeleteAllDocuments()
            End If

        Catch excep As System.Exception



            ' Display the error message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to delete all documents.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAll", excep:=excep)

        End Try

    End Function
End Class
