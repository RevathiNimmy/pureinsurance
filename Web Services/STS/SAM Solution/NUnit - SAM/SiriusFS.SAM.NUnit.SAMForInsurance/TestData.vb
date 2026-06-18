Friend Class TestData

#Region " Private Declarations "

    Private m_oLogFile As New System.IO.StreamWriter(My.Settings.Item("XMLTestDataFileName").ToString & ".log")
    Private m_nClaimKey As Integer
    Private m_sBranchCode As String
    Private m_sAnalysisCode As String
    Private m_sUpdateAnalysisCode As String
    Private m_sSubBranch As String
    Private m_nAgentKey As Integer
    Private m_sUserName As String
    Private m_sProductCode As String
    Private m_dtDOB As Date
    Private m_dtCoverStart As Date
    Private m_dtCoverEnd As Date
    Private m_sQuoteDescription As String
    Private m_sListCode As String
    Private m_nListType As Integer
    Private m_sDataModelCode As String
    Private m_sClaimDataModelCode As String
    Private m_sRiskTypeCode As String
    Private m_sScreenCode As String
    Private m_sRiskDescription As String
    Private m_sXMLDataSetElementToAdd As String
    Private m_sAddressLine1 As String
    Private m_sAddressLine2 As String
    Private m_sAddressLine3 As String
    Private m_sAddressLine4 As String
    Private m_sPostCode As String
    Private m_nAddressTypeCode As Integer
    Private m_sCountryCode As String
    Private m_sSurname As String
    Private m_sForename As String
    Private m_sInitials As String
    Private m_sTitle As String
    Private m_sTPIntroducer As String
    Private m_sTPUser As String
    Private m_nMaritalStatusCode As Integer
    Private m_nEmploymentStatusCode As Integer
    Private m_sAlternativeId As String
    Private m_sEmployerBusinessCode As String
    Private m_sOccupationCode As String
    Private m_sGenderCode As String
    Private m_sCompanyName As String
    Private m_sTradingName As String
    Private m_sBusinessCode As String
    Private m_bRunDefaultRules As Boolean
    Private m_sInvalidLookupCode As String
    Private m_sPassword As String
    Private m_sNewPassword As String
    Private m_nInsuranceFolderCnt As Integer
    Private m_nInsuranceFileCnt As Integer
    Private m_sInsuranceFileRef As String
    Private m_sNewInsuranceFileRef As String
    Private m_nPartyCnt As Integer
    Private m_nRiskCnt As Integer
    Private m_sFPAddressLine1 As String
    Private m_sFPAddressLine2 As String
    Private m_sFPAddressLine3 As String
    Private m_sFPAddressLine4 As String
    Private m_sFPAlternativeID As String
    Private m_sFPAreaCode As String
    Private m_dtFPDOB As Date
    Private m_sFPFirstName As String
    Private m_sFPName As String
    Private m_sFPPartyType As String
    Private m_sFPPolicyRef As String
    Private m_sFPPostCode As String
    Private m_sFPRiskIndex As String
    Private m_sFPShortName As String
    Private m_sFPTelephoneNumber As String
    Private m_sDocumentTemplateCode As String
    Private m_nGenerateDocumentMode As Integer
    Private m_bGenerateDocumentOutputAsHTML As Boolean
    Private m_sGenerateDocumentParameterXML As String
    Private m_sRiskDataXML As String
    Private m_nInvalidCnt As Integer
    Private m_nInvalidInsFileFolderCnt As Integer
    Private m_nInvalidInsFileRiskCnt As Integer
    Private m_vAddresses() As Address
    Private m_vContacts() As Contact
    Private m_oOpenClaimXMLStructure As OpenClaimXMLStructure
    Private m_oMaintainClaimXMLStructure As MaintainClaimXMLStructure
    Private m_oAddAgentReceiptXMLStructure As AddAgentReceiptXMLStructure
    Private m_oPostDocumentXMLStructure As PostDocumentXMLStructure
    Private m_oClientDataImportXMLStructure As ClientDataImportTestDataXMLStruture
    Private m_oFindInsuranceFileForClaimsTestData As FindInsuranceFileForClaimsTestData
    Private m_oPayClaimStructure As cClaimPayment  'GAURAV
    Private m_iPaymentMethod As Int32 = 0
    Private m_oUpdatePartyOtherTestData As UpdatePartyOtherTestData

    'vivek
    Private m_vGenerateClaimDoc() As GenerateClaims

    Private m_vClaimReceipt As ClaimReceiptTaxes

    Private m_vElementsToAdd As XMLElementToAdd()
    Private m_vMTAElementsToAdd As XMLElementToAdd()
    Private m_vClaimsElementsToAdd As XMLElementToAdd()

    Private m_sFindClaimBranchCode As String
    Private m_sFindClaimClaimNumber As String
    Private m_sFindClaimInsuranceFileRef As String
    Private m_sFindClaimClientShortName As String
    Private m_dtFindClaimLossDateFrom As Date
    Private m_dtFindClaimLossDateTo As Date
    Private m_sFindClaimRiskIndex As String
    Private m_vGenerateClaimDocument() As GenerateClaims
    Private m_sClaimMTABranchCode As String
    Private m_nClaimMTAInsuranceFolderKey As Integer
    Private m_nClaimMTAInsuranceFileKey As Integer
    Private m_nClaimMTARiskKey As Integer
    Private m_sClaimMTAScreenCode As String
    Private m_sClaimMTARiskDescription As String
    Private m_sClaimMTAXMLDataSet As String
    Private m_lGetClaimDetailsClaimKey As Integer
    Private m_sGetClaimDetailsBranchCode As String
    Private m_lGetClaimRiskClaimKey As Integer
    Private m_sGetClaimRiskBranchCode As String

    Private m_vConvictions() As Conviction
    Private m_vAccidents() As Accident
    Private m_vOtherPartyInfo As OtherPartyInfo
    Private m_vSuppBusiness() As SupplierBusiness

    Private m_lClaimKey As Integer

    Private m_iGetOptionSettingNumber As Integer
    Private m_iGetOptionSettingType As Integer

#End Region

#Region " Private Methods "

    Friend Overloads Function CheckAttribute(ByVal oNode As XmlNode, _
                        ByVal sAttrName As String, _
                        Optional ByVal sDefault As String = "") As String

        If oNode Is Nothing Then
            m_oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
            Return ""
        Else
            m_oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                m_oLogFile.WriteLine(" not found - default returned <" & sDefault & ">")
                Return sDefault
            Else
                m_oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & ">")
                Return oNode.Attributes(sAttrName).Value
            End If

        End If

    End Function

    Friend Overloads Function CheckAttribute(ByVal oNode As XmlNode, _
                        ByVal sAttrName As String, _
                        ByVal dtDefault As Date) As Date

        If oNode Is Nothing Then
            m_oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
        Else
            m_oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                m_oLogFile.WriteLine(" not found - default returned <" & dtDefault & ">")
                Return dtDefault
            Else
                If Not IsDate(oNode.Attributes(sAttrName).Value) Then
                    m_oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & "> - INVALID DATE!")
                    Err.Raise(vbObjectError + 513, , "The attribute '" & sAttrName & "' in the testdata XML file within node '" & oNode.Name.ToString & "' is not a valid date.")
                    Return ""
                Else
                    m_oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & ">")
                    Return CDate(oNode.Attributes(sAttrName).Value)
                End If
            End If

        End If

    End Function

    Friend Overloads Function CheckAttribute(ByVal oNode As XmlNode, _
                        ByVal sAttrName As String, _
                        ByVal iDefault As Integer) As Integer

        Dim iValue As Integer

        If oNode Is Nothing Then
            m_oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
        Else
            m_oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                m_oLogFile.WriteLine(" not found - default returned <" & iDefault & ">")
                Return iDefault
            Else
                If Not Int32.TryParse(oNode.Attributes(sAttrName).Value, iValue) Then
                    m_oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & "> - INVALID INTEGER!")
                    Err.Raise(vbObjectError + 513, , "The attribute '" & sAttrName & "' in the testdata XML file within node '" & oNode.Name.ToString & "' is not a valid integer.")
                    Return iDefault
                Else
                    Return iValue
                End If
            End If

        End If

    End Function


    Friend Overloads Function CheckAttribute(ByVal oNode As XmlNode, _
                        ByVal sAttrName As String, _
                        ByVal bDefault As Boolean) As Boolean

        Dim bValue As Boolean

        If oNode Is Nothing Then
            m_oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
        Else
            m_oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                m_oLogFile.WriteLine(" not found - default returned <" & bDefault & ">")
                Return bDefault
            Else
                If Not Boolean.TryParse(oNode.Attributes(sAttrName).Value, bValue) Then
                    m_oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & "> - INVALID BOOLEAN!")
                    Err.Raise(vbObjectError + 513, , "The attribute '" & sAttrName & "' in the testdata XML file within node '" & oNode.Name.ToString & "' is not a valid boolean.")
                    Return bDefault
                Else
                    Return bValue
                End If
            End If

        End If

    End Function

    Friend Overloads Function CheckAttribute(ByVal oNode As XmlNode, _
                        ByVal sAttrName As String, _
                        ByVal dDefault As Decimal) As Decimal

        Dim dValue As Decimal

        If oNode Is Nothing Then
            m_oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
        Else
            m_oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                m_oLogFile.WriteLine(" not found - default returned <" & dDefault & ">")
                Return dDefault
            Else
                If Not Decimal.TryParse(oNode.Attributes(sAttrName).Value, dValue) Then
                    m_oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & "> - INVALID DECIMAL!")
                    Err.Raise(vbObjectError + 513, , "The attribute '" & sAttrName & "' in the testdata XML file within node '" & oNode.Name.ToString & "' is not a valid decimal.")
                    Return dDefault
                Else
                    Return dValue
                End If
            End If

        End If

    End Function


    'Friend Sub LogDataTypeError( _
    'ByVal sNode As String, _
    'ByVal sAttribute As String, _
    'ByVal sExpectedDataType As String, _
    'ByVal sValue As String)

    '    m_oLogFile.WriteLine("<INVALID DATA FORMAT>" & _
    '    "<" & sNode & " " & sAttribute & ">" & _
    '     " Expected to be of type " & sExpectedDataType & _
    '     ". Actual value is " & sValue)

    'End Sub





#End Region

