Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml

Namespace Nexus

    Partial Class Modal_WriteOffPayment
        Inherits CMS.Library.Frontend.clsCMSPage

        Dim oWebService As NexusProvider.ProviderBase
        Dim oUserDetails As NexusProvider.UserDetails
        Dim oUserAuthority As New NexusProvider.UserAuthority
        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            oUserAuthority.UserCode = Session(CNLoginName)
            'set the authority options for reverse allocation
            oUserAuthority.UserAuthorityOption = NexusProvider.UserAuthority.UserAuthorityOptionType.HasWriteOffAuthority
            'initiate the GetUserAuthority method
            oWebService.GetUserAuthorityValue(oUserAuthority)

        End Sub
        Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


            Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_WriteOffHeading").ToString()
            sConfirmationMessage = sConfirmationMessage.Replace("#DocRef", Session(CNDocumentRef))
            WriteOffHeader.Text = sConfirmationMessage


        End Sub
        ''' <summary>
        '''  This WriteOffBtn_Ok_Click event is the Click event of the OK button in which the functionality for adding a writeoff Payment
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub WriteOffBtn_Ok_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles WriteOffBtn_Ok.Click
            If (Page.IsValid) Then

                If (Not IsNumeric(txtWriteoff_amtVal.Text) OrElse String.IsNullOrEmpty(txtWriteoff_amtVal.Text.Trim)) Then
                    rvWriteoffRange.Visible = True
                    rvWriteoffRange.Text = GetLocalResourceObject("WriteOffErr").ToString()
                    Exit Sub
                End If
                Dim txtWriteOffVal As String = txtWriteoff_amtVal.Text
                rvWriteoffRange.Text = ""
                Dim dWriteOffAmount As Decimal = IIf(IsNumeric(txtWriteOffVal), Convert.ToDecimal(txtWriteOffVal), 0.00)

                If (dWriteOffAmount = 0) Then
                    rvWriteoffRange.Visible = True
                    rvWriteoffRange.Text = GetLocalResourceObject("WriteOffAmtValid_Err").ToString()
                    Exit Sub
                ElseIf Math.Abs(dWriteOffAmount) > Math.Abs(oUserAuthority.UserAuthorityOptionalValue2) Then
                    rvWriteoffRange.Visible = True
                    rvWriteoffRange.Text = GetLocalResourceObject("WriteOffAmt_Err").ToString()
                    Exit Sub
                Else
                    Session(CNWriteOffAmount) = dWriteOffAmount
                    Dim PostBackStr As String = "self.parent." & Page.ClientScript.GetPostBackEventReference(Me, "RefreshGrid") & ";"
                    Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                    Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
                End If
            End If

        End Sub
        ''' <summary>
        '''  This WriteOffBtn_Cancel_Click event is the Click event of the Cancel button  all the Textboxes are made empty.
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub WriteOffBtn_Cancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles WriteOffBtn_Cancel.Click
            Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_remove();", True)
        End Sub

    End Class
End Namespace