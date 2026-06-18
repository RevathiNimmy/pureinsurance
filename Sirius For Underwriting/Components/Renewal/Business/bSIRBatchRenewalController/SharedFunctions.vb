Imports SharedFiles

Public NotInheritable Class SharedFunctions

#Region " CheckAttribute"

    Public Overloads Shared Function CheckAttribute(ByVal oNode As System.Xml.XmlNode, _
                    ByVal sAttrName As String, _
                    ByVal oLogFile As System.IO.StreamWriter, _
                    Optional ByVal sDefault As String = "") As String

        If oNode Is Nothing Then
            oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
            Return ""
        Else
            oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                oLogFile.WriteLine(" not found - default returned <" & sDefault & ">")
                Return sDefault
            Else
                oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & ">")
                Return oNode.Attributes(sAttrName).Value
            End If

        End If

    End Function

    Public Overloads Shared Function CheckAttribute(ByVal oNode As System.Xml.XmlNode, _
                        ByVal sAttrName As String, _
                        ByVal dtDefault As Date, _
                        ByVal oLogFile As System.IO.StreamWriter) As Date

        If oNode Is Nothing Then
            oLogFile.WriteLine("<NODE NOT FOUND!><" & sAttrName & ">")
        Else
            oLogFile.Write("<" & oNode.Name & "><" & sAttrName & ">")

            If oNode.Attributes(sAttrName) Is Nothing OrElse oNode.Attributes(sAttrName).Value.Trim = "" Then
                oLogFile.WriteLine(" not found - default returned <" & dtDefault & ">")
                Return dtDefault
            Else
                If Not IsDate(oNode.Attributes(sAttrName).Value) Then
                    oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & "> - INVALID DATE!")
                    gPMFunctions.RaiseError("CheckAttribute", "The attribute '" & sAttrName & "' in the testdata XML file within node '" & oNode.Name.ToString & "' is not a valid date.", vbObjectError + 513)
                    Return ""
                Else
                    oLogFile.WriteLine(" found <" & oNode.Attributes(sAttrName).Value & ">")
                    Return CDate(oNode.Attributes(sAttrName).Value)
                End If
            End If

        End If

    End Function

#End Region

#Region " GetODataNode"

    Friend Shared Function GetODataNode(ByVal oXML As System.Xml.XmlDocument, ByVal m_oLogFile As System.IO.StreamWriter) As XMLElementToAdd()
        Dim oDataNode As System.Xml.XmlNode
        ' Set up Attributes that will be added to the data model XML
        Dim oXMLElement As XMLElementToAdd
        Dim oXMLAttr As XMLAttributeToAdd

        Dim vElementsToAdd As XMLElementToAdd()

        oDataNode = oXML.SelectSingleNode("TEST_DATA") _
                    .SelectSingleNode("DATA_SET_ELEMENTS")
        If oDataNode Is Nothing Then
            m_oLogFile.WriteLine("<TEST_DATA><DATA_SET_ELEMENTS> not found")
        End If
        ReDim vElementsToAdd(oDataNode.ChildNodes.Count - 1)
        For iElementCnt As Integer = 0 To oDataNode.ChildNodes.Count - 1
            oXMLElement = New XMLElementToAdd
            oXMLElement.ElementName = CheckAttribute(oDataNode.ChildNodes(iElementCnt), "Name", m_oLogFile)
            oXMLElement.ParentNodeName = CheckAttribute(oDataNode.ChildNodes(iElementCnt), "ParentNodeName", m_oLogFile, "")
            ReDim oXMLElement.Attributes(oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1)
            For iAttrCnt As Integer = 0 To oDataNode.ChildNodes(iElementCnt).ChildNodes.Count - 1
                oXMLAttr = New XMLAttributeToAdd
                oXMLAttr.AttributeName = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Name", m_oLogFile)
                oXMLAttr.AttributeValue = CheckAttribute(oDataNode.ChildNodes(iElementCnt).ChildNodes(iAttrCnt), "Value", m_oLogFile)
                oXMLElement.Attributes(iAttrCnt) = oXMLAttr
            Next
            vElementsToAdd(iElementCnt) = oXMLElement
        Next
        Return vElementsToAdd
    End Function

#End Region

