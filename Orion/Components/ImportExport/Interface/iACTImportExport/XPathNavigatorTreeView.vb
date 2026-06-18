Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections.Generic
Imports System.Xml.XPath
Imports System.IO
Imports System.Text


Public Class XPathNavigatorTreeView
    Inherits TreeView
#Region "Variables"

    ''' <summary>
    ''' The XPathNavigator that represents the root of the tree.
    ''' </summary>
    Protected _navigator As XPathNavigator

#End Region

#Region "Constructors"

    ''' <summary>
    ''' Initializes a new XPathNavigatorTreeView.
    ''' </summary>
    Public Sub New()
        ' I always end up forgetting to turn this off each time I use a TreeView,
        ' so I'll just take care of it here automatically.
        MyBase.HideSelection = False
    End Sub

#End Region

#Region "Properties"

    Public Property Navigator() As XPathNavigator
        Get
            Return _navigator
        End Get

        Set(ByVal value As XPathNavigator)
            _navigator = value
            Me.Initialize()
        End Set
    End Property

#End Region

#Region "Methods"

    ''' <summary>
    ''' Loads the initial xml tree node.
    ''' </summary>
    Protected Overridable Sub Initialize()
        Dim node As XPathNavigatorTreeNode = Nothing

        ' suspend drawing of the tree while loading nodes to improve performance
        ' as adding each node would normally require an entire redraw of the tree
        MyBase.BeginUpdate()
        Try
            ' clear all the nodes in the tree
            MyBase.Nodes.Clear()

            ' setting the navigator to null should just clear the tree
            ' no exception needs thrown
            If _navigator IsNot Nothing Then
                ' if it's the root node of the document, load it's children
                ' we don't display the root
                If _navigator.NodeType = XPathNodeType.Root Then
                    Me.LoadNodes(_navigator.SelectChildren(XPathNodeType.Element), MyBase.Nodes)
                Else
                    ' otherwise, create a tree node and load it
                    node = New XPathNavigatorTreeNode(_navigator)
                    MyBase.Nodes.Add(node)
                End If
            End If
        Finally
            ' resume drawing of the tree
            MyBase.EndUpdate()
            If node IsNot Nothing Then
                node.Expand()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Handles the expanding of an XPathNavigatorTreeNode.
    ''' </summary>
    ''' <param name="node"></param>
    Protected Overridable Sub ExpandNode(ByVal treeNode As XPathNavigatorTreeNode)
        If treeNode Is Nothing Then
            Return
        End If

        ' for better performance opening large files, tree nodes are loaded on demand.
        ' return if the node has already been expanded and loaded
        If treeNode.HasExpanded Then
            Return
        End If

        treeNode.HasExpanded = True

        MyBase.BeginUpdate()
        Try
            ' load the child nodes of the specified xml tree node
            Me.LoadNodes(treeNode.Navigator.SelectChildren(XPathNodeType.All), treeNode.Nodes)
        Finally
            MyBase.EndUpdate()
        End Try
    End Sub

    ''' <summary>
    ''' Loads an XPathNavigatorTreeNode for each XPathNavigator in the specified XPathNodeIterator, into the 
    ''' specified TreeNodeCollection.
    ''' </summary>
    ''' <param name="iterator"></param>
    ''' <param name="treeNodeCollection"></param>
    Public Overridable Sub LoadNodes(ByVal iterator As XPathNodeIterator, ByVal treeNodeCollection As TreeNodeCollection)
        ' handle null arguments
        If iterator Is Nothing Then
            Throw New ArgumentNullException("navigator")
        End If

        If treeNodeCollection Is Nothing Then
            Throw New ArgumentNullException("parentNodeCollection")
        End If

        ' use the wait cursor, in case this takes a while
        Me.UseWaitCursor = True

        Try
            treeNodeCollection.Clear()

            ' create and add a node for each navigator
            For Each navigator As XPathNavigator In iterator
                Dim node As New XPathNavigatorTreeNode(navigator.Clone())
                treeNodeCollection.Add(node)
            Next
        Finally
            Me.UseWaitCursor = False
        End Try
    End Sub

    ''' <summary>
    ''' Loads the specified XML data into the tree.
    ''' </summary>
    ''' <param name="xml">XML string data to load.</param>
    Public Sub LoadXml(ByVal xml As String)
        ' create a StringReader around the XML data string
        Using reader As New StringReader(xml)
            ' create an XPathDocument around the StringReader
            Dim document As New XPathDocument(reader)

            ' load the navigator
            Me.Navigator = document.CreateNavigator()
        End Using
    End Sub

    ''' <summary>
    ''' Loads the specified XML file into the tree.
    ''' </summary>
    ''' <param name="filename">The full path of an XML data file to load.</param>
    Public Sub Open(ByVal filename As String)
        Dim document As New XPathDocument(filename)
        Me.Navigator = document.CreateNavigator()
    End Sub

    ''' <summary>
    ''' Finds and selects an XPathNavigatorTreeNode for the given XPathNavigator.
    ''' </summary>
    ''' <param name="navigator">An XPathNavigator to find and select in the tree.</param>
    ''' <returns></returns>
    Private Function SelectXmlTreeNode(ByVal navigator As XPathNavigator) As Boolean
        If navigator Is Nothing Then
            Throw New ArgumentNullException("navigator")
        End If

        ' we've found a node, so build an ancestor stack
        Dim ancestors As Stack(Of XPathNavigator) = Me.GetAncestors(navigator)

        ' now find treenodes to match the ancestors
        Dim treeNode As TreeNode = Me.GetXmlTreeNode(ancestors)

        If treeNode Is Nothing Then
            Return False
        End If

        ' select the node
        Me.SelectedNode = treeNode

        Return True
    End Function

    ''' <summary>
    ''' Finds a TreeNode for a given stack of XPathNavigators.
    ''' </summary>
    ''' <param name="ancestors">A Stack of XPathNavigators with which to find a TreeNode.</param>
    ''' <returns></returns>
    Private Function GetXmlTreeNode(ByVal ancestors As Stack(Of XPathNavigator)) As TreeNode
        Dim navigator As XPathNavigator = Nothing

        ' start at the root
        Dim nodes As TreeNodeCollection = Me.Nodes

        Dim treeNode As TreeNode = Nothing

        ' loop through the ancestor XPathNavigators
        While ancestors.Count > 0 AndAlso (InlineAssignHelper(navigator, ancestors.Pop())) IsNot Nothing
            ' loop through the TreeNodes at the current level
            For Each node As TreeNode In nodes
                ' make sure it's an XPathNavigatorTreeNode
                Dim xmlTreeNode As XPathNavigatorTreeNode = TryCast(node, XPathNavigatorTreeNode)
                If xmlTreeNode Is Nothing Then
                    Continue For
                End If

                ' check to see if we've found the correct TreeNode
                If xmlTreeNode.Navigator.IsSamePosition(navigator) Then
                    ' expand the tree node, if it hasn't alreay been expanded
                    If Not node.IsExpanded Then
                        node.Expand()
                    End If

                    ' we've taken another step towards the target node
                    treeNode = node

                    ' update the current level
                    nodes = node.Nodes

                    ' handle the next level, if any
                    Exit For
                End If
            Next
        End While

        ' return the result, if any was found
        Return treeNode
    End Function

    ''' <summary>
    ''' Builds and returns a Stack of XPathNavigator ancestors for a given XPathNavigator.
    ''' </summary>
    ''' <param name="navigator">The XPathNavigator from which to build a stack.</param>
    ''' <returns></returns>
    Private Function GetAncestors(ByVal navigator As XPathNavigator) As Stack(Of XPathNavigator)
        If navigator Is Nothing Then
            Throw New ArgumentNullException("navigator")
        End If

        Dim ancestors As New Stack(Of XPathNavigator)()

        ' navigate up the xml tree, building the stack as we go
        While navigator IsNot Nothing
            ' push the current ancestor onto the stack
            ancestors.Push(navigator)

            ' clone the current navigator cursor, so we don't lose our place
            navigator = navigator.Clone()

            ' if we've reached the top, we're done
            If Not navigator.MoveToParent() Then
                Exit While
            End If

            ' if we've reached the root, we're done
            If navigator.NodeType = XPathNodeType.Root Then
                Exit While
            End If
        End While

        ' return the result
        Return ancestors
    End Function

    ''' <summary>
    ''' Evaluates the XPath expression and returns the typed result.
    ''' </summary>
    ''' <param name="xpath">A string representing an XPath expression that can be evaluated.</param>
    ''' <returns></returns>
    Public Function SelectXmlNodes(ByVal xpath As String) As Object
        ' get the selected node
        Dim node As XPathNavigatorTreeNode = TryCast(Me.SelectedNode, XPathNavigatorTreeNode)

        ' if there is no selected node, default to the root node
        If node Is Nothing AndAlso Me.Nodes.Count > 0 Then
            node = Me.GetRootXmlTreeNode()
        End If

        If node Is Nothing Then
            Return Nothing
        End If

        ' evaluate the expression, return the result
        Return node.Navigator.Evaluate(xpath)
    End Function

    ''' <summary>
    ''' Returns the root XPathNavigatorTreeNode in the tree.
    ''' </summary>
    ''' <returns></returns>
    Private Function GetRootXmlTreeNode() As XPathNavigatorTreeNode
        For Each node As TreeNode In Me.Nodes
            Dim xmlTreeNode As XPathNavigatorTreeNode = TryCast(node, XPathNavigatorTreeNode)

            If xmlTreeNode Is Nothing OrElse xmlTreeNode.Navigator Is Nothing Then
                Continue For
            End If

            If xmlTreeNode.Navigator.NodeType <> XPathNodeType.Element Then
                Continue For
            End If

            Return xmlTreeNode
        Next

        Return TryCast(Me.Nodes(0), XPathNavigatorTreeNode)
    End Function

    ''' <summary>
    ''' Finds and selects an XPathNavigatorTreeNode using an XPath expression.
    ''' </summary>
    ''' <param name="xpath">An XPath expression.</param>
    ''' <returns></returns>
    Public Function FindByXpath(ByVal xpath As String) As Boolean
        ' evaluate the expression
        Dim result As Object = Me.SelectXmlNodes(xpath)

        If result Is Nothing Then
            Return False
        End If

        ' did the expression evaluate to a node set?
        Dim iterator As XPathNodeIterator = TryCast(result, XPathNodeIterator)

        If iterator IsNot Nothing Then
            ' the expression evaluated to a node set
            If iterator Is Nothing OrElse iterator.Count < 1 Then
                Return False
            End If

            If Not iterator.MoveNext() Then
                Return False
            End If

            ' select the first node in the set
            Return Me.SelectXmlTreeNode(iterator.Current)
        Else
            ' the expression evaluated to something else, most likely a count()
            ' show the result in a new window
            'ExpressionResultsWindow dialog = new ExpressionResultsWindow();
            'dialog.Expression = xpath;
            'dialog.Result = result.ToString();
            'dialog.ShowDialog(this.FindForm());
            Return True
        End If
    End Function

    ''' <summary>
    ''' Returns a string representing the full path of an XPathNavigator.
    ''' </summary>
    ''' <param name="navigator">An XPathNavigator.</param>
    ''' <returns></returns>
    Public Function GetXmlNodeFullPath(ByVal navigator As XPathNavigator) As String
        ' create a StringBuilder for assembling the path
        Dim sb As New StringBuilder()

        ' clone the navigator (cursor), so the node doesn't lose it's place
        navigator = navigator.Clone()

        ' traverse the navigator's ancestry all the way to the top
        While navigator IsNot Nothing
            ' skip anything but elements
            If navigator.NodeType = XPathNodeType.Element Then
                ' insert the node and a seperator
                sb.Insert(0, navigator.Name)
                sb.Insert(0, "/")
            End If
            If Not navigator.MoveToParent() Then
                Exit While
            End If
        End While

        Return sb.ToString()
    End Function

#End Region

#Region "Overrides"

    Protected Overrides Sub OnBeforeCollapse(ByVal e As TreeViewCancelEventArgs)
        ' remove the end tag we inserted when the node was expanded
        Dim node As TreeNode = TryCast(e.Node.Tag, TreeNode)
        If node IsNot Nothing Then
            ' remove it
            Dim nodes As TreeNodeCollection = MyBase.Nodes
            If node.Parent IsNot Nothing Then
                nodes = node.Parent.Nodes
            End If
            nodes.Remove(node)
        End If

        MyBase.OnBeforeCollapse(e)
    End Sub

    Protected Overrides Sub OnBeforeExpand(ByVal e As TreeViewCancelEventArgs)
        Dim node As XPathNavigatorTreeNode = TryCast(e.Node, XPathNavigatorTreeNode)

        ' expand the node, adding any child xml tree nodes
        Me.ExpandNode(node)

        If node IsNot Nothing Then
            If node.Nodes.Count > 0 Then
                Dim nodes As TreeNodeCollection = MyBase.Nodes
                If node.Parent IsNot Nothing Then
                    nodes = node.Parent.Nodes
                End If

                ' add a node for the xml end tag, such as </node>
                node.Tag = nodes.Insert(e.Node.Index + 1, String.Format("</{0}>", node.Navigator.Name))
            End If
        End If

        MyBase.OnBeforeExpand(e)
    End Sub
    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

#End Region
End Class


