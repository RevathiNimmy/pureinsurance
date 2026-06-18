Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'Developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

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

    ' Return value
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Instance of the database
    Private m_oDatabase As dPMDAO.Database

    ' To close, or not to close
    Private m_bCloseDatabase As Boolean

    ' GISSchemeID
    Private m_lGISSchemeID As Integer

    Public Property GISSchemeID() As Integer
        Get
            Return m_lGISSchemeID
        End Get
        Set(ByVal Value As Integer)
            m_lGISSchemeID = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 15/11/2001 CTAF - Created.
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
    ' History: 15/11/2001 CTAF - Created.
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
    ' History: 15/11/2001 CTAF - Created.
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
    ' Name: GetGroupSummary
    '
    ' Description: Gets the list types and the summary of groups associated
    '
    ' History: 15/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetGroupSummary(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the gis scheme id
            ' This is a property and set after this component is initialised
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter - gis_scheme_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupSummary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Call the sql
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSummarySQL, sSQLName:=ACGetSummaryName, bStoredProcedure:=ACGetSummaryStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SQLSelect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupSummary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGroupSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGroupSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetItemsSummary
    '
    ' Description:
    '
    ' History: 15/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetItemsSummary(ByVal v_lGISListTypeID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the gis scheme id
            ' This is a property and set after this component is initialised
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter - gis_scheme_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetItemsSummary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_type_id", vValue:=CStr(v_lGISListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter - gis_list_type_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetItemsSummary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Call the sql
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetItemsSummarySQL, sSQLName:=ACGetItemsSummaryName, bStoredProcedure:=ACGetItemsSummaryStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SQLSelect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetItemsSummary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetItemsSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetItemsSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetListItems
    '
    ' Description: Gets the list items for a type
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetListItems(ByVal v_lGISListTypeID As Integer, ByVal v_lGISListGroupingID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new ones
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter - gis_scheme_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_type_id", vValue:=CStr(v_lGISListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter - gis_list_type_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(v_lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter - gis_list_grouping_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Call the procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetListItemsSQL, sSQLName:=ACGetListItemsName, bStoredProcedure:=ACGetListItemsStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="SQLSelect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetListItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetListItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateGroupItems
    '
    ' Description: Updates the items associated with a group
    '
    ' History: 20/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateGroupItems(ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_lGISListGroupingID As Integer, ByVal v_vDataArray(,) As Object, ByVal v_bCheckUsed As Boolean) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CTAF 211101 - Having just finished this, my thoughts are that this
            '               could be best written as an SP if time allows.

            ' CTAF 111201 - Check if its in use or not?
            If v_bCheckUsed Then

                ' Check the code isn't in use already
                sSQL = "SELECT * FROM gis_list_grouping WHERE " & _
                       "gis_scheme_id = {gis_scheme_id} AND " & _
                       "code = {code} AND " & _
                       "description = {description} AND " & _
                       "gis_list_grouping_id <> {gis_list_grouping_id} AND " & _
                       "is_deleted = 0"

                ' Clear the parameters
                m_oDatabase.Parameters.Clear()

                ' Add gis_scheme_id
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Code
                m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Description
                m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' List_Grouping_id
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(v_lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the sql
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="CheckGroupExists", bStoredProcedure:=False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oDatabase.Records.Count() > 0 Then
                    ' Found, too bad
                    Return gPMConstants.PMEReturnCode.PMRecordInUse
                End If

            End If

            ' Update code and description
            sSQL = "UPDATE gis_list_grouping " & _
                   "SET code = {code}, " & _
                   "    description = {description} " & _
                   "WHERE gis_list_grouping_id = {gis_list_grouping_id}"

            ' Clear the paramters
            m_oDatabase.Parameters.Clear()

            ' Code
            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Description
            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' List_Grouping_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(v_lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateGroupItems", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGroupItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Now update the Items

            ' Delete the old ones
            sSQL = "DELETE FROM gis_list_grouping_items " & _
                   "WHERE gis_list_grouping_id = {gis_list_grouping_id} AND " & _
                   "gis_scheme_id = {gis_scheme_id}"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(v_lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteItems", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' This is going to be really ineffecient for mass data...
            ' What other way to do it???
            If Information.IsArray(v_vDataArray) Then

                ' Add the new ones
                For lLoop1 As Integer = 0 To v_vDataArray.GetUpperBound(1)


                    If CInt(v_vDataArray(ACListArraySelected, lLoop1)) = 1 Then

                        ' Add it
                        sSQL = "INSERT INTO gis_list_grouping_items " & _
                               "(gis_list_grouping_id, gis_scheme_id, gis_list_items_id) VALUES " & _
                               "({gis_list_grouping_id}, {gis_scheme_id}, {gis_list_items_id})"

                        m_oDatabase.Parameters.Clear()

                        ' Add parameters
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(v_lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Add parameters
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' GIS List Items ID

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_items_id", vValue:=CStr(v_vDataArray(ACListArrayID, lLoop1)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Call the SQL
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddItem", bStoredProcedure:=False)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGroupItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        End If

                    End If

                Next lLoop1

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateGroupItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateGroupItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddItems
    '
    ' Description: Adds items to the GIS List Grouping
    '
    ' History: 27/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function AddItems(ByVal v_sCode As String, ByVal v_sDescription As String, ByVal v_lGISListTypeID As Integer, ByVal v_vDataArray() As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lGISListGroupingID, lGISListItemsId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the new ones
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_type_id", vValue:=CStr(v_lGISListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_sDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the output parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddGroupingSQL, sSQLName:=ACAddGroupingName, bStoredProcedure:=ACAddGroupingStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the id
            lGISListGroupingID = m_oDatabase.Parameters.Item("gis_list_grouping_id").Value

            ' Add the items
            If Information.IsArray(v_vDataArray) Then

                sSQL = "INSERT INTO gis_list_grouping_items " & _
                       "(gis_list_grouping_id, gis_scheme_id, gis_list_items_id) VALUES " & _
                       "(" & CStr(lGISListGroupingID) & "," & CStr(m_lGISSchemeID) & ",{gis_list_items_id})"

                For lLoop1 As Integer = 0 To v_vDataArray.GetUpperBound(0)

                    ' Clear the parameters
                    m_oDatabase.Parameters.Clear()


                    lGISListItemsId = CInt(v_vDataArray(lLoop1))

                    ' Add the item_id
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_items_id", vValue:=CStr(lGISListItemsId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Call the sql
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddItem", bStoredProcedure:=False)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Next lLoop1

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItems Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: AutoGroup
    '
    ' Description: Autogroups the items
    '
    ' History: 26/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function AutoGroup(ByVal v_lGISListTypeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            ' Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(m_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_type_id", vValue:=CStr(v_lGISListTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the procedure
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAutoGroupSQL, sSQLName:=ACAutoGroupName, bStoredProcedure:=ACAutoGroupStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to autogroup the items. Check that the items are configured correctly.", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoGroup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Artinsoft.VB6.Utils.NotUpgradedHelper.NotifyNotUpgradedElement("Resume Statement")

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessDeleted
    '
    ' Description: Deletes/undeletes items
    '
    ' History: 28/11/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function ProcessDeleted(ByRef v_vDataArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lGISListGroupingID As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Exit, but dont error. that's bad mk'ay
            If Not Information.IsArray(v_vDataArray) Then
                Return result
            End If

            ' Construct the SQL
            sSQL = "DELETE FROM gis_list_grouping " & _
                   "WHERE gis_list_grouping_id = {gis_list_grouping_id}"

            ' CTAF 270602 - Permanently delete now!
            For lLoop1 As Integer = 0 To v_vDataArray.GetUpperBound(1)

                ' Marked for deletion?

                If CStr(v_vDataArray(ACGroupingArrayIsDeleted, lLoop1)) = "1" Then

                    ' Clear the parameters
                    m_oDatabase.Parameters.Clear()

                    ' Add the grouping_id parameter

                    lGISListGroupingID = CInt(v_vDataArray(ACGroupingArrayID, lLoop1))

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_list_grouping_id", vValue:=CStr(lGISListGroupingID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Call the SQL
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteItem", bStoredProcedure:=False)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update is_deleted for gis_list_grouping_id " & lGISListGroupingID, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDeleted", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If


                End If

            Next lLoop1

            '    ' Construct the SQL
            '    sSQL$ = "UPDATE gis_list_grouping " & _
            ''            "SET is_deleted = {is_deleted} " & _
            ''            "WHERE gis_list_grouping_id = {gis_list_grouping_id}"
            '
            '    For lLoop1 = 0 To UBound(v_vDataArray, 2)
            '
            '        ' Get the values
            '        iIsDeleted = v_vDataArray(ACGroupingArrayIsDeleted, lLoop1)
            '        lGISListGroupingID = v_vDataArray(ACGroupingArrayID, lLoop1)
            '
            '        ' Clear the parameters
            '        m_oDatabase.Parameters.Clear
            '
            '        ' Add the parameters
            '        m_lReturn& = m_oDatabase.Parameters.Add( _
            ''                        sName:="is_deleted", _
            ''                        vValue:=iIsDeleted, _
            ''                        iDirection:=PMParamInput, _
            ''                        iDatatype:=PMInteger)
            '        If (m_lReturn& <> PMTrue) Then
            '            ProcessDeleted = PMFalse
            '            Exit Function
            '        End If
            '
            '        m_lReturn& = m_oDatabase.Parameters.Add( _
            ''                        sName:="gis_list_grouping_id", _
            ''                        vValue:=lGISListGroupingID, _
            ''                        iDirection:=PMParamInput, _
            ''                        iDatatype:=PMInteger)
            '        If (m_lReturn& <> PMTrue) Then
            '            ProcessDeleted = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' Call the SQL
            '        m_lReturn& = m_oDatabase.SQLAction( _
            ''                        sSQL:=sSQL$, _
            ''                        sSQLName:="DeleteItem", _
            ''                        bStoredProcedure:=False)
            '        If (m_lReturn& <> PMTrue) Then
            '            ProcessDeleted = PMFalse
            '            ' Log Error Message
            '            LogMessage m_sUsername, _
            ''                iType:=PMLogOnError, _
            ''                sMsg:="Failed to update is_deleted for gis_list_grouping_id " & CStr(lGISListGroupingID), _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="ProcessDeleted", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '            Exit Function
            '        End If
            '
            '    Next lLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessDeleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDeleted", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
