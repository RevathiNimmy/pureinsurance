Imports System.Xml
Imports System.Xml.XPath
Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2
Imports System.Data
Imports System.IO

Partial Class New_Business_MotorEditRisk
    Inherits System.Web.UI.Page
    Dim UserToken As UsernameToken
    Dim m_nRiskCnt As Integer
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2
    Dim m_sRiskDataXML As String
    Dim Nodeindex As New System.Collections.Specialized.NameValueCollection()
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


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
            EnableControl()
            pnlNonElecTrical1.Visible = False
            pnlDriverdetails1.Visible = False
            pnlFinancierDetails1.Visible = False
            PreviousInsurancehistory1.Visible = False
            pnlVehicleMake.Visible = False
            pnlVehicleAddress.Visible = False
            Nodeindex.Add("PIHISTORY", 0)
            Nodeindex.Add("CLAIMSHISTORY", 0)
            Nodeindex.Add("DRIVERDETAILS", 0)
            Nodeindex.Add("NONELECTRICAL", 0)
            Nodeindex.Add("ELECTRICAL", 0)
            Nodeindex.Add("PADETAILS", 0)
            Nodeindex.Add("FINANCIERDETAILS", 0)

            pnlPIHistory.Visible = False
            pnlElectrical.Visible = False
            pnlNonelectrical.Visible = False
            pnlDriverDetails.Visible = False
            pnlPersonalaccidentDetails.Visible = False
            pnlfinancierDetails.Visible = False
            UserToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            ''create the request and response objects
            'Dim oAddRiskRequestType As New AddRiskRequestType
            'Dim oAddRiskResponseType As New AddRiskResponseType



            'select * from dbo.GIS_User_Def_Header 
            'get the quote response
            BuildLists(oSAM, ddlCoverOptionpackage, STSListType.UserDefinedTable, "CVR_OPTION")
            BuildLists(oSAM, ddlPolicyType, STSListType.UserDefinedTable, "CVR_DETAIL")
            BuildLists1(oSAM, ddlVehicleMake, STSListType.PMLookup, "udl_vehMake")
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
            BuildLists1(oSAM, ddlFinancerType, STSListType.PMLookup, "UDL_FNCRTYPE")

            'create the request and response objects
            Dim oGetRiskRequestType As New GetRiskRequestType
            Dim oGetRiskResponseType As New GetRiskResponseType

            'get the quote response
            'oAddRiskRequestType = CType(Session("AddRiskRequestType"), AddRiskRequestType)

            'set up request object with some values
            With oGetRiskRequestType
                .BranchCode = "Headoff"


                'add values from the quote we just added
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFileKey = Session("InsuranceFileKey")
                .QuoteTimeStamp = Session("QuoteTimeStamp")
                .RiskKey = Session("RiskKey")
            End With

            Try
                oGetRiskResponseType = oSAM.GetRisk(oGetRiskRequestType)
                Dim m_sRiskDataXML As String = Nothing
                Dim xmlDoc As New System.Xml.XmlDocument
                With oGetRiskResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                    End If
                    m_sRiskDataXML = .XMLDataSet.ToString
                    Session("QuoteTimeStamp") = .QuoteTimeStamp


                    xmlDoc.LoadXml(m_sRiskDataXML)
                    BuildXMLNode(xmlDoc, "")
                    'uday
                    Session("m_sRiskDataXML") = m_sRiskDataXML


                    Dim oElement_DATA_SET As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET")

                    If oElement_DATA_SET IsNot Nothing Then

                        Dim oElement_RISK_OBJECTS As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS")

                        If oElement_RISK_OBJECTS IsNot Nothing Then

                            Dim oElement_MOTOR_POLICY_BINDER As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER")

                            If oElement_MOTOR_POLICY_BINDER IsNot Nothing Then

                                '/* cover selection Start
                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("POLICY_START_DATE") Then
                                    txtPolicyStartDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("POLICY_START_DATE").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("POLICY_START_TIME") Then
                                    txtTime1.Text = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("POLICY_START_TIME").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("COVER_NOTE_DATE") Then
                                    txtCoverNoteDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("COVER_NOTE_DATE").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("COVER_NOTE_TIME") Then
                                    txtTime2.Text = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("COVER_NOTE_TIME").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("COVER_DETAILS") Then
                                    ddlPolicyType.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("COVER_DETAILS").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("COVER_OPTIONS_PACKAGE") Then
                                    ddlCoverOptionpackage.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("COVER_OPTIONS_PACKAGE").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("COVERAGE_SELECTED") Then
                                    ChkRestrictedCover.Checked = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("COVERAGE_SELECTED").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("ADD_ON_COVER_PACKAGE") Then
                                    chkAddonCover.Checked = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("ADD_ON_COVER_PACKAGE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("ELECTRICICAL_ACCESSORIES") Then
                                    chkElectrical.Checked = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("ELECTRICICAL_ACCESSORIES").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("NON_ELECTRICAL_ACCESSORIES") Then
                                    chkNonElectricalAccessoriesCovered.Checked = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("NON_ELECTRICAL_ACCESSORIES").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("TOTAL_ELEC_SI") Then
                                    txttotalSumInsured.Text = Val(oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("TOTAL_ELEC_SI").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("TOTAL_NONELEC_SI") Then
                                    txtSumInsurNonelec.Text = Val(oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("TOTAL_NONELEC_SI").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("DRIVER_DETAILS_INC") Then
                                    chkincludeDriverDetails.Checked = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("DRIVER_DETAILS_INC").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("TRAILER") Then
                                    ChkTrailer.Checked = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("TRAILER").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("TRAILER_REGISTRATION_NUMBER") Then
                                    txtTrailersRegisterNumber.Text = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("TRAILER_REGISTRATION_NUMBER").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("NUMBER_OF_TRAILERS") Then
                                    txtNumberoftrailers.Text = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("NUMBER_OF_TRAILERS").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("TRAILER_IDV") Then
                                    txtTrailersIDV.Text = oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("TRAILER_IDV").Value
                                End If
                                'Cover Note
                                ' for grid binding of the Driver details 

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasChildNodes Then
                                    Dim oElement_COVERSELECTION As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("COVERSELECTION").SelectNodes("DRIVERDETAILS")
                                    Dim str As String = Nothing
                                    If oElement_COVERSELECTION.Count > 0 Then
                                        str = "<DATASET>"


                                        For iCnt As Integer = 0 To oElement_COVERSELECTION.Count - 1
                                            str &= "<" & oElement_COVERSELECTION(0).Name & " "
                                            For icount As Integer = 0 To oElement_COVERSELECTION.Item(iCnt).Attributes.Count - 1
                                                If oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "US" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_COVERSELECTION_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_DRIVERDETAILS_ID" Then

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
                                        For iCount As Integer = 0 To ds.Tables(0).Rows.Count - 1

                                            ds.Tables(0).Rows(iCount)("VEHICLE_DRIVEN_BY") = ddlvehicledrivenby.Items.FindByValue(ds.Tables(0).Rows(iCount)("VEHICLE_DRIVEN_BY").ToString()).Text
                                            ds.Tables(0).Rows(iCount)("GENDER") = ddlGender.Items.FindByValue(ds.Tables(0).Rows(iCount)("GENDER").ToString()).Text
                                            ds.Tables(0).Rows(iCount)("RELATIONSHIP_WITH_OWNER") = ddlRelationshipwithowner.Items.FindByValue(ds.Tables(0).Rows(iCount)("RELATIONSHIP_WITH_OWNER").ToString()).Text
                                            ds.Tables(0).Rows(iCount)("TYPE_OF_LICENCE") = ddlTypeoflicence.Items.FindByValue(ds.Tables(0).Rows(iCount)("TYPE_OF_LICENCE").ToString()).Text

                                        Next
                                        Session("DriverDetails") = ds.Tables(0)
                                        gvDriverDetails.DataSource = ds
                                        gvDriverDetails.DataBind()

                                        pnlDriverdetails1.Visible = True


                                    End If
                                End If
                                'For Grid binding of Claim History

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasChildNodes Then
                                    Dim oElement_COVERSELECTION As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("COVERSELECTION").SelectNodes("CLAIMSHISTORY")
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
                                'For Grid Binding of Electrical

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasChildNodes Then
                                    Dim oElement_COVERSELECTION As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("COVERSELECTION").SelectNodes("ELECTRICAL")
                                    Dim str As String = Nothing
                                    If oElement_COVERSELECTION.Count > 0 Then
                                        str = "<DATASET>"

                                        For iCnt As Integer = 0 To oElement_COVERSELECTION.Count - 1
                                            str &= "<" & oElement_COVERSELECTION(0).Name & " "
                                            For icount As Integer = 0 To oElement_COVERSELECTION.Item(iCnt).Attributes.Count - 1
                                                If oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "US" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_ELECTRICAL_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_COVERSELECTION_ID" Then

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

                                        Session("Electricaldetails") = ds.Tables(0)
                                        gvElectricalaccessories.DataSource = ds
                                        gvElectricalaccessories.DataBind()


                                    End If



                                End If

                                'For Grid Binding of Non Electrical 

                                If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasChildNodes Then
                                    Dim oElement_COVERSELECTION As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("COVERSELECTION").SelectNodes("NONELECTRICAL")
                                    Dim str As String = Nothing
                                    If oElement_COVERSELECTION.Count > 0 Then
                                        str = "<DATASET>"
                                        For iCnt As Integer = 0 To oElement_COVERSELECTION.Count - 1
                                            str &= "<" & oElement_COVERSELECTION(0).Name & " "
                                            For icount As Integer = 0 To oElement_COVERSELECTION.Item(iCnt).Attributes.Count - 1
                                                If oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "US" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_NONELECTRICAL_ID" Or oElement_COVERSELECTION.Item(iCnt).Attributes(icount).Name = "MOTOR_COVERSELECTION_ID" Then

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
                                        Session("NonElectricaldetails") = ds.Tables(0)
                                        gvNonElecTrical.DataSource = ds
                                        gvNonElecTrical.DataBind()
                                        pnlNonElecTrical1.Visible = True

                                    End If
                                    pnlNonelectrical.Visible = False
                                End If
                                '/* cover selection End

                                '/* strat Previous Insurance

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("POLICY_NUMBER") Then
                                    txtPolicyNumber.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("POLICY_NUMBER").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("POLICY_TYPE") Then
                                    ddlPloicyType.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("POLICY_TYPE").Value).Selected = True
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("POLICY_EXPIRY_DATE") Then
                                    txtPreviousPolicyExpiryDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("POLICY_EXPIRY_DATE").Value)
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("PREVIOUS_YEAR_NCB") Then
                                    DDLPreviousYearNCB.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("PREVIOUS_YEAR_NCB").Value).Selected = True
                                End If



                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("DOCUMENT_PROOF") Then
                                    txtDocProof.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("DOCUMENT_PROOF").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("PREVIOUS_INSURER_NAME") Then
                                    txtInsuredName.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("PREVIOUS_INSURER_NAME").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("INSURER_ADD1") Then
                                    InsAddLine1.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("INSURER_ADD1").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("INSURER_ADD2") Then
                                    InsAddLine2.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("INSURER_ADD2").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("INSURER_CITY") Then
                                    InsAddCity.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("INSURER_CITY").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("INSURER_STATE") Then
                                    InsAddState.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("INSURER_STATE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("INSURER_PINCODE") Then
                                    InsAddPinCode.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("INSURER_PINCODE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("POLICY_EXPIRY_DATE_NCB") Then
                                    txtPolicyExpiryDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("POLICY_EXPIRY_DATE_NCB").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("VEHICLE_INSPECTIONDATE") Then
                                    txtInspectionDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("VEHICLE_INSPECTIONDATE").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("VEHICLE_INSPECTEDBY") Then
                                    txtInspectedByWhom.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("VEHICLE_INSPECTEDBY").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("VEHICLE_INSPECTIONREPORT") Then
                                    txtInspectionReport.Text = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("VEHICLE_INSPECTIONREPORT").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("PREVIOUSINSURANCEHISTORYINC") Then
                                    chkIsPreviousInsuranceHistory.Checked = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("PREVIOUSINSURANCEHISTORYINC").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasAttribute("VEHICLEINSPECTIONINC") Then
                                    chkVehicleInspection.Checked = oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").Attributes("VEHICLEINSPECTIONINC").Value
                                End If

                                'Start Chid PI history
                                If oElement_MOTOR_POLICY_BINDER("PREVIOUSINSURANCE").HasChildNodes Then
                                    Dim oElement_PREVIOUSINSURANCE As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("PREVIOUSINSURANCE").SelectNodes("PIHISTORY")
                                    Dim str As String = Nothing
                                    If oElement_PREVIOUSINSURANCE.Count > 0 Then
                                        str = "<DATASET>"

                                        For iCnt As Integer = 0 To oElement_PREVIOUSINSURANCE.Count - 1
                                            str &= "<" & oElement_PREVIOUSINSURANCE(0).Name & " "
                                            For icount As Integer = 0 To oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes.Count - 1
                                                If oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes(icount).Name = "US" Or oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes(icount).Name = "MOTOR_PREVIOUSINSURANCE_ID" Or oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes(icount).Name = "MOTOR_PIHISTORY_ID" Then

                                                Else

                                                    str &= oElement_PREVIOUSINSURANCE.Item(iCnt).Attributes(icount).OuterXml & " "
                                                End If
                                            Next

                                            str &= "/>"
                                        Next


                                        str &= "</DATASET>"
                                        Dim sr As New System.IO.StringReader(str)

                                        Dim ds As New DataSet
                                        ds.ReadXml(sr)

                                        ' ds.Tables(0).Columns(0).ColumnMapping = MappingType.Hidden
                                        'ds.Tables(0).Columns(1).ColumnMapping = MappingType.Hidden
                                        Session("PersonalInsuranceHistory") = ds.Tables(0)
                                        gvPreviousInsurancehistory.DataSource = ds
                                        gvPreviousInsurancehistory.DataBind()
                                        PreviousInsurancehistory1.Visible = True
                                    End If

                                End If
                                'End Chid  PI history

                                '/*End Previous Insurance

                                '//Start  PERSONAL ACCIDENT
                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_UN_NAMED") Then
                                    chkPersonalaccidentcoverforunnamedpersons.Checked = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_UN_NAMED").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_UN_NAMED_NUMBER") Then
                                    txtNumberOfunnamedPersons.Text = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_UN_NAMED_NUMBER").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_UN_NAMED_SUMINSURED") Then
                                    txtSumInsuredPerPerson.Text = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_UN_NAMED_SUMINSURED").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_PAID_DRIVER_CLEANER") Then
                                    chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner.Checked = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_PAID_DRIVER_CLEANER").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_NUMBER") Then
                                    txtNumberOfpaiddrivers.Text = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_NUMBER").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_SUM_INSURED") Then
                                    txtSumInsured2.Text = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_SUM_INSURED").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_NAMED_PERSON") Then
                                    chkDoyouwishtotakepersonalaccidentcoverfornamedpersons.Checked = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_NAMED_PERSON").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("PA_TOTAL_SI") Then
                                    txtTotalsuminsuredofallnamedpersons.Text = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("PA_TOTAL_SI").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("IS_COMP_PA") Then
                                    chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements.Checked = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("IS_COMP_PA").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("COMP_PA_SI") Then
                                    txtSumInsured1.Text = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("COMP_PA_SI").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasAttribute("IS_ONLY_CAR") Then
                                    chkThisvechcle.Checked = oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").Attributes("IS_ONLY_CAR").Value
                                End If


                                'Start Child Personal accient Details
                                If oElement_MOTOR_POLICY_BINDER("PERSONALACCIDENT").HasChildNodes Then
                                    Dim oElement_PERSONALACCIDENT As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("PERSONALACCIDENT").SelectNodes("PADETAILS")
                                    Dim str As String = Nothing
                                    If oElement_PERSONALACCIDENT.Count > 0 Then
                                        str = "<DATASET>"
                                        For iCnt As Integer = 0 To oElement_PERSONALACCIDENT.Count - 1
                                            str &= "<" & oElement_PERSONALACCIDENT(0).Name & " "
                                            For icount As Integer = 0 To oElement_PERSONALACCIDENT.Item(iCnt).Attributes.Count - 1
                                                If oElement_PERSONALACCIDENT.Item(iCnt).Attributes(icount).Name = "US" Or oElement_PERSONALACCIDENT.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_PERSONALACCIDENT.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_PERSONALACCIDENT.Item(iCnt).Attributes(icount).Name = "MOTOR_PERSONALACCIDENT_ID" Or oElement_PERSONALACCIDENT.Item(iCnt).Attributes(icount).Name = "MOTOR_PADETAILS_ID" Then

                                                Else
                                                    str &= oElement_PERSONALACCIDENT.Item(iCnt).Attributes(icount).OuterXml & " "
                                                End If
                                            Next
                                            str &= "/>"
                                        Next

                                        str &= "</DATASET>"
                                        Dim sr As New System.IO.StringReader(str)

                                        Dim ds As New DataSet
                                        ds.ReadXml(sr)

                                        gvpersonalAccidentDetails.DataSource = ds
                                        gvpersonalAccidentDetails.DataBind()
                                        Session("PersonalAccident") = ds.Tables(0)
                                    End If

                                End If
                                'End Chid  Personal accient Details
                                '//End PERSONALACCIDENT


                                'Start Chid FINANCIERDETAILS
                                If oElement_MOTOR_POLICY_BINDER("FINANCIER").HasChildNodes Then
                                    Dim oElement_FINANCIERDETAILS As System.Xml.XmlNodeList = xmlDoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER").SelectSingleNode("FINANCIER").SelectNodes("FINANCIERDETAILS")
                                    Dim str As String = Nothing
                                    If oElement_FINANCIERDETAILS.Count > 0 Then
                                        str = "<DATASET>"
                                        For iCnt As Integer = 0 To oElement_FINANCIERDETAILS.Count - 1
                                            str &= "<" & oElement_FINANCIERDETAILS(0).Name & " "
                                            For icount As Integer = 0 To oElement_FINANCIERDETAILS.Item(iCnt).Attributes.Count - 1
                                                If oElement_FINANCIERDETAILS.Item(iCnt).Attributes(icount).Name = "US" Or oElement_FINANCIERDETAILS.Item(iCnt).Attributes(icount).Name = "OI" Or oElement_FINANCIERDETAILS.Item(iCnt).Attributes(icount).Name = "MOTOR_POLICY_BINDER_ID" Or oElement_FINANCIERDETAILS.Item(iCnt).Attributes(icount).Name = "MOTOR_FINANCIER_ID" Or oElement_FINANCIERDETAILS.Item(iCnt).Attributes(icount).Name = "MOTOR_FINANCIERDETAILS_ID" Then

                                                Else
                                                    str &= oElement_FINANCIERDETAILS.Item(iCnt).Attributes(icount).OuterXml & " "
                                                End If
                                            Next
                                            str &= "/>"
                                        Next

                                        str &= "</DATASET>"
                                        Dim sr As New System.IO.StringReader(str)

                                        Dim ds As New DataSet
                                        ds.ReadXml(sr)

                                        For iCount As Integer = 0 To ds.Tables(0).Rows.Count - 1
                                            'gvFinancierDetails.Rows(iCount).Cells(2).Text = ddlagreementType.Items.FindByValue(gvFinancierDetails.Rows(iCount).Cells(2).Text).Text
                                            'gvFinancierDetails.Rows(iCount).Cells(3).Text = ddlFinancerType.Items.FindByValue(gvFinancierDetails.Rows(iCount).Cells(3).Text).Text
                                            ds.Tables(0).Rows(iCount)("AGREEMENT_TYPE") = ddlagreementType.Items.FindByValue(ds.Tables(0).Rows(iCount)("AGREEMENT_TYPE").ToString()).Text
                                            ds.Tables(0).Rows(iCount)("FINANCIER_TYPE") = ddlFinancerType.Items.FindByValue(ds.Tables(0).Rows(iCount)("FINANCIER_TYPE").ToString()).Text

                                        Next

                                        pnlFinancierDetails1.Visible = True

                                        gvFinancierDetails.DataSource = ds
                                        gvFinancierDetails.DataBind()


                                        Session("FinancierDetails") = ds.Tables(0)
                                    End If

                                End If
                                'End Chid  FINANCIERDETAILS


                                '//Start Vechicle Details
                                'If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEHICLE_MAKE") Then
                                '    ddlVehicleMake.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEHICLE_MAKE").Value).Selected = True
                                'End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEHICLE_MODEL") Then
                                    txtVehicleModel.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEHICLE_MODEL").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("BODY_TYPE") Then
                                    txtBodyType.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("BODY_TYPE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("SEATING_CAPACITY") Then
                                    txtSeatingcapacity.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("SEATING_CAPACITY").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("YEAR_MANUFACTURING") Then
                                    txtYearManufacture.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("YEAR_MANUFACTURING").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("ENGINE_NUMBER") Then
                                    txtEngineNumber.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("ENGINE_NUMBER").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("CHASSIS_NUMBER") Then
                                    txtChasisNumber.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("CHASSIS_NUMBER").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEH_REG_ADD1") Then
                                    txtLine1.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEH_REG_ADD1").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEH_REG_ADD2") Then
                                    txtLine2.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEH_REG_ADD2").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEH_REG_CITY") Then
                                    txtCity.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEH_REG_CITY").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("YEAR_MANUFACTURING") Then
                                    txtYearManufacture.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("YEAR_MANUFACTURING").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEH_REG_STATE") Then
                                    txtState.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEH_REG_STATE").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEH_REG_PINCODE") Then
                                    txtPinCode.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEH_REG_PINCODE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("CUBIC_CAPACITY") Then
                                    txtCC.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("CUBIC_CAPACITY").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEH_REG_PINCODE") Then
                                    txtPinCode.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEH_REG_PINCODE").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEHICLE_COLOR") Then
                                    txtVehicleColor.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEHICLE_COLOR").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("EX_SHOWROOM_PRICE") Then
                                    txtExShowRoom.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("EX_SHOWROOM_PRICE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("VEHICLE_ZONE") Then
                                    txtZone.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("VEHICLE_ZONE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("SYSTEM_GENERATED_IDV") Then
                                    txtIDVSystem.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("SYSTEM_GENERATED_IDV").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("DATE_OF_PURCHASE") Then
                                    ddlDateOf.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("DATE_OF_PURCHASE").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("INSURED_DECLARED_VALUE") Then
                                    txtIDV.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("INSURED_DECLARED_VALUE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("DATE_FIRST_REGISTRATION") Then
                                    txtDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("DATE_FIRST_REGISTRATION").Value)
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("NEW_VEHICLE") Then
                                    chkNewVehicle.Checked = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("NEW_VEHICLE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("REG_1") Then
                                    txtReg1.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("REG_1").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("REG_2") Then
                                    txtReg2.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("REG_2").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("REG_3") Then
                                    txtReg3.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("REG_3").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("REG_4") Then
                                    txtReg4.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("REG_4").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("RTO_MASTER") Then
                                    txtRTOLocation.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("RTO_MASTER").Value
                                End If

                                '//End Vechicle Details

                                '/Start Vechicle Usage Deatils

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").HasAttribute("VEHICLE_PARKING") Then
                                    ddlVechicleParked.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").Attributes("VEHICLE_PARKING").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").HasAttribute("MAIN_USAGE_OF_VEHICLE") Then
                                    ddlVechiclemainlyused.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").Attributes("MAIN_USAGE_OF_VEHICLE").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").HasAttribute("TYPE_OF_ROADS") Then
                                    ddlusuallydriven.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").Attributes("TYPE_OF_ROADS").Value).Selected = True
                                End If

                                If oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").HasAttribute("DISTANCE_DRIVEN_ANNUALLY") Then
                                    txtApproximatedistance.Text = oElement_MOTOR_POLICY_BINDER("VEHICLEUSAGEDETAILS").Attributes("DISTANCE_DRIVEN_ANNUALLY").Value

                                End If
                                If oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").HasAttribute("NEW_VEHICLE") Then
                                    chkNewVehicle.Checked = oElement_MOTOR_POLICY_BINDER("VEHICLEDETAILS").Attributes("NEW_VEHICLE").Value
                                End If
                                '/End Vechicle Usage Deatils


                                '/Start additional details
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("BI_FUEL_KIT_INBUILT") Then
                                    chkBifuel.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("BI_FUEL_KIT_INBUILT").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("LPG_CNG") Then
                                    chkLPGkit.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("LPG_CNG").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("LPG_CNG_SUM_INSURED") Then
                                    txtLPGSI.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("LPG_CNG_SUM_INSURED").Value

                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("NON_CONVENTIONAL_SOF") Then
                                    chkNonConvectionalPower.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("NON_CONVENTIONAL_SOF").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("DESC_NON_CONVENTIONAL_SOF") Then
                                    txtDescription.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("DESC_NON_CONVENTIONAL_SOF").Value

                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("ANTITHEFT_DEVICE") Then
                                    chkAntitheft.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("ANTITHEFT_DEVICE").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("HANDICAPPED_USER") Then
                                    chkIshandicapped.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("HANDICAPPED_USER").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("FOREIGN_EMBASSY") Then
                                    chkForiegnEmbassy.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("FOREIGN_EMBASSY").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("FIBRE_FUEL_TANK") Then
                                    chkFiberGlassFuelTank.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("FIBRE_FUEL_TANK").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("DRIVING_TUITIONS") Then
                                    chkIsvehicleusedfordrivingtutions.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("DRIVING_TUITIONS").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("LIMITED_TO_OWN_PREMISES") Then
                                    chkOwnprmesis.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("LIMITED_TO_OWN_PREMISES").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("VINTAGE_CAR") Then
                                    chkcertifiedVintage.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("VINTAGE_CAR").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("EXT_OF_GEO_AREA") Then
                                    chkGeographical.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("EXT_OF_GEO_AREA").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("VINTAGE_CAR") Then
                                    chkcertifiedVintage.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("VINTAGE_CAR").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("BANGLADESH") Then
                                    chkBangaldesh.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("BANGLADESH").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("BHUTAN") Then
                                    chkBhutan.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("BHUTAN").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("MALDIVES") Then
                                    chkMaldives.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("MALDIVES").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("NEPAL") Then
                                    chkNepal.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("NEPAL").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("PAKISTAN") Then
                                    chkPakistan.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("PAKISTAN").Value
                                End If
                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("SRILANKA") Then
                                    chkSrilanka.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("SRILANKA").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("RALLIES") Then
                                    chkVechileusedforrallies.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("RALLIES").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("START_DATE") Then
                                    txtStartDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("START_DATE").Value)

                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("END_DATE") Then
                                    txtEndDate.Text = FormatDDMMYY(oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("END_DATE").Value)

                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("RELIABILITY_TRIALS") Then
                                    chkReliabilitytrails.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("RELIABILITY_TRIALS").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("VOLUNTARY_EXCESS") Then
                                    chkVoluntaryDeductible.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("VOLUNTARY_EXCESS").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("VOLUNTARY_EXCESS_AMOUNT") Then
                                    ddlVoluntaryExcessamount.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("VOLUNTARY_EXCESS_AMOUNT").Value).Selected = True
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("IMPOSED_EXCESS") Then
                                    txtImposedExcess.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("IMPOSED_EXCESS").Value

                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("COMPULSORY_EXCESS") Then
                                    txtImposedExcess.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("COMPULSORY_EXCESS").Value

                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("CNG_KIT") Then
                                    chkCNGSI.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("CNG_KIT").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("CNG_SUM_INSURED") Then
                                    txtCNGSI.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("CNG_SUM_INSURED").Value

                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("AGENCY_COMMISSIONIND") Then
                                    chkBrokerageAgencyCommission.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("AGENCY_COMMISSIONIND").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("AGENCY_COMMISSION") Then
                                    txtDiscountAgentCommission.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("AGENCY_COMMISSION").Value

                                End If

                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("GOOD_FEATURE_DISCOUNTIND") Then
                                    chkGoodfeatureDiscount.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("GOOD_FEATURE_DISCOUNTIND").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("GOOD_FEATURE_DISCOUNT") Then
                                    txtDiscountGoodfeature.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("GOOD_FEATURE_DISCOUNT").Value

                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("SPECIAL_DISCOUNTIND") Then
                                    chkSpecialDiscount.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("SPECIAL_DISCOUNTIND").Value
                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("SPECIAL_DISCOUNT") Then
                                    txtDiscountSpecial.Text = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("SPECIAL_DISCOUNT").Value

                                End If


                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("RTA_PERMISSION_FOR_NCSOP") Then
                                    chkPermissionRTA.Checked = oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("RTA_PERMISSION_FOR_NCSOP").Value
                                End If



                                If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").HasAttribute("DETARRIFF_DISCOUNT") Then

                                    If oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("DETARRIFF_DISCOUNT").Value <> "0" Then
                                        ddlDetarriffDiscount.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("ADDITIONALCOVER").Attributes("DETARRIFF_DISCOUNT").Value).Selected = True
                                    End If
                                End If

                                '/End additional details

                                'Start LegalLiability
                                If oElement_MOTOR_POLICY_BINDER("LEGALLIABILITY").HasAttribute("WORKMEN_FOR_OPS_MAINTENANCE") Then
                                    txtNumberOfPaiddrivesCleanersWorkMan.Text = oElement_MOTOR_POLICY_BINDER("LEGALLIABILITY").Attributes("WORKMEN_FOR_OPS_MAINTENANCE").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("LEGALLIABILITY").HasAttribute("NO_OF_EMP_TRAVELLING_DRIVING") Then
                                    txtNumberOFEmployeeTravellingDriving.Text = oElement_MOTOR_POLICY_BINDER("LEGALLIABILITY").Attributes("NO_OF_EMP_TRAVELLING_DRIVING").Value
                                End If

                                If oElement_MOTOR_POLICY_BINDER("LEGALLIABILITY").HasAttribute("RESTRICTED_TPPD") Then
                                    chkDoyouwishtorestrictTPPDCovertostatutoryLimitofRs6000.Checked = oElement_MOTOR_POLICY_BINDER("LEGALLIABILITY").Attributes("RESTRICTED_TPPD").Value
                                End If

                                'End Legaliability


                            End If

                        End If
                    End If


                End With
                If Session("QUICKQUOTE") = "1" Then
                    BuildQuickQuote(xmlDoc)
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
            .QuoteTimeStamp = Session("QuoteTimeStamp")
        End With

        Try
            oDeleteRiskResponseType = oSAM.DeleteRisk(oDeleteRiskRequestType)


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
            Dim blnNewRow As Boolean
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

                    oElementToAddOrUpdate = Nothing
                    If xmlDatasetElement.ElementName = "PIHISTORY" Then

                        Dim itemCount As Integer = Nodeindex.Get("PIHISTORY")

                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("PREVIOUSINSURANCE") _
                            .SelectNodes("PIHISTORY").Item(itemCount)
                        If oElementToAddOrUpdate IsNot Nothing Then
                            Dim NewrowCount As Integer = gvPreviousInsurancehistory.Rows.Count - oElementToAddOrUpdate.ParentNode.ChildNodes.Count

                            'deleted record
                            If oElementToAddOrUpdate.Attributes("US").Value = "3" Then
                                itemCount = itemCount + 1
                                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                               .SelectSingleNode("RISK_OBJECTS") _
                               .SelectSingleNode("MOTOR_POLICY_BINDER") _
                               .SelectSingleNode("PREVIOUSINSURANCE") _
                               .SelectNodes("PIHISTORY").Item(itemCount)
                            End If

                            If NewrowCount > 0 Then
                                blnNewRow = True
                            Else
                                blnNewRow = False
                            End If

                            itemCount = itemCount + 1
                            Nodeindex.Set("PIHISTORY", itemCount.ToString())
                        End If

                    ElseIf xmlDatasetElement.ElementName = "CLAIMSHISTORY" Then
                        Dim itemCount As Integer = Nodeindex.Get("CLAIMHISTORY")
                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("COVERSELECTION") _
                            .SelectNodes("CLAIMSHISTORY").Item(itemCount)

                        itemCount = itemCount + 1
                        Nodeindex.Set("CLAIMHISTORY", itemCount.ToString())


                    ElseIf xmlDatasetElement.ElementName = "DRIVERDETAILS" Then
                        Dim itemCount As Integer = Nodeindex.Get("DRIVERDETAILS")
                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("COVERSELECTION") _
                            .SelectNodes("DRIVERDETAILS").Item(itemCount)
                        If oElementToAddOrUpdate IsNot Nothing Then
                            Dim NewrowCount As Integer = gvDriverDetails.Rows.Count - oElementToAddOrUpdate.ParentNode.ChildNodes.Count

                            'deleted record
                            If oElementToAddOrUpdate.Attributes("US").Value = "3" Then
                                itemCount = itemCount + 1
                                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                    .SelectSingleNode("COVERSELECTION") _
                                    .SelectNodes("DRIVERDETAILS").Item(itemCount)

                            End If

                            If NewrowCount > 0 Then
                                blnNewRow = True
                            Else
                                blnNewRow = False
                            End If

                            itemCount = itemCount + 1
                            Nodeindex.Set("DRIVERDETAILS", itemCount.ToString())
                        End If
                    ElseIf xmlDatasetElement.ElementName = "NONELECTRICAL" Then
                        Dim itemCount As Integer = Nodeindex.Get("NONELECTRICAL")
                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("COVERSELECTION") _
                            .SelectNodes("NONELECTRICAL").Item(itemCount)
                        If oElementToAddOrUpdate IsNot Nothing Then


                            Dim NewrowCount As Integer = gvNonElecTrical.Rows.Count - oElementToAddOrUpdate.ParentNode.ChildNodes.Count

                            'deleted record
                            If oElementToAddOrUpdate.Attributes("US").Value = "3" Then
                                itemCount = itemCount + 1
                                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                     .SelectSingleNode("RISK_OBJECTS") _
                                     .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                     .SelectSingleNode("COVERSELECTION") _
                                     .SelectNodes("NONELECTRICAL").Item(itemCount)

                            End If

                            If NewrowCount > 0 Then
                                blnNewRow = True
                            Else
                                blnNewRow = False
                            End If
                            itemCount = itemCount + 1
                            Nodeindex.Set("NONELECTRICAL", itemCount.ToString())
                        End If

                    ElseIf xmlDatasetElement.ElementName = "ELECTRICAL" Then
                        Dim itemCount As Integer = Nodeindex.Get("ELECTRICAL")
                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("COVERSELECTION") _
                            .SelectNodes("ELECTRICAL").Item(itemCount)
                        If oElementToAddOrUpdate IsNot Nothing Then
                            Dim NewrowCount As Integer = gvElectricalaccessories.Rows.Count - oElementToAddOrUpdate.ParentNode.ChildNodes.Count

                            'deleted record
                            If oElementToAddOrUpdate.Attributes("US").Value = "3" Then
                                itemCount = itemCount + 1
                                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("COVERSELECTION") _
                                .SelectNodes("ELECTRICAL").Item(itemCount)
                            End If

                            If NewrowCount > 0 Then
                                blnNewRow = True
                            Else
                                blnNewRow = False
                            End If

                            itemCount = itemCount + 1
                            Nodeindex.Set("ELECTRICAL", itemCount.ToString())
                        End If
                    ElseIf xmlDatasetElement.ElementName = "PADETAILS" Then
                        Dim itemCount As Integer = Nodeindex.Get("PADETAILS")
                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("PERSONALACCIDENT") _
                            .SelectNodes("PADETAILS").Item(itemCount)
                        If oElementToAddOrUpdate IsNot Nothing Then
                            Dim NewrowCount As Integer = gvpersonalAccidentDetails.Rows.Count - oElementToAddOrUpdate.ParentNode.ChildNodes.Count

                            'deleted record
                            If oElementToAddOrUpdate.Attributes("US").Value = "3" Then
                                itemCount = itemCount + 1
                                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                    .SelectSingleNode("PERSONALACCIDENT") _
                                    .SelectNodes("PADETAILS").Item(itemCount)

                            End If

                            If NewrowCount > 0 Then
                                blnNewRow = True
                            Else
                                blnNewRow = False
                            End If

                            itemCount = itemCount + 1
                            Nodeindex.Set("PADETAILS", itemCount.ToString())

                        End If
                    ElseIf xmlDatasetElement.ElementName = "FINANCIERDETAILS" Then
                        Dim itemCount As Integer = Nodeindex.Get("FINANCIERDETAILS")


                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                            .SelectSingleNode("RISK_OBJECTS") _
                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            .SelectSingleNode("FINANCIER") _
                            .SelectNodes("FINANCIERDETAILS").Item(itemCount)
                        If oElementToAddOrUpdate IsNot Nothing Then

                            Dim NewrowCount As Integer
                            If oElementToAddOrUpdate.ParentNode.HasChildNodes Then
                                NewrowCount = gvFinancierDetails.Rows.Count - oElementToAddOrUpdate.ParentNode.ChildNodes.Count
                            Else
                                NewrowCount = 0
                            End If


                            'deleted record
                            If oElementToAddOrUpdate.Attributes("US").Value = "3" Then
                                itemCount = itemCount + 1
                                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                .SelectSingleNode("RISK_OBJECTS") _
                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                .SelectSingleNode("FINANCIER") _
                                .SelectNodes("FINANCIERDETAILS").Item(itemCount)

                            End If

                            If NewrowCount > 0 Then
                                blnNewRow = True
                            Else
                                blnNewRow = False
                            End If

                            'Dim strIndex As String = oElementToAddOrUpdate.ParentNode.ChildNodes.Item(oElementToAddOrUpdate.ParentNode.ChildNodes.Count - 1).Attributes("OI").Value
                            'strIndex = strIndex.Remove(0, 2)
                            'Dim IndexCount As Integer = Convert.ToInt32(strIndex) + 1
                            'oElementToAddOrUpdate.Attributes("OI").Value = "OI" & strIndex
                            'oElementToAddOrUpdate.Attributes("US").Value = "1"

                            itemCount = itemCount + 1
                            Nodeindex.Set("FINANCIERDETAILS", itemCount.ToString())
                        End If
                    Else

                        oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                            .SelectSingleNode("RISK_OBJECTS") _
                                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                            .SelectSingleNode(xmlDatasetElement.ElementName)
                    End If
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

                    ElseIf blnNewRow Then
                        'BuildXMLFORNEWRECORD(xmlDatasetElementsToAdd)
                        BuildXMLFORNEWRECORD(xmlDatasetElement)

                        'For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        '    newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        '    'for maintain sequence in the OI index
                        '    If XMLAttribute.AttributeName = "OI" Then
                        '        newAttribute.Value = oElementToAddOrUpdate.ChildNodes.Item(oElementToAddOrUpdate.ParentNode.ChildNodes.Count - 1).Attributes("OI").Value + 1
                        '        newAttribute.Value = XMLAttribute.AttributeValue
                        '    End If
                        '    oElementToAddOrUpdate.Attributes.Append(newAttribute)
                        'Next
                        'bNewElement = True
                    Else
                        ' Update the Update
                        ' Status (US) attribute

                        If oElementToAddOrUpdate.Attributes("US").Value <> "3" Then
                            oElementToAddOrUpdate.Attributes("US").Value = "2"
                            For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                                newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                                newAttribute.Value = XMLAttribute.AttributeValue
                                oElementToAddOrUpdate.Attributes.Append(newAttribute)
                            Next
                        End If

                        'End If
                        bNewElement = False
                    End If

                    'for newly added record in a grid
                    'If blnNewRow Then
                    '    oElementToAddOrUpdate.Attributes("US").Value = "1"
                    '    oElementToAddOrUpdate.Attributes("OI").Value = oElementToAddOrUpdate.ParentNode.ChildNodes.Item(oElementToAddOrUpdate.ParentNode.ChildNodes.Count - 1).Attributes("OI").Value + 1
                    '    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                    '        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                    '        'for maintain sequence in the OI index
                    '        If XMLAttribute.AttributeName = "OI" Then
                    '            newAttribute.Value = oElementToAddOrUpdate.ChildNodes.Item(oElementToAddOrUpdate.ParentNode.ChildNodes.Count - 1).Attributes("OI").Value + 1

                    '            'for maintain the Added flag for US
                    '        ElseIf XMLAttribute.AttributeName = "US" Then
                    '            newAttribute.Value = "1"
                    '        Else
                    '            newAttribute.Value = XMLAttribute.AttributeValue
                    '        End If

                    '        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    '    Next
                    '    bNewElement = True
                    'End If

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

                            'oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                            '    .SelectSingleNode("RISK_OBJECTS") _
                            '    .SelectSingleNode("MOTOR_POLICY_BINDER") _
                            '    .SelectSingleNode("FINANCIER") _
                            '    .PrependChild(oElementToAddOrUpdate)
                        Else

                            oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                                    .SelectSingleNode("RISK_OBJECTS") _
                                                    .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                                    .AppendChild(oElementToAddOrUpdate)
                        End If

                    End If
                End If

                'End If
            Next

            'For Newly Added Record with existing some records.

            'BuildXMLFORNEWRECORD(xmlDatasetElementsToAdd)

            ' Write back the next OI number to the DATA_SET node
            nNextOINumber -= 1
            xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value = nNextOINumber

            With oRequest
                oRequest.TransactionType = "MTA"
                .RiskKey = Session("RiskKey")
                .XMLDataSet = xmlDoc.OuterXml
                .QuoteTimeStamp = Session("QuoteTimeStamp")


                .BranchCode = "Headoff"
                .ScreenCode = "MOTPC"
                .RiskDescription = "Motor private Car"



                'add values from the quote we just added
                .InsuranceFolderKey = Session("InsuranceFolderKey")
                .InsuranceFileKey = Session("InsuranceFileKey")
                '#Prakash: If process is renewal, set transaction type to REN
                If (Session("Process") = "REN") Then
                    .TransactionType = "REN"
                End If
            End With

            oResponse = oSAM.UpdateRisk(oRequest)

            m_sRiskDataXML = oResponse.XMLDataSet
            Session("QuoteTimeStamp") = oResponse.QuoteTimeStamp

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
        oXMLAttr(6).AttributeValue = InsAddLine1.Text

        oXMLAttr(7) = New XMLAttributeToAdd
        oXMLAttr(7).AttributeName = "INSURER_ADD2"
        oXMLAttr(7).AttributeValue = InsAddLine2.Text

        oXMLAttr(8) = New XMLAttributeToAdd
        oXMLAttr(8).AttributeName = "INSURER_CITY"
        oXMLAttr(8).AttributeValue = InsAddCity.Text

        oXMLAttr(9) = New XMLAttributeToAdd
        oXMLAttr(9).AttributeName = "INSURER_STATE"
        oXMLAttr(9).AttributeValue = InsAddState.Text

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
        oXMLAttr(4).AttributeName = "PRODUCT_SERIAL_NUMBER"
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


