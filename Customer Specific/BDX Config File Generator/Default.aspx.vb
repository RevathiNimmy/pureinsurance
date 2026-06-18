Imports System.Data.SqlClient
Imports System.Data
Imports System.Configuration
Imports System.Xml
Imports System.IO

Partial Class _Default
    Inherits System.Web.UI.Page
    Dim conn As SqlConnection
    Dim dbhelper As DbHelper
    Dim dtScreenDetails As DataTable
    Dim oPropertyDetails(,) As String

    Public lXMLData As List(Of Object) = New List(Of Object)
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            dbhelper = New DbHelper
            'adding Data Model Type
            ddlProducts.DataSource = dbhelper.GetProductCode()
            ddlProducts.DataTextField = "Productcode"
            ddlProducts.DataValueField = "product_id"
            ddlProducts.DataBind()

            'ddlProducts.DataSource = dbhelper.GetGISDataModelCode("1")
            ddlRisk.DataSource = dbhelper.GetRiskCode(ddlProducts.SelectedValue)
            ddlRisk.DataTextField = "Riskcode"
            ddlRisk.DataValueField = "risktypeid"
            ddlRisk.DataBind()

        End If

    End Sub

    Protected Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        lblPath.Text = ""
        btnSubmit.Enabled = False
        chkOnlyMap.Enabled = False
        lblCountFields.Visible = False
        txtColumnName.Visible = False
        lblExcelMap.Visible = False
        btnMapping.Visible = False
        lblWarning.Visible = False
        lblExcelMaprow.Visible = False
        txtRowNo.Visible = False
        dtScreenDetails = New DataTable
        dbhelper = New DbHelper
        dtScreenDetails = dbhelper.GetScreenDetails(ddlRisk.SelectedValue)
        If dtScreenDetails.Rows.Count > 0 Then
            oPropertyDetails = dbhelper.GetPropertyDetailsInObject(dtScreenDetails)
            lXMLData = dbhelper.GenerateMappingXML(oPropertyDetails)
        End If


        Dim path As String = "C:\XMLGenerator\" + Trim(ddlProducts.SelectedItem.Text) + "_Risk_" + Trim(ddlRisk.SelectedItem.Text) + ".xml"
        ' Create or overwrite the file.
        Dim fs As FileStream = File.Create(path)

        For count1 As Integer = 0 To lXMLData.Count - 1
            ' Add text to the file.
            Dim info As Byte() = New UTF8Encoding(True).GetBytes(lXMLData(count1).ToString() + vbNewLine)
            fs.Write(info, 0, info.Length)
        Next
        lblPath.Text = " File created " + path
        lblPath.ForeColor = Drawing.Color.Green
        lblCountFields.Text = "There are " + (lXMLData.Count - 1).ToString + " needs to be mapped "
        lblCountFields.Visible = True
        fs.Close()
        'txtColumnName.Visible = True
        'lblExcelMap.Visible = True
        'btnMapping.Visible = True
        'lblExcelMaprow.Visible = True
        'txtRowNo.Visible = True
        'lblWarning.Text = path
        btnSubmit.Enabled = True
        chkOnlyMap.Enabled = True
    End Sub

    Protected Sub btnMapping_Click(sender As Object, e As EventArgs) Handles btnMapping.Click
        If FileUpexcel.FileName <> "" AndAlso txtColumnName.Text <> "" AndAlso txtRowNo.Text <> "" AndAlso xmlfileupload.FileName <> "" Then

            Dim path As String = lblWarning.Text
            If path.Trim = "" Then
                path = Convert.ToString(xmlfileupload.PostedFile.FileName)
            End If
            lblWarning.Text = ""
            lblWarning.Visible = False
            Dim lReturn As Integer
            dbhelper = New DbHelper
            dbhelper.ExcelSheet_AlphabetArray_Generation()
            Dim excelpath As String = Convert.ToString(FileUpexcel.PostedFile.FileName)
            lReturn = dbhelper.ExcelSheet_MappedFields(path:=path, startColumn:=txtColumnName.Text, mappedRowNo:=Integer.Parse(txtRowNo.Text), excelpath:=excelpath, sheetName:="Sheet1")
            If lReturn = 0 Then
                lblWarning.Text = "Invalid Column Name:"
                lblWarning.Visible = True
                lblWarning.ForeColor = Drawing.Color.Red
            Else
                lblWarning.Text = "Success - Please Check C:\XMLGrnerator\"
                lblWarning.Visible = True
                lblWarning.ForeColor = Drawing.Color.Green
            End If
        Else
            lblWarning.Text = "Invalid File Name:"
            lblWarning.Visible = True
            lblWarning.ForeColor = Drawing.Color.Red
        End If
    End Sub

    Protected Sub chkOnlyMap_CheckedChanged(sender As Object, e As EventArgs) Handles chkOnlyMap.CheckedChanged
        If chkOnlyMap.Checked Then
            lblSelect.Visible = False
            lblSelectScreen.Visible = False
            ddlProducts.Visible = False
            ddlRisk.Visible = False
            btnSubmit.Visible = False
            lblfileupload.Visible = True
            FileUpexcel.Visible = True

            lblCountFields.Visible = True
            txtColumnName.Visible = True
            lblExcelMap.Visible = True
            btnMapping.Visible = True
            lblExcelMaprow.Visible = True
            txtRowNo.Visible = True
            lblxmlfile.Visible = True
            xmlfileupload.Visible = True
        Else
            lblfileupload.Visible = False
            FileUpexcel.Visible = False
            lblSelect.Visible = True
            lblSelectScreen.Visible = True
            ddlProducts.Visible = True
            ddlRisk.Visible = True
            btnSubmit.Visible = True
            lblxmlfile.Visible = False
            xmlfileupload.Visible = False

            lblCountFields.Visible = False
            txtColumnName.Visible = False
            lblExcelMap.Visible = False
            btnMapping.Visible = False
            lblWarning.Visible = False
            lblExcelMaprow.Visible = False
            txtRowNo.Visible = False
        End If
    End Sub
End Class
