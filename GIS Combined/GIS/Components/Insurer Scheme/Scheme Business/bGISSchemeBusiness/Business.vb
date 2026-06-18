Option Strict Off
Option Explicit On
Imports System.Text
Imports SSP.Shared
'developer guide no. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date:  15/06/1999
    '
    ' Description: Class to contain any business rules for access GIS
    '              scheme related data.
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

    Private m_sClassOfBusiness As String = ""
    ' ************************************************


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Business"

    ' Constants for SQL call statements for selecting property counts
    'developer guide no 39. 

    Private Const SQL_PROPERTY_COUNT_ALL_TYPE As String = "spu_GIS_Prop_Cnt_All_Type_sel"
    Private Const SQL_PROPERTY_COUNT_SCHEME_GROUP As String = "spu_GIS_Prop_Cnt_Group_sel"
    Private Const SQL_PROPERTY_COUNT_SCHEME As String = "spu_GIS_Prop_Cnt_Scheme_sel"

    ' Constants for building SQL statement to extract navigation profile.
    ' the where clause will be written within the code.

    'sj 02/08/2001 - start
    'Private Const SQL_NAV_PROFILE_PRE_SELECT = "SELECT DISTINCT sp.gis_property_id, o.object_name, p.property_name, MAX(required_pre) from gis_scheme_property sp, gis_property p, gis_object o  "
    Private Const SQL_NAV_PROFILE_PRE_SELECT As String = "SELECT DISTINCT p.gis_property_id, o.object_name, p.property_name, MAX(required_pre) from gis_scheme_property sp, gis_property p, gis_object o  "
    'Private Const SQL_NAV_PROFILE_POST_SELECT = "SELECT DISTINCT sp.gis_property_id, o.object_name, p.property_name, MAX(required_post) from gis_scheme_property sp, gis_property p, gis_object o "
    Private Const SQL_NAV_PROFILE_POST_SELECT As String = "SELECT DISTINCT p.gis_property_id, o.object_name, p.property_name, MAX(required_post) from gis_scheme_property sp, gis_property p, gis_object o, gis_policy_link l "
    Private Const SQL_NAV_PROFILE_WHERE_JOIN As String = "WHERE sp.gis_property_id = p.gis_property_id and sp.gis_object_id = o.gis_object_id "
    Private Const SQL_NAV_PROFILE_WHERE_JOIN_V2 As String = "WHERE sp.property_name = p.property_name " &
                                                            "AND sp.object_name = o.object_name " &
                                                            "AND o.gis_object_id = p.gis_object_id " &
                                                            "AND o.gis_data_model_id = l.gis_data_model_id " &
                                                            "AND l.gis_policy_link_id = "
    'Private Const SQL_NAV_PROFILE_GROUP = "GROUP BY sp.gis_property_id, o.object_name, p.property_name"
    Private Const SQL_NAV_PROFILE_GROUP As String = "GROUP BY p.gis_property_id, o.object_name, p.property_name"
    Private Const SQL_CHECK_NEW_SCHEME_PROPERTY As String = "SELECT * FROM syscolumns WHERE id = object_id('gis_scheme_property') AND name = 'object_name'"
    'sj 02/08/2001 - end

    ' Constants for SQL call statements to add and delete selected schemes.
    'developer guide no 39. 

    Private Const SQL_POLICY_SCHEMES_SEL_DELETE_ALL As String = "spu_GIS_PolicySchemesSelAll_del"
    Private Const SQL_POLICY_SCHEMES_SEL_ADD As String = "spu_GIS_PolicySchemesSel_add"
    Private Const SQL_POLICY_SCHEMES_SEL_SELECT As String = "spu_GIS_Pol_Sch_Sel_Select"

    ' Constants to define names of stored procedure that get property counts.
    Private Const SQL_NAME_PROPERTY_COUNT_ALL_TYPE As String = "PropertyCountAll"
    Private Const SQL_NAME_PROPERTY_COUNT_SCHEME_GROUP As String = "PropertyCountGroup"
    Private Const SQL_NAME_PROPERTY_COUNT_SCHEME As String = "PropertyCountScheme"

    ' A Constant for name of navigation profile statement.
    Private Const SQL_NAME_NAV_PROFILE As String = "NavProfile"

    ' Names of statements to add and delete selected schemes.
    Private Const SQL_NAME_POLICY_SCHEMES_SEL_DELETE_ALL As String = "PolicySchemeDel"
    Private Const SQL_NAME_POLICY_SCHEMES_SEL_ADD As String = "PolicySchemeAdd"
    Private Const SQL_NAME_POLICY_SCHEMES_SEL_SELECT As String = "PolicySchemeSelect"

    Private Const ACGetSchemesStored As Boolean = True ' CL150200
    Private Const ACGetSchemesName As String = "GetSchemes" ' CL150200
    'developer guide no 39. 
    Private Const ACGetSchemesSQL As String = "spu_gis_quote_param_sel" ' RFC300300

    Private Const ACGisSchemeDataSelStored As Boolean = True 'RJG190901
    Private Const ACGisSchemeDataSelName As String = "GisSchemeDataSel" 'RJG190901
    'developer guide no 39.
    Private Const ACGisSchemeDataSelSQL As String = "spu_gis_scheme_data_sel" 'RJG190901

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Contain a reference to a scheme group object.  Will provide
    ' business related methods to GIS_Scheme_Group.
    Private m_oSchemeGroup As bGISSchemeBusiness.SchemeGroup

    'sj 20/07/99 - start
    Private m_oSchemeProperty As SchemeProperty
    Private m_oSchemeCobolLinkage As SchemeCobolLinkage
    'sj 20/7/99 - end

    Private m_oQEMUsage As QEMUsage 'sj 22/7/99
    Private m_oSchemePaymentType As SchemePaymentType

    Private m_oSchemeData As SchemeData 'sj 15/03/01

    ' Contain a reference to a scheme group member object.  Will provide
    ' business related methods to GIS_Scheme_Group_Member.
    Private m_oSchemeGroupMember As bGISSchemeBusiness.SchemeGroupMember

    Private m_oScheme As bGISSchemeBusiness.Scheme

    ' PUBLIC Properties (Begin)

    'sj 02/08/2001 - start
    Public WriteOnly Property ClassOfBusiness() As String
        Set(ByVal Value As String)
            m_sClassOfBusiness = Value
        End Set
    End Property
    'sj 02/08/2001 - end
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            '    PMProductFamily = pmePFGemini
            'JSB 22/11/2000 - Changed product family reference from Gemini to Gemini II
            Return gPMConstants.PMEProductFamily.pmePFGeminiII
        End Get
    End Property

    'sj 20/7/99 - start
    Public ReadOnly Property SchemeProperty() As SchemeProperty
        Get

            ' Provides reference to SchemeProperty object.

            Dim result As SchemeProperty = Nothing
            Try

                If m_oSchemeProperty Is Nothing Then

                    ' if object does not exist then create it.
                    m_oSchemeProperty = New bGISSchemeBusiness.SchemeProperty()

                    ' set database object of object just created to same database
                    ' this object contains.
                    m_oSchemeProperty.Database = m_oDatabase

                    'set the class of business property
                    m_oSchemeProperty.ClassOfBusiness = m_sClassOfBusiness

                End If

                ' return object reference.

                Return m_oSchemeProperty

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SchemeProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SchemeProperty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property
    Public ReadOnly Property SchemeCobolLinkage() As SchemeCobolLinkage
        Get

            ' Provides reference to SchemeCobolLinkage object.

            Dim result As SchemeCobolLinkage = Nothing
            Try

                If m_oSchemeCobolLinkage Is Nothing Then

                    ' if object does not exist then create it.
                    m_oSchemeCobolLinkage = New bGISSchemeBusiness.SchemeCobolLinkage()

                    ' set database object of object just created to same database
                    ' this object contains.
                    m_oSchemeCobolLinkage.Database = m_oDatabase
                End If

                ' return object reference.

                Return m_oSchemeCobolLinkage

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SchemeCobolLinkage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SchemeCobolLinkage", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property
    'sj 20/7/99 - end

    'sj 22/7/99 - start
    Public ReadOnly Property QEMUsage() As QEMUsage
        Get

            ' Provides reference to QEMUsage object.

            Dim result As QEMUsage = Nothing
            Try

                If m_oQEMUsage Is Nothing Then

                    ' if object does not exist then create it.
                    m_oQEMUsage = New bGISSchemeBusiness.QEMUsage()

                    ' set database object of object just created to same database
                    ' this object contains.
                    m_oQEMUsage.Database = m_oDatabase
                End If

                ' return object reference.

                Return m_oQEMUsage

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMUsage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMUsage", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property
    'sj 22/7/99 - start

    Public ReadOnly Property SchemePaymentType() As SchemePaymentType
        Get

            ' Provides reference to SchemePaymentType object.

            Dim result As SchemePaymentType = Nothing
            Try

                If m_oSchemePaymentType Is Nothing Then

                    ' if object does not exist then create it.
                    m_oSchemePaymentType = New bGISSchemeBusiness.SchemePaymentType()

                    ' set database object of object just created to same database
                    ' this object contains.
                    m_oSchemePaymentType.Database = m_oDatabase
                End If

                ' return object reference.

                Return m_oSchemePaymentType

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SchemePaymentType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SchemePaymentType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property

    Public ReadOnly Property SchemeGroup() As SchemeGroup
        Get

            ' Provides reference to SchemeGroup object.

            Dim result As SchemeGroup = Nothing
            Try

                If m_oSchemeGroup Is Nothing Then

                    ' if object does not exist then create it.
                    m_oSchemeGroup = New bGISSchemeBusiness.SchemeGroup()

                    m_oSchemeGroup.SetGlobalData(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' set database object of object just created to same database
                    ' this object contains.
                    m_oSchemeGroup.Database = m_oDatabase
                End If

                ' return object reference.

                Return m_oSchemeGroup

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SchemeGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SchemeGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property


    Public ReadOnly Property SchemeGroupMember() As SchemeGroupMember
        Get

            ' Provides reference to SchemeGroupMember object.

            Dim result As SchemeGroupMember = Nothing
            Try

                If m_oSchemeGroupMember Is Nothing Then

                    ' Create object if it does not exist.
                    m_oSchemeGroupMember = New bGISSchemeBusiness.SchemeGroupMember()

                    ' Set database reference to be this database reference.
                    m_oSchemeGroupMember.Database = m_oDatabase
                End If

                ' return reference to scheme group member object.

                Return m_oSchemeGroupMember

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SchemeGroupMember Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SchemeGroupMember", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property


    Public ReadOnly Property Scheme() As Scheme
        Get

            ' Provides reference to Scheme object.

            Dim result As Scheme = Nothing
            Try

                If m_oScheme Is Nothing Then

                    ' Create scheme object if it does not exist.
                    m_oScheme = New bGISSchemeBusiness.Scheme()

                    ' RDC 22092003
                    m_oScheme.ClassOfBusiness = m_sClassOfBusiness

                    ' Set scheme objects database to this database object.
                    m_oScheme.Database = m_oDatabase
                End If

                ' return scheme object

                Return m_oScheme

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Scheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Scheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property
    'sj 14/03/2001 - start
    Public ReadOnly Property SchemeData() As SchemeData
        Get

            ' Provides reference to SchemeData object.

            Dim result As SchemeData = Nothing
            Try

                If m_oSchemeData Is Nothing Then

                    ' if object does not exist then create it.
                    m_oSchemeData = New bGISSchemeBusiness.SchemeData()

                    ' set database object of object just created to same database
                    ' this object contains.
                    m_oSchemeData.Database = m_oDatabase
                End If

                ' return object reference.

                Return m_oSchemeData

            Catch excep As System.Exception




                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SchemeData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SchemeData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result
            End Try
        End Get
    End Property
    'sj 14/03/2001 - end

    ' PUBLIC Properties (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Dim lReturn As Integer

        Try


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            '    ' Check the Supplied Database.
            '    lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            ''        v_lPMProductFamily:=PMProductFamily, _
            ''        r_bNewInstanceCreated:=m_bCloseDatabase, _
            ''        r_oCheckedDatabase:=m_oDatabase, _
            ''        v_vDatabase:=vDatabase)

            'developer guide no. As per vb code First three parameter passed as hard codeed
            m_oDatabase = bGEMFunc.GetGISDatabase("", 1, 1, lReturn, m_bCloseDatabase, vDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function GetPropertyCount(ByVal v_lPropertyCountType As Integer, ByRef r_iCount As Integer, Optional ByVal v_lBusinessType As Integer = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lSchemeGroupID As Integer = PARAMETER_NOT_PRESENT_NO, Optional ByVal v_lSchemeID As Integer = PARAMETER_NOT_PRESENT_NO) As Integer

        ' Provides a method of returning property counts.
        ' Returns diferent counts depending on value of v_lPropertyCoutType
        '
        ' v_lPropertyCountType = 1, requires BusinessType parameter to return
        '                           count of all schemes of a business type.
        '
        ' v_lPropertyCountType = 2, requires BusinessType and SchemeGroupID
        '                           parameters to return count of schemes of
        '                           a scheme group for a business type.
        '
        ' v_lPropertyCountType = 3, requires SchemeID parameter to return
        '                           count of a scheme.

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""
        Dim sSQLName As String = ""
        Dim vCountArray(,) As Object = Nothing

        Try

            m_oDatabase.Parameters.Clear()

            Select Case v_lPropertyCountType
                Case bGISSchemeBusinessConst.GISSB_GET_PROPERTY_COUNT_TYPE.GPCT_ALL_SCHEMES_OF_TYPE
                    ' Requested for property count of all schemes

                    ' Add business type parameter, raise error if does not exist
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, v_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sSQL = SQL_PROPERTY_COUNT_ALL_TYPE
                    sSQLName = SQL_NAME_PROPERTY_COUNT_ALL_TYPE

                Case bGISSchemeBusinessConst.GISSB_GET_PROPERTY_COUNT_TYPE.GPCT_SCHEME_GROUP_OF_TYPE
                    ' Requested for property count of a scheme group.

                    ' Adds parameter for business type, raises error if not present
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_BUSINESS_TYPE, v_lBusinessType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Adds parameter for scheme group id, raises error if not present
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_GROUP_ID, v_lSchemeGroupID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sSQL = SQL_PROPERTY_COUNT_SCHEME_GROUP
                    sSQLName = SQL_NAME_PROPERTY_COUNT_SCHEME_GROUP


                Case bGISSchemeBusinessConst.GISSB_GET_PROPERTY_COUNT_TYPE.GPCT_SCHEME
                    ' Requested for property count of a scheme.

                    ' Adds parameter for scheme id, raises error if not present
                    If AddParameter(m_oDatabase, GISSB_PARAM_NAME_GIS_SCHEME_ID, v_lSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False, PARAMETER_NOT_PRESENT_NO) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sSQL = SQL_PROPERTY_COUNT_SCHEME
                    sSQLName = SQL_NAME_PROPERTY_COUNT_SCHEME

                Case Else

                    ' Raises error if property count type requested is not
                    ' one of the above types.

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Property count type unknown!", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyCount")

                    Return result

            End Select

            ' Query database for property count.
            lReturn = m_oDatabase.SQLSelect(sSQL, sSQLName, True, , vCountArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that count is returned.
            If Informations.IsArray(vCountArray) Then

                ' return count

                r_iCount = vCountArray.GetUpperBound(1) + 1

                result = gPMConstants.PMEReturnCode.PMTrue

            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyCount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyCount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetNavigationProfile(ByVal v_lPolicyLinkID As Integer, ByVal v_bPostQuoteProfile As Boolean, ByRef r_lSelectedSchemes() As Integer, ByRef r_vProfileArray(,) As Object) As Integer

        ' Gets the navigation profile for a list schemes.
        ' List of schemes is an array of longs containing the scheme ids.
        ' Either the profile of required pre or required post with be returned
        ' depending on the status of v_bPostQuoteProfile.
        ' Policy Link Id is supplied so the list of schemes can be saved to the
        ' database.

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim sSQL As String = ""
        Dim sSelectedSchemes As New StringBuilder
        Dim bNewGisSchemeProperty As Boolean
        Dim sWhereJoin As String = ""

        Try

            ' Build string list of scheme ids delimited by a comma.
            For Each r_lSelectedSchemes_item As Integer In r_lSelectedSchemes
                sSelectedSchemes.Append(CStr(r_lSelectedSchemes_item) & ",")
            Next r_lSelectedSchemes_item

            ' Remove last comma from list of selected schemes
            If sSelectedSchemes.ToString().EndsWith(",") Then
                sSelectedSchemes = New StringBuilder(sSelectedSchemes.ToString().Substring(0, sSelectedSchemes.ToString().Length - 1))
            End If

            'sj 02/08/2001 - start
            ' Are we using the new format gis_scheme_property table
            'sj 24/08/2001 - start
            '    lReturn = CheckNewGISSchemeProperty( _
            ''        r_bNewGisSchemeProperty:=bNewGisSchemeProperty _
            ''        )
            bNewGisSchemeProperty = True
            'sj 24/08/2001 - end
            'sj 02/08/2001 - end

            ' Select which select statement to use depending on whether or not
            ' a post quote profile is required.
            If v_bPostQuoteProfile Then
                sSQL = SQL_NAV_PROFILE_POST_SELECT
            Else
                sSQL = SQL_NAV_PROFILE_PRE_SELECT
            End If

            'sj 02/08/2001 - start
            If Not bNewGisSchemeProperty Then
                sWhereJoin = SQL_NAV_PROFILE_WHERE_JOIN
            Else
                sWhereJoin = SQL_NAV_PROFILE_WHERE_JOIN_V2 & CStr(v_lPolicyLinkID) & " "
            End If
            'sj 02/08/2001 - end

            ' add where clause to statement
            sSQL = sSQL & sWhereJoin &
                   "AND sp.gis_scheme_id IN ( " & sSelectedSchemes.ToString() & " )" &
                   SQL_NAV_PROFILE_GROUP

            ' select profile
            lReturn = m_oDatabase.SQLSelect(sSQL, SQL_NAME_NAV_PROFILE, False, , r_vProfileArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' save selected schemes to database.
            If SaveSelectedSchemes(v_lPolicyLinkID, r_lSelectedSchemes) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNavigationProfile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNavigationProfile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'sj 02/08/2001 - start
    ' ***************************************************************** '
    '
    ' Name: CheckNewGISSchemeProperty
    '
    ' Description:
    '
    ' History: 02/08/2001 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckNewGISSchemeProperty) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckNewGISSchemeProperty(ByRef r_bNewGisSchemeProperty As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim vArray As Object
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'lReturn = m_oDatabase.SQLSelect(SQL_CHECK_NEW_SCHEME_PROPERTY, "CheckSchemeProperty", False,  , vArray)
    '
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'r_bNewGisSchemeProperty = Informations.IsArray(vArray)
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckNewGISSchemeProperty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckNewGISSchemeProperty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    'sj 02/08/2001 - end

    Public Function SaveSelectedSchemes(ByVal v_lPolicyLinkID As Integer, ByRef r_lSelectedSchemes() As Integer) As Integer

        ' Saves selected schemes to the database.

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lLBnd, lUBnd As Integer

        Try

            ' Start transaction
            m_oDatabase.SQLBeginTrans()

            ' Deletes selected schemes previously saved.
            If DeleteAllSelectedSchemes(v_lPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Rollback transaction and exit if delete fails.
                m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = ""

            lLBnd = r_lSelectedSchemes.GetLowerBound(0)
            lUBnd = r_lSelectedSchemes.GetUpperBound(0)

            ' Save every selected scheme.
            For iCnt As Integer = lLBnd To lUBnd

                sSQL = sSQL & "exec spu_GIS_PolicySchemesSel_add " & CStr(v_lPolicyLinkID) & ", " & CStr(r_lSelectedSchemes(iCnt)) & Strings.ChrW(13) & Strings.ChrW(10)

                '        If SaveSelectedScheme(v_lPolicyLinkID, _
                ''                              r_lSelectedSchemes(iCnt)) <> PMTrue Then
                '            ' Rollback transaction and exit if delete fails.
                '            m_oDatabase.SQLRollbackTrans
                '            SaveSelectedSchemes = PMFalse
                '            Exit Function
                '        End If

            Next

            ' run statement to add scheme.
            If m_oDatabase.SQLAction(sSQL, SQL_NAME_POLICY_SCHEMES_SEL_ADD, False) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Commit changes to database.
            m_oDatabase.SQLCommitTrans()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSelectedSchemes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FastSaveSelectedSchemes
    '
    ' Description:
    '
    ' History: 01/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (FastSaveSelectedSchemes) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function FastSaveSelectedSchemes(ByVal v_lPolicyLinkID As Integer, ByRef r_lSelectedSchemes() As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'Dim sSQL As String = ""
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'Const INSERT_GIS_POLICY_SCHEMES_SEL_1 As String = "INSERT INTO gis_policy_schemes_sel VALUES ("
    'Const INSERT_GIS_POLICY_SCHEMES_SEL_2 As String = ", "
    'Const INSERT_GIS_POLICY_SCHEMES_SEL_3 As String = ");" & Strings.ChrW(13) & Strings.ChrW(10)
    '
    ' Start transaction
    'm_oDatabase.SQLBeginTrans()
    '
    ' Deletes selected schemes previously saved.
    'If DeleteAllSelectedSchemes(v_lPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Rollback transaction and exit if delete fails.
    'm_oDatabase.SQLRollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Save every selected scheme.
    'sSQL = "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10) & "SET NOCOUNT ON " & Strings.ChrW(13) & Strings.ChrW(10)
    '
    'For	Each r_lSelectedSchemes_item As Integer In r_lSelectedSchemes
    'sSQL = sSQL & INSERT_GIS_POLICY_SCHEMES_SEL_1 & CStr(v_lPolicyLinkID) & INSERT_GIS_POLICY_SCHEMES_SEL_2 & CStr(r_lSelectedSchemes_item) & INSERT_GIS_POLICY_SCHEMES_SEL_3
    '
    'Next r_lSelectedSchemes_item
    '
    'sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & " SET NOCOUNT OFF " & Strings.ChrW(13) & Strings.ChrW(10) & "END"
    '
    ' Process the SQL Statements
    'lReturn = m_oDatabase.SQLAction(sSQL, "FastSaveSelectedSchemes", False)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    ' Rollback the Transaction
    'lReturn = m_oDatabase.SQLRollbackTrans()
    ' More to do here. If Failed to Rollback log an error
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
    '
    ' Commit changes to database.
    'm_oDatabase.SQLCommitTrans()
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FastSaveSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FastSaveSelectedSchemes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    Public Function SQLBeginTrans() As Integer

        Dim result As Integer = 0
        Try

            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLBeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SQLBeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return result
        End Try
    End Function

    Public Function SQLCommitTrans() As Integer

        Dim result As Integer = 0
        Try

            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLCommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SQLCommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function SQLRollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLRollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SQLRollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)


    Public Function DeleteAllSelectedSchemes(ByVal v_lPolicyLinkID As Integer) As Integer

        ' Deletes previously selected schemes from the database.

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            With m_oDatabase

                .Parameters.Clear()

                ' add parameter for policy link id
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_POLICY_LINK_ID, CStr(v_lPolicyLinkID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' call statement to delete schemes
                lReturn = .SQLAction(SQL_POLICY_SCHEMES_SEL_DELETE_ALL, SQL_NAME_POLICY_SCHEMES_SEL_DELETE_ALL, True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllSelectedSchemes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (SaveSelectedScheme) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SaveSelectedScheme(ByVal v_lPolicyLinkID As Integer, ByVal v_lSchemeID As Integer) As Integer
    '
    ' Saves a single selected scheme with the database.
    '
    'Dim result As Integer = 0
    'Try 
    '
    'With m_oDatabase
    '
    '.Parameters.Clear()
    '
    ' add parameter of policy link id.
    'If .Parameters.Add(GISSB_PARAM_NAME_GIS_POLICY_LINK_ID, CStr(v_lPolicyLinkID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' add parameter of scheme id
    'If .Parameters.Add(GISSB_PARAM_NAME_GIS_SCHEME_ID, CStr(v_lSchemeID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' run statement to add scheme.
    'If .SQLAction(SQL_POLICY_SCHEMES_SEL_ADD, SQL_NAME_POLICY_SCHEMES_SEL_ADD, True) <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'End With
    '
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSelectedScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSelectedScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    ' PRIVATE Methods (End)

    Public Function SelectSelectedScheme(ByVal v_lPolicyLinkID As Integer, ByRef r_lSchemeID As Integer) As Integer

        ' Saves a single selected scheme with the database.
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try


            With m_oDatabase

                .Parameters.Clear()

                ' add parameter of policy link id.
                If .Parameters.Add(GISSB_PARAM_NAME_GIS_POLICY_LINK_ID, CStr(v_lPolicyLinkID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' run statement to add scheme.
                If .SQLSelect(sSQL:=SQL_POLICY_SCHEMES_SEL_SELECT, sSQLName:=SQL_NAME_POLICY_SCHEMES_SEL_SELECT, bStoredProcedure:=True, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then


                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else

                r_lSchemeID = CInt(vResultArray(1, 0))
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSelectedScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSelectedScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    ' PRIVATE Methods (End)


    ' ***************************************************************** '
    ' Name: GetSchemes
    '
    ' Description: Get the List of Schemes to Quote for this
    '              DataModel/Business Type Combination.
    '
    ' RFC15022000 - Make PolicyLinkID Optional, Set to NULL if not supplied
    ' RFC13012000 - Tidy Up Effective Date. Effective Date is not required
    '               if the SchemeID has been supplied.
    ' RFC30032000 - Added Optional QuoteType Param
    ' BSJ23032001 - To avoid breaking compatibility calling GetSchemesPrivate from
    '               this function and also from GetSchemesByRiskGroup
    ' ***************************************************************** '
    Public Function GetSchemes(ByVal v_sGisBusinessTypeCode As String, ByVal v_sGisDataModelCode As String, ByRef r_vSchemesArray(,) As Object, Optional ByVal v_lGisPolicyLinkID As Integer = -1, Optional ByVal v_lGISSchemeId As Integer = -1, Optional ByVal v_dtEffectiveDate As Date = GISLowDate, Optional ByVal v_lQuoteType As Integer = -1, Optional ByVal v_bCalledFromSTS As Boolean = False, Optional ByVal v_sRealTransactionType As String = "") As Integer

        Dim result As Integer = 0

        Try



            ' Exit here
            'developer guide no 98. 
            Return GetSchemesPrivate(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=r_vSchemesArray, v_lGisPolicyLinkID:=v_lGisPolicyLinkID, v_lGISSchemeId:=v_lGISSchemeId, v_dtEffectiveDate:=v_dtEffectiveDate, v_lQuoteType:=v_lQuoteType, v_bCalledFromSTS:=v_bCalledFromSTS, v_sRealTransactionType:=v_sRealTransactionType)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemesFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Author:      BSJ 23/11/01
    ' Name:        GetSchemesByRiskGroup
    '
    ' Description: This is a replica of GetSchemes with the optional
    '              paramter (v_lRiskGroupID) provided.
    '
    ' ***************************************************************** '
    'developer guide no 17. 
    Public Function GetSchemesByRiskGroup(ByVal v_sGisBusinessTypeCode As String, ByVal v_sGisDataModelCode As String, ByRef r_vSchemesArray(,) As Object, Optional ByVal v_lGisPolicyLinkID As Integer = -1, Optional ByVal v_lGISSchemeId As Integer = -1, Optional ByVal v_dtEffectiveDate As Date = GISLowDate, Optional ByVal v_lQuoteType As Integer = -1, Optional ByVal v_lRiskGroupID As Integer = -1, Optional ByVal v_bCalledFromSTS As Boolean = False, Optional ByVal v_sRealTransactionType As String = "") As Integer

        Dim result As Integer = 0

        Try


            ' Exit here
            Return GetSchemesPrivate(v_sGisBusinessTypeCode, v_sGisDataModelCode, r_vSchemesArray, v_lGisPolicyLinkID, v_lGISSchemeId, v_dtEffectiveDate, v_lQuoteType, v_lRiskGroupID, v_bCalledFromSTS, v_sRealTransactionType)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemesByRiskGroupID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemesByRiskGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            'Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Author:      BSJ 23/11/01
    ' Name:        GetSchemesPrivate
    '
    ' Description: This is a replica of the original GetSchemes with added functionality,
    '              it is required so that we don't break compatibility the
    '              only difference is the fact that we now have an optional
    '              paramter (v_lRiskGroupID).  This function is called from
    '              either GetSchemes or GetSchemesByRiskGroup.
    '
    ' ***************************************************************** '
    'developer guide no 17. 
    Private Function GetSchemesPrivate(ByVal v_sGisBusinessTypeCode As String, ByVal v_sGisDataModelCode As String, ByRef r_vSchemesArray(,) As Object, Optional ByVal v_lGisPolicyLinkID As Integer = -1, Optional ByVal v_lGISSchemeId As Integer = -1, Optional ByVal v_dtEffectiveDate As Date = GISLowDate, Optional ByVal v_lQuoteType As Integer = -1, Optional ByVal v_lRiskGroupID As Integer = -1, Optional ByVal v_bCalledFromSTS As Boolean = False, Optional ByVal v_sRealTransactionType As String = "") As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sMsg As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'RFC13012000 - Set Effective Date to current date/time if it has not been supplied
        If v_dtEffectiveDate <= GISLowDate Then

            v_dtEffectiveDate = DateTime.Now
        End If

        r_vSchemesArray = Nothing

        m_oDatabase.Parameters.Clear()

        ' Business Type Code
        lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=v_sGisBusinessTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Effective Date Parameter
        'developer guide no 40. 
        lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Gis Policy Link Parameter
        ' RFC15022000 - Make PolicyLinkID Optional, Set to NULL if not supplied
        If v_lGisPolicyLinkID > 0 Then

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGisPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else


            'developer guide no. 85
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        ' GIS Data Model Code
        lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Add the Gis Scheme ID Parameter
        If v_lGISSchemeId > 0 Then

            ' GIS Scheme ID
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGISSchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            ' GIS Scheme ID

            'developer guide no. 85
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        ' RFC300300 Add the Gis Scheme ID Parameter
        If v_lQuoteType > 0 Then

            ' Quote Type
            lReturn = m_oDatabase.Parameters.Add(sName:="quote_type", vValue:=CStr(v_lQuoteType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            ' Quote Type

            'developer guide no. 85
            lReturn = m_oDatabase.Parameters.Add(sName:="quote_type", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        ' BSJ 23112001 Add the Risk Group ID Parameter
        If v_lRiskGroupID > 0 Then

            ' Risk Group ID
            lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(v_lRiskGroupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            ' Quote Type

            'developer guide no. 85
            lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If v_bCalledFromSTS Then
            lReturn = m_oDatabase.Parameters.Add(sName:="Called_By_STS", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else
            lReturn = m_oDatabase.Parameters.Add(sName:="Called_By_STS", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If v_sRealTransactionType <> "" Then
            lReturn = m_oDatabase.Parameters.Add(sName:="Real_Transaction_Type", vValue:=v_sRealTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        Else

            'developer guide no. 85
            lReturn = m_oDatabase.Parameters.Add(sName:="Real_Transaction_Type", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        End If
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSchemesSQL, sSQLName:=ACGetSchemesName, bStoredProcedure:=ACGetSchemesStored, vResultArray:=r_vSchemesArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CJB 100804 Improve message written to sirius log with a suggestion to check scheme dates!
        If Not Informations.IsArray(r_vSchemesArray) Then
            sMsg = "Failed to Find Schemes to quote for (using " & ACGetSchemesSQL & " ) with the following parameters : " & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  gis_business_type_code:" & v_sGisBusinessTypeCode & Strings.ChrW(13) & Strings.ChrW(10)
            'developer guide no 40. 
            sMsg = sMsg & "  effective_date        :" & v_dtEffectiveDate & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  gis_policy_link_id    :" & CStr(v_lGisPolicyLinkID) & " ( if > 0 then null is passed ) " & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  gis_data_model_code   :" & v_sGisDataModelCode & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  gis_scheme_id         :" & CStr(v_lGISSchemeId) & " ( if > 0 then null is passed ) " & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  quote_type            :" & CStr(v_lQuoteType) & " ( if > 0 then null is passed ) " & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  risk_group_id         :" & CStr(v_lRiskGroupID) & " ( if > 0 then null is passed ) " & Strings.ChrW(13) & Strings.ChrW(10)
            sMsg = sMsg & "  Try checking the scheme end date to ensure it is within range..."
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemes")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Public Function GetSchemeData(ByVal v_sGisBusinessTypeCode As String, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeId As Integer, ByRef r_vSchemeArray As Object) As Integer

        '*********************************************************************
        '*      RJG 19/09/01 - Get Threshold data for a given scheme.
        '*********************************************************************

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vSchemeArray = Nothing

            m_oDatabase.Parameters.Clear()

            ' Business Type Code
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=v_sGisBusinessTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' GIS Data Model Code
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Scheme ID Parameter
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGISSchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGisSchemeDataSelSQL, sSQLName:=ACGisSchemeDataSelName, bStoredProcedure:=ACGisSchemeDataSelStored, vResultArray:=r_vSchemeArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeDataFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
