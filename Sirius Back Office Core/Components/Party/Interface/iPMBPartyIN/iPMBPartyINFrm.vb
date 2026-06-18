Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    'Private vBranch As Variant
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' TF  031298 - Menu & Toolbar activity
    ' CJB 040205 - PM18533 Change IsStargateInstalled to check PM\Stargate\Client\"version" in registry.
    ' CJB 170305 - PN15893 Correct all tabbing - main changes being in tabMainTab_Click and on individual
    '              control's tabindex values. Also moved ABINumber combo to make bigger and not cutoff text.
    ' CJB 060705 - PN22166 Changed InterfaceToData to handle uctABINumber 'Not Found' error more gracefully.
    ' ***************************************************************** '
    'Replaced iPMFunc.GetResData to GetResData in the whole document
    'Developer Guide No.7
    Public Const vbFormCode As Integer = 0

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    'S4BDAT004
    Private Const TermsOfPaymentId As Integer = 0
    Private Const Description As Integer = 3
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    'Developer Guide No. 50
    Dim frmRiskGroups As frmRiskGroups

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyIN.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the Business object.
    Private m_oPaymentGroup As Object

    ' Declare an instance of the IPT Find interface.

    Private m_oIPTExtras As Object

    ' Declare an instance of the Insurer Rates interface.

    Private m_oRates As Object

    Private m_oPaymentGroups As Object

    'DP 11/11/2002 Add access to Stargate Configuration tool

    'Private m_oStargateConfig As iSGConfigManager.Interface_Renamed
    Private m_oStargateConfig As Object
    'DP end

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    'TN10072000 variable to determine system mode ie "Underwriting or Agency"
    Private m_sUnderwritingOrAgency As String = ""

    'JMK 19/10/2001 display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    'Stores the return value for reference id
    Private m_iRefID As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAccountID As Integer
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sName As String = ""
    Private m_lCurrencyId As Integer
    Private m_sPaymentTermCode As String = ""
    Private m_iStatements As Integer
    Private m_vABICodeOn81 As String = ""
    'Devlopment Work on AMInsurer Payment Locking
    Private m_iCboLockingTypeId As Integer

    Private m_vABICodes As Object

    Private m_vPaymentGroups As Object

    'DP 12/11/2002 - Added for Stargate
    Private m_lSGInsurerID As Integer

    'TN20001025 - change AgencyNumber to variant type
    Private m_vAgencyNumber As Object

    Private m_lBinderIndicator As Integer
    Private m_lReportIndicator As Integer
    Private m_iIsReInsurer As Integer
    Private m_lReInsuranceType As Integer
    Private m_bIsRetained As Boolean
    Private m_iIsReinsuranceDebitCreditNo As Integer
    ' Peter Finney 13/08/2003 - Changed to appropriate datatype
    Private m_lDefaultCommissionRate As Double

    'DC150803 -PS254 -fsa compliance
    Private m_lFSAInsurerStatus As Integer
    Private m_sFSARegistrationNumber As String = ""
    Private m_lFSAInsurerCreditRating As Integer

    'ECK Datasure 10102005 Claims Rating Agency
    Private m_lClaimsRatingAgencyId As Integer
    Private m_sClaimsRatingGrading As String = ""
    Private m_dtClaimsRatingDate As Date
    'S4BDAT005
    Private m_sClaimsRatingDescription As String = ""

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_sAddressLine1 As String = ""
    'Note the index in the lookup array of the main address
    Private m_sMainPostCode As String = ""
    Private m_iMainAddressIndex As Integer
    Private m_lAddressCount As Integer
    'eck100402
    Private m_vAddressRiskGroups() As Object

    'RWH(24/07/2000) RSAIB Process 004.
    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    ' Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    ' Declare an instance of the contact interface.

    Private m_oContact As iPMBContact.Interface_Renamed

    Private m_bGeminiIILink As Boolean
    'eck070601
    Private m_lMainAddressCnt As Integer
    ' CTAF 040800
    Private m_oGISListManager As iGISListManager.InterfaceNoLogin

    'DC150803 -PS254 -fsa compliance
    Private m_bEnableFSACompliance As Boolean
    'DC110706 PN29315 check for New Zealand Configuration
    Private m_bEnableNZConfig As Boolean

    ' RDC 23042004
    Private m_vBranch As Integer
    Private m_vSubBranch As Integer
    Private m_bShowSubBranchID As Boolean

    'RDT PN18099

    Private m_oPMUser As bPMUser.Business

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double

    'S4BDAT004
    Private m_lTermsOfPaymentId As Integer

    'PN 19428
    Private m_vSourceArray(,) As Object

    Private m_bDomicileForText As Boolean
    Private m_vRiskTransferAgreement As CheckState
    Private m_vBrokerlinkSubaccount As Integer
    Private m_vBrokerlinkUnderwritingId As String = ""

    Private m_vRiskTransferEditable As Object

    'Set the variable if new insurer is being added.
    Private m_bNewInsurer As Boolean

    Private m_vRiskCodes(,) As Object
    Private m_bSetupComplete As Boolean
    'PVY
    Private m_vBureauAccountPartyCnt As CheckState
    Private m_sBureauAccountPartyName As String = ""
    Private m_vInsurerTypeId As Integer
    ' Declare an instance of the FindParty Business object.
    Private m_oFindPartyBusiness As Object

    'Maintain Party Code
    Private m_bIsSetMaskingCode As Boolean
    Private m_bIsReadOnly As Boolean
    Private m_bIsViewOnlyInsurerMaintenance As Boolean

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)

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

    Public Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value

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
    ' {* USER DEFINED CODE (Begin) *}

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property
    Public Property LongName() As String
        Get

            Return m_sName

        End Get
        Set(ByVal Value As String)

            'For some reason this wont compile if use 'name' as the property
            'name.
            m_sName = Value

        End Set
    End Property

    Public Property AddressLine1() As String
        Get

            Return m_sAddressLine1

        End Get
        Set(ByVal Value As String)

            m_sAddressLine1 = Value

        End Set
    End Property

    'Maintain Party Code
    Private ReadOnly Property InsurerName() As String
        Get
            Return gPMFunctions.ToSafeString(txtName.Text.Replace(" ", "").ToUpper())
        End Get
    End Property

    Private ReadOnly Property InsurerType() As String
        Get
            Return gPMFunctions.ToSafeString(cboInsurerType.ItemCode.Replace(" ", "").ToUpper())
        End Get
    End Property

    Public Property DefaultCountryID() As Integer
        Get
            Return m_iDefaultCountryID
        End Get
        Set(ByVal Value As Integer)
            m_iDefaultCountryID = Value
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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998

            'Reference must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Commission
            ' Peter Finney 13/08/2003 - Changed to appropriate datatype
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDefaultCommissionRate, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory, lDecimalPlaces:=-5) 'DC150803 -PS254 -fsa compliance
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClaimsRatingGrading, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClaimsRatingDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4BDAT005
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtClaimsRatingDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC110706 PN29315 only set ABI to mandatory if not New Zealand
            If m_iDefaultCountryID = 1 And Not m_bEnableNZConfig Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctABINumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                lblABINumber.Font = VB6.FontChangeBold(lblABINumber.Font, True)
            Else
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctABINumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                lblABINumber.Font = VB6.FontChangeBold(lblABINumber.Font, False)

            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998
            'm_lPartyCnt& = 25

            m_lReturn = m_oBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)
            ' {* USER DEFINED CODE (End) *}

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

            ' Error Section.

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
    'ECK Datasure 10102005 Claims Rating Agency
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim sABIDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            If m_iTask <> gPMConstants.PMEComponentAction.PMAdd Then
                ' Assign the details from the business object
                ' to the data storage.
                m_lReturn = BusinessToData()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Assign the details to the interface.

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgencyNumber, vControlValue:=m_vAgencyNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For iLoop As Integer = 0 To cboTermsOfPayment.Items.Count - 1
                If VB6.GetItemData(cboTermsOfPayment, iLoop) = m_lTermsOfPaymentId Then
                    cboTermsOfPayment.SelectedIndex = iLoop
                    Exit For
                End If
            Next iLoop

            cboBinderIndicator.SelectedIndex = m_lBinderIndicator
            cboReportIndicator.SelectedIndex = m_lReportIndicator

            cboInsurerStatus.ItemId = m_lFSAInsurerStatus
            cboInsurerCreditRating.ItemId = m_lFSAInsurerCreditRating

            'DC150803 -PS254 -fsa compliance
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDefaultCommissionRate, vControlValue:=m_lDefaultCommissionRate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC110706 PN29315 only disable if FSA
            fraReinsurance.Visible = m_bEnableFSACompliance

            'DC130706 Datasure - as no abi code is required need to check if null

            If String.IsNullOrEmpty(m_vABICodeOn81) Or Convert.IsDBNull(m_vABICodeOn81) Or IsNothing(m_vABICodeOn81) Then
                m_vABICodeOn81 = ""
            End If

            If m_vABICodeOn81.Trim().Length > 0 Then

                ' CTAF 040800 - Display the ABI Insurer
                m_lReturn = m_oGISListManager.GetDescriptionFromABICode(v_sPropertyId:=uctABINumber.PropertyId, v_sABICode:=m_vABICodeOn81, r_sDescription:=sABIDescription)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'MKW270503 PN4267 - Remove return value as non critical START
                    'BusinessToInterface = PMFalse
                    'Exit Function
                    'MKW270503 PN4267 - END
                End If

                ' CTAF 040800 - Format it into the control
                uctABINumber.Text = sABIDescription

            End If

            'Fill the contact grid
            PopulateContacts()

            'Fill the address grid
            PopulateAddresses()

            'Fill the risk code grid
            m_lReturn = PopulateRiskCodes()

            'PN32510 - Set Default branch Id
            If m_vBranch = 0 Then
                m_vBranch = g_iSourceID
            End If

            'RDT PN18099 - Set the combo to the correct branch
            If m_vBranch = 0 Then
                uctBranch.SelectedIndex = -1
            Else
                For i As Integer = 0 To uctBranch.Items.Count - 1
                    If VB6.GetItemData(uctBranch, i) = m_vBranch Then
                        uctBranch.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If
            'eck25102005 Prevent crash where sub branch array not set up

            If m_vSubBranch.Equals(0) Then

                m_lReturn = GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)

                cboSubBranch.SelectedIndex = -1

                For iLoop As Integer = 0 To cboSubBranch.Items.Count - 1
                    If VB6.GetItemData(cboSubBranch, iLoop) = m_vSubBranch Then
                        cboSubBranch.SelectedIndex = iLoop
                    End If
                Next
            End If
            cboCurrency.CompanyId = m_vBranch
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = m_lCurrencyId
            ' ************************************************************
            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage
            ' ************************************************************

            txtBrokerlinkUnderwritingId.Text = gPMFunctions.ToSafeString(m_vBrokerlinkUnderwritingId, "")

            If Convert.IsDBNull(m_vBrokerlinkSubaccount) Or IsNothing(m_vBrokerlinkSubaccount) Then
                cboBrokerlinkSubAccount.SelectedIndex = -1
            Else
                For i As Integer = 0 To cboBrokerlinkSubAccount.Items.Count - 1
                    If VB6.GetItemData(cboBrokerlinkSubAccount, i) = m_vBrokerlinkSubaccount Then
                        cboBrokerlinkSubAccount.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

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
    'ECK Datasure 10102005 Claims Rating Agency
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
                'SP090989
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    '160605 Datasure pass Taxable indicator

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vAgencyNumber:=m_vAgencyNumber, vBinderIndicator:=m_lBinderIndicator, vReportIndicator:=m_lReportIndicator, vIsReinsurer:=m_iIsReInsurer, vReinsuranceType:=m_lReInsuranceType, vIsReinsuranceDebitCreditNo:=m_iIsReinsuranceDebitCreditNo, vDefaultCommRate:=m_lDefaultCommissionRate, vShortName:=m_sShortName, vName:=m_sName, vCurrencyId:=m_lCurrencyId, vPaymentTermCode:=m_sPaymentTermCode, vStatements:=m_iStatements, vABICodeOn81:=m_vABICodeOn81, vFSAInsurerStatus:=m_lFSAInsurerStatus, vFSARegistrationNumber:=m_sFSARegistrationNumber, vFSAInsurerCreditRating:=m_lFSAInsurerCreditRating, vIsRetained:=m_bIsRetained, vBranchID:=m_vBranch, vSubBranchID:=m_vSubBranch)
                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    '160605 Datasure pass Taxable indicator

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vAgencyNumber:=m_vAgencyNumber, vBinderIndicator:=m_lBinderIndicator, vReportIndicator:=m_lReportIndicator, vIsReinsurer:=m_iIsReInsurer, vReinsuranceType:=m_lReInsuranceType, vIsReinsuranceDebitCreditNo:=m_iIsReinsuranceDebitCreditNo, vDefaultCommRate:=m_lDefaultCommissionRate, vShortName:=m_sShortName, vName:=m_sName, vCurrencyId:=m_lCurrencyId, vPaymentTermCode:=m_sPaymentTermCode, vStatements:=m_iStatements, vABICodeOn81:=m_vABICodeOn81, vFSAInsurerStatus:=m_lFSAInsurerStatus, vFSARegistrationNumber:=m_sFSARegistrationNumber, vFSAInsurerCreditRating:=m_lFSAInsurerCreditRating, vIsRetained:=m_bIsRetained, vBranchID:=m_vBranch, vSubBranchID:=m_vSubBranch)

            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.
            m_lReturn = GetLookupValues()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get all of the lookup details.

            'RDT PN18099 - Call Get branch for the list of branches available to this user
            m_lReturn = GetBranchDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateAddresses
    '
    ' Description: Fills the grid control with address details
    '
    ' ***************************************************************** '
    Private Sub PopulateAddresses()

        Dim k As Integer

        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            'Just go if no addresses
            If Not Information.IsArray(m_vAddresses) Then
                Exit Sub
            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwAddresses.Items.Clear()
            m_lAddressCount = 0

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                For k = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If m_vAddresses(1, i).Equals(m_vAddressTypes(0, k)) Then
                        sAddressUsage = CStr(m_vAddressTypes(1, k)).Trim()
                        Exit For
                    End If
                Next k
                'See if this is the main address
                If CStr(m_vAddressTypes(2, k)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                    m_sMainPostCode = CStr(m_vAddresses(0, i))
                    m_iMainAddressIndex = CInt(m_vAddressTypes(0, k))
                    'eck070601
                    m_lMainAddressCnt = CInt(m_vAddresses(6, i))
                End If

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Assign the details to the first column.
                        ' Postcode
                        oListItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim())

                        ' Assign details to other the columns
                        ' Address Usage
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sAddressUsage
                        ' Address Line 1
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(2, i)).Trim()
                        ' Address Line 2
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(3, i)).Trim()
                        ' Address Line 3
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(4, i)).Trim()
                        ' Address Line 4
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(5, i)).Trim()
                        'eck081101 Hidden risk_group_id
                        ' Risk Group
                        'eck100402 code with array
                        'oListItem.SubItems(6) = m_vAddresses(7, i)

                        If Not Information.IsArray(m_vAddressRiskGroups) Then
                            ReDim m_vAddressRiskGroups(0)
                        Else
                            ReDim Preserve m_vAddressRiskGroups(m_vAddressRiskGroups.GetUpperBound(0) + 1)
                        End If
                        m_vAddressRiskGroups(m_vAddressRiskGroups.GetUpperBound(0)) = m_vAddresses(7, i)

                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vAddressRiskGroups.GetUpperBound(0))
                        'eck100402

                    Case Else
                        ' Assign the details to the first column.
                        ' Address Usage
                        oListItem = lvwAddresses.Items.Add(sAddressUsage)

                        ' Assign details to other the columns
                        ' Address Line 1
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddresses(2, i)).Trim()
                        ' Address Line 2
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAddresses(3, i)).Trim()
                        ' Address Line 3
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vAddresses(4, i)).Trim()
                        ' Address Line 4
                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vAddresses(5, i)).Trim()
                        ' Postcode
                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vAddresses(0, i)).Trim()
                        'eck081101 Hidden risk_group_id
                        ' Risk Group
                        'eck100402 code with array
                        'oListItem.SubItems(6) = m_vAddresses(7, i)

                        If Not Information.IsArray(m_vAddressRiskGroups) Then
                            ReDim m_vAddressRiskGroups(0)
                        Else
                            ReDim Preserve m_vAddressRiskGroups(m_vAddressRiskGroups.GetUpperBound(0) + 1)
                        End If
                        m_vAddressRiskGroups(m_vAddressRiskGroups.GetUpperBound(0)) = m_vAddresses(7, i)

                        ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vAddressRiskGroups.GetUpperBound(0))
                        'eck100402

                End Select

                ' Store the Address_cnt
                oListItem.Tag = CStr(m_vAddresses(6, i)).Trim()
                ' {* USER DEFINED CODE (End) *}
                m_lAddressCount += 1
                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateContacts
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Sub PopulateContacts()

        Dim oListItem As ListViewItem

        Try

            'Just go if no contacts
            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If
            lvwContacts.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vContacts(2, i)).Trim()

                ' Column 3
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vContacts(3, i)).Trim()

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vContacts(4, i)).Trim()

                ' Column 5
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vContacts(5, i)).Trim()

                ' Store the Contact_cnt
                oListItem.Tag = CStr(m_vContacts(0, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ValidateOK
    '
    ' Description: This validates mandatory address types and duplicate
    ' addresses
    '
    ' ***************************************************************** '
    Private Function ValidateOK() As Integer

        Dim result As Integer = 0
        Dim iMainAddresses As Integer
        Dim bDuplicate As Boolean
        Dim lAddressCnt As Integer 'PN6906

        Dim oListItem, oListItem2 As ListViewItem
        Dim sAddressUsage, sPartyCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Datasure - only need ABI for UK based systems
            'DC110706 PN29315 only give mandatory message if not New Zealand
            If m_iDefaultCountryID = 1 And Not m_bEnableNZConfig Then
                If uctABINumber.Text.Trim() = "" Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    uctABINumber.Focus()
                    MessageBox.Show("This is a mandatory field. You must enter data in this field.", "Mandatory Field - ABI Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            sPartyCode = txtIDReference.Text.Trim()

            m_lReturn = m_oBusiness.CheckReference(sPartyCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Unable to access business object", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sPartyCode = "" And m_sShortName.Trim().ToUpper() <> txtIDReference.Text.Trim().ToUpper() Then
                MessageBox.Show("New Insurer Code already exists", Application.ProductName)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iMainAddresses = 0

            'Count how many addresses are main address
            If lvwAddresses.Items.Count > 0 Then
                For i As Integer = 1 To lvwAddresses.Items.Count
                    oListItem = lvwAddresses.Items.Item(i - 1)

                    'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                    Select Case (m_sDefaultCountryCode.Trim())
                        Case "GBR"
                            sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(i - 1), 1).Text.Trim()
                        Case Else
                            sAddressUsage = lvwAddresses.Items.Item(i - 1).Text.Trim()
                    End Select

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                            iMainAddresses += 1
                        End If
                    Next j
                Next i
            End If

            Select Case iMainAddresses
                Case 0
                    'No
                    MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    cmdAddAd.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                Case 1
                    'Yes
                Case Else
                    'No.
                    MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    cmdAddAd.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
            End Select

            'Now Ensure addresses are not used twice
            If lvwAddresses.Items.Count >= 2 Then

                bDuplicate = False

                'Check for duplicates
                'Need to find out how to calculate no of lines in the grid
                For i As Integer = 1 To (lvwAddresses.Items.Count)
                    oListItem = lvwAddresses.Items.Item(i - 1)
                    If CBool(CStr(Convert.ToString(oListItem.Tag) <> "").Trim()) Then
                        lAddressCnt = Convert.ToString(oListItem.Tag)
                        'RWH(24/07/2000) Why not just use the count property?
                        '            For j = (i + 1) To m_lAddressCount
                        For j As Integer = (i + 1) To lvwAddresses.Items.Count
                            oListItem2 = lvwAddresses.Items.Item(j - 1)
                            If CBool(CStr(Convert.ToString(oListItem2.Tag) <> "").Trim()) Then
                                If (Convert.ToString(oListItem2.Tag)) = lAddressCnt Then
                                    bDuplicate = True
                                    Exit For
                                End If
                            End If
                        Next j
                    End If
                Next i

                If bDuplicate Then
                    MessageBox.Show("An address can only be used once by a particular party.", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: UpdateAddressPostCodeProperties
    '
    ' Description: This checks for the main address and gets the
    ' post code and address line 1 for it via the address business
    '
    ' ***************************************************************** '
    Private Sub UpdateAddressPostCodeProperties()

        Dim lAddressCnt As Integer

        Dim oAddressBusiness As bSIRAddress.Business
        Dim sAddressUsage As String = ""

        Try

            'Find the main address
            For i As Integer = 1 To lvwAddresses.Items.Count

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(i - 1), 1).Text.Trim()
                    Case Else
                        sAddressUsage = lvwAddresses.Items.Item(i - 1).Text.Trim()
                End Select

                For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                        lAddressCnt = Convert.ToString(lvwAddresses.Items.Item(i - 1).Tag)
                        Exit For
                    End If
                Next j
            Next i

            'Get address business to retrieve
            Dim temp_oAddressBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oAddressBusiness, "bSIRAddress.Business", vInstanceManager:="ClientManager")
            oAddressBusiness = temp_oAddressBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            oAddressBusiness.AddressCnt = lAddressCnt

            m_lReturn = oAddressBusiness.GetDetails(vAddressCnt:=lAddressCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
                oAddressBusiness = Nothing
                Exit Sub
            End If

            m_lReturn = oAddressBusiness.GetNext(vPostalCode:=m_sMainPostCode, vAddress1:=m_sAddressLine1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oAddressBusiness.Dispose()
                oAddressBusiness = Nothing
                Exit Sub
            End If

            oAddressBusiness.Dispose()
            oAddressBusiness = Nothing

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodePropertiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BusinessToData
    '
    ' Description: Updates the data storage from the business object.
    '
    ' ***************************************************************** '
    'ECK Datasure 10102005 Claims Rating Agency
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BusinessToData"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vAgencyNumber:=m_vAgencyNumber, vBinderIndicator:=m_lBinderIndicator, vReportIndicator:=m_lReportIndicator, vIsReinsurer:=m_iIsReInsurer, vReinsuranceType:=m_lReInsuranceType, vIsReinsuranceDebitCreditNo:=m_iIsReinsuranceDebitCreditNo, vDefaultCommRate:=m_lDefaultCommissionRate, vShortName:=m_sShortName, vName:=m_sName, vCurrencyId:=m_lCurrencyId, vPaymentTermCode:=m_sPaymentTermCode, vStatements:=m_iStatements, vABICodeOn81:=m_vABICodeOn81, vFSAInsurerStatus:=m_lFSAInsurerStatus, vFSARegistrationNumber:=m_sFSARegistrationNumber, vFSAInsurerCreditRating:=m_lFSAInsurerCreditRating, vIsRetained:=m_bIsRetained, vSourceID:=m_vBranch, vSubBranchID:=m_vSubBranch)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetNext", "vPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Foe underwriting, default party to ReInsurer
            m_iIsReInsurer = 1

            'Get additional party details
            m_lReturn = GetPartyDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetPartyDetails", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get addresses

            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetAddressDetails", "m_lPartyCnt = " & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get address type lookups

            m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetAddresstypelookups", "m_lPartyCnt = " & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get contacts

            m_lReturn = m_oBusiness.GetContactDetails(vContacts:=m_vContacts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetContactDetails", "m_lPartyCnt = " & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get risk codes

            m_lReturn = m_oBusiness.GetRiskCodeDetails(r_vRiskCodes:=m_vRiskCodes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetRiskCodeDetails", "m_lPartyCnt = " & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    'ECK Datasure 10102005 Claims Rating Agency
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Dim sMsg, sABICode, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998

            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))

            m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))

            m_vAgencyNumber = m_oFormFields.UnformatControl(ctlControl:=txtAgencyNumber)

            m_lCurrencyId = cboCurrency.CurrencyId
            m_lBinderIndicator = cboBinderIndicator.SelectedIndex
            m_lReportIndicator = cboReportIndicator.SelectedIndex

            m_sPaymentTermCode = cboTermsOfPayment.Text
            'S4BDAT004
            'PN32818
            If cboTermsOfPayment.SelectedIndex < 0 Then
                m_lTermsOfPaymentId = 0
            Else
                m_lTermsOfPaymentId = VB6.GetItemData(cboTermsOfPayment, cboTermsOfPayment.SelectedIndex)
            End If

            m_iIsReInsurer = 0
            m_lReInsuranceType = 0

            'DC150803 -PS254 -fsa compliance
            m_lFSAInsurerStatus = cboInsurerStatus.ItemId
            m_lFSAInsurerCreditRating = cboInsurerCreditRating.ItemId

            m_iStatements = 0

            m_iIsReinsuranceDebitCreditNo = 0

            m_lClaimsRatingAgencyId = 0
            m_sClaimsRatingGrading = ""
            m_dtClaimsRatingDate = DateTime.Today
            m_sClaimsRatingDescription = ""

            m_sFSARegistrationNumber = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDefaultCommissionRate))

            If uctABINumber.Text.Trim() = "" Then

                m_vABICodeOn81 = Nothing
            Else

                ' Get the ABI code from the chosen description
                m_lReturn = m_oGISListManager.GetABICodeFromDescription(v_sPropertyId:=uctABINumber.PropertyId, v_sDescription:=uctABINumber.Text, r_sABICode:=sABICode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then 'PN22166
                        MessageBox.Show("Cannot find the ABI code for " & uctABINumber.Text & "." & Strings.Chr(13) & Strings.Chr(10) &
                                        " You must either select an item from the list or enter an exact name.", "Invalid ABI Number", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on GetABICodeFromDescription for : " & uctABINumber.Text, vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                    Return result
                End If

                ' Save the code
                m_vABICodeOn81 = sABICode

            End If

            ' If this is an add then check for duplicate references

            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                    m_lReturn = m_oBusiness.CheckReference(m_sShortName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'If the returned reference is an empty string, then the reference exists
                    If m_sShortName = "" Then

                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefExists, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMsg, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If
            End If

            'Default branch information to null for broking
            If m_bShowSubBranchID Then

                'AR20050126 - PN18325 Check a branch has been selected
                If uctBranch.SelectedIndex <> -1 Then
                    m_vBranch = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)
                Else
                    m_vBranch = 0
                End If

                If cboSubBranch.SelectedIndex <> -1 Then
                    m_vSubBranch = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
                Else

                    m_vSubBranch = Nothing
                End If

            Else
                m_vBranch = 0

                m_vSubBranch = Nothing
            End If

            If cboClaimsRatingAgency.ListIndex <> -1 Then
                'AR20060922 - PN29953 Use correct ItemId property
                m_lClaimsRatingAgencyId = cboClaimsRatingAgency.ItemId
            Else
                m_lClaimsRatingAgencyId = 0
            End If

            m_sClaimsRatingGrading = CStr(m_oFormFields.UnformatControl(ctlControl:=txtClaimsRatingGrading))

            m_dtClaimsRatingDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtClaimsRatingDate))

            m_sClaimsRatingDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtClaimsRatingDescription))

            '***************************
            ' get the party tax details from the party tax control
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage
            '***************************

            If cboBrokerlinkSubAccount.SelectedIndex <> -1 Then
                m_vBrokerlinkSubaccount = VB6.GetItemData(cboBrokerlinkSubAccount, cboBrokerlinkSubAccount.SelectedIndex)
            Else

                m_vBrokerlinkSubaccount = Nothing
            End If
            m_vBrokerlinkUnderwritingId = txtBrokerlinkUnderwritingId.Text

            m_vInsurerTypeId = cboInsurerType.ItemId
            If gPMFunctions.ToSafeInteger(m_vBureauAccountPartyCnt) = 0 Then

                m_vBureauAccountPartyCnt = Nothing
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.

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
        Dim lCount As Integer
        Dim vTermsOfPayment(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'Get Country Code for Postcode checking.(Process 004)

            m_lReturn = m_oBusiness.GetDefaultCountryCode(v_iCountryID:=m_iDefaultCountryID, r_sCountryCode:=m_sDefaultCountryCode)

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get default country code", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")

                Return result
            End If

            m_bDomiciledForTax = True

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cmdEditAd.Enabled = False
            cmdRiskGroup.Enabled = False

            cmdDeleteAd.Enabled = False
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            chkRTA.Visible = False
            chkRTAEditable.Visible = False

            cboBinderIndicator.Items.Clear()
            cboBinderIndicator.Items.Add("All Outstanding")
            cboBinderIndicator.Items.Add("Paid by Client")

            cboReportIndicator.Items.Clear()
            cboReportIndicator.Items.Add("Payment Date")
            cboReportIndicator.Items.Add("Policy Number")
            cboReportIndicator.Items.Add("Client Code")
            cboReportIndicator.Items.Add("Effective Date")

            cboBrokerlinkSubAccount.Items.Clear()
            Dim cboBrokerlinkSubAccount_NewIndex As Integer = -1
            cboBrokerlinkSubAccount_NewIndex = cboBrokerlinkSubAccount.Items.Add("0")
            VB6.SetItemData(cboBrokerlinkSubAccount, cboBrokerlinkSubAccount_NewIndex, 0)
            cboBrokerlinkSubAccount_NewIndex = cboBrokerlinkSubAccount.Items.Add("1")
            VB6.SetItemData(cboBrokerlinkSubAccount, cboBrokerlinkSubAccount_NewIndex, 1)
            cboBrokerlinkSubAccount_NewIndex = cboBrokerlinkSubAccount.Items.Add("2")
            VB6.SetItemData(cboBrokerlinkSubAccount, cboBrokerlinkSubAccount_NewIndex, 2)
            cboBrokerlinkSubAccount_NewIndex = cboBrokerlinkSubAccount.Items.Add("3")
            VB6.SetItemData(cboBrokerlinkSubAccount, cboBrokerlinkSubAccount_NewIndex, 3)

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            'Set up the address grid control columns and headers

            ' TF291298 - Disable menu options if New Client
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cmdIPTExtras.Visible = False

                Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonFinancial - 1).Enabled = False
                Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonNotes - 1).Enabled = False
                Toolbar1.Items.Item(SharedFiles.PMBToolbarFunc.ACIButtonLetter - 1).Enabled = False

                cboBinderIndicator.SelectedIndex = 0
                cboReportIndicator.SelectedIndex = 0
            End If

            'DC130904 PN14929/14934 only apply available when adding
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                cmdApply.Visible = False
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cmdRates.Enabled = False
                cmdPaymentGroups.Enabled = False
            End If

            lblABINumber.Visible = True
            uctABINumber.Visible = True

            lblSubBranch.Visible = m_bShowSubBranchID
            cboSubBranch.Visible = m_bShowSubBranchID

            'AR20050126 - PN18325 Branch not visible for broking
            lblBranch.Visible = m_bShowSubBranchID
            uctBranch.Visible = m_bShowSubBranchID

            'AR20050126 - PN18325 Reposition controls
            If Not m_bShowSubBranchID Then

                lblTermsOfPayment.Top = lblCurrency.Top
                cboTermsOfPayment.Top = cboCurrency.Top

                lblCurrency.Top = lblBranch.Top
                cboCurrency.Top = uctBranch.Top

                lblABINumber.Top = lblReportIndicator.Top
                uctABINumber.Top = cboReportIndicator.Top

                lblReportIndicator.Top = lblBinderIndicator.Top
                cboReportIndicator.Top = cboBinderIndicator.Top

                lblBinderIndicator.Top = lblSubBranch.Top
                cboBinderIndicator.Top = cboSubBranch.Top

            End If

            'S4BDAT004

            m_lReturn = m_oBusiness.GetTermsOfPayment(v_lTermsOfPaymentId:=-1, r_vResult:=vTermsOfPayment)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'adding Blank Entry
            Dim cboTermsOfPayment_NewIndex As Integer = -1
            cboTermsOfPayment_NewIndex = cboTermsOfPayment.Items.Add("")

            If Information.IsArray(vTermsOfPayment) Then

                For iLoop As Integer = vTermsOfPayment.GetLowerBound(1) To vTermsOfPayment.GetUpperBound(1)

                    cboTermsOfPayment_NewIndex = cboTermsOfPayment.Items.Add(CStr(vTermsOfPayment(Description, iLoop)))

                    VB6.SetItemData(cboTermsOfPayment, cboTermsOfPayment_NewIndex, CInt(vTermsOfPayment(TermsOfPaymentId, iLoop)))
                Next iLoop
            End If

            'Hide some of the tabs
            SSTabHelper.SetTabVisible(tabMainTab, 4, False)
            SSTabHelper.SetTabVisible(tabMainTab, 5, False)
            SSTabHelper.SetTabVisible(tabMainTab, 6, False)

            'Hide some of the next buttons
            If SSTabHelper.GetTabVisible(tabMainTab, 3) And Not SSTabHelper.GetTabVisible(tabMainTab, 4) And Not SSTabHelper.GetTabVisible(tabMainTab, 5) And Not SSTabHelper.GetTabVisible(tabMainTab, 6) Then
                cmdNext(3).Visible = False
            End If
            If SSTabHelper.GetTabVisible(tabMainTab, 4) And Not SSTabHelper.GetTabVisible(tabMainTab, 5) And Not SSTabHelper.GetTabVisible(tabMainTab, 6) Then
                cmdNext(4).Visible = False
            End If
            If SSTabHelper.GetTabVisible(tabMainTab, 5) And Not SSTabHelper.GetTabVisible(tabMainTab, 6) Then
                cmdNext(5).Visible = False
            End If

            'Rename the tab headers to include a numerical prefix
            lCount = 0
            For iLoop As Integer = 0 To 6
                If SSTabHelper.GetTabVisible(tabMainTab, iLoop) Then
                    lCount += 1
                    SSTabHelper.SetTabCaption(tabMainTab, iLoop, "&" & lCount & " - " & SSTabHelper.GetTabCaption(tabMainTab, iLoop))
                End If
            Next

            '0405006 Datasure

            lblInsurerType.Visible = False
            cboInsurerType.Visible = False
            cmdBureauAccount.Visible = False
            txtBureauAccount.Visible = False

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
            ReDim m_ctlTabFirstLast(1, 6)

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
            'SP090998
            m_ctlTabFirstLast(ACControlStart, 0) = txtIDReference
            m_ctlTabFirstLast(ACControlEnd, 0) = chkRTAEditable

            m_ctlTabFirstLast(ACControlStart, 1) = lvwAddresses
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdDeleteAd

            m_ctlTabFirstLast(ACControlStart, 2) = lvwContacts
            m_ctlTabFirstLast(ACControlEnd, 2) = cmdDeleteCon

            m_ctlTabFirstLast(ACControlStart, 3) = uctPartyTax1
            m_ctlTabFirstLast(ACControlEnd, 3) = uctPartyTax1

            m_ctlTabFirstLast(ACControlStart, 4) = cboClaimsRatingAgency
            m_ctlTabFirstLast(ACControlEnd, 4) = txtClaimsRatingDescription

            m_ctlTabFirstLast(ACControlStart, 5) = txtBrokerlinkUnderwritingId
            m_ctlTabFirstLast(ACControlEnd, 5) = cboBrokerlinkSubAccount

            m_ctlTabFirstLast(ACControlStart, 6) = lvwRiskCodes
            m_ctlTabFirstLast(ACControlEnd, 6) = cmdEditRisk

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
    ' Name: DisplayCaptions
    '
    ' Description: Display all language specific captions.
    '
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Dim sPostCode, sAddressUsage, sAddressLine1, sAddressLine2, sAddressLine3, sAddressLine4 As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            'Display caption depending on hidden system option
            ' JMK 19/10/2001 extra Underwriting type option

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                   "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblIDReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACIDReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgencyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgencyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblTermsOfPayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTermsOfPayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblBinderIndicator.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBinderIndicator, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblReportIndicator.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReportIndicator, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC150803 -PS254 -fsa compliance

            fraReinsurance.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraFSA, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblReInsuranceType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFSAStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblInsurerCreditRating.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFSACreditRating, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCommission.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFSARegistrationNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClaimsRatingDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACClaimsRatingDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraFSA, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sAddressUsage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListUsage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sAddressLine1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sAddressLine2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sAddressLine3 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sAddressLine4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            sPostCode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditRisk.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblInsurerPaymentLocking.Visible = False
            cboLocking.Visible = False

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            With lvwAddresses
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        .Columns.Item(0).Text = sPostCode
                        .Columns.Item(1).Text = sAddressUsage
                        .Columns.Item(2).Text = sAddressLine1
                        .Columns.Item(3).Text = sAddressLine2
                        .Columns.Item(4).Text = sAddressLine3
                        .Columns.Item(5).Text = sAddressLine4
                        .Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1000))
                        .Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2200))
                    Case Else
                        .Columns.Item(0).Text = sAddressUsage
                        .Columns.Item(1).Text = sAddressLine1
                        .Columns.Item(2).Text = sAddressLine2
                        .Columns.Item(3).Text = sAddressLine3
                        .Columns.Item(4).Text = sAddressLine4
                        .Columns.Item(5).Text = sPostCode
                        .Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1000))
                        .Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2200))
                End Select
            End With

            chkRTAEditable.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_CAPTION_RTA_CAN_BE_CHANGED, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception

            ' Error Section.

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

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

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

            result = gPMConstants.PMEReturnCode.PMError

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
    ' Error Section.
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

    Private Sub cboInsurerType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboInsurerType.Click
        If cboInsurerType.ItemCaption.ToUpper() <> ("syndicate").ToUpper() Then
            cmdBureauAccount.Enabled = False
            txtBureauAccount.Text = ""
            m_vBureauAccountPartyCnt = CheckState.Unchecked
        Else
            cmdBureauAccount.Enabled = True
        End If
    End Sub

    Private Sub cboLocking_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboLocking.Click
        If cboLocking.ItemCaption.ToUpper() = ("Payment Group").ToUpper() Then
            m_lReturn = CheckPaymentGroup()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
        End If
    End Sub

    ' PRIVATE Methods (End)

    Private Sub cboTermsOfPayment_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTermsOfPayment.SelectionChangeCommitted
        m_lReturn = PMBGeneralFunc.FieldChange(Me)

    End Sub

    Private Sub cboTermsOfPayment_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTermsOfPayment.DropDown
        'm_lReturn = FillCombo(cboControl:=cboTermsOfPayment, _
        'bRefill:=False)

    End Sub

    Private Sub cboTermsOfPayment_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTermsOfPayment.Enter
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboTermsOfPayment)

    End Sub

    Private Sub cboTermsOfPayment_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboTermsOfPayment.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldChange(Me, KeyCode, Shift)

    End Sub

    Private Sub cboTermsOfPayment_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTermsOfPayment.Leave
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboTermsOfPayment)

    End Sub

    Private Sub chkRTA_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRTA.CheckStateChanged

        Dim sMessage As String = ""
        Dim eResult As DialogResult

        If Not m_bNewInsurer And m_bSetupComplete Then
            If chkRTA.CheckState <> Math.Abs(gPMFunctions.ToSafeInteger(m_vRiskTransferAgreement, 0)) Then
                sMessage = ""
                sMessage = sMessage & "You have changed the default risk transfer agreement option." & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Do you wish to set the risk transfer agreement option for all risks to the same as the new default value?." & Strings.Chr(13) & Strings.Chr(10)
                eResult = MessageBox.Show(sMessage, "Default Risk Transfer Agreement", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                If eResult = System.Windows.Forms.DialogResult.Yes Then
                    For Each oListItem As ListViewItem In lvwRiskCodes.Items
                        If chkRTA.CheckState = CheckState.Checked Then
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Yes"
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "No"
                        End If
                    Next oListItem
                End If

                m_vRiskTransferAgreement = chkRTA.CheckState
            End If
        End If

    End Sub

    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click

        Dim sTmp As String = ""
        Dim oListItem As ListViewItem

        Try

            'Create icontact if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the address interface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_oAddress.Reference = txtIDReference.Text
            '    m_lReturn = ControlLostFocus(cboTermsOfPayment)

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.PostCode = m_sMainPostCode

            'PN 19428
            If uctBranch.SelectedIndex > 0 Then

                m_oAddress.CountryID = gPMFunctions.ToSafeLong(CStr(m_vSourceArray(4, uctBranch.SelectedIndex)))
            Else

                m_oAddress.CountryID = Me.DefaultCountryID
            End If

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()

            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    ' Add the data to the list view

                    oListItem = lvwAddresses.Items.Add(m_oAddress.PostalCode)
                    ' Address Usage

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
                    ' Address line 1

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1
                    ' Address line 2

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2
                    ' Address line 3

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3
                    ' Address line 4

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
                Case Else
                    ' Add the data to the list view

                    oListItem = lvwAddresses.Items.Add(m_oAddress.AddressUsageType)
                    ' Address line 1

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1
                    ' Address line 2

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address2
                    ' Address line 3

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3
                    ' Address line 4

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
                    ' Postcode

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
            End Select

            ' Store the Address_cnt

            oListItem.Tag = m_oAddress.AddressCnt

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click

        Dim oListItem As ListViewItem

        Try

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_lReturn = m_oContact.ContactCnt

            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()

            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode)

            ' Assign details to other the columns
            ' Column 2
            'Temporary thing

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

            ' Store the Address_cnt

            oListItem.Tag = m_oContact.ContactCnt

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'JB 050199 Added check for Referenece id(shortname)

            'Check that a reference id has been entered

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'update the party cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                ' save additional details back to party record.
                m_lReturn = UpdatePartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

                'Update party addresses
                m_lReturn = UpdateAddresses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                    Exit Sub
                End If

                ' CTAF 021000 - Update Orion
                m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")
                    Exit Sub
                End If

                'Update party contacts
                m_lReturn = UpdateContacts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                    Exit Sub
                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()
            Else
                Exit Sub
            End If

            cmdApply.Visible = False
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
            Me.Task = gPMConstants.PMEComponentAction.PMEdit
            cmdRates.Enabled = True
            cmdPaymentGroups.Enabled = True
            BusinessToInterface()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub cmdBureauAccount_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBureauAccount.Click
        Try
            Dim vBureauAccountPartyCnt As String = ""
            Dim vShortName As Object
            Dim sBureauAccountPartyName As String = ""

            m_lReturn = SelectBureau(v_vPartyCnt:=CInt(vBureauAccountPartyCnt), v_vShortName:=CStr(vShortName), v_vName:=sBureauAccountPartyName, v_vSpecialParty:=gSIRLibrary.SIRPartyTypeInsurer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_vBureauAccountPartyCnt = gPMFunctions.ToSafeLong(vBureauAccountPartyCnt, 0)
            m_sBureauAccountPartyName = gPMFunctions.ToSafeString(sBureauAccountPartyName, "")
            txtBureauAccount.Text = m_sBureauAccountPartyName

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdBureauAccount_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click

        Dim oListItem As ListViewItem

        Try

            'Set row to be deleted - if a valid one selected
            'Set row to be deleted - if a valid one selected
            If lvwAddresses.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            oListItem = lvwAddresses.FocusedItem
            'set the address id

            m_oAddress.AddressCnt = Convert.ToString(oListItem.Tag)

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeId = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            'm_oAddress.addresscnt = m_lAddressCnt
            'm_oAddress.AddressUsageTypeId = m_lAddressUsageTypeID

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Update the address details
            ' & postcode

            'Reset Interface
            cmdEditAd.Enabled = False
            'eck081101
            cmdRiskGroup.Enabled = False
            cmdDeleteAd.Enabled = False

            lvwAddresses.Items.RemoveAt(lvwAddresses.FocusedItem.Index)

            lvwAddresses.Focus()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteCon.Click

        Try

            'Set row to be deleted - if a valid one selected
            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id

            m_oContact.ContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            lvwContacts.Items.RemoveAt(lvwContacts.FocusedItem.Index)

            lvwContacts.Focus()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click

        Dim oListItem As ListViewItem
        Dim sTmp As String = ""
        Dim sAddressUsage As String = ""

        Try

            'Set the address count being edited - if a valid one selected

            'Set the address count being edited - if a valid one selected

            If lvwAddresses.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.PostCode = m_sMainPostCode

            oListItem = lvwAddresses.FocusedItem
            'set the address id

            m_oAddress.AddressCnt = Convert.ToString(oListItem.Tag)

            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"

                    m_oAddress.PostCode = oListItem.Text
                    sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
                Case Else

                    m_oAddress.PostCode = ListViewHelper.GetListViewSubItem(oListItem, 5).Text
                    sAddressUsage = oListItem.Text
            End Select

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If sAddressUsage = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeId = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditAd.Enabled = False
            'eck081101
            cmdRiskGroup.Enabled = False
            cmdDeleteAd.Enabled = False

            With lvwAddresses.Items.Item(m_iLine - 1)
                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Postcode

                        .Text = m_oAddress.PostalCode
                        ' Address usage type

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 1).Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 2).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 3).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 4).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 5).Text = m_oAddress.Address4
                    Case Else
                        ' Address usage type

                        .Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 1).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 2).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 3).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 4).Text = m_oAddress.Address4
                        'Postcode

                        ListViewHelper.GetListViewSubItem(lvwAddresses.Items.Item(m_iLine - 1), 5).Text = m_oAddress.PostalCode
                End Select

                ' Store the AddressCnt in the tag

                .Tag = m_oAddress.AddressCnt
            End With

            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click

        Dim oListItem As ListViewItem


        Try

            'Set the contact count being edited - if a valid one selected

            'Set row to be deleted - if a valid one selected
            If lvwContacts.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode

            'set the contact id
            oListItem = lvwContacts.FocusedItem

            m_oContact.ContactCnt = Convert.ToString(oListItem.Tag)

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            oListItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '

    'Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vAgentOnly As String = "") As Integer
    'Dim result As Integer = 0
    'Dim iSIRFindParty As Object
    '
    'Dim oFindParty, vKeyArray As Object
    '
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'oFindParty = New iSIRFindParty.Interface()
    '
    'Set appropriate key if agent only

    'If (Not Information.IsNothing(vAgentOnly)) And (Not String.IsNullOrEmpty(vAgentOnly)) Then
    '
    ''ReDim vKeyArray(1, 0)

    'vKeyArray(0, 0) = gSIRLibrary.SIRNavKeyAgentOnly

    'vKeyArray(1, 0) = vAgentOnly
    '

    'm_lErrorNumber = oFindParty.SetKeys(vKeyArray)
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'm_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'oFindParty.CallingAppName = "iPMBPartyIN.Interface"
    '
    'SD 02/08/2002 Scalability

    'm_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lErrorNumber = oFindParty.Start()
    '
    'If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then

    'm_lErrorNumber = oFindParty.Terminate()
    'oFindParty = Nothing
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
    '

    'vPartyCnt = oFindParty.PartyCnt

    'vShortName = oFindParty.ShortName

    'If Not Information.IsNothing(vName) Then

    'vName = oFindParty.LongName
    'End If
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'End If
    '

    'm_lErrorNumber = oFindParty.Terminate()
    '
    'oFindParty = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: UpdateAddresses
    '
    ' Description: This goes thru all addresses in the the grid control
    ' and the original address array and sees what the differences
    ' are. It then adds new addresses or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateAddresses() As Integer

        Dim result As Integer = 0
        Dim vNewAddresses, vOldAddresses(,) As Object
        Dim bFirst As Boolean
        Dim i As Integer
        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Go thru original address array to get list of old addresses
            If Information.IsArray(m_vAddresses) Then
                'eck081101 Risk Group Id
                '       ReDim vOldAddresses(1, UBound(m_vAddresses, 2))
                ReDim vOldAddresses(2, m_vAddresses.GetUpperBound(1))
                For i = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                    vOldAddresses(0, i) = CInt(m_vAddresses(6, i))

                    vOldAddresses(1, i) = CInt(m_vAddresses(1, i))

                    vOldAddresses(2, i) = m_vAddresses(7, i)
                    'eck081101
                Next i
            End If

            'Go thru addresses grid to get list of new addresses
            'Go thru addresses grid to get list of new addresses
            i = 1
            bFirst = True
            Do
                If i > lvwAddresses.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwAddresses.Items.Item(i - 1)

                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
                    Case Else
                        sAddressUsage = oListItem.Text.Trim()
                End Select

                If sAddressUsage = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        'eck081101 Risk Group Id
                        '                ReDim vNewAddresses(1, i - 1)
                        ReDim vNewAddresses(2, i - 1)
                        bFirst = False
                    Else
                        'eck081101 Risk Group Id
                        '                ReDim Preserve vNewAddresses(1, i - 1)
                        ReDim Preserve vNewAddresses(2, i - 1)
                    End If

                    vNewAddresses(0, i - 1) = Convert.ToString(oListItem.Tag)

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        If sAddressUsage = CStr(m_vAddressTypes(1, j)) Then

                            vNewAddresses(1, i - 1) = m_vAddressTypes(0, j)
                            Exit For
                        End If
                    Next j

                    'eck081101 Risk Group Id
                    'eck100402 Array of RiskGroupIds
                    '           vNewAddresses(2, i - 1) = oListItem.SubItems(6)
                    'eck030502 check that its numeric
                    Dim dbNumericTemp As Double
                    If Double.TryParse(ListViewHelper.GetListViewSubItem(oListItem, 6).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        vNewAddresses(2, i - 1) = m_vAddressRiskGroups(CInt(ListViewHelper.GetListViewSubItem(oListItem, 6).Text))
                    End If
                End If
                i += 1
            Loop

            'Delete old address usages in database
            If (Information.IsArray(vOldAddresses)) And (Not Information.IsArray(vNewAddresses)) Then
                For i = 0 To vOldAddresses.GetUpperBound(1)
                    m_lReturn = m_oBusiness.DeleteAddress(m_lPartyCnt, vOldAddresses(0, i))
                Next
                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vAddAddresses:=vNewAddresses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'If we have old and new addresses, delete common ones
            If (Information.IsArray(vOldAddresses)) And (Information.IsArray(vNewAddresses)) Then

                'Delete unchanged addresses (ie set them to 0)

                For i = vOldAddresses.GetLowerBound(1) To vOldAddresses.GetUpperBound(1)
                    'eck081101 Risk Group Id
                    'eck100402 Extra compaisons for array

                    For j As Integer = vNewAddresses.GetLowerBound(1) To vNewAddresses.GetUpperBound(1)

                        If (vNewAddresses(0, j).Equals(vOldAddresses(0, i))) And (vNewAddresses(1, j).Equals(vOldAddresses(1, i))) And (Not ChangeRiskGroups(vNewArray:=vNewAddresses(2, j), vOldArray:=vOldAddresses(2, i))) Then

                            vNewAddresses(0, j) = 0

                            vOldAddresses(0, i) = 0
                        End If
                    Next j
                Next i

                For m As Integer = 0 To vOldAddresses.GetUpperBound(1)
                    Dim nUnmatchRecord As Integer = 0
                    nUnmatchRecord = DeleteAddress(CInt(vOldAddresses(0, m)), vNewAddresses)
                    If nUnmatchRecord = 1 Then
                        m_lReturn = m_oBusiness.DeleteAddress(m_lPartyCnt, vOldAddresses(0, m))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If
                Next
                'update the database

                m_lReturn = m_oBusiness.UpdateAddresses(vPartyCnt:=m_lPartyCnt, vDeleteAddresses:=vOldAddresses, vAddAddresses:=vNewAddresses)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAddressesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'eck100402 Check whether Risk Groups have changed
    Private Function ChangeRiskGroups(ByRef vNewArray() As Object, ByRef vOldArray() As Object) As Boolean
        Dim result As Boolean = False
        result = True

        If Not Information.IsArray(vNewArray) And Not Information.IsArray(vOldArray) Then
            Return False
        End If

        If Information.IsArray(vNewArray) And Information.IsArray(vOldArray) Then
            If vNewArray.GetUpperBound(0) <> vOldArray.GetUpperBound(0) Then
                Return result
            Else
                For lRow As Integer = 0 To vOldArray.GetUpperBound(0)

                    If Not vOldArray(lRow).Equals(vNewArray(lRow)) Then
                        Return result
                    End If
                Next lRow
                result = False
            End If
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateContacts
    '
    ' Description: This goes thru all contacts in the the grid control
    ' and the original contact array and sees what the differences
    ' are. It then adds new contacts or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateContacts() As Integer

        Dim result As Integer = 0
        Dim vNewContacts, vOldContacts As Object
        Dim bFirst As Boolean
        Dim i As Integer
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Go thru original contact array to get list of old contacts
            If Information.IsArray(m_vContacts) Then
                ReDim vOldContacts(m_vContacts.GetUpperBound(1))
                For i = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                    vOldContacts(i) = CInt(m_vContacts(0, i))
                Next i
            End If

            'Go thru contacts grid to get list of new contacts
            i = 1
            bFirst = True

            Do
                If i > lvwContacts.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwContacts.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vNewContacts(i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewContacts(i - 1)
                    End If

                    vNewContacts(i - 1) = Convert.ToString(oListItem.Tag)

                End If
                i += 1
            Loop

            'Delete old contact usages in database
            If (Information.IsArray(vOldContacts)) And (Not Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'Add new addresses in database
            If (Not Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vAddContacts:=vNewContacts)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'If we have old and new Contacts, delete common ones
            If (Information.IsArray(vOldContacts)) And (Information.IsArray(vNewContacts)) Then

                'Delete unchanged Contacts (ie set them to 0)

                For i = vOldContacts.GetLowerBound(0) To vOldContacts.GetUpperBound(0)

                    For j As Integer = vNewContacts.GetLowerBound(0) To vNewContacts.GetUpperBound(0)

                        If vNewContacts(j).Equals(vOldContacts(i)) Then

                            vNewContacts(j) = 0

                            vOldContacts(i) = 0
                        End If
                    Next j
                Next i

                'update the database

                m_lReturn = m_oBusiness.UpdateContacts(vPartyCnt:=m_lPartyCnt, vDeleteContacts:=vOldContacts, vAddContacts:=vNewContacts)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateContactsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdEditRisk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditRisk.Click

        Dim frm As New frmRiskCodeDetails

        'Default to True
        frm.RiskTransferAgreement = True
        frm.DelegatedAuthority = True

        'If any lines selected are set to "No" then default to false
        Dim lCount As Integer = 0
        For Each oListItem As ListViewItem In lvwRiskCodes.Items
            If oListItem.Selected Then
                lCount += 1
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "No" Then
                    frm.RiskTransferAgreement = False
                End If
                If ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No" Then
                    frm.DelegatedAuthority = False
                End If
            End If
        Next oListItem

        'Set the risk code description for the forms caption
        If lCount = 1 Then
            frm.RiskCodeDesc = lvwRiskCodes.FocusedItem.Text
        Else
            frm.RiskCodeDesc = "Multiple Risks Selected"
        End If

        'Show the form
        frm.ShowDialog()

        'If "OK" was selected then update all of the selected risks
        If frm.Status = gPMConstants.PMEReturnCode.PMOK Then
            For Each oListItem As ListViewItem In lvwRiskCodes.Items
                If oListItem.Selected Then
                    If frm.RiskTransferAgreement Then
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Yes"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "No"
                    End If
                    If frm.DelegatedAuthority Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No"
                    End If
                End If
            Next oListItem
        End If

        'Unload the form
        frm.Close()
        frm = Nothing

    End Sub

    ' PRIVATE Events (Begin)

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)

    End Sub

    'EK 7/10/99 Add Access to IPT Extras
    Private Sub cmdIPTExtras_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdIPTExtras.Click


        Try
            'Create shares object if not already done so
            If m_oIPTExtras Is Nothing Then

                ' Get an instance of the shares interface object via
                ' the public object manager.
                Dim temp_m_oIPTExtras As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oIPTExtras, sClassName:="iPMBIPTExtras.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oIPTExtras = temp_m_oIPTExtras

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get IPT Extras object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdIPTExtras_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oIPTExtras.SetProcessModes(vTask:=m_iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oIPTExtras.PartyCnt = m_lPartyCnt

            m_lReturn = m_oIPTExtras.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdIPTExtras_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdIPTExtras_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'EK 110400
    Private Sub cmdPaymentGroups_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdPaymentGroups.Click

        Try
            'Create shares object if not already done so
            If m_oPaymentGroups Is Nothing Then

                ' Get an instance of the shares interface object via
                ' the public object manager.
                Dim temp_m_oPaymentGroups As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPaymentGroups, sClassName:="iACTInsurerPaymentGroups.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oPaymentGroups = temp_m_oPaymentGroups

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Insurer Payment Group object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPaymentGroups_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oPaymentGroups.SetProcessModes(vTask:=m_iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oPaymentGroups.PartyCnt = m_lPartyCnt

            m_lReturn = m_oPaymentGroups.Start()

            If Not (m_oPaymentGroups Is Nothing) Then

                m_oPaymentGroups.Dispose()
                m_oPaymentGroups = Nothing
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdPaymentGroups_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdPaymentGroups_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_3.Click, _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Dim lLoop As Integer

        Try

            lLoop = Index
            Do While lLoop > -1
                If SSTabHelper.GetTabVisible(tabMainTab, lLoop) Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, lLoop)
                    If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                        m_ctlTabFirstLast(ACControlStart, lLoop).Focus()
                    End If
                    Exit Do
                End If
                lLoop -= 1
            Loop

        Catch

            ' Error Section

            Exit Sub
        End Try

    End Sub

    'EK 7/10/99 Access to Commission Rate Maintenance
    Private Sub cmdRates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRates.Click

        Try

            'Create shares object if not already done so
            If m_oRates Is Nothing Then

                ' Get an instance of the shares interface object via
                ' the public object manager.
                Dim temp_m_oRates As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oRates, sClassName:="iPMBInsurerRateFind.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oRates = temp_m_oRates

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get shares object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRates_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oRates.SetProcessModes(vTask:=m_iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            m_oRates.PartyCnt = m_lPartyCnt

            m_oRates.PartyName = m_sName

            m_oRates.AddressCnt = m_lMainAddressCnt

            m_oRates.ADdresses = VB6.CopyArray(m_vAddresses)
            '160605 Datasure

            m_oRates.IsDomiciledForTax = m_bDomiciledForTax
            If m_bTaxExempt Then

                m_oRates.IsTaxExempt = 1
            Else

                m_oRates.IsTaxExempt = 0
            End If

            m_lReturn = m_oRates.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdRates_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRates_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdStargate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdStargate.Click
        Const ACClass As String = "cmdStargate_Click"

        'Create shares object if not already done so
        If m_oStargateConfig Is Nothing Then

            ' Get an instance of the Stargate Config interface object via
            ' the public object manager.
            Dim temp_m_oStargateConfig As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oStargateConfig, sClassName:="iSGConfigManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oStargateConfig = temp_m_oStargateConfig

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GoTo Err_cmdStargate_Click
            End If

        End If

        'Start the app ...

        m_lReturn = m_oStargateConfig.Start(m_lSGInsurerID)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Err_cmdStargate_Click
        End If

Exit_cmdStargate_Click:
        If Not (m_oStargateConfig Is Nothing) Then

            m_oStargateConfig.Dispose()
            m_oStargateConfig = Nothing
        End If
        Exit Sub

Err_cmdStargate_Click:
        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Stargate Config Business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdStargate_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        m_lReturn = gPMConstants.PMEReturnCode.PMError
        GoTo Exit_cmdStargate_Click

    End Sub

    Private Sub cmdRiskGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRiskGroup.Click
        Dim lStatus As gPMConstants.PMEReturnCode


        Dim oListItem As ListViewItem = lvwAddresses.FocusedItem

        'eck100402New Form to
        '    With frmRiskGroup
        '        .RiskGroupId = oListItem.SubItems(6)
        '        .LookupValues = m_vLookupValues
        '        .LookupDetails = m_vLookupDetails
        '   End With

        '    frmRiskGroup.Show vbModal
        '
        '    With frmRiskGroup
        '        lStatus = .Status
        '        vRiskGroupId = .RiskGroupId
        '     End With
        '
        '    Unload frmRiskGroup
        '    Set frmRiskGroup = Nothing

        With frmRiskGroups
            Dim dbNumericTemp As Double
            If Double.TryParse(ListViewHelper.GetListViewSubItem(oListItem, 6).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                .RiskGroupId = m_vAddressRiskGroups(CInt(ListViewHelper.GetListViewSubItem(oListItem, 6).Text))
            Else

                .RiskGroupId = ""
            End If
            .LookupValues = VB6.CopyArray(m_vLookupValues)
            .LookupDetails = VB6.CopyArray(m_vLookupDetails)
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.

        Dim tempLoadForm As frmRiskGroups = frmRiskGroups

        frmRiskGroups.ShowDialog()

        With frmRiskGroups
            lStatus = .Status
            If lStatus = gPMConstants.PMEReturnCode.PMOK Then
                'eck030502
                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(ListViewHelper.GetListViewSubItem(oListItem, 6).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    ReDim Preserve m_vAddressRiskGroups(m_vAddressRiskGroups.GetUpperBound(0) + 1)
                    ListViewHelper.GetListViewSubItem(oListItem, 6).Text = CStr(m_vAddressRiskGroups.GetUpperBound(0))
                End If
                'eck030502

                m_vAddressRiskGroups(CInt(ListViewHelper.GetListViewSubItem(oListItem, 6).Text)) = .RiskGroupId
            End If
        End With

        frmRiskGroups.Close()
        frmRiskGroups = Nothing

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            ' Set tab 1 to be current so that controls on other tabs are disabled and so
            ' can't be tabbed to  PN15893
            tabMainTab_SelectedIndexChanged(tabMainTab, New EventArgs())

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Dim vValue As Object = Nothing

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyIN.Business", vInstanceManager:="ClientManager")
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

            'TN10072000
            'get hidden system option from business object

            m_sUnderwritingOrAgency = m_oBusiness.UnderwritingOrAgency

            'JMK 19/10/2001 - get second hidden option

            m_sUnderwritingType = m_oBusiness.UnderwritingType

            m_bGeminiIILink = m_oBusiness.GeminiIILink

            ' CTAF 040800
            'DC 04/10/00 check for GeminiII no longer required
            'If (m_bGeminiIILink = True) Then

            Dim temp_m_oGISListManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oGISListManager, "iGISListManager.InterfaceNoLogin", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oGISListManager = temp_m_oGISListManager
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iGISListManager.Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            ' Check the list versions

            m_lReturn = m_oGISListManager.CheckListVersions(v_sGISDataMOdelCode:="RSA")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            'DC 04/10/00
            'End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMBPartyIN.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            m_bShowSubBranchID = True

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Set language
            m_oFormFields.LanguageID = g_iLanguageID

            'RDT PN18099
            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

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

    Private Function GetUserAuthorities() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oUserAuthorities As bACTUserAuthorities.Business


            Dim temp_oUserAuthorities As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTUserAuthorities.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            End If

            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="is_view_only_insurer_maintenance", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=m_bIsViewOnlyInsurerMaintenance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            Else
                'Start - Prakash Varghese - PN 61117
                'Modified the condition ToSafeBoolean(m_bIsViewOnlyInsurerMaintenance) = True
                'to ToSafeBoolean(m_bIsViewOnlyInsurerMaintenance) since it is giving problems in runtime
                If gPMFunctions.ToSafeBoolean(m_bIsViewOnlyInsurerMaintenance) Then
                    m_iTask = gPMConstants.PMEComponentAction.PMView
                End If
                'End - Prakash Varghese - PN 61117
            End If

            'Terminate the object

            oUserAuthorities.Dispose()
            oUserAuthorities = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthoritiesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim bIsStargateInstalled As Boolean

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

            ' Get User Authorities for Setting up the mode
            m_lReturn = GetUserAuthorities()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Exit Sub
            End If

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

            'Gary Williams 13/05/2004 PN11890 - check if business
            'Underwriting if so hide Risk button
            cmdRiskGroup.Visible = False

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt
            ' {* USER DEFINED CODE (End) *}

            'DC150803 -PS254 -fsa compliance
            m_lReturn = GetHiddenOption(v_lSourceId:=g_iSourceID, r_vEnableFSACompliance:=m_bEnableFSACompliance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'DC110706 PN29315 set New Zealand Configuration option
            m_lReturn = GetHiddenOptionNZ(v_lSourceId:=g_iSourceID, r_vEnableNZConfig:=m_bEnableNZConfig)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Set g_oListManager = New iGEMListManager.Interface
            'm_lReturn = g_oListManager.Initialise()

            'm_lReturn = g_oListManager.CheckListVersions()

            ' Alix - 15/01/2003 - PN9610
            ' Moved this here from further down, as this flag is
            ' used in GetInterfaceDetails()
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iMainAddressIndex = 0) Then
                m_iIsReInsurer = 1
            End If
            ' /Alix

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

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            '    If (m_iTask% = PMAdd) Then
            'And if the insurer has no addresses and so hasn't set up the main address index
            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iMainAddressIndex = 0) Then
                'eck030502
                ReDim m_vAddressRiskGroups(0)

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                End If

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                        Exit For
                    End If

                Next i

            End If

            'DP 11/11/2002 - Check to see whether Stargate is installed. If so, make the
            'Stargate button visible
            bIsStargateInstalled = IsStargateInstalled()
            cmdStargate.Visible = False

            m_bSetupComplete = True

            'Maintain Party Code
            If Task = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = SetClientCodeCntl()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

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

            ' Terminate the address object (if used)
            If Not (m_oAddress Is Nothing) Then

                m_oAddress.Dispose()
                ' Destroy the instance of the Address object
                ' from memory.
                m_oAddress = Nothing

            End If

            ' Terminate the contact object (if used)
            If Not (m_oContact Is Nothing) Then

                m_oContact.Dispose()

                ' Destroy the instance of the contact object
                ' from memory.
                m_oContact = Nothing

            End If
            'EK 7/10/99 Access to IPT Extras
            ' Terminate the policy shares object (if used)
            If Not (m_oIPTExtras Is Nothing) Then

                m_oIPTExtras.Dispose()

                ' Destroy the instance of the policy shares object
                ' from memory.
                m_oIPTExtras = Nothing

            End If
            'EK 7/10/99 Access to Commission Rates
            ' Terminate the policy shares object (if used)
            If Not (m_oRates Is Nothing) Then

                m_oRates.Dispose()

                ' Destroy the instance of the policy shares object
                ' from memory.
                m_oRates = Nothing

            End If

            ' CTAF 040800
            'DC 04/10/00 check for GeminiII no longer required
            'If (m_bGeminiIILink = True) Then

            ' Terminate the object
            m_oGISListManager.Dispose()

            ' Remove the instance
            m_oGISListManager = Nothing
            'DC 04/10/00
            'End If

            If Not (m_oFindPartyBusiness Is Nothing) Then
                ' Terminate the FidParty business object

                m_oFindPartyBusiness.Dispose()

                ' Destroy the instance of the find party business object
                ' from memory.
                m_oFindPartyBusiness = Nothing

            End If
            'Set the arrays to nothing
            m_vAddresses = Nothing
            m_vAddressTypes = Nothing
            m_vContacts = Nothing
            m_vABICodes = Nothing

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
            'Developer Guide No 293
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
                tabMainTab.SelectedIndex = 0
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D2 Then
                tabMainTab.SelectedIndex = 1
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D3 Then
                tabMainTab.SelectedIndex = 2
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D4 Then
                tabMainTab.SelectedIndex = 3
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D5 Then
                tabMainTab.SelectedIndex = 4
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D6 Then
                tabMainTab.SelectedIndex = 5
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D7 Then
                tabMainTab.SelectedIndex = 6
            End If
        Catch

            ' Error Section.

            Exit Sub
        End Try

    End Sub

    Private Sub lvwAddresses_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Click

        If Not (lvwAddresses.FocusedItem Is Nothing) Then

            m_lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)
            m_iLine = lvwAddresses.FocusedItem.Index + 1
        End If

    End Sub

    Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        'Developer Guide No.70
        Dim y As Single = eventArgs.Y
        If lvwAddresses.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False
            'eck081101
            cmdRiskGroup.Enabled = False
        ElseIf gPMFunctions.ToSafeBoolean(m_bIsViewOnlyInsurerMaintenance) = True Then  'Sumit 1556 if this is read-only mode Edit and Delete is disabled for the address tab
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False
        Else
            cmdEditAd.Enabled = True
            cmdDeleteAd.Enabled = True
            'eck081101
            cmdRiskGroup.Enabled = True
        End If

    End Sub

    Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        'Developer Guide No.70
        Dim y As Single = eventArgs.Y

        If lvwContacts.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False
        Else
            cmdEditCon.Enabled = True
            cmdDeleteCon.Enabled = True
        End If

    End Sub

    ' ***************************************************************** '
    ' Name          : mnuFinancial_Click
    '
    ' Edit History  :
    ' RAM20020820   : ProcessToolbar is Modified to send the Button Key
    '                   rather than button index
    ' ***************************************************************** '

    'Private Sub mnuFinancial_Click()
    '
    ' TF291298
    '
    'Try 
    '
    ' Call Toolbar Control function
    'm_lReturn = ProcessToolbar(v_sButtonKey:=ACIButtonFinancialKey)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Continue as not serious
    'Exit Sub
    'End If
    '
    'Catch 
    '
    '
    '
    'Continue as not serious
    'Exit Sub
    'End Try
    '
    '
    'End Sub

    ' ***************************************************************** '
    ' Name          : mnuLetter_Click
    '
    ' Edit History  :
    ' RAM20020820   : ProcessToolbar is Modified to send the Button Key
    '                   rather than button index
    ' ***************************************************************** '

    'Private Sub mnuLetter_Click()
    '
    ' TF291298
    '
    'Try 
    '
    ' Call Toolbar Control function
    'm_lReturn = ProcessToolbar(v_sButtonKey:=ACIButtonLetterKey)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Continue as not serious
    'Exit Sub
    'End If
    '
    'Catch 
    '
    '
    '
    'Continue as not serious
    'Exit Sub
    'End Try
    '
    '
    'End Sub

    ' ***************************************************************** '
    ' Name          : mnuNotes_Click
    '
    ' Edit History  :
    ' RAM20020820   : ProcessToolbar is Modified to send the Button Key
    '                   rather than button index
    ' ***************************************************************** '

    'Private Sub mnuNotes_Click()
    '
    ' TF291298
    '
    'Try 
    '
    ' Call Toolbar Control function
    'm_lReturn = ProcessToolbar(v_sButtonKey:=ACIButtonNotesKey)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Continue as not serious
    'Exit Sub
    'End If
    '
    'Catch 
    '
    '
    '
    'Continue as not serious
    'Exit Sub
    'End Try
    '
    '
    'End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' To control tabbing correctly we need to only enable controls on the current tab PN15893

                Select Case SSTabHelper.GetSelectedIndex(tabMainTab)
                    Case 0
                        Frame3.Enabled = True
                        fraAppointment.Enabled = True
                        fraReinsurance.Enabled = True
                        cmdNext(0).Enabled = True

                        fraAddress.Enabled = False
                        cmdPrevious(0).Enabled = False
                        cmdNext(1).Enabled = False

                        fraContact.Enabled = False
                        cmdPrevious(1).Enabled = False

                    Case 1
                        Frame3.Enabled = False
                        fraAppointment.Enabled = False
                        fraReinsurance.Enabled = False
                        cmdNext(0).Enabled = False

                        fraAddress.Enabled = True
                        cmdPrevious(0).Enabled = True
                        cmdNext(1).Enabled = True

                        fraContact.Enabled = False
                        cmdPrevious(1).Enabled = False

                    Case 2
                        Frame3.Enabled = False
                        fraAppointment.Enabled = False
                        fraReinsurance.Enabled = False
                        cmdNext(0).Enabled = False

                        fraAddress.Enabled = False
                        cmdPrevious(0).Enabled = False
                        cmdNext(1).Enabled = False

                        fraContact.Enabled = True
                        cmdPrevious(1).Enabled = True
                End Select

                ' Set focus to the first control on the tab.
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If

            End With

        Catch

            ' Error Section.

            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            'JB 050199 Added check for Referenece id(shortname)

            'Check that a reference id has been entered

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
            End If

            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'update the party cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                '**************************************************
                ' save additional details back to party record.
                m_lReturn = UpdatePartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
                '**************************************************

                'Update party addresses
                m_lReturn = UpdateAddresses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                ' CTAF 021000 - Update Orion
                m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                'Update party contacts
                m_lReturn = UpdateContacts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                m_lReturn = UpdateRiskCodes()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Risk Code Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                    Exit Sub
                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()

                'DC260106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

                Dim oPartyBusiness As bSIRParty.Business
                Dim temp_oPartyBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oPartyBusiness = temp_oPartyBusiness
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If

                oPartyBusiness.AddPartyHistory(m_lPartyCnt, String.Empty)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If
            Else
                Exit Sub
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

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

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

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_3.Click, _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Dim lLoop As Integer

        Try

            lLoop = Index + 1
            Do While lLoop < SSTabHelper.GetTabCount(tabMainTab)
                If SSTabHelper.GetTabVisible(tabMainTab, lLoop) Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, lLoop)
                    If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                        m_ctlTabFirstLast(ACControlStart, lLoop).Focus()
                    End If
                    Exit Do
                End If
                lLoop += 1
            Loop

        Catch

            ' Error Section

            Exit Sub
        End Try

    End Sub

    ' ***************************************************************** '
    ' Name          : Toolbar1_ButtonClick
    '
    ' Edit History  :
    ' RAM20020820   : 1. Changed the Button.Index to Button.Key
    '                 2. Added an optional parameter to pass the party_cnt
    ' ***************************************************************** '
    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar1_Button1.Click, _Toolbar1_Button2.Click, _Toolbar1_Button3.Click, _Toolbar1_Button4.Click, _Toolbar1_Button5.Click, _Toolbar1_Button6.Click, _Toolbar1_Button7.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        ' TF291298

        Try

            ' Call Toolbar Control function
            ' RAM20020820 : Added code to sent the Party Cnt
            m_lReturn = SharedFiles.PMBToolbarFunc.ProcessToolbar(Button.Name, m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch

            'Continue as not serious
            Exit Sub
        End Try

    End Sub

    Private Sub txtAgencyNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgencyNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgencyNumber)
    End Sub

    Private Sub txtAgencyNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgencyNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgencyNumber)
    End Sub

    Private Sub txtClaimsRatingDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtClaimsRatingDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtClaimsRatingDate)
    End Sub

    Private Sub txtDefaultCommissionRate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDefaultCommissionRate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDefaultCommissionRate)
    End Sub

    Private Sub txtDefaultCommissionRate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDefaultCommissionRate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDefaultCommissionRate)
    End Sub

    Private Sub txtIDReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtIDReference)
    End Sub

    Private Sub txtIDReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtIDReference)
    End Sub

    Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtName)
    End Sub

    Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtName)
    End Sub

    Private Sub uctABINumber_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctABINumber.GotFocus

        'm_lReturn = m_oFormFields.GotFocus(ctlControl:=uctABINumber)

    End Sub

    Private Sub uctABINumber_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctABINumber.LostFocus

        'm_lReturn = m_oFormFields.LostFocus(ctlControl:=uctABINumber)

    End Sub

    ' ***************************************************************** '
    ' Name: UpdateOrion
    '
    ' Description: Update the party_address usage table with old
    ' and new addresses for the party.
    '
    ' CTAF 021000 - Taken from uctPartyPCControl (by ECK)
    '
    ' ***************************************************************** '
    Private Function UpdateOrion(ByRef vPartyCnt As Object) As Integer
        Dim result As Integer = 0
        Dim oSIROrionUpdate As bSIROrionUpdate.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oSIROrionUpdate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIROrionUpdate, "bSIROrionUpdate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSIROrionUpdate = temp_oSIROrionUpdate

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIROrionUpdate.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            ' CTAF 021000 - Commented out the source code for now until it's
            '               decided what to do about insurers and changing sources

            ' Get Orion Account IDs
            ' eck010900
            '    Select Case Task
            '        Case PMAdd

            m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt)
            '        Case PMEdit
            '            m_lReturn = oSIROrionUpdate.SiriusToOrion( _
            ''                            v_lPartyCnt:=vPartyCnt, _
            ''                            v_iOldSourceId:=m_iPartySourceId, _
            ''                            v_iOldPartyId:=m_iPartyId)
            '    End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIROrionUpdate.Dispose()

            oSIROrionUpdate = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DP - Added for Stargate
    Private Function IsStargateInstalled() As Boolean
        Const sMethod_Name As String = "IsStargateInstalled"

        Dim sValue As String = ""
        Dim bRet As Boolean

        Try

            'assume fail
            bRet = False

            ' If there is a PM\Stargate\Client\"version" registry item value then Stargate
            ' is deemed to be installed PN18533
            If gPMFunctions.GetPMRegSetting(gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, gPMConstants.PMEProductFamily.pmePFStargate, gPMConstants.PMERegSettingLevel.pmeRSLClient, "version", sValue) = gPMConstants.PMEReturnCode.PMTrue And sValue <> "" Then
                bRet = True
            End If

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="IsStargateInstalled Failed", vApp:=ACApp, vClass:="RegistryFunctions", vMethod:=sMethod_Name, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            bRet = False

        End Try

    End Function

    Private Sub uctBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctBranch.SelectionChangeCommitted

        If uctBranch.SelectedIndex < 1 Then
            ' the default (none) selected
            cboSubBranch.SelectedIndex = -1
            Exit Sub
        End If
        'eck25102005 Prevent crash where sub branch array not set up

        If Not (Convert.IsDBNull(m_vSubBranch) Or IsNothing(m_vSubBranch)) Then

            m_lReturn = GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)
        End If

        'DC130706 PN29329 only show if subbranch is visible
        If cboSubBranch.Visible Then

            If m_bShowSubBranchID Then
                cboSubBranch.SelectedIndex = -1
            Else
                cboSubBranch.SelectedIndex = 0
            End If
        End If
        ' RDC 20042004
        m_lReturn = GetSourceBaseCurrency()

    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetSubBranchDetails
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetSubBranchDetails(ByRef r_oSubBranch As ComboBox, ByRef r_oBranch As ComboBox, ByRef r_oBusiness As Object, ByVal v_lSubBranchId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACSubBranchId As Integer = 0
            Const ACSubBranchDescription As Integer = 3

            Dim lIndex, lSourceId As Integer
            Dim vSubBranchArray(,) As Object

            r_oSubBranch.Items.Clear()

            lIndex = r_oBranch.SelectedIndex
            If lIndex < 0 Then
                Return result
            End If

            'RDT PN18099
            lSourceId = VB6.GetItemData(r_oBranch, uctBranch.SelectedIndex)

            m_lReturn = r_oBusiness.GetSubBranches(v_lSourceId:=lSourceId, r_vSubBranchArray:=vSubBranchArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If
            If Not Information.IsArray(vSubBranchArray) Then
                Return result
            End If

            For i As Integer = 0 To vSubBranchArray.GetUpperBound(1)
                'Extra white spaces are removed from subbranch description
                Dim r_oSubBranch_NewIndex As Integer = -1

                r_oSubBranch_NewIndex = r_oSubBranch.Items.Add(CStr(vSubBranchArray(ACSubBranchDescription, i)).Trim())

                VB6.SetItemData(r_oSubBranch, r_oSubBranch_NewIndex, CInt(vSubBranchArray(ACSubBranchId, i)))

                If CInt(vSubBranchArray(ACSubBranchId, i)) = v_lSubBranchId Then
                    r_oSubBranch.SelectedIndex = r_oSubBranch_NewIndex
                End If
            Next i

            If v_lSubBranchId = 0 Then
                r_oSubBranch.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSourceBaseCurrency
    '
    ' Description: Set base currency when user changes source
    '
    ' History: 20042004 RDC created
    '
    ' ***************************************************************** '
    Private Function GetSourceBaseCurrency() As Integer
        Dim result As Integer = 0
        Dim iBaseCurrencyID As Integer
        Dim lSourceId As Integer

        Dim oPartyBusiness As bSIRParty.Business

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If uctBranch.SelectedIndex < 1 Then
                ' no branch selected
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' we need bSIRParty.Business
            Dim temp_oPartyBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPartyBusiness = temp_oPartyBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")

                Return result
            End If

            'RDT PN 18099
            lSourceId = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)

            ' this value SHOULD exist, but more error trapping here?

            ' call the business. to get the Base Currency ID

            m_lReturn = oPartyBusiness.GetBaseCurrencyID(lSourceId:=lSourceId, iCurrencyID:=iBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get base currency for selected branch", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")

                Return result
            End If

            cboCurrency.CompanyId = lSourceId
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = iBaseCurrencyID

            'PN32510 - Set Default Base currency
            If m_lCurrencyId <= 0 Then
                m_lCurrencyId = iBaseCurrencyID
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSourceBaseCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBranchDetails
    '
    ' Description: Gets all of the branch details
    '
    ' Updates: RDT PN18099 - Added new Get Branch method, taken from PartyPC
    '
    ' ***************************************************************** '
    Private Function GetBranchDetails() As Integer
        Dim result As Integer = 0
        Dim vSourceArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        'Only populate combo with addresses the user is authorised to access.

        m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_oObjectManager.UserID, v_bIncludeDeletedSources:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get branch details for the dropdown list")
        End If

        'PN 19428

        m_vSourceArray = vSourceArray

        'Clear combo.
        uctBranch.Items.Clear()

        Dim uctBranch_NewIndex As Integer = -1
        uctBranch_NewIndex = uctBranch.Items.Add("(none)")
        VB6.SetItemData(uctBranch, uctBranch_NewIndex, 0)

        'Populate branch combo

        For i As Integer = 1 To vSourceArray.GetUpperBound(1)
            'Add using branch description (3).

            uctBranch_NewIndex = uctBranch.Items.Add(CStr(vSourceArray(3, i)).Trim())

            VB6.SetItemData(uctBranch, uctBranch_NewIndex, CInt(vSourceArray(1, i)))

            If CInt(vSourceArray(1, i)) = m_vBranch Then
                uctBranch.SelectedIndex = uctBranch_NewIndex
            End If
        Next i

        uctBranch.SelectedIndex = 0

        Return result

        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the branch details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description: Returns additional party details from party record
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetPartyDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPartyDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oBusiness.GetPartyDetails(v_lPartyCnt:=m_lPartyCnt, r_vResults:=vPartyDetails)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetPartyDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Information.IsArray(vPartyDetails) Then

                m_sTaxNumber = CStr(vPartyDetails(kPartyDetailTaxNumber, 0))

                'Developer Guide No.98
                'start change
                m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailDomiciledForTax, 0), 0)

                m_bTaxExempt = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailTaxExempt, 0), 0)

                m_dTaxPercentage = gPMFunctions.ToSafeDouble(vPartyDetails(kPartyDetailTaxPercentage, 0), 0)
                'end change
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function UpdatePartyDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPartyDetails As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(BuildPartyDetailArray(r_vPartyDetails:=vPartyDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BuildPartyDetailArray Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' update party details

            lReturn = m_oBusiness.UpdatePartyDetails(v_lPartyCnt:=m_lPartyCnt, v_vPartyDetails:=vPartyDetails)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "UpdatePartyDetails", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: BuildPartyDetailArray
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 18-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function BuildPartyDetailArray(ByRef r_vPartyDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BuildPartyDetailArray"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim r_vPartyDetails(3, 0)

            If m_sTaxNumber <> "" Then

                r_vPartyDetails(kPartyDetailTaxNumber, 0) = m_sTaxNumber
            Else

                r_vPartyDetails(kPartyDetailTaxNumber, 0) = DBNull.Value
            End If

            If Not m_bDomiciledForTax Then

                r_vPartyDetails(kPartyDetailDomiciledForTax, 0) = 0
            Else

                r_vPartyDetails(kPartyDetailDomiciledForTax, 0) = 1
            End If

            If Not m_bTaxExempt Then

                r_vPartyDetails(kPartyDetailTaxExempt, 0) = 0
            Else

                r_vPartyDetails(kPartyDetailTaxExempt, 0) = 1
            End If

            If m_dTaxPercentage = 0 Then

                r_vPartyDetails(kPartyDetailTaxPercentage, 0) = DBNull.Value
            Else

                r_vPartyDetails(kPartyDetailTaxPercentage, 0) = m_dTaxPercentage
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    '040506 Datasure
    Private Sub uctPartyTax1_IsDomiciledForTaxChanged(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPartyTax1.IsDomiciledForTaxChanged
        m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(Me.uctPartyTax1.IsDomiciledForTax)
    End Sub

    Private Function CheckPaymentGroup() As Integer
        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oPaymentGroup As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPaymentGroup, "bACTInsurerPaymentGroups.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPaymentGroup = temp_m_oPaymentGroup

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

                Return result
            End If

            'Get AccountId from Payment Group

            m_lReturn = m_oPaymentGroup.GetAccountId(lPartyCnt:=m_lPartyCnt, lAccountID:=m_lAccountID)

            'Get Payment Details from Payment Group

            m_lReturn = m_oPaymentGroup.GetDetails(lAccountID:=m_lAccountID, vPaymentGroups:=m_vPaymentGroups)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                m_oPaymentGroup.Dispose()
                m_oPaymentGroup = Nothing

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPaymentGroup")

                Return result
            End If

            If Not Information.IsArray(m_vPaymentGroups) Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentGroupTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentGroupMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oPaymentGroup.Dispose()
            m_oPaymentGroup = Nothing

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the Payment Group Details", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPaymentGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function PopulateRiskCodes() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateRiskCodes"

        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vRiskCodes) Then

                'Make sure the list is clear
                lvwRiskCodes.Items.Clear()
                m_lAddressCount = 0

                'Loop through each risk code and add it to the list
                For lLoop As Integer = m_vRiskCodes.GetLowerBound(1) To m_vRiskCodes.GetUpperBound(1)

                    'Add the line with the risk code description
                    oListItem = lvwRiskCodes.Items.Add(CStr(m_vRiskCodes(1, lLoop)).Trim())

                    'Add the risk transfer column value
                    'Developer Guide No.98
                    If gPMFunctions.ToSafeBoolean(m_vRiskCodes(2, lLoop)) Then
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Yes"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "No"
                    End If

                    'Add the delegated authority column value
                    'Developer Guide No.98
                    If gPMFunctions.ToSafeBoolean(m_vRiskCodes(3, lLoop)) Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No"
                    End If

                    'Add the position in the array to the tag
                    oListItem.Tag = CStr(lLoop)

                Next

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

    Private Function UpdateRiskCodes() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateRiskCodes"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Update the array with any changes
            For Each oListItem As ListViewItem In lvwRiskCodes.Items
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Yes" Then
                    m_vRiskCodes(2, Convert.ToString(oListItem.Tag)) = 1
                Else
                    m_vRiskCodes(2, Convert.ToString(oListItem.Tag)) = 0
                End If
                If ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes" Then
                    m_vRiskCodes(3, Convert.ToString(oListItem.Tag)) = 1
                Else
                    m_vRiskCodes(3, Convert.ToString(oListItem.Tag)) = 0
                End If
            Next oListItem

            'Save the changes to the database

            m_lReturn = m_oBusiness.UpdateRiskCodes(v_lPartyCnt:=m_lPartyCnt, v_vRiskCodes:=m_vRiskCodes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.UpdateRiskCodes", "v_lPartyCnt:=" & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
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
    ' Name: SelectBureau
    '
    ' Description: Gets the Bureau details
    '
    ' History:
    '           Created : DeepakM : 06-02-2007 : PVY London Market
    ' *****************************************************************
    Private Function SelectBureau(ByRef v_vPartyCnt As Integer, ByRef v_vShortName As String, ByRef v_vName As String, ByRef v_vSpecialParty As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SelectBureau"
        Const kCallingApp As String = "iPMBPartyIN"

        'Developer Guide No. 109
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 109
            oFindParty = New iPMBFindParty.Interface_Renamed()

            m_lReturn = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (Not False) And (Not String.IsNullOrEmpty(v_vSpecialParty)) Then

                ReDim vKeyArray(1, 1)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "special_party"

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_vSpecialParty 'Setting Special key for Insurer ("IN")

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyBureauAccount

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = 1

                m_lReturn = oFindParty.SetKeys(vKeyArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            oFindParty.CallingAppName = kCallingApp

            m_lReturn = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vEffectiveDate:=DateTime.Now)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.NotEditable = CShort(gPMConstants.PMEReturnCode.PMTrue)

            m_lReturn = oFindParty.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                v_vPartyCnt = oFindParty.PartyCnt
                v_vShortName = oFindParty.ShortName
                v_vName = oFindParty.LongName
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()

            oFindParty = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            If Not (oFindParty Is Nothing) Then
                oFindParty.Dispose()
                oFindParty = Nothing
            End If
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectBureau Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetClientCodeCntl
    '
    ' Description:
    '
    ' History: VB
    '
    ' ***************************************************************** '
    Private Function SetClientCodeCntl() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object

        Const kMethodName As String = "SetClientCodeCntl"
        Dim r_bIsReadOnly, r_bIsNumberingSchemeExists As Boolean

        Dim oClientNumber As bSIRPolicyNumMaint.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If oClientNumber Is Nothing Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError("SetClientCodeCntl", "Can not create object of bSIRPolicyNumMaint.Business")
                    Return result
                End If
            End If

            m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeInsurer, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails Failed")
                Return result
            End If

            m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
            m_bIsReadOnly = r_bIsReadOnly

            If r_bIsNumberingSchemeExists And r_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
            ElseIf r_bIsNumberingSchemeExists And Not r_bIsReadOnly Then
                lblIDReference.Enabled = True
                txtIDReference.Enabled = True
            Else
                lblIDReference.Enabled = True
                txtIDReference.Enabled = True
            End If



        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function GeneratePartyCode() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object

        Const kMethodName As String = "GeneratePartyCode"
        Dim sFailureReason, sGeneratedClientCode, sInsurerType As String

        Dim oClientNumber As bSIRPolicyNumMaint.Business
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsSetMaskingCode And txtIDReference.Text = "" Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "Unable to get instance of  bSIRPolicyNumMaint.Business")
                    Return result
                End If

                sGeneratedClientCode = InsurerName
                sInsurerType = InsurerType

                m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeInsurer, v_iSourceID:=g_iSourceID, r_sGeneratedClientCode:=sGeneratedClientCode, r_sFailureReason:=sFailureReason, v_sType:=sInsurerType)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "GenerateClientCode failed")
                    Return result
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then  'Numbering Scheme not set
                    MessageBox.Show("Numbering scheme for Insurer is not set.", "Insurer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                ElseIf sFailureReason <> "" Then
                    MessageBox.Show(sFailureReason, "Insurer", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return result
                End If

                txtIDReference.Text = sGeneratedClientCode
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
            End If



        Catch ex As Exception
            ' Log Error Message
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function DeleteAddress(ByVal Value As Integer, ByVal vAddress As Object(,)) As Integer
        Dim nResult As Integer = 1
        Const kMethodName As String = "DeleteAddress"

        Try
            For i As Integer = 0 To vAddress.GetUpperBound(1)
                If Value = CInt(vAddress(0, i)) Then
                    nResult = 0
                    Exit For
                End If
            Next

            Return nResult
        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            Return nResult
        End Try
    End Function
End Class