#Region " GetXMLDataset"

    Public Shared Function GetXmlDataset(ByVal v_RiskDataXML As String, ByVal v_oTestData As Object) As System.Xml.XmlDocument
        Dim xmlDoc As New System.Xml.XmlDocument
        Dim oElementToAddOrUpdate As System.Xml.XmlNode
        Dim oElementToAddOrUpdateAdded As System.Xml.XmlNode
        Dim newAttribute As System.Xml.XmlAttribute
        Dim nNextOINumber As Integer
        Dim bNewElement As Boolean
        Dim sDataModelCode As String

        ' Read in the XML created in the AddRisk
        xmlDoc.LoadXml(v_RiskDataXML)

        ' Get Data Model
        sDataModelCode = xmlDoc.SelectSingleNode("DATA_SET") _
            .Attributes("DataModelCode").Value.ToUpper

        ' Get the NextOINumber and increment it
        nNextOINumber = xmlDoc.SelectSingleNode("DATA_SET") _
            .Attributes("NextOINumber").Value

        nNextOINumber = nNextOINumber + 1

        ' Create the new test elements
        For iElementCnt As Integer = v_oTestData.XMLDataSetElementsToAdd.GetLowerBound(0) To v_oTestData.XMLDataSetElementsToAdd.GetUpperBound(0)

            If v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ParentNodeName <> "" Then
                ' Check if the element already exists
                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode(sDataModelCode & "_POLICY_BINDER") _
                                    .SelectSingleNode(v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ParentNodeName) _
                                    .SelectSingleNode(v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ElementName)
            Else
                ' Check if the element already exists
                oElementToAddOrUpdate = xmlDoc.SelectSingleNode("DATA_SET") _
                                    .SelectSingleNode("RISK_OBJECTS") _
                                    .SelectSingleNode(sDataModelCode & "_POLICY_BINDER") _
                                    .SelectSingleNode(v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ElementName)
            End If

            ' If not, create it
            If oElementToAddOrUpdate Is Nothing Then
                oElementToAddOrUpdate = xmlDoc.CreateNode(System.Xml.XmlNodeType.Element, v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ElementName, "")

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
                ' Update the Update Status (US) attribute
                oElementToAddOrUpdate.Attributes("US").Value = "2"
                bNewElement = False
            End If

            ' Add specific attributes for this test
            For iAttrCnt As Integer = v_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes.GetLowerBound(0) To v_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes.GetUpperBound(0)
                newAttribute = xmlDoc.CreateAttribute(v_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes(iAttrCnt).AttributeName)
                newAttribute.Value = v_oTestData.XMLDataSetElementsToAdd(iElementCnt).Attributes(iAttrCnt).AttributeValue
                oElementToAddOrUpdate.Attributes.Append(newAttribute)
            Next

            If bNewElement Then
                ' Append the new element node to the XML under the POLICY BINDER
                If v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ParentNodeName <> "" Then
                    oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                        .SelectSingleNode("RISK_OBJECTS") _
                        .SelectSingleNode(sDataModelCode & "_POLICY_BINDER") _
                        .SelectSingleNode(v_oTestData.XMLDataSetElementsToAdd(iElementCnt).ParentNodeName) _
                        .AppendChild(oElementToAddOrUpdate)
                Else
                    oElementToAddOrUpdateAdded = xmlDoc.SelectSingleNode("DATA_SET") _
                        .SelectSingleNode("RISK_OBJECTS") _
                        .SelectSingleNode(sDataModelCode & "_POLICY_BINDER") _
                        .AppendChild(oElementToAddOrUpdate)
                End If
            End If

        Next

        ' Write back the next OI number to the DATA_SET node
        nNextOINumber -= 1
        xmlDoc.SelectSingleNode("DATA_SET") _
            .Attributes("NextOINumber").Value = nNextOINumber
        Return xmlDoc
    End Function

#End Region

#Region " XMLElementToAdd"

    Friend Class XMLElementToAdd

        Private m_sName As String
        Private m_sParentNodeName As String
        Private m_vXMLAttributes As XMLAttributeToAdd()

        Public Property ElementName() As String
            Get
                ElementName = m_sName
            End Get
            Set(ByVal value As String)
                m_sName = value
            End Set
        End Property
        Public Property ParentNodeName() As String
            Get
                ParentNodeName = m_sParentNodeName
            End Get
            Set(ByVal value As String)
                m_sParentNodeName = value
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

#End Region

#Region " XMLAttributeToAdd"

    Friend NotInheritable Class XMLAttributeToAdd

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

#End Region

End Class

