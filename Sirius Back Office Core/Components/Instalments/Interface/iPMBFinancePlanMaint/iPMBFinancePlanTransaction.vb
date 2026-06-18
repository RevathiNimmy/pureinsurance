Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
'developer guide no. 129 (guide)
Partial Friend Class frmTransactions
	Inherits System.Windows.Forms.Form

    Private m_vTransactionData(,) As Object

    Private Const ACFindImage As String = "FindImage"

    Public Property TransactionData() As Object
        Get
            Return VB6.CopyArray(m_vTransactionData)
        End Get
        Set(ByVal Value As Object)
            m_vTransactionData = Value
        End Set
    End Property

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Me.Hide()
    End Sub

    Private Sub frmTransactions_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim cOSCurrencyAmount As Decimal
        Dim lCurrencyId As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim nLower, nUpper As Integer
        Dim oListItem As ListViewItem
        Dim sFormattedCurrency As String = String.Empty
        Dim sFormattedOSCurrency As String = String.Empty
        Dim sLookupDesc As String = String.Empty
        Dim vConversionDate As Object = Nothing

        ' Clear the listview
        lvwTransactions.Items.Clear()

        ' Check that search details are valid before continuing.
        If Information.IsArray(m_vTransactionData) Then

            lReturn = CType(ListViewFunc.ListViewBatchStart(lvwList:=lvwTransactions), gPMConstants.PMEReturnCode)

            nLower = m_vTransactionData.GetLowerBound(1)
            nUpper = m_vTransactionData.GetUpperBound(1)

            ' Assign the details to the interface.
            For nCount As Integer = nLower To nUpper

                'Changes as per Vb code
                'oListItem = lvwTransactions.Items.Add(CStr(m_vTransactionData(ACISourceID, nCount)).Trim(), "")
                oListItem = lvwTransactions.Items.Add(CStr(m_vTransactionData(ACISourceID, nCount)).Trim(), ACFindImage)
                ' Get the currencyID
                lCurrencyId = CInt(m_vTransactionData(ACICurrencyID, nCount))

                ' Assign details to other the columns
                ListViewHelper.GetListViewSubItem(oListItem, ACListAccountShortCode).Text = CStr(m_vTransactionData(ACIAccountShortCode, nCount)).Trim()

                ListViewHelper.GetListViewSubItem(oListItem, ACListAccountingDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vTransactionData(ACIAccountingDate, nCount)))

                ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentDate).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateShort, vFieldValue:=CStr(m_vTransactionData(ACIDocumentDate, nCount)))

                ListViewHelper.GetListViewSubItem(oListItem, ACListPeriodName).Text = CStr(m_vTransactionData(ACIPeriodName, nCount)).Trim()

                'eck260301 added conversion date paramater to keep in like with CF changes

                lReturn = g_oFindTransaction.FormatCurrency(vCurrencyID:=m_vTransactionData(ACIAmountCurrencyID, nCount), vCurrencyAmount:=m_vTransactionData(ACIBaseAmount, nCount), vFormattedCurrency:=sFormattedCurrency, vConversionDate:=vConversionDate)

                ListViewHelper.GetListViewSubItem(oListItem, ACListCurrencyAmount).Text = sFormattedCurrency.Trim()

                If Not CBool(m_vTransactionData(ACIPrimarySettled, nCount)) Then
                    ListViewHelper.GetListViewSubItem(oListItem, ACListPrimarySettled).Text = "N"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, ACListPrimarySettled).Text = "Y"
                End If

                cOSCurrencyAmount = CDec(m_vTransactionData(ACIOutstandingBaseAmount, nCount))

                lReturn = g_oFindTransaction.FormatCurrency(vCurrencyID:=m_vTransactionData(ACIAmountCurrencyID, nCount), vCurrencyAmount:=cOSCurrencyAmount, vFormattedCurrency:=sFormattedOSCurrency, vConversionDate:=vConversionDate)

                ListViewHelper.GetListViewSubItem(oListItem, ACListOSCurrencyAmount).Text = sFormattedOSCurrency.Trim()

                lReturn = CType(GetLookupDesc(lLookupRow:=ACLDocumentType, lLookupID:=CInt(m_vTransactionData(ACIDocumentTypeId, nCount)), sLookupDesc:=sLookupDesc), gPMConstants.PMEReturnCode)

                ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentTypeId).Text = sLookupDesc
                ListViewHelper.GetListViewSubItem(oListItem, ACListInsuranceRef).Text = CStr(m_vTransactionData(ACIInsuranceRef, nCount)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, ACListOperatorName).Text = CStr(m_vTransactionData(ACIOperatorName, nCount)).Trim()

                ' Document Type Group
                lReturn = CType(GetLookupDesc(lLookupRow:=ACLDocTypeGroup, lLookupID:=CInt(m_vTransactionData(ACIDocTypeGroupId, nCount)), sLookupDesc:=sLookupDesc), gPMConstants.PMEReturnCode)

                ListViewHelper.GetListViewSubItem(oListItem, ACListDocTypeGroupId).Text = sLookupDesc
                ListViewHelper.GetListViewSubItem(oListItem, ACListDocTypeGroupId).Text = sLookupDesc
                ListViewHelper.GetListViewSubItem(oListItem, ACListDocumentRef).Text = CStr(m_vTransactionData(ACIDocumentRef, nCount)).Trim()

                ' Set the tag property with the index of the search data storage.
                oListItem.Tag = CStr(nCount)
            Next nCount
        End If

        ' Size the columns
        If lvwTransactions.Items.Count > 0 Then
            ListView6Autosize(lvwList:=lvwTransactions, bSizeHeaders:=False)

            ' Activate the list view
            lReturn = CType(ListViewFunc.ListViewBatchEnd(), gPMConstants.PMEReturnCode)
        End If

        iPMFunc.CenterForm(Me)
    End Sub
End Class