Imports System.Xml
Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data

Partial Class New_Business_MotorCar
    Inherits System.Web.UI.Page
    Dim StartDate As Date
    Dim UserToken As UsernameToken
    Dim m_nRiskCnt As Integer
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2
    Dim m_sRiskDataXML As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblNewVehicle.Visible = False
        lblNewVehicle1.Visible = False
        txtSeatingcapacity.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtCC.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtExShowRoom.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtIDV.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNumberoftrailers.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtTrailersIDV.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtPinCode.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtFinancierPincode.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        InsAddPinCode.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNumberofClaims.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtTotalamountofclaims.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtIncurredLossRatio.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtelectricalSerialNumber.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtelecricalProductserialNumber.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtelecricalSumInsured.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txttotalSumInsured.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNonelectricalSerialNumber.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNonelectricalProductserialNumber.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNonelectricalSumInsured.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtAge.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtCNGSI.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtLPGSI.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtDiscountAgentCommission.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtDiscountGoodfeature.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtDiscountSpecial.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtImposedExcess.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNumberOfPaiddrivesCleanersWorkMan.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNumberOFEmployeeTravellingDriving.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtSumInsured1.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNumberOfunnamedPersons.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtSumInsuredPerPerson.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtNumberOfpaiddrivers.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtSumInsured2.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtSerialnumber.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtsumInsured.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")
        txtPAPin.Attributes.Add("onkeypress", "IsNumFieldKeyPress()")




        If Not IsPostBack Then
            pnlVehicleMake.Visible = False
            pnlFinancierDetails1.Visible = False
            pnlVehicleAddress.Visible = False
            txtPolicyStartDate.Text = Today
            txtTime1.Text = DateTime.Now.TimeOfDay.Hours & ":" & DateTime.Now.TimeOfDay.Minutes
            EnableControl()
            pnlPIHistory.Visible = False
            pnlElectrical.Visible = False
            pnlNonelectrical.Visible = False
            pnlDriverDetails.Visible = False
            pnlPersonalaccidentDetails.Visible = False
            pnlfinancierDetails.Visible = False
            UserToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            'create the request and response objects
            Dim oAddRiskRequestType As New AddRiskRequestType
            Dim oAddRiskResponseType As New AddRiskResponseType



            'select * from dbo.GIS_User_Def_Header 
            'get the quote response
            BuildLists(oSAM, ddlCoverOptionpackage, STSListType.UserDefinedTable, "CVR_OPTION")
            BuildLists(oSAM, ddlPolicyType, STSListType.UserDefinedTable, "CVR_DETAIL")
            BuildLists1(oSAM, ddlVehicleMake, STSListType.PMLookup, "udl_vehMake")
            If ddlVehicleMake.Items.Count > 0 Then
                ddlVehicleMake.Items.FindByText("%").Selected = True
            End If

            BuildLists1(oSAM, ddlDateOf, STSListType.PMLookup, "udl_vehdates")
            BuildLists(oSAM, ddlPloicyType, STSListType.UserDefinedTable, "POLCY_TYPE")
            BuildLists(oSAM, DDLPreviousYearNCB, STSListType.UserDefinedTable, "NOCLM_BONS")
            BuildLists(oSAM, ddlVechicleParked, STSListType.UserDefinedTable, "VEH_PARKNG")
            BuildLists(oSAM, ddlVechiclemainlyused, STSListType.UserDefinedTable, "MN_VEH_USE")
            BuildLists(oSAM, ddlusuallydriven, STSListType.UserDefinedTable, "ROADS_TYPE")
            BuildLists(oSAM, ddlVoluntaryExcessamount, STSListType.UserDefinedTable, "VL_EXS_AMT")
            BuildLists(oSAM, ddlagreementType, STSListType.UserDefinedTable, "FINAN_TYPE")
            BuildLists1(oSAM, ddlDetarriffDiscount, STSListType.PMLookup, "udl_dtdiscount")

            'for Panel Driver Details
            BuildLists(oSAM, ddlvehicledrivenby, STSListType.UserDefinedTable, "VEH_DRVNBY")

            BuildLists(oSAM, ddlGender, STSListType.UserDefinedTable, "GENDER")
            BuildLists(oSAM, ddlRelationshipwithowner, STSListType.UserDefinedTable, "REL_WD_ONR")

            BuildLists(oSAM, ddlTypeoflicence, STSListType.UserDefinedTable, "LICNS_TYPE")
            ' BuildLists(oSAM, ddlNoClaims, STSListType.UserDefinedTable, "NOCLM_BONS")
            BuildLists1(oSAM, ddlFinancerType, STSListType.PMLookup, "UDL_FNCRTYPE")


            'for financer Details






            'BuildLists(oSAM, ddlVehicleMake, STSListType.UserDefinedTable, "VEH_MAKE")

            'set up request object with some values
            With oAddRiskRequestType
                .BranchCode = "Headoff"
                .RiskTypeCode = "MOTORPC"
                .ScreenCode = "MOTPC"
                .DataModelCode = "MOTOR"
                .RunDefaultRules = True
                .RiskDescription = "Motor private Car"


                'add values from the quote we just added
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFileKey = Session("InsuranceFileKey")
                .QuoteTimeStamp = Session("TimeStamp")
                .ProductCode = "MOTOR"

            End With

            Try
                StartDate = Date.Now
                oAddRiskResponseType = oSAM.AddRisk(oAddRiskRequestType)
                WriteToLog(Session, "MotorAddRisk.aspx", "SAMForInsuranceV2", "AddRisk",StartDate, Date.Now)

                Session("RiskKey") = oAddRiskResponseType.RiskKey
                With oAddRiskResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    End If
                    Session("m_sRiskDataXML") = .XMLDataSet.ToString
                    Session("TimeStamp") = .QuoteTimeStamp
                End With
                Dim xmlDoc1 As New System.Xml.XmlDocument
                xmlDoc1.LoadXml(Session("m_sRiskDataXML"))
               

                'Start Claim History
                Dim oElement_MOTOR_POLICY_BINDER As System.Xml.XmlNode = xmlDoc1.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER")
                'For Grid binding of Claim History
                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION") IsNot Nothing AndAlso oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasChildNodes Then
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
                'output dataset to the screen to show results

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
            WriteToLog(Session, "MotorAddRisk.aspx", "SAMForInsuranceV2", "DeleteRisk",StartDate, Date.Now)

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
        MCView.ActiveViewIndex = Int32.Parse(e.Item.Value)
    End Sub
    Dim xmlDoc As New System.Xml.XmlDocument
    Dim PreseverCount As Integer
    Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
        Try
            If chkNewVehicle.Checked = False Then
                If (txtReg1.Text = String.Empty Or txtReg2.Text = String.Empty) Then
                    lblNewVehicle.Visible = True
                    'lblNewVehicle1.Visible = True
                    Exit Sub
                End If
            End If

            UserToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim xmlDatasetElementsToAdd(7) As XMLElementToAdd


            '****Start View of 1- CoverSelection*****

            'CoverNote
            Dim oCoverSelXmlElement As XMLElementToAdd = Nothing
            CoverNote(oCoverSelXmlElement)
            xmlDatasetElementsToAdd(0) = oCoverSelXmlElement

            'VehicleDetails

            Dim oVehicleDetailXmlElement As XMLElementToAdd = Nothing
            VehicleDetails(oVehicleDetailXmlElement)
            xmlDatasetElementsToAdd(1) = oVehicleDetailXmlElement


            'Financier

            Dim oFinancierXmlElement As XMLElementToAdd = Nothing
            Financier(oFinancierXmlElement)
            xmlDatasetElementsToAdd(2) = oFinancierXmlElement




            '******End View of 1- CoverSelection*******


            '******Start view 2- Additional Cover



            '******End view 2- Additional Cover


            '******Start view 3- Additional Cover continue..

            'VehicleUsage
            Dim oVehicleUsageXmlElement As XMLElementToAdd = Nothing
            VehicleUsage(oVehicleUsageXmlElement)
            xmlDatasetElementsToAdd(3) = oVehicleUsageXmlElement

            'Additional Cover Details
            Dim oAddtionalCoverDetailsXmlElement As XMLElementToAdd = Nothing
            AdditionalCoverDetails(oAddtionalCoverDetailsXmlElement)
            xmlDatasetElementsToAdd(4) = oAddtionalCoverDetailsXmlElement

            '******End view 3- Additional Cover continue..


            '****Start view of 4- PreviousInsurance*****
            Dim oPreviousInsXmlElement As XMLElementToAdd = Nothing
            PreviousInsurance(oPreviousInsXmlElement)
            xmlDatasetElementsToAdd(5) = oPreviousInsXmlElement




            'If gvPreviousInsurancehistory IsNot Nothing AndAlso gvPreviousInsurancehistory.Rows.Count > 0 Then

            '    Dim oPreviousInsHistoryXmlElement As XMLElementToAdd = Nothing
            '    PreviousInsuranceHistory(oPreviousInsHistoryXmlElement)
            '    xmlDatasetElementsToAdd(6) = oPreviousInsHistoryXmlElement
            'End If

            '*****end view of 4- PreviousInsurance****

            '****Start view of 5- Legal Liability*****

            'Legal Liability
            Dim oLegalLiabilityXmlElement As XMLElementToAdd = Nothing

            LegalLiability1(oLegalLiabilityXmlElement)
            xmlDatasetElementsToAdd(6) = oLegalLiabilityXmlElement

            'Personal Accident
            Dim oPersonalAccidentXmlElement As XMLElementToAdd = Nothing
            PersonalAccident(oPersonalAccidentXmlElement)
            xmlDatasetElementsToAdd(7) = oPersonalAccidentXmlElement

            '****End view of 5- Legal Liability*****

            'Start ***** 7-Claim History
            'Dim oCliamHistoryXmlElement As XMLElementToAdd = Nothing
            'ClaimHistory(oCliamHistoryXmlElement)
            'xmlDatasetElementsToAdd(8) = oCliamHistoryXmlElement

            If gvElectricalaccessories IsNot Nothing AndAlso gvElectricalaccessories.Rows.Count > 0 Then
                PreseverCount = xmlDatasetElementsToAdd.GetUpperBound(0)
                Dim iElementCount As Integer = xmlDatasetElementsToAdd.GetUpperBound(0) + gvElectricalaccessories.Rows.Count
                ReDim Preserve xmlDatasetElementsToAdd(iElementCount)
                For iCount As Integer = 0 To gvElectricalaccessories.Rows.Count - 1
                    Dim oElectricalXmlElement As XMLElementToAdd = Nothing
                    ElectricalDetails(oElectricalXmlElement, iCount)
                    xmlDatasetElementsToAdd(PreseverCount + iCount + 1) = oElectricalXmlElement
                Next
            End If

            If gvNonElecTrical IsNot Nothing AndAlso gvNonElecTrical.Rows.Count > 0 Then
                PreseverCount = xmlDatasetElementsToAdd.GetUpperBound(0)
                Dim iElementCount As Integer = xmlDatasetElementsToAdd.GetUpperBound(0) + gvNonElecTrical.Rows.Count
                ReDim Preserve xmlDatasetElementsToAdd(iElementCount)
                For iCount As Integer = 0 To gvNonElecTrical.Rows.Count - 1
                    Dim oNonElectricalXmlElement As XMLElementToAdd = Nothing
                    NONElectricalDetails(oNonElectricalXmlElement, iCount)
                    xmlDatasetElementsToAdd(PreseverCount + iCount + 1) = oNonElectricalXmlElement
                Next

            End If

            'FinancierDetails
            If gvFinancierDetails IsNot Nothing AndAlso gvFinancierDetails.Rows.Count > 0 Then
                PreseverCount = xmlDatasetElementsToAdd.GetUpperBound(0)
                Dim iElementCount As Integer = xmlDatasetElementsToAdd.GetUpperBound(0) + gvFinancierDetails.Rows.Count
                ReDim Preserve xmlDatasetElementsToAdd(iElementCount)
                For iCount As Integer = 0 To gvFinancierDetails.Rows.Count - 1
                    Dim oFinancierDetailXmlElement As XMLElementToAdd = Nothing
                    FinancierDetails(oFinancierDetailXmlElement, iCount)
                    xmlDatasetElementsToAdd(PreseverCount + iCount + 1) = oFinancierDetailXmlElement
                Next

            End If

            'DriverDetails

            If gvDriverDetails IsNot Nothing AndAlso gvDriverDetails.Rows.Count > 0 Then
                PreseverCount = xmlDatasetElementsToAdd.GetUpperBound(0)
                Dim iElementCount As Integer = xmlDatasetElementsToAdd.GetUpperBound(0) + gvDriverDetails.Rows.Count
                ReDim Preserve xmlDatasetElementsToAdd(iElementCount)
                For iCount As Integer = 0 To gvDriverDetails.Rows.Count - 1
                    Dim oDriverDetailXmlElement As XMLElementToAdd = Nothing
                    DriverDetails(oDriverDetailXmlElement, iCount)
                    xmlDatasetElementsToAdd(PreseverCount + iCount + 1) = oDriverDetailXmlElement
                Next

            End If

            'PIHISTORY
            If gvPreviousInsurancehistory IsNot Nothing AndAlso gvPreviousInsurancehistory.Rows.Count > 0 Then
                PreseverCount = xmlDatasetElementsToAdd.GetUpperBound(0)
                Dim iElementCount As Integer = xmlDatasetElementsToAdd.GetUpperBound(0) + gvPreviousInsurancehistory.Rows.Count
                ReDim Preserve xmlDatasetElementsToAdd(iElementCount)
                For iCount As Integer = 0 To gvPreviousInsurancehistory.Rows.Count - 1
                    Dim oPreviousInsHistoryXmlElement As XMLElementToAdd = Nothing
                    PreviousInsuranceHistory(oPreviousInsHistoryXmlElement, iCount)
                    xmlDatasetElementsToAdd(PreseverCount + iCount + 1) = oPreviousInsHistoryXmlElement
                Next
            End If

            'Personal Accident Details
            If gvpersonalAccidentDetails IsNot Nothing AndAlso gvpersonalAccidentDetails.Rows.Count > 0 Then
                PreseverCount = xmlDatasetElementsToAdd.GetUpperBound(0)
                Dim iElementCount As Integer = xmlDatasetElementsToAdd.GetUpperBound(0) + gvpersonalAccidentDetails.Rows.Count
                ReDim Preserve xmlDatasetElementsToAdd(iElementCount)
                For iCOunt As Integer = 0 To gvpersonalAccidentDetails.Rows.Count - 1
                    Dim oPersonalAccidentDetailsXmlElement As XMLElementToAdd = Nothing
                    PersonalAccidentDetails(oPersonalAccidentDetailsXmlElement, iCOunt)
                    xmlDatasetElementsToAdd(PreseverCount + iCOunt + 1) = oPersonalAccidentDetailsXmlElement
                Next

            End If
            'End ***** 7-Claim History



            'Dim oXMLElement As XMLElementToAdd
            'Dim oXMLElement1 As XMLElementToAdd
            'Dim oXMLAttr1(0) As XMLAttributeToAdd

            'oXMLElement1 = New XMLElementToAdd
            'oXMLElement1.ElementName = "PREVIOUSINSURANCE"

            'oXMLAttr1(0) = New XMLAttributeToAdd
            'oXMLAttr1(0).AttributeName = "POLICY_EXPIRY_DATE"
            'oXMLAttr1(0).AttributeValue = txtPreviousPolicyExpiryDate.Text

            'oXMLElement1.Attributes = oXMLAttr1


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


            'Dim xmlDatasetElementsToAdd(1) As XMLElementToAdd
            'xmlDatasetElementsToAdd(0) = oXMLElement
            'xmlDatasetElementsToAdd(1) = oXMLElement1

            ' Create the new test elements
            For Each xmlDatasetElement As XMLElementToAdd In xmlDatasetElementsToAdd

                ' Check if the element already exists
                If xmlDatasetElement IsNot Nothing Then


                    oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                        .SelectSingleNode("RISK_OBJECTS") _
                                        .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                        .SelectSingleNode(xmlDatasetElement.ElementName)
                    ' If not, create it
                    If oElementToAddOrUpdate Is Nothing Then
                        oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                        ' Add the common Object Instance (OI) and Update
                        ' Status (US) attributes
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
                        'If xmlDatasetElement.ElementName = "PIHISTORY" Then
                        '    PIToADD(xmlDatasetElementsToAdd(6), oElementToAddOrUpdateAdded)
                        'End If
                    Else
                        ' Update the Update
                        ' Status (US) attribute
                        oElementToAddOrUpdate.Attributes("US").Value = "2"

                        For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                            newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                            newAttribute.Value = XMLAttribute.AttributeValue
                            oElementToAddOrUpdate.Attributes.Append(newAttribute)
                        Next

                        bNewElement = False
                    End If



                    If bNewElement Then

                        ' Append the new element node to the XML under the POLICY BINDER
                        If xmlDatasetElement.ElementName = "PIHISTORY" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("PREVIOUSINSURANCE") _
                                .AppendChild(oElementToAddOrUpdate)

                        ElseIf xmlDatasetElement.ElementName = "DRIVERDETAILS" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("COVERSELECTION") _
                                .AppendChild(oElementToAddOrUpdate)

                        ElseIf xmlDatasetElement.ElementName = "NONELECTRICAL" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("COVERSELECTION") _
                                .AppendChild(oElementToAddOrUpdate)

                        ElseIf xmlDatasetElement.ElementName = "ELECTRICAL" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("COVERSELECTION") _
                                .AppendChild(oElementToAddOrUpdate)


                        ElseIf xmlDatasetElement.ElementName = "DRIVERDETAILS" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("ADDITIONALCOVER") _
                                .AppendChild(oElementToAddOrUpdate)

                        ElseIf xmlDatasetElement.ElementName = "PADETAILS" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("PERSONALACCIDENT") _
                                .AppendChild(oElementToAddOrUpdate)

                        ElseIf xmlDatasetElement.ElementName = "FINANCIERDETAILS" Then
                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("FINANCIER") _
                                .AppendChild(oElementToAddOrUpdate)
                        Else

                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                                    .SelectSingleNode("RISK_OBJECTS") _
                                                    .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                                    .AppendChild(oElementToAddOrUpdate)
                        End If

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
            WriteToLog(Session, "MotorAddRisk.aspx", "SAMForInsuranceV2", "UpdateRisk",StartDate, Date.Now)
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
           
            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try

    End Sub

