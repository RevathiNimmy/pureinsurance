Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    ' Date: 09/06/1999
    ' Description: Main interface.
    ' Edit History:
    ' ***************************************************************** '
    'Developer Guide No.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lRiskTypeId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lItemsFound As Integer

    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction
    Private m_lRIModelUsageCnt As Integer
    Private m_lRIModelId As Integer
    Private m_lPortfolioRIModelId As Integer
    Private m_sDesc As String = ""
    Private m_lRIBand As Integer
    Private m_sRIBand As String = ""
    Private m_sRIDescription As String = ""
    Private m_iIsDeleted As Integer
    Private m_dtRIEffectiveDate As Date
    Private m_dtRIExpiryDate As Object
    'JMK 23/10/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""
    ' AMB 28/05/2003: 1.8.6 Deferred RI RFC
    Private m_lIsDeferred As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURIModelUsage.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRRIModelUsage.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_vRIModelUsage(,) As Object
    ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - store list of ri_model's, deferred or not
    Private m_vRIModelIsDeferred(,) As Object
    Private m_vPortfolioRIModel As Object
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property


    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    ' Set the interface exit status.
    'm_lStatus = Value
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property
    Public WriteOnly Property IsDeferred() As Integer
        Set(ByVal Value As Integer)
            m_lIsDeferred = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    ' Description: Sets the rules for validating fields.
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRIModel, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRIBand, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtExpiryDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DataToDetail
    ' Description: Populate Treaty Party Details storage.
    ' ***************************************************************** '
    Private Function DataToDetail() As Integer

        Dim result As Integer = 0
        Dim lSelectedItem As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if an item is selected
            If lvwRIModelUsage.FocusedItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            lSelectedItem = Convert.ToString(lvwRIModelUsage.Items.Item(lvwRIModelUsage.FocusedItem.Index).Tag)

            ' Store id (needed for replaces filtering)
            m_lRIModelUsageCnt = CInt(m_vRIModelUsage(ACRRiskTypeRIModelUsageCnt, lSelectedItem))

            ' Update the property members.
            m_lRIModelId = CInt(m_vRIModelUsage(ACRRIModelId, lSelectedItem))
            If CStr(m_vRIModelUsage(ACRTransferFromModelID, lSelectedItem)) = "" Then
                m_lPortfolioRIModelId = 0
            Else
                m_lPortfolioRIModelId = CInt(m_vRIModelUsage(ACRTransferFromModelID, lSelectedItem))
            End If

            ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - set the combo item
            For lComboItem As Integer = 0 To cboRIModel.Items.Count - 1
                If VB6.GetItemData(cboRIModel, lComboItem) = CInt(m_vRIModelUsage(ACRRIModelId, lSelectedItem)) Then

                    cboRIModel.SelectedIndex = lComboItem
                    Exit For

                End If
            Next lComboItem

            m_lRIBand = CInt(m_vRIModelUsage(ACRRIModelBand, lSelectedItem))
            m_sRIBand = CStr(m_vRIModelUsage(ACRRIBandDescription, lSelectedItem))

            cboRIBand.ItemId = m_lRIBand

            m_sRIDescription = CStr(m_vRIModelUsage(ACRDescription, lSelectedItem))

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sRIDescription)

            m_iIsDeleted = CInt(m_vRIModelUsage(ACRIsDeleted, lSelectedItem))

            If m_iIsDeleted = 1 Then
                chkIsDeleted.CheckState = CheckState.Checked
            Else
                chkIsDeleted.CheckState = CheckState.Unchecked
            End If

            m_dtRIEffectiveDate = CDate(m_vRIModelUsage(ACREffectiveDate, lSelectedItem))

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtRIEffectiveDate)


            m_dtRIExpiryDate = m_vRIModelUsage(ACRExpiryDate, lSelectedItem)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtExpiryDate, vControlValue:=m_dtRIExpiryDate)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ClearDetail
    ' Description: Clear Treaty Party Details storage.
    ' ***************************************************************** '
    Private Function ClearDetail() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the Detail details.

            m_lRIModelId = 0
            m_lPortfolioRIModelId = 0
            m_sDesc = ""
            ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - select none
            cboRIModel.SelectedIndex = -1
            cboPortfolioRIModel.SelectedIndex = -1
            m_lRIBand = 0
            m_sRIDescription = ""
            m_iIsDeleted = 0
            m_dtRIEffectiveDate = DateTime.Today

            m_dtRIExpiryDate = ""

            cboRIBand.ItemId = m_lRIBand

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sRIDescription)

            chkIsDeleted.CheckState = CheckState.Unchecked

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtRIEffectiveDate)

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtExpiryDate, vControlValue:=m_dtRIExpiryDate)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ''' <summary>
    ''' Populate RI Model Usage Refreshs storage
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataRefresh() As Integer

        Dim nResult As Integer
        Dim nSelectedItem As Integer
        Dim nRIModelId As Integer
        Dim nPortfolioRIModelId As Integer
        Dim nItemStatus As Integer
        Dim bExists As Boolean
        Dim bExists1 As Boolean
        Dim bDBCheck As Boolean

        Try
            nResult = PMEReturnCode.PMTrue

            ' Update the property members.

            ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development -
            ' replaced PMLookup combo with normal combo
            If cboRIModel.SelectedIndex <> -1 Then
                m_lRIModelId = VB6.GetItemData(cboRIModel, cboRIModel.SelectedIndex)
            End If
            If cboPortfolioRIModel.SelectedIndex <> -1 Then
                m_lPortfolioRIModelId = VB6.GetItemData(cboPortfolioRIModel, cboPortfolioRIModel.SelectedIndex)
            Else
                m_lPortfolioRIModelId = 0
            End If
            m_sDesc = cboRIModel.Text

            m_lRIBand = cboRIBand.ItemId
            m_sRIBand = cboRIBand.ItemCaption

            m_sRIDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))

            m_iIsDeleted = chkIsDeleted.CheckState

            m_dtRIEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))

            m_dtRIExpiryDate = m_oFormFields.UnformatControl(ctlControl:=txtExpiryDate)

            If IsArray(m_vRIModelUsage) Then
                If m_iAction = PMEComponentAction.PMAdd Then
                    For ncount As Integer = m_vRIModelUsage.GetLowerBound(1) To m_vRIModelUsage.GetUpperBound(1)
                        If ToSafeInteger(m_vRIModelUsage(ACRRIModelBand, ncount)) = m_lRIBand AndAlso ToSafeInteger(m_vRIModelUsage(ACRRIModelId, ncount)) = m_lRIModelId AndAlso CDate(m_vRIModelUsage(ACREffectiveDate, ncount)) = m_dtRIEffectiveDate Then
                            If ToSafeInteger(m_vRIModelUsage(ACRIsDeleted, ncount)) <> m_iIsDeleted Then
                                bDBCheck = True
                            Else
                                bExists1 = True
                            End If
                            Exit For
                        End If
                        If ToSafeInteger(m_vRIModelUsage(ACRRIModelBand, ncount)) = m_lRIBand AndAlso ToSafeInteger(m_vRIModelUsage(ACRItemStatus, ncount)) = 3 AndAlso _
                                CDate(m_vRIModelUsage(ACREffectiveDate, ncount)) = m_dtRIEffectiveDate AndAlso CDate(m_vRIModelUsage(ACRExpiryDate, ncount)) = m_dtRIExpiryDate Then
                            bDBCheck = True
                            Exit For
                        End If

                    Next ncount
                ElseIf m_iAction = PMEComponentAction.PMEdit Then
                    For icount As Integer = m_vRIModelUsage.GetLowerBound(1) To m_vRIModelUsage.GetUpperBound(1)
                        If icount <> Convert.ToString(lvwRIModelUsage.Items.Item(lvwRIModelUsage.FocusedItem.Index).Tag) Then
                            If ToSafeInteger(m_vRIModelUsage(ACRRIModelBand, icount)) = m_lRIBand AndAlso ToSafeInteger(m_vRIModelUsage(ACRRIModelId, icount)) = m_lRIModelId AndAlso CDate(m_vRIModelUsage(ACREffectiveDate, icount)) = m_dtRIEffectiveDate Then
                                If CInt(m_vRIModelUsage(ACRIsDeleted, icount)) <> m_iIsDeleted Then
                                    bDBCheck = True
                                Else
                                    bExists1 = True
                                End If
                                Exit For
                            End If
                        End If
                    Next icount
                End If
            End If

            'Check for duplicates in Db
            If m_iAction = PMEComponentAction.PMAdd Then
                If Not bDBCheck Then
                    m_lReturn = m_oBusiness.ValidateRIModelUsage(m_lRiskTypeId, m_lRIBand, m_lRIModelId, m_dtRIEffectiveDate, bExists)
                End If
            ElseIf m_iAction = PMEComponentAction.PMEdit Then

                nSelectedItem = Convert.ToString(lvwRIModelUsage.Items.Item(lvwRIModelUsage.FocusedItem.Index).Tag)
                If CInt(m_vRIModelUsage(ACRRIModelBand, nSelectedItem)) <> m_lRIBand AndAlso ToSafeInteger(m_vRIModelUsage(ACRRIModelId, nSelectedItem)) <> m_lRIModelId AndAlso CDate(m_vRIModelUsage(ACREffectiveDate, nSelectedItem)) <> m_dtRIEffectiveDate Then

                    m_lReturn = m_oBusiness.ValidateRIModelUsage(m_lRiskTypeId, m_lRIBand, m_lRIModelId, m_dtRIEffectiveDate, bExists)
                End If
            End If

            If bExists1 OrElse bExists Then
                MessageBox.Show("Duplicate Usage Record created. Please correct your values and try again.", "Reinsurance Model", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            End If

            Select Case m_iAction
                Case PMEComponentAction.PMAdd

                    If Information.IsArray(m_vRIModelUsage) Then
                        nSelectedItem = m_vRIModelUsage.GetUpperBound(1) + 1
                        ReDim Preserve m_vRIModelUsage(ACRMax, nSelectedItem)
                    Else
                        nSelectedItem = 0
                        ReDim m_vRIModelUsage(ACRMax, nSelectedItem)
                    End If

                    nRIModelId = m_lRIModelId
                    nPortfolioRIModelId = m_lPortfolioRIModelId
                    nItemStatus = ACItemStatus_Added
                Case PMEComponentAction.PMEdit

                    nSelectedItem = Convert.ToString(lvwRIModelUsage.Items.Item(lvwRIModelUsage.FocusedItem.Index).Tag)

                    nRIModelId = m_lRIModelId
                    nPortfolioRIModelId = m_lPortfolioRIModelId
                    If ToSafeInteger(m_vRIModelUsage(ACRItemStatus, nSelectedItem)) <> ACItemStatus_Added Then
                        nItemStatus = ACItemStatus_Changed
                    Else
                        nItemStatus = ACItemStatus_Added
                    End If

                Case PMEComponentAction.PMDelete

                    nSelectedItem = Convert.ToString(lvwRIModelUsage.Items.Item(lvwRIModelUsage.FocusedItem.Index).Tag)

                    nItemStatus = ACItemStatus_Deleted

            End Select

            m_vRIModelUsage(ACRRIModelBand, nSelectedItem) = m_lRIBand
            m_vRIModelUsage(ACRRIBandDescription, nSelectedItem) = m_sRIBand
            m_vRIModelUsage(ACRRIModelId, nSelectedItem) = nRIModelId
            m_vRIModelUsage(ACRTransferFromModelID, nSelectedItem) = nPortfolioRIModelId
            m_vRIModelUsage(ACRDescription, nSelectedItem) = m_sRIDescription
            m_vRIModelUsage(ACRRIModelDescription, nSelectedItem) = m_sDesc
            m_vRIModelUsage(ACRIsDeleted, nSelectedItem) = m_iIsDeleted
            m_vRIModelUsage(ACREffectiveDate, nSelectedItem) = m_dtRIEffectiveDate

            m_vRIModelUsage(ACRExpiryDate, nSelectedItem) = m_dtRIExpiryDate
            m_vRIModelUsage(ACRItemStatus, nSelectedItem) = nItemStatus

            m_lReturn = BusinessToInterface()

            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the refresh from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            'Tell it what we're getting

            m_oBusiness.RiskTypeId = m_lRiskTypeId

            ' AMB 28/05/2003: 1.8.6 Deferred RI RFC - m_lIsDeferred added

            m_lReturn = m_oBusiness.GetRIModelUsage(r_vRIModelUsage:=m_vRIModelUsage, v_lIsDeferred:=m_lIsDeferred)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            ' get list of ri_models for cboRIModel

            m_lReturn = m_oBusiness.GetRIModelIsDeferred(r_vRIModelIsDeferred:=m_vRIModelIsDeferred, v_lIsDeferred:=m_lIsDeferred)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return result
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim sTemp As String = ""

        Const ACFindImage As String = "FindImage"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the interface.
            lblModelUsageValue.Text = m_sDescription

            ' Populate listview
            lvwRIModelUsage.Items.Clear()
            If Information.IsArray(m_vRIModelUsage) Then
                For lTemp As Integer = m_vRIModelUsage.GetLowerBound(1) To m_vRIModelUsage.GetUpperBound(1)
                    If CDbl(m_vRIModelUsage(ACRRIModelId, lTemp)) <> 0 Then

                        'Developer Guide No.49
                        oListItem = lvwRIModelUsage.Items.Add(CStr(m_vRIModelUsage(ACRRIModelDescription, lTemp)), ACFindImage)

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRIModelUsage(ACRRIBandDescription, lTemp))

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vRIModelUsage(ACRDescription, lTemp))

                        If CDbl(m_vRIModelUsage(ACRIsDeleted, lTemp)) = 1 Then
                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Yes"
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "No"
                        End If

                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_vRIModelUsage(ACREffectiveDate, lTemp))
                        'Developer Guide No. (As per VB Format)
                        If Information.IsDate(txtEffectiveDate.Text) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = Convert.ToDateTime(txtEffectiveDate.Text).ToString("dd MMMM yyyy")
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = txtEffectiveDate.Text
                        End If

                        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtExpiryDate, vControlValue:=m_vRIModelUsage(ACRExpiryDate, lTemp))
                        'Developer Guide No. (As per VB Format)
                        If Information.IsDate(txtExpiryDate.Text) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = Convert.ToDateTime(txtExpiryDate.Text).ToString("dd MMMM yyyy")
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = txtExpiryDate.Text
                        End If

                        m_lReturn = GetModelDesc(m_vRIModelUsage(ACRTransferFromModelID, lTemp), sTemp)
                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sTemp

                        oListItem.Tag = CStr(lTemp)
                    End If
                Next lTemp

            End If

            If Information.IsArray(m_vRIModelIsDeferred) Then
                ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - load up the cboRIModel combo
                cboRIModel.Items.Clear()
                For lTemp As Integer = m_vRIModelIsDeferred.GetLowerBound(1) To m_vRIModelIsDeferred.GetUpperBound(1)
                    Dim cboRIModel_NewIndex As Integer = -1
                    cboRIModel_NewIndex = cboRIModel.Items.Add(gPMFunctions.NullToString(m_vRIModelIsDeferred(MainModule.enumRIModelData.edDesc, lTemp)))
                    VB6.SetItemData(cboRIModel, cboRIModel_NewIndex, gPMFunctions.NullToLong(m_vRIModelIsDeferred(MainModule.enumRIModelData.edRIModelID, lTemp)))
                Next lTemp
            End If

            If lvwRIModelUsage.Items.Count = 0 Then
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_oBusiness.RiskTypeId = m_lRiskTypeId

            ' If the array is empty, it means there is nothing to save.
            If Not Information.IsArray(m_vRIModelUsage) Then
                ' Exit without raising an error
                Return result
            End If

            ' AMB 29/05/2003: 1.8.6 Deferred RI RFC - added 'is_deferred'
            If m_lIsDeferred <> 1 Then
                m_sScreenHierarchy = $"Risk Type({m_sDescription.Trim()})/Model"
            Else
                m_sScreenHierarchy = $"Risk Type({m_sDescription.Trim()})/Deferred Model"
            End If

            m_lReturn = m_oBusiness.Update(v_vRIModelUsage:=m_vRIModelUsage, v_visdeferred:=m_lIsDeferred, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            tabDetailTab.Visible = False
            tabDetailTab.Top = VB6.TwipsToPixelsY(120)
            tabDetailTab.Left = VB6.TwipsToPixelsX(120)
            tabMainTab.Top = VB6.TwipsToPixelsY(120)
            tabMainTab.Left = VB6.TwipsToPixelsX(120)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwRIModelUsage.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwRIModelUsage.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwRIModelUsage.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwRIModelUsage.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(900))
            lvwRIModelUsage.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwRIModelUsage.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwRIModelUsage.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(2000))

            cmdEdit.Enabled = False

            cmdDelete.Enabled = False

            cmdAdd.Enabled = Not (Task = gPMConstants.PMEComponentAction.PMView)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    ' Description: Display all language specific captions.
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            If m_sUnderwritingType = "1" Then
                Me.Text = "Insurance Model Usage"
            End If

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_sUnderwritingType = "1" Then

                tabMainTab.Name = "&1 - Insurance Model Usage"

                lblRIBand.Text = "Insurance Band:"

                lblRIModel.Text = "Insurance Model:"

            End If


            SSTabHelper.SetTabCaption(tabDetailTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblModelUsage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCRiskType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRIModelUsage.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRIModelUsage.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHIsDeleted, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRIModelUsage.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRIModelUsage.Columns.Item(5).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHExpiryDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwRIModelUsage.Columns.Item(6).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHRIPortfolio, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkIsDeleted.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCIsDeleted, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboRIModel_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRIModel.Leave

        m_lReturn = FillPortfolioList()

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_iAction = gPMConstants.PMEComponentAction.PMAdd

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True

        cboRIModel.Enabled = True
        cboPortfolioRIModel.Enabled = True

        m_lReturn = ClearDetail()

        m_lReturn = FillPortfolioList()

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim lSelectedItem As Integer
        Dim sMessage As String = ""

        Try

            ' Select item in array

            lSelectedItem = Convert.ToString(lvwRIModelUsage.Items.Item(lvwRIModelUsage.FocusedItem.Index).Tag)

            ' First we need to check if this item is not referrenced by another one
            ' i.e. this model is not replaced by another model
            For lIndex As Integer = m_vRIModelUsage.GetLowerBound(1) To m_vRIModelUsage.GetUpperBound(1)
                If CStr(m_vRIModelUsage(ACRTransferFromModelID, lIndex)) = CStr(m_vRIModelUsage(ACRRiskTypeRIModelUsageCnt, lSelectedItem)) Then
                    ' Warn user he can't delete this item

                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCantDeleteModel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                    MessageBox.Show(sMessage, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                End If
            Next lIndex

            ' Mark item as deleted in array
            m_vRIModelUsage(ACRIsDeleted, lSelectedItem) = 1
            m_vRIModelUsage(ACRItemStatus, lSelectedItem) = ACItemStatus_Deleted

            ' Reenable buttons
            cmdCancel.Enabled = True
            cmdAdd.Enabled = True
            cmdOK.Enabled = True
            cmdDelete.Enabled = False
            cmdEdit.Enabled = False

            ' Refresh list
            m_iAction = gPMConstants.PMEComponentAction.PMDelete
            m_lReturn = DataRefresh()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDelete_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        Dim sError, sTitle As String

        Try

            ' Check mandatory field
            If cboRIModel.SelectedIndex = -1 Then


                sError = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryField, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHRIModel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                MessageBox.Show(sError, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                If cboRIModel.Enabled Then
                    cboRIModel.Focus()
                End If

                Exit Sub
            End If

            If txtExpiryDate.Text <> "" And txtEffectiveDate.Text <> "" Then
                If CDate(txtEffectiveDate.Text) > CDate(txtExpiryDate.Text) Then
                    MessageBox.Show("Expiry Date should be greater than Effective Date.", "Reinsurance Model", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    txtExpiryDate.Focus()
                    Exit Sub
                End If
            End If

            ' Update interface
            tabDetailTab.Visible = False
            tabMainTab.Visible = True
            cmdCancel.Enabled = True
            cmdAdd.Enabled = True
            cmdOK.Enabled = True

            ' Refresh the list
            m_lReturn = DataRefresh()

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDetailOK_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDetailOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Try

            m_iAction = gPMConstants.PMEComponentAction.PMEdit

            ' Update interface
            cmdOK.Enabled = False
            cmdCancel.Enabled = False
            tabMainTab.Visible = False
            tabDetailTab.Visible = True
            cboRIModel.Enabled = False
            cboPortfolioRIModel.Enabled = True

            ' Copy data from array to global variables/screen
            m_lReturn = DataToDetail()

            ' Fill portfolio list
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = FillPortfolioList()
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEdit_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRIModelUsage.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURIModelUsage.General()

            ' Call the initialise RIBand passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub


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

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.

            m_oBusiness.RiskTypeId = m_lRiskTypeId

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = iPMFunc.GetSystemOption(5005, m_sUnderwritingType)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

            'Added the below line without which the lookup control will not populate.
            'start
            cboRIBand.FirstItem = ""
            'end

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




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
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide No.7
                    eventArgs.Cancel = True
                    Cancel = 1

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

            ' Terminate the business object

            m_oBusiness.Dispose()


            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()


            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisableInterface
    '
    ' Description: Disables parts of the interface while a search is
    '              in progress.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisableInterface) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisableInterface(ByRef bDisable As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'cmdOK.Enabled = Not bDisable
    'cmdEdit.Enabled = Not bDisable
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error Section.
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to disable the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer
        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabDetailTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetTabCount(tabDetailTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabDetailTab) < (SSTabHelper.GetTabCount(tabDetailTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabDetailTab, SSTabHelper.GetSelectedIndex(tabDetailTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                        End If
                End Select
            End With
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                If tabMainTab.Visible Then
                    tabMainTab.SelectedIndex = 0
                End If

            End If

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                If tabDetailTab.Visible Then
                    tabDetailTab.SelectedIndex = 0
                End If
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Dim nCount As Integer
        Dim bExists1 As Boolean
        Dim iRow As Integer
        Dim nRIBand As Integer
        Dim dtRIEffectiveDate As Date
        Dim nIsDeleted As Integer
        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            If Information.IsArray(m_vRIModelUsage) Then
                For iRow = LBound(m_vRIModelUsage, 2) To UBound(m_vRIModelUsage, 2)
                    nRIBand = m_vRIModelUsage(ACRRIModelBand, iRow)
                    dtRIEffectiveDate = m_vRIModelUsage(ACREffectiveDate, iRow)
                    nIsDeleted = m_vRIModelUsage(ACRIsDeleted, iRow)
                    If m_vRIModelUsage(ACRItemStatus, iRow) <> ACItemStatus_Deleted Then
                        For nCount = LBound(m_vRIModelUsage, 2) To UBound(m_vRIModelUsage, 2)
                            If iRow <> nCount Then
                                If m_vRIModelUsage(ACRRIModelBand, nCount) = nRIBand And _
                                            m_vRIModelUsage(ACREffectiveDate, nCount) = dtRIEffectiveDate And _
                                                m_vRIModelUsage(ACRIsDeleted, nCount) = nIsDeleted Then
                                    If m_vRIModelUsage(ACRItemStatus, nCount) = ACItemStatus_Deleted Then
                                        bExists1 = False
                                    Else
                                        bExists1 = True
                                    End If
                                    Exit For
                                End If
                            End If
                        Next nCount
                    End If
                    If bExists1 = True Then
                        Exit For
                    End If
                Next iRow

            End If

            If bExists1 Then
                MessageBox.Show("RI Arrangement Already Exists", "RI Model Usage", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




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

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwRIModelUsage_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRIModelUsage.DoubleClick

        If cmdEdit.Enabled Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If

    End Sub

    Private Sub lvwRIModelUsage_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRIModelUsage.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwRIModelUsage.GetItemAt(x, y) Is Nothing Then
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If

    End Sub

    Private Sub txtDescription_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDescription)

    End Sub

    Private Sub txtDescription_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDescription)

    End Sub

    Private Sub txtEffectiveDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtEffectiveDate)

    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
        m_lReturn = FillPortfolioList()

    End Sub

    Private Sub txtExpiryDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpiryDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtExpiryDate)

    End Sub

    Private Sub txtExpiryDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpiryDate.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtExpiryDate)
        m_lReturn = FillPortfolioList()

    End Sub


    ' ********************************************************************
    ' Name:         FillPortfolioList
    ' Description:  Fill the "transfer from" list with all matching models
    ' ********************************************************************
    Private Function FillPortfolioList() As Integer

        Dim result As Integer = 0
        Dim bFound, bExpiryDateIsOK, bEffectiveDateIsOK As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Empty the list
            cboPortfolioRIModel.Items.Clear()

            ' Check we have all the fields we need
            If cboRIModel.SelectedIndex = -1 Or cboRIBand.ItemCaption.Trim() = "" Or txtEffectiveDate.Text.Trim() = "" Then
                Return result
            End If

            ' Add blank item
            Dim cboPortfolioRIModel_NewIndex As Integer = -1
            cboPortfolioRIModel_NewIndex = cboPortfolioRIModel.Items.Add("")
            VB6.SetItemData(cboPortfolioRIModel, cboPortfolioRIModel_NewIndex, 0)

            ' Fill it with appropriate items
            If Information.IsArray(m_vRIModelUsage) Then
                For lRow As Integer = m_vRIModelUsage.GetLowerBound(1) To m_vRIModelUsage.GetUpperBound(1)

                    ' Check expiry date beforehand, because the main "if" statement underneath is complicated enough
                    If Not Information.IsDate(txtExpiryDate.Text) Or Not Information.IsDate(m_vRIModelUsage(ACREffectiveDate, lRow)) Then
                        ' If no date or invalid one, we assume it's okay
                        bExpiryDateIsOK = True
                    Else
                        ' If valid expiry date, make sure the effective date of this model < expiry date
                        bExpiryDateIsOK = CDate(m_vRIModelUsage(ACREffectiveDate, lRow)) <= CDate(txtExpiryDate.Text)
                    End If

                    ' Check effective date beforehand, because the main "if" statement underneath is complicated enough
                    If CStr(m_vRIModelUsage(ACRExpiryDate, lRow)) = "29/12/1899" Or Not Information.IsDate(m_vRIModelUsage(ACRExpiryDate, lRow)) Or Not Information.IsDate(txtEffectiveDate.Text) Then
                        ' If no date or invalid one, we assume it's okay
                        bEffectiveDateIsOK = True
                    Else
                        ' If valid expiry date, make sure the expiry date of this model > expiry date
                        bEffectiveDateIsOK = CDate(m_vRIModelUsage(ACRExpiryDate, lRow)) <= CDate(txtEffectiveDate.Text)
                    End If

                    ' Check of this model is valid
                    If CStr(cboRIBand.ItemId).Trim() = CStr(m_vRIModelUsage(ACRRIModelBand, lRow)) And CStr(m_vRIModelUsage(ACRRiskTypeRIModelUsageCnt, lRow)) <> "" And bEffectiveDateIsOK And bExpiryDateIsOK And CStr(m_vRIModelUsage(ACRIsDeleted, lRow)) = "0" And CDbl(m_vRIModelUsage(ACRRiskTypeRIModelUsageCnt, lRow)) <> m_lRIModelUsageCnt Then

                        ' Add to list
                        cboPortfolioRIModel_NewIndex = cboPortfolioRIModel.Items.Add(gPMFunctions.NullToString(m_vRIModelUsage(ACRRIModelDescription, lRow)))
                        VB6.SetItemData(cboPortfolioRIModel, cboPortfolioRIModel_NewIndex, gPMFunctions.NullToLong(m_vRIModelUsage(ACRRiskTypeRIModelUsageCnt, lRow)))

                    End If

                Next lRow
            End If

            ' Re-set the combo list
            bFound = False
            For lRow As Integer = 0 To cboPortfolioRIModel.Items.Count - 1
                If VB6.GetItemData(cboPortfolioRIModel, lRow) = m_lPortfolioRIModelId Then
                    cboPortfolioRIModel.SelectedIndex = lRow
                    bFound = True
                    Exit For
                End If
            Next lRow

            If Not bFound Then
                cboPortfolioRIModel.SelectedIndex = -1
                m_lPortfolioRIModelId = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FillPortfolioList failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FillPortfolioList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetModel
    ' Description:  Go through the data array and returns the description
    '               of a given item (from its ID).
    ' ***************************************************************** '
    Private Function GetModelDesc(ByVal v_vUsageCnt As Object, ByRef r_sModelDesc As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sModelDesc = ""


            If CStr(v_vUsageCnt) = "" Or CStr(v_vUsageCnt) = "0" Then
                Return result
            End If

            If Information.IsArray(m_vRIModelUsage) Then
                For lRow As Integer = m_vRIModelUsage.GetLowerBound(1) To m_vRIModelUsage.GetUpperBound(1)

                    If CStr(v_vUsageCnt) = CStr(m_vRIModelUsage(ACRRiskTypeRIModelUsageCnt, lRow)) Then
                        r_sModelDesc = CStr(m_vRIModelUsage(ACRRIModelDescription, lRow))
                        Exit For
                    End If
                Next lRow
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetModelDesc failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetModelDesc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
End Class