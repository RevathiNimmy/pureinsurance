Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 09/06/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a Risk Type RI Limit.
    '
    ' Edit History:
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 22/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lRiskTypeId As Integer

    Private m_lGISHeaderIndId1 As Integer
    Private m_lGISHeaderIndId2 As Integer
    Private m_lGISHeaderIndId3 As Integer

    'JMK 23/10/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""
    Private m_nRiskTypeLimitVersionId As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            Value = Value
        End Set
    End Property

    Public Property RiskTypeId() As Integer
        Get
            Return m_lRiskTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public Property GISHeaderIndId1() As Integer
        Get
            Return m_lGISHeaderIndId1
        End Get
        Set(ByVal Value As Integer)
            m_lGISHeaderIndId1 = Value
        End Set
    End Property

    Public Property GISHeaderIndId2() As Integer
        Get
            Return m_lGISHeaderIndId2
        End Get
        Set(ByVal Value As Integer)
            m_lGISHeaderIndId2 = Value
        End Set
    End Property

    Public Property GISHeaderIndId3() As Integer
        Get
            Return m_lGISHeaderIndId3
        End Get
        Set(ByVal Value As Integer)
            m_lGISHeaderIndId3 = Value
        End Set
    End Property

    Public Property RiskTypeLimitVersionId() As Integer
        Get
            Return m_nRiskTypeLimitVersionId
        End Get
        Set(value As Integer)
            m_nRiskTypeLimitVersionId = value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 23/10/2001    Created
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (getUnderwritingType) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function getUnderwritingType() As Integer
    '
    'Dim result As Integer = 0
    'Dim sSQL As String = ""
    '
    'Try 
    '
    '
    'Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUnderwritingTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                m_iTask = CInt(vTask)
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetRiskTypeRIValues(ByRef r_vRiskTypeRIValues As Object) As Integer

        Dim result As Integer
        Dim vArray(,) As Object
        Dim lLevel, lProperty As Integer
        Dim sSQL As String
        Dim vValueArray(,) As Object
        Dim vValue As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'It goes something like this...
            'We get the property for the risk type id and the level we're at

            'We should never, ever, have this level...
            lLevel = 4

            If m_lGISHeaderIndId3 = 0 Then
                lLevel = 3
            End If

            If m_lGISHeaderIndId2 = 0 Then
                lLevel = 2
            End If

            If m_lGISHeaderIndId1 = 0 Then
                lLevel = 1
            End If

            'If we've got nothing, we want the first level, construction type
            'If we've got a specific construction type indicator, we want the second level,
            'occupation, etc.

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_properties_seq_id", vValue:=CStr(lLevel), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeLimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISPropertySQL, sSQLName:=ACSelectGISPropertyName, bStoredProcedure:=ACSelectGISPropertyStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRIValues")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vArray) Then
                Return result
            End If


            lProperty = CInt(vArray(2, 0))

            'So now we've got the property id, so we have to get the indicators for it...

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_property_id", vValue:=CStr(lProperty), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'GSD 010702 Added specials reference
            sSQL = "select i.gis_user_def_header_inds_id," & Strings.Chr(13) & Strings.Chr(10) & _
                   "i.code" & Strings.Chr(13) & Strings.Chr(10) & _
                   "from gis_user_def_header_inds i," & Strings.Chr(13) & Strings.Chr(10) & _
                   "gis_property p" & Strings.Chr(13) & Strings.Chr(10) & _
                   "Where p.specials_type = 6" & Strings.Chr(13) & Strings.Chr(10) & _
                   "and p.specials_type_reference = i.gis_user_def_header_id" & Strings.Chr(13) & Strings.Chr(10) & _
                   "and p.gis_property_id = {gis_property_id}" & Strings.Chr(13) & Strings.Chr(10) & _
                   "and i.is_deleted = 0" & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Indicators", bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRIValues")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vArray) Then

                r_vRiskTypeRIValues = ""
                Return result
            End If

            'Now we've got the indicators we can get the values, if any...

            'One day maybe this will be rejigged so that we can have unlimited levels, but for now...
            Dim lHeaderIndId(2) As Integer

            lHeaderIndId(0) = m_lGISHeaderIndId1
            lHeaderIndId(1) = m_lGISHeaderIndId2
            lHeaderIndId(2) = m_lGISHeaderIndId3


            ReDim r_vRiskTypeRIValues(2, vArray.GetUpperBound(1))


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                m_oDatabase.Parameters.Clear()


                lHeaderIndId(lLevel - 1) = CInt(vArray(0, lTemp))

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                For lTemp2 As Integer = lHeaderIndId.GetLowerBound(0) To lHeaderIndId.GetUpperBound(0)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_user_def_header_inds_id" & lTemp2 + 1, vValue:=CStr(lHeaderIndId(lTemp2)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next lTemp2

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeLimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRiskTypeRIValuesSQL, sSQLName:=ACSelectRiskTypeRIValuesName, bStoredProcedure:=ACSelectRiskTypeRIValuesStored, vResultArray:=vValueArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vValueArray) Then

                    vValue = CInt(vValueArray(4, 0))
                Else

                    vValue = Nothing
                End If



                r_vRiskTypeRIValues(0, lTemp) = vArray(0, lTemp)


                r_vRiskTypeRIValues(1, lTemp) = vArray(1, lTemp)

                r_vRiskTypeRIValues(2, lTemp) = vValue
            Next lTemp

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskTypeRIValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTypeRIValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Name: Update (Public)
    '
    '
    ' ***************************************************************** '
    Public Function Update(ByVal v_vRiskTypeRIValues(,) As Object, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lLevel As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lLevel = 4

            If m_lGISHeaderIndId3 = 0 Then
                lLevel = 3
            End If

            If m_lGISHeaderIndId2 = 0 Then
                lLevel = 2
            End If

            If m_lGISHeaderIndId1 = 0 Then
                lLevel = 1
            End If

            For lTemp As Integer = v_vRiskTypeRIValues.GetLowerBound(1) To v_vRiskTypeRIValues.GetUpperBound(1)
                sSQL = "UPDATE risk_type_ri_values SET " &
                       "UserId = " & m_iUserID & ", " &
                       "UniqueId = '" & CStr(v_sUniqueId) & "', " &
                       "ScreenHierarchy = '" & CStr(v_sScreenHierarchy & "/Breakdown(" & CStr(v_vRiskTypeRIValues(1, lTemp)).Trim() & ")") & "' " &
                       "WHERE risk_type_id = " & CStr(m_lRiskTypeId) & " " &
                       "AND risk_type_ri_limit_version_id = " & CStr(m_nRiskTypeLimitVersionId)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Update Risk Type Values", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lTemp


            sSQL = "DELETE FROM risk_type_ri_values " &
                   "WHERE risk_type_id = " & CStr(m_lRiskTypeId) & " " &
                   "AND risk_type_ri_limit_version_id = " & CStr(m_nRiskTypeLimitVersionId) & vbCrLf


            'Slightly strange requirement here.  We want to ignore the last non-zero item,
            'but include earlier ones and delete when latter ones are 0

            Select Case lLevel
                Case 1
                    sSQL = sSQL & "AND gis_user_def_header_inds_id2 = 0" & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & "AND gis_user_def_header_inds_id3 = 0" & Strings.Chr(13) & Strings.Chr(10)
                Case 2
                    sSQL = sSQL & "AND gis_user_def_header_inds_id1 = " & CStr(m_lGISHeaderIndId1) & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & "AND gis_user_def_header_inds_id3 = 0" & Strings.Chr(13) & Strings.Chr(10)
                Case 3
                    sSQL = sSQL & "AND gis_user_def_header_inds_id1 = " & CStr(m_lGISHeaderIndId1) & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & "AND gis_user_def_header_inds_id2 = " & CStr(m_lGISHeaderIndId2) & Strings.Chr(13) & Strings.Chr(10)
            End Select

            'Delete the risk type values
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Delete Risk Type Values", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(v_vRiskTypeRIValues) Then
                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            Dim lHeaderIndId(2) As Integer

            lHeaderIndId(0) = m_lGISHeaderIndId1
            lHeaderIndId(1) = m_lGISHeaderIndId2
            lHeaderIndId(2) = m_lGISHeaderIndId3

            'Re-add the values
            For lTemp As Integer = v_vRiskTypeRIValues.GetLowerBound(1) To v_vRiskTypeRIValues.GetUpperBound(1)
                m_oDatabase.Parameters.Clear()


                lHeaderIndId(lLevel - 1) = CInt(v_vRiskTypeRIValues(0, lTemp))

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(m_lRiskTypeId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                For lTemp2 As Integer = lHeaderIndId.GetLowerBound(0) To lHeaderIndId.GetUpperBound(0)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_user_def_header_inds_id" & lTemp2 + 1, vValue:=CStr(lHeaderIndId(lTemp2)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next lTemp2


                m_lReturn = m_oDatabase.Parameters.Add(sName:="value", vValue:=CStr(v_vRiskTypeRIValues(2, lTemp)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeLimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy & $"/Breakdown({CStr(v_vRiskTypeRIValues(1, lTemp)).Trim()})", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskTypeRIValuesSQL, sSQLName:=ACInsertRiskTypeRIValuesName, bStoredProcedure:=ACInsertRiskTypeRIValuesStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update ", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' to get upper limit set in ri model
    ''' </summary>
    ''' <param name="r_nRIModelUpperLimit"></param>
    ''' <remarks></remarks>   
    Public Function GetRIModelUpperLimit(ByRef r_nRIModelUpperLimit As Long) As Long

        Dim dtResult As DataTable
        Dim nResult As Integer
        Dim nReturn As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=m_lRiskTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="risk_type_ri_limit_version_id", vValue:=m_nRiskTypeLimitVersionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACSelectRIModelUpperLimitSQL, sSQLName:=ACSelectRIModelUpperLimitName, bStoredProcedure:=ACSelectRIModelUpperLimitStored, oRecordset:=dtResult)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIModelUpperLimit")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If dtResult IsNot Nothing And dtResult.Rows.Count > 0 Then
                r_nRIModelUpperLimit = dtResult.Rows(0).Item(0)
            End If

            Return nResult


        Catch ex As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRIModelUpperLimit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRIModelUpperLimit", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)
            Return nResult
        End Try

    End Function



End Class