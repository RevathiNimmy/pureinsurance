Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Security.Policy
Imports Artinsoft.VB6.Gui
Imports Microsoft.Reporting.WinForms
'Developer Guide No. 129
Imports SharedFiles
'Partial Public Class frmInterface

Public Class frmInterface
    Inherits System.Windows.Forms.Form

    Public Const ACRptName_AccountsEarnedPremium As String = "ACCOUNTS\EARNED_PREMIUM"
    Public Const ACRptName_AccountsUnearnedPremium As String = "ACCOUNTS\UNEARNED_PREMIUM"
    Public Const ACRptName_ClaimsOSClaims As String = "CLAIMS\OUTSTANDING_CLAIMS"
    Public Const ACRptName_ClaimsOSClaimsGrossToNet As String = "CLAIMS\OUTSTANDING_CLAIMS_GROSS_TO_NET"
    'MKR 25/10/2004 PN 15730 -- specific checks on some reports added
    Public Const ACRptName_AccountsProfitAndLossAll As String = "ACCOUNTS\PROFIT_&_LOSS_ALL"
    Public Const ACRptName_AccountsProfitAndLossYear As String = "ACCOUNTS\PROFIT_&_LOSS_YEAR"
    Public Const ACRptName_AccountsProfitAndLossBudget As String = "ACCOUNTS\PROFIT_&_LOSS_BUDGET"
    Public Const ACRptName_TrialBalance As String = "ACCOUNTS\TRIAL_BALANCE_U"
    Public Const ACRptName_TrialBalanceSummary As String = "ACCOUNTS\TRIAL_BALANCE_SUMMARY"
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
        End If
    End Sub
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 07/01/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '
    'Developer Guide No. 69
    Public frmParameters As frmParameters
    Private dataTable As DataTable
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    'Public m_sCallingAppName As String = ""
    Dim strParam As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_oReport As Object

    Private m_sReportPath As String = ""
    Private m_sCustomer As String = ""
    Private m_sCompiledReportPath As String = ""
    'Private m_sReportOutputLocation As String = ""
    Private m_vParameters(,) As Object
    'Developer Guide No. 33
    Private m_vDefaultValues As Object
    ' JMK 12/07/2001 new variable for ReportName with UserID appended
    'Private m_sUserReportName As String = ""
    Private m_sReportName As String = ""
    Private m_sDescription As String = String.Empty
    Private m_sReportType As String = ""
    Private m_iPrintReport As Integer
    Private m_sUniqueReportName As String = ""
    'DC270303 -ISS1911
    Private m_lSessionId As Integer

    '31/10/2002 - PWC- Added for filtering of reports
    Private m_sFilterName As String = ""
    Private m_sFilterValue As String = ""
    Private Const ksFilterByReportGroup As String = "report_group"
    Private m_bSaveParams As Boolean

    Private m_vKeyPrompts(,) As Object 'Array to hold passed prompt values

    Private Const ACLabelTop As Integer = 360
    Private Const ACListBoxTop As Integer = 600
    Private Const ACListBoxFullHeight As Integer = 2736

    ' To hold separate lists of reports
    Private m_colReportFolders As Collection
    Private m_colReportList() As Collection

    ' JMK30012001 to hold Underwriting Limited list (U/W hidden option)
    Private m_vLimitedReportList(,) As Object
    ' JMK30012001 variable to determine system mode ie "Underwriting or Agency"
    Private m_sUnderwritingOrAgency As String = ""
    ' TB 12/07/2002: Progress Bar
    Private m_oProgressBar As iPMBProgressBarWrapper.Wrapper

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBReportPrint.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' TF021000 - Constants for use with Crystal Control
    Private Const crptMaximized As Integer = 2
    Private Const crptToWindow As Integer = 0
    ' PRIVATE Data Members (End)

    Private m_iBaseCurrencyID As Integer
    Private m_sCurrencySymbol As String = ""
    'Checking For All Branches
    Private m_bAllowAll As Boolean
    Private m_sAllBranch As String = ""

    Private m_bAttachToScheduler As Boolean
    Private m_sFrequency As String = ""

    Private m_vIDValues As Object

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property AllBranch() As String
        Get

            Return m_sAllBranch

        End Get
    End Property

    Public ReadOnly Property AllowAll() As Boolean
        Get

            Return m_bAllowAll

        End Get
    End Property

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

    ' {* USER DEFINED CODE (End) *}
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
    ' {* USER DEFINED CODE (Begin) *}
    Public Property ReportName() As String
        Get

            Return m_sReportName

        End Get
        Set(ByVal Value As String)

            m_sReportName = Value

        End Set
    End Property

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(value As String)
            m_sDescription = value
        End Set
    End Property

    Public WriteOnly Property PrintReport() As Integer
        Set(ByVal Value As Integer)

            m_iPrintReport = Value

        End Set
    End Property

    Public WriteOnly Property Parameters() As Object
        Set(ByVal Value As Object)

            m_vParameters = Value

        End Set
    End Property

    '31/10/2002 - PWC - Added for filtering of reports
    Public Property FilterName() As String
        Get

            Return m_sFilterName

        End Get
        Set(ByVal Value As String)

            m_sFilterName = Value

        End Set
    End Property

    Public Property FilterValue() As String
        Get

            Return m_sFilterValue

        End Get
        Set(ByVal Value As String)

            m_sFilterValue = Value

        End Set
    End Property

    Public Property SaveParams() As Boolean
        Get

            Return m_bSaveParams

        End Get
        Set(ByVal Value As Boolean)

            m_bSaveParams = Value

        End Set
    End Property

    Public Property KeyPrompts() As Object
        Get

            Return VB6.CopyArray(m_vKeyPrompts)

        End Get
        Set(ByVal Value As Object)

            m_vKeyPrompts = Value

        End Set
    End Property

    '8.5
    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
        End Set
    End Property

    'DC270303 -ISS1911
    Public WriteOnly Property SessionId() As Integer
        Set(ByVal Value As Integer)

            m_lSessionId = Value

        End Set
    End Property

    Public WriteOnly Property Business() As Object
        Set(ByVal value As Object)
            m_oBusiness = value
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

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function initializeObject() As Integer 'PN 2062-Ritu
        Dim temp_m_oBusiness As Object = Nothing
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness
        Return m_lReturn
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

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}

            If (m_oBusiness Is Nothing) Then 'PN 2062-Ritu
                initializeObject()
            End If

            m_lReturn = m_oBusiness.GetReportsList()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

            ' Predefined Report will now have path added to it

            m_sReportName = m_oBusiness.ReportName
            m_sDescription = m_oBusiness.Description
            ' {* USER DEFINED CODE (End) *}

            'Has a filter been set?
            Select Case m_sFilterName
                Case ksFilterByReportGroup
                    'Filter the reports based on the report group passed

                    m_lReturn = m_oBusiness.GetReportsFilterByReportGroupCode(v_sReportGroupCode:=m_sFilterValue, r_vLimitedReportList:=m_vLimitedReportList)
                Case Else
                    'No, so process as before...
                    ' JMK30012001 (U/W hidden option)
                    ' Get the Limited list of Reports
                    ' SET 27/02/2003 AON44 - include this functionality for Broking
                    '            If m_sUnderwritingOrAgency = "U" Then
                    '                m_lReturn& = m_oBusiness.GetLimitedReportList(r_vLimitedReportList:=m_vLimitedReportList)
                    '            End If

                    m_lReturn = m_oBusiness.GetLimitedReportList(r_vLimitedReportList:=m_vLimitedReportList)
                    ' SET 27/02/2003 - End
            End Select

            'Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

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
        Dim iCount As Integer

        Try

            ' Assign the details to the interface.
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Assign the details from the business object
            ' to the data storage.
            m_lReturn = BusinessToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' Populate folders list box for non empty folders
            ' (reports list box populated by folders click event)
            For Each vElement As String In m_colReportFolders
                If lstDirectories.Items.Contains(vElement) Then
                    Continue For
                End If
                iCount += 1
                If vElement <> "Navigator" Then
                    If m_colReportList(iCount).Count > 0 Then
                        Dim lstDirectories_NewIndex As Integer = -1
                        lstDirectories_NewIndex = lstDirectories.Items.Add(vElement)
                        VB6.SetItemData(lstDirectories, lstDirectories_NewIndex, iCount)
                    End If
                End If
            Next vElement

            ' Hide list box if only one directory
            If lstDirectories.Items.Count < 2 Then
                lblCategory.Visible = False
                lstDirectories.Visible = False
                lblReports.Top = VB6.TwipsToPixelsY(ACLabelTop)
                lstReports.Top = VB6.TwipsToPixelsY(ACListBoxTop)
                lstReports.Height = VB6.TwipsToPixelsY(ACListBoxFullHeight)
            End If

            ' Default to 1st folder
            If lstDirectories.Items.Count > 0 Then
                ListBoxHelper.SetSelectedIndex(lstDirectories, 0)
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim lBusinessDataID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.

            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID)
                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID)
                    ' {* USER DEFINED CODE (End) *}
            End Select

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
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retreive all of the lookup
            ' descriptions for a given lookup type.
            ' The GetLookupDetails function will allow you to do this.
            '
            ' Example:-
            '
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=PMLookupCodeName, _
            ''        ctlLookup:=cmbCodeName)
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        DisplayLookupDetails = PMFalse
            '        Exit Function
            '    End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Sub CreateGrid()
        Dim dtCol As DataColumn = Nothing
        'Create Parameter Data Table object to hold columns and rows
        dataTable = New DataTable("dtParam")

        ' This array method gives larger, incorrect column widths!
        ' Array seems to set a larger default font size!
        '
        '			//Create String array object, initialize array with column names
        '			arrstr			= new string [7];
        '			arrstr[0]		= "Prompt";		
        '			arrstr[1]		= "Value";
        '			arrstr[2]		= "Kind";
        '			arrstr[3]		= "Min";
        '			arrstr[4]		= "Max";
        '			arrstr[5]		= "Parameter";
        '			//Add string array of columns to the DataColumn object 		
        '			for(int i=0; i< 7;i++)
        '			{	
        '				string str		= arrstr[i];
        '				dtCol			= new DataColumn(str);
        '				dtCol.DataType		= System.Type.GetType("System.String");
        '				dtCol.DefaultValue  	= "";
        '				dataTable.Columns.Add(dtCol);		
        '			}
        '			

        dtCol = New DataColumn("Prompt")
        dtCol.DataType = System.Type.[GetType]("System.String")
        dtCol.DefaultValue = ""
        dataTable.Columns.Add(dtCol)

        dtCol = New DataColumn("Value")
        dtCol.DataType = System.Type.[GetType]("System.String")
        dtCol.DefaultValue = ""
        dataTable.Columns.Add(dtCol)

        dtCol = New DataColumn("Kind")
        dtCol.DataType = System.Type.[GetType]("System.String")
        dtCol.DefaultValue = ""
        dataTable.Columns.Add(dtCol)

        dtCol = New DataColumn("Min")
        dtCol.DataType = System.Type.[GetType]("System.String")
        dtCol.DefaultValue = ""
        dataTable.Columns.Add(dtCol)

        dtCol = New DataColumn("Max")
        dtCol.DataType = System.Type.[GetType]("System.String")
        dtCol.DefaultValue = ""
        dataTable.Columns.Add(dtCol)

        dtCol = New DataColumn("Param")
        dtCol.DataType = System.Type.[GetType]("System.String")
        dtCol.DefaultValue = ""
        dataTable.Columns.Add(dtCol)

    End Sub
    ''Private Function GetDefaultValue(ByVal crDefaultValues As CrystalDecisions.Shared.ParameterValues) As String
    ''    ' Test if there are any default values and that they are not null
    ''    If crDefaultValues.Count > 0 AndAlso (crDefaultValues(0) IsNot Nothing) Then
    ''        ' Extract from default values collection and cast
    ''        ' to type ParameterDiscreteValue
    ''        Dim strCollection As String = ""
    ''        Dim theDefaultValue As CrystalDecisions.Shared.ParameterDiscreteValue
    ''        If crDefaultValues.Count > 1 Then
    ''            For Each collection As CrystalDecisions.Shared.ParameterDiscreteValue In crDefaultValues
    ''                strCollection = strCollection + collection.Value + "\"
    ''            Next
    ''            strCollection = strCollection.Substring(0, strCollection.Length - 1)
    ''            Dim newParam As New CrystalDecisions.Shared.ParameterDiscreteValue
    ''            newParam.Value = strCollection
    ''            'crDefaultValues.Add(DirectCast(strCollection, CrystalDecisions.Shared.ParameterDiscreteValue))
    ''            'theDefaultValue = DirectCast(crDefaultValues(crDefaultValues.Count - 1), CrystalDecisions.Shared.ParameterDiscreteValue)
    ''            theDefaultValue = newParam
    ''        Else
    ''            theDefaultValue = DirectCast(crDefaultValues(0), CrystalDecisions.Shared.ParameterDiscreteValue)
    ''        End If
    ''        ' Return default value as a string
    ''        Dim defaultVal As String = theDefaultValue.Value.ToString()
    ''        Return defaultVal
    ''    Else
    ''        Return ""
    ''    End If
    ''End Function
    ''Private Function GetDefaultDate(ByVal crDefaultValues As CrystalDecisions.Shared.ParameterValues) As DateTime
    ''    ' If there is a defalut value and it is not null
    ''    If crDefaultValues.Count > 0 AndAlso (crDefaultValues(0) IsNot Nothing) Then
    ''        ' Extract first element in collection and cast
    ''        ' it to a ParameterDiscreteValue type.
    ''        Dim theDefaultValue As CrystalDecisions.Shared.ParameterDiscreteValue = DirectCast(crDefaultValues(0), CrystalDecisions.Shared.ParameterDiscreteValue)

    ''        ' Cast value property of ParameterDiscreteValue to DateTime object
    ''        Dim defaultVal As DateTime = CType(theDefaultValue.Value, DateTime)
    ''        Return defaultVal
    ''    Else
    ''        Return DateTime.Now
    ''    End If
    ''End Function

    ' ***************************************************************** '
    ' Name: ProduceReport
    '
    ' Description:  Produce Report.
    '
    ' Changes:
    ' TB 12/07/2002:  1) Add progress bar when creating the reports
    ' 2) Add new paramter CrystalError to SendToPrint
    ' TR 28/05/2003:  Added message for "No Printers Installed"
    ' ***************************************************************** '
    Public Function ProduceReport(Optional ByRef ReportType As String = "") As Integer

        Dim result As Integer = 0
        Dim sParamValue As String = ""
        Dim bNavigator As Boolean
        Dim vResultArray(,) As Object = Nothing
        Dim bShowParamForm As Boolean
        Dim sReportName As String = ""
        Dim sDescription As String = String.Empty

        'Use this to save the parameters that were passed in
        '(should be ok awith local scope)
        Dim vKeyParameters(,) As Object = Nothing

        Const ksBranchOption As Integer = 4050
        Const ksInsurerOption As Integer = 4051
        Const AC_REPORT_AUDIT_REPORT_DEBIT_CREDIT As String = "audit_report_for_debits_and_credits"
        Const AC_REPORT_AUDIT_REPORT_FOR_FEES As String = "audit_report_for_fees"
        Const AC_REPORT_COMM_DUE_FROM_DEBTORS As String = "commission_due_from_debtors"
        Const AC_REPORT_FEES_FOR_PAYMENT As String = "fees_for_payment"
        Const AC_REPORT_INCOME_EARNED As String = "income_earned"
        Const AC_REPORT_INCOME_TRANSACTED As String = "income_transacted"
        Const AC_REPORT_NZ_SUFFIX As String = "_NZ"

        Dim sPrintBranchAdd As String = String.Empty
        Dim sPrintInsurerAdd As String = String.Empty
        Dim vValue As String = ""
        Dim obSIRReportScheduler As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        '31/10/2002 - PWC
        'Ensure that we save the key parameters if passed in so they don't get trashed
        'Note - if there are error we'll currently allow params to get trashed
        m_bSaveParams = Information.IsArray(m_vParameters)
        If m_bSaveParams Then

            vKeyParameters = m_vParameters
        End If

        'Destroy existing report object
        'JMK 16/06/2001 - Tom says this is better
        'If (IsObject(m_oReport)) Then
        If Not (m_oReport Is Nothing) Then
            m_oReport = Nothing
        End If
        ' Clear existing properties
        ' Keep parameters as may be passed in
        m_sReportPath = ""
        m_sCompiledReportPath = ""
        m_vDefaultValues = Nothing

        If (m_oBusiness Is Nothing) Then 'PN 2062-Ritu
            initializeObject()
        End If

        With m_oBusiness

            ' If report name known then it was set by Navigator
            If m_sReportName > "" Then
                bNavigator = True

                .ReportName = m_sReportName
            Else
                '31/10/2002 - PWC - Only allow params to be trashed if not saving them
                If Not m_bSaveParams Then

                    m_vParameters = Nothing
                End If

                ' Lookup out reports object and extract the report name

                sReportName = m_colReportList(VB6.GetItemData(lstDirectories, ListBoxHelper.GetSelectedIndex(lstDirectories)))(lstReports.Text).ReportName
                sDescription = m_colReportList(VB6.GetItemData(lstDirectories, ListBoxHelper.GetSelectedIndex(lstDirectories)))(lstReports.Text).Description
                'S4BDAT007 Report Changes These reports are specific for NZ
                Select Case sReportName.ToLower()
                    Case AC_REPORT_AUDIT_REPORT_DEBIT_CREDIT, AC_REPORT_AUDIT_REPORT_FOR_FEES, AC_REPORT_COMM_DUE_FROM_DEBTORS, AC_REPORT_FEES_FOR_PAYMENT, AC_REPORT_INCOME_EARNED, AC_REPORT_INCOME_TRANSACTED

                        m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=vValue)
                        sReportName = sReportName & (IIf(gPMFunctions.ToSafeString(vValue, "0") = "1", AC_REPORT_NZ_SUFFIX, ""))

                End Select

                .ReportName = lstDirectories.Text & "\" & sReportName
                .Description = lstDirectories.Text & "\" & sDescription
                '   End If
            End If
            .PrintReport = m_iPrintReport

            m_lReturn = GetReportPath()

            m_sReportName = .ReportName
            m_sDescription = .Description
            ' JMK 12/07/2001 new variable for ReportName with UserID appended

            m_sUserReportName = m_sReportName & .ReportUniqueKey
            ' LoadReport("c:\Program Files\PM\Sirius Core\Server\Reports\" + m_sReportName + ".rpt", ReportType)
            ' m_sReportName = ""
            ' Return result
            ' Retrieve parameters required
            'LoadReport("c:\Program Files\PM\Sirius Core\Server\Reports\" + m_sReportName + ".rpt")
            ' Return True
            m_lReturn = .GetParameters(r_vParameters:=m_vParameters, r_vDefaultValues:=m_vDefaultValues)
            '31485
            If m_bSaveParams Then
                For icnt As Integer = 0 To m_vParameters.GetUpperBound(0)
                    For jcnt As Integer = 0 To vKeyParameters.GetUpperBound(0)
                        If m_vParameters(icnt, 0) = vKeyParameters(jcnt, 0) Then
                            m_vParameters(icnt, 1) = vKeyParameters(1, jcnt)
                            Exit For
                        End If
                    Next
                Next
            End If
            m_bAllowAll = True
            m_sAllBranch = "1"

            If m_sReportName.ToUpper() = ("Navigator\RemittanceAdvice").ToUpper() Then
                m_lReturn = iPMFunc.GetSystemOption(ksBranchOption, sPrintBranchAdd, g_iSourceID)
                m_lReturn = iPMFunc.GetSystemOption(ksInsurerOption, sPrintInsurerAdd, g_iSourceID)

                For iCount As Integer = m_vParameters.GetLowerBound(0) To m_vParameters.GetUpperBound(0)

                    If gPMFunctions.ToSafeString(CStr(m_vParameters(iCount, 0)), "").ToUpper() = ("print_branch_add").ToUpper() Then

                        m_vParameters(iCount, 1) = gPMFunctions.ToSafeBoolean(sPrintBranchAdd, True)
                    ElseIf gPMFunctions.ToSafeString(CStr(m_vParameters(iCount, 0)), "").ToUpper() = ("print_insurer_add").ToUpper() Then

                        m_vParameters(iCount, 1) = gPMFunctions.ToSafeBoolean(sPrintInsurerAdd, True)
                    End If
                Next
            End If

            If Information.IsArray(m_vParameters) Then
                'Set passed prompts (if any)
                If Information.IsArray(m_vKeyPrompts) Then
                    'Loop through all the passed prompts and find in the parrameters array
                    For i As Integer = m_vKeyPrompts.GetLowerBound(0) To m_vKeyPrompts.GetUpperBound(0)

                        For j As Integer = m_vParameters.GetLowerBound(0) To m_vParameters.GetUpperBound(0)
                            'Match on the name

                            If m_vParameters(j, 0).Equals(m_vKeyPrompts(0, i)) Then
                                'Set the value

                                m_vParameters(j, 3) = m_vKeyPrompts(1, i)
                                Exit For
                            End If
                        Next j

                    Next i

                End If

                'TN20010212 Start
                ' JMK 20010226 - amend 'For' loop, array does not always have 2 dimensions (Subscript out of range error)
                'If (bNavigator = False) Then

                bShowParamForm = False

                'check to see if current value is missing

                For iCount As Integer = m_vParameters.GetLowerBound(0) To m_vParameters.GetUpperBound(0)

                    Dim auxVar As Object = m_vParameters(iCount, 1)

                    If (Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Or CStr(auxVar) = "") And m_sReportName <> "Underwriting\PLICO_certificate" Then
                        bShowParamForm = True
                        Exit For
                    End If
                Next

                If Not bNavigator Or (bNavigator And bShowParamForm) Then
                    'TN20010212 End

                    frmParameters = New frmParameters
                    With frmParameters
                        .Business = m_oBusiness
                        If lstReports.Text <> "" Then
                            .Text = lstReports.Text
                        Else

                            .Text = m_oBusiness.ReportName
                        End If

                        .frmInterface = Me
                        .ReportName = m_oBusiness.ReportName 'lstReports.Text

                        '8.5
                        .AttachToScheduler = m_bAttachToScheduler
                        .ShowInTaskbar = True
                        .Parameters = m_vParameters
                        .DefaultValues = m_vDefaultValues
                        VB6.ShowForm(frmParameters, 1)

                        If Not m_bSaveParams Then m_vParameters = Nothing
                        m_vDefaultValues = Nothing
                        'Developer Guide No. 24
                        m_vParameters = .Parameters

                        m_sFrequency = .Frequency

                        If m_bAttachToScheduler Then
                            If .Status = gPMConstants.PMEReturnCode.PMCancel Then

                                m_lReturn = m_oBusiness.CloseReport()
                                m_sReportName = ""
                                m_sReportPath = ""
                                m_sCompiledReportPath = ""

                                m_vParameters = Nothing
                                m_vDefaultValues = Nothing
                                Return result
                            End If
                            If .Status = gPMConstants.PMEReturnCode.PMOK Then

                                m_lReturn = m_oBusiness.CloseReport()
                                m_sReportPath = ""
                                m_sCompiledReportPath = ""
                                ' Create iPMBReport object
                                Dim temp_obSIRReportScheduler As Object = Nothing
                                m_lReturn = g_oObjectManager.GetInstance(temp_obSIRReportScheduler, "bSIRReportScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)

                                obSIRReportScheduler = temp_obSIRReportScheduler
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("Produce Report", "obSIRReportScheduler.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If

                                '                            'Check Duplicate
                                '                            m_lReturn = obSIRReportScheduler.DigScheduler(v_sReportName:=ToSafeString(lstReports.Text), _
                                ''                                                                            r_iReportSchedulerId:=r_iReportSchedulerId)
                                '                            If (m_lReturn& <> PMTrue) Then
                                '                                RaiseError "Produce Report", "Calling AddSchedulerReport Failed", PMLogError
                                '                            End If
                                '                            If r_iReportSchedulerId > 0 Then
                                '                                MsgBox "Duplicate Found ! Report Already Added to Scheduler", vbInformation + vbOKOnly, "Produce Report"
                                '                                Exit Function
                                '                            End If

                                m_lReturn = obSIRReportScheduler.AddSchedulerReport(v_vParameters:=m_vParameters, v_sReportName:=gPMFunctions.ToSafeString(lstReports.Text), v_sFrequency:=m_sFrequency, v_sReportPath:=m_sReportName)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("Produce Report", "Calling AddSchedulerReport Failed", gPMConstants.PMELogLevel.PMLogError)
                                End If

                                obSIRReportScheduler.Dispose()
                                obSIRReportScheduler = Nothing

                                m_vParameters = Nothing
                                m_vDefaultValues = Nothing
                                m_sReportName = ""
                                Return result
                            End If
                        End If

                        'DC270303 -ISS1911
                        m_lSessionId = .SessionId
                        If .Status = gPMConstants.PMEReturnCode.PMCancel Then
                            result = gPMConstants.PMEReturnCode.PMFalse

                            m_lReturn = m_oBusiness.CloseReport()

                            'DC270303 -ISS1911

                            m_lReturn = m_oBusiness.ClearSessionId(r_lSessionId:=m_lSessionId)

                            m_lReturn = m_oBusiness.DeleteTempReportRecords(lSessionID:=m_lSessionId)
                            m_sReportName = ""
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return result
                        End If
                    End With
                End If
            End If

            If .ReportName = "Daily_Activity_by Scheme.rpt" Then
                'This would work _if_ ReportName was getable from the business object...

                m_lReturn = m_oBusiness.getinsurerid(r_vResultArray:=vResultArray)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    'Check array, then loop through running each report

                    For iCount As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                        ' Process report.

                        m_lReturn = m_oBusiness.SendToPrint(v_sReportTitle:=lstReports.Text, r_sCompiledReportPath:=m_sCompiledReportPath, v_vParameters:=vResultArray, v_sReportOPSubPath:=vResultArray)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_sReportName = ""
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return result
                        End If
                    Next iCount
                End If

            Else

                ' TB 12/07/2002: Display a progress bar to let user know it hasn't crashed
                Try  ' Don't error if it fails
                    m_oProgressBar = New iPMBProgressBarWrapper.Wrapper()

                Catch
                End Try
                '
                ''Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("On Error Goto Label (Err_ProduceReport)")
                If Not (m_oProgressBar Is Nothing) Then
                    m_oProgressBar.Caption = "  Creating  Report . . ."
                    m_oProgressBar.Text = "Reports may take several minutes to produce. " &
                                          "Please wait"
                    m_oProgressBar.StartBar = True
                    iPMFunc.CenterForm(m_oProgressBar.frmLoadAVI)
                End If

                ' Process report.

                m_lReturn = m_oBusiness.SendToPrint(v_sReportTitle:=lstReports.Text, r_sCompiledReportPath:=m_sCompiledReportPath, v_vParameters:=m_vParameters, r_sCrystalErrorLine:="")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'JMK 16/06/2001 - allow user to choose whether to Preview or not if there are no records
                    ' also log message if SendToPrint fails
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        If Not (m_oProgressBar Is Nothing) Then
                            ' TB 12/07/2002: Stop the progress bar
                            m_oProgressBar.StopBar = True
                        End If
                        Dim sHeaderReportName As String
                        If m_sReportName.Contains("\") Then
                            sHeaderReportName = (m_sReportName.Split("\"))(1)
                        Else
                            sHeaderReportName = m_sReportName
                        End If
                        If MessageBox.Show("There is no data currently available for this report" & Strings.Chr(13) & Strings.Chr(10) &
                                           "Do you still wish to preview?", sHeaderReportName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
                            result = gPMConstants.PMEReturnCode.PMNotFound

                            m_lReturn = m_oBusiness.CloseReport()

                            'DC270303 -ISS1911

                            m_lReturn = m_oBusiness.ClearSessionId(r_lSessionId:=m_lSessionId)

                            m_lReturn = m_oBusiness.DeleteTempReportRecords(lSessionID:=m_lSessionId)

                            m_sReportName = ""
                            m_sReportPath = ""
                            m_sCompiledReportPath = ""
                            '31/10/2002 - PWC - Only allow params to be trashed if not saving them
                            If Not m_bSaveParams Then

                                m_vParameters = Nothing
                            End If
                            m_vDefaultValues = Nothing
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            If bNavigator Then
                                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                            End If
                            ' TB 12/07/2002: destroy the progress bar
                            If Not (m_oProgressBar Is Nothing) Then
                                m_oProgressBar = Nothing
                            End If
                            Return result
                        End If
                    Else
                        If Not (m_oProgressBar Is Nothing) Then
                            ' TB 12/07/2002: Stop the progress bar
                            m_oProgressBar.StopBar = True
                        End If
                        'TR - 28/05/2003 - Nasty hack I know, but couldn't
                        'extrapolate this error from the others in the business
                        'object, as it caused problenms with further processing.
                        'May be a problem if Crystal uses this error message
                        'for any other errors we encounter
                        ''If sCrystalError.ToLower() = "error 545: request cancelled by the user." Then
                        ''    MessageBox.Show("No Printers Installed", Application.ProductName)
                        ''Else
                        ''    'Log message if SendToPrint fails
                        ''    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed at SendToPrint: " & sCrystalError & " " & CStr(m_lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceReport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        ''End If
                        result = gPMConstants.PMEReturnCode.PMFalse

                        m_lReturn = m_oBusiness.CloseReport()

                        'DC270303 -ISS1911

                        m_lReturn = m_oBusiness.ClearSessionId(r_lSessionId:=m_lSessionId)

                        m_lReturn = m_oBusiness.DeleteTempReportRecords(lSessionID:=m_lSessionId)
                        m_sReportName = ""
                        m_sReportPath = ""
                        m_sCompiledReportPath = ""

                        m_vParameters = Nothing
                        m_vDefaultValues = Nothing
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        If bNavigator Then
                            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                        End If
                        If Not (m_oProgressBar Is Nothing) Then
                            m_oProgressBar = Nothing
                        End If
                        Return result
                    End If
                End If
            End If

            ' Retrieve file using ActiveX control if Preview required.
            If (m_iPrintReport = PMNavKeyConst.AC_VIEW_ONLY) Or (m_iPrintReport = PMNavKeyConst.AC_PRINT_AND_VIEW) Then
                Dim previewForm As New frmviewReport
                Dim connectionString As String = ""
                Dim reportPath As String = ""
                m_oBusiness.GetReportPath(reportPath)
                With previewForm
                    .ShowInTaskbar = True
                    .ReportFileName = m_sUserReportName ' DirectCast(m_oBusiness.m_oReportDocument, CrystalDecisions.CrystalReports.Engine.ReportDocument).FileName
                    ' .ReportDocument = m_oBusiness.m_oReportDocument
                    m_oBusiness.GetConnectionString(connectionString)
                    .ConnectionString = connectionString
                    .ReportParametersObjects = m_vParameters
                    .Text = Me.Text
                    .ReportPath = reportPath
                    GetReportName()
                    .ReportName = m_sReportName
                    '.PrintReport()
                    '.ShowReport()
                    .WindowState = FormWindowState.Maximized
                    If Not (m_oProgressBar Is Nothing) Then
                        ' TB 12/07/2002: Stop the progress bar
                        m_oProgressBar.StopBar = True
                    End If

                    .ShowDialog(Me)
                    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then ' 0 = success
                    '    'Clearing the variable value PN 19342
                    '    m_sReportName = ""
                    '    hwndPreviewWindow = GetActiveWindow()
                    '    Do While IsWindow(hwndPreviewWindow)
                    '        Sleep(10) 'MKW160204 PN10450
                    '        Application.DoEvents()
                    '    Loop
                    'Else
                    '    'Log message if PrintReport fails
                    '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed at .PrintReport " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="ProduceReport", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    'End If

                End With
            End If
            m_sReportPath = ""
            m_sCompiledReportPath = ""

            m_vParameters = Nothing
            m_vDefaultValues = Nothing

        End With
        ' TB 12/07/2002: Stop the progress bar
        If Not (m_oProgressBar Is Nothing) Then
            m_oProgressBar.StopBar = True
        End If
        ' Must clear as it represents Navigator value if set

        m_lReturn = m_oBusiness.CloseReport()

        'DC270303 -ISS1911

        m_lReturn = m_oBusiness.ClearSessionId(r_lSessionId:=m_lSessionId)

        m_lReturn = m_oBusiness.DeleteTempReportRecords(lSessionID:=m_lSessionId)
        m_sReportName = ""

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        '31/10/2002 - PWC - Restore the passed params
        If m_bSaveParams Then
            m_vParameters = vKeyParameters
        End If

        If bNavigator Then
            m_lStatus = gPMConstants.PMEReturnCode.PMOK
        End If

        ' TB 12/07/2002: destroy the progress bar
        If Not (m_oProgressBar Is Nothing) Then
            m_oProgressBar = Nothing
        End If

        Return result

    End Function
    Private Function GetReportName() As String
        If m_sReportName <> "" Then

            If m_sReportName = "Remittance_Advice" Then
                m_sReportName = "Navigator\Remittance_Advice"
            End If
            'For Issue PN-61851 (By Nitesh Dwivedi as on 20-Jan-2009)
            If m_sReportName = "Reconciliation_Report" Then
                m_sReportName = "Navigator\Reconciliation_Report"
            End If
            'For Issue PN-1878 -  Ritu----------------
            If m_sReportName = "Cheque_Register" Then
                m_sReportName = "Navigator\Cheque_Register"
            End If
            If m_sReportName = "Client_Statement_By_PartyCnt_U" Then
                m_sReportName = "Navigator\Client_Statement_By_PartyCnt_U"
            End If
            If m_sReportName = "Insurer_Payment_Marked_Items" Then
                m_sReportName = "Navigator\Insurer_Payment_Marked_Items"
            End If
            If m_sReportName = "Marked_For_Payment" Then
                m_sReportName = "Navigator\Marked_For_Payment"
            End If
            If m_sReportName = "Marked_For_Reconciliation" Then
                m_sReportName = "Navigator\Marked_For_Reconciliation"
            End If
            'For Issue PN-1749
            If m_sReportName = "Remittance_Advice_Agency" Then
                m_sReportName = "Agency\Remittance_Advice_Agency"
            End If
            'For Issue PM027771
            If m_sReportName = "PLICO_certificate" Then
                m_sReportName = "Underwriting\PLICO_certificate"
            End If
            'For Issue PM028018
            If m_sReportName = "PLICO_LossHistoryLetter" Then
                m_sReportName = "Underwriting\PLICO_LossHistoryLetter"
            End If
        End If
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Dim sIsUnderwriting As String = ""
        Dim sLtdReport As String = ""
        Dim TempCol As Collection
        Dim oReport As iPMBReportPrint.Report

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            ' Populate Collections from Business object
            With m_oBusiness

                m_colReportFolders = .ReportFolders
                ReDim m_colReportList(m_colReportFolders.Count)
                For iCount As Integer = 1 To m_colReportFolders.Count

                    m_colReportList(iCount) = .ReportList(iCount)
                Next iCount
            End With

            'JMK30012001 Limit the Report List collection (U/W hidden option)
            ' SET 27/02/2003 AON44 - include this functionality for Broking
            '    If m_sUnderwritingOrAgency = "U" Then
            'SET 27/02/2003 AON44 - End
            If Information.IsArray(m_vLimitedReportList) Then
                ' check each Report Folder
                For iFolderCount As Integer = 1 To m_colReportFolders.Count
                    TempCol = New Collection()
                    ' check each Report in that Folder
                    For Each vReport As String In m_colReportList(iFolderCount)
                        ' check against the limited list
                        For iLimitCount As Integer = m_vLimitedReportList.GetLowerBound(1) To m_vLimitedReportList.GetUpperBound(1)
                            sLtdReport = CStr(m_vLimitedReportList(0, iLimitCount))
                            If sLtdReport = "0" OrElse sLtdReport = "" Then
                                oReport = Nothing
                            Else
                                If sLtdReport.IndexOf("Commission_Earnt") + 1 Then
                                    'Debug.Print
                                End If

                                ' SET 27/02/2003 AON44 - should be case insensitve
                                '                    If Left(sLtdReport, Len(sLtdReport) - 4) = vReport Then
                                'eck050603 PN4512 Trim to remove trailing spaces
                                If sLtdReport.Substring(0, sLtdReport.Length - 4).Trim().ToUpper() = vReport.Trim().ToUpper() Then
                                    ' Peter Finney 29/06/2003
                                    '   Add the report and it's description to a new object so we
                                    '   have access to both.
                                    oReport = New iPMBReportPrint.Report()

                                    oReport.ReportName = vReport
                                    oReport.Description = CStr(m_vLimitedReportList(1, iLimitCount))

                                    ' Add the item using the description as the key so we can find it again!
                                    TempCol.Add(oReport, oReport.Description)

                                    oReport = Nothing
                                    Exit For
                                End If
                            End If
                        Next iLimitCount
                    Next vReport
                    m_colReportList(iFolderCount) = TempCol
                Next iFolderCount
            Else
                ' no reports available for this user
                TempCol = New Collection()
                For iCount As Integer = 1 To m_colReportFolders.Count
                    m_colReportList(iCount) = TempCol
                Next iCount
            End If
            '    End If
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Try

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

            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' Disable buttons until report selected
            cmdPreview.Enabled = False
            cmdPrint.Enabled = False

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdPreview.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPreviewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdPrint.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPrintButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
            '
            ' Example:-
            '
            '    lblDesc.Caption = GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACDesc, _
            ''        iDataType:=PMResString)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

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
    ' Name: GetLookupValues
    '
    ' Description: Gets all of the lookup values, ready to be used by
    '              the lookup function.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupValues) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupValues() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'commented out to make component work
    ''
    '    ' Gets all of the lookup values.
    ''
    '    ' Check the task.
    '    Select Case (m_iTask)
    '        Case PMAdd
    '            ' Get all of the lookup values.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupAll, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    ''
    '        Case PMEdit
    '            ' Get all of the lookup values with the correct
    '            ' effective date.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupAllEffective, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    ''
    '        Case PMView
    '            ' Get lookup values for viewing only.
    '            m_lReturn& = m_oBusiness.GetLookupValues( _
    ''                iLookupType:=PMLookupSingle, _
    ''                vTableArray:=m_vLookupValues, _
    ''                iLanguageID:=g_iLanguageID%, _
    ''                vResultArray:=m_vLookupDetails)
    '    End Select
    ''
    '    ' Check for errors.
    '    If (m_lReturn& <> PMTrue) Then
    '        GetLookupValues = PMFalse
    ''
    '        ' Log Error.
    '        LogMessagePopup _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to get the lookup values from the business object", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="GetLookupValues"
    ''
    '        Exit Function
    '    End If
    ''
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetLookupDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    '
    'Dim result As Integer = 0
    'Dim lRow As Integer
    'Dim bFoundMatch As Boolean
    '
    ' Lookup value contants.
    'Const ACValueTableName As Integer = 0
    'Const ACValueID As Integer = 1
    'Const ACValueStartPos As Integer = 2
    'Const ACValueNumber As Integer = 3
    '
    ' Lookup detail contants.
    'Const ACDetailKey As Integer = 0
    'Const ACDetailDesc As Integer = 1
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get the lookup values.
    '
    'bFoundMatch = False
    '
    'For 'lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
    ' Check for a match of the table name.
    'If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
    ' Found a match
    'bFoundMatch = True
    'Exit For
    'End If
    'Next lRow
    '
    ' Check if there has been a table match.
    'If Not bFoundMatch Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
    '
    'Return result
    'End If
    '
    ' Using the lookup values, populate the control with
    ' the details from the lookup details array.
    '
    'For 'lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
    ' Add the details to the control.

    'ctlLookup.AddItem(m_vLookupDetails(ACDetailDesc, lCntr))

    'ctlLookup.ItemData(ctlLookup.NewIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))
    '
    'SP150998 - compare long value not string
    ' Check if this is the selected index.
    'If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
    'If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then

    'ctlLookup.ListIndex = ctlLookup.NewIndex
    'End If
    'End If
    '
    'Next lCntr
    '
    ' Check if the selected index is blank. If so,
    ' we set the controls index to zero.
    'If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

    'ctlLookup.ListIndex = 0
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetReportPath
    '
    ' Description: Gets the Report Templates location from the registry.
    '
    ' ***************************************************************** '
    Private Function GetReportPath() As Integer

        Dim result As Integer = 0
        Dim sRegPath As String = ""

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set to LocalMachine/Sirius/Client
            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            ' Location for Exported Reports
            sRegPath = ""
            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="PrntFileDir", r_sSettingValue:=sRegPath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Report Destination directory from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_sReportOutputLocation = sRegPath
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReportPathFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReportPath", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)

    'UPGRADE_NOTE: (7001) The following declaration (cmdExport_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdExport_Click()
    '
    'm_iPrintReport = PMNavKeyConst.AC_EXPORT_TO_HTML
    '
    'm_lReturn = ProduceReport()
    '
    'End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen
        ' If its underwriting use base ScreenHelpID
        ' if it's agency add on 1000
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub

    Private Sub cmdPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPrint.Click

        m_iPrintReport = PMNavKeyConst.AC_PRINT_ONLY

        m_lReturn = ProduceReport()

    End Sub

    Private Sub cmdPreview_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPreview.Click

        m_iPrintReport = PMNavKeyConst.AC_VIEW_ONLY

        m_lReturn = ProduceReport()

    End Sub

    Private Sub lstDirectories_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstDirectories.SelectedIndexChanged

        ' Check which folder selected as list box is sorted
        Dim iFolder As Integer = VB6.GetItemData(lstDirectories, ListBoxHelper.GetSelectedIndex(lstDirectories))

        ' Populate reports list box from collection
        lstReports.Items.Clear()
        For Each vElement As iPMBReportPrint.Report In m_colReportList(iFolder)
            lstReports.Items.Add(vElement.Description)
        Next vElement

        ' Disable buttons
        cmdPreview.Enabled = False
        cmdPrint.Enabled = False

    End Sub

    Private Sub lstReports_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstReports.SelectedIndexChanged

        ' Enable buttons
        cmdPreview.Enabled = True
        cmdPrint.Enabled = True

        ' Set default to View Report
        VB6.SetDefault(cmdPreview, True)

    End Sub

    Private Sub lstReports_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lstReports.DoubleClick

        ' Enable buttons
        cmdPreview.Enabled = True
        cmdPrint.Enabled = True

        m_iPrintReport = PMNavKeyConst.AC_VIEW_ONLY
        m_lReturn = ProduceReport()

    End Sub
    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' SET 26112002 - amendments to show this form in the taskbar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.

            'OK, in this situation the business object has a form.  It needs to be displayed.
            'So we don't create the object on the server, but locally
            'Update - no it doesn't, at least not in the latest version that was nicked

            'Dim temp_m_oBusiness As Object
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oBusiness = temp_m_oBusiness

            '    m_lReturn& = g_oObjectManager.GetInstance( _
            'oObject:=m_oBusiness, _
            'sClassName:="bSIRReportPrint.Business", _
            'vInstanceManager:=PMGetLocalBusiness)

            'TFTemp
            'Set m_oBusiness = CreateObject("bSIRReportPrint.Business")
            'm_lReturn& = m_oBusiness.Initialise( _
            'sUsername:="sa", _
            'sPassword:="", _
            'iUserID:=1, _
            'iSourceID:=1, _
            'iLanguageID:=1, _
            'iCurrencyID:=1, _
            'iLogLevel:=1, _
            'sCallingAppName:="Test")

            ' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    ' Failed to get an instance of the business object.
            '    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

            '    ' Display error stating the problem.

            '    ' Get description from the resource file.

            '    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    ' Display message.
            '    MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

            '    Exit Sub
            'End If

            ' JMK 30012001 (U/W hidden option)
            ' get system option from business object

            ' JMK 23022001 - deactivate hidden option until there's time to test it properly
            ' JMK 23032001 - reactivated

            'm_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency
            '' m_sUnderwritingOrAgency = "X"

            '' Create an instance of the general interface object.
            'm_oGeneral = New iPMBReportPrint.General()

            '' Call the initialise method passing this interface
            '' and the business object as parameters.
            'm_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            '' Check for errors.
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            '    Exit Sub
            'End If

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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Forms load event.

        Try

            ' SET 26112002 - amendments to show this form in the taskbar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            'If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            '    ' We have already encountered an error,
            '    ' so we MUST exit now.
            '    Exit Sub
            'End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBReportPrint.General

            m_lReturn = m_oGeneral.Initialise(
                   frmInterface:=Me,
                   oBusiness:=m_oBusiness)

            ' Set the business keys.
            With m_oBusiness

                .ReportName = m_sReportName

                .PrintReport = m_iPrintReport
            End With

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

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

            'Developer Guide No. 19 (No Solution)
            If UnloadMode <> vbFormCode Then

                'Set the interface status, so that it can behave like cancel... PN 14834
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                If m_oGeneral Is Nothing Then
                    m_oGeneral = New iPMBReportPrint.General
                End If

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                Else
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Me.Hide()

                    Exit Sub

                End If

            End If

            ' Destroy the report object
            If Information.IsReference(m_oReport) Then
                m_oReport = Nothing
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

    'UPGRADE_NOTE: (7001) The following declaration (cmdOK_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdOK_Click()
    '
    ' Click event of the OK button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMOK
    '
    ' Check mandatory controls have been entered into.
    'm_lReturn = m_oFormFields.CheckMandatoryControls()
    '
    ' Check for errors
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = m_oGeneral.ProcessCommand()
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdNavigate_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdNavigate_Click()
    '
    ' Click event of the Navigate button.
    '
    'Try 
    '
    ' Set the interface status.
    'm_lStatus = gPMConstants.PMEReturnCode.PMNavigate
    '
    ' Process the next set of actions depending
    ' upon the interface task etc.
    'm_lReturn = m_oGeneral.ProcessCommand()
    '
    ' Check the return value.
    'If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
    ' Everything OK, so we can hide the interface.
    'Me.Hide()
    'End If
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub
    'PN30098 - Datasure
    Private Function GetBaseCurrencySymbol() As Integer
        Dim result As Integer = 0
        Dim oBusiness As bSIRInsuranceFile.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRInsuranceFile.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oBusiness.GetBranchBaseCurrency(vSourceID:=g_iSourceID, iCurrencyID:=m_iBaseCurrencyID, sCurSymbol:=m_sCurrencySymbol)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Branch Base Currency", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrencySymbol ", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            oBusiness = Nothing

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get currency symbol of selected branch", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseCurrencySymbol", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Sub CallLoad()
        Try

            ' SET 26112002 - amendments to show this form in the taskbar
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

            'Dim temp_m_oBusiness As Object
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReportPrint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oBusiness = temp_m_oBusiness

            If (m_oBusiness Is Nothing) Then 'PN 2062-Ritu
                initializeObject()
            End If

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the business keys.

            With m_oBusiness

                .ReportName = m_sReportName

                .PrintReport = m_iPrintReport
            End With

            'Create an instance of the general interface object.
            m_oGeneral = New iPMBReportPrint.General

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.InitialiseParameters(frmParameters:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()

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

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    '    Private Function ReportExport() As Integer
    '        Dim viewer = New Microsoft.Reporting.WinForms.ReportViewer With {
    '        .ProcessingMode = ProcessingMode.Local
    '        }

    '        Dim localReport As LocalReport = viewer.LocalReport
    '        localReport.ReportPath = m_sCompiledReportPath
    '        ' localReport.LoadReportDefinition()(Stream, From _ In _ Select _)
    '        localReport.DataSources.Clear()

    '        localReport.DataSources.Add(New ReportDataSource With {
    '.Name = DataSet.Name,
    '        .Value = _(USE, dataTable, THAT, HAS, THE, SAME, [STRUCTURE], THAN, YOUR, DATASOURCE)
    '    })
    '        Dim warningslocal As Warning() = Nothing
    '        Dim encoding As String
    '        Dim streamIds As String()
    '        Dim mimeType As String
    '        Dim extension As String
    '        Dim result = localReport.Render(Format, deviceInfo, mimeType, encoding, extension, streamIds, warningslocal)
    '    End Function
    '    ' PRIVATE Events (End)
End Class
