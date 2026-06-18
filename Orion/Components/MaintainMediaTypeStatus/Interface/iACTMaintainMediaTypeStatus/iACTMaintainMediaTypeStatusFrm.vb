Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form



    Private Const ACClass As String = "frmInterface"

    Private m_oGeneral As iACTMaintainMediaTypeStatus.General

    Private m_oACTMaintainMediaTypeStatus As Object

    Public m_vSearchData(,) As Object
    Dim m_oBusiness As Object

    Private m_sCallingAppName As String = ""
    Private m_sProcessStatus As String = ""
    Private m_sMapStatus As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_lReturn As Integer
    Private m_vSourceArray(,) As Object

    Private m_oPMUser As bPMUser.Business
    Private m_vBranch As Integer
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_bIsNavigatorProcess As Boolean
    Private m_sPolicyRef As String = ""
    Private m_dtCollectionDateFrom As Date
    Private m_sDocumentRef As String = ""
    Private m_dtCollectionDateTo As Date
    Private m_sMediaTypeStatus As String = ""
    Private m_lMediaTypeStatusId As Integer
    'Start Renuka PN 63396
    'Private m_lPartyCnt As Long
    'End Renuka PN 63396
    Private m_vSelectedReceiptsArray As Object
    Private m_vOriginalArray(,) As Object
    Private m_dtUpdateDate As Date
    Private m_sComments As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_lBranchId As Integer
    Private m_lBankAccountId As Integer
    Private m_sClientCode As String = ""
    Private m_vResultReceiptsArray(,) As Object
    Private m_sStepStatus As String = ""


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


    Public Property Status() As Integer
        Get

            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            m_lStatus = Value

        End Set
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




    Public Property MediaTypeStatus() As String
        Get

            Return m_sMediaTypeStatus

        End Get
        Set(ByVal Value As String)

            m_sMediaTypeStatus = Value

        End Set
    End Property


    Public Property MediaTypeStatusId() As Integer
        Get

            Return m_lMediaTypeStatusId

        End Get
        Set(ByVal Value As Integer)

            m_lMediaTypeStatusId = Value

        End Set
    End Property


    Public Property UpdateDate() As Date
        Get

            Return m_dtUpdateDate

        End Get
        Set(ByVal Value As Date)

            m_dtUpdateDate = Value

        End Set
    End Property



    Public Property Comments() As String
        Get

            Return m_sComments

        End Get
        Set(ByVal Value As String)

            m_sComments = Value

        End Set
    End Property

    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetStatus"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus = sProcessStatus.Trim()
            m_sMapStatus = sMapStatus.Trim()
            m_sStepStatus = sStepStatus.Trim()


        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PerformSearch
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function PerformSearch() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PerformSearch"
        Dim iBankAccountId As Integer
        Dim dtCollectionDateFrom, dtCollectionDateTo As Date
        Dim lBranch As Integer
        Dim sClientCode, sPolicyNumber, sMediaReference, sDocumentRef As String
        'Start Renuka PN 63396
        Dim sShortName As String = ""
        'End Renuka PN 63396
        Dim lDrawnBankId As Integer
        Dim vResultArrray As Object
        Dim lMediaTypeStatus As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'Start Arul PN 63413
            '    m_lReturn& = DisableInterface( _
            ''        bDisable:=True)
            'End Arul PN 63413
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            If uctBankAccount.Id > 0 Then
                iBankAccountId = uctBankAccount.Id
            Else

                iBankAccountId = -1
            End If


            If Information.IsDate(txtCollectionDateFrom.Text) And txtCollectionDateFrom.Text.Trim() <> "" Then
                dtCollectionDateFrom = CDate(txtCollectionDateFrom.Text)
            Else
                dtCollectionDateFrom = #12/30/1899#
            End If

            If Information.IsDate(txtCollectionDateTo.Text) And txtCollectionDateTo.Text.Trim() <> "" Then
                dtCollectionDateTo = CDate(txtCollectionDateTo.Text)
            Else
                dtCollectionDateTo = #12/30/1899#
            End If


            If cboBranch.SelectedIndex > 0 Then
                lBranch = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            Else
                '
                lBranch = -1
            End If

            If cboDrawnBankName.ListIndex > 0 Then
                lDrawnBankId = cboDrawnBankName.ItemData(cboDrawnBankName.ListIndex)
            Else
                lDrawnBankId = -1
            End If


            'Start Renuka PN 63396
            'If m_lPartyCnt <= 0 Then
            '   m_lPartyCnt = -1
            'End If

            If txtClientCode.Text.Trim() <> "" Then
                sShortName = txtClientCode.Text.Trim()
            Else
                sShortName = ""
            End If
            'End Renuka PN 63396

            If txtPolicyNumber.Text.Trim() <> "" Then
                sPolicyNumber = txtPolicyNumber.Text.Trim()
            Else
                sPolicyNumber = ""
            End If

            If txtMediaReference.Text.Trim() <> "" Then
                sMediaReference = txtMediaReference.Text.Trim()
            Else
                sMediaReference = ""
            End If

            If txtDocumentRef.Text.Trim() <> "" Then
                sDocumentRef = txtDocumentRef.Text.Trim()
            Else
                sDocumentRef = ""
            End If

            If cboMediaTypeStatus.ListIndex > 0 Then

                lMediaTypeStatus = cboMediaTypeStatus.ItemData(cboMediaTypeStatus.ListIndex)
            Else
                lMediaTypeStatus = -1
            End If

            'Start Renuka PN 63396 - Changed a paramater

            m_lReturn = g_oACTMaintainMediaTypeStatus.GetReceiptsForMediaTypeStatusMaintenance(v_iUserID:=g_iUserID, r_vResultArray:=m_vOriginalArray, v_vBranchID:=lBranch, v_vBankAccountID:=iBankAccountId, v_vShortName:=sShortName, v_vInsurance_Ref:=sPolicyNumber, v_vCollectionDateFrom:=dtCollectionDateFrom, v_vCollectionDateTo:=dtCollectionDateTo, v_vMediaReference:=sMediaReference, v_vMediaTypeStatusID:=lMediaTypeStatus, v_vDrawnBankID:=lDrawnBankId, v_vDocumentRef:=sDocumentRef, v_vMaxRowsToFetch:=0)

            'End Renuka PN 63396


            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    'Success
                Case gPMConstants.PMEReturnCode.PMNotFound
                    'No record Found
                Case Else
                    gPMFunctions.RaiseError(kMethodName, "Failed to get search details from the business object")

            End Select

            m_vSearchData = VB6.CopyArray(m_vOriginalArray)



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
        Const kMethodName As String = "DataToInterface"
        Dim oListItem As ListViewItem

        Dim sClientCode, sClientName, sPolicyNumber, sMediaTypeStatus As String
        Dim lCashItemId As Integer
        Dim sDrawnBankName, sMediaReference, sBranchName, sBankAccount, sBatchReference, sTheirReference, sOurReference, sDocumentReference, sPaymentType, sMediaType As String

        Dim lCashListItemID, lMediaTypeID, lMediaTypeStatusID, lPartyCnt, lInsuranceFileCnt As Integer
        Dim dtBankReconcilationDate As Date


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the search details.
            lvwFindReceipts.Items.Clear()

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
            End If

            ' Assign the details to the interface.

            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                oListItem = lvwFindReceipts.Items.Insert(0, "")
                sDocumentReference = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_DOCUMENT_REF, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexDocumentRef).Text = sDocumentReference.Trim()

                sBranchName = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_BRANCH, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexBranch).Text = sBranchName.Trim()

                sClientCode = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_CLIENT_CODE, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexClientCode).Text = sClientCode.Trim()

                sClientName = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CLIENT_NAME, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexClientName).Text = sClientName.Trim()

                sPolicyNumber = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_POLICY_NUMBER, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexPolicyNumber).Text = sPolicyNumber.Trim()

                sMediaType = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPE, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexMediaType).Text = sMediaType.Trim()

                sMediaReference = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIAREFERENCE, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexMediaReference).Text = sMediaReference.Trim()

                sDrawnBankName = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_DRAWN_BANK_NAME, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexDrawnBankName).Text = sDrawnBankName

                sMediaTypeStatus = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexMediaTypeStatus).Text = sMediaTypeStatus

                lCashItemId = CInt(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CASHLISTITEM_ID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexCashlistitemId).Text = CStr(lCashItemId)


                lMediaTypeID = CInt(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPE_ID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexMediaTypeId).Text = CStr(lMediaTypeID)

                lMediaTypeStatusID = gPMFunctions.ToSafeLong(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS_ID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexMediaTypeStatusId).Text = CStr(lMediaTypeStatusID)

                lInsuranceFileCnt = gPMFunctions.ToSafeLong(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_INSURANCEFILE_ID, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexInsuranceFileId).Text = CStr(lInsuranceFileCnt)
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexUpdatedDate).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kReceiptMaintColHIndexComments).Text = ""


                oListItem.Tag = CStr(lRow)

            Next lRow

            ' Select the first item.


            If lvwFindReceipts.Items.Count > 0 Then
                cmdSelectAll.Enabled = True
            End If

            ' Enable the interface now that the search
            ' has completed.
            'Start  Arul PN 63413
            '    m_lReturn& = DisableInterface( _
            ''        bDisable:=False)
            'End  Arul PN 63413
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
            End If


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
        Const kMethodName As String = "DataToProperties"
        Dim lSelectedItem, lSelectedReceiptsCount As Integer


        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            Dim lRowCount As Integer
            lRowCount = 0
            ReDim m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_MAX_INDEX, 0)
            For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
                If lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then

                    ReDim Preserve m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_MAX_INDEX, lRowCount)
                    m_vResultReceiptsArray(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CASHLISTITEM_ID, lRowCount) = gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexCashlistitemId).Text)

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_COMMENTS, lRowCount) = lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexComments).Text

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_DATE_MODIFIED, lRowCount) = lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexUpdatedDate).Text

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_DOCUMENT_REF, lRowCount) = lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexDocumentRef).Text

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_INSURANCEFILE_ID, lRowCount) = gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexInsuranceFileId).Text)

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_MEDIATYPESTATUS_ID, lRowCount) = gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId).Text)

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_MEDIATYPE_ID, lRowCount) = gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeId).Text)

                    m_vResultReceiptsArray(MainModule.enuUpdMediaTypeStatusFields.enuUpdMTSV_USER_ID, lRowCount) = g_iUserID
                    lRowCount += 1
                End If
            Next lReceiptsCount

            If Information.IsArray(m_vResultReceiptsArray) And m_vResultReceiptsArray.GetUpperBound(0) >= 1 Then

                m_lReturn = m_oACTMaintainMediaTypeStatus.UpdateMediaTypeStatusForPolicyReciepts(v_vUpdateArray:=m_vResultReceiptsArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("DataToProperties", "UpdateMediaTypeStatusForPolicyReciepts method failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If


        Catch ex As Exception

            ' Error Section.
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ' ********************************************************************************* '
    ' Name: Private Function                                                            '
    '                                                                                   '
    ' Description: Checks that the transaction is for one of the branches being paid    '
    '                                                                                   '
    ' ********************************************************************************* '
    'UPGRADE_NOTE: (7001) The following declaration (ValidSource) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ValidSource(ByVal vSource As Object) As Boolean
    '
    'Dim result As Boolean = False
    'If Not Information.IsArray(m_vSourceArray) Then
    'Return True
    'End If
    'For 'i As Integer = 1 To m_vSourceArray.GetUpperBound(1)

    'If CInt(m_vSourceArray(1, i)) = CInt(vSource) Then
    'result = True
    'End If
    'Next i
    'Return result
    'End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Fill Branchs
            m_lReturn = GetBranchDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            uctBankAccount.FirstItem = ("(Any)")

            cboDrawnBankName.FirstItem = "(Any)"

            cboMediaTypeStatus.FirstItem = "(Any)"
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwFindReceipts.Handle.ToInt32(), v_vShowRowSelect:=True)



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
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
        Const kMethodName As String = "ClearInterface"
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Dim ctrl As Control

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display the message.
            iMsgResult = MessageBox.Show("A new search will clear all of your existing search details." & Strings.Chr(13) & Strings.Chr(10) & _
                     "Do you wish to continue?", "New Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                ' Don't continue with the clear.
                Return result
            End If

            ' Clear the interface details.

            ' Clear the search data array.
            m_vSearchData = Nothing

            ' Clear the search list details.
            txtClientCode.Text = ""
            txtCollectionDateFrom.Text = ""
            txtCollectionDateTo.Text = ""
            txtDocumentRef.Text = ""
            txtMediaReference.Text = ""
            txtPolicyNumber.Text = ""
            cboBranch.SelectedIndex = 0
            cboDrawnBankName.ItemId = 0
            cboMediaTypeStatus.ItemId = 0
            lvwFindReceipts.Items.Clear()
            uctBankAccount.ListIndex = 0
            cboBranch.Focus()
            ' Arul PN 63413
            cmdSelectAll.Text = "&Select All"
            cmdSelectAll.Enabled = False
            ' Disable parts of the interface, so the
            ' user can now only enter a new search
            'Start  Arul PN 63413
            'm_lReturn& = DisableInterface(bDisable:=True)
            m_lReturn = EnableDisableOkButton()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method EnableDisableOkButton calling failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            'End  Arul PN 63413
            With lvwFindReceipts
                '.SortOrder = (.SortOrder + 1) Mod 2
                '.SortKey = ColumnHeader.Index - 1
                ListViewHelper.SetSortedProperty(lvwFindReceipts, True)
            End With



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"
        Dim sAnyText As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error.
                gPMFunctions.RaiseError("DisplayCaptions", "Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                    "Please check the file exists and the correct captions are available")
                Return result

            End If

            sAnyText = "Any"

            '    uctBankAccount.FirstItem = sAnyText
            '    cboPaymentType.FirstItem = sAnyText
            '    cboMediaType.FirstItem = sAnyText
            '    cboPaymentStatus.FirstItem = sAnyText



        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    'Start Arul PN 63413
    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '

    'Private Function DisableInterface( _
    ''    bDisable As Boolean) As Long
    '
    '    Const kMethodName = "DisableInterface"
    '
    '    On Error GoTo Catch
    '
    'Try:
    '    DisableInterface = PMTrue
    '
    '    cmdOK.Enabled = bDisable
    '
    '
    '    GoTo Finally
    '
    'Catch:
    '
    '    LogError _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=DisableInterface, _
    ''          v_sUsername:=g_sUsername
    '
    'Finally:
    '    Exit Function
    '    Resume
    '
    'End Function

    'End Arul PN 63413

    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayStatusSearching) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub DisplayStatusSearching()
    '
    'Static sMessage As String = ""
    '
    ''UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Try 
    '
    '
    ' Get message text if not already present.
    '
    'Catch 
    'End Try
    '
    '
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    '
    'Exit Sub


    '
    'End Sub



    'UPGRADE_NOTE: (7001) The following declaration (cmdAddTask_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdAddTask_Click()
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'm_lReturn = CreateWorkManagerTask()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    'GoTo Finally_Renamed
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Task Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddTask_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Finally_Renamed: '
    '
    'Exit Sub
    'Resume 
    '
    'End Sub





    'UPGRADE_NOTE: (7001) The following declaration (cmdHelp_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdHelp_Click()
    ' Fire up the help screen
    ''m_lReturn& = ShowHelp(dlgHelp, ScreenHelpID)
    'End Sub

    Private Sub cmdClientCode_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdClientCode_0.Click
        'Start Renuka PN 63396
        'Dim vCnt As Variant
        'End Renuka PN 63396
        Dim vShortName As String = ""
        Dim vName As Object
        Const kMethodName As String = "cmdClientCode_Click"


        Try

            'Start Renuka PN 63396 - Removed a paramter

            m_lReturn = SelectParty(vShortName:=vShortName, vName:=CStr(vName))
            'End Renuka PN 63396

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Raise Error
                gPMFunctions.RaiseError(v_sSource:=kMethodName, v_sDescription:="Failed to Process Select Party")
                Exit Sub
            End If

            txtClientCode.Text = vShortName



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Process Select Party, cmdClientCode_Click", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub cmdPolicyNumber_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPolicyNumber_1.Click
        Dim iPMBFindInsurance As Object


        Const kMethodName As String = "cmdPolicyNumber_Click"

        Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed
        Dim PolicyNumber As gPMConstants.PMEReturnCode

        Try

            PolicyNumber = gPMConstants.PMEReturnCode.PMTrue


            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                PolicyNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Set component properties and start interface

            oFindPolicy.CallingAppName = ACApp

            oFindPolicy.InsReference = txtPolicyNumber.Text

            oFindPolicy.FindMode = 1 'PN 36697


            m_lReturn = oFindPolicy.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                PolicyNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyInfo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            'Retrieve InsuranceRef and set as PolicyRef

            If oFindPolicy.Status <> gPMConstants.PMEReturnCode.PMCancel Then


                m_sPolicyRef = oFindPolicy.InsReference


                'Display Policy Reference on form
                txtPolicyNumber.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPolicyRef.Trim())
            End If
            ' Destroy Find Insurance object

            oFindPolicy.Dispose()
            oFindPolicy = Nothing




        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PolicyNumber, v_sUsername:=g_sUsername.Value, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    Private Sub cmdSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectAll.Click

        b_RunChecked = True
        Const kMethodName As String = "cmdSelectAll_Click"
        Dim oFindPolicy As Object
        Dim SelectAllReceipts As gPMConstants.PMEReturnCode

        Try

            SelectAllReceipts = gPMConstants.PMEReturnCode.PMTrue
            If cmdSelectAll.Text = "&Select All" Then
                For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
                    lvwFindReceipts.Items.Item(lReceiptsCount).Checked = True

                Next lReceiptsCount

                cmdUpdateAllSelected.Enabled = True
                cmdSelectAll.Text = "&DeSelect All"
                lvwFindReceipts.MultiSelect = True
            ElseIf cmdSelectAll.Text = "&DeSelect All" Then

                For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
                    lvwFindReceipts.Items.Item(lReceiptsCount).Checked = False

                    For lRow As Integer = m_vOriginalArray.GetLowerBound(1) To m_vOriginalArray.GetUpperBound(1)
                        If lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexCashlistitemId - 1).Text.Trim() = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CASHLISTITEM_ID, lRow)).Trim() Then

                            lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatus - 1).Text = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS, lRow))
                            lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexUpdatedDate - 1).Text = ""
                            lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexComments - 1).Text = ""
                        End If
                    Next lRow
                Next lReceiptsCount
                cmdOk.Enabled = False
                cmdUpdateAllSelected.Enabled = False
                cmdSelectAll.Text = "&Select All"
            End If

            b_RunChecked = False


        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SelectAllReceipts, v_sUsername:=g_sUsername.Value, excep:=ex)
        Finally
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdUpdateAllSelected_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdUpdateAllSelected.Click
        On Error GoTo Catch_Renamed
        Const kMethodName As String = "cmdUpdateAllSelected_Click"
        Dim lReceiptsCount As Integer
        Dim UpdateAllSelected As gPMConstants.PMEReturnCode
        Dim bIsPolicyCancelled, bIsClaimPaymentInitiated As Boolean
        Dim sInsuranceRef As String = ""

        If IsNothing(objfrmUpdateMediaTypeStatus) Then
            objfrmUpdateMediaTypeStatus = New frmUpdateMediaTypeStatus
        End If


        UpdateAllSelected = gPMConstants.PMEReturnCode.PMTrue
        With objfrmUpdateMediaTypeStatus
            .txtCommments.Text = ""
            .txtUpdateDate.Text = StringsHelper.Format(DateTime.Today, ACDateDispaly)

            .cboMediaTypeStatus.ItemId = 0
            .ShowDialog()
        End With

        If m_lStatus = gPMConstants.PMEReturnCode.PMOK Then
            For lReceiptsCount = 0 To lvwFindReceipts.Items.Count - 1
                If lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then

                    If gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId).Text.Trim()) <> m_lMediaTypeStatusId Then

                        If StringsHelper.ToDoubleSafe(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId).Text.Trim()) = ACMaintainMediaTypeStatusBounced Then

                            m_lReturn = m_oACTMaintainMediaTypeStatus.GetPolicyStatusForMediaTypeStatusMaintenance(v_lInsuranceFileID:=gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexInsuranceFileId).Text.Trim()), r_bIsPolicyCancelled:=bIsPolicyCancelled, r_bIsClaimPaymentInitiated:=bIsClaimPaymentInitiated)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GetPolicyStatusForMediaTypeStatusMaintenance method ", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            If bIsPolicyCancelled Then
                                m_lReturn = MessageBox.Show("User is not allowed to change the status from bounced as the associated transaction(s) are cancelled", "Maintain Media Type Status", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                lvwFindReceipts.Items.Item(lReceiptsCount).Checked = False

                                GoTo Continue_Renamed
                            End If
                        End If

                        If StringsHelper.ToDoubleSafe(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId).Text.Trim()) = ACMaintainMediaTypeStatusCleared Then

                            m_lReturn = m_oACTMaintainMediaTypeStatus.GetPolicyStatusForMediaTypeStatusMaintenance(v_lInsuranceFileID:=gPMFunctions.ToSafeLong(lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexInsuranceFileId).Text.Trim()), r_bIsPolicyCancelled:=bIsPolicyCancelled, r_bIsClaimPaymentInitiated:=bIsClaimPaymentInitiated)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "GetPolicyStatusForMediaTypeStatusMaintenance method ", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            If bIsClaimPaymentInitiated Then
                                sInsuranceRef = lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexPolicyNumber).Text
                                m_lReturn = MessageBox.Show("This policy " & sInsuranceRef & " already has a claim payment initiated, do you still wish to proceed?", "Maintain Media Type Status", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                                If m_lReturn = System.Windows.Forms.DialogResult.No Then
                                    lvwFindReceipts.Items.Item(lReceiptsCount).Checked = False

                                    GoTo Continue_Renamed
                                End If
                            End If
                        End If

                        If m_lMediaTypeStatusId = ACMaintainMediaTypeStatusBounced Then
                            m_sInsuranceRef = lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexPolicyNumber).Text
                            m_sClientCode = lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexClientCode).Text

                            m_lReturn = CreateWorkManagerTask()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "CreateWorkManagerTask failed to add the Task", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                        'developer guide no. 63
                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId).Text = m_lMediaTypeStatusId
                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatus).Text = m_sMediaTypeStatus
                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexUpdatedDate).Text = m_dtUpdateDate
                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexComments).Text = m_sComments
                    Else

                        lvwFindReceipts.Items.Item(lReceiptsCount).Checked = False

                    End If

                End If