#Region " Public Methods "

    Public Sub New()

        Dim sXMLDoc As String = My.Computer.FileSystem.ReadAllText(My.Settings.Item("XMLTestDataFileName").ToString)
        Dim oXML As New System.Xml.XmlDocument
        Dim oDataNode As XmlNode

        m_oLogFile.WriteLine("Test Run: " & Now)

        Try
            oXML.LoadXml(sXMLDoc)
            oDataNode = oXML.SelectSingleNode("TEST_DATA")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA> not found")
            End If

            'm_lClaimKey = CheckAttribute(oDataNode, "ClaimKey") 'GAURAV
            m_sAnalysisCode = CheckAttribute(oDataNode, "AnalysisCode")
            m_sUpdateAnalysisCode = CheckAttribute(oDataNode, "UpdateAnalysisCode")
            m_sBranchCode = CheckAttribute(oDataNode, "BranchCode")
            m_sSubBranch = CheckAttribute(oDataNode, "SubBranch")
            m_nAgentKey = CheckAttribute(oDataNode, "AgentKey")
            m_sUserName = CheckAttribute(oDataNode, "UserName")
            m_sProductCode = CheckAttribute(oDataNode, "ProductCode")
            m_dtCoverStart = CheckAttribute(oDataNode, "CoverStartDate", Today)
            m_dtCoverEnd = CheckAttribute(oDataNode, "CoverEndDate", DateAdd(DateInterval.Year, 1, DateAdd(DateInterval.Day, -1, Today)))
            m_sQuoteDescription = CheckAttribute(oDataNode, "QuoteDescription", "Quote created: " & Now)
            m_sRiskDescription = CheckAttribute(oDataNode, "RiskDescription", "Risk Added: " & Now)
            m_sDataModelCode = CheckAttribute(oDataNode, "DataModelCode")
            m_sClaimDataModelCode = CheckAttribute(oDataNode, "ClaimDataModelCode")
            m_sRiskTypeCode = CheckAttribute(oDataNode, "RiskTypeCode")
            m_sScreenCode = CheckAttribute(oDataNode, "ScreenCode")
            m_dtDOB = CheckAttribute(oDataNode, "DOB", New Date)
            m_sListCode = CheckAttribute(oDataNode, "ListCode")
            m_nListType = CheckAttribute(oDataNode, "ListType", "2") ' PMLookup
            m_nAddressTypeCode = CheckAttribute(oDataNode, "AddressTypeCode")
            m_sAddressLine1 = CheckAttribute(oDataNode, "AddressLine1")
            m_sAddressLine2 = CheckAttribute(oDataNode, "AddressLine2")
            m_sAddressLine3 = CheckAttribute(oDataNode, "AddressLine3")
            m_sAddressLine4 = CheckAttribute(oDataNode, "AddressLine4")
            m_sPostCode = CheckAttribute(oDataNode, "PostCode")
            m_sCountryCode = CheckAttribute(oDataNode, "CountryCode")
            m_sSurname = CheckAttribute(oDataNode, "Surname")
            m_sForename = CheckAttribute(oDataNode, "Forename")
            m_sInitials = CheckAttribute(oDataNode, "Initials")
            m_sTitle = CheckAttribute(oDataNode, "Title")
            m_sGenderCode = CheckAttribute(oDataNode, "GenderCode")
            m_sTPIntroducer = CheckAttribute(oDataNode, "TPIntroducer")
            m_sTPUser = CheckAttribute(oDataNode, "TPUser")
            m_nMaritalStatusCode = CheckAttribute(oDataNode, "MaritalStatusCode")
            m_nEmploymentStatusCode = CheckAttribute(oDataNode, "EmploymentStatusCode")
            m_sAlternativeId = CheckAttribute(oDataNode, "AlternativeID")
            m_sEmployerBusinessCode = CheckAttribute(oDataNode, "EmployerBusinessCode")
            m_sOccupationCode = CheckAttribute(oDataNode, "OccupationCode")
            m_sCompanyName = CheckAttribute(oDataNode, "CompanyName")
            m_sTradingName = CheckAttribute(oDataNode, "TradingName")
            m_sBusinessCode = CheckAttribute(oDataNode, "BusinessCode")
            m_bRunDefaultRules = (CheckAttribute(oDataNode, "RunDefaultRules", "1") = "1")
            m_sInvalidLookupCode = CheckAttribute(oDataNode, "InvalidLookupCode", "INVALIDXXX")
            m_sPassword = CheckAttribute(oDataNode, "Password")
            m_sNewPassword = CheckAttribute(oDataNode, "NewPassword")
            m_sFPAddressLine1 = CheckAttribute(oDataNode, "FindPartyAddressLine1")
            m_sFPAddressLine2 = CheckAttribute(oDataNode, "FindPartyAddressLine2")
            m_sFPAddressLine3 = CheckAttribute(oDataNode, "FindPartyAddressLine3")
            m_sFPAddressLine4 = CheckAttribute(oDataNode, "FindPartyAddressLine4")
            m_sFPAlternativeID = CheckAttribute(oDataNode, "FindPartyAlternativeId")
            m_sFPAreaCode = CheckAttribute(oDataNode, "FindPartyAreaCode")
            m_dtFPDOB = CheckAttribute(oDataNode, "FindPartyDateOfBirth", New Date)
            m_sFPFirstName = CheckAttribute(oDataNode, "FindPartyFirstName")
            m_sFPName = CheckAttribute(oDataNode, "FindPartyName")
            m_sFPPartyType = CheckAttribute(oDataNode, "FindPartyPartyType")
            m_sFPPolicyRef = CheckAttribute(oDataNode, "FindPartyPolicyRef")
            m_sFPPostCode = CheckAttribute(oDataNode, "FindPartyPostCode")
            m_sFPRiskIndex = CheckAttribute(oDataNode, "FindPartyRiskIndex")
            m_sFPShortName = CheckAttribute(oDataNode, "FindPartyShortName")
            m_sFPTelephoneNumber = CheckAttribute(oDataNode, "FindPartyTelephoneNumber")
            m_nInsuranceFolderCnt = CheckAttribute(oDataNode, "InsuranceFolderCnt", "0")
            m_nInsuranceFileCnt = CheckAttribute(oDataNode, "InsuranceFileCnt", "0")
            m_sInsuranceFileRef = CheckAttribute(oDataNode, "InsuranceFileRef")
            m_sNewInsuranceFileRef = CheckAttribute(oDataNode, "NewInsuranceFileRef")
            m_nPartyCnt = CheckAttribute(oDataNode, "PartyCnt", "0")
            m_nRiskCnt = CheckAttribute(oDataNode, "RiskCnt", "0")
            m_nInvalidCnt = CheckAttribute(oDataNode, "InvalidCnt", "-1")
            m_nInvalidInsFileFolderCnt = CheckAttribute(oDataNode, "InvalidInsFileFolderCnt", "1")
            m_nInvalidInsFileRiskCnt = CheckAttribute(oDataNode, "InvalidInsFileRiskCnt", "1")
            m_sDocumentTemplateCode = CheckAttribute(oDataNode, "DocumentTemplateCode")
            m_nGenerateDocumentMode = CheckAttribute(oDataNode, "GenerateDocumentMode", "0")
            m_bGenerateDocumentOutputAsHTML = (CheckAttribute(oDataNode, "GenerateDocumentOutputAsHTML", "0") = "1")
            m_sGenerateDocumentParameterXML = CheckAttribute(oDataNode, "GenerateDocumentParameterXML")
            m_iPaymentMethod = CheckAttribute(oDataNode, "PaymentMethod", 0)
            m_iGetOptionSettingNumber = CheckAttribute(oDataNode, "GetOptionSettingNumber", 1)
            If CheckAttribute(oDataNode, "GetOptionSettingType", "") = "ProductOption" Then
                m_iGetOptionSettingType = 0
            Else
                m_iGetOptionSettingType = 1
            End If

            m_lGetClaimRiskClaimKey = CheckAttribute(oDataNode, "GetClaimRiskClaimKey")
            m_sGetClaimRiskBranchCode = CheckAttribute(oDataNode, "GetClaimRiskBranchCode")

            m_sClaimMTABranchCode = CheckAttribute(oDataNode, "ClaimMTABranchCode")
            m_nClaimMTAInsuranceFolderKey = CheckAttribute(oDataNode, "ClaimMTAInsuranceFolderKey", "0")
            m_nClaimMTAInsuranceFileKey = CheckAttribute(oDataNode, "ClaimMTAInsuranceFileKey", "0")
            m_nClaimMTARiskKey = CheckAttribute(oDataNode, "ClaimMTARiskKey", "0")
            m_sClaimMTAScreenCode = CheckAttribute(oDataNode, "ClaimMTAScreenCode")
            m_sClaimMTARiskDescription = CheckAttribute(oDataNode, "ClaimMTARiskDescription")
            Try
                m_sClaimMTAXMLDataSet = My.Computer.FileSystem.ReadAllText(My.Settings.Item("ClaimMTAXMLDataSetFileName").ToString)
            Catch SettingNotFound As System.Configuration.SettingsPropertyNotFoundException
                m_oLogFile.WriteLine("Configuration setting not found : ClaimMTAXMLDataSetFileName")
            Catch ex As Exception
                m_oLogFile.WriteLine("Error attempting to read file :" & My.Settings.Item("ClaimMTAXMLDataSetFileName").ToString & vbCrLf * "Error message : " & ex.Message)
            End Try

            Try
                m_sRiskDataXML = My.Computer.FileSystem.ReadAllText(My.Settings.Item("XMLTestRiskDataFileName").ToString)
            Catch SettingNotFound As System.Configuration.SettingsPropertyNotFoundException
                m_oLogFile.WriteLine("Configuration setting not found : XMLTestRiskDataFileName")
            Catch ex As Exception
                m_oLogFile.WriteLine("Error attempting to read file :" & My.Settings.Item("XMLTestRiskDataFileName").ToString & vbCrLf * "Error message : " & ex.Message)
            End Try

            ' Set up Attributes that will be added to the data model XML
            Dim oXMLElement As XMLElementToAdd
            Dim oXMLAttr As XMLAttributeToAdd

            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                        .SelectSingleNode("DATA_SET_ELEMENTS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><DATA_SET_ELEMENTS> not found")
            End If
            ReDim m_vElementsToAdd(oDataNode.ChildNodes.Count - 1)
            For iElementCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                oXMLElement = New XMLElementToAdd
                oXMLElement.ElementName = CheckAttribute(oDataNode.ChildNodes(iElementCnt), "Name")
                ReDim oXMLElement.Attributes(oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1)
                For iAttrCnt As Integer = 0 To oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1
                    'If oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt).Name = "DATA_SET_ELEMENT" Then
                    '    oXMLElement = New XMLElementToAdd
                    '    oXMLElement.ElementName = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Name")
                    '    ReDim oXMLElement.Attributes(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt).ChildNodes.Count - 1)
                    '    For iAttrCnt2 As Integer = 0 To oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt).ChildNodes.Count - 1
                    '        oXMLAttr = New XMLAttributeToAdd
                    '        oXMLAttr.AttributeName = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt).ChildNodes(iAttrCnt2), "Name")
                    '        oXMLAttr.AttributeValue = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt).ChildNodes(iAttrCnt2), "Value")
                    '        oXMLElement.Attributes(iAttrCnt) = oXMLAttr
                    '    Next
                    '    oXMLElement()
                    'Else
                    oXMLAttr = New XMLAttributeToAdd
                    oXMLAttr.AttributeName = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Name")
                    oXMLAttr.AttributeValue = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Value")
                    oXMLElement.Attributes(iAttrCnt) = oXMLAttr
                    'End If
                Next
                m_vElementsToAdd(iElementCnt) = oXMLElement
            Next

            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                        .SelectSingleNode("MTA_DATA_SET_ELEMENTS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><MTA_DATA_SET_ELEMENTS> not found")
            End If
            ReDim m_vMTAElementsToAdd(oDataNode.ChildNodes.Count - 1)
            For iElementCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                oXMLElement = New XMLElementToAdd
                oXMLElement.ElementName = CheckAttribute(oDataNode.ChildNodes(iElementCnt), "Name")
                ReDim oXMLElement.Attributes(oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1)
                For iAttrCnt As Integer = 0 To oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1
                    oXMLAttr = New XMLAttributeToAdd
                    oXMLAttr.AttributeName = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Name")
                    oXMLAttr.AttributeValue = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Value")
                    oXMLElement.Attributes(iAttrCnt) = oXMLAttr
                Next
                m_vMTAElementsToAdd(iElementCnt) = oXMLElement
            Next

            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                        .SelectSingleNode("CLAIMS_DATA_SET_ELEMENTS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><CLAIMS_DATA_SET_ELEMENTS> not found")
            End If
            ReDim m_vClaimsElementsToAdd(oDataNode.ChildNodes.Count - 1)
            For iElementCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                oXMLElement = New XMLElementToAdd
                oXMLElement.ElementName = CheckAttribute(oDataNode.ChildNodes(iElementCnt), "Name")
                ReDim oXMLElement.Attributes(oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1)
                For iAttrCnt As Integer = 0 To oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1
                    oXMLAttr = New XMLAttributeToAdd
                    oXMLAttr.AttributeName = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Name")
                    oXMLAttr.AttributeValue = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Value")
                    oXMLElement.Attributes(iAttrCnt) = oXMLAttr
                Next
                m_vClaimsElementsToAdd(iElementCnt) = oXMLElement
            Next

            ' Set up addresses
            Dim oAddress As Address

            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                        .SelectSingleNode("ADDRESSES")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><ADDRESSES> not found")
            Else
                ReDim m_vAddresses(oDataNode.ChildNodes.Count - 1)
                For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                    oAddress = New Address
                    oAddress.TypeCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "Type")
                    oAddress.Line1 = CheckAttribute(oDataNode.ChildNodes(iCnt), "Line1")
                    oAddress.Line2 = CheckAttribute(oDataNode.ChildNodes(iCnt), "Line2")
                    oAddress.Line3 = CheckAttribute(oDataNode.ChildNodes(iCnt), "Line3")
                    oAddress.Line4 = CheckAttribute(oDataNode.ChildNodes(iCnt), "Line4")
                    oAddress.PostCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "PostCode")
                    oAddress.CountryCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "CountryCode")

                    Dim oAddContact As Contact
                    ReDim oAddress.Contact(oDataNode.ChildNodes(iCnt).ChildNodes.Count - 1)
                    Dim oXMLNode As XmlNode
                    'Dim SelNode As XmlNode
                    oXMLNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("ADDRESSES").SelectSingleNode("ADDRESS").SelectSingleNode("CONTACT")
                    If oXMLNode IsNot Nothing Then
                        Dim contactsCnt As Integer = 0
                        For ContactCNT As Integer = 0 To oDataNode.ChildNodes(iCnt).ChildNodes.Count - 1
                            'For Each oXMLNode In oDataNode.ChildNodes(iCnt).ChildNodes
                            oAddContact = New Contact
                            oAddContact.AreaCode = oXMLNode.Attributes.GetNamedItem("AreaCode").Value
                            oAddContact.Type = oXMLNode.Attributes.GetNamedItem("ContactTypeCode").Value
                            If oAddContact.Type <> 0 Then
                                oAddContact.ElementName = ProxyWS.ItemChoiceType.Number
                            Else
                                oAddContact.ElementName = ProxyWS.ItemChoiceType.EmailAddress
                            End If
                            oAddContact.Item = oXMLNode.Attributes.GetNamedItem("ContactDetail").Value
                            oAddress.Contact(contactsCnt) = oAddContact
                            contactsCnt += 1
                            ''oXMLNode = oXML.NextSibling("CONTACT")
                        Next
                    End If
                    m_vAddresses(iCnt) = oAddress
                Next
            End If

            ' Set up contacts
            Dim oContact As Contact

            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                        .SelectSingleNode("CONTACTS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><CONTACTS> not found")
            Else
                ReDim m_vContacts(oDataNode.ChildNodes.Count - 1)
                For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                    oContact = New Contact
                    oContact.Type = CheckAttribute(oDataNode.ChildNodes(iCnt), "Type")
                    If oContact.Type <> 0 Then
                        oContact.ElementName = ProxyWS.ItemChoiceType.Number
                    Else
                        oContact.ElementName = ProxyWS.ItemChoiceType.EmailAddress
                    End If
                    oContact.AreaCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "AreaCode")
                    oContact.Item = CheckAttribute(oDataNode.ChildNodes(iCnt), "Item")
                    m_vContacts(iCnt) = oContact
                Next
            End If
            'Dim oGenerateClaimDocument As New GenerateClaims

            '         oDataNode = oXML.SelectSingleNode("TEST_DATA") _
            '                   .SelectSingleNode("GENERATECLAIM")
            '         If oDataNode Is Nothing Then
            '             m_oLogFile.WriteLine("<TEST_DATA><GENERATECLAIMS> not found")
            '         Else
            '             ReDim m_vGenerateClaimDocument(oDataNode.ChildNodes.Count - 1)
            '             For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
            '                 oGenerateClaimDocument = New GenerateClaims
            '                 oGenerateClaimDocument.Mode = CheckAttribute(oDataNode.ChildNodes(iCnt), "Mode")
            '                 oGenerateClaimDocument.ClaimKey = CheckAttribute(oDataNode.ChildNodes(iCnt), "ClaimKey")
            '                 oGenerateClaimDocument.TransactionType = CheckAttribute(oDataNode.ChildNodes(iCnt), "TransactionType")
            '                 oGenerateClaimDocument.ParameterXML = CheckAttribute(oDataNode.ChildNodes(iCnt), "ParameterXML", 0)
            '                 oGenerateClaimDocument.OutputAsHTML = CheckAttribute(oDataNode.ChildNodes(iCnt), "OutputAsHTML", 1)
            '                 m_vGenerateClaimDocument(iCnt) = oGenerateClaimDocument
            '             Next
            '         End If
            'Dim oClaimReceipt As New ClaimReceiptAndTaxes
            'oDataNode = oXML.SelectSingleNode("TEST_DATA") _
            '          .SelectSingleNode("CLAIMRECEIPTDETAILS")
            'If oDataNode Is Nothing Then
            '    m_oLogFile.WriteLine("<TEST_DATA><CLAIMRECEIPTDETAILS> not found")
            'Else
            '    oClaimReceipt = New ClaimReceiptAndTaxes
            '    oClaimReceipt.AddressLine1 = CheckAttribute(oDataNode, "AddressLine1")
            '    oClaimReceipt.AddressLine2 = CheckAttribute(oDataNode, "AddressLine2")
            '    oClaimReceipt.AddressLine3 = CheckAttribute(oDataNode, "AddressLine3")
            '    oClaimReceipt.AddressLine4 = CheckAttribute(oDataNode, "AddressLine4")
            '    oClaimReceipt.PostCode = CheckAttribute(oDataNode, "PostCode")
            '    oClaimReceipt.CountryCode = CheckAttribute(oDataNode, "CountryCode")
            '    oClaimReceipt.TheirReference = CheckAttribute(oDataNode, "TheirReference")
            '    oClaimReceipt.BankCode = CheckAttribute(oDataNode, "BankCode")
            '    oClaimReceipt.BankName = CheckAttribute(oDataNode, "BankName")
            '    oClaimReceipt.BankNumber = CheckAttribute(oDataNode, "BankNumber")
            '    oClaimReceipt.Name = CheckAttribute(oDataNode, "Name")
            '    oClaimReceipt.MediaReference = CheckAttribute(oDataNode, "MediaReference")
            '    oClaimReceipt.MediaTypeCode = CheckAttribute(oDataNode, "MediaTypeCode")
            '    oClaimReceipt.InsuredDomiciled = CheckAttribute(oDataNode, "InsuredDomiciled")
            '    oClaimReceipt.InsuredPercentage = CheckAttribute(oDataNode, "InsuredPercentage")
            '    oClaimReceipt.InsuredTaxNumber = CheckAttribute(oDataNode, "InsuredTaxNumber")
            '    oClaimReceipt.IsSettlement = CheckAttribute(oDataNode, "IsSettlement")
            '    oClaimReceipt.IsTaxExempt = CheckAttribute(oDataNode, "IsTaxExempt")
            '    oClaimReceipt.ReceivableTaxPercentage = CheckAttribute(oDataNode, "ReceivableTaxPercentage")
            '    oClaimReceipt.BaseClaimKey = CheckAttribute(oDataNode, "BaseClaimKey")
            '    oClaimReceipt.BaseClaimPerilKey = CheckAttribute(oDataNode, "BaseClaimPerilKey")
            '    oClaimReceipt.ClaimVersonDescription = CheckAttribute(oDataNode, "ClaimVersonDescription")
            '    oClaimReceipt.CurrencyCode = CheckAttribute(oDataNode, "CurrencyCode")
            '    oClaimReceipt.PartyKey = CheckAttribute(oDataNode, "PartyKey")
            '    oClaimReceipt.ReceiptType = CheckAttribute(oDataNode, "ReceiptType")
            '    'ReDim m_vGenerateClaimDocument(oDataNode.ChildNodes.Count - 1)
            '    For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
            '        oClaimReceipt = New ClaimReceiptAndTaxes
            '        oClaimReceipt.BaseRecoveryKey = CheckAttribute(oDataNode.ChildNodes(iCnt), "BaseRecoveryKey")
            '        oClaimReceipt.ReceiptAmount = CheckAttribute(oDataNode.ChildNodes(iCnt), "ReceiptAmount")
            '        oClaimReceipt.TaxGroupCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "TaxGroupCode")
            '        'm_vClaimReceiptAndTaxes(iCnt) = oClaimReceipt
            '    Next
            'End If
            '----------------------------GENERATE CLAIM DOCUMENTS----------------------------------------------

            Dim oGenerateClaimDocument As New GenerateClaims

            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                      .SelectSingleNode("GENERATECLAIM")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><GENERATECLAIMS> not found")
            Else
                ReDim m_vGenerateClaimDoc(oDataNode.ChildNodes.Count - 1)
                For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                    oGenerateClaimDocument = New GenerateClaims
                    oGenerateClaimDocument.Mode = CheckAttribute(oDataNode.ChildNodes(iCnt), "Mode")
                    oGenerateClaimDocument.ClaimKey = CheckAttribute(oDataNode.ChildNodes(iCnt), "ClaimKey")
                    oGenerateClaimDocument.TransactionType = CheckAttribute(oDataNode.ChildNodes(iCnt), "TransactionType")
                    oGenerateClaimDocument.ParameterXML = CheckAttribute(oDataNode.ChildNodes(iCnt), "ParameterXML", 0)
                    oGenerateClaimDocument.OutputAsHTML = CheckAttribute(oDataNode.ChildNodes(iCnt), "OutputAsHTML", 1)
                    m_vGenerateClaimDoc(iCnt) = oGenerateClaimDocument
                Next
            End If
            '-----------------------------CLAIM RECEIPT---------------------------------------------------------

            Dim oClaimReceipt As New ClaimReceiptTaxes
            Dim oClaimReceiptNode As XmlNode
            oClaimReceiptNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("CLAIMRECEIPTDETAILS")

            If oClaimReceiptNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><CLAIMRECEIPTDETAILS> not found")
            Else
                oClaimReceipt.LoadClaimReceiptStructure(oClaimReceiptNode, Me)
                m_vClaimReceipt = oClaimReceipt
            End If



            '************** OPEN CLAIM **************************'
            Dim oOpenClaimStructure As New OpenClaimXMLStructure
            Dim oOpenClaimNode As XmlNode
            oOpenClaimNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("OPENCLAIM").SelectSingleNode("CLAIM")

            If oOpenClaimNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><OPENCLAIM> not found")
            Else
                oOpenClaimStructure.LoadClaimStructure(oOpenClaimNode, Me)
                m_oOpenClaimXMLStructure = oOpenClaimStructure
            End If

            '************** MAINTAIN CLAIM **************************'
            Dim oMaintainClaimStructure As New MaintainClaimXMLStructure
            Dim oMaintainClaimNode As XmlNode
            oMaintainClaimNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("MAINTAINCLAIM").SelectSingleNode("CLAIM")

            If oMaintainClaimNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><MAINTAINCLAIM> not found")
            Else
                oMaintainClaimStructure.LoadClaimStructure(oMaintainClaimNode, Me)
                m_oMaintainClaimXMLStructure = oMaintainClaimStructure
            End If

            '************** ADDAGENTRECEIPT **************************'
            Dim oAddAgentReceiptStructure As New AddAgentReceiptXMLStructure
            Dim oAddAgentReceiptNode As XmlNode
            oAddAgentReceiptNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("ADDAGENTRECEIPT")

            If oAddAgentReceiptNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><ADDAGENTRECEIPT> not found")
            Else
                oAddAgentReceiptStructure.Load(oAddAgentReceiptNode, Me)
                m_oAddAgentReceiptXMLStructure = oAddAgentReceiptStructure
            End If
            '*********************************************************'

            '************** POSTDOCUMENT **************************'
            Dim oPostDocumentXMLStructure As New PostDocumentXMLStructure
            Dim oPostDocumentNode As XmlNode
            oPostDocumentNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("POSTDOCUMENT")

            If oPostDocumentNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><POSTDOCUMENT> not found")
            Else
                oPostDocumentXMLStructure.Load(oPostDocumentNode, Me)
                m_oPostDocumentXMLStructure = oPostDocumentXMLStructure
            End If
            '*********************************************************'

            '************** CLIENTDATAIMPORT **************************'
            Dim oClientDataImportXMLStructure As New ClientDataImportTestDataXMLStruture
            Dim oClientDataImportNode As XmlNode
            oClientDataImportNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("CLIENTDATAIMPORT")

            If oClientDataImportNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><CLIENTDATAIMPORT> not found")
            Else
                oClientDataImportXMLStructure.Load(oClientDataImportNode, Me)
                m_oClientDataImportXMLStructure = oClientDataImportXMLStructure
            End If
            '*********************************************************'

            '************** FINDINSURANCEFILEFORCLAIM **************************'
            Dim oFindInsuranceFileForClaims As New FindInsuranceFileForClaimsTestData
            Dim oFindInsuranceFileForClaimsNode As XmlNode
            oFindInsuranceFileForClaimsNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("FINDINSURANCEFILEFORCLAIMS")

            If oFindInsuranceFileForClaimsNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><FINDINSURANCEFILEFORCLAIMS> not found")
            Else
                oFindInsuranceFileForClaims.Load(oFindInsuranceFileForClaimsNode, Me)
                m_oFindInsuranceFileForClaimsTestData = oFindInsuranceFileForClaims
            End If
            '*********************************************************'

            '************** FIND CLAIM **************************'
            Dim oFindClaimsNode As XmlNode
            oFindClaimsNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("FINDCLAIM")

            If oFindClaimsNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><FINDCLAIM> not found")
            Else
                m_sFindClaimBranchCode = CheckAttribute(oFindClaimsNode, "BranchCode", String.Empty)
                m_sFindClaimClaimNumber = CheckAttribute(oFindClaimsNode, "ClaimNumber", String.Empty)
                m_sFindClaimInsuranceFileRef = CheckAttribute(oFindClaimsNode, "InsuranceFileRef", String.Empty)
                m_sFindClaimClientShortName = CheckAttribute(oFindClaimsNode, "ClientShortName", String.Empty)
                m_dtFindClaimLossDateFrom = CheckAttribute(oFindClaimsNode, "LossDateFrom", New Date)
                m_dtFindClaimLossDateTo = CheckAttribute(oFindClaimsNode, "LossDateTo", New Date)
                m_sFindClaimRiskIndex = CheckAttribute(oFindClaimsNode, "RiskIndex", String.Empty)
            End If
            '*********************************************************'
            '************** UPDATE PARTY OTHER **************************'
            '*********************************************************'
            Dim oUpdatePartyOtherTestData As New UpdatePartyOtherTestData
            Dim oUpdatePartyOtherNode As XmlNode
            oUpdatePartyOtherNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("UPDATEPARTYOTHER")

            If oUpdatePartyOtherNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><UPDATEPARTYOTHER> not found")
            Else
                oUpdatePartyOtherTestData.Load(oUpdatePartyOtherNode, Me)
                m_oUpdatePartyOtherTestData = oUpdatePartyOtherTestData
            End If
            '*********************************************************'

            ' Pay Claim GAURAV
            Dim oPaymentClaimNode As XmlNode
            Dim oClaimPayment As New cClaimPayment
            oPaymentClaimNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("PayClaim")
            If oPaymentClaimNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><PAYMENTCLAIM> not found")
            Else
                oClaimPayment.LoadClaimPayment(oPaymentClaimNode, Me)
                m_oPayClaimStructure = oClaimPayment
            End If

            ' Get Claim Details
            Dim oGetClaimDetailsNode As XmlNode
            'Dim oClaimPayment As New cClaimPayment
            oGetClaimDetailsNode = oXML.SelectSingleNode("TEST_DATA").SelectSingleNode("GetClaimDetails")
            If oGetClaimDetailsNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><GETCLAIMDETAILS> not found")
            Else
                m_lGetClaimDetailsClaimKey = oGetClaimDetailsNode.Attributes.GetNamedItem("ClaimKey").Value
                m_sGetClaimDetailsBranchCode = oGetClaimDetailsNode.Attributes.GetNamedItem("BranchCode").Value
            End If


            Dim oConviction As Conviction
            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
            .SelectSingleNode("CONVICTIONS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><CONVICTIONS> not found")
            Else
                ReDim m_vConvictions(oDataNode.ChildNodes.Count - 1)
                For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                    oConviction = New Conviction
                    oConviction.Key = CheckAttribute(oDataNode.ChildNodes(iCnt), "Key", 0) 'DONE:05-OCT-2006
                    oConviction.AlcoholLevel = CheckAttribute(oDataNode.ChildNodes(iCnt), "AlcoholLevel")
                    oConviction.AlcoholMeasurementMethod = CheckAttribute(oDataNode.ChildNodes(iCnt), "AlcoholMeasurementMethod")
                    oConviction.Date = CheckAttribute(oDataNode.ChildNodes(iCnt), "Date")
                    oConviction.Description = CheckAttribute(oDataNode.ChildNodes(iCnt), "Description")
                    oConviction.DrivingLicencePenaltyPoints = CheckAttribute(oDataNode.ChildNodes(iCnt), "DrivingLicencePenaltyPoints")
                    oConviction.FineAmount = CheckAttribute(oDataNode.ChildNodes(iCnt), "FineAmount")
                    oConviction.SentenceDescription = CheckAttribute(oDataNode.ChildNodes(iCnt), "SentenceDescription")
                    oConviction.SentenceDuration = CheckAttribute(oDataNode.ChildNodes(iCnt), "SentenceDuration")
                    oConviction.SentenceDurationQualifier = CheckAttribute(oDataNode.ChildNodes(iCnt), "SentenceDurationQualifier")

                    oConviction.SentenceEffectiveDate = CheckAttribute(oDataNode.ChildNodes(iCnt), "SentenceEffectiveDate")
                    oConviction.SentenceTypeCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "SentenceTypeCode")
                    oConviction.StatusCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "StatusCode")

                    oConviction.TypeCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "TypeCode")

                    m_vConvictions(iCnt) = oConviction
                Next

            End If

            'GAURAV
            Dim oAccident As Accident
            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
            .SelectSingleNode("ACCIDENTS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><ACCIDENTS> not found")
            Else
                ReDim m_vAccidents(oDataNode.ChildNodes.Count - 1)
                For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                    oAccident = New Accident
                    oAccident.Date = CheckAttribute(oDataNode.ChildNodes(iCnt), "Date")
                    oAccident.Description = CheckAttribute(oDataNode.ChildNodes(iCnt), "Description")
                    oAccident.IsAtFault = CheckAttribute(oDataNode.ChildNodes(iCnt), "IsAtFault")
                    m_vAccidents(iCnt) = oAccident
                Next
            End If

            Dim oSuppBusiness As SupplierBusiness
            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
            .SelectSingleNode("SUPPLIERBUSINESS")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><SUPPLIERBUSINESS> not found")
            Else
                ReDim m_vSuppBusiness(oDataNode.ChildNodes.Count - 1)
                For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                    oSuppBusiness = New SupplierBusiness
                    oSuppBusiness.BusinessCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "BUSINESSCODE")
                    oSuppBusiness.SpecialityCode = CheckAttribute(oDataNode.ChildNodes(iCnt), "SPECIALITYCODE")
                    m_vSuppBusiness(iCnt) = oSuppBusiness
                Next
            End If

            'GAURAV
            Dim oOtherPartyInfo As OtherPartyInfo
            oDataNode = oXML.SelectSingleNode("TEST_DATA") _
            .SelectSingleNode("OTHERPARTYINFO")
            If oDataNode Is Nothing Then
                m_oLogFile.WriteLine("<TEST_DATA><OtherPartyInfo> not found")
            Else
                'For iCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
                oOtherPartyInfo = New OtherPartyInfo
                oOtherPartyInfo.ActiveIndicator = CheckAttribute(oDataNode.ChildNodes(0), "ActiveIndicator")
                oOtherPartyInfo.AfterHoursIndicator = CheckAttribute(oDataNode.ChildNodes(0), "AfterHoursIndicator")
                oOtherPartyInfo.Code = CheckAttribute(oDataNode.ChildNodes(0), "Code")

                oOtherPartyInfo.DateOfBirth = CheckAttribute(oDataNode.ChildNodes(0), "DateOfBirth")
                oOtherPartyInfo.DriverStatusCode = CheckAttribute(oDataNode.ChildNodes(0), "DriverStatusCode")
                oOtherPartyInfo.Gender = CheckAttribute(oDataNode.ChildNodes(0), "Gender")

                oOtherPartyInfo.LicenseNumber = CheckAttribute(oDataNode.ChildNodes(0), "LicenseNumber")
                oOtherPartyInfo.LicenseTypeCode = CheckAttribute(oDataNode.ChildNodes(0), "LicenseTypeCode")
                oOtherPartyInfo.Name = CheckAttribute(oDataNode.ChildNodes(0), "Name")

                oOtherPartyInfo.PriorityIndicator = CheckAttribute(oDataNode.ChildNodes(0), "PriorityIndicator")
                oOtherPartyInfo.RegNumber = CheckAttribute(oDataNode.ChildNodes(0), "RegNumber")
                oOtherPartyInfo.TypeCode = CheckAttribute(oDataNode.ChildNodes(0), "TypeCode")


                m_vOtherPartyInfo = oOtherPartyInfo
                'Next
            End If

        Catch ex As Exception
            m_oLogFile.WriteLine("An error occurred reading the test data: " & ex.Message)
        End Try

        m_oLogFile.Close()

    End Sub
