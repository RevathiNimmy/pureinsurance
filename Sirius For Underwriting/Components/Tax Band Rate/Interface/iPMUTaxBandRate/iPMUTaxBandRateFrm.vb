Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
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
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lTaxBandId As Integer
    Private m_sCode As String = ""
    Private m_sDescription As String = ""
    Private m_lItemsFound As Integer

    'Variables to store data taken from the List View
    Private m_iAction As gPMConstants.PMEComponentAction
    Private m_lTaxBandRateId As Integer
    Private m_sRateCode As String = ""
    Private m_lRateCaptionID As Integer
    Private m_sRateDescription As String = ""
    Private m_lIsDeleted As Integer
    Private m_lIsValue As Integer
    Private m_lRate As Double
    Private m_lSumInsuredValue As Integer
    Private m_lCalcBasis As Integer
    Private m_bSuppressDecimalValues As Boolean

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUTaxBandRate.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Stores the search data from the business object.
    Private m_vTaxBandRate(,) As Object

    Private m_iCurrencyID As Integer
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String


    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
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

    Public WriteOnly Property TaxBandId() As Integer
        Set(ByVal Value As Integer)
            m_lTaxBandId = Value
        End Set
    End Property

    Public WriteOnly Property Description() As String
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property

    Public WriteOnly Property Code() As String
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property

    Public WriteOnly Property UniqueId() As String
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public WriteOnly Property ScreenHierarchy() As String
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property
    Public Property IsSuppressDecimalValues() As Boolean
        Get
            Return m_bSuppressDecimalValues
        End Get
        Set(ByVal Value As Boolean)
            m_bSuppressDecimalValues = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    ' ***************************************************************** '
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRatePercentage, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRateValue, lFieldType:=gPMConstants.PMEDataType.PMDecimal, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSumInsuredValue, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
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

    ''' <summary>
    ''' Populate Tax Band Rate Details storage
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataToDetail() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nSelectedItem As Integer
        Dim oPMSystem As bPMSystem.Business
        Try

            ' get the selected item
            nSelectedItem = Convert.ToString(lvwTaxBandRate.FocusedItem.Tag)

            ' Code
            m_sCode = CStr(m_vTaxBandRate(ACRCode, nSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)
            ' Description
            m_sRateDescription = CStr(m_vTaxBandRate(ACRDescription, nSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_sRateDescription)
            ' Effective Date
            m_dtEffectiveDate = CDate(m_vTaxBandRate(ACREffectiveDate, nSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)

            ' Calculation Basis
            m_lCalcBasis = CInt(m_vTaxBandRate(ACRCalcBasis, nSelectedItem))
            optBasis(m_lCalcBasis).Checked = True
            ' Is value
            chkIsValue.CheckState = CInt(m_vTaxBandRate(ACRIsValue, nSelectedItem))
            ' Rate (value and/or percentage)
            m_lRate = CDbl(m_vTaxBandRate(ACRRate, nSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRateValue, vControlValue:=m_lRate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRatePercentage, vControlValue:=m_lRate)
            ' Sum insured value
            m_lSumInsuredValue = CInt(m_vTaxBandRate(ACRSumInsuredValue, nSelectedItem))
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSumInsuredValue, vControlValue:=m_lSumInsuredValue)
            ' Rounded?
            chkRounded.CheckState = ToSafeInteger(m_vTaxBandRate(ACRSumInsuredRounded, nSelectedItem))
            ' Tax credit?
            chkAllowCredit.CheckState = ToSafeInteger(m_vTaxBandRate(ACRAllowTaxCredit, nSelectedItem))

            ' Affected transactions
            chkNB.CheckState = ToSafeInteger(m_vTaxBandRate(ACRNB, nSelectedItem))
            chkAMTA.CheckState = ToSafeInteger(m_vTaxBandRate(ACRAMTA, nSelectedItem))
            chkRMTA.CheckState = ToSafeInteger(m_vTaxBandRate(ACRRMTA, nSelectedItem))
            chkCANC.CheckState = ToSafeInteger(m_vTaxBandRate(ACRCANC, nSelectedItem))
            chkREN.CheckState = ToSafeInteger(m_vTaxBandRate(ACRREN, nSelectedItem))

            ' Additional transaction types
            chkTTRI.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTRI, nSelectedItem))
            chkTTRIC.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTRIC, nSelectedItem))
            chkTTAC.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTAC, nSelectedItem))
            chkTTF.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTF, nSelectedItem))
            chkTTCP.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTCP, nSelectedItem))
            chkTTCS.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTCS, nSelectedItem))
            chkTTCR.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTCR, nSelectedItem))
            chkTTI.CheckState = ToSafeInteger(m_vTaxBandRate(ACRTTI, nSelectedItem))

            ' Filters
            cboCountry.ItemId = ToSafeLong(m_vTaxBandRate(ACRCountryID, nSelectedItem))
            cboState.ItemId = ToSafeLong(m_vTaxBandRate(ACRStateID, nSelectedItem))
            cboCOB.ItemId = ToSafeLong(m_vTaxBandRate(ACRCOBID, nSelectedItem))

            ' Currency
            m_iCurrencyID = ToSafeLong(m_vTaxBandRate(ACRCurrencyID, nSelectedItem))
            If m_iCurrencyID = 0 Then
                'Default to system currency
                Dim temp_oPMSystem As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oPMSystem, "bPMSystem.Business", vInstanceManager:=PMGetViaClientManager)
                oPMSystem = temp_oPMSystem
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = oPMSystem.GetDetails(vMessageID:=1)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

                m_lReturn = oPMSystem.GetNext(iCurrencyID:=m_iCurrencyID)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If
            End If
            cboCurrency.CurrencyId = m_iCurrencyID
            chkUseForRefundWhenexpired.Checked = ToSafeBoolean(m_vTaxBandRate(ACRUseForRefundWhenExpired, nSelectedItem))
            chkUseForBackdatedNB.Checked = ToSafeBoolean(m_vTaxBandRate(ACRUseForBackdatedNB, nSelectedItem))
            chkTTRIPR.Checked = ToSafeBoolean(m_vTaxBandRate(kIsRIPaymentsRecoveries, nSelectedItem))

            If chkTTRIPR.Checked Then
                chkTTRI.Checked = False
                chkTTRI.Enabled = False
                chkTTRIC.Checked = False
                chkTTRIC.Enabled = False
            Else
                chkTTRI.Enabled = True
                chkTTRIC.Enabled = True
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Clear Treaty Party Details storage
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClearDetail() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            ' Update the Detail details.
            txtCode.Text = m_sCode
            txtDescription.Text = ""
            m_lRateCaptionID = 0
            m_sRateDescription = ""
            m_dtEffectiveDate = DateTime.Today
            m_lIsDeleted = 0
            m_lIsValue = 0
            m_lRate = 0
            m_lCalcBasis = 0
            m_lSumInsuredValue = 0

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtEffectiveDate)

            ' Calculation
            optBasis(m_lCalcBasis).Checked = True
            chkIsValue.CheckState = CheckState.Unchecked
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRateValue, vControlValue:=m_lRate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRatePercentage, vControlValue:=m_lRate)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSumInsuredValue, vControlValue:=m_lSumInsuredValue)
            chkRounded.CheckState = CheckState.Unchecked
            chkAllowCredit.CheckState = CheckState.Unchecked

            ' Affected transactions
            chkNB.CheckState = False
            chkAMTA.CheckState = False
            chkRMTA.CheckState = False
            chkCANC.CheckState = False
            chkREN.CheckState = False

            ' Additional transaction types
            chkTTRI.CheckState = False
            chkTTRIC.CheckState = False
            chkTTAC.CheckState = False
            chkTTF.CheckState = False
            chkTTCP.CheckState = False
            chkTTCS.CheckState = False
            chkTTCR.CheckState = False
            chkTTI.CheckState = False
            chkTTRIPR.CheckState = CheckState.Unchecked
            ' Filters

            cboCountry.FirstItem = "(None)"
            cboState.FirstItem = "(None)"
            cboCOB.FirstItem = "(Any)"
            cboCountry.ItemId = 0
            cboState.ItemId = 0
            cboCOB.ItemId = 0
            cboCurrency.ListIndex = 0

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the Detail details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' Populate Treaty Party Refresh storage.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DataRefresh() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nSelectedItem As Integer
        Dim sCode As String = ""

        Try
            m_sCode = txtCode.Text
            m_sRateDescription = txtDescription.Text
            m_dtEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))
            m_lIsValue = chkIsValue.CheckState

            If m_lIsValue Then
                If Len(txtRateValue.Text) Then
                    m_lRate = CDbl(m_oFormFields.UnformatControl(txtRateValue))
                Else
                    m_lRate = 0
                End If
            Else
                If Len(txtRatePercentage.Text) Then
                    m_lRate = CDbl(m_oFormFields.UnformatControl(txtRatePercentage))
                Else
                    m_lRate = 0
                End If
            End If

            If Len(txtSumInsuredValue.Text) Then
                m_lSumInsuredValue = CInt(m_oFormFields.UnformatControl(txtSumInsuredValue))
            Else
                m_lSumInsuredValue = 0
            End If

            For nCount As Integer = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
                If optBasis(nCount).Checked Then
                    m_lCalcBasis = nCount
                    Exit For
                End If
            Next

            m_iCurrencyID = cboCurrency.CurrencyId
            Select Case m_iAction
                Case PMEComponentAction.PMAdd
                    If Information.IsArray(m_vTaxBandRate) Then
                        nSelectedItem = m_vTaxBandRate.GetUpperBound(1) + 1
                        ReDim Preserve m_vTaxBandRate(ACRMaxArray, nSelectedItem)
                    Else
                        nSelectedItem = 0
                        ReDim m_vTaxBandRate(ACRMaxArray, nSelectedItem)
                    End If
                    sCode = m_sCode

                Case PMEComponentAction.PMCopy
                    nSelectedItem = m_vTaxBandRate.GetUpperBound(1)
                    sCode = m_sCode

                Case PMEComponentAction.PMEdit

                    nSelectedItem = Convert.ToString(lvwTaxBandRate.Items.Item(lvwTaxBandRate.FocusedItem.Index).Tag)
                    sCode = m_sCode

                Case PMEComponentAction.PMDelete

                    nSelectedItem = Convert.ToString(lvwTaxBandRate.Items.Item(lvwTaxBandRate.FocusedItem.Index).Tag)
                    sCode = ""
            End Select

            m_vTaxBandRate(ACRTaxBandId, nSelectedItem) = m_lTaxBandId
            m_vTaxBandRate(ACRTaxBandRateId, nSelectedItem) = m_lTaxBandRateId
            m_vTaxBandRate(ACRCode, nSelectedItem) = sCode
            m_vTaxBandRate(ACRCaptionId, nSelectedItem) = m_lRateCaptionID
            m_vTaxBandRate(ACRDescription, nSelectedItem) = m_sRateDescription
            m_vTaxBandRate(ACREffectiveDate, nSelectedItem) = m_dtEffectiveDate
            m_vTaxBandRate(ACRIsDeleted, nSelectedItem) = m_lIsDeleted
            m_vTaxBandRate(ACRIsValue, nSelectedItem) = m_lIsValue
            m_vTaxBandRate(ACRRate, nSelectedItem) = m_lRate
            m_vTaxBandRate(ACRSumInsuredRounded, nSelectedItem) = chkRounded.CheckState
            m_vTaxBandRate(ACRCalcBasis, nSelectedItem) = m_lCalcBasis
            m_vTaxBandRate(ACRSumInsuredValue, nSelectedItem) = m_lSumInsuredValue
            m_vTaxBandRate(ACRNB, nSelectedItem) = chkNB.CheckState
            m_vTaxBandRate(ACRAMTA, nSelectedItem) = chkAMTA.CheckState
            m_vTaxBandRate(ACRRMTA, nSelectedItem) = chkRMTA.CheckState
            m_vTaxBandRate(ACRCANC, nSelectedItem) = chkCANC.CheckState
            m_vTaxBandRate(ACRREN, nSelectedItem) = chkREN.CheckState
            m_vTaxBandRate(ACRCurrencyID, nSelectedItem) = m_iCurrencyID
            m_vTaxBandRate(ACRAllowTaxCredit, nSelectedItem) = chkAllowCredit.CheckState
            m_vTaxBandRate(ACRCountryID, nSelectedItem) = IIf(cboCountry.ItemId = 0, DBNull.Value, cboCountry.ItemId)
            m_vTaxBandRate(ACRStateID, nSelectedItem) = IIf(cboState.ItemId = 0, DBNull.Value, cboState.ItemId)
            m_vTaxBandRate(ACRCOBID, nSelectedItem) = IIf(cboCOB.ItemId = 0, DBNull.Value, cboCOB.ItemId)
            m_vTaxBandRate(ACRCOBDesc, nSelectedItem) = IIf(cboCOB.ItemId = 0, "", cboCOB.ItemCaption)
            m_vTaxBandRate(ACRCountryDesc, nSelectedItem) = IIf(cboCountry.ItemId = 0, "", cboCountry.ItemCaption)
            m_vTaxBandRate(ACRStateDesc, nSelectedItem) = IIf(cboState.ItemId = 0, "", cboState.ItemCaption)
            m_vTaxBandRate(ACRUseForRefundWhenExpired, nSelectedItem) = IIf(chkUseForRefundWhenexpired.Checked, 1, 0)
            m_vTaxBandRate(ACRUseForBackdatedNB, nSelectedItem) = IIf(chkUseForBackdatedNB.Checked, 1, 0)
            ' Additional transaction types (only save if we are a premium based rate)
            If (m_lCalcBasis = 0) Or (m_lCalcBasis = 3) Or (m_lCalcBasis = 4) Then
                m_vTaxBandRate(ACRTTRI, nSelectedItem) = chkTTRI.CheckState
                m_vTaxBandRate(ACRTTRIC, nSelectedItem) = chkTTRIC.CheckState
                m_vTaxBandRate(ACRTTAC, nSelectedItem) = chkTTAC.CheckState
                m_vTaxBandRate(ACRTTF, nSelectedItem) = chkTTF.CheckState
                m_vTaxBandRate(ACRTTCP, nSelectedItem) = chkTTCP.CheckState
                m_vTaxBandRate(ACRTTCS, nSelectedItem) = chkTTCS.CheckState
                m_vTaxBandRate(ACRTTCR, nSelectedItem) = chkTTCR.CheckState
                m_vTaxBandRate(ACRTTI, nSelectedItem) = chkTTI.CheckState
            Else
                m_vTaxBandRate(ACRTTRI, nSelectedItem) = False
                m_vTaxBandRate(ACRTTRIC, nSelectedItem) = False
                m_vTaxBandRate(ACRTTAC, nSelectedItem) = False
                m_vTaxBandRate(ACRTTF, nSelectedItem) = False
                m_vTaxBandRate(ACRTTCP, nSelectedItem) = False
                m_vTaxBandRate(ACRTTCS, nSelectedItem) = False
                m_vTaxBandRate(ACRTTCR, nSelectedItem) = False
                m_vTaxBandRate(ACRTTI, nSelectedItem) = False
            End If

            m_vTaxBandRate(kIsRIPaymentsRecoveries, nSelectedItem) = chkTTRIPR.CheckState
            m_lReturn = BusinessToInterface()

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update the data from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            'Tell it what we're getting

            m_oBusiness.TaxBandId = m_lTaxBandId


            m_lReturn = m_oBusiness.GetTaxBandRate(r_vTaxBandRate:=m_vTaxBandRate)

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
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Try

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

            ' Assign the details to the interface.

            ' ************************************************************
            ' Enter your code here to assign the all of the interface
            ' details from the business object, using the FormatField
            ' function for any type conversion.
            '
            ' Example:-
            '
            '    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName$)
            '    m_lReturn = m_oFormFields.FormatControl(ctlControl:=optChoice, vControlValue:=m_iDChoice%)
            '    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_dtDDate)
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            pnlTaxBand.Text = m_sDescription

            lvwTaxBandRate.Items.Clear()

            If Information.IsArray(m_vTaxBandRate) Then
                For lTemp As Integer = m_vTaxBandRate.GetLowerBound(1) To m_vTaxBandRate.GetUpperBound(1)
                    If CStr(m_vTaxBandRate(ACRCode, lTemp)) <> "" Then
                        ' Description
                        oListItem = lvwTaxBandRate.Items.Add(CStr(m_vTaxBandRate(ACRDescription, lTemp)))
                        ' Effective date
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vTaxBandRate(ACREffectiveDate, lTemp))
                        ' Rate or value
                        If CBool(m_vTaxBandRate(ACRIsValue, lTemp)) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatCurrency, m_vTaxBandRate(ACRRate, lTemp))
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatPercent, m_vTaxBandRate(ACRRate, lTemp))
                        End If
                        ' Class of business
                        If Strings.Len(CStr(m_vTaxBandRate(ACRCOBDesc, lTemp))) Then
                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vTaxBandRate(ACRCOBDesc, lTemp))
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "(Any)"
                        End If
                        ' Country
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vTaxBandRate(ACRCountryDesc, lTemp))
                        ' State
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vTaxBandRate(ACRStateDesc, lTemp))

                        oListItem.Tag = CStr(lTemp)
                    End If
                Next lTemp

            End If

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
        Dim sDescription As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the business object.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oBusiness.TaxBandId = m_lTaxBandId


            m_lReturn = m_oBusiness.Update(v_vTaxBandRate:=m_vTaxBandRate, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

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
        Try


            ' Assign the details to the data storage.

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
            'Display the ListView Tab

            tabDetailTab.Visible = False
            tabDetailTab.Top = VB6.TwipsToPixelsY(120)
            tabMainTab.Top = VB6.TwipsToPixelsY(120)

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the status of the Navigate button.

            ' Set any other default values to the interface.

            ' Set the column widths for the search list.
            lvwTaxBandRate.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(3000))
            lvwTaxBandRate.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1400))
            lvwTaxBandRate.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(900))
            lvwTaxBandRate.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(2400))
            lvwTaxBandRate.Columns.Item(4).Width = CInt(VB6.TwipsToPixelsX(2400))
            lvwTaxBandRate.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(2400))

            cmdAdd.Enabled = (Task <> gPMConstants.PMEComponentAction.PMView)
            cmdCopy.Enabled = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False

            'Get the Decimal Suppression flag
            Dim sTempOptionValue As String = ""
            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=1, r_vUnderwriting:=sTempOptionValue)

            If Trim(sTempOptionValue) <> "" AndAlso Trim(sTempOptionValue) = "1" Then
                IsSuppressDecimalValues = True
            End If

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
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            Me.Text = "Tax Band Rates"

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            'Developer Guide No. 243
            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 243
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            'Developer Guide No. 243
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, "Tax Band Rate")
            SSTabHelper.SetTabCaption(tabDetailTab, 0, "Details")

            lvwTaxBandRate.Columns.Item(0).Text = "Description"
            lvwTaxBandRate.Columns.Item(1).Text = "Effective Date"
            lvwTaxBandRate.Columns.Item(2).Text = "Rate"
            lvwTaxBandRate.Columns.Item(3).Text = "Class of Business"
            lvwTaxBandRate.Columns.Item(4).Text = "Country"
            lvwTaxBandRate.Columns.Item(5).Text = "State"


            'Developer Guide No. 243
            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetFormControls()

        Dim nBasis As Integer

        For nCount As Integer = optBasis.GetLowerBound(0) To optBasis.GetUpperBound(0)
            If optBasis(nCount).Checked Then
                nBasis = nCount
                Exit For
            End If
        Next

        txtRatePercentage.Visible = (chkIsValue.CheckState <> CheckState.Checked)
        txtRateValue.Visible = (chkIsValue.CheckState = CheckState.Checked)
        lblPer.Visible = (nBasis = 1 Or nBasis = 2) And (chkIsValue.CheckState = CheckState.Checked)
        txtSumInsuredValue.Visible = lblPer.Visible
        lblOfSI.Visible = lblPer.Visible
        chkRounded.Visible = lblPer.Visible
        chkTTRI.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTRIC.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTAC.Enabled = (nBasis = 0 Or nBasis = 3 Or nBasis = 4)
        chkTTF.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTCP.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTCS.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTCR.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTI.Enabled = (nBasis = 0 Or nBasis = 3)
        lblTTPolicy.Enabled = (nBasis = 0 Or nBasis = 3 Or nBasis = 4)
        lblTTClaim.Enabled = (nBasis = 0 Or nBasis = 3)
        chkTTRIPR.Enabled = (nBasis = 0 Or nBasis = 3)

        If nBasis = 4 Then
            chkTTRI.Checked = False
            chkTTRIC.Checked = False
            chkTTAC.Checked = True
            chkTTF.Checked = False
            chkTTCP.Checked = False
            chkTTCS.Checked = False
            chkTTCR.Checked = False
            chkTTI.Checked = False
        End If

        If chkIsValue.CheckState Then
            lblRate.Text = "Value:"
        Else
            lblRate.Text = "Rate:"
        End If

        If chkTTRIPR.Enabled = True OrElse chkTTRI.Enabled = True OrElse chkTTRIC.Enabled = True Then
            If chkTTRI.CheckState = CheckState.Checked Or chkTTRIC.CheckState = CheckState.Checked Then
                chkTTRIPR.Enabled = False
            Else
                chkTTRIPR.Enabled = True
                If chkTTRIPR.CheckState = CheckState.Checked Then
                    chkTTRI.Enabled = False
                    chkTTRIC.Enabled = False
                End If
            End If
        End If

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRTaxBandRate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMUTaxBandRate.General()

            ' Call the initialise method passing this interface
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


            Me.cboCountry.FirstItem = "(None)"
            Me.cboState.FirstItem = "(None)"
            Me.cboCOB.FirstItem = "(Any)"
            Me.cboCurrency.FirstItem = ""
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
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.TaxBandId = m_lTaxBandId
            ' {* USER DEFINED CODE (End) *}

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
                    Cancel = 1
                    eventArgs.cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            ' Check for errors.

            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the form control object.
            m_oFormFields.Dispose()

            ' Check for errors.

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









    Private Sub cboCountry_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCountry.Click
        ' Only show states that match the current country
        cboState.WhereClause = "country_id = " & cboCountry.ItemId
        cboState.RefreshList()
    End Sub

    Private Sub chkIsValue_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsValue.CheckStateChanged
        SetFormControls()
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_iAction = gPMConstants.PMEComponentAction.PMAdd

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Top = VB6.TwipsToPixelsY(120)
        tabDetailTab.Visible = True
        SetFormControls()

        m_lReturn = ClearDetail()

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

    Private Sub cmdCopy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCopy.Click

        Const kMethodName As String = "cmdCopy_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lSelectedItem, lNewItem As Integer

        Try


            ' Check for selected item
            If lvwTaxBandRate.FocusedItem Is Nothing Then
                cmdCopy.Enabled = False
                Exit Sub
            End If

            ' Get selected item

            lSelectedItem = Convert.ToString(lvwTaxBandRate.FocusedItem.Tag)

            ' Increase the array for the copied item
            lNewItem = m_vTaxBandRate.GetUpperBound(1) + 1
            ReDim Preserve m_vTaxBandRate(m_vTaxBandRate.GetUpperBound(0), lNewItem)

            ' Copy all fields
            For lCount As Integer = m_vTaxBandRate.GetLowerBound(0) To m_vTaxBandRate.GetUpperBound(0)
                m_vTaxBandRate(lCount, lNewItem) = m_vTaxBandRate(lCount, lSelectedItem)
            Next lCount

            ' Alter the important ones
            m_vTaxBandRate(ACRTaxBandRateId, lNewItem) = 0

            ' Start the edit process immediately
            m_iAction = gPMConstants.PMEComponentAction.PMCopy
            cmdOK.Enabled = False
            cmdCancel.Enabled = False
            tabMainTab.Visible = False
            tabDetailTab.Visible = True
            SetFormControls()

            ' Display data
            lReturn = CType(DataToDetail(), gPMConstants.PMEReturnCode)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=0, excep:=ex)

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here


            ' This is for debugging only

        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click


        m_iAction = gPMConstants.PMEComponentAction.PMDelete


        Dim lSelectedItem As Integer = Convert.ToString(lvwTaxBandRate.FocusedItem.Tag)

        m_vTaxBandRate(ACRTaxBandId, lSelectedItem) = 0

        cmdOK.Enabled = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdCopy.Enabled = False
        cmdEdit.Enabled = False
        cmdDelete.Enabled = False

        m_lReturn = DataRefresh()

    End Sub

    Private Sub cmdDetailCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailCancel.Click

        ' If this is a cancelled copy delete the new row
        If m_iAction = gPMConstants.PMEComponentAction.PMCopy Then
            ReDim Preserve m_vTaxBandRate(m_vTaxBandRate.GetUpperBound(0), m_vTaxBandRate.GetUpperBound(1) - 1)
            m_iAction = gPMConstants.PMEComponentAction.PMAdd
        End If

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            cmdOK.Enabled = True
        End If

    End Sub

    Private Sub cmdDetailOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDetailOK.Click

        ' Check mandatory controls have been entered into.
        m_lReturn = m_oFormFields.CheckMandatoryControls()

        ' Check for errors
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        tabDetailTab.Visible = False
        tabMainTab.Visible = True
        cmdCancel.Enabled = True
        cmdAdd.Enabled = True
        cmdOK.Enabled = True

        m_lReturn = DataRefresh()

    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_iAction = gPMConstants.PMEComponentAction.PMEdit

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        tabMainTab.Visible = False
        tabDetailTab.Visible = True
        SetFormControls()

        m_lReturn = DataToDetail()

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=MainModule.ScreenHelpID)
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim Msg As String = ""
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

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

    Private Sub lvwTaxBandRate_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTaxBandRate.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwTaxBandRate.Columns(eventArgs.Column)

        Static lOrder As SortOrder
        Static lLastCol As Integer

        If lLastCol <> ColumnHeader.Index + 1 Then
            lLastCol = ColumnHeader.Index + 1
            lOrder = SortOrder.Ascending
        Else
            lOrder = IIf(lOrder = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending)
        End If

        Select Case lLastCol
            Case 1, 4, 5, 6
                ListViewHelper.SetSortedProperty(lvwTaxBandRate, False)
                ListViewHelper.SetSortKeyProperty(lvwTaxBandRate, lLastCol - 1)
                ListViewHelper.SetSortOrderProperty(lvwTaxBandRate, lOrder)
                ListViewHelper.SetSortedProperty(lvwTaxBandRate, True)
            Case 2
                'Developer Guide No. 178
                ListViewFunc.ListViewSortByDate(lvwTaxBandRate, lLastCol - 1, lOrder)
            Case 3
                'Developer Guide No. 178
                ListView6Func.ListViewSortByValue(lvwTaxBandRate, lLastCol - 1, lOrder, False, True)
        End Select
    End Sub

    Private Sub lvwTaxBandRate_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTaxBandRate.DoubleClick
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If Not (lvwTaxBandRate.FocusedItem Is Nothing) Then
                cmdEdit_Click(cmdEdit, New EventArgs())
            End If
        End If
    End Sub

    Private Sub lvwTaxBandRate_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTaxBandRate.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        'Developer Guide No. 74
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Task <> gPMConstants.PMEComponentAction.PMView Then
            If lvwTaxBandRate.GetItemAt(x, y) Is Nothing Then
                cmdCopy.Enabled = False
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            Else
                cmdCopy.Enabled = True
                cmdDelete.Enabled = True
                cmdEdit.Enabled = True
            End If
        End If
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optBasis_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _optBasis_3.CheckedChanged, _optBasis_2.CheckedChanged, _optBasis_0.CheckedChanged, _optBasis_1.CheckedChanged, _optBasis_4.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            SetFormControls()
        End If
    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub
    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
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
    End Sub

    Private Sub txtRatePercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRatePercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRatePercentage)
    End Sub

    Private Sub txtRateValue_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtRateValue.KeyPress
        If chkIsValue.CheckState = CheckState.Checked AndAlso IsSuppressDecimalValues Then
            gPMFunctions.NumPress(sender, e)
        End If
    End Sub
    Private Sub txtRatePercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRatePercentage.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRatePercentage)
    End Sub

    Private Sub txtRateValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRateValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRateValue)
    End Sub
    Private Sub txtRateValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRateValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRateValue)
    End Sub

    Private Sub txtSumInsuredValue_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsuredValue.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSumInsuredValue)
    End Sub
    Private Sub txtSumInsuredValue_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSumInsuredValue.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSumInsuredValue)
    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            If tabMainTab.Visible Then
                tabMainTab.SelectedIndex = 0
            End If
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            If tabDetailTab.Visible Then
                tabDetailTab.SelectedIndex = 0
            End If
        End If

    End Sub

    Private Sub chkTTRIPR_CheckedChanged(sender As Object, e As EventArgs) Handles chkTTRIPR.CheckedChanged
        If chkTTRIPR.CheckState = CheckState.Checked Then

            chkTTRI.CheckState = CheckState.Unchecked
            chkTTRI.Enabled = False
            chkTTRIC.CheckState = CheckState.Unchecked
            chkTTRIC.Enabled = False

        Else

            chkTTRI.Enabled = True
            chkTTRIC.Enabled = True

        End If

    End Sub

    Private Sub chkTTRI_CheckedChanged(sender As Object, e As EventArgs) Handles chkTTRI.CheckedChanged

        If chkTTRI.CheckState = CheckState.Checked Then

            chkTTRIPR.CheckState = CheckState.Unchecked
            chkTTRIPR.Enabled = False

        ElseIf chkTTRI.CheckState <> CheckState.Checked AndAlso chkTTRIC.CheckState <> CheckState.Checked Then
            chkTTRIPR.Enabled = True
        End If
    End Sub

    Private Sub chkTTRIC_CheckedChanged(sender As Object, e As EventArgs) Handles chkTTRIC.CheckedChanged
        If chkTTRIC.CheckState = CheckState.Checked Then

            chkTTRIPR.CheckState = CheckState.Unchecked
            chkTTRIPR.Enabled = False

        ElseIf chkTTRIC.CheckState <> CheckState.Checked AndAlso chkTTRI.CheckState <> CheckState.Checked Then
            chkTTRIPR.Enabled = True
        End If
    End Sub    
End Class
