Option Strict Off
Option Explicit On
' developer guide no. 129
Imports SSP.Shared
'<System.Runtime.InteropServices.ProgId("Scheme_NET.Scheme")> _
Public NotInheritable Class Scheme
    ' ***************************************************************** '
    ' Class Name: Scheme
    '
    ' Date:  15/06/1999
    '
    ' Description: Class to contain any business rules for accessing
    '              GIS_Scheme table.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer

    ' RDC 22092003
    Private m_sClassOfBusiness As String = ""
    ' ************************************************


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Scheme"

    ' Constants to define stored procedure statements for updating GIS_Scheme table.
    ' TB 14/5/2001 - added 2 extra columns to ADD AND UPD
    'developer guide no 39. 
    Private Const SQL_SCHEME_ADD As String = "spe_GIS_Scheme_add"
    Private Const SQL_SCHEME_UPD As String = "spe_GIS_Scheme_upd"

    'sj 24/05/2001 - start
    'Add new stored procedures
    'developer guide no 39. 
    Private Const SQL_SCHEME_ADD_V2 As String = "spu_GIS_Scheme_add_V2"
    Private Const SQL_SCHEME_UPD_V2 As String = "spu_GIS_Scheme_upd_V2"

    'sj 24/05/2001 - end
    'TF081001
    'developer guide no 39.
    Private Const SQL_SCHEME_UPD_V3 As String = "spu_GIS_Scheme_upd_V3"

    'developer guide no 39.
    Private Const SQL_SCHEME_DEL As String = "spe_GIS_Scheme_del"

    ' constants to define SQL call statements for returning lists of schemes
    'developer guide no 39.
    Private Const SQL_SCHEME_ALL_OF_TYPE_SEL As String = "spu_GIS_Scheme_All_Type_sel"
    'sj 24/05/2001 - start
    'Make this non Erwin generated
    'developer guide no 39.
    Private Const SQL_SCHEME_SINGLE_SEL As String = "spu_GIS_Scheme_sel"
    'sj 24/05/2001 - end
    'developer guide no 39.

    Private Const SQL_SCHEME_NAME_ONLY_TYPE_SEL As String = "spu_GIS_Scheme_Name_Type_sel"
    Private Const SQL_SCHEME_NAME_ONLY_TYPE_INS_SEL As String = "spu_GIS_Scheme_Name_TypeIns_sel"
    Private Const SQL_SCHEME_AGENCY_SEL As String = "spu_GIS_Scheme_Agency_sel"
    Private Const SQL_SCHEME_NAME_GROUP_SCHEMES_SEL As String = "spu_GIS_Scheme_GrpSchemes_sel"

    'sj 9/6/2000 - start

    'developer guide no 39.
    Private Const SQL_SCHEME_ALL_LEGACY_SCHEMES_SEL As String = "spu_GIS_Scheme_LegacySchms_sel"
    'sj 9/6/2000 - end
    'RT180700 - Start
    'developer guide no 39.
    Private Const SQL_SCHEME_ALL_OF_TYPE_WITH_INSURER_NAME_SEL As String = "spu_GIS_Scheme_Type_InsName_sel"
    Private Const SQL_SCHEME_FULL_COL_LIST As String = "spu_GIS_Scheme_Full_Col_List"
    'RT180700 - End
    'sj 26/7/2000 - start
    Private Const SQL_FULL_LINK_BRANCH_SCHEME_SEL As String = "spu_GIS_Full_link_Br_Sch_sel"
    'sj 26/7/2000 - end
    'TB 14/5/2001
    'developer guide no 39.
    Private Const SQL_SCHEME_LEGACY_SCHEMES_BY_CLASS As String = "spu_GIS_LegacySchms_by_Class"

    Private Const SQL_SCHEME_NAME_WITH_INS_SEL As String = "spu_GIS_Scheme_AllWithIns_sel"
    ' TB 23/5/01
    Private Const SQL_NAME_FULL_LINK_BRANCH_CLASS As String = "FullLinkBranchSchemeByClass"

    ' TB 23/5/01
    'developer guide no 39. 
    Private Const SQL_FULL_LINK_BRANCH_CLASS As String = "spu_GIS_Full_Link_Br_Sch_New"

    ' TB 19/6/01
    'developer guide no 39.
    Private Const SQL_SCHEME_ALL_OF_CLASS_WITH_INSURER_NAME_SEL As String = "spu_GIS_Scheme_Class_InsName_sel"
    Private Const SQL_NAME_SCHEME_ALL_OF_CLASS_SEL As String = "SchemeClassSelWithInsName"


    ' Constants to define names of above stored procedure statements for scheme table.
    Private Const SQL_NAME_SCHEME_ADD As String = "SchemeAdd"
    Private Const SQL_NAME_SCHEME_DEL As String = "SchemeDelete"
    Private Const SQL_NAME_SCHEME_UPD As String = "SchemeUpdate"

    ' constants to define names of SQL call statements for lists of schemes
    Private Const SQL_NAME_SCHEME_ALL_OF_TYPE_SEL As String = "SchemeTypeSel"
    'sj 26/7/2000 - start
    Private Const SQL_NAME_FULL_LINK_BRANCH_SCHEME_SEL As String = "FullLinkBranchSchemeSel"
    'sj 27/7/2000 - end
    ' TB 14/5/2001
    Private Const SQL_NAME_LEGACY_SCHEMES_BY_CLASS As String = "LegacySchmsByClass"

    'RT180700 - Start
    Private Const SQL_NAME_SCHEME_ALL_OF_TYPE_WITH_INSURER_NAME_SEL As String = "SchemeTypeSelWithInsName"
    Private Const SQL_NAME_SCHEME_FULL_COL_LIST As String = "SchemeFullColList"
    'RT180700 - End
    Private Const SQL_NAME_SCHEME_SINGLE_SEL As String = "SchemeSingleSel"
    Private Const SQL_NAME_SCHEME_NAME_ONLY_TYPE_SEL As String = "SchemeNameOnlyType"
    Private Const SQL_NAME_SCHEME_NAME_ONLY_TYPE_INS_SEL As String = "SchemeNameOnlyTypeIns"
    Private Const SQL_NAME_SCHEME_AGENCY_SEL As String = "SchemeAgency"
    Private Const SQL_NAME_SCHEME_NAME_GROUP_SCHEMES_SEL As String = "SchemeGroupSchemes"
    Private Const SQL_NAME_SCHEME_ALL_LEGACY_SCHEMES_SEL As String = "LegacySchemes"
    Private Const SQL_NAME_SCHEME_NAME_WITH_INS_SEL As String = "AllSchemesWithIns"

    Private Const GIS_SCHEME_ID As Integer = 0
    Private Const GIS_QUOTE_ENGINE_ID As Integer = 1
    Private Const GIS_BUSINESS_TYPE_ID As Integer = 2
    Private Const GIS_INSURER_ID As Integer = 3
    Private Const SCHEME_NO As Integer = 4
    Private Const SCHEME_VER As Integer = 5
    Private Const SCHEME_STATUS As Integer = 6
    Private Const START_DATE As Integer = 7
    Private Const SCHEME_DESC As Integer = 8
    Private Const PRIORITY As Integer = 9
    Private Const AGENCY_CODE As Integer = 10
    Private Const PRODUCT_CODE As Integer = 11
    Private Const ACTIVATION_LEVEL As Integer = 12
    Private Const PRINTING_PRIVILEGES As Integer = 13
    Private Const BROKER_GROUP As Integer = 14
    Private Const COMMISION_PERC As Integer = 15
    Private Const QUOTE_DAY_NUM As Integer = 16
    Private Const SELECTION_DAY_NUM As Integer = 17
    Private Const INVITE_DAY_NUM As Integer = 18
    Private Const CONFIRM_DAY_NUM As Integer = 19
    Private Const LAPSE_DAY_NUM As Integer = 20
    Private Const MAX_CHANGE_NUM As Integer = 21
    Private Const MIN_CHANGE_NUM As Integer = 22
    Private Const EXPIRY_DATE As Integer = 23
    Private Const QM_INSURER_REF As Integer = 24
    Private Const SCHEME_TYPE_FLAGS As Integer = 25
    Private Const FILENAME As Integer = 26
    Private Const EDI_MAIL_BOX As Integer = 27
    Private Const REFER_EMAIL_ADDRESS As Integer = 28
    Private Const REFER_FAX_NUMBER As Integer = 29
    Private Const SCHEME_TYPE As Integer = 30
    Private Const SCHEME_VARIANT As Integer = 31
    'sj 24/05/2001 - start
    Private Const SCHEME_DICT_VER As Integer = 32
    Private Const SCHEME_CLASS_OF_BUSINESS As Integer = 33
    Private Const COUNTRY_ID As Integer = 34
    'TF081001
    Private Const PRE_SELECTION_DAY_NUM As Integer = 35
    Private Const REMINDER_DAY_NUM As Integer = 36
    'sj 24/05/2001 - end

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Parameter variables for getting scheme lists.
    ' Is filled by ExtractParameterValues
    Private m_lBusinessType As Integer
    Private m_lSchemeID As Integer
    Private m_lSchemeGroupID As Integer
    Private m_lInsurerID As Integer
    Private m_lSourceId As Integer 'sj 26/7/2000
    'Private m_sClassOfBusiness As String ' TB 14/05/2001
    Private m_lCountryId As Integer ' TB 19/6/01

    ' PRIVATE Data Members (End)

    ' PUBLIC Properties (Begin)

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property

    ' RDC 22092003
    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_sClassOfBusiness = Value
        End Set
    End Property

    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)
    Public Function Add(ByRef r_lSchemeID As Integer, ByVal v_lQuoteEngineID As Integer, ByVal v_lBusinessType As Integer, ByVal v_lInsurerID As Integer, ByVal v_lSchemeNo As Integer, ByVal v_iSchemeVer As Integer, ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, ByVal v_sSchemeDesc As String, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        ' Adds a scheme to the database.
        ' Fields that are nullable optional and will store a Null if not supplied.

        Dim result As Integer = 0
        Try

            ' Add parameters needed for stored procedure.
            ' Scheme id is an output parameter that will be return by the
            ' stored procedure.
            If SetupSchemeParameters(gPMConstants.PMEParameterDirection.PMParamOutput, r_lSchemeID, v_lQuoteEngineID, v_lBusinessType, v_lInsurerID, v_lSchemeNo, v_iSchemeVer, v_iSchemeStatus, v_dtStartDate, v_sSchemeDesc, v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vQuoteDayNum, v_vSelectionDayNum, v_vInviteDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMaxChangeNum, v_vMinChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add scheme to database.
            If m_oDatabase.SQLAction(SQL_SCHEME_ADD, SQL_NAME_SCHEME_ADD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' return scheme id.
            r_lSchemeID = m_oDatabase.Parameters.Item(GISSB_PARAM_NAME_GIS_SCHEME_ID).Value


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function Update(ByVal v_lSchemeID As Integer, ByVal v_lQuoteEngineID As Integer, ByVal v_lBusinessType As Integer, ByVal v_lInsurerID As Integer, ByVal v_lSchemeNo As Integer, ByVal v_iSchemeVer As Integer, ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, ByVal v_sSchemeDesc As String, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        ' Updates a GIS_scheme record within the database as keyed by Scheme ID.
        ' Fields that are Nullable within the database are optional to this
        ' procedure and will supply null to the procedure if not supplied.

        Dim result As Integer = 0
        Try


            ' add add parameters needed for stored procedure
            ' Scheme ID is also an input parameter.
            If SetupSchemeParameters(gPMConstants.PMEParameterDirection.PMParamInput, v_lSchemeID, v_lQuoteEngineID, v_lBusinessType, v_lInsurerID, v_lSchemeNo, v_iSchemeVer, v_iSchemeStatus, v_dtStartDate, v_sSchemeDesc, v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vQuoteDayNum, v_vSelectionDayNum, v_vInviteDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMaxChangeNum, v_vMinChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' update scheme record.
            If m_oDatabase.SQLAction(SQL_SCHEME_UPD, SQL_NAME_SCHEME_UPD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'sj 24/05/2001 - start
    Public Function Add_V2(ByRef r_lSchemeID As Integer, ByVal v_lBusinessType As Integer, ByVal v_lInsurerID As Integer, ByVal v_lSchemeNo As Integer, ByVal v_iSchemeVer As Integer, ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, ByVal v_sSchemeDesc As String, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vDictVer As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vClassOfBusiness As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vHousekeepDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPreSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vReminderDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vRenewDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vIsInsurerLead As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vCountryId As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        ' Adds a scheme to the database.
        ' Fields that are nullable optional and will store a Null if not supplied.

        Dim result As Integer = 0
        Try

            ' Add parameters needed for stored procedure.
            ' Scheme id is an output parameter that will be return by the
            ' stored procedure.
            If SetupSchemeParameters(gPMConstants.PMEParameterDirection.PMParamOutput, r_lSchemeID, 0, v_lBusinessType, v_lInsurerID, v_lSchemeNo, v_iSchemeVer, v_iSchemeStatus, v_dtStartDate, v_sSchemeDesc, v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vQuoteDayNum, v_vSelectionDayNum, v_vInviteDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMaxChangeNum, v_vMinChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant, v_vDictVer, v_vClassOfBusiness, v_vCountryId) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add scheme to database.
            If m_oDatabase.SQLAction(SQL_SCHEME_ADD_V2, SQL_NAME_SCHEME_ADD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' return scheme id.
            r_lSchemeID = m_oDatabase.Parameters.Item(GISSB_PARAM_NAME_GIS_SCHEME_ID).Value


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add_V2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add_V2", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function Update_V2(ByVal v_lSchemeID As Integer, ByVal v_lBusinessType As Integer, ByVal v_lInsurerID As Integer, ByVal v_lSchemeNo As Integer, ByVal v_iSchemeVer As Integer, ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, ByVal v_sSchemeDesc As String, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vDictVer As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vClassOfBusiness As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vHousekeepDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPreSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vReminderDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vRenewDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vIsInsurerLead As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vCountryId As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        ' Updates a GIS_scheme record within the database as keyed by Scheme ID.
        ' Fields that are Nullable within the database are optional to this
        ' procedure and will supply null to the procedure if not supplied.

        Dim result As Integer = 0
        Try


            ' add add parameters needed for stored procedure
            ' Scheme ID is also an input parameter.
            If SetupSchemeParameters(gPMConstants.PMEParameterDirection.PMParamInput, v_lSchemeID, 0, v_lBusinessType, v_lInsurerID, v_lSchemeNo, v_iSchemeVer, v_iSchemeStatus, v_dtStartDate, v_sSchemeDesc, v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vQuoteDayNum, v_vSelectionDayNum, v_vInviteDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMaxChangeNum, v_vMinChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant, v_vDictVer, v_vClassOfBusiness, v_vCountryId) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' update scheme record.
            If m_oDatabase.SQLAction(SQL_SCHEME_UPD_V2, SQL_NAME_SCHEME_UPD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update_V2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update_V2", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'sj 24/05/2001 - end

    ' Line that appears below must be replaced due to bug with VB and long Parameter Lists
    ' Optional ByVal v_vReferEmailAddress = PARAMETER_NOT_PRESENT_STR,
    Public Function UpdateDetail(ByVal v_lSchemeID As Integer, Optional ByVal v_lQuoteEngineID As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lBusinessType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lInsurerID As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lSchemeNo As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_iSchemeVer As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_iSchemeStatus As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_dtStartDate As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_sSchemeDesc As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        Dim result As Integer = 0
        Dim vParameters As Object = Nothing
        Dim r_vSchemes(,) As Object = Nothing
        Dim lCnt As Integer

        ' Updates a GIS_scheme record within the database as keyed by Scheme ID.
        ' Fields that are Nullable within the database are optional to this
        ' procedure and will supply null to the procedure if not supplied.

        Try

            ' set parameters for a business type and an insurer.
            ReDim vParameters(1, 0)

            vParameters(0, 0) = GISSB_PARAM_NAME_GIS_SCHEME_ID

            vParameters(1, 0) = v_lSchemeID



            If GetList(bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_SINGLE_SCHEME_FULL_COL_LIST, r_vSchemes, vParameters) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lCnt = r_vSchemes.GetUpperBound(1)



            v_lQuoteEngineID = If(CDbl(v_lQuoteEngineID) <> PARAMETER_NOT_PRESENT_NO, v_lQuoteEngineID, r_vSchemes(GIS_QUOTE_ENGINE_ID, lCnt))


            v_lBusinessType = If(CDbl(v_lBusinessType) <> PARAMETER_NOT_PRESENT_NO, v_lBusinessType, r_vSchemes(GIS_BUSINESS_TYPE_ID, lCnt))


            v_lInsurerID = If(CDbl(v_lInsurerID) <> PARAMETER_NOT_PRESENT_NO, v_lInsurerID, r_vSchemes(GIS_INSURER_ID, lCnt))


            v_lSchemeNo = If(CDbl(v_lSchemeNo) <> PARAMETER_NOT_PRESENT_NO, v_lSchemeNo, r_vSchemes(SCHEME_NO, lCnt))


            v_iSchemeVer = If(CDbl(v_iSchemeVer) <> PARAMETER_NOT_PRESENT_NO, v_iSchemeVer, r_vSchemes(SCHEME_VER, lCnt))


            v_iSchemeStatus = If(CDbl(v_iSchemeStatus) <> PARAMETER_NOT_PRESENT_NO, v_iSchemeStatus, r_vSchemes(SCHEME_STATUS, lCnt))


            v_dtStartDate = If(CStr(v_dtStartDate) <> PARAMETER_NOT_PRESENT_STR, v_dtStartDate, r_vSchemes(START_DATE, lCnt))


            v_sSchemeDesc = If(CStr(v_sSchemeDesc) <> PARAMETER_NOT_PRESENT_STR, v_sSchemeDesc, r_vSchemes(SCHEME_DESC, lCnt))


            v_vPriority = If(CDbl(v_vPriority) <> PARAMETER_NOT_PRESENT_NO, v_vPriority, r_vSchemes(PRIORITY, lCnt))


            v_vAgencyCode = If(CStr(v_vAgencyCode) <> PARAMETER_NOT_PRESENT_STR, v_vAgencyCode, r_vSchemes(AGENCY_CODE, lCnt))


            v_vProductCode = If(CStr(v_vProductCode) <> PARAMETER_NOT_PRESENT_STR, v_vProductCode, r_vSchemes(PRODUCT_CODE, lCnt))


            v_vActivationLevel = If(CDbl(v_vActivationLevel) <> PARAMETER_NOT_PRESENT_NO, v_vActivationLevel, r_vSchemes(ACTIVATION_LEVEL, lCnt))


            v_vPrintingPrivileges = If(CDbl(v_vPrintingPrivileges) <> PARAMETER_NOT_PRESENT_NO, v_vPrintingPrivileges, r_vSchemes(PRINTING_PRIVILEGES, lCnt))


            v_vBrokerGroup = If(CStr(v_vBrokerGroup) <> PARAMETER_NOT_PRESENT_STR, v_vBrokerGroup, r_vSchemes(BROKER_GROUP, lCnt))


            v_vCommisionPerc = If(CDbl(v_vCommisionPerc) <> PARAMETER_NOT_PRESENT_NO, v_vCommisionPerc, r_vSchemes(COMMISION_PERC, lCnt))


            v_vQuoteDayNum = If(CDbl(v_vQuoteDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vQuoteDayNum, r_vSchemes(QUOTE_DAY_NUM, lCnt))


            v_vSelectionDayNum = If(CDbl(v_vSelectionDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vSelectionDayNum, r_vSchemes(SELECTION_DAY_NUM, lCnt))


            v_vInviteDayNum = If(CDbl(v_vInviteDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vInviteDayNum, r_vSchemes(INVITE_DAY_NUM, lCnt))


            v_vConfirmDayNum = If(CDbl(v_vConfirmDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vConfirmDayNum, r_vSchemes(CONFIRM_DAY_NUM, lCnt))


            v_vLapseDayNum = If(CDbl(v_vLapseDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vLapseDayNum, r_vSchemes(LAPSE_DAY_NUM, lCnt))


            v_vMaxChangeNum = If(CDbl(v_vMaxChangeNum) <> PARAMETER_NOT_PRESENT_NO, v_vMaxChangeNum, r_vSchemes(MAX_CHANGE_NUM, lCnt))


            v_vMinChangeNum = If(CDbl(v_vMinChangeNum) <> PARAMETER_NOT_PRESENT_NO, v_vMinChangeNum, r_vSchemes(MIN_CHANGE_NUM, lCnt))


            v_vExpiryDate = If(CDbl(v_vExpiryDate) <> PARAMETER_NOT_PRESENT_NO, v_vExpiryDate, r_vSchemes(EXPIRY_DATE, lCnt))


            v_vQMInsurerRef = If(CStr(v_vQMInsurerRef) <> PARAMETER_NOT_PRESENT_STR, v_vQMInsurerRef, r_vSchemes(QM_INSURER_REF, lCnt))


            v_vSchemeTypeFlags = If(CDbl(v_vSchemeTypeFlags) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeTypeFlags, r_vSchemes(SCHEME_TYPE_FLAGS, lCnt))


            v_vFilename = If(CStr(v_vFilename) <> PARAMETER_NOT_PRESENT_STR, v_vFilename, r_vSchemes(FILENAME, lCnt))


            v_vEdiMailBox = If(CStr(v_vEdiMailBox) <> PARAMETER_NOT_PRESENT_STR, v_vEdiMailBox, r_vSchemes(EDI_MAIL_BOX, lCnt))


            v_vReferEmailAddress = If(CStr(v_vReferEmailAddress) <> PARAMETER_NOT_PRESENT_STR, v_vReferEmailAddress, r_vSchemes(REFER_EMAIL_ADDRESS, lCnt))


            v_vReferFaxNumber = If(CStr(v_vReferFaxNumber) <> PARAMETER_NOT_PRESENT_STR, v_vReferFaxNumber, r_vSchemes(REFER_FAX_NUMBER, lCnt))


            v_vSchemeType = If(CDbl(v_vSchemeType) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeType, r_vSchemes(SCHEME_TYPE, lCnt))


            v_vSchemeVariant = If(CDbl(v_vSchemeVariant) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeVariant, r_vSchemes(SCHEME_VARIANT, lCnt))


            ' add add parameters needed for stored procedure
            ' Scheme ID is also an input parameter.






            If SetupSchemeParameters(gPMConstants.PMEParameterDirection.PMParamInput, v_lSchemeID, v_lQuoteEngineID, v_lBusinessType, CInt(v_lInsurerID), CInt(v_lSchemeNo), CInt(v_iSchemeVer), CInt(v_iSchemeStatus), CDate(v_dtStartDate), CStr(v_sSchemeDesc), v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vQuoteDayNum, v_vSelectionDayNum, v_vInviteDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMaxChangeNum, v_vMinChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' UpdateDetail scheme record.
            If m_oDatabase.SQLAction(SQL_SCHEME_UPD, SQL_NAME_SCHEME_UPD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    'sj 24/05/2001 - start
    Public Function UpdateDetail_V2(ByVal v_lSchemeID As Integer, Optional ByVal v_lQuoteEngineID As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lBusinessType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lInsurerID As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lSchemeNo As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_iSchemeVer As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_iSchemeStatus As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_dtStartDate As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_sSchemeDesc As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vDictVer As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vClassOfBusiness As Object = PARAMETER_NOT_PRESENT_STR, Optional ByRef v_vCountryId As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        Dim result As Integer = 0
        Dim vParameters As Object = Nothing
        Dim r_vSchemes(,) As Object = Nothing
        Dim lCnt As Integer

        ' Updates a GIS_scheme record within the database as keyed by Scheme ID.
        ' Fields that are Nullable within the database are optional to this
        ' procedure and will supply null to the procedure if not supplied.

        Try

            ' set parameters for a business type and an insurer.
            ReDim vParameters(1, 0)

            vParameters(0, 0) = GISSB_PARAM_NAME_GIS_SCHEME_ID

            vParameters(1, 0) = v_lSchemeID



            If GetList(bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_SINGLE_SCHEME_FULL_COL_LIST, r_vSchemes, vParameters) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lCnt = r_vSchemes.GetUpperBound(1)



            v_lQuoteEngineID = If(CDbl(v_lQuoteEngineID) <> PARAMETER_NOT_PRESENT_NO, v_lQuoteEngineID, r_vSchemes(GIS_QUOTE_ENGINE_ID, lCnt))


            v_lBusinessType = If(CDbl(v_lBusinessType) <> PARAMETER_NOT_PRESENT_NO, v_lBusinessType, r_vSchemes(GIS_BUSINESS_TYPE_ID, lCnt))


            v_lInsurerID = If(CDbl(v_lInsurerID) <> PARAMETER_NOT_PRESENT_NO, v_lInsurerID, r_vSchemes(GIS_INSURER_ID, lCnt))


            v_lSchemeNo = If(CDbl(v_lSchemeNo) <> PARAMETER_NOT_PRESENT_NO, v_lSchemeNo, r_vSchemes(SCHEME_NO, lCnt))


            v_iSchemeVer = If(CDbl(v_iSchemeVer) <> PARAMETER_NOT_PRESENT_NO, v_iSchemeVer, r_vSchemes(SCHEME_VER, lCnt))


            v_iSchemeStatus = If(CDbl(v_iSchemeStatus) <> PARAMETER_NOT_PRESENT_NO, v_iSchemeStatus, r_vSchemes(SCHEME_STATUS, lCnt))


            v_dtStartDate = If(CStr(v_dtStartDate) <> PARAMETER_NOT_PRESENT_STR, v_dtStartDate, r_vSchemes(START_DATE, lCnt))


            v_sSchemeDesc = If(CStr(v_sSchemeDesc) <> PARAMETER_NOT_PRESENT_STR, v_sSchemeDesc, r_vSchemes(SCHEME_DESC, lCnt))


            v_vPriority = If(CDbl(v_vPriority) <> PARAMETER_NOT_PRESENT_NO, v_vPriority, r_vSchemes(PRIORITY, lCnt))


            v_vAgencyCode = If(CStr(v_vAgencyCode) <> PARAMETER_NOT_PRESENT_STR, v_vAgencyCode, r_vSchemes(AGENCY_CODE, lCnt))


            v_vProductCode = If(CStr(v_vProductCode) <> PARAMETER_NOT_PRESENT_STR, v_vProductCode, r_vSchemes(PRODUCT_CODE, lCnt))


            v_vActivationLevel = If(CDbl(v_vActivationLevel) <> PARAMETER_NOT_PRESENT_NO, v_vActivationLevel, r_vSchemes(ACTIVATION_LEVEL, lCnt))


            v_vPrintingPrivileges = If(CDbl(v_vPrintingPrivileges) <> PARAMETER_NOT_PRESENT_NO, v_vPrintingPrivileges, r_vSchemes(PRINTING_PRIVILEGES, lCnt))


            v_vBrokerGroup = If(CStr(v_vBrokerGroup) <> PARAMETER_NOT_PRESENT_STR, v_vBrokerGroup, r_vSchemes(BROKER_GROUP, lCnt))


            v_vCommisionPerc = If(CDbl(v_vCommisionPerc) <> PARAMETER_NOT_PRESENT_NO, v_vCommisionPerc, r_vSchemes(COMMISION_PERC, lCnt))


            v_vQuoteDayNum = If(CDbl(v_vQuoteDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vQuoteDayNum, r_vSchemes(QUOTE_DAY_NUM, lCnt))


            v_vSelectionDayNum = If(CDbl(v_vSelectionDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vSelectionDayNum, r_vSchemes(SELECTION_DAY_NUM, lCnt))


            v_vInviteDayNum = If(CDbl(v_vInviteDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vInviteDayNum, r_vSchemes(INVITE_DAY_NUM, lCnt))


            v_vConfirmDayNum = If(CDbl(v_vConfirmDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vConfirmDayNum, r_vSchemes(CONFIRM_DAY_NUM, lCnt))


            v_vLapseDayNum = If(CDbl(v_vLapseDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vLapseDayNum, r_vSchemes(LAPSE_DAY_NUM, lCnt))


            v_vMaxChangeNum = If(CDbl(v_vMaxChangeNum) <> PARAMETER_NOT_PRESENT_NO, v_vMaxChangeNum, r_vSchemes(MAX_CHANGE_NUM, lCnt))


            v_vMinChangeNum = If(CDbl(v_vMinChangeNum) <> PARAMETER_NOT_PRESENT_NO, v_vMinChangeNum, r_vSchemes(MIN_CHANGE_NUM, lCnt))


            v_vExpiryDate = If(CDbl(v_vExpiryDate) <> PARAMETER_NOT_PRESENT_NO, v_vExpiryDate, r_vSchemes(EXPIRY_DATE, lCnt))


            v_vQMInsurerRef = If(CStr(v_vQMInsurerRef) <> PARAMETER_NOT_PRESENT_STR, v_vQMInsurerRef, r_vSchemes(QM_INSURER_REF, lCnt))


            v_vSchemeTypeFlags = If(CDbl(v_vSchemeTypeFlags) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeTypeFlags, r_vSchemes(SCHEME_TYPE_FLAGS, lCnt))


            v_vFilename = If(CStr(v_vFilename) <> PARAMETER_NOT_PRESENT_STR, v_vFilename, r_vSchemes(FILENAME, lCnt))


            v_vEdiMailBox = If(CStr(v_vEdiMailBox) <> PARAMETER_NOT_PRESENT_STR, v_vEdiMailBox, r_vSchemes(EDI_MAIL_BOX, lCnt))


            v_vReferEmailAddress = If(CStr(v_vReferEmailAddress) <> PARAMETER_NOT_PRESENT_STR, v_vReferEmailAddress, r_vSchemes(REFER_EMAIL_ADDRESS, lCnt))


            v_vReferFaxNumber = If(CStr(v_vReferFaxNumber) <> PARAMETER_NOT_PRESENT_STR, v_vReferFaxNumber, r_vSchemes(REFER_FAX_NUMBER, lCnt))


            v_vSchemeType = If(CDbl(v_vSchemeType) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeType, r_vSchemes(SCHEME_TYPE, lCnt))


            v_vSchemeVariant = If(CDbl(v_vSchemeVariant) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeVariant, r_vSchemes(SCHEME_VARIANT, lCnt))


            v_vDictVer = If(CStr(v_vDictVer) <> PARAMETER_NOT_PRESENT_STR, v_vDictVer, r_vSchemes(SCHEME_DICT_VER, lCnt))


            v_vClassOfBusiness = If(CStr(v_vClassOfBusiness) <> PARAMETER_NOT_PRESENT_STR, v_vClassOfBusiness, r_vSchemes(SCHEME_CLASS_OF_BUSINESS, lCnt))


            v_vCountryId = If(CDbl(v_vCountryId) <> PARAMETER_NOT_PRESENT_NO, v_vCountryId, r_vSchemes(COUNTRY_ID, lCnt))

            ' add add parameters needed for stored procedure
            ' Scheme ID is also an input parameter.






            If SetupSchemeParameters(gPMConstants.PMEParameterDirection.PMParamInput, v_lSchemeID, v_lQuoteEngineID, v_lBusinessType, CInt(v_lInsurerID), CInt(v_lSchemeNo), CInt(v_iSchemeVer), CInt(v_iSchemeStatus), CDate(v_dtStartDate), CStr(v_sSchemeDesc), v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vQuoteDayNum, v_vSelectionDayNum, v_vInviteDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMaxChangeNum, v_vMinChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant, v_vDictVer, v_vClassOfBusiness, v_vCountryId) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' UpdateDetail scheme record.
            If m_oDatabase.SQLAction(SQL_SCHEME_UPD_V2, SQL_NAME_SCHEME_UPD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDetail_V2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDetail_V2", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    'sj 24/05/2001 - end

    'TF081001 - Built from UpdateDetail_V2 for Renewal options
    Public Function UpdateDetail_V3(ByVal v_lSchemeID As Integer, Optional ByVal v_lQuoteEngineID As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lBusinessType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lInsurerID As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lSchemeNo As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_iSchemeVer As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_iSchemeStatus As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_dtStartDate As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_sSchemeDesc As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vPriority As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vAgencyCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vProductCode As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vActivationLevel As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPrintingPrivileges As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vBrokerGroup As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vCommisionPerc As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vPreSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSelectionDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQuoteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vInviteDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vReminderDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vConfirmDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vLapseDayNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMinChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vMaxChangeNum As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vExpiryDate As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vQMInsurerRef As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeTypeFlags As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vFilename As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vEdiMailBox As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferEmailAddress As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vReferFaxNumber As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vSchemeType As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vSchemeVariant As Object = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_vDictVer As Object = PARAMETER_NOT_PRESENT_STR, Optional ByVal v_vClassOfBusiness As Object = PARAMETER_NOT_PRESENT_STR, Optional ByRef v_vCountryId As Object = PARAMETER_NOT_PRESENT_NO) As Integer

        Dim result As Integer = 0
        Dim vParameters As Object = Nothing
        Dim r_vSchemes(,) As Object = Nothing
        Dim lCnt As Integer

        ' Updates a GIS_scheme record within the database as keyed by Scheme ID.
        ' Fields that are Nullable within the database are optional to this
        ' procedure and will supply null to the procedure if not supplied.

        Try

            ' set parameters for a business type and an insurer.
            ReDim vParameters(1, 0)

            vParameters(0, 0) = GISSB_PARAM_NAME_GIS_SCHEME_ID

            vParameters(1, 0) = v_lSchemeID



            If GetList(bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_SINGLE_SCHEME_FULL_COL_LIST, r_vSchemes, vParameters) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            lCnt = r_vSchemes.GetUpperBound(1)



            v_lQuoteEngineID = If(CDbl(v_lQuoteEngineID) <> PARAMETER_NOT_PRESENT_NO, v_lQuoteEngineID, r_vSchemes(GIS_QUOTE_ENGINE_ID, lCnt))


            v_lBusinessType = If(CDbl(v_lBusinessType) <> PARAMETER_NOT_PRESENT_NO, v_lBusinessType, r_vSchemes(GIS_BUSINESS_TYPE_ID, lCnt))


            v_lInsurerID = If(CDbl(v_lInsurerID) <> PARAMETER_NOT_PRESENT_NO, v_lInsurerID, r_vSchemes(GIS_INSURER_ID, lCnt))


            v_lSchemeNo = If(CDbl(v_lSchemeNo) <> PARAMETER_NOT_PRESENT_NO, v_lSchemeNo, r_vSchemes(SCHEME_NO, lCnt))


            v_iSchemeVer = If(CDbl(v_iSchemeVer) <> PARAMETER_NOT_PRESENT_NO, v_iSchemeVer, r_vSchemes(SCHEME_VER, lCnt))


            v_iSchemeStatus = If(CDbl(v_iSchemeStatus) <> PARAMETER_NOT_PRESENT_NO, v_iSchemeStatus, r_vSchemes(SCHEME_STATUS, lCnt))


            v_dtStartDate = If(CStr(v_dtStartDate) <> PARAMETER_NOT_PRESENT_STR, v_dtStartDate, r_vSchemes(START_DATE, lCnt))


            v_sSchemeDesc = If(CStr(v_sSchemeDesc) <> PARAMETER_NOT_PRESENT_STR, v_sSchemeDesc, r_vSchemes(SCHEME_DESC, lCnt))


            v_vPriority = If(CDbl(v_vPriority) <> PARAMETER_NOT_PRESENT_NO, v_vPriority, r_vSchemes(PRIORITY, lCnt))


            v_vAgencyCode = If(CStr(v_vAgencyCode) <> PARAMETER_NOT_PRESENT_STR, v_vAgencyCode, r_vSchemes(AGENCY_CODE, lCnt))


            v_vProductCode = If(CStr(v_vProductCode) <> PARAMETER_NOT_PRESENT_STR, v_vProductCode, r_vSchemes(PRODUCT_CODE, lCnt))


            v_vActivationLevel = If(CDbl(v_vActivationLevel) <> PARAMETER_NOT_PRESENT_NO, v_vActivationLevel, r_vSchemes(ACTIVATION_LEVEL, lCnt))


            v_vPrintingPrivileges = If(CDbl(v_vPrintingPrivileges) <> PARAMETER_NOT_PRESENT_NO, v_vPrintingPrivileges, r_vSchemes(PRINTING_PRIVILEGES, lCnt))


            v_vBrokerGroup = If(CStr(v_vBrokerGroup) <> PARAMETER_NOT_PRESENT_STR, v_vBrokerGroup, r_vSchemes(BROKER_GROUP, lCnt))


            v_vCommisionPerc = If(CDbl(v_vCommisionPerc) <> PARAMETER_NOT_PRESENT_NO, v_vCommisionPerc, r_vSchemes(COMMISION_PERC, lCnt))


            v_vPreSelectionDayNum = If(CDbl(v_vPreSelectionDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vPreSelectionDayNum, r_vSchemes(PRE_SELECTION_DAY_NUM, lCnt))


            v_vSelectionDayNum = If(CDbl(v_vSelectionDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vSelectionDayNum, r_vSchemes(SELECTION_DAY_NUM, lCnt))


            v_vQuoteDayNum = If(CDbl(v_vQuoteDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vQuoteDayNum, r_vSchemes(QUOTE_DAY_NUM, lCnt))


            v_vInviteDayNum = If(CDbl(v_vInviteDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vInviteDayNum, r_vSchemes(INVITE_DAY_NUM, lCnt))


            v_vReminderDayNum = If(CDbl(v_vReminderDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vReminderDayNum, r_vSchemes(REMINDER_DAY_NUM, lCnt))


            v_vConfirmDayNum = If(CDbl(v_vConfirmDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vConfirmDayNum, r_vSchemes(CONFIRM_DAY_NUM, lCnt))


            v_vLapseDayNum = If(CDbl(v_vLapseDayNum) <> PARAMETER_NOT_PRESENT_NO, v_vLapseDayNum, r_vSchemes(LAPSE_DAY_NUM, lCnt))


            v_vMinChangeNum = If(CDbl(v_vMinChangeNum) <> PARAMETER_NOT_PRESENT_NO, v_vMinChangeNum, r_vSchemes(MIN_CHANGE_NUM, lCnt))


            v_vMaxChangeNum = If(CDbl(v_vMaxChangeNum) <> PARAMETER_NOT_PRESENT_NO, v_vMaxChangeNum, r_vSchemes(MAX_CHANGE_NUM, lCnt))


            v_vExpiryDate = If(CDbl(v_vExpiryDate) <> PARAMETER_NOT_PRESENT_NO, v_vExpiryDate, r_vSchemes(EXPIRY_DATE, lCnt))


            v_vQMInsurerRef = If(CStr(v_vQMInsurerRef) <> PARAMETER_NOT_PRESENT_STR, v_vQMInsurerRef, r_vSchemes(QM_INSURER_REF, lCnt))


            v_vSchemeTypeFlags = If(CDbl(v_vSchemeTypeFlags) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeTypeFlags, r_vSchemes(SCHEME_TYPE_FLAGS, lCnt))


            v_vFilename = If(CStr(v_vFilename) <> PARAMETER_NOT_PRESENT_STR, v_vFilename, r_vSchemes(FILENAME, lCnt))


            v_vEdiMailBox = If(CStr(v_vEdiMailBox) <> PARAMETER_NOT_PRESENT_STR, v_vEdiMailBox, r_vSchemes(EDI_MAIL_BOX, lCnt))


            v_vReferEmailAddress = If(CStr(v_vReferEmailAddress) <> PARAMETER_NOT_PRESENT_STR, v_vReferEmailAddress, r_vSchemes(REFER_EMAIL_ADDRESS, lCnt))


            v_vReferFaxNumber = If(CStr(v_vReferFaxNumber) <> PARAMETER_NOT_PRESENT_STR, v_vReferFaxNumber, r_vSchemes(REFER_FAX_NUMBER, lCnt))


            v_vSchemeType = If(CDbl(v_vSchemeType) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeType, r_vSchemes(SCHEME_TYPE, lCnt))


            v_vSchemeVariant = If(CDbl(v_vSchemeVariant) <> PARAMETER_NOT_PRESENT_NO, v_vSchemeVariant, r_vSchemes(SCHEME_VARIANT, lCnt))


            v_vDictVer = If(CStr(v_vDictVer) <> PARAMETER_NOT_PRESENT_STR, v_vDictVer, r_vSchemes(SCHEME_DICT_VER, lCnt))


            v_vClassOfBusiness = If(CStr(v_vClassOfBusiness) <> PARAMETER_NOT_PRESENT_STR, v_vClassOfBusiness, r_vSchemes(SCHEME_CLASS_OF_BUSINESS, lCnt))


            v_vCountryId = If(CDbl(v_vCountryId) <> PARAMETER_NOT_PRESENT_NO, v_vCountryId, r_vSchemes(COUNTRY_ID, lCnt))

            ' add add parameters needed for stored procedure
            ' Scheme ID is also an input parameter.






            If SetupSchemeParameters_V3(gPMConstants.PMEParameterDirection.PMParamInput, v_lSchemeID, v_lQuoteEngineID, v_lBusinessType, CInt(v_lInsurerID), CInt(v_lSchemeNo), CInt(v_iSchemeVer), CInt(v_iSchemeStatus), CDate(v_dtStartDate), CStr(v_sSchemeDesc), v_vPriority, v_vAgencyCode, v_vProductCode, v_vActivationLevel, v_vPrintingPrivileges, v_vBrokerGroup, v_vCommisionPerc, v_vPreSelectionDayNum, v_vSelectionDayNum, v_vQuoteDayNum, v_vInviteDayNum, v_vReminderDayNum, v_vConfirmDayNum, v_vLapseDayNum, v_vMinChangeNum, v_vMaxChangeNum, v_vExpiryDate, v_vQMInsurerRef, v_vSchemeTypeFlags, v_vFilename, v_vEdiMailBox, v_vReferEmailAddress, v_vReferFaxNumber, v_vSchemeType, v_vSchemeVariant, v_vDictVer, v_vClassOfBusiness, v_vCountryId) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' UpdateDetail scheme record.
            If m_oDatabase.SQLAction(SQL_SCHEME_UPD_V3, SQL_NAME_SCHEME_UPD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDetail_V3 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDetail_V3", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    'Public Function Add(ByRef r_lSchemeID As Long, ByVal v_lQuoteEngineID As Long, _
    ''                    ByVal v_lBusinessType As Long, ByVal v_lInsurerID As Long, _
    ''                    ByVal v_lSchemeNo As Long, ByVal v_iSchemeVer As Integer, _
    ''                    ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, _
    ''                    ByVal v_sSchemeDesc As String, _
    ''                    Optional ByVal v_vPriority As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vAgencyCode As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                    Optional ByVal v_vProductCode As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                    Optional ByVal v_vActivationLevel As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vPrintingPrivileges As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vBrokerGroup As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                    Optional ByVal v_vCommisionPerc As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vQuoteDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vSelectionDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vInviteDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vConfirmDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vLapseDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vMaxChangeNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vMinChangeNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vExpiryDate As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                    Optional ByVal v_vQMInsurerRef As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                    Optional ByVal v_vSchemeTypeFlags As Variant = PARAMETER_NOT_PRESENT_NO) As Long
    '
    '' Adds a scheme to the database.
    '' Fields that are nullable optional and will store a Null if not supplied.
    '
    '    On Error GoTo Err_Add
    '
    '    ' Add parameters needed for stored procedure.
    '    ' Scheme id is an output parameter that will be return by the
    '    ' stored procedure.
    '    If SetupSchemeParameters(PMParamOutput, r_lSchemeID, v_lQuoteEngineID, _
    ''                          v_lBusinessType, v_lInsurerID, _
    ''                          v_lSchemeNo, v_iSchemeVer, _
    ''                          v_iSchemeStatus, v_dtStartDate, _
    ''                          v_sSchemeDesc, v_vPriority, _
    ''                          v_vAgencyCode, v_vProductCode, _
    ''                          v_vActivationLevel, v_vPrintingPrivileges, _
    ''                          v_vBrokerGroup, v_vCommisionPerc, _
    ''                          v_vQuoteDayNum, v_vSelectionDayNum, _
    ''                          v_vInviteDayNum, v_vConfirmDayNum, _
    ''                          v_vLapseDayNum, v_vMaxChangeNum, _
    ''                          v_vMinChangeNum, v_vExpiryDate, _
    ''                          v_vQMInsurerRef, v_vSchemeTypeFlags) <> PMTrue Then
    '        Add = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' Add scheme to database.
    '    If m_oDatabase.SQLAction(SQL_SCHEME_ADD, _
    ''                             SQL_NAME_SCHEME_ADD, _
    ''                             True) <> PMTrue Then
    '        Add = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' return scheme id.
    '    r_lSchemeID = m_oDatabase.Parameters.Item(GISSB_PARAM_NAME_GIS_SCHEME_ID).Value
    '
    '    Add = PMTrue
    '
    '    Exit Function
    '
    'Err_Add:
    '
    '    Add = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Add Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Add", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    'End Function
    '
    'Public Function Update(ByVal v_lSchemeID As Long, ByVal v_lQuoteEngineID As Long, _
    ''                       ByVal v_lBusinessType As Long, ByVal v_lInsurerID As Long, _
    ''                       ByVal v_lSchemeNo As Long, ByVal v_iSchemeVer As Integer, _
    ''                       ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, _
    ''                       ByVal v_sSchemeDesc As String, _
    ''                       Optional ByVal v_vPriority As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vAgencyCode As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                       Optional ByVal v_vProductCode As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                       Optional ByVal v_vActivationLevel As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vPrintingPrivileges As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vBrokerGroup As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                       Optional ByVal v_vCommisionPerc As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vQuoteDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vSelectionDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vInviteDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vConfirmDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vLapseDayNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vMaxChangeNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vMinChangeNum As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vExpiryDate As Variant = PARAMETER_NOT_PRESENT_NO, _
    ''                       Optional ByVal v_vQMInsurerRef As Variant = PARAMETER_NOT_PRESENT_STR, _
    ''                       Optional ByVal v_vSchemeTypeFlags As Variant = PARAMETER_NOT_PRESENT_NO) As Long
    '
    '' Updates a GIS_scheme record within the database as keyed by Scheme ID.
    '' Fields that are Nullable within the database are optional to this
    '' procedure and will supply null to the procedure if not supplied.
    '
    '    On Error GoTo Err_Update
    '
    '
    '    ' add add parameters needed for stored procedure
    '    ' Scheme ID is also an input parameter.
    '    If SetupSchemeParameters(PMParamInput, v_lSchemeID, v_lQuoteEngineID, _
    ''                          v_lBusinessType, v_lInsurerID, _
    ''                          v_lSchemeNo, v_iSchemeVer, _
    ''                          v_iSchemeStatus, v_dtStartDate, _
    ''                          v_sSchemeDesc, v_vPriority, _
    ''                          v_vAgencyCode, v_vProductCode, _
    ''                          v_vActivationLevel, v_vPrintingPrivileges, _
    ''                          v_vBrokerGroup, v_vCommisionPerc, _
    ''                          v_vQuoteDayNum, v_vSelectionDayNum, _
    ''                          v_vInviteDayNum, v_vConfirmDayNum, _
    ''                          v_vLapseDayNum, v_vMaxChangeNum, _
    ''                          v_vMinChangeNum, v_vExpiryDate, _
    ''                          v_vQMInsurerRef, v_vSchemeTypeFlags) <> PMTrue Then
    '        Update = PMFalse
    '        Exit Function
    '    End If
    '
    '    ' update scheme record.
    '    If m_oDatabase.SQLAction(SQL_SCHEME_UPD, _
    ''                             SQL_NAME_SCHEME_UPD, _
    ''                             True) <> PMTrue Then
    '        Update = PMFalse
    '        Exit Function
    '    End If
    '
    '    Update = PMTrue
    '
    '    Exit Function
    '
    'Err_Update:
    '
    '    Update = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Update Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Update", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    'End Function

    Public Function Delete(ByVal v_lSchemeID As Integer) As Integer

        ' Delete GIS_Scheme record as keyed as GIS_Scheme_ID

        Dim result As Integer = 0
        Try

            With m_oDatabase

                .Parameters.Clear()

                ' add scheme id parameter
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lSchemeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' delete GIS_Scheme record.
                If .SQLAction(SQL_SCHEME_DEL, SQL_NAME_SCHEME_DEL, True) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'developer guide no. 17
    Public Function GetList(ByVal v_lListType As Integer, ByRef r_vSchemesArray(,) As Object, Optional ByVal v_vParameterValues As Object = Nothing) As Integer

        ' Gets various lists of schemes
        ' The list returned, and parameters required, depends on the value of v_lListType.
        '
        ' GSL_FULL_ACTIVE_OF_TYPE
        ' v_lListType = 1  - Returns all fields for all active schemes for a specific
        '                    business type.
        '                    Parameters: gis_business_type.
        '
        ' GSL_SINGLE_SCHEME
        ' v_lListType = 2  - Returns all fields for a single scheme.
        '                    Parameters: gis_scheme_id.
        '
        ' GSL_ID_NAME_OF_TYPE
        ' v_lListType = 3  - Returns id and name fields only for active schemes for a
        '                    specific business type.
        '                    Parameters: gis_business_id.
        '
        ' GSL_ID_NAME_OF_TYPE_FOR_INSURER
        ' v_lListType = 4  - Returns id and name fields only for active schemes for a
        '                    specific business type and insurer no.
        '                    Parameters: gis_business_id
        '                                gis_insurer_id
        '
        ' GSL_AGENCY
        ' v_lListType = 5  - Returns id and name fields only for active schemes for a
        '                    specific business type where agency_code is not null.
        '                    Parameters: gis_business_id
        '
        ' GSL_ID_NAME_OF_TYPE_FOR_GROUP
        ' v_lListType = 6  - Returns id and name fields only for active schemes for a
        '                    specific business type and scheme group id.
        '                    Parameters: gis_business_id
        '                                gis_scheme_group_id
        '
        ' GSL_ALL_LEGACY_SCHEMES
        ' v_lListType = 7  - Returns all fields of GIS_Scheme where qm_insurer_ref is not null
        '                  - and for a given business type (sj 9/6/2000)
        ' GSL_ID_NAME_WITH_INSURER
        ' v_lListType = 8  - Returns id and name fields and insurer details for all schemes
        '
        ' GSL_FULL_ACTIVE_OF_TYPE_WITH_INSURER_NAME
        ' v_lListType = 9  - Returns all fields for all active schemes for a specific
        '                    business type.
        '                    Parameters: gis_business_type.
        '
        ' GSL_SINGLE_SCHEME_FULL_COL_LIST
        ' v_lListType = 10  - Returns all fields for a single scheme.
        '                     Parameters: gis_scheme_id.
        '
        ' v_vParameterValues must be a two dimensional array.  The first dimension
        ' being parameter name and parameter value.  The second dimension being the
        ' number of parameters.  Any parameters not supplied will raise an error
        ' in PMMessage.
        '
        ' The function will return an array containing the resulting list.

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL, sSQLName As String

        Try

            ' Search ParameterValues array for required parameters
            If ExtractParameterValues(v_vParameterValues) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            With m_oDatabase

                .Parameters.Clear()

                ' Adds parameters and set which procedure to call.

                Select Case v_lListType
                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_FULL_ACTIVE_OF_TYPE
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        sSQL = SQL_SCHEME_ALL_OF_TYPE_SEL
                        sSQLName = SQL_NAME_SCHEME_ALL_OF_TYPE_SEL


                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_FULL_ACTIVE_OF_TYPE_WITH_INSURER_NAME
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        sSQL = SQL_SCHEME_ALL_OF_TYPE_WITH_INSURER_NAME_SEL
                        sSQLName = SQL_NAME_SCHEME_ALL_OF_TYPE_SEL


                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_FULL_ACTIVE_OF_CLASS_WITH_INSURER_NAME
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CLASS_OF_BUSINESS, m_sClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_COUNTRY_ID, m_lCountryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        sSQL = SQL_SCHEME_ALL_OF_CLASS_WITH_INSURER_NAME_SEL
                        sSQLName = SQL_NAME_SCHEME_ALL_OF_CLASS_SEL

                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_SINGLE_SCHEME
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_ID, m_lSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        sSQL = SQL_SCHEME_SINGLE_SEL
                        sSQLName = SQL_NAME_SCHEME_SINGLE_SEL


                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_SINGLE_SCHEME_FULL_COL_LIST
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_ID, m_lSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'sj 24/05/2001 - start
                        'These two are the same
                        'sSQL = SQL_SCHEME_FULL_COL_LIST
                        sSQL = SQL_SCHEME_SINGLE_SEL
                        'sj 24/05/2001 - end
                        sSQLName = SQL_NAME_SCHEME_FULL_COL_LIST

                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_ID_NAME_OF_TYPE
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        sSQL = SQL_SCHEME_NAME_ONLY_TYPE_SEL
                        sSQLName = SQL_NAME_SCHEME_NAME_ONLY_TYPE_SEL

                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_ID_NAME_OF_TYPE_FOR_INSURER
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_INSURER_ID, m_lInsurerID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_SCHEME_NAME_ONLY_TYPE_INS_SEL
                        sSQLName = SQL_NAME_SCHEME_NAME_ONLY_TYPE_INS_SEL

                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_AGENCY
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        sSQL = SQL_SCHEME_AGENCY_SEL
                        sSQLName = SQL_NAME_SCHEME_AGENCY_SEL

                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_ID_NAME_OF_TYPE_FOR_GROUP
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, m_lSchemeGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_SCHEME_NAME_GROUP_SCHEMES_SEL
                        sSQLName = SQL_NAME_SCHEME_NAME_GROUP_SCHEMES_SEL


                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_ALL_LEGACY_SCHEMES

                        'sj 09/6/2000 - start
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'sj 09/6/2000 - end

                        sSQL = SQL_SCHEME_ALL_LEGACY_SCHEMES_SEL
                        sSQLName = SQL_NAME_SCHEME_ALL_LEGACY_SCHEMES_SEL

                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_ID_NAME_WITH_INSURER
                        sSQL = SQL_SCHEME_NAME_WITH_INS_SEL
                        sSQLName = SQL_NAME_SCHEME_NAME_WITH_INS_SEL

                        'sj 26/7/2000 - start
                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_FULL_LINK_BRANCH_SCHEME

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, m_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SOURCE_ID, m_lSourceId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_FULL_LINK_BRANCH_SCHEME_SEL
                        sSQLName = SQL_NAME_FULL_LINK_BRANCH_SCHEME_SEL

                        'sj 26/7/2000 - end
                        ' TB 14/05/2001
                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_LEGACY_SCHEMES_BY_CLASS

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CLASS_OF_BUSINESS, m_sClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_SCHEME_LEGACY_SCHEMES_BY_CLASS
                        sSQLName = SQL_NAME_LEGACY_SCHEMES_BY_CLASS

                        ' TB 23/5/01: Used in bGIISchemeSetup
                    Case bGISSchemeBusinessConst.GISSB_GET_SCHEME_LISTS.GSL_FULL_LINK_BRANCH_CLASS

                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CLASS_OF_BUSINESS, m_sClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SOURCE_ID, m_lSourceId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        sSQL = SQL_FULL_LINK_BRANCH_CLASS
                        sSQLName = SQL_NAME_FULL_LINK_BRANCH_CLASS

                    Case Else
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="List type unknown!", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList")

                        Return result

                End Select

                ' Run procedure
                lReturn = .SQLSelect(sSQL, sSQLName, True, , r_vSchemesArray, , , , True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    Private Function ExtractParameterValues(ByVal v_vParameterValues(,) As Object) As Integer

        ' Extract parameter values after checking that the supplied parameter is
        ' an array of the correct size.

        Dim result As Integer = 0



        ' Initialize ALL parameters to not present.
        m_lBusinessType = PARAMETER_NOT_PRESENT_NO
        m_lSchemeID = PARAMETER_NOT_PRESENT_NO
        m_lInsurerID = PARAMETER_NOT_PRESENT_NO
        m_lSchemeGroupID = PARAMETER_NOT_PRESENT_NO
        m_lSourceId = PARAMETER_NOT_PRESENT_NO
        m_sClassOfBusiness = PARAMETER_NOT_PRESENT_STR

        If False Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        ' Check that parameter is two dimensional array.
        If CheckArrayDimension(v_vParameterValues, 2) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check first dimension of parameter has an upper bound of 1.
        If v_vParameterValues.GetUpperBound(0) <> 1 Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="First dimension of ParameterValues parameter must be a value of 1", vApp:=ACApp, vClass:=ACClass, vMethod:="ExtractParameterValues")
            Return result
        End If


        ' Extract parameter values
        For iCnt As Integer = v_vParameterValues.GetLowerBound(1) To v_vParameterValues.GetUpperBound(1)


            Select Case CStr(v_vParameterValues(PARAMETER_POS_NAME, iCnt)).ToUpper()
                Case GISSB_PARAM_NAME_GIS_BUSINESS_TYPE.ToUpper()

                    m_lBusinessType = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case GISSB_PARAM_NAME_GIS_SCHEME_ID.ToUpper()

                    m_lSchemeID = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID.ToUpper()

                    m_lSchemeGroupID = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case GISSB_PARAM_NAME_INSURER_ID.ToUpper()

                    m_lInsurerID = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                Case GISSB_PARAM_NAME_SOURCE_ID.ToUpper()

                    m_lSourceId = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                        ' TB 14/5/2001
                Case GISSB_PARAM_NAME_CLASS_OF_BUSINESS.ToUpper()

                    m_sClassOfBusiness = CStr(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))

                        ' TB 19/6/01
                Case GISSB_PARAM_NAME_COUNTRY_ID.ToUpper()

                    m_lCountryId = CInt(v_vParameterValues(PARAMETER_POS_VALUE, iCnt))
                Case Else
                    ' Ignore any parameter that were not expected

            End Select

        Next


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    Private Function SetupSchemeParameters(ByVal v_lSchemeIDDirection As gPMConstants.PMEParameterDirection, ByVal v_lSchemeID As Integer, ByVal v_lQuoteEngineID As Object, ByVal v_lBusinessType As Object, ByVal v_lInsurerID As Integer, ByVal v_lSchemeNo As Integer, ByVal v_iSchemeVer As Integer, ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, ByVal v_sSchemeDesc As String, ByVal v_vPriority As Object, ByVal v_vAgencyCode As Object, ByVal v_vProductCode As Object, ByVal v_vActivationLevel As Object, ByVal v_vPrintingPrivileges As Object, ByVal v_vBrokerGroup As Object, ByVal v_vCommisionPerc As Object, ByVal v_vQuoteDayNum As Object, ByVal v_vSelectionDayNum As Object, ByVal v_vInviteDayNum As Object, ByVal v_vConfirmDayNum As Object, ByVal v_vLapseDayNum As Object, ByVal v_vMaxChangeNum As Object, ByVal v_vMinChangeNum As Object, ByVal v_vExpiryDate As Object, ByVal v_vQMInsurerRef As Object, ByVal v_vSchemeTypeFlags As Object, ByVal v_vFilename As Object, ByVal v_vEdiMailBox As Object, ByVal v_vReferEmailAddress As Object, ByVal v_vReferFaxNumber As Object, ByVal v_vSchemeType As Object, ByVal v_vSchemeVariant As Object, Optional ByVal v_vDictVer As Object = Nothing, Optional ByVal v_vClassOfBusiness As Object = Nothing, Optional ByVal v_vCountryId As Object = Nothing) As Integer

        ' Adds all parameters needed for adding and updating schemes.

        Dim result As Integer = 0


        m_oDatabase.Parameters.Clear()

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_ID, v_lSchemeID, v_lSchemeIDDirection, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'sj 24/05/2001 - start

        If CDbl(v_lQuoteEngineID) <> 0 Then
            If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_QUOTE_ENGINE, v_lQuoteEngineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'sj 24/05/2001 - end

        'sj 24/05/2001 - Make v_lBusinessType & v_lQuoteEngineID variants as
        'they can now be null
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, v_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_INSURER_ID, v_lInsurerID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_NO, v_lSchemeNo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_VER, v_iSchemeVer, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_STATUS, v_iSchemeStatus, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_START_DATE, v_dtStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_DESC, v_sSchemeDesc, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRIORITY, v_vPriority, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_AGENCY_CODE, v_vAgencyCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRODUCT_CODE, v_vProductCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If CStr(v_vActivationLevel).Trim() = "" Then


            v_vActivationLevel = DBNull.Value
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_ACTIVATION_LEVEL, v_vActivationLevel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRINTING_PRIVILEGES, v_vPrintingPrivileges, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_BROKER_GROUP, v_vBrokerGroup, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'sj 24/05/2001 - Change from PMDecimal to PMCurrency for ADO
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_COMMISION_PERC, v_vCommisionPerc, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_QUOTE_DAY_NUM, v_vQuoteDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SELECT_DAY_NUM, v_vSelectionDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_INVITE_DAY_NUM, v_vInviteDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CONFIRM_DAY_NUM, v_vConfirmDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_LAPSE_DAY_NUM, v_vLapseDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_MAX_CHANGE_NUM, v_vMaxChangeNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_MIN_CHANGE_NUM, v_vMinChangeNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_EXPIRY_DATE, v_vExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_QM_INSURER_REF, v_vQMInsurerRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_TYPE_FLAGS, v_vSchemeTypeFlags, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_FILENAME, v_vFilename, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_EDI_MAIL_BOX, v_vEdiMailBox, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_REFER_EMAIL_ADDRESS, v_vReferEmailAddress, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_REFER_FAX_NUMBER, v_vReferFaxNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_TYPE, v_vSchemeType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_VARIANT, v_vSchemeVariant, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TB 13/06/01

        If Informations.IsNothing(v_vDictVer) Then


            v_vDictVer = DBNull.Value
        End If
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_DICT_VER, v_vDictVer, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TB 13/06/01

        If Informations.IsNothing(v_vClassOfBusiness) Then


            v_vClassOfBusiness = DBNull.Value
        End If
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CLASS_OF_BUSINESS, v_vClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TB 13/06/01

        If Informations.IsNothing(v_vCountryId) Then


            v_vCountryId = DBNull.Value
        End If

        'sj 24/05/2001 - start
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_COUNTRY_ID, v_vCountryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'sj 24/05/2001 - end


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function
    'TF081001 - Created from SetupSchemeParameters_V3 to include renewal parameters
    Private Function SetupSchemeParameters_V3(ByVal v_lSchemeIDDirection As gPMConstants.PMEParameterDirection, ByVal v_lSchemeID As Integer, ByVal v_lQuoteEngineID As Object, ByVal v_lBusinessType As Object, ByVal v_lInsurerID As Integer, ByVal v_lSchemeNo As Integer, ByVal v_iSchemeVer As Integer, ByVal v_iSchemeStatus As Integer, ByVal v_dtStartDate As Date, ByVal v_sSchemeDesc As String, ByVal v_vPriority As Object, ByVal v_vAgencyCode As Object, ByVal v_vProductCode As Object, ByVal v_vActivationLevel As Object, ByVal v_vPrintingPrivileges As Object, ByVal v_vBrokerGroup As Object, ByVal v_vCommisionPerc As Object, ByVal v_vPreSelectionDayNum As Object, ByVal v_vSelectionDayNum As Object, ByVal v_vQuoteDayNum As Object, ByVal v_vInviteDayNum As Object, ByVal v_vReminderDayNum As Object, ByVal v_vConfirmDayNum As Object, ByVal v_vLapseDayNum As Object, ByVal v_vMinChangeNum As Object, ByVal v_vMaxChangeNum As Object, ByVal v_vExpiryDate As Object, ByVal v_vQMInsurerRef As Object, ByVal v_vSchemeTypeFlags As Object, ByVal v_vFilename As Object, ByVal v_vEdiMailBox As Object, ByVal v_vReferEmailAddress As Object, ByVal v_vReferFaxNumber As Object, ByVal v_vSchemeType As Object, ByVal v_vSchemeVariant As Object, Optional ByVal v_vDictVer As Object = Nothing, Optional ByVal v_vClassOfBusiness As Object = Nothing, Optional ByVal v_vCountryId As Object = Nothing) As Integer

        ' Adds all parameters needed for adding and updating schemes.

        Dim result As Integer = 0


        m_oDatabase.Parameters.Clear()

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_ID, v_lSchemeID, v_lSchemeIDDirection, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'sj 24/05/2001 - start

        If CDbl(v_lQuoteEngineID) <> 0 Then
            If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_QUOTE_ENGINE, v_lQuoteEngineID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'sj 24/05/2001 - end

        'sj 24/05/2001 - Make v_lBusinessType & v_lQuoteEngineID variants as
        'they can now be null
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, v_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_INSURER_ID, v_lInsurerID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_NO, v_lSchemeNo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_VER, v_iSchemeVer, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_STATUS, v_iSchemeStatus, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_START_DATE, v_dtStartDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_DESC, v_sSchemeDesc, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRIORITY, v_vPriority, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_AGENCY_CODE, v_vAgencyCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRODUCT_CODE, v_vProductCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If CStr(v_vActivationLevel).Trim() = "" Then


            v_vActivationLevel = DBNull.Value
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_ACTIVATION_LEVEL, v_vActivationLevel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRINTING_PRIVILEGES, v_vPrintingPrivileges, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_BROKER_GROUP, v_vBrokerGroup, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'sj 24/05/2001 - Change from PMDecimal to PMCurrency for ADO
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_COMMISION_PERC, v_vCommisionPerc, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_PRE_SELECT_DAY_NUM, v_vPreSelectionDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SELECT_DAY_NUM, v_vSelectionDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_QUOTE_DAY_NUM, v_vQuoteDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_INVITE_DAY_NUM, v_vInviteDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_REMINDER_DAY_NUM, v_vReminderDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CONFIRM_DAY_NUM, v_vConfirmDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_LAPSE_DAY_NUM, v_vLapseDayNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_MIN_CHANGE_NUM, v_vMinChangeNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_MAX_CHANGE_NUM, v_vMaxChangeNum, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_EXPIRY_DATE, v_vExpiryDate, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_QM_INSURER_REF, v_vQMInsurerRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_TYPE_FLAGS, v_vSchemeTypeFlags, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_FILENAME, v_vFilename, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_EDI_MAIL_BOX, v_vEdiMailBox, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_REFER_EMAIL_ADDRESS, v_vReferEmailAddress, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_REFER_FAX_NUMBER, v_vReferFaxNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_TYPE, v_vSchemeType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_SCHEME_VARIANT, v_vSchemeVariant, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TB 13/06/01

        If Informations.IsNothing(v_vDictVer) Then


            v_vDictVer = DBNull.Value
        End If
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_DICT_VER, v_vDictVer, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TB 13/06/01

        If Informations.IsNothing(v_vClassOfBusiness) Then


            v_vClassOfBusiness = DBNull.Value
        End If
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_CLASS_OF_BUSINESS, v_vClassOfBusiness, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True, PARAMETER_NOT_PRESENT_STR) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' TB 13/06/01

        If Informations.IsNothing(v_vCountryId) Then


            v_vCountryId = DBNull.Value
        End If

        'sj 24/05/2001 - start
        If AddParameter(m_oDatabase, GISSB_PARAM_NAME_COUNTRY_ID, v_vCountryId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'sj 24/05/2001 - end


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' PRIVATE Methods (End)
End Class

