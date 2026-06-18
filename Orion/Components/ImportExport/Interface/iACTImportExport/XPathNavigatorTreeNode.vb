Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Xml.XPath


Public NotInheritable Class XPathNavigatorTreeNode 
    Inherits TreeNode
#Region "Variables"

    Private _hasExpanded As Boolean
    Private _navigator As XPathNavigator

#End Region

#Region "Constructors"

    Public Sub New()
    End Sub

    Public Sub New(ByVal navigator As XPathNavigator)
        Me.Navigator = navigator
    End Sub

#End Region

#Region "Properties"

    ''' <summary>
    ''' Gets or sets whether this XPathNavigatorTreeNode has been expanded and loaded.
    ''' </summary>
    Public Property HasExpanded() As Boolean
        Get
            Return _hasExpanded
        End Get
        Set(ByVal value As Boolean)
            _hasExpanded = value
        End Set
    End Property

    ''' <summary>
    ''' Gets or sets the XPathNavigator this XPathNavigatorTreeNode represents.
    ''' </summary>
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
    ''' Returns the text used to display this XPathNavigatorTreeNode, formatted using the XPathNavigator it represents.
    ''' </summary>
    ''' <returns></returns>
    Public Function GetDisplayText() As String
        If _navigator Is Nothing Then
            Return String.Empty
        End If

        Dim builder As New StringBuilder()
        Select Case _navigator.NodeType
            Case XPathNodeType.Comment
                ' comments are easy, just append the value inside <!-- --> tags
                builder.Append("<!--")
                builder.Append(Me.StripNonPrintableChars(_navigator.Value))
                builder.Append(" -->")
                Exit Select

            Case XPathNodeType.Root, XPathNodeType.Element
                ' append the start of the element
                builder.AppendFormat("<{0} ", _navigator.Name)

                ' append any attributes
                If _navigator.HasAttributes Then
                    ' clone the node's navigator (cursor), so it doesn't lose it's position
                    Dim attributeNavigator As XPathNavigator = _navigator.Clone()
                    If attributeNavigator.MoveToFirstAttribute() Then
                        Do
                            builder.AppendFormat("{0}=""{1}"" ", attributeNavigator.Name, attributeNavigator.Value)
                        Loop While attributeNavigator.MoveToNextAttribute()
                    End If
                End If

                ' if the element has no children, close the node immediately
                If Not _navigator.HasChildren Then
                    builder.Append("/>")
                Else
                    ' otherwise, an end tag node will be appended by the XPathNavigatorTreeView after it's expanded
                    builder.Append(">")
                End If
                Exit Select
            Case Else

                ' all other node types are easy, just append the value
                ' strings, whitespace, etc.
                builder.Append(Me.StripNonPrintableChars(_navigator.Value))
                Exit Select
        End Select
        Return builder.ToString()
    End Function

    ''' <summary>
    ''' Initializes the node using it's XPathNavigator
    ''' </summary>
    Public Sub Initialize()
        ' default to an empty string
        Dim displayText As String = String.Empty

        MyBase.Nodes.Clear()
        If _navigator IsNot Nothing Then
            displayText = Me.GetDisplayText()

            ' if the xml node has children, add a placeholder node,
            ' so that it can be expanded.  a treenode with no child 
            ' nodes has no expansion indicator , and cannot be expanded
            If _navigator.HasChildren Then
                MyBase.Nodes.Add(String.Empty)
            End If
        End If
        MyBase.Text = displayText
    End Sub

    Private Function StripNonPrintableChars(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return value
        End If

        ' todo: use a regex instead
        value = value.Trim(New Char() {ControlChars.Cr, ControlChars.Lf, ControlChars.Tab})
        value = value.Replace(vbCr, Nothing)
        value = value.Replace(vbLf, " ")
        value = value.Replace(vbTab, Nothing)

        Return value
    End Function

#End Region

End Class


