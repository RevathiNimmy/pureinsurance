Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus
    Partial Class Controls_Addresses
        Inherits System.Web.UI.UserControl
        Dim oParty As NexusProvider.BaseParty = Nothing
        ''' <summary>
        ''' Binds the Address collection from the Session.
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub BindAddressData()
            Dim iCount As Integer
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'Need to Retreive the Data from Session
            RetreiveData()

            'Check Country Description
            If oParty IsNot Nothing AndAlso oParty.Addresses IsNot Nothing AndAlso oParty.Addresses.Count > 0 Then
                For iCount = 0 To oParty.Addresses.Count - 1
                    Dim oCountryList As NexusProvider.LookupListCollection
                    oCountryList = oWebService.GetList(NexusProvider.ListType.PMLookup, "Country", True, False)
                    If oParty.Addresses(iCount).CountryDescription Is Nothing Then
                        For jCount As Integer = 0 To oCountryList.Count - 1
                            If oCountryList(jCount).Code.Trim = oParty.Addresses(iCount).CountryCode.Trim Then
                                oParty.Addresses(iCount).CountryDescription = oCountryList(jCount).Description
                            End If
                        Next

                    ElseIf oParty.Addresses(iCount).CountryDescription IsNot Nothing Then
                        If oParty.Addresses(iCount).CountryDescription.Trim.Length = 0 Then
                            For jCount As Integer = 0 To oCountryList.Count - 1
                                If oCountryList(jCount).Code.Trim = oParty.Addresses(iCount).CountryCode.Trim Then
                                    oParty.Addresses(iCount).CountryDescription = oCountryList(jCount).Description
                                End If
                            Next
                        End If
                    End If
                Next
            End If
            drgAddresses.DataSource = oParty.Addresses
            drgAddresses.DataBind()
            'PnlAddress.Update()
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
        ''' <summary>
        ''' Delete address 
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAddresses_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles drgAddresses.RowCommand
            If e.CommandName = "DeleteRow" Then
                'Need to Retreive the Data from Session
                RetreiveData()

                Dim iCount As Integer
                Dim oColl As NexusProvider.AddressCollection = oParty.Addresses
                For iCount = 0 To oColl.Count - 1
                    If oColl(iCount).Key.Trim = e.CommandArgument.ToString.Trim Then
                        oParty.Addresses.Remove(iCount)
                        Exit For
                    End If
                Next

                For iCount = 0 To oColl.Count - 1
                    oParty.Addresses(iCount).Key = iCount.ToString()
                Next

                Session(CNParty) = oParty
                BindAddressData()
            End If
        End Sub

        ''' <summary>
        ''' Address DataBound event for corporate / personal client
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub drgAddresses_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles drgAddresses.RowDataBound
            If e.Row.RowType = DataControlRowType.DataRow Then
                If CType(Session(CNClientMode), Mode) = Mode.Add Or CType(Session(CNClientMode), Mode) = Mode.Edit Then
                    'Finding the Edit link in the gridview to assign the navigate URL along with AddressID. 
                    Dim hypEdit As LinkButton = e.Row.Cells(8).FindControl("hypAddressEdit")
                    hypEdit.Visible = True

                    If HttpContext.Current.Session.IsCookieless Then
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Address.aspx?PostbackTo=" & PnlAddresses.ClientID.ToString.Trim() & "&AddressID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    Else
                        hypEdit.OnClientClick = "tb_show(null ,'" & AppSettings("webroot") & "Modal/Address.aspx?PostbackTo=" & PnlAddresses.ClientID.ToString.Trim() & "&AddressID=" & e.Row.RowIndex & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
                    End If


                    Dim hypDelete As LinkButton = e.Row.Cells(8).FindControl("hypAddressDelete")
                    hypDelete.Visible = True
                    hypDelete.CommandArgument = CType(e.Row.DataItem, NexusProvider.Address).Key

                    'NOTE - this will need to be changed to give each row a unique id
                    'this needs to be matched in markup for the menu (id="Menu_<%# Eval("Key") %>")
                    e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.Address).Key)
                End If
                If CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.CorrespondenceAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_CorrespondenceAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.BillingAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_BillingAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.BranchAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_BranchAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.BusinessAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_BusinessAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.EmailAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_EMailAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.HomeAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_HomeAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.OtherAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_OtherAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.PreviousAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_PreviousAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.RegisteredAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_RegisteredAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.SiteAddress Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_SiteAddress")
                ElseIf CType(e.Row.DataItem, NexusProvider.Address).AddressType = NexusProvider.AddressType.SubAgent Then
                    e.Row.Cells(0).Text = GetLocalResourceObject("lbl_SubAgent")
                End If
            ElseIf e.Row.RowType = DataControlRowType.Header Then
                If Session(CNClientMode) IsNot Nothing AndAlso Session(CNClientMode) = Mode.View Then
                    drgAddresses.Columns(8).Visible = False
                Else
                    drgAddresses.Columns(8).Visible = True

                End If
            End If
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Request("__EVENTARGUMENT") = "UpdateAddress" Then

                'Need to Retreive the Data from Session
                RetreiveData()

                Page.ClientScript.GetPostBackEventReference(Me, "")
                Dim sAddressData() As String = txtAddressData.Value.Split(";")
                If sAddressData(0).ToUpper = "ADD" Then
                    Dim oNewAddress As New NexusProvider.Address
                    Dim sAddress As String = sAddressData(1).ToUpper()

                    Select Case sAddress
                        Case "3131 XBI"
                            oNewAddress.AddressType = NexusProvider.AddressType.BillingAddress
                            Exit Select
                        Case "3131 XBA"
                            oNewAddress.AddressType = NexusProvider.AddressType.BranchAddress
                            Exit Select
                        Case "3131 002"
                            oNewAddress.AddressType = NexusProvider.AddressType.BusinessAddress
                            Exit Select
                        Case "3131 XCO"
                            oNewAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                            Exit Select
                        Case "3131 ECK"
                            oNewAddress.AddressType = NexusProvider.AddressType.EmailAddress
                            Exit Select
                        Case "3131 001"
                            oNewAddress.AddressType = NexusProvider.AddressType.HomeAddress
                            Exit Select
                        Case "3131 0X9"
                            oNewAddress.AddressType = NexusProvider.AddressType.OtherAddress
                            Exit Select
                        Case "3131 XPR"
                            oNewAddress.AddressType = NexusProvider.AddressType.PreviousAddress
                            Exit Select
                        Case "3131 XRE"
                            oNewAddress.AddressType = NexusProvider.AddressType.RegisteredAddress
                            Exit Select
                        Case "3131 XSA"
                            oNewAddress.AddressType = NexusProvider.AddressType.SiteAddress
                            Exit Select
                        Case "3131 0XR"
                            oNewAddress.AddressType = NexusProvider.AddressType.SubAgent
                            Exit Select
                    End Select

                    With oNewAddress
                        .Address1 = sAddressData(2)
                        .Address2 = sAddressData(3)
                        .Address3 = sAddressData(4)
                        .Address4 = sAddressData(5)
                        .StateCode = sAddressData(6)
                        .PostCode = sAddressData(7)
                        'Call SAM to get the Code by passing the Value for binding it in Address object
                        .CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, sAddressData(8), "COUNTRY", True)
                        .CountryDescription = sAddressData(9)

                        .Address5 = sAddressData(11)
                        .Address6 = sAddressData(12)
                        .Address7 = sAddressData(13)
                        .Address8 = sAddressData(14)
                        .Address9 = sAddressData(15)
                        .Address10 = sAddressData(16)

                    End With

                    oParty.Addresses.Add(oNewAddress)

                    Session(CNParty) = oParty
                ElseIf sAddressData(0).ToUpper = "UPDATE" Then

                    Dim oUpdateAddress As NexusProvider.Address = oParty.Addresses.Item(CType(sAddressData(10), Integer))
                    Dim sAddress As String = sAddressData(1).ToUpper()

                    Select Case sAddress
                        Case "3131 XBI"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.BillingAddress
                            Exit Select
                        Case "3131 XBA"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.BranchAddress
                            Exit Select
                        Case "3131 002"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.BusinessAddress
                            Exit Select
                        Case "3131 XCO"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.CorrespondenceAddress
                            Exit Select
                        Case "3131 ECK"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.EmailAddress
                            Exit Select
                        Case "3131 001"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.HomeAddress
                            Exit Select
                        Case "3131 0X9"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.OtherAddress
                            Exit Select
                        Case "3131 XPR"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.PreviousAddress
                            Exit Select
                        Case "3131 XRE"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.RegisteredAddress
                            Exit Select
                        Case "3131 XSA"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.SiteAddress
                            Exit Select
                        Case "3131 0XR"
                            oUpdateAddress.AddressType = NexusProvider.AddressType.SubAgent
                            Exit Select
                    End Select

                    With oUpdateAddress
                        .Address1 = sAddressData(2)
                        .Address2 = sAddressData(3)
                        .Address3 = sAddressData(4)
                        .Address4 = sAddressData(5)
                        .StateCode = sAddressData(6)
                        .PostCode = sAddressData(7)
                        'Call SAM to get the Code by passing the Value for binding it in Address object
                        .CountryCode = GetCodeForKey(NexusProvider.ListType.PMLookup, sAddressData(8), "COUNTRY", True)
                        .CountryDescription = sAddressData(9)
                        .Address5 = sAddressData(11)
                        .Address6 = sAddressData(12)
                        .Address7 = sAddressData(13)
                        .Address8 = sAddressData(14)
                        .Address9 = sAddressData(15)
                        .Address10 = sAddressData(16)
                    End With

                    Dim Addresses As NexusProvider.AddressCollection = oParty.Addresses

                    Addresses.Update(oUpdateAddress)
                    Session(CNParty) = oParty
                End If
                BindAddressData()
            End If

            If Not IsPostBack Then
                BindAddressData()
            End If
        End Sub

        Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
            'Assign nagivate URL along with Client type to differentiate corporate / personal client.
            'Also pass client id of the update panel as this allows for a partial postback
            If HttpContext.Current.Session.IsCookieless Then
                hypAddress.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/Address.aspx?PostbackTo=" & PnlAddresses.ClientID.ToString.Trim() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            Else
                hypAddress.OnClientClick = "tb_show(null ,'" & AppSettings("WebRoot") & "Modal/Address.aspx?PostbackTo=" & PnlAddresses.ClientID.ToString.Trim() & "&modal=true&KeepThis=true&TB_iframe=true&height=500&width=750' , null);return false;"
            End If

            If CType(Session(CNClientMode), Mode) = Mode.Edit Then
                're-binding address in edit mode.
                BindAddressData()
                hypAddress.Visible = True
            ElseIf CType(Session(CNClientMode), Mode) = Mode.View Then
                BindAddressData()
                hypAddress.Visible = False
            End If

        End Sub
    End Class

End Namespace
