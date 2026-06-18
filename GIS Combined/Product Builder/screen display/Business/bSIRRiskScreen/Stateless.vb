Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129
<System.Runtime.InteropServices.ProgId("Stateless_NET.Stateless")>
Public NotInheritable Class Stateless
    Implements IDisposable
    '*******************************************************************************
    ' Class Name: Stateless
    '
    ' Description:
    '
    ' Edit History:
    '   10/12/2004 TR - Imported CLAIMS BUILDER from 1.9
    ' RKS 29/04/2005    354-Standard Wording Control Enchanements
    ' CJB 15/06/2005 PN21786 Changed RiskScreenOKClick to rollback tran if error
    ' CJB 13/01/2006 PN26792 Changed RunNBQuote to check UseRiskTypeID reg setting at
    '                        GIS level if not already found (this code was incorrectly
    '                        taken out previously)
    '*******************************************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Stateless"

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private m_bCloseDatabase As Boolean
    Private m_oDatabase As dPMDAO.Database

    ' This module variable should be acceptable because it is only set from initialise
    ' and cannot change during the life of an object using this class
    Private m_bIsUnderwriting As Boolean


    '******************************************************************************
    ' Name:         LoadNewScreen
    ' Description:  replicate functionality that was originally in iPMUScreenControl
    '               in loadnewscreen. moved from RiskScreenLoadRisk
    ' History:
    '   PW  12/08/2004 : created (resilience)
    '   RAW 17/08/2004 : Resilience : removed v_bUseOriginal param
    '   PW  24/08/2004 : remove XML dataset string param - this function
    '                    will now use the 'stateful' versions of GIS methods.
    '   RAW 03/09/2004 : Resilience (#2) : added extra params to replace module
    '                    variables
    '   RAW 20/09/2004 : CQ6832 : added v_lGISDataModelType param
    '*******************************************************************************
    Private Function LoadNewScreen(ByRef r_oGIS As Object, ByVal v_sGisDataModelCode As String, ByVal v_lGISDataModelType As Integer, ByVal v_lScreenId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_vScreenDetailsArray(,) As Object, ByRef r_vScreenValuesArray As Object, ByVal v_bSubScreen As Boolean, ByRef r_sMyObjectName As Object, ByRef r_sMyOIKey As Object, ByRef r_sParentObjectName As Object, ByRef r_sParentOIKey As Object, ByRef r_bChildAddStatus As Boolean, ByVal v_lTransactionType As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vChildOIKeyArray As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lQuoteType As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Create any new GIS data instances that are required
        ' ==========================================================
        ' RAW110804 moved from iPMUScreenControl.LoadNewScreen
        For lTemp As Integer = v_vScreenDetailsArray.GetLowerBound(1) To v_vScreenDetailsArray.GetUpperBound(1)
            'The object will not be null when it's a listview, but the property will be

            If Not (Convert.IsDBNull(v_vScreenDetailsArray(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)) Or Informations.IsNothing(v_vScreenDetailsArray(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp))) Then
                'This bit is for a 'normal' control

                'In the parent we've either got RSA_policy_binder or RSA_motor
                'If this is the top screen, we've no child and the array object name could be anything
                'If this is a sub screen, we've a child (but maybe no key) and the array object name
                'MUST be the same as the child

                If r_sMyObjectName = "" Then

                    ' we have NOT been asked to load a specific type of object we
                    ' so get all instances for the current type of child object
                    ' currently being processed (This means that we must be
                    'processing a normal control on a parent /top level screen)

                    lReturn = r_oGIS.GetChildOIKey(v_sParentObjectName:=r_sParentObjectName, v_sParentOIKey:=r_sParentOIKey, v_sChildObjectName:=v_vScreenDetailsArray(PBDatabaseConsts.ACDExtraGISObjectName, lTemp), r_vChildOIKeyArray:=vChildOIKeyArray)
                    If Not Informations.IsArray(vChildOIKeyArray) Then
                        ' Create the new OI of whatever...

                        lReturn = r_oGIS.NewObjectInstance(v_sObjectName:=v_vScreenDetailsArray(PBDatabaseConsts.ACDExtraGISObjectName, lTemp), r_sOIKey:=r_sMyOIKey, v_sParentOIKey:=r_sParentOIKey)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = lReturn
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadNewScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadNewScreen")
                            Return result
                        End If
                    Else
                        ' Else use the first one


                        r_sMyOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))
                    End If
                Else
                    'r_sMyObjectName is blank
                    ' we know what type of object we have been asked to load but
                    ' not the actual instance (ie this is displaying a child screen
                    ' in add mode). This means that this must be a normal control
                    ' on a child screen that is to be displayed
                    If r_sMyOIKey.Length = 0 Then
                        ' We have not been asked for a specific instance of a
                        ' paricular type of object so this must be a request to add
                        ' a new one (ie this is displaying a child screen in add
                        ' mode). Note that this will only run for the first control
                        ' on this object. Subsequent controls on this object will
                        'follow the 'ELSE' path

                        ' Create the new OI of whatever...

                        lReturn = r_oGIS.NewObjectInstance(v_sObjectName:=r_sMyObjectName, r_sOIKey:=r_sMyOIKey, v_sParentOIKey:=r_sParentOIKey)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = lReturn
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadNewScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadNewScreen")
                            Return result
                        End If

                        ' Good - we have created a new instance so lets run the
                        ' default rules to 'initialise' the object
                        lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeDefault

                    Else
                        'm_sChildOIKey is blank
                        ' Either we have been asked to display a specific (child)
                        ' instance (ie this is displaying a child screen in edit
                        ' mode) OR this is NOT the first control for a new object
                        ' instance

                        'AK 200603 - rerun the edit script (main) using the current
                        ' rules - but only if not run already for an earlier control
                        If lQuoteType <> PBQuoteTypeEncode.PBCQemQuoteTypeDefault Then
                            lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                        End If
                    End If
                End If
            Else
                'This is a list view control...


                'developer guide no 270. 
                If Not (Convert.IsDBNull(v_vScreenDetailsArray(PBDatabaseConsts.ACDChildScreenId, lTemp)) Or Informations.IsNothing(v_vScreenDetailsArray(PBDatabaseConsts.ACDChildScreenId, lTemp))) Or Convert.ToString(v_vScreenDetailsArray(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)).ToLower() = "work_claim_peril" Then


                    'developer guide no 270. 
                    If Convert.ToString(v_vScreenDetailsArray(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)).ToLower() = "work_claim_peril" Then
                        r_sMyOIKey = ""
                    End If

                    'List views from the top level screen can use the parent
                    'RSA_policy_binder. This is because there's only one motor, and
                    'so all drivers hang off it and so indirectly off the binder
                    'From a sub screen we use the child RSA driver
                    'This is because the convictions hang off the driver
                    If r_sMyOIKey.Length = 0 Then
                        'The trouble here is that we have the conviction
                        'frame/listview being processed before any of the fields.
                        'So when we're adding we haven't yet got the OI key for the
                        'driver which we need to hang the convictions off.  So
                        'let's go get a new one.
                        If v_bSubScreen Then

                            lReturn = r_oGIS.NewObjectInstance(v_sObjectName:=r_sMyObjectName, r_sOIKey:=r_sMyOIKey, v_sParentOIKey:=r_sParentOIKey)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = lReturn
                                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadNewScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadNewScreen")
                                Return result
                            End If

                            r_bChildAddStatus = True 'signal child has been added
                            lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeDefault
                        End If
                    End If
                End If
            End If
        Next lTemp

        ' Now run NB Quote
        If lQuoteType <> 0 Then
            lReturn = CType(RunNBQuote(r_oGIS:=r_oGIS, v_lTransactionType:=v_lTransactionType, v_iPBCQemQuoteType:=lQuoteType, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISDataModelType:=v_lGISDataModelType, v_lScreenId:=v_lScreenId, v_lRiskTypeId:=v_lRiskTypeId, v_sChildOIKey:=r_sMyOIKey, v_lInsuranceFileCnt:=v_lInsuranceFileCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadNewScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadNewScreen")
                Return result
            End If
        End If

        Return result

    End Function

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property


    '******************************************************************************
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    '******************************************************************************
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

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


            lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' set the module level variable
            m_bIsUnderwriting = True

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    '******************************************************************************
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



    '<XMLTransport method="RiskScreenLoadRisk">
    '<inputs>
    '   <property name="iTask" type="Integer"/>
    '   <property name="iSourceID" type="Integer"/>
    '   <property name="lNavigate" type="Long"/>
    '   <property name="lProcessMode" type="Long"/>
    '   <property name="sTransactionType" type="String"/>
    '   <property name="dtEffectiveDate" type="Date"/>
    '   <property name="bSubScreen" type="Boolean"/>
    '   <property name="lScreenId" type="Long"/>
    '   <property name="lRiskId" type="Long"/>
    '   <property name="lRiskTypeId" type="Long"/>
    '   <property name="sGisDataModelCode" type="String"/>
    '   <property name="lGISDataModelType" type="Long"/>
    '   <property name="lObjectType" type="Long"/>
    '   <property name="sGISXMLDataset" type="String"/>
    '   <property name="sMyOIKey" type="String"/>
    '   <property name="sMyObjectName" type="String"/>
    '   <property name="sParentOIKey" type="String"/>
    '   <property name="sParentObjectName" type="String"/>
    '   <property name="lPolicyLinkId" type="Long"/>
    '   <property name="lInsuranceFolderCnt" type="Long"/>
    '   <property name="lInsuranceFileCnt" type="Long"/>
    '   <property name="vScreenDetailsArray" type="Variant"/>
    '   <property name="vScreenValuesArray" type="Variant"/>
    '   <property name="vRiskDetailsArray" type="Variant"/>
    '   <property name="vRiskTypeDetailsArray" type="Variant"/>
    '   <property name="lTransactionType" type="Long"/>
    '   <property name="lProductId" type="Long"/>
    '   <property name="lPartyCnt" type="Long"/>
    '   <property name="lClaimID" type="Long"/>
    '   <property name="bCopyRisk" type="Boolean"/>
    '</inputs>
    '<outputs>
    '   <property name="lRiskId" type="Long"/>
    '   <property name="sGISXMLDataset" type="String"/>
    '   <property name="sMyOIKey" type="String"/>
    '   <property name="sMyObjectName" type="String"/>
    '   <property name="sParentOIKey" type="String"/>
    '   <property name="sParentObjectName" type="String"/>
    '   <property name="lPolicyLinkId" type="Long"/>
    '   <property name="vScreenValuesArray" type="Variant"/>
    '   <property name="vRiskDetailsArray" type="Variant"/>
    '   <property name="vRiskTypeDetailsArray" type="Variant"/>
    '   <property name="bChildAddStatus" type="Boolean"/>
    '   <property name="bRiskAdded" type="Boolean"/>
    '   <property name="bRiskCopied" type="Boolean"/>
    '   <property name="sReferReasons" type="String"/>
    '   <property name="sDeclineReasons" type="String"/>
    '   <property name="sMessages" type="String"/>
    '   <property name="lQuoteType" type="Long"/>
    '</outputs>
    '</XMLTransport>
    '*******************************************************************************
    ' Name: RiskScreenLoadRisk
    '
    ' Description: Put all database updates into a single business-side transaction,
    '              when loading a new risk.
    '
    ' History : RAW110804 - created (resilience)
    '           PW120804 - receive originalscreenvalues and claimversioning flag
    '                      params and use them for claim versioning. Also add
    '                      bRiskAdd flag
    ' RAW 17/08/2004 : Resilience : removed r_vOriginalScreenValues and v_bUsingClaimVersions params
    ' RAW 03/09/2004 : Resilience (#2) : replaced parameters with XMLTransport string
    ' RAW 20/09/2004 : CQ6832 : added extra properties to output structure
    '*******************************************************************************
    Public Function RiskScreenLoadRisk(ByVal v_sInput As String) As String

        Dim result As String = String.Empty
        Const ksFunctionName As String = "RiskScreenLoadRisk"

        Dim TheInput As XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskIn = XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskIn.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : added
        Dim TheOutput As XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskOut = XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskOut.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : replaced SIRRiskScreenBasicOut

        Dim oBusiness As New bSIRRiskScreen.Business
        ' Dim oGIS As bGIS.Application
        Dim oGIS As Object = Nothing


        Dim bDBTransStarted As Boolean
        Dim lReturn As Integer
        Dim sMsg As String = ""

        Dim lRiskId, lOldGISPolicyLinkId As Integer
        Dim lNewPolicyLinkId As Object
        Dim lNewRiskID As Object

        ' PW110804
        Dim vOIKeyArray As Object = Nothing
        Dim sParentOIKey As Object = ""

        Dim lQuoteType As Integer
        Dim bChildAddStatus, bRiskAdded, bRiskCopied As Boolean

        Try


            ' DeSerialise the Input into the Structure
            ' =========================================
            ' RAW 03/09/2004 : Resilience (#2) : added
            TheInput = DeserializeRiskScreenLoadRiskIn(sXML:=v_sInput)


            Dim vDataDictionary As Object = Nothing
            Dim vScreenHeader As Object = Nothing
            Dim vScreenDetails As Object = Nothing
            Dim vChildScreenDetails As Object = Nothing
            With TheInput

                ' RAW 03/09/2004 : Resilience (#2) : added TheInput to replace parameter and module variables

                ' RAW 03/09/2004 : Resilience (#2) : added
                With TheOutput
                    ' set initial values for data items that are to be returned only when all is successful
                    .lRiskId = TheInput.lRiskId
                    .sGISXMLDataset = ""
                    .sMyOIKey = TheInput.sMyOIKey
                    .sMyObjectName = TheInput.sMyObjectName
                    .sParentOIKey = TheInput.sParentOIKey
                    .sParentObjectName = TheInput.sParentObjectName
                    .lPolicyLinkId = TheInput.lPolicyLinkId

                    .vScreenValuesArray = Nothing

                    .vRiskDetailsArray = Nothing

                    .vRiskTypeDetailsArray = Nothing
                    .bChildAddStatus = False
                    .bRiskAdded = False
                    .bRiskCopied = False
                End With

                ' create business object
                oBusiness = New bSIRRiskScreen.Business
                oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                oBusiness.SetProcessModes(vTask:= .iTask, vNavigate:= .lNavigate, vProcessMode:= .lProcessMode, vTransactionType:= .sTransactionType, vEffectiveDate:= .dtEffectiveDate)

                If oBusiness Is Nothing Then
                    sMsg = "Failed to get business object "
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If


                oBusiness.InsuranceFileCnt = .lInsuranceFileCnt

                '' create bGIS.Application
                oGIS = Nothing
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))
                oGIS.SetProcessModes(vTask:=ToSafeInteger(.iTask), vNavigate:=ToSafeInteger(.lNavigate), vProcessMode:=ToSafeInteger(.lProcessMode), vTransactionType:=ToSafeString(.sTransactionType), vEffectiveDate:=ToSafeDate(.dtEffectiveDate))


                If oGIS Is Nothing Then
                    sMsg = "Failed to get GIS object "
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If


                ' Initialise the business component(s)

                ' PW110804 - required only for sub screen
                If .bSubScreen Then

                    lReturn = oGIS.LoadFromXML(v_sDataModelCode:= .sGisDataModelCode, v_sXMLDataSet:= .sGISXMLDataset)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMsg = "Failed to load data into GIS"
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                    End If
                End If


                ' Start a DB transaction
                ' ==============================
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Start a Database Transaction"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                bDBTransStarted = True


                ' now do the stuff
                ' ======================
                lRiskId = .lRiskId


                If Not .bSubScreen Then
                    ' ==============================================================================
                    ' This is a top level screen
                    ' ==============================================================================

                    'Get the Risk - if it doesn't exist create it, we need the number for the GIS

                    '-------------------------------------------------------------------------------------
                    '   18/07/2002  RVH BEGIN
                    '                   Only do GetRisk for RISK type GIS data models
                    '-------------------------------------------------------------------------------------
                    If .lGISDataModelType = GISDataModelType.GISDMTypeRisk Then

                        ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                        lReturn = oBusiness.GetRisk_Stateless(vRiskArray:= .vRiskDetailsArray, vRiskTypeArray:= .vRiskTypeDetailsArray, r_lRiskId:=lRiskId, v_lProductId:= .lProductId, v_iSourceID:= .iSourceID, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_lInsuranceFolderCnt:= .lInsuranceFolderCnt, v_lInsuranceFileCnt:= .lInsuranceFileCnt)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "An error occurred when attempting to get the risk"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If

                        ' RAW 03/09/2004 : Resilience (#2) : moved to within IF statement
                        lNewRiskID = lRiskId ' RAW 03/09/2004 : Resilience (#2) : replaced oBusiness.RiskId with lRiskId
                        If .lRiskId = 0 Then
                            ' PW120804
                            bRiskAdded = True
                        Else
                            ' RAW 03/09/2004 : Resilience (#2) : added
                            If lRiskId <> .lRiskId Then
                                bRiskCopied = True
                            End If
                            ' Even for risks that have been copied, continue to point to the old risk for now
                            ' until all other data for the risk has been copied
                            lRiskId = .lRiskId
                        End If
                        ' RAW 03/09/2004 : Resilience (#2) : end
                    End If

                    '-------------------------------------------------------------------------------------
                    '   18/07/2002  RVH END
                    '-------------------------------------------------------------------------------------

                    'If this is broking and the risk has been copied then don't create a new one as that
                    'will happen when the old risk is copied.
                    If Not (bRiskCopied And Not m_bIsUnderwriting) Then

                        'TF300102 - Underwriting needs to pass InsuranceFolderCnt for now
                        If m_bIsUnderwriting Then

                            If .lGISDataModelType = GISDataModelType.GISDMTypeRisk Then

                                'get the current risk details
                                ' RAW110804 replaced iGIS with bGIS
                                ' PW240804 - call the 'stateful' version of the GIS method

                                lReturn = oGIS.LoadRiskFromDBStateful(r_sGisDataModelCode:= .sGisDataModelCode, v_lInsuranceFileCnt:=ToSafeInteger(.lInsuranceFolderCnt), v_lRiskId:=ToSafeInteger(lRiskId))

                                '25/04/2003 - PWC - (408) User Definable Fields
                            ElseIf .lGISDataModelType = GISDataModelType.GISDMTypeParty Then
                                'Get the current party details

                                '                    ' PW120804
                                '                   bRiskAdded = False              ' RAW 03/09/2004 : Resilience (#2) : removed

                                ' RAW110804 replaced iGIS with bGIS
                                ' PW240804 - call the 'stateful' version of the GIS method

                                lReturn = oGIS.LoadPartyFromDBStateful(r_sGisDataModelCode:= .sGisDataModelCode, v_lPartyCnt:=ToSafeInteger(.lPartyCnt))
                            ElseIf .lGISDataModelType = GISDataModelType.GISDMTypeCase Then
                                'Get the current party details

                                lReturn = oGIS.LoadCaseFromDBStateful(r_sGisDataModelCode:= .sGisDataModelCode, v_lCaseID:=ToSafeInteger(.lCaseID))

                            Else
                                'get the current claim details
                                '   RVH 26/8/2003 - START : ClaimId parameter of LoadClaimFromDB
                                '                   renamed to WorkClaimId to make it clearer what
                                '                   claim id is required
                                ' RAW110804 replaced iGIS with bGIS
                                ' PW240804 - call the 'stateful' version of the GIS method

                                lReturn = oGIS.LoadClaimFromDBStateful(r_sGisDataModelCode:= .sGisDataModelCode, v_lClaimID:=ToSafeInteger(.lClaimID))
                                '   RVH 26/8/2003 - END

                                ' AMB 10/01/03 - Start - IAG 217 Spec - load original GIS data

                            End If
                        Else
                            'broking
                            ' PW240804 - call the 'stateful' version of the GIS method
                            If .lGISDataModelType = GISDataModelType.GISDMTypeRisk Then

                                lReturn = oGIS.LoadRiskFromDBStateful(r_sGisDataModelCode:= .sGisDataModelCode, v_lInsuranceFileCnt:=ToSafeInteger(.lInsuranceFileCnt))

                            ElseIf .lGISDataModelType = GISDataModelType.GISDMTypeParty Then
                                'Get the current party details

                                lReturn = oGIS.LoadPartyFromDBStateful(r_sGisDataModelCode:= .sGisDataModelCode, v_lPartyCnt:=ToSafeInteger(.lPartyCnt))
                                ' ElseIf .lGISDataModelType = GISDMTypeCase Then
                                'Get the current party details
                                '    lReturn = oGIS.LoadCaseFromDBStateful( _
                                'r_sGisDataModelCode:=.sGisDataModelCode, _
                                'v_lCaseID:=.lCaseID)
                            End If
                        End If

                        ' Check for errors
                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                            .lPolicyLinkId = oGIS.PolicyLinkID

                            'If we have just done a copy risk then run a different set of scripts.
                            If .bCopyRisk Then
                                lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
                            Else
                                lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                            End If

                        Else
                            ' Failed to load risk, party or claim from GIS

                            ' RAW 06/10/2003 : CQ2746 : replace call to NBQuote which now occurs later
                            lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeDefault

                            'JES 2307200 - Fixes Multiple gis_policy_link records - implemented by ED

                            ' RAW 03/09/2004 : Resilience (#2) : - restructured
                            If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                                '
                                sMsg = "Failed to load from DB in GIS"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            Else

                                ' we have not found a dataset

                                'TF300102 - Underwriting needs to pass InsuranceFolderCnt for now
                                ' PW030903 - CQ1912 - underwriting needs ins file too
                                If m_bIsUnderwriting Then

                                    If .lGISDataModelType = GISDataModelType.GISDMTypeRisk Then

                                        ' RAW 03/09/2004 : Resilience (#2) : replaced (lRiskID = lNewRiskID) test with bRiskAdd = true
                                        If bRiskAdded Then

                                            ' This is a new Risk so create a new dataset for it

                                            ' RAW110804 replaced iGIS with bGIS
                                            ' PW240804 - call the 'stateful' version of the GIS method

                                            lReturn = oGIS.NewRiskDatasetStateful(v_sGisDataModelCode:= .sGisDataModelCode, r_lPolicyLinkID:= .lPolicyLinkId, r_sTopOIKey:= .sParentOIKey, v_lInsuranceFileCnt:= .lInsuranceFolderCnt, v_lRiskID:=lNewRiskID)
                                        Else
                                            ' this is not a new risk but we could not find GIS data for it
                                            sMsg = "Failed to find risk from DB in GIS"
                                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                                        End If

                                        '25/04/2003 - PWC - (408) User Definable Fields
                                        'Is it a party?
                                    ElseIf .lGISDataModelType = GISDataModelType.GISDMTypeParty Then

                                        ' we couldn't find any data in GIS so this must be a new party

                                        'Create a new data set for new party

                                        ' We cannot add risks from here
                                        ' RAW 15/10/2004 : changed true to false
                                        bRiskAdded = False

                                        ' RAW110804 replaced iGIS with bGIS
                                        ' PW240804 - call the 'stateful' version of the GIS method

                                        lReturn = oGIS.NewPartyDatasetStateful(v_sGisDataModelCode:= .sGisDataModelCode, r_lPolicyLinkId:= .lPolicyLinkId, r_sTopOIKey:= .sParentOIKey, v_lPartyCnt:=ToSafeInteger(.lPartyCnt))
                                    ElseIf .lGISDataModelType = GISDataModelType.GISDMTypeCase Then

                                        ' we couldn't find any data in GIS so this must be a new party
                                        'Create a new data set for new party
                                        bRiskAdded = False

                                        If .iTask = gPMConstants.PMEComponentAction.PMAdd Then
                                            lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypePreScreen
                                        End If


                                        lReturn = oGIS.NewCaseDatasetStateful(v_sGisDataModelCode:= .sGisDataModelCode, r_lPolicyLinkId:= .lPolicyLinkId, r_sTopOIKey:= .sParentOIKey, v_lCaseID:=ToSafeInteger(.lCaseID))
                                    Else
                                        'not risk or party, must be claim

                                        ' we couldn't find any data in GIS so this must be a new claim

                                        'Create a new data set for new claim

                                        ' We cannot add risks from here
                                        ' RAW 15/10/2004 : changed true to false
                                        bRiskAdded = False ' RAW 03/09/2004 : Resilience (#2) : added

                                        '   RVH 26/8/2003 - START : ClaimId parameter of NewClaimDataset
                                        '                   renamed to WorkClaimId to make it clearer what
                                        '                   claim id is required
                                        ' RAW110804 replaced iGIS with bGIS
                                        ' PW240804 - call the 'stateful' version of the GIS method


                                        lReturn = oGIS.NewClaimDatasetStateful(v_sGisDataModelCode:= .sGisDataModelCode, r_lPolicyLinkId:= .lPolicyLinkId, r_sTopOIKey:= .sParentOIKey, v_lClaimID:=ToSafeInteger(.lClaimID))
                                        '   RVH 26/8/2003 - END
                                    End If

                                Else
                                    ' Broking
                                    ' This is a new Risk so create a new dataset for it

                                    ' RAW110804 replaced iGIS with bGIS
                                    ' PW240804 - call the 'stateful' version of the GIS method
                                    If .lGISDataModelType = GISDataModelType.GISDMTypeParty Then

                                        lReturn = oGIS.NewPartyDatasetStateful(v_sGisDataModelCode:= .sGisDataModelCode, r_lPolicyLinkId:= .lPolicyLinkId, r_sTopOIKey:= .sParentOIKey, v_lPartyCnt:=ToSafeInteger(.lPartyCnt))
                                    Else

                                        lReturn = oGIS.NewRiskDatasetStateful(v_sGisDataModelCode:= .sGisDataModelCode, r_lPolicyLinkId:= .lPolicyLinkId, r_sTopOIKey:= .sParentOIKey, v_lInsuranceFileCnt:=ToSafeInteger(.lInsuranceFileCnt), v_lRiskId:=lNewRiskID)
                                    End If
                                End If

                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    sMsg = "Failed to create a new dataset"
                                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                                End If

                            End If
                        End If
                    End If

                    ' ==============================================================================
                    ' Note - This is STILL processing a top level screen
                    ' ==============================================================================
                    ' RAW 03/09/2004 : Resilience (#2) : replaced existing test with bRiskCopied = true
                    If bRiskCopied Then

                        ' A copy has just been taken of the main risk data so lets copy and save the rest of it

                        ' PW290803 - CQ1912 - deal with associated clients / disclosures
                        ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent
                        'lReturn = oBusiness.CopyInsuredsAndDisclosures_Stateless( _
                        'v_lInsuranceFileCnt:=.lInsuranceFileCnt, _
                        'v_lOldRiskCnt:=.lRiskId, _
                        'v_lNewRiskCnt:=lNewRiskID)
                        'If lReturn <> PMTrue Then
                        '     sMsg = "Failed to copy Associated Clients and Disclosures"
                        '     RaiseBackOfficeError ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice
                        ' End If

                        'copy GIS details
                        'TF300102 - Underwriting needs to pass InsuranceFolderCnt for now
                        ' PW080903 - CQ1912 - need to pass ins file too
                        If m_bIsUnderwriting Then

                            ' added to ensure we have the old policy link id....

                            lReturn = oBusiness.GetPolicyLinkIdFromRisk(v_lRiskId:= .lRiskId, r_lGisPolicyLinkId:=lOldGISPolicyLinkId)

                            .lPolicyLinkId = lOldGISPolicyLinkId
                            ' RAW110804 replaced iGIS with bGIS
                            ' PW240804 - call the 'stateful' version of the GIS method

                            lReturn = oGIS.CopyDataSetStateful(v_sDataModelCode:=ToSafeString(.sGisDataModelCode), r_lNewGISPolicyLinkId:=lNewPolicyLinkId, v_vOldGISPolicyLinkId:=ToSafeInteger(TheInput.lPolicyLinkId), v_vOldInsuranceFileCnt:=ToSafeInteger(.lInsuranceFolderCnt), v_vOldRiskID:=ToSafeInteger(.lRiskId), v_vNewInsuranceFileCnt:=ToSafeInteger(.lInsuranceFolderCnt), v_vNewRiskID:=ToSafeInteger(lNewRiskID))
                        Else
                            'JES Get old policy link id ( TEMP FIX )
                            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                            lReturn = oBusiness.GetOldPolicyLinkId_Stateless(.lRiskId, .lPolicyLinkId)

                            ' RAW110804 replaced iGIS with bGIS
                            ' PW240804 - call the 'stateful' version of the GIS method

                            lReturn = oGIS.CopyDataSetStateful(v_sDataModelCode:= .sGisDataModelCode, r_lNewGISPolicyLinkId:=lNewPolicyLinkId, v_vOldGISPolicyLinkId:=ToSafeInteger(.lPolicyLinkId), v_vOldInsuranceFileCnt:=ToSafeInteger(.lInsuranceFileCnt), v_vOldRiskID:=ToSafeInteger(.lRiskId), v_vNewInsuranceFileCnt:=ToSafeInteger(.lInsuranceFileCnt), v_vNewRiskID:=ToSafeInteger(lNewRiskID))
                        End If

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to copy the dataset"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If


                        'save GIS details to Database
                        ' RAW110804 replaced iGIS with bGIS
                        ' PW240804 - use the 'stateful' version of the GIS method

                        lReturn = oGIS.SaveToDBStateful

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to save the dataset"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If


                        ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                        lReturn = oBusiness.CopyExtraDetails_Stateless(sDataModelCode:= .sGisDataModelCode, lNewGISPolicyLinkID:=lNewPolicyLinkId, lOldGISPolicyLinkId:= .lPolicyLinkId, lOldRiskID:= .lRiskId, lNewRiskID:=lNewRiskID)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to copy the extra details"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If

                        lRiskId = lNewRiskID

                    End If

                    ' ==============================================================================
                    ' Note - This is STILL processing a top level screen
                    ' ==============================================================================

                    .lRiskId = lRiskId

                    '        If Not IsNull(vPolicyLinkId) Then
                    If lNewPolicyLinkId <> 0 Then
                        .lPolicyLinkId = lNewPolicyLinkId
                    End If

                    ' RAW 06/10/2003 : CQ2746 : end

                    ' PW110804 - moved from iPMUScreenControl: Start
                    .sParentObjectName = .sGisDataModelCode & "_policy_binder"

                    ' Get the Top Level OI Key

                    lReturn = oGIS.GetAllOIKey(v_sObjectName:=(.sParentObjectName), r_vOIKeyArray:=vOIKeyArray)

                    If Informations.IsArray(vOIKeyArray) Then


                        sParentOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))
                    End If

                    vOIKeyArray = Nothing

                    If sParentOIKey = "" Then

                        lReturn = oGIS.NewObjectInstance(v_sObjectName:= .sGisDataModelCode & "_policy_binder", r_sOIKey:=sParentOIKey)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to get a new instance of object " & .sGisDataModelCode & "_policy_binder"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If
                    End If

                    .sParentOIKey = sParentOIKey
                    ' PW110804 - moved from iPMUScreenControl: End

                End If

                ' Now run NB Quote
                ' =======================
                If (.iTask = gPMConstants.PMEComponentAction.PMAdd Or .iTask = gPMConstants.PMEComponentAction.PMEdit Or .iTask = gPMConstants.PMEComponentAction.PMCopy) And lQuoteType <> 0 Then

                    'TBD: This is required for Datasure so that document template links can be made in the default script
                    'If lQuoteType = PBCQemQuoteTypeDefault Then
                    '    'do this so we can create items in the default script
                    '    lReturn = m_oDatabase.SQLCommitTrans()
                    'End If

                    'only run scripts if in an editable mode and script id is valid

                    ' PW240804 - using 'stateful' GIS components so don't pass XML dataset
                    ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                    ' RAW 20/09/2004 : CQ6832 : added v_lGISDataModelType param
                    lReturn = RunNBQuote(r_oGIS:=oGIS, v_lTransactionType:= .lTransactionType, v_iPBCQemQuoteType:=lQuoteType, v_sGisDataModelCode:= .sGisDataModelCode, v_lGISDataModelType:= .lGISDataModelType, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_sChildOIKey:= .sMyOIKey, v_lInsuranceFileCnt:= .lInsuranceFileCnt)


                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMsg = "Failed to run NBQuote"
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                    End If
                End If

                ' RDT 03/10/2006 - More efficient to get the screen details here if they're not
                '                  passed in than to retrieve them in the calling component.
                If (Not Informations.IsArray(.vScreenDetailsArray)) And .lScreenId > 0 Then


                    ReDim vDataDictionary(1)


                    oBusiness.ScreenId = .lScreenId


                    lReturn = oBusiness.GetScreenDetails(r_vDataDictionary:=vDataDictionary, r_vScreenHeader:=vScreenHeader, r_vScreenDetails:=vScreenDetails, r_vChildScreenDetails:=vChildScreenDetails)
                    If Not Informations.IsArray(vScreenDetails) Then
                        sMsg = "Failed to get screen details from the bSirRiskScreen.Business for Screen ID - " & .lScreenId
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                    End If



                    .vScreenDetailsArray = vScreenDetails

                End If

                ' PW120804 - moved code to LoadNewScreen so it can be called for claim
                ' versioning seperately
                ' PW240804 - using 'stateful' GIS components so don't pass XML dataset
                ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                ' RAW 20/09/2004 : CQ6832 : added v_lGISDataModelType param

                lReturn = LoadNewScreen(r_oGIS:=oGIS, v_sGisDataModelCode:= .sGisDataModelCode, v_lGISDataModelType:= .lGISDataModelType, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_vScreenDetailsArray:= .vScreenDetailsArray, r_vScreenValuesArray:= .vScreenValuesArray, v_bSubScreen:= .bSubScreen, r_sMyObjectName:= .sMyObjectName, r_sMyOIKey:= .sMyOIKey, r_sParentObjectName:= .sParentObjectName, r_sParentOIKey:= .sParentOIKey, r_bChildAddStatus:=bChildAddStatus, v_lTransactionType:= .lTransactionType, v_lInsuranceFileCnt:= .lInsuranceFileCnt)


                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to load new screen"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If


                ' RAW 17/08/2004 : Resilience : removed code to handle claim versions


                ' Refresh xml string with updated values from GIS

                lReturn = oGIS.ReturnAsXML(r_sXMLDataset:= .sGISXMLDataset)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to refresh the XML"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                ' Commit the DB transaction
                ' ============================
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseBackOfficeError(ksFunctionName, lReturn, "Failed to commit changes to the database.", TheOutput.Errors.BackOffice)
                End If

            End With


            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are to be returned only when all is successful
                .lRiskId = lRiskId
                .sGISXMLDataset = TheInput.sGISXMLDataset
                .sMyOIKey = TheInput.sMyOIKey
                .sMyObjectName = TheInput.sMyObjectName
                .sParentOIKey = TheInput.sParentOIKey
                .sParentObjectName = TheInput.sParentObjectName
                .lPolicyLinkId = TheInput.lPolicyLinkId


                .vScreenValuesArray = TheInput.vScreenValuesArray


                .vRiskDetailsArray = TheInput.vRiskDetailsArray


                .vRiskTypeDetailsArray = TheInput.vRiskTypeDetailsArray
                .bChildAddStatus = bChildAddStatus
                .bRiskAdded = bRiskAdded
                .bRiskCopied = bRiskCopied
                .lQuoteType = lQuoteType ' RAW 20/09/2004 : CQ6832 : added
            End With


        Catch ex As Exception

            ' Error Section.

            Dim lErrNumber As gPMConstants.PMEReturnCode
            Dim sErrDescription As String = ""

            ' Store the Error Details
            lErrNumber = Informations.Err().Number
            sErrDescription = Informations.Err().Description

            ' Rollback The Transaction
            If bDBTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            ' What Sort of Error do we have

            Select Case lErrNumber
                ' A Back Office Error of some sort
                Case gPMConstants.PMEReturnCode.PMBackOfficeError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=TheOutput.Errors.BackOffice.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' A Business Rule Error
                Case gPMConstants.PMEReturnCode.PMBusinessRuleError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(TheOutput.Errors.BusinessRule.Detail.Code) & " " & TheOutput.Errors.BusinessRule.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' Just a normal VB  error
                Case Else

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ksFunctionName & " Failed - Internal Exception.", vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, vErrNo:=lErrNumber, vErrDesc:=sErrDescription, excep:=ex)

                    ' Add the Exception to the output
                    AddInternalExceptionError(ksFunctionName, CStr(lErrNumber) & " " & sErrDescription, TheOutput.Errors.InternalException)

            End Select

            ' Can dump out the contents of the XML to aid debugging if required



        Finally

            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are always to be returned
                ' none
            End With


            ' Serialise the Output so we can return it across the wire
            result = SerializeRiskScreenLoadRiskOut(oTypeOut:=TheOutput)

            ' Do any cleanup here
            ' You can log the exit point of the function for tracing if we need to.

            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            ' RAW 03/09/2004 : Resilience (#2) : added
            If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
                oBusiness = Nothing
            End If


        End Try
        Return result
    End Function


    '<XMLTransport method="RiskScreenOKClick">
    '<inputs>
    '   <property name="iTask" type="Integer"/>
    '   <property name="iSourceID" type="Integer"/>
    '   <property name="lNavigate" type="Long"/>
    '   <property name="lProcessMode" type="Long"/>
    '   <property name="sTransactionType" type="String"/>
    '   <property name="dtEffectiveDate" type="Date"/>
    '   <property name="bSubScreen" type="Boolean"/>
    '   <property name="lScreenId" type="Long"/>
    '   <property name="lRiskId" type="Long"/>
    '   <property name="lRiskTypeId" type="Long"/>
    '   <property name="sGisDataModelCode" type="String"/>
    '   <property name="lGISDataModelType" type="Long"/>
    '   <property name="lObjectType" type="Long"/>
    '   <property name="sGISXMLDataset" type="String"/>
    '   <property name="sMyOIKey" type="String"/>
    '   <property name="sMyObjectName" type="String"/>
    '   <property name="sParentOIKey" type="String"/>
    '   <property name="sParentObjectName" type="String"/>
    '   <property name="lPolicyLinkId" type="Long"/>
    '   <property name="lInsuranceFileCnt" type="Long"/>
    '   <property name="vScreenDetailsArray" type="Variant"/>
    '   <property name="vScreenValuesArray" type="Variant"/>
    '   <property name="vRiskDetailsArray" type="Variant"/>
    '   <property name="lTransactionType" type="Long"/>
    '   <property name="bPostQuote" type="Boolean"/>
    '   <property name="dtCoverStartDate" type="Date"/>
    '</inputs>
    '<outputs>
    '   <property name="sGISXMLDataset" type="String"/>
    '   <property name="vScreenValuesArray" type="Variant"/>
    '   <property name="vRiskDetailsArray" type="Variant"/>
    '   <property name="sReferReasons" type="String"/>
    '   <property name="sDeclineReasons" type="String"/>
    '   <property name="sMessages" type="String"/>
    '   <property name="lQuoteType" type="Long"/>
    '</outputs>
    '</XMLTransport>
    '*******************************************************************************
    ' Name: RiskScreenOKClick
    '
    ' Description: Put all database updates into a single business-side transaction,
    '              when OK clicking on a risk.
    '
    ' History : RAW110804 - created
    '           PW120804 - we need messages returned too
    ' RAW 03/09/2004 : Resilience (#2) : replaced parameters with XMLTransport string
    ' RAW 20/09/2004 : CQ6832 : added lQuoteType property to output structure
    '*******************************************************************************
    Public Function RiskScreenOKClick(ByVal v_sInput As String) As String

        Dim result As String = String.Empty
        Const ksFunctionName As String = "RiskScreenOKClick"

        Dim TheInput As XMLTransRiskScreenOKClick.RiskScreenOKClickIn = XMLTransRiskScreenOKClick.RiskScreenOKClickIn.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : added
        Dim TheOutput As XMLTransRiskScreenOKClick.RiskScreenOKClickOut = XMLTransRiskScreenOKClick.RiskScreenOKClickOut.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : replaced SIRRiskScreenBasicOut

        Dim oBusiness As Object = Nothing
        Dim oGIS As Object = Nothing

        Dim bDBTransStarted As Boolean
        Dim lReturn As Integer
        Dim sMsg As String = ""

        Dim vGISPropertyValue As Object = ""
        Dim bIsInsured, bTalkedToPerson As Boolean
        Dim lDisclosureTypeID As Object
        Dim lQuoteType As Integer

        Dim sReferReasons As String = ""
        Dim sDeclineReasons As String = ""
        Dim sMessages As String = ""
        'developer guide no. 101
        Dim vArray As Object = Nothing

        'Const ksTransTypeMTA As String = "MTA"

        Try


            ' DeSerialise the Input into the Structure
            ' =========================================
            ' RAW 03/09/2004 : Resilience (#2) : added
            TheInput = DeserializeRiskScreenOKClickIn(sXML:=v_sInput)


            With TheInput

                ' RAW 03/09/2004 : Resilience (#2) : added TheInput to replace parameter and module variables

                ' RAW 03/09/2004 : Resilience (#2) : added
                With TheOutput
                    ' set initial values for data items that are to be returned only when all is successful
                    .sGISXMLDataset = TheInput.sGISXMLDataset

                    If Informations.IsArray(TheInput.vScreenValuesArray) Then
                        .vScreenValuesArray = TheInput.vScreenValuesArray
                    Else
                        TheInput.vScreenValuesArray = Nothing
                    End If

                    If Informations.IsArray(TheInput.vRiskDetailsArray) Then
                        .vRiskDetailsArray = TheInput.vRiskDetailsArray
                    Else
                        TheInput.vRiskDetailsArray = Nothing
                    End If
                End With

                ' create business object
                oBusiness = New bSIRRiskScreen.Business
                oBusiness.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
                oBusiness.SetProcessModes(vTask:=ToSafeInteger(.iTask), vNavigate:=ToSafeInteger(.lNavigate), vProcessMode:=ToSafeInteger(.lProcessMode), vTransactionType:=ToSafeString(.sTransactionType), vEffectiveDate:=ToSafeDate(.dtEffectiveDate))

                If oBusiness Is Nothing Then
                    sMsg = "Failed to get business object "
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                ' create bGIS.Application
                oGIS = Nothing
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
                oGIS.SetProcessModes(vTask:=ToSafeInteger(TheInput.iTask), vNavigate:=ToSafeInteger(TheInput.lNavigate), vProcessMode:=ToSafeInteger(TheInput.lProcessMode), vTransactionType:=ToSafeString(TheInput.sTransactionType), vEffectiveDate:=ToSafeDate(TheInput.dtEffectiveDate))

                If oGIS Is Nothing Then
                    sMsg = "Failed to get GIS object "
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                ' Initialise the business component(s)

                lReturn = oGIS.LoadFromXML(v_sDataModelCode:=ToSafeString(TheInput.sGisDataModelCode), v_sXMLDataSet:=ToSafeString(TheInput.sGISXMLDataset))
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to load data into GIS"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                ' Start a DB transaction
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Start a Database Transaction"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                bDBTransStarted = True

                ' now do the stuff
                If Not .bPostQuote Then

                    ' "Validation"
                    lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeValidate ' RAW 20/09/2004 : CQ6832 : added

                    ' PW240804 - using 'stateful' GIS components so don't pass XML dataset
                    ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                    ' RAW 20/09/2004 : CQ6832 : added new params to handle output from NBQuote

                    lReturn = RunNBQuote(r_oGIS:=oGIS, v_lTransactionType:= .lTransactionType, v_iPBCQemQuoteType:=lQuoteType, v_sGisDataModelCode:= .sGisDataModelCode, v_lGISDataModelType:= .lGISDataModelType, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_sChildOIKey:= .sMyOIKey, r_vRiskDetailsArray:= .vRiskDetailsArray, r_sReferReasons:=sReferReasons, r_sDeclineReasons:=sDeclineReasons, r_sMessages:=sMessages, v_lInsuranceFileCnt:= .lInsuranceFileCnt)


                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMsg = "Failed to run NB Quote"
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                    End If

                    ' RAW 20/09/2004 : CQ6832 : moved code that handles output from NBQuote to within RunNBQuote


                    If sReferReasons <> "" Or sDeclineReasons <> "" Then
                        ' PW110804 - no error raised for this in original code, so don't do
                        ' it here. however, does set the function to PMFalse, so need to set a
                        ' flag in the XML output to indicate this.
                        ' RAW 23/08/2004 : Resilience : removed code that raised an error
                        With TheOutput
                            ' set data items that are always to be returned
                            .lQuoteType = lQuoteType ' RAW 20/09/2004 : CQ6832 : added
                            .sReferReasons = sReferReasons
                            .sDeclineReasons = sDeclineReasons
                            .sMessages = sMessages
                        End With

                        result = SerializeRiskScreenOKClickOut(oTypeOut:=TheOutput)
                        Return result
                    End If
                End If

                If Not .bSubScreen Then

                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20020904 - We should run the underwriting authority limits only for Underwriting System
                    '               Not for a broking system
                    ' HG13022004  - CQ4303 Don't run UA script for Client Screens
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    If m_bIsUnderwriting And .lGISDataModelType <> GISDataModelType.GISDMTypeParty Then

                        ' CTAF 20020809 start
                        If Not .bPostQuote Then

                            ' let's check the underwriting authority limits

                            ' "User Authority Limits"

                            lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeUal ' RAW 20/09/2004 : CQ6832 : added

                            ' AMB 10/07/2003: 1.9 IAG PS068 Date Effective Rating -
                            ' call RunNBQuote with the correct cover start date

                            If .dtCoverStartDate <> #12/30/1899# Then
                                ' pass in cover start date
                                ' PW240804 - using 'stateful' GIS components so don't pass XML dataset
                                ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                                ' RAW 20/09/2004 : CQ6832 : added new params to handle output from NBQuote

                                lReturn = RunNBQuote(r_oGIS:=oGIS, v_lTransactionType:= .lTransactionType, v_iPBCQemQuoteType:=lQuoteType, v_sGisDataModelCode:= .sGisDataModelCode, v_lGISDataModelType:= .lGISDataModelType, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_sChildOIKey:= .sMyOIKey, r_vRiskDetailsArray:= .vRiskDetailsArray, r_sReferReasons:=sReferReasons, r_sDeclineReasons:=sDeclineReasons, r_sMessages:=sMessages, v_dtCoverStartDate:= .dtCoverStartDate, v_lInsuranceFileCnt:= .lInsuranceFileCnt)

                            Else
                                ' run NB quote as per usual
                                ' PW240804 - using 'stateful' GIS components so don't pass XML dataset
                                ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                                ' RAW 20/09/2004 : CQ6832 : added new params to handle output from NBQuote

                                lReturn = RunNBQuote(r_oGIS:=oGIS, v_lTransactionType:= .lTransactionType, v_iPBCQemQuoteType:=lQuoteType, v_sGisDataModelCode:= .sGisDataModelCode, v_lGISDataModelType:= .lGISDataModelType, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_sChildOIKey:= .sMyOIKey, r_vRiskDetailsArray:= .vRiskDetailsArray, r_sReferReasons:=sReferReasons, r_sDeclineReasons:=sDeclineReasons, r_sMessages:=sMessages, v_lInsuranceFileCnt:= .lInsuranceFileCnt)
                            End If

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to run NB quote"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                            ' AMB 09/07/2003: 1.9 IAG PS068 Date Effective Rating - end


                            ' RAW 20/09/2004 : CQ6832 : moved code that handles output from NBQuote to within RunNBQuote

                        End If ' CTAF 20020908 END

                    End If ' m_bIsUnderwriting
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20020904 - END
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    ' Remember this is still a parent screen

                    'Always write it out...
                    ' RAW 20/09/2004 : CQ6832 : code moved to new function SaveToDB
                    lReturn = SaveToDB(r_oTheInput:=TheInput, r_oTheOutput:=TheOutput, r_oBusiness:=oBusiness, r_oGIS:=oGIS)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMsg = "Failed to save to the database"
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                    End If


                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20020904 - We should run the Auto Rating NBQuote only for Broking System
                    '               Not for a Underwriting system
                    ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    If Not m_bIsUnderwriting And .lGISDataModelType <> GISDataModelType.GISDMTypeParty Then ' Run this for Broking system only

                        If Not .bPostQuote Then ' Also check are we in PostQuote Screen ????

                            ' "Auto Rating"

                            lQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeQuote ' RAW 20/09/2004 : CQ6832 : added

                            ' PW240804 - using 'stateful' GIS components so don't pass XML dataset
                            ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                            ' RAW 20/09/2004 : CQ6832 : added new params to handle output from NBQuote

                            lReturn = RunNBQuote(r_oGIS:=oGIS, v_lTransactionType:= .lTransactionType, v_iPBCQemQuoteType:=lQuoteType, v_sGisDataModelCode:= .sGisDataModelCode, v_lGISDataModelType:= .lGISDataModelType, v_lScreenId:= .lScreenId, v_lRiskTypeId:= .lRiskTypeId, v_sChildOIKey:= .sMyOIKey, r_vRiskDetailsArray:= .vRiskDetailsArray, r_sReferReasons:=sReferReasons, r_sDeclineReasons:=sDeclineReasons, r_sMessages:=sMessages, v_lInsuranceFileCnt:= .lInsuranceFileCnt, v_lPartyCnt:= .lPartyCnt, v_dPolicyStartDate:= .dtPolicyStartDate, v_dPolicyEndDate:= .dtPolicyEndDate, v_lAgentCnt:= .lAgentCnt, v_lRiskCodeId:= .lRiskCodeId, v_lRiskGroupId:= .lRiskGroupId, v_lCountryId:= .lCountryId)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to run NB quote"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If

                            'Always write it out...
                            ' Save back to Database
                            ' RAW110804 replaced iGIS with bGIS
                            ' PW240804 - use the 'stateful' version of the GIS method
                            ' RAW 20/09/2004 : CQ6832 : replaced code with call to SaveToDB
                            lReturn = SaveToDB(r_oTheInput:=TheInput, r_oTheOutput:=TheOutput, r_oBusiness:=oBusiness, r_oGIS:=oGIS)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to save to the database"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If
                    End If

                Else
                    ' sub screen
                    If .lObjectType = GISDataModelType.GISOTDisclosure Then
                        ' disclosure screen

                        ' RAW110804 replaced iGIS with bGIS

                        lReturn = oGIS.GetPropertyValue(v_sObjectName:= .sParentObjectName, v_sPropertyName:="is_insured", v_sOIKey:= .sParentOIKey, r_vPropertyValue:=vGISPropertyValue)

                        bIsInsured = (Val(vGISPropertyValue) <> 0)

                        ' RAW110804 replaced iGIS with bGIS

                        lReturn = oGIS.GetPropertyValue(v_sObjectName:= .sMyObjectName, v_sPropertyName:="talked_to_person", v_sOIKey:= .sMyOIKey, r_vPropertyValue:=vGISPropertyValue)
                        bTalkedToPerson = (Val(vGISPropertyValue) <> 0)

                        ' RAW110804 replaced iGIS with bGIS

                        lReturn = oGIS.GetPropertyValue(v_sObjectName:= .sMyObjectName, v_sPropertyName:="disclosure_type", v_sOIKey:= .sMyOIKey, r_vPropertyValue:=vGISPropertyValue)
                        lDisclosureTypeID = CInt(Val(vGISPropertyValue))

                        If bTalkedToPerson And bIsInsured Then

                            ' PW281003 - CQ2772 - Update Risk Status'
                            ' PW141103 - CQ2772 - Pass disclosure type
                            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                            lReturn = oBusiness.UpdateRiskStatus_Stateless(lInsuranceFileCnt:= .lInsuranceFileCnt, lDisclosureTypeID:=lDisclosureTypeID)
                            ' should this also specify the party

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to update risk status'"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If
                    End If

                    'Handle standard wording at sub screen level

                    For lTemp As Integer = .vScreenDetailsArray.GetLowerBound(1) To .vScreenDetailsArray.GetUpperBound(1)

                        If CDbl(.vScreenDetailsArray(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOStdWordingType Then
                            'It's a standard wording


                            vArray = CObj(.vScreenValuesArray(lTemp))

                            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent



                            lReturn = oBusiness.UpdateStandardWording_Stateless(lPolicyLinkId:= .lPolicyLinkId, sDataModel:= .sGisDataModelCode, lGISPropertyID:= .vScreenDetailsArray(PBDatabaseConsts.ACDGISPropertyId, lTemp), lGISObjectID:= .vScreenDetailsArray(PBDatabaseConsts.ACDGISObjectId, lTemp), vStandardWordingArray:=vArray, sKeyName:= .sGisDataModelCode & "_" & .sMyObjectName & "_id", sKeyValue:=Mid(.sMyOIKey, 3))

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to update standard wording"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If
                    Next lTemp
                End If

                ' PW240804 - Refresh xml string with updated values from GIS

                lReturn = oGIS.ReturnAsXML(r_sXMLDataset:= .sGISXMLDataset)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to refresh the XML"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                ' Commit the DB transaction
                ' ============================
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseBackOfficeError(ksFunctionName, lReturn, "Failed to commit changes to the database.", TheOutput.Errors.BackOffice)
                Else
                    ' Our transaction is closed, don't close any others
                    bDBTransStarted = False
                End If

            End With


            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are to be returned only when all is successful
                .sGISXMLDataset = TheInput.sGISXMLDataset


                .vScreenValuesArray = TheInput.vScreenValuesArray


                .vRiskDetailsArray = TheInput.vRiskDetailsArray
            End With



        Catch ex As Exception

            ' Error Section.

            Dim lErrNumber As gPMConstants.PMEReturnCode
            Dim sErrDescription As String = ""

            ' Store the Error Details
            lErrNumber = Informations.Err().Number
            sErrDescription = Informations.Err().Description

            ' Rollback The Transaction
            If bDBTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            ' What Sort of Error do we have

            Select Case lErrNumber
                ' A Back Office Error of some sort
                Case gPMConstants.PMEReturnCode.PMBackOfficeError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=TheOutput.Errors.BackOffice.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' A Business Rule Error
                Case gPMConstants.PMEReturnCode.PMBusinessRuleError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(TheOutput.Errors.BusinessRule.Detail.Code) & " " & TheOutput.Errors.BusinessRule.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' Just a normal VB  error
                Case Else

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ksFunctionName & " Failed - Internal Exception.", vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, vErrNo:=lErrNumber, vErrDesc:=sErrDescription, excep:=ex)

                    ' Add the Exception to the output
                    AddInternalExceptionError(ksFunctionName, CStr(lErrNumber) & " " & sErrDescription, TheOutput.Errors.InternalException)

            End Select

            ' Can dump out the contents of the XML to aid debugging if required



        Finally

            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are always to be returned
                .lQuoteType = lQuoteType ' RAW 20/09/2004 : CQ6832 : added
                .sReferReasons = sReferReasons
                .sDeclineReasons = sDeclineReasons
                .sMessages = sMessages
            End With


            ' Serialise the Output so we can return it across the wire
            result = SerializeRiskScreenOKClickOut(oTypeOut:=TheOutput)

            ' Rollback The Transaction  PN21786
            If bDBTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            ' Do any cleanup here
            ' You can log the exit point of the function for tracing if we need to.

            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            ' RAW 03/09/2004 : Resilience (#2) : added
            If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
                oBusiness = Nothing
            End If


        End Try
        Return result
    End Function


    '<XMLTransport method="RiskScreenCancel">
    '<inputs>
    '   <property name="iTask" type="Integer"/>
    '   <property name="iSourceID" type="Integer"/>
    '   <property name="lNavigate" type="Long"/>
    '   <property name="lProcessMode" type="Long"/>
    '   <property name="sTransactionType" type="String"/>
    '   <property name="dtEffectiveDate" type="Date"/>
    '   <property name="bSubScreen" type="Boolean"/>
    '   <property name="lScreenId" type="Long"/>
    '   <property name="lRiskId" type="Long"/>
    '   <property name="sGisDataModelCode" type="String"/>
    '   <property name="lGISDataModelType" type="Long"/>
    '   <property name="lObjectType" type="Long"/>
    '   <property name="sGISXMLDataset" type="String"/>
    '   <property name="sMyOIKey" type="String"/>
    '   <property name="sMyObjectName" type="String"/>
    '   <property name="sParentOIKey" type="String"/>
    '   <property name="sParentObjectName" type="String"/>
    '   <property name="lPolicyLinkId" type="Long"/>
    '   <property name="bRevertBackRisk" type="Boolean"/>
    '   <property name="lInsuranceFileCnt" type="Long"/>
    '   <property name="bRiskAdded" type="Boolean"/>
    '   <property name="bRiskCopied" type="Boolean"/>
    '</inputs>
    '<outputs>
    '   <property name="sGISXMLDataset" type="String"/>
    '</outputs>
    '</XMLTransport>
    '*******************************************************************************
    ' Name: RiskScreenCancel
    '
    ' Description: Put all database updates into a single business-side transaction,
    '              when cancel clicking on a risk.
    '
    ' History : RAW110804 - created
    '           PW120804 - accept flag to indicate if a risk is being added
    ' RAW 03/09/2004 : Resilience (#2) : replaced parameters with XMLTransport string
    '*******************************************************************************
    Public Function RiskScreenCancel(ByVal v_sInput As String) As String

        Dim result As String = String.Empty
        Const ksFunctionName As String = "RiskScreenCancel"

        Dim TheInput As XMLTransRiskScreenCancel.RiskScreenCancelIn = XMLTransRiskScreenCancel.RiskScreenCancelIn.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : added
        Dim TheOutput As XMLTransRiskScreenCancel.RiskScreenCancelOut = XMLTransRiskScreenCancel.RiskScreenCancelOut.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : replaced SIRRiskScreenBasicOut

        Dim oBusiness As Object = Nothing
        Dim oGIS As Object = Nothing

        Dim bDBTransStarted As Boolean
        Dim lReturn As Integer
        Dim sMsg As String = ""
        Dim vGISPropertyValue As Object = ""
        Dim lPartyCnt, lUpdateStatus As Integer

        Try


            ' DeSerialise the Input into the Structure
            ' =========================================
            ' RAW 03/09/2004 : Resilience (#2) : added
            TheInput = DeserializeRiskScreenCancelIn(sXML:=v_sInput)


            With TheInput

                ' RAW 03/09/2004 : Resilience (#2) : added TheInput to replace parameter and module variables

                ' RAW 03/09/2004 : Resilience (#2) : added
                With TheOutput
                    ' set initial values for data items that are to be returned only when all is successful
                    .sGISXMLDataset = TheInput.sGISXMLDataset
                End With


                ' NOT VITAL _ DO LAST
                ' Validate input
                ' =================
                '    If v_lXXXX <= 0 Then
                '        sMsg = "Illegal xxxxxx passed to function - " & v_lXXXX
                '        AddInvalidDataError ksFunctionName, FieldName, MandatoryInputMissing, sMsg, v_lXXXX, TheOutput.Errors.BackOffice
                '    End If
                '    If TheOutput.HasInvalidDataErrors Then
                '        GoTo Err_RiskScreenLoadRisk
                '    End If

                ' All is OK so far


                ' RAW 03/09/2004 : Resilience (#2) : removed code that loaded module variables


                ' create objects
                ' =================
                ' RAW 03/09/2004 : Resilience (#2) : added
                ' create business object

                oBusiness = New bSIRRiskScreen.Business
                oBusiness.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
                oBusiness.SetProcessModes(vTask:=ToSafeInteger(.iTask), vNavigate:=ToSafeInteger(.lNavigate), vProcessMode:=ToSafeInteger(.lProcessMode), vTransactionType:=ToSafeString(.sTransactionType), vEffectiveDate:=ToSafeDate(.dtEffectiveDate))

                If oBusiness Is Nothing Then
                    sMsg = "Failed to get business object "
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If
                ' RAW 03/09/2004 : Resilience (#2) : end

                ' create bGIS.Application
                ' RAW110804 replaced iGIS with bGIS
                ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                oGIS = Nothing
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=oGIS, v_sClassName:="bGIS.Application", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bGIS.Application"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Application", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
                oGIS.SetProcessModes(vTask:=ToSafeInteger(.iTask), vNavigate:=ToSafeInteger(.lNavigate), vProcessMode:=ToSafeInteger(.lProcessMode), vTransactionType:=ToSafeString(.sTransactionType), vEffectiveDate:=ToSafeDate(.dtEffectiveDate))

                ' RAW 03/09/2004 : Resilience (#2) : added
                If oGIS Is Nothing Then
                    sMsg = "Failed to get GIS object "
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If


                ' Initialise the business component(s)
                ' ======================================================
                ' RAW 03/09/2004 : Resilience (#2) : removed code that sets business object properties


                lReturn = oGIS.LoadFromXML(v_sDataModelCode:= .sGisDataModelCode, v_sXMLDataSet:= .sGISXMLDataset)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to load data into GIS"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                ' Start a DB transaction
                ' ==============================
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Start a Database Transaction"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                bDBTransStarted = True

                ' now do the stuff
                ' ======================

                If Not .bSubScreen Then

                    ' top level screen
                    ' PW120804 - the bRiskAdd flag should be used to check if a risk
                    ' is being added, not the task
                    If .bRiskAdded Then
                        If .lGISDataModelType = GISDataModelType.GISDMTypeParty Then
                            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                            lReturn = oBusiness.DeleteGISPolicyLinkCancelledOnAdd_Stateless(v_sDataModelCode:= .sGisDataModelCode, v_lPolicyLinkId:=ToSafeInteger(.lPolicyLinkId))
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to delete GIS policy link for party data model"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        Else
                            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                            lReturn = oBusiness.DeleteRiskCancelledOnAdd_Stateless(v_lRiskId:=ToSafeInteger(.lRiskId))

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to delete risk"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If
                    End If



                    Select Case .iTask
                        Case gPMConstants.PMEComponentAction.PMEdit

                            If .bRevertBackRisk Then

                                ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                                lReturn = oBusiness.RevertBackRisk_Stateless(v_lScreenId:=ToSafeInteger(.lScreenId), v_lInsuranceFileCnt:=ToSafeInteger(.lInsuranceFileCnt), v_lRiskId:=ToSafeInteger(.lRiskId), v_sGISDataModel:= .sGisDataModelCode)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    sMsg = "Failed to revert back risk"
                                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                                End If
                            End If

                    End Select

                Else
                    ' subscreen

                    If .lObjectType = GISDataModelType.GISOTAssociatedClient Then
                        ' associated client screen

                        ' RAW110804 replaced iGIS with bGIS

                        lReturn = oGIS.GetPropertyValue(v_sObjectName:= .sMyObjectName, v_sPropertyName:="party_cnt", v_sOIKey:= .sMyOIKey, r_vPropertyValue:=vGISPropertyValue)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to get property value for party_cnt"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If

                        lPartyCnt = (Val(vGISPropertyValue) <> 0)

                        ' RAW110804 replaced iGIS with bGIS

                        lReturn = oGIS.GetPropertyValue(v_sObjectName:= .sMyObjectName, v_sPropertyName:="US", v_sOIKey:= .sMyOIKey, r_vPropertyValue:=vGISPropertyValue)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to get property value for US (update status)"
                            RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                        End If

                        lUpdateStatus = (Val(vGISPropertyValue) <> 0)

                        If lUpdateStatus = 1 Then
                            ' this screen added an object

                            ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                            lReturn = oBusiness.DeletePolicyClient_Stateless(lInsuranceFileCnt:=ToSafeInteger(.lInsuranceFileCnt), lPartyCnt:=ToSafeInteger(lPartyCnt))
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to delete the policy_client record"
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If
                    End If
                End If

                ' Commit the DB transaction
                ' ============================
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseBackOfficeError(ksFunctionName, lReturn, "Failed to commit changes to the database.", TheOutput.Errors.BackOffice)
                End If

            End With


            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are to be returned only when all is successful
                .sGISXMLDataset = TheInput.sGISXMLDataset
            End With


        Catch ex As Exception

            ' Error Section.

            Dim lErrNumber As gPMConstants.PMEReturnCode
            Dim sErrDescription As String = ""

            ' Store the Error Details
            lErrNumber = Informations.Err().Number
            sErrDescription = Informations.Err().Description

            ' Rollback The Transaction
            If bDBTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            ' What Sort of Error do we have

            Select Case lErrNumber
                ' A Back Office Error of some sort
                Case gPMConstants.PMEReturnCode.PMBackOfficeError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=TheOutput.Errors.BackOffice.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' A Business Rule Error
                Case gPMConstants.PMEReturnCode.PMBusinessRuleError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(TheOutput.Errors.BusinessRule.Detail.Code) & " " & TheOutput.Errors.BusinessRule.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' Just a normal VB  error
                Case Else

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ksFunctionName & " Failed - Internal Exception.", vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, vErrNo:=lErrNumber, vErrDesc:=sErrDescription, excep:=ex)

                    ' Add the Exception to the output
                    AddInternalExceptionError(ksFunctionName, CStr(lErrNumber) & " " & sErrDescription, TheOutput.Errors.InternalException)

            End Select

            ' Can dump out the contents of the XML to aid debugging if required



        Finally

            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are always to be returned
                ' none
            End With


            ' Serialise the Output so we can return it across the wire
            result = SerializeRiskScreenCancelOut(oTypeOut:=TheOutput)

            ' Do any cleanup here
            ' You can log the exit point of the function for tracing if we need to.

            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            ' RAW 03/09/2004 : Resilience (#2) : added
            If Not (oBusiness Is Nothing) Then

                oBusiness.Dispose()
                oBusiness = Nothing
            End If


        End Try
        Return result
    End Function


    '<XMLTransport method="RiskScreenLinkToParty">
    '<inputs>
    '   <property name="iTask" type="Integer"/>
    '   <property name="iSourceID" type="Integer"/>
    '   <property name="lNavigate" type="Long"/>
    '   <property name="lProcessMode" type="Long"/>
    '   <property name="sTransactionType" type="String"/>
    '   <property name="dtEffectiveDate" type="Date"/>
    '   <property name="lOldPartyId" type="Long"/>
    '   <property name="lNewPartyId" type="Long"/>
    '   <property name="lClaimID" type="Long"/>
    '</inputs>
    '<outputs>
    '</outputs>
    '</XMLTransport>
    '*******************************************************************************
    ' Name: RiskScreenLinkToParty
    '
    ' Description: Put all database updates into a single business-side transaction,
    '              when creating a party link on a risk.
    '
    ' History : RAW110804 - created
    ' RAW 03/09/2004 : Resilience (#2) : replaced parameters with XMLTransport string
    '*******************************************************************************
    Public Function RiskScreenLinkToParty(ByVal v_sInput As String) As String

        Dim result As String = String.Empty
        Const ksFunctionName As String = "RiskScreenLinkToParty"

        Dim TheInput As XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyIn = XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyIn.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : added
        Dim TheOutput As XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyOut = XMLTransRiskScreenLinkToParty.RiskScreenLinkToPartyOut.CreateInstance() ' RAW 03/09/2004 : Resilience (#2) : replaced SIRRiskScreenBasicOut

        Dim bDBTransStarted As Boolean
        Dim lReturn As Integer
        Dim sMsg As String = ""

        Dim oClaimPartyLink As Object = Nothing

        Try


            ' DeSerialise the Input into the Structure
            ' =========================================
            ' RAW 03/09/2004 : Resilience (#2) : added
            TheInput = DeserializeRiskScreenLinkToPartyIn(sXML:=v_sInput)


            With TheInput

                ' RAW 03/09/2004 : Resilience (#2) : added TheInput to replace parameter and module variables

                ' RAW 03/09/2004 : Resilience (#2) : added
                With TheOutput
                    ' set initial values for data items that are to be returned only when all is successful
                    ' none
                End With


                ' RAW 03/09/2004 : Resilience (#2) : moved
                ' As it is only for claims screens that we can update the database at the moment then get out of here if not claims
                If .lClaimID = 0 Then
                    result = SerializeRiskScreenLinkToPartyOut(oTypeOut:=TheOutput)
                    Return result
                End If

                ' RAW 03/09/2004 : Resilience (#2) : added
                If .iTask = gPMConstants.PMEComponentAction.PMView Then
                    result = SerializeRiskScreenLinkToPartyOut(oTypeOut:=TheOutput)
                    Return result
                End If


                ' Validate input
                ' =================
                Select Case .iTask
                    Case gPMConstants.PMEComponentAction.PMAdd
                        If .lNewPartyId = 0 Then
                            sMsg = "new party id is missing for an add "
                            AddInvalidDataError(ksFunctionName, ".lNewPartyId", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lNewPartyId, TheOutput.Errors.InvalidData)
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit
                        If .lOldPartyId = 0 Then
                            sMsg = "old party id is missing for an edit "
                            AddInvalidDataError(ksFunctionName, ".lOldPartyId", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lOldPartyId, TheOutput.Errors.InvalidData)
                        End If

                        If .lNewPartyId = 0 Then
                            sMsg = "new party id is missing for an edit "
                            AddInvalidDataError(ksFunctionName, ".lNewPartyId", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lNewPartyId, TheOutput.Errors.InvalidData)
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete
                        If .lOldPartyId = 0 Then
                            sMsg = "old party id is missing for a delete "
                            AddInvalidDataError(ksFunctionName, ".lOldPartyId", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lOldPartyId, TheOutput.Errors.InvalidData)
                        End If

                    Case Else
                        sMsg = "Illegal task value passed to function "
                        AddInvalidDataError(ksFunctionName, ".iTask", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .iTask, TheOutput.Errors.InvalidData)
                End Select

                If .lClaimID < 0 Then
                    sMsg = "Illegal claim id passed to function - " & .lClaimID
                    AddInvalidDataError(ksFunctionName, ".lClaimID", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lClaimID, TheOutput.Errors.InvalidData)
                End If

                If .lOldPartyId < 0 Then
                    sMsg = "Illegal old party id passed to function - " & .lOldPartyId
                    AddInvalidDataError(ksFunctionName, ".lOldPartyId", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lOldPartyId, TheOutput.Errors.InvalidData)
                End If

                If .lNewPartyId < 0 Then
                    sMsg = "Illegal new party id passed to function - " & .lNewPartyId
                    AddInvalidDataError(ksFunctionName, ".lNewPartyId", gPMConstants.PMEReturnCode.MandatoryInputMissing, sMsg, .lNewPartyId, TheOutput.Errors.InvalidData)
                End If


                ' All is OK so far


                ' RAW 03/09/2004 : Resilience (#2) : removed code that loaded module variables


                ' create objects
                ' =================
                ' create bCLMCLaimPartyLink.Business
                If .lClaimID <> 0 Then
                    ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
                    oClaimPartyLink = New bCLMClaimPartyLink.Business
                    oClaimPartyLink.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
                    oClaimPartyLink.SetProcessModes(vTask:=ToSafeInteger(.iTask), vNavigate:=ToSafeInteger(.lNavigate), vProcessMode:=ToSafeInteger(.lProcessMode), vTransactionType:=ToSafeString(.sTransactionType), vEffectiveDate:=ToSafeDate(.dtEffectiveDate))

                    ' RAW 03/09/2004 : Resilience (#2) : added
                    If oClaimPartyLink Is Nothing Then
                        sMsg = "Failed to get ClaimPartyLink object "
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                    End If

                End If

                ' Initialise the business component(s)
                ' ======================================================
                ' none


                ' Start a DB transaction
                ' ==============================
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to Start a Database Transaction"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                End If

                bDBTransStarted = True

                ' now do the stuff
                ' ======================

                ' Do we need to delete a link

                Select Case .iTask
                    Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMEdit
                        If Not (oClaimPartyLink Is Nothing) Then
                            ' remove it from the claim party link table

                            lReturn = oClaimPartyLink.DeleteClaimPartyLink(v_lClaimPartyId:=ToSafeInteger(.lOldPartyId), v_lClaimID:=ToSafeInteger(.lClaimID))

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to remove claim party link " &
                                       " for party:" & CStr(.lOldPartyId) &
                                       " claim id:" & CStr(.lClaimID)
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If

                End Select

                ' Do we need to add a link

                Select Case .iTask
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit

                        If Not (oClaimPartyLink Is Nothing) Then
                            ' add the new claim party link

                            lReturn = oClaimPartyLink.AddClaimPartyLink(v_lClaimPartyId:=ToSafeInteger(.lNewPartyId), v_lClaimID:=ToSafeInteger(.lClaimID))

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                sMsg = "Failed to add claim party link " &
                                       " for party:" & CStr(.lNewPartyId) &
                                       " claim id:" & CStr(.lClaimID)
                                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, TheOutput.Errors.BackOffice)
                            End If
                        End If

                End Select

                ' Commit the DB transaction
                ' ============================
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseBackOfficeError(ksFunctionName, lReturn, "Failed to commit changes to the database.", TheOutput.Errors.BackOffice)
                End If

            End With


            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are to be returned only when all is successful
                ' none
            End With


        Catch ex As Exception

            ' Error Section.

            Dim lErrNumber As gPMConstants.PMEReturnCode
            Dim sErrDescription As String = ""

            ' Store the Error Details
            lErrNumber = Informations.Err().Number
            sErrDescription = Informations.Err().Description

            ' Rollback The Transaction
            If bDBTransStarted Then
                lReturn = m_oDatabase.SQLRollbackTrans()
            End If

            ' What Sort of Error do we have

            Select Case lErrNumber
                ' A Back Office Error of some sort
                Case gPMConstants.PMEReturnCode.PMBackOfficeError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=TheOutput.Errors.BackOffice.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' A Business Rule Error
                Case gPMConstants.PMEReturnCode.PMBusinessRuleError

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=CStr(TheOutput.Errors.BusinessRule.Detail.Code) & " " & TheOutput.Errors.BusinessRule.Detail.Description, vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, excep:=ex)

                    ' Just a normal VB  error
                Case Else

                    ' Log Error.
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ksFunctionName & " Failed - Internal Exception.", vApp:=ACApp, vClass:=ACClass, vMethod:=ksFunctionName, vErrNo:=lErrNumber, vErrDesc:=sErrDescription, excep:=ex)

                    ' Add the Exception to the output
                    AddInternalExceptionError(ksFunctionName, CStr(lErrNumber) & " " & sErrDescription, TheOutput.Errors.InternalException)

            End Select

            ' Can dump out the contents of the XML to aid debugging if required



        Finally

            ' RAW 03/09/2004 : Resilience (#2) : added
            With TheOutput
                ' set data items that are always to be returned
                ' none
            End With


            ' Serialise the Output so we can return it across the wire
            result = SerializeRiskScreenLinkToPartyOut(oTypeOut:=TheOutput)

            ' Do any cleanup here
            ' You can log the exit point of the function for tracing if we need to.
            oClaimPartyLink = Nothing


        End Try
        Return result
    End Function


    ' RAW 03/09/2004 : Resilience (#2) : removed SetProcessModes function


    '******************************************************************************
    ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
    '******************************************************************************
    Private Function GetObject(ByVal v_sClassName As String, ByVal v_iTask As Integer, ByVal v_lNavigate As Integer, ByVal v_lProcessMode As Integer, ByVal v_sTransactionType As String, ByVal v_dtEffectiveDate As Date) As Object

        Dim result As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sMsg As String = ""
        Dim oObject As Object = Nothing
        Dim lErrorNumber As Integer


        lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMsg = "Failed to create instance of " & v_sClassName
            Throw New System.Exception(gPMConstants.PMEReturnCode.PMBackOfficeError.ToString() + ", " + ACApp & "." & ACClass + ", " + sMsg)
        End If

        result = oObject

        ' RAW 18/08/2004 : Resilience : added

        Try  ' temporarily disable error handler in case the object does not support this function

            lReturn = oObject.SetProcessModes(vTask:=ToSafeInteger(v_iTask), vNavigate:=ToSafeInteger(v_lNavigate), vProcessMode:=ToSafeInteger(v_lProcessMode), vTransactionType:=ToSafeInteger(v_sTransactionType), vEffectiveDate:=ToSafeDate(v_dtEffectiveDate))

            lErrorNumber = Informations.Err().Number


            If lErrorNumber = 0 Then
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to set the process modes for object " & v_sClassName
                    Throw New System.Exception(gPMConstants.PMEReturnCode.PMBackOfficeError.ToString() + ", " + ACApp & "." & ACClass + ", " + sMsg)
                End If
            End If
            ' RAW 18/08/2004 : Resilience : end

            GoTo Exit_GetObject