#Region "commented code"
    'Protected Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
    '    Try
    '        UserToken = GetUserToken("sirius", "sirius")
    '        oSAM.SetClientCredential(UserToken)
    '        oSAM.SetPolicy("SamClientPolicy")

    '        Dim xmlDatasetElementsToAdd(13) As XMLElementToAdd


    '        '****Start View of 1- CoverSelection*****

    '        'CoverNote
    '        Dim oCoverSelXmlElement As XMLElementToAdd = Nothing
    '        CoverNote(oCoverSelXmlElement)
    '        xmlDatasetElementsToAdd(0) = oCoverSelXmlElement

    '        'VehicleDetails

    '        Dim oVehicleDetailXmlElement As XMLElementToAdd = Nothing
    '        VehicleDetails(oVehicleDetailXmlElement)
    '        xmlDatasetElementsToAdd(1) = oVehicleDetailXmlElement


    '        'FinancierDetails
    '        Dim oFinancierDetailXmlElement As XMLElementToAdd = Nothing
    '        FinancierDetails(oFinancierDetailXmlElement)
    '        xmlDatasetElementsToAdd(11) = oFinancierDetailXmlElement

    '        '******End View of 1- CoverSelection*******


    '        '******Start view 2- Additional Cover

    '        'DriverDetails

    '        If gvDriverDetails IsNot Nothing AndAlso gvDriverDetails.Rows.Count > 0 Then
    '            Dim oDriverDetailXmlElement As XMLElementToAdd = Nothing
    '            DriverDetails(oDriverDetailXmlElement)
    '            xmlDatasetElementsToAdd(4) = oDriverDetailXmlElement

    '        End If

    '        '******End view 2- Additional Cover


    '        '******Start view 3- Additional Cover continue..

    '        'VehicleUsage
    '        Dim oVehicleUsageXmlElement As XMLElementToAdd = Nothing
    '        VehicleUsage(oVehicleUsageXmlElement)
    '        xmlDatasetElementsToAdd(3) = oVehicleUsageXmlElement

    '        'Additional Cover Details
    '        Dim oAddtionalCoverDetailsXmlElement As XMLElementToAdd = Nothing
    '        AdditionalCoverDetails(oAddtionalCoverDetailsXmlElement)
    '        xmlDatasetElementsToAdd(2) = oAddtionalCoverDetailsXmlElement

    '        '******End view 3- Additional Cover continue..


    '        '****Start view of 4- PreviousInsurance*****
    '        Dim oPreviousInsXmlElement As XMLElementToAdd = Nothing
    '        PreviousInsurance(oPreviousInsXmlElement)
    '        xmlDatasetElementsToAdd(5) = oPreviousInsXmlElement

    '        'PIHISTORY
    '        If gvPreviousInsurancehistory IsNot Nothing AndAlso gvPreviousInsurancehistory.Rows.Count > 0 Then
    '            Dim oPreviousInsHistoryXmlElement As XMLElementToAdd = Nothing
    '            PreviousInsuranceHistory(oPreviousInsHistoryXmlElement)
    '            xmlDatasetElementsToAdd(6) = oPreviousInsHistoryXmlElement
    '        End If
    '        '*****end view of 4- PreviousInsurance****

    '        '****Start view of 5- Legal Liability*****
    '        'Legal Liability
    '        Dim oLegalLiabilityXmlElement As XMLElementToAdd = Nothing

    '        LegalLiability1(oLegalLiabilityXmlElement)
    '        xmlDatasetElementsToAdd(7) = oLegalLiabilityXmlElement

    '        'Personal Accident
    '        Dim oPersonalAccidentXmlElement As XMLElementToAdd = Nothing
    '        PersonalAccident(oPersonalAccidentXmlElement)
    '        xmlDatasetElementsToAdd(8) = oPersonalAccidentXmlElement

    '        'Personal Accident Details
    '        If gvpersonalAccidentDetails IsNot Nothing AndAlso gvpersonalAccidentDetails.Rows.Count > 0 Then
    '            Dim oPersonalAccidentDetailsXmlElement As XMLElementToAdd = Nothing
    '            PersonalAccidentDetails(oPersonalAccidentDetailsXmlElement)
    '            xmlDatasetElementsToAdd(9) = oPersonalAccidentDetailsXmlElement
    '        End If


    '        '****End view of 5- Legal Liability*****

    '        'Start ***** 7-Claim History
    '        Dim oCliamHistoryXmlElement As XMLElementToAdd = Nothing
    '        ClaimHistory1(oCliamHistoryXmlElement)
    '        xmlDatasetElementsToAdd(10) = oCliamHistoryXmlElement

    '        If gvElectricalaccessories IsNot Nothing AndAlso gvElectricalaccessories.Rows.Count > 0 Then
    '            Dim oElectricalXmlElement As XMLElementToAdd = Nothing
    '            ElectricalDetails(oElectricalXmlElement)
    '            xmlDatasetElementsToAdd(12) = oElectricalXmlElement
    '        End If

    '        If gvNonElecTrical IsNot Nothing AndAlso gvNonElecTrical.Rows.Count > 0 Then
    '            Dim oNonElectricalXmlElement As XMLElementToAdd = Nothing
    '            NONElectricalDetails(oNonElectricalXmlElement)
    '            xmlDatasetElementsToAdd(13) = oNonElectricalXmlElement
    '        End If
    '        'End ***** 7-Claim History



    '        'Dim oXMLElement As XMLElementToAdd
    '        'Dim oXMLElement1 As XMLElementToAdd
    '        'Dim oXMLAttr1(0) As XMLAttributeToAdd

    '        'oXMLElement1 = New XMLElementToAdd
    '        'oXMLElement1.ElementName = "PREVIOUSINSURANCE"

    '        'oXMLAttr1(0) = New XMLAttributeToAdd
    '        'oXMLAttr1(0).AttributeName = "POLICY_EXPIRY_DATE"
    '        'oXMLAttr1(0).AttributeValue = txtPreviousPolicyExpiryDate.Text

    '        'oXMLElement1.Attributes = oXMLAttr1


    '        Dim oElementToAddOrUpdate As System.Xml.XmlNode
    '        Dim oElementToAddOrUpdateAdded As System.Xml.XmlNode = Nothing
    '        Dim newAttribute As System.Xml.XmlAttribute
    '        Dim nNextOINumber As Integer
    '        Dim bNewElement As Boolean
    '        Dim oRequest As New UpdateRiskRequestType
    '        Dim oResponse As New UpdateRiskResponseType
    '        ' Read in the XML created in the AddRisk
    '        xmlDoc.LoadXml(Session("m_sRiskDataXML"))

    '        ' Get the NextOINumber and increment it
    '        nNextOINumber = xmlDoc.SelectSingleNode("DATA_SET") _
    '            .Attributes("NextOINumber").Value

    '        nNextOINumber = nNextOINumber + 1


    '        'Dim xmlDatasetElementsToAdd(1) As XMLElementToAdd
    '        'xmlDatasetElementsToAdd(0) = oXMLElement
    '        'xmlDatasetElementsToAdd(1) = oXMLElement1



    '        ' Create the new test elements
    '        For Each xmlDatasetElement As XMLElementToAdd In xmlDatasetElementsToAdd

    '            ' Check if the element already exists
    '            If xmlDatasetElement IsNot Nothing Then


    '                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
    '                                    .SelectSingleNode("RISK_OBJECTS") _
    '                                    .SelectSingleNode("MOTOR_POLICY_BINDER") _
    '                                    .SelectSingleNode(xmlDatasetElement.ElementName)
    '                ' If not, create it
    '                If oElementToAddOrUpdate Is Nothing Then
    '                    oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

    '                    ' Add the common Object Instance (OI) and Update
    '                    ' Status (US) attributes
    '                    newAttribute = xmlDoc.CreateAttribute("OI")
    '                    newAttribute.Value = "OI" & nNextOINumber.ToString
    '                    oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '                    nNextOINumber += 1

    '                    newAttribute = xmlDoc.CreateAttribute("US")
    '                    newAttribute.Value = "1"
    '                    oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '                    bNewElement = True
    '                    If xmlDatasetElement.Attributes.Length > 0 Then
    '                        For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
    '                            newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
    '                            newAttribute.Value = XMLAttribute.AttributeValue
    '                            oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '                        Next
    '                    End If
    '                    'If xmlDatasetElement.ElementName = "PIHISTORY" Then
    '                    '    PIToADD(xmlDatasetElementsToAdd(6), oElementToAddOrUpdateAdded)
    '                    'End If
    '                Else
    '                    ' Update the Update
    '                    ' Status (US) attribute
    '                    oElementToAddOrUpdate.Attributes("US").Value = "2"

    '                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
    '                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
    '                        newAttribute.Value = XMLAttribute.AttributeValue
    '                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '                    Next

    '                    bNewElement = False
    '                End If



    '                If bNewElement Then
    '                    ' Append the new element node to the XML under the POLICY BINDER
    '                    If xmlDatasetElement.ElementName = "PIHISTORY" Then
    '                        oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
    '                            .SelectSingleNode("RISK_OBJECTS") _
    '                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
    '                            .SelectSingleNode("PREVIOUSINSURANCE") _
    '                            .AppendChild(oElementToAddOrUpdate)

    '                    ElseIf xmlDatasetElement.ElementName = "DRIVERDETAILS" Then
    '                        oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
    '                            .SelectSingleNode("RISK_OBJECTS") _
    '                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
    '                            .SelectSingleNode("ADDITIONALCOVER") _
    '                            .AppendChild(oElementToAddOrUpdate)

    '                    ElseIf xmlDatasetElement.ElementName = "PADETAILS" Then
    '                        oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
    '                            .SelectSingleNode("RISK_OBJECTS") _
    '                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
    '                            .SelectSingleNode("LEGALLIABILITY") _
    '                            .AppendChild(oElementToAddOrUpdate)
    '                    Else

    '                        oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
    '                                                .SelectSingleNode("RISK_OBJECTS") _
    '                                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
    '                                                .AppendChild(oElementToAddOrUpdate)
    '                    End If

    '                End If
    '            End If
    '        Next
    '        ' Write back the next OI number to the DATA_SET node
    '        nNextOINumber -= 1
    '        xmlDoc.SelectSingleNode("DATA_SET") _
    '            .Attributes("NextOINumber").Value = nNextOINumber

    '        With oRequest
    '            oRequest.TransactionType = "NB"
    '            .RiskKey = Session("RiskKey")
    '            .XMLDataSet = xmlDoc.OuterXml
    '            .QuoteTimeStamp = Session("TimeStamp")


    '            .BranchCode = "Headoff"
    '            .ScreenCode = "MOTPC"
    '            .RiskDescription = "Motor private Car"



    '            'add values from the quote we just added
    '            .InsuranceFolderKey = Session("InsuranceFolderKey")
    '            .InsuranceFileKey = Session("InsuranceFileKey")
    '        End With

    '        oResponse = oSAM.UpdateRisk(oRequest)

    '        m_sRiskDataXML = oResponse.XMLDataSet
    '        Session("TimeStamp") = oResponse.QuoteTimeStamp

    '        'm_dGrossPremium = oResponse.PremiumDueGross
    '        Response.Redirect("RiskPremiumDetails.aspx")
    '    Catch os As SamResponseException
    '        'should do some error handling here. Just output error for now
    '        If m_nRiskCnt <> 0 Then
    '            btnCancel_Click(sender, e)
    '        End If
    '        Response.Write("An error occured calling SAM:<br>" & os.Message)
    '    Catch oe As Exception
    '        'should do some error handling here. Just output error for now
    '        If Session("RiskKey") <> 0 Then
    '            btnCancel_Click(sender, e)
    '        End If
    '        Response.Write("An error occured:<br>" & oe.Message)
    '    Finally
    '        'clean up any objects here
    '    End Try

    'End Sub


