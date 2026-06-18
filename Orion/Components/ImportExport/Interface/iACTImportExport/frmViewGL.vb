Imports System.Collections.ObjectModel
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Imports System.Xml.XPath
Imports System.IO
Imports SharedFiles
Public Class frmViewGL
    Private sfilename As String
    Private sTitle As String
    Private sfilesize As String
    Private _tabsCurrentlyLoading As New List(Of Object)()
    Private _startedLoading As DateTime

    Public WriteOnly Property FileName() As String
        Set(ByVal s_vfilename As String)
            sfilename = s_vfilename
        End Set
    End Property
    Public WriteOnly Property FileSize() As Integer
        Set(ByVal s_vfilesize As Integer)
            sfilesize = s_vfilesize
        End Set
    End Property
    Public WriteOnly Property Title() As String
        Set(ByVal s_vsTitle As String)
            sTitle = s_vsTitle
        End Set
    End Property

    Private Sub cmdOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub

    Private Sub frmViewGL_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = sTitle
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
        ' create a tab page
        Dim tabPage As XmlExplorerTabPage = Me.CreateXmlExplorerTabPage()

        ' instruct the tab to open the specified file.
        tabPage.Open(sfilename)
        tabPage.AutoScroll = True

        ' add the tabpage to the tab control
        Me.tabControl.TabPages.Add(tabPage)
        ' select the newly opened tab
        Me.tabControl.SelectedTab = tabPage
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    End Sub
    ''' <summary>
    ''' Returns an initialized XmlExplorerTabPage.
    ''' </summary>
    ''' <returns></returns>
    Private Function CreateXmlExplorerTabPage() As XmlExplorerTabPage
        ' the main window handle needs to be created before creating a new tab page
        If Not Me.IsHandleCreated Then
            Me.CreateHandle()
        End If

        Dim tabPage As New XmlExplorerTabPage()
        ' read the options for the tab page, such as font and forecolor
        'this.ReadTabPageOptions(tabPage);

        ' wire to the tab's events
        'AddHandler tabPage.NeedsClosed, AddressOf Me.OnTabPageNeedsClosed
        AddHandler tabPage.LoadingFileCompleted, AddressOf Me.OnTabPageLoadingFileCompleted
        AddHandler tabPage.LoadingFileFailed, AddressOf Me.OnTabPageLoadingFileFailed
        AddHandler tabPage.LoadingFileStarted, AddressOf Me.OnTabPageLoadingFileStarted
        AddHandler tabPage.XPathNavigatorTreeView.AfterSelect, AddressOf Me.OnTabPage_XmlTreeView_AfterSelect

        Return tabPage
    End Function

    ''' <summary>
    ''' Occurs when a tab page has completed asynchronously loading an XML file.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OnTabPageLoadingFileCompleted(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' this event will likely occur on a background thread,
            ' marshal the event back to the window's thread
            If Me.InvokeRequired Then
                Me.Invoke(New EventHandler(Of EventArgs)(AddressOf Me.OnTabPageLoadingFileCompleted), sender, e)
                Return
            End If

            ' remove the tab
            Me.RemoveLoadingTab(sender)
        Catch ex As Exception
            Debug.WriteLine(ex)
            MessageBox.Show(Me, ex.ToString())
        End Try
    End Sub
    ''' <summary>
    ''' Removes a tab from our list of tabs currently loading xml files.
    ''' </summary>
    ''' <param name="tab">The tab that has finished loading, and needs removed from
    ''' the list of currently loading tabs.</param>
    Private Sub RemoveLoadingTab(ByVal tab As Object)
        ' lock the list for thread safety
        SyncLock _tabsCurrentlyLoading
            ' remove the tab if it's in the list
            If _tabsCurrentlyLoading.Contains(tab) Then
                _tabsCurrentlyLoading.Remove(tab)
            End If

            ' update the loading status
            Me.UpdateLoadingStatus()
        End SyncLock
    End Sub
    ''' <summary>
    ''' Displays progress for tabs that are loading xml files.
    ''' </summary>
    Private Sub UpdateLoadingStatus()
        ' get the number of tabs that are currently loading xml files
        Dim count As Integer = _tabsCurrentlyLoading.Count

        Dim loadingStatus As String = String.Empty

        ' build the status text
        If count > 0 Then
            Dim suffix As String = If(count = 1, String.Empty, "s")
            loadingStatus = String.Format("Loading {0} file{1}", count.ToString(), suffix)
        Else
            Dim elapsed As TimeSpan = DateTime.Now - _startedLoading
            loadingStatus = String.Format("Loaded in {0}", elapsed.ToString())
        End If

        ' update the progress bar, we want it to scroll if any tabs are still loading,
        ' but be hidden if there are none loading
        Me.toolStripProgressBar.Style = ProgressBarStyle.Marquee
        Me.toolStripProgressBar.Enabled = count > 0
        Me.toolStripProgressBar.Visible = count > 0

        ' update the status text
        Me.toolStripStatusLabelMain.Text = loadingStatus
    End Sub

    ''' <summary>
    ''' Occurs when an exception occurs while loading an XML file.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OnTabPageLoadingFileFailed(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' this event will likely occur on a background thread,
            ' marshal the event back to the window's thread
            If Me.InvokeRequired Then
                Me.Invoke(New EventHandler(Of EventArgs)(AddressOf Me.OnTabPageLoadingFileFailed), sender, e)
                Return
            End If

            ' remove the tab
            Me.RemoveLoadingTab(sender)
        Catch ex As Exception
            Debug.WriteLine(ex)
            MessageBox.Show(Me, ex.ToString())
        End Try
    End Sub
    ''' <summary>
    ''' Occurs when a tab page has begun asynchronously loading an XML file.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub OnTabPageLoadingFileStarted(ByVal sender As Object, ByVal e As EventArgs)
        Try
            ' this event will likely occur on a background thread,
            ' marshal the event back to the window's thread
            If Me.InvokeRequired Then
                Me.Invoke(New EventHandler(Of EventArgs)(AddressOf Me.OnTabPageLoadingFileStarted), sender, e)
                Return
            End If

            ' lock the list for thread safety
            SyncLock _tabsCurrentlyLoading
                If _tabsCurrentlyLoading.Count < 1 Then
                    _startedLoading = DateTime.Now
                End If

                ' add the tab
                _tabsCurrentlyLoading.Add(sender)

                ' update the status and progress
                Me.UpdateLoadingStatus()
            End SyncLock
        Catch ex As Exception
            Debug.WriteLine(ex)
            MessageBox.Show(Me, ex.ToString())
        End Try
    End Sub
    ''' <summary>
    ''' Occurs when the selected xml node of a tab page has changed.
    ''' </summary>
    Private Sub OnTabPage_XmlTreeView_AfterSelect(ByVal sender As Object, ByVal e As TreeViewEventArgs)
        Try
            ' get the newly selected node
            Dim node As XPathNavigatorTreeNode = TryCast(e.Node, XPathNavigatorTreeNode)
            If node Is Nothing Then
                Return
            End If

            If node.Navigator Is Nothing Then
                Return
            End If

            Dim treeView As XPathNavigatorTreeView = TryCast(sender, XPathNavigatorTreeView)
            If treeView Is Nothing Then
                Return
            End If

            ' get the xml node's child count
            Dim count As Integer = node.Navigator.SelectChildren(XPathNodeType.Element).Count

            ' update the status bar with the child count
            Me.toolStripStatusLabelChildCount.Text = String.Format("{0} child node{1}", count, If(count = 1, String.Empty, "s"))

            ' update the status bar with the full path of the selected xml node
            Me.toolStripStatusLabelMain.Text = treeView.GetXmlNodeFullPath(node.Navigator)
        Catch ex As Exception
            Debug.WriteLine(ex)
            MessageBox.Show(Me, ex.ToString())
        End Try
    End Sub

    Private Sub frmViewGL_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        GC.Collect()
        GC.WaitForPendingFinalizers()
        GC.Collect()
    End Sub
End Class
