Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    Dim m_lReturn As Integer

    Dim m_vProduct(,) As Object ' product descriptions and codes
    Dim m_vUpdate(,) As Object ' update details from PMProduct_Update_History
    Dim m_vVersion As Array ' client version for each product


    Dim m_oBusiness As bPMProductUpdateHistory.Business
    Dim m_oObjectManager As bObjectManager.ObjectManager

    Public Function Start() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try


            ' we need Object Manager to get the business object
            m_oObjectManager = New bObjectManager.ObjectManager()

            If m_oObjectManager Is Nothing Then
                MessageBox.Show("Failed to create Object Manager", "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' initialise Object Manager
            m_lReturn = m_oObjectManager.Initialise("ProductUpdateHistory")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to get initialise Object Manager", "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            ' get the ProductUpdateHistory business object
            Dim temp_m_oBusiness As Object
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bPMProductUpdateHistory.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            m_oObjectManager.Dispose()

            m_oObjectManager = Nothing
            ' call function that gets update data from database and populates form
            m_lReturn = PopulateForm()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve information from database", "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Error)


                m_oBusiness.Dispose()
                m_oBusiness = Nothing

                Return result
            End If

            ' show the form
            Me.ShowDialog()

            ' close the business object

            m_oBusiness.Dispose()

            m_oBusiness = Nothing


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' get update data from business object and populate form controls
    Private Function PopulateForm() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get product descriptions and codes for treeview

            m_lReturn = m_oBusiness.GetProducts(m_vProduct)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vProduct) Then
                Return result
            End If

            ' get updates for each product

            m_lReturn = m_oBusiness.GetProductUpdates(m_vUpdate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vUpdate) Then
                Return result
            End If

            ' get installed client version for each product
            m_lReturn = GetClientVersions()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' set initial view displays
            ResetViews()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' gets client versions for each product
    Private Function GetClientVersions() As Integer

        Dim result As Integer = 0
        Dim iBtm, iTop As Integer
        Dim lProductCode As Integer
        Dim sVersion As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            iBtm = m_vProduct.GetLowerBound(1)
            iTop = m_vProduct.GetUpperBound(1)

            m_vVersion = Array.CreateInstance(GetType(Object), New Integer() {iTop - iBtm + 1}, New Integer() {iBtm})

            For iLoop As Integer = iBtm To iTop
                lProductCode = gPMConstants.PMProductFamilyByCode(CStr(m_vProduct(ProductCode, iLoop)).Trim())
                m_lReturn = GetClientVersion(lProductCode, sVersion)
                m_vVersion(iLoop) = sVersion

                m_lReturn = SetInstalledVersion(iLoop, sVersion)
            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' checks each update record on each product.
    ' if version number matches installed version it adds '(installed)'
    Private Function SetInstalledVersion(ByVal iProduct As Integer, ByVal sVersion As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            For iLoop As Integer = m_vUpdate.GetLowerBound(1) To m_vUpdate.GetUpperBound(1)

                If m_vProduct(ProductDescription, iProduct).Equals(m_vUpdate(UpdateDescription, iLoop)) And CStr(m_vUpdate(UpdateNewProductVersion, iLoop)) = sVersion Then

                    m_vUpdate(UpdateNewProductVersion, iLoop) = CStr(m_vUpdate(UpdateNewProductVersion, iLoop)) & " (installed)"
                End If

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' set views to start values
    Public Sub ResetViews()

        ' setup treeview control
        m_lReturn = FirstTreeView()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        ' setup initial view for listview
        m_lReturn = FirstListView()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

    End Sub

    ' initial treeview display
    Public Function InitialiseTreeView() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get rid of all nodes and add root node
            tvwProduct.Nodes.Clear()

            tvwProduct.Nodes.Add("root", "Products", "COMPUTER", "COMPUTER")


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' set initial values in treeview
    Private Function FirstTreeView() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = InitialiseTreeView()

            ' for every product
            For iLoop As Integer = m_vProduct.GetLowerBound(1) To m_vProduct.GetUpperBound(1)
                tvwProduct.Nodes.Find("root", True)(0).Nodes.Add(m_vProduct(UpdateUpdateID, iLoop), m_vProduct(UpdateUpdateID, iLoop), "PRODUCT", "PRODUCT")
            Next

            ' for every product update
            For iLoop As Integer = m_vUpdate.GetLowerBound(1) To m_vUpdate.GetUpperBound(1)
                tvwProduct.Nodes.Find(m_vUpdate(UpdateDescription, iLoop), True)(0).Nodes.Add(CStr(m_vUpdate(UpdateDescription, iLoop)) & CStr(m_vUpdate(UpdateNewProductVersion, iLoop)), m_vUpdate(UpdateNewProductVersion, iLoop), "NOTE", "NOTE")
            Next

            ' show contents of root node
            tvwProduct.Nodes.Item("root").Expand()

            'Developer Guide No. 35
            tvwProduct.Nodes.Item("root").Checked = True


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' initial values for listview
    Private Function FirstListView() As Integer

        Dim result As Integer = 0
        Dim oItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' setup columns for product view
            m_lReturn = SetListViewProduct()

            ' for every product
            For iLoop As Integer = m_vProduct.GetLowerBound(1) To m_vProduct.GetUpperBound(1)

                oItem = lvwProduct.Items.Insert(iLoop, CStr(m_vProduct(UpdateUpdateID, iLoop)), CStr(m_vProduct(UpdateUpdateID, iLoop)), "PRODUCT")

                ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vProduct(UpdateUpdateID, iLoop))

            Next


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        MessageBox.Show("Sorry, help is not available", "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        Me.Close()

    End Sub

    ' initial display for listview (product)
    Private Function SetListViewProduct() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' remove all items
            lvwProduct.Items.Clear()
            lvwProduct.Columns.Clear()

            ' add columns for product view
            lvwProduct.Columns.Insert(0, "ID", "ID", CInt(VB6.TwipsToPixelsX(0)))
            lvwProduct.Columns.Insert(1, "DESC", "Product", CInt(lvwProduct.Width - VB6.TwipsToPixelsX(360)))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' set isplay for note display
    Private Function SetListViewNote() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' remove all items and columns
            lvwProduct.Items.Clear()
            lvwProduct.Columns.Clear()

            ' add columns for note view
            lvwProduct.Columns.Insert(0, "ID", "ID", CInt(VB6.TwipsToPixelsX(0)))
            lvwProduct.Columns.Insert(1, "DESC", "Product", CInt(VB6.TwipsToPixelsX(1200)))
            lvwProduct.Columns.Insert(2, "VERSION", "Version", CInt(VB6.TwipsToPixelsX(1200)))
            lvwProduct.Columns.Insert(3, "DATE", "Date", CInt(VB6.TwipsToPixelsX(650)))
            lvwProduct.Columns.Insert(4, "UPDESC", "Decription", CInt(VB6.TwipsToPixelsX(2500)))
            lvwProduct.Columns.Insert(5, "PATH", "Path", CInt(VB6.TwipsToPixelsX(0)))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwProduct.Handle.ToInt32(), v_vShowRowSelect:=True)

    End Sub

    Private Sub lvwProduct_ItemClick(ByVal Item As ListViewItem)

        ' doesn't work
        Exit Sub


        'Developer Guide No. 126

    End Sub

    Public Sub mnuFileExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFileExit.Click

        Me.Close()

    End Sub

    Public Sub mnuViewDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewDetails.Click

        lvwProduct.View = View.Details
        lvwProduct.Refresh()

    End Sub

    Public Sub mnuViewLarge_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewLarge.Click
        lvwProduct.View = View.LargeIcon
        lvwProduct.Refresh()
    End Sub

    Public Sub mnuViewSmall_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuViewSmall.Click
        lvwProduct.View = View.SmallIcon
        lvwProduct.Refresh()
    End Sub

    Private Sub tvwProduct_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tvwProduct.DoubleClick

        Dim sText, sIcon, sParent As String

        Try

            sText = tvwProduct.SelectedNode.Text


            sIcon = tvwProduct.SelectedNode.ImageKey

            Select Case sIcon
                Case "COMPUTER", "PRODUCT"
                    ' do nothing
                Case "NOTE"
                    ' display contents of release note (text editor/Word)
                    sParent = tvwProduct.SelectedNode.Parent.Text
                    SetViewNote(sParent, sText)
                Case Else
                    MessageBox.Show("Unknown node type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Select

        Catch
        End Try





    End Sub

    ' set display modes depending on node type
    Private Sub tvwProduct_AfterSelect(ByVal eventSender As Object, ByVal eventArgs As TreeViewEventArgs) Handles tvwProduct.AfterSelect
        Dim Node As TreeNode = eventArgs.Node

        Dim sText, sIcon As String

        Try

            sText = Node.Text


            sIcon = Node.ImageKey

            Select Case sIcon
                Case "COMPUTER"
                    ' set listview to display products
                    ResetViews()
                Case "PRODUCT"
                    ' set listview to display notes for product
                    SetViewProduct(sText)
                Case "NOTE"
                    ' set listview to display notes for product
                    sText = Node.Parent.Text
                    SetViewProduct(sText)
                Case Else
                    MessageBox.Show("Unknown node type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Select

        Catch
        End Try




    End Sub

    ' when product is selected in treeview
    Public Sub SetViewProduct(ByVal sProduct As String)

        Dim oItem As ListViewItem

        m_lReturn = SetListViewNote()

        Dim iIndex As Integer = 1

        For iLoop As Integer = m_vUpdate.GetLowerBound(1) To m_vUpdate.GetUpperBound(1)

            If CStr(m_vUpdate(UpdateDescription, iLoop)) = sProduct Then

                oItem = lvwProduct.Items.Insert(iIndex - 1, sProduct & CStr(m_vUpdate(UpdateUpdateID, iLoop)), CStr(m_vUpdate(UpdateDescription, iLoop)), "NOTE")

                ListViewHelper.GetListViewSubItem(oItem, 1).Text = CStr(m_vUpdate(UpdateDescription, iLoop))
                ListViewHelper.GetListViewSubItem(oItem, 2).Text = CStr(m_vUpdate(UpdateNewProductVersion, iLoop))
                ListViewHelper.GetListViewSubItem(oItem, 3).Text = CDate(m_vUpdate(UpdateInstallDate, iLoop)).ToString("dd/MM/yy")
                ListViewHelper.GetListViewSubItem(oItem, 4).Text = CStr(m_vUpdate(UpdateUpdateDescription, iLoop))
                ListViewHelper.GetListViewSubItem(oItem, 5).Text = CStr(m_vUpdate(UpdateReleaseNotesPath, iLoop))

                iIndex += 1
            End If

        Next

    End Sub

    ' when note is selected in treeview
    Public Sub SetViewNote(ByVal sParent As String, ByVal sVersion As String)

        Dim lStatus As Integer
        Dim sNotePath, sFileExt As String
        Dim oBrowser As frmBrowse

        Try

            ' not usingthe listview
            lvwProduct.Items.Clear()
            lvwProduct.Columns.Clear()

            ' search for the filepath to the note
            For iLoop As Integer = m_vUpdate.GetLowerBound(1) To m_vUpdate.GetUpperBound(1)
                If CStr(m_vUpdate(UpdateDescription, iLoop)) = sParent And CStr(m_vUpdate(UpdateNewProductVersion, iLoop)) = sVersion Then

                    sNotePath = CStr(m_vUpdate(UpdateReleaseNotesPath, iLoop))
                End If
            Next

            ' call display the file
            sFileExt = GetFileExt(sNotePath)

            Select Case sFileExt.ToUpper()
                Case ""
                    ' can't work out the file type - 'dot' missing
                    MessageBox.Show("Unable to establish file type", "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Case "TXT"
                    ' use notepad

                    Dim startInfo As ProcessStartInfo = New ProcessStartInfo("NOTEPAD " & sNotePath)
                    startInfo.WindowStyle = ProcessWindowStyle.Normal
                    m_lReturn = CInt(Process.Start(startInfo).Id)
                Case "HTM", "HTML"
                    ' RDC 16012001 test ShellExecute
                    'Call ShellExecute(Me.hWnd, "open", sNotePath$, vbNullString, vbNullString, SE_SHOW_NORMAL)
                    'Exit Sub

                    ' use browser
                    oBrowser = New frmBrowse()


                    'Developer Guide No. 68

                    oBrowser.NotePath = sNotePath
                    oBrowser.ShowNote()

                    oBrowser.ShowDialog()

                    lStatus = oBrowser.Status

                    oBrowser.Close()

                    oBrowser = Nothing

                Case Else
                    ' unsupported file type
                    MessageBox.Show("File type '" & sFileExt & "' not supported." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Supported file types: TXT,HTM, HTML.", "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End Select

        Catch excep As System.Exception



            MessageBox.Show("Failed to display selected release notes:" & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                            Information.Err().Number & ": " & excep.Message, "ProductUpdateHistory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

        End Try


    End Sub

    Public Function GetFileExt(ByVal sFileName As String) As String

        Dim result As String = String.Empty
        Dim iPos As Integer

        Try

            result = ""

            iPos = sFileName.Length

            Do Until Mid(sFileName, iPos, 1) = "." Or iPos = 0
                result = Mid(sFileName, iPos)
                iPos -= 1
            Loop

            Return result

        Catch



            Return ""
        End Try

    End Function
End Class
