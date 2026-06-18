Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.IO
Imports System.Web.UI
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports System.Web.Services

Namespace Nexus

    Partial Class Modal_CreditCardDetail : Inherits System.Web.UI.Page
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
                    btnAddCreditCard.Visible = False
                    btnUpdateCreditCard.Visible = True

                    'Dim oCreditCard As NexusProvider.CreditCard = oParty.CreditCardDetails.Item(CType(Request("BankKey"), Integer))
                    Dim oBank As NexusProvider.Bank = oParty.BankDetails.Item(CType(Request("BankKey"), Integer))
                    With oBank
                        GISNBankPaymentType.SelectedValue = oBank.BankPaymentTypeCode
                        GISNBankPaymentType.Enabled = False
                        txtAccountCode.Value = .CreditCard.TypeCode
                        txtAccountHolderName.Text = .CreditCard.CardHolder.Name
                        txtAccountType.Text = .CreditCard.AccountType
                        txtCardNo.Text = .CreditCard.Number
                        txtCounty.Text = .CreditCard.Address1
                        txtCSVPin.Text = .CreditCard.CCPin
                        txtExpiryDate.Text = .CreditCard.ExpiryDate
                        txtIssueNumber.Text = .CreditCard.Issue
                        txtLocality.Text = .CreditCard.Address2
                        txtManualAuth.Text = .CreditCard.ManualAuthCode
                        txtNameOnCard.Text = .CreditCard.NameOnCreditCard
                        txtPostCode.Text = .CreditCard.PostCode
                        txtPostTown.Text = .CreditCard.Address4
                        txtStartDate.Text = .CreditCard.StartDate
                        txtStreet.Text = .CreditCard.Address3
                        GISCountry.Value = .CreditCard.CountryCode

                        ''old
                        'GISNBankPaymentType.SelectedValue = oBank.BankPaymentTypeCode
                        'GISNBankPaymentType.Enabled = False
                        'txtAccountType.Text = oBank.AccountType
                        'txtAccountHolderName.Text = oBank.AccountHolderName

                        'txtAccountCode.Value = oBank.AccountKey
                        ''oBank.PartyBankAddress = New NexusProvider.Address
                        'txtStreet.Text = oBank.PartyBankAddress.Address1
                        'txtLocality.Text = oBank.PartyBankAddress.Address2
                        'txtPostTown.Text = oBank.PartyBankAddress.Address3
                        'txtCounty.Text = oBank.PartyBankAddress.Address4
                        'txtPostCode.Text = oBank.PartyBankAddress.PostCode
                        'GISCountry.Value = oBank.PartyBankAddress.CountryCode


                    End With
                Else
                    'This Code will execute when user is in Add Mode
                    txtMode.Value = "Add"
                    'Set the default country setting
                    GISCountry.Value = sDefaultCountry
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
                End Select
            End If
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
            Dim sAccountNumber As String = txtCardNo.Text
            Dim iCountryID As Integer = GetKeyForDescription(NexusProvider.ListType.PMLookup, GISCountry.Text, "Country")
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            'Validating Duplicate Account Type 
            'Start
            Dim oBankDetails As New NexusProvider.BankCollection
            Dim bAccountTypeValidateion As Boolean = True

            'Need to Retreive the Data from Session
            RetreiveData()
            oBankDetails = oParty.BankDetails

            If Trim(Request("BankKey")) = "" Then
                For iCount = 0 To oBankDetails.Count - 1
                    If Trim(oBankDetails.Item(iCount).AccountType) = Trim(txtAccountType.Text) And oBankDetails.Item(iCount).TaskMode <> NexusProvider.Bank.Mode.Delete Then
                        bAccountTypeValidateion = False
                        Exit For
                    End If
                Next
            Else
                For iCount = 0 To oBankDetails.Count - 1
                    If iCount <> Trim(Request("BankKey")) Then
                        If Trim(oBankDetails.Item(iCount).AccountType) = Trim(txtAccountType.Text) And oBankDetails.Item(iCount).TaskMode <> NexusProvider.Bank.Mode.Delete Then
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
                oValidationDetailsCollection = oWebService.ValidateBankAccountNumber(iBankMediaId, iCountryID, sAccountNumber, hidBankMediaCode.Value)

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
    End Class
End Namespace