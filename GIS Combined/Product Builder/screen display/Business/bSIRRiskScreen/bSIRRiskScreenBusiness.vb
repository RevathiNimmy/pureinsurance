Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    '******************************************************************************
    ' Class Name:   Business
    ' Date:         07/05/1999
    ' Description:  Creatable Business class which contains all the methods,
    '               Business rules required to manipulate User Defined Data
    '               Maintenance.
    ' Edit History:
    ' CLG 18/11/2003 : CD3303  : Combo Id Shown in Child Grid
    ' CJB 08/03/2005 : PN19313 : Added new function DeletePolicyVersion initially called from iPMUScreenControl for
    '                  new renewal what-if quotes only, if the quote has not been saved and the user is exiting,
    '                  delete the policy version.
    ' VB    27/04/2005 : 354 Standard Wording Control Enhancements
    ' RKS   29/04/2005 : 354 Standard Wording Control Enhancements
    ' RKS   06/05/2005 : 354 Standard Wording Control Enhancements
    '                   (Edited DeleteUnusedEditedStandardWording)
    ' CJB 20/09/2005 : PN24176 Created new function GetRiskCodeFromID
    ' RAM20040511    : Code changes related to caching of GIS Screen Details
    ' RAM20040521    : Code changes related to use the standard bPMFunc to fetch the UW / Agency flag, rather than
    '                   the direct creation of bSIROptions
    '******************************************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    'CT 23/10/00 - cater for SBO risk screen as well as underwriting (start)
    ' Database Class (Private)
    'Private m_oArchDatabase As dPMDAO.Database

    ' Close Architecture Database Flag (Private)
    Private m_bCloseArchDatabase As Boolean
    'What system we are in
    Private m_sSystem As String = ""
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' Process Mode Properties
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lProductId As Integer
    Private m_lScreenId As Integer
    Private m_sScreen As String = ""
    Private m_bEvent As Boolean
    Private m_oGIS As Object
    Private m_bSubScreen As Boolean
    Private m_lTransactionType As Integer
    Private m_lRiskFolderCnt As Integer



    Private m_oPMLookup As BPMLOOKUP.Business
    Private m_oGISLookup As bGISUserDefLookup.Business
    'Caching
    Public Shared iCache As ICacheManager

    Dim sFunctionCalls As String = ""


    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property RiskId() As Integer
        Get
            Return m_lRiskId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskId = Value
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

    Public Property ProductId() As Integer
        Get
            Return m_lProductId
        End Get
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

    Public Property SubScreen() As Boolean
        Get
            Return m_bSubScreen
        End Get
        Set(ByVal Value As Boolean)
            m_bSubScreen = Value
        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get
            Return m_bEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bEvent = Value
        End Set
    End Property

    Public ReadOnly Property TransactionType() As Integer
        Get
            If m_lTransactionType = 0 Then
                m_lReturn = CType(GetTransactionType(), gPMConstants.PMEReturnCode)
            End If
            Return m_lTransactionType
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get
            Return m_sSystem
        End Get
    End Property
    Public WriteOnly Property RiskFolderCnt() As Integer
        Set(ByVal Value As Integer)
            m_lRiskFolderCnt = Value
        End Set
    End Property

    '******************************************************************************
    ' Name:         Initialise (Standard Method)
    ' Description:  Entry point for any initialisation code for this object.
    ' Edit History:
    '******************************************************************************
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            Dim sSystem As String = ""
            result = gPMConstants.PMEReturnCode.PMTrue

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


            'm_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseArchDatabase, r_oCheckedDatabase:=m_oArchDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If


            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040521 : Call the bPMFunctions standard method to fetch the UW / Agecny Option
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'm_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=sUsername, v_sPassword:=sPassword, v_iUserID:=iUserID, v_iMainSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_iCurrencyID:=iCurrencyID, v_iLogLevel:=iLogLevel, v_sCallingAppName:=sCallingAppName, r_vUnderwriting:=sSystem), gPMConstants.PMEReturnCode)

            ' set the module level variable
            m_sSystem = "U"

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         Terminate (Standard Method)
    ' Description:  Entry point for any termination code for this object.
    ' Edit History  :
    ' RAM20040521   : Code added to clean up the Lookup Components
    '******************************************************************************
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oPMLookup = Nothing
                If m_oGISLookup Is Nothing Then
                Else
                    m_oGISLookup = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    '******************************************************************************
    ' Name:         SetProcessModes (Standard Method)
    ' Description:  Set the optional process modes.
    '******************************************************************************
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes" & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetScreenDetails (Public)
    ' Description:  Gets the data dictionary and screen layout
    ' History:      17/12/2004 TR - Removed gis_data_model_type_id parameter from
    '               ACGetDataDictionarySQL call as it was not used. Also added
    '               use of AddParamterLite functions
    '******************************************************************************
    'NIIT - Replaced with the Migrated code 1144 


    Public Function GetScreenDetails(ByRef r_vDataDictionary As Object,
                                         ByRef r_vScreenHeader(,) As Object,
                                         ByRef r_vScreenDetails(,) As Object,
                                         ByRef r_vChildScreenDetails(,) As Object) As Integer


        Dim result As Integer = 0
        Dim sKey As String = ""

        Dim vDataDictionary As Object = Nothing
        Dim vScreenHeader(,) As Object = Nothing
        Dim vScreenDetails(,) As Object = Nothing
        Dim vChildScreenDetails(,) As Object = Nothing
        Dim sCachePath As String = ""
        Dim sCacheFilename As String = ""

        Dim sContent(1) As String
        sContent(0) = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vScreenHeader = Nothing
            r_vScreenDetails = Nothing
            r_vChildScreenDetails = Nothing

            'Caching

            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception
            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(sCachePath, 1) <> "\" Then
                sCachePath += "\"
            End If

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeySystemOptionCacheFileName, r_sSettingValue:=sCacheFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get the screen details for each data model type
            For lTemp As Integer = GISDataModelType.GISDMTypeRisk To GISDataModelType.GISDMTypeRisk 'TBD
                r_vDataDictionary(lTemp) = Nothing


                ' Create key for the input parameters
                ' eg. KEY_KEY_DATA_MODEL_TYPE_00001_GIS_SCREEN_00026_DATA_DICTIONARY :
                '   means : The data is the data Dictionary for data Model Type 1 and GIS Screen ID 26
                sKey = "KEY_DATA_MODEL_TYPE_" & lTemp.ToString("00000") & "_GIS_SCREEN_" & m_lScreenId.ToString("00000") & "_DATA_DICTIONARY"

                ' Clear it before we get it from cache
                vDataDictionary = Nothing

                ' Get from the Cache by the Key, if available
                ' Not in the CACHE, therefore we need to hit the database to get the value
                If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                    vDataDictionary = iCache.GetData(sKey)
                End If

                If Informations.IsNothing(vDataDictionary) Then

                    ' The data is not cached so, use the usual way to fetch the data

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_type_id", vValue:=CStr(lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Cleanup Memory
                        iCache.Remove(sKey)
                        Return result
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDataDictionarySQL, sSQLName:=ACGetDataDictionaryName, bStoredProcedure:=ACGetDataDictionaryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vDataDictionary(lTemp))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Cleanup Memory
                        iCache.Remove(sKey)
                        Return result
                    End If

                    ' Do we have any records ?
                    If lTemp = GISDataModelType.GISDMTypeRisk And Not Informations.IsArray(r_vDataDictionary) Then
                        ' No Records, return PMNotFound
                        result = gPMConstants.PMEReturnCode.PMNotFound

                        ' Cleanup Memory
                        iCache.Remove(sKey)
                        Return result
                    End If

                    ' We got the values, so put them in the CACHE
                    ' Add them to the Cache
                    If Not FileExists(sCachePath + sCacheFilename) Then
                        Dim fileIO As FileStream
                        fileIO = File.Create(sCachePath + sCacheFilename)
                        fileIO.Close()
                        File.WriteAllLines(sCachePath + sCacheFilename, sContent)
                    End If
                    ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                    ' Sirius Cache Controller
                    If Not iCache Is Nothing Then
                        iCache.Add(sKey, r_vDataDictionary, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sCachePath + sCacheFilename))

                    End If

                Else
                    ' The value is already in the cache, so set the value, we got it from the cache
                    ' and assign it to the return value
                    r_vDataDictionary = vDataDictionary
                End If


            Next


            'Now get the screen header
            ' Create key for the input parameters

            sKey = "KEY_GIS_SCREEN_" & m_lScreenId.ToString("00000") & "_HEADER_DETAILS"
            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                vScreenHeader = iCache.GetData(sKey)
            End If
            ' Not in the CACHE, therefore we need to hit the database to get the value
            If Informations.IsNothing(vScreenHeader) Then

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Add the screen
                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Cleanup Memory
                    iCache.Remove(sKey)
                    Return result
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=PBDatabaseConsts.ACGetAllScreenHeaderSQL, sSQLName:=PBDatabaseConsts.ACGetAllScreenHeaderName, bStoredProcedure:=PBDatabaseConsts.ACGetAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScreenHeader, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Cleanup Memory
                    iCache.Remove(sKey)
                    Return result
                End If

                '' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                '' Sirius Cache Controller
                If Not FileExists(sCachePath + sCacheFilename) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sCachePath + sCacheFilename)
                    fileIO.Close()
                    File.WriteAllLines(sCachePath + sCacheFilename, sContent)
                End If

                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller
                If Not iCache Is Nothing Then
                    iCache.Add(sKey, r_vScreenHeader, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sCachePath + sCacheFilename))
                End If

            Else
                ' The value is already in the cache, so set the value, we got it from the cache
                ' and assign it to the return value
                r_vScreenHeader = vScreenHeader
            End If

            'Now get the screen details

            ' Create key for the input parameters
            ' eg. KEY_GIS_SCREEN_00026_DETAILS :  means : The data is the DETAILS for the GIS Screen ID 26
            sKey = "KEY_GIS_SCREEN_" & m_lScreenId.ToString("00000") & "_DETAILS"

            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                vScreenDetails = iCache.GetData(sKey)
            End If

            ' Not in the CACHE, therefore we need to hit the database to get the value

            'developer guide no. 160
            If Informations.IsNothing(vScreenDetails) Then

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Add the screen
                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Cleanup Memory
                    iCache.Remove(sKey)
                    Return result
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllScreenDetailsSQL, sSQLName:=ACGetAllScreenDetailsName, bStoredProcedure:=ACGetAllScreenDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScreenDetails, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Cleanup Memory
                    iCache.Remove(sKey)
                    Return result
                End If

                If Not FileExists(sCachePath + sCacheFilename) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sCachePath + sCacheFilename)
                    fileIO.Close()
                    File.WriteAllLines(sCachePath + sCacheFilename, sContent)
                End If

                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller
                If Not iCache Is Nothing Then
                    iCache.Add(sKey, r_vScreenDetails, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sCachePath + sCacheFilename))
                    'vKeyArray = oCache.Item("SIRIUS_CACHE_KEYS")
                End If
            Else
                ' The value is already in the cache, so set the value, we got it from the cache
                ' and assign it to the return value
                r_vScreenDetails = vScreenDetails
            End If

            'Now get the child screen details
            ' Create key for the input parameters
            sKey = "KEY_GIS_SCREEN_" & m_lScreenId.ToString("00000") & "_CHILD_SCREEN_DETAILS"

            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                vChildScreenDetails = iCache.GetData(sKey)
            End If

            ' Not in the CACHE, therefore we need to hit the database to get the value
            If Informations.IsNothing(vChildScreenDetails) Then

                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Add the screen
                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Cleanup Memory
                    iCache.Remove(sKey)
                    Return result
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllChildScreenDetailsSQL,
                                                  sSQLName:=ACGetAllChildScreenDetailsName,
                                                  bStoredProcedure:=ACGetAllChildScreenDetailsStored,
                                                  lNumberRecords:=gPMConstants.PMAllRecords,
                                                  vResultArray:=r_vChildScreenDetails, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Cleanup Memory
                    iCache.Remove(sKey)
                    Return result
                End If

                If Not FileExists(sCachePath + sCacheFilename) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sCachePath + sCacheFilename)
                    fileIO.Close()
                    File.WriteAllLines(sCachePath + sCacheFilename, sContent)
                End If

                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller
                If Not iCache Is Nothing Then
                    iCache.Add(sKey, r_vChildScreenDetails, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sCachePath + sCacheFilename))
                End If

            Else
                ' The value is already in the cache, so set the value, we got it from the cache
                ' and assign it to the return value
                r_vChildScreenDetails = vChildScreenDetails
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetScreenDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScreenDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    '******************************************************************************
    ' Name:         Update (Public)
    ' Description:  Redoes the data
    '******************************************************************************
    Public Function Update() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    '******************************************************************************
    ' Name:         GetAddress
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '******************************************************************************
    Public Function GetAddress(ByRef lAddressCnt As Integer, ByRef vAddressArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            Return PBGetAddressFromAddressCnt.GetAddressFromAddressCnt(m_oDatabase, CStr(lAddressCnt), vAddressArray)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddress " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetParty
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '******************************************************************************
    Public Function GetParty(ByRef lPartyCnt As Integer, ByRef lPartyTypeId As Integer, ByRef vPartyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""

            If lPartyCnt = 0 Then
                sSQL = sSQL & "SELECT 0, '', pt.code" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM party_type pt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE pt.party_type_id = " & CStr(lPartyTypeId) & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "SELECT p.party_cnt, p.name, pt.code" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM party p, party_type pt" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE party_cnt = " & CStr(lPartyCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND p.party_type_id = pt.party_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetPartyName, bStoredProcedure:=ACGetPartyStored, vResultArray:=vPartyArray)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetPolicy
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '******************************************************************************
    Public Function GetPolicy(ByRef lInsuranceFileCnt As Integer, ByRef lProductId As Integer, ByRef vPolicyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""

            If lInsuranceFileCnt = 0 Then
                sSQL = sSQL & "SELECT 0, '', p.code" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM Product p" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE p.Product_id = " & CStr(lProductId) & Strings.ChrW(13) & Strings.ChrW(10)
            Else
                sSQL = sSQL & "SELECT i.insurance_file_cnt, i.insurance_ref, p.code" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "FROM insurance_file i, product p" & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "WHERE i.insurance_file_cnt = " & CStr(lInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
                sSQL = sSQL & "AND i.product_id = p.product_id" & Strings.ChrW(13) & Strings.ChrW(10)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetPolicyName, bStoredProcedure:=ACGetPolicyStored, vResultArray:=vPolicyArray)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicy " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetStandardWording
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '******************************************************************************
    'developer guide no. 101
    Public Function GetStandardWording(ByRef lPolicyLinkId As Integer,
                                       ByRef sDataModel As String,
                                       ByRef lGISPropertyID As Integer,
                                       ByRef lGISObjectID As Integer,
                                       ByRef vStandardWordingArray(,) As Object,
                                       ByRef sKeyName As String,
                                       ByRef sKeyValue As String) As Integer


        Dim result As Integer = 0
        Dim lPolicyBinderID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 12
            vStandardWordingArray = Nothing

            m_lReturn = CType(GetBinder(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lPolicyBinderID:=lPolicyBinderID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Data_Model", sDataModel + "_", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)
            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", lGISObjectID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If sKeyName <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_name", sKeyName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)
            End If

            Dim sQuery As String
            Dim vRArray(,) As Object = Nothing
            If sKeyName <> "" Then
                sQuery = "select table_name from GIS_Object where gis_object_id={gis_object_id} and object_name = Replace(REPLACE(" + "'" + "{parent_key_name}" + "'," + "'" + "{Data_Model}" + "'" + ",''),'_id','')"
            Else
                sQuery = "select table_name from GIS_Object where gis_object_id={gis_object_id}"
            End If
            Dim sSpecialKeyName As String = ""
            m_lReturn = m_oDatabase.SQLSelect(sQuery, sSQLName:="Get Actual Column Name", bStoredProcedure:=False, vResultArray:=vRArray)
            If vRArray IsNot Nothing Then
                sSpecialKeyName = vRArray(0, 0) + "_id"
            End If

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_property_id", lGISPropertyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", lGISObjectID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If sSpecialKeyName <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_name", sKeyName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)

                bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_value", sKeyValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            If sKeyName <> "" Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStandardWordingChildSQL, sSQLName:=ACGetStandardWordingName, bStoredProcedure:=ACGetStandardWordingStored, vResultArray:=vStandardWordingArray)
            Else
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStandardWordingSQL, sSQLName:=ACGetStandardWordingName, bStoredProcedure:=ACGetStandardWordingStored, vResultArray:=vStandardWordingArray)
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStandardWording Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStandardWording", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetSumInsured
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '******************************************************************************
    'developer guide no. 101
    Public Function GetSumInsured(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lSumInsuredType As Integer, ByRef vSumInsuredArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lPolicyBinderID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vSumInsuredArray = Nothing

            m_lReturn = CType(GetBinder(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lPolicyBinderID:=lPolicyBinderID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_type", lSumInsuredType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSumInsuredSQL, sSQLName:=ACGetSumInsuredName, bStoredProcedure:=ACGetSumInsuredStored, vResultArray:=vSumInsuredArray)

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSumInsured " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSumInsured", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetGISPolicyLinkDetails
    ' Description:  Returns the claim_peril_id and gis_policy_link_id for a claim
    ' History:      AMB 09/01/03 - Created
    '******************************************************************************
    Public Function GetGISPolicyLinkDetails(ByRef lClaimID As Integer, ByRef r_lGisPolicyLinkId As Integer, ByRef r_lClaimPerilID As Integer) As Integer

        Dim result As Integer = 0
        Dim vGPLResultArray As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGISPolicyLinkDetailsSQL, sSQLName:=ACGetGISPolicyLinkDetailsName, bStoredProcedure:=ACGetGISPolicyLinkDetailsStored, vResultArray:=vGPLResultArray)

            ' HG210104 - Changed "is nothing" test to isarray
            If Informations.IsArray(vGPLResultArray) Then

                r_lGisPolicyLinkId = CInt(vGPLResultArray(0))

                r_lClaimPerilID = CInt(vGPLResultArray(1))
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISPolicyLinkDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISPolicyLinkDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Public Function GetOldPolicyLinkId_Stateless(ByRef lRiskId As Integer, ByRef lPolicyLinkId As Integer) As Integer
        'developer guide no. 71
        Dim vData As Object = Nothing
        Dim sSQL As String = "select r.insurance_file_cnt,l.gis_policy_link_id from insurance_file_risk_link r"
        sSQL = sSQL & " left join gis_policy_link l"
        sSQL = sSQL & " on r.risk_cnt=l.risk_id"
        sSQL = sSQL & " where r.risk_cnt=" & CStr(lRiskId)

        m_lReturn = m_oDatabase.SQLSelect(sSQL, "Get bits", False, , vData)


        lPolicyLinkId = CInt(vData(1, 0))
        Return m_lReturn
    End Function

    '******************************************************************************
    ' Name:         GetBinder_Stateless
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '               03/09/2004 RAW - Resilience : renamed procedure by appending
    '               '_Stateless'. The original named procedure still exists but is
    '               basically a wrapper to this function
    '******************************************************************************
    Public Function GetBinder_Stateless(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lPolicyBinderID As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_link_id", lPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBinderSQL, sSQLName:=ACGetBinderName, bStoredProcedure:=ACGetBinderStored, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                lPolicyBinderID = CInt(vArray(0, 0))
                vArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBinder_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBinder_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         DeleteRiskCancelledOnAdd_Stateless

    ' Description:
    ' When a risk is created some of the back office records must be written to
    ' hang the risk details off. If during the add the risk is cancelled these
    ' records are left behind. This is (now) causing a problem so this procedure
    ' must be called when a risk which is being added is cancelled.

    ' History:      11/12/2002 CLG - Created.
    '               03/09/2004 RAW - Resilience : renamed procedure by appending
    '               '_Stateless' and replaced module variables with new parameters.
    '               The original named procedure still exists but is basically a
    '               wrapper to this function
    '******************************************************************************
    Public Function DeleteRiskCancelledOnAdd_Stateless(ByVal v_lRiskId As Object) As Integer

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************
        Dim result As Integer = 0
        Try

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", v_lRiskId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRiskCancelledOnAddSQL, sSQLName:=ACDeleteRiskCancelledOnAddName, bStoredProcedure:=ACDeleteRiskCancelledOnAddStored)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRiskCancelledOnAdd_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRiskCancelledOnAdd_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    ''' <summary>
    ''' _Stateless' and replaced module variables with new parameters.
    '''  The original named procedure still exists but is basically a
    '''  wrapper to this function
    ''' </summary>
    ''' <param name="vRiskArray"></param>
    ''' <param name="vRiskTypeArray"></param>
    ''' <param name="r_lRiskId"></param>
    ''' <param name="v_lProductId"></param>
    ''' <param name="v_iSourceID"></param>
    ''' <param name="v_lScreenId"></param>
    ''' <param name="v_lRiskTypeId"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRisk_Stateless(ByRef vRiskArray(,) As Object, ByRef vRiskTypeArray(,) As Object,
                                      ByRef r_lRiskId As Integer, ByVal v_lProductId As Integer,
                                      ByVal v_iSourceID As Integer, ByVal v_lScreenId As Integer,
                                      ByVal v_lRiskTypeId As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                      ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim nResult As Integer
        Dim oProductArray As Object
        Dim oRiskFolderArray As Object
        Dim vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0) As Object
        Dim nOriginalRiskCnt As Integer = 0
        Dim oInsuranceFileCoverDate As Object = Nothing

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************

        'New logic for endorsements
        'If we don't find a risk record for the passed cnt (which would then be 0)
        'we proceed as normal, creating a risk folder record, a risk record, and
        ' an insurance file risk link record.
        'If we do find a record and the status flag is "C" we can exit
        'If we do find a record and the status flag is "U" we need to delete the
        'existing insurance file risk link record, create a new risk record, and
        'create a new insurance file risk link record
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            vRiskArray = Nothing
            vRiskTypeArray = Nothing
            oProductArray = Nothing
            oRiskFolderArray = Nothing
            vInsuranceFileRiskLinkArray = Nothing

            If m_sSystem = "U" Then 'for U/W SBO risk screen control

                'TR - Replaced with AddParamterLite functions
                bPMAddParameter.AddParameterLite(m_oDatabase, "product_id", v_lProductId,
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong, True)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductDetailsSQL, sSQLName:=ACGetProductDetailsName,
                                                  bStoredProcedure:=ACGetProductDetailsStored,
                                                  vResultArray:=oProductArray, bKeepNulls:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_sSystem = "U" Then 'CT 23/10/00 cater for U/W and SBO risk screen control

                'TR - Replaced with AddParamterLite functions
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_lRiskTypeId,
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong, True)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskTypeDetailsSQL, sSQLName:=ACGetRiskTypeDetailsName,
                                                  bStoredProcedure:=ACGetRiskTypeDetailsStored,
                                                  vResultArray:=vRiskTypeArray, bKeepNulls:=True)
            Else
                'TR - Replaced with AddParamterLite functions
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_group_id", v_lRiskTypeId,
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong, True)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskGroupDetailsSQL,
                                                  sSQLName:=ACGetRiskGroupDetailsName,
                                                  bStoredProcedure:=ACGetRiskGroupDetailsStored,
                                                  vResultArray:=vRiskTypeArray, bKeepNulls:=True)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", r_lRiskId,
                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                             gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailsSQL, sSQLName:=ACGetRiskDetailsName,
                                              bStoredProcedure:=ACGetRiskDetailsStored, vResultArray:=vRiskArray,
                                              bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vRiskArray) Then

                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", m_lInsuranceFileCnt,
                                                 gPMConstants.PMEParameterDirection.PMParamInput,
                                                 gPMConstants.PMEDataType.PMLong, True)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFileSQL, sSQLName:=ACGetInsuranceFileName,
                                                  bStoredProcedure:=ACGetInsuranceFileStored,
                                                  vResultArray:=oInsuranceFileCoverDate, bKeepNulls:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'So we must create the risk record, but first we need to create the risk folder
                ReDim oRiskFolderArray(ACRFInsuranceFolderCnt, 0)

                'Use risk folder cnt if supplied
                If m_lRiskFolderCnt = 0 Then


                    oRiskFolderArray(ACRFRiskFolderCnt, 0) = 0

                    oRiskFolderArray(ACRFRiskFolderId, 0) = 0

                    oRiskFolderArray(ACRFSourceId, 0) = v_iSourceID _
                    ' RAW 03/09/2004 : Resilience (#2) : replaced m_iSourceID with v_iSourceID

                    oRiskFolderArray(ACRFRiskFolderTypeId, 0) = 1

                    oRiskFolderArray(ACRFCode, 0) = ""


                    oRiskFolderArray(ACRFDescription, 0) = vRiskTypeArray(ACRTDescription, 0)

                    oRiskFolderArray(ACRFInsuranceFolderCnt, 0) = v_lInsuranceFolderCnt _
                    ' RAW 03/09/2004 : Resilience (#2) : replaced m_lInsuranceFolderCnt with v_lInsuranceFolderCnt

                    ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                    m_lReturn = CType(UpdateRiskFolder_Stateless(vRiskFolderArray:=oRiskFolderArray),
                                      gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else

                    oRiskFolderArray(ACRFRiskFolderCnt, 0) = m_lRiskFolderCnt
                End If

                ReDim vRiskArray(ACRMax, 0)

                r_lRiskId = 0 ' RAW 03/09/2004 : Resilience (#2) : replaced m_lRiskId with r_lRiskId


                vRiskArray(ACRRiskId, 0) = 0


                vRiskArray(ACRRiskStatusId, 0) = DBNull.Value


                vRiskArray(ACRRiskFolderCnt, 0) = oRiskFolderArray(ACRFRiskFolderCnt, 0)


                vRiskArray(ACRAccumulationId, 0) = DBNull.Value

                vRiskArray(ACRRiskTypeId, 0) = v_lRiskTypeId _
                ' RAW 03/09/2004 : Resilience (#2) : replaced m_lRiskTypeId with v_lRiskTypeId


                vRiskArray(ACRDescription, 0) = vRiskTypeArray(ACRTDescription, 0)

                vRiskArray(ACRSequenceNumber, 0) = 1 'Need real defaults


                vRiskArray(ACRSumInsuredRequested, 0) = DBNull.Value
                If Informations.IsArray(oInsuranceFileCoverDate) Then


                    vRiskArray(ACRInceptionDate, 0) = oInsuranceFileCoverDate(21, 0)
                Else
                    'Just to handle unexpected


                    vRiskArray(ACRInceptionDate, 0) = DBNull.Value
                End If


                vRiskArray(ACRExpiryDate, 0) = DBNull.Value

                vRiskArray(ACRIsNotIndexLinked, 0) = gPMConstants.PMEReturnCode.PMTrue  'Need real defaults
                If m_sSystem = "U" Then 'CT 23/10/00 cater for underwriting and SBO risk screen control


                    vRiskArray(ACRIsAccumulated, 0) = oProductArray(ACPIsAccumulation, 0)
                Else

                    vRiskArray(ACRIsAccumulated, 0) = 0
                End If


                vRiskArray(ACRLapsedReasonId, 0) = DBNull.Value


                vRiskArray(ACRLapsedDate, 0) = DBNull.Value


                vRiskArray(ACRLapsedDescription, 0) = DBNull.Value


                vRiskArray(ACRVarDataRef, 0) = DBNull.Value


                vRiskArray(ACRTotalSumInsured, 0) = DBNull.Value


                vRiskArray(ACRTotalAnnualPremium, 0) = DBNull.Value


                vRiskArray(ACRTotalThisPremium, 0) = DBNull.Value


                vRiskArray(ACRIsRiAtRiskLevel, 0) = vRiskTypeArray(ACRTIsRiAtRiskLevel, 0)


                vRiskArray(ACRIsAutoReinsured, 0) = vRiskTypeArray(ACRTIsAutoReinsured, 0)

                vRiskArray(ACRGISScreenId, 0) = v_lScreenId


                vRiskArray(ACREMLPercentage, 0) = DBNull.Value


                vRiskArray(ACRRiskNumber, 0) = DBNull.Value


                vRiskArray(ACRVariationNumber, 0) = DBNull.Value


                vRiskArray(ACRIsRiskSelected, 0) = DBNull.Value


                vRiskArray(ACRCoverage, 0) = DBNull.Value


                vRiskArray(ACRInsuredItem, 0) = DBNull.Value


                vRiskArray(ACRExtensions, 0) = DBNull.Value

                ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                m_lReturn = CType(UpdateRisk_Stateless(vRiskArray:=vRiskArray, r_lRiskId:=r_lRiskId),
                                  gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                r_lRiskId = CInt(vRiskArray(ACRRiskId, 0))

                ReDim vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0)

                vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0) = v_lInsuranceFileCnt

                vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0) = r_lRiskId

                vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0) = "C"


                vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0) = DBNull.Value


                vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0) = DBNull.Value

                ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                m_lReturn =
                    CType(
                        UpdateInsuranceFileRiskLink_Stateless(vInsuranceFileRiskLinkArray:=vInsuranceFileRiskLinkArray,
                                                              iMode:=gPMConstants.PMEComponentAction.PMAdd),
                        gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else
                'So we've got a record - now if the status is "U" we need to copy it, remove the
                'old insurance file link record and replace it with a new one
                'And if it's not add we update the link record to "C" - just in case

                ReDim vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0)

                vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0) = v_lInsuranceFileCnt

                vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0) = r_lRiskId
                If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                    ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                    m_lReturn =
                        CType(
                            UpdateInsuranceFileRiskLink_Stateless(
                                vInsuranceFileRiskLinkArray:=vInsuranceFileRiskLinkArray,
                                iMode:=gPMConstants.PMEComponentAction.PMView),
                            gPMConstants.PMEReturnCode)
                End If
                ' Only do this if we are not viewing
                If Informations.IsArray(vInsuranceFileRiskLinkArray) Then

                    If _
                        CStr(vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0)) = "U" OrElse
                        vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0) = "R" Then
                        ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                        If (vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0) = "R") Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceFileCnt", m_lInsuranceFileCnt,
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong, True)
                            bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCnt", r_lRiskId,
                                                             gPMConstants.PMEParameterDirection.PMParamInput,
                                                             gPMConstants.PMEDataType.PMLong)

                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTaxDetailsBatchRenewalsSQL,
                                                              sSQLName:=ACDeleteTaxDetailsBatchRenewalsName,
                                                              bStoredProcedure:=ACDeleteTaxDetailsBatchRenewalsStored)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                GetRisk_Stateless = gPMConstants.PMEReturnCode.PMFalse
                                Exit Function
                            End If
                        End If

                        m_lReturn =
                            CType(
                                UpdateInsuranceFileRiskLink_Stateless(
                                    vInsuranceFileRiskLinkArray:=vInsuranceFileRiskLinkArray,
                                    iMode:=gPMConstants.PMEComponentAction.PMDelete),
                                gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        If vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0) = "U" Then
                            nOriginalRiskCnt = r_lRiskId
                        End If
                        vRiskArray(ACRRiskId, 0) = 0
                        r_lRiskId = 0 ' RAW 03/09/2004 : Resilience (#2) : replaced m_lRiskId with r_lRiskId


                        vRiskArray(ACRRiskStatusId, 0) = 4  ' Unquoted

                        vRiskArray(ACRTotalAnnualPremium, 0) = 0

                        vRiskArray(ACRTotalThisPremium, 0) = 0

                        ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                        m_lReturn = CType(UpdateRisk_Stateless(vRiskArray:=vRiskArray, r_lRiskId:=r_lRiskId),
                                          gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                        r_lRiskId = CInt(vRiskArray(ACRRiskId, 0))

                        ReDim vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0)


                        vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0) = v_lInsuranceFileCnt

                        vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0) = r_lRiskId

                        vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0) = "C"

                        vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0) = nOriginalRiskCnt

                        vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0) = 1

                        ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                        m_lReturn =
                            CType(
                                UpdateInsuranceFileRiskLink_Stateless(
                                    vInsuranceFileRiskLinkArray:=vInsuranceFileRiskLinkArray,
                                    iMode:=gPMConstants.PMEComponentAction.PMAdd),
                                gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' update the risk with the previous billed amount
                        m_lReturn = CType(CopyOriginalRiskPremiumThisYearToNewRisk(v_lNewRiskCnt:=r_lRiskId,
                                                                                   v_lOldRiskCnt:=nOriginalRiskCnt),
                                          gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            oProductArray = Nothing

            Return nResult

        Catch excep As System.Exception


            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetGISDataModel
    ' Description:
    ' History:      05/09/2000 Tomo - Created.
    '               18/07/2002 RVH  - Add optional parameter r_lGISDataModelType
    '******************************************************************************
    Public Function GetGISDataModel_Stateless(ByVal v_lScreenId As Integer, ByRef r_lGISDataModelId As Integer, ByRef r_sGISDataModel As String, Optional ByRef r_lGISDataModelType As Integer = -1) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_screen_id", v_lScreenId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGISDataModelSQL, sSQLName:=ACGetGISDataModelName, bStoredProcedure:=ACGetGISDataModelStored, vResultArray:=vArray)

            If Informations.IsArray(vArray) Then

                r_lGISDataModelId = CInt(vArray(0, 0))

                r_sGISDataModel = CStr(vArray(1, 0)).Trim()
                '18/07/2002  RVH  - Return GIS Data Model Type if required...
                If r_lGISDataModelType <> -1 Then

                    If CStr(vArray(2, 0)) = "" Then
                        'If no data model type then do not proceed further as it will fail  PN27045
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="The datamodel linked to the risk screen does not have a TYPE setup (in Lookup Maintenance/GIS Data Model). Screens linked to data models without a type cannot be used within New Business. Aborting...", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISDataModel_Stateless")
                        Return gPMConstants.PMEReturnCode.PMFalse
                    Else

                        r_lGISDataModelType = CInt(vArray(2, 0))
                    End If
                End If
                vArray = Nothing
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISDataModel_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISDataModel_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetObjectKeys
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '******************************************************************************
    Public Function GetObjectKeys(ByRef sObjectName As String, ByVal lDataModelId As Integer, ByRef vArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "object_name", sObjectName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "datamodel_id", lDataModelId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetObjectKeysSQL, sSQLName:=ACGetObjectKeysName, bStoredProcedure:=ACGetObjectKeysStored, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectKeys " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         UpdateStandardWording_Stateless
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '               03/09/2004 RAW  - Resilience : renamed procedure by appending
    '               '_Stateless'. The original named procedure still exists but is
    '               basically a wrapper to this function
    '******************************************************************************
    Public Function UpdateStandardWording_Stateless(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lGISPropertyID As Integer, ByRef lGISObjectID As Integer, ByRef vStandardWordingArray(,) As Object, ByRef sKeyName As String, ByRef sKeyValue As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyBinderID As Integer

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'RAW Resilience - replaced function call with call to stateless equivalent
            m_lReturn = CType(GetBinder_Stateless(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lPolicyBinderID:=lPolicyBinderID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Replaced with AddParamterLite functions

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Data_Model", sDataModel + "_", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", lGISObjectID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If sKeyName <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_name", sKeyName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)
            End If

            Dim sQuery As String
            Dim vRArray(,) As Object = Nothing
            If sKeyName <> "" Then
                sQuery = "select table_name from GIS_Object where gis_object_id={gis_object_id} and object_name = Replace(REPLACE(" + "'" + "{parent_key_name}" + "'," + "'" + "{Data_Model}" + "'" + ",''),'_id','')"
            Else
                sQuery = "select table_name from GIS_Object where gis_object_id={gis_object_id}"
            End If

            Dim sSpecialKeyName As String = ""
            m_lReturn = m_oDatabase.SQLSelect(sQuery, sSQLName:="Get Actual Column Name", bStoredProcedure:=False, vResultArray:=vRArray)
            If vRArray IsNot Nothing Then
                sSpecialKeyName = vRArray(0, 0) + "_id"
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Data_Model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_property_id", lGISPropertyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", lGISObjectID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If sSpecialKeyName <> "" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_name", sKeyName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)

                bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_value", sKeyValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            If sKeyName <> "" Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteStandardWordingChildSQL, sSQLName:=ACDeleteStandardWordingName, bStoredProcedure:=ACDeleteStandardWordingStored)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteStandardWordingSQL, sSQLName:=ACDeleteStandardWordingName, bStoredProcedure:=ACDeleteStandardWordingStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vStandardWordingArray) Then
                Return result
            End If

            For lTemp As Integer = vStandardWordingArray.GetLowerBound(1) To vStandardWordingArray.GetUpperBound(1)
                'TR - Replaced with AddParamterLite functions
                bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

                bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_id", lTemp + 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "document_template_id", vStandardWordingArray(0, lTemp), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "gis_property_id", lGISPropertyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", lGISObjectID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                If sSpecialKeyName <> "" Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_name", sKeyName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName)

                    bPMAddParameter.AddParameterLite(m_oDatabase, "parent_key_value", sKeyValue, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If

                If sKeyName <> "" Then
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertStandardWordingChildSQL, sSQLName:=ACInsertStandardWordingName, bStoredProcedure:=ACInsertStandardWordingStored)
                Else
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertStandardWordingSQL, sSQLName:=ACInsertStandardWordingName, bStoredProcedure:=ACInsertStandardWordingStored)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next lTemp

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStandardWording_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStandardWording_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         UpdateSumInsured
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '               03/09/2004 RAW  - Resilience : renamed procedure by appending
    '               '_Stateless'. The original named procedure still exists but is
    '               basically a wrapper to this function
    '******************************************************************************
    Public Function UpdateSumInsured_Stateless(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lSumInsuredType As Integer, ByRef vSumInsuredArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lPolicyBinderID As Integer
        Dim sTemp As String = ""
        Dim vTemp As Object
        Dim lLbound As Integer

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent
            m_lReturn = CType(GetBinder_Stateless(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lPolicyBinderID:=lPolicyBinderID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_type", lSumInsuredType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSumInsuredSQL, sSQLName:=ACDeleteSumInsuredName, bStoredProcedure:=ACDeleteSumInsuredStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vSumInsuredArray) Then
                Return result
            End If

            ' if the first element in the sum insured array hasnt been populated
            ' (as is the case with the sum insured array at the moment ignore it)

            If CStr(vSumInsuredArray(0, vSumInsuredArray.GetLowerBound(1))) = "" Then
                lLbound = 1
            Else
                lLbound = vSumInsuredArray.GetLowerBound(1)
            End If

            ' the lbound (0) row is always blank for the sum insured control
            For lTemp As Integer = lLbound To vSumInsuredArray.GetUpperBound(1)

                'TR - Replaced with AddParamterLite functions
                bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)

                bPMAddParameter.AddParameterLite(m_oDatabase, "policy_binder_id", lPolicyBinderID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_type_id", lSumInsuredType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_id", lTemp + 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


                If Convert.IsDBNull(vSumInsuredArray(0, lTemp)) Or Informations.IsNothing(vSumInsuredArray(0, lTemp)) Then


                    vTemp = vSumInsuredArray(0, lTemp)
                Else

                    sTemp = CStr(vSumInsuredArray(0, lTemp))

                    m_lReturn = CType(bPMFunc.ValidateSQL(sSQLStatement:=sTemp), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    vTemp = sTemp
                End If
                bPMAddParameter.AddParameterLite(m_oDatabase, "description", vTemp, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


                If Convert.IsDBNull(vSumInsuredArray(1, lTemp)) Or Informations.IsNothing(vSumInsuredArray(1, lTemp)) Then


                    vTemp = vSumInsuredArray(1, lTemp)
                Else

                    sTemp = CStr(vSumInsuredArray(1, lTemp))

                    m_lReturn = CType(bPMFunc.ValidateSQL(sSQLStatement:=sTemp), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    vTemp = sTemp
                End If

                bPMAddParameter.AddParameterLite(m_oDatabase, "reference", vTemp, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vSumInsuredArray(2, lTemp), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                bPMAddParameter.AddParameterLite(m_oDatabase, "date_added", gPMFunctions.ToSafeDate(vSumInsuredArray(3, lTemp), #12/29/1899#), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

                '        If IsEmpty(vSumInsuredArray(4, lTemp)) Then
                '            vSumInsuredArray(4, lTemp) = Null
                '        End If
                '        If vSumInsuredArray(4, lTemp) = "" Then
                '            vSumInsuredArray(4, lTemp) = Null
                '        End If

                bPMAddParameter.AddParameterLite(m_oDatabase, "date_deleted", gPMFunctions.ToSafeDate(vSumInsuredArray(4, lTemp), #12/29/1899#), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

                bPMAddParameter.AddParameterLite(m_oDatabase, "is_valuation_required", vSumInsuredArray(5, lTemp), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                '        If IsEmpty(vSumInsuredArray(6, lTemp)) Then
                '            vSumInsuredArray(6, lTemp) = Null
                '        End If
                '        If vSumInsuredArray(6, lTemp) = "" Then
                '            vSumInsuredArray(6, lTemp) = Null
                '        End If

                bPMAddParameter.AddParameterLite(m_oDatabase, "valuation_date", gPMFunctions.ToSafeDate(vSumInsuredArray(6, lTemp), #12/29/1899#), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)


                If Object.Equals(vSumInsuredArray(7, lTemp), Nothing) Then


                    vSumInsuredArray(7, lTemp) = DBNull.Value
                End If

                If CStr(vSumInsuredArray(7, lTemp)) = "" Then


                    vSumInsuredArray(7, lTemp) = DBNull.Value
                End If
                bPMAddParameter.AddParameterLite(m_oDatabase, "rate", vSumInsuredArray(7, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)


                If CStr(vSumInsuredArray(8, lTemp)) = "" Then


                    vSumInsuredArray(8, lTemp) = DBNull.Value
                End If

                If Object.Equals(vSumInsuredArray(8, lTemp), Nothing) Then


                    vSumInsuredArray(8, lTemp) = DBNull.Value
                End If
                bPMAddParameter.AddParameterLite(m_oDatabase, "premium", vSumInsuredArray(8, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertSumInsuredSQL, sSQLName:=ACInsertSumInsuredName, bStoredProcedure:=ACInsertSumInsuredStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next lTemp

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSumInsured_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSumInsured_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result


            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name:         UpdateRisk
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '               03/09/2004 RAW  - Resilience : renamed procedure by appending
    '               '_Stateless' and replaced module variables with new parameters.
    '               The original named procedure still exists but is basically a
    '               wrapper to this function
    '               17/12/2004 TR   - Replaced with AddParamterLite functions
    '******************************************************************************
    Public Function UpdateRisk_Stateless(ByRef vRiskArray(,) As Object, ByRef r_lRiskId As Integer) As Integer

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If r_lRiskId = 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", r_lRiskId, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", r_lRiskId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", vRiskArray(ACRRiskStatusId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_cnt", vRiskArray(ACRRiskFolderCnt, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' RAW 02/07/2003 : CQ1044 : added <= 0 test

            'developer guide no. IsDBNULL() Check is required to assign value.
            If Informations.IsDBNull(vRiskArray(ACRAccumulationId, 0)) OrElse String.IsNullOrEmpty(vRiskArray(ACRAccumulationId, 0)) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "accumulation_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                If CDbl(vRiskArray(ACRAccumulationId, 0)) <= 0 Then

                    bPMAddParameter.AddParameterLite(m_oDatabase, "accumulation_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    bPMAddParameter.AddParameterLite(m_oDatabase, "accumulation_id", vRiskArray(ACRAccumulationId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If
            End If
            'Broking don't store risk types at the moment
            If m_sSystem = "U" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", vRiskArray(ACRRiskTypeId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "description", vRiskArray(ACRDescription, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_number", vRiskArray(ACRSequenceNumber, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_requested", vRiskArray(ACRSumInsuredRequested, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "inception_date", vRiskArray(ACRInceptionDate, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", vRiskArray(ACRExpiryDate, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_not_index_linked", CInt(vRiskArray(ACRIsNotIndexLinked, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_accumulated", vRiskArray(ACRIsAccumulated, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_reason_id", vRiskArray(ACRLapsedReasonId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_date", vRiskArray(ACRLapsedDate, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_description", vRiskArray(ACRLapsedDescription, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "var_data_ref", vRiskArray(ACRVarDataRef, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "total_sum_insured", vRiskArray(ACRTotalSumInsured, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "total_annual_premium", vRiskArray(ACRTotalAnnualPremium, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "total_this_premium", vRiskArray(ACRTotalThisPremium, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_ri_at_risk_level", vRiskArray(ACRIsRiAtRiskLevel, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_auto_reinsured", vRiskArray(ACRIsAutoReinsured, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_screen_id", vRiskArray(ACRGISScreenId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            Dim dbNumericTemp As Double
            'developer guide no. IsDBNULL() Check is required to assign value.
            If Informations.IsDBNull(vRiskArray(ACREMLPercentage, 0)) Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "eml_percentage", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
            Else
                If Double.TryParse(CStr(vRiskArray(ACREMLPercentage, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "eml_percentage", vRiskArray(ACREMLPercentage, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                Else

                    bPMAddParameter.AddParameterLite(m_oDatabase, "eml_percentage", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                End If
            End If
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_number", vRiskArray(ACRRiskNumber, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "variation_number", vRiskArray(ACRVariationNumber, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_risk_selected", vRiskArray(ACRIsRiskSelected, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "coverage", vRiskArray(ACRCoverage, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "insured_item", vRiskArray(ACRInsuredItem, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "extensions", vRiskArray(ACRExtensions, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            If r_lRiskId = 0 Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskDetailsSQL, sSQLName:=ACInsertRiskDetailsName, bStoredProcedure:=ACInsertRiskDetailsStored)
            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskDetailsSQL, sSQLName:=ACUpdateRiskDetailsName, bStoredProcedure:=ACUpdateRiskDetailsStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_lRiskId = 0 Then
                r_lRiskId = m_oDatabase.Parameters.Item("risk_cnt").Value

                vRiskArray(ACRRiskId, 0) = r_lRiskId
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRisk_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisk_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         UpdateRiskFolder
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '               03/09/2004 RAW  - Resilience : renamed procedure by appending
    '               '_Stateless'
    '               16/12/2004 TR   - Utilised AddParamterLite function
    '******************************************************************************
    Private Function UpdateRiskFolder_Stateless(ByRef vRiskFolderArray(,) As Object) As Integer

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If CDbl(vRiskFolderArray(ACRFRiskFolderCnt, 0)) = 0 Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_cnt", vRiskFolderArray(ACRFRiskFolderCnt, 0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_cnt", vRiskFolderArray(ACRFRiskFolderCnt, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        End If

        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_id", vRiskFolderArray(ACRFRiskFolderId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "source_id", vRiskFolderArray(ACRFSourceId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_type_id", vRiskFolderArray(ACRFRiskFolderTypeId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "code", vRiskFolderArray(ACRFCode, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameterLite(m_oDatabase, "description", vRiskFolderArray(ACRFDescription, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_folder_cnt", vRiskFolderArray(ACRFInsuranceFolderCnt, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


        If CDbl(vRiskFolderArray(ACRFRiskFolderCnt, 0)) = 0 Then
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertRiskFolderDetailsSQL, sSQLName:=ACInsertRiskFolderDetailsName, bStoredProcedure:=ACInsertRiskFolderDetailsStored)
        Else
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskFolderDetailsSQL, sSQLName:=ACUpdateRiskFolderDetailsName, bStoredProcedure:=ACUpdateRiskFolderDetailsStored)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If CDbl(vRiskFolderArray(ACRFRiskFolderCnt, 0)) = 0 Then

            vRiskFolderArray(ACRFRiskFolderCnt, 0) = m_oDatabase.Parameters.Item("risk_folder_cnt").Value
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:         UpdateInsuranceFileRiskLink
    ' Description:
    ' History:      25/08/2000 Tomo - Created.
    '               03/09/2004 RAW  - Resilience : renamed procedure by appending
    '               '_Stateless'
    '******************************************************************************
    Private Function UpdateInsuranceFileRiskLink_Stateless(ByRef vInsuranceFileRiskLinkArray(,) As Object, ByRef iMode As Integer) As Integer

        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        If (iMode = gPMConstants.PMEComponentAction.PMAdd) Or (iMode = gPMConstants.PMEComponentAction.PMEdit) Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "status_flag", vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "original_risk_cnt", vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_manually_changed", vInsuranceFileRiskLinkArray(ACIFRLIsManuallyChanged, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
        End If

        Select Case iMode
            Case gPMConstants.PMEComponentAction.PMAdd
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACInsertInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACInsertInsuranceFileRiskLinkDetailsStored)
            Case gPMConstants.PMEComponentAction.PMEdit
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACUpdateInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACUpdateInsuranceFileRiskLinkDetailsStored)
            Case gPMConstants.PMEComponentAction.PMDelete
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACDeleteInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACDeleteInsuranceFileRiskLinkDetailsStored)
            Case gPMConstants.PMEComponentAction.PMView
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInsuranceFileRiskLinkDetailsSQL, sSQLName:=ACGetInsuranceFileRiskLinkDetailsName, bStoredProcedure:=ACGetInsuranceFileRiskLinkDetailsStored, vResultArray:=vInsuranceFileRiskLinkArray)
        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:         CopyExtraDetails
    ' Description:
    ' History:      24/11/2000 Tomo - Created.
    '               03/09/2004 RAW  - Resilience : renamed procedure by appending
    '               '_Stateless'. The original named procedure still exists but is
    '               basically a wrapper to this function
    '******************************************************************************
    Public Function CopyExtraDetails_Stateless(ByRef sDataModelCode As String, ByRef lNewGISPolicyLinkID As Integer, ByRef lOldGISPolicyLinkId As Integer, ByRef lOldRiskID As Integer, ByRef lNewRiskID As Integer) As Integer

        Dim result As Integer = 0
        Dim lOldPolicyBinderID, lNewPolicyBinderID As Integer
        Dim oFindInsuranceFile As bSIRFindInsurance.Form
        ' *************************************************************************
        ' This function should not refer to any module variables - unless they have
        ' been set from Initialise
        ' *************************************************************************

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oFindInsuranceFile = New bSIRFindInsurance.Form()
            oFindInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageId:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            
            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_model", sDataModelCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMTableName, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "old_policy_link", lOldGISPolicyLinkId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "new_policy_link", lNewGISPolicyLinkID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopySumInsuredSQL, sSQLName:=ACCopySumInsuredName, bStoredProcedure:=ACCopySumInsuredStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Binder IDs
            If GetBinder_Stateless(lOldGISPolicyLinkId, sDataModelCode, lOldPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If GetBinder_Stateless(lNewGISPolicyLinkID, sDataModelCode, lNewPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Call function to duplicate standard wordings
            m_lReturn = oFindInsuranceFile.CopyRiskStandardWordings(v_lOldPolicyBinderId:=lOldPolicyBinderID, v_lNewPolicyBinderId:=lNewPolicyBinderID, v_sDataModelCode:=sDataModelCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "old_risk_cnt", lOldRiskID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "new_risk_cnt", lNewRiskID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskExtrasSQL, sSQLName:=ACCopyRiskExtrasName, bStoredProcedure:=ACCopyRiskExtrasStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyExtraDetails_Stateless Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyExtraDetails_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            oFindInsuranceFile.Dispose()
            oFindInsuranceFile = Nothing
        End Try
    End Function

    '******************************************************************************
    ' Name:         GetTransactionType
    ' Description:
    ' History:      03/07/2001 Tomo - Created.
    '******************************************************************************
    Public Function GetTransactionType() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Replaced with AddParamterLite functions
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeSQL, sSQLName:=ACGetTransactionTypeName, bStoredProcedure:=ACGetTransactionTypeStored, vResultArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vArray) Then

                m_lTransactionType = CInt(vArray(0, 0))
                vArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         BeginTrans (Private)
    ' Description:  Begins a Transaction.
    '******************************************************************************

    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    'Return result
    '
    'End Try
    'End Function

    '******************************************************************************
    ' Name:         CommitTrans (Private)
    ' Description:  Commits a Transaction (Saves changes to DB).
    '******************************************************************************

    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    'Return result
    '
    'End Try
    'End Function

    '******************************************************************************
    ' Name:         RollbackTrans (Private)
    ' Description:  Rollback a Transaction (Undo changes to DB).
    '******************************************************************************

    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    'Return result
    '
    'End Try
    'End Function

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: GetGISUserDefHeaderDetail
    '
    ' Description:
    '
    ' History: 18/11/2003 CLG
    ' CLG 18/11/2003 : CD3303 : Combo Id Shown in Child Grid
    ' RAM20040521    : Use the GetUserDef function to fetch the values
    '                   1. Peformance enhancement
    '                   2. Use caching
    '                   3. Make it generic
    ' ***************************************************************** '
    Public Function GetGISUserDefHeaderDetail(ByVal v_lGISHeaderId As Integer, ByVal v_vValue As Object, ByRef v_vDescription As String) As Integer

        Dim result As Integer = 0
        Try


            Dim vResults As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            v_vDescription = ""

            ' Call the following function to get the details

            'developer guide no. 98
            m_lReturn = CType(GetUserDef(iUserDef:=v_lGISHeaderId, vValue:=v_vValue, vDescription:=vResults), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch GISUserDef Details Description." & Strings.ChrW(13) & Strings.ChrW(10) &
                                   "GIS_User_Def_Header_ID : " & CStr(v_lGISHeaderId) & Strings.ChrW(13) & Strings.ChrW(10) &
                                   "GIS_User_Def_Detail_ID : " & CStr(v_vValue), vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISUserDefHeaderDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


                Return result
            Else
                ' Return the value
                v_vDescription = vResults
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISUserDefHeaderDetail Failed" & Strings.ChrW(13) & Strings.ChrW(10) & "GIS_User_Def_Header_ID : " & CStr(v_lGISHeaderId) & Strings.ChrW(13) & Strings.ChrW(10) & "GIS_User_Def_Detail_ID : " & CStr(v_vValue), vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISUserDefHeaderDetail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : GetUserDef
    ' Description   : Function to Fetch the Lookup Description for the
    '                   supplied table and ID
    ' Edit History  :
    ' RAM20040521   : Created. Replaced the old function with this one
    ' RAM20040521   : Use bGISUserDefLookup to fetch this description, so that it uses
    '                 caching, flexibility for future modification etc
    '                 Mainly for performance enhancement
    ' ***************************************************************** '
    Public Function GetUserDef(ByRef iUserDef As Integer, ByRef vValue As Integer, ByRef vDescription As String) As Integer

        Dim result As Integer = 0
        Dim vLookupKeyArray As Object = Nothing
        Dim vLookupDetails As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default it to empty string
            vDescription = ""

            ' Check if we have the component
            If m_oGISLookup Is Nothing Then

                ' Create the lookup object, NOTE, we are passing in the dPMDAO

                m_oGISLookup = New bGISUserDefLookup.Business
                m_lReturn = m_oGISLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Business Object [ bGISUserDefLookup.Business ]", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDef", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ReDim vLookupKeyArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0)

            vLookupKeyArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = iUserDef  ' GIS User Def Header ID

            vLookupKeyArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vValue  ' GIS User Def Detail ID


            ' Note 1. We are using the language id of the module level variable !!!
            '      2. We are also using the Effective date as now, but it doesn't matter,
            '           since, we always fetch the data based on the ID

            m_lReturn = m_oGISLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vLookupKeyArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vLookupDetails)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' we got some values
                If Informations.IsArray(vLookupDetails) Then

                    vDescription = CStr(vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, 0)).Trim()
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserDef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name          : GetLookup
    ' Description   : Function to Fetch the Lookup Description for the
    '                   supplied table and ID
    ' Edit History  :
    ' RAM20040521   : Created. Replaced the old function with this one
    ' RAM20040521   : Use bPMLookup to fetch this description, so that it uses
    '                 caching, flexibility for future modification etc
    '                 Mainly for performance enhancement
    ' ***************************************************************** '
    Public Function GetLookup(ByRef sPMLookup As String, ByRef vValue As String, ByRef vDescription As String, Optional ByRef vMode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim vLookupKeyArray As Object = Nothing
        Dim vLookupDetails As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default it to empty string
            vDescription = ""

            ' Check the input parameters
            If sPMLookup.Trim().Length = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Parameter Supplied. Table Name missing. sPMLookup = " & sPMLookup, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If vValue.Trim().Length = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid Parameter Supplied. ID Key missing. vValue = " & vValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Check if we have the component
            If m_oPMLookup Is Nothing Then

                ' Create the lookup object, NOTE, we are passing in the dPMDAO

                m_oPMLookup = New BPMLOOKUP.Business
                m_lReturn = m_oPMLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Business Object [ bPMLookup.Business ]", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ReDim vLookupKeyArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, 0)

            vLookupKeyArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = sPMLookup.Trim()

            vLookupKeyArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = vValue

            ' Note 1. We are using the language id of the module level variable !!!
            '      2. We are also using the Effective date as now, but it doesn't matter,
            '           since, we always fetch the data based on the ID

            m_lReturn = m_oPMLookup.GetLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vLookupKeyArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=DateTime.Now, vResultArray:=vLookupDetails)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' we got some values
                If Informations.IsArray(vLookupDetails) Then

                    vDescription = CStr(vLookupDetails(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, 0)).Trim()
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' RAW 03/09/2004 : Resilience : created as a wrapper to replace existing
    'function that has been renamed by adding '_Stateless' suffix
    Public Function GetOldPolicyLinkId(ByRef lRiskId As Integer, ByRef lPolicyLinkId As Integer) As Integer
        ' pass this onto the stateless function to action
        Return GetOldPolicyLinkId_Stateless(lRiskId:=lRiskId, lPolicyLinkId:=lPolicyLinkId)
    End Function

    '******************************************************************************
    ' Name:         GetBinder
    ' Description:
    ' History:      RAW 03/09/2004 : Resilience : created as a wrapper to replace
    '               existing function that has been renamed by adding '_Stateless' suffix
    '******************************************************************************
    Public Function GetBinder(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lPolicyBinderID As Object) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action


            'developer guide no. 98
            Return GetBinder_Stateless(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lPolicyBinderID:=lPolicyBinderID)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBinder " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBinder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         DeleteRiskCancelledOnAdd
    ' Description:
    ' History:      RAW 03/09/2004 : Resilience : created as a wrapper to replace
    '               existing function that has been renamed by adding '_Stateless'
    '               suffix
    '******************************************************************************
    Public Function DeleteRiskCancelledOnAdd() As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action

            Return DeleteRiskCancelledOnAdd_Stateless(v_lRiskId:=m_lRiskId)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteRiskCancelledOnAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRiskCancelledOnAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetRisk
    ' Description:
    ' History:      RAW 03/09/2004 : Resilience : created as a wrapper to replace
    '               existing function that has been renamed by adding '_Stateless'
    '               suffix
    '******************************************************************************
    Public Function GetRisk(ByRef vRiskArray(,) As Object, ByRef vRiskTypeArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action

            Return GetRisk_Stateless(vRiskArray:=vRiskArray, vRiskTypeArray:=vRiskTypeArray, r_lRiskId:=m_lRiskId, v_lProductId:=m_lProductId, v_iSourceID:=m_iSourceID, v_lScreenId:=m_lScreenId, v_lRiskTypeId:=m_lRiskTypeId, v_lInsuranceFolderCnt:=m_lInsuranceFolderCnt, v_lInsuranceFileCnt:=m_lInsuranceFileCnt)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetGISDataModel
    ' Description:
    ' History:      RAW 03/09/2004 : Resilience : created as a wrapper to replace
    '               existing function that has been renamed by adding '_Stateless'
    '               suffix
    '******************************************************************************
    Public Function GetGISDataModel(ByRef r_lGISDataModelId As Integer, ByRef r_sGISDataModel As String, Optional ByRef r_lGISDataModelType As Integer = -1) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action

            Return GetGISDataModel_Stateless(v_lScreenId:=m_lScreenId, r_lGISDataModelId:=r_lGISDataModelId, r_sGISDataModel:=r_sGISDataModel, r_lGISDataModelType:=r_lGISDataModelType)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISDataModel" & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISDataModel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         UpdateStandardWording
    ' Description:
    ' History:      RAW 03/09/2004 : Resilience : created as a wrapper to replace
    '               existing function that has been renamed by adding '_Stateless'
    '               suffix
    '******************************************************************************
    Public Function UpdateStandardWording(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lGISPropertyID As Integer, ByRef lGISObjectID As Integer, ByRef vStandardWordingArray(,) As Object, ByRef sKeyName As String, ByRef sKeyValue As String) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action


            Return UpdateStandardWording_Stateless(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lGISPropertyID:=lGISPropertyID, lGISObjectID:=lGISObjectID, vStandardWordingArray:=vStandardWordingArray, sKeyName:=sKeyName, sKeyValue:=sKeyValue)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateStandardWording Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateStandardWording", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:             UpdateSumInsured
    ' Description:
    ' History:          RAW 03/09/2004 : Resilience : created as a wrapper to
    '                   replace existing function that has been renamed by adding
    '                   '_Stateless' suffix
    '******************************************************************************
    Public Function UpdateSumInsured(ByRef lPolicyLinkId As Integer, ByRef sDataModel As String, ByRef lSumInsuredType As Integer, ByRef vSumInsuredArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action


            Return UpdateSumInsured_Stateless(lPolicyLinkId:=lPolicyLinkId, sDataModel:=sDataModel, lSumInsuredType:=lSumInsuredType, vSumInsuredArray:=vSumInsuredArray)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSumInsured Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSumInsured", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:             UpdateRisk
    ' Description:
    ' History:          RAW 03/09/2004 Resilience : created as a wrapper to replace
    '                   existing function that has been renamed by adding
    '                   '_Stateless' suffix
    '******************************************************************************
    Public Function UpdateRisk(ByRef vRiskArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action


            Return UpdateRisk_Stateless(vRiskArray:=vRiskArray, r_lRiskId:=m_lRiskId)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRisk " & "Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:             CopyExtraDetails
    ' Description:
    ' History:          RAW 03/09/2004 Resilience : created as a wrapper to replace
    '                   existing function that has been renamed by adding
    '                   '_Stateless' suffix
    '******************************************************************************
    Public Function CopyExtraDetails(ByRef sDataModelCode As String, ByRef lNewGISPolicyLinkID As Integer, ByRef lOldGISPolicyLinkId As Integer, ByRef lOldRiskID As Integer, ByRef lNewRiskID As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' pass this onto the stateless function to action

            Return CopyExtraDetails_Stateless(sDataModelCode:=sDataModelCode, lNewGISPolicyLinkID:=lNewGISPolicyLinkID, lOldGISPolicyLinkId:=lOldGISPolicyLinkId, lOldRiskID:=lOldRiskID, lNewRiskID:=lNewRiskID)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyExtraDetails" & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyExtraDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetPolicyLinkIdFromRisk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-03-2005 : PN18879
    ' ***************************************************************** '
    Public Function GetPolicyLinkIdFromRisk(ByVal v_lRiskId As Integer, ByRef r_lGisPolicyLinkId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPolicyLinkIdFromRisk"

        'Dim lReturn As Integer
        Dim vResults(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="risk_id", v_vValue:=v_lRiskId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetPolicyLinkIdFromRiskSQL, sSQLName:=kGetPolicyLinkIdFromRiskName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetPolicyLinkIdFromRiskSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResults) Then

                r_lGisPolicyLinkId = CInt(vResults(0, 0))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                  ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RevertBackRisk_Stateless
    ' Description: Revert the risk in the insurance_file_risk_link table
    '              back to the one being pointed to in original_risk
    ' History:
    ' RAW 16/09/2003 : CQ809 : copied from bSIRListRisks
    ' RAM20040812    : Bug fix. Check for Array before checking its content
    ' RAW 03/09/2004 : Resilience (#2) : renamed procedure by appending '_Stateless' and replaced module variables with new parameters.
    '          The original named procedure still exists but is basically a wrapper to this function
    ' ***************************************************************** '
    Public Function RevertBackRisk_Stateless(ByVal v_lScreenId As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskId As Integer, ByVal v_sGISDataModel As String) As Integer

        Dim result As Integer = 0
        Dim vInsuranceFileRiskLinkArray(,) As Object
        Dim lOriginalRiskCnt As Integer
        Dim sSQL As String = ""
        Dim lGISDataModelId As Integer
        Dim sGISDataModel As String = ""

        ' *****************************************************************************************
        ' This function should not refer to any module variables - unless they have been set from Initialise
        ' *****************************************************************************************

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Note
            ' ====
            ' This function is only concerned at the moment with changing rows to point to the
            ' original risk id instead of the risk being reverted
            ' Where additional rows were added for the risk to be reverted then there is no attempt
            ' to remove them.
            ' I think they should be removed but as they are for a risk that is now dead then
            ' perhaps the complete risk should be deleted from all tables
            ' This is too big an issue for me to deal with now - it will have to be addressed later


            ' RAW 09/10/2003 : CQ2818 : added

            ' Get original risk id
            ReDim vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0)


            vInsuranceFileRiskLinkArray(ACIFRLInsuranceFileCnt, 0) = v_lInsuranceFileCnt

            vInsuranceFileRiskLinkArray(ACIFRLRiskCnt, 0) = v_lRiskId

            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

            m_lReturn = CType(UpdateInsuranceFileRiskLink_Stateless(vInsuranceFileRiskLinkArray:=vInsuranceFileRiskLinkArray, iMode:=gPMConstants.PMEComponentAction.PMView), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get original risk id", vApp:=ACApp, vClass:=ACClass, vMethod:="RevertBackRisk_Stateless")
                Return result
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040812 : Check if we got an array, else exit
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If Not Informations.IsArray(vInsuranceFileRiskLinkArray) Then
                Return result
            End If
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040812 : End
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            Dim auxVar As Object = vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0)



            If CStr(vInsuranceFileRiskLinkArray(ACIFRLStatusFlag, 0)) = "C" And Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then
            Else
                ' This one was not originally shared so we do not need to revert back
                Return result
            End If


            ' If we have reached here then this is a risk that was previously shared so lets link back to it
            ' -------------------------------------------------------------------------------------------


            lOriginalRiskCnt = CInt(vInsuranceFileRiskLinkArray(ACIFRLOriginalRiskCnt, 0))


            ' Revert Policy_Client table to original risk
            '----------------------------------------------
            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent
            'm_lReturn& = UpdateRiskForPolicyClient_Stateless( _
            'v_lInsuranceFileCnt:=v_lInsuranceFileCnt, _
            'v_lOldRiskCnt:=v_lRiskId, _
            'v_lNewRiskCnt:=lOriginalRiskCnt)

            'If m_lReturn& <> PMTrue Then
            '    RevertBackRisk_Stateless = PMFalse
            '    LogMessage m_sUsername, iType:=PMLogError, _
            'sMsg:="Failed to revert policy clients to original risk", _
            'vApp:=ACApp, _
            'vClass:=ACClass, _
            'vMethod:="RevertBackRisk_Stateless"
            '    Exit Function
            'End If


            ' Revert GIS associated_client table to original risk
            '------------------------------------------------------
            With m_oDatabase
                .Parameters.Clear()

                ' RAW 03/09/2004 : Resilience (#2) : replaced m_sGISDataModel with v_sGISDataModel
                sGISDataModel = v_sGISDataModel ' RAW110804 added

                ' RAW110804 replaced m_sGISDataModel with local variable
                If sGISDataModel = "" Then
                    ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent
                    m_lReturn = CType(GetGISDataModel_Stateless(v_lScreenId:=v_lScreenId, r_lGISDataModelId:=lGISDataModelId, r_sGISDataModel:=sGISDataModel), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

                ' This is hard-coded because it is a GIS table whose name depends on the DataModel
                ' RAW110804 replaced m_sGISDataModel with local variable
                'sSQL = "UPDATE " & sGISDataModel & "_associated_client " & _
                '"SET    risk_cnt = " & lOriginalRiskCnt & " " & _
                '"WHERE  insurance_file_cnt = " & v_lInsuranceFileCnt & " " & _
                '"  AND  risk_cnt = " & v_lRiskId

                'm_lReturn& = .SQLAction(sSQL:=sSQL, _
                'sSQLName:="revert associated client risk", _
                'bStoredProcedure:=False)

                'If (m_lReturn& <> PMTrue) Then
                '    RevertBackRisk_Stateless = PMFalse
                '    LogMessage m_sUsername, iType:=PMLogError, _
                'sMsg:="Failed to revert risk for associated client", _
                'vApp:=ACApp, _
                'vClass:=ACClass, _
                'vMethod:="RevertBackRisk_Stateless"
                '     Exit Function
                ' End If
            End With

            ' RAW 09/10/2003 : CQ2818 : end


            ' Revert Insurance_file_risk_link table
            '--------------------------------------
            With m_oDatabase
                With .Parameters
                    .Clear()

                    m_lReturn = .Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter Insurance_File_Cnt", vApp:=ACApp, vClass:=ACClass, vMethod:="RevertBackRisk_Stateless")

                        Return result
                    End If

                    m_lReturn = .Add(sName:="risk_cnt", vValue:=CStr(v_lRiskId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse

                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter r_lRiskID", vApp:=ACApp, vClass:=ACClass, vMethod:="RevertBackRisk_Stateless")

                        Return result
                    End If
                End With

                m_lReturn = .SQLAction(sSQL:=ACRevertRiskSQL, sSQLName:=ACRevertRiskName, bStoredProcedure:=ACRevertRiskStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run " & ACRevertRiskSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="RevertBackRisk_Stateless")

                    Return result
                End If
            End With

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RevertBackRisk_Stateless", vApp:=ACApp, vClass:=ACClass, vMethod:="RevertBackRisk_Stateless", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeletePolicyVersion
    ' Description: Initially created for new renewal what-if quotes only,
    '              if the quote has not been saved and the user is exiting,
    '              delete the policy version (note that the dataset specific
    '              quote output and risk data has been deleted separately).
    ' History: 07/03/2005 CJB - Created (as part of PN19313).
    '
    ' ***************************************************************** '
    Public Function DeletePolicyVersion(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("Insurance_File_Cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'developer guide no 39. 
                m_lReturn = .SQLAction("spu_SIR_Insurance_File_Del", "DeletePolicyVersion", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", DeletePolicyVersion, SQLAction failed.")
                End If

            End With


        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteUnusedEditedStandardWording
    '
    ' Description:
    '
    ' History: 29/04/2005 RKS - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteUnusedEditedStandardWording() As Integer

        Dim result As Integer = 0
        Dim o_bSIRDocTemplate As New bSIRDocTemplate.Business

        Dim vUnusedDocumentTemplate(,) As Object = Nothing

        Dim sDocumentDirectory As String = ""
        Dim sClauseDocTemplateDirectory As String = ""

        Dim sDocTemplateFile As String = ""

        Try

            'get all edited document template no longer used
            bPMAddParameter.AddParameterLite(m_oDatabase, "created_by_id", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUnusedEditedStandardWordingSQL, sSQLName:=ACGetUnusedEditedStandardWordingName, bStoredProcedure:=ACGetUnusedEditedStandardWordingStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vUnusedDocumentTemplate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vUnusedDocumentTemplate) Then
                Return m_lReturn
            End If

            'Get an instance of the bPMBDocTemplate.
            o_bSIRDocTemplate = New bSIRDocTemplate.Business()
            ' Check for errors.
            If o_bSIRDocTemplate Is Nothing Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Document Template business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUnusedEditedStandardWording", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            m_lReturn = o_bSIRDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get initialise Document Template business object", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUnusedEditedStandardWording", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            'Get template file details
            m_lReturn = CType(bPMDocFunctions.GetDocumentDirectory(sDocumentDirectory), gPMConstants.PMEReturnCode)
            sClauseDocTemplateDirectory = sDocumentDirectory & "\Type 7"


            For lTemp As Integer = vUnusedDocumentTemplate.GetLowerBound(1) To vUnusedDocumentTemplate.GetUpperBound(1)

                sDocTemplateFile = sClauseDocTemplateDirectory & "\Doc " & CStr(vUnusedDocumentTemplate(0, lTemp)) & ".zip"

                'Delete the doc template
                m_lReturn = CType(DOCGeneralFunc.DeleteFile(sDocTemplateFile), gPMConstants.PMEReturnCode)

                'Delete the document link

                m_lReturn = CType(o_bSIRDocTemplate.DeleteDocumentLink(CInt(vUnusedDocumentTemplate(0, lTemp))), gPMConstants.PMEReturnCode)
            Next lTemp

            o_bSIRDocTemplate.Dispose()
            o_bSIRDocTemplate = Nothing



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteUnusedEditedStandardWording Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteUnusedEditedStandardWording", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetRiskCodeFromID
    ' Description:  Gets a code from a given risk_code_id.
    ' History:      20/09/2005 CJB Created for PN24176
    '******************************************************************************
    Public Function GetRiskCodeFromID(ByVal v_lRiskCodeId As Integer, ByRef v_sRiskCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("RiskCodeID", CStr(v_lRiskCodeId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect("SELECT code FROM risk_code where risk_code_id = {RiskCodeID}", "Select_GetRiskCodeFromID", False)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", GetRiskCodeFromID, SQLSelect failed.")
                End If

                v_sRiskCode = gPMFunctions.NullToString(.Records.Item(1).Fields()("code")).Trim()

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskCodeFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskCodeFromID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyOriginalRiskPremiumThisYearToNewRisk
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 20-12-2005 : Discount / Loading
    ' ***************************************************************** '
    Private Function CopyOriginalRiskPremiumThisYearToNewRisk(ByVal v_lNewRiskCnt As Integer, ByVal v_lOldRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CopyOriginalRiskPremiumThisYearToNewRisk"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        lReturn = CType(AddInputParameter(v_sName:="new_risk_cnt", v_vValue:=v_lNewRiskCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        lReturn = CType(AddInputParameter(v_sName:="old_risk_cnt", v_vValue:=v_lOldRiskCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kCopyOriginalRiskPremiumThisYearToNewRiskSQL, sSQLName:=kCopyOriginalRiskPremiumThisYearToNewRiskName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kCopyOriginalRiskPremiumThisYearToNewRiskSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name       : UpdateCaseGISPolicyLink
    ' Parameters : n/a
    ' Description:
    ' History    :
    ' Created    : 18-09-2007 (VB)
    ' ***************************************************************** '
    Public Function UpdateCaseGISPolicyLink(ByVal v_lCaseID As Integer, ByVal v_lScreenId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCaseGISPolicyLink"

        Dim lReturn As gPMConstants.PMEReturnCode
        'Dim lStatus As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "case_id", v_lCaseID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_id", v_lScreenId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kUpdateCaseGISPolicyLinkSQL, sSQLName:=kUpdateCaseGISPolicyLinkName, bStoredProcedure:=True, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kUpdateCaseGISPolicyLinkSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here.

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetDefaultClausesForRisk(ByVal v_lInsuranceFileCnt As Integer, _
                                             ByVal v_iSoruceId As Integer, _
                                             ByVal v_lRiskTypeId As Integer, _
                                             ByRef r_vDefaultClausesForRisk(,) As Object) As Integer



        Dim result As Integer = 0
        Const kMethodName As String = "GetDefaultClausesForRisk"
        Try


            'Dim i As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_lRiskTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Source_id", vValue:=CStr(v_iSoruceId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDefaultClausesForRiskSQL, sSQLName:=ACGetDefaultClausesForRiskName, bStoredProcedure:=ACGetDefaultClausesForRiskStored, vResultArray:=r_vDefaultClausesForRisk)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetDefaultClausesForRisk", "The stored procedure " & ACGetAllScreenDetailsSQL & " failed to fetch the record", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here.

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function
End Class