#End Region

#Region " Public Properties"
    Public ReadOnly Property AnalysisCode() As String
        Get
            AnalysisCode = m_sAnalysisCode
        End Get
    End Property
    Public ReadOnly Property UpdateAnalysisCode() As String
        Get
            UpdateAnalysisCode = m_sUpdateAnalysisCode
        End Get
    End Property
    Public ReadOnly Property GetClaimDetailsClaimKey() As String 'GAURAV
        Get
            GetClaimDetailsClaimKey = m_lGetClaimDetailsClaimKey
        End Get
    End Property

    Public ReadOnly Property GetClaimDetailsBranchCode() As String 'GAURAV
        Get
            GetClaimDetailsBranchCode = m_sGetClaimDetailsBranchCode
        End Get
    End Property

    Public ReadOnly Property GetClaimRiskClaimKey() As String 'GAURAV
        Get
            GetClaimRiskClaimKey = m_lGetClaimRiskClaimKey
        End Get
    End Property

    Public ReadOnly Property GetClaimRiskBranchCode() As String 'GAURAV
        Get
            GetClaimRiskBranchCode = m_sGetClaimRiskBranchCode
        End Get
    End Property

    Public ReadOnly Property Generateclaim() As GenerateClaims()
        Get
            Generateclaim = m_vGenerateClaimDoc
        End Get
    End Property

    Public ReadOnly Property PayClaim() As cClaimPayment
        Get
            PayClaim = m_oPayClaimStructure
        End Get
    End Property

    Public ReadOnly Property ClaimReceiptAndTaxes() As ClaimReceiptTaxes
        Get
            ClaimReceiptAndTaxes = m_vClaimReceipt
        End Get
    End Property
    Public ReadOnly Property BranchCode() As String
        Get
            BranchCode = m_sBranchCode
        End Get
    End Property
    Public ReadOnly Property ClaimKey() As Integer
        Get
            ClaimKey = m_nClaimKey
        End Get
    End Property
    Public ReadOnly Property SubBranch() As String
        Get
            SubBranch = m_sSubBranch
        End Get
    End Property
    Public ReadOnly Property AgentKey() As Integer
        Get
            AgentKey = m_nAgentKey
        End Get
    End Property
    Public ReadOnly Property UserName() As String
        Get
            UserName = m_sUserName
        End Get
    End Property
    Public ReadOnly Property ProductCode() As String
        Get
            ProductCode = m_sProductCode
        End Get
    End Property
    Public ReadOnly Property DateOfBirth() As Date
        Get
            DateOfBirth = m_dtDOB
        End Get
    End Property
    Public ReadOnly Property CoverStartDate() As Date
        Get
            CoverStartDate = m_dtCoverStart
        End Get
    End Property
    Public ReadOnly Property CoverEndDate() As Date
        Get
            CoverEndDate = m_dtCoverEnd
        End Get
    End Property
    Public ReadOnly Property QuoteDescription() As String
        Get
            QuoteDescription = m_sQuoteDescription
        End Get
    End Property
    Public ReadOnly Property ListCode() As String
        Get
            ListCode = m_sListCode
        End Get
    End Property
    Public ReadOnly Property DataModelCode() As String
        Get
            DataModelCode = m_sDataModelCode
        End Get
    End Property
    Public ReadOnly Property ClaimDataModelCode() As String
        Get
            ClaimDataModelCode = m_sClaimDataModelCode
        End Get
    End Property
    Public ReadOnly Property RiskTypeCode() As String
        Get
            RiskTypeCode = m_sRiskTypeCode
        End Get
    End Property
    Public ReadOnly Property ScreenCode() As String
        Get
            ScreenCode = m_sScreenCode
        End Get
    End Property
    Public ReadOnly Property RiskDescription() As String
        Get
            RiskDescription = m_sRiskDescription
        End Get
    End Property
    Public ReadOnly Property AddressTypeCode() As Integer
        Get
            AddressTypeCode = m_nAddressTypeCode
        End Get
    End Property
    Public ReadOnly Property AddressLine1() As String
        Get
            AddressLine1 = m_sAddressLine1
        End Get
    End Property
    Public ReadOnly Property AddressLine2() As String
        Get
            AddressLine2 = m_sAddressLine2
        End Get
    End Property
    Public ReadOnly Property AddressLine3() As String
        Get
            AddressLine3 = m_sAddressLine3
        End Get
    End Property
    Public ReadOnly Property AddressLine4() As String
        Get
            AddressLine4 = m_sAddressLine4
        End Get
    End Property
    Public ReadOnly Property PostCode() As String
        Get
            PostCode = m_sPostCode
        End Get
    End Property
    Public ReadOnly Property CountryCode() As String
        Get
            CountryCode = m_sCountryCode
        End Get
    End Property

    Public ReadOnly Property Surname() As String
        Get
            Surname = m_sSurname
        End Get
    End Property
    Public ReadOnly Property Forename() As String
        Get
            Forename = m_sForename
        End Get
    End Property
    Public ReadOnly Property Initials() As String
        Get
            Initials = m_sInitials
        End Get
    End Property
    Public ReadOnly Property Title() As String
        Get
            Title = m_sTitle
        End Get
    End Property
    Public ReadOnly Property TPIntroducer() As String
        Get
            TPIntroducer = m_sTPIntroducer
        End Get
    End Property
    Public ReadOnly Property TPUser() As String
        Get
            TPUser = m_sTPUser
        End Get
    End Property
    Public ReadOnly Property AlternativeId() As String
        Get
            AlternativeId = m_sAlternativeId
        End Get
    End Property
    Public ReadOnly Property EmployerBusinessCode() As String
        Get
            EmployerBusinessCode = m_sEmployerBusinessCode
        End Get
    End Property
    Public ReadOnly Property OccupationCode() As String
        Get
            OccupationCode = m_sOccupationCode
        End Get
    End Property
    Public ReadOnly Property GenderCode() As String
        Get
            GenderCode = m_sGenderCode
        End Get
    End Property
    Public ReadOnly Property CompanyName() As String
        Get
            CompanyName = m_sCompanyName
        End Get
    End Property
    Public ReadOnly Property TradingName() As String
        Get
            TradingName = m_sTradingName
        End Get
    End Property
    Public ReadOnly Property BusinessCode() As String
        Get
            BusinessCode = m_sBusinessCode
        End Get
    End Property
    Public ReadOnly Property MaritalStatusCode() As Integer
        Get
            MaritalStatusCode = m_nMaritalStatusCode
        End Get
    End Property
    Public ReadOnly Property EmploymentStatusCode() As Integer
        Get
            EmploymentStatusCode = m_nEmploymentStatusCode
        End Get
    End Property
    Public Property RunDefaultRules() As Boolean
        Get
            RunDefaultRules = m_bRunDefaultRules
        End Get
        Set(ByVal value As Boolean)
            m_bRunDefaultRules = value
        End Set
    End Property
    Public ReadOnly Property Password() As String
        Get
            Password = m_sPassword
        End Get
    End Property
    Public ReadOnly Property NewPassword() As String
        Get
            NewPassword = m_sNewPassword
        End Get
    End Property
    Public ReadOnly Property InvalidLookupCode() As String
        Get
            InvalidLookupCode = m_sInvalidLookupCode
        End Get
    End Property
    Public ReadOnly Property InsuranceFileRef() As String
        Get
            InsuranceFileRef = m_sInsuranceFileRef
        End Get
    End Property
    Public ReadOnly Property NewInsuranceFileRef() As String
        Get
            NewInsuranceFileRef = m_sNewInsuranceFileRef
        End Get
    End Property
    Public ReadOnly Property InsuranceFolderCnt() As Integer
        Get
            InsuranceFolderCnt = m_nInsuranceFolderCnt
        End Get
    End Property
    Public ReadOnly Property InsuranceFileCnt() As Integer
        Get
            InsuranceFileCnt = m_nInsuranceFileCnt
        End Get
    End Property
    Public ReadOnly Property PartyCnt() As Integer
        Get
            PartyCnt = m_nPartyCnt
        End Get
    End Property
    Public ReadOnly Property RiskCnt() As Integer
        Get
            RiskCnt = m_nRiskCnt
        End Get
    End Property
    Public ReadOnly Property ListType() As Integer
        Get
            ListType = m_nListType
        End Get
    End Property
    Public ReadOnly Property FPAddressLine1() As String
        Get
            FPAddressLine1 = m_sFPAddressLine1
        End Get
    End Property
    Public ReadOnly Property FPAddressLine2() As String
        Get
            FPAddressLine2 = m_sFPAddressLine2
        End Get
    End Property
    Public ReadOnly Property FPAddressLine3() As String
        Get
            FPAddressLine3 = m_sFPAddressLine3
        End Get
    End Property
    Public ReadOnly Property FPAddressLine4() As String
        Get
            FPAddressLine4 = m_sFPAddressLine4
        End Get
    End Property
    Public ReadOnly Property FPAreaCode() As String
        Get
            FPAreaCode = m_sFPAreaCode
        End Get
    End Property
    Public ReadOnly Property FPAlternativeId() As String
        Get
            FPAlternativeId = m_sFPAlternativeID
        End Get
    End Property
    Public ReadOnly Property FPDateOfBirth() As Date
        Get
            FPDateOfBirth = m_dtFPDOB
        End Get
    End Property
    Public ReadOnly Property FPFirstName() As String
        Get
            FPFirstName = m_sFPFirstName
        End Get
    End Property
    Public ReadOnly Property FPName() As String
        Get
            FPName = m_sFPName
        End Get
    End Property
    Public ReadOnly Property FPPartyType() As String
        Get
            FPPartyType = m_sFPPartyType
        End Get
    End Property
    Public ReadOnly Property FPPolicyRef() As String
        Get
            FPPolicyRef = m_sFPPolicyRef
        End Get
    End Property
    Public ReadOnly Property FPPostCode() As String
        Get
            FPPostCode = m_sFPPostCode
        End Get
    End Property
    Public ReadOnly Property FPRiskIndex() As String
        Get
            FPRiskIndex = m_sFPRiskIndex
        End Get
    End Property
    Public ReadOnly Property FPShortName() As String
        Get
            FPShortName = m_sFPShortName
        End Get
    End Property
    Public ReadOnly Property FPTelephoneNumber() As String
        Get
            FPTelephoneNumber = m_sFPTelephoneNumber
        End Get
    End Property
    Public ReadOnly Property DocumentTemplateCode() As String
        Get
            DocumentTemplateCode = m_sDocumentTemplateCode
        End Get
    End Property
    Public ReadOnly Property GenerateDocumentMode() As Integer
        Get
            GenerateDocumentMode = m_nGenerateDocumentMode
        End Get
    End Property
    Public ReadOnly Property GenerateDocumentOutputAsHTML() As Boolean
        Get
            GenerateDocumentOutputAsHTML = m_bGenerateDocumentOutputAsHTML
        End Get
    End Property
    Public ReadOnly Property GenerateDocumentParameterXML() As String
        Get
            GenerateDocumentParameterXML = m_sGenerateDocumentParameterXML
        End Get
    End Property
    Public ReadOnly Property InvalidCnt() As String
        Get
            InvalidCnt = m_nInvalidCnt
        End Get
    End Property
    Public ReadOnly Property InvalidInsFileFolderCnt() As String
        Get
            InvalidInsFileFolderCnt = m_nInvalidInsFileFolderCnt
        End Get
    End Property
    Public ReadOnly Property InvalidInsFileRiskCnt() As String
        Get
            InvalidInsFileRiskCnt = m_nInvalidInsFileRiskCnt
        End Get
    End Property
    Public ReadOnly Property FindClaimBranchCode() As String
        Get
            FindClaimBranchCode = m_sFindClaimBranchCode
        End Get
    End Property
    Public ReadOnly Property FindClaimClaimNumber() As String
        Get
            FindClaimClaimNumber = m_sFindClaimClaimNumber
        End Get
    End Property
    Public ReadOnly Property FindClaimInsuranceFileRef() As String
        Get
            FindClaimInsuranceFileRef = m_sFindClaimInsuranceFileRef
        End Get
    End Property
    Public ReadOnly Property FindClaimClientShortName() As String
        Get
            FindClaimClientShortName = m_sFindClaimClientShortName
        End Get
    End Property
    Public ReadOnly Property FindClaimLossDateFrom() As Date
        Get
            FindClaimLossDateFrom = m_dtFindClaimLossDateFrom
        End Get
    End Property
    Public ReadOnly Property FindClaimLossDateTo() As Date
        Get
            FindClaimLossDateTo = m_dtFindClaimLossDateTo
        End Get
    End Property
    Public ReadOnly Property FindClaimRiskIndex() As String
        Get
            FindClaimRiskIndex = m_sFindClaimRiskIndex
        End Get
    End Property

    Public ReadOnly Property ClaimMTABranchCode() As String
        Get
            ClaimMTABranchCode = m_sClaimMTABranchCode
        End Get
    End Property
    Public ReadOnly Property ClaimMTAInsuranceFolderKey() As Integer
        Get
            ClaimMTAInsuranceFolderKey = m_nClaimMTAInsuranceFolderKey
        End Get
    End Property
    Public ReadOnly Property ClaimMTAInsuranceFileKey() As Integer
        Get
            ClaimMTAInsuranceFileKey = m_nClaimMTAInsuranceFileKey
        End Get
    End Property
    Public ReadOnly Property ClaimMTARiskKey() As Integer
        Get
            ClaimMTARiskKey = m_nClaimMTARiskKey
        End Get
    End Property
    Public ReadOnly Property ClaimMTAScreenCode() As String
        Get
            ClaimMTAScreenCode = m_sClaimMTAScreenCode
        End Get
    End Property
    Public ReadOnly Property ClaimMTARiskDescription() As String
        Get
            ClaimMTARiskDescription = m_sClaimMTARiskDescription
        End Get
    End Property
    Public ReadOnly Property ClaimMTAXMLDataSet() As String
        Get
            ClaimMTAXMLDataSet = m_sClaimMTAXMLDataSet
        End Get
    End Property

    Public ReadOnly Property Addresses() As Address()
        Get
            Addresses = m_vAddresses
        End Get
    End Property
    Public ReadOnly Property Contacts() As Contact()
        Get
            Contacts = m_vContacts
        End Get
    End Property

    Public ReadOnly Property OtherPartyInfo() As OtherPartyInfo
        Get
            OtherPartyInfo = m_vOtherPartyInfo
        End Get
    End Property

    Public ReadOnly Property Accidents() As Accident()
        Get
            Accidents = m_vAccidents
        End Get
    End Property

    Public ReadOnly Property Convictions() As Conviction()
        Get
            Convictions = m_vConvictions
        End Get
    End Property

    Public ReadOnly Property SuppBusiness() As SupplierBusiness()
        Get
            SuppBusiness = m_vSuppBusiness
        End Get
    End Property

    Public ReadOnly Property XMLDataSetElementsToAdd() As XMLElementToAdd()
        Get
            XMLDataSetElementsToAdd = m_vElementsToAdd
        End Get
    End Property

    Public ReadOnly Property XMLDataSetMTAElementsToAdd() As XMLElementToAdd()
        Get
            XMLDataSetMTAElementsToAdd = m_vMTAElementsToAdd
        End Get
    End Property

    Public ReadOnly Property XMLDataSetClaimsElementsToAdd() As XMLElementToAdd()
        Get
            XMLDataSetClaimsElementsToAdd = m_vClaimsElementsToAdd
        End Get
    End Property

    Public ReadOnly Property RiskDataXML() As String
        Get
            RiskDataXML = m_sRiskDataXML
        End Get
    End Property

    Public ReadOnly Property OpenClaim() As OpenClaimXMLStructure
        Get
            Return m_oOpenClaimXMLStructure
        End Get
    End Property

    Public ReadOnly Property MaintainClaim() As MaintainClaimXMLStructure
        Get
            Return m_oMaintainClaimXMLStructure
        End Get
    End Property

    Public ReadOnly Property AgentReceipt() As AddAgentReceiptXMLStructure
        Get
            Return m_oAddAgentReceiptXMLStructure
        End Get
    End Property

    Public ReadOnly Property PostDocument() As PostDocumentXMLStructure
        Get
            Return m_oPostDocumentXMLStructure
        End Get
    End Property

    Public ReadOnly Property ClientDataImport() As ClientDataImportTestDataXMLStruture
        Get
            Return m_oClientDataImportXMLStructure
        End Get
    End Property

    Public ReadOnly Property FindInsuranceFileForClaims() As FindInsuranceFileForClaimsTestData
        Get
            Return m_oFindInsuranceFileForClaimsTestData
        End Get
    End Property

    Public ReadOnly Property PaymentMethod() As Integer
        Get
            PaymentMethod = m_iPaymentMethod
        End Get
    End Property

    Public ReadOnly Property UpdatePartyOtherTestData() As UpdatePartyOtherTestData
        Get
            Return m_oUpdatePartyOtherTestData
        End Get
    End Property

    Public ReadOnly Property GetOptionSettingNumber() As Integer
        Get
            GetOptionSettingNumber = m_iGetOptionSettingNumber
        End Get
    End Property

    Public ReadOnly Property GetOptionSettingType() As Integer
        Get
            GetOptionSettingType = m_iGetOptionSettingType
        End Get
    End Property

