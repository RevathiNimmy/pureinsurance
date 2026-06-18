Friend Class UpdatePartyOtherTestData
    Inherits TestDataHelper

    
    Public Sub Load(ByVal oClientDataImportNode As XmlNode, ByVal oTestData As TestData)

        Try

            For Each oXMLNode As XmlNode In oClientDataImportNode.ChildNodes

                Select Case oXMLNode.Name

                    Case "ADDRESS"
                        MyBase.ListOfAddressesWithContacts.Add(LoadAddressWithContactsType(oXMLNode, oTestData))

                    Case "CONTACT"
                        MyBase.ListOfContacts.Add(LoadContact(oXMLNode, oTestData))

                End Select

            Next

        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
