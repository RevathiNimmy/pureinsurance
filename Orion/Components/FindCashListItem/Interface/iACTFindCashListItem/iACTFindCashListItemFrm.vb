Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 1st September 1997
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    'sw added payment maintenance
    Private m_lCashListTypeID As Integer
    Private m_sActionkey As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lCashListItemId As Integer
    Private m_lCashListID As Integer
    Private m_lCashlistItemMode As Integer
    Private m_lBatchID As Integer
    Private m_sBatchReference As String = ""
    'developer Guide No.7
    Private Const vbFormCode As Integer = 0
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iACTFindCashListItem.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the search data from the business object.
    Public m_vSearchData(,) As Object

    Private m_vSourceArray As Object
    ' PRIVATE Data Members (End)
    Private m_lClaimMode As Integer ' MEvans : 23-06-2004 : CQ4740

    '************
    ' MEvans : 23-06-2004 : CQ4740
    Public WriteOnly Property ClaimMode() As Integer
        Set(ByVal Value As Integer)
            m_lClaimMode = Value
        End Set
    End Property
    '************

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property



    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

        End Set
    End Property

    Public ReadOnly Property NavigatorTitle() As String
        Get

            ' Return the objects parameter value.
            Return m_sNavigatorTitle

        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public WriteOnly Property SourceArray() As Object()
        Set(ByVal Value As Object())

            ' Set the valid sources for the user
            m_vSourceArray = Value

        End Set
    End Property



    'sw added this pair of property procedures for Payment Maintenance 04-11-2002
    Public Property CashlistTypeID() As Integer
        Get

            Return m_lCashListTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lCashListTypeID = Value

        End Set
    End Property
    Public Property BatchReference() As String
        Get
            Return m_sBatchReference
        End Get
        Set(ByVal Value As String)
            m_sBatchReference = Value
        End Set
    End Property
    Public Property BatchID() As Integer
        Get
            Return m_lBatchID
        End Get
        Set(ByVal Value As Integer)
            m_lBatchID = Value
        End Set
    End Property

    'sw added payment maintenance 11-11-2002

    Public Property ActionKey() As String
        Get
            Return m_sActionkey
        End Get
        Set(ByVal Value As String)
            m_sActionkey = Value
        End Set
    End Property


    'sw added payment maintenance / Front Office Receipting, to make component navigator aware


    Public Property CashListID() As Integer
        Get

            Return m_lCashListID

        End Get
        Set(ByVal Value As Integer)

            m_lCashListID = Value

        End Set
    End Property

    'sw added payment maintenance / Front Office Receipting, to make component navigator aware


    Public Property CashListItemID() As Integer
        Get

            Return m_lCashListItemId

        End Get
        Set(ByVal Value As Integer)

            m_lCashListItemId = Value

        End Set
    End Property


    Public Property CashListItemMode() As Integer
        Get

            Return m_lCashlistItemMode

        End Get
        Set(ByVal Value As Integer)

            m_lCashlistItemMode = Value

        End Set
    End Property
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        'Variants required to handle blanks which will be converted to NULLs
        Dim result As Integer = 0
        Dim vStartDate As String = ""
        Dim vEndDate As String = ""
        Dim vReceiptTypeId As Integer
        Dim vAccount As String = ""
        Dim vMediaReference As String = ""
        Dim vTheirReference As String = ""
        Dim vAmount As String = "" 'used for payments and receipts
        Dim vBatchReference As String = "" 'used for payments and receipts
        Dim vMediaTypeId As Integer
        Dim vReceiptNumber As String = ""

        'variants for payments (sw payment maintenance 05-11-2002)
        Dim vPayeeName As String = ""
        Dim vPaymentTypeID As Integer
        Dim vPaymentMediaTypeID As Integer
        Dim vChequeEFTNo As String = ""
        Dim vPaymentStatusID As Integer
        Dim vAccountID As Integer
        Dim vBatchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' Display a searching message.
            DisplayStatusSearching()

            ' Disable parts of the interface while
            ' a search is in progress.
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}


            'if clause added, sw Payment maintenance 05-11-2002

            If m_lCashListTypeID = ACReceiptingType Then

                ' Check if document (from) date has been entered.
                If txtDateFrom.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vStartDate = Nothing
                Else
                    vStartDate = txtDateFrom.Text.Trim()
                End If

                ' Check if document (to) date has been entered.
                If txtDateTo.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vEndDate = Nothing
                Else
                    vEndDate = txtDateTo.Text.Trim()
                End If

                ' Check if User Group has been entered.
                If uctPMLookupReceiptType.ItemId = 0 Then

                    vReceiptTypeId = Nothing
                Else
                    vReceiptTypeId = uctPMLookupReceiptType.ItemId
                End If

                ' Check if Account has been entered.
                If uctAccountLookup.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vAccount = Nothing
                Else
                    vAccount = uctAccountLookup.Text.Trim()
                End If

                ' Check if Media Reference has been entered.
                If txtMediaReference.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vMediaReference = Nothing
                Else
                    vMediaReference = txtMediaReference.Text.Trim()
                End If

                ' Check if Their Reference has been entered.
                If txtTheirReference.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vTheirReference = Nothing
                Else
                    vTheirReference = txtTheirReference.Text.Trim()
                End If

                ' Check if Batch Number has been entered.
                If txtPaymentBatchReference.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vBatchReference = Nothing
                Else
                    vBatchReference = txtPaymentBatchReference.Text.Trim()
                End If

                ' Check if Media Type has been entered.
                If cboPMLookupMediaType.ItemId = 0 Then

                    vMediaTypeId = Nothing
                Else
                    vMediaTypeId = cboPMLookupMediaType.ItemId
                End If

                ' Check if Receipt Number has been entered.
                If txtReceiptNumber.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vReceiptNumber = Nothing
                Else
                    vReceiptNumber = txtReceiptNumber.Text.Trim()
                End If

                'sw added in this if clause, add amount as param for search query
                If txtAmount.Text.Trim().Length = 0 Then

                    vAmount = Nothing
                Else
                    vAmount = txtAmount.Text.Trim()
                End If
                'end sw 12/02/2003

                'TR - Added Batch ID & Ref
                If m_lBatchID > 0 Then
                    vBatchID = m_lBatchID
                Else

                    vBatchID = Nothing
                End If

                'Go get the data from the business object

                m_lReturn = g_oBusiness.SearchReceiptDetails(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vStartDate:=vStartDate, v_vEndDate:=vEndDate, v_vReceiptTypeId:=vReceiptTypeId, v_vAccount:=vAccount, v_vMediaReference:=vMediaReference, v_vTheirReference:=vTheirReference, v_vAmount:=vAmount, v_vBatchNumber:=vBatchID, v_vMediaTypeId:=vMediaTypeId, v_vReceiptNumber:=vReceiptNumber, v_vBatchReference:=vBatchReference)


            ElseIf m_lCashListTypeID = ACPaymentType Then

                ' Check if document (from) date has been entered.
                If txtPayeeName.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vPayeeName = Nothing
                Else
                    vPayeeName = txtPayeeName.Text.Trim()
                End If

                ' Check if Account has been entered.
                If uctPaymentAccountLookUp.AccountId = 0 Then
                    ' Store a non entered value.

                    vAccountID = Nothing
                Else
                    vAccountID = uctPaymentAccountLookUp.AccountId
                End If

                ' Check if User Group has been entered.
                If uctPMLookUpPaymentType.ItemId = 0 Then

                    vPaymentTypeID = Nothing
                Else
                    vPaymentTypeID = uctPMLookUpPaymentType.ItemId
                End If

                ' Check if User Group has been entered.
                If uctPMLookupPaymentMediaType.ItemId = 0 Then

                    vPaymentMediaTypeID = Nothing
                Else
                    vPaymentMediaTypeID = uctPMLookupPaymentMediaType.ItemId
                End If

                ' Check if document (to) date has been entered.
                If txtChequeEFTNo.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vChequeEFTNo = Nothing
                Else
                    vChequeEFTNo = txtChequeEFTNo.Text.Trim()
                End If

                ' Check if Account has been entered.
                If uctPMLookUpPaymentStatus.ItemId = 0 Then
                    ' Store a non entered value.

                    vPaymentStatusID = Nothing
                Else
                    vPaymentStatusID = uctPMLookUpPaymentStatus.ItemId
                End If

                ' Check if Media Reference has been entered.
                If txtAmountPayment.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vAmount = Nothing
                Else
                    vAmount = txtAmountPayment.Text.Trim()
                End If

                ' Check if Their Reference has been entered.
                If txtPaymentBatchReference.Text.Trim().Length = 0 Then
                    ' Store a non entered value.

                    vBatchReference = Nothing
                Else
                    vBatchReference = txtPaymentBatchReference.Text.Trim()
                End If

                'TR - Added Batch ID & Ref
                If m_lBatchID > 0 Then
                    vBatchID = m_lBatchID
                Else

                    vBatchID = Nothing
                End If

                'Go get the data from the business object

                m_lReturn = g_oBusiness.SearchPaymentDetails(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vPayeeName:=vPayeeName, v_vAccountID:=vAccountID, v_vPaymentTypeID:=vPaymentTypeID, v_vPaymentMediaTypeID:=vPaymentMediaTypeID, v_vChequeEFTNo:=vChequeEFTNo, v_vPaymentStatusID:=vPaymentStatusID, v_vAmount:=vAmount, v_vBatchNumber:=vBatchID, v_vBatchReference:=vBatchReference)

            End If


            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.

                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No found search details

                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            ' Display the number of item found message.
            DisplayStatusFound()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        'Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Clear the search details.
            lvwSearchDetailsReceipt.Items.Clear()
            lvwSearchDetailsPayment.Items.Clear()


            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If


            If m_lCashListTypeID = ACReceiptingType Then
                ' Assign the details to the interface.
                For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                    ' Assign the details to the ListItem
                    oListItem = lvwSearchDetailsReceipt.Items.Add(CStr(m_vSearchData(ACReceiptCashListItemId, lRow)).Trim())

                    With oListItem

                        .Tag = CStr(lRow) ' Set the tag property with the index of
                        ' the search data storage.

                        ' Assign details to other the columns
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateMedium, vFieldValue:=CStr(m_vSearchData(ACReceiptTransactionDate, lRow)))
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACReceiptMediaType, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACReceiptMediaReference, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACReceiptTheirRefernce, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACReceiptType, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vSearchData(ACReceiptAccount, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 7).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vSearchData(ACReceiptAmount, lRow)))
                        ListViewHelper.GetListViewSubItem(oListItem, 8).Text = CStr(m_vSearchData(ACReceiptAllocationStatus, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 9).Text = CStr(m_vSearchData(ACPMUserReceipt, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 10).Text = CStr(m_vSearchData(ACReceiptCashListId, lRow)).Trim()
                    End With
                Next lRow

                ' Select the first item.
                lvwSearchDetailsReceipt.Items.Item(0).Selected = True

            ElseIf m_lCashListTypeID = ACPaymentType Then
                ' Assign the details to the interface.
                For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)
                    ' Assign the details to the ListItem
                    oListItem = lvwSearchDetailsPayment.Items.Add(CStr(m_vSearchData(ACPaymentCashListItemId, lRow)).Trim())

                    With oListItem

                        .Tag = CStr(lRow) ' Set the tag property with the index of
                        ' the search data storage.

                        ' Assign details to other the columns
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vSearchData(ACPaymentName, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vSearchData(ACPaymentAccountName, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vSearchData(ACPaymentTypeDesc, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vSearchData(ACPaymentMethod, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vSearchData(ACPaymentMediaRef, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(m_vSearchData(ACPaymentAmount, lRow)))
                        ListViewHelper.GetListViewSubItem(oListItem, 7).Text = CStr(m_vSearchData(ACPaymentStatus, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 8).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatDateMedium, vFieldValue:=CStr(m_vSearchData(ACPaymentDatePresented, lRow)))
                        ListViewHelper.GetListViewSubItem(oListItem, 9).Text = CStr(m_vSearchData(ACPaymentBatchRef, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 10).Text = CStr(m_vSearchData(ACPMUserPayment, lRow)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, 11).Text = CStr(m_vSearchData(ACPaymentCashListID, lRow)).Trim()
                    End With
                Next lRow

                ' Select the first item.
                lvwSearchDetailsPayment.Items.Item(0).Selected = True
            End If

            ' Enable the interface now that the search
            ' has completed.
            m_lReturn = CType(DisableInterface(bDisable:=False), gPMConstants.PMEReturnCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToProperties
    '
    ' Description: Updates the property member from the search data
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToProperties() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lCashListTypeID = ACReceiptingType Then
                ' Store the selected item's tag, so we can use this
                ' as the index to the search data storage details.

                lSelectedItem = Convert.ToString(lvwSearchDetailsReceipt.Items.Item(lvwSearchDetailsReceipt.FocusedItem.Index).Tag)
            ElseIf m_lCashListTypeID = ACPaymentType Then

                lSelectedItem = Convert.ToString(lvwSearchDetailsPayment.Items.Item(lvwSearchDetailsPayment.FocusedItem.Index).Tag)
            End If

            ' Update the property members.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the details from the search
            ' data storage to the property members.
            '
            ' Example:-
            '
            ' m_sName$ = Trim$(m_vSearchData(ACName, lSelectedItem&))
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************
            If m_lCashListTypeID = ACReceiptingType Then
                m_lCashListItemId = CInt(m_vSearchData(ACReceiptCashListItemId, lSelectedItem))
                m_lCashListID = CInt(m_vSearchData(ACReceiptCashListId, lSelectedItem))
            ElseIf m_lCashListTypeID = ACPaymentType Then
                m_lCashListItemId = CInt(m_vSearchData(ACPaymentCashListItemId, lSelectedItem))
                m_lCashListID = CInt(m_vSearchData(ACPaymentCashListID, lSelectedItem))
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the property members", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

   

    ' ***************************************************************** '
    ' Name: PropertiesToInterface
    '
    ' Description: Updates the interface details from the property
    '              members.
    '
    ' ***************************************************************** '
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try


            ' Update the interface details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the details from the
            ' property members to the interface.
            '
            ' Example:-
            '
            ' txtName.Text = Trim$(m_sName$)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            If m_lCashListTypeID = ACPaymentType Then
                Me.Tag = "CAP;820"
            ElseIf m_lCashListTypeID = ACReceiptingType Then
                Me.Tag = "CAP;100"
            End If

            ' Display all language specific captions.
            m_lReturn = CType(iPMForms.DisplayCaptions(Me), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the combo entry that means ALL - get the text from the res file
            Dim sAnyText As String = ""

            sAnyText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAllComboEntry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'sw payment maintenance 05-11-2002 Show the correct tab strip depdt on
            'cashlisttypeID
            If m_lCashListTypeID = ACPaymentType Then
                tabReceiptTab.Visible = False
                tabPaymentTab.Visible = True
                lvwSearchDetailsPayment.Visible = True
                lvwSearchDetailsReceipt.Visible = False
            ElseIf m_lCashListTypeID = ACReceiptingType Then
                tabReceiptTab.Visible = True
                tabPaymentTab.Visible = False
                lvwSearchDetailsPayment.Visible = False
                lvwSearchDetailsReceipt.Visible = True
            End If

            uctPMLookupReceiptType.FirstItem = sAnyText

            '**********
            ' MEvans : 23-06-2004 : CQ4740
            If m_lClaimMode = ACClaimModeDebtAllocation Then
                For lItem As Integer = 0 To uctPMLookupReceiptType.ListCount - 1
                    uctPMLookupReceiptType.ListIndex = lItem
                    If uctPMLookupReceiptType.ItemCode = ACReceiptTypeClaimRecovery Then
                        uctPMLookupReceiptType.ListIndex = lItem + 1
                        Exit For
                    End If
                Next
                uctPMLookupReceiptType.Enabled = False
            End If
            '**********

            cboPMLookupMediaType.FirstItem = sAnyText
            uctPMLookUpPaymentType.FirstItem = sAnyText
            uctPMLookupPaymentMediaType.FirstItem = sAnyText
            uctPMLookUpPaymentStatus.FirstItem = sAnyText

            ' Set the status of the Navigate button.
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    cmdNavigate.Visible = True
                    cmdNavigate.Enabled = False

                Case Else
                    cmdNavigate.Visible = False
            End Select

            ' Update the interface details with the
            ' property members.
            m_lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(SetFirstLastControls(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetailsReceipt.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwSearchDetailsPayment.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            ' {* USER DEFINED CODE (End) *}


            'ENABLE VIEW BUTTON (FIND RECEIPT OR FIND PAYMENT)
            If m_sActionkey = "" Or m_sActionkey = ACTViewBatch Or m_sActionkey = gACTLibrary.ACTViewCheque Then
                cmdView.Visible = True
            End If

            If m_sActionkey = ACTViewBatch Then
                txtPaymentBatchReference.Text = m_sBatchReference
                txtPaymentBatchReference.Enabled = False

                'TR - For ViewBatch mode, perform an automatic find
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                m_oGeneral.GetInterfaceDetails()
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearInterface
    '
    ' Description: Clears all of the interface details for a new
    '              search.
    '
    ' ***************************************************************** '
    Private Function ClearInterface() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if the user still wishes to clear
            ' the interface.


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClearDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the message.
            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            lvwSearchDetailsReceipt.Items.Clear()
            lvwSearchDetailsPayment.Items.Clear()

            ' Clear the search status bar.
            stbStatus.Text = ""

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to clear all of the interface details
            ' for a new search.
            '
            ' Example:-
            '
            '    txtName.Text = ""
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            If m_lCashListTypeID = ACReceiptingType Then
                txtDateFrom.Text = ""
                txtDateTo.Text = ""
                uctPMLookupReceiptType.ItemId = 0
                uctAccountLookup.AccountId = 0
                uctAccountLookup.Text = ""
                txtMediaReference.Text = ""
                txtTheirReference.Text = ""
                txtAmount.Text = ""
                txtReceiptBatchReference.Text = ""
                txtPaymentBatchReference.Text = ""
                cboPMLookupMediaType.ItemId = 0
                txtReceiptNumber.Text = ""

                ' Set focus to the search details.
                txtDateFrom.Focus()

            ElseIf m_lCashListTypeID = ACPaymentType Then
                txtPayeeName.Text = ""
                uctPaymentAccountLookUp.AccountId = 0
                uctPaymentAccountLookUp.Text = ""
                uctPMLookUpPaymentType.ItemId = 0
                uctPMLookupPaymentMediaType.ItemId = 0
                uctPMLookUpPaymentStatus.ItemId = 0
                txtChequeEFTNo.Text = ""
                txtAmountPayment.Text = ""
                txtPaymentBatchReference.Text = ""
                txtPayeeName.Focus()
            End If

            If m_sActionkey = ACTViewBatch Then
                txtPaymentBatchReference.Text = m_sBatchReference
            End If
            ' {* USER DEFINED CODE (End) *}

            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            m_lReturn = CType(DisableInterface(bDisable:=True), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to clear the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetFirstLastControls
    '
    ' Description: Sets the first and last data entry controls for
    '              each tab to the control array, for use with the
    '              keyboard navigation.
    '
    ' ***************************************************************** '
    Private Function SetFirstLastControls() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the control array with the number of
            ' tabs which contain data entry fields on (Remember
            ' that arrays start from zero, therefore you must
            ' subtract one from the number of tabs).
            ReDim m_ctlTabFirstLast(1, 0)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to set the first and last data entry
            ' controls for all of the tabs.
            '
            ' Example:-
            '
            '    Set m_ctlTabFirstLast(ACControlStart, 0) = txtName
            '    Set m_ctlTabFirstLast(ACControlEnd, 0) = txtAge
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            If m_lCashListTypeID = ACPaymentType Then
                m_ctlTabFirstLast(ACControlStart, 0) = txtPayeeName
                m_ctlTabFirstLast(ACControlEnd, 0) = txtReceiptBatchReference
            Else
                m_ctlTabFirstLast(ACControlStart, 0) = txtDateFrom
                m_ctlTabFirstLast(ACControlEnd, 0) = txtReceiptNumber
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    Private Function DisableInterface(ByRef bDisable As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdOK.Enabled = Not bDisable
            cmdPrint.Enabled = Not bDisable

            Return result

        Catch excep As System.Exception
            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & sMessage

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""
        Dim lItemsFound As Integer

        Try

            ' Store the total of item found.
            If Not Information.IsArray(m_vSearchData) Then
                lItemsFound = 0
            Else
                lItemsFound = (m_vSearchData.GetUpperBound(1) + 1)
            End If

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            stbStatus.Text = " " & lItemsFound & " " & sMessage

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide no.184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = CType(PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click
        Dim Catch_Renamed As Boolean = False
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdPrint_Click
        ' PURPOSE: Process the print request
        ' AUTHOR: Paul Cunnigham
        ' DATE: 31 October 2002, 16:12:11
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lSelectedItem, lCashListItemID As Integer


        Try
            Catch_Renamed = True

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' RVH 7/11/2003 CQ2810 - START : Selected item was not being located, value passed to DisplayReports was always 0
            If lvwSearchDetailsPayment.Items.Count = 0 And lvwSearchDetailsReceipt.Items.Count = 0 Then
                Exit Sub
            End If

            If m_lCashListTypeID = ACReceiptingType Then
                ' Store the selected item's tag, so we can use this
                ' as the index to the search data storage details.
                lSelectedItem = Convert.ToString(lvwSearchDetailsReceipt.Items.Item(lvwSearchDetailsReceipt.FocusedItem.Index).Tag)
            ElseIf m_lCashListTypeID = ACPaymentType Then
                lSelectedItem = Convert.ToString(lvwSearchDetailsPayment.Items.Item(lvwSearchDetailsPayment.FocusedItem.Index).Tag)
            End If

            If m_lCashListTypeID = ACReceiptingType Then
                lCashListItemID = CInt(m_vSearchData(ACReceiptCashListItemId, lSelectedItem))
            ElseIf m_lCashListTypeID = ACPaymentType Then
                lCashListItemID = CInt(m_vSearchData(ACPaymentCashListItemId, lSelectedItem))
            End If

            'Display the chosen cashlist
            m_lReturn = CType(DisplayReports(r_lCashListItemId:=lCashListItemID), gPMConstants.PMEReturnCode)
            ' RVH 7/11/2003 CQ2810 - END

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

            If Catch_Renamed Then

                Select Case Information.Err().Number
                    Case Else
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPrint_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                        Exit Sub
                End Select

            End If
        End Try

    End Sub


    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click

        Dim lSelectedItem, lCashListItemID, lCashListID As Integer

        Try

            If lvwSearchDetailsPayment.Items.Count = 0 And lvwSearchDetailsReceipt.Items.Count = 0 Then
                Exit Sub
            End If

            If m_lCashListTypeID = ACReceiptingType Then
                ' Store the selected item's tag, so we can use this
                ' as the index to the search data storage details.
                lSelectedItem = Convert.ToString(lvwSearchDetailsReceipt.Items.Item(lvwSearchDetailsReceipt.FocusedItem.Index).Tag)
            ElseIf m_lCashListTypeID = ACPaymentType Then
                lSelectedItem = Convert.ToString(lvwSearchDetailsPayment.Items.Item(lvwSearchDetailsPayment.FocusedItem.Index).Tag)
            End If

            If m_lCashListTypeID = ACReceiptingType Then
                lCashListItemID = CInt(m_vSearchData(ACReceiptCashListItemId, lSelectedItem))
                lCashListID = CInt(m_vSearchData(ACReceiptCashListId, lSelectedItem))
            ElseIf m_lCashListTypeID = ACPaymentType Then
                lCashListItemID = CInt(m_vSearchData(ACPaymentCashListItemId, lSelectedItem))
                lCashListID = CInt(m_vSearchData(ACPaymentCashListID, lSelectedItem))
            End If

            m_lReturn = CType(DisplayCashListItems(lCashListID, lCashListItemID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to display " & (IIf(m_lCashListTypeID = ACReceiptingType, "payment.", "receipt.")), "An Error Occured", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

        Catch excep As System.Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to view the cash list item", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdView_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTFindCashListItem.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function DisplayCashListItems(ByVal v_lCashListID As Integer, ByVal v_lCashListItemID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayCashListItems
        ' PURPOSE: Call the iACTCashListItem component in view mode
        ' AUTHOR: Paul Cunnigham
        ' DATE: 17 October 2002, 16:12:11
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oCashListItem As iACTCashListItem.Interface_Renamed
        Dim vKeys As Object



        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oCashListItem = New iACTCashListItem.Interface_Renamed()

            With oCashListItem

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                .CallingAppName = "iACTFindCashListItem.Interface"

                If m_lCashListTypeID = ACPaymentType Then
                    'create the keys here
                    'Set the start up options

                    m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                    If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ReDim vKeys(1, 4)


                    vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListId

                    vKeys(1, 0) = v_lCashListID

                    vKeys(0, 1) = PMNavKeyConst.ACTKeyNameCashListItemId

                    vKeys(1, 1) = v_lCashListItemID

                    vKeys(0, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

                    vKeys(1, 2) = ACPaymentType

                    vKeys(0, 3) = PMNavKeyConst.ACTKeyNameCashListItemMode

                    vKeys(1, 3) = gACTLibrary.ACTUseListHidden

                    Select Case m_sActionkey
                        Case gACTLibrary.ACTEditCheque

                            vKeys(0, 4) = PMNavKeyConst.ACTKeyNameActionKey

                            vKeys(1, 4) = gACTLibrary.ACTEditCheque
                        Case gACTLibrary.ACTCancelCheque

                            vKeys(0, 4) = PMNavKeyConst.ACTKeyNameActionKey

                            vKeys(1, 4) = gACTLibrary.ACTCancelCheque
                        Case gACTLibrary.ACTStopCheque

                            vKeys(0, 4) = PMNavKeyConst.ACTKeyNameActionKey

                            vKeys(1, 4) = gACTLibrary.ACTStopCheque
                    End Select

                ElseIf m_lCashListTypeID = ACReceiptingType Then

                    m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                    If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return result
                    End If

                    ReDim vKeys(1, 3)


                    vKeys(0, 0) = PMNavKeyConst.ACTKeyNameCashListId

                    vKeys(1, 0) = v_lCashListID

                    vKeys(0, 1) = PMNavKeyConst.ACTKeyNameCashListItemId

                    vKeys(1, 1) = v_lCashListItemID

                    vKeys(0, 2) = PMNavKeyConst.ACTKeyNameCashListTypeId

                    vKeys(1, 2) = ACReceiptingType

                    vKeys(0, 3) = PMNavKeyConst.ACTKeyNameCashListItemMode

                    vKeys(1, 3) = gACTLibrary.ACTUseListHidden

                End If


                .SetKeys(vKeys)

                m_lErrorNumber = .Start()

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End With

            'refresh the list
            cmdFindNow_Click(cmdFindNow, New EventArgs())

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCashListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


            End Select

        Finally
            oCashListItem.Dispose()
            oCashListItem = Nothing



        End Try
        Return result
    End Function

    Private Function DisplayReports(ByRef r_lCashListItemId As Integer) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: DisplayReports
        ' PURPOSE: Call the Crystal Reports component
        ' AUTHOR: Paul Cunnigham
        ' DATE: 17 October 2002, 16:12:11
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oReport As iPMBReportPrint.Interface_Renamed
        Const ksReportGroupCode As String = "ACTFR1"


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            oReport = New iPMBReportPrint.Interface_Renamed()

            With oReport

                m_lReturn = .Initialise()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                .CallingAppName = "iACTFindCashList.Interface"

                'Ensure the PMView is set so we enter in read only mode
                m_lErrorNumber = .SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView, vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                'Set the start up options
                Dim vKeys(1, 4) As Object
                'Set the name of parameter1
                ' RVH 7/11/2003 CQ2810 - Parameter name was incorrect - was "cashlist_id" s/be "cashListItem_id"

                vKeys(0, 0) = "param_name1"

                vKeys(1, 0) = "cashListItem_id"
                'set the value of parameter1

                vKeys(0, 1) = "cashListItem_id"

                vKeys(1, 1) = r_lCashListItemId
                'Tell the Report Print component that we want to
                'filter the list of reports that are displayed

                vKeys(0, 2) = "filter_reports"
                'set the filter name to 'report_group'

                vKeys(1, 2) = "report_group"
                'set the filter value for the above filter to csFindCashListReportGroup

                vKeys(0, 3) = "report_group"

                vKeys(1, 3) = ksReportGroupCode
                'set so the params we pass don't get trashed

                vKeys(0, 4) = "save_params"
                'vKeys(1, 4) 'NOT USED

                .SetKeys(vKeys)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
                m_lErrorNumber = .Start()

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

            End With

            result = gPMConstants.PMEReturnCode.PMTrue



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayReports", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    Return result
            End Select

        Finally
            oReport.Dispose()
            oReport = Nothing



        End Try
        Return result
    End Function

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            '    ' Set the status for the business object.
            'BB    m_lReturn& = g_oBusiness.SetStatus( _
            ''        sProcessStatus:=m_sProcessStatus$, _
            ''        sMapStatus:=m_sMapStatus$, _
            ''        sStepStatus:=m_sStepStatus$)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the status for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the rules for validating fields.

            m_lReturn = CType(iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields), gPMConstants.PMEReturnCode)

            'Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check if the search contains more or equal
            ' to the miniumum search length.

            ' {* USER DEFINED CODE (Begin) *}

            '    If (Len(Trim$(txtReference.Text)) < ACMinSearchLength) Then
            '        ' Because of the search length, we can't
            '        ' continue with the search.
            '
            '        ' Set the mouse pointer to normal.
            '        SetMousePointer PMMouseNormal
            '
            '        Exit Sub
            '    End If

            ' {* USER DEFINED CODE (End) *}
            'don't seacrch automatically sw 05-11-2002
            ' Gets the interface details to be displayed.
            ' START CHANGES - Changed By: AAB  - Changed On: 06-Apr-2004 22:22
            ' CQ 3795, to load the screen if we have a batchID passed in.
            If m_lBatchID > 0 Then
                'Set the txtPaymentBatchReference
                txtPaymentBatchReference.Text = m_sBatchReference

                m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the interface details.
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If
            ' END CHANGES - Changed By: AAB  - Changed On: 06-Apr-2004 22:22

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

           

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabReceiptTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabReceiptTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabReceiptTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabReceiptTab, SSTabHelper.GetSelectedIndex(tabReceiptTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabReceiptTab, SSTabHelper.GetTabCount(tabReceiptTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabReceiptTab) < (SSTabHelper.GetTabCount(tabReceiptTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabReceiptTab, SSTabHelper.GetSelectedIndex(tabReceiptTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabReceiptTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabReceiptTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabReceiptTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabReceiptTab)).Focus()
                            End If
                        End If
                End Select
            End With

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub tabReceiptTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabReceiptTab.SelectedIndexChanged

        Try

            With tabReceiptTab
                ' Set the default button.
                'BB        If (.Tab < cmdNext.Count) Then
                '            cmdNext(.Tab).Default = True
                '        Else
                '            cmdOK.Default = True
                '        End If

                VB6.SetDefault(cmdOK, True)

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabReceiptTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabReceiptTab)).Focus()
                End If

            End With

        Catch



            ' Error Section.


            tabReceiptTabPreviousTab = tabReceiptTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim oValidation As bSIRMediaTypeValidation.Business
        Dim sMediaCode, sPaymentTypeCode As String
        Dim lRow, lMediaTypeID As Integer
        Dim sStatusCode As String = ""
        Dim bIsStoppable, bIsManual As Boolean

        ' Click event of the OK button.
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            Else
                ' Everything OK, so we can hide the interface.
                If m_lCashListTypeID = ACPaymentType Then

                    'establish the mediatypeID
                    lRow = Convert.ToString(lvwSearchDetailsPayment.FocusedItem.Tag)
                    ' the search data storage.
                    lMediaTypeID = CInt(CStr(m_vSearchData(ACPaymentMediaTypeID, lRow)).Trim())
                    sStatusCode = CStr(m_vSearchData(ACPaymentStatusCode, lRow)).Trim()
                    sPaymentTypeCode = CStr(m_vSearchData(ACPaymentTypeCode, lRow)).Trim()
                    bIsStoppable = gPMFunctions.NullToBoolean(CStr(m_vSearchData(ACPaymentIsStoppable, lRow)))
                    ' RVH 10/12/2003 CQ3623
                    bIsManual = gPMFunctions.NullToBoolean(CStr(m_vSearchData(ACPaymentIsManual, lRow)))

                    'create an instance of the mediatypevalidation object
                    Dim temp_oValidation As Object
                    m_lReturn = g_oObjectManager.GetInstance(temp_oValidation, "bSIRMediaTypeValidation.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                    oValidation = temp_oValidation

                    ' Check the return value.
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Exit Sub
                    End If

                    If lMediaTypeID <> 0 Then

                        sMediaCode = oValidation.GetValidationCode(lMediaTypeID)
                    End If

                    oValidation = Nothing


                    Select Case m_sActionkey
                        Case gACTLibrary.ACTEditCheque
                            'check for issued cheque
                            'KG 05/06/03 - logic in IF block swapped round
                            ' RVH 10/12/2003 CQ3623 - rectified logic as per spec, swapped IF block back
                            If bIsManual And (sStatusCode = ACStatusIssued Or sStatusCode = ACStatusPresented) Then
                                m_lCashlistItemMode = gACTLibrary.ACTUseListHidden
                            Else
                                MessageBox.Show("This form of payment cannot be altered." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Process Aborted.", "Error: Payment Details cannot be altered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If
                        Case gACTLibrary.ACTStopCheque

                            If bIsStoppable And (sStatusCode = ACStatusIssued Or sStatusCode = ACStatusStopRequested) Then
                                'Display the chosen cashlist
                                m_lCashlistItemMode = gACTLibrary.ACTUseListHidden

                            ElseIf sStatusCode = ACStatusStopped Then
                                MessageBox.Show("This payment has already been stopped." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Process " & _
                                                "Aborted.", "Error: Payment Details cannot be altered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            Else
                                MessageBox.Show("This form of payment cannot be altered." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                                "Process Aborted.", "Error: Payment Details cannot be altered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If

                        Case gACTLibrary.ACTCancelCheque
                            'check for issued cheque
                            If bIsStoppable And (sPaymentTypeCode <> ACPaymentTypeCommission) And (sPaymentTypeCode <> ACPaymentTypeClaim) Then
                                If sStatusCode <> ACStatusCancelled Then
                                    'Display the chosen cashlist
                                    m_lCashlistItemMode = gACTLibrary.ACTUseListHidden
                                Else
                                    MessageBox.Show("This payment has already been cancelled." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Process " & _
                                                    "Aborted.", "Error: Payment Details cannot be altered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                    Exit Sub
                                End If
                            Else
                                MessageBox.Show("This form of payment cannot be cancelled." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Process " & _
                                                "Aborted.", "Error: Payment Details cannot be altered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Exit Sub
                            End If

                    End Select


                ElseIf m_lCashListTypeID = ACReceiptingType Then
                    'do nothing

                End If
                'return to navigator process
                Me.Hide()

            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.

        Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040514 : Code changes related to Performance Enhancement - START
            '               Check if atleast on field is provided for search criteria
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            m_lReturn = CType(CheckMandatory(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMFail Then
                    ' There is some search string provided. But not matching the minimum search chars
                    MessageBox.Show("Search string must be atleast [" & ACMinSearchLength & "] characters length." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Note : The Results are limited to 500 records.", "Warning: Search string length is less than minimum length.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    MessageBox.Show("Please provide atleast one search criteria." & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & "Note : The Results are limited to 500 records.", "Warning: Minimum search criteria required.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
                Exit Sub
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040514 : Code changes related to Performance Enhancement - END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            ' Set the focus.
            If m_lCashListTypeID = ACReceiptingType Then
                lvwSearchDetailsReceipt.Focus()
            ElseIf m_lCashListTypeID = ACPaymentType Then
                lvwSearchDetailsPayment.Focus()
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try

            ' Clear the interface details.
            m_lReturn = CType(ClearInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

            ' Process the next set of actions.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetailsReceipt_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetailsReceipt.DoubleClick

        ' Double click event for the search details.

        Try


            ' Check if there are any items available.
            If lvwSearchDetailsReceipt.Items.Count = 0 Then
                Exit Sub
            End If

            'sw issue 2822 if view button is visible then fire the click event
            If cmdView.Visible Then
                cmdView_Click(cmdView, New EventArgs())
                Exit Sub
            End If


            'Process the OK click event
            cmdOK_Click(cmdOK, New EventArgs())

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsReceipt_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub lvwSearchDetailsPayment_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetailsPayment.DoubleClick

        ' Double click event for the search details.

        Try

            ' Check if there are any items available.
            If lvwSearchDetailsPayment.Items.Count = 0 Then
                Exit Sub
            End If

            'sw 17/03/2003 issue 2822 If view button is visible then fire the click event
            If cmdView.Visible Then
                cmdView_Click(cmdView, New EventArgs())
                Exit Sub
            End If

            'Process the OK click event
            cmdOK_Click(cmdOK, New EventArgs())

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the double click event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsPayment_DblClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetailsReceipt_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetailsReceipt.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsReceipt_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetailsPayment_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetailsPayment.Enter

        ' GotFocus Event for the search details

        Try

            ' Set the default button
            VB6.SetDefault(cmdOK, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsPayment_GotFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub lvwSearchDetailsReceipt_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetailsReceipt.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsReceipt_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetailsPayment_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetailsPayment.Leave

        ' LostFocus Event for the search details

        Try

            ' Set the default button.
            VB6.SetDefault(cmdFindNow, True)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the default button", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsPayment_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetailsReceipt_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetailsReceipt.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetailsReceipt.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            With lvwSearchDetailsReceipt

                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsReceipt, False)
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetailsReceipt, (ListViewHelper.GetSortOrderProperty(lvwSearchDetailsReceipt) + 1) Mod 2)
                    'Use the special sort function for dates
                    ListViewFunc.ListViewSortByDate(lvwSearchDetailsReceipt, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwSearchDetailsReceipt))
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsReceipt, True)

                    ' If current sort column header is pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetailsReceipt)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetailsReceipt, (ListViewHelper.GetSortOrderProperty(lvwSearchDetailsReceipt) + 1) Mod 2)

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsReceipt, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetailsReceipt, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetailsReceipt, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsReceipt, True)
                End If
            End With

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsReceipt_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwSearchDetailsPayment_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetailsPayment.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetailsPayment.Columns(eventArgs.Column)

        ' Column click event for the search details

        Try

            With lvwSearchDetailsPayment

                'Identify the Date type columns
                If Convert.ToString(ColumnHeader.Tag).ToUpper() = "DATESORT" Then
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsPayment, False)
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetailsPayment, (ListViewHelper.GetSortOrderProperty(lvwSearchDetailsPayment) + 1) Mod 2)
                    'Use the special sort function for dates
                    ListViewFunc.ListViewSortByDate(lvwSearchDetailsPayment, ColumnHeader.Index + 1 - 1, ListViewHelper.GetSortOrderProperty(lvwSearchDetailsPayment))
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsPayment, True)

                    ' If current sort column header is pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwSearchDetailsPayment)) Then
                    ' Set sort order opposite of current direction.
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetailsPayment, (ListViewHelper.GetSortOrderProperty(lvwSearchDetailsPayment) + 1) Mod 2)

                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsPayment, False)
                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwSearchDetailsPayment, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwSearchDetailsPayment, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwSearchDetailsPayment, True)
                End If
            End With

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetailsPayment_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Enter
        iPMFunc.SelectText(txtAmount)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtAmount)

    End Sub

    Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtAmount)

    End Sub



    Private Sub txtReceiptBatchReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReceiptBatchReference.Enter

        iPMFunc.SelectText(txtReceiptBatchReference)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtReceiptBatchReference)

    End Sub

    Private Sub txtReceiptBatchReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReceiptBatchReference.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtDateFrom)

    End Sub

    Private Sub txtDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Enter

        iPMFunc.SelectText(txtDateFrom)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtDateFrom)

    End Sub

    Private Sub txtDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateFrom.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtDateFrom)

    End Sub
    Private Sub txtDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Enter

        iPMFunc.SelectText(txtDateTo)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtDateTo)

    End Sub

    Private Sub txtDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateTo.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtDateTo)

    End Sub

    Private Sub txtMediaReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMediaReference.Enter

        iPMFunc.SelectText(txtMediaReference)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtMediaReference)

    End Sub

    Private Sub txtMediaReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMediaReference.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtMediaReference)

    End Sub

    Private Sub txtReceiptNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReceiptNumber.Enter

        iPMFunc.SelectText(txtReceiptNumber)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtReceiptNumber)

    End Sub

    Private Sub txtReceiptNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReceiptNumber.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtMediaReference)

    End Sub

    Private Sub txtTheirReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTheirReference.Enter

        iPMFunc.SelectText(txtTheirReference)


        m_lReturn = m_oFormfields.GotFocus(ctlControl:=txtTheirReference)

    End Sub

    Private Sub txtTheirReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTheirReference.Leave


        m_lReturn = m_oFormfields.LostFocus(ctlControl:=txtTheirReference)

    End Sub


    ' ***************************************************************** '
    ' Name: CheckMandatory
    '
    ' Description: Check if all mandatory fields have been entered in
    '              order for the search to proceed.
    '
    ' Edit History  :
    ' RAM20040517   : Created
    ' ***************************************************************** '
    Private Function CheckMandatory() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check all fields for data, based on Whether we are in Payments (or) Receipts screen Tab
            If m_lCashListTypeID = ACReceiptingType Then

                ' We are in Receipts Screen Tab

                If txtDateFrom.Text.Trim() <> "" Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If txtDateTo.Text.Trim() <> "" Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                ' -1 is no Entries selected, 0 is (Any)
                If uctPMLookupReceiptType.ListIndex > 0 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If uctAccountLookup.Text.Trim() <> "" Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If txtMediaReference.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtMediaReference.Text) Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

                If txtTheirReference.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtTheirReference.Text) Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

                If txtAmount.Text.Trim() <> "" Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If txtReceiptBatchReference.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtTheirReference.Text) Then
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

                ' -1 is no Entries selected, 0 is (Any)
                If cboPMLookupMediaType.ListIndex > 0 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If txtReceiptNumber.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtTheirReference.Text) Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

            ElseIf m_lCashListTypeID = ACPaymentType Then

                ' We are in Payments Screen Tab

                If txtPayeeName.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtPayeeName.Text) Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

                If uctPaymentAccountLookUp.Text.Trim() <> "" Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                ' -1 is no Entries selected, 0 is (Any)
                If uctPMLookUpPaymentType.ListIndex > 0 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                ' -1 is no Entries selected, 0 is (Any)
                If uctPMLookupPaymentMediaType.ListIndex > 0 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If txtChequeEFTNo.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtChequeEFTNo.Text) Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

                ' -1 is no Entries selected, 0 is (Any)
                If uctPMLookUpPaymentStatus.ListIndex > 0 Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If


                If txtAmountPayment.Text.Trim() <> "" Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If txtPaymentBatchReference.Text.Trim() <> "" Then
                    ' Performance Issue, check field length as well
                    If CheckFieldLength(txtChequeEFTNo.Text) Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check for mandatory fields", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : CheckFieldLength
    ' Description   : Function to check length of text field excluding
    '                 wildcards
    ' Edit History  :
    ' RAM20040517   : Created
    ' ***************************************************************** '
    Private Function CheckFieldLength(ByVal sData As String) As Boolean
        Dim result As Boolean = False

        If sData.Replace("%", "").Trim().Length >= ACMinSearchLength Then
            result = True
        End If
        Return result
    End Function
End Class