#End Region

#Region "Private Procedures"

#End Region

End Class

Friend Class cClaimPayment 'GAURAV
    Public BaseClaimKey As Integer
    Public BranchCode As String
    Public BaseClaimPerilKey As Integer
    Public PartyKey As Integer
    Public PaymentPartyType As Integer
    Public CurrencyCode As String
    Public ClaimVersionDescription As String

    Public oPayee As Payee
    Public oAdvanceTaxDetails As cAdvancetaxDetails
    Public oPaymentItems As New List(Of cPaymentItems)

    Friend Sub LoadClaimPayment(ByVal oClaimNode As XmlNode, ByVal oTestData As TestData)

        Dim oNode As XmlNode

        For Each oNode In oClaimNode.ChildNodes

            Select Case oNode.Name

                Case "Payee"
                    oPayee = New Payee
                    oPayee.loadPayee(oNode)
                Case "AdvancedTaxDetails"
                    oAdvanceTaxDetails = New cAdvancetaxDetails
                    oAdvanceTaxDetails.loadAdvanceTaxDetails(oNode)
                Case "Reserve"

                    Dim oPaymentItem As New cPaymentItems
                    oPaymentItem.loadPaymentItems(oNode)
                    oPaymentItems.Add(oPaymentItem)
            End Select

        Next

        Me.BranchCode = oClaimNode.Attributes.GetNamedItem("BranchCode").Value
        Me.BaseClaimKey = oClaimNode.Attributes.GetNamedItem("BaseClaimKey").Value
        Me.BaseClaimPerilKey = oClaimNode.Attributes.GetNamedItem("BaseClaimPerilKey").Value
        Me.PartyKey = oClaimNode.Attributes.GetNamedItem("PartyKey").Value
        Me.PaymentPartyType = oClaimNode.Attributes.GetNamedItem("PaymentPartyType").Value
        Me.CurrencyCode = oClaimNode.Attributes.GetNamedItem("CurrencyCode").Value
        Me.ClaimVersionDescription = oClaimNode.Attributes.GetNamedItem("ClaimVersionDescription").Value

    End Sub

