Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 28/08/1998
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required by the Navigator
    '              Interface component.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property
    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set Password

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            ' Create Component Services
            '    Set oComponentServices = New PMServerBusinessCS

            ' Check the Database instance we have been supplied.
            '    m_lReturn& = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID,  _
            'v_lPMProductFamily:=PMProductFamily, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oComponentServices = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Name: GetProcessID
    '
    ' Description: Gets the ProcessID for the supplied combination of,
    '              Process Code, Product Code and Effective Date.
    '
    ' ***************************************************************** '
    Public Function GetProcessID(ByVal v_sPMNavProcessCode As String, ByVal v_sPMProductCode As String, ByVal v_dtEffectiveDate As Date, ByRef r_lPMNavProcessID As Integer) As Integer

        Dim result As Integer = 0
        Dim vProcessID As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase.Parameters

                .Clear()

                m_lReturn = .Add(sname:="product_code", vValue:=CStr(v_sPMProductCode.Trim()), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Add(sname:="process_code", vValue:=CStr(v_sPMNavProcessCode.Trim()), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Add(sname:="effective_date", vValue:=CStr(v_dtEffectiveDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Add(sname:="process_id", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, idatatype:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            m_lReturn = m_oDatabase.SQLAction(sSql:=ACGetProcessIDSQL, ssqlname:=ACGetProcessIDName, bstoredprocedure:=ACGetProcessIDStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_lPMNavProcessID = 0
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Returned Process ID

            vProcessID = m_oDatabase.Parameters.Item("process_id").Value


            If Convert.IsDBNull(vProcessID) Or IsNothing(vProcessID) Then
                'developer guide no. 40
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Process found for Process Code - " & v_sPMNavProcessCode & " : Product Code - " & v_sPMProductCode & " : Effective Date - " & v_dtEffectiveDate, vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessDetails")
                r_lPMNavProcessID = 0
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                r_lPMNavProcessID = CInt(vProcessID)
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetProcessDetails
    '
    ' Description: Gets the details for the supplied Process ID.
    '
    ' ***************************************************************** '
    Public Function GetProcessDetails(ByVal v_lPMNavProcessID As Object, ByRef r_vProcessDetailsArray(,) As Object, ByRef r_vSetKeyArray(,) As Object, ByRef r_vGetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase.Parameters

                .Clear()

                m_lReturn = .Add(sname:="pmnav_process_id", vValue:=CStr(v_lPMNavProcessID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Add(sname:="language_id", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelProcessSQL, ssqlname:=ACSelProcessName, bstoredprocedure:=ACSelProcessStored, vresultarray:=r_vProcessDetailsArray, bkeepnulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Process
            If Not Information.IsArray(r_vProcessDetailsArray) Then
                ' No so log a message and exit.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Nav Process found for ID - " & CStr(v_lPMNavProcessID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Set Keys
            m_lReturn = CType(GetProcessSetKeys(v_lPMNavProcessID:=v_lPMNavProcessID, r_vSetKeyArray:=r_vSetKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Get Keys
            m_lReturn = CType(GetProcessGetKeys(v_lPMNavProcessID:=v_lPMNavProcessID, r_vGetKeyArray:=r_vGetKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetMapDetails
    '
    ' Description: Gets the details for the supplied Map ID.
    '
    ' ***************************************************************** '
    Public Function GetMapDetails(ByVal v_lPMNavMapID As Object, ByRef r_vMapDetailsArray(,) As Object, ByRef r_vMapSetKeyArray(,) As Object, ByRef r_vMapStepsArray(,) As Object, ByRef r_vStepsSetKeyArray(,) As Object, ByRef r_vStepsGetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase.Parameters

                .Clear()

                m_lReturn = .Add(sname:="pmnav_map_id", vValue:=CStr(v_lPMNavMapID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .Add(sname:="language_id", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelMapSQL, ssqlname:=ACSelMapName, bstoredprocedure:=ACSelMapStored, vresultarray:=r_vMapDetailsArray, bkeepnulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Map
            If Not Information.IsArray(r_vMapDetailsArray) Then
                ' No so log a message and exit.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Nav Map found for ID - " & CStr(v_lPMNavMapID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetMapDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Map Set Keys
            m_lReturn = CType(GetMapSetKeys(v_lPMNavMapID:=v_lPMNavMapID, r_vSetKeyArray:=r_vMapSetKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Map Steps
            m_lReturn = CType(GetMapSteps(v_lPMNavMapID:=v_lPMNavMapID, r_vMapStepsArray:=r_vMapStepsArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Map Steps Set Keys
            m_lReturn = CType(GetMapStepsSetKeys(v_lPMNavMapID:=v_lPMNavMapID, r_vSetKeyArray:=r_vStepsSetKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Map Steps Get Keys
            m_lReturn = CType(GetMapStepsGetKeys(v_lPMNavMapID:=v_lPMNavMapID, r_vGetKeyArray:=r_vStepsGetKeyArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMapDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMapDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetProcessSetKeys
    '
    ' Description: Get the Set Keys for the Process.
    '
    ' ***************************************************************** '
    Private Function GetProcessSetKeys(ByVal v_lPMNavProcessID As Object, ByRef r_vSetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase.Parameters

            .Clear()

            m_lReturn = .Add(sname:="pmnav_process_id", vValue:=CStr(v_lPMNavProcessID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelProcessSetKeysSQL, ssqlname:=ACSelProcessSetKeysName, bstoredprocedure:=ACSelProcessSetKeysStored, vresultarray:=r_vSetKeyArray, bkeepnulls:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProcessGetKeys
    '
    ' Description: Get the Get Keys for the Process.
    '
    ' ***************************************************************** '
    Private Function GetProcessGetKeys(ByVal v_lPMNavProcessID As Object, ByRef r_vGetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase.Parameters

            .Clear()

            m_lReturn = .Add(sname:="pmnav_process_id", vValue:=CStr(v_lPMNavProcessID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelProcessGetKeysSQL, ssqlname:=ACSelProcessGetKeysName, bstoredprocedure:=ACSelProcessGetKeysStored, vresultarray:=r_vGetKeyArray, bkeepnulls:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetMapSetKeys
    '
    ' Description: Get the Set Keys for the Map.
    '
    ' ***************************************************************** '
    Private Function GetMapSetKeys(ByVal v_lPMNavMapID As Object, ByRef r_vSetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase.Parameters

            .Clear()

            m_lReturn = .Add(sname:="pmnav_Map_id", vValue:=CStr(v_lPMNavMapID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelMapSetKeysSQL, ssqlname:=ACSelMapSetKeysName, bstoredprocedure:=ACSelMapSetKeysStored, vresultarray:=r_vSetKeyArray, bkeepnulls:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetMapSteps
    '
    ' Description: Get all of the Steps for this Map.
    '
    ' ***************************************************************** '
    Private Function GetMapSteps(ByVal v_lPMNavMapID As Object, ByRef r_vMapStepsArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase.Parameters

            .Clear()

            m_lReturn = .Add(sname:="pmnav_Map_id", vValue:=CStr(v_lPMNavMapID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Add(sname:="language_id", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelMapStepsSQL, ssqlname:=ACSelMapStepsName, bstoredprocedure:=ACSelMapStepsStored, vresultarray:=r_vMapStepsArray, bkeepnulls:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any Steps
        If Not Information.IsArray(r_vMapStepsArray) Then
            ' No so log a message and exit.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No Steps found for Map ID - " & CStr(v_lPMNavMapID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetMapSteps")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetMapStepsSetKeys
    '
    ' Description: Get the Set Keys for all of the Map Steps.
    '
    ' ***************************************************************** '
    Private Function GetMapStepsSetKeys(ByVal v_lPMNavMapID As Object, ByRef r_vSetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase.Parameters

            .Clear()

            m_lReturn = .Add(sname:="pmnav_Map_id", vValue:=CStr(v_lPMNavMapID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelMapStepsSetKeysSQL, ssqlname:=ACSelMapStepsSetKeysName, bstoredprocedure:=ACSelMapStepsSetKeysStored, vresultarray:=r_vSetKeyArray, bkeepnulls:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetMapStepsGetKeys
    '
    ' Description: Get the Get Keys for all of the Map Steps.
    '
    ' ***************************************************************** '
    Private Function GetMapStepsGetKeys(ByVal v_lPMNavMapID As Object, ByRef r_vGetKeyArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase.Parameters

            .Clear()

            m_lReturn = .Add(sname:="pmnav_Map_id", vValue:=CStr(v_lPMNavMapID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, idatatype:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        m_lReturn = m_oDatabase.SQLSelect(sSql:=ACSelMapStepsGetKeysSQL, ssqlname:=ACSelMapStepsGetKeysName, bstoredprocedure:=ACSelMapStepsGetKeysStored, vresultarray:=r_vGetKeyArray, bkeepnulls:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
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
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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
        ' Error.
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

End Class

