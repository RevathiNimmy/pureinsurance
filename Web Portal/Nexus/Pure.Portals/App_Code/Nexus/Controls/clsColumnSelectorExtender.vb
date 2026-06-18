Imports System.Web.UI.WebControls
Imports Microsoft.VisualBasic

Namespace Nexus

    Public Class ColumnSelectorExtender
        Inherits WebControl

        Private sControlToExtend As String
        Private sBranchCode As String
        Private sInterfaceName As String
        Private sLinkText As String = "Select Columns"
        Private sButtonText As String = "Select"
        Private sHeight As String = "300"
        Private sWidth As String = "150"
        Private grdvTargetGridView As GridView
        Private chlstColumnSelector As New CheckBoxList
        Private sColumnList As String = String.Empty

        Public ReadOnly Property ColumnSelector() As CheckBoxList
            Get
                Return chlstColumnSelector
            End Get
        End Property
        Public WriteOnly Property Inetrface() As String
            Set(ByVal value As String)
                sInterfaceName = value
            End Set
        End Property

        Public WriteOnly Property BranchCode() As String
            Set(ByVal value As String)
                sBranchCode = value
            End Set
        End Property
        Public WriteOnly Property ControlToExtend() As String
            Set(ByVal value As String)
                sControlToExtend = value
            End Set
        End Property

        Public WriteOnly Property LinkText() As String
            Set(ByVal value As String)
                sLinkText = value
            End Set
        End Property

        Public WriteOnly Property ButtonText() As String
            Set(ByVal value As String)
                sButtonText = value
            End Set
        End Property

        Public WriteOnly Property ModalHeight() As String
            Set(ByVal value As String)
                sHeight = value
            End Set
        End Property

        Public WriteOnly Property ModalWidth() As String
            Set(ByVal value As String)
                sWidth = value
            End Set
        End Property
        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)

            'declare function level Variable
            Dim sCssText As String = String.Empty
            Dim sHeaderDiv As New StringBuilder
            Dim sJsBuilder As New StringBuilder
            MyBase.OnInit(e)

            'add hyperlink which will open the modal
            Dim hypOpenSelectionBox As New HyperLink
            Dim parent_Page As Page = Me.Parent.Page
            If parent_Page.Request.QueryString("AllocationAccountkey") IsNot Nothing Then

                MyBase.Controls.Add(New LiteralControl("<div id=""fede_allocation"" class='black_overlay'></div>"))

                sJsBuilder.Append("<style>")
                'Pop-Up div class 
                sJsBuilder.Append(".thickbox_allocation {")
                sJsBuilder.Append("position: absolute;")
                sJsBuilder.Append("top: 10px;")
                sJsBuilder.Append("left: 270px;")
                sJsBuilder.Append("z-index: 1002;")
                sJsBuilder.Append("width: 270px;")
                sJsBuilder.Append("height: auto;")
                sJsBuilder.Append("padding: 0px 0px 0px 0px;")
                sJsBuilder.Append("background: #ffffff;")
                sJsBuilder.Append("}")

                'Fade Class
                sJsBuilder.Append(".black_overlay {")
                sJsBuilder.Append("display: none;")
                sJsBuilder.Append("position: fixed;")
                sJsBuilder.Append("top: 0%;")
                sJsBuilder.Append("left: 0%;")
                sJsBuilder.Append("width: 100%;")
                sJsBuilder.Append("height: 100%;")
                sJsBuilder.Append("background-color: #c0c0c0;")
                sJsBuilder.Append("z-index: 1001;")
                sJsBuilder.Append("-moz-opacity: 0.8;")
                sJsBuilder.Append("opacity: .80;")
                sJsBuilder.Append("filter: alpha(opacity=80);")
                sJsBuilder.Append("}")
                sJsBuilder.Append("</style>")


                Dim GenericDiv As New HtmlGenericControl
                GenericDiv.InnerHtml = sJsBuilder.ToString
                MyBase.Controls.Add(GenericDiv)
                sCssText = "thickbox_allocation"


                sHeaderDiv.Append("<div class='ui-dialog-titlebar ui-widget-header ui-corner-all ui-helper-clearfix' sizcache='2' sizset='0'>")
                sHeaderDiv.Append("<span id='ui-dialog-title-ColumnSelector' class='ui-dialog-title'>&nbsp;</SPAN>")
                sHeaderDiv.Append("<a class='ui-dialog-titlebar-close ui-corner-all' style='float:right;margin:4px 5px 4px 0px' role='button' href='#'>")
                sHeaderDiv.Append("<span class='ui-icon ui-icon-closethick'")
                sHeaderDiv.Append(" onclick=""document.getElementById('ColumnSelector').style.display='none';document.getElementById('fede_allocation').style.display='none';"" ")
                sHeaderDiv.Append(">close</span>")
                sHeaderDiv.Append("</a></div>")

                hypOpenSelectionBox.NavigateUrl = "#"
                hypOpenSelectionBox.Attributes.Add("onclick", "document.getElementById('ColumnSelector').style.display='block';document.getElementById('fede_allocation').style.display='block';")
                'hypOpenSelectionBox.NavigateUrl = "#TB_inline?height=" & sHeight & "&width=" & sWidth & "&inlineId=ColumnSelector"
                'hypOpenSelectionBox.CssClass = "thickbox"
                hypOpenSelectionBox.Attributes.Add("data-bs-toggle", "modal")
                hypOpenSelectionBox.Attributes.Add("href", "#")
                hypOpenSelectionBox.Attributes.Add("data-bs-target", "#ColumnSelector")
                hypOpenSelectionBox.Text = "<i class='fa fa-th-list' aria-hidden='true'></i> " + sLinkText
                hypOpenSelectionBox.SkinID = "btnHSM"
            Else
                hypOpenSelectionBox.NavigateUrl = "#TB_inline?height=" & sHeight & "&width=" & sWidth & "&inlineId=ColumnSelector"
                hypOpenSelectionBox.CssClass = "thickbox"
                hypOpenSelectionBox.Attributes.Add("data-bs-toggle", "modal")
                hypOpenSelectionBox.Attributes.Add("data-bs-target", "#ColumnSelector")
            End If

            hypOpenSelectionBox.Text = sLinkText
            MyBase.Controls.Add(hypOpenSelectionBox)

            'open a div which is the hidden modal. controls inside this div are launced when the hyperlink is clicked
            MyBase.Controls.Add(New LiteralControl("<div id='ColumnSelector' class='modal fade'  tabindex='-1' role='dialog' aria-labelledby='myModalLabel'><div id='modalpage' class='modal-dialog' role='document'><div class='modal-content'><div class='modal-header'> <h4 class='modal-title'>Select Columns</h4></div><div class='modal-body'>"))

            'create a checklist control
            chlstColumnSelector = New CheckBoxList
            chlstColumnSelector.ID = "chlstColumnSelector"
            chlstColumnSelector.CssClass = "asp-check"
            chlstColumnSelector.RepeatLayout = RepeatLayout.Table

            'get a reference to the first target gridview
            Dim grdvTempGridView() As String = sControlToExtend.Split(",")

            grdvTargetGridView = Me.Parent.FindControl(grdvTempGridView(0).ToString())

            Dim oUserPreferredColumns As NexusProvider.UserPreferredColumnList
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oUserPreferredColumns = New NexusProvider.UserPreferredColumnList
            oUserPreferredColumns = oWebService.GetUserPreferredColumnList(sBranchCode, sInterfaceName)

            'add a list item to the checklist for each column in the grid view
            Dim li As ListItem
            sColumnList = oUserPreferredColumns.ColumnList
            If (sColumnList IsNot Nothing) Then
                Dim gridColumnList() As String = sColumnList.Split(",")

                For Each grdColumn As DataControlField In grdvTargetGridView.Columns
                    If grdColumn.HeaderText.Trim() <> "" AndAlso grdColumn.HeaderText.Trim() <> "Trans Detail Keys" Then
                        li = New ListItem
                        li.Text = grdColumn.HeaderText
                        For colCnt As Integer = 0 To gridColumnList.Length - 1
                            If gridColumnList(colCnt) = grdColumn.HeaderText Then
                                li.Selected = True
                            End If
                        Next
                        chlstColumnSelector.Items.Add(li)

                    End If
                Next

            Else
                For Each grdColumn As DataControlField In grdvTargetGridView.Columns
                    If grdColumn.HeaderText.Trim() <> "" AndAlso grdColumn.HeaderText.Trim() <> "Trans Detail Keys" Then
                        li = New ListItem
                        li.Text = grdColumn.HeaderText
                        If grdColumn.Visible Then li.Selected = True
                        chlstColumnSelector.Items.Add(li)
                    End If
                Next
            End If
            MyBase.Controls.Add(chlstColumnSelector)


            'add button to hidden div
            Dim btnUpdateColumns As New LinkButton
            btnUpdateColumns.Text = sButtonText
            btnUpdateColumns.SkinID = "btnPrimary"
            btnUpdateColumns.OnClientClick = "tb_remove();"
            'hook up to event handler
            AddHandler btnUpdateColumns.Click, AddressOf btnUpdateColumns_Click
            Controls.Add(btnUpdateColumns)

            'close the div
            MyBase.Controls.Add(New LiteralControl("</div></div></div></div>"))

            If Not Page.IsPostBack Then
                SetGridColumnsVisiblity()
            End If

        End Sub

        Private Sub btnUpdateColumns_Click(ByVal sender As Object, ByVal e As System.EventArgs)
            SetGridColumnsVisiblity()
            DirectCast(Me.Page, Object).UpdateUserPreferredColumnList()
            DirectCast(Me.Page, Object).GridRefresh()
            Parent.Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "tb_remove();tb_updated('','SelectColumns');", True)
        End Sub

        Private Sub SetGridColumnsVisiblity()

            Dim grdvTempGridView() As String = sControlToExtend.Split(",")
            Dim iCt As Integer

            For iCt = 0 To grdvTempGridView.GetUpperBound(0)
                grdvTargetGridView = Me.Parent.FindControl(grdvTempGridView(iCt).ToString())
                Dim bCheckBox As Boolean = False
                Dim li As ListItem
                Dim x As Integer
                'Check Whether first column is Checkbox in grid
                If TypeOf grdvTargetGridView.Columns(0) Is System.Web.UI.WebControls.TemplateField Then
                    bCheckBox = True
                End If
                'loop through the list items, as they are in the same order as the columns they should have the same index
                For x = 0 To chlstColumnSelector.Items.Count - 1
                    li = chlstColumnSelector.Items(x)
                    If bCheckBox = True Then
                        grdvTargetGridView.Columns(x + 2).Visible = li.Selected
                    Else
                        grdvTargetGridView.Columns(x).Visible = li.Selected
                    End If
                    If Not sControlToExtend.Contains("grdvAuthorisepayments") Then
                        grdvTargetGridView.Columns(1).Visible = True
                    End If
                Next
            Next
            Parent.Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "tb_remove();", True)

        End Sub
    End Class

End Namespace