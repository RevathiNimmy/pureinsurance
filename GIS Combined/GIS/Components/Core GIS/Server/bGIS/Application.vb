Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Net.Mail
Imports System.Text
Imports System.Xml
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports Ionic.Zip
Imports System.Runtime.Caching
Imports SSP.Shared
Imports System.Runtime.ExceptionServices

<System.Runtime.InteropServices.ProgId("Application_NET.Application")>
Public NotInheritable Class Application
    Implements IDisposable

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Public vRecipient As Object
    Public vSubject As Object
    Public vTextBody As Object
    Public vMailbox As Object
    Public vServer As Object

    Private Const ACClass As String = "Application"

    Dim m_lReturn As gPMConstants.PMEReturnCode
    Private m_oDatabase As dPMDAO.Database
    Private m_oDataSet As cGISDataSetControl.Application
    Private m_oGISSchemeBusiness As bGISSchemeBusiness.Business
    Private m_bCloseDatabase As Boolean
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sAddSQL As String = ""
    Private m_sUpdateSQL As String = ""
    Private m_sDeleteSQL As String = ""
    Private m_sAddUpdateSQL As String = ""
    Private m_iFileNumber As Integer
    Private m_lXMLStrLength As Integer
    Private m_lIsMidnightRenewal As Integer
    Private m_lAllowPositiveCancellation As Integer
    Private m_lUnifiedRenewalDay As Integer
    Private m_sDataModelType As String
    Private m_bDataBackboneCreated As Boolean = False
    Protected Const DefaultDatasetKey As String = "DefaultDataset_"
    Public Shared iCache As MemoryCache

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property XMLStrLength() As Integer
        Get
            Return m_lXMLStrLength
        End Get
        Set(ByVal Value As Integer)
            m_lXMLStrLength = Value
        End Set
    End Property

    Public ReadOnly Property Risk() As cGISDataSetControl.Node
        Get
            Try
                Return m_oDataSet.Risk
            Catch excep As System.Exception
                Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message, excep)
            End Try
        End Get
    End Property

    Public Property DataBackboneCreated() As Boolean
        Get
            Return m_bDataBackboneCreated
        End Get
        Set(ByVal value As Boolean)
            m_bDataBackboneCreated = value
        End Set
    End Property

    Public Function PolicyLinkID() As Integer
        Dim result As Integer = 0
        Try
            result = -1
            Return m_oDataSet.PolicyLinkID()
        Catch excep As System.Exception
            result = -1
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PolicyLinkIDFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PolicyLinkID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ''' <summary>
    ''' Initialise
    ''' </summary>
    ''' <param name="sUserName"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUserName As String,
                               ByVal sPassword As String,
                               ByVal iUserID As Integer,
                               ByVal iSourceID As Integer,
                               ByVal iLanguageID As Integer,
                               ByVal iCurrencyID As Integer,
                               ByVal iLogLevel As Integer,
                               ByVal sCallingAppName As String,
                               Optional ByVal bStandAlone As Boolean = False,
                               Optional ByVal vDatabase As Object = Nothing) As Long

        Dim nResult As Integer = 0

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUserName
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

            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                m_oGISSchemeBusiness = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
            End If
            m_oDatabase = Nothing
        End If
        Me.disposedValue = True
    End Sub

    Public Function NewDataSet(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String) As Integer
        Return NewDataSet(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vInsuranceFileCnt:=0, r_sQuoteRef:="", r_sQuoteRefPassword:="", v_vRiskID:=0)
    End Function
    Public Function NewDataSet(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_vInsuranceFileCnt As Integer, ByVal v_vRiskID As Integer) As Integer
        Return NewDataSet(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vInsuranceFileCnt:=v_vInsuranceFileCnt, r_sQuoteRef:="", r_sQuoteRefPassword:="", v_vRiskID:=v_vRiskID)
    End Function
    Public Function NewDataSet(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_vInsuranceFileCnt As Integer, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String, ByVal v_vRiskID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecordsAffected As Integer
        Dim sDataSetDefFile, sDataSetFile As String
        Dim sTopLevelObject, sTopLevelTable As String
        Dim sQuoteRef, sQuoteRefPassword As String
        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add Data Model ID Output Parameter
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Policy Link ID Output Param
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Data Model Code Input Param
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=(v_sGisDataModelCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsNothing(v_vInsuranceFileCnt) Then
                v_vInsuranceFileCnt = -1
            End If

            ' Optional Insurance File Cnt Param
            If v_vInsuranceFileCnt < 0 Then
                lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=(v_vInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If Informations.IsNothing(v_vRiskID) Then
                v_vRiskID = -1
            End If

            If v_vRiskID < 0 Then
                lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=v_vRiskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPolicyLinkSQL, sSQLName:=ACAddPolicyLinkName, bStoredProcedure:=ACAddPolicyLinkStored, lRecordsAffected:=lRecordsAffected)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the created Policy Link ID and the Data Model ID
            r_lPolicyLinkID = m_oDatabase.Parameters.Item("gis_policy_link_id").Value

            ' Generate the Quote Reference
            lReturn = CType(GenerateQuoteRef(v_lGISPolicyLinkID:=r_lPolicyLinkID, r_sQuoteRef:=sQuoteRef, v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Generate a random Password
            lReturn = CType(GeneratePassword(sQuoteRefPassword), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the Quote Ref and Password
            lReturn = CType(UpdateQuoteRef(v_lGISPolicyLinkID:=r_lPolicyLinkID, v_sQuoteRef:=sQuoteRef, v_sQuoteRefPassword:=sQuoteRefPassword, v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data Model Definition
            lReturn = CType(GetDataModelDef(v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Top Level Table Name
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create an Instance of the Top Level Object in the Data Set
            lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=sTopLevelObject, r_sOIKey:=r_sTopOIKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set PK Property for the Top Level Object to the Policy Link ID
            lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=sTopLevelObject & "_ID", v_sOIKey:=r_sTopOIKey, v_vPropertyValue:=CStr(r_lPolicyLinkID), v_bIsAssumedInfo:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Policy Link ID Property in the Top Level Object
            lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=GISSharedConstants.GISPolLinkIDName, v_sOIKey:=r_sTopOIKey, v_vPropertyValue:=CStr(r_lPolicyLinkID), v_bIsAssumedInfo:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Data Set in XML Format
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Quote Reference and Password
            r_sQuoteRef = sQuoteRef
            r_sQuoteRefPassword = sQuoteRefPassword

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewDataSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSet", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function SaveToDB(ByVal v_sGisDataModelCode As String, ByRef r_sXMLDataset As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load the Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Save the Dataset in the Database
            lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Return the Data Set
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveToDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_sGisDataModelCode As String, Optional ByRef r_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_vPolicyLinkID As Object = Nothing, Optional ByRef r_vQuoteRef As String = "", Optional ByRef r_vQuoteRefPassword As String = "", Optional ByRef r_dtGuaranteedQuoteDate As Date = #12/30/1899#, Optional ByRef r_vRiskID As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""
        Dim sTopLevelObject, sTopLevelTable, sSuppliedPassword, sSuppliedEncrypted, sRealEncrypted As String
        Dim bGetViaQuoteRef As Boolean
        Dim bNew As Boolean
        Dim lLoadFromDBMode As Integer
        Dim sDummyDataset As String = ""
        Dim lPos As Integer
        Dim sTopLevelQuoteObject, sTopLevelQuoteTable As String
        Dim lObjectCount, lQuoteCount As Integer
        Dim sRiskXML, sQuoteXML As String
        Dim r_sTopOIKey As String = ""
        Dim vTemp As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDataSet = New cGISDataSetControl.Application()

            ' Are we getting the Quote using the Quote Reference As the Key
            If Informations.IsNothing(r_vQuoteRef) Then
                ' No
                bGetViaQuoteRef = False
            ElseIf (r_vQuoteRef.Trim() = "") Then
                ' No
                bGetViaQuoteRef = False
            Else
                ' Yes
                bGetViaQuoteRef = True
            End If

            lReturn = CType(GetPolicyLink(r_sGISDataModelCode:=sDataModelCode, r_sQuoteRefPassword:=sRealEncrypted, r_vInsuranceFileCnt:=r_vInsuranceFileCnt, r_vPolicyLinkID:=r_vPolicyLinkID, r_vQuoteRef:=r_vQuoteRef, r_dtGuaranteedQuoteDate:=r_dtGuaranteedQuoteDate, r_vRiskID:=r_vRiskID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' If we are getting the Quote via the reference,
            ' then we need to check that the password matches.
            If bGetViaQuoteRef Then
                sSuppliedPassword = r_vQuoteRefPassword

                ' We Need to encrypt the Supplied Password
                lReturn = CType(bPMFunc.Encrypt(sSuppliedPassword, sSuppliedEncrypted), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Check that the Passwords Match
                If sSuppliedEncrypted.Trim() = sRealEncrypted.Trim() Then
                    ' Passwords Match
                Else
                    ' Passwords DO NOT MATCH
                    Return gPMConstants.PMEReturnCode.PMIncorrectPassword
                End If
            End If

            ' Get the Data Model Definition
            lReturn = CType(GetDataModelDef(v_sGisDataModelCode:=sDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Top Level Object Name
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How do we want to load from the database
            lLoadFromDBMode = GISSharedConstants.GetLoadSaveDBMode(v_sGisDataModelCode)

            ' Are we still useing the Old Slow Method
            If lLoadFromDBMode = GISSharedConstants.GISRegLoadSaveDBModeSlow Then
                ' Load the Data Set From the Database
                lReturn = CType(ObjectInstancesFromDB(v_sObjectName:=sTopLevelObject, v_sTopLevelTableName:=sTopLevelTable, v_lPolicyLinkID:=CInt(r_vPolicyLinkID), v_sTopLevelObjectName:=sTopLevelObject), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDataSet.GetAllOIKey(sTopLevelObject, vTemp)
                If Informations.IsArray(vTemp) Then

                    If vTemp.GetLowerBound(0) = 0 And vTemp.GetUpperBound(0) >= 0 Then
                        r_sTopOIKey = CStr(vTemp(0))
                    Else
                        r_sTopOIKey = ""

                        result = gPMConstants.PMEReturnCode.PMError

                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No OI Key Returned for " & sTopLevelObject, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        Return result
                    End If
                Else
                    lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=sTopLevelObject, r_sOIKey:=r_sTopOIKey)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set PK Property for the Top Level Object to the Policy Link ID

                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=sTopLevelObject & "_ID", v_sOIKey:=r_sTopOIKey, v_vPropertyValue:=CStr(r_vPolicyLinkID), v_bIsAssumedInfo:=False)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set Policy Link ID Property in the Top Level Object

                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=GISSharedConstants.GISPolLinkIDName, v_sOIKey:=r_sTopOIKey, v_vPropertyValue:=CStr(r_vPolicyLinkID), v_bIsAssumedInfo:=False)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' Return the Data Set in XML
                lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                lObjectCount = 0

                ' Use the Quick Stored Proc Method
                lReturn = CType(ObjectInstancesFromDBViaSP(v_lGISPolicyLinkID:=CInt(r_vPolicyLinkID), v_sTopLevelTableName:=sTopLevelTable, r_sXMLDataset:=sRiskXML, r_lObjectCount:=lObjectCount, r_lQuoteCount:=0, v_bQuoteObject:=False), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If we can Load the Quotes from the Database Also
                If lLoadFromDBMode = GISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                    lReturn = m_oDataSet.GetTopLevelQuoteObject(r_sObjectName:=sTopLevelQuoteObject, r_sTableName:=sTopLevelQuoteTable)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    lQuoteCount = 0

                    lReturn = CType(ObjectInstancesFromDBViaSP(v_lGISPolicyLinkID:=CInt(r_vPolicyLinkID), v_sTopLevelTableName:=sTopLevelQuoteTable, r_sXMLDataset:=sQuoteXML, r_lObjectCount:=lObjectCount, r_lQuoteCount:=lQuoteCount, v_bQuoteObject:=True), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                ' Return the Data Set in XML
                lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=sDummyDataset), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If lLoadFromDBMode = GISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                    lObjectCount += 1
                    lQuoteCount += 1 'PN18822
                    r_sXMLDataset = "<DATA_SET DataModelCode=" & Strings.ChrW(34).ToString() & v_sGisDataModelCode.Trim() & Strings.ChrW(34).ToString() & " NextOINumber=" & Strings.ChrW(34).ToString() & CStr(lObjectCount) & Strings.ChrW(34).ToString() & "><RISK_OBJECTS OI=""RISK_OBJECTS"">" & sRiskXML & "</RISK_OBJECTS>"
                    r_sXMLDataset = r_sXMLDataset & "<DELETED_OBJECTS OI=""DELETED_OBJECTS""/>"
                    r_sXMLDataset = r_sXMLDataset & "<QUOTES NextQuoteNumber=" & Strings.ChrW(34).ToString() & CStr(lQuoteCount) & Strings.ChrW(34).ToString() & " OI=""QUOTES"">"
                    r_sXMLDataset = r_sXMLDataset & sQuoteXML & "</QUOTES></DATA_SET>"
                Else
                    lObjectCount += 1
                    r_sXMLDataset = "<DATA_SET DataModelCode=" & Strings.ChrW(34).ToString() & v_sGisDataModelCode.Trim() & Strings.ChrW(34).ToString() & " NextOINumber=" & Strings.ChrW(34).ToString() & CStr(lObjectCount) & Strings.ChrW(34).ToString() & "><RISK_OBJECTS OI=""RISK_OBJECTS"">" & sRiskXML & "</RISK_OBJECTS>"
                    r_sXMLDataset = r_sXMLDataset & "<DELETED_OBJECTS OI=""DELETED_OBJECTS""/>"
                    r_sXMLDataset = r_sXMLDataset & "<QUOTES NextQuoteNumber=""1"" OI=""QUOTES""/></DATA_SET>"
                End If
                lPos = (sDummyDataset.IndexOf("<DATA_SET", StringComparison.CurrentCultureIgnoreCase) + 1)
                r_sXMLDataset = sDummyDataset.Substring(0, lPos - 1) & r_sXMLDataset

                r_sXMLDataset = r_sXMLDataset.Replace("&amp;", "&")
                r_sXMLDataset = r_sXMLDataset.Replace("&", "&amp;")

                ' Load the XML
                lReturn = CType(LoadFromXML(v_sDataModelCode:=sDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function NBStart(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lPartyCnt As Integer, ByRef r_lInsuranceFileCnt As Object, ByRef r_sQuoteRef As Object, ByRef r_sQuoteRefPassword As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oBom As Object
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.NBStartBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sQuoteRef:=r_sQuoteRef,
                                             r_sQuoteRefPassword:=r_sQuoteRefPassword, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            lReturn = CType(NewRiskDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.NBStartAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt),
                                            r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword,
                                            r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet

                lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSet:=r_sXMLDataset, r_sXMLDataSetDef:=r_sXMLDataSetDef)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBStartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBStart", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NBQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_sXMLDataset As String, Optional ByVal v_lGISSchemeID As Integer = -1, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_lRiskGroupID As Integer = -1, Optional ByVal v_bIsBackdatedMTA As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sGisDataModelCode As String = ""
        Dim sXMLDataSetDef As String = ""
        Dim sSourceOfBusiness As String = ""

        Dim bPMZipp As bPMZipper.Business

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lXMLStrLength > 0 Then
                bPMZipp = New bPMZipper.Business()
                lReturn = bPMZipp.DecompressString(r_sXMLDataset, m_lXMLStrLength)
            End If

            'Load the Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = CType(NBQuoteStateful(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=v_dtEffectiveDate, v_lGISSchemeID:=v_lGISSchemeID, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_lRiskGroupID:=v_lRiskGroupID, v_bIsBackdatedMTA:=v_bIsBackdatedMTA), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn < gPMConstants.PMEReturnCode.PMNBQuoteReferred Then
                Return lReturn
            End If

            ' Return the Data Set
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If m_lXMLStrLength > 0 Then
                m_lXMLStrLength = r_sXMLDataset.Length
                lReturn = bPMZipp.CompressString(r_sXMLDataset)
            End If

            bPMZipp = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NBTransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_sXMLDataset As String) As Integer
        Return NBTransact(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID, r_sXMLDataset:=r_sXMLDataset, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function NBTransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_sXMLDataset As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISPolicyLinkID As Integer
        Dim vSchemeArray As Object
        Dim sQEMName As String = ""
        Dim oQEM As Object
        Dim sXMLDataSetDef, sSourceOfBusiness As String
        Dim oOptions As Object
        Dim sSystem As String = ""
        Dim bNew As Boolean
        Dim oBom As Object
        Dim vUnderwriting As Object
        Dim bUnderwritingBranchEnabled, bIsUnderwritingBranch As Boolean
        Dim lPolicyLinkID As Integer
        Dim sSchemeBusinessTypeCode As String = ""
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sSourceOfBusiness = ""

            If Not Informations.IsNothing(r_vAdditionalDataArray) Then
                If Informations.IsArray(r_vAdditionalDataArray) Then
                    For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                        Select Case r_vAdditionalDataArray(0, i)
                            Case CNSourceOfBusiness
                                sSourceOfBusiness = CStr(r_vAdditionalDataArray(1, i))
                            Case CNEdiSolution
                                bUnderwritingBranchEnabled = CBool(r_vAdditionalDataArray(1, i))
                            Case CNPolicyLinkId
                                lPolicyLinkID = CInt(Val(CStr(r_vAdditionalDataArray(1, i))))
                            Case "SchemeBusinessTypeCode"
                                sSchemeBusinessTypeCode = CStr(r_vAdditionalDataArray(1, i))
                        End Select
                    Next i
                End If
            End If

            If sSchemeBusinessTypeCode = "" Then
                sSchemeBusinessTypeCode = v_sGisBusinessTypeCode
            End If

            If sSourceOfBusiness = CNAgentsOnline Then
                lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=CNAgentsOnline, v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                ' Create Back Office Mapper if Required
                lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateBOM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact", vErrNo:=0, vErrDesc:="Failed to create the BOM for datamodel " & (If(sSourceOfBusiness = CNAgentsOnline, CNAgentsOnline, v_sGisDataModelCode)))
                Return result
            End If

            If sSourceOfBusiness = CNAgentsOnline Then
                If Not (oBom Is Nothing) Then
                    lReturn = oBom.NBTransactAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_vSchemeArray:=vSchemeArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oBOM.NBTransactAfter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact", vErrNo:=0, vErrDesc:="The call to bGISBOMAOL.NBTransactAfter Failed")

                        lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

                        Return result
                    End If
                    m_oDataSet = oDataSet
                End If
                Return result
            End If

            If bUnderwritingBranchEnabled Then
                'This is an broking solution called by the sts
                m_lReturn = CType(TransactBrokingSts(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID, v_lPolicyLinkID:=lPolicyLinkID, r_oBOM:=oBom, r_vAdditionalDataArray:=r_vAdditionalDataArray), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactBrokingSts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                    Return result
                End If
                lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                'We don't need to do anything else so exit here
                Return result
            End If

            If (r_sXMLDataset.Trim().Length <> 0) And Not (m_oDataSet Is Nothing) Then
                ' Get the Data Set Definition
                lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

            End If

            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()

            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the details of the Scheme that we are Transacting For
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=sSchemeBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.GetSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")
                Return result
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.NBTransactBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_vSchemeArray:=vSchemeArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                    Return result
                End If

                m_oDataSet = oDataSet
                ' Need to get the XML Back as it will have been changed by the BOM
                ' and we will need to pass the new XML to the QEM
                lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            If v_sGisDataModelCode.Trim() = "OIM" Then
                lReturn = CType(TempOIMTransact(v_lGISSchemeID:=v_lGISSchemeID, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TempOIMTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")

                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

                    Return result
                End If
            Else
                ' Call the QEM to Transact
                lReturn = CType(CallQEMToNBTransact(v_lGISSchemeID:=v_lGISSchemeID, v_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_vSchemeArray:=vSchemeArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMToNBTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")

                    ' CJB 021002 Return the Data Set to ensure any updated properties are not lost
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                    Return result
                End If
            End If

            ' Are we Processing OIM
            If v_sGisDataModelCode.Trim() <> "OIM" Then
                ' Update the Trans Status to say we are about to Save to the DB
                lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISNBTransTypeSaveInDB, r_oDatabase:=m_oDatabase, v_lGISSchemeID:=v_lGISSchemeID), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLinkTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")

                    ' CJB 021002 Return the Data Set to ensure any updated properties are not lost
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

                    Return result
                End If

                ' Save the Details
                lReturn = SaveInDB()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveInDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")
                    Return result

                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                End If
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.NBTransactAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_vSchemeArray:=vSchemeArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBOM.NBTransactAfter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")

                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            If Not (oBom Is Nothing) Then
                oBom.Dispose()
                oBom = Nothing
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM OK.", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")

            ' Update the Trans Type to say that the NB Transact is COMPLETE
            lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISNBTransTypeComplete, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISNBTransTypeComplete) Failed", ACApp, ACClass, "NBTransact")
                result = gPMConstants.PMEReturnCode.PMFalse

                lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

                Return result
            End If

            ' Very Last thing to do is Return the XML
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact")
                Return result
            End If

            Return result
        Catch excep As System.Exception
            If Not (oBom Is Nothing) Then
                oBom.Dispose()
                oBom = Nothing
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBTransactFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBTransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            ' CJB 021002 Return the Data Set to ensure any updated properties are not lost
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

            Return result
        End Try
    End Function

    Public Function RenewalTransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_sXMLDataset As String) As Integer
        Return RenewalTransact(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID, r_sXMLDataset:=r_sXMLDataset, v_lInsuranceFileCnt:=0, r_vAdditionalDataArray:=Nothing)
    End Function
    Public Function RenewalTransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_sXMLDataset As String, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return RenewalTransact(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID, r_sXMLDataset:=r_sXMLDataset, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vAdditionalDataArray:=Nothing)
    End Function
    Public Function RenewalTransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_sXMLDataset As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Return RenewalTransact(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID, r_sXMLDataset:=r_sXMLDataset, v_lInsuranceFileCnt:=0, r_vAdditionalDataArray:=r_vAdditionalDataArray)
    End Function
    Public Function RenewalTransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_sXMLDataset As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISPolicyLinkID As Integer
        Dim vSchemeArray As Object
        Dim sQEMName As String = ""
        Dim oQEM As Object
        Dim sXMLDataSetDef As String = ""
        Dim bNew As Boolean
        Dim oBom As Object
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data Set Definition
            lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=v_sGisDataModelCode, r_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)

            ' Create Back Office Mapper if Required
            lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Create instance of bGISScheme
            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()

            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the details of the Scheme that we are Transacting For
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.GetSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                Return result
            End If

            ' Update the Trans Status to say we are about to Save to the DB
            lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISNBTransTypeSaveInDB, r_oDatabase:=m_oDatabase, v_lGISSchemeID:=v_lGISSchemeID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLinkTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")

                Return result
            End If

            ' Save the Details
            lReturn = SaveInDB()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveInDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                Return result
            End If

            ' Call BOM's RenewalTransact method
            If Not (oBom Is Nothing) Then

                lReturn = oBom.RenewalTransactAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode),
                                                    r_oDataSet:=oDataSet, v_vSchemeArray:=vSchemeArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBOM.RenewalTransactAfter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            If Not (oBom Is Nothing) Then

                oBom.Dispose()
                oBom = Nothing
            End If

            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM OK.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")

            ' Update the Trans Type to say that the Renewal Transact is COMPLETE
            lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISNBTransTypeComplete, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISNBTransTypeComplete) Failed", ACApp, ACClass, "RenewalTransact")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Very Last thing to do is Return the XML
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                Return result
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalTransactFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            If Not (oBom Is Nothing) Then
                oBom.Dispose()
                oBom = Nothing
            End If

            Return result
        End Try
    End Function

    Public Function Refer(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataset As String, ByVal v_sInsurerCode As String) As Integer
        Return Refer(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataset:=r_sXMLDataset, v_sInsurerCode:=v_sInsurerCode, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function Refer(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataset As String, ByVal v_sInsurerCode As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim oBom As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a BackOfficeMapper object if required
            lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            ' Call the BOM Refer method (if required)
            If Not (oBom Is Nothing) Then
                lReturn = oBom.Refer(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_sInsurerCode:=gPMFunctions.ToSafeString(v_sInsurerCode), r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
            End If

            ' Get the Data Set
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReferFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Refer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function
    Public Function MTAStart(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_iType As Integer, ByVal v_dtCoverStartDate As Date, ByVal v_dtExpiryDate As Date, ByVal v_lPolicyVersion As Integer, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String) As Integer
        Return MTAStart(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_iType:=v_iType, v_dtCoverStartDate:=v_dtCoverStartDate, v_dtExpiryDate:=v_dtExpiryDate, v_lPolicyVersion:=v_lPolicyVersion, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lOldGISPolicyLinkID:=0, v_lOldInsuranceFileCnt:=0, r_lNewGISPolicyLinkID:=0, r_lNewInsuranceFileCnt:=0, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function MTAStart(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_iType As Integer, ByVal v_dtCoverStartDate As Date, ByVal v_dtExpiryDate As Date, ByVal v_lPolicyVersion As Integer, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lOldGISPolicyLinkID As Integer, ByVal v_lOldInsuranceFileCnt As Integer, ByRef r_lNewGISPolicyLinkID As Integer, ByRef r_lNewInsuranceFileCnt As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oBom As Object
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.MTAStartBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_iType:=gPMFunctions.ToSafeInteger(v_iType), v_lOldInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lOldInsuranceFileCnt), v_dtCoverStartDate:=gPMFunctions.ToSafeDate(v_dtCoverStartDate), v_dtExpiryDate:=gPMFunctions.ToSafeDate(v_dtExpiryDate), v_lPolicyVersion:=gPMFunctions.ToSafeInteger(v_lPolicyVersion), r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            lReturn = CType(CopyDataSet(v_sDataModelCode:=v_sGisDataModelCode, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vOldGISPolicyLinkId:=v_lOldGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=r_lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Now we've copied the dataset we need to ensure the gis_scheme_id on the gis_policy_link
            ' record for the newly copied policy version is set to the gis_scheme_id on the old risk
            lReturn = CType(UpdatePolicyLinkSchemeIDViaInsFileCnt(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLinkSchemeIDViaInsFileCnt Failed, lReturn:" & lReturn & " v_lOldInsuranceFileCnt:" & CStr(v_lOldInsuranceFileCnt) & " v_lNewInsuranceFileCnt:" & CStr(r_lNewInsuranceFileCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="MTAStart", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Load the New Dataset
            lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.MTAStartAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_lNewInsuranceFileCnt:=r_lNewInsuranceFileCnt, r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
            End If

            lReturn = SaveInDB()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAStartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAStart", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function MTAQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_sXMLOldRisk As String, ByRef r_sXMLNewRisk As String) As Integer
        Return MTAQuote(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=v_dtEffectiveDate, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=r_sXMLNewRisk, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function MTAQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_sXMLOldRisk As String, ByRef r_sXMLNewRisk As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAllSchemesArray, vQEMSchemeArray(,) As Object
        Dim sCurrentQEM As String = ""
        Dim sNextQEM As New StringBuilder
        Dim lQEMSchemeRow As Integer
        Dim iCounter As Integer
        Dim sGisDataModelCode As String = ""
        Dim lGISPolicyLinkID As Integer
        Dim sXMLDataSetDef As String = ""
        Dim vArray As Object
        Dim lGISSchemeID, lOldGISSchemeID As Integer
        Dim oBom As Object
        Dim bNew As Boolean
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBOM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create instance of bGISScheme
            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()

            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Load the OLD Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=v_sXMLOldRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXML Old Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' now get OLD gis policy link so we can get the gis scheme id it originally transacted against - CL120500
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            ' Load the NEW Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXML New Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' RFC02091999 - Clear any existing Quote Output before quoting
            lReturn = m_oDataSet.ClearAllQuoteOutput()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearAllQuoteOutput Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.MTAQuoteBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBOM.MTAQuoteBefore Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            ' Get the Data Set Definition
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Get the Old GIS SchemeID from the Old Risk Policy Link
            lReturn = CType(GetSchemeIDFromLink(v_lGISPolicyLinkID:=lGISPolicyLinkID, r_lGisSchemeId:=lOldGISSchemeID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeIDFromLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Get the Current Effective GIS SchemeID
            lReturn = CType(GetCurrentSchemeID(v_lOldGISSchemeID:=lOldGISSchemeID, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_lNewGISSchemeID:=lGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentSchemeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Get the Schemes that we need to quote for
            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vAllSchemesArray, v_lGISSchemeId:=lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(vAllSchemesArray) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemes returned nothing", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Get the Data Model Code
            sGisDataModelCode = m_oDataSet.GISDataModelCode

            ' Get the First Quote  Engine Mapper From the List

            sCurrentQEM = CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim()

            sCurrentQEM = sCurrentQEM & "." & CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim()

            ' For each Scheme
            For lRow As Integer = vAllSchemesArray.GetLowerBound(1) To vAllSchemesArray.GetUpperBound(1)
                ' Get the Quote Engine Mapper
                sNextQEM = New StringBuilder(CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, lRow)).Trim())
                sNextQEM.Append("." & CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, lRow)).Trim())

                ' Is the QEM the same as the last one
                If sCurrentQEM <> sNextQEM.ToString() Then

                    lReturn = CType(CallQEMToMTAQuote(v_sQEMName:=sCurrentQEM, v_vQEMSchemeArray:=vQEMSchemeArray, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=r_sXMLNewRisk, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate)), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMToMTAQuote failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                    vQEMSchemeArray = Nothing ' reset
                    sCurrentQEM = sNextQEM.ToString()
                End If

                If Informations.IsArray(vQEMSchemeArray) Then

                    ReDim Preserve vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, vQEMSchemeArray.GetUpperBound(1) + 1)
                Else
                    ReDim vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, 0)
                End If

                lQEMSchemeRow = vQEMSchemeArray.GetUpperBound(1)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchObjectName, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchClassName, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchSchemeNo, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchSchemeNo, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchSchemeVer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchSchemeVer, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchFilename, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchFilename, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchQMInsurerRef, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchQMInsurerRef, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchType, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchType, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchVariant, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchVariant, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchCommPerc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchCommPerc, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchID, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchID, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchDesc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchDesc, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAbi81Insurer, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchAgencyCode, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAgencyCode, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchEdiMailBox, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchEdiMailBox, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchInsurerDesc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchInsurerDesc, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchDictVer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchDictVer, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchTypeFlags, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchTypeFlags, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchInsurerCode, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchInsurerCode, lRow)
            Next lRow

            If Informations.IsArray(vQEMSchemeArray) Then
                lReturn = CType(CallQEMToMTAQuote(v_sQEMName:=sCurrentQEM, v_vQEMSchemeArray:=vQEMSchemeArray, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=r_sXMLNewRisk, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate)), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMToMTAQuote(2) failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.MTAQuoteAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBOM.MTAQuoteAfter failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            If GISSharedConstants.SaveOnMTAQuote(v_sGisDataModelCode, v_sGisBusinessTypeCode) Then
                ' Save in the Database
                lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveInDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If

            ' Return the Data Set
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXML failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTAQuoteFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTAQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function MTATransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_sXMLOldRisk As String, ByRef r_sXMLNewRisk As String) As Integer
        Return MTATransact(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=r_sXMLNewRisk, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function MTATransact(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_sXMLOldRisk As String, ByRef r_sXMLNewRisk As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAllSchemesArray, vQEMSchemeArray As Object
        Dim sCurrentQEM As String = ""
        Dim sNextQEM As New StringBuilder
        Dim lQEMSchemeRow As Integer
        Dim iCounter As Integer
        Dim sGisDataModelCode As String = ""
        Dim lGISPolicyLinkID As Integer
        Dim sXMLDataSetDef As String = ""
        Dim vArray As Object
        Dim lGISSchemeID, lOldGISSchemeID As Integer
        Dim lPolicyLinkID As Integer
        Dim bUnderwritingBranchEnabled As Boolean
        Dim sSourceOfBusiness As String = ""
        Dim bNew As Boolean

        Dim oBom As Object
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGIS.MTATransact starting...", vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact")

            If Not Informations.IsNothing(r_vAdditionalDataArray) Then
                If Informations.IsArray(r_vAdditionalDataArray) Then
                    For i As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                        Select Case r_vAdditionalDataArray(0, i)
                            Case CNSourceOfBusiness

                                sSourceOfBusiness = CStr(r_vAdditionalDataArray(1, i))
                            Case CNEdiSolution

                                bUnderwritingBranchEnabled = CBool(r_vAdditionalDataArray(1, i))
                            Case CNPolicyLinkId

                                lPolicyLinkID = CInt(Val(CStr(r_vAdditionalDataArray(1, i))))
                            Case CNSchemeId

                                lGISSchemeID = CInt(Val(CStr(r_vAdditionalDataArray(1, i))))
                        End Select
                    Next i
                End If
            End If

            If sSourceOfBusiness = CNAgentsOnline Then
                sGisDataModelCode = CNAgentsOnline
            Else
                sGisDataModelCode = v_sGisDataModelCode
            End If

            lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If bUnderwritingBranchEnabled Then
                'This is an broking solution called by the sts
                m_lReturn = CType(TransactBrokingSts(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=lGISSchemeID, v_lPolicyLinkID:=lPolicyLinkID, r_oBOM:=oBom, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_sTransactType:=bGISTemp.GISMTATransTypeComplete), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactBrokingSts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact")
                    Return result
                End If
                'We don't need to do anything else so exit here
                Return result
            End If

            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()
            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the OLD Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=v_sXMLOldRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' now get OLD gis policy link so we can get the gis scheme id it originally transacted against - CL120500
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()
            GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bGIS.MTATransact: old risk lGISPolicyLinkID=" & lGISPolicyLinkID, vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact")

            ' Load the NEW Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' RFC02091999 - Clear any existing Quote Output before quoting
            lReturn = m_oDataSet.ClearAllQuoteOutput()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Data Set Definition
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' RFC040800 - Get the Old GIS SchemeID from the Old Risk Policy Link
            lReturn = CType(GetSchemeIDFromLink(v_lGISPolicyLinkID:=lGISPolicyLinkID, r_lGisSchemeId:=lOldGISSchemeID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' RFC040800 - Get the Current Effective GIS SchemeID
            lReturn = CType(GetCurrentSchemeID(v_lOldGISSchemeID:=lOldGISSchemeID, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_lNewGISSchemeID:=lGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Schemes that we need to quote for
            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vAllSchemesArray, v_lGISSchemeId:=lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not Informations.IsArray(vAllSchemesArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.MTATransactBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet,
                                                 v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), v_vSchemeArray:=vAllSchemesArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            ' Get the Data Model Code
            sGisDataModelCode = m_oDataSet.GISDataModelCode

            ' Get the First Quote  Engine Mapper From the List
            sCurrentQEM = CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim().ToUpper()
            sCurrentQEM = sCurrentQEM & "." & CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim().ToUpper()

            ' For each Scheme
            For lRow As Integer = vAllSchemesArray.GetLowerBound(1) To vAllSchemesArray.GetUpperBound(1)
                ' Get the Quote Engine Mapper
                sNextQEM = New StringBuilder(CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, lRow)).Trim().ToUpper())
                sNextQEM.Append("." & CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, lRow)).Trim().ToUpper())

                ' Is the QEM the same as the last one
                If sCurrentQEM <> sNextQEM.ToString() Then
                    lReturn = CType(CallQEMToMTATransact(v_sQEMName:=sCurrentQEM, v_vQEMSchemeArray:=vQEMSchemeArray, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=r_sXMLNewRisk, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate)), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = lReturn
                        lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
                        Return result
                    End If

                    vQEMSchemeArray = Nothing ' reset
                    sCurrentQEM = sNextQEM.ToString()
                End If

                If Informations.IsArray(vQEMSchemeArray) Then
                    ReDim Preserve vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, vQEMSchemeArray.GetUpperBound(1) + 1)
                Else
                    ReDim vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, 0)
                End If

                lQEMSchemeRow = vQEMSchemeArray.GetUpperBound(1)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchObjectName, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchClassName, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchSchemeNo, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchSchemeNo, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchSchemeVer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchSchemeVer, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchFilename, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchFilename, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchQMInsurerRef, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchQMInsurerRef, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchType, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchType, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchVariant, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchVariant, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchCommPerc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchCommPerc, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchID, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchID, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchDesc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchDesc, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAbi81Insurer, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchAgencyCode, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAgencyCode, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchEdiMailBox, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchEdiMailBox, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchInsurerDesc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchInsurerDesc, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchDictVer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchDictVer, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchTypeFlags, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchTypeFlags, lRow)
                vQEMSchemeArray(GISSharedConstants.GISQEMSchInsurerCode, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchInsurerCode, lRow)
            Next lRow

            If Informations.IsArray(vQEMSchemeArray) Then
                lReturn = CType(CallQEMToMTATransact(v_sQEMName:=sCurrentQEM, v_vQEMSchemeArray:=vQEMSchemeArray, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLOldRisk:=v_sXMLOldRisk, r_sXMLNewRisk:=r_sXMLNewRisk, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate)), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
                    Return result
                End If
            End If

            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()
            lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=lGISPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeSaveInDB, r_oDatabase:=m_oDatabase, v_lGISSchemeID:=lGISSchemeID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLinkTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact")
                lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
                Return result
            End If

            lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
                Return result
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.MTATransactAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), v_vSchemeArray:=vQEMSchemeArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            If Not (oBom Is Nothing) Then
                oBom.Dispose()
                oBom = Nothing
            End If

            ' RFC040800 - Update the Trans Status to say we are COMPLETE
            lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=gPMFunctions.ToSafeInteger(lGISPolicyLinkID), v_dTransactDate:=DateTime.Now, v_sTransactType:=bGISTemp.GISMTATransTypeComplete, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLinkTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact")

                ' CJB 021002 Return the Data Set to ensure any updated properties are not lost
                lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)

                Return result
            End If

            ' Return the Data Set
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTATransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTATransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
            Return result
        End Try
    End Function

    Public Function CopyDataSet(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkID As Integer, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, Optional ByVal v_vOldGISPolicyLinkId As Integer = -1, Optional ByVal v_vOldInsuranceFileCnt As Integer = -1, Optional ByVal v_vOldXMLDataSet As String = "", Optional ByVal v_vNewInsuranceFileCnt As Integer = -1, Optional ByVal v_vOldRiskID As Integer = -1, Optional ByVal v_vNewRiskID As Integer = -1, Optional ByVal v_vCopyQuotes As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSetDef, sXMLDataSet, sTopOIKey As String
        Dim vOIKeyArray As Object
        Dim sTopLevelObject, sTopLevelTable As String
        Dim lPos As Integer
        Dim sComp As String = ""
        Dim lSaveToDBMode As Integer
        Dim bIgnoreFirstInst As Boolean
        Dim lStartPosition, lEndPosition As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If (v_vOldXMLDataSet.Trim() = "") And (v_vOldGISPolicyLinkId < 1) And (v_vOldInsuranceFileCnt < 1) Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Either an XML DataSet, PolicyLinkID, InsuranceFileCnt MUST be supplied.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDataSet")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lSaveToDBMode = GISSharedConstants.GetLoadSaveDBMode(v_sDataModelCode)
            If lSaveToDBMode <> GISSharedConstants.GISRegLoadSaveDBModeSlow And (v_vOldXMLDataSet.Trim() = "") Then
                Return CopyDataSetViaSP(v_sDataModelCode:=v_sDataModelCode, v_lSaveToDBMode:=lSaveToDBMode, v_bCopyQuotes:=v_vCopyQuotes, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID, v_vNewRiskID:=v_vNewRiskID)
            End If

            lReturn = CType(CopyDataSetStateful(v_sDataModelCode:=v_sDataModelCode, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vOldXMLDataSet:=v_vOldXMLDataSet, v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID, v_vNewRiskID:=v_vNewRiskID, v_vCopyQuotes:=v_vCopyQuotes), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Data Set in XML Format
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bIgnoreFirstInst = False
            lPos = (r_sXMLDataset.IndexOf(" US=""0""") + 1)
            Do While lPos <> 0
                If Not bIgnoreFirstInst Then
                    bIgnoreFirstInst = True
                    lPos += 1
                Else
                    r_sXMLDataset = r_sXMLDataset.Remove(lPos - 1, 7).Insert(lPos - 1, " US=""1""")
                End If
                lPos = Informations.inStr(lPos, r_sXMLDataset, " US=""0""")
            Loop

            If v_sDataModelCode = "XEM" Then
                lPos = (r_sXMLDataset.IndexOf(" US=""2""") + 1)

                Do While lPos <> 0
                    r_sXMLDataset = r_sXMLDataset.Remove(lPos - 1, 7).Insert(lPos - 1, " US=""1""")
                    lPos = Informations.inStr(lPos, r_sXMLDataset, " US=""2""")
                Loop
            End If

            ' Find start of deleted objects
            lStartPosition = (r_sXMLDataset.IndexOf("<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """>") + 1)

            ' If we found deleted objects
            If lStartPosition > 0 Then
                lEndPosition = (r_sXMLDataset.IndexOf("</DELETED_OBJECTS>") + 1)
                ' If we found delimiter
                If lEndPosition > 0 Then
                    ' Copy first bit of message + empty deleted objects + end bit of original message
                    r_sXMLDataset = Mid(r_sXMLDataset, 1, lStartPosition - 1) &
                                    "<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """/>" &
                                    Mid(r_sXMLDataset, lEndPosition + ("</DELETED_OBJECTS>").Length, r_sXMLDataset.Length)
                End If
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDataSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDataSet", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function PrintForm(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef v_sXMLDataSet As String, ByVal v_lFormNumber As Integer, ByVal v_lGISSchemeID As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn, lGISPolicyLinkID As Integer
        Dim vSchemeArray As Object
        Dim sQEMName As String = ""
        Dim oQEM As Object
        Dim sXMLDataSetDef As String = ""
        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of bGISScheme - CL150200
            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()

            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the XML
            lReturn = LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=v_sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sQEMName = CStr(vSchemeArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim().ToUpper()

            sQEMName = sQEMName & "." & CStr(vSchemeArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim().ToUpper()

            lReturn = CreateQEM(v_sQEMName:=sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Data Set Definition
            lReturn = ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = oQEM.PrintForm(v_sXMLDataSetDef:=gPMFunctions.ToSafeString(sXMLDataSetDef), v_sXMLDataSet:=gPMFunctions.ToSafeString(v_sXMLDataSet), v_vSchemeArray:=vSchemeArray, v_lFormNumber:=gPMFunctions.ToSafeInteger(v_lFormNumber))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            oQEM.Dispose()
            oQEM = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintFormFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintForm", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Function NBPostQuoteProcess(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lProcessType As Integer, ByRef r_sXMLDataset As Object, ByVal v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn, lGISPolicyLinkID As Integer
        Dim vSchemeArray As Object
        Dim sQEMName As String = ""
        Dim oQEM As Object
        Dim sXMLDataSetDef As Object = ""

        Dim bNew As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()

            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the XML
            lReturn = LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sQEMName = CStr(vSchemeArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim().ToUpper()

            sQEMName = sQEMName & "." & CStr(vSchemeArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim().ToUpper()

            lReturn = CreateQEM(v_sQEMName:=sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Data Set Definition
            lReturn = ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = oQEM.NBPostQuoteProcess(v_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vSchemeArray:=vSchemeArray, v_lProcessType:=gPMFunctions.ToSafeInteger(v_lProcessType))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            oQEM.Dispose()
            oQEM = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBPostQuoteProcessFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBPostQuoteProcess", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Function LoadFromXML(ByVal v_sDataModelCode As String, ByVal v_sXMLDataSet As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDataSet = New cGISDataSetControl.Application()

            ' Load the Data Set with an Empty Data Set for this Model
            lReturn = CType(GetDataModelDef(v_sDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update with the Data Set
            Return m_oDataSet.UpdateDataSetFromXML(v_sXMLDataSet:=v_sXMLDataSet)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadFromXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function ReturnAsXML(Optional ByRef r_sXMLDataset As String = "", Optional ByRef r_sXMLDataSetDef As String = "") As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Return m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataset)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReturnAsXMLFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReturnAsXML", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function GetObjectAndPropertyDefs(ByVal v_sGisDataModelCode As String, ByRef r_vObjectArray(,) As Object, ByRef r_vPropertyArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetObjectsSQL, sSQLName:=ACGetObjectsName, bStoredProcedure:=ACGetObjectsStored, vResultArray:=r_vObjectArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPropertiesSQL, sSQLName:=ACGetPropertiesName, bStoredProcedure:=ACGetPropertiesStored, vResultArray:=r_vPropertyArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function

    Private Function ObjectInstancesToDB(ByVal v_sObjectName As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray(), vPropertyArray(,) As Object

        Dim vOIKeyArray As Object
        Dim sOIKey, sObjectName As String

        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_sInsertSQL:=m_sAddSQL, r_sUpdateSQL:=m_sUpdateSQL, r_sDeleteSQL:=m_sDeleteSQL)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=vOIKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If there Are NO Instances of this Object Type then EXIT
        If Not Informations.IsArray(vOIKeyArray) Then
            Return result
        End If

        For lRow As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)
            sOIKey = CStr(vOIKeyArray(lRow))

            lReturn = CType(ObjectInstanceToDB(v_sObjectName:=v_sObjectName, v_sOIKey:=sOIKey, v_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        ' If there Are NO Child Objects for this Object Type then EXIT
        If Not Informations.IsArray(vChildObjectArray) Then
            Return result
        End If

        ' For Each Child Object
        For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)

            sObjectName = CStr(vChildObjectArray(lRow))
            lReturn = CType(ObjectInstancesToDB(v_sObjectName:=sObjectName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        Return result
    End Function

    Private Function DeleteObjectsInDB() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPropertyArray As Object
        Dim lFrom, lTo As Integer
        Dim vDelObjArray(,) As Object
        Dim sObjectName, sOIKey As String


        result = gPMConstants.PMEReturnCode.PMTrue
        ' Get any Deleted Objects
        lReturn = m_oDataSet.GetDelObjectsArray(vDelObjArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If there aren't any deleted objects then exit
        If Not Informations.IsArray(vDelObjArray) Then
            Return result
        End If

        lFrom = vDelObjArray.GetLowerBound(1)
        lTo = vDelObjArray.GetUpperBound(1)

        For lRow As Integer = lFrom To lTo
            sObjectName = CStr(vDelObjArray(0, lRow))
            sOIKey = CStr(vDelObjArray(1, lRow))
            lReturn = CType(DeleteObjectInstancesInDB(v_sObjectName:=sObjectName, v_sOIKey:=sOIKey), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow
        Return result
    End Function

    Private Function DeleteObjectInstancesInDB(ByVal v_sObjectName As String, ByVal v_sOIKey As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vPropertyArray As Object
        Dim vChildObjectArray As Object
        Dim lFrom, lTo As Integer
        Dim sObjectName As String = ""
        Dim vOIKeyArray As Object
        Dim sOIKey As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_vPropertyArray:=vPropertyArray, r_vChildObjectArray:=vChildObjectArray, r_sInsertSQL:=m_sAddSQL, r_sUpdateSQL:=m_sAddUpdateSQL, r_sDeleteSQL:=m_sDeleteSQL)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If there Are no Child Objects for this Object Type then delete
        If Not Informations.IsArray(vChildObjectArray) Then
            lReturn = CType(ObjectInstanceToDB(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey, v_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            Return result
        Else
            ' For Each Child Object
            For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)
                sObjectName = CStr(vChildObjectArray(lRow))

                ' Get the Keys for All Instances of this Type
                lReturn = m_oDataSet.GetDeletedOIKey(v_sObjectName:=sObjectName, r_vOIKeyArray:=vOIKeyArray, v_bDeletedObjects:=True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                If Informations.IsArray(vOIKeyArray) Then
                    sOIKey = CStr(vOIKeyArray(vOIKeyArray.GetLowerBound(0)))

                    ' Update status of an Instance of the Specified Object to deleted
                    lReturn = m_oDataSet.UpdateObjectInstanceStatus(v_sOIKey:=sOIKey)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                    lReturn = CType(DeleteObjectInstancesInDB(v_sObjectName:=sObjectName, v_sOIKey:=sOIKey), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If
                End If
            Next lRow

            ' Get the Object Definition Details for the Parent
            lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_vPropertyArray:=vPropertyArray, r_sInsertSQL:=m_sAddSQL, r_sUpdateSQL:=m_sAddUpdateSQL, r_sDeleteSQL:=m_sDeleteSQL)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Now update the instance of Parent in DB
            lReturn = CType(ObjectInstanceToDB(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey, v_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If


        End If

        Return result
    End Function
    ''' <summary>
    ''' ObjectInstanceToDB
    ''' </summary>
    ''' <param name="v_sObjectName"></param>
    ''' <param name="v_sOIKey"></param>
    ''' <param name="v_vPropertyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ObjectInstanceToDB(ByVal v_sObjectName As String, ByVal v_sOIKey As String, ByVal v_vPropertyArray(,) As Object) As Integer
        Dim nResult As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecordsAffected As Integer
        Dim lUpdateStatus As gPMConstants.PMEComponentAction
        Dim sColumnName, sPropertyName As String
        Dim vPropertyValue As Object
        Dim bIsAssumedInfo As Boolean
        Dim sSQL, sStringField As String
        Dim lDataType As Integer
        Dim iPMDataType As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Get the Update Status for this Object Instance
        lReturn = m_oDataSet.GetObjectInstDetails(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey, r_lUpdateStatus:=lUpdateStatus)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' What is the Update Status for this Object Instance
        Select Case lUpdateStatus
                ' Add
            Case gPMConstants.PMEComponentAction.PMAdd
                sSQL = m_sAddSQL
            Case gPMConstants.PMEComponentAction.PMEdit
                sSQL = m_sUpdateSQL
            Case gPMConstants.PMEComponentAction.PMDelete
                sSQL = m_sDeleteSQL
            Case Else
                Return nResult
        End Select

        For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)
            sPropertyName = CStr(v_vPropertyArray(0, lRow))

            lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RFC 290200 - Store the SQL in the Data Set Def
            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName, r_lDataType:=lDataType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Convert the GIS Data Type to its PM One
            lReturn = CType(GISSharedConstants.GISToPMDataType(v_iGISDataType:=lDataType, r_iPMDataType:=iPMDataType), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If (lDataType = GISSharedConstants.GISDataTypeDate) Then
                If vPropertyValue = "0" Or vPropertyValue = "" Then
                    vPropertyValue = System.DBNull.Value
                Else
                    ''Remove T and Z to ignore any timezone conversions on time
                    vPropertyValue = String.Format(gPMFunctions.ToSafeDate(vPropertyValue.ToString().Replace("T", " ").Replace("Z", "")).ToString, "yyyy/MM/dd HH:mm:ss")
                End If
            End If

            If (Convert.IsDBNull(vPropertyValue) OrElse Informations.IsNothing(vPropertyValue)) OrElse (vPropertyValue = "") Then
                lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                sStringField = vPropertyValue

                If (iPMDataType = 0 AndAlso lDataType = 5) Then
                    sStringField = Mid(sStringField, 1, 255)
                End If
                lReturn = CType(StripQuote(r_sStringField:=sStringField), gPMConstants.PMEReturnCode)
                vPropertyValue = sStringField
                lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=vPropertyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        Next lRow

        ' Add/Update the Instance
        lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="ObjectInstanceToDB", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected, bKeepQuery:=True)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If lUpdateStatus = PMEComponentAction.PMDelete And Not v_sObjectName.ToString().ToUpper().StartsWith(m_oDataSet.GISDataModelCode.ToString().ToUpper() + "_") Then
            Dim sXMLDef, sXMLDet As String
            Dim xDocDataSet As New System.Xml.XmlDocument
            Dim xnCurNode As XmlNode
            Dim sTableName, sParenTableName, sChildTableName, sSQLChildDelete, sSelOIKey As String
            Dim iColChildNode, iColCurNode, iColGrandChildNode As Integer
            sXMLDef = ""
            sXMLDet = ""
            lReturn = m_oDataSet.ReturnAsXML(sXMLDef, sXMLDet)
            sParenTableName = "DELETE FROM " + m_oDataSet.GISDataModelCode.ToString() + "_" + v_sObjectName
            xDocDataSet.LoadXml(sXMLDet)
            Dim xnlNodeList As XmlNodeList
            xnlNodeList = xDocDataSet.GetElementsByTagName(v_sObjectName)
            For iColCurNode = 0 To xnlNodeList.Count - 1
                If (xnlNodeList(iColCurNode).Attributes(m_oDataSet.GISDataModelCode.ToString() + "_" + v_sObjectName + "_ID") IsNot Nothing) Then
                    sSelOIKey = "OI" + xnlNodeList(iColCurNode).Attributes(m_oDataSet.GISDataModelCode.ToString() + "_" + v_sObjectName + "_ID").Value.ToString()
                    If sSelOIKey = v_sOIKey Then
                        For Each xnCurNode In xnlNodeList
                            Dim oNodeList1 As XmlNodeList
                            oNodeList1 = xnCurNode.ChildNodes()
                            For iColChildNode = 0 To oNodeList1.Count - 1
                                For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)
                                    sPropertyName = CStr(v_vPropertyArray(0, lRow))
                                    lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If
                                    ' RFC 290200 - Store the SQL in the Data Set Def
                                    lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName, r_lDataType:=lDataType)

                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    ' Convert the GIS Data Type to its PM One
                                    lReturn = CType(GISSharedConstants.GISToPMDataType(v_iGISDataType:=lDataType, r_iPMDataType:=iPMDataType), gPMConstants.PMEReturnCode)

                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return gPMConstants.PMEReturnCode.PMFalse
                                    End If

                                    If (lDataType = GISSharedConstants.GISDataTypeDate) Then
                                        If vPropertyValue = "0" Or vPropertyValue = "" Then
                                            vPropertyValue = System.DBNull.Value
                                        End If
                                    End If

                                    If (Convert.IsDBNull(vPropertyValue) OrElse Informations.IsNothing(vPropertyValue)) OrElse (vPropertyValue = "") Then
                                        lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    Else
                                        sStringField = vPropertyValue
                                        If (iPMDataType = 0 AndAlso lDataType = 5) Then
                                            sStringField = Mid(sStringField, 1, 255)
                                        End If
                                        lReturn = CType(StripQuote(r_sStringField:=sStringField), gPMConstants.PMEReturnCode)
                                        vPropertyValue = sStringField
                                        lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=vPropertyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                    End If
                                Next lRow
                                'Delete Grand Grand Child if any
                                If oNodeList1.Item(iColChildNode).ChildNodes IsNot Nothing Then
                                    Dim oNodeList2 As XmlNodeList
                                    oNodeList2 = oNodeList1.Item(iColChildNode).ChildNodes()
                                    For iColGrandChildNode = 0 To oNodeList2.Count - 1
                                        sTableName = oNodeList2.Item(iColGrandChildNode).Name
                                        sChildTableName = "DELETE FROM " + m_oDataSet.GISDataModelCode.ToString() + "_" + sTableName
                                        sSQLChildDelete = sSQL.ToString().ToUpper().Replace(sParenTableName.ToUpper(), sChildTableName.ToUpper())
                                        lReturn = m_oDatabase.SQLAction(sSQL:=sSQLChildDelete, sSQLName:="ObjectInstanceToDB", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected, bKeepQuery:=True)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                        For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)
                                            sPropertyName = CStr(v_vPropertyArray(0, lRow))
                                            lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If
                                            ' RFC 290200 - Store the SQL in the Data Set Def
                                            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName, r_lDataType:=lDataType)

                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If

                                            ' Convert the GIS Data Type to its PM One
                                            lReturn = CType(GISSharedConstants.GISToPMDataType(v_iGISDataType:=lDataType, r_iPMDataType:=iPMDataType), gPMConstants.PMEReturnCode)

                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If

                                            If (lDataType = GISSharedConstants.GISDataTypeDate) Then
                                                If vPropertyValue = "0" Or vPropertyValue = "" Then
                                                    vPropertyValue = System.DBNull.Value
                                                End If
                                            End If

                                            If (Convert.IsDBNull(vPropertyValue) OrElse Informations.IsNothing(vPropertyValue)) OrElse (vPropertyValue = "") Then
                                                lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If
                                            Else
                                                sStringField = vPropertyValue
                                                If (iPMDataType = 0 AndAlso lDataType = 5) Then
                                                    sStringField = Mid(sStringField, 1, 255)
                                                End If
                                                lReturn = CType(StripQuote(r_sStringField:=sStringField), gPMConstants.PMEReturnCode)
                                                vPropertyValue = sStringField
                                                lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=vPropertyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                    Return gPMConstants.PMEReturnCode.PMFalse
                                                End If
                                            End If
                                        Next lRow
                                    Next
                                    For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)
                                        sPropertyName = CStr(v_vPropertyArray(0, lRow))
                                        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vPropertyValue, r_bIsAssumedInfo:=bIsAssumedInfo)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If
                                        ' RFC 290200 - Store the SQL in the Data Set Def
                                        lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_sColumnName:=sColumnName, r_lDataType:=lDataType)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        ' Convert the GIS Data Type to its PM One
                                        lReturn = CType(GISSharedConstants.GISToPMDataType(v_iGISDataType:=lDataType, r_iPMDataType:=iPMDataType), gPMConstants.PMEReturnCode)
                                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                            Return gPMConstants.PMEReturnCode.PMFalse
                                        End If

                                        If (lDataType = GISSharedConstants.GISDataTypeDate) Then
                                            If vPropertyValue = "0" Or vPropertyValue = "" Then
                                                vPropertyValue = System.DBNull.Value
                                            End If
                                        End If

                                        If (Convert.IsDBNull(vPropertyValue) OrElse Informations.IsNothing(vPropertyValue)) OrElse (vPropertyValue = "") Then
                                            lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If
                                        Else
                                            sStringField = vPropertyValue
                                            lReturn = CType(StripQuote(r_sStringField:=sStringField), gPMConstants.PMEReturnCode)
                                            vPropertyValue = sStringField
                                            lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=vPropertyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                Return gPMConstants.PMEReturnCode.PMFalse
                                            End If
                                        End If
                                    Next lRow
                                End If
                                sTableName = oNodeList1.Item(iColChildNode).Name
                                sChildTableName = "DELETE FROM " + m_oDataSet.GISDataModelCode.ToString() + "_" + sTableName
                                sSQLChildDelete = sSQL.ToString().ToUpper().Replace(sParenTableName.ToUpper(), sChildTableName.ToUpper())
                                lReturn = m_oDatabase.SQLAction(sSQL:=sSQLChildDelete, sSQLName:="ObjectInstanceToDB", bStoredProcedure:=False, lRecordsAffected:=lRecordsAffected, bKeepQuery:=True)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFalse
                                End If
                            Next
                        Next
                    End If
                End If
            Next
        End If
        ' Reset the Update Status for this Object Instance
        lReturn = m_oDataSet.ResetUpdateStatus(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function

    Private Function ResetPolicyLinkID(ByVal v_sObjectName As String, ByVal v_lNewPolicyLinkID As Integer, ByVal v_sTopLevelObject As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lIsQuoteObject As Integer
        Dim vChildObjectArray As Object
        Dim vOIKeyArray As Object
        Dim sOIKey, sObjectName As String


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Keys for All Instances of this Type
        lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=vOIKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If there Are NO Instances of this Object Type then EXIT
        If Not Informations.IsArray(vOIKeyArray) Then
            Return result
        End If

        ' If we are doing the Top Level Object
        If v_sObjectName = v_sTopLevelObject Then
            sOIKey = CStr(vOIKeyArray(vOIKeyArray.GetLowerBound(0)))

            ' Set Policy Link ID Property in the Top Level Object
            ' RFC100500 - Use the Loaded From DB Flag as we do not want to affect the Update Status
            lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=v_sTopLevelObject, v_sPropertyName:=GISSharedConstants.GISPolLinkIDName, v_sOIKey:=sOIKey, v_vPropertyValue:=CStr(v_lNewPolicyLinkID), v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        End If

        ' Get the Object Definition Details
        lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_vChildObjectArray:=vChildObjectArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' For Each Object Instance
        For lRow As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)
            sOIKey = CStr(vOIKeyArray(lRow))

            ' Reset the Top Level Primary Key value for this Object
            ' RFC100500 - Use the Loaded From DB Flag as we do not want to affect the Update Status
            lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sTopLevelObject & "_ID", v_sOIKey:=sOIKey, v_vPropertyValue:=CStr(v_lNewPolicyLinkID), v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        ' If there Are NO Child Objects for this Object Type then EXIT
        If Not Informations.IsArray(vChildObjectArray) Then
            Return result
        End If

        ' For Each Child Object

        For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)
            sObjectName = CStr(vChildObjectArray(lRow))
            lReturn = CType(ResetPolicyLinkID(v_sObjectName:=sObjectName, v_lNewPolicyLinkID:=v_lNewPolicyLinkID, v_sTopLevelObject:=v_sTopLevelObject), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        Return result
    End Function

    Private Function ObjectInstancesFromDB(ByVal v_sObjectName As String, ByVal v_sTopLevelTableName As String, ByVal v_lPolicyLinkID As Integer, ByVal v_sTopLevelObjectName As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray, vPropertyArray(,) As Object
        Dim sPropertyName As String = ""
        Dim sColumnName As String = ""
        Dim vPropertyValue As Object
        Dim sObjectName As String = ""
        Dim sParentTableName As String = ""
        Dim vOIArray(,) As Object
        Dim sOIKey As String = ""
        Dim lParentKeyCol, lParentKeyValue, lPKKeyCol, lPKKeyValue As Integer
        Dim sParentOIKey As String = ""
        Dim sSelectSQL As String = ""
        Dim lDataType As Integer
        Dim iPMDataType As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Paramaters Collection
        m_oDatabase.Parameters.Clear()

        ' Get the Object Definition Details
        lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_sSelectSQL:=sSelectSQL)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If there is a Parent Object
        If sParentObjectName.Trim() <> "" Then
            ' Get its Table Name
            lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=sParentObjectName, r_sTableName:=sParentTableName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        End If

        lPKKeyCol = -1
        lParentKeyCol = -1

        For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)
            sPropertyName = CStr(vPropertyArray(0, lRow))

            lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType, r_sColumnName:=sColumnName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If sColumnName.ToUpper() = (v_sTopLevelTableName.Trim() & ACIDCol).ToUpper() Then
                ' Convert the GIS Data Type to its PM One
                lReturn = CType(GISSharedConstants.GISToPMDataType(v_iGISDataType:=lDataType, r_iPMDataType:=iPMDataType), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Add a Parameter for this Column
                lReturn = m_oDatabase.Parameters.Add(sName:=sColumnName, vValue:=CStr(v_lPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iPMDataType)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            ' Find the PK Key Col
            If ((sTableName.Trim() & ACIDCol).ToUpper()) = sColumnName.Trim().ToUpper() Then
                lPKKeyCol = lRow
            End If

            ' If there is a Parent Table
            If sParentTableName.Trim() <> "" Then
                ' Find the Parent PK Key Col
                If ((sParentTableName.Trim() & ACIDCol).ToUpper()) = sColumnName.Trim().ToUpper() Then
                    lParentKeyCol = lRow
                End If
            End If
        Next lRow

        ' Get Object Instances From DB
        lReturn = m_oDatabase.SQLSelect(sSQL:=sSelectSQL, sSQLName:="ObjectInstancesFromDB", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vOIArray, bKeepNulls:=True)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' If there are No Object Instances then EXIT.
        ' Note: This will mean that the will NOT check for Child Objects
        '       as there cannot be any.
        If Not Informations.IsArray(vOIArray) Then
            Return result
        End If

        ' Have we found the PK Column
        If lPKKeyCol < 0 Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Find the PK Column for Object : " & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ObjectInstancesFromDB")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If there is a Parent Table
        If sParentTableName.Trim() <> "" Then
            ' Have we found the Parent PK Column
            If lParentKeyCol < 0 Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Find the PK Column for Object : " & v_sObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="ObjectInstancesFromDB")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' For Each Object Instance
        For lRow As Integer = vOIArray.GetLowerBound(1) To vOIArray.GetUpperBound(1)
            If sParentTableName.Trim() = "" Then
                sParentOIKey = ""
            Else
                ' Get the Parent Key Value
                lParentKeyValue = CInt(vOIArray(lParentKeyCol, lRow))

                ' we need to add the PolicyBinder OI Offset.
                ' This is to stop the OI value clashing with other objects
                If sParentObjectName.ToUpper() = v_sTopLevelObjectName.ToUpper() Then
                    ' Build the Parent Object Instance Key
                    sParentOIKey = m_oDataSet.BuildOIKey(v_sObjectName:=sParentObjectName, v_lOINumber:=lParentKeyValue + GISSharedConstants.GISPolicyBinderOIOffset)
                Else
                    ' Build the Parent Object Instance Key
                    sParentOIKey = m_oDataSet.BuildOIKey(v_sObjectName:=sParentObjectName, v_lOINumber:=lParentKeyValue)
                End If
            End If

            ' Get the PK Value
            lPKKeyValue = CInt(vOIArray(lPKKeyCol, lRow))

            ' Create the Object Instance
            lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=v_sObjectName, v_sParentOIKey:=sParentOIKey, r_sOIKey:=sOIKey, v_bLoadedFromDB:=True, v_lOINumber:=lPKKeyValue)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' More to do here, log an error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Each Property Value which is NOT NULL
            For lCol As Integer = vOIArray.GetLowerBound(0) To vOIArray.GetUpperBound(0)
                ' Get the Property Value
                vPropertyValue = vOIArray(lCol, lRow)

                sPropertyName = CStr(vPropertyArray(0, lCol))

                lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_lDataType:=lDataType)
                ' If there is a NOT NULL value
                If Not (Convert.IsDBNull(vPropertyValue) Or Informations.IsNothing(vPropertyValue)) Then
                    ' Set the Value
                    ' More to do here. We will need to set assumed info correctly
                    lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=sOIKey, v_vPropertyValue:=CStr(vPropertyValue), v_bIsAssumedInfo:=False, v_bLoadedFromDB:=True)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If
                End If
            Next lCol
        Next lRow

        ' If there Are NO Child Objects for this Object Type then EXIT
        If Not Informations.IsArray(vChildObjectArray) Then
            Return result
        End If

        ' For Each Child Object
        For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)
            sObjectName = CStr(vChildObjectArray(lRow))
            lReturn = CType(ObjectInstancesFromDB(v_sObjectName:=sObjectName, v_sTopLevelTableName:=v_sTopLevelTableName, v_lPolicyLinkID:=v_lPolicyLinkID, v_sTopLevelObjectName:=v_sTopLevelObjectName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        Return result
    End Function

    Private Function ObjectInstancesFromDBViaSP(ByVal v_lGISPolicyLinkID As Integer, ByVal v_sTopLevelTableName As String, ByRef r_sXMLDataset As String, ByRef r_lObjectCount As Integer, ByRef r_lQuoteCount As Integer, ByVal v_bQuoteObject As Boolean) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSPName As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        If v_bQuoteObject Then
            sSPName = "spg_" & v_sTopLevelTableName.Trim() & "_sel"
        Else
            sSPName = "spg_" & v_sTopLevelTableName.Trim() & "_sel"
        End If

        With m_oDatabase

            m_oDatabase.Parameters.Clear()

            lReturn = .Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = .Parameters.Add(sName:="object_count", vValue:=CStr(r_lObjectCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If v_bQuoteObject Then
                lReturn = .Parameters.Add(sName:="quote_count", vValue:=CStr(r_lQuoteCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            lReturn = .SQLSelectTextField(sSQL:=sSPName, sSQLName:="LoadFromDBViaSP", bStoredProcedure:=True, sTextData:=r_sXMLDataset)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            r_lObjectCount = .Parameters.Item("object_count").Value

            If v_bQuoteObject Then
                r_lQuoteCount = .Parameters.Item("quote_count").Value
            End If

        End With

        Return result
    End Function

    Private Function GetPolicyLink(ByRef r_sGISDataModelCode As String, ByRef r_sQuoteRefPassword As String) As Integer
        Return GetPolicyLink(r_sGISDataModelCode:=r_sGISDataModelCode, r_sQuoteRefPassword:=r_sQuoteRefPassword, r_vInsuranceFileCnt:=Nothing, r_vPolicyLinkID:=Nothing, r_vQuoteRef:=Nothing, r_dtGuaranteedQuoteDate:=#12/30/1899#, r_vRiskID:=Nothing, r_lClaimId:=-1, r_lPartyID:=-1, r_lCaseID:=-1)
    End Function
    Private Function GetPolicyLink(ByRef r_sGISDataModelCode As String, ByRef r_sQuoteRefPassword As String, ByRef r_vInsuranceFileCnt As Object, ByRef r_vPolicyLinkID As Object, ByRef r_vQuoteRef As Object, ByRef r_dtGuaranteedQuoteDate As Date, ByRef r_vRiskID As Object) As Integer
        Return GetPolicyLink(r_sGISDataModelCode:=r_sGISDataModelCode, r_sQuoteRefPassword:=r_sQuoteRefPassword, r_vInsuranceFileCnt:=r_vInsuranceFileCnt, r_vPolicyLinkID:=r_vPolicyLinkID, r_vQuoteRef:=r_vQuoteRef, r_dtGuaranteedQuoteDate:=r_dtGuaranteedQuoteDate, r_vRiskID:=r_vRiskID, r_lClaimId:=-1, r_lPartyID:=-1, r_lCaseID:=-1)
    End Function
    Private Function GetPolicyLink(ByRef r_sGISDataModelCode As String, ByRef r_sQuoteRefPassword As String, ByRef r_vInsuranceFileCnt As Object, ByRef r_vPolicyLinkID As Object, ByRef r_vQuoteRef As Object, ByRef r_dtGuaranteedQuoteDate As Date, ByRef r_vRiskID As Object, ByRef r_lClaimId As Integer, ByRef r_lPartyID As Integer, ByRef r_lCaseID As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vClaimId As Integer
        Dim vCaseId As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        If Informations.IsNothing(r_vInsuranceFileCnt) Then
            r_vInsuranceFileCnt = -1
        End If

        If Informations.IsNothing(r_vPolicyLinkID) Then
            r_vPolicyLinkID = -1
        End If

        If Informations.IsNothing(r_vQuoteRef) Then
            r_vQuoteRef = CInt("")
        End If

        If Informations.IsNothing(r_vRiskID) Then
            r_vRiskID = -1
        End If

        ' If NO Key Supplied Log an Error
        If (r_vInsuranceFileCnt < 0) And (r_vPolicyLinkID < 0) And (CStr(r_vQuoteRef).Trim() = "") And (r_lClaimId < 0) And (r_lPartyID < 0 And r_lCaseID < 0) Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="InsuranceFileCnt, PolicyLinkID OR QuoteRef MUST be supplied!", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDB")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Clear the Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Policy Link Param
        lReturn = CType(AddDatabaseParameter("gis_policy_link_id", r_vPolicyLinkID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add Insurance File Cnt Param
        lReturn = CType(AddDatabaseParameter("insurance_file_cnt", r_vInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Optional RiskID Param
        lReturn = CType(AddDatabaseParameter("risk_id", r_vRiskID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add Quote Ref Param
        lReturn = CType(AddDatabaseParameter("quote_ref", r_vQuoteRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' check that the claim id being passed is valid
        ' if not replace with null so the sproc ignores it.
        If r_lClaimId < 0 Then
            vClaimId = Nothing
        Else
            vClaimId = r_lClaimId
        End If

        lReturn = CType(AddDatabaseParameter("claim_id", vClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = CType(AddDatabaseParameter("party_cnt", r_lPartyID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If r_lCaseID < 0 Then

            vCaseId = Nothing
        Else
            vCaseId = r_lCaseID
        End If

        lReturn = CType(AddDatabaseParameter("case_id", vCaseId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyLinkSQL, sSQLName:=ACGetPolicyLinkName, bStoredProcedure:=ACGetPolicyLinkStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If there are No Records return NOT Found
        If m_oDatabase.Records.Count() < 1 Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' If there is no Policy Link ID then the record was not found.
        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("gis_policy_link_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("gis_policy_link_id")) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        Else
            r_vPolicyLinkID = m_oDatabase.Records.Item(0).Fields()("gis_policy_link_id")
        End If

        ' If the Policy Link ID is less than one, then the record was not found
        If r_vPolicyLinkID < 1 Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        r_sGISDataModelCode = m_oDatabase.Records.Item(0).Fields()("gis_data_model_code").Trim()

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("insurance_file_cnt")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("insurance_file_cnt")) Then
            r_vInsuranceFileCnt = -1
        Else
            r_vInsuranceFileCnt = m_oDatabase.Records.Item(0).Fields()("insurance_file_cnt")
        End If

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("risk_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("risk_id")) Then
            r_vRiskID = -1
        Else
            r_vRiskID = m_oDatabase.Records.Item(0).Fields()("risk_id")
        End If

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("quote_ref")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("quote_ref")) Then
            r_vQuoteRef = ""
        Else
            r_vQuoteRef = m_oDatabase.Records.Item(0).Fields()("quote_ref").Trim()
        End If

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("quote_ref_password")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("quote_ref_password")) Then
            r_sQuoteRefPassword = ""
        Else
            r_sQuoteRefPassword = m_oDatabase.Records.Item(0).Fields()("quote_ref_password").Trim()
        End If

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("guaranteed_quote_date")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("guaranteed_quote_date")) Then
            r_dtGuaranteedQuoteDate = DateTime.Now
        Else
            r_dtGuaranteedQuoteDate = m_oDatabase.Records.Item(0).Fields()("guaranteed_quote_date")
        End If

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("claim_id")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("claim_id")) Then
            r_lClaimId = -1
        Else
            r_lClaimId = m_oDatabase.Records.Item(0).Fields()("claim_id")
        End If

        If Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("party_cnt")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("party_cnt")) Then
            r_lPartyID = -1
        Else
            r_lPartyID = m_oDatabase.Records.Item(0).Fields()("party_cnt")
        End If

        Return result
    End Function

    Public Function GetQemDmCode(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sQemCode As String, ByRef r_sDmCode As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Policy Link Param
            lReturn = CType(AddDatabaseParameter("insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "select gq.code 'Qem_Code', gdm.code 'Datamodel_code'" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "from gis_qem gq" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "join gis_qem_usage gqu on gqu.gis_qem_id = gq.gis_qem_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "join gis_policy_link gpl on gpl.gis_scheme_id=gqu.gis_scheme_id and gpl.gis_data_model_id = gqu.gis_datA_model_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "join gis_data_model gdm on gdm.gis_data_model_id = gqu.gis_data_model_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "where gpl.insurance_file_cnt={insurance_file_cnt}" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there are No Records return NOT Found
            If m_oDatabase.Records.Count() < 1 Then
                'Return True as No Record Found
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("Qem_Code")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("Qem_Code"))) Then
                r_sQemCode = m_oDatabase.Records.Item(0).Fields()("Qem_Code")
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("Datamodel_code")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("Datamodel_code"))) Then
                r_sDmCode = m_oDatabase.Records.Item(0).Fields()("Datamodel_code")
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQemDmCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQemDmCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function CheckIfNexusRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bNexus As Boolean) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Policy Link Param
            lReturn = CType(AddDatabaseParameter("insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIfNexusRiskSQL, sSQLName:=ACCheckIfNexusRiskName, bStoredProcedure:=ACCheckIfNexusRiskStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there are No Records return NOT Found
            If m_oDatabase.Records.Count() < 1 Then
                'Return True as No Record Found
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("is_nexus")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("is_nexus"))) Then
                r_bNexus = m_oDatabase.Records.Item(0).Fields()("is_nexus")
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckIfNexusRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckIfNexusRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function GetQemCodeForScheme(ByVal v_lSchemeID As Integer, ByRef r_sQemCode As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Policy Link Param
            lReturn = CType(AddDatabaseParameter("gis_scheme_id", v_lSchemeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSQL = "select gq.code 'Qem_Code'"
            sSQL = sSQL & "from gis_qem gq" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "join gis_qem_usage gqu on gqu.gis_qem_id = gq.gis_qem_id" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "where gqu.gis_scheme_id={gis_scheme_id}" & Strings.ChrW(13) & Strings.ChrW(10)

            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there are No Records return NOT Found
            If m_oDatabase.Records.Count() < 1 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            If Not (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("Qem_Code")) Or Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("Qem_Code"))) Then
                r_sQemCode = m_oDatabase.Records.Item(0).Fields()("Qem_Code")
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetQemCodeForScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetQemCodeForScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function CallQEMToQuote(ByVal v_sQEMName As String, ByVal v_vQEMDREAdditionalArray As Object, ByVal v_lQuoteType As Integer, ByVal v_sXMLDataSetDef As String, ByVal v_sXMLDataSet As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_bIsBackdatedMTA As Boolean = False, Optional ByVal v_bAfterPRETriggerRules As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet As String = ""
        Dim oDataSet As cGISDataSetControl.Application

        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = CType(CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        oDataSet = m_oDataSet
        If v_sQEMName = "bGISQEMDRE.DRE" Then
            lReturn = oQEM.NBQuote(v_vQEMDREAdditionalArray:=v_vQEMDREAdditionalArray, v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), r_oDataSet:=DirectCast(oDataSet, cGISDataSetControl.Application), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vAdditionalDataArray:=r_vAdditionalDataArray)
        Else
            lReturn = oQEM.NBQuote(v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), r_oDataSet:=DirectCast(oDataSet, cGISDataSetControl.Application), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=gPMFunctions.ToSafeBoolean(v_bIsBackdatedMTA), v_bAfterPRETriggerRules:=gPMFunctions.ToSafeBoolean(v_bAfterPRETriggerRules))
        End If
        oDataSet = Nothing

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        oQEM = Nothing
        Return result

    End Function

    Private Function CallQEMToNBTransact(ByVal v_lGISSchemeID As Integer, ByVal v_sXMLDataSetDef As Object, ByRef r_sXMLDataset As Object, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_vSchemeArray As Object) As Integer
        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As Integer
        Dim sQEMName As String = ""
        Dim oDataSet As cGISDataSetControl.Application


        result = gPMConstants.PMEReturnCode.PMTrue

        GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CallQEMToNBTransact START", ACApp, ACClass, "CallQEMToNBTransact")

        sQEMName = CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim().ToUpper()

        sQEMName = sQEMName & "." & CStr(v_vSchemeArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim().ToUpper()

        lReturn = CreateQEM(v_sQEMName:=sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateQEM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMToNBTransact")
            Return result
        End If

        GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CallQEMToNBTransact Created QEM OK", ACApp, ACClass, "CallQEMToNBTransact")

        If GISSharedConstants.GetQEMMethodsVersionNum(v_sGisDataModelCode) = 1 Then
            lReturn = oQEM.NBTransact(v_sXMLDataSetDef:=v_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vSchemeArray:=v_vSchemeArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oQEM.NBTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMToNBTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CallQEMToNBTransact QEM.NBTransact OK", ACApp, ACClass, "CallQEMToNBTransact")

            lReturn = m_oDataSet.UpdateDataSetFromXML(v_sXMLDataSet:=r_sXMLDataset)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.UpdateDataSetFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMToNBTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CallQEMToNBTransact Updated XML OK", ACApp, ACClass, "CallQEMToNBTransact")
        Else
            oDataSet = m_oDataSet

            lReturn = oQEM.NBTransact(v_vSchemeArray:=v_vSchemeArray, r_oDataSet:=DirectCast(oDataSet, cGISDataSetControl.Application))

            ' Release Local Ref
            oDataSet = Nothing

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oQEM.NBTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMToNBTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CallQEMToNBTransact QEM.NBTransact OK", ACApp, ACClass, "CallQEMToNBTransact")
        End If

        oQEM.Dispose()
        oQEM = Nothing

        GISSharedConstants.DebugLogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CallQEMToNBTransact END", ACApp, ACClass, "CallQEMToNBTransact")

        Return result
    End Function

    Private Function CallQEMToMTAQuote(ByVal v_sQEMName As String, ByVal v_vQEMSchemeArray As Object, ByVal v_lQuoteType As Integer, ByVal v_sXMLDataSetDef As Object, ByVal v_sXMLOldRisk As String, ByRef r_sXMLNewRisk As Object, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date) As Integer
        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = CType(CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        sXMLDataSet = v_sXMLOldRisk

        lReturn = oQEM.MTAQuote(v_vSchemeArray:=v_vQEMSchemeArray, v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), v_sXMLDataSetDef:=v_sXMLDataSetDef, r_sXMLNewRisk:=r_sXMLNewRisk, v_sXMLOldRisk:=gPMFunctions.ToSafeString(v_sXMLOldRisk), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLNewRisk), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        sXMLDataSet = ""
        oQEM = Nothing

        Return result
    End Function

    Private Function CallQEMToMTATransact(ByVal v_sQEMName As String, ByVal v_vQEMSchemeArray As Object, ByVal v_lQuoteType As Integer, ByVal v_sXMLDataSetDef As Object, ByVal v_sXMLOldRisk As String, ByRef r_sXMLNewRisk As Object, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_dtEffectiveDate As Date) As Integer
        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = CType(CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        sXMLDataSet = v_sXMLOldRisk

        lReturn = oQEM.MTATransact(v_vSchemeArray:=v_vQEMSchemeArray, v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), v_sXMLDataSetDef:=v_sXMLDataSetDef, r_sXMLNewRisk:=r_sXMLNewRisk, v_sXMLOldRisk:=gPMFunctions.ToSafeString(v_sXMLOldRisk), v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=v_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLNewRisk)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        sXMLDataSet = ""
        oQEM = Nothing

        Return result
    End Function

    Private Function CallQEMCreateEDI(ByVal v_sQEMName As String, ByVal v_lGISSchemeID As Integer, ByVal v_sXMLDataSetDef As Object, ByRef r_sXMLDataset As Object, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet As String = ""
        Dim vSchemeArray As Object
        Dim lGISPolicyLinkID As Integer


        result = gPMConstants.PMEReturnCode.PMTrue
        GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMCreateEDI START", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMCreateEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        lReturn = CType(CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateQEM Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMCreateEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        sXMLDataSet = r_sXMLDataset

        lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

        lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lGISSchemeID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.GetSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMCreateEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        lReturn = oQEM.NBTransact(v_sXMLDataSetDef:=v_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vSchemeArray:=vSchemeArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oQEM.NBTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMCreateEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        lReturn = m_oDataSet.UpdateDataSetFromXML(v_sXMLDataSet:=r_sXMLDataset)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.UpdateDataSetFromXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMCreateEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        oQEM = Nothing

        GISSharedConstants.DebugLogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMCreateEDI END " & result, vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMCreateEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result
    End Function

    Friend Function CreateQEM(ByVal v_sQEMName As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oQEM As Object) As Integer
        Return CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=r_oQEM, vDatabase:=Nothing)
    End Function

    Friend Function CreateQEM(ByVal v_sQEMName As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oQEM As Object, ByRef vDatabase As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            r_oQEM = gPMFunctions.CreateLateBoundObject(v_sQEMName)
        Catch
        End Try
        If r_oQEM Is Nothing Then
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Create QEM Object :- " & v_sQEMName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateQEM")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Dim oDatabase As Object = m_oDatabase
        lReturn = r_oQEM.Initialise(sUsername:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=gPMFunctions.ToSafeString(ACApp), vDatabase:=oDatabase)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If
        m_oDatabase = oDatabase

        Try
            m_lReturn = r_oQEM.SetProcessModes(vTask:=gPMFunctions.ToSafeInteger(m_iTask), vNavigate:=gPMFunctions.ToSafeInteger(m_lNavigate), vProcessMode:=gPMFunctions.ToSafeInteger(m_lProcessMode),
                                               vTransactionType:=gPMFunctions.ToSafeString(m_sTransactionType), vEffectiveDate:=gPMFunctions.ToSafeDate(m_dtEffectiveDate))

            If Informations.Err().Number = 438 Then
                Dim v = Informations.Err().Clear()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = r_oQEM.InitialiseEngine(gPMFunctions.ToSafeString(v_sGisDataModelCode), gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result

        Catch exc As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateQEMFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateQEM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End Try
    End Function

    Public Function GetDataModelDef(ByVal v_sGisDataModelCode As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sDataSetDefFile, sDataSetFile As String
        Dim vObjectArray, vPropertyArray As Object
        Dim bSuppressXSDProduction As Boolean
        Dim sTimestamp As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDataSetDefFile, r_sDataSetFile:=sDataSetFile), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a New Data Set
            m_oDataSet = New cGISDataSetControl.Application()

            ' Try to load from the Empty XML files
            lReturn = m_oDataSet.LoadFromXMLFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile)

            ' If we could NOT Load from stored Empty Files
            ' then we will need to build it up ourselves.
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Get the Objects and Properties for this Data Model
                lReturn = CType(GetObjectAndPropertyDefs(v_sGisDataModelCode:=v_sGisDataModelCode, r_vObjectArray:=vObjectArray, r_vPropertyArray:=vPropertyArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' determine whether or not to suppress XSD production
                lReturn = CType(GetXSDSuppressionFlag(v_sGisDataModelCode, bSuppressXSDProduction), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Initialise the Data Set with the Object/Properties

                lReturn = m_oDataSet.InitialiseDataSet(v_sGisDataModelCode:=v_sGisDataModelCode,
                                                       v_vObjectArray:=vObjectArray, v_vPropertyArray:=vPropertyArray,
                                                       v_bSupressXSDProduction:=bSuppressXSDProduction,
                                                       v_sDataModelType:=m_sDataModelType)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Save to the Data Set Folder for Next Time
                lReturn = m_oDataSet.SaveXMLToFile(v_sDataSetDefFile:=sDataSetDefFile, v_sDataSetFile:=sDataSetFile)
                ' If we failed to save
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log an Error Message but do not fail
                    ' as we should be able to carry on
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Save XML Data Set Definition to : " & sDataSetDefFile, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModelDef")
                End If

            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModelDefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModelDef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function GenerateQuoteRef(ByVal v_lGISPolicyLinkID As Integer, ByRef r_sQuoteRef As String, ByVal v_sGisDataModelCode As String) As Integer
        Dim result As Integer = 0
        Dim oAutoNum As bPMAutoNumber.Business
        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            oAutoNum = New bPMAutoNumber.Business
            lReturn = oAutoNum.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oAutoNum = Nothing
                Return lReturn
            End If

            result = oAutoNum.EncodeLongV2(v_lGISPolicyLinkID, r_sQuoteRef)

            oAutoNum.Dispose()
            oAutoNum = Nothing

            If (v_sGisDataModelCode.Trim().ToUpper() = "MAR") Or (v_sGisDataModelCode.Trim().ToUpper() = "SIF") Then
                r_sQuoteRef = "LAW" & r_sQuoteRef.Substring(r_sQuoteRef.Length - (r_sQuoteRef.Length - 3))
            End If

            Return result
        Catch excep As System.Exception
            oAutoNum.Dispose()
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateQuoteRefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateQuoteRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function GeneratePassword(ByRef r_sPassword As String) As Integer
        Dim result As Integer = 0
        Dim MyValue As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Initialize random-number generator.

        Dim rnd As New Random
        ' Generate 8 Chars
        For lSub As Integer = 1 To 8
            ' Generate random value between 65 and 91.
            ' i.e. ASCII A to Z - but exclude any vowels
            Do
                MyValue = rnd.Next(65, 91)
            Loop While MyValue = 65 Or MyValue = 69 Or MyValue = 73 Or MyValue = 79 Or MyValue = 85

            ' Convert to a Char and add to Password
            r_sPassword = r_sPassword & Strings.ChrW(MyValue).ToString()
        Next lSub

        Return result
    End Function

    Public Function UpdateQuoteRef(ByVal v_lGISPolicyLinkID As Integer, ByVal v_sQuoteRef As String, ByVal v_sQuoteRefPassword As String, ByVal v_sGisDataModelCode As String) As Integer
        Dim result As Integer = 0
        Dim sEncryptedPassword As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecordsAffected As Integer

        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Encrypt the Password
            lReturn = CType(bPMFunc.Encrypt(v_sQuoteRefPassword, sEncryptedPassword), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Parameters
            m_oDatabase.Parameters.Clear()

            ' Policy Link ID Input Param
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Quote Ref Input Param
            lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref", vValue:=CStr(v_sQuoteRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Quote Ref Password Input Param
            lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref_password", vValue:=CStr(sEncryptedPassword), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateQteRefSQL, sSQLName:=ACUpdateQteRefName, bStoredProcedure:=ACUpdateQteRefStored, lRecordsAffected:=lRecordsAffected)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateQuoteRefFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateQuoteRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function UpdatePartyCnt(ByVal v_lGISPolicyLinkID As Integer, ByVal v_lPartyCnt As Object, ByVal v_lInsuranceFileCnt As Object, ByVal v_sGisDataModelCode As String) As Integer
        Dim result As Integer = 0
        Dim oBom As Object
        Dim sClassBOMAppName As String = ""
        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.UpdatePartyCnt(v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Destroy the BOM.Security class
                oBom.Dispose()
                oBom = Nothing
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyCntFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePartyCnt", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function PostcodeSearch(ByVal v_sHouseNameNum As String, ByVal v_sPostcode As String, ByRef r_vMatchArray As Object) As Integer
        Dim result As Integer = 0
        Dim oAddress As bPMAddressControl.BusinessOIS
        Dim lReturn, lNum As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            oAddress = New bPMAddressControl.BusinessOIS()
            result = oAddress.Interrogate(v_sPremiseID:=v_sHouseNameNum, v_sPostcode:=v_sPostcode, r_lNumMatches:=lNum, r_vArray:=r_vMatchArray)
            If lNum = 0 And v_sHouseNameNum.Trim() <> "" Then
                result = oAddress.Interrogate(v_sPremiseID:="", v_sPostcode:=v_sPostcode, r_lNumMatches:=lNum, r_vArray:=r_vMatchArray)
            End If

            oAddress = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostcodeSearchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostcodeSearch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function SaveInDB() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTopLevelObject, sTopLevelTable As String
        Dim lSaveToDBMode, lStartPosition, lEndPosition, lPos As Integer
        Dim sXMLDataSet, sXMLDataSetDef As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'PN19544 - Reset the Update Status for all newly added child objects within Deleted_Objects prior
            'to saving to the db
            lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            lStartPosition = (sXMLDataSet.IndexOf("<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """>") + 1)
            If lStartPosition > 0 Then
                ' Find end of deleted objects
                lEndPosition = (sXMLDataSet.IndexOf("</DELETED_OBJECTS>") + 1)
                If lEndPosition > 0 Then
                    'Amend all Deleted Objects with an Update Status of PMAdd to PMView so newly
                    'added Child Objects are not Inserted if the Parent Object has been deleted
                    lPos = Informations.inStr(lStartPosition, sXMLDataSet, " US=""1""")
                    Do While lPos <> 0 And lPos < lEndPosition
                        sXMLDataSet = sXMLDataSet.Remove(lPos - 1, 7).Insert(lPos - 1, " US=""0""")
                        lPos = Informations.inStr(lPos, sXMLDataSet, " US=""1""")
                    Loop
                End If
                lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            ' Need to set the Primary Key ID values for any new Objects that have been added
            lReturn = m_oDataSet.SetIDValues()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            'RFC110701 - How do we want to save to the Database
            lSaveToDBMode = GISSharedConstants.GetLoadSaveDBMode(m_oDataSet.GISDataModelCode)

            If lSaveToDBMode = GISSharedConstants.GISRegLoadSaveDBModeSlow Then
                ' Slow Mode
            Else
                ' Save Via XSL (Fast) and Exit
                lReturn = CType(SaveInDBViaXSL(), gPMConstants.PMEReturnCode)
                Return lReturn
            End If

            ' Get the Top Level Object Name
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Begin a Transaction
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save all Instances of the Top Level Object to the DB
            lReturn = CType(ObjectInstancesToDB(v_sObjectName:=sTopLevelObject), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Rollback the Transaction
                lReturn = m_oDatabase.SQLRollbackTrans()
                ' More to do here. If Failed to Rollback log an error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Delete any Deleted Objects from the DB
            lReturn = CType(DeleteObjectsInDB(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Rollback the Transaction
                lReturn = m_oDatabase.SQLRollbackTrans()
                ' More to do here. If Failed to Rollback log an error
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Commit the Transaction
            lReturn = m_oDatabase.SQLCommitTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' More to do here. Log a Proper message if failed to commit
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            lReturn = m_oDatabase.SQLRollbackTrans()
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveInDBFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveInDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function SaveInDBViaXSL() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""

        Dim oTime As bGIS.DebugTimings


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Do the Transform and generate the SQL Statements
        lReturn = m_oDataSet.GenSaveSQLViaXSL(sSQL)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        If sSQL.Length < 1 Then
            Return result
        End If

        sSQL = "BEGIN" & Strings.ChrW(13) & Strings.ChrW(10) & "SET NOCOUNT ON " & Strings.ChrW(13) & Strings.ChrW(10) &
        "SET QUOTED_IDENTIFIER OFF " & Strings.ChrW(13) & Strings.ChrW(10) &
        sSQL
        ' restore quoted identifier mode to the default
        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & "SET QUOTED_IDENTIFIER ON "
        ' RAW 25/06/2003 : CQ1521 : end

        sSQL = sSQL & Strings.ChrW(13) & Strings.ChrW(10) & " SET NOCOUNT OFF " & Strings.ChrW(13) & Strings.ChrW(10) & "END"
        ' Begin a Transaction
        m_oDatabase.Parameters.Clear()

        lReturn = m_oDatabase.SQLBeginTrans()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Process the SQL Statements
        lReturn = m_oDatabase.SQLAction(sSQL, "SaveInDBViaXSL", False)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Rollback the Transaction
            lReturn = m_oDatabase.SQLRollbackTrans()
            ' More to do here. If Failed to Rollback log an error
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        ' Commit the Transaction
        lReturn = m_oDatabase.SQLCommitTrans()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' More to do here. Log a Proper message if failed to commit
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Reset the Update Status
        lReturn = m_oDataSet.ResetUpdateStatusWholeDataset()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function

    Private Function StripQuote(ByRef r_sStringField As String) As Integer
        Dim result As Integer = 0
        Dim sTmp As String = ""
        Dim lTmp, lStart As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        lStart = 1
        lTmp = Informations.inStr(lStart, r_sStringField, "'")
        While (lTmp > 0)
            sTmp = r_sStringField.Substring(0, lTmp) & "'" & r_sStringField.Substring(r_sStringField.Length - (r_sStringField.Length - lTmp))
            r_sStringField = sTmp
            lStart = lTmp + 2
            lTmp = Informations.inStr(lStart, r_sStringField, "'")
        End While

        lTmp = (r_sStringField.IndexOf("|"c) + 1)
        If lTmp > 0 Then
            r_sStringField = r_sStringField.Remove(lTmp - 1, 1).Insert(lTmp - 1, "")
        End If

        lTmp = (r_sStringField.IndexOf(Strings.ChrW(34).ToString()) + 1)
        If lTmp > 0 Then
            r_sStringField = r_sStringField.Remove(lTmp - 1, 1).Insert(lTmp - 1, "")
        End If

        Return result
    End Function

    Private Function TempOIMTransact(ByVal v_lGISSchemeID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim oAutoNum As bPMAutoNumber.Business
        Dim lReturn As Integer
        Dim lEDINumRange, lPolNoNumRange As Integer
        Dim lEDINo, lPolNo As Integer
        Dim sPolNo, sEDINo As String
        Dim bIsAssumedInfo As Boolean
        Dim sPolicyKey As String = ""
        Dim vPolicyKeyArray As Object
        Dim dtCoverStartDate As Date
        Dim sXMLDataSet, sXMLDataSetDef As String
        Dim sPolicyObj As String = ""
        Dim lGISPolicyLinkID As Integer
        Dim sQuoteRef As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Work out what the Policy Object is called
        If v_sGisDataModelCode.Trim() = "OIM" Then
            sPolicyObj = "OIM_Policy"
        Else
            sPolicyObj = "XEM_Policy"
        End If
        oAutoNum = New bPMAutoNumber.Business
        lReturn = oAutoNum.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="gPMComponentServices.CreateBusinessObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Get the Policy Number Range
        lReturn = oAutoNum.GetNumberRange(v_sGroupCode:=TMPAutoNumGroupCode, v_sRangeCode:=TMPPolNoNumRange, v_sPMProductCode:=gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusSolutions), r_lNumberRangeID:=lPolNoNumRange)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oAutoNum.GetNumberRange 1 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Get the EDI Number Range
        lReturn = oAutoNum.GetNumberRange(v_sGroupCode:=TMPAutoNumGroupCode, v_sRangeCode:=TMPEDINumRange, v_sPMProductCode:=gPMConstants.PMProductCode(gPMConstants.PMEProductFamily.pmePFSiriusSolutions), r_lNumberRangeID:=lEDINumRange)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oAutoNum.GetNumberRange 2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If v_sGisDataModelCode.Trim() = "OIM" Then
            'Get the policy link id
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            'Generate the quote reference (again)
            lReturn = GenerateQuoteRef(v_lGISPolicyLinkID:=lGISPolicyLinkID, r_sQuoteRef:=sQuoteRef, v_sGisDataModelCode:=v_sGisDataModelCode)
        Else
            ' Generate the Policy Number
            lReturn = oAutoNum.GenerateNumber(v_lNumberRangeID:=lPolNoNumRange, v_iUserId:=1, r_lNumber:=lPolNo)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oAutoNum.GenerateNumber 1 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If

        lReturn = oAutoNum.GenerateNumber(v_lNumberRangeID:=lEDINumRange, v_iUserId:=1, r_lNumber:=lEDINo)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oAutoNum.GenerateNumber 2 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        oAutoNum.Dispose()
        oAutoNum = Nothing

        If v_sGisDataModelCode.Trim() = "OIM" Then
            sPolNo = sQuoteRef
            sEDINo = TMPEDIPrefix & StringsHelper.Format(lEDINo, TMPEDIFormat)
        End If

        If v_sGisDataModelCode.Trim() = "XEM" Then
            ' Format the Policy Number
            sPolNo = TMPPolNoPrefixXEM & StringsHelper.Format(lPolNo, TMPPolNoFormat)
            ' Format the EDI Number
            sEDINo = TMPEDIPrefixXEM & StringsHelper.Format(lEDINo, TMPEDIFormat)

        End If

        If (v_sGisDataModelCode.Trim() = "XEM") And (v_sGisBusinessTypeCode.Trim().ToUpper() = "I3M") Then
            ' Format the Policy Number
            sPolNo = TMPPolNoPrefixI3M & StringsHelper.Format(lPolNo, TMPPolNoFormat)
            ' Format the EDI Number
            sEDINo = TMPEDIPrefixI3M & StringsHelper.Format(lEDINo, TMPEDIFormat)

        End If

        ' Get the Policy Object Key
        lReturn = m_oDataSet.GetAllOIKey(v_sObjectName:=sPolicyObj, r_vOIKeyArray:=vPolicyKeyArray)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetAllOIKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' There should only be ONE Policy Key
        sPolicyKey = CStr(vPolicyKeyArray(vPolicyKeyArray.GetLowerBound(0)))

        ' Get the Cover Start Date

        lReturn = m_oDataSet.GetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="Effective_Start_Date", v_sOIKey:=sPolicyKey, r_vPropertyValue:=dtCoverStartDate, r_bIsAssumedInfo:=bIsAssumedInfo)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue PolicyObj Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Set the Inception Date to be the Cover Start Date

        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="Inception_Date", v_sOIKey:=sPolicyKey, v_vPropertyValue:=gPMFunctions.ToSafeDate(dtCoverStartDate), v_bIsAssumedInfo:=bIsAssumedInfo)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.GetPropertyValue Inception_Date Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Set the Policy Number
        lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sPolicyObj, v_sPropertyName:="Policy_No", v_sOIKey:=sPolicyKey, v_vPropertyValue:=sPolNo, v_bIsAssumedInfo:=False)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.SetPropertyValue Policy_No Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDataSet.ReturnAsXML Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If v_sGisDataModelCode.Trim() = "XEM" Then
            ' following bodge added on rob's permission for I3M transact- CL050400
            If v_sGisBusinessTypeCode.Trim().ToUpper() = "I3M" Then
                lReturn = CallQEMCreateEDI(v_sQEMName:="bgisqempoli3.i3m", v_lGISSchemeID:=v_lGISSchemeID, v_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_sGisDataModelCode:="XEM", v_sGisBusinessTypeCode:="i3m")

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMCreateEDI bgisqempoli3.i3m Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            Else
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="I AM HERE.....", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                lReturn = CallQEMCreateEDI(v_sQEMName:="bgisqempolxelector.xem", v_lGISSchemeID:=v_lGISSchemeID, v_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_sGisDataModelCode:="XEM", v_sGisBusinessTypeCode:="XEL")
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMCreateEDI bgisqempolxelector.xem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
        Else
            lReturn = CallQEMCreateEDI(v_sQEMName:="bgisqempolaris.oim", v_lGISSchemeID:=v_lGISSchemeID, v_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_sGisDataModelCode:="OIM", v_sGisBusinessTypeCode:="OIM")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMCreateEDI bgisqempolaris.oim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMCreateEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Save the Details
        lReturn = SaveInDB()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveInDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        lReturn = UpdatePolicyLinkSchemeID(v_lGISPolicyLinkID:=lGISPolicyLinkID, v_lGISSchemeID:=v_lGISSchemeID)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLinkSchemeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicyLinkSchemeID
    '
    ' Description: Update the scheme id in the gis_policy_link table.
    '              THese can then be used for
    ' This needs to be combined with the UpdatePolicyLinkTransact method.
    ' RFC210700
    '
    ' ***************************************************************** '
    Friend Function UpdatePolicyLinkSchemeID(ByRef v_lGISPolicyLinkID As Integer, ByRef v_lGISSchemeID As Integer) As Integer
        Dim result As Integer = 0
        Dim sDate As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            ' GIS policy link id
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add gis_policy_link_id Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' GIS scheme id
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add gis_scheme_id Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSchIdSQL, sSQLName:=ACUpdateSchIdName, bStoredProcedure:=ACUpdateSchIdStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TempOIMTransact", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run-time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkSchemeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Friend Function GetSchemeIDFromLink(ByVal v_lGISPolicyLinkID As Integer, ByRef r_lGisSchemeId As Integer) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            vArray = Nothing
            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyLinkSchIdSQL, sSQLName:=ACGetPolicyLinkSchIdName, bStoredProcedure:=ACGetPolicyLinkSchIdStored, vResultArray:=vArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lGISPolicyLinkID", v_lGISPolicyLinkID)
                oDict.Add("r_lGisSchemeId", r_lGisSchemeId)
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Scheme ID For Policy Link ID : " & v_lGISPolicyLinkID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeIDFromLink", oDicParms:=oDict)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lGisSchemeId = CInt(vArray(0, 0))

            vArray = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSchemeIDFromLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeIDFromLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Friend Function GetCurrentSchemeID(ByVal v_lOldGISSchemeID As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_lNewGISSchemeID As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            vArray = Nothing

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lOldGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RFC121000 - Add Business Type to Get Current SchemeId select
            lReturn = m_oDatabase.Parameters.Add(sName:="gis_business_type_code", vValue:=v_sGisBusinessTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCurrentSchemeIdSQL, sSQLName:=ACGetCurrentSchemeIdName, bStoredProcedure:=ACGetCurrentSchemeIdStored, vResultArray:=vArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vArray) Then
                'CONVERSION FOR LOGMESSAGEFILE OUTSIDE CATCH BLOCK
                '                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Current Scheme ID For Scheme ID : " & v_lOldGISSchemeID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeIDFromLink")
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lOldGISSchemeID", v_lOldGISSchemeID)
                oDict.Add("v_dtEffectiveDate", v_dtEffectiveDate)
                oDict.Add("r_lNewGISSchemeID", r_lNewGISSchemeID)
                oDict.Add("v_sGisBusinessTypeCode", v_sGisBusinessTypeCode)
                gPMFunctions.LogMessageToFile(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get the Current Scheme ID For Scheme ID : " & v_lOldGISSchemeID, vApp:=ACApp, vClass:=ACClass, vMethod:="GetSchemeIDFromLink", oDicParms:=oDict)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lNewGISSchemeID = CInt(vArray(0, 0))

            vArray = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentSchemeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentSchemeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function CopyDataSetViaSP(ByVal v_sDataModelCode As String, ByVal v_lSaveToDBMode As Integer, ByVal v_bCopyQuotes As Boolean, ByRef r_lNewGISPolicyLinkID As Integer, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, Optional ByVal v_vOldGISPolicyLinkId As Object = Nothing, Optional ByVal v_vOldInsuranceFileCnt As Object = Nothing, Optional ByVal v_vNewInsuranceFileCnt As Object = Nothing, Optional ByVal v_vOldRiskID As Object = Nothing, Optional ByVal v_vNewRiskID As Object = Nothing, Optional ByVal v_vNewQuoteRef As Object = Nothing, Optional ByVal v_vNewQuoteRefPassword As Object = Nothing) As Integer
        Dim result As Integer = 0
        m_lReturn = CType(CopyDataSetViaSPStateful(v_sDataModelCode:=v_sDataModelCode, v_lSaveToDBMode:=v_lSaveToDBMode, v_bCopyQuotes:=v_bCopyQuotes, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID, v_vNewRiskID:=v_vNewRiskID, v_vNewQuoteRef:=v_vNewQuoteRef, v_vNewQuoteRefPassword:=v_vNewQuoteRefPassword), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return LoadFromDBPrivate(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=v_sDataModelCode, r_lPolicyLinkID:=r_lNewGISPolicyLinkID)
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function CallQEMToRefer(ByVal v_sXMLDataSetDef As Object, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef v_sXMLDataSet As Object, ByVal v_sInsurerCode As String) As Integer
        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sGisDataModelCode <> "OIM" Then
                Return result
            End If

            lReturn = CType(CreateQEM(v_sQEMName:="bGISQEMPolaris.OIM", v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lReturn = oQEM.Refer(v_sXMLDataSetDef:=v_sXMLDataSetDef, r_sXMLDataset:=v_sXMLDataSet, v_sInsurerCode:=gPMFunctions.ToSafeString(v_sInsurerCode))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            sXMLDataSet = ""
            oQEM = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CallQEMToRefer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CallQEMToRefer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function SendEmail(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataset As String, ByVal v_lEMailType As Integer, ByVal v_sEMailFrom As Object, ByVal v_sEMailTo As Object, ByVal v_sEMailCC As String, ByVal v_sEMailSubject As Object, ByVal v_sEMailText As Object) As Integer
        Return SendEmail(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataset:=r_sXMLDataset, v_lEMailType:=v_lEMailType, v_sEMailFrom:=v_sEMailFrom, v_sEMailTo:=v_sEMailTo, v_sEMailCC:=v_sEMailCC, v_sEMailSubject:=v_sEMailSubject, v_sEMailText:=v_sEMailText, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function SendEmail(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataset As String, ByVal v_lEMailType As Integer, ByVal v_sEMailFrom As Object, ByVal v_sEMailTo As Object, ByVal v_sEMailCC As Object, ByVal v_sEMailSubject As Object, ByVal v_sEMailText As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim CDONTS As Object
        Dim oCDONTS As MailMessage
        Dim oBom As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a BackOfficeMapper object if required
            lReturn = CType(CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the Risk Data
            If r_sXMLDataset <> "" Then
                lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            ' Call the BOM SendEmailBefore method (if required)
            If Not (oBom Is Nothing) Then
                lReturn = oBom.SendEmailBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet,
                                               v_lEMailType:=gPMFunctions.ToSafeInteger(v_lEMailType), r_sEMailFrom:=v_sEMailFrom, r_sEMailTo:=v_sEMailTo, r_sEMailCC:=v_sEMailCC, r_sEMailSubject:=v_sEMailSubject,
                                               r_sEMailText:=v_sEMailText, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
            End If

            ' Check that we have a recipient
            If v_sEMailTo.Trim() = "" Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No recipient specified [EmailType=" & v_lEMailType & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="SendEmail")
                Return result
            End If

            If oCDONTS Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            With oCDONTS
                .From = v_sEMailFrom
                .Subject = v_sEMailSubject
                .Body = v_sEMailText
            End With

            oCDONTS = Nothing

            ' Call the BOM SendEmailAfter method (if required)
            If Not (oBom Is Nothing) Then
                lReturn = oBom.SendEmailAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_lEMailType:=gPMFunctions.ToSafeInteger(v_lEMailType), r_sEMailFrom:=v_sEMailFrom, r_sEMailTo:=v_sEMailTo, r_sEMailCC:=v_sEMailCC, r_sEMailSubject:=v_sEMailSubject, r_sEMailText:=v_sEMailText, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendEmail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SendEmail", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function VehicleLookup(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataset As String, ByVal v_sRegistrationNumber As String) As Integer
        Return VehicleLookup(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataset:=r_sXMLDataset, v_sRegistrationNumber:=v_sRegistrationNumber, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function VehicleLookup(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataset As String, ByVal v_sRegistrationNumber As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim oBom As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a BackOfficeMapper object if required
            lReturn = CType(CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the Risk Data
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Call the BOM VehicleLookup method (if required)
            If Not (oBom Is Nothing) Then
                lReturn = oBom.VehicleLookup(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_sRegistrationNumber:=gPMFunctions.ToSafeString(v_sRegistrationNumber), r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
            End If

            ' Get the Data Set
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="VehicleLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="VehicleLookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function ProcessLookupRequest(ByRef v_vMergedArray(,) As Object, ByVal v_sGisDataModelCode As String) As Integer
        Dim result As Integer = 0
        Dim bNew As Boolean
        Dim lReturn As Integer
        Dim sObjectName, sPropertyName As String
        Dim vSearchCode As String = ""
        Dim lNumberOfRecords As Integer
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object
        Dim bIsDirectLookup As Boolean 'DD 05122001

        ' this must work out whether the lookup is PM or User Def.
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sObjectName = CStr(v_vMergedArray(1, 0))
            sPropertyName = CStr(v_vMergedArray(1, 1))

            If v_vMergedArray.GetUpperBound(1) >= 2 Then
                If CStr(v_vMergedArray(1, 2)) <> "" Then
                    vSearchCode = CStr(v_vMergedArray(1, 2))
                End If
            End If

            If v_vMergedArray.GetUpperBound(1) > 2 Then
                ReDim vResultArray(0, 0)
                vResultArray(0, 0) = "0"
                bIsDirectLookup = True
            Else
                bIsDirectLookup = False
            End If

            If Not bIsDirectLookup Then
                sSQL = sSQL & "SELECT column1 = CASE prop.Specials_Type WHEN 2 THEN prop.Specials_Type_Reference WHEN 6 THEN '' ELSE '' END " & ", column2 = CASE prop.Specials_Type WHEN 2 THEN '' WHEN 6 THEN prop.Specials_Type_Reference ELSE '' END "
                sSQL = sSQL & "FROM gis_property prop, gis_object obj "
                sSQL = sSQL & "WHERE prop.gis_object_id = obj.gis_object_id "
                sSQL = sSQL & "AND prop.property_name = '" & sPropertyName & "' "
                sSQL = sSQL & "AND obj.object_name = '" & sObjectName & "'"

                lNumberOfRecords = 999

                lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetLookupType", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to establish lookup type", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessLookupRequest")
                    Return result
                End If

                If (CStr(vResultArray(0, 0)) <> "" And CStr(vResultArray(1, 0)) <> "") Or (CStr(vResultArray(0, 0)) = "" And CStr(vResultArray(1, 0)) = "") Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to establish lookup type", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessLookupRequest")
                    Return result
                End If
            End If

            If CStr(vResultArray(0, 0)) <> "" Then
                ' PM lookup
                If String.IsNullOrEmpty(vSearchCode) Then
                    lReturn = GetPMLookupList(v_iLookupType:=gPMConstants.PMELookupType.PMLookupAll, v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, r_vListData:=v_vMergedArray, oGISDB:=m_oDatabase, v_bGoDirect:=bIsDirectLookup)
                Else
                    lReturn = GetPMLookupList(v_iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, r_vListData:=v_vMergedArray, oGISDB:=m_oDatabase, v_sCode:=vSearchCode, v_bGoDirect:=bIsDirectLookup)
                End If

            Else
                If String.IsNullOrEmpty(vSearchCode) Then
                    lReturn = GetUserDefLookup(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, r_vUserDefList:=v_vMergedArray, oGISDB:=m_oDatabase)
                Else
                    lReturn = GetUserDefLookup(v_sObjectName:=sObjectName, v_sPropertyName:=sPropertyName, r_vUserDefList:=v_vMergedArray, oGISDB:=m_oDatabase, v_vCode:=vSearchCode)
                End If
            End If

            Return lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProcessLookupRequest Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessLookupRequest", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function GetPMLookupList(ByVal v_iLookupType As Integer, ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Array, ByVal oGISDB As dPMDAO.Database)
        GetPMLookupList(v_iLookupType:=v_iLookupType, v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_vListData:=r_vListData, oGISDB:=oGISDB, v_sCode:="", v_bGoDirect:=False)
    End Function
    Public Function GetPMLookupList(ByVal v_iLookupType As Integer, ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Array, ByVal oGISDB As dPMDAO.Database, ByRef v_sCode As String)
        GetPMLookupList(v_iLookupType:=v_iLookupType, v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_vListData:=r_vListData, oGISDB:=oGISDB, v_sCode:=v_sCode, v_bGoDirect:=False)
    End Function
    Public Function GetPMLookupList(ByVal v_iLookupType As Integer, ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Array, ByVal oGISDB As dPMDAO.Database, ByRef v_bGoDirect As Boolean)
        GetPMLookupList(v_iLookupType:=v_iLookupType, v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_vListData:=r_vListData, oGISDB:=oGISDB, v_sCode:="", v_bGoDirect:=v_bGoDirect)
    End Function
    Public Function GetPMLookupList(ByVal v_iLookupType As Integer, ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vListData As Array, ByVal oGISDB As dPMDAO.Database, ByRef v_sCode As String, ByRef v_bGoDirect As Boolean) As Integer
        Dim result As Integer = 0
        Dim lReturn, lNumberOfRecords, lID As Integer
        Dim dtEffectiveDate As Date
        Dim sTableName, sSQL As String
        Dim vTableArray(3, 0) As Object
        Dim vLookupResult, vResultArray(,) As Object
        Dim oPMLookup As BPMLOOKUP.Business

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If v_bGoDirect Then
                sTableName = v_sObjectName
            Else
                sSQL = "SELECT prop.specials_type_reference FROM gis_property prop, gis_object obj "
                sSQL = sSQL & "WHERE prop.gis_object_id = obj.gis_object_id "
                sSQL = sSQL & "AND prop.property_name = '" & v_sPropertyName & "' AND obj.object_name = '" & v_sObjectName & "' AND prop.specials_type = 2"

                lReturn = oGISDB.SQLSelect(sSQL:=sSQL, sSQLName:="GetPMLookupTableName", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLookup table name", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList")
                    Return result
                End If
                sTableName = CStr(vResultArray(0, 0))
            End If

            ' create the PMLookup object
            oPMLookup = New BPMLOOKUP.Business()

            lReturn = oPMLookup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn

                oPMLookup = Nothing
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise bPMLookup", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList")

                Return result
            End If

            If v_bGoDirect Then
                'if we're going direct then point lookup to SBO/S4U
                oPMLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            End If

            dtEffectiveDate = DateTime.Now

            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = sTableName
            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, 0) = 1
            vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, 0) = 1

            ' only if looking for one item
            If v_iLookupType = gPMConstants.PMELookupType.PMLookupSingle Then
                ' convert supplied code to ID
                lReturn = oPMLookup.GetEffectiveIDFromCode(sTableName, v_sCode, dtEffectiveDate, lID)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    oPMLookup.Dispose()
                    oPMLookup = Nothing
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetLookupValues from bPMLookup", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList")
                    Return result
                End If

                vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = lID
            End If

            ' call the search
            lReturn = oPMLookup.GetLookupValues(iLookupType:=v_iLookupType, vTableArray:=vTableArray, iLanguageID:=m_iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vLookupResult)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                oPMLookup.Dispose()
                oPMLookup = Nothing
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetLookupValues from bPMLookup", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList")
                Return result
            End If

            'finished with PMLookup
            oPMLookup.Dispose()

            oPMLookup = Nothing

            If Not Informations.IsArray(vLookupResult) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to find description for specified lookup code(Table: " & sTableName & ", Code: " & v_sCode & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList")

                Return result
            Else
                ' do we need whole array, or just search for code?
                If v_iLookupType <> gPMConstants.PMELookupType.PMLookupSingle Then
                    ' get whole array
                    r_vListData = Array.CreateInstance(GetType(Object), New Integer() {2, vLookupResult.GetUpperBound(1) - vLookupResult.GetLowerBound(1) + 1}, New Integer() {0, vLookupResult.GetLowerBound(1)})

                    For lRowNum As Integer = vLookupResult.GetLowerBound(1) To vLookupResult.GetUpperBound(1)
                        If v_bGoDirect Then
                            r_vListData(0, lRowNum) = vLookupResult(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lRowNum)
                        Else
                            r_vListData(0, lRowNum) = vLookupResult(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lRowNum)
                        End If
                        r_vListData(1, lRowNum) = vLookupResult(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lRowNum)
                    Next
                Else
                    ' search list for supplied code
                    For lRowNum As Integer = vLookupResult.GetLowerBound(1) To vLookupResult.GetUpperBound(1)
                        If CStr(vLookupResult(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lRowNum)).Trim() = v_sCode Then
                            r_vListData = Array.CreateInstance(GetType(Object), New Integer() {2, 1}, New Integer() {0, 0})
                            r_vListData(0, 0) = v_sCode
                            r_vListData(1, 0) = vLookupResult(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lRowNum)
                            Exit For
                        End If
                    Next
                End If
            End If

            If Not Informations.IsArray(r_vListData) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Specified code is not in lookup list (Table: " & sTableName & ", Code: " & v_sCode & ")", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList")
                Return result
            End If

            ' arrays populated
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' close PMLookup
            If Not (oPMLookup Is Nothing) Then
                oPMLookup.Dispose()
                oPMLookup = Nothing
            End If
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function GetUserDefLookup(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vUserDefList(,) As Object, ByVal oGISDB As dPMDAO.Database) As gPMConstants.PMEReturnCode
        Return GetUserDefLookup(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, r_vUserDefList:=r_vUserDefList, oGISDB:=oGISDB, v_vCode:=Nothing)
    End Function

    Public Function GetUserDefLookup(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByRef r_vUserDefList(,) As Object, ByVal oGISDB As dPMDAO.Database, ByVal v_vCode As Object) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lRowNum, lReturn, lHeaderID, lNumberOfRecords As Integer
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT prop.specials_type_reference FROM gis_property prop, gis_object obj "
            sSQL = sSQL & "WHERE prop.property_name = '" & v_sPropertyName & "' "
            sSQL = sSQL & "AND obj.object_name = '" & v_sObjectName & "' "
            sSQL = sSQL & "AND prop.gis_object_id = obj.gis_object_id" ' "
            sSQL = sSQL & " AND prop.specials_type = 6"

            lReturn = oGISDB.SQLSelect(sSQL:=sSQL, sSQLName:="GetUserDefHeaderID", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get User Def Header ID from Object and Property name", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefLookup")

                Return result
            End If

            lHeaderID = CInt(vResultArray(0, 0))

            ' get code and description
            sSQL = "SELECT gis_user_def_detail_id, description FROM gis_user_def_detail "
            sSQL = sSQL & "WHERE gis_user_def_header_id = " & CStr(lHeaderID)

            ' add code value if single item search
            If Not Informations.IsNothing(v_vCode) Then
                sSQL = sSQL & " AND gis_user_def_detail_id = " & CStr(v_vCode)
            End If

            lNumberOfRecords = 999

            lReturn = oGISDB.SQLSelect(sSQL:=sSQL, sSQLName:="GetDescFromCode", bStoredProcedure:=False, lNumberRecords:=lNumberOfRecords, vResultArray:=vResultArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get GIS User Def codes & descriptions from gis_user_def_detail", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefLookup")
                Return result
            End If

            r_vUserDefList = vResultArray

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserDefLookup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserDefLookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function InitialiseDataSetCheck(ByVal v_sGisDataModelCode As String, ByRef r_sTimestamp As String) As Integer
        Return InitialiseDataSetCheck(v_sGisDataModelCode:=v_sGisDataModelCode, r_sTimestamp:=r_sTimestamp, v_sGisBusinessTypeCode:="")
    End Function

    Public Function InitialiseDataSetCheck(ByVal v_sGisDataModelCode As String, ByRef r_sTimestamp As String, ByVal v_sGisBusinessTypeCode As String) As Integer
        Dim result As Integer = 0
        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' get the empty datasets, recreate if missing
            m_lReturn = CType(GetDataModelDef(v_sGisDataModelCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' get the DSD timestamp
            r_sTimestamp = m_oDataSet.GISDSDTimestamp

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseDataSetCheck Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseDataSetCheck", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function GetDatasetDetails(ByRef r_sDataSetDef As String, ByRef r_sDataset As String) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sDataset, r_sXMLDataSetDef:=r_sDataSetDef), gPMConstants.PMEReturnCode)
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDatasetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDatasetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    Public Overloads Function RecreateDatasets(ByVal v_sGisDataModelCode As String) As Integer
        Return RecreateDatasets(v_sGisDataModelCode, False)
    End Function

    Public Overloads Function RecreateDatasets(ByVal v_sGisDataModelCode As String, ByRef bRecreateDataBackBone As Boolean) As Integer
        Dim result As Integer = 0
        Dim sDSDfilename, sDSfilename, sXSDfilename, sSetIDFilename, sSaveXSLFilename, sSPName, sSPPath, sFile As String
        Dim vFileArray, vSPNameArray As Object
        Dim lFrom, lTo As Integer
        Dim sFilename, sStoredProcedureName, strSQL As String
        Dim lFilePointer As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        'clear the DSD file from the cache regardless of whether this succeeds or not, and delete the cache manager file
        'if it fails there's something more fundamentally broken and if DRE is called it will recreate the cached item anyway
        Dim sKey As String = DefaultDatasetKey & v_sGisDataModelCode
        Dim sFilepath As String = String.Empty
        Dim sCachePath As String = String.Empty

        Try
            Try
                iCache = New MemoryCache("PureCache")
            Catch ex As Exception
            End Try

            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                iCache.Remove(sKey)
            End If

        Catch ex As Exception
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve cache manager.", vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMError

        Finally
            'delete the file regardless of whether the item was in the cache or not as this forces the refresh of the cache
            result = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                       v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                       v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                       v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to retrieve cache path from registry.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
            End If

            If Right(sCachePath, 1) <> "\" Then
                sCachePath += "\"
            End If
            sFilepath = sCachePath + sKey + ".xml"
            File.Delete(sFilepath)
        End Try

        ' fetch the file names
        m_lReturn = CType(GISSharedConstants.GetDataSetFileNames(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetDefFile:=sDSDfilename, r_sDataSetFile:=sDSfilename), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' fetch the XSD file name
        m_lReturn = CType(GISSharedConstants.GetDataSetXSDFileName(v_sDataModelCode:=v_sGisDataModelCode, r_sDataSetXSDFile:=sXSDfilename), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the SetID file name
        m_lReturn = CType(GISSharedConstants.GetSetIDXSLFileName(v_sGisDataModelCode:=v_sGisDataModelCode, r_sSetIDXSLFileName:=sSetIDFilename), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the SaveXSL file name
        m_lReturn = CType(GISSharedConstants.GetSaveXSLFileName(v_sGisDataModelCode:=v_sGisDataModelCode, r_sSaveXSLFileName:=sSaveXSLFilename), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Path for the Load/Save Stored Procedures
        m_lReturn = CType(GISSharedConstants.GetLoadSPPath(v_sDataModelCode:=v_sGisDataModelCode, r_sLoadSPPath:=sSPPath), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Build a search string for the Stored Procs for this DataModel
        sSPName = sSPPath & "\" & "spg_" & v_sGisDataModelCode & "_*.sql"
        ' delete the files, ignore errors

        Try
            File.Delete(sDSDfilename)
            File.Delete(sDSfilename)
            File.Delete(sSetIDFilename)
            File.Delete(sSaveXSLFilename)

            ' Kill all Stored Procedures for this Datamodel
            If Directory.Exists(sSPPath) Then
                For Each dfile As String In Directory.GetFiles(sSPPath, "spg_" & v_sGisDataModelCode & "_*.sql")
                    File.Delete(dfile)
                Next
            Else
                Directory.CreateDirectory(sSPPath)
            End If

            File.Delete(sXSDfilename)

            ' calling this function will recreate the files
            m_lReturn = CType(GetDataModelDef(v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetAllStoredProcedures(v_sPath:=sSPPath, v_sDataModelCode:=v_sGisDataModelCode, r_vFileArray:=vFileArray, r_vSPNameArray:=vSPNameArray), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' We got all the Stored procedures names, so add it to the database using dPMDAO
            If Informations.IsArray(vFileArray) And Informations.IsArray(vSPNameArray) Then
                lFrom = vFileArray.GetLowerBound(0)
                lTo = vFileArray.GetUpperBound(0)

                For lCounter As Integer = lFrom To lTo
                    ' Get stored procedure details
                    sFilename = CStr(vFileArray(lCounter)).Trim()
                    sStoredProcedureName = CStr(vSPNameArray(lCounter)).Trim()

                    If sFilename.Length > 0 And sStoredProcedureName.Length > 0 Then
                        ' Code for Drop Stored Procedure
                        strSQL = "EXEC DDLDropProcedure '" & sStoredProcedureName & "'"

                        ' Call the dPMDAO to drop the stored procedure
                        m_lReturn = m_oDatabase.SQLAction(sSQL:=strSQL, sSQLName:="Drop_Datamodel_Stored_Procedure", bStoredProcedure:=False)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Error happend while dropping the stored procedure
                            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecreateDatasets Failed. Failed to drop Stored Procedure - " & sStoredProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            Return result
                        End If

                        strSQL = ""

                        Dim sFullPath As String = sSPPath & "\" & sFilename
                        ' Get the content of the stored procedure
                        Dim line As String = ""
                        ' Create new StreamReader instance with Using block.
                        Using reader As StreamReader = New StreamReader(sFullPath)
                            ' Read one line from file
                            Do While (Not line Is Nothing)
                                ' Add this line to list.
                                line = reader.ReadLine
                                strSQL = strSQL & line & Strings.ChrW(13) & Strings.ChrW(10)
                            Loop
                        End Using

                        ' Code to Add the Stored Procedure to the Database
                        If strSQL.Trim().Length > 0 Then

                            ' Call the dPMDAO to set quoted identifier
                            ' this must be done because the GIS stored procedures
                            ' use quoted string to generate dynamic sql
                            m_lReturn = m_oDatabase.SQLAction(sSQL:="SET QUOTED_IDENTIFIER OFF", sSQLName:="Set_Quoted_Indentifiers OFF", bStoredProcedure:=False)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Error happend while dropping the stored procedure
                                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecreateDatasets Failed. Failed to drop Stored Procedure - " & sStoredProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result
                            End If
                            ' strSQL = "SET QUOTED_IDENTIFIER OFF " & strSQL
                            ' Call the dPMDAO to add the stored procedure
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=strSQL, sSQLName:="Create_Datamodel_Stored_Procedure", bStoredProcedure:=False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Error happend while adding the stored procedure
                                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecreateDatasets Failed. Failed to add Stored Procedure - " & sStoredProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result
                            End If

                            ' Call the dPMDAO to reset the quoted identifier
                            m_lReturn = m_oDatabase.SQLAction(sSQL:="SET QUOTED_IDENTIFIER ON", sSQLName:="Set_Quoted_Indentifiers ON", bStoredProcedure:=False)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                ' Error happend while dropping the stored procedure
                                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecreateDatasets Failed. Failed to drop Stored Procedure - " & sStoredProcedureName, vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                Return result
                            End If
                        End If
                    End If
                Next lCounter
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                ' We haven't got any Array, so missing stored procedures for the datamodel
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecreateDatasets Failed. Stored Procedures are missing. sSPPath = " & sSPPath, vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Dim sCCMDocProduction As String = String.Empty
            m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=0, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                            v_iCurrencyID:=1, v_iLogLevel:=0, v_sCallingAppName:=ACApp, v_iOptionNumber:=GeneralConst.kSystemOptionDocumentProductionSystem,
                                            r_sOptionValue:=sCCMDocProduction)

            'if CCM is enabled or SCH is Enabled
            If sCCMDocProduction = "1" OrElse sCCMDocProduction = "2" Then
                Dim ccmDocumentProdBusiness As bCCMDocumentProduction.Business = New bCCMDocumentProduction.Business
                result = ccmDocumentProdBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                result = ccmDocumentProdBusiness.CCMRecreateDataSets(sGISDataModelCode:=v_sGisDataModelCode, bRecreateDataBackBone:=bRecreateDataBackBone)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return gPMConstants.PMEReturnCode.PMTrue

Err_RecreateDatasets:

            result = gPMConstants.PMEReturnCode.PMError

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecreateDatasets Failed. Stored Procedures will not be updated in database.", vApp:=ACApp, vClass:=ACClass, vMethod:="RecreateDatasets", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result

        Catch exc As System.Exception
            '''NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
        Return result
    End Function

    Public Function GetRegSetting(ByVal v_lPMERegSettingRoot As gPMConstants.PMERegSettingRoot, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As gPMConstants.PMERegSettingLevel, ByVal v_sSettingName As String, ByRef r_sSettingValue As String) As Integer
        Return GetRegSetting(v_lPMERegSettingRoot:=v_lPMERegSettingRoot, v_lPMEProductFamily:=v_lPMEProductFamily, v_lPMERegSettingLevel:=v_lPMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=r_sSettingValue, v_sSubKey:="")
    End Function

    Public Function GetRegSetting(ByVal v_lPMERegSettingRoot As gPMConstants.PMERegSettingRoot, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As gPMConstants.PMERegSettingLevel, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, ByVal v_sSubKey As String) As Integer
        Dim result As Integer = 0
        Try
            Dim sSettingValue As String = ""

            result = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot, v_lPMEProductFamily, v_lPMERegSettingLevel, v_sSettingName, sSettingValue, v_sSubKey)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                r_sSettingValue = sSettingValue
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSetting", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetAddress(ByRef lAddressCnt As Integer, ByRef vAddressArray As Object) As Integer
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetAddressFromAddressCnt")

        Try
            result = PBGetAddressFromAddressCnt.GetAddressFromAddressCnt(m_oDatabase, CStr(lAddressCnt), vAddressArray)
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetAddressFromAddressCnt")
            Return result
        Catch excep As System.Exception
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetAddressFromAddressCnt")
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAddress Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAddress", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function UpdatePolicyLinkSchemeIDViaInsFileCnt(ByRef v_lOldInsuranceFileCnt As Integer, ByRef v_lNewInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode


        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()

        ' Old Insurance File Cnt
        lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add OldInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkSchemeIDViaInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' New Insurance File Cnt
        lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add NewInsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkSchemeIDViaInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSchIdViaInsFileCntSQL, sSQLName:=ACUpdateSchIdViaInsFileCntName, bStoredProcedure:=ACUpdateSchIdViaInsFileCntStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLinkSchemeIDViaInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        Return result
    End Function


    ''' <summary>
    ''' AddQuote
    ''' </summary>
    ''' <param name="v_sGisDataModelCode"></param>
    ''' <param name="v_sGisBusinessTypeCode"></param>
    ''' <param name="v_dtEffectiveDate"></param>
    ''' <param name="v_dtExpirationDate"></param>
    ''' <param name="v_sInsuredName"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="r_lAgentCnt"></param>
    ''' <param name="r_lInsuranceFolderCnt"></param>
    ''' <param name="r_lInsuranceFileCnt"></param>
    ''' <param name="v_sInsuranceFolderDescription"></param>
    ''' <param name="r_sInsuranceFileRef"></param>
    ''' <param name="r_lRiskCodeId"></param>
    ''' <param name="v_lSourceID"></param>
    ''' <param name="r_lInsurerCnt"></param>
    ''' <param name="v_lscreenid"></param>
    ''' <param name="v_vAlternateReference"></param>
    ''' <param name="r_lRiskCnt"></param>
    ''' <param name="r_lRiskGroupId"></param>
    ''' <param name="r_lGisSchemeId"></param>
    ''' <param name="r_vAdditionalDataArray"></param>
    ''' <param name="v_lCurrencyID"></param>
    ''' <param name="v_lAnalysisCodeId"></param>
    ''' <param name="v_sPolicyStatusCode"></param>
    ''' <param name="v_lPolicyVersion"></param>
    ''' <param name="v_sRenewalFrequency"></param>
    ''' <param name="v_sInsuranceFileStructure"></param>
    ''' <param name="v_sBusinessType"></param>
    ''' <param name="v_sPaymentMethod"></param>
    ''' <param name="v_blConsLeadAgntComm"></param>
    ''' <param name="v_blConsSubAgntComm"></param>
    ''' <param name="v_lLapsedReasonId"></param>
    ''' <param name="v_dtLapsedDate"></param>
    ''' <param name="v_sLapsedReasonDescription"></param>
    ''' <param name="v_dtInceptionDate"></param>
    ''' <param name="v_dtInceptionDateTPI"></param>
    ''' <param name="v_dtRenewalDate"></param>
    ''' <param name="v_sAlternateReference"></param>
    ''' <param name="v_sOldPolicyNumber"></param>
    ''' <param name="v_sAccountExecutiveShortname"></param>
    ''' <param name="v_sAccountHandlerShortname"></param>
    ''' <param name="v_sInsuranceFileTypeCode"></param>
    ''' <param name="sCoInsurancePlacement"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function AddQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String,
                             ByVal v_dtEffectiveDate As Date, ByVal v_dtExpirationDate As Date,
                             ByVal v_sInsuredName As String, ByVal v_lPartyCnt As Integer, ByRef r_lAgentCnt As Integer,
                             ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer,
                             Optional ByVal v_sInsuranceFolderDescription As String = "",
                             Optional ByRef r_sInsuranceFileRef As String = "",
                             Optional ByRef r_lRiskCodeId As Integer = 0, Optional ByVal v_lSourceID As Integer = 0,
                             Optional ByRef r_lInsurerCnt As Integer = 0, Optional ByVal v_lscreenid As Integer = 0,
                             Optional ByVal v_vAlternateReference As Object = Nothing,
                             Optional ByRef r_lRiskCnt As Integer = 0, Optional ByRef r_lRiskGroupId As Integer = 0,
                             Optional ByRef r_lGisSchemeId As Integer = 0,
                             Optional ByRef r_vAdditionalDataArray As Object = Nothing,
                             Optional ByVal v_lCurrencyID As Integer = 0,
                             Optional ByVal v_lAnalysisCodeId As Integer = 0,
                             Optional ByVal v_sPolicyStatusCode As String = "",
                             Optional ByVal v_lPolicyVersion As Integer = 0,
                             Optional ByVal v_sRenewalFrequency As String = "",
                             Optional ByVal v_sInsuranceFileStructure As String = "",
                             Optional ByVal v_sBusinessType As String = "",
                             Optional ByVal v_sPaymentMethod As String = "",
                             Optional ByVal v_blConsLeadAgntComm As Boolean = False,
                             Optional ByVal v_blConsSubAgntComm As Boolean = False,
                             Optional ByVal v_lLapsedReasonId As Integer = 0,
                             Optional ByVal v_dtLapsedDate As Date = GISSharedConstants.GISLowDate,
                             Optional ByVal v_sLapsedReasonDescription As String = "",
                             Optional ByVal v_dtInceptionDate As Date = GISSharedConstants.GISLowDate,
                             Optional ByVal v_dtInceptionDateTPI As Date = GISSharedConstants.GISLowDate,
                             Optional ByVal v_dtRenewalDate As Date = GISSharedConstants.GISLowDate,
                             Optional ByVal v_sAlternateReference As String = "",
                             Optional ByVal v_sOldPolicyNumber As String = "",
                             Optional ByVal v_sAccountExecutiveShortname As String = "",
                             Optional ByVal v_sAccountHandlerShortname As String = "",
                             Optional ByVal v_sInsuranceFileTypeCode As String = "",
                             Optional ByVal sCoInsurancePlacement As String = "") As Integer

        Dim nResult As Integer = 0
        Dim nReturn As Integer
        Dim oBom As Object
        Dim sQuoteRef As String
        Dim sQuoteRefPassword As String
        Dim nInsuranceFolderCnt As Object
        Dim nInsuranceFileCnt As Object
        Dim sInsuranceFileRef As Object = ""

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            nReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode,
                                v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACClass,
                                r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="AddQuote Failed to Create BOM", vApp:=ACApp,
                                                  vClass:=ACClass, vMethod:="AddQuote",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            If Not (oBom Is Nothing) Then
                nInsuranceFolderCnt = r_lInsuranceFolderCnt
                nInsuranceFileCnt = r_lInsuranceFileCnt
                sInsuranceFileRef = r_sInsuranceFileRef
                Dim lAgentCnt As Object = r_lAgentCnt
                Dim lRiskCodeId As Object = r_lRiskCodeId
                Dim lInsurerCnt As Object = r_lInsurerCnt
                Dim lRiskCnt As Object = r_lRiskCnt
                Dim lRiskGroupId As Object = r_lRiskGroupId
                Dim lGisSchemeId As Object = r_lGisSchemeId

                nReturn = oBom.AddQuote(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode),
                                        v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode),
                                        v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_dtExpirationDate:=gPMFunctions.ToSafeDate(v_dtExpirationDate),
                                        v_sInsuredName:=gPMFunctions.ToSafeString(v_sInsuredName), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt),
                                        r_lAgentCnt:=lAgentCnt, r_lInsuranceFolderCnt:=nInsuranceFolderCnt,
                                        r_lInsuranceFileCnt:=nInsuranceFileCnt,
                                        v_sInsuranceFolderDescription:=gPMFunctions.ToSafeString(v_sInsuranceFolderDescription),
                                        r_sInsuranceFileRef:=sInsuranceFileRef, r_lRiskCodeId:=lRiskCodeId,
                                        v_lSourceID:=gPMFunctions.ToSafeInteger(v_lSourceID), r_lInsurerCnt:=lInsurerCnt,
                                        v_lscreenid:=gPMFunctions.ToSafeInteger(v_lscreenid), v_vAlternateReference:=v_vAlternateReference,
                                        r_lRiskCnt:=lRiskCnt, r_lRiskGroupId:=lRiskGroupId, r_lGisSchemeId:=lGisSchemeId,
                                        r_vAdditionalDataArray:=r_vAdditionalDataArray, v_lCurrencyID:=gPMFunctions.ToSafeInteger(v_lCurrencyID),
                                        v_lAnalysisCodeId:=gPMFunctions.ToSafeInteger(v_lAnalysisCodeId),
                                        v_sPolicyStatusCode:=gPMFunctions.ToSafeString(v_sPolicyStatusCode), v_lPolicyVersion:=gPMFunctions.ToSafeInteger(v_lPolicyVersion),
                                        v_sRenewalFrequency:=gPMFunctions.ToSafeString(v_sRenewalFrequency),
                                        v_sInsuranceFileStructure:=gPMFunctions.ToSafeString(v_sInsuranceFileStructure),
                                        v_sBusinessType:=gPMFunctions.ToSafeString(v_sBusinessType), v_sPaymentMethod:=gPMFunctions.ToSafeString(v_sPaymentMethod),
                                        v_blConsLeadAgntComm:=gPMFunctions.ToSafeBoolean(v_blConsLeadAgntComm),
                                        v_blConsSubAgntComm:=gPMFunctions.ToSafeBoolean(v_blConsSubAgntComm),
                                        v_lLapsedReasonId:=gPMFunctions.ToSafeInteger(v_lLapsedReasonId), v_dtLapsedDate:=gPMFunctions.ToSafeDate(v_dtLapsedDate),
                                        v_sLapsedReasonDescription:=gPMFunctions.ToSafeString(v_sLapsedReasonDescription),
                                        v_dtInceptionDate:=gPMFunctions.ToSafeDate(v_dtInceptionDate),
                                        v_dtInceptionDateTPI:=gPMFunctions.ToSafeDate(v_dtInceptionDateTPI), v_dtRenewalDate:=gPMFunctions.ToSafeDate(v_dtRenewalDate),
                                        v_sAlternateReference:=gPMFunctions.ToSafeString(v_sAlternateReference),
                                        v_sOldPolicyNumber:=gPMFunctions.ToSafeString(v_sOldPolicyNumber),
                                        v_sAccountExecutiveShortname:=gPMFunctions.ToSafeString(v_sAccountExecutiveShortname),
                                        v_sAccountHandlerShortname:=gPMFunctions.ToSafeString(v_sAccountHandlerShortname),
                                        v_sInsuranceFileTypeCode:=gPMFunctions.ToSafeString(v_sInsuranceFileTypeCode))



                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="BOM AddQuote Method Failed", vApp:=ACApp,
                                                      vClass:=ACClass, vMethod:="AddQuote", vErrNo:=nReturn,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                r_lAgentCnt = lAgentCnt
                r_lRiskCodeId = lRiskCodeId
                r_lInsurerCnt = lInsurerCnt
                r_lRiskCnt = lRiskCnt
                r_lRiskGroupId = lRiskGroupId
                r_lGisSchemeId = lGisSchemeId
                r_lInsuranceFolderCnt = nInsuranceFolderCnt
                r_lInsuranceFileCnt = nInsuranceFileCnt
                r_sInsuranceFileRef = sInsuranceFileRef
            End If

            oBom.Dispose()
            oBom = Nothing

            Return nResult
        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:=
                                                 "AddQuote Failed" &
                                                 (If(Informations.Err().Number = 438,
                                                      ". Please ensure that the GIS Datamodel is accessible via SAM by checking the setting in the GIS Datamodel Editor for Datamodel code - " &
                                                      v_sGisDataModelCode, "")), vApp:=ACApp, vClass:=ACClass,
                                              vMethod:="AddQuote", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function

    Public Function AddRisk(ByVal v_sBackOfficeMapperCode As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lRiskScreenId As Integer, ByVal v_sRiskDescription As String, ByVal v_lProductID As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_lRiskCnt As Integer, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String) As Integer
        Return AddRisk(v_sBackOfficeMapperCode:=v_sBackOfficeMapperCode, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPartyCnt:=v_lPartyCnt, v_lRiskTypeId:=v_lRiskTypeId, v_lRiskScreenId:=v_lRiskScreenId, v_sRiskDescription:=v_sRiskDescription, v_lProductID:=v_lProductID, r_lRiskFolderCnt:=r_lRiskFolderCnt, r_lRiskCnt:=r_lRiskCnt, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword, r_vAdditionalDataArray:=Nothing)
    End Function

    Public Function AddRisk(ByVal v_sBackOfficeMapperCode As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_lRiskScreenId As Integer, ByVal v_sRiskDescription As String, ByVal v_lProductID As Integer, ByRef r_lRiskFolderCnt As Integer, ByRef r_lRiskCnt As Integer, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBom As Object
        Dim sQuoteRef, sQuoteRefPassword, sTopOIKey, sXMLDataSetDef, sXMLDataSet As String
        Dim lPolicyLinkID As Integer
        Dim lRiskCnt As Object
        Dim oOptions As Object
        Dim sSystem, sMessage As String
        Dim lRiskFolderCnt As Object 'must use a local long or this will not work with the STS
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sBackOfficeMapperCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed to Create BOM", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not (oBom Is Nothing) Then
                lRiskFolderCnt = r_lRiskFolderCnt 'copy into the local and use that
                lRiskCnt = r_lRiskCnt
                lReturn = oBom.AddRisk(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt),
                                       v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_sQuoteRef:=gPMFunctions.ToSafeString(sQuoteRef), v_sQuoteRefPassword:=gPMFunctions.ToSafeString(sQuoteRefPassword),
                                       v_lRiskTypeId:=gPMFunctions.ToSafeInteger(v_lRiskTypeId), v_lRiskScreenId:=gPMFunctions.ToSafeInteger(v_lRiskScreenId), v_sRiskDescription:=gPMFunctions.ToSafeString(v_sRiskDescription),
                                       v_lProductID:=gPMFunctions.ToSafeInteger(v_lProductID), r_lRiskFolderCnt:=lRiskFolderCnt, r_lRiskCnt:=lRiskCnt, r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM AddRisk Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                m_oDataSet = oDataSet
                r_lRiskFolderCnt = lRiskFolderCnt
                r_lRiskCnt = lRiskCnt
                If r_lPolicyLinkID > 0 Then
                    lPolicyLinkID = r_lPolicyLinkID
                Else
                    r_lPolicyLinkID = lPolicyLinkID
                End If

                r_sTopOIKey = sTopOIKey
                r_sQuoteRef = sQuoteRef
                r_sQuoteRefPassword = sQuoteRefPassword
                r_lRiskCnt = lRiskCnt
                r_lRiskFolderCnt = lRiskFolderCnt 'put the local value back into the passed

                lReturn = NewRiskDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=lPolicyLinkID, r_sTopOIKey:=sTopOIKey, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_lInsuranceFileCnt:=v_lInsuranceFolderCnt, r_sQuoteRef:=sQuoteRef, r_sQuoteRefPassword:=sQuoteRefPassword, v_lRiskID:=lRiskCnt)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed to Create a NEW DataSet", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' CTAF - Return the values
                r_lPolicyLinkID = lPolicyLinkID
                r_sQuoteRef = sQuoteRef
                r_sQuoteRefPassword = sQuoteRefPassword

                lReturn = SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed to Save Data to DB", vApp:=ACApp, vClass:=ACClass, vMethod:="AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                r_sXMLDataSetDef = sXMLDataSetDef
                r_sXMLDataset = sXMLDataSet
                oBom.Dispose()
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="!AddRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, sUsername:=gPMFunctions.ToSafeString(m_sUsername), excep:=excep)
            Return result
        End Try
    End Function

    Public Function GeneratePolicyDocument(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lProcessTypesDocId As Integer, ByVal v_lDocumentTemplateId As Integer, ByVal v_lDocumentTypeId As Integer, ByRef r_sMergedFilePath As Object, ByRef r_sSpooledFilePath As Object, ByRef r_vAdditionalDataArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBom As Object
        Dim lInsuranceFolderCnt, lInsuranceFileCnt As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePolicyDocument Failed to Create BOM", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not (oBom Is Nothing) Then

                lInsuranceFolderCnt = v_lInsuranceFolderCnt
                lInsuranceFileCnt = v_lInsuranceFileCnt

                ' PN20978 Used to call GenerateDocument

                lReturn = oBom.GeneratePolicyDocument(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(lInsuranceFolderCnt),
                                                      v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), v_lProcessTypesDocId:=gPMFunctions.ToSafeInteger(v_lProcessTypesDocId), v_lDocumentTemplateId:=gPMFunctions.ToSafeInteger(v_lDocumentTemplateId), v_lDocumentTypeId:=gPMFunctions.ToSafeInteger(v_lDocumentTypeId), r_sMergedFilePath:=r_sMergedFilePath, r_sSpooledFilePath:=r_sSpooledFilePath, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BOM GeneratePolicyDocument Method Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyDocument", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                v_lInsuranceFolderCnt = lInsuranceFolderCnt
                v_lInsuranceFileCnt = lInsuranceFileCnt
            End If

            ' Destroy the BOM class

            oBom.Dispose()
            oBom = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeneratePolicyDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyDocument", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function UpdateLeadInsurer(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLeadInsurerCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oBom As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the BOM
            lReturn = CType(CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                ' Call the method on the BOM
                lReturn = oBom.UpdateLeadInsurer(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_lLeadInsurerCnt:=gPMFunctions.ToSafeInteger(v_lLeadInsurerCnt))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Remove BOM
                oBom.Dispose()
                oBom = Nothing
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLeadInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLeadInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    Public Function NewRiskDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lRiskID As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return NewRiskDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lRiskID:=v_lRiskID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQuoteRef:="-1", r_sQuoteRefPassword:="-1")
    End Function

    Public Function NewRiskDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lInsuranceFileCnt As Integer, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String) As Integer
        Return NewRiskDataset(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lRiskID:=-1, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword)
    End Function

    Public Function NewRiskDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lRiskID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String) As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lRiskID:=v_lRiskID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword), gPMConstants.PMEReturnCode)

            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewRiskDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewRiskDataset", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadRiskFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String, Optional ByVal r_sQuoteRef As String = "", Optional ByRef r_sQuoteRefPassword As String = "", Optional ByRef r_dtGuaranteedQuoteDate As Date = #12/30/1899#, Optional ByVal v_lRiskID As Integer = -1, Optional ByVal v_lInsuranceFileCnt As Integer = -1) As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=r_sGISDataModelCode, r_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lRiskID:=v_lRiskID), gPMConstants.PMEReturnCode)

            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRiskFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadClaimFromDBStateful(ByRef r_sGISDataModelCode As String) As Integer
        Return LoadClaimFromDBStateful(r_sGISDataModelCode:=r_sGISDataModelCode, v_lClaimID:=-1)
    End Function

    Public Function LoadClaimFromDBStateful(ByRef r_sGISDataModelCode As String, ByVal v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:="", r_sXMLDataset:="", r_sGISDataModelCode:=r_sGISDataModelCode, r_lClaimId:=v_lClaimID), gPMConstants.PMEReturnCode)

            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadClaimFromDBStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadClaimFromDBStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadClaimFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String) As Integer
        Return LoadClaimFromDB(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=r_sGISDataModelCode, v_lClaimID:=-1)
    End Function

    Public Function LoadClaimFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String, ByVal v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=r_sGISDataModelCode, r_lClaimId:=v_lClaimID), gPMConstants.PMEReturnCode)

            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadClaimFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadClaimFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadPartyFromDBStateful(ByRef r_sGISDataModelCode As String, ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:="", r_sXMLDataset:="", r_sGISDataModelCode:=r_sGISDataModelCode, r_lPartyCnt:=v_lPartyCnt), gPMConstants.PMEReturnCode)

            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadPartyFromDBStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPartyFromDBStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadPartyFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String, ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=r_sGISDataModelCode, r_lPartyCnt:=v_lPartyCnt), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadPartyFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPartyFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadPolicyFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=r_sGISDataModelCode, r_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadPolicyFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadPolicyFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function MTADiffAndMerge(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lOMTARiskID As Integer, ByVal v_lNMTARiskID As Integer, ByVal v_lIMTARiskID As Integer) As Integer
        Dim result As Integer = 0
        Dim oOMTA, oNMTA, oIMTA As cGISDataSetControl.Application
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet, sXMLDataSetDef, sGisDataModelCode, sTopLevelObj, sTopLevelTable As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Dataset References
            oOMTA = New cGISDataSetControl.Application()
            oNMTA = New cGISDataSetControl.Application()
            oIMTA = New cGISDataSetControl.Application()

            ' Load Original Version of the Risk
            lReturn = CType(LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=sGisDataModelCode, v_lRiskID:=v_lOMTARiskID, v_lInsuranceFileCnt:=v_lInsuranceFolderCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromDB Original MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            ' Load the Original Risk into a Dataset
            lReturn = oOMTA.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromXML Original MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            ' Load New Version of the Risk
            lReturn = CType(LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=sGisDataModelCode, v_lRiskID:=v_lNMTARiskID, v_lInsuranceFileCnt:=v_lInsuranceFolderCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromDB New MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            ' Load the New Risk into a Dataset
            lReturn = oNMTA.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromXML New MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            ' Load Invalid Version of the Risk
            lReturn = CType(LoadRiskFromDB(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=sGisDataModelCode, v_lRiskID:=v_lIMTARiskID, v_lInsuranceFileCnt:=v_lInsuranceFolderCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromDB Invalid MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            ' Load the Invalid Risk into a Dataset
            lReturn = oIMTA.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to LoadFromXML Invalid MTA", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            ' Get Top Level Object Name
            lReturn = oOMTA.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObj, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Compare and Merge Each Object Type
            lReturn = CType(MTADiffAndMergeObjects(v_sObjectName:=sTopLevelObj, v_sTopLevelObj:=sTopLevelObj, oOMTA:=oOMTA, oNMTA:=oNMTA, oIMTA:=oIMTA), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the New XML
            lReturn = oNMTA.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load it into the real instance of data set control
            lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Save the NEW to the Database
            lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Save Merged MTA back to the Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge")
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MTADiffAndMerge failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MTADiffAndMerge", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        Finally
            oOMTA.Dispose()
            oOMTA = Nothing

            oNMTA.Dispose()
            oNMTA = Nothing
            oIMTA.Dispose()
            oIMTA = Nothing
        End Try
    End Function

    Private Function MTADiffAndMergeObjects(ByVal v_sObjectName As String, ByVal v_sTopLevelObj As String, ByRef oOMTA As cGISDataSetControl.Application, ByRef oNMTA As cGISDataSetControl.Application, ByRef oIMTA As cGISDataSetControl.Application) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray, vPropertyArray(,) As Object
        Dim sPropertyName As String = ""
        Dim vOIKeyArray As Object
        Dim sOIKey, sParentOIKey As String
        Dim vParentOIKey As Object
        Dim sObjectName As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim lIsNonGIS As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the Object Definition Details
        lReturn = oOMTA.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=lIsQuoteObject, r_lGISObjectID:=lGISObjectID, r_sTableName:=sTableName, r_lMaxInstances:=lMaxInstances, r_lPolarisObjectID:=lPolarisObjectID, r_sParentObjectName:=sParentObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, r_sInsertSQL:=m_sAddSQL, r_sUpdateSQL:=m_sUpdateSQL, r_sDeleteSQL:=m_sDeleteSQL, r_lIsNonGIS:=lIsNonGIS)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Do no need to merge specials like Claim/ClaimPeril or AssociatedClient/Disclosure
        If (lIsNonGIS <> GISDataModelType.GISOTRisk) And (lIsNonGIS <> GISDataModelType.GISOTNonGisSpecials) Then
            Return result
        End If

        ' Do not need to do this for the Top Level Object (i.e. DMC_POLICY_BINDER)
        ' If no Parent then this is the Top Level Object
        If sParentObjectName.Trim() = "" Then

        Else
            ' Get the Keys for All Instances of this Type from the New MTA
            lReturn = oNMTA.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=vOIKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' When we are comparing Objects, we want to ignore PK values,
            ' so strip the PK properties from the PropertyArray
            For lRow As Integer = vPropertyArray.GetLowerBound(1) To vPropertyArray.GetUpperBound(1)
                sPropertyName = CStr(vPropertyArray(0, lRow))
                lReturn = m_oDataSet.GetPropertyDefDetails(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, r_iIsPrimaryKey:=iIsPrimaryKey)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Just blank out the Property Name in the Array if it is a PK
                If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then

                    vPropertyArray(0, lRow) = ""
                End If
            Next lRow

            ' If there Are Instances of this Object Type
            If Informations.IsArray(vOIKeyArray) Then
                For lRow As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)

                    sOIKey = CStr(vOIKeyArray(lRow))

                    ' Does this Object Instance Exist in the Invalid MTA
                    lReturn = oIMTA.ObjectInstExists(v_sObjectName:=v_sObjectName, v_sOIKey:=sOIKey)

                    Select Case lReturn
                        Case gPMConstants.PMEReturnCode.PMTrue
                            lReturn = CType(MTADiffAndMergeCompare(v_sObjectName:=v_sObjectName, v_vPropertyArray:=vPropertyArray, v_sOIKey:=sOIKey, oOMTA:=oOMTA, oNMTA:=oNMTA, oIMTA:=oIMTA), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return lReturn
                            End If
                        Case gPMConstants.PMEReturnCode.PMFalse
                            lReturn = oOMTA.ObjectInstExists(v_sObjectName:=v_sObjectName, v_sOIKey:=sOIKey)
                            Select Case lReturn
                                Case gPMConstants.PMEReturnCode.PMTrue
                                    lReturn = oNMTA.DelObjectInstance(v_sObjectName:=v_sObjectName, v_sOIKey:=sOIKey)
                                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                        Return lReturn
                                    End If
                                Case gPMConstants.PMEReturnCode.PMFalse

                                Case Else
                                    Return lReturn
                            End Select
                        Case Else
                            Return lReturn
                    End Select
                Next lRow
            End If

            ' Get the Keys for All Instances of this Type from the Invalid MTA
            lReturn = oIMTA.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=vOIKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
            ' If there Are Instances of this Object Type
            If Informations.IsArray(vOIKeyArray) Then
                ' For Each Object Instance
                For lRow As Integer = vOIKeyArray.GetLowerBound(0) To vOIKeyArray.GetUpperBound(0)
                    sOIKey = CStr(vOIKeyArray(lRow))

                    ' Does this Object Instance Exist in the Original MTA
                    lReturn = oOMTA.ObjectInstExists(v_sObjectName:=v_sObjectName, v_sOIKey:=sOIKey)

                    Select Case lReturn
                        Case gPMConstants.PMEReturnCode.PMTrue
                        Case gPMConstants.PMEReturnCode.PMFalse
                            If sParentObjectName.Trim().ToUpper() = v_sTopLevelObj.Trim().ToUpper() Then
                                lReturn = oNMTA.GetAllOIKey(v_sObjectName:=v_sTopLevelObj, r_vOIKeyArray:=vParentOIKey)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return lReturn
                                End If
                                sParentOIKey = CStr(vParentOIKey(vParentOIKey.GetLowerBound(0)))
                            Else
                                lReturn = oIMTA.GetObjectInstDetails(v_sObjectName:=v_sObjectName, v_sOIKey:=sOIKey, r_sParentOIKey:=sParentOIKey)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return lReturn
                                End If
                            End If

                            lReturn = CType(MTADiffAndMergeAdd(v_sObjectName:=v_sObjectName, r_vChildObjectArray:=vChildObjectArray, r_vPropertyArray:=vPropertyArray, v_sOIKey:=sOIKey, oNMTA:=oNMTA, oIMTA:=oIMTA, v_sParentOIKey:=sParentOIKey), gPMConstants.PMEReturnCode)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return lReturn
                            End If

                        Case Else
                            Return lReturn
                    End Select
                Next lRow
            End If
        End If

        ' If there Are NO Child Objects for this Object Type then EXIT
        If Not Informations.IsArray(vChildObjectArray) Then
            Return result
        End If

        ' For Each Child Object

        For lRow As Integer = vChildObjectArray.GetLowerBound(0) To vChildObjectArray.GetUpperBound(0)
            sObjectName = CStr(vChildObjectArray(lRow))
            lReturn = CType(MTADiffAndMergeObjects(v_sObjectName:=sObjectName, v_sTopLevelObj:=v_sTopLevelObj, oOMTA:=oOMTA, oNMTA:=oNMTA, oIMTA:=oIMTA), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        Next lRow

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: MTADiffAndMergeCompare
    '
    ' Description: Compare the the property values and update as necessary.
    '
    ' ***************************************************************** '
    Private Function MTADiffAndMergeCompare(ByVal v_sObjectName As String, ByVal v_vPropertyArray(,) As Object, ByVal v_sOIKey As String, ByRef oOMTA As cGISDataSetControl.Application, ByRef oNMTA As cGISDataSetControl.Application, ByRef oIMTA As cGISDataSetControl.Application) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray, vPropertyArray As Object
        Dim sPropertyName As String = ""
        Dim vOIKeyArray As Object
        Dim sOIKey, sObjectName As String
        Dim vOValue, vIValue, vNValue As Object
        Dim vValue As Object
        Dim lDataType As Long
        Dim bAssumedInfo As Boolean

        result = gPMConstants.PMEReturnCode.PMTrue

        ' For Each Property
        For lRow As Integer = v_vPropertyArray.GetLowerBound(1) To v_vPropertyArray.GetUpperBound(1)

            sPropertyName = CStr(v_vPropertyArray(0, lRow)).Trim()

            ' If there is no Property Name then this is a PK, do not comare
            If sPropertyName <> "" Then
                lReturn = oNMTA.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vNValue, r_bIsAssumedInfo:=bAssumedInfo)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                lReturn = oIMTA.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vIValue, r_bIsAssumedInfo:=bAssumedInfo)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                If vNValue.Equals(vIValue) Then

                Else
                    lReturn = oOMTA.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vOValue, r_bIsAssumedInfo:=bAssumedInfo)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Else
                        If Not vIValue.Equals(vOValue) Then

                            ' Get the property Data type from the Invalid MTA
                            lDataType = 0
                            lReturn = oIMTA.GetPropertyDefDetails(
                                v_sObjectName:=v_sObjectName,
                                v_sPropertyName:=sPropertyName,
                                r_lDataType:=lDataType)
                            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                'Do Nothing
                            End If

                            ' Set the New Value to be the Future/Old Value
                            If lDataType = 20 And ((vOValue = 0 And vIValue = 1) Or (vOValue = 1 And vIValue = 0)) Then
                                vValue = 1
                            ElseIf vOValue = Nothing And vIValue <> Nothing Then
                                vValue = vIValue
                                'ElseIf vOValue = 0 And vIValue <> 0 Then
                                'vValue = vIValue
                            Else
                                vValue = vOValue
                            End If





                            lReturn = oNMTA.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, v_vPropertyValue:=vIValue, v_bIsAssumedInfo:=False)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return lReturn
                            End If
                        End If
                    End If
                End If
            End If
        Next lRow
        Return result
    End Function

    Private Function MTADiffAndMergeAdd(ByVal v_sObjectName As String, ByRef r_vChildObjectArray() As Object, ByRef r_vPropertyArray(,) As Object, ByVal v_sOIKey As String, ByRef oNMTA As cGISDataSetControl.Application, ByRef oIMTA As cGISDataSetControl.Application, Optional ByRef v_sParentOIKey As String = "") As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lIsQuoteObject, lGISObjectID As Integer
        Dim sTableName As String = ""
        Dim lMaxInstances, lPolarisObjectID As Integer
        Dim sParentObjectName As String = ""
        Dim vChildObjectArray, vPropertyArray As Object
        Dim sPropertyName As String = ""
        Dim vOIKeyArray As Object
        Dim sOIKey, sObjectName As String
        Dim vOValue, vIValue, vNValue As Object
        Dim bAssumedInfo As Boolean
        Dim sNewOIKey As String = ""
        Dim sChildObject As String = ""
        Dim vChildOI As Object
        Dim sChildOI As String = ""
        Dim vChildProp(,) As Object
        Dim sChildProp As String = ""
        Dim iIsPrimaryKey As gPMConstants.PMEReturnCode
        Dim vChildChild As Object


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add a New Object Instance
        lReturn = oNMTA.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=sNewOIKey, v_sParentOIKey:=v_sParentOIKey)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' For Each Property
        For lRow As Integer = r_vPropertyArray.GetLowerBound(1) To r_vPropertyArray.GetUpperBound(1)

            sPropertyName = CStr(r_vPropertyArray(0, lRow)).Trim()

            ' If there is no Property Name then this is a PK, do not comare
            If sPropertyName <> "" Then
                lReturn = oIMTA.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=vIValue, r_bIsAssumedInfo:=bAssumedInfo)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                lReturn = oNMTA.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=sPropertyName, v_sOIKey:=sNewOIKey, v_vPropertyValue:=vIValue, v_bIsAssumedInfo:=False)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If
        Next lRow

        If Informations.IsArray(r_vChildObjectArray) Then
            ' For Each Child Object Type
            For lRow As Integer = r_vChildObjectArray.GetLowerBound(0) To r_vChildObjectArray.GetUpperBound(0)

                ' Get the Child Object Name

                sChildObject = CStr(r_vChildObjectArray(lRow))

                ' Get All Child Instance Keys for this Object Type
                lReturn = oIMTA.GetChildOIKey(v_sParentObjectName:=v_sObjectName, v_sParentOIKey:=v_sOIKey, v_sChildObjectName:=sChildObject, r_vChildOIKeyArray:=vChildOI)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                ' Are there Any?
                If Informations.IsArray(vChildOI) Then

                    ' Get the Properties for this Object
                    lReturn = oIMTA.GetObjectDefDetails(v_sObjectName:=sChildObject, r_vPropertyArray:=vChildProp, r_vChildObjectArray:=vChildChild)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return lReturn
                    End If

                    ' Remove PK's from the Property Array (remember we do not copy PK Values)
                    For lRow2 As Integer = vChildProp.GetLowerBound(1) To vChildProp.GetUpperBound(1)
                        sChildProp = CStr(vChildProp(0, lRow2))

                        ' Get the Property Definition
                        lReturn = oIMTA.GetPropertyDefDetails(v_sObjectName:=sChildObject, v_sPropertyName:=sChildProp, r_iIsPrimaryKey:=iIsPrimaryKey)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lReturn
                        End If

                        ' Is it a Primary Key
                        If iIsPrimaryKey = gPMConstants.PMEReturnCode.PMTrue Then
                            ' If the Property is a PK, blank out the name (we do not copy PK values)
                            vChildProp(0, lRow2) = ""
                        End If
                    Next lRow2

                    ' For Each Object Instance of that type
                    For lRow2 As Integer = vChildOI.GetLowerBound(0) To vChildOI.GetUpperBound(0)
                        sChildOI = CStr(vChildOI(lRow2))
                        lReturn = CType(MTADiffAndMergeAdd(v_sObjectName:=sChildObject, r_vChildObjectArray:=vChildChild, r_vPropertyArray:=vChildProp, v_sOIKey:=sChildOI, oNMTA:=oNMTA, oIMTA:=oIMTA, v_sParentOIKey:=sNewOIKey), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lReturn
                        End If
                    Next lRow2
                End If
            Next lRow
        End If

        ' Delete the Object Instance from the Source so that we do not try and re-add it later.
        lReturn = oIMTA.DelObjectInstance(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        Return result

    End Function

    Public Function GetDocTemplate(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lSchemeID As Integer, ByVal v_lAgentCnt As Integer, ByRef v_vDocumentArray As Object) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oBom As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Use the CreateBOM function to create the BOM (if required)
            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not (oBom Is Nothing) Then
                lReturn = oBom.GetDocTemplate(v_lSchemeID:=gPMFunctions.ToSafeString(v_lSchemeID), v_lAgentCnt:=gPMFunctions.ToSafeInteger(v_lAgentCnt), v_sProcessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_vDocumentArray:=v_vDocumentArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                oBom.Dispose()
                oBom = Nothing
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function TransactBrokingSts(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lPolicyLinkID As Integer, ByRef r_oBOM As Object, Optional ByRef r_vAdditionalDataArray As Object = Nothing, Optional ByVal v_sTransactType As Object = bGISTemp.GISNBTransTypeComplete) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim vSchemeArray As Object
        Dim oDataSet As Object = m_oDataSet

        m_lReturn = CType(bGISTemp.UpdatePolicyLinkTransact(v_lPolicyLinkID:=v_lPolicyLinkID, v_dTransactDate:=DateTime.Now, v_sTransactType:=v_sTransactType, r_oDatabase:=m_oDatabase, v_lGISSchemeID:=v_lGISSchemeID), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "UpdatePolicyLinkTransact (GISNBTransTypeComplete) Failed", ACApp, ACClass, "TransactBrokingSts")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (r_oBOM Is Nothing) Then
            m_lReturn = r_oBOM.NBTransactAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_vSchemeArray:=vSchemeArray, r_vAdditionalDataArray:=r_vAdditionalDataArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="r_oBOM.NBTransactAfter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactBrokingSts")
                Return result
            End If
            m_oDataSet = oDataSet
        End If
        Return result
    End Function

    Public Function ValidatePostQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lProcessType As Integer, ByRef r_sXMLDataset As String, ByVal v_lGISSchemeID As Integer, ByVal v_vValidationArray As Object, ByRef r_vResultArray As Object) As Integer
        Return ValidatePostQuote(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lProcessType:=v_lProcessType, r_sXMLDataset:=r_sXMLDataset, v_lGISSchemeID:=v_lGISSchemeID, v_vValidationArray:=v_vValidationArray, r_vResultArray:=r_vResultArray, v_sTransactionType:="")
    End Function

    Public Function ValidatePostQuote(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lProcessType As Integer, ByRef r_sXMLDataset As Object, ByVal v_lGISSchemeID As Integer, ByVal v_vValidationArray As Object, ByRef r_vResultArray As Object, ByVal v_sTransactionType As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lGISPolicyLinkID As Integer
        Dim vSchemeArray As Object
        Dim sQEMName As String = ""
        Dim oQEM As Object
        Dim sXMLDataSetDef As Object = ""

        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of bGISScheme
            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()

            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Load the XML
            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lGISSchemeID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sQEMName = CStr(vSchemeArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim().ToUpper()
            sQEMName = sQEMName & "." & CStr(vSchemeArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim().ToUpper()

            lReturn = CType(CreateQEM(v_sQEMName:=sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Data Set Definition
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Call the QEM to validate the post quote data

            lReturn = oQEM.ValidatePostQuote(v_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_vSchemeArray:=vSchemeArray, v_lProcessType:=gPMFunctions.ToSafeInteger(v_lProcessType), v_lGISSchemeID:=gPMFunctions.ToSafeInteger(v_lGISSchemeID), v_vValidationArray:=v_vValidationArray, r_vResultArray:=r_vResultArray, v_sTransactionType:=gPMFunctions.ToSafeString(v_sTransactionType))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            oQEM = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidatePostQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidatePostQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' RAW 22/07/2003 : CQ1672 : added
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
                If vTransactionType = "REN" Then
                    nTransactionType = 10
                Else
                    nTransactionType = 0
                End If

            End If

            If Not Informations.IsNothing(vEffectiveDate) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function ClearPBQuoteOutputs(ByVal v_sGisDataModelCode As String, ByRef r_sXMLDataset As String) As Integer
        Dim result As Integer = 0
        Dim iOutputObjectCount As Integer
        Dim GIS_OutputObjectName, sDataSetDef As String

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            GIS_OutputObjectName = "OUTPUT"

            With m_oDataSet.Risk
                iOutputObjectCount = .Count(v_sGisDataModelCode & "_" & GIS_OutputObjectName)
                If iOutputObjectCount > 0 Then
                    GIS_OutputObjectName = v_sGisDataModelCode & "_" & GIS_OutputObjectName
                Else
                    iOutputObjectCount = .Count(GIS_OutputObjectName)
                End If

                If iOutputObjectCount > 0 Then
                    ' We have some output object
                    ' So use the DeleteObject Method to clear the output object
                    For iCounter As Integer = 1 To iOutputObjectCount Step 1
                        .Item(GIS_OutputObjectName, 1).DeleteObject()
                    Next iCounter
                End If

                ' Return the XML (since we won't have it at the client with objects removed)
                m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sDataSetDef, r_sXMLDataSet:=r_sXMLDataset)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Return as XML", vApp:=ACApp, vClass:=ACClass, vMethod:="ClearPBQuoteOutputs")
                    Return m_lReturn
                End If
            End With

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClearPBQuoteOutputs Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetPremiumData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer, ByRef r_cPremiumDueGross As Decimal, ByRef r_cPremiumDueNet As Decimal, ByRef r_cPremiumDueTax As Decimal, ByRef r_cTotalAnnualTax As Decimal) As Integer
        Return GetPremiumData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=v_lRiskID, r_cPremiumDueGross:=r_cPremiumDueGross, r_cPremiumDueNet:=r_cPremiumDueNet, r_cPremiumDueTax:=r_cPremiumDueTax, r_cTotalAnnualTax:=r_cTotalAnnualTax, r_bCancelledPolicy:=False)
    End Function

    Public Function GetPremiumData(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskID As Integer, ByRef r_cPremiumDueGross As Decimal, ByRef r_cPremiumDueNet As Decimal, ByRef r_cPremiumDueTax As Decimal, ByRef r_cTotalAnnualTax As Decimal, ByRef r_bCancelledPolicy As Boolean) As Integer

        Dim result As Integer = 0
        Dim oBusiness As bSirPerilAllocation.Business
        Dim vRateTypes, vRatingSection(,) As Object

        Dim cReturnPremium, cNewPremiumNet, cNewPremiumTax, cNewPremiumGross, cNewAnnPremNet, cNewAnnPremTax, cNewAnnPremGross, cOldPremiumNet, cOldPremiumTax, cOldPremiumGross, cOldAnnPremNet, cOldAnnPremTax, cOldAnnPremGross As Decimal

        Const ACOriginalFlagCol As Integer = 13

        Try

            result = gPMConstants.PMEReturnCode.PMFalse
            oBusiness = New bSirPerilAllocation.Business
            m_lReturn = CType(oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPremiumData Failed to Initialise bSIRPerilAllocation.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPremiumData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            oBusiness.InsuranceFolderCnt = v_lInsuranceFolderCnt
            oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt
            oBusiness.RiskID = v_lRiskID
            oBusiness.TransactionType = m_sTransactionType
            'Get the Rating types from the database to be used
            m_lReturn = oBusiness.GetRateTypes(vResultArray:=vRateTypes)
            'Get the values of Rating section from the database
            m_lReturn = oBusiness.GetRatingSections(vResultArray:=vRatingSection)
            Dim vOriginalRatingSections(,) As Object

            Dim iLBnd, iUBnd, iColsUBnd, iColsLBnd As Integer
            If m_sTransactionType = "MTA" Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                If Informations.IsArray(vRatingSection) Then
                    iLBnd = vRatingSection.GetLowerBound(1)
                    iUBnd = vRatingSection.GetUpperBound(1)
                    iColsUBnd = vRatingSection.GetUpperBound(0)
                    iColsLBnd = vRatingSection.GetLowerBound(0)
                    For iCnt As Integer = iLBnd To iUBnd
                        If CStr(vRatingSection(ACOriginalFlagCol, iCnt)) = "1" Then
                            If Not Informations.IsArray(vOriginalRatingSections) Then
                                ReDim vOriginalRatingSections(vRatingSection.GetUpperBound(0), 0)
                            Else
                                ReDim Preserve vOriginalRatingSections(vOriginalRatingSections.GetUpperBound(0), vOriginalRatingSections.GetUpperBound(1) + 1)
                            End If

                            For iCols As Integer = iColsLBnd To iColsUBnd
                                vOriginalRatingSections(iCols, vOriginalRatingSections.GetUpperBound(1)) = vRatingSection(iCols, iCnt)
                            Next iCols
                        End If
                    Next iCnt
                End If
            End If

            Dim PremDueGr, PremDueNet, PremDueTax, TotalAnnTax As Decimal
            m_lReturn = oBusiness.RecalculatePremium(r_vRatingSection:=vRatingSection, v_vRateTypes:=vRateTypes, r_cReturnPremium:=cReturnPremium, r_cPremiumDueNet:=PremDueNet, r_cPremiumDueTax:=PremDueTax, r_cPremiumDueGross:=PremDueGr, r_cNewPremiumNet:=cNewPremiumNet, r_cNewPremiumTax:=cNewPremiumTax, r_cNewPremiumGross:=cNewPremiumGross, r_cNewAnnPremNet:=cNewAnnPremNet, r_cNewAnnPremTax:=cNewAnnPremTax, r_cNewAnnPremGross:=cNewAnnPremGross, r_cOldPremiumNet:=cOldPremiumNet, r_cOldPremiumTax:=cOldPremiumTax, r_cOldPremiumGross:=cOldPremiumGross, r_cOldAnnPremNet:=cOldAnnPremNet, r_cOldAnnPremTax:=cOldAnnPremTax, r_cOldAnnPremGross:=cOldAnnPremGross, r_cTotalAnnualTax:=TotalAnnTax, v_vOriginal:=vOriginalRatingSections, r_bCancelledPolicy:=r_bCancelledPolicy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSirPerilAllocation.Business.RecalculatePremium method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPremiumData", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            r_cPremiumDueGross = PremDueGr
            r_cPremiumDueNet = PremDueNet
            r_cPremiumDueTax = PremDueTax
            r_cTotalAnnualTax = TotalAnnTax

            Return gPMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPremiumData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPremiumData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            oBusiness.Dispose()
            oBusiness = Nothing
        End Try

    End Function

    Public Function NewObjectInstance(ByVal v_sObjectName As String, ByRef r_sOIKey As String) As Integer
        Return NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=r_sOIKey, v_sParentOIKey:="")
    End Function

    Public Function NewObjectInstance(ByVal v_sObjectName As String, ByRef r_sOIKey As String, ByVal v_sParentOIKey As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Return m_oDataSet.NewObjectInstance(v_sObjectName:=v_sObjectName, r_sOIKey:=r_sOIKey, v_sParentOIKey:=v_sParentOIKey)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewObjectInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function DelObjectInstance(ByVal v_sObjectName As String, ByVal v_sOIKey As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_oDataSet.DelObjectInstance(v_sObjectName:=v_sObjectName, v_sOIKey:=v_sOIKey)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstanceFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function SetPropertyValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sOIKey As String, ByVal v_vPropertyValue As Object) As Integer
        Return SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, v_vPropertyValue:=v_vPropertyValue, v_bIsAssumedInfo:=False)
    End Function

    Public Function SetPropertyValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sOIKey As String, ByVal v_vPropertyValue As Object, ByVal v_bIsAssumedInfo As Boolean) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer

        Try
            Return m_oDataSet.SetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, v_vPropertyValue:=v_vPropertyValue, v_bIsAssumedInfo:=v_bIsAssumedInfo)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertyValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetPropertyValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sOIKey As String, ByRef r_vPropertyValue As Object) As Integer
        Return GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=r_vPropertyValue, r_bIsAssumedInfo:=False)
    End Function

    Public Function GetPropertyValue(ByVal v_sObjectName As String, ByVal v_sPropertyName As String, ByVal v_sOIKey As String, ByRef r_vPropertyValue As Object, ByRef r_bIsAssumedInfo As Boolean) As Integer
        Dim result As Integer = 0
        Dim lReturn As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_oDataSet.GetPropertyValue(v_sObjectName:=v_sObjectName, v_sPropertyName:=v_sPropertyName, v_sOIKey:=v_sOIKey, r_vPropertyValue:=r_vPropertyValue, r_bIsAssumedInfo:=r_bIsAssumedInfo)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPropertyValueFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPropertyValue", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetObjectDefDetails(ByVal v_sObjectName As String, Optional ByRef r_lIsQuoteObject As Integer = 0, Optional ByRef r_lGISObjectID As Integer = 0, Optional ByRef r_sTableName As String = "", Optional ByRef r_lMaxInstances As Integer = 0, Optional ByRef r_lPolarisObjectID As Integer = 0, Optional ByRef r_sParentObjectName As String = "", Optional ByRef r_vChildObjectArray As Object = Nothing, Optional ByRef r_vPropertyArray As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try
            Return m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_lIsQuoteObject:=r_lIsQuoteObject, r_lGISObjectID:=r_lGISObjectID, r_sTableName:=r_sTableName, r_lMaxInstances:=r_lMaxInstances, r_lPolarisObjectID:=r_lPolarisObjectID, r_sParentObjectName:=r_sParentObjectName, r_vChildObjectArray:=r_vChildObjectArray, r_vPropertyArray:=r_vPropertyArray)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObjectDefDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObjectDefDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetAllOIKey(ByVal v_sObjectName As String, ByRef r_vOIKeyArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Return m_oDataSet.GetAllOIKey(v_sObjectName:=v_sObjectName, r_vOIKeyArray:=r_vOIKeyArray)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Function GetChildOIKey(ByVal v_sParentObjectName As String, ByVal v_sParentOIKey As String, ByVal v_sChildObjectName As String, ByRef r_vChildOIKeyArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            Return m_oDataSet.GetChildOIKey(v_sParentObjectName:=v_sParentObjectName, v_sParentOIKey:=v_sParentOIKey, v_sChildObjectName:=v_sChildObjectName, r_vChildOIKeyArray:=r_vChildOIKeyArray)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildOIKeyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildOIKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function GetAllStoredProcedures(ByVal v_sPath As String, ByVal v_sDataModelCode As String, ByRef r_vFileArray() As Object, ByRef r_vSPNameArray() As Object) As Integer
        Dim result As Integer = 0
        Dim oFSO As Object
        Dim oFolder As System.IO.DirectoryInfo
        Dim lFileCount, lCounter As Integer

        Dim vFileArray() As Object
        Dim vSPNameArray() As Object
        Dim sFilename, sFileExtension, sSPName, strToCompare As String



        oFSO = New Object()

        oFolder = New DirectoryInfo(v_sPath)
        lFileCount = oFolder.GetFiles.Length

        If lFileCount > 0 Then
            strToCompare = "spg_" & v_sDataModelCode & "_"
            For Each oFile As FileInfo In oFolder.GetFiles   'Files

                sFilename = oFile.Name
                sFileExtension = sFilename.ToLower().Substring(sFilename.ToLower().Length - 3) ' Already converted to Lower Case here
                If (sFilename.IndexOf(strToCompare, StringComparison.CurrentCultureIgnoreCase) + 1) = 1 Then
                    If sFileExtension = "sql" Then ' Since it is already converted to Lower Case
                        If lCounter < 1 Then
                            ReDim vFileArray(lCounter)
                            ReDim vSPNameArray(lCounter)
                        Else
                            ReDim Preserve vFileArray(lCounter)
                            ReDim Preserve vSPNameArray(lCounter)
                        End If
                        vFileArray(lCounter) = sFilename
                        vSPNameArray(lCounter) = sFilename.Substring(0, sFilename.Length - 4)
                        lCounter += 1
                    End If
                End If
            Next oFile
        End If
        r_vFileArray = vFileArray
        r_vSPNameArray = vSPNameArray
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Private Function NewDataSetPrivate(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, Optional ByVal v_lInsuranceFileCnt As Integer = -1, Optional ByRef r_sQuoteRef As String = "", Optional ByRef r_sQuoteRefPassword As String = "", Optional ByVal v_lPartyCnt As Integer = -1, Optional ByVal v_lRiskID As Integer = -1, Optional ByVal v_lClaimID As Integer = -1, Optional ByVal v_lCaseID As Integer = -1) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecordsAffected As Integer
        Dim sDataSetDefFile, sDataSetFile As String
        Dim sTopLevelObject, sTopLevelTable As String
        Dim sQuoteRef, sQuoteRefPassword As String
        Dim bNew As Boolean
        Dim sSpecialObject As String = ""
        Dim lNonGISType As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=CStr(v_sGisDataModelCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            If v_lInsuranceFileCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            If v_lClaimID > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' RAW 20/08/2003 : CQ1914/2086/2088 : added
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            If v_lPartyCnt > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            If v_lRiskID > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(v_lRiskID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Optional CaseID
            If v_lCaseID > 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="case_id", vValue:=CStr(v_lCaseID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="case_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Call the SQL
            lReturn = m_oDatabase.SQLAction(sSQL:=ACAddPolicyLinkSQL, sSQLName:=ACAddPolicyLinkName, bStoredProcedure:=ACAddPolicyLinkStored, lRecordsAffected:=lRecordsAffected)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Return the created Policy Link ID and the Data Model ID
            r_lPolicyLinkID = m_oDatabase.Parameters.Item("gis_policy_link_id").Value

            ' Generate the Quote Reference
            lReturn = CType(GenerateQuoteRef(v_lGISPolicyLinkID:=r_lPolicyLinkID, r_sQuoteRef:=sQuoteRef, v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Generate a random Password
            lReturn = CType(GeneratePassword(sQuoteRefPassword), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Update the Quote Ref and Password
            lReturn = CType(UpdateQuoteRef(v_lGISPolicyLinkID:=r_lPolicyLinkID, v_sQuoteRef:=sQuoteRef, v_sQuoteRefPassword:=sQuoteRefPassword, v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            r_sQuoteRef = sQuoteRef
            r_sQuoteRefPassword = sQuoteRefPassword
            lReturn = CType(GetDataModelDef(v_sGisDataModelCode:=v_sGisDataModelCode), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' RAW 20/08/2003 : CQ1914/2086/2088 : added
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Get the Top Level Table Name
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Create an Instance of the Top Level Object in the Data Set
            lReturn = m_oDataSet.NewObjectInstance(v_sObjectName:=sTopLevelObject, r_sOIKey:=r_sTopOIKey)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Set PK Property for the Top Level Object to the Policy Link ID
            lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=sTopLevelObject & "_ID", v_sOIKey:=r_sTopOIKey, v_vPropertyValue:=r_lPolicyLinkID, v_bIsAssumedInfo:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' Set Policy Link ID Property in the Top Level Object
            lReturn = m_oDataSet.SetPropertyValue(v_sObjectName:=sTopLevelObject, v_sPropertyName:=GISSharedConstants.GISPolLinkIDName, v_sOIKey:=r_sTopOIKey, v_vPropertyValue:=r_lPolicyLinkID, v_bIsAssumedInfo:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lReturn = m_oDataSet.GetSpecialObject(r_sObjectName:=sSpecialObject, r_lNonGISType:=lNonGISType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            ' 10 stands for case and we don't have anything special to process here
            If sSpecialObject.Trim() <> "" And lNonGISType <> 10 Then
                ' Yes, so build the XML for those
                lReturn = CType(LoadFromDBSpecials(v_lGISPolicyLinkID:=r_lPolicyLinkID, v_sObjectName:=sSpecialObject, v_lNonGISType:=lNonGISType, v_lClaimID:=v_lClaimID), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    lReturn = m_oDatabase.SQLRollbackTrans()
                    Return result
                End If

            End If

            ' ******************** SPECIALS ******************
            lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            lReturn = m_oDatabase.SQLCommitTrans()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            lReturn = m_oDatabase.SQLRollbackTrans()
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewDataSetPrivate failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewDataSetPrivate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function SaveToDBStateful() As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bNew As Boolean

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveToDBStatefulFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveToDBStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function CopyDataSetStateful(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkID As Integer, ByVal v_vOldGISPolicyLinkId As Integer, ByVal v_vOldInsuranceFileCnt As Integer, ByVal v_vNewInsuranceFileCnt As Integer, ByVal v_vOldRiskID As Integer, ByVal v_vNewRiskID As Integer) As Integer
        Return CopyDataSetStateful(v_sDataModelCode:=v_sDataModelCode, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vOldXMLDataSet:="", v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID, v_vNewRiskID:=v_vNewRiskID, v_vCopyQuotes:=False)
    End Function

    Public Function CopyDataSetStateful(ByVal v_sDataModelCode As String, ByRef r_lNewGISPolicyLinkID As Integer, ByVal v_vOldGISPolicyLinkId As Integer, ByVal v_vOldInsuranceFileCnt As Integer, ByVal v_vOldXMLDataSet As String, ByVal v_vNewInsuranceFileCnt As Integer, ByVal v_vOldRiskID As Integer, ByVal v_vNewRiskID As Integer, ByVal v_vCopyQuotes As Boolean) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim sTopOIKey As String = String.Empty
        Dim vOIKeyArray As Object
        Dim sTopLevelObject, sTopLevelTable As String
        Dim lPos As Integer
        Dim sComp As String = ""
        Dim lSaveToDBMode As Integer
        Dim bIgnoreFirstInst As Boolean
        Dim lStartPosition, lEndPosition As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If (v_vOldXMLDataSet.Trim() = "") And (v_vOldGISPolicyLinkId < 1) And (v_vOldInsuranceFileCnt < 1) Then
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Either an XML DataSet, PolicyLinkID, InsuranceFileCnt MUST be supplied.", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDataSetStateful")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lSaveToDBMode = GISSharedConstants.GetLoadSaveDBMode(v_sDataModelCode)
            If lSaveToDBMode <> GISSharedConstants.GISRegLoadSaveDBModeSlow And (v_vOldXMLDataSet.Trim() = "") Then
                result = CopyDataSetViaSPStateful(v_sDataModelCode:=v_sDataModelCode, v_lSaveToDBMode:=lSaveToDBMode, v_bCopyQuotes:=v_vCopyQuotes, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_vOldGISPolicyLinkId:=v_vOldGISPolicyLinkId, v_vOldInsuranceFileCnt:=v_vOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_vOldRiskID:=v_vOldRiskID, v_vNewRiskID:=v_vNewRiskID)

                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    result = LoadFromDBPrivate(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=v_sDataModelCode, r_lPolicyLinkID:=r_lNewGISPolicyLinkID)
                End If

                Return result
            End If

            ' Create a New Policy Link Record
            lReturn = CType(NewRiskDataset(v_sGisDataModelCode:=v_sDataModelCode, r_lPolicyLinkID:=r_lNewGISPolicyLinkID, r_sTopOIKey:=sTopOIKey, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_lInsuranceFileCnt:=v_vNewInsuranceFileCnt, v_lRiskID:=v_vNewRiskID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If v_vOldXMLDataSet.Trim() <> "" Then
                lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sDataModelCode, v_sXMLDataSet:=v_vOldXMLDataSet), gPMConstants.PMEReturnCode)
            Else
                lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=v_sDataModelCode, r_lPolicyLinkID:=v_vOldGISPolicyLinkId, r_lInsuranceFileCnt:=v_vOldInsuranceFileCnt, r_lRiskID:=v_vOldRiskID), gPMConstants.PMEReturnCode)

            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Top Level Object Name
            lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Reset the Policy Link ID of all the Objects
            lReturn = CType(ResetPolicyLinkID(v_sObjectName:=sTopLevelObject, v_lNewPolicyLinkID:=r_lNewGISPolicyLinkID, v_sTopLevelObject:=sTopLevelObject), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Return the Data Set in XML Format
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lPos = sXMLDataSet.IndexOf("<" & sTopLevelObject)
            lPos = Informations.inStr(lPos, sXMLDataSet, " US=""0""")
            lPos += 1

            ' update all other US flags to 1
            lPos = Informations.inStr(lPos, sXMLDataSet, " US=""0""")

            Do While lPos <> 0
                sXMLDataSet = sXMLDataSet.Remove(lPos - 1, 7).Insert(lPos - 1, " US=""1""")
                lPos = Informations.inStr(lPos, sXMLDataSet, " US=""0""")
            Loop
            'Remove any deleted objects from the dataset
            ' Find start of deleted objects
            lStartPosition = (sXMLDataSet.IndexOf("<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """>") + 1)

            ' If we found deleted objects
            If lStartPosition > 0 Then
                lEndPosition = (sXMLDataSet.IndexOf("</DELETED_OBJECTS>") + 1)
                ' If we found delimiter
                If lEndPosition > 0 Then
                    ' Copy first bit of message + empty deleted objects + end bit of original message
                    sXMLDataSet = Mid(sXMLDataSet, 1, lStartPosition - 1) &
                                  "<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """/>" &
                                  Mid(sXMLDataSet, lEndPosition + ("</DELETED_OBJECTS>").Length, sXMLDataSet.Length)
                End If
            End If

            ' reload the dataset with the updated xml
            lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyDataSetStatefulFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyDataSetStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LoadFromDBPrivate
    '
    ' Description: Loads a Risk Data Set from the Database.
    '
    ' History:
    '    RFC13012000    Return Guaranteed Quote Date
    '    RDC 11122002   Add parms required by new calls
    '    MEvans : 29-09-2003 : CQ2664 - Added proper support for claim
    ' ***************************************************************** '
    Private Function LoadFromDBPrivate(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String, Optional ByRef r_lInsuranceFileCnt As Integer = -1, Optional ByRef r_lPolicyLinkID As Integer = -1, Optional ByRef r_sQuoteRef As String = "", Optional ByRef r_sQuoteRefPassword As String = "", Optional ByRef r_dtGuaranteedQuoteDate As Date = #12/30/1899#, Optional ByRef r_lRiskID As Integer = -1, Optional ByRef r_lPartyCnt As Integer = -1, Optional ByRef r_lClaimId As Integer = -1, Optional ByRef r_lCaseID As Integer = -1) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sTopLevelObject, sTopLevelTable, sSuppliedPassword, sSuppliedEncrypted, sRealEncrypted As String
        Dim bGetViaQuoteRef As Boolean
        Dim bNew As Boolean
        Dim lLoadFromDBMode As Integer
        Dim sDummyDataset As String = ""
        Dim lPos As Integer
        Dim sTopLevelQuoteObject, sTopLevelQuoteTable As String
        Dim lObjectCount, lQuoteCount As Integer
        Dim sRiskXML, sQuoteXML As String
        Dim oOptions As Object
        Dim sSystem As String = ""
        Dim sSpecialObject As String = ""
        Dim lNonGISType As Integer
        Dim bSuppressDataSetDefReturn As Boolean


        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDataSet = New cGISDataSetControl.Application()

        ' Are we getting the Quote using the Quote Reference As the Key
        If Not True Then
            ' No
            bGetViaQuoteRef = False
        ElseIf (r_sQuoteRef.Trim() = "") Then
            ' No
            bGetViaQuoteRef = False
        Else
            ' Yes
            bGetViaQuoteRef = True
        End If
        'NIIT Comment: When r_sXMLDataSetDef=Nothing  r_sXMLDataSetDef.ToLower() Gives error.
        If Not (r_sXMLDataSetDef Is Nothing) Then
            If r_sXMLDataSetDef.ToLower() = "suppress" Then
                bSuppressDataSetDefReturn = True
            End If
        End If
        '*********
        ' MEvans : 29-09-2003 : CQ 2664
        ' added claim id param to getpolicylink call

        ' Need to get the Data Model ID from the Policy Link Record

        'lReturn = CType(GetPolicyLink(r_sGISDataModelCode:=r_sGISDataModelCode, r_sQuoteRefPassword:=sRealEncrypted, r_vInsuranceFileCnt:=gpmfunctions.ToSafeInteger(r_lInsuranceFileCnt), r_vPolicyLinkID:=r_lPolicyLinkID, r_vQuoteRef:=CInt(r_sQuoteRef), r_dtGuaranteedQuoteDate:=r_dtGuaranteedQuoteDate, r_vRiskID:=r_lRiskID, r_lPartyID:=gpmfunctions.ToSafeInteger(r_lPartyCnt), r_lClaimId:=r_lClaimId, r_lCaseID:=r_lCaseID), gPMConstants.PMEReturnCode)
        lReturn = CType(GetPolicyLink(r_sGISDataModelCode:=r_sGISDataModelCode, r_sQuoteRefPassword:=sRealEncrypted, r_vInsuranceFileCnt:=r_lInsuranceFileCnt, r_vPolicyLinkID:=r_lPolicyLinkID, r_vQuoteRef:=r_sQuoteRef, r_dtGuaranteedQuoteDate:=r_dtGuaranteedQuoteDate, r_vRiskID:=r_lRiskID, r_lPartyID:=r_lPartyCnt, r_lClaimId:=r_lClaimId, r_lCaseID:=r_lCaseID), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        '*********

        If bGetViaQuoteRef Then

            sSuppliedPassword = r_sQuoteRefPassword

            ' We Need to encrypt the Supplied Password
            lReturn = CType(bPMFunc.Encrypt(sSuppliedPassword, sSuppliedEncrypted), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Check that the Passwords Match
            If sSuppliedEncrypted.Trim() = sRealEncrypted.Trim() Then
                ' Passwords Match
            Else
                ' Passwords DO NOT MATCH
                Return gPMConstants.PMEReturnCode.PMIncorrectPassword
            End If

        End If

        ' Get the Data Model Definition
        lReturn = CType(GetDataModelDef(v_sGisDataModelCode:=r_sGISDataModelCode), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Top Level Object Name
        lReturn = m_oDataSet.GetTopLevelRiskObject(r_sObjectName:=sTopLevelObject, r_sTableName:=sTopLevelTable)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' How do we want to load from the database
        lLoadFromDBMode = GISSharedConstants.GetLoadSaveDBMode(r_sGISDataModelCode)

        ' Are we still useing the Old Slow Method
        If lLoadFromDBMode = GISSharedConstants.GISRegLoadSaveDBModeSlow Then

            ' Load the Data Set From the Database
            lReturn = CType(ObjectInstancesFromDB(v_sObjectName:=sTopLevelObject, v_sTopLevelTableName:=sTopLevelTable, v_lPolicyLinkID:=r_lPolicyLinkID, v_sTopLevelObjectName:=sTopLevelObject), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Data Set in XML
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else
            ' sj 24/08/2001 - start
            lObjectCount = 0
            ' sj 24/08/2001 - end
            ' Use the Quick Stored Proc Method
            lReturn = CType(ObjectInstancesFromDBViaSP(v_lGISPolicyLinkID:=r_lPolicyLinkID, v_sTopLevelTableName:=sTopLevelTable, r_sXMLDataset:=sRiskXML, r_lObjectCount:=lObjectCount, r_lQuoteCount:=0, v_bQuoteObject:=False), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If we can Load the Quotes from the Database Also
            If lLoadFromDBMode = GISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                lReturn = m_oDataSet.GetTopLevelQuoteObject(r_sObjectName:=sTopLevelQuoteObject, r_sTableName:=sTopLevelQuoteTable)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' sj 24/08/2001 - start
                lQuoteCount = 0
                ' sj 24/08/2001 - end
                lReturn = CType(ObjectInstancesFromDBViaSP(v_lGISPolicyLinkID:=r_lPolicyLinkID, v_sTopLevelTableName:=sTopLevelQuoteTable, r_sXMLDataset:=sQuoteXML, r_lObjectCount:=lObjectCount, r_lQuoteCount:=lQuoteCount, v_bQuoteObject:=True), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Return the Data Set in XML
            lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=sDummyDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lLoadFromDBMode = GISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes Then
                lObjectCount += 1
                r_sXMLDataset = "<DATA_SET DataModelCode=" & Strings.ChrW(34).ToString() & r_sGISDataModelCode.Trim() & Strings.ChrW(34).ToString() & " NextOINumber=" & Strings.ChrW(34).ToString() & CStr(lObjectCount) & Strings.ChrW(34).ToString() & "><RISK_OBJECTS OI=""RISK_OBJECTS"">" & sRiskXML & "</RISK_OBJECTS>"
                r_sXMLDataset = r_sXMLDataset & "<DELETED_OBJECTS OI=""DELETED_OBJECTS""/>"
                r_sXMLDataset = r_sXMLDataset & "<QUOTES NextQuoteNumber=" & Strings.ChrW(34).ToString() & CStr(lQuoteCount) & Strings.ChrW(34).ToString() & " OI=""QUOTES"">"
                r_sXMLDataset = r_sXMLDataset & sQuoteXML & "</QUOTES></DATA_SET>"
            Else
                lObjectCount += 1
                r_sXMLDataset = "<DATA_SET DataModelCode=" & Strings.ChrW(34).ToString() & r_sGISDataModelCode.Trim() & Strings.ChrW(34).ToString() & " NextOINumber=" & Strings.ChrW(34).ToString() & CStr(lObjectCount) & Strings.ChrW(34).ToString() & "><RISK_OBJECTS OI=""RISK_OBJECTS"">" & sRiskXML & "</RISK_OBJECTS>"
                r_sXMLDataset = r_sXMLDataset & "<DELETED_OBJECTS OI=""DELETED_OBJECTS""/>"
                r_sXMLDataset = r_sXMLDataset & "<QUOTES NextQuoteNumber=""1"" OI=""QUOTES""/></DATA_SET>"
            End If
            lPos = (sDummyDataset.IndexOf("<DATA_SET", StringComparison.CurrentCultureIgnoreCase) + 1)
            r_sXMLDataset = sDummyDataset.Substring(0, lPos - 1) & r_sXMLDataset

            r_sXMLDataset = r_sXMLDataset.Replace("&amp;", "&")
            r_sXMLDataset = r_sXMLDataset.Replace("&", "&amp;")

            ' Load the XML
            lReturn = CType(LoadFromXML(v_sDataModelCode:=r_sGISDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDataSet.GetSpecialObject(r_sObjectName:=sSpecialObject, r_lNonGISType:=lNonGISType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If sSpecialObject.Trim() <> "" Then
                lReturn = CType(LoadFromDBSpecials(v_lGISPolicyLinkID:=r_lPolicyLinkID, v_sObjectName:=sSpecialObject, v_lNonGISType:=lNonGISType, v_lClaimID:=r_lClaimId), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                lReturn = CType(ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

        End If

        If bSuppressDataSetDefReturn Then
            r_sXMLDataSetDef = ""
        End If

        Return result
    End Function

    Public Function NBQuoteStateful(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date) As Integer
        Return NBQuoteStateful(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_lGISSchemeID:=-1, r_vAdditionalDataArray:=Nothing, v_lRiskGroupID:=-1, v_bIsBackdatedMTA:=False)
    End Function

    Public Function NBQuoteStateful(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByRef r_vAdditionalDataArray As Object, ByVal v_lRiskGroupID As Integer) As Integer
        Return NBQuoteStateful(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lQuoteType:=v_lQuoteType, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_lGISSchemeID:=-1, r_vAdditionalDataArray:=r_vAdditionalDataArray, v_lRiskGroupID:=v_lRiskGroupID, v_bIsBackdatedMTA:=False)
    End Function

    Public Function NBQuoteStateful(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lQuoteType As Integer, ByVal v_dtEffectiveDate As Date, ByVal v_lGISSchemeID As Integer, ByRef r_vAdditionalDataArray As Object, ByVal v_lRiskGroupID As Integer, ByVal v_bIsBackdatedMTA As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vAllSchemesArray, vQEMSchemeArray As Object
        Dim sCurrentQEM As String = ""
        Dim sNextQEM As New StringBuilder
        Dim lQEMSchemeRow As Integer
        Dim iCounter As Integer
        Dim sGisDataModelCode As String = ""
        Dim lGISPolicyLinkID As Integer
        Dim sXMLDataSetDef As String = ""
        Dim bNew As Boolean
        Dim oBom As Object
        Dim sSourceOfBusiness As String = ""
        Dim sXMLDataSet As String = ""
        Dim lEncodedQuoteType, lTransactionType, lGISScreenId, lQuoteType, lGisDataModelType, lPolicyTypeID As Integer
        Dim bBypassQuoting, bCalledFromSts, bIsSchemesWotIf As Boolean
        Dim lInsFileCnt As Integer
        Dim sRealTransactionType, sOriginalBusinessTypeCode As String
        Dim vRatingInfo As Object
        Dim sRatingEngineDesc As String = String.Empty
        Dim bRunPrePRERule As Boolean
        Dim bRunPostPRERule As Boolean
        Dim sPRECompiledAssembly As String = String.Empty
        Dim sPREEffectiveDateOption As String
        Dim oDataSet As Object = m_oDataSet

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            sSourceOfBusiness = ""
            lGisDataModelType = GISDataModelType.GISDMTypeNotSet
            bCalledFromSts = False

            If Not Informations.IsNothing(r_vAdditionalDataArray) Then
                If Informations.IsArray(r_vAdditionalDataArray) Then
                    For iCnt As Integer = 0 To r_vAdditionalDataArray.GetUpperBound(1)
                        Select Case r_vAdditionalDataArray(0, iCnt)
                            Case CNSourceOfBusiness
                                sSourceOfBusiness = CStr(r_vAdditionalDataArray(1, iCnt))
                            Case CNDataModelType
                                lGisDataModelType = CInt(r_vAdditionalDataArray(1, iCnt))
                            Case "POLICY_TYPE_ID"
                                lPolicyTypeID = CInt(r_vAdditionalDataArray(1, iCnt))
                            Case CNCalledFromSTS
                                If CInt(r_vAdditionalDataArray(1, iCnt)) = ToSafeDouble("1") Then bCalledFromSts = False
                            Case "insurance_file_cnt"
                                lInsFileCnt = CInt(r_vAdditionalDataArray(1, iCnt))
                            Case "Real_Transaction_Type"
                                sRealTransactionType = CStr(r_vAdditionalDataArray(1, iCnt))
                        End Select
                    Next iCnt
                End If
            End If

            If lPolicyTypeID = 3 Or lPolicyTypeID = 10 Then '(3 = general, 10 = schemes)
                ' Check whether we have the required data model specific registry settings  PN27045
                If CheckRqdRegSettingsExistForNBQuote(v_sGisDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Aborting processing since we do not have the required data model specific registry settings (Under PM\SiriusSolutions\Server\GIS\" & v_sGisDataModelCode & ")" & Strings.ChrW(13) & Strings.ChrW(10) & "Please check the registry and correct.", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuoteStateful", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            lReturn = CreateBOM(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_sGisDataModelCode:=CNAgentsOnline, v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_sClassName:=ACClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()
            lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lEncodedQuoteType = v_lQuoteType
            PBQuoteTypeEncode.decodeTransactionScreenAndType(lEncodedQuoteType, lTransactionType, lGISScreenId, lQuoteType)

            If lQuoteType <> PBQuoteTypeEncode.PBCQemQuoteTypePreScreen And lQuoteType <> PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk Then

                ' RFC02091999 - Clear any existing Quote Output before quoting
                lReturn = m_oDataSet.ClearAllQuoteOutput()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            If Not (oBom Is Nothing) Then
                If m_sTransactionType = "MTA" Or m_sTransactionType = "REN" Or m_sTransactionType = "MTC" Then
                    sOriginalBusinessTypeCode = v_sGisBusinessTypeCode
                    v_sGisBusinessTypeCode = m_sTransactionType
                End If

                lReturn = oBom.NBQuoteBefore(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
                If m_sTransactionType = "MTA" Or m_sTransactionType = "REN" Or m_sTransactionType = "MTC" Then
                    v_sGisBusinessTypeCode = sOriginalBusinessTypeCode
                End If
            End If

            lReturn = m_oDataSet.LoadAndApplyGISDefaults("NB")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Get the Data Set Definition
            lReturn = ReturnAsXML(r_sXMLDataset:=sXMLDataSet, r_sXMLDataSetDef:=sXMLDataSetDef)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            ' For quote types other than Validate, Default or PreScreen...
            ' Only actually quote on a scheme if we are processing a Schemes type policy
            ' (as opposed to BackOffice or G2 etc)...but we if could not get the
            ' policy type id (e.g. it was not passed in) then process as usual... PN27045


            If lPolicyTypeID <> 3 Then '(3 = general)
                If bIsSchemesWotIf And v_lRiskGroupID <> -1 Then
                    ' Schemes what-If quote so quote against all schemes for the riskgroup
                    lReturn = m_oGISSchemeBusiness.GetSchemesByRiskGroup(v_lGisPolicyLinkID:=-1, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vAllSchemesArray, v_lGISSchemeId:=-1, v_lQuoteType:=v_lQuoteType, v_lRiskGroupID:=v_lRiskGroupID, v_bCalledFromSTS:=bCalledFromSts, v_sRealTransactionType:=sRealTransactionType)
                ElseIf (v_lRiskGroupID <> -1) Then
                    lReturn = m_oGISSchemeBusiness.GetSchemesByRiskGroup(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vAllSchemesArray, v_lGISSchemeId:=v_lGISSchemeID, v_lQuoteType:=v_lQuoteType, v_lRiskGroupID:=v_lRiskGroupID, v_bCalledFromSTS:=bCalledFromSts, v_sRealTransactionType:=sRealTransactionType)
                Else
                    ' Get the Schemes that we need to quote for

                    lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vAllSchemesArray, v_lGISSchemeId:=v_lGISSchemeID, v_lQuoteType:=v_lQuoteType, v_bCalledFromSTS:=bCalledFromSts, v_sRealTransactionType:=sRealTransactionType)
                End If

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.GetSchemes returned:" & lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuoteStateful", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return lReturn
                End If

                If Not Informations.IsArray(vAllSchemesArray) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'If lPolicyTypeID = 3 Then
                bBypassQuoting = True
            End If

            ' Bypass quoting if flag set previously PN27045
            If Not bBypassQuoting Then
                Dim vDREArray(0, 2) As Object
                ' Get the Data Model Code
                sGisDataModelCode = m_oDataSet.GISDataModelCode
                ' Get the First Quote  Engine Mapper From the List
                '    sCurrentQEM = CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim()
                '   sCurrentQEM = sCurrentQEM & "." & CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, 0)).Trim()

                'DRE Integration - do we need to use bGISQEMPMU or bGISQEMDRE now?

                If (v_lQuoteType Mod 100) = PBCQemQuoteTypeRenewal Then
                    lReturn = GetRatingInfo(v_lGISPolicyLinkID:=lGISPolicyLinkID, v_dEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vRatingArray:=vRatingInfo, v_lQuoteType:=(v_lQuoteType Mod 100), v_sTransType:="RN")
                Else
                    lReturn = GetRatingInfo(v_lGISPolicyLinkID:=lGISPolicyLinkID, v_dEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vRatingArray:=vRatingInfo, v_lQuoteType:=(v_lQuoteType Mod 100))
                End If

                If Informations.IsArray(vRatingInfo) Then
                    sRatingEngineDesc = gPMFunctions.ToSafeString(vRatingInfo(1, 0)).Trim.ToUpper()
                    bRunPostPRERule = gPMFunctions.ToSafeBoolean(vRatingInfo(7, 0))
                    bRunPrePRERule = gPMFunctions.ToSafeBoolean(vRatingInfo(8, 0))
                    sPREEffectiveDateOption = gPMFunctions.ToSafeString(vRatingInfo(9, 0)).ToUpper()
                    sPRECompiledAssembly = gPMFunctions.ToSafeString(vRatingInfo(0, 0)).ToUpper()
                    sPREEffectiveDateOption = gPMFunctions.ToSafeString(vRatingInfo(9, 0)).ToUpper()

                    vDREArray(0, 0) = sPRECompiledAssembly
                    vDREArray(0, 1) = bRunPrePRERule
                    vDREArray(0, 2) = bRunPostPRERule
                    If bRunPrePRERule = True AndAlso sPRECompiledAssembly.ToUpper().Contains(".RUL") AndAlso ((v_lQuoteType Mod 100) = PBCQemQuoteTypeQuote Or (v_lQuoteType Mod 100) = PBCQemQuoteTypeRenewal) Then
                        'set it back to vbs and then run the scripts - the scripts should not do any rating though as it will override the DRE generated ratings
                        sCurrentQEM = "bGISQEMPMU.VBS"
                        lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMDREAdditionalArray:=Nothing, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return lReturn
                        End If
                    ElseIf sRatingEngineDesc = "SCRIPT" Or (v_lQuoteType Mod 100) = PBCQemQuoteTypeUal Then

                        sCurrentQEM = "bGISQEMPMU.VBS"
                    ElseIf sRatingEngineDesc = "PRE" Then
                        If ((v_lQuoteType Mod 100) = PBCQemQuoteTypeQuote Or (v_lQuoteType Mod 100) = PBCQemQuoteTypeRenewal) And ToSafeLong(vRatingInfo(6, 0)) = 1 Then
                            sCurrentQEM = "bGISQEMDRE.DRE"
                            v_dtEffectiveDate = GetEffectiveDateForPRE(v_lGISPolicyLinkID:=lGISPolicyLinkID, v_sPREEffectiveDateOption:=sPREEffectiveDateOption)
                        Else
                            sCurrentQEM = "bGISQEMPMU.VBS"
                        End If
                    ElseIf sRatingEngineDesc = "COMPILED" Then
                        sCurrentQEM = "bGISQEMCOMPILED.NET"
                    Else
                        NBQuoteStateful = lReturn
                        Exit Function
                    End If
                Else
                    sCurrentQEM = "bGISQEMPMU.VBS"
                End If

                'get the screen code as we need this for the assembly name
                Dim vScreenArray(,) As Object
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(lGISScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=PBDatabaseConsts.ACGetAllScreenHeaderSQL, sSQLName:=PBDatabaseConsts.ACGetAllScreenHeaderName, bStoredProcedure:=PBDatabaseConsts.ACGetAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vScreenArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'this block checks if gis screen is running compiled and whether defaults / validation to set the correct QEM (vbs / compiled)
                If (Not Informations.IsNothing(vScreenArray) AndAlso (Informations.IsArray(vScreenArray))) Then
                    Dim nIsEnabledCompiledRule As Integer = If(vScreenArray(PBDatabaseConsts.ACHEnableCompiledRule, 0) = "", 1, gPMFunctions.ToSafeInteger(vScreenArray(PBDatabaseConsts.ACHEnableCompiledRule, 0)))
                    If nIsEnabledCompiledRule = 3 Then
                        If (v_lQuoteType Mod 100) = PBCQemQuoteTypeDefault OrElse (v_lQuoteType Mod 100) = PBCQemQuoteTypeValidate OrElse
                            (v_lQuoteType Mod 100) = PBQuoteTypeEncode.PBCQemQuoteTypePreScreen Then
                            sCurrentQEM = "bGISQEMCOMPILED.NET"
                        End If
                    Else
                        If (v_lQuoteType Mod 100) = PBCQemQuoteTypeDefault OrElse (v_lQuoteType Mod 100) = PBCQemQuoteTypeValidate OrElse
                            (v_lQuoteType Mod 100) = PBQuoteTypeEncode.PBCQemQuoteTypePreScreen Then
                            sCurrentQEM = "bGISQEMPMU.VBS"
                        End If
                    End If
                End If

                'single call to quote now as Pure does not use schemes (hangover from broking code)
                lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMDREAdditionalArray:=vDREArray, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA)

                ' For each Scheme
                'For lRow As Integer = vAllSchemesArray.GetLowerBound(1) To vAllSchemesArray.GetUpperBound(1)
                '    sNextQEM = New StringBuilder(CStr(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchObjectName, lRow)).Trim())
                '    sNextQEM.Append("." & CStr(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchClassName, lRow)).Trim())

                '    ' Is the QEM the same as the last one
                '    If sCurrentQEM <> sNextQEM.ToString() Then
                '        lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMDREAdditionalArray:=directcast(vQEMSchemeArray,object(,)), v_lQuoteType:=gpmfunctions.ToSafeInteger(v_lQuoteType), v_sXMLDataSetDef:=gpmfunctions.ToSafeString(sXMLDataSetDef), v_sXMLDataSet:=gpmfunctions.ToSafeString(sXMLDataSet), v_sGisDataModelCode:=gpmfunctions.ToSafeString(sGisDataModelCode), v_sGisBusinessTypeCode:=gpmfunctions.ToSafeString(v_sGisBusinessTypeCode), v_dtEffectiveDate:=gpmfunctions.ToSafedate(v_dtEffectiveDate), r_vAdditionalDataArray:=directcast(r_vAdditionalDataArray,object(,)))

                '        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                '            Return lReturn

                '        End If

                '        vQEMSchemeArray = Nothing ' reset

                '        sCurrentQEM = sNextQEM.ToString()
                '    End If

                '    If Informations.IsArray(vQEMSchemeArray) Then
                '        ReDim Preserve vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, vQEMSchemeArray.GetUpperBound(1) + 1)
                '    Else
                '        ReDim vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, 0)

                '    End If

                '    lQEMSchemeRow = vQEMSchemeArray.GetUpperBound(1)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchObjectName, lRow), GISSharedConstants.GISQEMSchObjectName, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchClassName, lRow), GISSharedConstants.GISQEMSchClassName, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchSchemeNo, lRow), GISSharedConstants.GISQEMSchSchemeNo, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchSchemeVer, lRow), GISSharedConstants.GISQEMSchSchemeVer, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchFilename, lRow), GISSharedConstants.GISQEMSchFilename, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchQMInsurerRef, lRow), GISSharedConstants.GISQEMSchQMInsurerRef, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchPolarisInsurerNo, lRow), GISSharedConstants.GISQEMSchPolarisInsurerNo, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchType, lRow), GISSharedConstants.GISQEMSchType, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchVariant, lRow), GISSharedConstants.GISQEMSchVariant, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchCommPerc, lRow), GISSharedConstants.GISQEMSchCommPerc, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchID, lRow), GISSharedConstants.GISQEMSchID, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchDesc, lRow), GISSharedConstants.GISQEMSchDesc, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchAbi81Insurer, lRow), GISSharedConstants.GISQEMSchAbi81Insurer, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lRow), GISSharedConstants.GISQEMSchAbi1EdiDirectory, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchAgencyCode, lRow), GISSharedConstants.GISQEMSchAgencyCode, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchEdiMailBox, lRow), GISSharedConstants.GISQEMSchEdiMailBox, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchInsurerDesc, lRow), GISSharedConstants.GISQEMSchInsurerDesc, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchDictVer, lRow), GISSharedConstants.GISQEMSchDictVer, lQEMSchemeRow)
                '    vQEMSchemeArray.SetValue(vAllSchemesArray.GetValue(GISSharedConstants.GISQEMSchInsurerCode, lRow), GISSharedConstants.GISQEMSchInsurerCode, lQEMSchemeRow)
                'Next lRow

                'If Informations.IsArray(vQEMSchemeArray) Then
                'lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMDREAdditionalArray:=directcast(vQEMSchemeArray,object(,)), v_lQuoteType:=gpmfunctions.ToSafeInteger(v_lQuoteType), v_sXMLDataSetDef:=gpmfunctions.ToSafeString(sXMLDataSetDef), v_sXMLDataSet:=gpmfunctions.ToSafeString(sXMLDataSet), v_sGisDataModelCode:=gpmfunctions.ToSafeString(sGisDataModelCode), v_sGisBusinessTypeCode:=gpmfunctions.ToSafeString(v_sGisBusinessTypeCode), v_dtEffectiveDate:=gpmfunctions.ToSafedate(v_dtEffectiveDate), r_vAdditionalDataArray:=directcast(r_vAdditionalDataArray,object(,)))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If

                'End If
            Else
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bBypassQuoting set to TRUE in bGIS.Application.NBQuoteStateful", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuoteStateful")
            End If
            If bRunPostPRERule = True AndAlso sPRECompiledAssembly.ToUpper().Contains(".RUL") AndAlso ((v_lQuoteType Mod 100) = PBCQemQuoteTypeQuote Or (v_lQuoteType Mod 100) = PBCQemQuoteTypeRenewal) Then
                'set it back to vbs and then run the scripts - the scripts should not do any rating though as it will override the DRE generated ratings
                sCurrentQEM = "bGISQEMPMU.VBS"
                lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMDREAdditionalArray:=Nothing, v_lQuoteType:=v_lQuoteType, v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_dtEffectiveDate:=gPMFunctions.ToSafeDate(v_dtEffectiveDate), r_vAdditionalDataArray:=r_vAdditionalDataArray, v_bIsBackdatedMTA:=v_bIsBackdatedMTA, v_bAfterPRETriggerRules:=True)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If
            If Not (oBom Is Nothing) Then
                If m_sTransactionType = "MTA" Or m_sTransactionType = "REN" Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                    sOriginalBusinessTypeCode = v_sGisBusinessTypeCode
                    v_sGisBusinessTypeCode = m_sTransactionType
                End If
                lReturn = oBom.NBQuoteAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), v_lQuoteType:=gPMFunctions.ToSafeInteger(v_lQuoteType), r_oDataSet:=oDataSet, r_vAdditionalDataArray:=r_vAdditionalDataArray)

                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNBQuoteReferred And lReturn <> gPMConstants.PMEReturnCode.PMNBQuoteDeclined) Then
                    Return lReturn
                End If
                m_oDataSet = oDataSet
                If m_sTransactionType = "MTA" Or m_sTransactionType = "REN" Or m_sTransactionType = "MTC" Or m_sTransactionType = "MTR" Then
                    v_sGisBusinessTypeCode = sOriginalBusinessTypeCode
                End If

            End If

            If GISSharedConstants.SaveOnQuote(v_sGisDataModelCode, v_sGisBusinessTypeCode) Then
                lReturn = SaveInDB()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NBQuoteStatefulFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuoteStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function





    Public Function ReloadSpecialsFromDB(ByVal v_sGisDataModelCode As String, ByRef r_sXMLDataset As String) As Integer
        Return ReloadSpecialsFromDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=r_sXMLDataset, v_lClaimID:=-1)
    End Function

    Public Function ReloadSpecialsFromDB(ByVal v_sGisDataModelCode As String, ByRef r_sXMLDataset As String, ByVal v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSet, sXMLDataSetDef As String
        Dim lPolicyLinkID As Integer
        Dim sSpecialObject As String = ""
        Dim lNonGISType As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(LoadFromXML(v_sDataModelCode:=v_sGisDataModelCode, v_sXMLDataSet:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Save to the Database before we reload
            lReturn = CType(SaveInDB(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            ' Is there a Special Object
            lReturn = m_oDataSet.GetSpecialObject(r_sObjectName:=sSpecialObject, r_lNonGISType:=lNonGISType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sSpecialObject.Trim() <> "" Then

                lPolicyLinkID = m_oDataSet.PolicyLinkID()

                ' Yes, so load them from the DB
                lReturn = CType(LoadFromDBSpecials(v_lGISPolicyLinkID:=lPolicyLinkID, v_sObjectName:=sSpecialObject, v_lNonGISType:=lNonGISType, v_lClaimID:=v_lClaimID), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Return the Dataset XML
            lReturn = CType(ReturnAsXML(r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReloadSpecialsFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReloadSpecialsFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function LoadFromDBSpecials(ByVal v_lGISPolicyLinkID As Integer, ByVal v_sObjectName As String, ByVal v_lNonGISType As Integer, Optional ByVal v_lClaimID As Integer = -1) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSPName, sTableName As String
        Dim oDOM As XmlDocument


        result = gPMConstants.PMEReturnCode.PMTrue

        oDOM = New XmlDocument()
        ' Get the TableName
        lReturn = m_oDataSet.GetObjectDefDetails(v_sObjectName:=v_sObjectName, r_sTableName:=sTableName)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Check what type of Special we are loading

        Select Case v_lNonGISType
            Case GISDataModelType.GISOTClaim

                If v_lClaimID < 1 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Claim ID MUST be supplied to load Claim Specials.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDBSpecials")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Case GISDataModelType.GISOTAssociatedClient
                If v_lGISPolicyLinkID < 1 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Policy Link ID MUST be supplied to load Associated Client Specials.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDBSpecials")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Case Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unknown Special Type.", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadFromDBSpecials")
                Return gPMConstants.PMEReturnCode.PMFalse

        End Select

        ' Build the Stored Proc Name
        sSPName = "spg_" & sTableName.Trim() & "_sel"

        With m_oDatabase
            m_oDatabase.Parameters.Clear()
            ' If we have been supplied a Claim ID then we must be trying to LoadClaimSpecials.
            If v_lClaimID > 0 Then
                ' live claim id
                lReturn = CType(AddDatabaseParameter(sName:="claim_id", vValue:=v_lClaimID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            Else
                lReturn = .Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return lReturn
                End If
            End If

            lReturn = .SQLSelectForXML(sSQL:=sSPName, bStoredProcedure:=True, oXMLDOM:=oDOM)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If
        End With

        ' Update the Dataset
        lReturn = m_oDataSet.UpdateSpecialObjects(v_sObjectName, oDOM)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            oDOM = Nothing
            Return lReturn
        End If

        Return result
    End Function

    Private Function AddDatabaseParameter(ByVal sName As String, ByVal vValue As Object, ByVal iDirection As Integer, ByVal iType As Integer) As Integer
        Dim result As Integer = 0
        Dim bEmpty As Boolean


        result = gPMConstants.PMEReturnCode.PMFalse
        bEmpty = False

        Select Case iType
            Case gPMConstants.PMEDataType.PMString, gPMConstants.PMEDataType.PMTableName, gPMConstants.PMEDataType.PMFieldName
                If CStr(vValue).Trim = "" Then
                    bEmpty = True
                End If
            Case gPMConstants.PMEDataType.PMInteger, gPMConstants.PMEDataType.PMLong, gPMConstants.PMEDataType.PMDouble, gPMConstants.PMEDataType.PMCurrency, gPMConstants.PMEDataType.PMDecimal
                If CInt(vValue) < 1 Then
                    bEmpty = True
                End If
            Case Else
        End Select

        If bEmpty Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:=sName, vValue:=(DBNull.Value), iDirection:=iDirection, iDataType:=iType)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:=sName, vValue:=CStr(vValue), iDirection:=iDirection, iDataType:=iType)
        End If

        Return m_lReturn
    End Function

    Private Function CopyDataSetViaSPStateful(ByVal v_sDataModelCode As String, ByVal v_lSaveToDBMode As Integer, ByVal v_bCopyQuotes As Boolean, ByRef r_lNewGISPolicyLinkID As Integer, Optional ByVal v_vOldGISPolicyLinkId As Integer = 0, Optional ByVal v_vOldInsuranceFileCnt As Integer = 0, Optional ByVal v_vNewInsuranceFileCnt As Integer = 0, Optional ByVal v_vOldRiskID As Integer = 0, Optional ByVal v_vNewRiskID As Integer = 0, Optional ByVal v_vNewQuoteRef As String = "", Optional ByVal v_vNewQuoteRefPassword As String = "") As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sXMLDataSetDef, sXMLDataSet, sSPCall As String
        Dim bNew As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        'CJB121101 If any of the optional parms were not passed in then set to default values
        If Informations.IsNothing(v_vOldGISPolicyLinkId) Then v_vOldGISPolicyLinkId = -1
        If Informations.IsNothing(v_vOldInsuranceFileCnt) Then v_vOldInsuranceFileCnt = -1
        If Informations.IsNothing(v_vNewInsuranceFileCnt) Then v_vNewInsuranceFileCnt = -1
        If Informations.IsNothing(v_vOldRiskID) Then v_vOldRiskID = -1
        If Informations.IsNothing(v_vNewRiskID) Then v_vNewRiskID = -1
        If Informations.IsNothing(v_vNewQuoteRef) Then v_vNewQuoteRef = ""
        If Informations.IsNothing(v_vNewQuoteRefPassword) Then v_vNewQuoteRefPassword = ""

        ' Format the SP name based on the Data Model Code
        sSPCall = "spg_" & v_sDataModelCode.Trim().ToLower() & "_copy_dataset"

        ' Clear the Parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = CType(AddDatabaseParameter(sName:="old_gis_policy_link_id", vValue:=v_vOldGISPolicyLinkId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(AddDatabaseParameter(sName:="old_insurance_file_cnt", vValue:=v_vOldInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(AddDatabaseParameter(sName:="old_risk_id", vValue:=v_vOldRiskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(AddDatabaseParameter(sName:="new_insurance_file_cnt", vValue:=v_vNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(AddDatabaseParameter(sName:="new_risk_id", vValue:=v_vNewRiskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If we are saving Quotes in the Database and we Want to Copy them
        If v_lSaveToDBMode = GISSharedConstants.GISRegLoadSaveDBModeFastWithQuotes And v_bCopyQuotes Then
            lReturn = m_oDatabase.Parameters.Add(sName:="copy_quotes", vValue:=CStr(gPMConstants.PMEReturnCode.PMTrue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else
            lReturn = m_oDatabase.Parameters.Add(sName:="copy_quotes", vValue:=CStr(gPMConstants.PMEReturnCode.PMFalse), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(AddDatabaseParameter(sName:="new_quote_ref", vValue:=CInt(v_vNewQuoteRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CType(AddDatabaseParameter(sName:="new_quote_ref_password", vValue:=CInt(v_vNewQuoteRefPassword), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lReturn = m_oDatabase.Parameters.Add(sName:="new_gis_policy_link_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLAction(sSQL:=sSPCall, sSQLName:="CopyDataset", bStoredProcedure:=True)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Return the created Policy Link ID
        r_lNewGISPolicyLinkID = m_oDatabase.Parameters.Item("new_gis_policy_link_id").Value

        Return result
    End Function

    Public Function NewRiskDatasetStateful(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lRiskID As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return NewRiskDatasetStateful(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, v_lRiskID:=v_lRiskID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQuoteRef:="-1", r_sQuoteRefPassword:="-1")
    End Function

    Public Function NewRiskDatasetStateful(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lRiskID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:="", r_sXMLDataset:="", v_lRiskID:=v_lRiskID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sQuoteRef:=r_sQuoteRef, r_sQuoteRefPassword:=r_sQuoteRefPassword), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewRiskDatasetStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewRiskDatasetStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewClaimDatasetStateful(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:="", r_sXMLDataset:="", v_lClaimID:=v_lClaimID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewClaimDatasetStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewClaimDatasetStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewClaimDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lClaimID:=v_lClaimID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewClaimDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewClaimDataset", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewPartyDatasetStateful(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:="", r_sXMLDataset:="", v_lPartyCnt:=v_lPartyCnt), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewPartyDatasetStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewPartyDatasetStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewPartyDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lPartyCnt:=v_lPartyCnt), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewPartyDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewPartyDataset", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewPolicyDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewPolicyDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewPolicyDataset", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadRiskFromDBStateful(ByRef r_sGISDataModelCode As String, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return LoadRiskFromDBStateful(r_sGISDataModelCode:=r_sGISDataModelCode, r_sQuoteRef:="", r_sQuoteRefPassword:="", r_dtGuaranteedQuoteDate:=#12/30/1899#, v_lRiskID:=-1, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
    End Function

    Public Function LoadRiskFromDBStateful(ByRef r_sGISDataModelCode As String, ByVal v_lRiskID As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Return LoadRiskFromDBStateful(r_sGISDataModelCode:=r_sGISDataModelCode, r_sQuoteRef:="", r_sQuoteRefPassword:="", r_dtGuaranteedQuoteDate:=#12/30/1899#, v_lRiskID:=v_lRiskID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
    End Function

    Public Function LoadRiskFromDBStateful(ByRef r_sGISDataModelCode As String, ByVal r_sQuoteRef As String, ByRef r_sQuoteRefPassword As String, ByRef r_dtGuaranteedQuoteDate As Date, ByVal v_lRiskID As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:="", r_sXMLDataset:="", r_sGISDataModelCode:=r_sGISDataModelCode, r_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lRiskID:=v_lRiskID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadRiskFromDBStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRiskFromDBStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function GetXSDSuppressionFlag(ByVal v_sDataModelCode As String, ByRef r_bSuppressXSDProduction As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetXSDSuppressionFlag"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vDataModelDetails As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' by default suppress XSD production
            r_bSuppressXSDProduction = True

            ' get the details for the data model
            lReturn = CType(GetDataModelDetails(v_sDataModelCode, r_vResults:=vDataModelDetails), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetDataModelDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(vDataModelDetails) Then

                m_sDataModelType = CStr(vDataModelDetails(0, 0)).Trim().ToUpper()
            End If

            If m_sDataModelType = kGisDataModelTypeRISK Or m_sDataModelType = kGisDataModelTypeCLAIM _
                Or m_sDataModelType = kGisDataModelTypePOLICY Or m_sDataModelType = kGisDataModelTypePARTY Or m_sDataModelType = kGisDataModelTypeCASE Then
                r_bSuppressXSDProduction = False
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    Public Function GetDataModelDetails(ByVal v_sDataModelCode As String, ByRef r_vResults(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetDataModelDetails"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            'm_lReturn = CType(AddDatabaseParameter(sName:="code", vValue:=CInt(v_sDataModelCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddDatabaseParameter(sName:="code", vValue:=(v_sDataModelCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetDataModelDetailsSQL, sSQLName:=kGetDataModelDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetDataModelDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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

    Public Function CheckRqdRegSettingsExistForNBQuote(ByVal v_sDataModelCode As String) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sQEMMethodsVersionNum, sRulePath As String

        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            ' First check we have a value for QEMMethodsVersionNum under HKLM\Software\PM\SiriusSolutions\Server\GIS\<datamodelcode>
            lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:=GISSharedConstants.GISQEMMethodsVersionNum, r_sSettingValue:=sQEMMethodsVersionNum, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer), gPMConstants.PMEReturnCode)

            If sQEMMethodsVersionNum.Trim() <> "" Then
                ' Now check we have a value for RulePath under HKLM\Software\PM\SiriusSolutions\Server\GIS\<datamodelcode>
                lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sDataModelCode, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer), gPMConstants.PMEReturnCode)
            End If

            If sQEMMethodsVersionNum.Trim() <> "" And sRulePath.Trim() <> "" Then
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result
        Catch excep As System.Exception
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckRqdRegSettingsExistForNBQuote", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRqdRegSettingsExistForNBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewCaseDataset(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByVal v_lCaseID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, v_lCaseID:=v_lCaseID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewCaseDataset failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewCaseDataset", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function NewCaseDatasetStateful(ByVal v_sGisDataModelCode As String, ByRef r_lPolicyLinkID As Integer, ByRef r_sTopOIKey As String, ByVal v_lCaseID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(NewDataSetPrivate(v_sGisDataModelCode:=v_sGisDataModelCode, r_lPolicyLinkID:=r_lPolicyLinkID, r_sTopOIKey:=r_sTopOIKey, r_sXMLDataSetDef:="", r_sXMLDataset:="", v_lCaseID:=v_lCaseID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewCaseDatasetStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewCaseDatasetStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadCaseFromDB(ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sGISDataModelCode As String, ByVal v_lCaseID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=r_sGISDataModelCode, r_lCaseID:=v_lCaseID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadCaseFromDB failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadCaseFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Public Function LoadCaseFromDBStateful(ByRef r_sGISDataModelCode As String, ByVal v_lCaseID As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = CType(LoadFromDBPrivate(r_sXMLDataSetDef:="", r_sXMLDataset:="", r_sGISDataModelCode:=r_sGISDataModelCode, r_lCaseID:=v_lCaseID), gPMConstants.PMEReturnCode)
            Return m_lReturn
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadCaseFromDBStateful failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadCaseFromDBStateful", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
    Public Function UpdateRiskAfter(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
ByVal v_lRiskCnt As Integer, ByVal sTransactionType As String) As Integer
        Return UpdateRiskAfter(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                     v_lRiskCnt:=v_lRiskCnt, sTransactionType:=sTransactionType,
                                     v_bViaSAM:=False,
                                     v_iRiskStatusID:=0)
    End Function

    Public Function UpdateRiskAfter(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
ByVal v_lRiskCnt As Integer, ByVal sTransactionType As String, ByVal v_bViaSAM As Boolean) As Integer
        Return UpdateRiskAfter(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                     v_lRiskCnt:=v_lRiskCnt, sTransactionType:=sTransactionType,
                                     v_bViaSAM:=v_bViaSAM,
                                     v_iRiskStatusID:=0)
    End Function

    Public Function UpdateRiskAfter(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
ByVal v_lRiskCnt As Integer, ByVal sTransactionType As String, ByVal v_iRiskStatusID As Integer) As Integer
        Return UpdateRiskAfter(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt,
                                     v_lRiskCnt:=v_lRiskCnt, sTransactionType:=sTransactionType,
                                     v_bViaSAM:=False,
                                     v_iRiskStatusID:=v_iRiskStatusID)
    End Function

    ''' <summary>
    ''' This function will update risk detals in database
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lRiskCnt"></param>
    ''' <param name="sTransactionType"></param>
    ''' <param name="v_bViaSAM"></param>
    ''' <param name="v_iRiskStatusID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Function UpdateRiskAfter(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer,
                                        ByVal v_lRiskCnt As Integer, ByVal sTransactionType As String,
                                        Optional ByVal v_bViaSAM As Boolean = False,
                                        Optional ByVal v_iRiskStatusID As Integer = 0) As Integer
        Dim nResult As Integer
        Dim oBusiness As Object
        Dim nReturn As Integer = 0

        Dim i As Integer
        Dim bApplyReinsurance As Boolean
        Dim bApplyTaxes As Boolean
        Dim sTaxesDescription As String = ""
        Dim bDisplayScreen As Object
        Dim oCommissionArray As Object
        Dim bTaxesSwithedOff As Boolean
        Dim bIsRIValid As Boolean
        Dim nRIValid As Object = 0
        Dim nRIBand As Object = 0
        Dim nInsuranceFileCnt As Integer = 0
        Dim nNBProRata As Integer = 0
        Dim nMTAProRata As Integer = 0
        Dim nShortPeriodRated As Integer = 0
        Dim nOriginalRiskCnt As Integer = 0
        Dim oArray(,) As Object
        Dim sStatusFlag As String
        Dim sDataModel As String
        Dim nPolicyBinder As Integer = 0
        Dim dtOldCoverStartDate As Date
        Dim dtOldExpiryDate As Date
        Dim dtCoverStartDate As Date
        Dim dtExpiryDate As Date
        Dim dtInceptionDate As Date
        Dim nProductID As Integer = 0
        Dim nInsuranceFileTypeId As Integer = 0
        Dim nIsTemporaryMTA As Integer = 0
        Dim nRoundPremium As Integer = 0
        Dim nRoundingSection As Integer = 0
        Dim sRoundingSectionCode As String = ""
        Dim dProRataRate As Double
        Dim bShortPeriodRated As Boolean
        Dim nProRata As Integer = 0
        Dim bSkipRI As Boolean
        Dim sIsRI2007 As String = ""
        Dim oReinsurance As Object
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            bIsRIValid = True

            ' Create bSIRPerilAllocation object
            nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                v_sClassName:="bSirPerilAllocation.Business",
                                                                v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername),
                                                                v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                                                v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                                                v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                                                v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                                                v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSIRReinsurance.Form object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            oBusiness.InsuranceFolderCnt = v_lInsuranceFolderCnt
            oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt
            oBusiness.RiskID = v_lRiskCnt
            oBusiness.TransactionType = sTransactionType

            m_lReturn = CType(GetProRataFlag(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_iNBProrata:=nNBProRata,
                                             r_iMTAProrata:=nMTAProRata, r_iShortPeriodRate:=nShortPeriodRated),
                              gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the original risk cnt
            m_oDatabase.Parameters.Clear()
            AddDatabaseParameter("insurance_file_cnt", v_lInsuranceFileCnt,
                                 gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            AddDatabaseParameter("risk_cnt", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput,
                                 gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectIFRLSQL, sSQLName:=ACSelectIFRLName,
                                              bStoredProcedure:=ACSelectIFRLStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Renewals puts 0 into this instead of null

            If (gPMFunctions.NullToString(oArray(3, 0)) = "") OrElse (gPMFunctions.NullToString(oArray(3, 0)) = "0") Then
                nOriginalRiskCnt = -1
            Else
                nOriginalRiskCnt = CInt(oArray(3, 0))
            End If

            sStatusFlag = CStr(oArray(2, 0))

            oArray = Nothing

            ' moved to AFTER we've got the original risk id
            m_oDatabase.Parameters.Clear()
            AddDatabaseParameter("insurance_file_cnt", v_lInsuranceFolderCnt,
                                 gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' if cancelling, use original risk id
            If m_sTransactionType = "MTC" Then
                AddDatabaseParameter("risk_id", nOriginalRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput,
                                     gPMConstants.PMEDataType.PMLong)
            Else
                AddDatabaseParameter("risk_id", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput,
                                     gPMConstants.PMEDataType.PMLong)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISDataModelSQL, sSQLName:=ACSelectGISDataModelName,
                                              bStoredProcedure:=ACSelectGISDataModelStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sDataModel = CStr(oArray(0, 0)).Trim()
            nPolicyBinder = CInt(oArray(1, 0))
            oArray = Nothing

            If nOriginalRiskCnt = -1 Then
                nInsuranceFileCnt = v_lInsuranceFileCnt
            Else
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(nOriginalRiskCnt),
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                       iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If this is an MTA Reinstatement then get the dates from the cancellation version.
                If m_sTransactionType = "MTR" Then
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileCntMTRSQL,
                                                      sSQLName:=ACSelectInsuranceFileCntName,
                                                      bStoredProcedure:=ACSelectInsuranceFileCntStored,
                                                      lNumberRecords:=gPMConstants.PMAllRecords,
                                                      vResultArray:=oArray)
                Else
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileCntSQL,
                                                      sSQLName:=ACSelectInsuranceFileCntName,
                                                      bStoredProcedure:=ACSelectInsuranceFileCntStored,
                                                      lNumberRecords:=gPMConstants.PMAllRecords,
                                                      vResultArray:=oArray)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not Informations.IsArray(oArray) Then
                    nOriginalRiskCnt = -1
                    nInsuranceFileCnt = v_lInsuranceFileCnt
                Else
                    nInsuranceFileCnt = CInt(oArray(0, 0))

                    dtOldCoverStartDate = CDate(oArray(1, 0))

                    dtOldExpiryDate = CDate(oArray(2, 0))
                End If

                oArray = Nothing

            End If

            'Now we get the insurance file record and get the cover start and expiry dates

            'We can get both from the current insurance file record - vital
            'if it's a temporary MTA
            m_oDatabase.Parameters.Clear()
            AddDatabaseParameter("insurance_file_cnt", v_lInsuranceFileCnt,
                                 gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectInsuranceFileSQL, sSQLName:=ACSelectInsuranceFileName,
                                              bStoredProcedure:=ACSelectInsuranceFileStored,
                                              lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If _
                Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("cover_start_date")) OrElse
                Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("cover_start_date")) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                dtCoverStartDate = m_oDatabase.Records.Item(0).Fields()("cover_start_date")
            End If

            If _
                Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("expiry_date")) OrElse
                Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("expiry_date")) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                dtExpiryDate = m_oDatabase.Records.Item(0).Fields()("expiry_date")
            End If

            If _
                Not _
                (Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("inception_date_tpi")) OrElse
                 Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("inception_date_tpi"))) Then
                dtInceptionDate = m_oDatabase.Records.Item(0).Fields()("inception_date_tpi")
            End If

            If nOriginalRiskCnt = -1 Then
                dtOldCoverStartDate = dtCoverStartDate
                dtOldExpiryDate = dtExpiryDate
            End If

            If dtExpiryDate > dtOldExpiryDate Then
                'This will never be the case...
                If sTransactionType = "NB" AndAlso nNBProRata = 1 Then
                    nNBProRata = 0
                End If
                'Though this could, if the set up is dodgy
                If sTransactionType = "MTA" AndAlso nMTAProRata = 1 Then
                    nMTAProRata = 0
                End If
            End If

            nProductID = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields()("product_id"))

            If _
                Convert.IsDBNull(m_oDatabase.Records.Item(0).Fields()("insurance_file_type_id")) OrElse
                Informations.IsNothing(m_oDatabase.Records.Item(0).Fields()("insurance_file_type_id")) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                nInsuranceFileTypeId = m_oDatabase.Records.Item(0).Fields()("insurance_file_type_id")
            End If

            If nInsuranceFileTypeId = 6 OrElse nInsuranceFileTypeId = 7 Then
                nIsTemporaryMTA = True
            End If

            m_lReturn = CType(GetUWProductOptions(v_lProductID:=nProductID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(GetRoundingInfo(v_lProductID:=nProductID, r_iRoundPremium:=nRoundPremium,
                                              r_lRoundingSectionID:=nRoundingSection,
                                              r_sRoundingSectionCode:=sRoundingSectionCode),
                              gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If nShortPeriodRated = 1 Then
                ' Find short period rate
                ' Store the rate as if it is a standard pro-rata rate as it will be
                ' used in the same way.
                m_lReturn = CType(GetShortPeriodRate(v_lProductID:=nProductID, v_dtOldStartDate:=dtOldCoverStartDate,
                                                     v_dtOldEndDate:=dtOldExpiryDate,
                                                     v_dtStartDate:=gPMFunctions.ToSafeDate(dtCoverStartDate), v_dtEndDate:=gPMFunctions.ToSafeDate(dtExpiryDate),
                                                     r_dProRataRate:=dProRataRate),
                                  gPMConstants.PMEReturnCode)

                ' Check return
                Select Case m_lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue
                        bShortPeriodRated = True
                    Case gPMConstants.PMEReturnCode.PMNotFound
                        bShortPeriodRated = False
                    Case Else
                        bShortPeriodRated = False

                        ' Log Error Message and continue without SPR
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                           sMsg:="Unable to determine short period rating", vApp:=ACApp,
                                           vClass:=ACClass, vMethod:="UpdateRiskAfter")
                End Select
            Else
                bShortPeriodRated = False
            End If

            Select Case m_sTransactionType
                Case "NB"
                    nProRata = nNBProRata
                Case Else
                    nProRata = nMTAProRata
            End Select

            If Not bShortPeriodRated Then
                ' Peter Finney 10/09/2003 - Use property as this will do any checks for us
                If nProRata Then
                    'get pro rata rate
                    If dtOldExpiryDate > dtExpiryDate Then
                        'Fetch pro rata on the basis of original cover expiry date
                        m_lReturn = CType(GetProRataRate(v_lProductID:=nProductID,
                                                         v_dtOldStartDate:=dtOldCoverStartDate,
                                                         v_dtOldEndDate:=dtOldExpiryDate,
                                                         v_dtStartDate:=gPMFunctions.ToSafeDate(dtCoverStartDate),
                                                         v_dtEndDate:=dtOldExpiryDate, r_dProRataRate:=dProRataRate),
                                          gPMConstants.PMEReturnCode)
                    Else
                        m_lReturn = CType(GetProRataRate(v_lProductID:=nProductID,
                                                         v_dtOldStartDate:=dtOldCoverStartDate,
                                                         v_dtOldEndDate:=dtOldExpiryDate,
                                                         v_dtStartDate:=gPMFunctions.ToSafeDate(dtCoverStartDate), v_dtEndDate:=gPMFunctions.ToSafeDate(dtExpiryDate),
                                                         r_dProRataRate:=dProRataRate,
                                                         v_dtInceptionDate:=dtInceptionDate),
                                          gPMConstants.PMEReturnCode)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If

                Else
                    dProRataRate = 1
                End If
            End If

            nReturn = UpdateRisk(v_lRiskCnt, dProRataRate)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSirPerilAllocation.Business.UpdateRisk method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="NBQuoteAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            'Create bSIRReinsurance.Form object
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword,
                                                      v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID,
                                                      v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID,
                                                      v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp,
                                                      v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007,
                                                      v_vBranch:=m_iSourceID, r_vUnderwriting:=sIsRI2007)

            If sIsRI2007 = "1" Then
                nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                    v_sClassName:="bSIRReinsuranceRI2007.Form",
                                                                    v_sCallingAppName:=ACApp,
                                                                    v_sUsername:=gPMFunctions.ToSafeString(m_sUsername),
                                                                    v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                                                    v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                                                    v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                                                    v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                                                    v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel),
                                                                    v_oDatabase:=m_oDatabase)
            Else
                nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                    v_sClassName:="bSIRReinsurance.Form",
                                                                    v_sCallingAppName:=ACApp,
                                                                    v_sUsername:=gPMFunctions.ToSafeString(m_sUsername),
                                                                    v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                                                    v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                                                    v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                                                    v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                                                    v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel),
                                                                    v_oDatabase:=m_oDatabase)
            End If


            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSIRReinsurance.Form object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            m_lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0,
                                                  vProcessMode:=110, vTransactionType:=gPMFunctions.ToSafeString(sTransactionType),
                                                  vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to SetProcessModes for bSIRReinsurance.Form object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Set the business keys.
            oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt
            oBusiness.RiskID = v_lRiskCnt

            nReturn = oBusiness.AutoReinsure(gPMFunctions.ToSafeString(bApplyReinsurance))

            If oBusiness IsNot Nothing AndAlso oBusiness.ApplyReIns IsNot Nothing Then
                bApplyReinsurance = oBusiness.ApplyReIns
            End If

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSIRReinsurance.Form.ApplyReinsurance method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            If Not bApplyReinsurance Then
                nReturn = oBusiness.ChangeRiskStatus
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="bSIRReinsurance.Form.ChangeRiskStatus method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
            Else
                ' Delete previous Reinsurance
                nReturn = DeleteExistingReinsurance(v_lRiskCnt)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="DeleteExistingReinsurance method failed.", vApp:=ACApp,
                                                      vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                nReturn = oBusiness.CalculateRI
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="bSIRReinsurance.Form.CalculateRI method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                nReturn = oBusiness.GetDetails
                If nReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    bSkipRI = True
                End If
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso nReturn <> gPMConstants.PMEReturnCode.PMNotFound _
                    Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="bSIRReinsurance.Form.GetDetails method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                nReturn = oBusiness.Update
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="bSIRReinsurance.Form.Update method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                nReturn = oBusiness.ValidateBands(r_lValid:=nRIValid, r_lBand:=nRIBand)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="bSIRReinsurance.Form.ValidateBands method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                If nRIValid <> 0 Then
                    bIsRIValid = False
                End If
                If bSkipRI And nRIValid = 0 Then
                    bIsRIValid = False
                End If
            End If

            oBusiness.Dispose()
            oBusiness = Nothing

            ' Create the bSIRRITax.Business object
            nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                v_sClassName:="bSIRRiskData.Business",
                                                                v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername),
                                                                v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                                                v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                                                v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                                                v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                                                v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSirRiskData.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            m_lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0,
                                                  vProcessMode:=110, vTransactionType:=gPMFunctions.ToSafeString(sTransactionType),
                                                  vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to SetProcessModes for bSirRiskData.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            If Not (v_bViaSAM AndAlso (v_iRiskStatusID = 1 OrElse v_iRiskStatusID = 2)) Then
                m_lReturn = oBusiness.UpdateRiskStatus(v_lRiskCnt:=gPMFunctions.ToSafeInteger(v_lRiskCnt),
                                                       v_lRiskStatusID:=If(bIsRIValid, 3, 4))
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:="bSIRReinsurance.Form.ChangeRiskStatus method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
            End If

            oBusiness.Dispose()
            oBusiness = Nothing

            'We need three business object here
            Dim oSIRPartyFee As bSIRPartyFee.UBusiness
            Dim oSIRRITax As bSIRRITax.Business
            Dim oSIRListRisks As Object = Nothing ' bSIRListRisks.Business

            ' Create the oSIRPartyFee.Business object
            oSIRPartyFee = New bSIRPartyFee.UBusiness
            nReturn = oSIRPartyFee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSIRPartyFee.UBusiness object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            m_lReturn = oSIRPartyFee.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0,
                                                     vProcessMode:=110, vTransactionType:=sTransactionType,
                                                     vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to SetProcessModes for bSIRPartyFee.UBusiness object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Create the bSIRRITax.Business object
            oSIRRITax = New bSIRRITax.Business
            nReturn = oSIRRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSIRRITax.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            m_lReturn = oSIRRITax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0,
                                                  vProcessMode:=110, vTransactionType:=sTransactionType,
                                                  vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to SetProcessModes for bSIRRITax.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Create the bSIRListRisks.Business object
            'oSIRListRisks = New bSIRListRisks.Business
            nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSIRListRisks, v_sClassName:="bSIRListRisks.Business", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskAfter Failed - Failed to create business object to bSIRInsuranceFile.Services.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If
            Dim oDatabase As Object = m_oDatabase
            nReturn = oSIRListRisks.Initialise(sUsername:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSIRListRisks.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If
            m_oDatabase = oDatabase

            m_lReturn = oSIRListRisks.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0,
                                                      vProcessMode:=110, vTransactionType:=gPMFunctions.ToSafeString(sTransactionType),
                                                      vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to SetProcessModes for bSIRListRisks.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Recalculate the fees and taxes

            nReturn = oSIRPartyFee.RecalculateRiskFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                       v_lRiskCnt:=v_lRiskCnt,
                                                       v_lTransactionTypeId:=nTransactionType,
                                                       v_bUseExistingFeeDetail:=False)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSIRPartyFee.UBusiness.RecalculateRiskFees method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If
            oSIRRITax.InsuranceFileCnt = v_lInsuranceFileCnt
            ' Recalculate the fees and taxes

            nReturn = oSIRRITax.RecalculatePolicyRiskTaxes(v_lRiskCnt:=v_lRiskCnt,
                                                           v_lTask:=gPMConstants.PMEComponentAction.PMEdit,
                                                           v_sTransactionType:=sTransactionType)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSIRRITax.Business.RecalculateRisk method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Recalculate the Policy taxes

            nReturn = oSIRListRisks.UpdatePolicyPremium(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSIRListRisks.Business.UpdatePolicyPremium method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Recalculate the Policy taxes
            If v_bViaSAM Then
                nReturn = oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=-1,
                                                                             v_lTransactionTypeId:=nTransactionType,
                                                                             v_bUseExistingFeeDetail:=False,
                                                                             nViaSam:=1)
            Else


                nReturn = oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=-1,
                                                             v_lTransactionTypeId:=nTransactionType,
                                                             v_bUseExistingFeeDetail:=False)
            End If
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSIRPartyFee.UBusiness.RecalculatePolicyFees method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            ' Recalculate the Policy taxes

            nReturn = oSIRRITax.RecalculatePolicyTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                       v_lTask:=gPMConstants.PMEComponentAction.PMEdit,
                                                       v_sTransactionType:=sTransactionType)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:="bSIRListRisks.Business.RecalculatePolicyTaxes method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            oSIRPartyFee.Dispose()
            oSIRPartyFee = Nothing

            oSIRListRisks.Dispose()
            oSIRListRisks = Nothing

            oSIRRITax.Dispose()
            oSIRRITax = Nothing


            ' Create the bSirAgentCommission.Business object
            nReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                v_sClassName:="bSirAgentCommission.Business",
                                                                v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername),
                                                                v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                                                v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                                                v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                                                v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                                                v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to Initialise bSirAgentCommission.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            m_lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0,
                                                  vProcessMode:=110, vTransactionType:=gPMFunctions.ToSafeString(sTransactionType),
                                                  vEffectiveDate:=DateTime.Now)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "UpdateRiskAfter Failed to SetProcessModes for bSirAgentCommission.Business object.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            'Set the Business Object Properties
            oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt

            nReturn = oBusiness.CheckDisplayCommission(r_bDisplayScreen:=bDisplayScreen)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = nReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                  sMsg:=
                                                     "bSirAgentCommission.Business.CheckDisplayCommission method failed.",
                                                  vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                  vErrNo:=Informations.Err().Number,
                                                  vErrDesc:=Informations.Err().Description)
                Return nResult
            End If

            If Not bDisplayScreen Then
                'No commission do nothing
            Else

                nReturn = oBusiness.CalculateAgentCommission(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt),
                                                             v_sTransactionType:=gPMFunctions.ToSafeString(sTransactionType),
                                                             r_vntResult:=oCommissionArray)

                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = nReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                      sMsg:=
                                                         "bSirAgentCommission.Business.CalculateAgentCommission method failed.",
                                                      vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                      vErrNo:=Informations.Err().Number,
                                                      vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                If Informations.IsArray(oCommissionArray) Then
                    'Update the lead commission records for the insurancefile

                    nReturn = oBusiness.UpdateLeadCommission(gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        nResult = nReturn
                        ' Log Error Message
                        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                                          sMsg:=
                                                             "bSirAgentCommission.Business.UpdateLeadCommission method failed.",
                                                          vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskAfter",
                                                          vErrNo:=Informations.Err().Number,
                                                          vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                End If

            End If

            oBusiness.Dispose()
            oBusiness = Nothing

            Return nResult
        Catch excep As System.Exception
            m_oDataSet = Nothing
            nResult = gPMConstants.PMEReturnCode.PMError
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                              sMsg:="UpdateRiskAfter Failed", vApp:=ACApp, vClass:=ACClass,
                                              vMethod:="UpdateRiskAfter", vErrNo:=Informations.Err().Number,
                                              vErrDesc:=excep.Message)
            Return nResult
        End Try
    End Function

    Public Function Recalculate(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal sTransactionType As String, Optional ByVal v_bViaSAM As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim oBusiness As Object ' bSirAgentCommission.Business
        Dim lTransactionType As Integer
        Dim i As Integer
        Dim bApplyReinsurance, bApplyTaxes As Boolean
        Dim vTaxesArray As Object
        Dim sTaxesDescription As String = ""
        Dim bDisplayScreen As Object
        Dim vCommissionArray As Object
        Dim bTaxesSwithedOff, bIsRIValid As Boolean
        Dim lRIValid, lRIBand As Integer

        result = gPMConstants.PMEReturnCode.PMTrue
        'We need three business object here
        Dim oSIRPartyFee As bSIRPartyFee.UBusiness
        Dim oSIRRITax As bSIRRITax.Business
        Dim oSIRListRisks As Object = Nothing ' bSIRListRisks.Business

        ' Create the oSIRPartyFee.Business object
        oSIRPartyFee = New bSIRPartyFee.UBusiness
        Dim lReturn As Integer = oSIRPartyFee.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to Initialise bSIRPartyFee.UBusiness object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        m_lReturn = oSIRPartyFee.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to SetProcessModes for bSIRPartyFee.UBusiness object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Create the bSIRRITax.Business object
        oSIRRITax = New bSIRRITax.Business
        lReturn = oSIRRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to Initialise bSIRRITax.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        m_lReturn = oSIRRITax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to SetProcessModes for bSIRRITax.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Create the bSIRListRisks.Business object
        'oSIRListRisks = New bSIRListRisks.Business
        lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSIRListRisks, v_sClassName:="bSIRListRisks.Business", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed - Failed to create business object to bSIRInsuranceFile.Services.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrenciesByBranch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        Dim oDatabase As Object = m_oDatabase
        lReturn = oSIRListRisks.Initialise(sUsername:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to Initialise bSIRListRisks.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_oDatabase = oDatabase

        m_lReturn = oSIRListRisks.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=gPMFunctions.ToSafeString(sTransactionType), vEffectiveDate:=DateTime.Now)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to SetProcessModes for bSIRListRisks.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Recalculate the fees and taxes
        lReturn = oSIRPartyFee.RecalculateRiskFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=v_lRiskCnt, v_lTransactionTypeId:=lTransactionType, v_bUseExistingFeeDetail:=False)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRPartyFee.UBusiness.RecalculateRiskFees method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        oSIRRITax.InsuranceFileCnt = v_lInsuranceFileCnt
        ' Recalculate the fees and taxes
        lReturn = oSIRRITax.RecalculatePolicyRiskTaxes(v_lRiskCnt:=v_lRiskCnt, v_lTask:=gPMConstants.PMEComponentAction.PMEdit, v_sTransactionType:=sTransactionType)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRITax.Business.RecalculateRisk method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Recalculate the Policy taxes
        lReturn = oSIRListRisks.UpdatePolicyPremium(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRListRisks.Business.UpdatePolicyPremium method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        ' Recalculate the Policy taxes
        lReturn = oSIRRITax.RecalculatePolicyTaxes(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lTask:=gPMConstants.PMEComponentAction.PMEdit, v_sTransactionType:=sTransactionType)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRListRisks.Business.RecalculatePolicyTaxes method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        If v_bViaSAM Then
            lReturn = oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=-1, v_lTransactionTypeId:=lTransactionType, v_bUseExistingFeeDetail:=False, nViaSam:=1)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRPartyFee.UBusiness.RecalculatePolicyFees method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        Else
            lReturn = oSIRPartyFee.RecalculatePolicyFees(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lProductId:=-1, v_lTransactionTypeId:=lTransactionType, v_bUseExistingFeeDetail:=False)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRPartyFee.UBusiness.RecalculatePolicyFees method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If
        ' ecalculate the Policy taxes

        oSIRPartyFee.Dispose()
        oSIRPartyFee = Nothing

        oSIRListRisks.Dispose()
        oSIRListRisks = Nothing

        oSIRRITax.Dispose()
        oSIRRITax = Nothing
        lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oBusiness,
                                                                v_sClassName:="bSirAgentCommission.Business",
                                                                v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername),
                                                                v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID),
                                                                v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                                                v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID),
                                                                v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                                                v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)
        oDatabase = m_oDatabase
        lReturn = oBusiness.Initialise(sUsername:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to Initialise bSirAgentCommission.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_oDatabase = oDatabase

        m_lReturn = oBusiness.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vNavigate:=0, vProcessMode:=110, vTransactionType:=gPMFunctions.ToSafeString(sTransactionType), vEffectiveDate:=DateTime.Now)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Recalculate Failed to SetProcessModes for bSirAgentCommission.Business object.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'Set the Business Object Properties
        oBusiness.InsuranceFileCnt = v_lInsuranceFileCnt

        lReturn = oBusiness.CheckDisplayCommission(r_bDisplayScreen:=bDisplayScreen)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.CheckDisplayCommission method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If Not bDisplayScreen Then

        Else
            lReturn = oBusiness.CalculateAgentCommission(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_sTransactionType:=gPMFunctions.ToSafeString(sTransactionType), r_vntResult:=vCommissionArray)
            '            End If
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.CalculateAgentCommission method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Informations.IsArray(vCommissionArray) Then
                'Update the lead commission records for the insurancefile

                lReturn = oBusiness.UpdateLeadCommission(gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = lReturn
                    ' Log Error Message
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSirAgentCommission.Business.UpdateLeadCommission method failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="Recalculate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                'lReturn& = AddToArray(m_susername$, r_vAdditionalDataArray, CNAgentCommArray, vCommissionArray)
            End If

        End If

        oBusiness.Dispose()
        oBusiness = Nothing

        Return result
    End Function

    Private Function GetShortPeriodRate(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Dim oBusiness As bsirshortperiodrate.Business


        Catch_Renamed = True

        result = gPMConstants.PMEReturnCode.PMTrue
        oBusiness = New bsirshortperiodrate.Business
        m_lReturn = CType(oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(ObjectError.ToString() + ", GetShortPeriodRate, Unable to create business object 'bSIRShortPeriodRate.Business'")
        End If
        ' Get the rate based on new policy details.
        m_lReturn = oBusiness.GetShortPeriodRate(v_lProductID:=gPMFunctions.ToSafeInteger(v_lProductID), v_sType:=If(m_sTransactionType = "MTC", "C", "P"), v_dtStartDate:=v_dtStartDate, v_dtEndDate:=v_dtEndDate, v_dtTransactDate:=v_dtStartDate, r_dRefundRate:=r_dProRataRate)
        Select Case m_lReturn
            Case gPMConstants.PMEReturnCode.PMTrue, gPMConstants.PMEReturnCode.PMNotFound
                result = m_lReturn
            Case Else
                Throw New System.Exception(ObjectError.ToString() + ", GetShortPeriodRate, Unable to retrieve short period rates")
        End Select
        ' Terminate the business object.
        oBusiness.Dispose()
        oBusiness = Nothing
        Return result

    End Function

    Public Function GetProRataRate(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double) As Integer
        Return GetProRataRate(v_lProductID:=v_lProductID, v_dtOldStartDate:=v_dtOldStartDate, v_dtOldEndDate:=v_dtOldEndDate, v_dtStartDate:=v_dtStartDate, v_dtEndDate:=v_dtEndDate, r_dProRataRate:=r_dProRataRate, v_dtInceptionDate:=#12/30/1899#)
    End Function

    Public Function GetProRataRate(ByVal v_lProductID As Integer, ByVal v_dtOldStartDate As Date, ByVal v_dtOldEndDate As Date, ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByRef r_dProRataRate As Double, ByRef v_dtInceptionDate As Date) As Integer
        Dim result As Integer = 0
        Dim lBaseLength, lPeriodLength As Integer
        Dim b_IsTrueMonthlyPolicy As Boolean
        Dim vResultArray(,) As Object
        Dim lMonthCount As Integer
        Dim lTotalDaysInMth, lPolicyDays As Integer
        Dim dtDate, dtStartDate, dtEndDate, dtTmpDate, dtTmpDate1, dtInceptionDate As Date
        Dim dProRataRate, dProRataRateFractionVal As Double

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check whether its a TMP
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("Prod_id", CStr(v_lProductID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTMPStatusSQL, sSQLName:=ACGetTMPStatusName, bStoredProcedure:=False, vResultArray:=vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                b_IsTrueMonthlyPolicy = If(CDbl(vResultArray(0, 0)) = 1, 1, 0)

                If b_IsTrueMonthlyPolicy Then

                    r_dProRataRate = 0
                    lMonthCount = Informations.DateDiff("m", v_dtStartDate, v_dtEndDate, gPMConstants.FirstDayOfWeek.Sunday, gPMConstants.FirstWeekOfYear.FirstJan1)

                    If (v_dtEndDate.Day) < (v_dtStartDate.Day) Then
                        lMonthCount -= 1
                    End If

                    If lMonthCount = 0 Then
                        dtDate = v_dtStartDate
                    Else
                        dtDate = v_dtStartDate.AddMonths(lMonthCount)
                    End If

                    lPolicyDays = Informations.DateDiff("d", dtDate, v_dtEndDate, gPMConstants.FirstDayOfWeek.Sunday, gPMConstants.FirstWeekOfYear.FirstJan1) + m_lIsMidnightRenewal

                    If lPolicyDays = -1 Then 'its a complete month
                        lPolicyDays = 0
                    End If

                    Select Case dtDate.Month 'Identify total days in a month
                        Case 1, 3, 5, 7, 8, 10, 12
                            lTotalDaysInMth = 31
                        Case 4, 6, 9, 11
                            lTotalDaysInMth = 30
                        Case 2
                            If dtDate.Year Mod 4 = 0 Then
                                lTotalDaysInMth = 29
                            Else
                                lTotalDaysInMth = 28
                            End If
                    End Select
                    ' add full for total months in period plus apply prorata on remaining days
                    r_dProRataRate = lMonthCount + lPolicyDays / lTotalDaysInMth
                Else
                    dtStartDate = v_dtStartDate
                    dtInceptionDate = v_dtInceptionDate
                    dtEndDate = v_dtEndDate

                    dtTmpDate = v_dtInceptionDate

                    If m_lIsMidnightRenewal = 0 Then
                        dtEndDate = v_dtEndDate.AddDays(-1)
                    End If

                    lBaseLength = 365

                    If ((dtInceptionDate.Month) = 2 And (dtInceptionDate.Day) = 29) Or ((dtEndDate.Month) = 2 And (dtEndDate.Day) = 29) Then
                        lBaseLength = 366
                    Else
                        Do While CDate(dtTmpDate) < CDate(dtEndDate)
                            If ((dtTmpDate.Day) = 1 And (dtTmpDate.Month) <> 2) Or ((dtTmpDate.Year) Mod 4 <> 0) Then
                                dtTmpDate = Informations.DateAdd("m", 1, dtTmpDate)
                            Else
                                dtTmpDate = Informations.DateAdd("d", 1, dtTmpDate)
                            End If

                            If (dtTmpDate.Month) = 2 And (dtTmpDate.Day) = 29 Then
                                lBaseLength = 366
                                Exit Do
                            End If
                        Loop
                    End If

                    lPeriodLength = Informations.DateDiff("d", dtStartDate, dtEndDate) + 1
                    dProRataRate = lPeriodLength / lBaseLength

                    r_dProRataRate = dProRataRate

                End If
            End If
            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProRataRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProRataRate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function GetRoundingInfo(ByVal v_lProductID As Integer, ByRef r_iRoundPremium As Integer, ByRef r_lRoundingSectionID As Integer, ByRef r_sRoundingSectionCode As String) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_lProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRoundingInfoSQL, sSQLName:=ACGetRoundingInfoName, bStoredProcedure:=ACGetRoundingInfoStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'store rounding flag
        Dim auxVar As Object = vResultArray(0, 0)

        If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
            r_iRoundPremium = 0
        Else

            r_iRoundPremium = CInt(vResultArray(0, 0))
        End If

        'store rounding rating section ID
        Dim auxVar_2 As Object = vResultArray(1, 0)

        If Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2) Then
            r_lRoundingSectionID = 0
        Else

            r_lRoundingSectionID = CInt(vResultArray(1, 0))
        End If

        'store rounding rating section code??
        Dim auxVar_3 As Object = vResultArray(2, 0)

        If Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3) Then
            r_sRoundingSectionCode = ""
        Else

            r_sRoundingSectionCode = CStr(vResultArray(2, 0))
        End If

        Return result

    End Function
    Private Function GetProRataFlag(ByVal v_lInsuranceFileCnt As Integer, ByRef r_iNBProrata As Integer, ByRef r_iMTAProrata As Integer, Optional ByRef r_iShortPeriodRate As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProrataFlagSQL, sSQLName:=ACGetProrataFlagName, bStoredProcedure:=ACGetProrataFlagStored, vResultArray:=vResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        'store new business prorata flag
        Dim auxVar As Object = vResultArray(0, 0)

        If Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar) Then
            r_iNBProrata = 0
        Else

            r_iNBProrata = CInt(vResultArray(0, 0))
        End If

        'store MTA prorata flag
        Dim auxVar_2 As Object = vResultArray(1, 0)

        If Convert.IsDBNull(auxVar_2) Or Informations.IsNothing(auxVar_2) Then
            r_iMTAProrata = 0
        Else

            r_iMTAProrata = CInt(vResultArray(1, 0))
        End If

        'store short period rated flag
        Dim auxVar_3 As Object = vResultArray(2, 0)

        If Convert.IsDBNull(auxVar_3) Or Informations.IsNothing(auxVar_3) Then
            r_iShortPeriodRate = 0
        Else

            r_iShortPeriodRate = CInt(vResultArray(2, 0))
        End If

        Return result

    End Function

    Public Function DeleteExistingReinsurance(ByVal v_lRiskCnt As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            AddDatabaseParameter("risk_cnt", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteExistingReinsuranceSQL, sSQLName:=ACDeleteExistingReinsuranceName, bStoredProcedure:=ACDeleteExistingReinsuranceStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update last message count received", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteExistingReinsurance")
                Return result
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteExistingReinsurance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteExistingReinsurance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetUWProductOptions(ByVal v_lProductID As Integer) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()
            AddDatabaseParameter("product_id", v_lProductID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute call
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectUWProductOptionsSQL, sSQLName:=ACSelectUWProductOptionsName, bStoredProcedure:=ACSelectUWProductOptionsStored, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords, bKeepNulls:=True)

            ' Check results
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Store options
            m_lIsMidnightRenewal = gPMFunctions.NullToLong(vResultArray(0, 0))
            m_lAllowPositiveCancellation = gPMFunctions.NullToLong(vResultArray(1, 0))
            m_lUnifiedRenewalDay = gPMFunctions.NullToLong(vResultArray(2, 0))

            Return result
        Catch excep As System.Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUWProductOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUWProductOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function UpdateRisk(ByRef v_lRiskCnt As Integer, ByRef v_dProRataRate As Double) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' 01/08/2003 Peter Finney - Add actual proratarate to the risk
            ' Note: At the moment it appears this value is always used. We
            ' may need to tighten up on it later.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pro_rata_rate", vValue:=CStr(v_dProRataRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskSQL, sSQLName:=ACUpdateRiskName, bStoredProcedure:=ACUpdateRiskStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Function GetRatingInfo(ByVal v_lGISPolicyLinkID As Integer, ByVal v_dEffectiveDate As Date, ByRef r_vRatingArray(,) As Object,
                                        Optional ByVal v_lQuoteType As Integer = 0, Optional ByVal v_sTransType As String = "RT") As Integer


        GetRatingInfo = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(
                sName:="gis_policy_link_id",
                vValue:=v_lGISPolicyLinkID,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return m_lReturn
        End If

        m_lReturn = m_oDatabase.Parameters.Add(
                sName:="effective_date",
                vValue:=v_dEffectiveDate,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMDate)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return m_lReturn
        End If

        m_lReturn = m_oDatabase.Parameters.Add(
                sName:="quote_type",
                vValue:=v_lQuoteType,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return m_lReturn
        End If
        m_lReturn = m_oDatabase.Parameters.Add(
               sName:="type",
               vValue:=v_sTransType,
               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
               iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return m_lReturn
        End If
        m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACGetRuleFileSQL,
                sSQLName:=ACGetRuleFileName,
                bStoredProcedure:=ACGetRuleFileStored,
                vResultArray:=r_vRatingArray)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return m_lReturn
        End If

        Exit Function

    End Function

    Public Function RunPostPaymentProcessingRule(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer, ByVal sPaymentReference As String, ByVal sPolicyNumber As String) As Integer
        Return RunPostPaymentProcessingRule(iInsuranceFileCnt:=iInsuranceFileCnt, iInsuranceFolderCnt:=iInsuranceFolderCnt, iPartyCnt:=iPartyCnt, sPaymentReference:=sPaymentReference, sPolicyNumber:=sPolicyNumber, sTaskUserGroup:="SYSADMIN")
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="iInsuranceFileCnt"></param>
    ''' <param name="iInsuranceFolderCnt"></param>
    ''' <param name="iPartyCnt"></param>
    ''' <param name="sPaymentReference"></param>
    ''' <param name="sPolicyNumber"></param>
    ''' <param name="sTaskUserGroup"></param>
    ''' <returns></returns>
    Public Function RunPostPaymentProcessingRule(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer, ByVal sPaymentReference As String, ByVal sPolicyNumber As String, ByVal sTaskUserGroup As String) As Integer
        Dim sOptionValue As String
        m_lReturn = GetSystemOption(nOptionNumber:=GeneralConst.kSystemOptionRuleTypePaymentGateway, r_sSystemOptionValue:=sOptionValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sOptionValue = "1" Then
            ExecuteRulFileForPostPaymentProcessing(iInsuranceFileCnt:=iInsuranceFileCnt, iInsuranceFolderCnt:=iInsuranceFolderCnt, iPartyCnt:=iPartyCnt, sPaymentReference:=sPaymentReference, sPolicyNumber:=sPolicyNumber, sTaskUserGroup:="SYSADMIN")
        Else
            ExecuteCompiledRuleForPostPaymentProcessing(iInsuranceFileCnt:=iInsuranceFileCnt, iInsuranceFolderCnt:=iInsuranceFolderCnt, iPartyCnt:=iPartyCnt, sPaymentReference:=sPaymentReference, sPolicyNumber:=sPolicyNumber, sTaskUserGroup:="SYSADMIN")
        End If
        Return m_lReturn
    End Function

    ''' <summary>
    ''' Execute rule file
    ''' </summary>
    ''' <param name="iInsuranceFileCnt"></param>
    ''' <param name="iInsuranceFolderCnt"></param>
    ''' <param name="iPartyCnt"></param>
    ''' <param name="sPaymentReference"></param>
    ''' <param name="sPolicyNumber"></param>
    ''' <param name="sTaskUserGroup"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <HandleProcessCorruptedStateExceptions>
    Private Function ExecuteRulFileForPostPaymentProcessing(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer, ByVal sPaymentReference As String, ByVal sPolicyNumber As String, ByVal sTaskUserGroup As String) As Integer
        Dim oScriptControl As MSScriptControl.ScriptControl
        Dim sScript As String
        Dim oSharedStorage As New SharedStorage

        Dim oExtras As Object

        Try
            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oExtras, v_sClassName:="bGISPMUExtras.Business", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oExtras.SetProcessModes(vTask:=gPMFunctions.ToSafeInteger(m_iTask), vNavigate:=gPMFunctions.ToSafeInteger(m_lNavigate), vProcessMode:=gPMFunctions.ToSafeInteger(m_lProcessMode), vTransactionType:=gPMFunctions.ToSafeString(m_sTransactionType), vEffectiveDate:=gPMFunctions.ToSafeDate(m_dtEffectiveDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oExtras.InsuranceFileCnt = iInsuranceFileCnt
            oExtras.InsuranceFolderCnt = iInsuranceFolderCnt
            oExtras.PartyCnt = iPartyCnt

            oExtras.TaskUserGroup = sTaskUserGroup

            oScriptControl = New MSScriptControl.ScriptControl()

            oSharedStorage.InsuranceFileCnt = iInsuranceFileCnt
            oSharedStorage.PolicyNumber = sPolicyNumber
            oSharedStorage.PaymentReference = sPaymentReference

            oScriptControl.Language = "VBScript"
            oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)
            oScriptControl.AddObject("Extras", oExtras, False)

            ' Read in the script and run it
            m_lReturn = CType(GetScriptFile(v_sScript:=sScript), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oScriptControl.AddCode(sScript)
            oScriptControl.Run("main")
            If oSharedStorage.Status = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Paymentgateway.rul file failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunPaymentProcessingRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMError

        Finally
            oExtras.Dispose()
            oExtras = Nothing
            oScriptControl = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Execute compiled rules
    ''' </summary>
    ''' <param name="iInsuranceFileCnt"></param>
    ''' <param name="iInsuranceFolderCnt"></param>
    ''' <param name="iPartyCnt"></param>
    ''' <param name="sPaymentReference"></param>
    ''' <param name="sPolicyNumber"></param>
    ''' <param name="sTaskUserGroup"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteCompiledRuleForPostPaymentProcessing(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer, ByVal sPaymentReference As String, ByVal sPolicyNumber As String, ByVal sTaskUserGroup As String) As Integer
        Dim oSharedStorage As New SharedStorage
        Dim sAssemblyClassName As String = String.Empty
        Dim oExtras As Object
        Dim oRules As Object

        Try
            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oExtras, v_sClassName:="bGISPMUExtras.Business", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oExtras.SetProcessModes(vTask:=gPMFunctions.ToSafeInteger(m_iTask), vNavigate:=gPMFunctions.ToSafeInteger(m_lNavigate), vProcessMode:=gPMFunctions.ToSafeInteger(m_lProcessMode), vTransactionType:=gPMFunctions.ToSafeString(m_sTransactionType), vEffectiveDate:=gPMFunctions.ToSafeDate(m_dtEffectiveDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oExtras.InsuranceFileCnt = iInsuranceFileCnt
            oExtras.InsuranceFolderCnt = iInsuranceFolderCnt
            oExtras.PartyCnt = iPartyCnt
            oExtras.TaskUserGroup = sTaskUserGroup

            oSharedStorage.InsuranceFileCnt = iInsuranceFileCnt
            oSharedStorage.PolicyNumber = sPolicyNumber
            oSharedStorage.PaymentReference = sPaymentReference

            ' Read in the script and run it
            m_lReturn = CType(GetCompiledRuleFile(v_sScript:=sAssemblyClassName), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sAssemblyClassName = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)
            If Not (oRules Is Nothing) Then
                oRules.oSharedStorage = oSharedStorage
                oRules.Extras = oExtras
                oRules.main()

                If oRules.oSharedStorage.Status = 0 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sAssemblyClassName & " file failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunPaymentProcessingRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMError

        Finally
            oExtras.Dispose()
            oExtras = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Get compiled rule file name from system options
    ''' </summary>
    ''' <param name="v_sScript"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetCompiledRuleFile(ByRef v_sScript As String) As Integer

        Dim nResult As Integer = GetSystemOption(nOptionNumber:=GeneralConst.kSystemOptionCompiledRulePaymentGateway,
                                  r_sSystemOptionValue:=v_sScript)

        Return nResult

    End Function

    ''' <summary>
    ''' Run payment gateway rule file/compiled rules based on system options
    ''' </summary>
    ''' <param name="iInsuranceFileCnt"></param>
    ''' <param name="iInsuranceFolderCnt"></param>
    ''' <param name="iPartyCnt"></param>
    ''' <param name="sPaymentReference"></param>
    ''' <param name="dAmount"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <returns></returns>
    Public Function RunPaymentProcessingRule(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer, ByVal sPaymentReference As String, ByVal dAmount As Double, ByVal sCurrencyCode As String) As Integer
        Dim sOptionValue As String
        m_lReturn = GetSystemOption(nOptionNumber:=GeneralConst.kSystemOptionRuleTypePaymentGateway, r_sSystemOptionValue:=sOptionValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sOptionValue = "1" Then
            Return ExecuteRulFileForPaymentProcessing(iInsuranceFileCnt:=iInsuranceFileCnt, iInsuranceFolderCnt:=iInsuranceFolderCnt, iPartyCnt:=iPartyCnt, sPaymentReference:=sPaymentReference, dAmount:=dAmount, sCurrencyCode:=sCurrencyCode)
        Else
            Return ExecuteCompiledRuleForPaymentProcessing(iInsuranceFileCnt:=iInsuranceFileCnt, iInsuranceFolderCnt:=iInsuranceFolderCnt, iPartyCnt:=iPartyCnt, sPaymentReference:=sPaymentReference, dAmount:=dAmount, sCurrencyCode:=sCurrencyCode)
        End If
    End Function

    ''' <summary>
    ''' Execute rule file
    ''' </summary>
    ''' <param name="iInsuranceFileCnt"></param>
    ''' <param name="iInsuranceFolderCnt"></param>
    ''' <param name="iPartyCnt"></param>
    ''' <param name="sPaymentReference"></param>
    ''' <param name="dAmount"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <HandleProcessCorruptedStateExceptions>
    Private Function ExecuteRulFileForPaymentProcessing(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer, ByVal sPaymentReference As String, ByVal dAmount As Double, ByVal sCurrencyCode As String) As Integer
        Dim oScriptControl As MSScriptControl.ScriptControl
        Dim sScript As String
        Dim oSharedStorage As New SharedStorage
        Dim oExtras As Object

        Try
            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oExtras, v_sClassName:="bGISPMUExtras.Business", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oExtras.SetProcessModes(vTask:=gPMFunctions.ToSafeInteger(m_iTask), vNavigate:=gPMFunctions.ToSafeInteger(m_lNavigate), vProcessMode:=gPMFunctions.ToSafeInteger(m_lProcessMode), vTransactionType:=gPMFunctions.ToSafeString(m_sTransactionType), vEffectiveDate:=gPMFunctions.ToSafeDate(m_dtEffectiveDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oExtras.InsuranceFileCnt = iInsuranceFileCnt
            oExtras.InsuranceFolderCnt = iInsuranceFolderCnt
            oExtras.PartyCnt = iPartyCnt

            oScriptControl = New MSScriptControl.ScriptControl()


            oSharedStorage.InsuranceFileCnt = iInsuranceFileCnt
            oSharedStorage.PaymentReference = sPaymentReference
            oSharedStorage.Amount = dAmount
            oSharedStorage.Currency = sCurrencyCode.Trim()


            oScriptControl.Language = "VBScript"
            oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)
            oScriptControl.AddObject("Extras", oExtras, False)
            ' Read in the script and run it
            m_lReturn = CType(GetScriptFile(v_sScript:=sScript), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oScriptControl.AddCode(sScript)
            oScriptControl.Run("start")
            If oSharedStorage.Status = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Paymentgateway.rul file failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception

            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunPaymentProcessingRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMError

        Finally
            oExtras.Dispose()
            oExtras = Nothing
            oScriptControl = Nothing
        End Try
    End Function

    ''' <summary>
    ''' Execute compiled rule file
    ''' </summary>
    ''' <param name="iInsuranceFileCnt"></param>
    ''' <param name="iInsuranceFolderCnt"></param>
    ''' <param name="iPartyCnt"></param>
    ''' <param name="sPaymentReference"></param>
    ''' <param name="dAmount"></param>
    ''' <param name="sCurrencyCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteCompiledRuleForPaymentProcessing(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer, ByVal iPartyCnt As Integer,
                                                             ByVal sPaymentReference As String, ByVal dAmount As Double, ByVal sCurrencyCode As String) As Integer
        Dim oSharedStorage As New SharedStorage
        Dim oExtras As Object
        Dim sAssemblyClassName As String = String.Empty
        Dim oRules As Object

        Try
            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oExtras, v_sClassName:="bGISPMUExtras.Business", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oExtras.SetProcessModes(vTask:=gPMFunctions.ToSafeInteger(m_iTask), vNavigate:=gPMFunctions.ToSafeInteger(m_lNavigate), vProcessMode:=gPMFunctions.ToSafeInteger(m_lProcessMode), vTransactionType:=gPMFunctions.ToSafeString(m_sTransactionType), vEffectiveDate:=gPMFunctions.ToSafeDate(m_dtEffectiveDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oExtras.InsuranceFileCnt = iInsuranceFileCnt
            oExtras.InsuranceFolderCnt = iInsuranceFolderCnt
            oExtras.PartyCnt = iPartyCnt

            oSharedStorage.InsuranceFileCnt = iInsuranceFileCnt
            oSharedStorage.PaymentReference = sPaymentReference
            oSharedStorage.Amount = dAmount
            oSharedStorage.Currency = sCurrencyCode.Trim()

            ' Read in the script and run it
            m_lReturn = CType(GetCompiledRuleFile(v_sScript:=sAssemblyClassName), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sAssemblyClassName = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)
            If Not (oRules Is Nothing) Then
                oRules.oSharedStorage = oSharedStorage
                oRules.Extras = oExtras
                oRules.start()

                If oRules.oSharedStorage.Status = 0 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sAssemblyClassName & " file failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunPaymentProcessingRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunPaymentProcessingRule", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMError

        Finally
            oExtras.Dispose()
            oExtras = Nothing
        End Try
    End Function


    '*********************************************************************
    ' Name: GetScriptFile
    '
    ' Description : Find and read the VBScript file
    '
    ' Created: 01/03/2013
    '*********************************************************************
    Private Function GetScriptFile(ByRef v_sScript As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sFullPath As String = ""
        Dim iFile As Integer
        Dim lFileLength As Integer
        Dim sPathName As String = ""
        Dim lFileNumber As gPMConstants.PMEReturnCode
        Dim sStr, sStr2 As String

        ' Get the path to the validation script from the registry
        lFileNumber = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", v_sSubKey:="GIS", r_sSettingValue:=sPathName), gPMConstants.PMEReturnCode)

        ' Build the path to the script file
        sPathName = sPathName.Trim()
        If Not sPathName.EndsWith("\") And Not sPathName.EndsWith(":") Then
            sPathName = sPathName & "\"
        End If
        sFullPath = sPathName & "Payment_gateway.rul"

        ' Ensure the file exists
        If FileSystem.Dir(sFullPath, FileAttribute.Normal) = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Payment_gateway.rul file not found", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScriptFile")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Open the VBscript file
        iFile = FileSystem.FreeFile()
        FileSystem.FileOpen(iFile, sFullPath, OpenMode.Input)
        lFileLength = FileSystem.LOF(iFile)

        ' Read the script into the string variable
        sStr2 = FileSystem.InputString(iFile, lFileLength)

        FileSystem.FileClose(iFile)

        ' Add the option explicit in case it's missing
        sStr = "Option Explicit" & Strings.Chrw(13) & Strings.Chrw(10)

        sStr = sStr & sStr2 & Strings.Chrw(13) & Strings.Chrw(10)

        ' Return the script
        v_sScript = sStr.Trim()
        Return nResult

    End Function
    ''' <summary>
    ''' Get System option
    ''' </summary>
    ''' <param name="nOptionNumber"></param>
    ''' <param name="r_sSystemOptionValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSystemOption(ByVal nOptionNumber As Integer, ByRef r_sSystemOptionValue As String) As Integer
        Dim nReturn As Integer = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=nOptionNumber, r_sOptionValue:=r_sSystemOptionValue)
        Return nReturn
    End Function


    ''' <summary>
    ''' Extract Policy , Claim , Party and related data and return a byte array.
    ''' </summary>
    ''' <param name="nClientID"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="r_oByteArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExtractData(ByVal nClientID As Integer, ByVal sPassword As String, ByRef r_oByteArray() As Byte) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oSpreadSheetDocument As SpreadsheetDocument
        Dim oWorkbookPart As WorkbookPart
        Dim oWorkSheetPart As WorksheetPart
        Dim oSheet As Sheet
        Dim oSheets As Sheets
        Dim oSharedStringTablePart As SharedStringTablePart
        Dim sGUID As String = System.Guid.NewGuid.ToString()
        Dim bSaveNCloseInFinally As Boolean = True
        Dim sTempPath As String = Path.GetTempPath
        Dim sDescription As String = String.Empty
        Dim dtDate As DateTime = Date.MinValue
        Dim sExcelName As String

        Try
            dtDate = DateTime.ParseExact(DateTime.Now.ToString("dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture), "dd/MM/yyyy H:mm:ss", CultureInfo.InvariantCulture)

            nResult = GetLookupDescription(99, "Party_R", nClientID, sDescription, False) 'Get the shortname
            sExcelName = sDescription.Trim().ToUpper() + "_" + dtDate.ToString("ddMMyyyy") + "_" + dtDate.ToString("hhmm")

            oSpreadSheetDocument = SpreadsheetDocument.Create(sTempPath + sExcelName + ".xlsx", SpreadsheetDocumentType.Workbook)
            oWorkbookPart = oSpreadSheetDocument.AddWorkbookPart()
            oWorkbookPart.Workbook = New Workbook()
            oSpreadSheetDocument.WorkbookPart.Workbook.Save()

            oWorkSheetPart = oWorkbookPart.AddNewPart(Of WorksheetPart)()
            oWorkSheetPart.Worksheet = New Worksheet(New SheetData())
            oSpreadSheetDocument.WorkbookPart.Workbook.Save()

            oSpreadSheetDocument.WorkbookPart.Workbook.Sheets = New Sheets()
            oSpreadSheetDocument.WorkbookPart.Workbook.Save()

            oSpreadSheetDocument.WorkbookPart.Workbook.Save()

            nResult = ExtractPartyData(nClientID, oSpreadSheetDocument)
            oSpreadSheetDocument.WorkbookPart.Workbook.Save()

            nResult = ExtractPolicyData(nClientID, oSpreadSheetDocument)
            oSpreadSheetDocument.WorkbookPart.Workbook.Save()

            nResult = ExtractClaimData(nClientID, oSpreadSheetDocument)
            oSpreadSheetDocument.WorkbookPart.Workbook.Save()
            oSpreadSheetDocument.Dispose() '.Close()


            Dim zip As ZipFile = New ZipFile()
            zip.Password = sPassword
            zip.AddFile(sTempPath + sExcelName + ".xlsx", "\")
            zip.Save(sTempPath + nClientID.ToString() + "_" + sGUID.ToString() + ".zip")


            bSaveNCloseInFinally = False

            Using fsInputFile As FileStream = File.OpenRead(sTempPath + nClientID.ToString() + "_" + sGUID.ToString() + ".zip")
                ReDim r_oByteArray(CInt(fsInputFile.Length - 1))
                fsInputFile.Read(r_oByteArray, 0, CInt(fsInputFile.Length))
                fsInputFile.Close()
            End Using

            File.Delete(sTempPath + sExcelName + ".xlsx")
            File.Delete(sTempPath + nClientID.ToString() + "_" + sGUID.ToString() + ".zip")

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            Throw
            Return PMEReturnCode.PMFalse
        Finally
            If bSaveNCloseInFinally Then
                oSpreadSheetDocument.Dispose()
                File.Delete(sTempPath + sExcelName + ".xlsx")
                File.Delete(sTempPath + nClientID.ToString() + "_" + sGUID.ToString() + ".zip")
            End If

        End Try



    End Function

    ''' <summary>
    ''' Extract the Party, related data and add it to an excel workbook object
    ''' </summary>
    ''' <param name="nClientID"></param>
    ''' <param name="r_oExcelWorkbook"></param>
    ''' <param name="r_oExcelSheet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExtractPartyData(ByVal nClientID As Integer, ByRef r_oSpreadSheetDocument As SpreadsheetDocument) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sXMLDataSet As String = String.Empty
        Dim kInsuranceFileCnt As Integer = 2
        Dim kInsuranceRef As Integer = 3
        Dim kInsuranceFolderCnt As Integer = 12
        Dim kRiskCnt As Integer = 1
        Dim kRiskDescription As Integer = 3
        Dim sDataModelCode As String = String.Empty
        Dim nExcelSheetRow As Integer = 1
        Dim nExcelSheetColumn As Integer = 1
        Dim obGISRenewals As bGIS.Renewals
        Dim sDescription As String = String.Empty
        Dim dtPolicyData As System.Data.DataTable
        Dim dsPolicyData As System.Data.DataSet


        Try

            nExcelSheetRow = 1
            nExcelSheetColumn = 1

            Dim sSheetName As String = "Party"
            AddWorksheet(r_oSpreadSheetDocument, sSheetName)
            Dim relId As String = r_oSpreadSheetDocument.WorkbookPart.Workbook.Descendants(Of Sheet)().First(Function(s) sSheetName.Equals(s.Name)).Id
            Dim oWorkSheetPart As WorksheetPart = CType(r_oSpreadSheetDocument.WorkbookPart.GetPartById(relId), WorksheetPart)


            nResult = GetPartyDetails(nClientID, dsPolicyData)

            If dsPolicyData IsNot Nothing And dsPolicyData.Tables.Count > 0 Then

                'Party from the select statement in the SP
                If dsPolicyData.Tables(0) IsNot Nothing AndAlso dsPolicyData.Tables(0).Rows IsNot Nothing AndAlso dsPolicyData.Tables(0).Rows.Count > 0 Then

                    For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(0).Columns
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dsPolicyData.Tables(0).Rows(0)(oDataColumn.ColumnName).ToString())
                        nExcelSheetColumn += 1
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Data Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If


                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3

                'Party Account Details
                If dsPolicyData.Tables(1) IsNot Nothing AndAlso dsPolicyData.Tables(1).Rows IsNot Nothing AndAlso dsPolicyData.Tables(1).Rows.Count > 0 Then

                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Year To Date Turnover")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dsPolicyData.Tables(1).Rows(0)(0).ToString())
                    nExcelSheetColumn += 1

                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Last Year Turnover")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dsPolicyData.Tables(1).Rows(0)(1).ToString())
                    nExcelSheetColumn += 1

                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Account Balance")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dsPolicyData.Tables(1).Rows(0)(2).ToString())

                    nExcelSheetColumn += 1
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Account Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If

                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3


                'Party Address Details
                If dsPolicyData.Tables(2) IsNot Nothing AndAlso dsPolicyData.Tables(2).Rows IsNot Nothing AndAlso dsPolicyData.Tables(2).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(2).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(2).Columns
                            If oDataColumn.ColumnName.ToLower = "address_usage_type_id" Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "address_usage_type")
                                nResult = GetLookupDescription(2, "address_usage_type", oDataRow("address_usage_type_id").ToString(), sDescription, False)
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)

                                nExcelSheetColumn += 1
                            Else
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())

                                nExcelSheetColumn += 1
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Address Detail Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3


                'Party Contact Details
                If dsPolicyData.Tables(3) IsNot Nothing AndAlso dsPolicyData.Tables(3).Rows IsNot Nothing AndAlso dsPolicyData.Tables(3).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(3).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(3).Columns
                            If oDataColumn.Ordinal = 5 OrElse
                                oDataColumn.Ordinal = 6 OrElse
                                oDataColumn.Ordinal = 7 OrElse
                                oDataColumn.Ordinal = 8 OrElse
                                oDataColumn.Ordinal = 14 Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())
                                nExcelSheetColumn += 1
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Contact Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3

                'Party Associate Details
                If dsPolicyData.Tables(4) IsNot Nothing AndAlso dsPolicyData.Tables(4).Rows IsNot Nothing AndAlso dsPolicyData.Tables(4).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(4).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(4).Columns
                            If oDataColumn.Ordinal = 3 OrElse
                                oDataColumn.Ordinal = 4 OrElse
                                oDataColumn.Ordinal = 5 OrElse
                                oDataColumn.Ordinal = 6 Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())
                                nExcelSheetColumn += 1
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Associate Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3



                'party conviction details
                If dsPolicyData.Tables(5) IsNot Nothing AndAlso dsPolicyData.Tables(5).Rows IsNot Nothing AndAlso dsPolicyData.Tables(5).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(5).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(5).Columns
                            If Not (oDataColumn.Ordinal = 0 OrElse oDataColumn.Ordinal = 1) Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())
                                nExcelSheetColumn += 1
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Conviction Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3



                'Party Lifestyle Details
                If dsPolicyData.Tables(6) IsNot Nothing AndAlso dsPolicyData.Tables(6).Rows IsNot Nothing AndAlso dsPolicyData.Tables(6).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(6).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(6).Columns
                            If Not (oDataColumn.Ordinal = 0 OrElse oDataColumn.Ordinal = 1 OrElse oDataColumn.Ordinal = 3) Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())
                                nExcelSheetColumn += 1
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Lifestyle Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3


                'Party Loyalty Scheme Details
                If dsPolicyData.Tables(7) IsNot Nothing AndAlso dsPolicyData.Tables(7).Rows IsNot Nothing AndAlso dsPolicyData.Tables(7).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(7).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(7).Columns
                            If Not (oDataColumn.Ordinal = 0 OrElse oDataColumn.Ordinal = 1 OrElse oDataColumn.Ordinal = 2) Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())
                                nExcelSheetColumn += 1
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Loyalty Scheme Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3


                'Party Bank Details
                If dsPolicyData.Tables(7) IsNot Nothing AndAlso dsPolicyData.Tables(8).Rows IsNot Nothing AndAlso dsPolicyData.Tables(8).Rows.Count > 0 Then

                    For Each oDataRow As System.Data.DataRow In dsPolicyData.Tables(8).Rows
                        For Each oDataColumn As System.Data.DataColumn In dsPolicyData.Tables(8).Columns
                            If Not (oDataColumn.Ordinal = 0 OrElse oDataColumn.Ordinal = 1 _
                                    OrElse oDataColumn.Ordinal = 2 OrElse oDataColumn.Ordinal = 34 _
                                    OrElse oDataColumn.Ordinal = 35 OrElse oDataColumn.Ordinal = 36) Then

                                If oDataColumn.Ordinal = 5 Then
                                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                    nResult = GetLookupDescription(2, "Bank_Payment_Type", oDataRow(oDataColumn.ColumnName).ToString(), sDescription, False)
                                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription.ToString())
                                    nExcelSheetColumn += 1
                                ElseIf oDataColumn.Ordinal = 9 Then
                                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                    nResult = GetLookupDescription(2, "CashListItem_Bank", oDataRow(oDataColumn.ColumnName).ToString(), sDescription, False)
                                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription.ToString())
                                    nExcelSheetColumn += 1
                                ElseIf oDataColumn.Ordinal = 19 Then
                                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                    nResult = GetLookupDescription(2, "Country", oDataRow(oDataColumn.ColumnName).ToString(), sDescription, False)
                                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription.ToString())
                                    nExcelSheetColumn += 1
                                Else
                                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, oDataColumn.ColumnName.ToString())
                                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, oDataRow(oDataColumn.ColumnName).ToString())
                                    nExcelSheetColumn += 1
                                End If
                            End If
                        Next
                        nExcelSheetColumn = 1
                        nExcelSheetRow += 2
                    Next
                Else
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Bank Details Not Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                End If
                nExcelSheetColumn = 1
                InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                nExcelSheetRow += 3

                sXMLDataSet = String.Empty
                nResult = LoadPartyFromDB(r_sXMLDataSetDef:="", r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=sDataModelCode, v_lPartyCnt:=nClientID)
                If nResult <> PMEReturnCode.PMTrue Then
                    'Log Error
                End If

                If sXMLDataSet Is Nothing OrElse sXMLDataSet = String.Empty Then
                    InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "No Party Data Found")
                    InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    Return PMEReturnCode.PMFail
                Else

                    Dim oLookupProperties As Hashtable = New Hashtable()
                    obGISRenewals = New bGIS.Renewals()

                    'Get all the loopup types, GIS, List, UDL etc.
                    obGISRenewals.GetLookupProperties(v_sGisDataModelCode:=sDataModelCode, r_oLookupProperties:=oLookupProperties, vDatabase:=m_oDatabase)
                    If nResult <> PMEReturnCode.PMTrue Then
                        'Log Error
                    End If
                    obGISRenewals.Dispose()
                    obGISRenewals = Nothing

                    Dim srDataset As New System.IO.StringReader(sXMLDataSet)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim xDoc As New XmlDocument

                    xDoc.Load(xmlTR)
                    xmlTR.Close()

                    Dim oNodes As XmlNodeList = xDoc.SelectNodes("//" + sDataModelCode.ToUpper() + "_POLICY_BINDER")

                    Dim oDataSet As DataSet = New DataSet()
                    Dim bAddedToExcel As Boolean = False
                    If oNodes IsNot Nothing Then
                        ChildNode(oNodes, nExcelSheetColumn, nExcelSheetRow, oWorkSheetPart, oLookupProperties, sDataModelCode)
                    End If
                End If
            Else
                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Data Details Not Found")
                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
            End If
        Finally

        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Extract the latest version of Policy, related data and add it to an excel workbook object
    ''' </summary>
    ''' <param name="nClientID"></param>
    ''' <param name="r_oSpreadSheetDocument"></param>
    ''' <returns></returns>
    Private Function ExtractPolicyData(ByVal nClientID As Integer, ByRef r_oSpreadSheetDocument As SpreadsheetDocument) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oFindInsurance As bSIRFindInsurance.Form
        Dim oFindRisk As bSIRFindRisk.Form
        Dim oResultArrayFindInsurance As Object(,) = Nothing
        Dim oResultArrayFindRisk As Object(,) = Nothing
        Dim sXMLDataSet As String = String.Empty
        Dim kInsuranceFileCnt As Integer = 2
        Dim kInsuranceRef As Integer = 3
        Dim kInsuranceFolderCnt As Integer = 12
        Dim kRiskCnt As Integer = 1
        Dim kRiskDescription As Integer = 3
        Dim sDataModelCode As String = String.Empty
        Dim nExcelSheetRow As Integer = 1
        Dim nExcelSheetColumn As Integer = 1
        Dim obGISRenewals As bGIS.Renewals
        Dim sDescription As String = String.Empty
        Dim dtPolicyHeaderData As System.Data.DataTable


        Try

            ' oFindInsurance = New bSIRFindInsurance.Form()
            nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=oFindInsurance, v_sClassName:="bSIRFindInsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=gPMFunctions.ToSafeString(m_sUsername), v_sPassword:=gPMFunctions.ToSafeString(m_sPassword), v_iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=m_oDatabase)

            nResult = oFindInsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageId:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to initialise bSIRFindInsurance.Form")
            End If


            nResult = oFindInsurance.SearchAll(r_vResultArray:=oResultArrayFindInsurance, v_vPartyCnt:=nClientID)

            oFindInsurance.Dispose()
            oFindInsurance = Nothing

            oFindRisk = New bSIRFindRisk.Form()
            nResult = oFindRisk.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If nResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to initialise bSIRFindRisk.Form")
            End If

            'Loop through all the policies and risks on a policy and write them to a temperoray excel
            If Informations.IsArray(oResultArrayFindInsurance) Then

                For nPolicyRow As Integer = oResultArrayFindInsurance.GetLowerBound(1) To oResultArrayFindInsurance.GetUpperBound(1)
                    nExcelSheetColumn = 1
                    nExcelSheetRow = 1
                    dtPolicyHeaderData = New System.Data.DataTable


                    Dim sSheetName As String = "Policy" + (nPolicyRow + 1).ToString()
                    AddWorksheet(r_oSpreadSheetDocument, sSheetName)
                    Dim relId As String = r_oSpreadSheetDocument.WorkbookPart.Workbook.Descendants(Of Sheet)().First(Function(s) sSheetName.Equals(s.Name)).Id
                    Dim oWorkSheetPart As WorksheetPart = CType(r_oSpreadSheetDocument.WorkbookPart.GetPartById(relId), WorksheetPart)


                    nResult = GetPolicyVersionDetails(oResultArrayFindInsurance(kInsuranceFileCnt, nPolicyRow), dtPolicyHeaderData)
                    If nResult <> PMEReturnCode.PMTrue Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Party Header Data Not Found")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    Else
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Policy Header Data")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    End If

                    nExcelSheetRow += 2
                    If dtPolicyHeaderData.Rows.Count = 1 Then

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Insured")
                        nResult = GetLookupDescription(99, "Party", dtPolicyHeaderData.Rows(0)("insured_cnt").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "alternate_reference")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("alternate_reference").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "insurance_ref")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("insurance_ref").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "policy_status")
                        nResult = GetLookupDescription(2, "policy_status", dtPolicyHeaderData.Rows(0)("policy_status_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "source_desc")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("source_desc").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Analysis_Code")
                        nResult = GetLookupDescription(2, "Analysis_Code", dtPolicyHeaderData.Rows(0)("Analysis_Code_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "sub_Branch")
                        nResult = GetLookupDescription(2, "sub_Branch", dtPolicyHeaderData.Rows(0)("sub_Branch_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "business_type")
                        nResult = GetLookupDescription(2, "business_type", dtPolicyHeaderData.Rows(0)("business_type_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "agent_name")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("agent_name").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "currency")
                        nResult = GetLookupDescription(2, "currency", dtPolicyHeaderData.Rows(0)("currency_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "last_trans_description")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("last_trans_description").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Contact Type")
                        nResult = GetLookupDescription(2, "Contact_Type", dtPolicyHeaderData.Rows(0)("Default_Preferred_Correspondence").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "cover_start_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("cover_start_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "inception_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("inception_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "renewal_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("renewal_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "inception_date_tpi")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("inception_date_tpi").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "date_issued")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("date_issued").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "proposal_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("proposal_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "quote_expiry_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("quote_expiry_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "policy_Deductibles")
                        nResult = GetLookupDescription(2, "policy_Deductibles", dtPolicyHeaderData.Rows(0)("policy_Deductibles_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Policy_Limits")
                        nResult = GetLookupDescription(2, "Policy_Limits", dtPolicyHeaderData.Rows(0)("Policy_Limits_code").ToString(), sDescription, True)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        If gPMFunctions.ToSafeInteger(dtPolicyHeaderData.Rows(0)("Contact_user_id")) > 0 Then
                            GetContactUserDetails(dtPolicyHeaderData.Rows(0)("Contact_user_id").ToString(), dtPolicyHeaderData)
                            If dtPolicyHeaderData.Rows.Count > 0 Then

                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Agency Contact")
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtPolicyHeaderData.Rows(0)("FullUsername").ToString())
                                nExcelSheetColumn += 1
                            End If
                        End If
                    End If
                    nExcelSheetColumn = 1
                    InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                    nExcelSheetRow += 3

                    oResultArrayFindRisk = Nothing
                    nResult = oFindRisk.SearchAll(r_vResultArray:=oResultArrayFindRisk, v_vInsuranceFileCnt:=oResultArrayFindInsurance(kInsuranceFileCnt, nPolicyRow))
                    If nResult <> PMEReturnCode.PMTrue Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Unable to Find Risk Data")
                    End If

                    If Informations.IsArray(oResultArrayFindRisk) Then
                        For nRiskRow As Integer = oResultArrayFindRisk.GetLowerBound(1) To oResultArrayFindRisk.GetUpperBound(1)

                            InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Risk " + (nRiskRow + 1).ToString() + " , Description: " + oResultArrayFindRisk(kRiskDescription, nRiskRow))
                            InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                            nExcelSheetRow += 2

                            sXMLDataSet = String.Empty

                            'v_lInsuranceFileCnt: Its actually insurance_folder_cnt, compared via BO clientmanager view risk
                            nResult = LoadRiskFromDB(r_sXMLDataSetDef:="", r_sXMLDataset:=sXMLDataSet, r_sGISDataModelCode:=sDataModelCode,
                                                     r_sQuoteRef:="", r_sQuoteRefPassword:="", r_dtGuaranteedQuoteDate:=Nothing,
                                                     v_lInsuranceFileCnt:=oResultArrayFindInsurance(kInsuranceFolderCnt, nPolicyRow),
                                                     v_lRiskID:=oResultArrayFindRisk(kRiskCnt, nRiskRow))

                            If nResult <> PMEReturnCode.PMTrue Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "No Risk Data Found")
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                                nExcelSheetRow += 2
                                Continue For
                            End If

                            If sXMLDataSet Is Nothing OrElse sXMLDataSet = String.Empty Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "No Risk Data Found")
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                                nExcelSheetRow += 2
                                Continue For
                            End If

                            Dim oLookupProperties As Hashtable = New Hashtable()
                            obGISRenewals = New bGIS.Renewals()

                            'Get all the lookpup types, GIS, List, UDL etc.
                            obGISRenewals.GetLookupProperties(v_sGisDataModelCode:=sDataModelCode, r_oLookupProperties:=oLookupProperties, vDatabase:=m_oDatabase)
                            If nResult <> PMEReturnCode.PMTrue Then
                                InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "GetLookupProperties Failed")
                                InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                                nExcelSheetRow += 2
                            End If
                            obGISRenewals.Dispose()
                            obGISRenewals = Nothing

                            Dim srDataset As New System.IO.StringReader(sXMLDataSet)
                            Dim xmlTR As New XmlTextReader(srDataset)
                            Dim xDoc As New XmlDocument

                            xDoc.Load(xmlTR)
                            xmlTR.Close()

                            Dim oNodes As XmlNodeList = xDoc.SelectNodes("//" + sDataModelCode.ToUpper() + "_POLICY_BINDER")
                            Dim oNode As XmlNode
                            Dim oDataSet As DataSet = New DataSet()
                            Dim oDataTable As System.Data.DataTable
                            Dim oAttribute As XmlAttribute
                            Dim oDataRow As System.Data.DataRow
                            Dim bAddedToExcel As Boolean = False

                            If oNodes IsNot Nothing Then
                                ChildNode(oNodes, nExcelSheetColumn, nExcelSheetRow, oWorkSheetPart, oLookupProperties, sDataModelCode)
                            End If
                        Next 'Risk Loop Ends
                    Else
                        InsertCellInWorksheet(1, 1, oWorkSheetPart, "No Risk Records Found")
                    End If


                Next 'Policy Loop Ends
            Else
                Dim sSheetName As String = "Policy Records"
                AddWorksheet(r_oSpreadSheetDocument, sSheetName)
                Dim relId As String = r_oSpreadSheetDocument.WorkbookPart.Workbook.Descendants(Of Sheet)().First(Function(s) sSheetName.Equals(s.Name)).Id
                Dim oWorkSheetPart As WorksheetPart = CType(r_oSpreadSheetDocument.WorkbookPart.GetPartById(relId), WorksheetPart)
                InsertCellInWorksheet(1, 1, oWorkSheetPart, "No Policy Records Found")
            End If

            oFindRisk.Dispose()
            oFindRisk = Nothing



            Return nResult

        Finally
            If oFindInsurance IsNot Nothing Then
                oFindInsurance.Dispose()
                oFindInsurance = Nothing
            End If
            If oFindRisk IsNot Nothing Then
                oFindRisk.Dispose()
                oFindRisk = Nothing
            End If
            If obGISRenewals IsNot Nothing Then
                obGISRenewals.Dispose()
                obGISRenewals = Nothing
            End If
        End Try
    End Function

    ''' <summary>
    ''' Extract the latest version of Claim, related data and add it to a excel workbook object
    ''' </summary>
    ''' <param name="nClientID"></param>
    ''' <param name="r_oExcelWorkbook"></param>
    ''' <param name="r_oExcelSheet"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExtractClaimData(ByVal nClientID As Integer, ByRef r_oSpreadSheetDocument As SpreadsheetDocument) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oFindClaim As bCLMFindClaim.Business
        Dim oResultArrayFindClaim As Object(,) = Nothing
        Dim sXMLDataSet As String = String.Empty
        Dim kClaimKey As Integer = 1
        Dim kClaimNumber As Integer = 3


        Dim sDataModelCode As String = String.Empty
        Dim nExcelSheetRow As Integer = 1
        Dim nExcelSheetColumn As Integer = 1
        Dim obGISRenewals As bGIS.Renewals
        Dim sDescription As String = String.Empty
        Dim dtClaimHeaderData As System.Data.DataTable

        Try
            oFindClaim = New bCLMFindClaim.Business()
            nResult = oFindClaim.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If nResult <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to initialise bSIRFindInsurance.Form")
            End If

            nResult = GetLookupDescription(99, "Party_R", nClientID, sDescription, False) 'Get the shortname
            nResult = oFindClaim.GetClaimDetailsSFU(r_vResultArray:=oResultArrayFindClaim, v_vClientName:=sDescription.ToUpper.Trim())

            oFindClaim.Dispose()
            oFindClaim = Nothing

            'Loop through all the Claims and Claim risks on a policy and write them to a temperoray excel
            If Informations.IsArray(oResultArrayFindClaim) Then

                For nClaimRow As Integer = oResultArrayFindClaim.GetLowerBound(1) To oResultArrayFindClaim.GetUpperBound(1)
                    nExcelSheetRow = 1
                    nExcelSheetColumn = 1
                    dtClaimHeaderData = New System.Data.DataTable

                    Dim sSheetName As String = "Claim" + (nClaimRow + 1).ToString()
                    AddWorksheet(r_oSpreadSheetDocument, sSheetName)
                    Dim relId As String = r_oSpreadSheetDocument.WorkbookPart.Workbook.Descendants(Of Sheet)().First(Function(s) sSheetName.Equals(s.Name)).Id
                    Dim oWorkSheetPart As WorksheetPart = CType(r_oSpreadSheetDocument.WorkbookPart.GetPartById(relId), WorksheetPart)

                    nResult = GetClaimVersionDetails(oResultArrayFindClaim(kClaimKey, nClaimRow), dtClaimHeaderData, False)
                    If nResult <> PMEReturnCode.PMTrue Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claim Header Data not found")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    Else
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claim Header Data")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    End If

                    nExcelSheetRow += 2
                    If dtClaimHeaderData.Rows.Count = 1 Then

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claim_Number")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Claim_Number").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Policy_Number")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Policy_Number").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Description")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Description").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claim_Status")
                        nResult = GetLookupDescription(2, "Claim_Status", dtClaimHeaderData.Rows(0)("Claim_Status_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Progress_Status")
                        nResult = GetLookupDescription(2, "Progress_Status", dtClaimHeaderData.Rows(0)("Progress_Status_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Primary_Cause")
                        nResult = GetLookupDescription(2, "Primary_Cause", dtClaimHeaderData.Rows(0)("Primary_Cause_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Secondary_Cause")
                        nResult = GetLookupDescription(2, "Secondary_Cause", dtClaimHeaderData.Rows(0)("Secondary_Cause_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Catastrophe_code")
                        nResult = GetLookupDescription(2, "Catastrophe_code", dtClaimHeaderData.Rows(0)("Catastrophe_code_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Handler")
                        nResult = GetLookupDescription(2, "Handler", dtClaimHeaderData.Rows(0)("Handler_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claims_status_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Claims_status_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Location")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Location").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Town")
                        nResult = GetLookupDescription(2, "Town", dtClaimHeaderData.Rows(0)("Town").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Loss_from_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Loss_from_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Loss_to_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Loss_to_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Reported_to_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Reported_to_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Last_modified_date")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Last_modified_date").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "underwriting_year")
                        nResult = GetLookupDescription(2, "underwriting_year", dtClaimHeaderData.Rows(0)("underwriting_year_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Risk_type")
                        nResult = GetLookupDescription(2, "Risk_type", dtClaimHeaderData.Rows(0)("Risk_type_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Currency")
                        nResult = GetLookupDescription(2, "Currency", dtClaimHeaderData.Rows(0)("Currency_id").ToString(), sDescription, False)
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, sDescription)
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Info_only")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Info_only").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Likely_claim")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("Likely_claim").ToString())
                        nExcelSheetColumn += 1
                    End If
                    InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                    nExcelSheetRow += 3 'Move to Value row and then add a blank row
                    nExcelSheetColumn = 1


                    'Get the Reserve details
                    dtClaimHeaderData = New System.Data.DataTable
                    nResult = GetClaimVersionDetails(oResultArrayFindClaim(kClaimKey, nClaimRow), dtClaimHeaderData, True)
                    If nResult <> PMEReturnCode.PMTrue Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claim Reserve and Payment Data not found")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    Else
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "Claim Reserve and Payment Data")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                    End If

                    nExcelSheetRow += 2
                    If dtClaimHeaderData.Rows.Count > 0 Then 'Use the zeroth row as thats for the latest claim version

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "TotalIncurred")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("TotalIncurred").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "TotalPaid")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("TotalPaid").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "ThisRevision")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("ThisRevision").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "ThisPayment")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("ThisPayment").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "ThisSalvageRecovery")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("ThisSalvageRecovery").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "ThisThirdPartyRecovery")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("ThisThirdPartyRecovery").ToString())
                        nExcelSheetColumn += 1

                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "CurrentReserve")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, dtClaimHeaderData.Rows(0)("CurrentReserve").ToString())
                        nExcelSheetColumn += 1

                    End If
                    nExcelSheetColumn = 1
                    InsertCellInWorksheet(nExcelSheetRow + 2, nExcelSheetColumn, oWorkSheetPart, "")
                    nExcelSheetRow += 3


                    sXMLDataSet = String.Empty
                    nResult = LoadClaimFromDB(r_sXMLDataSetDef:="", r_sXMLDataset:=sXMLDataSet,
                                              r_sGISDataModelCode:=sDataModelCode, v_lClaimID:=oResultArrayFindClaim(kClaimKey, nClaimRow))
                    If nResult <> PMEReturnCode.PMTrue Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "No Claim Risk Data Found")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                        nExcelSheetRow += 2
                        Continue For
                    End If

                    If sXMLDataSet Is Nothing OrElse sXMLDataSet = String.Empty Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "No Claim Risk Data Found")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                        nExcelSheetRow += 2
                        Continue For
                    End If

                    Dim oLookupProperties As Hashtable = New Hashtable()
                    obGISRenewals = New bGIS.Renewals()

                    'Get all the loopup types, GIS, List, UDL etc.
                    obGISRenewals.GetLookupProperties(v_sGisDataModelCode:=sDataModelCode, r_oLookupProperties:=oLookupProperties,
                                                      vDatabase:=m_oDatabase)
                    If nResult <> PMEReturnCode.PMTrue Then
                        InsertCellInWorksheet(nExcelSheetRow, nExcelSheetColumn, oWorkSheetPart, "GetLookupProperties Failed")
                        InsertCellInWorksheet(nExcelSheetRow + 1, nExcelSheetColumn, oWorkSheetPart, "")
                        nExcelSheetRow += 2
                    End If
                    obGISRenewals.Dispose()
                    obGISRenewals = Nothing

                    Dim srDataset As New System.IO.StringReader(sXMLDataSet)
                    Dim xmlTR As New XmlTextReader(srDataset)
                    Dim xDoc As New XmlDocument

                    xDoc.Load(xmlTR)
                    xmlTR.Close()

                    Dim oNodes As XmlNodeList = xDoc.SelectNodes("//" + sDataModelCode.ToUpper() + "_POLICY_BINDER")
                    Dim oNode As XmlNode
                    Dim oDataSet As DataSet = New DataSet()
                    Dim oDataTable As System.Data.DataTable
                    Dim oAttribute As XmlAttribute
                    Dim oDataRow As System.Data.DataRow
                    Dim bAddedToExcel As Boolean = False

                    If oNodes IsNot Nothing Then
                        ChildNode(oNodes, nExcelSheetColumn, nExcelSheetRow, oWorkSheetPart, oLookupProperties, sDataModelCode)
                    End If

                Next
            Else
                Dim sSheetName As String = "Claim Records"
                AddWorksheet(r_oSpreadSheetDocument, sSheetName)
                Dim relId As String = r_oSpreadSheetDocument.WorkbookPart.Workbook.Descendants(Of Sheet)().First(Function(s) sSheetName.Equals(s.Name)).Id
                Dim oWorkSheetPart As WorksheetPart = CType(r_oSpreadSheetDocument.WorkbookPart.GetPartById(relId), WorksheetPart)
                InsertCellInWorksheet(1, 1, oWorkSheetPart, "No Claim Records Found")

            End If
            Return nResult
        Finally

            If oFindClaim IsNot Nothing Then
                oFindClaim.Dispose()
                oFindClaim = Nothing
            End If

            If obGISRenewals IsNot Nothing Then
                obGISRenewals.Dispose()
                obGISRenewals = Nothing
            End If
        End Try
    End Function


    Private Sub ChildNode(ByRef r_oNodes As XmlNodeList, ByRef r_nExcelSheetColumn As Integer, ByRef r_nExcelSheetRow As Integer,
                          ByRef r_oWorkSheetPart As WorksheetPart, ByRef r_oLookupProperties As Hashtable, ByRef r_sDataModelCode As String)

        Dim oNode As XmlNode
        Dim oAttribute As XmlAttribute
        Dim sDescription As String
        Dim bAddedToExcel As Boolean = False


        For Each oNode In r_oNodes.Item(0).ChildNodes

            If oNode.Name.ToString() = "S4IDEFAULT" OrElse oNode.Name.ToString() = "WORK_CLAIM" _
                OrElse oNode.Name.ToString() = r_sDataModelCode + "_OUTPUT" Then
                Continue For
            End If

            bAddedToExcel = False
            r_nExcelSheetColumn = 1
            InsertCellInWorksheet(r_nExcelSheetRow, r_nExcelSheetColumn, r_oWorkSheetPart, oNode.Name.ToString())
            r_nExcelSheetRow += 1

            For Each oAttribute In oNode.Attributes

                Select Case oAttribute.Name

                    Case "OI", "US", "UID", "GIS_POLICY_LINK_ID",
                        r_sDataModelCode + "_POLICY_LINK_ID",
                        r_sDataModelCode + "_POLICY_BINDER_ID",
                        r_sDataModelCode + "_" + oNode.Name + "_ID",
                        r_sDataModelCode + "_OUTPUT_ID"

                        'Dont add these to excel
                    Case Else

                        'If lookup type then get its actual value (description) 
                        'Lookup = 2
                        'UDL = 6
                        If r_oLookupProperties.Contains(oNode.Name + ":" + oAttribute.Name) AndAlso
                            (r_oLookupProperties(oNode.Name + ":" + oAttribute.Name + "|TYPE") = 2 Or
                             r_oLookupProperties(oNode.Name + ":" + oAttribute.Name + "|TYPE") = 6) Then

                            sDescription = String.Empty

                            GetLookupDescription((r_oLookupProperties(oNode.Name + ":" + oAttribute.Name + "|TYPE")),
                                                           r_oLookupProperties(oNode.Name + ":" + oAttribute.Name),
                                                           oAttribute.Value, sDescription, False)

                            InsertCellInWorksheet(r_nExcelSheetRow, r_nExcelSheetColumn, r_oWorkSheetPart, oAttribute.Name) 'Column Name
                            InsertCellInWorksheet(r_nExcelSheetRow + 1, r_nExcelSheetColumn, r_oWorkSheetPart, sDescription) 'Values                                                    nExcelSheetColumn += 1
                            r_nExcelSheetColumn += 1
                            bAddedToExcel = True
                        Else
                            InsertCellInWorksheet(r_nExcelSheetRow, r_nExcelSheetColumn, r_oWorkSheetPart, oAttribute.Name) 'Column Name
                            InsertCellInWorksheet(r_nExcelSheetRow + 1, r_nExcelSheetColumn, r_oWorkSheetPart, oAttribute.Value) 'Values
                            r_nExcelSheetColumn += 1
                            bAddedToExcel = True

                        End If
                End Select

            Next ' oAttribute Loop Ends

            If Not bAddedToExcel Then
                InsertCellInWorksheet(r_nExcelSheetRow + 1, r_nExcelSheetColumn, r_oWorkSheetPart, "No Data Found")
            End If

            'Increase by 1 as we have saved column values in (nExcelSheetRow + 1) 
            'else will be overwritten in next iteration
            InsertCellInWorksheet(r_nExcelSheetRow + 2, r_nExcelSheetColumn, r_oWorkSheetPart, "")
            r_nExcelSheetRow += 3


            If oNode.HasChildNodes Then
                'Loop Through the child nodes and their attributes
                Dim oNodes As XmlNodeList
                oNodes = oNode.SelectNodes("//" + oNode.Name)

                ChildNode(oNodes, r_nExcelSheetColumn, r_nExcelSheetRow, r_oWorkSheetPart, r_oLookupProperties, r_sDataModelCode)
            End If

        Next 'oNode Loop Ends



    End Sub

    ''' <summary>
    ''' Get the description based on code of lookup and similar list maintained in the system.
    ''' </summary>
    ''' <param name="nSpecialType"></param>
    ''' <param name="sSpecialTypeReference"></param>
    ''' <param name="sValue"></param>
    ''' <param name="r_sDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetLookupDescription(ByVal nSpecialType As Integer, ByVal sSpecialTypeReference As String,
                                          ByVal sValue As String, ByRef r_sDescription As String, ByVal bCode As Boolean) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oObjectArray(,) As Object
        r_sDescription = ""
        If sValue = "" Then
            Return nResult
        End If

        m_oDatabase.Parameters.Clear()

        nResult = m_oDatabase.Parameters.Add(sName:="nSpecialType", vValue:=nSpecialType,
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = m_oDatabase.Parameters.Add(sName:="sSpecialTypeReference", vValue:=sSpecialTypeReference,
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = m_oDatabase.Parameters.Add(sName:="sValue", vValue:=sValue.Trim(),
                                             iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If bCode Then
            nResult = m_oDatabase.Parameters.Add(sName:="nCode", vValue:=1,
                                              iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

        Else
            nResult = m_oDatabase.Parameters.Add(sName:="nCode", vValue:=0,
                                              iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

        End If
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = m_oDatabase.SQLSelect(sSQL:=kGisGetLookupDataSQL, sSQLName:=kGisGetLookupDataName, bStoredProcedure:=kGisGetLookupDataStored,
                              vResultArray:=oObjectArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If oObjectArray IsNot Nothing AndAlso oObjectArray(0, 0) IsNot Nothing Then
            r_sDescription = oObjectArray(0, 0)
        Else
            Return PMEReturnCode.PMFalse
        End If
        Return nResult
    End Function

    ''' <summary>
    ''' GetClaim Version Details
    ''' </summary>
    ''' <param name="nClaimID"></param>
    ''' <param name="r_dtResult"></param>
    ''' <param name="bGetReserveDetails"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetClaimVersionDetails(ByVal nClaimID As Integer, ByRef r_dtResult As System.Data.DataTable, ByVal bGetReserveDetails As Boolean) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()


        nResult = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=nClaimID,
                                 iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If bGetReserveDetails Then
            nResult = m_oDatabase.ExecuteDataTable(sSQL:=kGetClaimVersionReserveDetailsSQL, sSQLName:=kGetClaimVersionReserveDetailsName,
                                                   bStoredProcedure:=kGetClaimVersionReserveDetailsStored, oRecordset:=r_dtResult)
        Else
            nResult = m_oDatabase.ExecuteDataTable(sSQL:=kGetClaimVersionDetailsSQL, sSQLName:=kGetClaimVersionDetailsName,
                                                   bStoredProcedure:=kGetClaimVersionDetailsStored, oRecordset:=r_dtResult)
        End If

        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If Not (r_dtResult IsNot Nothing) Then
            Return PMEReturnCode.PMFalse
        End If


        Return nResult
    End Function

    ''' <summary>
    ''' Get Policy Version Details
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_dtResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPolicyVersionDetails(ByVal nInsuranceFileCnt As Integer, ByRef r_dtResult As System.Data.DataTable) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()


        nResult = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt,
                                 iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = m_oDatabase.ExecuteDataTable(sSQL:=kGetPolicyVersionDetailsSQL, sSQLName:=kGetPolicyVersionDetailsName,
                                               bStoredProcedure:=kGetPolicyVersionDetailsStored, oRecordset:=r_dtResult)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If Not (r_dtResult IsNot Nothing) Then
            Return PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function

    ''' <summary>
    ''' Get Agency Contact User Details
    ''' </summary>
    ''' <param name="nUserID"></param>
    ''' <param name="r_dtResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetContactUserDetails(ByVal nUserID As Integer, ByRef r_dtResult As System.Data.DataTable) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()


        nResult = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=nUserID,
                                 iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = m_oDatabase.ExecuteDataTable(sSQL:=kGetContactUserDetailsSQL, sSQLName:=kGetContactUserDetailsName,
                                               bStoredProcedure:=kGetContactUserDetailsStored, oRecordset:=r_dtResult)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If Not (r_dtResult IsNot Nothing) Then
            Return PMEReturnCode.PMFalse
        End If
        Return nResult
    End Function

    ''' <summary>
    ''' Get Party and related details
    ''' </summary>
    ''' <param name="nUserID"></param>
    ''' <param name="r_dtResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPartyDetails(ByVal nUserID As Integer, ByRef r_dtResult As System.Data.DataSet) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()


        nResult = m_oDatabase.Parameters.Add(sName:="nPartyCnt", vValue:=nUserID,
                                 iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        nResult = m_oDatabase.ExecuteDataSet(sSQL:=kGetPartyDetailsSQL, sSQLName:=kGetContactUserDetailsName,
                                             bStoredProcedure:=kGetContactUserDetailsStored, oRecordset:=r_dtResult)


        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If Not (r_dtResult IsNot Nothing) Then
            Return PMEReturnCode.PMFalse
        End If
        Return nResult
    End Function

    ''' <summary>
    ''' Adds a new work sheet and assigns its name
    ''' </summary>
    ''' <param name="oSpreadSheet"></param>
    ''' <param name="sSheetName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddWorksheet(ByRef oSpreadSheet As SpreadsheetDocument, ByVal sSheetName As String) As Boolean
        Dim oSheet As Sheet
        Dim oWorksheetPart As WorksheetPart

        ' Add the worksheetpart
        oWorksheetPart = oSpreadSheet.WorkbookPart.AddNewPart(Of WorksheetPart)()
        oWorksheetPart.Worksheet = New Worksheet(New SheetData())
        oWorksheetPart.Worksheet.Save()

        Dim oSheets As Sheets = oSpreadSheet.WorkbookPart.Workbook.GetFirstChild(Of Sheets)()

        ' Add the sheet and make relation to workbook
        oSheet = New Sheet With {
           .Id = oSpreadSheet.WorkbookPart.GetIdOfPart(oWorksheetPart),
           .SheetId = oSheets.ChildElements.Count + 1,
           .Name = sSheetName}
        oSheets.Append(oSheet)
        oSpreadSheet.WorkbookPart.Workbook.Save()

        Return True
    End Function

    ''' <summary>
    ''' Creates a new Row (If not created already) and a new Cell and sets the cells value
    ''' If the cell already exists, it returns without modifying any values.
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="columnIndex"></param>
    ''' <param name="worksheetPart"></param>
    ''' <param name="sCellValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InsertCellInWorksheet(ByVal rowIndex As UInteger, ByVal columnIndex As UInteger,
                                           ByVal worksheetPart As WorksheetPart, ByVal sCellValue As String) As Cell
        Dim columnName = GetExcelColumnName(columnIndex)

        Dim worksheet As Worksheet = worksheetPart.Worksheet
        Dim sheetData As SheetData = worksheet.GetFirstChild(Of SheetData)()
        Dim cellReference As String = (columnName + rowIndex.ToString())

        ' If the worksheet does not contain a row with the specified row index, insert one.
        Dim row As Row
        If (sheetData.Elements(Of Row).Where(Function(r) r.RowIndex.Value = rowIndex).Count() <> 0) Then
            row = sheetData.Elements(Of Row).Where(Function(r) r.RowIndex.Value = rowIndex).First()
        Else
            row = New Row()
            row.RowIndex = rowIndex
            sheetData.Append(row)
        End If

        ' If there is not a cell with the specified column name, insert one.  
        If (row.Elements(Of Cell).Where(Function(c) c.CellReference.Value = columnName + rowIndex.ToString()).Count() > 0) Then
            'If we ever reach this point, it implies we are trying to create a cell already been created.
            'We are sedning incorrect row and column indexes or we need to update the column instead of creating it.
            Return row.Elements(Of Cell).Where(Function(c) c.CellReference.Value = cellReference).First()
        Else
            ' Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
            Dim refCell As Cell = Nothing
            Dim newCell As Cell = New Cell
            newCell.CellReference = cellReference
            newCell.CellValue = New CellValue(sCellValue.ToString())
            newCell.DataType = CellValues.String

            row.InsertBefore(newCell, refCell)
            worksheet.Save()

            Return newCell
        End If
    End Function

    ''' <summary>
    ''' Returns the column name (A, B... AA ... ZZ) based on the column index (non zero based)
    ''' </summary>
    ''' <param name="nColumnNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetExcelColumnName(ByVal nColumnNumber As Integer) As String

        Dim nDividend As Integer = nColumnNumber
        Dim sColumnName As String = String.Empty
        Dim nModulo As Integer
        While nDividend > 0
            nModulo = (nDividend - 1) Mod 26
            sColumnName = Convert.ToChar(65 + nModulo).ToString() & sColumnName
            nDividend = CInt(((nDividend - nModulo) / 26))
        End While
        Return sColumnName

    End Function

    Private Function GetEffectiveDateForPRE(ByVal v_lGISPolicyLinkID As Integer, ByVal v_sPREEffectiveDateOption As String) As Date
        Dim dtPredate As DateTime = Date.Today
        Dim vDateArray As Object
        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="gis_policy_link_id",
                vValue:=v_lGISPolicyLinkID,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return dtPredate
            End If

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="PREEffectiveDateOption",
                vValue:=v_sPREEffectiveDateOption,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return dtPredate
            End If

            m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACGetEffectiveDateSQL,
                sSQLName:=ACGetEffectiveDate,
                bStoredProcedure:=ACEffectiveDateStored,
                vResultArray:=vDateArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return dtPredate
            End If
            If Informations.IsArray(vDateArray) Then
                dtPredate = vDateArray(0, 0)
            End If


            Return dtPredate

        Catch
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveDateForPRE Failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="GetRatingInfo", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err.Description)
            Return dtPredate
        End Try
    End Function

End Class


<System.Runtime.InteropServices.ProgId("SharedStorage_NET.SharedStorage")>
Public NotInheritable Class SharedStorage

    Public InsuranceFileCnt As Integer
    Public PolicyNumber As String
    Public PaymentReference As String
    Public Amount As Double
    Public Currency As String
    Public Status As Integer

End Class
