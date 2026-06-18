Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Reflection
Imports System.Linq

Namespace Nexus

    Partial Class Modal_OtherPartyDetails
        Inherits BaseOtherParty

        Protected Shadows Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))

        End Sub

        Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            If Request.QueryString("PartyType") IsNot Nothing Then
                Session(CNPartyType) = Request.QueryString("PartyType").Trim()
            End If

        End Sub

        Private Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
            If Not IsPostBack Then
                Dim oUserDetails As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                If oUserDetails.ListOfBranches.Count > 1 Then
                    'Sort the branches
                    oUserDetails.ListOfBranches.SortColumn = "Description"
                    oUserDetails.ListOfBranches.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
                    oUserDetails.ListOfBranches.Sort()
                End If
                ddlBranch.DataSource = oUserDetails.ListOfBranches
                ddlBranch.DataBind()
                ddlBranch.Items.Insert(0, New ListItem("(none)", ""))
                ddlBranch.SelectedValue = Session(CNBranchCode)

                FillSubBranches(ddlBranch.SelectedValue)
                FillCurrency()
            End If
        End Sub

        Protected Sub ddlBranch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlBranch.SelectedIndexChanged
            FillSubBranches(ddlBranch.SelectedValue.Trim())
        End Sub

        ''' <summary>
        ''' This methods fill the sub branches by default or based on the value passed in it.
        ''' </summary>
        ''' <param name="oBranchCode"></param>
        ''' <remarks></remarks>
        Private Sub FillSubBranches(Optional ByVal oBranchCode As String = Nothing)
            'Fill Sub Branch
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oLookup As New NexusProvider.LookupListCollection
            'sam call to retreive the list of branch from table source
            oLookup = oWebService.GetList(NexusProvider.ListType.PMLookup, "Source", False, False, "Source_ID")
            'Retreival of the Branch Key, which will latet identify the sub-branch
            Dim iBranchKey As Integer = 0
            For iBranchCount As Integer = 0 To oLookup.Count - 1
                If oLookup(iBranchCount).Code = oBranchCode Then
                    iBranchKey = oLookup(iBranchCount).Key
                    Exit For
                End If
            Next
            If oBranchCode Is Nothing OrElse String.IsNullOrEmpty(oBranchCode) OrElse iBranchKey = 0 Then
                ddlSubBranch.Items.Clear()
                ddlSubBranch.Items.Insert(0, New ListItem("(none)", ""))
            Else
                'sam call to retreive the list of sub-branch from table source
                oLookup = Nothing
                oLookup = oWebService.GetList(NexusProvider.ListType.PMLookup, "Sub_Branch", True, False, "Source_ID", iBranchKey, Session(CNTransBranchCode))

                ddlSubBranch.Items.Clear()
                For iSubBranchCount As Integer = 0 To oLookup.Count - 1
                    Dim lstSubBranch As New ListItem
                    lstSubBranch.Text = oLookup(iSubBranchCount).Description
                    lstSubBranch.Value = Trim(oLookup(iSubBranchCount).Code)
                    ddlSubBranch.Items.Add(lstSubBranch)
                    ddlSubBranch.DataBind()
                Next
                ddlSubBranch.Items.Insert(0, New ListItem("(none)", ""))
            End If
        End Sub

        Private Sub FillCurrency()

            Dim webService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCurrencyColl As NexusProvider.CurrencyCollection

            If Not String.IsNullOrEmpty(ddlBranch.SelectedValue) Then
                oCurrencyColl = webService.GetCurrenciesByBranch(ddlBranch.SelectedValue)
            Else
                oCurrencyColl = webService.GetCurrenciesByBranch(Session(CNBranchCode))
            End If

            'sort the collection before binding
            oCurrencyColl.SortColumn = "Description"
            oCurrencyColl.SortingOrder = NexusProvider.GenericComparer.SortOrder.Ascending
            oCurrencyColl.Sort()
            ddlCurrency.Items.Clear()
            For i As Integer = 0 To oCurrencyColl.Count - 1
                Dim lstCurrency As New ListItem
                lstCurrency.Text = oCurrencyColl.Item(i).Description.ToString
                lstCurrency.Value = Trim(oCurrencyColl.Item(i).CurrencyCode.ToString)
                ddlCurrency.Items.Add(lstCurrency)
            Next
            ddlCurrency.DataBind()
            ddlCurrency.SelectedValue = oCurrencyColl(0).BaseCurrencyCode
        End Sub

        Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
            If Page.IsValid Then

                CollectPartyData()
                If Not VldBranchPickList.IsValid Then
                    Exit Sub
                End If

                BtnSubmitClientClick(sender, e)

                ScriptManager.RegisterClientScriptBlock(Me.Page, GetType(String), "SendOtherPartyData", "self.parent.ReceiveOtherPartyData('" + DirectCast(Session(CNParty), NexusProvider.OtherParty).ShortName + "');", True)

            End If
        End Sub

        Private Sub CollectPartyData()

            With CType(Session(CNParty), NexusProvider.OtherParty)
                .TypeCode = Session(CNPartyType)
                .Name = txtName.Text
                .Code = txtCode.Text
                .ShortName = txtCode.Text
                If Not String.IsNullOrEmpty(txtDOB.Text) Then
                    If Not (txtDOB.Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper()) Then
                        .DateOfBirth = CType(txtDOB.Text, Date)
                        .DOB = CType(txtDOB.Text, Date).ToString()
                    End If
                Else
                    .DateOfBirth = Nothing
                    .DOB = Nothing
                End If
                .Gender = ddlGender.Value
                .LicenseTypeCode = Licence_Type.Value
                .LicenseNumber = txtLicenceNO.Text
                .DriverStatusCode = ddlDriverStatus.Value
                .RegistrationNumber = txtRegistrationNO.Text
                .BranchCode = ddlBranch.SelectedValue
                .SubBranchCode = ddlSubBranch.SelectedValue
                .Currency = ddlCurrency.SelectedValue
                .IsTPASettleDirectly = IIf(String.IsNullOrEmpty(ddlTPASettle.SelectedValue.Trim()), 0, ddlTPASettle.SelectedValue.Trim())

                ' Tax Data
                .TaxNumber = txtTaxNumber.Text
                If Not String.IsNullOrEmpty(txtTaxPercentage.Text) Then
                    .TaxPercentage = txtTaxPercentage.Text
                Else
                    .TaxPercentage = 0
                End If
                .TaxExempt = chkTaxExempt.Checked
                .DomiciledForTax = chkIsDomiciledForTax.Checked

            End With

            Dim oBranchCollection As New NexusProvider.BranchCollection
            Dim oSupplierBusinessCollection As New NexusProvider.SupplierBusinessCollection

            'Retreiving the values from the branch pick list
            Dim iBranchCount As Integer = 0
            For Each oListItem As ListItem In BranchPickList.GetSelectedItems()
                If oListItem.Selected Then
                    Dim oBranch As New NexusProvider.Branch(oListItem.Value, oListItem.Text)
                    oBranch.BranchKey = GetKeyForDescription(NexusProvider.ListType.PMLookup, oBranch.Description, "source", False)
                    oBranchCollection.Add(oBranch)
                    oBranch = Nothing
                    iBranchCount += 1
                End If
            Next
            If iBranchCount = 0 Then
                VldBranchPickList.ErrorMessage = GetLocalResourceObject("err_VldBranchPickList")
                VldBranchPickList.Enabled = True
                VldBranchPickList.IsValid = False
                Exit Sub
            Else
                VldBranchPickList.Enabled = False
                VldBranchPickList.IsValid = True
            End If
            CType(Session(CNParty), NexusProvider.OtherParty).Branches = oBranchCollection

            'Retreiving the values from the supply pick list
            Dim iSupplyCount As Integer = 0
            For Each oListItem As ListItem In SupplyPickList.GetSelectedItems()
                If oListItem.Selected Then
                    Dim oSupplierBussiness As New NexusProvider.SupplierBusiness()
                    oSupplierBussiness.BusinessCode = oListItem.Value
                    oSupplierBusinessCollection.Add(oSupplierBussiness)
                    oSupplierBussiness = Nothing
                    iSupplyCount += 1
                End If
            Next

            'Retreiving the values from the speciality pick list
            Dim iSpecialityCount As Integer = 0
            For Each oListItem As ListItem In SpecialityPickList.GetSelectedItems()
                If oListItem.Selected Then
                    If iSpecialityCount <= iSupplyCount Then
                        oSupplierBusinessCollection(iSpecialityCount).SpecialityCode = oListItem.Value
                    Else
                        Dim oSupplierBussiness As New NexusProvider.SupplierBusiness()
                        oSupplierBussiness.SpecialityCode = oListItem.Value
                        oSupplierBusinessCollection.Add(oSupplierBussiness)
                        oSupplierBussiness = Nothing
                    End If
                    iSpecialityCount += 1
                End If
            Next

            CType(Session(CNParty), NexusProvider.OtherParty).SupplierBusinesses = oSupplierBusinessCollection

        End Sub

        Protected Sub cusvldBranch_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)

            If ddlBranch.SelectedItem.Value.Trim = "" OrElse
                ddlBranch.SelectedItem.Text.Trim.ToUpper = "(NONE)" Then
                args.IsValid = False
            End If

        End Sub

    End Class

End Namespace
