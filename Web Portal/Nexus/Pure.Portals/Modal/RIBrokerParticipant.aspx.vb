Imports CMS.Library
Imports System.Data
Imports Nexus.Library
Imports System.Configuration.ConfigurationManager
Imports Nexus.Constants
Imports Nexus.Constants.Session

Imports System
Imports System.Globalization
Imports System.Threading
Imports NexusProvider.SAMForInsurance
Imports Nexus.Utils
Imports Nexus.Reinsurance
Imports System.Xml
Partial Class Modal_RIBrokerParticipant
    Inherits Frontend.clsCMSPage
    Dim oReinsurerColl As New NexusProvider.ReinsurerCollection
    Dim oBrokerParticipationCol As New BrokerParticipantsCollection
    Dim txtTotalParticipation As TextBox
    Dim oFilteredReinsurerXOL As New ArrangementLinesType
    Dim FilteredReinsurerpageCacheID As New Guid
    Dim oRIArrangemenline As NexusProvider.ReinsuranceArrangementLines
    Dim dTotalParticipationAmount As Double
    Dim oFilteredReinsurerColl As New NexusProvider.ReinsurerCollection
    Dim dPartyTotal As Double
#Region "Page event"

    Protected Sub Page_Load1(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oReinsurer As New NexusProvider.ReinsurerSearchCriteria
        Dim iRIArrangementLineKey As Integer
        If Not IsPostBack Then
            Dim sType As String = Convert.ToString(Request.QueryString("Type")).ToUpper.Trim
            Dim sVMode As String = String.Empty
            Dim sPMode As String = String.Empty
            'create a unique key and add this to viewstate
            'this will be used to cache the results of the SAM call
            Dim RIBrokerParticipantpageCacheID As Guid
            RIBrokerParticipantpageCacheID = Guid.NewGuid()
            ViewState.Add("RIBrokerParticipantpageCacheID", RIBrokerParticipantpageCacheID.ToString)
            Dim ReinsurerpageCacheID As Guid
            ReinsurerpageCacheID = Guid.NewGuid()
            ViewState.Add("ReinsurerpageCacheID", ReinsurerpageCacheID.ToString)

            If Request.QueryString("Mode") IsNot Nothing Then
                sVMode = Convert.ToString(Request.QueryString("Mode")).ToUpper.Trim
            End If
            If Request.QueryString("PMode") IsNot Nothing Then
                sPMode = Convert.ToString(Request.QueryString("PMode")).ToUpper.Trim
            End If

            Integer.TryParse(Request.QueryString("RIArrangementLineKey"), iRIArrangementLineKey)

            If iRIArrangementLineKey > 0 AndAlso Session(CNRIFACProp) Is Nothing AndAlso Session(CNRIFACXol) Is Nothing Then
                'In case of View from Claim RI Screen
                Dim oArrangementLineType As ArrangementLinesType
                If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim _
                Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or (sVMode = "VIEW" And Session(CNMode) = Mode.ViewClaim) Then
                    oArrangementLineType = GetRIArrangementLineFromXML(iRIArrangementLineKey, Session(CNRIXMLData), "", True)
                Else
                    oArrangementLineType = GetRIArrangementLineFromXML(iRIArrangementLineKey, Session(CNRIXMLData), sType, False)
                End If
                'Putting in session tyo support the coded functionality
                If sType = "FACPROP" Then
                    Session(CNRIFACProp) = oArrangementLineType
                Else
                    Session(CNRIFACXol) = oArrangementLineType
                End If
            End If

            If sVMode = "VIEW" Or sPMode = "VIEW" Then
                If sType = "FACPROP" Then
                    CallShowParticipatuions(sType, Session(CNRIFACProp), sVMode)
                Else
                    CallShowParticipatuions(sType, Session(CNRIFACXol), sVMode)
                End If

            ElseIf sVMode = "EDIT" Or sPMode = "EDIT" Then
                If sType = "FACPROP" Then
                    If Session(CNRIFACProp) IsNot Nothing Then
                        CallShowParticipatuions(sType, Session(CNRIFACProp), sVMode)
                    End If
                ElseIf sType = "FACXOL" Then
                    If Session(CNRIFACXol) IsNot Nothing Then
                        CallShowParticipatuions(sType, Session(CNRIFACXol), sVMode)
                    End If
                End If
            Else
                
                If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                    lblTitle.Text = GetLocalResourceObject("lbl_FACXOL_title")
                    If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
                        oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
                    End If
                    If oReinsurerColl Is Nothing Then
                        oReinsurer.RITypeCode = "FAC PROP"
                        oReinsurer.IsBroker = False
                        oReinsurer.IsBrokerSpecified = True
                        oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                        Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    End If

                    grdvSearchResults.Visible = True
                    grdvSearchResults.DataSource = oReinsurerColl
                    grdvSearchResults.DataBind()

                ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                    lblTitle.Text = GetLocalResourceObject("lbl_FACXOL_title")
                    pnlFACProp_Part.Visible = False

                    If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
                        oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
                    End If
                    If oReinsurerColl Is Nothing Then
                        oReinsurer.IsFAX = True
                        oReinsurer.IsFAXSpecified = True
                        oReinsurer.IsBroker = False
                        oReinsurer.IsBrokerSpecified = True
                        oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                        Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
                    End If

                    grdvSearchResults.Visible = True
                    grdvSearchResults.DataSource = oReinsurerColl
                    grdvSearchResults.DataBind()
                End If
            End If
        End If
        PopulateDropDown()
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim bMode As Boolean = True
        Dim sMode As String = String.Empty
        Dim sPMode As String = String.Empty

        If Request.QueryString("Mode") IsNot Nothing Then
            sMode = Request.QueryString("Mode")
        End If
        If Request.QueryString("PMode") IsNot Nothing Then
            sPMode = Request.QueryString("PMode")
        End If

        'Set the properties of controls according to the Mode
        If (String.IsNullOrEmpty(sMode) = False AndAlso sMode.ToUpper.Trim = "VIEW") Or (String.IsNullOrEmpty(sPMode) = False AndAlso sPMode.ToUpper.Trim = "VIEW") Then
            bMode = False
            drpType.Enabled = bMode
            txtFileCode.Enabled = bMode
            txtReinsurerCode.Enabled = bMode
            txtName.Enabled = bMode
            btnNewSearch.Enabled = bMode
            btnSearch.Enabled = bMode
            pnlFindRIBrokerPart.Enabled = bMode
            btnNewSearch.Enabled = bMode
            btnSearch.Enabled = bMode
            grdvSearchResults.Enabled = bMode
            grdParticipants.Enabled = bMode

        End If
    End Sub
#End Region

#Region "Private Method"
    ''' <summary>
    ''' This will populate the DropDown Box
    ''' </summary>
    ''' <remarks></remarks>
    Sub PopulateDropDown()
        Dim sType As String
        If Request.QueryString("Type") IsNot Nothing Then
            sType = Request.QueryString("Type")
            If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                drpType.Items.Clear()
                drpType.Items.Insert(0, New ListItem("Insurer", "Insurer"))
                drpType.DataBind()
                drpType.SelectedIndex = 0
                drpType.Enabled = False
            ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                drpType.Items.Clear()
                drpType.Items.Insert(0, New ListItem("<ALL>", "ALL"))
                drpType.Items.Insert(1, New ListItem("Reinsurer", "Reinsurer"))
                drpType.DataBind()
                drpType.SelectedIndex = 1
            End If
        End If
    End Sub
    ''' <summary>
    ''' this will search the Reinsurer 
    ''' </summary>
    ''' <param name="bSearchButton"></param>
    ''' <param name="bNewSearch"></param>
    ''' <remarks></remarks>
    Sub FindReInsurer(Optional ByVal bSearchButton As Boolean = False, Optional ByVal bNewSearch As Boolean = False)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oReinsurer As New NexusProvider.ReinsurerSearchCriteria
        Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())
        Dim sType As String
        If bSearchButton = False AndAlso bNewSearch = False AndAlso ViewState("ReinsurerpageCacheID") IsNot Nothing Then
            oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
            If oReinsurerColl Is Nothing Then

                oReinsurer.IsBroker = False
                oReinsurer.IsBrokerSpecified = True

                If Request.QueryString("Type") IsNot Nothing Then
                    sType = Request.QueryString("Type")
                    If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                        oReinsurer.IsFAX = False
                        oReinsurer.IsFAXSpecified = False
                    ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                        oReinsurer.IsFAX = True
                        oReinsurer.IsFAXSpecified = True
                    End If
                End If
                oReinsurer.MaxRowsToFetch = oPortal.MaxSearchResults

                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If
        Else
            If bSearchButton Then
                If txtReinsurerCode.Text <> "" Then
                    oReinsurer.RICode = txtReinsurerCode.Text
                End If
                If txtName.Text <> "" Then
                    oReinsurer.RIName = txtName.Text
                End If
                If txtFileCode.Text <> "" Then
                    oReinsurer.FileCode = txtFileCode.Text
                End If

                If Request.QueryString("Type") IsNot Nothing Then
                    sType = Request.QueryString("Type")
                    If sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACPROP" Then
                        oReinsurer.IsFAX = False
                        oReinsurer.IsFAXSpecified = False
                    ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then
                        oReinsurer.IsFAX = True
                        oReinsurer.IsFAXSpecified = True
                    End If
                End If
                oReinsurer.IsBroker = False
                oReinsurer.IsBrokerSpecified = True
                'to limit the search return from SAM
                oReinsurer.MaxRowsToFetch = oPortal.MaxSearchResults

                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                If ViewState("ReinsurerpageCacheID") Is Nothing Then
                    Dim ReinsurerpageCacheID As Guid
                    ReinsurerpageCacheID = Guid.NewGuid()
                    ViewState.Add("ReinsurerpageCacheID", ReinsurerpageCacheID.ToString)
                End If
                Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))
            End If
            If bNewSearch Then
                oReinsurer.MaxRowsToFetch = 0
                txtReinsurerCode.Text = ""
                txtName.Text = ""
                txtFileCode.Text = ""
                btnOk.Enabled = False
                oReinsurerColl = Nothing
                ViewState.Remove("ReinsurerpageCacheID")
            Else
                btnOk.Enabled = True
            End If
        End If

        grdvSearchResults.Visible = True
        grdvSearchResults.DataSource = oReinsurerColl
        grdvSearchResults.DataBind()
    End Sub
    ''' <summary>
    ''' this will show the Broker participants according to the Type e.g. F or FX
    ''' </summary>
    ''' <param name="sType"></param>
    ''' <param name="oArrangementLines"></param>
    ''' <param name="sVMode"></param>
    ''' <remarks></remarks>
    Sub CallShowParticipatuions(ByVal sType As String, Optional ByRef oArrangementLines As ArrangementLinesType = Nothing, Optional ByVal sVMode As String = "")
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
        Dim oReinsurer As New NexusProvider.ReinsurerSearchCriteria
        Dim ReinsurerpageCacheID As Guid
        ReinsurerpageCacheID = Guid.NewGuid()
        ViewState.Add("ReinsurerpageCacheID", ReinsurerpageCacheID.ToString)
        Dim sPMode As String = String.Empty
        If Request.QueryString("PMode") IsNot Nothing Then
            sPMode = Request.QueryString("PMode").ToUpper.ToString
        End If

        If sType = "FACPROP" Then
            If oArrangementLines Is Nothing Then
                oReinsurer.RITypeCode = "FAC PROP"
                oReinsurer.IsBroker = False
                oReinsurer.IsBrokerSpecified = True
                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                pnlFindRIBrokerPart.Visible = True
                pnlFindRIBrokerPart.Enabled = False
                pnlFACProp_Part.Visible = True
                pnlFACProp_Part.Enabled = True
                grdvSearchResults.Visible = True
                grdvSearchResults.DataSource = oReinsurerColl
                grdvSearchResults.DataBind()
               
            ElseIf oArrangementLines IsNot Nothing AndAlso (sVMode = "EDIT" Or sVMode = "VIEW" Or sPMode = "EDIT" Or sPMode = "VIEW") Then
                Dim oBrokerParticipant As BrokerParticipants
                Dim dTotal As Double
                oReinsurer.RITypeCode = "FAC PROP"
                oReinsurer.IsBroker = False
                oReinsurer.IsBrokerSpecified = True
                oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
                Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

                grdvSearchResults.Visible = True
                grdvSearchResults.DataSource = oReinsurerColl
                grdvSearchResults.DataBind()

                If oArrangementLines.BrokerParticipants IsNot Nothing Then
                    For Each oBrokerParticipant In oArrangementLines.BrokerParticipants
                        dTotal += oBrokerParticipant.ParticipationPercentage
                    Next
                    For Each oBrokerParticipant In oArrangementLines.BrokerParticipants
                        oBrokerParticipant.TotalParticipationPercentage = dTotal
                    Next

                    grdParticipants.Visible = True
                    grdParticipants.DataSource = oArrangementLines.BrokerParticipants
                    grdParticipants.DataBind()
                    Dim txtTotal As TextBox = grdParticipants.FooterRow.FindControl("txtTotalParticipation")
                    txtTotal.Text = dTotal
                    Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oArrangementLines.BrokerParticipants, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
                End If
                
                pnlFindRIBrokerPart.Visible = True
                pnlFindRIBrokerPart.Enabled = True
                pnlFACProp_Part.Visible = True
                pnlFACProp_Part.Enabled = True
               
                If sVMode = "VIEW" Or sPMode = "VIEW" Then
                    pnlFindRIBrokerPart.Enabled = False
                    pnlFACProp_Part.Enabled = False
                    grdvSearchResults.EditIndex = False
                End If
            End If

            btnCancel.Enabled = True

        ElseIf sType = "FACXOL" Then

            oReinsurer.IsBroker = False
            oReinsurer.IsBrokerSpecified = True
            oReinsurer.IsFAX = True
            oReinsurer.IsFAXSpecified = True
            oReinsurerColl = oWebService.FindReinsurer(oReinsurer)
            Cache.Insert(ViewState("ReinsurerpageCacheID"), oReinsurerColl, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(5))

            If oArrangementLines IsNot Nothing AndAlso (sVMode = "EDIT" Or sVMode = "VIEW" Or sPMode = "EDIT" Or sPMode = "VIEW") Then
                Dim sPartyCode As String = String.Empty
                Dim dFAXTotal As Double
                If Request.QueryString("Code") IsNot Nothing Then
                    sPartyCode = Request.QueryString("Code")
                End If
                If oArrangementLines.FAXParticipants IsNot Nothing Then
                    If String.IsNullOrEmpty(sPartyCode) = False Then
                        For iCount As Integer = 0 To oArrangementLines.FAXParticipants.Count - 1
                            If oArrangementLines.FAXParticipants(iCount).PartyCode.ToUpper.Trim = sPartyCode.ToUpper.Trim Then
                                For Each oFAXBrokerParticipant As BrokerParticipants In oArrangementLines.FAXParticipants(iCount).BrokerParticipants
                                    dFAXTotal += oFAXBrokerParticipant.ParticipationPercentage
                                Next
                                For Each oFAXBrokerParticipant As BrokerParticipants In oArrangementLines.FAXParticipants(iCount).BrokerParticipants
                                    oFAXBrokerParticipant.TotalParticipationPercentage = dFAXTotal
                                Next

                                grdParticipants.Visible = True
                                grdParticipants.DataSource = oArrangementLines.FAXParticipants(iCount).BrokerParticipants
                                grdParticipants.DataBind()
                                Dim txtTotal As TextBox = grdParticipants.FooterRow.FindControl("txtTotalParticipation")
                                txtTotal.Text = dFAXTotal

                                Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oArrangementLines.FAXParticipants(iCount).BrokerParticipants, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
                                Exit For
                            End If
                        Next
                    End If
                End If
            End If

            pnlFACProp_Part.Visible = True
            grdvSearchResults.Visible = True
            grdvSearchResults.DataSource = oReinsurerColl
            grdvSearchResults.DataBind()

            If sVMode = "VIEW" Or sPMode = "VIEW" Then
                pnlFindRIBrokerPart.Enabled = False
                pnlFACProp_Part.Enabled = False
                grdvSearchResults.Enabled = False
                grdParticipants.Enabled = False
                btnNewSearch.Enabled = False
                btnSearch.Enabled = False
            End If
        End If

    End Sub
    ''' <summary>
    ''' this will check whether the participnants are present or not
    ''' </summary>
    ''' <param name="sRICode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function CheckCollectionOfPartcipants(ByVal sRICode As String) As Boolean

        Dim bPresent As Boolean = False
        Dim iRowCount As Integer = grdParticipants.Rows.Count - 1
        For iCount As Integer = 0 To iRowCount
            If sRICode.ToUpper.Trim = grdParticipants.Rows(iCount).Cells(0).Text.ToUpper.Trim Then
                bPresent = True
                Exit For
            End If
        Next
        Return bPresent
    End Function
    ''' <summary>
    ''' this will generate a unique line id which will be added into xml
    ''' </summary>
    ''' <param name="iNewId"></param>
    ''' <param name="iRIArrangementId"></param>
    ''' <remarks></remarks>
    Sub GenerateUniqueRILineId(ByRef iNewId As Integer, Optional ByRef iRIArrangementId As Integer = 0)
        Dim oXMLDoc As New XmlDocument
        Dim sXML As String = Session(CNRIXMLData)
        Dim sRIBandID As String = "Current_" & Session(CNRIBandKey)
        oXMLDoc.LoadXml(sXML)
        iNewId = 0
        'get the max FAC line id
        Dim oFXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='F']")
        For Each oFXMLNode As XmlNode In oFXMLNodes
            If Convert.ToInt32(oFXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oFXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        'get the MAX FAX line id
        Dim oFXXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='FX']")
        For Each oFXXMLNode As XmlNode In oFXXMLNodes
            If Convert.ToInt32(oFXXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oFXXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oTXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='T']")
        For Each oTXMLNode As XmlNode In oTXMLNodes
            If Convert.ToInt32(oTXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oTXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oTXXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='TX']")
        For Each oTXXMLNode As XmlNode In oTXXMLNodes
            If Convert.ToInt32(oTXXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oTXXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oTCXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='TC']")
        For Each oTCXMLNode As XmlNode In oTCXMLNodes
            If Convert.ToInt32(oTCXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oTCXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oRXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='R']")
        For Each oRXMLNode As XmlNode In oRXMLNodes
            If Convert.ToInt32(oRXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oRXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        Dim oTFSXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Type='TFS']")
        For Each oTFSXMLNode As XmlNode In oTFSXMLNodes
            If Convert.ToInt32(oTFSXMLNode.Attributes("RIArrangementLineKey").Value.Trim) > iNewId.ToString.Trim Then
                iNewId = Convert.ToInt32(oTFSXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
            End If
        Next
        iNewId += 1
        'get the arrangement Key
        Dim oALLXMLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow")
        For Each oALLXMLNode As XmlNode In oALLXMLNodes
            If oALLXMLNode.Attributes("RIarrangementKey").Value.Trim <> "" Then
                If Convert.ToInt32(oALLXMLNode.Attributes("RIarrangementKey").Value.Trim) <> 0 Then
                    iRIArrangementId = Convert.ToInt32(oALLXMLNode.Attributes("RIarrangementKey").Value.Trim)
                    Exit For
                End If
            End If
        Next

    End Sub
    ''' <summary>
    ''' this will get into work when we edit the line. 
    ''' </summary>
    ''' <param name="iRIArrangementId"></param>
    ''' <param name="sRIName"></param>
    ''' <remarks></remarks>
    Sub GetRILineId(ByRef iRIArrangementId As Integer, ByVal sRIName As String)
        Dim oXMLDoc As New XmlDocument
        Dim sXML As String = Session(CNRIXMLData)
        Dim sRIBandID As String = "Current_" & Session(CNRIBandKey)
        oXMLDoc.LoadXml(sXML.Replace("'", "%apos%"))
        iRIArrangementId = 0
        Dim oFXMLNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBandID & "']/ArrangementRow[@Name='" & sRIName.Replace("'", "%apos%") & "']")
        If oFXMLNode IsNot Nothing Then
            iRIArrangementId = Convert.ToInt32(oFXMLNode.Attributes("RIArrangementLineKey").Value.Trim)
        End If
    End Sub
   
    Sub SetGridVisibilitiesFalse()
        If grdParticipants.Visible = True Then
            grdParticipants.Visible = False
        End If
    End Sub
    ''' <summary>
    ''' this will be used to recalculate the participants amount
    ''' </summary>
    ''' <param name="value"></param>
    ''' <remarks></remarks>
    Sub processtextchange(Optional ByVal value As String = "0")
        Dim iRowCount As Integer = grdParticipants.Rows.Count
        Dim dTotal As Double
        If ViewState("RIBrokerParticipantpageCacheID") IsNot Nothing Then
            oBrokerParticipationCol = CType(Cache.Item(ViewState("RIBrokerParticipantpageCacheID")), BrokerParticipantsCollection)
        End If
        If iRowCount > 0 Then
            For iCount As Integer = 0 To iRowCount - 1
                Dim txtpart As TextBox = grdParticipants.Rows(iCount).FindControl("txtParticipation")
                dTotal += Convert.ToDouble(txtpart.Text)
                If oBrokerParticipationCol IsNot Nothing AndAlso oBrokerParticipationCol.Count = iRowCount Then
                    Double.TryParse(txtpart.Text, oBrokerParticipationCol(iCount).ParticipationPercentage)
                End If
            Next
            Dim txtTotalParticipation As TextBox = grdParticipants.FooterRow.FindControl("txtTotalParticipation")
            txtTotalParticipation.Text = dTotal
            Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oBrokerParticipationCol, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
        End If
    End Sub
    ''' <summary>
    ''' this will work on Edit and view mode in showing back the participants details
    ''' </summary>
    ''' <param name="v_iRIArrangementLineKey"></param>
    ''' <param name="v_sXML"></param>
    ''' <param name="sType"></param>
    ''' <param name="v_bIsClaim"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetRIArrangementLineFromXML(ByVal v_iRIArrangementLineKey As Integer, ByVal v_sXML As String, _
   Optional ByVal sType As String = "", Optional ByVal v_bIsClaim As Boolean = False) As ArrangementLinesType
        'Variable to Hold the returned data
        Dim oArrangementLineType As New ArrangementLinesType
        'Load the XML Data 
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(v_sXML)

        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']")

        If v_bIsClaim Then
            'For Claim
            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details
                With oArrangementLineType

                    .ActionType = oNode.Attributes("ActionType").Value
                    .AgreementCode = oNode.Attributes("AgreementCode").Value
                    Boolean.TryParse(oNode.Attributes("CedePremiumOnly").Value, .CedePremiumOnly)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Integer.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIArrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, .RecoverToDate)
                    Decimal.TryParse(oNode.Attributes("ThisPayment").Value, .ThisPayment)
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, .ReserveToDate)
                    Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, .PaymentToDate)
                    Decimal.TryParse(oNode.Attributes("ThisReserve").Value, .ThisReserve)
                    Decimal.TryParse(oNode.Attributes("Balance").Value, .Balance)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("Incurred").Value, .Incurred)

                    If .Type = "F" Then
                        'Populate Broker Details
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    End If
                    If .Type = "FX" Then
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart

                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PaymentToDate").Value, .PaymentToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("RecoverToDate").Value, .RecoverToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ReserveToDate").Value, .ReserveToDate)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ThisPayment").Value, .ThisPayment)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ThisReserve").Value, .ThisReserve)
                                    Decimal.TryParse(oFAXPartNode.Attributes("Premium").Value, .PremiumValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumTax").Value, .PremiumTax)
                                    .ActionType = oFAXPartNode.Attributes("ActionType").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow[@PartyCode='" & .PartyCode & "']/FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants

                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If
        Else

            If oNode IsNot Nothing Then
                'Populate Arrangement Line Details
                With oArrangementLineType

                    .ActionType = oNode.Attributes("ActionType").Value
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    .CedePremiumOnly = oNode.Attributes("CedePremiumOnly").Value
                    Decimal.TryParse(oNode.Attributes("CommissionPerc").Value, .CommissionPerc)
                    Integer.TryParse(oNode.Attributes("Grouping").Value, .Grouping)
                    Boolean.TryParse(oNode.Attributes("IsCommissionModified").Value, .IsCommissionModified)
                    Boolean.TryParse(oNode.Attributes("IsDomiciledForTax").Value, .IsDomiciledForTax)
                    Boolean.TryParse(oNode.Attributes("IsRIBroker").Value, .IsRIBroker)
                    Decimal.TryParse(oNode.Attributes("LineLimit").Value, .LineLimit)
                    Decimal.TryParse(oNode.Attributes("LowerLimit").Value, .LowerLimit)
                    Integer.TryParse(oNode.Attributes("NumberOfLines").Value, .NumberOfLines)
                    Integer.TryParse(oNode.Attributes("PartyKey").Value, .PartyKey)
                    Decimal.TryParse(oNode.Attributes("PremiumPercent").Value, .PremiumPercent)
                    Integer.TryParse(oNode.Attributes("Priority").Value, .Priority)
                    .ReinsuranceTypeCode = oNode.Attributes("ReinsuranceTypeCode").Value
                    Decimal.TryParse(oNode.Attributes("Retained").Value, .Retained)
                    Integer.TryParse(oNode.Attributes("RIarrangementKey").Value, .RIarrangementKey)
                    Integer.TryParse(oNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)
                    .RIPlacement = oNode.Attributes("Placement").Value
                    .TreatyCode = oNode.Attributes("TreatyCode").Value
                    .Type = oNode.Attributes("Type").Value
                    .RIName = oNode.Attributes("Name").Value
                    Decimal.TryParse(oNode.Attributes("DefaultPerc").Value, .DefaultPerc)
                    Decimal.TryParse(oNode.Attributes("ThisPerc").Value, .ThisPerc)
                    Decimal.TryParse(oNode.Attributes("SumInsured").Value, .SumInsured)
                    .AgreementCode = oNode.Attributes("Agreement").Value
                    Decimal.TryParse(oNode.Attributes("ParticipationPercent").Value, .ParticipationPercent)
                    Decimal.TryParse(oNode.Attributes("Premium").Value, .PremiumValue)
                    Decimal.TryParse(oNode.Attributes("Tax").Value, .Tax)
                    Decimal.TryParse(oNode.Attributes("Commission").Value, .CommissionValue)
                    Decimal.TryParse(oNode.Attributes("CommissionTax").Value, .CommissionTax)

                    'Populate Broker Details
                    If sType.Trim <> "FACXOL" Then
                        Dim oBrokerPart As BrokerParticipants
                        Dim oBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/BrokerParticipentRow")
                        If oBrokerNodeList IsNot Nothing AndAlso oBrokerNodeList.Count > 0 Then
                            For Each oBrokerPartNode As XmlNode In oBrokerNodeList
                                oBrokerPart = New BrokerParticipants
                                With oBrokerPart
                                    .PartyCode = oBrokerPartNode.Attributes("PartyCode").Value
                                    .PartyName = oBrokerPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                End With
                                oArrangementLineType.BrokerParticipants.Add(oBrokerPart)
                            Next
                        End If
                    Else
                        'Populate FAX Participents
                        Dim oFAXPart As FAXParticipants
                        Dim oFAXNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow")
                        If oFAXNodeList IsNot Nothing AndAlso oFAXNodeList.Count > 0 Then
                            For Each oFAXPartNode As XmlNode In oFAXNodeList
                                oFAXPart = New FAXParticipants
                                With oFAXPart

                                    .PartyCode = oFAXPartNode.Attributes("PartyCode").Value
                                    .PartyName = oFAXPartNode.Attributes("PartyName").Value
                                    Integer.TryParse(oFAXPartNode.Attributes("PartyKey").Value, .PartyKey)
                                    Decimal.TryParse(oFAXPartNode.Attributes("ParticipationPercentage").Value, .ParticipationPercentage)
                                    .AccountType = oFAXPartNode.Attributes("AccountType").Value
                                    .AgreementCode = oFAXPartNode.Attributes("AgreementCode").Value
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionPercent").Value, .CommissionPercent)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionTax").Value, .CommissionTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("CommissionValue").Value, .CommissionValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumTax").Value, .PremiumTax)
                                    Decimal.TryParse(oFAXPartNode.Attributes("PremiumValue").Value, .PremiumValue)
                                    Decimal.TryParse(oFAXPartNode.Attributes("SumInsured").Value, .SumInsured)
                                    Integer.TryParse(oFAXPartNode.Attributes("RIArrangementLineKey").Value, .RIArrangementLineKey)

                                    'Populate FAX Broker Part
                                    Dim oFAXBrokerPart As BrokerParticipants
                                    Dim oFAXBrokerNodeList As XmlNodeList = oNode.SelectNodes("//*[@RIArrangementLineKey='" & v_iRIArrangementLineKey & "']/FAXParticipentRow[@PartyCode='" & .PartyCode & "']/FAXBrokerParticipentRow")
                                    If oFAXBrokerNodeList IsNot Nothing AndAlso oFAXBrokerNodeList.Count > 0 Then
                                        For Each oFAXBrokerPartNode As XmlNode In oFAXBrokerNodeList
                                            oFAXBrokerPart = New BrokerParticipants

                                            With oFAXBrokerPart
                                                .PartyCode = oFAXBrokerPartNode.Attributes("PartyCode").Value
                                                .PartyName = oFAXBrokerPartNode.Attributes("PartyName").Value
                                                Integer.TryParse(oFAXBrokerPartNode.Attributes("PartyKey").Value, .PartyKey)
                                                Decimal.TryParse(oFAXBrokerPartNode.Attributes("ParticipationPercent").Value, .ParticipationPercentage)
                                            End With
                                            oFAXPart.BrokerParticipants.Add(oFAXBrokerPart)
                                        Next
                                    End If
                                End With
                                oArrangementLineType.FAXParticipants.Add(oFAXPart)
                            Next
                        End If
                    End If
                End With
            End If
        End If
        'Return the Data
        Return oArrangementLineType
    End Function
    ''' <summary>
    ''' this will caculate the total participation amount.
    ''' </summary>
    ''' <param name="dTotalPart"></param>
    ''' <remarks></remarks>
    Sub CalculateTotalPart(Optional ByRef dTotalPart As Double = 0)
        Dim iTotalRow, iCount As Integer
        Dim dTotalParticipation As Double
        Dim txtParticipant As TextBox
        iTotalRow = grdParticipants.Rows.Count - 1
        For iCount = 0 To iTotalRow
            txtParticipant = grdParticipants.Rows(iCount).FindControl("txtParticipation")
            Double.TryParse(txtParticipant.Text, dTotalParticipation)
            dTotalPart += dTotalParticipation
        Next
    End Sub

    Protected Sub txtParticipation_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        processtextchange()
    End Sub
