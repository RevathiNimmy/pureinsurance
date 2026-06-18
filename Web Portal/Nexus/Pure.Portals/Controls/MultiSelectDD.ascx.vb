Imports System.Collections
Imports System.Configuration
Imports System.Data
Imports System.Linq
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Xml.Linq
Imports System.Data.Odbc

Namespace Nexus
    Partial Public Class Controls_MultiSelectDD
        Inherits System.Web.UI.UserControl

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

        End Sub
        ''' <summary>
        ''' Set the Width of the CheckBoxList
        ''' </summary>
        Public WriteOnly Property WidthCheckListBox() As Integer
            Set(ByVal value As Integer)
                chkList.Width = value
                Panel111.Width = value + 20
            End Set
        End Property
        ''' <summary>
        ''' Set the Width of the Combo
        ''' </summary>
        Public Property Width() As Integer
            Get
                Return CType(txtCombo.Width.Value, Int32)
            End Get
            Set(ByVal value As Integer)
                txtCombo.Width = value
            End Set
        End Property
        Public WriteOnly Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                txtCombo.Enabled = value
            End Set
        End Property
        ''' <summary>
        ''' Set the CheckBoxList font Size
        ''' </summary>
        Public Property fontSizeCheckBoxList() As FontUnit
            Get
                Return chkList.Font.Size
            End Get
            Set(ByVal value As FontUnit)
                chkList.Font.Size = value
            End Set
        End Property
        ''' <summary>
        ''' Set the ComboBox font Size
        ''' </summary>
        Public WriteOnly Property fontSizeTextBox() As FontUnit
            Set(ByVal value As FontUnit)
                txtCombo.Font.Size = value
            End Set
        End Property



        ''' <summary>
        ''' Add Items to the CheckBoxList.
        ''' </summary>
        ''' <param name="array">ArrayList to be added to the CheckBoxList</param>
        Public Sub AddItems(ByVal array As ArrayList)
            For i As Integer = 0 To array.Count - 1
                chkList.Items.Add(array(i).ToString())
            Next
        End Sub


        ''' <summary>
        ''' Add Items to the CheckBoxList
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <param name="nombreCampoTexto">Field Name of the OdbcDataReader to Show in the CheckBoxList</param>
        ''' <param name="nombreCampoValor">Value Field of the OdbcDataReader to be added to each Field Name (it can be the same string of the textField)</param>
        Public Sub AddItems(ByVal dr As OdbcDataReader, ByVal textField As String, ByVal valueField As String)
            ClearAll()
            Dim i As Integer = 0
            While dr.Read()
                chkList.Items.Add(dr(textField).ToString())
                chkList.Items(i).Value = i.ToString()
                i += 1
            End While
        End Sub


        ''' <summary>
        ''' Uncheck of the Items of the CheckBox
        ''' </summary>
        Public Sub unselectAllItems()
            For i As Integer = 0 To chkList.Items.Count - 1
                chkList.Items(i).Selected = False
            Next
        End Sub

        ''' <summary>
        ''' Delete all the Items of the CheckBox;
        ''' </summary>
        Public Sub ClearAll()
            txtCombo.Text = ""
            chkList.Items.Clear()
        End Sub

        ''' <summary>
        ''' Get or Set the Text shown in the Combo
        ''' </summary>
        Public Property Text() As String
            Get
                Return hidVal.Value
            End Get
            Set(ByVal value As String)
                txtCombo.Text = value
            End Set
        End Property

    End Class
End Namespace