#Region "Filling the XML"

    Public Sub CoverNote(ByRef oCoverSelXmlElement As XMLElementToAdd)
        'Start CoverSelection
        'Dim oCoverSelXmlElement As XMLElementToAdd
        Dim oCoverSelXmlAttribute(14) As XMLAttributeToAdd

        oCoverSelXmlElement = New XMLElementToAdd
        oCoverSelXmlElement.ElementName = "COVERSELECTION"

        oCoverSelXmlAttribute(0) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(0).AttributeName = "POLICY_START_DATE"
        oCoverSelXmlAttribute(0).AttributeValue = FormatYYMMDD(txtPolicyStartDate.Text)

        'oCoverSelXmlAttribute(0).AttributeValue = Convert.ToDateTime(txtPolicyStartDate.Text).ToString("yy-MM-dd") 'txtPolicyStartDate.Text

        oCoverSelXmlAttribute(1) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(1).AttributeName = "POLICY_START_TIME"
        oCoverSelXmlAttribute(1).AttributeValue = txtTime1.Text

        oCoverSelXmlAttribute(2) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(2).AttributeName = "COVER_NOTE_DATE"
        oCoverSelXmlAttribute(2).AttributeValue = FormatYYMMDD(txtCoverNoteDate.Text)

        oCoverSelXmlAttribute(3) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(3).AttributeName = "COVER_NOTE_TIME"
        oCoverSelXmlAttribute(3).AttributeValue = txtTime2.Text

        oCoverSelXmlAttribute(4) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(4).AttributeName = "COVER_OPTIONS_PACKAGE"
        oCoverSelXmlAttribute(4).AttributeValue = ddlCoverOptionpackage.SelectedValue

        oCoverSelXmlAttribute(5) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(5).AttributeName = "COVERAGE_SELECTED"
        oCoverSelXmlAttribute(5).AttributeValue = Convert.ToInt32(ChkRestrictedCover.Checked)

        oCoverSelXmlAttribute(6) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(6).AttributeName = "ADD_ON_COVER_PACKAGE"
        oCoverSelXmlAttribute(6).AttributeValue = Convert.ToInt32(chkAddonCover.Checked)


        oCoverSelXmlAttribute(7) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(7).AttributeName = "ELECTRICICAL_ACCESSORIES"
        oCoverSelXmlAttribute(7).AttributeValue = Convert.ToInt32(chkElectrical.Checked)

        oCoverSelXmlAttribute(8) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(8).AttributeName = "NON_ELECTRICAL_ACCESSORIES"
        oCoverSelXmlAttribute(8).AttributeValue = Convert.ToInt32(chkNonElectricalAccessoriesCovered.Checked)

        oCoverSelXmlAttribute(9) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(9).AttributeName = "TRAILER"
        oCoverSelXmlAttribute(9).AttributeValue = Convert.ToInt32(ChkTrailer.Checked)

        oCoverSelXmlAttribute(10) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(10).AttributeName = "FULL_QUOTE"
        oCoverSelXmlAttribute(10).AttributeValue = 1

        oCoverSelXmlAttribute(11) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(11).AttributeName = "QUICK_QUOTE"
        oCoverSelXmlAttribute(11).AttributeValue = 0

        oCoverSelXmlAttribute(12) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(12).AttributeName = "COVER_DETAILS"
        oCoverSelXmlAttribute(12).AttributeValue = ddlPolicyType.SelectedValue

        oCoverSelXmlAttribute(13) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(13).AttributeName = "TOTAL_ELEC_SI"
        oCoverSelXmlAttribute(13).AttributeValue = Val(txttotalSumInsured.Text)

        oCoverSelXmlAttribute(14) = New XMLAttributeToAdd
        oCoverSelXmlAttribute(14).AttributeName = "TOTAL_NONELEC_SI"
        oCoverSelXmlAttribute(14).AttributeValue = Val(txtSumInsurNonelec.Text)


        If ChkTrailer.Checked Then
            ReDim Preserve oCoverSelXmlAttribute(17)
            oCoverSelXmlAttribute(15) = New XMLAttributeToAdd
            oCoverSelXmlAttribute(15).AttributeName = "NUMBER_OF_TRAILERS"
            oCoverSelXmlAttribute(15).AttributeValue = txtNumberoftrailers.Text

            oCoverSelXmlAttribute(16) = New XMLAttributeToAdd
            oCoverSelXmlAttribute(16).AttributeName = "TRAILER_IDV"
            oCoverSelXmlAttribute(16).AttributeValue = txtTrailersIDV.Text

            oCoverSelXmlAttribute(17) = New XMLAttributeToAdd
            oCoverSelXmlAttribute(17).AttributeName = "TRAILER_REGISTRATION_NUMBER"
            oCoverSelXmlAttribute(17).AttributeValue = txtTrailersIDV.Text
        End If




        oCoverSelXmlElement.Attributes = oCoverSelXmlAttribute
        'End Cover Note
        'Return oCoverSelXmlElement
    End Sub

    Public Sub VehicleDetails(ByRef oVehicleXmlElement As XMLElementToAdd)
        Dim oXMLAttr(26) As XMLAttributeToAdd

        oVehicleXmlElement = New XMLElementToAdd
        oVehicleXmlElement.ElementName = "VEHICLEDETAILS"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "VEHICLE_MAKE"
        oXMLAttr(0).AttributeValue = ddlVehicleMake.SelectedValue

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "VEHICLE_MODEL"
        oXMLAttr(1).AttributeValue = txtVehicleModel.Text

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "BODY_TYPE"
        oXMLAttr(2).AttributeValue = txtBodyType.Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "SEATING_CAPACITY"
        oXMLAttr(3).AttributeValue = txtSeatingcapacity.Text



        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "YEAR_MANUFACTURING"
        oXMLAttr(4).AttributeValue = txtYearManufacture.Text

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "ENGINE_NUMBER"
        oXMLAttr(5).AttributeValue = txtEngineNumber.Text

        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "CHASSIS_NUMBER"
        oXMLAttr(6).AttributeValue = txtChasisNumber.Text

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "VEH_REG_ADD1"
        oXMLAttr(7).AttributeValue = txtLine1.Text

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "VEH_REG_ADD2"
        oXMLAttr(8).AttributeValue = txtLine2.Text

        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "VEH_REG_CITY"
        oXMLAttr(9).AttributeValue = txtCity.Text

        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "VEH_REG_STATE"
        oXMLAttr(10).AttributeValue = txtState.Text

        oXMLAttr(11) = New XMLAttributeToAdd
        oXMLAttr(11).AttributeName = "VEH_REG_PINCODE"
        oXMLAttr(11).AttributeValue = txtPinCode.Text
        oVehicleXmlElement.Attributes = oXMLAttr

        oXMLAttr(12) = New XMLAttributeToAdd
        oXMLAttr(12).AttributeName = "CUBIC_CAPACITY"
        oXMLAttr(12).AttributeValue = txtCC.Text

        oXMLAttr(13) = New XMLAttributeToAdd
        oXMLAttr(13).AttributeName = "VEHICLE_COLOR"
        oXMLAttr(13).AttributeValue = txtVehicleColor.Text

        oXMLAttr(14) = New XMLAttributeToAdd
        oXMLAttr(14).AttributeName = "EX_SHOWROOM_PRICE"
        oXMLAttr(14).AttributeValue = txtExShowRoom.Text


        oXMLAttr(15) = New XMLAttributeToAdd
        oXMLAttr(15).AttributeName = "EX_SHOWROOM_PRICE"
        oXMLAttr(15).AttributeValue = txtExShowRoom.Text

        oXMLAttr(16) = New XMLAttributeToAdd
        oXMLAttr(16).AttributeName = "VEHICLE_ZONE"
        oXMLAttr(16).AttributeValue = txtZone.Text

        oXMLAttr(17) = New XMLAttributeToAdd
        oXMLAttr(17).AttributeName = "SYSTEM_GENERATED_IDV"
        oXMLAttr(17).AttributeValue = txtIDVSystem.Text

        oXMLAttr(18) = New XMLAttributeToAdd
        oXMLAttr(18).AttributeName = "DATE_OF"
        oXMLAttr(18).AttributeValue = ddlDateOf.SelectedValue

        oXMLAttr(19) = New XMLAttributeToAdd
        oXMLAttr(19).AttributeName = "INSURED_DECLARED_VALUE"
        oXMLAttr(19).AttributeValue = txtIDV.Text

        oXMLAttr(20) = New XMLAttributeToAdd
        oXMLAttr(20).AttributeName = "DATE_FIRST_REGISTRATION"
        oXMLAttr(20).AttributeValue = FormatYYMMDD(txtDate.Text)

        oXMLAttr(21) = New XMLAttributeToAdd
        oXMLAttr(21).AttributeName = "NEW_VEHICLE"
        oXMLAttr(21).AttributeValue = Convert.ToInt32(chkNewVehicle.Checked)

        oXMLAttr(22) = New XMLAttributeToAdd
        oXMLAttr(22).AttributeName = "REG_1"
        oXMLAttr(22).AttributeValue = txtReg1.Text

        oXMLAttr(23) = New XMLAttributeToAdd
        oXMLAttr(23).AttributeName = "REG_2"
        oXMLAttr(23).AttributeValue = txtReg2.Text

        oXMLAttr(24) = New XMLAttributeToAdd
        oXMLAttr(24).AttributeName = "REG_3"
        oXMLAttr(24).AttributeValue = txtReg3.Text

        oXMLAttr(25) = New XMLAttributeToAdd
        oXMLAttr(25).AttributeName = "REG_4"
        oXMLAttr(25).AttributeValue = txtReg4.Text

        oXMLAttr(26) = New XMLAttributeToAdd
        oXMLAttr(26).AttributeName = "RTO_MASTER"
        oXMLAttr(26).AttributeValue = txtRTOLocation.Text

        oVehicleXmlElement.Attributes = oXMLAttr

    End Sub

    Public Sub PreviousInsurance(ByRef oPreviousInsXmlElement As XMLElementToAdd)

        Dim oXMLAttr(16) As XMLAttributeToAdd

        oPreviousInsXmlElement = New XMLElementToAdd
        oPreviousInsXmlElement.ElementName = "PREVIOUSINSURANCE"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "POLICY_NUMBER"
        oXMLAttr(0).AttributeValue = txtPolicyNumber.Text

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "POLICY_TYPE"
        oXMLAttr(1).AttributeValue = ddlPloicyType.SelectedValue

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "POLICY_EXPIRY_DATE"
        oXMLAttr(2).AttributeValue = txtPolicyExpiryDate.Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "PREVIOUS_YEAR_NCB"
        oXMLAttr(3).AttributeValue = DDLPreviousYearNCB.SelectedValue

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "DOCUMENT_PROOF"
        oXMLAttr(4).AttributeValue = txtDocProof.Text

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "PREVIOUS_INSURER_NAME"
        oXMLAttr(5).AttributeValue = txtInsuredName.Text

        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "INSURER_ADD1"
        oXMLAttr(6).AttributeValue = txtLine1.Text

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "INSURER_ADD2"
        oXMLAttr(7).AttributeValue = txtLine2.Text

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "INSURER_CITY"
        oXMLAttr(8).AttributeValue = txtCity.Text

        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "INSURER_STATE"
        oXMLAttr(9).AttributeValue = txtState.Text

        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "INSURER_PINCODE"
        oXMLAttr(10).AttributeValue = txtPinCode.Text

        oXMLAttr(11) = New XMLAttributeToAdd
        oXMLAttr(11).AttributeName = "POLICY_EXPIRY_DATE_NCB"
        oXMLAttr(11).AttributeValue = FormatYYMMDD(txtPreviousPolicyExpiryDate.Text)

        oXMLAttr(12) = New XMLAttributeToAdd
        oXMLAttr(12).AttributeName = "VEHICLE_INSPECTIONDATE"
        oXMLAttr(12).AttributeValue = FormatYYMMDD(txtInspectionDate.Text)

        oXMLAttr(13) = New XMLAttributeToAdd
        oXMLAttr(13).AttributeName = "VEHICLE_INSPECTEDBY"
        oXMLAttr(13).AttributeValue = txtInspectedByWhom.Text

        oXMLAttr(14) = New XMLAttributeToAdd
        oXMLAttr(14).AttributeName = "VEHICLE_INSPECTIONREPORT"
        oXMLAttr(14).AttributeValue = txtInspectionReport.Text

        oXMLAttr(15) = New XMLAttributeToAdd
        oXMLAttr(15).AttributeName = "PREVIOUSINSURANCEHISTORYINC"
        oXMLAttr(15).AttributeValue = Convert.ToInt32(chkIsPreviousInsuranceHistory.Checked)

        oXMLAttr(16) = New XMLAttributeToAdd
        oXMLAttr(16).AttributeName = "VEHICLEINSPECTIONINC"
        oXMLAttr(16).AttributeValue = Convert.ToInt32(chkVehicleInspection.Checked)

        'oXMLAttr(17) = New XMLAttributeToAdd
        'oXMLAttr(17).AttributeName = "NO_CLAIM_BONUS"
        'oXMLAttr(17).AttributeValue = ddlNoClaims.SelectedValue


        oPreviousInsXmlElement.Attributes = oXMLAttr

    End Sub

    Public Sub PreviousInsuranceHistory(ByRef oPreviousInsHistoryXmlElement As XMLElementToAdd, ByVal iCount As Integer)
        Dim dt As DataTable = Session("PersonalInsuranceHistory")
        Dim oXMLAttr(4) As XMLAttributeToAdd

        oPreviousInsHistoryXmlElement = New XMLElementToAdd
        oPreviousInsHistoryXmlElement.ElementName = "PIHISTORY"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "PI_POLICYNUMBER"
        oXMLAttr(0).AttributeValue = dt.Rows(iCount).Item("PI_POLICYNUMBER")
        'oXMLAttr(0).AttributeValue = gvPreviousInsurancehistory.Rows(iCount).Cells(2).Text


        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "PI_NUMBEROFCLAIMS"
        oXMLAttr(1).AttributeValue = dt.Rows(iCount).Item("PI_NUMBEROFCLAIMS")
        'oXMLAttr(1).AttributeValue = gvPreviousInsurancehistory.Rows(iCount).Cells(3).Text



        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "PI_YEAR"
        oXMLAttr(2).AttributeValue = dt.Rows(iCount).Item("PI_YEAR")
        'oXMLAttr(2).AttributeValue = gvPreviousInsurancehistory.Rows(iCount).Cells(4).Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "PI_TOTALAMOUNTOFCLAIMS"
        oXMLAttr(3).AttributeValue = dt.Rows(iCount).Item("PI_TOTALAMOUNTOFCLAIMS")
        'oXMLAttr(3).AttributeValue = gvPreviousInsurancehistory.Rows(iCount).Cells(5).Text

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "PI_INCURREDLOSSRATIO"
        oXMLAttr(4).AttributeValue = dt.Rows(iCount).Item("PI_INCURREDLOSSRATIO")
        'oXMLAttr(4).AttributeValue = gvPreviousInsurancehistory.Rows(iCount).Cells(6).Text

        oPreviousInsHistoryXmlElement.Attributes = oXMLAttr

    End Sub


    Public Sub DriverDetails(ByRef oDriverDetailsXmlElement As XMLElementToAdd)
        Dim oXMLAttr(14) As XMLAttributeToAdd

        oDriverDetailsXmlElement = New XMLElementToAdd
        oDriverDetailsXmlElement.ElementName = "DRIVERDETAILS"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "PAID_DRIVER"
        oXMLAttr(0).AttributeValue = gvDriverDetails.Rows(0).Cells(2).Text

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "VEHICLE_DRIVEN_BY"
        oXMLAttr(1).AttributeValue = gvDriverDetails.Rows(0).Cells(3).Text


        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "MORE_THAN_ONE_DRIVER"
        oXMLAttr(2).AttributeValue = gvDriverDetails.Rows(0).Cells(4).Text


        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "DRIVER_NAME"
        oXMLAttr(3).AttributeValue = gvDriverDetails.Rows(0).Cells(5).Text


        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "AGE"
        oXMLAttr(4).AttributeValue = gvDriverDetails.Rows(0).Cells(6).Text


        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "GENDER"
        oXMLAttr(5).AttributeValue = gvDriverDetails.Rows(0).Cells(7).Text


        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "RELATIONSHIP_WITH_OWNER"
        oXMLAttr(6).AttributeValue = gvDriverDetails.Rows(0).Cells(8).Text

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "DRIVING_LICENCE_NUMBER"
        oXMLAttr(7).AttributeValue = gvDriverDetails.Rows(0).Cells(9).Text



        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "DATE_OF_FIRST_ISSUE"
        oXMLAttr(8).AttributeValue = gvDriverDetails.Rows(0).Cells(10).Text


        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "ENDORSEMENTS"
        oXMLAttr(9).AttributeValue = gvDriverDetails.Rows(0).Cells(11).Text


        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "RENEWAL_DATE"
        oXMLAttr(10).AttributeValue = gvDriverDetails.Rows(0).Cells(12).Text


        oXMLAttr(11) = New XMLAttributeToAdd
        oXMLAttr(11).AttributeName = "TYPE_OF_LICENCE"
        oXMLAttr(11).AttributeValue = gvDriverDetails.Rows(0).Cells(13).Text

        oXMLAttr(12) = New XMLAttributeToAdd
        oXMLAttr(12).AttributeName = "DEFENSIVE_ADVANCED_DRIVING"
        oXMLAttr(12).AttributeValue = gvDriverDetails.Rows(0).Cells(14).Text

        oXMLAttr(13) = New XMLAttributeToAdd
        oXMLAttr(13).AttributeName = "LICENSE_ISSUING_AUTHORITY"
        oXMLAttr(13).AttributeValue = gvDriverDetails.Rows(0).Cells(15).Text



        oXMLAttr(14) = New XMLAttributeToAdd
        oXMLAttr(14).AttributeName = "DRIVER_EDUCATION_QUALIFICATION"
        oXMLAttr(14).AttributeValue = gvDriverDetails.Rows(0).Cells(16).Text


        oXMLAttr(15) = New XMLAttributeToAdd
        oXMLAttr(15).AttributeName = "DRIVERS_EXPERIENCE"
        oXMLAttr(15).AttributeValue = gvDriverDetails.Rows(0).Cells(17).Text


        oDriverDetailsXmlElement.Attributes = oXMLAttr


    End Sub

    Public Sub DriverDetails(ByRef oDriverDetailsXmlElement As XMLElementToAdd, ByVal iCount As Integer)
        Dim dt As DataTable = Session("DriverDetails")
        Dim oXMLAttr(11) As XMLAttributeToAdd
        oDriverDetailsXmlElement = New XMLElementToAdd
        oDriverDetailsXmlElement.ElementName = "DRIVERDETAILS"

        

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "VEHICLE_DRIVEN_BY"
        oXMLAttr(0).AttributeValue = ddlvehicledrivenby.Items.FindByText(dt.Rows(iCount).Item("VEHICLE_DRIVEN_BY").ToString()).Value
        'oXMLAttr(1).AttributeValue = ddlvehicledrivenby.Items.FindByText(gvDriverDetails.Rows(iCount).Cells(3).Text).Value



        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "GENDER"
        'oXMLAttr(5).AttributeValue = ddlGender.Items.FindByText(gvDriverDetails.Rows(iCount).Cells(7).Text).Value
        oXMLAttr(1).AttributeValue = ddlGender.Items.FindByText(dt.Rows(iCount).Item("GENDER").ToString()).Value


        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "RELATIONSHIP_WITH_OWNER"
        'oXMLAttr(6).AttributeValue = ddlRelationshipwithowner.Items.FindByText(gvDriverDetails.Rows(iCount).Cells(8).Text).Value
        oXMLAttr(2).AttributeValue = ddlRelationshipwithowner.Items.FindByText(dt.Rows(iCount).Item("RELATIONSHIP_WITH_OWNER").ToString()).Value

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "DRIVING_LICENCE_NUMBER"
        oXMLAttr(3).AttributeValue = dt.Rows(iCount).Item("DRIVING_LICENCE_NUMBER")
        'oXMLAttr(7).AttributeValue = gvDriverDetails.Rows(iCount).Cells(9).Text



        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "DATE_OF_FIRST_ISSUE"
        oXMLAttr(4).AttributeValue = dt.Rows(iCount).Item("DATE_OF_FIRST_ISSUE")
        'oXMLAttr(8).AttributeValue = gvDriverDetails.Rows(iCount).Cells(10).Text


        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "ENDORSEMENTS"
        oXMLAttr(5).AttributeValue = dt.Rows(iCount).Item("ENDORSEMENTS")
        'oXMLAttr(9).AttributeValue = gvDriverDetails.Rows(iCount).Cells(11).Text


        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "RENEWAL_DATE"
        oXMLAttr(6).AttributeValue = dt.Rows(iCount).Item("RENEWAL_DATE")
        'oXMLAttr(10).AttributeValue = gvDriverDetails.Rows(iCount).Cells(12).Text


        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "TYPE_OF_LICENCE"
        oXMLAttr(7).AttributeValue = ddlTypeoflicence.Items.FindByText(dt.Rows(iCount).Item("TYPE_OF_LICENCE").ToString()).Value
        'oXMLAttr(11).AttributeValue = ddlTypeoflicence.Items.FindByText(gvDriverDetails.Rows(iCount).Cells(13).Text).Value

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "DEFENSIVE_ADVANCED_DRIVING"
        oXMLAttr(8).AttributeValue = dt.Rows(iCount).Item("DEFENSIVE_ADVANCED_DRIVING")
        'oXMLAttr(12).AttributeValue = gvDriverDetails.Rows(iCount).Cells(14).Text

        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "LICENSE_ISSUING_AUTHORITY"
        oXMLAttr(9).AttributeValue = dt.Rows(iCount).Item("LICENSE_ISSUING_AUTHORITY")
        'oXMLAttr(13).AttributeValue = gvDriverDetails.Rows(iCount).Cells(15).Text



        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "DRIVER_EDUCATION_QUALIFICATION"
        oXMLAttr(10).AttributeValue = dt.Rows(iCount).Item("DRIVER_EDUCATION_QUALIFICATION")
        'oXMLAttr(14).AttributeValue = gvDriverDetails.Rows(iCount).Cells(16).Text


        oXMLAttr(11) = New XMLAttributeToAdd
        oXMLAttr(11).AttributeName = "DRIVERS_EXPERIENCE"
        oXMLAttr(11).AttributeValue = dt.Rows(iCount).Item("DRIVERS_EXPERIENCE")
        'oXMLAttr(15).AttributeValue = gvDriverDetails.Rows(iCount).Cells(17).Text

        If ddlvehicledrivenby.SelectedItem.Text = "Paid Driver" Then
            ReDim Preserve oXMLAttr(15)

            oXMLAttr(12) = New XMLAttributeToAdd
            oXMLAttr(12).AttributeName = "PAID_DRIVER"
            'oXMLAttr(0).AttributeValue = gvDriverDetails.Rows(iCount).Cells(2).Text
            oXMLAttr(12).AttributeValue = dt.Rows(iCount).Item("PAID_DRIVER")

            oXMLAttr(13) = New XMLAttributeToAdd
            oXMLAttr(13).AttributeName = "MORE_THAN_ONE_DRIVER"
            oXMLAttr(13).AttributeValue = dt.Rows(iCount).Item("MORE_THAN_ONE_DRIVER")
            'oXMLAttr(2).AttributeValue = gvDriverDetails.Rows(iCount).Cells(4).Text

            oXMLAttr(14) = New XMLAttributeToAdd
            oXMLAttr(14).AttributeName = "DRIVER_NAME"
            oXMLAttr(14).AttributeValue = dt.Rows(iCount).Item("DRIVER_NAME")
            'oXMLAttr(3).AttributeValue = gvDriverDetails.Rows(iCount).Cells(5).Text

            oXMLAttr(15) = New XMLAttributeToAdd
            oXMLAttr(15).AttributeName = "AGE"
            oXMLAttr(15).AttributeValue = dt.Rows(iCount).Item("AGE")
            'oXMLAttr(4).AttributeValue = gvDriverDetails.Rows(iCount).Cells(6).Text

           
        End If
        oDriverDetailsXmlElement.Attributes = oXMLAttr

    End Sub


    Public Sub VehicleUsage(ByRef oVehicleInsXmlElement As XMLElementToAdd)
        Dim oXMLAttr(3) As XMLAttributeToAdd

        oVehicleInsXmlElement = New XMLElementToAdd
        oVehicleInsXmlElement.ElementName = "VEHICLEUSAGEDETAILS"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "VEHICLE_PARKING"
        oXMLAttr(0).AttributeValue = ddlVechicleParked.SelectedValue

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "MAIN_USAGE_OF_VEHICLE"
        oXMLAttr(1).AttributeValue = ddlVechiclemainlyused.SelectedValue

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "DISTANCE_DRIVEN_ANNUALLY"
        oXMLAttr(2).AttributeValue = txtApproximatedistance.Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "TYPE_OF_ROADS"
        oXMLAttr(3).AttributeValue = ddlusuallydriven.SelectedValue

        oVehicleInsXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub AdditionalCoverDetails(ByRef oAdditionalCoverDetailXmlElement As XMLElementToAdd)
        Dim oXMLAttr(36) As XMLAttributeToAdd

        oAdditionalCoverDetailXmlElement = New XMLElementToAdd
        oAdditionalCoverDetailXmlElement.ElementName = "ADDITIONALCOVER"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "BI_FUEL_KIT_INBUILT"
        oXMLAttr(0).AttributeValue = Convert.ToInt32(chkBifuel.Checked)

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "LPG_CNG"
        oXMLAttr(1).AttributeValue = Convert.ToInt32(chkLPGkit.Checked)



        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "NON_CONVENTIONAL_SOF"
        oXMLAttr(2).AttributeValue = Convert.ToInt32(chkNonConvectionalPower.Checked)

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "DESC_NON_CONVENTIONAL_SOF"
        oXMLAttr(3).AttributeValue = txtDescription.Text

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "ANTITHEFT_DEVICE"
        oXMLAttr(4).AttributeValue = Convert.ToInt32(chkAntitheft.Checked)

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "HANDICAPPED_USER"
        oXMLAttr(5).AttributeValue = Convert.ToInt32(chkIshandicapped.Checked)

        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "FOREIGN_EMBASSY"
        oXMLAttr(6).AttributeValue = Convert.ToInt32(chkForiegnEmbassy.Checked)

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "FIBRE_FUEL_TANK"
        oXMLAttr(7).AttributeValue = Convert.ToInt32(chkFiberGlassFuelTank.Checked)

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "DRIVING_TUITIONS"
        oXMLAttr(8).AttributeValue = Convert.ToInt32(chkIsvehicleusedfordrivingtutions.Checked)

        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "LIMITED_TO_OWN_PREMISES"
        oXMLAttr(9).AttributeValue = Convert.ToInt32(chkOwnprmesis.Checked)

        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "VINTAGE_CAR"
        oXMLAttr(10).AttributeValue = Convert.ToInt32(chkcertifiedVintage.Checked)

        oXMLAttr(11) = New XMLAttributeToAdd
        oXMLAttr(11).AttributeName = "EXT_OF_GEO_AREA"
        oXMLAttr(11).AttributeValue = Convert.ToInt32(chkGeographical.Checked)

        oXMLAttr(12) = New XMLAttributeToAdd
        oXMLAttr(12).AttributeName = "BANGLADESH"
        oXMLAttr(12).AttributeValue = Convert.ToInt32(chkBangaldesh.Checked)

        oXMLAttr(13) = New XMLAttributeToAdd
        oXMLAttr(13).AttributeName = "BHUTAN"
        oXMLAttr(13).AttributeValue = Convert.ToInt32(chkBhutan.Checked)

        oXMLAttr(14) = New XMLAttributeToAdd
        oXMLAttr(14).AttributeName = "MALDIVES"
        oXMLAttr(14).AttributeValue = Convert.ToInt32(chkMaldives.Checked)

        oXMLAttr(15) = New XMLAttributeToAdd
        oXMLAttr(15).AttributeName = "NEPAL"
        oXMLAttr(15).AttributeValue = Convert.ToInt32(chkNepal.Checked)

        oXMLAttr(16) = New XMLAttributeToAdd
        oXMLAttr(16).AttributeName = "PAKISTAN"
        oXMLAttr(16).AttributeValue = Convert.ToInt32(chkPakistan.Checked)

        oXMLAttr(17) = New XMLAttributeToAdd
        oXMLAttr(17).AttributeName = "SRILANKA"
        oXMLAttr(17).AttributeValue = Convert.ToInt32(chkSrilanka.Checked)

        oXMLAttr(18) = New XMLAttributeToAdd
        oXMLAttr(18).AttributeName = "RALLIES"
        oXMLAttr(18).AttributeValue = Convert.ToInt32(chkVechileusedforrallies.Checked)

        oXMLAttr(19) = New XMLAttributeToAdd
        oXMLAttr(19).AttributeName = "START_DATE"
        oXMLAttr(19).AttributeValue = FormatYYMMDD(txtStartDate.Text)

        oXMLAttr(20) = New XMLAttributeToAdd
        oXMLAttr(20).AttributeName = "END_DATE"
        oXMLAttr(20).AttributeValue = FormatYYMMDD(txtEndDate.Text)

        oXMLAttr(21) = New XMLAttributeToAdd
        oXMLAttr(21).AttributeName = "RELIABILITY_TRIALS"
        oXMLAttr(21).AttributeValue = Convert.ToInt32(chkBrokerageAgencyCommission.Checked)

        oXMLAttr(22) = New XMLAttributeToAdd
        oXMLAttr(22).AttributeName = "VOLUNTARY_EXCESS"
        oXMLAttr(22).AttributeValue = Convert.ToInt32(chkVoluntaryDeductible.Checked)

        oXMLAttr(23) = New XMLAttributeToAdd
        oXMLAttr(23).AttributeName = "VOLUNTARY_EXCESS_AMOUNT"
        oXMLAttr(23).AttributeValue = ddlVoluntaryExcessamount.SelectedValue

        oXMLAttr(24) = New XMLAttributeToAdd
        oXMLAttr(24).AttributeName = "IMPOSED_EXCESS"
        oXMLAttr(24).AttributeValue = txtImposedExcess.Text

        oXMLAttr(25) = New XMLAttributeToAdd
        oXMLAttr(25).AttributeName = "COMPULSORY_EXCESS"
        oXMLAttr(25).AttributeValue = txtCompulsoryExcess.Text

        oXMLAttr(26) = New XMLAttributeToAdd
        oXMLAttr(26).AttributeName = "CNG_KIT"
        oXMLAttr(26).AttributeValue = Convert.ToInt32(chkCNGSI.Checked)

        oXMLAttr(27) = New XMLAttributeToAdd
        oXMLAttr(27).AttributeName = "CNG_SUM_INSURED"
        oXMLAttr(27).AttributeValue = txtCNGSI.Text

        oXMLAttr(28) = New XMLAttributeToAdd
        oXMLAttr(28).AttributeName = "AGENCY_COMMISSIONIND"
        oXMLAttr(28).AttributeValue = Convert.ToInt32(chkBrokerageAgencyCommission.Checked)

        oXMLAttr(29) = New XMLAttributeToAdd
        oXMLAttr(29).AttributeName = "AGENCY_COMMISSION"
        oXMLAttr(29).AttributeValue = txtDiscountAgentCommission.Text

        oXMLAttr(30) = New XMLAttributeToAdd
        oXMLAttr(30).AttributeName = "GOOD_FEATURE_DISCOUNTIND"
        oXMLAttr(30).AttributeValue = Convert.ToInt32(chkGoodfeatureDiscount.Checked)

        oXMLAttr(31) = New XMLAttributeToAdd
        oXMLAttr(31).AttributeName = "GOOD_FEATURE_DISCOUNT"
        oXMLAttr(31).AttributeValue = txtDiscountGoodfeature.Text

        oXMLAttr(32) = New XMLAttributeToAdd
        oXMLAttr(32).AttributeName = "SPECIAL_DISCOUNTIND"
        oXMLAttr(32).AttributeValue = Convert.ToInt32(chkSpecialDiscount.Checked)

        oXMLAttr(33) = New XMLAttributeToAdd
        oXMLAttr(33).AttributeName = "SPECIAL_DISCOUNT"
        oXMLAttr(33).AttributeValue = txtDiscountSpecial.Text

        oXMLAttr(34) = New XMLAttributeToAdd
        oXMLAttr(34).AttributeName = "RTA_PERMISSION_FOR_NCSOP"
        oXMLAttr(34).AttributeValue = Convert.ToInt32(chkPermissionRTA.Checked)

        oXMLAttr(35) = New XMLAttributeToAdd
        oXMLAttr(35).AttributeName = "DETARRIFF_DISCOUNT"
        oXMLAttr(35).AttributeValue = ddlDetarriffDiscount.SelectedValue

        oXMLAttr(36) = New XMLAttributeToAdd
        oXMLAttr(36).AttributeName = "LPG_CNG_SUM_INSURED"
        oXMLAttr(36).AttributeValue = txtLPGSI.Text

        oAdditionalCoverDetailXmlElement.Attributes = oXMLAttr

    End Sub

    Public Sub LegalLiability1(ByRef oLegalLiabilityXmlElement As XMLElementToAdd)
        Dim oXMLAttr(2) As XMLAttributeToAdd

        oLegalLiabilityXmlElement = New XMLElementToAdd
        oLegalLiabilityXmlElement.ElementName = "LEGALLIABILITY"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "WORKMEN_FOR_OPS_MAINTENANCE"
        oXMLAttr(0).AttributeValue = txtNumberOfPaiddrivesCleanersWorkMan.Text

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "NO_OF_EMP_TRAVELLING_DRIVING"
        oXMLAttr(1).AttributeValue = txtNumberOFEmployeeTravellingDriving.Text

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "RESTRICTED_TPPD"
        oXMLAttr(2).AttributeValue = Convert.ToInt32(chkDoyouwishtorestrictTPPDCovertostatutoryLimitofRs6000.Checked)

        oLegalLiabilityXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub PersonalAccident(ByRef oPersonalAccidentXmlElement As XMLElementToAdd)
        Dim oXMLAttr(10) As XMLAttributeToAdd

        oPersonalAccidentXmlElement = New XMLElementToAdd
        oPersonalAccidentXmlElement.ElementName = "PERSONALACCIDENT"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "PA_UN_NAMED"
        oXMLAttr(0).AttributeValue = Convert.ToInt32(chkPersonalaccidentcoverforunnamedpersons.Checked)

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "PA_UN_NAMED_NUMBER"
        oXMLAttr(1).AttributeValue = txtNumberOfunnamedPersons.Text

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "PA_UN_NAMED_SUMINSURED"
        oXMLAttr(2).AttributeValue = txtSumInsuredPerPerson.Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "PA_PAID_DRIVER_CLEANER"
        oXMLAttr(3).AttributeValue = Convert.ToInt32(chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner.Checked)

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "PA_NUMBER"
        oXMLAttr(4).AttributeValue = txtNumberOfpaiddrivers.Text

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "PA_SUM_INSURED"
        oXMLAttr(5).AttributeValue = txtSumInsured2.Text

        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "PA_NAMED_PERSON"
        oXMLAttr(6).AttributeValue = Convert.ToInt32(chkDoyouwishtotakepersonalaccidentcoverfornamedpersons.Checked)

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "PA_TOTAL_SI"
        oXMLAttr(7).AttributeValue = txtTotalsuminsuredofallnamedpersons.Text

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "IS_COMP_PA"
        oXMLAttr(8).AttributeValue = Convert.ToInt32(chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements.Checked)

        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "COMP_PA_SI"
        oXMLAttr(9).AttributeValue = txtSumInsured1.Text

        oXMLAttr(10) = New XMLAttributeToAdd
        oXMLAttr(10).AttributeName = "IS_ONLY_CAR"
        oXMLAttr(10).AttributeValue = Convert.ToInt32(chkThisvechcle.Checked)



        oPersonalAccidentXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub PersonalAccidentDetails(ByRef oPersonalAccidentDetailsXmlElement As XMLElementToAdd, ByVal iCount As Integer)
        Dim oXMLAttr(8) As XMLAttributeToAdd
        Dim dt As DataTable = Session("PersonalAccident")
        oPersonalAccidentDetailsXmlElement = New XMLElementToAdd
        oPersonalAccidentDetailsXmlElement.ElementName = "PADETAILS"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "PA_NAME"
        oXMLAttr(0).AttributeValue = dt.Rows(iCount).Item("PA_NAME")
        'oXMLAttr(0).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(2).Text

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "PA_SUM_INSURED"
        oXMLAttr(1).AttributeValue = dt.Rows(iCount).Item("PA_SUM_INSURED")
        'oXMLAttr(1).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(3).Text

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "PA_SERIAL_NUMBER"
        oXMLAttr(2).AttributeValue = dt.Rows(iCount).Item("PA_SERIAL_NUMBER")
        'oXMLAttr(2).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(4).Text



        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "PA_NOMINEE_DETAILS"
        oXMLAttr(3).AttributeValue = dt.Rows(iCount).Item("PA_NOMINEE_DETAILS")
        'oXMLAttr(3).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(5).Text

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "NOMINEE_ADD1"
        oXMLAttr(4).AttributeValue = dt.Rows(iCount).Item("NOMINEE_ADD1")
        'oXMLAttr(4).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(6).Text

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "NOMINEE_ADD2"
        oXMLAttr(5).AttributeValue = dt.Rows(iCount).Item("NOMINEE_ADD2")
        'oXMLAttr(5).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(7).Text

        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "NOMINEE_CITY"
        oXMLAttr(6).AttributeValue = dt.Rows(iCount).Item("NOMINEE_CITY")
        'oXMLAttr(6).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(8).Text

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "NOMINEE_STATE"
        oXMLAttr(7).AttributeValue = dt.Rows(iCount).Item("NOMINEE_STATE")
        'oXMLAttr(7).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(9).Text

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "NOMINEE_PINCODE"
        oXMLAttr(8).AttributeValue = dt.Rows(iCount).Item("NOMINEE_PINCODE")
        'oXMLAttr(8).AttributeValue = gvpersonalAccidentDetails.Rows(iCount).Cells(10).Text

        oPersonalAccidentDetailsXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub ClaimHistory(ByRef oClaimHistoryXmlElement As XMLElementToAdd)
        Dim oXMLAttr(5) As XMLAttributeToAdd

        oClaimHistoryXmlElement = New XMLElementToAdd
        oClaimHistoryXmlElement.ElementName = "CLAIMSHISTORY"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "CLAIM_NUMBER"
        oXMLAttr(0).AttributeValue = ""

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "CLAIM_STATUS"
        oXMLAttr(1).AttributeValue = ""

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "POLICY_NUMBER"
        oXMLAttr(2).AttributeValue = ""

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "LOSS_TIME"
        oXMLAttr(3).AttributeValue = ""

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "LOSS_DATE"
        oXMLAttr(4).AttributeValue = ""

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "CAUSE"
        oXMLAttr(5).AttributeValue = ""

        oClaimHistoryXmlElement.Attributes = oXMLAttr
    End Sub
    Public Sub Financier(ByRef oFinancierXmlElement As XMLElementToAdd)
        Dim oXMLAttr(0) As XMLAttributeToAdd

        oFinancierXmlElement = New XMLElementToAdd
        oFinancierXmlElement.ElementName = "FINANCIER"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "FINANCIERDETAILSIND"
        oXMLAttr(0).AttributeValue = Convert.ToInt32(chkFinancierDetails.Checked)

        oFinancierXmlElement.Attributes = oXMLAttr
    End Sub
    Public Sub FinancierDetails(ByRef oFinancierDetailXmlElement As XMLElementToAdd, ByVal iCount As Integer)
        Dim oXMLAttr(7) As XMLAttributeToAdd
        Dim dt As DataTable = Session("FinancierDetails")
        oFinancierDetailXmlElement = New XMLElementToAdd
        oFinancierDetailXmlElement.ElementName = "FINANCIERDETAILS"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "AGREEMENT_TYPE"
        oXMLAttr(0).AttributeValue = ddlagreementType.Items.FindByText(dt.Rows(iCount).Item("AGREEMENT_TYPE").ToString()).Value
        'oXMLAttr(0).AttributeValue = ddlagreementType.Items.FindByText(gvFinancierDetails.Rows(iCount).Cells(2).Text).Value

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "FINANCIER_TYPE"
        oXMLAttr(1).AttributeValue = ddlFinancerType.Items.FindByText(dt.Rows(iCount).Item("FINANCIER_TYPE").ToString()).Value
        'oXMLAttr(1).AttributeValue = ddlFinancerType.Items.FindByText(gvFinancierDetails.Rows(iCount).Cells(3).Text).Value

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "NAME_OF_FINANCIER"
        'oXMLAttr(2).AttributeValue = gvFinancierDetails.Rows(iCount).Cells(4).Text
        oXMLAttr(2).AttributeValue = dt.Rows(iCount).Item("NAME_OF_FINANCIER").ToString()

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "FINANCIER_ADD1"
        'oXMLAttr(3).AttributeValue = gvFinancierDetails.Rows(iCount).Cells(5).Text
        oXMLAttr(3).AttributeValue = dt.Rows(iCount).Item("FINANCIER_ADD1").ToString()

        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "FINANCIER_ADD2"
        'oXMLAttr(4).AttributeValue = gvFinancierDetails.Rows(iCount).Cells(6).Text
        oXMLAttr(4).AttributeValue = dt.Rows(iCount).Item("FINANCIER_ADD2").ToString()

        oXMLAttr(5) = New XMLAttributeToAdd
        oXMLAttr(5).AttributeName = "FINANCIER_CITY"
        'oXMLAttr(5).AttributeValue = gvFinancierDetails.Rows(iCount).Cells(7).Text
        oXMLAttr(5).AttributeValue = dt.Rows(iCount).Item("FINANCIER_CITY").ToString()


        oXMLAttr(6) = New XMLAttributeToAdd
        oXMLAttr(6).AttributeName = "FINANCIER_STATE"
        'oXMLAttr(6).AttributeValue = gvFinancierDetails.Rows(iCount).Cells(9).Text
        oXMLAttr(6).AttributeValue = dt.Rows(iCount).Item("FINANCIER_STATE").ToString()

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "FINANCIER_PINCODE"
        'oXMLAttr(7).AttributeValue = gvFinancierDetails.Rows(iCount).Cells(8).Text
        oXMLAttr(7).AttributeValue = dt.Rows(iCount).Item("FINANCIER_PINCODE").ToString()

        oFinancierDetailXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub ElectricalDetails(ByRef oElectricalDetailXmlElement As XMLElementToAdd, ByVal iCount As Integer)
        Dim oXMLAttr(4) As XMLAttributeToAdd
        Dim dt As DataTable = Session("Electricaldetails")
        oElectricalDetailXmlElement = New XMLElementToAdd
        oElectricalDetailXmlElement.ElementName = "ELECTRICAL"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "ELECTRIC_MAKE"
        oXMLAttr(0).AttributeValue = dt.Rows(iCount).Item("ELECTRIC_MAKE")
        'oXMLAttr(0).AttributeValue = gvElectricalaccessories.Rows(iCount).Cells(2).Text

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "ELECTRIC_SUMINSURED"
        oXMLAttr(1).AttributeValue = dt.Rows(iCount).Item("ELECTRIC_SUMINSURED")
        'oXMLAttr(1).AttributeValue = gvElectricalaccessories.Rows(iCount).Cells(3).Text

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "ELECTRICAL_DESCRIPTION"
        oXMLAttr(2).AttributeValue = dt.Rows(iCount).Item("ELECTRICAL_DESCRIPTION")
        'oXMLAttr(2).AttributeValue = gvElectricalaccessories.Rows(iCount).Cells(4).Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "ELECTRICAL_SERIAL_NUMBER"
        oXMLAttr(3).AttributeValue = dt.Rows(iCount).Item("ELECTRICAL_SERIAL_NUMBER")
        'oXMLAttr(3).AttributeValue = gvElectricalaccessories.Rows(iCount).Cells(5).Text


        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "PRODUCT_SERIAL_NUMBER"
        oXMLAttr(4).AttributeValue = dt.Rows(iCount).Item("PRODUCT_SERIAL_NUMBER")
        'oXMLAttr(4).AttributeValue = gvElectricalaccessories.Rows(iCount).Cells(6).Text

        oElectricalDetailXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub NONElectricalDetails(ByRef oNonElectricalDetailXmlElement As XMLElementToAdd, ByVal iCOunt As Integer)
        Dim dt As DataTable = Session("NonElectricaldetails")
        Dim oXMLAttr(4) As XMLAttributeToAdd

        oNonElectricalDetailXmlElement = New XMLElementToAdd
        oNonElectricalDetailXmlElement.ElementName = "NONELECTRICAL"

        oXMLAttr(0) = New XMLAttributeToAdd
        oXMLAttr(0).AttributeName = "NON_ELECTRICAL_MAKE"
        oXMLAttr(0).AttributeValue = dt.Rows(iCOunt).Item("NON_ELECTRICAL_MAKE")
        'oXMLAttr(0).AttributeValue = gvNonElecTrical.Rows(iCOunt).Cells(2).Text

        oXMLAttr(1) = New XMLAttributeToAdd
        oXMLAttr(1).AttributeName = "NON_ELECTRIC_SUMINSURED"
        oXMLAttr(1).AttributeValue = dt.Rows(iCOunt).Item("NON_ELECTRIC_SUMINSURED")
        'oXMLAttr(1).AttributeValue = gvNonElecTrical.Rows(iCOunt).Cells(3).Text

        oXMLAttr(2) = New XMLAttributeToAdd
        oXMLAttr(2).AttributeName = "NON_ELECTRICAL_DESCRIPTION"
        oXMLAttr(2).AttributeValue = dt.Rows(iCOunt).Item("NON_ELECTRICAL_DESCRIPTION")
        'oXMLAttr(2).AttributeValue = gvNonElecTrical.Rows(iCOunt).Cells(4).Text

        oXMLAttr(3) = New XMLAttributeToAdd
        oXMLAttr(3).AttributeName = "NON_ELECTRICAL_SERIAL_NUMBER"
        oXMLAttr(3).AttributeValue = dt.Rows(iCOunt).Item("NON_ELECTRICAL_SERIAL_NUMBER")
        'oXMLAttr(3).AttributeValue = gvNonElecTrical.Rows(iCOunt).Cells(5).Text


        oXMLAttr(4) = New XMLAttributeToAdd
        oXMLAttr(4).AttributeName = "NON_PRODUCT_SERIAL_NUMBER"
        oXMLAttr(4).AttributeValue = dt.Rows(iCOunt).Item("NON_PRODUCT_SERIAL_NUMBER")
        'oXMLAttr(4).AttributeValue = gvNonElecTrical.Rows(iCOunt).Cells(6).Text

        oNonElectricalDetailXmlElement.Attributes = oXMLAttr
    End Sub

    Public Sub VehicleMake()
        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim strconn As String = ConfigurationManager.AppSettings("SiriusConn").ToString()
        conn.ConnectionString = strconn
        conn.Open()
        cmd.Connection = conn
        cmd.CommandText = "Select description,BodyType,CC,SeatCap,Make from UDL_VEHMAKEMAST where is_deleted = 0  and Make like '" & ddlVehicleMake.SelectedItem.Text & "'  and effective_date <=   '" & Today & "' " '{ts'2008-08-26 16:38:32'}"
        Dim dr As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
        Dim dt As New DataTable
        dt.Load(dr)
        gvVehicleMake.DataSource = dt
        gvVehicleMake.DataBind()

    End Sub

    Public Sub VehicleRegistrationAddress()
        Dim conn As New System.Data.SqlClient.SqlConnection
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim strconn As String = ConfigurationManager.AppSettings("SiriusConn").ToString() '"Data Source=localhost;Initial Catalog=Sirius_UIIC;User ID=sa"
        conn.ConnectionString = strconn
        conn.Open()
        cmd.Connection = conn
        cmd.CommandText = "Select description,State,Zone from UDL_RTO_MSTR where is_deleted = 0 and effective_date <=  '" & Today & "'"
        Dim dr As System.Data.SqlClient.SqlDataReader = cmd.ExecuteReader()
        Dim dt As New DataTable
        dt.Load(dr)
        gvVehicleAddress.DataSource = dt
        gvVehicleAddress.DataBind()
    End Sub

