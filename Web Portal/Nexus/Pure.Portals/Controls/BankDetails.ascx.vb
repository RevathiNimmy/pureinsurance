Imports Nexus.Library
Imports CMS.Library
Imports System.Data
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Controls_BankDetails : Inherits System.Web.UI.UserControl

        Dim oBankDetails As NexusProvider.BankCollection = Nothing
        Dim oParty As NexusProvider.BaseParty = Nothing
        Dim oBank As NexusProvider.Bank = Nothing
        Dim oWebService As NexusProvider.ProviderBase = Nothing
        Dim oPaymentHubEnabled As NexusProvider.OptionTypeSetting = Nothing


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                BindBankData()
                grdBankHistory.DataSource = Nothing
                grdBankHistory.DataBind()
            End If
            If Request("__EVENTARGUMENT") = "UpdateBank" Then

                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sBankData() As String = txtBankDetailData.Value.Split(";")

                'Need to Retreive the Data from Session
                RetreiveData()

                If sBankData(0).ToUpper = "ADD" Then

                    Dim oNewBank As New NexusProvider.Bank

                    With oNewBank
                        .BankPaymentTypeCode = sBankData(1)
                        .AccountHolderName = sBankData(2)
                        .AccountType = sBankData(3)
                        .AccountNumber = sBankData(4)
                        .AccountCode = sBankData(4)
                        .BranchCode = sBankData(5)
                        .BankBranch = sBankData(6)
                        .BIC = sBankData(15)
                        .IBAN = sBankData(16)

                        .BankCode = sBankData(7)
                        .BankName = sBankData(8)


                        .StreetName = sBankData(9)
                        .Locality = sBankData(10)
                        .PostTown = sBankData(11)
                        .County = sBankData(12)
                        .PostCode = sBankData(13)
                        .Country = sBankData(14)
                        .PartyBankAddress.Address1 = sBankData(9)
                        .PartyBankAddress.Address2 = sBankData(10)
                        .PartyBankAddress.Address3 = sBankData(11)
                        .PartyBankAddress.Address4 = sBankData(12)
                        .PartyBankAddress.PostCode = sBankData(13)
                        .PartyBankAddress.CountryCode = sBankData(14)
                        .TaskMode = NexusProvider.Bank.Mode.Add
                        .IsActive = True
                        '.BankKey = sBankData(14)
                    End With

                    oParty.BankDetails.Add(oNewBank)
                    Session(CNParty) = oParty

                ElseIf sBankData(0).ToUpper = "UPDATE" Then
                    Dim oUpdateBankCollection As NexusProvider.BankCollection = oParty.BankDetails
                    Dim oUpdateBanks As NexusProvider.Bank = oParty.BankDetails.Item(CType(sBankData(17), Integer))

                    With oUpdateBanks
                        .BankPaymentTypeCode = sBankData(1)
                        .AccountHolderName = sBankData(2)
                        .AccountType = sBankData(3)
                        .AccountNumber = sBankData(4)
                        .AccountCode = sBankData(4)
                        .BranchCode = sBankData(5)
                        .BankBranch = sBankData(6)
                        .BIC = sBankData(15)
                        .IBAN = sBankData(16)

                        .BankCode = sBankData(7)
                        .BankName = sBankData(8)

                        .StreetName = sBankData(9)
                        .Locality = sBankData(10)
                        .PostTown = sBankData(11)
                        .County = sBankData(12)
                        .PostCode = sBankData(13)
                        .Country = sBankData(14)
                        .PartyBankAddress.Address1 = sBankData(9)
                        .PartyBankAddress.Address2 = sBankData(10)
                        .PartyBankAddress.Address3 = sBankData(11)
                        .PartyBankAddress.Address4 = sBankData(12)
                        .PartyBankAddress.PostCode = sBankData(13)
                        .PartyBankAddress.CountryCode = sBankData(14)
                        If String.IsNullOrEmpty(.PartyBankKey) = False AndAlso .PartyBankKey > 0 Then
                            .TaskMode = NexusProvider.Bank.Mode.Edit
                        Else
                            .TaskMode = NexusProvider.Bank.Mode.Add
                        End If

                        '.BankKey = sBankData(14)
                    End With

                    oUpdateBankCollection.Update(oUpdateBanks)
                    Session(CNParty) = oParty
                End If
                BindBankData()
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

            'hypBank.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            If HttpContext.Current.Session.IsCookieless Then
                hypBank.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypBank.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "/Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If
            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding bank in edit mode.
                BindBankData()
                hypBank.Visible = True
                hypOnlineRegisteredCard.Visible = False
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindBankData()
                hypBank.Visible = False
                oWebService = New NexusProvider.ProviderManager().Provider
                oPaymentHubEnabled = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.SystemOptionPaymentHubEnabled)
                If oPaymentHubEnabled.OptionValue = "1" AndAlso Request.QueryString("PartyKey") IsNot Nothing AndAlso Request.QueryString("Code") IsNot Nothing Then

                    If Request.Url.ToString().ToUpper().Contains("PERSONALCLIENTDETAILS") Then
                        hypOnlineRegisteredCard.Visible = True
                        hypOnlineRegisteredCard.PostBackUrl = "~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "&PartyType=PC"
                    ElseIf Request.Url.ToString().ToUpper().Contains("CORPORATECLIENTDETAILS") Then
                        hypOnlineRegisteredCard.Visible = True
                        hypOnlineRegisteredCard.PostBackUrl = "~/secure/Payment/OnlineCardPayment.aspx?PartyKey=" & Request.QueryString("PartyKey") & "&Code=" & Request.QueryString("Code") & "&PartyType=CC"
                    End If
                Else
                    hypOnlineRegisteredCard.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' This method will bind the Bank collection from the Session - CNParty
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindBankData()

            'Need to Retreive the Data from Session
            RetreiveData()

            Dim otempCollection As New NexusProvider.BankCollection
            Dim oBankDetails As NexusProvider.BankCollection = oParty.BankDetails
            For iCount = 0 To oBankDetails.Count - 1
                'Filling the Collection with the Non-Deleted values.
                'If oBankDetails(iCount).TaskMode <> NexusProvider.Bank.Mode.Delete Then
                otempCollection.Add(oBankDetails(iCount))
                'End If
            Next

            grdBankDetails.DataSource = otempCollection
            grdBankDetails.DataBind()

        End Sub

        ''' <summary>
        ''' Retreive the Party data from Session
        ''' </summary>
        ''' <remarks></remarks>
        Sub RetreiveData()
            'Need to Retreive the Data from Session
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                    Case Else
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub

        Sub BindHistory(ByVal iSelectedBankId As Integer)

            'Need to Retreive the Data from Session
            RetreiveData()

            Dim oBankCollection As NexusProvider.BankCollection = oParty.BankDetails
            Dim oBankDetails As NexusProvider.Bank = oParty.BankDetails.Item(CType(iSelectedBankId, Integer))

            'Bind the History grid for selected bank only
            grdBankHistory.DataSource = oBankDetails.History
            grdBankHistory.DataBind()

            'cleaning up
            oBankCollection = Nothing
            oBankDetails = Nothing

        End Sub

        Protected Sub grdBankDetails_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdBankDetails.DataBound
            If grdBankDetails.Rows.Count = 0 Or grdBankDetails.PageCount = 1 Then
                grdBankDetails.AllowPaging = False
            End If
        End Sub

        Protected Sub grdBankDetails_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdBankDetails.PageIndexChanging
            RetreiveData()
            If oParty.BankDetails IsNot Nothing Then
                grdBankDetails.PageIndex = e.NewPageIndex
                grdBankDetails.DataSource = oParty.BankDetails
                grdBankDetails.DataBind()
            End If
        End Sub

        Protected Sub grdBankDetails_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdBankDetails.RowCommand
            Dim iCount As Integer
            Dim oBankDetails As NexusProvider.BankCollection

            'Need to Retreive the Data from Session
            RetreiveData()
            oBankDetails = oParty.BankDetails

            Select Case e.CommandName
                Case "Select"
                    'user has opted the 'Select' option to view the Bank History
                    BindHistory(e.CommandArgument.ToString.Trim)
                    grdBankHistory.Focus()

                Case "DeleteRow"
                    Dim oBaseParty As NexusProvider.BaseParty = Session(CNParty)
                    'New Client
                    If oBaseParty Is Nothing Or (oBaseParty IsNot Nothing AndAlso oBaseParty.Key = 0) Then

                        For iCount = 0 To oBankDetails.Count - 1
                            If oBankDetails(iCount).Key = CInt(e.CommandArgument) Then
                                oParty.BankDetails.Remove(iCount)
                                Exit For
                            End If
                        Next
                    ElseIf oBaseParty IsNot Nothing AndAlso oBaseParty.Key <> 0 Then
                        'Update Client
                        For iCount = 0 To oBankDetails.Count - 1
                            If oBankDetails(iCount).Key = CInt(e.CommandArgument) Then
                                If oBankDetails(iCount).PartyBankKey = 0 Then
                                    oParty.BankDetails.Remove(iCount)
                                Else
                                    oParty.BankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.Delete
                                End If
                                Exit For
                            End If
                        Next
                    End If

                    Session(CNParty) = oParty
                    BindBankData()
                Case "Activate"
                    For iCount = 0 To oBankDetails.Count - 1
                        If oBankDetails(iCount).Key = CInt(e.CommandArgument) Then
                            oParty.BankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.MakeActive
                            oParty.BankDetails(iCount).IsActive = True
                            Exit For
                        End If
                    Next
                    Session(CNParty) = oParty
                    BindBankData()
                Case "Deactivate"
                    For iCount = 0 To oBankDetails.Count - 1
                        If oBankDetails(iCount).Key = CInt(e.CommandArgument) Then
                            oParty.BankDetails(iCount).TaskMode = NexusProvider.Bank.Mode.MakeInactive
                            oParty.BankDetails(iCount).IsActive = False
                            Exit For
                        End If
                    Next
                    Session(CNParty) = oParty
                    BindBankData()
            End Select
        End Sub

        '''' <summary>
        '''' Contact DataBound event for corporate / personal client
        '''' </summary>
        '''' <param name="sender"></param>
        '''' <param name="e"></param>
        '''' <remarks></remarks>
        Protected Sub grdBankDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdBankDetails.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow AndAlso CType(e.Row.DataItem, NexusProvider.Bank).TaskMode <> NexusProvider.Bank.Mode.Delete Then
                Dim hypEdit As LinkButton = CType(e.Row.FindControl("btnEdit"), LinkButton)
                Dim hypDelete As LinkButton = CType(e.Row.FindControl("btnBankDelete"), LinkButton)
                Dim hypActivate As LinkButton = CType(e.Row.FindControl("btnBankActivate"), LinkButton)
                Dim hypSelect As LinkButton = CType(e.Row.FindControl("btnSelect"), LinkButton)
                If CType(e.Row.DataItem, NexusProvider.Bank).IsBankItem Then
                    If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then

                        'Finding the Edit link in the gridview to assign the navigate URL along with BankID. 

                        Dim BankKey As Integer = 0
                        For BankKey = 0 To oParty.BankDetails.Count - 1
                            If oParty.BankDetails(BankKey).PartyBankKey = CType(e.Row.DataItem, NexusProvider.Bank).PartyBankKey Then
                                If CType(e.Row.DataItem, NexusProvider.Bank).IsPartyBankLinkedWithInst Then
                                    If HttpContext.Current.Session.IsCookieless Then
                                        hypEdit.OnClientClick = "if( confirm('" & GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                                    Else
                                        hypEdit.OnClientClick = "if( confirm('" & GetLocalResourceObject("lbl_EditConfirmMsg").ToString() & "') == 1) {" & "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;}else return false;"
                                    End If
                                Else
                                    If HttpContext.Current.Session.IsCookieless Then
                                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                                    Else
                                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/BankDetail.aspx?PostbackTo=" & UpdBankDetails.ClientID.ToString & "&BankKey=" & BankKey & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                                    End If
                                End If
                                Exit For
                            End If
                        Next
                        'Finding the Delete link in the gridview to assign the navigate URL along with BankID. 
                        hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Bank).Key
                        hypActivate.CommandArgument = CType(e.Row.DataItem, NexusProvider.Bank).Key
                        'NOTE - this will need to be changed to give each row a unique id
                        'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                        e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Bank).PartyBankKey)

                        hypDelete.Attributes.Add("onclick", "return confirm('" & GetLocalResourceObject("lbl_DeleteConfirmMsg").ToString() & "');")

                        If CType(e.Row.DataItem, NexusProvider.Bank).IsActive Then
                            hypActivate.CommandName = "Deactivate"
                            hypActivate.Text = GetLocalResourceObject("lkbtn_Deactivate").ToString()
                            hypActivate.Attributes.Add("onclick", "return confirm('" & GetLocalResourceObject("lbl_DeactivateConfirmMsg").ToString() & "');")
                            hypDelete.Visible = True
                            hypEdit.Visible = True
                        Else
                            hypActivate.CommandName = "Activate"
                            hypActivate.Text = GetLocalResourceObject("lkbtn_Activate").ToString()
                            hypActivate.Attributes.Add("onclick", "return confirm('" & GetLocalResourceObject("lbl_ActivateConfirmMsg").ToString() & "');")
                            hypDelete.Visible = False
                            hypEdit.Visible = False
                        End If
                        If CType(e.Row.DataItem, NexusProvider.Bank).PartyBankKey = 0 Then
                            hypActivate.Visible = False
                            hypDelete.Visible = True
                        Else
                            hypActivate.Visible = True
                        End If
                        If CType(e.Row.DataItem, NexusProvider.Bank).IsPartyBankInUse Then
                            hypDelete.Visible = False
                        Else
                            hypDelete.Visible = True
                        End If
                    ElseIf Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                        'Hiding the Edit and Delete link in View Mode
                        hypDelete.Visible = False
                        hypEdit.Visible = False
                        hypActivate.Visible = False

                    End If
                Else
                    hypDelete.Visible = False
                    hypEdit.Visible = False
                    hypActivate.Visible = False
                    hypSelect.Visible = False
                End If

            ElseIf e.Row.RowType = DataControlRowType.DataRow Then
                e.Row.Visible = False
            End If
        End Sub

        Protected Sub grdBankDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles grdBankDetails.RowEditing
            'This Event need blank body in order to "Edit" the Bank Details during MTA Client Edit
        End Sub

        Protected Sub grdBankDetails_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdBankDetails.Sorting
            Dim oBankDetails As NexusProvider.BankCollection
            'Need to Retreive the Data from Session
            RetreiveData()
            oBankDetails = oParty.BankDetails
            oBankDetails.SortColumn = e.SortExpression
            'check that the sort expression is the same as stored in viewstate as we should start again if reordering by a new column
            Dim _sortDirection As New SortDirection
            If ViewState("SortDirection") = SortDirection.Ascending And ViewState("SortExpression") = e.SortExpression Then
                _sortDirection = SortDirection.Descending
            Else
                _sortDirection = SortDirection.Ascending
            End If
            'store the current sortdirection for comparison on the next sort
            ViewState("SortDirection") = _sortDirection
            'store the SortExpression in viewstate so that we can check if we are sorting by a new column on the next sort
            ViewState("SortExpression") = e.SortExpression
            oBankDetails.SortingOrder = _sortDirection
            oBankDetails.Sort()
            CType(sender, GridView).DataSource = oBankDetails
            CType(sender, GridView).DataBind()
        End Sub
    End Class

End Namespace
