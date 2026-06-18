Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Library
Imports Nexus.Utils
Imports Nexus.Constants
Imports Nexus.Constants.Session

Namespace Nexus

    Partial Class Modal_CoverNoteSheet
        Inherits System.Web.UI.Page

        Dim oWebService As NexusProvider.ProviderBase
        Dim sEdit As String = "Edit"

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ViewState.Add(CNCoverMode, Request.QueryString("Mode"))
            ViewState.Add("CoverNoteSheetNumber", Request.QueryString("CoverNoteSheetNumber"))

            'Ram begin: Add the startnumber and endnumber to view state
            ViewState.Add(CNStartNumber, Request.QueryString("SN"))
            ViewState.Add(CNEndNumber, Request.QueryString("EN"))
            'Ram end: Add the startnumber and endnumber to view state
            If Not IsPostBack Then
                ' populate the page
                PopulatePage()
                txtAssignedDate.Text = CDate(Date.Now.ToShortDateString)
                txtAssignedDate.Enabled = False
            End If

        End Sub
        Sub FillCoverNoteSheetStatus()
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oList As NexusProvider.LookupListCollection = oWebService.GetList(NexusProvider.ListType.PMLookup, "cover_note_sheet_status", True, False)
            ddlSheetStatus.Items.Clear()
            ddlSheetStatus.DataSource = oList
            ddlSheetStatus.DataTextField = "Description"
            ddlSheetStatus.DataValueField = "Code"
            ddlSheetStatus.DataBind()
        End Sub
        Protected Sub PopulatePage()

            ' Instansiating object for use
            oWebService = New NexusProvider.ProviderManager().Provider
            Dim oCovernote As New NexusProvider.CoverNote
            Dim oCoverNoteSheet As New NexusProvider.CoverNoteSheetType
           
            Try
                ' assigning values to properties 
                oCovernote.CoverNoteBookKey = CType(Request.QueryString("CoverNoteBookKey"), Integer)
                oCoverNoteSheet.CoverNoteSheetNumber = CType(Request.QueryString("CoverNoteSheetNumber"), Integer)
                oCovernote.CoverNoteSheets.Add(oCoverNoteSheet)
                oCoverNoteSheet = Nothing

                FillCoverNoteSheetStatus()

                If ViewState.Item(CNCoverMode) = sEdit Then
                  
                    'To set the Focus
                    Page.SetFocus(txtComments)

                    ' obtaining values from SAM
                    oWebService.GetCoverNoteSheet(oCovernote)
                    oCoverNoteSheet = oCovernote.CoverNoteSheets(0)
                    txtSheetNumber.Text = oCoverNoteSheet.CoverNoteSheetNumber
                    txtPolicyNumber.Text = oCoverNoteSheet.PolicyNumber

                    txtPolicyNumber.Attributes.Add("readonly", "readonly")

                    ViewState("InsuranceFileCnt") = oCovernote.InsuranceFileDetails.InsuranceFileCnt
                    'If Not oCovernote.AssignedD  Then
                    If Not CDate(oCovernote.AssignedDate).ToShortDateString = CDate("01/01/0001").ToShortDateString Then
                        txtAssignedDate.Text = CDate(oCovernote.AssignedDate)
                    End If
                    
                    ddlSheetStatus.SelectedValue = oCovernote.Code
                    If oCovernote.Code.Trim.ToUpper = "ISSUED" Then
                        ddlSheetStatus.Enabled = False
                    Else
                        txtSheetNumber.Attributes.Add("readonly", "readonly")
                    End If

                    txtComments.Text = oCovernote.Comments
                    Session(CNTimeStamp) = oCovernote.CoverNoteBookTimestamp
                    txtPolicyNumber.Enabled = False

                Else
                  
                    'To set the Focus
                    Page.SetFocus(txtSheetNumber)

                    ddlSheetStatus.SelectedValue = "NOTISS"
                    ddlSheetStatus.Enabled = False
                    txtAssignedDate.Text = CDate(DateTime.Now.ToShortDateString)
                    txtPolicyNumber.Enabled = False
                End If

            Finally
                oWebService = Nothing
                oCovernote = Nothing
            End Try


        End Sub

        Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit

            ' assigning values to ViewState and setting styles for the page
            CMS.Library.Frontend.Functions.SetTheme(Page, AppSettings("ModalPageTemplate"))
           

        End Sub

        Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            If Page.IsValid Then
                ' Instansiating object for use
                oWebService = New NexusProvider.ProviderManager().Provider
                Dim oCovernote As New NexusProvider.CoverNote
                Dim oCoverNoteSheet As New NexusProvider.CoverNoteSheetType

                Try

                    ' assigning values to properties 
                    oCovernote.CoverNoteBookKey = CType(Request.QueryString("CoverNoteBookKey"), Integer)
                    Dim sCoverNoteSheetNumber As String = Request.QueryString("CoverNoteSheetNumber")

                    If Not sCoverNoteSheetNumber = String.Empty Then
                        oCoverNoteSheet.CoverNoteSheetNumber = CType(sCoverNoteSheetNumber, Integer)
                    Else
                        oCoverNoteSheet.CoverNoteSheetNumber = txtSheetNumber.Text.Trim()
                    End If

                    oCoverNoteSheet.PolicyNumber = txtPolicyNumber.Text.Trim()
                    'oCoverNoteSheet.DateImported = txtAssignedDate.Text

                    oCovernote.Comments = txtComments.Text.Trim()
                    oCovernote.CoverNoteBookTimestamp = Session(CNTimeStamp)
                    If Not txtAssignedDate.Text.Trim().Length = 0 Then
                        oCovernote.AssignedDate = CDate(txtAssignedDate.Text) 'CType(txtAssignedDate.Text, Date)
                    End If

                    If ViewState.Item(CNCoverMode) = sEdit Then

                        oCovernote.CoverNoteStatusCode = ddlSheetStatus.SelectedValue
                        oCoverNoteSheet.NewCoverNoteSheetNumber = CType(ViewState.Item("CoverNoteSheetNumber"), Integer)
                        oCoverNoteSheet.OldCoverNoteSheetNumber = CType(ViewState.Item("CoverNoteSheetNumber"), Integer)
                        oCovernote.InsuranceFileDetails.InsuranceFileCnt = ViewState("InsuranceFileCnt")
                        oCovernote.CoverNoteSheets.Add(oCoverNoteSheet)
                        ' updating values to SAM
                        oWebService.UpdateCoverNoteSheet(oCovernote)

                    Else
                        oCovernote.CoverNoteBookStatusCode = ddlSheetStatus.SelectedValue
                        oCovernote.CoverNoteSheets.Add(oCoverNoteSheet)
                        oCovernote.CoverNoteSheets.Add(oCoverNoteSheet)
                        ' adding values to SAM

                        'begin: Check for already existing Coversheet Number
                        For Each coverNoteSheetItem As NexusProvider.CoverNoteSheetType In Session(CNCoverNoteSheetData)
                            If coverNoteSheetItem.CoverNoteSheetNumber = Me.txtSheetNumber.Text Then
                                PnlError.Visible = True
                                lblError.Text = GetLocalResourceObject("Err_CSNExist") '"CoverSheetNumber already exists."
                                Exit Sub
                            End If
                        Next
                        'end: Check for already existing Coversheet Number

                        ' begin: Check for cover note range
                        If oCoverNoteSheet.CoverNoteSheetNumber >= ViewState.Item(CNStartNumber) _
                                AndAlso oCoverNoteSheet.CoverNoteSheetNumber <= ViewState.Item(CNEndNumber) Then
                            oWebService.AddCoverNoteSheet(oCovernote)
                        Else
                            PnlError.Visible = True
                            lblError.Text = GetLocalResourceObject("Err_CSNRange") '"Cover Note Sheet Number should be within the specified range."
                            Exit Sub
                        End If
                        'end: Check for cover note range

                    End If
                Finally

                    oWebService = Nothing
                    oCoverNoteSheet = Nothing
                    oCovernote = Nothing

                End Try
                'Close the thickbox once new Cover Note Sheet is added
                Page.ClientScript.RegisterStartupScript(GetType(String), "closeThickBox", "self.parent.tb_updated('" & Request.QueryString("PostbackTo") & "','RefreshGrid');", True)
                'Close the thickbox once new Cover Note Sheet is added
            End If
        End Sub

        Protected Sub cusValidSheetNo_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cusValidSheetNo.ServerValidate
            Dim iResult As Integer
            If Integer.TryParse(txtSheetNumber.Text.Trim, iResult) = False Then
                args.IsValid = False
                cusValidSheetNo.ErrorMessage = GetLocalResourceObject("Err_VldSheetNumber")
            ElseIf iResult <= 0 Then
                args.IsValid = False
                cusValidSheetNo.ErrorMessage = GetLocalResourceObject("Err_VldSheetNumber")
            ElseIf ViewState.Item(CNCoverMode) = sEdit And ddlSheetStatus.SelectedValue = "ISSUED" Then
                args.IsValid = False
                cusValidSheetNo.ErrorMessage = GetLocalResourceObject("Err_InvalidSheetStatus")
            Else
                args.IsValid = True
            End If
        End Sub
    End Class

End Namespace