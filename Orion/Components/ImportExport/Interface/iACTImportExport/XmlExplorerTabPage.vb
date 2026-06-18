Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms
Imports System.Xml
Imports System.Diagnostics
Imports System.Threading
Imports System.Text
Imports System.Xml.XPath


Partial Public Class XmlExplorerTabPage
    Inherits TabPage
#Region "Variables"

    ' the background thread used to load xml file data 
    Private _loadFileThread As Thread

    ' the full path of the file currently opened, if opened
    ' from a file, or once a node set has been saved to a file
    Private _filename As String

    ' the node set currently opened, if opened from
    ' an XPath expression result
    Private _nodes As XPathNodeIterator

#End Region

#Region "Events"

    ''' <summary>
    ''' Occurs before a tab is closed.
    ''' </summary>
    Public Event BeforeClosed As EventHandler(Of CancelEventArgs)

    ''' <summary>
    ''' Occurs after a tab has been closed, and needs removed from the display.
    ''' </summary>
    Public Event NeedsClosed As EventHandler(Of EventArgs)

    ''' <summary>
    ''' Occurs when the asynchronous loading of an xml file has started.
    ''' </summary>
    Public Event LoadingFileStarted As EventHandler(Of EventArgs)

    ''' <summary>
    ''' Occurs when the asynchronous loading of an xml file has completed.
    ''' </summary>
    Public Event LoadingFileCompleted As EventHandler(Of EventArgs)

    ''' <summary>
    ''' Occurs when an exception is encountered while loading an xml file.
    ''' </summary>
    Public Event LoadingFileFailed As EventHandler(Of EventArgs)

#End Region

#Region "Constructors"

    Public Sub New()
        MyBase.New()
        Me.InitializeComponent()
        Me.xmlTreeView.HideSelection = False
        Me.xmlTreeView.LabelEdit = True
        AddHandler Me.xmlTreeView.AfterLabelEdit, AddressOf Me.xmlTreeView_AfterLabelEdit
        'AddHandler Me.xmlTreeView.ItemDrag, AddressOf Me.xmlTreeView_ItemDrag
        'Me.xmlTreeView.AfterLabelEdit += New NodeLabelEditEventHandler(AddressOf xmlTreeView_AfterLabelEdit)
        'Me.xmlTreeView.ItemDrag += New ItemDragEventHandler(AddressOf xmlTreeView_ItemDrag)
    End Sub

    Public Sub New(ByVal text As String)
        Me.New()
        MyBase.Text = text
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets an XPathNavigator to display in the TreeView.
    ''' </summary>
    Public Property Navigator() As XPathNavigator
        Get
            Return Me.xmlTreeView.Navigator
        End Get
        Set(ByVal value As XPathNavigator)
            Me.xmlTreeView.Navigator = value
        End Set
    End Property

    ''' <summary>
    ''' Gets the XPathNavigatorTreeView used to display XML data.
    ''' </summary>
    Public ReadOnly Property XPathNavigatorTreeView() As XPathNavigatorTreeView
        Get
            Return Me.xmlTreeView
        End Get
    End Property

    ''' <summary>
    ''' Gets the filename, if any, of the currently opened file.
    ''' </summary>
    Public ReadOnly Property Filename() As String
        Get
            Return _filename
        End Get
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Closes the tab, aborting the background thread used for loading if needed.
    ''' </summary>
    Public Sub Close()
        Dim e As New CancelEventArgs(False)

        ' raise the BeforeClosed event to give the user the chance to cancel, if needed.
        RaiseEvent BeforeClosed(Me, e)

        If e.Cancel Then
            Return
        End If

        ' if an xml file is currently being loaded on a background thread
        If _loadFileThread IsNot Nothing AndAlso _loadFileThread.IsAlive Then
            Try
                ' abort the load thread
                Debug.WriteLine("Aborting xml document file load thread by user request...")
                _loadFileThread.Abort()
            Catch ex As ThreadAbortException
                Debug.WriteLine(ex)
            End Try
        End If

        ' raise the NeedsClosed event, so the tab can be removed from the display
        RaiseEvent NeedsClosed(Me, EventArgs.Empty)
    End Sub

    ''' <summary>
    ''' Loads XML data from a string.
    ''' </summary>
    ''' <param name="xml"></param>
    Public Sub LoadXml(ByVal xml As String)
        Me.xmlTreeView.LoadXml(xml)
    End Sub

    ''' <summary>
    ''' Loads XML file data from a given filename.
    ''' </summary>
    ''' <param name="filename"></param>
    Public Sub Open(ByVal filename As String)
        _filename = filename

        ' set the tab text and tooltip
        Me.Text = Path.GetFileName(filename)
        Me.ToolTipText = filename

        ' begin loading the file on a background thread
        Me.BeginLoadFile()
    End Sub

    ''' <summary>
    ''' Loads an XML node set.
    ''' </summary>
    ''' <param name="iterator"></param>
    Public Sub Open(ByVal iterator As XPathNodeIterator)
        _filename = String.Empty
        Me.Text = "XPath results"

        _nodes = iterator.Clone()

        RaiseEvent LoadingFileStarted(Me, EventArgs.Empty)

        Me.xmlTreeView.BeginUpdate()
        Try
            Me.xmlTreeView.LoadNodes(iterator, Me.xmlTreeView.Nodes)

            RaiseEvent LoadingFileCompleted(Me, EventArgs.Empty)
        Catch ex As ThreadAbortException
            Debug.WriteLine(ex)
            RaiseEvent LoadingFileFailed(Me, EventArgs.Empty)
        Catch ex As Exception
            Debug.WriteLine(ex)
            MessageBox.Show(Me, ex.ToString())
            RaiseEvent LoadingFileFailed(Me, EventArgs.Empty)
        Finally
            Me.xmlTreeView.EndUpdate()
        End Try
    End Sub

    ''' <summary>
    ''' Begins loading an XML file on a background thread.
    ''' </summary>
    Private Sub BeginLoadFile()
        _loadFileThread = New Thread(New ThreadStart(AddressOf Me.LoadFile))
        _loadFileThread.IsBackground = True
        _loadFileThread.Start()
    End Sub

    ''' <summary>
    ''' Finds and selects a tree node from an XPath expression.
    ''' </summary>
    ''' <param name="xpath"></param>
    ''' <returns></returns>
    Public Function FindByXpath(ByVal xpath As String) As Boolean
        Return xmlTreeView.FindByXpath(xpath)
    End Function

    ''' <summary>
    ''' Evaluates an XPath expression, and returns the typed result.
    ''' </summary>
    ''' <param name="xpath"></param>
    ''' <returns></returns>
    Public Function SelectXmlNodes(ByVal xpath As String) As XPathNodeIterator
        Return TryCast(Me.XPathNavigatorTreeView.SelectXmlNodes(xpath), XPathNodeIterator)
    End Function

    ''' <summary>
    ''' The background worker method used to load an XML file in the background.
    ''' </summary>
    Private Sub LoadFile()
        Try
            Dim document As XPathDocument
            RaiseEvent LoadingFileStarted(Me, EventArgs.Empty)

            Debug.WriteLine(String.Format("Peak RAM Before......{0}", Process.GetCurrentProcess().PeakWorkingSet64.ToString()))
            Debug.Write("Loading XPathDocument.")
            Dim start As DateTime = DateTime.Now

            ' load the document
            document = New XPathDocument(_filename)

            Debug.WriteLine(String.Format("Done. Elapsed: {0}ms.", DateTime.Now.Subtract(start).TotalMilliseconds))


            ' the UI has to be updated on the thread that created it, so invoke back to the main UI thread.
            'Dim del As MethodInvoker = AddressOf LoadDocument(document)
            'Me.Invoke(New MethodInvoker(Function() Me.Text = "Test"))

            Me.Invoke(CType(Function() LoadDocument(document), MethodInvoker))




            'Dim del As MethodInvoker =  delegate()  
            'Me.LoadDocument(document)
            'Me.Invoke(del)
            document = Nothing
            RaiseEvent LoadingFileCompleted(Me, EventArgs.Empty)
        Catch ex As ThreadAbortException
            ' do not display the exception to the user, as they most likely aborted the thread by 
            ' closing the tab or application themselves
            Debug.WriteLine(ex)
            RaiseEvent LoadingFileFailed(Me, EventArgs.Empty)
        Catch ex As Exception
            Debug.WriteLine(ex)

            'Dim del As MethodInvoker = Sub() 
            MessageBox.Show(Me, ex.ToString())

            ' Me.Invoke(del)

            RaiseEvent LoadingFileFailed(Me, EventArgs.Empty)
        End Try
    End Sub

    ''' <summary>
    ''' Loads an XPathDocument into the tree.
    ''' </summary>
    ''' <param name="document"></param>
    Private Function LoadDocument(ByVal document As XPathDocument) As Object
        If document Is Nothing Then
            Throw New ArgumentNullException("document")
        End If

        Debug.Write("Loading UI.")
        Dim start As DateTime = DateTime.Now
        Me.xmlTreeView.Navigator = document.CreateNavigator()
        Debug.WriteLine(String.Format("Done. Elapsed: {0}ms.", DateTime.Now.Subtract(start).TotalMilliseconds))
        Debug.WriteLine(String.Format("Peak RAM After......{0}", Process.GetCurrentProcess().PeakWorkingSet64.ToString()))
        Return 0
    End Function

    ''' <summary>
    ''' Reloads the display from the original file or node set.
    ''' </summary>
    Public Sub Reload()
        Me.xmlTreeView.Nodes.Clear()

        If Not String.IsNullOrEmpty(_filename) Then
            Me.Open(_filename)
        ElseIf _nodes IsNot Nothing Then
            Me.Open(_nodes)
        End If
    End Sub

    ''' <summary>
    ''' Copies the current selected xml node (and all of it's sub nodes) to the clipboard
    ''' as formatted XML text.
    ''' </summary>
    Public Sub CopyFormattedOuterXml()
        Dim selected As XPathNavigatorTreeNode = TryCast(Me.xmlTreeView.SelectedNode, XPathNavigatorTreeNode)

        If selected Is Nothing Then
            Return
        End If

        Dim text As String = GetXmlTreeNodeFormattedOuterXml(selected)

        If Not String.IsNullOrEmpty(text) Then
            Clipboard.SetData(DataFormats.Text, text)
        End If
    End Sub

    ''' <summary>
    ''' Copies the current selected xml node to the clipboard as XML text.
    ''' </summary>
    Public Sub CopyNodeText()
        Dim selected As XPathNavigatorTreeNode = TryCast(Me.xmlTreeView.SelectedNode, XPathNavigatorTreeNode)

        If selected Is Nothing Then
            Return
        End If

        Dim text As String = selected.Text

        If Not String.IsNullOrEmpty(text) Then
            Clipboard.SetData(DataFormats.Text, text)
        End If
    End Sub

    ''' <summary>
    ''' Returns the selected xml node (and all of it's sub nodes) as formatted XML text.
    ''' </summary>
    Public Function GetXmlTreeNodeFormattedOuterXml(ByVal node As XPathNavigatorTreeNode) As String
        Using stream As New MemoryStream()
            Using writer As New XmlTextWriter(stream, Encoding.[Default])
                writer.Formatting = Formatting.Indented

                node.Navigator.WriteSubtree(writer)

                writer.Flush()

                Return Encoding.[Default].GetString(stream.ToArray())
            End Using
        End Using
    End Function

  

#End Region

#Region "Event Handlers"

    Protected Overrides Sub OnMouseUp(ByVal e As MouseEventArgs)
        MyBase.OnMouseUp(e)
        If e.Button = MouseButtons.Middle Then
            Me.Close()
        End If
    End Sub

    Private Sub xmlTreeView_AfterLabelEdit(ByVal sender As Object, ByVal e As NodeLabelEditEventArgs)
        e.CancelEdit = True
    End Sub

   

#End Region

    
End Class


