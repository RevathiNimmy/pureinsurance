Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    '
    ' Edit History:
    ' 25/02/05 CJB PN16559 Changed RateInUse to accept new GIS_Scheme_ID parameter which is now
    '              required as we have changed the unique index to be based upon description AND
    '              GIS_Scheme_ID in the GIS_Rate_type table.
    '

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
    ' ************************************************


    Private Const ACClass As String = "Business"

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Instance of the database
    Private m_oDatabase As dPMDAO.Database

    ' To close, or not to close
    Private m_bCloseDatabase As Boolean

    ' GISSchemeID
    Private m_lGISSchemeID As Integer

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property GISSchemeID() As Integer
        Get
            Return m_lGISSchemeID
        End Get
        Set(ByVal Value As Integer)
            m_lGISSchemeID = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
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
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise





        Dim result As Integer = 0
        Try

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


            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Get component services

            ' Initialise the database

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to get connection to the database", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Remove component services


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetRateTypes
    '
    ' Description: Gets the values in the gis_rate_tables
    '
    ' History: 29/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetRateTypeTable(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear parameters
            m_oDatabase.Parameters.Clear()

            ' Add scheme_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRateTypeTableSQL, sSQLName:=ACGetRateTypeTableName, bStoredProcedure:=ACGetRateTypeTableStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetRateTypeTable SQL Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRateTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRateTypeTable Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRateTypeTable", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    Public Function GetListTypes(ByRef vData(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetListTypes, sSQLName:="Get List Types", bStoredProcedure:=True, vResultArray:=vData)

            'check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetListTypes SQL Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRateTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function SaveRateType(ByRef lSchemeID As Integer, ByRef sDescription As String, ByRef lListType1 As Integer, ByRef lListType2 As Integer, ByRef lListType3 As Integer, ByRef lRateTypeID As Integer) As Object
        Try

            'clear params
            m_oDatabase.Parameters.Clear()

            'add schemeid
            m_lReturn = m_oDatabase.Parameters.Add(sName:="SchemeID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'add description
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'add lu1
            m_lReturn = m_oDatabase.Parameters.Add(sName:="listtype1", vValue:=CStr(lListType1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'add lu2

            If lListType2 = 0 Then


                'Developer Guide No 85

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType2", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType2", vValue:=CStr(lListType2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If



            'ad lu3

            If lListType3 = 0 Then


                'Developer Guide No. 85

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType3", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType3", vValue:=CStr(lListType3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            End If

            'ratetypeid
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RateTypeID", vValue:=CStr(lRateTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'do it
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskType, sSQLName:="Add Risk Type", bStoredProcedure:=True)

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SaveRateType", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRateType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try


    End Function

    Public Function GetAxes(ByRef sRateType As String, ByRef lSchemeID As Integer, ByRef vData(,) As Object) As Object
        Try

            'clear parameters
            m_oDatabase.Parameters.Clear()

            'add listtype
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RateTypeDesc", vValue:=sRateType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            'add scheme
            m_lReturn = m_oDatabase.Parameters.Add(sName:="SchemeID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'do it
            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAxes, sSQLName:="Get Rate Axes", bStoredProcedure:=True, vResultArray:=vData)

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetAxes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAxes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Function

    Public Function GetAxis(ByRef lSchemeID As Integer, ByRef lListTypeID As Integer, ByRef vData(,) As Object) As Object
        Try

            'clear parameters
            m_oDatabase.Parameters.Clear()

            'schemeid
            m_lReturn = m_oDatabase.Parameters.Add(sName:="SchemeID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            'list type id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ListTypeId", vValue:=CStr(lListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'do it
            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAxis, sSQLName:="Get Rate Axis", bStoredProcedure:=True, lNumberRecords:=-1, vResultArray:=vData)

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Axis", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAxis", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try


    End Function

    Public Function GetMatrix(ByRef lSchemeID As Integer, ByRef sRateType As String, ByRef vData(,) As Object, Optional ByRef sGroupZ As String = "") As Object
        Try

            'clear params
            m_oDatabase.Parameters.Clear()

            'add schemeid
            m_lReturn = m_oDatabase.Parameters.Add(sName:="SchemeID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'add sratetype
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RateType", vValue:=sRateType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Add zgroup
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ZGroup", vValue:=sGroupZ, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'go for it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMatrix, sSQLName:="Get Matrix", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vData)

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Matrix", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMatrix", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Function

    Public Function FillMatrix(ByRef lSchemeID As Integer, ByRef sRateType As String) As Object
        Try

            'clear params
            m_oDatabase.Parameters.Clear()
            'add schemeid
            m_lReturn = m_oDatabase.Parameters.Add(sName:="SchemeID", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'add ratetype
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RateType", vValue:=sRateType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'go for it
            'developer guide no.39

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_GIS_Fill_Matrix", sSQLName:="Fill Matrix", bStoredProcedure:=True)

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fill Matrix", vApp:=ACApp, vClass:=ACClass, vMethod:="Fill Matrix", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Function

    'ED 12072002 - Change Rate to double as can hold decimals - up to four
    Public Function SaveRate(ByRef lSchemeID As Integer, ByRef sRateType As String, ByRef LU1 As Integer, ByRef LU2 As Integer, ByRef LU3 As Integer, ByRef dRate As Double) As Object
        Try

            'clear params
            m_oDatabase.Parameters.Clear()

            'schemeid
            m_lReturn = m_oDatabase.Parameters.Add(sName:="schemeid", vValue:=CStr(lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'ratetype
            m_lReturn = m_oDatabase.Parameters.Add(sName:="RateType", vValue:=sRateType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'lu1
            If LU1 = 0 Then

                'Developer guide No. 85

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup1", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup1", vValue:=CStr(LU1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            'lu2
            If LU2 = 0 Then

                'Developer Guide No. 85

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup2", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup2", vValue:=CStr(LU2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            'lu3
            If LU3 = 0 Then

                'Developer Guide No.85

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup3", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Lookup3", vValue:=CStr(LU3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            'rate
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Rate", vValue:=CStr(dRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

            'do it
            'developer guide no.39

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_GIS_Save_rate", sSQLName:="Save Rate", bStoredProcedure:=True)

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save rate", vApp:=ACApp, vClass:=ACClass, vMethod:="Save Rate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Function

    Public Function RateInUse(ByRef sDescription As String, ByVal v_lGISSchemeID As Integer) As Boolean  ' PN16559

        Try
            Dim vData(,) As Object
            'clear params
            m_oDatabase.Parameters.Clear()

            'Add description
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            ' Add gis_scheme_id  PN16559
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_Scheme_ID", vValue:=CStr(v_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACRateInUse, sSQLName:="Get Use", bStoredProcedure:=True, vResultArray:=vData)

            'Returns Count in Vdata


            Return CInt(vData(0, 0)) > 0

        Catch
        End Try


        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Rate", vApp:=ACApp, vClass:=ACClass, vMethod:="RateInUse", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    End Function
    Private Shared _DefaultInstance As Business = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Business
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Business
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