#End Region

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
            WriteToLog(Session, "MotorAddRisk.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
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
            WriteToLog(Session, "MotorAddRisk.aspx", "SAMForInsuranceV2", "GetList",StartDate, Date.Now)
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

    Protected Sub btnaddDriver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddDriver.Click

        If chkincludeDriverDetails.Checked = True Then
            pnlDriverDetails.Visible = True
            Session("DRIVERDETAILEDIT") = 0
        Else
            pnlDriverDetails.Visible = False
        End If
        ClearText(pnlDriverDetails)
    End Sub

    Protected Sub btnAddPreviousInsurancehistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddPreviousInsurancehistory.Click
        If chkIsPreviousInsuranceHistory.Checked = True Then
            pnlPIHistory.Visible = True
            Session("PIHISTORYEDIT") = 0
        Else
            pnlPIHistory.Visible = False
        End If
        ClearText(pnlPIHistory)
    End Sub


    Protected Sub btnAddpersonalAccidentDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddpersonalAccidentDetails.Click
        If chkDoyouwishtotakepersonalaccidentcoverfornamedpersons.Checked = True Then
            pnlPersonalaccidentDetails.Visible = True
            Session("PADETAILSEDIT") = 0
        Else
            pnlPersonalaccidentDetails.Visible = False
        End If
        ClearText(pnlPersonalaccidentDetails)
    End Sub

    Protected Sub btnPIHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPIHistory.Click

        Dim dt As New DataTable

        Dim dtExist As New DataTable

        dtExist = Session("PersonalInsuranceHistory")
        If Session("PIHISTORYEDIT") = 1 Then
            Dim iCount = gvPreviousInsurancehistory.SelectedRow.RowIndex
            dtExist.Rows(iCount).Item("PI_POLICYNUMBER") = txtPIPolicyNumber.Text
            dtExist.Rows(iCount).Item("PI_NUMBEROFCLAIMS") = txtNumberofClaims.Text
            dtExist.Rows(iCount).Item("PI_YEAR") = txtYear.Text
            dtExist.Rows(iCount).Item("PI_TOTALAMOUNTOFCLAIMS") = txtTotalamountofclaims.Text
            dtExist.Rows(iCount).Item("PI_INCURREDLOSSRATIO") = txtIncurredLossRatio.Text
            Session("PersonalInsuranceHistory") = dtExist
            gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
            gvPreviousInsurancehistory.DataBind()
        Else
            dt.Columns.Add(New DataColumn("PI_POLICYNUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PI_NUMBEROFCLAIMS", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PI_YEAR", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PI_TOTALAMOUNTOFCLAIMS", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PI_INCURREDLOSSRATIO", System.Type.GetType("System.String")))


            Dim dr As DataRow
            dr = dt.NewRow()
            dr.Item("PI_POLICYNUMBER") = txtPIPolicyNumber.Text
            dr.Item("PI_NUMBEROFCLAIMS") = txtNumberofClaims.Text
            dr.Item("PI_YEAR") = txtYear.Text
            dr.Item("PI_TOTALAMOUNTOFCLAIMS") = txtTotalamountofclaims.Text
            dr.Item("PI_INCURREDLOSSRATIO") = txtIncurredLossRatio.Text
            dt.Rows.Add(dr)
            If Session("PersonalInsuranceHistory") IsNot Nothing Then
                dtExist = Session("PersonalInsuranceHistory")
                dt.Merge(dtExist)
                Session("PersonalInsuranceHistory") = dt
                gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
                gvPreviousInsurancehistory.DataBind()
            Else

                Session("PersonalInsuranceHistory") = dt
                gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
                gvPreviousInsurancehistory.DataBind()
            End If


        End If

        pnlPIHistory.Visible = False
    End Sub

    Protected Sub chkIsPreviousInsuranceHistory_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkIsPreviousInsuranceHistory.CheckedChanged
        'If chkIsPreviousInsuranceHistory.Checked = True Then
        '    pnlPIHistory.Visible = True
        'Else
        '    pnlPIHistory.Visible = False
        'End If
    End Sub


    Protected Sub btnElectriaclAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnElectriaclAdd.Click
        If chkElectrical.Checked = True Then
            pnlElectrical.Visible = True
            Session("ELECTRICALEDIT") = 0
        Else
            pnlElectrical.Visible = False
        End If
        ClearText(pnlElectrical)
    End Sub

    Protected Sub btnelectricalOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnelectricalOk.Click
        Dim dt As New DataTable
        Dim dtelectrialmerge As New DataTable
        dtelectrialmerge = Session("Electricaldetails")
        If Session("ELECTRICALEDIT") = 1 Then
            Dim iCount As Integer = gvElectricalaccessories.SelectedRow.RowIndex
            dtelectrialmerge.Rows(iCount).Item("ELECTRICAL_SERIAL_NUMBER") = txtelectricalSerialNumber.Text
            dtelectrialmerge.Rows(iCount).Item("PRODUCT_SERIAL_NUMBER") = txtelecricalProductserialNumber.Text
            dtelectrialmerge.Rows(iCount).Item("ELECTRIC_MAKE") = txtelecricalmake.Text
            dtelectrialmerge.Rows(iCount).Item("ELECTRIC_SUMINSURED") = txtelecricalSumInsured.Text
            dtelectrialmerge.Rows(iCount).Item("ELECTRICAL_DESCRIPTION") = txtelecricalDescription.Text
            Session("Electricaldetails") = dtelectrialmerge
            gvElectricalaccessories.DataSource = Session("Electricaldetails")
            gvElectricalaccessories.DataBind()

        Else
            dt.Columns.Add(New DataColumn("ELECTRICAL_SERIAL_NUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PRODUCT_SERIAL_NUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("ELECTRIC_MAKE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("ELECTRIC_SUMINSURED", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("ELECTRICAL_DESCRIPTION", System.Type.GetType("System.String")))


            Dim dr As DataRow
            dr = dt.NewRow()
            dr.Item("ELECTRICAL_SERIAL_NUMBER") = txtelectricalSerialNumber.Text
            dr.Item("PRODUCT_SERIAL_NUMBER") = txtelecricalProductserialNumber.Text
            dr.Item("ELECTRIC_MAKE") = txtelecricalmake.Text
            dr.Item("ELECTRIC_SUMINSURED") = txtelecricalSumInsured.Text
            dr.Item("ELECTRICAL_DESCRIPTION") = txtelecricalDescription.Text
            dt.Rows.Add(dr)


            If Session("Electricaldetails") IsNot Nothing Then
                dtelectrialmerge = Session("Electricaldetails")
                dt.Merge(dtelectrialmerge)
                Session("Electricaldetails") = dt
                gvElectricalaccessories.DataSource = Session("Electricaldetails")
                gvElectricalaccessories.DataBind()
            Else

                Session("Electricaldetails") = dt
                gvElectricalaccessories.DataSource = Session("Electricaldetails")
                gvElectricalaccessories.DataBind()
            End If
        End If
        pnlElectrical.Visible = False
        txttotalSumInsured.Text = TotalSumInsured(DirectCast(Session("Electricaldetails"), DataTable), "ELECTRIC_SUMINSURED")
    End Sub

    Protected Sub btnNonElectricalAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNonElectricalAdd.Click
        If chkNonElectricalAccessoriesCovered.Checked = True Then
            pnlNonelectrical.Visible = True
            Session("NONELECTRICALEDIT") = 0
        Else
            pnlNonelectrical.Visible = False
        End If
        ClearText(pnlNonelectrical)
    End Sub

    Protected Sub btnNonelectricAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNonelectricAdd.Click
        Dim dt As New DataTable
        Dim dtNonelecmerge As New DataTable
        dtNonelecmerge = Session("NonElectricaldetails")
        If Session("NONELECTRICALEDIT") = 1 Then
            Dim iCount As Integer = gvNonElecTrical.SelectedRow.RowIndex
            dtNonelecmerge.Rows(iCount).Item("NON_ELECTRICAL_SERIAL_NUMBER") = txtNonelectricalSerialNumber.Text
            dtNonelecmerge.Rows(iCount).Item("NON_PRODUCT_SERIAL_NUMBER") = txtNonelectricalProductserialNumber.Text
            dtNonelecmerge.Rows(iCount).Item("NON_ELECTRICAL_MAKE") = txtNonelectricalmake.Text
            dtNonelecmerge.Rows(iCount).Item("NON_ELECTRIC_SUMINSURED") = txtNonelectricalSumInsured.Text
            dtNonelecmerge.Rows(iCount).Item("NON_ELECTRICAL_DESCRIPTION") = txtNonelectricalDescription.Text
            Session("NonElectricaldetails") = dtNonelecmerge
            gvNonElecTrical.DataSource = Session("NonElectricaldetails")
            gvNonElecTrical.DataBind()

        Else
            dt.Columns.Add(New DataColumn("NON_ELECTRICAL_SERIAL_NUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NON_PRODUCT_SERIAL_NUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NON_ELECTRICAL_MAKE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NON_ELECTRIC_SUMINSURED", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NON_ELECTRICAL_DESCRIPTION", System.Type.GetType("System.String")))


            Dim dr As DataRow
            dr = dt.NewRow()
            dr.Item("NON_ELECTRICAL_SERIAL_NUMBER") = txtNonelectricalSerialNumber.Text
            dr.Item("NON_PRODUCT_SERIAL_NUMBER") = txtNonelectricalProductserialNumber.Text
            dr.Item("NON_ELECTRICAL_MAKE") = txtNonelectricalmake.Text
            dr.Item("NON_ELECTRIC_SUMINSURED") = txtNonelectricalSumInsured.Text
            dr.Item("NON_ELECTRICAL_DESCRIPTION") = txtNonelectricalDescription.Text
            dt.Rows.Add(dr)


            If Session("NonElectricaldetails") IsNot Nothing Then
                dtNonelecmerge = Session("NonElectricaldetails")
                dt.Merge(dtNonelecmerge)
                Session("NonElectricaldetails") = dt

                gvNonElecTrical.DataSource = Session("NonElectricaldetails")
                gvNonElecTrical.DataBind()
            Else

                Session("NonElectricaldetails") = dt
                gvNonElecTrical.DataSource = Session("NonElectricaldetails")
                gvNonElecTrical.DataBind()
            End If


        End If

        pnlNonelectrical.Visible = False
        txtSumInsurNonelec.Text = TotalSumInsured(DirectCast(Session("NonElectricaldetails"), DataTable), "NON_ELECTRIC_SUMINSURED")
    End Sub


    Protected Sub btnOkdriverDetails_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkdriverDetails.Click

        Dim dt As New DataTable
        Dim dtDriverdetailsmerge As New DataTable
        dtDriverdetailsmerge = Session("DriverDetails")

        If Session("DRIVERDETAILEDIT") = 1 Then
            Dim iCount As Integer = gvDriverDetails.SelectedRow.RowIndex


            dtDriverdetailsmerge.Rows(iCount).Item("VEHICLE_DRIVEN_BY") = ddlvehicledrivenby.SelectedItem.Text

            If ddlvehicledrivenby.SelectedItem.Text = "Paid Driver" Then
                dtDriverdetailsmerge.Rows(iCount).Item("PAID_DRIVER") = Convert.ToInt32(chkPaidDriver.Checked)
                dtDriverdetailsmerge.Rows(iCount).Item("MORE_THAN_ONE_DRIVER") = Convert.ToInt32(chkVehicleDrivenBymorethanoneperson.Checked)
                dtDriverdetailsmerge.Rows(iCount).Item("DRIVER_NAME") = txtDrivername.Text
                dtDriverdetailsmerge.Rows(iCount).Item("AGE") = txtAge.Text
            End If
            dtDriverdetailsmerge.Rows(iCount).Item("GENDER") = ddlGender.SelectedItem.Text
            dtDriverdetailsmerge.Rows(iCount).Item("RELATIONSHIP_WITH_OWNER") = ddlRelationshipwithowner.SelectedItem.Text
            dtDriverdetailsmerge.Rows(iCount).Item("DRIVER_EDUCATION_QUALIFICATION") = txtDriverEduQualification.Text
            dtDriverdetailsmerge.Rows(iCount).Item("DRIVING_LICENCE_NUMBER") = txtDriversLicenceNumber.Text
            dtDriverdetailsmerge.Rows(iCount).Item("LICENSE_ISSUING_AUTHORITY") = txtDriversLicenceIssueAutority.Text
            dtDriverdetailsmerge.Rows(iCount).Item("DATE_OF_FIRST_ISSUE") = txtDriversIssueDate.Text
            dtDriverdetailsmerge.Rows(iCount).Item("RENEWAL_DATE") = txtDriversExpiryDate.Text
            dtDriverdetailsmerge.Rows(iCount).Item("DRIVERS_EXPERIENCE") = txtDriversExperience.Text
            dtDriverdetailsmerge.Rows(iCount).Item("TYPE_OF_LICENCE") = ddlTypeoflicence.SelectedItem.Text
            dtDriverdetailsmerge.Rows(iCount).Item("DEFENSIVE_ADVANCED_DRIVING") = Convert.ToInt32(chkHasthedriverUndergone.Checked)

            dtDriverdetailsmerge.Rows(iCount).Item("ENDORSEMENTS") = txtEndorsements.Text
            Session("DRIVERDETAILEDIT") = dtDriverdetailsmerge
            gvDriverDetails.DataSource = Session("DRIVERDETAILEDIT")
            gvDriverDetails.DataBind()


        Else

            If Session("DriverDetails") IsNot Nothing AndAlso gvDriverDetails.Rows.Count > 0 AndAlso ddlvehicledrivenby.SelectedItem.Text <> dtDriverdetailsmerge.Rows(0).Item("VEHICLE_DRIVEN_BY").ToString() Then

                Session("DriverDetails") = Nothing
                'Session("DRIVERDETAILEDIT") = 0
                'btnOkdriverDetails_Click(sender, e)
            End If
            dt.Columns.Add(New DataColumn("VEHICLE_DRIVEN_BY", System.Type.GetType("System.String")))
            If ddlvehicledrivenby.SelectedItem.Text = "Paid Driver" Then
                dt.Columns.Add(New DataColumn("PAID_DRIVER", System.Type.GetType("System.String")))
                dt.Columns.Add(New DataColumn("MORE_THAN_ONE_DRIVER", System.Type.GetType("System.String")))
                dt.Columns.Add(New DataColumn("DRIVER_NAME", System.Type.GetType("System.String")))
                dt.Columns.Add(New DataColumn("AGE", System.Type.GetType("System.String")))
            End If

            dt.Columns.Add(New DataColumn("GENDER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("RELATIONSHIP_WITH_OWNER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("DRIVER_EDUCATION_QUALIFICATION", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("DRIVING_LICENCE_NUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("LICENSE_ISSUING_AUTHORITY", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("DATE_OF_FIRST_ISSUE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("RENEWAL_DATE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("DRIVERS_EXPERIENCE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("TYPE_OF_LICENCE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("DEFENSIVE_ADVANCED_DRIVING", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("ENDORSEMENTS", System.Type.GetType("System.String")))

            Dim dr As DataRow
            dr = dt.NewRow()
            dr.Item("VEHICLE_DRIVEN_BY") = ddlvehicledrivenby.SelectedItem.Text

            If ddlvehicledrivenby.SelectedItem.Text = "Paid Driver" Then
                dr.Item("PAID_DRIVER") = Convert.ToInt32(chkPaidDriver.Checked)
                dr.Item("MORE_THAN_ONE_DRIVER") = Convert.ToInt32(chkVehicleDrivenBymorethanoneperson.Checked)
                dr.Item("DRIVER_NAME") = txtDrivername.Text
                dr.Item("AGE") = txtAge.Text
            End If
            dr.Item("GENDER") = ddlGender.SelectedItem.Text
            dr.Item("RELATIONSHIP_WITH_OWNER") = ddlRelationshipwithowner.SelectedItem.Text
            dr.Item("DRIVER_EDUCATION_QUALIFICATION") = txtDriverEduQualification.Text
            dr.Item("DRIVING_LICENCE_NUMBER") = txtDriversLicenceNumber.Text
            dr.Item("LICENSE_ISSUING_AUTHORITY") = txtDriversLicenceIssueAutority.Text
            dr.Item("DATE_OF_FIRST_ISSUE") = txtDriversIssueDate.Text
            dr.Item("RENEWAL_DATE") = txtDriversExpiryDate.Text
            dr.Item("DRIVERS_EXPERIENCE") = txtDriversExperience.Text
            dr.Item("TYPE_OF_LICENCE") = ddlTypeoflicence.SelectedItem.Text
            dr.Item("DEFENSIVE_ADVANCED_DRIVING") = Convert.ToInt32(chkHasthedriverUndergone.Checked)

            dr.Item("ENDORSEMENTS") = txtEndorsements.Text


            dt.Rows.Add(dr)

            If Session("DriverDetails") IsNot Nothing Then
                dtDriverdetailsmerge = Session("DriverDetails")
                dt.Merge(dtDriverdetailsmerge)
                Session("DriverDetails") = dt

                gvDriverDetails.DataSource = Session("DriverDetails")
                gvDriverDetails.DataBind()
            Else

                Session("DriverDetails") = dt
                gvDriverDetails.DataSource = Session("DriverDetails")
                gvDriverDetails.DataBind()
            End If


        End If


        pnlDriverDetails.Visible = False

    End Sub

    Protected Sub btnOkPersonal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOkPersonal.Click
        Dim dt As New DataTable
        Dim dtpersonalaccidentmerge As New DataTable
        dtpersonalaccidentmerge = Session("PersonalAccident")
        If Session("PADETAILSEDIT") = 1 Then
            Dim iCount As Integer = gvpersonalAccidentDetails.SelectedRow.RowIndex
            dtpersonalaccidentmerge.Rows(iCount).Item("PA_SERIAL_NUMBER") = txtSerialnumber.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("PA_NAME") = txtname.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("PA_SUM_INSURED") = txtsumInsured.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("PA_NOMINEE_DETAILS") = txtNomineename.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("NOMINEE_ADD1") = txtPALine1.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("NOMINEE_ADD2") = txtPALine2.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("NOMINEE_CITY") = txtPACity.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("NOMINEE_STATE") = txtPAState.Text
            dtpersonalaccidentmerge.Rows(iCount).Item("NOMINEE_PINCODE") = txtPAPin.Text
            Session("PersonalAccident") = dtpersonalaccidentmerge
            gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
            gvpersonalAccidentDetails.DataBind()

        Else
            dt.Columns.Add(New DataColumn("PA_SERIAL_NUMBER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PA_NAME", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PA_SUM_INSURED", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("PA_NOMINEE_DETAILS", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NOMINEE_ADD1", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NOMINEE_ADD2", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NOMINEE_CITY", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NOMINEE_STATE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NOMINEE_PINCODE", System.Type.GetType("System.String")))


            Dim dr As DataRow
            dr = dt.NewRow()
            dr.Item("PA_SERIAL_NUMBER") = txtSerialnumber.Text
            dr.Item("PA_NAME") = txtname.Text
            dr.Item("PA_SUM_INSURED") = txtsumInsured.Text
            dr.Item("PA_NOMINEE_DETAILS") = txtNomineename.Text
            dr.Item("NOMINEE_ADD1") = txtPALine1.Text
            dr.Item("NOMINEE_ADD2") = txtPALine2.Text
            dr.Item("NOMINEE_CITY") = txtPACity.Text
            dr.Item("NOMINEE_STATE") = txtPAState.Text
            dr.Item("NOMINEE_PINCODE") = txtPAPin.Text
            dt.Rows.Add(dr)


            If Session("PersonalAccident") IsNot Nothing Then
                dtpersonalaccidentmerge = Session("PersonalAccident")
                dt.Merge(dtpersonalaccidentmerge)
                Session("PersonalAccident") = dt

                gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
                gvpersonalAccidentDetails.DataBind()
            Else

                Session("PersonalAccident") = dt
                gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
                gvpersonalAccidentDetails.DataBind()
            End If
        End If

        pnlPersonalaccidentDetails.Visible = False
        txtTotalsuminsuredofallnamedpersons.Text = TotalSumInsured(DirectCast(Session("PersonalAccident"), DataTable), "PA_SUM_INSURED")
    End Sub

    Protected Sub btnaddFinancier_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnaddFinancier.Click
        ClearText(pnlfinancierDetails)

        If chkFinancierDetails.Checked = True Then
            Session("FINANCIEREDIT") = 0

            pnlfinancierDetails.Visible = True
        Else
            pnlfinancierDetails.Visible = False
        End If

    End Sub

    Protected Sub btnFinancierOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinancierOk.Click
        pnlFinancierDetails1.Visible = True
        Dim dt As New DataTable
        Dim dtFinanciermerge As New DataTable
        dtFinanciermerge = Session("FinancierDetails")
        If Session("FINANCIEREDIT") = 1 Then
            Dim iCount As Integer = gvFinancierDetails.SelectedRow.RowIndex
            dtFinanciermerge.Rows(iCount).Item("AGREEMENT_TYPE") = ddlagreementType.SelectedItem.Text
            dtFinanciermerge.Rows(iCount).Item("FINANCIER_TYPE") = ddlFinancerType.SelectedItem.Text
            dtFinanciermerge.Rows(iCount).Item("NAME_OF_FINANCIER") = txtNameofFinancier.Text
            dtFinanciermerge.Rows(iCount).Item("FINANCIER_ADD1") = txtFinancierLine1.Text
            dtFinanciermerge.Rows(iCount).Item("FINANCIER_ADD2") = txtFinancierLine2.Text
            dtFinanciermerge.Rows(iCount).Item("FINANCIER_CITY") = txtFinancierCity.Text
            dtFinanciermerge.Rows(iCount).Item("FINANCIER_PINCODE") = txtFinancierPincode.Text
            dtFinanciermerge.Rows(iCount).Item("FINANCIER_STATE") = txtFinancerState.Text
            Session("FinancierDetails") = dtFinanciermerge
            gvFinancierDetails.DataSource = Session("FinancierDetails")
            gvFinancierDetails.DataBind()
        Else
            dt.Columns.Add(New DataColumn("AGREEMENT_TYPE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("FINANCIER_TYPE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("NAME_OF_FINANCIER", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("FINANCIER_ADD1", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("FINANCIER_ADD2", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("FINANCIER_CITY", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("FINANCIER_PINCODE", System.Type.GetType("System.String")))
            dt.Columns.Add(New DataColumn("FINANCIER_STATE", System.Type.GetType("System.String")))


            Dim dr As DataRow
            dr = dt.NewRow()
            dr.Item("AGREEMENT_TYPE") = ddlagreementType.SelectedItem.Text
            dr.Item("FINANCIER_TYPE") = ddlFinancerType.SelectedItem.Text
            dr.Item("NAME_OF_FINANCIER") = txtNameofFinancier.Text
            dr.Item("FINANCIER_ADD1") = txtFinancierLine1.Text
            dr.Item("FINANCIER_ADD2") = txtFinancierLine2.Text
            dr.Item("FINANCIER_CITY") = txtFinancierCity.Text
            dr.Item("FINANCIER_PINCODE") = txtFinancierPincode.Text
            dr.Item("FINANCIER_STATE") = txtFinancerState.Text
            dt.Rows.Add(dr)
            ''ds.Tables.Add(dt)
            'If Session("FinancierDetails") IsNot Nothing Then
            '    dt1 = Session("FinancierDetails")
            '    dt1.Merge(dt)
            'Else
            '    Session("FinancierDetails") = dt
            'End If

            'Response.Redirect("MotorAddRisk.aspx")
            If Session("FinancierDetails") IsNot Nothing Then
                dtFinanciermerge = Session("FinancierDetails")
                dt.Merge(dtFinanciermerge)
                Session("FinancierDetails") = dt

                gvFinancierDetails.DataSource = Session("FinancierDetails")
                gvFinancierDetails.DataBind()
            Else

                Session("FinancierDetails") = dt
                gvFinancierDetails.DataSource = Session("FinancierDetails")
                gvFinancierDetails.DataBind()
            End If
        End If
        pnlfinancierDetails.Visible = False
        pnlFinancierDetails1.Visible = True

    End Sub

    Protected Sub gvPreviousInsurancehistory_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvPreviousInsurancehistory.RowEditing
        gvPreviousInsurancehistory.EditIndex = e.NewEditIndex
        gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
        gvPreviousInsurancehistory.DataBind()


    End Sub

    Protected Sub gvPreviousInsurancehistory_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvPreviousInsurancehistory.RowUpdating

        Dim dt As DataTable = DirectCast(Session("PersonalInsuranceHistory"), DataTable)
        Dim row As GridViewRow = gvPreviousInsurancehistory.Rows(e.RowIndex)
        dt.Rows(gvPreviousInsurancehistory.EditIndex)("PI_POLICYNUMBER") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvPreviousInsurancehistory.EditIndex)("PI_NUMBEROFCLAIMS") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvPreviousInsurancehistory.EditIndex)("PI_YEAR") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text

        dt.Rows(gvPreviousInsurancehistory.EditIndex)("PI_TOTALAMOUNTOFCLAIMS") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text

        dt.Rows(gvPreviousInsurancehistory.EditIndex)("PI_INCURREDLOSSRATIO") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        gvPreviousInsurancehistory.EditIndex = -1

        Session("PersonalInsuranceHistory") = dt
        gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
        gvPreviousInsurancehistory.DataBind()

    End Sub

    Protected Sub gvPreviousInsurancehistory_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvPreviousInsurancehistory.RowCancelingEdit
        gvPreviousInsurancehistory.EditIndex = -1
        gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
        gvPreviousInsurancehistory.DataBind()
    End Sub

    Protected Sub gvFinancierDetails_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvFinancierDetails.RowUpdating
        Dim dt As DataTable = DirectCast(Session("FinancierDetails"), DataTable)
        Dim row As GridViewRow = gvFinancierDetails.Rows(e.RowIndex)
        dt.Rows(gvFinancierDetails.EditIndex)("AGREEMENT_TYPE") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_TYPE") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvFinancierDetails.EditIndex)("NAME_OF_FINANCIER") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text

        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_ADD1") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text

        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_ADD2") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_CITY") = DirectCast((row.Cells(7).Controls(0)), TextBox).Text
        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_PINCODE") = DirectCast((row.Cells(8).Controls(0)), TextBox).Text
        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_STATE") = DirectCast((row.Cells(9).Controls(0)), TextBox).Text

        gvFinancierDetails.EditIndex = -1

        Session("FinancierDetails") = dt
        gvFinancierDetails.DataSource = Session("FinancierDetails")
        gvFinancierDetails.DataBind()

    End Sub

    Protected Sub gvFinancierDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvFinancierDetails.RowEditing
        gvFinancierDetails.EditIndex = e.NewEditIndex
        gvFinancierDetails.DataSource = Session("FinancierDetails")
        gvFinancierDetails.DataBind()

    End Sub

    Protected Sub gvFinancierDetails_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvFinancierDetails.RowCancelingEdit
        gvFinancierDetails.EditIndex = -1
        gvFinancierDetails.DataSource = Session("FinancierDetails")
        gvFinancierDetails.DataBind()
    End Sub

    Protected Sub gvElectricalaccessories_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvElectricalaccessories.RowEditing
        gvElectricalaccessories.EditIndex = e.NewEditIndex
        gvElectricalaccessories.DataSource = Session("Electricaldetails")
        gvElectricalaccessories.DataBind()

    End Sub

    Protected Sub gvElectricalaccessories_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvElectricalaccessories.RowCancelingEdit
        gvElectricalaccessories.EditIndex = -1
        gvElectricalaccessories.DataSource = Session("Electricaldetails")
        gvElectricalaccessories.DataBind()
    End Sub

    Protected Sub gvElectricalaccessories_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvElectricalaccessories.RowUpdating

        Dim dt As DataTable = DirectCast(Session("Electricaldetails"), DataTable)
        Dim row As GridViewRow = gvElectricalaccessories.Rows(e.RowIndex)
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRICAL_SERIAL_NUMBER") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("PRODUCT_SERIAL_NUMBER") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRIC_MAKE") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRIC_SUMINSURED") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRICAL_DESCRIPTION") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        gvElectricalaccessories.EditIndex = -1

        Session("Electricaldetails") = dt
        gvElectricalaccessories.DataSource = Session("Electricaldetails")
        gvElectricalaccessories.DataBind()


    End Sub
    Protected Sub gvNonElecTrical_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvNonElecTrical.RowEditing
        gvNonElecTrical.EditIndex = e.NewEditIndex
        gvNonElecTrical.DataSource = Session("NonElectricaldetails")
        gvNonElecTrical.DataBind()
    End Sub
    Protected Sub gvNonElecTrical_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvNonElecTrical.RowUpdating
        Dim dt As DataTable = DirectCast(Session("NonElectricaldetails"), DataTable)
        Dim row As GridViewRow = gvNonElecTrical.Rows(e.RowIndex)
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRICAL_SERIAL_NUMBER") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_PRODUCT_SERIAL_NUMBER") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRIC_MAKE") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRIC_SUMINSURED") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRICAL_DESCRIPTION") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        gvNonElecTrical.EditIndex = -1

        Session("NonElectricaldetails") = dt
        gvNonElecTrical.DataSource = Session("NonElectricaldetails")
        gvNonElecTrical.DataBind()
    End Sub



    Protected Sub gvNonElecTrical_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvNonElecTrical.RowCancelingEdit
        gvNonElecTrical.EditIndex = -1
        gvNonElecTrical.DataSource = Session("NonElectricaldetails")
        gvNonElecTrical.DataBind()
    End Sub


    Protected Sub gvDriverDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvDriverDetails.RowEditing
        gvDriverDetails.EditIndex = e.NewEditIndex
        gvDriverDetails.DataSource = Session("DriverDetails")
        gvDriverDetails.DataBind()
    End Sub

    Protected Sub gvDriverDetails_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvDriverDetails.RowUpdating
        Dim dt As DataTable = DirectCast(Session("DriverDetails"), DataTable)
        Dim row As GridViewRow = gvDriverDetails.Rows(e.RowIndex)
        dt.Rows(gvDriverDetails.EditIndex)("VEHICLE_DRIVEN_BY") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("PAID_DRIVER") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("MORE_THAN_ONE_DRIVER") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVER_NAME") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("AGE") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("GENDER") = DirectCast((row.Cells(7).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("RELATIONSHIP_WITH_OWNER") = DirectCast((row.Cells(8).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVER_EDUCATION_QUALIFICATION") = DirectCast((row.Cells(9).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVING_LICENCE_NUMBER") = DirectCast((row.Cells(10).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("LICENSE_ISSUING_AUTHORITY") = DirectCast((row.Cells(11).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DATE_OF_FIRST_ISSUE") = DirectCast((row.Cells(12).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("RENEWAL_DATE") = DirectCast((row.Cells(13).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVERS_EXPERIENCE") = DirectCast((row.Cells(14).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("TYPE_OF_LICENCE") = DirectCast((row.Cells(15).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DEFENSIVE_ADVANCED_DRIVING") = DirectCast((row.Cells(16).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("ENDORSEMENTS") = DirectCast((row.Cells(17).Controls(0)), TextBox).Text
        gvDriverDetails.EditIndex = -1

        Session("DriverDetails") = dt
        gvDriverDetails.DataSource = Session("DriverDetails")
        gvDriverDetails.DataBind()
    End Sub

    Protected Sub gvDriverDetails_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvDriverDetails.RowCancelingEdit
        gvDriverDetails.EditIndex = -1
        gvDriverDetails.DataSource = Session("DriverDetails")
        gvDriverDetails.DataBind()
    End Sub


    Protected Sub gvpersonalAccidentDetails_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvpersonalAccidentDetails.RowEditing
        gvpersonalAccidentDetails.EditIndex = e.NewEditIndex
        gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
        gvpersonalAccidentDetails.DataBind()
    End Sub

    Protected Sub gvpersonalAccidentDetails_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvpersonalAccidentDetails.RowUpdating
        Dim dt As DataTable = DirectCast(Session("PersonalAccident"), DataTable)
        Dim row As GridViewRow = gvpersonalAccidentDetails.Rows(e.RowIndex)
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("PA_SERIAL_NUMBER") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("PA_NAME") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("PA_SUM_INSURED") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("PA_NOMINEE_DETAILS") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("NOMINEE_ADD1") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("NOMINEE_ADD2") = DirectCast((row.Cells(7).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("NOMINEE_CITY") = DirectCast((row.Cells(8).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("NOMINEE_STATE") = DirectCast((row.Cells(9).Controls(0)), TextBox).Text
        dt.Rows(gvpersonalAccidentDetails.EditIndex)("NOMINEE_PINCODE") = DirectCast((row.Cells(10).Controls(0)), TextBox).Text

        gvpersonalAccidentDetails.EditIndex = -1

        Session("PersonalAccident") = dt
        gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
        gvpersonalAccidentDetails.DataBind()
    End Sub

    Protected Sub gvpersonalAccidentDetails_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvpersonalAccidentDetails.RowCancelingEdit
        gvpersonalAccidentDetails.EditIndex = -1
        gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
        gvpersonalAccidentDetails.DataBind()
    End Sub

    Protected Sub gvPreviousInsurancehistory_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvPreviousInsurancehistory.RowDeleting
        Dim dt As DataTable = DirectCast(Session("PersonalInsuranceHistory"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("PersonalInsuranceHistory") = dt
        gvPreviousInsurancehistory.DataSource = Session("PersonalInsuranceHistory")
        gvPreviousInsurancehistory.DataBind()
    End Sub

    Protected Sub gvElectricalaccessories_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvElectricalaccessories.RowDeleting
        Dim dt As DataTable = DirectCast(Session("Electricaldetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("Electricaldetails") = dt
        gvElectricalaccessories.DataSource = Session("Electricaldetails")
        gvElectricalaccessories.DataBind()
        txttotalSumInsured.Text = TotalSumInsured(dt, "ELECTRIC_SUMINSURED")
    End Sub

    Protected Sub gvNonElecTrical_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvNonElecTrical.RowDeleting
        Dim dt As DataTable = DirectCast(Session("NonElectricaldetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("NonElectricaldetails") = dt
        gvNonElecTrical.DataSource = Session("NonElectricaldetails")
        gvNonElecTrical.DataBind()
        txtSumInsurNonelec.Text = TotalSumInsured(DirectCast(Session("NonElectricaldetails"), DataTable), "NON_ELECTRIC_SUMINSURED")
        
    End Sub

    Protected Sub gvDriverDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDriverDetails.RowDeleting
        Dim dt As DataTable = DirectCast(Session("DriverDetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("DriverDetails") = dt
        gvDriverDetails.DataSource = Session("DriverDetails")
        gvDriverDetails.DataBind()
    End Sub

    Protected Sub gvpersonalAccidentDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvpersonalAccidentDetails.RowDeleting
        Dim dt As DataTable = DirectCast(Session("PersonalAccident"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("PersonalAccident") = dt
        gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
        gvpersonalAccidentDetails.DataBind()

        txtTotalsuminsuredofallnamedpersons.Text = TotalSumInsured(DirectCast(Session("PersonalAccident"), DataTable), "PA_SUM_INSURED")
    End Sub

    Protected Sub gvFinancierDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvFinancierDetails.RowDeleting
        Dim dt As DataTable = DirectCast(Session("FinancierDetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("FinancierDetails") = dt
        gvFinancierDetails.DataSource = Session("FinancierDetails")
        gvFinancierDetails.DataBind()
        If gvFinancierDetails.Rows.Count > 0 Then
            pnlFinancierDetails1.Visible = True
        Else
            pnlFinancierDetails1.Visible = False
        End If
    End Sub

    Protected Sub btnFind_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFind.Click
        Try
            pnlVehicleMake.Visible = True
            VehicleMake()
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnVehicleOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVehicleOk.Click

        pnlVehicleMake.Visible = False
        If gvVehicleMake IsNot Nothing AndAlso gvVehicleMake.Rows.Count > 0 Then
            txtVehicleModel.Text = gvVehicleMake.SelectedRow.Cells(1).Text
            txtBodyType.Text = gvVehicleMake.SelectedRow.Cells(2).Text
            txtCC.Text = gvVehicleMake.SelectedRow.Cells(3).Text
            txtSeatingcapacity.Text = gvVehicleMake.SelectedRow.Cells(4).Text
        End If

    End Sub

    Protected Sub btnFindVehicleAddress_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindVehicleAddress.Click
        Try
            pnlVehicleAddress.Visible = True
            VehicleRegistrationAddress()
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub btnVehicleAddressOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVehicleAddressOk.Click
        Try
            pnlVehicleAddress.Visible = False
            If gvVehicleAddress IsNot Nothing AndAlso gvVehicleAddress.Rows.Count > 0 Then
                txtCity.Text = gvVehicleAddress.SelectedRow.Cells(1).Text
                txtRTOLocation.Text = gvVehicleAddress.SelectedRow.Cells(1).Text
                txtState.Text = gvVehicleAddress.SelectedRow.Cells(2).Text
                txtZone.Text = gvVehicleAddress.SelectedRow.Cells(3).Text
            End If
        Catch ex As Exception

        End Try
    End Sub

    Public Sub EnableControl()
        If (ChkTrailer.Checked) Then
            txtNumberoftrailers.Enabled = True
            txtTrailersRegisterNumber.Enabled = True
            txtTrailersIDV.Enabled = True

            RequiredFieldValidator12.Enabled = True
            RequiredFieldValidator13.Enabled = True
            RequiredFieldValidator14.Enabled = True


        Else
            txtNumberoftrailers.Enabled = False
            'txtNumberoftrailers.BackColor = Drawing.Color.Gray
            txtTrailersRegisterNumber.Enabled = False
            'txtTrailersRegisterNumber.BackColor = Drawing.Color.Gray
            txtTrailersIDV.Enabled = False


            RequiredFieldValidator12.Enabled = False
            RequiredFieldValidator13.Enabled = False
            RequiredFieldValidator14.Enabled = False


        End If

        If (chkLPGkit.Checked) Then
            chkCNGSI.Checked = False
            txtCNGSI.Text = ""
            txtLPGSI.Enabled = True
            txtCNGSI.Enabled = False
            RequiredFieldValidator49.Enabled = True

        Else
            txtLPGSI.Enabled = False
            txtCNGSI.Enabled = True
            RequiredFieldValidator49.Enabled = False
        End If
        If (chkCNGSI.Checked) Then
            chkLPGkit.Checked = False
            txtLPGSI.Text = ""
            txtLPGSI.Enabled = False
            txtCNGSI.Enabled = True
            RequiredFieldValidator25.Enabled = True

        Else
            RequiredFieldValidator25.Enabled = False
            txtLPGSI.Enabled = True
            txtCNGSI.Enabled = False
        End If

        If chkVechileusedforrallies.Checked Then
            txtStartDate.Enabled = True
            txtEndDate.Enabled = True

            RequiredFieldValidator29.Enabled = True
            RequiredFieldValidator30.Enabled = True


        Else
            txtStartDate.Enabled = False
            txtEndDate.Enabled = False

            RequiredFieldValidator29.Enabled = False
            RequiredFieldValidator30.Enabled = False
        End If

        If chkGeographical.Checked Then
            chkBangaldesh.Enabled = True
            chkPakistan.Enabled = True
            chkSrilanka.Enabled = True
            chkMaldives.Enabled = True
            chkNepal.Enabled = True
            chkBhutan.Enabled = True
        Else
            chkBangaldesh.Enabled = False
            chkPakistan.Enabled = False
            chkSrilanka.Enabled = False
            chkMaldives.Enabled = False
            chkNepal.Enabled = False
            chkBhutan.Enabled = False
        End If

        If chkBrokerageAgencyCommission.Checked Then
            txtDiscountAgentCommission.Enabled = True

            RequiredFieldValidator26.Enabled = True

        Else
            txtDiscountAgentCommission.Enabled = False
            RequiredFieldValidator26.Enabled = False
        End If

        If chkGoodfeatureDiscount.Checked Then
            txtDiscountGoodfeature.Enabled = True
            RequiredFieldValidator27.Enabled = True
        Else
            txtDiscountGoodfeature.Enabled = False
            RequiredFieldValidator27.Enabled = False
        End If

        If chkSpecialDiscount.Checked Then
            txtDiscountSpecial.Enabled = True

            RequiredFieldValidator28.Enabled = True
        Else
            txtDiscountSpecial.Enabled = False
            RequiredFieldValidator28.Enabled = False
        End If

        If chkNonConvectionalPower.Checked Then
            txtDescription.Enabled = True
        Else
            txtDescription.Enabled = False
        End If

        If chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements.Checked Then
            chkThisvechcle.Enabled = True
        Else
            chkThisvechcle.Enabled = False
        End If

        If chkThisvechcle.Checked Then
            txtSumInsured1.Enabled = True
        Else
            txtSumInsured1.Enabled = False
        End If

        If chkPersonalaccidentcoverforunnamedpersons.Checked Then
            txtNumberOfunnamedPersons.Enabled = True
            txtSumInsuredPerPerson.Enabled = True

            RequiredFieldValidator36.Enabled = True
            RequiredFieldValidator37.Enabled = True

        Else
            txtNumberOfunnamedPersons.Enabled = False
            txtSumInsuredPerPerson.Enabled = False

            RequiredFieldValidator36.Enabled = False
            RequiredFieldValidator37.Enabled = False

        End If
        If chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner.Checked Then
            txtNumberOfpaiddrivers.Enabled = True
            txtSumInsured2.Enabled = True
        Else
            txtNumberOfpaiddrivers.Enabled = False
            txtSumInsured2.Enabled = False

        End If
        If chkVehicleInspection.Checked Then
            txtInspectionDate.Enabled = True
            txtInspectedByWhom.Enabled = True
            txtInspectionReport.Enabled = True

            RequiredFieldValidator46.Enabled = True
            RequiredFieldValidator47.Enabled = True
            RequiredFieldValidator48.Enabled = True


        Else
            txtInspectionDate.Enabled = False
            txtInspectedByWhom.Enabled = False
            txtInspectionReport.Enabled = False

            RequiredFieldValidator46.Enabled = False
            RequiredFieldValidator47.Enabled = False
            RequiredFieldValidator48.Enabled = False


        End If
        ChkRestrictedCover.Enabled = False

    End Sub


    Protected Sub ChkTrailer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkTrailer.CheckedChanged
        EnableControl()
    End Sub

    Protected Sub chkLPGkit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkLPGkit.CheckedChanged
        EnableControl()
    End Sub

    Protected Sub chkCNGSI_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkCNGSI.CheckedChanged
        EnableControl()
    End Sub

    Protected Sub chkVechileusedforrallies_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkVechileusedforrallies.CheckedChanged
        EnableControl()
    End Sub

    Protected Sub chkGeographical_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGeographical.CheckedChanged
        EnableControl()
    End Sub

    Protected Sub chkBrokerageAgencyCommission_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkBrokerageAgencyCommission.CheckedChanged, _
        chkGoodfeatureDiscount.CheckedChanged, _
        chkSpecialDiscount.CheckedChanged, _
        chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements.CheckedChanged, _
        chkNonConvectionalPower.CheckedChanged, _
        chkThisvechcle.CheckedChanged, _
        chkPersonalaccidentcoverforunnamedpersons.CheckedChanged, _
        chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner.CheckedChanged, _
        chkVehicleInspection.CheckedChanged

        EnableControl()
    End Sub

    'Protected Sub chkGoodfeatureDiscount_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkGoodfeatureDiscount.CheckedChanged
    '    EnableControl()
    'End Sub

    Protected Sub btnfinanciercancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfinanciercancel.Click
        pnlfinancierDetails.Visible = False
        ClearText(pnlfinancierDetails)
    End Sub

    Protected Sub btnPIHistoryCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPIHistoryCancel.Click
        pnlPIHistory.Visible = False
    End Sub

    Protected Sub btnelectricalcancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnelectricalcancel.Click
        pnlElectrical.Visible = False
    End Sub

    Protected Sub btnNonElectricalCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNonElectricalCancel.Click
        pnlNonelectrical.Visible = False
    End Sub

    Protected Sub btnCancelPersonal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelPersonal.Click
        pnlPersonalaccidentDetails.Visible = False
    End Sub
    Public Function FormatYYMMDD(ByRef strDate As String) As String
        Dim d, m, y As String
        Dim dt As Date '= Convert.ToDateTime(strDate)

        'If strDate = "" Then
        '    strDate = ""

        If strDate <> String.Empty Then
            dt = Convert.ToDateTime(strDate)
            d = Format(dt, "dd")
            m = Format(dt, "MM")
            y = Format(dt, "yyy")
            strDate = y & "-" & m & "-" & d
        End If
        Return strDate
    End Function

    Protected Sub gvFinancierDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvFinancierDetails.SelectedIndexChanged
        pnlfinancierDetails.Visible = True
        Dim dt As DataTable = Session("FinancierDetails")
        Dim iCount As Integer = gvFinancierDetails.SelectedRow.RowIndex
        ddlagreementType.SelectedItem.Text = dt.Rows(iCount).Item("AGREEMENT_TYPE").ToString()
        ddlFinancerType.SelectedItem.Text = dt.Rows(iCount).Item("FINANCIER_TYPE").ToString()
        txtNameofFinancier.Text = dt.Rows(iCount).Item("NAME_OF_FINANCIER").ToString()
        txtFinancierLine1.Text = dt.Rows(iCount).Item("FINANCIER_ADD1").ToString()
        txtFinancierLine2.Text = dt.Rows(iCount).Item("FINANCIER_ADD2").ToString()
        txtFinancierCity.Text = dt.Rows(iCount).Item("FINANCIER_CITY").ToString()
        txtFinancerState.Text = dt.Rows(iCount).Item("FINANCIER_STATE").ToString()
        txtFinancierPincode.Text = dt.Rows(iCount).Item("FINANCIER_PINCODE").ToString()
        Session("FINANCIEREDIT") = 1
    End Sub

    Protected Sub gvPreviousInsurancehistory_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvPreviousInsurancehistory.SelectedIndexChanged
        pnlPIHistory.Visible = True
        Dim dt As DataTable = Session("PersonalInsuranceHistory")
        Dim iCount As Integer = gvPreviousInsurancehistory.SelectedRow.RowIndex
        txtPIPolicyNumber.Text = dt.Rows(iCount).Item("PI_POLICYNUMBER")
        txtNumberofClaims.Text = dt.Rows(iCount).Item("PI_NUMBEROFCLAIMS")
        txtYear.Text = dt.Rows(iCount).Item("PI_YEAR")
        txtTotalamountofclaims.Text = dt.Rows(iCount).Item("PI_TOTALAMOUNTOFCLAIMS")
        txtIncurredLossRatio.Text = dt.Rows(iCount).Item("PI_INCURREDLOSSRATIO")
        Session("PIHISTORYEDIT") = 1
    End Sub

    Protected Sub gvElectricalaccessories_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvElectricalaccessories.SelectedIndexChanged
        pnlElectrical.Visible = True

        Dim dt As DataTable = Session("Electricaldetails")
        Dim iCount As Integer = gvElectricalaccessories.SelectedRow.RowIndex
        txtelectricalSerialNumber.Text = dt.Rows(iCount).Item("ELECTRICAL_SERIAL_NUMBER")
        txtelecricalProductserialNumber.Text = dt.Rows(iCount).Item("PRODUCT_SERIAL_NUMBER")
        txtelecricalmake.Text = dt.Rows(iCount).Item("ELECTRIC_MAKE")
        txtelecricalSumInsured.Text = dt.Rows(iCount).Item("ELECTRIC_SUMINSURED")
        txtelecricalDescription.Text = dt.Rows(iCount).Item("ELECTRICAL_DESCRIPTION")
        Session("ELECTRICALEDIT") = 1
    End Sub

    Protected Sub gvNonElecTrical_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvNonElecTrical.SelectedIndexChanged
        pnlNonelectrical.Visible = True
        Dim dt As DataTable = Session("NonElectricaldetails")
        Dim iCount As Integer = gvNonElecTrical.SelectedRow.RowIndex
        txtNonelectricalSerialNumber.Text = dt.Rows(iCount).Item("NON_ELECTRICAL_SERIAL_NUMBER")
        txtNonelectricalProductserialNumber.Text = dt.Rows(iCount).Item("NON_PRODUCT_SERIAL_NUMBER")
        txtNonelectricalmake.Text = dt.Rows(iCount).Item("NON_ELECTRICAL_MAKE")
        txtNonelectricalSumInsured.Text = dt.Rows(iCount).Item("NON_ELECTRIC_SUMINSURED")
        txtNonelectricalDescription.Text = dt.Rows(iCount).Item("NON_ELECTRICAL_DESCRIPTION")
        Session("NONELECTRICALEDIT") = 1
    End Sub

    Protected Sub gvDriverDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDriverDetails.SelectedIndexChanged
        pnlDriverDetails.Visible = True
        Dim dt As DataTable = Session("DriverDetails")
        Dim iCount As Integer = gvDriverDetails.SelectedRow.RowIndex
        If ddlvehicledrivenby.SelectedItem.Text = "Paid Driver" Then
            chkPaidDriver.Checked = dt.Rows(iCount).Item("PAID_DRIVER")
            chkVehicleDrivenBymorethanoneperson.Checked = dt.Rows(iCount).Item("MORE_THAN_ONE_DRIVER")
            txtDrivername.Text = dt.Rows(iCount).Item("DRIVER_NAME")
            dt.Rows(iCount).Item("AGE") = txtAge.Text
        End If
        ddlGender.SelectedItem.Text = dt.Rows(iCount).Item("GENDER")
        ddlRelationshipwithowner.SelectedItem.Text = dt.Rows(iCount).Item("RELATIONSHIP_WITH_OWNER")
        txtDriverEduQualification.Text = dt.Rows(iCount).Item("DRIVER_EDUCATION_QUALIFICATION")
        txtDriversLicenceNumber.Text = dt.Rows(iCount).Item("DRIVING_LICENCE_NUMBER")
        txtDriversLicenceIssueAutority.Text = dt.Rows(iCount).Item("LICENSE_ISSUING_AUTHORITY")
        txtDriversIssueDate.Text = dt.Rows(iCount).Item("DATE_OF_FIRST_ISSUE")
        txtDriversExpiryDate.Text = dt.Rows(iCount).Item("RENEWAL_DATE")
        txtDriversExperience.Text = dt.Rows(iCount).Item("DRIVERS_EXPERIENCE")
        ddlTypeoflicence.SelectedItem.Text = dt.Rows(iCount).Item("TYPE_OF_LICENCE")
        chkHasthedriverUndergone.Checked = dt.Rows(iCount).Item("DEFENSIVE_ADVANCED_DRIVING")
        txtEndorsements.Text = dt.Rows(iCount).Item("ENDORSEMENTS")
        Session("DRIVERDETAILEDIT") = 1
    End Sub

    Protected Sub gvpersonalAccidentDetails_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvpersonalAccidentDetails.SelectedIndexChanged
        pnlPersonalaccidentDetails.Visible = True
        Dim dt As DataTable = Session("PersonalAccident")
        Dim iCount As Integer = gvpersonalAccidentDetails.SelectedRow.RowIndex

        txtSerialnumber.Text = dt.Rows(iCount).Item("PA_SERIAL_NUMBER")
        txtname.Text = dt.Rows(iCount).Item("PA_NAME")
        txtsumInsured.Text = dt.Rows(iCount).Item("PA_SUM_INSURED")
        txtNomineename.Text = dt.Rows(iCount).Item("PA_NOMINEE_DETAILS")
        txtPALine1.Text = dt.Rows(iCount).Item("NOMINEE_ADD1")
        txtPALine2.Text = dt.Rows(iCount).Item("NOMINEE_ADD2")
        txtPACity.Text = dt.Rows(iCount).Item("NOMINEE_CITY")
        txtPAState.Text = dt.Rows(iCount).Item("NOMINEE_STATE")
        txtPAPin.Text = dt.Rows(iCount).Item("NOMINEE_PINCODE")

        Session("PADETAILSEDIT") = 1
    End Sub

    Protected Sub txtExShowRoom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExShowRoom.TextChanged
        'txtIDV.Text = txtExShowRoom.Text
        'txtIDVSystem.Text = txtExShowRoom.Text
    End Sub

    Public Sub ClearText(ByRef panelCtrl As Panel)
        For i As Integer = 0 To panelCtrl.Controls.Count - 1
            If panelCtrl.Controls(i).GetType().Name = "TextBox" Then
                Dim txtClear As TextBox
                txtClear = CType(panelCtrl.Controls(i), TextBox)
                txtClear.Text = ""
            End If
        Next
    End Sub
    Public Function TotalSumInsured(ByVal dt As System.Data.DataTable, ByVal ColumnName As String) As String
        Dim SumInsured As Integer = 0
        For iCount As Integer = 0 To dt.Rows.Count - 1
            SumInsured += dt.Rows(iCount).Item(ColumnName)
        Next
        Return SumInsured.ToString()
    End Function

    Protected Sub txtIDV_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIDV.TextChanged
        If txtIDV.Text.Length > 0 AndAlso CInt(txtIDV.Text) > CInt(txtExShowRoom.Text) Then
            txtIDV.Text = txtExShowRoom.Text
        End If
        txtIDVSystem.Text = txtIDV.Text
    End Sub

    Protected Sub txtNumberOfPaiddrivesCleanersWorkMan_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNumberOfPaiddrivesCleanersWorkMan.TextChanged

        If Convert.ToInt32(txtNumberOfPaiddrivesCleanersWorkMan.Text) > 4 Then
            txtNumberOfPaiddrivesCleanersWorkMan.Text = 4
        End If
    End Sub

    Protected Sub btnDriverCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDriverCancel.Click

        pnlDriverDetails.Visible = False
    End Sub

    'Public Sub workaround()

    '    txtAge.Text = "111"
    '    txtDrivername.Text = "111"
    '    txtDriversExpiryDate.Text = "dfdf"
    '    txtDriversIssueDate.Text = "dfsdf"
    '    txtDriversLicenceNumber.Text = "dsfsdf"
    'End Sub
End Class
