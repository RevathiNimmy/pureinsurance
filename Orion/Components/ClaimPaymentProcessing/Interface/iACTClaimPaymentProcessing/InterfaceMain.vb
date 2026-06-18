Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Develoer Guide no 129
Imports SharedFiles
Module InterfaceMain

    ' ***************************************************************** '
    ' Module Name: InterfaceMain
    '
    ' Date:
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "iACTClaimPaymentProcessing"

    ' Public source and language ID's from the Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    Private Const ACClass As String = "MainModule"
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()>
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public Const kPartyAccountDetailsAccountId As Integer = 4
    Public Const kAccountShortCodeCLMPAYABLE As String = "CLMPAYABLE"

    Public Const kPayeeOptionCLMPAYABLE As Integer = 1
    Public Const kPayeeOptionOtherParty As Integer = 2
    Public Const kPayeeOptionClient As Integer = 3
    Public Const kPayeeOptionAgent As Integer = 4

    Public Const kClaimPaymentDetailsDocumentId As Integer = 0
    Public Const kClaimPaymentDetailsDocumentRef As Integer = 1
    Public Const kClaimPaymentDetailsCurrencyAmount As Integer = 2
    Public Const kClaimPaymentDetailsCurrencyId As Integer = 3
    Public Const kClaimPaymentDetailsBaseAmount As Integer = 4
    Public Const kClaimPaymentDetailsBaseCurrencyId As Integer = 5
    Public Const kClaimPaymentDetailsAccountAmount As Integer = 6
    Public Const kClaimPaymentDetailsAccountCurrencyId As Integer = 7
    Public Const kClaimPaymentDetailsClaimNumber As Integer = 8
    Public Const kClaimPaymentDetailsDocComment As Integer = 9
    Public Const kClaimPaymentDetailsCurrencyDescription As Integer = 10
    Public Const kClaimPaymentDetailsCurrencyFormatString As Integer = 11
    Public Const kClaimPaymentDetailsClaimPaymentdate As Integer = 12
    Public Const kClaimPaymentDetailsPaymentMediaTypeId As Integer = 13
    Public Const kClaimPaymentDetailsBaseCurrencyDescription As Integer = 14
    Public Const kClaimPaymentDetailsBaseCurrencyFormat As Integer = 15
    Public Const kClaimPaymentDetailsClaimPaymentId As Integer = 16
    Public Const kClaimPaymentDetailsDocumentDate As Integer = 17
    Public Const kClaimPaymentDetailsAccountId As Integer = 18
    Public Const kClaimPaymentDetailsBaseClaimPaymentId As Integer = 19
    Public Const kClaimPaymentDetailsAccountName As Integer = 20
    'WR05
    Public Const kClaimPaymentDetailsStatus As Integer = 21
    Public Const kClaimPaymentDetailsMediaType As Integer = 22
    Public Const kClaimPaymentDetailsBranch As Integer = 23
    Public Const kClaimPaymentDetailsBank As Integer = 24
    Public Const kClaimPaymentMediaTypeID As Integer = 25
    Public Const kClaimPaymentBankAccountID As Integer = 26
    Public Const kClaimPayeeName As Integer = 33
    Public Const kClaimPaymentCashItemLink As Integer = 31
    Public Const kClaimPaymentOurRef As Integer = 32

    Public Const kClaimPaymentTheirRef As Integer = 39
    Public Const kClaimPaymentPayeeSortCode As Integer = 40
    Public Const kClaimPaymentPayeeAccountNo As Integer = 41
    Public Const kClaimPaymentPartyBankId As Integer = 42
    Public Const kClaimPaymentIsReversed As Integer = 43
    '61600
    Public Const kClaimPaymentSourceID As Integer = 29
    '61600


    ''WR05

    Public Const kColumnHeaderIndexDocumentId As Integer = 1
    Public Const kColumnHeaderIndexAccountName As Integer = 2
    Public Const kColumnHeaderIndexDocumentReference As Integer = 3
    Public Const kColumnHeaderIndexClaimReference As Integer = 4
    Public Const kColumnHeaderIndexClaimPaymentDate As Integer = 5
    Public Const kColumnHeaderIndexClaimPaymentAmount As Integer = 6
    Public Const kColumnHeaderIndexClaimPaymentCurrency As Integer = 7
    Public Const kColumnHeaderIndexClaimBaseAmount As Integer = 8
    Public Const kColumnHeaderIndexClaimBaseCurrency As Integer = 9
    Public Const kColumnHeaderIndexClaimPaymentMediaTypeId As Integer = 10
    Public Const kColumnHeaderIndexClaimPaymentCurrencyId As Integer = 11
    'WR05
    Public Const kColumnHeaderIndexClaimPaymentStatus As Integer = 12
    Public Const kColumnHeaderIndexClaimPaymentMediaType As Integer = 13
    Public Const kColumnHeaderIndexClaimPaymentBranch As Integer = 14
    Public Const kColumnHeaderIndexClaimPaymentBank As Integer = 15

    ''WR05


    Public Const kColumnHeaderKeyAccountName As String = "account_name"
    Public Const kColumnHeaderKeyDocumentId As String = "document_id"
    Public Const kColumnHeaderKeyDocumentReference As String = "document_ref"
    Public Const kColumnHeaderKeyClaimReference As String = "claim_number"
    Public Const kColumnHeaderKeyClaimPaymentDate As String = "payment_date"
    Public Const kColumnHeaderKeyClaimPaymentAmount As String = "currency_amount"
    Public Const kColumnHeaderKeyClaimPaymentCurrency As String = "currency_description"
    Public Const kColumnHeaderKeyClaimBaseAmount As String = "base_currency_amount"
    Public Const kColumnHeaderKeyClaimBaseCurrency As String = "base_currency_description"
    Public Const kColumnHeaderKeyClaimPaymentMediaTypeId As String = "media_type_id"
    Public Const kColumnHeaderKeyClaimPaymentCurrencyId As String = "currency_id"
    'WR05
    Public Const kColumnHeaderKeyClaimPaymentStatus As String = "Status"
    Public Const kColumnHeaderKeyClaimPaymentMediaType As String = "Media Type"
    Public Const kColumnHeaderKeyClaimPaymentBranch As String = "Branch"
    Public Const kColumnHeaderKeyClaimPaymentBank As String = "Bank"
    ''WR05
    Public Const kSubItemAccountName As Integer = 1
    Public Const kSubItemDocumentReference As Integer = 2
    Public Const kSubItemClaimReference As Integer = 3
    Public Const kSubItemClaimPaymentDate As Integer = 4
    Public Const kSubItemClaimPaymentAmount As Integer = 5
    Public Const kSubItemClaimPaymentCurrency As Integer = 6
    Public Const kSubItemClaimBaseAmount As Integer = 7
    Public Const kSubItemClaimBaseCurrency As Integer = 8
    Public Const kSubItemClaimPaymentMediaTypeId As Integer = 9
    Public Const kSubItemClaimPaymentCurrencyId As Integer = 10
    'WR05
    Public Const kSubItemClaimPaymentStatus As Integer = 11
    Public Const kSubItemClaimPaymentMediaType As Integer = 12
    Public Const kSubItemClaimPaymentBranch As Integer = 13
    Public Const kSubItemClaimPaymentBank As Integer = 14

    Public Enum ACListPaymentSummary
        PSMediaType = 0
        PSPaymentCount = 1
        PSPaymentValue = 2
    End Enum


    Public Sub Main_Renamed()

    End Sub
    Public Function OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "OnColumnClick"
        Try

            result = True
            Dim lColumnHeaderIndex, lReturn As Integer
            lColumnHeaderIndex = ColumnHeader.Index + 1 - 1

            With ListView

                Select Case ColumnHeader.Index + 1
                    Case kColumnHeaderIndexDocumentId, kColumnHeaderIndexClaimPaymentMediaTypeId, kColumnHeaderIndexClaimPaymentCurrencyId
                        ListViewHelper.SetSortedProperty(ListView, False)
                        ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)

                        ListViewFunc.ListViewSortByValue(ListView, lColumnHeaderIndex, ListViewHelper.GetSortOrderProperty(ListView))

                    Case kColumnHeaderIndexClaimPaymentDate

                        lReturn = ListViewFunc.ListViewSortByDate(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)

                    Case kColumnHeaderIndexClaimPaymentAmount, kColumnHeaderIndexClaimBaseAmount

                        lReturn = ListViewSortByCurrency(v_oListView:=ListView, v_iSourceColumn:=lColumnHeaderIndex, v_iDirection:=(ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                    Case Else
                        ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
                End Select

            End With


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function

    Public Function ListViewSortByCurrency(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder, Optional ByVal v_bMarkSortedColumn As Boolean = False) As Integer


        Dim result As Integer = 0
        Dim lSortColumn As Integer
        Dim sValue As String = ""
        Dim dValue As Double
        Dim sStr() As String

        Const kMethodName As String = "ListViewSortByCurrency"
        Const ACLVTag As String = "SORT_VALUE_HIDDEN"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add a dummy sort column and get the index of this new column
            ' -1 because it's a sub item
            v_oListView.Columns.Add(ACLVTag, ACLVTag, CInt(VB6.TwipsToPixelsX(0)))
            lSortColumn = v_oListView.Columns.Count - 1

            ' Not sorted yet
            ListViewHelper.SetSortedProperty(v_oListView, False)

            ' Add the items
            For Each oListItem As ListViewItem In v_oListView.Items

                If v_iSourceColumn Then
                    sValue = ListViewHelper.GetListViewSubItem(oListItem, v_iSourceColumn).Text
                Else
                    sValue = oListItem.Text
                End If

                Dim dbNumericTemp As Double
                If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    dValue = CDbl(sValue) + 100000000000.0#
                Else

                    sStr = sValue.Split(" "c)
                    sValue = sStr(0).Replace(",", "")
                    dValue = Conversion.Val(sValue) + 100000000000.0#

                End If

                ' Extend our value as the list view will only sort as strings
                ' Extended further to cope with currency (14.) and doubles (.12 for now)
                ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = StringsHelper.Format(dValue, "000000000000.0000")
            Next oListItem

            ' Set sort column and direction and sort
            ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
            ListViewHelper.SetSortKeyProperty(v_oListView, lSortColumn)
            ListViewHelper.SetSortedProperty(v_oListView, True)

            ' Remove the column now
            v_oListView.Columns.RemoveAt(lSortColumn)

            ' Set to original for asc/desc analysis?
            If v_bMarkSortedColumn Then
                ' Note: We must remove the sorted flag first of this will botch everything!
                ListViewHelper.SetSortedProperty(v_oListView, False)
                ListViewHelper.SetSortKeyProperty(v_oListView, v_iSourceColumn)
            End If


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function
End Module
