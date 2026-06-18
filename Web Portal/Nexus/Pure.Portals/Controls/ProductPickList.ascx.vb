Imports System.Data

Namespace Nexus

    Partial Class Controls_ProductPickList
        Inherits System.Web.UI.UserControl

        Private _UseSearch As Boolean

        Public Function GetSelectedItems() As ListItemCollection
            Return PckProduct.GetSelectedItems()
        End Function

        Public Sub SetSelectedValues(ByVal Products As NexusProvider.ProductCollection)
            'create a listitemcollection from the list of products passed in 
            'this allows us to compare this to the current items collection of the picklist
            Dim lBranchList As New ListItemCollection
            For i As Integer = 0 To Products.Count - 1
                Dim item As New ListItem
                item.Value = Trim(Products.Item(i).ProductCode)
                item.Text = Trim(Products.Item(i).Description)
                lBranchList.Add(item)
            Next

            'check if the items are already in the list, if not then add them
            'we need to pass an array of the values, so make one at the same time
            Dim sValues As New ArrayList

            For Each item As ListItem In lBranchList
                sValues.Add(item.Value)
                If item IsNot Nothing Then
                    If Not PckProduct.Items.Contains(item) Then
                        'this item is not already present so we need to add it
                        PckProduct.Items.Add(item)
                    End If
                End If
            Next
            'set the selected items of the picklist to the values passed
            PckProduct.SetSelectedValues(sValues.ToArray)
        End Sub

        Public WriteOnly Property UseSearch() As Boolean
            Set(ByVal value As Boolean)
                _UseSearch = value
            End Set
        End Property
        ''' <summary>
        ''' Control the enable or disable of all button
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Public WriteOnly Property Enabled() As Boolean
            Set(ByVal value As Boolean)
                PckProduct.Enabled = value
            End Set
        End Property
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                If _UseSearch Then
                    'it's the first time we load the page, and search is enabled
                    'show search controls
                    olFindProduct.Visible = True
                Else
                    'On Page_Load populating of Branches based on Branches available in BO, only do this if search is disabled
                    FillProducts(String.Empty)
                    'No search so hide the seach box and show picklist
                    olFindProduct.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Fill the picklist with appropriate products
        ''' </summary>
        ''' <param name="sFilter"></param>
        ''' <remarks></remarks>
        Sub FillProducts(ByVal sFilter As String)
            'Initialize the webservice object
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim olist As New NexusProvider.LookupListCollection
            'store the currently seleted items so that we can add them back later
            'we need to do this as clear will clear everythin
            Dim oTempSelectedItems As ListItemCollection = PckProduct.GetSelectedItems
            Dim dtProducts = New DataTable("Products")
            Dim rTempSelectedRows As DataRow()
            Dim iCounterVar As Integer = 0


            'remove any items currently in pick list
            PckProduct.Items.Clear()

            'Retreiving the values from product table into object
            olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "product", False, False)

            'If sFilter= String.Empty, then display all the branches
            'Else making the SQL query with the search criteria
            If Not sFilter = String.Empty Then
                sFilter = "Description like '" & sFilter.Trim & "'"
            End If

            'Adding columns names to datatable
            dtProducts.Columns.Add("Id", GetType(Integer))
            dtProducts.Columns.Add("Code", GetType(String))
            dtProducts.Columns.Add("Description", GetType(String))

            'Setting PrimaryKey for datatable
            dtProducts.Columns("Id").Unique = True
            dtProducts.PrimaryKey = New DataColumn() {dtProducts.Columns("Id")}

            'adding all the branches to datatable, so that we can pull the Branches using SQL Query
            For Each item As NexusProvider.LookupListItem In olist
                iCounterVar = iCounterVar + 1
                dtProducts.Rows.Add( _
                                 New Object() {iCounterVar, item.Code, item.Description})
            Next

            'Use the Select method to find all rows matching the filter.
            rTempSelectedRows = dtProducts.Select(sFilter)

            'check whether results found or not based on user's input
            If rTempSelectedRows.Length = 0 Then
                VldPckProduct.IsValid = False
            End If
            'now we have all the searched rows in searchedRows
            'run the loop for all the selected items in datatable and add all the values to picklist
            If rTempSelectedRows.Length > 0 Then
                For Each rows As DataRow In rTempSelectedRows
                    Dim tempItem As New ListItem
                    tempItem.Value = rows.Item(1).ToString 'Code
                    tempItem.Text = rows.Item(2).ToString '.Description
                    'add an item to the picklist control with the appropriate values
                    PckProduct.Items.Add(tempItem)
                Next
            End If

            'add the selected items back in, and set them to be selected
            'first create an array list to hold the values, we'll use this to set the selected values
            Dim sValues As New ArrayList
            'look through the stored selected items 
            For Each li As ListItem In oTempSelectedItems
                sValues.Add(li.Value)
                If li IsNot Nothing Then
                    If Not PckProduct.Items.Contains(li) Then
                        'this item is not already present so we need to add it
                        PckProduct.Items.Add(li)
                    End If
                End If
            Next

            'set the seleted items as they were before we did the search
            PckProduct.SetSelectedValues(sValues.ToArray)

            'clean up
            oWebService = Nothing
            olist = Nothing
            dtProducts = Nothing
            rTempSelectedRows = Nothing
        End Sub

        Protected Sub btnFindProducts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindProducts.Click
            'call Fillproducts, passing the filter value
            FillProducts(txtProductCode.Text)
            'make the picklist visible to show the results
            PckProduct.Visible = True
        End Sub
    End Class
End Namespace