Err_GetObject:

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetObject", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + Informations.Err().Source + ", " + Informations.Err().Description + ", " + Informations.Err().HelpFile + ", " + Informations.Err().HelpContext)

Exit_GetObject:

            Return result




        Catch exc As System.Exception

        End Try
    End Function


    '******************************************************************************
    '
    ' Name: RunNBQuote
    '
    ' Description:
    '
    ' History: 05/07/2002 CLG - Created.
    '
    ' AMB 10/07/2003: 1.9 IAG PS068 Date Effective Rating - added v_dtCoverStartDate param
    ' RVH 7/8/2003    IAG - Create additional data array and pass to NBQuote
    ' PW240804 - remove XML dataset string param - this function will now use the
    '            'stateful' versions of GIS methods.
    ' RAW 03/09/2004 : Resilience (#2) : added params to replace module variables
    ' RAW 20/09/2004 : CQ6832 : moved code from RiskScreenOKClick to handle output from NBQuote and added new params to support this
    ' CJB 13/01/2006 : PN26790 Changed RunNBQuote to check UseRiskTypeID reg setting at GIS level if not already found
    '******************************************************************************
    Private Function RunNBQuote(ByRef r_oGIS As Object, ByVal v_lTransactionType As Integer, ByVal v_iPBCQemQuoteType As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISDataModelType As Integer, ByVal v_lScreenId As Integer, ByVal v_lRiskTypeId As Integer, ByVal v_sChildOIKey As String, Optional ByRef r_vRiskDetailsArray(,) As Object = Nothing, Optional ByRef r_sReferReasons As Object = "", Optional ByRef r_sDeclineReasons As Object = "", Optional ByRef r_sMessages As Object = "", Optional ByVal v_dtCoverStartDate As Date = #1/1/1900#, Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_dPolicyStartDate As Date = #1/1/1900#, Optional ByVal v_dPolicyEndDate As Date = #1/1/1900#, Optional ByVal v_lAgentCnt As Integer = 0, Optional ByVal v_lRiskCodeId As Integer = 0, Optional ByVal v_lRiskGroupId As Integer = 0, Optional ByVal v_lCountryId As Integer = 0) As Integer

        ' AMB 10/07/2003: 1.9 IAG PS068 - v_dtCoverStartDate is only passed in
        ' when transaction is MTA and we're running the UAL script - see Update function

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RunNBQuote")

        Dim lQuoteType As Integer 'encoded quote type value
        Dim lRiskTypeId As Integer ' SFB=RiskTypeId SFU=-1
        Dim lReturn As gPMConstants.PMEReturnCode
        ' RVH 7/7/2003 : IAG - START: Create additional data array to be passed down
        Dim vArray As Object
        Dim sUseRiskType As String = ""
        Dim lPolicyTypeID, lLoop1 As Integer

        Const KeyChildOIKey As String = "CHILD_OIKEY"
        ' RVH 7/7/2003 : IAG - END: Create additional data array to be passed down



        result = gPMConstants.PMEReturnCode.PMTrue

        PBQuoteTypeEncode.EncodeTransactionScreenAndType(lQuoteType, v_lTransactionType, v_lScreenId, v_iPBCQemQuoteType)

        lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel(v_sGisDataModelCode, GISSharedConstants.GISRegUseRiskTypeID, sUseRiskType), gPMConstants.PMEReturnCode)
        If lReturn = gPMConstants.PMEReturnCode.PMTrue And sUseRiskType = "" Then
            'check if it is set at GIS level   PN26790
            lReturn = CType(GISSharedConstants.GetRegSettingFromDataBusModel("", GISSharedConstants.GISRegUseRiskTypeID, sUseRiskType), gPMConstants.PMEReturnCode)
        End If
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If sUseRiskType = "Y" Then
            lRiskTypeId = v_lRiskTypeId
        Else
            lRiskTypeId = -1
        End If

        If Not m_bIsUnderwriting Then
            ' Get the policy type as it is rqd in bGIS in order to decide what processing to do PN27045
            ' Note that if we pass v_lInsuranceFileCnt of zero to this then it logs error but returns ok
            lReturn = CType(GetPolicyTypeIDForInsFile(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lPolicyTypeID:=lPolicyTypeID), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' RVH 7/7/2003 : IAG - START: Create additional data array to be passed down
        ReDim vArray(1, 2)

        vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = KeyChildOIKey

        vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_sChildOIKey

        vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "DATA_MODEL_TYPE"

        vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_lGISDataModelType

        vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "POLICY_TYPE_ID"

        vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = lPolicyTypeID

        If v_lPartyCnt <> 0 Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Party_Cnt"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_lPartyCnt
        End If

        If v_dPolicyStartDate <> #1/1/1900# Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Policy_Start_Date"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_dPolicyStartDate
        End If

        If v_dPolicyEndDate <> #1/1/1900# Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Policy_End_Date"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_dPolicyEndDate
        End If

        If v_lAgentCnt <> 0 Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Agent_Cnt"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_lAgentCnt
        End If

        If v_lRiskCodeId <> 0 Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Risk_Code_Id"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_lRiskCodeId
        End If

        If v_lRiskGroupId <> 0 Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Risk_Group_Id"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_lRiskGroupId
        End If

        If v_lCountryId <> 0 Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "Country_Id"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_lCountryId
        End If

        If v_lInsuranceFileCnt <> 0 Then

            lLoop1 = vArray.GetUpperBound(1) + 1
            ReDim Preserve vArray(1, lLoop1)


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, vArray.GetUpperBound(1)) = "insurance_file_cnt"


            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, vArray.GetUpperBound(1)) = v_lInsuranceFileCnt
        End If

        ' clear quote output except Data Model Type is = 4 (Party)

        If v_lGISDataModelType <> 4 Then
            lReturn = ClearOutputDetails(r_oGISRisk:=r_oGIS.Risk, v_sGisDataModelCode:=v_sGisDataModelCode)
        End If

        ' AMB 10/07/2003: 1.9 IAG PS068 Date Effective Rating - use the appropriate cover start date
        If v_dtCoverStartDate = #1/1/1900# Then
            ' RVH 7/7/2003 : IAG : Pass additional data array
            ' RAW110804 replaced iGIS with bGIS
            ' PW240804 - use 'stateful' version of GIS NBQuote method

            lReturn = r_oGIS.NBQuoteStateful(v_lQuoteType:=ToSafeInteger(lQuoteType), v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=DateTime.Today, v_lRiskGroupId:=ToSafeInteger(lRiskTypeId), r_vAdditionalDataArray:=vArray)
        Else
            ' RVH 7/7/2003 : IAG : Pass additional data array
            ' RAW110804 replaced iGIS with bGIS
            ' PW240804 - use 'stateful' version of GIS NBQuote method

            lReturn = r_oGIS.NBQuoteStateful(v_lQuoteType:=ToSafeInteger(lQuoteType), v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=ToSafeDate(v_dtCoverStartDate), v_lRiskGroupId:=ToSafeInteger(lRiskTypeId), r_vAdditionalDataArray:=vArray)
        End If

        ' RAW 08/09/2003 : CQ2377 : added
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="NBQuote Failed for " & PBQuoteTypeEncode.GetQuoteTypeDesc(v_iPBCQemQuoteType), vApp:=ACApp, vClass:=ACClass, vMethod:="RunNBQuote")
            Return result
        End If


        ' RAW 20/09/2004 : CQ6832 : code moved to here from RiskScreenOKClick
        ' RAW110804 replaced iGIS with bGIS
        ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables

        lReturn = GetOutputDetails(r_oGISRisk:=r_oGIS.Risk, v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), r_sReferReasons:=r_sReferReasons, r_sDeclineReasons:=r_sDeclineReasons, r_sMessages:=r_sMessages)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = lReturn
            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get output details", vApp:=ACApp, vClass:=ACClass, vMethod:="RunNBQuote")
            Return result
        End If

        '-------------------------------------------------------------------------------------
        '   18/07/2002  RVH BEGIN
        '                   Only do this if the GIS data model type is RISK
        '-------------------------------------------------------------------------------------

        If Not Informations.IsNothing(r_vRiskDetailsArray) Then
            If Informations.IsArray(r_vRiskDetailsArray) Then

                If v_lGISDataModelType = GISDataModelType.GISDMTypeRisk Then

                    r_vRiskDetailsArray(ACRRiskStatusId, 0) = 4 'Unquoted

                    If r_sReferReasons <> "" Then

                        r_vRiskDetailsArray(ACRRiskStatusId, 0) = 1 'Referred
                    End If

                    If r_sDeclineReasons <> "" Then

                        r_vRiskDetailsArray(ACRRiskStatusId, 0) = 2 'Declined
                    End If
                End If
            End If
        End If
        '-------------------------------------------------------------------------------------
        '   18/07/2002  RVH END
        '-------------------------------------------------------------------------------------
        ' RAW 20/09/2004 : CQ6832 : end

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RunNBQuote")

        Return result

    End Function


    '******************************************************************************
    '
    ' Name: SaveToDB
    '
    ' Description:
    '
    ' History:
    ' RAW 20/09/2004 : CQ6832 : created by moving code from RiskScreenOKClick
    '******************************************************************************
    Private Function SaveToDB(ByRef r_oTheInput As XMLTransRiskScreenOKClick.RiskScreenOKClickIn, ByRef r_oTheOutput As XMLTransRiskScreenOKClick.RiskScreenOKClickOut, ByRef r_oBusiness As Object, ByRef r_oGIS As Object) As Integer

        Dim result As Integer = 0
        Const ksFunctionName As String = "SaveToDB"
        Dim lReturn As Integer
        Dim sMsg As String = ""
        'developer guide no. 101
        Dim vArray As Object = Nothing
        Dim lSumInsuredType As Object
        Dim sKeyName, sKeyValue As Object

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SaveToDB")




        result = gPMConstants.PMEReturnCode.PMTrue


        ' RAW 20/09/2004 : CQ6832 : moved code from RiskScreenOKClick
        With r_oTheInput

            ' RAW110804 replaced iGIS with bGIS
            ' PW240804 - use the 'stateful' version of the GIS method

            lReturn = r_oGIS.SaveToDBStateful

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMsg = "Failed to save the dataset to the database"
                RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, r_oTheOutput.Errors.BackOffice)
            End If

            ' MAW 03/09/2003 CW2183 Following block moved from commented out oGIS.SaveToDB earlier in this function


            For lTemp As Integer = .vScreenDetailsArray.GetLowerBound(1) To .vScreenDetailsArray.GetUpperBound(1)
                'GSD

                If gPMFunctions.ToSafeDouble(.vScreenDetailsArray(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOSumInsuredTypeID Then
                    'It's a sum insured


                    vArray = .vScreenValuesArray(lTemp)


                    lSumInsuredType = gPMFunctions.ToSafeInteger(.vScreenDetailsArray(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                    ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                    lReturn = r_oBusiness.UpdateSumInsured_Stateless(lPolicyLinkId:= .lPolicyLinkId, sDataModel:= .sGisDataModelCode, lSumInsuredType:=lSumInsuredType, vSumInsuredArray:=vArray)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMsg = "Failed to update sum insured"
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, r_oTheOutput.Errors.BackOffice)
                    End If

                ElseIf gPMFunctions.ToSafeDouble(.vScreenDetailsArray(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOStdWordingType Then
                    'It's a standard wording


                    vArray = .vScreenValuesArray(lTemp)

                    If .bSubScreen Then
                        sKeyName = .sGisDataModelCode & "_" & .sParentObjectName & "_id"
                        sKeyValue = Mid(.sMyOIKey, 3)
                    Else
                        sKeyName = ""
                        sKeyValue = ""
                    End If

                    ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent



                    lReturn = r_oBusiness.UpdateStandardWording_Stateless(lPolicyLinkId:= .lPolicyLinkId, sDataModel:= .sGisDataModelCode, lGISPropertyID:= .vScreenDetailsArray(PBDatabaseConsts.ACDGISPropertyId, lTemp), lGISObjectID:= .vScreenDetailsArray(PBDatabaseConsts.ACDGISObjectId, lTemp), vStandardWordingArray:=vArray, sKeyName:=sKeyName, sKeyValue:=sKeyValue)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMsg = "Failed to update standard wording"
                        RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, r_oTheOutput.Errors.BackOffice)
                    End If

                End If

            Next lTemp

            If .lGISDataModelType = GISDataModelType.GISDMTypeRisk Then
                lReturn = r_oBusiness.DeleteUnusedEditedStandardWording()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' MAW 03/09/2003 CW2183 End block move


            '-------------------------------------------------------------------------------------
            '   18/07/2002  RVH BEGIN
            '                   Only do this if the GIS data model type is RISK
            '-------------------------------------------------------------------------------------
            If .lGISDataModelType = GISDataModelType.GISDMTypeRisk Then

                ' HG 13/10/2003 : CQ2654 : added

                lReturn = UpdateRiskArrayFromGIS(r_oGIS:=r_oGIS, v_sGisDataModelCode:= .sGisDataModelCode, r_vRiskDetailsArray:= .vRiskDetailsArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to update risk from GIS"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, r_oTheOutput.Errors.BackOffice)
                End If
                ' HG 13/10/2003 : CQ2654 : end

                ' RAW 03/09/2004 : Resilience (#2) : replaced function call with call to stateless equivalent

                lReturn = r_oBusiness.UpdateRisk_Stateless(vRiskArray:= .vRiskDetailsArray, r_lRiskId:= .lRiskId)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMsg = "Failed to update risk"
                    RaiseBackOfficeError(ksFunctionName, lReturn, sMsg, r_oTheOutput.Errors.BackOffice)
                End If
            End If
        End With

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SaveToDB")

        Return result

    End Function


    '******************************************************************************
    ' Name:         UpdateRiskArrayFromGIS
    ' Description:
    ' History:
    '  HG  13/10/2003 : CQ2654 : created
    '  RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
    '  RAW 20/09/2004 : CQ6832 : renamed for clarity
    '  TR  10/12/2004 : Updated with 1.8.6 fix for accumulations PN1650
    '******************************************************************************
    Public Function UpdateRiskArrayFromGIS(ByRef r_oGIS As Object, ByVal v_sGisDataModelCode As String, ByRef r_vRiskDetailsArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "UpdateRiskArrayFromGIS"
        Dim sParentObjectName As Object = ""
        Dim vChildObjects As Object = Nothing
        Dim vTopLevelObjectOIKeys As Object = Nothing
        Dim vChildOIKey As Object = Nothing
        'developer guide no.17 
        Dim vValue As Object = Nothing
        Dim bAssumedInfo As Boolean
        Dim vFieldArray As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If r_vRiskDetailsArray Is Nothing Then
                Return result
            End If

            ' In the format: Gis field name, Risk table field name, data type


            vFieldArray = New Object() {"DESCRIPTION", "ACCUMULATION", "EML_PERCENTAGE", "COVERAGE", "INSURED_ITEM", "EXTENSIONS", "PACKAGE", "NCD", "EXCESS"}

            sParentObjectName = v_sGisDataModelCode & "_policy_binder"

            ' RAW110804 replaced iGIS with bGIS

            lReturn = r_oGIS.GetAllOIKey(v_sObjectName:=sParentObjectName, r_vOIKeyArray:=vTopLevelObjectOIKeys)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAllOIKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                Return result
            End If

            ' RAW110804 replaced iGIS with bGIS

            lReturn = r_oGIS.GetObjectDefDetails(v_sObjectName:=ToSafeString(sParentObjectName), r_vChildObjectArray:=vChildObjects)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetObjectDefDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                Return result
            End If

            'normally you'd expect only one but for completeness you should I suppose loop around the results

            For lChildObjectCount As Integer = 0 To vChildObjects.GetUpperBound(0)


                If CStr(vChildObjects(lChildObjectCount)).ToUpper() <> v_sGisDataModelCode.ToUpper() & "_OUTPUT" Then

                    ' RAW110804 replaced iGIS with bGIS


                    lReturn = r_oGIS.GetAllOIKey(v_sObjectName:=CStr(vChildObjects(lChildObjectCount)), r_vOIKeyArray:=vChildOIKey)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                        oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
                        gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetAllOIKey(2) Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                        Return result
                    End If

                    'for each instance of the object (there may only be 1 instance of each top level object)
                    If Informations.IsArray(vChildOIKey) Then

                        For lChildObjectInstance As Integer = 0 To vChildOIKey.GetUpperBound(0)


                            For iFieldCnt As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                                ' RAW110804 replaced iGIS with bGIS




                                lReturn = r_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjects(lChildObjectCount)), v_sPropertyName:=CStr(vFieldArray(iFieldCnt)), v_sOIKey:=CStr(vChildOIKey(lChildObjectInstance)), r_vPropertyValue:=vValue, r_bIsAssumedInfo:=ToSafeBoolean(bAssumedInfo))
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                                    oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
                                    gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetPropertyValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                                    Return result
                                End If


                                'developer guide no. Change as per vb code
                                If Not vValue Is DBNull.Value AndAlso Not String.IsNullOrEmpty(vValue) Then

                                    Select Case CStr(vFieldArray(iFieldCnt)).ToUpper()
                                        Case "DESCRIPTION"

                                            r_vRiskDetailsArray(ACRDescription, 0) = vValue
                                        Case "ACCUMULATION"
                                            If vValue <> "0" Then

                                                r_vRiskDetailsArray(ACRAccumulationId, 0) = vValue
                                            End If
                                        Case "EML_PERCENTAGE"

                                            r_vRiskDetailsArray(ACREMLPercentage, 0) = vValue
                                        Case "COVERAGE"

                                            r_vRiskDetailsArray(ACRCoverage, 0) = vValue
                                        Case "INSURED_ITEM"

                                            r_vRiskDetailsArray(ACRInsuredItem, 0) = vValue
                                        Case "EXTENSIONS"

                                            r_vRiskDetailsArray(ACRExtensions, 0) = vValue
                                    End Select

                                End If

                            Next
                        Next
                    End If
                End If
            Next

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_sGisDataModelCode", v_sGisDataModelCode)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    Private Function ClearOutputDetails(ByRef r_oGISRisk As Object, ByVal v_sGisDataModelCode As String) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iOutputObjectCount As Integer : Dim GIS_OutputObjectName As String = "OUTPUT"



        result = gPMConstants.PMEReturnCode.PMTrue

        'DN 17/12/02 - ISS 1617 - Don't clear down output if Broking
        'Some output properties may be needed for documentation (eg Endorsements)
        If Not m_bIsUnderwriting Then
            Return result
        End If

        With r_oGISRisk

            'this expects "OUTPUT" , nearly everything else expects "DataModelCode_OUTPUT" so check for both

            iOutputObjectCount = .Count(v_sGisDataModelCode & "_" & GIS_OutputObjectName)
            If iOutputObjectCount > 0 Then
                GIS_OutputObjectName = v_sGisDataModelCode & "_" & GIS_OutputObjectName
            Else

                iOutputObjectCount = .Count(ToSafeString(GIS_OutputObjectName))
            End If

            If iOutputObjectCount > 0 Then
                ' We have some output object
                ' So use the DeleteObject Method to clear the output object
                For iCounter As Integer = 1 To iOutputObjectCount Step 1

                    .Item(ToSafeString(GIS_OutputObjectName), 1).DeleteObject()
                Next iCounter
            End If

        End With

        Return result

    End Function

    ' RAW 03/09/2004 : Resilience (#2) : added extra params to replace module variables
    Private Function GetOutputDetails(ByRef r_oGISRisk As Object, ByVal v_sGisDataModelCode As String, Optional ByRef r_sDeclineReasons As String = "", Optional ByRef r_sReferReasons As String = "", Optional ByRef r_sMessages As String = "") As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim vDeclineReason As String = ""
        Dim vReferReason As String = ""
        Dim vMessage As String = ""
        Dim iOutputObjectCount As Integer : Dim GIS_OutputObjectName As String = "OUTPUT"



        result = gPMConstants.PMEReturnCode.PMTrue

        r_sReferReasons = ""
        r_sDeclineReasons = ""
        'Keep this one - we may be here in UA after Validation
        '    r_sMessages = ""


        With r_oGISRisk

            'this expects "OUTPUT", nearly everything else expects "DataModelCode_OUTPUT" so check for both

            iOutputObjectCount = .Count(v_sGisDataModelCode & "_" & GIS_OutputObjectName)
            If iOutputObjectCount > 0 Then
                GIS_OutputObjectName = v_sGisDataModelCode & "_" & GIS_OutputObjectName
            Else

                iOutputObjectCount = .Count(ToSafeString(GIS_OutputObjectName))
            End If

            If iOutputObjectCount > 0 Then
                ' We have some output object
                ' Check the properties from each of the output object
                For iCounter As Integer = 1 To iOutputObjectCount Step 1


                    vDeclineReason = .Item(ToSafeString(GIS_OutputObjectName), ToSafeInteger(iCounter)).Item("decline_reason").value

                    vReferReason = .Item(ToSafeString(GIS_OutputObjectName), ToSafeInteger(iCounter)).Item("refer_reason").value

                    vMessage = .Item(ToSafeString(GIS_OutputObjectName), ToSafeInteger(iCounter)).Item("Message").value

                    If vDeclineReason <> "" Then
                        r_sDeclineReasons = r_sDeclineReasons & vDeclineReason & " <br />"
                    End If

                    If vReferReason <> "" Then
                        ' PW120804 - use correct string value
                        r_sReferReasons = r_sReferReasons & vReferReason & Strings.ChrW(13) & Strings.ChrW(10)
                    End If

                    If vMessage <> "" Then
                        ' PW120804 - use correct string value
                        r_sMessages = r_sMessages & vMessage & Strings.ChrW(13) & Strings.ChrW(10)
                    End If

                Next iCounter
            End If

        End With

        Return result

    End Function



    '******************************************************************************
    ' Name:         GetPolicyTypeIDForInsFile
    ' Description:  Get the policy type id given the ins file cnt
    ' History:
    ' CJB 260106 PN27045 Created
    '******************************************************************************
    Public Function GetPolicyTypeIDForInsFile(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPolicyTypeID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lReturn As gPMConstants.PMEReturnCode
            Dim vResultArray(,) As Object = Nothing

            If v_lInsuranceFileCnt = 0 Then
                ' Log error but do not return error return code as we are not sure if this scenario
                ' may be valid for some passes thru this component
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyTypeIDForInsFile Failed as v_lInsuranceFileCnt is zero!", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTypeIDForInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'developer guide no 39. 
                lReturn = .SQLSelect(sSQL:="spu_Get_Policy_Type_ID", sSQLName:="GetPolicyTypeIDForInsFile", bStoredProcedure:=True, vResultArray:=vResultArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Or CStr(vResultArray(0, 0)) = "" Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction (spu_Get_Policy_Type_ID) failed. m_lReturn:" & lReturn & " , v_lInsuranceFileCnt:" & CStr(v_lInsuranceFileCnt), vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTypeIDForInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                r_lPolicyTypeID = CInt(vResultArray(0, 0))

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyTypeIDForInsFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyTypeIDForInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
End Class
