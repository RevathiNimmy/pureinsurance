'Option Strict On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Friend Class frmInterface_Renamed
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
    Private Const ACClass As String = "frmInterface_Renamed"

    ' Object parameter members.
    Private m_sCallingAppName As String
    Private m_nStatus As Integer
    Private m_nErrorNumber As Integer

    Private m_nTask As Integer
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_nRiskTypeId As Integer
    Private m_sDescription As String
    'Variables to store data taken from the List View
    Private m_nAction As Integer
    Private m_nRiskTypeLimitVersionId As Integer

    'JMK 23/10/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String
    ' AMB 28/05/2003: 1.8.6 Deferred RI RFC

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURiskTypeRILimits.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'Private m_oBusiness As bSIRRIModelUsage.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_oFrmRILimits As iPMURiskTypeRILimits.frmInterface
    ' Stores the return value for the a
    ' function call.
    Private m_nReturn As Integer
    Private m_oRiskTypeRILimits As Object
    ' Control array to store the first and last
    ' text box controls for each tab.
    ' Stores the search data from the business object.
    Private m_oRILimitVersion As Object

    Private m_dtLimitEffectiveDate As Date
    Private m_dtLimitExpiryDate As Date
    Private m_sLimitDescription As String
    Private m_oBusinessRILimits As Object
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Return any error number that might have
            ' occurred on the interface.
            ErrorNumber = m_nErrorNumber

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property



    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Status = m_nStatus
        End Get
        Set(ByVal Value As Integer)
            ' Set the interface exit status.
            m_nStatus = Value
        End Set
    End Property

    Public Property Task() As Short
        Get
            Task = m_nTask
        End Get
        Set(ByVal Value As Short)
            m_nTask = Value
        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_nNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_nProcessMode = Value
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
            m_nRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public Property LimitEffectiveDate() As Date
        Get
            LimitEffectiveDate = m_dtLimitEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtLimitEffectiveDate = Value
        End Set
    End Property

    Public Property LimitExpiryDate() As Date
        Get
            LimitExpiryDate = m_dtLimitExpiryDate
        End Get
        Set(ByVal Value As Date)
            m_dtLimitExpiryDate = Value
        End Set
    End Property

    Public Property LimitDescription() As String
        Get
            LimitDescription = m_sLimitDescription
        End Get
        Set(ByVal Value As String)
            m_sLimitDescription = Value
        End Set
    End Property

    Public Property RiskTypeLimitVersionId() As Integer
        Get
            RiskTypeLimitVersionId = m_nRiskTypeLimitVersionId
        End Get
        Set(ByVal Value As Integer)
            m_nRiskTypeLimitVersionId = Value
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


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetFieldValidation() As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataRefresh() As Integer

        Dim nSelectedItem As Integer
        Dim nItemStatus As Integer
        Dim bExists1 As Boolean
        Dim sLimitDescription As String
        Dim dtLimitEffectiveDate As Date
        Dim dtLimitExpiryDate As Date
        Dim nResult As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_oRILimitVersion) Then
                If m_nAction = gPMConstants.PMEComponentAction.PMAdd Then
                    For ncount As Integer = m_oRILimitVersion.GetLowerBound(1) To m_oRILimitVersion.GetUpperBound(1)
                        If CDate(m_oRILimitVersion(ACREffectiveDate, ncount)) <= m_dtLimitEffectiveDate AndAlso CDate(m_oRILimitVersion(ACRExpiryDate, ncount)) > m_dtLimitEffectiveDate Then
                            bExists1 = True
                            Exit For
                        End If
                    Next ncount
                ElseIf m_nAction = gPMConstants.PMEComponentAction.PMEdit Then
                    For ncount As Integer = m_oRILimitVersion.GetLowerBound(1) To m_oRILimitVersion.GetUpperBound(1)
                        If ncount <> DirectCast(lvwRILimitVersion.Items.Item(lvwRILimitVersion.FocusedItem.Index).Tag, Integer) Then
                            If m_oRILimitVersion(ACRRILimitVersionId, ncount) = m_nRiskTypeLimitVersionId AndAlso CDate(m_oRILimitVersion(ACREffectiveDate, ncount)) <= m_dtLimitEffectiveDate AndAlso CDate(m_oRILimitVersion(ACRExpiryDate, ncount)) > m_dtLimitEffectiveDate Then

                                bExists1 = True
                            End If
                            Exit For
                        End If
                    Next ncount
                End If
            End If


            If bExists1 Then
                MessageBox.Show("Duplicate Usage Record created. Please correct your values and try again.", "Reinsurance Model", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return nResult
            End If

            Select Case m_nAction
                Case gPMConstants.PMEComponentAction.PMAdd

                    If Information.IsArray(m_oRILimitVersion) Then
                        nSelectedItem = m_oRILimitVersion.GetUpperBound(1) + 1
                        ReDim Preserve m_oRILimitVersion(ACRMax, nSelectedItem)
                    Else
                        nSelectedItem = 0
                        ReDim m_oRILimitVersion(ACRMax, nSelectedItem)
                    End If

                    nItemStatus = ACItemStatus_Added
                    sLimitDescription = m_sLimitDescription
                    dtLimitEffectiveDate = m_dtLimitEffectiveDate
                    dtLimitExpiryDate = m_dtLimitExpiryDate
                Case gPMConstants.PMEComponentAction.PMEdit

                    nSelectedItem = CType(Convert.ToString(lvwRILimitVersion.Items.Item(lvwRILimitVersion.FocusedItem.Index).Tag), Integer)

                    If m_oRILimitVersion(ACRItemStatus, nSelectedItem) <> ACItemStatus_Added Then
                        nItemStatus = ACItemStatus_Changed
                    Else
                        nItemStatus = ACItemStatus_Added
                    End If

                    sLimitDescription = m_sLimitDescription
                    dtLimitEffectiveDate = m_dtLimitEffectiveDate
                    dtLimitExpiryDate = m_dtLimitExpiryDate


                Case gPMConstants.PMEComponentAction.PMDelete

                    nSelectedItem = CType(Convert.ToString(lvwRILimitVersion.Items.Item(lvwRILimitVersion.FocusedItem.Index).Tag), Integer)

                    nItemStatus = ACItemStatus_Deleted

            End Select

            m_oRILimitVersion(ACRRiskTypeVersionId, nSelectedItem) = m_nRiskTypeId
            m_oRILimitVersion(ACRDescription, nSelectedItem) = sLimitDescription
            m_oRILimitVersion(ACREffectiveDate, nSelectedItem) = dtLimitEffectiveDate
            m_oRILimitVersion(ACRExpiryDate, nSelectedItem) = dtLimitExpiryDate
            m_oRILimitVersion(ACRItemStatus, nSelectedItem) = nItemStatus

            m_nReturn = BusinessToInterface()

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the refresh from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

   
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBusiness() As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            'Tell it what we're getting

            m_oBusiness.RiskTypeId = m_nRiskTypeId

            ' AMB 28/05/2003: 1.8.6 Deferred RI RFC - m_lIsDeferred added

            m_nReturn = m_oBusiness.GetRiskTypeRILimitsVersion(m_oRILimitVersion:=m_oRILimitVersion)

            ' Check for errors
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                nResult = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                Return nResult
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

   
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer

        Dim nResult As Integer
        Dim oListItem As System.Windows.Forms.ListViewItem
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the interface.
            lblRiskType.Text = m_sDescription

            ' Populate listview
            lvwRILimitVersion.Items.Clear()
            If Information.IsArray(m_oRILimitVersion) Then
                For lTemp As Integer = m_oRILimitVersion.GetLowerBound(1) To m_oRILimitVersion.GetUpperBound(1)
                    If m_oRILimitVersion(ACRDescription, lTemp) <> "" Then

                        oListItem = lvwRILimitVersion.Items.Add(Trim(m_oRILimitVersion(ACRDescription, lTemp)))

                        oListItem.SubItems.Add(Trim(CDate(m_oRILimitVersion(ACREffectiveDate, lTemp))))

                        oListItem.SubItems.Add(Trim(CDate(m_oRILimitVersion(ACRExpiryDate, lTemp))))

                        oListItem.Tag = lTemp

                    End If
                Next lTemp


                If lvwRILimitVersion.Items.Count = 1 And m_oRILimitVersion(ACRItemStatus, 0) = ACItemStatus_Added Then
                    btnCopy.Enabled = False
                End If
            End If

            If lvwRILimitVersion.Items.Count = 0 Then
                btnEdit.Enabled = False
                btnDelete.Enabled = False
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

   
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer

        Dim nResult As Integer
        Dim nRILimitVersionId As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            m_oBusiness.RiskTypeId = m_nRiskTypeId

            ' If the array is empty, it means there is nothing to save.
            If Not Information.IsArray(m_oRILimitVersion) Then
                ' Exit without raising an error
                Return nResult
            End If

            ' AMB 29/05/2003: 1.8.6 Deferred RI RFC - added 'is_deferred'
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If
            m_sScreenHierarchy = $"Risk Type({m_sDescription.Trim()})"
            m_nReturn = m_oBusiness.UpdateLimitVersions(m_oRILimitVersion:=m_oRILimitVersion, nRILimitVersionId:=nRILimitVersionId, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            If nRILimitVersionId > 0 Then
                Dim temp_m_obRILimits As Object
                m_nReturn = g_oObjectManager.GetInstance(oObject:=temp_m_obRILimits, sClassName:="bSIRRiskTypeRILimits.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oBusinessRILimits = temp_m_obRILimits
                m_oBusinessRILimits.RiskTypeId = m_nRiskTypeId

                m_oBusinessRILimits.RiskTypeRILimitVersionId = nRILimitVersionId

                m_nReturn = m_oBusinessRILimits.Update(v_vRiskTypeRILimits:=m_oRiskTypeRILimits)

            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

 
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)
            tabMainTab.Left = CType(VB6.TwipsToPixelsX(120), Integer)

            ' Display all language specific captions.
            m_nReturn = DisplayCaptions()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            btnEdit.Enabled = False
            btnCopy.Enabled = False
            btnDelete.Enabled = False

            If (Task = gPMConstants.PMEComponentAction.PMView) Then
                btnAdd.Enabled = False
            Else
                btnAdd.Enabled = True
            End If
            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

  
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisplayCaptions() As Integer

        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                nResult = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return nResult
            End If


            btnOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            btnCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            btnHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub btnAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnAdd.Click

        Dim oForm As frmInterface
        m_nAction = gPMConstants.PMEComponentAction.PMAdd
        oForm = New frmInterface()
        With oForm
            .CallingAppName = m_sCallingAppName
            .Task = m_nTask
            .Navigate = m_nNavigate
            .ProcessMode = m_nProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            .RiskTypeId = m_nRiskTypeId
            .Description = m_sDescription
            .UniqueId = m_sUniqueId
            .ScreenHierarchy = m_sScreenHierarchy
            .Mode = CType(gPMConstants.PMEComponentAction.PMAdd, Short)
            ' {* USER DEFINED CODE (End) *}
        End With

        oForm.ShowDialog()

        If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
            m_sLimitDescription = ToSafeString(oForm.LimitDescription)
            m_dtLimitEffectiveDate = ToSafeDate(oForm.LimitEffectiveDate)
            m_dtLimitExpiryDate = ToSafeDate(oForm.LimitExpiryDate)
            m_oRiskTypeRILimits = oForm.RiskTypeRILimits
            m_nReturn = DataRefresh()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub btnCopy_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles btnCopy.Click
        Dim nSelectedItem As Integer
        Dim nIndex As Integer
        Dim dtLimitEffectiveDate As Date
        Dim dtLimitExpiryDate As Date
        Dim bExists As Boolean
        Try

            m_nAction = CType(gPMConstants.PMEComponentAction.PMCopy, Short)

            ' Select item in array

            nSelectedItem = CType(lvwRILimitVersion.Items.Item(lvwRILimitVersion.FocusedItem.Index).Tag, Integer)

            ' Store id (needed for replaces filtering)
            m_nRiskTypeLimitVersionId = m_oRILimitVersion(ACRRILimitVersionId, nSelectedItem)
            m_sLimitDescription = m_oRILimitVersion(ACRDescription, nSelectedItem)
            dtLimitEffectiveDate = DateAdd(Microsoft.VisualBasic.DateInterval.Year, 1, m_oRILimitVersion(ACREffectiveDate, nSelectedItem))
            dtLimitExpiryDate = DateAdd(Microsoft.VisualBasic.DateInterval.Year, 1, m_oRILimitVersion(ACRExpiryDate, nSelectedItem))

            For nIndex = LBound(m_oRILimitVersion, 2) To UBound(m_oRILimitVersion, 2)
                If m_oRILimitVersion(ACREffectiveDate, nIndex) = dtLimitEffectiveDate And m_oRILimitVersion(ACRExpiryDate, nIndex) = dtLimitExpiryDate Then
                    bExists = True
                End If
            Next
            If bExists = True Then
                MessageBox.Show("Can't copy limit as Limit already exists for next period", "Copy Limits", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            ElseIf m_nRiskTypeLimitVersionId = 0 Then
                MessageBox.Show("Can't copy. Please save the newly added limit first.", "Copy Limits", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            m_dtLimitEffectiveDate = dtLimitEffectiveDate
            m_dtLimitExpiryDate = dtLimitExpiryDate


            m_nReturn = CopyLimits()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            m_nReturn = GetBusiness()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            m_nReturn = BusinessToInterface()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If
            Exit Sub
        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="btnCopy_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="btnCopy_Click", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub btnDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnDelete.Click

        Dim nSelectedItem As Integer
        Dim nRiskTypeRILimitVersionId As Integer
        Try

            ' Select item in array

            nSelectedItem = CType(Convert.ToString(lvwRILimitVersion.Items.Item(lvwRILimitVersion.FocusedItem.Index).Tag), Integer)

            m_oRILimitVersion(ACRItemStatus, nSelectedItem) = ACItemStatus_Deleted

            nRiskTypeRILimitVersionId = m_oRILimitVersion(ACRRILimitVersionId, nSelectedItem)

            m_nReturn = m_oBusiness.DeleteRiskTypeRILimitVersion(r_nRiskTypeRILimitVersionId:=nRiskTypeRILimitVersionId)
            If m_nReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Exit Sub
            End If

            ' Reenable buttons
            btnCancel.Enabled = True
            btnAdd.Enabled = True
            btnOK.Enabled = True
            btnDelete.Enabled = False
            btnEdit.Enabled = False

            ' Refresh list
            m_nAction = CType(gPMConstants.PMEComponentAction.PMDelete, Short)


            m_nReturn = DataRefresh()

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="btnDelete_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="btnDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub btnEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnEdit.Click
        Dim oForm As frmInterface
        Dim nSelectedItem As Integer
        Try

            m_nAction = CType(gPMConstants.PMEComponentAction.PMEdit, Short)

            btnOK.Enabled = False
            btnCancel.Enabled = False
            oForm = New frmInterface()
            nSelectedItem = CType(lvwRILimitVersion.Items.Item(lvwRILimitVersion.FocusedItem.Index).Tag, Integer)
            With oForm
                .CallingAppName = m_sCallingAppName
                .Task = m_nTask
                .Navigate = m_nNavigate
                .ProcessMode = m_nProcessMode
                .TransactionType = m_sTransactionType
                .EffectiveDate = m_dtEffectiveDate

                ' {* USER DEFINED CODE (Begin) *}
                .RiskTypeId = m_nRiskTypeId
                .Description = m_sDescription
                .Mode = gPMConstants.PMEComponentAction.PMEdit
                .LimitDescription = ToSafeString(m_oRILimitVersion(ACRDescription, nSelectedItem))
                .LimitEffectiveDate = ToSafeDate(m_oRILimitVersion(ACREffectiveDate, nSelectedItem))
                .LimitExpiryDate = ToSafeDate(m_oRILimitVersion(ACRExpiryDate, nSelectedItem))
                .RiskTypeLimitVersionId = ToSafeInteger(m_oRILimitVersion(ACRRILimitVersionId, nSelectedItem))
                .ItemStatus = ToSafeInteger(m_oRILimitVersion(ACRItemStatus, nSelectedItem))
                .UniqueId = m_sUniqueId
                ' {* USER DEFINED CODE (End) *}
            End With
            'pass selected details to Risk Type object

            btnOK.Enabled = True
            btnCancel.Enabled = True
            oForm.ShowDialog()

            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                m_sLimitDescription = oForm.LimitDescription
                m_dtLimitEffectiveDate = oForm.LimitEffectiveDate
                m_dtLimitExpiryDate = oForm.LimitExpiryDate
                'Update the existing item
                m_nReturn = DataRefresh()

                If (m_nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ' Failed to update the data.
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="btnEdit_Click failed", vApp:=ACApp, vClass:=ACClass, vMethod:="btnEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub



    Private Sub btnHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnHelp.Click

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_nErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_nReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskTypeRILimits.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURiskTypeRILimits.General()

            ' Call the initialise RIBand passing this interface
            ' and the business object as parameters.
            m_nReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()
            m_oFrmRILimits = New iPMURiskTypeRILimits.frmInterface()
            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception


            m_nErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Renamed_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' Possibly creating the business object.
            If m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_nReturn = m_oBusiness.SetProcessModes(vTask:=m_nTask, vNavigate:=m_nNavigate,
                                                    vProcessMode:=m_nProcessMode,
                                                    vTransactionType:=m_sTransactionType,
                                                    vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to set the process modes for the business object", vApp:=ACApp,
                                   vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

            ' Set the business keys.

            m_oBusiness.RiskTypeId = m_nRiskTypeId

            ' Validate fields using Forms Control
            m_nReturn = SetFieldValidation()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_nReturn = iPMFunc.GetSystemOption(5005, m_sUnderwritingType)

            ' Set the interface default values.
            m_nReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_nReturn = m_oGeneral.GetInterfaceDetails()

            ' Check for errors.
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

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

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_renamed_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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
                m_nReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    'Developer Guide No.7
                    eventArgs.Cancel = True

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




            m_nErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub btnOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnOK.Click

        ' Click event of the OK button.
        Dim nCount As Integer
        Dim bExists1 As Boolean
        Dim nRow As Integer
        Dim dtRIEffectiveDate As Date
        Dim dtRIExpiryDate As Date
        Try

            ' Set the interface status.
            m_nStatus = gPMConstants.PMEReturnCode.PMOK

            If IsArray(m_oRILimitVersion) Then
                For nRow = LBound(m_oRILimitVersion, 2) To UBound(m_oRILimitVersion, 2)
                    dtRIEffectiveDate = m_oRILimitVersion(ACREffectiveDate, nRow)
                    dtRIExpiryDate = m_oRILimitVersion(ACRExpiryDate, nRow)

                    For nCount = LBound(m_oRILimitVersion, 2) To UBound(m_oRILimitVersion, 2)
                        If nRow <> nCount Then
                            If (m_oRILimitVersion(ACREffectiveDate, nCount) <= dtRIEffectiveDate And m_oRILimitVersion(ACRExpiryDate, nCount) >= dtRIEffectiveDate) Or (m_oRILimitVersion(ACREffectiveDate, nCount) <= dtRIExpiryDate And m_oRILimitVersion(ACRExpiryDate, nCount) >= dtRIExpiryDate) Then

                                bExists1 = True
                                Exit For
                            End If
                        End If
                    Next nCount

                    If bExists1 = True Then
                        Exit For
                    End If
                Next nRow

            End If

            If bExists1 Then
                MessageBox.Show("RI Arrangement Already Exists", "RI Model Usage", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_nReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="btnOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_nReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="btnCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwRIModelUsage_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRILimitVersion.DoubleClick

        If btnEdit.Enabled Then
            btnEdit_Click(btnEdit, New EventArgs())
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwRIModelUsage_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRILimitVersion.MouseDown
        'Developer Guide No.70
        Dim x As Integer = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwRILimitVersion.GetItemAt(x, CType(y, Integer)) Is Nothing Then
                btnDelete.Enabled = False
                btnEdit.Enabled = False
            Else
                btnDelete.Enabled = True
                btnEdit.Enabled = True
                btnCopy.Enabled = True
            End If
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CopyLimits() As Integer
        Dim nResult As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' Check if an item is selected
            If lvwRILimitVersion.FocusedItem Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            m_nReturn = m_oBusiness.CopyRiskTypeRILimitsVersion(nRiskTypeRILimitVersionId:=m_nRiskTypeLimitVersionId)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return nResult

        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyLimits", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
End Class
