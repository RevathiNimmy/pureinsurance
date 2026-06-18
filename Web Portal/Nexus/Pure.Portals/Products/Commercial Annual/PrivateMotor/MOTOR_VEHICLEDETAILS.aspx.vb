Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports Nexus.Library
Imports CMS.Library
Imports Nexus.Utils
Imports System.Xml
Imports System.Xml.XPath
Imports System.Globalization.CultureInfo
Namespace Nexus

    Partial Class MOTOR_VEHICLEDETAILS : Inherits BaseRisk

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            SetPageProgress(3)
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            'Find Branch of selected quote/policy       
            Dim sBranchName As String = ""
            If Session(CNAgentDetails) IsNot Nothing Then
                Dim oBranchs As NexusProvider.BranchCollection = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ListOfBranches
                If oBranchs IsNot Nothing Then
                    For Each oBranch As NexusProvider.Branch In oBranchs
                        If oBranch.Code = Session(CNBranchCode) Then
                            sBranchName = oBranch.Description
                            Exit For
                        End If
                    Next
                End If
            End If

            Dim sClientCode As String = ""
            Dim oParty As NexusProvider.BaseParty = Session.Item(CNParty)
            Select Case True
                Case TypeOf oParty Is NexusProvider.CorporateParty
                    With CType(oParty, NexusProvider.CorporateParty)
                        If Session(CNLoginType) = LoginType.Customer Then
                        Else
                            If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                sClientCode = .ClientSharedData.ShortName.Trim()
                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                sClientCode = .UserName.Trim()
                            End If
                        End If
                    End With
                Case TypeOf oParty Is NexusProvider.PersonalParty
                    With CType(oParty, NexusProvider.PersonalParty)
                        If Session(CNLoginType) = LoginType.Customer Then
                            If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                sClientCode = .ClientSharedData.ShortName.Trim()
                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                sClientCode = .UserName.Trim()
                            End If
                        Else
                            If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                sClientCode = .ClientSharedData.ShortName.Trim()
                            ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                sClientCode = .UserName.Trim()
                            End If
                        End If
                    End With
            End Select

            HyperLink1.NavigateUrl = "~/secure/DocumentManager.aspx?path=" & Trim(sBranchName) & "|" & Trim(sClientCode) & "|" & Trim(oQuote.InsuranceFileRef)
        End Sub

        Public Overrides Sub PostDataSetWrite()
        End Sub

        Public Overrides Sub PreDataSetWrite()

        End Sub

        Protected Sub VEHDET__CLAS_USE_SelectedIndexChange(sender As Object, e As EventArgs) Handles VEHDET__CLAS_USE.SelectedIndexChange

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "alert", "SetRate();", True)
        End Sub
    End Class

End Namespace
