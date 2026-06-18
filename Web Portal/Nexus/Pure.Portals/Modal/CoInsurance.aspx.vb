Imports System.Web.Configuration.WebConfigurationManager
Imports CMS.Library
Imports Nexus.Constants
Imports Nexus.Constants.Session
Namespace Nexus
    Partial Class Modal_CoInsurance1 : Inherits CMS.Library.Frontend.clsCMSPage

        Public Property Coinsurer() As String
            Get
                Return txtCoinsurer.Text
            End Get
            Set(ByVal value As String)
                txtCoinsurer.Text = value

            End Set
        End Property
        Public Property Arrangement_Ref() As String
            Get
                Return txtArrangementRef.Text
            End Get
            Set(ByVal value As String)
                txtArrangementRef.Text = value

            End Set
        End Property
        Public Property Share() As Double
            Get
                Return txtShare.Text
            End Get
            Set(ByVal value As Double)
                txtShare.Text = value

            End Set
        End Property
        Public Property Commission() As Double
            Get
                Return txtCommission.Text
            End Get
            Set(ByVal value As Double)
                txtCommission.Text = value

            End Set
        End Property
        Public Property CoinsurerKey() As Integer
            Get
                Return hiddenCoinsurerCode.Value
            End Get
            Set(ByVal value As Integer)
                hiddenCoinsurerCode.Value = value

            End Set
        End Property
        Public Property CoInsurance() As NexusProvider.CoInsurers
            Get
                Dim oCoInsurance As NexusProvider.CoInsurers = Nothing
                With oCoInsurance
                    .ArrangementRef = Arrangement_Ref
                    .SharePerc = Share
                    .CommissionPerc = Commission
                    .CoInsurer = Coinsurer
                    .CoInsurerKey = CoinsurerKey

                End With
                Return oCoInsurance
            End Get
            Set(ByVal value As NexusProvider.CoInsurers)
                If value Is Nothing Then
                    Arrangement_Ref = String.Empty
                    Share = Nothing
                    Commission = Nothing
                    Coinsurer = String.Empty
                    CoinsurerKey = Nothing
                Else
                    With value
                        Arrangement_Ref = .ArrangementRef
                        Share = .SharePerc
                        Commission = .CommissionPerc
                        Coinsurer = .CoInsurer
                        CoinsurerKey = .CoInsurerKey

                    End With

                End If

            End Set
        End Property


        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                If Request("CoInsuranceID") Is Nothing Then
                    btnAdd.Visible = True
                    btnUpdate.Visible = False
                Else
                    btnAdd.Visible = False
                    btnUpdate.Visible = True

                    Dim oCoInsurance As NexusProvider.CoInsurers = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections).Item(CType(Request("CoInsuranceID"), Integer))
                    txtCoinsurer.Text = oCoInsurance.CoInsurer
                    txtArrangementRef.Text = oCoInsurance.ArrangementRef
                    txtShare.Text = oCoInsurance.SharePerc
                    txtCommission.Text = oCoInsurance.CommissionPerc
                    hiddenCoinsurerCode.Value = oCoInsurance.CoInsurerKey


                End If
            End If

            'Me.btnCoInsurer.Attributes.Add("onclick", "javascript:return PopupFindReinsurer()")

        End Sub

        Protected Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            If Page.IsValid Then

                Dim CoInsurer As NexusProvider.CoInsurersCollections = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections)

                Dim oCoInsurer As New NexusProvider.CoInsurers
                With oCoInsurer

                    .ArrangementRef = txtArrangementRef.Text
                    .CoInsurer = txtCoinsurer.Text
                    If txtCommission.Text.Trim.Length > 0 Then
                        If Double.IsNaN(txtCommission.Text) = False Then
                            .CommissionPerc = txtCommission.Text
                        End If
                    End If
                    If txtShare.Text.Trim.Length > 0 Then
                        If Double.IsNaN(txtShare.Text) = False Then
                            .SharePerc = txtShare.Text
                        End If
                    End If

                    .CoInsurerKey = CoinsurerKey
                End With
                CoInsurer.Add(oCoInsurer)

                'Dim PostBackStr As String = "self.parent.__doPostBack('' , 'Refresh');self.parent.tb_remove();"
                'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "');", True)
            End If
        End Sub
        Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
            If Page.IsValid Then

                Dim CoInsurer As NexusProvider.CoInsurersCollections = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections)
                Dim oUpdateCoInsurer As NexusProvider.CoInsurers = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections).Item(CType(Request("CoInsuranceID"), Integer))
                With oUpdateCoInsurer
                    .ArrangementRef = txtArrangementRef.Text
                    .CoInsurer = txtCoinsurer.Text
                    .CommissionPerc = txtCommission.Text
                    .SharePerc = txtShare.Text
                    .CoInsurerKey = hiddenCoinsurerCode.Value
                End With
                If Request("CoInsuranceID") <> "" Then
                    CoInsurer.Update(oUpdateCoInsurer, Request("CoInsuranceID"))
                Else
                    CoInsurer.Update(oUpdateCoInsurer)
                End If

                'Dim PostBackStr As String = "self.parent.__doPostBack('' , 'Refresh');self.parent.tb_remove();"
                'Page.ClientScript.RegisterStartupScript(GetType(String), "ParentPostBack", PostBackStr, True)
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "');", True)
            End If
        End Sub

        Protected Sub Page_PreInit1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
        End Sub

        Protected Sub cusValidCoinsurer_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cusValidCoinsurer.ServerValidate
            Dim CoInsurer As NexusProvider.CoInsurersCollections = CType(Session.Item("CoInsurance"), NexusProvider.CoInsurersCollections)
            If Request("CoInsuranceID") <> "" Then
                Dim iTotalCount As Integer = 0
                Dim oldCoInsuranceKey As Integer = CoInsurer(Request("CoInsuranceID")).CoInsurerKey
                CoInsurer(Request("CoInsuranceID")).CoInsurerKey = hiddenCoinsurerCode.Value.Trim
                For iCount As Integer = 0 To CoInsurer.Count - 1
                    If CoInsurer(iCount).CoInsurerKey = hiddenCoinsurerCode.Value.Trim Then
                        iTotalCount += 1
                    End If
                Next
                If iTotalCount > 1 Then
                    args.IsValid = False
                    CoInsurer(Request("CoInsuranceID")).CoInsurerKey = oldCoInsuranceKey
                Else
                    args.IsValid = True
                End If
            Else
                For iCount As Integer = 0 To CoInsurer.Count - 1
                    If CoInsurer(iCount).CoInsurerKey = hiddenCoinsurerCode.Value.Trim Then
                        args.IsValid = False
                        Exit For
                    Else
                        args.IsValid = True
                    End If
                Next
            End If

            'Check the value of Share and Commision
            If args.IsValid Then
                Dim iShare As Decimal

                If txtShare.Text.Trim.Length <> 0 Then
                    If Decimal.TryParse(txtShare.Text.Trim, iShare) Then
                        If iShare <= 0 Then
                            args.IsValid = False
                            cusValidCoinsurer.ErrorMessage = GetLocalResourceObject("Err_ShareLessThanZero")
                        End If
                    Else
                        args.IsValid = False
                        cusValidCoinsurer.ErrorMessage = GetLocalResourceObject("Err_ShareNAN")
                    End If
                Else
                    args.IsValid = False
                    cusValidCoinsurer.ErrorMessage = GetLocalResourceObject("Err_ShareBlank")
                End If
            End If

            If args.IsValid Then
                Dim iCommision As Decimal

                'For Commission
                If txtCommission.Text.Trim.Length <> 0 Then
                    If Decimal.TryParse(txtCommission.Text.Trim, iCommision) Then
                        If iCommision < 0 Then
                            args.IsValid = False
                            cusValidCoinsurer.ErrorMessage = GetLocalResourceObject("Err_CommisionLessThanZero")
                        End If
                    Else
                        args.IsValid = False
                        cusValidCoinsurer.ErrorMessage = GetLocalResourceObject("Err_CommisionNAN")
                    End If
                End If
            End If
        End Sub
    End Class
End Namespace
