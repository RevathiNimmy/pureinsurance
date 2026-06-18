Imports System.Web.HttpContext

Namespace Nexus

    Public Class RiskContainer : Inherits WebControls.PlaceHolder

        Private sScreenCode As String

        Private sOI As String
        Private sParentElement As String
        Private sChildElement As String

        ''' <summary>
        ''' The BackOffice screen code that the risk container represents
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ScreenCode() As String
            Get
                Return sScreenCode
            End Get
            Set(ByVal value As String)
                sScreenCode = value
            End Set
        End Property

        ''' <summary>
        ''' Determines the mode the risk container is in, the child item
        ''' that the risk container displays can either be an add or edit
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Mode() As ChildMode
            Get
                Dim o As Object = ViewState("Mode")
                If (o IsNot Nothing) Then
                    Return CType(o, ChildMode)
                End If
                Return ChildMode.Add
            End Get
            Set(ByVal value As ChildMode)
                ViewState.Add("Mode", value)
            End Set
        End Property

        ''' <summary>
        ''' The SAM DataSet identifier for the child item
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property OI() As String
            Get
                Dim o As Object = ViewState("OI")
                If (o IsNot Nothing) Then
                    Return CType(o, String)
                End If
                Return String.Empty
            End Get
            Set(ByVal value As String)
                ViewState.Add("OI", value)
            End Set
        End Property

        ''' <summary>
        ''' The name of the parent element within the dataset, this ensures the
        ''' child item is add in the correct place within the dataset.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ParentElement() As String
            Get
                Return sParentElement
            End Get
            Set(ByVal value As String)
                sParentElement = value
            End Set
        End Property

        ''' <summary>
        ''' The name of the child element with the dataset,
        ''' we need to know what type of element to create
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChildElement() As String
            Get
                Return sChildElement
            End Get
            Set(ByVal value As String)
                sChildElement = value
            End Set
        End Property

        Public Enum ChildMode
            Add = 0
            Edit = 1
        End Enum

    End Class

End Namespace