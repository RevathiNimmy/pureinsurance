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
Imports SiriusFS.SAM.Client
Imports Nexus.Utils

Namespace Nexus

    ''' <summary>
    ''' Risk control to show list of items from child screen
    ''' </summary>
    ''' <remarks>This is just an overloaded GridView to make use of the existing functionality.</remarks>
    Public Class EditItemGridExtended : Inherits System.Web.UI.WebControls.GridView
        Private sScreenCode As String = String.Empty
        Private sGroupField As String = String.Empty
        Private sGroupFieldValue As String = String.Empty
        Private sChildColumnNames As String = String.Empty
        Private sChildColumnHeaders As String = String.Empty
        Private sChildColumnTypes As String = String.Empty
        Private sChildColumnIsEditable As String = String.Empty
        Private bAllowDelete As Boolean = False
        Private bAllowAdd As Boolean = False
        Private bAllowComments As Boolean = False
        Private bAllowAddLink As Boolean = False
        Private sAddLinkCaption As String = String.Empty
        Private sGridParent As String = String.Empty
        Private sGridChild As String = String.Empty
        Private sParent As String = String.Empty
        Private sChild As String = String.Empty
        Private sDefaultFieldName As String = String.Empty
        Private sChildColumnCSS As String = String.Empty
        Private sParentOI As String = String.Empty
        Shared iPanelCount As Integer = 1
        Private sChildRepeaterId As String = String.Empty

        ''' <summary>
        ''' The BackOffice screen code of the child screen
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ScreenCode() As String
            Set(ByVal value As String)
                sScreenCode = value
            End Set
        End Property

        ''' <summary>
        ''' Field Name on which that child screen show records group by
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property GroupField() As String
            Set(ByVal value As String)
                sGroupField = value
            End Set
        End Property

        ''' <summary>
        ''' Pass True to diplay remove link at last column in grid
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowDelete() As Boolean
            Set(ByVal value As Boolean)
                bAllowDelete = value
            End Set
        End Property

        ''' <summary>
        ''' Pass True to diplay add button in grid header row
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowAdd() As Boolean
            Set(ByVal value As Boolean)
                bAllowAdd = value
            End Set
        End Property

        ''' <summary>
        ''' Pass True to diplay comment button in grid footer row
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowComments() As Boolean
            Set(ByVal value As Boolean)
                bAllowComments = value
            End Set
        End Property

        ''' <summary>
        ''' Pass True to diplay add link in grid footer row
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AllowAddLink() As Boolean
            Set(ByVal value As Boolean)
                bAllowAddLink = value
            End Set
        End Property

        ''' <summary>
        ''' Pass string to show caption of add link
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property AddLinkCaption() As String
            Set(ByVal value As String)
                sAddLinkCaption = value
            End Set
        End Property

        ''' <summary>
        ''' Pass backoffice default flag field name to populate specific rows
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property DefaultFieldName() As String
            Set(ByVal value As String)
                sDefaultFieldName = value
            End Set
        End Property

        ''' <summary>
        ''' Pass parent Repeater Id if used NexusChildRepeater control 
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ChildRepeaterId() As String
            Set(ByVal value As String)
                sChildRepeaterId = value
            End Set
        End Property

        ''' <summary>
        ''' Bind the data in grid 
        ''' pass bGenerateColumn = true when need to generate columns in grid
        ''' pass bSetDefault = true when need to load specific rows on the basis of Default field
        ''' </summary>
        ''' <param name="bGenerateColumn"></param>
        ''' <param name="bSetDefault"></param>
        ''' <remarks></remarks>
        Public Sub BindData(ByVal bGenerateColumn As Boolean, Optional ByVal bSetDefault As Boolean = False)
            Dim sDataModelCode As String = Current.Session(CNDataModelCode)
            Dim iInnerCount As Integer
            If bGenerateColumn Then
                Dim iOriginalColumnCount As Integer
                If Me.Columns.Count > 0 Then
                    iOriginalColumnCount = Me.Columns.Count
                End If
                For iCCount As Integer = 0 To Me.Columns.Count - 1
                    Dim bField As New TemplateField
                    Dim tempColumn As Object
                    If TypeOf Me.Columns(iCCount) Is GridTextField Then
                        tempColumn = CType(Me.Columns(iCCount), GridTextField)
                        If tempColumn.IsEditable = True Then
                            Dim txtTemplate As New TextBoxTemplate(tempColumn.DataField.Trim)
                            bField.ItemTemplate = txtTemplate
                        Else
                            Dim lblTemplate As New LabelTemplate(tempColumn.DataField.Trim)
                            bField.ItemTemplate = lblTemplate
                        End If
                    ElseIf TypeOf Me.Columns(iCCount) Is GridDropDownField Then
                        tempColumn = CType(Me.Columns(iCCount), GridDropDownField)
                        If tempColumn.IsEditable = True Then
                            Dim ddlTemplate As New DropDownListTemplate(tempColumn.ListType, tempColumn.ListName, tempColumn.datafield, tempColumn.DataTextField, tempColumn.DataValueField, tempColumn.Postback)
                            bField.ItemTemplate = ddlTemplate
                        Else
                            Dim lblTemplate As New LabelTemplate(tempColumn.DataField.Trim)
                            bField.ItemTemplate = lblTemplate
                        End If
                    End If
                    Me.Columns.Add(bField)
                    If Not String.IsNullOrEmpty(tempColumn.FieldHeaderText) Then
                        Me.Columns.Item(Me.Columns.Count - 1).HeaderText = tempColumn.FieldHeaderText.Trim
                    End If
                    If Not String.IsNullOrEmpty(tempColumn.CSSName) Then
                        Me.Columns.Item(Me.Columns.Count - 1).ItemStyle.CssClass = tempColumn.CSSName.Trim
                    End If
                Next
                For iCount As Integer = 0 To iOriginalColumnCount - 1
                    Me.Columns.RemoveAt(0)
                Next

                If bAllowDelete Then
                    Dim bField As New TemplateField
                    Dim lnkTemplate As New LinkButtonTemplate("Remove")
                    bField.ItemTemplate = lnkTemplate
                    Me.Columns.Add(bField)
                    iInnerCount = iInnerCount + 1
                End If

                If bAllowAdd Or bAllowComments Or bAllowAddLink Then
                    Me.ShowFooter = True
                    Dim fields As DataControlField() = New DataControlField(Me.Columns.Count - 1) {}
                    Me.Columns.CopyTo(fields, 0)
                    Dim oEmptyFooterRow As GridViewRow = MyBase.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal)
                    Me.InitializeRow(oEmptyFooterRow, fields)
                    iInnerCount = iInnerCount + 1
                End If
            End If

            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)
            xmlTR.Close()

            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml)

            If Not String.IsNullOrEmpty(sChildRepeaterId) Then
                Dim oMaster As ContentPlaceHolder
                Dim oNexusConfig As Nexus.Library.Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork)
                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                Dim cRepeater As Nexus.ChildRepeater = oMaster.FindControl(sChildRepeaterId)

                If Not String.IsNullOrEmpty(cRepeater.ID) Then

                    sParent = Regex.Split(cRepeater.ID, "__")(0)
                    sChild = Regex.Split(cRepeater.ID, "__")(1)
                    sGroupField = cRepeater.GroupField
                    If bGenerateColumn Then
                        sGroupFieldValue = cRepeater.GroupFieldValue
                    End If
                    sGridParent = Regex.Split(Me.ID, "__")(0)
                    sGridChild = Regex.Split(Me.ID, "__")(1)

                    'For get parent OI
                    Dim ParentXML = From GridColls In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sParent).Descendants(sChild) _
                        Where GridColls.Attribute(sGroupField).Value = sGroupFieldValue

                    sParentOI = ParentXML.ElementAt(0).Attribute("OI").Value

                    Dim FinalGrid = From FinalGrids In (From GridColls In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sParent).Descendants(sChild) _
                        Where GridColls.Attribute(sGroupField).Value = sGroupFieldValue).Descendants(sGridChild) Where FinalGrids.Attribute("US").Value <> 3 Select FinalGrids
                    If FinalGrid.Count <= 0 Then
                        'to show grid without datasource
                        Dim strXML As String = "<Temp TempVar=""No Data Found""></Temp>"
                        Dim docX1 As XDocument = XDocument.Parse(strXML)
                        Dim FinalGrid1 = _
                               From FinalGrids In docX1.Descendants("Temp")

                        Me.DataSource = FinalGrid1
                    Else
                        Me.DataSource = FinalGrid
                    End If

                End If
            Else

                sGridParent = Regex.Split(Me.ID, "__")(0)
                sGridChild = Regex.Split(Me.ID, "__")(1)

                If bSetDefault Then

                    Dim ParentXML = From GridColls In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sGridParent) _
                        Where GridColls.Attribute("US").Value <> 3

                    sParentOI = ParentXML.ElementAt(0).Attribute("OI").Value

                    Dim FinalGrid = _
                    From FinalGrids In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sGridParent).Descendants(sGridChild) Select FinalGrids _
                    Where FinalGrids.Attribute("US").Value <> 3 And FinalGrids.Attribute(sDefaultFieldName).Value = True

                    If FinalGrid.Count <= 0 Then
                        'to show grid without datasource
                        Dim strXML As String = "<Temp TempVar=""No Data Found""></Temp>"
                        Dim docX1 As XDocument = XDocument.Parse(strXML)
                        Dim FinalGrid1 = _
                               From FinalGrids In docX1.Descendants("Temp")

                        Me.DataSource = FinalGrid1
                    Else
                        Me.DataSource = FinalGrid
                    End If
                    Dim ManualGrid = _
                        From FinalGrids In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sGridParent).Descendants(sGridChild) Select FinalGrids _
                        Where FinalGrids.Attribute("US").Value <> 3 And FinalGrids.Attribute(sDefaultFieldName).Value = False
                    Dim strQuery As String
                    For Each row In ManualGrid
                        strQuery = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sGridParent & "/" & sGridChild & "[@OI='" & row.Attribute("OI").Value & "']"
                        UpdateXML(strQuery, sGridChild, "US", 3, True)
                    Next
                    bSetDefault = False
                Else

                    Dim ParentXML = From GridColls In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sGridParent) _
                        Where GridColls.Attribute("US").Value <> 3
                    If ParentXML IsNot Nothing Then
                        If ParentXML.Count > 0 Then
                            sParentOI = ParentXML.ElementAt(0).Attribute("OI").Value
                        End If
                    End If
                    Dim FinalGrid = _
                    From FinalGrids In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sGridParent).Descendants(sGridChild) Select FinalGrids _
                    Where FinalGrids.Attribute("US").Value <> 3

                    If FinalGrid.Count <= 0 Then
                        'to show grid without datasource
                        Dim strXML As String = "<Temp TempVar=""No Data Found""></Temp>"
                        Dim docX1 As XDocument = XDocument.Parse(strXML)
                        Dim FinalGrid1 = _
                               From FinalGrids In docX1.Descendants("Temp")

                        Me.DataSource = FinalGrid1
                    Else
                        Me.DataSource = FinalGrid
                    End If
                End If
            End If
            Me.DataBind()
        End Sub

        ''' <summary>
        ''' init event of grid 
        ''' Always generate column at the time of initialisation
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditItemGridExtended_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            BindData(True)
        End Sub

        ''' <summary>
        ''' Handles the row command event of grid
        ''' Fires delete item event when click on remove link
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditItemGridExtended_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Me.RowCommand
            Select Case LCase(e.CommandName)
                Case "remove"
                    DeleteItem(e.CommandArgument)
            End Select

        End Sub

        ''' <summary>
        ''' Handles the child item deletion event
        ''' </summary>
        ''' <param name="v_sOI">Dataset identifier of the selected child item</param>
        ''' <remarks></remarks>
        Public Sub DeleteItem(ByVal v_sOI As String)
            Dim sDataModelCode As String = Current.Session(CNDataModelCode)
            Dim strQuery As String
            If Not String.IsNullOrEmpty(sChild) Then
                strQuery = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sParent & "/" & sChild & "/" & sGridChild & "[@OI='" & v_sOI & "']"
                UpdateXML(strQuery, sGridChild, "US", 3, True)
            Else
                strQuery = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sGridParent & "/" & sGridChild & "[@OI='" & v_sOI & "']"
                UpdateXML(strQuery, sGridChild, "US", 3, True)
            End If
            'rebind the grid
            BindData(False)
        End Sub
        ''' <summary>
        ''' Handles the row created event of grid
        ''' Add footer and header items in grid based on properties
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditItemGridExtended_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowCreated
            If Current.Session(CNMode) <> Mode.View Then
                Select Case e.Row.RowType
                    Case DataControlRowType.Header
                        If bAllowAdd Then
                            Dim obtnAdd As New Button
                            obtnAdd.Text = "Add"
                            obtnAdd.CssClass = "submit"
                            obtnAdd.CausesValidation = False
                            obtnAdd.ToolTip = sParentOI
                            obtnAdd.Attributes.Add("runat", "server")
                            AddHandler obtnAdd.Click, AddressOf AddClicked
                            e.Row.Cells(e.Row.Cells.Count - 1).Controls.Add(obtnAdd)
                        End If
                    Case DataControlRowType.Footer
                        If bAllowComments Then
                            iPanelCount = iPanelCount + 1
                            Dim pnlComments As New Panel
                            pnlComments.ID = "pnlTest" & iPanelCount
                            pnlComments.Attributes.Add("runat", "server")
                            pnlComments.Attributes.Add("title", "Comments")
                            e.Row.Cells(e.Row.Cells.Count - 1).Controls.Add(pnlComments)
                            Dim obtnComment As New HtmlInputButton
                            obtnComment.ID = "_btnComment" & iPanelCount
                            obtnComment.Value = "Comments"
                            obtnComment.Attributes.Add("class", "thickbox , submit")
                            obtnComment.CausesValidation = False
                            obtnComment.Attributes.Add("alt", "#TB_inline?height=200&width=800&inlineId=" & pnlComments.ClientID)
                            e.Row.Cells(e.Row.Cells.Count - 1).Controls.Add(obtnComment)

                            Dim txtComment As New TextBox
                            txtComment.ID = "txtComments" & iPanelCount
                            txtComment.TextMode = TextBoxMode.MultiLine
                            txtComment.Height = 60
                            txtComment.Width = 150
                            pnlComments.Controls.Add(txtComment)
                            Dim obtnOk As New Button
                            obtnOk.ID = "btnOK" & iPanelCount
                            obtnOk.Text = "Ok"
                            obtnOk.Attributes.Add("onclick", "self.tb_remove(); return false;")
                            obtnOk.CausesValidation = False
                            pnlComments.Controls.Add(obtnOk)
                            pnlComments.Attributes.Add("Style", "display: none")

                        End If
                        If bAllowAddLink Then
                            Dim obtnAdd As New LinkButton
                            obtnAdd.Text = sAddLinkCaption
                            'obtnAdd.CssClass = "submit"
                            obtnAdd.CausesValidation = False
                            obtnAdd.Attributes.Add("runat", "server")
                            obtnAdd.ToolTip = sParentOI
                            AddHandler obtnAdd.Click, AddressOf AddClicked
                            e.Row.Cells(0).Controls.Add(obtnAdd)
                        End If
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Handles the Add event in Grid either add button or add link clicked in grid
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub AddClicked(ByVal sender As Object, ByVal e As EventArgs)

            Dim oDataSet As DataSetControl.Application
            Dim oQuote As NexusProvider.Quote = Current.Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim oDoc As New XmlDocument

            oDoc.Load(xmlTR)
            xmlTR.Close()

            Dim sDataSetDefinition As String = GetDataSetDefinition(Current.Session(CNDataModelCode))
            oDataSet = New DataSetControl.Application
            oDataSet.LoadFromXML(sDataSetDefinition, oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

            For iCount As Integer = 0 To Me.Rows.Count - 1
                For cCount As Integer = 0 To Me.Columns.Count - 1
                    Dim strColumnName As String = String.Empty
                    If TypeOf DirectCast(DirectCast(DirectCast(Me.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate) Is TextBoxTemplate Then
                        strColumnName = DirectCast(DirectCast(DirectCast(DirectCast(Me.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate), Nexus.TextBoxTemplate).ColumnName
                    ElseIf TypeOf DirectCast(DirectCast(DirectCast(Me.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate) Is DropDownListTemplate Then
                        strColumnName = DirectCast(DirectCast(DirectCast(DirectCast(Me.Columns(cCount), System.Web.UI.WebControls.DataControlField), System.Web.UI.WebControls.TemplateField).ItemTemplate, System.Web.UI.ITemplate), Nexus.DropDownListTemplate).ColumnName
                    End If
                    If Not String.IsNullOrEmpty(strColumnName) Then
                        Dim oCtrl As Control = Me.Rows(iCount).FindControl(strColumnName)
                        If oCtrl IsNot Nothing Then
                            If TypeOf oCtrl Is TextBox Then
                                'Save the value in XML
                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(strColumnName, "__"), CType(oCtrl, TextBox).Text, CType(oCtrl, TextBox).ToolTip)
                            ElseIf TypeOf oCtrl Is DropDownList Then
                                'Save the value in XML
                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(strColumnName, "__"), CType(oCtrl, DropDownList).SelectedValue, CType(oCtrl, DropDownList).ToolTip)
                            End If
                        End If
                    End If
                Next
            Next
            oDataSet.ReturnAsXML(oQuote.Risks(Current.Session(CNCurrentRiskKey)).XMLDataset)

            Current.Session(CNQuote) = oQuote
            CreateElementFromXML(sScreenCode, sender.ToolTip, sGridParent, sGridChild, True)

            sDataSetDefinition = Nothing

            BindData(False)
        End Sub

        Private Sub EditItemGridExtended_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Me.RowDeleting
            'Have to refresh the page when delete event is fired because,
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Path, False)
        End Sub

        Public Sub SetDefault()
            BindData(False, True)
        End Sub
        'do not delete below lines
        Private Sub EditItemGridExtended_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Me.RowEditing

        End Sub
    End Class

    ''' <summary>
    ''' GridTextField class to add text box in grid with properties  
    ''' </summary>
    ''' <remarks>This is just a implementaion layer class</remarks>
    Public Class GridTextField : Inherits TemplateField
        
        Private bIsEditable As Boolean
        ''' <summary>
        ''' IsEditable Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsEditable() As Boolean
            Get
                Return bIsEditable
            End Get
            Set(ByVal value As Boolean)
                bIsEditable = value
            End Set
        End Property


        Dim sDataField As String
        ''' <summary>
        ''' DataField Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataField() As String
            Get
                Return sDataField
            End Get
            Set(ByVal value As String)
                sDataField = value
            End Set
        End Property

        Dim sCSSName As String
        ''' <summary>
        ''' CSSName Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CSSName() As String
            Get
                Return sCSSName
            End Get
            Set(ByVal value As String)
                sCSSName = value
            End Set
        End Property

        Dim sFieldHeaderText As String
        ''' <summary>
        ''' FieldHeaderText Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FieldHeaderText() As String
            Get
                Return sFieldHeaderText
            End Get
            Set(ByVal value As String)
                sFieldHeaderText = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' GridDropDownField class to add DropDown in grid with properties
    ''' </summary>
    ''' <remarks>This is just a implementaion layer class</remarks>
    Public Class GridDropDownField : Inherits TemplateField

        Private oListType As NexusProvider.ListType

        ''' <summary>
        ''' Define the type of list that will be retrived from SAM for matching the data field attribute
        ''' </summary>
        ''' <value>Uses the ListType enumerator, only 3 possible values, GIS, UserDefined and PMLookup</value>
        ''' <remarks></remarks>
        Public Property ListType() As NexusProvider.ListType
            Get
                Return oListType
            End Get
            Set(ByVal value As NexusProvider.ListType)
                oListType = value
            End Set
        End Property

        Private sListName As String
        ''' <summary>
        ''' ListName Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListName() As String
            Get
                Return sListName
            End Get
            Set(ByVal value As String)
                sListName = value
            End Set
        End Property

        Private bIsEditable As Boolean
        ''' <summary>
        ''' IsEditable Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsEditable() As Boolean
            Get
                Return bIsEditable
            End Get
            Set(ByVal value As Boolean)
                bIsEditable = value
            End Set
        End Property

        Private sDataTextField As String
        ''' <summary>
        ''' DataTextField Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataTextField() As String
            Get
                Return sDataTextField
            End Get
            Set(ByVal value As String)
                sDataTextField = value
            End Set
        End Property

        Private sDataValueField As String
        ''' <summary>
        ''' DataValueField Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataValueField() As String
            Get
                Return sDataValueField
            End Get
            Set(ByVal value As String)
                sDataValueField = value
            End Set
        End Property

        Private bPostBack As Boolean
        ''' <summary>
        ''' PostBack Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PostBack() As Boolean
            Get
                Return bPostBack
            End Get
            Set(ByVal value As Boolean)
                bPostBack = value
            End Set
        End Property

        Dim sCSSName As String
        ''' <summary>
        ''' CSSName Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property CSSName() As String
            Get
                Return sCSSName
            End Get
            Set(ByVal value As String)
                sCSSName = value
            End Set
        End Property

        Dim sDataField As String
        ''' <summary>
        ''' DataField Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataField() As String
            Get
                Return sDataField
            End Get
            Set(ByVal value As String)
                sDataField = value
            End Set
        End Property

        Dim sFieldHeaderText As String
        ''' <summary>
        ''' FieldHeaderText Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FieldHeaderText() As String
            Get
                Return sFieldHeaderText
            End Get
            Set(ByVal value As String)
                sFieldHeaderText = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' LabelTemplate class to add Label in grid dynamically
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LabelTemplate
        Implements ITemplate
        Private m_ColumnName As String
        ''' <summary>
        '''  column name to set id of control (as nexus standard objectname__propertyname)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ColumnName() As String
            Get
                Return m_ColumnName
            End Get
            Set(ByVal value As String)
                m_ColumnName = value
            End Set
        End Property
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ColumnName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ColumnName As String)
            Me.ColumnName = ColumnName
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ThisColumn"></param>
        ''' <remarks></remarks>
        Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
                Implements ITemplate.InstantiateIn
            Dim gItem As New Label
            gItem.ID = ColumnName
            AddHandler gItem.DataBinding, AddressOf gItem_DataBinding
            gItem.CssClass = "fullwidth"
            ThisColumn.Controls.Add(gItem)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub gItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim gItem As Label = DirectCast(sender, Label)
            Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            Dim sPropertyName = Regex.Split(ColumnName, "__")(1)
            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI") IsNot Nothing Then
                Dim CurrentOIDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value
                gItem.ToolTip = CurrentOIDataItem.ToString
            Else
                gItem.Visible = False
            End If
            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(sPropertyName) IsNot Nothing Then
                Dim CurrentDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(sPropertyName).Value
                If Not CurrentDataItem Is DBNull.Value Then
                    gItem.Text = CurrentDataItem.ToString
                End If
            End If
        End Sub
        '
    End Class

    ''' <summary>
    ''' ButtonTemplate class to add Button in grid dynamically
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ButtonTemplate
        Implements ITemplate
        Private m_ColumnName As String
        ''' <summary>
        '''  column name to set id of control (as nexus standard objectname__propertyname)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ColumnName() As String
            Get
                Return m_ColumnName
            End Get
            Set(ByVal value As String)
                m_ColumnName = value
            End Set
        End Property
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ColumnName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ColumnName As String)
            Me.ColumnName = ColumnName
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ThisColumn"></param>
        ''' <remarks></remarks>
        Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
                Implements ITemplate.InstantiateIn
            Dim gItem As New Button
            gItem.ID = ColumnName
            AddHandler gItem.DataBinding, AddressOf gItem_DataBinding
            ThisColumn.Controls.Add(gItem)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub gItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim gItem As Button = DirectCast(sender, Button)
            'oEmptyFooterRow = MyBase.CreateRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal)
            'Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            gItem.Text = ColumnName
            gItem.Visible = False
            'Dim CurrentDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(ColumnName).Value
            'If Not CurrentDataItem Is DBNull.Value Then
            '    gItem.Text = CurrentDataItem.ToString
            'End If
        End Sub
        '
    End Class

    ''' <summary>
    ''' TextBoxTemplate class to add TextBox in grid dynamically
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TextBoxTemplate
        Implements ITemplate
        Private m_ColumnName As String
        ''' <summary>
        '''  column name to set id of control (as nexus standard objectname__propertyname)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ColumnName() As String
            Get
                Return m_ColumnName
            End Get
            Set(ByVal value As String)
                m_ColumnName = value
            End Set
        End Property
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ColumnName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ColumnName As String)
            Me.ColumnName = ColumnName
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ThisColumn"></param>
        ''' <remarks></remarks>
        Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
                Implements ITemplate.InstantiateIn
            Dim gItem As New TextBox
            gItem.ID = ColumnName
            AddHandler gItem.DataBinding, AddressOf gItem_DataBinding
            gItem.CssClass = "fullwidth"
            ThisColumn.Controls.Add(gItem)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub gItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim gItem As TextBox = DirectCast(sender, TextBox)
            Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            Dim sPropertyName = Regex.Split(ColumnName, "__")(1)
            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI") IsNot Nothing Then
                Dim CurrentOIDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value
                gItem.ToolTip = CurrentOIDataItem.ToString
            Else
                gItem.Visible = False
            End If
            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(sPropertyName) IsNot Nothing Then
                Dim CurrentDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(sPropertyName).Value
                If Not CurrentDataItem Is DBNull.Value Then
                    gItem.Text = CurrentDataItem.ToString
                    gItem.Attributes.Add("onblur", "javascript:if(document.getElementById('" & gItem.ClientID & "').value != '" & CurrentDataItem.ToString & "'){document.getElementById('" & gItem.ClientID & "').setAttribute('Class', 'updated fullwidth');}")
                End If
            End If
        End Sub

        '
    End Class
    ''' <summary>
    ''' DropDownListTemplate class to add DropDownList in grid dynamically
    ''' </summary>
    ''' <remarks></remarks>
    Public Class DropDownListTemplate
        Implements ITemplate
        Private m_ColumnName As String
        ''' <summary>
        '''  column name to set id of control (as nexus standard objectname__propertyname)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ColumnName() As String
            Get
                Return m_ColumnName
            End Get
            Set(ByVal value As String)
                m_ColumnName = value
            End Set
        End Property

        Private m_DataField As String
        ''' <summary>
        ''' DataField Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataField() As String
            Get
                Return m_DataField
            End Get
            Set(ByVal value As String)
                m_DataField = value
            End Set
        End Property

        Private m_DataFieldValue As String
        ''' <summary>
        ''' DataFieldValue Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property DataFieldValue() As String
            Get
                Return m_DataFieldValue
            End Get
            Set(ByVal value As String)
                m_DataFieldValue = value
            End Set
        End Property

        Private m_ListName As String
        ''' <summary>
        ''' ListName Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListName() As String
            Get
                Return m_ListName
            End Get
            Set(ByVal value As String)
                m_ListName = value
            End Set
        End Property

        Private m_ListType As NexusProvider.ListType
        ''' <summary>
        ''' ListType Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ListType() As NexusProvider.ListType
            Get
                Return m_ListType
            End Get
            Set(ByVal value As NexusProvider.ListType)
                m_ListType = value
            End Set
        End Property

        Private m_PostBack As Boolean = False
        ''' <summary>
        ''' PostBack Property
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PostBack() As Boolean
            Get
                Return m_PostBack
            End Get
            Set(ByVal value As Boolean)
                m_PostBack = value
            End Set
        End Property

        Public Sub New()
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ListType"></param>
        ''' <param name="ListName"></param>
        ''' <param name="ColumnName"></param>
        ''' <param name="DataField"></param>
        ''' <param name="DataFieldValue"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ListType As NexusProvider.ListType, ByVal ListName As String, ByVal ColumnName As String, ByVal DataField As String, ByVal DataFieldValue As String)
            Me.ColumnName = ColumnName
            Me.ListName = ListName
            Me.ListType = ListType
            Me.DataField = DataField
            Me.DataFieldValue = DataFieldValue
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ListType"></param>
        ''' <param name="ListName"></param>
        ''' <param name="ColumnName"></param>
        ''' <param name="DataField"></param>
        ''' <param name="DataFieldValue"></param>
        ''' <param name="PostBack"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ListType As NexusProvider.ListType, ByVal ListName As String, ByVal ColumnName As String, ByVal DataField As String, ByVal DataFieldValue As String, ByVal PostBack As Boolean)
            Me.ColumnName = ColumnName
            Me.ListName = ListName
            Me.ListType = ListType
            Me.DataField = DataField
            Me.DataFieldValue = DataFieldValue
            Me.PostBack = PostBack
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="ThisColumn"></param>
        ''' <remarks></remarks>
        Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
                Implements ITemplate.InstantiateIn
            Dim gItem As New DropDownList
            gItem.ID = ColumnName
            AddHandler gItem.DataBinding, AddressOf gItem_DataBinding
            AddHandler gItem.DataBound, AddressOf gItem_DataBound
            gItem.AutoPostBack = PostBack
            If PostBack Then
                AddHandler gItem.SelectedIndexChanged, AddressOf gItem_SelectedIndexChanged
            End If
            gItem.CssClass = "fullwidth"
            ThisColumn.Controls.Add(gItem)
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub gItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim gItem As DropDownList = DirectCast(sender, DropDownList)
            Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            Dim sPropertyName = Regex.Split(ColumnName, "__")(1)
            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI") IsNot Nothing Then
                Dim CurrentOIDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value
                gItem.ToolTip = CurrentOIDataItem.ToString
            Else
                gItem.Visible = False
            End If
            '
            If String.IsNullOrEmpty(gItem.DataMember) Then
                gItem.DataSource = GetList(ListType, ListName)
            Else
                gItem.DataSource = GetList(ListType, gItem.DataMember)
            End If
            gItem.DataTextField = DataField
            gItem.DataValueField = DataFieldValue

        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub gItem_DataBound(ByVal sender As Object, ByVal e As EventArgs)
            Dim gItem As DropDownList = DirectCast(sender, DropDownList)
            Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            Dim sPropertyName = Regex.Split(ColumnName, "__")(1)

            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(sPropertyName) IsNot Nothing Then
                Dim CurrentDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute(sPropertyName).Value
                If Not CurrentDataItem Is DBNull.Value Then
                    If CurrentDataItem.ToString <> "0" Then
                        gItem.SelectedValue = CurrentDataItem.ToString
                    End If
                End If
            End If
        End Sub
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gItem_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim TempGrid As Object
            If TypeOf sender.parent.parent.BindingContainer Is EditItemGrid Then
                TempGrid = CType(sender.parent.parent.BindingContainer, EditItemGrid)
            ElseIf TypeOf sender.parent.parent.BindingContainer Is EditItemGridExtended Then
                TempGrid = CType(sender.parent.parent.BindingContainer, EditItemGridExtended)
            End If
            Dim gItem As DropDownList = CType(sender, DropDownList)
            Dim sDataModelCode As String = Current.Session(CNDataModelCode)
            Dim sParent As String = Regex.Split(sender.ID, "__")(0)
            Dim sChild As String = Regex.Split(sender.ID, "__")(1)
            Dim strQuery As String = "//" & sParent & "[@OI='" & gItem.ToolTip & "']"
            UpdateXML(strQuery, sParent, sChild, gItem.SelectedItem.Value)
            If TempGrid IsNot Nothing Then
                TempGrid.BindData(False, False)
            End If
        End Sub
        ''' <summary>
        ''' Retrives the List of the datafield from the specified list from SAM
        ''' </summary>
        ''' <param name="oListType">List Type</param>
        ''' <param name="sListName">List Name</param>
        ''' <returns>List</returns>
        ''' <remarks></remarks>
        Private Function GetList(ByVal oListType As NexusProvider.ListType, ByVal sListName As String) As NexusProvider.LookupListCollection

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection
            Try
                oList = oWebService.GetList(oListType, sListName, True, False)
            Finally
                oWebService = Nothing
            End Try

            Return oList

        End Function
        '
    End Class
    ''' <summary>
    ''' LinkButtonTemplate class to add link button in grid dynamically
    ''' </summary>
    ''' <remarks></remarks>
    Public Class LinkButtonTemplate
        Implements ITemplate
        Private m_ColumnName As String
        ''' <summary>
        ''' column name to set id of control (as nexus standard objectname__propertyname)
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ColumnName() As String
            Get
                Return m_ColumnName
            End Get
            Set(ByVal value As String)
                m_ColumnName = value
            End Set
        End Property
        ''' <summary>
        ''' default Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub
        ''' <summary>
        ''' override Constructor with column name
        ''' </summary>
        ''' <param name="ColumnName"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal ColumnName As String)
            Me.ColumnName = ColumnName
        End Sub
        ''' <summary>
        ''' initialise the control
        ''' </summary>
        ''' <param name="ThisColumn"></param>
        ''' <remarks></remarks>
        Private Sub InstantiateIn(ByVal ThisColumn As System.Web.UI.Control) _
                Implements ITemplate.InstantiateIn
            Dim gItem As New LinkButton
            gItem.ID = ColumnName
            AddHandler gItem.DataBinding, AddressOf gItem_DataBinding
            ThisColumn.Controls.Add(gItem)
        End Sub
        ''' <summary>
        ''' bind the control with data source
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub gItem_DataBinding(ByVal sender As Object, ByVal e As EventArgs)
            Dim gItem As LinkButton = DirectCast(sender, LinkButton)
            Dim CurrentRow As GridViewRow = DirectCast(gItem.NamingContainer, GridViewRow)
            If CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI") IsNot Nothing Then
                gItem.Text = ColumnName
                gItem.CommandName = ColumnName
                Dim CurrentDataItem As Object = CType(CurrentRow.DataItem, System.Xml.Linq.XElement).Attribute("OI").Value
                If Not CurrentDataItem Is DBNull.Value Then
                    gItem.CommandArgument = CurrentDataItem.ToString
                End If
            Else
                gItem.Visible = False
            End If
        End Sub

        '
    End Class
End Namespace
