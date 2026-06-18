Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Configuration.ConfigurationManager

Namespace Nexus

    Partial Class BankGuarantee : Inherits Frontend.clsCMSPage

        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())


        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            btnAdd.Attributes.Add("onclick", "javascript:return setAgentorClient();")
          
            'To set the Focus
            Page.SetFocus(btnClient)

            If String.IsNullOrEmpty(txtClientKey.Value) And String.IsNullOrEmpty(txtAgentKey.Value) Then
                btnAdd.Enabled = False
            Else
                btnAdd.Enabled = True
            End If

            If txtPolicyRefKey.Value.Trim() IsNot Nothing AndAlso txtInsuranceFile.Text.Trim() <> String.Empty Then
                GetClientKeyFromInsuranceRef()
            End If
        End Sub

        Sub fillGrid()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oBankGuaranteeCollection As New NexusProvider.BankGuaranteeCollection
            Dim oBankGuarantee As New NexusProvider.BankGuarantee
            grdvBankGuarantee.Visible = True

            Try
                With oBankGuarantee

                    If Not String.IsNullOrEmpty(txtClient.Text) Then
                        .PartyCode = txtClient.Text.Trim()
                    End If

                    If Not String.IsNullOrEmpty(txtBankGuaranteeRef.Text) Then
                        .BankGuaranteeRef = txtBankGuaranteeRef.Text.Trim()
                    End If

                    If Not String.IsNullOrEmpty(txtAgent.Text) Then
                        .AgentCode = txtAgent.Text.Trim()
                    End If

                    If Not String.IsNullOrEmpty(txtInsuranceFile.Text.Trim()) Then
                        ' GetPolicyDetails()

                        'If String.IsNullOrEmpty(Me.txtPolicyRefKey.Value) = False AndAlso Me.txtPolicyRefKey.Value <> "0" Then
                        '    .InsuranceFileKey = CInt(Me.txtPolicyRefKey.Value)
                        'End If
                        .InsuranceRef = txtInsuranceFile.Text.Trim()

                    End If

                    If Not String.IsNullOrEmpty(txtBankName.Text) Then
                        .BankNameCode = txtBankName.Text.Trim()
                    End If

                    .BGStatusCode = ddlBGStatus.Value

                    'to limit the search return from SAM
                    .MaxRowsToFetch = oPortal.MaxSearchResults

                End With

                oBankGuaranteeCollection = oWebService.FindBankGuarantee(oBankGuarantee)

                grdvBankGuarantee.Visible = True
                grdvBankGuarantee.AllowPaging = True
                grdvBankGuarantee.DataSource = oBankGuaranteeCollection
                grdvBankGuarantee.DataBind()

                If oBankGuaranteeCollection IsNot Nothing Then
                    'validate size of dataset. if 500(configured at portal level) or more results are returned then add a validation message to the screen
                    If oBankGuaranteeCollection.Count >= oPortal.MaxSearchResults Then
                        'create a custom validator
                        Dim cstMaxResults As New CustomValidator
                        cstMaxResults.IsValid = False
                        'look for a validation message in the page resources, but if there is not one defined add a default message
                        cstMaxResults.ErrorMessage = IIf(GetLocalResourceObject("cstMaxResults") Is Nothing, "Maximum number of search results exceeded, please refine your search criteria", GetLocalResourceObject("cstMaxResults"))
                        cstMaxResults.Display = ValidatorDisplay.None 'we only want the error messages in the validation summary
                        'add the validator to the page, this will have the effect of making the page invalid
                        Page.Validators.Add(cstMaxResults)
                    End If

                End If

            Finally
                oWebService = Nothing
                oBankGuaranteeCollection = Nothing
                oBankGuarantee = Nothing
                oPortal = Nothing
                oNexusConfig = Nothing
                txtPolicyRefKey.Value = Nothing
            End Try

        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            fillGrid()
        End Sub

        Protected Sub grdvBankGuarantee_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdvBankGuarantee.DataBound
            If grdvBankGuarantee.Rows.Count = 0 Or grdvBankGuarantee.PageCount = 1 Then
                grdvBankGuarantee.AllowPaging = False
            End If
        End Sub

        Protected Sub grdvBankGuarantee_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvBankGuarantee.PageIndexChanging
            grdvBankGuarantee.PageIndex = e.NewPageIndex
            fillGrid()
        End Sub

        Protected Sub grdvBankGuarantee_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBankGuarantee.RowCreated

            'Hide the BGKey column
            If e.Row.RowType = DataControlRowType.DataRow Or e.Row.RowType = DataControlRowType.Header Then
                e.Row.Cells(2).Visible = False
            End If

        End Sub

        Protected Sub grdvBankGuarantee_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvBankGuarantee.RowDataBound

            If e.Row.RowType = DataControlRowType.DataRow Then

                Dim oBankGuarantee As NexusProvider.BankGuarantee
                Dim sBGKey, sStatusDescription As String

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("ClaimKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.BankGuarantee).BGKey)


                oBankGuarantee = CType(e.Row.DataItem, NexusProvider.BankGuarantee)

                'collect the required details into local variables to assign
                sBGKey = oBankGuarantee.BGKey
                sStatusDescription = oBankGuarantee.StatusDescription

                Dim liDelete As HtmlGenericControl = e.Row.Cells(10).FindControl("liDelete")
                Dim likEdit As HtmlGenericControl = e.Row.Cells(10).FindControl("liEdit")
                Dim likView As HtmlGenericControl = e.Row.Cells(10).FindControl("liView")
                Dim lnkDelete As LinkButton = e.Row.Cells(10).FindControl("lnkDelete")
                Dim lnkEdit As LinkButton = e.Row.Cells(10).FindControl("lnkEdit")
                Dim lnkView As LinkButton = e.Row.Cells(10).FindControl("lnkView")

                If sStatusDescription <> "Active" Then

                    ' "Delete" and "Edit" options should be hide, only "View" will be visible.
                    lnkDelete.Visible = False
                    lnkEdit.Visible = False
                    liDelete.Visible = False
                    likEdit.Visible = False

                    lnkView.PostBackUrl = "../secure/BankGuaranteeSetup.aspx?BGkey=" + sBGKey + "&BankGuaranteeMode=View"

                Else

                    ' "Delete" and "Edit" both options should be visible, "View" should be hide.
                    lnkView.Visible = False
                    likView.Visible = False

                    lnkEdit.PostBackUrl = "../secure/BankGuaranteeSetup.aspx?BGkey=" + sBGKey + "&BankGuaranteeMode=Edit"
                End If

                'cleaning up
                oBankGuarantee = Nothing
                sBGKey = Nothing
                sStatusDescription = Nothing

            ElseIf e.Row.RowType = DataControlRowType.Header Then


            End If

        End Sub

        Protected Sub grdvBankGuarantee_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles grdvBankGuarantee.RowDeleting
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oBankGuaranteeCollection As New NexusProvider.BankGuaranteeCollection
            Dim oBankGuarantee As New NexusProvider.BankGuarantee
            Try

                oBankGuarantee.BGKey = grdvBankGuarantee.Rows(e.RowIndex).Cells(2).Text
                oBankGuarantee.BankGuaranteeRef = grdvBankGuarantee.Rows(e.RowIndex).Cells(1).Text
                oBankGuarantee.Deleted = True
                oBankGuarantee.DeletedFieldSpecified = True
                oBankGuarantee.ActionType = NexusProvider.ActionType.DelUnDel
                oBankGuaranteeCollection.Add(oBankGuarantee)
                oWebService.UpdateBankGuaranteeConditionally(oBankGuaranteeCollection)
                fillGrid()

            Catch ex As System.Exception
            Finally
                oWebService = Nothing
                oBankGuaranteeCollection = Nothing
                oBankGuarantee = Nothing
            End Try
        End Sub

        Protected Sub GetPolicyDetails()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oInsuranceFileSearchCriteria As New NexusProvider.InsuranceFileDetails
            Dim oInsuranceFileDetails As NexusProvider.InsuranceFileDetailsCollection = Nothing
            Dim oGrdInsuranceFileDetails As New NexusProvider.InsuranceFileDetailsCollection
            Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
            Dim sBranchCode As String = oUserDetails.ListOfBranches(0).Code
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
            Dim iMaxSearchResults As Integer = 0

            If String.IsNullOrEmpty(oPortal.MaxSearchResults) = False Then
                iMaxSearchResults = CInt(oPortal.MaxSearchResults)
            End If

            With oInsuranceFileSearchCriteria
                .InsuranceRef = Trim(txtInsuranceFile.Text)
            End With

            Try
                oInsuranceFileDetails = oWebService.FindPolicy(Trim(txtInsuranceFile.Text), "", "", NexusProvider.InsuranceFileTypes.ALL, False, iMaxSearchResults, sBranchCode)

                If oInsuranceFileDetails IsNot Nothing AndAlso oInsuranceFileDetails.Count > 0 Then
                    Me.txtPolicyRefKey.Value = oInsuranceFileDetails(0).InsuranceFileKey
                    Me.txtClient.Text = oInsuranceFileDetails(0).ClientShortName
                End If
                
            Finally
                oInsuranceFileSearchCriteria = Nothing
                oWebService = Nothing
            End Try
        End Sub

        Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Me.txtAgent.Text = ""
            Me.txtAgentKey.Value = ""
            Me.txtBankGuaranteeRef.Text = ""
            Me.txtBankName.Text = ""
            Me.txtClient.Text = ""
            Me.txtInsuranceFile.Text = ""
            grdvBankGuarantee.Visible = False
            btnAdd.Enabled = False
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "AgentConfirmation", _
                "<script language=""JavaScript"" type=""text/javascript"">function AgentConfirmation(){return confirm('" & Replace(GetLocalResourceObject("msg_AgentConfirm").ToString(), "#AgentName", "' + document.getElementById('" & txtAgent.ClientID & "').value.replace(/^\s\s*/, '').replace(/\s\s*$/, '') + '") & "'); }</script>")


            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ClientConfirmation", _
                "<script language=""JavaScript"" type=""text/javascript"">function ClientConfirmation(){return confirm('" & Replace(GetLocalResourceObject("msg_ClientConfirm").ToString(), "#ClientName", "' + document.getElementById('" & txtClient.ClientID & "').value.replace(/^\s\s*/, '').replace(/\s\s*$/, '') + '") & "'); }</script>")
        End Sub

        Private Sub GetClientKeyFromInsuranceRef()
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote
            Dim iInsuranceFileKey As Integer
            Try
                If txtPolicyRefKey.Value IsNot Nothing AndAlso txtPolicyRefKey.Value <> "" Then
                    iInsuranceFileKey = Convert.ToInt32(txtPolicyRefKey.Value.Trim())
                    If iInsuranceFileKey <> 0 Then
                        oQuote = oWebService.GetHeaderAndSummariesByKey(iInsuranceFileKey)
                        txtClientKey.Value = oQuote.PartyKey
                        If String.IsNullOrEmpty(txtClientKey.Value) Then
                            btnAdd.Enabled = False
                        Else
                            btnAdd.Enabled = True
                        End If
                    End If
                End If
                Me.txtInsuranceFile.Focus()
            Finally
                oQuote = Nothing
            End Try
        End Sub
        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            If hChoice.Value IsNot Nothing Then
                If hChoice.Value.Trim.ToUpper = "TRUE" Then
                    If txtAgentKey.Value IsNot Nothing AndAlso txtAgentKey.Value.Trim.Length <> 0 Then
                        Response.Redirect("../secure/BankGuaranteeSetup.aspx?BankGuaranteeMode=Add&PartyKey=" & txtAgentKey.Value, False)
                    ElseIf txtClientKey.Value IsNot Nothing AndAlso txtClientKey.Value.Trim.Length <> 0 Then
                        Response.Redirect("../secure/BankGuaranteeSetup.aspx?BankGuaranteeMode=Add&PartyKey=" & txtClientKey.Value, False)
                    End If
                End If
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Secure/Agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;"
                btnAgent.OnClientClick = "tb_show(null ,'../Modal/FindAgent.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750' , null);return false;"
                btnInsuranceFile.OnClientClick = "tb_show(null ,'../Modal/FindInsuranceFile.aspx?Page=RS&modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750' , null);return false;"
                btnBankName.OnClientClick = "tb_show(null ,'../Modal/FindBank.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                btnClient.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Secure/Agent/FindClient.aspx?modal=true&RequestPage=BG&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;"
                btnAgent.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindAgent.aspx?modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750' , null);return false;"
                btnInsuranceFile.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindInsuranceFile.aspx?Page=RS&modal=true&KeepThis=true&FromPage=BG&TB_iframe=true&height=500&width=750' , null);return false;"
                btnBankName.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/FindBank.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
        End Sub
    End Class
End Namespace