End Class

Friend Class Payee
    Public Name As String
    Public BankName As String
    Public BankNumber As String
    Public BankCode As String
    Public MediaTypeCode As String
    Public MediaReference As String
    Public TheirReference As String
    Public Comments As String
    Public oAddress As New Address

    Public Sub loadPayee(ByVal oClaimPayeeNode As XmlNode)
        For Each oNode As XmlNode In oClaimPayeeNode.ChildNodes

            Select Case oNode.Name

                Case "Address"
                    oAddress.loadAddress(oNode)
            End Select
        Next

        Me.Name = oClaimPayeeNode.Attributes.GetNamedItem("Name").Value
        Me.BankName = oClaimPayeeNode.Attributes.GetNamedItem("BankName").Value
        Me.BankNumber = oClaimPayeeNode.Attributes.GetNamedItem("BankNumber").Value
        Me.BankCode = oClaimPayeeNode.Attributes.GetNamedItem("BankCode").Value
        Me.MediaTypeCode = oClaimPayeeNode.Attributes.GetNamedItem("MediaTypeCode").Value
        Me.MediaReference = oClaimPayeeNode.Attributes.GetNamedItem("MediaReference").Value
        Me.TheirReference = oClaimPayeeNode.Attributes.GetNamedItem("TheirReference").Value
        Me.Comments = oClaimPayeeNode.Attributes.GetNamedItem("Comments").Value

    End Sub

End Class

Public Class cAdvancetaxDetails
    Public InsuredDomiciled As Boolean
    Public InsuredPercentage As Decimal
    Public InsuranceTaxNumber As String
    Public PayeeDomiciled As Boolean
    Public PayeePercentage As Decimal
    Public PayeeTaxNumber As String
    Public SafeHarbourCode As String
    Public SafeHarbourPercentage As Decimal
    Public IsTaxExempt As Boolean
    Public IsWHTExempt As Boolean
    Public IsSettlement As Boolean


    Public Sub loadAdvanceTaxDetails(ByVal oClaimAdvanceTaxDet As XmlNode)
        Me.InsuredDomiciled = oClaimAdvanceTaxDet.Attributes.GetNamedItem("InsuredDomiciled").Value
        Me.InsuredPercentage = oClaimAdvanceTaxDet.Attributes.GetNamedItem("InsuredPercentage").Value
        Me.InsuranceTaxNumber = oClaimAdvanceTaxDet.Attributes.GetNamedItem("InsuranceTaxNumber").Value
        Me.PayeeDomiciled = oClaimAdvanceTaxDet.Attributes.GetNamedItem("PayeeDomiciled").Value
        Me.PayeePercentage = oClaimAdvanceTaxDet.Attributes.GetNamedItem("PayeePercentage").Value
        Me.PayeeTaxNumber = oClaimAdvanceTaxDet.Attributes.GetNamedItem("PayeeTaxNumber").Value
        Me.SafeHarbourCode = oClaimAdvanceTaxDet.Attributes.GetNamedItem("SafeHarbourCode").Value
        Me.SafeHarbourPercentage = oClaimAdvanceTaxDet.Attributes.GetNamedItem("SafeHarbourPercentage").Value
        Me.IsTaxExempt = oClaimAdvanceTaxDet.Attributes.GetNamedItem("IsTaxExempt").Value
        Me.IsWHTExempt = oClaimAdvanceTaxDet.Attributes.GetNamedItem("IsWHTExempt").Value
        Me.IsSettlement = oClaimAdvanceTaxDet.Attributes.GetNamedItem("IsSettlement").Value

    End Sub
End Class

Public Class cPaymentItems
    Public BaseReserveKey As Integer
    Public TaxGroupCode As String
    Public PaymentAmount As Decimal
    Public ReverseExcess As Boolean

    Public Sub loadPaymentItems(ByVal oClaimPaymentItems As XmlNode)
        Me.BaseReserveKey = oClaimPaymentItems.Attributes.GetNamedItem("BaseReserveKey").Value
        Me.TaxGroupCode = oClaimPaymentItems.Attributes.GetNamedItem("TaxGroupCode").Value
        Me.PaymentAmount = oClaimPaymentItems.Attributes.GetNamedItem("PaymentAmount").Value
        Me.ReverseExcess = oClaimPaymentItems.Attributes.GetNamedItem("ReverseExcess").Value
    End Sub
End Class


