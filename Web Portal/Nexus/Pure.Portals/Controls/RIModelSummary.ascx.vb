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
Imports System.Xml
Imports Nexus.Reinsurance
Imports System.Linq

Namespace Nexus

    Partial Class secure_Control_RIModelSummary : Inherits System.Web.UI.UserControl

        Dim oWebService As NexusProvider.ProviderBase
        'instance of Priority Lines
        Dim oRIPriorityLinesCollections As New NexusProvider.PriorityLineCollections

        Private oMaster As ContentPlaceHolder
        Dim oPersonalClientControl As Object
        Dim dExtendedLimitAmount As Double
        Dim bIsExtendedLimitAmount As Boolean
        Private oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
        Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                PopulateModelSummary()
            End If
        End Sub
        ''' <summary>
        ''' this will create a coolection of nodes which will be used to populate the tree
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub PopulateModelSummary()
            If Me.Visible Then

                oMaster = GetMasterPlaceHolder(Page, oNexusConfig.MainContainerName)
                Dim iBandKey As Int32 = Convert.ToInt32(Session(CNRIBandKey))
                Dim oUserControl As Object = oMaster.FindControl("ReInsurance2007Cntrl")
                Dim iRIVersion As Integer = 0
                If oUserControl IsNot Nothing Then
                    Dim ddlReinsurance As Object = oUserControl.FindControl("ddlReinsurance")
                    Dim oRIVersion As Object = oUserControl.FindControl("ddlRIVersion")
                    If ddlReinsurance IsNot Nothing Then
                        Session(CNRIBandKey) = CType(ddlReinsurance, DropDownList).SelectedValue
                    End If
                    If oRIVersion IsNot Nothing Then
                        Dim ddlRIVersion As DropDownList = CType(oRIVersion, DropDownList)
                        If ddlRIVersion.Visible = True AndAlso ddlRIVersion.Items.Count > 0 Then
                            iRIVersion = ddlRIVersion.SelectedValue
                        End If
                    End If
                End If

                'Try
                Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
                Dim sRIModelCode As String = ""
                Dim iRiskKey As Integer = 0
                Dim oGetRIArrangementTypeCol As New NexusProvider.ArrangementsTypeCollection

                'get the risk key
                If Session(CNCurrentRiskKey) IsNot Nothing Then
                    iRiskKey = CType(Session(CNQuote), NexusProvider.Quote).Risks(Session(CNCurrentRiskKey)).Key
                End If

                If iRiskKey = 0 Then
                    iRiskKey = DirectCast(Session(CNRisks), NexusProvider.RiskCollection).Item(0).Key
                End If
                'Remove all existing nodes(if any)
                treeRIModel.Nodes.Clear()

                'get RI Model code
                oGetRIArrangementTypeCol = oWebService.GetRiskReinsuranceArrangements(iRiskKey, iRIVersion)
                For Each oArrangementType As NexusProvider.ArrangementsType In oGetRIArrangementTypeCol
                    If oArrangementType.RIModelCode IsNot Nothing AndAlso oArrangementType.BandId = Session(CNRIBandKey) AndAlso oArrangementType.IsOriginal = False Then
                        'Get Model Detail 
                        Dim bShowAllPriorityLines As Boolean = False
                        If oArrangementType.IsExtendedLimitapplied = True Then
                            bIsExtendedLimitAmount = oArrangementType.IsExtendedLimitapplied
                            If Not String.IsNullOrEmpty(oArrangementType.ExtendedLimitAmount) Then
                                dExtendedLimitAmount = Convert.ToDouble(oArrangementType.ExtendedLimitAmount)
                            End If
                        End If


                        'If both models are same then show all lines in a single model
                        If oArrangementType.XOLRIModelCode = oArrangementType.RIModelCode Then
                            ViewState("RITreatyLinesCollectionsFor" + oArrangementType.ModelId.ToString()) = Nothing
                            bShowAllPriorityLines = True
                            GetRIModelDetail(oArrangementType.RIModelCode, False, True, 1, oArrangementType.ArrangementId)
                        Else
                            ViewState("RITreatyLinesCollectionsFor" + oArrangementType.ModelId.ToString()) = Nothing
                            GetRIModelDetail(oArrangementType.RIModelCode, False, False, 1, oArrangementType.ArrangementId)
                        End If
                        'If XOL Model is also attached then show XOL model details
                        If oArrangementType.XOLRIModelCode <> oArrangementType.RIModelCode Then
                            ViewState("oRITreatyPartyLinesCollectionsFor" + oArrangementType.XOLRIModelID.ToString()) = Nothing
                            GetRIModelDetail(oArrangementType.XOLRIModelCode, True, False, 1, oArrangementType.ArrangementId)
                        End If
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Calculate total amount for lines with same priority
        ''' </summary>
        Private Function CalculateTotalAmount(ByVal riModelLineDetails As NexusProvider.RIModelLineDetailsCollection, ByVal priority As Integer, Optional ByVal treatyTypeCode As String = "") As Decimal
            Dim totalAmount As Decimal = 0
            For Each oSumLine In riModelLineDetails
                If oSumLine.Priority = priority AndAlso (String.IsNullOrEmpty(treatyTypeCode) OrElse oSumLine.TreatyTypeCode = treatyTypeCode) Then
                    If treatyTypeCode = "PROP" Then
                        totalAmount += oSumLine.LineLimit
                    Else
                        totalAmount += (oSumLine.LineLimit - oSumLine.LowerLimit)
                    End If
                End If
            Next
            Return totalAmount
        End Function

        ''' <summary>
        ''' Create priority line node
        ''' </summary>
        Private Function CreatePriorityLineNode(ByVal oPriorityLine As NexusProvider.RIModelLineDetails, ByVal totalAmount As Decimal, ByVal modelNode As TreeNode, Optional ByVal treatyTypeCode As String = "", Optional ByVal v_iManualCount As Integer = 1) As TreeNode
            Dim PriorityLine As TreeNode = New TreeNode()
            Dim sSurplusCodes() As String = {"001", "002", "003"}
            Dim bIsSurplus As Boolean = Array.IndexOf(sSurplusCodes, oPriorityLine.ReinsuranceTypeCode.Trim()) >= 0
            Dim sNoOfLines As String = If(oPriorityLine.ManuallyAdded, v_iManualCount.ToString(), If(bIsSurplus, FormatNumber(oPriorityLine.NoOfLines, 2), CInt(oPriorityLine.NoOfLines).ToString()))
            PriorityLine.Text = "Priority " & oPriorityLine.Priority & "-" & sNoOfLines & " line(s) of " & FormatNumber(totalAmount, 2)
            PriorityLine.Value = oPriorityLine.Priority
            PriorityLine.PopulateOnDemand = True
            modelNode.ChildNodes.Add(PriorityLine)
            Return PriorityLine
        End Function

        ''' <summary>
        ''' Create RI Model detail node in tree and adding associated priority lines
        ''' </summary>
        ''' <param name="sRIModelCode"></param>
        ''' <param name="bIsXOL"></param>
        ''' <remarks></remarks>
        Public Sub GetRIModelDetail(ByVal sRIModelCode As String, Optional ByVal bIsXOL As Boolean = False,
                                    Optional ByVal bShowAllPriorityLines As Boolean = False,
                                     Optional ByVal v_iFilterType As Integer = 0,
                                    Optional ByVal v_lRIArrangementID As Long = 0)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'Instance of treaty Lines
            Dim oRITreatyLinesCollections As New NexusProvider.TreatyLineCollections
            'Instance of Treaty Party Lines
            Dim oRITreatyPartyLinesCollections As New NexusProvider.TreatyPartyLineCollections

            Dim oReinsurance As New NexusProvider.Reinsurances
            oReinsurance = oWebService.RIModelDetails(sRIModelCode,, bIsXOL, v_iFilterType, v_lRIArrangementID)

            With oReinsurance
                ' Add RI Model Main Node
                Dim ModelNode As TreeNode = New TreeNode(.Description, .RIModelLineKey)
                ModelNode.SelectAction = TreeNodeSelectAction.Expand
                ModelNode.PopulateOnDemand = False
                ModelNode.Expanded = True
                treeRIModel.Nodes.Add(ModelNode)

                'Add RI Model Details
                Dim ModelDetail As TreeNode = New TreeNode("RI Model Details")
                ModelDetail.PopulateOnDemand = False
                ModelNode.ChildNodes.Add(ModelDetail)
                'Add effective date etc to ModelDetail
                ModelDetail.ChildNodes.Add(New TreeNode("Effective Date : " + FormatDateTime(.EffectiveDate, DateFormat.LongDate)))
                If oReinsurance.ExpiryDate <> DateTime.MinValue Then
                    ModelDetail.ChildNodes.Add(New TreeNode("Expiry Date : " + FormatDateTime(.ExpiryDate, DateFormat.LongDate)))
                End If
                ModelDetail.ChildNodes.Add(New TreeNode("Claims Allocations : " + .FACPremiums))
                ModelDetail.ChildNodes.Add(New TreeNode("RI Model Currency : " + GetCurrencyForCode(.CurrencyCode)))
                If bIsExtendedLimitAmount = True Then
                    ModelDetail.ChildNodes.Add(New TreeNode("Extended Limit Amount : " + dExtendedLimitAmount.ToString()))
                End If
                ' Priority Line
                Dim oPriorityLine As NexusProvider.RIModelLineDetails
                Dim oPriorityLines As NexusProvider.RIModelLineDetailsCollection = .RIModelLineDetails

                oPriorityLines.SortColumn = "Priority"
                oPriorityLines.Sort()

                ' Normalize priorities: all manually added treaties share a single priority
                Dim iManualPriority As Integer = 0
                Dim allManualLines = .RIModelLineDetails.Cast(Of NexusProvider.RIModelLineDetails)().Where(Function(l) l.ManuallyAdded).ToList()
                If allManualLines.Count > 0 Then
                    ' Determine the single priority for all manual treaties (use max system priority + 1)
                    Dim iMaxSystemPriority As Integer = 0
                    For Each pl In .RIModelLineDetails
                        If Not pl.ManuallyAdded AndAlso pl.Priority > iMaxSystemPriority Then
                            iMaxSystemPriority = pl.Priority
                        End If
                    Next
                    iManualPriority = iMaxSystemPriority + 1
                    ' Set all manual lines to the same priority
                    For Each ml In allManualLines
                        ml.Priority = iManualPriority
                    Next
                End If

                Dim iOldPriorityKey As Integer = 0
                For Each oPriorityLine In .RIModelLineDetails
                    If oPriorityLine.Priority <> iOldPriorityKey Then
                        Dim totalAmount As Decimal = 0
                        Dim iManualCount As Integer = .RIModelLineDetails.Cast(Of NexusProvider.RIModelLineDetails)().Count(Function(l) l.ManuallyAdded AndAlso l.Priority = oPriorityLine.Priority)

                        If bShowAllPriorityLines Then
                            totalAmount = CalculateTotalAmount(.RIModelLineDetails, oPriorityLine.Priority)
                            iOldPriorityKey = oPriorityLine.Priority
                            CreatePriorityLineNode(oPriorityLine, totalAmount, ModelNode, oPriorityLine.TreatyTypeCode, iManualCount)
                        ElseIf bIsXOL = True AndAlso (oPriorityLine.TreatyTypeCode = "XOL" OrElse oPriorityLine.ManuallyAdded) Then
                            totalAmount = CalculateTotalAmount(.RIModelLineDetails, oPriorityLine.Priority, "XOL")
                            iOldPriorityKey = oPriorityLine.Priority
                            CreatePriorityLineNode(oPriorityLine, totalAmount, ModelNode, "XOL", iManualCount)
                        ElseIf bIsXOL = False AndAlso (oPriorityLine.TreatyTypeCode = "PROP" OrElse oPriorityLine.ManuallyAdded) Then
                            totalAmount = CalculateTotalAmount(.RIModelLineDetails, oPriorityLine.Priority, "PROP")
                            iOldPriorityKey = oPriorityLine.Priority
                            CreatePriorityLineNode(oPriorityLine, totalAmount, ModelNode, "PROP", iManualCount)
                        End If
                    End If
                Next

                If ViewState("RITreatyLinesCollectionsFor" + .RIModelLineKey.ToString()) IsNot Nothing Then
                    oRITreatyLinesCollections = ViewState("RITreatyLinesCollectionsFor" + .RIModelLineKey.ToString())
                End If

                'Get all treaty lines and save them in viewstate. 
                'This collection will be required when user will expand the treaty node for first time
                Dim oTreatyLine As NexusProvider.RIModelLineDetails
                Dim oRITreatyLines As NexusProvider.TreatyLine
                For Each oTreatyLine In .RIModelLineDetails
                    If bShowAllPriorityLines = True Then
                        oRITreatyLines = New NexusProvider.TreatyLine
                        oRITreatyLines.TreatyKey = oTreatyLine.TreatyKey
                        oRITreatyLines.TreatyName = oTreatyLine.TreatyCode
                        oRITreatyLines.Description = "Treaty(" & If(oTreatyLine.ManuallyAdded, "100", oTreatyLine.SharePercentage) & "%) " & oTreatyLine.Description
                        oRITreatyLines.TreatyPriorityKey = oTreatyLine.Priority
                        oRITreatyLines.CedingRate = oTreatyLine.CedingRate
                        oRITreatyLines.ReinsuranceTypeCode = oTreatyLine.ReinsuranceTypeCode
                        oRITreatyLines.LowerLimit = oTreatyLine.LowerLimit
                        oRITreatyLines.UpperLimit = oTreatyLine.LineLimit
                        oRITreatyLines.EffectiveDate = oTreatyLine.EffectiveDate
                        oRITreatyLines.ExpiryDate = oTreatyLine.ExpiryDate
                        oRITreatyLinesCollections.Add(oRITreatyLines)
                    Else
                        If bIsXOL = True AndAlso (oTreatyLine.TreatyTypeCode = "XOL" OrElse oTreatyLine.ManuallyAdded) Then
                            oRITreatyLines = New NexusProvider.TreatyLine
                            oRITreatyLines.TreatyKey = oTreatyLine.TreatyKey
                            oRITreatyLines.TreatyName = oTreatyLine.TreatyCode
                            oRITreatyLines.Description = "Treaty(" & If(oTreatyLine.ManuallyAdded, "100", oTreatyLine.SharePercentage) & "%) " & oTreatyLine.Description
                            oRITreatyLines.TreatyPriorityKey = oTreatyLine.Priority
                            oRITreatyLines.CedingRate = oTreatyLine.CedingRate
                            oRITreatyLines.ReinsuranceTypeCode = oTreatyLine.ReinsuranceTypeCode
                            oRITreatyLines.LowerLimit = oTreatyLine.LowerLimit
                            oRITreatyLines.UpperLimit = oTreatyLine.LineLimit
                            oRITreatyLines.EffectiveDate = oTreatyLine.EffectiveDate
                            oRITreatyLines.ExpiryDate = oTreatyLine.ExpiryDate
                            oRITreatyLinesCollections.Add(oRITreatyLines)
                        ElseIf bIsXOL = False AndAlso (oTreatyLine.TreatyTypeCode = "PROP" OrElse oTreatyLine.ManuallyAdded) Then
                            oRITreatyLines = New NexusProvider.TreatyLine
                            oRITreatyLines.TreatyKey = oTreatyLine.TreatyKey
                            oRITreatyLines.TreatyName = oTreatyLine.TreatyCode
                            oRITreatyLines.Description = "Treaty(" & If(oTreatyLine.ManuallyAdded, "100", oTreatyLine.SharePercentage) & "%) " & oTreatyLine.Description
                            oRITreatyLines.TreatyPriorityKey = oTreatyLine.Priority
                            oRITreatyLines.CedingRate = oTreatyLine.CedingRate
                            oRITreatyLines.ReinsuranceTypeCode = oTreatyLine.ReinsuranceTypeCode
                            oRITreatyLines.LowerLimit = oTreatyLine.LowerLimit
                            oRITreatyLines.UpperLimit = oTreatyLine.LineLimit
                            oRITreatyLines.EffectiveDate = oTreatyLine.EffectiveDate
                            oRITreatyLines.ExpiryDate = oTreatyLine.ExpiryDate
                            oRITreatyLinesCollections.Add(oRITreatyLines)
                        End If
                    End If
                Next
                ViewState("RITreatyLinesCollectionsFor" + .RIModelLineKey.ToString()) = oRITreatyLinesCollections

                'Get all treaty party lines and save them in viewstate. 
                'This collection will be required when user will expand the treaty party node for first time
                If ViewState("RITreatyPartyLinesCollectionsFor" + .RIModelLineKey.ToString()) IsNot Nothing Then
                    oRITreatyPartyLinesCollections = ViewState("RITreatyPartyLinesCollectionsFor" + .RIModelLineKey.ToString())
                End If
                Dim oRITreatyPartyLines As NexusProvider.TreatyPartyLine
                For Each oTreatyPartyLine In .RITreatyParty

                    oRITreatyPartyLines = New NexusProvider.TreatyPartyLine
                    oRITreatyPartyLines.TreatyPartyKey = oTreatyPartyLine.TreatyPartyKey
                    oRITreatyPartyLines.TreatyPartyName = oTreatyPartyLine.ResolvedName
                    oRITreatyPartyLines.Description = oTreatyPartyLine.ResolvedName & "[" & oTreatyPartyLine.SharePercentage & "%]"
                    oRITreatyPartyLines.TreatyKey = oTreatyPartyLine.TreatyKey
                    If oRITreatyLinesCollections.Count > 0 Then
                        For Each oTreatyLineSub In oRITreatyLinesCollections
                            If oTreatyPartyLine.TreatyKey = oTreatyLineSub.TreatyKey Then
                                oRITreatyPartyLines.ReinsuranceTypeCode = oTreatyLineSub.ReinsuranceTypeCode
                                oRITreatyPartyLines.LowerLimit = oTreatyLineSub.LowerLimit
                                oRITreatyPartyLines.UpperLimit = oTreatyLineSub.UpperLimit
                                oRITreatyPartyLines.CedingRate = oTreatyLineSub.CedingRate
                                oRITreatyPartyLines.EffectiveDate = FormatDateTime(oTreatyLineSub.EffectiveDate, DateFormat.LongDate)
                                oRITreatyPartyLines.ExpiryDate = FormatDateTime(oTreatyLineSub.ExpiryDate, DateFormat.LongDate)
                                Exit For
                            End If
                        Next
                    End If
                    oRITreatyPartyLinesCollections.Add(oRITreatyPartyLines)

                Next

                ViewState("RITreatyPartyLinesCollectionsFor" + .RIModelLineKey.ToString()) = oRITreatyPartyLinesCollections

            End With
        End Sub

        ''' <summary>
        ''' get the treaty lines
        ''' </summary>
        ''' <param name="node"></param>
        ''' <remarks></remarks>
        Protected Sub GetTreatyLines(ByVal node As TreeNode)
            'Instance of treaty Lines
            Dim oRITreatyLinesCollections As NexusProvider.TreatyLineCollections = ViewState("RITreatyLinesCollectionsFor" + node.Parent.Value)
            Dim iPrioriityLineId As Integer = node.Value
            If oRITreatyLinesCollections IsNot Nothing AndAlso oRITreatyLinesCollections.Count > 0 Then
                For Each C As NexusProvider.TreatyLine In oRITreatyLinesCollections
                    If iPrioriityLineId = C.TreatyPriorityKey Then
                        Dim newNode As TreeNode = New TreeNode(C.TreatyName, C.TreatyKey)
                        newNode.SelectAction = TreeNodeSelectAction.Expand
                        newNode.Text = C.Description
                        newNode.PopulateOnDemand = True
                        node.ChildNodes.Add(newNode)
                    End If
                Next
            End If
        End Sub
        ''' <summary>
        ''' get the treaty party lines
        ''' </summary>
        ''' <param name="node"></param>
        ''' <remarks></remarks>
        Protected Sub GetRIPartyLines(ByVal node As TreeNode)

            Dim iTreatyLineId As Integer = node.Value
            Dim newNode As TreeNode
            Dim oRITreatyPartyLinesCollections As NexusProvider.TreatyPartyLineCollections = ViewState("RITreatyPartyLinesCollectionsFor" + node.Parent.Parent.Value)
            If oRITreatyPartyLinesCollections IsNot Nothing AndAlso oRITreatyPartyLinesCollections.Count > 0 Then
                For Each C As NexusProvider.TreatyPartyLine In oRITreatyPartyLinesCollections
                    If iTreatyLineId = C.TreatyKey Then
                        newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                        newNode.Text = C.Description
                        node.ChildNodes.Add(newNode)
                        newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                        If C.ReinsuranceTypeCode.Trim = "XOL" OrElse C.ReinsuranceTypeCode.Trim = "FAX" OrElse C.ReinsuranceTypeCode.Trim = "CAT" Then
                            newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                            newNode.Text = "Effective Date : " & Convert.ToString(FormatDateTime(C.EffectiveDate, DateFormat.LongDate))
                            node.ChildNodes.Add(newNode)
                            If C.ExpiryDate <> DateTime.MinValue Then
                                newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                                newNode.Text = "Expiry Date : " & Convert.ToString(FormatDateTime(C.ExpiryDate, DateFormat.LongDate))
                                node.ChildNodes.Add(newNode)
                            End If
                            newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                            newNode.Text = "Ceding Rate : " & Convert.ToString(C.CedingRate) & "%"
                            node.ChildNodes.Add(newNode)
                            newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                            newNode.Text = "Lower Limit : " & FormatNumber(C.LowerLimit, 2)
                            node.ChildNodes.Add(newNode)
                            newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                            newNode.Text = "Upper Limit : " & FormatNumber(C.UpperLimit, 2)
                            node.ChildNodes.Add(newNode)
                        End If
                        If C.ReinsuranceTypeCode.Trim = "QUO" OrElse C.ReinsuranceTypeCode.Trim = "001" OrElse C.ReinsuranceTypeCode.Trim = "002" OrElse C.ReinsuranceTypeCode.Trim = "003" OrElse C.ReinsuranceTypeCode.Trim = "RET" Then
                            newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                            newNode.Text = "Effective Date : " & Convert.ToString(FormatDateTime(C.EffectiveDate, DateFormat.LongDate))
                            node.ChildNodes.Add(newNode)
                            If C.ExpiryDate <> DateTime.MinValue Then
                                newNode = New TreeNode(C.TreatyPartyName, C.TreatyPartyKey)
                                newNode.Text = "Expiry Date : " & Convert.ToString(FormatDateTime(C.ExpiryDate, DateFormat.LongDate))
                                node.ChildNodes.Add(newNode)
                            End If
                        End If
                    End If
                Next

            End If
        End Sub

        ''' <summary>
        ''' this will be called when there is need of expansion of node
        ''' </summary>
        ''' <param name="source"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Public Sub PopulateNode(ByVal source As Object, ByVal e As TreeNodeEventArgs)
            Select Case e.Node.Depth
                Case 1
                    GetTreatyLines(e.Node)
                Case 2
                    GetRIPartyLines(e.Node)
            End Select
        End Sub

        Protected Sub treeRIModel_TreeNodeCheckChanged(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles treeRIModel.TreeNodeCheckChanged

        End Sub

        Protected Sub treeRIModel_TreeNodeExpanded(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles treeRIModel.TreeNodeExpanded

        End Sub

        Protected Sub treeRIModel_TreeNodePopulate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.TreeNodeEventArgs) Handles treeRIModel.TreeNodePopulate
            'Nothing to do. "PopulateNode" function will be called after this event
        End Sub

    End Class
End Namespace