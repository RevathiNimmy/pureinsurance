Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.IO
Imports System.Web.UI
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports System.Web.Services

Namespace Nexus

    Partial Class Modal_BankDetail : Inherits System.Web.UI.Page
        Private sDefaultCountry As String = CType(GetSection("NexusFrameWork"), Nexus.Library.Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID().ToString()).Countries.DefaultCountryCode
        Dim oParty As NexusProvider.BaseParty = Nothing

        Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
            MyBase.OnInit(e)
        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        ''' <summary>
        ''' Page Load Method
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            'To set the Focus
            Page.SetFocus(GISNBankPaymentType)

            'Need to store this value in hidden in order to read from javascript
            txtPostBackTo.Value = Request.QueryString("PostbackTo")
            If Not IsPostBack Then
                PopulatePaymentType()
                'Need to Retreive the Data from Session
                RetreiveData()

                txtMode.Value = ""
                Dim oNewBank As New NexusProvider.Bank

                If Request("BankKey") <> "" Then

                    txtMode.Value = "Update"
                    txtBankKey.Value = Request("BankKey")

                    'Change visibility of buttons
                    btnAddBank.Visible = False
                    btnUpdateBank.Visible = True

                    Dim oBank As NexusProvider.Bank = oParty.BankDetails.Item(CType(Request("BankKey"), Integer))
                    GISNBankPaymentType.SelectedValue = oBank.BankPaymentTypeCode
                    GISNBankPaymentType.Enabled = False
                    txtAccountType.Text = oBank.AccountType

                    ''disable account type in case Instalments bank selected
                    ''during Instalment NB
                    If GISNBankPaymentType.SelectedValue = "INS" Then
                        txtAccountType.Enabled = False
                    Else
                        txtAccountType.Enabled = True
                    End If
                    txtAccountHolderName.Text = oBank.AccountHolderName
                    txtAccountNo.Text = oBank.AccountNumber
                    txtBankBranchCode.Text = oBank.BranchCode
                    txtBankBranch.Text = oBank.BankBranch
                    GISLookup_BankList.Value = oBank.BankCode
                    txtAccountCode.Value = oBank.AccountKey
                    txtBIC.Text = oBank.BIC
                    txtIBAN.Text = oBank.IBAN
                    txtStreet.Text = oBank.PartyBankAddress.Address1
                    txtLocality.Text = oBank.PartyBankAddress.Address2
                    txtPostTown.Text = oBank.PartyBankAddress.Address3
                    txtCounty.Text = oBank.PartyBankAddress.Address4
                    txtPostCode.Text = oBank.PartyBankAddress.PostCode
                    GISCountry.Value = oBank.PartyBankAddress.CountryCode
                Else
                    'This Code will execute when user is in Add Mode
                    txtMode.Value = "Add"
                    'Set the default country setting
                    GISCountry.Value = sDefaultCountry

                    If oParty IsNot Nothing Then
                        Select Case True
                            Case TypeOf oParty Is NexusProvider.CorporateParty
                                txtAccountHolderName.Text = DirectCast(oParty, NexusProvider.CorporateParty).CompanyName
                            Case TypeOf oParty Is NexusProvider.PersonalParty
                                txtAccountHolderName.Text = DirectCast(oParty, NexusProvider.PersonalParty).ClientSharedData.ResolvedName
                        End Select
                    End If
                End If
            End If

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
                    Case TypeOf Session(CNParty) Is NexusProvider.OtherParty
                        oParty = CType(Session(CNParty), NexusProvider.OtherParty)
                End Select
            End If
        End Sub

        ''' <summary>
        ''' Custom validator to check the Account Number validation
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="args"></param>
        ''' <remarks></remarks>
        Protected Sub CustVldValidateAccountNumber_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles CustVldValidateAccountNumber.ServerValidate
            Dim oValidationDetailsCollection As NexusProvider.ValidationDetailsCollection
            'Bank Media ID should always go as '0'
            Dim iBankMediaId As Integer = 0
            'Account Number should be passed as BankBranchCode|AccountNumber
            Dim sAccountNumber As String = txtBankBranchCode.Text & "|" & txtAccountNo.Text & "|" & txtAccountType.Text
            Dim iCountryID As Integer = GetKeyForDescription(NexusProvider.ListType.PMLookup, GISCountry.Text, "Country")
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sBankName As String = GISLookup_BankList.Value

            'Validating Duplicate Account Type 
            'Start
            Dim oBankDetails As New NexusProvider.BankCollection
            Dim bAccountTypeValidateion As Boolean = True

            'Need to Retreive the Data from Session
            RetreiveData()
            If oParty IsNot Nothing AndAlso oParty.BankDetails IsNot Nothing Then
                oBankDetails = oParty.BankDetails
            End If

            CustVldValidateAccountNumber.ErrorMessage = String.Empty
            CustVldValidateAccountType.ErrorMessage = String.Empty

          If Trim(Request("BankKey")) = "" Then
                For iCount = 0 To oBankDetails.Count - 1
                    If Trim(oBankDetails.Item(iCount).AccountType) = Trim(txtAccountType.Text) AndAlso oBankDetails.Item(iCount).TaskMode <> NexusProvider.Bank.Mode.Delete AndAlso Trim(oBankDetails.Item(iCount).BankPaymentTypeCode) = Trim(GISNBankPaymentType.SelectedValue) Then
                        bAccountTypeValidateion = False
                        Exit For
                    End If
                Next
            Else
                For iCount = 0 To oBankDetails.Count - 1
                    If iCount <> Trim(Request("BankKey")) Then
                        If Trim(oBankDetails.Item(iCount).AccountType) = Trim(txtAccountType.Text) AndAlso oBankDetails.Item(iCount).TaskMode <> NexusProvider.Bank.Mode.Delete AndAlso Trim(oBankDetails.Item(iCount).BankPaymentTypeCode) = Trim(GISNBankPaymentType.SelectedValue) Then
                            bAccountTypeValidateion = False
                            Exit For
                        End If
                    End If
                Next
            End If

            If Not bAccountTypeValidateion Then
                CustVldValidateAccountType.ErrorMessage = GetLocalResourceObject("lbl_errmsg_ValidateAccountType").ToString()
                args.IsValid = False
            Else
                'Validate the Bank Account Number using SAM call ValidateBankAccountNumber

                oValidationDetailsCollection = oWebService.ValidateBankAccountNumber(iBankMediaId, iCountryID, sAccountNumber, hidBankMediaCode.Value, sBIC:=txtBIC.Text, sIBAN:=txtIBAN.Text, sBankName:=sBankName)

                If oValidationDetailsCollection(0).IsValid Then
                    'If the Account Number is Valid
                    args.IsValid = True
                    'Before send back to parent page , show the confirmation page
                    Me.ClientScript.RegisterStartupScript(GetType(String), "StartupScripts", "confirmation();", True)
                    If Trim(oValidationDetailsCollection(0).PostalCode) <> "" Then
                        txtStreet.Text = oValidationDetailsCollection(0).AddressLine1
                        txtLocality.Text = oValidationDetailsCollection(0).AddressLine2
                        txtPostTown.Text = oValidationDetailsCollection(0).AddressLine3
                        txtCounty.Text = oValidationDetailsCollection(0).AddressLine4
                        txtPostCode.Text = oValidationDetailsCollection(0).PostalCode
                        GISLookup_BankList.Value = oValidationDetailsCollection(0).BankName
                    End If
                Else
                    'If the Account Number is Invalid
                    args.IsValid = False
                    If oValidationDetailsCollection(0).ValidationMessageDataset <> "" AndAlso oValidationDetailsCollection(0).ValidationMessageDataset IsNot Nothing Then
                        'If the Collection returna any message from Back office Script
                        CustVldValidateAccountNumber.ErrorMessage = oValidationDetailsCollection(0).ValidationMessageDataset
                    Else
                        'If the Collection does not returns any BO script message, and IsValid key is false, then a custom message is passed 
                        CustVldValidateAccountNumber.ErrorMessage = GetLocalResourceObject("lbl_errmsg_ValidateAccountNumber").ToString()
                    End If
                    If oValidationDetailsCollection(0).IsValidationOverridable = False Then
                        'It is must to override the values returned by SAM
                        txtStreet.Text = oValidationDetailsCollection(0).AddressLine1
                        txtLocality.Text = oValidationDetailsCollection(0).AddressLine2
                        txtPostTown.Text = oValidationDetailsCollection(0).AddressLine3
                        txtCounty.Text = oValidationDetailsCollection(0).AddressLine4
                        txtPostCode.Text = oValidationDetailsCollection(0).PostalCode
                        GISLookup_BankList.Value = oValidationDetailsCollection(0).BankName
                    End If
                End If
            End If

            'cleaning up
            oWebService = Nothing
            oValidationDetailsCollection = Nothing
            iBankMediaId = Nothing
            sAccountNumber = Nothing
            iCountryID = Nothing
        End Sub

        Private Sub PopulatePaymentType()
            GISNBankPaymentType.Items.Clear()
            GISNBankPaymentType.Items.Add(New ListItem("(Please Select)", ""))
            Dim oWebService As NexusProvider.ProviderBase = Nothing
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oLookUP As New NexusProvider.LookupListCollection
            oLookUP = oWebService.GetList(NexusProvider.ListType.PMLookup, "Bank_Payment_Type", True, False)

            'Sort collecton before binding
            oLookUP.Sort(NexusProvider.DataItemTypes.Description, NexusProvider.Direction.Asc)

            For i As Integer = 0 To oLookUP.Count - 1
                Dim oListItem As New ListItem
                Select Case Request.QueryString("loc")
                    Case "Instalments"
                        If oLookUP(i).Code.ToUpper() = "ANY" Or oLookUP(i).Code.ToUpper() = "INS" Then
                            oListItem.Text = oLookUP(i).Description.ToString
                            oListItem.Value = oLookUP(i).Code.Trim
                            GISNBankPaymentType.Items.Add(oListItem)
                        End If
                    Case Else
                        oListItem.Text = oLookUP(i).Description.ToString
                        oListItem.Value = oLookUP(i).Code.Trim
                        GISNBankPaymentType.Items.Add(oListItem)
                End Select
            Next

            GISNBankPaymentType.DataBind()
            oWebService = Nothing
            oLookUP = Nothing

        End Sub
    End Class
End Namespace