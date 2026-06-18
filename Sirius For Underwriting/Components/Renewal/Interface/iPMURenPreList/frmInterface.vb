Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer Guide No. 19
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    Private m_SourceID As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURenPreList.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    'iGIS object
    Private m_oGIS As iGIS.Application

    'risk data information

    Private m_oRiskData As bSIRRiskData.Business

    'bSirInsuranceFile

    Private m_oInsuranceFile As bSIRInsuranceFile.Services

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' {* USER DEFINED CODE (Begin) *}
    Private m_vProductID As Integer
    Private m_dtCompareDate As Date


    'Report
    Private m_iPrintMode As Integer
    Private m_sCompiledReportPath As String = ""

    Private m_vAutoRenewalList As Object
    Private m_vManualRenewalList As Object

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
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

    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
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

    Public Property StepStatus() As String
        Get

            Return m_sStepStatus

        End Get
        Set(ByVal Value As String)

            m_sStepStatus = Value

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
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the controls to
            ' PMFormControl
            '
            ' Example:-
            '
            '        ' Pass control and required settings to FormControl
            '        m_lReturn = m_oFormFields.AddNewFormField( _
            ''                       ctlControl:=<Control Name>, _
            ''                       lFieldType:=<PM field type>, _
            ''                       lFormat:=<PM format string>, _
            ''                       lMandatory:=<PMMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            result = gPMConstants.PMEReturnCode.PMFalse

            If m_oFormFields.AddNewFormField(ctlControl:=txtRenewalDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory) = gPMConstants.PMEReturnCode.PMTrue Then


                result = m_oFormFields.AddNewFormField(ctlControl:=cboProductCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer


        Dim result As Integer = 0
        Return gPMConstants.PMEReturnCode.PMTrue



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

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
        Dim vRenewalList(,) As Object
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the details from the interface to the data storage.
        result = InterfaceToData()

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        'get policies which are not in Renewal Status table or has the status of "Policy Details Changed"
        'and expiry date is within range, specified in the Product table and user entered date.
        'add these to Renewal_Report table

        'prepare data for renewal selection

        result = m_oBusiness.DelRenewalStatusPolicies(v_lRenewalStatusTypeID:=gPMConstants.PMBRenewalStatusTypePolicyChanged, v_dtCompareDate:=m_dtCompareDate)

        If result <> gPMConstants.PMEReturnCode.PMTrue And result <> gPMConstants.PMEReturnCode.PMNotFound Then
            MessageBox.Show("Failed to prepare data for Renewal Prelist", ACApp, MessageBoxButtons.OK)
            Return result
        End If

        'get all policy that needs renewal

        result = m_oBusiness.GetRenewalSelection(v_vProductID:=m_vProductID, v_vBranchID:=m_SourceID, v_dtCompareDate:=m_dtCompareDate, r_vResultArray:=vRenewalList)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to select policies for Renewal Prelist", ACApp, MessageBoxButtons.OK)
            Return result
        End If

        'do we have any data
        If Not Information.IsArray(vRenewalList) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'delete all in renewal_report table ready for new data

        result = m_oBusiness.DelRenewalReport()

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to prepare Renewal Report table", ACApp, MessageBoxButtons.OK)
            Return result
        End If

        'loop through and process each policy
        'RWH(21/11/2000) Set to not found to indicate that no invalid renewals
        result = gPMConstants.PMEReturnCode.PMNotFound

        For lCount As Integer = 0 To vRenewalList.GetUpperBound(1)

            'RWH(16/11/2000) Check all renewal criteria for a policy.

            m_lReturn = CType(CheckRenewalCriteria(vRenewalList, lCount), gPMConstants.PMEReturnCode)

            'RWH(21/11/2000) Yes, we've found some invalid ones.
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

        Next lCount

        '*********************************************************

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)



    ' ***************************************************************** '
    ' Name: PrintRenewalReport
    '
    ' Description: Print Renewal Reports
    '
    ' ***************************************************************** '
    Private Function PrintRenewalReport() As Integer
        Dim result As Integer = 0


        Dim oReport As iPMBReportPrint.Interface_Renamed

        Dim vReportKeys As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oReport As Object
            result = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ReDim vReportKeys(1, 2)


            vReportKeys(0, 0) = "report_name"
            'JMK 06/08/2001
            'JMK 08/08/2001 - ManualRenewal is correct, Renewal_Pre_List is stand-alone
            '                   as per process 030.

            vReportKeys(1, 0) = "ManualRenewal"
            '    vReportKeys(1, 0) = "Renewal_Pre_List"

            vReportKeys(0, 1) = "report_print_options"

            vReportKeys(1, 1) = m_iPrintMode
            vReportKeys(0, 2) = "param_name1"
            vReportKeys(1, 2) = g_oObjectManager.UserName

            result = oReport.SetKeys(vReportKeys)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then

                oReport.Dispose()
                oReport = Nothing

                Return result
            End If


            result = oReport.Start


            oReport.Dispose()

            oReport = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PreviewReport
    '
    ' Description: Preview printed report
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PreviewReport) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PreviewReport(ByVal v_sReportFileName As String, ByVal v_sWindowTitle As String, ByRef r_cryControl As AxCrystal.AxCrystalReport) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'With r_cryControl
    '
    '.ReportFileName = v_sReportFileName
    'Set window to maximised
    '.WindowState = Crystal.WindowStateConstants.crptMaximized
    'Set Title for window if in preview mode
    '.WindowTitle = v_sWindowTitle
    'Tell the control where the report is going to
    '.Destination = Crystal.DestinationConstants.crptToWindow
    'result = .PrintReport()
    '
    '
    'End With
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreviewReport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreviewReport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign all of the details from the
            ' interface to the data storage.
            '
            ' Example:-
            '
            '    m_DName$ = trim$(txtName.Text)
            '    m_DDate = CDate(txtDate.Text)
            '    m_iDCodeID% = cmbCode.ItemData(cmbCode.ListIndex)
            '    m_lReturn& = m_oFormFields.UnformatControl(txtName)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            m_dtCompareDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtRenewalDate))

            'all product
            If VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex) = 0 Then

                m_vProductID = Nothing
            Else
                m_vProductID = VB6.GetItemData(cboProductCode, cboProductCode.SelectedIndex)
            End If

            'KB 19/3/02 add branch selection
            m_SourceID = VB6.GetItemData(cboBranchCode, cboBranchCode.SelectedIndex)


            If chkPreview.CheckState = CheckState.Checked Then
                If chkPrint.CheckState = CheckState.Checked Then
                    m_iPrintMode = MainModule.AC_PRINT_AND_VIEW
                Else
                    m_iPrintMode = MainModule.AC_VIEW_ONLY
                End If
            Else
                If chkPrint.CheckState = CheckState.Checked Then
                    m_iPrintMode = MainModule.AC_PRINT_ONLY
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            'default to first tab
            SSTabHelper.SetSelectedIndex(Me.tabMainTab, 0)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

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

            If m_oFormFields.FormatControl(ctlControl:=txtRenewalDate, vControlValue:=DateTime.Today) = gPMConstants.PMEReturnCode.PMTrue Then


                'Me.cmdRePrint.Enabled = False
                Me.chkPreview.CheckState = CheckState.Checked
                result = GetComboDetails(Me.cboProductCode)
                result = GetComboDetailsBranch(Me.cboBranchCode)
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_ctlTabFirstLast(ACControlStart, 0) = txtRenewalDate
            m_ctlTabFirstLast(ACControlEnd, 0) = cboProductCode

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the first and last controls", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFirstLastControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            'Developer Guide No. 243
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The iPMFunc.GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************


            lblRenewalDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRenewalDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblProductCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACProductCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkPreview.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPreview, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkPrint.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPrint, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ValidateForm
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If chkPreview.CheckState = CheckState.Checked Or chkPrint.CheckState = CheckState.Checked Then
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                MessageBox.Show("Preview or Print must be selected", ACApp, MessageBoxButtons.OK)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateForm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetComboDetails
    '
    ' Description: get details from numbering scheme and add to combobox
    '
    '
    ' ***************************************************************** '
    Private Function GetComboDetails(ByRef r_cboControl As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMFalse

        Try

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            Dim r_cboControl_NewIndex As Integer = -1
            r_cboControl_NewIndex = r_cboControl.Items.Add("All")
            VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)



            If m_oBusiness.GetLookUp(v_sTableName:="Product", v_sKeyIDFieldName:="product_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray) = gPMConstants.PMEReturnCode.PMTrue Then

                If Information.IsArray(vResultArray) Then

                    For icount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(1, icount)))

                        VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, icount)))
                    Next
                End If

                result = gPMConstants.PMEReturnCode.PMTrue

            End If

            'default to all products
            r_cboControl.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetComboDetailsBranch
    '
    ' Description: get details from numbering scheme and add to combobox
    ' 190302 Add branch as a selection parameter
    '
    ' ***************************************************************** '
    Private Function GetComboDetailsBranch(ByRef r_cboControl As ComboBox) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMFalse

        Try

            'make sure combobox is empty
            r_cboControl.Items.Clear()

            'add in non applicable value with ID of 0
            Dim r_cboControl_NewIndex As Integer = -1
            r_cboControl_NewIndex = r_cboControl.Items.Add("All")
            VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, 0)

            'VB 15/02/2005 PN-18396 Changed parameter value i.e (v_sDescFieldName ="description")

            If m_oBusiness.GetLookUp(v_sTableName:="Source", v_sKeyIDFieldName:="source_id", v_sDescFieldName:="description", r_vResultArray:=vResultArray) = gPMConstants.PMEReturnCode.PMTrue Then

                If Information.IsArray(vResultArray) Then

                    For icount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                        r_cboControl_NewIndex = r_cboControl.Items.Add(CStr(vResultArray(1, icount)).Trim())

                        VB6.SetItemData(r_cboControl, r_cboControl_NewIndex, CInt(vResultArray(0, icount)))
                    Next
                End If

                result = gPMConstants.PMEReturnCode.PMTrue

            End If

            'default to all branches
            r_cboControl.SelectedIndex = 0

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComboDetailsBranch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDetailsBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub chkPreview_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPreview.CheckStateChanged

        'RWH(24/05/2001) Make check boxes behave like radio buttons so only one
        'can be selected.
        If chkPreview.CheckState = CheckState.Checked Then
            chkPrint.CheckState = CheckState.Unchecked
        Else
            chkPrint.CheckState = CheckState.Checked
        End If

    End Sub

    Private Sub chkPrint_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPrint.CheckStateChanged

        'RWH(24/05/2001) Make check boxes behave like radio buttons so only one
        'can be selected.
        If chkPrint.CheckState = CheckState.Checked Then
            chkPreview.CheckState = CheckState.Unchecked
        Else
            chkPreview.CheckState = CheckState.Checked
        End If

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        'm_lReturn = CType(PMHelpFunc.ShowHelp(dlgHelp, ScreenHelpID), gPMConstants.PMEReturnCode)
        'Developer Guide No. 51
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdRePrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRePrint.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        'RWH(25/05/01) Set print mode.
        If chkPrint.CheckState = CheckState.Checked Then
            m_iPrintMode = MainModule.AC_PRINT_ONLY
        Else
            m_iPrintMode = MainModule.AC_VIEW_ONLY
        End If

        m_lReturn = CType(PrintRenewalReport(), gPMConstants.PMEReturnCode)

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    End Sub

    ' PRIVATE Methods (End)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRenSelection.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oRiskData As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskData, "bSIRRiskData.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oRiskData = temp_m_oRiskData

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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oInsuranceFile As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oInsuranceFile, "bSIRInsuranceFile.Services", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oInsuranceFile = temp_m_oInsuranceFile

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
            m_oGeneral = New iPMURenPreList.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

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

            'set up GIS object
            m_oGIS = New iGIS.Application()

            'was object created
            If m_oGIS Is Nothing Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create GIS object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub

            End If

            'can we initialise GIS object
            'Developer Guide No. 9
            If m_oGIS.Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)

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

            ' Gets the interface details to be displayed.
            m_lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)

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

            txtRenewalDate.Select()

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
                'Start Renuka PN 61400
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                'End Renuka PN 61400
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
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


            ' Terminate the form control object.
            m_oFormFields.Dispose()
            ' Destroy the instance of the form control object
            ' from memory.
            m_oFormFields = Nothing

            'destroy GIS object
            If Not (m_oGIS Is Nothing) Then

                m_oGIS.Dispose()
                m_oGIS = Nothing
            End If

            'destroy Policy object
            If Not (m_oInsuranceFile Is Nothing) Then
                m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

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

    'Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown

    '    Dim KeyCode As Integer = eventArgs.KeyCode
    '    Dim Shift As Integer = eventArgs.KeyData \ &H10000

    '    Dim iCtrlDown As Integer

    '    Const ACCtrlMask As Integer = 2

    '    Try

    '        ' Set the control key value.
    '        iCtrlDown = (Shift And ACCtrlMask) > 0

    '        With tabMainTab
    '            ' Check the key pressed.
    '            Select Case KeyCode
    '                Case Keys.PageUp
    '                    ' Page Up key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Display the first tab.
    '                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
    '                    Else
    '                        ' Check we are not on the
    '                        ' first tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
    '                            ' Display the previous tab.
    '                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
    '                        End If
    '                    End If

    '                Case Keys.PageDown
    '                    ' Page Down key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Display the last tab.
    '                        SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
    '                    Else
    '                        ' Check we are not on the
    '                        ' last tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
    '                            ' Display the next tab.
    '                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
    '                        End If
    '                    End If

    '                Case Keys.Home
    '                    ' Home key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Set focus the the start control on
    '                        ' the tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    '                            m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
    '                        End If
    '                    End If

    '                Case Keys.End
    '                    ' End key has been pressed.

    '                    ' Check if the control key has also
    '                    ' been pressed.
    '                    If iCtrlDown Then
    '                        ' Set focus the the start control on
    '                        ' the tab.
    '                        If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
    '                            m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
    '                        End If
    '                    End If
    '            End Select
    '        End With
    '        'Developer Guide No 293
    '        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
    '            tabMainTab.SelectedIndex = 0
    '            _tabMainTab_TabPage0.Select()
    '            _tabMainTab_TabPage0.Focus()
    '        End If
    '    Catch



    '        ' Error Section.

    '        Exit Sub
    '    End Try

    'End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            'Start Renuka PN 61400
            'm_lStatus& = PMOK
            'End Renuka PN 61400

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check mandatory controls have been entered into.
            If m_oFormFields.CheckMandatoryControls() = gPMConstants.PMEReturnCode.PMTrue Then
                If ValidateForm() = gPMConstants.PMEReturnCode.PMTrue Then
                    'Start Renuka PN 61400
                    m_lStatus = gPMConstants.PMEReturnCode.PMOK
                    'End Renuka PN 61400
                    ' Process the next set of actions depending
                    ' upon the interface task etc.
                    m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                    'turn off reprint - data in the renewal_report table may be empty
                    Me.cmdRePrint.Enabled = False

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If Not (Status = gPMConstants.PMEReturnCode.PMCancel) Then

                            '***************PRINT OUT REPORTS*****************
                            If PrintRenewalReport() = gPMConstants.PMEReturnCode.PMTrue Then
                                MessageBox.Show("Renewal Prelist completed successfully", "Renewal Prelist Completed", MessageBoxButtons.OK)
                                'TN20010703 - user may want to run prelist against another product
                                'Me.Hide
                            Else
                                MessageBox.Show("Failed to preview/print report", "Preview/Print Failed", MessageBoxButtons.OK)
                                'failed to print - give user option to reprint
                                Me.cmdRePrint.Enabled = True
                            End If
                        End If

                    ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        MessageBox.Show("No data found for current criteria", ACApp, MessageBoxButtons.OK)
                    Else
                        MessageBox.Show("Renewal Prelist Failed", Application.ProductName)
                    End If

                    'Tomo230801
                    'Enable it anyway, they may want to reprint without exiting and re-entering
                    'the program
                    cmdRePrint.Enabled = True

                End If
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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
            If m_oGeneral.ProcessCommand() = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtRenewalDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRenewalDate.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRenewalDate)
    End Sub
    Private Sub txtRenewalDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRenewalDate.Leave


        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRenewalDate)


    End Sub

    ' ***************************************************************** '
    '
    ' Name: CheckRenewalCriteria
    '
    ' Description: Checks all Renewal Criteria for a policy.
    '
    ' History: 16/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CheckRenewalCriteria(ByRef vRenewalList(,) As Object, ByRef lCount As Integer) As Integer

        Dim result As Integer = 0
        Dim lCheckForClaim As gPMConstants.PMEReturnCode 'set to PMTrue if there are no claims
        Dim lRenewalStatusTypeID As Integer 'renewal status to go on the Renewal Status table
        Dim sFailureCriterion, sFailureDetail As String

        'Thinh Nguyen 15/08/2003
        Dim lIsAgentCancelled As gPMConstants.PMEReturnCode


        Const PMNumberOfRenewalCriteria As Integer = 8

        Const PMRenCritAutoRenewalSet As Integer = 1
        Const PMRenCritPartyRenewalStop As Integer = 2
        Const PMRenCritPolicyRenewalStop As Integer = 3
        Const PMRenCritReferredAtRenewal As Integer = 4
        Const PMRenCritClaims As Integer = 5
        Const PMRenCritAgentRenewalStop As Integer = 6
        Const PMRenCritAgentCancelled As Integer = 7
        Const PMRenCritAgentTransferred As Integer = 8

        Const PMAutoRenewalDesc As String = "Auto-renewal flag not set"
        Const PMPartyRenewalStopDesc As String = "Party renewal stop code"
        Const PMPolicyRenewalStopDesc As String = "Policy renewal stop code"
        Const PMReferredAtRenewalDesc As String = "Referred at Renewal"
        Const PMClaimsMadeDesc As String = "Failed claims criteria"
        Const PMAgentRenewalStopDesc As String = "Agent renewal stop code"
        Const PMAgentCancelled As String = "Agent has been cancelled"
        Const PMAgentTransferred As String = "Agent is subject to a transfer"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'check for claims (WHAT HAPPEN IF ITS FAILED/ERRORED ????????)

            lCheckForClaim = m_oBusiness.CheckForClaim(v_lInsuranceFileCnt:=vRenewalList(PMFieldPosInsuranceFileCnt, lCount))

            'Thinh Nguyen 15/08/2003


            If vRenewalList(PMFieldPosLeadAgentCnt, lCount) Is DBNull.Value Then


                m_lReturn = m_oBusiness.IsAgentCancelled(v_lPartyCnt:=CInt(vRenewalList(PMFieldPosLeadAgentCnt, lCount)), r_lIsCancelled:=lIsAgentCancelled)
            End If

            'Check each renewal failure criterion.
            For iRenewalCriterion As Integer = 1 To PMNumberOfRenewalCriteria
                sFailureCriterion = ""
                sFailureDetail = ""

                Select Case (iRenewalCriterion)
                    Case PMRenCritAutoRenewalSet

                        If CDbl(vRenewalList(PMFieldPosIsAutoRenewable, lCount)) = 0 Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMAutoRenewalDesc
                        End If

                    Case PMRenCritPartyRenewalStop

                        If Not (Convert.IsDBNull(vRenewalList(PMFieldPosClientStopReason, lCount)) Or IsNothing(vRenewalList(PMFieldPosClientStopReason, lCount))) Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMPartyRenewalStopDesc

                            sFailureDetail = CStr(vRenewalList(PMFieldPosClientStopReason, lCount))
                        End If

                    Case PMRenCritPolicyRenewalStop

                        If Not (Convert.IsDBNull(vRenewalList(PMFieldPosPolicyStopReason, lCount)) Or IsNothing(vRenewalList(PMFieldPosPolicyStopReason, lCount))) Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMPolicyRenewalStopDesc

                            sFailureDetail = CStr(vRenewalList(PMFieldPosPolicyStopReason, lCount))
                        End If

                    Case PMRenCritReferredAtRenewal

                        If CDbl(vRenewalList(PMFieldPosReferredAtRenewal, lCount)) <> 0 Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMReferredAtRenewalDesc
                        End If

                    Case PMRenCritClaims
                        If lCheckForClaim <> gPMConstants.PMEReturnCode.PMTrue Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMClaimsMadeDesc
                        End If

                    Case PMRenCritAgentRenewalStop


                        If (Not (Convert.IsDBNull(vRenewalList(PMFieldPosAgentStopReason, lCount)) Or IsNothing(vRenewalList(PMFieldPosAgentStopReason, lCount)))) And (CStr(vRenewalList(PMFieldPosAgentStopReason, lCount)) <> "") Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview

                            sFailureCriterion = PMAgentRenewalStopDesc & " - " & CStr(vRenewalList(PMFieldPosAgentStopReason, lCount))
                        End If

                    Case PMRenCritAgentCancelled
                        If lIsAgentCancelled = gPMConstants.PMEReturnCode.PMTrue Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMAgentCancelled
                        End If

                    Case PMRenCritAgentTransferred

                        If gPMFunctions.ToSafeInteger(CStr(vRenewalList(PMFieldPosAgentInTransfer, lCount)), 0) = 1 Then
                            lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview
                            sFailureCriterion = PMAgentTransferred
                        End If
                End Select

                If lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeManualReview Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    'add to Renewal_Report table for each renewal criterion the policy
                    'fails on.

                    m_lReturn = m_oBusiness.AddRenewalReport(v_sReportType:=IIf(lRenewalStatusTypeID = gPMConstants.PMBRenewalStatusTypeAutoRated, "AutoRenewal", "ManualRenewal"), v_vClientName:=vRenewalList(PMFieldPosClientName, lCount), v_vPolicyNumber:=vRenewalList(PMFieldPosInsuranceRef, lCount), v_vAgentCode:=vRenewalList(PMFieldPosAgentName, lCount), v_vCoverStartDate:=vRenewalList(PMFieldPosCoverStartDate, lCount), v_vCoverEndDate:=vRenewalList(PMFieldPosCoverEndDate, lCount), v_vProductCode:=vRenewalList(PMFieldPosProductCode, lCount), v_vFailureCriterion:=sFailureCriterion, v_vFailureDetail:=sFailureDetail)
                End If

                lRenewalStatusTypeID = 0

            Next iRenewalCriterion

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRenewalCriteria Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRenewalCriteria", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
        MemoryHelper.ReleaseMemory()
    End Sub
    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
            tabMainTab.Focus()
        End If
    End Sub

End Class
