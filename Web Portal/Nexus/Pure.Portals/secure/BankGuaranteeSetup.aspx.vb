Imports CMS.library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports NexusProvider.SAMForInsurance
Imports Nexus.Library
Imports Nexus.Utils
Imports System.Configuration.ConfigurationManager

Namespace Nexus

    Partial Class Secure_BankGuaranteeSetup : Inherits Frontend.clsCMSPage

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
          
            'To set the Focus
            Page.SetFocus(btnBankName)

            If Not IsPostBack Then
                'As there is no SAM call for Wildcard search for Product and Branch,
                'So all the Products and Branches are being filled in page load.
                txtBankName.Attributes.Add("readonly", "readonly")

                If Request.QueryString("BankGuaranteeMode") = "Edit" Or Request.QueryString("BankGuaranteeMode") = "View" Then
                    Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                    Dim oBankGuarantee As New NexusProvider.BankGuarantee
                    Dim iBGKey As Integer = Request.QueryString("BGKey")
                    oWebService.GetBankGuarantee(oBankGuarantee, iBGKey)

                    With oBankGuarantee

                        Me.txtBankName.Text = .BankNameDescription.Trim
                        txtBankNameKey.Value = .BankNameKey
                        txtBankBranch.Text = .BankBranch
                        txtNumber.Text = .BankGuaranteeReference
                        ddlBGCustodyBranch.Value = .CustodyBranchCode.Trim
                        ddlCurrency.Value = .CurrencyCode.Trim
                        txtAmount.Text = .Limit
                        txtLimitsAvl.Text = .LimitAvailable
                        txtLimitsAvlKey.Value = .LimitAvailable
                        txtIssueDate.Text = .IssueDate
                        txtExpiryDate.Text = .ExpiryDate
                        chkSinglePolicyLock.Checked = .PolicyLock
                        txtPartyKey.Value = .PartyKey
                        ViewState("TimeStamp") = .TimeStamp

                        'populating of branch pick list with the previous selected values (Edit Mode)
                        If .Branches.Count > 0 Then
                            'Calling on controls functionalities SetSelectedValues to set the values in Pick list
                            pckBranch.SetSelectedValues(.Branches)
                        End If

                        'populating of product pick list with the previous selected values (Edit Mode)
                        If .Products.Count > 0 Then
                            'Calling on controls functionalities SetSelectedValues to set the values in Pick list
                            pckProduct.SetSelectedValues(.Products)
                        End If

                        If Request.QueryString("BankGuaranteeMode") = "View" Then
                            Polices.Visible = True
                            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
                            DisableControls(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName))
                            calIssueDate.Enabled = False
                            calExpiryDate.Enabled = False
                            pckBranch.Enabled = False
                            pckProduct.Enabled = False
                            oWebService = New NexusProvider.ProviderManager().Provider
                            Dim oPolicyCollection As NexusProvider.PolicyCollection
                            oPolicyCollection = oWebService.GetPoliciesOnBankGuaranteeByKey(iBGKey)
                            grdvBankGuaranteePolicies.DataSource = oPolicyCollection
                            grdvBankGuaranteePolicies.DataBind()
                        Else
                            Polices.Visible = False
                        End If
                    End With
                End If
            End If
        End Sub

        ''' <summary>
        ''' Add/update the BankGuarantee
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click
            If Page.IsValid Then
                If Request.QueryString("BankGuaranteeMode") = "View" Then
                    Response.Redirect("../secure/BankGuarantee.aspx", False)
                End If
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oBankGuarantee As New NexusProvider.BankGuarantee
                Dim oBankGuaranteeCollection As New NexusProvider.BankGuaranteeCollection
                Dim iCount As Integer = 0
                Dim oProductCollection As New NexusProvider.ProductCollection
                Dim oBranchCollection As New NexusProvider.BranchCollection
                With oBankGuarantee
                    .BankNameKey = txtBankNameKey.Value
                    .BankNameDescription = txtBankName.Text
                    .BankBranch = txtBankBranch.Text

                    If Request.QueryString("BankGuaranteeMode") = "Edit" Then
                        .PartyKey = txtPartyKey.Value
                    Else
                        .PartyKey = Request.QueryString("PartyKey").Trim
                    End If

                    .BankGuaranteeReference = txtNumber.Text
                    .CustodyBranchCode = ddlBGCustodyBranch.Value
                    .CurrencyCode = ddlCurrency.Value
                    .BankGuaranteeRef = txtNumber.Text
                    .BGLimit = txtAmount.Text
                    .LimitAvailable = txtLimitsAvlKey.Value
                    .IssueDate = txtIssueDate.Text
                    .ExpiryDate = txtExpiryDate.Text
                    .PolicyLock = chkSinglePolicyLock.Checked
                    .Deleted = False
                    .BGStatusCode = "Active"
                    .TimeStamp = ViewState("TimeStamp")

                    'Retreiving the values from the product pick list with function GetSelectedItems
                    Dim oProductListColl As ListItemCollection
                    Dim iProductCount As Integer = 0
                    oProductListColl = pckProduct.GetSelectedItems()
                    For Each oListItem As ListItem In oProductListColl

                        If oListItem.Selected Then
                            Dim oProduct As New NexusProvider.Product
                            oProduct.Description = oListItem.Text
                            oProduct.ProductCode = oListItem.Value
                            .Products.Add(oProduct)
                            oProductCollection.Add(oProduct)
                            oProduct = Nothing
                            iProductCount += 1
                        End If

                    Next

                    If iProductCount = 0 Then
                        ''Validate the Product
                        ''Atleast one Product must be selected
                        VldProductAndBranch.ErrorMessage = GetLocalResourceObject("lbl_InvalidProduct_err")
                        VldProductAndBranch.Enabled = True
                        VldProductAndBranch.IsValid = False
                        Exit Sub
                    Else
                        VldProductAndBranch.Enabled = False
                        VldProductAndBranch.IsValid = True
                    End If

                    'Retreiving the values from the branch pick list with function GetSelectedItems
                    Dim oBranchListColl As ListItemCollection
                    Dim iBranchCount As Integer = 0
                    oBranchListColl = pckBranch.GetSelectedItems()
                    For Each oListItem As ListItem In oBranchListColl
                        If oListItem.Selected Then
                            Dim oBranch As New NexusProvider.Branch(oListItem.Value, oListItem.Text)
                            .Branches.Add(oBranch)
                            oBranchCollection.Add(oBranch)
                            oBranch = Nothing
                            iBranchCount += 1
                        End If
                    Next

                    If iBranchCount = 0 Then
                        ''Validate the Branch
                        ''Atleast one Branch must be selected
                        VldProductAndBranch.ErrorMessage = GetLocalResourceObject("lbl_InvalidBranch_err")
                        VldProductAndBranch.Enabled = True
                        VldProductAndBranch.IsValid = False
                        Exit Sub
                    Else
                        VldProductAndBranch.Enabled = False
                        VldProductAndBranch.IsValid = True
                    End If

                End With
                If Request.QueryString("BankGuaranteeMode") = "Add" Then
                    oBankGuarantee.BGKey = 0
                    oBankGuaranteeCollection.Add(oBankGuarantee)
                    Session("BankGuarantee") = oBankGuaranteeCollection
                    oWebService.AddBankGuarantee(oBankGuaranteeCollection)
                    Response.Redirect("../secure/BankGuarantee.aspx", False)
                ElseIf Request.QueryString("BankGuaranteeMode") = "View" Then
                    Response.Redirect("../secure/BankGuarantee.aspx", False)
                ElseIf Request.QueryString("BankGuaranteeMode") = "Edit" Then
                    oBankGuarantee.BGKey = Request.QueryString("BGKey")
                    oBankGuaranteeCollection.Add(oBankGuarantee)
                    oWebService.UpdateBankGuarantee(oBankGuaranteeCollection, oBranchCollection, oProductCollection)
                    Response.Redirect("../secure/BankGuarantee.aspx", False)
                End If
            End If
        End Sub

        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            'CMS.Library.Frontend.Functions.SetTheme(Page, System.Configuration.ConfigurationManager.AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            If HttpContext.Current.Session.IsCookieless Then
                btnBankName.OnClientClick = "tb_show(null ,'../Modal/FindBank.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=400&width=500' , null);return false;"
            Else
                btnBankName.OnClientClick = "tb_show(null ,'" & System.Configuration.ConfigurationManager.AppSettings("WebRoot") & "/Modal/FindBank.aspx?modal=true&KeepThis=true&FromPage=PC&TB_iframe=true&height=400&width=500' , null);return false;"
            End If
        End Sub

    End Class
End Namespace
