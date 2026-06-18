Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Utils
Imports System.Reflection
Imports System.Linq

Namespace Nexus

    Partial Class secure_OtherPartyDetails
        Inherits BaseOtherParty

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            If Request.QueryString("PartyType") IsNot Nothing Then
                Session(CNPartyType) = Request.QueryString("PartyType").Trim()
            End If
        End Sub

        Private Sub Page_Load1(sender As Object, e As EventArgs) Handles Me.Load
            If Session(CNClientMode) = Mode.Edit Then
                txtCode.ReadOnly = True
                ddlBranch.Enabled = True
                ddlSubBranch.Enabled = True

            End If
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
                Dim oParty As NexusProvider.BaseParty = Nothing
                If Session(CNParty) IsNot Nothing Then
                    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                    ddlBranch.SelectedValue = oParty.BranchCode
                Else
                    ddlBranch.SelectedValue = Session(CNBranchCode)
                End If

                FillSubBranches(ddlBranch.SelectedValue)

                If TypeOf Session(CNParty) Is NexusProvider.OtherParty Then
                    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                    With CType(oParty, NexusProvider.OtherParty)
                        Dim strSupply As String = ""
                        Dim strSpeciality As String = ""
                        If .SupplierBusinesses IsNot Nothing AndAlso .SupplierBusinesses.Count > 0 Then
                            For iCnt As Integer = 0 To .SupplierBusinesses.Count - 1
                                If Not strSupply.ToLower.Contains(.SupplierBusinesses(iCnt).BusinessCode.ToLower()) Then
                                    If Not (iCnt = .Branches.Count - 1) Then
                                        strSupply = strSupply + .SupplierBusinesses(iCnt).BusinessCode + ", <br/>"
                                    Else
                                        strSupply = strSupply + .SupplierBusinesses(iCnt).BusinessCode
                                    End If
                                End If
                                If Not String.IsNullOrEmpty(.SupplierBusinesses(iCnt).SpecialityCode) AndAlso
                                    Not strSpeciality.ToLower.Contains(.SupplierBusinesses(iCnt).SpecialityCode.ToLower()) Then
                                    If Not (iCnt = .Branches.Count - 1) Then
                                        strSpeciality = strSpeciality + .SupplierBusinesses(iCnt).SpecialityCode + ", <br/>"
                                    Else
                                        strSpeciality = strSpeciality + .SupplierBusinesses(iCnt).SpecialityCode
                                    End If
                                End If
                            Next
                            lblSupplyPLSelected_View.Text = strSupply
                            lblSpecialityPLSelected_View.Text = strSpeciality
                        Else
                            lblSupplyPLSelected_View.Text = strSupply
                            lblSpecialityPLSelected_View.Text = strSpeciality
                        End If

                        Dim strBranches As String = ""
                        If .Branches IsNot Nothing AndAlso .Branches.Count > 0 Then
                            For iCnt As Integer = 0 To .Branches.Count - 1
                                If Not strBranches.ToLower.Contains(.Branches(iCnt).Description.ToLower()) Then
                                    If Not (iCnt = .Branches.Count - 1) Then
                                        strBranches = strBranches + .Branches(iCnt).Description + ", <br/>"
                                    Else
                                        strBranches = strBranches + .Branches(iCnt).Description
                                    End If
                                End If
                            Next
                            lblBranchPLSelected_View.Text = strBranches
                        Else
                            lblBranchPLSelected_View.Text = strBranches
                        End If
                    End With
                End If

            ElseIf Request("__EVENTTARGET") IsNot Nothing AndAlso Request("__EVENTTARGET").Contains("btnEditClient") Then
                Dim oParty As NexusProvider.BaseParty = Nothing
                If TypeOf Session(CNParty) Is NexusProvider.OtherParty Then
                    oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                    With CType(oParty, NexusProvider.OtherParty)
                        Dim supplyPickListColl As New NexusProvider.PickListCollection
                        Dim specialityPickListColl As New NexusProvider.PickListCollection
                        If .SupplierBusinesses IsNot Nothing AndAlso .SupplierBusinesses.Count > 0 Then
                            For icnt = 0 To .SupplierBusinesses.Count - 1
                                If Not String.IsNullOrEmpty(.SupplierBusinesses.Item(icnt).BusinessCode) Then
                                    supplyPickListColl.Add(New NexusProvider.PickList(.SupplierBusinesses.Item(icnt).BusinessCode, ""))
                                End If
                                If Not String.IsNullOrEmpty(.SupplierBusinesses.Item(icnt).SpecialityCode) Then
                                    specialityPickListColl.Add(New NexusProvider.PickList(.SupplierBusinesses.Item(icnt).SpecialityCode, ""))
                                End If
                            Next
                            SupplyPickList.SetSelectedValues(supplyPickListColl)
                            SpecialityPickList.SetSelectedValues(specialityPickListColl)
                        End If

                        If .Branches IsNot Nothing AndAlso .Branches.Count > 0 Then
                            BranchPickList.SetSelectedValues(.Branches)
                        End If
                    End With
                End If

            End If

            If Not UserCanDoTask("EditOtherParty") Then
                btnEditClient.Visible = False
                btnEditClient.Enabled = False
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

        Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
            If Page.IsValid Then
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

                BtnSubmitClientClick(sender, e)
            End If
        End Sub

        Protected Sub cusvldBranch_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)

            If ddlBranch.SelectedItem.Value.Trim = "" OrElse
                ddlBranch.SelectedItem.Text.Trim.ToUpper = "(NONE)" Then
                args.IsValid = False
            End If

        End Sub

    End Class

End Namespace
