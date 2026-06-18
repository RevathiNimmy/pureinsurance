
Partial Class PaymentCashList_CashListItemsfirst
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim Table1 As Data.DataTable
        Table1 = New Data.DataTable("allocations")

        Dim Row1 As Data.DataRow

        Dim DocumentRef As Data.DataColumn = New Data.DataColumn("MediaReference")
        DocumentRef.DataType = System.Type.GetType("System.String")
        Table1.Columns.Add(DocumentRef)
        Dim Type As Data.DataColumn = New Data.DataColumn("MediaType")
        Type.DataType = System.Type.GetType("System.String")
        Table1.Columns.Add(Type)
        Dim TaxBand As Data.DataColumn = New Data.DataColumn("Amount")
        TaxBand.DataType = System.Type.GetType("System.String")
        Table1.Columns.Add(TaxBand)
        Dim Currency As Data.DataColumn = New Data.DataColumn("AccountShortCode")
        Currency.DataType = System.Type.GetType("System.String")
        Table1.Columns.Add(Currency)
        Dim OSmount As Data.DataColumn = New Data.DataColumn("Status")
        OSmount.DataType = System.Type.GetType("System.String")
        Table1.Columns.Add(OSmount)
        Dim Allocated As Data.DataColumn = New Data.DataColumn("Letter")
        Allocated.DataType = System.Type.GetType("System.String")
        Table1.Columns.Add(Allocated)
     

        Row1 = Table1.NewRow()

        Row1.Item("MediaReference") = ""
        Row1.Item("MediaType") = ""
        Row1.Item("Amount") = ""
        Row1.Item("AccountShortCode") = ""
        Row1.Item("Status") = ""
        Row1.Item("Letter") = ""

        Table1.Rows.Add(Row1)
        gvResult.DataSource = Table1
        gvResult.DataBind()

    End Sub
    Protected Sub btnadd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnadd.Click
        Response.Redirect("CreateCashListItemReceipt.aspx")
    End Sub
End Class
