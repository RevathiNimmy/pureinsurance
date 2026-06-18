Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmSelectProducts
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    Private Const ACClass As String = "frmSelectProducts"
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lStatus As Integer

    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try




        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMCancel

        Me.Hide()

        Catch ex As Exception

        ' Error Section
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Close Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        Try


        ' Set the interface status.
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        Me.Hide()



        Catch ex As Exception

        ' Error Section
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call cmdOk_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub


    Private Sub frmSelectProducts_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        SetPickList()

    End Sub

    Private Sub PickListProducts_Find(ByVal Sender As Object, ByVal e As EventArgs) Handles PickListProducts.Find

        For lCount As Integer = PickListProducts.ForeignKeys.Count To 1 Step -1
            PickListProducts.ForeignKeys.Remove(1)
        Next


        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "WhereClause"
        Key.ValueType = gPMConstants.PMEDataType.PMString
        PickListProducts.ForeignKeys.Add(Key, Key:="WhereClause")


        PickListProducts.ForeignKeys.Item("WhereClause").Value = PickListProducts.SearchString

        PickListProducts.PickListType = "AllProducts"
        m_lReturn = PickListProducts.LoadSearched()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to load list of Products", "Scheme Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If

    End Sub

    '''' Reusing the code from BAtch Renewal - Amit WPR12

    Private Function SetPickList() As Integer


        Dim Key As New uctPickList.PickListKey
        Key.KeyName = "WhereClause"
        Key.ValueType = gPMConstants.PMEDataType.PMString
        PickListProducts.ForeignKeys.Add(Key, Key:="WhereClause")

        PickListProducts.PickListType = "AllProducts"
        'Developer Guide No 108
        m_lReturn = PickListProducts.Load_Renamed()
    End Function
End Class
