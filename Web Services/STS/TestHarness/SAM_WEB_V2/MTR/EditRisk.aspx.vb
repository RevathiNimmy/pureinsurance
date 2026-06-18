Imports System.Xml
Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class MTA_EditRisk
    Inherits System.Web.UI.Page

    Dim UserToken As UsernameToken
    Dim m_nRiskCnt As Integer
    'set up the proxy object
    Dim oSAM As New SAMForInsuranceV2
    Dim m_sRiskDataXML As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            UserToken = GetUserToken("sirius", "sirius")
            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

            Dim oGetListRequestType As New GetListRequestType
            Dim oGetListResponseType As GetListResponseType
            oGetListRequestType.BranchCode = "HeadOff"
            oGetListRequestType.ListType = STSListType.UserDefinedTable
            oGetListRequestType.ListCode = "MOBMAKE"
            oGetListResponseType = oSAM.GetList(oGetListRequestType)
            cboMobileMake.DataSource = oGetListResponseType.List
            cboMobileMake.DataTextField = "Description"
            cboMobileMake.DataValueField = "Key"
            cboMobileMake.DataBind()

            oGetListRequestType.BranchCode = "HeadOff"
            oGetListRequestType.ListCode = "MOBMODEL"
            oGetListResponseType = oSAM.GetList(oGetListRequestType)
            cboMobileModel.DataSource = oGetListResponseType.List
            cboMobileModel.DataTextField = "Description"
            cboMobileModel.DataValueField = "Key"
            cboMobileModel.DataBind()

            oSAM.SetClientCredential(UserToken)
            oSAM.SetPolicy("SamClientPolicy")

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
                Dim m_sRiskDataXML As String
                With oGetRiskResponseType
                    If Not (.Errors) Is Nothing Then
                        'errors returned, so throw an exception
                        Response.Write(GetMessageFromSamError(.Errors))
                    End If
                    Session("m_sRiskDataXML") = .XMLDataSet.ToString
                    m_sRiskDataXML = .XMLDataSet.ToString
                    Session("QuoteTimeStamp") = .QuoteTimeStamp
                End With

                'output dataset to the screen to show results

                Dim xmlDoc As New System.Xml.XmlDocument
                xmlDoc.LoadXml(m_sRiskDataXML)
                Dim oElement As System.Xml.XmlNode = xmlDoc.SelectSingleNode("DATA_SET") _
                        .SelectSingleNode("RISK_OBJECTS") _
                        .SelectSingleNode("DEMO_POLICY_BINDER") _
                        .SelectSingleNode("GENERAL")

                If IsNothing(oElement.Attributes("MOBILE_MAKE")) = False Then
                    cboMobileMake.SelectedValue = oElement.Attributes("MOBILE_MAKE").Value
                End If
                If IsNothing(oElement.Attributes("MOBILE_MODEL")) = False Then
                    cboMobileModel.SelectedValue = oElement.Attributes("MOBILE_MODEL").Value
                End If
                If IsNothing(oElement.Attributes("MOBILE_VALUE")) = False Then
                    txtInsuredValue.Text = oElement.Attributes("MOBILE_VALUE").Value
                End If
                If IsNothing(oElement.Attributes("SECURITY_DETAILS")) = False Then
                    txtIdentification.Text = oElement.Attributes("SECURITY_DETAILS").Value
                End If
                If IsNothing(oElement.Attributes("DATE_BIRTH")) = False Then
                    txtDate.Text = oElement.Attributes("DATE_BIRTH").Value
                End If
                If IsNothing(oElement.Attributes("OCCUPATION")) = False Then
                    txtOccupation.Text = oElement.Attributes("OCCUPATION").Value
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

    Protected Sub btnOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOK.Click
        'create user token from credentials
        'normally the credentials would come from the log in

        'set up the proxy object
        UserToken = GetUserToken("sirius", "sirius")
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")


        Try
            'output dataset to the screen to show results
            Dim oXMLElement As XMLElementToAdd1
            Dim oXMLAttr(5) As XMLAttributeToAdd1

            oXMLElement = New XMLElementToAdd1
            oXMLElement.ElementName = "GENERAL"
            oXMLAttr(0) = New XMLAttributeToAdd1
            oXMLAttr(0).AttributeName = "MOBILE_MAKE"
            oXMLAttr(0).AttributeValue = cboMobileMake.SelectedValue.ToString
            'oXMLElement.Attributes(0) = oXMLAttr(0)
            oXMLAttr(1) = New XMLAttributeToAdd1
            oXMLAttr(1).AttributeName = "MOBILE_MODEL"
            oXMLAttr(1).AttributeValue = cboMobileModel.SelectedValue.ToString
            'oXMLElement.Attributes(1) = oXMLAttr(1)
            oXMLAttr(2) = New XMLAttributeToAdd1
            oXMLAttr(2).AttributeName = "MOBILE_VALUE"
            oXMLAttr(2).AttributeValue = txtInsuredValue.Text
            'oXMLElement.Attributes(2) = oXMLAttr(2)
            oXMLAttr(3) = New XMLAttributeToAdd1
            oXMLAttr(3).AttributeName = "OCCUPATION"
            oXMLAttr(3).AttributeValue = txtOccupation.Text
            'oXMLElement.Attributes(3) = oXMLAttr(3)
            oXMLAttr(4) = New XMLAttributeToAdd1
            oXMLAttr(4).AttributeName = "SECURITY_DETAILS"
            oXMLAttr(4).AttributeValue = txtIdentification.Text
            'oXMLElement.Attributes(4) = oXMLAttr(4)
            oXMLAttr(5) = New XMLAttributeToAdd1
            oXMLAttr(5).AttributeName = "DATE_BIRTH"
            oXMLAttr(5).AttributeValue = txtDate.Text
            'oXMLElement.Attributes(5) = oXMLAttr(5)
            oXMLElement.Attributes = oXMLAttr
            Dim xmlDoc As New System.Xml.XmlDocument
            Dim oElementToAddOrUpdate As System.Xml.XmlNode
            Dim oElementToAddOrUpdateAdded As System.Xml.XmlNode
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


            Dim xmlDatasetElementsToAdd(0) As XMLElementToAdd1
            xmlDatasetElementsToAdd(0) = oXMLElement
            oRequest.TransactionType = "MTR"


            ' Create the new test elements
            For Each xmlDatasetElement As XMLElementToAdd1 In xmlDatasetElementsToAdd

                ' Check if the element already exists
                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode("DEMO_POLICY_BINDER") _
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
                Else
                    ' Update the Update
                    ' Status (US) attribute
                    oElementToAddOrUpdate.Attributes("US").Value = "2"
                    bNewElement = False
                End If

                ' Add specific attributes for this test
                For Each XMLAttribute As XMLAttributeToAdd1 In xmlDatasetElement.Attributes
                    If InStr(XMLAttribute.AttributeName, "ADDRESS_CNT") > 0 Then
                        Dim addressElement As System.Xml.XmlNode = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, XMLAttribute.AttributeName, "")

                        newAttribute = xmlDoc.CreateAttribute("US")
                        newAttribute.Value = "1"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE1")
                        newAttribute.Value = "2500 The Crescent"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE2")
                        newAttribute.Value = "Birmingham Business Park"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE3")
                        newAttribute.Value = "Solihull"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("ADDRESS_LINE4")
                        newAttribute.Value = "West Midlands"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("POSTCODE")
                        newAttribute.Value = "B37 7YE"
                        addressElement.Attributes.Append(newAttribute)

                        newAttribute = xmlDoc.CreateAttribute("COUNTRYCODE")
                        newAttribute.Value = "GBR"
                        addressElement.Attributes.Append(newAttribute)

                        oElementToAddOrUpdate.AppendChild(addressElement)

                    ElseIf (oElementToAddOrUpdate.Attributes(XMLAttribute.AttributeName) Is Nothing) = False Then
                        oElementToAddOrUpdate.Attributes(XMLAttribute.AttributeName).Value = XMLAttribute.AttributeValue
                    Else
                        newAttribute = xmlDoc.CreateAttribute(XMLAttribute.AttributeName)
                        newAttribute.Value = XMLAttribute.AttributeValue
                        oElementToAddOrUpdate.Attributes.Append(newAttribute)
                    End If
                Next

                If bNewElement Then
                    ' Append the new element node to the XML under the POLICY BINDER
                    oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                        .SelectSingleNode("RISK_OBJECTS") _
                        .SelectSingleNode("DEMO_POLICY_BINDER") _
                        .AppendChild(oElementToAddOrUpdate)
                End If

            Next

            ' Write back the next OI number to the DATA_SET node
            nNextOINumber -= 1
            xmlDoc.SelectSingleNode("DATA_SET") _
                .Attributes("NextOINumber").Value = nNextOINumber

            With oRequest



                '.RiskKey = Session("RiskKey")
                .XMLDataSet = xmlDoc.OuterXml
                .QuoteTimeStamp = Session("QuoteTimeStamp")

                .BranchCode = "Headoff"
                .ScreenCode = "MOBILE"
                .RiskDescription = "Mobile Phone Cover"

                'add values from the quote we just added
                .InsuranceFolderKey = Session("oldInsuranceFolderKey")
                .InsuranceFileKey = Session("oldkey")
                .QuoteTimeStamp = Session("QuoteTimeStamp")
                .RiskKey = Session("RiskKey")

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
            Response.Write("An error occured:<br>" & oe.Message)
            'should do some error handling here. Just output error for now

            Response.Write("An error occured:<br>" & oe.Message)
        Finally
            'clean up any objects here
        End Try
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
            .InsuranceFolderKey = Session("oldInsuranceFolderKey")
            .InsuranceFileKey = Session("oldkey")
            .QuoteTimeStamp = Session("QuoteTimeStamp")
            .RiskKey = Session("RiskKey")
            '.QuoteTimeStamp = Session("TimeStamp")
        End With

        Try
            oDeleteRiskResponseType = oSAM.DeleteRisk(oDeleteRiskRequestType)


            With oDeleteRiskResponseType
                If Not (.Errors) Is Nothing Then
                    'errors returned, so throw an exception
                    Response.Write(GetMessageFromSamError(.Errors))
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

End Class

Friend Class XMLElementToAdd1

    Private m_sName As String
    Private m_vXMLAttributes As XMLAttributeToAdd1()

    Public Property ElementName() As String
        Get
            ElementName = m_sName
        End Get
        Set(ByVal value As String)
            m_sName = value
        End Set
    End Property
    Public Property Attributes() As XMLAttributeToAdd1()
        Get
            Attributes = m_vXMLAttributes
        End Get
        Set(ByVal value As XMLAttributeToAdd1())
            m_vXMLAttributes = value
        End Set
    End Property
End Class

Friend Class XMLAttributeToAdd1

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