Friend Class Accident 'GAURAV

    Private dateField As Date
    Private descriptionField As String
    Private isAtFaultField As Short


    '''<remarks/>

    Public Property [Date]() As Date
        Get
            Return Me.dateField
        End Get
        Set(ByVal value As Date)
            Me.dateField = Value
        End Set
    End Property


    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.descriptionField
        End Get
        Set(ByVal value As String)
            Me.descriptionField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property IsAtFault() As Short
        Get
            Return Me.isAtFaultField
        End Get
        Set(ByVal value As Short)
            Me.isAtFaultField = Value
        End Set
    End Property


End Class

Friend Class SupplierBusiness 'DONE - 09-OCT-2006
    Private BusinessCodeField As Integer
    Private SpecialityCodeField As Integer

    Public Property BusinessCode() As Integer
        Get
            Return Me.BusinessCodeField
        End Get
        Set(ByVal value As Integer)
            BusinessCodeField = value
        End Set
    End Property

    Public Property SpecialityCode() As Integer
        Get
            Return Me.SpecialityCodeField
        End Get
        Set(ByVal value As Integer)
            SpecialityCodeField = value
        End Set
    End Property
End Class

Friend Class Conviction 'GAURAV
    '''<remarks/>

    Private KeyField As Integer
    Private typeCodeField As String
    Private statusCodeField As String
    Private descriptionField As String
    Private fineAmountField As Decimal
    Private dateField As Date
    Private sentenceTypeCodeField As String
    Private sentenceDescriptionField As String
    Private sentenceDurationField As Decimal
    Private sentenceDurationQualifierField As String
    Private sentenceEffectiveDateField As Date
    Private alcoholLevelField As Decimal
    Private alcoholMeasurementMethodField As String
    Private drivingLicencePenaltyPointsField As Decimal

    Public Property Key() As Integer
        Get
            Return Me.KeyField
        End Get
        Set(ByVal value As Integer)
            Me.KeyField = value
        End Set
    End Property

    Public Property TypeCode() As String
        Get
            Return Me.typeCodeField
        End Get
        Set(ByVal value As String)
            Me.typeCodeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.statusCodeField
        End Get
        Set(ByVal value As String)
            Me.statusCodeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.descriptionField
        End Get
        Set(ByVal value As String)
            Me.descriptionField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property FineAmount() As Decimal
        Get
            Return Me.fineAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.fineAmountField = Value
        End Set
    End Property


    '''<remarks/>
    Public Property [Date]() As Date
        Get
            Return Me.dateField
        End Get
        Set(ByVal value As Date)
            Me.dateField = Value
        End Set
    End Property


    '''<remarks/>
    Public Property SentenceTypeCode() As String
        Get
            Return Me.sentenceTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sentenceTypeCodeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property SentenceDescription() As String
        Get
            Return Me.sentenceDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sentenceDescriptionField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property SentenceDuration() As Decimal
        Get
            Return Me.sentenceDurationField
        End Get
        Set(ByVal value As Decimal)
            Me.sentenceDurationField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property SentenceDurationQualifier() As String
        Get
            Return Me.sentenceDurationQualifierField
        End Get
        Set(ByVal value As String)
            Me.sentenceDurationQualifierField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property SentenceEffectiveDate() As Date
        Get
            Return Me.sentenceEffectiveDateField
        End Get
        Set(ByVal value As Date)
            Me.sentenceEffectiveDateField = Value
        End Set
    End Property


    '''<remarks/>
    Public Property AlcoholLevel() As Decimal
        Get
            Return Me.alcoholLevelField
        End Get
        Set(ByVal value As Decimal)
            Me.alcoholLevelField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property AlcoholMeasurementMethod() As String
        Get
            Return Me.alcoholMeasurementMethodField
        End Get
        Set(ByVal value As String)
            Me.alcoholMeasurementMethodField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property DrivingLicencePenaltyPoints() As Decimal
        Get
            Return Me.drivingLicencePenaltyPointsField
        End Get
        Set(ByVal value As Decimal)
            Me.drivingLicencePenaltyPointsField = Value
        End Set
    End Property

End Class

Friend Class OtherPartyInfo 'GAURAV
    Private codeField As String
    Private nameField As String
    Private typeCodeField As String
    Private licenseTypeCodeField As String
    Private licenseNumberField As String
    Private dateOfBirthField As String
    Private genderField As String
    Private driverStatusCodeField As String
    Private regNumberField As String
    Private activeIndicatorField As Boolean
    Private activeIndicatorFieldSpecified As Boolean
    Private afterHoursIndicatorField As Boolean
    Private afterHoursIndicatorFieldSpecified As Boolean
    Private priorityIndicatorField As Integer
    Private priorityIndicatorFieldSpecified As Boolean


    '''<remarks/>
    Public Property Code() As String
        Get
            Return Me.codeField
        End Get
        Set(ByVal value As String)
            Me.codeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property Name() As String
        Get
            Return Me.nameField
        End Get
        Set(ByVal value As String)
            Me.nameField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property TypeCode() As String
        Get
            Return Me.typeCodeField
        End Get
        Set(ByVal value As String)
            Me.typeCodeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property LicenseTypeCode() As String
        Get
            Return Me.licenseTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.licenseTypeCodeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property LicenseNumber() As String
        Get
            Return Me.licenseNumberField
        End Get
        Set(ByVal value As String)
            Me.licenseNumberField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property DateOfBirth() As String
        Get
            Return Me.dateOfBirthField
        End Get
        Set(ByVal value As String)
            Me.dateOfBirthField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property Gender() As String
        Get
            Return Me.genderField
        End Get
        Set(ByVal value As String)
            Me.genderField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property DriverStatusCode() As String
        Get
            Return Me.driverStatusCodeField
        End Get
        Set(ByVal value As String)
            Me.driverStatusCodeField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property RegNumber() As String
        Get
            Return Me.regNumberField
        End Get
        Set(ByVal value As String)
            Me.regNumberField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property ActiveIndicator() As Boolean
        Get
            Return Me.activeIndicatorField
        End Get
        Set(ByVal value As Boolean)
            Me.activeIndicatorField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property AfterHoursIndicator() As Boolean
        Get
            Return Me.afterHoursIndicatorField
        End Get
        Set(ByVal value As Boolean)
            Me.afterHoursIndicatorField = Value
        End Set
    End Property

    '''<remarks/>
    Public Property PriorityIndicator() As Integer
        Get
            Return Me.priorityIndicatorField
        End Get
        Set(ByVal value As Integer)
            Me.priorityIndicatorField = Value
        End Set
    End Property

End Class

Friend Class OpenClaimXMLStructure

    Public Claim As cClaim

    Public Sub LoadClaimStructure(ByVal oStructure As XmlNode, ByVal oTestData As TestData)

        Claim = New cClaim

        Claim.LoadClaim(oStructure, oTestData)

    End Sub



    Friend Class cClaim

        Public InsurancefileKey As Integer
        Public RiskKey As Integer
        Public BranchCode As String
        Public Description As String
        Public ProgressStatusCode As String
        Public PrimaryCauseCode As String
        Public LossFromDate As Date
        Public ReportedDate As Date
        Public HandlerCode As String
        Public InfoOnly As Boolean
        Public LikelyClaim As Boolean
        Public SecondaryCauseCode As String
        Public CatastropheCode As String
        Public ReportedToDate As Date
        Public LossToDate As Date
        Public Location As String
        Public TownCode As String
        Public UserDefFldACode As String
        Public UserDefFldBCode As String
        Public UserDefFldCCode As String
        Public UserDefFldDCode As String
        Public UserDefFldECode As String
        Public Comments As String
        Public ClaimVersionDescription As String
        Public CurrencyCode As String
        Public UnderwritingYearCode As String
        Public IgnoreWarnings As Boolean
        Public ClaimPerils As New List(Of cClaimPeril)
        Public Client As cClient
        Public Insurer As cInsurer
        Public ReinsuranceDeferredInsuranceFilecnt As Integer
        Public ReinsuranceDeferredRiskCnt As Integer
        Public PolicyVoidedInsuranceFileCnt As Integer
        Public PolicyVoidedRiskCnt As Integer
        Public PolicyExpiryDateBeforeLossDateInsurancefileCnt As Integer
        Public PolicyExpiryDateBeforeLossDateRiskCnt As Integer
        Public PolicyStartDateAfterLossDateInsuranceFileCnt As Integer
        Public PolicyStartDateAfterLossDateRiskCnt As Integer
        Public DuplicateClaimInsuranceFileCnt As Integer
        Public DuplicateClaimRiskCnt As Integer
        Public DuplicateClaimLossFromDate As Date


        Friend Sub LoadClaim(ByVal oClaimNode As XmlNode, ByVal oTestData As TestData)

            Dim oNode As XmlNode

            For Each oNode In oClaimNode.ChildNodes

                Select Case oNode.Name

                    Case "CLAIMPERIL"

                        Dim oClaimPeril As cClaimPeril = New cClaimPeril

                        oClaimPeril.LoadClaimPeril(oNode)

                        ClaimPerils.Add(oClaimPeril)

                    Case "CLIENT"
                        Dim oClient As cClient = New cClient

                        oClient.LoadClient(oNode)

                        Client = oClient

                    Case "INSURER"

                        Dim oInsurer As cInsurer = New cInsurer

                        oInsurer.LoadInsurer(oNode)

                        Insurer = oInsurer

                End Select

            Next

            Me.BranchCode = oTestData.CheckAttribute(oClaimNode, "BranchCode")
            Me.InsurancefileKey = oTestData.CheckAttribute(oClaimNode, "InsuranceFileCnt")
            Me.RiskKey = oTestData.CheckAttribute(oClaimNode, "RiskCnt")
            Me.Description = oTestData.CheckAttribute(oClaimNode, "Description")
            Me.Comments = oTestData.CheckAttribute(oClaimNode, "Comments")
            Me.CatastropheCode = oTestData.CheckAttribute(oClaimNode, "CatastropheCode")
            Me.HandlerCode = oTestData.CheckAttribute(oClaimNode, "HandlerCode")
            Me.ClaimVersionDescription = oTestData.CheckAttribute(oClaimNode, "ClaimVersionDescription")
            Me.LikelyClaim = oTestData.CheckAttribute(oClaimNode, "LikelyClaim")
            Me.Location = oTestData.CheckAttribute(oClaimNode, "Location")
            Me.LossFromDate = oTestData.CheckAttribute(oClaimNode, "LossFromDate")
            Me.LossToDate = oTestData.CheckAttribute(oClaimNode, "LossToDate")
            Me.PrimaryCauseCode = oTestData.CheckAttribute(oClaimNode, "PrimaryCauseCode")
            Me.ProgressStatusCode = oTestData.CheckAttribute(oClaimNode, "ProgressStatusCode")
            Me.ReportedDate = oTestData.CheckAttribute(oClaimNode, "ReportedDate")
            Me.TownCode = oTestData.CheckAttribute(oClaimNode, "TownCode")
            Me.ReportedToDate = oTestData.CheckAttribute(oClaimNode, "ReportedToDate")
            Me.SecondaryCauseCode = oTestData.CheckAttribute(oClaimNode, "SecondaryCauseCode")
            Me.UnderwritingYearCode = oTestData.CheckAttribute(oClaimNode, "UnderwritingYearCode")
            Me.UserDefFldACode = oTestData.CheckAttribute(oClaimNode, "UserDefFldACode")
            Me.UserDefFldBCode = oTestData.CheckAttribute(oClaimNode, "UserDefFldBCode")
            Me.UserDefFldCCode = oTestData.CheckAttribute(oClaimNode, "UserDefFldCCode")
            Me.UserDefFldDCode = oTestData.CheckAttribute(oClaimNode, "UserDefFldDCode")
            Me.UserDefFldECode = oTestData.CheckAttribute(oClaimNode, "UserDefFldECode")
            Me.InfoOnly = oTestData.CheckAttribute(oClaimNode, "InfoOnly")
            Me.CurrencyCode = oTestData.CheckAttribute(oClaimNode, "CurrencyCode")
            Me.IgnoreWarnings = oTestData.CheckAttribute(oClaimNode, "IgnoreWarnings")
            Me.ReinsuranceDeferredInsuranceFilecnt = oTestData.CheckAttribute(oClaimNode, "RiskDeferredInsuranceFileCnt")
            Me.ReinsuranceDeferredRiskCnt = oTestData.CheckAttribute(oClaimNode, "RiskDeferredRiskCnt")
            Me.PolicyVoidedInsuranceFileCnt = oTestData.CheckAttribute(oClaimNode, "PolicyVoidedInsuranceFileCnt")
            Me.PolicyVoidedRiskCnt = oTestData.CheckAttribute(oClaimNode, "PolicyVoidedRiskCnt")
            Me.PolicyExpiryDateBeforeLossDateInsurancefileCnt = oTestData.CheckAttribute(oClaimNode, "PolicyWithExpiryDateBeforeClaimLossDateInsuranceFileCnt", 0)
            Me.PolicyExpiryDateBeforeLossDateRiskCnt = oTestData.CheckAttribute(oClaimNode, "PolicyWithExpiryDateBeforeClaimLossDateRiskCnt", 0)
            Me.PolicyStartDateAfterLossDateInsuranceFileCnt = oTestData.CheckAttribute(oClaimNode, "PolicyWithStartDateAfterClaimLossDateInsuranceFileCnt", 0)
            Me.PolicyStartDateAfterLossDateRiskCnt = oTestData.CheckAttribute(oClaimNode, "PolicyWithStartDateAfterClaimLossDateRiskCnt", 0)
            Me.DuplicateClaimInsuranceFileCnt = oTestData.CheckAttribute(oClaimNode, "DuplicateClaimInsuranceFileCnt", 0)
            Me.DuplicateClaimRiskCnt = oTestData.CheckAttribute(oClaimNode, "DuplicateClaimRiskCnt", 0)
            Me.DuplicateClaimLossFromDate = oTestData.CheckAttribute(oClaimNode, "DuplicateClaimLossFromDate")
        End Sub

    End Class

    Friend Class cClaimPeril
        Public Recovery As New List(Of cRecovery)
        Public Reserve As New List(Of cReserve)

        Public Description As String
        Public TypeCode As String

        Public Sub LoadClaimPeril(ByVal oClaimPerilNode As XmlNode)

            For Each oNode As XmlNode In oClaimPerilNode.ChildNodes

                Select Case oNode.Name

                    Case "RECOVERY"

                        Dim oRecovery As cRecovery = New cRecovery

                        oRecovery.LoadRecovery(oNode)

                        Recovery.Add(oRecovery)

                    Case "RESERVE"

                        Dim oReserve As cReserve = New cReserve

                        oReserve.LoadReserve(oNode)

                        Reserve.Add(oReserve)

                End Select

            Next

            Me.Description = oClaimPerilNode.Attributes.GetNamedItem("Description").Value
            Me.TypeCode = oClaimPerilNode.Attributes.GetNamedItem("TypeCode").Value

        End Sub

    End Class

    Friend Class cRecovery
        Public TypeCode As String
        Public RevisionAmount As Decimal

        Public Sub LoadRecovery(ByVal oRecoveryNode As XmlNode)

            Me.TypeCode = oRecoveryNode.Attributes.GetNamedItem("TypeCode").Value
            Me.RevisionAmount = oRecoveryNode.Attributes.GetNamedItem("RevisionAmount").Value

        End Sub

    End Class

    Friend Class cReserve
        Public TypeCode As String
        Public RevisionAmount As Decimal

        Public Sub LoadReserve(ByVal oReserveNode As XmlNode)
            Me.TypeCode = oReserveNode.Attributes.GetNamedItem("TypeCode").Value
            Me.RevisionAmount = oReserveNode.Attributes.GetNamedItem("RevisionAmount").Value
        End Sub

    End Class

    Friend Class cClient
        Public VatRegistered As Boolean
        Public VatRegNumber As String
        Public AddressTypeCode As String
        Public Addressline1 As String
        Public Addressline2 As String
        Public Addressline3 As String
        Public Addressline4 As String
        Public PostCode As String
        Public CountryCode As String
        Public ClaimNumber As String

        Public Contacts As New List(Of cContact)

        Public Sub LoadClient(ByVal oClientNode As XmlNode)

            For Each oNode As XmlNode In oClientNode.ChildNodes


                Select Case oNode.Name

                    Case "CONTACT"

                        Dim oContact As cContact = New cContact

                        oContact.LoadContact(oNode)

                        Contacts.Add(oContact)

                End Select
            Next

            Me.VatRegistered = oClientNode.Attributes.GetNamedItem("TaxRegistered").Value
            Me.VatRegNumber = oClientNode.Attributes.GetNamedItem("TaxRegistrationNumber").Value
            Me.AddressTypeCode = oClientNode.Attributes.GetNamedItem("AddressTypeCode").Value
            Me.Addressline1 = oClientNode.Attributes.GetNamedItem("AddressLine1").Value
            Me.Addressline2 = oClientNode.Attributes.GetNamedItem("AddressLine2").Value
            Me.Addressline3 = oClientNode.Attributes.GetNamedItem("AddressLine3").Value
            Me.Addressline4 = oClientNode.Attributes.GetNamedItem("AddressLine4").Value
            Me.PostCode = oClientNode.Attributes.GetNamedItem("PostCode").Value
            Me.CountryCode = oClientNode.Attributes.GetNamedItem("CountryCode").Value
            Me.ClaimNumber = oClientNode.Attributes.GetNamedItem("PartyClaimNumber").Value

        End Sub

    End Class

    Friend Class cInsurer
        Public AddressTypeCode As String
        Public Addressline1 As String
        Public Addressline2 As String
        Public Addressline3 As String
        Public Addressline4 As String
        Public PostCode As String
        Public CountryCode As String
        Public ClaimNumber As String
        Public ContactName As String

        Public Contacts As New List(Of cContact)

        Public Sub LoadInsurer(ByVal oInsurerNode As XmlNode)

            For Each oNode As XmlNode In oInsurerNode.ChildNodes

                Select Case oNode.Name

                    Case "CONTACT"

                        Dim oContact As cContact = New cContact

                        oContact.LoadContact(oNode)

                        Contacts.Add(oContact)

                End Select

            Next

            Me.AddressTypeCode = oInsurerNode.Attributes.GetNamedItem("AddressTypeCode").Value
            Me.Addressline1 = oInsurerNode.Attributes.GetNamedItem("AddressLine1").Value
            Me.Addressline2 = oInsurerNode.Attributes.GetNamedItem("AddressLine2").Value
            Me.Addressline3 = oInsurerNode.Attributes.GetNamedItem("AddressLine3").Value
            Me.Addressline4 = oInsurerNode.Attributes.GetNamedItem("AddressLine4").Value
            Me.PostCode = oInsurerNode.Attributes.GetNamedItem("PostCode").Value
            Me.CountryCode = oInsurerNode.Attributes.GetNamedItem("CountryCode").Value
            Me.ClaimNumber = oInsurerNode.Attributes.GetNamedItem("PartyClaimNumber").Value
            Me.ContactName = oInsurerNode.Attributes.GetNamedItem("ContactName").Value

        End Sub

    End Class

    Friend Class cContact

        Public ContactTypeCode As String
        Public AreaCode As String
        Public ContactDetail As String

        Public Sub LoadContact(ByVal oContactNode As XmlNode)
            Me.ContactTypeCode = oContactNode.Attributes.GetNamedItem("ContactTypeCode").Value
            Me.AreaCode = oContactNode.Attributes.GetNamedItem("AreaCode").Value
            Me.ContactDetail = oContactNode.Attributes.GetNamedItem("ContactDetail").Value
        End Sub

    End Class

End Class

Friend Class MaintainClaimXMLStructure

    Public Claim As cClaim


    Public Sub LoadClaimStructure(ByVal oStructure As XmlNode, ByVal oTestData As TestData)

        Claim = New cClaim

        Claim.LoadClaim(oStructure, oTestData)

    End Sub

    Friend Class cClaim

        Public BranchCode As String
        Public Description As String
        Public ProgressStatusCode As String
        Public PrimaryCauseCode As String
        Public LossFromDate As Date
        Public ReportedDate As Date
        Public HandlerCode As String
        Public InfoOnly As Boolean
        Public LikelyClaim As Boolean
        Public SecondaryCauseCode As String
        Public CatastropheCode As String
        Public ReportedToDate As Date
        Public LossToDate As Date
        Public Location As String
        Public TownCode As String
        Public UserDefFldACode As String
        Public UserDefFldBCode As String
        Public UserDefFldCCode As String
        Public UserDefFldDCode As String
        Public UserDefFldECode As String
        Public Comments As String
        Public ClaimVersionDescription As String
        Public IgnoreWarnings As Boolean
        Public ClaimPerils As New List(Of cClaimPeril)
        Public Client As cClient
        Public Insurer As cInsurer
        Public ExternalHandler As Boolean
        Public BaseClaimKey As Integer
        Public RiskDeferredBaseClaimKey As Integer
        Public PolicyVoidedBaseClaimKey As Integer
        Public PolicyWithExpiryDateBeforeClaimLossDateBaseClaimKey As Integer
        Public PolicyWithStartDateAfterClaimLossDateBaseClaimKey As Integer
        Public RiskDeferredClaimDate As Date
        Public PolicyVoidedClaimDate As Date
        Public PolicyWithExpiryDateBeforeClaimLossDateClaimDate As Date
        Public PolicyWithStartDateAfterClaimLossDateClaimDate As Date

        Friend Sub LoadClaim(ByVal oClaimNode As XmlNode, ByVal oTestData As TestData)

            Dim oNode As XmlNode

            For Each oNode In oClaimNode.ChildNodes

                Select Case oNode.Name

                    Case "CLAIMPERIL"

                        Dim oClaimPeril As cClaimPeril = New cClaimPeril

                        oClaimPeril.LoadClaimPeril(oNode, oTestData)

                        ClaimPerils.Add(oClaimPeril)

                    Case "CLIENT"

                        Dim oClient As cClient = New cClient

                        oClient.LoadClient(oNode, oTestData)

                        Client = oClient

                    Case "INSURER"

                        Dim oInsurer As cInsurer = New cInsurer

                        oInsurer.LoadInsurer(oNode, oTestData)

                        Insurer = oInsurer

                End Select

            Next

            Me.BranchCode = oTestData.CheckAttribute(oClaimNode, "BranchCode")
            Me.Description = oTestData.CheckAttribute(oClaimNode, "Description")
            Me.Comments = oTestData.CheckAttribute(oClaimNode, "Comments")
            Me.CatastropheCode = oTestData.CheckAttribute(oClaimNode, "CatastropheCode")
            Me.HandlerCode = oTestData.CheckAttribute(oClaimNode, "HandlerCode")
            Me.ClaimVersionDescription = oTestData.CheckAttribute(oClaimNode, "ClaimVersionDescription")
            Me.LikelyClaim = oTestData.CheckAttribute(oClaimNode, "LikelyClaim")
            Me.Location = oTestData.CheckAttribute(oClaimNode, "Location")
            Me.LossFromDate = oTestData.CheckAttribute(oClaimNode, "LossFromDate")
            Me.LossToDate = oTestData.CheckAttribute(oClaimNode, "LossToDate")
            Me.PrimaryCauseCode = oTestData.CheckAttribute(oClaimNode, "PrimaryCauseCode")
            Me.ProgressStatusCode = oTestData.CheckAttribute(oClaimNode, "ProgressStatusCode")
            Me.ReportedDate = oTestData.CheckAttribute(oClaimNode, "ReportedDate")
            Me.TownCode = oTestData.CheckAttribute(oClaimNode, "TownCode")
            Me.ReportedToDate = oTestData.CheckAttribute(oClaimNode, "ReportedToDate")
            Me.SecondaryCauseCode = oTestData.CheckAttribute(oClaimNode, "SecondaryCauseCode")
            Me.UserDefFldACode = oTestData.CheckAttribute(oClaimNode, "UserDefFldACode")
            Me.UserDefFldBCode = oTestData.CheckAttribute(oClaimNode, "UserDefFldBCode")
            Me.UserDefFldCCode = oTestData.CheckAttribute(oClaimNode, "UserDefFldCCode")
            Me.UserDefFldDCode = oTestData.CheckAttribute(oClaimNode, "UserDefFldDCode")
            Me.UserDefFldECode = oTestData.CheckAttribute(oClaimNode, "UserDefFldECode")
            Me.InfoOnly = oTestData.CheckAttribute(oClaimNode, "InfoOnly")
            Me.IgnoreWarnings = oTestData.CheckAttribute(oClaimNode, "IgnoreWarnings")
            Me.ExternalHandler = oTestData.CheckAttribute(oClaimNode, "ExternalHandler")
            Me.BaseClaimKey = oTestData.CheckAttribute(oClaimNode, "BaseClaimKey")

            Me.RiskDeferredBaseClaimKey = oTestData.CheckAttribute(oClaimNode, "RiskDeferredBaseClaimKey")
            Me.RiskDeferredClaimDate = oTestData.CheckAttribute(oClaimNode, "RiskDeferredClaimDate")

            Me.PolicyVoidedBaseClaimKey = oTestData.CheckAttribute(oClaimNode, "PolicyVoidedBaseClaimKey")
            Me.PolicyVoidedClaimDate = oTestData.CheckAttribute(oClaimNode, "PolicyVoidedClaimDate")

            Me.PolicyWithExpiryDateBeforeClaimLossDateBaseClaimKey = oTestData.CheckAttribute(oClaimNode, "PolicyWithExpiryDateBeforeClaimLossDateBaseClaimKey")
            Me.PolicyWithExpiryDateBeforeClaimLossDateClaimDate = oTestData.CheckAttribute(oClaimNode, "PolicyWithExpiryDateBeforeClaimLossDateClaimDate")

            Me.PolicyWithStartDateAfterClaimLossDateBaseClaimKey = oTestData.CheckAttribute(oClaimNode, "PolicyWithStartDateAfterClaimLossDateBaseClaimKey")
            Me.PolicyWithStartDateAfterClaimLossDateClaimDate = oTestData.CheckAttribute(oClaimNode, "PolicyWithStartDateAfterClaimLossDateClaimDate")


        End Sub

    End Class

    Friend Class cClaimPeril
        Public Recovery As New List(Of cRecovery)
        Public Reserve As New List(Of cReserve)

        Public Description As String
        Public TypeCode As String
        Public BaseClaimPerilKey As Integer

        Public Sub LoadClaimPeril(ByVal oClaimPerilNode As XmlNode, ByVal oTestData As TestData)

            For Each oNode As XmlNode In oClaimPerilNode.ChildNodes

                Select Case oNode.Name

                    Case "RECOVERY"

                        Dim oRecovery As cRecovery = New cRecovery

                        oRecovery.LoadRecovery(oNode, oTestData)

                        Recovery.Add(oRecovery)

                    Case "RESERVE"

                        Dim oReserve As cReserve = New cReserve

                        oReserve.LoadReserve(oNode, oTestData)

                        Reserve.Add(oReserve)

                End Select

            Next

            Me.Description = oTestData.CheckAttribute(oClaimPerilNode, "Description")
            Me.TypeCode = oTestData.CheckAttribute(oClaimPerilNode, "TypeCode")
            Me.BaseClaimPerilKey = oTestData.CheckAttribute(oClaimPerilNode, "BaseClaimPerilKey")

        End Sub

    End Class

    Friend Class cRecovery
        Public TypeCode As String
        Public RevisionAmount As Decimal

        Public Sub LoadRecovery(ByVal oRecoveryNode As XmlNode, ByVal oTestData As TestData)

            Me.TypeCode = oRecoveryNode.Attributes.GetNamedItem("TypeCode").Value
            Me.RevisionAmount = oRecoveryNode.Attributes.GetNamedItem("RevisionAmount").Value

        End Sub

    End Class

    Friend Class cReserve
        Public TypeCode As String
        Public RevisionAmount As Decimal

        Public Sub LoadReserve(ByVal oReserveNode As XmlNode, ByVal oTestData As TestData)
            Me.TypeCode = oTestData.CheckAttribute(oReserveNode, "TypeCode")
            Me.RevisionAmount = oTestData.CheckAttribute(oReserveNode, "RevisionAmount")
        End Sub

    End Class

    Friend Class cClient
        Public VatRegistered As Boolean
        Public VatRegNumber As String
        Public AddressTypeCode As String
        Public Addressline1 As String
        Public Addressline2 As String
        Public Addressline3 As String
        Public Addressline4 As String
        Public PostCode As String
        Public CountryCode As String
        Public ClaimNumber As String

        Public Contacts As New List(Of cContact)

        Public Sub LoadClient(ByVal oClientNode As XmlNode, ByVal oTestData As TestData)

            For Each oNode As XmlNode In oClientNode.ChildNodes

                Select Case oNode.Name

                    Case "CONTACT"

                        Dim oContact As cContact = New cContact

                        oContact.LoadContact(oNode, oTestData)

                        Contacts.Add(oContact)

                End Select
            Next

            Me.VatRegistered = oTestData.CheckAttribute(oClientNode, "TaxRegistered")
            Me.VatRegNumber = oTestData.CheckAttribute(oClientNode, "TaxRegistrationNumber")
            Me.AddressTypeCode = oTestData.CheckAttribute(oClientNode, "AddressTypeCode")
            Me.Addressline1 = oTestData.CheckAttribute(oClientNode, "AddressLine1")
            Me.Addressline2 = oTestData.CheckAttribute(oClientNode, "AddressLine2")
            Me.Addressline3 = oTestData.CheckAttribute(oClientNode, "AddressLine3")
            Me.Addressline4 = oTestData.CheckAttribute(oClientNode, "AddressLine4")
            Me.PostCode = oTestData.CheckAttribute(oClientNode, "PostCode")
            Me.CountryCode = oTestData.CheckAttribute(oClientNode, "CountryCode")
            Me.ClaimNumber = oTestData.CheckAttribute(oClientNode, "PartyClaimNumber")

        End Sub

    End Class

    Friend Class cInsurer
        Public AddressTypeCode As String
        Public Addressline1 As String
        Public Addressline2 As String
        Public Addressline3 As String
        Public Addressline4 As String
        Public PostCode As String
        Public CountryCode As String
        Public ClaimNumber As String
        Public ContactName As String

        Public Contacts As New List(Of cContact)

        Public Sub LoadInsurer(ByVal oInsurerNode As XmlNode, ByVal oTestData As TestData)

            For Each oNode As XmlNode In oInsurerNode.ChildNodes

                Select Case oNode.Name

                    Case "CONTACT"

                        Dim oContact As cContact = New cContact

                        oContact.LoadContact(oNode, oTestData)

                        Contacts.Add(oContact)

                End Select

            Next

            Me.AddressTypeCode = oTestData.CheckAttribute(oInsurerNode, "AddressTypeCode")
            Me.Addressline1 = oTestData.CheckAttribute(oInsurerNode, "AddressLine1")
            Me.Addressline2 = oTestData.CheckAttribute(oInsurerNode, "AddressLine2")
            Me.Addressline3 = oTestData.CheckAttribute(oInsurerNode, "AddressLine3")
            Me.Addressline4 = oTestData.CheckAttribute(oInsurerNode, "AddressLine4")
            Me.PostCode = oTestData.CheckAttribute(oInsurerNode, "PostCode")
            Me.CountryCode = oTestData.CheckAttribute(oInsurerNode, "CountryCode")
            Me.ClaimNumber = oTestData.CheckAttribute(oInsurerNode, "PartyClaimNumber")
            Me.ContactName = oTestData.CheckAttribute(oInsurerNode, "ContactName")

        End Sub

    End Class

    Friend Class cContact

        Public ContactTypeCode As String
        Public AreaCode As String
        Public ContactDetail As String

        Public Sub LoadContact(ByVal oContactNode As XmlNode, ByVal oTestData As TestData)

            Me.ContactTypeCode = oTestData.CheckAttribute(oContactNode, "ContactTypeCode")
            Me.AreaCode = oTestData.CheckAttribute(oContactNode, "AreaCode")
            Me.ContactDetail = oTestData.CheckAttribute(oContactNode, "ContactDetail")
        End Sub

    End Class

End Class

Friend Class XMLElementToAdd

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

Friend Class XMLAttributeToAdd

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
Friend Class ClaimReceiptAndTaxes
    Private baseClaimKeyField As Long
    Private partyKeyField As Integer
    Private currencyCodeField As String
    Private claimVersonDescriptionField As String
    Private baseClaimPerilKeyField As String
    Private isSettlementField As Boolean
    Private isTaxExemptField As Boolean
    Private receivableTaxPercentageField As Decimal
    Private insuredDomiciledField As Boolean
    Private insuredPercentageField As Decimal
    Private insuredTaxNumberField As String
    Private baseRecoveryKeyField As Long
    Private taxGroupCodeField As String
    Private nameField As String
    Private bankNameField As String
    Private bankNumberField As String
    Private bankCodeField As String
    Private mediaTypeCodeField As String
    Private mediaReferenceField As String
    Private theirReferenceField As String

    Private addressLine1Field As String
    Private addressLine2Field As String
    Private addressLine3Field As String
    Private addressLine4Field As String
    Private countryCodeField As String
    Private postCodeField As String
    Private receiptAmountField As Decimal
    Private commentsField As String
    Private ReceiptPartyType As String
    '''<remarks/>
    Public Property ReceiptType() As String
        Get
            Return Me.ReceiptPartyType
        End Get
        Set(ByVal value As String)
            Me.ReceiptPartyType = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClaimVersonDescription() As String
        Get
            Return Me.claimVersonDescriptionField
        End Get
        Set(ByVal value As String)
            Me.claimVersonDescriptionField = value
        End Set
    End Property
    '''<remarks/>
    Public Property PostCode() As String
        Get
            Return Me.postCodeField
        End Get
        Set(ByVal value As String)
            Me.postCodeField = value
        End Set
    End Property
    '''<remarks/>
    Public Property AddressLine1() As String
        Get
            Return Me.addressLine1Field
        End Get
        Set(ByVal value As String)
            Me.addressLine1Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property AddressLine2() As String
        Get
            Return Me.addressLine2Field
        End Get
        Set(ByVal value As String)
            Me.addressLine2Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property AddressLine3() As String
        Get
            Return Me.addressLine3Field
        End Get
        Set(ByVal value As String)
            Me.addressLine3Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property AddressLine4() As String
        Get
            Return Me.addressLine4Field
        End Get
        Set(ByVal value As String)
            Me.addressLine4Field = value
        End Set
    End Property

    '''<remarks/>
    Public Property CountryCode() As String
        Get
            Return Me.countryCodeField
        End Get
        Set(ByVal value As String)
            Me.countryCodeField = value
        End Set
    End Property


    '''<remarks/>
    Public Property Name() As String
        Get
            Return Me.nameField
        End Get
        Set(ByVal value As String)
            Me.nameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Comments() As String
        Get
            Return Me.commentsField
        End Get
        Set(ByVal value As String)
            Me.commentsField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankName() As String
        Get
            Return Me.bankNameField
        End Get
        Set(ByVal value As String)
            Me.bankNameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankNumber() As String
        Get
            Return Me.bankNumberField
        End Get
        Set(ByVal value As String)
            Me.bankNumberField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BankCode() As String
        Get
            Return Me.bankCodeField
        End Get
        Set(ByVal value As String)
            Me.bankCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeCode() As String
        Get
            Return Me.mediaTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.mediaTypeCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaReference() As String
        Get
            Return Me.mediaReferenceField
        End Get
        Set(ByVal value As String)
            Me.mediaReferenceField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TheirReference() As String
        Get
            Return Me.theirReferenceField
        End Get
        Set(ByVal value As String)
            Me.theirReferenceField = value
        End Set
    End Property


    '''<remarks/>
    Public Property IsSettlement() As Boolean
        Get
            Return Me.isSettlementField
        End Get
        Set(ByVal value As Boolean)
            Me.isSettlementField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsTaxExempt() As Boolean
        Get
            Return Me.isTaxExemptField
        End Get
        Set(ByVal value As Boolean)
            Me.isTaxExemptField = value
        End Set
    End Property


    '''<remarks/>
    Public Property ReceivableTaxPercentage() As Decimal
        Get
            Return Me.receivableTaxPercentageField
        End Get
        Set(ByVal value As Decimal)
            Me.receivableTaxPercentageField = value
        End Set
    End Property


    '''<remarks/>
    Public Property InsuredDomiciled() As Boolean
        Get
            Return Me.insuredDomiciledField
        End Get
        Set(ByVal value As Boolean)
            Me.insuredDomiciledField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredPercentage() As Decimal
        Get
            Return Me.insuredPercentageField
        End Get
        Set(ByVal value As Decimal)
            Me.insuredPercentageField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuredTaxNumber() As String
        Get
            Return Me.insuredTaxNumberField
        End Get
        Set(ByVal value As String)
            Me.insuredTaxNumberField = value
        End Set
    End Property

    Public Property BaseClaimKey() As Long
        Get
            Return Me.baseClaimKeyField
        End Get
        Set(ByVal value As Long)
            Me.baseClaimKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BaseClaimPerilKey() As String
        Get
            Return Me.baseClaimPerilKeyField
        End Get
        Set(ByVal value As String)
            Me.baseClaimPerilKeyField = value
        End Set
    End Property


    '''<remarks/>
    Public Property BaseRecoveryKey() As Long
        Get
            Return Me.baseRecoveryKeyField
        End Get
        Set(ByVal value As Long)
            Me.baseRecoveryKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TaxGroupCode() As String
        Get
            Return Me.taxGroupCodeField
        End Get
        Set(ByVal value As String)
            Me.taxGroupCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReceiptAmount() As Decimal
        Get
            Return Me.receiptAmountField
        End Get
        Set(ByVal value As Decimal)
            Me.receiptAmountField = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return Me.partyKeyField
        End Get
        Set(ByVal value As Integer)
            Me.partyKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyCode() As String
        Get
            Return Me.currencyCodeField
        End Get
        Set(ByVal value As String)
            Me.currencyCodeField = value
        End Set
    End Property
    'end of the class