Continue_Renamed:
            Next lReceiptsCount

        End If

        'Enabling or disabling Ok Button
        lReceiptsCount = 0
        'Start  Arul PN 63413
        '   cmdOK.Enabled = False
        '   For lReceiptsCount = 0 To lvwFindReceipts.ListItems.Count - 1
        '        If lvwFindReceipts.ListItems(lReceiptsCount + 1).Checked = True And ToSafeDate(lvwFindReceipts.ListItems(lReceiptsCount + 1).ListSubItems(kReceiptMaintColHIndexUpdatedDate)) <> 0 Then
        '            cmdOK.Enabled = True
        '            Exit For
        '        End If
        '
        '    Next lReceiptsCount

        m_lReturn = EnableDisableOkButton()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Method EnableDisableOkButton calling failed", gPMConstants.PMELogLevel.PMLogError)
            Exit Sub
        End If

        'End  Arul PN 63413
        GoTo Finally_Renamed


Catch_Renamed:
        ' Log Error Message
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateAllSelected, v_sUsername:=g_sUsername.Value)
Finally_Renamed:
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Exit Sub

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try



            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iACTMaintainMediaTypeStatus.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error.
                gPMFunctions.RaiseError("Form_Initialize", "Failed to get PMUser")
                Exit Sub

            End If

            Dim temp_m_oACTMaintainMediaTypeStatus As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oACTMaintainMediaTypeStatus, "bACTMaintainMediaTypeStatus.Form", vInstanceManager:="ClientManager")
            m_oACTMaintainMediaTypeStatus = temp_m_oACTMaintainMediaTypeStatus

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error.
                gPMFunctions.RaiseError(v_sSource:="Initilize", v_sDescription:="Failed to initialise bACTMaintainMediaTypeStatus.Form")
                Exit Sub
            End If

            m_oFormFields = New iPMFormControl.FormFields()
            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            'developer guide no. 38
            Me.cboDrawnBankName.FirstItem = ""
            Me.cboMediaTypeStatus.FirstItem = ""
            Me.uctBankAccount.FirstItem = ""


        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialize interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initilize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const knMediaTypeCheque As Integer = 5
        ' Forms load event.

        Try



            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error
                gPMFunctions.RaiseError("Form_Load", "Failed to set the status for the business object")
                Exit Sub
            End If


            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If


            m_lReturn = EnableDisableCancelButton()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            cboBranch.Select()


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy) '





            If Not (m_oPMUser Is Nothing) Then


                m_oPMUser.Dispose()
                m_oPMUser = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
        End Try
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        '    ' Click event of the OK button.
        '
        Try
            '


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                Me.Hide()
            End If

            '
        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try



            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="cmdCancel_Click", r_lFunctionReturn:=False, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdFindNow_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFindNow.Click

        ' Click event of the Cancel button.
        Dim bIsValid As Boolean
        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
            End If

            m_lReturn = EnableDisableCancelButton()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Failed in EnableDisableCancel method
            End If
            ' Set the focus.


            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find Now command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFindNow_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    Private Sub cmdNewSearch_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewSearch.Click

        ' Click event of the New Search button.

        Try


            ' Clear the interface details.
            m_lReturn = ClearInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to clear the interface details.
            End If



        Catch ex As Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the new search command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNewSearch_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (lblAmountRangeTo_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub lblAmountRangeTo_Click()
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (lvwFindParty_ColumnClick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub lvwFindParty_ColumnClick(ByVal ColumnHeader As ColumnHeader)
    '
    ' Column click event for the search details
    '
    'On Error GoTo Catch_Renamed
    '
    '
    'If lvwFindReceipts.Items.Count > 0 Then
    'OnColumnClick(lvwFindReceipts, ColumnHeader)
    'End If
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwFindParty_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Finally_Renamed: '
    'Exit Sub
    'Resume 
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (lvwFindParty_ItemClick) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub lvwFindParty_ItemClick(ByVal Item As ListViewItem)
    '
    'm_lReturn = EnableDisableCancelButton()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (lvwFindParty_KeyUp) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub lvwFindParty_KeyUp(ByRef KeyCode As Integer, ByRef Shift As Integer)
    '
    'm_lReturn = EnableDisableCancelButton()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (txtAmountRangeFrom_KeyPress) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtAmountRangeFrom_KeyPress(ByRef KeyAscii As Integer)
    'If KeyAscii <> 8 Then
    'If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 46) Then
    'KeyAscii = 0
    'End If
    'End If
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (txtAmountRangeTo_KeyPress) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtAmountRangeTo_KeyPress(ByRef KeyAscii As Integer)
    'If KeyAscii <> 8 Then
    'If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii <> 46) Then
    'KeyAscii = 0
    'End If
    'End If
    'End Sub



    'UPGRADE_NOTE: (7001) The following declaration (txtBatchReference_KeyPress) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtBatchReference_KeyPress(ByRef KeyAscii As Integer)
    'If KeyAscii <> 8 Then
    'If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) Then
    'KeyAscii = 0
    'End If
    'End If
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (txtClientAccountNumber_KeyPress) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub txtClientAccountNumber_KeyPress(ByRef KeyAscii As Integer)
    'If KeyAscii <> 8 Then
    'If (KeyAscii < 48 Or KeyAscii > 57) And (KeyAscii < 65 Or KeyAscii > 90) And (KeyAscii < 97 Or KeyAscii > 122) Then
    'KeyAscii = 0
    'End If
    'End If
    'End Sub

    'Private Sub txtDateFrom_GotFocus()
    '
    '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateFrom)
    '
    'End Sub
    '
    'Private Sub txtDateFrom_LostFocus()
    '
    '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateFrom)
    '
    'End Sub
    'Private Sub txtDateTo_GotFocus()
    '
    '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateTo)
    '
    'End Sub
    '
    'Private Sub txtDateTo_LostFocus()
    '
    '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateTo)
    '
    'End Sub

    ' ***************************************************************** '
    ' Name: GetBranchDetails
    '
    ' Description: Gets all of the branch details
    '
    ' Updates: taken from PartyPC
    '
    ' ***************************************************************** '

    Private Function GetBranchDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBranchDetails"

        Dim vSourceArray(,) As Object

        Dim lReturn, lDefaultCurrencyId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Only populate combo with addresses the user is authorised to access.

            m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserID, v_bIncludeDeletedSources:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get branch details for the dropdown list")
            End If

            'PN 19428

            m_vSourceArray = vSourceArray

            'Clear combo.
            cboBranch.Items.Clear()

            Dim cboBranch_NewIndex As Integer = -1
            cboBranch_NewIndex = cboBranch.Items.Add("(Any)")
            VB6.SetItemData(cboBranch, cboBranch_NewIndex, 0)

            'Populate branch combo

            'developer guide no.162
            For i As Integer = 0 To vSourceArray.GetUpperBound(1)

                cboBranch_NewIndex = cboBranch.Items.Add(CStr(vSourceArray(2, i)).Trim())
                VB6.SetItemData(cboBranch, cboBranch_NewIndex, CInt(vSourceArray(0, i)))

                If CInt(vSourceArray(0, i)) = m_vBranch Then
                    cboBranch.SelectedIndex = cboBranch_NewIndex
                End If
            Next i
            cboBranch.SelectedIndex = 0



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    'Start Renuka PN 63396 - Removed a parameter
    Private Function SelectParty(ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "") As Integer
        'End Renuka PN 63396

        Dim result As Integer = 0
        Const kMethodName As String = "SelectParty"
        'developer guide no. 108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 108
            oFindParty = New iPMBFindParty.Interface_Renamed

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lReturn = oFindParty.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If
            'developer guide no. 9
            m_lReturn = oFindParty.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            oFindParty.CallingAppName = "uctInvoiceAccount"

            m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            oFindParty.IgnoreDPAQuestions = True

            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Start Renuka PN 63396
                'vPartyCnt = oFindParty.PartyCnt
                'm_lPartyCnt = oFindParty.PartyCnt
                'End Renuka PN 63396
                vShortName = oFindParty.ShortName

                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
            End If

            oFindParty.Dispose()

            oFindParty = Nothing



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function CreateWorkManagerTask() As Integer
        Dim result As Integer = 0
        Dim bPMLookup, iPMWrkTaskInstance As Object


        Const kMethodName As String = "SelectParty"

        Dim oWrkTaskInstance As iPMWrkTaskInstance.NavigatorV3

        Dim oPMLookUp As bPMLookup.Business
        Dim lTaskID, lTaskGroupID As Integer
        Dim vKeys As Object
        Dim sTaskDesc As String = ""
        Dim oTaskInstance As Object
        Dim lReturn, lTaskInstanceCnt As Integer
        Dim g_oACTMaintainMediaTypeStatus As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeys(1, 26)

            ' Change the cursor mode
            iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Object to create work manager tasks
            Dim temp_oWrkTaskInstance As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oWrkTaskInstance, "iPMWrkTaskInstance.NavigatorV3", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oWrkTaskInstance = temp_oWrkTaskInstance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of iPMWrkTaskInstance.NavigatorV3")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set to ADD mode
            m_lReturn = oWrkTaskInstance.NavigatorV3_SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oWrkTaskInstance.NavigatorV3_SetProcessModes Failed")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set the authority level
            ''oWrkTaskInstance.NavigatorV3_PMAuthorityLevel = m_lPMAuthorityLevel&


            ' Create an instance of bPMLookup
            Dim temp_oPMLookUp As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLookUp, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLookUp = temp_oPMLookUp
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bPMLookup.Business")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Set the product family

            oPMLookUp.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting

            ' Use the lookup to get the ID of the PMTMAINT task

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task", v_sCode:="UWCancel", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode for " & Environment.NewLine & _
                                        "TableName: pmwrk_task" & Environment.NewLine & _
                                        "Code: PMTMAINT" & Environment.NewLine & _
                                        "EffectiveDate: " & DateTime.Today)
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Use the lookup to get the ID of the SLACS task group

            m_lReturn = oPMLookUp.GetEffectiveIDFromCode(v_sTableName:="pmwrk_task_group", v_sCode:="UNDER", v_dtEffectiveDate:=DateTime.Now, r_lID:=lTaskGroupID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to GetEffectiveIDFromCode for " & Environment.NewLine & _
                                        "TableName: pmwrk_task_group" & Environment.NewLine & _
                                        "Code: SLACS" & Environment.NewLine & _
                                        "EffectiveDate: " & DateTime.Today)
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Remove instance of lookup

            oPMLookUp.Dispose()
            oPMLookUp = Nothing




            m_lReturn = m_oACTMaintainMediaTypeStatus.CreateTask(v_lTaskGroupId:=lTaskGroupID, v_lTaskId:=lTaskID, v_sCustomer:="Customer", v_lUserGroupId:=6, v_sDescription:="Please Cancel the Policy" & " " & m_sInsuranceRef, v_iIsVisible:=1)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to get an instance of bSirProduct.Business")
                ' Change the cursor mode
                iPMFunc.SetMousePointer(iMouseState:=gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally
            ' Terminate the object
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            oWrkTaskInstance.Dispose()
        End Try
        Return result
    End Function


    Private Function EnableDisableCancelButton() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisableCancelButton"

        Try
            Dim vOptionValue As String = ""


            result = gPMConstants.PMEReturnCode.PMTrue
            cmdUpdateAllSelected.Enabled = False
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=vOptionValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Form_load", "Failed to get the value for SIROPTEnableRI2007.", gPMConstants.PMELogLevel.PMLogError)
            End If

            If StringsHelper.ToDoubleSafe(vOptionValue.Trim()) = 1 Then
                txtCollectionDateFrom.Enabled = True
                txtCollectionDateFrom.BackColor = Color.White
                txtCollectionDateTo.Enabled = True
                txtCollectionDateTo.BackColor = Color.White
            Else
                txtCollectionDateFrom.Enabled = False
                txtCollectionDateFrom.BackColor = SystemColors.Control
                txtCollectionDateTo.Enabled = False
                txtCollectionDateTo.BackColor = SystemColors.Control

            End If

            cmdSelectAll.Enabled = lvwFindReceipts.Items.Count > 0
            'Start Arul PN 63413
            m_lReturn = EnableDisableOkButton()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method EnableDisableOkButton calling failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            'End Arul PN 63413


        Catch ex As Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally




        End Try
        Return result
    End Function
    'lvwFindReceipts_ItemChecked event is called instead of lvwFindReceipts_Click as it gets fired on every click which is not required
    '	Private Sub lvwFindReceipts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwFindReceipts.Click

    '        Const kMethodName As String = "lvwFindReceipts_Click"
    '        On Error GoTo Catch_Renamed
    '        Dim bCheckNumberOfRecord As Boolean


    '        cmdUpdateAllSelected.Enabled = False
    '        cmdOk.Enabled = False
    '        For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
    '            bCheckNumberOfRecord = True
    '            If lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then
    '                'If lvwFindReceipts.Items.Item(lReceiptsCount).Focused Then
    '                cmdUpdateAllSelected.Enabled = True
    '                Exit For
    '            End If
    '        Next
    '        For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
    '            If Not lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then
    '                'If Not lvwFindReceipts.Items.Item(lReceiptsCount).Focused Then

    '                For lRow As Integer = m_vOriginalArray.GetLowerBound(1) To m_vOriginalArray.GetUpperBound(1)
    '                    If lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexCashlistitemId - 1).Text.Trim() = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CASHLISTITEM_ID, lRow)).Trim() Then

    '                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatus - 1).Text = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS, lRow))
    '                        'Start Arul PN 63413
    '                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId - 1).Text = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS_ID, lRow))
    '                        'End Arul PN 63413
    '                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexUpdatedDate - 1).Text = ""
    '                        lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexComments - 1).Text = ""
    '                    End If
    '                Next lRow
    '            End If

    '        Next lReceiptsCount
    '        'Start  Arul PN 63413
    '        cmdSelectAll.Enabled = bCheckNumberOfRecord


    '        m_lReturn = EnableDisableOkButton()
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            gPMFunctions.RaiseError(kMethodName, "Method EnableDisableOkButton calling failed", gPMConstants.PMELogLevel.PMLogError)
    '            Exit Sub
    '        End If
    '        'Start  Arul PN 63413
    '        GoTo Finally_Renamed

    'Catch_Renamed:

    '        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value)

    'Finally_Renamed:
    '        Exit Sub

    '    End Sub

    Private Sub lvwFindReceipts_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwFindReceipts.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwFindReceipts.Columns(eventArgs.Column)

        Const kMethodName As String = "lvwFindReceipts_ColumnClick"
        Try


            If lvwFindReceipts.Items.Count > 0 Then
                OnColumnClick(lvwFindReceipts, ColumnHeader)
            End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally



        End Try
    End Sub





    Private isInitializingComponent As Boolean
    Private Sub txtCollectionDateFrom_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDateFrom.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If Information.IsDate(txtCollectionDateFrom.Text.Trim()) Then
            m_dtCollectionDateFrom = CDate(StringsHelper.Format(txtCollectionDateFrom.Text.Trim(), ACDateDispaly))
            txtCollectionDateFrom.Tag = CStr(True)
        End If
    End Sub

    Private Sub txtCollectionDateFrom_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDateFrom.Enter
        If Information.IsDate(txtCollectionDateFrom.Text) Then
            txtCollectionDateFrom.Text = StringsHelper.Format(txtCollectionDateFrom.Text, ACDateDispaly)
        End If
    End Sub

    Private Sub txtCollectionDateFrom_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDateFrom.Leave
        If (Not Information.IsDate(txtCollectionDateFrom.Text)) And txtCollectionDateFrom.Text <> "" Then
            txtCollectionDateFrom.Text = StringsHelper.Format(DateTime.Now, ACDateDispaly)
        Else
            txtCollectionDateFrom.Text = StringsHelper.Format(txtCollectionDateFrom.Text, ACDateDispaly)
        End If
    End Sub

    Private Sub txtCollectionDateTo_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDateTo.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If Information.IsDate(txtCollectionDateTo.Text.Trim()) Then
            txtCollectionDateTo.Text = StringsHelper.Format(txtCollectionDateTo.Text.Trim(), ACDateDispaly)
            txtCollectionDateTo.Tag = CStr(True)
        End If
    End Sub

    Private Sub txtCollectionDateTo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDateTo.Enter
        If Information.IsDate(txtCollectionDateTo.Text) Then
            txtCollectionDateTo.Text = StringsHelper.Format(txtCollectionDateTo.Text, ACDateDispaly)
        End If
    End Sub

    Private Sub txtCollectionDateTo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCollectionDateTo.Leave
        If (Not Information.IsDate(txtCollectionDateTo.Text)) And txtCollectionDateTo.Text <> "" Then
            txtCollectionDateTo.Text = StringsHelper.Format(DateTime.Now, ACDateDispaly)
        Else
            txtCollectionDateTo.Text = StringsHelper.Format(txtCollectionDateTo.Text, ACDateDispaly)
        End If
    End Sub
    'Start  Arul PN 63413
    Private Function EnableDisableOkButton() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableDisableOkButton"
        Try
            Dim bEnableDisableButton As Boolean


            result = gPMConstants.PMEReturnCode.PMTrue
            If lvwFindReceipts.Items.Count > 0 Then
                For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
                    If lvwFindReceipts.Items.Item(lReceiptsCount).Index = -1 Then
                        Exit For
                    End If
                    If lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then

                        For lRow As Integer = m_vOriginalArray.GetLowerBound(1) To m_vOriginalArray.GetUpperBound(1)
                            If lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexCashlistitemId).Text.Trim() = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CASHLISTITEM_ID, lRow)).Trim() Then
                                If lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId).Text.Trim() <> CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS_ID, lRow)).Trim() Then

                                    bEnableDisableButton = True
                                    Exit For
                                End If
                            End If
                        Next lRow
                    End If
                    If bEnableDisableButton Then
                        Exit For
                    End If

                Next lReceiptsCount
            Else
                bEnableDisableButton = False
            End If
            cmdOk.Enabled = bEnableDisableButton


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    Private Sub lvwFindReceipts_ItemChecked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles lvwFindReceipts.ItemChecked
        Const kMethodName As String = "lvwFindReceipts_Click"
        Try
            Dim bCheckNumberOfRecord As Boolean


            cmdUpdateAllSelected.Enabled = False
            cmdOk.Enabled = False

            For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
                bCheckNumberOfRecord = True
                If b_RunChecked = True Then
                    Exit For
                End If
                If lvwFindReceipts.Items.Item(lReceiptsCount).Index = -1 Then
                    Exit Sub
                End If
                If lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then
                    cmdUpdateAllSelected.Enabled = True
                    Exit For
                End If
            Next
            For lReceiptsCount As Integer = 0 To lvwFindReceipts.Items.Count - 1
                If lvwFindReceipts.Items.Item(lReceiptsCount).Index = -1 Then
                    Exit Sub
                End If
                If Not lvwFindReceipts.Items.Item(lReceiptsCount).Checked Then
                    If lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Count = kReceiptMaintColHIndexCashlistitemId Then
                        For lRow As Integer = m_vOriginalArray.GetLowerBound(1) To m_vOriginalArray.GetUpperBound(1)

                            If lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexCashlistitemId - 1).Text.Trim() = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSF_CASHLISTITEM_ID, lRow)).Trim() Then

                                lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatus - 1).Text = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS, lRow))
                                'Start Arul PN 63413
                                lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexMediaTypeStatusId - 1).Text = CStr(m_vSearchData(MainModule.enuSelMediaTypeStatusFields.enuSelMTSV_MEDIATYPESTATUS_ID, lRow))
                                'End Arul PN 63413
                                lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexUpdatedDate - 1).Text = ""
                                lvwFindReceipts.Items.Item(lReceiptsCount).SubItems.Item(kReceiptMaintColHIndexComments - 1).Text = ""
                            End If
                        Next lRow
                    End If
                Else
                    Exit For
                End If
            Next lReceiptsCount
            'Start  Arul PN 63413
            cmdSelectAll.Enabled = bCheckNumberOfRecord


            m_lReturn = EnableDisableOkButton()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Method EnableDisableOkButton calling failed", gPMConstants.PMELogLevel.PMLogError)
                Exit Sub
            End If
            'Start  Arul PN 63413


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally

        End Try
    End Sub


End Class
