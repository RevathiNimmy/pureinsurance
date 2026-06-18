Imports System.Web.HttpContext
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports Nexus.Utils

Partial Class Controls_UserInfo
    Inherits System.Web.UI.UserControl

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        If Current.User.Identity IsNot Nothing AndAlso Session.Item(CNLoginType) IsNot Nothing AndAlso Current.User.Identity.IsAuthenticated Then
            'PnlBranchName.Visible = True
            Select Case Session.Item(CNLoginType)

                Case LoginType.Agent
                    
                    If Not Session(CNAgentDetails) Is Nothing Then
                        Dim oUserDetail As NexusProvider.UserDetails = CType(Session(CNAgentDetails), NexusProvider.UserDetails)
                        lblUsername.Text = oUserDetail.ResolvedName

                        With CType(Session.Item(CNAgentDetails), NexusProvider.UserDetails)
                            If Not String.IsNullOrEmpty(.PartyType) Then
                                lblCompanyName.Text = .PartyName
                                lblCompanyName.Visible = True
                                lblBranchName.Visible = False
                                lblBranch.Visible = False
                            Else
                                Dim sBranchName As String = ""
                                Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                                If oBranchs IsNot Nothing Then
                                    For Each oBranch As NexusProvider.Branch In oBranchs
                                        If oBranch.Code = Session(CNBranchCode) Then
                                            sBranchName = oBranch.Description
                                            Session("BranchName") = sBranchName
                                            Exit For
                                        End If
                                    Next
                                End If
                                lblCompanyName.Visible = False
                                lblBranchName.Text = sBranchName
                                lblBranchName.Visible = True
                            End If
                            ' Check change password warning message
                            If Not IsWindowsAuthentication() Then
                                'Get No Of days to start showing waring message from system option
                                Dim oWebService As NexusProvider.ProviderBase = Nothing
                                oWebService = New NexusProvider.ProviderManager().Provider
                                Dim oPasswordExpiryDuration As NexusProvider.OptionTypeSetting = Nothing
                                oPasswordExpiryDuration = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5103)
                                Dim iPasswordExpiryDuration As Integer
                                If (Not String.IsNullOrEmpty(oPasswordExpiryDuration.OptionValue) AndAlso oPasswordExpiryDuration.OptionValue <> "0") Then
                                    iPasswordExpiryDuration = CType(oPasswordExpiryDuration.OptionValue, Integer)
                                    oUserDetail.PasswordExpiryDate = oUserDetail.PasswordChange.AddDays(iPasswordExpiryDuration)
                                End If

                                Dim oNumberOfDaysBeforeShowingPasswordExpiryWarning As NexusProvider.OptionTypeSetting = Nothing
                                oNumberOfDaysBeforeShowingPasswordExpiryWarning = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5111)

                                Dim iNumberOfDaysBeforeShowingPasswordExpiryWarning As Integer = CType(oNumberOfDaysBeforeShowingPasswordExpiryWarning.OptionValue, Integer)

                                ' In case of 0, no expiry warning is required as password expiry is not enabled
                                If iNumberOfDaysBeforeShowingPasswordExpiryWarning <> 0 Then
                                    Dim iNumberOfDayLeftToPasswordExpire As Integer = 0
                                    iNumberOfDayLeftToPasswordExpire = oUserDetail.PasswordExpiryDate.Value.Subtract(Now.Date).Days
                                    If iNumberOfDayLeftToPasswordExpire <= iNumberOfDaysBeforeShowingPasswordExpiryWarning Then
                                        phldrPasswordExpire.Visible = True
                                        ltPasswordExpireWarning.Text = GetLocalResourceObject("PasswordExpiryWarning").ToString().Replace("#NumberOfDays", iNumberOfDayLeftToPasswordExpire.ToString())
                                    End If
                                End If
                            End If
                        End With
                    End If
                Case LoginType.Customer
                    Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)

                    With CType(Session.Item(CNAgentDetails), NexusProvider.UserDetails)
                        lblCompanyName.Visible = False
                    End With
                    Select Case True
                        Case TypeOf oParty Is NexusProvider.CorporateParty

                            With CType(oParty, NexusProvider.CorporateParty)
                                lblUsername.Text = .MainContact
                            End With
                        Case TypeOf oParty Is NexusProvider.PersonalParty
                            lblUsername.Text = CType(oParty, NexusProvider.PersonalParty).ResolvedName
                        Case Else
                            'Unknown customer type, so hide control
                            Me.Visible = False
                    End Select
            End Select
            
        Else
            'Not logged in, so hide control
            Me.Visible = False
        End If

    End Sub
End Class
