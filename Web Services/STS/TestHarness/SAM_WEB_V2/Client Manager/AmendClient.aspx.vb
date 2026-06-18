Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data.SqlTypes
Imports System.Data


Partial Class MTA_AmendClient
    Inherits System.Web.UI.Page
    Dim strClient As String
    Dim objBaseparty As New Object
    Dim oGetPartyResponseType As New GetPartyResponseType
    Dim oAddPartyRequest As New AddPartyRequestType
    Dim objitem As New BasePartyType

    

    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, _
              ByVal e As MenuEventArgs) Handles Menu1.MenuItemClick
        Mv.ActiveViewIndex = Int32.Parse(e.Item.Value)



    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'create user token from credentials
        'normally the credentials would come from the log in

        Try
            strClient = Session("Client")
            Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
            If Not IsPostBack Then


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
                Dim oGetPartyResponseType As New GetPartyResponseType

                'get the quote response
                Dim quote As AddQuoteResponseType = CType(Session("quote"), AddQuoteResponseType)

                'set up request object with some values
                'Session("PartyKey") = 35

                With oGetPartyRequestType
                    .PartyKey = Session("PartyKey") 'CInt(Session("PARTYKEY").ToString)
                    .BranchCode = "Headoff" 'Session("BRANCHCODE").ToString
                End With

                'start vijay For lookups


                Dim oRequest As New GetListRequestType
                Dim oResponse As New GetListResponseType


                'Saurabh -- Start DataBind to Dropdowns
                '''Binding Claim Handler Drop down - 

                oRequest.BranchCode = "HeadOff"
                oRequest.ListType = STSListType.UserDefinedTable
                oRequest.ListCode = "131085"
                Try
                    oResponse = oSAM.GetList(oRequest)

                    With oResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        Else
                            ddltitle.DataSource = oResponse.List
                            ddltitle.DataTextField = "Description"
                            ddltitle.DataValueField = "Code"
                            ddltitle.DataBind()
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
                Try
                    oResponse = oSAM.GetList(oRequest)

                    With oResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        Else
                            ddlturnover.DataSource = oResponse.List
                            ddlturnover.DataTextField = "Description"
                            ddlturnover.DataValueField = "Key"
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
                            ddlbusiness.Items.Add(lstitem)
                            ddlbusiness.AppendDataBoundItems = True
                            ddlbusiness.DataBind()
                            ddlbusiness.DataSource = oResponse.List
                            ddlbusiness.DataTextField = "Description"
                            ddlbusiness.DataValueField = "Key"
                            ddlbusiness.DataBind()

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
                            ddlSic.Items.Add(lstitem)
                            ddlSic.AppendDataBoundItems = True
                            ddlSic.DataBind()
                            ddlSic.DataSource = oResponse.List
                            ddlSic.DataTextField = "Description"
                            ddlSic.DataValueField = "Key"
                            ddlSic.DataBind()

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
                            ddlNoofEmployees.Items.Add(lstitem)
                            ddlNoofEmployees.AppendDataBoundItems = True
                            ddlNoofEmployees.DataBind()
                            ddlNoofEmployees.DataSource = oResponse.List
                            ddlNoofEmployees.DataTextField = "Description"
                            ddlNoofEmployees.DataValueField = "Key"
                            ddlNoofEmployees.DataBind()

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
                Try
                    oResponse = oSAM.GetList(oRequest)

                    With oResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        Else
                            ddlPriEmpBusiness.DataSource = oResponse.List
                            ddlPriEmpBusiness.DataTextField = "Description"
                            ddlPriEmpBusiness.DataValueField = "Code"
                            ddlPriEmpBusiness.DataBind()


                            ddlSecEmpsBusiness.DataSource = oResponse.List
                            ddlSecEmpsBusiness.DataTextField = "Description"
                            ddlSecEmpsBusiness.DataValueField = "Code"
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
                Try
                    oResponse = oSAM.GetList(oRequest)

                    With oResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        Else
                            ddloccupationcode.DataSource = oResponse.List
                            ddloccupationcode.DataTextField = "Description"
                            ddloccupationcode.DataValueField = "Code"
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
                Try
                    oResponse = oSAM.GetList(oRequest)

                    With oResponse
                        If Not (.Errors) Is Nothing Then
                            'errors returned, so throw an exception
                            Throw New SamResponseException(.Errors)
                        Else
                            ddlsecoccupationcode.DataSource = oResponse.List
                            ddlsecoccupationcode.DataTextField = "Description"
                            ddlsecoccupationcode.DataValueField = "Code"
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




                If strClient = "PC" Then
                    GVContacts.Visible = True

                    txtgrname.Enabled = False
                    txtgrtype.Enabled = False
                    txtwage.Enabled = False
                    'txtturnover.Enabled = False
                    ddlturnover.Enabled = False
                    txtfinancial.Enabled = False



                    lblArea.Visible = True
                    ddlArea.Visible = True
                    lblFileCode.Visible = True
                    txtFileCode.Visible = True

                    lblBusiness.Visible = False
                    ddlbusiness.Visible = False
                    lblTrade.Visible = False
                    ddlTrade.Visible = False
                    lblSICcode.Visible = False
                    ddlSic.Visible = False
                    lblNoofEmployees.Visible = False
                    ddlNoofEmployees.Visible = False
                    lblNoOfOffices.Visible = False
                    txtNoOfOffices.Visible = False
                    lblTradeSince.Visible = False
                    txtTradeSince.Visible = False
                    lblMainContact.Visible = False
                    txtMainContact.Visible = False

                    pnlCharityDetails.Visible = False
                    lblCharityDetails.Visible = False

                    txtCompanyName.Visible = False
                    lblCompanyName.Visible = False

                    txtMainContact.Visible = False
                    lblMainContact.Visible = False
                    txtAccbalance.Text = ""
                    txtYearTodateTurnOver.Text = ""
                    txtLastYearturnOver.Text = ""

                    txtgrname.Visible = False
                    lblGroupName.Visible = False
                    txtgrtype.Visible = False
                    lblGroupType.Visible = False
                    txtPIname.Text = ""
                    txtPIname.Visible = False
                    'txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerKey
                    txtPBname.Text = ""
                    txtPBname.Visible = False



                End If

                If strClient = "CC" Then 'oGetPartyResponseType.Item IsNot Nothing AndAlso _
                    'oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then

                    'Dim objBaseparty As New BasePartyCCType
                    'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType
                    txtClientCode.Visible = False
                    txtLastName.Visible = False
                    txtForeName.Visible = False
                    ddltitle.Visible = False
                    txtInitial.Visible = False
                    txtgrname.Visible = False
                    txtgrtype.Visible = False
                    lblTitle.Visible = False
                    'VijayakumarStart

                    lblArea.Visible = False
                    ddlArea.Visible = False
                    lblFileCode.Visible = False
                    txtFileCode.Visible = False

                    lblBusiness.Visible = True
                    ddlbusiness.Visible = True
                    lblTrade.Visible = True
                    ddlTrade.Visible = True
                    lblSICcode.Visible = True
                    ddlSic.Visible = True
                    lblNoofEmployees.Visible = True
                    ddlNoofEmployees.Visible = True
                    lblNoOfOffices.Visible = True
                    txtNoOfOffices.Visible = True
                    lblTradeSince.Visible = True
                    txtTradeSince.Visible = True
                    lblMainContact.Visible = False
                    txtMainContact.Visible = False
                    pnlCharityDetails.Visible = False
                    lblCharityDetails.Visible = False

                    txtAccbalance.Text = ""
                    txtYearTodateTurnOver.Text = ""
                    txtLastYearturnOver.Text = ""

                    txtCompanyName.Visible = True
                    lblCompanyName.Visible = True

                    txtMainContact.Visible = True
                    lblMainContact.Visible = True

                    lblLastName.Visible = False
                    txtLastName.Visible = False
                    txtForeName.Visible = False
                    lblForeName.Visible = False
                    lblGroupName.Visible = False
                    lblGroupType.Visible = False



                    lblInitials.Visible = False
                    txtInitial.Visible = False




                    ddlPriOccupation.Enabled = False
                    ddlPriEmpBusiness.Enabled = False
                    ddlPriStatus.Enabled = False
                    ddlSecOccupation.Enabled = False
                    ddlSecEmpsBusiness.Enabled = False
                    ddlSecStatus.Enabled = False


                    'lblBusiness.Visible = True
                    'ddlbusiness.Visible = True
                    'lblTrade.Visible = True
                    'ddlTrade.Visible = True
                    'lblSICcode.Visible = True
                    'ddlSic.Visible = True
                    'lblTrade.Visible = True
                    'ddlTrade.Visible = True
                    'lblNoOfOffices.Visible = True
                    'txtNoOfOffices.Visible = True
                    'lblNoofEmployees.Visible = True
                    'ddlNoofEmployees.Visible = True
                    'txtsalutation.Text = objBaseparty.Salutation
                    'txtpreferedcorr.Text = objBaseparty.ClientDetail.CorrespondenceCode
                    ''txtIsTps.Text = objBaseparty.TPS
                    ''txtIsMps.Text = objBaseparty.MPS
                    ''txtIseMps.Text = objBaseparty.eMPS
                    'chkTPS.Checked = objBaseparty.TPS
                    'chkMPS.Checked = objBaseparty.MPS
                    'chkeMPS.Checked = objBaseparty.eMPS
                    ''txtIsprospect.Text = objBaseparty.ClientDetail.IsProspect
                    'chkIsprospect.Checked = objBaseparty.ClientDetail.IsProspect
                    '' txtIsagent.Text = objBaseparty.ClientDetail.IsAgent
                    'chkIsagent.Checked = objBaseparty.ClientDetail.IsAgent
                    'If objBaseparty.Currency IsNot Nothing AndAlso objBaseparty.Currency <> "" Then
                    '    ddlCurrency.Items.FindByText(objBaseparty.Currency).Selected = True
                    'End If
                    '' txtpayment.Text = objBaseparty.ClientDetail.PaymentCode
                    'If objBaseparty.ClientDetail.PaymentCode IsNot Nothing AndAlso objBaseparty.ClientDetail.PaymentCode <> "" Then
                    '    ddlPaymentMethod.Items.FindByText(objBaseparty.ClientDetail.PaymentCode).Selected = True
                    'End If

                    ' txtreminder.Text = objBaseparty.ClientDetail.ReminderCode
                    'If objBaseparty.ClientDetail.ReminderCode IsNot Nothing AndAlso objBaseparty.ClientDetail.ReminderCode <> "" Then
                    '    ddlReminderType.Items.FindByValue(objBaseparty.ClientDetail.ReminderCode).Selected = True
                    'End If





                    'txttermspay.Text = objBaseparty.ClientDetail.PaymentTermCode
                    '' txtrenewal.Text = objBaseparty.ClientDetail.RenewalStopCode
                    'If objBaseparty.ClientDetail.RenewalStopCode IsNot Nothing AndAlso objBaseparty.ClientDetail.RenewalStopCode <> "" Then
                    '    ddlrenewaldtopcode.Items.FindByValue(objBaseparty.ClientDetail.RenewalStopCode).Selected = True
                    'End If
                    'txtsource.Text = objBaseparty.Source



                    'gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                    'gvassociates.DataBind()

                    'gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    'gvConcictions.DataBind()

                    txtDobirth.Enabled = False
                    'txtmatital.Enabled = False
                    ddmaritalstatus.Enabled = False

                    'txtseasonal.Text = objBaseparty.ClientDetail.SeasonalGiftCode
                    'If objBaseparty.ClientDetail.SeasonalGiftCode IsNot Nothing AndAlso objBaseparty.ClientDetail.SeasonalGiftCode <> "" Then
                    '    ddseasonalgift.Items.FindByValue(objBaseparty.ClientDetail.SeasonalGiftCode).Selected = True
                    'End If
                    'txtloyalty.Text = objBaseparty.ClientDetail.LoyaltyNumber
                    pnlLifeStyle.Visible = False



                    'gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
                    'gvloyalty.DataBind()



                    'txtagentref.Text = objBaseparty.ClientDetail.AgentReference
                    'txtcurrentagent.Text = objBaseparty.ClientDetail.CurrentIntermediaryKey
                    '' txtstrength.Text = objBaseparty.ClientDetail.StrengthCode
                    'If objBaseparty.ClientDetail.StrengthCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StrengthCode <> "" Then
                    '    ddlprosStrengthCode.Items.FindByText(objBaseparty.ClientDetail.StrengthCode).Selected = True
                    'End If
                    '' txtpstatus.Text = objBaseparty.ClientDetail.StatusCode
                    'If objBaseparty.ClientDetail.StatusCode IsNot Nothing AndAlso objBaseparty.ClientDetail.StatusCode <> "" Then
                    '    ddlprosStatus.Items.FindByText(objBaseparty.ClientDetail.StatusCode).Selected = True
                    'End If
                    'txtPIcode.Text = objBaseparty.ClientDetail.PreviousInsurerKey
                    txtPIname.Text = ""
                    txtPIname.Enabled = False
                    'txtPBcode.Text = objBaseparty.ClientDetail.PreviousBrokerKey
                    txtPBname.Text = ""
                    txtPBname.Enabled = False

                    'gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
                    'gvpolicies.DataBind()

                    'txttaxno.Text = objBaseparty.TaxNumber
                    'txtisdomicile.Text = objBaseparty.DomiciledForTax
                    'chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                    'txtexempt.Text = objBaseparty.TaxExempt
                    'txtpercentage.Text = objBaseparty.TaxPercentage

                End If
                If strClient = "GC" Then 'oGetPartyResponseType.Item IsNot Nothing AndAlso _
                    'oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then


                    Dim objBaseparty As New BasePartyOTHERType
                    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)

                    txtClientCode.Enabled = False
                    txtLastName.Visible = False
                    txtForeName.Visible = False
                    ddltitle.Visible = False
                    txtInitial.Visible = False
                    txtwage.Visible = False
                    'txtturnover.Enabled = False
                    ddlturnover.Enabled = False
                    txtfinancial.Enabled = False
                    txtsource.Enabled = False

                    txtgrname.Text = objBaseparty.Name
                    txtgrtype.Text = objBaseparty.TypeCode

                    'GVContacts.DataSource = objBaseparty.Addresses
                    'GVContacts.DataBind()

                    'GVContacts1.DataSource = objBaseparty.Contacts
                    'GVContacts1.DataBind()


                    pnlcorrespondence.Visible = False

                    'If Not objBaseparty.Currency Is Nothing Then
                    '    ddlCurrency.Items.FindByValue(objBaseparty.Currency).Selected = True
                    'End If
                    pnlemp.Visible = False


                    btnAssosicates.Visible = False

                    'gvConcictions.DataSource = objBaseparty.Convictions
                    'gvConcictions.DataBind()

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
                    'txtisdomicile.Text = objBaseparty.DomiciledForTax
                    'chkDomicileTax.Checked = objBaseparty.DomiciledForTax
                    txtexempt.Text = objBaseparty.TaxExempt
                    txtpercentage.Text = objBaseparty.TaxPercentage

                End If

            End If


            'SAMHelper.setTextFieldsInGridView(GVContacts1)
            'SAMHelper.setTextFieldsInGridView(gvassociates)
            'SAMHelper.setTextFieldsInGridView(gvConcictions)
            'SAMHelper.setTextFieldsInGridView(gvDependents)
            'SAMHelper.setTextFieldsInGridView(gvloyalty)
            'SAMHelper.setTextFieldsInGridView(gvpolicies)
            'SAMHelper.setTextFieldsInGridView(GVContacts)






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

    Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        pnlAddConvictions.Visible = True
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        pnlAddConvictions.Visible = False

    End Sub

    Protected Sub gvDependents_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDependents.SelectedIndexChanged

    End Sub

    Protected Sub btnLSOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLSOk.Click
        Try
            ' Dim objBaseparty As New Object
            'objBaseparty = setobject()
            objBaseparty = Session("AddParty")
            If objBaseparty Is Nothing Then
                objBaseparty = setobject()
                'objBaseparty.ClientDetail = objClientDetail
            End If


            Dim objLS As New BasePartyPCTypeLifestyle
            objLS.Name = txtName.Text
            If IsDate(txtDOB.Text) Then
                objLS.DateOfBirth = txtDOB.Text
            Else
                objLS.DateOfBirth = SqlDateTime.Null
            End If

            'objLS.CategoryCode = txtcategorycode.Text
            objLS.CategoryCode = ddlcategory.SelectedValue

            'objLS.GenderCode = ddgender.SelectedItem.ToString()
            objLS.GenderCode = GenderCodeType.F
            objLS.OccupationCode = ddlPriOccupation.SelectedItem.ToString
            objLS.SecOccupationCode = ddlSecOccupation.SelectedItem.ToString

            'objLS.Smoker = txtissmoker.Text
            If chkIssmoker.Checked = True Then
                objLS.Smoker = True
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
            Session("AddParty") = objBaseparty

            gvDependents.DataSource = objBaseparty.Lifestyle
            gvDependents.DataBind()
            pnlAddDependents.Visible = False
            'SAMHelper.setTextFieldsInGridView(gvDependents)

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
    End Sub

    Protected Sub btnAddPolicies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPolicies.Click
        pnlAddPolicies.Visible = True
    End Sub

    Protected Sub btnLoyaltyAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLoyaltyAdd.Click
        pnlAddLoyaltySchemes.Visible = True
    End Sub

    Protected Sub btnAddLoyaltyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLoyaltyCancel.Click
        pnlAddLoyaltySchemes.Visible = False

    End Sub

    Protected Sub btnPolicyCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPolicyCancel.Click
        pnlAddPolicies.Visible = False
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload

    End Sub

    Protected Sub btnNewAssociates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAssociates.Click
        pnlNewAssociates.Visible = True


    End Sub

    Protected Sub btnNewAssociatesOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAssociatesOk.Click
        Try
            If gvassociates.SelectedIndex = -1 Then
                Dim objBaseparty As New Object
                objBaseparty = setobject()


                Dim objassociate As New BaseAssociateType
                objassociate.AssociateKey = txtclient.Text
                objassociate.RelationshipDescription = ddlrelationshipcode.SelectedItem.ToString()
                objassociate.RelationshipCode = ddlrelationshipcode.SelectedValue.ToString()
                objassociate.ClientKey = 180

                Dim vassociate() As BaseAssociateType
                'If objBaseparty.ClientDetail IsNot Nothing Then
              
                'If objBaseparty.ClientDetail.Associates IsNot Nothing Then
                '    ReDim vassociate(objBaseparty.ClientDetail.Associates.Length)
                '    objBaseparty.ClientDetail.Associates.CopyTo(vassociate, 0)
                '    vassociate(objBaseparty.ClientDetail.Associates.Length) = objassociate
                'Else
                '    ReDim vassociate(0)
                '    vassociate(0) = objassociate
                'End If

                'objBaseparty.ClientDetail.Associates = vassociate
                ' End If

                gvassociates.DataSource = objassociate
                gvassociates.DataBind()
                pnlNewAssociates.Visible = False
                'SAMHelper.setTextFieldsInGridView(gvassociates)
            Else
                Dim oGetPartyResponseType As New GetPartyResponseType

                oGetPartyResponseType = Session("GetPartyResponse")

                If oGetPartyResponseType.Item IsNot Nothing AndAlso _
                            oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
                    Dim objBaseparty As New BasePartyPCType
                    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey = Convert.ToInt32(txtclient.Text)
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

                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).AssociateKey = Convert.ToInt32(txtclient.Text)
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipDescription = ddlrelationshipcode.Text
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).RelationshipCode = ddlrelationshipcode.SelectedValue
                    objBaseparty.ClientDetail.Associates(gvassociates.SelectedIndex).ClientKey = 180
                    gvassociates.DataSource = objBaseparty.ClientDetail.Associates
                    gvassociates.DataBind()
                    oGetPartyResponseType.Item = objBaseparty
                End If


                Session("GetPartyResponse") = oGetPartyResponseType
                pnlNewAssociates.Visible = False
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

    Protected Sub btnNewAssociatesCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewAssociatesCancel.Click
        pnlNewAssociates.Visible = False

    End Sub

    Protected Sub btnAssosicates_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAssosicates.Click
        pnlAddAssociates.Visible = True
    End Sub



    Protected Sub GVContacts1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVContacts1.RowDeleting
        'Dim objBaseparty As New Object
        objBaseparty = setobject()

        SAMHelper.RemoveFromArray(Of BaseContactType)(objBaseparty.Contacts, e.RowIndex)
        GVContacts1.DataSource = objBaseparty.Contacts
        GVContacts1.DataBind()


    End Sub
    Protected Sub GVContacts_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GVContacts.RowDeleting
        'Dim objBaseparty As New Object
        objBaseparty = setobject()

        SAMHelper.RemoveFromArray(Of BaseAddressWithContactsType)(objBaseparty.Addresses, e.RowIndex)
        GVContacts.DataSource = objBaseparty.Addresses
        GVContacts.DataBind()
        ' SAMHelper.setTextFieldsInGridView(GVContacts)


    End Sub

    Protected Sub gvassociates_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvassociates.RowDeleting
        'Dim objBaseparty As New Object
        objBaseparty = setobject()

        SAMHelper.RemoveFromArray(Of BaseAssociateType)(objBaseparty.ClientDetail.Associates, e.RowIndex)
        gvassociates.DataSource = objBaseparty.ClientDetail.Associates
        gvassociates.DataBind()
        'SAMHelper.setTextFieldsInGridView(gvassociates)

    End Sub
    Protected Sub gvConcictions_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvConcictions.RowDeleting
        'Dim objBaseparty As New Object
        objBaseparty = setobject()
        SAMHelper.RemoveFromArray(Of BaseConvictionType)(objBaseparty.ClientDetail.Convictions, e.RowIndex)
        gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
        gvConcictions.DataBind()
        'SAMHelper.setTextFieldsInGridView(gvConcictions)

    End Sub
    Protected Sub gvDependents_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDependents.RowDeleting

        'Dim objBaseparty As New Object
        objBaseparty = setobject()

        SAMHelper.RemoveFromArray(Of BasePartyPCTypeLifestyle)(objBaseparty.Lifestyle, e.RowIndex)
        gvDependents.DataSource = objBaseparty.Lifestyle
        gvDependents.DataBind()
        ' SAMHelper.setTextFieldsInGridView(gvDependents)

    End Sub
    Protected Sub gvloyalty_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvloyalty.RowDeleting
        'Dim objBaseparty As New Object
        objBaseparty = setobject()

        SAMHelper.RemoveFromArray(Of BaseClientSharedDataTypeLoyaltyScheme)(objBaseparty.ClientDetail.LoyaltyScheme, e.RowIndex)
        gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
        gvloyalty.DataBind()
        'SAMHelper.setTextFieldsInGridView(gvloyalty)

    End Sub
    Protected Sub gvpolicies_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvpolicies.RowDeleting
        'Dim objBaseparty As New Object
        objBaseparty = setobject()

        SAMHelper.RemoveFromArray(Of BaseClientSharedDataTypeProspectPolicies)(objBaseparty.ClientDetail.ProspectPolicies, e.RowIndex)
        gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
        gvpolicies.DataBind()
        'SAMHelper.setTextFieldsInGridView(gvpolicies)

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
        lblcref.Text = ""
        txtareacode.Text = ""
        txtextension.Text = ""
        lblcpostcode.Text = ""
        txtdescription.Text = ""
        txtNumber.Text = ""
        pnlAddAddress.Visible = False
    End Sub

    Protected Sub btnaddaddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddaddress.Click
        pnlAddAddress.Visible = True
        txtstname.Text = ""
        lblref.Text = ""
        txtposttown.Text = ""
        txtpostcode.Text = ""
        txtlocality.Text = ""
        txtcounty.Text = ""
        lblpostcode.Text = ""
        plnAddContact.Visible = False
    End Sub

    Protected Sub btnaok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaok.Click
        Try
            If (GVContacts.SelectedIndex = -1) Then
                'Dim objBaseparty As New Object
                'objBaseparty = setobject()
                objBaseparty = Session("AddParty")
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If




                Dim objaddr As New BaseAddressWithContactsType
                'Dim objitem As  New BasePartyType
                'objaddr.AddressTypeCode = txttype.Text
                objaddr.AddressTypeCode = CType(ddlconType.SelectedIndex, AddressTypeType)
                If ddlconType.SelectedItem.Text = "Correspondence Address" Then
                    objaddr.AddressTypeCode = AddressTypeType.Item3131XCO
                End If



                'If objBaseparty Is Nothing Then
                '    objBaseparty = New BasePartyPCType()
                '    objBaseparty.Item = objitem
                'End If

                objaddr.AddressLine1 = txtstname.Text
                objaddr.AddressLine3 = txtposttown.Text
                objaddr.PostCode = txtpostcode.Text
                objaddr.AddressLine2 = txtlocality.Text
                objaddr.AddressLine4 = txtcounty.Text
                'objaddr.CountryCode = txtcountry.Text
                objaddr.CountryCode = ddlconCountry.SelectedValue.ToString

                Dim vAddresses() As BaseAddressWithContactsType
                If objBaseparty IsNot Nothing And objBaseparty.Addresses IsNot Nothing Then
                    ReDim vAddresses(objBaseparty.Addresses.Length)
                    objBaseparty.Addresses.CopyTo(vAddresses, 0)
                    vAddresses(objBaseparty.Addresses.Length) = objaddr
                Else
                    ReDim vAddresses(0)
                    vAddresses(0) = objaddr
                End If

                objBaseparty.Addresses = vAddresses
                Session("AddParty") = objBaseparty
                GVContacts.DataSource = objBaseparty.Addresses
                GVContacts.DataBind()
                pnlAddAddress.Visible = False
                'SAMHelper.setTextFieldsInGridView(GVContacts)
            Else

                Dim oGetPartyResponseType As New BaseAddressWithContactsType

                objBaseparty = Session("AddParty")

                oGetPartyResponseType = objBaseparty.Addresses(GVContacts.SelectedIndex)

                oGetPartyResponseType.AddressLine1 = txtstname.Text
                oGetPartyResponseType.AddressLine3 = txtposttown.Text
                oGetPartyResponseType.PostCode = txtpostcode.Text
                oGetPartyResponseType.AddressLine2 = txtlocality.Text
                oGetPartyResponseType.AddressLine4 = txtcounty.Text
                oGetPartyResponseType.CountryCode = ddlconCountry.SelectedValue
                pnlAddAddress.Visible = False
                Session("AddParty") = objBaseparty
                GVContacts.DataSource = objBaseparty.Addresses
                GVContacts.DataBind()
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

    Protected Sub btncok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btncok.Click
        Try
            'Dim objBaseparty As New Object
            'objBaseparty = setobject()
            If (GVContacts1.SelectedIndex = -1) Then
                Dim objBasepartyType As BasePartyPCType

                objBaseparty = Session("AddParty")
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                End If

                Dim objcontact As New BaseContactType

                Dim obj As New BaseContactDetailType
                'oAddPartyRequest.Item.Contacts(0).ContactDetail
                obj = New BaseContactDetailType
                'obj.Item = txtcontact.Text
                obj.Item = ddlconContact.SelectedValue.ToString()
                obj.ItemElementName = ItemChoiceType.EmailAddress
                objcontact.ContactDetail = obj
                objcontact.AreaCode = txtareacode.Text
                objcontact.Description = txtdescription.Text

                objcontact.ContactTypeCode = ContactTypeType.EMAIL
                'oAddPartyRequest = Session("AddParty")

                'If objBaseparty Is Nothing Then
                '    objBaseparty = setobject()
                '    'oAddPartyRequest = New AddPartyRequestType
                '    'objitem = New BasePartyType
                '    'oAddPartyRequest.Item = objitem
                '    oAddPartyRequest.Item.Contacts(0) = objcontact
                '    'Else
                '    '    dim iContact as Integer = oAddPartyRequest.Item.Contacts.GetUpperBound(0)
                'End If
                'objBaseparty.Contacts = Session("contact")



                Dim vContact() As BaseContactType

                objBaseparty.Contacts()
                If objBaseparty.Contacts IsNot Nothing Then
                    ReDim vContact(objBaseparty.Contacts.Length)
                    objBaseparty.Contacts.CopyTo(vContact, 0)
                    vContact(objBaseparty.Contacts.Length) = objcontact
                Else
                    ReDim vContact(0)
                    vContact(0) = objcontact
                End If

                objBaseparty.Contacts = vContact
                'objBaseparty.Addresses(0).Contacts = vContact
                Session("AddParty") = objBaseparty



                GVContacts1.DataSource = objBaseparty.Contacts
                GVContacts1.DataBind()
                plnAddContact.Visible = False
                'SAMHelper.setTextFieldsInGridView(GVContacts1)
            Else
                Dim oGetPartyResponseType As New BaseContactType

                objBaseparty = Session("AddParty")


                oGetPartyResponseType = objBaseparty.Contacts(GVContacts1.SelectedIndex)

                oGetPartyResponseType.AreaCode = txtareacode.Text
                oGetPartyResponseType.Description = txtdescription.Text
               
                pnlAddAddress.Visible = False
                Session("AddParty") = objBaseparty
                GVContacts1.DataSource = objBaseparty.Contacts
                GVContacts1.DataBind()
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

    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If gvConcictions.SelectedIndex = -1 Then
                Dim objClientDetail As New BaseClientSharedDataType
                objBaseparty = Session("AddParty")
                If objBaseparty Is Nothing Then
                    objBaseparty = setobject()
                    objBaseparty.ClientDetail = objClientDetail
                Else
                    If objBaseparty.ClientDetail Is Nothing Then
                        objBaseparty.ClientDetail = objClientDetail
                    End If
                End If
                'Dim objBaseparty1 As New BasePartyCCType
                'objBaseparty1.ClientDetail.Convictions
                'objBaseparty = setobject()




                Dim objconv As New BaseConvictionType
                objconv.TypeCode = ddlconvictiontype.SelectedItem.ToString
                objconv.StatusCode = ddlconvictionstatus.SelectedItem.ToString
                objconv.Description = txtcdescription.Text
                If txtcfine.Text <> "" Then
                    objconv.FineAmount = Convert.ToDecimal(txtcfine.Text)
                End If
                If (IsDate(txtcdate.Text)) Then
                    objconv.Date = txtcdate.Text
                Else
                    objconv.Date = SqlDateTime.Null
                End If
                'objconv.SentenceTypeCode = txtstype.Text
                objconv.SentenceTypeCode = ddlsentencetype.SelectedItem.ToString
                objconv.SentenceDescription = txtsdescription.Text
                If (IsDate(txtsdate.Text)) Then
                    objconv.SentenceEffectiveDate = txtsdate.Text
                Else
                    objconv.SentenceEffectiveDate = SqlDateTime.Null
                End If
                If txtsduration.Text <> "" Then
                    objconv.SentenceDuration = Convert.ToDecimal(txtsduration.Text)
                End If

                'objconv.SentenceDurationQualifier = txtstime.Text
                objconv.SentenceDurationQualifier = ddlsentencetime.SelectedItem.ToString
                'objconv.AlcoholMeasurementMethod = txtmethod.Text
                objconv.AlcoholMeasurementMethod = ddlAlcoholmethod.SelectedItem.ToString
                If txtalcohollevel.Text <> "" Then
                    objconv.AlcoholLevel = Convert.ToDecimal(txtalcohollevel.Text)
                End If
                If txtpenality.Text <> "" Then
                    objconv.DrivingLicensePenaltyPoints = Convert.ToDecimal(txtpenality.Text)
                End If

                Dim vConviction() As BaseConvictionType = Nothing
                If objBaseparty.GetType Is GetType(BasePartyOTHERType) Then
                    If objBaseparty.ClientDetail.Convictions IsNot Nothing Then
                        ReDim vConviction(objBaseparty.Convictions.Length)
                        objBaseparty.ClientDetail.Convictions.CopyTo(vConviction, 0)
                        vConviction(objBaseparty.Convictions.Length) = objconv
                    Else
                        ReDim vConviction(0)
                        vConviction(0) = objconv
                    End If
                    objBaseparty.ClientDetail.Convictions = vConviction
                    Session("AddParty") = objBaseparty
                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()
                    pnlAddConvictions.Visible = False
                    'SAMHelper.setTextFieldsInGridView(gvConcictions)


                Else


                    'If objBaseparty.ClientDetail IsNot Nothing Then
                    If objBaseparty.ClientDetail.Convictions IsNot Nothing Then
                        ReDim vConviction(objBaseparty.ClientDetail.Convictions.Length)
                        objBaseparty.ClientDetail.Convictions.CopyTo(vConviction, 0)
                        vConviction(objBaseparty.ClientDetail.Convictions.Length) = objconv
                        'End If
                    Else
                        ReDim vConviction(0)
                        vConviction(0) = objconv
                    End If

                    objBaseparty.ClientDetail.Convictions = vConviction

                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()
                    pnlAddConvictions.Visible = False
                    'SAMHelper.setTextFieldsInGridView(gvConcictions)
                    Session("AddParty") = objBaseparty
                End If
            Else
                Dim objBaseparty As New Object
                objBaseparty = setobject()

                'Dim objBaseparty As New BasePartyPCType
                'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

                Dim objconv As New BaseConvictionType
                objBaseparty = Session("AddParty")


                objconv = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex)

                objconv.TypeCode = ddlconvictiontype.SelectedItem.ToString
                objconv.StatusCode = ddlconvictionstatus.SelectedItem.ToString
                objconv.Description = txtcdescription.Text
                If txtcfine.Text <> "" Then
                    objconv.FineAmount = Convert.ToDecimal(txtcfine.Text)
                End If

                If (IsDate(txtcdate.Text)) Then
                    objconv.Date = txtcdate.Text
                Else
                    objconv.Date = SqlDateTime.Null
                End If

                objconv.SentenceTypeCode = ddlsentencetype.SelectedItem.ToString
                objconv.SentenceDescription = txtsdescription.Text
                If (IsDate(txtsdate.Text)) Then
                    objconv.SentenceEffectiveDate = txtsdate.Text
                Else
                    objconv.SentenceEffectiveDate = SqlDateTime.Null
                End If

                objconv.SentenceDuration = txtsduration.Text
                'objconv.SentenceDurationQualifier = txtstime.Text
                objconv.SentenceDurationQualifier = ddlsentencetime.SelectedItem.ToString
                'objconv.AlcoholMeasurementMethod = txtmethod.Text
                objconv.AlcoholMeasurementMethod = ddlAlcoholmethod.SelectedItem.ToString
                If txtalcohollevel.Text <> "" Then
                    objconv.AlcoholLevel = Convert.ToDecimal(txtalcohollevel.Text)
                End If
                If txtpenality.Text <> "" Then
                    objconv.DrivingLicensePenaltyPoints = txtpenality.Text
                End If


                If objBaseparty.GetType Is GetType(BasePartyOTHERType) Then

                    'objBaseparty.Convictions(gvConcictions.SelectedIndex) = objconv
                    'gvConcictions.DataSource = objBaseparty.Convictions
                    'gvConcictions.DataBind()
                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()


                Else
                    pnlAddConvictions.Visible = False
                    Session("AddParty") = objBaseparty

                    gvConcictions.DataSource = objBaseparty.ClientDetail.Convictions
                    gvConcictions.DataBind()

                End If

                Session("AddParty") = objBaseparty
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

    Protected Sub btnAddLoyaltyAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddLoyaltyAdd.Click
        Try
            'Dim objBaseparty As New Object
            'objBaseparty = setobject()

            objBaseparty = Session("AddParty")
            If objBaseparty Is Nothing Then
                objBaseparty = setobject()
            End If

            Dim objloyalty As New BaseClientSharedDataTypeLoyaltyScheme
            objloyalty.LoyaltySchemeCode = txtloyaltyscheme.Text
            objloyalty.MembershipNumber = txtmembership.Text
            objloyalty.OtherReference = txtotherref.Text
            If (IsDate(txtstart.Text)) Then
                objloyalty.StartDate = txtstart.Text
            Else
                objloyalty.StartDate = SqlDateTime.Null
            End If
            If (IsDate(txtend.Text)) Then
                objloyalty.EndDate = txtend.Text
            Else
                objloyalty.EndDate = SqlDateTime.Null
            End If

            objloyalty.MainMember = txtMain.Text
            'objloyalty.Active = txtisactive.Text
            If chkActive.Checked = True Then
                objloyalty.Active = True
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

            If objBaseparty Is Nothing Then
                objBaseparty = setobject()
            End If
            gvloyalty.DataSource = objBaseparty.ClientDetail.LoyaltyScheme
            gvloyalty.DataBind()
            pnlAddLoyaltySchemes.Visible = False
            'SAMHelper.setTextFieldsInGridView(gvloyalty)
            Session("AddParty") = objBaseparty
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
            'Dim objBaseparty As New Object
            'objBaseparty = setobject()
            objBaseparty = Session("AddParty")
            If objBaseparty Is Nothing Then
                objBaseparty = setobject()
            End If


            Dim objProspect As New BaseClientSharedDataTypeProspectPolicies
            objProspect.ProspectTypeCode = ddlprostype.SelectedValue.ToString()
            If (IsDate(txtprewnal.Text)) Then
                objProspect.RenewalDate = txtprewnal.Text
            Else
                objProspect.RenewalDate = SqlDateTime.Null
            End If

            objProspect.TimesQuoted = txttimequoted.Text
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
            gvpolicies.DataSource = objBaseparty.ClientDetail.ProspectPolicies
            gvpolicies.DataBind()
            pnlAddPolicies.Visible = False
            ' SAMHelper.setTextFieldsInGridView(gvpolicies)
            Session("AddParty") = objBaseparty
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

        'Dim oGetPartyResponseType As New GetPartyResponseType


        objBaseparty = Session("AddParty")



        'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
        '   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
        If strClient = "PC" Then
            'Dim objBaseparty As New BasePartyPCType
            'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)


            objBaseparty.Surname = txtLastName.Text
            objBaseparty.Forename = txtForeName.Text
            objBaseparty.Title = ddltitle.SelectedItem.ToString()
            objBaseparty.Initials = txtInitial.Text
            objBaseparty.TradingName = txtTradingName.Text


            objBaseparty.Salutation = txtsalutation.Text
            objBaseparty.ClientDetail.CorrespondenceCode = txtpreferedcorr.Text
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
            objBaseparty.AccountExecutive = txtAccExecutiveCode.Text
            objBaseparty.AlternativeId = txtAlternativeIdentifier.Text
            If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
            End If
            If ddServiceLevel.SelectedValue <> "" Then
                objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedItem.Text
            End If

            objBaseparty.BranchCode = ddBranch.SelectedValue

            If ddlArea.SelectedValue <> 0 Then
                objBaseparty.ClientDetail.AreaCode = ddlArea.SelectedValue
            End If


            'VijayakumarEnd


            'objBaseparty.Currency = txtcurrency.Text
            objBaseparty.Currency = ddlCurrency.SelectedValue

            objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.ToString
            objBaseparty.ClientDetail.ReminderCode = ddlReminderType.SelectedValue
            objBaseparty.ClientDetail.PaymentTermCode = txttermspay.Text
            objBaseparty.ClientDetail.RenewalStopCode = ddlrenewaldtopcode.SelectedValue
            objBaseparty.Source = txtsource.Text
            objBaseparty.OccupationCode = ddlPriOccupation.SelectedItem.ToString
            objBaseparty.EmployersBusinessCode = ddlPriEmpBusiness.SelectedValue
            If ddlPriStatus.SelectedValue = 0 Then
                objBaseparty.EmploymentStatusCodeSpecified = False
            Else
                objBaseparty.EmploymentStatusCodeSpecified = True
            End If
            objBaseparty.EmploymentStatusCode = CType(ddlPriStatus.SelectedIndex - 1, EmploymentStatusCodeType)
            'EmploymentStatusCodeType()
            'objBaseparty.MaritalStatusCode = CType(ddlPriStatus.SelectedIndex, EmploymentStatusCodeType)

            objBaseparty.SecOccupationCode = ddlSecOccupation.SelectedItem.ToString
            objBaseparty.SecEmployersBusinessCode = ddlSecEmpsBusiness.SelectedItem.ToString
            If ddlSecStatus.SelectedValue = 0 Then
                objBaseparty.SecEmploymentStatusCodeSpecified = False
            Else
                objBaseparty.SecEmploymentStatusCodeSpecified = True
            End If
            ' objBaseparty.SecEmploymentStatusCode = ddlSecStatus.SelectedIndex
            objBaseparty.SecEmploymentStatusCode = CType(ddlSecStatus.SelectedIndex - 1, EmploymentStatusCodeType)



            If (IsDate(txtDobirth.Text)) Then
                objBaseparty.DateOfBirth = txtDobirth.Text
                objBaseparty.DateOfBirthSpecified = True
            Else
                objBaseparty.DateOfBirth = SqlDateTime.Null
                objBaseparty.DateOfBirthSpecified = False
            End If

            If ddmaritalstatus.SelectedValue = "0" Then
                objBaseparty.MaritalStatusCodeSpecified = False
            Else
                objBaseparty.MaritalStatusCodeSpecified = True
            End If
            objBaseparty.MaritalStatusCode = CType(ddmaritalstatus.SelectedIndex - 1, MaritalStatusCodeType)
            'objBaseparty.MaritalStatusCode = ddmaritalstatus.SelectedItem.ToString()
            objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
            objBaseparty.ClientDetail.LoyaltyNumber = txtloyalty.Text
            If txtpet.Text = "" Then
                objBaseparty.PetOwnerSpecified = False
            Else
                objBaseparty.PetOwnerSpecified = True
            End If
            'Ravi
            objBaseparty.PetOwner = False 'txtpet.Text
            objBaseparty.GenderCode = ddgender.SelectedItem.ToString()
            objBaseparty.NationalityCode = ddnationality.SelectedItem.ToString()
            objBaseparty.AccommodationCode = txtaccomodation.Text
            objBaseparty.Lifestyle(0).SmokerSpecified = True
            If chksmoker.Checked = True Then
                objBaseparty.Lifestyle(0).Smoker = True
            Else
                objBaseparty.Lifestyle(0).Smoker = False
            End If



            objBaseparty.ClientDetail.AgentReference = txtagentref.Text
            If txtcurrentagent.Text = "" Then
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
            Else
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
            End If

            'Ravi
            objBaseparty.ClientDetail.CurrentIntermediaryKey = 123 'Convert.ToInt32(txtcurrentagent.Text)
            objBaseparty.ClientDetail.StrengthCode = txtstrength.Text
            'objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue.ToString()
            objBaseparty.ClientDetail.StatusCode = txtpstatus.Text
            'objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue.ToString
            If txtPIcode.Text = "" Then
                objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
            Else
                objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
            End If
            'Ravi
            objBaseparty.ClientDetail.PreviousInsurerKey = 1 'txtPIcode.Text
            If txtPBcode.Text = "" Then
                objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
            Else
                objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
            End If
            'Ravi
            objBaseparty.ClientDetail.PreviousBrokerKey =1 'txtPBcode.Text

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

            If txtexempt.Text = "" Then
                objBaseparty.TaxExemptSpecified = False
            Else
                objBaseparty.TaxExemptSpecified = True
            End If
            objBaseparty.TaxExempt = txtexempt.Text
            If txtpercentage.Text = "" Then
                objBaseparty.TaxPercentageSpecified = False
            Else
                objBaseparty.TaxPercentageSpecified = True
            End If
            objBaseparty.TaxPercentage = txtpercentage.Text



            Dim icnt As Integer
            icnt = 0
            For Each row As GridViewRow In GVContacts.Rows

                objBaseparty.Addresses(icnt).AddressLine2 = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine3 = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).PostCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine1 = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine4 = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).CountryCode = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In GVContacts1.Rows


                Dim obj As BaseContactDetailType
                obj = New BaseContactDetailType
                obj.Item = "a.yahoo.com"
                obj.ItemElementName = ItemChoiceType.EmailAddress

                objBaseparty.Contacts(icnt).AreaCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.Contacts(icnt).ContactDetail = obj
                objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In gvassociates.Rows

                objBaseparty.ClientDetail.Associates(icnt).RelationshipCode = DirectCast(row.Cells(0).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Associates(icnt).RelationshipDescription = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Associates(icnt).ClientKey = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Associates(icnt).AssociateKey = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next
            'Session("Associates) = 

            icnt = 0
            For Each row As GridViewRow In gvConcictions.Rows

                objBaseparty.ClientDetail.Convictions(icnt).SentenceTypeCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                If DirectCast(row.Cells(5).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = True
                End If

                If DirectCast(row.Cells(1).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = True
                End If

                If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = True
                End If

                If DirectCast(row.Cells(12).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = True
                End If

                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = True
                End If



                objBaseparty.ClientDetail.Convictions(icnt).SentenceDuration = Convert.ToDecimal(DirectCast(row.Cells(2).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).TypeCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPoints = Convert.ToInt32(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).FineAmount = Convert.ToDecimal(DirectCast(row.Cells(6).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).Date = Convert.ToDateTime(DirectCast(row.Cells(7).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).ConvictionKey = Convert.ToInt32(DirectCast(row.Cells(8).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).AlcoholMeasurementMethod = DirectCast(row.Cells(9).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationQualifier = DirectCast(row.Cells(10).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).SentenceDescription = DirectCast(row.Cells(11).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).Description = DirectCast(row.Cells(12).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevel = Convert.ToInt32(DirectCast(row.Cells(13).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).StatusCode = DirectCast(row.Cells(14).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In gvDependents.Rows

                objBaseparty.Lifestyle(icnt).SecOccupationCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.Lifestyle(icnt).OccupationCode = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.Lifestyle(icnt).CategoryCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.Lifestyle(icnt).LifestyleKey = DirectCast(row.Cells(4).Controls(0), TextBox).Text


                If DirectCast(row.Cells(5).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Lifestyle(icnt).SmokerSpecified = False
                Else
                    objBaseparty.Lifestyle(icnt).SmokerSpecified = True
                End If

                objBaseparty.Lifestyle(icnt).Name = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                If DirectCast(row.Cells(6).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Lifestyle(icnt).Smoker = False
                Else
                    objBaseparty.Lifestyle(icnt).Smoker = True
                End If

                If DirectCast(row.Cells(6).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Lifestyle(icnt).DateOfBirthSpecified = False
                Else
                    objBaseparty.Lifestyle(icnt).DateOfBirthSpecified = True
                End If

                'If DirectCast(row.Cells(8).Controls(0), TextBox).Text = "" Then
                objBaseparty.Lifestyle(icnt).GenderCodeSpecified = True
                'Else
                'objBaseparty.Lifestyle(icnt).GenderCodeSpecified = DirectCast(row.Cells(8).Controls(0), TextBox).Text

                'End If

                objBaseparty.Lifestyle(icnt).DateOfBirth = Convert.ToDateTime(DirectCast(row.Cells(7).Controls(0), TextBox).Text)
                objBaseparty.Lifestyle(icnt).GenderCode = GenderCodeType.F

                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In gvloyalty.Rows

                objBaseparty.ClientDetail.LoyaltyScheme(icnt).MainMember = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).OtherReference = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDate = Convert.ToDateTime(DirectCast(row.Cells(3).Controls(0), TextBox).Text)
                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = False
                Else
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = True
                End If

                objBaseparty.ClientDetail.LoyaltyScheme(icnt).StartDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeKey = Convert.ToInt32(DirectCast(row.Cells(6).Controls(0), TextBox).Text)
                If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = False
                Else
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = True
                End If

                objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeCode = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).MembershipNumber = DirectCast(row.Cells(8).Controls(0), TextBox).Text

                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = False
                Else
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = True

                End If


                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In gvpolicies.Rows
                If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = False
                Else
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = True
                End If
                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = False
                Else
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = True
                End If


                objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectPolicyKey = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectTypeCode = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                If DirectCast(row.Cells(5).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = False
                Else
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = True
                End If

                objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuoted = Convert.ToInt32(DirectCast(row.Cells(3).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremium = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                icnt = icnt + 1

            Next
            Return objBaseparty


        End If

        If oGetPartyResponseType.Item IsNot Nothing AndAlso _
           oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then


            Dim objBaseparty As New BasePartyOTHERType
            objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)



            objBaseparty.Name = txtgrname.Text
            objBaseparty.TypeCode = txtgrtype.Text

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
            If txtexempt.Text = "" Then
                objBaseparty.TaxExemptSpecified = False
            Else
                objBaseparty.TaxExemptSpecified = True
            End If
            objBaseparty.TaxExempt = txtexempt.Text
            If txtpercentage.Text = "" Then
                objBaseparty.TaxPercentageSpecified = False
            Else
                objBaseparty.TaxPercentageSpecified = True
            End If
            objBaseparty.TaxPercentage = txtpercentage.Text



            Dim icnt As Integer
            icnt = 0
            For Each row As GridViewRow In GVContacts.Rows

                objBaseparty.Addresses(icnt).AddressLine2 = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine3 = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).PostCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine1 = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine4 = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).CountryCode = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In GVContacts1.Rows


                Dim obj As BaseContactDetailType
                obj = New BaseContactDetailType
                obj.Item = "a.yahoo.com"
                obj.ItemElementName = ItemChoiceType.EmailAddress

                objBaseparty.Contacts(icnt).AreaCode = DirectCast(row.Cells(0).Controls(0), TextBox).Text
                objBaseparty.Contacts(icnt).ContactDetail = obj
                objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                icnt = icnt + 1

            Next




            icnt = 0
            For Each row As GridViewRow In gvConcictions.Rows

                objBaseparty.Convictions(icnt).SentenceTypeCode = DirectCast(row.Cells(0).Controls(0), TextBox).Text
                If DirectCast(row.Cells(1).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Convictions(icnt).FineAmountSpecified = False
                Else
                    objBaseparty.Convictions(icnt).FineAmountSpecified = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                End If

                If DirectCast(row.Cells(2).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Convictions(icnt).SentenceDurationSpecified = False
                Else
                    objBaseparty.Convictions(icnt).SentenceDurationSpecified = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                End If

                If DirectCast(row.Cells(8).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                Else
                    objBaseparty.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = DirectCast(row.Cells(8).Controls(0), TextBox).Text
                End If

                If DirectCast(row.Cells(12).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Convictions(icnt).AlcoholLevelSpecified = False
                Else
                    objBaseparty.Convictions(icnt).AlcoholLevelSpecified = DirectCast(row.Cells(12).Controls(0), TextBox).Text
                End If

                If DirectCast(row.Cells(13).Controls(0), TextBox).Text = "" Then
                    objBaseparty.Convictions(icnt).SentenceEffectiveDateSpecified = False
                Else
                    objBaseparty.Convictions(icnt).SentenceEffectiveDateSpecified = DirectCast(row.Cells(13).Controls(0), TextBox).Text
                End If



                objBaseparty.Convictions(icnt).SentenceDuration = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).TypeCode = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).DrivingLicensePenaltyPoints = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).SentenceEffectiveDate = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).FineAmount = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).Date = DirectCast(row.Cells(9).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).ConvictionKey = DirectCast(row.Cells(10).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).AlcoholMeasurementMethod = DirectCast(row.Cells(11).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).SentenceDurationQualifier = DirectCast(row.Cells(14).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).SentenceDescription = DirectCast(row.Cells(15).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).Description = DirectCast(row.Cells(16).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).AlcoholLevel = DirectCast(row.Cells(17).Controls(0), TextBox).Text
                objBaseparty.Convictions(icnt).StatusCode = DirectCast(row.Cells(18).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next

            Return objBaseparty

        End If

        'uday
        If strClient = "CC" Then
            'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
            '      oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then


            'Dim objBaseparty As New BasePartyCCType
            'objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
            objBaseparty.CompanyName = txtCompanyName.Text
            objBaseparty.Companyreg = txtCompanyName.Text
            If txtwage.Text.Length > 0 Then
                objBaseparty.WageRoll = Convert.ToDecimal(txtwage.Text)
            Else

            End If
            'objBaseparty.TurnoverCode = txtturnover.Text
            objBaseparty.TurnoverCode = ddlturnover.SelectedValue
            If txtfinancial.Text.Length > 0 Then
                objBaseparty.FinancialYear = Convert.ToDateTime(txtfinancial.Text)
            End If
            objBaseparty.TradeCode = txtTradingName.Text

            objBaseparty.Salutation = txtsalutation.Text
            objBaseparty.ClientDetail.CorrespondenceCode = txtpreferedcorr.Text
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


            objBaseparty.AccountExecutive = txtAccExecutiveCode.Text
            If objBaseparty.ClientDetail.LeadAgentKeySpecified = True Then
                objBaseparty.ClientDetail.LeadAgentKey = hdLeadAgent.Value
            End If
            'txtAlternativeIdentifier.Text = objBaseparty.AlternativeId

            If ddServiceLevel.SelectedValue <> "" Then
                'objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedValue
                objBaseparty.ClientDetail.ServiceLevelCode = ddServiceLevel.SelectedItem.Text
            End If

            objBaseparty.BranchCode = ddBranch.SelectedValue

            If ddlbusiness.SelectedValue <> 0 Then
                objBaseparty.BusinessCode = ddlbusiness.SelectedItem.ToString()
            End If
            If ddlTrade.SelectedValue <> 0 Then
                objBaseparty.TradeCode = ddlTrade.SelectedItem.ToString()
            End If
            If ddlNoofEmployees.SelectedValue <> 0 Then
                objBaseparty.NumberOfEmployees = ddlNoofEmployees.SelectedValue
            End If

            If ddlSic.SelectedItem IsNot Nothing Then
                objBaseparty.SICCode = ddlSic.SelectedItem.ToString()
            End If
            If txtTradeSince.Text.Length > 0 Then
                objBaseparty.TradingSince = Convert.ToDateTime(txtTradeSince.Text)
            End If
            objBaseparty.NumberOfOffices = Convert.ToInt32(txtNoOfOffices.Text)

            'VijayakumarEnd


            objBaseparty.Currency = ddlCurrency.SelectedValue
            objBaseparty.ClientDetail.PaymentCode = ddlPaymentMethod.SelectedItem.ToString()
            objBaseparty.ClientDetail.ReminderCode = ddlReminderType.SelectedValue
            objBaseparty.ClientDetail.PaymentTermCode = txttermspay.Text
            objBaseparty.ClientDetail.RenewalStopCode = ddlrenewaldtopcode.SelectedValue
            objBaseparty.Source = txtsource.Text

            objBaseparty.ClientDetail.SeasonalGiftCode = ddseasonalgift.SelectedValue
            objBaseparty.ClientDetail.LoyaltyNumber = txtloyalty.Text



            objBaseparty.ClientDetail.AgentReference = txtagentref.Text
            If txtcurrentagent.Text = "" Then
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = False
            Else
                objBaseparty.ClientDetail.CurrentIntermediaryKeySpecified = True
            End If
            If txtcurrentagent.Text.Length > 0 Then
                objBaseparty.ClientDetail.CurrentIntermediaryKey = Convert.ToInt32(txtcurrentagent.Text)
            End If
            objBaseparty.ClientDetail.StrengthCode = txtstrength.Text
            ' objBaseparty.ClientDetail.StrengthCode = ddlprosStrengthCode.SelectedValue.ToString
            objBaseparty.ClientDetail.StatusCode = txtpstatus.Text
            'objBaseparty.ClientDetail.StatusCode = ddlprosStatus.SelectedValue.ToString


            If txtPIcode.Text = "" Then
                objBaseparty.ClientDetail.PreviousInsurerKeySpecified = False
            Else
                objBaseparty.ClientDetail.PreviousInsurerKeySpecified = True
            End If
            If txtPIcode.Text.Length > 0 Then
                objBaseparty.ClientDetail.PreviousInsurerKey = Convert.ToInt32(txtPIcode.Text)
            End If
            If txtPBcode.Text = "" Then
                objBaseparty.ClientDetail.PreviousBrokerKeySpecified = False
            Else
                objBaseparty.ClientDetail.PreviousBrokerKeySpecified = True
            End If
            If txtPBcode.Text.Length > 0 Then
                objBaseparty.ClientDetail.PreviousBrokerKey = txtPBcode.Text
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

            If txtexempt.Text = "" Then
                objBaseparty.TaxExemptSpecified = False
            Else
                objBaseparty.TaxExemptSpecified = True
            End If
            objBaseparty.TaxExempt = txtexempt.Text
            If txtpercentage.Text = "" Then
                objBaseparty.TaxPercentageSpecified = False
            Else
                objBaseparty.TaxPercentageSpecified = True
            End If
            If txtpercentage.Text.Length > 0 Then
                objBaseparty.TaxPercentage = Convert.ToDecimal(txtpercentage.Text)
            End If
            If ddlbusiness.SelectedValue <> 0 Then
                objBaseparty.BusinessCode = ddlbusiness.SelectedItem.Text
            End If

            If ddlSic.SelectedValue <> "0" Then 'ddlSic.SelectedValue <> 0 then
                objBaseparty.SICCode = ddlSic.SelectedValue
            End If

            objBaseparty.TradeCode = ddlTrade.SelectedItem.ToString()
            If ddlNoofEmployees.SelectedValue = 0 Then
                objBaseparty.NumberOfEmployeesSpecified = False
            Else
                objBaseparty.NumberOfEmployeesSpecified = True
                objBaseparty.NumberOfEmployees = ddlNoofEmployees.SelectedValue
            End If

            Dim icnt As Integer

            icnt = 0
            For Each row As GridViewRow In GVContacts.Rows

                objBaseparty.Addresses(icnt).AddressLine2 = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine3 = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).PostCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine1 = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).AddressLine4 = DirectCast(row.Cells(5).Controls(0), TextBox).Text
                objBaseparty.Addresses(icnt).CountryCode = DirectCast(row.Cells(6).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In GVContacts1.Rows


                Dim obj As BaseContactDetailType
                obj = New BaseContactDetailType
                obj.Item = "a.yahoo.com"
                obj.ItemElementName = ItemChoiceType.EmailAddress

                objBaseparty.Contacts(icnt).AreaCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.Contacts(icnt).ContactDetail = obj
                objBaseparty.Contacts(icnt).ContactTypeCode = ContactTypeType.EMAIL

                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In gvassociates.Rows

                objBaseparty.ClientDetail.Associates(icnt).RelationshipCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Associates(icnt).RelationshipDescription = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Associates(icnt).ClientKey = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Associates(icnt).AssociateKey = DirectCast(row.Cells(4).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next


            icnt = 0
            For Each row As GridViewRow In gvConcictions.Rows

                objBaseparty.ClientDetail.Convictions(icnt).SentenceTypeCode = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                If DirectCast(row.Cells(2).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).FineAmountSpecified = True
                End If

                If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationSpecified = True 'DirectCast(row.Cells(3).Controls(1), TextBox).Text
                End If

                If DirectCast(row.Cells(9).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPointsSpecified = True 'DirectCast(row.Cells(9).Controls(1), TextBox).Text
                End If

                If DirectCast(row.Cells(13).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevelSpecified = True 'DirectCast(row.Cells(13).Controls(1), TextBox).Text
                End If

                If DirectCast(row.Cells(14).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = False
                Else
                    objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDateSpecified = True 'DirectCast(row.Cells(14).Controls(1), TextBox).Text
                End If



                objBaseparty.ClientDetail.Convictions(icnt).SentenceDuration = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).TypeCode = DirectCast(row.Cells(3).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).DrivingLicensePenaltyPoints = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).SentenceEffectiveDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).FineAmount = Convert.ToDecimal(DirectCast(row.Cells(6).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.Convictions(icnt).Date = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).ConvictionKey = DirectCast(row.Cells(8).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).AlcoholMeasurementMethod = DirectCast(row.Cells(9).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).SentenceDurationQualifier = DirectCast(row.Cells(10).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).SentenceDescription = DirectCast(row.Cells(11).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).Description = DirectCast(row.Cells(12).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).AlcoholLevel = DirectCast(row.Cells(13).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.Convictions(icnt).StatusCode = DirectCast(row.Cells(14).Controls(0), TextBox).Text
                icnt = icnt + 1

            Next



            icnt = 0
            For Each row As GridViewRow In gvloyalty.Rows

                objBaseparty.ClientDetail.LoyaltyScheme(icnt).MainMember = DirectCast(row.Cells(1).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).OtherReference = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDate = Convert.ToDateTime(DirectCast(row.Cells(3).Controls(0), TextBox).Text)
                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = False
                Else
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).Active = True 'DirectCast(row.Cells(3).Controls(1), TextBox).Text

                End If

                objBaseparty.ClientDetail.LoyaltyScheme(icnt).StartDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeKey = Convert.ToInt32(DirectCast(row.Cells(6).Controls(0), TextBox).Text)
                If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = False
                Else
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).EndDateSpecified = True 'DirectCast(row.Cells(6).Controls(1), TextBox).Text
                End If

                objBaseparty.ClientDetail.LoyaltyScheme(icnt).LoyaltySchemeCode = DirectCast(row.Cells(7).Controls(0), TextBox).Text
                objBaseparty.ClientDetail.LoyaltyScheme(icnt).MembershipNumber = DirectCast(row.Cells(8).Controls(0), TextBox).Text

                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = False
                Else
                    objBaseparty.ClientDetail.LoyaltyScheme(icnt).ActiveSpecified = True 'DirectCast(row.Cells(9).Controls(1), TextBox).Text

                End If


                icnt = icnt + 1

            Next

            icnt = 0
            For Each row As GridViewRow In gvpolicies.Rows
                If DirectCast(row.Cells(3).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = False
                Else
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuotedSpecified = True 'DirectCast(row.Cells(0).Controls(1), TextBox).Text
                End If
                If DirectCast(row.Cells(4).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = False
                Else
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremiumSpecified = True 'DirectCast(row.Cells(1).Controls(1), TextBox).Text
                End If


                objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectPolicyKey = Convert.ToInt32(DirectCast(row.Cells(1).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.ProspectPolicies(icnt).ProspectTypeCode = DirectCast(row.Cells(2).Controls(0), TextBox).Text
                If DirectCast(row.Cells(5).Controls(0), TextBox).Text = "" Then
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = False
                Else
                    objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDateSpecified = True 'DirectCast(row.Cells(4).Controls(1), TextBox).Text
                End If

                objBaseparty.ClientDetail.ProspectPolicies(icnt).TimesQuoted = Convert.ToDecimal(DirectCast(row.Cells(3).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.ProspectPolicies(icnt).TargetPremium = Convert.ToDecimal(DirectCast(row.Cells(4).Controls(0), TextBox).Text)
                objBaseparty.ClientDetail.ProspectPolicies(icnt).RenewalDate = Convert.ToDateTime(DirectCast(row.Cells(5).Controls(0), TextBox).Text)
                icnt = icnt + 1

            Next
            Return objBaseparty

        End If


    End Function

    Protected Sub GVContacts_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVContacts.SelectedIndexChanged
        Dim oGetPartyResponseType As New BaseAddressWithContactsType

        objBaseparty = Session("AddParty")

        oGetPartyResponseType = objBaseparty.Addresses(GVContacts.SelectedIndex)

        txtstname.Text = oGetPartyResponseType.AddressLine1
        txtposttown.Text = oGetPartyResponseType.AddressLine3
        txtpostcode.Text = oGetPartyResponseType.PostCode
        txtlocality.Text = oGetPartyResponseType.AddressLine2
        txtcounty.Text = oGetPartyResponseType.AddressLine4
        ddlconCountry.SelectedValue = oGetPartyResponseType.CountryCode
        plnAddContact.Visible = False
        pnlAddAddress.Visible = True

    End Sub
    Public Function setobject() As Object
        'Dim oGetPartyResponseType As New GetPartyResponseType
        'oGetPartyResponseType = Session("GetPartyResponse")

        '

        If strClient = "PC" Then
            objBaseparty = New BasePartyPCType
        End If

        If strClient = "CC" Then
            objBaseparty = New BasePartyCCType
        End If

        If strClient = "GC" Then
            objBaseparty = New BasePartyOTHERType
        End If


        'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
        '    oGetPartyResponseType.Item.GetType() Is GetType(BasePartyPCType) Then
        '    objBaseparty = New BasePartyPCType
        '    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyPCType)

        'End If



        'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
        '       oGetPartyResponseType.Item.GetType() Is GetType(BasePartyCCType) Then


        '    objBaseparty = New BasePartyCCType
        '    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyCCType)
        'End If
        'If oGetPartyResponseType.Item IsNot Nothing AndAlso _
        '   oGetPartyResponseType.Item.GetType() Is GetType(BasePartyOTHERType) Then


        '    objBaseparty = New BasePartyOTHERType
        '    objBaseparty = DirectCast(oGetPartyResponseType.Item, BasePartyOTHERType)

        'End If



        Return objBaseparty

    End Function

    Protected Sub btnccancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnccancel.Click
        plnAddContact.Visible = False

    End Sub

    Protected Sub btnacancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnacancel.Click
        pnlAddAddress.Visible = False

    End Sub

    Protected Sub ddmaritalstatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddmaritalstatus.SelectedIndexChanged

    End Sub


    Protected Sub GVContacts1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GVContacts1.SelectedIndexChanged
        Dim oGetPartyResponseType As New BaseContactType

        objBaseparty = Session("AddParty")

        'Dim objcontact As New BaseContactType

        oGetPartyResponseType = objBaseparty.Contacts(GVContacts1.SelectedIndex)
        txtareacode.Text = oGetPartyResponseType.AreaCode
        txtdescription.Text = oGetPartyResponseType.Description


        'txtstname.Text = oGetPartyResponseType.AddressLine1
        'txtposttown.Text = oGetPartyResponseType.AddressLine3
        'txtpostcode.Text = oGetPartyResponseType.PostCode
        'txtlocality.Text = oGetPartyResponseType.AddressLine2
        'txtcounty.Text = oGetPartyResponseType.AddressLine4
        'ddlconCountry.SelectedValue = oGetPartyResponseType.CountryCode

        pnlAddAddress.Visible = False
        plnAddContact.Visible = True

    End Sub

    Protected Sub gvConcictions_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvConcictions.SelectedIndexChanged
        Dim oGetPartyResponseType As New BaseConvictionType

        objBaseparty = Session("AddParty")
        oGetPartyResponseType = objBaseparty.ClientDetail.Convictions(gvConcictions.SelectedIndex)

        If oGetPartyResponseType IsNot Nothing Then
            'Dim objBaseparty As New BasePartyPCType
            'objBaseparty = DirectCast(oGetPartyResponseType, BasePartyPCType)

            'ddlconvictiontype.SelectedValue = oGetPartyResponseType.TypeCode
            'ddlconvictionstatus.SelectedValue = oGetPartyResponseType.StatusCode
            ddlconvictiontype.SelectedItem.Text = oGetPartyResponseType.TypeCode
            ddlconvictionstatus.SelectedItem.Text = oGetPartyResponseType.StatusCode
            txtcdescription.Text = oGetPartyResponseType.Description
            txtcfine.Text = oGetPartyResponseType.FineAmount

            txtcdate.Text = oGetPartyResponseType.Date

            ddlsentencetype.SelectedItem.Text = oGetPartyResponseType.SentenceTypeCode
            'ddlsentencetype.SelectedItem.Text = oGetPartyResponseType.SentenceTypeCode
            txtsdescription.Text = oGetPartyResponseType.SentenceDescription
            If oGetPartyResponseType.SentenceEffectiveDateSpecified Then
                txtsdate.Text = oGetPartyResponseType.SentenceEffectiveDate
            Else
                txtsdate.Text = ""

            End If

            txtsduration.Text = oGetPartyResponseType.SentenceDuration

            ddlsentencetime.SelectedItem.Text = oGetPartyResponseType.SentenceDurationQualifier

            ddlAlcoholmethod.SelectedItem.Text = oGetPartyResponseType.AlcoholMeasurementMethod


            If oGetPartyResponseType.AlcoholLevelSpecified Then
                txtalcohollevel.Text = oGetPartyResponseType.AlcoholLevel
            Else
                txtalcohollevel.Text = ""
            End If


            If oGetPartyResponseType.DrivingLicensePenaltyPointsSpecified Then
                txtpenality.Text = oGetPartyResponseType.DrivingLicensePenaltyPoints
            Else
                txtpenality.Text = ""
            End If


        End If

        pnlAddConvictions.Visible = True
    End Sub
End Class
