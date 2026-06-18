Friend Class TestDataHelper

    Public ListOfAddressesWithContacts As New List(Of ProxyWS.BaseAddressWithContactsType)
    Public ListOfContacts As New List(Of ProxyWS.BaseContactType)

    Public Function LoadAddressWithContactsType(ByVal oAddressNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseAddressWithContactsType

        Dim ListOfContacts As New List(Of ProxyWS.BaseContactType)
        Dim oAddress As New ProxyWS.BaseAddressWithContactsType

        For Each oXMLNode As XmlNode In oAddressNode.ChildNodes

            If oXMLNode.Name = "CONTACT" Then
                ListOfContacts.Add(LoadContact(oXMLNode, oTestData))
            End If

        Next

        oAddress.AddressLine1 = oTestData.CheckAttribute(oAddressNode, "Line1")
        oAddress.AddressLine2 = oTestData.CheckAttribute(oAddressNode, "Line2")
        oAddress.AddressLine3 = oTestData.CheckAttribute(oAddressNode, "Line3")
        oAddress.AddressLine4 = oTestData.CheckAttribute(oAddressNode, "Line4")

        oAddress.AddressTypeCode = CType(System.Enum.Parse(GetType(ProxyWS.AddressTypeType), oTestData.CheckAttribute(oAddressNode, "TypeEnum")), ProxyWS.AddressTypeType)
        oAddress.CountryCode = oTestData.CheckAttribute(oAddressNode, "CountryCode")
        oAddress.PostCode = oTestData.CheckAttribute(oAddressNode, "PostCode")

        oAddress.Contacts = ListOfContacts.ToArray

        Return oAddress

    End Function

    Public Function LoadAddress(ByVal oAddressNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseAddressType

        Dim oAddress As New ProxyWS.BaseAddressType

        oAddress.AddressLine1 = oTestData.CheckAttribute(oAddressNode, "Line1")
        oAddress.AddressLine2 = oTestData.CheckAttribute(oAddressNode, "Line2")
        oAddress.AddressLine3 = oTestData.CheckAttribute(oAddressNode, "Line3")
        oAddress.AddressLine4 = oTestData.CheckAttribute(oAddressNode, "Line4")
        oAddress.AddressTypeCode = CType(System.Enum.Parse(GetType(ProxyWS.AddressTypeType), oTestData.CheckAttribute(oAddressNode, "TypeEnum")), ProxyWS.AddressTypeType)
        oAddress.CountryCode = oTestData.CheckAttribute(oAddressNode, "CountryCode")
        oAddress.PostCode = oTestData.CheckAttribute(oAddressNode, "PostCode")

        Return oAddress

    End Function

    Public Function LoadContact(ByVal oContactNode As XmlNode, ByVal oTestData As TestData) As ProxyWS.BaseContactType

        Dim oContact As New ProxyWS.BaseContactType
        Dim oContactDetail As New ProxyWS.BaseContactDetailType

        oContact.AreaCode = oTestData.CheckAttribute(oContactNode, "AreaCode")
        oContact.ContactTypeCode = CType(System.Enum.Parse(GetType(ProxyWS.ContactTypeType), oTestData.CheckAttribute(oContactNode, "ContactTypeEnum")), ProxyWS.ContactTypeType)

        oContactDetail.Item = oTestData.CheckAttribute(oContactNode, "ContactDetail")
        oContactDetail.ItemElementName = CType(System.Enum.Parse(GetType(ProxyWS.ItemChoiceType), oTestData.CheckAttribute(oContactNode, "ContactDetailTypeEnum")), ProxyWS.ItemChoiceType)

        oContact.ContactDetail = oContactDetail

        Return oContact

    End Function

End Class
