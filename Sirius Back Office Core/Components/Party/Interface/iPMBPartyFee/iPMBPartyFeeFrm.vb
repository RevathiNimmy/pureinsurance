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
    ' Date: 05/05/1999
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' CJB 170805 PN23229 Changed ValidateForm to validate if cboPMTransType was
    '            not set rather than set to 0 (None) which is valid! Also set
    '            listindex to -1 after creating the combo in SetFieldValidation.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sStepStatus As String = ""
    Private m_lPartyCnt As Integer
    Private m_lRiskGroupID As Integer 'Added MK 990924 1715
    Private m_sRiskGroup As String = "" 'Added MK 991001
    Private m_vExistingFees As Object
    Private m_sAccountType As String = ""
    Private m_dFeePercentage As Double
    Private m_cFeeAmount As Decimal
    Private m_dFeeCommissionPercentage As Double
    Private m_cFeeCommissionAmount As Decimal
    Private m_iDisplayOnQuotes As Integer
    Private m_lTransactionTypeID As Integer
    Private m_vDiscounts As Object
    Private m_cPremium As Decimal

    'DC140303 -ISS3018 -bring into line from 1.6.9
    ' CTAF 280502 - TaxRatesID
    Private m_lTaxRatesID As Integer

    'Datasure
    Private m_lTaxGroupId As Integer
    Private m_lCommissionTaxGroupId As Integer

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyFee.General

    ' Declare an instance of the Business object.

    Private m_oBusiness As bSIRPartyFee.Business
    'Private m_oBusiness As bSIRPFee.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.


    Private m_vLookupValues As Object
    Private m_vLookupDetails As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Private m_vExtraSchemes(,) As Object
    Private m_lExtraSchemeID As Integer
    Private m_sExtraSchemeDesc As String = ""

    Private m_lCurrencyID As Integer
    Private m_sCurrencyName As String = ""

    Private m_bFSAComplianceFlag As Boolean
    Private m_iFsaTypeOfSaleId As Integer
    Private m_bHideScheme As Boolean

    Public Property HideScheme() As Boolean
        Get
            Return m_bHideScheme
        End Get
        Set(ByVal Value As Boolean)
            m_bHideScheme = Value
        End Set
    End Property

    'DC140303 -ISS3018 -bring into line from 1.6.9
    ' CTAF 280502 - TaxRatesID
    Public Property TaxRatesID() As Integer
        Get
            Return m_lTaxRatesID
        End Get
        Set(ByVal Value As Integer)
            m_lTaxRatesID = Value
        End Set
    End Property
    'New for Datasure
    Public Property TaxGroupID() As Integer
        Get
            Return m_lTaxGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lTaxGroupId = Value
        End Set
    End Property
    Public Property CommissionTaxGroupID() As Integer
        Get
            Return m_lCommissionTaxGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionTaxGroupId = Value
        End Set
    End Property
    'Datasure End
    Public WriteOnly Property ExistingFees() As Object
        Set(ByVal Value As Object)


            m_vExistingFees = Value
        End Set
    End Property
    Public Property AccountType() As String
        Get
            Return m_sAccountType
        End Get
        Set(ByVal Value As String)
            m_sAccountType = Value
        End Set
    End Property
    Public Property Premium() As Decimal
        Get
            Return m_cPremium
        End Get
        Set(ByVal Value As Decimal)
            m_cPremium = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    'Added 991006 1723 MK
    Public Property RiskGroupID() As Integer
        Get
            Return m_lRiskGroupID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskGroupID = Value
        End Set
    End Property

    'Added 990924 1723 MK
    Public Property RiskGroup() As String
        Get
            Return m_sRiskGroup
        End Get
        Set(ByVal Value As String)
            m_sRiskGroup = Value
        End Set
    End Property

    Public Property FeePercentage() As Double
        Get
            Return m_dFeePercentage
        End Get
        Set(ByVal Value As Double)
            m_dFeePercentage = Value
        End Set
    End Property

    Public Property FeeAmount() As Decimal
        Get
            Return m_cFeeAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cFeeAmount = Value
        End Set
    End Property
    'EK 14/09/99 New Properties for Extras
    Public Property FeeCommissionPercentage() As Double
        Get
            Return m_dFeeCommissionPercentage
        End Get
        Set(ByVal Value As Double)
            m_dFeeCommissionPercentage = Value
        End Set
    End Property

    Public Property FeeCommissionAmount() As Decimal
        Get
            Return m_cFeeCommissionAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cFeeCommissionAmount = Value
        End Set
    End Property
    'EK 14/09/99 End

    'PSA 22/06/00

    'PSA 22/06/00
    Public Property DisplayOnQuotes() As Integer
        Get
            Return m_iDisplayOnQuotes
        End Get
        Set(ByVal Value As Integer)
            m_iDisplayOnQuotes = Value
        End Set
    End Property

    ' CJB 270802 - TransactionTypeID
    Public Property TransactionTypeID() As Integer
        Get
            Return m_lTransactionTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lTransactionTypeID = Value
        End Set
    End Property

    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Public Property ExtraSchemeID() As Integer
        Get
            Return m_lExtraSchemeID
        End Get
        Set(ByVal Value As Integer)
            m_lExtraSchemeID = Value
        End Set
    End Property
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    ' AMB 13-Oct-03: 1.8.6 Accident Management development
    Public Property ExtraSchemeDesc() As String
        Get
            Return m_sExtraSchemeDesc
        End Get
        Set(ByVal Value As String)
            m_sExtraSchemeDesc = Value
        End Set
    End Property

    Public Property CurrencyID() As Integer
        Get
            Return m_lCurrencyID
        End Get
        Set(ByVal Value As Integer)
            m_lCurrencyID = Value
        End Set
    End Property

    Public Property CurrencyName() As String
        Get
            Return m_sCurrencyName
        End Get
        Set(ByVal Value As String)
            m_sCurrencyName = Value
        End Set
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


    ' {* USER DEFINED CODE (Begin) *}
    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)
    ' PRIVATE Property Procedures (Begin)


    'Private Sub Status(ByVal Value As Integer)
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
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


    Public Property FSATypeOfSaleId() As Integer
        Get
            Return m_iFsaTypeOfSaleId
        End Get
        Set(ByVal Value As Integer)
            If Value > -1 And Value < 2 Then
                m_iFsaTypeOfSaleId = Value
            Else
                m_iFsaTypeOfSaleId = -1
            End If
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
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRiskGroup, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboScheme, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCurrency, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPercentage, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAmount, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionPercentage, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommissionAmount, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=chkDisplayOnQuotes, lFieldType:=gPMConstants.PMEDataType.PMInteger, lFormat:=gPMConstants.PMEFormatStyle.PMFormatInteger, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPMTransType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboPMTransType.ListIndex = -1 'PN23229



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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



            result = gPMConstants.PMEReturnCode.PMerror

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


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=m_dFeePercentage)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=m_cFeeAmount)

            'EK 14/9/99 New Fields for Extra
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage, vControlValue:=m_dFeeCommissionPercentage)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionAmount, vControlValue:=m_cFeeCommissionAmount)
            'PSA 22/06/00
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=chkDisplayOnQuotes, vControlValue:=m_iDisplayOnQuotes)

            ' CJB 270802
            cboPMTransType.ItemId = m_lTransactionTypeID

            'DC140303 -ISS3018 -bring into line from 1.6.9
            ' CTAF 280502 start
            'Datasure
            'cboPMTax.ItemId = m_lTaxRatesID
            cboPMTax.ItemId = m_lTaxGroupId
            cboPMCommissionTax.ItemId = m_lCommissionTaxGroupId
            'DatasureEnd

            ' AMB 13-Oct-03: 1.8.6 Accident Management development - set the combo item
            For lComboItem As Integer = 0 To cboScheme.Items.Count - 1
                If VB6.GetItemData(cboScheme, lComboItem) = m_lExtraSchemeID Then
                    cboScheme.SelectedIndex = lComboItem
                    Exit For
                End If
            Next lComboItem

            cboCurrency.CompanyId = g_iSourceID
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = m_lCurrencyID

            cboFSATypeOfSale.ItemId = m_iFsaTypeOfSaleId
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

            ' {* USER DEFINED CODE (Begin) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMerror

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


            ' Risk Group
            m_lRiskGroupID = cboRiskGroup.SelectedIndex
            m_sRiskGroup = VB6.GetItemString(cboRiskGroup, cboRiskGroup.SelectedIndex)

            ' Percentage

            m_dFeePercentage = CDbl(m_oFormFields.UnformatControl(txtPercentage))

            ' AMount

            m_cFeeAmount = CDec(m_oFormFields.UnformatControl(txtAmount))

            'EK 14/09/99 Extra Values for Extras

            ' Commission Percentage

            m_dFeeCommissionPercentage = CDbl(m_oFormFields.UnformatControl(txtCommissionPercentage))

            ' Commission Amount

            m_cFeeCommissionAmount = CDec(m_oFormFields.UnformatControl(txtCommissionAmount))

            'PSA 22/06/00
            ' Display On Quotes

            m_iDisplayOnQuotes = CInt(m_oFormFields.UnformatControl(chkDisplayOnQuotes))
            'PSA 22/06/00

            ' CJB 270802
            m_lTransactionTypeID = cboPMTransType.ItemId
            ' CJB 270802

            'DC140303 -ISS3018 -bring into line from 1.6.9
            ' CTAF 280502
            'Datasure
            'm_lTaxRatesID = cboPMTax.ItemId
            m_lTaxGroupId = cboPMTax.ItemId
            m_lCommissionTaxGroupId = cboPMCommissionTax.ItemId
            ' {* USER DEFINED CODE (End) *}

            ' AMB 13-Oct-03: 1.8.6 Accident Management development
            If m_sAccountType = gSIRLibrary.SIRPartyTypeExtra Then
                If cboScheme.SelectedIndex = -1 Then
                    ' none selected
                    m_lExtraSchemeID = 0
                    m_sExtraSchemeDesc = ""
                Else
                    m_lExtraSchemeID = VB6.GetItemData(cboScheme, cboScheme.SelectedIndex)
                    m_sExtraSchemeDesc = cboScheme.Text
                End If
            Else
                m_lExtraSchemeID = 0
                m_sExtraSchemeDesc = ""
            End If

            m_lCurrencyID = cboCurrency.CurrencyId
            m_sCurrencyName = cboCurrency.CurrencyName

            If cboFSATypeOfSale.Visible Then
                m_iFsaTypeOfSaleId = cboFSATypeOfSale.ItemId
            Else
                m_iFsaTypeOfSaleId = -1
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            m_lReturn = SetFirstLastControls()
            'EK 14/09/99
            If m_sAccountType = gSIRLibrary.SIRPartyTypeExtra Then
                lblCommissionPercentage.Visible = True
                txtCommissionPercentage.Visible = True
                lblCommissionAmount.Visible = True
                txtCommissionAmount.Visible = True
                'Datasure
                lblCommissionTax.Visible = True
                cboPMCommissionTax.Visible = True
                If Not m_bHideScheme Then
                    lblScheme.Visible = True
                    lblScheme.Enabled = True
                    cboScheme.Visible = True
                    cboScheme.Enabled = True
                Else
                    lblScheme.Visible = False
                    lblScheme.Enabled = False
                    cboScheme.Visible = False
                    cboScheme.Enabled = False
                End If

                lblFSATypeOfSale.Visible = False
                cboFSATypeOfSale.Visible = False
            Else

                lblCommissionPercentage.Visible = False
                txtCommissionPercentage.Visible = False
                lblCommissionAmount.Visible = False
                txtCommissionAmount.Visible = False
                lblScheme.Visible = False
                lblScheme.Enabled = False
                cboScheme.Visible = False
                cboScheme.Enabled = False
                'Datasure
                lblCommissionTax.Visible = False
                cboPMCommissionTax.Visible = False


                lblCurrency.Visible = True
                cboCurrency.Visible = True
                lblFSATypeOfSale.Visible = False
                cboFSATypeOfSale.Visible = False

                ' bit of a re-shuffle
                lblAmount.Top = lblPercentage.Top
                txtAmount.Top = txtPercentage.Top
                lblPercentage.Top = lblCurrency.Top
                txtPercentage.Top = cboCurrency.Top
                lblCurrency.Top = lblTax.Top
                cboCurrency.Top = cboPMTax.Top

                lblTax.Top = lblPMTransType.Top
                cboPMTax.Top = cboPMTransType.Top
                lblPMTransType.Top = lblDisplayOnQuotes.Top
                cboPMTransType.Top = chkDisplayOnQuotes.Top
                lblDisplayOnQuotes.Top = lblScheme.Top
                chkDisplayOnQuotes.Top = cboScheme.Top
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}


            Select Case m_iTask
                Case gPMConstants.PMEComponentAction.PMAdd
                    cboRiskGroup.Enabled = True
                Case gPMConstants.PMEComponentAction.PMEdit
                    cboRiskGroup.Enabled = False
            End Select
            ' {* USER DEFINED CODE (End) *}


            ' AMB 10-Sep-03: 1.8.6 Deferred Reinsurance development - select none
            If m_sAccountType = gSIRLibrary.SIRPartyTypeExtra Then cboScheme.SelectedIndex = -1

            cboCurrency.CompanyId = g_iSourceID
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = m_lCurrencyID


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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

            m_ctlTabFirstLast(ACControlStart, 0) = cboRiskGroup
            'PSA 22/06/00
            'Set m_ctlTabFirstLast(ACControlEnd, 0) = txtCommissionAmount
            m_ctlTabFirstLast(ACControlEnd, 0) = chkDisplayOnQuotes
            'PSA 22/06/00


            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMerror

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNavigate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNavigateButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Select Case m_sAccountType
                Case gSIRLibrary.SIRPartyTypeExtra

                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Case gSIRLibrary.SIRPartyTypeFee

                    SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End Select
            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to display all language specific
            ' captions.
            ' The GetResData function will allow you to do this.
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

            '    lblType.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCaptionType, _
            ''        iDataType:=PMResString)

            lblRiskGroup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionPercentage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCommissionAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCommission, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCommissionPercentage.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCommissionPercentage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCommissionAmount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCommissionAmount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'PSA 22/06/00

            lblDisplayOnQuotes.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionDisplayOnQuotes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' AMB 13-Oct-03: 1.8.6 Accident Management development

            lblScheme.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionScheme, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTax.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTaxGroup, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCaptionFSATypeOfSale, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function ValidateForm() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboRiskGroup.SelectedIndex = -1 Then
                MessageBox.Show("No risk group entered", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
                cboRiskGroup.Focus()
            End If

            If cboPMTransType.ListIndex = -1 Then 'PN23229
                MessageBox.Show("Transaction type must be selected.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                result = gPMConstants.PMEReturnCode.PMFalse
                cboPMTransType.Focus()
            End If

            If m_bFSAComplianceFlag And m_sAccountType = gSIRLibrary.SIRPartyTypeExtra Then
                If cboFSATypeOfSale.ItemData(cboFSATypeOfSale.ListIndex) = -1 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Type Of Sale", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    cboFSATypeOfSale.Focus()
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cboRiskGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRiskGroup.SelectedIndexChanged
        Dim cTemp As Double = m_dFeePercentage
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=cTemp)
        cTemp = m_cFeeAmount
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=cTemp)
        cTemp = m_dFeeCommissionPercentage
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage, vControlValue:=cTemp)
        cTemp = m_cFeeCommissionAmount
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionAmount, vControlValue:=cTemp)
        'PSA 22/06/00
        cTemp = m_iDisplayOnQuotes
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=chkDisplayOnQuotes, vControlValue:=cTemp)
        'PSA 22/06/00
    End Sub



    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyFee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMBPartyFee.General()

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
        Dim sValue As String = ""

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

            ' Get FSA compliance product option.
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for FSA Compliance", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_load")
            End If

            m_bFSAComplianceFlag = Not (sValue <> "1")

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

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

            If m_bFSAComplianceFlag Then
                ' Itemdata hardcoded to -1
                ' to avoid conflict between type of sale having value zero in lookup table
                ' and first value in combo
                cboFSATypeOfSale.ItemData(0) = -1
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

                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                cmdCancel.Focus()

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

    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With tabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetTabCount(tabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) < (SSTabHelper.GetTabCount(tabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(tabMainTab, SSTabHelper.GetSelectedIndex(tabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With

            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
        Catch




            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged


        'Try 
        '
        '    With tabMainTab
        '        ' Set the default button.
        '        If (.Tab < cmdNext.Count) Then
        '            cmdNext(.Tab).Default = True
        '        Else
        '            cmdOK.Default = True
        '        End If
        ''
        '        ' Now I know this is crap, this goes against
        '        ' all my principles, but for some reason when
        '        ' using the mouse to select a tab the setfocus
        '        ' code below doesn't work. The cursor sticks,
        '        ' and you can't tab off. Therefore I've used
        '        ' this to get around the problem.
        '        DoEvents
        ''
        '        ' Set focus to the first control on the tab.
        '        If (.Tab <= UBound(m_ctlTabFirstLast, 2)) Then
        '            m_ctlTabFirstLast(ACControlStart, .Tab).SetFocus
        '        End If
        '    End With
        '
        'Catch 
        '
        '
        '
        '
        '
        'tabMainTabPreviousTab = tabMainTab.SelectedIndex
        'End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim nReturn As Integer
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = ValidateForm()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
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

            Dim oPartyBusiness As bSIRParty.Business = Nothing
            nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
            End If

            oPartyBusiness.AddPartyHistory(m_lPartyCnt, String.Empty)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
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

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click

        ' Click event of the Navigate button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMNavigate

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PRIVATE Events (End)
    Private Sub txtAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAmount)
    End Sub

    Private Sub txtAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAmount.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAmount)


        Dim cTemp As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtAmount))

        If cTemp <> 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=0)
        End If

    End Sub

    Private Sub txtCommissionAmount_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionAmount.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCommissionAmount)

    End Sub

    Private Sub txtCommissionAmount_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionAmount.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCommissionAmount)


        Dim cTemp As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCommissionAmount))

        If cTemp <> 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage, vControlValue:=0)
        End If

    End Sub

    Private Sub txtCommissionPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionPercentage.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCommissionPercentage)

    End Sub

    Private Sub txtCommissionPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommissionPercentage.Leave

        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCommissionPercentage)
        'DJM 11/09/2003 : Prevent error if txtCommissionPercentage set to blank.
        If txtCommissionPercentage.Text = "" Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionPercentage, vControlValue:=0)
        End If


        Dim cTemp As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtCommissionPercentage))

        If cTemp <> 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommissionAmount, vControlValue:=0)
        End If

    End Sub

    Private Sub txtPercentage_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPercentage)
    End Sub

    Private Sub txtPercentage_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPercentage.Leave


        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPercentage)

        If txtPercentage.Text.Trim() = "" Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPercentage, vControlValue:=0)
        End If


        Dim cTemp As Decimal = CDec(m_oFormFields.UnformatControl(ctlControl:=txtPercentage))

        If cTemp <> 0 Then
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAmount, vControlValue:=0)
        End If

    End Sub


    ' ***************************************************************** '
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Dim lUBound, lLBound As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            'MK 991007
            m_lReturn = GetLookupDetails(sLookupTable:="Risk_group", ctlLookup:=cboRiskGroup)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AMB 13-Oct-03: 1.8.6 Accident Management development - get list of Extra schemes
            If m_sAccountType = gSIRLibrary.SIRPartyTypeExtra Then


                m_lReturn = m_oBusiness.GetExtraSchemeDetails(r_vExtraSchemes:=m_vExtraSchemes)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details list of Extra Schemes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                    End If
                End If

                ' AMB 13-Oct-03: 1.8.6 Accident Management development - load up the Extra schemes
                cboScheme.Items.Clear()
                ' add a blank entry
                Dim cboScheme_NewIndex As Integer = -1
                cboScheme_NewIndex = cboScheme.Items.Add("")
                VB6.SetItemData(cboScheme, cboScheme_NewIndex, 0)

                If Information.IsArray(m_vExtraSchemes) Then
                    lLBound = m_vExtraSchemes.GetLowerBound(1)
                    lUBound = m_vExtraSchemes.GetUpperBound(1)
                    For lTemp As Integer = lLBound To lUBound
                        cboScheme_NewIndex = cboScheme.Items.Add(gPMFunctions.NullToString(m_vExtraSchemes(ACExtraSchemeDesc, lTemp)))
                        VB6.SetItemData(cboScheme, cboScheme_NewIndex, gPMFunctions.NullToLong(m_vExtraSchemes(ACExtraSchemeID, lTemp)))
                    Next lTemp
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function GetLookupValues() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Gets all of the lookup values.

            ' Check the task.

            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Get all of the lookup values.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Get all of the lookup values with the correct
                    ' effective date.

                    m_oBusiness.RiskGroupID = m_lRiskGroupID

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_oBusiness.RiskGroupID = m_lRiskGroupID

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    'changes as Control does not support the properties of combobox
    'Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control) As Integer
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As ComboBox) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim bFoundMatch As Boolean

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        ' Lookup detail contants.
        Const ACDetailKey As Integer = 0
        Const ACDetailDesc As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            bFoundMatch = False

            For lRow = m_vLookupValues.GetLowerBound(1) To m_vLookupValues.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupValues(ACValueTableName, lRow)).Trim() = sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table - " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            Dim newIndex As Integer = 0
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                ' Add the details to the control.



                newIndex = ctlLookup.Items.Add(m_vLookupDetails(ACDetailDesc, lCntr))



                ctlLookup.Items(newIndex) = CInt(m_vLookupDetails(ACDetailKey, lCntr))

                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'dveloper guide no. 28
                        ctlLookup.SelectedIndex = newIndex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow)) = "" Then

                'dveloper guide no. 28
                'ctlLookup.ListIndex = 0
                ctlLookup.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMerror

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
