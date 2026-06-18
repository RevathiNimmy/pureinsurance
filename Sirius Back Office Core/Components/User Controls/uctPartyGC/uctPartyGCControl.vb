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
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPartyGCControl_NET.uctPartyGCControl")>
Partial Public Class uctPartyGCControl
    Inherits System.Windows.Forms.UserControl
    Implements IDisposable
    Public Event OrgNameChange()
    Public Event FromEventChange()
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' DJM 22/04/2002 : MainContactCnt changed from a int to a long.
    ' TF031298 - Menu & Toolbar activity
    ' RAW 18/11/2002 : PS005 : Add tab6 for customer loyalty scheme
    ' RKS 24/01/2005 : Implemented Duplicate Client Identification
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "uctPartyGCControl"

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
    Dim m_BorderStyle As Integer
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
    ' PUBLIC Data Members (End)

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

    'RWH(24/07/2000) RSAIB Process 004
    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    'DC 04/08/00
    Private m_lMainContactCnt As Integer
    Private m_sMainContactDesc As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the Lock object.
    Private m_oPMLock As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Declare an instance of the prospect policy interface.

    'developer guide no. 88
    Private m_oProspectPolicy As Object
    'eck010900
    Private m_oPMUser As Object

    '2005 Layout Changes
    Private m_oAccount As Object
    Private m_oCurrencyConvert As Object

    Private m_bChangedProspect As Boolean

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(1, 6) As Control

    ' Stores the details from the business object.

    ' {* USER DEFINED CODE (Begin) *}
    'Data Retreived via Get Details from Party & PartyPC

    Private m_iPartyTypeId As Integer

    'DC 03/05/00
    'Multi-Branch
    Private m_iPartySourceId As Integer
    'eck010900
    'DN 01/06/01 - Change PartyIDs to Longs
    Private m_iNewPartySourceId As Integer
    Private m_lPartyId As Integer
    Private m_lNewPartyId As Integer
    '

    Private m_iIsRegisteredCharity As CheckState
    Private m_iIsAlsoAgent As CheckState
    Private m_iIsProspect As CheckState
    Private m_iCurrencyId As Integer

    'DC 25/09/00
    Private m_sOldIdReference As String = ""
    Private m_sOldResolvedName As String = ""

    Private m_lPartyCnt As Integer
    Private m_lPartyGroupTypeId As Integer
    Private m_lNumberofMembers As Integer
    Private m_lAgentCnt As Integer
    Private m_lConsultantCnt As Integer
    Private m_iAreaId As Integer
    Private m_lReminderTypeID As Integer
    Private m_lServiceLevelId As Integer
    Private m_lCCJs As Integer
    'developer guide no. 71(guide)
    Private m_vSeasonalGiftId As Object
    Private m_vStrengthCodeId As Object
    Private m_vPreviousInsurerCnt As Object
    Private m_vPreviousBrokerCnt As Object
    Private m_sInsurerRef As String = ""
    Private m_sInsurerName As String = ""
    Private m_sBrokerRef As String = ""
    Private m_sBrokerName As String = ""
    '2005 Screen Layout Changes
    Private m_cYearToDateTurnover As Decimal
    Private m_cLastYearTurnover As Decimal
    Private m_cClientBalance As Decimal

    Private m_vCorrespondenceTypeId As Integer

    'Tomo060700
    Private m_vRenewalStopCodeId As Integer

    Private m_sCharityNumber As String = ""
    Private m_sCreditCardCode As String = ""
    Private m_sServiceLevel As String = ""
    Private m_sPaymentMethodCode As String = ""
    Private m_iPaymentTermId As Integer
    'Private m_sPaymentTermCode As String = ""
    Private m_sFileCode As String = ""
    Private m_sGroupTypeCode As String = ""
    Private m_sName As String = ""
    Private m_sShortName As String = ""
    Private m_sResolved As String = ""

    'References from Party Lookups
    Private m_sAgentRef As String = ""
    Private m_sAgentName As String = ""
    Private m_sConsultantRef As String = ""
    Private m_sConsultantName As String = ""
    'developer guide no. 71(guide)
    Private m_vAssociates As Object
    'DC 03/05/00
    Private m_vRelationships As Object

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lAddressUsageTypeID As Integer
    Private m_lContactCnt As Integer
    Private m_sMainPostCode As String = ""
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_sAddressLine1 As String = ""
    Private m_vConvictions(,) As Object
    Private m_vLoyaltySchemes(,) As Object
    Private m_vCorrespondenceTypes(,) As Object

    'Flag to indicate whether we need to check the headoffice id matches
    'the headoffice ref as user may change the reference directly
    Private m_bVerifyHeadOfficeCnt As Boolean
    Private m_bVerifyAgentCnt As Boolean
    Private m_bVerifyConsultantCnt As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    Private m_vOldAddress(,) As Object
    Private m_vNewAddress(,) As Object

    ' Declare an instance of the address interface.

    'developer guide no. 88 (guide)
    Private m_oAddress As iPMBAddress.Interface_Renamed

    'DC 03/05/00
    ' Declare an instance of the associates interface.

    'developer guide no. 88 (guide)
    Private m_oAssociates As Object

    ' Declare an instance of the contact interface.

    'developer guide no. 88 (guide)
    Private m_oContact As iPMBContact.Interface_Renamed

    ' Declare an instance of the conviction interface.

    'developer guide no. 88 (guide)
    Private m_oConviction As iPMBPartyConviction.Interface_Renamed

    ' Declare an instance of the prospecting interface.

    Private m_oProspect As Object

    ' Declare an instance of the PartyLoyaltyScheme interface.

    'developer guide no. 88 (guide)
    Private m_oPartyLoyaltyScheme As iPMBPartyLoyaltyScheme.Interface_Renamed

    Private m_bChangedProspectPolicies As Boolean

    ' Agent
    Private m_lCurrentAgent As Integer
    Private m_sCurrentAgentRef As String = ""
    Private m_sCurrentAgentName As String = ""
    Private m_bVerifyCurrentAgentCnt As Boolean

    Private m_bEvent As Boolean

    ' CTAF 270900 - SwiftPartyID
    Private m_lSwiftPartyID As Integer

    'RWH(24/07/2000) Used thes instead of resource file 'cos rc file
    'appeared to have grown too large and would not compile.
    Private Const sPOSTCODE_TITLE As String = "Postcode"
    Private Const sADDR_USAGE_TITLE As String = "Address Usage"
    Private Const sADDR_LINE1_TITLE As String = "Address Line 1"
    Private Const sADDR_LINE2_TITLE As String = "Address Line 2"
    Private Const sADDR_LINE3_TITLE As String = "Address Line 3"
    Private Const sADDR_LINE4_TITLE As String = "Address Line 4"
    'sj 13/06/2002 - start
    Private m_sLoyaltyNumber As String = ""
    Private m_sAlternativeIdentifier As String = ""
    Private m_sTradingName As String = ""
    Private m_lSubBranchId As Integer
    Private m_bUserMode As Boolean
    Private m_bIsNRMA As Boolean
    Private m_bValidateAlternativeIdentifier As Boolean
    Private m_sBranchPrefix As String = ""
    Private m_sLoyaltyNumberScript As String = ""
    Private m_sAlternativeIdentifierScript As String = ""
    Private m_dtTobLetter As Date


    Private m_bFutureDateAddressChanges As Boolean
    Private m_vFutureDatedAddresses As Object
    Private m_bUpdateFutureDatedAddress As Boolean
    Private m_sUnderwritingOrAgency As String = ""
    Private m_bMultiTreeAccounting As Boolean

    Private m_iTPSind As CheckState
    Private m_iMailshot As CheckState
    Private m_iEMPSind As CheckState

    Private m_bIncludeClosedBranchChecked As Boolean
    Private m_bShowSubBranchID As Boolean
    Private m_bDuplicateClientIdentification As Boolean

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double
    Private m_vBlackListReasonId As Double
    Private m_bSystemOptionClientBlacklistingInForce As Boolean
    '**************************************************

    Private m_vSourceArray(,) As Object
    Private m_vTurnover As Integer

    'MIPS Client Numbering
    Private m_oClientNumber As Object
    Private m_vIsFeeClient As String = ""

    'Maintain Party Code
    Private m_bIsSetMaskingCode As Boolean
    Private m_bIsReadOnly As Boolean
    Private m_sMaskCode As String = ""

    'Party Bank Details
    Private m_vPartyBankDetails(,) As Object
    Private m_vPartyBankHistory As Object
    'For PN-43232
    Private m_sCurrentResolvedName As String = ""
    'PN48908
    Private m_bValidateNumberingScheme As Boolean

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
            m_lStatus = Value
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

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_lNavigate = Value

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_lProcessMode = Value

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the type of business.
            m_sTransactionType = Value

        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)

            ' Standard Property.

            ' Set the effective date.
            m_dtEffectiveDate = Value

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

        End Set
    End Property

    <Browsable(True)>
    Public Property ShortName() As String
        Get

            Return m_sShortName

        End Get
        Set(ByVal Value As String)

            m_sShortName = Value

        End Set
    End Property


    <Browsable(True)>
    Public Property ResolvedName() As String
        Get

            Return m_sResolved

        End Get
        Set(ByVal Value As String)

            m_sResolved = Value

        End Set
    End Property

    <Browsable(True)>
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
    <Browsable(True)>
    Public Property MainPostCode() As String
        Get

            Return m_sMainPostCode

        End Get
        Set(ByVal Value As String)

            m_sMainPostCode = Value

        End Set
    End Property
    <Browsable(True)>
    Public Property AddressLine1() As String
        Get

            Return m_sAddressLine1

        End Get
        Set(ByVal Value As String)

            m_sAddressLine1 = Value

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

    'JDW 14092001

    <Browsable(True)>
    Public Property OrgName() As String
        Get
            Return txtName.Text
        End Get
        Set(ByVal Value As String)
            txtName.Text = Value
            RaiseEvent OrgNameChange()
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

    ' {* USER DEFINED CODE (End) *}
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' Log client viewed event
    Private Function LogClientViewedEvent(lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oPartyBusiness As bSIRParty.Business

            ' we need bSIRParty.Business
            Dim temp_oPartyBusiness As Object = Nothing
            m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_oPartyBusiness, "bSIRParty.Business", gPMConstants.PMGetViaClientManager)
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

    ' PUBLIC Methods (Begin)

    Public Function AddQASAddress(ByVal v_sAdd1 As String, ByVal v_sAdd2 As String, ByVal v_sAdd3 As String, ByVal v_sAdd4 As String, ByVal v_sPostCode As String) As Boolean
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


            m_lReturn = m_oQASAddress.EditAdd(lRow:=lBusinessDataID, vAddress1:=v_sAdd1, vAddress2:=v_sAdd2, vAddress3:=v_sAdd3, vAddress4:=v_sAdd4, vPostalCode:=v_sPostCode, vCountryID:=m_iDefaultCountryID)

            Debug.WriteLine(m_oQASAddress.addresscnt)

            m_oQASAddress.Update()

            'add address to list view
            oItem = lvwAddresses.Items.Add(v_sPostCode)
            ListViewHelper.GetListViewSubItem(oItem, 1).Text = "Correspondence Address"
            ListViewHelper.GetListViewSubItem(oItem, 2).Text = v_sAdd1
            ListViewHelper.GetListViewSubItem(oItem, 3).Text = v_sAdd2
            ListViewHelper.GetListViewSubItem(oItem, 4).Text = v_sAdd3
            ListViewHelper.GetListViewSubItem(oItem, 5).Text = v_sAdd4

            oItem.Tag = m_oQASAddress.addresscnt

            'tidy up
            m_oQASAddress = Nothing

            Return result

        Catch excep As System.Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddQASAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddQASAddress", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            '    Me.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACInterfaceTitle, _
            ''        iDataType:=PMResString)
            '
            '    ' Check for an error.
            '    If (Me.Caption = "") Then
            '        ' Failed to get data from the resource file.
            '        DisplayCaptions = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Unable to retrieve data from the resource file." & Chr(10) & _
            ''            "Please check the file exists and the correct captions are available", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="DisplayCaptions"
            '
            '        Exit Function
            '    End If
            '
            '    cmdOK.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACOKButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdCancel.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACCancelButton, _
            ''        iDataType:=PMResString)
            '
            '    cmdHelp.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lID:=ACHelpButton, _
            ''        iDataType:=PMResString)


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 4, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 18/11/2002 : PS005 : Added

            SSTabHelper.SetTabCaption(tabMainTab, 5, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle6, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'ECK 2005 Roadmap

            SSTabHelper.SetTabCaption(tabMainTab, 6, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle7, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab 1


            lblIDReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGroupCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'PN15600
            '    lblIsAgent.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACIsAgent, _
            ''        iDataType:=PMResString)

            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGroupName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            'eck 2005 roadmap
            '    cmdProspect.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACProspect, _
            ''        iDataType:=PMResString)

            lblGroupType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGroupType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC 04/08/00 New Field

            lblMainContact.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainContact, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            fraCharityDetails.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCharityDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCharity.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCharity, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblMembers.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNoOfMembers, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblCharityNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCharityNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC101204
            If m_sUnderwritingOrAgency = "U" Then


                fraAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLeadAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Else


                fraAgent.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThirdParty, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                cmdCurrentAgent.Text = "Third Party ..."
                lblAgentReference.Text = "Third Party Ref.:"

            End If


            cmdAgentLookUp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCode1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblAgentName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            fraConsultant.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConsultant, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            cmdConsultantLookup.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCode2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblConsultantName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'DC 20/06/00
            'New Label
            'PN15600 Removed
            '    lblIsProspect.Caption = iPMFunc.GetResData( _
            ''        iLangID:=g_iLanguageID%, _
            ''        lId:=ACIsProspect, _
            ''        iDataType:=PMResString)


            ' Tab 2


            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab 3

            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab 4

            cmdAddConv.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteConv.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditConv.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCCJ.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCGC, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab 5

            lblCurrency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCurrency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblServicelevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACServiceLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCreditCard.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCreditCardType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblCreditCard.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCreditCardType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblReminderType.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReminderType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTermsOfPayment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTermsOfPayment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAssociates.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAssociates, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblArea.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACArea, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFileCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFileCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTurnover.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTurnover, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab 6

            ' RAW 18/11/2002 : PS005 : Added

            cmdAddLoyaltyScheme.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAdd, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 18/11/2002 : PS005 : Added

            cmdDeleteLoyaltyScheme.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 18/11/2002 : PS005 : Added

            cmdEditLoyaltyScheme.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEdit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' RAW 18/11/2002 : PS005 : Added

            fraLoyaltySchemes.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLoyaltySchemes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Tab 6 - End


            '    sAddressUsage = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddressListUsage, _
            ''          iDataType:=PMResString)
            '
            '    sAddressLine1 = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddressListLine1, _
            ''          iDataType:=PMResString)
            '
            '    sAddressLine2 = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddressListLine2, _
            ''          iDataType:=PMResString)
            '
            '    sAddressLine3 = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddressListLine3, _
            ''          iDataType:=PMResString)
            '
            '    sAddressLine4 = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddressListLine4, _
            ''          iDataType:=PMResString)
            '
            '    sPostCode = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACAddressListPostCode, _
            ''          iDataType:=PMResString)
            'sj 18/06/2002 - start

            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTradingName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTradingName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSubBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSubBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAlternativeIdentifier.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAlternativeIdentifier, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblLoyaltyNumber.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLoyaltyNumber, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            chkFeeClient.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=AC_CAPTION_FEECLIENT, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'sj 18/06/2002 - end
            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    lvwAddresses.Columns.Item(0).Text = sPOSTCODE_TITLE
                    lvwAddresses.Columns.Item(1).Text = sADDR_USAGE_TITLE
                    lvwAddresses.Columns.Item(2).Text = sADDR_LINE1_TITLE
                    lvwAddresses.Columns.Item(3).Text = sADDR_LINE2_TITLE
                    lvwAddresses.Columns.Item(4).Text = sADDR_LINE3_TITLE
                    lvwAddresses.Columns.Item(5).Text = sADDR_LINE4_TITLE
                    lvwAddresses.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1000))
                    lvwAddresses.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(2200))
                Case Else
                    lvwAddresses.Columns.Item(0).Text = sADDR_USAGE_TITLE
                    lvwAddresses.Columns.Item(1).Text = sADDR_LINE1_TITLE
                    lvwAddresses.Columns.Item(2).Text = sADDR_LINE2_TITLE
                    lvwAddresses.Columns.Item(3).Text = sADDR_LINE3_TITLE
                    lvwAddresses.Columns.Item(4).Text = sADDR_LINE4_TITLE
                    lvwAddresses.Columns.Item(5).Text = sPOSTCODE_TITLE
                    lvwAddresses.Columns.Item(5).Width = CInt(VB6.TwipsToPixelsX(1000))
                    lvwAddresses.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(2200))

            End Select
            'sj 12/06/2002 - start
            If m_bIsNRMA Then
                m_lReturn = PartyFunc.SetAddressHeaders(r_oAddresses:=lvwAddresses, v_sPostCode:=sPOSTCODE_TITLE, v_sAddressUsage:=sADDR_USAGE_TITLE)
            End If
            'sj 12/06/2002 - end
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            'Text Boxes

            If m_bDuplicateClientIdentification Then
                'txtIDReference.Enabled = False
                'lblIDReference.FontBold = False
                'lblIDReference.ForeColor = QBColor(7)

                'Reference must be entered
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            Else
                'Reference must be entered
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC 04/08/00 Main Contact
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMainContact, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Agent Ref
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Consultant Ref
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtConsultantRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Charity Number
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCharityNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Offices
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtMembers, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Employees
            'mk 16/9 1208 m_lReturn = m_oFormFields.AddNewFormField( _
            'ctlControl:=txtEmployees, _
            'lFieldType:=PMLong, _
            'lFormat:=PMFormatString, _
            'lMandatory:=PMNonMandatory)


            'CCJ
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCCJ, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Trading Since Date
            'MK 0916 1455    m_lReturn = m_oFormFields.AddNewFormField( _
            'ctlControl:=txtTradingSince, _
            'lFieldType:=PMDate, _
            'lFormat:=PMFormatDateLong, _
            'lMandatory:=PMNonMandatory)


            'File Code
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFileCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Previous Insurer
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            ' Previous Broker
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBrokerRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            'COMBO BOXES
            'Party Business must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboGroupType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Party Trade must be entered
            'MK 0916 1647   m_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=cboGroupType, _
            'lFieldType:=PMString, _
            'lFormat:=PMFormatString, _
            'lMandatory:=PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Currency is now defaulted into the database somewhere down the
            'line if not entered. Don't know where.

            '    'Currency must be entered
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                            ctlControl:=cboCurrency, _
            ''                            lFieldType:=PMString, _
            ''                            lFormat:=PMFormatString, _
            ''                            lMandatory:=PMMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    'Payment Method
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                            ctlControl:=cboPaymentMethod, _
            ''                            lFieldType:=PMString, _
            ''                            lFormat:=PMFormatString, _
            ''                            lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    'Reminder Type
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                            ctlControl:=cboReminderType, _
            ''                            lFieldType:=PMString, _
            ''                            lFormat:=PMFormatString, _
            ''                            lMandatory:=PMMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    'Service Level
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                            ctlControl:=cboServiceLevel, _
            ''                            lFieldType:=PMString, _
            ''                            lFormat:=PMFormatString, _
            ''                            lMandatory:=PMMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    'Credit Card
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                            ctlControl:=cboCreditCard, _
            ''                            lFieldType:=PMString, _
            ''                            lFormat:=PMFormatString, _
            ''                            lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If
            '
            '    'Terms of Payment
            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            ''                            ctlControl:=cboTermsOfPayment, _
            ''                            lFieldType:=PMString, _
            ''                            lFormat:=PMFormatString, _
            ''                            lMandatory:=PMNonMandatory)
            '
            '    If m_lReturn <> PMTrue Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            ' txtAgentReference
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            ' hidden date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtHiddenDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' hidden currency
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtHiddenCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'FSA Phase III
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTobLetter, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'FSA Phase III End

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboTurnover, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim sTemp As String = ""

        '2005 Screen Layout

        Dim sFormattedCurrency As String = ""
        Dim cConvertedAmount As Decimal
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
            'SP090998

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC 25/09/00
            m_sOldIdReference = m_sShortName
            m_sOldResolvedName = m_sName


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC 04/08/00
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMainContact, vControlValue:=m_sMainContactDesc)



            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sTemp = m_sAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")


            'developer guide no. 26(latest guide)
            lblAgentNameLabel.Text = sTemp
            '2005 Client Screen Layout Changes
            '    m_lReturn = m_oCurrencyConvert.GetBaseCurrency( _
            ''        v_lCompanyID:=m_iPartySourceId, _
            ''        r_iBaseCurrencyID:=iBaseCurrencyID)


            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cClientBalance, cCurrencyAmount:=cConvertedAmount)

            m_cClientBalance = cConvertedAmount


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cClientBalance, vFormattedCurrency:=sFormattedCurrency)


            'developer guide no. 26 (guide)
            lblClientBalanceLabel.Text = sFormattedCurrency


            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cYearToDateTurnover, cCurrencyAmount:=cConvertedAmount)

            m_cYearToDateTurnover = cConvertedAmount


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cYearToDateTurnover, vFormattedCurrency:=sFormattedCurrency)


            'developer guide no. 26 (guide)
            lblYearToDateTurnoverLabel.Text = sFormattedCurrency


            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cLastYearTurnover, cCurrencyAmount:=cConvertedAmount)

            m_cLastYearTurnover = cConvertedAmount


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cLastYearTurnover, vFormattedCurrency:=sFormattedCurrency)


            'developer guide no. 26 (guide)
            lblLastYearTurnoverLabel.Text = sFormattedCurrency
            '2005 End


            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sTemp = m_sConsultantName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            'developer guide no. 26 (guide)
            lblConsultantNamelabel.Text = sTemp

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCharityNumber, vControlValue:=m_sCharityNumber)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MK 0916 1516    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtTradingSince, _
            'vControlValue:=m_dtTradingSinceDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtMembers, vControlValue:=m_lNumberofMembers)

            txtMembers.Text = CStr(m_lNumberofMembers)
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'MK 0916 1650    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtEmployees, _
            'vControlValue:=m_lNoOfEmployees)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFileCode, vControlValue:=m_sFileCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCCJ, vControlValue:=m_lCCJs)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 13/06/2002 - start
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
            'sj 13/06/2002 - end

            'ABI lookup Combos
            cboServiceLevel.Text = m_sServiceLevel 'MK 991013 m_lServiceLevelId
            ddPaymentMethod.Text = m_sPaymentMethodCode
            cboCreditCard.Text = m_sCreditCardCode



            If ddPaymentMethod.Text = CreditCard Or ddPaymentMethod.Text = DebitCard Then
                cboCreditCard.Text = m_sCreditCardCode
                lblCreditCard.Visible = True
                cboCreditCard.Visible = True
            Else
                lblCreditCard.Visible = False
                cboCreditCard.Visible = False
                cboCreditCard.Text = ""
            End If
            chkCharity.CheckState = m_iIsRegisteredCharity
            chkAgent.CheckState = m_iIsAlsoAgent
            chkProspect.CheckState = m_iIsProspect

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges Then

                m_lReturn = m_oBusiness.GetFutureDatedAddresses(r_vFutureDatedAddresses:=m_vFutureDatedAddresses, v_vPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface")
                    Return result
                End If
            End If
            'sj 23/07/2002 - end

            'mkw090204 PN10359. Add TPS, Emps and Mailshot fields. START
            chkTPS.CheckState = m_iTPSind
            chkMailshot.CheckState = m_iMailshot
            chkeMPS.CheckState = m_iEMPSind
            'mkw090204 PN10359. Add TPS, Emps and Mailshot fields. END
            'FSA Phase III
            'developer guide no. 113(latest guide)
            If m_dtTobLetter = CDate(#12/29/1899#) Then
                txtTobLetter.Text = ""
            Else
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTobLetter, vControlValue:=m_dtTobLetter)
            End If
            'FSA Phase IIIEnd

            'Fill the contact grid
            PopulateContacts()

            'Fill the address grid
            PopulateAddresses()

            'Fill the Convictions list view
            PopulateConvictions()

            'DC 28/06/00
            'Fill the Correspondence Types combo box
            PopulateCorrespondenceTypes()

            '    Fill the PartyLoyaltyScheme list view
            PopulateLoyaltySchemes() 'RAW 18/11/2002 : PS005 : Added

            'Party Bank Details
            LoadPartyBankControl()

            'frmInterface.Caption = "Group Client: " & m_sShortName & " " & m_sMainPostCode
            cboTurnover.ItemId = gPMFunctions.NullToLong(m_vTurnover)

            chkFeeClient.CheckState = IIf(gPMFunctions.ToSafeLong(m_vIsFeeClient) = 1, CheckState.Checked, CheckState.Unchecked)

            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage

            ' Log client viewed event
            If m_lPartyCnt > 0 Then
                m_lReturn = LogClientViewedEvent(m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error but continue processing
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to log client viewed event", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
                End If
            End If

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

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1

            ' Check the task.
            Select Case (m_iTask)

                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vPartyGroupTypeID:=m_lPartyGroupTypeId,
                                                    vIsRegisteredCharity:=m_iIsRegisteredCharity, vCharityNumber:=m_sCharityNumber,
                                                    vNumberOfMembers:=m_lNumberofMembers, vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sResolved,
                                                    vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt,
                                                    vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId,
                                                    vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId,
                                                    vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId,
                                                    vCCJs:=m_lCCJs, vSourceID:=m_iNewPartySourceId, vSeasonalGiftID:=m_vSeasonalGiftId,
                                                    vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vRenewalStopCodeId:=m_vRenewalStopCodeId,
                                                    vSwiftPartyID:=m_lSwiftPartyID, vLoyaltyNumber:=m_sLoyaltyNumber, vAlternativeIdentifier:=m_sAlternativeIdentifier,
                                                    vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter, vTPSInd:=m_iTPSind,
                                                    vMailshot:=m_iMailshot, vEMPSind:=m_iEMPSind, vTurnover:=m_vTurnover, vIsFeeClient:=m_vIsFeeClient)


                Case gPMConstants.PMEComponentAction.PMEdit
                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vPartyGroupTypeID:=m_lPartyGroupTypeId,
                                                       vIsRegisteredCharity:=m_iIsRegisteredCharity, vCharityNumber:=m_sCharityNumber,
                                                       vNumberOfMembers:=m_lNumberofMembers, vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sResolved,
                                                       vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt,
                                                       vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId,
                                                       vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId,
                                                       vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId,
                                                       vCCJs:=m_lCCJs, vSeasonalGiftID:=m_vSeasonalGiftId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId,
                                                       vRenewalStopCodeId:=m_vRenewalStopCodeId, vSourceID:=m_iNewPartySourceId, vPartyID:=m_lNewPartyId,
                                                       vSwiftPartyID:=m_lSwiftPartyID, vLoyaltyNumber:=m_sLoyaltyNumber, vAlternativeIdentifier:=m_sAlternativeIdentifier,
                                                       vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId, vTobLetter:=m_dtTobLetter, vTPSInd:=m_iTPSind,
                                                       vMailshot:=m_iMailshot, vEMPSind:=m_iEMPSind, vTurnover:=m_vTurnover, vIsFeeClient:=m_vIsFeeClient)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPartyGroupType, ctlLookup:=cboGroupType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupArea, ctlLookup:=cboArea)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupProspectStatus, ctlLookup:=cboProspectingStatus)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupReminderType, ctlLookup:=cboReminderType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupServiceLevel, ctlLookup:=cboServiceLevel)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Although we don't use it, it's the easiest way to store these lookup values
            '    m_lReturn& = GetLookupDetails( _
            'sLookupTable:=SIRLookupPolicyType, _
            'ctlLookup:=cboPolicyType)
            'Tomo211299
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRiskGroup, ctlLookup:=cboPolicyType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupSeasonalGift, ctlLookup:=cboSeasonalGift)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Tomo060700
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalStopCode, ctlLookup:=cboRenewalStopCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'eck 2005 Roadmap Moved from Populate prospect
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupStrengthCode, ctlLookup:=cboStrengthCode)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPFFrequency, ctlLookup:=cboTermsOfPayment)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            SelectcboItem(cboTermsOfPayment, CInt(m_iPaymentTermId))

            'eck010900 Get Branches
            m_lReturn = GetBranchDetails()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 11/06/2002 - start
            m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=cboBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_lSubBranchId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DJM 13/01/2004 : Copied from 1.8.5 issue 5885.
            If m_bValidateAlternativeIdentifier Then
                'Get number validation scripts

                m_lReturn = m_oBusiness.GetNumberValidationScripts(v_sBranchPrefix:=m_sBranchPrefix, r_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, r_sAlternativeIdentifierScript:=m_sAlternativeIdentifierScript)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oBusiness.GetNumberValidationScripts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'sj 11/06/2002 - end

            'sj 21/08/2002 - start
            'Get relationship type lookups

            m_lReturn = m_oBusiness.GetRelationshipTypeLookups(vRelationships:=m_vRelationships)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the relationship type lookups", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
            End If

            cmdAssociates.Enabled = Not (False)
            'sj 21/08/2002 - end

            '***********************
            'blacklist reason id
            If m_sUnderwritingOrAgency = "U" And m_bSystemOptionClientBlacklistingInForce Then

                m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupBlackListReason, ctlLookup:=cboBlackListReason, sInitialOption:="(not blacklisted)")

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                SelectcboItem(cboBlackListReason, CInt(m_vBlackListReasonId))

            End If
            '***********************


            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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

                        oListItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim(), "")

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

                        oListItem = lvwAddresses.Items.Add(sAddressUsage, "")

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

        Dim oListItem As ListViewItem


        Try

            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If
            lvwContacts.Items.Clear()


            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1

                oListItem = lvwContacts.Items.Add(CStr(m_vContacts(1, i)).Trim(), "")

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

        'Const ConvictionImage As String = "ConvictionImage"    ''Unused Local Variable
        Dim oListItem As ListViewItem

        Try

            If Not Information.IsArray(m_vConvictions) Then
                Exit Sub
            End If

            lvwConvictions.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vConvictions.GetLowerBound(1) To m_vConvictions.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1

                oListItem = lvwConvictions.Items.Add(CStr(m_vConvictions(2, i)).Trim(), "")

                ' Assign details to other the columns
                ' Date
                '990922 mk changed txtdate to txthiddendate
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(m_vConvictions(3, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()

                ' Description
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vConvictions(4, i)).Trim()

                ' Fine
                '990922 mk changed txtcurrency to txthiddencurrency
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenCurrency, vControlValue:=CStr(m_vConvictions(5, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text.Trim()

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
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(m_vLoyaltySchemes(PMBLoyaltyStartDate, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtHiddenDate.Text.Trim()

                ' EndDate
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(m_vLoyaltySchemes(PMBLoyaltyEndDate, i)).Trim())

                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenDate.Text.Trim()

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
        Dim oListItem, oListItem2 As ListViewItem

        Dim sAddressUsage, sChangeCode As String

        Dim sPartyCode, sMsg As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '********************************************************************************
            'MKR 12/10/2004 PN 6021 - Restricting Entry of Spl. Chars in GroupCode   --Start

            'PN48908
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd And m_bValidateNumberingScheme Then 'Not completely validate numbering scheme
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            txtIDReference.Text = txtIDReference.Text.Trim()
            If txtIDReference.Text.Trim() = "" Then
                MessageBox.Show("Must have Client Code", "Group Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                If txtIDReference.Enabled Then
                    txtIDReference.Focus()
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If txtIDReference.Text.IndexOf("'"c) >= 0 OrElse txtIDReference.Text.IndexOf("’"c) >= 0 Then
                MessageBox.Show("' (Apostrophes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                If txtIDReference.Enabled Then
                    txtIDReference.Focus()
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If txtIDReference.Text.IndexOf("|"c) >= 0 Then
                MessageBox.Show("| (Pipes) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                If txtIDReference.Enabled Then
                    txtIDReference.Focus()
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If txtIDReference.Text.IndexOf(","c) >= 0 Then
                MessageBox.Show(", (Commas) are not Allowed in Client Code", Application.ProductName)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                If txtIDReference.Enabled Then
                    txtIDReference.Focus()
                End If
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'MKR 12/10/2004 PN 6021 - Restricting Entry of Spl. Chars in ClientCode   --End
            '********************************************************************************

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

            iMainAddresses = 0

            'Count how many addresses are main address
            If lvwAddresses.Items.Count > 0 Then
                For i As Integer = 1 To lvwAddresses.Items.Count
                    oListItem = lvwAddresses.Items.Item(i - 1)

                    'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                    Select Case (m_sDefaultCountryCode.Trim())
                        Case "GBR"
                            sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
                        Case Else
                            sAddressUsage = oListItem.Text.Trim()
                    End Select

                    For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                        'RWH(24/07/2000)
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
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    Return result
                Case 1
                    'Yes
                Case Else
                    'No.
                    MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", "Address Usage Validation", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    Return result
            End Select

            'Now Ensure addresses are not used twice
            'EK 17/11/99
            '    If (m_lAddressCount < 2) Then
            If lvwAddresses.Items.Count < 2 Then
                'less than 2 addresses so cant have duplicates
                Return result
            End If

            bDuplicate = False

            'Check for duplicates
            'Need to find out how to calculate no of lines in the grid
            'EK 17/11/99
            '    For i = 1 To (m_lAddressCount)
            For i As Integer = 1 To (lvwAddresses.Items.Count)
                oListItem = lvwAddresses.Items.Item(i - 1)
                If CBool(CStr(Convert.ToString(oListItem.Tag) <> "").Trim()) Then
                    lAddressCnt = Convert.ToString(oListItem.Tag)
                    'EK 17/11/99
                    '            For j = (i + 1) To m_lAddressCount
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

            'FSA Phase III

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vSourceID:=m_iPartySourceId, vPartyID:=m_lPartyId, vPartyGroupTypeID:=m_lPartyGroupTypeId,
                                            vIsRegisteredCharity:=m_iIsRegisteredCharity, vCharityNumber:=m_sCharityNumber,
                                            vNumberOfMembers:=m_lNumberofMembers, vShortName:=m_sShortName, vResolvedName:=m_sResolved,
                                            vName:=m_sName, vIsAlsoAgent:=m_iIsAlsoAgent, vIsProspect:=m_iIsProspect, vAgentCnt:=m_lAgentCnt,
                                            vConsultantCnt:=m_lConsultantCnt, vFileCode:=m_sFileCode, vCurrencyID:=m_iCurrencyId,
                                            vPaymentMethodCode:=m_sPaymentMethodCode, vReminderTypeId:=m_lReminderTypeID, vAreaId:=m_iAreaId,
                                            vServiceLevelId:=m_lServiceLevelId, vCreditCardCode:=m_sCreditCardCode, vPaymentTermCode:=m_iPaymentTermId, vCCJs:=m_lCCJs,
                                            vSeasonalGiftID:=m_vSeasonalGiftId, vCorrespondenceTypeId:=m_vCorrespondenceTypeId,
                                            vRenewalStopCodeId:=m_vRenewalStopCodeId, vSwiftPartyID:=m_lSwiftPartyID, vLoyaltyNumber:=m_sLoyaltyNumber,
                                            vAlternativeIdentifier:=m_sAlternativeIdentifier, vTradingName:=m_sTradingName, vSubBranchId:=m_lSubBranchId,
                                            vTobLetter:=m_dtTobLetter, vTPSInd:=m_iTPSind, vMailshot:=m_iMailshot, vEMPSind:=m_iEMPSind, vTurnover:=m_vTurnover,
                                            vIsFeeClient:=m_vIsFeeClient)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
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

            'Get additional details required for display that not stored on this
            'record

            m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=m_lAgentCnt, vAgentref:=m_sAgentRef, vAgentName:=m_sAgentName, vConsultantCnt:=m_lConsultantCnt, vConsultantRef:=m_sConsultantRef, vConsultantName:=m_sConsultantName, vInsurerCnt:=m_vPreviousInsurerCnt, vInsurerRef:=m_sInsurerRef, vInsurerName:=m_sInsurerName, vBrokerCnt:=m_vPreviousBrokerCnt, vBrokerRef:=m_sBrokerRef, vBrokerName:=m_sBrokerName, vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'DC 04/08/00

            m_lReturn = m_oBusiness.GetMainContact(lMainContactCnt:=m_lMainContactCnt, sMainContactDesc:=m_sMainContactDesc)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the main contact from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'DC 03/05/00
            'Get relationship type lookups
            'sj 21/08/2002 - start
            '    m_lReturn& = m_oBusiness.GetRelationshipTypeLookups(vRelationships:=m_vRelationships)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        BusinessToData = PMFalse
            '
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to retrieve the relationship ", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="BusinessToData"
            '    End If
            '
            '    If (IsMissing(m_vRelationships)) Then
            '        cmdAssociates.Enabled = False
            '    Else
            '        cmdAssociates.Enabled = True
            '    End If
            'sj 21/08/2002 - end

            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresse type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get contacts for the party

            m_lReturn = m_oBusiness.GetContactDetails(vContacts:=m_vContacts)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the contact details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get convictions for the Party

            m_lReturn = m_oBusiness.GetConvictionDetails(vPartyCnt:=m_lPartyCnt, vConviction:=m_vConvictions)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the conviction details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'DC 28/06/00
            'Get Correspondence Types

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
            '2005End

            ' {* USER DEFINED CODE (End) *}

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
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer

        Dim result As Integer = 0
        Dim sMsg As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            ' {* USER DEFINED CODE (Begin) *}


            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))

            ' If this is an add then check for duplicate references

            'DC 04/08/00

            m_sMainContactDesc = CStr(m_oFormFields.UnformatControl(ctlControl:=txtMainContact))


            m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))
            m_sResolved = m_sName
            If CurrentResolvedName <> "" Then
                If CurrentResolvedName <> m_sResolved Then
                    If m_sOldResolvedName <> m_sResolved Then
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

            m_sCharityNumber = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCharityNumber))

            m_lNumberofMembers = CInt(m_oFormFields.UnformatControl(ctlControl:=txtMembers))


            m_lCCJs = CInt(m_oFormFields.UnformatControl(ctlControl:=txtCCJ))

            m_sFileCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtFileCode))

            'ABI Combos
            m_sGroupTypeCode = cboGroupType.Text

            m_sPaymentMethodCode = ddPaymentMethod.Text
            m_sCreditCardCode = cboCreditCard.Text
            m_sServiceLevel = cboServiceLevel.Text

            'Lookup Combos
            'RWH(09/06/2000) - Protect combos against no selection.
            If cboGroupType.SelectedIndex <> -1 Then
                m_lPartyGroupTypeId = VB6.GetItemData(cboGroupType, cboGroupType.SelectedIndex)
            Else
                m_lPartyGroupTypeId = 0
            End If
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

            'Check Boxes
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

            If chkCharity.CheckState = CheckState.Unchecked Then
                m_iIsRegisteredCharity = CheckState.Unchecked
            Else
                m_iIsRegisteredCharity = CheckState.Checked
            End If

            m_vTurnover = cboTurnover.ItemId

            'sj 13/06/2002 - start
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
            'sj 17/07/2002 - start
            'Make sure we have a sub branch selected
            If cboSubBranch.SelectedIndex >= 0 Then
                m_lSubBranchId = CInt(Conversion.Val(CStr(VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex))))
            Else
                m_lSubBranchId = 0
            End If
            'm_lSubBranchId = Val(cboSubBranch.ItemData(cboSubBranch.ListIndex))
            'sj 13/06/2002 - end

            'eck010900
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
                'MKW180703 PN5425
            End If


            'Validation of following not required if we are cancelling out

            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                'Get party count for Agent (if valid ref supplied)
                If (txtAgentRef.Text.Trim() <> "") And (m_bVerifyAgentCnt) Then


                    m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtAgentRef.Text.Trim(), vPartyCnt:=m_lAgentCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lAgentCnt = 0 Then

                        'DC101204
                        If m_sUnderwritingOrAgency = "U" Then


                            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            MessageBox.Show("Agent Missing", Application.ProductName)

                        Else


                            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThirdPartyMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            MessageBox.Show("Third Party Missing", Application.ProductName)

                        End If

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

                If cboSeasonalGift.SelectedIndex <> -1 Then
                    m_vSeasonalGiftId = VB6.GetItemData(cboSeasonalGift, cboSeasonalGift.SelectedIndex)
                Else

                    m_vSeasonalGiftId = Nothing
                End If

                'Get party count for Consultant (if valid ref supplied)
                If (txtConsultantRef.Text.Trim() <> "") And (m_bVerifyConsultantCnt) Then


                    m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtConsultantRef.Text.Trim(), vPartyCnt:=m_lConsultantCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lConsultantCnt = 0 Then

                        'DC101204
                        If m_sUnderwritingOrAgency = "U" Then


                            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            MessageBox.Show("Agent Missing", Application.ProductName)

                        Else


                            sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACThirdPartyMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                            MessageBox.Show("Third Party Missing", Application.ProductName)

                        End If

                        MessageBox.Show(sMsg, "Group Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

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

            'DC 28/06/00
            If cboCorrespondenceType.SelectedIndex <> -1 Then
                m_vCorrespondenceTypeId = VB6.GetItemData(cboCorrespondenceType, cboCorrespondenceType.SelectedIndex)
            Else

                m_vCorrespondenceTypeId = Nothing
            End If

            'Tomo060700
            If cboRenewalStopCode.SelectedIndex <> -1 Then
                m_vRenewalStopCodeId = VB6.GetItemData(cboRenewalStopCode, cboRenewalStopCode.SelectedIndex)
            Else

                m_vRenewalStopCodeId = 0
            End If

            'mkw090204 PN10359. Add TPS, Emps and Mailshot fields. START
            If chkTPS.CheckState = CheckState.Unchecked Then
                m_iTPSind = CheckState.Unchecked
            Else
                m_iTPSind = CheckState.Checked
            End If
            If chkMailshot.CheckState = CheckState.Unchecked Then
                m_iMailshot = CheckState.Unchecked
            Else
                m_iMailshot = CheckState.Checked
            End If
            If chkeMPS.CheckState = CheckState.Unchecked Then
                m_iEMPSind = CheckState.Unchecked
            Else
                m_iEMPSind = CheckState.Checked
            End If
            'mkw090204 PN10359. Add TPS, Emps and Mailshot fields. END
            'FSA Phase III
            If txtTobLetter.Text.Trim().Length = 0 Then
                'developer guide no. 113(Latest guide)
                m_dtTobLetter = CDate(#12/29/1899#)
                'm_dtTobLetter = CDate("00:00:00")
            Else
                m_dtTobLetter = CDate(txtTobLetter.Text)
            End If
            'FSA Phase IIIEnd

            '***************************
            ' get the party tax details from the party tax control
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage
            '***************************

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

            If m_sUnderwritingOrAgency = "A" Then
                m_vIsFeeClient = CStr(IIf(chkFeeClient.CheckState = CheckState.Checked, 1, 0))
            Else

                m_vIsFeeClient = Nothing
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
        Dim bNZConfig As Boolean
        Dim vValue As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AR20061107 - Get New Zealand product option

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTNewZealandConfiguration, v_vBranch:=1, r_vUnderwriting:=CStr(vValue))
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

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            tabMainTab.Visible = True
            '2005 Roadmap
            'tabProspecting.Visible = False

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '2005 Roadmap
            'cmdProspect.Enabled = False
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False
            cmdEditConv.Enabled = False
            cmdDeleteConv.Enabled = False
            cmdEditPol.Enabled = False
            cmdDeletePol.Enabled = False
            cmdEditLoyaltyScheme.Enabled = False
            cmdDeleteLoyaltyScheme.Enabled = False

            cboCreditCard.Visible = False
            lblCreditCard.Visible = False

            chkAgent.CheckState = CheckState.Unchecked
            chkProspect.CheckState = CheckState.Unchecked

            '2005 Raodmap
            'cmdProspect.Enabled = True

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' CF020699 - Added list view functions

            lvwAddresses.FullRowSelect = True
            lvwAddresses.HideSelection = False
            lvwCampaigns.FullRowSelect = True
            lvwCampaigns.HideSelection = False
            lvwContacts.FullRowSelect = True
            lvwContacts.HideSelection = False
            lvwConvictions.FullRowSelect = True
            lvwConvictions.HideSelection = False
            lvwPolicies.FullRowSelect = True
            lvwPolicies.HideSelection = False
            lvwLoyaltySchemes.FullRowSelect = True
            lvwLoyaltySchemes.HideSelection = False

            'Get Correspondence Types
            m_lReturn = m_oBusiness.GetCorrespondenceTypes(vCorrespondenceTypes:=m_vCorrespondenceTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If

            PopulateCorrespondenceTypes()

            ' Should we show sub branch?
            If m_sUnderwritingOrAgency = "A" Then
                lblSubBranch.Visible = m_bShowSubBranchID
                cboSubBranch.Visible = m_bShowSubBranchID
            End If

            'sj 12/06/2002 - start
            If m_bIsNRMA Then
                txtLoyaltyNumberPrefix.Visible = True
                txtLoyaltyNumber.MaxLength = 10
                'Right justify
                txtAlternativeIdentifier.TextAlign = HorizontalAlignment.Right
                cboBranch.Enabled = False
                txtAlternativeIdentifier.Text = PartyFunc.m_kBlankAlternativeIdentifier
            Else
                txtLoyaltyNumberPrefix.Visible = False
                txtLoyaltyNumber.Left = cboReminderType.Left
                txtLoyaltyNumber.Width = cboReminderType.Width
            End If

            If m_bMultiTreeAccounting Then
                'Branch field read only
                cboBranch.Enabled = False
            End If

            fraBlackList.Visible = False

            If m_sUnderwritingOrAgency = "U" Then
                If m_bSystemOptionClientBlacklistingInForce Then
                    fraBlackList.Visible = True
                End If
            End If

            chkFeeClient.Visible = (m_sUnderwritingOrAgency = "A")
            chkFeeClient.CheckState = CheckState.Unchecked

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                'AR20061107 - Default Domiciled for Tax to ticked for NZ market
                uctPartyTax1.IsDomiciledForTax = bNZConfig
                cboTurnover.ListIndex = -1
            End If

            'Maintain Client Code
            'PN: 47806
            m_lReturn = SetClientCodeCntl()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If

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
            m_ctlTabFirstLast(ACControlStart, 0) = txtIDReference
            m_ctlTabFirstLast(ACControlEnd, 0) = txtCharityNumber
            m_ctlTabFirstLast(ACControlStart, 1) = lvwAddresses
            m_ctlTabFirstLast(ACControlStart, 2) = lvwContacts
            m_ctlTabFirstLast(ACControlStart, 3) = lvwConvictions
            m_ctlTabFirstLast(ACControlStart, 4) = cboCurrency
            m_ctlTabFirstLast(ACControlStart, 5) = lvwLoyaltySchemes 'RAW 18/11/2002 : PS005 : Added
            m_ctlTabFirstLast(ACControlEnd, 5) = lvwLoyaltySchemes 'RAW 18/11/2002 : PS005 : Added
            m_ctlTabFirstLast(ACControlStart, 6) = txtAgentReference



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

                    'DC030401 changed lookup type to All Effective
                Case gPMConstants.PMEComponentAction.PMView
                    ' Get lookup values for viewing only.

                    m_lReturn = m_oBusiness.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupAllEffective, vTableArray:=m_vLookupValues, iLanguageID:=g_iLanguageID, vResultArray:=m_vLookupDetails)
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
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control, Optional ByRef bSecondary As Boolean = False, Optional ByRef sInitialOption As String = "") As Integer

        Dim result As Integer = 0
        Dim lRow, lRow2 As Integer
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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            'DJM 26/04/2002 : Clear the control first. This prevents duplicates in
            '                 the comboboxes when the apply button in clicked.

            'developer guide no. 9(Guide)
            Dim ctl1 As ComboBox

            If Not ctlLookup Is Nothing Then
                ctl1 = DirectCast(ctlLookup, ComboBox)
            Else
                Return gPMConstants.PMEReturnCode.PMError
            End If
            '-end
            ctl1.Items.Clear()

            If sInitialOption <> "" Then

                'ctlLookup.AddItem(sInitialOption, 0)
                'developer guide no. 
                Dim newIndex As Integer = ctl1.Items.Add(New VB6.ListBoxItem(sInitialOption, 0))

                ' NB:m_vLookupValues(ACValueID, lRow2&)) is the value from the party record
                ' if the value from the party matches the item
                If (m_vLookupValues(ACValueID, lRow)) = 0 Then
                    ' then select it.

                    'developer guide no. 153(Guide)
                    ctl1.SelectedIndex = newIndex
                End If
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            'developer guide no. 
            ctl1.Items.Add("")
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To (CInt(m_vLookupValues(ACValueStartPos, lRow)) + CInt(m_vLookupValues(ACValueNumber, lRow))) - 1
                ' Add the details to the control.

                'developer guide no. 
                Dim index As Integer = ctl1.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CLng(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If bSecondary Then
                    lRow2 = lRow + 1
                Else
                    lRow2 = lRow
                End If

                If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then

                    If CDbl(m_vLookupValues(ACValueID, lRow2)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then


                        'developer guide no. 
                        ctl1.SelectedIndex = index
                    End If

                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then

                'developer guide no. 
                ctl1.SelectedIndex = -1
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBranchDetails
    '
    ' Description: Gets all of the branch details
    '
    ' ***************************************************************** '
    Private Function GetBranchDetails() As Integer
        Dim result As Integer = 0
        Dim vSourceArray(,) As Object

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

        'Clear combo.
        cboBranch.Items.Clear()


        m_vSourceArray = vSourceArray

        'Populate branch combo.
        'developer guide no. 162(latest guide)
        For i As Integer = 0 To vSourceArray.GetUpperBound(1)

            'Add using branch description (3).
            Dim cboBranch_NewIndex As Integer = -1
            'developer guide no. 162(latest code)
            cboBranch_NewIndex = cboBranch.Items.Add(New VB6.ListBoxItem(Trim(vSourceArray(2, i)), CInt(vSourceArray(0, i))))
            'developer guide no. 162(latest code)
            If CInt(vSourceArray(0, i)) = m_iPartySourceId Then
                cboBranch.SelectedIndex = cboBranch_NewIndex

                'developer guide no. 162(latest code)
                m_sBranchPrefix = CStr(vSourceArray(1, i)).Trim()
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

        'cboCurrency.CurrencyID = m_iCurrencyId
        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            '        cboCurrency.CurrencyID = g_iCurrencyID
        Else
            ' otherwise use the currency id that was previously saved
            cboCurrency.RefreshList()
            cboCurrency.CurrencyId = m_iCurrencyId
        End If

        m_bUserMode = True

        Return result



        result = gPMConstants.PMEReturnCode.PMError

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

    Private Sub cboBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBranch.SelectionChangeCommitted
        If m_bUserMode Then
            m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=cboBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_lSubBranchId)
            If m_bShowSubBranchID Then
                cboSubBranch.SelectedIndex = -1
            Else
                cboSubBranch.SelectedIndex = 0
            End If
        End If

        m_lReturn = GetSourceBaseCurrency()
        m_iPartySourceId = VB6.GetItemData(cboBranch, cboBranch.SelectedIndex)

    End Sub

    Private Sub cboCurrency_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCurrency.GotFocus
        If SSTabHelper.GetTabVisible(tabMainTab, 4) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 4)
        End If
    End Sub
    ' PRIVATE Methods (End)

    Private Sub cboTurnover_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTurnover.GotFocus
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=cboTurnover)
    End Sub

    Private Sub cboTurnover_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTurnover.LostFocus
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=cboTurnover)
        End If
    End Sub

    Private Sub cboGroupType_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGroupType.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

        m_lReturn = m_oFormFields.GotFocus(cboGroupType)

    End Sub


    Private Sub cboGroupType_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGroupType.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(cboGroupType)
        End If
    End Sub


    Private Sub cboCreditCard_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.SelectionChangeCommitted

        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)

    End Sub

    Private Sub cboCreditCard_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.DropDown

        'Kevin Renshaw (CMG) 02/01/01 - issue 1645 the combo box refills only selected item
        'when event called

        'm_lReturn& = FillCombo(cboControl:=cboCreditCard, _
        ''                       bRefill:=True)

    End Sub

    Private Sub cboCreditCard_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboCreditCard.Enter
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

    Private Sub chkeMPS_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkeMPS.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 2)
        End If
    End Sub

    Private Sub cmdAddAd_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 1)
        End If
    End Sub

    Private Sub cmdAddPol_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddPol.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 6) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 6)
        End If
    End Sub

    'DC081200 changed from combo to dropdown
    Private Sub ddTermsOfPayment_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me)
    End Sub

    'DC081200 changed from combo to dropdown
    Private Sub ddTermsOfPayment_DropDown(ByVal Sender As Object, ByVal e As EventArgs)

        'm_lReturn& = FillCombo(cboControl:=cboCreditCard, _
        'bRefill:=True)

    End Sub
    'DC081200 changed from combo to dropdown
    Private Sub cboTermsOfPayment_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.ControlGotFocus(cboTermsOfPayment)
    End Sub

    'DC081200 changed from combo to dropdown
    'developer guide no to do to be checked at runtime
    Private Sub ddTermsOfPayment_KeyDown(ByVal eventSender As Object, ByVal eventArgs As PMListMgrDropdown.uctDropdown.KeyDownEventArgs)
        Dim KeyCode As Integer = eventArgs.KeyCode
        'Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim Shift As Integer = eventArgs.KeyCode \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(Me, KeyCode, Shift)
    End Sub

    'DC081200 changed from combo to dropdown
    Private Sub cboTermsOfPayment_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        m_lReturn = PMBGeneralFunc.ControlLostFocus(cboTermsOfPayment)
    End Sub

    Private Sub cboGroupType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGroupType.SelectedIndexChanged
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(cboGroupType)
    End Sub

    Private Sub cboGroupType_DropDown(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGroupType.DropDown

        '    m_lReturn& = FillCombo(cboControl:=cboGroupType, _
        'bRefill:=True)

    End Sub

    Private Sub cboGroupType_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles cboGroupType.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        m_lReturn = PMBGeneralFunc.FieldOnControlChange(cboGroupType, KeyCode, Shift)
    End Sub

    Private Sub chkProspect_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkProspect.CheckStateChanged

        'DC 20/06/00
        'Prospect checkbox no longer determines whether
        'Prospect Details can be entered
        '    If chkProspect.Value = 0 Then
        '        cmdProspect.Enabled = False
        '    End If
        '
        '    If chkProspect.Value = 1 Then
        '        cmdProspect.Enabled = True
        '    End If

    End Sub

    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click
        'RWH(11/07/2000) Altered to move PostCode from far left to far
        'right of ListView.


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

            'sj 12/06/2002 - start

            m_oAddress.IsNRMA = m_bIsNRMA
            'sj 12/06/2002 - end

            'sj 23/07/2002 - start

            m_oAddress.CorrespondanceAddressExists = CorrespondanceAddressExists()
            'sj 23/07/2002 - end

            'set the main postcode and reference

            m_oAddress.Reference = txtIDReference.Text
            '    m_lReturn& = ControlLostFocus(cboTermsOfPayment)


            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.Postcode = m_sMainPostCode


            'developer guide no. 162(latest guide)
            m_oAddress.CountryID = gPMFunctions.ToSafeLong(CStr(m_vSourceArray(3, cboBranch.SelectedIndex)))

            'developer guide no. 162(latest guide)
            m_oAddress.SourceID = gPMFunctions.ToSafeLong(CStr(m_vSourceArray(0, cboBranch.SelectedIndex))) 'PN64670

            m_oAddress.Task = m_iTask 'PN64670


            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    ' Add the data to the list view


                    oListItem = lvwAddresses.Items.Add(m_oAddress.PostalCode, "")
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


                    oListItem = lvwAddresses.Items.Add(m_oAddress.AddressUsageType, "")
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

            'developer guide no. 218(latest guide)
            oListItem.Tag = m_oAddress.AddressCnt


            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode

                m_iMainAddressIndex = m_oAddress.AddressUsageTypeID
                'Caption = "Group Client: " & m_sShortName & " " & m_sMainPostCode
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click
        'RWH(09/06/2000) - Tidy up object when leaving sub. This has to be done in a number of
        'places but means reference is re-initialised, and consequently all variables within
        'object, every time it is used. Avoids problems where other objects using the same
        'Contact object destroy their reference triggering a Terminate and the destruction of
        'some of our variables within Contact.


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

                    'RWH(09/06/2000) - Tidy up object.
                    If Not (m_oContact Is Nothing) Then
                        m_oContact = Nothing
                    End If
                    Exit Sub

                End If

            End If

            'set the main postcode and reference


            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(09/06/2000) - Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If


            'developer guide no. 218(latest guide)
            m_lReturn = m_oContact.ContactCnt


            m_oContact.Reference = txtIDReference.Text

            'developer guide no. 218(latest guide)
            m_oContact.PostCode = m_sMainPostCode


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(09/06/2000) - Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RWH(09/06/2000) - Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'Me.Refresh



            oListItem = lvwContacts.Items.Add(m_oContact.AreaCode, "")

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

            'developer guide no. 218(latest guide)
            oListItem.Tag = m_oContact.ContactCnt

            'RWH(09/06/2000) - Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RWH(09/06/2000) - Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If
            Exit Sub
        End Try
    End Sub

    Private Sub cmdAddConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddConv.Click


        Dim oListItem As ListViewItem

        Try
            'MSS210901 - Added UW code. Has more funtionality
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

            'Me.Refresh


            oListItem = lvwConvictions.Items.Add(m_oConviction.Code)

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

            ' Penalty Points

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oConviction.DrivingLicencePenaltyPoints

            ' Store the Address_cnt

            oListItem.Tag = m_oConviction.PartyConvictionID

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: Refresh
    '
    ' Description: What is this supposed to do?
    '
    ' ***************************************************************** '
    Public Overrides Sub Refresh()

    End Sub

    Private Sub cmdAddPol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddPol.Click


        Dim oListItem As ListViewItem

        'According to user guide this functionality is not supported more in Pure BO  (Confirmation by Mousumi Borah)
        Exit Sub

        If m_lPartyCnt < 1 Then
            MessageBox.Show("Must Save Client Details before adding Prospect Policies", Application.ProductName)
            Exit Sub
        End If

        Try

            'Create iProspectPolicy if not already done so
            If m_oProspectPolicy Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oProspectPolicy As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oProspectPolicy, sClassName:="iPMBProspectPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oProspectPolicy = temp_m_oProspectPolicy

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policies", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If



            m_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oProspectPolicy.PolicyTypeID = -1

            m_oProspectPolicy.PolicyTypeDescription = ""

            m_oProspectPolicy.RenewalDate = DateTime.Today

            m_oProspectPolicy.NoOfTimesQuoted = 0

            m_oProspectPolicy.TargetPremium = 0


            m_lReturn = m_oProspectPolicy.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            m_bChangedProspectPolicies = True

            Me.Refresh()

            ' Column 1


            oListItem = lvwPolicies.Items.Add(m_oProspectPolicy.PolicyTypeDescription.Trim(), "")


            oListItem.Tag = m_oProspectPolicy.PolicyTypeID

            ' Assign details to other the columns
            ' Column 2

            m_lReturn = m_oFormFields.FormatControl(txtHiddenDate, m_oProspectPolicy.RenewalDate)
            '990923 mk txtdate to txthiddenDATE
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oProspectPolicy.NoOfTimesQuoted
            ' Column 4


            m_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, m_oProspectPolicy.TargetPremium)
            '990923 mk txtcurrency to txthiddencurrency
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text

            'Column 5


            'developer guide no. 197(latest guide)
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = DateTime.FromOADate(m_oProspectPolicy.RenewalDate.ToOADate()).ToString("yyyyMMdd")
        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddPol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
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

            'Create iPartyLoyaltyScheme if not already done so
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

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.StartDate)

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtHiddenDate.Text.Trim()

            ' Column 4

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.EndDate)

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenDate.Text.Trim()

            ' Store the PartyLoyaltySchemeId

            oListItem.Tag = m_oPartyLoyaltyScheme.PartyLoyaltySchemeId

            'developer guide no. 234(latset guide)
            'Check if any item is focused on the listView, if yes, then remove the focus
            If (Not Information.IsNothing(lvwLoyaltySchemes.FocusedItem)) Then
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
        SSTabHelper.SetSelectedIndex(tabMainTab, 5)

    End Sub

    Private Sub cmdAgentLookUp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentLookUp.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try




            'Developer Guide 98
            'm_lReturn = SelectParty(vPartyCnt:=CInt(vCnt), vShortName:=CStr(vShortName), vName:=CStr(vName), vSpecialParty:="AG")
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="AG")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtAgentRef.Tag = CStr(vCnt)


            m_sAgentRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentRef, vControlValue:=m_sAgentRef)


            m_sAgentName = CStr(vName)

            sTemp = m_sAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            'developer guide no. 26 (guide)
            lblAgentNameLabel.Text = sTemp

            'because we know Agent cnt matches the Agent ref, can bypass
            'the validation at the end
            m_bVerifyAgentCnt = False

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAssociates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAssociates.Click

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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get associates object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMaintainAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdMaintainAssociates_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMaintainAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdBrokerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrokerLookup.Click
        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try




            'Developer Guide No 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="BR")

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


            'developer guide no. 26(latest guide)
            lblBrokerNameLabel.Text = sTemp
            m_bChangedProspect = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdBrokerLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdConsultantLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConsultantLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp, vResolvedName As String 'CT 19/07/00

        Try




            'Developer Guide No 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="CO", vResolvedName:=vResolvedName) 'CT 19/07/00 added extra resolvedName parameter


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtConsultantRef.Tag = CStr(vCnt)


            m_sConsultantRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)

            'm_sConsultantName$ = CStr(vName)
            m_sConsultantName = vResolvedName 'CT 19/07/00

            sTemp = m_sConsultantName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            'developer guide no. 26(latest guide)
            lblConsultantNamelabel.Text = sTemp

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdConsultantLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdCurrentAgent_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCurrentAgent.Click


        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try




            'developer guide no. 98(latest guide)
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="AG")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lCurrentAgent = CInt(CStr(vCnt))


            m_sCurrentAgentRef = CStr(vShortName)


            m_sCurrentAgentName = CStr(vName)

            sTemp = m_sCurrentAgentName
            m_lReturn = PMBGeneralFunc.DoubleCharacter(r_sString:=sTemp, v_sChar:="&")


            'developer guide no. 26(latset guide)
            lblCurrentAgentLabel.Text = sTemp

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

            ' CF 030699 - Commented following...
            ' This was due to the fact that its not really needed. Deleting the
            ' row from the listview suffices, as UpdateContacts function updates
            ' the business etc...
            ' Also, the below code would delete the actual address and not just the
            ' link to it.

            '    'Create address component if not already done so
            '    If ((m_oAddress Is Nothing) = True) Then
            '
            '        ' Get an instance of the contactinterface object via
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
            '    m_lReturn& = m_oAddress.SetProcessModes(vTask:=PMDelete)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            '
            '    'set the address id
            '    m_oAddress.addresscnt = m_lAddressCnt
            '    m_oAddress.AddressUsageTypeId = m_lAddressUsageTypeID
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
            '
            '    'Update the address details
            '    ' & postcode
            '
            'Reset Interface
            cmdEditAd.Enabled = False
            cmdDeleteAd.Enabled = False

            lvwAddresses.Items.RemoveAt(m_iLine - 1)

            ' Decrement the number of addresses
            m_lAddressCount -= 1

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

            ' CF 030699 - Commented following...
            ' This was due to the fact that its not really needed. Deleting the
            ' row from the listview suffices, as UpdateContacts function updates
            ' the business etc...
            ' Also, the below code would delete the actual address and not just the
            ' link to it.


            '    'Create address component if not already done so
            '    If ((m_oContact Is Nothing) = True) Then
            '
            '        ' Get an instance of the contactinterface object via
            '        ' the public object manager.
            '        m_lReturn& = g_oObjectManager.GetInstance( _
            ''            oObject:=m_oContact, _
            ''            sClassName:="iPMBContact.Interface", _
            ''            vInstanceManager:=PMGetLocalInterface)
            '
            '        ' Check for errors.
            '        If (m_lReturn& <> PMTrue) Then
            '            ' Log Error Message
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="Failed to get contact component", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="cmdEditCon_Click", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '
            '            Exit Sub
            '
            '        End If
            '
            '    End If
            '
            '    m_lReturn& = m_oContact.SetProcessModes(vTask:=PMDelete)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            '
            '    'set the address id
            '    m_oContact.contactcnt = m_lContactCnt
            '
            '    m_lReturn& = m_oContact.Start()
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        Exit Sub
            '    End If
            '
            '    'If not cancelled, edit grid
            '    If (m_oContact.Status = PMCancel) Then
            '        Exit Sub
            '    End If

            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            lvwContacts.Items.RemoveAt(m_iLine - 1)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdDeleteConv_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteConv.Click

        Dim iRow As Integer

        Try

            'Set row to be deleted - if a valid one selected
            If lvwConvictions.Items.Count < 1 Then
                Exit Sub
            End If


            iRow = Convert.ToString(lvwConvictions.FocusedItem.Tag)

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

            m_oConviction.PartyConvictionID = iRow


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

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteConv_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteConv_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeletePol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeletePol.Click

        Dim oListItem As ListViewItem

        'According to user guide this functionality is not supported more in Pure BO  (Confirmation by Mousumi Borah)
        Exit Sub
        Try

            'Set row to be deleted - if a valid one selected
            If lvwPolicies.Items.Count < 1 Then
                Exit Sub
            End If

            'Create prospect policy component if not already done so
            If m_oProspectPolicy Is Nothing Then

                ' Get an instance of the prospect policy interface object via
                ' the public object manager.
                Dim temp_m_oProspectPolicy As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oProspectPolicy, sClassName:="iPMBProspectPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oProspectPolicy = temp_m_oProspectPolicy

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policy component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeletePol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'User PMView instead
            'm_lReturn& = m_oProspectPolicy.SetProcessModes(vTask:=PMDelete)

            m_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the Ids
            oListItem = lvwPolicies.FocusedItem



            m_oProspectPolicy.PolicyTypeID = Convert.ToString(oListItem.Tag)


            m_oProspectPolicy.PolicyTypeDescription = oListItem.Text

            txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text



            m_oProspectPolicy.RenewalDate = m_oFormFields.UnformatControl(txtHiddenDate)


            m_oProspectPolicy.NoOfTimesQuoted = ListViewHelper.GetListViewSubItem(oListItem, 2).Text

            txtHiddenCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text



            m_oProspectPolicy.TargetPremium = m_oFormFields.UnformatControl(txtHiddenCurrency)


            m_lReturn = m_oProspectPolicy.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            m_bChangedProspectPolicies = True

            'Reset Interface
            cmdEditPol.Enabled = False
            cmdDeletePol.Enabled = False

            lvwPolicies.Items.RemoveAt(lvwPolicies.FocusedItem.Index)

            lvwPolicies.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeletePol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeletePol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            'TODO: MILAN
            'The following code renders delete functionality as useless as the Status will always be Cancel. Thus commenting it
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


                'Developer Guide No 17
                m_oAddress.FutureDatedAddresses = Nothing
            End If
            'sj 23/07/2002 - end


            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.Postcode = m_sMainPostCode

            'set the address id

            m_oAddress.addresscnt = m_lAddressCnt

            oListItem = lvwAddresses.Items.Item(m_iLine - 1)

            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"

                    m_oAddress.Postcode = oListItem.Text
                    sAddressUsage = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
                Case Else

                    m_oAddress.Postcode = ListViewHelper.GetListViewSubItem(oListItem, 5).Text
                    sAddressUsage = oListItem.Text
            End Select

            For k As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                If sAddressUsage = CStr(m_vAddressTypes(1, k)) Then

                    m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k


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

            With oListItem
                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Postcode

                        .Text = m_oAddress.PostalCode
                        ' Address usage type

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.Address4
                    Case Else
                        ' Address usage type

                        .Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oAddress.Address4
                        'Postcode

                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oAddress.PostalCode
                End Select

                ' Store the Address_cnt


                .Tag = m_oAddress.addresscnt
            End With


            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
                'Caption = "Group Client: " & m_sShortName & " " & m_sMainPostCode
            End If

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


            'developer guide no. changes made as per the VB6.0 code
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

        'RWH(09/06/2000) - Tidy up object when leaving sub. This has to be done in a number of
        'places but means reference is re-initialised, and consequently all variables within
        'object, every time it is used. Avoids problems where other objects using the same
        'Contact object destroy their reference triggering a Terminate and the destruction of
        'some of our variables within Contact.



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

                    'RWH(09/06/2000) - Tidy up object.
                    If Not (m_oContact Is Nothing) Then
                        m_oContact = Nothing
                    End If
                    Exit Sub

                End If

            End If


            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(09/06/2000) - Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'set the contact id

            m_oContact.contactcnt = m_lContactCnt

            m_oContact.Reference = txtIDReference.Text

            m_oContact.Postcode = m_sMainPostCode


            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RWH(09/06/2000) - Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                'RWH(09/06/2000) - Tidy up object.
                If Not (m_oContact Is Nothing) Then
                    m_oContact = Nothing
                End If
                Exit Sub
            End If


            'Reset Interface
            cmdEditCon.Enabled = False
            cmdDeleteCon.Enabled = False

            oListItem = lvwContacts.Items.Item(m_iLine - 1)


            oListItem.Text = m_oContact.AreaCode
            ' Column 2

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_oContact.Number

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oContact.Extension

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = m_oContact.ContactType

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oContact.Description


            'RWH(09/06/2000) - Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RWH(09/06/2000) - Tidy up object.
            If Not (m_oContact Is Nothing) Then
                m_oContact = Nothing
            End If
            Exit Sub

        End Try
    End Sub




    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "") As Integer 'CT 19/07/00 added vResolvedName parameter



        Dim result As Integer = 0
        'developer guide no. 108 (guide)
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 108 (guide)
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
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = "iPMBPartyGC.Interface"
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

                'developer guide no. 143(latest guide)
                vName = oFindParty.LongName
                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                    'developer guide no. 229(latest guide)
                Else
                    vName = ""
                End If
                vResolvedName = oFindParty.ResolvedName 'CT 19/07/00
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
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
                        'RWH(24/07/2000)
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

    'DC 03/05/00

    ' ***************************************************************** '
    ' Name: UpdateAssociates
    '
    ' Description: This goes thru all Associates in the the grid control
    ' and the original Associate array and sees what the differences
    ' are. It then adds new Associates or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateAssociates() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.UpdateAssociates(vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAssociates Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAssociates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC 04/08/00
    ' ***************************************************************** '
    ' Name: UpdateMainContact
    '
    ' Description: Updates the Main Contact for the party
    '
    ' ***************************************************************** '
    Private Function UpdateMainContact() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.UpdateMainContact(vPartyCnt:=m_lPartyCnt, lMainContactCnt:=m_lMainContactCnt, sMainContactDesc:=m_sMainContactDesc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateMainContact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateMainContact", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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
        Dim vStrengthCodeID As String = ""
        Dim vPreviousInsurerCnt, vPreviousBrokerCnt, vCampaigns, vPolicies(,) As Object
        Dim sTemp As String = ""
        Dim oListItem As ListViewItem

        ' Lookup value contants.
        'Const ACValueTableName As Integer = 0      ''Unused Local Variable
        'Const ACValueID As Integer = 1     ''Unused Local Variable
        'Const ACValueStartPos As Integer = 2       ''Unused Local Variable
        'Const ACValueNumber As Integer = 3     ''Unused Local Variable

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Create iProspect if not already done so

            If m_oProspect Is Nothing Then

                ' Get an instance of the prospect interface object via
                ' the public object manager.
                Dim temp_m_oProspect As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oProspect, "bSIRProspect.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oProspect = temp_m_oProspect

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdProspect_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If

            End If
            ' MKW PN14171 START
            If m_sUnderwritingOrAgency = "A" Then
                fraPreviousBroker.Visible = False
            End If
            ' MKW PN14171 END

            'MSS210901 - Added from UW
            If m_lPartyCnt = 0 Then
                m_iProspectTask = gPMConstants.PMEComponentAction.PMAdd
                Return result
            End If
            'MSS210901 - Merge end

            'Always do this, as the data may have changed...

            m_lReturn = m_oProspect.GetProspectData(v_vPartyCnt:=m_lPartyCnt, r_vAgentReference:=vAgentReference, r_vCurrentIntermediary:=vCurrentIntermediary, r_vProspectStatusID:=vProspectStatusID, r_vStrengthCodeId:=vStrengthCodeID, r_vPreviousInsurerCnt:=vPreviousInsurerCnt, r_vPreviousBrokerCnt:=vPreviousBrokerCnt, r_vCampaigns:=vCampaigns, r_vPolicies:=vPolicies)

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                m_iProspectTask = gPMConstants.PMEComponentAction.PMAdd
            Else
                m_iProspectTask = gPMConstants.PMEComponentAction.PMEdit
            End If


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


            'developer guide no. 26(latset guide)
            lblCurrentAgentLabel.Text = sTemp


            If Convert.IsDBNull(vProspectStatusID) Or IsNothing(vProspectStatusID) Then
                cboProspectingStatus.SelectedIndex = -1
            Else
                For i As Integer = 0 To cboProspectingStatus.Items.Count - 1
                    If VB6.GetItemData(cboProspectingStatus, i) = vProspectStatusID Then
                        cboProspectingStatus.SelectedIndex = i
                    End If
                Next i
            End If

            '2005 Get Details with all other lookups
            '    m_lReturn& = GetLookupDetails( _
            ''        sLookupTable:=SIRLookupStrengthCode, _
            ''        ctlLookup:=cboStrengthCode)

            lvwCampaigns.Items.Clear()
            lvwCampaigns.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(1100))
            lvwCampaigns.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(1600))
            lvwCampaigns.Columns.Item(2).Width = CInt(0)



            'developer guide no. 26(latest guide)
            lblInsurerNameLabel.Text = m_sInsurerName
            txtInsurerRef.Text = m_sInsurerRef



            'developer guide no. 26(latest guide)
            lblBrokerNameLabel.Text = m_sBrokerName
            txtBrokerRef.Text = m_sBrokerRef

            'MSS210901 - Replaced existing SFORB with better UW code
            If vStrengthCodeID > "" Then
                For i As Integer = 0 To cboStrengthCode.Items.Count - 1
                    If VB6.GetItemData(cboStrengthCode, i) = StringsHelper.ToDoubleSafe(vStrengthCodeID) Then
                        cboStrengthCode.SelectedIndex = i
                    End If
                Next i
            End If
            'MSS210901 - Merge end

            If Information.IsArray(vCampaigns) Then

                ' Assign the details to the interface.

                For i As Integer = vCampaigns.GetLowerBound(1) To vCampaigns.GetUpperBound(1)

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to the first column.
                    ' Column 1
                    'Call GetCampaignLookups(vCategoryId:=vCampaigns(0, i), _
                    'vCategoryDescription:=vCategory)

                    'Set oListItem = lvwLifestyle.ListItems.Add(, , _
                    'Trim$(vCategory), , CampaignImage)


                    oListItem = lvwCampaigns.Items.Add(CStr(vCampaigns(PMBCampaignDescription, i)).Trim(), "")

                    ' Assign details to other the columns
                    ' Column 2

                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(vCampaigns(PMBCampaignCampaignDate, i)).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()


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


                    oListItem = lvwPolicies.Items.Add(sTemp, "")

                    ' Assign details to other the columns
                    ' Column 2

                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=CStr(vPolicies(PMBPolicyRenewalDate, i)).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()


                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(vPolicies(PMBPolicyNoOfTimesQuoted, i)).Trim()


                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenCurrency, vControlValue:=CStr(vPolicies(PMBPolicyTargetPremium, i)).Trim())

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text.Trim()


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
    ' Name: SaveProspect
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function SaveProspect() As Integer

        Dim result As Integer = 0
        Dim vTurnover As Integer
        Dim vAgentReference As Object
        Dim vCurrentIntermediary As Integer
        Dim vPreviousInsurerCnt As Integer
        Dim vPreviousBrokerCnt As Integer
        Dim vStrengthCodeID As Integer
        Dim vProspectStatusID As Integer

        'MK 990921 Dim vWageRoll As Variant

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'MK 990921    vTurnover = m_oFormFields.UnformatControl(ctlControl:=txtTurnover)
            'MK 990921    vWageRoll = m_oFormFields.UnformatControl(ctlControl:=txtWageRoll)



            vAgentReference = m_oFormFields.UnformatControl(ctlControl:=txtAgentReference)

            If cboProspectingStatus.SelectedIndex <> -1 Then
                vProspectStatusID = VB6.GetItemData(cboProspectingStatus, cboProspectingStatus.SelectedIndex)
            End If

            vTurnover = cboTurnover.ItemId

            vCurrentIntermediary = m_lCurrentAgent
            If Convert.ToString(txtInsurerRef.Tag) = "" Then
                vPreviousInsurerCnt = 0
            Else
                vPreviousInsurerCnt = CInt(Convert.ToString(txtInsurerRef.Tag))
            End If
            If Convert.ToString(txtBrokerRef.Tag) = "" Then
                vPreviousBrokerCnt = 0
            Else
                vPreviousBrokerCnt = CInt(Convert.ToString(txtBrokerRef.Tag))
            End If
            If cboStrengthCode.SelectedIndex <> -1 Then
                vStrengthCodeID = VB6.GetItemData(cboStrengthCode, cboStrengthCode.SelectedIndex)
            Else
                vStrengthCodeID = 0
            End If

            If m_iProspectTask = gPMConstants.PMEComponentAction.PMAdd Then


                m_lReturn = m_oProspect.EditAdd(vPartyCnt:=m_lPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_iProspectTask = gPMConstants.PMEComponentAction.PMEdit

            Else


                m_lReturn = m_oProspect.EditUpdate(vPartyCnt:=m_lPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = m_oProspect.Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveProspect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveProspect", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

            oListItem = lvwConvictions.Items.Item(m_iLine - 1)

            ' Assign details to other the columns
            ' Column 1

            oListItem.Text = m_oConviction.Code

            'Conviction Date

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oConviction.ConvictionDate.Trim())
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text.Trim()

            ' Description

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oConviction.Description.Trim()

            ' Fine

            m_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, Convert.ToString(ReflectionHelper.GetMember(m_oConviction, "FineAmount")).Trim())

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text.Trim()

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

    Private Sub cmdEditPol_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditPol.Click


        Dim oListItem As ListViewItem
        'According to user guide this functionality is not supported more in Pure BO  (Confirmation by Mousumi Borah)
        Exit Sub
        If m_lPartyCnt < 1 Then
            MessageBox.Show("Must Save Client Details before editing Prospect Policies", Application.ProductName)
        End If

        Try

            'Create prospect policy component if not already done so
            If m_oProspectPolicy Is Nothing Then

                ' Get an instance of the prospect policy interface object via
                ' the public object manager.
                Dim temp_m_oProspectPolicy As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oProspectPolicy, sClassName:="iPMBProspectPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oProspectPolicy = temp_m_oProspectPolicy

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policy component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If


            m_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            'set the Ids
            oListItem = lvwPolicies.FocusedItem



            m_oProspectPolicy.PolicyTypeID = Convert.ToString(oListItem.Tag)


            m_oProspectPolicy.PolicyTypeDescription = oListItem.Text

            txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text



            m_oProspectPolicy.RenewalDate = m_oFormFields.UnformatControl(txtHiddenDate)


            m_oProspectPolicy.NoOfTimesQuoted = ListViewHelper.GetListViewSubItem(oListItem, 2).Text

            txtHiddenCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text



            m_oProspectPolicy.TargetPremium = m_oFormFields.UnformatControl(txtHiddenCurrency)


            m_lReturn = m_oProspectPolicy.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            m_bChangedProspectPolicies = True

            'Reset Interface
            cmdEditPol.Enabled = False
            cmdDeletePol.Enabled = False

            'Set oListItem = lvwLifestyle.ListItems.Item(m_iLine)


            oListItem.Text = m_oProspectPolicy.PolicyTypeDescription

            oListItem.Tag = m_oProspectPolicy.PolicyTypeID

            ' Assign details to other the columns
            ' Column 2

            m_lReturn = m_oFormFields.FormatControl(txtHiddenDate, m_oProspectPolicy.RenewalDate)
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oProspectPolicy.NoOfTimesQuoted
            ' Column 4


            m_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, m_oProspectPolicy.TargetPremium)
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text

            'Column 5

            'developer guide no. 197(latest guide)
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = DateTime.FromOADate(m_oProspectPolicy.RenewalDate.ToOADate()).ToString("yyyyMMdd")

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditPol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdEditPol_Clickold) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdEditPol_Clickold()
    '
    '
    'Dim iRow As Integer
    'Dim oListItem As ListViewItem
    '
    'If m_lPartyCnt < 1 Then
    'MessageBox.Show("Must Save Client Details before editing Prospect Policies", Application.ProductName)
    'End If
    '
    'Try 
    '
    'Create prospect policy component if not already done so
    'If m_oProspectPolicy Is Nothing Then
    '
    ' Get an instance of the prospect policy interface object via
    ' the public object manager.
    'Dim temp_m_oProspectPolicy As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_m_oProspectPolicy, sClassName:="iPMBProspectPolicy.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
    'm_oProspectPolicy = temp_m_oProspectPolicy
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get prospect policy component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    '
    'Exit Sub
    '
    'End If
    '
    'End If
    '

    'm_lReturn = m_oProspectPolicy.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    '
    'set the Ids
    'oListItem = lvwPolicies.FocusedItem
    '


    'm_oProspectPolicy.PolicyTypeID = Convert.ToString(oListItem.Tag)
    '

    'm_oProspectPolicy.PolicyTypeDescription = oListItem.Text
    '
    'txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text
    '


    'm_oProspectPolicy.RenewalDate = m_oFormFields.UnformatControl(txtHiddenDate)
    '

    'm_oProspectPolicy.NoOfTimesQuoted = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
    '
    'txtHiddenCurrency.Text = ListViewHelper.GetListViewSubItem(oListItem, 3).Text
    '


    'm_oProspectPolicy.TargetPremium = m_oFormFields.UnformatControl(txtHiddenCurrency)
    '

    'm_lReturn = m_oProspectPolicy.Start()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Exit Sub
    'End If
    '
    'If not cancelled, edit grid

    'If m_oProspectPolicy.Status = gPMConstants.PMEReturnCode.PMCancel Then
    'Exit Sub
    'End If
    '
    'm_bChangedProspectPolicies = True
    '
    'Reset Interface
    'cmdEditPol.Enabled = False
    'cmdDeletePol.Enabled = False
    '
    'Set oListItem = lvwLifestyle.ListItems.Item(m_iLine)
    '

    'oListItem.Text = m_oProspectPolicy.PolicyTypeDescription

    'oListItem.Tag = m_oProspectPolicy.PolicyTypeID
    '
    ' Assign details to other the columns
    ' Column 2

    'm_lReturn = m_oFormFields.FormatControl(txtHiddenDate, m_oProspectPolicy.RenewalDate)
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtHiddenDate.Text
    '
    ' Column 3

    'ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oProspectPolicy.NoOfTimesQuoted
    ' Column 4
    '

    'm_lReturn = m_oFormFields.FormatControl(txtHiddenCurrency, m_oProspectPolicy.TargetPremium)
    'ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenCurrency.Text
    '
    'Column 5

    'ListViewHelper.GetListViewSubItem(oListItem, 4).Text = DateTime.FromOADate(m_oProspectPolicy.RenewalDate).ToString("yyyyMMdd")
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditPol_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditPol_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

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

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.StartDate)

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtHiddenDate.Text.Trim()

            ' Column 4

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtHiddenDate, vControlValue:=m_oPartyLoyaltyScheme.EndDate)

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtHiddenDate.Text.Trim()

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

        'If we're here by tabbing (or back-tabbing) we're on tab 5
        SSTabHelper.SetSelectedIndex(tabMainTab, 5)

    End Sub

    Public Sub ShowHelpScreen(Optional ByRef cmdHelp As Object = Nothing, Optional ByRef ScreenHelpID As Object = Nothing)
        ' Fire up the help screen
        'developer guide no. 184 (guide)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdInsurerLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInsurerLookup.Click
        Dim vCnt, vShortName, vName As Object
        Dim sTemp As String = ""

        Try




            'Developer Guide No 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="IN")

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

            'developer guide no. 63
            lblInsurerNameLabel.Text = sTemp
            m_bChangedProspect = True

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="Err_cmdInsurerLookup_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_7.Click, _cmdPrevious_6.Click, _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_2.Click, _cmdPrevious_3.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    ' PRIVATE Events (Begin)

    ' ***************************************************************** '
    ' Name: DisableForm
    '
    ' Description: Sets all of the interface details to the disable
    '              state passed.
    '
    ' ***************************************************************** '
    Private Function DisableForm(ByRef lDisabled As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'DC030401 added check for uctDropdown
            ' Set all of the forms controls to the disable state.
            'developer guide no. 157 guide
            'start
            Dim ctlFormControl As System.Windows.Forms.Control
            For Each ctlFormControl In Me.Controls
                If (TypeOf ctlFormControl Is System.Windows.Forms.TabControl) Then
                    For Each ctlTabPage As Control In ctlFormControl.Controls
                        If (TypeOf ctlTabPage Is System.Windows.Forms.TabPage) Then
                            For Each ctl As Control In ctlTabPage.Controls
                                If (TypeOf ctl Is GroupBox) Then
                                    For Each ctlChild As Control In ctl.Controls
                                        'Check the type of the control.
                                        If (TypeOf ctlChild Is System.Windows.Forms.TextBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.ComboBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.CheckBox) Then
                                            ctlChild.Enabled = Not lDisabled
                                        ElseIf (TypeOf ctlChild Is System.Windows.Forms.RadioButton) Then
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
                                ElseIf (TypeOf ctl Is System.Windows.Forms.RadioButton) Then
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

            'DC030401 Now the command buttons...
            cmdAgentLookUp.Enabled = Not lDisabled
            cmdConsultantLookup.Enabled = Not lDisabled
            cmdAddAd.Enabled = Not lDisabled
            cmdAddCon.Enabled = Not lDisabled
            cmdAddConv.Enabled = Not lDisabled
            cmdCurrentAgent.Enabled = Not lDisabled
            cmdAddPol.Enabled = Not lDisabled
            cmdInsurerLookup.Enabled = Not lDisabled
            cmdBrokerLookup.Enabled = Not lDisabled
            cmdAddLoyaltyScheme.Enabled = Not lDisabled ' RAW 18/11/2002 : PS005 : Added

            uctPartyTax1.ReadOnly_Renamed = lDisabled

            cboTurnover.Enabled = Not lDisabled
            uctPartyBankControl1.ReadOnly_Renamed = lDisabled

            'MKR PN14483 22/09/2004
            '    cmdProspect.Enabled = Not lDisabled
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
    ' Name: GetParty
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetParty() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



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



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Public Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check the task.
            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                Select Case (Task)
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
            Select Case (Task)
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
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                        'AR20050214 - PN18407 Move UpdateOrion call until after addresses have been saved

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
                        '                 End If
                    End If

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Check if form has been cancelled, if so,
                    ' check if the details have changed and if
                    ' so, prompt if they wish to cancel.
                    If Status = gPMConstants.PMEReturnCode.PMCancel Then
                        ' Check the details havn't changed.

                        m_lReturn = m_oBusiness.Cancel()
                        'MH Request - Allways confirm cancellation
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

                        ' Update the details using the business object.

                        m_lReturn = m_oBusiness.Update()

                        ' Check for errors.
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Failed to update the details
                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update the details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessCommand")
                        End If
                        'AR20050214 - PN18407 Move UpdateOrion call until after addresses have been saved

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
                If m_oAccount IsNot Nothing Then
                    m_oAccount.Dispose()
                    m_oAccount = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oConviction IsNot Nothing Then
                    m_oConviction.Dispose()
                    m_oConviction = Nothing
                End If
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If
                If m_oPMLock IsNot Nothing Then
                    m_oPMLock.Dispose()
                    m_oPMLock = Nothing
                End If
                If m_oPMUser IsNot Nothing Then
                    m_oPMUser.Dispose()
                    m_oPMUser = Nothing
                End If
                If PMBGeneralFunc.g_oListManager IsNot Nothing Then
                    PMBGeneralFunc.g_oListManager.Dispose()
                    PMBGeneralFunc.g_oListManager = Nothing
                End If
                If g_oGIS IsNot Nothing Then
                    g_oGIS.Dispose()
                    g_oGIS = Nothing
                End If
                If m_oFormFields IsNot Nothing Then
                    m_oFormFields.Dispose()
                    m_oFormFields = Nothing
                End If
                If m_oProspectPolicy IsNot Nothing Then
                    m_oProspectPolicy.Dispose()
                    m_oProspectPolicy = Nothing
                End If
                If m_oAddress IsNot Nothing Then
                    m_oAddress.Dispose()
                    m_oAddress = Nothing
                End If
                If m_oAssociates IsNot Nothing Then
                    m_oAssociates.Dispose()
                    m_oAssociates = Nothing
                End If
                If m_oContact IsNot Nothing Then
                    m_oContact.Dispose()
                    m_oContact = Nothing
                End If
                If m_oProspect IsNot Nothing Then
                    m_oProspect.Dispose()
                    m_oProspect = Nothing
                End If
                If m_oPartyLoyaltyScheme IsNot Nothing Then
                    m_oPartyLoyaltyScheme.Dispose()
                    m_oPartyLoyaltyScheme = Nothing
                End If
                If m_oClientNumber IsNot Nothing Then
                    m_oClientNumber.Dispose()
                    m_oClientNumber = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If


            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sHelpFile, sValue As String

        Dim m_lReturn As Integer
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserId = .UserID
                g_iCurrencyID = .CurrencyID 'PN16993
                'RWH(24/07/2000) RSAIB Process 004.
                m_iDefaultCountryID = .CountryID
            End With

            'Get bPMLock
            Dim temp_m_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMLock = temp_m_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            ' Initialise the process modes.
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = GIIPMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'developer guide no. 39 (no solution)
                'App.HelpFile = sHelpFile
            End If

            'DN 05/06/01 - Late bind list manager to avoid clash between GI and GII
            Dim temp_g_oListManager As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oListManager, sClassName:="iGEMListManager.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            PMBGeneralFunc.g_oListManager = temp_g_oListManager

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of iGEMListManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If


            m_lReturn = PMBGeneralFunc.g_oListManager.CheckListVersions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Faied to check list manager versions.", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If
            '2005 New Client Screen Layout
            Dim temp_m_oAccount As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oAccount, "bACTAccount.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oAccount = temp_m_oAccount

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTAccount", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            Dim temp_m_oCurrencyConvert As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oCurrencyConvert, "bACTCurrencyConvert.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oCurrencyConvert = temp_m_oCurrencyConvert

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bACTCurrencyConvert", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            '2005 End


            'sj 11/06/2002 - start
            m_lReturn = PartyFunc.GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vIsNRMA:=m_bIsNRMA, r_vValidateAlternativeIdentifier:=m_bValidateAlternativeIdentifier, r_vFutureDateAddressChanges:=m_bFutureDateAddressChanges, r_vMultiTreeAccounting:=m_bMultiTreeAccounting)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 11/06/2002 - end
            'FSA Phase III

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for FSA Compliance", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sValue <> "1" Then
                lblTobLetter.Visible = False
                lblTobLetter.Enabled = False
                txtTobLetter.Visible = False
                txtTobLetter.Enabled = False
            End If

            iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)

            'Broking doesn't show Sub Branch by default
            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking, v_vBranch:=g_iSourceID, r_vUnderwriting:=sValue)

            m_bShowSubBranchID = (sValue = "1") Or (m_sUnderwritingOrAgency = "U")


            'Retrieve System Option for Duplicate Client Identification
            m_lReturn = iPMFunc.GetSystemOption(SYSOPTDuplicateClientIdentification, sValue)
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

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UnloadControl
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function UnloadControl() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            Dispose()
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

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Party Bank Details
    Public Function LoadPartyBankControl() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "LoadPartyBankControl"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Developer Guide No 9
            m_lReturn = uctPartyBankControl1.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            uctPartyBankControl1.PartyCnt = m_lPartyCnt
            'developer guide no. 68 (guide)
            m_lReturn = uctPartyBankControl1.Load_Renamed()
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


    Private Sub ddPaymentMethod_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddPaymentMethod.GotFocus

        If ddPaymentMethod.Text = "" Then
            m_lReturn = HighlightContol(ddPaymentMethod, optBoolDropDown:=True)
        End If

    End Sub

    Private Sub ddPaymentMethod_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddPaymentMethod.LostFocus

        m_lReturn = ValidateListField(ddPaymentMethod)

    End Sub

    Private Sub lvwAddresses_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Click
        'RWH(11/07/2000) Altered to move PostCode from far left to far
        'right of ListView.



        If lvwAddresses.Items.Count > 0 Then


            m_lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)
            m_iLine = lvwAddresses.FocusedItem.Index + 1

        End If

        ' Moved these into MouseDown event
        'cmdDeleteAd.Enabled = True
        'cmdEditAd.Enabled = True

    End Sub

    Private Sub lvwAddresses_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.DoubleClick

        If lvwAddresses.Items.Count > 0 Then

            m_lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)
        End If

        'cmdDeleteAd.Enabled = True
        'cmdEditAd.Enabled = True

    End Sub

    Private Sub lvwAddresses_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 1) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 1)
        End If
    End Sub

    Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70(Latest Guide)
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwAddresses.GetItemAt(x, y) Is Nothing Then
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False
        Else
            'DC030401 check if in View mode first ....
            '        cmdDeleteAd.Enabled = True
            '        cmdEditAd.Enabled = True
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

    Private Sub lvwContacts_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Click

        If lvwContacts.Items.Count > 0 Then

            m_lContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
            m_iLine = lvwContacts.FocusedItem.Index + 1
        End If

        ' Moved these into MouseDown event
        'cmdDeleteCon.Enabled = True
        'cmdEditCon.Enabled = True

    End Sub

    Private Sub lvwContacts_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.DoubleClick

        If lvwContacts.Items.Count > 0 Then

            m_lContactCnt = Convert.ToString(lvwContacts.FocusedItem.Tag)
        End If

        'cmdDeleteCon.Enabled = True
        'cmdEditCon.Enabled = True

    End Sub

    Private Sub lvwContacts_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContacts.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 2) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 2)
        End If
    End Sub

    Private Sub lvwContacts_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContacts.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwContacts.GetItemAt(x, y) Is Nothing Then
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        Else
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        End If

    End Sub


    Private Sub lvwConvictions_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwConvictions.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
            tabMainTab.SelectTab(3)
        End If
    End Sub

    Private Sub lvwConvictions_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwConvictions.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwConvictions.GetItemAt(x, y) Is Nothing Then
            cmdDeleteConv.Enabled = False
            cmdEditConv.Enabled = False
        Else
            'DC030401 check for View Mode first ....
            '        cmdDeleteConv.Enabled = True
            '        cmdEditConv.Enabled = True
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditConv.Enabled = True
                cmdDeleteConv.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwLoyaltySchemes_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwLoyaltySchemes.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
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
        'Developer Guide No. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If lvwPolicies.GetItemAt(x, y) Is Nothing Then
            cmdDeletePol.Enabled = False
            cmdEditPol.Enabled = False
        Else
            'DC030401 check for View mode first ...
            '        cmdDeletePol.Enabled = True
            '        cmdEditPol.Enabled = True
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditPol.Enabled = True
                cmdDeletePol.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwLoyaltySchemes_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwLoyaltySchemes.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no. 70(guide)

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y             'RAW 18/11/2002 : PS005 : Added

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
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        ToolTip1.SetToolTip(pnlAgentName, m_sAgentName)
    End Sub

    'PN 23222
    Private Sub pnlBrokerName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlBrokerName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        ToolTip1.SetToolTip(pnlBrokerName, m_sBrokerName)
    End Sub

    'PN23222
    Private Sub pnlConsultantName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlConsultantName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        ToolTip1.SetToolTip(pnlConsultantName, m_sConsultantName)
    End Sub

    'PN23222
    Private Sub pnlCurrentAgent_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlCurrentAgent.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        ToolTip1.SetToolTip(pnlCurrentAgent, m_sCurrentAgentName)
    End Sub


    'PN 23222
    Private Sub pnlInsurerName_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles pnlInsurerName.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        ToolTip1.SetToolTip(pnlInsurerName, m_sInsurerName)
    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab

                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    If Not (cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)).FindForm() Is Nothing) Then
                        VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                    End If
                Else
                    'cmdOK.Default = True
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                'Application.DoEvents()

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

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_7.Click, _cmdNext_6.Click, _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_3.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try

            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
            End If

            '' Set focus to the first control on the tab.
            'If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
            '	m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            'End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

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
    ' Name: CancelClick
    '
    ' Description:
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
            '        tabMainTab.Top = 0
            '        tabMainTab.Visible = True
            '        'This stops it closing the form
            '        CancelClick = PMFalse
            '        Exit Function
            '    End If

            If tabMainTab.Visible Then
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

    'EK 9/12/99
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
        Dim sText As String = ""


        'DC 28/09/00

        Dim sOriginalReference As String = ""


        'sj 12/06/2002 - start
        Dim sMessage As String = ""
        'sj 12/06/2002 - end

        'sj 23/08/2002 - start
        Dim sClientcode As String = ""
        'sj 23/08/2002 - end


        Dim sName As String = ""

        Dim sSelectedClientCode As String = ""
        Dim lSelectedClientPartyCnt As Integer
        Dim iOKAction As Integer

        Dim bDuplicateFound As Boolean


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK



            '
            '    ' Check for errors
            '    If m_lReturn <> PMTrue Then
            '        OKClick = PMFalse
            '        Exit Function
            '    End If


            '2005 Roadmap Always save propsect information
            '    If (tabProspecting.Visible = True) Then
            '
            '        ' Save the prospect information
            '        m_lReturn = SaveProspect()
            '        If (m_lReturn& <> PMTrue) Then
            '            ' Log Error.
            '            LogMessage _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="Failed to save prospect data.", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="cmdOK_Click", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '        End If
            '
            '        If (m_bChangedProspectPolicies = True) Then
            '            m_lReturn = SaveProspectPolicies
            '
            '            If (m_lReturn <> PMTrue) Then
            '                OKClick = m_lReturn
            '                Exit Function
            '            End If
            '        End If
            '
            '        tabMainTab.Visible = True
            '        tabProspecting.Visible = False
            '
            '        OKClick = PMFalse
            '
            '        'MKR 22/09/2004 PN 14543 Added to stop attempt to try and save Party Prospect
            '        'twice with same Primary Key value causing a DB error.  -- Start
            '        m_bChangedProspect = False
            '        'MKR 22/09/2004 -- End
            '        Exit Function
            '
            '    End If



            ' Check mandatory controls have been entered into.
            ' 62594
            m_lReturn = CheckMainMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            sOriginalReference = txtIDReference.Text
            'If txtIDReference.Text = "" And m_bIsSetMaskingCode = True Then
            If txtIDReference.Text = "" Then
                m_lReturn = GenerateClientCode(v_sName:=txtName.Text, r_sOriginalCode:=sOriginalReference, r_sClientCode:=sClientcode, r_bDuplicateFound:=bDuplicateFound)

                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GenerateClientCode", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                txtIDReference.Text = sClientcode
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
                            Me.ShortName = sSelectedClientCode
                            Me.PartyCnt = lSelectedClientPartyCnt

                            m_oBusiness.PartyCnt = lSelectedClientPartyCnt
                            Return gPMConstants.PMEReturnCode.PMTrue
                        End If
                    End If

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
                End If
            End If


            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'update the party cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                ' save additional details back to party record.
                m_lReturn = UpdatePartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Update party addresses
                m_lReturn = UpdateAddresses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Return result
                End If

                '2005 Roadmap changes
                m_lReturn = SaveProspect()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save prospect data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                End If

                If m_bChangedProspectPolicies Then
                    m_lReturn = SaveProspectPolicies()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                End If


                'AR20050214 - PN18407 Move update orion call until after addresses are saved
                m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the Orion details", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick")

                    Return result

                End If

                'Party Bank Details
                'developer guide no. 9(latest guide)
                m_lReturn = uctPartyBankControl1.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("OKClick", "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                uctPartyBankControl1.PartyCnt = m_lPartyCnt
                m_lReturn = uctPartyBankControl1.UpdatePartyBankDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("OKClick", "uctPartyBankControl1.UpdatePartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Update party contacts
                m_lReturn = UpdateContacts()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Return result
                End If

                'DC 03/05/00
                'Update party associates
                m_lReturn = UpdateAssociates()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Associate Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Return result
                End If

                'DC 04/08/00
                'Update main contact
                m_lReturn = UpdateMainContact()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Main Contact Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Return result
                End If

            End If

            'DN 19/10/00 - Update Gemini if installed

            m_lReturn = m_oBusiness.UpdateGemini(vPartyCnt:=m_lPartyCnt, vTask:=Task)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Gemini Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                Return result
            End If
            'DN 19/10/00 - End

            'sj 23/07/2002 - start
            If m_bFutureDateAddressChanges Then

                m_lReturn = m_oBusiness.CreateFutureDatedAddresses(v_vFutureDatedAddresses:=m_vFutureDatedAddresses, v_lPartyCnt:=m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetFutureDatedAddresses Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick")
                    Return result
                End If
            End If
            'sj 23/07/2002 - end

            If tabMainTab.Visible Then

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                'UpdateAddressPostCodeProperties

                ' Everything OK, so we can hide the interface.
                'Me.Hide

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="OKClick Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="OKClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: EditClick
    '
    ' Description:
    '
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

    Private isInitializingComponent As Boolean
    Private Sub txtAgentRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        If txtAgentRef.Text.Trim.Length = 0 Then
            m_sAgentName = ""
            lblAgentNameLabel.Text = ""
        End If
        m_bVerifyAgentCnt = True
    End Sub

    Private Sub txtAgentRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentRef)
    End Sub

    Private Sub txtAgentRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentRef.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentRef)
        End If
        If txtAgentRef.Text.Trim.Length = 0 Then
            m_sAgentName = ""
            lblAgentNameLabel.Text = ""
        End If
        cboBlackListReason.Focus()
    End Sub

    Private Sub txtAgentReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.Enter

        If SSTabHelper.GetTabVisible(tabMainTab, 6) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 6)
        End If

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentReference)
    End Sub

    Private Sub txtAgentReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentReference.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentReference)
        End If
    End Sub

    Private Sub txtCCJ_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCCJ.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 3)
        End If
    End Sub

    Private Sub txtCharityNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCharityNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCharityNumber)
    End Sub

    Private Sub txtCharityNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCharityNumber.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCharityNumber)
        End If
    End Sub

    Private Sub txtConsultantRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bVerifyConsultantCnt = True
    End Sub

    Private Sub txtConsultantRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Enter
        If SSTabHelper.GetTabVisible(tabMainTab, 0) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
        End If

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtConsultantRef)
    End Sub

    Private Sub txtConsultantRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtConsultantRef)
        End If
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 1)
        lvwAddresses.Focus()
    End Sub

    Private Sub txtIDReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        'frmInterface.Caption = "Group Client: " & txtIDReference.Text & " " & m_sMainPostCode
    End Sub

    Private Sub txtIDReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtIDReference)
        End If
    End Sub

    Private Sub txtIDReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtIDReference)
        End If
    End Sub
    'developer guide no. 78 (guide)
    Private Sub txtMainContact_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtMainContact.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        RestrictChars(KeyAscii, True)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtName)
        End If
    End Sub

    'developer guide no. 78 (guide)
    Private Sub txtName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        '**********************************************************************
        'MKR 12/10/2004 PN 6021 -- Allowing "," in Resolved Name
        If KeyAscii <> 44 Then
            RestrictChars(KeyAscii, True)
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtName.Leave

        ' CTAF 180200 - Format name
        'txtName.Text = Strings.StrConv(txtName.Text, VbStrConv.ProperCase)
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtName)
        End If
    End Sub

    'PN 73087 (Sumit Singla)
    Private Sub txtMembers_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtMembers.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii < 48 Or KeyAscii > 57) And KeyAscii <> 8 Then
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub
    'PN 73087 (Sumit Singla)

    Private Sub txtMembers_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMembers.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtMembers)
    End Sub

    Private Sub txtMembers_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtMembers.Leave
        If Not (m_oFormFields Is Nothing) Then
            m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtMembers)
        End If
    End Sub

    ' PRIVATE Events (End)

    ' ***************************************************************** '
    ' Name: LoadControl
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function LoadControl() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Forms initialise event.

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyGC.Business", vInstanceManager:="ClientManager")
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

                Return result
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

            ' Set the process modes for the busines object.

            m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")

                Return result
            End If

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt
            ' {* USER DEFINED CODE (End) *}

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If

            'DN 05/06/01 - Late bind list manager to avoid clash between GI and GII
            '    Set g_oListManager = New iGEMListManager.Interface
            '    m_lReturn = g_oListManager.Initialise()
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Faied to initialise list manager.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '    End If

            '    m_lReturn = g_oListManager.CheckListVersions()
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Log Error.
            '        LogMessage _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Faied to check list manager versions.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="Form_Load", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '    End If

            '    ' Gets the interface details to be displayed.
            '    ' No we don't - this is explicitly called from outside...
            '    m_lReturn& = GetParty()
            '
            '    ' Check for errors.
            '    If (m_lReturn& <> PMTrue) Then
            '        ' Failed to get the interface details.
            '        m_lErrorNumber& = PMFalse
            '
            '        ' Set the mouse pointer to normal.
            '        SetMousePointer PMMouseNormal
            '
            '        Exit Function
            '    End If

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                'Get addresse type lookups for the party

                m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retreive the address type look up details from the business object LoadControl", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
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

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveProspectPolicies
    '
    ' Description: This deletes all the prospect policy records then recreates
    ' them based on what's in the array
    '
    ' ***************************************************************** '
    Private Function SaveProspectPolicies() As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim bFirst As Boolean
        Dim oListItem As ListViewItem
        Dim vArray As Object
        'DC 27/09/00
        Dim cPremium As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Loop round the listview
            i = 1
            bFirst = True

            vArray = Nothing

            Do
                If i > lvwPolicies.Items.Count Then
                    Exit Do
                End If

                oListItem = lvwPolicies.Items.Item(i - 1)

                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vArray(3, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vArray(3, i - 1)
                    End If

                    'Type


                    vArray(0, i - 1) = Convert.ToString(oListItem.Tag)
                    'Renewal Date
                    txtHiddenDate.Text = ListViewHelper.GetListViewSubItem(oListItem, 1).Text


                    vArray(1, i - 1) = m_oFormFields.UnformatControl(txtHiddenDate)
                    'No Of Times Quoted

                    vArray(2, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text
                    'Target Premium
                    'DC 27/09/00 chnage way premium is unformatted
                    'txtHiddenCurrency.Text = oListItem.SubItems(3)
                    cPremium = CDec(ListViewHelper.GetListViewSubItem(oListItem, 3).Text)
                    txtHiddenCurrency.Text = CStr(cPremium)
                    'DC


                    vArray(3, i - 1) = m_oFormFields.UnformatControl(txtHiddenCurrency)

                End If
                i += 1
            Loop

            'Delete old policies from database

            m_lReturn = m_oProspect.DeletePolicies(vPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add new policies to database
            If Information.IsArray(vArray) Then

                m_lReturn = m_oProspect.AddPolicies(vPartyCnt:=m_lPartyCnt, vPolicies:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveProspectPolicies Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveProspectPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'sj 12/06/2002 - start
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
    ' Name: txtAlternativeIdentifier_KeyPress
    '
    ' Description:
    '
    ' History: 12/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'developer guide no. 78 (guide)
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
    'developer guide no. 78 (guide)
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
                PartyFunc.LoyaltyNumberLostFocus(v_oNewLoyaltyNumber:=txtLoyaltyNumber, v_sLoyaltyNumberScript:=m_sLoyaltyNumberScript, v_iTask:=Task)
            End If
            'sj 25/07/2002 - end

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="txtLoyaltyNumber_LostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="txtLoyaltyNumber_LostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtTobLetter_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtTobLetter.Enter

        SSTabHelper.SetSelectedIndex(tabMainTab, 4)
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

    'developer guide no. 78 (guide)
    Private Sub txtTradingName_KeyPress(ByVal eventSender As Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtTradingName.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        '************************************************************************
        'MKR 12/10/2004 PN 6021 -- Allowing "," in Trading Name and Making entry
        'of Trading Name consistant to other party types
        '    If KeyAscii <> 44 Then
        '        Call RestrictChars(KeyAscii, False)
        '    End If

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub


    'UPGRADE_NOTE: (7001) The following declaration (uctPartyBankControl1_RefreshBankDetails) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub uctPartyBankControl1_RefreshBankDetails(ByRef vBankDetails( ,  ) As Object)
    'm_vPartyBankDetails = vBankDetails
    'End Sub

    'sj 12/06/2002 - end
    'Initialize Properties for User Control

    Private Sub UserControl_InitProperties()
        m_BackColor = m_def_BackColor
        m_ForeColor = m_def_ForeColor
        m_Enabled = m_def_Enabled

        'developer guide no. 2 (no solution)
        m_Font = MyBase.Font
        m_BackStyle = m_def_BackStyle
        m_BorderStyle = m_def_BorderStyle
        m_PartyCnt = m_def_PartyCnt
    End Sub

    Private Sub uctPartyGCControl_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
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

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub uctPartyGCControl_Paint(ByVal eventSender As Object, ByVal eventArgs As PaintEventArgs) Handles MyBase.Paint
        Static bDoNotSetFocus As Boolean

        If Not DesignMode Then
            If Not bDoNotSetFocus Then
                If txtIDReference.Enabled Then
                    txtIDReference.Focus()
                ElseIf txtName.Enabled Then
                    txtName.Focus()
                End If
                bDoNotSetFocus = True
            End If
        End If
    End Sub

    'Load property values from storage


    'developer guide no. 1 (no solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)



        m_BackColor = CInt(PropBag.ReadProperty("BackColor", m_def_BackColor))


        m_ForeColor = CInt(PropBag.ReadProperty("ForeColor", m_def_ForeColor))


        m_Enabled = CBool(PropBag.ReadProperty("Enabled", m_def_Enabled))


        'developer guide no. 2 (guide)
        m_Font = PropBag.ReadProperty("Font", MyBase.Font)


        m_BackStyle = CInt(PropBag.ReadProperty("BackStyle", m_def_BackStyle))


        m_BorderStyle = CInt(PropBag.ReadProperty("BorderStyle", m_def_BorderStyle))


        m_PartyCnt = CInt(PropBag.ReadProperty("PartyCnt", m_def_PartyCnt))

    End Sub

    Private Sub uctPartyGCControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        ' Maintain minimum width
        If VB6.PixelsToTwipsX(Width) < 9015 Then Width = VB6.TwipsToPixelsX(9825)
        ' and minimum height
        If VB6.PixelsToTwipsY(Height) < 4995 Then Height = VB6.TwipsToPixelsY(4995)

    End Sub

    Private Sub UserControl_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        'developer guide no. 220(latest guide)
        Me.cboTurnover.FirstItem = ""

        'added following line to hide "Prospect Policy" tab against issue no 1730
        'Made following tab hidden as per 'Stephen Ross' mail
        Me.tabMainTab.TabPages.RemoveAt(6)
    End Sub

    'Write property values to storage


    'developer guide no. 1 (guide)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)


        PropBag.WriteProperty("BackColor", m_BackColor, m_def_BackColor)

        PropBag.WriteProperty("ForeColor", m_ForeColor, m_def_ForeColor)

        PropBag.WriteProperty("Enabled", m_Enabled, m_def_Enabled)


        'developer guide no. 2 (no solution)
        PropBag.WriteProperty("Font", m_Font, MyBase.Font)

        PropBag.WriteProperty("BackStyle", m_BackStyle, m_def_BackStyle)

        PropBag.WriteProperty("BorderStyle", m_BorderStyle, m_def_BorderStyle)

        PropBag.WriteProperty("PartyCnt", m_PartyCnt, m_def_PartyCnt)
    End Sub

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

            lblAgentNameLabel.Text = m_sAgentName

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultAgentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultAgentDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateClientCode
    '
    ' Description:
    '
    ' History: 23/08/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateClientCode(ByVal v_sName As String, ByRef r_sOriginalCode As String, ByRef r_sClientCode As String, ByRef r_bDuplicateFound As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vArray As Object
            Dim sText As String = ""
            Dim sName As New StringBuilder
            Dim iMax As Integer
            Dim iPartyFound As gPMConstants.PMEReturnCode
            Dim sOriginalReference As String = ""
            Dim lPartyCnt As Integer
            Dim iNumber As Integer
            Dim sClientcode As String = ""

            'Set maximum length of name
            If m_sUnderwritingOrAgency = "U" Then
                iMax = 16
            Else
                iMax = 9
            End If

            ReDim vArray(0)
            sText = v_sName

            While sText.Length > 0

                ReDim Preserve vArray(vArray.GetUpperBound(0) + 1)
                If sText.IndexOf(" "c) > 0 Then


                    vArray(vArray.GetUpperBound(0)) = sText.Substring(0, sText.IndexOf(" "c)).ToUpper()
                    sText = Mid(sText, (sText.IndexOf(" "c) + 1) + 1)
                Else


                    vArray(vArray.GetUpperBound(0)) = sText.ToUpper()
                    sText = ""
                End If
            End While


            If vArray.GetUpperBound(0) > 0 Then

                For iInt As Integer = 1 To vArray.GetUpperBound(0)

                    sText = CStr(vArray(iInt))
                    If sText = "LTD." Or sText = "LTD" Or sText = "LIMITED" Or sText = "CO." Or sText = "CO" Or sText = "COMPANY" Or sText = "PLC." Or sText = "PLC" Or sText = "&" Or sText = "SON" Or sText = "SONS" Or sText = "DAUGHTER" Or sText = "DAUGHTERS" Or sText = "ASSOC." Or sText = "ASSOC" Or sText = "ASSOCIATION" Or sText = "CLUB" Then
                        'Do nothing
                    Else
                        sName.Append(sText)
                    End If

                Next iInt
            End If

            'Remove illegal characters
            sText = sName.ToString()
            sName = New StringBuilder("")
            For iInt As Integer = 1 To sText.Length
                If sText.Substring(iInt - 1, 1) = " " Or sText.Substring(iInt - 1, 1) = "'" Or sText.Substring(iInt - 1, 1) = "|" Or sText.Substring(iInt - 1, 1) = "," Then
                Else
                    sName.Append(sText.Substring(iInt - 1, 1))
                End If
            Next iInt

            'MIPS Client Numbering

            If m_sUnderwritingOrAgency = "U" Then


                'Generate Client Code
                m_lReturn = GetClientCode(r_sClientCode:=sClientcode, v_sGroupName:=sName.ToString())
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sClientcode = "") Then
                    Return m_lReturn
                Else
                    sName = New StringBuilder(sClientcode.Trim())
                End If
            Else
                'Make the name the correct length for Underwriting(16) or Broking(9)
                If sName.ToString().Length > iMax Then
                    sName = New StringBuilder(sName.ToString().Substring(0, iMax))
                End If
            End If
            If m_sUnderwritingOrAgency = "U" Then
                iNumber = 1
            Else
                iNumber = 0
            End If

            iPartyFound = gPMConstants.PMEReturnCode.PMTrue
            r_bDuplicateFound = False
            r_sOriginalCode = sName.ToString()
            sOriginalReference = sName.ToString()

            Do While iPartyFound = gPMConstants.PMEReturnCode.PMTrue


                m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=sName.ToString(), vPartyCnt:=lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lPartyCnt <> 0 Then
                    iNumber += 1
                    If m_sUnderwritingOrAgency = "U" Then
                        If iNumber > 999 Then
                            MessageBox.Show("Client code counter exceeds 999." & Strings.Chr(13) & Strings.Chr(10) &
                                            "You must manually enter a unique client code.", Application.ProductName)
                            iPartyFound = gPMConstants.PMEReturnCode.PMFalse
                            txtIDReference.Enabled = True
                        Else
                            sName = New StringBuilder(sOriginalReference & StringsHelper.Format(CStr(iNumber), "000"))
                        End If
                    Else
                        sName = New StringBuilder(sOriginalReference & CStr(iNumber))
                    End If
                    r_bDuplicateFound = True
                Else
                    iPartyFound = gPMConstants.PMEReturnCode.PMFalse
                End If
            Loop

            r_sClientCode = sName.ToString()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateClientCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateClientCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            m_lReturn = oPartyBusiness.GetBaseCurrencyID(lSourceId:=lSourceId, iCurrencyID:=iBaseCurrencyID)

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
    ' PRIVATE Events (End)

    ' ***************************************************************** '
    '
    ' Name: ShowDuplicateParty
    '
    ' Description:
    '
    ' History: 24/01/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Private Function ShowDuplicateParty(ByVal sOriginalClientCode As String, ByVal sUniqueClientCode As String, ByRef sSelectedClientCode As String, ByRef lSelectedClientPartyCnt As Integer, ByRef iOKAction As Integer) As Integer

        Dim result As Integer = 0

        'developer guide no. 88 (guide)
        Dim oPMBPartyDuplicate As iPMBPartyDuplicate.Interface_Renamed
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
            oPMBPartyDuplicate.PartyTypeCode = gSIRLibrary.SIRPartyTypeGroupClient
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

            If m_oFormFields.Item("txtName-0").IsMandatory Then
                If txtName.Text.Trim().Length = 0 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Group Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    'developer guide no. 222(latest guide)
                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(tabMainTab)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    If txtName.Visible Then
                        txtName.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(tabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'ends
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If

            If m_oFormFields.Item("cboGroupType-0").IsMandatory Then
                If cboGroupType.Text.Trim().Length = 0 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Group Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'sj 18/06/2002 - start
                    'developer guide no. 222(latest guide)
                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(tabMainTab)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    If cboGroupType.Visible Then
                        cboGroupType.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(tabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'ends
                    'sj 18/06/2002 - end
                    Return result
                End If
            End If

            ' PN 62594
            If m_sUnderwritingOrAgency = "U" Then
                If m_bIsSetMaskingCode Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtTradingName.Focus()
                    m_lReturn = ValidateNumberingScheme()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If


            If txtCCJ.Text.Trim() <> "" Then
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtCCJ.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    MessageBox.Show("County court judgements must be a number", "Numeric Field", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'developer guide no. 222(latest guide)

                    Dim selIndex As Integer = 0
                    selIndex = SSTabHelper.GetSelectedIndex(tabMainTab)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    If txtCCJ.Visible Then
                        txtCCJ.Focus()
                    Else
                        SSTabHelper.SetSelectedIndex(tabMainTab, selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'ends
                    Return result
                End If
            End If

            If m_oFormFields.Item("cboTurnover-0").IsMandatory Then
                If cboTurnover.ListIndex = -1 Then
                    MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Turnover", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    'developer guide no. 222(latest guide)

                    Dim selIndex As Integer = 0
                    'selIndex = SSTabHelper.GetSelectedIndex(tabMainTab)
                    selIndex = tabMainTab.SelectedIndex
                    'SSTabHelper.SetSelectedIndex(tabMainTab, 4)
                    RemoveHandler cmdAssociates.Leave, AddressOf cmdAssociates_Leave
                    tabMainTab.SelectTab(4)
                    If cboTurnover.Visible Then
                        cboTurnover.Focus()
                    Else
                        'SSTabHelper.SetSelectedIndex(tabMainTab, selIndex)
                        tabMainTab.SelectTab(selIndex)
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'ends
                    AddHandler cmdAssociates.Leave, AddressOf cmdAssociates_Leave
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

            'developer guide no. 26(latest guide)
            lblClientBalanceLabel.Text = sFormattedCurrency


            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cYearToDateTurnover, cCurrencyAmount:=cConvertedAmount)

            m_cYearToDateTurnover = cConvertedAmount


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cYearToDateTurnover, vFormattedCurrency:=sFormattedCurrency)

            'developer guide no. 26(latest guide)
            lblYearToDateTurnoverLabel.Name = sFormattedCurrency


            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=m_iCurrencyId, lCompanyID:=g_iSourceID, cBaseAmount:=m_cLastYearTurnover, cCurrencyAmount:=cConvertedAmount)

            m_cLastYearTurnover = cConvertedAmount


            m_lReturn = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=m_iCurrencyId, vCurrencyAmount:=m_cLastYearTurnover, vFormattedCurrency:=sFormattedCurrency)

            'developer guide no. 26(latest guide)
            lblLastYearTurnoverLabel.Text = sFormattedCurrency

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


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

            If m_vBlackListReasonId = 0 Then
                r_vPartyDetails(kPartyDetailBlackListReasonId, 0) = DBNull.Value
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
    Private Function GetClientCode(ByRef r_sClientCode As String, ByVal v_sGroupName As String) As Integer

        Dim result As Integer = 0
        Dim sFailureReason As String = ""
        Dim iBranchId As Integer
        Dim sTradingName As String = ""

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
            sTradingName = txtTradingName.Text.Trim().ToUpper()


            m_lReturn = m_oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeGroupClient, v_iSourceID:=iBranchId, r_sGeneratedClientCode:=r_sClientCode, r_sFailureReason:=sFailureReason, v_sValue:=v_sGroupName, v_sTradeName:=sTradingName)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to generate client code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode")

                Return result
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                MessageBox.Show("Numbering scheme for Group Client is not set.", "Group Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return gPMConstants.PMEReturnCode.PMCancel
            ElseIf sFailureReason <> "" Then
                MessageBox.Show(sFailureReason, "Group Client", MessageBoxButtons.OK, MessageBoxIcon.Error)
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


            m_lReturn = m_oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeGroupClient, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails falied")
                Return result
            End If

            m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
            m_bIsReadOnly = r_bIsReadOnly
            m_sMaskCode = r_sMaskCode

            ' Start - Sankar - PN 61152
            If m_sMaskCode.IndexOf("T"c) >= 0 Then
                'Trading Name must be entered
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTradingName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("SetClientCodeCntl", "AddNewFormField falied", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            'End - Sankar - PN 61152

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
            m_bValidateNumberingScheme = False

            If m_sMaskCode <> "" Then
                ' Branch
                If m_sMaskCode.IndexOf("B"c) >= 0 Then
                    If cboBranch.SelectedIndex < 0 Or BranchId < 1 Then
                        MessageBox.Show("Please select some Branch", "field - Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        m_bValidateNumberingScheme = True
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If
                ' Trading Name
                If m_sMaskCode.IndexOf("T"c) >= 0 Then
                    If txtTradingName.Text.Trim() = "" Then
                        MessageBox.Show("This is a mandatory field. You must enter data in this field", "Mandatory Field - Trading Name", MessageBoxButtons.OK, MessageBoxIcon.Error) ' PN 62954
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        txtTradingName.Focus()
                        m_bValidateNumberingScheme = True
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

    Private Sub txtTradingName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtTradingName.Leave
        txtAlternativeIdentifier.Focus()
    End Sub

    Private Sub cboSubBranch_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSubBranch.Leave
        cmdAgentLookUp.Focus()
    End Sub

    Private Sub cboBlackListReason_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboBlackListReason.Leave
        chkProspect.Focus()
    End Sub

    Private Sub chkAgent_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAgent.Leave
        chkCharity.Focus()
    End Sub

    Private Sub cmdAddAd_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddAd.Leave

        SSTabHelper.SetSelectedTabIndex(tabMainTab, 2)

    End Sub

    Private Sub chkeMPS_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkeMPS.Leave

        SSTabHelper.SetSelectedTabIndex(tabMainTab, 3)
    End Sub

    Private Sub txtCCJ_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtCCJ.Leave
        '' SSTabHelper.SetSelectedTabIndex(tabMainTab, 4)
    End Sub

    Private Sub cmdAssociates_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAssociates.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 4)
    End Sub
    Private Sub cmdAddConv_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddConv.Leave
        txtCCJ.Focus()
    End Sub

    Private Sub cboSeasonalGift_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSeasonalGift.Leave
        cboArea.Focus()
    End Sub

    Private Sub txtFileCode_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFileCode.Leave
        cmdAssociates.Focus()
    End Sub

    Private Sub cmdAddLoyaltyScheme_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddLoyaltyScheme.Leave
        SSTabHelper.SetSelectedTabIndex(tabMainTab, 6)
    End Sub
    ''' <summary>
    ''' Add Party History
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddPartyHistory() As Integer
        Dim nReturn As Integer
        Dim oPartyBusiness As bSIRParty.Business
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

    Private Sub txtName_Validated(sender As Object, e As EventArgs) Handles txtName.Validated
        If m_oFormFields Is Nothing = False Then
            m_lReturn = m_lReturn & m_oFormFields.LostFocus(ctlControl:=txtName)
            uctPartyBankControl1.PartyName = Trim$(txtName.Text)
        End If
    End Sub
End Class