#Region "Filling the XML"

    '    Public Sub CoverNote(ByRef oCoverSelXmlElement As XMLElementToAdd)
    '        'Start CoverSelection
    '        'Dim oCoverSelXmlElement As XMLElementToAdd
    '        Dim oCoverSelXmlAttribute(7) As XMLAttributeToAdd

    '        oCoverSelXmlElement = New XMLElementToAdd
    '        oCoverSelXmlElement.ElementName = "COVERSELECTION"

    '        oCoverSelXmlAttribute(0) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(0).AttributeName = "POLICY_START_DATE"
    '        oCoverSelXmlAttribute(0).AttributeValue = txtPolicyStartDate.Text

    '        oCoverSelXmlAttribute(1) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(1).AttributeName = "POLICY_START_TIME"
    '        oCoverSelXmlAttribute(1).AttributeValue = txtTime1.Text

    '        oCoverSelXmlAttribute(2) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(2).AttributeName = "COVER_NOTE_DATE"
    '        oCoverSelXmlAttribute(2).AttributeValue = txtCoverNoteDate.Text

    '        oCoverSelXmlAttribute(3) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(3).AttributeName = "COVER_NOTE_TIME"
    '        oCoverSelXmlAttribute(3).AttributeValue = txtTime2.Text

    '        oCoverSelXmlAttribute(4) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(4).AttributeName = "COVER_OPTIONS_PACKAGE"
    '        oCoverSelXmlAttribute(4).AttributeValue = ddlCoverOptionpackage.SelectedValue

    '        oCoverSelXmlAttribute(5) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(5).AttributeName = "COVERAGE_SELECTED"
    '        oCoverSelXmlAttribute(5).AttributeValue = ChkRestrictedCover.Checked

    '        oCoverSelXmlAttribute(6) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(6).AttributeName = "ADD_ON_COVER_PACKAGE"
    '        oCoverSelXmlAttribute(6).AttributeValue = chkAddonCover.Checked

    '        oCoverSelXmlAttribute(7) = New XMLAttributeToAdd
    '        oCoverSelXmlAttribute(7).AttributeName = "COVER_OPTIONS_PACKAGE"
    '        oCoverSelXmlAttribute(7).AttributeValue = ddlCoverOptionpackage.SelectedValue
    '        oCoverSelXmlElement.Attributes = oCoverSelXmlAttribute
    '        'End Cover Note
    '        'Return oCoverSelXmlElement
    '    End Sub

    '    Public Sub VehicleDetails(ByRef oVehicleXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(25) As XMLAttributeToAdd

    '        oVehicleXmlElement = New XMLElementToAdd
    '        oVehicleXmlElement.ElementName = "VEHICLEDETAILS"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "VEHICLE_MAKE"
    '        oXMLAttr(0).AttributeValue = ddlVehicleMake.SelectedValue

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "VEHICLE_MODEL"
    '        oXMLAttr(1).AttributeValue = txtVehicleModel.Text

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "BODY_TYPE"
    '        oXMLAttr(2).AttributeValue = txtBodyType.Text

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "SEATING_CAPACITY"
    '        oXMLAttr(3).AttributeValue = txtSeatingcapacity.Text



    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "YEAR_MANUFACTURING"
    '        oXMLAttr(4).AttributeValue = txtYearManufacture.Text

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "ENGINE_NUMBER"
    '        oXMLAttr(5).AttributeValue = txtEngineNumber.Text

    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "CHASSIS_NUMBER"
    '        oXMLAttr(6).AttributeValue = txtChasisNumber.Text

    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "VEH_REG_ADD1"
    '        oXMLAttr(7).AttributeValue = txtLine1.Text

    '        oXMLAttr(8) = New XMLAttributeToAdd
    '        oXMLAttr(8).AttributeName = "VEH_REG_ADD2"
    '        oXMLAttr(8).AttributeValue = txtLine2.Text

    '        oXMLAttr(9) = New XMLAttributeToAdd
    '        oXMLAttr(9).AttributeName = "VEH_REG_CITY"
    '        oXMLAttr(9).AttributeValue = txtCity.Text

    '        oXMLAttr(10) = New XMLAttributeToAdd
    '        oXMLAttr(10).AttributeName = "VEH_REG_STATE"
    '        oXMLAttr(10).AttributeValue = txtState.Text

    '        oXMLAttr(11) = New XMLAttributeToAdd
    '        oXMLAttr(11).AttributeName = "VEH_REG_PINCODE"
    '        oXMLAttr(11).AttributeValue = txtPinCode.Text
    '        oVehicleXmlElement.Attributes = oXMLAttr

    '        oXMLAttr(12) = New XMLAttributeToAdd
    '        oXMLAttr(12).AttributeName = "CUBIC_CAPACITY"
    '        oXMLAttr(12).AttributeValue = txtCC.Text

    '        oXMLAttr(13) = New XMLAttributeToAdd
    '        oXMLAttr(13).AttributeName = "VEHICLE_COLOR"
    '        oXMLAttr(13).AttributeValue = txtVehicleColor.Text

    '        oXMLAttr(14) = New XMLAttributeToAdd
    '        oXMLAttr(14).AttributeName = "EX_SHOWROOM_PRICE"
    '        oXMLAttr(14).AttributeValue = txtExShowRoom.Text


    '        oXMLAttr(15) = New XMLAttributeToAdd
    '        oXMLAttr(15).AttributeName = "EX_SHOWROOM_PRICE"
    '        oXMLAttr(15).AttributeValue = txtExShowRoom.Text

    '        oXMLAttr(16) = New XMLAttributeToAdd
    '        oXMLAttr(16).AttributeName = "VEHICLE_ZONE"
    '        oXMLAttr(16).AttributeValue = txtZone.Text

    '        oXMLAttr(17) = New XMLAttributeToAdd
    '        oXMLAttr(17).AttributeName = "SYSTEM_GENERATED_IDV"
    '        oXMLAttr(17).AttributeValue = txtIDVSystem.Text

    '        oXMLAttr(18) = New XMLAttributeToAdd
    '        oXMLAttr(18).AttributeName = "DATE_OF_PURCHASE"
    '        oXMLAttr(18).AttributeValue = ddlDateOf.SelectedValue

    '        oXMLAttr(19) = New XMLAttributeToAdd
    '        oXMLAttr(19).AttributeName = "INSURED_DECLARED_VALUE"
    '        oXMLAttr(19).AttributeValue = txtIDV.Text

    '        oXMLAttr(20) = New XMLAttributeToAdd
    '        oXMLAttr(20).AttributeName = "DATE_FIRST_REGISTRATION"
    '        oXMLAttr(20).AttributeValue = txtDate.Text

    '        oXMLAttr(21) = New XMLAttributeToAdd
    '        oXMLAttr(21).AttributeName = "NEW_VEHICLE"
    '        oXMLAttr(21).AttributeValue = chkNewVehicle.Checked

    '        oXMLAttr(22) = New XMLAttributeToAdd
    '        oXMLAttr(22).AttributeName = "REG_1"
    '        oXMLAttr(22).AttributeValue = txtReg1.Text

    '        oXMLAttr(23) = New XMLAttributeToAdd
    '        oXMLAttr(23).AttributeName = "REG_2"
    '        oXMLAttr(23).AttributeValue = txtReg2.Text

    '        oXMLAttr(24) = New XMLAttributeToAdd
    '        oXMLAttr(24).AttributeName = "REG_3"
    '        oXMLAttr(24).AttributeValue = txtReg3.Text

    '        oXMLAttr(25) = New XMLAttributeToAdd
    '        oXMLAttr(25).AttributeName = "REG_4"
    '        oXMLAttr(25).AttributeValue = txtReg4.Text

    '        oVehicleXmlElement.Attributes = oXMLAttr

    '    End Sub

    '    Public Sub PreviousInsurance(ByRef oPreviousInsXmlElement As XMLElementToAdd)

    '        Dim oXMLAttr(14) As XMLAttributeToAdd

    '        oPreviousInsXmlElement = New XMLElementToAdd
    '        oPreviousInsXmlElement.ElementName = "PREVIOUSINSURANCE"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "POLICY_NUMBER"
    '        oXMLAttr(0).AttributeValue = txtPolicyNumber.Text

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "POLICY_TYPE"
    '        oXMLAttr(1).AttributeValue = ddlPloicyType.SelectedValue

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "POLICY_EXPIRY_DATE"
    '        oXMLAttr(2).AttributeValue = txtPolicyExpiryDate.Text

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "PREVIOUS_YEAR_NCB"
    '        oXMLAttr(3).AttributeValue = DDLPreviousYearNCB.SelectedValue

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "DOCUMENT_PROOF"
    '        oXMLAttr(4).AttributeValue = txtDocProof.Text

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "PREVIOUS_INSURER_NAME"
    '        oXMLAttr(5).AttributeValue = txtInsuredName.Text

    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "INSURER_ADD1"
    '        oXMLAttr(6).AttributeValue = txtLine1.Text

    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "INSURER_ADD2"
    '        oXMLAttr(7).AttributeValue = txtLine2.Text

    '        oXMLAttr(8) = New XMLAttributeToAdd
    '        oXMLAttr(8).AttributeName = "INSURER_CITY"
    '        oXMLAttr(8).AttributeValue = txtCity.Text


    '        oXMLAttr(9) = New XMLAttributeToAdd
    '        oXMLAttr(9).AttributeName = "INSURER_STATE"
    '        oXMLAttr(9).AttributeValue = txtState.Text

    '        oXMLAttr(10) = New XMLAttributeToAdd
    '        oXMLAttr(10).AttributeName = "INSURER_PINCODE"
    '        oXMLAttr(10).AttributeValue = txtPinCode.Text

    '        oXMLAttr(11) = New XMLAttributeToAdd
    '        oXMLAttr(11).AttributeName = "POLICY_EXPIRY_DATE_NCB"
    '        oXMLAttr(11).AttributeValue = txtPreviousPolicyExpiryDate.Text

    '        oXMLAttr(12) = New XMLAttributeToAdd
    '        oXMLAttr(12).AttributeName = "VEHICLE_INSPECTIONDATE"
    '        oXMLAttr(12).AttributeValue = txtInspectionDate.Text

    '        oXMLAttr(13) = New XMLAttributeToAdd
    '        oXMLAttr(13).AttributeName = "VEHICLE_INSPECTEDBY"
    '        oXMLAttr(13).AttributeValue = txtInspectedByWhom.Text

    '        oXMLAttr(14) = New XMLAttributeToAdd
    '        oXMLAttr(14).AttributeName = "VEHICLE_INSPECTIONREPORT"
    '        oXMLAttr(14).AttributeValue = txtInspectionReport.Text

    '        oPreviousInsXmlElement.Attributes = oXMLAttr

    '    End Sub

    '    Public Sub PreviousInsuranceHistory(ByRef oPreviousInsHistoryXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(4) As XMLAttributeToAdd

    '        oPreviousInsHistoryXmlElement = New XMLElementToAdd
    '        oPreviousInsHistoryXmlElement.ElementName = "PIHISTORY"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "PI_POLICYNUMBER"
    '        oXMLAttr(0).AttributeValue = gvPreviousInsurancehistory.Rows(0).Cells(0).Text

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "PI_NUMBEROFCLAIMS"
    '        oXMLAttr(1).AttributeValue = gvPreviousInsurancehistory.Rows(0).Cells(1).Text



    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "PI_YEAR"
    '        oXMLAttr(2).AttributeValue = gvPreviousInsurancehistory.Rows(0).Cells(2).Text

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "PI_TOTALAMOUNTOFCLAIMS"
    '        oXMLAttr(3).AttributeValue = gvPreviousInsurancehistory.Rows(0).Cells(3).Text

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "PI_INCURREDLOSSRATIO"
    '        oXMLAttr(4).AttributeValue = gvPreviousInsurancehistory.Rows(0).Cells(4).Text

    '        oPreviousInsHistoryXmlElement.Attributes = oXMLAttr

    '    End Sub


    '    Public Sub DriverDetails(ByRef oDriverDetailsXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(14) As XMLAttributeToAdd

    '        oDriverDetailsXmlElement = New XMLElementToAdd
    '        oDriverDetailsXmlElement.ElementName = "DRIVERDETAILS"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "VEHICLE_DRIVEN_BY"
    '        oXMLAttr(0).AttributeValue = ddlvehicledrivenby.Items.FindByText(gvDriverDetails.Rows(0).Cells(0).Text).Value

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "PAID_DRIVER"
    '        oXMLAttr(1).AttributeValue = gvDriverDetails.Rows(0).Cells(1).Text

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "MORE_THAN_ONE_DRIVER"
    '        oXMLAttr(2).AttributeValue = gvDriverDetails.Rows(0).Cells(2).Text


    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "DRIVER_NAME"
    '        oXMLAttr(3).AttributeValue = gvDriverDetails.Rows(0).Cells(3).Text


    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "AGE"
    '        oXMLAttr(4).AttributeValue = gvDriverDetails.Rows(0).Cells(4).Text


    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "GENDER"
    '        oXMLAttr(5).AttributeValue = ddlGender.Items.FindByText(gvDriverDetails.Rows(0).Cells(5).Text).Value


    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "RELATIONSHIP_WITH_OWNER"
    '        oXMLAttr(6).AttributeValue = ddlRelationshipwithowner.Items.FindByText(gvDriverDetails.Rows(0).Cells(6).Text).Value


    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "DRIVER_EDUCATION_QUALIFICATION"
    '        oXMLAttr(7).AttributeValue = gvDriverDetails.Rows(0).Cells(7).Text


    '        oXMLAttr(8) = New XMLAttributeToAdd
    '        oXMLAttr(8).AttributeName = "DRIVING_LICENCE_NUMBER"
    '        oXMLAttr(8).AttributeValue = gvDriverDetails.Rows(0).Cells(8).Text


    '        oXMLAttr(9) = New XMLAttributeToAdd
    '        oXMLAttr(9).AttributeName = "LICENSE_ISSUING_AUTHORITY"
    '        oXMLAttr(9).AttributeValue = gvDriverDetails.Rows(0).Cells(9).Text


    '        oXMLAttr(10) = New XMLAttributeToAdd
    '        oXMLAttr(10).AttributeName = "DATE_OF_FIRST_ISSUE"
    '        oXMLAttr(10).AttributeValue = gvDriverDetails.Rows(0).Cells(10).Text


    '        oXMLAttr(11) = New XMLAttributeToAdd
    '        oXMLAttr(11).AttributeName = "DRIVERS_EXPERIENCE"
    '        oXMLAttr(11).AttributeValue = gvDriverDetails.Rows(0).Cells(12).Text


    '        oXMLAttr(12) = New XMLAttributeToAdd
    '        oXMLAttr(12).AttributeName = "TYPE_OF_LICENCE"
    '        oXMLAttr(12).AttributeValue = ddlTypeoflicence.Items.FindByText(gvDriverDetails.Rows(0).Cells(13).Text).Value


    '        oXMLAttr(13) = New XMLAttributeToAdd
    '        oXMLAttr(13).AttributeName = "DEFENSIVE_ADVANCED_DRIVING"
    '        oXMLAttr(13).AttributeValue = gvDriverDetails.Rows(0).Cells(14).Text


    '        oXMLAttr(13) = New XMLAttributeToAdd
    '        oXMLAttr(13).AttributeName = "RENEWAL_DATE"
    '        oXMLAttr(13).AttributeValue = gvDriverDetails.Rows(0).Cells(11).Text


    '        oXMLAttr(14) = New XMLAttributeToAdd
    '        oXMLAttr(14).AttributeName = "ENDORSEMENTS"
    '        oXMLAttr(14).AttributeValue = gvDriverDetails.Rows(0).Cells(15).Text


    '        oDriverDetailsXmlElement.Attributes = oXMLAttr


    '    End Sub


    '    Public Sub VehicleUsage(ByRef oVehicleInsXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(3) As XMLAttributeToAdd

    '        oVehicleInsXmlElement = New XMLElementToAdd
    '        oVehicleInsXmlElement.ElementName = "VEHICLEUSAGEDETAILS"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "VEHICLE_PARKING"
    '        oXMLAttr(0).AttributeValue = ""

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "MAIN_USAGE_OF_VEHICLE"
    '        oXMLAttr(1).AttributeValue = ""

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "DISTANCE_DRIVEN_ANNUALLY"
    '        oXMLAttr(2).AttributeValue = ""

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "TYPE_OF_ROADS"
    '        oXMLAttr(3).AttributeValue = ""

    '        oVehicleInsXmlElement.Attributes = oXMLAttr
    '    End Sub

    '    Public Sub AdditionalCoverDetails(ByRef oAdditionalCoverDetailXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(35) As XMLAttributeToAdd

    '        oAdditionalCoverDetailXmlElement = New XMLElementToAdd
    '        oAdditionalCoverDetailXmlElement.ElementName = "ADDITIONALCOVER"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "BI_FUEL_KIT_INBUILT"
    '        oXMLAttr(0).AttributeValue = ""

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "LPG_CNG"
    '        oXMLAttr(1).AttributeValue = chkLPGkit.Checked

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "LPG_CNG_SUM_INSURED"
    '        oXMLAttr(1).AttributeValue = txtLPGSI.Text

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "NON_CONVENTIONAL_SOF"
    '        oXMLAttr(2).AttributeValue = chkNonConvectionalPower.Checked

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "DESC_NON_CONVENTIONAL_SOF"
    '        oXMLAttr(3).AttributeValue = txtDescription.Text

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "ANTITHEFT_DEVICE"
    '        oXMLAttr(4).AttributeValue = chkAntitheft.Checked

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "HANDICAPPED_USER"
    '        oXMLAttr(5).AttributeValue = chkIshandicapped.Checked

    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "FOREIGN_EMBASSY"
    '        oXMLAttr(6).AttributeValue = chkForiegnEmbassy.Checked

    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "FIBRE_FUEL_TANK"
    '        oXMLAttr(7).AttributeValue = chkFiberGlassFuelTank.Checked

    '        oXMLAttr(8) = New XMLAttributeToAdd
    '        oXMLAttr(8).AttributeName = "DRIVING_TUITIONS"
    '        oXMLAttr(8).AttributeValue = chkIsvehicleusedfordrivingtutions.Checked

    '        oXMLAttr(9) = New XMLAttributeToAdd
    '        oXMLAttr(9).AttributeName = "LIMITED_TO_OWN_PREMISES"
    '        oXMLAttr(9).AttributeValue = chkOwnprmesis.Checked

    '        oXMLAttr(10) = New XMLAttributeToAdd
    '        oXMLAttr(10).AttributeName = "VINTAGE_CAR"
    '        oXMLAttr(10).AttributeValue = chkcertifiedVintage.Checked

    '        oXMLAttr(11) = New XMLAttributeToAdd
    '        oXMLAttr(11).AttributeName = "EXT_OF_GEO_AREA"
    '        oXMLAttr(11).AttributeValue = chkGeographical.Checked

    '        oXMLAttr(12) = New XMLAttributeToAdd
    '        oXMLAttr(12).AttributeName = "BANGLADESH"
    '        oXMLAttr(12).AttributeValue = chkBangaldesh.Checked

    '        oXMLAttr(13) = New XMLAttributeToAdd
    '        oXMLAttr(13).AttributeName = "BHUTAN"
    '        oXMLAttr(13).AttributeValue = chkBhutan.Checked

    '        oXMLAttr(14) = New XMLAttributeToAdd
    '        oXMLAttr(14).AttributeName = "MALDIVES"
    '        oXMLAttr(14).AttributeValue = chkMaldives.Checked

    '        oXMLAttr(15) = New XMLAttributeToAdd
    '        oXMLAttr(15).AttributeName = "NEPAL"
    '        oXMLAttr(15).AttributeValue = chkNepal.Checked

    '        oXMLAttr(16) = New XMLAttributeToAdd
    '        oXMLAttr(16).AttributeName = "PAKISTAN"
    '        oXMLAttr(16).AttributeValue = chkPakistan.Checked

    '        oXMLAttr(17) = New XMLAttributeToAdd
    '        oXMLAttr(17).AttributeName = "SRILANKA"
    '        oXMLAttr(17).AttributeValue = chkSrilanka.Checked

    '        oXMLAttr(18) = New XMLAttributeToAdd
    '        oXMLAttr(18).AttributeName = "RALLIES"
    '        oXMLAttr(18).AttributeValue = chkVechileusedforrallies.Checked

    '        oXMLAttr(19) = New XMLAttributeToAdd
    '        oXMLAttr(19).AttributeName = "START_DATE"
    '        oXMLAttr(19).AttributeValue = txtStartDate.Text

    '        oXMLAttr(20) = New XMLAttributeToAdd
    '        oXMLAttr(20).AttributeName = "END_DATE"
    '        oXMLAttr(20).AttributeValue = txtEndDate.Text

    '        oXMLAttr(21) = New XMLAttributeToAdd
    '        oXMLAttr(21).AttributeName = "RELIABILITY_TRIALS"
    '        oXMLAttr(21).AttributeValue = chkBrokerageAgencyCommission.Checked

    '        oXMLAttr(22) = New XMLAttributeToAdd
    '        oXMLAttr(22).AttributeName = "VOLUNTARY_EXCESS"
    '        oXMLAttr(22).AttributeValue = chkVoluntaryDeductible.Checked

    '        oXMLAttr(23) = New XMLAttributeToAdd
    '        oXMLAttr(23).AttributeName = "VOLUNTARY_EXCESS_AMOUNT"
    '        oXMLAttr(23).AttributeValue = ddlVoluntaryExcessamount.SelectedItem.Text

    '        oXMLAttr(24) = New XMLAttributeToAdd
    '        oXMLAttr(24).AttributeName = "IMPOSED_EXCESS"
    '        oXMLAttr(24).AttributeValue = txtImposedExcess.Text

    '        oXMLAttr(25) = New XMLAttributeToAdd
    '        oXMLAttr(25).AttributeName = "COMPULSORY_EXCESS"
    '        oXMLAttr(25).AttributeValue = txtCompulsoryExcess.Text

    '        oXMLAttr(26) = New XMLAttributeToAdd
    '        oXMLAttr(26).AttributeName = "CNG_KIT"
    '        oXMLAttr(26).AttributeValue = chkCNGSI.Checked

    '        oXMLAttr(27) = New XMLAttributeToAdd
    '        oXMLAttr(27).AttributeName = "CNG_SUM_INSURED"
    '        oXMLAttr(27).AttributeValue = txtCNGSI.Text

    '        oXMLAttr(28) = New XMLAttributeToAdd
    '        oXMLAttr(28).AttributeName = "AGENCY_COMMISSIONIND"
    '        oXMLAttr(28).AttributeValue = chkBrokerageAgencyCommission.Checked

    '        oXMLAttr(29) = New XMLAttributeToAdd
    '        oXMLAttr(29).AttributeName = "AGENCY_COMMISSION"
    '        oXMLAttr(29).AttributeValue = txtDiscountAgentCommission.Text

    '        oXMLAttr(30) = New XMLAttributeToAdd
    '        oXMLAttr(30).AttributeName = "GOOD_FEATURE_DISCOUNTIND"
    '        oXMLAttr(30).AttributeValue = chkGoodfeatureDiscount.Checked

    '        oXMLAttr(31) = New XMLAttributeToAdd
    '        oXMLAttr(31).AttributeName = "GOOD_FEATURE_DISCOUNT"
    '        oXMLAttr(31).AttributeValue = txtDiscountGoodfeature.Text

    '        oXMLAttr(32) = New XMLAttributeToAdd
    '        oXMLAttr(32).AttributeName = "SPECIAL_DISCOUNTIND"
    '        oXMLAttr(32).AttributeValue = chkSpecialDiscount.Checked

    '        oXMLAttr(33) = New XMLAttributeToAdd
    '        oXMLAttr(33).AttributeName = "SPECIAL_DISCOUNT"
    '        oXMLAttr(33).AttributeValue = txtDiscountSpecial.Text

    '        oXMLAttr(34) = New XMLAttributeToAdd
    '        oXMLAttr(34).AttributeName = "RTA_PERMISSION_FOR_NCSOP"
    '        oXMLAttr(34).AttributeValue = chkPermissionRTA.Checked

    '        oXMLAttr(35) = New XMLAttributeToAdd
    '        oXMLAttr(35).AttributeName = "DETARRIFF_DISCOUNT"
    '        oXMLAttr(35).AttributeValue = ddlDetarriffDiscount.SelectedItem.Text
    '        oAdditionalCoverDetailXmlElement.Attributes = oXMLAttr

    '    End Sub

    '    Public Sub LegalLiability1(ByRef oLegalLiabilityXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(2) As XMLAttributeToAdd

    '        oLegalLiabilityXmlElement = New XMLElementToAdd
    '        oLegalLiabilityXmlElement.ElementName = "LEGALLIABILITY"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "WORKMEN_FOR_OPS_MAINTENANCE"
    '        oXMLAttr(0).AttributeValue = txtNumberOfPaiddrivesCleanersWorkMan.Text

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "NO_OF_EMP_TRAVELLING_DRIVING"
    '        oXMLAttr(1).AttributeValue = txtNumberOFEmployeeTravellingDriving.Text

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "RESTRICTED_TPPD"
    '        oXMLAttr(2).AttributeValue = chkDoyouwishtorestrictTPPDCovertostatutoryLimitofRs6000.Checked

    '        oLegalLiabilityXmlElement.Attributes = oXMLAttr
    '    End Sub

    '    Public Sub PersonalAccident(ByRef oPersonalAccidentXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(10) As XMLAttributeToAdd

    '        oPersonalAccidentXmlElement = New XMLElementToAdd
    '        oPersonalAccidentXmlElement.ElementName = "PERSONALACCIDENT"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "PA_UN_NAMED"
    '        oXMLAttr(0).AttributeValue = chkPersonalaccidentcoverforunnamedpersons.Checked

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "PA_UN_NAMED_NUMBER"
    '        oXMLAttr(1).AttributeValue = txtNumberOfunnamedPersons.Text

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "PA_UN_NAMED_SUMINSURED"
    '        oXMLAttr(2).AttributeValue = txtSumInsuredPerPerson.Text

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "PA_PAID_DRIVER_CLEANER"
    '        oXMLAttr(3).AttributeValue = chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner.Checked

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "PA_NUMBER"
    '        oXMLAttr(4).AttributeValue = txtNumberOfpaiddrivers.Text

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "PA_SUM_INSURED"
    '        oXMLAttr(5).AttributeValue = txtSumInsured2.Text

    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "PA_NAMED_PERSON"
    '        oXMLAttr(6).AttributeValue = chkDoyouwishtotakepersonalaccidentcoverfornamedpersons.Checked

    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "PA_TOTAL_SI"
    '        oXMLAttr(7).AttributeValue = txtTotalsuminsuredofallnamedpersons.Text

    '        oXMLAttr(8) = New XMLAttributeToAdd
    '        oXMLAttr(8).AttributeName = "IS_COMP_PA"
    '        oXMLAttr(8).AttributeValue = chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements.Checked

    '        oXMLAttr(9) = New XMLAttributeToAdd
    '        oXMLAttr(9).AttributeName = "COMP_PA_SI"
    '        oXMLAttr(9).AttributeValue = txtSumInsured1.Text

    '        oXMLAttr(10) = New XMLAttributeToAdd
    '        oXMLAttr(10).AttributeName = "IS_ONLY_CAR"
    '        oXMLAttr(10).AttributeValue = chkThisvechcle.Checked



    '        oPersonalAccidentXmlElement.Attributes = oXMLAttr
    '    End Sub


    '    Public Sub PersonalAccidentDetails(ByRef oPersonalAccidentDetailsXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(8) As XMLAttributeToAdd

    '        oPersonalAccidentDetailsXmlElement = New XMLElementToAdd
    '        oPersonalAccidentDetailsXmlElement.ElementName = "PADETAILS"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "PA_NAME"
    '        oXMLAttr(0).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(1).Text

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "PA_SUM_INSURED"
    '        oXMLAttr(1).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(2).Text

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "PA_SERIAL_NUMBER"
    '        oXMLAttr(2).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(0).Text

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "PA_NOMINEE_DETAILS"
    '        oXMLAttr(3).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(3).Text

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "NOMINEE_ADD1"
    '        oXMLAttr(4).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(4).Text

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "NOMINEE_ADD2"
    '        oXMLAttr(5).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(5).Text

    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "NOMINEE_CITY"
    '        oXMLAttr(6).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(6).Text

    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "NOMINEE_STATE"
    '        oXMLAttr(7).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(7).Text

    '        oXMLAttr(8) = New XMLAttributeToAdd
    '        oXMLAttr(8).AttributeName = "NOMINEE_PINCODE"
    '        oXMLAttr(8).AttributeValue = gvpersonalAccidentDetails.Rows(0).Cells(8).Text

    '        oPersonalAccidentDetailsXmlElement.Attributes = oXMLAttr
    '    End Sub


    '    Public Sub ClaimHistory1(ByRef oClaimHistoryXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(5) As XMLAttributeToAdd

    '        oClaimHistoryXmlElement = New XMLElementToAdd
    '        oClaimHistoryXmlElement.ElementName = "CLAIMSHISTORY"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "CLAIM_NUMBER"
    '        oXMLAttr(0).AttributeValue = ""

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "CLAIM_STATUS"
    '        oXMLAttr(1).AttributeValue = ""

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "POLICY_NUMBER"
    '        oXMLAttr(2).AttributeValue = ""

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "LOSS_TIME"
    '        oXMLAttr(3).AttributeValue = ""

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "LOSS_DATE"
    '        oXMLAttr(4).AttributeValue = ""

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "CAUSE"
    '        oXMLAttr(5).AttributeValue = ""

    '        oClaimHistoryXmlElement.Attributes = oXMLAttr
    '    End Sub
    '    Public Sub FinancierDetails(ByRef oFinancierDetailXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(7) As XMLAttributeToAdd

    '        oFinancierDetailXmlElement = New XMLElementToAdd
    '        oFinancierDetailXmlElement.ElementName = "FINANCIERDETAILS"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "AGREEMENT_TYPE"
    '        oXMLAttr(0).AttributeValue = ""

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "FINANCIER_TYPE"
    '        oXMLAttr(1).AttributeValue = ""

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "NAME_OF_FINANCIER"
    '        oXMLAttr(2).AttributeValue = ""

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "FINANCIER_ADD1"
    '        oXMLAttr(3).AttributeValue = ""

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "FINANCIER_ADD2"
    '        oXMLAttr(4).AttributeValue = ""

    '        oXMLAttr(5) = New XMLAttributeToAdd
    '        oXMLAttr(5).AttributeName = "FINANCIER_CITY"
    '        oXMLAttr(5).AttributeValue = ""

    '        oXMLAttr(6) = New XMLAttributeToAdd
    '        oXMLAttr(6).AttributeName = "FINANCIER_PINCODE"
    '        oXMLAttr(6).AttributeValue = ""

    '        oXMLAttr(7) = New XMLAttributeToAdd
    '        oXMLAttr(7).AttributeName = "FINANCIER_STATE"
    '        oXMLAttr(7).AttributeValue = ""

    '        oFinancierDetailXmlElement.Attributes = oXMLAttr
    '    End Sub

    '    Public Sub ElectricalDetails(ByRef oElectricalDetailXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(4) As XMLAttributeToAdd

    '        oElectricalDetailXmlElement = New XMLElementToAdd
    '        oElectricalDetailXmlElement.ElementName = "ELECTRICAL"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "ELECTRICAL_SERIAL_NUMBER"
    '        oXMLAttr(0).AttributeValue = ""

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "PRODUCT_SERIAL_NUMBER"
    '        oXMLAttr(1).AttributeValue = ""

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "ELECTRIC_MAKE"
    '        oXMLAttr(2).AttributeValue = ""

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "ELECTRIC_SUMINSURED"
    '        oXMLAttr(3).AttributeValue = ""

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "ELECTRICAL_DESCRIPTION"
    '        oXMLAttr(4).AttributeValue = ""

    '        oElectricalDetailXmlElement.Attributes = oXMLAttr
    '    End Sub

    '    Public Sub NONElectricalDetails(ByRef oNonElectricalDetailXmlElement As XMLElementToAdd)
    '        Dim oXMLAttr(4) As XMLAttributeToAdd

    '        oNonElectricalDetailXmlElement = New XMLElementToAdd
    '        oNonElectricalDetailXmlElement.ElementName = "NONELECTRICAL"

    '        oXMLAttr(0) = New XMLAttributeToAdd
    '        oXMLAttr(0).AttributeName = "NON_ELECTRICAL_SERIAL_NUMBER"
    '        oXMLAttr(0).AttributeValue = ""

    '        oXMLAttr(1) = New XMLAttributeToAdd
    '        oXMLAttr(1).AttributeName = "NON_PRODUCT_SERIAL_NUMBER"
    '        oXMLAttr(1).AttributeValue = ""

    '        oXMLAttr(2) = New XMLAttributeToAdd
    '        oXMLAttr(2).AttributeName = "NON_ELECTRIC_MAKE"
    '        oXMLAttr(2).AttributeValue = ""

    '        oXMLAttr(3) = New XMLAttributeToAdd
    '        oXMLAttr(3).AttributeName = "NON_ELECTRIC_SUMINSURED"
    '        oXMLAttr(3).AttributeValue = ""

    '        oXMLAttr(4) = New XMLAttributeToAdd
    '        oXMLAttr(4).AttributeName = "NON_ELECTRICAL_DESCRIPTION"
    '        oXMLAttr(4).AttributeValue = ""

    '        oNonElectricalDetailXmlElement.Attributes = oXMLAttr
    '    End Sub
    '    Public Sub PIToADD(ByRef oPreviousInsHistoryXmlElement As XMLElementToAdd, ByRef oElementToAddOrUpdateAdded As System.Xml.XmlNode)
    '        'Dim xmlDoc As New System.Xml.XmlDocument
    '        Dim oElementToAddOrUpdate As System.Xml.XmlNode
    '        'Dim oElementToAddOrUpdateAdded As System.Xml.XmlNode
    '        'Dim newAttribute As System.Xml.XmlAttribute
    '        'Dim nNextOINumber As Integer
    '        'Dim bNewElement As Boolean
    '        ''Adding PI to PreviousInsurance
    '        oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, oPreviousInsHistoryXmlElement.ElementName, "")

    '        '' Add the common Object Instance (OI) and Update
    '        '' Status (US) attributes
    '        'newAttribute = xmlDoc.CreateAttribute("OI")
    '        'newAttribute.Value = "OI" & nNextOINumber.ToString
    '        'oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '        'nNextOINumber += 1

    '        'newAttribute = xmlDoc.CreateAttribute("US")
    '        'newAttribute.Value = "1"
    '        'oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '        'bNewElement = True
    '        'If oPreviousInsHistoryXmlElement.Attributes.Length > 0 Then
    '        '    For Each XMLAttribute As XMLAttributeToAdd In oPreviousInsHistoryXmlElement.Attributes
    '        '        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
    '        '        newAttribute.Value = XMLAttribute.AttributeValue
    '        '        oElementToAddOrUpdate.Attributes.Append(newAttribute)
    '        '    Next


    '        'End If
    '        oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
    '                                .SelectSingleNode("RISK_OBJECTS") _
    '                                .SelectSingleNode("MOTOR_POLICY_BINDER") _
    '                                .SelectSingleNode("PREVIOUSINSURANCE") _
    '        .AppendChild(oElementToAddOrUpdate)
    '        'End
    '    End Sub
