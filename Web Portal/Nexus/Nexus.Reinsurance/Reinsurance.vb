Imports System.Xml
Imports System.Data
Imports Newtonsoft.Json

Public Enum TreatyTypeID
    Proportional = 1
    NonProportional = 2
End Enum

Public Class Reinsurance

    ''' <summary>
    ''' this will be called when a new line is added.
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="oArrangementLinesType"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sRIType"></param>
    ''' <param name="sTransType"></param>
    ''' <param name="bEditCall"></param>
    ''' <param name="sEditiedRIArrangementLineKey"></param>
    ''' <remarks></remarks>
    Public Sub AddRIArrangementLines(ByRef sXML As String,
                                        ByVal oArrangementLinesType As ArrangementLinesType,
                                        ByVal sRIBANDID As String,
                                        Optional ByVal sRIArrangementLineKey As String = "",
                                        Optional ByVal sRIarrangementKey As String = "",
                                        Optional ByVal sRIType As String = "",
                                        Optional ByVal sTransType As String = "NB",
                                        Optional ByVal bEditCall As Boolean = False,
                                        Optional ByRef sEditiedRIArrangementLineKey As String = "0",
                                        Optional ByVal bIsPortfolioRIAmendment As Boolean = False,
                                        Optional ByVal bIsManuallyAdded As Boolean = False)
        'Initialize the xml  doc element
        Dim oXMLDoc As New XmlDocument
        'Load the xml
        oXMLDoc.LoadXml(sXML)
        'set the instance to get the MAX FAC id
        Dim iMAXFRIID, iMAXFXRIID, iMAXTRIID As Integer
        'initialize the Fac variable to find whetherFAC  is present or not
        Dim bFACPropPresent, bFACXOLPresent, bFoundTreatyQSH As Boolean
        Dim dLineLimit, dLowerLimit As Double
        Dim oDocElement As XmlElement = oXMLDoc.DocumentElement
        Dim sElement As String = "RIBAND"
        Dim bInsertBefore As Boolean = False
        Dim random As New Random()

        'get the RIBand node
        Dim RIBAND As XmlNode = oDocElement.SelectSingleNode("//*[@Name='Current_" & sRIBANDID & "']")

        'add the new arrangement line type
        'Check the RIPlcament details if it is nothing then no need to add the lines
        If oArrangementLinesType.RIPlacement IsNot Nothing AndAlso oArrangementLinesType.RIPlacement <> "" Then
            Dim sArrangementRow As String = "ArrangementRow"
            Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
            Dim iRIArrangementLineKey As Integer
            Integer.TryParse(sRIArrangementLineKey, iRIArrangementLineKey)
            'set IsNew Field as True
            With oArrangementLinesType
                ArrangementRow.SetAttribute("IsNew", True)
                ArrangementRow.SetAttribute("IsEdited", False)
                ArrangementRow.SetAttribute("IsEditedDB", False)
                ArrangementRow.SetAttribute("IsDeleted", False)
                If bIsManuallyAdded Then
                    ArrangementRow.SetAttribute("ManuallyAdded", True)
                    ArrangementRow.SetAttribute("IsObligatory", False)
                    ArrangementRow.SetAttribute("IsPortfolioTransferred", False)
                    If sRIType = "T" Then
                        ArrangementRow.SetAttribute("TreatyTypeID", TreatyTypeID.Proportional)
                    ElseIf sRIType = "TX" OrElse sRIType = "PX" Then
                        ArrangementRow.SetAttribute("TreatyTypeID", TreatyTypeID.NonProportional)
                    End If
                End If
                ArrangementRow.SetAttribute("Type", sRIType)
                ArrangementRow.SetAttribute("TreatyCode", .TreatyCode)
                If .TreatyID <> 0 Then
                    ArrangementRow.SetAttribute("TreatyId", .TreatyID)
                End If
                ' If a tombstone exists for this key, treat as new insert
                Dim oTombstone As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey & "' and @IsDeleted='True']")
                If oTombstone IsNot Nothing Then sRIArrangementLineKey = "0"
                ArrangementRow.SetAttribute("RIArrangementLineKey", sRIArrangementLineKey)
                ArrangementRow.SetAttribute("RIarrangementKey", sRIarrangementKey)
                ArrangementRow.SetAttribute("Retained", .Retained)
                ArrangementRow.SetAttribute("ReinsuranceTypeCode", .ReinsuranceTypeCode)
                If bIsManuallyAdded Then
                    Dim iMaxPriority As Integer = 0
                    Dim oAllNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Priority > 0 and @Type!='F' and @Type!='FX']")
                    For Each oPNode As XmlNode In oAllNodes
                        If oPNode.Attributes("IsDeleted") Is Nothing OrElse oPNode.Attributes("IsDeleted").Value <> "True" Then
                            Dim iPriority As Integer = 0
                            If Integer.TryParse(oPNode.Attributes("Priority").Value, iPriority) AndAlso iPriority > iMaxPriority Then
                                iMaxPriority = iPriority
                            End If
                        End If
                    Next
                    ArrangementRow.SetAttribute("Priority", iMaxPriority + 1)
                Else
                    ArrangementRow.SetAttribute("Priority", (random.Next(0, Integer.MaxValue - iRIArrangementLineKey - 1) + iRIArrangementLineKey))
                End If
                ArrangementRow.SetAttribute("PremiumPercent", Format(Math.Round(.PremiumPercent, 2), "0.00"))
                ArrangementRow.SetAttribute("PartyKey", .PartyKey)
                ArrangementRow.SetAttribute("ParticipationPercent", Format(Math.Round(.ParticipationPercent, 2), "0.00"))
                If bIsManuallyAdded AndAlso sRIType = "T" Then
                    ArrangementRow.SetAttribute("NumberOfLines", 1)
                Else
                    ArrangementRow.SetAttribute("NumberOfLines", .NumberOfLines)
                End If

                ArrangementRow.SetAttribute("LineLimit", Format(Math.Round(.LineLimit, 2), "0.00"))
                'get then limits which will be used in shorting
                dLineLimit = Math.Round(.LineLimit, 2)
                ArrangementRow.SetAttribute("LowerLimit", Format(Math.Round(.LowerLimit, 2), "0.00"))
                dLowerLimit = Math.Round(.LowerLimit, 2)
                'Store retained limits for display in NB
                If sRIType = "R" Then
                    ArrangementRow.SetAttribute("RetainedLineLimit", Format(Math.Round(.LineLimit, 2), "0.00"))
                    ArrangementRow.SetAttribute("RetainedLowerLimit", Format(Math.Round(.LowerLimit, 2), "0.00"))
                End If
                ArrangementRow.SetAttribute("IsRIBroker", .IsBroker)
                If bEditCall Then
                    ArrangementRow.SetAttribute("IsRIBroker", .IsRIBroker)
                End If
                ArrangementRow.SetAttribute("IsDomiciledForTax", .IsDomiciledForTax)
                ArrangementRow.SetAttribute("IsCommissionModified", .IsCommissionModified)
                ArrangementRow.SetAttribute("Grouping", .Grouping)
                ArrangementRow.SetAttribute("CedePremiumOnly", .CedePremiumOnly)
                ArrangementRow.SetAttribute("ActionType", If(bIsManuallyAdded, "0", .ActionType))
                ArrangementRow.SetAttribute("Placement", .RIPlacement)
                ArrangementRow.SetAttribute("Name", .RIName)
                ArrangementRow.SetAttribute("DefaultPerc", Format(Math.Round(.DefaultPerc, 4), "0.0000"))
                ArrangementRow.SetAttribute("ThisPerc", Format(Math.Round(.ThisPerc, 4), "0.0000"))
                ArrangementRow.SetAttribute("SumInsured", Format(Math.Round(.SumInsured, 2), "0.00"))
                ArrangementRow.SetAttribute("Premium", Format(Math.Round(.PremiumValue, 2), "0.00"))
                ArrangementRow.SetAttribute("Tax", Format(Math.Round(.Tax, 2), "0.00"))
                ArrangementRow.SetAttribute("CommissionPerc", Format(Math.Round(.CommissionPerc, 4), "0.0000"))
                ArrangementRow.SetAttribute("Commission", Format(Math.Round(.CommissionValue, 2), "0.00"))
                ArrangementRow.SetAttribute("CommissionTax", Format(Math.Round(.CommissionTax, 2), "0.00"))
                ArrangementRow.SetAttribute("Agreement", .AgreementCode)
                If .RIPlacement = "FAC Prop" Then
                    ArrangementRow.SetAttribute("FACPropPremiumPerc", Format(Math.Round(.ThisPerc, 4), "0.0000"))
                    ArrangementRow.SetAttribute("FACPremiumType", "")
                End If
            End With

            'process FAC XOL
            If oArrangementLinesType.FAXParticipants IsNot Nothing AndAlso oArrangementLinesType.FAXParticipants.Count > 0 Then
                Dim oFACPart As FAXParticipants
                Dim oFACPartCol As New FAXParticipantsCollection
                For Each oFACPart In oArrangementLinesType.FAXParticipants
                    Dim sFACParticipentRow As String = "FAXParticipentRow"
                    Dim FACParticipentRow As XmlElement = oXMLDoc.CreateElement(sFACParticipentRow)
                    With oFACPart
                        FACParticipentRow.SetAttribute("IsNew", True)
                        FACParticipentRow.SetAttribute("IsDeleted", False)
                        FACParticipentRow.SetAttribute("AccountType", .AccountType)
                        FACParticipentRow.SetAttribute("AgreementCode", .AgreementCode)
                        FACParticipentRow.SetAttribute("CommissionPercent", Format(Math.Round(.CommissionPercent, 4), "0.0000"))
                        FACParticipentRow.SetAttribute("CommissionTax", Format(Math.Round(.CommissionTax, 2), "0.00"))
                        FACParticipentRow.SetAttribute("CommissionValue", Format(Math.Round(.CommissionValue, 2), "0.00"))
                        FACParticipentRow.SetAttribute("ParticipationPercentage", Format(Math.Round(.ParticipationPercentage, 2), "0.0000"))
                        FACParticipentRow.SetAttribute("PartyCode", .PartyCode)
                        FACParticipentRow.SetAttribute("PartyKey", .PartyKey)
                        FACParticipentRow.SetAttribute("PartyName", .PartyName)
                        FACParticipentRow.SetAttribute("PremiumTax", Format(Math.Round(.PremiumTax, 2), "0.00"))
                        FACParticipentRow.SetAttribute("PremiumValue", Format(Math.Round(.PremiumValue, 2), "0.00"))
                        FACParticipentRow.SetAttribute("RIArrangementLineKey", .RIArrangementLineKey)
                        FACParticipentRow.SetAttribute("SumInsured", Format(Math.Round(.SumInsured, 2), "0.00"))
                        'process FAX XOL Broker participants
                        If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                            Dim oBrokerPartCol As New BrokerParticipantsCollection
                            For Each oBrokerParticipant As BrokerParticipants In .BrokerParticipants
                                Dim sBrokerParticipentRow As String = "FAXBrokerParticipentRow"
                                Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)
                                With oBrokerParticipant
                                    BrokerParticipentRow.SetAttribute("IsNew", True)
                                    BrokerParticipentRow.SetAttribute("IsDeleted", False)
                                    BrokerParticipentRow.SetAttribute("PartyCode", .PartyCode)
                                    BrokerParticipentRow.SetAttribute("PartyName", .PartyName)
                                    BrokerParticipentRow.SetAttribute("PartyKey", .PartyKey)
                                    BrokerParticipentRow.SetAttribute("ParticipationPercent", Format(Math.Round(.ParticipationPercentage, 2), "0.0000"))
                                    BrokerParticipentRow.SetAttribute("RIArrangementLineKey", sRIArrangementLineKey)
                                    BrokerParticipentRow.SetAttribute("RIarrangementKey", sRIarrangementKey)
                                End With
                                'append the child in FAX participants
                                FACParticipentRow.AppendChild(BrokerParticipentRow)
                            Next
                        End If
                    End With
                    'aappend the FAX in arrangement row
                    ArrangementRow.AppendChild(FACParticipentRow)
                Next
            End If

            'Process FAC Prop Broker Participants
            If oArrangementLinesType.BrokerParticipants IsNot Nothing AndAlso oArrangementLinesType.BrokerParticipants.Count > 0 Then
                Dim oBrokerPart As BrokerParticipants
                Dim oBrokerPartCol As New BrokerParticipantsCollection
                For Each oBrokerPart In oArrangementLinesType.BrokerParticipants
                    Dim sBrokerParticipentRow As String = "BrokerParticipentRow"
                    Dim BrokerParticipentRow As XmlElement = oXMLDoc.CreateElement(sBrokerParticipentRow)
                    With oBrokerPart
                        BrokerParticipentRow.SetAttribute("IsNew", True)
                        BrokerParticipentRow.SetAttribute("IsDeleted", False)
                        BrokerParticipentRow.SetAttribute("PartyCode", .PartyCode)
                        BrokerParticipentRow.SetAttribute("PartyName", .PartyName)
                        BrokerParticipentRow.SetAttribute("PartyKey", .PartyKey)
                        BrokerParticipentRow.SetAttribute("ParticipationPercent", Format(Math.Round(.ParticipationPercentage, 2), "0.0000"))
                        BrokerParticipentRow.SetAttribute("RIArrangementLineKey", sRIArrangementLineKey)
                        BrokerParticipentRow.SetAttribute("RIarrangementKey", sRIarrangementKey)
                    End With
                    ArrangementRow.AppendChild(BrokerParticipentRow)
                Next
            End If

            ' Calculate 'T' Type
            Dim oTNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='T']")
            For Each oTNode As XmlNode In oTNodes
                If Convert.ToString(oTNode.Attributes("IsDeleted").Value) <> "True" AndAlso Convert.ToString(oTNode.Attributes("IsObligatory").Value) = "True" Then
                    bFoundTreatyQSH = True
                    'get the MAX RI Line id
                    If Convert.ToInt32(oTNode.Attributes("RIArrangementLineKey").Value) > iMAXTRIID Then
                        iMAXTRIID = Convert.ToInt32(oTNode.Attributes("RIArrangementLineKey").Value)
                    End If
                End If
            Next

            ' Calculate 'F' Type
            Dim oFNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='F']")
            For Each oFNode As XmlNode In oFNodes
                If Convert.ToString(oFNode.Attributes("IsDeleted").Value) <> "True" Then
                    bFACPropPresent = True
                    If Convert.ToInt32(oFNode.Attributes("RIArrangementLineKey").Value) > iMAXFRIID Then
                        iMAXFRIID = Convert.ToInt32(oFNode.Attributes("RIArrangementLineKey").Value)
                    End If
                End If
            Next

            'calculaye and get Max key of FAC
            Dim oFXNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='FX']")
            For Each oFXNode As XmlNode In oFXNodes
                If Convert.ToString(oFXNode.Attributes("IsDeleted").Value) <> "True" Then
                    bFACXOLPresent = True
                    If oFXNodes.Count > 1 Then
                        If Convert.ToDouble(oFXNode.Attributes("LineLimit").Value) >= dLowerLimit Then
                            bInsertBefore = False
                            iMAXFXRIID = Convert.ToInt32(oFXNode.Attributes("RIArrangementLineKey").Value)
                            Exit For
                        ElseIf Convert.ToDouble(oFXNode.Attributes("LowerLimit").Value) >= dLineLimit Then
                            bInsertBefore = True
                            iMAXFXRIID = Convert.ToInt32(oFXNode.Attributes("RIArrangementLineKey").Value)
                            Exit For
                        End If
                    Else
                        If Convert.ToInt32(oFXNode.Attributes("RIArrangementLineKey").Value) > iMAXFXRIID Then
                            iMAXFXRIID = Convert.ToInt32(oFXNode.Attributes("RIArrangementLineKey").Value)
                        End If
                    End If
                End If
            Next

            'Insert at specific position in the XML doc
            If bIsManuallyAdded Then
                'Manually added treaties always go last - find the last treaty node with Priority > 0 (excluding FAC lines)
                Dim oLastPriorityNode As XmlNode = Nothing
                Dim oPriorityNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Priority > 0 and @Type!='F' and @Type!='FX']")
                If oPriorityNodes IsNot Nothing AndAlso oPriorityNodes.Count > 0 Then
                    oLastPriorityNode = oPriorityNodes(oPriorityNodes.Count - 1)
                    RIBAND.InsertAfter(ArrangementRow, oLastPriorityNode)
                    'Update priority after insertion to ensure correct ordering
                    Dim iMaxPriority As Integer = 0
                    For Each oPNode As XmlNode In oPriorityNodes
                        If oPNode.Attributes("IsDeleted") Is Nothing OrElse oPNode.Attributes("IsDeleted").Value <> "True" Then
                            Dim iPriority As Integer = 0
                            If Integer.TryParse(oPNode.Attributes("Priority").Value, iPriority) AndAlso iPriority > iMaxPriority Then
                                iMaxPriority = iPriority
                            End If
                        End If
                    Next
                    ArrangementRow.SetAttribute("Priority", iMaxPriority + 1)
                Else
                    'No priority nodes exist, insert after Net of FAC or last FAC/FX/T node
                    Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
                    If oNetFACNode IsNot Nothing Then
                        RIBAND.InsertAfter(ArrangementRow, oNetFACNode)
                    ElseIf iMAXFXRIID > 0 AndAlso iMAXFXRIID > iMAXFRIID AndAlso iMAXFXRIID > iMAXTRIID Then
                        Dim oFXNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']")
                        If oFXNode IsNot Nothing Then RIBAND.InsertAfter(ArrangementRow, oFXNode) Else RIBAND.InsertAfter(ArrangementRow, RIBAND.FirstChild)
                    ElseIf iMAXFRIID > 0 AndAlso iMAXFRIID > iMAXTRIID Then
                        Dim oFNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFRIID & "']")
                        If oFNode IsNot Nothing Then RIBAND.InsertAfter(ArrangementRow, oFNode) Else RIBAND.InsertAfter(ArrangementRow, RIBAND.FirstChild)
                    ElseIf iMAXTRIID > 0 Then
                        Dim oTNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXTRIID & "']")
                        If oTNode IsNot Nothing Then RIBAND.InsertAfter(ArrangementRow, oTNode) Else RIBAND.InsertAfter(ArrangementRow, RIBAND.FirstChild)
                    Else
                        'No existing nodes, insert after Band Total
                        Dim oBandTotalNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                        If oBandTotalNode IsNot Nothing Then
                            RIBAND.InsertAfter(ArrangementRow, oBandTotalNode)
                        Else
                            RIBAND.InsertAfter(ArrangementRow, RIBAND.FirstChild)
                        End If
                    End If
                End If
            ElseIf bFACXOLPresent = False AndAlso bFACPropPresent = False AndAlso bFoundTreatyQSH = False Then
                'if none of the FAC is present then insert after the Band total
                RIBAND.InsertAfter(ArrangementRow, RIBAND.FirstChild)
            ElseIf bFACPropPresent = True AndAlso bFACXOLPresent = False AndAlso bFoundTreatyQSH = False Then
                'If Only Fac Prop is present then insert after first one
                RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFRIID & "']"))
            ElseIf bFoundTreatyQSH AndAlso bFACXOLPresent = False AndAlso bFACPropPresent = False Then
                RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXTRIID & "']"))
            ElseIf bFACXOLPresent = True AndAlso bFACPropPresent = False AndAlso bFoundTreatyQSH = False Then
                'if FAX is present then check the type of newly added arrangemnt row
                If sRIType.Trim = "F" Then
                    'Process F tpye placement
                    RIBAND.InsertBefore(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                Else
                    'Process FAX placement
                    If bInsertBefore Then
                        RIBAND.InsertBefore(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                    Else
                        RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                    End If
                End If
            ElseIf bFACXOLPresent = True AndAlso bFACPropPresent = False AndAlso bFoundTreatyQSH = True Then
                If bInsertBefore Then
                    RIBAND.InsertBefore(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                Else
                    RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                End If
            ElseIf bFACXOLPresent = False AndAlso bFACPropPresent = True AndAlso bFoundTreatyQSH = True Then
                RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFRIID & "']"))
            ElseIf bFACXOLPresent = True AndAlso bFACPropPresent = True Then
                'if both are present then again prcoess the insertion as per the RI Type.
                If sRIType.Trim = "F" Then
                    RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFRIID & "']"))
                Else
                    If bInsertBefore Then
                        RIBAND.InsertBefore(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                    Else
                        RIBAND.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                    End If
                End If
            End If
        End If

        'return the xml
        sXML = oDocElement.OuterXml
        'recalulate whole lines for the adjustement of SI ,Premium,Tax,commission.
        If sRIArrangementLineKey = "" Then
            sRIArrangementLineKey = "0"
        End If
        Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)

    End Sub
    ''' <summary>
    ''' this will be called when any of the line get edited
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="oArrangementLinesType"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sRIType"></param>
    ''' <param name="sTransType"></param>
    ''' <remarks></remarks>
    Public Sub EditRIArrangementLines(ByRef sXML As String,
                                   ByVal oArrangementLinesType As ArrangementLinesType,
                                   ByVal sRIBANDID As String,
                                   Optional ByVal sRIArrangementLineKey As String = "",
                                   Optional ByVal sRIarrangementKey As String = "",
                                   Optional ByVal sRIType As String = "",
                                    Optional ByVal sTransType As String = "NB")
        'Initialize the xml  doc element
        Dim oXMLDoc As New XmlDocument
        'Load the passed xml
        oXMLDoc.LoadXml(sXML)
        Dim oDocElement As XmlElement = oXMLDoc.DocumentElement
        Dim sElement As String = "RIBAND"
        Dim RIBAND As XmlNode = oDocElement.SelectSingleNode("//*[@Name='Current_" & sRIBANDID & "']")
        'get the node as per the RI arrangement Line key
        Dim oFNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        If oFNode IsNot Nothing Then
            'if it is not new line and alos not returned by SAm then delete it from XMl 
            If oFNode.Attributes("IsNew").Value.ToUpper.Trim = "FALSE" AndAlso oFNode.Attributes("IsEdited").Value.ToUpper.Trim = "TRUE" Then
                oFNode.Attributes("IsDeleted").Value = "True"
            Else
                'Remove into XML Document
                RIBAND.RemoveChild(oFNode)
            End If
        End If
        ''Get the xml
        sXML = oDocElement.OuterXml
        'add the newly edited line
        If sRIType <> "FX" Then
            AddRIArrangementLines(sXML, oArrangementLinesType, sRIBANDID,
                                        sRIArrangementLineKey,
                                        sRIarrangementKey,
                                        sRIType,
                                        sTransType, True)
        End If

    End Sub
    ''' <summary>
    ''' this will recaluate all the line values if any of the line is added or edited
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="iRIArrangementLineKey"></param>
    ''' <param name="sTransType"></param>
    ''' <param name="bDeleteFromXML"></param>
    ''' <param name="dTaxPercentage"></param>
    ''' <remarks></remarks>
    Public Sub Recalculate(ByRef sXML As String,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIarrangementKey As String,
                                    Optional ByVal iRIArrangementLineKey As String = "0",
                                    Optional ByVal sTransType As String = "NB",
                                    Optional ByVal bDeleteFromXML As Boolean = False,
                                    Optional ByVal dTaxPercentage As Double = 0,
                                    Optional ByVal bViewOnly As Boolean = False,
                                    Optional ByVal bIsPortfolioRIAmendment As Boolean = False,
                                    Optional ByVal bIsCancellation As Boolean = False)

        'Populate the XML into the Memory
        Dim srDataSet As New IO.StringReader(sXML)
        Dim xmlTR As New XmlTextReader(srDataSet)
        Dim oXMLDoc As New XmlDocument
        Dim v_iRIArrangementLineKey As Integer
        Dim ddefault_percent As Double
        Dim dthis_percent As Double

        Dim iRIModelId As Integer
        Dim iXolRIModelId As Integer

        'Treat empty string as 0
        If String.IsNullOrEmpty(iRIArrangementLineKey) Then
            v_iRIArrangementLineKey = 0
        Else
            Integer.TryParse(iRIArrangementLineKey, v_iRIArrangementLineKey)
        End If

        oXMLDoc.Load(xmlTR)
        xmlTR.Close()
        sRIBANDID = "Current_" & sRIBANDID

        If bViewOnly = False Then

            'Declare the parameters
            Dim dband_si As Double
            Dim dband_premium As Double

            'set the variable to handle the values which could be used to store gross totals
            Dim iSetBand_Premium, iret_line_id As Integer
            Dim inumber_of_lines As Decimal
            Dim dgrossnet_premium, dpriority_share, dGrossPremium As Double
            Dim ilast_priority As Integer
            Dim dthis_premium, drunning_premium, dfac_si, dfac_premium, d_CommTaxperc As Double
            Dim dfac_taxperc, dfac_Commperc, dfac_CommTaxperc, dFSI, dFXSI, dFPremium, dFXPremium As Double

            Dim dpriority_premium, dthis_si, drunning_si, dpriority_si, dManualThisPerc As Double
            Dim dline_limit, dlower_limit, dceding_rate, dFACTotalPremium, dRIArrangementSI, dGrossSI As Double
            Dim inegative_si, icede_premium_only, iSetBand_Premium_For_CAT As Integer
            Dim dpriority_limit As Double
            Dim QSTotal As Double
            Dim dgrossnet_SI As Double
            Dim dPriority_allocated_SI, dPriority_allocated_Premium As Double
            Dim oPriorityNodes As XmlNodeList

            'Root Element
            Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']")

            If iRIArrangementLineKey <> "0" Then
                Dim oDNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "' and (@IsDeleted='False' or not(@IsDeleted))]")
                If oDNode Is Nothing Then
                    ' Fallback: tombstone exists with same key, find by TreatyCode where IsNew=True
                    Dim oTombstone As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iRIArrangementLineKey & "' and @IsDeleted='True']")
                    If oTombstone IsNot Nothing AndAlso oTombstone.Attributes("TreatyCode") IsNot Nothing Then
                        Dim sTreatyCode As String = oTombstone.Attributes("TreatyCode").Value.Replace("'", "''")
                        oDNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode & "' and @IsNew='True' and (@IsDeleted='False' or not(@IsDeleted))]")
                    End If
                End If
                If bDeleteFromXML AndAlso oDNode IsNot Nothing Then
                    If Convert.ToString(oDNode.Attributes("IsNew").Value) = "True" Then
                        oRootNode.RemoveChild(oDNode)
                    Else
                        oDNode.Attributes("IsDeleted").Value = "True"
                    End If
                End If
            End If

            'Get Retained Line ID
            Dim oRetNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='R']")
            If oRetNode IsNot Nothing Then
                iret_line_id = Convert.ToInt32(oRetNode.Attributes("RIArrangementLineKey").Value)
            End If

            'Get Band SI & Premium
            'Along with Extended RI LImit
            Dim dExtendedLimitAmount As Double
            Dim bIsExtendedLimitApplied As Boolean

            Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
            If oNode IsNot Nothing Then
                Decimal.TryParse(oNode.Attributes("SumInsured").Value, dband_si)
                Decimal.TryParse(oNode.Attributes("Premium").Value, dband_premium)
                Double.TryParse(oNode.Attributes("ExtendedLimitAmount").Value, dExtendedLimitAmount)
                Boolean.TryParse(oNode.Attributes("IsExtendedLimitApplied").Value, bIsExtendedLimitApplied)
                Integer.TryParse(oNode.Attributes("RIModelId").Value, iRIModelId)
                Integer.TryParse(oNode.Attributes("XolRIModelId").Value, iXolRIModelId)
            End If

            'Set default values    
            drunning_si = dband_si
            drunning_premium = dband_premium
            ilast_priority = -666
            dpriority_share = 0
            dpriority_si = 0
            dpriority_premium = 0
            iSetBand_Premium = 0
            dgrossnet_premium = dband_premium
            dgrossnet_SI = dband_si
            dfac_premium = 0
            dfac_si = 0
            iSetBand_Premium_For_CAT = 0
            dFACTotalPremium = 0
            dRIArrangementSI = dband_si
            dGrossSI = dband_si
            dGrossPremium = dband_premium
            dPriority_allocated_SI = 0
            dPriority_allocated_Premium = 0
            dFSI = 0
            dFXSI = 0
            dFPremium = 0
            dFXPremium = 0

            If dband_si < 0 Then
                inegative_si = -1
            Else
                inegative_si = 1
            End If
            Dim bFACPropPresent As Boolean
            Dim bFACXOLPresent As Boolean
            Dim iMAXFRIID, iMAXFXRIID, iMAXTRIID As Integer

            ' Calculate Treaty TSH Type 
            Dim dOblTQSHPremium As Double
            Dim dOblTQSHSSI As Double
            Dim dTQS_Commperc As Double = 0
            Dim dTQS_CommTaxperc As Double = 0

            Dim bFoundTreatyQSH As Boolean = False
            Dim bIsObligatory As Boolean = True
            Dim bIsPortfolioTransferred As Boolean = True
            Dim dOblQSHPremium As Double
            Dim dOblQSHSI As Double
            Dim oTSHNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='T' and (not(@ManuallyAdded) or @ManuallyAdded='False') and @IsDeleted!='True']")
            For Each oTSHNode As XmlNode In oTSHNodes
                bIsObligatory = Convert.ToBoolean(oTSHNode.Attributes("IsObligatory").Value)
                If bIsObligatory = True Then
                    bFoundTreatyQSH = True
                    'Process Treaty Quota Share
                    CalculateFACQSH(oTSHNode, sRIBANDID, bIsObligatory, iMAXTRIID, drunning_si, dband_si, drunning_premium, dGrossSI, dGrossPremium, dRIArrangementSI, dfac_premium,
                            dfac_si, dband_premium, dOblTQSHPremium, dOblTQSHSSI)
                End If
            Next
            dOblQSHPremium = dOblTQSHPremium
            dOblQSHSI = dOblTQSHSSI

            ' Update the Net totals
            dgrossnet_SI = dgrossnet_SI - dOblTQSHSSI
            dgrossnet_premium = dgrossnet_premium - dOblQSHPremium

            ' Calculate 'F' Type
            Dim oFNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='F']")
            For Each oFNode As XmlNode In oFNodes
                'Check whether the node is live or not. if it is live then process the node
                If Convert.ToString(oFNode.Attributes("IsDeleted").Value) <> "True" Then
                    'set the presence of FACProp so the insertion could take place in right place
                    bFACPropPresent = True
                    'get the Default percentage
                    ddefault_percent = Convert.ToDouble(oFNode.Attributes("ThisPerc").Value)
                    'Format the default percenmtage to 4 decimal place and set it in ThisPercentage field
                    'get the MAX RI Line id
                    Dim sRIKey As String = If(oFNode.Attributes("RIArrangementLineKey")?.Value, "")
                    If Not String.IsNullOrEmpty(sRIKey) AndAlso IsNumeric(sRIKey) Then
                        If Convert.ToInt32(sRIKey) > iMAXFRIID Then
                            iMAXFRIID = Convert.ToInt32(sRIKey)
                        End If
                    End If
                    'get the Comm percentage from XML
                    dfac_Commperc = Convert.ToDouble(oFNode.Attributes("CommissionPerc").Value)
                    oFNode.Attributes("CommissionPerc").Value = Format(dfac_Commperc, "0.0000")
                    If iRIArrangementLineKey = "" Then
                        iRIArrangementLineKey = "0"
                    End If
                    If Not String.IsNullOrEmpty(sRIKey) AndAlso IsNumeric(sRIKey) AndAlso Convert.ToInt32(sRIKey) = Convert.ToInt32(iRIArrangementLineKey) AndAlso dTaxPercentage <> 0 Then
                        dfac_taxperc = dTaxPercentage
                        dfac_CommTaxperc = dTaxPercentage
                    Else
                        Dim premium = Convert.ToDouble(oFNode.Attributes("Premium").Value)
                        Dim tax = Convert.ToDouble(oFNode.Attributes("Tax").Value)
                        dfac_taxperc = If(premium = 0 OrElse tax = 0, 0, (tax * 100) / premium)

                        Dim commission = Convert.ToDouble(oFNode.Attributes("Commission").Value)
                        Dim commTax = Convert.ToDouble(oFNode.Attributes("CommissionTax").Value)
                        dfac_CommTaxperc = If(commission = 0 OrElse commTax = 0, 0, (commTax * 100) / commission)
                    End If
                    ' process Running SI and Band SI
                    'Initially Band si is equal to Running SI
                    If drunning_si > 0 OrElse dband_si = 0 Then
                        'Calculate This SI amount 
                        'Handled bankers rounding
                        dthis_si = Math.Round((((dband_si - dOblQSHSI) * ddefault_percent) / 100) + 0.00000001, 4)
                        'recalculate this percent on basis of rounded SI
                        ddefault_percent = (dthis_si * 100) / (dband_si - dOblQSHSI)

                        ' Check if FACPropPremiumPerc exists and differs from ThisPerc
                        Dim dFACPropPerc As Double = 0
                        Dim bUseFACPropPerc As Boolean = False
                        Dim sFACPremType As String = If(oFNode.Attributes("FACPremiumType") IsNot Nothing, oFNode.Attributes("FACPremiumType").Value, "")
                        If Not String.IsNullOrEmpty(sFACPremType) AndAlso sFACPremType <> "0" AndAlso sFACPremType.ToUpper() <> "FALSE" Then
                            If oFNode.Attributes("FACPropPremiumPerc") IsNot Nothing Then
                                Double.TryParse(oFNode.Attributes("FACPropPremiumPerc").Value, dFACPropPerc)
                                If dFACPropPerc <> 0 AndAlso dFACPropPerc <> ddefault_percent Then
                                    bUseFACPropPerc = True
                                End If
                            End If
                        End If

                        ' Calculate premium based on FACPropPremiumPerc if set, otherwise use ThisPerc
                        If bUseFACPropPerc Then
                            dthis_premium = (dband_premium - dOblQSHPremium) * dFACPropPerc / 100
                        Else
                            dthis_premium = (dband_premium - dOblQSHPremium) * ddefault_percent / 100
                            ' Set FACPropPremiumPerc same as ThisPerc if not manually set
                            If oFNode.Attributes("FACPropPremiumPerc") IsNot Nothing Then
                                oFNode.Attributes("FACPropPremiumPerc").Value = ddefault_percent
                            End If
                        End If

                        'get total FAC SI and premium
                        dfac_si = (dfac_si) + dthis_si
                        dfac_premium = (dfac_premium) + dthis_premium
                        dFACTotalPremium = dFACTotalPremium + dthis_premium
                        dRIArrangementSI = dRIArrangementSI - dthis_si
                        dFSI += dthis_si
                        dFPremium += dthis_premium
                        'set back the Values in xml
                        oFNode.Attributes("NumberOfLines").Value = "1"
                        oFNode.Attributes("SumInsured").Value = dthis_si
                        oFNode.Attributes("Premium").Value = dthis_premium
                        oFNode.Attributes("Tax").Value = (dfac_taxperc * dthis_premium) / 100
                        oFNode.Attributes("Commission").Value = (dthis_premium * dfac_Commperc) / 100
                        oFNode.Attributes("CommissionTax").Value = (Convert.ToDouble(oFNode.Attributes("Commission").Value) * dfac_CommTaxperc) / 100
                    End If
                End If
            Next

            ' Update the Net totals
            dgrossnet_SI = dgrossnet_SI - dFSI
            dgrossnet_premium = dgrossnet_premium - dFPremium

            'recalculate the Running premium and Running SI
            drunning_premium = drunning_premium - dFPremium
            drunning_si = drunning_si - dFSI


            ' Calculate 'FX' Type
            Dim oFXNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='FX']")
            For Each oFXNode As XmlNode In oFXNodes
                'Check whether the node is live or not. if it is live then process the node
                If Convert.ToString(oFXNode.Attributes("IsDeleted").Value) <> "True" Then
                    'set the presence of FACProp so the insertion could take place in right place
                    bFACXOLPresent = True
                    Dim dSILimit As Decimal = 0
                    'get the MAX RI Line id
                    If Convert.ToInt32(oFXNode.Attributes("RIArrangementLineKey").Value) > iMAXFXRIID Then
                        iMAXFXRIID = Convert.ToInt32(oFXNode.Attributes("RIArrangementLineKey").Value)
                    End If
                    'get the limits from XML
                    dlower_limit = Convert.ToDouble(oFXNode.Attributes("LowerLimit").Value)
                    dline_limit = Convert.ToDouble(oFXNode.Attributes("LineLimit").Value)
                    dthis_si = Convert.ToDouble(oFXNode.Attributes("SumInsured").Value)
                    'add this condition to check the limits'
                    'the fac xol limit check is not applied in the proc :spu_ri_arrangemnt_calc_ri2007
                    dSILimit = dband_si - dOblQSHSI
                    If (dSILimit) < dline_limit Then
                        If (dSILimit) > dlower_limit Then
                            dthis_si = Math.Max(0, dSILimit - (dlower_limit * inegative_si))
                        Else
                            dthis_si = 0
                        End If
                    Else
                        dthis_si = Math.Max(0, (dline_limit - dlower_limit) * inegative_si)
                    End If
                    'set back the Values in xml
                    oFXNode.Attributes("SumInsured").Value = dthis_si
                    dthis_premium = Convert.ToDouble(oFXNode.Attributes("Premium").Value)
                    Dim premium = Convert.ToDouble(oFXNode.Attributes("Premium").Value)
                    Dim tax = Convert.ToDouble(oFXNode.Attributes("Tax").Value)
                    dfac_taxperc = If(premium = 0 OrElse tax = 0, 0, (tax * 100) / premium)

                    dfac_Commperc = Convert.ToDouble(oFXNode.Attributes("CommissionPerc").Value)
                    oFXNode.Attributes("CommissionPerc").Value = dfac_Commperc.ToString("0.0000")

                    Dim commission = Convert.ToDouble(oFXNode.Attributes("Commission").Value)
                    Dim commTax = Convert.ToDouble(oFXNode.Attributes("CommissionTax").Value)
                    dfac_CommTaxperc = If(commission = 0 OrElse commTax = 0, 0, (commTax * 100) / commission)

                    'get total FAC SI and premium
                    dfac_si = (dfac_si) + dthis_si
                    dfac_premium = (dfac_premium) + dthis_premium
                    dFACTotalPremium = dFACTotalPremium + dthis_premium
                    dRIArrangementSI = dRIArrangementSI - dthis_si
                    dFXSI += dthis_si
                    dFXPremium += dthis_premium
                    'set back the Values in xml
                    oFXNode.Attributes("NumberOfLines").Value = "1"
                End If
            Next

            ' Update the Net totals
            dgrossnet_SI = dgrossnet_SI - dFXSI
            dgrossnet_premium = dgrossnet_premium - dFXPremium

            'recalculate the Running premium and Running SI
            drunning_premium = drunning_premium - dFXPremium
            drunning_si = drunning_si - dFXSI

            'Process Gross net line
            Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
            If oFNodes.Count <> 0 OrElse oFXNodes.Count <> 0 OrElse (bFoundTreatyQSH) Then
                ' Add a new line in xml for Gross Net of FAC
                If oNetFACNode Is Nothing Then
                    Dim oDocElement As XmlElement = oXMLDoc.DocumentElement
                    Dim RIBAND As XmlNode = oDocElement.SelectSingleNode("//*[@Name='" & sRIBANDID & "']")
                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
                    ArrangementRow.SetAttribute("IsNew", True)
                    ArrangementRow.SetAttribute("IsEdited", False)
                    ArrangementRow.SetAttribute("IsEditedDB", False)
                    ArrangementRow.SetAttribute("IsDeleted", False)
                    ArrangementRow.SetAttribute("Type", "")
                    ArrangementRow.SetAttribute("TreatyCode", "")
                    ArrangementRow.SetAttribute("RIArrangementLineKey", "")
                    ArrangementRow.SetAttribute("RIarrangementKey", sRIarrangementKey)
                    ArrangementRow.SetAttribute("Retained", "")
                    ArrangementRow.SetAttribute("ReinsuranceTypeCode", "")
                    ArrangementRow.SetAttribute("Priority", "")
                    ArrangementRow.SetAttribute("PremiumPercent", "")
                    ArrangementRow.SetAttribute("PartyKey", "")
                    ArrangementRow.SetAttribute("ParticipationPercent", "")
                    ArrangementRow.SetAttribute("NumberOfLines", "")
                    ArrangementRow.SetAttribute("LineLimit", "")
                    ArrangementRow.SetAttribute("LowerLimit", "")
                    ArrangementRow.SetAttribute("IsRIBroker", "")
                    ArrangementRow.SetAttribute("IsDomiciledForTax", "")
                    ArrangementRow.SetAttribute("IsCommissionModified", "")
                    ArrangementRow.SetAttribute("Grouping", "")
                    ArrangementRow.SetAttribute("CedePremiumOnly", "")
                    ArrangementRow.SetAttribute("ActionType", "")
                    ArrangementRow.SetAttribute("Placement", "GROSS NET")
                    ArrangementRow.SetAttribute("Name", "Net of FAC")
                    ArrangementRow.SetAttribute("DefaultPerc", "")
                    ArrangementRow.SetAttribute("ThisPerc", "")
                    ArrangementRow.SetAttribute("SumInsured", drunning_si)
                    ArrangementRow.SetAttribute("Premium", drunning_premium)
                    ArrangementRow.SetAttribute("Tax", "")
                    ArrangementRow.SetAttribute("CommissionPerc", "")
                    ArrangementRow.SetAttribute("Commission", "")
                    ArrangementRow.SetAttribute("CommissionTax", "")
                    ArrangementRow.SetAttribute("Agreement", "")

                    If iMAXFXRIID > iMAXFRIID AndAlso iMAXFXRIID > iMAXTRIID Then
                        oRootNode.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFXRIID & "']"))
                    ElseIf iMAXFRIID > iMAXTRIID Then
                        oRootNode.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXFRIID & "']"))
                    ElseIf iMAXTRIID <> 0 Then
                        oRootNode.InsertAfter(ArrangementRow, oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iMAXTRIID & "']"))
                    End If
                Else
                    If bFACXOLPresent = False AndAlso bFACPropPresent = False AndAlso bFoundTreatyQSH = False Then
                        oRootNode.RemoveChild(oNetFACNode)
                    Else
                        oNetFACNode.Attributes("SumInsured").Value = drunning_si
                        oNetFACNode.Attributes("Premium").Value = drunning_premium
                        oNetFACNode.Attributes("IsDeleted").Value = False
                    End If
                End If
            ElseIf oFNodes.Count = 0 AndAlso oFXNodes.Count = 0 AndAlso (bFoundTreatyQSH = False) AndAlso oNetFACNode IsNot Nothing Then
                oRootNode.RemoveChild(oNetFACNode)
            End If

            Dim iTreatyTypeID As Integer = 0
            Dim nPriority As Integer = 0
            Dim sTreatyType As String = String.Empty

            Dim dPT_taxperc As Double = 0
            Dim dPT_Commperc As Double = 0
            Dim dPT_CommTaxperc As Double = 0
            Dim dTotalSIAllocated As Double
            Dim dTotalPremiumAllocated As Double

            Dim dTotalAllocated As Double ' Hold the total allocated value to Treaty XOL and Treaty CAT
            Dim dNPTPremium As Double = 0
            Dim dNPT_taxperc As Double = 0
            Dim dNPT_Commperc As Double = 0
            Dim dNPT_CommTaxperc As Double = 0

            oPriorityNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Priority > 0 and @Type!='F' and @Type!='FX' and @IsDeleted!='True']")
            'Dim sortedPriorityNodes = oPriorityNodes.Cast(Of XmlNode)().OrderBy(Function(node) Convert.ToInt32(node.Attributes("Priority").Value)).ThenBy(Function(node) If(node.Attributes("TreatyTypeID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(node.Attributes("TreatyTypeID").Value), Convert.ToDouble(node.Attributes("TreatyTypeID").Value), 0)).ToList()

            ' Ensure all priority nodes have TreatyTypeID for sorting (derive from Type if missing)
            For Each oPNode As XmlNode In oPriorityNodes
                If oPNode.Attributes("TreatyTypeID") Is Nothing OrElse String.IsNullOrEmpty(oPNode.Attributes("TreatyTypeID").Value) Then
                    Dim sNodeType As String = If(oPNode.Attributes("Type") IsNot Nothing, oPNode.Attributes("Type").Value, "")
                    Dim attrTTID As XmlAttribute = oXMLDoc.CreateAttribute("TreatyTypeID")
                    If sNodeType = "TX" OrElse sNodeType = "FX" OrElse sNodeType = "PX" Then
                        attrTTID.Value = "2"
                    Else
                        attrTTID.Value = "1"
                    End If
                    oPNode.Attributes.Append(attrTTID)
                End If
            Next

            Dim nodesWithTreatyType = oPriorityNodes.Cast(Of XmlNode)().Where(Function(node) node.Attributes("TreatyTypeID") IsNot Nothing AndAlso Not String.IsNullOrEmpty(node.Attributes("TreatyTypeID").Value) AndAlso (node.Attributes("IsDeleted") Is Nothing OrElse node.Attributes("IsDeleted").Value <> "True"))
            Dim PriorityNodes = nodesWithTreatyType.OrderBy(Function(node) Convert.ToInt32(node.Attributes("Priority").Value)).ThenBy(Function(node) Convert.ToDouble(node.Attributes("TreatyTypeID").Value)).ThenByDescending(Function(node) Convert.ToDouble(node.Attributes("LineLimit").Value)).ThenBy(Function(node) If(node.Attributes("ReinsuranceTypeCode") IsNot Nothing AndAlso node.Attributes("ReinsuranceTypeCode").Value = "QSR", 1, 0))

            Dim sortedPriorityNodes = PriorityNodes.ToList()

            For Each oTreatyNode As XmlNode In sortedPriorityNodes
                Dim dThisIterationSI As Double = 0
                Dim dThisIterationPremium As Double = 0
                iTreatyTypeID = oTreatyNode.Attributes("TreatyTypeID").Value
                sTreatyType = oTreatyNode.Attributes("Type").Value

                'Skip QSR nodes - they will be handled by ApplyQSRSplit as part of the R node
                Dim sReinsTypeCode As String = If(oTreatyNode.Attributes("ReinsuranceTypeCode") IsNot Nothing, oTreatyNode.Attributes("ReinsuranceTypeCode").Value, "")
                If sReinsTypeCode = "QSR" Then Continue For

                'Skip obligatory T-type nodes - already processed in Step 1 by CalculateFACQSH
                Dim bIsObligatoryNode As Boolean = False
                Dim bIsManuallyAddedNode As Boolean = False
                If oTreatyNode.Attributes("IsObligatory") IsNot Nothing Then Boolean.TryParse(oTreatyNode.Attributes("IsObligatory").Value, bIsObligatoryNode)
                If oTreatyNode.Attributes("ManuallyAdded") IsNot Nothing Then Boolean.TryParse(oTreatyNode.Attributes("ManuallyAdded").Value, bIsManuallyAddedNode)
                If bIsObligatoryNode AndAlso Not bIsManuallyAddedNode AndAlso sTreatyType = "T" Then Continue For

                'Check if manually added first
                Dim bIsManuallyAdded As Boolean = False
                If oTreatyNode.Attributes("ManuallyAdded") IsNot Nothing Then
                    Boolean.TryParse(oTreatyNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
                End If

                'Check if row has been manually edited
                Dim bIsEditedDB As Boolean = False
                If oTreatyNode.Attributes("IsEditedDB") IsNot Nothing Then
                    Boolean.TryParse(oTreatyNode.Attributes("IsEditedDB").Value, bIsEditedDB)
                End If


                If bIsEditedDB And Not bIsManuallyAdded Then
                    'Row is edited � preserve the user-entered SI and recalculate premium from it.
                    'On NB: user typed a new SI value which was written to the node by UpdateRISumInsured;
                    'we must honour it instead of recalculating from DefaultPerc.
                    'On MTA/Renewal: preserve all persisted values as-is.
                    If oTreatyNode.Attributes("SumInsured") Is Nothing OrElse oTreatyNode.Attributes("Premium") Is Nothing Then
                        Throw New InvalidOperationException(String.Format("RI arrangement node (key={0}) is missing required 'SumInsured' or 'Premium' attribute. Cannot preserve persisted values.", If(oTreatyNode.Attributes("RIArrangementLineKey") IsNot Nothing, oTreatyNode.Attributes("RIArrangementLineKey").Value, "unknown")))
                    End If
                    Dim dPersistedSI As Double
                    Dim dPersistedPremium As Double
                    Double.TryParse(oTreatyNode.Attributes("SumInsured").Value, dPersistedSI)
                    Double.TryParse(oTreatyNode.Attributes("Premium").Value, dPersistedPremium)

                    If sTransType = "NB" Then
                        ' On NB recalculate premium proportionally from the user-entered SI,
                        ' UNLESS the user directly edited the premium (IsPremiumEdited=True),
                        ' in which case honour the persisted premium as-is (same as MTA/Renewal).
                        Dim bIsPremiumEdited As Boolean = False
                        If oTreatyNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                            Boolean.TryParse(oTreatyNode.Attributes("IsPremiumEdited").Value, bIsPremiumEdited)
                        End If
                        If Not bIsPremiumEdited Then
                            Dim bIsObligatoryRow As Boolean = False
                            If oTreatyNode.Attributes("IsObligatory") IsNot Nothing Then
                                Boolean.TryParse(oTreatyNode.Attributes("IsObligatory").Value, bIsObligatoryRow)
                            End If
                            If bIsObligatoryRow Then
                                dPersistedPremium = If(dband_si <> 0, Math.Round(dPersistedSI / dband_si * dband_premium, 2), 0)
                            Else
                                dPersistedPremium = If(dgrossnet_SI <> 0, Math.Round(dPersistedSI / dgrossnet_SI * dgrossnet_premium, 2), 0)
                            End If
                            oTreatyNode.Attributes("Premium").Value = dPersistedPremium.ToString(System.Globalization.CultureInfo.InvariantCulture)
                        End If
                    End If

                    Dim dCommPerc As Double
                    Double.TryParse(oTreatyNode.Attributes("CommissionPerc").Value, dCommPerc)

                    ' Derive CommissionTax rate BEFORE overwriting Commission
                    Dim dOldCommForTax As Double = CDbl(oTreatyNode.Attributes("Commission").Value)
                    Dim dCommTaxPerc As Double = If(dOldCommForTax = 0 OrElse CDbl(oTreatyNode.Attributes("CommissionTax").Value) = 0, 0, CDbl(oTreatyNode.Attributes("CommissionTax").Value) * 100 / dOldCommForTax)

                    oTreatyNode.Attributes("Commission").Value = Format(dPersistedPremium * dCommPerc / 100, "0.00")

                    Dim dTaxPerc As Double = If(CDbl(oTreatyNode.Attributes("Premium").Value) = 0 OrElse CDbl(oTreatyNode.Attributes("Tax").Value) = 0, 0, CDbl(oTreatyNode.Attributes("Tax").Value) * 100 / CDbl(oTreatyNode.Attributes("Premium").Value))
                    oTreatyNode.Attributes("Tax").Value = Format((dTaxPerc * dPersistedPremium) / 100, "0.00")

                    oTreatyNode.Attributes("CommissionTax").Value = Format((Convert.ToDouble(oTreatyNode.Attributes("Commission").Value) * dCommTaxPerc) / 100, "0.00")

                    drunning_si = drunning_si - dPersistedSI
                    drunning_premium = drunning_premium - dPersistedPremium
                    Continue For
                End If

                If iTreatyTypeID = TreatyTypeID.Proportional Then
                    Dim dPersistedSI_manual As Double
                    Dim dPersistedPremium_manual As Double
                    Dim dPersistedCommPerc_manual As Double
                    bIsObligatory = If(bIsManuallyAdded, False, Convert.ToBoolean(oTreatyNode.Attributes("IsObligatory").Value))
                    bIsPortfolioTransferred = If(bIsManuallyAdded, False, Convert.ToBoolean(oTreatyNode.Attributes("IsPortfolioTransferred").Value))

                    If bIsObligatory = False Then
                        dline_limit = Convert.ToDouble(oTreatyNode.Attributes("LineLimit").Value)
                        inumber_of_lines = Convert.ToDecimal(oTreatyNode.Attributes("NumberOfLines").Value)
                        nPriority = Convert.ToInt32(oTreatyNode.Attributes("Priority").Value)

                        If bIsManuallyAdded Then
                            'Manually added: keep DefaultPerc as 0, calculate SI and Premium from ThisPerc against running base
                            Double.TryParse(oTreatyNode.Attributes("SumInsured").Value, dPersistedSI_manual)
                            Dim dManualThisPercNode As Double = 0
                            Double.TryParse(oTreatyNode.Attributes("ThisPerc").Value, dManualThisPercNode)
                            dManualThisPerc = If(dgrossnet_SI = 0, 0, dPersistedSI_manual / dgrossnet_SI)
                            ' Recalculate premium from ThisPerc against the current running premium base
                            Double.TryParse(oTreatyNode.Attributes("Premium").Value, dPersistedPremium_manual)
                            'dPersistedPremium_manual = If(dgrossnet_premium = 0, 0, dgrossnet_premium * dManualThisPercNode / 100)
                            If Double.IsNaN(dthis_premium) Then dthis_premium = 0
                            ddefault_percent = 0
                        Else
                            ddefault_percent = Convert.ToDouble(oTreatyNode.Attributes("DefaultPerc").Value)
                        End If
                        oTreatyNode.Attributes("DefaultPerc").Value = Format(ddefault_percent, "0.00")

                        UpdatePriorityValues(ilast_priority, nPriority, dline_limit, inumber_of_lines, drunning_si, drunning_premium, dgrossnet_SI, dgrossnet_premium, bIsExtendedLimitApplied, dExtendedLimitAmount, dpriority_limit, dpriority_si, dpriority_premium, QSTotal)
                        Dim PropTreatyResult = CalculateProportionalTreaty(sTreatyType, iTreatyTypeID, dpriority_si, dpriority_limit, ddefault_percent, dgrossnet_SI, dgrossnet_premium, dband_si, dPriority_allocated_SI, dPriority_allocated_Premium, QSTotal, inumber_of_lines)

                        If bIsManuallyAdded Then
                            'Keep user-entered SI and Premium; DefaultPerc is 0 so priority_share is 0
                            dPriority_allocated_SI = PropTreatyResult.Item3
                            dPriority_allocated_Premium = PropTreatyResult.Item4
                            ' dpriority_share = PropTreatyResult.Item5
                            QSTotal = PropTreatyResult.Item6
                        Else
                            dthis_si = PropTreatyResult.Item1
                            dthis_premium = PropTreatyResult.Item2
                            dPriority_allocated_SI = PropTreatyResult.Item3
                            dPriority_allocated_Premium = PropTreatyResult.Item4
                            dpriority_share = PropTreatyResult.Item5
                            QSTotal = PropTreatyResult.Item6
                        End If

                        If bIsManuallyAdded Then
                            ' dthis_si/dthis_premium are shared variables potentially overwritten by F-type loop;
                            ' re-read directly from the node to avoid clobbering user-entered values
                            Double.TryParse(oTreatyNode.Attributes("SumInsured").Value, dPersistedSI_manual)
                            Double.TryParse(oTreatyNode.Attributes("Premium").Value, dPersistedPremium_manual)
                            'For manually added: preserve the user-entered SI and Premium from the node
                            oTreatyNode.Attributes("ThisPerc").Value = If(Double.IsNaN(dManualThisPerc), "0.0000", (dManualThisPerc * 100).ToString())
                            Double.TryParse(oTreatyNode.Attributes("CommissionPerc").Value, dPersistedCommPerc_manual)
                            dthis_si = If(Double.IsNaN(dPersistedSI_manual), 0, dPersistedSI_manual)
                            dthis_premium = If(Double.IsNaN(dPersistedPremium_manual), 0, dPersistedPremium_manual)
                            dPT_Commperc = Convert.ToDouble(oTreatyNode.Attributes("CommissionPerc").Value)
                        Else
                            oTreatyNode.Attributes("ThisPerc").Value = If(Double.IsNaN(dpriority_share), "0.0000", (dpriority_share * 100).ToString())
                        End If
                        oTreatyNode.Attributes("SumInsured").Value = If(Double.IsNaN(dthis_si), "0.00", dthis_si.ToString())

                        ' Derive tax rate from OLD premium/tax before overwriting Premium
                        dPT_taxperc = If(CDbl(oTreatyNode.Attributes("Premium").Value) = 0 OrElse CDbl(oTreatyNode.Attributes("Tax").Value) = 0, 0, CDbl(oTreatyNode.Attributes("Tax").Value) * 100 / CDbl(oTreatyNode.Attributes("Premium").Value))
                        ' Derive commission tax rate from OLD commission/commissionTax before overwriting Commission
                        ' Must be read here before Commission is recalculated, using CommissionTaxPerc if available
                        Dim dOldCommission As Double = CDbl(oTreatyNode.Attributes("Commission").Value)
                        Dim dOldCommTax As Double = CDbl(oTreatyNode.Attributes("CommissionTax").Value)
                        dPT_CommTaxperc = If(oTreatyNode.Attributes("CommissionTaxPerc") IsNot Nothing AndAlso CDbl(oTreatyNode.Attributes("CommissionTaxPerc").Value) > 0,
                                             CDbl(oTreatyNode.Attributes("CommissionTaxPerc").Value),
                                             If(dOldCommission = 0 OrElse dOldCommTax = 0, 0, dOldCommTax * 100 / dOldCommission))
                        oTreatyNode.Attributes("Premium").Value = If(Double.IsNaN(dthis_premium), "0.0000", dthis_premium.ToString())
                        dPT_Commperc = Convert.ToDouble(oTreatyNode.Attributes("CommissionPerc").Value)
                        oTreatyNode.Attributes("CommissionPerc").Value = Format(dPT_Commperc, "0.0000")
                        oTreatyNode.Attributes("Commission").Value = (dthis_premium * dPT_Commperc) / 100
                        oTreatyNode.Attributes("CommissionTax").Value = (Convert.ToDouble(oTreatyNode.Attributes("Commission").Value) * dPT_CommTaxperc) / 100
                        oTreatyNode.Attributes("Tax").Value = (dPT_taxperc * dthis_premium) / 100

                        dThisIterationSI += dthis_si
                        dThisIterationPremium += dthis_premium
                    End If
                ElseIf iTreatyTypeID = TreatyTypeID.NonProportional Then
                    Dim dPersistedPremium_manual As Double
                    Dim dPersistedCommPerc_manual As Double
                    bIsPortfolioTransferred = If(bIsManuallyAdded, False, Convert.ToBoolean(oTreatyNode.Attributes("IsPortfolioTransferred").Value))
                    dline_limit = Convert.ToDouble(oTreatyNode.Attributes("LineLimit").Value)
                    dlower_limit = Convert.ToDouble(oTreatyNode.Attributes("LowerLimit").Value)
                    icede_premium_only = If(bIsManuallyAdded, 0, Convert.ToInt32(oTreatyNode.Attributes("CedePremiumOnly").Value))
                    dceding_rate = Convert.ToDouble(oTreatyNode.Attributes("DefaultPerc").Value)
                    nPriority = Convert.ToInt32(oTreatyNode.Attributes("Priority").Value)
                    inumber_of_lines = Convert.ToDecimal(oTreatyNode.Attributes("NumberOfLines").Value)

                    UpdatePriorityValues(ilast_priority, nPriority, dline_limit, inumber_of_lines, drunning_si, drunning_premium, dgrossnet_SI, dgrossnet_premium, bIsExtendedLimitApplied, dExtendedLimitAmount, dpriority_limit, dpriority_si, dpriority_premium, QSTotal)

                    Dim NPropTreatyResult = CalculateNonProportionalTreaty(iTreatyTypeID, icede_premium_only, sTreatyType, dpriority_limit, dline_limit, QSTotal, dlower_limit, inegative_si, dpriority_si, dpriority_premium, dceding_rate, bIsPortfolioTransferred)
                    dthis_si = NPropTreatyResult.Item1

                    If bIsManuallyAdded Then
                        'Manually added TX: use calculated SI from limits if limits are set,
                        'otherwise keep the persisted SumInsured from the node
                        If dline_limit > 0 Then
                            dthis_si = NPropTreatyResult.Item1
                        Else
                            Double.TryParse(oTreatyNode.Attributes("SumInsured").Value, dthis_si)
                        End If
                        Double.TryParse(oTreatyNode.Attributes("Premium").Value, dPersistedPremium_manual)
                        Double.TryParse(oTreatyNode.Attributes("CommissionPerc").Value, dPersistedCommPerc_manual)
                        ' If SumInsured is 0 for a manually added XOL treaty, premium must also be 0
                        If Math.Round(dthis_si, 2) = 0 Then dPersistedPremium_manual = 0
                        dthis_premium = If(Double.IsNaN(dPersistedPremium_manual), 0, dPersistedPremium_manual)
                        dNPT_Commperc = If(Double.IsNaN(dPersistedCommPerc_manual), 0, dPersistedCommPerc_manual)
                    Else
                        dthis_premium = NPropTreatyResult.Item2
                        If icede_premium_only = 1 Then dthis_si = 0
                    End If

                    oTreatyNode.Attributes("DefaultPerc").Value = Format(dceding_rate, "0.0000")
                    oTreatyNode.Attributes("SumInsured").Value = If(Double.IsNaN(dthis_si), "0.00", dthis_si.ToString())
                    If Not Double.IsNaN(dthis_si) Then dTotalAllocated += dthis_si

                    If bIsPortfolioRIAmendment = False OrElse bIsManuallyAdded Then
                        dNPT_taxperc = If(CDbl(oTreatyNode.Attributes("Premium").Value) = 0 OrElse CDbl(oTreatyNode.Attributes("Tax").Value) = 0, 0, CDbl(oTreatyNode.Attributes("Tax").Value) * 100 / CDbl(oTreatyNode.Attributes("Premium").Value))
                        oTreatyNode.Attributes("Premium").Value = If(Double.IsNaN(dthis_premium), "0.00", dthis_premium.ToString())
                        dNPT_Commperc = Convert.ToDouble(oTreatyNode.Attributes("CommissionPerc").Value)
                        oTreatyNode.Attributes("CommissionPerc").Value = Format(dNPT_Commperc, "0.0000")
                        dNPT_CommTaxperc = If(oTreatyNode.Attributes("CommissionTaxPerc") IsNot Nothing AndAlso CDbl(oTreatyNode.Attributes("CommissionTaxPerc").Value) > 0,
                                              CDbl(oTreatyNode.Attributes("CommissionTaxPerc").Value),
                                              If(CDbl(oTreatyNode.Attributes("Commission").Value) = 0 OrElse CDbl(oTreatyNode.Attributes("CommissionTax").Value) = 0, 0, CDbl(oTreatyNode.Attributes("CommissionTax").Value) * 100 / CDbl(oTreatyNode.Attributes("Commission").Value)))
                        oTreatyNode.Attributes("Tax").Value = (dNPT_taxperc * dthis_premium) / 100
                        oTreatyNode.Attributes("Commission").Value = (dthis_premium * dNPT_Commperc) / 100
                        oTreatyNode.Attributes("CommissionTax").Value = (Convert.ToDouble(oTreatyNode.Attributes("Commission").Value) * dNPT_CommTaxperc) / 100
                        dTotalPremiumAllocated += dthis_premium
                    End If
                    dThisIterationSI += dthis_si
                    dThisIterationPremium += dthis_premium
                End If

                drunning_si = (drunning_si - dThisIterationSI)
                drunning_premium = (drunning_premium - dThisIterationPremium)

            Next

            Dim dTotalUnallocatedSI As Double
            Dim dTotalUnallocatedPremium As Double

            dTotalUnallocatedSI = drunning_si
            drunning_premium = dGrossPremium - SumOfNonRetPremiums(oXMLDoc, sRIBANDID)

            'IF RETAINED NODE EXISTS AND THERE IS REMAINING AMOUNT IN PREMIUM ASSIGN IT TO R TYPE
            ' Add remaining amounts to 'R' type if drunning_premium
            If drunning_premium <> 0 Then
                Dim oRNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='R']")
                If oRNodes IsNot Nothing AndAlso oRNodes.Count > 0 Then
                    For Each oRNode As XmlNode In oRNodes
                        Dim bRIsEdited As Boolean = False
                        If oRNode.Attributes("IsEditedDB") IsNot Nothing Then
                            Boolean.TryParse(oRNode.Attributes("IsEditedDB").Value, bRIsEdited)
                        End If
                        If Not bRIsEdited Then
                            Dim dOldRetPremium As Double = Convert.ToDouble(oRNode.Attributes("Premium").Value)
                            Dim dNewRetPremium As Double = drunning_premium
                            oRNode.Attributes("Premium").Value = dNewRetPremium
                            ' Recalculate Tax and CommissionTax proportionally for the new premium
                            If dOldRetPremium <> 0 Then
                                Dim dRetTaxPerc As Double = If(CDbl(oRNode.Attributes("Tax").Value) = 0, 0, CDbl(oRNode.Attributes("Tax").Value) * 100 / dOldRetPremium)
                                oRNode.Attributes("Tax").Value = Format((dRetTaxPerc * dNewRetPremium) / 100, "0.00")
                                Dim dRetCommPerc As Double = 0
                                Double.TryParse(oRNode.Attributes("CommissionPerc").Value, dRetCommPerc)
                                Dim dRetOldComm As Double = Convert.ToDouble(oRNode.Attributes("Commission").Value)
                                Dim dRetComm As Double = (dNewRetPremium * dRetCommPerc) / 100
                                oRNode.Attributes("Commission").Value = Format(dRetComm, "0.00")
                                Dim dRetNewTax As Double = CDbl(oRNode.Attributes("Tax").Value)
                                Dim dRetCommTaxPerc As Double = If(dNewRetPremium = 0, 0, dRetNewTax * 100 / dNewRetPremium)
                                oRNode.Attributes("CommissionTax").Value = Format((dRetComm * dRetCommTaxPerc) / 100, "0.00")
                            End If
                        End If
                    Next
                    ' Subtract edited QSR premium from R node (already accounted in running premium)
                    Dim oEditedQSRNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ReinsuranceTypeCode='QSR' and @IsDeleted!='True' and @IsEdited='True']")
                    If oEditedQSRNodes IsNot Nothing AndAlso oEditedQSRNodes.Count > 0 Then
                        For Each oRNode3 As XmlNode In oRNodes
                            Dim dEditedQSRTotal As Double = 0
                            For Each oEQSR As XmlNode In oEditedQSRNodes
                                Dim dEQSRP As Double = 0
                                If oEQSR.Attributes("Premium") IsNot Nothing Then Double.TryParse(oEQSR.Attributes("Premium").Value, dEQSRP)
                                dEditedQSRTotal += dEQSRP
                            Next
                            Dim dAdjRetPrem As Double = Convert.ToDouble(oRNode3.Attributes("Premium").Value) - dEditedQSRTotal
                            oRNode3.Attributes("Premium").Value = dAdjRetPrem
                        Next
                    End If

                    ' Apply QSR split on adjusted R premium if QSR nodes exist
                    Dim oQSRNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ReinsuranceTypeCode='QSR' and @IsDeleted!='True' and @IsEdited!='True']")
                    If oQSRNodes IsNot Nothing AndAlso oQSRNodes.Count > 0 Then
                        For Each oRNode2 As XmlNode In oRNodes
                            Dim dRetSI2 As Double = 0
                            Double.TryParse(oRNode2.Attributes("SumInsured").Value, dRetSI2)
                            Dim dRetPrem2 As Double = 0
                            Double.TryParse(oRNode2.Attributes("Premium").Value, dRetPrem2)
                            ApplyQSRSplit(oXMLDoc, sRIBANDID, oRNode2, dRetSI2, dRetPrem2)
                            ' Reset IsEditedDB on R node since SI was recalculated by QSR split
                            If oRNode2.Attributes("IsEditedDB") IsNot Nothing Then
                                oRNode2.Attributes("IsEditedDB").Value = "False"
                            End If
                        Next
                    End If
                    dTotalUnallocatedPremium = 0
                Else
                    dTotalUnallocatedPremium = drunning_premium
                End If
            Else
                dTotalUnallocatedPremium = drunning_premium
            End If
            dTotalSIAllocated = dGrossSI - dTotalUnallocatedSI
            dTotalPremiumAllocated = dGrossPremium - dTotalUnallocatedPremium
            'get the allocated amount
            Dim oAllocNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Allocated']")
            If oAllocNode IsNot Nothing Then
                If dTotalSIAllocated > dGrossSI Then
                    If dGrossSI.ToString = Double.NaN.ToString Then
                        oAllocNode.Attributes("SumInsured").Value = "0.00"
                        dGrossSI = 0
                    Else
                        oAllocNode.Attributes("SumInsured").Value = dGrossSI
                    End If
                    If dGrossPremium.ToString = Double.NaN.ToString Then
                        oAllocNode.Attributes("Premium").Value = "0.00"
                        dGrossPremium = 0
                    Else
                        oAllocNode.Attributes("Premium").Value = dGrossPremium
                    End If
                    oAllocNode.Attributes("IsDeleted").Value = False
                Else
                    If dTotalSIAllocated.ToString = Double.NaN.ToString Then
                        oAllocNode.Attributes("SumInsured").Value = "0.00"
                        dTotalSIAllocated = 0
                    Else
                        oAllocNode.Attributes("SumInsured").Value = dTotalSIAllocated
                    End If
                    If dTotalPremiumAllocated.ToString = Double.NaN.ToString Then
                        oAllocNode.Attributes("Premium").Value = "0.00"
                        dTotalPremiumAllocated = 0
                    Else
                        oAllocNode.Attributes("Premium").Value = dTotalPremiumAllocated
                    End If
                    oAllocNode.Attributes("IsDeleted").Value = False
                End If

            End If

            If bIsCancellation = True AndAlso (dTotalUnallocatedSI <> 0 OrElse dTotalUnallocatedPremium <> 0) Then
                'In case of cancellation, add unallocated amount to retention. During cancellation nothing should go to unallocated
                Dim oExistingRNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='R']")
                'process Retained line
                For Each oRNode As XmlNode In oExistingRNodes
                    If oRNode IsNot Nothing Then
                        oRNode.Attributes("SumInsured").Value = Convert.ToDouble(oRNode.Attributes("SumInsured").Value) + dTotalUnallocatedSI
                        oRNode.Attributes("Premium").Value = Convert.ToDouble(oRNode.Attributes("Premium").Value) + dTotalUnallocatedPremium
                    End If
                Next
                'Remove unallocated node if any
                Dim oUnAllocNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Unallocated']")
                If oUnAllocNode IsNot Nothing Then
                    oRootNode.RemoveChild(oUnAllocNode)
                End If
            Else
                'Logic will remain same as previous
                'process the Unallocated amount
                Dim oUnAllocNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Unallocated']")
                If (Math.Round(dGrossSI, 2) - Math.Round(dTotalSIAllocated, 2)) <> 0 OrElse (Math.Round(dGrossPremium, 2) - Math.Round(dTotalPremiumAllocated, 2)) <> 0 Then
                    If oUnAllocNode IsNot Nothing Then
                        If (dGrossSI - dTotalSIAllocated).ToString = Double.NaN.ToString Then
                            oUnAllocNode.Attributes("SumInsured").Value = "0.00"
                        Else
                            oUnAllocNode.Attributes("SumInsured").Value = (dGrossSI - dTotalSIAllocated)
                        End If
                        If (dGrossPremium - dTotalPremiumAllocated).ToString = Double.NaN.ToString Then
                            oUnAllocNode.Attributes("Premium").Value = "0.00"
                        Else
                            oUnAllocNode.Attributes("Premium").Value = (dGrossPremium - dTotalPremiumAllocated)
                        End If
                        oUnAllocNode.Attributes("IsDeleted").Value = False
                    Else
                        'Add into the XML
                        Dim sArrangementRowUA As String = "ArrangementRow"
                        Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRowUA)
                        Dim oDocElement As XmlElement = oXMLDoc.DocumentElement
                        Dim RIBAND As XmlNode = oDocElement.SelectSingleNode("//*[@Name='" & sRIBANDID & "']")
                        ArrangementRow.SetAttribute("IsNew", True)
                        ArrangementRow.SetAttribute("IsEdited", False)
                        ArrangementRow.SetAttribute("IsEditedDB", False)
                        ArrangementRow.SetAttribute("IsDeleted", False)
                        ArrangementRow.SetAttribute("Type", "")
                        ArrangementRow.SetAttribute("TreatyCode", "")
                        ArrangementRow.SetAttribute("RIArrangementLineKey", "")
                        ArrangementRow.SetAttribute("RIarrangementKey", "")
                        ArrangementRow.SetAttribute("Retained", "")
                        ArrangementRow.SetAttribute("ReinsuranceTypeCode", "")
                        ArrangementRow.SetAttribute("Priority", "")
                        ArrangementRow.SetAttribute("PremiumPercent", "")
                        ArrangementRow.SetAttribute("PartyKey", "")
                        ArrangementRow.SetAttribute("ParticipationPercent", "")
                        ArrangementRow.SetAttribute("NumberOfLines", "")
                        ArrangementRow.SetAttribute("LineLimit", "")
                        ArrangementRow.SetAttribute("LowerLimit", "")
                        ArrangementRow.SetAttribute("IsRIBroker", "")
                        ArrangementRow.SetAttribute("IsDomiciledForTax", "")
                        ArrangementRow.SetAttribute("IsCommissionModified", "")
                        ArrangementRow.SetAttribute("Grouping", "")
                        ArrangementRow.SetAttribute("CedePremiumOnly", "")
                        ArrangementRow.SetAttribute("ActionType", "")
                        ArrangementRow.SetAttribute("Placement", "")
                        ArrangementRow.SetAttribute("DefaultPerc", "")
                        ArrangementRow.SetAttribute("ThisPerc", "")
                        ArrangementRow.SetAttribute("Tax", "")
                        ArrangementRow.SetAttribute("CommissionPerc", "")
                        ArrangementRow.SetAttribute("Commission", "")
                        ArrangementRow.SetAttribute("CommissionTax", "")
                        ArrangementRow.SetAttribute("Agreement", "")
                        ArrangementRow.SetAttribute("Name", "Unallocated")
                        If dGrossSI.ToString = Double.NaN.ToString OrElse dTotalSIAllocated.ToString = Double.NaN.ToString Then
                            ArrangementRow.SetAttribute("SumInsured", "0.00")
                        Else
                            ArrangementRow.SetAttribute("SumInsured", (dGrossSI - dTotalSIAllocated))
                        End If
                        If dGrossPremium.ToString = Double.NaN.ToString OrElse dTotalPremiumAllocated.ToString = Double.NaN.ToString Then
                            ArrangementRow.SetAttribute("Premium", "0.00")
                        Else
                            ArrangementRow.SetAttribute("Premium", (dGrossPremium - dTotalPremiumAllocated))
                        End If
                        'Add it at the last row
                        oRootNode.InsertAfter(ArrangementRow, oRootNode.LastChild)
                    End If
                Else
                    If oUnAllocNode IsNot Nothing Then
                        oRootNode.RemoveChild(oUnAllocNode)
                    End If
                End If
            End If

        End If

        'Now we are saving actual this percent during NB (without rounding it to 4 decimal places)
        'So , during MTA we have to display it by rounding upto 4 DP
        Dim sOriginalRIBandID = sRIBANDID.Replace("Current", "Original")
        Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sOriginalRIBandID & "']/ArrangementRow")

        ' Get Original GrossNet SI (Net of FAC) for recalculating ThisPerc on R/QSR nodes
        Dim dOrigGrossNetSI As Double = 0
        Dim oOrigNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sOriginalRIBandID & "']/ArrangementRow[@Name='Net of FAC']")
        If oOrigNetFACNode IsNot Nothing Then
            Double.TryParse(oOrigNetFACNode.Attributes("SumInsured").Value, dOrigGrossNetSI)
        Else
            Dim oOrigBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sOriginalRIBandID & "']/ArrangementRow[@Name='Band Total']")
            If oOrigBandNode IsNot Nothing Then Double.TryParse(oOrigBandNode.Attributes("SumInsured").Value, dOrigGrossNetSI)
        End If

        For Each oFNode As XmlNode In oNodes
            If Not String.IsNullOrEmpty(oFNode.Attributes("ThisPerc").Value) Then
                ' Recalculate ThisPerc for R and QSR nodes based on their SI / GrossNetSI
                Dim sNodeType As String = If(oFNode.Attributes("Type") IsNot Nothing, oFNode.Attributes("Type").Value, "")
                Dim sReinsCode As String = If(oFNode.Attributes("ReinsuranceTypeCode") IsNot Nothing, oFNode.Attributes("ReinsuranceTypeCode").Value, "")
                If (sNodeType = "R" OrElse sReinsCode = "QSR") AndAlso dOrigGrossNetSI <> 0 Then
                    Dim dNodeSI As Double = 0
                    If oFNode.Attributes("SumInsured") IsNot Nothing Then Double.TryParse(oFNode.Attributes("SumInsured").Value, dNodeSI)
                    dthis_percent = (dNodeSI / dOrigGrossNetSI) * 100
                Else
                    dthis_percent = Convert.ToDouble(oFNode.Attributes("ThisPerc").Value)
                End If
                oFNode.Attributes("ThisPerc").Value = Format(Math.Abs(dthis_percent), "0.0000")
            End If
        Next

        'Update the XML before returning
        Dim swContent As New System.IO.StringWriter
        Dim xmlwContent As New XmlTextWriter(swContent)

        'return the xml
        oXMLDoc.WriteTo(xmlwContent)
        sXML = swContent.ToString()

        xmlwContent.Close()
        swContent.Close()

        'ADD CODE FOR VARIABLE TREATY PREMIUM - Execute AFTER standard recalculation
        If bViewOnly = False Then
            Dim iTreatyPremiumType As Int16 = 0
            ' Look up TreatyPremiumType for the current RI arrangement only
            Dim oTreatyCodeNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIarrangementKey='" & sRIarrangementKey & "' and @TreatyCode!='']")
            If oTreatyCodeNode IsNot Nothing AndAlso oTreatyCodeNode.Attributes("TreatyPremiumType") IsNot Nothing Then
                Int16.TryParse(oTreatyCodeNode.Attributes("TreatyPremiumType").Value, iTreatyPremiumType)
            End If
            If iTreatyPremiumType = 1 Then
                RecalculateForTreatyPremium(sXML, sRIBANDID, sRIarrangementKey)
            End If
        End If

    End Sub
    ''' <summary>
    ''' this will upadte the this percentage amount if percentage amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateThisPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String)
        UpdateThisPercentage(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, "NB")

    End Sub
    ''' <summary>
    ''' this will upadte the this percentage amount if percentage amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateThisPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String)
        UpdateThisPercentage(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sTransType, 0)

    End Sub
    ''' <summary>
    ''' this will upadte the this percentage amount if percentage amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <param name="dTaxPercentage"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateThisPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                    ByVal dTaxPercentage As Double)
        UpdateThisPercentage(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sTransType, dTaxPercentage, False)

    End Sub

    ''' <summary>
    ''' this will upadte the this percentage amount if percentage amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <param name="dTaxPercentage"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateThisPercentage(ByRef sXML As String,
                                  ByVal sValue As Double,
                                  ByVal sRIBANDID As String,
                                  ByVal sRIArrangementLineKey As String,
                                  ByVal sRIarrangementKey As String,
                                  ByVal sTransType As String,
                                  ByVal dTaxPercentage As Double,
                                  ByVal bIsPortfolioRIAmendment As Boolean,
                                  Optional ByVal sOriginalXML As String = "")
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(sXML)
        'Root Elelemnt
        Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']")

        'Find the Node which has been changed
        Dim oChangedNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")

        If oChangedNode IsNot Nothing Then
            'Only set IsEditedDB if value actually differs from original DB XML
            Dim bIsEdited As Boolean = HasValueChangedFromOriginal(sOriginalXML, sRIBANDID, sRIArrangementLineKey, "ThisPerc", sValue.ToString())
            If bIsEdited Then
                If oChangedNode.Attributes("IsEditedDB") Is Nothing Then
                    Dim attrEdited As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
                    attrEdited.Value = "True"
                    oChangedNode.Attributes.Append(attrEdited)
                Else
                    oChangedNode.Attributes("IsEditedDB").Value = "True"
                End If
            End If
            Dim bIsNewNode As Boolean = False
            If oChangedNode.Attributes("IsNew") IsNot Nothing Then Boolean.TryParse(oChangedNode.Attributes("IsNew").Value, bIsNewNode)
            If Not bIsNewNode AndAlso bIsEdited Then
                If oChangedNode.Attributes("ActionType") Is Nothing Then
                    Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                    attrAction.Value = "1"
                    oChangedNode.Attributes.Append(attrAction)
                Else
                    oChangedNode.Attributes("ActionType").Value = "1"
                End If
            End If
            oChangedNode.Attributes("ThisPerc").Value = sValue
        End If
        sXML = oXMLDoc.OuterXml
        Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
    End Sub

    ''' <summary>
    ''' this will upadte the SI amount if SI amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateRISumInsured(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String)

        UpdateRISumInsured(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, "NB")
    End Sub

    ''' <summary>
    ''' this will upadte the SI amount if SI amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateRISumInsured(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String)

        UpdateRISumInsured(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sTransType, 0)
    End Sub

    ''' <summary>
    ''' this will upadte the SI amount if SI amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateRISumInsured(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                    ByVal dTaxPercentage As Double)

        UpdateRISumInsured(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sTransType, dTaxPercentage, False)
    End Sub
    ''' <summary>
    ''' this will upadte the SI amount if SI amount is changed
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <param name="dTaxPercentage"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateRISumInsured(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                    ByVal dTaxPercentage As Double,
                                    ByVal bIsPortfolioRIAmendment As Boolean,
                                    Optional ByVal sTreatyCode As String = "",
                                    Optional ByVal sOriginalXML As String = "")

        Dim oXMLDoc As New XmlDocument
        Dim dbandsi As Double
        Dim dThisPerc As Double
        oXMLDoc.LoadXml(sXML)

        'Root Elelemnt
        Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']")

        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Double.TryParse(oNode.Attributes("SumInsured").Value, dbandsi)
        End If

        Dim dOblQSHSI As Double = 0
        Dim bChangedNodeIsObligatory As Boolean = False
        Dim oOblQSHNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='T']")
        For Each oOblQSHNode As XmlNode In oOblQSHNodes
            If oOblQSHNode.Attributes("IsObligatory").Value = True Then
                dOblQSHSI += oOblQSHNode.Attributes("SumInsured").Value
                ' Check if the changed node is this obligatory node
                If Not String.IsNullOrEmpty(sRIArrangementLineKey) AndAlso sRIArrangementLineKey <> "0" Then
                    If oOblQSHNode.Attributes("RIArrangementLineKey") IsNot Nothing AndAlso oOblQSHNode.Attributes("RIArrangementLineKey").Value.Trim() = sRIArrangementLineKey.Trim() Then
                        bChangedNodeIsObligatory = True
                    End If
                ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                    If oOblQSHNode.Attributes("TreatyCode") IsNot Nothing AndAlso oOblQSHNode.Attributes("TreatyCode").Value.Trim().ToUpper() = sTreatyCode.Trim().ToUpper() Then
                        bChangedNodeIsObligatory = True
                    End If
                End If
            End If
        Next

        ' Obligatory QSH: ThisPerc = SI / GrossSI; non-obligatory: ThisPerc = SI / (GrossSI - OblQSHSI)
        If bChangedNodeIsObligatory Then
            dThisPerc = If(dbandsi = 0, 0, (sValue / dbandsi) * Convert.ToDouble(100))
        Else
            dThisPerc = If((dbandsi - dOblQSHSI) = 0, 0, (sValue / (dbandsi - dOblQSHSI)) * Convert.ToDouble(100))
        End If
        'Find the Node which has been changed
        Dim oChangedNode As XmlNode = Nothing
        If String.IsNullOrEmpty(sRIArrangementLineKey) OrElse sRIArrangementLineKey = "0" Then
            If Not String.IsNullOrEmpty(sTreatyCode) Then
                oChangedNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
            End If
        Else
            oChangedNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        End If

        If oChangedNode IsNot Nothing Then
            If oChangedNode.Attributes("ActionType") IsNot Nothing OrElse True Then
                Dim bIsNewNode As Boolean = False
                If oChangedNode.Attributes("IsNew") IsNot Nothing Then Boolean.TryParse(oChangedNode.Attributes("IsNew").Value, bIsNewNode)
                If Not bIsNewNode Then
                    If oChangedNode.Attributes("ActionType") Is Nothing Then
                        Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                        attrAction.Value = "1"
                        oChangedNode.Attributes.Append(attrAction)
                    Else
                        oChangedNode.Attributes("ActionType").Value = "1"
                    End If
                End If
            End If
            Dim bIsManuallyAdded As Boolean = False
            If oChangedNode.Attributes("ManuallyAdded") IsNot Nothing Then
                Boolean.TryParse(oChangedNode.Attributes("ManuallyAdded").Value, bIsManuallyAdded)
            End If

            If bIsManuallyAdded Then
                oChangedNode.Attributes("SumInsured").Value = sValue.ToString()

                ' Read ThisPerc from the node (set by txtSumInsured_TextChanged before calling here)
                Dim dNodeThisPerc As Double = 0
                If oChangedNode.Attributes("ThisPerc") IsNot Nothing Then
                    Double.TryParse(oChangedNode.Attributes("ThisPerc").Value, dNodeThisPerc)
                End If

                Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
                Dim dNetFACPremium As Double = 0
                If oNetFACNode IsNot Nothing Then
                    Double.TryParse(oNetFACNode.Attributes("Premium").Value, dNetFACPremium)
                End If
                Dim dBandPremium As Double = 0
                If dNetFACPremium = 0 Then
                    Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                    If oBandNode IsNot Nothing Then
                        Double.TryParse(oBandNode.Attributes("Premium").Value, dBandPremium)
                    End If
                End If
                Dim dBasePremium As Double = If(dNetFACPremium = 0, dBandPremium, dNetFACPremium)
                Dim dCalculatedPremium As Double = (dNodeThisPerc / 100) * dBasePremium
                oChangedNode.Attributes("Premium").Value = Math.Round(dCalculatedPremium, 2).ToString()
            End If
            If oChangedNode.Attributes("IsEditedDB") Is Nothing Then
                Dim attrEdited As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
                attrEdited.Value = If(HasValueChangedFromOriginal(sOriginalXML, sRIBANDID, sRIArrangementLineKey, "SumInsured", sValue.ToString(), sTreatyCode), "True", "False")
                oChangedNode.Attributes.Append(attrEdited)
            Else
                oChangedNode.Attributes("IsEditedDB").Value = If(HasValueChangedFromOriginal(sOriginalXML, sRIBANDID, sRIArrangementLineKey, "SumInsured", sValue.ToString(), sTreatyCode), "True", "False")
            End If
            oChangedNode.Attributes("SumInsured").Value = sValue.ToString()
            If Not bIsManuallyAdded Then
                oChangedNode.Attributes("ThisPerc").Value = dThisPerc.ToString()
            End If
            ' SI re-edited: reset IsPremiumEdited so premium is recalculated from the new SI
            If oChangedNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                oChangedNode.Attributes("IsPremiumEdited").Value = "False"
            End If
        End If
        sXML = oXMLDoc.OuterXml
        Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
    End Sub

    ''' <summary>
    ''' this will update the edited commission perc and will recalculate the comm
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateComissionPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    Optional ByVal sTreatyCode As String = "")

        UpdateComissionPercentage(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, "NB", 0, False, sTreatyCode)
    End Sub

    ''' <summary>
    ''' this will update the edited commission perc and will recalculate the comm
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateComissionPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                    Optional ByVal sTreatyCode As String = "")

        UpdateComissionPercentage(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sTransType, 0, False, sTreatyCode)
    End Sub

    ''' <summary>
    ''' this will update the edited commission perc and will recalculate the comm
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    '''  <param name="sTransType"></param>
    ''' <param name="dTaxPercentage"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateComissionPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                   ByVal dTaxPercentage As Double)

        UpdateComissionPercentage(sXML, sValue, sRIBANDID, sRIArrangementLineKey, sRIarrangementKey, sTransType, dTaxPercentage, False)
    End Sub
    ''' <summary>
    ''' this will update the edited commission perc and will recalculate the comm
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <param name="dTaxPercentage"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateComissionPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                    ByVal dTaxPercentage As Double,
                                    ByVal bIsPortfolioRIAmendment As Boolean,
                                    Optional ByVal sTreatyCode As String = "",
                                    Optional ByVal sOriginalXML As String = "")
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(sXML)
        'Root Elelemnt
        Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']")

        'Find the Node which has been changed
        Dim oChangedNode As XmlNode = Nothing
        If String.IsNullOrEmpty(sRIArrangementLineKey) OrElse sRIArrangementLineKey.Trim = "0" Then
            If Not String.IsNullOrEmpty(sTreatyCode) Then
                oChangedNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "' and @IsDeleted='False']")
            End If
        Else
            oChangedNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        End If

        If oChangedNode IsNot Nothing Then
            'Only set IsEditedDB if value actually differs from original DB XML
            Dim bIsEdited As Boolean = HasValueChangedFromOriginal(sOriginalXML, sRIBANDID, sRIArrangementLineKey, "CommissionPerc", sValue.ToString(), sTreatyCode)
            If bIsEdited Then
                If oChangedNode.Attributes("IsEditedDB") Is Nothing Then
                    Dim attrEdited As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
                    attrEdited.Value = "True"
                    oChangedNode.Attributes.Append(attrEdited)
                Else
                    oChangedNode.Attributes("IsEditedDB").Value = "True"
                End If
            End If
            Dim bIsNewNode As Boolean = False
            If oChangedNode.Attributes("IsNew") IsNot Nothing Then Boolean.TryParse(oChangedNode.Attributes("IsNew").Value, bIsNewNode)
            If Not bIsNewNode AndAlso bIsEdited Then
                If oChangedNode.Attributes("ActionType") Is Nothing Then
                    Dim attrAction As XmlAttribute = oXMLDoc.CreateAttribute("ActionType")
                    attrAction.Value = "1"
                    oChangedNode.Attributes.Append(attrAction)
                Else
                    oChangedNode.Attributes("ActionType").Value = "1"
                End If
            End If
            oChangedNode.Attributes("CommissionPerc").Value = sValue
        End If
        sXML = oXMLDoc.OuterXml
        ''ReCalculate
        Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
    End Sub
    ''' <summary>
    ''' this will upadte the xml with the agreement code
    ''' </summary>
    ''' <param name="sXML"></param>
    ''' <param name="sValue"></param>
    ''' <param name="sRIBANDID"></param>
    ''' <param name="sRIArrangementLineKey"></param>
    ''' <param name="sRIarrangementKey"></param>
    ''' <param name="sTransType"></param>
    ''' <remarks></remarks>
    Public Sub UpdateAgreement(ByRef sXML As String,
                                    ByVal sValue As String,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    Optional ByVal sTransType As String = "NB",
                                    Optional ByVal sTreatyCode As String = "",
                                    Optional ByVal sOriginalXML As String = "")
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(sXML)
        '  sRIBANDID = "Current_" & sRIBANDID
        'Root Elelemnt
        Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']")

        'Find the Node which has been changed
        Dim oChangedNode As XmlNode = Nothing
        If String.IsNullOrEmpty(sRIArrangementLineKey) OrElse sRIArrangementLineKey.Trim = "0" Then
            If Not String.IsNullOrEmpty(sTreatyCode) Then
                oChangedNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "' and @IsDeleted='False']")
            End If
        Else
            oChangedNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        End If

        If oChangedNode IsNot Nothing Then
            'Update the Line with new values
            'If oChangedNode.Attributes("IsEditedDB") Is Nothing Then
            '    Dim attrEdited As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
            '    attrEdited.Value = "True"
            '    oChangedNode.Attributes.Append(attrEdited)
            'Else
            '    oChangedNode.Attributes("IsEditedDB").Value = True
            'End If
            ' PBI 35359: Set IsEditedDB so is_edited=1 is persisted to DB on save
            ' ensuring Agreement override is carried forward on MTA via spu_RI_Arrangement_copy
            Dim bIsEdited As Boolean = HasValueChangedFromOriginal(sOriginalXML, sRIBANDID, sRIArrangementLineKey, "Agreement", sValue, sTreatyCode)
            If bIsEdited Then
                If oChangedNode.Attributes("IsEditedDB") Is Nothing Then
                    Dim attrDB As XmlAttribute = oXMLDoc.CreateAttribute("IsEditedDB")
                    attrDB.Value = "True"
                    oChangedNode.Attributes.Append(attrDB)
                Else
                    oChangedNode.Attributes("IsEditedDB").Value = "True"
                End If
            End If
            oChangedNode.Attributes("Agreement").Value = sValue
        End If
        sXML = oXMLDoc.OuterXml
        'ReCalculate
        'Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType)
    End Sub

    Public Sub UpdateClaimThisPercentage(ByRef sXML As String,
                                    ByVal sValue As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal iRIArrangementLineKey As Integer)

        'Populate the XML into the Memory
        Dim srClaimDataSet As New System.IO.StringReader(sXML)
        Dim xmlClaimTR As New XmlTextReader(srClaimDataSet)
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.Load(xmlClaimTR)
        xmlClaimTR.Close()

        'Root Elelemnt
        Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']")

        'Calculate/Retreive Band Total
        Dim dBANDSumInsured, dBANDReserveToDate, dBANDThisReserve, dBANDPaymentToDate, dBANDBalance, dBANDRecoverToDate As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dBANDReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dBANDThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dBANDPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dBANDBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dBANDRecoverToDate)
        End If

        'Find the Node which has been changed
        Dim oChangedNode As XmlNode = oXMLDoc.SelectSingleNode("//*[@RIArrangementLineKey='" & iRIArrangementLineKey.ToString & "']")

        If oChangedNode IsNot Nothing Then
            Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")

            If (oChangedNode.Attributes("Placement").Value = "Treaty Surplus" Or oChangedNode.Attributes("Placement").Value = "Treaty QSH") AndAlso oNetFACNode IsNot Nothing Then
                If oNetFACNode IsNot Nothing Then
                    Dim dNetSumInsured, dNetReserveToDate, dNetThisReserve, dNetPaymentToDate, dNetBalance, dNetRecoverToDate As Decimal
                    Decimal.TryParse(oNetFACNode.Attributes("SumInsured").Value, dNetSumInsured)
                    Decimal.TryParse(oNetFACNode.Attributes("ReserveToDate").Value, dNetReserveToDate)
                    Decimal.TryParse(oNetFACNode.Attributes("ThisReserve").Value, dNetThisReserve)
                    Decimal.TryParse(oNetFACNode.Attributes("PaymentToDate").Value, dNetPaymentToDate)
                    Decimal.TryParse(oNetFACNode.Attributes("Balance").Value, dNetBalance)
                    Decimal.TryParse(oNetFACNode.Attributes("RecoverToDate").Value, dNetRecoverToDate)

                    'Check the Limit of QSH and Surplus
                    Dim NoOfLines As Decimal
                    Dim dUpperLimit, dTotalLimit, dTempSumInsured As Decimal

                    Decimal.TryParse(oChangedNode.Attributes("LineLimit").Value, dUpperLimit)
                    Decimal.TryParse(oChangedNode.Attributes("NumberOfLines").Value, NoOfLines)
                    dTotalLimit = NoOfLines * dUpperLimit
                    dTempSumInsured = (dNetSumInsured * sValue) / 100
                    If dTempSumInsured > dTotalLimit Then
                        sValue = (dTotalLimit / dNetSumInsured) * 100
                        sValue = Math.Round(sValue, 4)
                    End If

                    oChangedNode.Attributes("IsEdited").Value = True
                    oChangedNode.Attributes("ThisPerc").Value = sValue
                    oChangedNode.Attributes("SumInsured").Value = ((dNetSumInsured * sValue) / 100)
                    oChangedNode.Attributes("ReserveToDate").Value = ((dNetReserveToDate * sValue) / 100)
                    oChangedNode.Attributes("ThisReserve").Value = ((dNetThisReserve * sValue) / 100)
                    oChangedNode.Attributes("PaymentToDate").Value = ((dNetPaymentToDate * sValue) / 100)
                    oChangedNode.Attributes("Balance").Value = ((dNetBalance * sValue) / 100)
                    oChangedNode.Attributes("RecoverToDate").Value = ((dNetRecoverToDate * sValue) / 100)
                End If

            Else
                'Retreive the Old valued before Changing value
                Dim dOldSumInsured, dOldReserveToDate, dOldThisReserve, dOldPaymentToDate, dOldBalance, dOldRecoverToDate As Decimal
                Decimal.TryParse(oChangedNode.Attributes("SumInsured").Value, dOldSumInsured)
                Decimal.TryParse(oChangedNode.Attributes("ReserveToDate").Value, dOldReserveToDate)
                Decimal.TryParse(oChangedNode.Attributes("ThisReserve").Value, dOldThisReserve)
                Decimal.TryParse(oChangedNode.Attributes("PaymentToDate").Value, dOldPaymentToDate)
                Decimal.TryParse(oChangedNode.Attributes("Balance").Value, dOldBalance)
                Decimal.TryParse(oChangedNode.Attributes("RecoverToDate").Value, dOldRecoverToDate)

                If (oChangedNode.Attributes("Placement").Value = "Treaty Surplus" Or oChangedNode.Attributes("Placement").Value = "Treaty QSH") Then
                    'Check the Limit of QSH and Surplus
                    Dim NoOfLines As Decimal
                    Dim dUpperLimit, dTotalLimit, dTempSumInsured As Decimal

                    Decimal.TryParse(oChangedNode.Attributes("LineLimit").Value, dUpperLimit)
                    Decimal.TryParse(oChangedNode.Attributes("NumberOfLines").Value, NoOfLines)
                    dTotalLimit = NoOfLines * dUpperLimit
                    dTempSumInsured = (dBANDSumInsured * sValue) / 100
                    If dTempSumInsured > dTotalLimit Then
                        sValue = (dTotalLimit / dBANDSumInsured) * 100
                        sValue = Math.Round(sValue, 4)
                    End If
                End If

                'Update the Line with new values
                oChangedNode.Attributes("IsEdited").Value = True
                oChangedNode.Attributes("ThisPerc").Value = sValue
                If dBANDSumInsured <> 0 Then
                    oChangedNode.Attributes("SumInsured").Value = ((dBANDSumInsured * sValue) / 100)
                End If

                If dBANDReserveToDate <> 0 Then
                    oChangedNode.Attributes("ReserveToDate").Value = ((dBANDReserveToDate * sValue) / 100)
                End If

                If dBANDThisReserve <> 0 Then
                    oChangedNode.Attributes("ThisReserve").Value = ((dBANDThisReserve * sValue) / 100)
                End If

                If dBANDPaymentToDate <> 0 Then
                    oChangedNode.Attributes("PaymentToDate").Value = ((dBANDPaymentToDate * sValue) / 100)
                End If

                If dBANDBalance <> 0 Then
                    oChangedNode.Attributes("Balance").Value = ((dBANDBalance * sValue) / 100)
                End If

                If dBANDRecoverToDate <> 0 Then
                    oChangedNode.Attributes("RecoverToDate").Value = ((dBANDRecoverToDate * sValue) / 100)
                End If
            End If
            'Calculate/Retreive Allocated Total
            Dim dAllocatedSumInsured, dAllocatedReserveToDate, dAllocatedThisReserve, dAllocatedPaymentToDate, dAllocatedBalance, dAllocatedRecoverToDate As Decimal
            Dim oAllocatedNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Allocated']")

            'Recalculate the Net FAC
            Dim sPlacement As String = oChangedNode.Attributes("Placement").Value
            If String.IsNullOrEmpty(sPlacement) = False AndAlso sPlacement.Trim.ToUpper = "FAC PROP" Then

                'Retreive the FAC Prop
                Dim dFACTotSumInsured, dFACTotReserveToDate, dFACTotThisReserve, dFACTotPaymentToDate, dFACTotBalance, dFACTotRecoverToDate As Decimal
                Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='FAC Prop']")
                Dim xmlNode As XmlNode = Nothing
                If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then

                    For Each xmlNode In xmlNodes
                        Dim dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                        Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                        Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                        Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dFACThisReserve)
                        Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                        Decimal.TryParse(xmlNode.Attributes("Balance").Value, dFACBalance)
                        Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)

                        dFACTotSumInsured += dFACSumInsured
                        dFACTotReserveToDate += dFACReserveToDate
                        dFACTotThisReserve += dFACThisReserve
                        dFACTotPaymentToDate += dFACPaymentToDate
                        dFACTotBalance += dFACBalance
                        dFACTotRecoverToDate += dFACRecoverToDate
                    Next
                End If

                'Retreive the FAC XOL
                xmlNodes = Nothing
                xmlNodes = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='FAC XOL']")

                If xmlNodes IsNot Nothing AndAlso xmlNodes.Count > 0 Then

                    xmlNode = Nothing
                    For Each xmlNode In xmlNodes
                        Dim dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                        Decimal.TryParse(xmlNode.Attributes("SumInsured").Value, dFACSumInsured)
                        Decimal.TryParse(xmlNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                        Decimal.TryParse(xmlNode.Attributes("ThisReserve").Value, dFACThisReserve)
                        Decimal.TryParse(xmlNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                        Decimal.TryParse(xmlNode.Attributes("Balance").Value, dFACBalance)
                        Decimal.TryParse(xmlNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)

                        dFACTotSumInsured += dFACSumInsured
                        dFACTotReserveToDate += dFACReserveToDate
                        dFACTotThisReserve += dFACThisReserve
                        dFACTotPaymentToDate += dFACPaymentToDate
                        dFACTotBalance += dFACBalance
                        dFACTotRecoverToDate += dFACRecoverToDate
                    Next
                End If

                'Calculate/Retreive Net FAC
                Dim dNetFACSumInsured, dNetFACReserveToDate, dNetFACThisReserve, dNetFACPaymentToDate, dNetFACBalance, dNetFACRecoverToDate As Decimal
                ' Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
                If oNetFACNode IsNot Nothing Then
                    'Update the Net FAC with New Values
                    dNetFACSumInsured = dBANDSumInsured - dFACTotSumInsured
                    dNetFACReserveToDate = dBANDReserveToDate - dFACTotReserveToDate
                    dNetFACThisReserve = dBANDThisReserve - dFACTotThisReserve
                    dNetFACPaymentToDate = dBANDPaymentToDate - dFACTotPaymentToDate
                    dNetFACBalance = dBANDBalance - dFACTotBalance
                    dNetFACRecoverToDate = dBANDRecoverToDate - dFACTotRecoverToDate

                    oNetFACNode.Attributes("SumInsured").Value = dNetFACSumInsured
                    oNetFACNode.Attributes("ReserveToDate").Value = dNetFACReserveToDate
                    oNetFACNode.Attributes("ThisReserve").Value = dNetFACThisReserve
                    oNetFACNode.Attributes("PaymentToDate").Value = dNetFACPaymentToDate
                    oNetFACNode.Attributes("Balance").Value = dNetFACBalance
                    oNetFACNode.Attributes("RecoverToDate").Value = dNetFACRecoverToDate
                End If

                'Surplus
                'Calculate/Retreive Surplus
                Dim dTotSurplusSumInsured, dTotSurplusReserveToDate, dTotSurplusThisReserve, dTotSurplusPaymentToDate, dTotSurplusBalance, dTotSurplusRecoverToDate As Decimal
                Dim oSurplusNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty Surplus']")
                If oSurplusNodeList IsNot Nothing AndAlso oSurplusNodeList.Count > 0 Then
                    For Each oSurplusNode As XmlNode In oSurplusNodeList
                        Dim dSurplusThisPerc, dSurplusSumInsured, dSurplusReserveToDate, dSurplusThisReserve, dSurplusPaymentToDate, dSurplusBalance, dSurplusRecoverToDate As Decimal
                        Decimal.TryParse(oSurplusNode.Attributes("ThisPerc").Value, dSurplusThisPerc)

                        If oNetFACNode IsNot Nothing Then
                            dSurplusSumInsured = (dNetFACSumInsured * dSurplusThisPerc) / 100
                            dSurplusReserveToDate = (dNetFACReserveToDate * dSurplusThisPerc) / 100
                            dSurplusThisReserve = (dNetFACThisReserve * dSurplusThisPerc) / 100
                            dSurplusPaymentToDate = (dNetFACPaymentToDate * dSurplusThisPerc) / 100
                            dSurplusBalance = (dNetFACBalance * dSurplusThisPerc) / 100
                            dSurplusRecoverToDate = (dNetFACRecoverToDate * dSurplusThisPerc) / 100
                        Else
                            dSurplusSumInsured = (dBANDSumInsured * dSurplusThisPerc) / 100
                            dSurplusReserveToDate = (dBANDReserveToDate * dSurplusThisPerc) / 100
                            dSurplusThisReserve = (dBANDThisReserve * dSurplusThisPerc) / 100
                            dSurplusPaymentToDate = (dBANDPaymentToDate * dSurplusThisPerc) / 100
                            dSurplusBalance = (dBANDBalance * dSurplusThisPerc) / 100
                            dSurplusRecoverToDate = (dBANDRecoverToDate * dSurplusThisPerc) / 100
                        End If


                        dTotSurplusSumInsured = dTotSurplusSumInsured + dSurplusSumInsured
                        dTotSurplusReserveToDate = dTotSurplusReserveToDate + dSurplusReserveToDate
                        dTotSurplusThisReserve = dTotSurplusThisReserve + dSurplusThisReserve
                        dTotSurplusPaymentToDate = dTotSurplusPaymentToDate + dSurplusPaymentToDate
                        dTotSurplusBalance = dTotSurplusBalance + dSurplusBalance
                        dTotSurplusRecoverToDate = dTotSurplusRecoverToDate + dSurplusRecoverToDate

                        oSurplusNode.Attributes("IsEdited").Value = True
                        oSurplusNode.Attributes("SumInsured").Value = dSurplusSumInsured
                        oSurplusNode.Attributes("ReserveToDate").Value = dSurplusReserveToDate
                        oSurplusNode.Attributes("ThisReserve").Value = dSurplusThisReserve
                        oSurplusNode.Attributes("PaymentToDate").Value = dSurplusPaymentToDate
                        oSurplusNode.Attributes("Balance").Value = dSurplusBalance
                        oSurplusNode.Attributes("RecoverToDate").Value = dSurplusRecoverToDate
                    Next
                End If

                'Treaty QSH
                'Calculate/Retreive QSH
                Dim dTotQSHSumInsured, dTotQSHReserveToDate, dTotQSHThisReserve, dTotQSHPaymentToDate, dTotQSHBalance, dTotQSHRecoverToDate As Decimal
                Dim oQSHNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty QSH']")
                If oQSHNodeList IsNot Nothing AndAlso oQSHNodeList.Count > 0 Then

                    For Each oQSHNode As XmlNode In oQSHNodeList
                        Dim dQSHThisPerc, dQSHSumInsured, dQSHReserveToDate, dQSHThisReserve, dQSHPaymentToDate, dQSHBalance, dQSHRecoverToDate As Decimal
                        Decimal.TryParse(oQSHNode.Attributes("ThisPerc").Value, dQSHThisPerc)

                        If oNetFACNode IsNot Nothing Then
                            dQSHSumInsured = (dNetFACSumInsured * dQSHThisPerc) / 100
                            dQSHReserveToDate = (dNetFACReserveToDate * dQSHThisPerc) / 100
                            dQSHThisReserve = (dNetFACThisReserve * dQSHThisPerc) / 100
                            dQSHPaymentToDate = (dNetFACPaymentToDate * dQSHThisPerc) / 100
                            dQSHBalance = (dNetFACBalance * dQSHThisPerc) / 100
                            dQSHRecoverToDate = (dNetFACRecoverToDate * dQSHThisPerc) / 100
                        Else
                            dQSHSumInsured = (dBANDSumInsured * dQSHThisPerc) / 100
                            dQSHReserveToDate = (dBANDReserveToDate * dQSHThisPerc) / 100
                            dQSHThisReserve = (dBANDThisReserve * dQSHThisPerc) / 100
                            dQSHPaymentToDate = (dBANDPaymentToDate * dQSHThisPerc) / 100
                            dQSHBalance = (dBANDBalance * dQSHThisPerc) / 100
                            dQSHRecoverToDate = (dBANDRecoverToDate * dQSHThisPerc) / 100
                        End If

                        dTotQSHSumInsured = dTotQSHSumInsured + dQSHSumInsured
                        dTotQSHReserveToDate = dTotQSHReserveToDate + dQSHReserveToDate
                        dTotQSHThisReserve = dTotQSHThisReserve + dQSHThisReserve
                        dTotQSHPaymentToDate = dTotQSHPaymentToDate + dQSHPaymentToDate
                        dTotQSHBalance = dTotQSHBalance + dQSHBalance
                        dTotQSHRecoverToDate = dTotQSHRecoverToDate + dQSHRecoverToDate

                        oQSHNode.Attributes("IsEdited").Value = True
                        oQSHNode.Attributes("SumInsured").Value = dQSHSumInsured
                        oQSHNode.Attributes("ReserveToDate").Value = dQSHReserveToDate
                        oQSHNode.Attributes("ThisReserve").Value = dQSHThisReserve
                        oQSHNode.Attributes("PaymentToDate").Value = dQSHPaymentToDate
                        oQSHNode.Attributes("Balance").Value = dQSHBalance
                        oQSHNode.Attributes("RecoverToDate").Value = dQSHRecoverToDate
                    Next
                End If

                'Treaty XOL
                Dim dXOLTotSumInsured, dXOLTotThisReserve, dXOLTotBalance As Decimal
                Dim oXOLNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty XOL']")
                Dim dXOLSumInsured, dXOLThisReserve, dXOLBalance As Decimal

                If oNetFACNode IsNot Nothing Then
                    dXOLSumInsured = dNetFACSumInsured - dTotQSHSumInsured
                    dXOLThisReserve = dNetFACThisReserve - dTotQSHThisReserve
                    dXOLBalance = dNetFACBalance - dTotQSHBalance
                Else
                    dXOLSumInsured = dBANDSumInsured - dTotQSHSumInsured
                    dXOLThisReserve = dBANDThisReserve - dTotQSHThisReserve
                    dXOLBalance = dBANDBalance - dTotQSHBalance
                End If

                For Each oXOLNode As XmlNode In oXOLNodeList
                    Dim dLowerLimit, dUpperLimit As Decimal

                    Decimal.TryParse(oXOLNode.Attributes("LowerLimit").Value, dLowerLimit)
                    Decimal.TryParse(oXOLNode.Attributes("LineLimit").Value, dUpperLimit)

                    'Sum Insured
                    If dXOLTotSumInsured <= dXOLSumInsured Then
                        If dXOLSumInsured < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = 0

                        ElseIf dXOLSumInsured >= dLowerLimit AndAlso dXOLSumInsured <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLSumInsured - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = dLimitDiff
                            '  dXOLSumInsured = dXOLSumInsured - dLimitDiff
                            dXOLTotSumInsured = dXOLTotSumInsured + dLimitDiff

                        ElseIf dXOLSumInsured > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = dLimitDiff
                            ' dXOLSumInsured = dXOLSumInsured - dLimitDiff
                            dXOLTotSumInsured = dXOLTotSumInsured + dLimitDiff
                        End If
                    End If


                    'This Reserve
                    If dXOLTotThisReserve <= dXOLThisReserve Then
                        If dXOLThisReserve < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = 0

                        ElseIf dXOLThisReserve >= dLowerLimit AndAlso dXOLThisReserve <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLThisReserve - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = dLimitDiff
                            ' dXOLThisReserve = dXOLThisReserve - dLimitDiff
                            dXOLTotThisReserve = dXOLTotThisReserve + dLimitDiff

                        ElseIf dXOLThisReserve > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = dLimitDiff
                            ' dXOLThisReserve = dXOLThisReserve - dLimitDiff
                            dXOLTotThisReserve = dXOLTotThisReserve + dLimitDiff
                        End If
                    End If

                    'Balance
                    If dXOLTotBalance <= dXOLBalance Then
                        If dXOLBalance < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = 0

                        ElseIf dXOLBalance >= dLowerLimit AndAlso dXOLBalance <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLBalance - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = dLimitDiff
                            '  dXOLBalance = dXOLBalance - dLimitDiff
                            dXOLTotBalance = dXOLTotBalance + dLimitDiff

                        ElseIf dXOLBalance > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = dLimitDiff
                            '  dXOLBalance = dXOLBalance - dLimitDiff
                            dXOLTotBalance = dXOLTotBalance + dLimitDiff
                        End If
                    End If
                Next

                'Retained
                Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                Dim oRetNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Net']")
                If oNetFACNode IsNot Nothing Then
                    If oRetNodeList IsNot Nothing AndAlso oRetNodeList.Count > 0 Then
                        For Each oRetNode As XmlNode In oRetNodeList
                            Dim dRetThisPerc, dRetSumInsured, dRetReserveToDate, dRetThisReserve, dRetPaymentToDate, dRetBalance, dRetRecoverToDate As Decimal
                            Decimal.TryParse(oRetNode.Attributes("ThisPerc").Value, dRetThisPerc)

                            dRetSumInsured = ((dNetFACSumInsured - (dTotQSHSumInsured + dTotSurplusSumInsured + dXOLTotSumInsured)))
                            dRetReserveToDate = ((dNetFACReserveToDate - (dTotQSHReserveToDate + dTotSurplusReserveToDate)))
                            dRetThisReserve = ((dNetFACThisReserve - (dXOLTotThisReserve + dTotQSHThisReserve + dTotSurplusThisReserve)))
                            dRetPaymentToDate = ((dNetFACPaymentToDate - (dTotQSHPaymentToDate + dTotSurplusPaymentToDate)))
                            dRetBalance = ((dNetFACBalance - (dXOLTotBalance + dTotQSHBalance + dTotSurplusBalance)))
                            dRetRecoverToDate = ((dNetFACRecoverToDate - (dTotQSHRecoverToDate + dTotSurplusRecoverToDate)))

                            oRetNode.Attributes("IsEdited").Value = True
                            oRetNode.Attributes("SumInsured").Value = dRetSumInsured
                            oRetNode.Attributes("ReserveToDate").Value = dRetReserveToDate
                            oRetNode.Attributes("ThisReserve").Value = dRetThisReserve
                            oRetNode.Attributes("PaymentToDate").Value = dRetPaymentToDate
                            oRetNode.Attributes("Balance").Value = dRetBalance
                            oRetNode.Attributes("RecoverToDate").Value = dRetRecoverToDate
                        Next
                    End If
                ElseIf oBandNode IsNot Nothing Then
                    If oRetNodeList IsNot Nothing AndAlso oRetNodeList.Count > 0 Then
                        For Each oRetNode As XmlNode In oRetNodeList
                            Dim dRetThisPerc, dRetSumInsured, dRetReserveToDate, dRetThisReserve, dRetPaymentToDate, dRetBalance, dRetRecoverToDate As Decimal
                            Decimal.TryParse(oRetNode.Attributes("ThisPerc").Value, dRetThisPerc)

                            dRetSumInsured = ((dBANDSumInsured - (dTotQSHSumInsured + dTotSurplusSumInsured + dXOLTotSumInsured)))
                            dRetReserveToDate = ((dBANDReserveToDate - (dTotQSHReserveToDate + dTotSurplusReserveToDate)))
                            dRetThisReserve = ((dBANDThisReserve - (dXOLTotThisReserve + dTotQSHThisReserve + dTotSurplusThisReserve)))
                            dRetPaymentToDate = ((dBANDPaymentToDate - (dTotQSHPaymentToDate + dTotSurplusPaymentToDate)))
                            dRetBalance = ((dBANDBalance - (dXOLTotBalance + dTotQSHBalance + dTotSurplusBalance)))
                            dRetRecoverToDate = ((dBANDRecoverToDate - (dTotQSHRecoverToDate + dTotSurplusRecoverToDate)))

                            oRetNode.Attributes("IsEdited").Value = True
                            oRetNode.Attributes("SumInsured").Value = dRetSumInsured
                            oRetNode.Attributes("ReserveToDate").Value = dRetReserveToDate
                            oRetNode.Attributes("ThisReserve").Value = dRetThisReserve
                            oRetNode.Attributes("PaymentToDate").Value = dRetPaymentToDate
                            oRetNode.Attributes("Balance").Value = dRetBalance
                            oRetNode.Attributes("RecoverToDate").Value = dRetRecoverToDate
                        Next
                    End If
                End If

                'Calculation of Allocated
                Dim xmlAllNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow")
                'Dim dFACTotSumInsured, dFACTotReserveToDate, dFACTotThisReserve, dFACTotPaymentToDate, dFACTotBalance, dFACTotRecoverToDate As Decimal
                dFACTotSumInsured = 0
                dFACTotReserveToDate = 0
                dFACTotThisReserve = 0
                dFACTotPaymentToDate = 0
                dFACTotBalance = 0
                dFACTotRecoverToDate = 0

                For Each oArrangementNode As XmlNode In xmlAllNodes
                    Dim sArrangementName, sArrangementPlacement As String
                    sArrangementName = oArrangementNode.Attributes("Name").Value
                    sArrangementPlacement = oArrangementNode.Attributes("Placement").Value

                    If sArrangementName = "Net of FAC" Or sArrangementName = "Band Total" _
                    Or sArrangementName = "Allocated" Or sArrangementName = "Unallocated" Then
                        'These will not take part in calculation
                    Else
                        Dim dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                        Decimal.TryParse(oArrangementNode.Attributes("SumInsured").Value, dFACSumInsured)
                        Decimal.TryParse(oArrangementNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                        Decimal.TryParse(oArrangementNode.Attributes("ThisReserve").Value, dFACThisReserve)
                        Decimal.TryParse(oArrangementNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                        Decimal.TryParse(oArrangementNode.Attributes("Balance").Value, dFACBalance)
                        Decimal.TryParse(oArrangementNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)

                        dFACTotSumInsured += dFACSumInsured
                        dFACTotReserveToDate += dFACReserveToDate
                        dFACTotThisReserve += dFACThisReserve
                        dFACTotPaymentToDate += dFACPaymentToDate
                        dFACTotBalance += dFACBalance
                        dFACTotRecoverToDate += dFACRecoverToDate
                    End If

                Next

                'Calculate/Retreive Allocated Total
                If oAllocatedNode IsNot Nothing Then
                    dAllocatedSumInsured = dFACTotSumInsured
                    dAllocatedReserveToDate = dFACTotReserveToDate
                    dAllocatedThisReserve = dFACTotThisReserve
                    dAllocatedPaymentToDate = dFACTotPaymentToDate
                    dAllocatedBalance = dFACTotBalance
                    dAllocatedRecoverToDate = dFACTotRecoverToDate

                    oAllocatedNode.Attributes("SumInsured").Value = dAllocatedSumInsured
                    oAllocatedNode.Attributes("ReserveToDate").Value = dAllocatedReserveToDate
                    oAllocatedNode.Attributes("ThisReserve").Value = dAllocatedThisReserve
                    oAllocatedNode.Attributes("PaymentToDate").Value = dAllocatedPaymentToDate
                    oAllocatedNode.Attributes("Balance").Value = dAllocatedBalance
                    oAllocatedNode.Attributes("RecoverToDate").Value = dAllocatedRecoverToDate
                End If

            ElseIf String.IsNullOrEmpty(sPlacement) = False AndAlso sPlacement.Trim.ToUpper = "TREATY SURPLUS" Then
                'Calculate/Retreive Net FAC
                Dim dFACNetSumInsured, dFACNetReserveToDate, dFACNetThisReserve, dFACNetPaymentToDate, dFACNetBalance, dFACNetRecoverToDate As Decimal
                ' Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
                If oNetFACNode IsNot Nothing Then

                    Decimal.TryParse(oNetFACNode.Attributes("SumInsured").Value, dFACNetSumInsured)
                    Decimal.TryParse(oNetFACNode.Attributes("ReserveToDate").Value, dFACNetReserveToDate)
                    Decimal.TryParse(oNetFACNode.Attributes("ThisReserve").Value, dFACNetThisReserve)
                    Decimal.TryParse(oNetFACNode.Attributes("PaymentToDate").Value, dFACNetPaymentToDate)
                    Decimal.TryParse(oNetFACNode.Attributes("Balance").Value, dFACNetBalance)
                    Decimal.TryParse(oNetFACNode.Attributes("RecoverToDate").Value, dFACNetRecoverToDate)

                End If

                'Surplus
                'Calculate/Retreive Surplus
                Dim dTotSurplusSumInsured, dTotSurplusReserveToDate, dTotSurplusThisReserve, dTotSurplusPaymentToDate, dTotSurplusBalance, dTotSurplusRecoverToDate As Decimal
                Dim oSurplusNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty Surplus']")
                If oSurplusNodeList IsNot Nothing AndAlso oSurplusNodeList.Count > 0 Then
                    For Each oSurplusNode As XmlNode In oSurplusNodeList
                        Dim iSurplusRIArrLineKey As Integer
                        Dim dSurplusThisPerc, dSurplusSumInsured, dSurplusReserveToDate, dSurplusThisReserve, dSurplusPaymentToDate, dSurplusBalance, dSurplusRecoverToDate As Decimal
                        Integer.TryParse(oSurplusNode.Attributes("RIArrangementLineKey").Value, iSurplusRIArrLineKey)
                        Decimal.TryParse(oSurplusNode.Attributes("ThisPerc").Value, dSurplusThisPerc)

                        If iSurplusRIArrLineKey <> iRIArrangementLineKey Then

                            If oNetFACNode IsNot Nothing Then
                                dSurplusSumInsured = (dFACNetSumInsured * dSurplusThisPerc) / 100
                                dSurplusReserveToDate = (dFACNetReserveToDate * dSurplusThisPerc) / 100
                                dSurplusThisReserve = (dFACNetThisReserve * dSurplusThisPerc) / 100
                                dSurplusPaymentToDate = (dFACNetPaymentToDate * dSurplusThisPerc) / 100
                                dSurplusBalance = (dFACNetBalance * dSurplusThisPerc) / 100
                                dSurplusRecoverToDate = (dFACNetRecoverToDate * dSurplusThisPerc) / 100
                            Else
                                dSurplusSumInsured = (dBANDSumInsured * dSurplusThisPerc) / 100
                                dSurplusReserveToDate = (dBANDReserveToDate * dSurplusThisPerc) / 100
                                dSurplusThisReserve = (dBANDThisReserve * dSurplusThisPerc) / 100
                                dSurplusPaymentToDate = (dBANDPaymentToDate * dSurplusThisPerc) / 100
                                dSurplusBalance = (dBANDBalance * dSurplusThisPerc) / 100
                                dSurplusRecoverToDate = (dBANDRecoverToDate * dSurplusThisPerc) / 100
                            End If

                            dTotSurplusSumInsured = dTotSurplusSumInsured + dSurplusSumInsured
                            dTotSurplusReserveToDate = dTotSurplusReserveToDate + dSurplusReserveToDate
                            dTotSurplusThisReserve = dTotSurplusThisReserve + dSurplusThisReserve
                            dTotSurplusPaymentToDate = dTotSurplusPaymentToDate + dSurplusPaymentToDate
                            dTotSurplusBalance = dTotSurplusBalance + dSurplusBalance
                            dTotSurplusRecoverToDate = dTotSurplusRecoverToDate + dSurplusRecoverToDate

                            oSurplusNode.Attributes("IsEdited").Value = True
                            oSurplusNode.Attributes("SumInsured").Value = dSurplusSumInsured
                            oSurplusNode.Attributes("ReserveToDate").Value = dSurplusReserveToDate
                            oSurplusNode.Attributes("ThisReserve").Value = dSurplusThisReserve
                            oSurplusNode.Attributes("PaymentToDate").Value = dSurplusPaymentToDate
                            oSurplusNode.Attributes("Balance").Value = dSurplusBalance
                            oSurplusNode.Attributes("RecoverToDate").Value = dSurplusRecoverToDate
                        Else
                            Decimal.TryParse(oSurplusNode.Attributes("SumInsured").Value, dSurplusSumInsured)
                            Decimal.TryParse(oSurplusNode.Attributes("ReserveToDate").Value, dSurplusReserveToDate)
                            Decimal.TryParse(oSurplusNode.Attributes("ThisReserve").Value, dSurplusThisReserve)
                            Decimal.TryParse(oSurplusNode.Attributes("PaymentToDate").Value, dSurplusPaymentToDate)
                            Decimal.TryParse(oSurplusNode.Attributes("Balance").Value, dSurplusBalance)
                            Decimal.TryParse(oSurplusNode.Attributes("RecoverToDate").Value, dSurplusRecoverToDate)

                            dTotSurplusSumInsured = dTotSurplusSumInsured + dSurplusSumInsured
                            dTotSurplusReserveToDate = dTotSurplusReserveToDate + dSurplusReserveToDate
                            dTotSurplusThisReserve = dTotSurplusThisReserve + dSurplusThisReserve
                            dTotSurplusPaymentToDate = dTotSurplusPaymentToDate + dSurplusPaymentToDate
                            dTotSurplusBalance = dTotSurplusBalance + dSurplusBalance
                            dTotSurplusRecoverToDate = dTotSurplusRecoverToDate + dSurplusRecoverToDate
                        End If

                    Next
                End If

                'Treaty QSH
                'Calculate/Retreive QSH
                Dim dTotQSHSumInsured, dTotQSHReserveToDate, dTotQSHThisReserve, dTotQSHPaymentToDate, dTotQSHBalance, dTotQSHRecoverToDate As Decimal
                Dim oQSHNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty QSH']")
                If oQSHNodeList IsNot Nothing AndAlso oQSHNodeList.Count > 0 Then
                    For Each oQSHNode As XmlNode In oQSHNodeList
                        Dim dQSHThisPerc, dQSHSumInsured, dQSHReserveToDate, dQSHThisReserve, dQSHPaymentToDate, dQSHBalance, dQSHRecoverToDate As Decimal
                        Decimal.TryParse(oQSHNode.Attributes("ThisPerc").Value, dQSHThisPerc)

                        If oNetFACNode IsNot Nothing Then
                            dQSHSumInsured = (dFACNetSumInsured * dQSHThisPerc) / 100
                            dQSHReserveToDate = (dFACNetReserveToDate * dQSHThisPerc) / 100
                            dQSHThisReserve = (dFACNetThisReserve * dQSHThisPerc) / 100
                            dQSHPaymentToDate = (dFACNetPaymentToDate * dQSHThisPerc) / 100
                            dQSHBalance = (dFACNetBalance * dQSHThisPerc) / 100
                            dQSHRecoverToDate = (dFACNetRecoverToDate * dQSHThisPerc) / 100
                        Else
                            dQSHSumInsured = (dBANDSumInsured * dQSHThisPerc) / 100
                            dQSHReserveToDate = (dBANDReserveToDate * dQSHThisPerc) / 100
                            dQSHThisReserve = (dBANDThisReserve * dQSHThisPerc) / 100
                            dQSHPaymentToDate = (dBANDPaymentToDate * dQSHThisPerc) / 100
                            dQSHBalance = (dBANDBalance * dQSHThisPerc) / 100
                            dQSHRecoverToDate = (dBANDRecoverToDate * dQSHThisPerc) / 100
                        End If

                        dTotQSHSumInsured = dTotQSHSumInsured + dQSHSumInsured
                        dTotQSHReserveToDate = dTotQSHReserveToDate + dQSHReserveToDate
                        dTotQSHThisReserve = dTotQSHThisReserve + dQSHThisReserve
                        dTotQSHPaymentToDate = dTotQSHPaymentToDate + dQSHPaymentToDate
                        dTotQSHBalance = dTotQSHBalance + dQSHBalance
                        dTotQSHRecoverToDate = dTotQSHRecoverToDate + dQSHRecoverToDate

                        oQSHNode.Attributes("IsEdited").Value = True
                        oQSHNode.Attributes("SumInsured").Value = dQSHSumInsured
                        oQSHNode.Attributes("ReserveToDate").Value = dQSHReserveToDate
                        oQSHNode.Attributes("ThisReserve").Value = dQSHThisReserve
                        oQSHNode.Attributes("PaymentToDate").Value = dQSHPaymentToDate
                        oQSHNode.Attributes("Balance").Value = dQSHBalance
                        oQSHNode.Attributes("RecoverToDate").Value = dQSHRecoverToDate
                    Next
                End If

                'Treaty XOL
                Dim dXOLTotSumInsured, dXOLTotThisReserve, dXOLTotBalance As Decimal
                Dim oXOLNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty XOL']")
                Dim dXOLSumInsured, dXOLThisReserve, dXOLBalance As Decimal

                If oNetFACNode IsNot Nothing Then
                    dXOLSumInsured = dFACNetSumInsured - dTotQSHSumInsured
                    dXOLThisReserve = dFACNetThisReserve - dTotQSHThisReserve
                    dXOLBalance = dFACNetBalance - dTotQSHBalance
                Else
                    dXOLSumInsured = dBANDSumInsured - dTotQSHSumInsured
                    dXOLThisReserve = dBANDThisReserve - dTotQSHThisReserve
                    dXOLBalance = dBANDBalance - dTotQSHBalance
                End If

                For Each oXOLNode As XmlNode In oXOLNodeList
                    Dim dLowerLimit, dUpperLimit As Decimal

                    Decimal.TryParse(oXOLNode.Attributes("LowerLimit").Value, dLowerLimit)
                    Decimal.TryParse(oXOLNode.Attributes("LineLimit").Value, dUpperLimit)

                    'Sum Insured
                    If dXOLTotSumInsured <= dXOLSumInsured Then
                        If dXOLSumInsured < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = 0

                        ElseIf dXOLSumInsured >= dLowerLimit AndAlso dXOLSumInsured <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLSumInsured - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = dLimitDiff
                            '  dXOLSumInsured = dXOLSumInsured - dLimitDiff
                            dXOLTotSumInsured = dXOLTotSumInsured + dLimitDiff

                        ElseIf dXOLSumInsured > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = dLimitDiff
                            ' dXOLSumInsured = dXOLSumInsured - dLimitDiff
                            dXOLTotSumInsured = dXOLTotSumInsured + dLimitDiff
                        End If
                    End If


                    'This Reserve
                    If dXOLTotThisReserve <= dXOLThisReserve Then
                        If dXOLThisReserve < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = 0

                        ElseIf dXOLThisReserve >= dLowerLimit AndAlso dXOLThisReserve <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLThisReserve - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = dLimitDiff
                            ' dXOLThisReserve = dXOLThisReserve - dLimitDiff
                            dXOLTotThisReserve = dXOLTotThisReserve + dLimitDiff

                        ElseIf dXOLThisReserve > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = dLimitDiff
                            ' dXOLThisReserve = dXOLThisReserve - dLimitDiff
                            dXOLTotThisReserve = dXOLTotThisReserve + dLimitDiff
                        End If
                    End If

                    'Balance
                    If dXOLTotBalance <= dXOLBalance Then
                        If dXOLBalance < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = 0

                        ElseIf dXOLBalance >= dLowerLimit AndAlso dXOLBalance <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLBalance - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = dLimitDiff
                            '  dXOLBalance = dXOLBalance - dLimitDiff
                            dXOLTotBalance = dXOLTotBalance + dLimitDiff

                        ElseIf dXOLBalance > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = dLimitDiff
                            '  dXOLBalance = dXOLBalance - dLimitDiff
                            dXOLTotBalance = dXOLTotBalance + dLimitDiff
                        End If
                    End If
                Next

                'Retained
                Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                Dim oRetNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Net']")
                If oNetFACNode IsNot Nothing Then
                    If oRetNodeList IsNot Nothing AndAlso oRetNodeList.Count > 0 Then
                        For Each oRetNode As XmlNode In oRetNodeList
                            Dim dRetThisPerc, dRetSumInsured, dRetReserveToDate, dRetThisReserve, dRetPaymentToDate, dRetBalance, dRetRecoverToDate As Decimal
                            Decimal.TryParse(oRetNode.Attributes("ThisPerc").Value, dRetThisPerc)

                            dRetSumInsured = ((dFACNetSumInsured - (dTotQSHSumInsured + dTotSurplusSumInsured + dXOLTotSumInsured)))
                            dRetReserveToDate = ((dFACNetReserveToDate - (dTotQSHReserveToDate + dTotSurplusReserveToDate)))
                            dRetThisReserve = ((dFACNetThisReserve - (dXOLTotThisReserve + dTotQSHThisReserve + dTotSurplusThisReserve)))
                            dRetPaymentToDate = ((dFACNetPaymentToDate - (dTotQSHPaymentToDate + dTotSurplusPaymentToDate)))
                            dRetBalance = ((dFACNetBalance - (dXOLTotBalance + dTotQSHBalance + dTotSurplusBalance)))
                            dRetRecoverToDate = ((dFACNetRecoverToDate - (dTotQSHRecoverToDate + dTotSurplusRecoverToDate)))

                            oRetNode.Attributes("IsEdited").Value = True
                            oRetNode.Attributes("SumInsured").Value = dRetSumInsured
                            oRetNode.Attributes("ReserveToDate").Value = dRetReserveToDate
                            oRetNode.Attributes("ThisReserve").Value = dRetThisReserve
                            oRetNode.Attributes("PaymentToDate").Value = dRetPaymentToDate
                            oRetNode.Attributes("Balance").Value = dRetBalance
                            oRetNode.Attributes("RecoverToDate").Value = dRetRecoverToDate
                        Next
                    End If
                ElseIf oBandNode IsNot Nothing Then
                    If oRetNodeList IsNot Nothing AndAlso oRetNodeList.Count > 0 Then
                        For Each oRetNode As XmlNode In oRetNodeList
                            Dim dRetThisPerc, dRetSumInsured, dRetReserveToDate, dRetThisReserve, dRetPaymentToDate, dRetBalance, dRetRecoverToDate As Decimal
                            Decimal.TryParse(oRetNode.Attributes("ThisPerc").Value, dRetThisPerc)

                            dRetSumInsured = ((dBANDSumInsured - (dTotQSHSumInsured + dTotSurplusSumInsured + dXOLTotSumInsured)))
                            dRetReserveToDate = ((dBANDReserveToDate - (dTotQSHReserveToDate + dTotSurplusReserveToDate)))
                            dRetThisReserve = ((dBANDThisReserve - (dXOLTotThisReserve + dTotQSHThisReserve + dTotSurplusThisReserve)))
                            dRetPaymentToDate = ((dBANDPaymentToDate - (dTotQSHPaymentToDate + dTotSurplusPaymentToDate)))
                            dRetBalance = ((dBANDBalance - (dXOLTotBalance + dTotQSHBalance + dTotSurplusBalance)))
                            dRetRecoverToDate = ((dBANDRecoverToDate - (dTotQSHRecoverToDate + dTotSurplusRecoverToDate)))

                            oRetNode.Attributes("IsEdited").Value = True
                            oRetNode.Attributes("SumInsured").Value = dRetSumInsured
                            oRetNode.Attributes("ReserveToDate").Value = dRetReserveToDate
                            oRetNode.Attributes("ThisReserve").Value = dRetThisReserve
                            oRetNode.Attributes("PaymentToDate").Value = dRetPaymentToDate
                            oRetNode.Attributes("Balance").Value = dRetBalance
                            oRetNode.Attributes("RecoverToDate").Value = dRetRecoverToDate
                        Next
                    End If
                End If

                'Calculation of Allocated
                Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow")
                Dim dFACTotSumInsured, dFACTotReserveToDate, dFACTotThisReserve, dFACTotPaymentToDate, dFACTotBalance, dFACTotRecoverToDate As Decimal
                For Each oArrangementNode As XmlNode In xmlNodes
                    Dim sArrangementName, sArrangementPlacement As String
                    sArrangementName = oArrangementNode.Attributes("Name").Value
                    sArrangementPlacement = oArrangementNode.Attributes("Placement").Value

                    If sArrangementName = "Net of FAC" Or sArrangementName = "Band Total" _
                    Or sArrangementName = "Allocated" Or sArrangementName = "Unallocated" Then
                        'These will not take part in calculation
                    Else
                        Dim dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                        Decimal.TryParse(oArrangementNode.Attributes("SumInsured").Value, dFACSumInsured)
                        Decimal.TryParse(oArrangementNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                        Decimal.TryParse(oArrangementNode.Attributes("ThisReserve").Value, dFACThisReserve)
                        Decimal.TryParse(oArrangementNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                        Decimal.TryParse(oArrangementNode.Attributes("Balance").Value, dFACBalance)
                        Decimal.TryParse(oArrangementNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)

                        dFACTotSumInsured += dFACSumInsured
                        dFACTotReserveToDate += dFACReserveToDate
                        dFACTotThisReserve += dFACThisReserve
                        dFACTotPaymentToDate += dFACPaymentToDate
                        dFACTotBalance += dFACBalance
                        dFACTotRecoverToDate += dFACRecoverToDate
                    End If

                Next

                'Calculate/Retreive Allocated Total
                If oAllocatedNode IsNot Nothing Then
                    dAllocatedSumInsured = dFACTotSumInsured
                    dAllocatedReserveToDate = dFACTotReserveToDate
                    dAllocatedThisReserve = dFACTotThisReserve
                    dAllocatedPaymentToDate = dFACTotPaymentToDate
                    dAllocatedBalance = dFACTotBalance
                    dAllocatedRecoverToDate = dFACTotRecoverToDate

                    oAllocatedNode.Attributes("SumInsured").Value = dAllocatedSumInsured
                    oAllocatedNode.Attributes("ReserveToDate").Value = dAllocatedReserveToDate
                    oAllocatedNode.Attributes("ThisReserve").Value = dAllocatedThisReserve
                    oAllocatedNode.Attributes("PaymentToDate").Value = dAllocatedPaymentToDate
                    oAllocatedNode.Attributes("Balance").Value = dAllocatedBalance
                    oAllocatedNode.Attributes("RecoverToDate").Value = dAllocatedRecoverToDate
                End If
            ElseIf String.IsNullOrEmpty(sPlacement) = False AndAlso sPlacement.Trim.ToUpper = "TREATY QSH" Then

                'Calculate/Retreive Net FAC
                Dim dFACNetSumInsured, dFACNetReserveToDate, dFACNetThisReserve, dFACNetPaymentToDate, dFACNetBalance, dFACNetRecoverToDate As Decimal
                ' Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
                If oNetFACNode IsNot Nothing Then

                    Decimal.TryParse(oNetFACNode.Attributes("SumInsured").Value, dFACNetSumInsured)
                    Decimal.TryParse(oNetFACNode.Attributes("ReserveToDate").Value, dFACNetReserveToDate)
                    Decimal.TryParse(oNetFACNode.Attributes("ThisReserve").Value, dFACNetThisReserve)
                    Decimal.TryParse(oNetFACNode.Attributes("PaymentToDate").Value, dFACNetPaymentToDate)
                    Decimal.TryParse(oNetFACNode.Attributes("Balance").Value, dFACNetBalance)
                    Decimal.TryParse(oNetFACNode.Attributes("RecoverToDate").Value, dFACNetRecoverToDate)

                End If

                'Treaty QSH
                'Calculate/Retreive QSH
                Dim dTotQSHSumInsured, dTotQSHReserveToDate, dTotQSHThisReserve, dTotQSHPaymentToDate, dTotQSHBalance, dTotQSHRecoverToDate As Decimal
                Dim oQSHNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty QSH']")
                If oQSHNodeList IsNot Nothing AndAlso oQSHNodeList.Count > 0 Then
                    For Each oQSHNode As XmlNode In oQSHNodeList
                        Dim iQSHRIArrLineKey As Integer
                        Dim dQSHThisPerc, dQSHSumInsured, dQSHReserveToDate, dQSHThisReserve, dQSHPaymentToDate, dQSHBalance, dQSHRecoverToDate As Decimal
                        Decimal.TryParse(oQSHNode.Attributes("ThisPerc").Value, dQSHThisPerc)
                        Integer.TryParse(oQSHNode.Attributes("RIArrangementLineKey").Value, iQSHRIArrLineKey)


                        If iQSHRIArrLineKey <> iRIArrangementLineKey Then

                            If oNetFACNode IsNot Nothing Then
                                dQSHSumInsured = (dFACNetSumInsured * dQSHThisPerc) / 100
                                dQSHReserveToDate = (dFACNetReserveToDate * dQSHThisPerc) / 100
                                dQSHThisReserve = (dFACNetThisReserve * dQSHThisPerc) / 100
                                dQSHPaymentToDate = (dFACNetPaymentToDate * dQSHThisPerc) / 100
                                dQSHBalance = (dFACNetBalance * dQSHThisPerc) / 100
                                dQSHRecoverToDate = (dFACNetRecoverToDate * dQSHThisPerc) / 100
                            Else
                                dQSHSumInsured = (dBANDSumInsured * dQSHThisPerc) / 100
                                dQSHReserveToDate = (dBANDReserveToDate * dQSHThisPerc) / 100
                                dQSHThisReserve = (dBANDThisReserve * dQSHThisPerc) / 100
                                dQSHPaymentToDate = (dBANDPaymentToDate * dQSHThisPerc) / 100
                                dQSHBalance = (dBANDBalance * dQSHThisPerc) / 100
                                dQSHRecoverToDate = (dBANDRecoverToDate * dQSHThisPerc) / 100
                            End If

                            dTotQSHSumInsured = dTotQSHSumInsured + dQSHSumInsured
                            dTotQSHReserveToDate = dTotQSHReserveToDate + dQSHReserveToDate
                            dTotQSHThisReserve = dTotQSHThisReserve + dQSHThisReserve
                            dTotQSHPaymentToDate = dTotQSHPaymentToDate + dQSHPaymentToDate
                            dTotQSHBalance = dTotQSHBalance + dQSHBalance
                            dTotQSHRecoverToDate = dTotQSHRecoverToDate + dQSHRecoverToDate

                            oQSHNode.Attributes("IsEdited").Value = True
                            oQSHNode.Attributes("SumInsured").Value = dQSHSumInsured
                            oQSHNode.Attributes("ReserveToDate").Value = dQSHReserveToDate
                            oQSHNode.Attributes("ThisReserve").Value = dQSHThisReserve
                            oQSHNode.Attributes("PaymentToDate").Value = dQSHPaymentToDate
                            oQSHNode.Attributes("Balance").Value = dQSHBalance
                            oQSHNode.Attributes("RecoverToDate").Value = dQSHRecoverToDate
                        Else

                            Decimal.TryParse(oQSHNode.Attributes("SumInsured").Value, dQSHSumInsured)
                            Decimal.TryParse(oQSHNode.Attributes("ReserveToDate").Value, dQSHReserveToDate)
                            Decimal.TryParse(oQSHNode.Attributes("ThisReserve").Value, dQSHThisReserve)
                            Decimal.TryParse(oQSHNode.Attributes("PaymentToDate").Value, dQSHPaymentToDate)
                            Decimal.TryParse(oQSHNode.Attributes("Balance").Value, dQSHBalance)
                            Decimal.TryParse(oQSHNode.Attributes("RecoverToDate").Value, dQSHRecoverToDate)

                            dTotQSHSumInsured = dTotQSHSumInsured + dQSHSumInsured
                            dTotQSHReserveToDate = dTotQSHReserveToDate + dQSHReserveToDate
                            dTotQSHThisReserve = dTotQSHThisReserve + dQSHThisReserve
                            dTotQSHPaymentToDate = dTotQSHPaymentToDate + dQSHPaymentToDate
                            dTotQSHBalance = dTotQSHBalance + dQSHBalance
                            dTotQSHRecoverToDate = dTotQSHRecoverToDate + dQSHRecoverToDate

                        End If

                    Next
                End If

                'Treaty XOL
                Dim dXOLTotSumInsured, dXOLTotThisReserve, dXOLTotBalance As Decimal
                Dim oXOLNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Treaty XOL']")
                Dim dXOLSumInsured, dXOLThisReserve, dXOLBalance As Decimal

                If oNetFACNode IsNot Nothing Then
                    dXOLSumInsured = dFACNetSumInsured - dTotQSHSumInsured
                    dXOLThisReserve = dFACNetThisReserve - dTotQSHThisReserve
                    dXOLBalance = dFACNetBalance - dTotQSHBalance
                Else
                    dXOLSumInsured = dBANDSumInsured - dTotQSHSumInsured
                    dXOLThisReserve = dBANDThisReserve - dTotQSHThisReserve
                    dXOLBalance = dBANDBalance - dTotQSHBalance
                End If

                For Each oXOLNode As XmlNode In oXOLNodeList
                    Dim dLowerLimit, dUpperLimit As Decimal

                    Decimal.TryParse(oXOLNode.Attributes("LowerLimit").Value, dLowerLimit)
                    Decimal.TryParse(oXOLNode.Attributes("LineLimit").Value, dUpperLimit)

                    'Sum Insured
                    If dXOLTotSumInsured <= dXOLSumInsured Then
                        If dXOLSumInsured < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = 0

                        ElseIf dXOLSumInsured >= dLowerLimit AndAlso dXOLSumInsured <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLSumInsured - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = dLimitDiff
                            '  dXOLSumInsured = dXOLSumInsured - dLimitDiff
                            dXOLTotSumInsured = dXOLTotSumInsured + dLimitDiff

                        ElseIf dXOLSumInsured > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("SumInsured").Value = dLimitDiff
                            ' dXOLSumInsured = dXOLSumInsured - dLimitDiff
                            dXOLTotSumInsured = dXOLTotSumInsured + dLimitDiff
                        End If
                    End If


                    'This Reserve
                    If dXOLTotThisReserve <= dXOLThisReserve Then
                        If dXOLThisReserve < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = 0

                        ElseIf dXOLThisReserve >= dLowerLimit AndAlso dXOLThisReserve <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLThisReserve - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = dLimitDiff
                            ' dXOLThisReserve = dXOLThisReserve - dLimitDiff
                            dXOLTotThisReserve = dXOLTotThisReserve + dLimitDiff

                        ElseIf dXOLThisReserve > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("ThisReserve").Value = dLimitDiff
                            ' dXOLThisReserve = dXOLThisReserve - dLimitDiff
                            dXOLTotThisReserve = dXOLTotThisReserve + dLimitDiff
                        End If
                    End If

                    'Balance
                    If dXOLTotBalance <= dXOLBalance Then
                        If dXOLBalance < dLowerLimit Then
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = 0

                        ElseIf dXOLBalance >= dLowerLimit AndAlso dXOLBalance <= dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dXOLBalance - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = dLimitDiff
                            '  dXOLBalance = dXOLBalance - dLimitDiff
                            dXOLTotBalance = dXOLTotBalance + dLimitDiff

                        ElseIf dXOLBalance > dUpperLimit Then
                            Dim dLimitDiff As Decimal
                            dLimitDiff = dUpperLimit - dLowerLimit
                            oXOLNode.Attributes("IsEdited").Value = True
                            oXOLNode.Attributes("Balance").Value = dLimitDiff
                            '  dXOLBalance = dXOLBalance - dLimitDiff
                            dXOLTotBalance = dXOLTotBalance + dLimitDiff
                        End If
                    End If
                Next

                'Retained
                Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                Dim oRetNodeList As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Placement='Net']")
                If oNetFACNode IsNot Nothing Then
                    If oRetNodeList IsNot Nothing AndAlso oRetNodeList.Count > 0 Then
                        For Each oRetNode As XmlNode In oRetNodeList
                            Dim dRetThisPerc, dRetSumInsured, dRetReserveToDate, dRetThisReserve, dRetPaymentToDate, dRetBalance, dRetRecoverToDate As Decimal
                            Decimal.TryParse(oRetNode.Attributes("ThisPerc").Value, dRetThisPerc)

                            dRetSumInsured = ((dFACNetSumInsured - (dTotQSHSumInsured + dXOLTotSumInsured)))
                            dRetReserveToDate = ((dFACNetReserveToDate - dTotQSHReserveToDate))
                            dRetThisReserve = ((dFACNetThisReserve - (dXOLTotThisReserve + dTotQSHThisReserve)))
                            dRetPaymentToDate = ((dFACNetPaymentToDate - dTotQSHPaymentToDate))
                            dRetBalance = ((dFACNetBalance - (dXOLTotBalance + dTotQSHBalance)))
                            dRetRecoverToDate = ((dFACNetRecoverToDate - dTotQSHRecoverToDate))

                            oRetNode.Attributes("IsEdited").Value = True
                            oRetNode.Attributes("SumInsured").Value = dRetSumInsured
                            oRetNode.Attributes("ReserveToDate").Value = dRetReserveToDate
                            oRetNode.Attributes("ThisReserve").Value = dRetThisReserve
                            oRetNode.Attributes("PaymentToDate").Value = dRetPaymentToDate
                            oRetNode.Attributes("Balance").Value = dRetBalance
                            oRetNode.Attributes("RecoverToDate").Value = dRetRecoverToDate
                        Next
                    End If
                ElseIf oBandNode IsNot Nothing Then
                    If oRetNodeList IsNot Nothing AndAlso oRetNodeList.Count > 0 Then
                        For Each oRetNode As XmlNode In oRetNodeList
                            Dim dRetThisPerc, dRetSumInsured, dRetReserveToDate, dRetThisReserve, dRetPaymentToDate, dRetBalance, dRetRecoverToDate As Decimal
                            Decimal.TryParse(oRetNode.Attributes("ThisPerc").Value, dRetThisPerc)

                            dRetSumInsured = ((dBANDSumInsured - (dTotQSHSumInsured + dXOLTotSumInsured)))
                            dRetReserveToDate = ((dBANDReserveToDate - dTotQSHReserveToDate))
                            dRetThisReserve = ((dBANDThisReserve - (dXOLTotThisReserve + dTotQSHThisReserve)))
                            dRetPaymentToDate = ((dBANDPaymentToDate - dTotQSHPaymentToDate))
                            dRetBalance = ((dBANDBalance - (dXOLTotBalance + dTotQSHBalance)))
                            dRetRecoverToDate = ((dBANDRecoverToDate - dTotQSHRecoverToDate))

                            oRetNode.Attributes("IsEdited").Value = True
                            oRetNode.Attributes("SumInsured").Value = dRetSumInsured
                            oRetNode.Attributes("ReserveToDate").Value = dRetReserveToDate
                            oRetNode.Attributes("ThisReserve").Value = dRetThisReserve
                            oRetNode.Attributes("PaymentToDate").Value = dRetPaymentToDate
                            oRetNode.Attributes("Balance").Value = dRetBalance
                            oRetNode.Attributes("RecoverToDate").Value = dRetRecoverToDate
                        Next
                    End If
                End If

                'Calculation of Allocated
                Dim xmlNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow")
                Dim dFACTotSumInsured, dFACTotReserveToDate, dFACTotThisReserve, dFACTotPaymentToDate, dFACTotBalance, dFACTotRecoverToDate As Decimal
                For Each oArrangementNode As XmlNode In xmlNodes
                    Dim sArrangementName, sArrangementPlacement As String
                    sArrangementName = oArrangementNode.Attributes("Name").Value
                    sArrangementPlacement = oArrangementNode.Attributes("Placement").Value

                    If sArrangementName = "Net of FAC" Or sArrangementName = "Band Total" _
                    Or sArrangementName = "Allocated" Or sArrangementName = "Unallocated" Then
                        'These will not take part in calculation
                    Else
                        Dim dFACSumInsured, dFACReserveToDate, dFACThisReserve, dFACPaymentToDate, dFACBalance, dFACRecoverToDate As Decimal
                        Decimal.TryParse(oArrangementNode.Attributes("SumInsured").Value, dFACSumInsured)
                        Decimal.TryParse(oArrangementNode.Attributes("ReserveToDate").Value, dFACReserveToDate)
                        Decimal.TryParse(oArrangementNode.Attributes("ThisReserve").Value, dFACThisReserve)
                        Decimal.TryParse(oArrangementNode.Attributes("PaymentToDate").Value, dFACPaymentToDate)
                        Decimal.TryParse(oArrangementNode.Attributes("Balance").Value, dFACBalance)
                        Decimal.TryParse(oArrangementNode.Attributes("RecoverToDate").Value, dFACRecoverToDate)

                        dFACTotSumInsured += dFACSumInsured
                        dFACTotReserveToDate += dFACReserveToDate
                        dFACTotThisReserve += dFACThisReserve
                        dFACTotPaymentToDate += dFACPaymentToDate
                        dFACTotBalance += dFACBalance
                        dFACTotRecoverToDate += dFACRecoverToDate
                    End If

                Next

                'Calculate/Retreive Allocated Total
                If oAllocatedNode IsNot Nothing Then
                    dAllocatedSumInsured = dFACTotSumInsured
                    dAllocatedReserveToDate = dFACTotReserveToDate
                    dAllocatedThisReserve = dFACTotThisReserve
                    dAllocatedPaymentToDate = dFACTotPaymentToDate
                    dAllocatedBalance = dFACTotBalance
                    dAllocatedRecoverToDate = dFACTotRecoverToDate

                    oAllocatedNode.Attributes("SumInsured").Value = dAllocatedSumInsured
                    oAllocatedNode.Attributes("ReserveToDate").Value = dAllocatedReserveToDate
                    oAllocatedNode.Attributes("ThisReserve").Value = dAllocatedThisReserve
                    oAllocatedNode.Attributes("PaymentToDate").Value = dAllocatedPaymentToDate
                    oAllocatedNode.Attributes("Balance").Value = dAllocatedBalance
                    oAllocatedNode.Attributes("RecoverToDate").Value = dAllocatedRecoverToDate
                End If
            End If

            'Find UnAllocated
            Dim dUnAllocatedSumInsured, dUnAllocatedReserveToDate, dUnAllocatedThisReserve, dUnAllocatedPaymentToDate, dUnAllocatedRecoverToDate, dUnAllocatedBalance As Decimal
            Dim oUnAllocatedNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Unallocated']")

            'Calculation of UnAllocated
            dUnAllocatedSumInsured = dBANDSumInsured - dAllocatedSumInsured
            dUnAllocatedReserveToDate = dBANDReserveToDate - dAllocatedReserveToDate
            dUnAllocatedThisReserve = dBANDThisReserve - dAllocatedThisReserve
            dUnAllocatedPaymentToDate = dBANDPaymentToDate - dAllocatedPaymentToDate
            dUnAllocatedRecoverToDate = dBANDRecoverToDate - dAllocatedRecoverToDate
            dUnAllocatedBalance = dBANDBalance - dAllocatedBalance

            'UnAllocated Not Found
            If oUnAllocatedNode Is Nothing Then
                If dUnAllocatedSumInsured <> 0 Or dUnAllocatedReserveToDate <> 0 Or dUnAllocatedThisReserve <> 0 Or dUnAllocatedPaymentToDate <> 0 _
Or dUnAllocatedBalance <> 0 Or dUnAllocatedRecoverToDate <> 0 Then

                    'Add into the XML
                    Dim sArrangementRow As String = "ArrangementRow"
                    Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)

                    'Adding all the attributes
                    For Each oAttribute As XmlAttribute In oNode.Attributes
                        ArrangementRow.SetAttribute(oAttribute.Name, "")
                    Next

                    ArrangementRow.Attributes("Name").Value = "Unallocated"
                    ArrangementRow.Attributes("SumInsured").Value = dUnAllocatedSumInsured
                    ArrangementRow.Attributes("ThisReserve").Value = dUnAllocatedThisReserve
                    ArrangementRow.Attributes("ReserveToDate").Value = dUnAllocatedReserveToDate
                    ArrangementRow.Attributes("PaymentToDate").Value = dUnAllocatedPaymentToDate
                    ArrangementRow.Attributes("Balance").Value = dUnAllocatedBalance
                    ArrangementRow.Attributes("RecoverToDate").Value = dUnAllocatedRecoverToDate

                    'Add it at the last row
                    oRootNode.InsertAfter(ArrangementRow, oRootNode.LastChild)
                End If
            ElseIf dUnAllocatedSumInsured = 0 And dUnAllocatedReserveToDate = 0 And dUnAllocatedThisReserve = 0 And dUnAllocatedPaymentToDate = 0 _
And dUnAllocatedBalance = 0 And dUnAllocatedRecoverToDate = 0 Then
                'Remove the UnAllocated if it is Fully Allocated
                oRootNode.RemoveChild(oUnAllocatedNode)
            Else
                'Update the UnAllocate with New Values
                oUnAllocatedNode.Attributes("SumInsured").Value = dUnAllocatedSumInsured
                oUnAllocatedNode.Attributes("ThisReserve").Value = dUnAllocatedThisReserve
                oUnAllocatedNode.Attributes("ReserveToDate").Value = dUnAllocatedReserveToDate
                oUnAllocatedNode.Attributes("PaymentToDate").Value = dUnAllocatedPaymentToDate
                oUnAllocatedNode.Attributes("Balance").Value = dUnAllocatedBalance
                oUnAllocatedNode.Attributes("RecoverToDate").Value = dUnAllocatedRecoverToDate
            End If
        End If

        'Update the XML before returning
        Dim swClaimContent As New System.IO.StringWriter
        Dim xmlwClaimContent As New XmlTextWriter(swClaimContent)

        oXMLDoc.WriteTo(xmlwClaimContent)
        sXML = swClaimContent.ToString()

        xmlwClaimContent.Close()
        swClaimContent.Close()
    End Sub

    Public Sub CalculateFACQSH(ByVal oTSHNode As XmlNode, ByVal sRIBANDID As String, ByVal bIsObligatory As Boolean, ByRef iMAXTRIID As Integer,
                               ByRef drunning_si As Double, ByVal dband_si As Double, ByRef drunning_premium As Double, ByVal dGrossSI As Double,
                               ByVal dGrossPremium As Double, ByVal dRIArrangementSI As Double, ByRef dfac_premium As Double, ByRef dfac_si As Double,
                               ByVal dband_premium As Double, ByRef dTQSHPremium As Double, ByRef dTQSHSSI As Double)

        ' Update MAX RI Line ID
        Dim currentLineKey = Convert.ToInt32(oTSHNode.Attributes("RIArrangementLineKey").Value)
        If currentLineKey > iMAXTRIID Then iMAXTRIID = currentLineKey

        ' PBI 35359: If this obligatory T-type line was edited by the user (IsEditedDB=True),
        ' preserve its persisted SI/Premium instead of recalculating - same behaviour as
        ' the IsEditedDB guard applied to non-obligatory priority nodes in Recalculate.
        Dim bIsEditedDB As Boolean = False
        If oTSHNode.Attributes("IsEditedDB") IsNot Nothing Then
            Boolean.TryParse(oTSHNode.Attributes("IsEditedDB").Value, bIsEditedDB)
        End If
        If bIsEditedDB Then
            ' Preserve persisted SI; recalculate premium proportionally from the edited SI.
            If oTSHNode.Attributes("SumInsured") Is Nothing OrElse oTSHNode.Attributes("Premium") Is Nothing Then
                Throw New InvalidOperationException(String.Format("TSH node (key={0}) is missing required 'SumInsured' or 'Premium' attribute. Cannot preserve persisted values.", If(oTSHNode.Attributes("RIArrangementLineKey") IsNot Nothing, oTSHNode.Attributes("RIArrangementLineKey").Value, "unknown")))
            End If
            Dim dPersistedSI As Double = 0
            Double.TryParse(oTSHNode.Attributes("SumInsured").Value, dPersistedSI)
            Dim dPersistedPremium As Double = 0
            Dim bIsPremiumEdited As Boolean = False
            If oTSHNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                Boolean.TryParse(oTSHNode.Attributes("IsPremiumEdited").Value, bIsPremiumEdited)
            End If
            If bIsPremiumEdited Then
                ' User directly edited premium — honour it as-is
                Double.TryParse(oTSHNode.Attributes("Premium").Value, dPersistedPremium)
            Else
                ' SI was edited — recalculate premium proportionally: premium = (SI / GrossSI) * GrossPremium
                dPersistedPremium = If(dGrossSI <> 0, Math.Round(dPersistedSI / dGrossSI * dGrossPremium, 2), 0)
                oTSHNode.Attributes("Premium").Value = dPersistedPremium.ToString(System.Globalization.CultureInfo.InvariantCulture)
            End If
            ' Recalculate derived fields (Commission, Tax) from recalculated premium
            Dim dCommPerc As Double
            Double.TryParse(oTSHNode.Attributes("CommissionPerc").Value, dCommPerc)
            oTSHNode.Attributes("CommissionPerc").Value = Format(dCommPerc, "0.0000")
            oTSHNode.Attributes("Commission").Value = Format(dPersistedPremium * dCommPerc / 100, "0.00")
            Dim dTaxPerc As Double = GetTaxPercentage(oTSHNode, "Premium", "Tax")
            oTSHNode.Attributes("Tax").Value = Format((dTaxPerc * dPersistedPremium) / 100, "0.00")
            Dim dCommTaxPerc As Double = GetTaxPercentage(oTSHNode, "Commission", "CommissionTax")
            oTSHNode.Attributes("CommissionTax").Value = Format((Convert.ToDouble(oTSHNode.Attributes("Commission").Value) * dCommTaxPerc) / 100, "0.00")
            ' Update running totals
            dTQSHPremium += dPersistedPremium
            dTQSHSSI += dPersistedSI
            drunning_si -= dPersistedSI
            drunning_premium -= dPersistedPremium
            Return
        End If

        ' Get values from XML
        Dim dline_limit = Convert.ToDouble(oTSHNode.Attributes("LineLimit").Value)
        Dim ddefault_percent = Convert.ToDouble(oTSHNode.Attributes("DefaultPerc").Value)
        oTSHNode.Attributes("DefaultPerc").Value = Format(ddefault_percent, "0.00")

        ' Calculate SI and Premium
        Dim dthis_si, dthis_premium, dpriority_share As Double

        If dband_si = 0 Then
            dthis_si = 0
            dthis_premium = drunning_premium * ddefault_percent / 100
        ElseIf bIsObligatory Then
            dthis_si = dGrossSI * ddefault_percent / 100
            dpriority_share = dthis_si / dband_si
            dthis_premium = dGrossPremium * ddefault_percent / 100
        ElseIf drunning_si >= dline_limit Then
            dthis_si = Math.Min(dline_limit * ddefault_percent / 100, drunning_si - dTQSHSSI)
            dpriority_share = If((dband_si - dfac_si) > 0, dthis_si / (dband_si - dfac_si), 0)
            dthis_premium = (dband_premium - dfac_premium) * dpriority_share
        Else
            ' Running SI < Line Limit
            Dim availableSI = drunning_si - dTQSHSSI
            If availableSI - (drunning_si * ddefault_percent / 100) < 0 Then
                dthis_si = availableSI * ddefault_percent / 100
                dthis_premium = (drunning_premium - dTQSHPremium) * ddefault_percent / 100
            Else
                dthis_si = drunning_si * ddefault_percent / 100
                dthis_premium = drunning_premium * ddefault_percent / 100
            End If
            dpriority_share = If(drunning_si > 0, dthis_si / drunning_si, 0)
        End If
        'dthis_premium = dthis_si * dpriority_share

        ' Calculate and set tax and commission values
        Dim dTQS_taxperc = GetTaxPercentage(oTSHNode, "Premium", "Tax")
        Dim dTQS_Commperc = Convert.ToDouble(oTSHNode.Attributes("CommissionPerc").Value)
        Dim dTQS_CommTaxperc = GetTaxPercentage(oTSHNode, "Commission", "CommissionTax")
        ' Update XML attributes
        oTSHNode.Attributes("ThisPerc").Value = If(Double.IsNaN(dpriority_share), "0.0000", (dpriority_share * 100).ToString())
        oTSHNode.Attributes("SumInsured").Value = dthis_si
        oTSHNode.Attributes("Premium").Value = If(Double.IsNaN(dthis_premium), "0.0000", dthis_premium.ToString())

        oTSHNode.Attributes("CommissionPerc").Value = Format(dTQS_Commperc, "0.0000")
        oTSHNode.Attributes("Tax").Value = (dTQS_taxperc * dthis_premium) / 100
        oTSHNode.Attributes("Commission").Value = (dthis_premium * dTQS_Commperc) / 100
        oTSHNode.Attributes("CommissionTax").Value = (Convert.ToDouble(oTSHNode.Attributes("Commission").Value) * dTQS_CommTaxperc) / 100

        ' Update running totals
        dTQSHPremium += dthis_premium
        dTQSHSSI += dthis_si
        drunning_si = drunning_si - dthis_si
        drunning_premium = drunning_premium - dthis_premium
    End Sub

    Public Sub ApplyManualTreatyFinancials(ByVal oNode As XmlNode, ByVal dPremium As Double, ByVal dTaxPercentage As Double, Optional ByVal dCommPercOverride As Double = -1)
        oNode.Attributes("Tax").Value = (dTaxPercentage * dPremium) / 100
        Dim dCommPerc As Double = dCommPercOverride
        If dCommPerc < 0 Then
            dCommPerc = 0
            If oNode.Attributes("CommissionPerc") IsNot Nothing Then
                Double.TryParse(oNode.Attributes("CommissionPerc").Value, dCommPerc)
            End If
        End If
        oNode.Attributes("CommissionPerc").Value = Format(dCommPerc, "0.0000")
        Dim dCommission As Double = (dPremium * dCommPerc) / 100
        oNode.Attributes("Commission").Value = dCommission.ToString()
        Dim dCommTaxPerc As Double = dTaxPercentage
        If oNode.Attributes("CommissionTaxPerc") IsNot Nothing Then
            Double.TryParse(oNode.Attributes("CommissionTaxPerc").Value, dCommTaxPerc)
        End If
        oNode.Attributes("CommissionTax").Value = (dCommission * dCommTaxPerc) / 100
    End Sub

    Private Function GetTaxPercentage(node As XmlNode, baseField As String, taxField As String) As Double
        Dim baseValue = Convert.ToDouble(node.Attributes(baseField).Value)
        Dim taxValue = Convert.ToDouble(node.Attributes(taxField).Value)
        Return If(baseValue = 0 OrElse taxValue = 0, 0, (taxValue * 100) / baseValue)
    End Function

    ''' <summary>
    ''' Compares the new value for a given attribute against the original (non-mutated) XML from the DB.
    ''' Returns True if the value has actually changed (meaning IsEditedDB should be set).
    ''' If original XML is not provided or the node is not found in the original, returns True (safe default).
    ''' </summary>
    Public Function HasValueChangedFromOriginal(sOriginalXML As String, sRIBANDID As String, sRIArrangementLineKey As String, sAttributeName As String, sNewValue As String, Optional sTreatyCode As String = "") As Boolean
        If String.IsNullOrEmpty(sOriginalXML) Then Return True
        Try
            Dim oOrigDoc As New XmlDocument()
            oOrigDoc.LoadXml(sOriginalXML)
            Dim oOrigNode As XmlNode = Nothing
            If Not String.IsNullOrEmpty(sRIArrangementLineKey) AndAlso sRIArrangementLineKey <> "0" Then
                oOrigNode = oOrigDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim() & "']")
            ElseIf Not String.IsNullOrEmpty(sTreatyCode) Then
                oOrigNode = oOrigDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@TreatyCode='" & sTreatyCode.Replace("'", "''") & "']")
            End If
            If oOrigNode Is Nothing Then Return True
            If oOrigNode.Attributes(sAttributeName) Is Nothing Then Return True
            Dim sOrigValue As String = oOrigNode.Attributes(sAttributeName).Value
            ' Compare as rounded doubles for numeric fields
            Dim dOrig As Double, dNew As Double
            If Double.TryParse(sOrigValue, dOrig) AndAlso Double.TryParse(sNewValue, dNew) Then
                Return Math.Round(dOrig, 4) <> Math.Round(dNew, 4)
            End If
            Return Not String.Equals(sOrigValue.Trim(), sNewValue.Trim(), StringComparison.OrdinalIgnoreCase)
        Catch
            Return True
        End Try
    End Function
    Public Function UpdatePriorityValues(
        ByRef ilast_priority As Integer,
        nPriority As Integer,
        dline_limit As Double,
        inumber_of_lines As Decimal,
        drunning_si As Double,
        drunning_premium As Double,
        dgrossnet_SI As Double,
        dgrossnet_Premium As Double,
        bIsExtendedLimitApplied As Boolean,
        dExtendedLimitAmount As Double,
        ByRef dpriority_limit As Double,
        ByRef dpriority_si As Double,
        ByRef dpriority_premium As Double,
        ByRef QSTotal As Double
    ) As Boolean

        If ilast_priority <> nPriority Then
            ilast_priority = nPriority
            dpriority_limit = dline_limit
            dpriority_si = drunning_si
            dpriority_premium = drunning_premium
            QSTotal = 0

            If dline_limit = 0 Then
                dpriority_limit = dgrossnet_SI
            End If

            If bIsExtendedLimitApplied AndAlso dExtendedLimitAmount > 0 Then
                If dpriority_limit > dExtendedLimitAmount Then
                    dpriority_limit = dExtendedLimitAmount
                End If
            End If
            Return True
        End If

        Return False
    End Function
    Public Function CalculateProportionalTreaty(sTreatyType As String,
    iTreatyTypeID As Integer,
    dpriority_si As Double,
    dpriority_limit As Double,
    ddefault_percent As Double,
    dgrossnet_SI As Double,
    dgrossnet_premium As Double,
    dband_si As Double,
    dPriority_allocated_SI As Double,
    dPriority_allocated_Premium As Double,
    QSTotal As Double,
    inumber_of_lines As Decimal
) As (Double, Double, Double, Double, Double, Double)

        Dim dthis_si As Double = 0, dthis_premium As Double = 0, dpriority_share As Double = 0

        dthis_si = Math.Max(0, Math.Min(dpriority_si * ddefault_percent / 100, dpriority_limit * inumber_of_lines * (ddefault_percent / 100)))
        dpriority_share = If(dband_si = 0, 0, dthis_si / dgrossnet_SI)
        dthis_premium = If(dgrossnet_SI <> 0, dgrossnet_premium * dpriority_share, 0)
        If dthis_si > 0 OrElse dthis_premium > 0 Then
            dPriority_allocated_SI += dthis_si
            dPriority_allocated_Premium += dthis_premium
        End If
        If sTreatyType = "T" Then
            QSTotal += dthis_si
        End If
        Return (dthis_si, dthis_premium, dPriority_allocated_SI, dPriority_allocated_Premium, dpriority_share, QSTotal)
    End Function
    Public Function CalculateNonProportionalTreaty(
    iTreatyTypeID As Integer,
    icede_premium_only As Integer,
    treatyCode As String,
    dpriority_limit As Double,
    dline_limit As Double,
    QSTotal As Double,
    dlower_limit As Double,
    inegative_si As Integer,
    dpriority_si As Double,
    dpriority_premium As Double,
    dceding_rate As Double,
    isPortfolioTransfer As Boolean
) As (Double, Double)

        Dim dthis_si As Double = 0
        Dim dthis_premium As Double = 0
        If icede_premium_only = 0 AndAlso treatyCode <> "TC" Then
            If dpriority_limit - QSTotal < dline_limit AndAlso QSTotal <> 0 Then
                dline_limit = dpriority_limit - QSTotal
            End If
            If (dpriority_si - QSTotal) < dline_limit Then
                If (dpriority_si - QSTotal) > dlower_limit Then
                    dthis_si = Math.Max(0, dpriority_si - QSTotal - (dlower_limit * inegative_si))
                End If
            Else
                dthis_si = Math.Max(0, (dline_limit - dlower_limit) * inegative_si)
            End If
            'IN CASE OF PORTFOLIO TRANSFER ONLY PREMIUM IS SET TO 0
            'dthis_si = If(isPortfolioTransfer, 0, dthis_si)
            dthis_premium = If(isPortfolioTransfer, 0, (dpriority_premium * dceding_rate / 100))
        End If
        Return (dthis_si, dthis_premium)
    End Function
    Public Sub RecalculateForTreatyPremium(ByRef sXML As String,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIarrangementKey As String)

        Dim oXMLDoc As New XmlDocument()
        oXMLDoc.LoadXml(sXML)

        ' Get gross and fac premium/SI from XML
        Dim dGrossPremium As Double = 0
        Dim dFacPremium As Double = 0
        Dim dObligatoryPremium As Double = 0
        Dim dNetPremium As Double = 0
        Dim dRunningPremium As Double = 0
        Dim iRetLineId As Integer = 0
        Dim maxIterations As Integer = 100
        Dim iterationCount As Integer = 0
        Dim dGrossSI As Double = 0
        Dim dFacSI As Double = 0
        Dim dObligatorySI As Double = 0
        Dim dNetSI As Double = 0

        ' Get gross premium and SI from arrangement node
        Dim oArrangementNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
        If oArrangementNode IsNot Nothing Then
            dGrossPremium = Convert.ToDouble(oArrangementNode.Attributes("Premium").Value)
            dGrossSI = Convert.ToDouble(oArrangementNode.Attributes("SumInsured").Value)
        End If

        ' FAC (F/FX only) -- mirrors SP: @fac_premium = SUM where TYPE IN ('F','FX')
        Dim oFacNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIarrangementKey='" & sRIarrangementKey & "' and (@Type='F' or @Type='FX') and not(@IsDeleted='True')]")
        For Each oNode As XmlNode In oFacNodes
            dFacPremium += Convert.ToDouble(oNode.Attributes("Premium").Value)
            dFacSI += Convert.ToDouble(oNode.Attributes("SumInsured").Value)
        Next

        ' Obligatory T -- mirrors SP: @obligatory_premium = SUM where TYPE='T' AND Is_Obligatory=1
        Dim oObligNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIarrangementKey='" & sRIarrangementKey & "' and @Type='T' and (@IsObligatory='True' or @IsObligatory='1')]")
        For Each oObligNode As XmlNode In oObligNodes
            dObligatoryPremium += Convert.ToDouble(oObligNode.Attributes("Premium").Value)
            dObligatorySI += Convert.ToDouble(oObligNode.Attributes("SumInsured").Value)
        Next

        ' net_premium = gross - obligatory - fac  (mirrors SP exactly)
        dNetPremium = dGrossPremium - dObligatoryPremium - dFacPremium
        dNetSI = dGrossSI - dObligatorySI - dFacSI
        dRunningPremium = dNetPremium

        ' Create temp table for RI premiums
        Dim dtRIPremiums As New DataTable()
        dtRIPremiums.Columns.Add("RIArrangementLineKey", GetType(Integer))
        dtRIPremiums.Columns.Add("TreatyID", GetType(Integer))
        dtRIPremiums.Columns.Add("Type", GetType(String))
        dtRIPremiums.Columns.Add("RIModelLineID", GetType(Integer))
        dtRIPremiums.Columns.Add("LowerLimit", GetType(Double))
        dtRIPremiums.Columns.Add("PremiumPerc", GetType(Double))
        dtRIPremiums.Columns.Add("TreatyTypeID", GetType(Integer))
        dtRIPremiums.Columns.Add("Premium", GetType(Double))
        dtRIPremiums.Columns.Add("ThisSharePerc", GetType(Double))
        dtRIPremiums.Columns.Add("PremiumCalcBasisID", GetType(Integer))
        dtRIPremiums.Columns.Add("CalculationFactors", GetType(String))
        dtRIPremiums.Columns.Add("CalculatedInIteration", GetType(Boolean))
        dtRIPremiums.Columns.Add("IsCalculated", GetType(Boolean))
        dtRIPremiums.Columns.Add("IsQSR", GetType(Boolean))
        dtRIPremiums.Columns.Add("IsPremiumEdited", GetType(Boolean))
        dtRIPremiums.Columns.Add("XmlNode", GetType(XmlNode))

        ' Load RI lines -- mirrors SP: obligatory T lines included (Is_Obligatory filter commented out in SP).
        ' Manually added treaties excluded; their premiums are user-entered and accounted for in retained absorption.
        ' FIX 1: Exclude obligatory T lines (already pre-deducted from gross to derive net_premium).
        ' FIX 2: Exclude non-CAT/XOL lines with zero SI (no risk ceded, no premium to calculate).
        Dim oRINodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIarrangementKey='" & sRIarrangementKey & "' and (@Type='R' or @Type='T' or @Type='TX' or @Type='TC' or @Type='TFS' or @Type='PX') and (not(@ManuallyAdded) or @ManuallyAdded='False') and @IsDeleted!='True' and not(@Type='T' and (@IsObligatory='True' or @IsObligatory='1')) and (@ReinsuranceTypeCode!='QSR' or not(@ReinsuranceTypeCode))]")
        ' Note: zero-SI guard for non-CAT lines is enforced below via ThisSharePerc=0 when SumInsured=0
        For Each oNode As XmlNode In oRINodes
            ' Skip lines with empty CalculationFactors (mirrors SP: ISNULL(pcb.calculation_Factors, '') <> '')
            Dim sCalcFactorsCheck As String = If(oNode.Attributes("CalculationFactors") IsNot Nothing, oNode.Attributes("CalculationFactors").Value, "")
            Dim sType As String = oNode.Attributes("Type").Value

            If String.IsNullOrEmpty(sCalcFactorsCheck) AndAlso sType <> "R" Then Continue For
            Dim dThisSharePerc As Double

            ' Apply logic from spu_RI_treaty_premium_calc_RI2007
            Dim dParsedThisPerc As Double = 0
            If oNode.Attributes("ThisPerc") IsNot Nothing Then
                Double.TryParse(oNode.Attributes("ThisPerc").Value, dParsedThisPerc)
            End If

            If sType = "TX" OrElse sType = "TC" OrElse sType = "PX" Then
                ' For TX, TC and PX types, use ceding_rate (DefaultPerc)
                dThisSharePerc = If(oNode.Attributes("DefaultPerc") IsNot Nothing, Convert.ToDouble(oNode.Attributes("DefaultPerc").Value), 0)
            ElseIf (oNode.Attributes("ThisPerc") Is Nothing OrElse dParsedThisPerc = 0) AndAlso Math.Abs(dNetSI) < 0.0001 Then
                ' If ThisPerc is null or 0 AND net SI is ~0, use DefaultPerc
                dThisSharePerc = If(oNode.Attributes("DefaultPerc") IsNot Nothing, Convert.ToDouble(oNode.Attributes("DefaultPerc").Value), 0)
            Else
                ' Otherwise use ThisPerc (ISNULL equivalent - treat Nothing as 0)
                dThisSharePerc = dParsedThisPerc
            End If

            ' Determine if this is a QSR line -- mirrors SP: CASE WHEN ISNULL(pcb.reinsurance_type_id, 0) = 14 THEN 1 ELSE 0 END
            Dim bIsQSR As Boolean = oNode.Attributes("ReinsuranceTypeCode") IsNot Nothing AndAlso oNode.Attributes("ReinsuranceTypeCode").Value = "QSR"
            If bIsQSR Then
                ' QSR: always use DefaultPerc for ThisSharePerc
                dThisSharePerc = If(oNode.Attributes("DefaultPerc") IsNot Nothing, Convert.ToDouble(oNode.Attributes("DefaultPerc").Value), 0)
            End If

            ' Skip lines where the computed this_share_percent is zero (no premium to calculate)
            ' For TX/TC/PX this uses DefaultPerc (ceding_rate); for others ThisPerc
            ' Exclude R type: its premium is set by retained absorption, not iterative calc
            ' Exclude QSR lines (is_qsr=1): their premium is split from retained using default_share_percent -- mirrors SP DELETE
            If dThisSharePerc = 0 AndAlso sType <> "R" AndAlso Not bIsQSR Then Continue For

            Dim bIsPremiumEditedSeed As Boolean = False
            If oNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                Boolean.TryParse(oNode.Attributes("IsPremiumEdited").Value, bIsPremiumEditedSeed)
            End If
            Dim dSeedPremium As Double = 0
            If bIsPremiumEditedSeed Then
                Double.TryParse(oNode.Attributes("Premium").Value, dSeedPremium)
            End If

            dtRIPremiums.Rows.Add(
                Convert.ToInt32(oNode.Attributes("RIArrangementLineKey").Value),
                If(oNode.Attributes("TreatyID") IsNot Nothing, Convert.ToInt32(oNode.Attributes("TreatyID").Value), 0),
                oNode.Attributes("Type").Value,
                If(oNode.Attributes("RIModelLineID") IsNot Nothing, Convert.ToInt32(oNode.Attributes("RIModelLineID").Value), 0),
                If(oNode.Attributes("LowerLimit") IsNot Nothing, Convert.ToDouble(oNode.Attributes("LowerLimit").Value), 0),
                0,
                If(oNode.Attributes("TreatyTypeID") IsNot Nothing, Convert.ToInt32(oNode.Attributes("TreatyTypeID").Value), 0),
                dSeedPremium,
                dThisSharePerc,
                If(oNode.Attributes("PremiumCalcBasisID") IsNot Nothing, Convert.ToInt32(oNode.Attributes("PremiumCalcBasisID").Value), 0),
                If(oNode.Attributes("CalculationFactors") IsNot Nothing, oNode.Attributes("CalculationFactors").Value, ""),
                False,
                False,
                bIsQSR,
                bIsPremiumEditedSeed,
                oNode
            )

            If oNode.Attributes("Type").Value = "R" Then
                iRetLineId = Convert.ToInt32(oNode.Attributes("RIArrangementLineKey").Value)
            End If
        Next

        ' Create temp table for factors
        Dim dtFactors As New DataTable()
        dtFactors.Columns.Add("Type", GetType(String))
        dtFactors.Columns.Add("Amount", GetType(Double))

        ' Insert initial factors G and G,F
        dtFactors.Rows.Add("G", dGrossPremium)
        dtFactors.Rows.Add("G,F", dNetPremium)

        ' Seed derived factors for TX/TC/PX nodes that were skipped (empty CalculationFactors)
        ' but have a non-zero premium. This ensures lines depending on e.g. G,F,TX can calculate.
        Dim oExtNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIarrangementKey='" & sRIarrangementKey & "' and (@Type='TX' or @Type='TC' or @Type='PX') and @IsDeleted!='True' and (not(@ManuallyAdded) or @ManuallyAdded='False')]")
        For Each oExtNode As XmlNode In oExtNodes
            Dim sExtCalcFactors As String = If(oExtNode.Attributes("CalculationFactors") IsNot Nothing, oExtNode.Attributes("CalculationFactors").Value, "")
            If Not String.IsNullOrEmpty(sExtCalcFactors) Then Continue For  ' already in dtRIPremiums
            Dim dExtPremium As Double = 0
            If oExtNode.Attributes("Premium") IsNot Nothing Then Double.TryParse(oExtNode.Attributes("Premium").Value, dExtPremium)
            If dExtPremium = 0 Then Continue For
            Dim sExtType As String = oExtNode.Attributes("Type").Value
            ' Derive factor combinations: e.g. G,F -> G,F,TX = G,F amount - TX premium
            Dim factorSnapshot As DataRow() = dtFactors.Select()
            For Each fRow As DataRow In factorSnapshot
                Dim sComposite As String = fRow("Type").ToString() & "," & sExtType
                If dtFactors.Select("Type = '" & sComposite & "'").Length = 0 Then
                    dtFactors.Rows.Add(sComposite, Convert.ToDouble(fRow("Amount")) - dExtPremium)
                End If
            Next
        Next

        ' IsPremiumEdited lines: premium is pre-seeded, they participate in iteration normally
        ' Initialize running_premium
        dRunningPremium = dNetPremium

        ' START LOOP for iterative calculation
        While iterationCount < maxIterations
            iterationCount += 1

            ' Reset calculated_in_iteration flag
            For Each row As DataRow In dtRIPremiums.Rows
                row("CalculatedInIteration") = False
            Next

            ' Calculate premiums for lines whose factor is available
            ' AND all contributing types that will produce premium are done (readiness guard)
            For Each riRow As DataRow In dtRIPremiums.Select("IsCalculated = False AND IsQSR = False")
                Dim sCalcFactors As String = riRow("CalculationFactors").ToString()
                Dim factorRows() As DataRow = dtFactors.Select("Type = '" & sCalcFactors & "'")

                If factorRows.Length > 0 Then
                    ' Check readiness: block until all lines whose mapped type feeds into this factor are done
                    Dim bReady As Boolean = True
                    For Each rp2 As DataRow In dtRIPremiums.Select("IsCalculated = False")
                        If Convert.ToInt32(rp2("RIArrangementLineKey")) = Convert.ToInt32(riRow("RIArrangementLineKey")) Then Continue For
                        If Convert.ToDouble(rp2("ThisSharePerc")) = 0 Then Continue For  ' skip lines that won't produce premium
                        Dim sRp2Type As String = rp2("Type").ToString()
                        Dim bPToken As Boolean = (sCalcFactors = "P" OrElse sCalcFactors.EndsWith(",P") OrElse sCalcFactors.Contains(",P,"))
                        Dim bTXToken As Boolean = (sCalcFactors = "TX" OrElse sCalcFactors.Contains(",TX"))
                        Dim bTCToken As Boolean = (sCalcFactors = "TC" OrElse sCalcFactors.Contains(",TC"))
                        Dim bPXToken As Boolean = (sCalcFactors = "PX" OrElse sCalcFactors.Contains(",PX"))
                        Dim bRToken As Boolean = (sCalcFactors = "R" OrElse sCalcFactors.EndsWith(",R") OrElse sCalcFactors.Contains(",R,"))
                        If (bPToken AndAlso (sRp2Type = "T" OrElse sRp2Type = "TFS")) OrElse
                           (bTXToken AndAlso sRp2Type = "TX") OrElse
                           (bTCToken AndAlso sRp2Type = "TC") OrElse
                           (bPXToken AndAlso sRp2Type = "PX") OrElse
                           (bRToken AndAlso sRp2Type = "R") Then
                            bReady = False
                            Exit For
                        End If
                    Next

                    If bReady Then
                        Dim dFactorAmount As Double = Convert.ToDouble(factorRows(0)("Amount"))
                        Dim dSharePerc As Double = Convert.ToDouble(riRow("ThisSharePerc"))
                        Dim dCalcPremium As Double = dFactorAmount * (dSharePerc / 100.0)

                        ' Skip premium recalculation if user edited, keep seeded value
                        If Not Convert.ToBoolean(riRow("IsPremiumEdited")) Then
                            riRow("Premium") = dCalcPremium
                        End If
                        riRow("CalculatedInIteration") = True
                        riRow("IsCalculated") = True
                    End If
                End If
            Next

            ' Check if any uncalculated lines remain
            If dtRIPremiums.Select("IsCalculated = False").Length = 0 Then Exit While

            ' Create temp table for current iteration factors - GROUP BY type
            Dim dtCurrentFactor As New DataTable()
            dtCurrentFactor.Columns.Add("Type", GetType(String))
            dtCurrentFactor.Columns.Add("TotalPremium", GetType(Double))

            ' Group by type (T/TFS -> P) and sum premiums - matching SQL GROUP BY
            For Each riRow As DataRow In dtRIPremiums.Select("CalculatedInIteration = True")
                Dim sType As String = riRow("Type").ToString()
                Dim dPremium As Double = Convert.ToDouble(riRow("Premium"))
                Dim sFactorType As String = If(sType = "T" OrElse sType = "TFS", "P", sType)

                Dim existingRows() As DataRow = dtCurrentFactor.Select("Type = '" & sFactorType & "'")
                If existingRows.Length > 0 Then
                    existingRows(0)("TotalPremium") = Convert.ToDouble(existingRows(0)("TotalPremium")) + dPremium
                Else
                    dtCurrentFactor.Rows.Add(sFactorType, dPremium)
                End If
            Next

            ' Insert new derived factors - using cursor logic from SQL
            For Each cfRow As DataRow In dtCurrentFactor.Rows
                Dim sFactorType As String = cfRow("Type").ToString()
                Dim dFactorAmount As Double = Convert.ToDouble(cfRow("TotalPremium"))

                Dim factorRowsCopy As DataRow() = dtFactors.Select()
                For Each fRow As DataRow In factorRowsCopy
                    Dim sBaseType As String = fRow("Type").ToString()
                    Dim sNewType As String = sBaseType & "," & sFactorType
                    Dim dBaseAmount As Double = Convert.ToDouble(fRow("Amount"))

                    Dim existingFactorRows() As DataRow = dtFactors.Select("Type = '" & sNewType & "'")
                    If existingFactorRows.Length > 0 Then
                        ' Cumulatively update existing factor key by subtracting new amount
                        existingFactorRows(0)("Amount") = Convert.ToDouble(existingFactorRows(0)("Amount")) - dFactorAmount
                    Else
                        dtFactors.Rows.Add(sNewType, dBaseAmount - dFactorAmount)
                    End If
                Next
            Next

            ' Seed R factor once R line is calculated and remaining lines depend on R
            ' Mirrors the SQL proc's explicit R-factor seeding block
            Dim rCalculatedRows() As DataRow = dtRIPremiums.Select("Type = 'R' AND IsCalculated = True")
            If rCalculatedRows.Length > 0 AndAlso dtFactors.Select("Type = 'R'").Length = 0 Then
                Dim dRAmount As Double = Convert.ToDouble(rCalculatedRows(0)("Premium"))
                dtFactors.Rows.Add("R", dRAmount)
                Dim factorSnapshot As DataRow() = dtFactors.Select("Type <> 'R'")
                For Each fRow As DataRow In factorSnapshot
                    Dim sComposite As String = "R," & fRow("Type").ToString()
                    If dtFactors.Select("Type = '" & sComposite & "'").Length = 0 Then
                        dtFactors.Rows.Add(sComposite, dRAmount - Convert.ToDouble(fRow("Amount")))
                    End If
                Next
            End If
        End While

        ' Retained absorption -- mirrors SP exactly:
        ' unallocated = net_premium - SUM(non-retained in dtRIPremiums) - retained_current
        ' Manually added treaties excluded from dtRIPremiums but consume from net base;
        ' their premiums are added into the non-retained total here.
        If iRetLineId > 0 Then
            ' Sum all non-retained premiums from the iterative calculation table
            Dim dTotalNonRetPremium As Double = 0
            For Each row As DataRow In dtRIPremiums.Rows
                If Convert.ToInt32(row("RIArrangementLineKey")) <> iRetLineId Then
                    dTotalNonRetPremium += Convert.ToDouble(row("Premium"))
                End If
            Next

            ' Add manually added treaty premiums (excluded from dtRIPremiums but still consume premium)
            Dim oManualNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ManuallyAdded='True' and @IsDeleted!='True' and @Type!='']")
            For Each oManualNode As XmlNode In oManualNodes
                Dim dManualPremium As Double = 0
                If oManualNode.Attributes("Premium") IsNot Nothing Then
                    Double.TryParse(oManualNode.Attributes("Premium").Value, dManualPremium)
                End If
                dTotalNonRetPremium += dManualPremium
            Next

            ' Get current retained premium from dtRIPremiums
            Dim dRetCurrentPremium As Double = 0
            Dim retRows() As DataRow = dtRIPremiums.Select("RIArrangementLineKey = " & iRetLineId)
            If retRows.Length > 0 Then
                dRetCurrentPremium = Convert.ToDouble(retRows(0)("Premium"))
            End If

            ' Unallocated = net_premium - non_ret_total - ret_current  (mirrors SP)
            Dim dUnallocatedPremium As Double = dNetPremium - dTotalNonRetPremium - dRetCurrentPremium

            If dUnallocatedPremium <> 0 AndAlso retRows.Length > 0 Then
                retRows(0)("Premium") = dRetCurrentPremium + dUnallocatedPremium
            End If
        End If

        ' Split Retained premium between R and QSR lines - handled by ApplyQSRSplit below
        'SplitRetainedAndQSR(dtRIPremiums, iRetLineId, dGrossPremium, oXMLDoc, sRIBANDID, sRIarrangementKey)

        ' Calculate premium_percent
        For Each row As DataRow In dtRIPremiums.Rows
            Dim dPremiumValue As Double = Convert.ToDouble(row("Premium"))
            row("PremiumPerc") = If(dGrossPremium > 0, (dPremiumValue / dGrossPremium) * 100, 0)
        Next

        ' Update XML with calculated premiums (skip QSR lines - updated in SplitRetainedAndQSR)
        ' Also skip lines where the user has directly edited the premium (IsPremiumEdited=True) -
        ' their premium is already correct in the XML and was used in factor calculations above.
        For Each row As DataRow In dtRIPremiums.Select("IsQSR = False")
            Dim iLineKey As Integer = Convert.ToInt32(row("RIArrangementLineKey"))
            Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIarrangementKey='" & sRIarrangementKey & "' and @RIArrangementLineKey='" & iLineKey & "']")
            If oNode IsNot Nothing Then
                ' Skip premium write-back if user directly edited this premium
                Dim bIsPremiumEdited As Boolean = False
                If oNode.Attributes("IsPremiumEdited") IsNot Nothing Then
                    Boolean.TryParse(oNode.Attributes("IsPremiumEdited").Value, bIsPremiumEdited)
                End If
                If bIsPremiumEdited Then
                    ' Still recalculate Tax/Commission/CommissionTax from the preserved premium
                    Dim dPreservedPremium As Double = 0
                    Double.TryParse(oNode.Attributes("Premium").Value, dPreservedPremium)
                    Dim dTaxPercPE As Double = GetTaxPercentage(oNode, "Premium", "Tax")
                    Dim dCommPercPE As Double = If(oNode.Attributes("CommissionPerc") IsNot Nothing, Convert.ToDouble(oNode.Attributes("CommissionPerc").Value), 0)
                    Dim dCommTaxPercPE As Double = GetTaxPercentage(oNode, "Commission", "CommissionTax")
                    oNode.Attributes("Tax").Value = ((dTaxPercPE * dPreservedPremium) / 100).ToString()
                    oNode.Attributes("Commission").Value = ((dPreservedPremium * dCommPercPE) / 100).ToString()
                    oNode.Attributes("CommissionTax").Value = ((Convert.ToDouble(oNode.Attributes("Commission").Value) * dCommTaxPercPE) / 100).ToString()
                    Continue For
                End If
                Dim dCalcPremium As Double = Convert.ToDouble(row("Premium"))

                ' Derive tax rates BEFORE overwriting Premium (order-of-operations fix)
                Dim dTaxPerc As Double = GetTaxPercentage(oNode, "Premium", "Tax")
                Dim dCommPerc As Double = If(oNode.Attributes("CommissionPerc") IsNot Nothing, Convert.ToDouble(oNode.Attributes("CommissionPerc").Value), 0)
                Dim dCommTaxPerc As Double = GetTaxPercentage(oNode, "Commission", "CommissionTax")

                oNode.Attributes("Premium").Value = dCalcPremium.ToString()

                If row("PremiumPerc") IsNot DBNull.Value AndAlso oNode.Attributes("PremiumPerc") IsNot Nothing Then
                    oNode.Attributes("PremiumPerc").Value = Format(Convert.ToDouble(row("PremiumPerc")), "0.0000")
                End If

                oNode.Attributes("Tax").Value = ((dTaxPerc * dCalcPremium) / 100).ToString()
                oNode.Attributes("Commission").Value = ((dCalcPremium * dCommPerc) / 100).ToString()
                oNode.Attributes("CommissionTax").Value = ((Convert.ToDouble(oNode.Attributes("Commission").Value) * dCommTaxPerc) / 100).ToString()
            End If
        Next

        ' Apply QSR split on R node (R premium already includes unallocated)
        Dim oRetNodeFinal As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type='R' and @IsDeleted!='True']")
        If oRetNodeFinal IsNot Nothing Then
            Dim dRetPremFinal As Double = 0
            Double.TryParse(oRetNodeFinal.Attributes("Premium").Value, dRetPremFinal)
            Dim dRetSIFinal As Double = 0
            Double.TryParse(oRetNodeFinal.Attributes("SumInsured").Value, dRetSIFinal)
            ApplyQSRSplit(oXMLDoc, sRIBANDID, oRetNodeFinal, dRetSIFinal, dRetPremFinal, bPremiumOnly:=True)
        End If

        ' Update Allocated node premium = GrossPremium (all premium is allocated when RET exists)
        Dim oAllocNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Allocated']")
        If oAllocNode IsNot Nothing Then
            oAllocNode.Attributes("Premium").Value = dGrossPremium.ToString()
        End If

        ' Remove unallocated premium node if it exists (premium is fully allocated)
        Dim oUnAllocNodeTP As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Unallocated']")
        If oUnAllocNodeTP IsNot Nothing Then
            oUnAllocNodeTP.Attributes("Premium").Value = "0.00"
        End If

        ' Update the XML before returning
        Dim swContent As New System.IO.StringWriter()
        Dim xmlwContent As New XmlTextWriter(swContent)
        oXMLDoc.WriteTo(xmlwContent)
        sXML = swContent.ToString()
        xmlwContent.Close()
        swContent.Close()
    End Sub

    ''' <summary>
    ''' Splits the Retained premium between R and QSR (Quota Share Retained) lines
    ''' based on their default_share_percent. Mirrors the logic in spu_RI_treaty_premium_calc_RI2007.
    ''' </summary>
    Private Sub SplitRetainedAndQSR(ByVal dtRIPremiums As DataTable, ByVal iRetLineId As Integer,
                                    ByVal dGrossPremium As Double, ByVal oXMLDoc As XmlDocument,
                                    ByVal sRIBANDID As String, ByVal sRIarrangementKey As String)

        If iRetLineId = 0 Then Exit Sub
        Dim qsrRows() As DataRow = dtRIPremiums.Select("IsQSR = True")
        If qsrRows.Length = 0 Then Exit Sub

        ' Get R default percent from XML
        Dim oRetNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iRetLineId & "']")
        If oRetNode Is Nothing Then Exit Sub

        Dim dRDefaultPct As Double = 0
        If oRetNode.Attributes("DefaultPerc") IsNot Nothing Then
            Double.TryParse(oRetNode.Attributes("DefaultPerc").Value, dRDefaultPct)
        End If

        ' Sum all QSR default percents
        Dim dTotalQSRPct As Double = 0
        For Each qsrRow As DataRow In qsrRows
            Dim iQSRLineKey As Integer = Convert.ToInt32(qsrRow("RIArrangementLineKey"))
            Dim oQSRNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iQSRLineKey & "']")
            If oQSRNode IsNot Nothing AndAlso oQSRNode.Attributes("DefaultPerc") IsNot Nothing Then
                Dim dQSRPct As Double = 0
                Double.TryParse(oQSRNode.Attributes("DefaultPerc").Value, dQSRPct)
                dTotalQSRPct += dQSRPct
            End If
        Next

        Dim dTotalPct As Double = dRDefaultPct + dTotalQSRPct
        If dTotalPct = 0 Then Exit Sub

        ' Get retained premium from dtRIPremiums
        Dim retRows() As DataRow = dtRIPremiums.Select("RIArrangementLineKey = " & iRetLineId)
        If retRows.Length = 0 Then Exit Sub
        Dim dRetainedPremium As Double = Convert.ToDouble(retRows(0)("Premium"))

        ' Update R line with its proportional share
        Dim dQSRTotalPrem As Double = 0
        For Each qsrRowCalc As DataRow In qsrRows
            Dim iQSRLK As Integer = Convert.ToInt32(qsrRowCalc("RIArrangementLineKey"))
            Dim oQSRN As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iQSRLK & "']")
            If oQSRN IsNot Nothing AndAlso oQSRN.Attributes("DefaultPerc") IsNot Nothing Then
                Dim dQP As Double = 0
                Double.TryParse(oQSRN.Attributes("DefaultPerc").Value, dQP)
                dQSRTotalPrem += dRetainedPremium * dQP / 100
            End If
        Next
        retRows(0)("Premium") = dRetainedPremium - dQSRTotalPrem

        ' Update each QSR line individually
        For Each qsrRow As DataRow In qsrRows
            Dim iQSRLineKey As Integer = Convert.ToInt32(qsrRow("RIArrangementLineKey"))
            Dim oQSRNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & iQSRLineKey & "']")
            If oQSRNode IsNot Nothing Then
                Dim dQSRDefaultPct As Double = 0
                If oQSRNode.Attributes("DefaultPerc") IsNot Nothing Then
                    Double.TryParse(oQSRNode.Attributes("DefaultPerc").Value, dQSRDefaultPct)
                End If

                Dim dQSRPremium As Double = dRetainedPremium * dQSRDefaultPct / 100
                Dim dQSRPremiumPct As Double = If(dGrossPremium > 0, (dQSRPremium / dGrossPremium) * 100, 0)

                oQSRNode.Attributes("Premium").Value = dQSRPremium.ToString()
                If oQSRNode.Attributes("PremiumPerc") IsNot Nothing Then
                    oQSRNode.Attributes("PremiumPerc").Value = Format(dQSRPremiumPct, "0.0000")
                End If

                Dim dCommPerc As Double = If(oQSRNode.Attributes("CommissionPerc") IsNot Nothing, Convert.ToDouble(oQSRNode.Attributes("CommissionPerc").Value), 0)
                Dim dTaxPerc As Double = GetTaxPercentage(oQSRNode, "Premium", "Tax")
                Dim dCommTaxPerc As Double = GetTaxPercentage(oQSRNode, "Commission", "CommissionTax")

                oQSRNode.Attributes("Tax").Value = ((dTaxPerc * dQSRPremium) / 100).ToString()
                oQSRNode.Attributes("Commission").Value = ((dQSRPremium * dCommPerc) / 100).ToString()
                oQSRNode.Attributes("CommissionTax").Value = ((Convert.ToDouble(oQSRNode.Attributes("Commission").Value) * dCommTaxPerc) / 100).ToString()
            End If
        Next
    End Sub

    ''' <summary>
    ''' Updates or removes the Unallocated node based solely on premium difference (SI is never calculated here).
    ''' Shows node when band premium minus sum of all typed treaty premiums is non-zero; removes it otherwise.
    ''' </summary>
    Public Sub UpdateUnallocatedPremiumNode(ByVal oXMLDoc As XmlDocument, ByVal sRIBANDID As String, ByVal dBandPremium As Double)
        Dim dAllocatedPremium As Double = 0
        Dim oTreatyLines As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Type!='' and @Name!='Band Total' and @Name!='Net of FAC' and @Name!='Allocated' and @Name!='Unallocated' and @IsDeleted!='True']")
        For Each oNode As XmlNode In oTreatyLines
            Dim dPremium As Double = 0
            If oNode.Attributes("Premium") IsNot Nothing Then
                Double.TryParse(oNode.Attributes("Premium").Value, dPremium)
            End If
            dAllocatedPremium += dPremium
        Next

        Dim dUnallocatedPremium As Double = Math.Round(dBandPremium - dAllocatedPremium, 2)
        Dim oUnAllocNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Unallocated']")

        If dUnallocatedPremium <> 0 Then
            If oUnAllocNode IsNot Nothing Then
                oUnAllocNode.Attributes("Premium").Value = dUnallocatedPremium.ToString()
                oUnAllocNode.Attributes("IsDeleted").Value = "False"
            Else
                ' Recalculate did not create an Unallocated node (SI was fully allocated).
                ' Use Band Total node as template ? same pattern as Recalculate uses.
                Dim oAllocTemplate As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
                If oAllocTemplate IsNot Nothing Then
                    Dim oNewNode As XmlElement = oXMLDoc.CreateElement("ArrangementRow")
                    For Each attr As XmlAttribute In oAllocTemplate.Attributes
                        oNewNode.SetAttribute(attr.Name, "")
                    Next
                    oNewNode.SetAttribute("Name", "Unallocated")
                    oNewNode.SetAttribute("SumInsured", "0.00")
                    oNewNode.SetAttribute("Premium", dUnallocatedPremium.ToString())
                    oNewNode.SetAttribute("IsNew", "True")
                    oNewNode.SetAttribute("IsDeleted", "False")
                    Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']")
                    oRootNode.InsertAfter(oNewNode, oRootNode.LastChild)
                End If
            End If
        Else
            If oUnAllocNode IsNot Nothing Then
                Dim oRootNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']")
                ' Only remove if Sum Insured is also fully allocated
                Dim dNodeSI As Double = 0
                If oUnAllocNode.Attributes("SumInsured") IsNot Nothing AndAlso Not String.IsNullOrEmpty(oUnAllocNode.Attributes("SumInsured").Value) Then
                    Double.TryParse(oUnAllocNode.Attributes("SumInsured").Value, dNodeSI)
                End If
                If Math.Round(dNodeSI, 2) = 0 Then
                    oRootNode.RemoveChild(oUnAllocNode)
                Else
                    oUnAllocNode.Attributes("Premium").Value = "0.00"
                    oUnAllocNode.Attributes("IsDeleted").Value = "False"
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Update FAC Prop Premium based on percentage when FAC Premium is Non-Proportional
    ''' </summary>
    Public Sub UpdateFACPropPremiumPercentage(ByRef sXML As String,
                                                  ByVal dPercentage As Double,
                                                  ByVal sRIBANDID As String,
                                                  ByVal sRIArrangementLineKey As String,
                                                  ByVal sRIarrangementKey As String,
                                                  ByVal sTransType As String,
                                                  ByVal dTaxPercentage As Double,
                                                  Optional ByVal bIsPortfolioRIAmendment As Boolean = False)
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(sXML)

        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
        Dim dBandPremium As Double = 0
        If oNode IsNot Nothing Then
            Double.TryParse(oNode.Attributes("Premium").Value, dBandPremium)
        End If

        Dim dOblQSHPremium As Double = 0
        Dim oOblQSHNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='T']")
        For Each oOblQSHNode As XmlNode In oOblQSHNodes
            If oOblQSHNode.Attributes("IsObligatory").Value = "True" Then
                dOblQSHPremium += Convert.ToDouble(oOblQSHNode.Attributes("Premium").Value)
            End If
        Next

        Dim oChangedNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        If oChangedNode IsNot Nothing Then
            Dim dPremium As Double = ((dBandPremium - dOblQSHPremium) * dPercentage) / 100
            oChangedNode.Attributes("IsEdited").Value = True
            oChangedNode.Attributes("Premium").Value = dPremium
            'the code assigns to the FACPropPremiumPerc attribute ensuring it exists
            Dim facPropAttr As XmlAttribute = oChangedNode.Attributes("FACPropPremiumPerc")
            If facPropAttr Is Nothing Then
                facPropAttr = oXMLDoc.CreateAttribute("FACPropPremiumPerc")
                oChangedNode.Attributes.Append(facPropAttr)
            End If
            facPropAttr.Value = dPercentage.ToString()
            oChangedNode.Attributes("Tax").Value = (dTaxPercentage * dPremium) / 100
        End If

        sXML = oXMLDoc.OuterXml
        Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
    End Sub

    ''' <summary>
    ''' Update FAC Prop Premium directly when FAC Premium is Non-Proportional
    ''' </summary>
    Public Sub UpdateFACPropPremium(ByRef sXML As String,
                                    ByVal dPremium As Double,
                                    ByVal sRIBANDID As String,
                                    ByVal sRIArrangementLineKey As String,
                                    ByVal sRIarrangementKey As String,
                                    ByVal sTransType As String,
                                    ByVal dTaxPercentage As Double,
                                    Optional ByVal bIsPortfolioRIAmendment As Boolean = False)
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(sXML)

        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
        Dim dBandPremium As Double = 0
        If oNode IsNot Nothing Then
            Double.TryParse(oNode.Attributes("Premium").Value, dBandPremium)
        End If

        Dim dOblQSHPremium As Double = 0
        Dim oOblQSHNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='T']")
        For Each oOblQSHNode As XmlNode In oOblQSHNodes
            If oOblQSHNode.Attributes("IsObligatory").Value = "True" Then
                dOblQSHPremium += Convert.ToDouble(oOblQSHNode.Attributes("Premium").Value)
            End If
        Next

        Dim oChangedNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@RIArrangementLineKey='" & sRIArrangementLineKey.Trim & "']")
        If oChangedNode IsNot Nothing Then
            Dim dPercentage As Double = If((dBandPremium - dOblQSHPremium) > 0, (dPremium * 100) / (dBandPremium - dOblQSHPremium), 0)
            oChangedNode.Attributes("IsEdited").Value = True
            oChangedNode.Attributes("Premium").Value = dPremium

            Dim facPropAttr As XmlAttribute = oChangedNode.Attributes("FACPropPremiumPerc")
            If facPropAttr Is Nothing Then
                facPropAttr = oXMLDoc.CreateAttribute("FACPropPremiumPerc")
                oChangedNode.Attributes.Append(facPropAttr)
            End If
            facPropAttr.Value = dPercentage.ToString()

            oChangedNode.Attributes("Tax").Value = (dTaxPercentage * dPremium) / 100
        End If
        sXML = oXMLDoc.OuterXml
        Recalculate(sXML, sRIBANDID, sRIarrangementKey, sRIArrangementLineKey, sTransType, False, dTaxPercentage, bIsPortfolioRIAmendment:=bIsPortfolioRIAmendment)
    End Sub

    ''' <summary>
    ''' Recalculate all derived values in the arrangement
    ''' </summary>
    Public Sub Recalculate(riXml As XmlDocument)
        Dim oNetFACNode As XmlNode = riXml.SelectSingleNode("//ArrangementRow[@Name='Net of FAC']")

        Dim dNetSumInsured As Double = Convert.ToDouble(oNetFACNode.Attributes("SumInsured").Value)
        Dim dNetPremium As Double = Convert.ToDouble(oNetFACNode.Attributes("Premium").Value)

        Dim dTotalSumInsured As Double = 0
        Dim dTotalPremium As Double = 0
        Dim dTotalThisPercentage As Double = 0

        Dim oTreatyNodes As XmlNodeList = riXml.SelectNodes("//ArrangementRow[@Type='T' and @manually_added='1']")
        For Each oNode As XmlNode In oTreatyNodes
            Dim dThisPerc As Double = (Convert.ToDouble(oNode.Attributes("SumInsured").Value) / dNetSumInsured) * 100
            oNode.Attributes("ThisPerc").Value = Format(dThisPerc, "0.0000")
            oNode.Attributes("Premium").Value = Format((dNetPremium * dThisPerc) / 100, "0.00")

            dTotalSumInsured += Convert.ToDouble(oNode.Attributes("SumInsured").Value)
            dTotalPremium += Convert.ToDouble(oNode.Attributes("Premium").Value)
            dTotalThisPercentage += dThisPerc
        Next

        Dim oTotalsNode As XmlNode = riXml.SelectSingleNode("//Totals")
        If oTotalsNode IsNot Nothing Then
            oTotalsNode.Attributes("TotalSumInsured").Value = Format(dTotalSumInsured, "0.00")
            oTotalsNode.Attributes("TotalPremium").Value = Format(dTotalPremium, "0.00")
            oTotalsNode.Attributes("TotalThisPercentage").Value = Format(dTotalThisPercentage, "0.0000")
        End If
    End Sub

    ''' <summary>
    ''' Helper method to update or create XML element
    ''' </summary>
    Private Sub UpdateOrCreateElement(doc As XmlDocument, parent As XmlNode, elementName As String, value As String)
        Dim element As XmlNode = parent.SelectSingleNode(elementName)
        If element Is Nothing Then
            element = doc.CreateElement(elementName)
            parent.AppendChild(element)
        End If
        element.InnerText = value
    End Sub

    ''' <summary>
    ''' Process MTA transaction for manually added treaties.
    ''' Recalculates SI/Premium proportionally from Net of FAC using the saved ThisPerc,
    ''' then reapplies the DB-sourced Tax%, CommissionPerc and CommissionTax% rates
    ''' so those values are preserved rather than recalculated from scratch.
    ''' so those values are preserved rather than recalculated from scratch.
    ''' </summary>
    Public Sub ProcessMTA(ByRef riXml As XmlDocument, newSumInsured As Double)
        Dim oNetFACNode As XmlNode = riXml.SelectSingleNode("//ArrangementRow[@Name='Net of FAC']")

        ' PBI 35359: Only rows with IsEditedDB=True from the DB (genuinely user-edited)
        ' are preserved by Recalculate. Non-edited rows stay IsEditedDB=False so
        ' Recalculate recalculates them normally from DefaultPerc.
        ' The IsUserEdited snapshot (for highlighting) was already taken in PopulateGrid
        ' before ProcessMTA is called, so highlighting is unaffected.

        ' Early return if no FAC - no manually added treaty recalc needed
        If oNetFACNode Is Nothing Then Return

        Dim dNetSI As Double = Convert.ToDouble(oNetFACNode.Attributes("SumInsured").Value)
        Dim dNetPremium As Double = Convert.ToDouble(oNetFACNode.Attributes("Premium").Value)
        ' Mark as IsEdited so Recalculate preserves these values
        ' Mark all editable line types as IsEdited so Recalculate preserves saved values
        ' Exclude obligatory T nodes ? they are handled by CalculateFACQSH and must not enter
        ' the bIsEdited preservation path in Recalculate, which would double-subtract their SI.
        ' Recalculate manually added proportional treaty SI/Premium from Net of FAC
        If oNetFACNode Is Nothing Then Return
        Dim oManualNodes As XmlNodeList = riXml.SelectNodes("//ArrangementRow[@ManuallyAdded='True' or @ManuallyAdded='1']")
        For Each oNode As XmlNode In oManualNodes
            If oNode.Attributes("Placement") IsNot Nothing AndAlso oNode.Attributes("Placement").Value.Contains("Prop") Then
                Dim dThisPerc As Double = Convert.ToDouble(oNode.Attributes("ThisPerc").Value)
                oNode.Attributes("SumInsured").Value = Format((dNetSI * dThisPerc) / 100, "0.00")
                oNode.Attributes("Premium").Value = Format((dNetPremium * dThisPerc) / 100, "0.00")
            End If
        Next
    End Sub

    ''' <summary>
    ''' Get manually added proportional treaty lines
    ''' </summary>
    Public Function GetManuallyAddedProportionalLines(riXml As XmlDocument) As List(Of XmlNode)
        Dim lines As New List(Of XmlNode)()
        Dim oNodes As XmlNodeList = riXml.SelectNodes("//ArrangementRow[@ManuallyAdded='True' or @ManuallyAdded='1']")
        For Each oNode As XmlNode In oNodes
            If oNode.Attributes("Placement") IsNot Nothing AndAlso oNode.Attributes("Placement").Value.Contains("Prop") Then
                lines.Add(oNode)
            End If
        Next
        Return lines
    End Function

    ''' <summary>
    ''' Process renewal transaction - remove manually added treaties if not true monthly or is annual renewal
    ''' </summary>
    Public Sub ProcessRenewal(ByRef riXml As XmlDocument, isTrueMonthly As Boolean, isAnnualRenewal As Boolean)
        If Not isTrueMonthly OrElse isAnnualRenewal Then
            RemoveManuallyAddedTreaties(riXml)
        End If
    End Sub

    ''' <summary>
    ''' Remove all manually added treaties from arrangement
    ''' </summary>
    Public Function RemoveManuallyAddedTreaties(ByRef riXml As XmlDocument) As Integer
        Dim count As Integer = 0
        Dim oNodes As XmlNodeList = riXml.SelectNodes("//ArrangementRow[@ManuallyAdded='True' or @ManuallyAdded='1']")
        Dim nodesToRemove As New List(Of XmlNode)()
        For Each oNode As XmlNode In oNodes
            nodesToRemove.Add(oNode)
        Next
        For Each oNode As XmlNode In nodesToRemove
            oNode.ParentNode.RemoveChild(oNode)
            count += 1
        Next
        Return count
    End Function

    ''' <summary>
    ''' Validate manually added XOL treaty premiums do not exceed Net of FAC premium
    ''' </summary>
    Public Function ValidateManualXOLPremiums(sXML As String, sRIBANDID As String, ByRef sErrorMessage As String) As Boolean
        Dim oXMLDoc As New XmlDocument
        oXMLDoc.LoadXml(sXML)

        Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
        Dim dNetFACPremium As Double = 0
        If oNetFACNode IsNot Nothing Then
            Double.TryParse(oNetFACNode.Attributes("Premium").Value, dNetFACPremium)
        Else
            Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
            If oBandNode IsNot Nothing Then
                Double.TryParse(oBandNode.Attributes("Premium").Value, dNetFACPremium)
            End If
        End If

        Dim dTotalManualXOLPremium As Double = 0
        Dim sXOLTreatyNames As New List(Of String)
        Dim oManualXOLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[(@Type='TX' or @Type='PX') and @ManuallyAdded='True']")

        For Each oNode As XmlNode In oManualXOLNodes
            If oNode.Attributes("IsDeleted") Is Nothing OrElse oNode.Attributes("IsDeleted").Value <> "True" Then
                Dim dPremium As Double = 0
                Double.TryParse(oNode.Attributes("Premium").Value, dPremium)
                dTotalManualXOLPremium += dPremium

                Dim sTreatyName As String = ""
                If oNode.Attributes("Name") IsNot Nothing Then
                    sTreatyName = oNode.Attributes("Name").Value
                End If
                If Not String.IsNullOrEmpty(sTreatyName) AndAlso Not sXOLTreatyNames.Contains(sTreatyName) Then
                    sXOLTreatyNames.Add(sTreatyName)
                End If
            End If
        Next

        If dTotalManualXOLPremium > dNetFACPremium Then
            Dim sTreatyList As String = If(sXOLTreatyNames.Count > 0, String.Join(", ", sXOLTreatyNames), "XOL Treaty")
            sErrorMessage = "Sum premium share for " & sTreatyList & " must be less than or equal to Gross Premium less FAC premiums."
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Process portfolio transfer - tag manually added treaties with transfer details
    ''' </summary>
    Public Sub ProcessPortfolioTransfer(ByRef riXml As XmlDocument, transferDate As Date, targetPolicyKey As String)
        Dim oManualNodes As XmlNodeList = riXml.SelectNodes("//ArrangementRow[@ManuallyAdded='True' or @ManuallyAdded='1']")
        For Each oNode As XmlNode In oManualNodes
            If oNode.Attributes("TransferDate") Is Nothing Then
                Dim attr As XmlAttribute = riXml.CreateAttribute("TransferDate")
                oNode.Attributes.Append(attr)
            End If
            oNode.Attributes("TransferDate").Value = transferDate.ToString("yyyy-MM-dd")

            If oNode.Attributes("TargetPolicyKey") Is Nothing Then
                Dim attr As XmlAttribute = riXml.CreateAttribute("TargetPolicyKey")
                oNode.Attributes.Append(attr)
            End If
            oNode.Attributes("TargetPolicyKey").Value = targetPolicyKey
        Next
    End Sub

    ''' <summary>
    ''' Validate transaction state for manual treaty operations
    ''' </summary>
    Public Function ValidateTransactionState(transactionType As String, isEditable As Boolean) As (Boolean, String)
        If String.IsNullOrEmpty(transactionType) Then
            Return (False, "Transaction type is required")
        End If
        If Not (transactionType = "NB" OrElse transactionType = "RN" OrElse transactionType = "MTA") Then
            Return (False, "Manual treaty operations are only available for New Business, Renewal, and MTA transactions")
        End If
        If Not isEditable Then
            Return (False, "Manual treaty operations are not available in read-only mode")
        End If
        Return (True, String.Empty)
    End Function

    ''' <summary>
    ''' Handle division by zero when Net of FAC is zero
    ''' </summary>
    Public Function SafeDivide(numerator As Double, denominator As Double, Optional defaultValue As Double = 0) As Double
        If denominator = 0 Then
            Return defaultValue
        End If
        Return numerator / denominator
    End Function

    ''' <summary>
    ''' Validate session state and return error if session is lost
    ''' </summary>
    Public Function ValidateSessionState(session As Object, sessionKey As String) As (Boolean, String)
        If session Is Nothing Then
            Return (False, "Session has expired. Please refresh the page and try again")
        End If
        If String.IsNullOrEmpty(sessionKey) Then
            Return (False, "Session key is required")
        End If
        If session(sessionKey) Is Nothing Then
            Return (False, "RI arrangement data not found in session. Please reload the page")
        End If
        Return (True, String.Empty)
    End Function

    ''' <summary>
    ''' Handle API call failures with retry logic
    ''' </summary>
    Public Function ExecuteAPICall(Of T)(apiUrl As String, Optional maxRetries As Integer = 3) As (Boolean, T, String)
        Dim retryCount As Integer = 0
        Dim lastError As String = String.Empty

        While retryCount < maxRetries
            Try
                Using client As New System.Net.WebClient()
                    Dim response As String = client.DownloadString(apiUrl)
                    Dim result As T = Newtonsoft.Json.JsonConvert.DeserializeObject(Of T)(response)
                    Return (True, result, String.Empty)
                End Using
            Catch ex As System.Net.WebException
                lastError = "Network error: " & ex.Message
                retryCount += 1
                If retryCount < maxRetries Then
                    System.Threading.Thread.Sleep(1000 * retryCount)
                End If
            Catch ex As Exception
                lastError = "API error: " & ex.Message
                Exit While
            End Try
        End While

        Return (False, Nothing, lastError)
    End Function

    ''' <summary>
    ''' Validate treaty ID exists and is valid
    ''' </summary>
    Public Function ValidateTreatyID(treatyID As String, availableTreaties As DataTable) As (Boolean, String)
        If String.IsNullOrEmpty(treatyID) Then
            Return (False, "Treaty ID is required")
        End If

        If availableTreaties Is Nothing OrElse availableTreaties.Rows.Count = 0 Then
            Return (False, "No treaties available")
        End If

        Dim found As Boolean = False
        For Each row As DataRow In availableTreaties.Rows
            If row("TreatyID").ToString() = treatyID Then
                found = True
                Exit For
            End If
        Next

        If Not found Then
            Return (False, "Invalid treaty ID. Treaty not found in available treaties")
        End If

        Return (True, String.Empty)
    End Function

    ''' <summary>
    ''' Handle concurrent modification by checking timestamp/version
    ''' </summary>
    Public Function CheckConcurrentModification(riXml As XmlDocument, lastModifiedTimestamp As DateTime) As (Boolean, String)
        Dim oRootNode As XmlNode = riXml.SelectSingleNode("/rows")
        If oRootNode Is Nothing Then
            Return (False, "Invalid RI arrangement data")
        End If

        Dim currentTimestamp As DateTime
        If oRootNode.Attributes("LastModified") IsNot Nothing Then
            DateTime.TryParse(oRootNode.Attributes("LastModified").Value, currentTimestamp)
            If currentTimestamp > lastModifiedTimestamp Then
                Return (False, "This arrangement has been modified by another user. Please reload to see the latest changes or overwrite their changes.")
            End If
        End If

        Return (True, String.Empty)
    End Function

    ''' <summary>
    ''' Handle business rule violations
    ''' </summary>
    Public Function ValidateBusinessRules(riXml As XmlDocument, sRIBANDID As String) As (Boolean, String)
        Try
            Dim oNetFACNode As XmlNode = riXml.SelectSingleNode("//RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
            If oNetFACNode Is Nothing Then
                Return (True, String.Empty)
            End If

            Dim oManualNodes As XmlNodeList = riXml.SelectNodes("//RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@ManuallyAdded='True' or @ManuallyAdded='1']")

            Dim dNetSI As Double = Convert.ToDouble(oNetFACNode.Attributes("SumInsured").Value)
            Dim dTotalManualSI As Double = 0
            For Each oNode As XmlNode In oManualNodes
                If oNode.Attributes("SumInsured") IsNot Nothing Then
                    dTotalManualSI += Convert.ToDouble(oNode.Attributes("SumInsured").Value)
                End If
            Next

            If dTotalManualSI > dNetSI Then
                Return (False, "Total manually added treaty sum insured exceeds Net of FAC. Please adjust the allocations.")
            End If

            Return (True, String.Empty)
        Catch ex As Exception
            Return (False, "Business rule validation error: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Validate manually added XOL treaty upper limits do not exceed Gross Sum Insured less FAC Sum Insureds
    ''' </summary>
    Public Function ValidateManualXOLUpperLimits(sXML As String, sRIBANDID As String, ByRef sErrorMessage As String) As Boolean
        Dim oXMLDoc As New XmlDocument()
        oXMLDoc.LoadXml(sXML)

        'Get Gross Sum Insured (Band Total)
        Dim dGrossSI As Double = 0
        Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
        If oBandNode IsNot Nothing Then
            Double.TryParse(oBandNode.Attributes("SumInsured").Value, dGrossSI)
        End If

        'Get total FAC Sum Insured (both F and FX types)
        Dim dTotalFACSI As Double = 0
        Dim oFACNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[@Type='F' or @Type='FX']")
        For Each oFACNode As XmlNode In oFACNodes
            If Convert.ToString(oFACNode.Attributes("IsDeleted").Value) <> "True" Then
                Dim dFACSI As Double = 0
                Double.TryParse(oFACNode.Attributes("SumInsured").Value, dFACSI)
                dTotalFACSI += dFACSI
            End If
        Next

        'Calculate available SI for XOL treaties
        Dim dAvailableSI As Double = dGrossSI - dTotalFACSI

        'Check manually added XOL treaties
        Dim oManualXOLNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='Current_" & sRIBANDID & "']/ArrangementRow[(@Type='TX' or @Type='PX') and @ManuallyAdded='True']")
        Dim invalidTreaties As New List(Of String)()

        For Each oXOLNode As XmlNode In oManualXOLNodes
            If Convert.ToString(oXOLNode.Attributes("IsDeleted").Value) <> "True" Then
                Dim dUpperLimit As Double = 0
                Dim sTreatyName As String = ""

                If oXOLNode.Attributes("LineLimit") IsNot Nothing Then
                    Double.TryParse(oXOLNode.Attributes("LineLimit").Value, dUpperLimit)
                End If

                If oXOLNode.Attributes("Name") IsNot Nothing Then
                    sTreatyName = oXOLNode.Attributes("Name").Value
                End If

                'Validate upper limit does not exceed available SI
                If dUpperLimit > dAvailableSI Then
                    invalidTreaties.Add(sTreatyName)
                End If
            End If
        Next

        'Build error message if validation fails
        If invalidTreaties.Count > 0 Then
            sErrorMessage = "The " & String.Join(", ", invalidTreaties) & " Upper Limit must be less than or equal to Gross Sum Insured less FAC Sum Insureds."
            Return False
        End If

        sErrorMessage = ""
        Return True
    End Function
    Public Function SumOfNonRetPremiums(oXMLDoc As XmlDocument, sRIBANDID As String) As Double
        Dim dTotal As Double = 0
        Dim oNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@IsDeleted!='True' and @Type!='' and @Type != 'R' and @Name!='Band Total' and @Name!='Net of FAC' and @Name!='Allocated' and @Name!='Unallocated']")
        For Each oNode As XmlNode In oNodes
            Dim sType As String = If(oNode.Attributes("Type") IsNot Nothing, oNode.Attributes("Type").Value, "")
            Dim sReinsTypeCode As String = If(oNode.Attributes("ReinsuranceTypeCode") IsNot Nothing, oNode.Attributes("ReinsuranceTypeCode").Value, "")
            Dim bIsEdited As Boolean = False
            Dim bIsManuallyAdded As Boolean = False
            If oNode.Attributes("IsEdited") IsNot Nothing Then Boolean.TryParse(oNode.Attributes("IsEdited").Value, bIsEdited)
            Dim bIsObligatory As Boolean = False
            If oNode.Attributes("IsObligatory") IsNot Nothing Then Boolean.TryParse(oNode.Attributes("IsObligatory").Value, bIsObligatory)

            Dim bInclude As Boolean = False
            If sType = "F" OrElse sType = "FX" OrElse sType = "T" OrElse sType = "TX" OrElse sType = "PX" OrElse sType = "TC" OrElse sType = "TFS" Then
                bInclude = True
            ElseIf bIsManuallyAdded Then
                bInclude = True
            End If

            If bInclude Then
                Dim dP As Double = 0
                If oNode.Attributes("Premium") IsNot Nothing Then Double.TryParse(oNode.Attributes("Premium").Value, dP)
                dTotal += dP
            End If
        Next
        Return dTotal
    End Function

    ''' <summary>
    ''' Splits the Retained node premium between R and non-edited QSR nodes.
    ''' QSR gets its share based on DefaultPerc. R gets the remainder.
    ''' ThisPerc is recalculated for both.
    ''' </summary>
    Private Sub ApplyQSRSplit(ByVal oXMLDoc As XmlDocument, ByVal sRIBANDID As String, ByVal oRetNode As XmlNode, ByVal dRetSI As Double, ByVal dRetPremium As Double, Optional ByVal bPremiumOnly As Boolean = False)
        ' Get non-edited QSR nodes (use IsPremiumEdited when bPremiumOnly, IsEdited otherwise)
        Dim sQSRFilter As String = If(bPremiumOnly,
            "/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ReinsuranceTypeCode='QSR' and @IsDeleted!='True' and (@IsPremiumEdited!='True' or not(@IsPremiumEdited))]",
            "/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ReinsuranceTypeCode='QSR' and @IsDeleted!='True' and @IsEdited!='True']")
        Dim oQSRNodes As XmlNodeList = oXMLDoc.SelectNodes(sQSRFilter)
        If oQSRNodes Is Nothing OrElse oQSRNodes.Count = 0 Then Exit Sub

        ' Get GrossNet SI (Net of FAC) for ThisPerc calculation - consistent with priority loop treaties
        Dim dGrossNetSI As Double = 0
        Dim oNetFACNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Net of FAC']")
        If oNetFACNode IsNot Nothing Then
            Double.TryParse(oNetFACNode.Attributes("SumInsured").Value, dGrossNetSI)
        Else
            Dim oBandNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@Name='Band Total']")
            If oBandNode IsNot Nothing Then Double.TryParse(oBandNode.Attributes("SumInsured").Value, dGrossNetSI)
        End If

        ' Subtract premium-edited QSR from the pool (their premium is preserved)
        Dim dEditedQSRPremTotal As Double = 0
        If bPremiumOnly Then
            Dim oEditedQSRPrem As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ReinsuranceTypeCode='QSR' and @IsDeleted!='True' and @IsPremiumEdited='True']")
            If oEditedQSRPrem IsNot Nothing Then
                For Each oENode As XmlNode In oEditedQSRPrem
                    Dim dEP As Double = 0
                    If oENode.Attributes("Premium") IsNot Nothing Then Double.TryParse(oENode.Attributes("Premium").Value, dEP)
                    dEditedQSRPremTotal += dEP
                Next
            End If
        Else
            Dim oEditedQSRSI As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & sRIBANDID & "']/ArrangementRow[@ReinsuranceTypeCode='QSR' and @IsDeleted!='True' and @IsEdited='True']")
            If oEditedQSRSI IsNot Nothing Then
                For Each oENode As XmlNode In oEditedQSRSI
                    Dim dEP As Double = 0
                    If oENode.Attributes("Premium") IsNot Nothing Then Double.TryParse(oENode.Attributes("Premium").Value, dEP)
                    dEditedQSRPremTotal += dEP
                Next
            End If
        End If
        dRetPremium = dRetPremium - dEditedQSRPremTotal

        ' Calculate each QSR share and subtract from R
        Dim dTotalQSRPremium As Double = 0
        Dim dTotalQSRSI As Double = 0

        For Each oQSRNode As XmlNode In oQSRNodes
            Dim dQSRDefaultPct As Double = 0
            If oQSRNode.Attributes("DefaultPerc") IsNot Nothing Then
                Double.TryParse(oQSRNode.Attributes("DefaultPerc").Value, dQSRDefaultPct)
            End If

            Dim dQSRPremium As Double = dRetPremium * dQSRDefaultPct / 100
            Dim dQSRSI As Double = dRetSI * dQSRDefaultPct / 100
            dTotalQSRPremium += dQSRPremium
            dTotalQSRSI += dQSRSI

            oQSRNode.Attributes("Premium").Value = Format(dQSRPremium, "0.00")
            If Not bPremiumOnly Then oQSRNode.Attributes("SumInsured").Value = Format(dQSRSI, "0.00")

            ' Recalculate ThisPerc
            If Not bPremiumOnly Then
                Dim dQSRThisPerc As Double = If(dGrossNetSI <> 0, (dQSRSI / dGrossNetSI) * 100, 0)
                If Not bPremiumOnly AndAlso oQSRNode.Attributes("ThisPerc") IsNot Nothing Then oQSRNode.Attributes("ThisPerc").Value = Format(dQSRThisPerc, "0.0000")
            End If

            ' Recalculate Tax, Commission, CommissionTax
            Dim dCommPerc As Double = If(oQSRNode.Attributes("CommissionPerc") IsNot Nothing, Convert.ToDouble(oQSRNode.Attributes("CommissionPerc").Value), 0)
            Dim dTaxPerc As Double = GetTaxPercentage(oQSRNode, "Premium", "Tax")
            Dim dCommTaxPerc As Double = GetTaxPercentage(oQSRNode, "Commission", "CommissionTax")
            oQSRNode.Attributes("Tax").Value = Format((dTaxPerc * dQSRPremium) / 100, "0.00")
            oQSRNode.Attributes("Commission").Value = Format((dQSRPremium * dCommPerc) / 100, "0.00")
            oQSRNode.Attributes("CommissionTax").Value = Format((Convert.ToDouble(oQSRNode.Attributes("Commission").Value) * dCommTaxPerc) / 100, "0.00")
        Next

        ' R gets the remainder after QSR shares are removed
        Dim dRFinalPremium As Double = dRetPremium - dTotalQSRPremium
        Dim dRFinalSI As Double = dRetSI - dTotalQSRSI
        oRetNode.Attributes("Premium").Value = Format(dRFinalPremium, "0.00")
        If Not bPremiumOnly Then
            If Not bPremiumOnly Then oRetNode.Attributes("SumInsured").Value = Format(dRFinalSI, "0.00")
            ' Recalculate ThisPerc for R
            Dim dRThisPerc As Double = If(dGrossNetSI <> 0, (dRFinalSI / dGrossNetSI) * 100, 0)
            If Not bPremiumOnly AndAlso oRetNode.Attributes("ThisPerc") IsNot Nothing Then oRetNode.Attributes("ThisPerc").Value = Format(dRThisPerc, "0.0000")
        End If
        ' Recalculate Tax, Commission, CommissionTax for R node
        Dim dRTaxPerc As Double = GetTaxPercentage(oRetNode, "Premium", "Tax")
        Dim dRCommPerc As Double = If(oRetNode.Attributes("CommissionPerc") IsNot Nothing, Convert.ToDouble(oRetNode.Attributes("CommissionPerc").Value), 0)
        oRetNode.Attributes("Tax").Value = Format((dRTaxPerc * dRFinalPremium) / 100, "0.00")
        oRetNode.Attributes("Commission").Value = Format((dRFinalPremium * dRCommPerc) / 100, "0.00")
        Dim dRNewTax As Double = CDbl(oRetNode.Attributes("Tax").Value)
        Dim dRCommTaxPerc As Double = If(dRFinalPremium = 0, 0, dRNewTax * 100 / dRFinalPremium)
        oRetNode.Attributes("CommissionTax").Value = Format((Convert.ToDouble(oRetNode.Attributes("Commission").Value) * dRCommTaxPerc) / 100, "0.00")
    End Sub

End Class