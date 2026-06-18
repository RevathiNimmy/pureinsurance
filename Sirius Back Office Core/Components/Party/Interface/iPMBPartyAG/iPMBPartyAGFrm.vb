Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Windows.Forms
'developer giude no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 23/06/1998
    '
    ' Description: Main interface.
    '
    ' Edit History: TF031298 - Menu & Toolbar activity
    '               PW100702 - Add additional details tab
    ' MKR 10/02/2005 : PN 18683 - Used ItemData property instead of ListIndex
    '                  for cmbAgentType.
    ' CJB 21/06/2005 : PN15905 Various tidy up as it was in a mess
    ' CJB 20/07/2005 : PN22502 Changed InterfaceToData to only check if Registration Number was unique
    '                  if the optional field had a value
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"
    Private Const vbFormCode As Integer = 0
    Private Const ACPaymentMethodNA As String = "(N/A)"
    Private Const ACPaymentMethodEFT As String = "EFT"

    'Float Balance and Pre-Payment (RC)
    Const AC_PARTYAG_MakeLiveInvoice As Integer = 0
    Const AC_PARTYAG_MakeLiveInstallments As Integer = 1
    Const AC_PARTYAG_MakeLivePayNow As Integer = 2
    Const AC_PARTYAG_IsStandardAccount As Integer = 3
    Const AC_PARTYAG_IsFloatBalanceAccount As Integer = 4
    Const AC_PARTYAG_IsPrepaymentAccount As Integer = 5
    Const AC_PARTYAG_IsOverdraftAccount As Integer = 6
    Const AC_PARTYAG_FloatBalanceLimit As Integer = 7
    Const AC_PARTYAG_ExpectedDailyPremium As Integer = 8
    Const AC_PARTYAG_OverdraftLimit As Integer = 9
    Const AC_PARTYAG_DaysAllowed As Integer = 10
    Const AC_PARTYAG_OverdraftExpiry As Integer = 11
    '(RC) QBENZ014
    Const AC_PARTYAG_AltRefMandatory As Integer = 12
    Const AC_PARTYAG_AltRefRequiredForEachTrans As Integer = 13

    '(RC) PLICO 9-10
    Const AC_PARTYAG_CommissionPostingType As Integer = 14

    Const AC_PARTYAG_IsSingleInstalmentPlanOnly As Integer = 15
    Const AC_PARTYAG_CommonRenewalDate As Integer = 16
    'Batch Renewal
    Const AC_PARTYAG_IsProduceAgentRenewalList As Integer = 17
    Const AC_PARTYAG_MakeLiveBankGuarantee As Integer = 18 ' Gaurav
    Const AC_PARTYAG_MakeLiveCashDeposit As Integer = 19 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
    Const AC_PARTYAG_MakeLiveIsGrossAgent As Integer = 20

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


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
    Private m_lAddressCount As Integer
    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyAG.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    'DC131204
    Private m_oFindBusiness As bSIRFindParty.Business

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    'EK 10/10/99 Add access to Commission Rates
    ' Declare an instance of the Agent Rates interface.

    Private m_oRates As Object

    ' Variables to store the lookup values/details.
    Private m_vLookupValues(,) As Object
    Private m_vLookupDetails(,) As Object

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
    ' PW090702 - add variables required for new additional detail tab
    Private m_sConsultantName As String = ""
    Private m_sConsultantRef As String = ""
    Private m_lConsultantCnt As Integer
    Private m_bVerifyConsultantCnt As Boolean
    Private m_sAgentGroupName As String = ""
    Private m_sAgentGroupRef As String = ""
    Private m_lAgentGroupCnt As Integer
    Private m_bVerifyAgentGroupCnt As Boolean
    Private m_vAssociates As Object
    Private m_vRelationships As Object
    Private m_vPaymentMethod As Integer
    Private m_vPaymentFrequency As Integer
    Private m_vAddressOnNotice As Integer
    Private m_iMultipac As MainModule.ACYesNoComboOption
    Private m_vTypeOfStatement As Object
    Private m_vSource As Object
    Private m_sTitle As String = ""
    'developer guide no.101
    'start
    Private m_vBranch As Object
    Private m_vSubBranch As Object
    Private m_vRenewalStopCode As Object
    Private m_vDocsSuppressed As Object
    Private m_vDocsAvailable As Object
    'end
    Private m_sContactPerson As String = ""
    Private m_sFirstName As String = ""
    Private m_sBankAccount As String = ""
    Private m_bConsolidate As Boolean
    Private m_vDateCancelled As Object
    Private m_iIsInTransferMode As CheckState
    Private m_lTransferToPartyCnt As Integer
    Private m_lTransferToBusinessTypeID As Integer

    'Float Balance and Pre-Payment (RC)
    Private m_vMakeLiveArray() As Object

    'Addresses and Contacts
    Private m_iLine As Integer
    Private m_lAddressCnt As Integer
    Private m_lPartyAgentTypeID As Integer
    Private m_lAddressUsageTypeID As Integer

    Private m_vCorrespondenceTypeId As Object
    Private m_vCorrespondenceTypes(,) As Object

    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_sShortName_Original As String = "" 'AR20050317 - PN18400
    Private m_sMainPostCode As String = ""
    Private m_sName As String = ""
    Private m_iPartyAgentOriginID As Integer
    Private m_iIsBranch As Integer
    Private m_iIsHeadOffice As Integer
    Private m_dtAgencyAgreement As Date
    Private m_dtAgencyNextReview As Date
    Private m_lHeadOfficeCnt As Object
    Private m_sHeadOfficeRef As String = ""
    Private m_vAddresses(,) As Object
    Private m_vAddressTypes(,) As Object
    Private m_vContacts(,) As Object
    Private m_sAddressLine1 As String = ""
    Private m_iStatement As Integer
    Private m_sFileCode As String = ""
    Private m_lCurrencyID As Integer
    Private m_iTermsOfPaymentID As Integer
    Private m_lPartyCategoryID As Integer
    Private m_iOverride As Integer
    Private m_iOverrideRenewal As Integer

    'Batch Renewal
    Private m_iProduceAgentRenewalList As Integer

    'RWH(24/07/2000) RSAIB Process 004.
    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    'DC131204
    'developer guide no. 101
    Private m_vAgentTypes As Object

    'CMG/PB 16072002
    Private m_bDataChanged As Boolean

    ' CF 280699
    Private m_vDefaultCommissionPercent As Object
    Private m_sAgencyAccountNumber As String = ""
    Private m_sTradingName As String = ""
    Private m_lBinderIndicator As Integer
    Private m_lReportIndicator As Integer

    'DC220803 -PS253 -fsa compliance
    Private m_vRiskGroups(,) As Object
    Private m_vAgentStatus As Object
    'DC021203 -PN -fsa compliance -registration number
    'developer guide no.101
    Private m_vRegistrationNumber As Object
    Private m_vRegistrationNumber_Original As Object 'PN 21971

    'TN20001117
    Private m_sAgencyOrUnderwriting As String = ""

    ' PW150702
    Private m_bIsNRMA As Boolean

    'Flag to indicate whether we need to check the headoffice id matches
    'the headoffice ref as user may change the reference directly
    Private m_bVerifyHeadOfficeCnt As Boolean

    'Note the index in the lookup array of the main address
    Private m_iMainAddressIndex As Integer

    ' PW100702 - Declare an instance of the associates interface.

    Private m_oAssociates As Object
    Private m_oCommissionLevel As Object
    Private m_vCommissionLevel(,) As Object

    ' Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    ' Declare an instance of the contact interface.

    Private m_oContact As iPMBContact.Interface_Renamed

    ' Gemini List Manager
    Private m_oListManager As iGEMListManager.Interface_Renamed

    ' PW110702

    Private m_oPMUser As Bpmuser.Business

    'DC220803 -PS253 -fsa compliance
    Private m_bEnableFSACompliance As Boolean
    Private bVisibleCertificateYearTab As Boolean = False
    Private m_bUnderwritingBranchEnabled As Boolean
    Private m_bIsUnderwritingBranch As Boolean
    Private m_sBrokerAbiId As String = ""
    Private m_bShowSubBranchID As Boolean

    'DC141204 added expense account id
    'developer guide no.101
    Private m_vExpenseAccountId As Object

    'AR20050317 - PN15901
    Private Const ACTAB_AGENCY As Integer = 0
    Private Const ACTAB_ADDRESS As Integer = 1
    Private Const ACTAB_CONTACTS As Integer = 2
    Private Const ACTAB_PAYMENTS As Integer = 3
    Private Const ACTAB_DOCUMENTS As Integer = 4
    Private Const ACTAB_ADDITIONAL As Integer = 5
    Private Const ACTAB_FSA As Integer = 6

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double
    '**************************************************
    Private m_vSourceArray(,) As Object
    Private m_bIsViewableOnly As Boolean

    Private m_bChanged As Boolean ''Agent Filtering
    Private m_iBankAccount As Integer

    Public Enum EnabledLevel
        elEnabled = 0
        elDisabledStandard = 1
        elDisabledTotal = 2
    End Enum
    Private m_vPayNowOptionValue As Object

    'Maintain Party Code
    Private m_bIsSetMaskingCode As Boolean
    Private m_bIsReadOnly As Boolean
    Private m_bIsViewOnlyAgent As Boolean
    Private m_sMaskCode As String = ""

    'Party Bank Details
    Private m_vPartyBankDetails(,) As Object
    Private m_vPartyBankHistory As Object

    'Party View
    Private m_bIsViewOnlyAgentMaintenance As Boolean
    Private m_lCommissionLevel As Integer
    Private m_vAgencyUsers As Object
    Private m_vCertificateYearDetails(,) As Object
    Private m_bReceivesClientCorrespodence As Boolean
    Private m_sUniqueId As String
    Private m_SScreenHierarchy As String
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property CommissionLevel() As Integer
        Get
            CommissionLevel = m_lCommissionLevel
        End Get
        Set(ByVal Value As Integer)
            m_lCommissionLevel = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Relationships
    '
    ' Description:
    '
    ' History: 26/09/02 APS - Created.
    '
    ' ***************************************************************** '
    Public WriteOnly Property Relationships() As Object
        Set(ByVal Value As Object)
            m_vRelationships = Value
        End Set
    End Property


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
    Public Property MainPostCode() As String
        Get

            Return m_sMainPostCode

        End Get
        Set(ByVal Value As String)

            m_sMainPostCode = Value

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

    Public ReadOnly Property IsViewableOnly() As Boolean
        Get
            Return m_bIsViewableOnly
        End Get
    End Property

    Public WriteOnly Property IsViewOnly() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsViewOnlyAgent = Value

        End Set
    End Property

    Public ReadOnly Property BranchId() As Integer
        Get
            Dim result As Integer = 0
            If uctBranch.SelectedIndex > -1 Then
                result = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)
            End If
            Return result
        End Get
    End Property

    'Party Bank Details
    Public ReadOnly Property PartyBankDetails() As Object
        Get
            Return VB6.CopyArray(m_vPartyBankDetails)
        End Get
    End Property
    'Party Bank Details
    Public ReadOnly Property PartyBankHistory() As Object
        Get
            Return m_vPartyBankHistory
        End Get
    End Property
    ' PRIVATE Property Procedures (End)

    'Maintain Party Code
    Private ReadOnly Property TradeName() As String
        Get
            Return gPMFunctions.ToSafeString(txtName.Text.Replace(" ", "").ToUpper())
        End Get
    End Property

    Private ReadOnly Property AgentType() As String
        Get
            Return gPMFunctions.ToSafeString(uctAgentType.ItemCode.Replace(" ", "").ToUpper())
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


    '' ***************************************************************** '
    ''
    '' Name: GetSubBranchDetails
    ''
    '' Description:
    ''
    '' History: 11/06/2002 SJ - Created.
    ''
    '' ***************************************************************** '
    'Public Function GetSubBranchDetails( _
    ''    ByRef r_oSubBranch As ComboBox, _
    ''    ByRef r_oBranch As cboPMLookup, _
    ''    ByRef r_oBusiness As Object, _
    ''    ByVal v_lSubBranchId As Long) As Long
    '
    '    On Error GoTo Err_GetSubBranchDetails
    '
    '    GetSubBranchDetails = PMTrue
    '
    '    Const ACSubBranchId = 0
    '    Const ACSubBranchDescription = 3
    '
    '    Dim lIndex As Long
    '    Dim lSourceId As Long
    '    Dim vSubBranchArray As Variant
    '    Dim i As Integer
    '
    '    r_oSubBranch.Clear
    '
    '    lIndex = r_oBranch.ListIndex
    '    If lIndex < 0 Then
    '        Exit Function
    '    End If
    '
    '    lSourceId = r_oBranch.ItemId
    '
    '    m_lReturn = r_oBusiness.GetSubBranches( _
    ''        v_lSourceId:=lSourceId, _
    ''        r_vSubBranchArray:=vSubBranchArray)
    '    If m_lReturn <> PMTrue Then
    '
    '    End If
    '    If IsArray(vSubBranchArray) = False Then
    '        Exit Function
    '    End If
    '    For i = 0 To UBound(vSubBranchArray, 2)
    '        r_oSubBranch.AddItem (vSubBranchArray(ACSubBranchDescription, i))
    '        r_oSubBranch.ItemData(r_oSubBranch.NewIndex) = CLng(vSubBranchArray(ACSubBranchId, i))
    '        If CLng(vSubBranchArray(ACSubBranchId, i)) = v_lSubBranchId Then
    '            r_oSubBranch.ListIndex = r_oSubBranch.NewIndex
    '        End If
    '    Next i
    '
    '    If v_lSubBranchId = 0 Then
    '        r_oSubBranch.ListIndex = -1
    '    End If
    '
    '    Exit Function
    '
    'Err_GetSubBranchDetails:
    '
    '    GetSubBranchDetails = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="GetSubBranchDetails Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="GetSubBranchDetails", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    ' ***************************************************************** '
    ' Name: UpdateAssociates
    '
    ' Description: This goes thru all Associates in the the grid control
    ' and the original Associate array and sees what the differences
    ' are. It then adds new Associates or deletes existing ones according
    ' to what user has done.
    '
    ' PW100702 - copied from PartyPCControl
    '
    ' ***************************************************************** '

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
    ' Name: PopulateAssociates
    '
    ' Description: Fills the grid control with contact details
    ' PW100702 - copied from PartyPCControl
    '
    ' ***************************************************************** '
    Private Sub PopulateAssociates()

        Dim oListItem As ListViewItem
        Dim sShortName, sName As String

        Try

            If Not Information.IsArray(m_vAssociates) Then
                Exit Sub
            End If

            lvwAssociates.Items.Clear()

            ' Assign the details to the interface.

            For i As Integer = m_vAssociates.GetLowerBound(1) To m_vAssociates.GetUpperBound(1)


                If CStr(m_vAssociates(1, i)) <> "" Then

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to the first column.
                    ' Column 1 - Associate Shortname (Code)

                    sShortName = CStr(m_vAssociates(1, i)).Trim()
                    oListItem = lvwAssociates.Items.Add(sShortName)

                    ' Assign details to other the columns
                    'SD 14/08/2002 START Add Name to listview items

                    'the name to display must be obtained via the shortname
                    GetAssociateNameFromShortName(sShortName, sName)
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sName

                    ' Column 3 - Relationship Description

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vAssociates(4, i)).Trim()
                    'SD 14/08/2002 END Add Name
                    ' Store the relation_cnt

                    oListItem.Tag = CStr(m_vAssociates(0, i)).Trim()
                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.

                End If

            Next i
            '    'Populate the cells

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAssociates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    ' Name: PopulateSuppressedDocs
    '
    ' Description: Fills the list with suppressed documents
    ' PW150702 - created
    '
    ' ***************************************************************** '
    Private Sub PopulateSuppressedDocs()

        Dim lProcessType As Integer

        Try

            If Not Information.IsArray(m_vDocsSuppressed) Then
                Exit Sub
            End If

            ' Assign the details to the interface.
            lstDocsChosen.Items.Clear()

            For i As Integer = m_vDocsSuppressed.GetLowerBound(1) To m_vDocsSuppressed.GetUpperBound(1)
                ' Store the process type
                lProcessType = CInt(m_vDocsSuppressed(0, i))
                ' Remove the process type from the available docs and move it to the chosen docs
                For j As Integer = m_vDocsAvailable.GetLowerBound(1) To m_vDocsAvailable.GetUpperBound(1)
                    If CInt(m_vDocsAvailable(0, j)) = lProcessType Then
                        Dim lstDocsChosen_NewIndex As Integer = -1
                        lstDocsChosen_NewIndex = lstDocsChosen.Items.Add(CStr(m_vDocsAvailable(1, j)))
                        VB6.SetItemData(lstDocsChosen, lstDocsChosen_NewIndex, lProcessType)

                        Exit For
                    End If
                Next j
            Next i

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateSuppressedDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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

            'Party Source must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboSource, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Reference must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtIDReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Account Number can be null
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccountNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' File Code can be null
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFileCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Tomo28062001 - changed from PMFormatStringCase
            'Name must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Agency Next Review date must be entered
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgencyReviewDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctAgentType, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bIsUnderwritingBranch Then
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtBrokerAbiId, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=uctBranch, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW100702 - Consultant Ref
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtConsultantRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW100702 - Agent Group Ref
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAgentGroupRef, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW170702 - payment method
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPaymentMethod, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW170702 - address on notice
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboAddressOnNotice, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' cancelled date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateCancelled, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bIsNRMA Then

                'SD 14/08/2002 START NRMA changes combo boxes are mandatory, but do not
                'set this as it only applies to NRMA. Validate these fields separately
                '
                ' PW040902 - set aforementioned combo boxes back to mandatory. If we
                ' are not running as NRMA then these won't be set thanks to the
                ' cunningly placed 'if' statement encompassing this code!
                '

                ' PW170702 - multipac
                m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboMultipac, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Overdraft expiry date
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtOverdraftExpiry, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommonRenewalDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Commission Posting Type is mandatory
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboCommissionPostingType, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                SetFieldValidation = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0

        Try

            Dim vMakeLiveInvoice, vMakeLiveInstallments, vMakeLivePayNow, vMakeLiveBankGuarantee, vIsStandardAccount, vIsFloatBalanceAccount As Object
            Dim vIsPrepaymentAccount, vIsOverdraftAccount, vFloatBalanceLimit, vExpectedDailyPremium, vOverdraftLimit, vDaysAllowed, vOverdraftExpiry As Object
            Dim bAltRefMandatory, bAltRefRequiredForEachTrans As Boolean

            Dim bIsSingleInstalmentPlanOnly As Boolean
            Dim vCommonRenewalDate, vMakeLiveCashDeposit As Object
            Dim sTemp As String = ""
            'Batch Renewal
            Dim bIsProduceAgentRenewalList As Boolean
            Dim bIsGrossAgent As Boolean
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

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtIDReference, vControlValue:=m_sShortName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtName, vControlValue:=m_sName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccountNumber, vControlValue:=m_sAgencyAccountNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bIsUnderwritingBranch Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtBrokerAbiId, vControlValue:=m_sBrokerAbiId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            uctAgentType.ItemId = m_lPartyAgentTypeID


            cboBinderIndicator.SelectedIndex = m_lBinderIndicator
            cboReportIndicator.SelectedIndex = m_lReportIndicator

            uctCategory.ItemId = m_lPartyCategoryID

            If m_iStatement = 0 Then
                chkStatement.CheckState = CheckState.Unchecked
            Else
                chkStatement.CheckState = CheckState.Checked
            End If

            'Commission Coverride
            If m_iOverride = 0 Then
                chkOverrideCommission.CheckState = CheckState.Unchecked
            Else
                chkOverrideCommission.CheckState = CheckState.Checked
            End If
            'Commission Coverride Renewal
            If m_iOverrideRenewal = 0 Then
                chkOverrideCommissionRen.CheckState = CheckState.Unchecked
            Else
                chkOverrideCommissionRen.CheckState = CheckState.Checked
            End If
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFileCode, vControlValue:=m_sFileCode)

            'cboTermsOfPayment.Text = m_sTermsOfPayment
            If m_iTermsOfPaymentID = 0 Then
                cboTermsOfPayment.SelectedIndex = -1
            Else
                For i As Integer = 0 To cboTermsOfPayment.Items.Count - 1
                    If VB6.GetItemData(cboTermsOfPayment, i) = m_iTermsOfPaymentID Then
                        cboTermsOfPayment.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgencyReviewDate, vControlValue:=m_dtAgencyNextReview)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC021203 -PN -fsa compliance -registration number
            txtRegistrationNumber.Text = gPMFunctions.NullToString(m_vRegistrationNumber)

            ' PW100702 - consultant ref
            BrokerTransferToInterface()

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sTemp = m_sConsultantName
            m_lReturn = DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            plblConsultantName.Text = sTemp

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentGroupRef, vControlValue:=m_sAgentGroupRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sTemp = m_sAgentGroupName
            m_lReturn = DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            plblAgentGroupName.Text = sTemp

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ddTitle.Text = m_sTitle

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

            uctCurrency.CompanyId = m_vBranch
            uctCurrency.RefreshList()

            cboMultipac.SelectedIndex = m_iMultipac

            If m_vSubBranch = 0 Then
                cboSubBranch.SelectedIndex = -1
            Else
                For i As Integer = 0 To cboSubBranch.Items.Count - 1
                    If VB6.GetItemData(cboSubBranch, i) = m_vSubBranch Then
                        cboSubBranch.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

            txtContactPerson.Text = m_sContactPerson
            txtFirstName.Text = m_sFirstName

            If m_bConsolidate Then
                chkConsolidatedCommission.CheckState = CheckState.Checked
            Else
                chkConsolidatedCommission.CheckState = CheckState.Unchecked
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDateCancelled, vControlValue:=m_vDateCancelled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            uctCurrency.CurrencyId = m_lCurrencyID

            'Fill the contact grid
            PopulateContacts()

            'Fill the address grid
            PopulateAddresses()

            'DC220803 -PS253 -fsa compliance
            If m_bEnableFSACompliance Then
                DisplayRiskGroups()
            End If

            'Party Bank Details
            LoadPartyBankControl()

            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage

            'Float Balance and Pre-Payment (RC)
            If Information.IsArray(m_vMakeLiveArray) Then

                vMakeLiveInvoice = m_vMakeLiveArray(AC_PARTYAG_MakeLiveInvoice)

                vMakeLiveInstallments = m_vMakeLiveArray(AC_PARTYAG_MakeLiveInstallments)

                vMakeLivePayNow = m_vMakeLiveArray(AC_PARTYAG_MakeLivePayNow)

                vMakeLiveBankGuarantee = m_vMakeLiveArray(AC_PARTYAG_MakeLiveBankGuarantee)

                vIsStandardAccount = m_vMakeLiveArray(AC_PARTYAG_IsStandardAccount)

                vIsFloatBalanceAccount = m_vMakeLiveArray(AC_PARTYAG_IsFloatBalanceAccount)

                vIsPrepaymentAccount = m_vMakeLiveArray(AC_PARTYAG_IsPrepaymentAccount)

                vIsOverdraftAccount = m_vMakeLiveArray(AC_PARTYAG_IsOverdraftAccount)

                vFloatBalanceLimit = m_vMakeLiveArray(AC_PARTYAG_FloatBalanceLimit)

                vExpectedDailyPremium = m_vMakeLiveArray(AC_PARTYAG_ExpectedDailyPremium)

                vOverdraftLimit = m_vMakeLiveArray(AC_PARTYAG_OverdraftLimit)

                vDaysAllowed = m_vMakeLiveArray(AC_PARTYAG_DaysAllowed)

                vOverdraftExpiry = m_vMakeLiveArray(AC_PARTYAG_OverdraftExpiry)
                '(RC) QBENZ014
                bAltRefMandatory = CBool(m_vMakeLiveArray(AC_PARTYAG_AltRefMandatory))
                bAltRefRequiredForEachTrans = CBool(m_vMakeLiveArray(AC_PARTYAG_AltRefRequiredForEachTrans))
                bIsSingleInstalmentPlanOnly = CBool(m_vMakeLiveArray(AC_PARTYAG_IsSingleInstalmentPlanOnly))

                vCommonRenewalDate = m_vMakeLiveArray(AC_PARTYAG_CommonRenewalDate)
                'Batch Renewal
                bIsProduceAgentRenewalList = CBool(m_vMakeLiveArray(AC_PARTYAG_IsProduceAgentRenewalList))

                vMakeLiveCashDeposit = m_vMakeLiveArray(AC_PARTYAG_MakeLiveCashDeposit) 'Sankar - (UIIC_WPR85_Cash_Deposit_Process) - Paralleling
                bIsGrossAgent = m_vMakeLiveArray(AC_PARTYAG_MakeLiveIsGrossAgent)

                If CBool(vMakeLiveInvoice) Then
                    chkMakeLiveInvoice.CheckState = CheckState.Checked
                End If

                If CBool(vMakeLiveInstallments) Then
                    chkMakeLiveInstallments.CheckState = CheckState.Checked
                End If

                If CBool(vMakeLivePayNow) Then
                    chkMakeLivePayNow.CheckState = CheckState.Checked
                End If

                If CBool(vMakeLiveBankGuarantee) Then
                    chkMakeLiveBankGuarantee.CheckState = CheckState.Checked
                End If

                If CBool(vMakeLiveCashDeposit) Then
                    chkMakeLiveCashDeposit.CheckState = CheckState.Checked
                End If

                If CBool(vIsStandardAccount) Then
                    chkStandardAccount.CheckState = CheckState.Checked
                End If

                If CBool(vIsFloatBalanceAccount) Then
                    chkFloatBalanceAccount.CheckState = CheckState.Checked
                End If

                If CBool(vIsPrepaymentAccount) Then
                    chkPrepaymentAccount.CheckState = CheckState.Checked
                End If

                If CBool(vIsOverdraftAccount) Then
                    chkOverdraftAccount.CheckState = CheckState.Checked
                End If

                txtFloatBalanceLimit.Text = CStr(gPMFunctions.ToSafeCurrency(vFloatBalanceLimit))
                txtExpectedDailyPremium.Text = CStr(gPMFunctions.ToSafeCurrency(vExpectedDailyPremium))
                txtOverdraftLimit.Text = CStr(gPMFunctions.ToSafeCurrency(vOverdraftLimit))
                txtDaysAllowed.Text = CStr(gPMFunctions.ToSafeInteger(vDaysAllowed))

                If Information.IsDate(vOverdraftExpiry) Then

                    txtOverdraftExpiry.Text = CStr(vOverdraftExpiry)
                End If

                'PN 33575 (RC)
                If uctAgentType.ItemCaption = "Sub-Agent" Then
                    fraAltRef.Enabled = False
                Else
                    '(RC) QBENZ014
                    If bAltRefMandatory Then
                        chkAltRefMandatory.CheckState = CheckState.Checked
                    End If
                    If bAltRefRequiredForEachTrans Then
                        chkAltRefRequiredForEachTrans.CheckState = CheckState.Checked
                    End If
                End If

                If bIsSingleInstalmentPlanOnly Then
                    chkSingleInstalmentPlanOnly.CheckState = CheckState.Checked
                Else
                    chkSingleInstalmentPlanOnly.CheckState = CheckState.Unchecked
                End If

                If Information.IsDate(vCommonRenewalDate) Then
                    txtCommonRenewalDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, vCommonRenewalDate)
                End If

                'Batch Renewal
                If bIsProduceAgentRenewalList Then
                    chkProductAgentRenewalList.CheckState = CheckState.Checked
                Else
                    chkProductAgentRenewalList.CheckState = CheckState.Unchecked
                End If

            End If

            If uctAgentType.ItemCaption = "Sub-Agent" Then
                fraAccountLimits.Visible = False
            End If
            If uctAgentType.ItemCaption = "Broker" Then
                If bIsGrossAgent Then
                    OptGrossOfCommission.Checked = 1
                Else
                    OptNetOfcommission.Checked = 1
                End If
            End If

            If m_iBankAccount = 0 Then
                cboBankAccount.ListIndex = m_iBankAccount
            Else
                For i As Integer = 0 To cboBankAccount.ListCount - 1
                    If cboBankAccount.ItemData(i) = m_iBankAccount Then
                        cboBankAccount.ListIndex = i
                        Exit For
                    End If
                Next
            End If

            'Fill the Correspondence Type Combo Box
            PopulateCorrespondenceTypes()

            If m_bReceivesClientCorrespodence Then
                chkReceivesClientCorr.CheckState = CheckState.Checked
            Else
                chkReceivesClientCorr.CheckState = CheckState.Unchecked
            End If

            'Fill the CertificateYear grid
            PopulateCertYear()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Populate Corresondence Types", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCorrespondenceTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

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
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_SScreenHierarchy = $"Agent({txtIDReference.Text.ToString().Trim()})"
            ' Check the task.
            Select Case (m_iTask)

                Case gPMConstants.PMEComponentAction.PMAdd

                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vPartyAgentOriginID:=m_iPartyAgentOriginID, vPartyAgentTypeID:=m_lPartyAgentTypeID,
                                                    vIsBranch:=m_iIsBranch, vIsHeadOffice:=m_iIsHeadOffice, vAgencyAgreementDate:=m_dtAgencyAgreement,
                                                    vAgencyNextReviewDate:=m_dtAgencyNextReview, vAgencyAccountNumber:=m_sAgencyAccountNumber,
                                                    vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sName, vAgentCnt:=m_lHeadOfficeCnt,
                                                    vDefaultCommissionPercent:=m_vDefaultCommissionPercent, vTradingName:=m_sTradingName,
                                                    vBinderIndicator:=m_lBinderIndicator, vReportIndicator:=m_lReportIndicator, vCurrencyID:=m_lCurrencyID,
                                                    vFileCode:=m_sFileCode, vStatements:=m_iStatement, vPFFrequencyID:=m_iTermsOfPaymentID,
                                                    vPartyCategoryID:=m_lPartyCategoryID, vConsultantCnt:=m_lConsultantCnt, vAgentGroupCnt:=m_lAgentGroupCnt,
                                                    vpaymentmethod:=m_vPaymentMethod, vpaymentfrequency:=m_vPaymentFrequency, vaddressonnotice:=m_vAddressOnNotice,
                                                    vtypeofstatement:=m_vTypeOfStatement, vsource:=m_vSource, vtitle:=m_sTitle, vbranch:=m_vBranch,
                                                    vSubBranch:=m_vSubBranch, vRenewalstopcode:=m_vRenewalStopCode, vmultipac:=m_iMultipac,
                                                    vContactPerson:=m_sContactPerson, vFirstName:=m_sFirstName,
                                                    vDateCancelled:=m_vDateCancelled, vAgentStatus:=m_vAgentStatus,
                                                    vRegistrationNumber:=gPMFunctions.NullToString(m_vRegistrationNumber), vBrokerAbiId:=m_sBrokerAbiId,
                                                    vExpenseAccountId:=m_vExpenseAccountId, vIsInTransferMode:=m_iIsInTransferMode,
                                                    vTransferToBusinessTypeID:=m_lTransferToBusinessTypeID, vTransferToPartyCnt:=m_lTransferToPartyCnt,
                                                    vOverrideCommission:=m_iOverride, vOverrideCommissionRenewal:=m_iOverrideRenewal, vDomiciledForTax:=m_bDomiciledForTax, vAllowConsolidate:=m_bConsolidate,
                                                            vParamArray:=m_vMakeLiveArray, v_lCommissionLevel:=m_lCommissionLevel, vBankAccount:=m_iBankAccount,
                                                                    vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vReceivesClientCorr:=m_bReceivesClientCorrespodence, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_SScreenHierarchy)


                Case gPMConstants.PMEComponentAction.PMEdit

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyAgentOriginID:=m_iPartyAgentOriginID, vPartyAgentTypeID:=m_lPartyAgentTypeID,
                                                       vIsBranch:=m_iIsBranch, vIsHeadOffice:=m_iIsHeadOffice, vAgencyAgreementDate:=m_dtAgencyAgreement,
                                                       vAgencyNextReviewDate:=m_dtAgencyNextReview, vAgencyAccountNumber:=m_sAgencyAccountNumber,
                                                       vShortName:=m_sShortName, vName:=m_sName, vResolvedName:=m_sName, vAgentCnt:=m_lHeadOfficeCnt,
                                                       vDefaultCommissionPercent:=m_vDefaultCommissionPercent, vTradingName:=m_sTradingName,
                                                       vBinderIndicator:=m_lBinderIndicator, vReportIndicator:=m_lReportIndicator, vCurrencyID:=m_lCurrencyID,
                                                       vFileCode:=m_sFileCode, vStatements:=m_iStatement, vPFFrequencyID:=m_iTermsOfPaymentID,
                                                       vPartyCategoryID:=m_lPartyCategoryID, vConsultantCnt:=m_lConsultantCnt, vAgentGroupCnt:=m_lAgentGroupCnt,
                                                       vpaymentmethod:=m_vPaymentMethod, vpaymentfrequency:=m_vPaymentFrequency, vaddressonnotice:=m_vAddressOnNotice,
                                                       vtypeofstatement:=m_vTypeOfStatement, vsource:=m_vSource, vtitle:=m_sTitle, vbranch:=m_vBranch,
                                                       vSubBranch:=m_vSubBranch, vRenewalstopcode:=m_vRenewalStopCode, vmultipac:=m_iMultipac,
                                                       vContactPerson:=m_sContactPerson, vFirstName:=m_sFirstName,
                                                       vDateCancelled:=m_vDateCancelled, vAgentStatus:=m_vAgentStatus,
                                                       vRegistrationNumber:=gPMFunctions.NullToString(m_vRegistrationNumber), vBrokerAbiId:=m_sBrokerAbiId,
                                                       vExpenseAccountId:=m_vExpenseAccountId, vIsInTransferMode:=m_iIsInTransferMode,
                                                       vTransferToBusinessTypeID:=m_lTransferToBusinessTypeID, vTransferToPartyCnt:=m_lTransferToPartyCnt,
                                                       vOverrideCommission:=m_iOverride, vOverrideCommissionRenewal:=m_iOverrideRenewal, vDomiciledForTax:=m_bDomiciledForTax, vAllowConsolidate:=m_bConsolidate,
                                                               vParamArray:=m_vMakeLiveArray, v_lCommissionLevel:=m_lCommissionLevel, vBankAccount:=m_iBankAccount,
                                                                       vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vReceivesClientCorr:=m_bReceivesClientCorrespodence, vUniqueId:=m_sUniqueId, vScreenHierarchy:=m_SScreenHierarchy)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
                Return result
            End If

            'SD 14/08/2002 Document List can be stored with existing party count for edited records
            ' otherwise we need to run update function on business interface
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = StoreSuppressedDocumentList(m_lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
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

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPartyAgentOrigin, ctlLookup:=cboSource)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC220803 -PS253 -fsa compliance
            If m_bEnableFSACompliance Then

                m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupFSAAgentStatus, ctlLookup:=cboAgentStatus)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'RDT PN18099 - Call Get branch for the list of branches available to this user
            m_lReturn = GetBranchDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboPaymentMethod.ItemId = m_vPaymentMethod

            ' PW110702 - payment frequency lookup
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPaymentFrequency, ctlLookup:=cboPaymentFrequency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupPFFrequency, ctlLookup:=cboTermsOfPayment)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW110702 - address on notice lookup
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupAddressOnNotice, ctlLookup:=cboAddressOnNotice)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW110702 - renewal stop code lookup
            m_lReturn = GetLookupDetails(sLookupTable:=gSIRLibrary.SIRLookupRenewalStopCode, ctlLookup:=cboRenewalStopCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW110702 - Get sub branches
            m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                DisplayLookupDetails = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


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
    ' Name: ShowRates
    '
    ' Description: Entry point to show rates from SIRToolbarFunc
    '
    ' MSS260701 - Created
    ' ***************************************************************** '
    Public Function ShowRates() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdRates_Click(cmdRates, New EventArgs())

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show rates", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateAddresses
    '
    ' Description: Fills the grid control with address details
    '
    ' ***************************************************************** '
    Private Sub PopulateAddresses()
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim k As Integer
        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            'Just go if no addresses
            If Not Information.IsArray(m_vAddresses) Then
                Exit Sub
            End If
            lvwAddress.Items.Clear()

            m_lAddressCount = 0

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                'First column.
                For k = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    If m_vAddresses(1, i).Equals(m_vAddressTypes(0, k)) Then
                        'RWH(24/07/2000)
                        sAddressUsage = CStr(m_vAddressTypes(1, k)).Trim()
                        Exit For
                    End If
                Next k
                'See if this is the main address
                If CStr(m_vAddressTypes(2, k)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                    m_sMainPostCode = CStr(m_vAddresses(0, i))
                    m_iMainAddressIndex = CInt(m_vAddressTypes(0, k))
                End If

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Assign the details to the first column.
                        ' Postcode

                        oListItem = lvwAddress.Items.Add(CStr(m_vAddresses(0, i)).Trim(), ACIADDRESS)

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

                        oListItem = lvwAddress.Items.Add(sAddressUsage, ACIADDRESS)

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

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAddress, bSizeHeaders:=True)

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

        'Const ContactImage As String = "ContactImage"          '' Unused Local Variable
        Dim oListItem As ListViewItem


        Try

            If Not Information.IsArray(m_vContacts) Then
                Exit Sub
            End If

            lvwContact.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vContacts.GetLowerBound(1) To m_vContacts.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwContact.Items.Add(CStr(m_vContacts(1, i)).Trim())

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

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwContact, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ValidateNonMandatoryCombo
    '
    ' Description: This validates non-mandatory combos to make sure that
    ' the entry is either from the list or blank
    '
    ' PW120702 - created
    '
    ' ***************************************************************** '
    Function ValidateNonMandatoryCombo(ByRef cboCombo As ComboBox, ByRef lFieldID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If cboCombo.SelectedIndex = -1 And cboCombo.Text <> "" Then


                MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidComboEntry, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=lFieldID, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), MessageBoxButtons.OK)
                cboCombo.Focus()
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateNonMandatoryComboFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateNonMandatoryCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim result As Integer = 0
        Dim iMainAddresses As Integer
        Dim bDuplicate As Boolean

        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""
        Dim dtReviewDate As Date
        Dim lChar As Integer 'AR20050317 - PN15904
        Dim bValidAccount As Boolean 'AR20050317 - PN15904

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Check Agent Type
            'DC131204


            If uctAgentType.ListIndex = 0 Then
                MessageBox.Show("Please enter a valid Agent Type", "Agent Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                uctAgentType.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'MKR 06/07/2004 Issue No. 12440
            'Check Agent Source

            If cboSource.SelectedIndex = 0 Then
                MessageBox.Show("Please enter a valid Source", "Agent Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                cboSource.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AR20050317 - PN15904 Validate characters in Account Number field
            If Me.txtAccountNumber.Text <> "" Then

                bValidAccount = True

                For lLoop As Integer = 1 To Strings.Len(Me.txtAccountNumber.Text)
                    lChar = Strings.Asc(Mid(Me.txtAccountNumber.Text, lLoop, 1)(0))
                    Select Case lChar
                        Case 97 To 122, 65 To 90, 48 To 57, 32, 47, 92, 46
                            'Valid chars (alpha-numeric or / or \ or .)
                        Case Else
                            bValidAccount = False
                    End Select
                Next lLoop

                If Not bValidAccount Then
                    MessageBox.Show("Please enter a valid Account Number." & Strings.Chr(13) & Strings.Chr(10) & "Account numbers must contain only alpha-numeric or '\', '/' and '.' characters.", "Account Number", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.txtAccountNumber.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If


            'AR20050317 - PN15903 Check date is not in past
            If Me.txtAgencyReviewDate.Text <> "" Then
                dtReviewDate = gPMFunctions.ToSafeDate(Me.txtAgencyReviewDate.Text, CDate("01/01/1900"))
                If dtReviewDate < DateTime.Today Then
                    MessageBox.Show("Please select a Next Review Date that is not earlier than today.", "Next Review Date", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Me.txtAgencyReviewDate.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            If chkBrokerInTransferMode.CheckState = CheckState.Checked Then
                If cboBrokerTransferBusinessType.SelectedIndex = -1 Then
                    MessageBox.Show("A business type must be selected", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    cboBrokerTransferBusinessType.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If cboBrokerTransferBusinessType.Text = "Agent Transfer" Then
                    If txtBrokerTransferToCode.Text.Trim() = "" Then
                        MessageBox.Show("Please select agent to transfer to", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        txtBrokerTransferToCode.Focus()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If uctBranch.SelectedIndex = 0 Then
                MessageBox.Show("Please enter a valid Branch", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'SD 14/08/2002
                SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                uctBranch.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'SD 14/08/2002 Validate mandatory NRMA fields for underwriting
            If m_bIsNRMA Then

                If cboMultipac.SelectedIndex = 0 Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, 5)
                    cboMultipac.Focus()
                    MessageBox.Show("Please enter a valid choice", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If



            'Validate Addresses
            iMainAddresses = 0

            'Count how many addresses are main address
            If lvwAddress.Items.Count > 0 Then
                For i As Integer = 1 To lvwAddress.Items.Count
                    oListItem = lvwAddress.Items.Item(i - 1)

                    'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                    Select Case (m_sDefaultCountryCode.Trim())
                        Case "GBR"
                            sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(i - 1), 1).Text.Trim()
                        Case Else
                            sAddressUsage = lvwAddress.Items.Item(i - 1).Text.Trim()
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
            'If (grdAddress.MaxRows < 2) Then
            '    'less than 2 addresses so cant have duplicates
            '    Exit Function
            'End If

            bDuplicate = False
            'grdAddress.Col = 0

            'Check for duplicates
            'For i = 1 To (grdAddress.MaxRows - 1)

            'grdAddress.Row = i
            'If (Trim$(grdAddress.Text) <> "") Then

            'lAddressCnt = CLng(grdAddress.Text)
            'For j = (i + 1) To grdAddress.MaxRows
            'grdAddress.Row = j
            'If (Trim$(grdAddress.Text) <> "") Then
            'If (CLng(grdAddress.Text) = lAddressCnt) Then
            'bDuplicate = True
            'Exit For
            'End If
            'End If
            'Next j
            'End If
            'Next i

            'If (bDuplicate = True) Then
            'MsgBox "An address can only be used once by a particular party.", vbInformation, "Address Usage Validation"
            'ValidateOK = PMFalse
            'Exit Function
            'End If

            'Agent Filtering
            If uctPickListBranches.SelectedItems = 0 Then
                MessageBox.Show("You must attach at least one Branch to the Agent record", "Agent Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(tabMainTab, 8)
                uctPickListBranches.Focus()
                Return result
            End If

            'Float Balance and Pre-Payment (RC)
            If fraMakeLive.Visible And uctAgentType.ItemCaption <> "Sub-Agent" Then
                'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                If chkMakeLiveInvoice.CheckState = CheckState.Unchecked And chkMakeLiveInstallments.CheckState = CheckState.Unchecked And chkMakeLivePayNow.CheckState = CheckState.Unchecked And chkMakeLiveBankGuarantee.CheckState = CheckState.Unchecked And chkMakeLiveCashDeposit.CheckState = CheckState.Unchecked Then
                    MessageBox.Show("You must choose at least one of the Make Live options", "Agent Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    chkMakeLiveInvoice.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'validate Expected Daily Premium
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtExpectedDailyPremium.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    MessageBox.Show("Invalid Expected Daily Premium " & Strings.Chr(9), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtExpectedDailyPremium.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf CDec(txtExpectedDailyPremium.Text) < 0 Then
                    MessageBox.Show("Expected Daily Premium cannot be Negative ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtExpectedDailyPremium.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    txtExpectedDailyPremium.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(txtExpectedDailyPremium.Text), "Fixed")
                End If

                'validate Days Allowed
                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(txtDaysAllowed.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    MessageBox.Show("Invalid Days Allowed " & Strings.Chr(9), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtDaysAllowed.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf txtDaysAllowed.Text.IndexOf("."c) >= 0 Then
                    MessageBox.Show("Days Allowed cannot have decimal" & Strings.Chr(9), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtDaysAllowed.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf CDec(txtDaysAllowed.Text) < 0 Then
                    MessageBox.Show("Days Allowed cannot be Negative ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtDaysAllowed.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    txtDaysAllowed.Text = CStr(gPMFunctions.ToSafeCurrency(txtDaysAllowed.Text))
                End If

                'validate Overdraft Limit
                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(txtOverdraftLimit.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    MessageBox.Show("Invalid Overdraft Limit " & Strings.Chr(9), Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtOverdraftLimit.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf CDec(txtOverdraftLimit.Text) < 0 Then
                    MessageBox.Show("Overdraft Limit cannot be Negative ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                    txtOverdraftLimit.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                Else
                    txtOverdraftLimit.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(txtOverdraftLimit.Text), "Fixed")
                End If

            End If
            If chkSingleInstalmentPlanOnly.CheckState = CheckState.Checked Then
                If txtCommonRenewalDate.Text = "" Then
                    MessageBox.Show("Please enter Common Renewal Date. ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                    txtCommonRenewalDate.Focus()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' Validate Preferred Correspondence Type
            Dim bValidCorrespondenceType As Boolean = False

            If cboCorrespondenceType.SelectedIndex = -1 AndAlso lvwContact.Items.Count = 0 Then
                bValidCorrespondenceType = True
            ElseIf cboCorrespondenceType.SelectedIndex <> -1 AndAlso lvwContact.Items.Count = 0 _
                AndAlso cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper = "LETTER" Then
                bValidCorrespondenceType = True
            ElseIf cboCorrespondenceType.SelectedIndex = -1 AndAlso lvwContact.Items.Count > 0 Then
                bValidCorrespondenceType = True
            Else
                If cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper <> "LETTER" Then
                    For Each item As ListViewItem In lvwContact.Items
                        If item.SubItems(3).Text.Trim.ToUpper = cboCorrespondenceType.SelectedItem.ToString.Trim.ToUpper Then
                            bValidCorrespondenceType = True
                            Exit For
                        End If
                    Next
                Else
                    bValidCorrespondenceType = True
                End If
            End If

            If Not bValidCorrespondenceType Then
                MessageBox.Show("No corresponding contact type exists. Please add one or change the preferred method of correspondence. ", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 2)
                cboCorrespondenceType.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateOKFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateOK", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.

        Dim lAddressCnt As Integer

        Dim oAddressBusiness As bSIRAddress.Business
        Dim sAddressUsage As String = ""


        Try

            'Find the main address
            For i As Integer = 1 To lvwAddress.Items.Count

                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        sAddressUsage = ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(i - 1), 1).Text.Trim()
                    Case Else
                        sAddressUsage = lvwAddress.Items.Item(i - 1).Text.Trim()
                End Select

                For j As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)
                    'RWH(24/07/2000)
                    If (sAddressUsage = CStr(m_vAddressTypes(1, j))) And (CDbl(m_vAddressTypes(0, j)) = m_iMainAddressIndex) Then
                        lAddressCnt = Convert.ToString(lvwAddress.Items.Item(i - 1).Tag)
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodeProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function BusinessToData() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.
            m_lReturn = m_oBusiness.GetNext(
            vPartyCnt:=m_lPartyCnt,
            vPartyAgentTypeID:=m_lPartyAgentTypeID, vPartyAgentOriginID:=m_iPartyAgentOriginID%,
            vIsBranch:=m_iIsBranch, vIsHeadOffice:=m_iIsHeadOffice,
            vAgencyAgreementDate:=m_dtAgencyAgreement, vAgencyNextReviewDate:=m_dtAgencyNextReview,
            vAgencyAccountNumber:=m_sAgencyAccountNumber, vShortName:=m_sShortName,
            vName:=m_sName, vAgentCnt:=m_lHeadOfficeCnt,
            vBinderIndicator:=m_lBinderIndicator, vReportIndicator:=m_lReportIndicator,
            vStatements:=m_iStatement, vFileCode:=m_sFileCode,
            vCurrencyID:=m_lCurrencyID,
            vPartyCategoryID:=m_lPartyCategoryID, vConsultantCnt:=m_lConsultantCnt,
            vAgentGroupCnt:=m_lAgentGroupCnt, vpaymentmethod:=m_vPaymentMethod,
            vpaymentfrequency:=m_vPaymentFrequency, vaddressonnotice:=m_vAddressOnNotice,
            vtypeofstatement:=m_vTypeOfStatement,
            vsource:=m_vSource, vtitle:=m_sTitle,
            vbranch:=m_vBranch, vSubBranch:=m_vSubBranch,
            vmultipac:=m_iMultipac, vRenewalstopcode:=m_vRenewalStopCode,
            vContactPerson:=m_sContactPerson,
            vFirstName:=m_sFirstName,
            vDateCancelled:=m_vDateCancelled,
            vAgentStatus:=m_vAgentStatus, vRegistrationNumber:=m_vRegistrationNumber, vBrokerAbiId:=m_sBrokerAbiId, vExpenseAccountId:=m_vExpenseAccountId,
            vIsInTransferMode:=m_iIsInTransferMode, vTransferToBusinessTypeID:=m_lTransferToBusinessTypeID, vTransferToPartyCnt:=m_lTransferToPartyCnt, vOverrideCommission:=m_iOverride%, vOverrideCommissionRenewal:=m_iOverrideRenewal, vDomiciledForTax:=m_bDomiciledForTax, vAllowConsolidate:=m_bConsolidate,
            vParamArray:=m_vMakeLiveArray, vIsViewableOnly:=m_bIsViewableOnly, vTradingName:=m_sTradingName, vBankAccount:=m_iBankAccount, vCommissionlevel:=m_lCommissionLevel,
            vCorrespondenceTypeId:=m_vCorrespondenceTypeId, vReceivesClientCorrespondence:=m_bReceivesClientCorrespodence)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get correspondence types
            m_lReturn = m_oBusiness.GetCorrespondenceTypes(vCorrespondenceTypes:=m_vCorrespondenceTypes)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' returns additional party details
            ' these detail are not returned from get next because of parameter limit of 60
            m_lReturn = GetPartyDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'AR20050317 - PN18400
            m_sShortName_Original = m_sShortName.Trim()

            'PN 21971
            m_vRegistrationNumber_Original = m_vRegistrationNumber.Trim()

            'Get additional details required for display that not stored on this
            'record
            ' PW100702 - add associates/consultant/agent group

            m_lReturn = m_oBusiness.GetOtherDetails(vAgentCnt:=m_lHeadOfficeCnt, vAgentref:=m_sHeadOfficeRef, vConsultantCnt:=m_lConsultantCnt, vConsultantRef:=m_sConsultantRef, vConsultantName:=m_sConsultantName, vAgentGroupCnt:=m_lAgentGroupCnt, vAgentGroupRef:=m_sAgentGroupRef, vAgentGroupName:=m_sAgentGroupName, vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates, vDocs:=m_vDocsSuppressed)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the agent details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            ' PW100702

            PopulateAssociates()
            PopulateSuppressedDocs()

            ' PW100702 *ZAZ*
            'Get relationship type lookups

            m_lReturn = m_oBusiness.GetRelationshipTypeLookups(vRelationships:=m_vRelationships)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the relationship ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            cmdMaintainAssociates.Enabled = Not (False)



            'Get addresses for the party

            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get addresse type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

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

            'AR - NEXUS MTA
            If m_bIsViewableOnly Then
                m_iTask = gPMConstants.PMEComponentAction.PMView
            End If

            ' Party View
            ' To full disable form
            If m_bIsViewOnlyAgent Then
                m_bIsViewableOnly = True
            End If
            m_lReturn = m_oBusiness.GetCertificateYearDetails(vCertYear:=m_vCertificateYearDetails, vPartyCnt:=m_lPartyCnt)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the CertificateYears details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            m_lReturn = m_oBusiness.GetCommissionLevel(vPartyCnt:=m_lPartyCnt, vCommissionLevel:=m_vCommissionLevel)

            PopulateCommissionLevel()

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
        Dim iAddressCount As Integer
        Dim vCommonRenewalDate As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.

            m_sShortName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtIDReference))

            m_lReturn = ValidateNonMandatoryCombo(cboPaymentFrequency, ACPaymentFrequency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ValidateNonMandatoryCombo(cboAddressOnNotice, ACAddressOnNotice)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ValidateNonMandatoryCombo(cboRenewalStopCode, ACRenewalStopCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW180702 - validate Bank Account
            If cboPaymentMethod.ItemCaption.ToLower().Trim() = ACPaymentMethodEFT And cboBankAccount.Text.Trim() = "(Any)" Then
                MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAccountRequired, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), MessageBoxButtons.OK)
                result = gPMConstants.PMEReturnCode.PMFalse
                cboBankAccount.Focus()
                Return result
            End If

            ' PW180702 - validate Payment Method
            If uctAgentType.ItemCaption.ToLower().Trim() = "broker" And cboPaymentMethod.ItemCaption.Trim().ToUpper() <> ACPaymentMethodNA Then
                MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInvalidPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), MessageBoxButtons.OK)
                result = gPMConstants.PMEReturnCode.PMFalse
                cboPaymentMethod.Focus()
                SSTabHelper.SetSelectedIndex(tabMainTab, 3)
                Return result
            End If

            ' PW180702 - validate Address On Notice
            iAddressCount = 0
            For i As Integer = 1 To lvwAddress.Items.Count
                'PWF 05/12/2002 - International addresses will have address type in a different column
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        If ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(i - 1), 1).Text.Trim().ToLower() = cboAddressOnNotice.Text.Trim().ToLower() Then
                            iAddressCount += 1
                        End If
                    Case Else
                        If lvwAddress.Items.Item(i - 1).Text.Trim().ToLower() = cboAddressOnNotice.Text.Trim().ToLower() Then
                            iAddressCount += 1
                        End If
                End Select
            Next

            If iAddressCount <> 1 Then
                MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressOnNoticeError, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressOnNotice, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), MessageBoxButtons.OK)
                result = gPMConstants.PMEReturnCode.PMFalse
                cboAddressOnNotice.Focus()
                Return result
            End If

            ' PW180702 - validate Title
            If ddTitle.Text.Trim() = "" And txtFirstName.Text.Trim() <> "" Then
                MessageBox.Show(CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTitleRequired, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager)), MessageBoxButtons.OK)
                result = gPMConstants.PMEReturnCode.PMFalse
                ddTitle.Focus()
                Return result
            End If



            ' If this is an add then check for duplicate references
            'PN18400 check for duplicate references for Edit case also
            'AR20050317 - PN18400 Only check in Edit case if the Short name has changed
            If Task = gPMConstants.PMEComponentAction.PMAdd Or (Task = gPMConstants.PMEComponentAction.PMEdit And m_sShortName <> m_sShortName_Original) Then
                If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                    m_lReturn = m_oBusiness.CheckReference(m_sShortName)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'If the returned reference is an empty string, then the reference exists
                    If m_sShortName = "" Then

                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRefExists, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMsg, "Agent Code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        If txtIDReference.Enabled Then
                            Me.txtIDReference.Focus()
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                End If
            End If

            m_iPartyAgentOriginID = VB6.GetItemData(cboSource, cboSource.SelectedIndex)

            m_lPartyAgentTypeID = uctAgentType.ItemId

            m_sName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))
            m_iStatement = chkStatement.CheckState
            m_iOverride = chkOverrideCommission.CheckState

            m_iOverrideRenewal = chkOverrideCommissionRen.CheckState

            m_dtAgencyNextReview = CDate(m_oFormFields.UnformatControl(ctlControl:=txtAgencyReviewDate))

            m_sAgencyAccountNumber = CStr(m_oFormFields.UnformatControl(ctlControl:=txtAccountNumber))

            m_sTradingName = CStr(m_oFormFields.UnformatControl(ctlControl:=txtName))
            m_lBinderIndicator = cboBinderIndicator.SelectedIndex
            m_lReportIndicator = cboReportIndicator.SelectedIndex
            m_lCurrencyID = uctCurrency.CurrencyId

            m_sFileCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtFileCode))
            If cboTermsOfPayment.SelectedIndex > -1 Then
                m_iTermsOfPaymentID = VB6.GetItemData(cboTermsOfPayment, cboTermsOfPayment.SelectedIndex)
            Else
                m_iTermsOfPaymentID = 0
            End If
            m_lPartyCategoryID = uctCategory.ItemId

            If m_bIsUnderwritingBranch Then
                m_sBrokerAbiId = CStr(m_oFormFields.UnformatControl(ctlControl:=txtBrokerAbiId))
            End If

            m_iIsInTransferMode = chkBrokerInTransferMode.CheckState

            If cboBrokerTransferBusinessType.SelectedIndex <> -1 Then
                m_lTransferToBusinessTypeID = VB6.GetItemData(cboBrokerTransferBusinessType, cboBrokerTransferBusinessType.SelectedIndex)
                m_lTransferToPartyCnt = gPMFunctions.ToSafeLong(Convert.ToString(txtBrokerTransferToCode.Tag).Trim(), 0)
            Else
                m_lTransferToBusinessTypeID = 0
                m_lTransferToPartyCnt = 0
            End If

            If cboPaymentMethod.ListIndex <> -1 And cboPaymentMethod.ItemCaption <> ACPaymentMethodNA Then
                m_vPaymentMethod = cboPaymentMethod.ItemData(cboPaymentMethod.ListIndex)
            Else
                m_vPaymentMethod = Nothing
            End If

            If cboPaymentFrequency.SelectedIndex <> -1 Then
                m_vPaymentFrequency = VB6.GetItemData(cboPaymentFrequency, cboPaymentFrequency.SelectedIndex)
            Else

                m_vPaymentFrequency = Nothing
            End If

            If cboAddressOnNotice.SelectedIndex <> -1 Then
                m_vAddressOnNotice = VB6.GetItemData(cboAddressOnNotice, cboAddressOnNotice.SelectedIndex)
            Else

                m_vAddressOnNotice = Nothing
            End If

            If cboSubBranch.SelectedIndex <> -1 Then
                m_vSubBranch = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
            Else

                m_vSubBranch = Nothing
            End If

            If cboRenewalStopCode.SelectedIndex <> -1 Then
                m_vRenewalStopCode = VB6.GetItemData(cboRenewalStopCode, cboRenewalStopCode.SelectedIndex)
            Else

                m_vRenewalStopCode = Nothing
            End If

            'RDT PN 18099
            m_vBranch = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)

            m_sTitle = ddTitle.Text

            m_iMultipac = cboMultipac.SelectedIndex

            m_vTypeOfStatement = DBNull.Value


            m_vSource = DBNull.Value
            m_sContactPerson = txtContactPerson.Text
            m_sFirstName = txtFirstName.Text
            m_iBankAccount = ToSafeInteger(cboBankAccount.ItemData(cboBankAccount.ListIndex))
            m_bConsolidate = chkConsolidatedCommission.CheckState


            m_vDateCancelled = m_oFormFields.UnformatControl(ctlControl:=txtDateCancelled)

            ' PW150702 - put the suppressed docs into an array
            If lstDocsChosen.Items.Count > 0 Then
                m_vDocsSuppressed = Array.CreateInstance(GetType(Object), New Integer() {lstDocsChosen.Items.Count}, New Integer() {0})
                For i As Integer = 0 To lstDocsChosen.Items.Count - 1
                    m_vDocsSuppressed(i) = VB6.GetItemData(lstDocsChosen, i)
                Next
            Else
                m_vDocsSuppressed = Array.CreateInstance(GetType(Object), New Integer() {1}, New Integer() {0})
            End If

            'Validation of following not required if we are cancelling out

            If Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' PW100702 - Get party count for Consultant (if valid ref supplied)
                If (txtConsultantRef.Text.Trim() <> "") And (m_bVerifyConsultantCnt) Then


                    m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtConsultantRef.Text.Trim(), vPartyCnt:=m_lConsultantCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lConsultantCnt = 0 Then


                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConsultantMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

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

                ' PW100702 - Get party count for Agent Group (if valid ref supplied)
                If (txtAgentGroupRef.Text.Trim() <> "") And (m_bVerifyAgentGroupCnt) Then


                    m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtAgentGroupRef.Text.Trim(), vPartyCnt:=m_lAgentGroupCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lAgentGroupCnt = 0 Then


                        sMsg = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentGroupMissing, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        MessageBox.Show(sMsg, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                        iPMFunc.SelectText(txtAgentGroupRef)
                        txtAgentGroupRef.Focus()

                        Return gPMConstants.PMEReturnCode.PMFalse

                    End If

                Else

                    If txtAgentGroupRef.Text.Trim() = "" Then
                        m_lAgentGroupCnt = 0
                    Else
                        m_lAgentGroupCnt = CInt(Convert.ToString(txtAgentGroupRef.Tag))
                    End If

                End If

            End If
            'DC220803 -PS253 -fsa compliance


            m_vAgentStatus = DBNull.Value
            'DC021203 -PN -fsa compliance -registration number

            m_vRegistrationNumber = Nothing



            '***************************
            ' get the party tax details from the party tax control
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage
            '***************************

            If cboCorrespondenceType.SelectedIndex <> -1 Then
                m_vCorrespondenceTypeId = VB6.GetItemData(cboCorrespondenceType, cboCorrespondenceType.SelectedIndex)
            Else
                m_vCorrespondenceTypeId = Nothing
            End If

            m_bReceivesClientCorrespodence = gPMFunctions.ToSafeBoolean(chkReceivesClientCorr.CheckState)

            'Initialise Array
            ReDim m_vMakeLiveArray(21) 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling - 18 to 19

            m_vMakeLiveArray(AC_PARTYAG_MakeLiveInvoice) = gPMFunctions.ToSafeBoolean(chkMakeLiveInvoice.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_MakeLiveInstallments) = gPMFunctions.ToSafeBoolean(chkMakeLiveInstallments.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_MakeLivePayNow) = gPMFunctions.ToSafeBoolean(chkMakeLivePayNow.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_IsStandardAccount) = gPMFunctions.ToSafeBoolean(chkStandardAccount.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_IsFloatBalanceAccount) = gPMFunctions.ToSafeBoolean(chkFloatBalanceAccount.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_IsPrepaymentAccount) = gPMFunctions.ToSafeBoolean(chkPrepaymentAccount.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_IsOverdraftAccount) = gPMFunctions.ToSafeBoolean(chkOverdraftAccount.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_FloatBalanceLimit) = gPMFunctions.ToSafeCurrency(txtFloatBalanceLimit.Text)
            m_vMakeLiveArray(AC_PARTYAG_ExpectedDailyPremium) = gPMFunctions.ToSafeCurrency(txtExpectedDailyPremium.Text)
            m_vMakeLiveArray(AC_PARTYAG_OverdraftLimit) = gPMFunctions.ToSafeCurrency(txtOverdraftLimit.Text)
            m_vMakeLiveArray(AC_PARTYAG_DaysAllowed) = gPMFunctions.ToSafeInteger(txtDaysAllowed.Text)
            m_vMakeLiveArray(AC_PARTYAG_MakeLiveBankGuarantee) = gPMFunctions.ToSafeBoolean(chkMakeLiveBankGuarantee.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_MakeLiveCashDeposit) = gPMFunctions.ToSafeBoolean(chkMakeLiveCashDeposit.CheckState) 'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
            If Information.IsDate(txtOverdraftExpiry.Text) Then
                m_vMakeLiveArray(AC_PARTYAG_OverdraftExpiry) = gPMFunctions.ToSafeDate(txtOverdraftExpiry.Text)
            Else

                m_vMakeLiveArray(AC_PARTYAG_OverdraftExpiry) = DBNull.Value
            End If


            m_vMakeLiveArray(AC_PARTYAG_AltRefMandatory) = gPMFunctions.ToSafeInteger(chkAltRefMandatory.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_AltRefRequiredForEachTrans) = gPMFunctions.ToSafeInteger(chkAltRefRequiredForEachTrans.CheckState)


            If (cboCommissionPostingType.ListIndex > -1) Then
                m_vMakeLiveArray(AC_PARTYAG_CommissionPostingType) = gPMFunctions.ToSafeLong(cboCommissionPostingType.ItemData(cboCommissionPostingType.ListIndex))
            Else
                m_vMakeLiveArray(AC_PARTYAG_CommissionPostingType) = DBNull.Value
            End If
            m_vMakeLiveArray(AC_PARTYAG_IsSingleInstalmentPlanOnly) = gPMFunctions.ToSafeBoolean(chkSingleInstalmentPlanOnly.CheckState)

            'text box cannot be converted to string
            vCommonRenewalDate = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtCommonRenewalDate.Text)
            If Information.IsDate(vCommonRenewalDate) Then
                m_vMakeLiveArray(AC_PARTYAG_CommonRenewalDate) = gPMFunctions.ToSafeDate(vCommonRenewalDate)
            Else

                m_vMakeLiveArray(AC_PARTYAG_CommonRenewalDate) = DBNull.Value
            End If

            'Batch Renewal
            m_vMakeLiveArray(AC_PARTYAG_IsProduceAgentRenewalList) = gPMFunctions.ToSafeBoolean(chkProductAgentRenewalList.CheckState)
            m_vMakeLiveArray(AC_PARTYAG_MakeLiveIsGrossAgent) = ToSafeBoolean(OptGrossOfCommission.Checked)


            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
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

            iPMFunc.CenterForm(Me)

            SSTabHelper.SetSelectedIndex(tabMainTab, 0)

            'Get Country Code for Postcode checking.(Process 004)
            m_lReturn = m_oBusiness.GetDefaultCountryCode(v_iCountryID:=m_iDefaultCountryID, r_sCountryCode:=m_sDefaultCountryCode)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get default country code", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                Return result

            End If

            'Get correspondence types
            m_lReturn = m_oBusiness.GetCorrespondenceTypes(vCorrespondenceTypes:=m_vCorrespondenceTypes)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the correspondence types from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If
            PopulateCorrespondenceTypes()

            If cboCorrespondenceType.SelectedIndex = -1 AndAlso m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cboCorrespondenceType.Text = "LETTER"
            End If

            Dim cboBrokerTransferBusinessType_NewIndex As Integer = -1
            cboBrokerTransferBusinessType_NewIndex = cboBrokerTransferBusinessType.Items.Add("Direct")
            VB6.SetItemData(cboBrokerTransferBusinessType, cboBrokerTransferBusinessType_NewIndex, 1)
            cboBrokerTransferBusinessType_NewIndex = cboBrokerTransferBusinessType.Items.Add("Agent Transfer")
            VB6.SetItemData(cboBrokerTransferBusinessType, cboBrokerTransferBusinessType_NewIndex, 7)
            cboBrokerTransferBusinessType.SelectedIndex = -1

            ' don't display party agent type reinsurer in the agent type selection
            uctAgentType.WhereClause = "code <> 'Reins'"
            uctAgentType.RefreshList()
            chkSingleInstalmentPlanOnly.Visible = False
            txtCommonRenewalDate.Visible = False
            lblCommonRenewalDate.Visible = False

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_bDomiciledForTax = True
            End If

            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(m_vAgentTypes) Then

                cmbAgentType.Items.Clear()
                Dim cmbAgentType_NewIndex As Integer = -1
                cmbAgentType_NewIndex = 0
                cmbAgentType.Items.Insert(cmbAgentType_NewIndex, "")

                For iCount As Integer = m_vAgentTypes.GetLowerBound(1) To m_vAgentTypes.GetUpperBound(1)
                    'PN 18683 : Used ItemData property instead of ListIndex for cmbAgentType
                    If CStr(m_vAgentTypes(0, iCount)) = "AGENT" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(gSIRLibrary.SIRPartyTypeAgentText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                    If CStr(m_vAgentTypes(0, iCount)) = "SUB AGENT" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(PMBConst.PMBAgentTypeSubAgentText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                    If CStr(m_vAgentTypes(0, iCount)) = "INTRODUCER" Then
                        cmbAgentType_NewIndex = cmbAgentType.Items.Add(PMBConst.PMBAgentTypeIntroducerText)
                        VB6.SetItemData(cmbAgentType, cmbAgentType_NewIndex, CInt(m_vAgentTypes(1, iCount)))
                    End If
                Next iCount
            End If

            With cboBinderIndicator
                .Items.Clear()
                .Items.Add("All Outstanding")
                .Items.Add("Paid By Client")
            End With

            With cboReportIndicator
                .Items.Clear()
                .Items.Add("Payment Date")
                .Items.Add("Policy Number")
                .Items.Add("Client Code")
                .Items.Add("Renewal Date")
                .Items.Add("Risk Code")
            End With

            m_lReturn = PopulateYesNoCombo(cboMultipac)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = SetFirstLastControls()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get available documents to suppress
            m_lReturn = m_oBusiness.GetAvailableDocs(vDocs:=m_vDocsAvailable)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the document details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If

            ' Set any other default values to the interface.

            ' {* USER DEFINED CODE (Begin) *}

            ' Disable menu options if New Client
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                cmdRates.Enabled = False
                mnuFinancial.Enabled = False
                mnuCommission.Enabled = False
                mnuNotes.Enabled = False
                mnuLetter.Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonFinancial - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonCommission - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonNotes - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonLetter - 1).Enabled = False
                Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonCD - 1).Enabled = False ''68733
            End If

            cmbAgentType.Visible = False
            mnuCommission.Available = False
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonCommission - 1).Visible = False
            cmdRates.Visible = False
            SSTabHelper.SetTabVisible(tabMainTab, 6, False)

            ' PW150702 - hide NRMA only fields, if appropriate
            If Not m_bIsNRMA Then
                lblMultipac.Visible = False
                cboMultipac.Visible = False
            End If

            If m_bIsUnderwritingBranch Then
                lblBrokerAbiId.Visible = True
                txtBrokerAbiId.Visible = True
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                uctAgentType.Enabled = False
                cmbAgentType.Enabled = False
                fraPremiumSettlement.Enabled = False
                uctCurrency.Enabled = False
                uctBranch.Enabled = False
            End If

            'fsa compliance
            If m_bEnableFSACompliance Then

                'Get risk groups for agent
                m_lReturn = m_oBusiness.GetRiskGroups(vRiskGroups:=m_vRiskGroups)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the risk group details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
                End If

                DisplayRiskGroups()

            End If

            With tabMainTab
                'Set Visibility
                SSTabHelper.SetTabVisible(tabMainTab, MainModule.AC_TAB_PAG_PAYMENTS, True)
            End With

            fraMakeLive.Visible = True
            fraAccountLimits.Visible = True

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                chkMakeLiveInvoice.CheckState = CheckState.Checked
                chkMakeLiveInstallments.CheckState = CheckState.Checked
                chkMakeLivePayNow.CheckState = CheckState.Checked
                chkStandardAccount.CheckState = CheckState.Checked
                chkMakeLiveBankGuarantee.CheckState = CheckState.Checked
                chkMakeLiveCashDeposit.CheckState = CheckState.Checked
            End If

            txtFloatBalanceLimit.Enabled = False
            txtExpectedDailyPremium.Enabled = False
            txtDaysAllowed.Enabled = False
            txtOverdraftLimit.Enabled = False
            txtOverdraftExpiry.Enabled = False
            txtFloatBalanceLimit.Text = "0.00"
            txtExpectedDailyPremium.Text = "0.00"
            txtOverdraftLimit.Text = "0.00"
            txtDaysAllowed.Text = "0"
            txtFloatBalanceLimit.BackColor = Color.Silver
            txtExpectedDailyPremium.BackColor = Color.Silver
            txtDaysAllowed.BackColor = Color.Silver
            txtOverdraftLimit.BackColor = Color.Silver
            txtOverdraftExpiry.BackColor = Color.Silver

            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=g_iSourceID, r_vUnderwriting:=m_vPayNowOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                gPMFunctions.RaiseError("SetInterfaceDefaults", "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            If Conversion.Val(m_vPayNowOptionValue) = 0 Then
                fraAccountLimits.Visible = False
            End If

            If uctAgentType.ItemCaption = "Sub-Agent" AndAlso bVisibleCertificateYearTab Then
                SSTabHelper.SetTabVisible(tabMainTab, MainModule.AC_TAB_PAG_CERTIFICATEYEARS, True)
            Else
                SSTabHelper.SetTabVisible(tabMainTab, MainModule.AC_TAB_PAG_CERTIFICATEYEARS, False)
            End If

            '(RC) QBENZ014
            chkAltRefMandatory.CheckState = CheckState.Unchecked
            chkAltRefRequiredForEachTrans.CheckState = CheckState.Unchecked

            'Party Bank Details
            m_lReturn = uctPartyBankControl1.SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If

            ' {* USER DEFINED CODE (End) *}

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


            ReDim m_ctlTabFirstLast(1, 4)

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
            m_ctlTabFirstLast(ACControlEnd, 0) = cboTermsOfPayment

            m_ctlTabFirstLast(ACControlStart, 1) = lvwAddress
            m_ctlTabFirstLast(ACControlEnd, 1) = cmdEditAd

            m_ctlTabFirstLast(ACControlStart, 2) = lvwContact
            m_ctlTabFirstLast(ACControlEnd, 2) = cmdEditCon

            ' PW160702


            m_ctlTabFirstLast(ACControlStart, 3) = cboPaymentMethod
            m_ctlTabFirstLast(ACControlEnd, 3) = lstDocsChosen

            m_ctlTabFirstLast(ACControlStart, 4) = txtConsultantRef
            m_ctlTabFirstLast(ACControlEnd, 4) = cboRenewalStopCode

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

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAddAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteAd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAddCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteCon.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonFinancial - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFinancial, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonNotes - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNotes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonLetter - 1).ToolTipText = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACLetter, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblIDReference.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACReference, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblIDReference.Text = "Agent code:"
            lblName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAgencyNextReview.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgencyNextReview, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblSource.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSource, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            fraAppointment.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACfraAppointment, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sAddressUsage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListUsage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sAddressLine1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sAddressLine2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sAddressLine3 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sAddressLine4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            sPostCode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            '
            ' PW160702 - additional fields added
            '

            lblPaymentMethod.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentMethod, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPaymentFrequency.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPaymentFrequency, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAddressOnNotice.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressOnNotice, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblTitle.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblSubBranch.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSubBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblMultipac.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultipac, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblRenewalStopCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRenewalStopCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblBankAccount.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBankAccount, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDateCancelled.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateCancelled, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblContactPerson.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACContactPerson, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblFirstName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACFirstName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
            With lvwAddress
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        .Columns.Item(0).Text = sPostCode
                        .Columns.Item(1).Text = sAddressUsage
                        .Columns.Item(2).Text = sAddressLine1
                        .Columns.Item(3).Text = sAddressLine2
                        .Columns.Item(4).Text = sAddressLine3
                        .Columns.Item(5).Text = sAddressLine4

                    Case Else
                        .Columns.Item(0).Text = sAddressUsage
                        .Columns.Item(1).Text = sAddressLine1
                        .Columns.Item(2).Text = sAddressLine2
                        .Columns.Item(3).Text = sAddressLine3
                        .Columns.Item(4).Text = sAddressLine4
                        .Columns.Item(5).Text = sPostCode

                End Select
            End With

            lvwContact.Columns.Item(0).Text = "Area Code"
            lvwContact.Columns.Item(1).Text = "Number"
            lvwContact.Columns.Item(2).Text = "Extension"
            lvwContact.Columns.Item(3).Text = "Type"
            lvwContact.Columns.Item(4).Text = "Description"

            ' Set full row select on the address control
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwAddress.Handle.ToInt32(), v_vShowRowSelect:=True)

            ' Set full row select on the contact control
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwContact.Handle.ToInt32(), v_vShowRowSelect:=True)

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            'MKR 06/07/2004 Issue No. 12440
            'If the control name is cboSource then add none as the first option...
            If ctlLookup.Name = "cboSource" Then

                'type cast the control to combobox
                'ctlLookup.AddItem("(None)")
                'CType(ctlLookup, ComboBox).Items.Add(New Object() {"(None)"})
                ctlLookup.Items.Add(New VB6.ListBoxItem("(None)", 0))
            End If

            'GK 19/08/10 PN 74466
            'If the control name is cboSource then add none as the first option...
            If ctlLookup.Name = "cboCommissionLevel" Then
                ctlLookup.Items.Add(New VB6.ListBoxItem("(None)", 0))
            End If

            'GK 19/08/10 PN 74466
            'If the control name is cboRenewalStopCode then add none as the first option...
            If ctlLookup.Name = "cboRenewalStopCode" Then
                ctlLookup.Items.Add(New VB6.ListBoxItem("", 0))
            End If



            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            Dim newIndex As Integer = -1
            For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)


                newIndex = ctlLookup.Items.Add(New VB6.ListBoxItem(m_vLookupDetails(ACDetailDesc, lCntr), CInt(m_vLookupDetails(ACDetailKey, lCntr))))

                ' Check if this is the selected index.
                If CStr(m_vLookupValues(ACValueID, lRow)) <> "" Then
                    If CDbl(m_vLookupValues(ACValueID, lRow)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
                        ctlLookup.SelectedIndex = newIndex
                    End If
                End If

            Next lCntr

            ' Check if the selected index is blank. If so,
            ' we set the controls index to zero.
            ' PW120702 set to -1 to blank current entry
            '  however leave cboSource as it was as we don't want to
            '  change the current functionality
            'If (CStr(m_vLookupValues(ACValueID, lRow)) = "") Or (CDbl(m_vLookupValues(ACValueID, lRow)) = -1) Then
            If (CStr(m_vLookupValues(ACValueID, lRow)) = "" Or CStr(m_vLookupValues(ACValueID, lRow)) = "0") OrElse (CDbl(m_vLookupValues(ACValueID, lRow)) = -1) Then
                If ctlLookup.Name = "cboSource" Or ctlLookup.Name = "cboPaymentMethod" Or ctlLookup.Name = "cboCommissionLevel" Or ctlLookup.Name = "cboRenewalStopCode" Then

                    'NIIT - type cast the control to combobox
                    'ctlLookup.ListIndex = 0
                    ctlLookup.SelectedIndex = 0
                Else

                    'NIIT - type cast the control to combobox
                    'ctlLookup.ListIndex = -1
                    ctlLookup.SelectedIndex = -1
                End If
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

    Private Sub cboAddressOnNotice_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAddressOnNotice.Enter

        ' Alix - 31/10/2003 - PN7862 - Check if tab is visible first
        If SSTabHelper.GetTabVisible(tabMainTab, 4) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 4)
        End If

    End Sub

    Private Sub cboBankAccount_Click()
        If Trim(cboBankAccount.Text) <> "(Any)" Then

            m_iBankAccount = VB6.GetItemData(cboBankAccount, cboBankAccount.ListIndex)
        End If
    End Sub

    Private Sub cboBankAccount_GotFocus()
        If SSTabHelper.GetTabVisible(tabMainTab, 3) = True Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 3)
        End If
    End Sub

    Private Sub cboBrokerTransferBusinessType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboBrokerTransferBusinessType.SelectedIndexChanged
        cmdBrokerTransferTo.Enabled = (cboBrokerTransferBusinessType.Text = "Agent Transfer")
        txtBrokerTransferToCode.Enabled = (cboBrokerTransferBusinessType.Text = "Agent Transfer")
        If cboBrokerTransferBusinessType.Text = "Direct" Then
            txtBrokerTransferToCode.Text = ""
            txtBrokerTransferToCode.Tag = "0"
            cmdBrokerTransferTo.Font = VB6.FontChangeBold(cmdBrokerTransferTo.Font, False)
        Else
            cmdBrokerTransferTo.Font = VB6.FontChangeBold(cmdBrokerTransferTo.Font, True)
        End If
    End Sub

    Private Sub cboPaymentFrequency_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentFrequency.Enter

        If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 3)
        End If

    End Sub


    Private Sub cboPaymentMethod_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPaymentMethod.GotFocus

        If SSTabHelper.GetTabVisible(tabMainTab, 3) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 3)
        End If

    End Sub


    Private Sub cboRenewalStopCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRenewalStopCode.Enter

        If SSTabHelper.GetTabVisible(tabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
        End If

    End Sub

    'DC220803 -PS253 -fsa compliance
    Private Sub cboAgentStatus_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAgentStatus.Enter

        If SSTabHelper.GetTabVisible(tabMainTab, 6) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 6)
        End If

    End Sub

    'DC220803 -PS253 -fsa compliance
    ' ***************************************************************** '
    ' Name: AddAllGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddAllGroups() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwUnderTraining.Items.Count
                m_lReturn = AddGroup(v_lRiskGroupid:=Convert.ToString(lvwUnderTraining.Items.Item(iRow - 1).Tag))
            Next iRow

            m_lReturn = DisplayRiskGroups()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAllGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddGroups() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwUnderTraining.Items.Count
                If lvwUnderTraining.Items.Item(iRow - 1).Selected Then
                    m_lReturn = AddGroup(v_lRiskGroupid:=Convert.ToString(lvwUnderTraining.Items.Item(iRow - 1).Tag))
                End If

            Next iRow

            m_lReturn = DisplayRiskGroups()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddGroup
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddGroup(ByRef v_lRiskGroupid As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 0 To m_vRiskGroups.GetUpperBound(1)


                If CInt(v_lRiskGroupid) = CInt(m_vRiskGroups(0, iRow)) Then
                    If CStr(m_vRiskGroups(2, iRow)) = "0" Then
                        m_vRiskGroups(3, iRow) = "NEW"
                    Else
                        m_vRiskGroups(3, iRow) = ""
                    End If
                End If

            Next iRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RemoveAllGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RemoveAllGroups() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwCompetent.Items.Count
                m_lReturn = RemoveGroup(v_lRiskGroupid:=Convert.ToString(lvwCompetent.Items.Item(iRow - 1).Tag))
            Next iRow

            m_lReturn = DisplayRiskGroups()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveAllGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveAllGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RemoveGroups
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RemoveGroups() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 1 To lvwCompetent.Items.Count
                If lvwCompetent.Items.Item(iRow - 1).Selected Then
                    m_lReturn = RemoveGroup(v_lRiskGroupid:=Convert.ToString(lvwCompetent.Items.Item(iRow - 1).Tag))
                End If

            Next iRow

            m_lReturn = DisplayRiskGroups()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: RemoveGroup
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RemoveGroup(ByRef v_lRiskGroupid As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iRow As Integer = 0 To m_vRiskGroups.GetUpperBound(1)


                If CInt(v_lRiskGroupid) = CInt(m_vRiskGroups(0, iRow)) Then
                    If CStr(m_vRiskGroups(2, iRow)) <> "0" Then
                        m_vRiskGroups(3, iRow) = "DEL"
                    Else
                        m_vRiskGroups(3, iRow) = ""
                    End If
                End If

            Next iRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DisplayRiskGroups
    '
    ' Description: Displays Risk Groups
    '
    ' ***************************************************************** '
    Public Function DisplayRiskGroups() As Integer

        Dim result As Integer = 0
        Dim oListUnderTraining, oListCompetent, oList As ListView
        Dim oListItem As ListViewItem
        Dim lItems As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwUnderTraining)
            oListUnderTraining = lvwUnderTraining
            m_lReturn = ListViewFunc.ListViewBatchStart(lvwList:=lvwCompetent)
            oListCompetent = lvwCompetent

            oListUnderTraining.Items.Clear()
            oListCompetent.Items.Clear()

            lItems = m_vRiskGroups.GetUpperBound(1)

            ' Assign the details to the interface.
            For lRow As Integer = m_vRiskGroups.GetLowerBound(1) To lItems

                ' {* USER DEFINED CODE (Begin) *}
                If (CStr(m_vRiskGroups(2, lRow)) = "0" And CStr(m_vRiskGroups(3, lRow)) = "") Or (CStr(m_vRiskGroups(2, lRow)) <> "0" And CStr(m_vRiskGroups(3, lRow)) = "DEL") Then
                    oList = oListUnderTraining
                    oListItem = oList.Items.Add(CStr(m_vRiskGroups(1, lRow)).Trim())
                Else
                    oList = oListCompetent
                    oListItem = oList.Items.Add(CStr(m_vRiskGroups(1, lRow)).Trim())

                End If
                With oListItem

                    .Tag = CStr(m_vRiskGroups(0, lRow))
                End With

            Next lRow
            If lvwUnderTraining.Items.Count > 0 Or lvwCompetent.Items.Count > 0 Then
                m_lReturn = ListViewFunc.ListViewBatchEnd()
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the risk groups", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub chkBrokerInTransferMode_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkBrokerInTransferMode.CheckStateChanged

        If chkBrokerInTransferMode.CheckState <> CheckState.Checked Then
            txtBrokerTransferToCode.Text = ""
            txtBrokerTransferToCode.Tag = "0"
            cboBrokerTransferBusinessType.SelectedIndex = -1
            lblBrokerTransferBusinessType.Font = VB6.FontChangeBold(lblBrokerTransferBusinessType.Font, False)
        Else
            lblBrokerTransferBusinessType.Font = VB6.FontChangeBold(lblBrokerTransferBusinessType.Font, True)
        End If

        cboBrokerTransferBusinessType.Enabled = (chkBrokerInTransferMode.CheckState = CheckState.Checked)

    End Sub

    Private Sub chkSingleInstalmentPlanOnly_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSingleInstalmentPlanOnly.CheckStateChanged
        If chkSingleInstalmentPlanOnly.CheckState = CheckState.Checked Then
            lblCommonRenewalDate.Font = VB6.FontChangeBold(lblCommonRenewalDate.Font, True)
        Else
            lblCommonRenewalDate.Font = VB6.FontChangeBold(lblCommonRenewalDate.Font, False)
            txtCommonRenewalDate.Text = ""
        End If
    End Sub
    ''' <summary>
    ''' cmbAgentType_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmbAgentType_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbAgentType.Click
        If cmbAgentType.Text = PMBAgentTypeIntroducerText Then

            lblExpenseAcc.Visible = True
            actExpenseAcc.Visible = True

            m_oFormFields.Item("actExpenseAcc-0").IsMandatory = True

        Else

            lblExpenseAcc.Visible = False
            actExpenseAcc.Visible = False
            actExpenseAcc.AccountId = 0

            m_oFormFields.Item("actExpenseAcc-0").IsMandatory = False

        End If

        '(RC) PLICO 9-10
        SetCommisionRelease()

    End Sub

    'DC141204
    Private Sub cmbAgentType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmbAgentType.SelectedIndexChanged


        lblExpenseAcc.Visible = False
        actExpenseAcc.Visible = False
        actExpenseAcc.AccountId = 0

        m_oFormFields.Item("actExpenseAcc-0").IsMandatory = False

        chkSingleInstalmentPlanOnly.Visible = True
        '(RC) PLICO 9-10
        SetCommisionRelease()

    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = AddGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to select Risk Groups", "Error")
        End If

    End Sub

    Private Sub cmdAddAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAll.Click

        m_lReturn = AddAllGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to select Risk Groups", "Error")
        End If

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        Try


            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = ValidateNumberingScheme()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
            End If

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Validate some address stuff
            m_lReturn = ValidateOK()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            Else
                cmdApply.Visible = False
            End If




        Catch ex As Exception
            'Error Log

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            Exit Sub

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdBrokerTransferTo_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrokerTransferTo.Click

        Dim lPartyCnt As Integer
        Dim sShortName As String = ""

        If SelectParty(vPartyCnt:=lPartyCnt, vShortName:=sShortName, vSpecialParty:="AG", bSuppressSubAgents:=True, bSuppressCancelledAgents:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
            Exit Sub
        End If

        txtBrokerTransferToCode.Text = sShortName
        txtBrokerTransferToCode.Tag = CStr(lPartyCnt)

    End Sub


    Private Sub cmdRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemove.Click

        m_lReturn = RemoveGroups()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to deselect Risk Groups", "Error")
        End If
    End Sub

    Private Sub cmdRemoveAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveAll.Click

        m_lReturn = RemoveAllGroups()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Interaction.MsgBox("Failed to deselect Risk Groups", "Error")
        End If

    End Sub
    'DC220803 -PS253 -fsa compliance -end
    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            Me.Activate()
        End If
    End Sub

    Private Sub lvwAddress_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwAddress.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        lvwAddress_Click(lvwAddress, New EventArgs())
    End Sub

    Private Sub lvwContact_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContact.Click
        If Not (lvwContact.FocusedItem Is Nothing) Then
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        Else
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        End If

    End Sub

    Private Sub lvwContact_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwContact.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        lvwContact_Click(lvwContact, New EventArgs())
    End Sub

    Private Sub cmdMaintain_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMaintain.Click
        MaintainSuppressedDocs()
    End Sub

    Private Sub txtCommonRenewalDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommonRenewalDate.Enter
        txtCommonRenewalDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtCommonRenewalDate.Text)
    End Sub


    Private Sub txtCommonRenewalDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommonRenewalDate.Leave
        txtCommonRenewalDate.Text = gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, txtCommonRenewalDate.Text)
    End Sub

    Private Sub txtDateCancelled_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateCancelled.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateCancelled)
    End Sub


    Private Sub txtDateCancelled_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateCancelled.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateCancelled)
    End Sub

    Private Sub uctAgentType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctAgentType.Click
        If uctAgentType.ItemCaption = "Sub-Agent" Then
            chkMakeLiveInvoice.Enabled = False
            chkMakeLiveInstallments.Enabled = False
            chkMakeLivePayNow.Enabled = False
            chkMakeLiveBankGuarantee.Enabled = False
            'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
            chkMakeLiveCashDeposit.Enabled = False

            chkMakeLiveInvoice.CheckState = CheckState.Unchecked
            chkMakeLiveInstallments.CheckState = CheckState.Unchecked
            chkMakeLivePayNow.CheckState = CheckState.Unchecked
            chkMakeLiveBankGuarantee.CheckState = CheckState.Unchecked
            'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
            chkMakeLiveCashDeposit.CheckState = CheckState.Unchecked

            fraMakeLive.Enabled = False

            'PN 33575 (RC)
            chkAltRefMandatory.Enabled = False
            chkAltRefRequiredForEachTrans.Enabled = False
            chkAltRefMandatory.CheckState = CheckState.Unchecked
            chkAltRefRequiredForEachTrans.CheckState = CheckState.Unchecked
            fraAltRef.Enabled = False
            fraAccountLimits.Visible = False
            If bVisibleCertificateYearTab Then
                SSTabHelper.SetTabVisible(tabMainTab, AC_TAB_PAG_CERTIFICATEYEARS, True)
                SSTabHelper.SetTabCaption(tabMainTab, AC_TAB_PAG_CERTIFICATEYEARS, "11-Certificate Years")
            Else
                SSTabHelper.SetTabVisible(tabMainTab, AC_TAB_PAG_CERTIFICATEYEARS, False)
            End If
        Else
            chkMakeLiveInvoice.Enabled = True
            chkMakeLiveInstallments.Enabled = True
            chkMakeLivePayNow.Enabled = True
            chkMakeLiveBankGuarantee.Enabled = True
            If Not uctAgentType.IsListIndexSetByCode Then
                chkMakeLiveInvoice.CheckState = CheckState.Checked
                chkMakeLiveInstallments.CheckState = CheckState.Checked
                chkMakeLivePayNow.CheckState = CheckState.Checked
                chkMakeLiveBankGuarantee.CheckState = CheckState.Checked
                'Sankar - (WPR85_Cash_Deposit_Process) - Paralleling
                chkMakeLiveCashDeposit.CheckState = CheckState.Checked
            End If
            fraMakeLive.Enabled = True

            'PN 33575 (RC)
            chkAltRefMandatory.Enabled = True
            chkAltRefRequiredForEachTrans.Enabled = True
            fraAltRef.Enabled = True
            If Conversion.Val(m_vPayNowOptionValue) = 1 Then
                fraAccountLimits.Visible = True
            End If
            SSTabHelper.SetTabVisible(tabMainTab, AC_TAB_PAG_CERTIFICATEYEARS, False)
        End If

        If uctAgentType.ItemCaption = "Broker" Then
            chkSingleInstalmentPlanOnly.Visible = True
            lblCommonRenewalDate.Visible = True
            txtCommonRenewalDate.Visible = True
            fraPremiumSettlement.Visible = True
        Else
            chkSingleInstalmentPlanOnly.CheckState = CheckState.Unchecked
            chkSingleInstalmentPlanOnly.Visible = False
            txtCommonRenewalDate.Visible = False
            lblCommonRenewalDate.Visible = False
            OptNetOfcommission.Checked = True
            fraPremiumSettlement.Visible = False
        End If

        '(RC) PLICO 9-10
        SetCommisionRelease()

    End Sub

    Private Sub uctBranch_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctBranch.SelectionChangeCommitted

        m_lReturn = PartyFunc.GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)

        If m_bShowSubBranchID Then
            cboSubBranch.SelectedIndex = -1
        Else
            'Choose first item by default
            cboSubBranch.SelectedIndex = 0
        End If

        ' RDC 20042004
        m_lReturn = GetSourceBaseCurrency()

    End Sub


    Private Sub uctBranch_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctBranch.Enter

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        'SD 14/08/2002

        If SSTabHelper.GetTabVisible(tabMainTab, 0) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
        End If

    End Sub


    ' PRIVATE Methods (End)

    Private Sub cboSource_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSource.Enter
        m_lReturn = m_oFormFields.GotFocus(cboSource)
    End Sub

    Private Sub cboSource_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboSource.Leave
        m_lReturn = m_oFormFields.LostFocus(cboSource)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: MaintainSuppressedDocs
    '
    ' Description: Maintain Suppressed Documents
    '
    ' History: 19/07/2002 CMG/PB - Created.
    '
    ' ***************************************************************** '
    Private Function MaintainSuppressedDocs() As Integer

        Dim result As Integer = 0
        Dim oDetail As frmDetails
        Dim sTag As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get a new instance of the form
            oDetail = New frmDetails()

            ' Load it

            ' Pass the array through for the list box
            ' Pass the array through for the combo box
            'developer guide no 24
            oDetail.DocsAvailable = m_vDocsAvailable
            oDetail.DocsSuppressed = m_vDocsSuppressed
            ' Initialise it
            'developer guide no. 9
            m_lReturn = oDetail.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse's pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Show it
            oDetail.ShowDialog()

            ' Get the values
            If oDetail.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' Warn about data changed when exiting
                m_bDataChanged = True

                'get detail form data to save must redim to refresh suppressed docs list
                m_vDocsSuppressed = oDetail.DocsSuppressed


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Terminate it
            oDetail.Dispose()

            ' Unload the form and remove it
            oDetail.Close()

            oDetail = Nothing

            ' Refresh the list
            PopulateSuppressedDocs()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MaintainSuppressedDocs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MaintainSuppressedDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Private Sub cmdAddAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAd.Click
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.
        Dim sTmp As String = ""
        Dim oListItem As ListViewItem

        Try

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create icontact if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the address interface object via
                ' the public object manager.
                Dim temp_m_oAddress As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oAddress, sClassName:="iPMBAddress.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oAddress = temp_m_oAddress

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get address", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference

            m_oAddress.Reference = txtIDReference.Text

            m_oAddress.PostCode = m_sMainPostCode

            'PN 19428
            If uctBranch.SelectedIndex > 0 Then
                'developer guide no.162
                m_oAddress.CountryID = gPMFunctions.ToSafeLong(m_vSourceArray(3, uctBranch.SelectedIndex - 1))
            Else
                m_oAddress.CountryID = DefaultCountryID
            End If

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_SScreenHierarchy = "" Then
                m_SScreenHierarchy = $"Agent({txtIDReference.Text.Trim()})"
            End If

            m_oAddress.UniqueId = m_sUniqueId
            m_oAddress.ScreenHeirarchy = m_SScreenHierarchy

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to list

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
            Select Case (m_sDefaultCountryCode.Trim())
                Case "GBR"
                    ' Add the data to the list view


                    oListItem = lvwAddress.Items.Add(m_oAddress.PostalCode, ACIADDRESS)
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


                    oListItem = lvwAddress.Items.Add(m_oAddress.AddressUsageType, ACIADDRESS)
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


            If m_oAddress.AddressUsageType = gSIRLibrary.SIRMainAddressABIDescription Then

                m_sMainPostCode = m_oAddress.PostalCode
            End If

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAddress, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddCon.Click

        Dim oListItem As ListViewItem

        Try

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create icontact if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contact interface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contacts", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            'set the main postcode and reference
            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode


            m_lReturn = m_oContact.ContactCnt

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_SScreenHierarchy = "" Then
                m_SScreenHierarchy = $"Agent({txtIDReference.Text.Trim()})"
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_SScreenHierarchy

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()


            oListItem = lvwContact.Items.Add(m_oContact.AreaCode)

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

            ' Store the Contact tag

            oListItem.Tag = m_oContact.ContactCnt

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwContact, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    'Private Sub cmdAgentLookUp_Click()
    '
    'Dim vCnt As Variant
    'Dim vShortName As Variant
    'Dim vName As Variant
    '
    '
    '    On Error GoTo Err_cmdAgentLookUp_Click
    '
    '    m_lReturn& = SelectParty(vPartyCnt:=vCnt, _
    ''                            vShortName:=vShortName, _
    ''                            vName:=vName, _
    ''                            vAgentOnly:=1)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Exit Sub
    '    End If
    '
    '    'save the count in the tag and update controls
    ''    txtHeadOffice.Tag = CStr(vCnt)
    '
    '    m_sHeadOfficeRef$ = CStr(vShortName)
    ''    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtHeadOffice, _
    '''                                            vControlValue:=m_sHeadOfficeRef$)
    '
    '    'because we know Agent cnt matches the Agent ref, can bypass
    '    'the validation at the end
    '    m_bVerifyHeadOfficeCnt = False
    '
    '    Exit Sub
    '
    'Err_cmdAgentLookUp_Click:
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:=PMErrorText, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="cmdAgentLookUp_Click", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    Private Sub cmdAgentGroupLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAgentGroupLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp, vResolvedName As String

        Try

            'developer guide no. 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="AGG", vResolvedName:=vResolvedName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtAgentGroupRef.Tag = CStr(vCnt)


            m_sAgentGroupRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAgentGroupRef, vControlValue:=m_sAgentGroupRef)

            'SD 14/08/2002
            '    m_sAgentGroupName = CStr(vResolvedName)

            m_sAgentGroupName = CStr(vResolvedName)

            sTemp = m_sAgentGroupName
            m_lReturn = DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            'developer guide no. 26
            plblAgentGroupName.Text = sTemp
        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentGroupLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    '******************************************************************************
    ' Function: DoubleCharacter
    '
    ' Description: Loops through a string and doubles each instance of
    '              the passed character. Defaults to ' if no character is
    '              passed.
    '
    ' Note: Modified from the "Apostrophes" function.
    ' PW090702 - copied from uctPartyCC
    '
    '******************************************************************************
    Private Function DoubleCharacter(ByRef r_sString As String, Optional ByVal v_sChar As String = "") As Integer

        Dim result As Integer = 0
        Dim i As Integer
        Dim sTemp As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 131
            If r_sString Is Nothing AndAlso r_sString = String.Empty Then
                Return result
            End If
            'If r_sString.Length = 0 Then
            '    Return result
            'End If

            ' Default to apostrophe
            If Not True Then
                v_sChar = "'"
            End If

            sTemp = New StringBuilder("")

            Do While True
                i = (r_sString.IndexOf(v_sChar) + 1)

                If i = 0 Then
                    sTemp.Append(r_sString)
                    Exit Do
                End If

                sTemp.Append(r_sString.Substring(0, i - 1) & v_sChar & v_sChar)
                r_sString = r_sString.Substring(i)
            Loop

            r_sString = sTemp.ToString()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to double character " & v_sChar & " in string " & r_sString, vApp:=ACApp, vClass:=ACClass, vMethod:="DoubleCharacter", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdConsultantLookup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdConsultantLookup.Click

        Dim vCnt, vShortName, vName As Object
        Dim sTemp, vResolvedName As String

        Try




            'developer guide no 98
            m_lReturn = SelectParty(vPartyCnt:=vCnt, vShortName:=vShortName, vName:=vName, vSpecialParty:="CO", vResolvedName:=vResolvedName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'save the count in the tag and update controls

            txtConsultantRef.Tag = CStr(vCnt)


            m_sConsultantRef = CStr(vShortName)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtConsultantRef, vControlValue:=m_sConsultantRef)

            m_sConsultantName = vResolvedName

            sTemp = m_sConsultantName
            m_lReturn = DoubleCharacter(r_sString:=sTemp, v_sChar:="&")

            'developer guide no. 26
            plblConsultantName.Text = sTemp

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdConsultantLookUp_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeleteAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAd.Click
        Try

            'Set row to be deleted - if a valid one selected
            If lvwAddress.Items.Count < 1 Then
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

            'set the address id


            m_oAddress.AddressCnt = m_lAddressCnt

            m_oAddress.AddressUsageTypeID = m_lAddressUsageTypeID

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

            lvwAddress.Items.RemoveAt(m_iLine - 1)
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub cmdDeleteCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteCon.Click

        Try

            'Set row to be deleted - if a valid one selected
            If lvwContact.Items.Count < 1 Then
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

            'Use pmview instead
            'm_lReturn& = m_oContact.SetProcessModes(vTask:=PMDelete)
            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMView)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'set the contact id

            m_oContact.ContactCnt = Convert.ToString(lvwContact.FocusedItem.Tag)
            'm_oContact.ContactCnt = m_lContactCnt&

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            lvwContact.Items.RemoveAt(lvwContact.FocusedItem.Index)
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False

            lvwContact.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteCon_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditAd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAd.Click
        'RWH(21/07/2000) Altered to move PostCode from far left to far
        'right of ListView.
        Dim lAddressCnt As Integer
        Dim sTmp As String = ""
        Dim oListItem As ListViewItem
        Dim sAddressUsage As String = ""

        Try

            'Set the address count being edited - if a valid one selected
            If lvwAddress.Items.Count < 1 Then
                Exit Sub
            End If

            'Create address component if not already done so
            If m_oAddress Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                'RWH(24/07/2000) Changed from 'iSIRAddress.Interface'
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

            '    'set the main postcode and reference

            m_oAddress.Reference = txtIDReference.Text
            '    m_oAddress.PostCode = pnlIDPostCode

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            oListItem = lvwAddress.FocusedItem

            'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
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

                    m_oAddress.AddressUsageTypeID = m_vAddressTypes(0, k)
                    Exit For
                End If
            Next k

            ' Get the address count

            lAddressCnt = Convert.ToString(lvwAddress.FocusedItem.Tag)

            'set the address id

            m_oAddress.AddressCnt = lAddressCnt

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_SScreenHierarchy = "" Then
                m_SScreenHierarchy = $"Agent({txtIDReference.Text.Trim()})"
            End If

            m_oAddress.UniqueId = m_sUniqueId
            m_oAddress.ScreenHeirarchy = m_SScreenHierarchy

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            With lvwAddress.Items.Item(m_iLine - 1)
                'RWH(24/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Postcode

                        .Text = m_oAddress.PostalCode
                        ' Address usage type

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 1).Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 2).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 3).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 4).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 5).Text = m_oAddress.Address4
                    Case Else
                        ' Address usage type

                        .Text = m_oAddress.AddressUsageType
                        ' Address lines 1-4

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 1).Text = m_oAddress.Address1

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 2).Text = m_oAddress.Address2

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 3).Text = m_oAddress.Address3

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 4).Text = m_oAddress.Address4
                        'Postcode

                        ListViewHelper.GetListViewSubItem(lvwAddress.Items.Item(m_iLine - 1), 5).Text = m_oAddress.PostalCode
                End Select
                ' Store the AddressCnt in the tag


                .Tag = m_oAddress.AddressCnt
            End With

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAddress, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditCon_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditCon.Click

        Dim oListItem As ListViewItem

        Try
            'Set row to be deleted - if a valid one selected
            If lvwContact.Items.Count < 1 Then
                Exit Sub
            End If

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create address component if not already done so
            If m_oContact Is Nothing Then

                ' Get an instance of the contactinterface object via
                ' the public object manager.
                Dim temp_m_oContact As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oContact, sClassName:="iPMBContact.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oContact = temp_m_oContact

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Reset the moust pointer
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get contact component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oContact.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            m_oContact.Reference = txtIDReference.Text

            m_oContact.PostCode = m_sMainPostCode

            'set the contact id
            oListItem = lvwContact.FocusedItem

            m_oContact.ContactCnt = Convert.ToString(oListItem.Tag)


            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_sUniqueId = "" Then
                m_sUniqueId = GetUniqueID()
            End If

            If m_SScreenHierarchy = "" Then
                m_SScreenHierarchy = $"Agent({txtIDReference.Text.Trim()})"
            End If

            m_oContact.UniqueId = m_sUniqueId
            m_oContact.ScreenHeirarchy = m_SScreenHierarchy

            m_lReturn = m_oContact.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oContact.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

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

            ' Store the Contact tag

            oListItem.Tag = m_oContact.ContactCnt

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwContact, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditCon_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditCon_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' PW090702 - added special party and resolved name
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Integer, ByRef vShortName As String, Optional ByRef vName As String = "", Optional ByRef vAgentOnly As String = "", Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As String = "", Optional ByRef bSuppressSubAgents As Boolean = False, Optional ByRef bSuppressCancelledAgents As Boolean = False) As Integer

        Dim result As Integer = 0
        'Dim oFindParty As iPMBFindParty.Interface
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide 108
            'oFindParty = New iPMBFindParty.Interface()
            oFindParty = New iPMBFindParty.Interface_Renamed()

            'Set appropriate key if agent only


            If (Not Information.IsNothing(vAgentOnly)) And (Not String.IsNullOrEmpty(vAgentOnly)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = gSIRLibrary.SIRNavKeyAgentOnly

                vKeyArray(1, 0) = vAgentOnly

                m_lErrorNumber = CType(oFindParty.SetKeys(vKeyArray), gPMConstants.PMEReturnCode)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' PW090702 - Set appropriate key if special party


            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = CType(oFindParty.SetKeys(vKeyArray), gPMConstants.PMEReturnCode)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            'developer guide no. 9
            m_lErrorNumber = oFindParty.Initialise()
            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = "iPMBPartyAG.Interface"

            '02082002 CMG/PB scalabiltiy changes explicit constant
            m_lErrorNumber = CType(oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=GIIConstants.GIIPMProcessModeGeneric, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)
            'End CMG

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'suppress sub agents if applicable
            oFindParty.SuppressSubAgents = bSuppressSubAgents

            ' RDC 20040901
            If Not False Then
                oFindParty.SuppressCancelledAgents = bSuppressCancelledAgents
            End If

            m_lErrorNumber = CType(oFindParty.Start(), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                vPartyCnt = oFindParty.PartyCnt
                vShortName = oFindParty.ShortName

                If Not Information.IsNothing(vName) Then
                    vName = oFindParty.LongName
                End If
                vResolvedName = oFindParty.ResolvedName
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
    ' Name: UpdateAddresses
    '
    ' Description: This goes thru all addresses in the the grid control
    ' and the original address array and sees what the differences
    ' are. It then adds new addresses or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateAddresses() As Integer
        'RWH(21/07/2000) Altered to move PostCode from far left to far
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
                If i > lvwAddress.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwAddress.Items.Item(i - 1)

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

                    For j As Integer = vNewAddresses.GetLowerBound(1) To vNewAddresses.GetUpperBound(1)




                        If (vNewAddresses(0, j).Equals(vOldAddresses(0, i))) And (vNewAddresses(1, j).Equals(vOldAddresses(1, i))) Then

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
                If i > lvwContact.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwContact.Items.Item(i - 1)
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

    ' ***************************************************************** '
    ' Name: UpdateRiskGroups
    '
    ' Description: This goes thru all risk groups in the the grid control
    ' and the original risk group array and sees what the differences
    ' are. It then adds new risk groups or deletes existing ones according
    ' to what user has done.
    '
    ' ***************************************************************** '
    Private Function UpdateRiskGroups() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim bFirst As Boolean
        Dim i, k As Integer
        Dim bFound As Boolean
        Dim vNewRiskGroups, vDelRiskGroups As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check for risk groups to be added

            i = 1
            k = 1
            bFirst = True

            Do
                If i > lvwCompetent.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwCompetent.Items.Item(i - 1)

                bFound = False

                For j As Integer = m_vRiskGroups.GetLowerBound(1) To m_vRiskGroups.GetUpperBound(1)

                    If Convert.ToString(oListItem.Tag).Equals(m_vRiskGroups(0, j)) And CStr(m_vRiskGroups(3, j)) = "NEW" Then
                        bFound = True
                    End If
                Next j

                If bFound Then

                    If bFirst Then
                        ReDim vNewRiskGroups(k - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vNewRiskGroups(k - 1)
                    End If



                    vNewRiskGroups(k - 1) = Convert.ToString(oListItem.Tag)

                    k += 1

                End If

                i += 1

            Loop

            'Now check for those risk groups to be removed.

            i = 1
            k = 1
            bFirst = True

            Do
                If i > lvwUnderTraining.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwUnderTraining.Items.Item(i - 1)

                bFound = False

                For j As Integer = m_vRiskGroups.GetLowerBound(1) To m_vRiskGroups.GetUpperBound(1)

                    If Convert.ToString(oListItem.Tag).Equals(m_vRiskGroups(0, j)) And CStr(m_vRiskGroups(3, j)) = "DEL" Then
                        bFound = True
                    End If
                Next j

                If bFound Then

                    If bFirst Then
                        ReDim vDelRiskGroups(k - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vDelRiskGroups(k - 1)
                    End If



                    vDelRiskGroups(k - 1) = Convert.ToString(oListItem.Tag)

                    k += 1

                End If

                i += 1

            Loop

            If Information.IsArray(vNewRiskGroups) Or Information.IsArray(vDelRiskGroups) Then

                m_lReturn = m_oBusiness.UpdateRiskGroups(vPartyCnt:=m_lPartyCnt, vNewRiskGroups:=vNewRiskGroups, vDelRiskGroups:=vDelRiskGroups)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskGroupsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskGroups", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
        ' Fire up the help screen
        'developer guide no. 51 (no solution)
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, ScreenHelpID)

    End Sub

    Private Sub cmdMaintainAssociates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMaintainAssociates.Click

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

            vTask = gPMConstants.PMEComponentAction.PMEdit

            m_lReturn = m_oAssociates.SetProcessModes(vTask:=vTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oAssociates.PartyCnt = m_lPartyCnt



            m_oAssociates.SearchData = m_vAssociates


            m_oAssociates.Relationships = m_vRelationships

            m_oAssociates.PartyType = gSIRLibrary.SIRPartyTypeAgent


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

            PopulateAssociates()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdMaintainAssociates_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMaintainAssociates_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_10.Click, _cmdPrevious_9.Click, _cmdPrevious_8.Click, _cmdPrevious_7.Click, _cmdPrevious_6.Click, _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_3.Click, _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Try

            Dim iTab As Integer

            'Change to the previous tab.
            'The "previous" tab may be hidden so search for the next visible one
            iTab = SSTabHelper.GetSelectedIndex(tabMainTab) - 1
            Do While (iTab > -1)
                If SSTabHelper.GetTabVisible(tabMainTab, iTab) Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, iTab)
                    Exit Do
                End If
                iTab -= 1
            Loop

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                'm_ctlTabFirstLast(ACControlStart, Index + 1).SetFocus
                m_ctlTabFirstLast(ACControlStart, Index).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub
    Private Sub cmdRates_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRates.Click
        Try
            'Create shares object if not already done so
            If m_oRates Is Nothing Then

                ' Get an instance of the shares interface object via
                ' the public object manager.
                Dim temp_m_oRates As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oRates, sClassName:="iPMBAgentRateFind.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oRates = temp_m_oRates

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Rates object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRates_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oRates.SetProcessModes(vTask:=m_iTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oRates.PartyCnt = m_lPartyCnt

            m_oRates.PartyName = m_sName

            '030506 Datasure
            If m_bDomiciledForTax Then


                m_oRates.TaxRegistered = 1

            Else


                m_oRates.TaxRegistered = 0

            End If

            'If Abs(CLng(m_bTaxExempt)) = 1 Then
            '    m_oRates.TaxRegistered = 0
            'Else
            '    m_oRates.TaxRegistered = 1
            'End If



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



    Private Sub ddTitle_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddTitle.GotFocus

        ' CTAF 220900
        ' Make sure we're on the right tab incase this was called
        ' from form controls.
        'SD 14/08/2002
        If SSTabHelper.GetTabVisible(tabMainTab, 5) Then
            SSTabHelper.SetSelectedIndex(tabMainTab, 5)
        End If

        If ddTitle.Text = "" Then
            m_lReturn = HighlightContol(ddTitle, optBoolDropDown:=True)
        End If

    End Sub

    '************************************************************************************************
    ' Name             :   HighlightControl
    ' Created by       :   Ram Chandrabose
    ' Date             :   29-Oct-1999
    ' Function for     :   Highlight the contents of the control
    ' Called from      :   Control's Got_Focus Event
    ' Input Parameters :   1.  Ctl              - Control
    '                      2.  optBoolDateField - Boolean  ( Optional Parameter )
    '                               if True     - Set the Control with 'DD/MM/YYYY' as default value.
    ' Edit History     :
    '*************************************************************************************************
    Public Function HighlightContol(ByRef ctl As PMListMgrDropdown.uctDropdown, Optional ByRef optBoolDateField As Boolean = False, Optional ByRef optBoolDropDown As Boolean = False) As Integer

        Dim result As Integer = 0
        Try

            With ctl

                ' For date fields only
                If Not False Then
                    If optBoolDateField Then
                        ' Convert date to short format on GotFocus
                        If Information.IsDate(.Text) Then
                            .Text = StringsHelper.Format(CDate(.Text), GIIConstants.GEMShortDate)
                        Else
                            .Text = GIIConstants.GEMShortDate
                        End If
                    End If
                End If

                ' Highlight the contents of the control
                .SelStart = 0
                .SelLength = Strings.Len(.Text)
            End With

            ' To Make explicit Drop Down using API
            If Not False Then
                If optBoolDropDown Then
                    Dim handle2 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
                    Try
                        Dim tmpPtr As IntPtr = handle2.AddrOfPinnedObject()
                        SendMessage(ctl.hwnd, CB_SHOWDROPDOWN, True, tmpPtr)
                    Finally
                        handle2.Free()
                    End Try
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in HightlightControl [" & ctl.Name & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="HighlightContol", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function


    Private Sub ddTitle_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles ddTitle.LostFocus

        m_lReturn = ValidateListField(ddTitle)

    End Sub


    ' ***************************************************************** '
    '
    ' Name: ValidateListField
    '
    ' Description:
    '
    ' History: 03/12/1999 JSB - Created.
    '
    ' ***************************************************************** '
    Public Function ValidateListField(ByRef ddList As PMListMgrDropdown.uctDropdown) As Integer

        Dim result As Integer = 0
        Dim sText As String = ""
        Dim bFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFound = False

            ' hold the current value
            sText = ddList.Text.ToUpper()

            ' Loop through the list
            For lCount As Integer = 0 To ddList.ListCount - 1

                ' See if the value matches the list item
                If ddList.List(lCount).ToUpper() = sText Then
                    ' If so, set the value to the list item
                    ddList.Text = ddList.List(lCount)
                    bFound = True
                End If

            Next

            If Not bFound Then
                ddList.Text = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateListField Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateListField", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyAG.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            'DC131204
            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oFindBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindBusiness, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oFindBusiness = temp_m_oFindBusiness

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
            m_oGeneral = New iPMBPartyAG.General()

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

            ' List manager
            m_oListManager = New iGEMListManager.Interface_Renamed()

            ' Initialise it
            'developer guide no 9
            m_lReturn = m_oListManager.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Check for latest version
            m_lReturn = m_oListManager.CheckListVersions()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

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

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


            m_sAgencyOrUnderwriting = m_oBusiness.UnderwritingOrAgency

            m_lReturn = PartyFunc.GetHiddenOptions(v_lSourceId:=g_iSourceID, r_vIsNRMA:=m_bIsNRMA)

            m_lReturn = CheckForUnderwritingBranch(v_iSourceId:=g_iSourceID, r_bUnderwritingBranchEnabled:=m_bUnderwritingBranchEnabled, r_bIsUnderwritingBranch:=m_bIsUnderwritingBranch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckForUnderwritingBranch Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize")
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If


            m_bShowSubBranchID = True

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


            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="is_view_only_agents_maintenance", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=m_bIsViewOnlyAgentMaintenance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            Else
                'Start - Prakash Varghese - PN 61117
                'Modified the condition ToSafeBoolean(m_bIsViewOnlyAgentMaintenance) = True
                'to ToSafeBoolean(m_bIsViewOnlyAgentMaintenance) since it is giving problems in runtime
                If gPMFunctions.ToSafeBoolean(m_bIsViewOnlyAgentMaintenance) Then
                    m_iTask = gPMConstants.PMEComponentAction.PMView
                    m_bIsViewOnlyAgent = True
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

        Dim Key As uctPickList.PickListKey ' 'Agent Filtering


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

            ' Set the business keys.
            ' {* USER DEFINED CODE (Begin) *}

            m_oBusiness.PartyCnt = m_lPartyCnt
            ' {* USER DEFINED CODE (End) *}

            'DC220803 -PS253 -fsa compliance
            m_lReturn = GetHiddenOption(v_lSourceId:=g_iSourceID, r_vEnableFSACompliance:=m_bEnableFSACompliance)
            m_lReturn = GetHiddenOptionForCertificateYearTab(iSourceId:=g_iSourceID, bCertificateYearTab:=bVisibleCertificateYearTab)
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
            'developer guide no. 38
            Me.uctCategory.FirstItem = "(none)"
            Me.cboPaymentMethod.FirstItem = "(N/A)"
            Me.uctAgentType.FirstItem = "(none)"
            Me.cboCommissionPostingType.FirstItem = "(none)"
            Me.cboBankAccount.FirstItem = "(Any)"
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

            'we want the currency stored with this agent not the base currency associate to this company_id
            uctCurrency.CurrencyId = m_lCurrencyID
            cboBankAccount.Visible = True
            lblBankAccount.Visible = True
            If m_lCurrencyID = 0 Then
                PopulateBankAccount(uctCurrency.DefaultCurrencyId)
            Else
                PopulateBankAccount(m_lCurrencyID)
            End If


            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

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

            'SJ 01/03/2004 - start
            If m_bIsUnderwritingBranch Then
                txtBrokerAbiId.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            End If
            'SJ 01/03/2004 - end


            ''Agent Filtering

            'Setup the picklists
            ' RAW 09/04/2003 : IAG ENDVR619 : changed to use class from uctPicklist
            Key = New uctPickList.PickListKey()
            Key.KeyName = "Party_Cnt"
            Key.ValueType = gPMConstants.PMEDataType.PMInteger
            uctPickListBranches.ForeignKeys.Add(Key, Key:="Party_Cnt")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "Branchid"
            Key.ValueType = gPMConstants.PMEDataType.PMInteger
            uctPickListBranches.ForeignKeys.Add(Key, Key:="Branchid")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "user_id"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="user_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "unique_id"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="unique_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "screen_hierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="screen_hierarchy")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "Party_Cnt"
            Key.ValueType = gPMConstants.PMEDataType.PMInteger
            uctPickListProducts.ForeignKeys.Add(Key, Key:="Party_Cnt")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "Product_id"
            Key.ValueType = gPMConstants.PMEDataType.PMInteger
            uctPickListProducts.ForeignKeys.Add(Key, Key:="Product_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "user_id"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListProducts.ForeignKeys.Add(Key, Key:="user_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "unique_id"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListProducts.ForeignKeys.Add(Key, Key:="unique_id")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "screen_hierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListProducts.ForeignKeys.Add(Key, Key:="screen_hierarchy")

            'Put in the picklist PK values
            SetPickListPKs()

            m_lReturn = uctPickListBranches.Load_Renamed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Branches", "Agent Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' GK 02/08/2010 : SAGICOR WPR 14 : added error handling
            m_lReturn = uctPickListProducts.Load_Renamed
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MsgBox("Failed to load list of Products", MsgBoxStyle.Information, "Agent Maintenance")
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'GK 03/08/2010 : SAGICOR WPR 14 : Choosing the Product
            ChooseProducts()
            ''Agent Filtering

            ' Alix - 31/10/2003 - Only allow tab stop on visible control
            txtIDReference.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)

            'DC131204

            uctAgentType.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)


            txtName.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            txtFileCode.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            txtAccountNumber.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            cboSource.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            cboBinderIndicator.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            uctCategory.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            cboReportIndicator.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            txtAgencyReviewDate.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            chkStatement.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            'commission Override
            chkOverrideCommission.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            'commission Override Renewal
            chkOverrideCommissionRen.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            cmdRates.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            uctCurrency.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            cboTermsOfPayment.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
            cmdNext(0).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)

            lvwAddress.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
            cmdAddAd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
            cmdDeleteAd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
            cmdEditAd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
            cmdPrevious(0).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
            cmdNext(1).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)

            lvwContact.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
            cmdAddCon.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
            cmdDeleteCon.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
            cmdEditCon.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
            cmdPrevious(1).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
            cmdNext(2).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)

            cboPaymentMethod.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
            cboPaymentFrequency.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)

            cboBankAccount.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
            cmdPrevious(2).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
            cmdNext(3).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)

            cboAddressOnNotice.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
            lstDocsChosen.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
            cmdMaintain.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
            cmdPrevious(3).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
            cmdNext(4).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)

            cmdConsultantLookup.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            txtConsultantRef.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            uctBranch.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            cboSubBranch.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            cmdAgentGroupLookup.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            txtAgentGroupRef.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            ddTitle.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            txtFirstName.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            txtContactPerson.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            lvwAssociates.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            cmdMaintainAssociates.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            txtDateCancelled.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            cboMultipac.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            cboRenewalStopCode.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
            cmdPrevious(4).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)

            lvwUnderTraining.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            cmdAdd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            cmdAddAll.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            cmdRemove.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            cmdRemoveAll.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            lvwCompetent.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            cmdPrevious(5).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
            cboAgentStatus.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)


            'Maintain Party Code
            If Task = gPMConstants.PMEComponentAction.PMAdd Or Task = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = SetClientCodeCntl()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            Else
                cmdApply.Visible = False
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


                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

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
            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()
            'DC131204
            ' Destroy the instance of the business object
            ' from memory.
            m_oBusiness = Nothing

            ' Terminate the business object

            m_oFindBusiness.Dispose()

            ' Destroy the instance of the business object
            ' from memory.
            m_oFindBusiness = Nothing

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
            'EK 10/10/99 Access to Commission Rates
            ' Terminate the commission rates object (if used)
            If Not (m_oRates Is Nothing) Then


                m_oRates.Dispose()

                ' Destroy the instance of the policy shares object
                ' from memory.
                m_oRates = Nothing

            End If

            ' PW110702
            ' Destroy the instance of the PMUser object
            ' from memory.
            If Not (m_oPMUser Is Nothing) Then



                m_oPMUser.Dispose()
                m_oPMUser = Nothing
            End If

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
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D8 Then
                tabMainTab.SelectedIndex = 7
            End If
            If eventArgs.Alt And eventArgs.KeyCode = Keys.D9 Then
                tabMainTab.SelectedIndex = 8
            End If

        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub


    Private Sub lvwAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.Click

        If Not (lvwAddress.FocusedItem Is Nothing) Then

            m_lAddressCnt = Convert.ToString(lvwAddress.FocusedItem.Tag)
            m_iLine = lvwAddress.FocusedItem.Index + 1
            cmdDeleteAd.Enabled = True
            cmdEditAd.Enabled = True
        Else
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False
        End If

    End Sub

    Private Sub lvwAddress_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddress.DoubleClick

        ' Active the edit button
        cmdEditAd_Click(cmdEditAd, New EventArgs())

    End Sub

    Private Sub lvwAddress_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddress.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Not (lvwAddress.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteAd.Enabled = True
            cmdEditAd.Enabled = True
        Else
            cmdDeleteAd.Enabled = False
            cmdEditAd.Enabled = False
        End If

    End Sub

    Private Sub lvwContact_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContact.DoubleClick

        ' Activate the edit button
        cmdEditCon_Click(cmdEditCon, New EventArgs())

    End Sub

    Private Sub lvwContact_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContact.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)

        If Not (lvwContact.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteCon.Enabled = True
            cmdEditCon.Enabled = True
        Else
            cmdDeleteCon.Enabled = False
            cmdEditCon.Enabled = False
        End If

    End Sub

    Public Sub mnuCommission_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuCommission.Click

        Try

            ' Call Toolbar Control function
            m_lReturn = SIRToolbarFunc.ProcessToolbar(v_iButton:=ACIButtonCommission)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch


            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuFinancial_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuFinancial.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = SIRToolbarFunc.ProcessToolbar(v_iButton:=SIRToolbarFunc.ACIButtonFinancial)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuLetter_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuLetter.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = SIRToolbarFunc.ProcessToolbar(v_iButton:=SIRToolbarFunc.ACIButtonLetter, v_lPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Public Sub mnuNotes_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuNotes.Click

        ' TF291298

        Try

            ' Call Toolbar Control function
            m_lReturn = SIRToolbarFunc.ProcessToolbar(v_iButton:=SIRToolbarFunc.ACIButtonNotes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab

                ' Set the default button.
                If SSTabHelper.GetSelectedIndex(tabMainTab) < cmdNext.Length Then
                    VB6.SetDefault(cmdNext(SSTabHelper.GetSelectedIndex(tabMainTab)), True)
                Else
                    VB6.SetDefault(cmdOK, True)
                End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

                ' Set focus to the first control on the tab.
                ' Cater for FSA tab being 6th tab at design time but 4th visible tab for broking
                If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                    m_ctlTabFirstLast(ACControlStart, SSTabHelper.GetSelectedIndex(tabMainTab)).Focus()
                End If


                'I really dont understand why i wrote it but its really
                'solving the PN-21827
                Application.DoEvents()

                ' Alix - 31/10/2003 - PN7862
                ' Talking about crap, what about being able to tab through
                ' controls which are on hidden tabs, creating a multitude of bugs?

                txtIDReference.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)

                'DC131204

                uctAgentType.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)

                txtName.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                txtFileCode.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                txtAccountNumber.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                cboSource.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                cboBinderIndicator.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                uctCategory.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                cboReportIndicator.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                txtAgencyReviewDate.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                chkStatement.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                chkOverrideCommission.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                chkOverrideCommissionRen.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                cmdRates.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                uctCurrency.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                cboTermsOfPayment.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)
                cmdNext(0).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 0)

                lvwAddress.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
                cmdAddAd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
                cmdDeleteAd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
                cmdEditAd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
                cmdPrevious(0).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)
                cmdNext(1).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 1)

                lvwContact.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
                cmdAddCon.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
                cmdDeleteCon.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
                cmdEditCon.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
                cmdPrevious(1).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)
                cmdNext(2).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 2)

                cboPaymentMethod.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
                cboPaymentFrequency.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
                cboBankAccount.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
                cmdPrevious(2).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)
                cmdNext(3).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 3)

                cboAddressOnNotice.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
                lstDocsChosen.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
                cmdMaintain.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
                cmdPrevious(3).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)
                cmdNext(4).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 4)

                cmdConsultantLookup.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                txtConsultantRef.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                uctBranch.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                cboSubBranch.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                cmdAgentGroupLookup.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                txtAgentGroupRef.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                ddTitle.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                txtFirstName.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                txtContactPerson.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                lvwAssociates.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                cmdMaintainAssociates.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                txtDateCancelled.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                cboMultipac.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                cboRenewalStopCode.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)
                cmdPrevious(4).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 5)

                lvwUnderTraining.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                cmdAdd.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                cmdAddAll.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                cmdRemove.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                cmdRemoveAll.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                lvwCompetent.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                cmdPrevious(5).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)
                cboAgentStatus.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)

                ''Agent Filtering
                cmdNext(6).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 6)

                uctPickListBranches.TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 7)
                cmdPrevious(6).TabStop = (SSTabHelper.GetSelectedIndex(tabMainTab) = 7)
                ''Agent Filtering

                ' /Alix
            End With

        Catch



            ' Error Section.


            tabMainTabPreviousTab = tabMainTab.SelectedIndex
        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim nReturn As Integer
        ' Click event of the OK button.
        Dim oPartyBusiness As bSIRParty.Business = Nothing
        Try

            'SJ 02/03/2004 - start
            Dim lPartyCnt As Integer
            Dim sTradingName As String = ""
            'SJ 02/03/2004 - end

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = ValidateNumberingScheme()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If

            If (m_iTask = gPMConstants.PMEComponentAction.PMAdd) Or (m_iTask = gPMConstants.PMEComponentAction.PMEdit) Then

                ' Check mandatory controls have been entered into.
                m_lReturn = m_oFormFields.CheckMandatoryControls()

                'Maintain Party Code
                If m_bIsReadOnly Then
                    lblIDReference.Enabled = False
                    txtIDReference.Enabled = False
                End If

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'Validate some address stuff
                m_lReturn = ValidateOK()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                'SJ 02/03/2004 - start
                If m_bIsUnderwritingBranch And txtBrokerAbiId.Text.Trim() <> "" Then
                    'Check to see if this broker abi id already exists for another agent
                    'if it does then raise an error

                    m_lReturn = m_oBusiness.GetPartyCntFromBrokerAbiId(v_sBrokerAbiId:=txtBrokerAbiId.Text.Trim(), r_lPartyCnt:=lPartyCnt, r_sTradingName:=sTradingName)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromBrokerAbiId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click")
                        Exit Sub
                    End If
                    If lPartyCnt > 0 And lPartyCnt <> m_lPartyCnt Then
                        MessageBox.Show("This broker abi id already exists for agent " & sTradingName, ACApp, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        SSTabHelper.SetSelectedIndex(tabMainTab, 0)
                        txtBrokerAbiId.Focus()
                        Exit Sub
                    End If

                End If
                'SJ 02/03/2004 - end
            End If

            ' Set the mouse pointer to the hourglass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Set the mouse back to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Me.Hide()
                Exit Sub
            End If

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                'update the party cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                ' save additional details back to party record.
                m_lReturn = UpdatePartyDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save party details data.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If

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

                'DC220803 -PS253 -fsa compliance
                If m_bEnableFSACompliance Then

                    'Update risk groups
                    m_lReturn = UpdateRiskGroups()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Risk Group Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")

                        Exit Sub
                    End If

                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()

                ' PW100702 - update associates


                m_lReturn = m_oBusiness.UpdateAssociates(vPartyCnt:=m_lPartyCnt, vAssociates:=m_vAssociates, sUniqueId:=m_sUniqueId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Associates", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If



                If m_oCommissionLevel IsNot Nothing Then
                    For i As Integer = 0 To m_vCommissionLevel.GetUpperBound(1)
                        Dim sScreenHierarchy As String = ""
                        If m_SScreenHierarchy <> "" Then
                            sScreenHierarchy = m_SScreenHierarchy & $"/Commission Level({m_vCommissionLevel(1, i).ToString().Trim()})"
                        End If
                        m_lReturn = m_oBusiness.SetCommissionLevel(v_iAutoID:=ToSafeInteger(m_vCommissionLevel(4, i)), v_iPartyAgentCnt:=m_lPartyCnt,
                                                                   v_iCommissionLevelId:=ToSafeInteger(m_vCommissionLevel(0, i)),
                                                                   v_dEffectiveDate:=ToSafeDate(m_vCommissionLevel(2, i)),
                                                                   v_bIsDeleted:=ToSafeBoolean(m_vCommissionLevel(3, i)),
                                                                   v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=sScreenHierarchy)
                    Next
                End If


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Associates", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                    Exit Sub
                End If

                'Update party CertYear
                If uctAgentType.ItemCaption = "Sub-Agent" Then
                    m_lReturn = UpdateCertYear()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to UpdateCertYearDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                End If
                'Party Bank Details
                'developer guide no.9
                m_lReturn = uctPartyBankControl1.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdOK_Click", "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                uctPartyBankControl1.PartyCnt = m_lPartyCnt
                uctPartyBankControl1.ScreenHierarchy = m_SScreenHierarchy
                uctPartyBankControl1.UniqueId = m_sUniqueId
                m_lReturn = uctPartyBankControl1.UpdatePartyBankDetails()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdOK_Click", "uctPartyBankControl1.UpdatePartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                'DC260106 PN27053 if cancel out of custom data screen will also close the form
                m_lReturn = PartyBuilderHandler.OpenPartyBuilderScreen(iTask:=m_iTask, lPartyCnt:=m_lPartyCnt)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Or m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If

                nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If

                oPartyBusiness.AddPartyHistory(m_lPartyCnt, String.Empty)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If
            End If

        Catch excep As System.Exception
            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        Finally
            If oPartyBusiness IsNot Nothing Then
                oPartyBusiness.Dispose()
                oPartyBusiness = Nothing
            End If
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

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_10.Click, _cmdNext_9.Click, _cmdNext_8.Click, _cmdNext_7.Click, _cmdNext_6.Click, _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_3.Click, _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try
            Dim iTab, iTab2 As Integer

            ' Change to the next tab.
            'The "next" tab may be hidden so search for the next visible one
            iTab = SSTabHelper.GetSelectedIndex(tabMainTab) + 1
            Do While (iTab < SSTabHelper.GetTabCount(tabMainTab))
                If SSTabHelper.GetTabVisible(tabMainTab, iTab) Then
                    SSTabHelper.SetSelectedIndex(tabMainTab, iTab)
                    'we now have the tab selected, are there any more visible? Do we need to enable the next button?
                    cmdNext(iTab).Visible = False 'assume no more visible tabs
                    iTab2 = iTab + 1
                    Do While (iTab2 < SSTabHelper.GetTabCount(tabMainTab))
                        If SSTabHelper.GetTabVisible(tabMainTab, iTab2) Then
                            cmdNext(iTab).Visible = True
                            Exit Do
                        End If
                        iTab2 += 1
                    Loop
                    Exit Do
                End If
                iTab += 1
            Loop

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar1_Button1.Click, _Toolbar1_Button2.Click, _Toolbar1_Button3.Click, _Toolbar1_Button4.Click, _Toolbar1_Button5.Click, _Toolbar1_Button6.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        ' TF291298

        Try

            ' Call Toolbar Control function
            ' Party Bank Details
            ' Added new optional Parameter PartyCnt
            'developer guide no.111
            iPMBListEvents.g_oObjectManager = g_oObjectManager
            PMBToolbarFunc.g_oObjectManager = g_oObjectManager
            m_lReturn = SIRToolbarFunc.ProcessToolbar(v_iButton:=Button.Owner.Items.IndexOf(Button) + 1, v_lPartyCnt:=m_lPartyCnt, v_sPartyCode:=m_sShortName, v_sPartyName:=m_sName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try


    End Sub

    Private Sub txtAccountNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAccountNumber)
    End Sub

    Private Sub txtAccountNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAccountNumber)
    End Sub

    Private Sub txtAgencyReviewDate_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgencyReviewDate.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgencyReviewDate)
    End Sub

    Private Sub txtAgencyReviewDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgencyReviewDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgencyReviewDate)
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub txtAgentGroupRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentGroupRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bVerifyAgentGroupCnt = True
    End Sub

    Private Sub txtAgentGroupRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentGroupRef.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAgentGroupRef)
    End Sub

    Private Sub txtAgentGroupRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAgentGroupRef.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAgentGroupRef)
    End Sub

    Private Sub txtConsultantRef_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        m_bVerifyConsultantCnt = True
    End Sub

    Private Sub txtConsultantRef_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtConsultantRef)
    End Sub


    Private Sub txtConsultantRef_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtConsultantRef.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtConsultantRef)
    End Sub


    Private Sub txtFileCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtFileCode)
    End Sub

    Private Sub txtFileCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtFileCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtFileCode)
    End Sub

    'Private Sub txtAgencyNextReview_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtAgencyNextReview)
    '
    'End Sub

    'Private Sub txtAgencyNextReview_LostFocus()
    'EK BUG158 07/01/99 This validation is now done in Validate OK
    '    If IsDate(txtAgencyNextReview) Then
    '
    '        If Year(txtAgencyNextReview) < 1900 Then
    '            txtAgencyNextReview = ""
    '        End If
    '
    '    End If

    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtAgencyNextReview)

    'End Sub

    'Private Sub txtheadoffice_Change()
    '
    '    'Agent ref may no longer match the party_cnt in the tag, so need to
    '    'verify this when validating
    '    m_bVerifyHeadOfficeCnt = True
    '
    'End Sub
    '
    'Private Sub txtheadoffice_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtHeadOffice)
    '
    'End Sub
    '
    '
    'Private Sub txtheadoffice_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtHeadOffice)
    '
    'End Sub


    'Private Sub txtAgencyAgreement_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=txtAgencyAgreement)
    '
    'End Sub
    '
    'Private Sub txtAgencyAgreement_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=txtAgencyAgreement)
    '
    'End Sub

    'Private Sub chkisbranch_GotFocus()
    '
    '    m_lReturn& = m_oFormFields.GotFocus(ctlControl:=chkIsBranch)
    '
    'End Sub
    '
    '
    'Private Sub chkisbranch_LostFocus()
    '
    '    m_lReturn& = m_oFormFields.LostFocus(ctlControl:=chkIsBranch)
    '
    'End Sub


    ' PRIVATE Events (End)


    ' PRIVATE Events (End)
    Private Sub txtIDReference_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtIDReference.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If

        'Set the reference on the second and third tab
        'pnlAdReference.Caption = txtIDReference.Text
        'pnlConReference.Caption = txtIDReference.Text


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

        uctPartyBankControl1.PartyName = txtName.Text
    End Sub
    Private Sub txtBrokerAbiId_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerAbiId.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtBrokerAbiId)
    End Sub
    Private Sub txtBrokerAbiId_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtBrokerAbiId.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtBrokerAbiId)
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
            '            m_lReturn& = oSIROrionUpdate.SiriusToOrion( _
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

    'SD 14/08/2002
    ' ***************************************************************** '
    ' Name: PopulateYesNoCombo
    '
    ' Description: Add Yes,No, and Blank to the combo box listitems
    '
    ' ***************************************************************** '
    Private Function PopulateYesNoCombo(ByRef r_oCombo As ComboBox) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If TypeOf r_oCombo Is ComboBox Then
                With r_oCombo
                    .Items.Clear()
                    .Items.Add(ACConstantBlank)
                    .Items.Add(ACConstantYes)
                    .Items.Add(ACConstantNo)
                    .SelectedIndex = 0
                End With
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateYesNoCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateYesNoCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'SD 14/08/2002
    ' ***************************************************************** '
    ' Name: GetAssociateNameFromShortName
    '
    ' Description: Returns party name given shortname
    '
    ' ***************************************************************** '
    Private Function GetAssociateNameFromShortName(ByVal v_sShortname As String, ByRef r_sName As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetPartyNameFromShortName(v_sPartyShortname:=v_sShortname, r_sPartyName:=r_sName)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get name from shortname", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAssociateNameFromShortName")

                Return result
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAssociateNameFromShortName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'SD 14/08/2002
    ' ***************************************************************** '
    ' Name: StoreSuppressedDocumentList
    '
    ' Description: Puts the document list in the database. For a new
    ' party, the count is passed in from business
    '
    ' ***************************************************************** '
    Public Function StoreSuppressedDocumentList(ByRef v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' PW150702 - store the list of suppressed documents
            m_lReturn = m_oBusiness.StoreSuppressedDocsList(lPartyCnt:=v_lPartyCnt, vSuppressedDocs:=m_vDocsSuppressed, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_SScreenHierarchy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Store Suppressed Documents", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreSuppressedDocumentList")
            End If

            Return result

        Catch excep As System.Exception

            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StoreSuppressedDocumentList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StoreSuppressedDocumentList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            If uctBranch.SelectedIndex = -1 Then
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
            ' get the SourceID
            lSourceId = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)


            If lSourceId <> 0 Then

                ' this value SHOULD exist, but more error trapping here?

                ' call the business. to get the Base Currency ID

                m_lReturn = oPartyBusiness.GetBaseCurrencyID(lSourceID:=lSourceId, iCurrencyID:=iBaseCurrencyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get base currency for selected branch", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")

                    Return result
                End If

                uctCurrency.CompanyId = lSourceId
                uctCurrency.RefreshList()
                uctCurrency.CurrencyId = iBaseCurrencyID
                PopulateBankAccount(iBaseCurrencyID)

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

        m_lReturn = m_oPMUser.GetUserSources(r_vSourceArray:=vSourceArray, v_vUserID:=g_iUserId, v_bIncludeDeletedSources:=False)

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
        'developer guide no.162
        For i As Integer = 0 To vSourceArray.GetUpperBound(1)
            'Add using branch description (3).
            uctBranch_NewIndex = uctBranch.Items.Add(CStr(vSourceArray(2, i)).Trim())
            VB6.SetItemData(uctBranch, uctBranch_NewIndex, CInt(vSourceArray(0, i)))

            If CInt(vSourceArray(0, i)) = m_vBranch Then
                uctBranch.SelectedIndex = uctBranch_NewIndex
            End If
        Next i

        Return result



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the branch details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function
    '************************************************************************************************
    'display broker transfer details
    '************************************************************************************************
    Private Sub BrokerTransferToInterface()

        Dim vResult As String = ""

        Try

            chkBrokerInTransferMode.CheckState = m_iIsInTransferMode

            If m_iIsInTransferMode = CheckState.Checked Then
                For lCount As Integer = 0 To cboBrokerTransferBusinessType.Items.Count - 1
                    If VB6.GetItemData(cboBrokerTransferBusinessType, lCount) = m_lTransferToBusinessTypeID Then
                        cboBrokerTransferBusinessType.SelectedIndex = lCount
                        Exit For
                    End If
                Next lCount

                'if its not direct business then get agent code
                If m_lTransferToBusinessTypeID <> 1 Then


                    m_lReturn = m_oBusiness.GetValueFromTable(v_sTableName:="Party", v_vReturnColumn:="shortname", v_sKeyColumn:="party_cnt", v_sKeyValue:=m_lTransferToPartyCnt, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResult)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to get transfer to agent code", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Exit Sub
                    End If

                    txtBrokerTransferToCode.Text = gPMFunctions.ToSafeString(vResult, "")
                    txtBrokerTransferToCode.Tag = CStr(m_lTransferToPartyCnt)
                End If
            End If



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display broker transfer details", vApp:=ACApp, vClass:=ACClass, vMethod:="BrokerTransferToInterface()", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

        End Try
        Exit Sub
    End Sub

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


                m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailDomiciledForTax, 0), 0)


                m_bTaxExempt = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailTaxExempt, 0), 0)

                m_dTaxPercentage = gPMFunctions.ToSafeDouble(vPartyDetails(kPartyDetailTaxPercentage, 0), 0)

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
            Else
                ''Agent Filtering
                SetPickListPKs()
                uctPickListBranches.Save()
                uctPickListProducts.Save()
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

    Private Sub uctCurrency_Click() Handles uctCurrency.Click
        If ToSafeLong(uctCurrency.CurrencyId) <> 0 Then
            lblBankAccount.Visible = True
            cboBankAccount.Visible = True
            PopulateBankAccount(ToSafeLong(uctCurrency.CurrencyId))

        Else
            lblBankAccount.Visible = False
            cboBankAccount.Visible = False
            PopulateBankAccount(ToSafeLong(uctCurrency.CurrencyId))
        End If
    End Sub

    'Private Sub uctPartyBankControl1_RefreshBankDetails(ByRef vBankDetails( ,  ) As Object)
    'm_vPartyBankDetails = vBankDetails
    'End Sub

    Private Sub uctPartyTax1_IsDomiciledForTaxChanged(ByVal Sender As Object, ByVal e As EventArgs) Handles uctPartyTax1.IsDomiciledForTaxChanged
        m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(Me.uctPartyTax1.IsDomiciledForTax)
    End Sub


    'Float Balance and Pre-Payment (RC) --------------------- start

    Private Sub chkPrepaymentAccount_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkPrepaymentAccount.CheckStateChanged

        If chkPrepaymentAccount.CheckState = CheckState.Checked Then
            chkStandardAccount.CheckState = CheckState.Unchecked
            chkStandardAccount.Enabled = False
        Else
            chkStandardAccount.Enabled = True
        End If

    End Sub

    Private Sub chkFloatBalanceAccount_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkFloatBalanceAccount.CheckStateChanged

        If chkFloatBalanceAccount.CheckState = CheckState.Checked Then
            chkStandardAccount.CheckState = CheckState.Unchecked
            chkStandardAccount.Enabled = False

            txtFloatBalanceLimit.Enabled = True
            txtExpectedDailyPremium.Enabled = True
            txtDaysAllowed.Enabled = True
            txtFloatBalanceLimit.BackColor = Color.White
            txtExpectedDailyPremium.BackColor = Color.White
            txtDaysAllowed.BackColor = Color.White
        Else
            chkStandardAccount.Enabled = True

            txtFloatBalanceLimit.Enabled = False
            txtExpectedDailyPremium.Enabled = False
            txtDaysAllowed.Enabled = False
            txtFloatBalanceLimit.BackColor = Color.Silver
            txtExpectedDailyPremium.BackColor = Color.Silver
            txtDaysAllowed.BackColor = Color.Silver
        End If

    End Sub

    Private Sub chkOverdraftAccount_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOverdraftAccount.CheckStateChanged

        If chkOverdraftAccount.CheckState = CheckState.Checked Then
            chkStandardAccount.CheckState = CheckState.Unchecked
            chkStandardAccount.Enabled = False

            txtOverdraftLimit.Enabled = True
            txtOverdraftExpiry.Enabled = True
            txtOverdraftLimit.BackColor = Color.White
            txtOverdraftExpiry.BackColor = Color.White
        Else
            chkStandardAccount.Enabled = True

            txtOverdraftLimit.Enabled = False
            txtOverdraftExpiry.Enabled = False
            txtOverdraftLimit.BackColor = Color.Silver
            txtOverdraftExpiry.BackColor = Color.Silver
        End If

    End Sub

    Private Sub chkStandardAccount_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkStandardAccount.CheckStateChanged

        If chkStandardAccount.CheckState = CheckState.Checked Then
            chkFloatBalanceAccount.CheckState = CheckState.Unchecked
            chkFloatBalanceAccount.Enabled = False
            chkPrepaymentAccount.CheckState = CheckState.Unchecked
            chkPrepaymentAccount.Enabled = False
            chkOverdraftAccount.CheckState = CheckState.Unchecked
            chkOverdraftAccount.Enabled = False
            txtOverdraftExpiry.Text = ""
        Else
            chkFloatBalanceAccount.Enabled = True
            chkPrepaymentAccount.Enabled = True
            chkOverdraftAccount.Enabled = True
        End If

    End Sub

    Private Sub txtDaysAllowed_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDaysAllowed.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CalcFloatBalance()
    End Sub

    Private Sub txtExpectedDailyPremium_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpectedDailyPremium.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        CalcFloatBalance()
    End Sub

    Sub CalcFloatBalance()
        txtFloatBalanceLimit.Text = StringsHelper.Format(gPMFunctions.ToSafeCurrency(txtExpectedDailyPremium.Text) * gPMFunctions.ToSafeCurrency(txtDaysAllowed.Text), "Fixed")
    End Sub

    Private Sub txtOverdraftLimit_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverdraftLimit.Enter
        SendKeys.Send("{Home}+{End}")
    End Sub

    Private Sub txtOverdraftExpiry_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverdraftExpiry.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtOverdraftExpiry)
    End Sub

    Private Sub txtOverdraftExpiry_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtOverdraftExpiry.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtOverdraftExpiry)
    End Sub

    Private Sub txtDaysAllowed_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDaysAllowed.Enter
        SendKeys.Send("{Home}+{End}")
    End Sub

    Private Sub txtExpectedDailyPremium_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtExpectedDailyPremium.Enter
        SendKeys.Send("{Home}+{End}")
    End Sub
    'Changes for WPR-42
    Private Sub uctPickListBranches_Change(ByVal Sender As Object, ByVal e As uctPickList.PickList.ChangeEventArgs) Handles uctPickListBranches.Change
        If e.IsEmpty = False Then
            If e.Action = uctPickList.PickList.ChangeAction.Delete Or e.Action = uctPickList.PickList.ChangeAction.DeleteAll Then
                e.Cancel = IIf(MsgBox("You are attempting to remove Branch Access for this Agent. Confirmation of this action will result in all System Users assigned to this Agent losing access to the same System Branch. Do you wish to proceed?", MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Warning") = MsgBoxResult.Yes, False, True)
            End If
        End If
    End Sub
    'End Changes for WPR-42

    'Float Balance and Pre-Payment (RC) --------------------- end

    Private Sub uctPickListBranches_Validating(ByVal eventSender As Object, ByVal eventArgs As CancelEventArgs) Handles uctPickListBranches.Validating
        Dim Cancel As Boolean = eventArgs.Cancel
        m_bChanged = True
        eventArgs.Cancel = Cancel
    End Sub
    Private Sub SetPickListPKs()
        uctPickListBranches.ForeignKeys.Item("Party_Cnt").Value = m_lPartyCnt
        uctPickListBranches.ForeignKeys.Item("Branchid").Value = 1

        uctPickListBranches.ForeignKeys.Item("user_id").Value = g_oObjectManager.UserID

        uctPickListBranches.ForeignKeys.Item("unique_id").Value = m_sUniqueId

        uctPickListBranches.ForeignKeys.Item("screen_hierarchy").Value = m_SScreenHierarchy

        uctPickListProducts.ForeignKeys.Item("Party_Cnt").Value = m_lPartyCnt

        uctPickListProducts.ForeignKeys.Item("user_id").Value = g_oObjectManager.UserID

        uctPickListProducts.ForeignKeys.Item("unique_id").Value = m_sUniqueId

        uctPickListProducts.ForeignKeys.Item("screen_hierarchy").Value = m_SScreenHierarchy

    End Sub

    '(RC) PLICO 9-10
    Sub SetCommisionRelease()

        Try

            Dim sReturn As String = ""
            Dim lCommissionPostingType As Integer

            'get system option "Agent Commission Suspended Postings" (5037)
            m_lReturn = iPMFunc.GetSystemOption(5037, sReturn, g_iSourceID)

            'if system option "Agent Commission Suspended Postings" (5037) is switched on
            'and Agent Type is "Sub-Agent" or "CommissionAccount"
            If gPMFunctions.ToSafeBoolean(gPMFunctions.ToSafeInteger(sReturn), False) And (m_lPartyAgentTypeID = PMBConst.PMBAgentTypeCommAccount Or m_lPartyAgentTypeID = PMBConst.PMBAgentTypeSubAgent Or cmbAgentType.Text = PMBConst.PMBAgentTypeSubAgentText Or cmbAgentType.Text = PMBConst.PMBAgentTypeCommAccountText Or uctAgentType.ItemCaption = PMBConst.PMBAgentTypeSubAgentText Or uctAgentType.ItemCaption = PMBConst.PMBAgentTypeCommAccountText) Then

                fraCommissionRelease.Visible = True

                If Information.IsArray(m_vMakeLiveArray) Then
                    lCommissionPostingType = CInt(m_vMakeLiveArray(AC_PARTYAG_CommissionPostingType))
                    If lCommissionPostingType > 0 Then
                        For i As Integer = 0 To cboCommissionPostingType.ListCount - 1
                            If cboCommissionPostingType.ItemData(i) = lCommissionPostingType Then
                                cboCommissionPostingType.ListIndex = i
                                Exit For
                            End If
                        Next
                    End If
                End If

                m_oFormFields.Item("cboCommissionPostingType-0").IsMandatory = True
                'm_oFormFields.Item("cboCommissionPostingType").IsMandatory = True

            Else

                m_oFormFields.Item("cboCommissionPostingType-0").IsMandatory = False
                'm_oFormFields.Item("cboCommissionPostingType").IsMandatory = False

                fraCommissionRelease.Visible = False
                For i As Integer = 0 To cboCommissionPostingType.ListCount - 1
                    If cboCommissionPostingType.ItemData(i) = 1 Then
                        cboCommissionPostingType.ListIndex = i
                        Exit For
                    End If
                Next
            End If

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=CStr(Information.Err().Number) & " - " & excep.Message, vApp:=ACApp, vClass:=ACClass, vMethod:="SetCommisionRelease", excep:=excep)

            Exit Sub

        End Try

    End Sub

    Public Function EnableForm(ByRef Level As EnabledLevel) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EnableForm"
        Try


            Dim bEnabled As Boolean

            bEnabled = (Level = EnabledLevel.elEnabled)

            With Me


                For Each oControl As Control In ContainerHelper.Controls(Me)
                    If (TypeOf oControl Is TextBox) Or (TypeOf oControl Is ComboBox) Or (TypeOf oControl Is CheckBox) Or (TypeOf oControl Is ListView) Or (TypeOf oControl Is UserControls.AccountLookup) Or (TypeOf oControl Is PMLookupControl.cboPMLookup) Or (TypeOf oControl Is UserControls.CurrencyLookup) Or (TypeOf oControl Is PMListMgrDropdown.uctDropdown) Or (TypeOf oControl Is uctPickList.PickList) Or (TypeOf oControl Is GroupBox) Then
                        ControlHelper.SetEnabled(oControl, bEnabled)
                    ElseIf TypeOf oControl Is uctPartyTaxControl.uctPartyTax Then
                        CType(oControl, uctPartyTaxControl.uctPartyTax).ReadOnly_Renamed = Not bEnabled
                    End If
                Next oControl

                .cmdAdd.Enabled = bEnabled

                .cmdAddAd.Enabled = bEnabled
                .cmdAddAll.Enabled = bEnabled
                .cmdRemove.Enabled = bEnabled
                .cmdRemoveAll.Enabled = bEnabled

                .cmdAddCon.Enabled = bEnabled

                .cmdDeleteAd.Enabled = bEnabled
                .cmdDeleteCon.Enabled = bEnabled
                .cmdEditAd.Enabled = bEnabled
                .cmdEditCon.Enabled = bEnabled

                If Level = EnabledLevel.elDisabledTotal Then

                    .cmdRates.Enabled = False
                    .cmdMaintain.Enabled = False
                    .cmdMaintainAssociates.Enabled = False
                    .cmdAgentGroupLookup.Enabled = False
                    .cmdBrokerTransferTo.Enabled = False
                    .cmdConsultantLookup.Enabled = False

                    .mnuFind.Enabled = False
                    .mnuRelatedDocuments.Enabled = False

                    .Toolbar1.Enabled = False
                    .uctPartyBankControl1.ReadOnly_Renamed = True

                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception


            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetClientCodeCntl
    '
    ' Description: Enable/Disable Client code and set global variables
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
        Dim r_sMaskCode As String = ""

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If oClientNumber Is Nothing Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Raise Error
                    gPMFunctions.RaiseError("SetClientCodeCntl", "Can not create object of bSIRPolicyNumMaint.Business")
                    Return result
                End If
            End If


            m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:=gSIRLibrary.SIRPartyTypeAgent, r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Raise Error.
                gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails Failed")
                Return result
            End If

            m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
            m_bIsReadOnly = r_bIsReadOnly
            m_sMaskCode = r_sMaskCode

            If r_bIsNumberingSchemeExists And r_bIsReadOnly Then
                lblIDReference.Enabled = False
                txtIDReference.Enabled = False
                cmdApply.Enabled = True
            ElseIf r_bIsNumberingSchemeExists And Not r_bIsReadOnly Then
                lblIDReference.Enabled = True
                txtIDReference.Enabled = True
                cmdApply.Enabled = True
            Else
                lblIDReference.Enabled = True
                txtIDReference.Enabled = True
            End If



        Catch ex As Exception

            ' Log Error
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function GeneratePartyCode() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object

        Const kMethodName As String = "GeneratePartyCode"
        Dim sFailureReason, sGeneratedClientCode, sTradingName, sAgentType As String

        Dim oClientNumber As bSIRPolicyNumMaint.Business
        Dim iBranchId As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsSetMaskingCode And txtIDReference.Text = "" Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Raise Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "Unable to get instance of  bSIRPolicyNumMaint.Business")
                    Return result
                End If

                sTradingName = TradeName
                sGeneratedClientCode = TradeName
                sAgentType = AgentType
                iBranchId = BranchId


                m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:=gSIRLibrary.SIRPartyTypeAgent, v_iSourceID:=iBranchId, r_sGeneratedClientCode:=sGeneratedClientCode, r_sFailureReason:=sFailureReason, v_sTradeName:=sTradingName, v_sType:=sAgentType, v_sValue:=sTradingName)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "GenerateClientCode Failed ")
                    Return result
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then  'Numbering Scheme not set
                    MessageBox.Show("Numbering scheme for Agent Group is not set.", "Agent", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                ElseIf sFailureReason <> "" Then
                    MessageBox.Show(sFailureReason, "Agent", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
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

    Private Function ValidateNumberingScheme() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateNumberingScheme"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sMaskCode <> "" Then
                ' Branch
                If m_sMaskCode.IndexOf("B"c) >= 0 Then
                    If uctBranch.SelectedIndex < 0 Or BranchId < 1 Then
                        MessageBox.Show("Please select some Branch", "field - Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                ' Last Name, First Name
                If (m_sMaskCode.IndexOf("L"c) >= 0) Or (m_sMaskCode.IndexOf("F"c) >= 0) Or (m_sMaskCode.IndexOf("N"c) >= 0) Or (m_sMaskCode.IndexOf("I"c) >= 0) Or (m_sMaskCode.IndexOf("T"c) >= 0) Then
                    If txtName.Text = "" Then
                        MessageBox.Show("Please Enter Name", "field - Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                ' Party Type
                If m_sMaskCode.IndexOf("A"c) >= 0 Then
                    If uctAgentType.ListIndex < 0 Or uctAgentType.ItemData(uctAgentType.ListIndex) < 1 Then
                        MessageBox.Show("Please select some Agent Type", "field - Agent Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
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

    Private Sub PopulateAgencyUsers()

        Dim i As Short
        Dim oListItem As ListViewItem
        Dim sUserNames As String

        Try

            If (IsArray(m_vAgencyUsers) = False) Then
                Exit Sub
            End If

            lvwAgencyUsers.Items.Clear()

            ' Assign the details to the interface.
            For i = LBound(m_vAgencyUsers, 2) To UBound(m_vAgencyUsers, 2)


                ' Assign the details to the Usernames column.
                ' Column 1 - Usernames (Code)
                'UPGRADE_WARNING: Couldn't resolve default property of object m_vAgencyUsers(). Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                sUserNames = Trim(m_vAgencyUsers(1, i))
                oListItem = lvwAgencyUsers.Items.Add(sUserNames)

            Next i
            '    'Populate the cells
            ' m_lReturn& = ListViewAutoSize(lvwList:=lvwAgencyUsers, bSizeHeaders:=True)
            Exit Sub

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAgencyUsers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub ChooseProducts()
        Dim sReturn As String
        Try
            'get system option "Agent and Agent User Product Link " (5071)
            m_lReturn = iPMFunc.GetSystemOption(5088, sReturn, g_iSourceID)
            If Not (sReturn = "") Then
                If CDbl(sReturn) = 1 Then
                    'Products Tab - Unavailable
                    SSTabHelper.SetTabVisible(tabMainTab, AC_TAB_PAG_PRODUCTS, True)
                    'tabMainTab.TabPages.Item(AC_TAB_PAG_USERS).Text = "&11 - Users"
                    If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                        '  uctPickListProducts.clear
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMView Or sReturn = "" Then
                        uctPickListProducts.Enabled = False
                    ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                        'Do Nothing
                    End If
                Else
                    'Products Tab - Unavailable
                    SSTabHelper.SetTabVisible(tabMainTab, AC_TAB_PAG_PRODUCTS, False)
                    SSTabHelper.SetTabCaption(tabMainTab, AC_TAB_PAG_USERS, "10-Users")
                End If
            Else
                'Products Tab - Unavailable
                SSTabHelper.SetTabVisible(tabMainTab, AC_TAB_PAG_PRODUCTS, False)
                SSTabHelper.SetTabCaption(tabMainTab, AC_TAB_PAG_USERS, "10-Users")
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                '  Do Nothing
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMView Or sReturn = "" Then
                'UPGRADE_WARNING: Couldn't resolve default property of object m_oBusiness.GetAgencyUsers. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = m_oBusiness.GetAgencyUsers(v_lPartyCnt:=m_lPartyCnt, r_vResults:=m_vAgencyUsers)
                If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                    PopulateAgencyUsers()
                End If
            ElseIf m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                'UPGRADE_WARNING: Couldn't resolve default property of object m_oBusiness.GetAgencyUsers. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                m_lReturn = m_oBusiness.GetAgencyUsers(v_lPartyCnt:=m_lPartyCnt, r_vResults:=m_vAgencyUsers)
                If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) Then
                    PopulateAgencyUsers()
                End If
            End If
            Exit Sub

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="ChooseProducts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try
    End Sub

    Private Sub Frame2_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Frame2.Enter

    End Sub


    Private Sub cmdEditCertYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditCertYear.Click
        Dim oListItem As ListViewItem
        Dim oCertYear As frmCertificateYear
        Dim vLvwArray As Object(,) = Nothing
        Try

            'Set row to be deleted - if a valid one selected
            If lvwCertYears.Items.Count < 1 Then
                Exit Sub
            End If

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'Create address component if not already done so
            If oCertYear Is Nothing Then
                oCertYear = New frmCertificateYear()
            End If
            CreatelvwCertYearCollection(vLvwArray)

            If vLvwArray IsNot Nothing AndAlso IsArray(vLvwArray) Then
                oCertYear.LvwArray = vLvwArray
            End If

            oListItem = lvwCertYears.FocusedItem
            oCertYear.Code = oListItem.SubItems(0).Text.ToString
            oCertYear.Description = oListItem.SubItems(1).Text.ToString
            oCertYear.StartDate = oListItem.SubItems(2).Text.ToString
            oCertYear.EndDate = oListItem.SubItems(3).Text.ToString
            oCertYear.Task = gPMConstants.PMEComponentAction.PMEdit

            m_lReturn = oCertYear.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the mouse's pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Show it
            oCertYear.ShowDialog()

            ' Get the values
            If oCertYear.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' Warn about data changed when exiting
                m_bDataChanged = True
            End If
            'oListItem = lvwCertYears.Items.Add(oCertYear.Code)
            'If oCertYear.EndDate > oCertYear.StartDate Then
            ListViewHelper.GetListViewSubItem(oListItem, 0).Text = oCertYear.Code
            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = oCertYear.Description
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = oCertYear.StartDate
            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = oCertYear.EndDate
            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "0" 'Non Deleted
            ' End If

            If m_vCertificateYearDetails IsNot Nothing Then
                oListItem.Tag = CStr(m_vCertificateYearDetails.GetUpperBound(1))
            Else
                oListItem.Tag = CStr(oListItem.SubItems.Count)
            End If
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwCertYears, bSizeHeaders:=True)


        Catch excep As System.Exception



            'result = gPMConstants.PMEReturnCode.PMError

            ' Set the mouse's pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MaintainSuppressedDocs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MaintainSuppressedDocs", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'Return result

        End Try
    End Sub
    Private Sub PopulateCertYear()

        'Const ContactImage As String = "ContactImage"      ''Unused Local Variable
        Dim oListItem As ListViewItem


        Try

            If Not Information.IsArray(m_vCertificateYearDetails) Then
                Exit Sub
            End If

            lvwCertYears.Items.Clear()

            ' Assign the details to the interface.
            For i As Integer = m_vCertificateYearDetails.GetLowerBound(1) To m_vCertificateYearDetails.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Column 1
                oListItem = lvwCertYears.Items.Add(CStr(m_vCertificateYearDetails(0, i)).Trim())

                ' Assign details to other the columns
                ' Column 2
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vCertificateYearDetails(1, i)).Trim()

                ' Column 3
                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CDate(CStr(m_vCertificateYearDetails(2, i)).Trim())

                ' Column 4
                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CDate(CStr(m_vCertificateYearDetails(3, i)).Trim())
                ' Column 4)
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vCertificateYearDetails(4, i)).Trim()

                ' Store the Contact_cnt
                If m_vCertificateYearDetails IsNot Nothing AndAlso IsArray(m_vCertificateYearDetails) Then
                    oListItem.Tag = CStr(m_vCertificateYearDetails(0, i)).Trim()
                End If
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i

            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwCertYears, bSizeHeaders:=True)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateContacts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddCertYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddCertYear.Click

        Dim oListItem As ListViewItem
        Dim oCertYear As frmCertificateYear
        Dim vLvwArray As Object(,) = Nothing
        Try

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            CreatelvwCertYearCollection(vLvwArray)

            'Create address component if not already done so
            If oCertYear Is Nothing Then
                oCertYear = New frmCertificateYear()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            oCertYear.Code = ""
            oCertYear.Description = ""
            oCertYear.Task = gPMConstants.PMEComponentAction.PMAdd
            oCertYear.Status = gPMConstants.PMEReturnCode.PMCancel
            If vLvwArray IsNot Nothing AndAlso IsArray(vLvwArray) Then
                oCertYear.LvwArray = vLvwArray
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Reset the moust pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = oCertYear.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the mouse's pointer
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            End If

            ' Show it
            oCertYear.ShowDialog()

            ' Get the values
            If oCertYear.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                ' Warn about data changed when exiting
                m_bDataChanged = True

            ElseIf oCertYear.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()

            oListItem = lvwCertYears.Items.Add(oCertYear.Code)


            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = oCertYear.Description

            ' Column 3

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = oCertYear.StartDate

            ' Column 4

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = oCertYear.EndDate

            ' Column 5

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "0" 'Non Deleted

            ' Store the Contact tag
            If m_vCertificateYearDetails IsNot Nothing Then
                oListItem.Tag = CStr(m_vCertificateYearDetails.GetUpperBound(1))
            End If
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwCertYears, bSizeHeaders:=True)

        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddCertYear_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddCertYear_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Function UpdateCertYear() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim vUpdatecertYear As Object
        Dim bFirst As Boolean
        Dim i As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            i = 1
            bFirst = True

            Do
                If i > lvwCertYears.Items.Count Then
                    Exit Do
                End If
                oListItem = lvwCertYears.Items.Item(i - 1)
                If ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim() = "" Then
                    Exit Do
                Else
                    If bFirst Then
                        ReDim vUpdatecertYear(4, i - 1)
                        bFirst = False
                    Else
                        ReDim Preserve vUpdatecertYear(4, i - 1)
                    End If

                    vUpdatecertYear(0, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 0).Text.Trim()
                    vUpdatecertYear(1, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
                    vUpdatecertYear(2, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text.Trim()
                    vUpdatecertYear(3, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text.Trim()
                    vUpdatecertYear(4, i - 1) = ListViewHelper.GetListViewSubItem(oListItem, 4).Text.Trim()
                End If
                i += 1
            Loop

            'Delete old contact usages in database
            If Information.IsArray(vUpdatecertYear) Then
                m_lReturn = m_oBusiness.UpdateCertYear(vPartyCnt:=m_lPartyCnt, vUpdatecertYear:=vUpdatecertYear)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCertYearFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCertYear", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdDelCertYear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDelCertYear.Click
        Dim oListItem As ListViewItem
        Try

            'Set row to be deleted - if a valid one selected
            If lvwCertYears.Items.Count < 1 Then
                Exit Sub
            End If
            oListItem = lvwCertYears.FocusedItem()
            If cmdDelCertYear.Text = "UnDelete" Then
                oListItem.BackColor = Color.White
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "0" 'UnDeleted
                cmdDelCertYear.Text = "Delete"
                cmdEditCertYear.Enabled = True
            ElseIf cmdDelCertYear.Text = "Delete" Then
                oListItem.BackColor = Color.LightGray
                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = "1" 'Deleted
                cmdDelCertYear.Text = "UnDelete"
                cmdEditCertYear.Enabled = False
            End If

            lvwCertYears.Focus()

        Catch excep As System.Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDelCertYear_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelCertYear_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Exit Sub

        End Try
    End Sub
    Private Sub lvwCertYears_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwCertYears.Click
        If Not (lvwCertYears.FocusedItem Is Nothing) Then
            cmdDelCertYear.Enabled = True
            cmdEditCertYear.Enabled = True
            enableDisableCertYearButton()
        Else
            cmdDelCertYear.Enabled = False
            cmdEditCertYear.Enabled = False
        End If

    End Sub

    Private Sub lvwCertYears_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwCertYears.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        lvwCertYears_Click(lvwCertYears, New EventArgs())
    End Sub
    Private Sub enableDisableCertYearButton()
        Dim oListItem As New ListViewItem
        oListItem = lvwCertYears.FocusedItem
        If oListItem.SubItems(4).Text.ToString = "1" Then
            cmdDelCertYear.Text = "UnDelete"
            cmdEditCertYear.Enabled = False
        ElseIf oListItem.SubItems(4).Text.ToString = "0" Then
            cmdDelCertYear.Text = "Delete"
            cmdEditCertYear.Enabled = True
        End If
    End Sub
    Private Sub CreatelvwCertYearCollection(ByRef vArray As Object(,))
        Dim ilvwItemsCount As Integer = 0
        Dim oListItem As New ListViewItem
        ilvwItemsCount = lvwCertYears.Items.Count
        ReDim vArray(5, lvwCertYears.Items.Count)
        For iCnt As Integer = 0 To lvwCertYears.Items.Count - 1
            oListItem = lvwCertYears.Items.Item(iCnt)
            vArray(0, iCnt) = ListViewHelper.GetListViewSubItem(oListItem, 0).Text.Trim()
            vArray(1, iCnt) = ListViewHelper.GetListViewSubItem(oListItem, 1).Text.Trim()
            vArray(2, iCnt) = ListViewHelper.GetListViewSubItem(oListItem, 2).Text.Trim()
            vArray(3, iCnt) = ListViewHelper.GetListViewSubItem(oListItem, 3).Text.Trim()
            vArray(4, iCnt) = ListViewHelper.GetListViewSubItem(oListItem, 4).Text.Trim()
        Next

    End Sub
    Private Function PopulateBankAccount(ByVal iCurrencyId As Integer) As Integer

        Const kMethodName As String = "PopulateBankAccount"
        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            cboBankAccount.WhereClause = "currency_id=" & CDec(iCurrencyId)
            cboBankAccount.FirstItem = "(Any)"

            For i As Integer = 0 To cboBankAccount.ListCount - 1
                If cboBankAccount.ItemData(i) = m_iBankAccount Then
                    cboBankAccount.ListIndex = i
                    Exit For
                End If
            Next
            Return result
        Catch ex As System.Exception
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        End Try


    End Function

    Private Sub cmdMaintainCommLevel_Click(sender As Object, e As EventArgs) Handles cmdMaintainCommLevel.Click
        Dim vTask As gPMConstants.PMEComponentAction

        Try
            'Create shares object if not already done so
            If m_oCommissionLevel Is Nothing Then

                ' Get an instance of the associates interface object via
                ' the public object manager.
                Dim temp_m_oCommLevel As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oCommLevel, sClassName:="iPMBCommissionLevel.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                m_oCommissionLevel = temp_m_oCommLevel

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get commission level object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMaintainCommLevel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            vTask = gPMConstants.PMEComponentAction.PMEdit

            m_lReturn = m_oCommissionLevel.SetProcessModes(vTask:=vTask)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If



            m_oCommissionLevel.SearchData = m_vCommissionLevel


            m_lReturn = m_oCommissionLevel.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oCommissionLevel.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()

            m_vCommissionLevel = Nothing

            m_vCommissionLevel = m_oCommissionLevel.SearchData

            PopulateCommissionLevel()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdMaintainCommLevel_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdMaintainCommLevel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub PopulateCommissionLevel()

        Dim oListItem As ListViewItem
        Dim sComLevelDescription As String

        Try

            If Not Information.IsArray(m_vCommissionLevel) Then
                Exit Sub
            End If

            ListView1.Items.Clear()

            ' Assign the details to the interface.

            For i As Integer = m_vCommissionLevel.GetLowerBound(1) To m_vCommissionLevel.GetUpperBound(1)


                If CStr(m_vCommissionLevel(1, i)) <> "" Then

                    ' {* USER DEFINED CODE (Begin) *}

                    ' Assign the details to the first column.
                    ' Column 1 - Commission Level Description

                    sComLevelDescription = CStr(m_vCommissionLevel(1, i)).Trim()
                    oListItem = ListView1.Items.Add(sComLevelDescription)

                    ' Column 2 - Effective Date

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(gPMFunctions.ToSafeDate(m_vCommissionLevel(2, i)))


                    oListItem.Tag = CStr(m_vCommissionLevel(0, i)).Trim()
                    ' {* USER DEFINED CODE (End) *}

                    ' Set the tag property with the index of
                    ' the search data storage.

                End If

            Next i
            '    'Populate the cells

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateCommissionLevel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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
