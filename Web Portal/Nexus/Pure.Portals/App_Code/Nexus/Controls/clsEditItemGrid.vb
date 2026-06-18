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

Namespace Nexus

    ''' <summary>
    ''' Risk control to show list of items from child screen
    ''' </summary>
    ''' <remarks>This is just an overloaded GridView to make use of the existing functionality.</remarks>
    Public Class EditItemGrid : Inherits System.Web.UI.WebControls.GridView

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
        ''' Pass the child columns names with comma seprate string
        ''' these column will be added in grid  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChildColumnNames() As String
            Get
                Return sChildColumnNames
            End Get
            Set(ByVal value As String)
                sChildColumnNames = value
            End Set
        End Property

        ''' <summary>
        ''' Pass the child columns headers with comma seprate string
        ''' these headers will be displayed in grid  
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ChildColumnHeaders() As String
            Set(ByVal value As String)
                sChildColumnHeaders = value
            End Set
        End Property

        ''' <summary>
        ''' Pass the child columns types with comma seprate string
        ''' these column types decide which control will be added in grid when editable is true  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChildColumnTypes() As String
            Get
                Return sChildColumnTypes
            End Get
            Set(ByVal value As String)
                sChildColumnTypes = value
            End Set
        End Property

        ''' <summary>
        ''' Pass the child columns Editable or not with comma seprate string(0 for non editable and 1 for editable)
        ''' these column will be added as editable in grid  
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ChildColumnIsEditable() As String
            Get
                Return sChildColumnIsEditable
            End Get
            Set(ByVal value As String)
                sChildColumnIsEditable = value
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
        ''' Pass the child columns CSS Names with comma seprate string
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ChildColumnCSS() As String
            Set(ByVal value As String)
                sChildColumnCSS = value
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
                Dim arrColumnNames As String()
                Dim arrColumnHeaders As String()
                Dim arrColumnTypes As String()
                Dim arrColumnEditable As String()
                Dim arrColumnCSS As String()

                arrColumnNames = sChildColumnNames.Split(",")
                arrColumnHeaders = sChildColumnHeaders.Split(",")
                arrColumnTypes = sChildColumnTypes.Split(",")
                arrColumnEditable = sChildColumnIsEditable.Split(",")
                arrColumnCSS = sChildColumnCSS.Split(",")

                For Each Str As String In arrColumnNames
                    Dim bField As New TemplateField
                    If arrColumnTypes(iInnerCount).Trim.ToLower = "text" Then
                        If arrColumnEditable(iInnerCount).Trim = "1" Then
                            Dim txtTemplate As New TextBoxTemplate(Str.Trim)
                            bField.ItemTemplate = txtTemplate
                        Else
                            Dim lblTemplate As New LabelTemplate(Str.Trim)
                            bField.ItemTemplate = lblTemplate
                        End If
                    ElseIf Left(arrColumnTypes(iInnerCount).Trim, 4).ToLower = "list" Then
                        If arrColumnEditable(iInnerCount).Trim = "1" Then
                            Dim arrListDetails As String() = arrColumnTypes(iInnerCount).Split(":")
                            Dim oType As NexusProvider.ListType
                            Dim ddlTemplate As Object
                            If arrListDetails(1).Trim.ToLower = "gis" Then
                                oType = NexusProvider.ListType.GIS
                            ElseIf arrListDetails(1).Trim.ToLower = "pmlookup" Then
                                oType = NexusProvider.ListType.PMLookup
                            ElseIf arrListDetails(1).Trim.ToLower = "userdefined" Then
                                oType = NexusProvider.ListType.UserDefined
                            End If
                            If arrListDetails.Length > 5 Then
                                ddlTemplate = New DropDownListTemplate(oType, arrListDetails(2).Trim, Str.Trim, arrListDetails(3).Trim, arrListDetails(4).Trim, arrListDetails(5).Split("=")(1))
                                bField.ItemTemplate = ddlTemplate
                            Else
                                ddlTemplate = New DropDownListTemplate(oType, arrListDetails(2).Trim, Str.Trim, arrListDetails(3).Trim, arrListDetails(4).Trim)
                                bField.ItemTemplate = ddlTemplate
                            End If
                        Else
                            Dim lblTemplate As New LabelTemplate(Str.Trim)
                            bField.ItemTemplate = lblTemplate
                        End If
                    End If
                    Me.Columns.Add(bField)
                    Me.Columns.Item(iInnerCount).HeaderText = arrColumnHeaders(iInnerCount)
                    If Not String.IsNullOrEmpty(sChildColumnCSS) Then
                        If arrColumnCSS.Length >= iInnerCount Then
                            Me.Columns.Item(iInnerCount).ItemStyle.CssClass = arrColumnCSS(iInnerCount)
                        End If
                    End If
                    iInnerCount = iInnerCount + 1
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

            If TypeOf Me.Parent.BindingContainer Is Nexus.ChildRepeater Then
                Dim cRepeater As Nexus.ChildRepeater = CType(Me.Parent.BindingContainer, Nexus.ChildRepeater)

                If Not String.IsNullOrEmpty(cRepeater.ID) Then

                    sParent = Regex.Split(cRepeater.ID, "__")(0)
                    sChild = Regex.Split(cRepeater.ID, "__")(1)
                    sGroupField = cRepeater.GroupField
                    Dim cRepeaterItem As RepeaterItem = CType(Me.Parent, RepeaterItem)
                    sGroupFieldValue = cRepeaterItem.ID

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
                'Split the Parent and Child names from ID
                sGridParent = Regex.Split(Me.ID, "__")(0)
                sGridChild = Regex.Split(Me.ID, "__")(1)
                'check default button clicked and need to load only default items 
                If bSetDefault Then
                    'load parent xml 
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

                    sParentOI = ParentXML.ElementAt(0).Attribute("OI").Value

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

        Private Sub EditItemGrid_DataBinding(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBinding

        End Sub

        Private Sub EditItemGrid_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.DataBound

        End Sub

        ''' <summary>
        ''' init event of grid 
        ''' Always generate column at the time of initialisation
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditItemGrid_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            BindData(True)
        End Sub

        Private Sub EditItemGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.Page.ClientScript.RegisterStartupScript(Me.GetType, "valuechanged", "<script>function valuechanged(changecontrol,changevalue,originalvalue) { if(changevalue != originalvalue){ changecontrol.setAttribute('Class', 'updated'); }}</script>")

        End Sub

        ''' <summary>
        ''' Handles the row command event of grid
        ''' Fires delete item event when click on remove link
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditItemGrid_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles Me.RowCommand

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
            BindData(False)
        End Sub

        ''' <summary>
        ''' Handles the row created event of grid
        ''' Add footer and header items in grid based on properties
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub EditItemGrid_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowCreated
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
        ''' Handles add event either Add button or Add link clicked in grid
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

            Dim arrColumnNames As String()
            Dim arrColumnTypes As String()
            Dim arrColumnEditable As String()

            arrColumnNames = Me.ChildColumnNames.Split(",")
            arrColumnTypes = Me.ChildColumnTypes.Split(",")
            arrColumnEditable = Me.ChildColumnIsEditable.Split(",")
            For iCount As Integer = 0 To Me.Rows.Count - 1
                For cCount As Integer = 0 To arrColumnNames.Length - 1
                    If arrColumnEditable(cCount) = "1" Then
                        Dim oCtrl As Control = Me.Rows(iCount).FindControl(arrColumnNames(cCount).Trim)
                        If oCtrl IsNot Nothing Then
                            If TypeOf oCtrl Is TextBox Then
                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(arrColumnNames(cCount), "__"), CType(oCtrl, TextBox).Text, CType(oCtrl, TextBox).ToolTip)
                            ElseIf TypeOf oCtrl Is DropDownList Then
                                WriteAttributeToXML(oDoc, oDataSet, Regex.Split(arrColumnNames(cCount), "__"), CType(oCtrl, DropDownList).SelectedValue, CType(oCtrl, DropDownList).ToolTip)
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

        Private Sub EditItemGrid_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles Me.RowDataBound

        End Sub

        Private Sub EditItemGrid_RowDeleted(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeletedEventArgs) Handles Me.RowDeleted

        End Sub

        Private Sub EditItemGrid_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles Me.RowDeleting
            'Have to refresh the page when delete event is fired because,
            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.Path, False)
        End Sub

        ''' <summary>
        ''' Procedure to set default rows based on backoffice default flag field
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetDefault()
            BindData(False, True)
        End Sub

        Private Sub EditItemGrid_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles Me.RowEditing

        End Sub

        Private Sub EditItemGrid_RowUpdated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdatedEventArgs) Handles Me.RowUpdated

        End Sub

        Private Sub EditItemGrid_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles Me.RowUpdating

        End Sub
    End Class
End Namespace
