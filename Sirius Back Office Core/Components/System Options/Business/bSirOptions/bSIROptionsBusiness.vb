Option Strict Off
Option Explicit On
Imports System.IO
Imports System.Text
'Imports Microsoft.Practices.EnterpriseLibrary.Common
Imports Microsoft.Practices.EnterpriseLibrary.Caching
'Imports Microsoft.Practices.EnterpriseLibrary.Common.Configuration
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database

    Private m_bCloseDatabase As Boolean

    Private m_vOptions(,) As Object

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_sOption10 As String = ""
    Private m_oSIRDOCAPI As Object

    Private m_bManageSalvageSet As Boolean

    Public Shared iCache As ICacheManager
    Private m_sCachePath As String
    Private m_sCacheFilename As String

    Const sSystemOption As String = "systemoptions"
    Public Property SystemOptionValue As String

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    'TINNY18072000 return "A" for Agency and "U" for Underwriting
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            Return "U"
        End Get
    End Property

    'JMK 18/10/2001 also returns "A" for Agency and "U" for Underwriting
    ' SJP   WARNING - This function should not be used as intended
    Public ReadOnly Property UnderwritingOptionTwo() As String
        Get
            Return "U"
        End Get
    End Property

    ' SD 25/12/2002 Manage Salvage hidden option
    Public ReadOnly Property ManageSalvageSet() As Boolean
        Get
            Return m_bManageSalvageSet
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim vValue As Byte

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

            If iSourceID = 0 Then
                m_iSourceID = 1
            End If

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password			
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'SD 25/11/2002
            'Get hidden option for Manage Salvage
            m_lReturn = CType(bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTManageSalvage, v_vBranch:=gPMConstants.SIRBCHHeadOffice, r_vUnderwriting:=CStr(vValue)), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getproductoptionvalue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTManageSalvage, vApp:=ACApp, vClass:=ACClass, vMethod:="ManageSalvageSet (Property Get)")
            End If

            m_bManageSalvageSet = vValue = 1

            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception

            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_sCachePath.ToString.Substring(m_sCachePath.Length - 1)) <> "\" Then
                m_sCachePath += "\"
            End If

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeySystemOptionCacheFileName, r_sSettingValue:=m_sCacheFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
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
            Me.disposedValue = True
            If disposing Then
                If m_oSIRDOCAPI IsNot Nothing Then
                    m_oSIRDOCAPI.Dispose()
                    m_oSIRDOCAPI = Nothing
                End If
                m_vOptions = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
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

            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If

            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If

            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the system options
    '
    ' ***************************************************************** '
    Public Function GetDetails() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Multi company still needs sorting - for now let's assume that source is the
            'company...
            m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, vResultArray:=m_vOptions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(m_vOptions) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'Quick - store option 10 for documaster
            For iTemp As Integer = m_vOptions.GetLowerBound(1) To m_vOptions.GetUpperBound(1)
                If CStr(m_vOptions(0, iTemp)) = "10" Then
                    m_sOption10 = CStr(m_vOptions(1, iTemp))
                    Exit For
                End If
            Next iTemp

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the system options
    '
    ' ***************************************************************** '
    Public Function GetNext(ByRef vOptions(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Array.Copy(m_vOptions, vOptions, m_vOptions.GetUpperBound(1))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Gets the system options
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef vOptions(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vOptions = vOptions

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description:      Gets the system options
    '
    ' Change History:   RWH(23/04/2001) Put transaction around delete & update
    '                   to prevent losing all exisiting data if something goes
    '                   wrong after delete.
    ' RAM20021217   : 1. Made changes to reflect DME's changes to support 5 Level of document folders
    '                 2. Ref. NRMA Project Changes. Sirius Process No. 189
    ' ************************************************************************ '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim bTransactionStarted As Boolean
        Dim sPolicyDesc As String = ""
        Dim iSourceID As Integer
        Dim vSourceArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First we delete the existing data

            m_lReturn = m_oDatabase.SQLBeginTrans()

            bTransactionStarted = True

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Multi company still needs sorting - for now let's assume that source is the
            'company...
            m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteDetailsSQL, sSQLName:=ACDeleteDetailsName, bStoredProcedure:=ACDeleteDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            'Now we write out the new data

            For lTemp As Integer = m_vOptions.GetLowerBound(1) To m_vOptions.GetUpperBound(1)
                m_oDatabase.Parameters.Clear()

                'Multi company still needs sorting - for now let's assume that source is the
                'company...
                m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(m_iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                    Return result
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=m_vOptions(0, lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                    Return result
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="value", vValue:=m_vOptions(1, lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                    Return result
                End If

                'SSL Update 15032001 - Start
                'Description : Adding another parameter for the description field

                m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=m_vOptions(2, lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'SSL - End
                '        Debug.Print m_vOptions(0, ltemp) & " " & m_vOptions(1, ltemp)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertDetailsSQL, sSQLName:=ACInsertDetailsName, bStoredProcedure:=ACInsertDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                    Return result
                End If

            Next lTemp

            m_lReturn = m_oDatabase.SQLCommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            bTransactionStarted = False

            'Now let's worry about updating DocuMaster...
            If m_sOption10 = "1" Then
                'Already set up
                Return result
            End If

            m_sOption10 = ""

            'get the new value for option 10
            For lTemp As Integer = m_vOptions.GetLowerBound(1) To m_vOptions.GetUpperBound(1)
                If CDbl(m_vOptions(0, lTemp)) = 10 Then
                    m_sOption10 = CStr(m_vOptions(1, lTemp))
                    Exit For
                End If
            Next lTemp

            If m_sOption10 <> "1" Then
                'Not set up
                Return result
            End If

            'Get all source details
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllSourceSQL, sSQLName:=ACGetAllSourceName, bStoredProcedure:=ACGetAllSourceStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSourceArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vSourceArray) Then
                Return result
            End If

            For lSourceLoop As Integer = vSourceArray.GetLowerBound(1) To vSourceArray.GetUpperBound(1)

                'Set to the current source ID

                iSourceID = vSourceArray(0, lSourceLoop)

                m_lReturn = CType(UpdateDocumaster(iSourceID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not (m_oSIRDOCAPI Is Nothing) Then

                    ' Terminate FileMaster Component
                    m_oSIRDOCAPI.Dispose()

                    ' Release FileMaster Reference
                    m_oSIRDOCAPI = Nothing

                End If

            Next lSourceLoop

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            If bTransactionStarted Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOption (Public)
    '
    ' Description: Gets a specific system option
    ' Edit History  :
    ' RAM20040304   : Code changes relates to Caching of System Options
    ' ***************************************************************** '
    Public Function GetOption(ByRef iOptionNumber As Integer, ByRef sValue As String) As Integer
        Return GetOption(iOptionNumber:=iOptionNumber, sValue:=sValue, v_iSourceID:=0)
    End Function

    Public Function GetOption(ByRef iOptionNumber As Integer, ByRef sValue As String, ByRef v_iSourceID As Integer) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        'Dim oCache As Hashtable
        Dim sKey As String = ""
        Dim vValue As Object = Nothing
        Dim iSourceID As Integer
        'Dim vKeyArray As Object

        Dim sContent(1) As String
        sContent(0) = ""
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040304 : Code changes related to Caching - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Multi company still needs sorting - for now let's assume that source is the
            'company...
            If v_iSourceID = 0 Then
                iSourceID = m_iSourceID
            Else
                iSourceID = v_iSourceID
            End If

            ' Create key for the input parameters
            ' eg. KEY_00001_00001 :  means : Source ID 1 Option Number 1
            sKey = "KEY_SYSTEM_OPTION_" & String.Format(iSourceID, "00000") & "_" & String.Format(iOptionNumber, "00000")

            ' Create the Cache Object

            'oCache = New Hashtable
            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                sValue = Convert.ToString(iCache.GetData(sKey))
                'vValue = oCache.Item(sKey)
            Else
                ' Not in the CACHE, therefore we need to hit the database to get the value
                If Object.Equals(vValue, Nothing) Then

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(iOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOneOptionSQL, sSQLName:=ACGetOneOptionName, bStoredProcedure:=ACGetOneOptionStored, lNumberRecords:=0, vResultArray:=vArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Do we have any records ?
                    If Not Informations.IsArray(vArray) Then
                        ' No Records, return PMFalse
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    End If

                    ' Return the value
                    sValue = CStr(vArray(0, 0))

                    ' Add them to the Cache
                    'oCache.Add(sKey, sValue)
                    If Not File.Exists(m_sCachePath + m_sCacheFilename) Then
                        Dim fileIO As FileStream
                        fileIO = File.Create(m_sCachePath + m_sCacheFilename)
                        fileIO.Close()
                        File.WriteAllLines(m_sCachePath + m_sCacheFilename, sContent)
                    End If

                    ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                    ' Sirius Cache Controller
                    If Not iCache Is Nothing Then
                        iCache.Add(sKey, sValue, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
                    End If
                    'vKeyArray = oCache.Item("SIRIUS_CACHE_KEYS")

                    'If Object.Equals(vKeyArray, Nothing) Then
                    '	ReDim vKeyArray(0)
                    'Else

                    '	ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
                    'End If

                    'vKeyArray(vKeyArray.GetUpperBound(0)) = sKey
                    ' Remove the existing keys first
                    ' oCache.Remove("SIRIUS_CACHE_KEYS")
                    ' Add the updated one
                    'oCache.Add("SIRIUS_CACHE_KEYS", vKeyArray)

                    'vArray = Nothing
                Else
                    sValue = CStr(vValue)
                End If
            End If
            SystemOptionValue = sValue
            ' Clear the Cache object
            'oCache = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetOptionForAllSources (Public)
    '
    ' Description: Gets a specific system option for each source
    'CT created to get option regardless of source
    ' ***************************************************************** '
    Public Function GetOptionForAllSources(ByRef iOptionNumber As Integer, ByRef vValues(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(iOptionNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetOptionAllSourcesSQL, sSQLName:=ACGetOptionAllSourcesName, bStoredProcedure:=ACGetOptionAllSourcesStored, lNumberRecords:=0, vResultArray:=vValues)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vValues) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set vValues = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOptionForAllSources Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOptionForAllSources", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetDocCodes
    '
    ' Description: Gets all the document template codes
    '
    ' History: 14/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetDocCodes(ByRef r_vDocCodes As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the sql
            sSQL = "SELECT document_template_id, code, description " &
                   "FROM document_template " &
                   "ORDER BY document_template_id"

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDocCodes", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check we have some results returned
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Assign the return value

            r_vDocCodes = vResultArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocCodes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocCodes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetUserTables
    '
    ' Description: Gets all the user tables
    '
    ' History: 15/11/2000 DC - Created.
    '
    ' ***************************************************************** '

    Public Function GetUserTables(ByRef r_vUserTables As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the sql
            sSQL = "SELECT gis_user_def_header_id, code, description " &
                   "FROM gis_user_def_header " &
                   "ORDER BY gis_user_def_header_id"

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUserTables", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check we have some results returned
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Assign the return value

            r_vUserTables = vResultArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserTables Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserTables", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name : GetArchitectureComboDetails
    '
    ' Desc : get details back to display on combo box from architecture
    '
    ' Hist : 01/06/2001 Tinny - Created
    '*************************************************************************
    Public Overloads Function GetArchitectureComboDetails(ByVal v_sTableName As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT " & v_sTableName & "_id, code, description FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ORDER BY code"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Data For Combo Box", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetArchitectureComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetArchitectureComboDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Overloads Function GetArchitectureComboDetails(ByVal v_sTableName As String, ByVal sWhereClause As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT " & v_sTableName & "_id, code, description FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE " & sWhereClause & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ORDER BY code"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Data For Combo Box", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetArchitectureComboDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetArchitectureComboDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name : GetPeriodDetails
    '
    ' Desc : get details back to display on combo box from architecture
    '
    ' Hist : 01/06/2001 Tinny - Created
    '*************************************************************************
    Public Function GetPeriodDetails(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT period_id , period_name FROM period" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE company_id = " & CStr(m_iSourceID) & " AND sub_branch_id = 1" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ORDER BY period_name"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Data For Combo Box", bStoredProcedure:=False, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPeriodDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPeriodDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserGroups
    '
    ' Description: Gets all the user groups
    '
    ' History: 25/11/2002 SD - Created.
    ' ***************************************************************** '
    Public Function GetUserGroups(ByRef r_vUserGroups As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserGroupSQL, sSQLName:=ACGetUserGroupName, bStoredProcedure:=True, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check we have some results returned
            If Not Informations.IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            '    ReDim vNewArray(3, 0 To (UBound(vArray, 2)) + 1)
            '    'Make the first item a blank (for populating combobox)
            '    vNewArray(0, 0) = 0
            '    vNewArray(1, 0) = 0
            '    vNewArray(2, 0) = ""
            '    vNewArray(3, 0) = ""
            '
            '    'We only want the ID, code and caption. If caption absent, use description
            '    For lCounter = 0 To UBound(vArray, 2)
            '        vNewArray(0, lCounter + 1) = lCounter + 1
            '        vNewArray(1, lCounter + 1) = CLng(vArray(iID, lCounter))
            '        vNewArray(2, lCounter + 1) = Trim(vArray(iCode, lCounter))
            '        If IsNull(vArray(iCaption, lCounter)) Or Trim(vArray(iCaption, lCounter) = "") Then
            '            vNewArray(3, lCounter + 1) = Trim(vArray(iDesc, lCounter))
            '        Else
            '            vNewArray(3, lCounter + 1) = Trim(vArray(iCaption, lCounter))
            '        End If
            '    Next lCounter
            '
            '    ' Assign the return value
            '    r_vUserGroups = vNewArray

            r_vUserGroups = vArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserGroups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetFirstLevelNodes(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                sSQL = "SELECT system_option_configuration_group_id, name from system_option_configuration_group " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE parent_id is NULL " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "order by display_order"
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetFirstLevelNodes", bStoredProcedure:=False, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertIntoNodeGroupArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertIntoNodeGroupArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetSecondLevelNodes(ByVal v_lId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                sSQL = "SELECT system_option_configuration_group_id, name from system_option_configuration_group " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE parent_id = " & CStr(v_lId) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "order by display_order"
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetSecondLevelNodes", bStoredProcedure:=False, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertIntoNodeGroupArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertIntoNodeGroupArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSystemOptionConfiguration
    '
    ' Description:
    '
    ' History: 03/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetSystemOptionConfiguration(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                '        .Parameters.Clear
                '
                '        m_lReturn = .Parameters.Add( _
                ''              sName:="system_option_configuration_group_id", _
                ''              vValue:=v_lId, _
                ''              idirection:=PMParamInput, _
                ''              iDataType:=PMLong)
                '
                '        If (m_lReturn& <> PMTrue) Then
                '            GetSystemOptionConfiguration = PMFalse
                '            Exit Function
                '        End If

                m_lReturn = .SQLSelect(sSQL:=ACGetSystemOptionConfigurationSQL, sSQLName:=ACGetSystemOptionConfigurationName, bStoredProcedure:=ACGetSystemOptionConfigurationStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionConfiguration Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionConfiguration", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadSystemOptionData
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function LoadSystemOptionData(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""

            With m_oDatabase

                sSQL = "SELECT option_number, value from system_options " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE branch_id = " & CStr(m_iSourceID) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "order by option_number"
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="LoadSystemOptionData", bStoredProcedure:=False, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSystemOptionData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name : UpdateDocumaster
    '
    ' Desc : Create folders in Documaster to match Sirius database
    '
    ' Hist : 29/01/03 - DN Created

    Public Function UpdateSystemOptionData(ByVal v_vUpdateArray(,) As Object, ByVal v_bInitialiseDocumaster As Boolean) As Integer

        Dim result As Integer = 0
        Dim bPCoption As Integer
        Dim bClaimWpFieldsUpdateRequired As Boolean 'MKW 27/10/03 PN6023

        'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)
        Dim bUpdateExistingPCResolvedNames As Boolean
        Const ACOptionValue As String = "5064"
        Const ACOptionValue5096 As String = "5148"
        Const ACOptionDefault As String = "1"
        'Const ACUpdateExistingPCResolvedNames = 5064 'Need to be checked
        'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)

        'DC130605 PN21731
        Dim vMultiBranchArray(,) As Object = Nothing
        Dim bMultiBranch As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACOptionNumber As Integer = 0
            Const ACValue As Integer = 1
            Const ACUpdateFlag As Integer = 2
            ' Const ACUpdateArraySize As Integer = 2

            Dim sSQL As New StringBuilder

            If Not Informations.IsArray(v_vUpdateArray) Then
                Return result
            End If

            'DC130605 PN21731 -start -check if multi branch system
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetMultiBranchSQL, sSQLName:=ACGetMultiBranchName, bStoredProcedure:=ACGetMultiBranchStored, vResultArray:=vMultiBranchArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vMultiBranchArray) Then
                ReDim vMultiBranchArray(0, 0)

                vMultiBranchArray(0, 0) = 0
            End If

            bMultiBranch = False


            If CInt(vMultiBranchArray(0, 0)) = 1 Then
                bMultiBranch = True
            End If
            'DC130605 PN21731 -end
            Dim sUniqueId As String = GetUniqueID()
            sSQL = New StringBuilder("BEGIN" & Strings.ChrW(13) & Strings.ChrW(10) & "SET NOCOUNT ON " & Strings.ChrW(13) & Strings.ChrW(10))
            bClaimWpFieldsUpdateRequired = False
            For i As Integer = 0 To v_vUpdateArray.GetUpperBound(1)

                If CBool(v_vUpdateArray(ACUpdateFlag, i)) Then
                    sSQL.Append("UPDATE system_options SET value = '")
                    'DC130605 PN21731 only be branch specific if multi branch
                    'v_vUpdateArray(ACValue, i) & "' WHERE option_number = " & _
                    ''v_vUpdateArray(ACOptionNumber, i) & " AND branch_id = " & m_iSourceID & ";" & vbCrLf


                    sSQL.Append(CStr(v_vUpdateArray(ACValue, i)) & "',UserID= " & m_iUserID.ToString & ",UniqueId='" & sUniqueId.ToString & "' WHERE option_number = " & CStr(v_vUpdateArray(ACOptionNumber, i)) & Strings.ChrW(13) & Strings.ChrW(10)) 'PN23556
                    '                    as where there were multiple sql commands there was no space!

                    If bMultiBranch Then
                        sSQL.Append(" AND branch_id = " & m_iSourceID & ";" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                    'DC130605 -end
                Else


                    sSQL.Append("INSERT INTO system_options (branch_id, option_number, value,UserID,UniqueId) " &
                                "VALUES (" & CStr(m_iSourceID) & "," & CStr(v_vUpdateArray(ACOptionNumber, i)) & ",'" &
                                CStr(v_vUpdateArray(ACValue, i)) & "'," & m_iUserID.ToString & ",'" & sUniqueId.ToString & "');" & Strings.ChrW(13) & Strings.ChrW(10))
                End If

                '' If claim Reserve Gross is checked then update Product is_Gross_Claim_Payment_Amount = 1 for active products
                If v_vUpdateArray(ACOptionNumber, i).ToString().Trim() = "5239" AndAlso v_vUpdateArray(ACValue, i).ToString.Trim() = "1" Then
                    m_lReturn = m_oDatabase.SQLAction(sSQL:="Update Product Set is_Gross_Claim_Payment_Amount = 1 WHERE is_deleted = 0",
                                                      sSQLName:="UpdateProductOptionis_Gross_Claim_Payment_Amount",
                                                      bStoredProcedure:=False)
                End If


                'MKW 27/10/03 PN6023 START

                If CStr(v_vUpdateArray(ACOptionNumber, i)) = "2003" Or CStr(v_vUpdateArray(ACOptionNumber, i)) = "2004" Or CStr(v_vUpdateArray(ACOptionNumber, i)) = "2005" Or CStr(v_vUpdateArray(ACOptionNumber, i)) = "2006" Or CStr(v_vUpdateArray(ACOptionNumber, i)) = "2007" Then
                    bClaimWpFieldsUpdateRequired = True
                End If
                'MKW 27/10/03 PN6023 END

                'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)
                'Defect 5649 Sumit.Kumar need to carry value for option 5064 
                If CStr(v_vUpdateArray(ACOptionNumber, i)) = ACOptionValue Then

                    If CStr(v_vUpdateArray(ACValue, i)) = ACOptionDefault Then
                        bPCoption = 1
                        bUpdateExistingPCResolvedNames = True
                    Else
                        bPCoption = 0
                    End If
                    'Defect 5649 Sumit.Kumar need to reset option 5064 if 5063 not selected
                ElseIf CStr(v_vUpdateArray(ACOptionNumber, i)) = ACOptionValue5096 Then
                    If CStr(v_vUpdateArray(ACValue, i)) = ACOptionNumber Then
                        m_lReturn = m_oDatabase.SQLAction(sSQL:="UPDATE system_options SET value = '0' WHERE option_number=5064", sSQLName:="Reset 5064 System Option", bStoredProcedure:=False)
                    End If
                End If
                'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)
            Next i

            sSQL.Append(" SET NOCOUNT OFF " & Strings.ChrW(13) & Strings.ChrW(10) & "END")

            With m_oDatabase

                m_lReturn = .SQLAction(sSQL:=sSQL.ToString(), sSQLName:="UpdateSystemOptionData", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            'MKW 27/10/03 PN6023 START
            If bClaimWpFieldsUpdateRequired And UnderwritingOrAgency = "A" Then

                'Update Wp Fields Entries for Claims UDF Change.
                With m_oDatabase

                    .Parameters.Clear()

                    m_lReturn = .SQLAction(sSQL:=ACUpdateClaimsWpFieldsSQL, sSQLName:=ACUpdateClaimsWpFieldsName, bStoredProcedure:=ACUpdateClaimsWpFieldsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End With

            End If
            'MKW 27/10/03 PN6023 END

            'Start (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)
            If bUpdateExistingPCResolvedNames Then
                'Update all existing PC Resolved Names
                With m_oDatabase
                    .Parameters.Clear()
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Update_Client", vValue:=CStr(bPCoption), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = .SQLAction(sSQL:=ACUpdateExistingPCResolvedNamesSQL, sSQLName:=ACUpdateExistingPCResolvedNamesName, bStoredProcedure:=ACUpdateExistingPCResolvedNamesFieldsStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End With
            End If
            'End (Girija chokkalingam) - (Tech Spec - WR38 - Personal Client Resolved Name.doc) - (5.3.1)

            If v_bInitialiseDocumaster Then
                m_lReturn = InitialiseDocumaster()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseDocumaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSystemOptionData")

                    ' DD 12/08/2004 - Reset documaster option if cannot be initialised
                    m_lReturn = m_oDatabase.SQLAction(sSQL:="UPDATE system_options SET value = '0' WHERE option_number=10" &
                                " AND branch_id = " & CStr(m_iSourceID) & ";" & Strings.ChrW(13) & Strings.ChrW(10), sSQLName:="Reset Documaster System Option", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Return a documaster specific error
                    Return gPMConstants.PMEReturnCode.PMDocumasterError
                End If
            End If
            'Clear the cache on change of system option
            m_lReturn = ClearCache()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSystemOptionData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: InitialiseDocumaster
    '
    ' Description:
    '
    ' History: 19/12/2002 sj - Created.
    '
    'RAM20021217   : 1. Made changes to reflect DME's changes to support 5 Level of document folders
    '                 2. Ref. NRMA Project Changes. Sirius Process No. 189
    ' ***************************************************************** '
    Private Function InitialiseDocumaster() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iSourceID As Integer
        Dim vSourceArray(,) As Object = Nothing

        'DN 29/01/03 - Make system option finally multi company
        'Get all source details
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllSourceSQL, sSQLName:=ACGetAllSourceName, bStoredProcedure:=ACGetAllSourceStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vSourceArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do we have any records ?
        If Not Informations.IsArray(vSourceArray) Then
            Return result
        End If


        For lSourceLoop As Integer = vSourceArray.GetLowerBound(1) To vSourceArray.GetUpperBound(1)

            'Set to the current source ID

            iSourceID = vSourceArray(0, lSourceLoop)

            m_lReturn = CType(UpdateDocumaster(iSourceID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Terminate the DME API object
            If Not (m_oSIRDOCAPI Is Nothing) Then

                ' Terminate FileMaster Component
                m_oSIRDOCAPI.Dispose()

                ' Release FileMaster Reference
                m_oSIRDOCAPI = Nothing

            End If

        Next lSourceLoop

        Return result

    End Function

    Public Function GetFirstLevelNodes(ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                sSQL = "SELECT system_option_configuration_group_id, name from system_option_configuration_group " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE parent_id is NULL " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "order by display_order"
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetFirstLevelNodes", bStoredProcedure:=False, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertIntoNodeGroupArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertIntoNodeGroupArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetSecondLevelNodes(ByVal v_lId As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQL As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                sSQL = "SELECT system_option_configuration_group_id, name from system_option_configuration_group " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE parent_id = " & CStr(v_lId) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "order by display_order"
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetSecondLevelNodes", bStoredProcedure:=False, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertIntoNodeGroupArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertIntoNodeGroupArray", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSystemOptionConfiguration
    '
    ' Description:
    '
    ' History: 03/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function GetSystemOptionConfiguration(ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                '        .Parameters.Clear
                '
                '        m_lReturn = .Parameters.Add( _
                ''              sName:="system_option_configuration_group_id", _
                ''              vValue:=v_lId, _
                ''              idirection:=PMParamInput, _
                ''              iDataType:=PMLong)
                '
                '        If (m_lReturn& <> PMTrue) Then
                '            GetSystemOptionConfiguration = PMFalse
                '            Exit Function
                '        End If

                m_lReturn = .SQLSelect(sSQL:=ACGetSystemOptionConfigurationSQL, sSQLName:=ACGetSystemOptionConfigurationName, bStoredProcedure:=ACGetSystemOptionConfigurationStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOptionConfiguration Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionConfiguration", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadSystemOptionData
    '
    ' Description:
    '
    ' History: 04/12/2002 sj - Created.
    '
    ' ***************************************************************** '
    Public Function LoadSystemOptionData(ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSQL As String = ""

            With m_oDatabase

                sSQL = "SELECT option_number, value from system_options " & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE branch_id = " & CStr(m_iSourceID) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "order by option_number"
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="LoadSystemOptionData", bStoredProcedure:=False, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadSystemOptionData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadSystemOptionData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    '*************************************************************************
    ' Name : UpdateDocumaster
    '
    ' Desc : Create folders in Documaster to match Sirius database
    '
    ' Hist : 29/01/03 - DN Created

    '*************************************************************************
    Public Function UpdateDocumaster(ByRef iSourceID As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Dim sPolicyDesc As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'First let's get the parties...
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllPartiesSQL, sSQLName:=ACGetAllPartiesName, bStoredProcedure:=ACGetAllPartiesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vArray) Then
                Return result
            End If

            If m_oSIRDOCAPI Is Nothing Then

                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSIRDOCAPI, v_sClassName:="bSIRDOCAPI.Form", v_sCallingAppName:=ToSafeString(m_sCallingAppName),
                                                                       v_sUsername:=ToSafeString(m_sUsername), v_sPassword:=ToSafeString(m_sPassword), v_iUserID:=ToSafeInteger(m_iUserID), v_iSourceID:=iSourceID,
                                                                       v_iLanguageID:=ToSafeInteger(m_iLanguageID), v_iCurrencyID:=ToSafeInteger(m_iCurrencyID), v_iLogLevel:=ToSafeInteger(m_iLogLevel), v_oDatabase:=CType(m_oDatabase, dPMDAO.Database)), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the DOC API object", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If


            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                m_lReturn = m_oSIRDOCAPI.ProcessIndex(lMode:=1, iSourceID:=iSourceID, lPartyId:=CInt(vArray(0, lTemp)), sPartyName:=CStr(vArray(1, lTemp)).Trim(), lInsuranceFolderId:=0, sInsuranceFileRef:="", lClaimId:=0, sClaimRef:="")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Process Index of Client", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            Next lTemp

            'Now let's get the insurance files...
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Multi company still needs sorting - for now let's assume that source is the
            'company...
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllPoliciesSQL, sSQLName:=ACGetAllPoliciesName, bStoredProcedure:=ACGetAllPoliciesStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vArray) Then
                Return result
            End If

            ' RFC09/10/01 - ProcessIndex method does NOT have an InsuranceFileID parmam, it has been changed to InsuranceFolderID
            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                sPolicyDesc = CStr(vArray(3, lTemp)).Trim() & "   " & CStr(vArray(4, lTemp)).Trim()

                If sPolicyDesc.Length > 70 Then
                    sPolicyDesc = sPolicyDesc.Substring(0, 70)
                End If

                m_lReturn = m_oSIRDOCAPI.ProcessIndex(lMode:=1, iSourceID:=iSourceID, lPartyId:=CInt(vArray(0, lTemp)), sPartyName:=CStr(vArray(1, lTemp)).Trim(), lInsuranceFolderId:=CInt(vArray(2, lTemp)), sInsuranceFileRef:=ToSafeString(sPolicyDesc), lClaimId:=0, sClaimRef:="")

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Process Index of Policy", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            Next lTemp

            'Now let's get the claims...
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(iSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllClaimsSQL, sSQLName:=ACGetAllClaimsName, bStoredProcedure:=ACGetAllClaimsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vArray) Then
                Return result
            End If

            For lTemp As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                sPolicyDesc = CStr(vArray(3, lTemp)).Trim() & "   " & CStr(vArray(4, lTemp)).Trim()
                If sPolicyDesc.Length > 70 Then
                    sPolicyDesc = sPolicyDesc.Substring(0, 70)
                End If

                m_lReturn = m_oSIRDOCAPI.ProcessIndex(lMode:=1, iSourceID:=iSourceID, lPartyId:=CInt(vArray(0, lTemp)), sPartyName:=CStr(vArray(1, lTemp)).Trim(), lInsuranceFolderId:=0, sInsuranceFileRef:="", lClaimId:=CInt(vArray(2, lTemp)), sClaimRef:=ToSafeString(sPolicyDesc))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Process Index of Claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumaster", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDocumaster Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumaster", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNumberingScheme (Public)
    '
    ' Description: Gets all case type numbering scheme
    '
    ' ***************************************************************** '
    Public Function GetNumberingSchemes(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllCaseNumberingSchemeSQL, sSQLName:=ACGetAllCaseNumberingSchemeName, bStoredProcedure:=ACGetAllCaseNumberingSchemeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNumberingSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberingSchemes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGISScreens (Public)
    '
    ' Description: Gets all case GIS Screen
    '
    ' ***************************************************************** '
    Public Function GetGISScreens(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sGISDataModelTypeCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sGISDataModelTypeCode = "CASE"

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_type_code", vValue:=sGISDataModelTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllCaseGISScreenSQL, sSQLName:=ACGetAllCaseGISScreenName, bStoredProcedure:=ACGetAllCaseGISScreenStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(r_vResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISScreens Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISScreens", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CheckMultipleQuoteVersionRecords(ByRef r_bMultipleVersions As Boolean) As Integer


        Dim result As Integer = 0
        Dim vMultipleVersions(,) As Object = Nothing


        Try
            result = gPMConstants.PMEReturnCode.PMTrue


            Dim sSQL As New StringBuilder
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACMultipleQuoteVersionSQL, sSQLName:=ACMultipleQuoteVersionName, bStoredProcedure:=ACMultipleQuoteVersionStored, vResultArray:=vMultipleVersions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                Return result
            End If

            If Informations.IsArray(vMultipleVersions) Then
                If vMultipleVersions(0, 0) > 0 Then
                    r_bMultipleVersions = True
                End If
            End If

            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMultipleQuoteVersionRecords Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMultipleQuoteVersionRecords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try


    End Function

    Public Function ClearCache() As Integer

        Dim result As Integer = 0
        Dim sCachePath As String = String.Empty
        Dim sCacheFilename As String = String.Empty
        Dim iReturm As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iReturm = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                  v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

            If iReturm <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (m_sCachePath.ToString.Substring(m_sCachePath.Length - 1)) <> "\" Then
                sCachePath += "\"
            End If

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                       v_sSettingName:=gPMConstants.PMRegKeySystemOptionCacheFileName, r_sSettingValue:=sCacheFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Clear the Cache on Change of System Options
            iCache.Flush()
            'Delete the File( FileDependency for System Options Caching)
            File.Delete(sCachePath & sCacheFilename)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ClearCache", r_lFunctionReturn:=result, excep:=excep)

            Return result

        End Try
    End Function

    Public Function RefreshCCMTemplates(ByVal bRefreshAll As Boolean) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Dim oCCMDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
        nResult = oCCMDocumentProdBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=(m_oDatabase))
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = oCCMDocumentProdBusiness.RefreshCCMTemplates(bRefreshAll, "")
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function
    ''' <summary>
    ''' ValidateSharepointOnlineUrl
    ''' </summary>
    ''' <param name="sSharepointSite"></param>
    ''' <param name="sSharepointLibrary"></param>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="sResponse"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateSharepointOnlineUrl(ByVal sSharepointSite As String,
                                                ByVal sSharepointLibrary As String,
                                                ByVal sUserName As String, sPassword As String,
                                                ByRef sResponse As String,
                                                ByVal sAppClientId As String,
                                                ByVal sSharepointTenantId As String) As String
        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            Dim sPurePath As String = ""
            If sPurePath = "" Then
                gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPurePath)
                If sPurePath.Contains("\") Then
                    sPurePath &= "Pure\Application\"
                Else
                    sPurePath &= "\Pure\Application\"
                End If
            End If
            Dim process As New Process()
            process.StartInfo.FileName = sPurePath & "SharePointOnlineValidate.EXE"
            process.StartInfo.Arguments = "URL=" + sSharepointSite + " LIBRARY=" + sSharepointLibrary + " USERID=" + sUserName + "  PASSWORD=" + sPassword + " APPCLIENTID=" + sAppClientId + " SHAREPOINTTENANTID=" + sSharepointTenantId
            process.StartInfo.UseShellExecute = False
            process.StartInfo.RedirectStandardOutput = True
            process.StartInfo.CreateNoWindow = True
            process.Start()

            Dim reader As StreamReader = process.StandardOutput
            Dim output As String = reader.ReadToEnd()

            process.WaitForExit()
            process.Close()

            sResponse = output
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ValidateSharepointOnlineUrl", r_lFunctionReturn:=nResult, excep:=ex)
            nResult = PMEReturnCode.PMFalse
        End Try
        Return nResult
    End Function

    Public Function GetTaxGroupForClaims(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTaxGroupForClaimsSQL, sSQLName:=ACGetTaxGroupForClaimsName, bStoredProcedure:=ACGetTaxGroupForClaimsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaxGroupForClaims Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNumberingSchemes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function

    Public Function GetALLCurrency(ByRef r_vResultArray(,) As Object, Optional ByRef r_BranchBaseCurrencyID As Integer = 0) As Integer



        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetALLCurrencySQL, sSQLName:=ACGetALLCurrencyName, bStoredProcedure:=ACGetTALLCurrencyNameStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

            End If

            If Not Informations.IsArray(r_vResultArray) Then

                result = gPMConstants.PMEReturnCode.PMNotFound

            End If

            m_lReturn = GetBranchBaseCurrency(m_iSourceID, m_oDatabase, r_BranchBaseCurrencyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result



        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ACGetALLCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetALLCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try



    End Function

End Class
