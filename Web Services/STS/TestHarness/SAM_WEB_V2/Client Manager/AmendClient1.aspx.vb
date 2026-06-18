Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data.SqlTypes
Imports System.Data
Partial Class Client_Manager_AmendClient1
    Inherits System.Web.UI.Page
    Dim strClient As String
    Dim objBaseparty As New Object
    Dim oGetPartyResponseType As New GetPartyResponseType
    Dim oAddPartyRequest As New AddPartyRequestType
    Dim objitem As New BasePartyType

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        strClient = Session("CLIENTTYPE")
        If Session("CLIENTTYPE") = "CC" Then
            Menu1.Items(4).Text = "5 - Address"
        End If

        Try
            If Not (IsPostBack) Then
                Session("GetPartyResponse") = Nothing

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
                ddBranch.SelectedIndex = "6"
                ddSubBranch.SelectedIndex = "0"
                ddlCurrency.SelectedIndex = "0"

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
                                ddlPriOccupation.DataValueField = "Code"
                                ddlPriOccupation.DataBind()

                                ddlSecOccupation.DataSource = oResponse.List
                                ddlSecOccupation.DataTextField = "Description"
                                ddlSecOccupation.DataValueField = "Code"
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
                                lstitem.Text = "--Select--"
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
                                ddlNoOfEmployees.DataValueField = "Code"
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
                                ddloccupationcode.DataValueField = "code"
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
                                ddlsecoccupationcode.DataValueField = "code"
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
                    If strClient = "PC" Then
                        GVContacts.Visible = True

                        'txtgrname.Enabled = False
                        'txtgrtype.Enabled = False
                        txtwage.Enabled = False
                        'txtturnover.Enabled = False
                        ddlturnover.Enabled = False
                        txtfinancial.Enabled = False



                        'lblArea.Visible = True
                        ddlArea.Visible = True
                        'lblFileCode.Visible = True
                        txtFileCode.Visible = True

                        'lblBusiness.Visible = False
                        ddlBusiness.Visible = False
                        'lblTrade.Visible = False
                        ddlTrade.Visible = False
                        'lblSICcode.Visible = False
                        'ddlSic.Visible = False
                        'lblNoofEmployees.Visible = False
                        ddlNoOfEmployees.Visible = False
                        'lblNoOfOffices.Visible = False
                        txtNoOfOffices.Visible = False
                        'lblTradeSince.Visible = False
                        'txtTradeSince.Visible = False
                        'lblMainContact.Visible = False
                        txtMainContact.Visible = False

                        'pnlCharityDetails.Visible = False
                        'lblCharityDetails.Visible = False

                        'txtCompanyName.Visible = False
                        'lblCompanyName.Visible = False

                        txtMainContact.Visible = False
                        'lblMainContact.Visible = False
                        txtAccbalance.Text = ""
                        txtYearTodateTurnOver.Text = ""
                        txtLastYearturnOver.Text = ""

                        'txtgrname.Visible = False
                        'lblGroupName.Visible = False
                        'txtgrtype.Visible = False
                        'lblGroupType.Visible = False
                        'txtPIname.Text = ""

                        'txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerKey
                        'txtPBname.Text = ""
                    End If
                End If
                txtLeadAgentCode.Text = objBaseparty.ClientDetail.LeadAgentCode
                txtLeadAgentName.Text = objBaseparty.ClientDetail.LeadAgentName
                If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                    hdLeadAgent.Value = objBaseparty.ClientDetail.LeadAgentKey
                End If

                txtAccExecutiveCode.Text = objBaseparty.AccountExecutiveCode
                hdAccExecutiveCode.Value = objBaseparty.AccountExecutiveCode

                txtAccExecutiveName.Text = objBaseparty.AccountExecutive


                txtPIcode.Text = objBaseparty.ClientDetail.PreviousInsurerCode
                txtPIname.Text = objBaseparty.ClientDetail.PreviousInsurerName
                txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerCode
                txtPBname.Text = objBaseparty.ClientDetail.PreviousBrokerName

                hdCurrentAgentKey.Value = objBaseparty.ClientDetail.CurrentIntermediaryKey
                hdPreviousInsurerKey.Value = objBaseparty.ClientDetail.PreviousInsurerKey
                hdPrevBrokerKey.Value = objBaseparty.ClientDetail.PreviousBrokerKey

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

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        Mv.ActiveViewIndex = Int32.Parse(e.Item.Value)
        If Session("CLIENTTYPE") = "CC" And Int32.Parse(e.Item.Value) = 4 Then
            Mv.ActiveViewIndex = 1
        End If
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
        ddlAdditionsArea.Items.Insert(0, "--Select--")
        'ddseasonalgift.Items.Insert(0, "--Select--")
        ddlNoOfEmployees.Items.Insert(0, "--Select--")
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
            Dim objBaseparty As New BasePartyPCType
            objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
            objBaseparty = Session("GetPartyResponse")


            'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
            '            oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
            '    Dim objBaseparty As New BasePartyPCType

            'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

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
            ddloccupationcode.SelectedItem.Text = objBaseparty.Lifestyle(gvDependents.SelectedIndex).OccupationCode
            ddlsecoccupationcode.SelectedItem.Text = objBaseparty.Lifestyle(gvDependents.SelectedIndex).SecOccupationCode
            If objBaseparty.Lifestyle(gvDependents.SelectedIndex).SmokerSpecified Then
                chkIssmoker.Checked = objBaseparty.Lifestyle(gvDependents.SelectedIndex).Smoker
            End If
            ' End If
            pnlAddDependents.Visible = True

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnLSOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLSOk.Click
        Try
            Dim objBaseparty As New Object
            If objBaseparty Is Nothing Then
                objBaseparty = setobject()
            End If
            'objBaseparty = setobject()
            Dim oGetPartyResponseType As New GetPartyResponseType

            objBaseparty = Session("GetPartyResponse")

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
                objLS.OccupationCode = ddloccupationcode.SelectedItem.Text
                objLS.SecOccupationCode = ddlsecoccupationcode.SelectedItem.Text

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
                Session("GetPartyResponse") = objBaseparty
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

                objBaseparty.Lifestyle(gvDependents.SelectedIndex).OccupationCode = ddloccupationcode.SelectedValue
                objBaseparty.Lifestyle(gvDependents.SelectedIndex).SecOccupationCode = ddlsecoccupationcode.SelectedValue

                If chkIssmoker.Checked = True Then
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).Smoker = True
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).SmokerSpecified = True
                Else
                    objBaseparty.Lifestyle(gvDependents.SelectedIndex).Smoker = False
                End If

                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = objBaseparty
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

    Protected Sub btnAddLoyaltyAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLoyaltyAdd.Click
        Try
            If gvloyalty.SelectedIndex = -1 Then
                Dim oGetPartyResponseType As New GetPartyResponseType
                Dim objClientDetail As New BaseClientSharedDataType

                objBaseparty = Session("GetPartyResponse")
                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()

                End If
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If

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
                Session("GetPartyResponse") = objBaseparty
            Else
                objBaseparty = Session("GetPartyResponse")
                'Dim objBaseparty As New BasePartyPCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                Dim objClientDetail As New BaseClientSharedDataType
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If
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
                Session("GetPartyResponse") = objBaseparty
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
                Dim objClientDetail As New BaseClientSharedDataType
                objBaseparty = Session("GetPartyResponse")

                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If

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
                Session("GetPartyResponse") = objBaseparty
                pnlNewAssociates.Visible = False

            Else
                Dim oGetPartyResponseType As New GetPartyResponseType
                objBaseparty = Session("GetPartyResponse")
                Dim objClientDetail As New BaseClientSharedDataType
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If

                ' If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                ' oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                'Dim objBaseparty As New BasePartyPCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
                objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName = txtclient.Text
                objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey = Convert.ToInt32(hdAssociateKey.Value)
                objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipDescription = ddlrelationshipcode.Text
                objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode = ddlrelationshipcode.SelectedValue
                objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey = Session("PartyKey")
                oGetPartyResponseType.Item = objBaseparty
                gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                gvassociates.DataBind()
                ' End If
                'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                '                    oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
                '    Dim objBaseparty As New BasePartyCCType
                '    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)

                '    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName = txtclient.Text
                '    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey = Convert.ToInt32(hdAssociateKey.Value)
                '    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipDescription = ddlrelationshipcode.Text
                '    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode = ddlrelationshipcode.SelectedValue
                '    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey = Session("PartyKey")
                '    gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                '    gvassociates.DataBind()
                '    oGetPartyResponseType.Item = objBaseparty
                'End If
                Session("GetPartyResponse") = objBaseparty
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

    Protected Sub GVContacts_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVContacts.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseAddressWithContactsType)(objBaseparty.Addresses, e.RowIndex)
            GVContacts.DataSource = objBaseparty.Addresses
            GVContacts.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            'SAMHelper.setTextFieldsInGridView(GVContacts)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GVContacts1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVContacts1.RowDeleting
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseContactType)(objBaseparty.Contacts, e.RowIndex)
            GVContacts1.DataSource = objBaseparty.Contacts
            GVContacts1.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            ' SAMHelper.setTextFieldsInGridView(GVContacts1)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvassociates_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvassociates.RowDeleting
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseAssociateType)(objBaseparty.ClientDetail.Associates, e.RowIndex)
            gvassociates.DataSource = objBaseparty.ClientDetail.Associates
            gvassociates.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            'SAMHelper.setTextFieldsInGridView(gvassociates)

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvConcictions_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvConcictions.RowDeleting
        Try
            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseConvictionType)(objBaseparty.ClientDetail.Convictions, e.RowIndex)
            gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
            gvConcictions.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            ' SAMHelper.setTextFieldsInGridView(gvConcictions)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvDependents_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDependents.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BasePartyPCTypeLifestyle)(objBaseparty.Lifestyle, e.RowIndex)
            gvDependents.DataSource = objBaseparty.Lifestyle
            gvDependents.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            'SAMHelper.setTextFieldsInGridView(gvDependents)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvloyalty_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvloyalty.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseClientSharedDataTypeLoyaltyScheme)(objBaseparty.ClientDetail.LoyaltyScheme, e.RowIndex)
            gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
            gvloyalty.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            ' SAMHelper.setTextFieldsInGridView(gvloyalty)
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvpolicies_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvpolicies.RowDeleting
        Try

            Dim objBaseparty As New Object
            objBaseparty = setobject()
            'Dim oGetPartyResponseType As New GetPartyResponseType
            objBaseparty = Session("GetPartyResponse")

            SAMHelper.RemoveFromArray(Of BaseClientSharedDataTypeProspectPolicies)(objBaseparty.ClientDetail.ProspectPolicies, e.RowIndex)
            gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
            gvpolicies.DataBind()

            oGetPartyResponseType.Item = objBaseparty
            Session("GetPartyResponse") = objBaseparty
            'SAMHelper.setTextFieldsInGridView(gvpolicies)

        Catch ex As Exception

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
            objBaseparty = Session("GetPartyResponse")

            If (GVContacts.SelectedIndex = -1) Then

                Dim oGetPartyResponseType As New GetPartyResponseType

                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If

                Dim objaddr As New BaseAddressWithContactsType
                'objaddr.AddressTypeCode = txttype.Text
                objaddr.AddressTypeCode = AddressTypeType.Item3131XCO

                'objaddr.AddressTypeCode = CType(ddlconType.SelectedIndex, AddressTypeType)

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
                'Session("GetPartyResponse") = oGetPartyResponseType
                Session("GetPartyResponse") = objBaseparty

                GVContacts.DataSource = objBaseparty.Addresses
                GVContacts.DataBind()
                pnlAddAddress.Visible = False
                ' SAMHelper.setTextFieldsInGridView(GVContacts)
            Else
                Dim oGetPartyResponseType As New GetPartyResponseType

                objBaseparty = Session("GetPartyResponse")
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressTypeCode = AddressTypeType.Item3131XCO
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine4 = txtcounty.Text
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine1 = txtstname.Text
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine3 = txtposttown.Text
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).PostCode = txtpostcode.Text
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine2 = txtlocality.Text
                'DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex). = txtcounty.Text
                DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).CountryCode = ddlconCountry.SelectedValue.ToString

                pnlAddAddress.Visible = False
                Session("GetPartyResponse") = oGetPartyResponseType
                GVContacts.DataSource = objBaseparty.Addresses
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

            objBaseparty = Session("GetPartyResponse")
            If GVContacts1.SelectedIndex = -1 Then

                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If

                Dim objcontact As New BaseContactType
                Dim obj As BaseContactDetailType

                obj = New BaseContactDetailType
                'obj.Item = txtcontact.Text
                obj.Item = txtNumber.Text
                obj.ItemElementName = ItemChoiceType.Number

                objcontact.ContactDetail = obj
                objcontact.AreaCode = txtareacode.Text
                objcontact.ContactTypeCode = ContactTypeType.HOMEPHONE
                objcontact.Description = txtdescription.Text
                objcontact.Extension = txtextension.Text

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
                        drContacts("Extension") = oContactType.Extension
                        drContacts("Type") = oContactType.ContactTypeCode
                        drContacts("Description") = oContactType.Description
                        dtContacts.Rows.Add(drContacts)
                    Next
                End If
                GVContacts1.DataSource = dtContacts
                GVContacts1.DataBind()
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = objBaseparty
            Else
                Dim objBaseparty As New Object
                objBaseparty = setobject()
                If objBaseparty.Contacts IsNot Nothing Then

                    If (objBaseparty.Contacts(GVContacts1.SelectedIndex).ContactTypeCode = ContactTypeType.MAIN) Then
                        ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", "<script>alert ('You Cannot Edit Contact of Type Main !!!')</script>")
                        plnAddContact.Visible = False
                        GVContacts1.SelectedIndex = -1
                        Exit Sub
                    End If
                End If


                Dim objcontact As New BaseContactType
                Dim obj As BaseContactDetailType

                obj = New BaseContactDetailType
                obj.Item = txtNumber.Text
                obj.ItemElementName = ItemChoiceType.Number

                objcontact.ContactDetail = obj
                objcontact.AreaCode = txtareacode.Text
                objcontact.ContactTypeCode = ContactTypeType.HOMEPHONE
                objcontact.Description = txtdescription.Text
                objcontact.Extension = txtextension.Text

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
                        drContacts("Extension") = oContactType.Extension
                        drContacts("Type") = oContactType.ContactTypeCode
                        drContacts("Description") = oContactType.Description
                        dtContacts.Rows.Add(drContacts)
                    Next
                End If
                GVContacts1.DataSource = dtContacts
                GVContacts1.DataBind()
                oGetPartyResponseType.Item = objBaseparty
                Session("GetPartyResponse") = objBaseparty
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
            Dim objClientDetail As New BaseClientSharedDataType


            objBaseparty = Session("GetPartyResponse")


            If gvConcictions.SelectedIndex = -1 Then
                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If
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
                    Session("GetPartyResponse") = objBaseparty
                End If
            Else
                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If

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
                Session("GetPartyResponse") = objBaseparty

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

    Protected Sub btnPolicyOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPolicyOk.Click
        Try


            'For add
            If gvpolicies.SelectedIndex = -1 Then
                Dim oGetPartyResponseType As New GetPartyResponseType
                Dim objClientDetail As New BaseClientSharedDataType

                objBaseparty = Session("GetPartyResponse")

                'Dim objBaseparty As New Object
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If
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
                Session("GetPartyResponse") = objBaseparty
            Else
                'for update
                Dim objProspect As New BaseClientSharedDataTypeProspectPolicies
                Dim objClientDetail As New BaseClientSharedDataType
                objBaseparty = Session("GetPartyResponse")
                If objBaseparty.ClientDetail Is Nothing Then
                    objBaseparty.ClientDetail = objClientDetail
                End If

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
                Session("GetPartyResponse") = objBaseparty
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

    Public Function Setvalues() As Object

        'Dim oGetPartyResponseType As New GetPartyResponseType


        objBaseparty = Session("GetPartyResponse")



        'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
        '   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
        If strClient = "PC" Then
            Dim objBaseparty As New BasePartyPCType
            'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)
            Dim objClientDetail As New BaseClientSharedDataType
            objBaseparty = setobject()
            objBaseparty.ClientDetail = objClientDetail

            objBaseparty.Surname = txtLastName.Text
            objBaseparty.Forename = txtForeName.Text
            objBaseparty.Title = ddltitle.SelectedItem.ToString()
            objBaseparty.Initials = txtInitial.Text
            objBaseparty.TradingName = txtTradingName.Text


            objBaseparty.Salutation = txtsalutation.Text
            If (ddlPreferedCorres.SelectedIndex > 0) Then
                objBaseparty.ClientDetail.CorrespondenceCode = ddlPreferedCorres.SelectedValue
            End If
            'objBaseparty.ClientDetail.AccountBalance = Convert.ToDecimal(txtAccbalance.Text)
            'objBaseparty.ClientDetail.CorrespondenceCode = txtpreferedcorr.Text
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

            'VijayakumarStart

            objBaseparty.FileCode = txtFileCode.Text
            objBaseparty.AccountExecutiveCode = txtAccExecutiveCode.Text
            objBaseparty.AccountExecutiveCode = hdAccExecutiveCode.Value

            objBaseparty.AccountExecutive = txtAccExecutiveName.Text

            objBaseparty.AlternativeId = txtAlternativeIdentifier.Text

            'Praveen
            If Not String.IsNullOrEmpty(hdLeadAgent.Value) Then
                objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
                objBaseparty.ClientDetail.LeadAgentKeySpecified = True
            Else
                objBaseparty.ClientDetail.LeadAgentKeySpecified = False
            End If
            'Praveen

            'objBaseparty.ClientDetail.LeadAgentCode = txtLeadAgentCode.Text
            'objBaseparty.ClientDetail.LeadAgentName = txtLeadAgentName.Text

            'If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
            '    objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
            'End If
            If ddServiceLevel.SelectedIndex > 0 Then
                objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
            End If
            If ddBranch.SelectedIndex > 0 Then
                objBaseparty.BranchCode = ddBranch.SelectedValue
            End If
            If (ddSubBranch.SelectedIndex > 0) Then
                objBaseparty.SubBranchCode = ddSubBranch.SelectedValue
            End If
            If ddlArea.SelectedIndex <> 0 Then
                objBaseparty.ClientDetail.AreaCode = ddlArea.SelectedValue
            End If

            If Not txtCountyCourtJudge.Text = "" Then
                objBaseparty.ClientDetail.CountyCourtJudgmentsSpecified = True
                objBaseparty.ClientDetail.CountyCourtJudgments = txtCountyCourtJudge.Text
            Else
                objBaseparty.ClientDetail.CountyCourtJudgmentsSpecified = False
            End If

            'VijayakumarEnd


            'objBaseparty.Currency = txtcurrency.Text
            If ddlCurrency.SelectedIndex > 0 Then
                objBaseparty.Currency = ddlCurrency.SelectedValue
            End If
            If ddlPaymentMethod.SelectedIndex > 0 Then
                objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.ToString
            End If
            If (ddlReminderType.SelectedIndex > 0) Then
                objBaseparty.ClientDetail.ReminderCode = ddlReminderType.SelectedValue
            End If
            'objBaseparty.ClientDetail.PaymentTermCode = txttermspay.Text
            If (ddlTermsOfPayment.SelectedIndex > 0) Then
                objBaseparty.ClientDetail.PaymentTermCode = ddlTermsOfPayment.SelectedValue
            End If
            If (ddlrenewaldtopcode.SelectedIndex > 0) Then
                objBaseparty.ClientDetail.RenewalStopCode = ddlrenewaldtopcode.SelectedValue
            End If
            objBaseparty.Source = txtsource.Text
            If ddlPriOccupation.SelectedIndex > 0 Then
                objBaseparty.OccupationCode = ddlPriOccupation.SelectedItem.Text
            End If
            If ddlPriEmpBusiness.SelectedIndex > 0 Then
                objBaseparty.EmployersBusinessCode = ddlPriEmpBusiness.SelectedValue
            End If
            If ddlPriStatus.SelectedIndex = 0 Then
                objBaseparty.EmploymentStatusCodeSpecified = False
                objBaseparty.EmploymentStatusCode = CType(ddlPriStatus.SelectedIndex - 1, EmploymentStatusCodeType)
            Else
                objBaseparty.EmploymentStatusCodeSpecified = True
            End If
            'EmploymentStatusCodeType()
            'objBaseparty.MaritalStatusCode = CType(ddmaritalstatus.SelectedIndex, EmploymentStatusCodeType)
            If (ddlSecOccupation.SelectedIndex > 0) Then
                objBaseparty.SecOccupationCode = ddlSecOccupation.SelectedItem.Text
            End If
            If (ddlSecEmpsBusiness.SelectedIndex > 0) Then
                objBaseparty.SecEmployersBusinessCode = ddlSecEmpsBusiness.SelectedItem.ToString
            End If
            If (ddlSecStatus.SelectedIndex > 0) Then
                objBaseparty.SecEmploymentStatusCodeSpecified = True
                objBaseparty.SecEmploymentStatusCode = CType(ddlSecStatus.SelectedIndex - 1, EmploymentStatusCodeType)

            Else
                objBaseparty.SecEmploymentStatusCodeSpecified = False

            End If

            If (ddlAdditionsArea.SelectedIndex > 0) Then
                objBaseparty.ClientDetail.AreaCode = ddlAdditionsArea.SelectedValue
            End If
            'objBaseparty.FileCode = txtAdditionsFileCode.Text
            objBaseparty.FileCode = txtFileCode.Text



            If (IsDate(txtDobirth.Text)) Then
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

            'objBaseparty.MaritalStatusCode = ddmaritalstatus.SelectedItem.ToString()
            If (ddseasonalgift.SelectedIndex > 0) Then
                objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
            End If
            objBaseparty.ClientDetail.LoyaltyNumber = txtloyalty.Text
            If chkPets.Checked = True Then
                objBaseparty.PetOwnerSpecified = True
            Else
                objBaseparty.PetOwnerSpecified = False
            End If
            'Ravi
            objBaseparty.PetOwner = False 'txtpet.Text
            If (ddgender.SelectedIndex > 0) Then
                objBaseparty.GenderCode = ddgender.SelectedItem.ToString()
            End If
            If (ddnationality.SelectedIndex > 0) Then
                objBaseparty.NationalityCode = ddnationality.SelectedValue
            End If
            If (ddlAccomodation.SelectedIndex > 0) Then
                objBaseparty.AccommodationCode = ddlAccomodation.SelectedItem.ToString()
            End If
            Dim oLifeStyle As BasePartyPCTypeLifestyle
            'Dim i As Integer
            'objBaseparty.Lifestyle = oLifeStyle
            'Yet to be clear

            'objBaseparty.Lifestyle(0).SmokerSpecified = True

            'If chksmoker.Checked = True Then
            '    objBaseparty.Lifestyle(0).Smoker = True
            'Else
            '    objBaseparty.Lifestyle(0).Smoker = False
            'End If



            objBaseparty.ClientDetail.AgentReference = txtagentref.Text
            If txtcurrentagent.Text = "" Then
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
            Else
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
            End If

            'Ravi
            'objBaseparty.ClientDetail.CurrentIntermediaryKey = 7 'Convert.ToInt32(txtcurrentagent.Text)

            If String.IsNullOrEmpty(hdCurrentAgentKey.Value) Then
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
            Else
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
                objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
            End If
            If ddlprosStrengthCode.SelectedIndex > 0 Then
                objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue
            End If
            If ddlprosStatus.SelectedIndex > 0 Then
                objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue
            End If

            'If txtPIcode.Text = "" Then
            '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
            'Else
            '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
            'End If
            ''Ravi
            'objBaseparty.ClientDetail.PreviousInsurerKey = 47 'txtPIcode.Text
            'If txtPBcode.Text = "" Then
            '    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
            'Else
            '    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
            'End If
            ''Ravi
            'objBaseparty.ClientDetail.PreviousBrokerKey = 11 'txtPBcode.Text


            'txtPIname.Enabled = False
            'txtPBname.Enabled = False

            objBaseparty.ClientDetail.PreviousInsurerCode = txtPIcode.Text
            objBaseparty.ClientDetail.PreviousInsurerName = txtPIname.Text
            txtPIname.Enabled = False
            objBaseparty.ClientDetail.PreviousBrokerCode = txtPBcode.Text
            objBaseparty.ClientDetail.PreviousBrokerName = txtPBname.Text
            txtPBname.Enabled = False
            If txtPIcode.Text = "" Then
                objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
            Else
                objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
            End If
            If txtPBcode.Text = "" Then
                objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
            Else
                objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
            End If
            If Not hdCurrentAgentKey.Value = "" Then
                objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
            End If
            If Not hdPreviousInsurerKey.Value = "" Then
                objBaseparty.ClientDetail.PreviousInsurerKey = hdPreviousInsurerKey.Value
            End If
            If Not hdPrevBrokerKey.Value = "" Then
                objBaseparty.ClientDetail.PreviousBrokerKey = hdPrevBrokerKey.Value
            End If
            'objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
            'objBaseparty.ClientDetail.PreviousInsurerKey = hdPreviousInsurerKey.Value
            'objBaseparty.ClientDetail.PreviousBrokerKey = hdPrevBrokerKey.Value

            objBaseparty.TaxNumber = txttaxno.Text

            objBaseparty.DomiciledForTaxSpecified = True
            If chkDomicileTax.Checked = True Then
                objBaseparty.DomiciledForTax = True
            Else
                objBaseparty.DomiciledForTax = False
            End If

            If chkTaxExempt.Checked Then
                objBaseparty.TaxExemptSpecified = True
            Else
                objBaseparty.TaxExemptSpecified = False
            End If
            objBaseparty.TaxExempt = chkTaxExempt.Checked
            If txtpercentage.Text = "" Then
                objBaseparty.TaxPercentageSpecified = False
            Else
                objBaseparty.TaxPercentageSpecified = True
            End If
            If txtpercentage.Text <> "" Then
                objBaseparty.TaxPercentage = Convert.ToDecimal(txtpercentage.Text)
            End If



            Dim icnt As Integer
            icnt = 0
            If GVContacts.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.Addresses(GVContacts.Rows.Count - 1)
                For Each row As GridViewRow In GVContacts.Rows

                    objBaseparty.Addresses(icnt) = New BaseAddressWithContactsType
                    'objBaseparty.Addresses(icnt).AddressTypeCode = CType(ddlconType.SelectedIndex, AddressTypeType)
                    '"Item3131XBA" 'GVContacts.Rows(0).Cells(2).Text
                    objBaseparty.Addresses(icnt).AddressTypeCode = AddressTypeType.Item3131XCO
                    objBaseparty.Addresses(icnt).AddressLine2 = GVContacts.Rows(0).Cells(3).Text
                    objBaseparty.Addresses(icnt).AddressLine3 = GVContacts.Rows(0).Cells(4).Text
                    objBaseparty.Addresses(icnt).PostCode = GVContacts.Rows(0).Cells(5).Text
                    objBaseparty.Addresses(icnt).AddressLine1 = GVContacts.Rows(0).Cells(6).Text
                    objBaseparty.Addresses(icnt).AddressLine4 = GVContacts.Rows(0).Cells(7).Text
                    objBaseparty.Addresses(icnt).CountryCode = GVContacts.Rows(0).Cells(8).Text
                    icnt = icnt + 1

                Next
            End If


            icnt = 0

            If GVContacts1.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.Contacts(GVContacts1.Rows.Count - 1)

                For Each row As GridViewRow In GVContacts1.Rows
                    objBaseparty.Contacts(icnt) = New BaseContactType

                    Dim obj As BaseContactDetailType
                    obj = New BaseContactDetailType
                    obj.Item = "a.yahoo.com"
                    obj.ItemElementName = ItemChoiceType.EmailAddress

                    objBaseparty.Contacts(icnt).AreaCode = GVContacts1.Rows(0).Cells(1).Text
                    objBaseparty.Contacts(icnt).Description = GVContacts1.Rows(0).Cells(5).Text
                    objBaseparty.Contacts(icnt).Extension = GVContacts1.Rows(0).Cells(3).Text
                    'objBaseparty.Contacts(icnt).ContactTypeCode = GVContacts1.Rows(0).Cells(4).Text
                    objBaseparty.Contacts(icnt).ContactDetail = obj
                    'objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                    icnt = icnt + 1

                Next
            End If

            icnt = 0
            If gvassociates.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.ClientDetail.Associates(gvassociates.Rows.Count - 1)
                For Each row As GridViewRow In gvassociates.Rows
                    objBaseparty.ClientDetail.Associates(icnt) = New BaseAssociateType

                    objBaseparty.ClientDetail.Associates(icnt).RelationshipCode = gvassociates.Rows(0).Cells(2).Text
                    objBaseparty.ClientDetail.Associates(icnt).RelationshipDescription = gvassociates.Rows(0).Cells(3).Text
                    objBaseparty.ClientDetail.Associates(icnt).ClientKey = "240" 'gvassociates.Rows(0).Cells(4).Text
                    objBaseparty.ClientDetail.Associates(icnt).AssociateKey = gvassociates.Rows(0).Cells(5).Text
                    icnt = icnt + 1

                Next
            End If

            'Session("Associates) = 

            icnt = 0
            If gvConcictions.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.ClientDetail.Convictions(gvConcictions.Rows.Count - 1)
                For Each row As GridViewRow In gvConcictions.Rows
                    objBaseparty.ClientDetail.Convictions(icnt) = New BaseConvictionType

                    objBaseparty.ClientDetail.Convictions(icnt).SentenceTypeCode = gvConcictions.Rows(0).Cells(2).Text
                    If gvConcictions.Rows(0).Cells(7).Text = "" Then
                        objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = False
                    Else
                        objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = True
                    End If

                    If gvConcictions.Rows(0).Cells(3).Text = "" Then
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = False
                    Else
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = True
                    End If

                    If gvConcictions.Rows(0).Cells(5).Text = "" Then
                        objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                    Else
                        objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = True
                    End If

                    If gvConcictions.Rows(0).Cells(14).Text = "" Then
                        objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = False
                    Else
                        objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = True
                    End If

                    If gvConcictions.Rows(0).Cells(6).Text = "" Then
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = False
                    Else
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = True
                    End If



                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDuration = Convert.ToDecimal(gvConcictions.Rows(0).Cells(3).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).TypeCode = gvConcictions.Rows(0).Cells(4).Text
                    objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPoints = Convert.ToInt32(gvConcictions.Rows(0).Cells(5).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDate = Convert.ToDateTime(gvConcictions.Rows(0).Cells(6).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).FineAmount = Convert.ToDecimal(gvConcictions.Rows(0).Cells(7).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).Date = Convert.ToDateTime(gvConcictions.Rows(0).Cells(8).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).ConvictionKey = Convert.ToInt32(gvConcictions.Rows(0).Cells(9).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).AlcoholMeasurementMethod = gvConcictions.Rows(0).Cells(10).Text
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationQualifier = gvConcictions.Rows(0).Cells(11).Text
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDescription = gvConcictions.Rows(0).Cells(12).Text
                    objBaseparty.ClientDetail.Convictions(icnt).Description = gvConcictions.Rows(0).Cells(13).Text
                    objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevel = Convert.ToInt32(gvConcictions.Rows(0).Cells(14).Text)
                    objBaseparty.ClientDetail.Convictions(icnt).StatusCode = gvConcictions.Rows(0).Cells(15).Text
                    icnt = icnt + 1

                Next
            End If


            icnt = 0
            If gvDependents.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.Lifestyle(gvDependents.Rows.Count - 1)
                For Each row As GridViewRow In gvDependents.Rows
                    objBaseparty.Lifestyle(icnt) = New BasePartyPCTypeLifestyle

                    objBaseparty.Lifestyle(icnt).SecOccupationCode = gvDependents.Rows(0).Cells(2).Text
                    objBaseparty.Lifestyle(icnt).OccupationCode = gvDependents.Rows(0).Cells(3).Text
                    objBaseparty.Lifestyle(icnt).CategoryCode = gvDependents.Rows(0).Cells(4).Text
                    objBaseparty.Lifestyle(icnt).LifestyleKey = "0"


                    If gvDependents.Rows(0).Cells(7).Text = "" Then
                        objBaseparty.Lifestyle(icnt).SmokerSpecified = False
                    Else
                        objBaseparty.Lifestyle(icnt).SmokerSpecified = True
                    End If

                    objBaseparty.Lifestyle(icnt).Name = gvDependents.Rows(0).Cells(6).Text
                    If gvDependents.Rows(0).Cells(7).Text = "" Then
                        objBaseparty.Lifestyle(icnt).Smoker = False
                    Else
                        objBaseparty.Lifestyle(icnt).Smoker = True
                    End If

                    If gvDependents.Rows(0).Cells(8).Text = "" Then
                        objBaseparty.Lifestyle(icnt).DateOfBirthSpecified = False
                    Else
                        objBaseparty.Lifestyle(icnt).DateOfBirthSpecified = True
                    End If

                    'If DirectCast(row.Cells(8).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Lifestyle(icnt).GenderCodeSpecified = True
                    'Else
                    'objBaseparty.Lifestyle(icnt).GenderCodeSpecified = DirectCast(row.Cells(8).Controls(0), TextBox).Text

                    'End If

                    objBaseparty.Lifestyle(icnt).DateOfBirth = Convert.ToDateTime(gvDependents.Rows(0).Cells(8).Text)
                    objBaseparty.Lifestyle(icnt).GenderCode = GenderCodeType.F

                    icnt = icnt + 1

                Next

            End If

            icnt = 0
            If gvloyalty.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.Rows.Count - 1)
                For Each row As GridViewRow In gvloyalty.Rows
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt) = New BaseClientSharedDataTypeLoyaltyScheme
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).MainMember = gvloyalty.Rows(0).Cells(2).Text
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).OtherReference = gvloyalty.Rows(0).Cells(3).Text
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDate = Convert.ToDateTime(gvloyalty.Rows(0).Cells(4).Text)
                    If gvloyalty.Rows(0).Cells(7).Text = "" Then
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = False
                    Else
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = True
                    End If

                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).StartDate = Convert.ToDateTime(gvloyalty.Rows(0).Cells(6).Text)
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeKey = Convert.ToInt32(gvloyalty.Rows(0).Cells(7).Text)
                    If gvloyalty.Rows(0).Cells(4).Text = "" Then
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = False
                    Else
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = True
                    End If

                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeCode = gvloyalty.Rows(0).Cells(8).Text
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).MembershipNumber = gvloyalty.Rows(0).Cells(9).Text

                    If gvloyalty.Rows(0).Cells(8).Text = "" Then
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = False
                    Else
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = True

                    End If


                    icnt = icnt + 1

                Next

            End If

            icnt = 0
            If gvpolicies.Rows.Count > 0 Then
                ReDim Preserve objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.Rows.Count - 1)
                For Each row As GridViewRow In gvpolicies.Rows
                    objBaseparty.ClientDetail.ProspectPolicies(icnt) = New BaseClientSharedDataTypeProspectPolicies

                    If gvpolicies.Rows(0).Cells(4).Text = "" Then
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = False
                    Else
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = True
                    End If
                    If gvpolicies.Rows(0).Cells(5).Text = "" Then
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = False
                    Else
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = True
                    End If


                    objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectPolicyKey = gvpolicies.Rows(0).Cells(2).Text
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectTypeCode = gvpolicies.Rows(0).Cells(3).Text
                    If gvpolicies.Rows(0).Cells(6).Text = "" Then
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = False
                    Else
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = True
                    End If

                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuoted = Convert.ToInt32(gvpolicies.Rows(0).Cells(4).Text)
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremium = Convert.ToDecimal(gvpolicies.Rows(0).Cells(5).Text)
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDate = Convert.ToDateTime(gvpolicies.Rows(0).Cells(6).Text)
                    icnt = icnt + 1

                Next
            End If

            Return objBaseparty


        End If

            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then


                Dim objBaseparty As New BasePartyOTHERType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)



                'objBaseparty.Name = txtgrname.Text
                'objBaseparty.TypeCode = txtgrtype.Text

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
                If chkTaxExempt.Text = "" Then
                    objBaseparty.TaxExemptSpecified = False
                Else
                    objBaseparty.TaxExemptSpecified = True
                End If
                objBaseparty.TaxExempt = chkTaxExempt.Text
                If txtpercentage.Text = "" Then
                    objBaseparty.TaxPercentageSpecified = False
                Else
                    objBaseparty.TaxPercentageSpecified = True
                End If
                objBaseparty.TaxPercentage = txtpercentage.Text

                '=====================================================
                'By Praveen

                'Dim icnt As Integer
                'icnt = 0
                'For Each row As GridViewRow In GVContacts.Rows

                '    objBaseparty.Addresses(icnt).AddressLine2 = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).AddressLine3 = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).PostCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).AddressLine1 = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).AddressLine4 = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).CountryCode = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                '    icnt = icnt + 1

                'Next

                'icnt = 0
                'For Each row As GridViewRow In GVContacts1.Rows


                '    Dim obj As BaseContactDetailType
                '    obj = New BaseContactDetailType
                '    obj.Item = "a.yahoo.com"
                '    obj.ItemElementName = ItemChoiceType.EmailAddress

                '    objBaseparty.Contacts(icnt).AreaCode = DirectCast(row.Cells(0).Controls(0), TextBox).Text
                '    objBaseparty.Contacts(icnt).ContactDetail = obj
                '    objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                '    icnt = icnt + 1

                'Next




                'icnt = 0
                'For Each row As GridViewRow In gvConcictions.Rows

                '    objBaseparty.Convictions(icnt).SentenceTypeCode = DirectCast(row.Cells(0).Controls(0), TextBox).Text
                '    If DirectCast(row.Cells(1).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.Convictions(icnt).FineAmountSpecified = False
                '    Else
                '        objBaseparty.Convictions(icnt).FineAmountSpecified = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(2).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.Convictions(icnt).SentenceDurationSpecified = False
                '    Else
                '        objBaseparty.Convictions(icnt).SentenceDurationSpecified = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(8).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                '    Else
                '        objBaseparty.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = DirectCast(row.Cells(8).Controls(0), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(12).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.Convictions(icnt).AlcoholLevelSpecified = False
                '    Else
                '        objBaseparty.Convictions(icnt).AlcoholLevelSpecified = DirectCast(row.Cells(12).Controls(0), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(13).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.Convictions(icnt).SentenceEffectiveDateSpecified = False
                '    Else
                '        objBaseparty.Convictions(icnt).SentenceEffectiveDateSpecified = DirectCast(row.Cells(13).Controls(0), TextBox).Text
                '    End If



                '    objBaseparty.Convictions(icnt).SentenceDuration = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).TypeCode = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).DrivingLicensePenaltyPoints = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).SentenceEffectiveDate = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).FineAmount = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).Date = DirectCast(row.Cells(9).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).ConvictionKey = DirectCast(row.Cells(10).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).AlcoholMeasurementMethod = DirectCast(row.Cells(11).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).SentenceDurationQualifier = DirectCast(row.Cells(14).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).SentenceDescription = DirectCast(row.Cells(15).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).Description = DirectCast(row.Cells(16).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).AlcoholLevel = DirectCast(row.Cells(17).Controls(0), TextBox).Text
                '    objBaseparty.Convictions(icnt).StatusCode = DirectCast(row.Cells(18).Controls(0), TextBox).Text
                '    icnt = icnt + 1

                'Next

                'By Praveen
                '=====================================================
                Return objBaseparty

            End If

            'uday
            If strClient = "CC" Then
                'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                '      oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then


                'Dim objBaseparty As New BasePartyCCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
                'objBaseparty.CompanyName = txtCompanyName.Text
                'objBaseparty.Companyreg = txtCompanyName.Text

                'Praveen
                Dim objBaseparty As New BasePartyCCType
                Dim objClientDetail As New BaseClientSharedDataType
                objBaseparty = setobject()
                objBaseparty.ClientDetail = objClientDetail

                txtClientCode.Enabled = False
                objBaseparty.ClientDetail.ShortName = txtClientCode.Text
                objBaseparty.CompanyName = txtTradingName.Text
                objBaseparty.MainContact = txtMainContact.Text
                objBaseparty.CompanyReg = txtCompanyReg.Text

                'objBaseparty.ClientDetail.AccountBalance = txtAccbalance.Text
                'objBaseparty.ClientDetail.LastYearTurnover = txtLastYearturnOver.Text
                'objBaseparty.ClientDetail.YearToDateTurnover = txtYearTodateTurnOver.Text

                'objBaseparty.AccountExecutiveCode = txtAccExecutiveCode.Text
                'objBaseparty.AccountExecutiveCode = hdAccExecutiveCode.Value
                'objBaseparty.AccountExecutive = txtAccExecutiveName.Text
                'objBaseparty.ClientDetail.LeadAgentCode = txtLeadAgentCode.Text
                'objBaseparty.ClientDetail.LeadAgentName = txtLeadAgentName.Text

                objBaseparty.AccountExecutiveCode = txtAccExecutiveCode.Text
                objBaseparty.AccountExecutiveCode = hdAccExecutiveCode.Value
                objBaseparty.AccountExecutive = txtAccExecutiveName.Text

                'Praveen
                If Not String.IsNullOrEmpty(hdLeadAgent.Value) Then
                    objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
                    objBaseparty.ClientDetail.LeadAgentKeySpecified = True
                Else
                    objBaseparty.ClientDetail.LeadAgentKeySpecified = False
                End If
                'Praveen

                objBaseparty.AlternativeId = txtAlternativeIdentifier.Text

                If ddServiceLevel.SelectedIndex > 0 Then
                    objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
                End If
                If ddBranch.SelectedIndex > 0 Then
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

                objBaseparty.AccountExecutiveCode = txtAccExecutiveCode.Text
                objBaseparty.AccountExecutiveCode = hdAccExecutiveCode.Value
                objBaseparty.AccountExecutive = txtAccExecutiveName.Text
                If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                    objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
                End If
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
                '''''''''''''''''''''''''''''Convictions''''''''''''''''''''''''''''''
                If Not txtCountyCourtJudge.Text = "" Then
                    objBaseparty.ClientDetail.CountyCourtJudgmentsSpecified = True
                    objBaseparty.ClientDetail.CountyCourtJudgments = txtCountyCourtJudge.Text
                Else
                    objBaseparty.ClientDetail.CountyCourtJudgmentsSpecified = False
                End If
                ''''''''''''''''''''''''''''''''''''Additions'''''''''''''''''''''''''''''''''''''
                If (ddlCurrency.SelectedIndex > 0) Then
                    objBaseparty.Currency = ddlCurrency.SelectedValue
                End If
                If (ddlPaymentMethod.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.Text
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
                objBaseparty.ClientDetail.LoyaltyNumber = txtLoyaltyNumber.Text
                If (ddlSeasonalGift.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.SeasonalGiftCode = ddlSeasonalGift.SelectedValue
                End If
                If Not txtwage.Text = "" Then
                    objBaseparty.WageRoll = txtwage.Text
                    objBaseparty.WageRollSpecified = True
                Else
                    objBaseparty.WageRollSpecified = False
                End If
                If (ddlturnover.SelectedIndex > 0) Then
                    objBaseparty.TurnoverCode = ddlturnover.SelectedValue
                End If
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
                txtDobirth.Enabled = False
                ddmaritalstatus.Enabled = False
                If (ddseasonalgift.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
                End If

                'objBaseparty.ClientDetail.LoyaltyNumber = txtloyalty.Text
                pnlLifeStyle.Visible = False

                ''''''''''''''''''''''''''''''''''''Prospecting'''''''''''''''''''''''''''''''''''''''
                objBaseparty.ClientDetail.AgentReference = txtagentref.Text
                If String.IsNullOrEmpty(hdCurrentAgentKey.Value) Then
                    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
                Else
                    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
                    objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
                End If
                objBaseparty.ClientDetail.CurrentIntermediaryName = txtcurrentagent.Text

                If (ddlprosStrengthCode.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue
                End If
                If (ddlprosStatus.SelectedIndex > 0) Then
                    objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue
                End If

                objBaseparty.ClientDetail.PreviousInsurerCode = txtPIcode.Text
                objBaseparty.ClientDetail.PreviousInsurerName = txtPIname.Text
                txtPIname.Enabled = False
                objBaseparty.ClientDetail.PreviousBrokerCode = txtPBcode.Text
                objBaseparty.ClientDetail.PreviousBrokerName = txtPBname.Text
                txtPBname.Enabled = False
                If txtPIcode.Text = "" Then
                    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
                Else
                    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
                End If
                If txtPBcode.Text = "" Then
                    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
                Else
                    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
            End If
            If Not hdCurrentAgentKey.Value = "" Then
                objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
            End If
            If Not hdPreviousInsurerKey.Value = "" Then
                objBaseparty.ClientDetail.PreviousInsurerKey = hdPreviousInsurerKey.Value
            End If
            If Not hdPrevBrokerKey.Value = "" Then
                objBaseparty.ClientDetail.PreviousBrokerKey = hdPrevBrokerKey.Value
            End If

                'If txtcurrentagent.Text = "" Then
                '    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
                'Else
                '    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
                'End If
                'If txtcurrentagent.Text.Length > 0 Then
                '    objBaseparty.ClientDetail.CurrentIntermediaryKey = Convert.ToInt32(txtcurrentagent.Text)
                'End If

                'If txtPIcode.Text = "" Then
                '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
                'Else
                '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
                'End If
                ''Ravi
                'objBaseparty.ClientDetail.PreviousInsurerKey = 47 'txtPIcode.Text
                'If txtPBcode.Text = "" Then
                '    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
                'Else
                '    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
                'End If
                ''Ravi
                'objBaseparty.ClientDetail.PreviousBrokerKey = 11
                'If txtPIcode.Text = "" Then
                '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
                'Else
                '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
                'End If
                'If txtPIcode.Text.Length > 0 Then
                '    objBaseparty.ClientDetail.PreviousInsurerKey = Convert.ToInt32(txtPIcode.Text)
                'End If
                'objBaseparty.ClientDetail.CurrentIntermediaryKey = hdCurrentAgentKey.Value
                'objBaseparty.ClientDetail.PreviousInsurerKey = hdPreviousInsurerKey.Value
                'objBaseparty.ClientDetail.PreviousBrokerKey = hdPrevBrokerKey.Value

                objBaseparty.TaxNumber = txttaxno.Text
                objBaseparty.DomiciledForTaxSpecified = True
                If chkDomicileTax.Checked = True Then
                    objBaseparty.DomiciledForTax = True
                Else
                    objBaseparty.DomiciledForTax = False
                End If
                If chkTaxExempt.Checked Then
                    objBaseparty.TaxExemptSpecified = True
                Else
                    objBaseparty.TaxExemptSpecified = False
                End If
                objBaseparty.TaxExempt = chkTaxExempt.Checked
                If txtpercentage.Text = "" Then
                    objBaseparty.TaxPercentageSpecified = False
                Else
                    objBaseparty.TaxPercentageSpecified = True
                End If
                If txtpercentage.Text <> "" Then
                    objBaseparty.TaxPercentage = Convert.ToDecimal(txtpercentage.Text)
                End If

                Dim icnt As Integer
                icnt = 0
                If GVContacts.Rows.Count > 0 Then
                    ReDim Preserve objBaseparty.Addresses(GVContacts.Rows.Count - 1)
                    For Each row As GridViewRow In GVContacts.Rows

                        objBaseparty.Addresses(icnt) = New BaseAddressWithContactsType
                    objBaseparty.Addresses(icnt).AddressTypeCode = AddressTypeType.Item3131XCO
                        '"Item3131XBA" 'GVContacts.Rows(0).Cells(2).Text
                        objBaseparty.Addresses(icnt).AddressLine2 = GVContacts.Rows(0).Cells(3).Text
                        objBaseparty.Addresses(icnt).AddressLine3 = GVContacts.Rows(0).Cells(4).Text
                        objBaseparty.Addresses(icnt).PostCode = GVContacts.Rows(0).Cells(5).Text
                        objBaseparty.Addresses(icnt).AddressLine1 = GVContacts.Rows(0).Cells(6).Text
                        objBaseparty.Addresses(icnt).AddressLine4 = GVContacts.Rows(0).Cells(7).Text
                        objBaseparty.Addresses(icnt).CountryCode = GVContacts.Rows(0).Cells(8).Text
                        icnt = icnt + 1

                    Next
                End If


                icnt = 0

                If GVContacts1.Rows.Count > 0 Then
                    ReDim Preserve objBaseparty.Contacts(GVContacts1.Rows.Count - 1)

                    For Each row As GridViewRow In GVContacts1.Rows
                        objBaseparty.Contacts(icnt) = New BaseContactType

                        Dim obj As BaseContactDetailType
                        obj = New BaseContactDetailType
                        obj.Item = "a.yahoo.com"
                        obj.ItemElementName = ItemChoiceType.EmailAddress

                        objBaseparty.Contacts(icnt).AreaCode = GVContacts1.Rows(0).Cells(1).Text
                        objBaseparty.Contacts(icnt).Description = GVContacts1.Rows(0).Cells(5).Text
                        objBaseparty.Contacts(icnt).Extension = GVContacts1.Rows(0).Cells(3).Text
                        'objBaseparty.Contacts(icnt).ContactTypeCode = GVContacts1.Rows(0).Cells(4).Text
                        objBaseparty.Contacts(icnt).ContactDetail = obj
                        'objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                        icnt = icnt + 1

                    Next
                End If

                icnt = 0
                If gvassociates.Rows.Count > 0 Then
                    ReDim Preserve objBaseparty.ClientDetail.Associates(gvassociates.Rows.Count - 1)
                    For Each row As GridViewRow In gvassociates.Rows
                        objBaseparty.ClientDetail.Associates(icnt) = New BaseAssociateType

                        objBaseparty.ClientDetail.Associates(icnt).RelationshipCode = gvassociates.Rows(0).Cells(2).Text
                        objBaseparty.ClientDetail.Associates(icnt).RelationshipDescription = gvassociates.Rows(0).Cells(3).Text
                        objBaseparty.ClientDetail.Associates(icnt).ClientKey = "240" 'gvassociates.Rows(0).Cells(4).Text
                        objBaseparty.ClientDetail.Associates(icnt).AssociateKey = gvassociates.Rows(0).Cells(5).Text
                        icnt = icnt + 1

                    Next
                End If

                'Session("Associates) = 

                icnt = 0
                If gvConcictions.Rows.Count > 0 Then
                    ReDim Preserve objBaseparty.ClientDetail.Convictions(gvConcictions.Rows.Count - 1)
                    For Each row As GridViewRow In gvConcictions.Rows
                        objBaseparty.ClientDetail.Convictions(icnt) = New BaseConvictionType

                        objBaseparty.ClientDetail.Convictions(icnt).SentenceTypeCode = gvConcictions.Rows(0).Cells(2).Text
                        If gvConcictions.Rows(0).Cells(7).Text = "" Then
                            objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = False
                        Else
                            objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = True
                        End If

                        If gvConcictions.Rows(0).Cells(3).Text = "" Then
                            objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = False
                        Else
                            objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = True
                        End If

                        If gvConcictions.Rows(0).Cells(5).Text = "" Then
                            objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                        Else
                            objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = True
                        End If

                        If gvConcictions.Rows(0).Cells(14).Text = "" Then
                            objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = False
                        Else
                            objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = True
                        End If

                        If gvConcictions.Rows(0).Cells(6).Text = "" Then
                            objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = False
                        Else
                            objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = True
                        End If



                        objBaseparty.ClientDetail.Convictions(icnt).SentenceDuration = Convert.ToDecimal(gvConcictions.Rows(0).Cells(3).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).TypeCode = gvConcictions.Rows(0).Cells(4).Text
                        objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPoints = Convert.ToInt32(gvConcictions.Rows(0).Cells(5).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDate = Convert.ToDateTime(gvConcictions.Rows(0).Cells(6).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).FineAmount = Convert.ToDecimal(gvConcictions.Rows(0).Cells(7).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).Date = Convert.ToDateTime(gvConcictions.Rows(0).Cells(8).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).ConvictionKey = Convert.ToInt32(gvConcictions.Rows(0).Cells(9).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).AlcoholMeasurementMethod = gvConcictions.Rows(0).Cells(10).Text
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationQualifier = gvConcictions.Rows(0).Cells(11).Text
                        objBaseparty.ClientDetail.Convictions(icnt).SentenceDescription = gvConcictions.Rows(0).Cells(12).Text
                        objBaseparty.ClientDetail.Convictions(icnt).Description = gvConcictions.Rows(0).Cells(13).Text
                        objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevel = Convert.ToInt32(gvConcictions.Rows(0).Cells(14).Text)
                        objBaseparty.ClientDetail.Convictions(icnt).StatusCode = gvConcictions.Rows(0).Cells(15).Text
                        icnt = icnt + 1

                    Next
                End If
                icnt = 0
                If gvloyalty.Rows.Count > 0 Then
                    ReDim Preserve objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.Rows.Count - 1)
                    For Each row As GridViewRow In gvloyalty.Rows
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt) = New BaseClientSharedDataTypeLoyaltyScheme
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).MainMember = gvloyalty.Rows(0).Cells(2).Text
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).OtherReference = gvloyalty.Rows(0).Cells(3).Text
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDate = Convert.ToDateTime(gvloyalty.Rows(0).Cells(4).Text)
                        If gvloyalty.Rows(0).Cells(7).Text = "" Then
                            objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = False
                        Else
                            objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = True
                        End If

                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).StartDate = Convert.ToDateTime(gvloyalty.Rows(0).Cells(6).Text)
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeKey = Convert.ToInt32(gvloyalty.Rows(0).Cells(7).Text)
                        If gvloyalty.Rows(0).Cells(4).Text = "" Then
                            objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = False
                        Else
                            objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = True
                        End If

                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeCode = gvloyalty.Rows(0).Cells(8).Text
                        objBaseparty.ClientDetail.LoyaltyScheme(icnt).MembershipNumber = gvloyalty.Rows(0).Cells(9).Text

                        If gvloyalty.Rows(0).Cells(8).Text = "" Then
                            objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = False
                        Else
                            objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = True

                        End If


                        icnt = icnt + 1

                    Next

                End If

                icnt = 0
                If gvpolicies.Rows.Count > 0 Then
                    ReDim Preserve objBaseparty.ClientDetail.ProspectPolicies(gvpolicies.Rows.Count - 1)
                    For Each row As GridViewRow In gvpolicies.Rows
                        objBaseparty.ClientDetail.ProspectPolicies(icnt) = New BaseClientSharedDataTypeProspectPolicies

                        If gvpolicies.Rows(0).Cells(4).Text = "" Then
                            objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = False
                        Else
                            objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = True
                        End If
                        If gvpolicies.Rows(0).Cells(5).Text = "" Then
                            objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = False
                        Else
                            objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = True
                        End If


                        objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectPolicyKey = gvpolicies.Rows(0).Cells(2).Text
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectTypeCode = gvpolicies.Rows(0).Cells(3).Text
                        If gvpolicies.Rows(0).Cells(6).Text = "" Then
                            objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = False
                        Else
                            objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = True
                        End If

                        objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuoted = Convert.ToInt32(gvpolicies.Rows(0).Cells(4).Text)
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremium = Convert.ToDecimal(gvpolicies.Rows(0).Cells(5).Text)
                        objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDate = Convert.ToDateTime(gvpolicies.Rows(0).Cells(6).Text)
                        icnt = icnt + 1

                    Next
                End If

                Return objBaseparty
                'End Praveen

                'Commented Block
                'If (ddlPreferedCorres.SelectedIndex > 0) Then
                '    objBaseparty.ClientDetail.CorrespondenceCode = ddlPreferedCorres.SelectedValue
                'End If



                'If txtwage.Text.Length > 0 Then
                '    objBaseparty.WageRoll = Convert.ToDecimal(txtwage.Text)
                'Else

                'End If
                ''objBaseparty.TurnoverCode = txtturnover.Text
                'objBaseparty.TurnoverCode = ddlturnover.SelectedValue
                'If txtfinancial.Text.Length > 0 Then
                '    objBaseparty.FinancialYear = Convert.ToDateTime(txtfinancial.Text)
                'End If
                'objBaseparty.TradeCode = txtTradingName.Text

                'objBaseparty.Salutation = txtsalutation.Text
                ''objBaseparty.ClientDetail.CorrespondenceCode = txtpreferedcorr.Text
                'objBaseparty.TPSSpecified = True
                'objBaseparty.MPSSpecified = True
                'objBaseparty.eMPSSpecified = True
                'If chkTPS.Checked = True Then
                '    objBaseparty.TPS = True
                'Else
                '    objBaseparty.TPS = False
                'End If

                'If chkMPS.Checked = True Then
                '    objBaseparty.MPS = True
                'Else
                '    objBaseparty.MPS = False
                'End If

                'If chkeMPS.Checked = True Then
                '    objBaseparty.eMPS = True
                'Else
                '    objBaseparty.eMPS = False
                'End If
                'objBaseparty.ClientDetail.IsProspectSpecified = True
                'If chkIsprospect.Checked = True Then
                '    objBaseparty.ClientDetail.IsProspect = True
                'Else
                '    objBaseparty.ClientDetail.IsProspect = False
                'End If

                'objBaseparty.ClientDetail.IsAgentSpecified = True
                'If chkIsagent.Checked = True Then
                '    objBaseparty.ClientDetail.IsAgent = True
                'Else
                '    objBaseparty.ClientDetail.IsAgent = False
                'End If
                ''VijayakumarStart            


                'objBaseparty.AccountExecutive = txtAccExecutiveCode.Text
                'If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                '    objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
                'End If
                ''txtAlternativeIdentifier.Text = objBaseparty.AlternativeId

                'If ddServiceLevel.SelectedValue <> 0 Then
                '    'objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
                '    objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
                'End If

                'objBaseparty.BranchCode = ddBranch.SelectedValue

                'If ddlBusiness.SelectedValue <> 0 Then
                '    objBaseparty.BusinessCode = ddlBusiness.SelectedItem.ToString()
                'End If
                'If ddlTrade.SelectedValue <> 0 Then
                '    objBaseparty.TradeCode = ddlTrade.SelectedItem.ToString()
                'End If
                'If ddlNoOfEmployees.SelectedValue <> 0 Then
                '    objBaseparty.NumberOfEmployees = ddlNoOfEmployees.SelectedValue
                'End If

                ''If ddlSic.SelectedItem IsNot Nothing Then
                ''    objBaseparty.SICCode = ddlSic.SelectedItem.ToString()
                ''End If
                ''If txtTradeSince.Text.Length > 0 Then
                ''    objBaseparty.TradingSince = Convert.ToDateTime(txtTradeSince.Text)
                ''End If
                'objBaseparty.NumberOfOffices = Convert.ToInt32(txtNoOfOffices.Text)

                ''VijayakumarEnd


                'objBaseparty.Currency = ddlCurrency.SelectedValue
                'objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.ToString()
                'objBaseparty.ClientDetail.ReminderCode = ddlReminderType.SelectedValue
                ''objBaseparty.ClientDetail.PaymentTermCode = txttermspay.Text
                'objBaseparty.ClientDetail.RenewalStopCode = ddlrenewaldtopcode.SelectedValue
                'objBaseparty.Source = txtsource.Text

                'objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
                'objBaseparty.ClientDetail.LoyaltyNumber = txtloyalty.Text



                'objBaseparty.ClientDetail.AgentReference = txtagentref.Text
                'If txtcurrentagent.Text = "" Then
                '    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
                'Else
                '    objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
                'End If
                'If txtcurrentagent.Text.Length > 0 Then
                '    objBaseparty.ClientDetail.CurrentIntermediaryKey = Convert.ToInt32(txtcurrentagent.Text)
                'End If
                ''objBaseparty.ClientDetail.StrengthCode = txtstrength.Text
                '' objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue.ToString
                '' objBaseparty.ClientDetail.StatusCode = txtpstatus.Text
                ''objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue.ToString


                'If txtPIcode.Text = "" Then
                '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
                'Else
                '    objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
                'End If
                'If txtPIcode.Text.Length > 0 Then
                '    objBaseparty.ClientDetail.PreviousInsurerKey = Convert.ToInt32(txtPIcode.Text)
                'End If
                'If txtPBcode.Text = "" Then
                '    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
                'Else
                '    objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
                'End If
                'If txtPBcode.Text.Length > 0 Then
                '    objBaseparty.ClientDetail.PreviousBrokerKey = txtPBcode.Text
                'End If

                'txtPIname.Text = ""
                'txtPIname.Enabled = False
                'txtPBname.Text = ""
                'txtPBname.Enabled = False

                'objBaseparty.TaxNumber = txttaxno.Text
                'objBaseparty.DomiciledForTaxSpecified = True
                'If chkDomicileTax.Checked = True Then
                '    objBaseparty.DomiciledForTax = True
                'Else
                '    objBaseparty.DomiciledForTax = False
                'End If

                ''If txtexempt.Text = "" Then
                ''    objBaseparty.TaxExemptSpecified = False
                ''Else
                ''    objBaseparty.TaxExemptSpecified = True
                ''End If
                ''objBaseparty.TaxExempt = txtexempt.Text
                'If txtpercentage.Text = "" Then
                '    objBaseparty.TaxPercentageSpecified = False
                'Else
                '    objBaseparty.TaxPercentageSpecified = True
                'End If
                'If txtpercentage.Text.Length > 0 Then
                '    objBaseparty.TaxPercentage = Convert.ToDecimal(txtpercentage.Text)
                'End If
                'If ddlBusiness.SelectedValue <> 0 Then
                '    objBaseparty.BusinessCode = ddlBusiness.SelectedItem.Text
                'End If

                ''If ddlSic.SelectedValue <> "0" Then 'ddlSic.SelectedValue <> 0 then
                ''    objBaseparty.SICCode = ddlSic.SelectedValue
                ''End If

                'objBaseparty.TradeCode = ddlTrade.SelectedItem.ToString()
                'If ddlNoOfEmployees.SelectedValue = 0 Then
                '    objBaseparty.NumberOfEmployeesSpecified = False
                'Else
                '    objBaseparty.NumberOfEmployeesSpecified = True
                '    objBaseparty.NumberOfEmployees = ddlNoOfEmployees.SelectedValue
                'End If

                'Dim icnt As Integer

                'icnt = 0
                'For Each row As GridViewRow In GVContacts.Rows

                '    objBaseparty.Addresses(icnt).AddressLine2 = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).AddressLine3 = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).PostCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).AddressLine1 = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).AddressLine4 = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                '    objBaseparty.Addresses(icnt).CountryCode = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                '    icnt = icnt + 1

                'Next

                'icnt = 0
                'For Each row As GridViewRow In GVContacts1.Rows


                '    Dim obj As BaseContactDetailType
                '    obj = New BaseContactDetailType
                '    obj.Item = "a.yahoo.com"
                '    obj.ItemElementName = ItemChoiceType.EmailAddress

                '    objBaseparty.Contacts(icnt).AreaCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    objBaseparty.Contacts(icnt).ContactDetail = obj
                '    objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                '    icnt = icnt + 1

                'Next

                'icnt = 0
                'For Each row As GridViewRow In gvassociates.Rows

                '    objBaseparty.ClientDetail.Associates(icnt).RelationshipCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Associates(icnt).RelationshipDescription = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Associates(icnt).ClientKey = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Associates(icnt).AssociateKey = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                '    icnt = icnt + 1

                'Next


                'icnt = 0
                'For Each row As GridViewRow In gvConcictions.Rows

                '    objBaseparty.ClientDetail.Convictions(icnt).SentenceTypeCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    If DirectCast(row.Cells(2).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = True
                '    End If

                '    If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = True 'DirectCast(row.Cells(3).Controls(1), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(9).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = True 'DirectCast(row.Cells(9).Controls(1), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(13).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = True 'DirectCast(row.Cells(13).Controls(1), TextBox).Text
                '    End If

                '    If DirectCast(row.Cells(14).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = True 'DirectCast(row.Cells(14).Controls(1), TextBox).Text
                '    End If



                '    objBaseparty.ClientDetail.Convictions(icnt).SentenceDuration = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.Convictions(icnt).TypeCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPoints = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.Convictions(icnt).FineAmount = Convert.ToDecimal(DirectCast(row.Cells(6).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.Convictions(icnt).Date = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).ConvictionKey = DirectCast(row.Cells(8).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).AlcoholMeasurementMethod = DirectCast(row.Cells(9).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationQualifier = DirectCast(row.Cells(10).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).SentenceDescription = DirectCast(row.Cells(11).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).Description = DirectCast(row.Cells(12).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevel = DirectCast(row.Cells(13).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.Convictions(icnt).StatusCode = DirectCast(row.Cells(14).Controls(0), TextBox).Text
                '    icnt = icnt + 1

                'Next



                'icnt = 0
                'For Each row As GridViewRow In gvloyalty.Rows

                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).MainMember = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).OtherReference = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDate = Convert.ToDateTime(DirectCast(row.Cells(3).Controls(0), TextBox).Text)
                '    If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = False
                '    Else
                '        objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = True 'DirectCast(row.Cells(3).Controls(1), TextBox).Text

                '    End If

                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).StartDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeKey = Convert.ToInt32(DirectCast(row.Cells(6).Controls(0), TextBox).Text)
                '    If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = True 'DirectCast(row.Cells(6).Controls(1), TextBox).Text
                '    End If

                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeCode = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                '    objBaseparty.ClientDetail.LoyaltyScheme(icnt).MembershipNumber = DirectCast(row.Cells(8).Controls(0), TextBox).Text

                '    If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = True 'DirectCast(row.Cells(9).Controls(1), TextBox).Text

                '    End If


                '    icnt = icnt + 1

                'Next

                'icnt = 0
                'For Each row As GridViewRow In gvpolicies.Rows
                '    If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = True 'DirectCast(row.Cells(0).Controls(1), TextBox).Text
                '    End If
                '    If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = True 'DirectCast(row.Cells(1).Controls(1), TextBox).Text
                '    End If


                '    objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectPolicyKey = Convert.ToInt32(DirectCast(row.Cells(1).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectTypeCode = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                '    If DirectCast(row.Cells(5).Controls(0), TextBox).Text = "" Then
                '        objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = False
                '    Else
                '        objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = True 'DirectCast(row.Cells(4).Controls(1), TextBox).Text
                '    End If

                '    objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuoted = Convert.ToDecimal(DirectCast(row.Cells(3).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremium = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                '    objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                '    icnt = icnt + 1

                'Next
                'Return objBaseparty

                'Commented Block
            End If


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
            objBaseparty = Session("GetPartyResponse")
            If objBaseparty.GetType().Name = "BasePartyPCType" Then
                txtstname.Text = DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine4
                txtposttown.Text = DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine3
                txtpostcode.Text = DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).PostCode
                txtlocality.Text = DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine2
                txtcounty.Text = DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).AddressLine4
                'txtcountry.Text = oGetPartyResponseType.Item.Addresses(GVContacts.SelectedIndex).CountryCode
                ddlconCountry.SelectedValue = DirectCast(objBaseparty, BasePartyPCType).Addresses(GVContacts.SelectedIndex).CountryCode

            End If
           
            plnAddContact.Visible = False
            pnlAddAddress.Visible = True

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub GVContacts1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVContacts1.SelectedIndexChanged
        Try
            txtareacode.Text = GVContacts1.SelectedRow.Cells(1).Text
            txtdescription.Text = GVContacts1.SelectedRow.Cells(5).Text
            txtNumber.Text = GVContacts1.SelectedRow.Cells(2).Text
            txtextension.Text = GVContacts1.SelectedRow.Cells(3).Text
            plnAddContact.Visible = True
        Catch ex As Exception

        End Try
    End Sub
    Public Function setobject() As Object
        If strClient = "PC" Then
            objBaseparty = New BasePartyPCType
        End If

        If strClient = "CC" Then
            objBaseparty = New BasePartyCCType
        End If

        If strClient = "GC" Then
            objBaseparty = New BasePartyOTHERType
        End If
        Return objBaseparty
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

        objBaseparty = Session("GetPartyResponse")
        Try
            'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
            '            oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
            If objBaseparty IsNot Nothing AndAlso _
                       objBaseparty.GetType() Is GetType(BasePartyPCType) Then
                'Dim objBaseparty As New BasePartyPCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

                txtclient.Text = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateName
                hdAssociateKey.Value = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey

                ddlrelationshipcode.SelectedValue = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode.Trim
                Session("PartyKey") = objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey
            End If
            'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
            '                    oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then
            If objBaseparty IsNot Nothing AndAlso _
                     objBaseparty.GetType() Is GetType(BasePartyCCType) Then
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

    Protected Sub gvConcictions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvConcictions.SelectedIndexChanged
        Dim oGetPartyResponseType As New GetPartyResponseType

        objBaseparty = Session("GetPartyResponse")
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

        objBaseparty = Session("GetPartyResponse")

        Try
            If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                               oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then

                Dim objBaseparty As New BasePartyCCType
                objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
                'ddlLoyaltySchemes.SelectedValue = objBaseparty.ClientDetail.LoyaltyScheme(gvloyalty.SelectedIndex).LoyaltySchemeCode
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
                objBaseparty = Session("GetPartyResponse")
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


        objBaseparty = Session("GetPartyResponse")
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
                objBaseparty = Session("GetPartyResponse")
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

    Protected Sub save_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles save.Click
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
            With oAddPartyResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Throw New SamResponseException(.Errors)
                Else
                    txtClientCode.Text = oAddPartyResponseType.Shortname
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
End Class
