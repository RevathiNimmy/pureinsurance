Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private m_oDatabase As dPMDAO.Database

    Private Const ACClass As String = "Business"

    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceid As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_iSourceID = iSourceid
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

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


    '################################################################################
    ' Method: GetBranchDetails
    ' Description: get branch (source) code and description
    '
    ' History: RDC 12112003 created
    '################################################################################
    Public Function GetBranchDetails(ByVal iSourceid As Integer, ByRef sBranchCode As String, ByRef sBranchDesc As String) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""
        Dim vBranch As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT code, description FROM source WHERE source_id = " & iSourceid

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchDetails", bStoredProcedure:=False, lNumberRecords:=lNumRecs)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            vBranch = m_oDatabase.Records.Fields()("code")


            If Convert.IsDBNull(vBranch) Or IsNothing(vBranch) Then
                ' didn't find it
                Return result
            Else
                sBranchCode = vBranch.Trim()
                sBranchDesc = m_oDatabase.Records.Fields()("description").Trim()
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBranchDetails failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: GetAllBranches
    ' Description: get all branches set up
    '
    ' History: DC220404 PN11171 -process all branches via scheduler or if core
    '################################################################################
    Public Function GetAllBranches(ByRef vBranches(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT source_id FROM source"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllBranches", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vBranches)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllBranches failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllBranches", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: MultiBranchCheck
    ' Description: check for multi branch
    '
    ' History: DC220404 PN11171 -process all branches via scheduler or if core
    '################################################################################
    Public Function MultiBranchCheck(ByRef lMulti As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vMulti(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT value FROM hidden_options WHERE option_number = 16 and branch_id = 1"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="MultiBranchCheck", bStoredProcedure:=False, lNumberRecords:=1, vResultArray:=vMulti)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lMulti = 0
                Return result
            End If

            If Information.IsArray(vMulti) Then


                lMulti = CInt(vMulti(0, 0))

            Else

                lMulti = 0

            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MultiBranchCheck failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MultiBranchCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: GetMediaTypes
    ' Description: get all media types for drop-down list
    '
    ' History: RDC 12112003 created
    '################################################################################
    Public Function GetMediaTypes(ByRef vMediaTypes(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lNumRecs As Integer
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "SELECT mediatype_id, description FROM MediaType ORDER BY description"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskMaps", bStoredProcedure:=False, lNumberRecords:=lNumRecs, vResultArray:=vMediaTypes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vMediaTypes) Then
                ' didn't find them
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMediaTypes failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMediaTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: GetExportData
    ' Description: get the cash list details for export to CSV
    '
    ' History: RDC 12112003 created
    '                       modified to exclude MediaType WHERE clause
    '
    ' ECK 22/12/03 Modified to link batch_id with cashlistitem not cashlist
    ' DC220404 PN11171 -process all branches via scheduler or if core -added sourceid
    ' RG 14/09/04  - Changed to stored procedure, which now also handles reversals
    '################################################################################
    Public Function GetExportData(ByVal sMediaDesc As String, ByVal iSourceid As Integer, ByRef vExportData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = "exec spu_ACT_get_cashlist_export_data " & iSourceid

            'DC200204 PN10444 get ALL records not just first 500
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetExportData", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vExportData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Information.IsArray(vExportData) Then
                ' didn't find any
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExportData failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExportData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '################################################################################
    ' Method: SetExportFlag
    ' Description: enables the export flag on the cashlist records, so that items
    '              will only be exported once.
    '
    ' History: RDC 09012003 created
    ' DC220404 PN11171 -process all branches if run via scheduler or core
    '################################################################################
    Public Function SetExportFlag(ByVal iSourceid As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sSQL = ""
            sSQL = sSQL & "UPDATE cli" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "SET cli.is_exported = 1" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "FROM cashlistitem cli" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "JOIN cashlist cl" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "    ON cl.cashlist_id = cli.cashlist_id" & Strings.Chr(13) & Strings.Chr(10)
            sSQL = sSQL & "WHERE cl.company_id = " & CStr(iSourceid) & Strings.Chr(13) & Strings.Chr(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetExportFlag", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUserName:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetExportFlag failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetExportFlag", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
End Class
