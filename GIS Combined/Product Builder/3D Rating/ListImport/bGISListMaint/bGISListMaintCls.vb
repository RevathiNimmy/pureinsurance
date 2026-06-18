Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Module Name: Main
    '
    ' Date: 28/06/2002
    '
    ' Description:  This contains the main constants
    '
    ' Edit History:
    '   28/06/2002 SJP  - Tidied up after merge from Carole Nash
    '                       Removed embedded SQL
    ' ***************************************************************** '


    Private m_lReturn As gPMConstants.PMEReturnCode
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

    'database object
    Private m_oDatabase As dPMDAO.Database
    Private m_oSA As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    Private Const ACClass As String = "Business"

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_vTableColumns As Object 'holds a copy of the table columns to imporve performance
    Private m_bAuditTrailCreated As Boolean 'tracks if audit trail was created from CreatePMLookup
    Private m_sUniqueId As String = "" 'holds the unique ID for audit trail

    Public Property AuditTrailCreated As Boolean
        Get
            Return m_bAuditTrailCreated
        End Get
        Set(value As Boolean)
            m_bAuditTrailCreated = value
        End Set
    End Property

    Public Property UniqueId As String
        Get
            Return m_sUniqueId
        End Get
        Set(value As String)
            m_sUniqueId = value
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

            'broking

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            'architecture

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oSA, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

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
    ' Name: GetListTypes
    '
    ' Description:  This will retrieve list types from database
    '
    ' History: 28/06/2002 SJP - tidied up and removed embedded SQL
    '
    ' ***************************************************************** '
    Public Function GetListTypes(ByRef vData(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            'NIIT DONE
            If Not m_oDatabase Is Nothing Then m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetListTypesSQL, sSQLName:=ACGetListTypesName, bStoredProcedure:=ACGetListTypesProc, vResultArray:=vData, bKeepNulls:=True)


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve List Types", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUnique", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Falied to get list types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListTypes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteListType
    '
    ' Description:  This will delete a list type
    '
    ' History: 28/06/2002 SJP - tidied up and removed embedded SQL
    '
    ' ***************************************************************** '
    Public Function DeleteListType(ByRef sListTypeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            'clear parameters
            m_oDatabase.Parameters.Clear()

            'add parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ID", vValue:=CStr(sListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteListTypeSQL, sSQLName:=ACDeleteListTypeName, bStoredProcedure:=ACDeleteListTypeProc)


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete List Type", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteListType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete lists type", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteListType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: IsUnique
    '
    ' Description:  This will check whether unique or not
    '
    ' History: 28/06/2002 SJP - tidied up, removed embedded SQL
    '
    ' ***************************************************************** '
    Public Function IsUnique(ByRef sCode As String, ByRef sDescription As String) As Integer

        Dim result As Integer = 0
        Dim vData(,) As Object

        Try

            'clear parameters
            m_oDatabase.Parameters.Clear()

            'add parameter Fields to look for
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'do it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACListTypeIsUniqueSQL, sSQLName:=ACListTypeIsUniqueName, bStoredProcedure:=ACListTypeIsUniqueProc, vResultArray:=vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to execute SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUnique", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If


            If Information.IsArray(vData) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check uniqueness", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUnique", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ListInUse
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Public Function ListInUse(ByRef sListTypeID As Integer, Optional ByRef sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim vData(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse 'assume not in use

            'clear parameters
            m_oDatabase.Parameters.Clear()

            'add parameter Field to look for
            m_lReturn = m_oDatabase.Parameters.Add(sName:="IDField", vValue:="gis_list_type_id", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'add parameter is to look for
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ID", vValue:=CStr(sListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsInUseSQL, sSQLName:=ACIsInUseName, bStoredProcedure:=ACIsInUseProc, vResultArray:=vData)

            'if we get data back and its in more than the current table then
            If Information.IsArray(vData) Then

                If vData.GetUpperBound(1) > 0 Then 'ignore entry in GIS_List_Type
                    result = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if list is in use", vApp:=ACApp, vClass:=ACClass, vMethod:="ListInUse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetListVersions
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Public Function GetListVersions(ByRef sListTypeID As Integer, ByRef vData(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            'clear params
            m_oDatabase.Parameters.Clear()

            'add param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ID", vValue:=CStr(sListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetListVersionsSQL, sSQLName:=ACGetListVersionsName, bStoredProcedure:=ACGetListVersionsProc, vResultArray:=vData)


            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get List versions SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list versions", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ListExists
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Public Function ListExists(ByRef sTable As String) As Integer

        Dim result As Integer = 0
        Dim vData(,) As Object

        Try


            m_vTableColumns = ""

            'clear params
            m_oDatabase.Parameters.Clear()

            'add param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'doit
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACListExistsSQL, sSQLName:=ACListExistsName, bStoredProcedure:=ACListExistsProc, vResultArray:=vData)


            If Information.IsArray(vData) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check list exists", vApp:=ACApp, vClass:=ACClass, vMethod:="ListExists", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ReplaceListItem
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up and removed embedded SQL
    '
    ' ***************************************************************** '
    Public Function ReplaceListItem(ByRef sListType As String, ByRef PMLookupID As Integer, ByRef vData(,) As Object, ByRef lIndex As Integer) As Integer

        Dim result As Integer = 0
        Dim vFields(,) As Object
        Dim sSQL As New StringBuilder
        Dim sOldCode, sDesc As String
        Dim vTemp(,) As Object
        Dim sNewCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(vData) Then

                sNewCode = CStr(vData(0, lIndex - 1))

                sDesc = CStr(vData(1, lIndex - 1))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType", vValue:=sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="newCode", vValue:=sNewCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelListItemSQL, sSQLName:=ACDelListItemName, bStoredProcedure:=ACDelListItemProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete list Item", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType", vValue:=sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup", vValue:=CStr(PMLookupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'go for it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetListItemSQL, sSQLName:=ACGetListItemName, bStoredProcedure:=ACGetListItemProc, vResultArray:=vTemp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list Item", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            If Information.IsArray(vTemp) Then

                sOldCode = CStr(vTemp(0, 0)).TrimEnd()
            End If


            'update rating
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="newCode", vValue:=sNewCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType", vValue:=sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup", vValue:=CStr(PMLookupID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'do it
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateListItemSQL, sSQLName:=ACUpdateListItemName, bStoredProcedure:=ACUpdateListItemProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get list Item", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceListItem", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'get column names
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:="UDL_" & sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFieldNamesSQL, sSQLName:=ACGetFieldNamesName, bStoredProcedure:=ACGetFieldNamesProc, vResultArray:=vFields)
            'check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get extra field names", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'update PMLookup

            'add non standard fields
            If Information.IsArray(vFields) Then

                For i As Integer = 6 To vFields.GetUpperBound(1)


                    sSQL.Append("," & CStr(vFields(0, i)) & " ='" & CStr(vData(i - 4, lIndex - 1)) & "'")
                Next i
            End If

            '   Add parametes
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="newCode", vValue:=sNewCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType", vValue:=sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="oldCode", vValue:=sOldCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="desc", vValue:=sDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="fields", vValue:=sSQL.ToString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'do it
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdatePMLookupSQL, sSQLName:=ACUpdatePMLookupName, bStoredProcedure:=ACUpdatePMLookupProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update PM lookup", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to replace list item", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplaceListItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreatePMLookup
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Public Function CreatePMLookup(ByRef sLName As String, ByRef vFields() As Object) As Integer

        Dim result As Integer = 0
        Dim fields As New StringBuilder

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'add extra fields
            If Information.IsArray(vFields) Then
                For Each vFields_item As Object In vFields

                    fields.Append(CStr(vFields_item) & " [varchar] (255),")
                Next vFields_item
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=sLName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="fields", vValue:=fields.ToString(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Add UniqueId parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Execute the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCreatePMLookupSQL, sSQLName:=ACCreatePMLookupName, bStoredProcedure:=ACCreatePMLookupProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create PM Lookup", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePMLookup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            Else
                m_bAuditTrailCreated = True
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create table", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePMLookup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' AddListEntry-This will add a list item
    ''' </summary>
    ''' <param name="sTable"></param>
    ''' <param name="vData"></param>
    ''' <param name="lIndex"></param>
    ''' <param name="dEffDate"></param>
    ''' <param name="lVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddListEntry(ByRef sTable As String, ByRef vData(,) As Object, ByRef lIndex As Integer,
                                 ByRef dEffDate As Date, ByVal lVersion As Long) As Integer

        Dim nResult As Integer = 0
        Dim sData As New StringBuilder
        Dim sFields As New StringBuilder
        Dim i As Integer
        Dim sEffDate As String = ""
        Dim iData As Integer

        Try

            'remove extra spaces
            sTable = sTable.Trim()

            If Not Information.IsArray(m_vTableColumns) Then

                'get extra field names
                m_oDatabase.Parameters.Clear()

                'add params and retrieve data
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable,
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                       iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFieldNamesSQL, sSQLName:=ACGetFieldNamesName,
                                                  bStoredProcedure:=ACGetFieldNamesProc,
                                                  vResultArray:=m_vTableColumns)
                'check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get extra field names", vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="AddListEntry", vErrNo:=Information.Err().Number,
                                       vErrDesc:=Information.Err().Description)
                    Return nResult
                End If

            End If
            iData = vData.GetUpperBound(0)
            'add extra fields

            'we need to add the data only to existing fields
            'add data            
            For i = 0 To iData 'MKR 10/08/04  PN: 13630
                If vData(i, lIndex) IsNot Nothing Then
                    If vData.GetUpperBound(0) >= i Then
                        If vData(i, lIndex).ToString().Trim() = "" Then
                            ' sData.Append(",'" & "NULL" & "'")
                        Else
                            If i <> 0 And i <> 1 Then ' Not execute when code and description
                                If UCase(m_vTableColumns(0, i + 4)) <> UCase("UDL_version") Then
                                    sFields.Append(",[" & ToSafeString(m_vTableColumns(0, i + 4)) & "]")
                                End If
                            End If
                            sData.Append(",'" & ToSafeString(vData(i, lIndex)).Replace("'", "''") & "'")
                        End If
                    Else
                        sData.Append(",''") 'The list getting imported has less fields
                    End If
                End If
            Next i

            '   Clear and insert
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="fields", vValue:=sFields.ToString(),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="data", vValue:=sData.ToString(),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            sEffDate = dEffDate.ToString("dd-MMM-yyyy")
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effdate", vValue:=sEffDate,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(m_iLanguageID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="caption", vValue:=CStr(vData(1, lIndex)),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="version", vValue:=lVersion,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If Not m_bAuditTrailCreated Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID),
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                       iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            ' Add UniqueId parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            '   Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACListEntryAddSQL, sSQLName:=ACListEntryAddName,
                                              bStoredProcedure:=ACListEntryAddProc)

            'check for errors

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                nResult = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to add list entry", vApp:=ACApp, vClass:=ACClass,
                                   vMethod:="AddListEntry", vErrNo:=Information.Err().Number,
                                   vErrDesc:=Information.Err().Description)
                Return nResult
            End If

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="Failed to add list entry", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="AddListEntry", vErrNo:=Information.Err().Number,
                               vErrDesc:=Information.Err().Description, excep:=excep)

            Return nResult
        End Try


    End Function

    ' ***************************************************************** '
    '
    ' Name: SetData
    '
    ' Description:  This will set the private variable
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '

    Public Function AddUsage(ByRef sTable As String, ByRef sCode As String, ByRef lVersion As Integer, ByRef dEffDate As Date) As Integer

        Dim result As Integer = 0
        Dim sListType As String

        Try

            'get list type
            sListType = Mid(sTable, 5, sTable.Length - 4).Trim()

            'rest of processing done in stored proc
            'it checks for presence in code rate lists, adds if necessary and adds entry to the usage table

            'clear params
            m_oDatabase.Parameters.Clear()

            'add table
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ListType", vValue:=sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'add code
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'add version
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Version", vValue:=CStr(lVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            'add eff date
            m_lReturn = m_oDatabase.Parameters.Add(sName:="EffDate", vValue:=dEffDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            'do it
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddUsageSQL, sSQLName:=ACAddUsageName, bStoredProcedure:=ACAddUsageProc)

            'check for errors

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add usage", vApp:=ACApp, vClass:=ACClass, vMethod:="AddUsage", vErrNo:=1, vErrDesc:="Failed to add usage")

                Return result
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try


        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add usage", vApp:=ACApp, vClass:=ACClass, vMethod:="AddUsage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetData
    '
    ' Description:  This will set the private variable
    '
    ' History: 28/06/2002 SJP - tidied up and removed embedded SQL
    '
    ' ***************************************************************** '
    Public Function ListItemExists(ByRef sTable As String, ByRef sCode As String, Optional ByRef lVersion As Long = 0) As Integer

        Dim result As Integer = 0
        Dim vData(,) As Object

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Version", vValue:=lVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'do it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACListItemExistsSQL, sSQLName:=ACListItemExistsName, bStoredProcedure:=ACListItemExistsProc, vResultArray:=vData)
            'error check
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check item exists", vApp:=ACApp, vClass:=ACClass, vMethod:="ListItemExists", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If


            If Information.IsArray(vData) Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check exists", vApp:=ACApp, vClass:=ACClass, vMethod:="ListItemExists", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: ImportList
    '
    ' Description:  This will import a list
    '
    ' History: 28/06/2002 SJP - kept from current code
    '
    ' ***************************************************************** '

    Public Function ImportList(ByRef sFile As String, ByRef vData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lFreeFile As Integer
        Dim sRecord As String = ""
        Dim lFieldCount, lTotalFields, lLastCommaPos, lRecordCount As Integer
        Dim bIsInBetweenDoubleQuote As Boolean
        Dim lLastDoubleQuotePosition, lCountOfConsecutiveDoubleQuotes As Integer
        Dim sNewStringPartOne As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get free file no
            lFreeFile = FileSystem.FreeFile()

            'set field count to zero
            lFieldCount = 0
            FileSystem.FileClose()
            'open file
            FileSystem.FileOpen(lFreeFile, sFile, OpenMode.Input)

            'set record count to zero
            lRecordCount = -1

            'do until no records are left
            Do While Not FileSystem.EOF(lFreeFile)

                'TR - Reset counters
                lCountOfConsecutiveDoubleQuotes = 0

                'get record from the ile
                sRecord = FileSystem.LineInput(lFreeFile)

                'if first time around then count fields
                If lRecordCount = -1 Then
                    lTotalFields = 0
                    For i As Integer = 1 To sRecord.Length
                        'TR - If we are inbetween Quote marks, ignore commas, otherwise use them to mark new fields
                        If Not bIsInBetweenDoubleQuote Then
                            If Mid(sRecord, i, 1) = "," Then
                                lTotalFields += 1
                            End If
                        End If

                        'TR - Is this a double Quote mark (to allow commas in description)
                        If Mid(sRecord, i, 1) = ChrW(34) Then
                            bIsInBetweenDoubleQuote = Not (bIsInBetweenDoubleQuote)
                        End If
                    Next i
                    'dim array to match fields
                    ReDim vData(lTotalFields, 0)
                End If

                'increment record count
                lRecordCount += 1

                'resize array to add new record
                ReDim Preserve vData(lTotalFields, lRecordCount)

                'set first field start to 0
                lLastCommaPos = 0
                lFieldCount = 0
                lLastDoubleQuotePosition = 0
                bIsInBetweenDoubleQuote = False

                'step thru char by char
                For i As Integer = 1 To sRecord.Length
                    'TR - If this is the char in the last string, then it can't be a delimiter comma
                    If i = sRecord.Length Then
                        'TR - If this is a , or a " then remove it
                        If Mid(sRecord, i, 1) = "," Or Mid(sRecord, i, 1) = ChrW(34) Then
                            'load into array string between last comma and current pos

                            vData(lFieldCount, lRecordCount) = Mid(sRecord, lLastCommaPos + 1, i - lLastCommaPos - 1)
                            If Mid(sRecord, i, 1) = ChrW(34) Then
                                bIsInBetweenDoubleQuote = False
                            End If
                        Else
                            'load into array string between last comma and current pos

                            vData(lFieldCount, lRecordCount) = Mid(sRecord, lLastCommaPos + 1, i - lLastCommaPos)
                        End If
                    Else
                        'TR - If we are inbetween Quote marks, ignore commas, otherwise use them to mark new fields
                        If Not bIsInBetweenDoubleQuote Then
                            'TR - look for delimiter commas
                            If Mid(sRecord, i, 1) = "," Then

                                'load into array string between last comma and current pos

                                vData(lFieldCount, lRecordCount) = Mid(sRecord, lLastCommaPos + 1, i - lLastCommaPos - 1)

                                'increment no of fields
                                lFieldCount += 1

                                'set pos of new comma
                                lLastCommaPos = i
                            End If
                        End If

                        'TR - Is this a double Quote mark (to allow commas in description)
                        If Mid(sRecord, i, 1) = ChrW(34) Then
                            'TR - First check that this double quote does not immediately follow another one
                            If lLastDoubleQuotePosition = i - 1 Then
                                'TR - Are we in a "between quotes" status or merely a "prefix quotes"
                                'Even number of "s (before this new one), remove this one
                                If lCountOfConsecutiveDoubleQuotes Mod 2 = 0 Then
                                    'TR - Remove this " from the string
                                    sNewStringPartOne = sRecord.Substring(0, i - 1) & sRecord.Substring(sRecord.Length - (sRecord.Length - i))
                                    sRecord = sNewStringPartOne
                                    'TR - Reduce i by 1 (to make up for deleting this character)
                                    i -= 1
                                End If
                                'TR -Add one to the count of consecutive Quote marks
                                lCountOfConsecutiveDoubleQuotes += 1
                            Else
                                'TR - Remove this " from the string
                                sNewStringPartOne = sRecord.Substring(0, i - 1) & sRecord.Substring(sRecord.Length - (sRecord.Length - i))
                                sRecord = sNewStringPartOne
                                'TR - Reduce i by 1 (to make up for deleting this character)
                                i -= 1
                                'TR - Reset consecutive counter
                                lCountOfConsecutiveDoubleQuotes = 1
                            End If
                            'Switch modes
                            bIsInBetweenDoubleQuote = Not (bIsInBetweenDoubleQuote)
                            lLastDoubleQuotePosition = i
                        Else
                            lCountOfConsecutiveDoubleQuotes = 0
                        End If
                    End If
                Next i
            Loop

            FileSystem.FileClose(lFreeFile)

            Return result

        Catch excep As System.Exception


            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to import list", vApp:=ACApp, vClass:=ACClass, vMethod:="ImportList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveNewListType
    '
    ' Description:
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Public Function SaveNewListType(ByRef sCode As String, ByRef sDescription As String) As Integer

        Dim result As Integer = 0
        Try

            'clear params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Description", vValue:=sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'do it
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSaveNewListTypeSQL, sSQLName:=ACSaveNewListTypeName, bStoredProcedure:=ACSaveNewListTypeProc)

            'error check
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Save New List Type", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveNewListType", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save new list", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveNewListType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetData
    '
    ' Description:  This will set the private variable
    '
    ' History: 28/06/2002 SJP - tidied up
    '
    ' ***************************************************************** '
    Public Function GetUserDefinedCodes(ByRef sListType As String, ByRef vData(,) As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            'clear parameters
            m_oDatabase.Parameters.Clear()

            'add param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="listtype", vValue:=sListType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            'do it
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserDefinedCodesSQL, sSQLName:=ACGetUserDefinedCodesName, bStoredProcedure:=ACGetUserDefinedCodesProc, vResultArray:=vData)
            'check for errors

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try


        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Falied to get codes", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefinedCodes", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetColumnList
    '
    ' Description:  This will return the list of columns to the calling
    ' function... and will used to determine that whether there is need
    ' for adding new column or not...
    '
    ' History: 10/08/2004 MKR PN : 13630 - new function created...
    '
    ' ***************************************************************** '
    Public Function GetColumnList(ByRef sTable As String, ByRef vFields(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'remove extra spaces
            sTable = sTable.Trim()

            'get field names
            m_oDatabase.Parameters.Clear()

            'add params and retrieve data
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFieldNamesSQL, sSQLName:=ACGetFieldNamesName, bStoredProcedure:=ACGetFieldNamesProc, vResultArray:=vFields)
            'check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get field names", vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumnList", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get field names", vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumnList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddColumn
    '
    ' Description: Will add an extra column in the table supplied as
    ' parameter...
    '
    ' History: 10/08/2004 MKR PN : 13630 - new function created...
    '
    ' ***************************************************************** '
    Public Function AddColumn(ByRef sLName As String, ByRef sField As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Adding an extra column...
            m_oDatabase.Parameters.Clear()

            'Adding the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=sLName.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="field", vValue:=sField.Trim(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Execute the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCreatePMLookupUpdateSQL, sSQLName:=ACCreatePMLookupUpdateName, bStoredProcedure:=ACCreatePMLookupUpdateProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add column", vApp:=ACApp, vClass:=ACClass, vMethod:="AddColumn", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add column", vApp:=ACApp, vClass:=ACClass, vMethod:="AddColumn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateListEntry
    '
    ' Description:  This will update a list item
    '
    ' History: 14/06/2006
    '
    ' ***************************************************************** '

    Public Function UpdateListEntry(ByRef sTable As String, ByRef vData(,) As Object, ByRef lIndex As Integer, ByRef dEffDate As Date) As Integer

        Dim result As Integer = 0
        Dim sField, sCode, sData As String
        Dim sEffDate As String = ""

        Try

            'remove extra spaces
            sTable = sTable.Trim()

            If Not Information.IsArray(m_vTableColumns) Then

                'get extra field names
                m_oDatabase.Parameters.Clear()

                'add params and retrieve data
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFieldNamesSQL, sSQLName:=ACGetFieldNamesName, bStoredProcedure:=ACGetFieldNamesProc, vResultArray:=m_vTableColumns)
                'check for errors
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get extra field names", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End If

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            sField = "Description"
            m_lReturn = m_oDatabase.Parameters.Add(sName:="field", vValue:=sField, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            sCode = CStr(vData(0, lIndex))

            sData = CStr(vData(1, lIndex))

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="data", vValue:=sData, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            sEffDate = dEffDate.ToString("dd-MMM-yyyy")
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effdate", vValue:=sEffDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="caption", vValue:=CStr(vData(1, lIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If Not m_bAuditTrailCreated Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            ' Add UniqueId parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            '   Execute the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACListEntryUpdateSQL, sSQLName:=ACListEntryUpdateName, bStoredProcedure:=ACListEntryUpdateProc)

            If Information.IsArray(m_vTableColumns) Then


                For i As Integer = 6 To m_vTableColumns.GetUpperBound(1)
                    '   Clear and insert
                    m_oDatabase.Parameters.Clear()

                    sField = "[" & CStr(m_vTableColumns(0, i)) & "]"


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Table", vValue:=sTable, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="field", vValue:=sField, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    sCode = CStr(vData(0, lIndex))

                    sData = CStr(vData(i - 4, lIndex))

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="data", vValue:=sData, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    sEffDate = dEffDate.ToString("dd-MMM-yyyy")
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="effdate", vValue:=sEffDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="caption", vValue:=CStr(vData(1, lIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If Not m_bAuditTrailCreated Then
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If

                    ' Add UniqueId parameter
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    '   Execute the stored procedure
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACListEntryUpdateSQL, sSQLName:=ACListEntryUpdateName, bStoredProcedure:=ACListEntryUpdateProc)
                Next
            End If

            'check for errors

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            Else
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add list entry", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

        Catch
        End Try


        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add list entry", vApp:=ACApp, vClass:=ACClass, vMethod:="AddListEntry", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)


        Return result
    End Function
    ' ******************************************************************** '
    ' Name: GetUDLID
    '
    ' Description: Gets the UDL ID for the passed table and code
    '
    ' ******************************************************************** '
    Public Function GetUDLData(ByVal v_sTableName As String, ByVal v_sCode As String, ByRef r_vUDLData(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the code
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUDLDataSQL, sSQLName:=ACGetUDLDataName, bStoredProcedure:=ACGetUDLDataProc, vResultArray:=r_vUDLData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that some data was returned
            If Not Information.IsArray(r_vUDLData) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUDLData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUDLData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ******************************************************************** '
    ' Name: UpdateUDLData
    '
    ' Description: update the UDL data for the passed code
    '
    ' ******************************************************************** '
    Public Function UpdateUDLData(ByVal v_sTableName As String, ByVal v_sCode As String, ByVal v_lCaption_id As Integer, ByVal v_sDescription As String, ByVal v_lVersion As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the code
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Add the caption_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(v_lCaption_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the description
            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="version", vValue:=v_lVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not m_bAuditTrailCreated Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Add UniqueId parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateUDLDataSQL, sSQLName:=ACUpdateUDLDataName, bStoredProcedure:=ACUpdateUDLDataProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUDLData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUDLData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Update the UDL version
    Public Function UpdateUDLVersion(ByVal v_sTableName As String,
                                    ByVal v_lOldVersion As Long,
                                    ByVal v_lVersion As Long) As Long



        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="version", vValue:=v_lVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="oldversion", vValue:=v_lOldVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not m_bAuditTrailCreated Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Add UniqueId parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateUDLVersionSQL, sSQLName:=ACUpdateUDLVersionName, bStoredProcedure:=ACUpdateUDLVersionProc)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUDLVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUDLVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function
    'Update the UDL version
    Public Function GetMaxUDLVersion(ByVal v_sTableName As String,
                               ByRef r_lVersion As Long) As Long


        Dim vData(,) As Object

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMaxUDLVersionSQL, sSQLName:=ACGetMaxUDLVersionName, bStoredProcedure:=ACGetMaxUDLVersionProc, vResultArray:=vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If IsArray(vData) Then
                r_lVersion = ToSafeLong(vData(0, 0))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUDLVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUDLVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function

    Public Function GetGISUDLDetail(ByVal v_sTableName As String,
                               ByRef r_vData(,) As Object) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="table", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGISUDLDetailSQL, sSQLName:=ACGetGISUDLDetailName, bStoredProcedure:=ACGetGISUDLDetailProc, vResultArray:=r_vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUDLVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUDLVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

    End Function



    ' ***************************************************************** '
    Public Function GetInsuranceFileDetails(
                            ByVal v_lInsuranceFileCnt As Long,
                            ByRef r_vResults(,) As Object) As Long


        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_SIR_Get_Insurance_File_Details", sSQLName:="spu_SIR_Get_Insurance_File_Details", bStoredProcedure:=True, vResultArray:=r_vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateUDLVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateUDLVersion", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try

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