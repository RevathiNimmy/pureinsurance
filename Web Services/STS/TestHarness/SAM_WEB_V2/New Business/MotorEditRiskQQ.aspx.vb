Imports System.Xml
Imports System.Xml.XPath
Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.IO
Partial Class New_Business_MotorEditRiskQQ
    Inherits System.Web.UI.Page
    Dim StartDate As Date


    Dim UserToken As UsernameToken
    Dim m_nRiskCnt As Integer
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2
    Dim m_sRiskDataXML As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        txtNoOfPerson.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")


        If Not IsPostBack Then
            UserToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")
            BuildLists1(oSAM, ddlVechicleRegisteredinIndividualorCorparateName, STSListType.PMLookup, "UDL_MC_HOWREG")
            BuildLists(oSAM, ddlCoverType, STSListType.UserDefinedTable, "MOTQQCT")
            BuildLists1(oSAM, ddlVolutaryDedecutible, STSListType.PMLookup, "UDL_VOLDED")
            Dim oGetRiskRequestType As New GetRiskRequestType
            Dim oGetRiskResponseType As New GetRiskResponseType


            With oGetRiskRequestType
                .BranchCode = "Headoff"


                'add values from the quote we just added
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFileKey = Session("InsuranceFileKey")
                .QuoteTimeStamp = Session("TimeStamp")
                .RiskKey = Session("RiskKey")
            End With

            Try
                StartDate = Date.Now
                oGetRiskResponseType = oSAM.GetRisk(oGetRiskRequestType)
                WriteToLog(Session, "MotorEditRiskQQ.aspx", "SAMForInsuranceV2", "GetRisk",StartDate, Date.Now)
                Dim m_sRiskDataXML As String = Nothing
                Dim xmlDoc As New System.Xml.XmlDocument
                With oGetRiskResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    End If
                    m_sRiskDataXML = .XMLDataSet.ToString
                    Session("TimeStamp") = .QuoteTimeStamp


                    xmlDoc.LoadXml(m_sRiskDataXML)
                    BuildXMLNode(xmlDoc, "")
                    Dim xmlDoc1 As New System.Xml.XmlDocument
                    xmlDoc1.LoadXml(m_sRiskDataXML)
                    'uday
                    Session("m_sRiskDataXML") = m_sRiskDataXML

                    'Start Claim History
                    Dim oElement_MOTOR_POLICY_BINDER As System.Xml.XmlNode = xmlDoc1.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER")
                    'For Grid binding of Claim History
                    If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasChildNodes Then
                        Dim oElement_COVERSELECTION As System.Xml.XmlNodeList = xmlDoc1.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("COVERSELECTION").SelectNodes("CLAIMSHISTORY")
                        Dim str As String = Nothing
                        If oElement_COVERSELECTION.Count > 0 Then
                            str = "<DATASET>"

                            For iCnt As Integer = 0 To oElement_COVERSELECTION.Count - 1
                                str &= "<" & oElement_COVERSELECTION(0).Name & " "
                                For icount As Integer = 0 To oElement_COVERSELECTION.Item(iCnt).Attributes.Count - 1
                                    If oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "US" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_COVERSELECTION_ID" Then

                                    Else
                                        str &= oElement_COVERSELECTION.Item(iCnt).Attributes(icount).OuterXml & " "
                                    End If
                                Next
                                str &= "/>"
                            Next

                            str &= "</DATASET>"
                            Dim sr As New System.IO.StringReader(str)

                            Dim ds As New DataSet
                            ds.ReadXml(sr)
                            gvClaimHistory.DataSource = ds
                            gvClaimHistory.DataBind()
                        End If
                        pnlClaimHistory.Visible = False

                    End If
                    'End If
                    'End Claim History
                End With


                    Dim oElement_DATA_SET As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET")

                    If oElement_DATA_SET IsNot Nothing Then

                        Dim oElement_RISK_OBJECTS As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS")

                        If oElement_RISK_OBJECTS IsNot Nothing Then

                            Dim oElement_MOTOR_POLICY_BINDER As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER")

                            If oElement_MOTOR_POLICY_BINDER IsNot Nothing Then




                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("QUICK_QUOTE") Then
                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("QUICK_QUOTE").Value <> 1 Then

                                    Response.Redirect("MotorEditRisk.aspx")
                                End If
                                Else
                                    Response.Redirect("MotorEditRisk.aspx")
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("FC1_USED") Then
                                    hdnFC1.Value = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("FC1_USED").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_MAKE") Then
                                    txtVechicleMake.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_MAKE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_MODEL") Then
                                    txtVechicleModel.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_MODEL").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("BODY_TYPE") Then
                                    txtBodyTypeQQ.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("BODY_TYPE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("SEATING_CAPACITY") Then
                                    txtSeatingCapacityQQ.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("SEATING_CAPACITY").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("CUBIC_CAPACITY") Then
                                    txtCubicleCapacity.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("CUBIC_CAPACITY").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("EX_SHOWROOM_PRICE") Then
                                    txtExShowRoomPrice.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("EX_SHOWROOM_PRICE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("INSURED_DECLARED_VALUE") Then
                                    txtInsuredDeclaredValue.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("INSURED_DECLARED_VALUE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_REGISTERED") Then
                                    ddlVechicleRegisteredinIndividualorCorparateName.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_REGISTERED").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("NEW_VEHICLE") Then
                                    chkNewVechicle.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("NEW_VEHICLE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("RENEWAL_OF_EXISTING_POLICY") Then
                                    chkRenewalofExistingPolicy.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("RENEWAL_OF_EXISTING_POLICY").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("RENEWAL_OF_EXISTING_POLICY") Then
                                    txtYearOfManufacture.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("RENEWAL_OF_EXISTING_POLICY").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("DATE_OF_FIRST_REGISTRATION") Then
                                    txtDateofFirstRegistration.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("DATE_OF_FIRST_REGISTRATION").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("COVER_TYPE") Then
                                    ddlCoverType.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("COVER_TYPE").Value).Selected = True
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("RTO_PIN_CODE") Then
                                    txtRTOPINCode.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("RTO_PIN_CODE").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("FC2_USED") Then
                                    hdnFC2.Value = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("FC2_USED").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_REG_STATE") Then
                                    txtVechicleRegstate.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_REG_STATE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_ZONE") Then
                                    txtVechicleZone.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_ZONE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("ADD_ON_COVERS") Then
                                    chkAddOnCovers.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("ADD_ON_COVERS").Value
                                    If chkAddOnCovers.Checked Then
                                        pnlAddOnCovers.Visible = True
                                    End If
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("AA_DISC") Then
                                    chkAutomobileAssociationMembership.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("AA_DISC").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("ARAI_ANTI_THEFT") Then
                                    chkARAIApprovedAntiTheftDeviceInstalled.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("ARAI_ANTI_THEFT").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VOLUNTARY_DEDUCTIBLE") Then
                                    ddlVolutaryDedecutible.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VOLUNTARY_DEDUCTIBLE").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("LEGAL_LIABILITY_FOR_PAID_DRIVER") Then
                                    chkLegalLiablityforpaidDriver.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("LEGAL_LIABILITY_FOR_PAID_DRIVER").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("EE_ACCESSORIES") Then
                                    txtElecticalElectronicAccessories.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("EE_ACCESSORIES").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("NON_E_ACCESSORIES") Then
                                    txtNonElecticalAccessories.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("NON_E_ACCESSORIES").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("PA_COVER") Then
                                    chkPACoveredforunnamedpersons.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("PA_COVER").Value
                                    If chkPACoveredforunnamedpersons.Checked Then
                                        pnlPACoveredforunnamedpersons.Visible = True
                                    End If
                                End If


                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("NO_OF_PERSONS") Then
                                    txtNoOfPerson.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("NO_OF_PERSONS").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("SUM_INSURED_PER_PERSON") Then
                                    txtSumInsuredPerPersonQQ.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("SUM_INSURED_PER_PERSON").Value
                                End If

                            End If
                        End If
                    End If



            Catch os As SamResponseException
                'should do some error handling here. Just output error for now

                Response.Write("An error occured calling SAM:<br>" & os.Message)
            Catch oe As Exception
                'should do some error handling here. Just output error for now

                Response.Write("An error occured:<br>" & oe.Message)
            Finally
                'clean up any objects here
            End Try
        Else


        End If
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Dim oDeleteRiskRequestType As New DeleteRiskRequestType
        Dim oDeleteRiskResponseType As New DeleteRiskResponseType

        'get the quote response
        'oAddRiskRequestType = CType(Session("AddRiskRequestType"), AddRiskRequestType)

        'set up request object with some values
        With oDeleteRiskRequestType
            .BranchCode = "Headoff"

            'add values from the quote we just added
            .InsuranceFolderKey = Session("InsuranceFolderKey")
            .InsuranceFileKey = Session("InsuranceFileKey")
            '.QuoteTimeStamp = QuoteTimeStamp
            .RiskKey = Session("RiskKey")
            .QuoteTimeStamp = Session("TimeStamp")
        End With

        Try
            StartDate = Date.Now
            oDeleteRiskResponseType = oSAM.DeleteRisk(oDeleteRiskRequestType)
            WriteToLog(Session, "MotorEditRiskQQ.aspx", "SAMForInsuranceV2", "DeleteRisk",StartDate, Date.Now)

            With oDeleteRiskResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                End If
            End With

            'output dataset to the screen to show results
            'add to session

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
        Catch oe As Exception
            'should do some error handling here. Just output error for now
        Finally
            'clean up any objects here
        End Try
    End Sub


    Protected Sub Menu1_MenuItemClick(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.MenuEventArgs) Handles Menu1.MenuItemClick
        MultiView1.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub
    Dim xmlDoc As New System.Xml.XmlDocument
    Dim PreseverCount As Integer
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click

        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        Dim xmlDatasetElementsToAdd(0) As XMLElementToAdd




        ''CoverNote
        'Dim oCoverSelXmlElement As XMLElementToAdd = Nothing
        'CoverNote(oCoverSelXmlElement)
        'xmlDatasetElementsToAdd(0) = oCoverSelXmlElement

        ''VehicleDetails

        'Dim oVehicleDetailXmlElement As XMLElementToAdd = Nothing
        'VehicleDetails(oVehicleDetailXmlElement)
        'xmlDatasetElementsToAdd(1) = oVehicleDetailXmlElement

        'QuickQuote

        Dim oQuickQuoteDetailXmlElement As XMLElementToAdd = Nothing
        QuickQuoteDetails(oQuickQuoteDetailXmlElement)
        xmlDatasetElementsToAdd(0) = oQuickQuoteDetailXmlElement


        Try


            Dim oElementToAddOrUpdate As System.Xml.XmlNode
            Dim oElementToAddOrUpdateAdded As System.Xml.XmlNode = Nothing
            Dim newAttribute As System.Xml.XmlAttribute
            Dim nNextOINumber As Integer
            Dim bNewElement As Boolean
            Dim oRequest As New UpdateRiskRequestType
            Dim oResponse As New UpdateRiskResponseType
            ' Read in the XML created in the AddRisk
            xmlDoc.LoadXml(Session("m_sRiskDataXML"))

            ' Get the NextOINumber and increment it
            nNextOINumber = xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value

            nNextOINumber = nNextOINumber + 1


            ' Create the new test elements
            For Each xmlDatasetElement As XMLElementToAdd In xmlDatasetElementsToAdd

                ' Check if the element already exists
                If xmlDatasetElement IsNot Nothing Then

                    oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                        .SelectSingleNode("RISK_OBJECTS") _
                                        .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                        .SelectSingleNode(xmlDatasetElement.ElementName)

                    If oElementToAddOrUpdate Is Nothing Then
                        oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                        newAttribute = xmlDoc.CreateAttribute("OI")
                        newAttribute.Value = "OI" & nNextOINumber.ToString
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                        nNextOINumber += 1

                        newAttribute = xmlDoc.CreateAttribute("US")
                        newAttribute.Value = "1"
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                        bNewElement = True
                        If xmlDatasetElement.Attributes.Length > 0 Then
                            For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                                newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                                newAttribute.Value = XMLAttribute.AttributeValue
                                oElementToAddOrUpdate.Attributes.Append(newAttribute)
                            Next
                        End If
                    Else
                        oElementToAddOrUpdate.Attributes("US").Value = "2"
                        For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                            newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                            newAttribute.Value = XMLAttribute.AttributeValue
                            oElementToAddOrUpdate.Attributes.Append(newAttribute)
                        Next
                        bNewElement = False
                    End If
                End If
            Next
            ' Write back the next OI number to the DATA_SET node
            nNextOINumber -= 1
            xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value = nNextOINumber

            With oRequest
                oRequest.TransactionType = "NB"
                .RiskKey = Session("RiskKey")
                .XMLDataSet = xmlDoc.OuterXml
                .QuoteTimeStamp = Session("TimeStamp")


                .BranchCode = "Headoff"
                .ScreenCode = "MOTPC"
                .RiskDescription = "Motor private Car"

                'add values from the quote we just added
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFileKey = Session("InsuranceFileKey")
            End With
            StartDate = Date.Now
            oResponse = oSAM.UpdateRisk(oRequest)
            WriteToLog(Session, "MotorEditRiskQQ.aspx", "SAMForInsuranceV2", "UpdateRisk",StartDate, Date.Now)

            m_sRiskDataXML = oResponse.XMLDataSet
            Session("TimeStamp") = oResponse.QuoteTimeStamp

            'm_dGrossPremium = oResponse.PremiumDueGross
            Response.Redirect("RiskPremiumDetails.aspx")
        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            If m_nRiskCnt <> 0 Then
                btnCancel_Click(sender, e)
            End If
            Response.Write("An error occured calling SAM:<br>" & os.Message)
        Catch oe As Exception
            'should do some error handling here. Just output error for now
            If Session("RiskKey") <> 0 Then
                btnCancel_Click(sender, e)
            End If
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try


    End Sub


    Public Sub CoverNote(ByRef oCoverSelXmlElement As XMLElementToAdd)
        'Start CoverSelection
        'Dim oCoverSelXmlElement As XMLElementToAdd
        Dim oCoverSelXmlAttribute(11) As XMLAttributeToAdd

        oCoverSelXmlElement = New XMLElementToAdd
        oCoverSelXmlElement.ElementName = "COVERSELECTION"


        oCoverSelXmlAttribute(10) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(10).AttributeName = "FULL_QUOTE"
        oCoverSelXmlAttribute(10).AttributeValue = 1

        oCoverSelXmlAttribute(11) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(11).AttributeName = "QUICK_QUOTE"
        oCoverSelXmlAttribute(11).AttributeValue = 0



        oCoverSelXmlElement.Attributes = oCoverSelXmlAttribute
        'End Cover Note
        'Return oCoverSelXmlElement
    End Sub

    Public Sub VehicleDetails(ByRef oVehicleXmlElement As XMLElementToAdd)
        Dim oXMLAttr(26) As XMLAttributeToAdd

        oVehicleXmlElement = New XMLElementToAdd
        oVehicleXmlElement.ElementName = "VEHICLEDETAILS"

        'oXMLAttr(0) = New XMLAttributeToAdd
        'oXMLAttr(0).AttributeName = "RESOLVED_NAME"
        'oXMLAttr(0).AttributeValue = txtr



        oVehicleXmlElement.Attributes = oXMLAttr

    End Sub

    Public Sub QuickQuoteDetails(ByRef oVehicleXmlElement As XMLElementToAdd)
        Dim oXMLAttr(27) As XMLAttributeToAdd

        oVehicleXmlElement = New XMLElementToAdd
        oVehicleXmlElement.ElementName = "QUICKQUOTE"


        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "FC1_USED"
        oXMLAttr(0).AttributeValue = hdnFC1.Value


        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "VEHICLE_MAKE"
        oXMLAttr(1).AttributeValue = txtVechicleMake.Text


        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "VEHICLE_MODEL"
        oXMLAttr(2).AttributeValue = txtVechicleModel.Text


        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "BODY_TYPE"
        oXMLAttr(3).AttributeValue = txtBodyTypeQQ.Text


        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "SEATING_CAPACITY"
        oXMLAttr(4).AttributeValue = txtSeatingCapacityQQ.Text


        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "CUBIC_CAPACITY"
        oXMLAttr(5).AttributeValue = txtCubicleCapacity.Text


        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "EX_SHOWROOM_PRICE"
        oXMLAttr(6).AttributeValue = txtExShowRoomPrice.Text


        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "INSURED_DECLARED_VALUE"
        oXMLAttr(7).AttributeValue = txtInsuredDeclaredValue.Text


        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "VEHICLE_REGISTERED"
        oXMLAttr(8).AttributeValue = ddlVechicleRegisteredinIndividualorCorparateName.SelectedValue


        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "NEW_VEHICLE"
        oXMLAttr(9).AttributeValue = Convert.ToInt32(chkNewVechicle.Checked)


        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "RENEWAL_OF_EXISTING_POLICY"
        oXMLAttr(10).AttributeValue = Convert.ToInt32(chkRenewalofExistingPolicy.Checked)


        oXMLAttr(11) = New XMLAttributeToAdd
        oXMLAttr(11).AttributeName = "YEAR_OF_MANUFACTURE"
        oXMLAttr(11).AttributeValue = txtYearOfManufacture.Text

        oXMLAttr(12) = New XMLAttributeToAdd
        oXMLAttr(12).AttributeName = "DATE_OF_FIRST_REGISTRATION"
        oXMLAttr(12).AttributeValue = txtDateofFirstRegistration.Text


        oXMLAttr(13) = New XMLAttributeToAdd
        oXMLAttr(13).AttributeName = "COVER_TYPE"
        oXMLAttr(13).AttributeValue = ddlCoverType.SelectedValue


        oXMLAttr(14) = New XMLAttributeToAdd
        oXMLAttr(14).AttributeName = "RTO_PIN_CODE"
        oXMLAttr(14).AttributeValue = txtRTOPINCode.Text


        oXMLAttr(15) = New XMLAttributeToAdd
        oXMLAttr(15).AttributeName = "FC2_USED"
        oXMLAttr(15).AttributeValue = hdnFC2.Value



        oXMLAttr(16) = New XMLAttributeToAdd
        oXMLAttr(16).AttributeName = "VEHICLE_REG_STATE"
        oXMLAttr(16).AttributeValue = txtVechicleRegstate.Text



        oXMLAttr(17) = New XMLAttributeToAdd
        oXMLAttr(17).AttributeName = "VEHICLE_ZONE"
        oXMLAttr(17).AttributeValue = txtVechicleZone.Text


        oXMLAttr(18) = New XMLAttributeToAdd
        oXMLAttr(18).AttributeName = "ADD_ON_COVERS"
        oXMLAttr(18).AttributeValue = Convert.ToInt32(chkAddOnCovers.Checked)


        oXMLAttr(19) = New XMLAttributeToAdd
        oXMLAttr(19).AttributeName = "AA_DISC"
        oXMLAttr(19).AttributeValue = Convert.ToInt32(chkAutomobileAssociationMembership.Checked)


        oXMLAttr(20) = New XMLAttributeToAdd
        oXMLAttr(20).AttributeName = "ARAI_ANTI_THEFT"
        oXMLAttr(20).AttributeValue = Convert.ToInt32(chkARAIApprovedAntiTheftDeviceInstalled.Checked)


        oXMLAttr(21) = New XMLAttributeToAdd
        oXMLAttr(21).AttributeName = "VOLUNTARY_DEDUCTIBLE"
        oXMLAttr(21).AttributeValue = ddlVolutaryDedecutible.SelectedValue


        oXMLAttr(22) = New XMLAttributeToAdd
        oXMLAttr(22).AttributeName = "LEGAL_LIABILITY_FOR_PAID_DRIVER"
        oXMLAttr(22).AttributeValue = Convert.ToInt32(chkLegalLiablityforpaidDriver.Checked)


        oXMLAttr(23) = New XMLAttributeToAdd
        oXMLAttr(23).AttributeName = "EE_ACCESSORIES"
        oXMLAttr(23).AttributeValue = txtElecticalElectronicAccessories.Text

        oXMLAttr(24) = New XMLAttributeToAdd
        oXMLAttr(24).AttributeName = "NON_E_ACCESSORIES"
        oXMLAttr(24).AttributeValue = txtNonElecticalAccessories.Text


        oXMLAttr(25) = New XMLAttributeToAdd
        oXMLAttr(25).AttributeName = "PA_COVER"
        oXMLAttr(25).AttributeValue = Convert.ToInt32(chkPACoveredforunnamedpersons.Checked)


        oXMLAttr(26) = New XMLAttributeToAdd
        oXMLAttr(26).AttributeName = "NO_OF_PERSONS"
        oXMLAttr(26).AttributeValue = txtNoOfPerson.Text


        oXMLAttr(27) = New XMLAttributeToAdd
        oXMLAttr(27).AttributeName = "SUM_INSURED_PER_PERSON"
        oXMLAttr(27).AttributeValue = txtSumInsuredPerPersonQQ.Text


        oVehicleXmlElement.Attributes = oXMLAttr

    End Sub


    Public Class XMLElementToAdd

        Private m_sName As String
        Private m_vXMLAttributes As XMLAttributeToAdd()

        Public Property ElementName() As String
            Get
                ElementName = m_sName
            End Get
            Set(ByVal value As String)
                m_sName = value
            End Set
        End Property
        Public Property Attributes() As XMLAttributeToAdd()
            Get
                Attributes = m_vXMLAttributes
            End Get
            Set(ByVal value As XMLAttributeToAdd())
                m_vXMLAttributes = value
            End Set
        End Property
    End Class

    Public Class XMLAttributeToAdd

        Private m_sName As String
        Private m_sValue As String

        Public Property AttributeName() As String
            Get
                AttributeName = m_sName
            End Get
            Set(ByVal value As String)
                m_sName = value
            End Set
        End Property
        Public Property AttributeValue() As String
            Get
                AttributeValue = m_sValue
            End Get
            Set(ByVal value As String)
                m_sValue = value
            End Set
        End Property
    End Class

    Private Sub BuildLists(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = ESTSLookup.UserDefinedTable
        oRequest.ListCode = ListCode


        Try
            
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "MotorEditRiskQQ.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "key"
                    objControl.DataBind()
                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub
    Private Sub BuildLists1(ByVal oSAM As SAMForInsuranceV2, ByRef objControl As DropDownList, ByVal ESTSLookup As STSListType, ByVal ListCode As String)
        Dim oRequest As New GetListRequestType
        Dim oResponse As New GetListResponseType


        oRequest.BranchCode = "HeadOff"
        oRequest.ListType = ESTSLookup.PMLookup
        oRequest.ListCode = ListCode


        Try
            
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "MotorEditRiskQQ.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "key"
                    objControl.DataBind()
                End If
                If ListCode = "source" Then
                    Session("Branch") = oResponse.List
                End If
            End With

        Catch os As SamResponseException
            'should do some error handling here. Just output error for now
            Response.Write("An error occured calling SAM:<br>" & os.Message)

        Catch oe As Exception
            'should do some error handling here. Just output error for now
            Response.Write("An error occured:<br>" & oe.Message)

        Finally
            'clean up any objects here
        End Try

    End Sub



    Protected Sub btnfindQQ1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfindQQ1.Click
        pnlVehicleDetails.Visible = True
        VehicleMake()

    End Sub

    Protected Sub BtnOkVD_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnOkVD.Click
        Try
            pnlVehicleDetails.Visible = False
            If gvVehicleDetails IsNot Nothing AndAlso gvVehicleDetails.Rows.Count > 0 AndAlso gvVehicleDetails.SelectedIndex <> -1 Then
                hdnFC1.Value = gvVehicleDetails.SelectedIndex + 1
                txtVechicleMake.Text = gvVehicleDetails.SelectedRow.Cells(5).Text
                txtVechicleModel.Text = gvVehicleDetails.SelectedRow.Cells(1).Text
                txtBodyTypeQQ.Text = gvVehicleDetails.SelectedRow.Cells(2).Text
                txtSeatingCapacityQQ.Text = gvVehicleDetails.SelectedRow.Cells(4).Text
                txtCubicleCapacity.Text = gvVehicleDetails.SelectedRow.Cells(3).Text

                txtVechicleMake.ReadOnly = True
                txtVechicleModel.ReadOnly = True
                txtBodyTypeQQ.ReadOnly = True
                txtSeatingCapacityQQ.ReadOnly = True
                txtCubicleCapacity.ReadOnly = True

            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub VehicleMake()
        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim strconn As String = ConfigurationManager.AppSettings("SiriusConn").ToString()
        conn.ConnectionString = strconn
        conn.Open()
        cmd.Connection = conn
        'cmd.CommandText = "Select description,BodyType,CC,SeatCap,Make from UDL_VEHMAKEMAST where is_deleted = 0  and Make like '" & ddlVehicleMake.SelectedItem.Text & "'  and effective_date <=   '" & Today & "' " '{ts'2008-08-26 16:38:32'}"
        cmd.CommandText = "Select description,BodyType,CC,SeatCap,Make from UDL_VEHMAKEMAST where is_deleted = 0  and effective_date <=   '" & Today & "' " '{ts'2008-08-26 16:38:32'}"
        Dim dr As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
        Dim dt As New DataTable
        dt.Load(dr)
        gvVehicleDetails.DataSource = dt
        gvVehicleDetails.DataBind()

    End Sub

    Public Sub VehicleRegistrationAddress()
        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim strconn As String = ConfigurationManager.AppSettings("SiriusConn").ToString() '"Data Source=localhost;Initial Catalog=Sirius_UIIC;User ID=sa"
        conn.ConnectionString = strconn
        conn.Open()
        cmd.Connection = conn
        cmd.CommandText = "Select State,Zone from UDL_RTO_MSTR where is_deleted = 0 and effective_date <=  '" & Today & "'"
        Dim dr As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
        Dim dt As New DataTable
        dt.Load(dr)
        GvVechicleZone.DataSource = dt
        GvVechicleZone.DataBind()
    End Sub

    Protected Sub btnFindQQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindQQ.Click
        pnlVechicleZone.Visible = True
        VehicleRegistrationAddress()
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            pnlVechicleZone.Visible = False
            If GvVechicleZone IsNot Nothing AndAlso GvVechicleZone.Rows.Count > 0 AndAlso GvVechicleZone.SelectedIndex <> -1 Then
                hdnFC2.Value = GvVechicleZone.SelectedIndex + 1
                txtVechicleRegstate.Text = GvVechicleZone.SelectedRow.Cells(1).Text
                txtVechicleZone.Text = GvVechicleZone.SelectedRow.Cells(2).Text

                txtVechicleRegstate.ReadOnly = True
                txtVechicleZone.ReadOnly = True


            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub chkPACoveredforunnamedpersons_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkPACoveredforunnamedpersons.CheckedChanged
        If pnlPACoveredforunnamedpersons.Visible = True Then
            pnlPACoveredforunnamedpersons.Visible = False
        Else
            pnlPACoveredforunnamedpersons.Visible = True
        End If

        txtNoOfPerson.Text = ""
        txtSumInsuredPerPersonQQ.Text = ""

    End Sub

    Protected Sub chkAddOnCovers_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkAddOnCovers.CheckedChanged
        If pnlAddOnCovers.Visible = True Then
            pnlAddOnCovers.Visible = False
        Else
            pnlAddOnCovers.Visible = True
        End If
        txtNoOfPerson.Text = ""
        txtSumInsuredPerPersonQQ.Text = ""
        chkLegalLiablityforpaidDriver.Checked = False
        chkPACoveredforunnamedpersons.Checked = False
        txtElecticalElectronicAccessories.Text = ""
        txtNonElecticalAccessories.Text = ""

    End Sub

    Protected Sub gvVehicleDetails_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvVehicleDetails.SelectedIndexChanging

    End Sub

    Public Sub BuildXMLNode(ByVal xdoc As XmlDocument, ByVal element As String)
        'Dim xpathDoc As XPathDocument
        Dim xmlNav As XPathNavigator
        Dim xmlNI As XPathNodeIterator
        Dim str As String
        xmlNav = xdoc.CreateNavigator()
        xmlNI = xmlNav.Select("/DATA_SET/RISK_OBJECTS/MOTOR_POLICY_BINDER")
        While xmlNI.MoveNext()
            If xmlNI.Current.Value = "PIHISTORY" Then
                str &= xmlNI.Current.InnerXml
            End If
        End While
    End Sub

    Protected Sub chkswaptofullquote_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkswaptofullquote.CheckedChanged
        Session("QUICKQUOTE") = "1"
        Response.Redirect("MotorEditRisk.aspx")

    End Sub

    Protected Sub btnCancleQQ1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancleQQ1.Click
        hdnFC1.Value = ""
        txtVechicleMake.Text = ""
        txtVechicleModel.Text = ""
        txtBodyTypeQQ.Text = ""
        txtSeatingCapacityQQ.Text = ""
        txtCubicleCapacity.Text = ""



        txtVechicleMake.ReadOnly = False
        txtVechicleModel.ReadOnly = False
        txtBodyTypeQQ.ReadOnly = False
        txtSeatingCapacityQQ.ReadOnly = False
        txtCubicleCapacity.ReadOnly = False

        pnlVehicleDetails.Visible = False
    End Sub

    Protected Sub btnCancleQQ_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancleQQ.Click
        hdnFC2.Value = ""
        txtVechicleRegstate.Text = ""
        txtVechicleZone.Text = ""


        txtVechicleRegstate.ReadOnly = False
        txtVechicleZone.ReadOnly = False

        pnlVechicleZone.Visible = False
    End Sub

    Protected Sub txtNoOfPerson_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNoOfPerson.TextChanged
        If txtNoOfPerson.Text > "4" Then
            txtNoOfPerson.Text = ""
        End If
    End Sub

    Protected Sub txtSumInsuredPerPersonQQ_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSumInsuredPerPersonQQ.TextChanged
        If txtSumInsuredPerPersonQQ.Text > "200000" Then
            txtSumInsuredPerPersonQQ.Text = ""
        End If
    End Sub
End Class


Public Class XMLElementToAdd

    Private m_sName As String
    Private m_vXMLAttributes As XMLAttributeToAdd()

    Public Property ElementName() As String
        Get
            ElementName = m_sName
        End Get
        Set(ByVal value As String)
            m_sName = value
        End Set
    End Property
    Public Property Attributes() As XMLAttributeToAdd()
        Get
            Attributes = m_vXMLAttributes
        End Get
        Set(ByVal value As XMLAttributeToAdd())
            m_vXMLAttributes = value
        End Set
    End Property
End Class
Public Class XMLAttributeToAdd

    Private m_sName As String
    Private m_sValue As String

    Public Property AttributeName() As String
        Get
            AttributeName = m_sName
        End Get
        Set(ByVal value As String)
            m_sName = value
        End Set
    End Property
    Public Property AttributeValue() As String
        Get
            AttributeValue = m_sValue
        End Get
        Set(ByVal value As String)
            m_sValue = value
        End Set
    End Property
End Class