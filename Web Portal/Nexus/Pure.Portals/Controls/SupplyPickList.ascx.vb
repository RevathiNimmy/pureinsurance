Imports System.Data

Namespace Nexus

    Partial Class Controls_SupplyPickList
        Inherits System.Web.UI.UserControl

        Private _UseSearch As Boolean

        ''' <summary>
        ''' returns the items which are in the right hand, i.e. selected, list
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSelectedItems() As ListItemCollection
            Return PckSupply.GetSelectedItems()
        End Function

        ''' <summary>
        ''' sets the values which are in the right hand, i.e. selected, list
        ''' </summary>
        ''' <param name="Supplies"></param>
        ''' <remarks></remarks>
        Public Sub SetSelectedValues(ByVal Supplies As NexusProvider.PickListCollection)
            'create a listitemcollection from the list of Supplies passed in 
            'this allows us to compare this to the current items collection of the picklist
            Dim listSupply As New ListItemCollection
            For i As Integer = 0 To Supplies.Count - 1
                Dim item As New ListItem
                item.Value = Trim(Supplies.Item(i).Code)
                listSupply.Add(item)
            Next

            'check if the items are alerady in the list, if not then add them
            'we need to pass an array of the values, so make one at the same time
            Dim sValues As New ArrayList

            For Each item As ListItem In listSupply
                sValues.Add(item.Value)
                If item IsNot Nothing Then
                    If Not PckSupply.Items.Contains(item) Then
                        'this item is not already present so we need to add it
                        'PckSupply.Items.Add(item)
                    End If
                End If
            Next
            'set the selected items of the picklist to the values passed
            PckSupply.SetSelectedValues(sValues.ToArray)
        End Sub

        ''' <summary>
        ''' Controls the visibility of the search textbox, and therefore whether search is enabled
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
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
                PckSupply.Enabled = value
            End Set
        End Property
        ''' <summary>
        ''' Set visibility of controls and populate items depending on whether search is enabled or not
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If Not IsPostBack Then
                If _UseSearch Then
                    'it's the first time we load the page, and search is enabled
                    'show search controls
                    olFindSupply.Visible = True
                Else
                    'On Page_Load populating of Supplies based on Supplies available in BO, only do this if search is disabled
                    FillSupplies(String.Empty)
                    'No search so hide the seach box and show picklist
                    olFindSupply.Visible = False
                End If
            End If
        End Sub

        ''' <summary>
        ''' Fills the picklist with Supplies
        ''' </summary>
        ''' <param name="sFilter">String to filter the Supplies by supply code. If null then no filter is applied</param>
        ''' <remarks></remarks>
        Sub FillSupplies(ByVal sFilter As String)
            'Initialize the webservice object
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim olist As New NexusProvider.LookupListCollection 'olist will hold the list items returned by SAM
            'store the currently seleted items so that we can add them back later
            'we need to do this as clear will clear everythin
            Dim oTempSelectedItems As ListItemCollection = PckSupply.GetSelectedItems
            Dim dtSupplies = New DataTable("Supplies")
            Dim rTempSelectedRows As DataRow()
            Dim iCounterVar As Integer = 0

            'remove any items currently in pick list
            PckSupply.Items.Clear()

            'Retreiving the values from supply table into object
            olist = oWebService.GetList(NexusProvider.ListType.PMLookup, "Supplier_Business", False, False)

            'If sFilter= String.Empty, then display all the Supplies
            'Else making the SQL query with the search criteria
            If Not sFilter = String.Empty Then
                sFilter = "Description like '" & sFilter.Trim & "'"
            End If

            'Adding columns names to datatable
            dtSupplies.Columns.Add("Id", GetType(Integer))
            dtSupplies.Columns.Add("Code", GetType(String))
            dtSupplies.Columns.Add("Description", GetType(String))

            'Setting PrimaryKey for datatable
            dtSupplies.Columns("Id").Unique = True
            dtSupplies.PrimaryKey = New DataColumn() {dtSupplies.Columns("Id")}

            'adding all the Supplies to datatable, so that we can pull the Supplies using SQL Query
            For Each item As NexusProvider.LookupListItem In olist
                iCounterVar = iCounterVar + 1
                dtSupplies.Rows.Add(
                                 New Object() {iCounterVar, item.Code, item.Description})
            Next

            'Use the Select method to find all rows matching the filter.
            rTempSelectedRows = dtSupplies.Select(sFilter)

            'check whether results found or not based on user's input
            If rTempSelectedRows.Length = 0 Then
                VldPckSupply.IsValid = False
            End If
            'now we have all the searched rows in searchedRows
            'run the loop for all the selected items in datatable and add all the values to picklist
            If rTempSelectedRows.Length > 0 Then
                For Each rows As DataRow In rTempSelectedRows
                    Dim tempItem As New ListItem
                    tempItem.Value = rows.Item(1).ToString 'Code
                    tempItem.Text = rows.Item(2).ToString '.Description
                    'add an item to the picklist control with the appropriate values
                    PckSupply.Items.Add(tempItem)
                Next
            End If

            'add the selected items back in, and set them to be selected
            'first create an array list to hold the values, we'll use this to set the selected values
            Dim sValues As New ArrayList
            'look through the stored selected items 
            For Each li As ListItem In oTempSelectedItems
                sValues.Add(li.Value)
                If li IsNot Nothing Then
                    If Not PckSupply.Items.Contains(li) Then
                        'this item is not already present so we need to add it
                        PckSupply.Items.Add(li)
                    End If
                End If
            Next

            'set the seleted items as they were before we did the search
            PckSupply.SetSelectedValues(sValues.ToArray)

            'clean up
            oWebService = Nothing
            olist = Nothing
            dtSupplies = Nothing
            rTempSelectedRows = Nothing
        End Sub

        Protected Sub btnFindSupplies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFindSupplies.Click
            'call FillSupplies, passing the filter value
            FillSupplies(txtSupplyCode.Text)
            'make the picklist visible to show the results
            PckSupply.Visible = True
        End Sub
    End Class
End Namespace