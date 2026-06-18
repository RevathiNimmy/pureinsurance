Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_PolicyAssociates
        Inherits System.Web.UI.UserControl
        Dim oParty As NexusProvider.BaseParty = Nothing
        Public Property IsViewOnly() As Boolean
        ''' <summary>
        ''' binds the Associate collection from the Session.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindAssociateData()
            Dim nInsurancefileKey As Integer = 0
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oSecondaryAssociateData As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
            Dim oSecondaryAssociateFilterData As NexusProvider.PolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection

            If oQuote Is Nothing Then
                Dim oClaim As NexusProvider.Claim = CType(Session(CNClaim), NexusProvider.Claim)
                If oClaim IsNot Nothing Then
                    nInsurancefileKey = oClaim.InsuranceFileKey
                    oQuote = oWebService.GetHeaderAndSummariesByKey(nInsurancefileKey)
                End If
            End If
            If Session(CNMode) IsNot Nothing AndAlso Session(CNMode) = Mode.View Then
                IsViewOnly = True
            End If
            If oQuote IsNot Nothing Then
                oSecondaryAssociateData = oWebService.GetPolicyAssociates(oQuote.InsuranceFileKey, oQuote.InsuranceFolderKey, Nothing)
                For Each oAssociate As NexusProvider.PolicyAssociate In oSecondaryAssociateData
                    If Not oAssociate.IsDeleted Then
                        oSecondaryAssociateFilterData.Add(oAssociate)
                    End If
                Next
                Session(CNPolicyAssociateCollection) = oSecondaryAssociateFilterData
                grdAssociate.DataSource = oSecondaryAssociateFilterData
                grdAssociate.DataBind()

                Session(CNPolicyAssociateCollection) = oSecondaryAssociateFilterData
            End If

            btnAddAssociate.Visible = Not IsViewOnly

        End Sub

        Protected Sub grdAssociate_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdAssociate.DataBound
            If CType(sender, GridView).Rows.Count = 0 Or CType(sender, GridView).PageCount = 1 Then
                CType(sender, GridView).AllowPaging = False
            End If
        End Sub

        Protected Sub grdAssociate_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdAssociate.Load
            If grdAssociate.PageCount = 1 Then
                grdAssociate.AllowPaging = False
            End If
        End Sub

        ''' <summary>
        ''' Associate DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub grdAssociate_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdAssociate.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                'If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                'Finding the Edit link in the gridview to assign the navigate URL along with AssociateID. 
                'Dim CommandCell As tem
                Dim hypEdit As LinkButton = e.Row.Cells(9).FindControl("hypAssociateEdit")
                Dim hypView As LinkButton = e.Row.Cells(9).FindControl("hypAssociateView")
                Dim hypDelete As LinkButton = e.Row.Cells(9).FindControl("hypAssociateDelete")

                If CDate(e.Row.Cells(8).Text) <= Date.MinValue Then
                    e.Row.Cells(8).Text = ""
                End If

                If (e.Row.Cells(8).Text <> "" OrElse Session(CNMTAType) = MTAType.CANCELLATION OrElse Session(CNMode) = Mode.View) Then
                    hypEdit.Visible = False
                    hypDelete.Visible = False
                End If

                If (Session(CNMTAType) = MTAType.CANCELLATION OrElse Session(CNMode) = Mode.View) Then
                    btnAddAssociate.Visible = False
                End If


                If e.Row.Cells(5).Text.ToString <> String.Empty Then
                    e.Row.Cells(5).Text = GetDescriptionForCode(NexusProvider.ListType.PMLookup, GetCodeForKey(NexusProvider.ListType.PMLookup, e.Row.Cells(5).Text.ToString, "Association_Type", True), "Association_Type")
                End If

                Dim nPolicyAssociateRowKey As Integer = grdAssociate.DataKeys(e.Row.RowIndex).Values("InsuranceFileAssociatesKey").ToString()
                If HttpContext.Current.Session.IsCookieless Then
                    hypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PolicyAssociate.aspx?RowKey=" & nPolicyAssociateRowKey & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
                    hypView.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/PolicyAssociate.aspx?RowKey=" & nPolicyAssociateRowKey & "&displaymode=view&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"

                Else
                    hypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "/Modal/PolicyAssociate.aspx?RowKey=" & nPolicyAssociateRowKey & "&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"
                    hypView.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "/Modal/PolicyAssociate.aspx?RowKey=" & nPolicyAssociateRowKey & "&displaymode=view&modal=true&KeepThis=true&TB_iframe=true&height=600&width=750' , null);return false;"

                End If


                hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.PolicyAssociate).RowKey
                hypDelete.Attributes.Add("onclick", "return ConfirmOnDelete()")

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PolicyAssociate).RowKey)
            End If
        End Sub

        Protected Sub grdAssociate_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAssociate.RowCommand

            If e.CommandName = "DeleteRow" Then
                Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = Session(CNPolicyAssociateCollection)
                Dim oSecondaryPolicyAssociate As NexusProvider.PolicyAssociate = New NexusProvider.PolicyAssociate
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim oQuote As NexusProvider.Quote = Session(CNQuote)
                For Each oAssociate As NexusProvider.PolicyAssociate In oPolicyAssociateCollection
                    If oAssociate.RowKey = e.CommandArgument.ToString.Trim Then
                        With oSecondaryPolicyAssociate
                            .InsuranceFileKey = oAssociate.InsuranceFileKey
                            .InsuranceFileAssociatesKey = oAssociate.InsuranceFileAssociatesKey
                            .InsuranceFolderCnt = oAssociate.InsuranceFolderCnt
                            .IsDeleted = True
                            .PartyKey = oAssociate.PartyKey
                            .RowKey = oAssociate.RowKey
                            .AssociationDetail = oAssociate.AssociationDetail
                            .AssociationTypeKey = oAssociate.AssociationTypeKey
                            .DateAttached = oAssociate.DateAttached
                            .DateRemoved = FormatDateTime(oQuote.CoverStartDate, DateFormat.GeneralDate)
                            .ActionType = NexusProvider.PolicyAssociateActionType.DeleteRow
                            .DelUnConfirmed = True
                            .AddUnConfirmed = oAssociate.AddUnConfirmed
                        End With
                        oPolicyAssociateCollection = New NexusProvider.PolicyAssociateCollection
                        oPolicyAssociateCollection.Add(oSecondaryPolicyAssociate)
                        Exit For
                    End If
                Next
                Dim oPolicyAssociate As NexusProvider.PolicyAssociate = New NexusProvider.PolicyAssociate
                oPolicyAssociate = oWebService.UpdatePolicyAssociates(oPolicyAssociateCollection, oQuote.TimeStamp, Nothing)
                oQuote.TimeStamp = oPolicyAssociate.TimeStamp
                Session(CNQuote) = oQuote

                BindAssociateData()
                Dim PostBackStr As String = "self." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
            End If
        End Sub
        ''' <summary>
        ''' Retreive the Party data from Session
        ''' </summary>
        ''' <remarks></remarks>
        Sub RetreiveData(ByRef sClientcode As String)
            'Need to Retreive the Data from Session
            If Session(CNParty) IsNot Nothing Then
                Select Case True
                    Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                        oParty = CType(Session(CNParty), NexusProvider.CorporateParty)

                        If Not oParty Is Nothing AndAlso Not CType(oParty, NexusProvider.CorporateParty).ClientSharedData Is Nothing Then
                            'Master Client Code need to fetch from ClientSharedData  as advised by Krishna
                            sClientcode = CType(oParty, NexusProvider.CorporateParty).ClientSharedData.ShortName.Trim()
                        End If

                    Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                        oParty = CType(Session(CNParty), NexusProvider.PersonalParty)

                        If Not oParty Is Nothing AndAlso Not CType(oParty, NexusProvider.PersonalParty).ClientSharedData Is Nothing Then
                            'Master Client Code need to fetch from ClientSharedData as advised by Krishna
                            sClientcode = CType(oParty, NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim()
                        End If

                End Select
            End If
        End Sub
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            BindAssociateData()
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            Dim sClientCode As String = String.Empty
            RetreiveData(sClientCode)

            If oParty IsNot Nothing AndAlso sClientCode <> "" Then
                sClientCode = sClientCode
            ElseIf Session(CNIsAnonymous) = True Then
                sClientCode = Session(CNAnonymous).ToString
            End If

            If ((sClientCode Is Nothing) AndAlso (DirectCast(DirectCast(DirectCast(Session(CNQuote), System.Object), NexusProvider.Quote), NexusProvider.Quote).ClientCode IsNot Nothing)) Then
                sClientCode = DirectCast(DirectCast(DirectCast(Session(CNQuote), System.Object), NexusProvider.Quote), NexusProvider.Quote).ClientCode
            End If
            If sClientCode IsNot Nothing Then
                If HttpContext.Current.Session.IsCookieless Then
                    btnAddAssociate.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/FindClient.aspx?SecondaryAssociate=true&ClientCode=" & sClientCode.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                Else
                    btnAddAssociate.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "/Modal/FindClient.aspx?SecondaryAssociate=true&ClientCode=" & sClientCode.ToString & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                End If
            End If
        End Sub

        Protected Sub grdAssociate_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdAssociate.PageIndexChanging
            Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = Session(CNPolicyAssociateCollection)
            grdAssociate.DataSource = oPolicyAssociateCollection
            grdAssociate.PageIndex = e.NewPageIndex
            grdAssociate.DataBind()
        End Sub

        Protected Sub grdAssociate_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles grdAssociate.Sorting
            'sort the Quote & Policy according to the column clicked
            'we need to store the current sort order in viewstate, and reverse it each time
            Dim oPolicyAssociateCollection As NexusProvider.PolicyAssociateCollection = Session(CNPolicyAssociateCollection)
            oPolicyAssociateCollection.SortColumn = e.SortExpression
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
            oPolicyAssociateCollection.SortingOrder = _sortDirection
            oPolicyAssociateCollection.Sort()
            CType(sender, GridView).DataSource = oPolicyAssociateCollection
            CType(sender, GridView).DataBind()
        End Sub
    End Class
End Namespace


