Imports System.Web.HttpContext
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Resources
Imports System.Xml
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Linq
Imports System.Xml.Linq
Imports Microsoft.VisualBasic

Namespace Nexus

    ''' <summary>
    ''' Risk control to show list of items from child screen on single screen
    ''' </summary>
    ''' <remarks>This is just an overloaded Repeater to make use of the existing functionality.</remarks>
    Public Class ChildRepeater : Inherits System.Web.UI.WebControls.Repeater

        Private sGroupField As String = String.Empty
        ''' <summary>
        ''' Field Name on which that child screen show records group by
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GroupField() As String
            Get
                Return sGroupField
            End Get
            Set(ByVal value As String)
                sGroupField = value
            End Set
        End Property

        Private sGroupFieldValue As String = String.Empty
        ''' <summary>
        ''' Field Value on which that child screen show records group by
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property GroupFieldValue() As String
            Get
                Return sGroupFieldValue
            End Get
            Set(ByVal value As String)
                sGroupFieldValue = value
            End Set
        End Property

        Private Sub ChildRepeater_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            BindData()
        End Sub

        ''' <summary>
        ''' Bind the Repeater control with child xml  
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub BindData()
            Dim sDataModelCode As String = Current.Session(CNDataModelCode)

            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)
            xmlTR.Close()

            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml)
            Dim v_sParent, v_sChild As String
            'Split the Parent and Child names from ID
            v_sParent = Regex.Split(Me.ID, "__")(0)
            v_sChild = Regex.Split(Me.ID, "__")(1)
            Dim RepeaterColl = _
                   From RepeaterColls In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(v_sParent).Descendants(v_sChild) Select RepeaterColls
            'Bind Repeater
            Me.DataSource = RepeaterColl

            'this code may be use in future do not delete
            'Dim btnSave As New Button
            'btnSave.Text = "Save"
            'btnSave.CssClass = "submit"
            'AddHandler btnSave.Click, AddressOf SaveClicked
            'Me.FooterTemplate = New RepeaterFooterTemplate(ListItemType.Footer, btnSave, True, True)

            Me.DataBind()

        End Sub

        Private Sub ChildRepeater_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Me.ItemCommand

        End Sub

        ''' <summary>
        ''' Event occur for every item in repeater control 
        ''' setting the child item group field value 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub ChildRepeater_ItemCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles Me.ItemCreated

            If e.Item.ItemType = ListItemType.Footer Then

            Else
                Dim item As RepeaterItem = e.Item
                GroupFieldValue = CType(item.DataItem, System.Xml.Linq.XElement).Attribute(GroupField).Value
                'item.ID = CType(item.DataItem, System.Xml.Linq.XElement).Attribute(GroupField).Value
            End If
        End Sub

        Private Sub ChildRepeater_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            
        End Sub

        Private Sub ChildRepeater_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        End Sub
        'this code may be use for future
        'Public Sub SaveClicked(ByVal sender As Object, ByVal e As EventArgs)
        '    For Each oItem As RepeaterItem In Me.Items
        '        If oItem.ItemType = ListItemType.Item Then


        '        End If
        '    Next
        'End Sub

        ''' <summary>
        ''' this procedure call child grid binddata with default flag true
        ''' call this procedure when we need to load default items(default property true for child records) 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetDefault()
            For Each oItem As RepeaterItem In Me.Items
                For Each Control In oItem.Controls
                    If Control.GetType.Name = "EditItemGrid" Then
                        CType(Control, EditItemGrid).BindData(False, True)
                    ElseIf Control.GetType.Name = "EditItemGridExtended" Then
                        CType(Control, EditItemGridExtended).BindData(False, True)
                    End If
                Next
            Next
        End Sub
    End Class

    ''' <summary>
    ''' Class RepeaterFooterTemplate 
    ''' to add footer items 
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RepeaterFooterTemplate
        Implements ITemplate
        Shared itemcount As Integer = 0
        Dim TemplateType As ListItemType

        Dim btnSave As Button
        Dim bShowDefault As Boolean
        Dim bShowExit As Boolean
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="type"></param>
        ''' <param name="btnSave"></param>
        ''' <param name="bShowDefault"></param>
        ''' <param name="bShowExit"></param>
        ''' <remarks></remarks>
        Sub New(ByVal type As ListItemType, ByVal btnSave As Button, ByVal bShowDefault As Boolean, ByVal bShowExit As Boolean)
            TemplateType = type
            Me.bShowDefault = bShowDefault
            Me.bShowExit = bShowExit
            Me.btnSave = btnSave
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="container"></param>
        ''' <remarks></remarks>
        Sub InstantiateIn(ByVal container As Control) _
           Implements ITemplate.InstantiateIn

            Select Case TemplateType
                ' all the cases are not needed. Just want to work with footer template 
                Case ListItemType.Header

                Case ListItemType.Item

                Case ListItemType.AlternatingItem

                Case ListItemType.Footer

                    If bShowExit Then
                        Dim btnExit As New Button
                        btnExit.Text = "Exit"
                        btnExit.CssClass = "submit"
                        container.Controls.Add(btnExit)
                    End If
                    If btnSave IsNot Nothing Then
                        container.Controls.Add(btnSave)
                    End If
                    If bShowDefault Then
                        Dim btnShowDefault As New Button
                        btnShowDefault.Text = "Default Excesses"
                        btnShowDefault.CssClass = "submit"
                        container.Controls.Add(btnShowDefault)
                    End If
            End Select
            itemcount += 1
        End Sub
    End Class
   
End Namespace