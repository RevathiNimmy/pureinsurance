Imports Nexus
Imports Nexus.Constants

Partial Class Controls_DocumentList
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim HTable As New Hashtable() 'to hold the document details
        Dim odocument As NexusProvider.DocumentCollection
        Dim oQuote As NexusProvider.Quote = Session(CNQuote)
        Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

        odocument = oWebService.GetDocumentList(oQuote.InsuranceFolderKey)

        Dim odocumentstr As New NexusProvider.Document

        'check if there is any object of document type returned
        If Not odocument Is Nothing Then
            If odocument.Count > 0 Then

                'need to store the unique record into HashTable with the highest DocNum
                Dim icount As Integer = 0
                'run the loop till the count reaches to the total documents present in the policy
                For icount = 0 To odocument.Count - 1
                    If HTable.ContainsKey(odocument.Item(icount).DocDescription) = False Then
                        'if NOT exist, then add the new one into HashTable
                        HTable.Add(odocument.Item(icount).DocDescription, odocument.Item(icount).DocNum)
                    Else
                        'if Exist, then update the data into Hash Table
                        HTable.Item(odocument.Item(icount).DocDescription) = odocument.Item(icount).DocNum
                    End If
                Next

                'displaying the data from the Hash Table 
                Dim HData As DictionaryEntry
                For Each HData In HTable
                    'create list item
                    Dim lstitem As New System.Web.UI.WebControls.ListItem
                    lstitem.Text = HData.Key
                    Session("SelectedDocId") = HData.Value
                    lstitem.Value = "~/secure/document.aspx"
                    lstDocLinks.Items.Add(lstitem)
                Next
            Else
                'no documents, hide control
                Me.Visible = False
            End If
        End If
    End Sub
End Class
