Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Diagnostics
Imports System.Drawing
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Imports System.Text

<System.Runtime.InteropServices.ProgId("uctPartyPCControl_NET.uctPartyPCControl")>
Partial Public Class uctPartyPCControl
	Inherits System.Windows.Forms.UserControl
    Implements IDisposable
	Public Event FromEventChange()
	Public Event InitialsChange()
	Public Event TitleChange()
	Public Event ForenameChange()
	Public Event SurnameChange()
	Public Event IDReferenceChange()
	Public Event BorderStyleChange()
	Public Event BackStyleChange()
	Public Event FontChange()
	Public Event EnabledChange()
	Public Event ForeColorChange()
	Public Event BackColorChange()
	Public Event PartyCntChange()
	Public Event EffectiveDateChange()
	Public Event TransactionTypeChange()
	Public Event ProcessModeChange()
	Public Event NavigateChange()
	Public Event TaskChange()
	Public Event StatusChange()
	Public Event CallingAppNameChange()
	' ***************************************************************** '
	' Form Name: frmInterface
	'
	' Date: 23/06/1998
	'
	' Description: Main interface.
	'
	' Edit History:
	'
	' DJM 10/06/2002 : Don't delete the insured record in function UpdateLifestyle.
	' TF  03/12/1998 : Menu & Toolbar activity
	' RAW 18/11/2002 : PS005 : Add tab6 for customer loyalty scheme
	' RKS 20/01/2005 : Implemented Duplicate Client Identification
	' MAE 27-09-2005 : Development 358 - Client BlackListing
	' CJB 15/12/2005 : PN26445 Changed PopulateProspect to avoid errors when adding new client
    ' ***************************************************************** '
    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document
    'developer guide no. 107
    Public g_oObjectManager As bObjectManager.ObjectManager
    Public g_oListManager As Object
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPartyPCControl"

    'Default Property Values:
    Const m_def_BackColor As Integer = 0
    Const m_def_ForeColor As Integer = 0
    Const m_def_Enabled As Integer = 0
    Const m_def_BackStyle As Integer = 0
    Const m_def_BorderStyle As Integer = 0
    Const m_def_PartyCnt As Integer = 0

    'Property Variables:
    Dim m_BackColor As Integer
    Dim m_ForeColor As Integer
    Dim m_Enabled As Boolean
    Dim m_Font As Font
    Dim m_BackStyle As Integer
    Dim m_BorderStyle As BorderStyle
    Dim m_PartyCnt As Integer


    'Event Declarations:
    Shadows Event Click(ByVal Sender As Object, ByVal e As EventArgs)
    Event DblClick(ByVal Sender As Object, ByVal e As EventArgs)
    Shadows Event KeyDown(ByVal Sender As Object, ByVal e As KeyDownEventArgs)
    Shadows Event KeyPress(ByVal Sender As Object, ByVal e As KeyPressEventArgs)
    Shadows Event KeyUp(ByVal Sender As Object, ByVal e As KeyUpEventArgs)
    Shadows Event MouseDown(ByVal Sender As Object, ByVal e As MouseDownEventArgs)
    Shadows Event MouseMove(ByVal Sender As Object, ByVal e As MouseMoveEventArgs)
    Shadows Event MouseUp(ByVal Sender As Object, ByVal e As MouseUpEventArgs)


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    'JDW

    Dim m_oQASAddress As bSIRAddress.Business
    'bSIRAddress.business

    Public Structure QASNamesData
        Dim Title As String
        Dim Forename As String
        Dim Surname As String
        Dim Initial As String
        Dim Postcode As String
        Dim OrgName As String
        Dim Add1 As String
        Dim Add2 As String
        Dim Add3 As String
        Dim Add4 As String
        Dim IsOrg As Boolean
        Public Shared Function CreateInstance() As QASNamesData
            Dim result As New QASNamesData
            result.Title = String.Empty
            result.Forename = String.Empty
            result.Surname = String.Empty
            result.Initial = String.Empty
            result.Postcode = String.Empty
            result.OrgName = String.Empty
            result.Add1 = String.Empty
            result.Add2 = String.Empty
            result.Add3 = String.Empty
            result.Add4 = String.Empty
            Return result
        End Function
    End Structure

    Private oQNAddress As QASNamesData = QASNamesData.CreateInstance()

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iProspectTask As gPMConstants.PMEComponentAction

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lAddressCount As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the Lock object.
    Private m_oPMLock As bPMLock.User

    'eck010900

    Private m_oPMUser As bPMUser.Business

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
    Private m_ctlTabFirstLast(1, 5) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    Private Const vbScrollBars As String = "0x80000000"
    Private Const vbDesktop As String = "0x80000001"
    Private Const vbActiveTitleBar As String = "0x80000002"
    Private Const vbInactiveTitleBar As String = "0x80000003"
    Private Const vbMenuBar As String = "0x80000004"
    Private Const vbWindowBackground As String = "H00FFFFC0"
    Private Const vbWindowFrame As String = "0x80000006"
    Private Const vbMenuText As String = "0x80000007"
    Private Const vbWindowText As String = "0x80000008"
    Private Const vbTitleBarText As String = "0x80000009"
    Private Const vbActiveBorder As String = "0x8000000A"
    Private Const vbInactiveBorder As String = "0x8000000B"
    Private Const vbApplicationWorkspace As String = "0x8000000C"
    Private Const vbHighlight As String = "0x8000000D"
    Private Const vbHighlightText As String = "0x8000000E"
    Private Const vbButtonFace As String = "0x8000000F"
    Private Const vbButtonShadow As String = "0x80000010"
    Private Const vbGrayText As String = "0x80000011"
    Private Const vbButtonText As String = "0x80000012"
    Private Const vbInactiveCaptionText As String = "0x80000013"
    Private Const vb3DHighlight As String = "0x80000014"
    Private Const vb3DDKShadow As String = "0x80000015"
    Private Const vb3DLight As String = "0x80000016"
    Private Const vbInfoText As String = "0x80000017"
    Private Const vbInfoBackground As String = "0x80000018"

    'Data retrieved via Get Details from Party & PartyPC
    'DC 25/09/00
    Private m_sOldIdReference As String = ""
    Private m_sOldResolvedName As String = ""

    Private m_lPartyCnt As Integer
    'DC 03/05/00
    'Multi-Branch
    Private m_iPartySourceId As Integer
    'eck010900
    'DN 01/06/01 - Change PartyIDs to Longs
    Private m_iNewPartySourceId As Integer
    Private m_lPartyId As Integer
    Private m_lNewPartyId As Integer
    '
    Private m_iPartyTypeId As Integer
    Private m_sPartyTitleCode As String = ""
    Private m_sForeName As String = ""
    Private m_sInitials As String = ""
    Private m_sResolved As String = ""
    Private m_sEmploymentStatusCode As String = ""
    Private m_sEmployerBusiness As String = ""
    Private m_sSecondaryEmploymentStatusCode As String = ""
    Private m_sSecondaryEmployerBusiness As String = ""
    Private m_sMaritalStatusCode As String = ""
    Private m_lNumberOFChildren As Integer
    'DC 28/09/00 was integer
    'developer guide no. 101
    Private m_vNationalityId As Object
    Private m_iMailshot As CheckState
    Private m_iIsPetOwner As CheckState
    Private m_sAccommodationTypeCode As String = ""
    Private m_sShortName As String = ""
    Private m_sSurName As String = ""
    Private m_iIsAlsoAgent As CheckState
    Private m_iIsProspect As CheckState
    Private m_lAgentCnt As Integer
    Private m_lConsultantCnt As Integer
    Private m_iAreaId As Integer
    Private m_sFileCode As String = ""
    Private m_iCurrencyId As Integer
    Private m_sPaymentMethodCode As String = ""
    Private m_lReminderTypeID As Integer
    Private m_lServiceLevelId As Integer
    Private m_sCreditCardCode As String = ""

    Private m_lCCJs As Integer

    Private m_vSeasonalGiftId As Object
    'DC 28/06/00
    'developer guide no. 101
    Private m_vCorrespondenceTypeId As Object
    'Tomo060700
    'developer guide no. 101
    Private m_vRenewalStopCodeId As Object
    Private m_dtTobLetter As Date 'FSA Phase III
    '2005 Screen Layout Change
    Private m_cYearToDateTurnover As Decimal
    Private m_cLastYearTurnover As Decimal
    Private m_cClientBalance As Decimal

    'Data retrieved via Get Details from PartyLifeStyle
    Private m_lPartyLifeStyleId As Integer
    Private m_sPartyLifeStyleName As String = ""
    Private m_dtDOB As Date
    Private m_lCategory As Integer
    Private m_sOccupation As String = ""
    Private m_sSecondaryOccupation As String = ""
    Private m_sGender As String = ""
    Private m_iIsSmoker As CheckState

    'References from Party Lookups
    Private m_sAgentRef As String = ""
    Private m_sAgentName As String = ""
    Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""
    'developer guide no. 101
    Private m_vAssociates As Object
    Private m_vRelationships As Object

    'DC 28/06/00
    Private m_vContactTypes As Object
    Private m_vCorrespondenceTypes(,) As Object

    Private m_vStrengthCodeId As Object
    Private m_vPreviousInsurerCnt As Object
    Private m_vPreviousBrokerCnt As Object
    Private m_sInsurerRef As String = ""
    Private m_sInsurerName As String = ""
    Private m_sBrokerRef As String = ""
    Private m_sBrokerName As String = "" ' DC 03/05/00
    Private m_sSalutation As String = ""

    Private m_sLoyaltyNumber As String = ""
    Private m_sAlternativeIdentifier As String = ""
    Private m_sTradingName As String = ""
    Private m_lSubBranchId As Integer

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_sMainPostCode As String = ""
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_vConvictions(,) As Object
    Private m_sAddressLine1 As String = ""
    Private m_vLifestyle(,) As Object
    Private m_vLoyaltySchemes(,) As Object
    Private m_vPersonTypes As Object
    Private m_vPersonSex As Object
    'Flags to indicate whether we need to check the employer/agent ids match
    'the employer/agent ref as user may change the reference directly
    Private m_bVerifyAgentCnt As Boolean
    Private m_bVerifyConsultantCnt As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    Private m_vOldAddress(,) As Object
    Private m_vNewAddress(,) As Object

    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""


    ' Declare an instance of the address interface.

    'developer guide no. 88
    Private m_oAssociates As Object

    ' Declare an instance of the address interface.
    Private m_oAddress As Object

    ' Declare an instance of the contact interface.
    Private m_oContact As Object

    ' Declare an instance of the lifestyle interface.
    Private m_oLifestyle As Object

    ' Declare an instance of the conviction interface.
    Private m_oConviction As Object

    Private m_oPartyLoyaltyScheme As Object

    '2005 Layout Changes

    Private m_oAccount As bACTAccount.Form

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_bChangedProspect As Boolean
    Private m_lCurrentAgent As Integer
    Private m_sCurrentAgentRef As String = ""
    Private m_sCurrentAgentName As String = ""
    Private m_bVerifyCurrentAgentCnt As Boolean
    Private m_bChangedProspectPolicies As Boolean

    Private m_sCaption As String = ""
    Private m_sOKCaption As String = ""
    Private m_sHelpCaption As String = ""
    Private m_sCancelCaption As String = ""

    Private m_bEvent As Boolean

    Private m_sUnderwritingOrBroking As String = ""

    Private m_lSwiftPartyID As Integer


    Private m_oLifestyleBusiness As bSIRLifeStyle.Business
    Private m_bUserMode As Boolean
    Private m_bIsNRMA As Boolean
    Private m_bValidateAlternativeIdentifier As Boolean
    Private m_sBranchPrefix As String = ""
    Private m_sLoyaltyNumberScript As String = ""
    Private m_sAlternativeIdentifierScript As String = ""
    Private m_bAONAffinity As Boolean
    Private m_bFutureDateAddressChanges As Boolean
    Private m_vFutureDatedAddresses As Object
    Private m_bUpdateFutureDatedAddress As Boolean

    Private m_bMultiTreeAccounting As Boolean

    Private m_bLimitPersonalClientEditFields As Boolean

    Private m_sUnderwritingOrAgency As String = ""


    Private m_sSource As String = ""
    Private m_iTPSind As CheckState
    Private m_iEMPSind As CheckState
    Private m_sTPPassword As String = ""

    Private m_bErrorMessageAlreadyShown As Boolean

    Private m_bAONPRClientScreenChanges As Boolean

    Private m_bIncludeClosedBranchChecked As Boolean
    Private m_bShowSubBranchID As Boolean
    Private m_bDuplicateClientIdentification As Boolean
    Private m_bSystemOptionClientBlacklistingInForce As Boolean
    Private m_bSystemOptionEnhancedResolvedName As Boolean
    Private m_bSystemOptionUpdateExistingClients As Boolean

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double
    '**************************************************
    Private m_vSourceArray(,) As Object
    Private m_bIsAmended As Boolean
    'developer guide no. 101
    Private m_vBlackListReasonId As Object
    Private m_iPaymentTermId As Integer

    'MIPS Client Numbering
    'developer guide no. 9
    Private m_oClientNumber As bSIRPolicyNumMaint.Business
    'developer guide no. 101
    Private m_vIsFeeClient As Object

    'Maintain Party Code
    Private m_bIsReadOnly As Boolean
    Private m_bIsSetMaskingCode As Boolean
    Private m_sMaskCode As String = ""

    'For PN-43232
    Private m_sCurrentResolvedName As String = ""

    'Party Bank Details
    Private m_vPartyBankDetails(,) As Object
    Private m_vPartyBankHistory As Object

    ' {* USER DEFINED CODE (End) *}
    ' PRIVATE Data Members (End)
    ' PUBLIC Property Procedures (Begin)


    <Browsable(True)>
    Public Property CurrentResolvedName() As String
        Get
            Return m_sCurrentResolvedName
        End Get
        Set(ByVal Value As String)
            m_sCurrentResolvedName = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property Controls_Renamed() As Object
        Get
            Return Me.Controls_Renamed
        End Get
    End Property

    <Browsable(False)>
    Public ReadOnly Property ErrorNumber() As Integer
        Get

            ' Standard Property.

            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber

        End Get
    End Property

    <Browsable(False)>
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value
            RaiseEvent CallingAppNameChange()

        End Set
    End Property


    <Browsable(True)>
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
            RaiseEvent StatusChange()

        End Set
    End Property


    <Browsable(True)>
    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_iTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the objects task.
            m_iTask = Value
            RaiseEvent TaskChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value
            RaiseEvent NavigateChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value
            RaiseEvent ProcessModeChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value
            RaiseEvent TransactionTypeChange()

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value
            RaiseEvent EffectiveDateChange()

        End Set
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    ' CTAF 270900
    <Browsable(True)>
    Public Property SwiftPartyID() As Integer
        Get
            Return m_lSwiftPartyID
        End Get
        Set(ByVal Value As Integer)
            m_lSwiftPartyID = Value
        End Set
    End Property

    'eck120500
    <Browsable(True)>
    Public Property PartySourceID() As Integer
        Get

            Return m_iPartySourceId

        End Get
        Set(ByVal Value As Integer)

            m_iPartySourceId = Value

        End Set
    End Property


    <Browsable(True)>
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value
            RaiseEvent PartyCntChange()

        End Set
    End Property

    ' {* USER DEFINED CODE (End) *}


    <Browsable(True)>
    Public Shadows Property BackColor() As Integer
        Get
            Return m_BackColor
        End Get
        Set(ByVal Value As Integer)
            m_BackColor = Value
            RaiseEvent BackColorChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property ForeColor() As Integer
        Get
            Return m_ForeColor
        End Get
        Set(ByVal Value As Integer)
            m_ForeColor = Value
            RaiseEvent ForeColorChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property Enabled() As Boolean
        Get
            Return m_Enabled
        End Get
        Set(ByVal Value As Boolean)
            m_Enabled = Value
            RaiseEvent EnabledChange()
        End Set
    End Property


    <Browsable(True)>
    Public Overrides Property Font() As Font
        Get
            Return m_Font
        End Get
        Set(ByVal Value As Font)
            m_Font = Value
            RaiseEvent FontChange()
        End Set
    End Property


    <Browsable(True)>
    Public Property BackStyle() As Integer
        Get
            Return m_BackStyle
        End Get
        Set(ByVal Value As Integer)
            m_BackStyle = Value
            RaiseEvent BackStyleChange()
        End Set
    End Property


    <Browsable(True)>
    Public Shadows Property BorderStyle() As Integer
        Get
            Return m_BorderStyle
        End Get
        Set(ByVal Value As Integer)
            m_BorderStyle = Value
            RaiseEvent BorderStyleChange()
        End Set
    End Property


    <Browsable(True)>
    Public Property IDReference() As String
        Get
            Return txtIDReference.Text
        End Get
        Set(ByVal Value As String)
            txtIDReference.Text = Value
            RaiseEvent IDReferenceChange()
        End Set
    End Property


    <Browsable(True)>
    Public Property Surname() As String
        Get
            Return txtSurname.Text
        End Get
        Set(ByVal Value As String)
            txtSurname.Text = Value
            RaiseEvent SurnameChange()
        End Set
    End Property

    'JDW added for CNIC 23/08/2001

    <Browsable(True)>
    Public Property Forename() As String
        Get
            Return txtForename.Text
        End Get
        Set(ByVal Value As String)
            txtForename.Text = Value
            RaiseEvent ForenameChange()
        End Set
    End Property


    <Browsable(True)>
    Public Property Title() As String
        Get
            Return ddTitle.Text
        End Get
        Set(ByVal Value As String)
            ddTitle.Text = Value
            RaiseEvent TitleChange()
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property TradeName() As String
        Get
            Return txtTradingName.Text
        End Get
    End Property


    <Browsable(True)>
    Public Property Initials() As String
        Get
            Return txtInitials.Text
        End Get
        Set(ByVal Value As String)
            txtInitials.Text = Value
            RaiseEvent InitialsChange()
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Postcode() As Object
        Set(ByVal Value As Object)

        End Set
    End Property


    <Browsable(True)>
    Public Property FromEvent() As Boolean
        Get
            Return m_bEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bEvent = Value
            RaiseEvent FromEventChange()
        End Set
    End Property
    'JT PN-13238
    <Browsable(True)>
    Public Property IsIncludeClosedBranchChecked() As Boolean
        Get
            Return m_bIncludeClosedBranchChecked
        End Get
        Set(ByVal Value As Boolean)
            m_bIncludeClosedBranchChecked = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property BranchId() As Integer
        Get
            Dim result As Integer = 0
            If cboBranch.SelectedIndex > -1 Then
                result = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            End If
            Return result
        End Get
    End Property

    'Party Bank Details
    <Browsable(False)>
    Public ReadOnly Property PartyBankDetails() As Object
        Get
            Return VB6.CopyArray(m_vPartyBankDetails)
        End Get
    End Property
    'Party Bank Details
    <Browsable(False)>
    Public ReadOnly Property PartyBankHistory() As Object
        Get
            Return m_vPartyBankHistory
        End Get
    End Property

    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)

    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)


    ' ***************************************************************** '
    ' Author        : jes
    ' Description   : Wrapper for AddQASAddress
    ' Edit History  : Created 20 August 2002
    ' ***************************************************************** '
    Public Function AddQASAddress(ByRef v_sTitle As String, ByRef v_sForename As String, ByRef v_sSurname As String, ByRef v_sInitial As String, ByRef v_sPostCode As String, ByRef v_sOrgName As String, ByRef v_sAdd1 As String, ByRef v_sAdd2 As String, ByRef v_sAdd3 As String, ByRef v_sAdd4 As String, ByRef v_bIsOrg As Boolean) As Boolean

        Dim result As Boolean = False
        Try

            Dim oData As QASNamesData = QASNamesData.CreateInstance()

            oData.Title = v_sTitle
            oData.Forename = v_sForename
            oData.Surname = v_sSurname
            oData.Initial = v_sInitial
            oData.Postcode = v_sPostCode
            oData.OrgName = v_sOrgName
            oData.Add1 = v_sAdd1
            oData.Add2 = v_sAdd2
            oData.Add3 = v_sAdd3
            oData.Add4 = v_sAdd4
            oData.IsOrg = v_bIsOrg


            '*********** ERROR HANDLING
            Return AddQASAddress1(QAS:=oData)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add address", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQASAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    Private Function AddQASAddress1(ByRef QAS As QASNamesData) As Boolean
        Dim result As Boolean = False
        Try


            result = True

            Dim oItem As ListViewItem
            Dim lBusinessDataID As Integer
            lBusinessDataID = 1


            'add address
            Dim temp_m_oQASAddress As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oQASAddress, "bSIRAddress.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oQASAddress = temp_m_oQASAddress


            m_lReturn = m_oQASAddress.EditAdd(lRow:=lBusinessDataID, vAddress1:=QAS.Add1, vAddress2:=QAS.Add2, vAddress3:=QAS.Add3, vAddress4:=QAS.Add4, vPostalCode:=QAS.Postcode, vCountryID:=m_iDefaultCountryID)

            Debug.WriteLine(m_oQASAddress.AddressCnt)

            m_oQASAddress.Update()

            'add address to list view
            oItem = lvwAddresses.Items.Add(QAS.Postcode)
            ListViewHelper.GetListViewSubItem(oItem, 1).Text = "Correspondence Address"
            ListViewHelper.GetListViewSubItem(oItem, 2).Text = QAS.Add1
            ListViewHelper.GetListViewSubItem(oItem, 3).Text = QAS.Add2
            ListViewHelper.GetListViewSubItem(oItem, 4).Text = QAS.Add3
            ListViewHelper.GetListViewSubItem(oItem, 5).Text = QAS.Add4

            oItem.Tag = m_oQASAddress.AddressCnt

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQASAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQASAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: CancelClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CancelClick() As Integer

        Dim result As Integer = 0



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'eck 2005 Roadmap
            '    If (tabProspecting.Visible = True) Then
            '
            '        If (m_bChangedProspect = True) _
            ''        Or (m_bChangedProspectPolicies = True) Then
            '            sTitle$ = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lId:=ACCancelDetailsTitle, _
            ''                iDataType:=PMResString)
            '
            '            sMessage$ = iPMFunc.GetResData( _
            ''                iLangID:=g_iLanguageID%, _
            ''                lId:=ACCancelDetails, _
            ''                iDataType:=PMResString)
            '
            '            iMsgResult = MsgBox(sMessage$, _
            ''                vbYesNo + vbDefaultButton2 + vbQuestion, sTitle$)
            '
            '            ' Check message result.
            '            If (iMsgResult = vbNo) Then
            '                ' Set return to false, meaning
            '                ' don't cancel.
            '                CancelClick = PMFalse
            '                Exit Function
            '            End If
            '        End If
            '
            '        tabProspecting.Visible = False
            '        TabMainTab.Top = 0
            '        TabMainTab.Visible = True
            '        'This stops it closing the form
            '        CancelClick = PMFalse
            '        Exit Function
            '    End If

            If TabMainTab.Visible Then
                Return CancelParty()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CancelParty
    '
    ' Description: Called when we wish to cancel any changes
    '
    ' ***************************************************************** '
    Private Function CancelParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                'Me.Hide
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Cancel the party", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetParty
    '
    ' Description: Gets the interface details and sets the appropriate
    '              style.
    '
    ' ***************************************************************** '
    Public Function GetParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    'If in edit mode, lock the party
            '    If (m_iTask = PMEdit) Then
            '
            '        m_lReturn = LockParty
            '
            '        If (m_lReturn <> PMTrue) Then
            '            GetParty = PMFalse
            '            Exit Function
            '        End If
            '
            '    End If

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMEdit Or Task = gPMConstants.PMEComponentAction.PMView Then
                ' Get the interface details from the
                ' business object.
                m_lReturn = GetBusiness()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Assign the details from the business object
                ' to the interface.
                m_lReturn = BusinessToInterface()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the details.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'sj 17/06/2002 - start
                m_lReturn = GetDefaultAgentDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'sj 17/06/2002 - end
                LoadPartyBankControl()
            End If


            ' Display all of the lookup details.
            m_lReturn = DisplayLookupDetails()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '2005 Roadmap changes
            'Prospect Now on the main tab
            PopulateProspect()

            ' Check the task.
            If Task = gPMConstants.PMEComponentAction.PMView Then
                ' Disable the interface to only allow viewing.
                m_lReturn = DisableForm(lDisabled:=True)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to disable the interface
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the party", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'eck011001
    ' ***************************************************************** '
    ' Name: ApplyParty
    '
    ' Description: Sets necessary business parameters to allow save after apply
    '
    ' ***************************************************************** '
    Public Function ApplyParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = LockParty()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to disable the interface
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to disable the interface
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the party", vApp:=ACApp, vClass:=ACClass, vMethod:="ApplyParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function Initialise() As Integer

        Dim nResult As Integer = 0
        Static bIsInitialised As Boolean
        Dim sTitle As String
        Dim sMessage As String
        Dim sHelpFile As String
        Dim sValue As String
        Dim m_lReturn As Integer

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If bIsInitialised Then
                Return nResult
            End If

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                nResult = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return nResult
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                nResult = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return nResult
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
                g_iCurrencyId = .CurrencyID 'PN16993
                'RWH(24/07/2000) RSAIB Process 004.
                m_iDefaultCountryID = .CountryID
            End With

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return nResult
            End If

            If sHelpFile <> "" Then
                'App.HelpFile = sHelpFile
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyPC.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                nResult = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If


            m_sUnderwritingOrBroking = "U"

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return nResult
            End If

            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return nResult
            End If

            ' SET 09082002 - Following lines removed cos
            ' new build has problem creating this object
            ' May have to re-instate...
            Dim temp_g_oListManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oListManager, sClassName:="iGEMListManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            g_oListManager = temp_g_oListManager
            PMBGeneralFunc.g_oListManager = g_oListManager

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iGEMListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return nResult
            End If


            m_lReturn = PMBGeneralFunc.g_oListManager.CheckListVersions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get latest list manager files.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return nResult
            End If

            '2005 New Client Screen Layout
            Dim temp_m_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTAccount", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return nResult
            End If

            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCurrencyConvert", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return nResult
            End If
            '2005 End
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' hold Initialised status
            bIsInitialised = True

            m_lReturn = PartyFunc.GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vIsNRMA:=m_bIsNRMA, r_vValidateAlternativeIdentifier:=m_bValidateAlternativeIdentifier, r_vAONAffinity:=m_bAONAffinity, r_vFutureDateAddressChanges:=m_bFutureDateAddressChanges, r_vMultiTreeAccounting:=m_bMultiTreeAccounting, r_vLimitPersonalClientEditFields:=m_bLimitPersonalClientEditFields, r_vAONPRClientScreenChanges:=m_bAONPRClientScreenChanges)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for FSA Compliance", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue <> "1" Then
                fraFSA.Visible = False
                fraFSA.Enabled = False
            End If

            iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)

            'Broking doesn't show Sub Branch by default
            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            m_bShowSubBranchID = (sValue = "1") Or (m_sUnderwritingOrAgency = "U")


            'Retrieve System Option for Duplicate Client Identification
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionDuplicateClientIdentification, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Duplicate Client Identificaiton", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bDuplicateClientIdentification = (sValue = "1")

            ' Get System Option for Client BlackListing
            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionClientBlacklistingInForce, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Client BlackListing In Force", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bSystemOptionClientBlacklistingInForce = (sValue = "1")

            ' Get System Option for Enhanced Resolved Name
            m_lReturn = iPMFunc.GetSystemOption(GeneralConst.kSystemOptionEnhancedResolvedName, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Enhanced Resolved Name", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bSystemOptionEnhancedResolvedName = (sValue = "1")

            m_lReturn = iPMFunc.GetSystemOption(kSystemOptionUpdateExistingClients, sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionValue Failed for Update Existing Clients", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_bSystemOptionUpdateExistingClients = (sValue = "1")

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description: Does all the extra stuff that initialise doesn't
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0

        ' Forms load event.

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt
            ' {* USER DEFINED CODE (End) *}
            If m_lPartyCnt > 0 Then
                m_lReturn = LogClientViewedEvent(m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error but continue processing
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log client viewed event", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
                End If
            End If

            m_oFormFields = New iPMFormControl.FormFields()

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddressTypeLookups(vAddressTypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


                    Return gPMConstants.PMEReturnCode.PMFalse

                End If
                'developer guide no. 131
                If (Not m_vAddressTypes Is Nothing) Then
                    'Set the index of the main address
                    For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                        'See if this is the main address
                        If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                            'EK 11/10/99 Set Index to AdressType
                            '                m_iMainAddressIndex = i
                            m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                            Exit For
                        End If

                    Next i
                End If 'end if'
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            ' Error Section

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load control", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function LogClientViewedEvent(lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oPartyBusiness As bSIRParty.Business

            ' we need bSIRParty.Business
            Dim temp_oPartyBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oPartyBusiness, "bSIRParty.Business", gPMConstants.PMGetViaClientManager)
            oPartyBusiness = temp_oPartyBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oPartyBusiness.LogClientViewedEvent(lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log client viewed event", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LogClientViewedEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LogClientViewedEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: What is this supposed to do?
    '
    ' ***************************************************************** '
    Public Overrides Sub Refresh()

    End Sub

    ' ***************************************************************** '
    ' Name: SaveParty
    '
    ' Description: Saves the displayed party details
    ' Edit History :
    ' RAM20031002  : Added code to restrict '|' or "," delimiter in Code and Long Names
    '                PN Issue No. 6021
    '
    ' ***************************************************************** '
    Private Function SaveParty() As Integer

        Dim result As Integer = 0
        Dim i2 As Integer
        Dim iPartyFound As gPMConstants.PMEReturnCode
        Dim sOriginalReference As String = ""
        Dim iNumber As Integer
        Dim lPartyCnt As Integer
        Dim iMax, iMax2 As Integer

        Dim sSelectedClientCode As String = ""
        Dim lSelectedClientPartyCnt As Integer
        Dim iOKAction As Integer

        'This is a temporary option for MIPS Australia
        Dim bM9Option As Boolean
        Dim vValue, sClientcode, sInitials As String
        ' Click event of the OK button.
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check mandatory controls have been entered into.
            m_lReturn = CheckMainMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTM9PartyCodeFormat, 1, vValue)
            bM9Option = gPMFunctions.ToSafeBoolean(vValue, False)

            If Not bM9Option Then
                If m_sUnderwritingOrBroking = "U" Then
                    iMax = 13
                    iMax2 = 3
                Else
                    iMax = 8
                    iMax2 = 1
                End If

                ' Modified to remove apostrophes from auto created shortcodes
                sOriginalReference = txtIDReference.Text

                If txtIDReference.Text = "" Then

                    If m_sUnderwritingOrAgency = "U" And m_bIsSetMaskingCode Then

                        'Restrict characters for the code
                        sClientcode = txtSurname.Text.Trim()
                        sClientcode = sClientcode.Replace(" ", "")
                        sClientcode = sClientcode.Replace("'", "")
                        sClientcode = sClientcode.Replace("|", "")
                        sClientcode = sClientcode.Replace(",", "")

                        sInitials = txtInitials.Text.Trim()
                        sInitials = sInitials.Replace(" ", "")
                        sInitials = sInitials.Replace("'", "")
                        sInitials = sInitials.Replace("|", "")
                        sInitials = sInitials.Replace(",", "")

                        'If m_bIsSetMaskingCode Then
                        m_lReturn = ValidateNumberingScheme()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If
                        'End If
                        'Generate Client Code
                        m_lReturn = GetClientCode(r_sClientCode:=sClientcode, v_sInitial:=sInitials, v_sValue:=sClientcode)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sClientcode = "") Then
                            Return m_lReturn
                        Else
                            txtIDReference.Text = sClientcode.Trim().ToUpper()
                        End If
                    Else
                        i2 = 0
                        For i As Integer = 1 To Strings.Len(txtSurname.Text)
                            If i2 = iMax Then
                                Exit For
                            End If
                            ' RAM20031002 : Restrict , and | characters for the code
                            If txtSurname.Text.Substring(i - 1, 1) = " " Or txtSurname.Text.Substring(i - 1, 1) = "'" Or txtSurname.Text.Substring(i - 1, 1) = "|" Or txtSurname.Text.Substring(i - 1, 1) = "," Then
                            Else
                                i2 += 1
                                txtIDReference.Text = txtIDReference.Text & txtSurname.Text.Substring(i - 1, 1)
                            End If
                        Next i

                        If Strings.Len(txtInitials.Text) > 0 Then
                            txtIDReference.Text = txtIDReference.Text & Strings.Left(txtInitials.Text, iMax2)
                        End If
                        txtIDReference.Text = txtIDReference.Text.ToUpper()
                    End If



                    ' DC 28/09/00 See if code already exists
                    If m_sUnderwritingOrBroking = "U" Then
                        iNumber = 1
                    Else
                        iNumber = 0
                    End If

                    iPartyFound = gPMConstants.PMEReturnCode.PMTrue
                    sOriginalReference = txtIDReference.Text

                    Do While iPartyFound = gPMConstants.PMEReturnCode.PMTrue


                        m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtIDReference.Text.Trim(), vPartyCnt:=lPartyCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return result
                        End If

                        If lPartyCnt <> 0 Then
                            iNumber += 1
                            If m_sUnderwritingOrBroking = "U" Then
                                If iNumber > 999 Then
                                    MessageBox.Show("Client code counter exceeds 999." & Strings.Chr(13) & Strings.Chr(10) &
                                                    "You must manually enter a unique client code.", Application.ProductName)
                                    iPartyFound = gPMConstants.PMEReturnCode.PMFalse
                                Else
                                    txtIDReference.Text = sOriginalReference & StringsHelper.Format(CStr(iNumber), "000")
                                End If
                            Else
                                txtIDReference.Text = sOriginalReference & CStr(iNumber)
                            End If
                        Else
                            iPartyFound = gPMConstants.PMEReturnCode.PMFalse
                        End If

                    Loop

                End If








                'Duplicate Client Identification
                If m_bDuplicateClientIdentification Then
                    If sOriginalReference <> txtIDReference.Text Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                        'Show Duplicate Client interface
                        m_lReturn = ShowDuplicateParty(sOriginalClientCode:=sOriginalReference, sUniqueClientCode:=txtIDReference.Text, sSelectedClientCode:=sSelectedClientCode, lSelectedClientPartyCnt:=lSelectedClientPartyCnt, iOKAction:=iOKAction)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                            txtIDReference.Text = ""
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If m_lReturn = gPMConstants.PMEReturnCode.PMOK Then
                            If iOKAction = kAbandonNewRecordandUseSelectedClient Then
                                'Open Selected Client in Edit Mode
                                Me.IDReference = sSelectedClientCode
                                Me.PartyCnt = lSelectedClientPartyCnt

                                m_oBusiness.PartyCnt = lSelectedClientPartyCnt
                                Return gPMConstants.PMEReturnCode.PMTrue

                            End If
                        End If

                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                    End If
                End If
            Else
                If m_sShortName <> "" Then
                    txtIDReference.Text = m_sShortName.Trim()
                Else
                    txtIDReference.Text = "M900000000"
                End If
            End If

            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                ' Update the party cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                If bM9Option Then
                    'The reference has already been overridden in the database
                    'but we need to do it here.
                    'PN 70111
                    If m_iTask <> gPMConstants.PMEComponentAction.PMEdit Then txtIDReference.Text = "M9" & StringsHelper.Format(m_lPartyCnt, "00000000")
                    m_sShortName = txtIDReference.Text
                End If

                ' save additional details back to party record.
                m_lReturn = UpdatePartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' Update party addresses
                m_lReturn = UpdateAddresses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")

                    Return result
                End If

                'AR20050214 - PN18407
                m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Orion details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")

                    Return result

                End If

                'Update party contacts
                m_lReturn = UpdateContacts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")

                    Return result
                End If

                'Update Associates

                '       Cater for more than one Associate

                m_lReturn = m_oBusiness.UpdateAssociates(vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Associates", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")

                    Return result
                End If

                'DN 19/10/00 - Call to update Gemini if installed

                m_lReturn = m_oBusiness.UpdateGemini(vPartyCnt:=m_lPartyCnt, vTask:=m_iTask)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Gemini", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")

                    Return result
                End If
                'DN 19/10/00 - End

                ''Update Orion if UW, Swift if SFORB
                'If m_sUnderwritingOrBroking = "U" Then
                '    'Finally, update Orion again so that the contact info we've changed since
                '    'can be done...


                m_lReturn = UpdateOrion(vPartyCnt:=m_oBusiness.PartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion (again)", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
                    Return result
                End If

                'developer guide no. 9
                m_lReturn = uctPartyBankControl1.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("SaveParty", "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Party Bank Details
                uctPartyBankControl1.PartyCnt = m_lPartyCnt
                m_lReturn = uctPartyBankControl1.UpdatePartyBankDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("SaveParty", "uctPartyBankControl1.UpdatePartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'DJM 14/05/2002 : Update party dependents
                m_lReturn = UpdateLifestyle()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Dependent Details", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")

                    Return result
                End If

                'sj 23/07/2002 - start
                If m_bFutureDateAddressChanges Then

                    m_lReturn = m_oBusiness.CreateFutureDatedAddresses(v_vFutureDatedAddresses:=m_vFutureDatedAddresses, v_lPartyCnt:=m_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty")
                        Return result
                    End If
                End If
                'sj 23/07/2002 - end
                ' Everything OK, so we can hide the interface.
                'Me.Hide
                result = gPMConstants.PMEReturnCode.PMTrue

            Else
                'sj 20/10/2003 - Start
                If Not m_bErrorMessageAlreadyShown Then
                    'sj 20/10/2003 - End
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on ProcessCommand", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    'sj 20/10/2003 - Start
                End If
                m_bErrorMessageAlreadyShown = False
                'sj 20/10/2003 - End
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the party", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: ShowHelpScreen
    '
    ' Description: Shows the help screen
    '
    ' ***************************************************************** '
    Public Function ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing) As Integer
        ' Fire up the help screen
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        Return PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)


    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
            For i As Integer = m_ctlTabFirstLast.GetLowerBound(1) To m_ctlTabFirstLast.GetUpperBound(1)
                m_ctlTabFirstLast(ACControlStart, i) = Nothing
                m_ctlTabFirstLast(ACControlEnd, i) = Nothing
            Next i
                If PMBGeneralFunc.g_oListManager IsNot Nothing Then
                    PMBGeneralFunc.g_oListManager.Dispose()
                PMBGeneralFunc.g_oListManager = Nothing
            End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                g_oObjectManager = Nothing
            End If
            If Not (m_oPMLock Is Nothing) Then

                'If in edit mode, unlock the party
                If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                    m_lReturn = UnlockParty()


                End If

                m_oPMLock = Nothing
            End If
                If m_oPMUser IsNot Nothing Then
                    m_oPMUser.Dispose()
                m_oPMUser = Nothing
            End If
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                m_oAccount = Nothing
            End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                m_oCurrencyConvert = Nothing
            End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                m_oBusiness = Nothing
            End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
                m_oFormFields = Nothing
            End If
                If m_oAddress IsNot Nothing Then
                    m_oAddress.Dispose()
                m_oAddress = Nothing
            End If
                If m_oContact IsNot Nothing Then
                    m_oContact.Dispose()
                m_oContact = Nothing
                End If
                If m_oLifestyle IsNot Nothing Then
                    m_oLifestyle.Dispose()
                m_oLifestyle = Nothing
                End If
                If m_oConviction IsNot Nothing Then
                    m_oConviction.Dispose()
                m_oConviction = Nothing
                End If
                If m_oPartyLoyaltyScheme IsNot Nothing Then
                    m_oPartyLoyaltyScheme.Dispose()
                    m_oPartyLoyaltyScheme = Nothing
            End If
                If m_oClientNumber IsNot Nothing Then
                    m_oClientNumber.Dispose()
                    m_oClientNumber = Nothing
                End If


            End If
            End If
        Me.disposedValue = True
    End Sub

    Private Const vbFormCode As Integer = 0
    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description: Cleans up then unloads the control
    '
    ' ***************************************************************** '
    Public Function UnLoadControl(ByRef Cancel As Integer, ByRef UnloadMode As Integer) As Integer

        ' Forms query unload event.

        Dim result As Integer = 0
        Debug.WriteLine("unload control")

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then

                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = ProcessCommand()

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    'eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Return result
                End If
            End If

            'If in edit mode, unlock the party
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                m_lReturn = UnlockParty()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Dispose()
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="UnLoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function ValidateParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Return ValidateOK()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vPartyTitleCode:=m_sPartyTitleCode, vSourceID:=m_iPartySourceId, vPartyID:=m_lPartyId, vForename:=m_sForeName, vInitials:=m_sInitials, vEmploymentStatusCode:=m_sEmploymentStatusCode, vEmployerBusiness:=m_sEmployerBusiness, vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, vSecondaryEmployerBusiness:=m_sSecondaryEmployerBusiness, vMaritalStatusCode:=m_sMaritalStatusCode, vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_vNationalityId, vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, vAccommodationTypeCode:=m_sAccommodationTypeCode, vShortName:=m_sShortName, vName:=m_sSurName, vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId, vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs, vSeasonalGiftID:=m_vSeasonalGiftId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId, vSwiftPartyID:=m_lSwiftPartyID, vSalutation:=m_sSalutation, vLoyaltyNumber:=m_sLoyaltyNumber, vAlternativeIdentifier:=m_sAlternativeIdentifier, vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter, vSource:=m_sSource, vTPSind:=m_iTPSind, vEMPSInd:=m_iEMPSind, vTPPassword:=m_sTPPassword, vIsFeeClient:=m_vIsFeeClient)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            ' returns additional party details
            ' these detail are not returned from get next because of parameter limit of 60
            m_lReturn = GetPartyDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get additional details required for display that not stored on this
            'record

            m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=m_lAgentCnt, vAgentref:=m_sAgentRef, vAgentName:=m_sAgentName, vConsultantCnt:=m_lConsultantCnt, vConsultantRef:=m_sConsultantRef, vConsultantName:=m_sConsultantName, vInsurerCnt:=m_vPreviousInsurerCnt, vInsurerRef:=m_sInsurerRef, vInsurerName:=m_sInsurerName, vBrokerCnt:=m_vPreviousBrokerCnt, vBrokerRef:=m_sBrokerRef, vBrokerName:=m_sBrokerName, vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            If IsArray(m_vAssociates) Then
                lblAssociates.Visible = True
            Else
                lblAssociates.Visible = False
            End If

            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresse type lookups for the party

            m_lReturn = m_oBusiness.GetAddressTypeLookups(vAddressTypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get contacts for the party

            m_lReturn = m_oBusiness.GetContactDetails(vContacts:=m_vContacts)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get convictions for the Party

            m_lReturn = m_oBusiness.GetConvictionDetails(vPartyCnt:=m_lPartyCnt, vConviction:=m_vConvictions)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the conviction details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get relationships for the Party

            m_lReturn = m_oBusiness.GetLifestyleDetails(vPartyCnt:=m_lPartyCnt, vLifestyle:=m_vLifestyle)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the lifestyle details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'DC 28/06/00
            'Get contact types

            m_lReturn = m_oBusiness.GetContactTypes(vContactTypes:=m_vContactTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the contact types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'DC 28/06/00
            'Get correspondence types

            m_lReturn = m_oBusiness.GetCorrespondenceTypes(vCorrespondenceTypes:=m_vCorrespondenceTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'RAW 18/11/2002 : PS005 : Added - Begin
            'Get loyalty schemes for the Party

            m_lReturn = m_oBusiness.GetLoyaltySchemeDetails(vPartyCnt:=m_lPartyCnt, vLoyaltySchemes:=m_vLoyaltySchemes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the Loyalty Scheme details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            'RAW 18/11/2002 : PS005 : End

            '2005 Layout Changes - Get Client Account Details figures

            m_lReturn = m_oAccount.GetClientAccountDetails(v_lAccountKey:=m_lPartyCnt, v_lCompanyID:=m_iPartySourceId, r_curYearToDateTurnover:=m_cYearToDateTurnover, r_curLastYearTurnover:=m_cLastYearTurnover, r_curClientBalance:=m_cClientBalance)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve turnover details from the account business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If
            '2005 End

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve turnover details from the account business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        '2005 Screen Layout

        Dim sFormattedCurrency As String = ""

        '2005End
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
            ddTitle.Text = m_sPartyTitleCode
            cboAccommodation.Text = m_sAccommodationTypeCode
            '2005 Client Screen Layout Changes
            '   m_lReturn = m_oCurrencyConvert.GetBaseCurrency( _
            ''       v_lCompanyID:=m_iPartySourceId, _
            ''       r_iBaseCurrencyID:=iBaseCurrencyID)

            'No need for conversion as the client balance amount retrieved is the account amount which is already in account currency.
            'm_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cClientBalance, cCurrencyAmount:=cConvertedAmount)

            'm_cClientBalance = cConvertedAmount


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cClientBalance, vFormattedCurrency:=sFormattedCurrency)

            pnlClientBalance.Text = sFormattedCurrency

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cYearToDateTurnover, vFormattedCurrency:=sFormattedCurrency)

            pnlYearToDateTurnover.Text = sFormattedCurrency

            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cLastYearTurnover, vFormattedCurrency:=sFormattedCurrency)

            pnlLastYearTurnover.Text = sFormattedCurrency
            '2005 End

            ddOccupation.Text = m_sOccupation
            ddEmployment.Text = m_sEmploymentStatusCode
            ddSecondaryOccupation.Text = m_sSecondaryOccupation
            ddSecEmploymentStatus.Text = m_sSecondaryEmploymentStatusCode
            ddMaritalStatus.Text = m_sMaritalStatusCode
            ddGender.Text = m_sGender
            cboAccommodation.Text = m_sAccommodationTypeCode

            ddPaymentMethod.Text = m_sPaymentMethodCode

            If (ddPaymentMethod.Text = CreditCard) Or (ddPaymentMethod.Text = DebitCard) Then
                cboCreditCard.Text = m_sCreditCardCode
                lblCreditCard.Visible = True
                cboCreditCard.Visible = True
            Else
                lblCreditCard.Visible = False
                cboCreditCard.Visible = False
                cboCreditCard.Text = ""
            End If

            chkAgent.CheckState = m_iIsAlsoAgent
            chkProspect.CheckState = m_iIsProspect

            chkSmoker.CheckState = m_iIsSmoker
            chkMailshot.CheckState = m_iMailshot
            chkPets.CheckState = m_iIsPetOwner

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC 25/09/00
            m_sOldIdReference = m_sShortName
            m_sOldResolvedName = m_sPartyTitleCode & " " & m_sInitials & " " & m_sSurName

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSurname, vControlValue:=m_sSurName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtForename, vControlValue:=m_sForeName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInitials, vControlValue:=m_sInitials)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'To avoid getting the Default Date "29/12/1899" or "30/12/1899"
            'when the field is blank
            If m_dtDOB = CDate("00:00:00") Then
                txtDOB.Text = ""
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDOB, vControlValue:=m_dtDOB)
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            pnlAgentName.Text = m_sAgentName

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'save the count in the tag
            txtConsultantRef.Tag = CStr(m_lConsultantCnt)

            PnlConsultantName.Text = m_sConsultantName

            'sj 02/07/2002 - start
            If m_bAONAffinity Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMembershipId, vControlValue:=m_sFileCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFileCode, vControlValue:=m_sFileCode)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'sj 02/07/2002 - end

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCCJ, vControlValue:=m_lCCJs)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ddBusiness.Text = m_sEmployerBusiness
            ddSecondaryBusiness.Text = m_sSecondaryEmployerBusiness

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSalutation, vControlValue:=m_sSalutation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Just set the text normally
                txtSalutation.Text = m_sSalutation
            End If

            txtLoyaltyNumber.Text = m_sLoyaltyNumber.Trim()
            If m_bIsNRMA Then
                If m_sShortName.Trim() = m_sAlternativeIdentifier.Trim() Then
                    txtAlternativeIdentifier.Text = PartyFunc.m_kBlankAlternativeIdentifier
                Else
                    txtAlternativeIdentifier.Text = m_sAlternativeIdentifier.Trim()
                End If
            Else
                txtAlternativeIdentifier.Text = m_sAlternativeIdentifier.Trim()
            End If
            txtTradingName.Text = m_sTradingName.Trim()

            If m_bFutureDateAddressChanges Then

                m_lReturn = m_oBusiness.GetFutureDatedAddresses(r_vFutureDatedAddresses:=m_vFutureDatedAddresses, v_vPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface")
                    Return result
                End If
            End If
            'sj 23/07/2002 - end

            ' +++ JJ 21/07/2003  PN249
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSource, vControlValue:=m_sSource)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            chkTPS.CheckState = m_iTPSind
            ' --- 21/07/2003

            'DD 24/10/2003
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTPPassword, vControlValue:=m_sTPPassword)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            chkeMPS.CheckState = m_iEMPSind

            'FSA Phase III
            If m_dtTobLetter = CDate("00:00:00") Then
                txtTobLetter.Text = ""
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTobLetter, vControlValue:=m_dtTobLetter)
            End If
            'FSA Phase IIIEnd

            chkFeeClient.CheckState = IIf(gPMFunctions.ToSafeLong(m_vIsFeeClient) = 1, CheckState.Checked, CheckState.Unchecked)

            'Fill the contact list view
            PopulateContacts()

            'Fill the address list view
            PopulateAddresses()

            '    Fill the Convictions list view
            PopulateConvictions()

            '    Fill the Lifestyle list view
            PopulateLifestyle()

            'DC 28/06/00
            '   Fill the Correspondence Type Combo Box
            PopulateCorrespondenceTypes()

            '    Fill the PartyLoyaltyScheme list view
            PopulateLoyaltySchemes() 'RAW 18/11/2002 : PS005 : Added

            'Party Bank Details
            LoadPartyBankControl()

            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 157
        Dim ctlFormControl As System.Windows.Forms.Control
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC030400 added check for uctDropdown
            ' Set all of the forms controls to the disable state.
            'developer guide no. 157
            For Each ctlFormControl In Me.Controls
                If (TypeOf ctlFormControl Is System.Windows.Forms.TabControl) Then
                    For Each ctlTabPage As Control In ctlFormControl.Controls
                        If (TypeOf ctlTabPage Is System.Windows.Forms.TabPage) Then
                            For Each ctl As Control In ctlTabPage.Controls
                                If (TypeOf ctl Is GroupBox) Then
                                    For Each ctlChild As Control In ctl.Controls
                                        If (TypeOf ctlChild Is System.Windows.Forms.TextBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.ComboBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.CheckBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is AxThreed.AxSSOption) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is PMListMgrDropdown.uctDropdown) Then
                                            ctlChild.Enabled = Not lDisabled
                                        End If
                                    Next
                                End If
                                'Check the type of the control.
                                If (TypeOf ctl Is System.Windows.Forms.TextBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.ComboBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is System.Windows.Forms.CheckBox) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is AxThreed.AxSSOption) Then
                                    ctl.Enabled = Not lDisabled
                                ElseIf (TypeOf ctl Is PMListMgrDropdown.uctDropdown) Then
                                    ctl.Enabled = Not lDisabled
                                End If
                            Next
                        End If
                    Next

                End If
            Next

            cboCurrency.Enabled = Not lDisabled

            'Now the command buttons...
            cmdAgentLookUp.Enabled = Not lDisabled

            cmdConsultantLookup.Enabled = Not lDisabled
            cmdAddAd.Enabled = Not lDisabled
            cmdAddCon.Enabled = Not lDisabled
            cmdAddConv.Enabled = Not lDisabled
            cmdAddLife.Enabled = Not lDisabled
            cmdCurrentAgent.Enabled = Not lDisabled
            cmdAddPol.Enabled = Not lDisabled
            cmdAddLoyaltyScheme.Enabled = Not lDisabled ' RAW 18/11/2002 : PS005 : Added
            cmdCurrentAgent.Enabled = Not lDisabled
            cmdInsurerLookup.Enabled = Not lDisabled
            cmdBrokerLookup.Enabled = Not lDisabled
            uctPartyTax1.ReadOnly_Renamed = lDisabled

            uctPartyBankControl1.ReadOnly_Renamed = lDisabled
            'ED 19/11/2002 - Added
            'PSS224
            'cmdProspect.Enabled = Not lDisabled

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the form disabling", vApp:=ACApp, vClass:=ACClass, vMethod:="DisableForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)
        Dim iLanguageId As Integer
        'In all instances where the GetResData function is called the g_iLanguageID% is replaced with iLanguageId%
        'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            '    Me.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACInterfaceTitle, _
            'iDataType:=PMResString)

            ' Check for an error.
            '    If (Me.Caption = "") Then
            ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse

            ' Log Error.
            '        LogMessage _
            'iType:=PMLogError, _
            'sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            '"Please check the file exists and the correct captions are available", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="DisplayCaptions"

            '        Exit Function
            '    End If

            '    cmdOK.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACOKButton, _
            'iDataType:=PMResString)

            '    cmdCancel.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACCancelButton, _
            'iDataType:=PMResString)

            '    cmdHelp.Caption = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACHelpButton, _
            'iDataType:=PMResString)

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)
            m_lReturn = gPMFunctions.GetUserIsAmericanLanguageID(iLanguageId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)



            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdAddCon.Text = cmdAddAd.Text
            cmdAddConv.Text = cmdAddAd.Text
            cmdAddLife.Text = cmdAddAd.Text
            cmdAddPol.Text = cmdAddAd.Text
            cmdAddLoyaltyScheme.Text = cmdAddAd.Text ' RAW 18/11/2002 : PS005 : Added


            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdEditCon.Text = cmdEditAd.Text
            cmdEditConv.Text = cmdEditAd.Text
            cmdEditLife.Text = cmdEditAd.Text
            cmdEditPol.Text = cmdEditAd.Text
            cmdEditLoyaltyScheme.Text = cmdEditAd.Text ' RAW 18/11/2002 : PS005 : Added


            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdDeleteCon.Text = cmdDeleteAd.Text
            cmdDeleteConv.Text = cmdDeleteAd.Text
            cmdDeleteLife.Text = cmdDeleteAd.Text
            cmdDeletePol.Text = cmdDeleteAd.Text
            cmdDeleteLoyaltyScheme.Text = cmdDeleteAd.Text ' RAW 18/11/2002 : PS005 : Added


            cmdAgentLookUp.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLookupButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '   DC 03/05/00
            '   Cater for more than one Associate
            '   cmdAssociateLookup.Caption = cmdAgentLookUp.Caption

            cmdConsultantLookup.Text = cmdAgentLookUp.Text

            '2005 Roadmap
            '    cmdProspect.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACProspectButton, _
            ''        iDataType:=PMResString)

            '    Toolbar1.Buttons(ACIButtonFinancial).ToolTipText = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACFinancial, _
            'iDataType:=PMResString)

            '    Toolbar1.Buttons(ACIButtonPolicy).ToolTipText = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACPolicy, _
            'iDataType:=PMResString)

            ' TF031298
            '    Toolbar1.Buttons(ACIButtonClaim).ToolTipText = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACClaim, _
            'iDataType:=PMResString)

            '    Toolbar1.Buttons(ACIButtonNotes).ToolTipText = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACNotes, _
            'iDataType:=PMResString)

            '    Toolbar1.Buttons(ACIButtonLetter).ToolTipText = iPMFunc.GetResData( _
            'iLangID:=g_iLanguageID%, _
            'lID:=ACLetter, _
            'iDataType:=PMResString)

            'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)


            lblYearToDateTurnOver.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACYearToDateTurnOver, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLastYearTurnover.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLastYearTurnover, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.5.1.1)


            lblIDReference.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSurname.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACSurname, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblForename.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACForename, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTitle.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblInitials.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACInitials, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'PN15600
            '    lblIsAgent.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACIsAgent, _
            ''        iDataType:=PMResString)

            'sj 02/07/2002 - start
            If m_bAONAffinity Then

                fraAgent.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAffinity, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else
                'DC101204
                If m_sUnderwritingOrBroking = "U" Then

                    fraAgent.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                Else

                    fraAgent.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACThirdParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                End If
            End If
            'sj 02/07/2002 - end


            lblAgentName.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblConsultantName.Text = lblAgentName.Text

            If m_bAONAffinity Then

                lblArea.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTeam, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                lblArea.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACArea, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If


            lblFileCode.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACFileCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'sj 02/07/2002 - start

            lblMembershipId.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACMembershipId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'sj 02/07/2002 - end


            fraConsultant.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACConsultant, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraContact.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACContacts, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC 28/06/00

            lblPreferredCorrespondence.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPreferredCorrespondence, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraPaymentDetails.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPaymentDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentMethod.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReminderType.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACReminderType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblServicelevel.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACServiceLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCreditCard.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCreditCard, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTermsOfPayment.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTermsOfPayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraEmploymentDetails.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACEmploymentDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblOccupation.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACOccupation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEmployer.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACEmployer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblEmploymentStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblSecOccupation.Text = lblOccupation.Text
            lblSecEmployer.Text = lblEmployer.Text
            lblSecEmploymentStatus.Text = lblEmploymentStatus.Text


            lblCCJ.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCCJ, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraLifestyle.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLifestyle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDOB.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACDOB, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMaritalStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACMaritalStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblGender.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACGender, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblNationality.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACNationality, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'PN15600
            '    lblMailshot.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACMailshot, _
            ''        iDataType:=PMResString)
            '    lblPets.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACPets, _
            ''        iDataType:=PMResString)


            lblSmoker.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACSmoker, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAccommodation.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAccommodation, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraDependants.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACDependants, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 18/11/2002 : PS005 : Added

            fraLoyaltySchemes.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLoyaltySchemes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(TabMainTab, 0, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(TabMainTab, 1, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(TabMainTab, 2, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(TabMainTab, 3, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(TabMainTab, 4, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 18/11/2002 : PS005 : Added

            SSTabHelper.SetTabCaption(TabMainTab, 5, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'eck 2005 Roadmap

            SSTabHelper.SetTabCaption(TabMainTab, 6, iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTabTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            '    tabProspecting.TabCaption(0) = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACProspecting, _
            ''        iDataType:=PMResString)

            If m_sUnderwritingOrBroking = "U" Then


                lblAgentReference.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAgentReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Else


                lblAgentReference.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACThirdPartyRef, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End If


            lblProspectStatus.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACProspectStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_sUnderwritingOrBroking = "U" Then


                cmdCurrentAgent.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCurrentAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Else


                cmdCurrentAgent.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCurrentThirdParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            End If


            fraCampaign.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACCampaigns, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraPolicies.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACPolicies, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC 20/06/00
            'New label
            'PN15300
            '    lblIsProspect.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACIsProspect, _
            ''        iDataType:=PMResString)


            sAddressUsage = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddressListUsage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine1 = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddressListLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine2 = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddressListLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine3 = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddressListLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine4 = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddressListLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sPostCode = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAddressListPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'sj 18/06/2002 - start

            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTradingName.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACTradingName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSubBranch.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACSubBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAlternativeIdentifier.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACAlternativeIdentifier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLoyaltyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=ACLoyaltyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'sj 18/06/2002 - end


            chkFeeClient.Text = CStr(iPMFunc.GetResData(iLangID:=iLanguageId, lId:=AC_CAPTION_ISFEECLIENT, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    lvwAddresses.Columns.Item(0).Text = sPostCode
                    lvwAddresses.Columns.Item(1).Text = sAddressUsage
                    lvwAddresses.Columns.Item(2).Text = sAddressLine1
                    lvwAddresses.Columns.Item(3).Text = sAddressLine2
                    lvwAddresses.Columns.Item(4).Text = sAddressLine3
                    lvwAddresses.Columns.Item(5).Text = sAddressLine4

                Case Else
                    lvwAddresses.Columns.Item(0).Text = sAddressUsage
                    lvwAddresses.Columns.Item(1).Text = sAddressLine1
                    lvwAddresses.Columns.Item(2).Text = sAddressLine2
                    lvwAddresses.Columns.Item(3).Text = sAddressLine3
                    lvwAddresses.Columns.Item(4).Text = sAddressLine4
                    lvwAddresses.Columns.Item(5).Text = sPostCode

            End Select
            'sj 12/06/2002 - start
            If m_bIsNRMA Then
                m_lReturn = PartyFunc.SetAddressHeaders(r_oAddresses:=lvwAddresses, v_sPostCode:=sPostCode, v_sAddressUsage:=sAddressUsage)
            End If
            'sj 12/06/2002 - end

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
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Private Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.
            m_lReturn = GetLookupValues()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupArea, ctlLookup:=cboArea)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupReminderType, ctlLookup:=cboReminderType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupServiceLevel, ctlLookup:=cboServiceLevel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupNationality, ctlLookup:=cboNationality)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupProspectStatus, ctlLookup:=cboProspectingStatus)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRiskGroup, ctlLookup:=cboPolicyType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSeasonalGift, ctlLookup:=cboSeasonalGift)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalStopCode, ctlLookup:=cboRenewalStopCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupStrengthCode, ctlLookup:=cboStrengthCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPFFrequency, ctlLookup:=cboTermsOfPayment)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            SelectcboItem(cboTermsOfPayment, CInt(m_iPaymentTermId))

            'blacklist reason id
            If m_bSystemOptionClientBlacklistingInForce Then

                m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupBlackListReason, ctlLookup:=cboBlackListReason, sInitialOption:="(not blacklisted)")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                SelectcboItem(cboBlackListReason, CInt(m_vBlackListReasonId))
            End If

            m_lReturn = GetBranchDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=cboBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_lSubBranchId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bValidateAlternativeIdentifier Then

                'Get number validation scripts
                m_lReturn = m_oBusiness.GetNumberValidationScripts(v_sBranchPrefix:=m_sBranchPrefix, r_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, r_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetNumberValidationScripts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Get relationship type lookups
            m_lReturn = m_oBusiness.GetRelationshipTypeLookups(vRelationships:=m_vRelationships)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the relationship type lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
            End If


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

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Get the details from the business object.

            ' {* USER DEFINED CODE (Begin) *}
            'Are we from events?

            m_oBusiness.FromEvent = FromEvent


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
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '
    ' ***************************************************************** '
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control, Optional ByRef bSecondary As Boolean = False, Optional ByRef sInitialOption As String = "") As Integer

        Dim result As Integer = 0
        Dim lRow, lRow2 As Integer
        Dim lCntr As Integer
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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")
                Return result
            End If

            Dim ctl1 As ComboBox

            If Not ctlLookup Is Nothing Then
                ctl1 = DirectCast(ctlLookup, ComboBox)
            Else
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ctl1.Items.Clear()

            If sInitialOption <> "" Then

                Dim newIndex As Integer = ctl1.Items.Add(New VB6.ListBoxItem(sInitialOption, 0))
                'ctlLookup.ItemData(ReflectionHelper.GetMember(ctlLookup, "NewIndex")) = 0
                ' NB:m_vLookupValues(ACValueID, lRow2&)) is the value from the party record
                ' if the value from the party matches the item

                If (m_vLookupValues(ACValueID, lRow)) = 0 Then
                    ' then select it.
                    ctl1.SelectedIndex = newIndex
                End If

            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            ' First entry should be blank for changing option in future

            ctl1.Items.Add("")
            For lCntr = CInt(m_vLookupValues(ACValueStartPos, lRow)) To (CInt(m_vLookupValues(ACValueStartPos, lRow)) + CInt(m_vLookupValues(ACValueNumber, lRow))) - 1
                ' Add the details to the control.
                Dim index As Integer = ctl1.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CLng(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If bSecondary Then
                    lRow2 = lRow + 1
                Else
                    lRow2 = lRow
                End If

                If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then

                    If gPMFunctions.ToSafeInteger((m_vLookupValues(ACValueID, lRow2))) = gPMFunctions.ToSafeInteger((m_vLookupDetails(ACDetailKey, lCntr))) Then
                        ctl1.SelectedIndex = index
                    End If

                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then
                ctl1.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd

                    ' Get all of the lookup values.
                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAll, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                Case gPMConstants.PMEComponentAction.PMEdit

                    ' Get all of the lookup values with the correct effective date.
                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails)

                    'DC030401 as per PMEdit - uses PMLookupAllEffective not PMLookupAll
                Case gPMConstants.PMEComponentAction.PMView

                    ' Get lookup values for viewing only.
                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageId:=g_iLanguageID, vResultArray:=m_vLookupDetails)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the lookup values from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                Return result
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup values", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try

    End Function
    'eck010900
    ' ***************************************************************** '
    ' Name: GetBranchDetails
    '
    ' Description: Gets all of the branch details
    '
    ' ***************************************************************** '
    Private Function GetBranchDetails() As Integer
        Dim result As Integer = 0
        Dim vSourceArray(,) As Object
        Dim i As Short
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        'If this is a multi-company system.
        If m_bMultiTreeAccounting Then
            'Always show correct branch even if the user is not authorise for that branch.

            m_lReturn = m_oPMUser.GetAllSources(r_vSourceArray:=vSourceArray)
        Else
            'Only populate combo with addresses the user is authorised to access.

            m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserId, v_bIncludeDeletedSources:=m_bIncludeClosedBranchChecked)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get branch details for the dropdown list")
        End If


        m_vSourceArray = vSourceArray
        'Clear combo.
        cboBranch.Items.Clear()

        'Populate branch combo
        'developer guide no. 162
        For i = vSourceArray.GetLowerBound(1) To vSourceArray.GetUpperBound(1)
            'Add using branch description (3).
            'developer guide no. 162
            Dim listIndex As Integer = cboBranch.Items.Add(New VB6.ListBoxItem(Trim(vSourceArray(2, i)), CInt(vSourceArray(0, i))))


            'developer guide no. 153,162
            If CShort(vSourceArray(0, i)) = m_iPartySourceId Then

                'cboBranch.SelectedIndex = CInt(ReflectionHelper.GetMember(cboBranch, "NewIndex"))
                cboBranch.SelectedIndex = listIndex

                'developer guide no. 162
                m_sBranchPrefix = Trim(vSourceArray(1, i))

            End If
        Next i

        'If this is a multi-company system.
        If m_bMultiTreeAccounting Then
            'Re-populate source array with branches that the user is authorised this.

            m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to get users branch details")
            End If
        End If
        'developer guide no. 131
        If cboBranch.SelectedIndex >= 0 Then
            cboCurrency.CompanyId = CInt(VB6.GetItemData(cboBranch, cboBranch.SelectedIndex))
        End If
        cboCurrency.RefreshList()

        'PN16993
        'cboCurrency.CurrencyID = m_iCurrencyId
        ' if this is add mode then default the currency id
        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            ' get the default currency id
            ' based on the selected parties branch id
            ' NB: This function also sets the currency id
            lReturn = GetSourceBaseCurrency()

            ' if the getsourcebasecurrency failed use
            ' the currency id defined on the object manager
            ' as a fall back
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cboCurrency.CurrencyId = g_iCurrencyId
            End If

        Else
            ' otherwise use the currency id that was previously saved
            cboCurrency.CurrencyId = m_iCurrencyId
        End If

        m_bUserMode = True

        Return result



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the branch details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Private Function GetOldAndNewAddress() As Integer

        Dim result As Integer = 0
        Dim lAddressCount As Integer
        Dim sAddressUsageDesc As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lAddressCount = 0

            For lLoopAddresses As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                sAddressUsageDesc = ""

                For lLoopAddresseTypes As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If m_vAddresses(1, lLoopAddresses).Equals(m_vAddressTypes(0, lLoopAddresseTypes)) Then
                        sAddressUsageDesc = CStr(m_vAddressTypes(1, lLoopAddresseTypes)).Trim()
                        Exit For
                    End If
                Next

                If sAddressUsageDesc <> "" Then
                    If lAddressCount = 0 Then
                        ReDim m_vOldAddress(6, lAddressCount)
                    Else
                        ReDim Preserve m_vOldAddress(6, lAddressCount)
                    End If

                    m_vOldAddress(0, lAddressCount) = CStr(m_vAddresses(6, lLoopAddresses))
                    m_vOldAddress(1, lAddressCount) = sAddressUsageDesc
                    m_vOldAddress(2, lAddressCount) = CStr(m_vAddresses(2, lLoopAddresses)).Trim()
                    m_vOldAddress(3, lAddressCount) = CStr(m_vAddresses(3, lLoopAddresses)).Trim()
                    m_vOldAddress(4, lAddressCount) = CStr(m_vAddresses(4, lLoopAddresses)).Trim()
                    m_vOldAddress(5, lAddressCount) = CStr(m_vAddresses(5, lLoopAddresses)).Trim()
                    m_vOldAddress(6, lAddressCount) = CStr(m_vAddresses(0, lLoopAddresses)).Trim()

                    lAddressCount += 1
                End If

            Next

            lAddressCount = 0

            For Each oListItem As ListViewItem In lvwAddresses.Items

                If lAddressCount = 0 Then
                    ReDim m_vNewAddress(6, lAddressCount)
                Else
                    ReDim Preserve m_vNewAddress(6, lAddressCount)
                End If

                'Check default country to see where Postcode is being displayed.
                Select Case m_sDefaultCountryCode.Trim()
                    Case "GBR"
                        sAddressUsageDesc = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
                    Case Else
                        sAddressUsageDesc = oListItem.Text.Trim()
                End Select

                Select Case m_sDefaultCountryCode.Trim()
                    Case "GBR"
                        m_vNewAddress(0, lAddressCount) = Convert.ToString(oListItem.Tag)
                        m_vNewAddress(1, lAddressCount) = sAddressUsageDesc
                        m_vNewAddress(2, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text.Trim()
                        m_vNewAddress(3, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text.Trim()
                        m_vNewAddress(4, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 4).Text.Trim()
                        m_vNewAddress(5, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 5).Text.Trim()
                        m_vNewAddress(6, lAddressCount) = oListItem.Text.Trim()
                    Case Else
                        m_vNewAddress(0, lAddressCount) = Convert.ToString(oListItem.Tag)
                        m_vNewAddress(1, lAddressCount) = sAddressUsageDesc
                        m_vNewAddress(2, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
                        m_vNewAddress(3, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text.Trim()
                        m_vNewAddress(4, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text.Trim()
                        m_vNewAddress(5, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 4).Text.Trim()
                        m_vNewAddress(6, lAddressCount) = ListViewHelper.GetListViewSubItem(oListItem, 5).Text.Trim()
                End Select

                lAddressCount += 1

            Next oListItem

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOldAndNewAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOldAndNewAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'developer guide no. 101
    Private Sub GetPersonLookups(ByRef vCategoryId As Object, ByRef vCategoryDescription As String)

        Select Case vCategoryId
            Case 1
                vCategoryDescription = "Insured"
            Case 2
                vCategoryDescription = "Spouse"
            Case 3
                vCategoryDescription = "1st Child"
            Case 4
                vCategoryDescription = "2nd Child"
            Case 5
                vCategoryDescription = "3rd Child"
            Case 6
                vCategoryDescription = "4th Child"
            Case 7
                vCategoryDescription = "5th Child"
            Case 8
                vCategoryDescription = "6th Child"
            Case 9
                vCategoryDescription = "Other Child"
            Case 10
                vCategoryDescription = "Partner"
            Case Else
                vCategoryDescription = "Undefined Relationship"
        End Select
    End Sub

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

            '***** BEWARE THIS COMMAND MAY SELF-CORRUPT IF MODIFIED
            'IF THIS HAPPENS RESTORE TO COMMENTED VERSION
            ' Check the task.
            '    Select Case (m_iTask)
            '        Case PMAdd
            '            m_lReturn& = m_oBusiness.EditAdd(lRow:=lBusinessDataID&, _
            ''                                    vPartyTitleCode:=m_sPartyTitleCode, _
            ''                                    vForename:=m_sForeName$, vInitials:=m_sInitials$, _
            ''                                    vEmploymentStatusCode:=m_sEmploymentStatusCode, _
            ''                                    vEmployerBusiness:=m_lEmployerBusiness, _
            ''                                    vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, _
            ''                                    vSecondaryEmployerBusiness:=m_lSecondaryEmployerBusiness, _
            ''                                    vMaritalStatusCode:=m_sMaritalStatusCode, _
            ''                                    vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_lNationalityId, _
            ''                                    vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, _
            ''                                    vAccommodationTypeCode:=m_sAccommodationTypeCode, _
            ''                                    vShortName:=m_sShortName$, vName:=m_sSurName$, vResolved:=m_sResolved, _
            ''                                    vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, _
            ''                                    vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, _
            ''                                    vFileCode:=m_sFileCode, vCurrencyId:=m_iCurrencyId, _
            ''                                    vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, _
            ''                                    vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, _
            ''                                    vPaymentTermCode:=m_sPaymentTermCode, vCCJs:=m_lCCJs, vPartyLifestyleId:=m_lPartyLifestyleId, vPartyLifeStyleName:=m_sPartyLifeStyleName, _
            ''                                    vCategory:=m_lCategory, vDateOfBirth:=m_dtDOB, vGender:=m_sGender, _
            ''                                    vOccupationCode:=m_sOccupatim_iIsSmoker)
            '        Case PMEdit
            '            m_lReturn& = m_oBusiness.EditUpdate(lRow:=lBusinessDataID&, _
            ''                                    vPartyTitleCode:=m_sPartyTitleCode, _
            ''                                    vForename:=m_sForeName$, vInitials:=m_sInitials$, _
            ''                                    vEmploymentStatusCode:=m_sEmploymentStatusCode, _
            ''                                    vEmployerBusiness:=m_lEmployerBusiness, _
            ''                                    vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, _
            ''                                    vSecondaryEmployerBusiness:=m_lSecondaryEmployerBusiness, _
            ''                                    vMaritalStatusCode:=m_sMaritalStatusCode, _
            ''                                    vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_lNationalityId, _
            ''                                    vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, _
            ''                                    vAccommodationTypeCode:=m_sAccommodationTypeCode, _
            ''                                    vShortName:=m_sShortName$, vName:=m_sSurName$, vResolved:=m_sResolved, _
            ''                                    vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, _
            ''                                    vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, _
            ''                                    vFileCode:=m_sFileCode, vCurrencyId:=m_iCurrencyId, _
            ''                                    vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, _
            ''                                    vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, _
            ''                                    vPaymentTermCode:=m_sPaymentTermCode, vCCJs:=m_lCCJs, vPartyLifestyleId:=m_lPartyLifestyleId, vPartyLifeStyleName:=m_sPartyLifeStyleName, _
            ''                                    vCategory:=m_lCategory, vDateOfBirth:=m_dtDOB, vGender:=m_sGender, _
            ''                                    vOccupationCode:=m_sOccupation, vSecondaryOccupationCode:=m_sSecondaryOccupation, vIsSmoker:=m_iIsSmoker)
            '    End Select
            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    'FSA Phase III TobLetter

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyTitleCode:=m_sPartyTitleCode, vForename:=m_sForeName, vInitials:=m_sInitials, vEmploymentStatusCode:=m_sEmploymentStatusCode, vEmployerBusiness:=m_sEmployerBusiness, vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, vSecondaryEmployerBusiness:=m_sSecondaryEmployerBusiness, vMaritalStatusCode:=m_sMaritalStatusCode, vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_vNationalityId, vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, vAccommodationTypeCode:=m_sAccommodationTypeCode, vShortName:=m_sShortName, vName:=m_sSurName, vResolved:=m_sResolved, vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId, vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs, vPartyLifestyleID:=m_lPartyLifeStyleId, vPartyLifeStyleName:=m_sPartyLifeStyleName, vCategory:=m_lCategory, vDateOfBirth:=m_dtDOB, vGender:=m_sGender, vOccupationCode:=m_sOccupation, vSecondaryOccupationCode:=m_sSecondaryOccupation, vIsSmoker:=m_iIsSmoker, vSourceID:=m_iNewPartySourceId, vSeasonalGiftID:=m_vSeasonalGiftId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId, vSwiftPartyID:=m_lSwiftPartyID, vSalutation:=m_sSalutation, vLoyaltyNumber:=m_sLoyaltyNumber, vAlternativeIdentifier:=m_sAlternativeIdentifier, vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter, vSource:=m_sSource, vTPSind:=m_iTPSind, vEMPSInd:=m_iEMPSind, vTPPassword:=m_sTPPassword, vIsFeeClient:=m_vIsFeeClient)

                Case gPMConstants.PMEComponentAction.PMEdit
                    'FSA Phase III TobLetter

                    m_lReturn = m_oBusiness.Editupdate(lRow:=lBusinessDataID, vPartyTitleCode:=m_sPartyTitleCode, vForename:=m_sForeName, vInitials:=m_sInitials, vEmploymentStatusCode:=m_sEmploymentStatusCode, vEmployerBusiness:=m_sEmployerBusiness, vSecondaryEmploymentStatusC:=m_sSecondaryEmploymentStatusCode, vSecondaryEmployerBusiness:=m_sSecondaryEmployerBusiness, vMaritalStatusCode:=m_sMaritalStatusCode, vNumberOfChildren:=m_lNumberOFChildren, vNationalityId:=m_vNationalityId, vMailshot:=m_iMailshot, vIsPetOwner:=m_iIsPetOwner, vAccommodationTypeCode:=m_sAccommodationTypeCode, vShortName:=m_sShortName, vName:=m_sSurName, vResolved:=m_sResolved, vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt, vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId, vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId, vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs, vPartyLifestyleID:=m_lPartyLifeStyleId, vPartyLifeStyleName:=m_sPartyLifeStyleName, vCategory:=m_lCategory, vDateOfBirth:=m_dtDOB, vGender:=m_sGender, vOccupationCode:=m_sOccupation, vSecondaryOccupationCode:=m_sSecondaryOccupation, vIsSmoker:=m_iIsSmoker, vSourceID:=m_iNewPartySourceId, vPartyID:=m_lNewPartyId, vSeasonalGiftID:=m_vSeasonalGiftId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId, vSwiftPartyID:=m_lSwiftPartyID, vSalutation:=m_sSalutation, vLoyaltyNumber:=m_sLoyaltyNumber, vAlternativeIdentifier:=m_sAlternativeIdentifier, vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter, vSource:=m_sSource, vTPSind:=m_iTPSind, vEMPSInd:=m_iEMPSind, vTPPassword:=m_sTPPassword, vIsFeeClient:=m_vIsFeeClient)
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
    ' Name: InterfaceToData
    '
    '
    '
    ' ***************************************************************** '

    ''' <summary>
    '''  Description: Updates the data storage from the interface details.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InterfaceToData() As Integer

        Dim nResult As Integer = 0
        Dim sMsg As String = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            m_sPartyTitleCode = ddTitle.Text
            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))
            m_sSurName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtSurname))
            m_sForeName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtForename))
            m_sInitials = CStr(m_oFormFields.UnformatControl(ctlControl:=txtInitials))


            If m_bSystemOptionEnhancedResolvedName Then
                m_sResolved = m_sPartyTitleCode & " " & m_sForeName & " " & m_sSurName
            Else
                m_sResolved = m_sPartyTitleCode & " " & m_sInitials & " " & m_sSurName
            End If

            If CurrentResolvedName <> "" Then
                If CurrentResolvedName <> m_sResolved Then
                    If m_sOldResolvedName <> m_sResolved AndAlso m_bSystemOptionUpdateExistingClients Then
                        If MessageBox.Show("The client name has been updated do you want to update the policyholder field on all of the policy records?", "Client Name", MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
                            m_sResolved = m_sResolved.Trim()
                        Else
                            m_sResolved = CurrentResolvedName.Trim()
                        End If
                    Else
                        m_sResolved = CurrentResolvedName.Trim()
                    End If
                End If
            End If

            m_dtDOB = CDate(m_oFormFields.UnformatControl(ctlControl:=txtDOB))
            If m_bAONAffinity Then

                m_sFileCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtMembershipId))
            Else

                m_sFileCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtFileCode))
            End If
            'From ABI Combos
            m_sOccupation = ddOccupation.Text
            m_sSecondaryOccupation = ddSecondaryOccupation.Text
            m_sEmploymentStatusCode = ddEmployment.Text
            m_sSecondaryEmploymentStatusCode = ddSecEmploymentStatus.Text
            m_sPartyTitleCode = ddTitle.Text
            m_sGender = ddGender.Text
            m_sAccommodationTypeCode = cboAccommodation.Text
            m_sMaritalStatusCode = ddMaritalStatus.Text
            If cboTermsOfPayment.SelectedIndex <> -1 Then
                m_iPaymentTermId = CInt(Conversion.Val(CStr(VB6.GetItemData(cboTermsOfPayment, cboTermsOfPayment.SelectedIndex))))
            End If
            m_sPaymentMethodCode = ddPaymentMethod.Text
            m_sCreditCardCode = cboCreditCard.Text
            m_sEmployerBusiness = ddBusiness.Text
            m_sSecondaryEmployerBusiness = ddSecondaryBusiness.Text

            m_sSource = txtSource.Text

            If chkTPS.CheckState = CheckState.Unchecked Then
                m_iTPSind = CheckState.Unchecked
            Else
                m_iTPSind = CheckState.Checked
            End If

            m_sTPPassword = txtTPPassword.Text
            If chkeMPS.CheckState = CheckState.Unchecked Then
                m_iEMPSind = CheckState.Unchecked
            Else
                m_iEMPSind = CheckState.Checked
            End If


            m_lCCJs = CInt(m_oFormFields.UnformatControl(ctlControl:=txtCCJ))

            'From Lookup Combos

            m_sSalutation = CStr(m_oFormFields.UnformatControl(ctlControl:=txtSalutation))

            m_sLoyaltyNumber = txtLoyaltyNumber.Text.Trim()
            If m_bIsNRMA Then
                If txtAlternativeIdentifier.Text.Trim() = PartyFunc.m_kBlankAlternativeIdentifier Then
                    m_sAlternativeIdentifier = m_sShortName.Trim()
                Else
                    m_sAlternativeIdentifier = txtAlternativeIdentifier.Text.Trim()
                End If
            Else
                m_sAlternativeIdentifier = txtAlternativeIdentifier.Text.Trim()
            End If

            m_sTradingName = txtTradingName.Text.Trim()
            'Make sure we have a sub branch selected
            If cboSubBranch.SelectedIndex >= 0 Then
                m_lSubBranchId = CInt(Conversion.Val(CStr(VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex))))
            Else
                m_lSubBranchId = 0
            End If

            If cboCorrespondenceType.SelectedIndex <> -1 Then
                m_vCorrespondenceTypeId = VB6.GetItemData(cboCorrespondenceType, cboCorrespondenceType.SelectedIndex)
            Else
                m_vCorrespondenceTypeId = Nothing
            End If

            If cboRenewalStopCode.SelectedIndex <> -1 Then
                m_vRenewalStopCodeId = VB6.GetItemData(cboRenewalStopCode, cboRenewalStopCode.SelectedIndex)
            Else
                m_vRenewalStopCodeId = 0
            End If

            ' CTAF - 180100 - Check that an area has been selected
            If cboArea.SelectedIndex <> -1 Then
                m_iAreaId = VB6.GetItemData(cboArea, cboArea.SelectedIndex)
            Else
                m_iAreaId = 0
            End If

            m_iCurrencyId = cboCurrency.CurrencyId

            If cboReminderType.SelectedIndex <> -1 Then
                m_lReminderTypeID = VB6.GetItemData(cboReminderType, cboReminderType.SelectedIndex)
            Else
                m_lReminderTypeID = 0
            End If
            If cboServiceLevel.SelectedIndex <> -1 Then
                m_lServiceLevelId = VB6.GetItemData(cboServiceLevel, cboServiceLevel.SelectedIndex)
            Else
                m_lServiceLevelId = 0
            End If

            'DC 03/08/00 Added in to set Natioanlity Id
            'DC 28/09/00 was m_lNationalityId
            If cboNationality.SelectedIndex <> -1 Then
                m_vNationalityId = CStr(VB6.GetItemData(cboNationality, cboNationality.SelectedIndex))
                If gPMFunctions.ToSafeLong(m_vNationalityId) = 0 Then

                    m_vNationalityId = Nothing
                End If
            Else

                m_vNationalityId = Nothing
            End If

            'From Check Boxes
            If chkAgent.CheckState = CheckState.Unchecked Then
                m_iIsAlsoAgent = CheckState.Unchecked
            Else
                m_iIsAlsoAgent = CheckState.Checked
            End If

            If chkProspect.CheckState = CheckState.Unchecked Then
                m_iIsProspect = CheckState.Unchecked
            Else
                m_iIsProspect = CheckState.Checked
            End If

            If chkPets.CheckState = CheckState.Unchecked Then
                m_iIsPetOwner = CheckState.Unchecked
            Else
                m_iIsPetOwner = CheckState.Checked
            End If

            If chkMailshot.CheckState = CheckState.Unchecked Then
                m_iMailshot = CheckState.Unchecked
            Else
                m_iMailshot = CheckState.Checked
            End If

            If chkSmoker.CheckState = CheckState.Unchecked Then
                m_iIsSmoker = CheckState.Unchecked
            Else
                m_iIsSmoker = CheckState.Checked
            End If

            m_sMaritalStatusCode = ddMaritalStatus.Text

            m_sPartyLifeStyleName = txtForename.Text & " " & txtSurname.Text

            m_lPartyLifeStyleId = Convert.ToString(lvwLifestyle.Items.Item(0).Tag)
            m_lCategory = 1

            'Validation of following not required if we are cancelling out

            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                'Get party count for Agent (if valid ref supplied)
                If (txtAgentRef.Text.Trim() <> "") And (m_bVerifyAgentCnt) Then

                    m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtAgentRef.Text.Trim(), vPartyCnt:=m_lAgentCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lAgentCnt = 0 Then
                        m_bErrorMessageAlreadyShown = True
                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        MessageBox.Show(sMsg, "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        iPMFunc.SelectText(txtAgentRef)
                        txtAgentRef.Focus()

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    If txtAgentRef.Text.Trim() = "" Then
                        m_lAgentCnt = 0
                    Else
                        m_lAgentCnt = CInt(Convert.ToString(txtAgentRef.Tag))
                    End If
                End If

                'Get party count for Consultant (if valid ref supplied)
                If (txtConsultantRef.Text.Trim() <> "") And (m_bVerifyConsultantCnt) Then

                    m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtConsultantRef.Text.Trim(), vPartyCnt:=m_lConsultantCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lConsultantCnt = 0 Then

                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConsultantMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMsg, "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        iPMFunc.SelectText(txtConsultantRef)
                        txtConsultantRef.Focus()

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    If txtConsultantRef.Text.Trim() = "" Then
                        m_lConsultantCnt = 0
                    Else
                        m_lConsultantCnt = CInt(Convert.ToString(txtConsultantRef.Tag))
                    End If
                End If

            End If

            If cboSeasonalGift.SelectedIndex <> -1 Then
                m_vSeasonalGiftId = VB6.GetItemData(cboSeasonalGift, cboSeasonalGift.SelectedIndex)
            Else

                m_vSeasonalGiftId = Nothing
            End If


            m_dtTobLetter = CDate(m_oFormFields.UnformatControl(ctlControl:=txtTobLetter))


            'DN 01/06/01 - Change PartyIDs to Longs
            If cboBranch.SelectedIndex <> -1 Then
                If m_iPartySourceId <> VB6.GetItemData(cboBranch, cboBranch.SelectedIndex) Then
                    ' Don't show message box for new client
                    If m_iPartySourceId > 0 Then
                        MessageBox.Show("You are about to change the client branch remember to change any relevant policies", Application.ProductName)
                    End If
                    m_lNewPartyId = 0
                Else
                    m_lNewPartyId = m_lPartyId
                End If
                m_iNewPartySourceId = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)
            Else
                'MKW180703 PN5425 Default to original party source id (if available)
                If m_iPartySourceId > 0 Then
                    m_iNewPartySourceId = m_iPartySourceId
                Else
                    m_iNewPartySourceId = g_iSourceID
                End If

            End If

            ' get the party tax details from the party tax control
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage


            If m_bSystemOptionClientBlacklistingInForce Then
                If cboBlackListReason.SelectedIndex <> -1 Then
                    If cboBlackListReason.SelectedIndex = 0 Then

                        m_vBlackListReasonId = Nothing
                    Else
                        m_vBlackListReasonId = VB6.GetItemData(cboBlackListReason, cboBlackListReason.SelectedIndex)
                    End If
                End If
            Else
                m_vBlackListReasonId = Nothing
            End If

            If cboTermsOfPayment.SelectedIndex <> -1 Then
                If cboTermsOfPayment.SelectedIndex = 0 Then
                    m_iPaymentTermId = 0
                Else
                    m_iPaymentTermId = VB6.GetItemData(cboTermsOfPayment, cboTermsOfPayment.SelectedIndex)
                End If
            End If

            m_vIsFeeClient = Nothing



            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: LockParty
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function LockParty() As Integer

        Dim result As Integer = 0
        Dim sLockedBy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oPMLock.LockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)


            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    'OK

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    If sLockedBy = "ERROR" Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    Else
                        result = gPMConstants.PMEReturnCode.PMFalse
                        MessageBox.Show("Party currently locked by " & sLockedBy &
                                        Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Party Lock")
                        Return result
                    End If


                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

            End Select

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: OKClick
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function OKClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'sj 12/06/2002 - start
            Dim sMessage As String = ""
            'sj 12/06/2002 - end

            If txtCCJ.Text.Trim() <> "" Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtCCJ.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    MessageBox.Show("County court judgements must be a number", "Numeric Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'developer guide no. 222
                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                    SSTabHelper.SetSelectedIndex(TabMainTab, 3)
                    If txtCCJ.Visible Then
                        txtCCJ.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            'MSS191001 - Added check here. It appears that we cannot set Title as a mandatory
            'control in the usual way. This is probably not the ideal solution but it will
            'work
            If ddTitle.Text.Trim().Length = 0 Then
                MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Title", MessageBoxButtons.OK, MessageBoxIcon.Error)
                'sj 18/06/2002 - start
                'developer guide no. 222
                Dim selIndex As Integer = 0
                selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                If ddTitle.Visible Then
                    ddTitle.Focus()
                Else
                    SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                End If
                result = gPMConstants.PMEReturnCode.PMFalse
                'sj 18/06/2002 - end
                Return result
            End If

            '2005 Roadmap Always save propsect information
            '    If (tabProspecting.Visible = True) Then
            '        If (m_bChangedProspect = True) Then
            '            m_lReturn = SaveProspect()
            '
            '            If (m_lReturn <> PMTrue) Then
            '                ' Log Error Message
            '                LogMessage _
            ''                    iType:=PMLogOnError, _
            ''                    sMsg:="Failed to SaveProspect.", _
            ''                    vApp:=ACApp, _
            ''                    vClass:=ACClass, _
            ''                    vMethod:="OKClick", _
            ''                    vErrNo:=Err.Number, _
            ''                    vErrDesc:=Err.Description
            '
            '                OKClick = m_lReturn
            '                Exit Function
            '            End If
            '        End If
            '
            '        If (m_bChangedProspectPolicies = True) Then
            '            m_lReturn = SaveProspectPolicies
            '
            '            If (m_lReturn <> PMTrue) Then
            '                ' Log Error Message
            '                LogMessage _
            ''                    iType:=PMLogOnError, _
            ''                    sMsg:="Failed to SaveProspectPolicies", _
            ''                    vApp:=ACApp, _
            ''                    vClass:=ACClass, _
            ''                    vMethod:="OKClick", _
            ''                    vErrNo:=Err.Number, _
            ''                    vErrDesc:=Err.Description
            '                OKClick = m_lReturn
            '                Exit Function
            '            End If
            '        End If
            '
            '        tabProspecting.Visible = False
            '        TabMainTab.Top = 0
            '        TabMainTab.Visible = True
            '        'This stops it closing the form
            '        OKClick = PMFalse
            '        'MKR 29/09/2004 PN 14560/14563 Added to stop attempt to try and save Party Prospect
            '        'twice with same Primary Key value causing a DB error. -- Start
            '        m_bChangedProspect = False
            '        'End
            '        Exit Function
            '    End If

            If TabMainTab.Visible Then

                m_lReturn = SaveParty()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                End If

                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC030401 to allow editting of party
    ' ***************************************************************** '
    ' Name: EditClick
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function EditClick() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = LockParty()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Party Bank Details
    Public Function LoadPartyBankControl() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "LoadPartyBankControl"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 9
        m_lReturn = uctPartyBankControl1.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        uctPartyBankControl1.PartyCnt = m_lPartyCnt

        'developer guide no. 108
        m_lReturn = uctPartyBankControl1.Load_Renamed
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "uctPartyBankControl1.Load Failed", gPMConstants.PMELogLevel.PMLogError)
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateCorrespondenceTypes
    '
    ' Description: Populate Correspondence Types
    '
    ' ***************************************************************** '
    Private Function PopulateCorrespondenceTypes() As Integer

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Correspondence List
            cboCorrespondenceType.Items.Clear()
            cboCorrespondenceType.SelectedIndex = -1

            ' Check if there is a list
            If Not Information.IsArray(m_vCorrespondenceTypes) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vCorrespondenceTypes.GetLowerBound(1) To m_vCorrespondenceTypes.GetUpperBound(1)

                Dim cboCorrespondenceType_NewIndex As Integer = -1
                cboCorrespondenceType_NewIndex = cboCorrespondenceType.Items.Add(CStr(m_vCorrespondenceTypes(1, lRow)))
                VB6.SetItemData(cboCorrespondenceType, cboCorrespondenceType_NewIndex, CInt(m_vCorrespondenceTypes(0, lRow)))

                If CDbl(m_vCorrespondenceTypes(0, lRow)) = m_vCorrespondenceTypeId Then
                    cboCorrespondenceType.SelectedIndex = cboCorrespondenceType_NewIndex
                    cboCorrespondenceType.Text = CStr(m_vCorrespondenceTypes(1, lRow))
                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Corresondence Types", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCorrespondenceTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'RWH(11/07/2000) Altered to move PostCode from far left to far
        'right of ListView.
        Dim k As Integer

        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""


        Try

            'Just go if no addresses
            If Not Information.IsArray(m_vAddresses) Then
                Exit Sub
            End If

            lvwAddresses.Items.Clear()
            m_lAddressCount = 0

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                '        Set oListItem = lvwAddresses.ListItems.Add(, , _
                ''            Trim$(m_vAddresses(0, i)), , AddressImage)

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
                End If

                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Assign the details to the first column.
                        ' Postcode

                        oListItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim(), "AddressImage")
                        'oListItem.ImageKey = "AddressImage"
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

                    Case Else
                        ' Assign the details to the first column.
                        ' Address Usage

                        oListItem = lvwAddresses.Items.Add(sAddressUsage, "AddressImage")

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

        'Const ContactImage As String = "ContactImage"      ''Unused Local Variables
        Dim oListItem As ListViewItem


        Try

            lvwContacts.Items.Clear()
            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If



            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1

                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim(), "ContactImage")

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
            '    'Populate the cells

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateConvictions
    '
    ' Description: Fills the grid control with contact details
    '
    ' ***************************************************************** '
    Private Sub PopulateConvictions()

        ''Const ConvictionImage As String = "ConvictionImage"       ''Unused Local Variables
        Dim oListItem As ListViewItem

        Try

            lvwConvictions.Items.Clear()
            If Not Information.IsArray(m_vConvictions) Then
                Exit Sub
            End If



            'EK 12/12/99 Correct column/data matching

            ' Assign the details to the interface.
            For i As Integer = m_vConvictions.GetLowerBound(1) To m_vConvictions.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1

                oListItem = lvwConvictions.Items.Add(CStr(m_vConvictions(2, i)).Trim(), "ConvictionImage")

                ' Assign details to other the columns
                ' Date

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=CStr(m_vConvictions(3, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDate.Text.Trim()

                ' Description
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vConvictions(4, i)).Trim()

                ' Fine
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=CStr(m_vConvictions(5, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtCurrency.Text.Trim()

                ' Conviction Status
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vConvictions(11, i)).Trim()

                ' Penalty points
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vConvictions(14, i)).Trim()

                ' Store the Conviction_cnt
                oListItem.Tag = CStr(m_vConvictions(1, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i
            '    'Populate the cells

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateConvictions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateLifestyle
    '
    ' Description: Populate Lifestyle List view
    '
    ' ***************************************************************** '
    Public Sub PopulateLifestyle()
        Dim oListItem As ListViewItem
        Dim vCategory, sSmoker As String

        Try
            lvwLifestyle.Items.Clear()
            If Not Information.IsArray(m_vLifestyle) Then
                ReDim m_vLifestyle(8, 0)
                m_vLifestyle(0, 0) = m_lPartyCnt
                m_vLifestyle(1, 0) = 1
                m_vLifestyle(2, 0) = ""
                m_vLifestyle(3, 0) = 1
                m_vLifestyle(4, 0) = ""
                m_vLifestyle(5, 0) = ""
                m_vLifestyle(6, 0) = ""
                m_vLifestyle(7, 0) = ""
                m_vLifestyle(8, 0) = 0
            End If



            ' Assign the details to the interface.
            For i As Integer = m_vLifestyle.GetLowerBound(1) To m_vLifestyle.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                'developer guide no. 101
                GetPersonLookups(vCategoryId:=m_vLifestyle(3, i), vCategoryDescription:=vCategory)

                oListItem = lvwLifestyle.Items.Add(vCategory.Trim(), "LifestyleImage")

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLifestyle(2, i)).Trim()

                'MSS210901 - Added section form UW with switch
                If m_sUnderwritingOrBroking = "U" Then
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_vLifestyle(4, i))

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtDate.Text
                Else
                    ' Column 3
                    'DC 20/10/00 check for Null Data Of Birth
                    'MKR Force date to specific format to cater
                    'for different regional date settings
                    Dim TempDate As Date
                    If (IIf(DateTime.TryParse(CStr(m_vLifestyle(4, i)).Trim(), TempDate), TempDate.ToString("dd/MM/yyyy"), CStr(m_vLifestyle(4, i)).Trim())) = "29/12/1899" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ""
                        m_vLifestyle(4, i) = ""
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vLifestyle(4, i)).Trim()
                    End If
                End If

                If i = 0 Then
                    If CStr(m_vLifestyle(4, i)) > "" Then
                        If m_sUnderwritingOrBroking = "U" Then
                            txtDOB.Text = txtDate.Text
                        Else
                            txtDOB.Text = CStr(m_vLifestyle(4, i))
                        End If
                    End If
                End If
                'MSS210901 - Merge end

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vLifestyle(5, i)).Trim()
                If i = 0 Then
                    If CStr(m_vLifestyle(5, i)) > "" Then
                        ddGender.Text = CStr(m_vLifestyle(5, i))
                    End If
                End If

                ' Column 5
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vLifestyle(6, i)).Trim()
                If i = 0 Then
                    If CStr(m_vLifestyle(6, i)) > "" Then
                        ddOccupation.Text = CStr(m_vLifestyle(6, i))
                    End If
                End If

                ' Column 6
                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(m_vLifestyle(7, i)).Trim()
                If i = 0 Then
                    If CStr(m_vLifestyle(7, i)) > "" Then
                        ddSecondaryOccupation.Text = CStr(m_vLifestyle(7, i))
                    End If
                End If

                ' Column 7
                'DJM 11/11/2003 : Display smoker column correctly (either Yes or No).
                sSmoker = "No"
                If CStr(m_vLifestyle(8, i)) <> "" Then
                    If CInt(m_vLifestyle(8, i)) = 1 Then
                        sSmoker = "Yes"
                    End If
                End If

                ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sSmoker.Trim()
                If i = 0 Then
                    If sSmoker = "No" Then
                        chkSmoker.CheckState = CheckState.Unchecked
                    Else
                        chkSmoker.CheckState = CheckState.Checked
                    End If
                End If

                ' Store the Lifestyle_id
                oListItem.Tag = CStr(m_vLifestyle(1, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateLifestyle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateProspect
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function PopulateProspect() As Integer
        Dim result As Integer = 0
        Dim vAgentReference As Object
        Dim vCurrentIntermediary As Integer
        Dim vProspectStatusID As Integer
        Dim vPreviousInsurerCnt As Object
        Dim vPreviousBrokerCnt, vCampaigns(,) As Object
        Dim vStrengthCodeID As String = ""
        Dim vPolicies(,) As Object

        Dim sTemp As String = ""
        Dim oListItem As ListViewItem
        ' Lookup value contants.
        'Const ACValueTableName As Integer = 0      ''Unused Local Variables
        'Const ACValueID As Integer = 1             ''Unused Local Variables
        'Const ACValueStartPos As Integer = 2       ''Unused Local Variables
        'Const ACValueNumber As Integer = 3         ''Unused Local Variables

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create iProspect if not already done so

            If Convert.IsDBNull(vAgentReference) Or IsNothing(vAgentReference) Then
                txtAgentReference.Text = ""
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentReference, vControlValue:=vAgentReference)

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Get additional details required for display that not stored on this
            'record

            m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=vCurrentIntermediary, vAgentref:=m_sCurrentAgentRef, vAgentName:=m_sCurrentAgentName, vInsurerCnt:=vPreviousInsurerCnt, vInsurerRef:=m_sInsurerRef, vInsurerName:=m_sInsurerName, vBrokerCnt:=vPreviousBrokerCnt, vBrokerRef:=m_sBrokerRef, vBrokerName:=m_sBrokerName)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProspect")
            End If


            If Convert.IsDBNull(vCurrentIntermediary) Or IsNothing(vCurrentIntermediary) Then
                m_lCurrentAgent = -1
            Else
                m_lCurrentAgent = vCurrentIntermediary
            End If

            'MKR 11/10/2004 PN 15599 Doubling the & character to avoid displaying "_"
            sTemp = m_sCurrentAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")
            pnlCurrentAgent.Text = sTemp



            If Convert.IsDBNull(vProspectStatusID) Or IsNothing(vProspectStatusID) Or vProspectStatusID.Equals(0) Then
                cboProspectingStatus.SelectedIndex = -1
            Else
                For i As Integer = 0 To cboProspectingStatus.Items.Count - 1
                    If VB6.GetItemData(cboProspectingStatus, i) = vProspectStatusID Then
                        cboProspectingStatus.SelectedIndex = i
                    End If
                Next i
            End If
            'Eck 2005 Roadmap - moved to DisplayLookupDetails
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=SIRLookupStrengthCode, _
            ''        ctlLookup:=cboStrengthCode)

            pnlInsurerName.Text = m_sInsurerName
            txtInsurerRef.Text = m_sInsurerRef

            txtInsurerRef.Tag = IIf(Convert.IsDBNull(vPreviousInsurerCnt) Or IsNothing(vPreviousInsurerCnt), "", vPreviousInsurerCnt)

            pnlBrokerName.Text = m_sBrokerName
            txtBrokerRef.Text = m_sBrokerRef

            'Get additional details required for display that not stored on this
            'record

            lvwCampaigns.Items.Clear()
            lvwCampaigns.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwCampaigns.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1600))
            lvwCampaigns.Columns.Item(2).Width = CInt(0)

            'MSS210901 - Added better UW code
            If vStrengthCodeID > "" Then
                For i As Integer = 0 To cboStrengthCode.Items.Count - 1
                    If VB6.GetItemData(cboStrengthCode, i) = StringsHelper.ToDoubleSafe(vStrengthCodeID) Then
                        cboStrengthCode.SelectedIndex = i
                    End If
                Next i
                '        For l = LBound(m_vLookupValues, 2) To UBound(m_vLookupValues, 2)
                '        ' Check for a match of the table name.
                '        If (Trim$(m_vLookupValues(ACValueTableName, l))) = SIRLookupStrengthCode Then
                '            m_vLookupValues(ACValueID, l) = CLng(vStrengthCodeID)
                '            Exit For
                '        End If
                '    Next l
            End If
            'MSS210901 - Merge end

            If Information.IsArray(vCampaigns) Then

                ' Assign the details to the interface.

                For i As Integer = vCampaigns.GetLowerBound(1) To vCampaigns.GetUpperBound(1)

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to the first column.
                    ' Column 1
                    '            Call GetCampaignLookups(vCategoryId:=vCampaigns(0, i), _
                    'vCategoryDescription:=vCategory)

                    '            Set oListItem = lvwLifestyle.ListItems.Add(, , _
                    'Trim$(vCategory), , CampaignImage)


                    oListItem = lvwCampaigns.Items.Add(CStr(vCampaigns(PMBCampaignDescription, i)).Trim(), "")

                    ' Assign details to other the columns
                    ' Column 2

                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=CStr(vCampaigns(PMBCampaignCampaignDate, i)).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDate.Text.Trim()


                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CDate(vCampaigns(PMBCampaignCampaignDate, i)).ToString("yyyyMMdd")

                    ' Store the id

                    oListItem.Tag = CStr(vCampaigns(PMBCampaignRecordNo, i)).Trim()
                    ' {* USER DEFINED CODE (End) *}

                Next i
            End If

            lvwPolicies.Items.Clear()
            lvwPolicies.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwPolicies.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1600))
            lvwPolicies.Columns.Item(2).Width = CInt(VB6.TwipsToPixelsX(600))
            lvwPolicies.Columns.Item(3).Width = CInt(VB6.TwipsToPixelsX(1600))

            If Information.IsArray(vPolicies) Then

                ' Assign the details to the interface.

                For i As Integer = vPolicies.GetLowerBound(1) To vPolicies.GetUpperBound(1)

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to the first column.
                    ' Column 1
                    '            Call GetCampaignLookups(vCategoryId:=vCampaigns(0, i), _
                    'vCategoryDescription:=vCategory)

                    '            Set oListItem = lvwLifestyle.ListItems.Add(, , _
                    'Trim$(vCategory), , CampaignImage)
                    For j As Integer = 0 To cboPolicyType.Items.Count - 1

                        If StringsHelper.ToDoubleSafe(CStr(vPolicies(PMBPolicyPolicyTypeID, i)).Trim()) = VB6.GetItemData(cboPolicyType, j) Then
                            sTemp = VB6.GetItemString(cboPolicyType, j).Trim()
                            Exit For
                        End If
                    Next j

                    'Set oListItem = lvwPolicies.ListItems.Add(, , _
                    'Trim$(vPolicies(PMBPolicyTypeDescription, i)), , PolicyImage)

                    ' oListItem.ImageKey = "PolicyImage"
                    oListItem = lvwPolicies.Items.Add(sTemp, "PolicyImage")

                    ' Assign details to other the columns
                    ' Column 2

                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=CStr(vPolicies(PMBPolicyRenewalDate, i)).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDate.Text.Trim()


                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(vPolicies(PMBPolicyNoOfTimesQuoted, i)).Trim()


                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=CStr(vPolicies(PMBPolicyTargetPremium, i)).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtCurrency.Text.Trim()


                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CDate(vPolicies(PMBPolicyRenewalDate, i)).ToString("yyyyMMdd")

                    ' Store the id

                    oListItem.Tag = CStr(vPolicies(PMBPolicyPolicyID, i)).Trim()
                    ' {* USER DEFINED CODE (End) *}

                Next i
            End If

            'SD 22/08/2002 If Broking, then previous broker irrelevant
            If m_sUnderwritingOrAgency = "A" Then
                fraPreviousBroker.Visible = False
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateProspect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateProspect", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateLoyaltySchemes
    '
    ' Description: Fills the grid control with PartyLoyaltyScheme details
    '
    ' ***************************************************************** '
    Private Sub PopulateLoyaltySchemes() 'RAW 18/11/2002 : PS005 : Added


        Dim oListItem As ListViewItem

        Try

            If Not Information.IsArray(m_vLoyaltySchemes) Then
                Exit Sub
            End If

            lvwLoyaltySchemes.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vLoyaltySchemes.GetLowerBound(1) To m_vLoyaltySchemes.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwLoyaltySchemes.Items.Add(CStr(m_vLoyaltySchemes(PMBLoyaltyLoyaltySchemeName, i)).Trim())

                ' Assign details to other the columns

                ' MemberNumber
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vLoyaltySchemes(PMBLoyaltyMemberNumber, i)).Trim()

                ' StartDate
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=CStr(m_vLoyaltySchemes(PMBLoyaltyStartDate, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtDate.Text.Trim()

                ' EndDate
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=CStr(m_vLoyaltySchemes(PMBLoyaltyEndDate, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDate.Text.Trim()

                ' Store the PartyLoyaltySchemeId
                oListItem.Tag = CStr(m_vLoyaltySchemes(PMBLoyaltyPartyLoyaltySchemeID, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i
            '    'Populate the cells

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateLoyaltySchemes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                Select Case Task
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit
                        ' Update the business from the interface.
                        m_lReturn = InterfaceToBusiness()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update business.
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                End Select

            End If

            ' Check the task.
            Select Case Task
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Check if form has been cancelled, if so,
                    ' prompt if you wish to lose details.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        ' Form hasn't been cancelled, so we just go
                        ' ahead and add the details.

                        ' Add the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to add the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details (m_oBusiness.Update)", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                        'AR20050214 - PN18407 Move call until after addresses have been saved
                        'eck310700
                        '                m_lReturn& = UpdateOrion(vPartyCnt:=m_oBusiness.PartyCnt)
                        '                ' Check for errors.
                        '                If (m_lReturn& <> PMTrue) Then
                        '                   ' Failed to add the details
                        '                   ProcessCommand = PMFalse
                        '
                        '                   ' Log Error.
                        '                   LogMessage _
                        ''                       iType:=PMLogError, _
                        ''                       sMsg:="Failed to add the Orion details", _
                        ''                       vApp:=ACApp, _
                        ''                       vClass:=ACClass, _
                        ''                       vMethod:="ProcessCommand"
                        '                End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then

                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()

                        'MH Request - Always confirm cancellation
                        '                If (m_lReturn& = PMDataChanged) Then
                        ' Get string messages


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetailsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

                        ' Check message result.
                        If iMsgResult = System.Windows.Forms.DialogResult.No Then
                            ' Set return to false, meaning
                            ' don't cancel.
                            result = gPMConstants.PMEReturnCode.PMFalse
                        End If
                        '                End If
                    Else

                        'Pass back old and new address
                        m_lReturn = GetOldAndNewAddress()

                        m_oBusiness.OldAddress = VB6.CopyArray(m_vOldAddress)

                        m_oBusiness.NewAddress = VB6.CopyArray(m_vNewAddress)

                        'Pass back old and new client code

                        m_oBusiness.OldClientCode = m_sOldIdReference.Trim()

                        m_oBusiness.NewClientCode = txtIDReference.Text.Trim()

                        'Pass back old and new client name

                        m_oBusiness.OldClientName = m_sOldResolvedName.Trim()

                        m_oBusiness.NewClientName = m_sResolved.Trim()


                        m_oBusiness.IsAmended = m_bIsAmended
                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                        'AR20050214 - PN18407 Move call until after addresses have been saved

                        ''eck060700
                        '                m_lReturn& = UpdateOrion(vPartyCnt:=m_oBusiness.PartyCnt)
                        '                ' Check for errors.
                        '                If (m_lReturn& <> PMTrue) Then
                        '                   ' Failed to add the details
                        '                   ProcessCommand = PMFalse
                        '
                        '                   ' Log Error.
                        '                   LogMessage _
                        ''                       iType:=PMLogError, _
                        ''                       sMsg:="Failed to add the Orion details", _
                        ''                       vApp:=ACApp, _
                        ''                       vClass:=ACClass, _
                        ''                       vMethod:="ProcessCommand"
                        '                End If
                        '
                    End If
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process command", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
	
	' ***************************************************************** '
	' Name: SelectParty
	'
	' Description: Call Find Party component to choose a party
	'
	' ***************************************************************** '
	Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef vDateCancelled As Object = Nothing) As Integer 'CT 19/07/00 added vResolvedName parameter
		
		
        Dim result As Integer = 0
        'developer guide no.108
        Dim oFindParty As iPMBFindParty.Interface_Renamed
		Dim vKeyArray As Object
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            'oFindParty = New iPMBFindParty.Interface()
            oFindParty = New iPMBFindParty.Interface_Renamed()
			
			'Set appropriate key if agent only


			If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 1)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNameSourceId

                vKeyArray(1, 1) = m_iPartySourceId

                m_lErrorNumber = oFindParty.SetKeys(vKeyArray)
				
				If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
					oFindParty = Nothing
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
			m_lErrorNumber = CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                'm_lErrorNumber = oFindParty.Terminate()
                oFindParty.Dispose()
				oFindParty = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oFindParty.CallingAppName = "iPMBPartyPC.Interface"
			
			'SD 31/07/2002
			m_lErrorNumber = oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=GIIPMProcessModeGeneric, vEffectiveDate:=DateTime.Now)
			
			If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
				oFindParty = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			oFindParty.NotEditable = 1
			
			m_lErrorNumber = oFindParty.Start()
			
			If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
				oFindParty = Nothing
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
				
				vPartyCnt = oFindParty.PartyCnt
				vShortName = oFindParty.ShortName


				vDateCancelled = oFindParty.DateCancelled
				

                'developer guide no. 118
                vName = oFindParty.LongName
                vResolvedName = oFindParty.ResolvedName 'CT 19/07/00
			Else
				If oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
					result = gPMConstants.PMEReturnCode.PMCancel
				Else
					result = gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			
            oFindParty.Dispose()
			
			oFindParty = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectPartyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
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
			
			'Party Title must be entered
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=ddTitle, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'sj 18/06/2002 - start
			lblTitle.Font = VB6.FontChangeBold(lblTitle.Font, True)
			'sj 18/06/2002 - end
			
			
			If m_bDuplicateClientIdentification Then
				'txtIDReference.Enabled = False
				'lblIDReference.FontBold = False
				'lblIDReference.ForeColor = QBColor(7)
				
				'Reference must be entered
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			Else
				'Reference must be entered
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			End If
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'SurName must be entered
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSurname, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'ForeName must be entered
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtForename, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Initials must be entered
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInitials, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringUpper, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'DOB must be entered
			'DC 20/10/00 now non mandatory
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDOB, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Agent Ref
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Agent Name
			
			'Consultant Ref
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtConsultantRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'File code
			'sj 02/07/2002 - start
			If m_bAONAffinity Then
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMembershipId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Else
				m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFileCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			'sj 02/07/2002 - end
			'Date
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Currency
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrency, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'Agent Reference
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBrokerRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatStringCase, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'MSS210901 - Added from UW
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCCJ, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'MSS210901 - Merge end
			
			' CTAF 270601 - Salutation
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSalutation, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSource, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTPPassword, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'FSA Phase III
			
			m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTobLetter, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			'FSA Phase III End
			
			' {* USER DEFINED CODE (End) *}
			
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
			m_ctlTabFirstLast(ACControlEnd, 0) = txtConsultantRef
			m_ctlTabFirstLast(ACControlStart, 1) = lvwAddresses
			m_ctlTabFirstLast(ACControlEnd, 1) = lvwContacts
			m_ctlTabFirstLast(ACControlStart, 2) = cboCurrency
			m_ctlTabFirstLast(ACControlEnd, 2) = ddSecEmploymentStatus
			m_ctlTabFirstLast(ACControlStart, 3) = lvwConvictions
			m_ctlTabFirstLast(ACControlEnd, 3) = txtCCJ
			m_ctlTabFirstLast(ACControlStart, 4) = txtDOB
			m_ctlTabFirstLast(ACControlEnd, 4) = lvwLifestyle
			m_ctlTabFirstLast(ACControlStart, 5) = lvwLoyaltySchemes
			m_ctlTabFirstLast(ACControlEnd, 5) = lvwLoyaltySchemes
			
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
	' Name: SetInterfaceDefaults
	'
	' Description: Sets all of the interface default values.
	'
	' ***************************************************************** '
	Private Function SetInterfaceDefaults() As Integer
		Dim result As Integer = 0
		Dim vCategory As Object
		Dim oListItem As ListViewItem
		Dim vValue As String = ""
		Dim bNZConfig As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Center the interface.
			'CenterForm Me
			
			SSTabHelper.SetSelectedIndex(TabMainTab, 0)
			TabMainTab.Visible = True
			TabMainTab.Top = 0
			'tabProspecting.Visible = False
			
			'DD 28/10/2003 - only show password when FSA is enabled.
			m_lReturn = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, 1, vValue)
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If gPMFunctions.NullToString(vValue) <> "1" Then
				lblTPPassword.Visible = False
				txtTPPassword.Visible = False
			End If
			
			'AR20061107 - Get New Zealand product option
			m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, v_vBranch:=1, r_vUnderwriting:=vValue)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			bNZConfig = (gPMFunctions.ToSafeString(vValue) = "1")
			
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
			
			' Display all language specific captions.
			m_lReturn = DisplayCaptions()
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Set the status of the Navigate button.
			'cmdProspect.Enabled = False
			cmdEditAd.Enabled = False
			cmdDeleteAd.Enabled = False
			cmdEditCon.Enabled = False
			cmdDeleteCon.Enabled = False
			cmdEditConv.Enabled = False
			cmdDeleteConv.Enabled = False
			cmdEditLife.Enabled = False
			cmdDeleteLife.Enabled = False
			cmdEditPol.Enabled = False
			cmdDeletePol.Enabled = False
			cmdEditLoyaltyScheme.Enabled = False
			cmdDeleteLoyaltyScheme.Enabled = False
			
			cboCreditCard.Visible = False
			lblCreditCard.Visible = False
			
			chkAgent.CheckState = CheckState.Unchecked
			chkProspect.CheckState = CheckState.Unchecked
			
			m_lReturn = SetFirstLastControls()

			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwAddresses.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwAddresses.FullRowSelect = True
            lvwAddresses.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwContacts.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwContacts.FullRowSelect = True
            lvwContacts.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwConvictions.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwConvictions.FullRowSelect = True
            lvwConvictions.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwLifestyle.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwLifestyle.FullRowSelect = True
            lvwLifestyle.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwCampaigns.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            '         End If
            lvwCampaigns.FullRowSelect = True
            lvwCampaigns.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwPolicies.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            lvwPolicies.FullRowSelect = True
            lvwPolicies.HideSelection = False
			
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=lvwLoyaltySchemes.Handle.ToInt32(), v_vShowRowSelect:=True)
			
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '	Return gPMConstants.PMEReturnCode.PMFalse
            '         End If
				
            lvwLoyaltySchemes.FullRowSelect = True
            lvwLoyaltySchemes.HideSelection = False

			' Set any other default values to the interface.
			
			' TF031298 - Disable menu options if New Client
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				' Column 1

                'developer guide no.98
                GetPersonLookups(vCategoryId:=1, vCategoryDescription:=vCategory)
                oListItem = lvwLifestyle.Items.Add(CStr(vCategory), "")
				
				' Assign details to other the columns
				' Column 2
				ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ""
				
				' Column 3
				ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ""
				' Column 4
				ListViewHelper.GetListViewSubItem(oListItem, 3).Text = ""
				
				' Column 5
				ListViewHelper.GetListViewSubItem(oListItem, 4).Text = ""
				
				' Column 6
				ListViewHelper.GetListViewSubItem(oListItem, 5).Text = ""
				
				' Column 7
				ListViewHelper.GetListViewSubItem(oListItem, 6).Text = ""
				
				' Store the Lifestyle_id
				oListItem.Tag = CStr(0)
				
				'AR20061107 - Default Domiciled for Tax to ticked for NZ market
				uctPartyTax1.IsDomiciledForTax = bNZConfig
				
			End If
			
			' CTAF 090300 - Add Terms of Payment
			'DC081200 changed from combo to dropdown
            'With ddTermsOfPayment
            '	.AddItem("")
            '	.AddItem("30 days")
            '	.AddItem("60 days")
            '	.AddItem("90 days")
            '	.AddItem("120 days")
            '	.AddItem("180 days")
            'End With
			
			'DC 26/07/00
			'Get correspondence types

			m_lReturn = m_oBusiness.GetCorrespondenceTypes(vCorrespondenceTypes:=m_vCorrespondenceTypes)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
			End If
			PopulateCorrespondenceTypes()
            If cboCorrespondenceType.SelectedIndex = -1 AndAlso m_iTask = PMEComponentAction.PMAdd Then
                cboCorrespondenceType.Text = "Letter"
            End If
			
			'Maintain Client Code
			m_lReturn = SetClientCodeCntl()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
			End If
			
			If Task = gPMConstants.PMEComponentAction.PMView Then
				txtIDReference.Enabled = False
			End If
			
			' {* USER DEFINED CODE (End) *}
			If m_bIsNRMA Then
				txtLoyaltyNumberPrefix.Visible = True
				txtLoyaltyNumber.MaxLength = 10
				'Right justify
				txtAlternativeIdentifier.TextAlign = HorizontalAlignment.Right
				cboBranch.Enabled = False
				txtAlternativeIdentifier.Text = PartyFunc.m_kBlankAlternativeIdentifier
			Else
				txtLoyaltyNumberPrefix.Visible = False
				txtLoyaltyNumber.Left = cboSeasonalGift.Left
				txtLoyaltyNumber.Width = cboSeasonalGift.Width
			End If
			
			If m_bAONAffinity Then
				txtMembershipId.Visible = True
				lblMembershipId.Visible = True
				txtFileCode.Visible = False
				lblFileCode.Visible = False
			Else
				txtMembershipId.Visible = False
				lblMembershipId.Visible = False
				txtFileCode.Visible = True
				lblFileCode.Visible = True
			End If
			
			' set agent and black list defaults
			fraAgent.Height = VB6.TwipsToPixelsY(1935)
			fraBlackList.Visible = False
			
			' if this is underwriting and the client blacklisting
			' system option is switched on then
			' then override defaults for agent and blacklisting frames
			If m_sUnderwritingOrAgency = "U" Then
				If m_bSystemOptionClientBlacklistingInForce Then
					fraAgent.Height = VB6.TwipsToPixelsY(1200)
					fraBlackList.Visible = True
				End If
			End If
			
			chkFeeClient.Visible = (m_sUnderwritingOrAgency = "A")
			chkFeeClient.CheckState = CheckState.Unchecked
			
			If m_bMultiTreeAccounting Then
				'Branch field read only
				cboBranch.Enabled = False
			End If
			
			If m_bLimitPersonalClientEditFields Then
				'First Disable complete form
				m_lReturn = DisableForm(True)
				
				'Enable required fields - Identity Tab
				txtIDReference.Enabled = True
				txtSurname.Enabled = True
				txtForename.Enabled = True
				txtInitials.Enabled = True
				
				cboBranch.Enabled = True
				cboSubBranch.Enabled = True
				
				ddTitle.Enabled = True
				
				'Enable required fields - Contacts Tab
				cmdAddCon.Enabled = True
				cmdAddAd.Enabled = True
				
				txtSalutation.Enabled = True
				
				'Enable required fields - Additions Tab
				cboRenewalStopCode.Enabled = True
				
				ddEmployment.Enabled = True
				ddSecEmploymentStatus.Enabled = True
				
				ddOccupation.Enabled = True
				ddSecondaryOccupation.Enabled = True
				
				ddBusiness.Enabled = True
				ddSecondaryBusiness.Enabled = True
				
				'Enable required fields - Lifestyle Tab
				txtDOB.Enabled = True
				ddMaritalStatus.Enabled = True
				
				'Hide Convictions and Misc tabs, renumber Lifestyle Tab
				SSTabHelper.SetTabVisible(TabMainTab, 3, False)
				SSTabHelper.SetTabVisible(TabMainTab, 5, False)
				SSTabHelper.SetTabCaption(TabMainTab, 4, "&4 - Lifestyle ")
				'DN 12/12/02
				cmdNext(4).Visible = False
				
			End If
			
			If m_bAONPRClientScreenChanges Then
				fraAreaCode.Visible = True
				lblArea.Text = "County:"
				lblSeasonalGift.Text = "Cancellation Reason:"
				cboSeasonalGift.Left += VB6.TwipsToPixelsX(480)
				cboSeasonalGift.Width -= VB6.TwipsToPixelsX(480)
			End If
			
			lblSubBranch.Visible = m_bShowSubBranchID
			cboSubBranch.Visible = m_bShowSubBranchID
			'eck19052005
			'fraClient.Height = 2100
			
			'Party Bank Details
			m_lReturn = uctPartyBankControl1.SetInterfaceDefaults()
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
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
	' Name: UnlockParty
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function UnlockParty() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			m_lReturn = m_oPMLock.UnLockKey(sKeyName:="party_cnt", vKeyValue:=m_lPartyCnt, iUserID:=g_iUserId)
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to process the interface.
				result = gPMConstants.PMEReturnCode.PMFalse
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock the party", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				Return result
				
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
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
		'RWH(11/07/2000) Altered to move PostCode from far left to far
		'right of ListView.
		
		Dim result As Integer = 0
		Dim oListItem As ListViewItem
        Dim vNewAddresses, vOldAddresses(,) As Object
		Dim bFirst As Boolean
        Dim i As Integer
		Dim sAddressUsage As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Go thru original address array to get list of old addresses
			If Information.IsArray(m_vAddresses) Then
				ReDim vOldAddresses(1, m_vAddresses.GetUpperBound(1))
				For i = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                    vOldAddresses(0, i) = CInt(m_vAddresses(6, i))

                    vOldAddresses(1, i) = CInt(m_vAddresses(1, i))
				Next i
			End If
			
			'Go thru addresses grid to get list of new addresses
			i = 1
			bFirst = True
			Do 
				If i > lvwAddresses.Items.Count Then
					Exit Do
				End If
				
				oListItem = lvwAddresses.Items.Item(i - 1)
				
				'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
				Select Case (m_sDefaultCountryCode.Trim())
					Case "GBR"
						sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
					Case Else
						sAddressUsage = oListItem.Text.Trim()
				End Select
				
				'RWH(11/07/2000) Usage is now first item in ListView.
				If sAddressUsage = "" Then
					Exit Do
				Else
					
					If bFirst Then
						ReDim vNewAddresses(1, i - 1)
						bFirst = False
					Else
						ReDim Preserve vNewAddresses(1, i - 1)
					End If
					


                    vNewAddresses(0, i - 1) = Convert.ToString(oListItem.Tag)
					
					For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
						'RWH(11/07/2000) Usage is now first item in ListView.
						If sAddressUsage = CStr(m_vAddressTypes(1, j)) Then

                            vNewAddresses(1, i - 1) = m_vAddressTypes(0, j)
							Exit For
						End If
					Next j
					
				End If
				i += 1
				
			Loop 
			
			'Delete old address usages in database
			If (Information.IsArray(vOldAddresses)) And (Not Information.IsArray(vNewAddresses)) Then

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

					For j As Integer = vNewAddresses.GetLowerBound(1) To vNewAddresses.GetUpperBound(1)




                        If (vNewAddresses(0, j).Equals(vOldAddresses(0, i))) And (vNewAddresses(1, j).Equals(vOldAddresses(1, i))) Then

                            vNewAddresses(0, j) = 0

                            vOldAddresses(0, i) = 0
						End If
					Next j
				Next i
				
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
	
	
	' ***************************************************************** '
	' Name: UpdateAssociates
	'
	' Description: This goes thru all Associates in the the grid control
	' and the original Associate array and sees what the differences
	' are. It then adds new Associates or deletes existing ones according
	' to what user has done.
	'
	' ***************************************************************** '
	'UPGRADE_NOTE: (7001) The following declaration (UpdateAssociates) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function UpdateAssociates() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			'
			'   DC 03/05/00
			'   Cater for more than one Associate
			'    m_lReturn& = m_oBusiness.UpdateAssociates(vPartyCnt:=m_lPartyCnt, _
			''                                            vIsAssociate:=1, _
			''                                            vAssociatedCnt:=m_lAssociatedCnt, _
			''                                            vAssociateDescription:=m_sAssociateRole)

			'm_lReturn = m_oBusiness.UpdateAssociates(vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)
			'
			'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				'Return gPMConstants.PMEReturnCode.PMFalse
			'End If
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
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAssociates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
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
		Dim oListItem As ListViewItem
		Dim vNewContacts, vOldContacts As Object
		Dim bFirst As Boolean
        Dim i As Integer
		
		
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
			'SP171298
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
			
			'Add new contacts in database
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
	
	' ***************************************************************** '
	' Name: ValidateOK
	'
	' Description: This validates mandatory address types and duplicate
	' addresses
	'
	' ***************************************************************** '
	Private Function ValidateOK() As Integer
		'RWH(11/07/2000) Altered to move PostCode from far left to far
		'right of ListView.
		
		Dim result As Integer = 0
		Dim iMainAddresses As Integer
		Dim bDuplicate As Boolean
		Dim lAddressCnt As Integer
		Dim SysDate, OldDate As Date
		Dim oListItem, oListItem2 As ListViewItem

		Dim sAddressUsage, sChangeCode As String

		Dim sPartyCode, sMsg As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			' RAM20031002 : Bug Fix for PN Issue 6021. Restrict Special Characters in the
			'               ShortName and name field - START
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			txtIDReference.Text = txtIDReference.Text.Trim()
			
			If txtIDReference.Text.Trim() = "" Then
				MessageBox.Show("Must have Client Code", "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
				SSTabHelper.SetSelectedIndex(TabMainTab, 0)
				If txtIDReference.Enabled Then
					txtIDReference.Focus()
				End If
				Return gPMConstants.PMEReturnCode.PMFalse
			End If

            If txtIDReference.Text.IndexOf("'"c) >= 0 OrElse txtIDReference.Text.IndexOf("’"c) >= 0 Then
                MessageBox.Show("' (Apostrophes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If txtIDReference.Text.IndexOf("|"c) >= 0 Then
				MessageBox.Show("| (Pipes) are not Allowed in Client Code", Application.ProductName)
				SSTabHelper.SetSelectedIndex(TabMainTab, 0)
				txtIDReference.Focus()
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			If txtIDReference.Text.IndexOf(","c) >= 0 Then
				MessageBox.Show(", (Commas) are not Allowed in Client Code", Application.ProductName)
				SSTabHelper.SetSelectedIndex(TabMainTab, 0)
				txtIDReference.Focus()
				Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Dim sOptionValue As String = String.Empty
            m_lReturn = iPMFunc.GetSystemOption(v_iOptionNumber:=10, r_sOptionValue:=sOptionValue, v_iSourceID:=g_oObjectManager.SourceID)

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "2" Then
            If txtIDReference.Text <> "" Then
                If IsValidString(txtIDReference.Text) = False Then
                    MessageBox.Show("Client Code can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    If txtIDReference.Visible Then
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        txtIDReference.Focus()
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If
            End If
			
			If txtSurname.Text.IndexOf("|"c) >= 0 Then
				MessageBox.Show("| (Pipes) are not Allowed in Last Name", Application.ProductName)
				SSTabHelper.SetSelectedIndex(TabMainTab, 0)
				txtSurname.Focus()
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			'MKR 12/10/2004 PN 6021 Allowing "," in Surname as it is a part
			'of Resolved name
			'If InStr(txtSurname.Text, ",") > 0 Then
			'    MsgBox (", (Commas) are not Allowed in Last Name")
			'    TabMainTab.Tab = 0
			'    txtSurname.SetFocus
			'    ValidateOK = PMFalse
			'    Exit Function
            'End If
            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "2" Then
            If txtSurname.Text <> "" Then
                If IsValidString(txtSurname.Text) = False Then
                    MessageBox.Show("Last Name can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    If txtSurname.Visible Then
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        txtSurname.Focus()
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If
            End If

			If txtForename.Text.IndexOf("|"c) >= 0 Then
				MessageBox.Show("| (Pipes) are not Allowed in Fore Name", Application.ProductName)
				SSTabHelper.SetSelectedIndex(TabMainTab, 0)
				txtForename.Focus()
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			If txtForename.Text.IndexOf(","c) >= 0 Then
				MessageBox.Show(", (Commas) are not Allowed in Fore Name", Application.ProductName)
				SSTabHelper.SetSelectedIndex(TabMainTab, 0)
				txtForename.Focus()
				Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not String.IsNullOrEmpty(sOptionValue) AndAlso sOptionValue = "2" Then
            If txtForename.Text <> "" Then
                If IsValidString(txtForename.Text) = False Then
                    MessageBox.Show("Fore Name can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    If txtForename.Visible Then
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        txtForename.Focus()
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If
            End If

            If txtInitials.Text <> "" Then
                If IsValidString(txtInitials.Text) = False Then
                    MessageBox.Show("Last Name can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } |", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    If txtInitials.Visible Then
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        txtInitials.Focus()
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If

			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			' RAM20031002 : Bug Fix for PN Issue 6021. Restrict Special Characters in the
			'               ShortName and name field - END
			''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
			
			'Check to see if client code already exists
			
			'If this is a new client or an existing one with it's client code changed then
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtIDReference.Text.Trim().ToUpper() <> m_sOldIdReference.Trim().ToUpper()) Then
				
				sPartyCode = txtIDReference.Text.Trim()
				

				m_lReturn = m_oBusiness.CheckReference(sPartyCode)
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					MessageBox.Show("Unable to access business object", "Internal Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
				'If the returned code is an empty string, then the code already exists
				If sPartyCode = "" Then
					
					sMsg = "The client code entered already exists."
					MessageBox.Show(sMsg, "Invalid Client Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
					
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
				
			End If
			
			If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
				'Add this just in case Apply is added at later stage
				m_sOldIdReference = txtIDReference.Text
			End If
			
			'Display warning message that client code has changed.
			If m_iTask = gPMConstants.PMEComponentAction.PMEdit And txtIDReference.Text.Trim().ToUpper() <> m_sOldIdReference.Trim().ToUpper() Then
				
				sChangeCode = CStr(MessageBox.Show("Warning! You are about to change the client code. Do you wish to change the code?", "Client Code Changed", MessageBoxButtons.YesNo))
				Select Case sChangeCode
					Case CStr(System.Windows.Forms.DialogResult.No)
						txtIDReference.Text = m_sOldIdReference
				End Select
				
			End If
			
			'EK060199 Bug 159
			'Date Validation
			SysDate = DateTime.Today
			OldDate = DateTime.Parse("January 1 1901")
			'DC 20/10/00 make sure theres somethin entered for txtDOB
			If txtDOB.Text <> "" Then
				If CDate(txtDOB.Text) > SysDate Or CDate(txtDOB.Text) < OldDate Then
					MessageBox.Show("Date of Birth must be later than 1900 and earlier than today", Application.ProductName)
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If

            ' Validate Preferred Correspondence Type
            Dim bValidCorrespondenceType As Boolean = False
            If cboCorrespondenceType.SelectedIndex = -1 AndAlso lvwContacts.Items.Count = 0 Then
                bValidCorrespondenceType = True
            ElseIf cboCorrespondenceType.SelectedIndex <> -1 AndAlso lvwContacts.Items.Count = 0 _
                AndAlso cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper <> "LETTER" Then
                bValidCorrespondenceType = False
            ElseIf cboCorrespondenceType.SelectedIndex = -1 AndAlso lvwContacts.Items.Count > 0 Then
                bValidCorrespondenceType = True
            Else
                If cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper = "LETTER" Then
                    bValidCorrespondenceType = True
                Else
                    For Each item As ListViewItem In lvwContacts.Items
                        If item.SubItems(3).Text.Trim.ToUpper = cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper Then
                            bValidCorrespondenceType = True
                            Exit For
                        End If
                    Next
                End If
            End If
            If Not bValidCorrespondenceType Then
                MessageBox.Show("No corresponding contact type exists. Please add one or change the preferred method of correspondence. ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(TabMainTab, 1)
                cboCorrespondenceType.Focus()
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
						'RWH(11/07/2000) Usage is now first item in ListView.
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
					result = gPMConstants.PMEReturnCode.PMFalse
					SSTabHelper.SetSelectedIndex(TabMainTab, 1)
					Return result
				Case 1
					'Yes
				Case Else
					'No.
					MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
					result = gPMConstants.PMEReturnCode.PMFalse
					SSTabHelper.SetSelectedIndex(TabMainTab, 1)
					Return result
			End Select
			
			'Now Ensure addresses are not used twice
			'EK 15/11/99 Use latest count
			'If (m_lAddressCount < 2) Then
			If lvwAddresses.Items.Count < 2 Then
				' less than 2 addresses so cant have duplicates
				Return result
			End If
			
			bDuplicate = False
			
			'Check for duplicates
			'But first reset the count...
			m_lAddressCount = lvwAddresses.Items.Count
			'Need to find out how to calculate no of lines in the grid
			'EK 15/11/99 Use latest count
			'For i = 1 To (m_lAddressCount)
			For i As Integer = 1 To (lvwAddresses.Items.Count)
				oListItem = lvwAddresses.Items.Item(i - 1)
				If CBool(CStr(Convert.ToString(oListItem.Tag) <> "").Trim()) Then
					lAddressCnt = Convert.ToString(oListItem.Tag)
					'EK 15/11/99
					'For j = (i + 1) To m_lAddressCount
					For j As Integer = 1 To lvwAddresses.Items.Count
						If i <> j Then
							oListItem2 = lvwAddresses.Items.Item(j - 1)
							If CBool(CStr(Convert.ToString(oListItem2.Tag) <> "").Trim()) Then
								If (Convert.ToString(oListItem2.Tag)) = lAddressCnt Then
									bDuplicate = True
									Exit For
								End If
							End If
						End If
					Next j
				End If
			Next i
			
			If bDuplicate Then
				MessageBox.Show("An address can only be used once by a particular party.", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result

			Return result
            Return result
		End Try
	End Function
	'eck060700
	' ***************************************************************** '
	' Name: UpdateOrion
	'
	' Description: Update the party_address usage table with old
	' and new addresses for the party.
	'
	' ***************************************************************** '
	Private Function UpdateOrion(ByRef vPartyCnt As Object) As gPMConstants.PMEReturnCode
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


		Dim oSIROrionUpdate As bSIROrionUpdate.Business
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			If Not m_oBusiness.IsOrionInstalled Then
				Return result
			End If
			
			Dim temp_oSIROrionUpdate As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oSIROrionUpdate, "bSIROrionUpdate.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
			oSIROrionUpdate = temp_oSIROrionUpdate
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMError
				
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise bSIROrionUpdate.Form.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOrion", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
			End If
			' Get Orion Account IDs
			'eck010900
			'DN 01/06/01 - Change PartyIDs to Longs
			Select Case Task
				Case gPMConstants.PMEComponentAction.PMAdd

					m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt)
				Case gPMConstants.PMEComponentAction.PMEdit

					m_lReturn = oSIROrionUpdate.SiriusToOrion(v_lPartyCnt:=vPartyCnt, v_iOldSourceId:=m_iPartySourceId, v_iOldPartyId:=m_lPartyId)
			End Select

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
	
	
	' PRIVATE Methods (End)
	' PRIVATE Events (Begin)
	
	Private Sub cboAccommodation_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccommodation.SelectionChangeCommitted
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
		
	End Sub
	
	Private Sub cboAccommodation_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccommodation.DropDown
		
		'Kevin Renshaw (CMG) 02/01/01 - issue 1645 the combo box refills only selected item
		'when event called
		
		'm_lReturn& = FillCombo(cboControl:=cboAccommodation, _
		''    bRefill:=True)
		
	End Sub
	
    Private Sub cboAccommodation_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccommodation.Enter
        If PMBGeneralFunc.g_oListManager Is Nothing Then
            PMBGeneralFunc.g_oListManager = g_oListManager
        End If
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboAccommodation)
    End Sub
	
	Private Sub cboAccommodation_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboAccommodation.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
		
	End Sub
	
	Private Sub cboAccommodation_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccommodation.Leave
		m_lReturn = PMBGeneralFunc.ControlLostFocus(cboAccommodation)
		
	End Sub
	
	Private Sub cboBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectionChangeCommitted

        If m_bUserMode Then
            m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=cboBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_lSubBranchId)

            m_lReturn = GetSourceBaseCurrency()

            If m_bShowSubBranchID Then
                cboSubBranch.SelectedIndex = -1
            Else
                cboSubBranch.SelectedIndex = 0
            End If
        End If
        m_iPartySourceId = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)

    End Sub
	
	Private Sub cboBranch_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.Enter
		
		' CTAF 220900
		' Make sure we're on the right tab incase this was called
		' from form controls.
		SSTabHelper.SetSelectedIndex(TabMainTab, 0)
		
	End Sub
	
	Private Sub cboCreditCard_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.SelectionChangeCommitted
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
		
	End Sub
	
	Private Sub cboCreditCard_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.DropDown
		
		m_lReturn = PMBGeneralFunc.FillCombo(cboControl:=cboCreditCard, bRefill:=False)
		
	End Sub
	
    Private Sub cboCreditCard_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.Enter
        If PMBGeneralFunc.g_oListManager Is Nothing Then
            PMBGeneralFunc.g_oListManager = g_oListManager
        End If
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboCreditCard)

    End Sub
	
	Private Sub cboCreditCard_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboCreditCard.KeyDown
		Dim KeyCode As Integer = eventArgs.KeyCode
		Dim Shift As Integer = eventArgs.KeyData \ &H10000
		m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
		
	End Sub
	
	Private Sub cboCreditCard_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.Leave
		m_lReturn = PMBGeneralFunc.ControlLostFocus(cboCreditCard)
	End Sub
	
	Private Sub cboCurrency_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.GotFocus
		'If we're here by tabbing (or back-tabbing) we're on tab 2
		SSTabHelper.SetSelectedIndex(TabMainTab, 2)
	End Sub
	
	Private Sub cboNationality_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboNationality.Leave
		
		m_lReturn = PMBGeneralFunc.ControlLostFocus(cboNationality)
		
	End Sub
	
	Private Sub cboProspectingStatus_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboProspectingStatus.SelectionChangeCommitted
		
		m_bChangedProspect = True
		
	End Sub
	
	Private Sub cboSeasonalGift_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSeasonalGift.Leave
		
		m_lReturn = PMBGeneralFunc.ControlLostFocus(cboSeasonalGift)
		
	End Sub
	
	Private Sub cboStrengthCode_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboStrengthCode.SelectionChangeCommitted
		
		m_bChangedProspect = True
		
	End Sub
	
	Private Sub chkeMPS_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkeMPS.Enter
		'If we're here by tabbing (or back-tabbing) we're on tab 0
		SSTabHelper.SetSelectedIndex(TabMainTab, 1)
	End Sub
	
	Private Sub cmdAssociates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAssociates.Click
		'DC 03/05/00
		'Allow Maintenance Of Associates
		Dim vTask As gPMConstants.PMEComponentAction
		
		Try 
			'Create shares object if not already done so
			If m_oAssociates Is Nothing Then
				
				' Get an instance of the associates interface object via
				' the public object manager.
				Dim temp_m_oAssociates As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oAssociates, sClassName:="iPMBAssociates.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
				m_oAssociates = temp_m_oAssociates
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error Message
					iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get associates object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					Exit Sub
					
				End If
				
			End If
			
			'DC030401 make sure correct mode
			'vTask = PMEdit
			vTask = m_iTask
			

			m_lReturn = m_oAssociates.SetProcessModes(vTask:=vTask)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			

			m_oAssociates.PartyCnt = m_lPartyCnt

			m_oAssociates.SearchData = m_vAssociates


			m_oAssociates.Relationships = m_vRelationships
			

			m_lReturn = m_oAssociates.Start()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Exit Sub
			End If
			
			'If not cancelled, add to grid

			If m_oAssociates.Status = gPMConstants.PMEReturnCode.PMCancel Then
				Exit Sub
			End If
			
			Me.Refresh()
			
			m_vAssociates = ""

			m_vAssociates = m_oAssociates.SearchData
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAssociates_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
		End Try
		
	End Sub
	
	'DC081200 changed from combo to dropdown
    Private Sub ddTermsOfPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub
	
	'DC081200 changed from combo to dropdown
    Private Sub ddTermsOfPayment_DropDown(ByVal Sender As Object, ByVal e As EventArgs)

        'm_lReturn& = FillCombo(cboControl:=cboTermsOfPayment, _
        ''                       bRefill:=True)

    End Sub
	
	

    Private Sub cboTermsOfPayment_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboTermsOfPayment)
    End Sub
	
	'DC081200 changed from combo to dropdown
    'Private Sub ddTermsOfPayment_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles ddTermsOfPayment.KeyDown
    Private Sub ddTermsOfPayment_KeyDownEvent(ByVal eventSender As System.Object, ByVal eventArgs As PMListMgrDropdown.uctDropdown.KeyDownEventArgs)
        Dim KeyCode As Integer = eventArgs.KeyCode
        'Dim Shift As Integer = EventArgs.KeyData \ &H10000
        Dim Shift As Integer = eventArgs.KeyCode \ &H10000

        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)

    End Sub

    'DC081200 changed from combo to dropdown
    Private Sub cboTermsOfPayment_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboTermsOfPayment)
    End Sub

    Private Sub chkSmoker_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSmoker.Leave

        If lvwLifestyle.Items.Count > 0 Then
            If chkSmoker.CheckState = CheckState.Unchecked Then
                ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 6).Text = "No"
            Else
                ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 6).Text = "Yes"
            End If
        End If

    End Sub

    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click
        'RWH(11/07/2000) Altered to move PostCode from far left to far
        'right of ListView.


        Dim sTmp As String = ""

        Dim oListItem As ListViewItem

        Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20031002 : Bug Fix for PN Issue 6021. Restrict Special Characters in the
            '               ShortName and name field - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'PN10703 eck 030304
            '    If InStr(txtIDReference.Text, " ") > 0 Then
            '        MsgBox ("Blank Spaces are not Allowed in Client Code")
            '        TabMainTab.Tab = 0
            '        txtIDReference.SetFocus
            '        Exit Sub
            '    End If
            If txtIDReference.Text.IndexOf("'"c) >= 0 OrElse txtIDReference.Text.IndexOf("’"c) >= 0 Then
                MessageBox.Show("' (Apostrophes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Exit Sub
            End If
            If txtIDReference.Text.IndexOf("|"c) >= 0 Then
                MessageBox.Show("| (Pipes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Exit Sub
            End If
            If txtIDReference.Text.IndexOf(","c) >= 0 Then
                MessageBox.Show(", (Commas) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Exit Sub
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20031002 : Bug Fix for PN Issue 6021. Restrict Special Characters in the
            '               ShortName and name field - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

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

            'sj 23/07/2002 - start

            m_oAddress.CorrespondanceAddressExists = CorrespondanceAddressExists()
            'sj 23/07/2002 - end

            'sj 12/06/2002 - start

            m_oAddress.IsNRMA = m_bIsNRMA
            'sj 12/06/2002 - end

            'set the main postcode and reference

            m_oAddress.Reference = txtIDReference.Text
            '    m_lReturn& = ControlLostFocus(cboTermsOfPayment)


            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.Postcode = m_sMainPostCode
            'developer guide no. 162
            'm_oAddress.CountryID = gPMFunctions.ToSafeLong(m_vSourceArray(3, cboBranch.SelectedIndex + 1))
            m_oAddress.CountryID = gPMFunctions.ToSafeLong(m_vSourceArray(3, cboBranch.SelectedIndex))


            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Me.Refresh

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    ' Add the data to the list view


                    oListItem = lvwAddresses.Items.Add(m_oAddress.PostalCode, "AddressImage")
                    ' Address Usage

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
                    ' Address line 1

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1
                    ' Address line 2

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.address2
                    ' Address line 3

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3
                    ' Address line 4

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
                Case Else
                    ' Add the data to the list view


                    oListItem = lvwAddresses.Items.Add(m_oAddress.AddressUsageType, "AddressImage")
                    ' Address line 1

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1
                    ' Address line 2

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.address2
                    ' Address line 3

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3
                    ' Address line 4

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
                    ' Postcode

                    ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
            End Select


            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode

                m_iMainAddressIndex = m_oAddress.AddressUsageTypeId
                'Caption = "Corporate Client: " & m_sShortName & " " & m_sMainPostCode
            End If

            ' Store the Address_cnt

            oListItem.Tag = m_oAddress.addresscnt

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click


        Dim oListItem As ListViewItem

        Try

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20031002 : Bug Fix for PN Issue 6021. Restrict Special Characters in the
            '               ShortName and name field - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'PN11613
            '    If InStr(txtIDReference.Text, " ") > 0 Then
            '        MsgBox ("Blank Spaces are not Allowed in Client Code")
            '        TabMainTab.Tab = 0
            '        txtIDReference.SetFocus
            '        Exit Sub
            '    End If
            If txtIDReference.Text.IndexOf("'"c) >= 0 OrElse txtIDReference.Text.IndexOf("’"c) >= 0 Then
                MessageBox.Show("' (Apostrophes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Exit Sub
            End If
            If txtIDReference.Text.IndexOf("|"c) >= 0 Then
                MessageBox.Show("| (Pipes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Exit Sub
            End If
            If txtIDReference.Text.IndexOf(","c) >= 0 Then
                MessageBox.Show(", (Commas) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                txtIDReference.Focus()
                Exit Sub
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20031002 : Bug Fix for PN Issue 6021. Restrict Special Characters in the
            '               ShortName and name field - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

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

                    'RWH(08/06/2000) Tidy up object.
                    If Not (m_oContact Is Nothing) Then
                        m_oContact = Nothing
                    End If
                    Exit Sub

                End If

            End If

            'set the main postcode and reference


            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            m_oContact.Postcode = m_sMainPostCode


            m_lReturn = m_oContact.ContactCnt


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            Me.Refresh()



            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode, "ContactImage")

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

            'RWH(08/06/2000) Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RWH(08/06/2000) Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If
            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddCon_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 1
        SSTabHelper.SetSelectedIndex(TabMainTab, 1)

    End Sub

    Private Sub cmdAddConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddConv.Click


        Dim oListItem As ListViewItem

        Try

            'MSS210901 - Inserted UW code as it had more functionality
            'DC030401 do not allow if client not applied / saved
            If m_lPartyCnt < 1 Then
                'RWH(17/05/2001) Let's allow them to add convictions without having to exit
                'and come back in.
                If MessageBox.Show("Must Save Client Details before adding Convictions. Do you wish to save Client Details now ?", "Personal Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    m_lReturn = OKClick()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    'RWH(17/05/2001) If update was successful then we are now effectively in Edit mode.
                    m_iTask = gPMConstants.PMEComponentAction.PMEdit

                    m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                    m_sOldIdReference = m_sShortName

                    'RWH(18/05/2001) Re get everything to ensure we are fully initialised in edit mode.
                    m_lReturn = GetBusiness()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    m_lReturn = BusinessToInterface()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    m_lReturn = LockParty()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Else
                    Exit Sub
                End If
            End If
            'MSS210901 - Merge end

            'Create icontact if not already done so
            If m_oConviction Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oConviction As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oConviction, sClassName:="iPMBPartyConviction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oConviction = temp_m_oConviction

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get convictions", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oConviction.PartyCnt = m_lPartyCnt


            m_lReturn = m_oConviction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()



            oListItem = lvwConvictions.Items.Add(m_oConviction.Code, "ConvictionImage")

            ' Assign details to other the columns
            ' Column 2
            'Temporary thing

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oConviction.ConvictionDate

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oConviction.Description

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oConviction.FineAmount

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oConviction.StatusCode

            ' Store the Address_cnt

            oListItem.Tag = m_oConviction.PartyConvictionID

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddLife_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddLife.Click


        Dim oListItem As ListViewItem
        Dim vCategory, sSmoker As String

        If m_lPartyCnt < 1 Then
            'eck 090903 PN6645
            MessageBox.Show("Must Save Client Details before adding Dependants", "Personal Client")

            Exit Sub
        End If

        Try

            'Create icontact if not already done so
            If m_oLifestyle Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oLifestyle As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLifestyle, sClassName:="iPMBLifestyle.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLifestyle = temp_m_oLifestyle

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get lifestyles", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddLife_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If



            m_lReturn = m_oLifestyle.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oLifestyle.PartyCnt = m_lPartyCnt


            m_lReturn = m_oLifestyle.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oLifestyle.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()

            ' Column 1

            GetPersonLookups(vCategoryId:=m_oLifestyle.category, vCategoryDescription:=vCategory)

            oListItem = lvwLifestyle.Items.Add(vCategory.Trim(), "")

            ' Assign details to other the columns
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oLifestyle.PartyName

            ' Column 3
            'DC 20/10/00 check that is not Null (i.e. 29/12/1899)
            'NS 08/04/2002 - F0031417 - Force date to specific format to cater
            'for different regional date settings.
            'If m_oLifestyle.DateOfBirth = "29/12/1899" Then

            'developer guide no. 
            If String.Format("{0:dd/MM/yyyy}", m_oLifestyle.DateOfBirth) = "29/12/1899" Then
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ""
            Else
                'GSD 05082002

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = DateTime.Parse(m_oLifestyle.DateOfBirth).ToString("D")
            End If
            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oLifestyle.GenderCode

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oLifestyle.OccupationCode

            ' Column 6

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oLifestyle.SecondaryOccupationCode

            ' Column 7
            'DJM 11/11/2003 : Display smoker column correctly (either Yes or No).
            sSmoker = "No"

            'developer guide no. 101
            If m_oLifestyle.IsSmoker = 1 Then
                sSmoker = "Yes"
            End If

            ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sSmoker.Trim()

            ' Set the tag property with the index of
            ' the search data storage.
            ' Store the Lifestyle_id

            oListItem.Tag = m_oLifestyle.PartyLifestyleID

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddLife_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddLife_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdAddLife_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddLife.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 4
        SSTabHelper.SetSelectedIndex(TabMainTab, 4)

    End Sub

    Private Sub cmdAddLoyaltyScheme_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddLoyaltyScheme.Click 'RAW 18/11/2002 : PS005 : Added


        Dim oListItem As ListViewItem

        Try

            If m_lPartyCnt < 1 Then
                ' This is a new party that has not yet been added to the database so no party_cnt has been generated

                'Let's allow them to add PartyLoyaltySchemes without having to exit
                'and come back in.
                If MessageBox.Show("Must Save Client Details before adding Loyalty Schemes. Do you wish to save Client Details now ?", "Personal Client", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1) = System.Windows.Forms.DialogResult.Yes Then

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                    m_lReturn = OKClick()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    'If update was successful then we are now effectively in Edit mode.
                    m_iTask = gPMConstants.PMEComponentAction.PMEdit

                    m_lReturn = m_oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                    m_sOldIdReference = m_sShortName

                    'Re get everything to ensure we are fully initialised in edit mode.
                    m_lReturn = GetBusiness()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    m_lReturn = BusinessToInterface()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    m_lReturn = LockParty()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Else
                    Exit Sub
                End If
            End If

            'Create icontact if not already done so
            If m_oPartyLoyaltyScheme Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oPartyLoyaltyScheme As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyLoyaltyScheme, sClassName:="iPMBPartyLoyaltyScheme.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oPartyLoyaltyScheme = temp_m_oPartyLoyaltyScheme

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get PartyLoyaltySchemes", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oPartyLoyaltyScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oPartyLoyaltyScheme.PartyCnt = m_lPartyCnt


            m_lReturn = m_oPartyLoyaltyScheme.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oPartyLoyaltyScheme.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()


            oListItem = lvwLoyaltySchemes.Items.Add(m_oPartyLoyaltyScheme.LoyaltySchemeName)

            ' Assign details to other the columns
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oPartyLoyaltyScheme.MemberNumber

            ' Column 3

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_oPartyLoyaltyScheme.StartDate)

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtDate.Text.Trim()

            ' Column 4

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_oPartyLoyaltyScheme.EndDate)

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDate.Text.Trim()

            ' Store the PartyLoyaltySchemeId

            oListItem.Tag = m_oPartyLoyaltyScheme.PartyLoyaltySchemeId
            'developer guide no. 131
            If Not lvwLoyaltySchemes.FocusedItem Is Nothing Then
                lvwLoyaltySchemes.FocusedItem.Selected = False
            End If


        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddLoyaltyScheme_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddLoyaltyScheme_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddLoyaltyScheme.Enter 'RAW 18/11/2002 : PS005 : Added

        'If we're here by tabbing (or back-tabbing) we're on tab 5
        'DN 12/12/02 - Only focus to property if tab is visible
        If SSTabHelper.GetTabVisible(TabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(TabMainTab, 5)
        End If

    End Sub

    Private Sub cmdAgentLookUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentLookUp.Click

        Dim vCnt, vShortName, vName As Object
        Dim dDefaultDate As Date
        Dim vDateCancelled As Object

        Try
            'developer guide no. 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt,
                            vShortName:=vShortName,
                            vName:=vName,
                            vSpecialParty:="AG",
                            vDateCancelled:=vDateCancelled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' PWF 15/08/2002 - Just exit, leave original agent!
                Exit Sub
            End If

            dDefaultDate = CDate("01/01/1900")


            If (CDate(vDateCancelled) <= DateTime.Today) And (CDate(vDateCancelled) >= dDefaultDate) Then
                MessageBox.Show("Agency cancelled - no new transactions can be placed through this agent", "Agency Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If


            'save the count in the tag and update controls

            txtAgentRef.Tag = CStr(vCnt)


            m_sAgentRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)


            m_sAgentName = CStr(vName)
            pnlAgentName.Text = m_sAgentName

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyAgentCnt = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' DC 03/05/00
    ' Cater for more than one Associate
    ' This routine no longer required
    'Private Sub cmdAssociateLookup_Click()
    '
    'Dim vCnt As Variant
    'Dim vShortName As Variant
    'Dim vName As Variant
    '
    '
    '    On Error GoTo Err_cmdAssociateLookUp_Click
    '
    '    m_lReturn& = SelectParty(vPartyCnt:=vCnt, _
    ''                            vShortName:=vShortName, _
    ''                            vName:=vName)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '
    '        ' CF 111199 - Check for cancel action
    '        If (m_lReturn& = PMCancel) Then
    '            vCnt = ""
    '            vShortName = ""
    '            vName = ""
    '        Else
    '            Exit Sub
    '        End If
    '
    '    End If
    '
    '    'save the count in the tag and update controls
    '    txtAssociateRef.Tag = CStr(vCnt)
    '    m_lAssociatedCnt = CLng(vCnt)
    '    m_sAssociatedName = CStr(vShortName)
    '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtAssociateRef, _
    ''                                            vControlValue:=m_sAssociatedName)
    '
    '    Exit Sub
    '
    'Err_cmdAssociateLookUp_Click:
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:=PMErrorText, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="cmdAssociateLookUp_Click", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    Private Sub cmdBrokerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrokerLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try
            'developer guide no. 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt,
                            vShortName:=vShortName,
                            vName:=vName,
                            vSpecialParty:="BR")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtBrokerRef.Tag = CStr(vCnt)


            m_sBrokerRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBrokerRef, vControlValue:=m_sBrokerRef)


            m_sBrokerName = CStr(vName)

            sTemp = m_sBrokerName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")
            pnlBrokerName.Text = sTemp
            m_bChangedProspect = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdBrokerLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdConsultantLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConsultantLookup.Click

        Dim vCnt, vShortName, vName, vResolvedName As Object
        'Dim vName, vResolvedName As String 'CT 19/07/00

        Try
            'developer guide no. 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt,
                            vShortName:=vShortName,
                            vName:=vName,
                            vSpecialParty:="CO",
                            vResolvedName:=vResolvedName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' CF 111199 - Check for cancel action
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    vCnt = ""
                    vShortName = ""
                    vName = ""
                    Exit Sub 'VB 24/10/2005 PN3055
                Else
                    Exit Sub
                End If

            End If
            If IsNothing(txtConsultantRef.Tag) = False Then
                If vCnt <> Convert.ToString(txtConsultantRef.Tag) Then
                    m_bIsAmended = True
                End If
            End If
            

            'save the count in the tag and update controls
            txtConsultantRef.Tag = vCnt

            m_sConsultantRef = vShortName
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)

            'm_sConsultantName$ = CStr(vName)
            m_sConsultantName = vResolvedName 'CT 19/07/00
            PnlConsultantName.Text = m_sConsultantName

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyConsultantCnt = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdConsultantLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    Private Sub cmdCurrentAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCurrentAgent.Click


        Dim vCnt As Object = Nothing
        Dim vShortName As Object = Nothing
        Dim vName As Object = Nothing
        Dim sTemp As String

        Try
            'developer guide no. 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt,
                            vShortName:=vShortName,
                            vName:=vName,
                            vSpecialParty:="AG")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' CF 111199 - Check for cancel action
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    vCnt = ""
                    vShortName = ""
                    vName = ""
                    'sj 30/08/2002 - start
                    Exit Sub
                    'sj 30/08/2002 - end
                Else
                    Exit Sub
                End If
            End If

            m_lCurrentAgent = CInt(vCnt)

            m_sCurrentAgentRef = vShortName

            m_sCurrentAgentName = vName

            'MKR 11/10/2004 PN 15599 Doubling the & character to avoid displaying "_"
            sTemp = m_sCurrentAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")
            pnlCurrentAgent.Text = sTemp

            'MKR 11/10/2004 PN 15598 Setting changed prospect flag to true...
            m_bChangedProspect = True

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyCurrentAgentCnt = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCurrentAgent_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click





        Try

            'Set row to be deleted - if a valid one selected
            If lvwAddresses.Items.Count < 1 Then
                Exit Sub
            End If

            'sj 23/07/2002 - start
            'Do not allow user to delete correspondance address
            If IsCorrespondanceAddress() Then
                MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Sub
            End If
            'sj 23/07/2002 - end
            'sj 23/07/2002 - start
            'Removed this in line with Group and Corporate client maintenance
            '    'Create address component if not already done so
            '    If ((m_oAddress Is Nothing) = True) Then
            '
            '        ' Get an instance of the contact interface object via
            '        ' the public object manager.
            '        m_lReturn& = g_oObjectManager.GetInstance( _
            ''            oObject:=m_oAddress, _
            ''            sClassName:="iPMBAddress.Interface", _
            ''            vInstanceManager:=PMGetLocalInterface)
            '
            '        ' Check for errors.
            '        If (m_lReturn& <> PMTrue) Then
            '            ' Log Error Message
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="Failed to get address component", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="cmdEditAd_Click", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '
            '            Exit Sub
            '
            '        End If
            '
            '    End If
            '
            '    'If we send PMView instead, everything is ok
            '    'm_lReturn& = m_oAddress.SetProcessModes(vTask:=PMDelete)
            '    m_lReturn& = m_oAddress.SetProcessModes(vTask:=PMView)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            '
            '    Set oListItem = lvwAddresses.SelectedItem
            '    'set the address id
            '    m_oAddress.addresscnt = oListItem.Tag
            '
            '    For k = LBound(m_vAddressTypes, 2) To UBound(m_vAddressTypes, 2)
            '        If oListItem.SubItems(1) = m_vAddressTypes(1, k) Then
            '            m_oAddress.AddressUsageTypeId = m_vAddressTypes(0, k)
            '            Exit For
            '        End If
            '    Next k
            '
            '    'm_oAddress.addresscnt = m_lAddressCnt
            '    'm_oAddress.AddressUsageTypeId = m_lAddressUsageTypeID
            '
            '    m_lReturn& = m_oAddress.Start()
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            '
            '    'If not cancelled, edit grid
            '    If (m_oAddress.Status = PMCancel) Then
            '        Exit Sub
            '    End If
            'sj 23/07/2002 - end

            'Update the address details
            ' & postcode

            'Reset Interface

            cmdEditAd.Enabled = False
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

                    'RWH(08/06/2000) Tidy up object.
                    If Not (m_oContact Is Nothing) Then
                        m_oContact = Nothing
                    End If
                    Exit Sub

                End If

            End If

            'Use pmview instead
            'm_lReturn& = m_oContact.SetProcessModes(vTask:=PMDelete)

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'set the contact id


            m_oContact.ContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
            'm_oContact.ContactCnt = m_lContactCnt&


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            lvwContacts.Items.RemoveAt(lvwContacts.FocusedItem.Index)

            lvwContacts.Focus()

            'RWH(08/06/2000) Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RWH(08/06/2000) Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If
            Exit Sub

        End Try
    End Sub

    Private Sub cmdDeleteConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteConv.Click



        Try

            'Set row to be deleted - if a valid one selected
            If lvwConvictions.Items.Count < 1 Then
                Exit Sub
            End If

            'Create conviction component if not already done so
            If m_oConviction Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oConviction As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oConviction, sClassName:="iPMBPartyConviction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oConviction = temp_m_oConviction

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get conviction component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the various ids

            m_oConviction.PartyCnt = m_lPartyCnt



            m_oConviction.PartyConvictionID = Convert.ToString(lvwConvictions.FocusedItem.Tag)


            m_lReturn = m_oConviction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditConv.Enabled = False
            cmdDeleteConv.Enabled = False

            lvwConvictions.Items.RemoveAt(lvwConvictions.FocusedItem.Index)

            lvwConvictions.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteLife_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteLife.Click



        Try

            'Set row to be deleted - if a valid one selected
            If lvwLifestyle.Items.Count < 1 Then
                Exit Sub
            End If

            If lvwLifestyle.Items.Count = 1 Then
                MessageBox.Show("You cannot delete Insured Details", Application.ProductName)
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oLifestyle Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oLifestyle As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLifestyle, sClassName:="iPMBLifestyle.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLifestyle = temp_m_oLifestyle

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get lifestyle component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteLife_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'User PMView instead
            'm_lReturn& = m_oLifestyle.SetProcessModes(vTask:=PMDelete)

            m_lReturn = m_oLifestyle.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the id

            m_oLifestyle.PartyCnt = m_lPartyCnt



            m_oLifestyle.PartyLifestyleID = Convert.ToString(lvwLifestyle.FocusedItem.Tag)


            m_lReturn = m_oLifestyle.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oLifestyle.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If


            'Reset Interface
            cmdEditLife.Enabled = False
            cmdDeleteLife.Enabled = False

            lvwLifestyle.Items.RemoveAt(lvwLifestyle.FocusedItem.Index)

            lvwLifestyle.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteLife_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteLife_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteLoyaltyScheme_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteLoyaltyScheme.Click 'RAW 18/11/2002 : PS005 : Added


        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            'Set row to be deleted - if a valid one selected
            If lvwLoyaltySchemes.FocusedItem.Index + 1 < 1 Then
                Exit Sub
            End If

            'Ensure that delete should proceed

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirmDeleteTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirmDeleteDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            If iMsgResult = System.Windows.Forms.DialogResult.No Then
                Exit Sub
            End If

            ' OK so we really do want to delete

            'Create PartyLoyaltyScheme component if not already done so
            If m_oPartyLoyaltyScheme Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oPartyLoyaltyScheme As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyLoyaltyScheme, sClassName:="iPMBPartyLoyaltyScheme.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oPartyLoyaltyScheme = temp_m_oPartyLoyaltyScheme

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PartyLoyaltyScheme component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oPartyLoyaltyScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMDelete)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the various ids

            m_oPartyLoyaltyScheme.PartyCnt = m_lPartyCnt



            m_oPartyLoyaltyScheme.PartyLoyaltySchemeId = Convert.ToString(lvwLoyaltySchemes.FocusedItem.Tag)


            m_lReturn = m_oPartyLoyaltyScheme.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            'TODO: MILAN:: The following code renders the delete functionality useless. As the status is always cancel, thus the item does not gets removed from listview
            'If m_oPartyLoyaltyScheme.Status = gPMConstants.PMEReturnCode.PMCancel Then
            '    Exit Sub
            'End If

            'Reset Interface
            cmdEditLoyaltyScheme.Enabled = False
            cmdDeleteLoyaltyScheme.Enabled = False

            lvwLoyaltySchemes.Items.RemoveAt(lvwLoyaltySchemes.FocusedItem.Index)

            lvwLoyaltySchemes.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteLoyaltyScheme_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click
        'RWH(11/07/2000) Altered to move PostCode from far left to far
        'right of ListView.


        Dim sTmp As String = ""
        Dim oListItem As ListViewItem

        Dim sAddressUsage As String = ""

        Try

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

            'sj 12/06/2002 - start

            m_oAddress.IsNRMA = m_bIsNRMA
            'sj 12/06/2002 - end

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges And IsCorrespondanceAddress() Then

                m_oAddress.FutureDateAddressChanges = True


                m_oAddress.FutureDatedAddresses = m_vFutureDatedAddresses
            Else

                m_oAddress.FutureDateAddressChanges = False

                'developer guide no.17
                m_oAddress.FutureDatedAddresses = Nothing

            End If
            'sj 23/07/2002 - end


            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            m_oAddress.Reference = txtIDReference.Text
            'developer guide no. 218
            m_oAddress.PostalCode = m_sMainPostCode

            oListItem = lvwAddresses.FocusedItem
            'set the address id

            'developer guide no. 218
            m_oAddress.AddressCnt = Convert.ToString(oListItem.Tag)

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"

                    'developer guide no. 218
                    m_oAddress.PostalCode = oListItem.Text
                    sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
                Case Else

                    'developer guide no. 218
                    m_oAddress.PostalCode = ListViewHelper.GetListViewSubItem(oListItem, 5).Text
                    sAddressUsage = oListItem.Text
            End Select

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If sAddressUsage = CStr(m_vAddressTypes(1, k)) Then

                    'developer guide no. 218
                    m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            'DJM 03/04/2002 : Moved following line from listview click event to prevent
            'conflict with convictions edit button which also uses m_iLine.
            m_iLine = lvwAddresses.FocusedItem.Index + 1


            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges And IsCorrespondanceAddress() Then


                m_vFutureDatedAddresses = m_oAddress.FutureDatedAddresses
            End If
            'sj 23/07/2002 - end

            'Reset Interface
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False

            oListItem = lvwAddresses.Items.Item(m_iLine - 1)

            With oListItem
                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Postcode

                        .Text = m_oAddress.PostalCode
                        ' Address usage type

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1

                        'developer guide no. 218
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
                    Case Else
                        ' Address usage type

                        .Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1

                        'developer guide no. 218
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
                        'Postcode

                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
                End Select


                If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                    m_sMainPostCode = m_oAddress.PostalCode
                    'Caption = "Corporate Client: " & m_sShortName & " " & m_sMainPostCode
                End If
                'EK 15/11/99 update the tag
                ' Store the Address_cnt


                'developer guide no. 218
                .Tag = m_oAddress.AddressCnt
            End With

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: CorrespondanceAddressExists
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function CorrespondanceAddressExists() As Boolean

        Dim result As Boolean = False
        Try



            For Each oListItem As ListViewItem In lvwAddresses.Items
                If IsCorrespondanceAddress(oListItem) Then
                    Return True
                End If
            Next oListItem

            Return result

        Catch excep As System.Exception



            result = False

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CorrespondanceAddressExists Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CorrespondanceAddressExists", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsCorrespondanceAddress
    '
    ' Description:
    '
    ' History: 23/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function IsCorrespondanceAddress(Optional ByVal v_vListItem As ListViewItem = Nothing) As Boolean

        Dim result As Boolean = False
        Try
            Dim oListItem As ListViewItem
            Dim sAddressUsageType As String = ""

            If Not Information.IsNothing(v_vListItem) Then
                oListItem = v_vListItem
            Else
                oListItem = lvwAddresses.FocusedItem
            End If

            With oListItem
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsageType = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
                    Case Else
                        sAddressUsageType = .Text
                End Select
            End With
            If sAddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then
                result = True
            End If
            Return result

        Catch excep As System.Exception



            result = False

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsCorrespondanceAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsCorrespondanceAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click



        Dim oListItem As ListViewItem

        Try

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

                    'RWH(08/06/2000) Tidy up object.
                    If Not (m_oContact Is Nothing) Then
                        m_oContact = Nothing
                    End If
                    Exit Sub

                End If

            End If


            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            'developer guide no. 218
            m_oContact.PostCode = m_sMainPostCode

            'set the contact id
            oListItem = lvwContacts.FocusedItem



            m_oContact.ContactCnt = Convert.ToString(oListItem.Tag)


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RWH(08/06/2000) Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            'Set oListItem = lvwContacts.ListItems.Item(m_iLine)


            oListItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description

            'RWH(08/06/2000) Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RWH(08/06/2000) Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If
            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditCon_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Enter

        'If we're here via tabbing (or back-tabbing) we're on tab 1
        SSTabHelper.SetSelectedIndex(TabMainTab, 1)

    End Sub

    Private Sub cmdEditConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditConv.Click


        Dim oListItem As ListViewItem
        Dim vCategory As String = ""

        If m_lPartyCnt < 1 Then
            MessageBox.Show("Must Save Client Details before editing Convictions", Application.ProductName)
        End If

        Try

            'Set row to be edited - if a valid one selected
            If lvwConvictions.Items.Count < 1 Then
                MessageBox.Show("You must maintain Insured Details from the main screen", Application.ProductName)
                Exit Sub
            End If

            'Create conviction component if not already done so
            If m_oConviction Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oConviction As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oConviction, sClassName:="iPMBPartyConviction.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oConviction = temp_m_oConviction

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get conviction component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the Ids

            m_oConviction.PartyCnt = m_lPartyCnt



            m_oConviction.PartyConvictionID = Convert.ToString(lvwConvictions.FocusedItem.Tag)

            m_iLine = lvwConvictions.FocusedItem.Index + 1


            m_lReturn = m_oConviction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditConv.Enabled = False
            cmdDeleteConv.Enabled = False
            'EK 12/12 Fixed to match data/headings

            oListItem = lvwConvictions.Items.Item(m_iLine - 1)

            ' Assign details to other the columns
            ' Column 1

            oListItem.Text = m_oConviction.Code

            'Conviction Date

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_oConviction.ConvictionDate.Trim())
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CDate(txtDate.Text).ToString("dd MMMM yyyy")

            ' Description

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oConviction.Description.Trim()

            ' Fine

            m_lReturn = m_oFormFields.FormatControl(txtCurrency, ReflectionHelper.GetMember(m_oConviction, "FineAmount"))

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtCurrency.Text.Trim()

            ' Conviction Status

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oConviction.StatusCode

            ' Penalty Points

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oConviction.DrivingLicencePenaltyPoints

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditLife_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditLife.Click


        Dim oListItem As ListViewItem
        Dim vCategory, sSmoker As String

        If m_lPartyCnt < 1 Then
            MessageBox.Show("Must Save Client Details before editing Dependants", "Personal Client")
            Exit Sub
        End If

        Try

            'Set row to be edited - if a valid one selected
            If lvwLifestyle.Items.Count = 1 Then
                MessageBox.Show("You must maintain Insured Details from the main screen", "Personal Client")
                Exit Sub
            End If

            If lvwLifestyle.FocusedItem.Index + 1 = 1 Then
                MessageBox.Show("You must maintain Insured Details from the main screen", "Personal Client")
                Exit Sub
            End If

            'Create lifestyle component if not already done so
            If m_oLifestyle Is Nothing Then

                ' Get an instance of the lifestyle interface object via
                ' the public object manager.
                Dim temp_m_oLifestyle As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLifestyle, sClassName:="iPMBLifestyle.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oLifestyle = temp_m_oLifestyle

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get lifestyle component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLife_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oLifestyle.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            'set the Ids

            m_oLifestyle.PartyCnt = m_lPartyCnt

            oListItem = lvwLifestyle.FocusedItem



            m_oLifestyle.PartyLifestyleID = Convert.ToString(oListItem.Tag)


            m_lReturn = m_oLifestyle.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oLifestyle.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditLife.Enabled = False
            cmdDeleteLife.Enabled = False

            'Set oListItem = lvwLifestyle.ListItems.Item(m_iLine)


            GetPersonLookups(vCategoryId:=m_oLifestyle.category, vCategoryDescription:=vCategory)

            oListItem.Text = vCategory.Trim()
            ' Assign details to other the columns
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oLifestyle.PartyName

            ' Column 3
            'DC 20/10/00 check for Null (i.e. 29/12/1899)
            'NS 08/04/2002 - F0031417 - Force date to specific format to cater
            'for different regional date settings.
            'If m_oLifestyle.DateOfBirth = "29/12/1899" Then

            'developer guide no. 
            If String.Format("{0:dd/MM/yyyy}", m_oLifestyle.DateOfBirth) = "29/12/1899" Then
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ""
            Else
                'GSD 05082002

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = DateTime.Parse(m_oLifestyle.DateOfBirth).ToString("D")
            End If
            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oLifestyle.GenderCode

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oLifestyle.OccupationCode

            ' Column 6

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oLifestyle.SecondaryOccupationCode

            ' Column 7
            'DJM 11/11/2003 : Display smoker column correctly (either Yes or No).
            sSmoker = "No"

            'If m_oLifestyle.IsSmoker <> "" Then

            If m_oLifestyle.IsSmoker = 1 Then
                sSmoker = "Yes"
            End If
            'End If

            ListViewHelper.GetListViewSubItem(oListItem, 6).Text = sSmoker.Trim()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditLife_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLife_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditLife_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditLife.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 4
        SSTabHelper.SetSelectedIndex(TabMainTab, 4)

    End Sub

    Private Sub cmdEditLoyaltyScheme_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditLoyaltyScheme.Click 'RAW 18/11/2002 : PS005 : Added


        Dim oListItem As ListViewItem
        Dim vCategory As String = ""

        Try

            If m_lPartyCnt < 1 Then
                ' This is a new party that has not yet been added to the database so no party_cnt has been generated

                ' ?? will this ever happen since party was forced to be saved when
                ' adding the first loyalty scheme and edit is only enabled when an
                ' entry has been selected from the list view
                MessageBox.Show("Must Save Client Details before editing PartyLoyaltySchemes", Application.ProductName)
            End If

            'Set row to be edited - if a valid one selected
            If lvwLoyaltySchemes.FocusedItem.Index + 1 < 1 Then
                Exit Sub
            End If

            'Create PartyLoyaltyScheme component if not already done so
            If m_oPartyLoyaltyScheme Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oPartyLoyaltyScheme As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPartyLoyaltyScheme, sClassName:="iPMBPartyLoyaltyScheme.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oPartyLoyaltyScheme = temp_m_oPartyLoyaltyScheme

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PartyLoyaltyScheme component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oPartyLoyaltyScheme.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the Ids

            m_oPartyLoyaltyScheme.PartyCnt = m_lPartyCnt



            m_oPartyLoyaltyScheme.PartyLoyaltySchemeId = Convert.ToString(lvwLoyaltySchemes.FocusedItem.Tag)

            m_iLine = lvwLoyaltySchemes.FocusedItem.Index + 1


            m_lReturn = m_oPartyLoyaltyScheme.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oPartyLoyaltyScheme.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditLoyaltyScheme.Enabled = False
            cmdDeleteLoyaltyScheme.Enabled = False

            oListItem = lvwLoyaltySchemes.Items.Item(m_iLine - 1)

            ' Assign details to other the columns
            ' Column 1

            oListItem.Text = m_oPartyLoyaltyScheme.LoyaltySchemeName

            ' Assign details to other the columns
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oPartyLoyaltyScheme.MemberNumber

            ' Column 3

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_oPartyLoyaltyScheme.StartDate)

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtDate.Text.Trim()

            ' Column 4

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_oPartyLoyaltyScheme.EndDate)

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtDate.Text.Trim()

            ' Store the PartyLoyaltySchemeId

            oListItem.Tag = m_oPartyLoyaltyScheme.PartyLoyaltySchemeId

            lvwLoyaltySchemes.FocusedItem.Selected = False

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditLoyaltyScheme_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditLoyaltyScheme_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditLoyaltyScheme_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditLoyaltyScheme.Enter 'RAW 18/11/2002 : PS005 : Added

        'If we're here by tabbing (or back-tabbing) we're on tab 6
        'DN 12/12/02 - Only focus to property if tab is visible
        If SSTabHelper.GetTabVisible(TabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(TabMainTab, 5)
        End If

    End Sub


    Private Sub cmdInsurerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurerLookup.Click
        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try
            'developer guide no. 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt,
                                     vShortName:=vShortName,
                                     vName:=vName,
                                     vSpecialParty:="IN")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtInsurerRef.Tag = CStr(vCnt)


            m_sInsurerRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInsurerRef, vControlValue:=m_sInsurerRef)


            m_sInsurerName = CStr(vName)

            sTemp = m_sInsurerName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")
            pnlInsurerName.Text = sTemp
            m_bChangedProspect = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdInsurerLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_7.Click, _cmdNext_6.Click, _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_0.Click, _cmdNext_1.Click, _cmdNext_2.Click, _cmdNext_3.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(TabMainTab) < SSTabHelper.GetTabCount(TabMainTab) - 1 Then
                Do While Not SSTabHelper.GetTabVisible(TabMainTab, Index + 1)
                    Index += 1
                Loop
                SSTabHelper.SetSelectedIndex(TabMainTab, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(TabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_7.Click, _cmdPrevious_6.Click, _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_3.Click, _cmdPrevious_0.Click, _cmdPrevious_1.Click, _cmdPrevious_2.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(TabMainTab) > 0 Then
                Do While Not SSTabHelper.GetTabVisible(TabMainTab, Index)
                    Index -= 1
                Loop
                'The previous button has index 0 on tab 1, etc
                'TabMainTab.Tab = Index - 1
                SSTabHelper.SetSelectedIndex(TabMainTab, Index)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(TabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                'm_ctlTabFirstLast(ACControlStart, Index + 1).SetFocus
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub
    'developer guide no. 
    'start'
    Private Sub ddBusiness_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddBusiness.Leave
        m_lReturn = ValidateListField(ddBusiness)
    End Sub

    Private Sub ddBusiness_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddBusiness.Enter
        If ddBusiness.Text = "" Then
            m_lReturn = HighlightContol(ddBusiness, , True)
        End If
    End Sub
    Private Sub ddEmployment_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddEmployment.Enter
        If ddEmployment.Text = "" Then
            m_lReturn = HighlightContol(ddEmployment, , True)
        End If
    End Sub

    Private Sub ddEmployment_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddEmployment.Leave
        m_lReturn = ValidateListField(ddEmployment)
    End Sub

    Private Sub ddGender_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddGender.Enter
        If ddGender.Text = "" Then
            m_lReturn = HighlightContol(ddGender, , True)
        End If
    End Sub

    Private Sub ddGender_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddGender.Leave
        m_lReturn = ValidateListField(ddGender)

        If lvwLifestyle.Items.Count > 0 Then
            ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 3).Text = ddGender.Text
        End If
    End Sub
    'End'

    Private Sub ddMaritalStatus_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddMaritalStatus.Leave
        m_lReturn = ValidateListField(ddMaritalStatus)
    End Sub

    Private Sub ddMaritalStatus_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddMaritalStatus.Enter
        If ddMaritalStatus.Text = "" Then
            m_lReturn = HighlightContol(ddMaritalStatus, , True)
        End If
    End Sub

    Private Sub ddOccupation_Change(ByVal Sender As Object, ByVal e As System.EventArgs) Handles ddOccupation.Change

    End Sub

    Private Sub ddOccupation_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddOccupation.LostFocus

        m_lReturn = ValidateListField(ddOccupation)
        'developer guide no. 131
        If ddOccupation.Text.Trim <> "" Then 'start if'
            If lvwLifestyle.Items.Count > 0 Then
                ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 4).Text = ddOccupation.Text
            End If 'end'
        End If
    End Sub
    Private Sub ddOccupation_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddOccupation.Enter
        If ddOccupation.Text = "" Then
            m_lReturn = HighlightContol(ddOccupation, , True)
        End If
    End Sub
    'end'

    Private Sub ddOccupation_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddOccupation.Leave
        m_lReturn = ValidateListField(ddOccupation)
        'developer guide no. 131
        If ddOccupation.Text.Trim <> "" Then 'start if'
            If lvwLifestyle.Items.Count > 0 Then
                ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 4).Text = ddOccupation.Text
            End If 'end'
        End If
    End Sub

    Private Sub ddPaymentMethod_Change(ByVal Sender As Object, ByVal e As System.EventArgs) Handles ddPaymentMethod.Change
        If (ddPaymentMethod.Text = CreditCard) Or (ddPaymentMethod.Text = DebitCard) Then
            cboCreditCard.Text = m_sCreditCardCode
            lblCreditCard.Visible = True
            cboCreditCard.Visible = True
        Else
            lblCreditCard.Visible = False
            cboCreditCard.Visible = False
            cboCreditCard.Text = ""
        End If
    End Sub

    Private Sub ddPaymentMethod_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddPaymentMethod.Click

      

    End Sub
    Private Sub ddPaymentMethod_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddPaymentMethod.Enter
        If ddPaymentMethod.Text = "" Then
            m_lReturn = HighlightContol(ddPaymentMethod, , True)
        End If
    End Sub
    'developer guide no. 
    Private Sub ddPaymentMethod_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddPaymentMethod.Leave
        m_lReturn = ValidateListField(ddPaymentMethod)
    End Sub
    Private Sub ddSecEmploymentStatus_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecEmploymentStatus.Enter
        'If we're here by tabbing (or back-tabbing) we're on tab 2
        SSTabHelper.SetSelectedIndex(TabMainTab, 2)

        If ddSecEmploymentStatus.Text = "" Then
            m_lReturn = HighlightContol(ddSecEmploymentStatus, , True)
        End If
    End Sub

    Private Sub ddSecEmploymentStatus_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecEmploymentStatus.Leave
        m_lReturn = ValidateListField(ddSecEmploymentStatus)
    End Sub

    Private Sub ddSecondaryBusiness_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecondaryBusiness.Leave
        m_lReturn = ValidateListField(ddSecondaryBusiness)
    End Sub

    Private Sub ddSecondaryBusiness_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecondaryBusiness.Enter
        If ddSecondaryBusiness.Text = "" Then
            m_lReturn = HighlightContol(ddSecondaryBusiness, , True)
        End If
    End Sub
    Private Sub ddSecondaryOccupation_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecondaryOccupation.Enter
        If ddSecondaryOccupation.Text = "" Then
            m_lReturn = HighlightContol(ddSecondaryOccupation, , True)
        End If
    End Sub

    Private Sub ddSecondaryOccupation_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecondaryOccupation.Leave
        m_lReturn = ValidateListField(ddSecondaryOccupation)
        'developer guide no. 131
        'start if'
        If ddSecondaryOccupation.Text.Trim <> "" Then
            If lvwLifestyle.Items.Count > 0 Then
                ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 5).Text = ddSecondaryOccupation.Text
            End If
        End If 'end'
    End Sub
    Private Sub ddTitle_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddTitle.Enter
        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        SSTabHelper.SetSelectedIndex(TabMainTab, 0)

        If ddTitle.Text = "" Then
            m_lReturn = HighlightContol(ddTitle, , True)
        End If
    End Sub
    'developer guide no. 
    Private Sub ddTitle_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddTitle.Leave
        m_lReturn = GIIFunctions.ValidateListField(ddTitle)
    End Sub


    'DJM 03/04/2002 : Don't change m_iLine here. I have moved it to cmdEditAd_Click.
    'Private Sub lvwAddresses_Click()
    '
    ''MSS210901 - Added UW code as it had better checks
    '    'm_iLine = lvwAddresses.SelectedItem.Index
    '
    '    'RWH(12/07/2000) Added this 'cos m_iLine was being used in cmdEditAd_Click but
    '    'it was not being set for Addresses.
    '    If (lvwAddresses.ListItems.Count > 0) Then
    '
    '        m_lAddressCnt& = lvwAddresses.SelectedItem.Tag
    '        m_iLine% = lvwAddresses.SelectedItem.Index
    '
    '    End If
    ''MSS210901 - Merge end
    '
    'End Sub

    Private Sub lvwAddresses_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Enter

        'If we're here via tabbing, we're on tab 1
        SSTabHelper.SetSelectedIndex(TabMainTab, 1)

    End Sub

    Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        If lvwAddresses.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditAd.Enabled = True
                cmdDeleteAd.Enabled = True
            End If
        End If

    End Sub


    Private Sub lvwCampaigns_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwCampaigns.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwCampaigns.Columns(eventArgs.Column)

        ' Column click event for the campaigns

        Try

            With lvwCampaigns
                ' If current sort column header is
                ' pressed.

                If ListViewHelper.GetSortKeyProperty(lvwCampaigns) = 2 Then
                    ListViewHelper.SetSortKeyProperty(lvwCampaigns, 1)
                End If

                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwCampaigns) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwCampaigns, (ListViewHelper.GetSortOrderProperty(lvwCampaigns) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwCampaigns, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwCampaigns, SortOrder.Ascending)
                    If ColumnHeader.Index + 1 = 2 Then
                        ListViewHelper.SetSortKeyProperty(lvwCampaigns, 2)
                    Else
                        ListViewHelper.SetSortKeyProperty(lvwCampaigns, ColumnHeader.Index + 1 - 1)
                    End If
                    ListViewHelper.SetSortedProperty(lvwCampaigns, True)
                End If
            End With

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwCampaigns_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwContacts.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditCon.Enabled = True
                cmdDeleteCon.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwConvictions_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwConvictions.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 3
        'DN 12/12/02 - Only focus to property if tab is visible
        If SSTabHelper.GetTabVisible(TabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(TabMainTab, 3)
        End If

    End Sub

    Private Sub lvwConvictions_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwConvictions.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwConvictions.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditConv.Enabled = False
            cmdDeleteConv.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditConv.Enabled = True
                cmdDeleteConv.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwLifestyle_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwLifestyle.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwLifestyle.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditLife.Enabled = False
            cmdDeleteLife.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditLife.Enabled = True

                'DJM 14/05/2002 : Do not allow deleting Insured dependent
                cmdDeleteLife.Enabled = lvwLifestyle.GetItemAt(x, y).Text.Trim() <> "Insured"
            End If
        End If

    End Sub

    Private Sub lvwLoyaltySchemes_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwLoyaltySchemes.Enter
        If SSTabHelper.GetTabVisible(TabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(TabMainTab, 5)
        End If
    End Sub

    Private Sub lvwPolicies_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwPolicies.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwPolicies.Columns(eventArgs.Column)

        ' Column click event for the campaigns

        Try

            With lvwPolicies
                ' If current sort column header is
                ' pressed.
                If ListViewHelper.GetSortKeyProperty(lvwPolicies) = 4 Then
                    ListViewHelper.SetSortKeyProperty(lvwPolicies, 2)
                End If

                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwPolicies) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwPolicies, (ListViewHelper.GetSortOrderProperty(lvwPolicies) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwPolicies, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwPolicies, SortOrder.Ascending)
                    If ColumnHeader.Index + 1 = 2 Then
                        ListViewHelper.SetSortKeyProperty(lvwPolicies, 4)
                    Else
                        ListViewHelper.SetSortKeyProperty(lvwPolicies, ColumnHeader.Index + 1 - 1)
                    End If
                    ListViewHelper.SetSortedProperty(lvwPolicies, True)
                End If
            End With

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwPolicies_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwPolicies_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwPolicies.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwPolicies.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditPol.Enabled = False
            cmdDeletePol.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditPol.Enabled = True
                cmdDeletePol.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwLoyaltySchemes_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwLoyaltySchemes.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwLoyaltySchemes.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditLoyaltyScheme.Enabled = False
            cmdDeleteLoyaltyScheme.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditLoyaltyScheme.Enabled = True
                cmdDeleteLoyaltyScheme.Enabled = True
            End If
        End If

    End Sub

    'PN23222
    Private Sub pnlAgentName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlAgentName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        ToolTip1.SetToolTip(pnlAgentName, m_sAgentName)
    End Sub

    'PN23222
    Private Sub pnlBrokerName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlBrokerName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        ToolTip1.SetToolTip(pnlBrokerName, m_sBrokerName)
    End Sub

    'PN23222
    Private Sub PnlConsultantName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles PnlConsultantName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        ToolTip1.SetToolTip(PnlConsultantName, m_sConsultantName)
    End Sub

    'PN23222
    Private Sub pnlCurrentAgent_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlCurrentAgent.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        ToolTip1.SetToolTip(pnlCurrentAgent, m_sCurrentAgentName)
    End Sub

    'PN23222
    Private Sub pnlInsurerName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlInsurerName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        ToolTip1.SetToolTip(pnlInsurerName, m_sInsurerName)
    End Sub


    Private Sub TabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles TabMainTab.SelectedIndexChanged

        Try


            ' Set the default button.
            If SSTabHelper.GetSelectedIndex(TabMainTab) < cmdNext.Length Then
                If Not (cmdNext(SSTabHelper.GetSelectedIndex(TabMainTab)).FindForm() Is Nothing) Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(TabMainTab)), True)
                End If
            End If

            ' Now I know this is crap, this goes against
            ' all my principles, but for some reason when
            ' using the mouse to select a tab the setfocus
            ' code below doesn't work. The cursor sticks,
            ' and you can't tab off. Therefore I've used
            ' this to get around the problem.
            Application.DoEvents()

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(TabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(TabMainTab)).Focus()
            End If

        Catch



            ' Error Section.


            tabMainTabPreviousTab = TabMainTab.SelectedIndex
        End Try

    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAgentRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'Agent ref may no longer match the party_cnt in the tag, so need to
        'verify this when validating
        m_bVerifyAgentCnt = True

        ' CF 111199 - Check for empty text. If so, then remove any agent
        If txtAgentRef.Text.Trim() = "" Then

            'save the count in the tag and update controls
            txtAgentRef.Tag = ""
            m_sAgentRef = ""
            m_sAgentName = ""
            pnlAgentName.Text = ""

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyAgentCnt = False

        End If

    End Sub

    Private Sub txtAgentRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.Enter
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentRef)
        End If
    End Sub

    Private Sub txtAgentRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentRef)
        End If
    End Sub

    Private Sub txtAgentReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bChangedProspect = True
    End Sub

    Private Sub txtAgentReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.Enter

        If SSTabHelper.GetTabVisible(TabMainTab, 6) Then
            SSTabHelper.SetSelectedIndex(TabMainTab, 6)
        End If

        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentReference)
        End If
    End Sub

    Private Sub txtAgentReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentReference)
        End If
    End Sub

    ' DC 03/05/00
    ' Cater for more than one Associate
    ' Routines no longer required
    'Private Sub txtAssociateRef_Change()
    '
    '    m_bVerifyAssociatedCnt = True
    '
    '    ' CF 111199 - Check for empty text. If so, then remove any agent
    '    If (Trim$(txtAssociateRef.Text) = "") Then
    '
    '        'save the count in the tag and update controls
    '        txtAssociateRef.Tag = ""
    '        m_lAssociatedCnt = 0
    '        m_sAssociatedName$ = ""
    '
    '        'because we know Agent cnt matches the Agent ref, can bypass
    '        'the validation at the end
    '        m_bVerifyAssociatedCnt = True
    '
    '    End If
    '
    'End Sub
    '
    'Private Sub txtAssociateRef_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtAssociateRef)
    '
    'End Sub
    '
    'Private Sub txtAssociateRef_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtAssociateRef)
    '
    'End Sub

    Private Sub txtCCJ_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCCJ.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 3
        'DN 12/12/02 - Only focus to property if tab is visible
        If SSTabHelper.GetTabVisible(TabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(TabMainTab, 3)
        End If

        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCCJ)
        End If
    End Sub

    Private Sub txtCCJ_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCCJ.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCCJ)
        End If
    End Sub

    ' DC 03/05/00
    ' Cater for more than one Associate
    ' Routine no longer required
    'Private Sub txtConsultantRef_Change()
    '
    '    m_bVerifyConsultantCnt = True
    '
    '    ' CF 111199 - Check for empty text. If so, then remove any agent
    '    If (Trim$(txtConsultantRef.Text) = "") Then
    '
    '        'save the count in the tag and update controls
    '        txtConsultantRef.Tag = ""
    '        m_lAssociatedCnt = 0
    '        m_sConsultantRef$ = ""
    '        m_sConsultantName$ = ""
    '        PnlConsultantName.Caption = ""
    '
    '        'because we know Agent cnt matches the Agent ref, can bypass
    '        'the validation at the end
    '        m_bVerifyConsultantCnt = False
    '
    '    End If
    '
    'End Sub

    Private Sub txtConsultantRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 0
        SSTabHelper.SetSelectedIndex(TabMainTab, 0)


    End Sub

    Private Sub txtDOB_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 4
        SSTabHelper.SetSelectedIndex(TabMainTab, 4)

        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDOB)
        End If
    End Sub

    Private Sub txtDOB_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDOB.Leave
        If Not (m_oFormFields Is Nothing) Then
            'PN11842
            If txtDOB.Text <> "" Then
                m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDOB)
            End If
        End If

        If lvwLifestyle.Items.Count > 0 Then
            ListViewHelper.GetListViewSubItem(lvwLifestyle.Items.Item(0), 2).Text = txtDOB.Text
        End If

    End Sub


    Private Sub txtForename_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtForename.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.

        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtForename)
        End If
    End Sub

    'Private Sub txtForename_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtForename.KeyPress
    Private Sub txtForename_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtForename.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        ValidateChars(KeyAscii:=KeyAscii, v_bIsBusiness:=False)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtForename_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtForename.Leave
        'DJM 04/09/2003 : Formatting function works now, so we don't need this.
        'txtForename.Text = StrConv(txtForename.Text, vbProperCase)

        'If Not (m_oFormFields Is Nothing) Then
        '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtForename)
        'End If
    End Sub

    Private Sub txtIDReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Dim MyValue As String = ""

        'Set the reference on the second and third tab
        'pnlAdReference.Caption = txtIDReference.Text
        'pnlConReference.Caption = txtIDReference.Text

    End Sub

    Private Sub txtIDReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 0
        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtIDReference)
        End If
    End Sub

    Private Sub txtIDReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Leave

        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtIDReference)
        End If
    End Sub

    Private Sub txtInitials_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitials.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInitials)
        End If
    End Sub

    Private Sub txtInitials_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitials.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInitials)
        End If
    End Sub
    'developer guide no. 42
    Private Sub txtInitials_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtInitials.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        '**********************************************************************
        'MKR 12/10/2004 PN 6021 -- Allowing "," in Initials as it is a part of
        'Resolved Name
        If KeyAscii <> 44 Then
            ValidateChars(KeyAscii:=KeyAscii, v_bIsBusiness:=False)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    'sj 12/06/2002 - start
    ' ***************************************************************** '
    '
    ' Name: txtAlternativeIdentifier_Change
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Sub txtAlternativeIdentifier_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternativeIdentifier.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            If m_bIsNRMA And m_bUserMode Then
                PartyFunc.AlternativeIdentifierChange(r_oAlternativeIdentifier:=txtAlternativeIdentifier)
            End If

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtAlternativeIdentifier_Change Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAlternativeIdentifier_Change", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: txtAlternativeIdentifier_LostFocus
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Sub txtAlternativeIdentifier_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternativeIdentifier.Leave

        Try

            Dim sMessage As String = ""

            If m_bIsNRMA Then
                If txtAlternativeIdentifier.Text <> PartyFunc.m_kBlankAlternativeIdentifier Then
                    PartyFunc.AlternativeIdentifierLostFocus(v_oAlternativeIdentifier:=txtAlternativeIdentifier, v_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript, v_iTask:=Task)
                End If
            Else
                PartyFunc.AlternativeIdentifierLostFocus(v_oAlternativeIdentifier:=txtAlternativeIdentifier, v_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript, v_iTask:=Task)
            End If

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtAlternativeIdentifier_LostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAlternativeIdentifier_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: txtAlternativeIdentifier_KeyPress
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 42
    Private Sub txtAlternativeIdentifier_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtAlternativeIdentifier.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Try

            If m_bIsNRMA Then
                PartyFunc.StopNonNumericCharacters(r_iKeyAscii:=KeyAscii)
            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try




        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtAlternativeIdentifier_KeyPress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtAlternativeIdentifier_KeyPress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        Exit Sub

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    Private Sub txtAlternativeIdentifier_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAlternativeIdentifier.Enter
        If m_bIsNRMA Then
            txtAlternativeIdentifier.SelectionStart = Strings.Len(txtAlternativeIdentifier.Text)
        End If
    End Sub

    ' ***************************************************************** '
    '
    ' Name: txtLoyaltyNumber_KeyPress
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 42
    Private Sub txtLoyaltyNumber_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtLoyaltyNumber.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)

        Try

            If m_bIsNRMA Then
                PartyFunc.StopNonNumericCharacters(r_iKeyAscii:=KeyAscii)
            End If

            If KeyAscii = 0 Then
                eventArgs.Handled = True
            End If
            Exit Sub

        Catch
        End Try




        ' Log Error Message
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtLoyaltyNumber_KeyPress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtLoyaltyNumber_KeyPress", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        Exit Sub

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    ' ***************************************************************** '
    '
    ' Name: txtLoyaltyNumber_LostFocus
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Sub txtLoyaltyNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLoyaltyNumber.Leave

        Try

            Dim sMessage As String = ""

            'sj 25/07/2002 - start
            '    LoyaltyNumberLostFocus _
            ''            v_oNewLoyaltyNumber:=txtLoyaltyNumber, _
            ''            v_sOldLoyaltyNumber:=m_sLoyaltyNumber, _
            ''            v_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, _
            ''            v_iTask:=Task
            If m_bIsNRMA Then
                PartyFunc.LoyaltyNumberLostFocus(v_oNewLoyaltyNumber:=txtLoyaltyNumber, v_sOldLoyaltyNumber:=m_sLoyaltyNumber, v_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, v_iTask:=Task)
            Else
                'MKW260903 PN7071 No Validation on Broking.
                If m_sUnderwritingOrAgency <> "A" Then
                    PartyFunc.LoyaltyNumberLostFocus(v_oNewLoyaltyNumber:=txtLoyaltyNumber, v_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, v_iTask:=Task)
                End If
            End If
            'sj 25/07/2002 - end

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtLoyaltyNumber_LostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtLoyaltyNumber_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    'sj 12/06/2002 - end

    Private Sub txtSurname_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSurname.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSurname)
        End If
    End Sub

    'developer guide no. 42
    Private Sub txtSurname_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtSurname.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        '************************************************************************
        'MKR 12/10/2004 PN 6021 -- Allowing "," in SurName (as surname is a part
        'of Resolved Name
        If KeyAscii <> 44 Then
            ValidateChars(KeyAscii:=KeyAscii, v_bIsBusiness:=False)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtSurname_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSurname.Leave
        'DJM 04/09/2003 : Formatting function works now, so we don't need this.
        'txtSurname.Text = StrConv(txtSurname.Text, vbProperCase)

        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSurname)
        End If
    End Sub

    Private Sub txtTobLetter_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTobLetter.Enter

        SSTabHelper.SetSelectedIndex(TabMainTab, 2)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtTobLetter)
        End If

    End Sub

    Private Sub txtTobLetter_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTobLetter.Leave
        'PN16761
        If Not (m_oFormFields Is Nothing) Then

            If txtTobLetter.Text <> "" Then
                m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtTobLetter)
            End If
        End If

    End Sub

    'developer guide no. 42
    Private Sub txtTradingName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtTradingName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        '**********************************************************************
        'MKR 12/10/2004 PN 6021 -- Allowing "," in Trading Name
        '    If KeyAscii <> 44 Then
        '        Call RestrictChars(KeyAscii, False)
        '    End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    'Party Bank Details
    'UPGRADE_NOTE: (7001) The following declaration (uctPartyBankControl1_RefreshBankDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub uctPartyBankControl1_RefreshBankDetails(ByRef vBankDetails( ,  ) As Object)
    'm_vPartyBankDetails = vBankDetails
    'End Sub

    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled

        'm_Font = Ambient.Font
        m_Font = MyBase.Font
        m_BackStyle = m_def_BackStyle
        'TODO:
        'm_BorderStyle = BorderStyle.None
        m_BorderStyle = Windows.Forms.BorderStyle.None
        m_PartyCnt = m_def_PartyCnt
    End Sub

    Private Sub uctPartyPCControl_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000

        Dim iCtrlDown As Integer

        Const ACCtrlMask As Integer = 2

        Try

            ' Set the control key value.
            iCtrlDown = (Shift And ACCtrlMask) > 0

            With TabMainTab
                ' Check the key pressed.
                Select Case KeyCode
                    Case Keys.PageUp
                        ' Page Up key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the first tab.
                            SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        Else
                            ' Check we are not on the
                            ' first tab.
                            If SSTabHelper.GetSelectedIndex(TabMainTab) > 0 Then
                                ' Display the previous tab.
                                SSTabHelper.SetSelectedIndex(TabMainTab, SSTabHelper.GetSelectedIndex(TabMainTab) - 1)
                            End If
                        End If

                    Case Keys.PageDown
                        ' Page Down key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Display the last tab.
                            SSTabHelper.SetSelectedIndex(TabMainTab, SSTabHelper.GetTabCount(TabMainTab) - 1)
                        Else
                            ' Check we are not on the
                            ' last tab.
                            If SSTabHelper.GetSelectedIndex(TabMainTab) < (SSTabHelper.GetTabCount(TabMainTab) - 1) Then
                                ' Display the next tab.
                                SSTabHelper.SetSelectedIndex(TabMainTab, SSTabHelper.GetSelectedIndex(TabMainTab) + 1)
                            End If
                        End If

                    Case Keys.Home
                        ' Home key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(TabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(TabMainTab)).Focus()
                            End If
                        End If

                    Case Keys.End
                        ' End key has been pressed.

                        ' Check if the control key has also
                        ' been pressed.
                        If iCtrlDown Then
                            ' Set focus the the start control on
                            ' the tab.
                            If SSTabHelper.GetSelectedIndex(TabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                                m_ctlTabFirstLast(ACControlEnd, SSTabHelper.GetSelectedIndex(TabMainTab)).Focus()
                            End If
                        End If
                End Select
            End With

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub uctPartyPCControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'developer guide no. 220
        Me.cboCurrency.FirstItem = ""
        'added following line to hide "Prospect Policy" tab against issue no 1730
        'Made following tab hidden as per 'Stephen Ross' mail
        Me.TabMainTab.TabPages.RemoveAt(6)
    End Sub

    Private Sub uctPartyPCControl_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
        'sj 12/06/2002 - start
        Static bDoNotSetFocus As Boolean

        If Not DesignMode Then
            If Not bDoNotSetFocus Then
                If txtIDReference.Enabled Then
                    txtIDReference.Focus()
                ElseIf txtSurname.Enabled Then
                    txtSurname.Focus()
                End If
                bDoNotSetFocus = True
            End If
        End If
        'sj 12/06/2002 - end
    End Sub

    'Load property values from storage
    'developer guide no. 1 (No Solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'developer guide no. 2(No Solution)
        'm_Font = PropBag.ReadProperty("Font", Ambient.Font)


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = PropBag.ReadProperty("BorderStyle", m_def_BorderStyle)


        m_PartyCnt = CInt(PropBag.ReadProperty("PartyCnt", m_def_PartyCnt))

    End Sub

    Private Sub uctPartyPCControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        ' Maintain minimum width
        If VB6.PixelsToTwipsX(Width) < 10695 Then Width = VB6.TwipsToPixelsX(10695)
        ' and minimum height
        If VB6.PixelsToTwipsY(Height) < 4995 Then Height = VB6.TwipsToPixelsY(4995)

    End Sub

    'Write property values to storage


    'developer guide no. 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)


        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'developer guide no. 2(NS)
        'PropBag.WriteProperty("Font", m_Font, Ambient.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("PartyCnt", m_PartyCnt, m_def_PartyCnt)
    End Sub

    ' PRIVATE Events (End)

    ' ***************************************************************** '
    ' Name: UpdateLifestyle
    '
    ' Description: Delete any dependents that have been removed.
    '
    ' Edit History:
    '
    ' DJM 06/06/2002 : Don't delete the insured record.
    ' DJM 14/05/2002 : Created
    ' ***************************************************************** '
    Private Function UpdateLifestyle() As Integer
        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        'developer guide no. 17
        Dim vNewLifestyle(,) As Object
        Dim bFirst As Boolean
        Dim iLoop, iRow As Integer
        Dim bFound As Boolean
        Dim vPartyLifestyleID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Go thru dependent grid to get list of new dependents
            iLoop = 1
            bFirst = True
            Do
                If iLoop > lvwLifestyle.Items.Count Then
                    Exit Do
                End If

                oListItem = lvwLifestyle.Items.Item(iLoop - 1)

                If bFirst Then
                    ReDim vNewLifestyle(1, iLoop - 1)
                    bFirst = False
                Else
                    ReDim Preserve vNewLifestyle(1, iLoop - 1)
                End If


                vNewLifestyle(0, iLoop - 1) = Convert.ToString(oListItem.Tag).Trim()

                iLoop += 1
            Loop

            'Create m_oLifestyleBusiness if not already done so
            If m_oLifestyleBusiness Is Nothing Then

                'Get an instance of the Lifestyle Business object via the public object manager.
                'sj 23/07/2002 - start
                '        m_lReturn& = g_oObjectManager.GetInstance( _
                ''            oObject:=m_oLifestyleBusiness, _
                ''            sClassName:="bSIRLifeStyle.Business", _
                ''            vInstanceManager:=PMGetLocalBusiness)
                Dim temp_m_oLifestyleBusiness As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oLifestyleBusiness, "bSIRLifeStyle.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oLifestyleBusiness = temp_m_oLifestyleBusiness
                'sj 23/07/2002 - end

                'Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get lifestyles business class", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLifestyle", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If


            m_lReturn = m_oLifestyleBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If we have old and new Lifestyle, delete common ones
            If Information.IsArray(vNewLifestyle) Then


                m_lReturn = m_oLifestyleBusiness.GetNext(vPartyLifestyleID:=vPartyLifestyleID)

                Do While m_lReturn <> gPMConstants.PMEReturnCode.PMEOF

                    'DJM 10/06/2002 : Don't delete the insured record.
                    If CInt(vPartyLifestyleID) <> 1 Then


                        iRow = m_oLifestyleBusiness.CurrentRecord

                        bFound = False

                        For iLoop = vNewLifestyle.GetLowerBound(1) To vNewLifestyle.GetUpperBound(1)

                            If vPartyLifestyleID.Trim() = CStr(vNewLifestyle(0, iLoop)).Trim() Then
                                bFound = True
                                Exit For
                            End If
                        Next iLoop

                        If Not bFound Then

                            m_lReturn = m_oLifestyleBusiness.EditDelete(iRow)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        End If
                    End If


                    m_lReturn = m_oLifestyleBusiness.GetNext(vPartyLifestyleID:=vPartyLifestyleID)

                Loop


                m_oLifestyleBusiness.Update()

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLifestyleFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLifestyle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetDefaultAgentDetails
    '
    ' Description:
    '
    ' History: 17/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetDefaultAgentDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lPartyCnt As Integer

            'See if there is an agent allocated to this user

            m_lReturn = m_oPMUser.GetDetails(vUserId:=g_iUserId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMUser.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails")
                Return result
            End If


            m_lReturn = m_oPMUser.GetNext(vPartyCnt:=lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMUser.GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails")
                Return result
            End If

            If lPartyCnt = 0 Then
                ' No agent allocated to this user so exit here
                Return result
            End If

            m_lAgentCnt = lPartyCnt

            'Get the agent cnt, name and ref from the "core" party table

            m_lReturn = m_oBusiness.GetCorePartyDetails(vPartyCnt:=m_lAgentCnt, vShortName:=m_sAgentRef, vName:=m_sAgentName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetCorePartyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails")
                Return result
            End If

            m_sAgentRef = m_sAgentRef.Trim()
            m_sAgentName = m_sAgentName.Trim()

            'Update controls on the form
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            pnlAgentName.Text = m_sAgentName

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultAgentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ValidateChars
    '
    ' Description: Restricts KeyPresses to a-z A-Z Space - '
    '
    ' CMG(SJP) ISS868 28/02/2003
    '
    ' ***************************************************************** '
    Private Sub ValidateChars(ByRef KeyAscii As Integer, ByVal v_bIsBusiness As Boolean)

        Try

            If v_bIsBusiness Then
                Select Case KeyAscii
                    Case Is < 33 ' RETURN, SPACE ETC
                    Case 39, 44, 45 ' - , and '
                    Case 48 To 57 ' 0 to 9
                    Case 65 To 90, 97 To 122 ' A to Z
                    Case Else
                        KeyAscii = 0
                End Select
            Else
                Select Case KeyAscii
                    Case Is < 33 ' RETURN, SPACE ETC
                    Case 39, 45 ' - and '
                        'Accept as valid characters
                    Case 65 To 90, 97 To 122 ' A to Z
                    Case Else
                        KeyAscii = 0
                End Select
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateChars Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateChars", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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


        Dim iIndex, iBaseCurrencyID As Integer
        Dim lSourceId As Integer

        Dim oPartyBusiness As bSIRParty.Business

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            iIndex = cboBranch.SelectedIndex

            If iIndex = -1 Then
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

            ' get the SourceID
            lSourceId = VB6.GetItemData(cboBranch, iIndex)

            ' this value SHOULD exist, but more error trapping here?

            ' call the business. to get the Base Currency ID

            m_lReturn = oPartyBusiness.GetBaseCurrencyID(lSourceID:=lSourceId, iCurrencyID:=iBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get base currency for SourceID " & lSourceId, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")

                Return result
            End If

            cboCurrency.CompanyId = lSourceId
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = iBaseCurrencyID


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSourceBaseCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: ShowDuplicateParty
    '
    ' Description:
    '
    ' History: 21/01/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Private Function ShowDuplicateParty(ByVal sOriginalClientCode As String, ByVal sUniqueClientCode As String, ByRef sSelectedClientCode As String, ByRef lSelectedClientPartyCnt As Integer, ByRef iOKAction As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMBPartyDuplicate As Object
        Dim vDuplicateClientData As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_lReturn = m_oBusiness.CheckDuplicateShortName(v_sShortName:=sOriginalClientCode, v_vMatchArray:=vDuplicateClientData)
            'Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get CheckDuplicateShortName", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Get an instance of the Duplicate Client interface object via
            'the public object manager.
            Dim temp_oPMBPartyDuplicate As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMBPartyDuplicate, sClassName:="iPMBPartyDuplicate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPMBPartyDuplicate = temp_oPMBPartyDuplicate

            'Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Duplicate Party Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oPMBPartyDuplicate.Initialise
            'Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initiliase Duplicate Party Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If





            oPMBPartyDuplicate.ClientData = vDuplicateClientData

            oPMBPartyDuplicate.PartyTypeCode = gSIRLibrary.SIRPartyTypePersonalClient

            oPMBPartyDuplicate.OriginalClientCode = sOriginalClientCode

            oPMBPartyDuplicate.UniqueClientCode = sUniqueClientCode


            oPMBPartyDuplicate.Start()



            iOKAction = oPMBPartyDuplicate.OKAction

            sSelectedClientCode = oPMBPartyDuplicate.SelectedClientCode

            lSelectedClientPartyCnt = oPMBPartyDuplicate.SelectedClientPartyCnt


            result = oPMBPartyDuplicate.Status


            oPMBPartyDuplicate.Dispose()
            oPMBPartyDuplicate = Nothing


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShowDuplicateParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDuplicateParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function CheckMainMandatoryControls() As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oFormFields.Item("txtSurname-0").IsMandatory Then
                If txtSurname.Text.Trim().Length = 0 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Surname", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    'developer guide no. 222
                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                    SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                    If txtSurname.Visible Then
                        txtSurname.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'sj 18/06/2002 - end
                    Return result
                Else
                    If Not IsValidString(txtSurname.Text) Then
                        MessageBox.Show("Last Name can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } | • ‣ ◦ ⁃", "Invalid Input - Last Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Dim selIndex As Integer = 0
                        selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        If txtSurname.Visible Then
                            txtSurname.Focus()
                        Else
                            SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                        End If
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                End If
            End If
            End If

            If m_oFormFields.Item("txtForename-0").IsMandatory Then
                If txtForename.Text.Trim().Length = 0 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Forename", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    'developer guide no. 222
                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                    SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                    If txtForename.Visible Then
                        txtForename.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'sj 18/06/2002 - end
                    Return result
                Else
                    If Not IsValidString(txtForename.Text) Then
                        MessageBox.Show("Forename can't contain any of the following characters. " & vbNewLine & ":~ "" # % & * : < > ? / \ { } | • ‣ ◦ ⁃", "Invalid Input - Forename", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Dim selIndex As Integer = 0
                        selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                        SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                        If txtForename.Visible Then
                            txtForename.Focus()
                        Else
                            SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                        End If
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                End If
            End If
            End If

            If m_oFormFields.Item("txtInitials-0").IsMandatory Then
                If txtInitials.Text.Trim().Length = 0 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Initials", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    'developer guide no. 222
                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(TabMainTab)
                    SSTabHelper.SetSelectedIndex(TabMainTab, 0)
                    If txtInitials.Visible Then
                        txtInitials.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(TabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMainMandatoryControls Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMainMandatoryControls", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function UpdateTurnoverDetails() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateTurnoverDetails"

        Dim sFormattedCurrency As String = ""
        Dim cConvertedAmount As Decimal

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        'Get Client Account Details

        m_lReturn = m_oAccount.GetClientAccountDetails(v_lAccountKey:=m_lPartyCnt, v_lCompanyID:=m_iPartySourceId, r_curYearToDateTurnover:=m_cYearToDateTurnover, r_curLastYearTurnover:=m_cLastYearTurnover, r_curClientBalance:=m_cClientBalance)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAccount.GetClientAccountDetails", "v_lAccountKey = " & m_lPartyCnt, gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cClientBalance, cCurrencyAmount:=cConvertedAmount)

        m_cClientBalance = cConvertedAmount

        'Update the interface with the turnover details

        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cClientBalance, vFormattedCurrency:=sFormattedCurrency)
        pnlClientBalance.Text = sFormattedCurrency


        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cYearToDateTurnover, vFormattedCurrency:=sFormattedCurrency)
        pnlYearToDateTurnover.Text = sFormattedCurrency


        m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cLastYearTurnover, vFormattedCurrency:=sFormattedCurrency)
        pnlLastYearTurnover.Text = sFormattedCurrency


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

        ' This is for debugging only
            '        Resume

            '        Return result
        End Try
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
                m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(CStr(vPartyDetails(kPartyDetailDomiciledForTax, 0)), 0)
                m_bTaxExempt = gPMFunctions.ToSafeBoolean(CStr(vPartyDetails(kPartyDetailTaxExempt, 0)), 0)
                m_dTaxPercentage = gPMFunctions.ToSafeDouble(CStr(vPartyDetails(kPartyDetailTaxPercentage, 0)), 0)
                m_vBlackListReasonId = gPMFunctions.ToSafeLong(CStr(vPartyDetails(kPartyDetailBlackListReasonId, 0)), 0)
            End If

            Return result

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            result = gPMConstants.PMEReturnCode.PMError
            Return result
        End Try
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

        ReDim r_vPartyDetails(4, 0)

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

        'developer guide no. 131
        If m_vBlackListReasonId Is DBNull.Value OrElse m_vBlackListReasonId = 0 Then


            r_vPartyDetails(kPartyDetailBlackListReasonId, 0) = Nothing
        Else

            r_vPartyDetails(kPartyDetailBlackListReasonId, 0) = m_vBlackListReasonId
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
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count
                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

            End If

            If bItemNotFound Then

                ' log that we havent found the specified item
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSelectedId", v_lSelectedId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedId", v_lSelectedId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetClientCode
    '
    ' Description:
    '
    ' History: VB
    '
    ' ***************************************************************** '
    Private Function GetClientCode(ByRef r_sClientCode As String, ByVal v_sInitial As String, ByVal v_sValue As String) As Integer
        Dim result As Integer = 0
        Dim sFailureReason As String = ""
        Dim iBranchId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oClientNumber Is Nothing Then
                Dim temp_m_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oClientNumber = temp_m_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of bSIRPolicyNumMaint.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode")
                    Return result
                End If
            End If

            iBranchId = BranchId


            m_lReturn = m_oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypePersonalClient, v_iSourceID:=iBranchId, r_sGeneratedClientCode:=r_sClientCode, r_sFailureReason:=sFailureReason, v_sValue:=v_sValue, v_sInitial:=v_sInitial, v_sTitle:=Title, v_sTradeName:=TradeName, v_sType:=Forename)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate client code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode")

                Return result
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("Numbering scheme for Personal Client is not set.", "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMCancel
            ElseIf sFailureReason <> "" Then
                MessageBox.Show(sFailureReason, "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMCancel
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Const kMethodName As String = "SetClientCodeCntl"

        Dim r_bIsReadOnly, r_bIsNumberingSchemeExists As Boolean
        Dim r_sMaskCode As String = ""
        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oClientNumber Is Nothing Then
            Dim temp_m_oClientNumber As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oClientNumber = temp_m_oClientNumber

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.RaiseError("SetClientCodeCntl", "bSIRPolicyNumMaint.Business instance not Created")
                    Return result
            End If
        End If


        m_lReturn = m_oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypePersonalClient, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails falied")
                Return result
        End If

        m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
        m_bIsReadOnly = r_bIsReadOnly
        m_sMaskCode = r_sMaskCode

        If (r_bIsNumberingSchemeExists And r_bIsReadOnly) Or (Not r_bIsNumberingSchemeExists And m_bDuplicateClientIdentification) Or (r_bIsNumberingSchemeExists And Not r_bIsReadOnly And m_bDuplicateClientIdentification) Then
            lblIDReference.Enabled = False
            txtIDReference.Enabled = False
        Else
            lblIDReference.Enabled = True
            txtIDReference.Enabled = True
        End If




        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Function ValidateNumberingScheme() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateNumberingScheme"

        Try


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_sMaskCode <> "" Then
            ' Branch
            If m_sMaskCode.IndexOf("B"c) >= 0 Then
                If cboBranch.SelectedIndex < 0 Or BranchId < 1 Then
                    MessageBox.Show("Please select some Branch", "field - Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                End If
            End If

            ' Last Name
            If m_sMaskCode.IndexOf("L"c) >= 0 Then
                If txtSurname.Text = "" Then
                    MessageBox.Show("Please Enter Surname", "field - Surname", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                End If
            End If

            ' Fore Name
            If m_sMaskCode.IndexOf("F"c) >= 0 Then
                If txtForename.Text = "" Then
                    MessageBox.Show("Please Enter Fore Name", "field - Fore Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                End If
            End If

            ' Initials
            If m_sMaskCode.IndexOf("I"c) >= 0 Then
                If txtInitials.Text = "" Then
                    MessageBox.Show("Please Enter Fore Initials", "field - Initials", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                End If
            End If

        End If



        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub ddSecEmploymentStatus_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ddSecEmploymentStatus.Load

    End Sub

    Private Sub fraPaymentDetails_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles fraPaymentDetails.Enter

    End Sub

    Private Function IsValidString(ByRef str As String) As Boolean
        Dim illegalChars As New RegularExpressions.Regex("[{}:~#%*<>?'`’\\/|,\u2022,\u2023,\u25E6,\u2043,\u2219]")

        If illegalChars.IsMatch(str) Then
                Return False
            End If

        Return True
    End Function

    Private Sub txtInitials_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtInitials.Validated
        If m_oFormFields Is Nothing = False Then
            m_lReturn = m_lReturn & m_oFormFields.LostFocus(ctlControl:=txtForename)
            uctPartyBankControl1.PartyName = Trim$(ddTitle.Text) & " " & Trim$(txtInitials.Text) & " " & Trim$(txtSurname.Text)
        End If
    End Sub

    Private Sub txtSurname_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSurname.Validated
        If m_oFormFields Is Nothing = False Then
            m_lReturn = m_lReturn & m_oFormFields.LostFocus(ctlControl:=txtForename)
            uctPartyBankControl1.PartyName = Trim$(ddTitle.Text) & " " & Trim$(txtInitials.Text) & " " & Trim$(txtSurname.Text)
        End If
    End Sub

    Private Sub ddTitle_Validated(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddTitle.Validated
        If m_oFormFields Is Nothing = False Then
            m_lReturn = m_lReturn & m_oFormFields.LostFocus(ctlControl:=txtForename)
            uctPartyBankControl1.PartyName = Trim$(ddTitle.Text) & " " & Trim$(txtInitials.Text) & " " & Trim$(txtSurname.Text)
        End If
    End Sub
    ''' <summary>
    ''' Add Party History
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyHistory() As Integer
        Dim nReturn As Integer
        Dim oPartyBusiness As bSIRParty.Business = Nothing
        Try
            nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyHistory")
                Return nReturn
            End If

            ' Create the Party history
            If PartyCnt > 0 Then
                nReturn = oPartyBusiness.AddPartyHistory(PartyCnt, String.Empty)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyHistory")
                End If
            End If
            Return nReturn
        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ex.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="AddPartyHistory")
            Return PMEReturnCode.PMFalse
        End Try

    End Function
End Class
