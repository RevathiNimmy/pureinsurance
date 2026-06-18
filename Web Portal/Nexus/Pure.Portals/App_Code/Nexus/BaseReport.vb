Imports Nexus.Utils
Imports Nexus.Library
Imports CMS.Library
Imports System.Web.Configuration.WebConfigurationManager

Namespace Nexus

    ''' <summary>
    ''' All the controls created for reports must be inherit from "BaseReport" class 
    ''' </summary>
    ''' <remarks></remarks>
    Public MustInherit Class BaseReport : Inherits System.Web.UI.UserControl

        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)

        ''' <summary>
        ''' "GenerateReport" must be associated with Click event of "Submit" button of Report Controls
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub GenerateReport(ByVal sender As Object, ByVal e As System.EventArgs)
            If Page.IsValid Then

                Dim oParametersCollection As New NexusProvider.ParametersCollection
                Dim oMaster As ContentPlaceHolder
                Dim sPlaceHolderControlID As String = "plcReportForm"
                Dim sDocumentFormatTypeControlID As String = "ddlDocumentFormatType"
                Dim sUrl As String = String.Empty
                Dim sReportsTypeControlID As String = "ddlReportsType"
                Dim sSelectedReportsType As String = Nothing
                Dim sCustomValidator As String = "cusReportForm"

                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)

                Dim sPlaceHolderReportForm As PlaceHolder = CType(oMaster.FindControl(sPlaceHolderControlID), PlaceHolder)

                'run the loop in the report control and collect the oParametersCollection for SAM request
                CollectParametersForSAMRequest(sPlaceHolderReportForm.Controls(0), oParametersCollection)

                'get the name of the selected Report to be generated
                Dim oddlReportsType As DropDownList = CType(oMaster.FindControl(sReportsTypeControlID), DropDownList)
                If oddlReportsType IsNot Nothing Then
                    sSelectedReportsType = oddlReportsType.SelectedValue
                End If
                'Executed Function from Dataset function
                Try

                    Session("Parameters") = oParametersCollection
                    sUrl = GetReportUrl(sSelectedReportsType, oParametersCollection)
                    'add client side code to open new window containing report
                    Page.ClientScript.RegisterStartupScript(GetType(String), "openReport", "var newWin=window.open('" & sUrl & "');newWin.focus();", True)

                Catch ex As NexusProvider.NexusException
                    'Checking  (bSIRReportPrint.Business.SendToPrint Failed : Failed : Return Value = PMNotFound) Error code , then display a message saying no record found 
                    If ex.Errors(0).Code = "1000019" Then
                        ex.Errors(0).Code = "88"
                    End If
                    Throw
                End Try

                'add client side code to open new window containing report
                'Page.ClientScript.RegisterStartupScript(GetType(String), "openReport", "var newWin=window.open('" & sUrl & "');newWin.focus();", True)
                ScriptManager.RegisterStartupScript(Me.Page, GetType(String), "openReport", "var newWin=window.open('" & sUrl & "');newWin.focus();", True)
                'cleaning up
                oParametersCollection = Nothing
                oMaster = Nothing
                sPlaceHolderControlID = Nothing
                sDocumentFormatTypeControlID = Nothing

            End If
        End Sub
        ''' <summary>
        ''' This will run the loop in the report control and collect the collection of params for SAM request
        ''' </summary>
        ''' <param name="v_oContainer"></param>
        ''' <param name="oParametersCollection"></param>
        ''' <remarks></remarks>
        Sub CollectParametersForSAMRequest(ByVal v_oContainer As Control, ByRef oParametersCollection As NexusProvider.ParametersCollection)

            Dim oControl As Object
            Dim sControlName() As String
            Dim sControlValue As String = String.Empty
            Dim iCountVar As Integer = 0
            Dim bSave As Boolean = False
            Dim oParameters As NexusProvider.Parameters
            Dim oChildControl As Control
            For Each oControl In v_oContainer.Controls

                If oControl.ID IsNot Nothing Then

                    bSave = False
                    sControlName = Regex.Split(oControl.ID, "__")

                    If sControlName.Length > 1 Then
                        'if length is greater than 1, means these are the data fields placed in report controls

                        Select Case oControl.GetType.Name

                            Case "HiddenField"

                                sControlValue = CType(oControl, HiddenField).Value
                                bSave = True

                            Case "TextBox"
                                If IsDate(CType(oControl, TextBox).Text) Then
                                    sControlValue = Convert.ToDateTime(CType(oControl, TextBox).Text).ToString("dd/MMM") + "/" + Convert.ToDateTime(CType(oControl, TextBox).Text).Year.ToString()
                                Else
                                    sControlValue = CType(oControl, TextBox).Text
                                End If
                                bSave = True

                            Case "DropDownList"

                                sControlValue = CType(oControl, DropDownList).SelectedValue
                                bSave = True

                            Case "LookupList"
                                sControlValue = CType(oControl, NexusProvider.LookupList).Value
                                bSave = True


                        End Select

                        If bSave Then

                            'add into the collection only if bSave=true
                            oParameters = New NexusProvider.Parameters
                            oParameters.ParamNameField = sControlName(1)
                            oParameters.ParamValueField = sControlValue.ToString

                            'add the param into the collection
                            oParametersCollection.Add(oParameters)
                            iCountVar = iCountVar + 1

                        End If

                    ElseIf Right(oControl.GetType.Name.ToString, 4).Equals("ascx") Then

                        'if length is 1, means its a user control inside the placeholder so make recursive call
                        CollectParametersForSAMRequest(oControl, oParametersCollection)
                    Else
                        oChildControl = CType(oControl, Control)
                        If oChildControl.HasControls Then
                            CollectParametersForSAMRequest(oChildControl, oParametersCollection)
                        End If

                    End If

                End If

            Next

        End Sub

    End Class

End Namespace