End Class




Friend Class Address

    Private m_nTypeCode As Integer
    Private m_sLine1 As String
    Private m_sLine2 As String
    Private m_sLine3 As String
    Private m_sLine4 As String
    Private m_sPostCode As String
    Private m_sCountryCode As String
    Private m_sContact As Contact()

    'DONE - 09-OCT-2006
    Public Property Contact() As Contact()
        Get
            Contact = m_sContact
        End Get
        Set(ByVal value As Contact())
            m_sContact = value
        End Set
    End Property

    Public Property TypeCode() As Integer
        Get
            TypeCode = m_nTypeCode
        End Get
        Set(ByVal value As Integer)
            m_nTypeCode = value
        End Set
    End Property

    Public Property CountryCode() As String
        Get
            CountryCode = m_sCountryCode
        End Get
        Set(ByVal value As String)
            m_sCountryCode = value
        End Set
    End Property

    Public Property Line1() As String
        Get
            Line1 = m_sLine1
        End Get
        Set(ByVal value As String)
            m_sLine1 = value
        End Set
    End Property

    Public Property Line2() As String
        Get
            Line2 = m_sLine2
        End Get
        Set(ByVal value As String)
            m_sLine2 = value
        End Set
    End Property

    Public Property Line3() As String
        Get
            Line3 = m_sLine3
        End Get
        Set(ByVal value As String)
            m_sLine3 = value
        End Set
    End Property

    Public Property Line4() As String
        Get
            Line4 = m_sLine4
        End Get
        Set(ByVal value As String)
            m_sLine4 = value
        End Set
    End Property

    Public Property PostCode() As String
        Get
            PostCode = m_sPostCode
        End Get
        Set(ByVal value As String)
            m_sPostCode = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub loadAddress(ByVal oAddress As XmlNode) 'GAURAV

        Me.TypeCode = oAddress.Attributes.GetNamedItem("Type").Value
        Me.CountryCode = oAddress.Attributes.GetNamedItem("CountryCode").Value
        Me.Line1 = oAddress.Attributes.GetNamedItem("Line1").Value
        Me.Line2 = oAddress.Attributes.GetNamedItem("Line2").Value
        Me.Line3 = oAddress.Attributes.GetNamedItem("Line3").Value
        Me.Line4 = oAddress.Attributes.GetNamedItem("Line4").Value
        Me.PostCode = oAddress.Attributes.GetNamedItem("PostCode").Value
    End Sub

