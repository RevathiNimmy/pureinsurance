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
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lPartyCnt As Integer

    Private Const ACClass As String = "bSIREmail.Business"

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            ' Set UserID

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
    ' Name: GetAddressContactDetails
    '
    ' Description: Get contact details for party via address.
    '
    ' ***************************************************************** '
    Public Function GetAddressContactDetails(ByRef vPartyCnt As Object, ByRef vContacts(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get all the contacts for the party
            ' PSL 29/05/2003 Issue 4361
            ' This join doesn't work, and it should be an SP anyway
            '        sSQL = "SELECT c.contact_cnt, c.area_code, c.number, " & _
            ''                "c.extension, ct.code, aut.code " & _
            ''                "FROM contact c, contact_type ct, contact_address_usage cau, " & _
            ''                "party_address_usage pau, address_usage_type aut " & _
            ''                "WHERE pau.party_cnt = " & CLng(vPartyCnt) & " " & _
            ''                "AND pau.address_cnt = cau.address_cnt " & _
            ''                "AND cau.contact_cnt = c.contact_cnt " & _
            ''                "AND pau.address_usage_type_id = aut.address_usage_type_id " & _
            ''                "AND c.contact_type_id = ct.contact_type_id"
            '
            '        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL$, _
            ''                                        sSQLName:="GETADDRESSCONTACTS", _
            ''                                        bStoredProcedure:=False, _
            ''                                        vResultArray:=vContacts)

            m_oDatabase.Parameters.Clear()


            'Developer Guide 101
            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=vPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Developer Guide 39
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_email_select", sSQLName:="spu_email_select", bStoredProcedure:=True, vResultArray:=vContacts)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAddressContactDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddressContactDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBranchCode
    ' Description: Get the code for the given Branch
    ' History: 10/01/2007 A.Robinson - Created
    ' ***************************************************************** '
    Public Function GetBranchCode(ByVal v_iSourceID As Integer, ByRef r_sBranchCode As String) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetBranchCode"
        Try


            Const SQL_SELECTSOURCE_SQL As String = "spu_PM_Select_Source"
            Const SQL_SELECTSOURCE_SP As Boolean = True
            Const SQL_SELECTSOURCE_NAME As String = "Select Branch code"

            Const SQL_FIELD_CODE As Integer = 1

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vResults(,) As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()
                'Developer Guide 101
                .Parameters.Add(sName:="source_id", vValue:=v_iSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                lReturn = .SQLSelect(sSQL:=SQL_SELECTSOURCE_SQL, sSQLName:=SQL_SELECTSOURCE_NAME, bStoredProcedure:=SQL_SELECTSOURCE_SP, vResultArray:=vResults)

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    r_sBranchCode = gPMFunctions.ToSafeString(vResults(SQL_FIELD_CODE, 0)).Trim()
                ElseIf lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                Else
                    gPMFunctions.RaiseError(ACMethod, "Call to stored procedure " & SQL_SELECTSOURCE_SQL & " failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDatabaseName
    ' Description: Get the database name from the registry
    ' History: 10/01/2007 A.Robinson - Created
    ' ***************************************************************** '
    Public Function GetDatabaseName(ByRef r_sDatabaseName As String) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetDatabaseName"
        Try

            Const REGISTRY_KEY_DATABASE As String = "Database"

            Dim sDSN, sRegValue As String

            result = gPMConstants.PMEReturnCode.PMTrue

            sDSN = gPMConstants.PMSiriusSolutionsDSN

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSubKey:="\Databases\" & sDSN, v_sSettingName:=REGISTRY_KEY_DATABASE, r_sSettingValue:=sRegValue), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACMethod, "Failed to get the database name from the registry", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_sDatabaseName = sRegValue.Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSystemName
    ' Description: Get the system name from the PMSystem table
    ' History: 10/01/2007 A.Robinson - Created
    ' ***************************************************************** '
    Public Function GetSystemName(ByRef r_sSystemName As String) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "GetSysemName"
        Try

            Const PMSYSTEM_PROG_ID As String = "bPMSystem.Business"

            Dim oPMSystem As bPMSystem.Business
            Dim lReturn As gPMConstants.PMEReturnCode

            Dim sProductCode As String = ""
            Dim iSystemID, iProductID As Integer
            Dim sSystemName As String = ""
            Dim iDefaultSourceID, iHomeCountryID, iCurrencyID, iLanguageID, iLicenceLimit As Integer
            Dim sLicenceKey As String = ""
            Dim iLogLevel, iPoolSize As Integer
            Dim vTimestamp As Object

            result = gPMConstants.PMEReturnCode.PMTrue
            oPMSystem = New bPMSystem.Business
            lReturn = CType(oPMSystem.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACMethod, "Failed to create business object " & PMSYSTEM_PROG_ID, gPMConstants.PMELogLevel.PMLogError)
            End If

            sProductCode = gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusArchitecture).ToUpper()


            lReturn = oPMSystem.GetValidSystem(sProductCode, iSystemID, iProductID, sSystemName, iDefaultSourceID, iHomeCountryID, iCurrencyID, iLanguageID, iLicenceLimit, sLicenceKey, iLogLevel, iPoolSize, vTimestamp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(ACMethod, "Call to GetValidSystem failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                r_sSystemName = CStr(gPMFunctions.GetSystemNameNoSID(sSystemName))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
