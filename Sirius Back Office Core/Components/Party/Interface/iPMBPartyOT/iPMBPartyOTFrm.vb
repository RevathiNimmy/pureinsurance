Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 02/07/1998
    '
    ' Description: Main interface.
    '
    ' Edit History:
    '   SP161198  - Update Address User control (uses QAS now) and remove
    '   terminate call which was a temporary work around an old bug.
    '   CJB090205 - PN18666 Changed SetInterfaceDefaults to NOT show Branch
    '               or SubBranch combos in Broking.
    ' ***************************************************************** '
    'Developer Guide No.243 (changed iPMFunc.GetResData to GetResData in the whole document)

    'Developer Guide No. 69
    Public frmAccidents As frmAccidents

    'Developer Guide No.7
    Public Const vbFormCode As Integer = 0
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"


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
    Private m_iLine As Integer

    Private m_iDefaultCountryID As Integer
    Private m_sDefaultCountryCode As String = ""

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}

    ' Constants for internal return flags
    Const ACAddressAdd As Integer = 1
    Const ACAddressDoNotAdd As Integer = 2
    Const ACAddressCancel As Integer = 3
    Const ACAddressEdit As Integer = 4

    'Constants for Tabs
    Private Const ACTAB_GENERAL As Integer = 0
    Private Const ACTAB_ADDRESS As Integer = 1
    Private Const ACTAB_SUPPLY As Integer = 2
    Private Const ACTAB_CONVICTIONS As Integer = 3
    Private Const ACTAB_ACCIDENTS As Integer = 4
    Private Const ACTAB_TAX As Integer = 5
    Private Const ACTAB_INSURER As Integer = 6
    'Party Bank Details
    Private Const ACTAB_PartyBank As Integer = 7

    Private Const ACTAB_INDEXLOW As Integer = 0
    'Party Bank Details
    Private Const ACTAB_INDEXHIGH As Integer = 7

    Private m_lTabFirst As Integer
    Private m_lTabLast As Integer

    ' Declare an instance of the address interface.

    Private m_oAddress As iPMBAddress.Interface_Renamed

    Private m_lCurrencyID As Integer
    Private m_lPartyCnt As Integer
    Private m_lAddressCnt As Integer
    Private m_lBusinessId As Integer
    Private m_vResultArray As Object
    Private m_vAddresses As Object
    Private m_vSpecialityArray As Object
    Private m_iCurrentBusIndex As Integer
    Private m_lAddressCount As Integer
    Private m_vAddressTypes As Object
    Private m_sPartyName As String = ""
    Private m_sPartyTypeCode As String = ""
    Private m_sPartyTypeDesc As String = ""
    Private m_sPartyCode As String = ""
    Private m_sMainPostCode As String = ""
    Private m_iMainAddressIndex As Integer
    Private m_sAddressLine1 As String = ""
    Private m_sMandatoryFieldsTitle As String = ""
    Private m_vDateOfBirth As Object
    Private m_vGender As String = ""
    Private m_vLicenceNumber As Object
    'Developer Guide No.(As per VB Code)
    Private m_vLicenceType As Object
    Private m_vStatus As Object
    Private m_vRegNumber As Object
    Private m_vConvictions As Object
    Private m_vAccidents As Object

    'S4B Claims Enhancements R&D 2005
    Private m_vReferenceNumber As Object
    Private m_vDatePassedTest As Object
    Private m_vContactName As Object
    Private m_vContactTelephoneNumber As Object
    Private m_vInsurerName As Object
    Private m_vInsurerAddress1 As Object
    Private m_vInsurerAddress2 As Object
    Private m_vInsurerAddress3 As Object
    Private m_vInsurerAddress4 As Object
    Private m_vInsurerPostcode As Object
    Private m_vInsurerTelephoneNumber As Object
    Private m_vInsurerFaxNumber As Object
    Private m_vInsurerContactName As Object
    Private m_vInsurerEmail As Object
    Private m_vInsurerNotes As Object
    Private m_vCompanyNotes As Object


    'SD 17/09/2002 Add Supplier party type
    Private m_bIsSupplier As Boolean
    Private m_bIsThirdParty As Boolean
    Private m_bIsIndividualPartyType As Boolean
    'Developer Guide No.(As per VB Code)
    Private m_vPartyDetail As Object

    ' RDC 22042004
    'Developer Guide No.(As per VB Code)
    Private m_vBranch As Object
    Private m_vSubBranch As Object

    ' {* USER DEFINED CODE (End) *}

    ' Declare an instance of the conviction interface.

    Private m_oConviction As iPMBPartyConviction.Interface_Renamed
    'Private m_oConviction As iPMBPartyConviction.Interface

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMBPartyOT.General

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

    'sj 08/11/2002 - start
    Private m_bCreateOrionAccount As Boolean
    'sj 08/11/2002 - end

    'sj 11/11/2002 - start
    Private m_vPartyOtherPostingType As Boolean
    'sj 11/11/2002 - end
    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    Private m_bSetBranch As Boolean
    Private m_bShowSubBranchID As Boolean
    Private m_sUnderwritingOrAgency As String = ""

    'RDT PN18099

    Private m_oPMUser As bPMUser.Business

    '**************************************************
    Private m_sTaxNumber As String = ""
    Private m_bDomiciledForTax As Boolean
    Private m_bTaxExempt As Boolean
    Private m_dTaxPercentage As Double
    '**************************************************
    'PN 19428
    Private m_vSourceArray(,) As Object

    'Maintain Party Code
    Private m_bIsSetMaskingCode As Boolean
    Private m_bIsReadOnly As Boolean
    Private m_sMaskCode As String = ""

    'Party Bank Details
    Private m_vPartyBankDetails(,) As Object
    Private m_vPartyBankHistory As Object
    Private m_bIsViewOnlyOTMaintenance As Boolean
    'declared to store the source id
    'start
    Private m_iSourceID As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    Public WriteOnly Property SourceID() As Integer
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property
    'end


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


    'Private Sub Status(ByVal Value As Integer)
    '
    ' Standard Property.
    '
    ' Set the interface exit status.
    'm_lStatus = Value
    '
    'End Sub
    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
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

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public Property AddressCnt() As Integer
        Get

            ' Return the objects parameter value.
            Return m_lAddressCnt

        End Get
        Set(ByVal Value As Integer)

            ' Set the objects parameter value.
            m_lAddressCnt = Value

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
    'Public Property Get Address1() As String
    '
    '    ' Return the objects parameter value.
    '    Address1 = m_sAddress1$
    '
    'End Property
    'Public Property Get Address2() As String
    '
    '    ' Return the objects parameter value.
    '    Address2 = m_sAddress2$
    '
    'End Property
    'Public Property Get Address3() As String
    '
    '    ' Return the objects parameter value.
    '    Address3 = m_sAddress3$
    '
    'End Property
    'Public Property Get Address4() As String
    '
    '    ' Return the objects parameter value.
    '    Address4 = m_sAddress4$
    '
    'End Property
    'Public Property Get PostalCode() As String
    '
    '    ' Return the objects parameter value.
    '    PostalCode = m_sPostalCode$
    '
    'End Property
    '
    '' CF30499 - Address Usage Type
    'Public Property Let AddressUsageTypeID(lTypeID As Long)
    '    m_lAddressUsageTypeID& = lTypeID
    'End Property
    'Public Property Get AddressUsageTypeID() As Long
    '    AddressUsageTypeID = m_lAddressUsageTypeID&
    'End Property
    '
    'Public Property Let AddressUsageType(sType As String)
    '    m_sAddressUsageType = sType$
    'End Property
    'Public Property Get AddressUsageType() As String
    '    AddressUsageType = m_sAddressUsageType$
    'End Property

    Public Property ShortName() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
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

    Private ReadOnly Property OtherPartyName() As String
        Get

            Return gPMFunctions.ToSafeString(txtPartyName.Text.Replace(" ", "")).ToUpper()
        End Get
    End Property


    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property


    Public Property PartyTypeCode() As String
        Get
            Return m_sPartyTypeCode
        End Get
        Set(ByVal Value As String)
            m_sPartyTypeCode = Value

            'SD 17/09/2002 Add Supplier party type changes. Also set IsSupplier value
            If String.Compare(m_sPartyTypeCode, ACPartyTypeSupplier) = 0 Then
                m_bIsSupplier = True
                m_bIsThirdParty = False
            Else
                m_bIsSupplier = False
                m_bIsThirdParty = (m_sPartyTypeCode.Trim().ToUpper() = ACPartyTypeThirdParty)
            End If

        End Set
    End Property


    Public Property PartyCode() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
        End Set
    End Property


    Public Property PartyName() As String
        Get
            Return m_sPartyName
        End Get
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property



    Public Property DefaultCountryCode() As String
        Get
            Return m_sDefaultCountryCode
        End Get
        Set(ByVal Value As String)
            m_sDefaultCountryCode = Value
        End Set
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

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPartyCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPartyName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDateOfBirth, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtLicenceNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRegNumber, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCurrency, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            'S4B Claim Enhancements R&D 2005
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtReference, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCompanyNotes, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDatePassedTest, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtContactName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtContactTelephoneNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerTelNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerFaxNo, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerContactName, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerEmailAddress, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInsurerNotes, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

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


            m_lReturn = m_oBusiness.GetDetails(vPartyCnt:=m_lPartyCnt)

            '    m_lReturn& = m_oBusiness.GetPartySupplierBusiness(v_lPartyCnt:=m_lPartyCnt, _
            ''                                                        r_sPartyName:=m_sPartyName, _
            ''                                                        r_sPartyTypeDesc:=m_sPartyTypeDesc, _
            ''                                                        r_vResultArray:=m_vResultArray)

            ' {* USER DEFINED CODE (End) *}

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
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
    ' Name: GetDefaultTypeID
    '
    ' Description: Gets the ID for the correspondance address
    '
    ' ***************************************************************** '

    'Private Function GetDefaultTypeID(ByRef r_lID As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oLookup As bPMLookup.Business
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get an instance of the lookup object
    'Dim temp_oLookup As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oLookup, "bPMLookup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oLookup = temp_oLookup
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Set the product family
    'oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    '
    ' Get the ID for the default
    'm_lReturn = oLookup.GetEffectiveIDFromCode(v_sTableName:="address_usage_type", v_sCode:=gSIRLibrary.SIRMainAddressABICode, v_dtEffectiveDate:=DateTime.Now, r_lID:=r_lID)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Remove instance from memory
    'm_lReturn = oLookup.Terminate()
    'oLookup = Nothing
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultTypeID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


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

            'Display Party identifiers.
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtPartyCode, vControlValue:=m_sPartyCode)
            '    m_lReturn& = m_oFormFields.FormatControl(ctlControl:=txtPartyName, vControlValue:=m_sPartyName)
            txtPartyCode.Text = m_sPartyCode.Trim()
            txtPartyName.Text = m_sPartyName.Trim()
            Me.Text = m_sPartyTypeDesc.Trim()


            If Not (Convert.IsDBNull(m_vDateOfBirth) Or IsNothing(m_vDateOfBirth)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDateOfBirth, vControlValue:=m_vDateOfBirth)
            End If




            If Not (Convert.IsDBNull(m_vGender) Or IsNothing(m_vGender)) Then
                ddGender.Text = m_vGender
            End If


            If Not (Convert.IsDBNull(m_vLicenceType) Or IsNothing(m_vLicenceType)) Then
                For lCount As Integer = 0 To cboLicenceType.Items.Count - 1
                    If VB6.GetItemData(cboLicenceType, lCount) = m_vLicenceType Then
                        cboLicenceType.SelectedIndex = lCount
                        Exit For
                    End If
                Next lCount
            End If


            If Not (Convert.IsDBNull(m_vLicenceNumber) Or IsNothing(m_vLicenceNumber)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtLicenceNumber, vControlValue:=m_vLicenceNumber)
            End If


            If Not (Convert.IsDBNull(m_vStatus) Or IsNothing(m_vStatus)) Then
                For lCount As Integer = 0 To cboStatus.Items.Count - 1
                    If VB6.GetItemData(cboStatus, lCount) = m_vStatus Then
                        cboStatus.SelectedIndex = lCount
                        Exit For
                    End If
                Next lCount
            End If


            If Not (Convert.IsDBNull(m_vRegNumber) Or IsNothing(m_vRegNumber)) Then
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRegNumber, vControlValue:=m_vRegNumber)
            End If

            'S4B Claims Enhancements R&D 2005
            'Show Businesses already stored against this Party.
            m_lReturn = DisplayAssociatedBusinesses()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TN20000928 - display associated data in lvwSupSpecSelected if we have one in lvwSupBusSelected (Start)
            If lvwSupBusSelected.Items.Count > 0 Then
                m_lReturn = DisplayAssociatedSpecialities()

                ' Check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to assign the data.
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            'Fill address ListView
            PopulateAddresses()

            PopulateConvictions()

            PopulateAccidents()

            'SD 17/09/2002 Add Supplier party type extra details
            If m_bIsSupplier Then
                PopulateExtraSupplierDetails()
            Else
                If CStr(m_vPartyDetail(ACTPASettleDirectly)) = 1 Then
                    cboTPASettle.SelectedIndex = 1
                Else
                    cboTPASettle.SelectedIndex = 0
                End If
            End If

            ' RDC 23042004
            m_bSetBranch = True

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

            m_lReturn = GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_vSubBranch = 0 Then
                cboSubBranch.SelectedIndex = -1
            Else
                For lCount As Integer = 0 To cboSubBranch.Items.Count - 1
                    If VB6.GetItemData(cboSubBranch, lCount) = m_vSubBranch Then
                        cboSubBranch.SelectedIndex = lCount
                        Exit For
                    End If
                Next
            End If
            m_bSetBranch = False

            uctCurrency.CompanyId = m_vBranch
            uctCurrency.RefreshList()
            uctCurrency.CurrencyId = m_lCurrencyID

            'Party Bank Details
            LoadPartyBankControl()

            ' ************************************************************
            uctPartyTax1.TaxNumber = m_sTaxNumber
            uctPartyTax1.IsDomiciledForTax = m_bDomiciledForTax
            uctPartyTax1.TaxExempt = m_bTaxExempt
            uctPartyTax1.TaxPercentage = m_dTaxPercentage
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

            ' {* USER DEFINED CODE (End) *}

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

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the mouse pointer to an hour glass
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the business data ID to one because we are only
            ' dealing with one record item only.
            lBusinessDataID = 1
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If
            m_sScreenHierarchy = $"Other Party({txtPartyCode.Text.Trim()})"
            ' Check the task.
            Select Case (m_iTask)
                'SP090989
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'SD 17/09/2002 Add Supplier party type extra details


                    m_lReturn = m_oBusiness.EditAdd(lRow:=lBusinessDataID, vShortName:=m_sPartyCode, vName:=m_sPartyName, vResolvedName:=m_sPartyName, vPartyTypeCode:=m_sPartyTypeCode, vDateOfBirth:=m_vDateOfBirth, vGender:=m_vGender, vLicenseTypeId:=m_vLicenceType, vLicenseNumber:=m_vLicenceNumber, vStatus:=m_vStatus, vReferenceNumber:=m_vReferenceNumber, vRegNumber:=m_vRegNumber, vSupplierBusinessArray:=m_vResultArray, vAccidentsArray:=m_vAccidents, vExtraSupplierDetails:=m_vPartyDetail, vCurrencyID:=m_lCurrencyID, vBranch:=m_vBranch, vSubBranch:=m_vSubBranch, vDatePassedTest:=m_vDatePassedTest, vContactName:=m_vContactName, vContactTelephoneNumber:=m_vContactTelephoneNumber, vInsurerName:=m_vInsurerName, vInsurerAddress1:=m_vInsurerAddress1, vInsurerAddress2:=m_vInsurerAddress2, vInsurerAddress3:=m_vInsurerAddress3, vInsurerAddress4:=m_vInsurerAddress4, vInsurerPostcode:=m_vInsurerPostcode, vInsurerTelephoneNumber:=m_vInsurerTelephoneNumber, vInsurerFaxNumber:=m_vInsurerFaxNumber, vInsurerContactName:=m_vInsurerContactName, vInsurerEmail:=m_vInsurerEmail, vInsurerNotes:=m_vInsurerNotes, vCompanyNotes:=m_vCompanyNotes, vUniqueid:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}

                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.

                    ' {* USER DEFINED CODE (Begin) *}
                    'SD 17/09/2002 Add Supplier party type extra details


                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lBusinessDataID, vPartyCnt:=m_lPartyCnt, vShortName:=m_sPartyCode, vName:=m_sPartyName, vResolvedName:=m_sPartyName, vDateOfBirth:=m_vDateOfBirth, vGender:=m_vGender, vLicenseTypeId:=m_vLicenceType, vLicenseNumber:=m_vLicenceNumber, vStatus:=m_vStatus, vReferenceNumber:=m_vReferenceNumber, vRegNumber:=m_vRegNumber, vSupplierBusinessArray:=m_vResultArray, vAccidentsArray:=m_vAccidents, vExtraSupplierDetails:=m_vPartyDetail, vCurrencyID:=m_lCurrencyID, vBranch:=m_vBranch, vSubBranch:=m_vSubBranch, vDatePassedTest:=m_vDatePassedTest, vContactName:=m_vContactName, vContactTelephoneNumber:=m_vContactTelephoneNumber, vInsurerName:=m_vInsurerName, vInsurerAddress1:=m_vInsurerAddress1, vInsurerAddress2:=m_vInsurerAddress2, vInsurerAddress3:=m_vInsurerAddress3, vInsurerAddress4:=m_vInsurerAddress4, vInsurerPostcode:=m_vInsurerPostcode, vInsurerTelephoneNumber:=m_vInsurerTelephoneNumber, vInsurerFaxNumber:=m_vInsurerFaxNumber, vInsurerContactName:=m_vInsurerContactName, vInsurerEmail:=m_vInsurerEmail, vInsurerNotes:=m_vInsurerNotes, vCompanyNotes:=m_vCompanyNotes, vUniqueid:=m_sUniqueId, vScreenHierarchy:=m_sScreenHierarchy)

                    ' {* USER DEFINED CODE (End) *}

            End Select

            ' Reset the mouse pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            Else
                m_oGeneral.UniqueId = m_sUniqueId
                m_oGeneral.ScreenHierarchy = m_sScreenHierarchy
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

            'Get addresse type lookups for the party

            m_lReturn = m_oBusiness.GetAddresstypelookups(vaddresstypes:=m_vAddressTypes)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address type look up details from the business object ", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails")
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}

            ' ************************************************************
            ' Enter your code here to retrieve all of the lookup
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


            m_lReturn = GetLookupDetails(sLookupTable:="Supplier_Business", ctlLookup:=lvwSupBusAvailable)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = GetLookupDetails(sLookupTable:="Supplier_Speciality", ctlLookup:=lvwSupSpecAvailable)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:="Driver_Status", ctlLookup:=cboStatus)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetLookupDetails(sLookupTable:="License_Type", ctlLookup:=cboLicenceType)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RDT PN18099 - Call Get branch for the list of branches available to this user
            m_lReturn = GetBranchDetails()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Party Bank Details
    Public Function LoadPartyBankControl() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "LoadPartyBankControl"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No.9
            m_lReturn = uctPartyBankControl1.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            uctPartyBankControl1.PartyCnt = m_lPartyCnt
            'Developer Guide No. 108
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
        Dim sTmp As String = ""
        'sj 11/11/2002 - start
        'ISS1127
        Dim sPartyOtherPostingType As String = ""
        'sj 11/11/2002 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the data storage.

            ' {* USER DEFINED CODE (Begin) *}

            m_lReturn = m_oBusiness.GetNext(vPartyCnt:=m_lPartyCnt, vShortName:=m_sPartyCode, vName:=m_sPartyName, vDateOfBirth:=m_vDateOfBirth, vGender:=m_vGender, vLicenseTypeId:=m_vLicenceType, vLicenseNumber:=m_vLicenceNumber, vStatus:=m_vStatus, vReferenceNumber:=m_vReferenceNumber, vRegNumber:=m_vRegNumber, vCurrencyID:=m_lCurrencyID, vSourceID:=m_vBranch, vSubBranchID:=m_vSubBranch, vDatePassedTest:=m_vDatePassedTest, vContactName:=m_vContactName, vContactTelephoneNumber:=m_vContactTelephoneNumber, vInsurerName:=m_vInsurerName, vInsurerAddress1:=m_vInsurerAddress1, vInsurerAddress2:=m_vInsurerAddress2, vInsurerAddress3:=m_vInsurerAddress3, vInsurerAddress4:=m_vInsurerAddress4, vInsurerPostcode:=m_vInsurerPostcode, vInsurerTelephoneNumber:=m_vInsurerTelephoneNumber, vInsurerFaxNumber:=m_vInsurerFaxNumber, vInsurerContactName:=m_vInsurerContactName, vInsurerEmail:=m_vInsurerEmail, vInsurerNotes:=m_vInsurerNotes, vCompanyNotes:=m_vCompanyNotes)

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

            'Get Supplier specific data.
            'SD 17/09/2002 Get party type code and additional party details
            '    m_lReturn& = m_oBusiness.GetPartySupplierBusiness(v_lPartyCnt:=m_lPartyCnt, _
            ''                                                      r_sPartyName:=m_sPartyName, _
            ''                                                      r_sPartyTypeDesc:=m_sPartyTypeDesc, _
            ''                                                      r_sPartyTypeCode:=m_sPartyTypeCode, _
            ''                                                      r_sPartyOtherPostingType:=sPartyOtherPostingType, _
            ''                                                      r_vResultArray:=m_vResultArray, _
            ''                                                      r_vPartyDetail:=m_vPartyDetail)


            m_lReturn = m_oBusiness.GetPartyAdditionalDetails(v_lPartyCnt:=m_lPartyCnt, r_sPartyName:=m_sPartyName, r_sPartyTypeDesc:=m_sPartyTypeDesc, r_sPartyTypeCode:=m_sPartyTypeCode, r_sPartyOtherPostingType:=sPartyOtherPostingType)


            m_lReturn = m_oBusiness.GetPartySupplierBusiness(v_lPartyCnt:=m_lPartyCnt, r_vResultArray:=m_vResultArray)


            m_lReturn = m_oBusiness.GetOtherPartyDetails(v_lPartyCnt:=m_lPartyCnt, r_vPartyDetail:=m_vPartyDetail)


            ' NOTE. Don't check for errors - the way the code is written, "Not Found" status is
            ' returned when no party supplier business is found. This is not an error!.


            'SD 17/09/2002 Set the boolean flag for a supplier (if it is not new party)
            'For a new party, this code below is never run.
            m_bIsSupplier = (String.Compare(m_sPartyTypeCode, ACPartyTypeSupplier) = 0)

            'sj 11/11/2002 - start
            'ISS1127
            m_bCreateOrionAccount = Not (sPartyOtherPostingType.Trim() = "NOACCOUNT")
            'sj 11/11/2002 - end


            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            'Get convictions and accidents

            m_lReturn = m_oBusiness.GetConvictionsAndAccidents(r_vConvictions:=m_vConvictions, r_vAccidents:=m_vAccidents)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the convictions details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If


            m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData")
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



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

        ' Temp string to hold postcode before validation
        Dim result As Integer = 0
        Dim sTempPostalCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data storage.
            '    m_sPartyName = m_oFormFields.UnformatControl(txtPartyName)
            '    m_sPartyCode = m_oFormFields.UnformatControl(txtPartyCode)
            m_sPartyName = txtPartyName.Text.Trim()
            m_sPartyCode = txtPartyCode.Text.Trim()

            If txtDateOfBirth.Text = "" Then
                m_vDateOfBirth = Nothing
            Else
                m_vDateOfBirth = CDate(txtDateOfBirth.Text)
            End If


            m_vGender = ddGender.Text

            If cboLicenceType.SelectedIndex = -1 Then

                m_vLicenceType = Nothing
            Else
                m_vLicenceType = VB6.GetItemData(cboLicenceType, cboLicenceType.SelectedIndex)
            End If



            m_vLicenceNumber = m_oFormFields.UnformatControl(ctlControl:=txtLicenceNumber)

            If cboStatus.SelectedIndex = -1 Then

                m_vStatus = Nothing
            Else
                m_vStatus = VB6.GetItemData(cboStatus, cboStatus.SelectedIndex)
            End If



            m_vRegNumber = m_oFormFields.UnformatControl(ctlControl:=txtRegNumber)

            'S4B Claims Enhancements R&D 2005
            'RDT PN 18099
            m_vBranch = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)

            If cboSubBranch.SelectedIndex <> -1 Then
                m_vSubBranch = VB6.GetItemData(cboSubBranch, cboSubBranch.SelectedIndex)
            Else

                m_vSubBranch = Nothing
            End If

            m_lCurrencyID = uctCurrency.CurrencyId

            'All other data automatically updated in background throughout.

            'SD 17/09/2002 Add Supplier party type changes

            m_lReturn = SaveExtraSupplierDetails()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save the extra supplier details", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData")
            End If

            '***************************
            ' get the party tax details from the party tax control
            m_sTaxNumber = uctPartyTax1.TaxNumber
            m_bDomiciledForTax = uctPartyTax1.IsDomiciledForTax
            m_bTaxExempt = uctPartyTax1.TaxExempt
            m_dTaxPercentage = uctPartyTax1.TaxPercentage
            '***************************

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


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

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
            Select Case (m_lNavigate)
                Case gPMConstants.PMENavigateButtonStatus.PMNavigateEnabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = True

                Case gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled
                    '            cmdNavigate.Visible = True
                    '            cmdNavigate.Enabled = False

                Case Else
                    '            cmdNavigate.Visible = False
            End Select

            m_lReturn = SetFirstLastControls()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set full row select on all ListView controls.
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSupBusAvailable.Handle.ToInt32(), v_vShowRowSelect:=True)
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSupBusSelected.Handle.ToInt32(), v_vShowRowSelect:=True)
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSupSpecAvailable.Handle.ToInt32(), v_vShowRowSelect:=True)
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwSupSpecSelected.Handle.ToInt32(), v_vShowRowSelect:=True)
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwAddresses.Handle.ToInt32(), v_vShowRowSelect:=True)
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwConvictions.Handle.ToInt32(), v_vShowRowSelect:=True)
            m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwAccidents.Handle.ToInt32(), v_vShowRowSelect:=True)

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                '  cmdAddConviction.Enabled = False
            Else
                txtPartyCode.Enabled = False
                'JMK only needed when adding 1st time
                cmdApply.Visible = False
            End If

            'SD 17/09/2002 Populate the Yes/No comboboxes
            m_lReturn = PopulateYesNoCombo(cboActive)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = PopulateYesNoCombo(cboAfterHours)
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'SD 17/09/2002 Populate the Priority combobox
            m_lReturn = PopulatePriorityCombo()
            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do NOT show Branch or SubBranch combos in Broking  PN18666
            m_lTabFirst = ACTAB_GENERAL
            'm_lTabLast = ACTAB_TAX
            'Party Bank Details
            m_lTabLast = ACTAB_PartyBank
            SSTabHelper.SetTabVisible(Me.tabMainTab, ACTAB_INSURER, False)
            Me.lblReference.Visible = False
            Me.txtReference.Visible = False
            Me.lblCompanyNotes.Visible = False
            Me.txtCompanyNotes.Visible = False
            Me.lblDatePassedTest.Visible = False
            Me.txtDatePassedTest.Visible = False
            Me.fraContactDetails.Visible = False


            If m_lTabFirst > ACTAB_INDEXLOW Then
                cmdPrevious(m_lTabFirst - 1).Visible = False
            End If

            If m_lTabLast < ACTAB_INDEXHIGH Then
                cmdNext(m_lTabLast).Visible = False
            End If

            ReNumberTabs(Me.tabMainTab, " - ", True)

            'Party Bank Details
            m_lReturn = uctPartyBankControl1.SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults")
            End If
            'Developer Guide No. (According to the VB Code)
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonNotes - 1).Enabled = (m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView)
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonFinancial - 1).Enabled = False
            Toolbar1.Items.Item(ACIButtonCommission - 1).Enabled = False
            Toolbar1.Items.Item(SIRToolbarFunc.ACIButtonLetter - 1).Enabled = False

            Return result

        Catch excep As System.Exception



            ' Error Section.

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
            ReDim m_ctlTabFirstLast(1, 1)

            ' Set the first and last data entry controls for
            ' all of the tabs.

            '     {* USER DEFINED CODE (Begin) *}
            m_ctlTabFirstLast(ACControlStart, 0) = txtPartyCode
            m_ctlTabFirstLast(ACControlEnd, 0) = lvwSupSpecSelected

            m_ctlTabFirstLast(ACControlStart, 1) = lvwAddresses
            m_ctlTabFirstLast(ACControlEnd, 1) = lvwAddresses
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
            ' Display all language specific captions

            If m_sPartyTypeCode.Trim() <> "" Then

                m_lReturn = m_oBusiness.GetPartyTypeDescription(m_sPartyTypeCode, m_sPartyTypeDesc)
                Me.Text = m_sPartyTypeDesc
            End If

            '    frmInterface.Caption = iPMFunc.GetResData( _
            ''          iLangID:=g_iLanguageID%, _
            ''          lID:=ACInterfaceCaption, _
            ''          iDataType:=PMResString)


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle0, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMainTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabMainTab, ACTAB_INSURER, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInsurerTabCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEditAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditConCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDeleteAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteConCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdAddAddress.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddConCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'SD 17/09/2002 Add Supplier party type extra controls

            lblActive.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACActive, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblAfterHours.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAfterHours, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPriority.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPriority, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPartyCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyCodeCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblPartyName.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyNameCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblDateOfBirth.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDateOfBirthCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            lblGender.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGenderCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            m_sMandatoryFieldsTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMandatoryFieldsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressUsage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListUsage, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine1 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine2 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine3 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sAddressLine4 = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListLine4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sPostCode = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressListPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

            ' {* USER DEFINED CODE (End) *}

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
    'Developer Guide No.101
    Private Function GetLookupDetails(ByRef sLookupTable As String, ByRef ctlLookup As Control, Optional ByRef bSecondary As Boolean = False) As Integer

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

        Dim oListItem As ListViewItem

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

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.

            If TypeOf ctlLookup Is ComboBox Then
                ' Using the lookup values, populate the control with
                ' the details from the lookup details array.
                For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)
                    ' Add the details to the control.
                    Dim ctlLookup_NewIndex As Integer = -1
                    'Developer Guide No. (As per VB Code)
                    ctlLookup_NewIndex = CType(ctlLookup, ComboBox).Items.Add(m_vLookupDetails(ACDetailDesc, lCntr))
                    VB6.SetItemData(ctlLookup, ctlLookup_NewIndex, CInt(m_vLookupDetails(ACDetailKey, lCntr)))

                    ' Check if this is the selected index.
                    If bSecondary Then
                        lRow2 = lRow + 1
                    Else
                        lRow2 = lRow
                    End If
                    If CStr(m_vLookupValues(ACValueID, lRow2)) <> "" Then
                        If CDbl(m_vLookupValues(ACValueID, lRow2)) = CInt(m_vLookupDetails(ACDetailKey, lCntr)) Then
                            'Developer Guide No. (As per VB Code)
                            CType(ctlLookup, ComboBox).SelectedIndex = ctlLookup_NewIndex
                        End If

                    End If

                Next lCntr

                ' Check if the selected index is blank. If so,
                ' we set the controls index to zero.
                If CStr(m_vLookupValues(ACValueID, lRow2)) = "" Then
                    'Developer Guide No. (As per VB Code)
                    CType(ctlLookup, ComboBox).SelectedIndex = -1
                End If
            Else

                CType(ctlLookup, ListView).Items.Clear()
                For lCntr As Integer = CInt(m_vLookupValues(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupValues(ACValueStartPos, lRow)) + CDbl(m_vLookupValues(ACValueNumber, lRow))) - 1)

                    If CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim().ToUpper() <> "NONE" Then

                        oListItem = CType(ctlLookup, ListView).Items.Add(m_vLookupDetails(ACDetailDesc, lCntr))
                        oListItem.Tag = CStr(m_vLookupDetails(ACDetailKey, lCntr))
                    End If
                Next lCntr

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
    ' Name: PopulateAccidents
    '
    ' Description: Fills the grid control with accident details
    '
    ' ***************************************************************** '
    Private Sub PopulateAccidents()

        Dim oListItem As ListViewItem

        Try

            lvwAccidents.Items.Clear()

            'EK 12/12/99 Correct column/data matching

            If Not Information.IsArray(m_vAccidents) Then
                Exit Sub
            End If

            ' Assign the details to the interface.
            For i As Integer = m_vAccidents.GetLowerBound(1) To m_vAccidents.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                ' Assign the details to the first column.
                ' Date
                'Developer Guide No.149
                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=Convert.ToString(m_vAccidents(2, i)).Trim())


                'Developer Guide No.49
                oListItem = lvwAccidents.Items.Add(txtDate.Text, "ACCIDENTIMAGE")
                'Developer Guide No.149
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = Convert.ToString(m_vAccidents(3, i)).Trim()

                'At Fault
                'Developer Guide No.248
                If gPMFunctions.ToSafeDouble(m_vAccidents(4, i)) = 1 Then
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes"
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No"
                End If

                ' Store the Conviction_cnt
                'Developer Guide No.149
                oListItem.Tag = Convert.ToString(m_vAccidents(1, i)).Trim()
                ' {* USER DEFINED CODE (End) *}

                ' Set the tag property with the index of
                ' the search data storage.

            Next i
            '    'Populate the cells

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAccidents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: PopulateConvictions
    '
    ' Description: Fills the grid control with conviction details
    '
    ' ***************************************************************** '
    Private Sub PopulateConvictions()

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

                oListItem = lvwConvictions.Items.Add(CStr(m_vConvictions(2, i)).Trim(), "CONVICTIONIMAGE")

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


    ' PRIVATE Methods (End)

    Private Sub cmdAddAccident_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAccident.Click

        Dim lCount, tagCount As Integer
        Dim sScreenHierarchy As String = ""


        'Developer Guide No.50
        frmAccidents = New frmAccidents

        Dim tempLoadForm As frmAccidents = frmAccidents

        frmAccidents.ShowDialog()

        If frmAccidents.Status = gPMConstants.PMEReturnCode.PMCancel Then
            frmAccidents.Close()
            frmAccidents = Nothing
            Exit Sub
        End If

        If Not Information.IsArray(m_vAccidents) Then
            lCount = 0
            ReDim m_vAccidents(4, lCount)
            tagCount = lvwAccidents.Items.Count + 1
        Else
            lCount = m_vAccidents.GetUpperBound(1) + 1
            tagCount = lvwAccidents.Items.Count + 1  'added by sumit
            ReDim Preserve m_vAccidents(4, lCount)
        End If

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If
        '        If lCount > 0 Then lCount -= 1

        'm_vAccidents(1, lCount) = lCount 'PN 20875  'commented by sumit
        m_vAccidents(1, lCount) = tagCount           'added by sumit
        m_vAccidents(2, lCount) = frmAccidents.AccidentDate
        m_vAccidents(3, lCount) = frmAccidents.Description
        m_vAccidents(4, lCount) = frmAccidents.IsAtFault

        frmAccidents.Close()
        frmAccidents = Nothing

        PopulateAccidents()


    End Sub

    Private Sub cmdAddAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAddress.Click


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

            m_oAddress.Reference = txtPartyCode.Text

            m_oAddress.PostCode = m_sMainPostCode

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
                m_oAddress.UniqueId = m_sUniqueId
            End If
            m_oAddress.ScreenHeirarchy = $"Other Party({txtPartyCode.Text.Trim()})"

            'PN 19428

            'Developer Guide No.162
            m_oAddress.CountryID = gPMFunctions.ToSafeLong(CStr(m_vSourceArray(3, uctBranch.SelectedIndex - 1)))

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            ' Reset the moust pointer
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


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


                    oListItem = lvwAddresses.Items.Add(m_oAddress.PostalCode, ACIADDRESS)
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


                    oListItem = lvwAddresses.Items.Add(m_oAddress.AddressUsageType, ACIADDRESS)
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
            'Developer Guide No. 178
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAddresses, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdAddConviction_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddConviction.Click


        Dim oListItem As ListViewItem

        Try

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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get convictions", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConviction_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Exit Sub

                End If

            End If

            m_lReturn = m_oConviction.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_oConviction.PartyCnt = m_lPartyCnt
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
                m_oConviction.UniqueId = m_sUniqueId
            Else
                m_oConviction.UniqueId = m_sUniqueId
            End If

            m_oConviction.ScreenHierarchy = $"Other Party({txtPartyCode.Text.Trim()})"

            m_lReturn = m_oConviction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, add to grid

            If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            Me.Refresh()



            'Developer Guide No.49
            oListItem = lvwConvictions.Items.Add(m_oConviction.Code, "CONVICTIONIMAGE")


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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdAddConviction_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAddConviction_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub



        End Try

    End Sub

    ' JMK 23/08/2001
    ' cmdApply
    ' added so that user can add convictions while adding
    ' a new Party. Largely the same as cmdOK_Click()

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click


        Dim sPartyOtherPostingType As String = ""

        ' Click event of the Apply button.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Maintain Party Code
            If m_bIsSetMaskingCode And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = ValidateNumberingScheme()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                m_lReturn = GeneratePartyCode()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblPartyCode.Enabled = False
                txtPartyCode.Enabled = False
            End If

            m_lReturn = CheckMandatoryFieldsExtra()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
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

                'update the contact cnt property

                m_lPartyCnt = m_oBusiness.PartyCnt

                'Update party addresses
                m_lReturn = UpdateAddresses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Address Details", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()

                'sj 11/11/2002 - start
                'ISS1127

                m_lReturn = m_oBusiness.GetOtherPartyPostingType(v_lPartyCnt:=m_lPartyCnt, r_sPartyOtherPostingType:=sPartyOtherPostingType)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.GetOtherPartyPostingType Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
                m_bCreateOrionAccount = Not (sPartyOtherPostingType.Trim() = "NOACCOUNT")
                'sj 11/11/2002 - end

                ' RWH(23/07/01) - Update Orion
                'sj 08/11/2002 - start
                If m_bCreateOrionAccount Then
                    'sj 08/11/2002 - end
                    m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                        ' Set the mouse pointer to normal.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                End If
                ' JMK 24/08/2001 - the following is where Apply differs from OK
                '        ' Everything OK, so we can hide the interface.
                '        Me.Hide

                ' change Task to Edit and update with new PartyCnt
                With Me
                    .Task = gPMConstants.PMEComponentAction.PMEdit
                    .PartyCnt = m_lPartyCnt
                End With

                ' populate address array

                m_lReturn = m_oBusiness.GetAddressDetails(vAddresses:=m_vAddresses)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to retrieve the address details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                End If

                ' re-fetch details - to make sure new PartyID is initialised
                m_lReturn = GetBusiness()
                'Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to get business
                    m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetBusiness()", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click")

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
                cmdAddConviction.Enabled = True
                cmdApply.Enabled = False


            End If
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdBusinessAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBusinessAdd.Click

        'TN20000928 - only move from lvwSupBusAvailable if we have data
        If lvwSupBusAvailable.Items.Count > 0 Then
            If AddSupplierBusiness() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = MoveListItem(lvwSupBusAvailable, lvwSupBusSelected)

                If lvwSupBusAvailable.Items.Count < 1 Then
                    cmdBusinessAdd.Enabled = False
                End If

            End If
        End If

    End Sub

    Private Sub cmdBusinessRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBusinessRemove.Click

        'TN20000928 - only move from lvwSupBusSelected if we have data
        If lvwSupBusSelected.Items.Count > 0 Then
            If RemoveSupplierBusiness() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = MoveListItem(lvwSupBusSelected, lvwSupBusAvailable)

                EnableSpecialityFrame(False)

                If lvwSupBusSelected.Items.Count < 1 Then
                    cmdBusinessRemove.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub cmdDeleteAccident_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAccident.Click

        Dim lCount2 As Integer

        Dim vArray(,) As Object = Nothing

        'PN 20875
        If MessageBox.Show("Are you sure want to delete the selected accident.", "Delete Accident", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.No Then
            Exit Sub
        End If

        If String.IsNullOrEmpty(UniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        Dim lTag As Integer = Convert.ToString(lvwAccidents.FocusedItem.Tag)
        If lvwAccidents.Items.Count <> 1 Then    'added by sumit to check if there is one row on the listview control
            For lCount As Integer = 0 To m_vAccidents.GetUpperBound(1)
                If lCount <> lTag - 1 Then
                    If Not Information.IsArray(vArray) Then 'PN 20875
                        lCount2 = 0
                        ReDim vArray(4, lCount2)
                    Else

                        lCount2 = vArray.GetUpperBound(1) + 1
                        ReDim Preserve vArray(4, lCount2) 'PN 20875
                    End If


                    vArray(0, lCount2) = m_vAccidents(0, lCount)

                    vArray(1, lCount2) = m_vAccidents(1, lCount)

                    vArray(2, lCount2) = m_vAccidents(2, lCount)

                    vArray(3, lCount2) = m_vAccidents(3, lCount)

                    vArray(4, lCount2) = m_vAccidents(4, lCount)
                End If
            Next lCount
        End If

        m_vAccidents = vArray

        PopulateAccidents()

        'Reset Interface
        cmdEditAccident.Enabled = False
        cmdDeleteAccident.Enabled = False

        lvwAccidents.Focus()





    End Sub

    Private Sub cmdDeleteAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAddress.Click


        Try

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

            'set the address id


            m_oAddress.AddressCnt = m_lAddressCnt
            '    m_oAddress.AddressUsageTypeID = m_lAddressUsageTypeID&


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

            lvwAddresses.Items.RemoveAt(m_iLine - 1)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    Private Sub cmdDeleteConviction_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteConviction.Click



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
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
                m_oConviction.UniqueId = m_sUniqueId
            Else
                m_oConviction.UniqueId = m_sUniqueId
            End If

            m_oConviction.ScreenHierarchy = $"Other Party({txtPartyCode.Text.Trim()})"

            m_lReturn = m_oConviction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditConviction.Enabled = False
            cmdDeleteConviction.Enabled = False

            lvwConvictions.Items.RemoveAt(lvwConvictions.FocusedItem.Index)

            lvwConvictions.Focus()

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteConviction_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteConviction_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditAccident_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAccident.Click

        Dim lCount As Integer

        Dim oListItem As ListViewItem = lvwAccidents.FocusedItem

        If oListItem Is Nothing Then
            Exit Sub
        End If
        'Developer Guide No.50
        frmAccidents = New frmAccidents
        If Convert.ToString(oListItem.Tag) > 0 Then
            lCount = CInt(Convert.ToString(oListItem.Tag) - 1)
        Else

            lCount = Convert.ToString(oListItem.Tag) 'PN #40289
        End If
        'Developer Guide No.248
        frmAccidents.AccidentDate = gPMFunctions.ToSafeDate(m_vAccidents(2, lCount))
        frmAccidents.Description = gPMFunctions.ToSafeString(m_vAccidents(3, lCount))
        frmAccidents.IsAtFault = gPMFunctions.ToSafeInteger(m_vAccidents(4, lCount))

        Dim tempLoadForm As frmAccidents = frmAccidents

        frmAccidents.ShowDialog()

        If frmAccidents.Status = gPMConstants.PMEReturnCode.PMCancel Then
            frmAccidents.Close()
            frmAccidents = Nothing
            Exit Sub
        End If

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_vAccidents(2, lCount) = frmAccidents.AccidentDate
        m_vAccidents(3, lCount) = frmAccidents.Description
        m_vAccidents(4, lCount) = frmAccidents.IsAtFault

        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_vAccidents(2, lCount))

        oListItem.Text = txtDate.Text
        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vAccidents(3, lCount))

        If CDbl(m_vAccidents(4, lCount)) = 1 Then
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Yes"
        Else
            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "No"
        End If

        frmAccidents.Close()
        frmAccidents = Nothing

    End Sub

    Private Sub cmdEditAddress_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditAddress.Click


        Dim lAddressCnt As Integer
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

            'set the main postcode and reference

            m_oAddress.Reference = txtPartyCode.Text

            oListItem = lvwAddresses.FocusedItem

            'set the address id


            m_oAddress.AddressCnt = Convert.ToString(oListItem.Tag)

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

            m_lReturn = m_oAddress.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Get the address count

            lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)

            'set the address id

            m_oAddress.AddressCnt = lAddressCnt
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
                m_oAddress.UniqueId = m_sUniqueId
            End If
            m_oAddress.ScreenHeirarchy = $"Other Party({txtPartyCode.Text.Trim()})"

            m_lReturn = m_oAddress.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oAddress.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

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
            'Developer Guide No. 178
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAddresses, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdEditConviction_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditConviction.Click


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
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get conviction component", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConviction_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

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
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
                m_oConviction.UniqueId = m_sUniqueId
            Else
                m_oConviction.UniqueId = m_sUniqueId
            End If

            m_oConviction.ScreenHierarchy = $"Other Party({txtPartyCode.Text.Trim()})"

                m_lReturn = m_oConviction.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If not cancelled, edit grid

            If m_oConviction.Status = gPMConstants.PMEReturnCode.PMCancel Then
                Exit Sub
            End If

            'Reset Interface
            cmdEditConviction.Enabled = False
            cmdDeleteConviction.Enabled = False
            'EK 12/12 Fixed to match data/headings

            oListItem = lvwConvictions.Items.Item(m_iLine - 1)

            ' Assign details to other the columns
            ' Column 1

            oListItem.Text = m_oConviction.Code

            'Conviction Date

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDate, vControlValue:=m_oConviction.ConvictionDate.Trim())

            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = txtDate.Text.Trim()

            ' Description

            ListViewHelper.GetListViewSubItem(oListItem, 2).Text = m_oConviction.Description.Trim()

            ' Fine

            'Developer Guide No.260
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCurrency, vControlValue:=m_oConviction.FineAmount)

            ListViewHelper.GetListViewSubItem(oListItem, 3).Text = txtCurrency.Text.Trim()

            ' Conviction Status

            ListViewHelper.GetListViewSubItem(oListItem, 4).Text = m_oConviction.StatusCode

            ' Penalty Points

            ListViewHelper.GetListViewSubItem(oListItem, 5).Text = m_oConviction.DrivingLicencePenaltyPoints

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdEditConviction_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEditConviction_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        '    MsgBox "No help currently available for this screen", vbInformation, m_sPartyTypeDesc
        ' Fire up the help screen
        'Developer Guide No. 184
        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, MainModule.ScreenHelpID)


    End Sub


    Private Sub cmdNext_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_5.Enter, _cmdNext_4.Enter, _cmdNext_3.Enter, _cmdNext_2.Enter, _cmdNext_1.Enter, _cmdNext_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)
        SSTabHelper.SetSelectedIndex(tabMainTab, Index)

    End Sub

    Private Sub cmdPrevious_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_6.Enter, _cmdPrevious_5.Enter, _cmdPrevious_4.Enter, _cmdPrevious_3.Enter, _cmdPrevious_2.Enter, _cmdPrevious_1.Enter, _cmdPrevious_0.Enter
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        SSTabHelper.SetSelectedIndex(tabMainTab, Index + 1)
    End Sub

    Private Sub cmdSpecialityAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSpecialityAdd.Click

        'TN20000928 - only from from lvwSupSpecAvailable if we have data
        If lvwSupSpecAvailable.Items.Count > 0 Then
            If AddSupplierSpeciality() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = MoveListItem(lvwSupSpecAvailable, lvwSupSpecSelected)

                If lvwSupSpecAvailable.Items.Count < 1 Then
                    cmdSpecialityAdd.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub cmdSpecialityRemove_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSpecialityRemove.Click

        'TN20000928 - only move from lvwSupSpecSelected if we have data
        If lvwSupSpecSelected.Items.Count > 0 Then
            If RemoveSupplierSpeciality() = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = MoveListItem(lvwSupSpecSelected, lvwSupSpecAvailable)

                If lvwSupSpecSelected.Items.Count < 1 Then
                    cmdSpecialityRemove.Enabled = False
                End If
            End If
        End If
    End Sub

    ' PRIVATE Events (Begin)

    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String
        Dim vValue As Object

        ' Forms initialise event.

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyOT.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMBPartyOT.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            'Broking doesn't show Sub Branch by default

            iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTSubBranchShowingForBroking, v_vBranch:=g_iSourceID, r_vUnderwriting:=CStr(vValue))

            iPMFunc.getUnderwritingOrAgency(m_sUnderwritingOrAgency)
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

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
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


            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", v_vReturnColumn:="is_view_only_other_party_maintenance", v_sKeyColumn:="user_id", v_sKeyValue:=g_oObjectManager.UserID, v_iDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=m_bIsViewOnlyOTMaintenance)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Execute GetPartyViewOptions", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", excep:=New Exception(Information.Err().Description))
                Return result
            Else
                'Start - Prakash Varghese - PN 61117
                'Modified the condition ToSafeBoolean(m_bIsViewOnlyOTMaintenance) = True
                'to ToSafeBoolean(m_bIsViewOnlyOTMaintenance) since it is giving problems in runtime
                If gPMFunctions.ToSafeBoolean(m_bIsViewOnlyOTMaintenance) Then
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



        Dim oOptions As bSIROptions.Business
        Dim sValue As String = ""
        'Changes for WPR-42
        Dim Key As uctPickList.PickListKey
        'End Changes for WPR-42
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

            Dim temp_oOptions As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oOptions, "bSIROptions.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oOptions = temp_oOptions

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If


            m_lReturn = oOptions.GetOption(iOptionNumber:=13, sValue:=sValue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the correct QAS database
            uctPMAddressControl1.QASDatabaseID = CInt(Conversion.Val(sValue))

            If Conversion.Val(sValue) <> 0 Then
                uctPMAddressControl1.PMDatabaseID = 0
            Else
                uctPMAddressControl1.PMDatabaseID = 1
            End If


            oOptions.Dispose()
            oOptions = Nothing



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

            'Add Supplier party type changes
            'If the party type is supplier, then show the supplier frame
            fraSupplier.Visible = m_bIsSupplier

            'If adding, still need to get address types for populating
            'the combo box cells in the grid control
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                'Set the index of the main address
                For i As Integer = m_vAddressTypes.GetLowerBound(1) To m_vAddressTypes.GetUpperBound(1)

                    'See if this is the main address
                    If CStr(m_vAddressTypes(2, i)).Trim().ToUpper() = gSIRLibrary.SIRMainAddressABICode Then
                        m_iMainAddressIndex = CInt(m_vAddressTypes(0, i))
                        Exit For
                    End If

                Next i

            End If

            'Maintain Party Code
            If Task = gPMConstants.PMEComponentAction.PMAdd Or Task = gPMConstants.PMEComponentAction.PMEdit Then
                m_lReturn = SetClientCodeCntl()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to set txtreference from SetClientCodeCntl ", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load")
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            'Changes for WPR-42
            ''Party Filtering

            'Setup the picklists
            Key = New uctPickList.PickListKey()
            Key.KeyName = "PartyCnt"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="PartyCnt")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "Branchid"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="Branchid")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "UserId"
            Key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListBranches.ForeignKeys.Add(Key, Key:="UserId")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "UniqueId"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="UniqueId")

            Key = New uctPickList.PickListKey()
            Key.KeyName = "ScreenHierarchy"
            Key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListBranches.ForeignKeys.Add(Key, Key:="ScreenHierarchy")

            SetPickListPKs()

            m_lReturn = uctPickListBranches.Load_Renamed

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Branches", "Agent Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            ''Party Filtering
            'End Changes for WPR-42
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
                    'Developer Guide No.7
                    eventArgs.Cancel = 1
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

            ' Terminate the contact object (is used)
            If Not (m_oAddress Is Nothing) Then


                m_oAddress.Dispose()

                ' Destroy the instance of the contact object
                ' from memory.
                m_oAddress = Nothing

            End If

            ' Terminate the conviction object (if used)
            If Not (m_oConviction Is Nothing) Then


                m_oConviction.Dispose()

                ' Destroy the instance of the conviction object
                ' from memory.
                m_oConviction = Nothing

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


        Catch



            ' Error Section.

            Exit Sub
        End Try


    End Sub

    Private Sub lvwAccidents_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAccidents.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 4
        SSTabHelper.SetSelectedIndex(tabMainTab, 4)

    End Sub

    Private Sub lvwAccidents_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAccidents.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If lvwAccidents.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditAccident.Enabled = False
            cmdDeleteAccident.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditAccident.Enabled = True
                cmdDeleteAccident.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwAddresses_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Click

        If Not (lvwAddresses.FocusedItem Is Nothing) Then

            m_lAddressCnt = Convert.ToString(lvwAddresses.FocusedItem.Tag)
            m_iLine = lvwAddresses.FocusedItem.Index + 1
        End If

    End Sub

    Private Sub lvwAddresses_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.DoubleClick

        ' Active the edit button
        cmdEditAddress_Click(cmdEditAddress, New EventArgs())

    End Sub

    Private Sub lvwAddresses_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAddresses.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 1)
    End Sub

    Private Sub lvwAddresses_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAddresses.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end
        If Not (lvwAddresses.GetItemAt(x, y) Is Nothing) Then
            cmdDeleteAddress.Enabled = True
            cmdEditAddress.Enabled = True
        Else
            cmdDeleteAddress.Enabled = False
            cmdEditAddress.Enabled = False
        End If

    End Sub

    Private Sub lvwConvictions_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwConvictions.Enter

        'If we're here by tabbing (or back-tabbing) we're on tab 4
        SSTabHelper.SetSelectedIndex(tabMainTab, 3)

    End Sub

    Private Sub lvwConvictions_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwConvictions.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end

        If lvwConvictions.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
            cmdEditConviction.Enabled = False
            cmdDeleteConviction.Enabled = False
        Else
            'Not if we're viewing...
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                cmdEditConviction.Enabled = True
                cmdDeleteConviction.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwSupBusAvailable_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSupBusAvailable.DoubleClick
        cmdBusinessAdd_Click(cmdBusinessAdd, New EventArgs())
    End Sub

    Private Sub lvwSupBusAvailable_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSupBusAvailable.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 2)
    End Sub

    Private Sub lvwSupBusAvailable_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSupBusAvailable.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        cmdBusinessAdd.Enabled = (Not (lvwSupBusAvailable.GetItemAt(x, y) Is Nothing))

    End Sub

    Private Sub lvwSupBusSelected_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSupBusSelected.Click


        If Not (lvwSupBusSelected.FocusedItem Is Nothing) Then
            m_lBusinessId = CInt(Conversion.Val(Convert.ToString(lvwSupBusSelected.FocusedItem.Tag)))
            m_iLine = lvwSupBusSelected.FocusedItem.Index + 1
            DisplayAssociatedSpecialities()
        End If

    End Sub

    Private Sub lvwSupBusSelected_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSupBusSelected.DoubleClick
        cmdBusinessRemove_Click(cmdBusinessRemove, New EventArgs())

    End Sub

    Private Sub lvwSupBusSelected_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSupBusSelected.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        If Not (lvwSupBusSelected.GetItemAt(x, y) Is Nothing) Then
            EnableSpecialityFrame(True)
            cmdBusinessRemove.Enabled = True
        Else
            EnableSpecialityFrame(False)
            cmdBusinessRemove.Enabled = False
        End If

    End Sub

    Private Sub lvwSupSpecAvailable_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSupSpecAvailable.DoubleClick
        cmdSpecialityAdd_Click(cmdSpecialityAdd, New EventArgs())
    End Sub

    Private Sub lvwSupSpecAvailable_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSupSpecAvailable.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        cmdSpecialityAdd.Enabled = (Not (lvwSupSpecAvailable.GetItemAt(x, y) Is Nothing))

    End Sub

    Private Sub lvwSupSpecSelected_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSupSpecSelected.DoubleClick
        cmdSpecialityRemove_Click(cmdSpecialityRemove, New EventArgs())

    End Sub

    Private Sub lvwSupSpecSelected_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwSupSpecSelected.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        cmdSpecialityRemove.Enabled = (Not (lvwSupSpecSelected.GetItemAt(x, y) Is Nothing))

    End Sub

    Private Sub tabMainTab_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabMainTab.SelectedIndexChanged

        Try

            With tabMainTab
                ' Set the default button.
                '        If (.Tab < cmdNext.Count) Then
                '            cmdNext(.Tab).Default = True
                '        Else
                '            cmdOK.Default = True
                '        End If

                ' Now I know this is crap, this goes against
                ' all my principles, but for some reason when
                ' using the mouse to select a tab the setfocus
                ' code below doesn't work. The cursor sticks,
                ' and you can't tab off. Therefore I've used
                ' this to get around the problem.
                Application.DoEvents()

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


        Dim sPartyOtherPostingType As String = ""
        Dim nReturn As Integer
        ' Click event of the OK button.

        Try

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

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Maintain Party Code
            If m_bIsReadOnly Then
                lblPartyCode.Enabled = False
                txtPartyCode.Enabled = False
            End If

            m_lReturn = CheckMandatoryFieldsExtra()

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

                'update the contact cnt property

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

                'sj 11/11/2002 - start
                'ISS1127
                If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                    m_lReturn = m_oBusiness.GetOtherPartyPostingType(v_lPartyCnt:=m_lPartyCnt, r_sPartyOtherPostingType:=sPartyOtherPostingType)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oBusiness.GetOtherPartyPostingType Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click")

                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        Exit Sub
                    End If
                    m_bCreateOrionAccount = Not (sPartyOtherPostingType.Trim() = "NOACCOUNT")
                End If
                'sj 11/11/2002 - end

                'Set the main address post code and address line 1 into the properties
                '(as this may have changed in the address component)
                UpdateAddressPostCodeProperties()

                ' RWH(23/07/01) - Update Orion
                'sj 08/11/2002 - start
                If m_bCreateOrionAccount Then
                    'sj 08/11/2002 - end
                    m_lReturn = UpdateOrion(vPartyCnt:=m_lPartyCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Update Orion. PartyCnt = " & m_lPartyCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                        Exit Sub
                    End If
                End If

                'Party Bank Details
                'Developer Guide No.9
                m_lReturn = uctPartyBankControl1.Initialise()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdOK_Click", "uctPartyBankControl1.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                uctPartyBankControl1.PartyCnt = m_lPartyCnt
                uctPartyBankControl1.ScreenHierarchy = m_sScreenHierarchy
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

                Dim oPartyBusiness As bSIRParty.Business = Nothing
                nReturn = g_oObjectManager.GetInstance(oPartyBusiness, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create bSIRParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If

                oPartyBusiness.AddPartyHistory(m_lPartyCnt, String.Empty)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Party History Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click")
                End If

            End If
            'Update three fields in to party_other table

            'm_lReturn = m_oBusiness.UpdateOtherPartyDetails()

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
    ' Error Section.
    '
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Navigate command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdNavigate_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Exit Sub
    '
    'End Try
    '
    'End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdNext_6.Click, _cmdNext_5.Click, _cmdNext_4.Click, _cmdNext_3.Click, _cmdNext_2.Click, _cmdNext_1.Click, _cmdNext_0.Click
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)

        Try


            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) < SSTabHelper.GetTabCount(tabMainTab) - 1 Then
                For lTabIndex As Integer = Index + 1 To ACTAB_INDEXHIGH
                    If SSTabHelper.GetTabVisible(Me.tabMainTab, lTabIndex) Then
                        SSTabHelper.SetSelectedIndex(Me.tabMainTab, lTabIndex)
                        Exit For
                    End If
                Next lTabIndex
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try


    End Sub

    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_7.Click, _cmdPrevious_6.Click, _cmdPrevious_5.Click, _cmdPrevious_4.Click, _cmdPrevious_3.Click, _cmdPrevious_2.Click, _cmdPrevious_1.Click, _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)

        Try


            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabMainTab) > 0 Then
                'The previous button has index 0 on tab 1, etc
                'tabMainTab.Tab = Index - 1
                For lTabIndex As Integer = Index To ACTAB_INDEXLOW Step -1
                    If SSTabHelper.GetTabVisible(Me.tabMainTab, lTabIndex) Then
                        SSTabHelper.SetSelectedIndex(Me.tabMainTab, lTabIndex)
                        Exit For
                    End If
                Next lTabIndex
            End If

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

    ' PRIVATE Events (End)

    Private Function MoveListItem(ByRef lvwFrom As ListView, ByRef lvwTo As ListView) As gPMConstants.PMEReturnCode
        '"Moves" selected list item by adding the information into lvwTo and then
        'removing the copied item from lvwFrom
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oList As ListViewItem

        Try

            oList = lvwTo.Items.Add(lvwFrom.FocusedItem.Text)

            oList.Tag = Convert.ToString(lvwFrom.FocusedItem.Tag)

            lvwFrom.Items.RemoveAt(lvwFrom.FocusedItem.Index)

            oList = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MoveListItem", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveListItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub EnableSpecialityFrame(ByRef bEnable As Boolean)

        If Not bEnable Then
            lvwSupSpecSelected.Items.Clear()
        End If

        fraSpeciality.Enabled = bEnable
        lvwSupSpecAvailable.Enabled = bEnable
        lvwSupSpecSelected.Enabled = bEnable

    End Sub

    Private Function DisplayAssociatedSpecialities() As Integer
        'RWH Routine displays Specialities stored in Speciality Array, for the
        'currently selected business, in the appropriate ListView. It does this
        'by matching stored ones against the viewed available ones and moving
        'the item from the Available ListView to the Selected one. This ensures
        'the 2 list views are in sync.
        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (m_vResultArray Is Nothing) Then
                'Find the element of the ResultArray relating to the Business lvw selection.
                For iRecordCount As Integer = 0 To m_vResultArray.GetUpperBound(1)
                    If Conversion.Val(Convert.ToString(lvwSupBusSelected.FocusedItem.Tag)) = CDbl(m_vResultArray(0, iRecordCount)) Then
                        m_iCurrentBusIndex = iRecordCount

                        m_vSpecialityArray = m_vResultArray(1, iRecordCount)
                        Exit For
                    End If
                Next iRecordCount

                lvwSupSpecSelected.Items.Clear()

                'Re-populate available specialities.

                m_lReturn = GetLookupDetails(sLookupTable:="Supplier_Speciality", ctlLookup:=lvwSupSpecAvailable)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Move listitems matching array values to 'Selected' ListView.

                If Not Object.Equals(m_vSpecialityArray, Nothing) Then

                    For iSpecialityCount As Integer = 0 To m_vSpecialityArray.GetUpperBound(0)
                        For iListCount As Integer = 1 To lvwSupSpecAvailable.Items.Count
                            oListItem = lvwSupSpecAvailable.Items.Item(iListCount - 1)
                            'Do we have a match.

                            If Conversion.Val(Convert.ToString(oListItem.Tag)) = CDbl(m_vSpecialityArray(iSpecialityCount)) Then
                                'Set matching item as selected.
                                lvwSupSpecAvailable.FocusedItem = lvwSupSpecAvailable.Items.Item(iListCount - 1)
                                'Call routine to move item.
                                MoveListItem(lvwSupSpecAvailable, lvwSupSpecSelected)
                                Exit For
                            End If
                        Next iListCount

                    Next iSpecialityCount
                End If
            End If
            If lvwSupSpecSelected.Items.Count > 0 Then
                lvwSupSpecSelected.FocusedItem = lvwSupSpecSelected.Items.Item(0)
            End If

            'RWH(16/10/2000) Protected setting of selected item.
            If lvwSupSpecAvailable.Items.Count > 0 Then
                lvwSupSpecAvailable.FocusedItem = lvwSupSpecAvailable.Items.Item(0)
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayAssociatedSpecialities", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayAssociatedSpecialities", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function AddSupplierBusiness() As Integer
        'Adds selected item into ResultArray.

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Redimension result array to include new addition.

            If m_vResultArray Is Nothing Then
                ReDim m_vResultArray(1, 0)
            Else
                ReDim Preserve m_vResultArray(1, m_vResultArray.GetUpperBound(1) + 1)
            End If

            'Add supplier_business_id into Result Array.
            m_vResultArray(0, m_vResultArray.GetUpperBound(1)) = Conversion.Val(Convert.ToString(lvwSupBusAvailable.FocusedItem.Tag))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSupplierBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSupplierBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function AddSupplierSpeciality() As Integer
        'Adds selected speciality into Speciality Array and then re-assigns this
        'into the overall Result Array.

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Redimension speciality array to include new addition.

            If Object.Equals(m_vSpecialityArray, Nothing) Then
                ReDim m_vSpecialityArray(0)
            Else

                ReDim Preserve m_vSpecialityArray(m_vSpecialityArray.GetUpperBound(0) + 1)
            End If

            'Add selected supplier_speciality_id into Speciality Array.


            m_vSpecialityArray(m_vSpecialityArray.GetUpperBound(0)) = Conversion.Val(Convert.ToString(lvwSupSpecAvailable.FocusedItem.Tag))

            'Assign Speciality Array into 2nd element of Result Array for
            'current Supplier Business.

            m_vResultArray(1, m_iCurrentBusIndex) = m_vSpecialityArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddSupplierSpeciality", vApp:=ACApp, vClass:=ACClass, vMethod:="AddSupplierSpeciality", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Function DisplayAssociatedBusinesses() As Integer
        'RWH Routine displays Businesses stored in Result Array in the
        'appropriate ListView. It does this by matching stored ones against
        'the viewed available ones and moving the item from the Available ListView
        'to the Selected one. This ensures the 2 list views are in sync.
        Dim result As Integer = 0

        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not (m_vResultArray Is Nothing) Then
                For iBusinessCount As Integer = 0 To m_vResultArray.GetUpperBound(1)
                    For iListCount As Integer = 1 To lvwSupBusAvailable.Items.Count
                        oListItem = lvwSupBusAvailable.Items.Item(iListCount - 1)
                        'Do we have a match.
                        If Conversion.Val(Convert.ToString(oListItem.Tag)) = CDbl(m_vResultArray(0, iBusinessCount)) Then
                            'Set matching item as selected.
                            lvwSupBusAvailable.FocusedItem = lvwSupBusAvailable.Items.Item(iListCount - 1)
                            'Call routine to move item.
                            MoveListItem(lvwSupBusAvailable, lvwSupBusSelected)
                            Exit For
                        End If
                    Next iListCount

                Next iBusinessCount

                'RWH(16/10/2000) Protected setting of selected item.
                If lvwSupBusSelected.Items.Count > 0 Then
                    lvwSupBusSelected.FocusedItem = lvwSupBusSelected.Items.Item(0)
                End If
            End If

            'RWH(16/10/2000) Protected setting of selected item.
            If lvwSupBusAvailable.Items.Count > 0 Then
                lvwSupBusAvailable.FocusedItem = lvwSupBusAvailable.Items.Item(0)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayAssociatedBusinesses", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayAssociatedBusinesses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function RemoveSupplierSpeciality() As gPMConstants.PMEReturnCode
        'Removes selected speciality from Speciality Array and then re-assigns this
        'into the overall Result Array.
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vTempArray() As Object
        Dim iNewCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iNewCount = 0
            'Copy SpecialityArray into TempArray ommitting item to be deleted.

            For iOldCount As Integer = 0 To m_vSpecialityArray.GetUpperBound(0)

                If CDbl(m_vSpecialityArray(iOldCount)) <> Conversion.Val(Convert.ToString(lvwSupSpecSelected.FocusedItem.Tag)) Then
                    If iNewCount = 0 Then
                        ReDim vTempArray(iNewCount)
                    Else
                        ReDim Preserve vTempArray(iNewCount)
                    End If


                    vTempArray(iNewCount) = m_vSpecialityArray(iOldCount)
                    iNewCount += 1
                End If
            Next iOldCount
            ReDim m_vSpecialityArray(0)

            'Copy new array into Speciality.


            m_vSpecialityArray = vTempArray
            'Re-assign SpecialityArray into second element of ResultArray for
            'the current Business.

            m_vResultArray(1, m_iCurrentBusIndex) = m_vSpecialityArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="RemoveSupplierSpeciality", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveSupplierSpeciality", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function RemoveSupplierBusiness() As Integer
        'RWH -  Removes selected business from Result Array.
        Dim result As Integer = 0
        Dim vTempArray(,) As Object
        Dim iNewCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iNewCount = 0
            'Copy ResultArray into TempArray ommitting item to be deleted.
            For iOldCount As Integer = 0 To m_vResultArray.GetUpperBound(1)
                If CDbl(m_vResultArray(0, iOldCount)) <> Conversion.Val(Convert.ToString(lvwSupBusSelected.FocusedItem.Tag)) Then
                    If iNewCount = 0 Then
                        ReDim vTempArray(1, iNewCount)
                    Else
                        ReDim Preserve vTempArray(1, iNewCount)
                    End If

                    vTempArray(0, iNewCount) = m_vResultArray(0, iOldCount)

                    vTempArray(1, iNewCount) = m_vResultArray(1, iOldCount)
                    iNewCount += 1
                End If
            Next iOldCount
            ReDim m_vResultArray(0, 0)
            'Copy new array into ResultArray.

            m_vResultArray = vTempArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="RemoveSupplierBusiness", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveSupplierBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            lvwAddresses.Items.Clear()

            m_lAddressCount = 0

            ' Assign the details to the interface.
            For i As Integer = m_vAddresses.GetLowerBound(1) To m_vAddresses.GetUpperBound(1)

                ' {* USER DEFINED CODE (Begin) *}

                'Establish the Address Usage.
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

                'RWH(21/07/2000) Check default country to see where Postcode is being displayed.
                Select Case (m_sDefaultCountryCode.Trim())
                    Case "GBR"
                        ' Assign the details to the first column.
                        ' Postcode

                        oListItem = lvwAddresses.Items.Add(CStr(m_vAddresses(0, i)).Trim(), ACIADDRESS)

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

                        oListItem = lvwAddresses.Items.Add(sAddressUsage, ACIADDRESS)

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
            'Developer Guide No. 178
            m_lReturn = ListViewFunc.ListViewAutoSize(lvwList:=lvwAddresses, bSizeHeaders:=True)

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateAddresses", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

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



    'Private Function AddAmpersand(ByRef sString As String) As String
    'Adds an extra ampersand into a string as a single ampersand shows up as
    'an underline to the next character in labels and panels.
    '
    'Dim result As String = String.Empty
    'Dim iPlaceHolder As Integer
    '
    'Try 
    '
    'iPlaceHolder = 1
    'Do While Strings.InStr(iPlaceHolder, sString, "&") > 0
    'iPlaceHolder = Strings.InStr(iPlaceHolder, sString, "&")
    'sString = sString.Substring(0, iPlaceHolder) & "&" & sString.Substring(iPlaceHolder)
    'iPlaceHolder += 2
    'Loop 
    '
    'Return sString
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = ""
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAmpersand", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAmpersand", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    Private Function CheckMandatoryFieldsExtra() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If txtPartyCode.Text.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("You must enter a Party Code", m_sMandatoryFieldsTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtPartyCode.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If txtPartyName.Text.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("You must enter a Party Name", m_sMandatoryFieldsTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtPartyName.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'RWH(16/10/2000) This is apparantly not mandatory.
            '    If lvwSupBusSelected.ListItems.Count = 0 Then
            '        CheckMandatoryFieldsExtra = PMFalse
            '        MsgBox "You must select a business", vbExclamation, m_sMandatoryFieldsTitle
            '        lvwSupBusAvailable.SetFocus
            '        CheckMandatoryFieldsExtra = PMFalse
            '        Exit Function
            '    End If

            'S4B Claims Enhancements R&D 2005 - Address non-mandatory for Other Party
            If lvwAddresses.Items.Count = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                MessageBox.Show("You must enter an address", m_sMandatoryFieldsTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                cmdAddAddress.Focus()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatoryFieldsExtra", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatoryFieldsExtra", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub Toolbar1_ButtonClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _Toolbar1_Button1.Click, _Toolbar1_Button2.Click, _Toolbar1_Button3.Click, _Toolbar1_Button4.Click, _Toolbar1_Button5.Click
        Dim Button As ToolStripItem = CType(eventSender, ToolStripItem)

        Try

            ' Call Toolbar Control function
            If Button.Owner.Items.IndexOf(Button) + 1 <> 4 Then
                Exit Sub
            End If
            'Developer Guide No. (As per VB Code)
            m_lReturn = SIRToolbarFunc.ProcessToolbar(v_iButton:=Button.Owner.Items.IndexOf(Button) + 1, v_lPartyCnt:=m_lPartyCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Continue as not serious
                Exit Sub
            End If

        Catch



            'Continue as not serious
            Exit Sub
        End Try

    End Sub

    Private Sub txtCompanyNotes_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCompanyNotes.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCompanyNotes)
    End Sub

    Private Sub txtCompanyNotes_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCompanyNotes.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCompanyNotes)
    End Sub

    Private Sub txtContactName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtContactName)
    End Sub

    Private Sub txtContactName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtContactName)
    End Sub

    Private Sub txtContactTelephoneNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactTelephoneNo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtContactTelephoneNo)
    End Sub

    Private Sub txtContactTelephoneNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtContactTelephoneNo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtContactTelephoneNo)
    End Sub

    Private Sub txtDateOfBirth_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateOfBirth.Enter

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDateOfBirth)

    End Sub

    Private Sub txtDateOfBirth_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDateOfBirth.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDateOfBirth)

    End Sub

    Private Sub txtDatePassedTest_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatePassedTest.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtDatePassedTest)
    End Sub

    Private Sub txtDatePassedTest_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDatePassedTest.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtDatePassedTest)
    End Sub

    Private Sub txtInsurerContactName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerContactName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerContactName)
    End Sub

    Private Sub txtInsurerContactName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerContactName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerContactName)
    End Sub

    Private Sub txtInsurerEmailAddress_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerEmailAddress.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerEmailAddress)
    End Sub

    Private Sub txtInsurerEmailAddress_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerEmailAddress.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerEmailAddress)
    End Sub

    Private Sub txtInsurerFaxNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerFaxNo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerFaxNo)
    End Sub

    Private Sub txtInsurerFaxNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerFaxNo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerFaxNo)
    End Sub

    Private Sub txtInsurerName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerName)
    End Sub

    Private Sub txtInsurerName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerName)
    End Sub

    Private Sub txtInsurerNotes_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerNotes.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerNotes)
    End Sub

    Private Sub txtInsurerNotes_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerNotes.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerNotes)
    End Sub

    Private Sub txtInsurerTelNo_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerTelNo.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtInsurerTelNo)
    End Sub

    Private Sub txtInsurerTelNo_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInsurerTelNo.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtInsurerTelNo)
    End Sub

    Private Sub txtLicenceNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLicenceNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtLicenceNumber)

    End Sub

    Private Sub txtLicenceNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtLicenceNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtLicenceNumber)

    End Sub

    Private Sub txtPartyCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyCode.Enter
        SSTabHelper.SetSelectedIndex(tabMainTab, 0)

        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPartyCode)
        '    txtPartyCode.SelStart = 0
        '    txtPartyCode.SelLength = Len(txtPartyCode)

    End Sub

    Private Sub txtPartyCode_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtPartyCode.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)


        KeyAscii = Strings.Asc(Strings.Chr(KeyAscii).ToString().ToUpper()(0))

        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtPartyCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPartyCode)

    End Sub

    Private Sub txtPartyName_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyName.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPartyName)
        '    txtPartyName.SelStart = 0
        '    txtPartyName.SelLength = Len(txtPartyName)

    End Sub

    Private Sub txtPartyName_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPartyName.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPartyName)

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
        Dim lPartyCnt As Integer
        Dim sAddressUsage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = m_oBusiness.GetPartyCnt(vPartyRef:=txtPartyCode.Text.Trim(), vPartyCnt:=lPartyCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Unable to access business object", m_sPartyTypeDesc, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lPartyCnt <> 0 Then
                    MessageBox.Show("Party Code already exists", m_sPartyTypeDesc, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

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
                    MessageBox.Show("You must have an address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", m_sPartyTypeDesc, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    Return result
                Case 1
                    'Yes
                Case Else
                    'No.
                    MessageBox.Show("You can have only one address of type '" & gSIRLibrary.SIRMainAddressABIDescription & "'", m_sPartyTypeDesc, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    SSTabHelper.SetSelectedIndex(tabMainTab, 1)
                    Return result
            End Select

            'Changes for WPR-42
            If uctPickListBranches.SelectedItems = 0 Then
                MessageBox.Show("You must attach at least one Branch to the Party record", "Other Party Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(tabMainTab, 8)
                uctPickListBranches.Focus()
                Return result
            End If
            'End Changes for WPR-42

            'Now Ensure addresses are not used twice
            If lvwAddresses.Items.Count < 2 Then
                'less than 2 addresses so cant have duplicates
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
                MessageBox.Show("An address can only be used once by a particular party.", m_sPartyTypeDesc, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateAddressPostCodeProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAddressPostCodeProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub txtReference_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtReference)
    End Sub

    Private Sub txtReference_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtReference.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtReference)
    End Sub

    Private Sub txtRegNumber_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNumber.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtRegNumber)

    End Sub

    Private Sub txtRegNumber_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtRegNumber.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtRegNumber)

    End Sub

    ' ***************************************************************** '
    ' Name: UpdateOrion
    '
    ' Description: Update relevant party on Orion.
    '
    ' RWH (23/07/01) - Taken from uctPartyPCControl (by ECK)
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

    'SD 17/09/2002
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

    ' ***************************************************************** '
    ' Name: PopulatePriorityCombo
    '
    ' Description: Add 1,2,3 to the priority combo box listitems
    '
    ' ***************************************************************** '
    Private Function PopulatePriorityCombo() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With cboPriority
                .Items.Clear()
                .Items.Add(ACConstantBlank)
                .Items.Add(ACComboPriorityHigh)
                .Items.Add(ACComboPriorityMed)
                .Items.Add(ACComboPriorityLow)
                .SelectedIndex = 0
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulatePriorityCombo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePriorityCombo", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PopulateExtraSupplierDetails
    '
    ' Description: Populate the extra details for this supplier
    '
    ' ***************************************************************** '
    Private Function PopulateExtraSupplierDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vPartyDetail) Then
                Return result
            End If

            'Set the active bit indicator in the dropdown. If it is null, set as blank
            If m_vPartyDetail(ACActiveIndicator) = "" Then
                cboActive.SelectedIndex = 0
            ElseIf CBool(m_vPartyDetail(ACActiveIndicator)) Then
                cboActive.SelectedIndex = 1
            ElseIf Not CBool(m_vPartyDetail(ACActiveIndicator)) Then
                cboActive.SelectedIndex = 2
            End If

            'Set the after hours bit indicator in the dropdown. If it is null, set as blank
            If m_vPartyDetail(ACAfterHoursIndicator) = "" Then
                cboAfterHours.SelectedIndex = 0
            ElseIf CBool(m_vPartyDetail(ACAfterHoursIndicator)) Then
                cboAfterHours.SelectedIndex = 1
            ElseIf Not CBool(m_vPartyDetail(ACAfterHoursIndicator)) Then
                cboAfterHours.SelectedIndex = 2
            End If

            'Set the priority indicator in the dropdown. If it is null, set as blank
            If CStr(m_vPartyDetail(ACPriorityIndicator)) = ACComboPriorityHigh Then
                cboPriority.SelectedIndex = 1
            ElseIf CStr(m_vPartyDetail(ACPriorityIndicator)) = ACComboPriorityMed Then
                cboPriority.SelectedIndex = 2
            ElseIf CStr(m_vPartyDetail(ACPriorityIndicator)) = ACComboPriorityLow Then
                cboPriority.SelectedIndex = 3
            Else
                cboPriority.SelectedIndex = 0
            End If
            If CStr(m_vPartyDetail(ACTPASettleDirectly)) = 1 Then
                cboTPASettle.SelectedIndex = 1
            Else
                cboTPASettle.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateExtraSupplierDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateExtraSupplierDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveExtraSupplierDetails
    '
    ' Description: Save the extra details for this supplier. If the party
    ' is not a supplier, then place null values for each detail
    '
    ' ***************************************************************** '
    Private Function SaveExtraSupplierDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsSupplier Then

                If Not Information.IsArray(m_vPartyDetail) Then
                    ReDim m_vPartyDetail(3)
                End If

                'Save the active bit indicator from the dropdown. If it is blank, set as null
                If cboActive.SelectedIndex = 1 Then
                    m_vPartyDetail(ACActiveIndicator) = True
                ElseIf cboActive.SelectedIndex = 2 Then
                    m_vPartyDetail(ACActiveIndicator) = False
                Else

                    m_vPartyDetail(ACActiveIndicator) = DBNull.Value
                End If

                'Save the after hours bit indicator from the dropdown. If it is blank, set as null
                If cboAfterHours.SelectedIndex = 1 Then
                    m_vPartyDetail(ACAfterHoursIndicator) = True
                ElseIf cboAfterHours.SelectedIndex = 2 Then
                    m_vPartyDetail(ACAfterHoursIndicator) = False
                Else

                    m_vPartyDetail(ACAfterHoursIndicator) = DBNull.Value
                End If

                'Save the priority indicator from the dropdown. If it is blank, set as null
                If cboPriority.SelectedIndex = 1 Then
                    m_vPartyDetail(ACPriorityIndicator) = ACComboPriorityHigh
                ElseIf cboPriority.SelectedIndex = 2 Then
                    m_vPartyDetail(ACPriorityIndicator) = ACComboPriorityMed
                ElseIf cboPriority.SelectedIndex = 3 Then
                    m_vPartyDetail(ACPriorityIndicator) = ACComboPriorityLow
                Else

                    m_vPartyDetail(ACPriorityIndicator) = DBNull.Value
                End If

                m_vPartyDetail(ACTPASettleDirectly) = IIf(cboTPASettle.SelectedIndex = 2, 1, 0)
            Else
                ReDim m_vPartyDetail(3)

                m_vPartyDetail(ACActiveIndicator) = DBNull.Value

                m_vPartyDetail(ACAfterHoursIndicator) = DBNull.Value

                m_vPartyDetail(ACPriorityIndicator) = DBNull.Value

                m_vPartyDetail(ACTPASettleDirectly) = IIf(cboTPASettle.SelectedIndex = 1, 1, 0)

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveExtraSupplierDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveExtraSupplierDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'Replaced in place of selectedchangecommitted
    Private Sub uctBranch_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uctBranch.SelectedIndexChanged

        If Not m_bSetBranch Then
            m_lReturn = GetSubBranchDetails(r_oSubBranch:=cboSubBranch, r_oBranch:=uctBranch, r_oBusiness:=m_oBusiness, v_lSubBranchId:=m_vSubBranch)

            If m_bShowSubBranchID Then
                cboSubBranch.SelectedIndex = -1
            Else
                If cboSubBranch.Items.Count > 0 Then
                    cboSubBranch.SelectedIndex = 0
                End If
            End If

            m_lReturn = GetSourceBaseCurrency()
        End If

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

            Dim lIndex, lSourceID As Integer
            Dim vSubBranchArray(,) As Object

            r_oSubBranch.Items.Clear()

            lIndex = r_oBranch.SelectedIndex
            If lIndex < 0 Then
                Return result
            End If

            lSourceID = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)

            If lSourceID <> 0 Then

                m_lReturn = r_oBusiness.GetSubBranches(v_lSourceId:=lSourceID, r_vSubBranchArray:=vSubBranchArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                End If
                If Not Information.IsArray(vSubBranchArray) Then
                    Return result
                End If

                For i As Integer = 0 To vSubBranchArray.GetUpperBound(1)
                    Dim r_oSubBranch_NewIndex As Integer = -1

                    r_oSubBranch_NewIndex = r_oSubBranch.Items.Add(CStr(vSubBranchArray(ACSubBranchDescription, i)))

                    VB6.SetItemData(r_oSubBranch, r_oSubBranch_NewIndex, CInt(vSubBranchArray(ACSubBranchId, i)))

                    If CInt(vSubBranchArray(ACSubBranchId, i)) = v_lSubBranchId Then
                        r_oSubBranch.SelectedIndex = r_oSubBranch_NewIndex
                    End If
                Next i

                If v_lSubBranchId = 0 Then
                    r_oSubBranch.SelectedIndex = -1
                End If
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
        Dim lSourceID As Integer

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

            ' get the SourceID
            'RDT PN 18099
            lSourceID = VB6.GetItemData(uctBranch, uctBranch.SelectedIndex)

            ' this value SHOULD exist, but more error trapping here?

            If lSourceID <> 0 Then
                ' call the business. to get the Base Currency ID

                m_lReturn = oPartyBusiness.GetBaseCurrencyID(lSourceID:=lSourceID, iCurrencyID:=iBaseCurrencyID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get base currency for selected branch", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceBaseCurrency")

                    Return result
                End If

                uctCurrency.CompanyId = lSourceID
                uctCurrency.RefreshList()
                uctCurrency.CurrencyId = iBaseCurrencyID
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

        'Clear combo.
        uctBranch.Items.Clear()

        Dim uctBranch_NewIndex As Integer = -1
        uctBranch_NewIndex = uctBranch.Items.Add("(none)")
        VB6.SetItemData(uctBranch, uctBranch_NewIndex, 0)

        'PN 19428

        m_vSourceArray = vSourceArray

        'Populate branch combo

        'Developer Guide No.161
        For i As Integer = 0 To vSourceArray.GetUpperBound(1)

            'Developer Guide No.162
            uctBranch_NewIndex = uctBranch.Items.Add(CStr(vSourceArray(2, i)).Trim())


            'Developer Guide No.161
            VB6.SetItemData(uctBranch, uctBranch_NewIndex, CInt(vSourceArray(0, i)))

            'Developer Guide No.162
            If CInt(vSourceArray(0, i)) = m_vBranch Then
                uctBranch.SelectedIndex = uctBranch_NewIndex
            End If
        Next i

        uctBranch.SelectedIndex = 0

        'PN 18708/19838 : Updating the hidden Combobox with the selected branch...
        'Branch's default currency will be auto selected...
        'Moved from Interface_Renamed as value needs to be populated before assigning any value
        If m_iSourceID > 0 Then
            'Developer Guide No. 69
            If uctBranch.Items.Count > 0 Then
                'Developer Guide No. 69
                For iCnt As Integer = 0 To uctBranch.Items.Count - 1
                    'Developer Guide No. 69
                    If VB6.GetItemData(uctBranch, iCnt) = m_iSourceID Then
                        'Developer Guide No. 69
                        uctBranch.SelectedIndex = iCnt
                        Exit For
                    End If
                Next
            End If
        End If

        Return result



        ' Error Section.

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the branch details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Public Function GetPartySystemTypeCode(ByVal v_sPartyTypeCode As String) As String


        Return m_oBusiness.GetPartySystemTypeCode(v_sPartyTypeCode)

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

                'm_bDomiciledForTax = gPMFunctions.ToSafeBoolean(CBool(vPartyDetails(kPartyDetailDomiciledForTax, 0)), 0)
                m_bDomiciledForTax = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailDomiciledForTax, 0), 0)

                'm_bTaxExempt = gPMFunctions.ToSafeBoolean(CBool(vPartyDetails(kPartyDetailTaxExempt, 0)), 0)
                m_bTaxExempt = gPMFunctions.ToSafeBoolean(vPartyDetails(kPartyDetailTaxExempt, 0), 0)

                'm_dTaxPercentage = gPMFunctions.ToSafeDouble(CBool(vPartyDetails(kPartyDetailTaxPercentage, 0)), 0)
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
            End If
            'Changes for WPR-42
            ''Party Filtering
            SetPickListPKs()
            uctPickListBranches.Save()
            'End Changes for WPR-42


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

    Private Sub ReNumberTabs(ByRef oTab As TabControl, ByVal sSplitChar As String, ByVal bShortcut As Boolean)

        Dim lNumber, lPos As Integer
        Dim sCaption As String = ""

        For lIndex As Integer = 0 To SSTabHelper.GetTabCount(oTab) - 1
            If SSTabHelper.GetTabVisible(oTab, lIndex) Then

                lNumber += 1
                sCaption = SSTabHelper.GetTabCaption(oTab, lIndex)
                lPos = (sCaption.IndexOf(sSplitChar) + 1)

                If lPos > 0 Then
                    sCaption = (IIf(bShortcut, "", "")) & CStr(lNumber) & sSplitChar & Mid(sCaption, lPos + sSplitChar.Length)
                    SSTabHelper.SetTabCaption(oTab, lIndex, sCaption)
                End If

            End If
        Next lIndex

    End Sub

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
        Dim r_sMaskCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oClientNumber Is Nothing Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError("SetClientCodeCntl", "bSIRPolicyNumMaint.Business instance not created")
                    Return result
                End If
            End If


            m_lReturn = oClientNumber.SendClientReadOnlyDetails(v_sPartyType:="OTHERPARTY", r_bIsReadOnly:=r_bIsReadOnly, r_bIsNumberingSchemeExists:=r_bIsNumberingSchemeExists, r_sMaskCode:=r_sMaskCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                gPMFunctions.RaiseError("SetClientCodeCntl", "SendClientReadOnlyDetails failed")
                Return result
            End If

            m_bIsSetMaskingCode = r_bIsNumberingSchemeExists
            m_bIsReadOnly = r_bIsReadOnly
            m_sMaskCode = r_sMaskCode

            If r_bIsNumberingSchemeExists And r_bIsReadOnly Then
                lblPartyCode.Enabled = False
                txtPartyCode.Enabled = False
            ElseIf r_bIsNumberingSchemeExists And Not r_bIsReadOnly Then
                lblPartyCode.Enabled = True
                txtPartyCode.Enabled = True
            Else
                lblPartyCode.Enabled = True
                txtPartyCode.Enabled = True
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function GeneratePartyCode() As Integer
        Dim result As Integer = 0
        Dim bSIRPolicyNumMaint As Object

        Const kMethodName As String = "GeneratePartyCode"
        Dim sFailureReason, sGeneratedClientCode As String

        Dim oClientNumber As bSIRPolicyNumMaint.Business
        Dim iBranchId As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If m_bIsSetMaskingCode And txtPartyCode.Text = "" Then
                'If m_oClientNumber Is Nothing Then
                Dim temp_oClientNumber As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_oClientNumber, "bSIRPolicyNumMaint.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oClientNumber = temp_oClientNumber

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "bSIRPolicyNumMaint.Business instance not Created")
                    Return result
                End If
                'End If

                sGeneratedClientCode = OtherPartyName
                iBranchId = BranchId


                m_lReturn = oClientNumber.GenerateClientCode(v_sPartyType:="OTHERPARTY", v_iSourceID:=iBranchId, r_sGeneratedClientCode:=sGeneratedClientCode, r_sFailureReason:=sFailureReason, v_sType:="OTHERPARTY", v_sTradeName:=OtherPartyName)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    ' Log Error.
                    gPMFunctions.RaiseError("GeneratePartyCode", "GenerateClientCode Failed ")
                    Return result
                ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then  'Numbering Scheme not set
                    MessageBox.Show("Numbering scheme for Insurer is not set.", "Other Party", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                ElseIf sFailureReason <> "" Then
                    MessageBox.Show(sFailureReason, "Other Party", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                txtPartyCode.Text = sGeneratedClientCode
                lblPartyCode.Enabled = False
                txtPartyCode.Enabled = False
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
                    If uctBranch.SelectedIndex < 0 Or BranchId < 1 Then
                        MessageBox.Show("Please select some Branch", "field - Branch", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If
                End If

                ' Last Name, First Name
                If (m_sMaskCode.IndexOf("L"c) >= 0) Or (m_sMaskCode.IndexOf("F"c) >= 0) Or (m_sMaskCode.IndexOf("N"c) >= 0) Or (m_sMaskCode.IndexOf("I"c) >= 0) Or (m_sMaskCode.IndexOf("T"c) >= 0) Then
                    If txtPartyName.Text = "" Then
                        MessageBox.Show("Please Enter Name", "field - Name", MessageBoxButtons.OK, MessageBoxIcon.Error)
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


    'Private Sub uctPartyBankControl1_RefreshBankDetails(ByRef vBankDetails( ,  ) As Object)
    'm_vPartyBankDetails = vBankDetails
    'End Sub

    'Party Bank Details
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


            'cmdRates_Click

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show rates", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowRates", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    'Changes for WPR-42
    Private Sub SetPickListPKs()

        uctPickListBranches.ForeignKeys.Item("PartyCnt").Value = m_lPartyCnt

        uctPickListBranches.ForeignKeys.Item("Branchid").Value = 1

        uctPickListBranches.ForeignKeys.Item("UserId").Value = Nothing
        uctPickListBranches.ForeignKeys.Item("UniqueId").Value = m_sUniqueId
        uctPickListBranches.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy
    End Sub
    Private Sub uctPickListBranches_Change(ByVal Sender As Object, ByVal e As uctPickList.PickList.ChangeEventArgs) Handles uctPickListBranches.Change
        If e.IsEmpty = False Then
            If e.Action = uctPickList.PickList.ChangeAction.Delete Or e.Action = uctPickList.PickList.ChangeAction.DeleteAll Then
                e.Cancel = IIf(MsgBox("You are attempting to remove Branch Access for this Party. Confirmation of this action will result in all System Users assigned to this Party losing access to the same System Branch.Do you wish to proceed?", MsgBoxStyle.YesNo + MsgBoxStyle.Critical, "Warning") = MsgBoxResult.Yes, False, True)
            End If
        End If
    End Sub

    Private Sub frmInterface_Click(sender As Object, e As EventArgs) Handles Me.Click

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
    'End Changes for WPR-42
End Class
