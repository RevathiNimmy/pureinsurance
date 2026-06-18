Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Text
Imports SSP.Shared
'refer Developer Guide No. 129

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              User Defined Data Maintenance.
    '
    ' Edit History:
    ' RAW 02/09/2003 : CQ2158 : added delete cascade where missing
    ' RKS 26/04/2005 : added gis_property_id to [DataModel]_standard_wording table
    ' RKS 03/05/2005 : added gis_property_id to primary key for [DataModel]_standard_wording table
    ' RKS 31/05/2005 : added gis_object_id to [DataModel]_standard_wording table
    '                  added gis_object_id to primary key for [DataModel]_standard_wording table
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

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lGISDataModelID As Integer
    Private m_sGISDataModel As String = ""
    Private m_lGISDataModelType As Integer
    Private m_sGISDataModelName As String = ""

    Private m_lSwiftIntegration As Integer

    Private m_oiSWSirius As Object
    Private m_sUnderwritingOrAgency As String = ""

    Private m_bIsMarketplaceDM As Boolean = False
    Private m_bIsImportedMarketplaceDM As Boolean = False
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property GISDataModelName() As String
        Get
            Return m_sGISDataModelName
        End Get
        Set(ByVal Value As String)
            m_sGISDataModelName = Value
        End Set
    End Property

    Public Property GISDataModelType() As Integer
        Get
            Return m_lGISDataModelType
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelType = Value
        End Set
    End Property

    Public Property GISDataModelID() As Integer
        Get
            Return m_lGISDataModelID
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelID = Value
        End Set
    End Property

    Public Property GISDataModel() As String
        Get
            Return m_sGISDataModel
        End Get
        Set(ByVal Value As String)
            m_sGISDataModel = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    'True for swift mode false for normal
    Public WriteOnly Property SwiftIntegration() As Integer
        Set(ByVal Value As Integer)
            Try
                If m_lSwiftIntegration = 0 And Value <> 0 Then
                End If
                m_lSwiftIntegration = Value

            Catch
            End Try

        End Set
    End Property


    Public ReadOnly Property SQLServerVersion() As Integer
        Get

            ' RAW 02/09/2003 : CQ2158 : moved detail to main module so that it is available to other modules
            Dim result As Integer = 0
            result = GetSQLServerVersion(r_oDatabase:=m_oDatabase)
            If result <= 0 Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        End Get
    End Property

    ''' <summary>
    ''' To read and write property whether this is a Market Place data model or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMarketplaceDM() As Boolean
        Get
            Return m_bIsMarketplaceDM
        End Get
        Set(ByVal Value As Boolean)
            m_bIsMarketplaceDM = Value
        End Set
    End Property

    ''' <summary>
    ''' To read and write property whether this is a imported Market Place data model or not
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsImportedMarketplaceDM() As Boolean
        Get
            Return m_bIsImportedMarketplaceDM
        End Get
        Set(ByVal Value As Boolean)
            m_bIsImportedMarketplaceDM = Value
        End Set
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' RAW 02/09/2003 : CQ2158 : added
            If GetSQLServerVersion(r_oDatabase:=m_oDatabase) <= 0 Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get SQLServer Version", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

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
                If Not (m_oiSWSirius Is Nothing) Then
                    m_oiSWSirius = Nothing
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
    ' Name: GetDataModelDetails (Public)
    '
    ' Description: Gets the data dictionary and screen layout
    '
    ' ***************************************************************** '
    Public Function GetDataModelDetails() As Integer
        Dim result As Integer = 0
        Dim iRetVal As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Now get the GIS data model type

            'get gis_data_model_type_id
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect(sSQL:="select gis_data_model_type_id,gis_data_model_id, PMCaption.caption, is_imported_marketplace_data_model , is_marketplace_data_model from gis_data_model INNER JOIN PMCaption ON GIS_Data_Model.caption_id = PMCaption.caption_id where code='" & m_sGISDataModel & "'", sSQLName:="bGISMaintainDataDictionary.GetDataModelDetails raw SQL", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=True)


            'could not read data model
            Dim sSQL As String = ""
            Dim lCaptionId As Integer
            Dim sCaption As String = ""
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vArray) Then


                Select Case GISDataModelType
                    ' Developer Guide No. 123
                    Case SSP.Shared.GISDataModelType.GISDMTypeRisk
                        'this should already be created
                    Case Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                End Select

                'get the caption id
                m_oDatabase.Parameters.Clear()
                ' Developer Guide No. 39
                sSQL = "spu_pm_caption_id_return"
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "language_id", m_iLanguageID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "caption", sCaption, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "caption_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                If iRetVal = gPMConstants.PMEReturnCode.PMTrue Then
                    iRetVal = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="bGISMaintainDataDictionary.GetDataModelDetails.GetCaption id raw sql", bStoredProcedure:=True)
                End If
                lCaptionId = m_oDatabase.Parameters.Item("caption_id").Value

                'get the gis_data_model_id
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.SQLSelect(sSQL:="select MAX(gis_data_model_id) + 1 FROM GIS_Data_Model", sSQLName:="bGISMaintainDataDictionary.GetDataModelDetails.Select Max raw SQL", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=True)

                m_lGISDataModelID = CInt(vArray(0, 0))

                'add the new entry
                m_oDatabase.Parameters.Clear()
                sSQL = "INSERT INTO gis_data_model (gis_data_model_id,code,caption_id ,description,is_deleted,effective_date,gis_data_model_type_id) values ()"
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "gis_data_model_id", m_lGISDataModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, 2)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "code", m_sGISDataModel, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, 2)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "caption_id", lCaptionId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, 2)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "Description", sCaption, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, 2)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "is_deleted", 0, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, 2)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "effective_date", DateTime.Now, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, 2)
                bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetVal, "gis_data_model_type_id", GISDataModelType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, 2)
                If iRetVal = gPMConstants.PMEReturnCode.PMTrue Then
                    iRetVal = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="bGISMaintainDataDictionary.GetDataModelDetails.Add data model raw sql", bStoredProcedure:=False)
                End If
            Else
                Dim auxVar As Object = vArray(0, 0)


                If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
                    m_lGISDataModelType = SSP.Shared.GISDataModelType.GISDMTypeNotSet
                Else

                    m_lGISDataModelType = CInt(vArray(0, 0))
                End If

                m_lGISDataModelID = CInt(vArray(1, 0))

                m_sGISDataModelName = CStr(vArray(2, 0))

                m_bIsImportedMarketplaceDM = ToSafeBoolean(vArray(3, 0))

                m_bIsMarketplaceDM = ToSafeBoolean(vArray(4, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModelDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModelDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '*******************************************************************************
    ' Name: GetDataModelBOMRequired
    '
    ' Description: Return the BOMRequired setting for GIS Data set.
    '
    ' Edit History:
    '   PW120406 - Created - PN28024
    '*******************************************************************************
    Public Function GetDataModelBOMRequired(ByVal v_sDataModelCode As String, ByRef r_sBOMRequired As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data Model Specific BOM Required setting
            lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISSharedConstants.GISRegDataSetBOMRequired, r_sSettingValue:=r_sBOMRequired), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModelBOMRequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModelBOMRequired", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    '*******************************************************************************
    ' Name: SetDataModelBOMRequired
    '
    ' Description: Set the BOMRequired setting for GIS Data set.
    '
    ' Edit History:
    '   PW120406 - Created - PN28024
    '*******************************************************************************
    Public Function SetDataModelBOMRequired(ByVal sDataModelCode As String, ByVal sBOMRequired As String) As Integer

        Dim result As Integer = 0
        Dim sSubKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Data Model Specific BOM Required setting
            sSubKey = GISSharedConstants.ACOIMGISSubKey & "\" & sDataModelCode

            'Set the value
            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=GISSharedConstants.GISRegDataSetBOMRequired, v_sSettingValue:=sBOMRequired, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetDataModelBOMRequired Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetDataModelBOMRequired", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    ' Name: GetObjectAndPropertyDetails (Public)
    '
    ' Description: Gets the data dictionary and screen layout
    '
    ' ***************************************************************** '
    'Developer Guide No. 33
    Public Function GetObjectAndPropertyDetails(ByRef r_vGISObject(,) As Object, ByRef r_vGISProperty As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Developer Guide No. 17
            r_vGISObject = Nothing

            r_vGISProperty = Nothing



            m_oDatabase.Parameters.Clear()

            'Now get the GIS objects

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(m_lGISDataModelID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllGISObjectSQL, sSQLName:=ACGetAllGISObjectName, bStoredProcedure:=ACGetAllGISObjectStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vGISObject, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Now get the GIS properties

            If Not Informations.IsArray(r_vGISObject) Then
                'Oops, we need to create the Policy binder

                m_lReturn = CType(CreateDefaultObjects(r_vGISObject:=r_vGISObject, r_vGISProperty:=r_vGISProperty, v_lGisDataModelType:=m_lGISDataModelType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'It still didn't work, so give up
                If Not Informations.IsArray(r_vGISObject) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Now let's set up the extra GIS stuff - the QEM Usage record
                'and the registry settings

                m_lReturn = CType(CreateGisQemUsage(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(CreateRegistrySettings(m_sGISDataModel), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(CopyDefaultGISLists(m_sGISDataModel), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            'refer Developer Guide No. 18

            ReDim r_vGISProperty((r_vGISObject).GetUpperBound(1))

            'refer Developer Guide No. 18

            For iTemp As Integer = (r_vGISObject).GetLowerBound(1) To (r_vGISObject).GetUpperBound(1)

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_object_id", vValue:=r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, iTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllGISPropertySQL, sSQLName:=ACGetAllGISPropertyName, bStoredProcedure:=ACGetAllGISPropertyStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                r_vGISProperty(iTemp) = vArray
            Next iTemp

            vArray = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectAndPropertyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectAndPropertyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOtherDetails (Public)
    '
    ' Description: Gets the data dictionary and screen layout
    '
    ' ***************************************************************** '
    ' Developer Guide No. 17
    Public Function GetOtherDetails(ByRef r_vPartyType As Object(,), ByRef r_vSumInsuredType As Object(,), ByRef r_vGISUserDefHeader As Object(,), ByRef r_vProduct As Object(,), ByRef r_vIndexLinking As Object, Optional ByRef r_vDocumentFilter As Object = Nothing, Optional ByRef r_vPMLookupList As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vPartyType = Nothing
            r_vSumInsuredType = Nothing
            r_vDocumentFilter = Nothing
            r_vGISUserDefHeader = Nothing
            r_vProduct = Nothing
            r_vIndexLinking = Nothing
            r_vPMLookupList = Nothing

            'Now get the GIS properties

            m_lReturn = CType(GetALookup(iLanguageID:=m_iLanguageID, sTableName:="party_type", vArray:=r_vPartyType, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetALookup(iLanguageID:=m_iLanguageID, sTableName:="sum_insured_type", vArray:=r_vSumInsuredType, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(GetDocumentFilter(vArray:=r_vDocumentFilter, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            m_lReturn = CType(GetALookup(iLanguageID:=m_iLanguageID, sTableName:="GIS_user_def_header", vArray:=r_vGISUserDefHeader, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetALookup(iLanguageID:=m_iLanguageID, sTableName:="product", vArray:=r_vProduct, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetALookup(iLanguageID:=m_iLanguageID, sTableName:="index_linking", vArray:=r_vIndexLinking, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetPMLookupList(r_vArray:=r_vPMLookupList, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOtherDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOtherDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Redoes the data
    '
    ' Edit History  :
    ' RAM20040914   : Code changes related to Adding Index to
    '                   Disclosure and Specials object
    ' ***************************************************************** '
    Public Function Update(ByRef r_vGISObject(,) As Object, ByRef r_vGISProperty() As Object, ByVal v_lSingleObjectId As Integer, Optional ByVal bFromPIE As Boolean = False, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vArray(,) As Object
        Dim sSQL2 As New StringBuilder
        Dim sSQL As New StringBuilder
        Dim bNewObject As Boolean
        Dim vKeyArray() As Object
        Dim sTemp As String = ""
        Dim llBound, lUbound As Integer
        Dim sSpText, sSpTextName As String
        Dim bAddLoadSaveDBModeKey As Integer
        Dim vAdditionalParentFields As Object = Nothing
        Dim bStandardWordingPresent As Boolean

        Dim bTableExists As Boolean
        Dim vObjectResults(,) As Object = Nothing
        Dim bNewProperty As Boolean
        Dim sFieldType As String = ""  'this holds the SQL data definition
        Dim sScreenHierarchy As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not bFromPIE Then
                m_lReturn = RebuildDefaultObjects(r_vGISObject(2, 0))
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                RaiseError("Update", "RebuildDefaultObjects Failed." & gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            sSQL2 = New StringBuilder("")

            'set the boundries of the loop
            If v_lSingleObjectId = -1 Then
                llBound = r_vGISObject.GetLowerBound(1)
                lUbound = r_vGISObject.GetUpperBound(1)
            Else
                llBound = v_lSingleObjectId
                lUbound = llBound
            End If

            'It goes something like this
            'First we loop around the object array, updating existing ones and
            'adding any new ones.
            For lTemp As Integer = llBound To lUbound

                bStandardWordingPresent = False

                'For each of the objects, we loop around its property array, adding
                'fields that don't already exist, and building up a script to update the
                'database

                m_oDatabase.Parameters.Clear()

                If bFromPIE Then
                    'Is object new or existing?
                    'Can't check GIS Object as record will have already been added as part
                    'of previous ImportFixedTables routine.  Must check the sysobjects table
                    'to see if item exists by this name in database.

                    m_lReturn = m_oDatabase.SQLSelect(sSQL:="SELECT * FROM sysobjects WHERE " &
                                "UPPER(name) LIKE '" &
                                CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).ToUpper() & "' AND UPPER(xtype) = 'U'", sSQLName:="Get SysObject Record", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vObjectResults)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If

                    bTableExists = Informations.IsArray(vObjectResults)
                Else

                    If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) = -1 Then
                        'We're adding

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_object_id", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    Else

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_object_id", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If



                    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="object_name", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="table_name", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="max_instances", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_quote_object", vValue:=(If(Not CBool(r_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lTemp)), gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If
                    If Not NullToBoolean(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or Convert.ToString(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) = "0" Then
                        r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp) = Nothing
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="parent_object_id", vValue:=r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="polaris_object_id", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOPolarisObjectId, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_selectable_for_screen", vValue:=(If(Not CBool(r_vGISObject(pbObjectAndPropertyConsts.ACOIsSelectableForScreen, lTemp)), gPMConstants.PMEReturnCode.PMFalse, gPMConstants.PMEReturnCode.PMTrue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_non_gis", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If
                    'GSD Added 010702

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="Edit_Flags", vValue:=(r_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lTemp)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sScreenHierarchy = v_sScreenHierarchy & $"/Object({CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)).Trim()})"
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'GSD End 010702

                    If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) = -1 Then
                        'We're adding
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertGISObjectSQL, sSQLName:=ACInsertGISObjectName, bStoredProcedure:=ACInsertGISObjectStored)
                    Else
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateGISObjectSQL, sSQLName:=ACUpdateGISObjectName, bStoredProcedure:=ACUpdateGISObjectStored)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If
                End If

                bNewObject = False
                sSQL = New StringBuilder("")


                If (CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) = -1) Or (Not bTableExists And bFromPIE) Then

                    If Not bFromPIE Then

                        r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp) = m_oDatabase.Parameters.Item("gis_object_id").Value
                    End If

                    bNewObject = True
                    'Build up the create table script

                    sSQL = New StringBuilder("CREATE TABLE " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "(")
                End If



                'vArray = r_vGISProperty(lTemp)
                vArray = r_vGISProperty(lTemp).Clone()


                vKeyArray = Nothing

                If Informations.IsArray(vArray) Then

                    For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)

                        m_oDatabase.Parameters.Clear()


                        If CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)) = "dElEtEd" Then
                            'need to delete. if -1 then new so just ignore, else really delete it

                            If CDbl(vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2)) <> -1 Then
                                sSpText = ACDeleteGISPropertySQL
                                bPMAddParameter.AddParameter(m_oDatabase, sSpText, m_lReturn, "gis_property_id", vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                bPMAddParameter.AddParameter(m_oDatabase, sSpText, m_lReturn, "gis_object_id", vArray(pbObjectAndPropertyConsts.ACPGISObjectId, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    'remove from gis_property
                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSpText, sSQLName:=ACDeleteGISPropertyName, bStoredProcedure:=True)
                                    'drop column from table

                                    sSpText = "ALTER TABLE " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & Strings.ChrW(13) & Strings.ChrW(10)

                                    sSpText = sSpText & "DROP COLUMN " & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2))
                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSpText, sSQLName:="Drop column", bStoredProcedure:=False)
                                End If
                            End If
                        Else
                            'adding or updating property

                            If bFromPIE And Not bNewObject Then
                                'We need to check if the column exists


                                m_lReturn = m_oDatabase.SQLSelect("select null from syscolumns where id = " &
                                            "object_id('" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "') and name = '" &
                                            CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & "'", "Check Property", False)

                                bNewProperty = (m_oDatabase.Records.Count() = 0)

                                m_oDatabase.Parameters.Clear()
                            ElseIf bNewObject And bFromPIE Then
                                bNewProperty = True
                            Else


                                If (CDbl(vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2)) = -1) Or bNewObject Or bNewProperty Then
                                    'We're adding
                                    sSpText = ACInsertGISPropertySQL
                                    sSpTextName = ACInsertGISPropertyName
                                    bPMAddParameter.AddParameter(m_oDatabase, sSpText, m_lReturn, "gis_property_id", vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                                Else
                                    'updating
                                    sSpText = ACUpdateGISPropertySQL
                                    sSpTextName = ACUpdateGISPropertyName
                                    bPMAddParameter.AddParameter(m_oDatabase, sSpText, m_lReturn, "gis_property_id", vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                End If

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                    Return result
                                End If

                                'add the rest of the parameters

                                addPropertyAddParameter(sSpText, r_vGISObject, lTemp, vArray, lTemp2, v_sUniqueId, sScreenHierarchy)

                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSpText, sSQLName:=sSpTextName, bStoredProcedure:=True)
                                End If

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                    Return result
                                End If
                            End If

                            'if adding, get the new key and store in attribute array

                            If CDbl(vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2)) = -1 Then

                                r_vGISProperty(lTemp)(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2) = m_oDatabase.Parameters.Item("gis_property_id").Value
                            End If


                            'If Not CBool(vArray.GetValue(pbObjectAndPropertyConsts.ACPEditFlags, lTemp2)) And GISSharedPropertyConstants.GISDSEditNoDBColumn Then
                            If CBool(Not vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTemp2) And GISSharedPropertyConstants.GISDSEditNoDBColumn) Then

                                If (CDbl(vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2)) = -1) Or bNewProperty Then
                                    'New object OR new property

                                    If bNewObject Then

                                        sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & " ")
                                    Else

                                        sSQL = New StringBuilder("ALTER TABLE " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & Strings.ChrW(13) & Strings.ChrW(10))

                                        sSQL.Append("ADD " & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)))
                                    End If

                                    sFieldType = ""  'build the SQL type for each

                                    Select Case vArray(pbObjectAndPropertyConsts.ACPDataType, lTemp2)
                                        Case GISSharedConstants.GISDataTypeComment
                                            sFieldType = " VARCHAR(MAX)"
                                        Case GISSharedConstants.GISDataTypeText
                                            sFieldType = " VARCHAR("

                                            If CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOGISListID Then
                                                sTemp = "70"
                                            Else
                                                sTemp = "255"
                                            End If
                                            sFieldType = sFieldType & sTemp & ")"
                                        Case GISSharedConstants.GISDataTypeNumeric, GISSharedConstants.GISDataTypeInteger
                                            sFieldType = " INT"
                                        Case GISSharedConstants.GISDataTypeDate
                                            sFieldType = " DATETIME"
                                        Case GISSharedConstants.GISDataTypeOption
                                            sFieldType = " TINYINT"
                                        Case GISSharedConstants.GISDataTypeCurrency
                                            sFieldType = " NUMERIC(19,4)"
                                        Case GISSharedConstants.GISDataTypePercentage
                                            sFieldType = " NUMERIC(7,4)"
                                        Case GISSharedConstants.GISDataTypecode
                                            sFieldType = " CHAR(10)"

                                    End Select

                                    sSQL.Append(sFieldType)

                                    Dim auxVar As Object = vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp2)


                                    If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                                        If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp2)) = 1 Then
                                            sSQL.Append(" NOT")
                                        End If
                                    End If

                                    If bNewObject Then
                                        sSQL.Append(" NULL,")
                                    Else
                                        sSQL.Append(" NULL" & Strings.ChrW(13) & Strings.ChrW(10))

                                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add column", bStoredProcedure:=False)

                                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            result = gPMConstants.PMEReturnCode.PMFalse
                                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                            Return result
                                        End If


                                        ' Developer Guide No. 123
                                        If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTClaim Then
                                            'create dmc_claim table
                                            'If FixupSqlAndReRun(sSQL, "Add column", m_sGISDataModel & "_Work_Claim", m_sGISDataModel & "_claim") <> PMTrue Then
                                            '    Update = PMFalse
                                            '    Exit Function
                                            'End If
                                        End If

                                        ' Developer Guide No. 123
                                        If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTPeril Then
                                            'create dmc_peril table
                                            'If FixupSqlAndReRun(sSQL, "Add column", m_sGISDataModel & "_Work_Claim_Peril", m_sGISDataModel & "_Claim_Peril") <> PMTrue Then
                                            '    Update = PMFalse
                                            '    Exit Function
                                            'End If
                                        End If

                                    End If
                                    'need to add bespoke properties into the Swift database
                                    'bNewObject = False ensures only add added parameters, not the object keys
                                    If m_lSwiftIntegration <> 0 And Not bNewObject Then

                                        If Not ((m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_NotRenderedByPB) = GISSharedPropertyConstants.SwiftMode_NotRenderedByPB) Then

                                            ' this property will be rendered on a screen by Sirius PB run-time

                                            'we are only interested in properties added to two objects "UserDefined" and "PolicyUserDefined"

                                            If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)).ToLower() = "userdefined" Or CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)).ToLower() = "policyuserdefined" Then




                                                If Not m_oiSWSirius.AddUserDefinedField(CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)), vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2), If((CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOSwiftCommonCode), "VARCHAR(10)", sFieldType)) Then


                                                    Throw New System.Exception("1, , " + "Error adding (" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)) & "." &
                                                                               CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)) & ") to Swift DB")
                                                End If
                                            End If
                                        Else
                                            ' this property will NOT be rendered on a screen by Sirius PB run-time

                                            If (m_lSwiftIntegration And GISSharedPropertyConstants.SwiftMode_UserDefinesOnAllObjects) = GISSharedPropertyConstants.SwiftMode_UserDefinesOnAllObjects Then


                                                If (CBool(vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTemp2)) And GISSharedPropertyConstants.GISDSEditNoDBColumn) = GISSharedPropertyConstants.GISDSEditNoDBColumn Then
                                                    ' this property should not exist as a column in the database
                                                Else


                                                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOSwiftAddress Then

                                                        ' this is a special type of control that needs special handling


                                                        If Not m_oiSWSirius.AddAddressSelectorToTable(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sColumnNamePrefix:=vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) Then


                                                            Throw New System.Exception("1, , " + "Error adding (" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "." &
                                                                                       CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & ") to Swift DB")
                                                        End If
                                                    Else



                                                        If Not m_oiSWSirius.AddColumnToTable(sTableName:=CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)), sColumnName:=vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2), sDataType:=If((CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = GISSharedPropertyConstants.ACOSwiftCommonCode), "VARCHAR(10)", sFieldType)) Then


                                                            Throw New System.Exception("1, , " + "Error adding (" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "." &
                                                                                       CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & ") to Swift DB")
                                                        End If
                                                    End If
                                                End If
                                            End If
                                        End If
                                    End If


                                End If

                                'Set up key array
                                Dim auxVar_2 As Object = vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp2)


                                If Not (Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2)) Then

                                    If CDbl(vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp2)) = 1 Then
                                        If Informations.IsArray(vKeyArray) Then

                                            ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
                                        Else
                                            ReDim vKeyArray(0)
                                        End If




                                        vKeyArray(vKeyArray.GetUpperBound(0)) = vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)
                                    End If
                                End If
                            End If  'adding or updating a property


                            If CDbl(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = 5 Then
                                'Standard Wording field in this table
                                'so we need to record the parent fields
                                bStandardWordingPresent = True
                            End If
                        End If  'only add properties if risk, or and key or editable,
                    Next lTemp2
                End If

                'Add all the parent keys for standard wording

                If Not (Convert.IsDBNull(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or Informations.IsNothing(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))) And bStandardWordingPresent Then

                    For lTemp2 As Integer = vKeyArray.GetLowerBound(0) To vKeyArray.GetUpperBound(0)

                        If (CStr(vKeyArray(lTemp2)).IndexOf("Policy_binder_") + 1) = 0 Then
                            If Informations.IsArray(vAdditionalParentFields) Then

                                ReDim Preserve vAdditionalParentFields(vAdditionalParentFields.GetUpperBound(0) + 1)
                            Else
                                ReDim vAdditionalParentFields(0)
                            End If




                            vAdditionalParentFields(vAdditionalParentFields.GetUpperBound(0)) = vKeyArray(lTemp2)
                        End If
                    Next lTemp2
                End If

                If bNewObject Then
                    'Finish off the create script
                    ' Developer Guide No. 
                    sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1) & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Create table", bStoredProcedure:=False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                        Return result
                    End If


                    ' Developer Guide No. 123
                    If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTClaim Then
                        'create dmc_claim table
                        'If FixupSqlAndReRun(sSQL, "Create table", m_sGISDataModel & "_Work_Claim", m_sGISDataModel & "_claim") <> PMTrue Then
                        '    Update = PMFalse
                        '    Exit Function
                        'End If
                    End If

                    ' Developer Guide No. 123
                    If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTPeril Then
                        'create dmc_peril table
                        'If FixupSqlAndReRun(sSQL, "Create table", m_sGISDataModel & "_Work_Claim_Peril", m_sGISDataModel & "_Claim_Peril") <> PMTrue Then
                        '    Update = PMFalse
                        '    Exit Function
                        'End If
                    End If

                    'Add the primary key
                    If Informations.IsArray(vKeyArray) Then


                        sSQL = New StringBuilder("ALTER TABLE " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & Strings.ChrW(13) & Strings.ChrW(10))
                        sSQL.Append("ADD PRIMARY KEY CLUSTERED (")

                        For lTemp2 As Integer = vKeyArray.GetLowerBound(0) To vKeyArray.GetUpperBound(0)

                            sSQL.Append(CStr(vKeyArray(lTemp2)) & ", ")
                        Next lTemp2

                        sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 2) & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add primary key", bStoredProcedure:=False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                            Return result
                        End If


                        ' Developer Guide No. 123
                        If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTClaim Then
                            'create dmc_claim table
                            'If FixupSqlAndReRun(sSQL, "Add Primary Key", m_sGISDataModel & "_Work_Claim", m_sGISDataModel & "_claim") <> PMTrue Then
                            '    Update = PMFalse
                            '    Exit Function
                            'End If
                        End If

                        If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTPeril Then
                            'create dmc_peril table
                            'If FixupSqlAndReRun(sSQL, "Add Primary Key", m_sGISDataModel & "_Work_Claim_Peril", m_sGISDataModel & "_Claim_Peril") <> PMTrue Then
                            '    Update = PMFalse
                            '    Exit Function
                            'End If
                        End If

                    End If

                    'Create the index linking to the parent
                    Select Case r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)
                        ' Developer Guide No. 123
                        Case SSP.Shared.GISDataModelType.GISOTRisk, SSP.Shared.GISDataModelType.GISOTCase

                            'We can be _extremely_ sneaky here, as we already have all the key Informations
                            'for the parent in vKeyArray

                            If Not (Convert.IsDBNull(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or Informations.IsNothing(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))) Then
                                'Just to be safe
                                If Informations.IsArray(vKeyArray) Then



                                    sSQL = New StringBuilder("CREATE INDEX XFK" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & " ON " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & Strings.ChrW(13) & Strings.ChrW(10))
                                    sSQL.Append("(")


                                    For lTemp2 As Integer = vKeyArray.GetLowerBound(0) To vKeyArray.GetUpperBound(0) - 1

                                        sSQL.Append(CStr(vKeyArray(lTemp2)) & ", ")
                                    Next lTemp2

                                    sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 2) & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add index", bStoredProcedure:=False)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                        Return result
                                    End If

                                End If
                            End If

                            'Add the foreign key to the parent
                            'Ditto on the stuff

                            If Not (Convert.IsDBNull(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp)) Or Informations.IsNothing(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))) Then
                                'Just to be safe
                                If Informations.IsArray(vKeyArray) Then


                                    sSQL = New StringBuilder("ALTER TABLE " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)))
                                    sSQL.Append(" ADD FOREIGN KEY (")


                                    For lTemp2 As Integer = vKeyArray.GetLowerBound(0) To vKeyArray.GetUpperBound(0) - 1

                                        sSQL.Append(CStr(vKeyArray(lTemp2)) & ", ")
                                    Next lTemp2

                                    sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 2) & ")" & Strings.ChrW(13) & Strings.ChrW(10))

                                    'The table is just the last (used) key minus the trailing _id
                                    ' RAW 02/09/2003 : CQ2158 : removed if test by adding GetDeleteCascadeText function


                                    sSQL.Append("REFERENCES " & CStr(vKeyArray(vKeyArray.GetUpperBound(0) - 1)).Substring(0, (CStr(vKeyArray(vKeyArray.GetUpperBound(0) - 1))).Length - 3) & GetDeleteCascadeText() & Strings.ChrW(13) & Strings.ChrW(10))

                                    m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add foreign key", bStoredProcedure:=False)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                        Return result
                                    End If
                                End If
                            End If

                            'Add the permissions

                            sSQL2.Append("GRANT ALL ON " & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & " TO " &
                                         "PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10))

                            ' Developer Guide No. 123
                        Case SSP.Shared.GISDataModelType.GISOTClaim
                            '            'no foreign keys which we implicity know for dmc_Work_Claim
                            '            sSQL = "ALTER TABLE " & m_sGISDataModel & "_Claim"
                            '            sSQL = sSQL & " ADD FOREIGN KEY (claim_id)"
                            '            ' RAW 02/09/2003 : CQ2158 : replaced hard-coded "ON DELETE CASCADE" with GetDeleteCascadeText
                            '            sSQL = sSQL & " REFERENCES Claim " & GetDeleteCascadeText
                            '            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
                            ''                                              sSQLName:="Add foreign key", _
                            ''                                              bStoredProcedure:=False)

                            'but foreign key on dmc_Claim
                            sSQL = New StringBuilder("ALTER TABLE " & m_sGISDataModel & "_Claim")
                            sSQL.Append(" ADD FOREIGN KEY (claim_id)")
                            ' RAW 02/09/2003 : CQ2158 : replaced hard-coded "ON DELETE CASCADE" with GetDeleteCascadeText
                            sSQL.Append(" REFERENCES Claim " & GetDeleteCascadeText())
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add foreign key", bStoredProcedure:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If

                            ' Developer Guide No. 123
                        Case SSP.Shared.GISDataModelType.GISOTPeril
                            '            'add foreign keys which we implicity know for dmc_Work_Peril
                            '            sSQL = "ALTER TABLE " & r_vGISObject(ACOTableName, lTemp)
                            '            sSQL = sSQL & " ADD FOREIGN KEY (claim_id)"
                            '            ' RAW 02/09/2003 : CQ2158 : replaced hard-coded "ON DELETE CASCADE" with GetDeleteCascadeText
                            '            sSQL = sSQL & " REFERENCES " & m_sGISDataModel & "_Claim " & GetDeleteCascadeText
                            '            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
                            ''                                              sSQLName:="Add foreign key", _
                            ''                                              bStoredProcedure:=False)
                            '
                            '            If (m_lReturn& <> PMTrue) Then
                            '                Update = PMFalse
                            '                m_lReturn = RollbackTrans()
                            '                Exit Function
                            '            End If

                            'add foreign keys which we implicity know for dmc_Peril
                            sSQL = New StringBuilder("ALTER TABLE " & m_sGISDataModel & "_Claim_Peril")
                            sSQL.Append(" ADD FOREIGN KEY (claim_id)")
                            ' RAW 02/09/2003 : CQ2158 : replaced hard-coded "ON DELETE CASCADE" with GetDeleteCascadeText
                            sSQL.Append(" REFERENCES Claim " & GetDeleteCascadeText())
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add foreign key", bStoredProcedure:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If

                            ' RVH 15/10/2004 - Add scalability index. Claim_Peril
                            ' RAM20041026 : Code added to check, if the index exists first
                            '            sSQL = ""
                            '            sSQL = sSQL & "IF NOT EXISTS(SELECT NULL from sysindexes where name = '" & "I__" & m_sGISDataModel & "_Claim_Peril__Claim_ID" & "')" & vbCrLf
                            '            sSQL = sSQL & "CREATE INDEX I__" & m_sGISDataModel & "_Claim_Peril__Claim_ID" & _
                            ''                          " ON " & m_sGISDataModel & "_Claim_Peril " & _
                            ''                          "(Claim_ID)"
                            '            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, _
                            ''                                              sSQLName:="Add Claim Peril Index", _
                            ''                                              bStoredProcedure:=False)
                            '
                            '            If (m_lReturn <> PMTrue) Then
                            '                Update = PMFalse
                            '                m_lReturn = RollbackTrans()
                            '                Exit Function
                            '            End If

                            ' RVH 15/10/2004 - Add scalability index. Claim_Peril
                            ' RAM20041026 : Code added to check, if the index exists first
                            sSQL = New StringBuilder("")
                            sSQL.Append("IF NOT EXISTS(SELECT NULL from sysindexes where name = '" &
                                        "I__" & m_sGISDataModel & "_Claim_Peril__Claim_ID" & "')" & Strings.ChrW(13) & Strings.ChrW(10))
                            sSQL.Append("CREATE INDEX I__" & m_sGISDataModel & "_Claim_Peril__Claim_ID" &
                                        " ON " & m_sGISDataModel & "_Claim_Peril " &
                                        "(Claim_ID)")

                            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add Work Claim Peril Index", bStoredProcedure:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If


                            ' Developer Guide No. 123
                        Case SSP.Shared.GISDataModelType.GISOTNonGisSpecials
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ' RAM20040914 : Add Index to the following tables
                            '               data_model_code_Specials Tables's  data_model_code_Policy_binder_id Property
                            '               Ref. Renewal Deadlock Issue and Performance enhancement of Renewal Processing
                            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                            ' SPECIALS OBJECT
                            'CREATE INDEX [I__ProductPer_Specials__ProductPer_Policy_binder_id] ON [dbo].[ProductPer_Specials] ([ProductPer_Policy_binder_id]) ON [PRIMARY]

                            If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)).ToLower() = (m_sGISDataModel &
                                    "_Specials").ToLower() Then


                                sSQL = New StringBuilder("IF NOT EXISTS(SELECT NULL from sysindexes where name = '" & "I__" &
                                       CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "__" & m_sGISDataModel & "_Policy_Binder_id" & "')" & Strings.ChrW(13) & Strings.ChrW(10))


                                sSQL.Append("CREATE INDEX [I__" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "__" &
                                            m_sGISDataModel & "_Policy_Binder_id] ON [dbo].[" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) & "] " & Strings.ChrW(13) & Strings.ChrW(10))
                                sSQL.Append("([" & m_sGISDataModel & "_Policy_Binder_id]" & ")  ON [PRIMARY]" & Strings.ChrW(13) & Strings.ChrW(10))

                                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Add index", bStoredProcedure:=False)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Index. " & sSQL.ToString(), vApp:=ACApp, vClass:=ACClass, vMethod:="Update")
                                    Return result
                                End If
                            End If

                    End Select
                End If



                ' Developer Guide No. 123
                'bAddLoadSaveDBModeKey = bAddLoadSaveDBModeKey Or (CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTClaim Or CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp)) = SSP.Shared.GISDataModelType.GISOTPeril)
                If r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp) = SSP.Shared.GISDataModelType.GISOTClaim Then
                    bAddLoadSaveDBModeKey = 1
                ElseIf r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lTemp) = SSP.Shared.GISDataModelType.GISOTPeril Then
                    bAddLoadSaveDBModeKey = 1
                Else
                    bAddLoadSaveDBModeKey = bAddLoadSaveDBModeKey

                End If
            Next lTemp

            If bAddLoadSaveDBModeKey Then

                'Set the value
                m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="LoadSaveDBMode", v_sSettingValue:="2", v_sSubKey:=GISSharedConstants.ACOIMGISSubKey & "\" & m_sGISDataModel), gPMConstants.PMEReturnCode)
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            'If we've got permissions to set, execute the script outside of the transaction
            If sSQL2.ToString() <> "" Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL2.ToString(), sSQLName:="Set permissions", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            If v_lSingleObjectId = -1 Then  'only need to do if doing all

                'Now let's do the sum insured
                If m_lSwiftIntegration <> 0 Then
                    ' dont do this for swift
                Else
                    m_lReturn = CType(GenerateSumInsured(), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'Now let's do the standard wording
                If m_lSwiftIntegration <> 0 Then
                    ' dont do this for swift
                Else

                    m_lReturn = CType(GenerateStandardWording(vAdditionalParentFields), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'Now let's do the search indexes
                m_lReturn = CType(GenerateSearchIndexes(r_vGISObject:=r_vGISObject, r_vGISProperty:=r_vGISProperty), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                'Now let's do the DP stored procedures
                m_lReturn = CType(GenerateStoredProcedure(r_vGISObject:=r_vGISObject, r_vGISProperty:=r_vGISProperty, v_sDatamodel:=m_sGISDataModel, r_oDatabase:=m_oDatabase, v_lPMProductFamily:=PMProductFamily, v_lGisDataModelType:=m_lGISDataModelType, v_lSwiftIntegration:=m_lSwiftIntegration, v_sUnderwritingOrAgency:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'now do any other data model related single stored procedures
                m_lReturn = CType(GenerateDMRelatedStoredProcedures(v_sDatamodel:=m_sGISDataModel, r_oDatabase:=m_oDatabase, v_lSwiftIntegration:=m_lSwiftIntegration), gPMConstants.PMEReturnCode)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return CInt(result)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()

            Return CInt(result)


            Return CInt(result)
        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: CreateDefaultObjects
    '
    ' Description:
    '
    ' History: 01/09/2000 Tomo - Created.
    '           TF021002 - Major Change to use GISObject & GISProperty objects
    '          20/02/03 - Pasted into branch code from Tip for Issue 1188 solution
    '                     Migration of 1.8.5 bug fixes t 1.8.6
    ' ***************************************************************** '
    Private Function CreateDefaultObjects(ByRef r_vGISObject(,) As Object, ByRef r_vGISProperty() As Object, ByVal v_lGisDataModelType As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim oGISObject As GISObject
        Dim colGISObjects As New ArrayList
        Dim lParentObjectID As Integer
        Dim sParentTableName As String = ""
        Dim r_vOption As String = ""
        Dim bIsBroking As Boolean
        Dim lObjectCount, lPropertyCount As Integer
        Dim lSectionObjectID As Integer
        Dim sSectionTableName As String = ""
        Dim sReferralParentTableName As String = ""
        Dim lObjectID As Integer = 0
        Dim sTableName As String = ""
        Dim lTempParentObjectID As Integer = 0
        Dim sTempTableName As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            'Check for Broking as extra Objects required
            m_lReturn = CType(bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, v_vBranch:=m_iSourceID, r_vUnderwriting:=r_vOption), gPMConstants.PMEReturnCode)
            bIsBroking = r_vOption = "A"

            'Policy Binder
            colGISObjects = New ArrayList()

            lObjectCount = 1
            oGISObject = New GISObject()
            With oGISObject
                .ID = lObjectCount
                .Database = m_oDatabase
                .GISDataModelID = m_lGISDataModelID
                .GISDataModel = m_sGISDataModel
                .ObjectName = m_sGISDataModel & "_Policy_Binder"
                .TableName = m_sGISDataModel & "_Policy_Binder"
                .GISDataModelType = ACPolicyBinder
                .IsBrokingObject = bIsBroking
                'Now add to database to get the ID
                m_lReturn = .AddToDatabase()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    'And destroy the objects - GISProperties are destroyed within GISObjects
                    For Each oGISObject2 As GISObject In colGISObjects
                        oGISObject = oGISObject2
                        oGISObject.Dispose()
                        colGISObjects.Remove(1)
                    Next oGISObject2
                    Return result
                End If
                'Store as parent object
                lParentObjectID = .GISObjectID
                sParentTableName = .TableName
                'Add to collection
                colGISObjects.Add(oGISObject)
            End With
            oGISObject = Nothing

            'create the default objects for a data model type

            Select Case v_lGisDataModelType
                ' Developer Guide No. 123
                Case SSP.Shared.GISDataModelType.GISDMTypeRisk

                    If bIsBroking Then
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Policy"
                            .TableName = m_sGISDataModel & "_Policy"
                            .MaxInstances = 100
                            .GISDataModelType = ACPolicy
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing
                    End If

                    'Plico 24-28
                    If Not bIsBroking Then
                        'S4IDEFAULT
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = "S4IDEFAULT"
                            .TableName = m_sGISDataModel & "_S4IDefault"
                            .MaxInstances = 100
                            .GISDataModelType = ACRiskS4IDefault
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = bIsBroking
                            .IsSelectableForScreen = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing
                    End If

                    'Output Object
                    lObjectCount += 1
                    oGISObject = New GISObject()
                    With oGISObject
                        .ID = lObjectCount
                        .Database = m_oDatabase
                        .GISDataModelID = m_lGISDataModelID
                        .GISDataModel = m_sGISDataModel
                        'Tomo151002
                        .ObjectName = m_sGISDataModel & "_Output"
                        '                .ObjectName = "Output"
                        .TableName = m_sGISDataModel & "_Output"
                        .MaxInstances = 100
                        .GISDataModelType = v_lGisDataModelType
                        .ParentObjectID = lParentObjectID
                        .ParentTableName = sParentTableName
                        .IsBrokingObject = bIsBroking
                        'Now add to database to get the ID
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                            'And destroy the objects - GISProperties are destroyed within GISObjects
                            For Each oGISObject2 As GISObject In colGISObjects
                                oGISObject = oGISObject2
                                oGISObject.Dispose()
                                colGISObjects.Remove(1)
                            Next oGISObject2
                            Return result
                        End If
                        'Store as parent object
                        lTempParentObjectID = .GISObjectID  'this is the parent for the rest of the tables
                        sTempTableName = .TableName
                        'Add to collection
                        colGISObjects.Add(oGISObject)
                    End With
                    oGISObject = Nothing

                    'Generic : CreateDefaultObjectTable
                    If Not m_bIsMarketplaceDM AndAlso Not m_bIsImportedMarketplaceDM Then
                        CreateDefaultObjectsTable(oGISObject, colGISObjects, lObjectCount, m_oDatabase, m_lGISDataModelID, m_sGISDataModel, "_Output_PremiumBreakdown", 100, ACOutputPremiumBreakdown, lParentObjectID, sParentTableName, False, CreateDefaultObjects, lObjectID, sTableName)
                        CreateDefaultObjectsTable(oGISObject, colGISObjects, lObjectCount, m_oDatabase, m_lGISDataModelID, m_sGISDataModel, "_Output_Commission", 100, ACOutputCommission, lParentObjectID, sParentTableName, False, CreateDefaultObjects, lObjectID, sTableName)
                        CreateDefaultObjectsTable(oGISObject, colGISObjects, lObjectCount, m_oDatabase, m_lGISDataModelID, m_sGISDataModel, "_Output_Referrals", 100, ACOutputReferrals, lParentObjectID, sParentTableName, False, CreateDefaultObjects, lObjectID, sTableName)
                        CreateDefaultObjectsTable(oGISObject, colGISObjects, lObjectCount, m_oDatabase, m_lGISDataModelID, m_sGISDataModel, "_Output_Referrals_Audit", 100, ACOutputReferralsAudit, lObjectID, sTableName, False, CreateDefaultObjects, lObjectID, sTableName)
                    End If
                    lParentObjectID = lTempParentObjectID
                    sParentTableName = sTempTableName


                    If m_bIsMarketplaceDM Then
                        'Output_Details Object
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Output_Details"
                            .TableName = m_sGISDataModel & "_Output_Details"
                            .MaxInstances = 100
                            .GISDataModelType = ACOutputDetails
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing

                        'Output_Endorsements Object
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Output_Endorsements"
                            .TableName = m_sGISDataModel & "_Output_Endorsements"
                            .MaxInstances = 100
                            .GISDataModelType = ACOutputEndorsements
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing

                        'Output_Excess Object
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Output_Excess"
                            .TableName = m_sGISDataModel & "_Output_Excess"
                            .MaxInstances = 100
                            .GISDataModelType = ACOutputExcess
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing

                        'Output_Fees Object
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Output_Fees"
                            .TableName = m_sGISDataModel & "_Output_Fees"
                            .MaxInstances = 100
                            .GISDataModelType = ACOutputFees
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing

                        'MKW 250506 Start - Datasure Section Changes
                        'Output_Sections Object
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Output_Sections"
                            .TableName = m_sGISDataModel & "_Output_Sections"
                            .MaxInstances = 100
                            .GISDataModelType = ACOutputSections
                            .ParentObjectID = lParentObjectID
                            .ParentTableName = sParentTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If

                            lSectionObjectID = .GISObjectID  'this is the parent for the rest of the tables
                            sSectionTableName = .TableName

                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing

                        'MKW 250506 Start - Datasure Section Changes
                        'Output_Sections Object
                        lObjectCount += 1
                        oGISObject = New GISObject()
                        With oGISObject
                            .ID = lObjectCount
                            .Database = m_oDatabase
                            .GISDataModelID = m_lGISDataModelID
                            .GISDataModel = m_sGISDataModel
                            .ObjectName = m_sGISDataModel & "_Output_Sections_Coinsurers"
                            .TableName = m_sGISDataModel & "_Output_Sections_Coinsurers"
                            .MaxInstances = 100
                            .GISDataModelType = ACOutputSectionsCoinsurers
                            .ParentObjectID = lSectionObjectID
                            .ParentTableName = sSectionTableName
                            .IsBrokingObject = True
                            'Now add to database to get the ID
                            m_lReturn = .AddToDatabase()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                'And destroy the objects - GISProperties are destroyed within GISObjects
                                For Each oGISObject2 As GISObject In colGISObjects
                                    oGISObject = oGISObject2
                                    oGISObject.Dispose()
                                    colGISObjects.Remove(1)
                                Next oGISObject2
                                Return result
                            End If
                            'Add to collection
                            colGISObjects.Add(oGISObject)
                        End With
                        oGISObject = Nothing
                        'MKW 250506 End   - Datasure Section Changes
                    End If
                    '========================================================

                    ' Developer Guide No. 123
                Case SSP.Shared.GISDataModelType.GISDMTypeClaim
                    '*************************************
                    ' Output Object ************
                    '*************************************
                    lObjectCount += 1
                    oGISObject = New GISObject()
                    With oGISObject
                        .ID = lObjectCount
                        .Database = m_oDatabase
                        .GISDataModelID = m_lGISDataModelID
                        .GISDataModel = m_sGISDataModel
                        .ObjectName = m_sGISDataModel & "_Output"
                        .TableName = m_sGISDataModel & "_Output"
                        .MaxInstances = 100

                        ' default to risk so the properties get added
                        .GISDataModelType = ACClaimsOutput
                        .ParentObjectID = lParentObjectID
                        .ParentTableName = sParentTableName
                        .IsBrokingObject = bIsBroking
                        'Now add to database to get the ID
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                            'And destroy the objects - GISProperties are destroyed within GISObjects
                            For Each oGISObject2 As GISObject In colGISObjects
                                oGISObject = oGISObject2
                                oGISObject.Dispose()
                                colGISObjects.Remove(1)
                            Next oGISObject2
                            Return result
                        End If

                        'Add to collection
                        colGISObjects.Add(oGISObject)
                    End With
                    oGISObject = Nothing

                    '*************************************
                    ' Claim Output Object ************
                    '*************************************

                    'Claim Output Object
                    '        lObjectCount = lObjectCount + 1
                    '        Set oGISObject = New GISObject
                    '        With oGISObject
                    '
                    '            ' this is used later on in the processing to determine
                    '            ' which properties need to be added, therefore we need
                    '            ' to reset this ( to the object we are creating  rather than
                    '            ' for the data model type ) so we can set the properties correctly
                    '            .GISDataModelType = ACClaimsDocFolder
                    '            '.IsNonGIS = GISOTClaimOutput
                    '            .ID = lObjectCount
                    '            .Database = m_oDatabase
                    '            .GISDataModelID = m_lGISDataModelID
                    '            .GISDataModel = m_sGISDataModel
                    '            .ObjectName = m_sGISDataModel & "_Claim_Output"
                    '            .TableName = m_sGISDataModel & "_Claim_Output"
                    '            .MaxInstances = 1
                    '            .ParentObjectID = lParentObjectID
                    '            .ParentTableName = sParentTableName
                    '            .IsBrokingObject = bIsBroking
                    '
                    '            'Now add to database to get the ID
                    '            m_lReturn = .AddToDatabase
                    '                If (m_lReturn <> PMTrue) Then
                    '                    CreateDefaultObjects = PMFalse
                    '                    LogMessage m_sUsername, _
                    ''                        iType:=PMLogOnError, _
                    ''                        sMsg:="Failed to Add Object " & .ObjectName & " to database.", _
                    ''                        vApp:=ACApp, _
                    ''                        vClass:=ACClass, _
                    ''                        vMethod:="CreateDefaultObjects"
                    '                    m_lReturn = RollbackTrans()
                    '                    'And destroy the objects - GISProperties are destroyed within GISObjects
                    '                    For Each oGISObject In colGISObjects
                    '                        oGISObject.Terminate
                    '                        colGISObjects.Remove 1
                    '                    Next oGISObject
                    '                    Exit Function
                    '                End If
                    '
                    '            'Store as parent object
                    '            lParentObjectID = .GISObjectID
                    '
                    '            '**********
                    '            ' MEvans : 03-09-2003 :
                    '            ' This was using wrong tablename which mean document request
                    '            ' generation foriegn key relates to policy binder instead of
                    '            ' claim_output
                    '            sParentTableName = .TableName
                    '            '**********
                    '
                    '            'Add to collection
                    '            colGISObjects.Add oGISObject
                    '        End With
                    '
                    '        Set oGISObject = Nothing
                    ' Developer Guide No. 123
                    'Case GISDataModelType.GISDMTypeCase
                Case SSP.Shared.GISDataModelType.GISDMTypeCase
                    lObjectCount += 1
                    oGISObject = New GISObject()
                    With oGISObject
                        .ID = lObjectCount
                        .Database = m_oDatabase
                        .GISDataModelID = m_lGISDataModelID
                        .GISDataModel = m_sGISDataModel
                        .ObjectName = "GENERAL"
                        .TableName = m_sGISDataModel & "_General"
                        .MaxInstances = 100
                        .GISDataModelType = ACCaseGeneral
                        .ParentObjectID = lParentObjectID
                        .ParentTableName = sParentTableName
                        .IsBrokingObject = bIsBroking
                        .IsSelectableForScreen = True
                        ' Developer Guide No. 123
                        '.IsNonGIS = GISDataModelType.GISOTCase
                        .IsNonGIS = SSP.Shared.GISDataModelType.GISOTCase
                        'Now add to database to get the ID
                        m_lReturn = .AddToDatabase()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                            'And destroy the objects - GISProperties are destroyed within GISObjects
                            For Each oGISObject2 As GISObject In colGISObjects
                                oGISObject = oGISObject2
                                oGISObject.Dispose()
                                colGISObjects.Remove(1)
                            Next oGISObject2
                            Return result
                        End If
                        'Add to collection
                        colGISObjects.Add(oGISObject)
                    End With
                    oGISObject = Nothing


            End Select

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process CommitTransaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                'And destroy the objects - GISProperties are destroyed within GISObjects
                For Each oGISObject2 As GISObject In colGISObjects
                    oGISObject = oGISObject2
                    oGISObject.Dispose()
                    colGISObjects.Remove(1)
                Next oGISObject2
                Return result
            End If

            'Set permissions outside of the transaction
            For Each oGISObject In colGISObjects
                sSQL.Append("GRANT ALL ON " & oGISObject.TableName & " TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10))
            Next oGISObject
            If sSQL.ToString() <> "" Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL.ToString(), sSQLName:="Set permissions", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to grant permissions on database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
                    'And destroy the objects - GISProperties are destroyed within GISObjects
                    For Each oGISObject2 As GISObject In colGISObjects
                        oGISObject = oGISObject2
                        oGISObject.Dispose()
                        colGISObjects.Remove(1)
                    Next oGISObject2
                    Return result
                End If
            End If

            'Now re-populate the arrays
            ReDim r_vGISProperty(colGISObjects.Count - 1)
            ReDim r_vGISObject(pbObjectAndPropertyConsts.ACOLastElement, colGISObjects.Count - 1)
            lObjectCount = 0
            'Object array
            For Each oGISObject In colGISObjects
                With oGISObject

                    r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lObjectCount) = .GISObjectID

                    r_vGISObject(pbObjectAndPropertyConsts.ACOGISDataModelId, lObjectCount) = .GISDataModelID

                    r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lObjectCount) = .ObjectName

                    r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lObjectCount) = .TableName

                    r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lObjectCount) = .MaxInstances

                    r_vGISObject(pbObjectAndPropertyConsts.ACOIsQuoteObject, lObjectCount) = .IsQuoteObject

                    r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lObjectCount) = .ParentObjectID


                    r_vGISObject(pbObjectAndPropertyConsts.ACOPolarisObjectId, lObjectCount) = .PolarisObjectID

                    r_vGISObject(pbObjectAndPropertyConsts.ACOIsSelectableForScreen, lObjectCount) = .IsSelectableForScreen

                    r_vGISObject(pbObjectAndPropertyConsts.ACOIsNonGIS, lObjectCount) = .IsNonGIS

                    r_vGISObject(pbObjectAndPropertyConsts.ACOEditFlags, lObjectCount) = .EditFlags

                    'Now do property array
                    Dim vArray(pbObjectAndPropertyConsts.ACPLastElement, .GISProperties.Count - 1) As Object
                    lPropertyCount = 0
                    For Each oGISProperty As GISProperty In .GISProperties
                        With oGISProperty

                            vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lPropertyCount) = .GISPropertyID

                            vArray(pbObjectAndPropertyConsts.ACPGISObjectId, lPropertyCount) = .GISObjectID

                            vArray(pbObjectAndPropertyConsts.ACPPropertyName, lPropertyCount) = .PropertyName

                            vArray(pbObjectAndPropertyConsts.ACPColumnName, lPropertyCount) = .ColumnName

                            vArray(pbObjectAndPropertyConsts.ACPDataType, lPropertyCount) = .GISDataType

                            vArray(pbObjectAndPropertyConsts.ACPIsInputProperty, lPropertyCount) = .IsInputProperty

                            vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lPropertyCount) = .IsIdentifyingProperty

                            vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lPropertyCount) = .IsPrimaryKey


                            vArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, lPropertyCount) = .PolarisPropertyID

                            vArray(pbObjectAndPropertyConsts.ACPIsDeleted, lPropertyCount) = .IsDeleted

                            vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lPropertyCount) = .IsSearchProperty


                            vArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, lPropertyCount) = .IndexLinkingID

                            vArray(pbObjectAndPropertyConsts.ACPEditFlags, lPropertyCount) = .EditFlags

                            vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lPropertyCount) = .SpecialsType


                            vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lPropertyCount) = .SpecialsTypeReference
                            lPropertyCount += 1
                        End With
                    Next oGISProperty

                    r_vGISProperty(lObjectCount) = vArray
                    lObjectCount += 1
                End With
            Next oGISObject

            'And destroy the objects - GISProperties are destroyed within GISObjects
            For Each oGISObject2 As GISObject In colGISObjects
                oGISObject = oGISObject2
                oGISObject.Dispose()
                colGISObjects.Remove(1)
            Next oGISObject2

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateDefaultObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'And destroy the objects - GISProperties are destroyed within GISObjects
            For Each oGISObject2 As GISObject In colGISObjects
                oGISObject = oGISObject2
                oGISObject.Dispose()
                colGISObjects.Remove(1)
            Next oGISObject2

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateSearchIndexes
    '
    ' Description:
    '
    ' History: 11/10/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateSearchIndexes(ByRef r_vGISObject(,) As Object, ByRef r_vGISProperty() As Object) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim vResultArray(,) As Object
        Dim sSQL, sKeyName As String



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Informations.IsArray(r_vGISObject) Then
            Return result
        End If

        For lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)



            ' Developer Guide No. 286
            vArray = r_vGISProperty(lTemp).Clone()

            If Informations.IsArray(vArray) Then

                For lTemp2 As Integer = vArray.GetLowerBound(1) To vArray.GetUpperBound(1)
                    'The premise is - if the key exists, leave it.  If it doesn't and we need
                    'it, then create it.
                    'Use ids as names might get too long
                    '<Pankaj PN:38991>


                    ' Developer Guide No.287 
                    If CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)) <> "dElEtEd" And NullToDouble(vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2)) <> -1 Then


                        ' Developer Guide No. 287
                        If (NullToDouble(vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lTemp2)) = 1) Or (NullToDouble(vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2)) = 3) Then  'Pk


                            sKeyName = "AK_GIS_Search_" & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) & "_" & CStr(vArray(pbObjectAndPropertyConsts.ACPGISPropertyId, lTemp2))

                            sSQL = "SELECT id FROM sysindexes WHERE name = '" & sKeyName & "'"

                            vResultArray = Nothing

                            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetIndex", bStoredProcedure:=False, vResultArray:=vResultArray)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If Not Informations.IsArray(vResultArray) Then
                                'Not one there - create it


                                sSQL = "CREATE INDEX " & sKeyName &
                                       " ON dbo." & CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOTableName, lTemp)) &
                                       "(" & CStr(vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2)) & ")"

                                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddIndex", bStoredProcedure:=False)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If

                            End If
                        End If
                    End If
                Next lTemp2
            End If

        Next lTemp

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateSumInsured
    '
    ' Description:
    '
    ' History: 18/10/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateSumInsured() As Integer

        Dim result As Integer = 0
        ' Developer Guide No. 17
        Dim vResultArray(,) As Object
        Dim sSQL As String



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT id FROM sysobjects WHERE name = '" & m_sGISDataModel & "_sum_insured'" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND xtype = 'U'" & Strings.ChrW(13) & Strings.ChrW(10)

        vResultArray = Nothing

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSumInsured", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResultArray) Then
            vResultArray = Nothing
            Return result
        End If

        sSQL = ""
        sSQL = sSQL & "CREATE TABLE " & m_sGISDataModel & "_sum_insured (" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id int not null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sum_insured_type_id int not null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sequence_id int not null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "description varchar(255) null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "reference varchar(20) null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sum_insured numeric(19,4) null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "date_added datetime null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "date_deleted datetime null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "is_valuation_required tinyint null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "valuation_date datetime null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "rate decimal(7,4) null," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "premium numeric(19,4) null)" & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddSumInsured", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sSQL = "ALTER TABLE " & m_sGISDataModel & "_sum_insured" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "ADD PRIMARY KEY CLUSTERED (" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sum_insured_type_id," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sequence_id)" & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddSumInsured", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sSQL = "CREATE INDEX XIF1" & m_sGISDataModel & "_sum_insured ON " & m_sGISDataModel & "_sum_insured(sum_insured_type_id)"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddSumInsured", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sSQL = "CREATE INDEX XIF2" & m_sGISDataModel & "_sum_insured ON " & m_sGISDataModel & "_sum_insured(" & m_sGISDataModel & "_Policy_binder_id)"

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddSumInsured", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sSQL = "ALTER TABLE " & m_sGISDataModel & "_sum_insured ADD " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Foreign Key(" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "REFERENCES " & m_sGISDataModel & "_Policy_binder (" & Strings.ChrW(13) & Strings.ChrW(10)
        ' RAW 02/09/2003 : CQ2158 : added delete cascade
        sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id)" & GetDeleteCascadeText() & "," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "Foreign Key(" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sum_insured_type_id)" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "REFERENCES sum_insured_type (" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "sum_insured_type_id)" & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddSumInsured", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add the permissions
        sSQL = "GRANT ALL ON " & m_sGISDataModel & "_sum_insured TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddSumInsured", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateStandardWording
    '
    ' Description:
    '
    ' History: 29/11/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function GenerateStandardWording(ByVal vParentKeys() As Object) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim sSQL As String
        Dim lLower, lUpper As Integer
        Dim sPrimaryKey As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = "SELECT id FROM sysobjects WHERE name = '" & m_sGISDataModel & "_standard_wording'" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND xtype = 'U'" & Strings.ChrW(13) & Strings.ChrW(10)

        vResultArray = Nothing

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetStandardWording", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            sSQL = ""
            sSQL = sSQL & "CREATE TABLE " & m_sGISDataModel & "_standard_wording (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id int not null," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "sequence_id int not null," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "document_template_id int null," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "gis_property_id int not null," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "gis_object_id int not null)" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "ALTER TABLE " & m_sGISDataModel & "_standard_wording" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ADD PRIMARY KEY CLUSTERED (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "sequence_id, gis_property_id, gis_object_id)" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "CREATE INDEX XIF1" & m_sGISDataModel & "_standard_wording ON " & m_sGISDataModel & "_standard_wording(document_template_id)"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "CREATE INDEX XIF2" & m_sGISDataModel & "_standard_wording ON " & m_sGISDataModel & "_standard_wording(" & m_sGISDataModel & "_Policy_binder_id)"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "ALTER TABLE " & m_sGISDataModel & "_standard_wording ADD " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Foreign Key(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "REFERENCES " & m_sGISDataModel & "_Policy_binder (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & m_sGISDataModel & "_Policy_binder_id)" & GetDeleteCascadeText() & "," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Foreign Key(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "document_template_id)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "REFERENCES document_template (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "document_template_id)," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Foreign Key(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "gis_property_id,gis_object_id)" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "REFERENCES gis_property(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "gis_property_id,gis_object_id)" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the permissions
            sSQL = "GRANT ALL ON " & m_sGISDataModel & "_standard_wording TO PUBLIC" & Strings.ChrW(13) & Strings.ChrW(10)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'Add the parent child flag
        sSQL = "EXEC DDLAddColumn '" & m_sGISDataModel & "_standard_wording', 'child','TINYINT NULL'"
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWordingKeys", bStoredProcedure:=False)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vParentKeys) Then
            lLower = vParentKeys.GetLowerBound(0)
            lUpper = vParentKeys.GetUpperBound(0)

            For lKey As Integer = lLower To lUpper

                sSQL = "EXEC DDLAddColumn '" & m_sGISDataModel & "_standard_wording', '" & CStr(vParentKeys(lKey)) & "','INT NULL'"

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWordingKeys", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If (sPrimaryKey.ToString().IndexOf(CStr(vParentKeys(lKey))) + 1) = 0 Then

                    sPrimaryKey.Append("," & CStr(vParentKeys(lKey)))
                End If
            Next lKey

            'Remove the Primary Key
            sSQL = "EXEC DDLDropPrimaryKey '" & m_sGISDataModel & "_standard_wording" & "'"

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="AddStandardWording", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateGisQemUsage
    '
    ' Description:
    '
    ' History: 06/02/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function CreateGisQemUsage() As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        'First, find if a record already exists
        'As far as S4U is concerned we don't care what the business type and scheme are...

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_data_model_id", vValue:=CStr(m_lGISDataModelID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_business_type_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_scheme_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGisQemUsageSQL, sSQLName:=ACGetGisQemUsageName, bStoredProcedure:=ACGetGisQemUsageStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vArray) Then
            Return result
        End If

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_data_model_id", vValue:=CStr(m_lGISDataModelID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_business_type_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_scheme_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_QEM_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddGisQemUsageSQL, sSQLName:=ACAddGisQemUsageName, bStoredProcedure:=ACAddGisQemUsageStored)

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
    Public Function BeginTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: addPropertyAddParameter
    '
    ' Description:
    '
    ' History: 08/03/2002 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub addPropertyAddParameter(ByRef sSQL As String, ByRef r_vGISObject(,) As Object, ByVal lTemp As Integer, ByRef vArray(,) As Object, ByVal lTemp2 As Integer, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "")

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addPropertyAddParameter")


        'GSD Deleted old references and added new ones
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "gis_object_id", r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "property_name", vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "column_name", vArray(pbObjectAndPropertyConsts.ACPColumnName, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "data_type", vArray(pbObjectAndPropertyConsts.ACPDataType, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_input_property", vArray(pbObjectAndPropertyConsts.ACPIsInputProperty, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_identifying_property", vArray(pbObjectAndPropertyConsts.ACPIsIdentifyingProperty, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_primary_key", vArray(pbObjectAndPropertyConsts.ACPIsPrimaryKey, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "polaris_property_id", vArray(pbObjectAndPropertyConsts.ACPPolarisPropertyId, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_deleted", vArray(pbObjectAndPropertyConsts.ACPIsDeleted, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_search_property", vArray(pbObjectAndPropertyConsts.ACPIsSearchProperty, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "index_linking_id", vArray(pbObjectAndPropertyConsts.ACPIndexLinkingId, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "Edit_Flags", vArray(pbObjectAndPropertyConsts.ACPEditFlags, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "Specials_Type", vArray(pbObjectAndPropertyConsts.ACPSpecialsType, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        ' Developer Guide No. 17
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "Specials_Type_Reference", Convert.ToString(vArray(pbObjectAndPropertyConsts.ACPSpecialsTypeReference, lTemp2)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_in_mis_export", vArray(pbObjectAndPropertyConsts.ACPIsInMISExport, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_formatted_text", vArray(pbObjectAndPropertyConsts.ACPIsFormattedText, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_chase_cycle_property", vArray(pbObjectAndPropertyConsts.ACPIsChaseCycleProperty, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "is_claim360display", vArray(pbObjectAndPropertyConsts.ACPISClaim360Display, lTemp2), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy & $"/Property({CStr(vArray(pbObjectAndPropertyConsts.ACPPropertyName, lTemp2)).Trim()})", gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addPropertyAddParameter")


    End Sub

    ' ***************************************************************** '
    '
    ' Name: IsParentObjectMultipleInstance
    '
    ' Description:
    '
    ' History: 02/05/2002 CLG - Created.
    '
    ' ***************************************************************** '

    'Private Function IsParentObjectMultipleInstance(ByVal v_sObjectName As String, ByRef r_vGISObject( ,  ) As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim lParentId As Integer
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")
    '
    'Try 
    '
    '
    'lParentId = -1
    '
    'If Informations.IsArray(r_vGISObject) Then
    'For 'lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)

    'If CStr(r_vGISObject(pbObjectAndPropertyConsts.ACOObjectName, lTemp)) = v_sObjectName Then

    'lParentId = CInt(r_vGISObject(pbObjectAndPropertyConsts.ACOParentObjectId, lTemp))
    'Exit For
    'End If
    'Next 
    'End If
    '
    'If lParentId <> -1 And Informations.IsArray(r_vGISObject) Then
    'For 'lTemp As Integer = r_vGISObject.GetLowerBound(1) To r_vGISObject.GetUpperBound(1)


    'If CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOGISObjectId, lTemp)) = lParentId And CDbl(r_vGISObject(pbObjectAndPropertyConsts.ACOMaxInstances, lTemp)) > 1 Then
    'Return 1
    'End If
    'Next 
    'End If
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".IsObjectMultipleInstance")
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsParentObjectMultipleInstance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsParentObjectMultipleInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: addObject
    '
    ' Description:
    '
    ' History: 17/12/2002 CLG - Created.
    '
    ' ***************************************************************** '

    'Private Function addObject(ByVal v_sName As String, ByVal v_bIsParent As Boolean, ByVal v_lMaxInstances As Integer, ByVal v_lGisDataModelType As Integer, ByVal v_bIsSelectableForScreen As Boolean, ByRef r_lParentObjectID As Integer, ByRef r_sParentTableName As String, ByVal v_bIsBroking As Boolean, ByRef r_lObjectCount As Integer, ByRef colGISObjects As Collection, ByVal v_bIsNonGIS As Object) As Integer
    '
    ' Debug message
    'Dim result As Integer = 0
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".addObject")
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim oGISObject As GISObject
    '
    'r_lObjectCount += 1
    'oGISObject = New GISObject()
    'With oGISObject
    '.ID = r_lObjectCount
    '.Database = m_oDatabase
    '.GISDataModelID = m_lGISDataModelID
    '.GISDataModel = m_sGISDataModel
    '.ObjectName = m_sGISDataModel & "_" & v_sName
    '.TableName = m_sGISDataModel & "_" & v_sName
    '.MaxInstances = v_lMaxInstances
    '.GISDataModelType = v_lGisDataModelType
    '.ParentObjectID = r_lParentObjectID
    '.ParentTableName = r_sParentTableName
    '.IsBrokingObject = v_bIsBroking
    '.IsSelectableForScreen = v_bIsSelectableForScreen

    '.IsNonGIS = CInt(v_bIsNonGIS)
    'Now add to database to get the ID
    'm_lReturn = .AddToDatabase()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjects")
    'm_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
    'And destroy the objects - GISProperties are destroyed within GISObjects
    'For	Each oGISObject2 As GISObject In colGISObjects
    'oGISObject = oGISObject2
    'oGISObject.Terminate()
    'colGISObjects.Remove(1)
    'Next oGISObject2
    'Return result
    'End If
    'Store as parent object
    'If v_bIsParent Then
    'r_lParentObjectID = .GISObjectID
    'r_sParentTableName = .ParentTableName
    'End If
    'Add to collection
    'colGISObjects.Add(oGISObject)
    'End With
    'oGISObject = Nothing
    '
    '
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".addObject")
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".addObject")
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="addObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="addObject", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: FixupSqlAndReRun
    '
    ' Description:
    '
    ' History: 26/02/2003 CLG - Created.
    '
    ' ***************************************************************** '

    'Private Function FixupSqlAndReRun(ByVal v_sSQL As String, ByVal v_sTitle As String, ByVal v_sOriginal As String, ByVal v_sReplacement As String) As Integer
    '
    ' Debug message
    'Dim result As Integer = 0
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".FixupSqlAndReRun")
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'v_sSQL = v_sSQL.Replace(v_sOriginal, v_sReplacement)
    'result = m_oDatabase.SQLAction(sSQL:=v_sSQL, sSQLName:=v_sTitle, bStoredProcedure:=False)
    'If result <> gPMConstants.PMEReturnCode.PMTrue Then
    'RollbackTrans()
    'End If
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".FixupSqlAndReRun")
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Debug message
    'Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".FixupSqlAndReRun")
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FixupSqlAndReRun Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FixupSqlAndReRun", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    Public Function GetSwiftSpecialListTypes(ByRef r_vListTypesArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not (m_oiSWSirius Is Nothing) Then

                m_lReturn = m_oiSWSirius.GetSpecialListTypes(vListTypesArray:=CType(r_vListTypesArray, Object))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to swift special list types", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSwiftSpecialListTypes")
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSwiftSpecialListTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSwiftSpecialListTypes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         RecreateDatasets
    '
    ' Description:  deletes and recreates DSD and DS
    '
    ' Author:       13/05/2005 CLG
    '
    ' ***************************************************************** '
    Public Function RecreateDatasets(ByVal v_sGisDataModelCode As String) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim o_bGIS As Object 'bGIS.Application
        Try

            'Get system option CCMDocProduction
            Dim sCCMDocProduction As String = String.Empty
            Dim bRecreateDataBackBone As Boolean = False
            result = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem, r_sOptionValue:=sCCMDocProduction)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sCCMDocProduction = "1" Then
                bRecreateDataBackBone = True
            End If
            'o_bGIS = New bGIS.Application()
            o_bGIS = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=o_bGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRRiskScreen.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If
            result = o_bGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                result = o_bGIS.RecreateDatasets(ToSafeString(v_sGisDataModelCode), ToSafeBoolean(bRecreateDataBackBone))
            End If

            o_bGIS = Nothing

            Return result

        Catch ex As Exception



            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to run bGIS.RecreateDatasets", vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", excep:=ex)


            Return result


        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : GetDataModelSearchFields
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '

    Public Function GetDataModelSearchFields(ByVal v_lGisDataModelID As Integer, ByRef r_vResultArray(,) As Object) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "GetDataModelSearchFields"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_data_model_id", v_lGisDataModelID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetGISDataModelSearchFieldsSQL, sSQLName:=ACGetGISDataModelSearchFieldsName, bStoredProcedure:=True, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetGISDataModelSearchFieldsName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Informations.IsArray(r_vResultArray) Then
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
    ' Name          : GetSearchFieldsSQL
    ' Description   :
    ' History       : VB
    ' ***************************************************************** '
    Public Function GetSearchFieldsSQL(ByVal cSearchFields As ArrayList, ByRef r_sSQLJoins As String, ByRef r_sSQLWhere As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSearchFieldsSQL"

        'Const kGISPropertyId As Integer = 0
        Const kPropertyName As Integer = 1
        Const kValue As Integer = 2
        'Const kGISObjectId As Integer = 3
        Const kDataType As Integer = 4
        'Const kSpecialType As Integer = 5
        'Const kSpecialTypeRef As Integer = 6
        Const kGisObjectTableName As Integer = 7
        Const kGISDataModelCode As Integer = 8

        Dim sSQLJoin As New StringBuilder
        Dim bIsWhere As Boolean
        Dim sTableName As String = ""
        Dim sColumnName As String = ""
        Dim vValue As String = ""
        Dim vSearchFields As Object = Nothing
        Dim sGISDataModelCode As String = ""
        Dim lCount As Integer

        Dim sTempSQLJoin As String = ""
        Dim sTempSQLJoin1 As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_sSQLJoins = ""
            r_sSQLWhere = ""
            sSQLJoin = New StringBuilder("")


            For lRow As Integer = 0 To cSearchFields.Count - 1



                vSearchFields = cSearchFields("R" & lRow)

                sGISDataModelCode = gPMFunctions.ToSafeString(CStr(vSearchFields(kGISDataModelCode, lRow))).Trim()

                sColumnName = gPMFunctions.ToSafeString(CStr(vSearchFields(kPropertyName, lRow))).Trim()


                If sTableName <> gPMFunctions.ToSafeString(CStr(vSearchFields(kGisObjectTableName, lRow))).Trim() Then
                    lCount += 1
                End If


                sTableName = gPMFunctions.ToSafeString(CStr(vSearchFields(kGisObjectTableName, lRow))).Trim()

                vValue = CStr(vSearchFields(kValue, lRow))
                If vValue.Trim().Length > 0 Then
                    If bIsWhere Then
                        r_sSQLWhere = r_sSQLWhere & " AND " & Strings.ChrW(13) & Strings.ChrW(10)
                    Else
                        r_sSQLWhere = r_sSQLWhere & " WHERE " & Strings.ChrW(13) & Strings.ChrW(10)
                        bIsWhere = True
                    End If


                    Select Case gPMFunctions.ToSafeLong(CStr(vSearchFields(kDataType, lRow)))
                        Case GISSharedConstants.GISDataTypeDate

                            r_sSQLWhere = r_sSQLWhere & "ISNULL(ot" & gPMFunctions.ToSafeString(CStr(lCount)).Trim() & "." & sColumnName & ",'') = '" &
                                          (gPMFunctions.ToSafeDate(vValue)) & "'" & Strings.ChrW(13) & Strings.ChrW(10)

                        Case GISSharedConstants.GISDataTypeNumeric

                            r_sSQLWhere = r_sSQLWhere & "ISNULL(ot" & gPMFunctions.ToSafeString(CStr(lCount)).Trim() & "." & sColumnName & ",'') = " & CStr(gPMFunctions.ToSafeLong(vValue)) & Strings.ChrW(13) & Strings.ChrW(10)

                        Case Else

                            r_sSQLWhere = r_sSQLWhere & "ISNULL(ot" & gPMFunctions.ToSafeString(CStr(lCount)).Trim() & "." & sColumnName & ",'') LIKE '%" & gPMFunctions.ToSafeString(vValue).Trim() & "%'" & Strings.ChrW(13) & Strings.ChrW(10)

                    End Select


                    sTempSQLJoin = "JOIN " & sTableName & " ot"
                    sTempSQLJoin1 = "ON pb." & sGISDataModelCode & "_policy_binder_id = " & "ot"

                    If Not (sSQLJoin.ToString().IndexOf(sTempSQLJoin) >= 0) And Not (sSQLJoin.ToString().IndexOf(sTempSQLJoin1) >= 0) Then
                        sSQLJoin.Append(" JOIN " & sTableName & " ot" & gPMFunctions.ToSafeString(CStr(lCount)).Trim() & Strings.ChrW(13) & Strings.ChrW(10))
                        sSQLJoin.Append("ON pb." & sGISDataModelCode & "_policy_binder_id = " & "ot" & gPMFunctions.ToSafeString(CStr(lCount)).Trim() & "." & sGISDataModelCode & "_policy_binder_id" & Strings.ChrW(13) & Strings.ChrW(10))
                    End If
                End If
            Next lRow

            r_sSQLJoins = " JOIN gis_policy_link gpl" & Strings.ChrW(13) & Strings.ChrW(10)
            r_sSQLJoins = r_sSQLJoins & "ON c.case_id = gpl.case_id" & Strings.ChrW(13) & Strings.ChrW(10)
            r_sSQLJoins = r_sSQLJoins & "LEFT JOIN " & sGISDataModelCode & "_policy_binder pb" & Strings.ChrW(13) & Strings.ChrW(10)
            r_sSQLJoins = r_sSQLJoins & "ON gpl.gis_policy_link_id = pb.gis_policy_link_id" & Strings.ChrW(13) & Strings.ChrW(10)

            r_sSQLJoins = r_sSQLJoins & sSQLJoin.ToString() & Strings.ChrW(13) & Strings.ChrW(10)

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function


    Private Function CreateDefaultObjectsTable(ByRef r_oGISObject As GISObject, ByRef r_colGISObjects As ArrayList, ByRef r_lObjectCount As Integer, ByRef r_oDatabase As Object, ByVal r_lGISDataModelID As Integer, ByVal r_sGISDataModel As String, ByVal v_sTableName As String, ByVal v_iMaxInstance As Integer, ByVal v_iGISDataModelType As Integer, ByVal r_lParentObjectID As Integer, ByVal v_sParentTableName As String, ByVal v_bIsBroking As Boolean, ByRef r_lCreateDefaultObjects As Long, ByRef r_lObjectID As Long, ByRef r_sTableName As String) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue



        r_lObjectCount += 1
        r_oGISObject = New GISObject()
        With r_oGISObject
            .ID = r_lObjectCount
            .Database = r_oDatabase
            .GISDataModelID = r_lGISDataModelID
            .GISDataModel = r_sGISDataModel
            .ObjectName = r_sGISDataModel & v_sTableName
            .TableName = r_sGISDataModel & v_sTableName
            .MaxInstances = v_iMaxInstance
            .GISDataModelType = v_iGISDataModelType
            .ParentObjectID = r_lParentObjectID
            .ParentTableName = v_sParentTableName
            .IsBrokingObject = v_bIsBroking
            m_lReturn = .AddToDatabase()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Object " & .ObjectName & " to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDefaultObjectsTable")
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                For Each oGISObject2 As GISObject In r_colGISObjects
                    r_oGISObject = oGISObject2
                    r_oGISObject.Dispose()
                    r_colGISObjects.Remove(1)
                Next
                Return result
            End If

            r_lObjectID = .GISObjectID
            r_sTableName = .TableName
            r_colGISObjects.Add(r_oGISObject)

        End With
        r_oGISObject = Nothing



        Return result



    End Function

    Public Function RebuildDefaultObjects(ByVal v_sPolicyBinderTable As String) As Integer

        Const kMethodName As String = "RebuildDefaultObjects"

        Dim Result As Integer = 0
        Dim vObjectResults(,) As Object = Nothing
        Dim sSQL As String
        Dim lRow As Long
        Dim lInnerRow As Long
        Dim sTableName As String
        Dim sGISDataModel As String
        Dim vRiskDefaultObjects(,) As Object = Nothing
        Dim lParentObjectID As Long
        Dim lTempParentObjectID As Long
        Dim sTempParentTableName As String = ""
        Dim sParentTableName As String
        Dim bTableExists As Boolean
        Dim oGISObject As GISObject
        Dim colGISObjects As ArrayList
        Dim lObjectCount As Long
        Dim lObjectID As Long
        Dim lGISDataModelID As Long
        Dim nTempChildParentObjectID As Integer = 0
        Dim sTempChildParentTableName As String = String.Empty

        Try

            If m_bIsMarketplaceDM AndAlso Not m_bIsImportedMarketplaceDM Then
                ReDim vRiskDefaultObjects(0 To 3, 0 To 6)
            Else
                ReDim vRiskDefaultObjects(0 To 2, 0 To 5)
            End If

            Result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = BeginTrans()

            bTableExists = False

            m_lReturn = GISTypeProperties("Risk", vRiskDefaultObjects)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Result = gPMConstants.PMEReturnCode.PMFalse
                Return Result
            End If


            sSQL = "SELECT GOBJ.table_name,GOBJ.gis_object_id,GOBJ.gis_data_model_id,GDM.gis_data_model_type_id,GDM.code "
            sSQL = sSQL + "FROM GIS_Object GOBJ "
            sSQL = sSQL + "INNER JOIN GIS_Data_Model GDM "
            sSQL = sSQL + "ON GDM.gis_data_model_id  = GOBJ.gis_data_model_id "
            sSQL = sSQL + "WHERE Upper(LTrim(RTrim(GOBJ.table_name))) = Upper(RTrim(LTrim('" + v_sPolicyBinderTable + "'))) "
            sSQL = sSQL + "AND GOBJ.parent_object_id IS NULL "




            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                        sSQLName:="Get SysObject Record",
                        bStoredProcedure:=False,
                        bKeepNulls:=True,
                        lNumberRecords:=gPMConstants.PMAllRecords,
                        vResultArray:=vObjectResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return Result
            End If


            colGISObjects = New ArrayList
            oGISObject = New GISObject

            ' _Policy_Binder is our starting point
            For lRow = vObjectResults.GetLowerBound(1) To vObjectResults.GetUpperBound(1)

                lObjectCount = 1
                sParentTableName = vObjectResults(0, lRow)
                lParentObjectID = vObjectResults(1, lRow)
                lGISDataModelID = vObjectResults(2, lRow)
                sGISDataModel = vObjectResults(4, lRow)
                sGISDataModel = Trim(sGISDataModel)



                ' Rebuild objects for Case, Claim, Party, Risk
                Select Case vObjectResults(3, lRow)
                    Case 1

                        For lInnerRow = vRiskDefaultObjects.GetLowerBound(1) To vRiskDefaultObjects.GetUpperBound(1)
                            If vRiskDefaultObjects(0, lInnerRow) IsNot Nothing Then
                                bTableExists = False
                                sTableName = sGISDataModel + vRiskDefaultObjects(0, lInnerRow)
                                m_lReturn = CheckIfObjectExists(sTableName, bTableExists)
                                lObjectCount = lObjectCount + 1


                                ' Table does not exists, create it
                                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And bTableExists = False Then

                                    If vRiskDefaultObjects(2, lInnerRow) = False Then
                                        m_lReturn = CreateDefaultObjectsTable(oGISObject, colGISObjects, lObjectCount, m_oDatabase, lGISDataModelID, sGISDataModel, vRiskDefaultObjects(0, lInnerRow), 100, vRiskDefaultObjects(1, lInnerRow), lParentObjectID, sParentTableName, False, 1, lObjectID, sTableName)
                                        lTempParentObjectID = lObjectID
                                        sTempParentTableName = sTableName
                                    Else

                                        If m_bIsMarketplaceDM AndAlso Not m_bIsImportedMarketplaceDM Then
                                            If _
                                                ToSafeBoolean(vRiskDefaultObjects(2, lInnerRow)) AndAlso
                                                Not ToSafeBoolean(vRiskDefaultObjects(3, lInnerRow)) Then
                                                m_lReturn = CreateDefaultObjectsTable(oGISObject, colGISObjects,
                                                                                      lObjectCount, m_oDatabase,
                                                                                      lGISDataModelID, sGISDataModel,
                                                                                      vRiskDefaultObjects(0, lInnerRow),
                                                                                      100,
                                                                                      vRiskDefaultObjects(1, lInnerRow),
                                                                                      lTempParentObjectID,
                                                                                      sTempParentTableName, False, 1,
                                                                                      lObjectID, sTableName)
                                                'This enhance to support grand child
                                                nTempChildParentObjectID = lObjectID
                                                sTempChildParentTableName = sTableName
                                                'if below condition true then it is grand child object
                                            ElseIf _
                                                vRiskDefaultObjects(2, lInnerRow) = True AndAlso
                                                vRiskDefaultObjects(3, lInnerRow) = True Then
                                                m_lReturn = CreateDefaultObjectsTable(oGISObject, colGISObjects,
                                                                                      lObjectCount, m_oDatabase,
                                                                                      lGISDataModelID, sGISDataModel,
                                                                                      vRiskDefaultObjects(0, lInnerRow),
                                                                                      100,
                                                                                      vRiskDefaultObjects(1, lInnerRow),
                                                                                      nTempChildParentObjectID,
                                                                                      sTempChildParentTableName, False,
                                                                                      1, lObjectID, sTableName)
                                            End If
                                        Else
                                            m_lReturn = CreateDefaultObjectsTable(oGISObject, colGISObjects,
                                                                                  lObjectCount, m_oDatabase,
                                                                                  lGISDataModelID, sGISDataModel,
                                                                                  vRiskDefaultObjects(0, lInnerRow), 100,
                                                                                  vRiskDefaultObjects(1, lInnerRow),
                                                                                  lTempParentObjectID,
                                                                                  sTempParentTableName, False, 1,
                                                                                  lObjectID, sTableName)
                                        End If
                                    End If
                                ElseIf bTableExists AndAlso Not ToSafeBoolean(vRiskDefaultObjects(2, lInnerRow)) Then
                                    sTempParentTableName = sTableName
                                    m_lReturn = GetObjectId(sTableName, lTempParentObjectID)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        RaiseError(kMethodName, "RebuildDefaultObjects  Failed for " & sTableName,
                                                   gPMConstants.PMELogLevel.PMLogError)
                                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                        Return Result
                                    End If
                                End If

                                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                                    Result = gPMConstants.PMEReturnCode.PMError
                                    RaiseError(kMethodName, "RebuildDefaultObjects  Failed for " & sTableName, gPMConstants.PMELogLevel.PMLogError)
                                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                    Return Result
                                End If
                            End If

                        Next
                    Case 2
                        ' Add Rebuild functionality if needed
                    Case 3
                        ' Add Rebuild functionality if needed
                    Case 4
                        ' Add Rebuild functionality if needed

                End Select

            Next


            m_lReturn = CommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                Result = gPMConstants.PMEReturnCode.PMError
                RaiseError(kMethodName, "Failed to commit transaction to database." & gPMConstants.PMELogLevel.PMLogError)
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return Result

            Else
                Return Result
            End If

        Catch ex As Exception

            Result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RebuildDefaultObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
            Return Result

        End Try

    End Function

    Private Function CheckIfObjectExists(ByVal v_lTableName As String, ByRef r_bTableExists As Boolean) As Long


        Dim Result As Integer = 0
        Dim vObjectResults(,) As Object = Nothing

        Result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String

        sSQL = "SELECT name FROM sysobjects where name = "
        sSQL = sSQL + " '" + v_lTableName + "' "
        sSQL = sSQL + "And xtype = 'U' "

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                    sSQLName:="CheckIfObjectExists",
                    bStoredProcedure:=False,
                    bKeepNulls:=True,
                    lNumberRecords:=gPMConstants.PMAllRecords,
                    vResultArray:=vObjectResults)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Result = gPMConstants.PMEReturnCode.PMFalse
            Return Result
        End If

        If Informations.IsArray(vObjectResults) Then
            r_bTableExists = True
        Else
            r_bTableExists = False
        End If

        Return Result
    End Function



    Private Function GISTypeProperties(ByVal v_GISType As String, ByRef r_vDefaultObjects As Object) As Long

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMFalse

        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Important : A Child should be immediately set after it's parent
        ' _Output_Referrals_Audit is a child of _Output_Referrals
        Select Case v_GISType

            Case "Risk"
                If m_bIsMarketplaceDM AndAlso Not m_bIsImportedMarketplaceDM Then
                    r_vDefaultObjects(0, 0) = "_Output"
                    r_vDefaultObjects(0, 1) = "_Output_Details"
                    r_vDefaultObjects(0, 2) = "_Output_Endorsements"
                    r_vDefaultObjects(0, 3) = "_Output_Excess"
                    r_vDefaultObjects(0, 4) = "_Output_Fees"
                    r_vDefaultObjects(0, 5) = "_Output_Sections"
                    r_vDefaultObjects(0, 6) = "_Output_Sections_Coinsurers"

                    r_vDefaultObjects(1, 0) = 1
                    r_vDefaultObjects(1, 1) = ACOutputDetails
                    r_vDefaultObjects(1, 2) = ACOutputEndorsements
                    r_vDefaultObjects(1, 3) = ACOutputExcess
                    r_vDefaultObjects(1, 4) = ACOutputFees
                    r_vDefaultObjects(1, 5) = ACOutputSections
                    r_vDefaultObjects(1, 6) = ACOutputSectionsCoinsurers

                    ' Is child, 
                    r_vDefaultObjects(2, 0) = False
                    r_vDefaultObjects(2, 1) = True
                    r_vDefaultObjects(2, 2) = True
                    r_vDefaultObjects(2, 3) = True
                    r_vDefaultObjects(2, 4) = True
                    r_vDefaultObjects(2, 5) = True
                    r_vDefaultObjects(2, 6) = True

                    ' Is grand child, 
                    r_vDefaultObjects(3, 0) = False
                    r_vDefaultObjects(3, 1) = False
                    r_vDefaultObjects(3, 2) = False
                    r_vDefaultObjects(3, 3) = False
                    r_vDefaultObjects(3, 4) = False
                    r_vDefaultObjects(3, 5) = False
                    r_vDefaultObjects(3, 6) = True

                ElseIf Not m_bIsMarketplaceDM AndAlso Not m_bIsImportedMarketplaceDM Then
                    r_vDefaultObjects(0, 0) = "_S4IDefault"
                    r_vDefaultObjects(0, 1) = "_Output"
                    r_vDefaultObjects(0, 2) = "_Output_PremiumBreakdown"
                    r_vDefaultObjects(0, 3) = "_Output_Commission"
                    r_vDefaultObjects(0, 4) = "_Output_Referrals"
                    r_vDefaultObjects(0, 5) = "_Output_Referrals_Audit"


                    r_vDefaultObjects(1, 0) = ACRiskS4IDefault
                    r_vDefaultObjects(1, 1) = 1
                    r_vDefaultObjects(1, 2) = ACOutputPremiumBreakdown
                    r_vDefaultObjects(1, 3) = ACOutputCommission
                    r_vDefaultObjects(1, 4) = ACOutputReferrals
                    r_vDefaultObjects(1, 5) = ACOutputReferralsAudit

                    ' Is child, _Output_Referrals_Audit is child of _Output_Referrals
                    r_vDefaultObjects(2, 0) = False
                    r_vDefaultObjects(2, 1) = False
                    r_vDefaultObjects(2, 2) = False
                    r_vDefaultObjects(2, 3) = False
                    r_vDefaultObjects(2, 4) = False
                    r_vDefaultObjects(2, 5) = True
                End If
            Case "Case"
                ' Add Rebuild functionality if needed
            Case "Claim"
                ' Add Rebuild functionality if needed
            Case "Party"
                ' Add Rebuild functionality if needed
        End Select

        Return nResult


    End Function

    ''' <summary>
    ''' To update the data model whether this a Market Place data model or not by passing bMPDataModel parameter
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <param name="bIsMPDataModel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateMPDataModel(ByVal sDataModelCode As String, ByVal bIsMPDataModel As Boolean) As Integer
        Const kMethodName As String = "UpdateMPDataModel"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="sDataModelCode", vValue:=sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="bIsMPDataModel", vValue:=ToSafeInteger(bIsMPDataModel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            nReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateMPDataModelSQL, sSQLName:=ACUpdateMPDataModelName, bStoredProcedure:=ACUpdateMPDataModelStored)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACUpdateMPDataModelSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GISTypeProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nReturn
        End Try

    End Function

    ''' <summary>
    ''' Get the gis_object_id from gis_object table by passing object_name
    ''' </summary>
    ''' <param name="sObjectName"></param>
    ''' <param name="o_nObjectId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetObjectId(ByVal sObjectName As String, ByRef o_nObjectId As Integer) As Integer
        Const kMethodName As String = "GetObjectId"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse


        m_oDatabase.Parameters.Clear()

        nReturn = m_oDatabase.Parameters.Add(sName:="sObjectName", vValue:=sObjectName,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMString)

        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
            Return nReturn
        End If

        Dim dtResults As New DataTable
        nReturn = m_oDatabase.ExecuteDataTable(sSQL:=kGetObjectIdSQL, sSQLName:=kGetObjectIdName,
                                               bStoredProcedure:=kGetObjectIdStored, oRecordset:=dtResults)
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                               sMsg:=kGetObjectIdSQL & " Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:="")
            Return nReturn
        End If
        If dtResults IsNot Nothing AndAlso dtResults.Rows IsNot Nothing AndAlso dtResults.Rows.Count > 0 Then
            o_nObjectId = ToSafeInteger(dtResults.Rows(0).Item(0))
        End If
        Return nReturn
    End Function
End Class

