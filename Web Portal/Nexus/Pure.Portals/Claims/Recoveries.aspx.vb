Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports CMS.Library
Namespace Nexus
    Partial Class Claims_Recoveries : Inherits CMS.Library.Frontend.clsCMSPage
        Dim m_sIsRecoveriesReadOnly As String
        ''' <summary>
        ''' Page_Load
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks> 
        Private nClaimPerilId As Integer = 0
        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            'if user is accessing this page directly
            If Session(CNLoginType) Is Nothing Then
                Response.Redirect(AppSettings("WebRoot") & "/Login.aspx", False)
            End If
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            oClaim = CType(Session(CNClaim), NexusProvider.ClaimOpen)

            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(CMS.Library.Portal.GetPortalID())
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            m_sIsRecoveriesReadOnly = oWebservice.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsRecoveriesReadOnly, NexusProvider.RiskTypeOptions.None, Session(CNProductCode), Nothing)

            If Request.QueryString("PerilID") IsNot Nothing Then
                nClaimPerilId = CInt(Request.QueryString("PerilID"))
            End If

            If Not IsPostBack Then
                'Setting of peril index and screen values
                Dim iPeril As Integer
                'Retreive the latest/updated the session variable
                GetClaimDetails(oClaim.ClaimKey, Nothing)

                If Request.QueryString("PerilIndex") IsNot Nothing Then
                    iPeril = CInt(Request.QueryString("PerilIndex"))
                    Session(CNClaimPerilIndex) = iPeril
                ElseIf Session(CNClaimPerilIndex) IsNot Nothing Then
                    iPeril = Session(CNClaimPerilIndex)
                End If

                'association of pages dynamically
                If HttpContext.Current.Session.IsCookieless Then
                    btnAddForTP.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SalvageRecovery.aspx?PostbackTo=" & updateThirdPartyRecovery.ClientID.ToString & "&PerilIndex=" & iPeril & "&PerilID=" & nClaimPerilId & "&modal=true&KeepThis=true&FromPage=TP&TB_iframe=true&height=600&width=700' , null);return false;"
                Else
                    btnAddForTP.OnClientClick = "tb_show(null , '../Modal/SalvageRecovery.aspx?PostbackTo=" & updateThirdPartyRecovery.ClientID.ToString & "&PerilIndex=" & iPeril & "&PerilID=" & nClaimPerilId & "&modal=true&KeepThis=true&FromPage=TP&TB_iframe=true&height=600&width=700' , null);return false;"
                End If
                If HttpContext.Current.Session.IsCookieless Then
                    btnAdd.OnClientClick = "tb_show(null , '" & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SalvageRecovery.aspx?PostbackTo=" & updateSalvageRecovery.ClientID.ToString & "&PerilIndex=" & iPeril & "&PerilID=" & nClaimPerilId & "&modal=true&KeepThis=true&FromPage=SA&TB_iframe=true&height=600&width=700' , null);return false;"
                Else
                    btnAdd.OnClientClick = "tb_show(null , '../Modal/SalvageRecovery.aspx?PostbackTo=" & updateSalvageRecovery.ClientID.ToString & "&PerilIndex=" & iPeril & "&PerilID=" & nClaimPerilId & "&modal=true&KeepThis=true&FromPage=SA&TB_iframe=true&height=600&width=700' , null);return false;"
                End If

                'if page is loaded first time then setting of the status of progres bar
                ucProgressBar.OverviewStyle = "complete"
                ucProgressBar.PerilsStyle = "in-progress"
                ucProgressBar.SummaryStyle = "incomplete"
                ucProgressBar.ReinsuranceStyle = "incomplete"
                ucProgressBar.CompleteStyle = "incomplete"

                ltPageHeading.Text = ltPageHeading.Text & oClaim.ClaimPeril(iPeril).Description
                'Binding Salvage Data
                BindDataSalvageRecovery(oClaim.ClaimPeril(iPeril).SalvageRecovery)
                'Binding TPRecovery data
                BindDataThirdPartyRecovery(oClaim.ClaimPeril(iPeril).TPRecovery)
                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        Session(CNRecovery) = oClaim
                    Case Mode.EditClaim
                        Session(CNRecovery) = oClaim
                        BindDataToGVCoInsurance(iPeril)
                        BindDataToGvReInsurance(iPeril)
                    Case Mode.ViewClaim, Mode.PayClaim, Mode.SalvageClaim, Mode.TPRecovery
                        btnAdd.Visible = False
                        btnAddForTP.Visible = False

                        If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.SalvageClaim _
                        Or CType(Session(CNMode), Mode) = Mode.TPRecovery Then
                            BindDataToGVCoInsurance(iPeril)
                            BindDataToGvReInsurance(iPeril)
                        End If
                End Select
                If CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                    DisableControls(GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName))
                End If
            Else

                Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                'For Salvage
                Dim oSalvageRecovery As NexusProvider.PerilRecoveryCollection = oClaim.ClaimPeril(iPeril).SalvageRecovery
                BindDataSalvageRecovery(oSalvageRecovery)

                'For TP Recovery
                Dim oThirdPartyRecovery As NexusProvider.PerilRecoveryCollection = oClaim.ClaimPeril(iPeril).TPRecovery
                BindDataThirdPartyRecovery(oThirdPartyRecovery)
            End If

            If m_sIsRecoveriesReadOnly = "1" Then
                btnAdd.Enabled = False
                btnAddForTP.Enabled = False
            End If
        End Sub

        ''' <summary>
        ''' Bind Salvage Recovery Data
        ''' </summary>
        ''' <param name="oRecoveryCollection"></param>
        ''' <remarks></remarks>
        Protected Sub BindDataSalvageRecovery(ByVal oRecoveryCollection As NexusProvider.PerilRecoveryCollection)
            Dim oSalvageCollection As New NexusProvider.PerilRecoveryCollection
            For iCount As Integer = 0 To oRecoveryCollection.Count - 1
                If oRecoveryCollection(iCount).IsDeleted = False Then
                    oSalvageCollection.Add(oRecoveryCollection(iCount))
                End If
            Next
            gvRecovery.DataSource = oSalvageCollection
            gvRecovery.DataBind()
        End Sub

        ''' <summary>
        ''' Bind Third Party Data
        ''' </summary>
        ''' <param name="oRecoveryCollection"></param>
        ''' <remarks></remarks>
        Protected Sub BindDataThirdPartyRecovery(ByVal oRecoveryCollection As NexusProvider.PerilRecoveryCollection)
            Dim oTPCollection As New NexusProvider.PerilRecoveryCollection
            For iCount As Integer = 0 To oRecoveryCollection.Count - 1
                If oRecoveryCollection(iCount).IsDeleted = False Then
                    oTPCollection.Add(oRecoveryCollection(iCount))
                End If
            Next
            gvRecoveryAmountForTP.DataSource = oTPCollection
            gvRecoveryAmountForTP.DataBind()
        End Sub
        ''' <summary>
        ''' BindDataToGVCoInsurance
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <remarks></remarks>
        Protected Sub BindDataToGVCoInsurance(ByVal iPerilIndex As Integer)
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oCoInsurersCollections As NexusProvider.CoInsurersCollections = Nothing
            Dim oTPCoInsurersCollections As NexusProvider.CoInsurersCollections = Nothing

            If CType(Session(CNMode), Mode) = Mode.EditClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                oCoInsurersCollections = oWebService.GetRecoveryCoinsurance(oClaim.ClaimPeril(iPerilIndex).ClaimPerilKey, True)
            ElseIf CType(Session(CNMode), Mode) = Mode.TPRecovery Then
                oTPCoInsurersCollections = oWebService.GetRecoveryCoinsurance(oClaim.ClaimPeril(iPerilIndex).ClaimPerilKey, False)
            End If

            gvCoInsurance.DataSource = oCoInsurersCollections
            gvCoInsurance.DataBind()

            gvTPCoInsurance.DataSource = oTPCoInsurersCollections
            gvTPCoInsurance.DataBind()
        End Sub
        ''' <summary>
        ''' BindDataToGvReInsurance
        ''' </summary>
        ''' <param name="iPerilIndex"></param>
        ''' <remarks></remarks>
        Protected Sub BindDataToGvReInsurance(ByVal iPerilIndex As Integer)
            Dim oClaim As NexusProvider.ClaimOpen = Nothing
            oClaim = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oReinsurancesCollection As NexusProvider.ReinsurancesCollection = Nothing
            Dim oTPReinsurancesCollection As NexusProvider.ReinsurancesCollection = Nothing

            If CType(Session(CNMode), Mode) = Mode.EditClaim Or CType(Session(CNMode), Mode) = Mode.ViewClaim Then
                oReinsurancesCollection = oWebService.GetRecoveryReinsurance(oClaim.ClaimPeril(iPerilIndex).ClaimPerilKey, True)
                oTPReinsurancesCollection = oWebService.GetRecoveryReinsurance(oClaim.ClaimPeril(iPerilIndex).ClaimPerilKey, False)
            End If

            gvReInsurance.DataSource = oReinsurancesCollection
            gvReInsurance.DataBind()

            gvTPReInsurance.DataSource = oTPReinsurancesCollection
            gvTPReInsurance.DataBind()
        End Sub

        ''' <summary>
        ''' gvRecovery_RowDataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRecovery_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRecovery.RowDataBound
            If e.Row.RowType = DataControlRowType.Header Then
                Dim bShowPartyColumns As Boolean = (CType(Session(CNMode), Mode) = Mode.EditClaim OrElse CType(Session(CNMode), Mode) = Mode.NewClaim)
                e.Row.Cells(1).Visible = bShowPartyColumns
                e.Row.Cells(2).Visible = bShowPartyColumns
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim bShowPartyColumns As Boolean = (CType(Session(CNMode), Mode) = Mode.EditClaim OrElse CType(Session(CNMode), Mode) = Mode.NewClaim)
                e.Row.Cells(1).Visible = bShowPartyColumns
                e.Row.Cells(2).Visible = bShowPartyColumns

                Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                Dim screen As String = Request("FromPage")
                Dim oFormatStringCurrency As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                Dim lblTotalReserve As Literal = e.Row.FindControl("lbl_TotalReserve")
                'lblTotalReserve.Text = FormatNumber(CType(e.Row.DataItem, NexusProvider.PerilRecovery).InitialRecovery + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisionAmount + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisedRecovery, 2)
                lblTotalReserve.Text = String.Format(oFormatStringCurrency, CType(e.Row.DataItem, NexusProvider.PerilRecovery).InitialRecovery + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisionAmount + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisedRecovery)

                Dim lnkhypEdit As LinkButton = e.Row.FindControl("lblhypEdit")
                Dim lnkhypDelete As LinkButton = e.Row.FindControl("lblhypDelete")

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("BaseRecoveryKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PerilRecovery).BaseRecoveryKey)
                If CType(e.Row.DataItem, NexusProvider.PerilRecovery).ClaimPerilId <> nClaimPerilId AndAlso CType(e.Row.DataItem, NexusProvider.PerilRecovery).ClaimPerilId <> 0 Then
                    e.Row.Visible = False
                End If

                If HttpContext.Current.Session.IsCookieless Then
                    lnkhypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SalvageRecoveryForEdit.aspx?PostbackTo=" & updateSalvageRecovery.ClientID.ToString & "&TypeCode=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).TypeCode & "&PerilIndex=" & iPeril & "&RecoveryPartyKey=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).RecoveryPartyKey & "&modal=true&KeepThis=true&FromPage=SA&TB_iframe=true&height=600&width=650' , null);return false;"
                Else
                    lnkhypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/SalvageRecoveryForEdit.aspx?PostbackTo=" & updateSalvageRecovery.ClientID.ToString & "&TypeCode=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).TypeCode & "&PerilIndex=" & iPeril & "&RecoveryPartyKey=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).RecoveryPartyKey & "&modal=true&KeepThis=true&FromPage=SA&TB_iframe=true&height=600&width=650' , null);return false;"
                End If

                If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.PayClaim _
                 Or CType(Session(CNMode), Mode) = Mode.TPRecovery Or CType(Session(CNMode), Mode) = Mode.SalvageClaim Then
                    lnkhypEdit.Visible = False
                    lnkhypDelete.Visible = False
                ElseIf CType(e.Row.DataItem, NexusProvider.PerilRecovery).CanDelete = False _
                And (CType(Session(CNMode), Mode) = Mode.EditClaim Or CType(Session(CNMode), Mode) = Mode.NewClaim) Then
                    lnkhypDelete.Attributes.Add("onclick", "return Warning()")
                End If
                If m_sIsRecoveriesReadOnly = "1" Then
                    lnkhypEdit.Text = "View"
                    lnkhypDelete.Visible = False
                End If
            End If
        End Sub
        ''' <summary>
        ''' gvRecovery_RowDeleting
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRecovery_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvRecovery.RowDeleting
            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            If CType(Session(CNMode), Mode) = Mode.NewClaim Or CType(Session(CNMode), Mode) = Mode.EditClaim Then
                If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery(e.RowIndex).IsNew = True Then
                    CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery.RemoveAt(e.RowIndex)
                Else
                    CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery(e.RowIndex).IsDeleted = True
                End If
            End If
            Dim oSalvageRecovery As NexusProvider.PerilRecoveryCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).SalvageRecovery
            BindDataSalvageRecovery(oSalvageRecovery)
        End Sub
        ''' <summary>
        ''' gvRecoveryAmountForTP_RowDataBound
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRecoveryAmountForTP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRecoveryAmountForTP.RowDataBound
            If e.Row.RowType = DataControlRowType.Header Then
                Dim bShowPartyColumns As Boolean = (CType(Session(CNMode), Mode) = Mode.EditClaim OrElse CType(Session(CNMode), Mode) = Mode.NewClaim)
                e.Row.Cells(1).Visible = bShowPartyColumns
                e.Row.Cells(2).Visible = bShowPartyColumns
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim bShowPartyColumns As Boolean = (CType(Session(CNMode), Mode) = Mode.EditClaim OrElse CType(Session(CNMode), Mode) = Mode.NewClaim)
                e.Row.Cells(1).Visible = bShowPartyColumns
                e.Row.Cells(2).Visible = bShowPartyColumns

                Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
                Dim lblTotalReserve As Literal = e.Row.FindControl("lbl_TotalReserve")
                'lblTotalReserve.Text = FormatNumber(CType(e.Row.DataItem, NexusProvider.PerilRecovery).InitialRecovery + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisionAmount + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisedRecovery, 2)
                Dim oFormatString As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).FormatStrings.FormatString("Currency").DataFormatString
                lblTotalReserve.Text = String.Format(oFormatString, CType(e.Row.DataItem, NexusProvider.PerilRecovery).InitialRecovery + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisionAmount + CType(e.Row.DataItem, NexusProvider.PerilRecovery).RevisedRecovery)

                Dim lnkhypEdit As LinkButton = e.Row.FindControl("lblhypEdit")
                Dim lnkhypDelete As LinkButton = e.Row.FindControl("lblhypDelete")

                'NOTE - this will need to be changed to give each row a unique id
                'this needs to be matched in markup for the menu (id="Menu_<%# Eval("BaseRecoveryKey") %>")
                e.Row.Attributes.Add("id", CType(e.Row.DataItem, NexusProvider.PerilRecovery).BaseRecoveryKey)
                If CType(e.Row.DataItem, NexusProvider.PerilRecovery).ClaimPerilId <> nClaimPerilId AndAlso CType(e.Row.DataItem, NexusProvider.PerilRecovery).ClaimPerilId <> 0 Then
                    e.Row.Visible = False
                End If

                If HttpContext.Current.Session.IsCookieless Then
                    lnkhypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SalvageRecoveryForEdit.aspx?PostbackTo=" & updateThirdPartyRecovery.ClientID.ToString & "&TypeCode=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).TypeCode & "&PerilIndex=" & iPeril & "&RecoveryPartyKey=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).RecoveryPartyKey & "&modal=true&KeepThis=true&FromPage=TP&TB_iframe=true&height=600&width=500' , null);return false;"
                Else
                    lnkhypEdit.OnClientClick = "tb_show(null , ' " & AppSettings("WebRoot") & "Modal/SalvageRecoveryForEdit.aspx?PostbackTo=" & updateThirdPartyRecovery.ClientID.ToString & "&TypeCode=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).TypeCode & "&PerilIndex=" & iPeril & "&RecoveryPartyKey=" & CType(e.Row.DataItem, NexusProvider.PerilRecovery).RecoveryPartyKey & "&modal=true&KeepThis=true&FromPage=TP&TB_iframe=true&height=600&width=500' , null);return false;"
                End If

                If CType(Session(CNMode), Mode) = Mode.ViewClaim Or CType(Session(CNMode), Mode) = Mode.PayClaim _
                                Or CType(Session(CNMode), Mode) = Mode.TPRecovery Or CType(Session(CNMode), Mode) = Mode.SalvageClaim Then
                    lnkhypDelete.Visible = False
                    lnkhypEdit.Visible = False
                ElseIf CType(e.Row.DataItem, NexusProvider.PerilRecovery).CanDelete = False _
                And (CType(Session(CNMode), Mode) = Mode.EditClaim Or CType(Session(CNMode), Mode) = Mode.NewClaim) Then
                    lnkhypDelete.Attributes.Add("onclick", "return Warning()")
                End If
                If m_sIsRecoveriesReadOnly = "1" Then
                    lnkhypEdit.Text = "View"
                    lnkhypDelete.Visible = False
                End If
            End If
        End Sub
        ''' <summary>
        ''' gvRecoveryAmountForTP_RowCommand
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRecoveryAmountForTP_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvRecoveryAmountForTP.RowCommand
            Select Case e.CommandName
                Case "Delete"
            End Select
        End Sub
        ''' <summary>
        ''' gvRecoveryAmountForTP_RowDeleting
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub gvRecoveryAmountForTP_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvRecoveryAmountForTP.RowDeleting
            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            If CType(Session(CNMode), Mode) = Mode.NewClaim Or CType(Session(CNMode), Mode) = Mode.EditClaim Then
                If CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery(e.RowIndex).IsNew = True Then
                    CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery.RemoveAt(e.RowIndex)
                Else
                    CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery(e.RowIndex).IsDeleted = True
                End If
            End If
            Dim oThirdPartyRecovery As NexusProvider.PerilRecoveryCollection = CType(Session(CNClaim), NexusProvider.ClaimOpen).ClaimPeril(iPeril).TPRecovery
            BindDataThirdPartyRecovery(oThirdPartyRecovery)
        End Sub
        ''' <summary>
        ''' btnNext_Click
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub btnNext_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNext.Click
            Dim oOpenClaim As NexusProvider.ClaimOpen = CType(Session(CNClaim), NexusProvider.ClaimOpen)
            Dim iPeril As Integer = CInt(Request.QueryString("PerilIndex"))
            Dim oClaimResponse As NexusProvider.ClaimResponse = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            If String.IsNullOrEmpty(m_sIsRecoveriesReadOnly) OrElse m_sIsRecoveriesReadOnly = "0" Then
                'Flag to indetify the updated peril and need to update the data inDB
                oOpenClaim.ClaimPeril(iPeril).PerilEdited = True
                Select Case CType(Session(CNMode), Mode)
                    Case Mode.NewClaim
                        'Update the recovery reserve
                        'arch issue 268
                        'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oOpenClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
                        oClaimResponse = UpdateClaimReservesOrPaymentsCall(oOpenClaim, Nothing, Session.Item(CNClaimTimeStamp), 1, sBranchCode)
                        If oClaimResponse Is Nothing Then
                            Exit Sub
                        End If
                        'Update the session variable
                        GetClaimDetails(oOpenClaim.ClaimKey, Nothing)
                    Case Mode.EditClaim
                        'Update the recovery reserve
                        'arch issue 268
                        'oClaimResponse = oWebservice.UpdateClaimReservesOrPayments(oOpenClaim, Nothing, Session.Item(CNClaimTimeStamp), 2, sBranchCode)
                        oClaimResponse = UpdateClaimReservesOrPaymentsCall(oOpenClaim, Nothing, Session.Item(CNClaimTimeStamp), 2, sBranchCode)
                        If oClaimResponse Is Nothing Then
                            Exit Sub
                        End If
                        'Update the session variable
                        GetClaimDetails(oOpenClaim.ClaimKey, Nothing)
                End Select
            End If
            If Session(CNClaimBuilder) IsNot Nothing AndAlso Session(CNClaimBuilder) = True Then
                If Request.QueryString("ReturnURL") IsNot Nothing Then
                    Response.Redirect(Request.QueryString("ReturnURL"), False)
                Else
                    Response.Redirect(AppSettings("WebRoot") & "Claims/Perils.aspx", False)
                End If
            Else
                Response.Redirect(AppSettings("WebRoot") & "Claims/Perils.aspx", False)
            End If
        End Sub

        Sub GetClaimDetails(ByVal v_iClaimKey As Integer, ByVal oClaimRisk As NexusProvider.ClaimRisk)
            Dim oClaimDetails As NexusProvider.ClaimDetails = Nothing
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oOriginalClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oQuote As NexusProvider.Quote = Session(CNClaimQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            'Retreiving the latest details
            'arch issue 268
            oClaimDetails = GetClaimDetailsCall(v_iClaimKey, sBranchCode)
            'updation of latest session values 
            Session.Item(CNClaimTimeStamp) = oClaimDetails.TimeStamp
            'If there is no need to update the claim risk details
            If oClaimRisk IsNot Nothing Then
                Session.Item(CNClaimRiskTimeStamp) = oClaimRisk.TimeStamp
                Session.Item(CNDataSet) = oClaimRisk.XMLDataSet
            End If
            Session.Item(CNBaseClaimKey) = oClaimDetails.BaseClaimKey
            Session.Item(CNClaimKey) = oClaimDetails.ClaimKey
            Session.Item(CNClaimNumber) = oClaimDetails.ClaimNumber

            With oClaimDetails
                oOriginalClaim.CatastropheCode = .CatastropheCode
                oOriginalClaim.BaseClaimKey = .BaseClaimKey
                oOriginalClaim.Claim = .Claim
                oOriginalClaim.ClaimCoInsurer = .ClaimCoInsurer
                oOriginalClaim.ClaimDescription = .ClaimDescription
                oOriginalClaim.ClaimHandlerDescription = .ClaimHandlerDescription
                oOriginalClaim.ClaimKey = .ClaimKey
                oOriginalClaim.ClaimNumber = .ClaimNumber
                oOriginalClaim.ClaimPeril = .ClaimPeril
                oOriginalClaim.ClaimStatus = .ClaimStatus
                oOriginalClaim.ClaimStatusDate = .ClaimStatusDate
                oOriginalClaim.ClaimStatusID = .ClaimStatusID
                oOriginalClaim.ClaimVersion = .ClaimVersion
                oOriginalClaim.ClaimVersionDescription = .ClaimVersionDescription
                oOriginalClaim.ClientClaimNumber = .ClientClaimNumber
                oOriginalClaim.ClientEmail = .ClientEmail
                oOriginalClaim.ClientFaxNo = .ClientFaxNo
                oOriginalClaim.ClientMobileNo = .ClientMobileNo
                oOriginalClaim.ClientName = .ClientName
                oOriginalClaim.ClientShortName = .ClientShortName
                oOriginalClaim.ClientTelNo = .ClientTelNo
                oOriginalClaim.ClientTelNoOff = .ClientTelNoOff
                oOriginalClaim.CloseClaimOnZeroReserveRecoveryBalance = .CloseClaimOnZeroReserveRecoveryBalance
                oOriginalClaim.Comments = .Comments
                oOriginalClaim.Contact = .Contact
                oOriginalClaim.CurrencyISOCode = .CurrencyISOCode
                oOriginalClaim.Description = .Description
                oOriginalClaim.ExternalHandler = .ExternalHandler
                oOriginalClaim.HandlerCode = .HandlerCode
                oOriginalClaim.IgnoreClaimMaintain = .IgnoreClaimMaintain
                oOriginalClaim.InfoOnly = .InfoOnly
                oOriginalClaim.InsuranceFileKey = .InsuranceFileKey
                oOriginalClaim.InsuranceRef = .InsuranceRef
                oOriginalClaim.InsurerClaimNumber = .InsurerClaimNumber
                oOriginalClaim.IsAllowedClosedClaims = .IsAllowedClosedClaims
                oOriginalClaim.IsDeleted = .IsDeleted
                oOriginalClaim.LastModifiedDate = .LastModifiedDate
                oOriginalClaim.LikelyClaim = .LikelyClaim
                oOriginalClaim.Location = .Location
                oOriginalClaim.LossDate = .LossDate
                oOriginalClaim.LossDateFrom = .LossDateFrom
                oOriginalClaim.LossFromDate = .LossToDate
                oOriginalClaim.LossToDate = .LossToDate
                oOriginalClaim.LossToDateSpecified = .LossToDateSpecified
                oOriginalClaim.Payments = .Payments
                oOriginalClaim.PolicyNumber = .PolicyNumber
                oOriginalClaim.PolicyType = .PolicyType
                oOriginalClaim.PrimaryCause = .PrimaryCause
                oOriginalClaim.PrimaryCauseCode = .PrimaryCauseCode
                oOriginalClaim.PrimaryCauseDescription = .PrimaryCauseDescription
                oOriginalClaim.ProductDescription = .ProductDescription
                oOriginalClaim.ProgressStatusCode = .ProgressStatusCode
                oOriginalClaim.ProgressStatusDescription = .ProgressStatusDescription
                oOriginalClaim.ReportedDate = .ReportedDate
                oOriginalClaim.RiskKey = .RiskKey
                oOriginalClaim.SecondaryCause = .SecondaryCause
                oOriginalClaim.SecondaryCauseCode = .SecondaryCauseCode
                oOriginalClaim.SecondaryCauseDescription = .SecondaryCauseDescription
                oOriginalClaim.TotalCurrentShareValue = .TotalCurrentShareValue
                oOriginalClaim.TotalShare = .TotalShare
                oOriginalClaim.Town = .Town
                oOriginalClaim.TownCode = .TownCode
                oOriginalClaim.UnderwritingYearCode = .UnderwritingYearCode
                oOriginalClaim.UserDefFldACode = .UserDefFldACode
                oOriginalClaim.UserDefFldBCode = .UserDefFldBCode
                oOriginalClaim.UserDefFldCCode = .UserDefFldCCode
                oOriginalClaim.UserDefFldDCode = .UserDefFldECode
                oOriginalClaim.UserDefFldECode = .UserDefFldECode
            End With
            Session.Item(CNClaim) = oOriginalClaim
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "Confirmation",
                                        "<script language=""JavaScript"" type=""text/javascript"">function Warning(){alert('" & GetLocalResourceObject("msg_DeleteRecoveryWarning").ToString() & "'); return false;}</script>")
        End Sub
    End Class
End Namespace