End Class

Friend Class Contact

    Private m_sAreaCode As String
    Private m_nType As Integer
    Private m_nElementName As Integer
    Private m_sItem As String

    Public Property Type() As Integer
        Get
            Type = m_nType
        End Get
        Set(ByVal value As Integer)
            m_nType = value
        End Set
    End Property

    Public Property ElementName() As Integer
        Get
            ElementName = m_nElementName
        End Get
        Set(ByVal value As Integer)
            m_nElementName = value
        End Set
    End Property

    Public Property AreaCode() As String
        Get
            AreaCode = m_sAreaCode
        End Get
        Set(ByVal value As String)
            m_sAreaCode = value
        End Set
    End Property

    Public Property Item() As String
        Get
            Item = m_sItem
        End Get
        Set(ByVal value As String)
            m_sItem = value
        End Set
    End Property
End Class
Friend Class ClaimReceiptTaxes
    Public ClaimReceipt As cClaimReceipt
    Public Sub LoadClaimReceiptStructure(ByVal oStructure As XmlNode, ByVal oTestData As TestData)
        ClaimReceipt = New cClaimReceipt
        ClaimReceipt.LoadClaimReceipt(oStructure, oTestData)
    End Sub
    Friend Class cClaimRecoveryItems
        'Items
        Public baseRecoveryKeyField As Long
        Public taxGroupCodeField As String
        Public receiptAmountField As Decimal

        Public Sub LoadRecoveryItems(ByVal oRecoveryNode As XmlNode)
            Me.baseRecoveryKeyField = oRecoveryNode.Attributes.GetNamedItem("BaseRecoveryKey").Value
            Me.receiptAmountField = oRecoveryNode.Attributes.GetNamedItem("ReceiptAmount").Value
            Me.taxGroupCodeField = oRecoveryNode.Attributes.GetNamedItem("TaxGroupCode").Value
        End Sub
    End Class
    Friend Class cClaimReceipt
        Public Recovery As New List(Of cClaimRecoveryItems)
        Public baseClaimKeyField As Long
        Public partyKeyField As Integer
        Public currencyCodeField As String
        Public claimVersonDescriptionField As String
        Public baseClaimPerilKeyField As String
        Public ReceiptPartyType As String
        'Payee
        Public nameField As String
        Public bankNameField As String
        Public bankNumberField As String
        Public bankCodeField As String
        Public mediaTypeCodeField As String
        Public mediaReferenceField As String
        Public theirReferenceField As String
        Public addressLine1Field As String
        Public addressLine2Field As String
        Public addressLine3Field As String
        Public addressLine4Field As String
        Public countryCodeField As String
        Public postCodeField As String
        'Advanced Tax details
        Public isSettlementField As Boolean
        Public isTaxExemptField As Boolean
        Public receivableTaxPercentageField As Decimal
        Public insuredDomiciledField As Boolean
        Public insuredPercentageField As Decimal
        Public insuredTaxNumberField As String


        Friend Sub LoadClaimReceipt(ByVal oClaimReceipt As XmlNode, ByVal oTestData As TestData)
            Dim oNode As XmlNode
            For Each oNode In oClaimReceipt.ChildNodes
                Select Case oNode.Name
                    Case "CLAIMRECEIPTDETAIL"
                        oClaimReceipt = oNode
                        Me.baseClaimKeyField = oTestData.CheckAttribute(oClaimReceipt, "Baseclaimkey")
                        Me.baseClaimPerilKeyField = oTestData.CheckAttribute(oClaimReceipt, "BaseClaimPerilKey")
                        Me.claimVersonDescriptionField = oTestData.CheckAttribute(oClaimReceipt, "ClaimVersionDescription")
                        Me.currencyCodeField = oTestData.CheckAttribute(oClaimReceipt, "currencyCode")
                        Me.partyKeyField = oTestData.CheckAttribute(oClaimReceipt, "PartyKey")
                        Me.ReceiptPartyType = oTestData.CheckAttribute(oClaimReceipt, "ReceiptPartyType")
                    Case "PAYEE"
                        oClaimReceipt = oNode
                        Me.nameField = oTestData.CheckAttribute(oClaimReceipt, "Name")
                        Me.bankNameField = oTestData.CheckAttribute(oClaimReceipt, "BankName")
                        Me.bankNumberField = oTestData.CheckAttribute(oClaimReceipt, "BankNumber")
                        Me.bankCodeField = oTestData.CheckAttribute(oClaimReceipt, "BankCode")
                        Me.mediaTypeCodeField = oTestData.CheckAttribute(oClaimReceipt, "MediaTypeCode")
                        Me.mediaReferenceField = oTestData.CheckAttribute(oClaimReceipt, "MediaReference")
                        Me.addressLine1Field = oTestData.CheckAttribute(oClaimReceipt, "AddressLine1")
                        Me.addressLine2Field = oTestData.CheckAttribute(oClaimReceipt, "AddressLine2")
                        Me.addressLine3Field = oTestData.CheckAttribute(oClaimReceipt, "AddressLine3")
                        Me.addressLine4Field = oTestData.CheckAttribute(oClaimReceipt, "AddressLine4")
                        Me.postCodeField = oTestData.CheckAttribute(oClaimReceipt, "PostCode")
                    Case "ADVANCEDTAXDETAILS"
                        oClaimReceipt = oNode
                        Me.insuredDomiciledField = oTestData.CheckAttribute(oClaimReceipt, "InsuredDomiciled")
                        Me.insuredPercentageField = oTestData.CheckAttribute(oClaimReceipt, "InsuredPercentage")
                        Me.insuredTaxNumberField = oTestData.CheckAttribute(oClaimReceipt, "InsuredTaxNumber")
                        Me.isSettlementField = oTestData.CheckAttribute(oClaimReceipt, "IsSettlement")
                        Me.isTaxExemptField = oTestData.CheckAttribute(oClaimReceipt, "IsTaxExempt")
                        Me.receivableTaxPercentageField = oTestData.CheckAttribute(oClaimReceipt, "ReceivableTaxPercentage")
                    Case "RECOVERY"
                        oClaimReceipt = oNode
                        Dim oRecovery As cClaimRecoveryItems = New cClaimRecoveryItems
                        oRecovery.LoadRecoveryItems(oClaimReceipt)
                        Recovery.Add(oRecovery)
                End Select
            Next

        End Sub
    End Class
End Class
Friend Class GenerateClaims

    Private modeField As Integer

    Private claimKeyField As Integer

    Private transactionTypeField As String

    Private parameterXMLField As String

    Private outputAsHTMLField As Boolean

    Private outputAsPDFField As Boolean

    '''<remarks/>
    Public Property Mode() As Integer
        Get
            Return Me.modeField
        End Get
        Set(ByVal value As Integer)
            Me.modeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimKey() As Integer
        Get
            Return Me.claimKeyField
        End Get
        Set(ByVal value As Integer)
            Me.claimKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TransactionType() As String
        Get
            Return Me.transactionTypeField
        End Get
        Set(ByVal value As String)
            Me.transactionTypeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ParameterXML() As String
        Get
            Return Me.parameterXMLField
        End Get
        Set(ByVal value As String)
            Me.parameterXMLField = value
        End Set
    End Property

    '''<remarks/>
    Public Property OutputAsHTML() As Boolean
        Get
            Return Me.outputAsHTMLField
        End Get
        Set(ByVal value As Boolean)
            Me.outputAsHTMLField = value
        End Set
    End Property

    '''<remarks/>
    Public Property OutputAsPDF() As Boolean
        Get
            Return Me.outputAsPDFField
        End Get
        Set(ByVal value As Boolean)
            Me.outputAsPDFField = value
        End Set
    End Property
End Class

Friend Class AddAgentReceiptXMLStructure

    Public BranchCode As String
    Public BankAccountName As String
    Public CurrencyCode As String
    Public ReceiptTypeCode As String
    Public MediaTypeCode As String
    Public TransactionDate As Date
    Public Amount As Decimal
    Public CashListRef As String
    Public MediaTypeIssuerCode As String
    Public MediaReference As String
    Public OurReference As String
    Public TheirReference As String
    Public ContactName As String
    Public Address1 As String
    Public Address2 As String
    Public Address3 As String
    Public Address4 As String
    Public PostalCode As String
    Public CountryCode As String
    Public ChequeName As String
    Public ChequeDate As Date
    Public CCName As String
    Public CCNumber As String
    Public CCExpiryDate As String
    Public CCStartDate As String
    Public CCIssue As String
    Public CCPin As String
    Public CCAuthCode As String
    Public CCManualAuthCode As String
    Public CCTransactionCode As String
    Public CCCustomer As String

    Friend Sub Load(ByVal oAgentReceipt As XmlNode, ByVal oTestData As TestData)

        Me.BranchCode = oTestData.CheckAttribute(oAgentReceipt, "BranchCode")
        Me.BankAccountName = oTestData.CheckAttribute(oAgentReceipt, "BankAccountName")
        Me.CurrencyCode = oTestData.CheckAttribute(oAgentReceipt, "CurrencyCode")
        Me.ReceiptTypeCode = oTestData.CheckAttribute(oAgentReceipt, "ReceiptTypeCode")
        Me.MediaTypeCode = oTestData.CheckAttribute(oAgentReceipt, "MediaTypeCode")
        Me.TransactionDate = oTestData.CheckAttribute(oAgentReceipt, "TransactionDate")
        Me.Amount = oTestData.CheckAttribute(oAgentReceipt, "Amount")
        Me.CashListRef = oTestData.CheckAttribute(oAgentReceipt, "CashListRef")
        Me.MediaTypeIssuerCode = oTestData.CheckAttribute(oAgentReceipt, "MediaTypeIssuerCode")
        Me.MediaReference = oTestData.CheckAttribute(oAgentReceipt, "MediaReference")
        Me.OurReference = oTestData.CheckAttribute(oAgentReceipt, "OurReference")
        Me.TheirReference = oTestData.CheckAttribute(oAgentReceipt, "TheirReference")
        Me.ContactName = oTestData.CheckAttribute(oAgentReceipt, "ContactName")
        Me.Address1 = oTestData.CheckAttribute(oAgentReceipt, "Address1")
        Me.Address2 = oTestData.CheckAttribute(oAgentReceipt, "Address2")
        Me.Address3 = oTestData.CheckAttribute(oAgentReceipt, "Address3")
        Me.Address4 = oTestData.CheckAttribute(oAgentReceipt, "Address4")
        Me.PostalCode = oTestData.CheckAttribute(oAgentReceipt, "PostalCode")
        Me.CountryCode = oTestData.CheckAttribute(oAgentReceipt, "CountryCode")
        Me.ChequeName = oTestData.CheckAttribute(oAgentReceipt, "ChequeName")
        Me.ChequeDate = oTestData.CheckAttribute(oAgentReceipt, "ChequeDate")
        Me.CCName = oTestData.CheckAttribute(oAgentReceipt, "CCName")
        Me.CCNumber = oTestData.CheckAttribute(oAgentReceipt, "CCNumber")
        Me.CCExpiryDate = oTestData.CheckAttribute(oAgentReceipt, "CCExpiryDate")
        Me.CCStartDate = oTestData.CheckAttribute(oAgentReceipt, "CCStartDate")
        Me.CCIssue = oTestData.CheckAttribute(oAgentReceipt, "CCIssue")
        Me.CCPin = oTestData.CheckAttribute(oAgentReceipt, "CCPin")
        Me.CCAuthCode = oTestData.CheckAttribute(oAgentReceipt, "CCAuthCode")
        Me.CCManualAuthCode = oTestData.CheckAttribute(oAgentReceipt, "CCManualAuthCode")
        Me.CCTransactionCode = oTestData.CheckAttribute(oAgentReceipt, "CCTransactionCode")
        Me.CCCustomer = oTestData.CheckAttribute(oAgentReceipt, "CCCustomer")

    End Sub
End Class

Friend Class PostDocumentXMLStructure

    Public BranchCode As String
    Public DocumentType As Integer
    Public Comment As String
    Public Transactions As New List(Of ProxyWS.BaseTransactionType)

    Friend Sub Load(ByVal oDocument As XmlNode, ByVal oTestData As TestData)

        Dim oNode As XmlNode

        For Each oNode In oDocument.ChildNodes

            Select Case oNode.Name

                Case "TRANSACTION"

                    Dim oTransaction As New ProxyWS.BaseTransactionType

                    oTransaction.Amount = oTestData.CheckAttribute(oNode, "Amount")
                    oTransaction.AccountCode = oTestData.CheckAttribute(oNode, "AccountCode")
                    oTransaction.Comment = oTestData.CheckAttribute(oNode, "Comment")
                    oTransaction.UnderwritingYearCode = oTestData.CheckAttribute(oNode, "UnderwritingYearCode")

                    Transactions.Add(oTransaction)

            End Select
        Next

        Me.BranchCode = oTestData.CheckAttribute(oDocument, "BranchCode")
        Me.DocumentType = oTestData.CheckAttribute(oDocument, "DocumentType")
        Me.Comment = oTestData.CheckAttribute(oDocument, "Comment")

    End Sub

End Class



