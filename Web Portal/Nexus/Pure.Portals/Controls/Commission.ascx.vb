Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports Nexus.Utils
Imports Nexus
Imports System.Web.HttpContext
Imports SiriusFS.SAM.Client
Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Xml.XPath
Imports System.Xml.XmlReader
Imports System.Globalization.CultureInfo
Imports System.Linq
Imports System.Xml.Linq

Namespace Nexus

    ''' <summary>
    ''' this page will enable the user to change the commission percentages for each section of the policy
    ''' </summary>
    ''' <remarks></remarks>
    Partial Class Controls_Commission
        Inherits System.Web.UI.UserControl
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim sDataModelCode As String = String.Empty
        Dim strCOB As String = String.Empty
        'show/hide the columns depending underwriter or broker portal
        Dim bShowCommissionEditLinks As Boolean = True
        Dim bShowSection As Boolean = True
        Dim bShowRecipient As Boolean = True
        Dim bShowCommPer As Boolean = True
        Dim bShowCommAmount As Boolean = True
        Dim bShowNetAnnualPremium As Boolean = True
        Dim bShowMaxComm As Boolean = True
        Dim bShowGrossAnnualPremium As Boolean = True
        Dim bShowNetAPRP As Boolean = True
        Dim bShowGrossAPRP As Boolean = True
        Dim bIsSubAgent As Boolean = False
        Dim bIsLeadAgent As Boolean = True
        Dim bIsRetained As Boolean = True
        Dim bShowExitButton As Boolean = True
        Dim bShowSaveButton As Boolean = True
        Dim bShowDefaultButton As Boolean = True
        'set the mode to view for specific case when session mode is not view but control work in view mode
        Dim bViewMode As Boolean = False

        ''' <summary>
        ''' property to show/hide edit links
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowEditLinks() As Boolean
            Set(ByVal value As Boolean)
                bShowCommissionEditLinks = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide policy sections column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowSection() As Boolean
            Set(ByVal value As Boolean)
                bShowSection = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide Recipient column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowRecipient() As Boolean
            Set(ByVal value As Boolean)
                bShowRecipient = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide commission percentage column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowCommPer() As Boolean
            Set(ByVal value As Boolean)
                bShowCommPer = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide commission amount column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowCommAmount() As Boolean
            Set(ByVal value As Boolean)
                bShowCommAmount = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide net annual premium column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowNetAnnualPremium() As Boolean
            Set(ByVal value As Boolean)
                bShowNetAnnualPremium = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide gross annual premium column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowGrossAnnualPremium() As Boolean
            Set(ByVal value As Boolean)
                bShowGrossAnnualPremium = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide net AP/RP column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowNetAPRP() As Boolean
            Set(ByVal value As Boolean)
                bShowNetAPRP = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide gross AP/RP column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowGrossAPRP() As Boolean
            Set(ByVal value As Boolean)
                bShowGrossAPRP = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide maximum commission column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowMaxComm() As Boolean
            Set(ByVal value As Boolean)
                bShowMaxComm = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide is sub agent? column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property IsSubAgent() As Boolean
            Set(ByVal value As Boolean)
                bIsSubAgent = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide is lead agent? column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property IsLeadAgent() As Boolean
            Set(ByVal value As Boolean)
                bIsLeadAgent = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide is retained? column
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property IsRetained() As Boolean
            Set(ByVal value As Boolean)
                bIsRetained = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide exit button
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowExitButton() As Boolean
            Set(ByVal value As Boolean)
                bShowExitButton = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide save button
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowSaveButton() As Boolean
            Set(ByVal value As Boolean)
                bShowSaveButton = value
            End Set
        End Property

        ''' <summary>
        ''' property to show/hide default button
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property ShowDefaultButton() As Boolean
            Set(ByVal value As Boolean)
                bShowDefaultButton = value
            End Set
        End Property

        ''' <summary>
        ''' property to set view mode in any specific case
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property SetViewMode() As Boolean
            Set(ByVal value As Boolean)
                bViewMode = value
            End Set
        End Property

        'to fire postback on parent page on every button click
        Private bPostBack As Boolean = True
        ''' <summary>
        ''' property to fire postback on parent page on every button click
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property PostBack() As Boolean
            Set(ByVal value As Boolean)
                bPostBack = value
            End Set
        End Property

        Protected Sub Page_AbortTransaction(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.AbortTransaction
            Dim bPostBack As Boolean = True
            'check if post required or not
            If Request.QueryString("PostBack") IsNot Nothing Then
                bPostBack = Convert.ToBoolean(Request.QueryString("PostBack"))
            End If
            If bPostBack Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                'refresh the parent page on postback with event argument RefreshGrid  
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            ShowHideColumns() 'call to show/hide the columns according to the user

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            sDataModelCode = Session(CNDataModelCode) 'data model code
            'Show edit links only to selected user groups
            If UserCanDoTask("ShowCommEditLinks") And Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                grdvCommission.Columns(11).Visible = True
            Else
                grdvCommission.Columns(11).Visible = False
            End If
            'To Show AP/RP columns only in case of MTA or MTC
            If Session(CNMTAType) IsNot Nothing Then
                If bShowNetAPRP = True Then
                    grdvCommission.Columns(9).Visible = True 'Net AP/RP
                End If
                If bShowGrossAPRP = True Then
                    grdvCommission.Columns(10).Visible = True 'Gross AP/RP
                End If
            Else
                grdvCommission.Columns(9).Visible = False
                grdvCommission.Columns(10).Visible = False
            End If
            btnExit.Visible = bShowExitButton
            'If opened in view mode then Save button and SetDefaults button is not available 
            If Session(CNMode) <> Mode.View And Session(CNMode) <> Mode.Review And bViewMode = False Then
                btnSave.Visible = bShowSaveButton
                btnDefault.Visible = bShowDefaultButton
            Else
                btnSave.Visible = False
                btnDefault.Visible = False
            End If
            If Not UserCanDoTask("ShowDefaultCommButton") Then
                btnDefault.Visible = False
            End If
            BindData() 'bind grid

        End Sub

        'do not delete this
        Protected Sub grdvCommission_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles grdvCommission.RowCancelingEdit

        End Sub

        Protected Sub grdvCommission_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvCommission.RowCommand

            'for every row of the grid
            For iTempVar As Integer = 0 To grdvCommission.Rows.Count - 1
                'fetch data from XML of output_commission object based on OI Key 
                Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@NET_ANNUAL_COMMISSION_OVERRIDEN!='0' and @OI='" & CType(grdvCommission.Rows(iTempVar).Cells(11).FindControl("lblOI"), Label).Text & "']"
                Dim strKey As String
                'policy section code and agent code - used to compare with command argument so as to identify the row
                strKey = CType(grdvCommission.Rows(iTempVar).Cells(0).FindControl("lblPolicySectionCode"), Label).Text.Trim + CType(grdvCommission.Rows(iTempVar).Cells(0).FindControl("lblAgentCode"), Label).Text.Trim
                If CStr(e.CommandArgument) = strKey Then
                    'show save and cancel link buttons in case the screen is in edit mode
                    If e.CommandName = "Edit" Then
                        grdvCommission.EditIndex = iTempVar 'edit mode enabled
                        BindData()  'Bind the grid
                        grdvCommission.Rows(iTempVar).Cells(10).FindControl("lnkEdit").Visible = False
                        grdvCommission.Rows(iTempVar).Cells(10).FindControl("lnkSave").Visible = True
                        grdvCommission.Rows(iTempVar).Cells(10).FindControl("lnkCancel").Visible = True
                        Session("RowIndex") = iTempVar
                        Exit For
                        'on click of cancel, hide save and cancel link buttons and show edit link
                    ElseIf e.CommandName = "Cancel" Then
                        grdvCommission.EditIndex = -1 'edit mode disabled
                        BindData()  'Bind the grid
                        grdvCommission.Rows(iTempVar).Cells(10).FindControl("lnkEdit").Visible = True
                        grdvCommission.Rows(iTempVar).Cells(10).FindControl("lnkSave").Visible = False
                        grdvCommission.Rows(iTempVar).Cells(10).FindControl("lnkCancel").Visible = False
                        Exit For
                    End If
                End If
            Next

        End Sub

        ''' <summary>
        ''' bind the grid
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub BindData()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            xmlTR.Close()  'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
            If bIsLeadAgent And bIsRetained Then
                'Fetch data from XML of output_commission object where is_lead <> 0 and is_retained <> 0
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where ((Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) <> 0 Or Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) <> 0) And CommissionOutputs.Attribute("COB_CODE").Value <> "0")
                'check if CommissionOutput has got data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        'bind the grid
                        grdvCommission.DataSource = CommissionOutput
                        grdvCommission.DataBind()
                        PopulateTotals() 'populate total premium
                    End If
                End If
            ElseIf bIsLeadAgent And bIsRetained = False Then
                'Fetch data from XML of output_commission object where is_lead is 1 and is_retained is 0
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 1 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0 And CommissionOutputs.Attribute("COB_CODE").Value <> "0")
                'check if CommissionOutput has got data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        'bind the grid
                        grdvCommission.DataSource = CommissionOutput
                        grdvCommission.DataBind()
                        PopulateTotals() 'populate total premium
                    End If
                End If
            ElseIf bIsLeadAgent = False And bIsRetained Then
                'Fetch data from XML of output_commission object where is_lead is 0 and is_retained is 1
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 1 And CommissionOutputs.Attribute("COB_CODE").Value <> "0")
                'check if CommissionOutput has got data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        'bind the grid
                        grdvCommission.DataSource = CommissionOutput
                        grdvCommission.DataBind()
                        PopulateTotals() 'populate total premium
                    End If
                End If
            ElseIf bIsSubAgent Then
                'Fetch data from XML of output_commission object where is_lead is 0 and is_retained is 0
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0 And CommissionOutputs.Attribute("COB_CODE").Value <> "0")
                'check if CommissionOutput has got data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        'bind the grid
                        grdvCommission.DataSource = CommissionOutput
                        grdvCommission.DataBind()
                        PopulateTotals() 'populate total premium
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' to show/hide the columns
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub ShowHideColumns()

            'Show/hide EditLinks
            If bShowCommissionEditLinks = False Then
                grdvCommission.Columns(11).Visible = False
            Else
                grdvCommission.Columns(11).Visible = True
            End If
            'Show/hide Recipient
            If bShowRecipient = False Then
                grdvCommission.Columns(3).Visible = False
            Else
                grdvCommission.Columns(3).Visible = True
            End If
            'Show/hide CommPer
            If bShowCommPer = False Then
                grdvCommission.Columns(4).Visible = False
            Else
                grdvCommission.Columns(4).Visible = True
            End If
            'Show/hide CommAmount
            If bShowCommAmount = False Then
                grdvCommission.Columns(5).Visible = False
            Else
                grdvCommission.Columns(5).Visible = True
            End If
            'Show/hide NetAnnualPremium
            If bShowNetAnnualPremium = False Then
                grdvCommission.Columns(6).Visible = False
            Else
                grdvCommission.Columns(6).Visible = True
            End If
            'Show/hide MaxComm
            If bShowMaxComm = False Then
                grdvCommission.Columns(7).Visible = False
            Else
                grdvCommission.Columns(7).Visible = True
            End If
            'Show/hide GrossAnnualPremium
            If bShowGrossAnnualPremium = False Then
                grdvCommission.Columns(8).Visible = False
            Else
                grdvCommission.Columns(8).Visible = True
            End If
            'Show/hide Net AP/RP
            If bShowNetAPRP = False Then
                grdvCommission.Columns(9).Visible = False
            Else
                grdvCommission.Columns(9).Visible = True
            End If
            'Show/hide Gross AP/RP
            If bShowGrossAPRP = False Then
                grdvCommission.Columns(10).Visible = False
            Else
                grdvCommission.Columns(10).Visible = True
            End If

        End Sub

        Protected Sub grdvCommission_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvCommission.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then
                'In Case of MTA commision amount will be bind with diff field
                If Session(CNMTAType) IsNot Nothing Then
                    'Find Commission Amount Label
                    Dim lblCommAmount As Label = CType(e.Row.FindControl("lblCommAmount"), Label)
                    'Assign the value
                    lblCommAmount.Text = CType(e.Row.DataItem, System.Xml.Linq.XElement).Attribute("NET_AP_RP_OVERRIDEN").Value
                End If
                'If edit links are not available then comm percentage should be displayed in textbox so that it is editable
                If bShowCommissionEditLinks = False Then
                    'find the comm percentage control- make it editable and set its borderstyle
                    If CType(e.Row.FindControl("txtCommPercen"), TextBox) IsNot Nothing Then
                        CType(e.Row.FindControl("txtCommPercen"), TextBox).ReadOnly = False
                        CType(e.Row.FindControl("txtCommPercen"), TextBox).BorderStyle = BorderStyle.Solid
                    End If
                End If
                'policy section code
                Dim lblCode As Label = CType(e.Row.FindControl("lblPolicySectionCode"), Label)
                'set the oQuote from session
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
                Dim xmlTR As New XmlTextReader(srDataset)
                Dim Doc As New XmlDocument
                Doc.Load(xmlTR)  'load the XML
                xmlTR.Close()  'close the text reader
                Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
                'Fetch data from XML of output_commission where is_lead is 1 or is_retained is 1
                If bIsLeadAgent And bIsRetained Then
                    Dim CommissionOutput = _
                           From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                           Where ((Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) <> 0 Or Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) <> 0) And CommissionOutputs.Attribute("COB_CODE").Value = lblCode.Text)
                    'check if CommissionOutput has got data
                    If CommissionOutput IsNot Nothing Then
                        If CommissionOutput.Count > 0 Then
                            If e.Row.RowIndex > 0 Then
                                'to merge the rows for same COB i.e for same policy section
                                'First time set the variable strCOB as the description of policy section,
                                'if second row is of same policy section then it is merged
                                If strCOB = lblCode.Text Then
                                    Dim PreviousRow As GridViewRow = grdvCommission.Rows(e.Row.RowIndex - 1)
                                    PreviousRow.Cells(2).RowSpan = 2
                                    PreviousRow.Cells(6).RowSpan = 2
                                    PreviousRow.Cells(7).RowSpan = 2
                                    PreviousRow.Cells(8).RowSpan = 2
                                    PreviousRow.Cells(9).RowSpan = 2
                                    PreviousRow.Cells(10).RowSpan = 2
                                    e.Row.Cells(2).CssClass = "hiddencol"
                                    e.Row.Cells(6).CssClass = "hiddencol"
                                    e.Row.Cells(7).CssClass = "hiddencol"
                                    e.Row.Cells(8).CssClass = "hiddencol"
                                    e.Row.Cells(9).CssClass = "hiddencol"
                                    e.Row.Cells(10).CssClass = "hiddencol"
                                Else
                                    strCOB = lblCode.Text.Trim  'in case of new policy section , set the value
                                    'Find the controls and set the values of premium and ap/rp
                                    CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                    CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                                End If
                            Else
                                strCOB = lblCode.Text.Trim  'initially set the value for policy section
                                'Find the controls and set the values of premium and ap/rp
                                CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                            End If
                        End If
                    End If
                    'Fetch data from XML of output_commission where is_lead is 1 and is_retained is 0
                ElseIf bIsLeadAgent And bIsRetained = False Then
                    Dim CommissionOutput = _
                           From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                           Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 1 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0 And CommissionOutputs.Attribute("COB_CODE").Value = lblCode.Text)
                    'check if CommissionOutput has got data
                    If CommissionOutput IsNot Nothing Then
                        If CommissionOutput.Count > 0 Then
                            'to merge the rows for same COB i.e for same policy section
                            'First time set the variable strCOB as the description of policy section,
                            'if second row is of same policy section then it is merged
                            If e.Row.RowIndex > 0 Then
                                'if policy section is same the row is merged, css is set to not show a seperate row
                                If strCOB = lblCode.Text Then
                                    Dim PreviousRow As GridViewRow = grdvCommission.Rows(e.Row.RowIndex - 1)
                                    PreviousRow.Cells(2).RowSpan = 2
                                    PreviousRow.Cells(6).RowSpan = 2
                                    PreviousRow.Cells(7).RowSpan = 2
                                    PreviousRow.Cells(8).RowSpan = 2
                                    PreviousRow.Cells(9).RowSpan = 2
                                    PreviousRow.Cells(10).RowSpan = 2
                                    e.Row.Cells(2).CssClass = "hiddencol"
                                    e.Row.Cells(6).CssClass = "hiddencol"
                                    e.Row.Cells(7).CssClass = "hiddencol"
                                    e.Row.Cells(8).CssClass = "hiddencol"
                                    e.Row.Cells(9).CssClass = "hiddencol"
                                    e.Row.Cells(10).CssClass = "hiddencol"
                                Else
                                    strCOB = lblCode.Text.Trim  'in case of new policy section , set the value
                                    'Find the controls and set the values of premium and ap/rp
                                    CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                    CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                                End If
                            Else
                                strCOB = lblCode.Text.Trim 'initially set the value for policy section
                                'Find the controls and set the values of premium and ap/rp
                                CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                            End If
                        End If
                    End If
                    'Fetch data from XML of output_commission where is_lead is 0 and is_retained is 1
                ElseIf bIsLeadAgent = False And bIsRetained Then
                    Dim CommissionOutput = _
                           From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                           Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 1 And CommissionOutputs.Attribute("COB_CODE").Value = lblCode.Text)
                    'check if CommissionOutput has got data
                    If CommissionOutput IsNot Nothing Then
                        If CommissionOutput.Count > 0 Then
                            If e.Row.RowIndex > 0 Then
                                'to merge the rows for same COB i.e for same policy section
                                'First time set the variable strCOB as the description of policy section,
                                'if second row is of same policy section then it is merged
                                If strCOB = lblCode.Text Then
                                    Dim PreviousRow As GridViewRow = grdvCommission.Rows(e.Row.RowIndex - 1)
                                    PreviousRow.Cells(2).RowSpan = 2
                                    PreviousRow.Cells(6).RowSpan = 2
                                    PreviousRow.Cells(7).RowSpan = 2
                                    PreviousRow.Cells(8).RowSpan = 2
                                    PreviousRow.Cells(9).RowSpan = 2
                                    PreviousRow.Cells(10).RowSpan = 2
                                    e.Row.Cells(2).CssClass = "hiddencol"
                                    e.Row.Cells(6).CssClass = "hiddencol"
                                    e.Row.Cells(7).CssClass = "hiddencol"
                                    e.Row.Cells(8).CssClass = "hiddencol"
                                    e.Row.Cells(9).CssClass = "hiddencol"
                                    e.Row.Cells(10).CssClass = "hiddencol"
                                Else
                                    strCOB = lblCode.Text.Trim  'in case of new policy section , set the value
                                    'Find the controls and set the values of premium and ap/rp
                                    CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                    CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                                End If
                            Else
                                strCOB = lblCode.Text.Trim  'initially set the value for policy section
                                'Find the controls and set the values of premium and ap/rp
                                CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                            End If
                        End If
                    End If
                    'Fetch data from XML of output_commission where is_lead is 0 and is_retained is 0
                ElseIf bIsSubAgent Then
                    Dim CommissionOutput = _
                           From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                           Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0 And CommissionOutputs.Attribute("COB_CODE").Value = lblCode.Text)
                    'check if CommissionOutput has got data
                    If CommissionOutput IsNot Nothing Then
                        If CommissionOutput.Count > 0 Then
                            If e.Row.RowIndex > 0 Then
                                'to merge the rows for same COB i.e for same policy section
                                'First time set the variable strCOB as the description of policy section,
                                'if second row is of same policy section then it is merged
                                If strCOB = lblCode.Text Then
                                    Dim PreviousRow As GridViewRow = grdvCommission.Rows(e.Row.RowIndex - 1)
                                    PreviousRow.Cells(2).RowSpan = 2
                                    PreviousRow.Cells(6).RowSpan = 2
                                    PreviousRow.Cells(7).RowSpan = 2
                                    PreviousRow.Cells(8).RowSpan = 2
                                    PreviousRow.Cells(9).RowSpan = 2
                                    PreviousRow.Cells(10).RowSpan = 2
                                    e.Row.Cells(2).CssClass = "hiddencol"
                                    e.Row.Cells(6).CssClass = "hiddencol"
                                    e.Row.Cells(7).CssClass = "hiddencol"
                                    e.Row.Cells(8).CssClass = "hiddencol"
                                    e.Row.Cells(9).CssClass = "hiddencol"
                                    e.Row.Cells(10).CssClass = "hiddencol"
                                Else
                                    strCOB = lblCode.Text.Trim  'in case of new policy section , set the value
                                    'Find the controls and set the values of premium and ap/rp
                                    CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                    CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                    CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                                End If

                            Else
                                strCOB = lblCode.Text.Trim  'initially set the value for policy section
                                'Find the controls and set the values of premium and ap/rp
                                CType(e.Row.FindControl("lblNetAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM").Value.ToString
                                CType(e.Row.FindControl("lblGrossAnnualPremium"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_ANNUAL_COMMISSION").Value.ToString
                                CType(e.Row.FindControl("lblNetAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("PREMIUM_AP_RP").Value.ToString
                                CType(e.Row.FindControl("lblGrossAPRP"), Label).Text = CommissionOutput.ElementAt(0).Attribute("GROSS_AP_RP").Value.ToString
                            End If
                        End If
                    End If
                End If
                'Find hidden control for original comm percentage
                Dim lblOriginalCommPer As HiddenField = CType(e.Row.FindControl("lblOriginalCommPer"), HiddenField)
                'Find text control for displaying comm percentage
                Dim txtCommPer As TextBox = CType(e.Row.FindControl("txtCommPer"), TextBox)
                'Find text control for displaying comm percentage
                Dim txtCommPercen As TextBox = CType(e.Row.FindControl("txtCommPercen"), TextBox)
                'Find text control for displaying max comm percentage
                Dim txtMaxComm As HiddenField = CType(e.Row.FindControl("lblMaxComm"), HiddenField)
                'If changed commission percentage is more than the max commission percentage then a msg popup is shown and user is not allowed to make that change
                If txtCommPercen IsNot Nothing Then
                    Dim cAmount As Double = txtCommPercen.Text
                    txtCommPercen.Attributes.Add("onblur", "javascript:if(parseFloat(document.getElementById('" & txtCommPercen.ClientID & "').value) > parseFloat(document.getElementById('" & txtMaxComm.ClientID & "').value)) { alert('Percentage cannot be more than Max allowed percentage'); document.getElementById('" & txtCommPercen.ClientID & "').value = " & cAmount & "; document.getElementById('" & txtCommPercen.ClientID & "').setAttribute('Class', ''); } else { if(document.getElementById('" & txtCommPercen.ClientID & "').value != " & cAmount & ") { document.getElementById('" & txtCommPercen.ClientID & "').setAttribute('Class', 'updated'); } else { document.getElementById('" & txtCommPercen.ClientID & "').setAttribute('Class', ''); } } ")
                    If lblOriginalCommPer.Value <> txtCommPercen.Text Then
                        txtCommPercen.Attributes.Add("Class", "updated")
                    End If
                ElseIf txtCommPer IsNot Nothing Then
                    Dim cAmount As Double = txtCommPer.Text
                    txtCommPer.Attributes.Add("onblur", "javascript:if(parseFloat(document.getElementById('" & txtCommPer.ClientID & "').value) > parseFloat(document.getElementById('" & txtMaxComm.ClientID & "').value)) { alert('Percentage cannot be more than Max allowed percentage'); document.getElementById('" & txtCommPer.ClientID & "').value = " & cAmount & "; document.getElementById('" & txtCommPer.ClientID & "').setAttribute('Class', ''); } else { if(document.getElementById('" & txtCommPer.ClientID & "').value != " & cAmount & ") { document.getElementById('" & txtCommPer.ClientID & "').setAttribute('Class', 'updated'); } else { document.getElementById('" & txtCommPer.ClientID & "').setAttribute('Class', ''); } } ")
                    If lblOriginalCommPer.Value <> txtCommPer.Text Then
                        txtCommPer.Attributes.Add("Class", "updated")
                    End If
                End If
            End If

        End Sub

        Protected Sub btnExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExit.Click
            Dim bPostBack As Boolean = True
            'check if post required or not
            If Request.QueryString("PostBack") IsNot Nothing Then
                bPostBack = Convert.ToBoolean(Request.QueryString("PostBack"))
            End If
            If bPostBack Then
                Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                'refresh the parent page on postback with event argument RefreshGrid  
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
            'close the modal page
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)

        End Sub

        '''' <summary>
        '''' Set back to the default values
        '''' </summary>
        '''' <remarks></remarks>
        'Private Sub SetDefaultValues()

        '    'set the oQuote from session
        '    Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        '    Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
        '    Dim xmlTR As New XmlTextReader(srDataset)
        '    Dim Doc As New XmlDocument
        '    Doc.Load(xmlTR) 'load the XML
        '    Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument   
        '    'Fetch data from XML of output_premium breakdown object
        '    Dim PremiumBreakDown = _
        '           From PremiumBreakDowns In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN") Select PremiumBreakDowns
        '    'check if PremiumBreakDown has got some data
        '    If PremiumBreakDown IsNot Nothing Then
        '        If PremiumBreakDown.Count > 0 Then
        '            For Each row In PremiumBreakDown
        '                'Fetch row from XML based on OI Key
        '                Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN[@OI='" & row.Attribute("OI").Value & "']"
        '                'Update XML - update the xml with original value
        '                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_BASE", row.Attribute("PERCENT_ORIGINAL").Value)
        '                'Update XML - update the value with overriden value
        '                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "PERCENT_OVERRIDE", row.Attribute("PERCENT_APPLICABLE").Value)
        '                'Update XML - set is_overriden as 1
        '                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_PREMIUMBREAKDOWN", "IS_OVERRIDEN", 1)
        '            Next
        '        End If
        '    End If
        '    Dim oWebService As NexusProvider.ProviderBase
        '    'retrieve the oQuote from session
        '    oQuote = System.Web.HttpContext.Current.Session(CNQuote)
        '    'create new instance of proxy
        '    oWebService = New NexusProvider.ProviderManager().Provider
        '    Try
        '        'call update risk to rerun rating scripts
        '        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
        '        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
        '        ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
        '        ElseIf Session(CNRenewal) Then
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
        '        Else
        '            oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
        '        End If
        '        'error handling - in case quote is referred or declined
        '    Catch ex As NexusProvider.NexusException
        '        If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
        '        ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
        '        End If
        '    Finally
        '        oWebService = Nothing  'clear the object
        '    End Try
        '    'create new instance of proxy
        '    oWebService = New NexusProvider.ProviderManager().Provider
        '    Try
        '        'call GetHeaderAndRisksByKey to retrieve the premium, tax etc.
        '        oWebService.GetHeaderAndRisksByKey(oQuote)
        '    Catch ex As NexusProvider.NexusException

        '    Finally
        '        oWebService = Nothing  'clear the object
        '    End Try
        '    'save the oQuote to session
        '    System.Web.HttpContext.Current.Session(CNQuote) = oQuote
        '    BindData() 'Bind repeater
        '    PopulateTotals() 'Calculate total premium

        'End Sub

        ''' <summary>
        ''' to set back to the default values
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub SetDefaultValues()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR)  'load the XML
            xmlTR.Close()  'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
            'Fetch data from XML of output_commission where is_lead is 1 or is_retained is 1
            If bIsLeadAgent And bIsRetained Then
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) <> 0 Or Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) <> 0)
                'Check if CommissionOutput has got some data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        For Each row In CommissionOutput
                            'Fetch the row using OI key
                            Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & row.Attribute("OI").Value & "']"
                            'Update XML - set the overridden commission percentage as the default percentage
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", row.Attribute("COMMISSION_PERCENT").Value)
                            'Update XML - set is_overriden as 1
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                        Next
                    End If
                End If
                'Fetch data from XML of output_commission where is_lead is 1 and is_retained is 0
            ElseIf bIsLeadAgent And bIsRetained = False Then
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 1 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0)
                'Check if CommissionOutput has got some data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        For Each row In CommissionOutput
                            'Fetch the row using OI key
                            Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & row.Attribute("OI").Value & "']"
                            'Update XML - set the overridden commission percentage as the default percentage
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", row.Attribute("COMMISSION_PERCENT").Value)
                            'Update XML - set is_overriden as 1 
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                        Next
                    End If
                End If
                'Fetch data from XML of output_commission where is_lead is 0 and is_retained is 1
            ElseIf bIsLeadAgent = False And bIsRetained Then
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 1)
                'Check if CommissionOutput has got some data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        For Each row In CommissionOutput
                            'Fetch the row using OI key
                            Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & row.Attribute("OI").Value & "']"
                            'Update XML - set the overridden commission percentage as the default percentage
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", row.Attribute("COMMISSION_PERCENT").Value)
                            'Update XML - set is_overriden as 1
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                        Next
                    End If
                End If
                'Fetch data from XML of output_commission where is_lead is 0 and is_retained is 0
            ElseIf bIsSubAgent Then
                Dim CommissionOutput = _
                       From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                       Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0)
                'Check if CommissionOutput has got some data
                If CommissionOutput IsNot Nothing Then
                    If CommissionOutput.Count > 0 Then
                        For Each row In CommissionOutput
                            'Fetch the row using OI key
                            Dim strQuery As String = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & row.Attribute("OI").Value & "']"
                            'Update XML - set the overridden commission percentage as the default percentage
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", row.Attribute("COMMISSION_PERCENT").Value)
                            'Update XML - set is_overriden as 1
                            UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                        Next
                    End If
                End If
            End If
            Dim oWebService As NexusProvider.ProviderBase
            'retrieve the oQuote from session
            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'call update risk to rerun rating scripts
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
                ElseIf Session(CNRenewal) Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
                Else
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
                End If
                'error handling - in case quote is referred or declined
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
                ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
                End If
            Finally
                oWebService = Nothing  'clear the object
            End Try
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'call GetHeaderAndRisksByKey to retrieve the premium, tax etc.
                oWebService.GetHeaderAndRisksByKey(oQuote)
            Catch ex As NexusProvider.NexusException

            Finally
                oWebService = Nothing  'clear the object
            End Try
            'save the oQuote to session
            System.Web.HttpContext.Current.Session(CNQuote) = oQuote
            BindData() 'bind the grid

        End Sub

        Protected Sub btnDefault_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDefault.Click

            SetDefaultValues()  'call to set back to the default values

        End Sub

        ''' <summary>
        ''' to populate total premium
        ''' </summary>
        ''' <remarks></remarks>
        Private Sub PopulateTotals()

            'set the oQuote from session
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim srDataset As New System.IO.StringReader(oQuote.Risks(Session(CNCurrentRiskKey)).XMLDataset)
            Dim xmlTR As New XmlTextReader(srDataset)
            Dim Doc As New XmlDocument
            Doc.Load(xmlTR) 'load the XML
            xmlTR.Close() 'close the text reader
            Dim docX As XDocument = XDocument.Parse(Doc.OuterXml) 'Convert from XML document to XDocument
            Dim strPolicyBinder As String = sDataModelCode & "_POLICY_BINDER_ID"
            'Find label control for policy section
            Dim lblPolicySectionTotal As Label = grdvCommission.FooterRow.FindControl("lblPolicySectionTotal")
            lblPolicySectionTotal.Text = "Total"
            'fetch data from XML of output_commission object, grouped on policy binder id and sum of premium, gross commission, net ap/rp and gross ap/rp are calculated.
            Dim CommissionOutputTotals = _
                    From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                    Where (CommissionOutputs.Attribute("COB_CODE").Value = "0") _
                 Group CommissionOutputs By PolicyBinder = CommissionOutputs.Attribute(strPolicyBinder).Value _
                 Into TotalNetAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("PREMIUM").Value)), _
                 TotalGrossAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("GROSS_ANNUAL_COMMISSION").Value)), _
                 TotalNetAPRP = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("PREMIUM_AP_RP").Value)), _
                 TotalGrossAPRP = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("GROSS_AP_RP").Value))
            'Check if CommissionOutputTotals has got some data
            If CommissionOutputTotals IsNot Nothing Then
                If CommissionOutputTotals.Count > 0 Then
                    'Find label controls for net and gross annual prem,ap/rp and assign the values
                    Dim lblTotalNetAnnualPremium As Label = grdvCommission.FooterRow.FindControl("lblTotalNetAnnualPremium")
                    lblTotalNetAnnualPremium.Text = CommissionOutputTotals.ElementAt(0).TotalNetAnnualComm.ToString
                    Dim lblTotalGrossAnnualPremium As Label = grdvCommission.FooterRow.FindControl("lblTotalGrossAnnualPremium")
                    lblTotalGrossAnnualPremium.Text = CommissionOutputTotals.ElementAt(0).TotalGrossAnnualComm.ToString
                    Dim lblTotalNetAPRP As Label = grdvCommission.FooterRow.FindControl("lblTotalNetAPRP")
                    lblTotalNetAPRP.Text = CommissionOutputTotals.ElementAt(0).TotalNetAPRP.ToString
                    Dim lblTotalGrossAPRP As Label = grdvCommission.FooterRow.FindControl("lblTotalGrossAPRP")
                    lblTotalGrossAPRP.Text = CommissionOutputTotals.ElementAt(0).TotalGrossAPRP.ToString
                End If
            End If
            Dim sCommFieldName As String = "NET_ANNUAL_COMMISSION_OVERRIDEN"
            If Session(CNMTAType) IsNot Nothing Then
                sCommFieldName = "NET_AP_RP_OVERRIDEN"
            End If
            If bIsLeadAgent And bIsRetained Then
                'fetch data from XML of output_commission object, grouped on policy binder id and find sum of comm percent and comm amount
                Dim CommissionOutputTotals1 = _
                    From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                    Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) <> 0 Or Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) <> 0 And CommissionOutputs.Attribute("COB_CODE").Value <> "0") _
                 Group CommissionOutputs By PolicyBinder = CommissionOutputs.Attribute(strPolicyBinder).Value _
                 Into TotalCommPerc = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("COMMISSION_PERCENT_OVERRIDEN").Value)), _
                 TotalNetAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute(sCommFieldName).Value))
                'Check if CommissionOutputTotals1 has got some data
                If CommissionOutputTotals1 IsNot Nothing Then
                    If CommissionOutputTotals1.Count > 0 Then
                        'Find label controls for total comm percent, comm amount and assign the values
                        Dim lblTotalCommPer As Label = grdvCommission.FooterRow.FindControl("lblTotalCommPer")
                        Dim lblTotalCommAmount As Label = grdvCommission.FooterRow.FindControl("lblTotalCommAmount")
                        lblTotalCommPer.Text = CommissionOutputTotals1.ElementAt(0).TotalCommPerc.ToString
                        lblTotalCommAmount.Text = CommissionOutputTotals1.ElementAt(0).TotalNetAnnualComm.ToString
                    End If
                End If
            ElseIf bIsLeadAgent And bIsRetained = False Then
                'fetch data from XML of output_commission object, grouped on policy binder id and find sum of comm percent and comm amount
                Dim CommissionOutputTotals1 = _
                    From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                    Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 1 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0 And CommissionOutputs.Attribute("COB_CODE").Value <> "0") _
                 Group CommissionOutputs By PolicyBinder = CommissionOutputs.Attribute(strPolicyBinder).Value _
                 Into TotalCommPerc = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("COMMISSION_PERCENT_OVERRIDEN").Value)), _
                 TotalNetAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute(sCommFieldName).Value))
                'Check if CommissionOutputTotals1 has got some data
                If CommissionOutputTotals1 IsNot Nothing Then
                    If CommissionOutputTotals1.Count > 0 Then
                        'Find label controls for total comm percent, comm amount and assign the values
                        Dim lblTotalCommPer As Label = grdvCommission.FooterRow.FindControl("lblTotalCommPer")
                        Dim lblTotalCommAmount As Label = grdvCommission.FooterRow.FindControl("lblTotalCommAmount")
                        lblTotalCommPer.Text = CommissionOutputTotals1.ElementAt(0).TotalCommPerc.ToString
                        lblTotalCommAmount.Text = CommissionOutputTotals1.ElementAt(0).TotalNetAnnualComm.ToString
                    End If
                End If
            ElseIf bIsLeadAgent = False And bIsRetained Then
                'fetch data from XML of output_commission object, grouped on policy binder id and find sum of comm percent and comm amount
                Dim CommissionOutputTotals1 = _
                     From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                     Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 1 And CommissionOutputs.Attribute("COB_CODE").Value <> "0") _
                 Group CommissionOutputs By PolicyBinder = CommissionOutputs.Attribute(strPolicyBinder).Value _
                 Into TotalCommPerc = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("COMMISSION_PERCENT_OVERRIDEN").Value)), _
                 TotalNetAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute(sCommFieldName).Value))
                'Check if CommissionOutputTotals1 has got some data
                If CommissionOutputTotals1 IsNot Nothing Then
                    If CommissionOutputTotals1.Count > 0 Then
                        Dim lblTotalCommPer As Label = grdvCommission.FooterRow.FindControl("lblTotalCommPer")
                        Dim lblTotalCommAmount As Label = grdvCommission.FooterRow.FindControl("lblTotalCommAmount")
                        lblTotalCommPer.Text = CommissionOutputTotals1.ElementAt(0).TotalCommPerc.ToString
                        lblTotalCommAmount.Text = CommissionOutputTotals1.ElementAt(0).TotalNetAnnualComm.ToString
                    End If
                End If
            ElseIf bIsSubAgent Then
                'fetch data from XML of output_commission object, grouped on policy binder id and find sum of comm percent and comm amount
                Dim CommissionOutputTotals1 = _
                    From CommissionOutputs In docX.Descendants("DATA_SET").Descendants("RISK_OBJECTS").Descendants(sDataModelCode & "_POLICY_BINDER").Descendants(sDataModelCode & "_OUTPUT_COMMISSION") _
                    Where (Convert.ToDecimal(CommissionOutputs.Attribute("IS_LEAD").Value) = 0 And Convert.ToDecimal(CommissionOutputs.Attribute("IS_RETAINED").Value) = 0 And CommissionOutputs.Attribute("COB_CODE").Value <> "0") _
                 Group CommissionOutputs By PolicyBinder = CommissionOutputs.Attribute(strPolicyBinder).Value _
                 Into TotalCommPerc = Sum(Convert.ToDecimal(CommissionOutputs.Attribute("COMMISSION_PERCENT_OVERRIDEN").Value)), _
                 TotalNetAnnualComm = Sum(Convert.ToDecimal(CommissionOutputs.Attribute(sCommFieldName).Value))
                'Check if CommissionOutputTotals1 has got some data
                If CommissionOutputTotals1 IsNot Nothing Then
                    If CommissionOutputTotals1.Count > 0 Then
                        'fetch data from XML of output_commission object, grouped on policy binder id and find sum of comm percent and comm amount
                        Dim lblTotalCommPer As Label = grdvCommission.FooterRow.FindControl("lblTotalCommPer")
                        Dim lblTotalCommAmount As Label = grdvCommission.FooterRow.FindControl("lblTotalCommAmount")
                        lblTotalCommPer.Text = CommissionOutputTotals1.ElementAt(0).TotalCommPerc.ToString
                        lblTotalCommAmount.Text = CommissionOutputTotals1.ElementAt(0).TotalNetAnnualComm.ToString
                    End If
                End If
            End If
            'check if policy refer then do not display primium
            If Not CheckRefer() Then
                divTotals.Visible = True
                Dim dPremium As Double, dTax As Double, dTaxRate As Double, dTotalPremium As Double, dSumInsured As Double, dFee As Double
                'calculate total premium and sum insured
                CalculatePremiumAndTax(dPremium, dTax, dTaxRate, dTotalPremium, dSumInsured, dFee)
                'assign the values of calculated totals
                lblPremiumValue.Text = New Money(dPremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                lblIPTValue.Text = New Money(dTax, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
                lblTotalPremiumValue.Text = New Money(dTotalPremium, New Currency(CType(Session.Item(CNCurrenyCode), String)).Type).Formatted.ToString
            Else
                divTotals.Visible = False
            End If
            If oQuote IsNot Nothing Then
                'Set the header for page                     
                lblHeader.Text = "Online Quotation : " & oQuote.ProductName
            End If
        End Sub

        ''' <summary>
        ''' to save the data to xml and re run update risk
        ''' </summary>
        ''' <param name="iRowIndex"></param>
        ''' <remarks></remarks>
        Private Sub SaveData(Optional ByVal iRowIndex As Integer = -1)

            Dim strQuery As String
            Dim oWebService As NexusProvider.ProviderBase
            Dim oQuote As NexusProvider.Quote
            'retrieve the oQuote from session
            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
            If iRowIndex = -1 Then 'save coming from edit links
                For iCount As Integer = 0 To grdvCommission.Rows.Count - 1
                    'Fetch the row from XML based on OI key
                    strQuery = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & CType(grdvCommission.Rows(iCount).Cells(11).FindControl("lblOI"), Label).Text & "']"
                    'update the commission percentage in xml
                    Dim dCommPer As Double
                    If CType(grdvCommission.Rows(iCount).Cells(4).FindControl("txtCommPer"), TextBox) IsNot Nothing Then
                        'save the overriden comm  percentage in the variable
                        dCommPer = Convert.ToDouble(CType(grdvCommission.Rows(iCount).Cells(4).FindControl("txtCommPer"), TextBox).Text.Trim)
                        'Update XML - with overriden comm  percentage
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", CType(grdvCommission.Rows(iCount).Cells(4).FindControl("txtCommPer"), TextBox).Text.Trim)
                    ElseIf CType(grdvCommission.Rows(iCount).Cells(4).FindControl("txtCommPercen"), TextBox) IsNot Nothing Then
                        'save the overriden comm  percentage in the variable
                        dCommPer = Convert.ToDouble(CType(grdvCommission.Rows(iCount).Cells(4).FindControl("txtCommPercen"), TextBox).Text.Trim)
                        'Update XML - with overriden comm  percentage
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", CType(grdvCommission.Rows(iCount).Cells(4).FindControl("txtCommPercen"), TextBox).Text.Trim)
                    End If
                    'update premium break down object. set IS_OVERRIDEN =1
                    If Convert.ToDouble(CType(grdvCommission.Rows(iCount).Cells(1).FindControl("lblCommPerOverridenPrev"), HiddenField).Value.Trim) <> dCommPer Then
                        UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                    End If
                    'Update XML - with the reason for change
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "REASON_FOR_OVERRIDE", txtReason.Text.Trim)
                Next
            Else
                strQuery = "/DATA_SET/RISK_OBJECTS/" & sDataModelCode & "_POLICY_BINDER/" & sDataModelCode & "_OUTPUT_COMMISSION[@OI='" & CType(grdvCommission.Rows(iRowIndex).Cells(11).FindControl("lblOI"), Label).Text & "']"
                'update the commission percentage in xml
                Dim dCommPer As Double
                If CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPer"), TextBox) IsNot Nothing Then
                    'save the overriden comm  percentage in the variable
                    dCommPer = Convert.ToDouble(CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPer"), TextBox).Text.Trim)
                    'Update XML - with overriden comm  percentage
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPer"), TextBox).Text.Trim)
                ElseIf CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPercen"), TextBox) IsNot Nothing Then
                    'save the overriden comm  percentage in the variable
                    dCommPer = Convert.ToDouble(CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPercen"), TextBox).Text.Trim)
                    'Update XML - with overriden comm  percentage
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "COMMISSION_PERCENT_OVERRIDEN", CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPercen"), TextBox).Text.Trim)
                End If
                'update commission output object. set IS_OVERRIDEN =1
                If Convert.ToDouble(CType(grdvCommission.Rows(iRowIndex).Cells(1).FindControl("lblCommPerOverridenPrev"), HiddenField).Value.Trim) <> dCommPer Then
                    UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "IS_OVERRIDEN", 1)
                End If
                'Update XML - with the reason for change
                UpdateXML(strQuery, sDataModelCode & "_OUTPUT_COMMISSION", "REASON_FOR_OVERRIDE", txtReason.Text.Trim)
                grdvCommission.EditIndex = -1 'disable the edit mode
                BindData() 'bind the grid
                'edit link button will be visible and save/cancel buttons will not be visible since edit mode in disabled.
                grdvCommission.Rows(iRowIndex).Cells(10).FindControl("lnkEdit").Visible = True
                grdvCommission.Rows(iRowIndex).Cells(10).FindControl("lnkSave").Visible = False
                grdvCommission.Rows(iRowIndex).Cells(10).FindControl("lnkCancel").Visible = False
                'display the exclamation size in case comm percentage has been overriden
                If CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPercen"), TextBox) IsNot Nothing And _
                    (CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPercen"), TextBox).Text.Trim <> CType(grdvCommission.Rows(iRowIndex).Cells(11).FindControl("lblOriginalCommPer"), HiddenField).Value) Then
                    CType(grdvCommission.Rows(iRowIndex).Cells(4).FindControl("txtCommPercen"), TextBox).Attributes.Add("Class", "updated")

                End If
            End If
            'retrieve the oQuote from session
            oQuote = System.Web.HttpContext.Current.Session(CNQuote)
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'call update risk to rerun rating scripts
                If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTA")
                ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTC")
                ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, , "MTR")
                ElseIf Session(CNRenewal) Then
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey), Nothing, Nothing, "REN")
                Else
                    oWebService.UpdateRisk(oQuote, Session(CNCurrentRiskKey))
                End If
                'error handling - in case quote is referred or declined
            Catch ex As NexusProvider.NexusException
                If ex.Errors(0).Code = "277" Or ex.Errors(0).Code = "279" Then
                ElseIf ex.Errors(0).Code = "278" Or ex.Errors(0).Code = "280" Then
                End If
            Finally
                oWebService = Nothing  'clear the object
            End Try
            'create new instance of proxy
            oWebService = New NexusProvider.ProviderManager().Provider
            Try
                'call GetHeaderAndRisksByKey to retrieve the premium, tax etc.
                oWebService.GetHeaderAndRisksByKey(oQuote)
            Catch ex As NexusProvider.NexusException

            Finally
                oWebService = Nothing  'clear the object
            End Try
            'save the oQuote to session
            System.Web.HttpContext.Current.Session(CNQuote) = oQuote
            BindData()  'Bind the grid

        End Sub

        ''' <summary>
        ''' on click of save after entering 'reason for change', XML is updated with the 
        ''' changed rates and reason for change and update risk is called
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnApplySave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApplySave.Click

            'When we save from link buttons we have session RowIndex
            If Session("RowIndex") IsNot Nothing Then
                SaveData(Session("RowIndex")) 'update the XML with all overriden values and call update risk to again run the script so as to calculate changed premium
                Session.Remove("RowIndex") 'clear the session
            Else
                SaveData()  'update the XML with all overriden values and call update risk to again run the script so as to calculate changed premium
                Dim bPostBack As Boolean = True
                'check if post required or not
                If Request.QueryString("PostBack") IsNot Nothing Then
                    bPostBack = Convert.ToBoolean(Request.QueryString("PostBack"))
                End If
                If bPostBack Then
                    Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                    'refresh the parent page on postback with event argument RefreshGrid  
                    Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                End If
                'close the modal page
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
            End If

        End Sub

        'do not delete this
        Protected Sub grdvCommission_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdvCommission.RowEditing

        End Sub

    End Class
End Namespace



