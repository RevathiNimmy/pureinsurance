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


    Public Const vbFormCode As Integer = 0
    ' Constant for the functions to identify

    'VB 08/02/2005 PN 10511 to disable the Apply Button when apply button was clicked
    'And enable ApplyButton when any changes occur while data entry

    'Rule set field position
    Private Const ACFieldPosRiskTypeRuleSetID As Integer = 0
    'private Const ACFieldPosCaptionID as Long = 1
    Private Const ACFieldPosCode As Integer = 2
    Private Const ACFieldPosDescription As Integer = 3
    Private Const ACFieldPosIsDeleted As Integer = 4
    Private Const ACFieldPosEffectiveDate As Integer = 5
    'private Const ACFieldPosRiskTypeID as Long = 6
    Private Const ACFieldPosFileName As Integer = 7
    Private Const ACFieldPosLive As Integer = 8
    Private Const ACFieldPosType As Integer = 9
    Private Const ACFieldPosTypeID As Integer = 10
    Private Const ACFieldPosDREExecutorURL As Integer = 11
    Private Const ACFieldPosDREDefaultToken As Integer = 12


    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' PUBLIC Data Members (Begin)
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
    Private m_sStepStatus As String = ""

    Private m_vRiskTypeList(,) As Object

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMURiskType.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    'Declare an instance of RuleEditor

    Private m_oRiskTypeRuleSet As iPMURiskTypeRuleSet.Interface_Renamed

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' {* USER DEFINED CODE (Begin) *}
    Private m_bApplyButton As Boolean

    Private m_lRiskTypeID As Integer
    Private m_sCode As String = ""

    Private m_vDescription As Object
    Private m_dtRiskTypeEffectiveDate As Date
    Private m_vShareWithCoInsurer As Object
    Private m_vShareWithReInsurer As Object
    Private m_vSuppressPublicText As Object
    Private m_vSuppressPrivateText As Object
    Private m_vSuppressTaxes As Object
    Private m_vReportPointer As Object
    Private m_vSectionMask As Object
    Private m_vStampDutyRate1 As Object
    Private m_vStampDutyRate2 As Object
    Private m_vPrimarySort As Object
    Private m_vSecondarySort As Object
    Private m_vHeaderClause As Object
    Private m_vTrailerClause As Object
    Private m_vIsRiAtRiskLevel As Object
    Private m_vIsAutoReinsured As Object
    Private m_vHeaderClauseId As Object
    Private m_vTrailerClauseId As Object
    Private m_vAccumulationLevel As Object
    Private m_vGISScreenId As Object
    Private m_vHeaderClauseDescription As Object
    Private m_vTrailerClauseDescription As Object
    Private m_vIsDeferredRIPermitted As Object
    Private m_vClaimsIsPostTaxes As Object
    Private m_vDisplayReinsurance As Object
    Private m_vDisplayClaimReinsurance As Object
    'end

    Private m_vGISScreen As Object 'All Gis screen
    Private m_vClauses As Object 'GISScreen which are linked to current risk type
    Private m_vRiskTypeGroup(,) As Object 'All risk type groups
    Private m_vLinkedRiskTypeGroup(,) As Object 'risk type groups which are linked to current risk type
    Private m_vRule(,) As Object 'all rules

    Private m_vRenewalRule(,) As Object 'renewal rules
    Private m_vRenewalLapseRule(,) As Object 'Lapse rule

    Private m_bAllowRatingSectionAdd As Boolean
    Private m_bAllowRatingSectionEdit As Boolean
    Private m_bAllowRatingSectionDelete As Boolean
    Private m_bAllowEditRatingSectionRateType As Boolean
    Private m_bAllowEditRatingSectionRate As Boolean
    Private m_bAllowEditRatingSectionSumInsured As Boolean
    Private m_bAllowEditRatingSectionThisPremium As Boolean

    Private m_bAttachClaimOutsideOfPolicyPeriod As Boolean
    Private m_lClaimsTypeBasis As Integer
    Private m_lClaimsCoverBasis As Integer


    ' Display Insurer/Reinsurer
    Private m_sUnderwritingType As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    ' Constants for listview icons
    Private Const LISTVIEW_ICON_TICK As String = "Tick"
    Private Const LISTVIEW_ICON_RULEFILE As String = "RuleFile"
    Private Const LISTVIEW_ICON_BLANK As String = "Blank"

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


    'USER DEFINED PUBLIC PROPERTY (End)

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

    Public WriteOnly Property RiskTypeList() As Object
        'Set(ByVal Value() As Object)
        Set(ByVal Value As Object)

            m_vRiskTypeList = Value

        End Set
    End Property




    'USER DEFINED PUBLIC PROPERTY (Begin)

    Public Property RiskTypeID() As Integer
        Get
            Return m_lRiskTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeID = Value
        End Set
    End Property

    Public ReadOnly Property Code() As String
        Get
            Return m_sCode
        End Get
    End Property

    Public ReadOnly Property Description() As Object
        Get
            Return m_vDescription
        End Get
    End Property

    Public ReadOnly Property RiskTypeEffectiveDate() As Date
        Get
            Return m_dtRiskTypeEffectiveDate
        End Get
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
            '        If (m_lReturn <> PMTrue) Then
            '          SetFieldValidation = PMFalse
            '          Exit Function
            '        End If
            '
            ' NOTE: Replace this section with your new code.
            ' ************************************************************

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtEffectiveDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Alix - 22/11/2002 - Issue 1386
            'm_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=txtReportPointer, _
            'lFieldType:=PMLong, _
            'lFormat:=PMFormatString, _
            'lMandatory:=PMNonMandatory)

            'If (m_lReturn& <> PMTrue) Then
            '    SetFieldValidation = PMFalse
            '    Exit Function
            'End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSectionMask, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtStampDutyRate1, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtStampDutyRate2, lFieldType:=gPMConstants.PMEDataType.PMDouble, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPrimarySort, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSecondarySort, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtAccumulationLevel, lFieldType:=gPMConstants.PMEDataType.PMLong, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    m_lReturn& = m_oFormFields.AddNewFormField( _
            'ctlControl:=txtTrailerClause, _
            'lFieldType:=PMString, _
            'lFormat:=PMFormatString, _
            'lMandatory:=PMNonMandatory)

            '    If (m_lReturn& <> PMTrue) Then
            '        SetFieldValidation = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtFormatDate, lFieldType:=gPMConstants.PMEDataType.PMDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateLong, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' {* USER DEFINED CODE (End) *}


            Return gPMConstants.PMEReturnCode.PMTrue

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


        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oResultArray(,) As Object = Nothing

        Try



            ' Get the details from the business object.

            'get risk type details

            m_lReturn = m_oBusiness.GetRiskTypeDetails(m_lRiskTypeID, oResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(oResultArray) Then
                'if can't find risk type details then there must be something wrong
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lRiskTypeID = CInt(oResultArray(ACIrisk_type_id, 0))

            m_sCode = CStr(oResultArray(ACICode, 0))

            m_vDescription = CStr(oResultArray(ACIDescription, 0))

            m_dtRiskTypeEffectiveDate = CDate(oResultArray(ACIeffective_date, 0))

            m_vSectionMask = CStr(oResultArray(ACIvsection_mask, 0))

            m_vStampDutyRate1 = CStr(oResultArray(ACIstamp_duty_rate1, 0))

            m_vStampDutyRate2 = CStr(oResultArray(ACIstamp_duty_rate2, 0))

            m_vPrimarySort = CStr(oResultArray(ACIprimary_sort, 0))

            m_vSecondarySort = CStr(oResultArray(ACIsecondary_sort, 0))

            m_vHeaderClause = CStr(oResultArray(ACIheader_clause, 0))

            m_vTrailerClause = CStr(oResultArray(ACItrailer_clause, 0))

            m_vHeaderClauseId = CStr(oResultArray(ACIheader_clause_id, 0))

            m_vTrailerClauseId = CStr(oResultArray(ACItrailer_clause_id, 0))

            m_vHeaderClauseDescription = CStr(oResultArray(ACIheader_clause_description, 0))

            m_vTrailerClauseDescription = CStr(oResultArray(ACItrailer_clause_description, 0))

            m_vAccumulationLevel = CStr(oResultArray(ACIaccumulation_level, 0))

            m_vGISScreenId = CStr(oResultArray(ACIgis_screen_id, 0))


            If CStr(oResultArray(ACIis_share_with_co_insurers, 0)) = "" Then
                m_vShareWithCoInsurer = CStr(0)
            Else

                m_vShareWithCoInsurer = CStr(oResultArray(ACIis_share_with_co_insurers, 0))
            End If


            If CStr(oResultArray(ACIis_share_with_re_insurers, 0)) = "" Then
                m_vShareWithReInsurer = CStr(0)
            Else

                m_vShareWithReInsurer = CStr(oResultArray(ACIis_share_with_re_insurers, 0))
            End If


            If CStr(oResultArray(ACIis_suppress_public_text, 0)) = "" Then
                m_vSuppressPublicText = CStr(0)
            Else

                m_vSuppressPublicText = CStr(oResultArray(ACIis_suppress_public_text, 0))
            End If


            If CStr(oResultArray(ACIis_suppress_private_text, 0)) = "" Then
                m_vSuppressPrivateText = CStr(0)
            Else

                m_vSuppressPrivateText = CStr(oResultArray(ACIis_suppress_private_text, 0))
            End If


            If CStr(oResultArray(ACIis_suppress_taxes, 0)) = "" Then
                m_vSuppressTaxes = CStr(0)
            Else

                m_vSuppressTaxes = CStr(oResultArray(ACIis_suppress_taxes, 0))
            End If


            If CStr(oResultArray(ACIis_ri_at_risk_level, 0)) = "" Then
                m_vIsRiAtRiskLevel = CStr(0)
            Else

                m_vIsRiAtRiskLevel = CStr(oResultArray(ACIis_ri_at_risk_level, 0))
            End If


            If CStr(oResultArray(ACIis_auto_reinsured, 0)) = "" Then
                m_vIsAutoReinsured = CStr(0)
            Else

                m_vIsAutoReinsured = CStr(oResultArray(ACIis_auto_reinsured, 0))
            End If


            If CStr(oResultArray(ACIis_deferred_ri_permitted, 0)) = "" Then
                m_vIsDeferredRIPermitted = CStr(gPMConstants.PMEReturnCode.PMFalse)
            Else

                m_vIsDeferredRIPermitted = CStr(oResultArray(ACIis_deferred_ri_permitted, 0))
            End If


            If CStr(oResultArray(ACIclaims_is_post_taxes, 0)) = "" Then
                m_vClaimsIsPostTaxes = CStr(gPMConstants.PMEReturnCode.PMFalse)
            Else

                m_vClaimsIsPostTaxes = CStr(oResultArray(ACIclaims_is_post_taxes, 0))
            End If


            If CStr(oResultArray(ACIdisplay_Reinsurance, 0)) = "" Then
                m_vDisplayReinsurance = CStr(gPMConstants.PMEReturnCode.PMFalse)
            Else

                m_vDisplayReinsurance = CStr(oResultArray(ACIdisplay_Reinsurance, 0))
            End If


            If CStr(oResultArray(ACIAllowRatingSectionAdd, 0)) = "" Then
                m_bAllowRatingSectionAdd = gPMConstants.PMEReturnCode.PMFalse
            Else

                m_bAllowRatingSectionAdd = CBool(oResultArray(ACIAllowRatingSectionAdd, 0))
            End If


            If CStr(oResultArray(ACIAllowRatingSectionEdit, 0)) = "" Then
                m_bAllowRatingSectionEdit = gPMConstants.PMEReturnCode.PMFalse
            Else

                m_bAllowRatingSectionEdit = CBool(oResultArray(ACIAllowRatingSectionEdit, 0))
            End If

            If CStr(oResultArray(ACIAllowRatingSectionDelete, 0)) = "" Then
                m_bAllowRatingSectionDelete = gPMConstants.PMEReturnCode.PMFalse
            Else

                m_bAllowRatingSectionDelete = CBool(oResultArray(ACIAllowRatingSectionDelete, 0))
            End If


            If CStr(oResultArray(ACIAllowEditRatingSectionRateType, 0)) = "" Then
                m_bAllowEditRatingSectionRateType = gPMConstants.PMEReturnCode.PMFalse
            Else

                m_bAllowEditRatingSectionRateType = CBool(oResultArray(ACIAllowEditRatingSectionRateType, 0))
            End If


            If CStr(oResultArray(ACIAllowEditRatingSectionRate, 0)) = "" Then
                m_bAllowEditRatingSectionRate = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_bAllowEditRatingSectionRate = CBool(oResultArray(ACIAllowEditRatingSectionRate, 0))
            End If


            If CStr(oResultArray(ACIAllowEditRatingSectionSumInsured, 0)) = "" Then
                m_bAllowEditRatingSectionSumInsured = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_bAllowEditRatingSectionSumInsured = CBool(oResultArray(ACIAllowEditRatingSectionSumInsured, 0))
            End If


            If CStr(oResultArray(ACIAllowEditRatingSectionThisPremium, 0)) = "" Then
                m_bAllowEditRatingSectionThisPremium = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_bAllowEditRatingSectionThisPremium = CBool(oResultArray(ACIAllowEditRatingSectionThisPremium, 0))
            End If


            If CStr(oResultArray(ACIdisplay_ClaimReinsurance, 0)) = "" Then
                m_vDisplayClaimReinsurance = CStr(gPMConstants.PMEReturnCode.PMFalse)
            Else
                m_vDisplayClaimReinsurance = CStr(oResultArray(ACIdisplay_ClaimReinsurance, 0))
            End If

            If CStr(oResultArray(kAttachClaimOutsideOfPolicyPeriod, 0)) = "" Then
                m_bAttachClaimOutsideOfPolicyPeriod = CStr(gPMConstants.PMEReturnCode.PMFalse)
            Else
                m_bAttachClaimOutsideOfPolicyPeriod = CStr(oResultArray(kAttachClaimOutsideOfPolicyPeriod, 0))
            End If

            m_lClaimsTypeBasis = gPMFunctions.ToSafeLong(oResultArray(ACIClaimsTypeBasis, 0))
            m_lClaimsCoverBasis = gPMFunctions.ToSafeLong(oResultArray(ACIClaimsCoverBasis, 0))


            'get allowed GIS Screen for current risk type code

            m_lReturn = m_oBusiness.GetClauses(m_lRiskTypeID, m_vClauses)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            'get risk type group for current risk type code

            m_lReturn = m_oBusiness.GetRiskTypeGroup(m_lRiskTypeID, m_vLinkedRiskTypeGroup)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            'get all rules associated to current risk type

            m_lReturn = m_oBusiness.GetAllRiskTypeRuleSet(v_lRiskTypeID:=m_lRiskTypeID, v_sType:="RT", r_vResultArray:=m_vRule)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)

            m_lReturn = uctSIRSelectClauses.Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctSIRSelectClauses.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return nResult
            End If
            uctSIRSelectClauses.ProductId = ACSelectClauseDefaultProductID
            uctSIRSelectClauses.RiskId = m_lRiskTypeID
            uctSIRSelectClauses.ClauseId = MainModule.ENClauseType.RiskType

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            m_sScreenHierarchy = $"Risk Type({m_sCode.Trim()})"
            uctSIRSelectClauses.UniqueId = m_sUniqueId
            uctSIRSelectClauses.ScreenHierarchy = m_sScreenHierarchy

            m_lReturn = uctSIRSelectClauses.Load_Renamed()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="uctSIRSelectClauses.Load Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return nResult
            End If
            uctSIRSelectClauses.Dispose()

            'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)

            'get all renewal rule for this risk type

            m_lReturn = m_oBusiness.GetAllRiskTypeRuleSet(v_lRiskTypeID:=m_lRiskTypeID, v_sType:="RN", r_vResultArray:=m_vRenewalRule)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            ''get all Lapse rule for this risk type
            m_lReturn = m_oBusiness.GetAllRiskTypeRuleSet(v_lRiskTypeID:=m_lRiskTypeID, v_sType:="RL", r_vResultArray:=m_vRenewalLapseRule)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If


            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult



            Return nResult
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

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try



            ' Update the interface details.

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtCode, vControlValue:=m_sCode)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=m_dtRiskTypeEffectiveDate)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtDescription, vControlValue:=m_vDescription)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Alix - 22/11/2002 - Issue 1386
            'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtReportPointer, _
            'vControlValue:=m_vReportPointer)
            ' Check for errors
            'If (m_lReturn& <> PMTrue) Then
            ' Failed to assign the data.
            '    BusinessToInterface = PMFalse
            '    Exit Function
            'End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSectionMask, vControlValue:=m_vSectionMask)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtStampDutyRate1, vControlValue:=m_vStampDutyRate1)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtStampDutyRate2, vControlValue:=m_vStampDutyRate2)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtPrimarySort, vControlValue:=m_vPrimarySort)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSecondarySort, vControlValue:=m_vSecondarySort)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtAccumulationLevel, vControlValue:=m_vAccumulationLevel)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Here
            '    If m_vGISScreenId <> "" Then
            '        cboGISScreenId.DefaultItemId = m_vGISScreenId
            '        cboGISScreenId.RefreshList
            '    End If

            If m_vGISScreenId = "" Then
                cboGISScreenID.SelectedIndex = -1
            Else
                For lTemp As Integer = 0 To cboGISScreenID.Items.Count - 1
                    If VB6.GetItemData(cboGISScreenID, lTemp) = StringsHelper.ToDoubleSafe(m_vGISScreenId) Then
                        cboGISScreenID.SelectedIndex = lTemp
                        Exit For
                    End If
                Next lTemp
            End If

            lblHeaderClause.Text = m_vHeaderClauseDescription
            lblTrailerClause.Text = m_vTrailerClauseDescription

            chkShareWithCoInsurer.CheckState = CInt(m_vShareWithCoInsurer)
            chkShareWithReInsurer.CheckState = CInt(m_vShareWithReInsurer)
            chkSuppressPrivateText.CheckState = CInt(m_vSuppressPrivateText)
            chkSuppressPublicText.CheckState = CInt(m_vSuppressPublicText)
            chkSuppressTaxes.CheckState = CInt(m_vSuppressTaxes)
            chkIsAutoReinsured.CheckState = CInt(m_vIsAutoReinsured)

            chkDeferredRI.CheckState = CInt(m_vIsDeferredRIPermitted)
            chkClaimsIsPostTaxes.CheckState = CInt(m_vClaimsIsPostTaxes)
            chkDisplayReinsurance.CheckState = CInt(m_vDisplayReinsurance)
            chkDisplayClaimReinsurance.CheckState = CInt(m_vDisplayClaimReinsurance)

            chkAttachClaimOutsideOfPolicyPeriod.CheckState = IIf(m_bAttachClaimOutsideOfPolicyPeriod, 1, 0)
            '' QBENZ022
            chkAllowAddRatingSection.CheckState = IIf(m_bAllowRatingSectionAdd, 1, 0)
            chkAllowEditRatingSection.CheckState = IIf(m_bAllowRatingSectionEdit, 1, 0)
            fraEdit.Enabled = chkAllowEditRatingSection.CheckState
            chkAllowDeleteRatingSection.CheckState = IIf(m_bAllowRatingSectionDelete, 1, 0)

            chkAllowEditRatingSectionRateType.CheckState = IIf(m_bAllowEditRatingSectionRateType, 1, 0)
            chkAllowEditRatingSectionRate.CheckState = IIf(m_bAllowEditRatingSectionRate, 1, 0)
            chkAllowEditRatingSectionSumInsured.CheckState = IIf(m_bAllowEditRatingSectionSumInsured, 1, 0)
            chkAllowEditRatingSectionThisPremium.CheckState = IIf(m_bAllowEditRatingSectionThisPremium, 1, 0)

            cboClaimsTypeBasis.ItemId = m_lClaimsTypeBasis
            cboClaimsCoverBasis.ItemId = m_lClaimsCoverBasis


            'get tag allowed gis screen
            '    If (TagAllowedGISScreen() <> PMTrue) Then
            '        If (m_lReturn& <> PMTrue) Then
            '            BusinessToInterface = PMFalse
            '            Exit Function
            '        End If
            '    End If
            'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
            ' m_lReturn = LoadClauses()
            'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
            '    If (lvwClauses.ListItems.Count > 0) Then
            '        cmdSelectClause.Enabled = True
            '    Else
            '        cmdSelectClause.Enabled = False
            '    End If
            '    m_lReturn = TagClauses()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        BusinessToInterface = PMFalse
            '    End If
            ' End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)

            'get tag linked risk type group
            If TagRiskTypeGroup() <> gPMConstants.PMEReturnCode.PMTrue Then
                '        If (m_lReturn& <> PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
                '        End If
            End If

            'load rules
            If LoadRules(v_vDataArray:=m_vRule, r_lvwListView:=lvwRules) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'load renewal rules
            If LoadRules(v_vDataArray:=m_vRenewalRule, r_lvwListView:=lvwRenewalRule) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'load Lapse rules
            If LoadRules(v_vDataArray:=m_vRenewalLapseRule, r_lvwListView:=lvwRenewalLapseRule) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult



            Return nResult
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

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try


            ' Assign the details from the interface to the data storage.
            m_lReturn = InterfaceToData()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the data.
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the business object.
            m_sScreenHierarchy = $"Risk Code({m_sCode.Trim()})"
            m_lReturn = m_oBusiness.UpdateRiskType(v_iTask:=m_iTask, r_lRiskTypeID:=m_lRiskTypeID, v_sCode:=m_sCode, v_vDescription:=m_vDescription, v_dtEffectiveDate:=m_dtRiskTypeEffectiveDate, v_vShareWithCoInsurer:=m_vShareWithCoInsurer, v_vShareWithReInsurer:=m_vShareWithReInsurer, v_vSuppressPublicText:=m_vSuppressPublicText, v_vSuppressPrivateText:=m_vSuppressPrivateText, v_vSuppressTaxes:=m_vSuppressTaxes, v_vReportPointer:=m_vReportPointer, v_vSectionMask:=m_vSectionMask, v_vStampDutyRate1:=m_vStampDutyRate1, v_vStampDutyRate2:=m_vStampDutyRate2, v_vPrimarySort:=m_vPrimarySort, v_vSecondarySort:=m_vSecondarySort, v_vHeaderClause:=m_vHeaderClause, v_vTrailerClause:=m_vTrailerClause, v_vIsRiAtRiskLevel:=m_vIsRiAtRiskLevel, v_vIsAutoReinsured:=m_vIsAutoReinsured, v_vHeaderClauseId:=m_vHeaderClauseId, v_vTrailerClauseId:=m_vTrailerClauseId, v_vAccumulationLevel:=m_vAccumulationLevel, v_vGISScreenId:=m_vGISScreenId, v_vClauses:=m_vClauses, r_vLinkedRiskTypeGroup:=m_vLinkedRiskTypeGroup, v_vIsDeferredRIPermitted:=m_vIsDeferredRIPermitted, v_vClaimsIsPostTaxes:=m_vClaimsIsPostTaxes, v_vDisplayReinsurance:=m_vDisplayReinsurance, v_vAllowRatingSectionAdd:=m_bAllowRatingSectionAdd, v_vAllowRatingSectionEdit:=m_bAllowRatingSectionEdit, v_vAllowRatingSectionDelete:=m_bAllowRatingSectionDelete, v_vAllowEditRatingSectionRateType:=m_bAllowEditRatingSectionRateType, v_vAllowEditRatingSectionRate:=m_bAllowEditRatingSectionRate, v_vAllowEditRatingSectionSumInsured:=m_bAllowEditRatingSectionSumInsured, v_vAllowEditRatingSectionThisPremium:=m_bAllowEditRatingSectionThisPremium, v_vDisplayClaimReinsurance:=m_vDisplayClaimReinsurance, v_lClaimsTypeBasis:=m_lClaimsTypeBasis, v_lClaimsCoverBasis:=m_lClaimsCoverBasis, oAttachClaimOutsideOfPolicyPeriod:=m_bAttachClaimOutsideOfPolicyPeriod, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)


    ' ***************************************************************** '
    ' Name: InterfaceToData
    '
    ' Description: Updates the data storage from the interface details.
    '
    ' ***************************************************************** '
    Private Function InterfaceToData() As Integer


        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
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


            m_sCode = CStr(m_oFormFields.UnformatControl(ctlControl:=txtCode))

            m_dtRiskTypeEffectiveDate = CDate(m_oFormFields.UnformatControl(ctlControl:=txtEffectiveDate))

            If txtDescription.Text = "" Then

                m_vDescription = Nothing
            Else

                m_vDescription = CStr(m_oFormFields.UnformatControl(ctlControl:=txtDescription))
            End If

            m_vReportPointer = DBNull.Value

            If txtSectionMask.Text = "" Then

                m_vSectionMask = Nothing
            Else

                m_vSectionMask = CStr(m_oFormFields.UnformatControl(ctlControl:=txtSectionMask))
            End If

            If txtStampDutyRate1.Text = "" Then

                m_vStampDutyRate1 = Nothing
            Else

                m_vStampDutyRate1 = CStr(m_oFormFields.UnformatControl(ctlControl:=txtStampDutyRate1))
            End If

            If txtStampDutyRate2.Text = "" Then

                m_vStampDutyRate2 = Nothing
            Else

                m_vStampDutyRate2 = CStr(m_oFormFields.UnformatControl(ctlControl:=txtStampDutyRate2))
            End If

            If txtPrimarySort.Text = "" Then

                m_vPrimarySort = Nothing
            Else

                m_vPrimarySort = CStr(m_oFormFields.UnformatControl(ctlControl:=txtPrimarySort))
            End If

            If txtSecondarySort.Text = "" Then

                m_vSecondarySort = Nothing
            Else

                m_vSecondarySort = CStr(m_oFormFields.UnformatControl(ctlControl:=txtSecondarySort))
            End If

            If txtAccumulationLevel.Text = "" Then

                m_vAccumulationLevel = Nothing
            Else

                m_vAccumulationLevel = CStr(m_oFormFields.UnformatControl(ctlControl:=txtAccumulationLevel))
            End If

            If cboGISScreenID.SelectedIndex = -1 Then

                m_vGISScreenId = Nothing
            Else
                m_vGISScreenId = CStr(VB6.GetItemData(cboGISScreenID, cboGISScreenID.SelectedIndex))
            End If

            m_vShareWithCoInsurer = CStr(chkShareWithCoInsurer.CheckState)
            m_vShareWithReInsurer = CStr(chkShareWithReInsurer.CheckState)
            m_vSuppressPublicText = CStr(chkSuppressPublicText.CheckState)
            m_vSuppressPrivateText = CStr(chkSuppressPrivateText.CheckState)
            m_vSuppressTaxes = CStr(chkSuppressTaxes.CheckState)

            m_vIsAutoReinsured = CStr(chkIsAutoReinsured.CheckState)

            m_vIsDeferredRIPermitted = CStr(chkDeferredRI.CheckState)
            m_vClaimsIsPostTaxes = CStr(chkClaimsIsPostTaxes.CheckState)
            m_vDisplayReinsurance = CStr(chkDisplayReinsurance.CheckState)
            m_vDisplayClaimReinsurance = CStr(chkDisplayClaimReinsurance.CheckState)

            m_bAllowRatingSectionAdd = chkAllowAddRatingSection.CheckState
            m_bAllowRatingSectionEdit = chkAllowEditRatingSection.CheckState
            m_bAllowRatingSectionDelete = chkAllowDeleteRatingSection.CheckState

            m_bAllowEditRatingSectionRateType = chkAllowEditRatingSectionRateType.CheckState
            m_bAllowEditRatingSectionRate = chkAllowEditRatingSectionRate.CheckState
            m_bAllowEditRatingSectionSumInsured = chkAllowEditRatingSectionSumInsured.CheckState
            m_bAllowEditRatingSectionThisPremium = chkAllowEditRatingSectionThisPremium.CheckState

            m_bAttachClaimOutsideOfPolicyPeriod = chkAttachClaimOutsideOfPolicyPeriod.CheckState

            If m_vHeaderClauseId Is Nothing OrElse String.IsNullOrEmpty(CStr(m_vHeaderClauseId)) Then
                m_vHeaderClauseId = Nothing
            ElseIf IsNumeric(m_vHeaderClauseId) Then
                m_vHeaderClauseId = Convert.ToInt32(m_vHeaderClauseId)
            Else
                m_vHeaderClauseId = Nothing
            End If

            If String.IsNullOrEmpty(m_vHeaderClause) Or m_vHeaderClause = "" Then
                m_vHeaderClause = Nothing
            End If

            If m_vTrailerClauseId Is Nothing OrElse String.IsNullOrEmpty(CStr(m_vTrailerClauseId)) Then
                m_vTrailerClauseId = Nothing
            ElseIf IsNumeric(m_vTrailerClauseId) Then
                m_vTrailerClauseId = Convert.ToInt32(m_vTrailerClauseId)
            Else
                m_vTrailerClauseId = Nothing
            End If

            If String.IsNullOrEmpty(m_vTrailerClause) Or m_vTrailerClause = "" Then
                m_vTrailerClause = Nothing
            End If

            m_lClaimsTypeBasis = cboClaimsTypeBasis.ItemId
            m_lClaimsCoverBasis = cboClaimsCoverBasis.ItemId


            'store selected GIS Screen to data storage
            'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
            'm_lReturn = UnLoadClauses()
            'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)

            'store selected risk type group to data storage
            m_lReturn = UnloadRiskTypeGroup()

            ' {* USER DEFINED CODE (End) *}

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to assign the data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

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

            'default to first tab
            SSTabHelper.SetSelectedIndex(tabMainTab, 0)
            '    tabMainTab.TabVisible(1) = False

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

            ' {* USER DEFINED CODE (Begin) *}

            'default values when adding
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtEffectiveDate, vControlValue:=DateTime.Today)

                ' Alix - 22/11/2002 - Issue 1386
                'm_lReturn = m_oFormFields.FormatControl(ctlControl:=txtReportPointer, _
                'vControlValue:=0)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtSectionMask, vControlValue:=0)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtStampDutyRate1, vControlValue:=0.0#)

                m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtStampDutyRate2, vControlValue:=0.0#)

                'hide rule tab - only available if risk type exists
                SSTabHelper.SetTabVisible(Me.tabMainTab, 3, False)
                SSTabHelper.SetTabCaption(Me.tabMainTab, 4, "4 - Rating Section")
            End If

            'get all risk GISScreen

            m_vGISScreen = Nothing


            m_lReturn = m_oBusiness.GetAllGISScreen(m_vGISScreen)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            cboGISScreenID.Items.Clear()

            If Information.IsArray(m_vGISScreen) Then

                For lTemp As Integer = m_vGISScreen.GetLowerBound(1) To m_vGISScreen.GetUpperBound(1)
                    Dim cboGISScreenID_NewIndex As Integer = -1

                    cboGISScreenID_NewIndex = cboGISScreenID.Items.Add(CStr(m_vGISScreen(2, lTemp)))

                    VB6.SetItemData(cboGISScreenID, cboGISScreenID_NewIndex, CInt(m_vGISScreen(0, lTemp)))
                Next lTemp
            End If

            '    m_lReturn = LoadClauses()
            '
            '    If (m_lReturn <> PMTrue) Then
            '        SetInterfaceDefaults = PMFalse
            '    End If

            '    If (lvwClauses.ListItems.Count > 0) Then
            '        cmdSelectClause.Enabled = True
            '    Else
            '        cmdSelectClause.Enabled = False
            '    End If

            'get all risk type group

            m_lReturn = m_oBusiness.GetAllRiskTypeGroup(m_vRiskTypeGroup)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If LoadRiskTypeGroup() <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' AMB 07/05/2003 - let's do this for the clauses listview also, shall we?
            'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
            '    If (lvwClauses.ListItems.Count > 0) Then
            '        cmdSelectClause.Enabled = True
            '    Else
            '        cmdSelectClause.Enabled = False
            '    End If
            'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)

            cmdSelectRiskTypeGroup.Enabled = (lvwRiskTypeGroup.Items.Count > 0)

            cmdRIModel.Enabled = m_iTask = gPMConstants.PMEComponentAction.PMEdit


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

            m_ctlTabFirstLast(ACControlStart, 0) = txtCode
            m_ctlTabFirstLast(ACControlEnd, 0) = cmdTrailerClause

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
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                    "Please check the file exists and the correct captions are available", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions")

                Return result
            End If

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdEditRule.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdAddRule.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdDeleteRule.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            If m_sUnderwritingType = "1" Then
                chkIsAutoReinsured.Text = "Auto Insured:"
                chkShareWithReInsurer.Text = "Share Tax with Insurer:"
                fraReinsurance.Text = "Insurance"
                chkDeferredRI.Text = "Deferred Insurance Permitted:"
            End If


            cmdApply.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACApplyButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 2, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            SSTabHelper.SetTabCaption(tabMainTab, 3, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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


            lblCode.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblEffectiveDate.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEffectiveDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblDescription.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblSectionMask.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSectionMask, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblStampDutyRate1.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStampDutyRate1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblStampDutyRate2.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACStampDutyRate2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblPrimarySort.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPrimarySort, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblSecondarySort.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSecondarySort, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkShareWithCoInsurer.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACShareWithCoInsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkSuppressPublicText.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSuppressPublicText, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkSuppressPrivateText.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSuppressPrivateText, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            chkSuppressTaxes.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSuppressTaxes, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdHeaderClause.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHeaderClause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            cmdTrailerClause.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTrailerClause, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblAccumulationLevel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAccumulationLevel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lblGISScreenId.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACGISScreenId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
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
        Dim lTemp As Integer
        Dim vRIModelUsageDeferredRIArray(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_vRiskTypeList) Then
                If m_sCode.Trim().ToUpper() <> Me.txtCode.Text.Trim().ToUpper() Then
                    For lRow As Integer = m_vRiskTypeList.GetLowerBound(1) To m_vRiskTypeList.GetUpperBound(1)
                        If CStr(m_vRiskTypeList(ACIrisk_folder_type_id, lRow)).Trim().ToUpper() = Me.txtCode.Text.Trim().ToUpper() Then
                            MessageBox.Show("Risk Type Code already exists", "Risk Type", MessageBoxButtons.OK, MessageBoxIcon.Error)
                            result = gPMConstants.PMEReturnCode.PMFalse
                            txtCode.Focus()
                            Return result
                        End If
                    Next
                End If
            End If

            lTemp = CInt(m_oFormFields.UnformatControl(ctlControl:=txtAccumulationLevel))

            If (lTemp < 0) Or (lTemp > 9) Then
                MessageBox.Show("Accumulation level must be between 1 and 9", "Risk Type", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                txtAccumulationLevel.Focus()
                Return result
            End If

            ' AMB 15-Sep-03: 1.8.6 Deferred Reinsurance development - check
            ' that the user has set up deferred RI correctly
            If m_vIsDeferredRIPermitted = CheckState.Checked Then

                m_lReturn = m_oBusiness.GetRIModelUsageDeferredRI(v_lRiskTypeID:=m_lRiskTypeID, r_vRIModelUsageDeferredRI:=vRIModelUsageDeferredRIArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check deferred RI model", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateForm", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If Not Information.IsArray(vRIModelUsageDeferredRIArray) Then
                    ' no RI models are attached to the risk type
                    MessageBox.Show("'Deferred Reinsurance Permitted' has been selected but" & _
                                    Strings.Chr(13) & Strings.Chr(10) & _
                                    "no deferred reinsurance model has been chosen for this risk type." & _
                                    Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                    "Please select a deferred reinsurance model.", "Deferred Reinsurance Permitted", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
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
    ' Name: TagRiskTypeGroup
    '
    ' Description: loop through and put a tick for each record
    '              in m_vLinkedRiskTypeGroup on the ListView
    ' ***************************************************************** '
    Private Function TagRiskTypeGroup() As Integer

        Dim result As Integer = 0
        Dim lPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vLinkedRiskTypeGroup) Then
                For lCount As Integer = m_vLinkedRiskTypeGroup.GetLowerBound(1) To m_vLinkedRiskTypeGroup.GetUpperBound(1)
                    lPos = IsInArray(CInt(m_vLinkedRiskTypeGroup(0, lCount)), m_vRiskTypeGroup)

                    If lPos <> -1 Then
                        Me.lvwRiskTypeGroup.Items.Item(lPos).Selected = True
                        'Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag = Convert.ToString(Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag) & LISTVIEW_ICON_TICK
                        Me.lvwRiskTypeGroup.Items.Item(lPos).Tag = Convert.ToString(Me.lvwRiskTypeGroup.Items.Item(lPos).Tag) & LISTVIEW_ICON_TICK

                        Me.lvwRiskTypeGroup.Items(lPos).ImageKey = LISTVIEW_ICON_TICK
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TagRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TagRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadRiskTypeGroup
    '
    ' Description: load listview with data from data storage
    '
    ' ***************************************************************** '
    Private Function LoadRiskTypeGroup() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Const ACICode As Integer = 1
        Const ACIDescription As Integer = 2
        Const ACIEffectiveDate As Integer = 3

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwRiskTypeGroup.Items.Clear()

            If Information.IsArray(m_vRiskTypeGroup) Then
                For lRow As Integer = m_vRiskTypeGroup.GetLowerBound(1) To m_vRiskTypeGroup.GetUpperBound(1)

                    'column 1 Code
                    oListItem = lvwRiskTypeGroup.Items.Add(CStr(m_vRiskTypeGroup(ACICode, lRow)))

                    'column 2 Description
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vRiskTypeGroup(ACIDescription, lRow))

                    'column 3 effective date
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatDate, vControlValue:=m_vRiskTypeGroup(ACIEffectiveDate, lRow))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If txtFormatDate.Text = "" Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = txtFormatDate.Text
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CDate(txtFormatDate.Text).ToString("dd MMMM yyyy")
                    End If

                    oListItem.Tag = CStr(lRow)

                    '            'ghosted deleted records
                    '            If (m_vRiskTypeGroup(4, lRow&) = PMTrue) Then
                    '                Me.lvwRiskType.ListItems(lRow& + 1).Selected = True
                    '                Me.lvwRiskType.SelectedItem.Ghosted = True
                    '            End If

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        lvwRiskTypeGroup.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        lvwRiskTypeGroup.Refresh()
                    End If

                Next
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadRules
    '
    ' Description: load rules to listview
    '
    ' ***************************************************************** '
    Private Function LoadRules(ByVal v_vDataArray(,) As Object, ByRef r_lvwListView As ListView) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_lvwListView.Items.Clear()

            If Information.IsArray(v_vDataArray) Then
                For lRow As Integer = v_vDataArray.GetLowerBound(1) To v_vDataArray.GetUpperBound(1)

                    'column 1 Code



                    oListItem = r_lvwListView.Items.Add(CStr(v_vDataArray(ACFieldPosCode, lRow)), LISTVIEW_ICON_RULEFILE)

                    'column 2 Description

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(v_vDataArray(ACFieldPosDescription, lRow))

                    'column 3 effective date
                    m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtFormatDate, vControlValue:=ToSafeDate(v_vDataArray(ACFieldPosEffectiveDate, lRow)))

                    ' Check for errors
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Failed to assign the data.
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ToSafeDate(txtFormatDate.Text).ToString("dd MMMM yyyy")
                    'column 4 Rule Filename

                    'ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(v_vDataArray(ACFieldPosFileName, lRow))

                    'column 4 live
                    If v_vDataArray(ACFieldPosLive, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Live"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = "Not Live"
                    End If

                    oListItem.Tag = CStr(lRow)

                    'ghosted deleted records
                    If v_vDataArray(ACFieldPosIsDeleted, lRow) = gPMConstants.PMEReturnCode.PMTrue Then
                        r_lvwListView.Items.Item(lRow).Selected = True


                        r_lvwListView.Items.Item(lRow).ForeColor = Color.Gray
                        'end
                    End If

                    ' Refresh the first X amount of rows, to
                    ' allow the user to see the results instantly.
                    If lRow = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                        ' Select the first item.
                        r_lvwListView.Items.Item(0).Selected = True

                        ' Refresh the initial results.
                        r_lvwListView.Refresh()
                    End If

                Next
            Else
                cmdEditRule.Enabled = False
                cmdDeleteRule.Enabled = False
                cmdEditRenRule.Enabled = False
                cmdDeleteRenRule.Enabled = False
                cmdEditRenLapseRule.Enabled = False
                cmdDeleteRenLapseRule.Enabled = False
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRules Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRules", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsInArray
    '
    ' Description:loop through r_vArray and return element position
    '             if first element is equals to v_lID
    '             return -1 if not found
    ' ***************************************************************** '
    Private Function IsInArray(ByVal v_lID As Integer, ByRef r_vArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = -1

            If Information.IsArray(r_vArray) Then
                For lCount As Integer = r_vArray.GetLowerBound(1) To r_vArray.GetUpperBound(1)

                    If v_lID = CDbl(r_vArray(0, lCount)) Then
                        result = lCount
                        Exit For
                    End If
                Next
            End If

            Return result

        Catch excep As System.Exception
            result = -1

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsInArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsInArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnLoadClauses
    '
    ' Description: loop through listview and store selected item to array
    '
    ' ***************************************************************** '
    'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
    'Private Function UnLoadClauses() As Long
    '
    'Dim lCount As Long
    'Dim lPos As Long
    'Dim bFirstTime As Boolean
    'Dim lTemp As Long
    '
    '    On Error GoTo Err_UnLoadClauses
    '
    '    UnLoadClauses = PMTrue
    '
    '    For lCount& = 1 To lvwClauses.ListItems.Count
    '
    ''        lvwClauses.ListItems(lCount&).Selected = True
    '
    '        'check to see if user has selected this record as allowed GIS Screen
    '        lPos = InStr(1, lvwClauses.ListItems(lCount&).Tag, LISTVIEW_ICON_TICK)
    '
    '        If (lPos <> 0) Then
    '            m_vClauses(3, lCount - 1) = 1
    '        Else
    '            m_vClauses(3, lCount - 1) = 0
    '        End If
    '    Next
    '
    '    Exit Function

    'Err_UnLoadClauses:
    '
    '    UnLoadClauses = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="UnLoadClauses Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="UnLoadClauses", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '    Resume
    '
    'End Function
    'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
    ' ***************************************************************** '
    '
    ' Name: UnloadRiskTypeGroup
    '
    ' Description: loop through listview and store selected item to array
    '
    ' ***************************************************************** '
    Private Function UnloadRiskTypeGroup() As Integer

        Dim result As Integer = 0
        Dim lPos As Integer
        Dim bFirstTime As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bFirstTime = True
            lPos = 0
            m_vLinkedRiskTypeGroup = VB6.CopyArray(Nothing)

            For lCount As Integer = 1 To lvwRiskTypeGroup.Items.Count

                lvwRiskTypeGroup.Items.Item(lCount - 1).Selected = True

                'check to see if user has selected this record as allowed GIS Screen
                lPos = (CStr(Convert.ToString(lvwRiskTypeGroup.Items.Item(lCount - 1).Tag)).IndexOf(LISTVIEW_ICON_TICK) + 1)

                If lPos <> 0 Then

                    If bFirstTime Then
                        bFirstTime = False
                        ReDim m_vLinkedRiskTypeGroup(0, 0)
                    Else
                        ReDim Preserve m_vLinkedRiskTypeGroup(0, m_vLinkedRiskTypeGroup.GetUpperBound(1) + 1)
                    End If

                    m_vLinkedRiskTypeGroup(0, m_vLinkedRiskTypeGroup.GetUpperBound(1)) = m_vRiskTypeGroup(0, CInt(Convert.ToString(lvwRiskTypeGroup.Items.Item(lCount - 1).Tag).Substring(0, lPos - 1)))

                End If
            Next

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnloadRiskTypeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadRiskTypeGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboGISScreenID_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboGISScreenID.SelectedIndexChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowAddRatingSection_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowAddRatingSection.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowDeleteRatingSection_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowDeleteRatingSection.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowEditRatingSection_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowEditRatingSection.CheckStateChanged
        fraEdit.Enabled = chkAllowEditRatingSection.CheckState
        If chkAllowEditRatingSection.CheckState = CheckState.Unchecked Then
            chkAllowEditRatingSectionRateType.CheckState = CheckState.Unchecked
            chkAllowEditRatingSectionRateType.Enabled = False
            chkAllowEditRatingSectionRate.CheckState = CheckState.Unchecked
            chkAllowEditRatingSectionRate.Enabled = False
            chkAllowEditRatingSectionSumInsured.CheckState = CheckState.Unchecked
            chkAllowEditRatingSectionSumInsured.Enabled = False
            chkAllowEditRatingSectionThisPremium.CheckState = CheckState.Unchecked
            chkAllowEditRatingSectionThisPremium.Enabled = False
        Else
            chkAllowEditRatingSectionRateType.Enabled = True
            chkAllowEditRatingSectionRate.Enabled = True
            chkAllowEditRatingSectionSumInsured.Enabled = True
            chkAllowEditRatingSectionThisPremium.Enabled = True
        End If
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowEditRatingSectionRate_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowEditRatingSectionRate.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowEditRatingSectionRateType_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowEditRatingSectionRateType.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowEditRatingSectionSumInsured_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowEditRatingSectionSumInsured.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkAllowEditRatingSectionThisPremium_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkAllowEditRatingSectionThisPremium.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkClaimsIsPostTaxes_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkClaimsIsPostTaxes.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkDeferredRI_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDeferredRI.CheckStateChanged
        If chkDeferredRI.CheckState = CheckState.Checked Then
            m_vIsDeferredRIPermitted = CStr(1)
            cmdDeferredRI.Enabled = True
        Else
            m_vIsDeferredRIPermitted = CStr(0)
            cmdDeferredRI.Enabled = False
        End If
        cmdApply.Enabled = True
    End Sub

    Private Sub chkDisplayClaimReinsurance_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDisplayClaimReinsurance.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkDisplayReinsurance_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkDisplayReinsurance.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkIsAutoReinsured_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkIsAutoReinsured.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkShareWithCoInsurer_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkShareWithCoInsurer.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkShareWithReInsurer_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkShareWithReInsurer.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkSuppressPrivateText_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSuppressPrivateText.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkSuppressPublicText_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSuppressPublicText.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub chkSuppressTaxes_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkSuppressTaxes.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub cmdAddRenRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddRenRule.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'Create Risk Type object if not already done so
        If m_oRiskTypeRuleSet Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskTypeRuleSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskTypeRuleSet, sClassName:="iPMURiskTypeRuleSet.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskTypeRuleSet = temp_m_oRiskTypeRuleSet

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type Rule Set object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub

            End If

        End If

        m_lReturn = m_oRiskTypeRuleSet.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        m_oRiskTypeRuleSet.RiskTypeID = m_lRiskTypeID

        m_oRiskTypeRuleSet.RuleType = "RN"

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Renewal Scripts)"
        m_oRiskTypeRuleSet.UniqueId = m_sUniqueId
        m_oRiskTypeRuleSet.ScreenHierarchy = m_sScreenHierarchy

        m_lReturn = m_oRiskTypeRuleSet.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oRiskTypeRuleSet.Status = gPMConstants.PMEReturnCode.PMCancel Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        'get data back from risk type rule set object
        If IsArray(m_vRenewalRule) Then
            ReDim Preserve m_vRenewalRule(12, m_vRenewalRule.GetUpperBound(1) + 1)
        Else
            ReDim m_vRenewalRule(12, 0)
        End If

        m_vRenewalRule(ACFieldPosRiskTypeRuleSetID, m_vRenewalRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.RiskTypeRuleSetID
        m_vRenewalRule(ACFieldPosCode, m_vRenewalRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Code
        m_vRenewalRule(ACFieldPosDescription, m_vRenewalRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Description
        m_vRenewalRule(ACFieldPosEffectiveDate, m_vRenewalRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.RuleEffectiveDate
        m_vRenewalRule(ACFieldPosFileName, m_vRenewalRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.FileName
        m_vRenewalRule(ACFieldPosLive, m_vRenewalRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Live
        m_vRenewalRule(ACFieldPosIsDeleted, m_vRenewalRule.GetUpperBound(1)) = gPMConstants.PMEReturnCode.PMFalse
        m_vRenewalRule(ACFieldPosTypeID, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.RuleTypeID
        m_vRenewalRule(ACFieldPosType, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.RuleTypeDescription
        m_vRenewalRule(ACFieldPosDREExecutorURL, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.DREExecutorURL
        m_vRenewalRule(ACFieldPosDREDefaultToken, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.DREDefaultToken



        m_lReturn = LoadRules(v_vDataArray:=m_vRenewalRule, r_lvwListView:=lvwRenewalRule)
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdAddRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddRule.Click

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'Create Risk Type object if not already done so
        If m_oRiskTypeRuleSet Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskTypeRuleSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskTypeRuleSet, sClassName:="iPMURiskTypeRuleSet.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskTypeRuleSet = temp_m_oRiskTypeRuleSet

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type Rule Set object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub

            End If

        End If

        m_lReturn = m_oRiskTypeRuleSet.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        m_oRiskTypeRuleSet.RiskTypeID = m_lRiskTypeID

        m_oRiskTypeRuleSet.RuleType = "RT"
        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Rating Scripts)"
        m_oRiskTypeRuleSet.UniqueId = m_sUniqueId
        m_oRiskTypeRuleSet.ScreenHierarchy = m_sScreenHierarchy

        m_lReturn = m_oRiskTypeRuleSet.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oRiskTypeRuleSet.Status = gPMConstants.PMEReturnCode.PMCancel Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        'get data back from risk type rule set object

        'get data back from risk type rule set object
        If Information.IsArray(m_vRule) Then

            'ReDim Preserve m_vRule(12, m_vRule.GetUpperBound(1) + 1)
            ' Changed the first parameter of redim statement to m_vRule.GetUpperBound(0). Earlier it was "12"
            ReDim Preserve m_vRule(m_vRule.GetUpperBound(0), m_vRule.GetUpperBound(1) + 1)

        Else
            ReDim m_vRule(12, 0)
        End If


        m_vRule(ACFieldPosRiskTypeRuleSetID, m_vRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.RiskTypeRuleSetID
        m_vRule(ACFieldPosCode, m_vRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Code
        m_vRule(ACFieldPosDescription, m_vRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Description
        m_vRule(ACFieldPosEffectiveDate, m_vRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.RuleEffectiveDate
        m_vRule(ACFieldPosFileName, m_vRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.FileName
        m_vRule(ACFieldPosLive, m_vRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Live
        m_vRule(ACFieldPosIsDeleted, m_vRule.GetUpperBound(1)) = gPMConstants.PMEReturnCode.PMFalse
        m_vRule(ACFieldPosType, UBound(m_vRule, 2)) = m_oRiskTypeRuleSet.RuleTypeDescription
        m_vRule(ACFieldPosTypeID, UBound(m_vRule, 2)) = m_oRiskTypeRuleSet.RuleTypeID
        m_vRule(ACFieldPosDREExecutorURL, UBound(m_vRule, 2)) = m_oRiskTypeRuleSet.DREExecutorURL
        m_vRule(ACFieldPosDREDefaultToken, UBound(m_vRule, 2)) = m_oRiskTypeRuleSet.DREDefaultToken


        m_lReturn = LoadRules(v_vDataArray:=m_vRule, r_lvwListView:=lvwRules)
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' Click event of the OK button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = ValidateForm()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                'details has been saved, show rules tab
                SSTabHelper.SetTabVisible(tabMainTab, 3, True)

                ' Set the interface status.
                m_bApplyButton = True

            End If

            'Set this, else ok (or another apply) will create another version)
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
            ' AMB 15/05/2003: 1.8.6 Deferred Reinsurance development - do not re-enable RIModel button for no reason

            SaveRatingSectionDetails()
            cmdApply.Enabled = False
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the  Apply command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdDeferredRI_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeferredRI.Click

        Dim oRIModel As iPMURIModelUsage.Interface_Renamed

        Try

            oRIModel = New iPMURIModelUsage.Interface_Renamed()

            If oRIModel.Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to initialise RIModelUsage object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                oRIModel = Nothing
                Exit Sub
            End If

            oRIModel.SetProcessModes(m_iTask)
            oRIModel.RiskTypeId = m_lRiskTypeID
            oRIModel.Description = m_vDescription
            ' AMB 28/05/2003: 1.8.6 Deferred RI RFC
            oRIModel.IsDeferred = gPMConstants.PMEReturnCode.PMTrue

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            oRIModel.UniqueId = m_sUniqueId

            If oRIModel.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to start RIModelUsage", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            oRIModel.Dispose()


            oRIModel = Nothing
            cmdApply.Enabled = True

        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the RI Model Usage object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRIModel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdDeleteRenRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteRenRule.Click


        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Dim iDelete As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue


        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRenewalRule.Items.Item(Me.lvwRenewalRule.FocusedItem.Index).Tag)


        'If Me.lvwRenewalRule.SelectedItem.Ghosted Then
        If Me.lvwRenewalRule.FocusedItem.ForeColor.Equals(Color.Gray) Then
            iDelete = gPMConstants.PMEReturnCode.PMFalse
        End If

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Renewal Scripts)/Code({lvwRenewalRule.FocusedItem.Text.Trim()})"

        m_lReturn = m_oBusiness.DelRiskTypeRuleSet(v_lRiskTypeID:=m_lRiskTypeID, v_lRiskTypeRuleSetID:=m_vRenewalRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem), v_iIsDeleted:=iDelete, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_vRenewalRule(ACFieldPosIsDeleted, lSelectedItem) = iDelete
            m_lReturn = LoadRules(v_vDataArray:=m_vRenewalRule, r_lvwListView:=lvwRenewalRule)
        End If

        cmdEditRenRule.Enabled = False
        cmdDeleteRenRule.Enabled = False
        cmdAddRenRule.Enabled = True

        cmdCancel.Enabled = True
        cmdOK.Enabled = True
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdDeleteRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteRule.Click


        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Dim iDelete As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue


        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRules.Items.Item(Me.lvwRules.FocusedItem.Index).Tag)



        'If Me.lvwRules.SelectedItem.Ghosted Then
        If Me.lvwRules.FocusedItem.ForeColor.Equals(Color.Gray) Then
            iDelete = gPMConstants.PMEReturnCode.PMFalse
        End If

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Rating Scripts)/Code({lvwRules.FocusedItem.Text.Trim()})"

        m_lReturn = m_oBusiness.DelRiskTypeRuleSet(v_lRiskTypeID:=m_lRiskTypeID, v_lRiskTypeRuleSetID:=m_vRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem), v_iIsDeleted:=iDelete, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_vRule(ACFieldPosIsDeleted, lSelectedItem) = iDelete
            m_lReturn = LoadRules(m_vRule, lvwRules)
        End If

        cmdEditRule.Enabled = False
        cmdDeleteRule.Enabled = False
        cmdAddRule.Enabled = True

        cmdCancel.Enabled = True
        cmdOK.Enabled = True
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdEditRenRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditRenRule.Click


        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'get position of current rule

        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRenewalRule.Items.Item(Me.lvwRenewalRule.FocusedItem.Index).Tag)

        'Create Risk Type object if not already done so
        If m_oRiskTypeRuleSet Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskTypeRuleSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskTypeRuleSet, sClassName:="iPMURiskTypeRuleSet.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskTypeRuleSet = temp_m_oRiskTypeRuleSet

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type Rule Set object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub

            End If

        End If

        m_lReturn = m_oRiskTypeRuleSet.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        'pass selected details to Risk Type object
        m_oRiskTypeRuleSet.RiskTypeRuleSetID = CInt(m_vRenewalRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem))
        m_oRiskTypeRuleSet.RiskTypeID = m_lRiskTypeID
        m_oRiskTypeRuleSet.RuleType = "RN"
        m_oRiskTypeRuleSet.RuleTypeID = ToSafeLong(m_vRenewalRule(ACFieldPosTypeID, lSelectedItem))
        m_oRiskTypeRuleSet.DREExecutorURL = ToSafeString(m_vRenewalRule(ACFieldPosDREExecutorURL, lSelectedItem))
        m_oRiskTypeRuleSet.DREDefaultToken = ToSafeString(m_vRenewalRule(ACFieldPosDREDefaultToken, lSelectedItem))

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Renewal Scripts)"
        m_oRiskTypeRuleSet.UniqueId = m_sUniqueId
        m_oRiskTypeRuleSet.ScreenHierarchy = m_sScreenHierarchy

        m_lReturn = m_oRiskTypeRuleSet.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oRiskTypeRuleSet.Status = gPMConstants.PMEReturnCode.PMCancel Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        'get data back from risk type rule set object

        m_vRenewalRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem) = m_oRiskTypeRuleSet.RiskTypeRuleSetID

        m_vRenewalRule(ACFieldPosCode, lSelectedItem) = m_oRiskTypeRuleSet.Code
        m_vRenewalRule(ACFieldPosDescription, lSelectedItem) = m_oRiskTypeRuleSet.Description

        m_vRenewalRule(ACFieldPosEffectiveDate, lSelectedItem) = m_oRiskTypeRuleSet.RuleEffectiveDate

        m_vRenewalRule(ACFieldPosFileName, lSelectedItem) = m_oRiskTypeRuleSet.FileName

        m_vRenewalRule(ACFieldPosLive, lSelectedItem) = m_oRiskTypeRuleSet.Live
        m_vRenewalRule(ACFieldPosIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse
        m_vRenewalRule(ACFieldPosTypeID, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.RuleTypeID
        m_vRenewalRule(ACFieldPosDREExecutorURL, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.DREExecutorURL
        m_vRenewalRule(ACFieldPosDREDefaultToken, UBound(m_vRenewalRule, 2)) = m_oRiskTypeRuleSet.DREDefaultToken


        m_lReturn = LoadRules(v_vDataArray:=m_vRenewalRule, r_lvwListView:=lvwRenewalRule)
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdEditRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditRule.Click


        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'get position of current rule

        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRules.Items.Item(Me.lvwRules.FocusedItem.Index).Tag)

        'Create Risk Type object if not already done so
        If m_oRiskTypeRuleSet Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskTypeRuleSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskTypeRuleSet, sClassName:="iPMURiskTypeRuleSet.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskTypeRuleSet = temp_m_oRiskTypeRuleSet

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type Rule Set object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub

            End If

        End If

        m_lReturn = m_oRiskTypeRuleSet.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        'pass selected details to Risk Type object
        m_oRiskTypeRuleSet.RiskTypeRuleSetID = CInt(m_vRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem))
        m_oRiskTypeRuleSet.RiskTypeID = m_lRiskTypeID
        m_oRiskTypeRuleSet.RuleType = "RT"
        m_oRiskTypeRuleSet.RuleTypeID = ToSafeLong(m_vRule(ACFieldPosTypeID, lSelectedItem))
        m_oRiskTypeRuleSet.DREExecutorURL = ToSafeString(m_vRule(ACFieldPosDREExecutorURL, lSelectedItem))
        m_oRiskTypeRuleSet.DREDefaultToken = ToSafeString(m_vRule(ACFieldPosDREDefaultToken, lSelectedItem))

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Rating Scripts)"
        m_oRiskTypeRuleSet.UniqueId = m_sUniqueId
        m_oRiskTypeRuleSet.ScreenHierarchy = m_sScreenHierarchy

        m_lReturn = m_oRiskTypeRuleSet.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oRiskTypeRuleSet.Status = gPMConstants.PMEReturnCode.PMCancel Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        'get data back from risk type rule set object

        m_vRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem) = m_oRiskTypeRuleSet.RiskTypeRuleSetID

        m_vRule(ACFieldPosCode, lSelectedItem) = m_oRiskTypeRuleSet.Code
        m_vRule(ACFieldPosDescription, lSelectedItem) = m_oRiskTypeRuleSet.Description

        m_vRule(ACFieldPosEffectiveDate, lSelectedItem) = m_oRiskTypeRuleSet.RuleEffectiveDate

        m_vRule(ACFieldPosFileName, lSelectedItem) = m_oRiskTypeRuleSet.FileName

        m_vRule(ACFieldPosLive, lSelectedItem) = m_oRiskTypeRuleSet.Live
        m_vRule(ACFieldPosIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse
        m_vRule(ACFieldPosType, lSelectedItem) = m_oRiskTypeRuleSet.RuleTypeDescription
        m_vRule(ACFieldPosTypeID, lSelectedItem) = m_oRiskTypeRuleSet.RuleTypeID
        m_vRule(ACFieldPosDREExecutorURL, lSelectedItem) = m_oRiskTypeRuleSet.DREExecutorURL
        m_vRule(ACFieldPosDREDefaultToken, lSelectedItem) = m_oRiskTypeRuleSet.DREDefaultToken


        m_lReturn = LoadRules(v_vDataArray:=m_vRule, r_lvwListView:=lvwRules)
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    ' PRIVATE Methods (End)

    ' PRIVATE Events (Begin)

    Private Sub cmdHeaderClause_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHeaderClause.Click

        Dim vResult As DialogResult

        Dim oPolicyWording As iPMBFindDocTemplate.Interface_Renamed

        'Based on code nicked from the policy control - need to call find document template,
        'locked on document type id = 7 instead

        Try

            ' Get an instance of the policy wording interface object via
            ' the public object manager.
            Dim temp_oPolicyWording As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPolicyWording, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPolicyWording = temp_oPolicyWording

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policy wording object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHeaderClause_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If


            m_lReturn = oPolicyWording.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Run it in Merge mode to hide some buttons

            oPolicyWording.Mode = 1


            m_lReturn = oPolicyWording.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If cancelled, ask if want to remove

            If oPolicyWording.Status = gPMConstants.PMEReturnCode.PMCancel Then
                If StringsHelper.ToDoubleSafe(m_vHeaderClauseId) <> 0 Then

                    vResult = MessageBox.Show("Do you wish to remove this clause?", "Risk Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If vResult = System.Windows.Forms.DialogResult.Yes Then
                        m_vHeaderClauseId = CStr(0)
                        lblHeaderClause.Text = ""
                    End If

                End If


                oPolicyWording.Dispose()

                oPolicyWording = Nothing

                Exit Sub
            End If


            m_vHeaderClauseId = oPolicyWording.DocumentTemplateId
            'Not yet available
            '    lblHeaderClause.Caption = oPolicyWording.DocumentDescription

            lblHeaderClause.Text = oPolicyWording.DocumentCode


            oPolicyWording.Dispose()

            oPolicyWording = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdHeaderClause_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdHeaderClause_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        ' Fire up the help screen

        PMHelpFunc.g_sProductFamily = g_sProductFamily
        m_lReturn = PMHelpFunc.ShowHelp(cmdHelp, lContextID:=ScreenHelpID)

    End Sub

    Private Sub cmdRILimits_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRILimits.Click

        Dim oRILimits As iPMURiskTypeRILimits.Interface_Renamed

        Try

            Dim temp_oRILimits As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oRILimits, sClassName:="iPMURiskTypeRILimits.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oRILimits = temp_oRILimits

            If oRILimits Is Nothing Then
                MessageBox.Show("Failed to initialise RI Limits object", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            m_lReturn = oRILimits.SetProcessModes(vTask:=m_iTask)

            oRILimits.RiskTypeId = m_lRiskTypeID

            oRILimits.Description = m_vDescription
            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            oRILimits.UniqueId = m_sUniqueId

            m_lReturn = oRILimits.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to start RI Limits", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            oRILimits.Dispose()


            oRILimits = Nothing
            cmdApply.Enabled = True

        Catch excep As System.Exception
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the RI Model Usage object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRILimits_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdRIModel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRIModel.Click

        Dim oRIModel As iPMURIModelUsage.Interface_Renamed

        Try

            oRIModel = New iPMURIModelUsage.Interface_Renamed()

            If oRIModel.Initialise() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to initialise RIModelUsage object", Application.ProductName)
                oRIModel = Nothing
                Exit Sub
            End If

            oRIModel.SetProcessModes(m_iTask)
            oRIModel.RiskTypeId = m_lRiskTypeID
            oRIModel.Description = m_vDescription
            ' AMB 28/05/2003: 1.8.6 Deferred RI RFC
            oRIModel.IsDeferred = gPMConstants.PMEReturnCode.PMFalse

            If String.IsNullOrEmpty(m_sUniqueId) Then
                m_sUniqueId = GetUniqueID()
            End If

            oRIModel.UniqueId = m_sUniqueId

            If oRIModel.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to start RIModelUsage", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If

            oRIModel.Dispose()


            oRIModel = Nothing
            cmdApply.Enabled = True

        Catch excep As System.Exception



            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the RI Model Usage object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdRIModel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub cmdSelectRiskTypeGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSelectRiskTypeGroup.Click

        ' Check to see if current record is selected as allowed risk type group
        Dim lPos As Integer = (CStr(Convert.ToString(Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag)).IndexOf(LISTVIEW_ICON_TICK) + 1)

        ' Select it if its not selected or unselected if its selected
        If lPos <> 0 Then
            Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag = Convert.ToString(Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag).Substring(0, lPos - 1)

            ' Set to a blank picture rather than 0, as it was causing refresh problems


            Me.lvwRiskTypeGroup.Items(Me.lvwRiskTypeGroup.FocusedItem.Index).ImageKey = LISTVIEW_ICON_BLANK

        Else
            Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag = Convert.ToString(Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag) & LISTVIEW_ICON_TICK



            Me.lvwRiskTypeGroup.Items(Me.lvwRiskTypeGroup.FocusedItem.Index).ImageKey = LISTVIEW_ICON_TICK
        End If
        cmdApply.Enabled = True
    End Sub


    Private Sub cmdTrailerClause_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTrailerClause.Click

        Dim vResult As DialogResult

        Dim oPolicyWording As iPMBFindDocTemplate.Interface_Renamed

        'Based on code nicked from the policy control - need to call find document template,
        'locked on document type id = 7 instead

        Try

            ' Get an instance of the policy wording interface object via
            ' the public object manager.
            Dim temp_oPolicyWording As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPolicyWording, sClassName:="iPMBFindDocTemplate.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oPolicyWording = temp_oPolicyWording

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policy wording object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTrailerClause_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Exit Sub

            End If


            m_lReturn = oPolicyWording.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Run it in Merge mode to hide some buttons

            oPolicyWording.Mode = 1


            m_lReturn = oPolicyWording.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'If cancelled, ask if want to remove

            If oPolicyWording.Status = gPMConstants.PMEReturnCode.PMCancel Then

                If StringsHelper.ToDoubleSafe(m_vTrailerClauseId) <> 0 Then
                    vResult = MessageBox.Show("Do you wish to remove this clause", "Risk Type", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

                    If vResult = System.Windows.Forms.DialogResult.Yes Then
                        m_vTrailerClauseId = CStr(0)
                        lblTrailerClause.Text = ""
                    End If
                End If


                oPolicyWording.Dispose()

                oPolicyWording = Nothing

                Exit Sub
            End If


            m_vTrailerClauseId = oPolicyWording.DocumentTemplateId
            'Not yet available
            '    lblTrailerClause.Caption = oPolicyWording.DocumentDescription

            lblTrailerClause.Text = oPolicyWording.DocumentCode


            oPolicyWording.Dispose()

            oPolicyWording = Nothing

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdTrailerClause_Click Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdTrailerClause_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender
            cmdApply.Enabled = False
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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskType.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMURiskType.General()

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

            'default apply button value
            m_bApplyButton = False

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
            Dim key As uctPickList.PickListKey
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
            m_lReturn = SetFieldValidation()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = iPMFunc.GetSystemOption(5005, m_sUnderwritingType)

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            cboClaimsCoverBasis.RefreshList()
            cboClaimsTypeBasis.RefreshList()

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                For lRow As Integer = 0 To cboClaimsTypeBasis.ListCount
                    cboClaimsTypeBasis.ListIndex = lRow
                    If cboClaimsTypeBasis.ItemCode.Trim().ToUpper() = "OCCUR" Then
                        Exit For
                    End If
                Next lRow

                For lRow As Integer = 0 To cboClaimsCoverBasis.ListCount
                    cboClaimsCoverBasis.ListIndex = lRow
                    If cboClaimsCoverBasis.ItemCode.Trim().ToUpper() = "STD" Then
                        Exit For
                    End If
                Next lRow

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
            'QBENZ022


            key = New uctPickList.PickListKey()
            key.KeyName = "risk_type_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListRatingSections.ForeignKeys.Add(key, Key:="risk_type_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "rating_section_type_id"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListRatingSections.ForeignKeys.Add(key, Key:="rating_section_type_id")

            key = New uctPickList.PickListKey()
            key.KeyName = "UserId"
            key.ValueType = gPMConstants.PMEDataType.PMLong
            uctPickListRatingSections.ForeignKeys.Add(key, Key:="UserId")

            key = New uctPickList.PickListKey()
            key.KeyName = "UniqueId"
            key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListRatingSections.ForeignKeys.Add(key, Key:="UniqueId")

            key = New uctPickList.PickListKey()
            key.KeyName = "ScreenHierarchy"
            key.ValueType = gPMConstants.PMEDataType.PMString
            uctPickListRatingSections.ForeignKeys.Add(key, Key:="ScreenHierarchy")

            SetPickListPKs()

            m_lReturn = uctPickListRatingSections.Load_Renamed()

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    Private Sub SetPickListPKs()

        With uctPickListRatingSections.ForeignKeys

            .Item("risk_type_id").Value = m_lRiskTypeID

            .Item("rating_section_type_id").Value = 0
            uctPickListRatingSections.ForeignKeys.Item("UserId").Value = Nothing
            uctPickListRatingSections.ForeignKeys.Item("UniqueId").Value = m_sUniqueId
            uctPickListRatingSections.ForeignKeys.Item("ScreenHierarchy").Value = m_sScreenHierarchy
        End With


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

                    eventArgs.Cancel = True
                    Cancel = 1

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Terminate the business object


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
        Catch




            Exit Sub
        End Try


    End Sub


    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            m_lReturn = ValidateForm()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

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
            SaveRatingSectionDetails()
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub SaveRatingSectionDetails() 'QBENZ022
        SetPickListPKs()
        uctPickListRatingSections.Save()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'set to PMOK if data has been Applied
            If m_bApplyButton Then
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Else
                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private isInitializingComponent As Boolean
    Private Sub lblHeaderClause_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblHeaderClause.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub


    Private Sub lblTrailerClause_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lblTrailerClause.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub
    'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
    'Private Sub lvwClauses_Click()
    '    If Me.lvwClauses.ListItems.Count > 0 Then
    '        If InStr(1, Me.lvwClauses.ListItems(Me.lvwClauses.SelectedItem.Index).Tag, LISTVIEW_ICON_TICK) <> 0 Then
    '            Me.cmdSelectClause.Caption = "&Unselect"
    '        Else
    '            Me.cmdSelectClause.Caption = "&Select"
    '        End If
    '    End If
    'End Sub

    'Private Sub lvwClauses_ColumnClick(ByVal ColumnHeader As MSComCtlLib.ColumnHeader)
    '    ' Column click event for the search details
    '
    '    On Error GoTo Err_lvwClauses_ColumnClick
    '
    '    With lvwClauses
    '        ' If current sort column header is
    '        ' pressed.
    '        If (ColumnHeader.Index - 1 = .SortKey) Then
    '            ' Set sort order opposite of
    '            ' current direction.
    '            .SortOrder = (.SortOrder + 1) Mod 2
    '        Else
    '            ' Sort by this column (ascending).
    '            .Sorted = False
    '
    '            ' Turn off sorting so that the list
    '            ' is not sorted twice
    '            .SortOrder = 0
    '            .SortKey = ColumnHeader.Index - 1
    '            .Sorted = True
    '        End If
    '    End With
    '
    '    Exit Sub
    '
    '
    'Err_lvwClauses_ColumnClick:
    '
    '    ' Log Error.
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Failed to sort the column", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="lvwClauses_ColumnClick", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Sub
    '
    'End Sub

    'Private Sub lvwClauses_MouseDown(Button As Integer, Shift As Integer, x As Single, y As Single)
    '    'Not if we're viewing, thank you very much
    '    If (Task <> PMView) Then
    '        If (Me.lvwClauses.HitTest(x, y) Is Nothing) Then
    '            Me.cmdSelectClause.Enabled = False
    '        Else
    '            Me.cmdSelectClause.Enabled = True
    '        End If
    '    End If
    '
    'End Sub
    'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
    Private Sub lvwRenewalRule_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRenewalRule.Click
        If Me.lvwRenewalRule.Items.Count > 0 Then


            'If Me.lvwRenewalRule.SelectedItem.Ghosted Then
            If Me.lvwRenewalRule.FocusedItem.ForeColor.Equals(Color.Gray) Then

                cmdDeleteRenRule.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACUndeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                cmdEditRenRule.Enabled = False

            Else


                cmdDeleteRenRule.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
        End If

    End Sub

    Private Sub lvwRenewalRule_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRenewalRule.Enter
        cmdEditRule.Enabled = False
        cmdAddRule.Enabled = False
        cmdDeleteRule.Enabled = False
        cmdAddRenLapseRule.Enabled = False
        cmdEditRenLapseRule.Enabled = False
        cmdDeleteRenLapseRule.Enabled = False
    End Sub

    Private Sub lvwRenewalRule_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRenewalRule.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end
        If Me.lvwRenewalRule.Items.Count > 0 Then
            'Not if we're viewing, thank you very much
            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If Me.lvwRenewalRule.GetItemAt(x, y) Is Nothing Then
                    cmdDeleteRenRule.Enabled = False
                    cmdEditRenRule.Enabled = False
                Else
                    cmdEditRenRule.Enabled = True
                    cmdDeleteRenRule.Enabled = True
                End If
            End If
        End If

        cmdAddRenRule.Enabled = True
    End Sub

    Private Sub lvwRiskTypeGroup_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRiskTypeGroup.Click
        If Me.lvwRiskTypeGroup.Items.Count > 0 Then
            If CStr(Convert.ToString(Me.lvwRiskTypeGroup.Items.Item(Me.lvwRiskTypeGroup.FocusedItem.Index).Tag)).IndexOf(LISTVIEW_ICON_TICK) >= 0 Then
                Me.cmdSelectRiskTypeGroup.Text = "&Unselect"
            Else
                Me.cmdSelectRiskTypeGroup.Text = "&Select"
            End If
        End If

    End Sub

    Private Sub lvwRiskTypeGroup_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwRiskTypeGroup.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwRiskTypeGroup.Columns(eventArgs.Column)
        ' Column click event for the search details

        Try
            'Start-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)
            With lvwRiskTypeGroup
                ' If current sort column header is
                ' pressed.
                If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwRiskTypeGroup) Then
                    ' Set sort order opposite of
                    ' current direction.
                    ListViewHelper.SetSortOrderProperty(lvwRiskTypeGroup, (ListViewHelper.GetSortOrderProperty(lvwRiskTypeGroup) + 1) Mod 2)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwRiskTypeGroup, False)

                    ' Turn off sorting so that the list
                    ' is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwRiskTypeGroup, SortOrder.Ascending)
                    ListViewHelper.SetSortKeyProperty(lvwRiskTypeGroup, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwRiskTypeGroup, True)
                End If
            End With

        Catch excep As System.Exception
            'End-(Arul Stephen)-(TechSpec WR6ClauseGrouping.doc)



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwRiskTypeGroup_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Private Sub lvwRiskTypeGroup_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRiskTypeGroup.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'Not if we're viewing, thank you very much
        If Task <> gPMConstants.PMEComponentAction.PMView Then
            Me.cmdSelectRiskTypeGroup.Enabled = Not (Me.lvwRiskTypeGroup.GetItemAt(x, y) Is Nothing)
        End If

    End Sub

    Private Sub lvwRules_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRules.Click

        If Me.lvwRules.Items.Count > 0 Then
            If Me.lvwRules.FocusedItem.ForeColor.Equals(Color.Gray) Then
                cmdDeleteRule.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACUndeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                cmdEditRule.Enabled = False
            Else
                cmdDeleteRule.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
        End If

    End Sub

    Private Sub lvwRules_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwRules.Enter
        cmdEditRenRule.Enabled = False
        cmdAddRenRule.Enabled = False
        cmdDeleteRenRule.Enabled = False
        cmdAddRenLapseRule.Enabled = False
        cmdEditRenLapseRule.Enabled = False
        cmdDeleteRenLapseRule.Enabled = False
    End Sub

    Private Sub lvwRules_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRules.MouseDown
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        If Me.lvwRules.Items.Count > 0 Then
            'Not if we're viewing, thank you very much
            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If Me.lvwRules.GetItemAt(x, y) Is Nothing Then
                    cmdDeleteRule.Enabled = False
                    cmdEditRule.Enabled = False
                Else
                    cmdEditRule.Enabled = True
                    cmdDeleteRule.Enabled = True
                End If
            End If
        End If

        cmdAddRule.Enabled = True
    End Sub

    Private Sub txtAccumulationLevel_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccumulationLevel.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub

    Private Sub txtAccumulationLevel_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccumulationLevel.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtAccumulationLevel)
    End Sub

    Private Sub txtAccumulationLevel_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccumulationLevel.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtAccumulationLevel)
    End Sub

    Private Sub txtCode_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub

    Private Sub txtCode_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtCode_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCode.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtCode)
    End Sub

    Private Sub txtDescription_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtDescription.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
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

    Private Sub txtEffectiveDate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtEffectiveDate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        cmdApply.Enabled = True
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtEffectiveDate_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtEffectiveDate.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtEffectiveDate)
    End Sub

    Private Sub txtPrimarySort_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPrimarySort.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub

    Private Sub txtSecondarySort_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSecondarySort.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub

    Private Sub txtSectionMask_TextChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSectionMask.TextChanged
        If isInitializingComponent Then
            Exit Sub
        End If
        cmdApply.Enabled = True
    End Sub

    ' Alix - 22/11/2002 - Issue 1386
    'Private Sub txtReportPointer_GotFocus()
    '    m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtReportPointer)
    'End Sub
    'Private Sub txtReportPointer_LostFocus()
    '    m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtReportPointer)
    'End Sub

    Private Sub txtSectionMask_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSectionMask.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSectionMask)
    End Sub

    Private Sub txtSectionMask_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSectionMask.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSectionMask)
    End Sub

    Private Sub txtStampDutyRate1_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStampDutyRate1.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtStampDutyRate1)
    End Sub

    Private Sub txtStampDutyRate1_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtStampDutyRate1.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        cmdApply.Enabled = True
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtStampDutyRate1_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStampDutyRate1.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtStampDutyRate1)
    End Sub

    Private Sub txtStampDutyRate2_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStampDutyRate2.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtStampDutyRate2)
    End Sub

    Private Sub txtStampDutyRate2_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtStampDutyRate2.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        cmdApply.Enabled = True
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    Private Sub txtStampDutyRate2_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtStampDutyRate2.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtStampDutyRate2)
    End Sub

    Private Sub txtPrimarySort_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPrimarySort.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtPrimarySort)
    End Sub

    Private Sub txtPrimarySort_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtPrimarySort.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtPrimarySort)
    End Sub

    Private Sub txtSecondarySort_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSecondarySort.Enter
        m_lReturn = m_oFormFields.GotFocus(ctlControl:=txtSecondarySort)
    End Sub

    Private Sub txtSecondarySort_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSecondarySort.Leave
        m_lReturn = m_oFormFields.LostFocus(ctlControl:=txtSecondarySort)
    End Sub
    '<Pankaj PN:38898>
    Private Sub uctPickListRatingSections_Change(ByVal Sender As Object, ByVal e As EventArgs)
        If cmdApply.Visible Then
            cmdApply.Enabled = True
        End If
    End Sub
    '</Pankaj PN:38898>

    Private Sub tabMainTab_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles tabMainTab.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            tabMainTab.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            tabMainTab.SelectedIndex = 2
        End If
        If e.Alt And e.KeyCode = Keys.D4 Then
            tabMainTab.SelectedIndex = 3
        End If
        If e.Alt And e.KeyCode = Keys.D5 Then
            tabMainTab.SelectedIndex = 4
        End If
    End Sub

    Private Sub cmdAddRenLapseRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddRenLapseRule.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'Create Risk Type object if not already done so
        If m_oRiskTypeRuleSet Is Nothing Then
            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskTypeRuleSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskTypeRuleSet, sClassName:="iPMURiskTypeRuleSet.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskTypeRuleSet = temp_m_oRiskTypeRuleSet

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Risk Type Rule Set object", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If
        End If
        m_lReturn = m_oRiskTypeRuleSet.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        m_oRiskTypeRuleSet.RiskTypeID = m_lRiskTypeID

        m_oRiskTypeRuleSet.RuleType = "RL"

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Lapse Scripts)"
        m_oRiskTypeRuleSet.UniqueId = m_sUniqueId
        m_oRiskTypeRuleSet.ScreenHierarchy = m_sScreenHierarchy

        m_lReturn = m_oRiskTypeRuleSet.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid

        If m_oRiskTypeRuleSet.Status = gPMConstants.PMEReturnCode.PMCancel Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If


        'get data back from risk type rule set object
        If IsArray(m_vRenewalLapseRule) Then
            ReDim Preserve m_vRenewalLapseRule(16, m_vRenewalLapseRule.GetUpperBound(1) + 1)
        Else
            ReDim m_vRenewalLapseRule(16, 0)
        End If

        m_vRenewalLapseRule(ACFieldPosRiskTypeRuleSetID, m_vRenewalLapseRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.RiskTypeRuleSetID
        m_vRenewalLapseRule(ACFieldPosCode, m_vRenewalLapseRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Code
        m_vRenewalLapseRule(ACFieldPosDescription, m_vRenewalLapseRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Description
        m_vRenewalLapseRule(ACFieldPosEffectiveDate, m_vRenewalLapseRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.RuleEffectiveDate
        m_vRenewalLapseRule(ACFieldPosFileName, m_vRenewalLapseRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.FileName
        m_vRenewalLapseRule(ACFieldPosLive, m_vRenewalLapseRule.GetUpperBound(1)) = m_oRiskTypeRuleSet.Live
        m_vRenewalLapseRule(ACFieldPosIsDeleted, m_vRenewalLapseRule.GetUpperBound(1)) = gPMConstants.PMEReturnCode.PMFalse
        m_vRenewalLapseRule(ACFieldPosTypeID, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.RuleTypeID
        m_vRenewalLapseRule(ACFieldPosType, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.RuleTypeDescription
        m_vRenewalLapseRule(ACFieldPosDREExecutorURL, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.DREExecutorURL
        m_vRenewalLapseRule(ACFieldPosDREDefaultToken, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.DREDefaultToken



        m_lReturn = LoadRules(v_vDataArray:=m_vRenewalLapseRule, r_lvwListView:=lvwRenewalLapseRule)
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdEditRenLapseRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdEditRenLapseRule.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        cmdOK.Enabled = False
        cmdCancel.Enabled = False

        'get position of current rule

        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRenewalLapseRule.Items.Item(Me.lvwRenewalLapseRule.FocusedItem.Index).Tag)

        'Create Risk Type object if not already done so
        If m_oRiskTypeRuleSet Is Nothing Then

            ' Get an instance of the Risk Type interface object via
            ' the public object manager.
            Dim temp_m_oRiskTypeRuleSet As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oRiskTypeRuleSet, sClassName:="iPMURiskTypeRuleSet.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            m_oRiskTypeRuleSet = temp_m_oRiskTypeRuleSet
        End If

        m_lReturn = m_oRiskTypeRuleSet.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        'pass selected details to Risk Type object
        m_oRiskTypeRuleSet.RiskTypeRuleSetID = CInt(m_vRenewalLapseRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem))
        m_oRiskTypeRuleSet.RiskTypeID = m_lRiskTypeID
        m_oRiskTypeRuleSet.RuleType = "RL"
        m_oRiskTypeRuleSet.RuleTypeID = ToSafeLong(m_vRenewalLapseRule(ACFieldPosTypeID, lSelectedItem))
        m_oRiskTypeRuleSet.DREExecutorURL = ToSafeString(m_vRenewalLapseRule(ACFieldPosDREExecutorURL, lSelectedItem))
        m_oRiskTypeRuleSet.DREDefaultToken = ToSafeString(m_vRenewalLapseRule(ACFieldPosDREDefaultToken, lSelectedItem))

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Lapse Scripts)"
        m_oRiskTypeRuleSet.UniqueId = m_sUniqueId
        m_oRiskTypeRuleSet.ScreenHierarchy = m_sScreenHierarchy

        m_lReturn = m_oRiskTypeRuleSet.Start()

        cmdOK.Enabled = True
        cmdCancel.Enabled = True

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        Me.Refresh()

        'If not cancelled, add to grid
        If m_oRiskTypeRuleSet.Status = gPMConstants.PMEReturnCode.PMCancel Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        'get data back from risk type rule set object

        m_vRenewalLapseRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem) = m_oRiskTypeRuleSet.RiskTypeRuleSetID

        m_vRenewalLapseRule(ACFieldPosCode, lSelectedItem) = m_oRiskTypeRuleSet.Code
        m_vRenewalLapseRule(ACFieldPosDescription, lSelectedItem) = m_oRiskTypeRuleSet.Description
        m_vRenewalLapseRule(ACFieldPosFileName, lSelectedItem) = m_oRiskTypeRuleSet.FileName
        m_vRenewalLapseRule(ACFieldPosLive, lSelectedItem) = m_oRiskTypeRuleSet.Live
        m_vRenewalLapseRule(ACFieldPosIsDeleted, lSelectedItem) = gPMConstants.PMEReturnCode.PMFalse
        m_vRenewalLapseRule(ACFieldPosTypeID, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.RuleTypeID
        m_vRenewalLapseRule(ACFieldPosDREExecutorURL, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.DREExecutorURL
        m_vRenewalLapseRule(ACFieldPosDREDefaultToken, UBound(m_vRenewalLapseRule, 2)) = m_oRiskTypeRuleSet.DREDefaultToken
        m_vRenewalLapseRule(ACFieldPosEffectiveDate, lSelectedItem) = m_oRiskTypeRuleSet.RuleEffectiveDate
        m_lReturn = LoadRules(v_vDataArray:=m_vRenewalLapseRule, r_lvwListView:=lvwRenewalLapseRule)
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    End Sub

    Private Sub cmdDeleteRenLapseRule_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDeleteRenLapseRule.Click
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Dim iDelete As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue


        Dim lSelectedItem As Integer = Convert.ToString(Me.lvwRenewalLapseRule.Items.Item(Me.lvwRenewalLapseRule.FocusedItem.Index).Tag)


        If Me.lvwRenewalLapseRule.FocusedItem.ForeColor.Equals(Color.Gray) Then
            iDelete = gPMConstants.PMEReturnCode.PMFalse
        End If

        If String.IsNullOrEmpty(m_sUniqueId) Then
            m_sUniqueId = GetUniqueID()
        End If

        m_sScreenHierarchy = $"Risk Type({txtCode.Text.Trim()})/Rules(Lapse Scripts)/Code({lvwRenewalLapseRule.FocusedItem.Text.Trim()})"

        m_lReturn = m_oBusiness.DelRiskTypeRuleSet(v_lRiskTypeID:=m_lRiskTypeID, v_lRiskTypeRuleSetID:=m_vRenewalLapseRule(ACFieldPosRiskTypeRuleSetID, lSelectedItem), v_iIsDeleted:=iDelete, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_vRenewalLapseRule(ACFieldPosIsDeleted, lSelectedItem) = iDelete
            m_lReturn = LoadRules(v_vDataArray:=m_vRenewalLapseRule, r_lvwListView:=lvwRenewalLapseRule)
        End If

        cmdEditRenLapseRule.Enabled = False
        cmdDeleteRenLapseRule.Enabled = False
        cmdAddRenLapseRule.Enabled = False

        cmdCancel.Enabled = True
        cmdOK.Enabled = True
        cmdApply.Enabled = True
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
    End Sub

    Private Sub lvwRenewalLapseRule_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRenewalLapseRule.Click
        If Me.lvwRenewalLapseRule.Items.Count > 0 Then

            If Me.lvwRenewalLapseRule.FocusedItem.ForeColor.Equals(Color.Gray) Then
                cmdDeleteRenLapseRule.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACUndeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                cmdEditRenLapseRule.Enabled = False
            Else
                cmdDeleteRenLapseRule.Text = CStr(iPMFunc.GetResData(g_iLanguageID, ACDeleteButton, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
        End If
    End Sub

    Private Sub lvwRenewalLapseRule_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwRenewalLapseRule.Enter
        cmdEditRule.Enabled = False
        cmdAddRule.Enabled = False
        cmdDeleteRule.Enabled = False
        cmdEditRenRule.Enabled = False
        cmdAddRenRule.Enabled = False
        cmdDeleteRenRule.Enabled = False
    End Sub

    Private Sub lvwRenewalLapseRule_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwRenewalLapseRule.MouseDown
        Dim Button As Integer = CInt(EventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = EventArgs.X
        Dim y As Single = EventArgs.Y
        'end
        If Me.lvwRenewalLapseRule.Items.Count > 0 Then
            'Not if we're viewing, thank you very much
            If Task <> gPMConstants.PMEComponentAction.PMView Then
                If Me.lvwRenewalLapseRule.GetItemAt(x, y) Is Nothing Then
                    cmdDeleteRenLapseRule.Enabled = False
                    cmdEditRenLapseRule.Enabled = False
                Else
                    cmdEditRenLapseRule.Enabled = True
                    cmdDeleteRenLapseRule.Enabled = True
                End If
            End If
        End If

        cmdAddRenLapseRule.Enabled = True
    End Sub


    Private Sub chkAttachClaimOutsideOfPolicyPeriod_CheckStateChanged(sender As Object, e As EventArgs) Handles chkAttachClaimOutsideOfPolicyPeriod.CheckStateChanged
        cmdApply.Enabled = True
    End Sub

    Private Sub cboClaimsCoverBasis_ItemCodeChange() Handles cboClaimsCoverBasis.ItemCodeChange
        If cboClaimsCoverBasis.ItemCode.Trim().ToUpper() = "STD" Then
            chkAttachClaimOutsideOfPolicyPeriod.Enabled = True
            cmdApply.Enabled = True
        Else
            chkAttachClaimOutsideOfPolicyPeriod.Enabled = False
            cmdApply.Enabled = True
            chkAttachClaimOutsideOfPolicyPeriod.Checked = False
        End If
    End Sub

    Private Sub frmInterface_Click(sender As Object, e As EventArgs) Handles Me.Click

    End Sub
End Class