#End Region

#Region "Button Event"
    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        FindReInsurer(True)
    End Sub
    ''' <summary>
    ''' this will process the the final add and update function
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Dim sType As String = Request.QueryString("Type")
        Dim sVMode As String = String.Empty
        Dim sRIArrLineKey As String = "0"
        Dim sPMode As String = String.Empty
        Dim sCode As String = String.Empty

        If Request.QueryString("PMode") IsNot Nothing Then
            sPMode = Request.QueryString("PMode").ToUpper.ToString
        End If

        If Request.QueryString("RIArrangementLineKey") IsNot Nothing Then
            sRIArrLineKey = Request.QueryString("RIArrangementLineKey").Trim
        End If

        If Request.QueryString("Code") IsNot Nothing Then
            sCode = Request.QueryString("Code")
        End If

        'In case of View Only
        If Request.QueryString("Mode") IsNot Nothing Then
            sVMode = Request.QueryString("Mode").ToUpper.Trim
            If sVMode = "VIEW" Then
                'For FAC PROP
                If sType.ToUpper = "FACPROP" Then
                    If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim _
                        Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or (sVMode = "VIEW" And Session(CNMode) = Mode.ViewClaim) Then
                        Response.Redirect("~/Claims/ClaimReinsurance.aspx")
                    Else
                        Response.Redirect("~/Secure/ReInsuranceDetails.aspx")
                    End If
                Else
                    'FOR FAC XOL
                    Response.Redirect("~\Modal\FACPlacement.aspx?RIArrangementLineKey=" & sRIArrLineKey & "&Type=FACXOL&Mode=VIEW&PMode=VIEW")
                End If
            End If
        End If

        'If Mode is Edit or New
        Dim oReinsurance2007 As New Reinsurance
        Dim oArranagementLineType As New ArrangementLinesType
        Dim oBrokerParticipantCol As New BrokerParticipantsCollection
        Dim dTotalPart As Double = 0

        'calculate total participation
        CalculateTotalPart(dTotalPart)

        If dTotalPart <> 100 Then
            Dim sMessage As String
            sMessage = GetLocalResourceObject("RIInVldPartPerc_Err")
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg", _
                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
            processtextchange()
            Exit Sub
        End If

        If sType.ToUpper = "FACPROP" Then

            oArranagementLineType = Session(CNRIFACProp)
            'update the broker participants
            If ViewState("RIBrokerParticipantpageCacheID") IsNot Nothing Then
                oBrokerParticipationCol = CType(Cache.Item(ViewState("RIBrokerParticipantpageCacheID")), BrokerParticipantsCollection)
            End If

            'Remove the Existing Broker and update/add the broker with latest values
            If oArranagementLineType.BrokerParticipants IsNot Nothing Then
                For iCount As Integer = 0 To oArranagementLineType.BrokerParticipants.Count - 1
                    oArranagementLineType.BrokerParticipants.RemoveAt(0)
                Next
            End If

            If oBrokerParticipationCol IsNot Nothing Then
                If oArranagementLineType IsNot Nothing AndAlso oArranagementLineType.BrokerParticipants.Count = 0 Then
                    For iCount As Integer = 0 To oBrokerParticipationCol.Count - 1
                        oArranagementLineType.BrokerParticipants.Add(oBrokerParticipationCol(iCount))
                    Next
                End If
            End If

            'updated in session
            Session(CNRIFACProp) = oArranagementLineType

            Dim iNewId As Integer = 0
            Dim iRILineId As Integer = 0

            If sVMode = "EDIT" Then
                GetRILineId(iRILineId, oArranagementLineType.RIName)
                oReinsurance2007.EditRIArrangementLines(Session(CNRIXMLData), Session(CNRIFACProp), Session(CNRIBandKey), iRILineId.ToString.Trim, Session(CNRIArrangementkey), "F", Session(CNRITransactionType))
            Else
                GenerateUniqueRILineId(iNewId)
                If Session(CNMode) = Mode.PortFolioTransferAmendment Or Session(CNMode) = Mode.ClonedTransferAmendment Then
                    oReinsurance2007.AddRIArrangementLines(Session(CNRIXMLData), Session(CNRIFACProp), Session(CNRIBandKey), iNewId.ToString.Trim, Session(CNRIArrangementkey), "F", Session(CNRITransactionType), bIsPortfolioRIAmendment:=True)
                Else
                    oReinsurance2007.AddRIArrangementLines(Session(CNRIXMLData), Session(CNRIFACProp), Session(CNRIBandKey), iNewId.ToString.Trim, Session(CNRIArrangementkey), "F", Session(CNRITransactionType))
                End If
            End If
            Session.Remove(CNRIFACProp)
            'Pass RICode=Multiple Acts as flag to identify on Reinsurance screen.
            Response.Redirect("~/Secure/ReInsuranceDetails.aspx")

        ElseIf sType.ToUpper = "FACXOL" Then
            Dim oArrangementLine As ArrangementLinesType = Session(CNRIFACXol)
            Dim oFaxParticipent As FAXParticipants = Session(CNRIFACXolTemp)

            'Value is transfered from previous screen
            If oFaxParticipent IsNot Nothing Then
                oArrangementLine.FAXParticipants.Add(oFaxParticipent)
            End If
            'update the broker participants
            If ViewState("RIBrokerParticipantpageCacheID") IsNot Nothing Then
                oBrokerParticipationCol = CType(Cache.Item(ViewState("RIBrokerParticipantpageCacheID")), BrokerParticipantsCollection)
            End If


            If oArrangementLine IsNot Nothing AndAlso oArrangementLine.FAXParticipants IsNot Nothing Then
                For iFAXCount As Integer = 0 To oArrangementLine.FAXParticipants.Count - 1
                    If String.IsNullOrEmpty(sCode) = False Then
                        If oArrangementLine.FAXParticipants(iFAXCount).PartyCode.ToUpper.Trim = sCode.ToUpper.Trim Then
                            'Remove the Existing Broker and update/add the broker with latest values
                            If oArrangementLine.FAXParticipants(iFAXCount).BrokerParticipants IsNot Nothing Then
                                For iCount As Integer = 0 To oArrangementLine.FAXParticipants(iFAXCount).BrokerParticipants.Count - 1
                                    oArrangementLine.FAXParticipants(iFAXCount).BrokerParticipants.RemoveAt(0)
                                Next
                            End If
                            'update/add the broker with latest values
                            If oArrangementLine.FAXParticipants(iFAXCount).BrokerParticipants IsNot Nothing Then
                                If oBrokerParticipationCol IsNot Nothing Then
                                    For iCount As Integer = 0 To oBrokerParticipationCol.Count - 1
                                        oArrangementLine.FAXParticipants(iFAXCount).BrokerParticipants.Add(oBrokerParticipationCol(iCount))
                                    Next
                                End If
                            End If
                            Exit For
                        End If
                    End If
                Next
            End If

            'Holding the Value Temporarily in session
            Session(CNRIFACXol) = oArrangementLine
            Session.Remove(CNRIFACXolTemp)

            If sVMode = "EDIT" Then

                Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & sType.Trim & "&CMode=EDITED&Mode=" & sVMode.Trim & "&RIArrangementLineKey=" & sRIArrLineKey.Trim & "&PMode=" & sPMode)

            Else

                If Request.QueryString("PMode") IsNot Nothing AndAlso Request.QueryString("Mode") IsNot Nothing Then
                    Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & sType.Trim & "&CMode=EDITED&Mode=" & sVMode.Trim & "&PMode=" & sPMode)
                ElseIf Request.QueryString("PMode") IsNot Nothing AndAlso Request.QueryString("Mode") Is Nothing Then
                    Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & sType.Trim & "&CMode=EDITED&PMode=" & sPMode)
                ElseIf Request.QueryString("PMode") Is Nothing AndAlso Request.QueryString("Mode") Is Nothing Then
                    Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & sType.Trim & "&CMode=New")
                End If
            End If
        End If

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim sVMode, sType As String

        'Mode holds the current mode i.e. Edit or View
        If Request.QueryString("Mode") IsNot Nothing Then
            sVMode = Convert.ToString(Request.QueryString("Mode")).ToUpper.Trim
        End If

        'Type holds the Type of Line i.e. FAX XOL or FAC PROP
        If Request.QueryString("Type") IsNot Nothing Then
            sType = Convert.ToString(Request.QueryString("Type")).ToUpper.Trim
        End If

        'if Mode is Edit
        If sVMode = "EDIT" Then
            'And Type is FAC XOL
            If sType.ToUpper = "FACXOL" Then

                Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & sType & "&Mode=" & sVMode)
            End If

            'FAC PROP
            'Response.Redirect("~/Secure/RiskDetails.aspx")
            Response.Redirect("~/secure/ReInsuranceDetails.aspx")
        ElseIf sVMode = "VIEW" Then
            'For Use of Claim only
            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim _
                        Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Or (sVMode = "VIEW" And Session(CNMode) = Mode.ViewClaim) Then
                Response.Redirect("~/Claims/ClaimReinsurance.aspx")
            Else
                'Rest of the cases
                Response.Redirect("~/Secure/RiskDetails.aspx")
            End If
        Else
            If String.IsNullOrEmpty(sVMode) = False Then
                Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & Request.QueryString("Type") & "&Mode=" & sVMode)
            Else
                Response.Redirect("~/Modal/FACPlacement.aspx?Type=" & Request.QueryString("Type"))
            End If
        End If
    End Sub

    Protected Sub btnNewSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNewSearch.Click
        If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
            Cache.Remove(ViewState("ReinsurerpageCacheID"))
        End If
        FindReInsurer(False, True)
    End Sub