#End Region

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
            Dim StartDate As Date
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
            With oResponse
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    lblSamErrorMessage.Text = GetMessageFromSamError(.Errors)
                Else

                    objControl.DataSource = oResponse.List
                    objControl.DataTextField = "Description"
                    objControl.DataValueField = "Key"
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
            Dim StartDate As Date
            StartDate = Date.Now
            oResponse = oSAM.GetList(oRequest)
            WriteToLog(Session, "PolicyHeader.aspx", "SAMForInsuranceV2", "GetList", StartDate, Date.Now)
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
        'If (chkincludeDriverDetails.Checked = True) Then
        '    Response.Redirect("Drivers Details.aspx")
        'End If

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
        'If chkDoyouwishtotakepersonalaccidentcoverfornamedpersons.Checked = True Then
        '    Response.Redirect("PersnnalAcciendt.aspx")
        'End If
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
        'If gvPreviousInsurancehistory.Rows.Count > 1 Then
        pnlPIHistory.Visible = False
        PreviousInsurancehistory1.Visible = True
        '    Dim addindex As String = Nodeindex.Get("PIHISTORY")
        '    Dim icount As Integer = CInt(addindex) + 1
        '    Nodeindex.Set("PIHISTORY", icount.ToString())

        'End If




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
        pnlNonElecTrical1.Visible = True
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
        pnlDriverdetails1.Visible = True
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
        If chkFinancierDetails.Checked = True Then
            Session("FINANCIEREDIT") = 0

            pnlfinancierDetails.Visible = True
        Else
            pnlfinancierDetails.Visible = False
        End If
        ClearText(pnlfinancierDetails)
    End Sub

    Protected Sub btnFinancierOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinancierOk.Click

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
        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_STATE") = DirectCast((row.Cells(8).Controls(0)), TextBox).Text
        dt.Rows(gvFinancierDetails.EditIndex)("FINANCIER_PINCODE") = DirectCast((row.Cells(9).Controls(0)), TextBox).Text


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
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRIC_MAKE") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRIC_SUMINSURED") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRICAL_DESCRIPTION") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("ELECTRICAL_SERIAL_NUMBER") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvElectricalaccessories.EditIndex)("PRODUCT_SERIAL_NUMBER") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
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
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRICAL_MAKE") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRIC_SUMINSURED") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRICAL_DESCRIPTION") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("NON_ELECTRICAL_SERIAL_NUMBER") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvNonElecTrical.EditIndex)("PRODUCT_SERIAL_NUMBER") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
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
        dt.Rows(gvDriverDetails.EditIndex)("PAID_DRIVER") = DirectCast((row.Cells(2).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("VEHICLE_DRIVEN_BY") = DirectCast((row.Cells(3).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("MORE_THAN_ONE_DRIVER") = DirectCast((row.Cells(4).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVER_NAME") = DirectCast((row.Cells(5).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("AGE") = DirectCast((row.Cells(6).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("GENDER") = DirectCast((row.Cells(7).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("RELATIONSHIP_WITH_OWNER") = DirectCast((row.Cells(8).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVING_LICENCE_NUMBER") = DirectCast((row.Cells(9).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DATE_OF_FIRST_ISSUE") = DirectCast((row.Cells(10).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("ENDORSEMENTS") = DirectCast((row.Cells(11).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("RENEWAL_DATE") = DirectCast((row.Cells(12).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("TYPE_OF_LICENCE") = DirectCast((row.Cells(13).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DEFENSIVE_ADVANCED_DRIVING") = DirectCast((row.Cells(14).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("LICENSE_ISSUING_AUTHORITY") = DirectCast((row.Cells(15).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVER_EDUCATION_QUALIFICATION") = DirectCast((row.Cells(16).Controls(0)), TextBox).Text
        dt.Rows(gvDriverDetails.EditIndex)("DRIVERS_EXPERIENCE") = DirectCast((row.Cells(17).Controls(0)), TextBox).Text
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

        'Remove the xml element
        xmlDoc.LoadXml(Session("m_sRiskDataXML"))
        Dim oElementToDelete As System.Xml.XmlNode
        'Dim ParentNode As XmlNode
        oElementToDelete = xmlDoc.SelectSingleNode("DATA_SET") _
                           .SelectSingleNode("RISK_OBJECTS") _
                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                           .SelectSingleNode("PREVIOUSINSURANCE") _
                           .SelectNodes("PIHISTORY").Item(e.RowIndex)
        If oElementToDelete IsNot Nothing Then
            oElementToDelete.Attributes("US").Value = "3"
            Session("m_sRiskDataXML") = xmlDoc.OuterXml
        End If
        If gvPreviousInsurancehistory.Rows.Count > 0 Then
            PreviousInsurancehistory1.Visible = True
        Else
            PreviousInsurancehistory1.Visible = False
        End If
    End Sub

    Protected Sub gvElectricalaccessories_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvElectricalaccessories.RowDeleting
        Dim dt As DataTable = DirectCast(Session("Electricaldetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("Electricaldetails") = dt
        gvElectricalaccessories.DataSource = Session("Electricaldetails")
        gvElectricalaccessories.DataBind()

        'Remove the xml element
        xmlDoc.LoadXml(Session("m_sRiskDataXML"))
        Dim oElementToDelete As System.Xml.XmlNode
        'Dim ParentNode As XmlNode

        oElementToDelete = xmlDoc.SelectSingleNode("DATA_SET") _
                           .SelectSingleNode("RISK_OBJECTS") _
                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                           .SelectSingleNode("COVERSELECTION") _
                           .SelectNodes("ELECTRICAL").Item(e.RowIndex)
        'ParentNode = oElementToDelete.ParentNode
        'ParentNode.RemoveChild(oElementToDelete)
        If oElementToDelete IsNot Nothing Then
            oElementToDelete.Attributes("US").Value = "3"
            Session("m_sRiskDataXML") = xmlDoc.OuterXml
        End If
        txttotalSumInsured.Text = TotalSumInsured(DirectCast(Session("Electricaldetails"), DataTable), "ELECTRIC_SUMINSURED")
    End Sub

    Protected Sub gvNonElecTrical_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvNonElecTrical.RowDeleting
        Dim dt As DataTable = DirectCast(Session("NonElectricaldetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("NonElectricaldetails") = dt
        gvNonElecTrical.DataSource = Session("NonElectricaldetails")
        gvNonElecTrical.DataBind()

        'Remove the xml element
        xmlDoc.LoadXml(Session("m_sRiskDataXML"))
        Dim oElementToDelete As System.Xml.XmlNode
        'Dim ParentNode As XmlNode

        oElementToDelete = xmlDoc.SelectSingleNode("DATA_SET") _
                           .SelectSingleNode("RISK_OBJECTS") _
                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                           .SelectSingleNode("COVERSELECTION") _
                           .SelectNodes("NONELECTRICAL").Item(e.RowIndex)
        'ParentNode = oElementToDelete.ParentNode
        'ParentNode.RemoveChild(oElementToDelete)
        If oElementToDelete IsNot Nothing Then
            oElementToDelete.Attributes("US").Value = "3"
            Session("m_sRiskDataXML") = xmlDoc.OuterXml
        End If
        txtSumInsurNonelec.Text = TotalSumInsured(DirectCast(Session("NonElectricaldetails"), DataTable), "NON_ELECTRIC_SUMINSURED")

        If gvNonElecTrical.Rows.Count > 0 Then
            pnlNonElecTrical1.Visible = True
        Else
            pnlNonElecTrical1.Visible = False
        End If
    End Sub

    Protected Sub gvDriverDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvDriverDetails.RowDeleting
        Dim dt As DataTable = DirectCast(Session("DriverDetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("DriverDetails") = dt
        gvDriverDetails.DataSource = Session("DriverDetails")
        gvDriverDetails.DataBind()

        'Remove the xml element
        xmlDoc.LoadXml(Session("m_sRiskDataXML"))
        Dim oElementToDelete As System.Xml.XmlNode = Nothing
        'Dim ParentNode As XmlNode

        oElementToDelete = xmlDoc.SelectSingleNode("DATA_SET") _
                                   .SelectSingleNode("RISK_OBJECTS") _
                                   .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                   .SelectSingleNode("COVERSELECTION") _
                                   .SelectNodes("DRIVERDETAILS").Item(e.RowIndex)
        If oElementToDelete IsNot Nothing Then
            oElementToDelete.Attributes("US").Value = "3"
            Session("m_sRiskDataXML") = xmlDoc.OuterXml
        End If

        If gvDriverDetails.Rows.Count > 0 Then
            pnlDriverdetails1.Visible = True
        Else
            pnlDriverdetails1.Visible = False
        End If

    End Sub

    Protected Sub gvpersonalAccidentDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvpersonalAccidentDetails.RowDeleting
        Dim dt As DataTable = DirectCast(Session("PersonalAccident"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("PersonalAccident") = dt
        gvpersonalAccidentDetails.DataSource = Session("PersonalAccident")
        gvpersonalAccidentDetails.DataBind()

        'Remove the xml element
        xmlDoc.LoadXml(Session("m_sRiskDataXML"))
        Dim oElementToDelete As System.Xml.XmlNode
        'Dim ParentNode As XmlNode

        oElementToDelete = xmlDoc.SelectSingleNode("DATA_SET") _
                           .SelectSingleNode("RISK_OBJECTS") _
                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                           .SelectSingleNode("PERSONALACCIDENT") _
                           .SelectNodes("PADETAILS").Item(e.RowIndex)
        'ParentNode = oElementToDelete.ParentNode
        'ParentNode.RemoveChild(oElementToDelete)
        If oElementToDelete IsNot Nothing Then
            oElementToDelete.Attributes("US").Value = "3"
            Session("m_sRiskDataXML") = xmlDoc.OuterXml
        End If
        txtTotalsuminsuredofallnamedpersons.Text = TotalSumInsured(DirectCast(Session("PersonalAccident"), DataTable), "PA_SUM_INSURED")

    End Sub

    Protected Sub gvFinancierDetails_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvFinancierDetails.RowDeleting
        Dim dt As DataTable = DirectCast(Session("FinancierDetails"), DataTable)
        dt.Rows.RemoveAt(e.RowIndex)
        dt.AcceptChanges()
        Session("FinancierDetails") = dt

        gvFinancierDetails.DataSource = Session("FinancierDetails")
        gvFinancierDetails.DataBind()

        'Remove the xml element
        xmlDoc.LoadXml(Session("m_sRiskDataXML"))
        Dim oElementToDelete As System.Xml.XmlNode
        'Dim ParentNode As XmlNode

        oElementToDelete = xmlDoc.SelectSingleNode("DATA_SET") _
                           .SelectSingleNode("RISK_OBJECTS") _
                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                           .SelectSingleNode("FINANCIER") _
                           .SelectNodes("FINANCIERDETAILS").Item(e.RowIndex)
        'ParentNode = oElementToDelete.ParentNode
        'ParentNode.RemoveChild(oElementToDelete)
        If oElementToDelete IsNot Nothing Then
            oElementToDelete.Attributes("US").Value = "3"
            Session("m_sRiskDataXML") = xmlDoc.OuterXml
        End If

        If gvFinancierDetails.Rows.Count > 0 Then
            pnlFinancierDetails1.Visible = True
        Else
            pnlFinancierDetails1.Visible = False
        End If
    End Sub

    Protected Sub gvDriverDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDriverDetails.RowDataBound
        'If e.Row.RowType = DataControlRowType.DataRow Then


        'gvDriverDetails.Rows(0).Cells(7).Text = ddlvehicledrivenby.Items.FindByValue(gvDriverDetails.Rows(e.Row.RowIndex.Cells(7).Text).Text


        'End If
        'End If

    End Sub

    Protected Sub gvDriverDetails_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvDriverDetails.PreRender


    End Sub

    Public Function FormatYYMMDD(ByRef strDate As String) As String
        Dim d, m, y As String
        Dim dt As Date '= Convert.ToDateTime(strDate)

        If strDate = "" Then
            strDate = ""

        Else
            dt = Convert.ToDateTime(strDate)
            d = Format(dt, "dd")
            m = Format(dt, "MM")
            y = Format(dt, "yyy")
            strDate = y & "-" & m & "-" & d
        End If
        Return strDate
    End Function
    Public Function FormatDDMMYY(ByRef strDate As String) As String
        Dim d, m, y As String
        Dim dt As Date '= Convert.ToDateTime(strDate)

        If strDate = "" Then
            strDate = ""

        Else
            dt = Convert.ToDateTime(strDate)
            d = Format(dt, "dd")
            m = Format(dt, "MM")
            y = Format(dt, "yyy")
            strDate = d & "-" & m & "-" & y
        End If
        Return strDate
    End Function
    Protected Sub gvPreviousInsurancehistory_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPreviousInsurancehistory.RowDataBound
        'If (e.Row.RowType = DataControlRowType.Header) Or (e.Row.RowType = DataControlRowType.DataRow) Then
        '    e.Row.Controls(2).Visible = False
        '    e.Row.Controls(3).Visible = False
        '    e.Row.Controls(4).Visible = False
        '    e.Row.Controls(5).Visible = False
        '    e.Row.Controls(6).Visible = False
        'End If
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
    Public Sub BuildXMLFORNEWRECORD(ByRef xmlDatasetElement As XMLElementToAdd) 'As XMLElementToAdd()

        Dim oElementToAddOrUpdate As System.Xml.XmlNode = Nothing
        Dim oElementToAddOrUpdate1 As System.Xml.XmlNode = Nothing
        Dim oElementToAddOrUpdateAdded As System.Xml.XmlNode = Nothing
        Dim newAttribute As System.Xml.XmlAttribute
        'For Each xmlDatasetElement As XMLElementToAdd In xmlDatasetElementsToAdd


        If xmlDatasetElement.ElementName = "PIHISTORY" Then
            oElementToAddOrUpdate1 = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("PREVIOUSINSURANCE") _
                                           .SelectSingleNode("PIHISTORY")
            Dim NewrowCount As Integer
            If oElementToAddOrUpdate1.ParentNode.HasChildNodes Then
                NewrowCount = gvPreviousInsurancehistory.Rows.Count - oElementToAddOrUpdate1.ParentNode.ChildNodes.Count
                'oElementToAddOrUpdate.ParentNode.ChildNodes.Item(oElementToAddOrUpdate.ParentNode.ChildNodes.Count - 1)
            Else
                NewrowCount = 0
            End If
            If NewrowCount > 0 Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                newAttribute = xmlDoc.CreateAttribute("OI")
                Dim strOIIndex As String = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("OI").Value
                strOIIndex = strOIIndex.Remove(0, 2)
                Dim OIindex = Convert.ToInt32(strOIIndex)
                OIindex = OIindex + 1
                strOIIndex = "OI" & OIindex
                newAttribute.Value = strOIIndex
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("US")
                newAttribute.Value = "1"
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_POLICY_BINDER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_POLICY_BINDER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_PREVIOUSINSURANCE_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_PREVIOUSINSURANCE_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_PIHISTORY_ID")
                newAttribute.Value = (Convert.ToInt32(oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_PIHISTORY_ID").Value) + 1).ToString()
                oElementToAddOrUpdate.Attributes.Append(newAttribute)


                If xmlDatasetElement.Attributes.Length > 0 Then


                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    Next
                End If
                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                .SelectSingleNode("RISK_OBJECTS") _
                .SelectSingleNode("MOTOR_POLICY_BINDER") _
                .SelectSingleNode("PREVIOUSINSURANCE") _
                .PrependChild(oElementToAddOrUpdate)

            End If

        End If


        If xmlDatasetElement.ElementName = "FINANCIERDETAILS" Then
            oElementToAddOrUpdate1 = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("FINANCIER") _
                                           .SelectSingleNode("FINANCIERDETAILS")
            Dim NewrowCount As Integer
            If oElementToAddOrUpdate1.ParentNode.HasChildNodes Then
                NewrowCount = gvFinancierDetails.Rows.Count - oElementToAddOrUpdate1.ParentNode.ChildNodes.Count

            Else
                NewrowCount = 0
            End If
            If NewrowCount > 0 Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                newAttribute = xmlDoc.CreateAttribute("OI")
                Dim strOIIndex As String = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("OI").Value
                strOIIndex = strOIIndex.Remove(0, 2)
                Dim OIindex = Convert.ToInt32(strOIIndex)
                OIindex = OIindex + 1
                strOIIndex = "OI" & OIindex
                newAttribute.Value = strOIIndex
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("US")
                newAttribute.Value = "1"
                oElementToAddOrUpdate.Attributes.Append(newAttribute)


                newAttribute = xmlDoc.CreateAttribute("MOTOR_POLICY_BINDER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_POLICY_BINDER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_FINANCIER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_FINANCIER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_FINANCIERDETAILS_ID")
                newAttribute.Value = (Convert.ToInt32(oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_FINANCIERDETAILS_ID").Value) + 1).ToString()
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                If xmlDatasetElement.Attributes.Length > 0 Then


                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    Next
                End If
                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                            .SelectSingleNode("RISK_OBJECTS") _
                                            .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                            .SelectSingleNode("FINANCIER") _
                                            .PrependChild(oElementToAddOrUpdate)

            End If

        End If


        If xmlDatasetElement.ElementName = "NONELECTRICAL" Then
            oElementToAddOrUpdate1 = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("COVERSELECTION") _
                                           .SelectSingleNode("NONELECTRICAL")
            Dim NewrowCount As Integer
            If oElementToAddOrUpdate1.ParentNode.HasChildNodes Then
                NewrowCount = gvNonElecTrical.Rows.Count - oElementToAddOrUpdate1.ParentNode.ChildNodes.Count

            Else
                NewrowCount = 0
            End If
            If NewrowCount > 0 Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                newAttribute = xmlDoc.CreateAttribute("OI")
                Dim strOIIndex As String = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("OI").Value
                strOIIndex = strOIIndex.Remove(0, 2)
                Dim OIindex = Convert.ToInt32(strOIIndex)
                OIindex = OIindex + 1
                strOIIndex = "OI" & OIindex
                newAttribute.Value = strOIIndex
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("US")
                newAttribute.Value = "1"
                oElementToAddOrUpdate.Attributes.Append(newAttribute)


                newAttribute = xmlDoc.CreateAttribute("MOTOR_POLICY_BINDER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_POLICY_BINDER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_COVERSELECTION_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_COVERSELECTION_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_NONELECTRICAL_ID")
                newAttribute.Value = (Convert.ToInt32(oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_NONELECTRICAL_ID").Value) + 1).ToString()
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                If xmlDatasetElement.Attributes.Length > 0 Then


                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    Next
                End If
                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("COVERSELECTION") _
                                            .PrependChild(oElementToAddOrUpdate)

            End If

        End If


        If xmlDatasetElement.ElementName = "ELECTRICAL" Then
            oElementToAddOrUpdate1 = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("COVERSELECTION") _
                                           .SelectSingleNode("ELECTRICAL")
            Dim NewrowCount As Integer
            If oElementToAddOrUpdate1.ParentNode.HasChildNodes Then
                NewrowCount = gvElectricalaccessories.Rows.Count - oElementToAddOrUpdate1.ParentNode.ChildNodes.Count

            Else
                NewrowCount = 0
            End If
            If NewrowCount > 0 Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                newAttribute = xmlDoc.CreateAttribute("OI")
                Dim strOIIndex As String = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("OI").Value
                strOIIndex = strOIIndex.Remove(0, 2)
                Dim OIindex = Convert.ToInt32(strOIIndex)
                OIindex = OIindex + 1
                strOIIndex = "OI" & OIindex
                newAttribute.Value = strOIIndex
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("US")
                newAttribute.Value = "1"
                oElementToAddOrUpdate.Attributes.Append(newAttribute)


                newAttribute = xmlDoc.CreateAttribute("MOTOR_POLICY_BINDER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_POLICY_BINDER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_COVERSELECTION_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_COVERSELECTION_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_ELECTRICAL_ID")
                newAttribute.Value = (Convert.ToInt32(oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_ELECTRICAL_ID").Value) + 1).ToString()
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                If xmlDatasetElement.Attributes.Length > 0 Then


                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    Next
                End If
                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("COVERSELECTION") _
                                            .PrependChild(oElementToAddOrUpdate)

            End If

        End If


        If xmlDatasetElement.ElementName = "DRIVERDETAILS" Then
            oElementToAddOrUpdate1 = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("COVERSELECTION") _
                                           .SelectSingleNode("DRIVERDETAILS")
            Dim NewrowCount As Integer
            If oElementToAddOrUpdate1.ParentNode.HasChildNodes Then
                NewrowCount = gvDriverDetails.Rows.Count - oElementToAddOrUpdate1.ParentNode.ChildNodes.Count

            Else
                NewrowCount = 0
            End If
            If NewrowCount > 0 Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                newAttribute = xmlDoc.CreateAttribute("OI")
                Dim strOIIndex As String = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("OI").Value
                strOIIndex = strOIIndex.Remove(0, 2)
                Dim OIindex = Convert.ToInt32(strOIIndex)
                OIindex = OIindex + 1
                strOIIndex = "OI" & OIindex
                newAttribute.Value = strOIIndex
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("US")
                newAttribute.Value = "1"
                oElementToAddOrUpdate.Attributes.Append(newAttribute)


                newAttribute = xmlDoc.CreateAttribute("MOTOR_POLICY_BINDER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_POLICY_BINDER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_COVERSELECTION_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_COVERSELECTION_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_DRIVERDETAILS_ID")
                newAttribute.Value = (Convert.ToInt32(oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_DRIVERDETAILS_ID").Value) + 1).ToString()
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                If xmlDatasetElement.Attributes.Length > 0 Then


                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    Next
                End If
                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("COVERSELECTION") _
                                            .PrependChild(oElementToAddOrUpdate)

            End If

        End If


        If xmlDatasetElement.ElementName = "PADETAILS" Then
            oElementToAddOrUpdate1 = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("PERSONALACCIDENT") _
                                           .SelectSingleNode("PADETAILS")
            Dim NewrowCount As Integer
            If oElementToAddOrUpdate1.ParentNode.HasChildNodes Then
                NewrowCount = gvpersonalAccidentDetails.Rows.Count - oElementToAddOrUpdate1.ParentNode.ChildNodes.Count

            Else
                NewrowCount = 0
            End If
            If NewrowCount > 0 Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, xmlDatasetElement.ElementName, "")

                newAttribute = xmlDoc.CreateAttribute("OI")
                Dim strOIIndex As String = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("OI").Value
                strOIIndex = strOIIndex.Remove(0, 2)
                Dim OIindex = Convert.ToInt32(strOIIndex)
                OIindex = OIindex + 1
                strOIIndex = "OI" & OIindex
                newAttribute.Value = strOIIndex
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("US")
                newAttribute.Value = "1"
                oElementToAddOrUpdate.Attributes.Append(newAttribute)


                newAttribute = xmlDoc.CreateAttribute("MOTOR_POLICY_BINDER_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_POLICY_BINDER_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_PERSONALACCIDENT_ID")
                newAttribute.Value = oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_PERSONALACCIDENT_ID").Value
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                newAttribute = xmlDoc.CreateAttribute("MOTOR_PADETAILS_ID")
                newAttribute.Value = (Convert.ToInt32(oElementToAddOrUpdate1.ParentNode.LastChild.Attributes("MOTOR_PADETAILS_ID").Value) + 1).ToString()
                oElementToAddOrUpdate.Attributes.Append(newAttribute)

                If xmlDatasetElement.Attributes.Length > 0 Then


                    For Each XMLAttribute As XMLAttributeToAdd In xmlDatasetElement.Attributes
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    Next
                End If
                oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                                           .SelectSingleNode("RISK_OBJECTS") _
                                           .SelectSingleNode("MOTOR_POLICY_BINDER") _
                                           .SelectSingleNode("PERSONALACCIDENT") _
                                            .PrependChild(oElementToAddOrUpdate)

            End If

        End If

        'Next
        'Return xmlDatasetElementsToAdd
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

        Else
            txtLPGSI.Enabled = False
            txtCNGSI.Enabled = True
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

    Protected Sub ChkTrailer_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ChkTrailer.CheckedChanged, _
        chkGoodfeatureDiscount.CheckedChanged, _
        chkSpecialDiscount.CheckedChanged, _
        chkMeetscompulsoryownerdriverCompulsoryPACoverrequirements.CheckedChanged, _
        chkNonConvectionalPower.CheckedChanged, _
        chkThisvechcle.CheckedChanged, _
        chkPersonalaccidentcoverforunnamedpersons.CheckedChanged, _
        chkDoyouwishtoincludePersonalaccidentforpaiddrivercleaner.CheckedChanged, _
        chkVehicleInspection.CheckedChanged, _
        ChkTrailer.CheckedChanged, _
        chkLPGkit.CheckedChanged, _
        chkCNGSI.CheckedChanged, _
        chkVechileusedforrallies.CheckedChanged, _
        chkGeographical.CheckedChanged, _
        chkBrokerageAgencyCommission.CheckedChanged

        EnableControl()

    End Sub

    Protected Sub btnfinanciercancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnfinanciercancel.Click
        pnlfinancierDetails.Visible = False
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

    Protected Sub btnFinancierEdit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFinancierEdit.Click

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

    Protected Sub txtExShowRoom_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExShowRoom.TextChanged
        'txtIDV.Text = txtExShowRoom.Text
        'txtIDVSystem.Text = txtExShowRoom.Text
    End Sub

    Protected Sub gvFinancierDetails_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFinancierDetails.RowDataBound

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
            If dt.Rows(iCount).Item(ColumnName).ToString <> "" Then
                SumInsured += dt.Rows(iCount).Item(ColumnName)
            End If

        Next
        Return SumInsured.ToString()
    End Function

    Public Sub BuildQuickQuote(ByRef xmldoc As XmlDocument)
        Dim oElement_DATA_SET As System.Xml.XmlNode = xmldoc.SelectSingleNode("DATA_SET")

        If oElement_DATA_SET IsNot Nothing Then

            Dim oElement_RISK_OBJECTS As System.Xml.XmlNode = xmldoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS")

            If oElement_RISK_OBJECTS IsNot Nothing Then

                Dim oElement_MOTOR_POLICY_BINDER As System.Xml.XmlNode = xmldoc.SelectSingleNode("DATA_SET").SelectSingleNode("RISK_OBJECTS").SelectSingleNode("MOTOR_POLICY_BINDER")

                If oElement_MOTOR_POLICY_BINDER IsNot Nothing Then




                    If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").HasAttribute("QUICK_QUOTE") Then
                        If oElement_MOTOR_POLICY_BINDER("COVERSELECTION").Attributes("QUICK_QUOTE").Value <> 1 Then
                            Response.Redirect("MotorEditRisk.aspx")
                        End If
                    Else
                        Response.Redirect("MotorEditRisk.aspx")
                    End If




                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_MAKE") Then
                        ddlVehicleMake.SelectedItem.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_MAKE").Value
                    End If

                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_MODEL") Then
                        txtVehicleModel.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_MODEL").Value
                    End If

                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("BODY_TYPE") Then
                        txtBodyType.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("BODY_TYPE").Value
                    End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("SEATING_CAPACITY") Then
                        txtSeatingcapacity.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("SEATING_CAPACITY").Value
                    End If



                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("CUBIC_CAPACITY") Then
                        txtCC.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("CUBIC_CAPACITY").Value
                    End If



                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("EX_SHOWROOM_PRICE") Then
                        txtExShowRoom.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("EX_SHOWROOM_PRICE").Value
                    End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("INSURED_DECLARED_VALUE") Then
                        txtIDV.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("INSURED_DECLARED_VALUE").Value
                    End If


                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_REGISTERED") Then
                    '    ddlVechicleRegisteredinIndividualorCorparateName.Items.FindByValue(oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_REGISTERED").Value).Selected = True
                    'End If

                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("NEW_VEHICLE") Then
                        chkNewVehicle.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("NEW_VEHICLE").Value
                    End If


                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("RENEWAL_OF_EXISTING_POLICY") Then
                    '    chkRenewalofExistingPolicy.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("RENEWAL_OF_EXISTING_POLICY").Value
                    'End If

                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("RENEWAL_OF_EXISTING_POLICY") Then
                    '    txtYearOfManufacture.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("RENEWAL_OF_EXISTING_POLICY").Value
                    'End If



                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("DATE_OF_FIRST_REGISTRATION") Then
                    '    txtDateofFirstRegistration.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("DATE_OF_FIRST_REGISTRATION").Value
                    'End If

                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("COVER_TYPE") Then
                        ddlPolicyType.SelectedValue = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("COVER_TYPE").Value
                    End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("RTO_PIN_CODE") Then
                        txtPinCode.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("RTO_PIN_CODE").Value
                    End If



                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("FC2_USED") Then
                    '    hdnFC2.Value = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("FC2_USED").Value
                    'End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_REG_STATE") Then
                        txtState.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_REG_STATE").Value
                    End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VEHICLE_ZONE") Then
                        txtZone.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VEHICLE_ZONE").Value
                    End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("ADD_ON_COVERS") Then
                        chkAddonCover.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("ADD_ON_COVERS").Value

                    End If

                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("AA_DISC") Then
                    '    chkAutomobileAssociationMembership.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("AA_DISC").Value
                    'End If

                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("ARAI_ANTI_THEFT") Then
                    '    chkARAIApprovedAntiTheftDeviceInstalled.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("ARAI_ANTI_THEFT").Value
                    'End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("VOLUNTARY_DEDUCTIBLE") Then
                        ddlVoluntaryExcessamount.SelectedItem.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("VOLUNTARY_DEDUCTIBLE").Value
                    End If

                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("LEGAL_LIABILITY_FOR_PAID_DRIVER") Then
                    '    chkLegalLiablityforpaidDriver.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("LEGAL_LIABILITY_FOR_PAID_DRIVER").Value
                    'End If



                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("EE_ACCESSORIES") Then
                    '    txtElecticalElectronicAccessories.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("EE_ACCESSORIES").Value
                    'End If

                    'If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("NON_E_ACCESSORIES") Then
                    '    txtNonElecticalAccessories.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("NON_E_ACCESSORIES").Value
                    'End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("PA_COVER") Then
                        chkPersonalaccidentcoverforunnamedpersons.Checked = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("PA_COVER").Value

                    End If


                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("NO_OF_PERSONS") Then
                        txtNumberOfunnamedPersons.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("NO_OF_PERSONS").Value
                    End If

                    If oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").HasAttribute("SUM_INSURED_PER_PERSON") Then
                        txtSumInsuredPerPerson.Text = oElement_MOTOR_POLICY_BINDER("QUICKQUOTE").Attributes("SUM_INSURED_PER_PERSON").Value
                    End If

                End If
            End If
        End If

    End Sub

    Protected Sub txtIDV_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtIDV.TextChanged
 


        If txtIDV.Text.Length > 0 AndAlso CInt(txtIDV.Text) > CInt(txtExShowRoom.Text) Then
            txtIDV.Text = txtExShowRoom.Text
        End If
        txtIDVSystem.Text = txtIDV.Text
    End Sub
End Class
