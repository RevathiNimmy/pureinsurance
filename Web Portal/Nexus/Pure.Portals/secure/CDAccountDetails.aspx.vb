Imports CMS.Library.Frontend
Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports System.Web.Configuration
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Namespace Nexus

    Partial Class secure_CDAccountDetails : Inherits CMS.Library.Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oCashDeposit As NexusProvider.CashDeposit

        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            If Not IsPostBack Then
                ViewState("SelectedCDAccount") = Request.QueryString("CDAccount") 'the CDAccount which is getting Edited
                ViewState("SelectedPartyCode") = Request.QueryString("PartyCode")

                If ViewState("SelectedCDAccount") IsNot Nothing Then
                    'Edit an existing CD accound
                    GetCashDepositAccountDetails() 'CD account is in EDIT mode, so collect all the information and populate in related fields
                Else
                    'Adding new CD account
                    GetNextCashDepositRef() 'Need to get the Next CashDepositRef for New CD account and populate in label
                End If
            End If
        End Sub

        Sub GetCashDepositAccountDetails()
            'populating the previous selected values(Edit Mode)
            oCashDeposit = New NexusProvider.CashDeposit
            oWebService = New NexusProvider.ProviderManager().Provider

            oCashDeposit = oWebService.GetCashDeposit(ViewState("SelectedPartyCode"), ViewState("SelectedCDAccount"))

            lblCDNumberValue.Text = ViewState("SelectedCDAccount").ToString ' set the CDAccount receiving from QueryString
            chkSinglePolicyLock.Checked = oCashDeposit.IsSinglePolicy
            ViewState("TimeStamp") = oCashDeposit.CDTimeStamp

            'Call SetSelectedValues to set the values in Pick list
            pckBranch.SetSelectedValues(oCashDeposit.Branches)

            'Collection of the Products received from SAM
            pckProduct.SetSelectedValues(oCashDeposit.Products)

            'cleaning up
            oCashDeposit = Nothing
            oWebService = Nothing
        End Sub

        ''' <summary>
        ''' 'Need to get the Next CashDepositRef for New CD account and populate in label
        ''' </summary>
        ''' <remarks></remarks>
        Sub GetNextCashDepositRef()

            oCashDeposit = New NexusProvider.CashDeposit
            oWebService = New NexusProvider.ProviderManager().Provider

            oCashDeposit = oWebService.GetNextCashDepositRef(ViewState("SelectedPartyCode"))
            ViewState("PartyKey") = oCashDeposit.PartyKey
            ViewState("PartyName") = oCashDeposit.PartyName
            lblCDNumberValue.Text = oCashDeposit.CashDepositRef 'populate in label

            'cleaning up
            oCashDeposit = Nothing
            oWebService = Nothing
        End Sub

        Protected Sub btnSubmit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmit.Click

            If Page.IsValid Then
                Dim oCashDepositCollection As New NexusProvider.CashDepositCollection
                Dim objBranches As NexusProvider.Branch
                Dim objProducts As NexusProvider.Product
                Dim oEventDetails As New NexusProvider.EventDetails
                Dim oPartySearchCriteria As New NexusProvider.PartySearchCriteria
                Dim oPartyCollection As NexusProvider.PartyCollection
                Dim iCounterVar As Integer
                Dim oPartyName, oPartyKey As String

                oCashDeposit = New NexusProvider.CashDeposit
                oWebService = New NexusProvider.ProviderManager().Provider

                oCashDeposit.CashDepositRef = lblCDNumberValue.Text
                oCashDeposit.UserName = Session(CNLoginName)
                oCashDeposit.IsSinglePolicy = chkSinglePolicyLock.Checked

                oCashDeposit.PartyType = NexusProvider.ClientAgentType.A
                oCashDeposit.PartyCode = ViewState("SelectedPartyCode")
                oCashDeposit.CDTimeStamp = ViewState("TimeStamp")

                If pckBranch.GetSelectedItems() IsNot Nothing Then
                    'Addinf the selected branches to the collection
                    For iCounterVar = 0 To pckBranch.GetSelectedItems().Count - 1
                        objBranches = New NexusProvider.Branch

                        objBranches.Code = pckBranch.GetSelectedItems().Item(iCounterVar).Value
                        oCashDeposit.Branches.Add(objBranches)
                    Next
                End If

                If pckProduct.GetSelectedItems() IsNot Nothing Then
                    'Adding the selected prodcuts to the collection
                    For iCounterVar = 0 To pckProduct.GetSelectedItems().Count - 1
                        objProducts = New NexusProvider.Product

                        objProducts.ProductCode = pckProduct.GetSelectedItems().Item(iCounterVar).Value
                        oCashDeposit.Products.Add(objProducts)
                    Next
                End If

                oCashDepositCollection.Add(oCashDeposit)

                If ViewState("SelectedCDAccount") IsNot Nothing Then
                    'Editing an existing CD accound
                    oCashDepositCollection = oWebService.UpdateCashDeposit(oCashDepositCollection)
                Else
                    'Adding new CD account
                    oCashDepositCollection = oWebService.AddCashDeposit(oCashDepositCollection)

                    'New Cash Deposit has been added, now adding the event fot this

                    oWebService = New NexusProvider.ProviderManager().Provider

                    'TODO: FINDPARTY is an alternative, raised the SAM PN for this
                    'Need to get the partykey for adding an event
                    oPartySearchCriteria.ShortName = ViewState("SelectedPartyCode")
                    oPartySearchCriteria.PartyType = "GC"
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.PC)
                    oPartySearchCriteria.PartyTypes.Add(NexusProvider.PartyTypeType.CC)
                    oPartyCollection = oWebService.FindParty(oPartySearchCriteria)

                    If oPartyCollection.Count <> 0 Then
                        'Event needs to be added for Client
                        oPartyName = oPartyCollection(0).UserName.Trim
                        oPartyKey = oPartyCollection(0).Key

                        With oEventDetails
                            .EventDate = Now()
                            .PartyKey = oPartyKey
                            .RtfText = "Cash Deposit Details Created - " & oPartyName.ToString & ", " & oCashDepositCollection(0).CashDepositRef & ", " & Session(CNLoginName)
                            .UserName = Session(CNLoginName)
                            .EventTypeKey = 1
                            .EventLogSubjectKey = 1
                        End With

                        oWebService.AddEvent(oEventDetails)
                    End If
                End If

                'cleaning up
                oWebService = Nothing
                oCashDepositCollection = Nothing
                objBranches = Nothing
                objProducts = Nothing
                oEventDetails = Nothing
                oPartySearchCriteria = Nothing
                oPartyCollection = Nothing
                iCounterVar = Nothing
                Response.Redirect("FindCDAccount.aspx", False)
            End If
        End Sub

        Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            Response.Redirect("FindCDAccount.aspx", False)
        End Sub

        Protected Sub custvldBranchProductRequired_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles custvldBranchProductRequired.ServerValidate
            'select atleast one product or branch
            If pckBranch.GetSelectedItems.Count <> 0 And pckProduct.GetSelectedItems.Count <> 0 Then
                args.IsValid = True
            Else
                args.IsValid = False
            End If
        End Sub

    End Class
End Namespace