#End Region

#Region "Grid Event"

    ''' <summary>
    ''' Bind grid for new page index
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub grdvSearchResults_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grdvSearchResults.PageIndexChanging

        grdvSearchResults.PageIndex = e.NewPageIndex
        If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
            oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
        End If
        grdvSearchResults.DataSource = oReinsurerColl
        grdvSearchResults.DataBind()

    End Sub
    Protected Sub grdvSearchResults_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdvSearchResults.RowCommand
        Dim oBrokerParticipants As BrokerParticipants
        Dim sType As String
        If Request.QueryString("Type") IsNot Nothing Then
            sType = Request.QueryString("Type")
        End If
        If e.CommandName IsNot Nothing Then
            Select Case e.CommandName
                Case "Select"

                    If String.IsNullOrEmpty(sType) = False AndAlso sType.Trim.ToUpper = "FACPROP" Then

                        If CheckCollectionOfPartcipants(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text) = True Then
                            'javascript message
                            Dim sMessage As String
                            sMessage = GetLocalResourceObject("RIBrokerExist_Msg")
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg", _
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                            Exit Sub
                        Else

                            If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
                                oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
                            End If
                            If ViewState("RIBrokerParticipantpageCacheID") IsNot Nothing Then
                                oBrokerParticipationCol = CType(Cache.Item(ViewState("RIBrokerParticipantpageCacheID")), BrokerParticipantsCollection)
                            End If

                            For iCount As Integer = 0 To oReinsurerColl.Count - 1
                                If Server.HtmlEncode(oReinsurerColl.Item(iCount).ReinsurerCode.Trim).ToUpper = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToUpper) Then
                                    oBrokerParticipants = New BrokerParticipants
                                    oBrokerParticipants.PartyCode = oReinsurerColl.Item(iCount).ReinsurerCode
                                    oBrokerParticipants.PartyKey = oReinsurerColl.Item(iCount).ReinsurerKey
                                    oBrokerParticipants.PartyName = oReinsurerColl.Item(iCount).RIName
                                    oBrokerParticipants.ParticipationPercentage = oReinsurerColl.Item(iCount).ParticipationPercentage

                                    If oBrokerParticipationCol Is Nothing Then
                                        oBrokerParticipationCol = New BrokerParticipantsCollection
                                        oBrokerParticipationCol.Add(oBrokerParticipants)
                                    Else
                                        If oBrokerParticipationCol.Count > 0 Then
                                            oBrokerParticipants.TotalParticipationPercentage = oBrokerParticipationCol(0).TotalParticipationPercentage
                                        End If
                                        oBrokerParticipationCol.Add(oBrokerParticipants)
                                    End If
                                    Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oBrokerParticipationCol, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
                                    Exit For
                                End If
                            Next

                            grdParticipants.Visible = True
                            grdParticipants.DataSource = oBrokerParticipationCol
                            grdParticipants.DataBind()
                            If grdParticipants.Rows.Count <> 0 Then
                                processtextchange()
                            End If
                        End If
                    ElseIf sType IsNot Nothing AndAlso sType.Trim.ToUpper = "FACXOL" Then

                        If CheckCollectionOfPartcipants(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text) = True Then
                            'javascript message
                            Dim sMessage As String
                            sMessage = GetLocalResourceObject("RIBrokerExist_Msg")
                            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "ShowMsg", _
                                                               "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){ShowMsg('" & sMessage & "');});</script>")
                            Exit Sub
                        Else

                            If ViewState("ReinsurerpageCacheID") IsNot Nothing Then
                                oReinsurerColl = CType(Cache.Item(ViewState("ReinsurerpageCacheID")), NexusProvider.ReinsurerCollection)
                            End If
                            If ViewState("RIBrokerParticipantpageCacheID") IsNot Nothing Then
                                oBrokerParticipationCol = CType(Cache.Item(ViewState("RIBrokerParticipantpageCacheID")), BrokerParticipantsCollection)
                            End If
                            For iCount As Integer = 0 To oReinsurerColl.Count - 1
                                If Server.HtmlEncode(oReinsurerColl.Item(iCount).ReinsurerCode.Trim).ToUpper = Trim(grdvSearchResults.Rows(CInt(e.CommandArgument)).Cells(0).Text.ToUpper) Then
                                    oBrokerParticipants = New BrokerParticipants
                                    oBrokerParticipants.PartyCode = oReinsurerColl.Item(iCount).ReinsurerCode
                                    oBrokerParticipants.PartyKey = oReinsurerColl.Item(iCount).ReinsurerKey
                                    oBrokerParticipants.PartyName = oReinsurerColl.Item(iCount).RIName
                                    oBrokerParticipants.ParticipationPercentage = oReinsurerColl.Item(iCount).ParticipationPercentage

                                    If oBrokerParticipationCol Is Nothing Then
                                        oBrokerParticipationCol = New BrokerParticipantsCollection
                                        oBrokerParticipationCol.Add(oBrokerParticipants)
                                    Else
                                        If oBrokerParticipationCol.Count > 0 Then
                                            oBrokerParticipants.TotalParticipationPercentage = oBrokerParticipationCol(0).TotalParticipationPercentage
                                        End If
                                        oBrokerParticipationCol.Add(oBrokerParticipants)
                                    End If
                                    Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oBrokerParticipationCol, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
                                    Exit For
                                End If
                            Next

                            Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oBrokerParticipationCol, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
                            grdParticipants.Visible = True
                            pnlFACProp_Part.Visible = True
                            grdParticipants.DataSource = oBrokerParticipationCol
                            grdParticipants.DataBind()
                            If grdParticipants.Rows.Count <> 0 Then
                                processtextchange()
                            End If
                        End If
                    End If
            End Select
        End If
    End Sub

    Protected Sub grdvSearchResults_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdvSearchResults.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim oReInsurer As NexusProvider.Reinsurer = CType(e.Row.DataItem, NexusProvider.Reinsurer)
            Dim lnkSelect As LinkButton = e.Row.FindControl("btnSelect")
            lnkSelect.CommandArgument = e.Row.RowIndex
            e.Row.Attributes.Add("Code", CType(e.Row.DataItem, NexusProvider.Reinsurer).ReinsurerCode)
        End If
        btnCancel.Enabled = True
    End Sub

    Protected Sub grdParticipants_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdParticipants.DataBound
        grdParticipants.Columns(3).Visible = False
    End Sub

    Protected Sub grdParticipants_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdParticipants.RowCommand
        If e.CommandName IsNot Nothing Then
            Select Case e.CommandName
                Case "Remove"
                    If ViewState("RIBrokerParticipantpageCacheID") IsNot Nothing Then
                        oBrokerParticipationCol = CType(Cache.Item(ViewState("RIBrokerParticipantpageCacheID")), BrokerParticipantsCollection)
                    End If
                    If oBrokerParticipationCol IsNot Nothing Then
                        Dim sPartyCode As String = grdParticipants.Rows(Convert.ToInt32(e.CommandArgument)).Cells(0).Text
                        If String.IsNullOrEmpty(sPartyCode) = False Then
                            For iCount As Integer = 0 To oBrokerParticipationCol.Count - 1
                                If Server.HtmlEncode(oBrokerParticipationCol(iCount).PartyCode.Trim).ToUpper = sPartyCode.ToUpper.Trim Then
                                    oBrokerParticipationCol.RemoveAt(iCount)
                                    Exit For
                                End If
                            Next
                        End If
                    End If

                    Cache.Insert(ViewState("RIBrokerParticipantpageCacheID"), oBrokerParticipationCol, Nothing, DateTime.MaxValue, TimeSpan.FromMinutes(10))
                    grdParticipants.Visible = True
                    pnlFACProp_Part.Visible = True
                    grdParticipants.DataSource = oBrokerParticipationCol
                    grdParticipants.DataBind()
                    If grdParticipants.Rows.Count = 0 Then
                        grdParticipants.Visible = False
                    Else
                        processtextchange()
                    End If
            End Select
        End If
    End Sub

    Protected Sub grdParticipants_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdParticipants.RowCreated

        If e.Row.RowType = DataControlRowType.Footer Then
            txtTotalParticipation = e.Row.FindControl("txtTotalParticipation")
            Dim s1 As String = txtTotalParticipation.ClientID
            txtTotalParticipation.Attributes.Add("gridTextBoxType", "totalsTextBox")
        End If

    End Sub

    Protected Sub grdParticipants_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdParticipants.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            grdParticipants.Columns(3).Visible = True
        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim txtParticipation As TextBox = e.Row.FindControl("txtParticipation")
            Dim s1 As String = txtParticipation.ClientID
            txtParticipation.Attributes.Add("gridTextBoxType", "calcTextBox")
            txtParticipation.Attributes.Add("Onblur", "return updateControls('" + grdParticipants.ClientID + "');")
            If IsNumeric(txtParticipation.Text) Then
                dPartyTotal += Convert.ToDouble(txtParticipation.Text)
            End If
            Dim linkRemove As LinkButton = e.Row.FindControl("btnRemove")
            linkRemove.CommandArgument = e.Row.RowIndex

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            Dim sVMode, sPMode As String
            If Request.QueryString("Mode") IsNot Nothing Then
                sVMode = Convert.ToString(Request.QueryString("Mode")).ToUpper.Trim
            End If
            If Request.QueryString("PMode") IsNot Nothing Then
                sPMode = Convert.ToString(Request.QueryString("PMode")).ToUpper.Trim
            End If
            If sVMode = "EDIT" Or sVMode = "VIEW" Or sPMode = "EDIT" Or sPMode = "VIEW" Then
                Dim txtPartyTotal As TextBox = e.Row.FindControl("txtTotalParticipation")
                txtPartyTotal.Text = dPartyTotal
            End If
        End If
        btnCancel.Enabled = True

    End Sub

#End Region
End Class
