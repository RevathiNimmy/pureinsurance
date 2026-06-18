Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmMultiAddresses
    Inherits System.Windows.Forms.Form
    Private Sub frmMultiAddresses_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        lvwAddresses.Tag = "False"
        Me.Hide()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        If Not (lvwAddresses.FocusedItem Is Nothing) Then
            lvwAddresses.Tag = "True"
            Me.Hide()
        End If

    End Sub


    Private Sub frmMultiAddresses_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            ' Init list
            lvwAddresses.Columns.Clear()
            lvwAddresses.View = View.Details
            lvwAddresses.LabelEdit = False
            SetExtraListViewProperties(lvwAddresses.Handle.ToInt32(), True)

            ' Create list headers

            lvwAddresses.Columns.Insert(0, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddHouse, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

            lvwAddresses.Columns.Insert(1, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddStreet, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

            lvwAddresses.Columns.Insert(2, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddSuburb, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

            lvwAddresses.Columns.Insert(3, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddCity, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

            lvwAddresses.Columns.Insert(4, CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddPostcode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), 94)

            ' Init captions

            lbl1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddLabel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultAddFormName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        Catch





        End Try


    End Sub
End Class