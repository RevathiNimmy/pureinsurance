Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data.SqlTypes
Imports System.Data


Partial Class MTA_AmendClient
    Inherits System.Web.UI.Page

    Dim oGetPartyResponseType As New GetPartyResponseType

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        Mv.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Session("CLIENTTYPE") = "Corporate Client" And Int32.Parse(e.Item.Value) = 4 Then
            Mv.ActiveViewIndex = 1
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create user token from credentials
        'normally the credentials would come from the log in
        If Session("CLIENTTYPE") = "Corporate Client" Then
            Menu1.Items(4).Text = "5 - Address"
        End If

        Try
            If Not (IsPostBack) Then

                Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
                btnAddParty.Visible = False
                'If Not IsPostBack Then
                btnAddParty.Visible = False
                'SAMHelper.setGridView(GVContacts1)
                'SAMHelper.setGridView(gvassociates)
                'SAMHelper.setGridView(gvConcictions)
                'SAMHelper.setGridView(gvDependents)
                'SAMHelper.setGridView(gvloyalty)
                'SAMHelper.setGridView(gvpolicies)

                'set up the proxy object
                Dim oSAM As New SAMForInsuranceV2
                oSAM.SetClientCredential(UserToken)
                oSAM.SetPolicy("SamClientPolicy")

                'create the request and response objects
                Dim oGetPartyRequestType As New GetPartyRequestType
                Dim oGetPartyResponseType As GetPartyResponseType


                'get the quote response
                Dim quote As AddQuoteResponseType = CType(Session("quote"), AddQuoteResponseType)

                'set up request object with some values
                'Session("PartyKey") = 35

                With oGetPartyRequestType
                    .PartyKey = Session("PartyKey") 'CInt(Session("PARTYKEY").ToString)
                    .BranchCode = "Headoff" 'Session("BRANCHCODE").ToString
                End With

                'start vijay For lookups

                If Not IsPostBack Then
                    Dim oRequest As New GetListRequestType
                    Dim oResponse As New GetListResponseType


                    'Saurabh -- Start DataBind to Dropdowns
                    '''Binding Claim Handler Drop down - 

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "131085"
                    oRequest.ExcludeDeletedRecords = True

                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlTitle.DataSource = oResponse.List
                                ddlTitle.DataTextField = "Description"
                                ddlTitle.DataValueField = "Code"
                                ddlTitle.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try
                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Loyalty_scheme"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlLoyaltySchemes.DataSource = oResponse.List
                                ddlLoyaltySchemes.DataTextField = "Description"
                                ddlLoyaltySchemes.DataValueField = "Code"
                                ddlLoyaltySchemes.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try
                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228226"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlPriOccupation.DataSource = oResponse.List
                                ddlPriOccupation.DataTextField = "Description"
                                ddlPriOccupation.DataValueField = "Description"
                                ddlPriOccupation.DataBind()

                                ddlSecOccupation.DataSource = oResponse.List
                                ddlSecOccupation.DataTextField = "Description"
                                ddlSecOccupation.DataValueField = "Description"
                                ddlSecOccupation.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try


                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "TurnoverBand"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlturnover.DataSource = oResponse.List
                                ddlturnover.DataTextField = "Description"
                                ddlturnover.DataValueField = "Code"
                                ddlturnover.DataBind()
                            End If
                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try




                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "risk_group"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlprostype.DataSource = oResponse.List
                                ddlprostype.DataTextField = "Description"
                                ddlprostype.DataValueField = "Code"
                                ddlprostype.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    'Start Girija
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "area"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddlArea.Items.Add(lstitem)
                                ddlArea.AppendDataBoundItems = True
                                ddlArea.DataBind()
                                ddlArea.DataSource = oResponse.List
                                ddlArea.DataTextField = "Description"
                                ddlArea.DataValueField = "Code"
                                ddlArea.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228228"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddlBusiness.Items.Add(lstitem)
                                ddlBusiness.AppendDataBoundItems = True
                                ddlBusiness.DataBind()
                                ddlBusiness.DataSource = oResponse.List
                                ddlBusiness.DataTextField = "Description"
                                ddlBusiness.DataValueField = "Code"
                                ddlBusiness.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228228"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddlTrade.Items.Add(lstitem)
                                ddlTrade.AppendDataBoundItems = True
                                ddlTrade.DataBind()
                                ddlTrade.DataSource = oResponse.List
                                ddlTrade.DataTextField = "Description"
                                ddlTrade.DataValueField = "Code"
                                ddlTrade.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "sic_code"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddlSICcode.Items.Add(lstitem)
                                ddlSICcode.AppendDataBoundItems = True
                                ddlSICcode.DataBind()
                                ddlSICcode.DataSource = oResponse.List
                                ddlSICcode.DataTextField = "Description"
                                ddlSICcode.DataValueField = "Code"
                                ddlSICcode.DataBind()
                            End If
                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "employeeband"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "Not Known"
                                lstitem.Value = 0
                                ddlNoOfEmployees.Items.Add(lstitem)
                                ddlNoOfEmployees.AppendDataBoundItems = True
                                ddlNoOfEmployees.DataBind()
                                ddlNoOfEmployees.DataSource = oResponse.List
                                ddlNoOfEmployees.DataTextField = "Description"
                                ddlNoOfEmployees.DataValueField = "Key"
                                ddlNoOfEmployees.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try
                    'End Girija

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "1114119"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlsentencetype.DataSource = oResponse.List
                                ddlsentencetype.DataTextField = "Description"
                                ddlsentencetype.DataValueField = "Code"
                                ddlsentencetype.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try


                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Contact_type"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlconContact.DataSource = oResponse.List
                                ddlconContact.DataTextField = "Description"
                                ddlconContact.DataValueField = "Code"
                                ddlconContact.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Country"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlconCountry.DataSource = oResponse.List
                                ddlconCountry.DataTextField = "Description"
                                ddlconCountry.DataValueField = "Code"
                                ddlconCountry.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Address_usage_type"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlconType.DataSource = oResponse.List
                                ddlconType.DataTextField = "Description"
                                ddlconType.DataValueField = "Code"
                                ddlconType.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "1114122"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlsentencetime.DataSource = oResponse.List
                                ddlsentencetime.DataTextField = "Description"
                                ddlsentencetime.DataValueField = "Code"
                                ddlsentencetime.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "1114126"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlAlcoholmethod.DataSource = oResponse.List
                                ddlAlcoholmethod.DataTextField = "Description"
                                ddlAlcoholmethod.DataValueField = "Code"
                                ddlAlcoholmethod.DataBind()

                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try


                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Currency"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlCurrency.DataSource = oResponse.List
                                ddlCurrency.DataTextField = "Description"
                                ddlCurrency.DataValueField = "Code"
                                ddlCurrency.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    'oRequest.ListType = STSListType.UserDefinedTable
                    'oRequest.ListCode = "6946819"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "payment_method"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlPaymentMethod.DataSource = oResponse.List
                                ddlPaymentMethod.DataTextField = "Description"
                                ddlPaymentMethod.DataValueField = "Code"
                                ddlPaymentMethod.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try




                    oRequest.BranchCode = "HeadOff"
                    'oRequest.ListType = STSListType.UserDefinedTable
                    'oRequest.ListCode = "6946819"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Relationship_Type"
                    oRequest.ExcludeDeletedRecords = True


                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlrelationshipcode.DataSource = oResponse.List
                                ddlrelationshipcode.DataTextField = "Description"
                                ddlrelationshipcode.DataValueField = "Code"
                                ddlrelationshipcode.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Reminder_Type"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlReminderType.DataSource = oResponse.List
                                ddlReminderType.DataTextField = "Description"
                                ddlReminderType.DataValueField = "Code"
                                ddlReminderType.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try




                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228228"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlPriEmpBusiness.DataSource = oResponse.List
                                ddlPriEmpBusiness.DataTextField = "Description"
                                ddlPriEmpBusiness.DataValueField = "Description"
                                ddlPriEmpBusiness.DataBind()


                                ddlSecEmpsBusiness.DataSource = oResponse.List
                                ddlSecEmpsBusiness.DataTextField = "Description"
                                ddlSecEmpsBusiness.DataValueField = "Description"
                                ddlSecEmpsBusiness.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228230"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddlPriStatus.Items.Add(lstitem)
                                ddlPriStatus.AppendDataBoundItems = True
                                ddlPriStatus.DataBind()
                                ddlPriStatus.DataSource = oResponse.List
                                ddlPriStatus.DataTextField = "Description"
                                ddlPriStatus.DataValueField = "Key"
                                ddlPriStatus.DataBind()


                                Dim lstitem1 As New ListItem
                                lstitem1.Text = "select"
                                lstitem1.Value = 0
                                ddlSecStatus.Items.Add(lstitem1)
                                ddlSecStatus.AppendDataBoundItems = True
                                ddlSecStatus.DataBind()
                                ddlSecStatus.DataSource = oResponse.List
                                ddlSecStatus.DataTextField = "Description"
                                ddlSecStatus.DataValueField = "Key"
                                ddlSecStatus.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try




                    'oRequest.BranchCode = "HeadOff"
                    'oRequest.ListType = STSListType.UserDefinedTable
                    'oRequest.ListCode = "29306889"
                    'Try
                    '    oResponse = oSAM.GetList(oRequest)

                    '    With oResponse
                    '        If Not (.Errors) Is Nothing Then
                    '            'errors returned, so throw an exception
                    '            Throw New SamResponseException(.Errors)
                    '        Else
                    '            ddltermofpayment.DataSource = oResponse.List
                    '            ddltermofpayment.DataTextField = "Description"
                    '            ddltermofpayment.DataValueField = "Code"
                    '            ddltermofpayment.DataBind()
                    '        End If


                    '    End With

                    'Catch os As SamResponseException
                    '    'should do some error handling here. Just output error for now
                    '    Response.Write("An error occured calling SAM:<br>" & os.Message)

                    'Catch oe As Exception
                    '    'should do some error handling here. Just output error for now
                    '    Response.Write("An error occured:<br>" & oe.Message)

                    'Finally
                    '    'clean up any objects here
                    'End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Renewal_stop_code"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlrenewaldtopcode.DataSource = oResponse.List
                                ddlrenewaldtopcode.DataTextField = "Description"
                                ddlrenewaldtopcode.DataValueField = "Code"
                                ddlrenewaldtopcode.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Prospect_Status"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlprosStatus.DataSource = oResponse.List
                                ddlprosStatus.DataTextField = "Description"
                                ddlprosStatus.DataValueField = "Code"
                                ddlprosStatus.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Strength_Code"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlprosStrengthCode.DataSource = oResponse.List
                                ddlprosStrengthCode.DataTextField = "Description"
                                ddlprosStrengthCode.DataValueField = "Code"
                                ddlprosStrengthCode.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Seasonal_gift"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddseasonalgift.DataSource = oResponse.List
                                ddseasonalgift.DataTextField = "Description"
                                ddseasonalgift.DataValueField = "Code"
                                ddseasonalgift.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try
                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Nationality"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddnationality.DataSource = oResponse.List
                                ddnationality.DataTextField = "Description"
                                ddnationality.DataValueField = "Code"
                                ddnationality.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "131091"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddgender.DataSource = oResponse.List
                                ddgender.DataTextField = "Description"
                                ddgender.DataValueField = "Code"
                                ddgender.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try
                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "131107"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else

                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddmaritalstatus.Items.Add(lstitem)
                                ddmaritalstatus.AppendDataBoundItems = True
                                ddmaritalstatus.DataBind()
                                ddmaritalstatus.DataSource = oResponse.List
                                ddmaritalstatus.DataTextField = "Description"
                                ddmaritalstatus.DataValueField = "Code"
                                ddmaritalstatus.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    'oRequest.BranchCode = "HeadOff"
                    'oRequest.ListType = STSListType.GisList
                    'oRequest.ListCode = "2228226"
                    'Try
                    '    oResponse = oSAM.GetList(oRequest)

                    '    With oResponse
                    '        If Not (.Errors) Is Nothing Then
                    '            'errors returned, so throw an exception
                    '            Throw New SamResponseException(.Errors)
                    '        Else
                    '            ddaccomidation.DataSource = oResponse.List
                    '            ddaccomidation.DataTextField = "Description"
                    '            ddaccomidation.DataValueField = "Code"
                    '            ddaccomidation.DataBind()
                    '        End If


                    '    End With

                    'Catch os As SamResponseException
                    '    'should do some error handling here. Just output error for now
                    '    Response.Write("An error occured calling SAM:<br>" & os.Message)

                    'Catch oe As Exception
                    '    'should do some error handling here. Just output error for now
                    '    Response.Write("An error occured:<br>" & oe.Message)

                    'Finally
                    '    'clean up any objects here
                    'End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "1114113"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlconvictiontype.DataSource = oResponse.List
                                ddlconvictiontype.DataTextField = "Description"
                                ddlconvictiontype.DataValueField = "Code"
                                ddlconvictiontype.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "1114124"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlconvictionstatus.DataSource = oResponse.List
                                ddlconvictionstatus.DataTextField = "Description"
                                ddlconvictionstatus.DataValueField = "Code"
                                ddlconvictionstatus.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Lifestyle_Category"
                    oRequest.ExcludeDeletedRecords = True

                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlcategory.DataSource = oResponse.List
                                ddlcategory.DataTextField = "Description"
                                ddlcategory.DataValueField = "Code"
                                ddlcategory.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228226"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddloccupationcode.DataSource = oResponse.List
                                ddloccupationcode.DataTextField = "Description"
                                ddloccupationcode.DataValueField = "Description"
                                ddloccupationcode.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try


                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.UserDefinedTable
                    oRequest.ListCode = "2228226"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddlsecoccupationcode.DataSource = oResponse.List
                                ddlsecoccupationcode.DataTextField = "Description"
                                ddlsecoccupationcode.DataValueField = "Description"
                                ddlsecoccupationcode.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try




                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Source"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddBranch.DataSource = oResponse.List
                                ddBranch.DataTextField = "Description"
                                ddBranch.DataValueField = "Code"
                                ddBranch.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "Sub_Branch"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                ddSubBranch.DataSource = oResponse.List
                                ddSubBranch.DataTextField = "Description"
                                ddSubBranch.DataValueField = "Code"
                                ddSubBranch.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try
                    oRequest.BranchCode = "HeadOff"
                    oRequest.ListType = STSListType.PMLookup
                    oRequest.ListCode = "service_level"
                    oRequest.ExcludeDeletedRecords = True
                    Try
                        oResponse = oSAM.GetList(oRequest)

                        With oResponse
                            If Not (.Errors) Is Nothing Then
                                'errors returned, so throw an exception
                                Throw New SamResponseException(.Errors)
                            Else
                                Dim lstitem As New ListItem
                                lstitem.Text = "select"
                                lstitem.Value = 0
                                ddServiceLevel.Items.Add(lstitem)
                                ddServiceLevel.AppendDataBoundItems = True
                                ddServiceLevel.DataBind()
                                ddServiceLevel.DataSource = oResponse.List
                                ddServiceLevel.DataTextField = "Description"
                                ddServiceLevel.DataValueField = "Code"
                                ddServiceLevel.DataBind()
                            End If


                        End With

                    Catch os As SamResponseException
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured calling SAM:<br>" & os.Message)

                    Catch oe As Exception
                        'should do some error handling here. Just output error for now
                        Response.Write("An error occured:<br>" & oe.Message)

                    Finally
                        'clean up any objects here
                    End Try

                    'End vijay lookups

                    BuildLists(oSAM, ddlPreferedCorres, STSListType.PMLookup, "Contact_Type", "")
                    BuildLists(oSAM, ddlSeasonalGift, STSListType.PMLookup, "Seasonal_gift", "")
                    BuildLists(oSAM, ddlAdditionsArea, STSListType.PMLookup, "area", "")
                    loadDefaultValueInCombo()

                    oGetPartyResponseType = oSAM.GetParty(oGetPartyRequestType)

                    With oGetPartyResponseType
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        End If
                    End With
                    'add to session
                    Session("GetPartyResponse") = oGetPartyResponseType

                    'For Personal Client
                    If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                        oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                        Dim objBaseparty As New BasePartyPCType
                        objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                        'Binding the mandatory fields with the relevant output parameters
                        With objBaseparty
                            txtClientCode.Text = .Surname & "" & .Initials
                            txtLastName.Text = .Surname
                            txtForeName.Text = .Forename
                            ddlTitle.ClearSelection()
                            ddlTitle.Items.FindByText(.Title).Selected = True
                            txtInitial.Text = .Initials
                            txtTradingName.Text = .TradingName
                        End With
                        txtAccbalance.Text = objBaseparty.ClientDetail.AccountBalance
                        txtLastYearturnOver.Text = objBaseparty.ClientDetail.LastYearTurnover
                        txtYearTodateTurnOver.Text = objBaseparty.ClientDetail.YearToDateTurnover
                        chkIsprospect.Checked = objBaseparty.ClientDetail.IsProspect
                        chkIsagent.Checked = objBaseparty.ClientDetail.IsAgent

                        txtAlternativeIdentifier.Text = objBaseparty.AlternativeId
                        If objBaseparty.ClientDetail.ServiceLevelCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ServiceLevelCode <> "" Then
                            ddServiceLevel.Items.FindByValue(objBaseparty.ClientDetail.ServiceLevelCode).Selected = True
                        End If
                        If objBaseparty.BranchCode IsNot Nothing AndAlso objBaseparty.BranchCode <> "" Then
                            ddBranch.Items.FindByValue(objBaseparty.BranchCode).Selected = True
                        End If
                        If objBaseparty.SubBranchCode IsNot Nothing AndAlso objBaseparty.SubBranchCode <> "" Then
                            ddSubBranch.Items.FindByValue(objBaseparty.SubBranchCode).Selected = True
                        End If

                        txtLeadAgentCode.Text = objBaseparty.ClientDetail.LeadAgentCode
                        txtLeadAgentName.Text = objBaseparty.ClientDetail.LeadAgentName
                        If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                            hdLeadAgent.Value = objBaseparty.ClientDetail.LeadAgentKey
                        End If

                        If objBaseparty.ClientDetail.AreaCode IsNot Nothing AndAlso objBaseparty.ClientDetail.AreaCode <> "" Then
                            ddlArea.Items.FindByValue(objBaseparty.ClientDetail.AreaCode).Selected = True
                        End If
                        txtFileCode.Text = objBaseparty.FileCode

                        txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode
                        hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode

                        txtAccExecutiveName.Text = objBaseparty.AccountExecutive
                        '''''''''''''''''Address Tab'''''''''''''''''''''''
                        GVContacts.Visible = True
                        GVContacts.DataSource = objBaseparty.Addresses
                        GVContacts.DataBind()

                        'Dim dsContacts As New DataSet
                        Dim dtContacts As New DataTable
                        Dim dcAreaCode As New DataColumn("AreaCode")
                        Dim dcNumber As New DataColumn("Number")
                        'Dim dcExtension As New DataColumn("Extension")
                        Dim dcType As New DataColumn("Type")
                        Dim dcDescription As New DataColumn("Description")
                        Dim drContacts As DataRow

                        dtContacts.Columns.Add(dcAreaCode)
                        dtContacts.Columns.Add(dcNumber)
                        'dtContacts.Columns.Add(dcExtension)
                        dtContacts.Columns.Add(dcType)
                        dtContacts.Columns.Add(dcDescription)

                        Dim oContactType As BaseContactType
                        If (objBaseparty.Contacts IsNot Nothing) Then
                            For Each oContactType In objBaseparty.Contacts
                                drContacts = dtContacts.NewRow()
                                drContacts("AreaCode") = oContactType.AreaCode
                                drContacts("Number") = oContactType.ContactDetail.Item
                                'drContacts("Extension") = ""
                                drContacts("Type") = oContactType.ContactTypeCode
                                drContacts("Description") = oContactType.Description
                                dtContacts.Rows.Add(drContacts)
                            Next
                        End If
                        GVContacts1.DataSource = dtContacts
                        GVContacts1.DataBind()

                        txtsalutation.Text = objBaseparty.Salutation
                        ddlPreferedCorres.SelectedValue = objBaseparty.ClientDetail.CorrespondenceCode
                        chkTPS.Checked = objBaseparty.TPS
                        chkMPS.Checked = objBaseparty.MPS
                        chkeMPS.Checked = objBaseparty.eMPS
                        '''''''''''''''''''''''''''Additions Tab'''''''''''''''''''''''''''''''
                        If Not objBaseparty.Currency Is Nothing Then
                            ddlCurrency.Items.FindByValue(objBaseparty.Currency).Selected = True
                        End If
                        If objBaseparty.ClientDetail.PaymentCode IsNot Nothing AndAlso objBaseparty.ClientDetail.PaymentCode <> "" Then
                            ddlPaymentMethod.Items.FindByText(objBaseparty.ClientDetail.PaymentCode).Selected = True
                        End If
                        If objBaseparty.ClientDetail.ReminderCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ReminderCode <> "" Then
                            ddlReminderType.Items.FindByValue(objBaseparty.ClientDetail.ReminderCode).Selected = True
                        End If
                        If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.PaymentTermCode) Then
                            ddlTermsOfPayment.Items.FindByValue(objBaseparty.ClientDetail.PaymentTermCode).Selected = True
                        End If

                        If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.RenewalStopCode) Then
                            ddlrenewaldtopcode.Items.FindByValue(objBaseparty.ClientDetail.RenewalStopCode).Selected = True
                        End If

                        txtsource.Text = objBaseparty.Source
                        If objBaseparty.OccupationCode IsNot Nothing AndAlso objBaseparty.OccupationCode <> "" Then
                            ddlPriOccupation.Items.FindByText(objBaseparty.OccupationCode).Selected = True
                        End If
                        If objBaseparty.EmployersBusinessCode IsNot Nothing AndAlso objBaseparty.EmployersBusinessCode <> "" Then
                            ddlPriEmpBusiness.Items.FindByText(objBaseparty.EmployersBusinessCode).Selected = True
                        End If

                        'If objBaseparty.EmploymentStatusCode.GetType IsNot Nothing Then
                        'ddlPriStatus.Items.FindByValue(objBaseparty.EmploymentStatusCode).Selected = True
                        If (objBaseparty.EmploymentStatusCodeSpecified) Then
                            ddlPriStatus.SelectedIndex = objBaseparty.EmploymentStatusCode + 1
                        Else
                            ddlPriStatus.ClearSelection()
                            ddlPriStatus.Items(0).Selected = True
                        End If

                        If objBaseparty.SecOccupationCode IsNot Nothing AndAlso objBaseparty.SecOccupationCode <> "" Then
                            ddlSecOccupation.Items.FindByText(objBaseparty.SecOccupationCode).Selected = True
                        End If

                        If objBaseparty.SecEmployersBusinessCode IsNot Nothing AndAlso objBaseparty.SecEmployersBusinessCode <> "" Then
                            ddlSecEmpsBusiness.Items.FindByText(objBaseparty.SecEmployersBusinessCode).Selected = True
                        End If
                        If objBaseparty.SecEmploymentStatusCodeSpecified Then
                            ' ddlSecStatus.Items.FindByValue(objBaseparty.SecEmploymentStatusCode).Selected = True
                            ddlSecStatus.ClearSelection()
                            ddlSecStatus.SelectedIndex = objBaseparty.SecEmploymentStatusCode + 1
                        Else
                            ddlSecStatus.Items(0).Selected = True
                        End If

                        'txttermspay.Text = objBaseparty.ClientDetail.PaymentTermCode
                        'txtrenewal.Text = objBaseparty.ClientDetail.RenewalStopCode
                        'txtoccupation.Text = objBaseparty.OccupationCode
                        'txtEmpbusiness.Text = objBaseparty.EmployersBusinessCode
                        'txtstatus.Text = objBaseparty.EmploymentStatusCode
                        ' txtsecoccupation.Text = objBaseparty.SecOccupationCode
                        ' txtsecEmpbusiness.Text = objBaseparty.SecEmployersBusinessCode
                        'txtsecstatus.Text = objBaseparty.SecEmploymentStatusCode
                        'vijay

                        gvassociates.Visible = True
                        gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                        gvassociates.DataBind()
                        txtFileCode.Visible = True
                        ddlArea.Visible = True

                        'txtwage.Enabled = False
                        'txtturnover.Enabled = False
                        'ddlturnover.Enabled = False
                        'txtfinancial.Enabled = False
                        'txtIsTps.Text = objBaseparty.TPS
                        'txtIsMps.Text = objBaseparty.MPS
                        'txtIseMps.Text = objBaseparty.eMPS
                        ' txtIsprospect.Text = objBaseparty.ClientDetail.IsProspect

                        'VijayakumarStart
                        'lblArea.Visible = True
                        'lblFileCode.Visible = True
                        'lblBusiness.Visible = False
                        'ddlbusiness.Visible = False
                        'lblTrade.Visible = False
                        'ddlTrade.Visible = False
                        'lblSICcode.Visible = False
                        'ddlSICcode.Visible = False
                        'lblNoofEmployees.Visible = False
                        'ddlNoofEmployees.Visible = False
                        'lblNoOfOffices.Visible = False
                        ' txtNoOfOffices.Visible = False
                        'lblTradeSince.Visible = False
                        'txtTradingSince.Visible = False
                        'lblMainContact.Visible = False
                        'txtMainContact.Visible = False
                        'pnlCharityDetails.Visible = False
                        'lblCharityDetails.Visible = False

                        'txtAccbalance.Text = ""
                        'txtYearTodateTurnOver.Text = ""
                        'txtLastYearturnOver.Text = ""

                        'VijayakumarEnd
                        ' txtIsagent.Text = objBaseparty.ClientDetail.IsAgent

                        gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                        gvConcictions.DataBind()
                        ''''''''''''''''''''''''''''''''Lifestyle''''''''''''''''''''''''''''''''''''''''''

                        txtDobirth.Text = objBaseparty.DateOfBirth
                        If objBaseparty.MaritalStatusCodeSpecified Then
                            ddmaritalstatus.ClearSelection()
                            ddmaritalstatus.SelectedIndex = objBaseparty.MaritalStatusCode + 1
                        Else
                            ddmaritalstatus.Items(0).Selected = True
                        End If

                        If objBaseparty.ClientDetail.SeasonalGiftCode IsNot Nothing AndAlso objBaseparty.ClientDetail.SeasonalGiftCode <> "" Then
                            ddseasonalgift.Items.FindByValue(objBaseparty.ClientDetail.SeasonalGiftCode).Selected = True
                        End If
                        txtloyalty.Text = objBaseparty.ClientDetail.LoyaltyNumber
                        chkPets.Checked = objBaseparty.PetOwner
                        chksmoker.Checked = objBaseparty.Lifestyle(0).Smoker
                        If objBaseparty.GenderCode IsNot Nothing AndAlso objBaseparty.GenderCode <> "" Then
                            ddgender.Items.FindByText(objBaseparty.GenderCode).Selected = True
                        End If

                        If Not String.IsNullOrEmpty(objBaseparty.NationalityCode) Then
                            ddnationality.Items.FindByValue(objBaseparty.NationalityCode).Selected = True
                        End If

                        If Not String.IsNullOrEmpty(objBaseparty.AccommodationCode) Then
                            ddlAccomodation.Items.FindByText(objBaseparty.AccommodationCode).Selected = True
                        End If
                        'txtaccomodation.Text = objBaseparty.AccommodationCode

                        gvDependents.DataSource = objBaseparty.Lifestyle
                        gvDependents.DataBind()

                        gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
                        gvloyalty.DataBind()
                        '''''''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''''''''''''''''''
                        txtagentref.Text = objBaseparty.ClientDetail.AgentReference
                        txtcurrentagent.Text = objBaseparty.ClientDetail.CurrentIntermediaryName
                        If objBaseparty.ClientDetail.StrengthCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StrengthCode <> "" Then
                            ddlprosStrengthCode.Items.FindByValue(objBaseparty.ClientDetail.StrengthCode).Selected = True
                        End If
                        If objBaseparty.ClientDetail.StatusCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StatusCode <> "" Then
                            ddlprosStatus.Items.FindByValue(objBaseparty.ClientDetail.StatusCode).Selected = True
                        End If

                        txtPIcode.Text = objBaseparty.ClientDetail.PreviousInsurerCode
                        txtPIname.Text = objBaseparty.ClientDetail.PreviousInsurerName
                        txtPIname.Enabled = False
                        txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerCode
                        txtPBname.Text = objBaseparty.ClientDetail.PreviousBrokerName
                        txtPBname.Enabled = False
                        hdCurrentAgentKey.Value = objBaseparty.ClientDetail.CurrentIntermediaryKey
                        hdPreviousInsurerKey.Value = objBaseparty.ClientDetail.PreviousInsurerKey
                        hdPrevBrokerKey.Value = objBaseparty.ClientDetail.PreviousBrokerKey

                        gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
                        gvpolicies.DataBind()

                        ''''''''''''''''''''''''''''''Tax''''''''''''''''''''''''''''''''''''
                        txttaxno.Text = objBaseparty.TaxNumber
                        chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                        chkTaxExempt.Checked = objBaseparty.TaxExempt
                        txtpercentage.Text = objBaseparty.TaxPercentage
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    End If
                'For Corporate Client
                If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then

                    Dim objBaseparty As New BasePartyCCType
                    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)

                    txtClientCode.Enabled = False
                    'txtLastName.Enabled = False
                    'txtForeName.Enabled = False
                    'ddltitle.Enabled = False
                    'txtInitial.Enabled = False
                    'txtgrname.Enabled = False
                    'txtgrtype.Enabled = False

                    txtTradingName.Text = objBaseparty.CompanyName
                    txtMainContact.Text = objBaseparty.MainContact
                    txtCompanyReg.Text = objBaseparty.CompanyReg

                    txtAccbalance.Text = objBaseparty.ClientDetail.AccountBalance
                    txtLastYearturnOver.Text = objBaseparty.ClientDetail.LastYearTurnover
                    txtYearTodateTurnOver.Text = objBaseparty.ClientDetail.YearToDateTurnover

                    txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode
                    hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode
                    txtAccExecutiveName.Text = objBaseparty.AccountExecutive
                    txtLeadAgentCode.Text = objBaseparty.ClientDetail.LeadAgentCode
                    txtLeadAgentName.Text = objBaseparty.ClientDetail.LeadAgentName

                    txtAlternativeIdentifier.Text = objBaseparty.AlternativeId
                    If objBaseparty.ClientDetail.ServiceLevelCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ServiceLevelCode <> "" Then
                        ddServiceLevel.Items.FindByValue(objBaseparty.ClientDetail.ServiceLevelCode).Selected = True
                    End If
                    If objBaseparty.BranchCode IsNot Nothing AndAlso objBaseparty.BranchCode <> "" Then
                        ddBranch.Items.FindByValue(objBaseparty.BranchCode).Selected = True
                    End If
                    If objBaseparty.SubBranchCode IsNot Nothing AndAlso objBaseparty.SubBranchCode <> "" Then
                        ddSubBranch.Items.FindByValue(objBaseparty.SubBranchCode).Selected = True
                    End If

                    If objBaseparty.BusinessCode IsNot Nothing AndAlso objBaseparty.BusinessCode <> "" Then
                        ddlBusiness.Items.FindByText(objBaseparty.BusinessCode).Selected = True
                    End If
                    If objBaseparty.TradeCode IsNot Nothing AndAlso objBaseparty.TradeCode <> "" Then
                        ddlTrade.Items.FindByText(objBaseparty.TradeCode).Selected = True
                    End If
                    If objBaseparty.SICCode IsNot Nothing AndAlso objBaseparty.SICCode <> "" Then
                        ddlSICcode.Items.FindByValue(objBaseparty.SICCode).Selected = True
                    End If
                    txtTradingSince.Text = objBaseparty.TradingSince
                    txtNoOfOffices.Text = objBaseparty.NumberOfOffices
                    If Not String.IsNullOrEmpty(objBaseparty.NumberOfEmployees) Then
                        ddlNoOfEmployees.Items.FindByText(objBaseparty.NumberOfEmployees).Selected = True
                    End If

                    txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode
                    hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode
                    txtAccExecutiveName.Text = objBaseparty.AccountExecutive
                    If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                        hdLeadAgent.Value = objBaseparty.ClientDetail.LeadAgentKey
                    End If
                    chkIsprospect.Checked = objBaseparty.ClientDetail.IsProspect
                    chkIsagent.Checked = objBaseparty.ClientDetail.IsAgent
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    'VijayakumarStart

                    ''''''''''''''''Commemted on 28-08-08
                    'txtAccbalance.Text = ""
                    'txtYearTodateTurnOver.Text = ""
                    'txtLastYearturnOver.Text = ""
                    'ddlArea.Visible = False
                    'txtFileCode.Visible = False
                    'ddlBusiness.Visible = True
                    'ddlTrade.Visible = True
                    'ddlSICcode.Visible = True
                    'txtNoOfOffices.Visible = True
                    'txtTradingSince.Visible = True
                    '''''''''''''''''''''

                    'lblArea.Visible = False
                    'lblFileCode.Visible = False
                    'lblBusiness.Visible = True
                    'lblTrade.Visible = True
                    'lblSICcode.Visible = True
                    'lblNoofEmployees.Visible = True
                    'ddlNoofEmployees.Visible = True
                    'lblNoOfOffices.Visible = True
                    'lblTradeSince.Visible = True
                    'lblMainContact.Visible = False
                    'txtMainContact.Visible = False
                    ' pnlCharityDetails.Visible = False
                    'lblCharityDetails.Visible = False

                    'VijayakumarEnd



                    'commented
                    'txtTradingName.Text = objBaseparty.TradeCode
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    GVContacts.DataSource = objBaseparty.Addresses
                    GVContacts.DataBind()

                    Dim dtContacts As New DataTable
                    Dim dcAreaCode As New DataColumn("AreaCode")
                    Dim dcNumber As New DataColumn("Number")
                        'Dim dcExtension As New DataColumn("Extension")
                    Dim dcType As New DataColumn("Type")
                    Dim dcDescription As New DataColumn("Description")
                    Dim drContacts As DataRow

                    dtContacts.Columns.Add(dcAreaCode)
                    dtContacts.Columns.Add(dcNumber)
                        'dtContacts.Columns.Add(dcExtension)
                    dtContacts.Columns.Add(dcType)
                    dtContacts.Columns.Add(dcDescription)

                    Dim oContactType As BaseContactType
                    If (objBaseparty.Contacts IsNot Nothing) Then
                        For Each oContactType In objBaseparty.Contacts
                            drContacts = dtContacts.NewRow()
                            drContacts("AreaCode") = oContactType.AreaCode
                            drContacts("Number") = oContactType.ContactDetail.Item
                                'drContacts("Extension") = ""
                            drContacts("Type") = oContactType.ContactTypeCode
                            drContacts("Description") = oContactType.Description
                            dtContacts.Rows.Add(drContacts)
                        Next
                    End If
                    GVContacts1.DataSource = dtContacts
                    GVContacts1.DataBind()
                    'GVContacts1.DataSource = objBaseparty.Contacts
                    GVContacts1.DataBind()

                    ddlPriOccupation.Enabled = False
                    ddlPriEmpBusiness.Enabled = False
                    ddlPriStatus.Enabled = False
                    ddlSecOccupation.Enabled = False
                    ddlSecEmpsBusiness.Enabled = False
                    ddlSecStatus.Enabled = False


                    txtsalutation.Text = objBaseparty.Salutation
                    'txtpreferedcorr.Text = objBaseparty.ClientDetail.CorrespondenceCode
                    If Not (String.IsNullOrEmpty(objBaseparty.ClientDetail.CorrespondenceCode)) Then
                        ddlPreferedCorres.Items.FindByValue(objBaseparty.ClientDetail.CorrespondenceCode).Selected = True
                    End If
                    chkTPS.Checked = objBaseparty.TPS
                    chkMPS.Checked = objBaseparty.MPS
                    chkeMPS.Checked = objBaseparty.eMPS

                    'txtIsTps.Text = objBaseparty.TPS
                    'txtIsMps.Text = objBaseparty.MPS
                    'txtIseMps.Text = objBaseparty.eMPS
                    'txtIsprospect.Text = objBaseparty.ClientDetail.IsProspect
                    ' txtIsagent.Text = objBaseparty.ClientDetail.IsAgent
                    ' txtpayment.Text = objBaseparty.ClientDetail.PaymentCode
                    'txtturnover.Text = objBaseparty.TurnoverCode
                    ' txtreminder.Text = objBaseparty.ClientDetail.ReminderCode
                    ''''''''''''''''''''''''''''''''''''Additions'''''''''''''''''''''''''''''''''''''''''''''
                    If objBaseparty.Currency IsNot Nothing AndAlso objBaseparty.Currency <> "" Then
                        ddlCurrency.Items.FindByText(Trim(objBaseparty.Currency)).Selected = True
                    End If
                    If objBaseparty.ClientDetail.PaymentCode IsNot Nothing AndAlso objBaseparty.ClientDetail.PaymentCode <> "" Then
                        ddlPaymentMethod.Items.FindByText(objBaseparty.ClientDetail.PaymentCode).Selected = True
                    End If
                    If objBaseparty.ClientDetail.ReminderCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ReminderCode <> "" Then
                        ddlReminderType.Items.FindByValue(objBaseparty.ClientDetail.ReminderCode).Selected = True
                    End If
                    If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.PaymentTermCode) Then
                        ddlTermsOfPayment.Items.FindByValue(objBaseparty.ClientDetail.PaymentTermCode).Selected = True
                    End If
                    If objBaseparty.ClientDetail.RenewalStopCode IsNot Nothing AndAlso objBaseparty.ClientDetail.RenewalStopCode <> "" Then
                        ddlrenewaldtopcode.Items.FindByValue(objBaseparty.ClientDetail.RenewalStopCode).Selected = True
                    End If
                    txtsource.Text = objBaseparty.Source
                    txtLoyaltyNumber.Text = objBaseparty.ClientDetail.LoyaltyNumber
                    If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.SeasonalGiftCode) Then
                        ddlSeasonalGift.Items.FindByValue(objBaseparty.ClientDetail.SeasonalGiftCode).Selected = True
                    End If

                    txtwage.Text = objBaseparty.WageRoll
                    If Not String.IsNullOrEmpty(objBaseparty.TurnoverCode) Then
                        'Dim i As Integer
                        'i = Convert.ToInt32(Convert.ToDouble(objBaseparty.TurnoverCode))
                        ddlturnover.Items.FindByValue(objBaseparty.TurnoverCode).Selected = True
                    End If
                    txtfinancial.Text = objBaseparty.FinancialYear

                    If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.AreaCode) Then
                        ddlAdditionsArea.Items.FindByValue(objBaseparty.ClientDetail.AreaCode).Selected = True
                    End If
                    txtAdditionsFileCode.Text = objBaseparty.FileCode

                    'txttermspay.Text = objBaseparty.ClientDetail.PaymentTermCode
                    'txtrenewal.Text = objBaseparty.ClientDetail.RenewalStopCode

                    gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                    gvassociates.DataBind()

                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()

                    txtDobirth.Enabled = False
                    'txtmatital.Enabled = False
                    ddmaritalstatus.Enabled = False

                    'txtseasonal.Text = objBaseparty.ClientDetail.SeasonalGiftCode
                    If objBaseparty.ClientDetail.SeasonalGiftCode IsNot Nothing AndAlso objBaseparty.ClientDetail.SeasonalGiftCode <> "" Then
                        ddseasonalgift.Items.FindByValue(objBaseparty.ClientDetail.SeasonalGiftCode).Selected = True
                    End If
                    txtloyalty.Text = objBaseparty.ClientDetail.LoyaltyNumber
                    pnlLifeStyle.Visible = False

                    gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
                    gvloyalty.DataBind()

                    ''''''''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''''''''''''''''
                    txtagentref.Text = objBaseparty.ClientDetail.AgentReference
                    txtcurrentagent.Text = objBaseparty.ClientDetail.CurrentIntermediaryName
                    If objBaseparty.ClientDetail.StrengthCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StrengthCode <> "" Then
                        ddlprosStrengthCode.Items.FindByValue(objBaseparty.ClientDetail.StrengthCode).Selected = True
                    End If
                    If objBaseparty.ClientDetail.StatusCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StatusCode <> "" Then
                        ddlprosStatus.Items.FindByValue(objBaseparty.ClientDetail.StatusCode).Selected = True
                    End If

                    txtPIcode.Text = objBaseparty.ClientDetail.PreviousInsurerCode
                    txtPIname.Text = objBaseparty.ClientDetail.PreviousInsurerName
                    txtPIname.Enabled = False
                    txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerCode
                    txtPBname.Text = objBaseparty.ClientDetail.PreviousBrokerName
                    txtPBname.Enabled = False

                    hdCurrentAgentKey.Value = objBaseparty.ClientDetail.CurrentIntermediaryKey
                    hdPreviousInsurerKey.Value = objBaseparty.ClientDetail.PreviousInsurerKey
                    hdPrevBrokerKey.Value = objBaseparty.ClientDetail.PreviousBrokerKey

                    gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
                    gvpolicies.DataBind()
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    txttaxno.Text = objBaseparty.TaxNumber
                    chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                    chkTaxExempt.Checked = objBaseparty.TaxExempt
                    txtpercentage.Text = objBaseparty.TaxPercentage
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' txtpstatus.Text = objBaseparty.ClientDetail.StatusCode
                    ' txtstrength.Text = objBaseparty.ClientDetail.StrengthCode
                    'txtisdomicile.Text = objBaseparty.DomiciledForTax
                End If
                If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then

                    Dim objBaseparty As New BasePartyOTHERType
                    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)

                    txtClientCode.Enabled = False
                    txtLastName.Enabled = False
                    txtForeName.Enabled = False
                    ddlTitle.Enabled = False
                    txtInitial.Enabled = False
                    txtwage.Enabled = False
                    'txtturnover.Enabled = False
                    ddlturnover.Enabled = False
                    txtfinancial.Enabled = False
                    txtsource.Enabled = False

                    'txtgrname.Text = objBaseparty.Name
                    'txtgrtype.Text = objBaseparty.TypeCode

                    GVContacts.DataSource = objBaseparty.Addresses
                    GVContacts.DataBind()

                    GVContacts1.DataSource = objBaseparty.Contacts
                    GVContacts1.DataBind()

                    pnlcorrespondence.Visible = False

                    If Not objBaseparty.Currency Is Nothing Then
                        ddlCurrency.Items.FindByValue(objBaseparty.Currency).Selected = True
                    End If
                    'pnlemp.Visible = False

                    btnAssosicates.Visible = False

                    gvConcictions.DataSource = objBaseparty.Convictions
                    gvConcictions.DataBind()

                    txtDobirth.Text = objBaseparty.DateOfBirth
                    pnlLifeStyle.Visible = False
                    pnlLoyaltySchemes.Visible = False

                    txtagentref.Enabled = False
                    txtcurrentagent.Enabled = False
                    'txtstrength.Enabled = False
                    ddlprosStrengthCode.Enabled = False
                    'txtpstatus.Enabled = False
                    ddlprosStatus.Enabled = False
                    txtPIcode.Enabled = False
                    txtPIname.Enabled = False
                    txtPIname.Enabled = False
                    txtPBcode.Enabled = False
                    txtPBname.Enabled = False
                    txtPBname.Enabled = False
                    btnAddPolicies.Visible = False

                    'pnlprospect.Visible = False

                    txttaxno.Text = objBaseparty.TaxNumber
                    chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                    chkTaxExempt.Text = objBaseparty.TaxExempt
                    txtpercentage.Text = objBaseparty.TaxPercentage
                End If
                'SAMHelper.setTextFieldsInGridView(GVContacts1)
                'SAMHelper.setTextFieldsInGridView(gvassociates)
                'SAMHelper.setTextFieldsInGridView(gvConcictions)
                'SAMHelper.setTextFieldsInGridView(gvDependents)
                'SAMHelper.setTextFieldsInGridView(gvloyalty)
                'SAMHelper.setTextFieldsInGridView(gvpolicies)
                'SAMHelper.setTextFieldsInGridView(GVContacts)
            End If
            oGetPartyResponseType = Session("GetPartyResponse")
            End If
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try

    End Sub
    Private Sub loadDefaultValueInCombo()
        ddlCurrency.Items.Insert(0, "--Select--")
        ddlPaymentMethod.Items.Insert(0, "--Select--")
        ddlReminderType.Items.Insert(0, "--Select--")
        ddlrenewaldtopcode.Items.Insert(0, "--Select--")
        ddlPriOccupation.Items.Insert(0, "--Select--")
        ddlPriEmpBusiness.Items.Insert(0, "--Select--")
        'ddlPriStatus.Items.Insert(0, "--Select--")
        ddlSecOccupation.Items.Insert(0, "--Select--")
        ddlSecEmpsBusiness.Items.Insert(0, "--Select--")
        'ddlSecStatus.Items.Insert(0, "--Select--")
        ddlturnover.Items.Insert(0, "--Select--")
        ddlrelationshipcode.Items.Insert(0, "--Select--")
        ddlPreferedCorres.Items.Insert(0, "--Select--")
        ddlconvictiontype.Items.Insert(0, "--Select--")
        ddlconvictionstatus.Items.Insert(0, "--Select--")
        ddlsentencetype.Items.Insert(0, "--Select--")
        ddlsentencetime.Items.Insert(0, "--Select--")
        ddlAlcoholmethod.Items.Insert(0, "--Select--")
        ddgender.Items.Insert(0, "--Select--")
        ddnationality.Items.Insert(0, "--Select--")
        ddseasonalgift.Items.Insert(0, "--Select--")
        ddlcategory.Items.Insert(0, "--Select--")
        ddloccupationcode.Items.Insert(0, "--Select--")
        ddlsecoccupationcode.Items.Insert(0, "--Select--")
        ddlprosStrengthCode.Items.Insert(0, "--Select--")
        ddlprosStatus.Items.Insert(0, "--Select--")
        ddlprostype.Items.Insert(0, "--Select--")
        ddlLoyaltySchemes.Items.Insert(0, "--Select--")
        ddBranch.Items.Insert(0, "--Select--")
        ddSubBranch.Items.Insert(0, "--Select--")
        ddlconCountry.Items.Insert(0, "--Select--")
        ddlconType.Items.Insert(0, "--Select--")
        ddlconContact.Items.Insert(0, "--Select--")
        ddlTitle.Items.Insert(0, "--Select--")
    End Sub
    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pnlAddConvictions.Visible = True
        gvConcictions.SelectedIndex = -1
        ClearConvictions()
    End Sub

    Private Sub ClearConvictions()
        ddlconvictiontype.ClearSelection()
        txtcfine.Text = ""
        ddlconvictionstatus.ClearSelection()
        txtcdate.Text = ""
        txtcdescription.Text = ""
        txtsdescription.Text = ""
        ddlsentencetype.ClearSelection()
        txtsdate.Text = ""
        ddlsentencetime.ClearSelection()
        ddlAlcoholmethod.ClearSelection()
        txtalcohollevel.Text = ""
        txtpenality.Text = ""
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlAddConvictions.Visible = False
        gvConcictions.SelectedIndex = -1
    End Sub

    Protected Sub gvDependents_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDependents.SelectedIndexChanged
        Try
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                        oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                Dim objBaseparty As New BasePartyPCType

                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

                txtName.Text = objBaseparty.Lifestyle(gvDependents.SelectedIndex).Name

                If objBaseparty.Lifestyle(gvDependents.SelectedIndex).DateOfBirthSpecified Then
                    txtDOB.Text = objBaseparty.Lifestyle(gvDependents.SelectedIndex).DateOfBirth
                Else
                    txtDOB.Text = ""
                End If

                ddlcategory.SelectedValue = objBaseparty.Lifestyle(gvDependents.SelectedIndex).CategoryCode

                If objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCodeSpecified Then
                    ddlGenderCode.SelectedValue = objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCode.ToString
                End If
                ddloccupationcode.SelectedValue = objBaseparty.Lifestyle(gvDependents.SelectedIndex).OccupationCode
                ddlsecoccupationcode.SelectedValue = objBaseparty.Lifestyle(gvDependents.SelectedIndex).SecOccupationCode
                If objBaseparty.Lifestyle(gvDependents.SelectedIndex).SmokerSpecified Then
                    chkIssmoker.Checked = objBaseparty.Lifestyle(gvDependents.SelectedIndex).Smoker
                End If
            End If
            pnlAddDependents.Visible = True

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnLSOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLSOk.Click
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")

            'Dim objBaseparty As New BasePartyPCType

            'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

            If gvDependents.SelectedIndex = -1 Then
                Dim objLS As New BasePartyPCTypeLifestyle
                objLS.Name = txtName.Text
                If IsDate(txtDOB.Text) Then
                    objLS.DateOfBirth = txtDOB.Text
                    objLS.DateOfBirthSpecified = True
                Else
                    objLS.DateOfBirth = SqlDateTime.Null
                End If

                objLS.CategoryCode = ddlcategory.SelectedValue
                If (ddlGenderCode.SelectedValue = "F") Then
                    objLS.GenderCode = GenderCodeType.F
                    objLS.GenderCodeSpecified = True
                Else
                    objLS.GenderCode = GenderCodeType.M
                    objLS.GenderCodeSpecified = True
                End If
                objLS.OccupationCode = ddloccupationcode.SelectedItem.ToString
                objLS.SecOccupationCode = ddlsecoccupationcode.SelectedItem.ToString

                If chkIssmoker.Checked = True Then
                    objLS.Smoker = True
                    objLS.SmokerSpecified = True
                Else
                    objLS.Smoker = False
                End If

                Dim vLifestyle() As BasePartyPCTypeLifestyle
                If objBaseparty.Lifestyle IsNot Nothing Then
                    ReDim vLifestyle(objBaseparty.Lifestyle.Length)
                    objBaseparty.Lifestyle.CopyTo(vLifestyle, 0)
                    vLifestyle(objBaseparty.Lifestyle.Length) = objLS
                Else
                    ReDim vLifestyle(0)
                    vLifestyle(0) = objLS
                End If
                objBaseparty.Lifestyle = vLifestyle
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            Else
                'Dim oGetPartyResponseType As New GetPartyResponseType
                'oGetPartyResponseType = Session("GetPartyResponse")

                objBaseparty.Lifestyle(gvDependents.SelectedIndex).Name = txtName.Text

                If IsDate(txtDOB.Text) Then
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).DateOfBirth = txtDOB.Text
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).DateOfBirthSpecified = True
                Else
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).DateOfBirth = SqlDateTime.Null
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).DateOfBirthSpecified = False
                End If

                objBaseparty.Lifestyle(gvDependents.SelectedIndex).CategoryCode = ddlcategory.SelectedValue
                If (ddlGenderCode.SelectedValue = "F") Then
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCode = GenderCodeType.F
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCodeSpecified = True
                Else
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCode = GenderCodeType.M
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCodeSpecified = True
                End If

                'objBaseparty.Lifestyle(gvDependents.SelectedIndex).GenderCode = ddlGenderCode.SelectedIndex

                objBaseparty.Lifestyle(gvDependents.SelectedIndex).OccupationCode = ddloccupationcode.SelectedItem.ToString
                objBaseparty.Lifestyle(gvDependents.SelectedIndex).SecOccupationCode = ddlsecoccupationcode.SelectedItem.ToString

                If chkIssmoker.Checked = True Then
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).Smoker = True
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).SmokerSpecified = True
                Else
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).Smoker = False
                End If

                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            End If
            gvDependents.DataSource = objBaseparty.Lifestyle
            gvDependents.DataBind()
            pnlAddDependents.Visible = False
            ' SAMHelper.setTextFieldsInGridView(gvDependents)
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnLSCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLSCancel.Click
        pnlAddDependents.Visible = False
    End Sub

    Protected Sub btnLSAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLSAdd.Click
        pnlAddDependents.Visible = True
        ClearLifeStyle()
        gvDependents.SelectedIndex = -1
    End Sub

    Private Sub ClearLifeStyle()
        txtName.Text = ""
        txtDOB.Text = ""
        ddlcategory.ClearSelection()
        ddlGenderCode.ClearSelection()
        ddloccupationcode.ClearSelection()
        ddlsecoccupationcode.ClearSelection()
        chkIssmoker.Checked = False
    End Sub

    Protected Sub btnAddPolicies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPolicies.Click
        pnlAddPolicies.Visible = True
        gvpolicies.SelectedIndex = -1
        ClearPolicies()
    End Sub

    Private Sub ClearPolicies()
        ddlprostype.ClearSelection()
        txtprewnal.Text = ""
        txttimequoted.Text = ""
        txttargetpremium.Text = ""
    End Sub

    Protected Sub btnLoyaltyAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoyaltyAdd.Click
        pnlAddLoyaltySchemes.Visible = True
        ClearLoyalty()
        gvloyalty.SelectedIndex = -1
    End Sub

    Private Sub ClearLoyalty()
        ddlLoyaltySchemes.ClearSelection()
        txtmembership.Text = ""
        txtotherref.Text = ""
        txtstart.Text = ""
        txtend.Text = ""
        txtMain.Text = ""
        chkActive.Checked = False
    End Sub

    Protected Sub btnAddLoyaltyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLoyaltyCancel.Click
        pnlAddLoyaltySchemes.Visible = False
        gvloyalty.SelectedIndex = -1
    End Sub

    Protected Sub btnPolicyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPolicyCancel.Click
        pnlAddPolicies.Visible = False
        gvpolicies.SelectedIndex = -1
    End Sub

    Protected Sub btnNewAssociates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAssociates.Click
        pnlNewAssociates.Visible = True
        gvassociates.SelectedIndex = -1
        ClearAssociates()
    End Sub

    Private Sub ClearAssociates()
        txtclient.Text = ""
        ddlrelationshipcode.ClearSelection()
    End Sub

    Protected Sub btnNewAssociatesOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAssociatesOk.Click

        Try
            If gvassociates.SelectedIndex = -1 Then
                Dim oGetPartyResponseType As New GetPartyResponseType
                oGetPartyResponseType = Session("GetPartyResponse")

                Dim objBaseparty As New Object
                objBaseparty = setobject()

                Dim objassociate As New BaseAssociateType
                objassociate.AssociateKey = hdAssociateKey.Value
                objassociate.RelationshipDescription = ddlrelationshipcode.SelectedItem.Text
                objassociate.RelationshipCode = ddlrelationshipcode.SelectedValue
                objassociate.ClientKey = Session("PartyKey")

                Dim vassociate() As BaseAssociateType
                If objBaseparty.ClientDetail.Associates IsNot Nothing Then
                    ReDim vassociate(objBaseparty.ClientDetail.Associates.Length)
                    objBaseparty.ClientDetail.Associates.CopyTo(vassociate, 0)
                    vassociate(objBaseparty.ClientDetail.Associates.Length) = objassociate
                Else
                    ReDim vassociate(0)
                    vassociate(0) = objassociate
                End If

                objBaseparty.ClientDetail.Associates = vassociate
                gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                gvassociates.DataBind()

                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
                pnlNewAssociates.Visible = False

            Else
                Dim oGetPartyResponseType As New GetPartyResponseType
                oGetPartyResponseType = Session("GetPartyResponse")

                If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                            oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                    Dim objBaseparty As New BasePartyPCType
                    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName = txtclient.Text
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey = Convert.ToInt32(hdAssociateKey.Value)
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipDescription = ddlrelationshipcode.Text
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode = ddlrelationshipcode.SelectedValue
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey = Session("PartyKey")
                    oGetPartyResponseType.Item = objBaseparty
                    gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                    gvassociates.DataBind()
                End If
                If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                                    oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                    Dim objBaseparty As New BasePartyCCType
                    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)

                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName = txtclient.Text
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey = Convert.ToInt32(hdAssociateKey.Value)
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipDescription = ddlrelationshipcode.Text
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode = ddlrelationshipcode.SelectedValue
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey = Session("PartyKey")
                    gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                    gvassociates.DataBind()
                    oGetPartyResponseType.Item = objBaseparty
                End If
                Session("GetPartyResponse") = oGetPartyResponseType
                pnlNewAssociates.Visible = False
            End If
            gvassociates.SelectedIndex = -1
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnNewAssociatesCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAssociatesCancel.Click
        pnlNewAssociates.Visible = False
    End Sub

    Protected Sub btnAssosicates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssosicates.Click
        pnlAddAssociates.Visible = True
    End Sub

    Protected Sub GVContacts1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVContacts1.RowDeleting
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseContactType)(objBaseparty.Contacts, e.RowIndex)
            GVContacts1.DataSource = objBaseparty.Contacts
            GVContacts1.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            ' SAMHelper.setTextFieldsInGridView(GVContacts1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GVContacts_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVContacts.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseAddressWithContactsType)(objBaseparty.Addresses, e.RowIndex)
            GVContacts.DataSource = objBaseparty.Addresses
            GVContacts.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            'SAMHelper.setTextFieldsInGridView(GVContacts)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvassociates_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvassociates.RowDeleting
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseAssociateType)(objBaseparty.ClientDetail.Associates, e.RowIndex)
            gvassociates.DataSource = objBaseparty.ClientDetail.Associates
            gvassociates.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            'SAMHelper.setTextFieldsInGridView(gvassociates)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvConcictions_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvConcictions.RowDeleting
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseConvictionType)(objBaseparty.ClientDetail.Convictions, e.RowIndex)
            gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
            gvConcictions.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            ' SAMHelper.setTextFieldsInGridView(gvConcictions)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvDependents_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDependents.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BasePartyPCTypeLifestyle)(objBaseparty.Lifestyle, e.RowIndex)
            gvDependents.DataSource = objBaseparty.Lifestyle
            gvDependents.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            'SAMHelper.setTextFieldsInGridView(gvDependents)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvloyalty_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvloyalty.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseClientSharedDataTypeLoyaltyScheme)(objBaseparty.ClientDetail.LoyaltyScheme, e.RowIndex)
            gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
            gvloyalty.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            ' SAMHelper.setTextFieldsInGridView(gvloyalty)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvpolicies_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvpolicies.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType
            oGetPartyResponseType = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseClientSharedDataTypeProspectPolicies)(objBaseparty.ClientDetail.ProspectPolicies, e.RowIndex)
            gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
            gvpolicies.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = oGetPartyResponseType
            'SAMHelper.setTextFieldsInGridView(gvpolicies)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub getParty()
        Try

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            'set up the proxy object
            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oGetPartyRequestType As New GetPartyRequestType
            With oGetPartyRequestType
                .PartyKey = Session("PartyKey")
                .BranchCode = "Headoff"
            End With
            oGetPartyResponseType = oSAM.GetParty(oGetPartyRequestType)
            Session("GetPartyResponse") = oGetPartyResponseType
            With oGetPartyResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
            End With
            'add to session
            Session("GetPartyResponse") = oGetPartyResponseType

            'For Personal Client
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                Dim objBaseparty As New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                'Binding the mandatory fields with the relevant output parameters
                With objBaseparty
                    txtClientCode.Text = .Surname & "" & .Initials
                    txtLastName.Text = .Surname
                    txtForeName.Text = .Forename
                    ddlTitle.ClearSelection()
                    ddlTitle.Items.FindByText(.Title).Selected = True
                    txtInitial.Text = .Initials
                    txtTradingName.Text = .TradingName
                End With
                txtAccbalance.Text = objBaseparty.ClientDetail.AccountBalance
                txtLastYearturnOver.Text = objBaseparty.ClientDetail.LastYearTurnover
                txtYearTodateTurnOver.Text = objBaseparty.ClientDetail.YearToDateTurnover
                chkIsprospect.Checked = objBaseparty.ClientDetail.IsProspect
                chkIsagent.Checked = objBaseparty.ClientDetail.IsAgent

                txtAlternativeIdentifier.Text = objBaseparty.AlternativeId
                If objBaseparty.ClientDetail.ServiceLevelCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ServiceLevelCode <> "" Then
                    ddServiceLevel.SelectedValue = objBaseparty.ClientDetail.ServiceLevelCode
                End If
                If objBaseparty.BranchCode IsNot Nothing AndAlso objBaseparty.BranchCode <> "" Then
                    ddBranch.SelectedValue = objBaseparty.BranchCode
                End If
                If objBaseparty.SubBranchCode IsNot Nothing AndAlso objBaseparty.SubBranchCode <> "" Then
                    ddSubBranch.SelectedValue = objBaseparty.SubBranchCode
                End If

                txtLeadAgentCode.Text = objBaseparty.ClientDetail.LeadAgentCode
                txtLeadAgentName.Text = objBaseparty.ClientDetail.LeadAgentName
                If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                    hdLeadAgent.Value = objBaseparty.ClientDetail.LeadAgentKey
                End If

                If objBaseparty.ClientDetail.AreaCode IsNot Nothing AndAlso objBaseparty.ClientDetail.AreaCode <> "" Then
                    ddlArea.SelectedValue = objBaseparty.ClientDetail.AreaCode
                End If
                txtFileCode.Text = objBaseparty.FileCode

                hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode
                txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode
                txtAccExecutiveName.Text = objBaseparty.AccountExecutive
                '''''''''''''''''Address Tab'''''''''''''''''''''''
                GVContacts.Visible = True
                GVContacts.DataSource = objBaseparty.Addresses
                GVContacts.DataBind()

                'Dim dsContacts As New DataSet
                Dim dtContacts As New DataTable
                Dim dcAreaCode As New DataColumn("AreaCode")
                Dim dcNumber As New DataColumn("Number")
                'Dim dcExtension As New DataColumn("Extension")
                Dim dcType As New DataColumn("Type")
                Dim dcDescription As New DataColumn("Description")
                Dim drContacts As DataRow

                dtContacts.Columns.Add(dcAreaCode)
                dtContacts.Columns.Add(dcNumber)
                'dtContacts.Columns.Add(dcExtension)
                dtContacts.Columns.Add(dcType)
                dtContacts.Columns.Add(dcDescription)

                Dim oContactType As BaseContactType
                If (objBaseparty.Contacts IsNot Nothing) Then
                    For Each oContactType In objBaseparty.Contacts
                        drContacts = dtContacts.NewRow()
                        drContacts("AreaCode") = oContactType.AreaCode
                        drContacts("Number") = oContactType.ContactDetail.Item
                        'drContacts("Extension") = ""
                        drContacts("Type") = oContactType.ContactTypeCode
                        drContacts("Description") = oContactType.Description
                        dtContacts.Rows.Add(drContacts)
                    Next
                End If
                GVContacts1.DataSource = dtContacts
                GVContacts1.DataBind()

                txtsalutation.Text = objBaseparty.Salutation
                ddlPreferedCorres.SelectedValue = objBaseparty.ClientDetail.CorrespondenceCode
                chkTPS.Checked = objBaseparty.TPS
                chkMPS.Checked = objBaseparty.MPS
                chkeMPS.Checked = objBaseparty.eMPS
                '''''''''''''''''''''''''''Additions Tab'''''''''''''''''''''''''''''''
                If Not objBaseparty.Currency Is Nothing Then
                    ddlCurrency.SelectedValue = objBaseparty.Currency
                End If
                If objBaseparty.ClientDetail.PaymentCode IsNot Nothing AndAlso objBaseparty.ClientDetail.PaymentCode <> "" Then
                    ddlPaymentMethod.ClearSelection()
                    ddlPaymentMethod.Items.FindByText(objBaseparty.ClientDetail.PaymentCode).Selected = True
                End If
                If objBaseparty.ClientDetail.ReminderCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ReminderCode <> "" Then
                    ddlReminderType.SelectedValue = objBaseparty.ClientDetail.ReminderCode
                End If
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.PaymentTermCode) Then
                    ddlTermsOfPayment.SelectedValue = objBaseparty.ClientDetail.PaymentTermCode
                End If

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.RenewalStopCode) Then
                    ddlrenewaldtopcode.SelectedValue = objBaseparty.ClientDetail.RenewalStopCode
                End If

                txtsource.Text = objBaseparty.Source
                If objBaseparty.OccupationCode IsNot Nothing AndAlso objBaseparty.OccupationCode <> "" Then
                    ddlPriOccupation.ClearSelection()
                    ddlPriOccupation.Items.FindByText(objBaseparty.OccupationCode).Selected = True
                End If
                If objBaseparty.EmployersBusinessCode IsNot Nothing AndAlso objBaseparty.EmployersBusinessCode <> "" Then
                    ddlPriEmpBusiness.ClearSelection()
                    ddlPriEmpBusiness.Items.FindByText(objBaseparty.EmployersBusinessCode).Selected = True
                End If

                If objBaseparty.EmploymentStatusCodeSpecified Then
                    ddlPriStatus.SelectedIndex = objBaseparty.EmploymentStatusCode + 1
                Else
                    ddlPriStatus.Items(0).Selected = True
                End If

                If objBaseparty.SecOccupationCode IsNot Nothing AndAlso objBaseparty.SecOccupationCode <> "" Then
                    ddlSecOccupation.ClearSelection()
                    ddlSecOccupation.Items.FindByText(objBaseparty.SecOccupationCode).Selected = True
                End If

                If objBaseparty.SecEmployersBusinessCode IsNot Nothing AndAlso objBaseparty.SecEmployersBusinessCode <> "" Then
                    ddlSecEmpsBusiness.ClearSelection()
                    ddlSecEmpsBusiness.Items.FindByText(objBaseparty.SecEmployersBusinessCode).Selected = True
                End If
                If objBaseparty.SecEmploymentStatusCodeSpecified Then
                    ddlSecStatus.ClearSelection()
                    ddlSecStatus.SelectedIndex = objBaseparty.SecEmploymentStatusCode + 1
                Else
                    ddlSecStatus.Items(0).Selected = True
                End If

                gvassociates.Visible = True
                gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                gvassociates.DataBind()
                txtFileCode.Visible = True
                ddlArea.Visible = True

                gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                gvConcictions.DataBind()
                ''''''''''''''''''''''''''''''''Lifestyle''''''''''''''''''''''''''''''''''''''''''

                txtDobirth.Text = objBaseparty.DateOfBirth
                If objBaseparty.MaritalStatusCodeSpecified Then
                    ddmaritalstatus.ClearSelection()
                    ddmaritalstatus.SelectedIndex = objBaseparty.MaritalStatusCode + 1
                Else
                    ddmaritalstatus.ClearSelection()
                    ddmaritalstatus.Items(0).Selected = True
                End If

                If objBaseparty.ClientDetail.SeasonalGiftCode IsNot Nothing AndAlso objBaseparty.ClientDetail.SeasonalGiftCode <> "" Then
                    ddseasonalgift.SelectedValue = objBaseparty.ClientDetail.SeasonalGiftCode
                End If
                txtloyalty.Text = objBaseparty.ClientDetail.LoyaltyNumber
                chkPets.Checked = objBaseparty.PetOwner
                chksmoker.Checked = objBaseparty.Lifestyle(0).Smoker
                If objBaseparty.GenderCode IsNot Nothing AndAlso objBaseparty.GenderCode <> "" Then
                    ddgender.Items.FindByText(objBaseparty.GenderCode).Selected = True
                End If

                If Not String.IsNullOrEmpty(objBaseparty.NationalityCode) Then
                    ddnationality.SelectedValue = objBaseparty.NationalityCode
                End If

                If Not String.IsNullOrEmpty(objBaseparty.AccommodationCode) Then
                    ddlAccomodation.ClearSelection()
                    ddlAccomodation.Items.FindByText(objBaseparty.AccommodationCode).Selected = True
                End If

                gvDependents.DataSource = objBaseparty.Lifestyle
                gvDependents.DataBind()

                gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
                gvloyalty.DataBind()
                '''''''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''''''''''''''''''
                txtagentref.Text = objBaseparty.ClientDetail.AgentReference
                txtcurrentagent.Text = objBaseparty.ClientDetail.CurrentIntermediaryName
                If objBaseparty.ClientDetail.StrengthCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StrengthCode <> "" Then
                    ddlprosStrengthCode.SelectedValue = objBaseparty.ClientDetail.StrengthCode
                End If
                If objBaseparty.ClientDetail.StatusCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StatusCode <> "" Then
                    ddlprosStatus.SelectedValue = objBaseparty.ClientDetail.StatusCode
                End If

                txtPIcode.Text = objBaseparty.ClientDetail.PreviousInsurerCode
                txtPIname.Text = objBaseparty.ClientDetail.PreviousInsurerName
                txtPIname.Enabled = False
                txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerCode
                txtPBname.Text = objBaseparty.ClientDetail.PreviousBrokerName
                txtPBname.Enabled = False
                hdCurrentAgentKey.Value = objBaseparty.ClientDetail.CurrentIntermediaryKey
                hdPreviousInsurerKey.Value = objBaseparty.ClientDetail.PreviousInsurerKey
                hdPrevBrokerKey.Value = objBaseparty.ClientDetail.PreviousBrokerKey

                gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
                gvpolicies.DataBind()

                ''''''''''''''''''''''''''''''Tax''''''''''''''''''''''''''''''''''''
                txttaxno.Text = objBaseparty.TaxNumber
                chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                chkTaxExempt.Checked = objBaseparty.TaxExempt
                txtpercentage.Text = objBaseparty.TaxPercentage
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If
            'For Corporate Client
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then

                Dim objBaseparty As New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)

                txtClientCode.Enabled = False

                txtTradingName.Text = objBaseparty.CompanyName
                txtMainContact.Text = objBaseparty.MainContact
                txtCompanyReg.Text = objBaseparty.CompanyReg

                txtAccbalance.Text = objBaseparty.ClientDetail.AccountBalance
                txtLastYearturnOver.Text = objBaseparty.ClientDetail.LastYearTurnover
                txtYearTodateTurnOver.Text = objBaseparty.ClientDetail.YearToDateTurnover

                hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode
                txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode

                txtAccExecutiveName.Text = objBaseparty.AccountExecutive
                txtLeadAgentCode.Text = objBaseparty.ClientDetail.LeadAgentCode
                txtLeadAgentName.Text = objBaseparty.ClientDetail.LeadAgentName

                txtAlternativeIdentifier.Text = objBaseparty.AlternativeId
                If objBaseparty.ClientDetail.ServiceLevelCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ServiceLevelCode <> "" Then
                    ddServiceLevel.SelectedValue = objBaseparty.ClientDetail.ServiceLevelCode
                End If
                If objBaseparty.BranchCode IsNot Nothing AndAlso objBaseparty.BranchCode <> "" Then
                    ddBranch.SelectedValue = objBaseparty.BranchCode
                End If
                If objBaseparty.SubBranchCode IsNot Nothing AndAlso objBaseparty.SubBranchCode <> "" Then
                    ddSubBranch.SelectedValue = objBaseparty.SubBranchCode
                End If

                If objBaseparty.BusinessCode IsNot Nothing AndAlso objBaseparty.BusinessCode <> "" Then
                    ddlBusiness.ClearSelection()
                    ddlBusiness.Items.FindByText(objBaseparty.BusinessCode).Selected = True
                End If
                If objBaseparty.TradeCode IsNot Nothing AndAlso objBaseparty.TradeCode <> "" Then
                    ddlTrade.ClearSelection()
                    ddlTrade.Items.FindByText(objBaseparty.TradeCode).Selected = True
                End If
                If objBaseparty.SICCode IsNot Nothing AndAlso objBaseparty.SICCode <> "" Then
                    ddlSICcode.SelectedValue = objBaseparty.SICCode
                End If
                txtTradingSince.Text = objBaseparty.TradingSince
                txtNoOfOffices.Text = objBaseparty.NumberOfOffices
                If Not String.IsNullOrEmpty(objBaseparty.NumberOfEmployees) Then
                    ddlNoOfEmployees.ClearSelection()
                    ddlNoOfEmployees.Items.FindByText(objBaseparty.NumberOfEmployees).Selected = True
                End If

                hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode
                txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode
                txtAccExecutiveName.Text = objBaseparty.AccountExecutive
                If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                    hdLeadAgent.Value = objBaseparty.ClientDetail.LeadAgentKey
                End If
                chkIsprospect.Checked = objBaseparty.ClientDetail.IsProspect
                chkIsagent.Checked = objBaseparty.ClientDetail.IsAgent
                ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                GVContacts.DataSource = objBaseparty.Addresses
                GVContacts.DataBind()

                Dim dtContacts As New DataTable
                Dim dcAreaCode As New DataColumn("AreaCode")
                Dim dcNumber As New DataColumn("Number")
                'Dim dcExtension As New DataColumn("Extension")
                Dim dcType As New DataColumn("Type")
                Dim dcDescription As New DataColumn("Description")
                Dim drContacts As DataRow

                dtContacts.Columns.Add(dcAreaCode)
                dtContacts.Columns.Add(dcNumber)
                'dtContacts.Columns.Add(dcExtension)
                dtContacts.Columns.Add(dcType)
                dtContacts.Columns.Add(dcDescription)

                Dim oContactType As BaseContactType
                If (objBaseparty.Contacts IsNot Nothing) Then
                    For Each oContactType In objBaseparty.Contacts
                        drContacts = dtContacts.NewRow()
                        drContacts("AreaCode") = oContactType.AreaCode
                        drContacts("Number") = oContactType.ContactDetail.Item
                        'drContacts("Extension") = ""
                        drContacts("Type") = oContactType.ContactTypeCode
                        drContacts("Description") = oContactType.Description
                        dtContacts.Rows.Add(drContacts)
                    Next
                End If
                GVContacts1.DataSource = dtContacts
                GVContacts1.DataBind()
                GVContacts1.DataBind()

                ddlPriOccupation.Enabled = False
                ddlPriEmpBusiness.Enabled = False
                ddlPriStatus.Enabled = False
                ddlSecOccupation.Enabled = False
                ddlSecEmpsBusiness.Enabled = False
                ddlSecStatus.Enabled = False


                txtsalutation.Text = objBaseparty.Salutation
                If Not (String.IsNullOrEmpty(objBaseparty.ClientDetail.CorrespondenceCode)) Then
                    ddlPreferedCorres.SelectedValue = objBaseparty.ClientDetail.CorrespondenceCode
                End If
                chkTPS.Checked = objBaseparty.TPS
                chkMPS.Checked = objBaseparty.MPS
                chkeMPS.Checked = objBaseparty.eMPS

                ''''''''''''''''''''''''''''''''''''Additions'''''''''''''''''''''''''''''''''''''''''''''
                If objBaseparty.Currency IsNot Nothing AndAlso objBaseparty.Currency <> "" Then
                    ddlCurrency.ClearSelection()
                    ddlCurrency.Items.FindByText(Trim(objBaseparty.Currency)).Selected = True
                End If
                If objBaseparty.ClientDetail.PaymentCode IsNot Nothing AndAlso objBaseparty.ClientDetail.PaymentCode <> "" Then
                    ddlPaymentMethod.ClearSelection()
                    ddlPaymentMethod.Items.FindByText(objBaseparty.ClientDetail.PaymentCode).Selected = True
                End If
                If objBaseparty.ClientDetail.ReminderCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ReminderCode <> "" Then
                    ddlReminderType.SelectedValue = objBaseparty.ClientDetail.ReminderCode
                End If
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.PaymentTermCode) Then
                    ddlTermsOfPayment.SelectedValue = objBaseparty.ClientDetail.PaymentTermCode
                End If
                If objBaseparty.ClientDetail.RenewalStopCode IsNot Nothing AndAlso objBaseparty.ClientDetail.RenewalStopCode <> "" Then
                    ddlrenewaldtopcode.SelectedValue = objBaseparty.ClientDetail.RenewalStopCode
                End If
                txtsource.Text = objBaseparty.Source
                txtLoyaltyNumber.Text = objBaseparty.ClientDetail.LoyaltyNumber
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.SeasonalGiftCode) Then
                    ddlSeasonalGift.SelectedValue = objBaseparty.ClientDetail.SeasonalGiftCode
                End If

                txtwage.Text = objBaseparty.WageRoll
                If Not String.IsNullOrEmpty(objBaseparty.TurnoverCode) Then
                    ddlturnover.SelectedValue = objBaseparty.TurnoverCode
                End If
                txtfinancial.Text = objBaseparty.FinancialYear

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.AreaCode) Then
                    ddlAdditionsArea.SelectedValue = objBaseparty.ClientDetail.AreaCode
                End If
                txtAdditionsFileCode.Text = objBaseparty.FileCode

                gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                gvassociates.DataBind()

                gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                gvConcictions.DataBind()

                txtDobirth.Enabled = False
                ddmaritalstatus.Enabled = False

                If objBaseparty.ClientDetail.SeasonalGiftCode IsNot Nothing AndAlso objBaseparty.ClientDetail.SeasonalGiftCode <> "" Then
                    ddseasonalgift.SelectedValue = objBaseparty.ClientDetail.SeasonalGiftCode
                End If
                txtloyalty.Text = objBaseparty.ClientDetail.LoyaltyNumber
                pnlLifeStyle.Visible = False

                gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
                gvloyalty.DataBind()

                ''''''''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''''''''''''''''
                txtagentref.Text = objBaseparty.ClientDetail.AgentReference
                txtcurrentagent.Text = objBaseparty.ClientDetail.CurrentIntermediaryName
                If objBaseparty.ClientDetail.StrengthCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StrengthCode <> "" Then
                    ddlprosStrengthCode.SelectedValue = objBaseparty.ClientDetail.StrengthCode
                End If
                If objBaseparty.ClientDetail.StatusCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StatusCode <> "" Then
                    ddlprosStatus.SelectedValue = objBaseparty.ClientDetail.StatusCode
                End If

                txtPIcode.Text = objBaseparty.ClientDetail.PreviousInsurerCode
                txtPIname.Text = objBaseparty.ClientDetail.PreviousInsurerName
                txtPIname.Enabled = False
                txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerCode
                txtPBname.Text = objBaseparty.ClientDetail.PreviousBrokerName
                txtPBname.Enabled = False

                hdCurrentAgentKey.Value = objBaseparty.ClientDetail.CurrentIntermediaryKey
                hdPreviousInsurerKey.Value = objBaseparty.ClientDetail.PreviousInsurerKey
                hdPrevBrokerKey.Value = objBaseparty.ClientDetail.PreviousBrokerKey

                gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
                gvpolicies.DataBind()
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                txttaxno.Text = objBaseparty.TaxNumber
                chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                chkTaxExempt.Checked = objBaseparty.TaxExempt
                txtpercentage.Text = objBaseparty.TaxPercentage
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
        Try
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")
            Dim objBaseparty As Object
            objBaseparty = Setvalues()

            Dim oUpdatePartyRequestType As New UpdatePartyRequestType
            Dim oUpdatePartyResponseType As New UpdatePartyResponseType

            'set up request object with some values
            With oUpdatePartyRequestType
                .PartyKey = Session("PartyKey")
                .BranchCode = "HeadOff"
                .Item = objBaseparty
                .PartyTimestamp = oGetPartyResponseType.PartyTimestamp
            End With

            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            oUpdatePartyResponseType = oSAM.UpdateParty(oUpdatePartyRequestType)

            With oUpdatePartyResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                End If
            End With
            oGetPartyResponseType.PartyTimestamp = oUpdatePartyResponseType.PartyTimestamp
            'getParty()
            Response.Redirect("List Policy.aspx")
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnaddcontacts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddcontacts.Click
        plnAddContact.Visible = True
        GVContacts1.SelectedIndex = -1
        ClearContacts()
    End Sub
    Private Sub ClearContacts()
        ddlconContact.ClearSelection()
        txtdescription.Text = ""
        txtareacode.Text = ""
        txtNumber.Text = ""
        txtextension.Text = ""
    End Sub

    Protected Sub btnaddaddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddaddress.Click
        pnlAddAddress.Visible = True
        GVContacts.SelectedIndex = -1
        ClearAddress()
    End Sub
    Private Sub ClearAddress()
        ddlconType.ClearSelection()
        txtstname.Text = ""
        txtlocality.Text = ""
        txtposttown.Text = ""
        txtcounty.Text = ""
        txtpostcode.Text = ""
        ddlconCountry.ClearSelection()
    End Sub

    Protected Sub btnaok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaok.Click
        Try
            If (GVContacts.SelectedIndex = -1) Then

                Dim oGetPartyResponseType As New GetPartyResponseType

                oGetPartyResponseType = Session("GetPartyResponse")
                Dim objBaseparty As New Object
                objBaseparty = setobject()

                Dim objaddr As New BaseAddressWithContactsType
                'objaddr.AddressTypeCode = txttype.Text
                objaddr.AddressTypeCode = CType(ddlconType.SelectedIndex, AddressTypeType)

                objaddr.AddressLine1 = txtstname.Text
                objaddr.AddressLine3 = txtposttown.Text
                objaddr.PostCode = txtpostcode.Text
                objaddr.AddressLine2 = txtlocality.Text
                objaddr.AddressLine4 = txtcounty.Text
                'objaddr.CountryCode = txtcountry.Text
                objaddr.CountryCode = ddlconCountry.SelectedValue.ToString

                Dim vAddresses() As BaseAddressWithContactsType
                If objBaseparty.Addresses IsNot Nothing Then
                    ReDim vAddresses(objBaseparty.Addresses.Length)
                    objBaseparty.Addresses.CopyTo(vAddresses, 0)
                    vAddresses(objBaseparty.Addresses.Length) = objaddr
                Else
                    ReDim vAddresses(0)
                    vAddresses(0) = objaddr
                End If

                objBaseparty.Addresses = vAddresses
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType

                GVContacts.DataSource = objBaseparty.Addresses
                GVContacts.DataBind()
                pnlAddAddress.Visible = False
                ' SAMHelper.setTextFieldsInGridView(GVContacts)
            Else
                Dim oGetPartyResponseType As New GetPartyResponseType

                oGetPartyResponseType = Session("GetPartyResponse")
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressTypeCode = CType(ddlconType.SelectedIndex, AddressTypeType)
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine1 = txtstname.Text
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine3 = txtposttown.Text
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).PostCode = txtpostcode.Text
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine2 = txtlocality.Text
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine4 = txtcounty.Text
                oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).CountryCode = ddlconCountry.SelectedValue.ToString

                pnlAddAddress.Visible = False
                Session("GetPartyResponse") = oGetPartyResponseType
                GVContacts.DataSource = oGetPartyResponseType.Item.Addresses
                GVContacts.DataBind()

            End If
            GVContacts.SelectedIndex = -1

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try


    End Sub

    Protected Sub btncok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncok.Click
        Try
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")
            If GVContacts1.SelectedIndex = -1 Then

                Dim objBaseparty As New Object
                objBaseparty = setobject()

                Dim objcontact As New BaseContactType
                Dim obj As BaseContactDetailType

                obj = New BaseContactDetailType
                'obj.Item = txtcontact.Text
                obj.Item = txtNumber.Text
                obj.ItemElementName = ItemChoiceType.EmailAddress

                objcontact.ContactDetail = obj
                objcontact.AreaCode = txtareacode.Text
                objcontact.ContactTypeCode = ContactTypeType.EMAIL
                objcontact.Description = txtdescription.Text

                Dim vContact() As BaseContactType
                If objBaseparty.Contacts IsNot Nothing Then
                    ReDim vContact(objBaseparty.Contacts.Length)
                    objBaseparty.Contacts.CopyTo(vContact, 0)
                    vContact(objBaseparty.Contacts.Length) = objcontact
                Else
                    ReDim vContact(0)
                    vContact(0) = objcontact
                End If

                objBaseparty.Contacts = vContact
                Dim dtContacts As New DataTable
                Dim dcAreaCode As New DataColumn("AreaCode")
                Dim dcNumber As New DataColumn("Number")
                Dim dcExtension As New DataColumn("Extension")
                Dim dcType As New DataColumn("Type")
                Dim dcDescription As New DataColumn("Description")
                Dim drContacts As DataRow

                dtContacts.Columns.Add(dcAreaCode)
                dtContacts.Columns.Add(dcNumber)
                dtContacts.Columns.Add(dcExtension)
                dtContacts.Columns.Add(dcType)
                dtContacts.Columns.Add(dcDescription)

                Dim oContactType As BaseContactType
                If (objBaseparty.Contacts IsNot Nothing) Then
                    For Each oContactType In objBaseparty.Contacts
                        drContacts = dtContacts.NewRow()
                        drContacts("AreaCode") = oContactType.AreaCode
                        drContacts("Number") = oContactType.ContactDetail.Item
                        drContacts("Extension") = ""
                        drContacts("Type") = oContactType.ContactTypeCode
                        drContacts("Description") = oContactType.Description
                        dtContacts.Rows.Add(drContacts)
                    Next
                End If
                GVContacts1.DataSource = dtContacts
                GVContacts1.DataBind()
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            Else
                Dim objBaseparty As New Object
                objBaseparty = setobject()

                Dim objcontact As New BaseContactType
                Dim obj As BaseContactDetailType

                obj = New BaseContactDetailType
                obj.Item = txtNumber.Text
                obj.ItemElementName = ItemChoiceType.EmailAddress

                objcontact.ContactDetail = obj
                objcontact.AreaCode = txtareacode.Text
                objcontact.ContactTypeCode = ContactTypeType.EMAIL
                objcontact.Description = txtdescription.Text
                objBaseparty.Contacts(GVContacts1.SelectedIndex) = objcontact

                Dim dtContacts As New DataTable
                Dim dcAreaCode As New DataColumn("AreaCode")
                Dim dcNumber As New DataColumn("Number")
                Dim dcExtension As New DataColumn("Extension")
                Dim dcType As New DataColumn("Type")
                Dim dcDescription As New DataColumn("Description")
                Dim drContacts As DataRow

                dtContacts.Columns.Add(dcAreaCode)
                dtContacts.Columns.Add(dcNumber)
                dtContacts.Columns.Add(dcExtension)
                dtContacts.Columns.Add(dcType)
                dtContacts.Columns.Add(dcDescription)

                Dim oContactType As BaseContactType
                If (objBaseparty.Contacts IsNot Nothing) Then
                    For Each oContactType In objBaseparty.Contacts
                        drContacts = dtContacts.NewRow()
                        drContacts("AreaCode") = oContactType.AreaCode
                        drContacts("Number") = oContactType.ContactDetail.Item
                        drContacts("Extension") = ""
                        drContacts("Type") = oContactType.ContactTypeCode
                        drContacts("Description") = oContactType.Description
                        dtContacts.Rows.Add(drContacts)
                    Next
                End If
                GVContacts1.DataSource = dtContacts
                GVContacts1.DataBind()
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            End If

            plnAddContact.Visible = False
            GVContacts1.SelectedIndex = -1

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try

            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")


            If gvConcictions.SelectedIndex = -1 Then
                Dim objBaseparty As New Object
                objBaseparty = setobject()
                Dim objconv As New BaseConvictionType
                objconv.TypeCode = ddlconvictiontype.SelectedItem.Text
                objconv.StatusCode = ddlconvictionstatus.SelectedItem.Text
                objconv.Description = txtcdescription.Text
                If Not (String.IsNullOrEmpty(txtcfine.Text)) Then
                    objconv.FineAmount = txtcfine.Text
                    objconv.FineAmountSpecified = True
                End If
                If (IsDate(txtcdate.Text)) Then
                    objconv.Date = txtcdate.Text
                Else
                    objconv.Date = SqlDateTime.Null
                End If

                objconv.SentenceTypeCode = ddlsentencetype.SelectedItem.Text
                objconv.SentenceDescription = txtsdescription.Text
                If (IsDate(txtsdate.Text)) Then
                    objconv.SentenceEffectiveDate = txtsdate.Text
                    objconv.SentenceEffectiveDateSpecified = True
                Else
                    objconv.SentenceEffectiveDate = SqlDateTime.Null
                End If
                If Not (String.IsNullOrEmpty(txtsduration.Text)) Then
                    objconv.SentenceDuration = txtsduration.Text
                    objconv.SentenceDurationSpecified = True
                End If

                objconv.SentenceDurationQualifier = ddlsentencetime.SelectedItem.Text
                objconv.AlcoholMeasurementMethod = ddlAlcoholmethod.SelectedItem.Text
                If Not String.IsNullOrEmpty(txtalcohollevel.Text) Then
                    objconv.AlcoholLevel = txtalcohollevel.Text
                    objconv.AlcoholLevelSpecified = True
                End If
                If Not String.IsNullOrEmpty(txtpenality.Text) Then
                    objconv.DrivingLicensePenaltyPoints = txtpenality.Text
                    objconv.DrivingLicensePenaltyPointsSpecified = True
                End If
                Dim vConviction() As BaseConvictionType
                If objBaseparty.GetType Is GetType(BasePartyOTHERType) Then
                    If objBaseparty.Convictions IsNot Nothing Then
                        ReDim vConviction(objBaseparty.Convictions.Length)
                        objBaseparty.Convictions.CopyTo(vConviction, 0)
                        vConviction(objBaseparty.Convictions.Length) = objconv
                    Else
                        ReDim vConviction(0)
                        vConviction(0) = objconv
                    End If
                    objBaseparty.Convictions = vConviction
                    gvConcictions.DataSource = objBaseparty.Convictions
                    gvConcictions.DataBind()
                    pnlAddConvictions.Visible = False
                    'SAMHelper.setTextFieldsInGridView(gvConcictions)
                Else
                    If objBaseparty.ClientDetail.Convictions IsNot Nothing Then
                        ReDim vConviction(objBaseparty.ClientDetail.Convictions.Length)
                        objBaseparty.ClientDetail.Convictions.CopyTo(vConviction, 0)
                        vConviction(objBaseparty.ClientDetail.Convictions.Length) = objconv
                    Else
                        ReDim vConviction(0)
                        vConviction(0) = objconv
                    End If
                    objBaseparty.ClientDetail.Convictions = vConviction

                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()
                    pnlAddConvictions.Visible = False
                    'SAMHelper.setTextFieldsInGridView(gvConcictions)
                    oGetPartyResponseType.Item = objBaseparty
                    Session("GetPartyResponse") = oGetPartyResponseType
                End If
            Else
                Dim objBaseparty As New Object
                objBaseparty = setobject()

                'Dim objBaseparty As New BasePartyPCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                Dim objconv As New BaseConvictionType
                objconv.TypeCode = ddlconvictiontype.SelectedItem.Text
                objconv.StatusCode = ddlconvictionstatus.SelectedItem.Text
                objconv.Description = txtcdescription.Text
                If Not (String.IsNullOrEmpty(txtcfine.Text)) Then
                    objconv.FineAmount = txtcfine.Text
                    objconv.FineAmountSpecified = True
                End If

                If (IsDate(txtcdate.Text)) Then
                    objconv.Date = txtcdate.Text
                Else
                    objconv.Date = SqlDateTime.Null
                End If

                objconv.SentenceTypeCode = ddlsentencetype.SelectedItem.Text
                objconv.SentenceDescription = txtsdescription.Text
                If (IsDate(txtsdate.Text)) Then
                    objconv.SentenceEffectiveDate = txtsdate.Text
                    objconv.SentenceEffectiveDateSpecified = True
                Else
                    objconv.SentenceEffectiveDate = SqlDateTime.Null
                End If
                If Not (String.IsNullOrEmpty(txtsduration.Text)) Then
                    objconv.SentenceDuration = txtsduration.Text
                    objconv.SentenceDurationSpecified = True
                End If
                objconv.SentenceDurationQualifier = ddlsentencetime.SelectedItem.Text
                objconv.AlcoholMeasurementMethod = ddlAlcoholmethod.SelectedItem.Text
                If Not (String.IsNullOrEmpty(txtalcohollevel.Text)) Then
                    objconv.AlcoholLevel = txtalcohollevel.Text
                    objconv.AlcoholLevelSpecified = True
                End If
                If Not (String.IsNullOrEmpty(txtpenality.Text)) Then
                    objconv.DrivingLicensePenaltyPoints = txtpenality.Text
                    objconv.DrivingLicensePenaltyPointsSpecified = True
                End If
                objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex) = objconv
                gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                gvConcictions.DataBind()
                pnlAddConvictions.Visible = False

                If objBaseparty.GetType Is GetType(BasePartyOTHERType) Then

                    objBaseparty.Convictions(gvConcictions.SelectedIndex) = objconv
                    gvConcictions.DataSource = objBaseparty.Convictions
                    gvConcictions.DataBind()
                Else
                    objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex) = objconv
                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()
                End If

                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType

            End If
            gvConcictions.SelectedIndex = -1

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try

    End Sub

    Protected Sub btnAddLoyaltyAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLoyaltyAdd.Click
        Try

            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")
            Dim objBaseparty As New Object
            objBaseparty = setobject()

            If gvloyalty.SelectedIndex = -1 Then

                Dim objloyalty As New BaseClientSharedDataTypeLoyaltyScheme
                objloyalty.LoyaltySchemeCode = ddlLoyaltySchemes.SelectedValue
                objloyalty.MembershipNumber = txtmembership.Text
                objloyalty.OtherReference = txtotherref.Text
                If (IsDate(txtstart.Text)) Then
                    objloyalty.StartDate = txtstart.Text
                Else
                    objloyalty.StartDate = SqlDateTime.Null
                End If
                If (IsDate(txtend.Text)) Then
                    objloyalty.EndDate = txtend.Text
                    objloyalty.EndDateSpecified = True
                Else
                    objloyalty.EndDate = SqlDateTime.Null
                End If

                objloyalty.MainMember = txtMain.Text
                If chkActive.Checked = True Then
                    objloyalty.Active = True
                    objloyalty.ActiveSpecified = True
                Else
                    objloyalty.Active = False
                End If

                Dim vLoyalty() As BaseClientSharedDataTypeLoyaltyScheme

                If objBaseparty.ClientDetail.LoyaltyScheme IsNot Nothing Then
                    ReDim vLoyalty(objBaseparty.ClientDetail.LoyaltyScheme.Length)
                    objBaseparty.ClientDetail.LoyaltyScheme.CopyTo(vLoyalty, 0)
                    vLoyalty(objBaseparty.ClientDetail.LoyaltyScheme.Length) = objloyalty
                Else
                    ReDim vLoyalty(0)
                    vLoyalty(0) = objloyalty
                End If
                objBaseparty.ClientDetail.LoyaltyScheme = vLoyalty
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            Else
                'Dim objBaseparty As New BasePartyPCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                Dim objloyalty As New BaseClientSharedDataTypeLoyaltyScheme
                objloyalty.LoyaltySchemeCode = ddlLoyaltySchemes.SelectedValue
                objloyalty.MembershipNumber = txtmembership.Text
                objloyalty.OtherReference = txtotherref.Text
                If (IsDate(txtstart.Text)) Then
                    objloyalty.StartDate = txtstart.Text
                Else
                    objloyalty.StartDate = SqlDateTime.Null
                End If
                If (IsDate(txtend.Text)) Then
                    objloyalty.EndDate = txtend.Text
                    objloyalty.EndDateSpecified = True
                Else
                    objloyalty.EndDate = SqlDateTime.Null
                End If

                objloyalty.MainMember = txtMain.Text
                If chkActive.Checked = True Then
                    objloyalty.Active = True
                    objloyalty.ActiveSpecified = True
                Else
                    objloyalty.Active = False
                End If

                objloyalty.LoyaltySchemeKey = gvloyalty.SelectedRow.Cells(7).Text
                objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex) = objloyalty
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            End If

            gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
            gvloyalty.DataBind()
            pnlAddLoyaltySchemes.Visible = False
            gvloyalty.SelectedIndex = -1
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnPolicyOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPolicyOk.Click
        Try
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")

            Dim objBaseparty As New Object
            objBaseparty = setobject()

            'For add
            If gvpolicies.SelectedIndex = -1 Then

                Dim objProspect As New BaseClientSharedDataTypeProspectPolicies
                objProspect.ProspectTypeCode = ddlprostype.SelectedValue.ToString()
                If (IsDate(txtprewnal.Text)) Then
                    objProspect.RenewalDate = txtprewnal.Text
                Else
                    objProspect.RenewalDate = SqlDateTime.Null
                End If
                If (String.IsNullOrEmpty(txttimequoted.Text)) Then
                    objProspect.TimesQuotedSpecified = False
                Else
                    objProspect.TimesQuoted = txttimequoted.Text
                    objProspect.TimesQuotedSpecified = True
                End If
                objProspect.TargetPremium = txttargetpremium.Text
                Dim vProspect() As BaseClientSharedDataTypeProspectPolicies
                If objBaseparty.ClientDetail.ProspectPolicies IsNot Nothing Then
                    ReDim vProspect(objBaseparty.ClientDetail.ProspectPolicies.Length)
                    objBaseparty.ClientDetail.ProspectPolicies.CopyTo(vProspect, 0)
                    vProspect(objBaseparty.ClientDetail.ProspectPolicies.Length) = objProspect
                Else
                    ReDim vProspect(0)
                    vProspect(0) = objProspect
                End If

                objBaseparty.ClientDetail.ProspectPolicies = vProspect
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            Else
                'for update
                Dim objProspect As New BaseClientSharedDataTypeProspectPolicies
                objProspect.ProspectTypeCode = ddlprostype.SelectedValue.ToString()
                If (IsDate(txtprewnal.Text)) Then
                    objProspect.RenewalDate = txtprewnal.Text
                    objProspect.RenewalDateSpecified = True
                Else
                    objProspect.RenewalDate = SqlDateTime.Null
                End If
                If txttimequoted.Text <> "" Then
                    objProspect.TimesQuotedSpecified = True
                End If

                objProspect.TimesQuoted = txttimequoted.Text

                If txttargetpremium.Text <> "" Then
                    objProspect.TargetPremiumSpecified = True
                End If

                objProspect.TargetPremium = txttargetpremium.Text

                objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex) = objProspect

                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = oGetPartyResponseType
            End If
            gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
            gvpolicies.DataBind()
            pnlAddPolicies.Visible = False
            ' SAMHelper.setTextFieldsInGridView(gvpolicies)
            gvpolicies.SelectedIndex = -1
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
    End Sub

    Protected Sub btnAddParty_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddParty.Click
        Try
            Dim objBaseparty As Object
            objBaseparty = Setvalues()
            Dim oAddPartyRequestType As New AddPartyRequestType
            Dim oAddPartyResponseType As New AddPartyResponseType
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")

            'set up request object with some values
            With oAddPartyRequestType
                .BranchCode = "HeadOff"
                .Item = objBaseparty

            End With

            Dim oSAM As New SAMForInsuranceV2
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            oAddPartyResponseType = oSAM.AddParty(oAddPartyRequestType)

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try

    End Sub

    Public Function Setvalues() As Object

        Dim oGetPartyResponseType As New GetPartyResponseType

        oGetPartyResponseType = Session("GetPartyResponse")
        Try

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                Dim objBaseparty As New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                '''''''''''''''''''''''''''''''''Identity'''''''''''''''''''''''''''''
                objBaseparty.Surname = txtLastName.Text
                objBaseparty.Forename = txtForeName.Text
                If (ddlTitle.SelectedIndex > 0) Then
                    objBaseparty.Title = ddlTitle.SelectedValue
                End If
                objBaseparty.Initials = txtInitial.Text
                objBaseparty.TradingName = txtTradingName.Text
                objBaseparty.ClientDetail.IsProspectSpecified = True
                If chkIsprospect.Checked = True Then
                    objBaseparty.ClientDetail.IsProspect = True
                Else
                    objBaseparty.ClientDetail.IsProspect = False
                End If
                objBaseparty.ClientDetail.IsAgentSpecified = True
                If chkIsagent.Checked = True Then
                    objBaseparty.ClientDetail.IsAgent = True
                Else
                    objBaseparty.ClientDetail.IsAgent = False
                End If
                objBaseparty.AlternativeId = txtAlternativeIdentifier.Text
                If ddServiceLevel.SelectedIndex > 0 Then
                    objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
                End If
                If (ddBranch.SelectedIndex > 0) Then
                    objBaseparty.BranchCode = ddBranch.SelectedValue
                End If
                If (ddSubBranch.SelectedIndex > 0) Then
                    objBaseparty.SubBranchCode = ddSubBranch.SelectedValue
                End If

                If Not String.IsNullOrEmpty(hdLeadAgent.Value) Then
                    objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
                    objBaseparty.ClientDetail.LeadAgentKeySpecified = True
                Else
                    objBaseparty.ClientDetail.LeadAgentKeySpecified = False
                End If
                If ddlArea.SelectedValue <> 0 Then
                    objBaseparty.ClientDetail.AreaCode = ddlArea.SelectedValue '.SelectedItem.Text
                End If
                objBaseparty.FileCode = txtFileCode.Text

                objBaseparty.AccountExecutiveCode = hdAccExecutiveCode.Value
                objBaseparty.AccountExecutive = txtAccExecutiveName.Text
                ''''''''''''''''''''''''''''''Contacts''''''''''''''''''''''''''''''''''
                objBaseparty.Salutation = txtsalutation.Text
                If (ddlPreferedCorres.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.CorrespondenceCode = ddlPreferedCorres.SelectedValue
                End If
                objBaseparty.TPSSpecified = True
                objBaseparty.MPSSpecified = True
                objBaseparty.eMPSSpecified = True
                If chkTPS.Checked = True Then
                    objBaseparty.TPS = True
                Else
                    objBaseparty.TPS = False
                End If

                If chkMPS.Checked = True Then
                    objBaseparty.MPS = True
                Else
                    objBaseparty.MPS = False
                End If

                If chkeMPS.Checked = True Then
                    objBaseparty.eMPS = True
                Else
                    objBaseparty.eMPS = False
                End If
                ''''''''''''''''''''''''''''''''''''Additions'''''''''''''''''''''''''''''''''''''
                objBaseparty.Currency = ddlCurrency.SelectedValue
                If (ddlPaymentMethod.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.ToString
                End If
                If (ddlReminderType.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.ReminderCode = ddlReminderType.SelectedValue
                End If
                If (ddlTermsOfPayment.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.PaymentTermCode = ddlTermsOfPayment.SelectedValue
                End If
                If (ddlrenewaldtopcode.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.RenewalStopCode = ddlrenewaldtopcode.SelectedValue
                End If
                objBaseparty.Source = txtsource.Text
                If (ddlPriOccupation.SelectedIndex > 0) Then
                    objBaseparty.OccupationCode = ddlPriOccupation.SelectedItem.ToString
                End If
                If (ddlPriEmpBusiness.SelectedIndex > 0) Then
                    objBaseparty.EmployersBusinessCode = ddlPriEmpBusiness.SelectedValue
                End If
                If ddlPriStatus.SelectedIndex > 0 Then
                    objBaseparty.EmploymentStatusCodeSpecified = True
                    objBaseparty.EmploymentStatusCode = CType(ddlPriStatus.SelectedIndex - 1, EmploymentStatusCodeType)
                Else
                    objBaseparty.EmploymentStatusCodeSpecified = False
                End If

                'EmploymentStatusCodeType()
                'objBaseparty.MaritalStatusCode = CType(ddlPriStatus.SelectedIndex, EmploymentStatusCodeType)
                If (ddlSecOccupation.SelectedIndex > 0) Then
                    objBaseparty.SecOccupationCode = ddlSecOccupation.SelectedItem.ToString
                End If
                If (ddlSecEmpsBusiness.SelectedIndex > 0) Then
                    objBaseparty.SecEmployersBusinessCode = ddlSecEmpsBusiness.SelectedItem.ToString
                End If
                If ddlSecStatus.SelectedIndex > 0 Then
                    objBaseparty.SecEmploymentStatusCodeSpecified = True
                    objBaseparty.SecEmploymentStatusCode = CType(ddlSecStatus.SelectedIndex - 1, EmploymentStatusCodeType)
                Else
                    objBaseparty.SecEmploymentStatusCodeSpecified = False
                End If

                'Associates
                ''''''''''''''''''''''''''''''''''Lifestyle''''''''''''''''''''''''''''''''''''''
                If IsDate(txtDobirth.Text) Then
                    objBaseparty.DateOfBirth = txtDobirth.Text
                    objBaseparty.DateOfBirthSpecified = True
                Else
                    objBaseparty.DateOfBirth = SqlDateTime.Null
                    objBaseparty.DateOfBirthSpecified = False
                End If

                If ddmaritalstatus.SelectedIndex > 0 Then
                    objBaseparty.MaritalStatusCodeSpecified = True
                    objBaseparty.MaritalStatusCode = CType(ddmaritalstatus.SelectedIndex - 1, MaritalStatusCodeType)
                Else
                    objBaseparty.MaritalStatusCodeSpecified = False
                End If

                If (ddseasonalgift.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
                End If
                objBaseparty.ClientDetail.LoyaltyNumber = txtloyalty.Text

                objBaseparty.PetOwnerSpecified = True
                objBaseparty.PetOwner = chkPets.Checked

                If (ddgender.SelectedIndex > 0) Then
                    objBaseparty.GenderCode = ddgender.SelectedItem.ToString()
                End If
                If (ddnationality.SelectedIndex > 0) Then
                    objBaseparty.NationalityCode = ddnationality.SelectedValue
                End If
                If (ddlAccomodation.SelectedIndex > 0) Then
                    objBaseparty.AccommodationCode = ddlAccomodation.SelectedValue
                End If
                Dim oLifeStyle As BasePartyPCTypeLifestyle
                Dim i As Integer
                For Each oLifeStyle In objBaseparty.Lifestyle
                    If (oLifeStyle.CategoryCode = "INSURED") Then
                        If IsDate(txtDobirth.Text) Then
                            objBaseparty.Lifestyle(i).DateOfBirth = txtDobirth.Text
                            objBaseparty.Lifestyle(i).DateOfBirthSpecified = True
                        Else
                            objBaseparty.Lifestyle(i).DateOfBirth = SqlDateTime.Null
                            objBaseparty.Lifestyle(i).DateOfBirthSpecified = False
                        End If
                        If (ddgender.SelectedValue = "F") Then
                            objBaseparty.Lifestyle(i).GenderCode = GenderCodeType.F
                            objBaseparty.Lifestyle(i).GenderCodeSpecified = True
                        ElseIf (ddgender.SelectedValue = "M") Then
                            objBaseparty.Lifestyle(i).GenderCode = GenderCodeType.M
                            objBaseparty.Lifestyle(i).GenderCodeSpecified = True
                        End If
                        If (ddlPriOccupation.SelectedIndex > 0) Then
                            objBaseparty.Lifestyle(i).OccupationCode = ddlPriOccupation.SelectedItem.ToString
                        End If
                        If (ddlSecOccupation.SelectedIndex > 0) Then
                            objBaseparty.Lifestyle(i).SecOccupationCode = ddlSecOccupation.SelectedItem.ToString
                        End If

                        objBaseparty.Lifestyle(i).Smoker = chksmoker.Checked
                        objBaseparty.Lifestyle(i).SmokerSpecified = True
                    End If
                    i = i + 1
                Next
                '''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''
                objBaseparty.ClientDetail.AgentReference = txtagentref.Text
                If String.IsNullOrEmpty(txtcurrentagent.Text) Then
                    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
                Else
                    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
                    objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
                End If
                'txtcurrentagent.Text
                If (ddlprosStrengthCode.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue.ToString()
                End If
                If (ddlprosStatus.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue.ToString
                End If

                If String.IsNullOrEmpty(txtPIcode.Text) Then
                    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
                Else
                    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
                    objBaseparty.ClientDetail.PreviousInsurerKey = hdPreviousInsurerKey.Value
                End If


                If String.IsNullOrEmpty(txtPBcode.Text) Then
                    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
                Else
                    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
                    objBaseparty.ClientDetail.PreviousBrokerKey = hdPrevBrokerKey.Value
                End If

                txtPBname.Enabled = False
                txtPIname.Enabled = False
                'txtPBname.Text = ""
                'txtPIname.Text = ""
                ''''''''''''''''''''''''''''''Tax''''''''''''''''''''''''''''''''''''''
                objBaseparty.TaxNumber = txttaxno.Text
                objBaseparty.DomiciledForTaxSpecified = True
                If chkDomicileTax.Checked = True Then
                    objBaseparty.DomiciledForTax = True
                Else
                    objBaseparty.DomiciledForTax = False
                End If
                objBaseparty.TaxExemptSpecified = True
                objBaseparty.TaxExempt = chkTaxExempt.Checked
                If txtpercentage.Text = "" Then
                    objBaseparty.TaxPercentageSpecified = False
                Else
                    objBaseparty.TaxPercentageSpecified = True
                End If
                objBaseparty.TaxPercentage = txtpercentage.Text
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                Return objBaseparty
            End If

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then

                Dim objBaseparty As New BasePartyOTHERType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)

                objBaseparty.Currency = ddlCurrency.SelectedValue

                If (IsDate(txtDobirth.Text)) Then
                    objBaseparty.DateOfBirth = txtDobirth.Text
                Else
                    objBaseparty.DateOfBirth = SqlDateTime.Null
                End If
                txtPIname.Text = ""
                txtPIname.Enabled = False
                txtPBname.Text = ""
                txtPBname.Enabled = False

                objBaseparty.TaxNumber = txttaxno.Text
                objBaseparty.DomiciledForTaxSpecified = True
                If chkDomicileTax.Checked = True Then
                    objBaseparty.DomiciledForTax = True
                Else
                    objBaseparty.DomiciledForTax = False
                End If

                objBaseparty.TaxExemptSpecified = True
                objBaseparty.TaxExempt = chkTaxExempt.Checked

                If String.IsNullOrEmpty(txtpercentage.Text) Then
                    objBaseparty.TaxPercentageSpecified = False
                Else
                    objBaseparty.TaxPercentageSpecified = True
                    objBaseparty.TaxPercentage = txtpercentage.Text
                End If

                Return objBaseparty
            End If
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then

                Dim objBaseparty As BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
                '''''''''''''''''''''Identity''''''''''''''''''''''''''''''
                objBaseparty.CompanyName = txtTradingName.Text
                objBaseparty.MainContact = txtMainContact.Text
                objBaseparty.CompanyReg = txtCompanyReg.Text
                If (objBaseparty.ClientDetail Is Nothing) Then
                    objBaseparty.ClientDetail = New BaseClientSharedDataType
                End If
                objBaseparty.ClientDetail.IsAgent = chkIsagent.Checked
                objBaseparty.ClientDetail.IsAgentSpecified = True
                objBaseparty.ClientDetail.IsProspect = chkIsprospect.Checked
                objBaseparty.ClientDetail.IsProspectSpecified = True
                objBaseparty.AlternativeId = txtAlternativeIdentifier.Text
                If ddServiceLevel.SelectedIndex > 0 Then
                    objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
                End If
                If (ddBranch.SelectedIndex > 0) Then
                    objBaseparty.BranchCode = ddBranch.SelectedValue
                End If
                If (ddSubBranch.SelectedIndex > 0) Then
                    objBaseparty.SubBranchCode = ddSubBranch.SelectedValue
                End If
                If (ddlBusiness.SelectedIndex > 0) Then
                    objBaseparty.BusinessCode = ddlBusiness.SelectedItem.Text
                End If
                If (ddlTrade.SelectedIndex > 0) Then
                    objBaseparty.TradeCode = ddlTrade.SelectedItem.Text
                End If
                If (ddlSICcode.SelectedIndex > 0) Then
                    objBaseparty.SICCode = ddlSICcode.SelectedValue
                End If
                If (IsDate(txtTradingSince.Text)) Then
                    objBaseparty.TradingSince = txtTradingSince.Text
                    objBaseparty.TradingSinceSpecified = True
                Else
                    objBaseparty.TradingSinceSpecified = False
                End If
                If Not (String.IsNullOrEmpty(txtNoOfOffices.Text)) Then
                    objBaseparty.NumberOfOffices = txtNoOfOffices.Text
                    objBaseparty.NumberOfOfficesSpecified = True
                Else
                    objBaseparty.NumberOfOfficesSpecified = False
                End If
                If (ddlNoOfEmployees.SelectedIndex > 0) Then
                    objBaseparty.NumberOfEmployees = ddlNoOfEmployees.SelectedValue

                End If

                'objBaseparty.AccountExecutiveCode = txtAccExecutiveCode.Text
                If Not String.IsNullOrEmpty(hdLeadAgent.Value) Then
                    objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
                    objBaseparty.ClientDetail.LeadAgentKeySpecified = True
                Else
                    objBaseparty.ClientDetail.LeadAgentKeySpecified = False
                End If

                objBaseparty.AccountExecutiveCode = hdAccExecutiveCode.Value
                objBaseparty.AccountExecutive = txtAccExecutiveName.Text
                '''''''''''''''''''''''''''''''''''Address''''''''''''''''''''''''''''''''''
                objBaseparty.Salutation = txtsalutation.Text
                objBaseparty.TPSSpecified = True
                objBaseparty.MPSSpecified = True
                objBaseparty.eMPSSpecified = True
                If chkTPS.Checked = True Then
                    objBaseparty.TPS = True
                Else
                    objBaseparty.TPS = False
                End If

                If chkMPS.Checked = True Then
                    objBaseparty.MPS = True
                Else
                    objBaseparty.MPS = False
                End If

                If chkeMPS.Checked = True Then
                    objBaseparty.eMPS = True
                Else
                    objBaseparty.eMPS = False
                End If
                If (ddlPreferedCorres.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.CorrespondenceCode = ddlPreferedCorres.SelectedValue
                End If
                ''''''''''''''''''''''''''''''Additions'''''''''''''''''''''''''''''''
                If (ddlCurrency.SelectedIndex > 0) Then
                    objBaseparty.Currency = ddlCurrency.SelectedValue
                End If
                If (ddlPaymentMethod.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.ToString()
                End If
                If (ddlReminderType.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.ReminderCode = ddlReminderType.SelectedValue
                End If
                If (ddlTermsOfPayment.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.PaymentTermCode = ddlTermsOfPayment.SelectedValue
                End If
                If (ddlrenewaldtopcode.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.RenewalStopCode = ddlrenewaldtopcode.SelectedValue
                End If
                objBaseparty.Source = txtsource.Text
                If (ddseasonalgift.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
                End If
                objBaseparty.ClientDetail.LoyaltyNumber = txtLoyaltyNumber.Text
                objBaseparty.WageRoll = txtwage.Text
                If (ddlturnover.SelectedIndex > 0) Then
                    objBaseparty.TurnoverCode = ddlturnover.SelectedValue
                End If
                If IsDate(txtfinancial.Text) Then
                    objBaseparty.FinancialYear = Convert.ToDateTime(txtfinancial.Text)
                    objBaseparty.FinancialYearSpecified = True
                Else
                    objBaseparty.FinancialYear = DateTime.MinValue
                End If
                If (ddlAdditionsArea.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.AreaCode = ddlAdditionsArea.SelectedValue
                End If
                objBaseparty.FileCode = txtAdditionsFileCode.Text
                ''''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''''''''''''''''''''''
                objBaseparty.ClientDetail.AgentReference = txtagentref.Text
                If (ddlprosStrengthCode.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue
                End If
                If (ddlprosStatus.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue
                End If

                If String.IsNullOrEmpty(txtcurrentagent.Text) Then
                    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
                Else
                    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
                    objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
                End If

                If String.IsNullOrEmpty(txtPIcode.Text) Then
                    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
                Else
                    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
                    objBaseparty.ClientDetail.PreviousInsurerKey = hdPreviousInsurerKey.Value
                End If

                If String.IsNullOrEmpty(txtPBcode.Text) Then
                    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
                Else
                    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
                    objBaseparty.ClientDetail.PreviousBrokerKey = hdPrevBrokerKey.Value
                End If

                txtPIname.Enabled = False
                txtPBname.Enabled = False
                '''''''''''''''''''''''''''''''''''''Tax''''''''''''''''''''''''''''''''''''''
                objBaseparty.TaxNumber = txttaxno.Text
                objBaseparty.DomiciledForTaxSpecified = True
                objBaseparty.DomiciledForTax = chkDomicileTax.Checked
                objBaseparty.TaxExemptSpecified = True
                objBaseparty.TaxExempt = chkTaxExempt.Checked
                If txtpercentage.Text = "" Then
                    objBaseparty.TaxPercentageSpecified = False
                Else
                    objBaseparty.TaxPercentageSpecified = True
                End If
                objBaseparty.TaxPercentage = txtpercentage.Text
                '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                Return objBaseparty
            End If
        Catch ex As Exception

        End Try
    End Function

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String, ByVal BindValue As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = STSListType.PMLookup
        oRequest.ListCode = ListCode
        oRequest.ExcludeDeletedRecords = True

        Try
            oResponse = oSAM.GetList(oRequest)

            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Code"
                    objControl.DataBind()
                    If Not (BindValue = "") Then
                        objControl.SelectedValue = BindValue
                    End If
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub

    Protected Sub GVContacts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVContacts.SelectedIndexChanged
        Try
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")

            txtstname.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine1
            txtposttown.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine3
            txtpostcode.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).PostCode
            txtlocality.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine2
            txtcounty.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).AddressLine4
            'txtcountry.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).CountryCode
            ddlconCountry.SelectedValue = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).CountryCode

            pnlAddAddress.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Public Function setobject() As Object
        Try
            Dim oGetPartyResponseType As New GetPartyResponseType

            oGetPartyResponseType = Session("GetPartyResponse")

            Dim objBaseparty As Object

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                objBaseparty = New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
            End If

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                objBaseparty = New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
            End If
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then
                objBaseparty = New BasePartyOTHERType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)

            End If
            Return objBaseparty

        Catch ex As Exception

        End Try
    End Function

    Protected Sub btnccancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnccancel.Click
        plnAddContact.Visible = False
        GVContacts1.SelectedIndex = -1
    End Sub

    Protected Sub btnacancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnacancel.Click
        pnlAddAddress.Visible = False
        GVContacts.SelectedIndex = -1
    End Sub


    Protected Sub gvassociates_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvassociates.SelectedIndexChanged
        Dim oGetPartyResponseType As New GetPartyResponseType

        oGetPartyResponseType = Session("GetPartyResponse")
        Try
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                        oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                Dim objBaseparty As New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

                txtclient.Text = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName
                hdAssociateKey.Value = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey

                ddlrelationshipcode.SelectedValue = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode.Trim
                Session("PartyKey") = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey
            End If
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                                oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                Dim objBaseparty As New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)

                txtclient.Text = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName
                hdAssociateKey.Value = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey

                ddlrelationshipcode.SelectedValue = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode.Trim
                Session("PartyKey") = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey

            End If

            pnlNewAssociates.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GVContacts1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVContacts1.SelectedIndexChanged

        Try
            txtareacode.Text = GVContacts1.SelectedRow.Cells(1).Text
            txtdescription.Text = GVContacts1.SelectedRow.Cells(4).Text
            txtNumber.Text = GVContacts1.SelectedRow.Cells(2).Text
            plnAddContact.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvConcictions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvConcictions.SelectedIndexChanged
        Dim oGetPartyResponseType As New GetPartyResponseType

        oGetPartyResponseType = Session("GetPartyResponse")
        Try
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                        oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                Dim objBaseparty As New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).TypeCode) Then
                    ddlconvictiontype.ClearSelection()
                    ddlconvictiontype.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).TypeCode).Selected = True
                End If
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).StatusCode) Then
                    ddlconvictionstatus.ClearSelection()
                    ddlconvictionstatus.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).StatusCode).Selected = True
                End If
                txtcdescription.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).Description
                txtcfine.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).FineAmount
                txtcdate.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).Date

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceTypeCode) Then
                    ddlsentencetype.ClearSelection()
                    ddlsentencetype.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceTypeCode).Selected = True
                End If
                txtsdescription.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDescription
                If objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceEffectiveDateSpecified Then
                    txtsdate.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceEffectiveDate
                Else
                    txtsdate.Text = ""
                End If
                txtsduration.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDuration
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDurationQualifier) Then
                    ddlsentencetime.ClearSelection()
                    ddlsentencetime.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDurationQualifier).Selected = True
                End If

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholMeasurementMethod) Then
                    ddlAlcoholmethod.ClearSelection()
                    ddlAlcoholmethod.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholMeasurementMethod).Selected = True
                End If

                If objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholLevelSpecified Then
                    txtalcohollevel.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholLevel
                Else
                    txtalcohollevel.Text = ""
                End If
                If objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).DrivingLicensePenaltyPointsSpecified Then
                    txtpenality.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).DrivingLicensePenaltyPoints
                Else
                    txtpenality.Text = ""
                End If
            End If

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                                oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                Dim objBaseparty As New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).TypeCode) Then
                    ddlconvictiontype.ClearSelection()
                    ddlconvictiontype.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).TypeCode).Selected = True
                End If
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).StatusCode) Then
                    ddlconvictionstatus.ClearSelection()
                    ddlconvictionstatus.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).StatusCode).Selected = True
                End If
                'ddlconvictiontype.SelectedValue = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).TypeCode
                'ddlconvictionstatus.SelectedValue = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).StatusCode
                txtcdescription.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).Description
                txtcfine.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).FineAmount

                txtcdate.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).Date
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceTypeCode) Then
                    ddlsentencetype.ClearSelection()
                    ddlsentencetype.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceTypeCode).Selected = True
                End If
                'ddlsentencetype.SelectedValue = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceTypeCode
                txtsdescription.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDescription
                If objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceEffectiveDateSpecified Then
                    txtsdate.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceEffectiveDate
                Else
                    txtsdate.Text = ""
                End If

                txtsduration.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDuration
                'ddlsentencetime.SelectedValue = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDurationQualifier
                'ddlAlcoholmethod.SelectedValue = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholMeasurementMethod
                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDurationQualifier) Then
                    ddlsentencetime.ClearSelection()
                    ddlsentencetime.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).SentenceDurationQualifier).Selected = True
                End If

                If Not String.IsNullOrEmpty(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholMeasurementMethod) Then
                    ddlAlcoholmethod.ClearSelection()
                    ddlAlcoholmethod.Items.FindByText(objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholMeasurementMethod).Selected = True
                End If
                If objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholLevelSpecified Then
                    txtalcohollevel.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).AlcoholLevel
                Else
                    txtalcohollevel.Text = ""
                End If
                If objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).DrivingLicensePenaltyPointsSpecified Then
                    txtpenality.Text = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex).DrivingLicensePenaltyPoints
                Else
                    txtpenality.Text = ""
                End If
            End If
            pnlAddConvictions.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvloyalty_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvloyalty.SelectedIndexChanged
        Dim oGetPartyResponseType As New GetPartyResponseType

        oGetPartyResponseType = Session("GetPartyResponse")
        Try
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                Dim objBaseparty As New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
                ddlLoyaltySchemes.SelectedValue = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).LoyaltySchemeCode
                txtmembership.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).MembershipNumber
                txtotherref.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).OtherReference
                txtstart.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).StartDate

                If objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).EndDateSpecified Then
                    txtend.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).EndDate
                Else
                    txtend.Text = ""
                End If

                txtMain.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).MainMember

                If objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).ActiveSpecified Then
                    chkActive.Checked = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).Active
                End If
            Else
                Dim objBaseparty As New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                ddlLoyaltySchemes.SelectedValue = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).LoyaltySchemeCode
                txtmembership.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).MembershipNumber
                txtotherref.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).OtherReference
                txtstart.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).StartDate

                If objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).EndDateSpecified Then
                    txtend.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).EndDate
                Else
                    txtend.Text = ""
                End If

                txtMain.Text = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).MainMember

                If objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).ActiveSpecified Then
                    chkActive.Checked = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).Active
                End If
            End If
            pnlAddLoyaltySchemes.Visible = True
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvpolicies_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvpolicies.SelectedIndexChanged
        Dim oGetPartyResponseType As New GetPartyResponseType

        oGetPartyResponseType = Session("GetPartyResponse")
        Try

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                Dim objBaseparty As New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
                ddlprostype.SelectedValue = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).ProspectTypeCode

                If objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).RenewalDateSpecified Then
                    txtprewnal.Text = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).RenewalDate
                End If

                If objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TimesQuotedSpecified Then
                    txttimequoted.Text = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TimesQuoted
                End If
                If objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TargetPremiumSpecified Then
                    txttargetpremium.Text = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TargetPremium
                End If
            Else
                Dim objBaseparty As New BasePartyPCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                ddlprostype.SelectedValue = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).ProspectTypeCode

                If objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).RenewalDateSpecified Then
                    txtprewnal.Text = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).RenewalDate
                End If

                If objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TimesQuotedSpecified Then
                    txttimequoted.Text = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TimesQuoted
                End If
                If objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TargetPremiumSpecified Then
                    txttargetpremium.Text = objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.SelectedIndex).TargetPremium
                End If
            End If
            pnlAddPolicies.Visible = True

        Catch ex As Exception

        End Try
    End Sub
End Class
