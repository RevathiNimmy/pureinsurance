Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 09/06/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURITax.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Main Tax Details
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskCnt As Integer
    Private m_sDescription As String = ""

    ' Stores the search data from the business object.
    Private m_vRITax(,) As Object
    Private m_lSelectedItem As Integer
    ' Keep this here so we don't risk any corruption with formatting and unformatting
    Private m_cTaxValue As Decimal

    ' Are we refreshing?
    Private m_bRefresh As Boolean


    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property InsuranceFileCnt() As String
        Set(ByVal Value As String)
            m_lInsuranceFileCnt = CInt(Value)
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

    Public WriteOnly Property RiskCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRiskCnt = Value
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

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Static bAlreadyRun As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the interface.
            lblRITax.Text = IIf(g_bRiskFlag, "Risk:", "Insurance File:")
            txtDescription.Text = m_sDescription

            ' Clear list
            lvwRITax.Items.Clear()
            cmdEdit.Enabled = False

            ' Check for data
            If Information.IsArray(m_vRITax) Then
                For lRow As Integer = m_vRITax.GetLowerBound(1) To m_vRITax.GetUpperBound(1)
                    ' Tax group
                    With lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), "")
                        ' Group sequence
                        ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 1).Text = CStr(m_vRITax(ACRSequence, lRow))
                        ' Tax band description
                        ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 2).Text = CStr(m_vRITax(ACRDescription, lRow))
                        ' Tax Amount
                        ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 3).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vRITax(ACRTaxValue, lRow))
                        ' Tax basis
                        Select Case m_vRITax(ACRCalcBasis, lRow)
                            Case ACCalcBasisRunningTotal
                                ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 4).Text = "Running Total"
                            Case ACCalcBasisSumInsuredChange
                                ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 4).Text = "Sum Insured Change"
                            Case ACCalcBasisSumInsured
                                ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 4).Text = "Sum Insured"
                            Case Else
                                ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 4).Text = "Premium"
                        End Select
                        ' Rate
                        If CBool(m_vRITax(ACRIsValue, lRow)) Then
                            ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 5).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vRITax(ACRTaxRate, lRow))
                        Else
                            ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 5).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vRITax(ACRTaxRate, lRow), -5)
                        End If
                        ' Class of Business
                        ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 6).Text = CStr(m_vRITax(ACRClassOfBusiness, lRow))
                        ' Country
                        ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 7).Text = CStr(m_vRITax(ACRCountry, lRow))
                        ' State
                        ListViewHelper.GetListViewSubItem(lvwRITax.Items.Add("Tax" & lRow, CStr(m_vRITax(ACRTaxGroup, lRow)), ""), 8).Text = CStr(m_vRITax(ACRState, lRow))
                        ' Row ID

                        .Tag = CStr(lRow)
                    End With
                Next lRow
            End If

            ' Size the list
            'TODo at run time
            'ListViewAutoSize(lvwList:=lvwRITax)

            ' If no taxes are displayed and this is the screen loading,
            ' clear array so calling procedure will unload form.
            If (lvwRITax.Items.Count = 0) And Not bAlreadyRun Then
                m_vRITax = Nothing
            End If

            'TR - This flag is used to decide whether or not to delete the array
            'if the list is empty. If the list is empty when the screen is first
            'loaded then the array must be deleted for the roadmap to "appear" to
            'skip this screen. Once it is loaded and the user can see it, the
            'array must be kept even if the list is blank as this would mean that
            'there are records to be delete when the user clicks the OK button.
            bAlreadyRun = True

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Recalculate a tax as it's method changes
    ' ***************************************************************** '
    Private Function CalculateTaxes() As Integer

        Dim result As Integer = 0
        Dim lCalcBasis As Integer
        Dim bIsValue As Boolean
        Dim dPercentage As Double
        Dim cFixedRate, cBasisValue As Decimal
        Dim bIsRounded, bAllowTaxCredit As Boolean
        Dim cTaxValue As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't do this if we are refreshing the screen
            If m_bRefresh Then
                Return result
            End If

            ' Get new values
            For lCalcBasis = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
                If optBasis(lCalcBasis).Checked Then
                    Exit For
                End If
            Next lCalcBasis
            bIsValue = (chkIsValue.CheckState = CheckState.Checked)

            dPercentage = CDbl(m_oFormFields.UnformatControl(txtPercentage))

            cFixedRate = CDec(m_oFormFields.UnformatControl(txtValue))

            cBasisValue = CDec(m_oFormFields.UnformatControl(txtBasisValue))
            bIsRounded = (chkRounded.CheckState = CheckState.Checked)
            bAllowTaxCredit = (chkAllowCredit.CheckState = CheckState.Checked)

            ' Calculate through business object

            m_lReturn = m_oBusiness.CalculateTax(vPremium:=m_vRITax(ACRPremium, m_lSelectedItem), vSumInsured:=m_vRITax(ACRSumInsured, m_lSelectedItem), vSumInsuredChange:=m_vRITax(ACRSumInsuredChange, m_lSelectedItem), vRunningTotal:=m_vRITax(ACRRunningTotal, m_lSelectedItem), vCalcBasis:=lCalcBasis, vIsValue:=bIsValue, vPercentage:=dPercentage, vFixedRate:=cFixedRate, vBasisValue:=cBasisValue, vIsRounded:=bIsRounded, vAllowTaxCredit:=bAllowTaxCredit, rTaxValue:=cTaxValue)

            ' Store and show new value
            m_cTaxValue = cTaxValue
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, cTaxValue)

            Return result

        Catch


            ' If we failed default tax to zero
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, 0)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Commit changes to array and refresh listview with new data
    ' ***************************************************************** '
    Private Function DataRefresh() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataRefresh"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Data is manually changed
            m_vRITax(ACRIsManuallyChanged, m_lSelectedItem) = 1
            ' Calculation basis
            For lCount As Integer = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
                If optBasis(lCount).Checked Then
                    m_vRITax(ACRCalcBasis, m_lSelectedItem) = lCount
                    Exit For
                End If
            Next lCount
            ' Is value
            m_vRITax(ACRIsValue, m_lSelectedItem) = chkIsValue.CheckState
            ' Rate
            If chkIsValue.CheckState = CheckState.Checked Then

                m_vRITax(ACRTaxRate, m_lSelectedItem) = m_oFormFields.UnformatControl(txtValue)
            Else

                m_vRITax(ACRTaxRate, m_lSelectedItem) = m_oFormFields.UnformatControl(txtPercentage)
            End If
            ' Basis value

            m_vRITax(ACRBasisValue, m_lSelectedItem) = m_oFormFields.UnformatControl(txtBasisValue)
            ' Is rounded
            m_vRITax(ACRIsSIRounded, m_lSelectedItem) = chkRounded.CheckState
            ' Allow tax credits?
            m_vRITax(ACRAllowTaxCredit, m_lSelectedItem) = chkAllowCredit.CheckState

            ' Tax value
            m_vRITax(ACRTaxValue, m_lSelectedItem) = m_cTaxValue

            ' If value is zero mark tax as deleted
            If m_cTaxValue = 0 Then
                m_vRITax(ACRIsDeleted, m_lSelectedItem) = 1
            End If


            ' Now we have stored the new values we need to refresh the entire tax list to account
            ' for possible changes in sequential taxes...

            lReturn = m_oBusiness.CalculateTaxes(vTaxArray:=m_vRITax)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CalculateTaxes", "Unable to recalculate taxes")
            End If


            ' As anything from just this one to all taxes may have been affected by this change
            ' refresh the entire grid
            lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BusinessToInterface", "Unable to refresh tax list")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here


            ' This is for debugging only



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Description: Populate Tax Band Rate Details storage.
    ' ***************************************************************** '
    Private Function DataToDetail() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_bRefresh = True

            ' Check which item is selected

            m_lSelectedItem = Convert.ToString(lvwRITax.Items.Item(lvwRITax.FocusedItem.Index).Tag)

            ' Summary fields
            txtTaxBand.Text = gPMFunctions.NullToString(m_vRITax(ACRDescription, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtSumInsured, m_vRITax(ACRSumInsured, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtSumInsuredChange, m_vRITax(ACRSumInsuredChange, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtOriginalSumInsured, m_vRITax(ACROriginalSumInsured, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtPremium, m_vRITax(ACRPremium, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtRunningTotal, m_vRITax(ACRRunningTotal, m_lSelectedItem))

            ' Calculation
            optBasis(CInt(m_vRITax(ACRCalcBasis, m_lSelectedItem))).Checked = True
            ' Is Value?
            chkIsValue.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(m_vRITax(ACRIsValue, m_lSelectedItem))))
            ' Precentage/Rate/Value
            m_lReturn = m_oFormFields.FormatControl(txtPercentage, m_vRITax(ACRTaxRate, m_lSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(txtValue, m_vRITax(ACRTaxRate, m_lSelectedItem))
            ' Basis value
            m_lReturn = m_oFormFields.FormatControl(txtBasisValue, m_vRITax(ACRBasisValue, m_lSelectedItem))
            ' Is rounded?
            chkRounded.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(m_vRITax(ACRIsSIRounded, m_lSelectedItem))))
            ' Allow tax credit?
            chkAllowCredit.CheckState = Math.Abs(CInt(gPMFunctions.ToSafeBoolean(m_vRITax(ACRAllowTaxCredit, m_lSelectedItem))))

            ' Filters
            txtCountry.Text = gPMFunctions.NullToString(m_vRITax(ACRCountry, m_lSelectedItem))
            txtState.Text = gPMFunctions.NullToString(m_vRITax(ACRState, m_lSelectedItem))
            txtCOB.Text = gPMFunctions.NullToString(m_vRITax(ACRClassOfBusiness, m_lSelectedItem))

            ' Set tax value
            m_lReturn = m_oFormFields.FormatControl(txtTaxValue, m_vRITax(ACRTaxValue, m_lSelectedItem))

            ' Set visible states
            SetFormControls()

            ' Set enabled states
            optBasis(0).Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            optBasis(1).Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            optBasis(2).Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            optBasis(3).Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            chkIsValue.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            txtValue.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            txtPercentage.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            txtBasisValue.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            chkRounded.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)
            chkAllowCredit.Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit)

            m_bRefresh = False
            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_bRefresh = False

            Return gPMConstants.PMEReturnCode.PMError



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Display all language specific captions.
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            Me.Text = "Taxes"


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If g_bRiskFlag Then
                SSTabHelper.SetTabCaption(tabMainTab, 0, "&1 - Risk Tax")
            Else
                SSTabHelper.SetTabCaption(tabMainTab, 0, "&1 - Insurance File Tax")
            End If
            SSTabHelper.SetTabCaption(tabDetailTab, 0, "&2 - Details")

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell it what we're getting
            If g_bRiskFlag Then

                m_oBusiness.RiskCnt = m_lRiskCnt

                m_lReturn = m_oBusiness.GetRiskTax(r_vRiskTax:=m_vRITax, r_sDescription:=m_sDescription, iTask:=m_iTask)
            Else

                m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

                m_lReturn = m_oBusiness.GetInsuranceFileTax(r_vInsuranceFileTax:=m_vRITax, r_sDescription:=m_sDescription, iTask:=m_iTask)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Updates all business members from the interface details.
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we are saving at risk level or at insurance file level
            If g_bRiskFlag Then
                If Information.IsArray(m_vRITax) Then

                    m_oBusiness.RiskCnt = m_lRiskCnt

                    m_lReturn = m_oBusiness.UpdateRiskTax(v_vRiskTax:=m_vRITax)
                End If
            Else
                If Information.IsArray(m_vRITax) Then

                    m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

                    m_lReturn = m_oBusiness.UpdateInsuranceFileTax(v_vInsuranceFileTax:=m_vRITax)
                End If
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Sets the rules for validating fields.
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

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
            ''                       lMandatory:=<PMNonMandatory or PMNonMandatory)
            '
            '        'Error checking
            '        If m_lReturn <> PMTrue Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsured, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsuredChange, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOriginalSumInsured, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPremium, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRunningTotal, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtValue, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBasisValue, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTaxValue, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' ***************************************************************** '
    ' Description: Sets appropriate visibility for form controls.
    ' ***************************************************************** '
    Private Sub SetFormControls()

        Dim lBasis As Integer

        ' Get the calculation basis
        For lCount As Integer = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
            If optBasis(lCount).Checked Then
                lBasis = lCount
                Exit For
            End If
        Next

        ' Set control properties
        txtPercentage.Visible = (chkIsValue.CheckState <> CheckState.Checked)
        txtValue.Visible = (chkIsValue.CheckState = CheckState.Checked)

        lblPer.Visible = (lBasis = 1 Or lBasis = 2) And (chkIsValue.CheckState = CheckState.Checked)
        txtBasisValue.Visible = lblPer.Visible
        lblOfSI.Visible = lblPer.Visible
        chkRounded.Visible = lblPer.Visible

        ' Set appropriate rate caption
        If chkIsValue.CheckState Then
            lblPercentage.Text = "Value:"
        Else
            lblPercentage.Text = "Rate:"
        End If

    End Sub

    ' ***************************************************************** '
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display the ListView Tab
            tabMainTab.Visible = True
            tabDetailTab.Visible = False
            cmdEdit.Enabled = False

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the column widths for the search list.
            lvwRITax.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwRITax.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwRITax.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(2400))
            lvwRITax.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwRITax.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwRITax.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1500))
            lvwRITax.Columns.Item(6).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwRITax.Columns.Item(7).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwRITax.Columns.Item(8).Width = CInt(VB6.TwipsToPixelsX(1800))

            Return result

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
    End Function


    Private Sub chkAllowCredit_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowCredit.CheckStateChanged
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub chkIsValue_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsValue.CheckStateChanged
        ' Display appropriate fields
        SetFormControls()
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub chkRounded_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRounded.CheckStateChanged
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                If Me.Visible Then Me.Hide()
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        Try

            ' Show main details
            tabDetailTab.Visible = False
            tabMainTab.Visible = True

            cmdCancel.Enabled = True
            cmdOK.Enabled = True

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Detail cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDetailCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        Try

            ' Store data and update main tab
            m_lReturn = DataRefresh()

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Show main details
                tabDetailTab.Visible = False
                tabMainTab.Visible = True

                cmdCancel.Enabled = True
                cmdOK.Enabled = True
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Detail OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDetailOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        Try

            ' Copy data to controls
            m_lReturn = DataToDetail()

            ' Show tax details
            tabMainTab.Visible = False
            tabDetailTab.Visible = True

            cmdOK.Enabled = False
            cmdCancel.Enabled = False

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the edit command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Friend Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If Me.Visible Then Me.Hide()
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub



    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim bApplyTaxes, bTaxesSwitchedOff As Boolean

        Dim sMessage, sTitle As String

        ' Forms load event.
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via  the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRITax.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem. Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Set the business keys.

            m_oBusiness.RiskCnt = m_lRiskCnt

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMURITax.General()

            ' Call the initialise method passing this interface and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            ' Check options (System & Risk level) before applying taxes.

            m_lReturn = m_oBusiness.ApplyTaxes(m_lInsuranceFileCnt, m_lRiskCnt, bApplyTaxes, bTaxesSwitchedOff)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMOK
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' If we are not applying taxes delete any that may already exist
            If bTaxesSwitchedOff Then

                m_lReturn = m_oBusiness.DeleteAllTaxes(m_lInsuranceFileCnt)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMOK
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            ElseIf Not bApplyTaxes Then

                m_lReturn = m_oBusiness.DeleteTaxes(m_lInsuranceFileCnt, m_lRiskCnt)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMOK
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_lReturn = m_oGeneral.GetInterfaceDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'TR - If there are no Taxes in the array, set value to PMOk so that caller unlaods this form
            If Not Information.IsArray(m_vRITax) Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMOK
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

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                '    ' Process the next set of actions depending upon the interface task etc.
                m_lReturn = m_oGeneral.ProcessCommand()

                '    ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    eventArgs.Cancel = True

                    ' Do not procced with the interface termination.
                    Cancel = 1
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Destroy the instance of the form control object from memory.
            m_oFormFields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            eventArgs.Cancel = Cancel <> 0
        End Try

    End Sub


    Private Sub lvwRITax_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRITax.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRITax.Columns(eventArgs.Column)

        Static lOrder As SortOrder
        Static lLastCol As Integer

        If lLastCol <> ColumnHeader.Index + 1 Then
            lLastCol = ColumnHeader.Index + 1
            lOrder = SortOrder.Ascending
        Else
            lOrder = IIf(lOrder = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)
        End If

        Select Case lLastCol
            Case 1, 3, 5, 7, 8, 9
                ListViewHelper.SetSortedProperty(lvwRITax, False)
                ListViewHelper.SetSortKeyProperty(lvwRITax, lLastCol - 1)
                ListViewHelper.SetSortOrderProperty(lvwRITax, lOrder)
                ListViewHelper.SetSortedProperty(lvwRITax, True)
            Case 2, 4, 6
                'TODO Check at runtime
                '	ListViewSortByValue(lvwRITax, lLastCol - 1, lOrder, False, True)
        End Select

    End Sub

    Private Sub lvwRITax_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRITax.DoubleClick
        ' If the edit button is enabled call it
        If cmdEdit.Enabled Then
            cmdEdit_Click(cmdEdit, New EventArgs())
        End If
    End Sub

    Private Sub lvwRITax_ItemClick(ByVal Item As ListViewItem)
        ' Enable this button as long as we are not in view mode
        cmdEdit.Enabled = (Task <> gPMConstants.PMEComponentAction.PMView)
    End Sub


    Private isInitializingComponent As Boolean
    Private Sub optBasis_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optBasis_2.CheckedChanged, _optBasis_3.CheckedChanged, _optBasis_1.CheckedChanged, _optBasis_0.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            ' Display appropriate fields
            SetFormControls()
            ' Recalculate
            m_lReturn = CalculateTaxes()
        End If
    End Sub


    Private Sub txtBasisValue_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBasisValue.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub txtBasisValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBasisValue.Enter
        m_lReturn = m_oFormFields.GotFocus(txtBasisValue)
    End Sub

    Private Sub txtBasisValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBasisValue.Leave
        m_lReturn = m_oFormFields.LostFocus(txtBasisValue)
    End Sub


    Private Sub txtPercentage_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub txtPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(txtPercentage)
    End Sub

    Private Sub txtPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Leave
        m_lReturn = m_oFormFields.LostFocus(txtPercentage)
        ' If blank, set to 0
        If txtPercentage.Text.Trim() = "" Then
            m_lReturn = m_oFormFields.FormatControl(txtPercentage, 0)
        End If
    End Sub


    Private Sub txtValue_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        ' Recalculate
        m_lReturn = CalculateTaxes()
    End Sub

    Private Sub txtValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Enter
        m_lReturn = m_oFormFields.GotFocus(txtValue)
    End Sub

    Private Sub txtValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtValue.Leave
        m_lReturn = m_oFormFields.LostFocus(txtValue)
        ' If blank, set to 0
        If txtValue.Text.Trim() = "" Then
            m_lReturn = m_oFormFields.FormatControl(txtValue, 0)
        End If
    End Sub
End Class
