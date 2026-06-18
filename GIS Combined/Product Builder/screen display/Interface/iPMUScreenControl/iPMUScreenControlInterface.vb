Option Strict Off
Option Explicit On
Imports System
Imports System.Globalization

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    '******************************************************************************
    ' Class Name: Interface
    '
    ' Date: 09/06/1999
    '
    ' Edit History:
    ' CJB 08/03/2005 : PN19313 For new renewal what-if quotes only, if the quote has not been saved and the user is
    '                  exiting, delete the quote and the policy version. Added new function - DeleteQuote - to do
    '                  this. This is called from uctRiskScreenControl.ocx when necessary.
    ' CJB 21/10/2005 : PN24176 Changed SetKeys to save KeyArray so it can be passed onto child
    '                  sceens in DisplaySubscreen so that the dynamic logic in there has access to it.
    ' CJB 13/01/2006 : PN26792 Changed Initialise to check UseRiskTypeID reg setting at
    '                  GIS level if not already found (as is done in bSIRRiskScreen)
    '******************************************************************************
    Private g_oObjectManager As bObjectManager.ObjectManager
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_iSourceId As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lChildIndex As Integer 'index number of child record beinf added (used as count var in XML)
    Private m_lClaimId As Integer
    Private m_lClaimPerilID As Integer
    Private m_lPerilID As Integer
    Private m_sClaimTransactionType As String = ""
    Private m_lClaimInsFileCnt As Integer
    Private m_lClaimRiskId As Integer
    Private m_oGIS As Object
    Private m_oGISSellerTool As Object
    Private m_lCaseID As Integer

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Declare an instance of the Lock object.

    'Private m_oPMLock As Object
    Private m_lPartyCnt As Integer
    Private m_sShortName As String = ""
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskId As Integer
    Private m_lRiskTypeId As Integer
    Private m_lProductId As Integer
    Private m_bLossSchedule As Boolean
    Private m_lLossScheduleTypeId As Integer
    Private m_lPerilTypeId As Integer
    Private m_lPolicyLinkId As Integer
    Private m_lPolicyBinderId As Integer
    Private m_lScreenId As Integer
    Private m_sScreen As String = ""
    Private m_lGISDataModelId As Integer
    Private m_sGISDataModel As String = ""
    Private m_lObjectType As Integer 'Risk , specials, associated client etc
    'Store the GIS Data Model Type
    Private m_lGISDataModelType As Integer
    Private m_bEvent As Boolean

    Private m_vScreenDetails(,) As Object
    Private m_vChildScreenDetails(,) As Object



    Private m_vRiskDetails(,) As Object
    'As per VB Code
    Private m_vRiskTypeDetails(,) As Object
    Private m_vDataDictionary As Object
    Private m_bSubScreen As Boolean
    Private m_sMyOIKey As String = ""
    Private m_sParentOIKey As String = ""
    Private m_sChildOIKey As String = ""
    Private m_sMyObjectName As String = ""
    Private m_sParentObjectName As String = ""
    Private m_sChildObjectName As String = ""
    Private m_sGISObjectName As String = ""
    Private m_vScreenValues As Object
    Private m_sReferReasons As String = ""
    Private m_sDeclineReasons As String = ""
    Private m_sMessages As String = ""
    Private m_sQuoteType As String = ""
    ' Stores the return value for the a function call.
    Private m_lReturn As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_bIsUnderwriting As Boolean
    Private m_sUseRiskType As String = ""
    Private m_bChildAddStatus As Boolean 'signal that a child object has been added
    Private m_bRiskAdd As Boolean 'signal that a risk has been created
    Private m_bRiskCopied As Boolean

    Private m_vXMLDataSet As Object
    Private m_bSwiftIntegration As Boolean 'indicates running in Swift Itegration mode

    Private m_oPMUExtras As bGISPMUExtras.Business 'used in Swift Integration mode
    Private m_bCopyRisk As Boolean
    Private m_vKeyArray(,) As Object 'PN24176
    Private m_dtPolicyStartDate As Date
    Private m_dtPolicyEndDate As Date
    Private m_lAgentCnt As Integer
    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer
    Private m_lCountryId As Integer

    Public Property LossSchedule() As Boolean
        Get
            Return m_bLossSchedule
        End Get
        Set(ByVal Value As Boolean)
            m_bLossSchedule = Value
        End Set
    End Property

    Public Property LossScheduleTypeId() As Integer
        Get
            Return m_lLossScheduleTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lLossScheduleTypeId = Value
        End Set
    End Property

    Public Property PerilTypeId() As Integer
        Get
            Return m_lPerilTypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimPerilID() As Integer
        Set(ByVal Value As Integer)
            m_lClaimPerilID = Value
        End Set
    End Property
    Public WriteOnly Property PerilID() As Integer
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property

    Public WriteOnly Property ClaimTransactionType() As String
        Set(ByVal Value As String)
            m_sClaimTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property ClaimInsFileCnt() As Integer
        Set(ByVal Value As Integer)
            m_lClaimInsFileCnt = Value
        End Set
    End Property

    Public WriteOnly Property ClaimRiskId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimRiskId = Value
        End Set
    End Property

    Public WriteOnly Property ClaimId() As Integer
        Set(ByVal Value As Integer)
            m_lClaimId = Value
        End Set
    End Property

    Public WriteOnly Property CaseID() As Integer
        Set(ByVal Value As Integer)
            m_lCaseID = Value
        End Set
    End Property

    Public WriteOnly Property ChildIndex() As Integer
        Set(ByVal Value As Integer)
            m_lChildIndex = Value
        End Set
    End Property

    Public Property ObjectType() As Integer
        Get
            Return m_lObjectType
        End Get
        Set(ByVal Value As Integer)
            m_lObjectType = Value
        End Set
    End Property

    Public ReadOnly Property XMLDataSet() As String
        Get
            Return m_vXMLDataSet
        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    Public Property Screen() As String
        Get
            Return m_sScreen
        End Get
        Set(ByVal Value As String)
            m_sScreen = Value
        End Set
    End Property

    Public Property GISDataModelId() As Integer
        Get
            Return m_lGISDataModelId
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelId = Value
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

    Public Property SourceId() As Integer
        Get
            Return m_iSourceId
        End Get
        Set(ByVal Value As Integer)
            m_iSourceId = Value
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

    Public Property ShortName() As String
        Get
            Return m_sShortName
        End Get
        Set(ByVal Value As String)
            m_sShortName = Value
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

    Public Property FromEvent() As Boolean
        Get
            Return m_bEvent
        End Get
        Set(ByVal Value As Boolean)
            m_bEvent = Value
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

    Public Property ParentOIKey() As String
        Get
            Return m_sParentOIKey
        End Get
        Set(ByVal Value As String)
            m_sParentOIKey = Value
        End Set
    End Property

    Public Property ChildOIKey() As String
        Get
            Return m_sChildOIKey
        End Get
        Set(ByVal Value As String)
            m_sChildOIKey = Value
            m_sMyOIKey = Value ' RAW110804 added - is this variable really needed?
        End Set
    End Property

    Public Property ParentObjectName() As String
        Get
            Return m_sParentObjectName
        End Get
        Set(ByVal Value As String)
            m_sParentObjectName = Value
        End Set
    End Property

    Public Property ChildObjectName() As String
        Get
            Return m_sChildObjectName
        End Get
        Set(ByVal Value As String)
            m_sChildObjectName = Value
            m_sMyObjectName = Value ' RAW110804 added - is this variable really needed?
        End Set
    End Property

    Public Property GISObjectName() As String
        Get
            Return m_sGISObjectName
        End Get
        Set(ByVal Value As String)
            m_sGISObjectName = Value
        End Set
    End Property

    Public Property GIS() As Object
        Get
            Return m_oGIS
        End Get
        Set(ByVal Value As Object)
            m_oGIS = Value
        End Set
    End Property

    Public Property ScreenValues() As Object
        Get
            Return VB6.CopyArray(m_vScreenValues)
        End Get
        Set(ByVal Value As Object)
            m_vScreenValues = Value
        End Set
    End Property

    Public Property ReferReasons() As String
        Get
            Return m_sReferReasons
        End Get
        Set(ByVal Value As String)
            m_sReferReasons = Value
        End Set
    End Property

    Public Property DeclineReasons() As String
        Get
            Return m_sDeclineReasons
        End Get
        Set(ByVal Value As String)
            m_sDeclineReasons = Value
        End Set
    End Property

    Public Property Messages() As String
        Get
            Return m_sMessages
        End Get
        Set(ByVal Value As String)
            m_sMessages = Value
        End Set
    End Property

    Public Property QuoteType() As String
        Get
            Return m_sQuoteType
        End Get
        Set(ByVal Value As String)
            m_sQuoteType = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            ' Return the interface exit status.
            m_lStatus = Value
        End Set
    End Property
    Public ReadOnly Property ChildAddStatus() As Boolean
        Get
            ' Return the child add status, true = child was added.
            Return m_bChildAddStatus
        End Get
    End Property

    Public WriteOnly Property SwiftIntegration() As Boolean
        Set(ByVal Value As Boolean)

            m_bSwiftIntegration = Value

            If m_bSwiftIntegration Then
                'Get bPMLock
                Dim temp_m_oPMUExtras As Object
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUExtras, "bGISPMUExtras.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oPMUExtras = temp_m_oPMUExtras

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bGISPMUExtras", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Property
                End If
            End If
        End Set
    End Property

    Public WriteOnly Property CopyRisk() As Boolean
        Set(ByVal Value As Boolean)
            m_bCopyRisk = Value
        End Set
    End Property

    Public WriteOnly Property GISPolicyLinkID() As Integer
        Set(ByVal Value As Integer)
            m_lPolicyLinkId = Value
        End Set
    End Property

    '******************************************************************************
    ' Name:         Initialise (Standard Method)
    ' Description:  Entry point for any initialisation code for this object.
    '******************************************************************************
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Set the object manager to nothing.
                g_oObjectManager = Nothing
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise " & "the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMEdit
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            'Get bPMLock
            'Dim temp_m_oPMLock As Object
            'm_lReturn = g_oObjectManager.GetInstance(temp_m_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            'm_oPMLock = temp_m_oPMLock
            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    ' Failed to process the interface.
            '    result = gPMConstants.PMEReturnCode.PMFalse
            '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            '    Return result
            'End If

            m_lReturn = iGISSharedConstants.GetRegSettingFromDataBusModel(m_sGISDataModel, iGISSharedConstants.GISRegUseRiskTypeID, m_sUseRiskType)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And m_sUseRiskType = "" Then
                'check if it is set at GIS level  PN26792
                m_lReturn = iGISSharedConstants.GetRegSettingFromDataBusModel("", iGISSharedConstants.GISRegUseRiskTypeID, m_sUseRiskType)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise " & "the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         Terminate (Standard Method)
    ' Description:  Entry point for any termination code for this
    '               object.
    '******************************************************************************
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_bRiskAdd And m_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                    m_lReturn = GetBusinessObject()

                    m_oBusiness.DeleteRiskCancelledOnAdd()
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                m_oPMUExtras = Nothing
                m_lReturn = RemoveGISInterfaceObject()
                m_lReturn = RemoveBusinessObject()
                m_vScreenDetails = Nothing
                m_vChildScreenDetails = Nothing
                m_vRiskDetails = Nothing
                m_vRiskTypeDetails = Nothing
                m_vScreenValues = Nothing
                m_vDataDictionary = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    '******************************************************************************
    ' Name:         SetKeys (Standard Method)
    ' Description:  Stores all of the parameter members with the key array.
    '******************************************************************************
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the correct key array item.

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameRiskScreenID

                        m_lScreenId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNamePolicyStartDate

                        m_dtPolicyStartDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case "policy_end_date"

                        m_dtPolicyEndDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameAgentCnt

                        m_lAgentCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameRiskCodeID

                        m_lRiskCodeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameRiskGroupID

                        m_lRiskGroupId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameCountryId

                        m_lCountryId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select
            Next lRow


            ' Save the incoming keyarray for passing on to child screens  PN24176
            m_vKeyArray = vKeyArray

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetKeys (Standard Method)
    ' Description:  Stores all of the key array with the parameter members
    '******************************************************************************
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    '******************************************************************************
    ' Name:         GetSummary (Standard Method)
    ' Description:  Stores all of the summary array with the parameter members.
    '******************************************************************************
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialise the summary array with the number of items needed to be
            ' returned.  Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameNavigatorTitle1

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sNavigatorTitle

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         SetProcessModes (Standard Method)
    ' Description:  Set the optional process modes.
    '******************************************************************************
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.

            If Not Information.IsNothing(vTask) Then

                m_iTask = CType(CInt(vTask), gPMConstants.PMEComponentAction)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CType(CInt(vNavigate), gPMConstants.PMENavigateButtonStatus)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetScreenDetails
    ' Description:  Retrieves the screen details from the business object.
    '******************************************************************************
    Public Function GetScreenDetails(ByRef r_vDataDictionary As Object, _
                                     ByRef r_vScreenHeader(,) As Object, _
                                     ByRef r_vScreenDetails(,) As Object, _
                                     ByRef r_vChildScreenDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oBusiness.GetScreenDetails(r_vDataDictionary:=r_vDataDictionary, r_vScreenHeader:=r_vScreenHeader, r_vScreenDetails:=r_vScreenDetails, r_vChildScreenDetails:=r_vChildScreenDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get screen " & "details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScreenDetails")
            End If



            m_vScreenDetails = r_vScreenDetails
            m_vChildScreenDetails = r_vChildScreenDetails


            m_vDataDictionary = r_vDataDictionary

            m_lReturn = RemoveBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details " & "from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScreenDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetScreenValues
    ' Description:  Retrieves the values from the GIS and business object.
    '******************************************************************************

    Public Function GetScreenValues(ByRef r_vScreenValues As Object, _
                                    ByRef r_vRiskDetails(,) As Object, _
                                    ByRef r_vRiskTypeDetails(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vArray As Object
        Dim lTransactionType As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(m_vScreenDetails) Then

                r_vScreenValues = Nothing

                r_vRiskDetails = Nothing
                r_vRiskTypeDetails = Nothing
                Return result
            End If

            ' Get the details from the business object.

            'It goes something like this... Get everything possible from the GIS
            'Some items aren't GISable, and need to be got via the business object
            'These include lookups, party info, standard wordings and sums insured
            m_lReturn = GetGISInterfaceObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = GetBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RemoveGISInterfaceObject()
                Return result
            End If


            lTransactionType = m_oBusiness.TransactionType

            'Pick up data model type from business object
            If m_sGISDataModel = "" Then

                m_lReturn = m_oBusiness.GetGISDataModel(r_lGISDataModelId:=m_lGISDataModelId, r_sGISDataModel:=m_sGISDataModel, r_lGISDataModelType:=m_lGISDataModelType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RemoveGISInterfaceObject()
                    Return result
                End If
            End If

            'if launched from Swift Black Box or test harness and not a sub-screen
            Dim sOIKeys As Object
            If m_lInsuranceFolderCnt = -1 And Not m_bSubScreen Then
                m_sParentObjectName = m_sGISDataModel & "_policy_binder"
                m_oGIS.GetAllOIKey(m_sParentObjectName, sOIKeys)

                m_sParentOIKey = CStr(sOIKeys(0))
            End If

            'Ensure variables that were passed by reference are passed through to
            'resilience function byref, so we get the values back
            m_lReturn = SaveOnLoadRisk(v_iTask:=Task, r_vScreenValues:=r_vScreenValues, r_vRiskDetails:=r_vRiskDetails, r_vRiskTypeDetails:=r_vRiskTypeDetails, v_lTransactionType:=lTransactionType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' PW110804


            m_vRiskDetails = r_vRiskDetails
            m_vRiskTypeDetails = r_vRiskTypeDetails

            'Top level screen only processing
            If Not m_bSubScreen Then
                If m_lPolicyBinderId = 0 Then

                    m_lReturn = m_oBusiness.GetBinder(lPolicyLinkId:=m_lPolicyLinkId, sDataModel:=m_sGISDataModel, lPolicyBinderId:=m_lPolicyBinderId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If 'not sub screen or launched from swift

            'So now we loop around the screen details and find what we can find

            ReDim r_vScreenValues(m_vScreenDetails.GetUpperBound(1))

            'As example - top level motor screen,
            'parent is RSA_policy_binder
            'child is nothing
            'sub level driver screen,
            'parent is RSA_motor
            'child is RSA_driver
            'sub sub level driver screen,
            'parent is RSA_driver
            'child is RSA_convictions

            'Starts to coincide here
            ' Now lets load the screen values
            m_lReturn = LoadNewScreen(r_vScreenValues:=r_vScreenValues)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = RemoveBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            vArray = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details " & "from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScreenValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         IsScreenPostQuote
    ' Description:  Returns if a screen is a post quote screen for the given
    '               risk group
    '
    ' History:      10/04/2002 CTAF - Created.
    '               09/08/2002 CTAF - Merged in from CNIC
    '               26/02/2004 TR   - Changed so that local values for RiskType
    '               and ScreenID are always used (instead of passing them in everytime)
    '******************************************************************************
    Private Function IsScreenPostQuote(ByRef r_bIsPostQuote As Boolean) As Integer
        Dim result As Integer = 0
        Dim oRiskGroup As Object
        Dim vRiskGroups(,) As Object
        Dim vPQScreenID As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the risk group object
        Dim temp_oRiskGroup As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oRiskGroup, "bSIRRiskGroup.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oRiskGroup = temp_oRiskGroup
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRRiskGroup.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="IsScreenPostQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return result
        End If

        ' GetRiskGroup

        m_lReturn = oRiskGroup.GetRiskGroup(r_vDetailArray:=vRiskGroups)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bSIRRiskGroup." & "GetRiskGroup failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsScreenPostQuote")
            Return result
        End If

        ' Terminate and clear up

        oRiskGroup.Dispose()
        oRiskGroup = Nothing

        ' Find the risk group

        For lLoop1 As Integer = 0 To vRiskGroups.GetUpperBound(1)

            If CInt(vRiskGroups(ACArrayRiskGroupID, lLoop1)) = m_lRiskTypeId Then
                ' Get the post quote screen id

                vPQScreenID = CStr(vRiskGroups(ACArrayPQRiskScreenType, lLoop1))
                If vPQScreenID = "" Then
                    vPQScreenID = CStr(0)
                End If
                ' Exit the loop
                Exit For
            End If
        Next lLoop1

        ' Is it the post quote screen?
        'Developer Guide No. CInt() gives error when vPQScreenID = ""
        If (Integer.TryParse(vPQScreenID, vPQScreenID)) Then
            If (vPQScreenID = m_lScreenId) Then
                r_bIsPostQuote = True
            Else
                r_bIsPostQuote = False
            End If
        Else
            r_bIsPostQuote = False
        End If
        Return result

    End Function

    '******************************************************************************
    ' Name:         Update
    ' Description:
    ' History:
    '  14/08/2000 Tomo - Created.
    '  04/09/2002 RAM Code added to run the UAL NBQuote Method for Underwriting only.
    '                 Code added to run the Rating NBQuote Method for Broking System only
    '  21/02/2003 Kevin Renshaw Isssue 1650 donot run accumulations if id is 0
    '  08/09/2003 RAW CQ2377 detail moved into a separate private function DoUpdate
    '                 so that Risk status can be set if update fails for whatever reason
    '******************************************************************************
    Public Function Update(ByRef r_vScreenValues() As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = DoUpdate(r_vScreenValues:=r_vScreenValues)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_bSubScreen Then
                ' Refresh my screen values in case they have been changed by scripts.
                m_lReturn = RefreshScreenValuesFromGIS(r_vScreenValues:=r_vScreenValues)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to refresh " & "screen data after update", vApp:=ACApp, vClass:=ACClass, vMethod:="Update")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Note - my screen values may also be picked up by a parent screen
                'through the ScreenValues property
                m_vScreenValues = r_vScreenValues
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result



            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name: DisplaySubScreen
    ' Description:
    ' History:
    '   09/09/2000 Tomo - Created.
    '   22/07/2003 RAW CQ1783 : copied code from 1.8.6
    '   30/05/2003 DP  added optional vData param for passing data from parent to child
    '   14/10/2003 RAW CQ2754 : added r_vMyScreenValues param and renamed vArray
    '                           param as r_vSubScreenValues for clarity
    '   15/06/2004 PW  CQ3821 : pass sub screen status back to calling program
    '******************************************************************************
    Public Function DisplaySubScreen(ByRef lScreenId As Integer, ByRef sParentOIKey As String, ByRef sChildOIKey As String, ByRef sParentObjectName As String, ByRef sChildObjectName As String, ByRef r_vMyScreenValues() As Object, ByRef r_vSubScreenValues As Object, ByRef vRiskTypeDetails As Object, Optional ByRef vData(,) As Object = Nothing, Optional ByRef r_lStatus As Integer = 0, Optional ByRef r_bReserveLimitExceeded As Boolean = False, Optional ByRef r_dExceededReserve As Decimal = 0) As Integer

        Dim result As Integer = 0

        Dim oObject, aChildData(,) As Object
        Dim iVal As Integer
        Dim vGISXMLDataset As Object
        Dim sInterfaceName As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Claimsbuilder changes - bring 1.8.6 into 1.9 code
            If m_bSwiftIntegration Then
                'Developer Guide No.108
                sInterfaceName = "iPMURiskSwift.Interface_Renamed"
            Else
                'Developer Guide No.108
                sInterfaceName = "iPMURisk.Interface_Renamed"
            End If

            'Create the appropriate object

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oObject, sClassName:=sInterfaceName, vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'PN12473 The transaction type is not being pulled through to the parameter
            'array in child objects

            oObject.SetProcessModes(vTask:=m_iTask, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode)


            m_lReturn = oObject.SetKeys(m_vKeyArray) 'PN24176


            oObject.SubScreen = True

            oObject.ScreenId = lScreenId


            oObject.GIS = m_oGIS

            oObject.ParentOIKey = sParentOIKey

            oObject.ChildOIKey = sChildOIKey

            oObject.ParentObjectName = sParentObjectName

            oObject.ChildObjectName = sChildObjectName

            'DP 30/05/2003 - added for Stargate to enable data to passed from the
            'parent to the child
            If Information.IsArray(vData) Then
                'vData:- column 0 = target childscreen name; column 1 = value being
                'passed loop through the array and only pick out the values
                'relevant to this child screen
                For iCount As Integer = 0 To vData.GetUpperBound(1)

                    If Convert.ToString(vData(0, iCount)).ToUpper() = sChildObjectName.ToUpper() Then
                        'we have a match, so
                        If Not Information.IsArray(aChildData) Then
                            ReDim aChildData(1, 0)
                        End If

                        iVal = aChildData.GetUpperBound(1)
                        'save the data in the new array
                        'first - the value name (basically acts as an identifier for
                        'whoever writes the dyn logic scripts)


                        aChildData(0, iVal) = vData(1, iCount)
                        'now - the datavalue itself


                        aChildData(1, iVal) = vData(2, iCount)
                        ReDim Preserve aChildData(1, (iVal + 1))
                    End If
                Next iCount
                'now pass this array into the object


                oObject.ChildDataFromParent = aChildData
            End If


            oObject.ObjectType = m_lObjectType

            oObject.InsuranceFolderCnt = m_lInsuranceFolderCnt

            oObject.InsuranceFileCnt = m_lInsuranceFileCnt

            oObject.RiskId = m_lRiskId

            oObject.RiskTypeId = m_lRiskTypeId ' RAW 26/06/2003 : CQ1507 : added


            oObject.RiskTypeDetails = vRiskTypeDetails

            oObject.ChildIndex = m_lChildIndex

            oObject.ClaimPerilID = m_lClaimPerilID

            oObject.PerilID = m_lPerilID

            oObject.ClaimTransactionType = m_sClaimTransactionType

            oObject.ClaimInsFileCnt = m_lClaimInsFileCnt

            oObject.ClaimRiskId = m_lClaimRiskId

            oObject.ClaimId = m_lClaimId

            oObject.LossSchedule = m_bLossSchedule

            oObject.LossScheduleTypeId = m_lLossScheduleTypeId

            oObject.PerilTypeId = m_lPerilTypeId

            oObject.ProductId = m_lProductId

            oObject.GISPolicyLinkID = m_lPolicyLinkId

            oObject.CaseID = m_lCaseID

            m_lReturn = LoadGisFromScreenValues(r_vScreenValues:=r_vMyScreenValues)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load screen " & "values into GIS", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplaySubScreen")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' store current contents of GIS in case the next screen is cancelled and we have to restore it

            m_lReturn = m_oGIS.ReturnAsXML(r_vXMLDataSet:=vGISXMLDataset)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to store GIS data", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplaySubScreen")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oObject.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_lStatus = oObject.Status
            r_bReserveLimitExceeded = oObject.ReserveLimitExceeded
            r_dExceededReserve = oObject.ExceededReserve
            If r_lStatus = gPMConstants.PMEReturnCode.PMCancel Then
                ' restore the contents of GIS to it's state as it was just before
                'the screen was displayed

                m_lReturn = m_oGIS.LoadFromXML(v_sGisDataModelCode:=m_sGISDataModel, v_sXMLDataSet:=vGISXMLDataset)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to restore GIS " & "data", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplaySubScreen")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                Return result
            End If


            sChildOIKey = oObject.ChildOIKey

            ' These are the values from the child screen
            ' Note - This includes ALL sub screen values (including list view details
            ' from that screen) whereas the child screen parts of MyScreenValues only
            ' includes those columns included in my listviews


            r_vSubScreenValues = oObject.ScreenValues

            ' Refresh my screen values with details that may have been changed by
            'the child screen
            m_lReturn = RefreshScreenValuesFromGIS(r_vScreenValues:=r_vMyScreenValues)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to refresh parent " & "screen from child screen", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplaySubScreen")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_vScreenValues = r_vMyScreenValues ' RAW 03/11/2003 : CQ2754/2862 : added


            oObject.Dispose()
            oObject = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplaySubScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplaySubScreen", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result



            Return result
        End Try
    End Function

    '******************************************************************************
    ' Name:         DelObjectInstance
    ' Description:
    ' History:      09/09/2000 Tomo - Created.
    '******************************************************************************
    Public Function DelObjectInstance(ByRef sObjectName As String, ByRef sOIKey As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oGIS.DelObjectInstance(v_sObjectName:=sObjectName, v_sOIKey:=sOIKey)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DelObjectInstance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DelObjectInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         GetBusinessObject
    ' Description:
    ' History:      01/08/2000 Tomo - Created.
    '******************************************************************************
    Private Function GetBusinessObject() As Integer

        Dim result As Integer = 0
        Dim sTitle, sMessage As String



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oBusiness Is Nothing) Then
            Return result
        End If

        ' Get an instance of the business object via the public object manager.
        Dim temp_m_oBusiness As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRRiskScreen.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Display error stating the problem. Get string from the resource file.

            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))

            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString))
            ' Display message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result
        End If

        ' Set the process modes for the busines object.

        m_lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to process the interface.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process" & " modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadControl")
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Set the business keys.

        m_oBusiness.PartyCnt = m_lPartyCnt

        m_oBusiness.InsuranceFolderCnt = m_lInsuranceFolderCnt

        m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

        m_oBusiness.RiskId = m_lRiskId

        m_oBusiness.RiskTypeId = m_lRiskTypeId

        m_oBusiness.ProductId = m_lProductId

        m_oBusiness.ScreenId = m_lScreenId

        m_oBusiness.SubScreen = m_bSubScreen


        m_bIsUnderwriting = (m_oBusiness.UnderwritingOrAgency = "U")

        Return result

    End Function

    '******************************************************************************
    ' Name:         RemoveBusinessObject
    ' Description:
    ' History:      01/08/2000 Tomo - Created.
    '******************************************************************************
    Private Function RemoveBusinessObject() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oBusiness Is Nothing) Then
            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing
        End If
        Return result

    End Function

    '******************************************************************************
    ' Name:         GetGISInterfaceObject
    ' Description:
    ' History:      01/08/2000 Tomo - Created.
    '******************************************************************************
    Private Function GetGISInterfaceObject() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (m_oGIS Is Nothing) Then
            Return result
        End If

        m_oGIS = CreateLateBoundObject("iGIS.Application")
        'Developer Guide No 9. 
        m_lReturn = m_oGIS.Initialise()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the" & " GIS interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISInterfaceObject")
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RAW 22/07/2003 : CQ1672 : added - Set the process modes for the object.
        m_lReturn = m_oGIS.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the " & "process modes for the GIS interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISInterfaceObject")
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:         RemoveGISInterfaceObject
    ' Description:
    ' History:      01/08/2000 Tomo - Created.
    '******************************************************************************
    Private Function RemoveGISInterfaceObject() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Do not remove the object if it's passed in - if it's a sub screen
        If SubScreen Then
            Return result
        End If

        If Not (m_oGIS Is Nothing) Then
            ' Terminate the GIS object
            m_oGIS.Dispose()
            ' Destroy the instance of the business object from memory.
            m_oGIS = Nothing
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:         GetDescription
    ' Description:
    ' History:      30/08/2000 Tomo - Created.
    '               CLG 11/11/2003 : CQ2380 : Party Field Displayed as ID
    '               CLG 18/11/2003 : CD3303 : Combo Id Shown in Child Grid
    '******************************************************************************
    Private Function GetDescription(ByVal v_vSpecialsType As Integer, ByVal v_vSpecialsTypeReference As String, ByVal vValue As String, ByRef vDescription As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Default the description to the value
        vDescription = vValue


        If Convert.IsDBNull(vValue) Or IsNothing(vValue) Then
            Return result
        End If

        If vValue = "" Then
            Return result
        End If

        If vValue = "0" Then
            vDescription = ""
            Return result
        End If

        Dim vArray(,) As Object
        Select Case v_vSpecialsType
            Case GISSharedPropertyConstants.ACOGISUserDefHeaderID
                Dim dbNumericTemp As Double
                If Double.TryParse(v_vSpecialsTypeReference, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    If m_bSwiftIntegration Then
                        'value is stored as code, must get the ID
                        vDescription = vValue

                        m_oPMUExtras.GetIdFromCode("0" & v_vSpecialsTypeReference, vDescription, vValue)
                    End If

                    m_lReturn = m_oBusiness.GetGISUserDefHeaderDetail(v_lGISHeaderId:=CInt(v_vSpecialsTypeReference), v_vValue:=vValue, v_vDescription:=vDescription)
                End If

            Case GISSharedPropertyConstants.ACOPMLookupTableName
                If v_vSpecialsTypeReference <> "" Then

                    m_lReturn = m_oBusiness.GetLookup(sPMLookup:=v_vSpecialsTypeReference, vValue:=vValue, vDescription:=vDescription, vMode:=0)
                End If

            Case ACPSpecialsTypeReference
                If ToSafeDouble(v_vSpecialsTypeReference) <> 0 Then

                    m_lReturn = m_oBusiness.GetUserDef(iUserDef:=CInt(v_vSpecialsTypeReference), vValue:=vValue, vDescription:=vDescription)
                End If

            Case GISSharedPropertyConstants.ACOPartyTypeID
                Dim dbNumericTemp3 As Double
                Dim dbNumericTemp2 As Double
                If Double.TryParse(vValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) And Double.TryParse(v_vSpecialsTypeReference, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then

                    m_lReturn = m_oBusiness.GetParty(lPartyCnt:=CInt(vValue), lPartyTypeId:=CInt(v_vSpecialsTypeReference), vPartyArray:=vArray)
                End If
                If Information.IsArray(vArray) Then

                    vValue = CStr(vArray(0, 0))

                    vDescription = CStr(vArray(1, 0))
                End If
        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:         LockRisk
    ' Description:
    '******************************************************************************

    'Private Function LockRisk() As Integer
    '
    'Dim result As Integer = 0
    'Dim sLockedBy As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.LockKey(sKeyName:="risk_cnt", vKeyValue:=m_lRiskId, iUserID:=g_oObjectManager.UserID, sCurrentlyLockedBy:=sLockedBy)
    '
    'Select Case m_lReturn
    'Case gPMConstants.PMEReturnCode.PMTrue 'OK
    '
    'Case gPMConstants.PMEReturnCode.PMFalse 'Locked or error
    'If sLockedBy = "ERROR" Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to lock " & "record", vApp:=ACApp, vClass:=ACClass, vMethod:="LockRisk")
    'Return result
    'Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'MessageBox.Show("Risk currently locked by " & sLockedBy & Strings.Chr(13) & Strings.Chr(10) & "Please try later", "Risk Lock")
    'Return result
    'End If
    '
    'Case Else
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock the risk", vApp:=ACApp, vClass:=ACClass, vMethod:="LockRisk")
    'Return result
    '
    'End Select
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    'Return result
    '
    'End Try
    'End Function

    '******************************************************************************
    ' Name:         UnlockRisk
    ' Description:
    '******************************************************************************

    'Private Function UnlockRisk() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oPMLock.UnLockKey(sKeyName:="risk_cnt", vKeyValue:=m_lRiskId, iUserID:=g_iUserID)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error trying to unlock " & "the risk", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockRisk")
    'Return result
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockRisk", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    'Return result
    '
    'End Try
    'End Function

    '******************************************************************************
    ' Name:         GetParentForChild
    ' Description:
    ' History:      16/09/2000 Tomo - Created.
    '******************************************************************************
    Private Function GetParentForChild(ByRef sChildObjectName As String, ByRef sParentObjectName As String, ByRef sParentOIKey As String, ByVal lObjectType As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        If sChildObjectName = "" Then
            Return result
        End If

        'How do we get the parent object name?
        Select Case lObjectType
            Case GISDataModelType.GISOTPeril
                'Left from 1.9 - hard code this value for GISOTPerils
                sParentObjectName = "work_claim"

            Case Else
                'Use the proper method on the Gis to get the parent
                m_lReturn = m_oGIS.GetObjectDefDetails(v_sObjectName:=sChildObjectName, r_sParentObjectName:=sParentObjectName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get parent " & "name for " & sChildObjectName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetParentForChild")
                    Return result
                End If
        End Select

        'Everything should already be set up
        If m_sParentObjectName.Trim().ToUpper() = sParentObjectName.Trim().ToUpper() Then
            sParentOIKey = m_sParentOIKey
        Else
            m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=m_sParentObjectName, v_sParentOIKey:=m_sParentOIKey, v_sChildObjectName:=sParentObjectName, r_vChildOIKeyArray:=vArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there aren't any
            If Not Information.IsArray(vArray) Then
                ' Create the new OI of whatever...
                m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=sParentObjectName, r_sOIKey:=sParentOIKey, v_sParentOIKey:=m_sParentOIKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else


                sParentOIKey = CStr(vArray(vArray.GetLowerBound(0)))
            End If
        End If

        vArray = Nothing

        Return result

    End Function
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '******************************************************************************
    ' Name:         UpdateListRiskValue
    ' Description:  loop through top level objects and update magic fields which
    '               get transfer to ListRisk screen
    ' History:      13/10/2004 Thinh Nguyen - Created
    ' Note :        (Trust me I am not mad.) Looping throught child intances and
    '               overwrite the the r_vRiskScreenDetail(xxx,0) is the way our
    '               system currently works and I am not allow to change it so there.....
    '******************************************************************************

    'Private Function UpdateListRiskValue(ByVal v_sTopLevelObjectName As String, ByVal v_sOIKey As String, ByRef r_vRiskScreenDetail( ,  ) As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim vChildObjectArray, vChildOIKeyArray As Object
    'Dim lChildMax, lChildInstanceMax As Integer
    'Dim vPropertyValue, sMessage As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'do we have a valid risk screen
    'If Not Information.IsArray(r_vRiskScreenDetail) Then
    'Return result
    'End If
    '
    'get all child objects for this top level object
    'm_lReturn = m_oGIS.GetObjectDefDetails(v_sObjectName:=v_sTopLevelObjectName, r_vChildObjectArray:=vChildObjectArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'sMessage = "Failed to get child objects for " & v_sTopLevelObjectName
    'Throw New Exception()
    'End If
    '
    'Nothign further to do if there are no child objects
    'If Not Information.IsArray(vChildObjectArray) Then
    'Return result
    'End If
    '

    'lChildMax = vChildObjectArray.GetUpperBound(0)
    '
    'loop through all object and update magic fields with value from dataset
    'For 'lChildCount As Integer = 0 To lChildMax
    '

    'm_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=v_sTopLevelObjectName, v_sParentOIKey:=v_sOIKey, v_sChildObjectName:=CStr(vChildObjectArray(lChildCount)), r_vChildOIKeyArray:=vChildOIKeyArray)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get Child OI Key for " & CStr(vChildObjectArray(lChildCount))
    'Throw New Exception()
    'End If
    '
    'do we have any records for this object
    'If Information.IsArray(vChildOIKeyArray) Then

    'lChildInstanceMax = vChildOIKeyArray.GetUpperBound(0)
    '
    'loop through child instances and update riskdetails
    'For 'lChildInstance As Integer = 0 To lChildInstanceMax
    '
    'DESCRIPTION
    'vPropertyValue = ""


    'm_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjectArray(lChildCount)), v_sPropertyName:="DESCRIPTION", v_sOIKey:=CStr(vChildOIKeyArray(lChildInstance)), r_vPropertyValue:=vPropertyValue)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get property value for " & CStr(vChildObjectArray(lChildCount)) & ".DESCRIPTION"
    'Throw New Exception()
    'End If
    'If vPropertyValue <> "" Then

    'r_vRiskScreenDetail(ACRDescription, 0) = vPropertyValue
    'End If
    '
    'ACCUMULATION
    'vPropertyValue = ""


    'm_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjectArray(lChildCount)), v_sPropertyName:="ACCUMULATION", v_sOIKey:=CStr(vChildOIKeyArray(lChildInstance)), r_vPropertyValue:=vPropertyValue)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get property value for " & CStr(vChildObjectArray(lChildCount)) & ".ACCUMULATION"
    'Throw New Exception()
    'End If
    'If vPropertyValue <> "" Then

    'r_vRiskScreenDetail(ACRAccumulationId, 0) = vPropertyValue
    'End If
    '
    'EML_PERCENTAGE
    'vPropertyValue = ""


    'm_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjectArray(lChildCount)), v_sPropertyName:="EML_PERCENTAGE", v_sOIKey:=CStr(vChildOIKeyArray(lChildInstance)), r_vPropertyValue:=vPropertyValue)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get property value for " & CStr(vChildObjectArray(lChildCount)) & ".EML_PERCENTAGE"
    'Throw New Exception()
    'End If
    'If vPropertyValue <> "" Then

    'r_vRiskScreenDetail(ACREMLPercentage, 0) = vPropertyValue
    'End If
    '
    'COVERAGE
    'vPropertyValue = ""


    'm_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjectArray(lChildCount)), v_sPropertyName:="COVERAGE", v_sOIKey:=CStr(vChildOIKeyArray(lChildInstance)), r_vPropertyValue:=vPropertyValue)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get property value for " & CStr(vChildObjectArray(lChildCount)) & ".COVERAGE"
    'Throw New Exception()
    'End If
    'If vPropertyValue <> "" Then

    'r_vRiskScreenDetail(ACRCoverage, 0) = vPropertyValue
    'End If
    '
    'INSURED_ITEM
    'vPropertyValue = ""


    'm_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjectArray(lChildCount)), v_sPropertyName:="INSURED_ITEM", v_sOIKey:=CStr(vChildOIKeyArray(lChildInstance)), r_vPropertyValue:=vPropertyValue)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get property value for " & CStr(vChildObjectArray(lChildCount)) & ".INSURED_ITEM"
    'Throw New Exception()
    'End If
    'If vPropertyValue <> "" Then

    'r_vRiskScreenDetail(ACRInsuredItem, 0) = vPropertyValue
    'End If
    '
    'EXTENSIONS
    'vPropertyValue = ""


    'm_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(vChildObjectArray(lChildCount)), v_sPropertyName:="EXTENSIONS", v_sOIKey:=CStr(vChildOIKeyArray(lChildInstance)), r_vPropertyValue:=vPropertyValue)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

    'sMessage = "Failed to get property value for " & CStr(vChildObjectArray(lChildCount)) & ".EXTENSIONS"
    'Throw New Exception()
    'End If
    'If vPropertyValue <> "" Then

    'r_vRiskScreenDetail(ACRExtensions, 0) = vPropertyValue
    'End If
    'Next  'child instance
    'End If
    'Next  'top level objects
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    'If sMessage = "" Then
    'sMessage = "Failed to update values which appear on ListRisk Screen " & "from dataset"
    'End If
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateListRiskValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    'End Try
    'End Function

    '******************************************************************************
    ' Name:         RunScreenRule
    ' Description:
    ' History:      05/08/2003 RVH - Created - need mechanism to call a particular
    '               script for a given screen - specifically for use by claim risk
    '               -> peril interaction
    '******************************************************************************
    Public Function RunScreenRule(ByVal iPBCQemQuoteType As Integer, ByVal lScreenId As Integer, ByVal sChildOIKey As String, Optional ByVal v_dtCoverStartDate As Date = #1/1/1900#, Optional ByVal lTransactionType As Integer = -1) As Integer

        ' AMB 10/07/2003: v_dtCoverStartDate is only passed in when transaction
        'is MTA and we're running the UAL script - see Update function

        Dim result As Integer = 0
        Const KeyChildOIKey As String = "CHILD_OIKEY"

        Dim lQuoteType As Integer 'encoded quote type value
        Dim lRiskTypeId As Integer ' SFB=RiskTypeId SFU=-1
        Dim vArray(1, 0) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Check if transaction type was specified, if not use the value from the
            'business object
            m_lReturn = GetBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lTransactionType = -1 Then

                lTransactionType = m_oBusiness.TransactionType
            End If

            If iPBCQemQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypePreScreen And m_bCopyRisk Then
                iPBCQemQuoteType = PBQuoteTypeEncode.PBCQemQuoteTypeCopyRisk
            End If

            PBQuoteTypeEncode.EncodeTransactionScreenAndType(lQuoteType, lTransactionType, lScreenId, iPBCQemQuoteType)

            If m_sUseRiskType = "Y" Then
                lRiskTypeId = m_lRiskTypeId
            Else
                lRiskTypeId = -1
            End If

            ' RVH 7/7/2003 : Create additional data array to be passed down

            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = KeyChildOIKey

            vArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sChildOIKey

            ' AMB 10/07/2003 Date Effective Rating - use the appropriate cover start date
            If v_dtCoverStartDate = #1/1/1900# Then
                ' RVH 7/7/2003 : IAG : Pass additional data array
                m_lReturn = m_oGIS.NBQuote(v_lQuoteType:=lQuoteType, v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=DateTime.Today, v_lRiskGroupID:=lRiskTypeId, r_vAdditionalDataArray:=vArray)
            Else
                ' RVH 7/7/2003 : IAG : Pass additional data array
                m_lReturn = m_oGIS.NBQuote(v_lQuoteType:=lQuoteType, v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=v_dtCoverStartDate, v_lRiskGroupID:=lRiskTypeId, r_vAdditionalDataArray:=vArray)
            End If

            m_lReturn = RemoveBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RunScreenRule Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RunScreenRule", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         LoadGisFromScreenValues
    ' Description:  Code ripped from procedure "DoUpdate"
    '               The gis object needs to be loaded from the gis screen values
    '               and passed down to its sub screens without updating the database
    ' History:      Created : MEvans : 23-09-2003 : CQ2089 \ CQ1943
    '******************************************************************************
    Public Function LoadGisFromScreenValues(ByRef r_vScreenValues() As Object) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "LoadGisFromScreenValues"

        Dim lTransactionType As Integer
        Dim vOIKeyArray As Object
        Dim sOIKey As String = ""
        Dim vChildOIKeyArray As Object
        Dim sChildOIKey As String = ""
        Dim vValue As Object
        Dim vArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Code ripped from procedure "DoUpdate"
            ' The gis object needs to be loaded from the gis screen values
            ' and passed down to its sub screens without updating the database

            'This was stolen from GetBusiness, and a lot of superfluous stuff is retained.
            'It _shouldn't_ be necessary to retest and recreate the instances, but just in case...
            'OK, if we're in a sub screen we _cannot_ have nasty stuff like sums insured and
            'standard wordings.  We _only_ have stuff that can be stored in the GIS

            m_lReturn = GetBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RemoveGISInterfaceObject() 'Tidy up
                Return result
            End If


            lTransactionType = m_oBusiness.TransactionType

            If Not m_bSubScreen Then
                ' Get the Top Level OI Key
                m_lReturn = m_oGIS.GetAllOIKey(v_sObjectName:=m_sGISDataModel & "_policy_binder", r_vOIKeyArray:=vOIKeyArray)
                If Information.IsArray(vOIKeyArray) Then


                    sOIKey = CStr(vOIKeyArray(vOIKeyArray.GetUpperBound(0)))
                End If

                vOIKeyArray = Nothing

                If sOIKey.Length = 0 Then
                    m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=m_sGISDataModel & "_policy_binder", r_sOIKey:=sOIKey)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            'So now we loop around the screen details and update what we can update

            For lTemp As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)
                Dim auxVar_2 As Object = m_vScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)


                If Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Then
                    If m_sChildObjectName = "" Then

                        m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=m_sGISDataModel & "_policy_binder", v_sParentOIKey:=sOIKey, v_sChildObjectName:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)), r_vChildOIKeyArray:=vChildOIKeyArray)

                        ' If there aren't any
                        If Not Information.IsArray(vChildOIKeyArray) Then
                            ' Create the new OI of whatever...

                            m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)), r_sOIKey:=sChildOIKey, v_sParentOIKey:=sOIKey)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                        Else
                            ' use the first one


                            sChildOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))
                        End If
                    Else
                        If m_sChildOIKey.Length = 0 Then
                            ' Create the new OI of whatever...

                            m_lReturn = m_oGIS.NewObjectInstance(v_sObjectName:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)), r_sOIKey:=sChildOIKey, v_sParentOIKey:=m_sParentOIKey)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            m_sChildOIKey = sChildOIKey
                        Else
                            sChildOIKey = m_sChildOIKey
                        End If
                    End If

                    vValue = "Who would set it to this?"

                    If CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISColumnName, lTemp)).ToLower.Contains("address_cnt") Then
                        vArray = r_vScreenValues(lTemp)
                        If Information.IsArray(vArray) Then
                            vValue = vArray(0, 0)
                        End If
                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOPartyTypeID Then  'It's a party

                        vArray = r_vScreenValues(lTemp)
                        If Information.IsArray(vArray) Then
                            vValue = vArray(0, 0)
                        End If

                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOProductID Then  'It's a policy

                        vArray = r_vScreenValues(lTemp)
                        If Information.IsArray(vArray) Then
                            vValue = vArray(0, 0)
                        End If

                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOSumInsuredTypeID Then
                        'It's a sum insured
                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOStdWordingType Then
                        'It's a standard Wording
                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOReserveID Then
                        'It's a Reserve
                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOPaymentID Then
                        'It's a Payment

                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOGISUserDefHeaderID And m_bSwiftIntegration Then
                        'need to convert id to code is in Swift mode


                        m_oPMUExtras.GetCodeAndIndicator(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp), r_vScreenValues(lTemp)(0), vValue, Convert.ToString(DBNull.Value))

                    ElseIf CDbl(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)) = GISSharedPropertyConstants.ACOPMLookupTableName And m_bSwiftIntegration Then
                        'need to convert id to code is in Swift mode

                        vValue = r_vScreenValues(lTemp)(0)


                        m_lReturn = m_oBusiness.GetLookup(CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp)), vValue:=r_vScreenValues(lTemp)(0), vDescription:="")

                        vValue = r_vScreenValues(lTemp)(0)
                    Else
                        'It's 'just' a value
                        Dim auxVar As Object = m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp)


                        If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
                            'This _could_ be a sub screen...
                        Else

                            vArray = r_vScreenValues(lTemp)
                            'NIIT Comments 
                            If vArray Is Nothing Then
                                vValue = Nothing
                            Else
                                vValue = vArray(0)
                            End If

                        End If
                    End If
                    ' Set the property - if we have to
                    ''If Not vValue Is Nothing Then

                    If Convert.ToString(vValue) <> "Who would set it to this?" Then


                        m_lReturn = m_oGIS.SetPropertyValue(v_sObjectName:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)), v_sPropertyName:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)), v_sOIKey:=sChildOIKey, v_vPropertyValue:=vValue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse


                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to SetPropertyValue for " & CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)) & "." & CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)), vApp:=ACApp, vClass:=ACClass, vMethod:="LoadGISFromScreenValues")
                            Return result
                        End If

                        'Could be empty - shouldn't be but...

                        If String.IsNullOrEmpty(vValue) Then

                            vValue = Nothing
                        End If
                    End If
                    ''End If
                End If
            Next lTemp

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            Return result

        End Try
    End Function

    '******************************************************************************
    ' Name:         LoadNewScreen
    ' Description:  Code ripped from procedure "GetScreenValues"
    ' History:      RAW 14/10/2003 : CQ2754 : created
    '               RAW 23/10/2003 : CQ2754/2950 : renamed from LoadFromDB since
    '                                not loaded from DB if loading a child
    '******************************************************************************
    Private Function LoadNewScreen(ByRef r_vScreenValues As Object, Optional ByVal v_bUseOriginal As Boolean = False) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "LoadNewScreen"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load values array for those normal controls that are loaded from GIS

        m_lReturn = RefreshScreenValuesFromGIS(r_vScreenValues:=r_vScreenValues)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to load screen data" & "from GIS", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadNewScreen")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_bSubScreen Then
            'here m_sChildOIKey represents the main object that this screen is handling
            m_sMyOIKey = m_sChildOIKey
            m_sMyObjectName = m_sChildObjectName
        Else
            'here m_sChildOIKey represents the last child object that has been loaded
            m_sMyOIKey = m_sParentOIKey
            m_sMyObjectName = m_sParentObjectName
            m_sChildOIKey = ""
            m_sChildObjectName = ""
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:         RefreshScreenValuesFromGIS
    ' Description:  Based on LoadNewScreen but with DB reads, NBQuote etc removed
    '               and m_oGIS.NewObjectInstance replaced with Get
    ' History:      RAW 14/10/2003 : CQ2754 : created
    '******************************************************************************
    Public Function RefreshScreenValuesFromGIS(ByRef r_vScreenValues() As Object) As Integer

        Dim result As Integer = 0.0
        Const sFunctionName As String = "RefreshScreenValuesFromGIS"

        Dim lCount As Integer
        Dim vChildOIKeyArray, vArray(,) As Object
        Dim vValue As String = ""
        Dim lValue As Integer
        Dim vDescription, sObjectName, sOIKey, sChildOIKey, sStoreParentObjectName, sStoreChildObjectName, sStoreParentOIKey, sStoreChildOIKey As String
        Dim bBusinessObjectCreatedHere As Boolean
        Dim sThisChildOIKey, sThisChildObjectName As String
        Dim lGISPropertyID, lGISObjectID, lSpecialTypeRef As Integer
        Dim sKeyName, sKeyValue As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oBusiness Is Nothing Then
                m_lReturn = GetBusinessObject()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RemoveBusinessObject()
                    Return result
                End If
                bBusinessObjectCreatedHere = True
            End If


            For lTemp As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)
                'The object will not be null when it's a listview, but the property will be
                Dim auxVar_3 As Object = m_vScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp)


                If Not (Convert.IsDBNull(auxVar_3) Or IsNothing(auxVar_3)) Then

                    'This bit is for a 'normal' control

                    'In the parent we've either got RSA_policy_binder or RSA_motor
                    'If this is the top screen, we've no child and the array object
                    'name could be anything. If this is a sub screen, we've a child
                    '(but maybe no key) and the array object name MUST be the same as
                    'the child

                    If Not m_bSubScreen Then
                        ' we are displaying a parent/ top-level screen
                        ' Get the OI key of the object that this property exists on
                        m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=m_sParentObjectName, v_sParentOIKey:=m_sParentOIKey, v_sChildObjectName:=Convert.ToString(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)), r_vChildOIKeyArray:=vChildOIKeyArray)
                        If Information.IsArray(vChildOIKeyArray) Then
                            ' use the first one


                            sThisChildOIKey = CStr(vChildOIKeyArray(vChildOIKeyArray.GetLowerBound(0)))
                        End If
                    Else
                        ' we are displaying a subscreen so all normal controls displayed here are properties or my own object
                        sThisChildOIKey = m_sMyOIKey
                    End If

                    ' RAW 23/10/2003 : CQ2754/2950 : changed to use local variable instead of m_sChildOIKey
                    If sThisChildOIKey <> "" Then


                        'Developer Guide No.101
                        m_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp), _
                                                            v_sPropertyName:=m_vScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp), _
                                                            v_sOIKey:=sThisChildOIKey, r_vPropertyValue:=vValue)

                        Select Case m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp)
                            Case GISSharedPropertyConstants.ACOPartyTypeID ' it's a party
                                If (vValue <> "") And (vValue <> "0") Then
                                    lValue = CInt(vValue)

                                    lSpecialTypeRef = CInt(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                                    m_lReturn = m_oBusiness.GetParty(lPartyCnt:=lValue, lPartyTypeId:=lSpecialTypeRef, vPartyArray:=vArray)
                                Else

                                    lValue = CInt(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                                    m_lReturn = m_oBusiness.GetParty(lPartyCnt:=0, lPartyTypeId:=lValue, vPartyArray:=vArray)
                                End If


                                r_vScreenValues(lTemp) = vArray

                            Case GISSharedPropertyConstants.ACOProductID ' it's a policy
                                If (vValue <> "") And (vValue <> "0") Then
                                    lValue = CInt(vValue)

                                    m_lReturn = m_oBusiness.GetPolicy(lInsuranceFileCnt:=lValue, lProductId:=0, vPolicyArray:=vArray)
                                Else

                                    lValue = CInt(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                                    m_lReturn = m_oBusiness.GetPolicy(lInsuranceFileCnt:=0, lProductId:=lValue, vPolicyArray:=vArray)
                                End If


                                r_vScreenValues(lTemp) = vArray

                            Case GISSharedPropertyConstants.ACOSumInsuredTypeID ' its a sum insured

                                lValue = CInt(m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                                m_lReturn = m_oBusiness.GetSumInsured(lPolicyLinkId:=m_lPolicyLinkId, _
                                                                      sDataModel:=m_sGISDataModel, _
                                                                      lSumInsuredType:=lValue, vSumInsuredArray:=vArray)


                                r_vScreenValues(lTemp) = vArray

                            Case GISSharedPropertyConstants.ACOStdWordingType ' its a standard wording
                                'Retain the existing array if we already have it
                                If Information.IsArray(m_vScreenValues) Then
                                    If Information.IsArray(m_vScreenValues(lTemp)) Then

                                        r_vScreenValues(lTemp) = m_vScreenValues(lTemp)
                                    End If
                                End If

                                If Not Information.IsArray(r_vScreenValues(lTemp)) Then

                                    lGISPropertyID = CInt(m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lTemp))

                                    lGISObjectID = CInt(m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lTemp))

                                    If SubScreen Then
                                        sKeyName = GISDataModel & "_" & Me.ChildObjectName & "_id"
                                        sKeyValue = Mid(ChildOIKey, 3)
                                    Else
                                        sKeyName = ""
                                        sKeyValue = ""
                                    End If


                                    m_lReturn = m_oBusiness.GetStandardWording(lPolicyLinkId:=m_lPolicyLinkId, _
                                                                               lGISPropertyID:=lGISPropertyID, _
                                                                               lGISObjectID:=lGISObjectID, _
                                                                               sDataModel:=m_sGISDataModel, _
                                                                               vStandardWordingArray:=vArray, _
                                                                               sKeyName:=sKeyName, _
                                                                               sKeyValue:=sKeyValue)

                                    r_vScreenValues(lTemp) = vArray
                                End If

                            Case Else

                                If Convert.ToString(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISColumnName, lTemp)).Length >= 11 Then
                                    If Convert.ToString(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISColumnName, lTemp)).Substring(0, 11).ToLower() = "address_cnt" Then
                                        If vValue <> "" Then
                                            lValue = CInt(vValue)

                                            m_lReturn = m_oBusiness.GetAddress(lAddressCnt:=lValue, vAddressArray:=vArray)


                                            r_vScreenValues(lTemp) = vArray
                                        End If
                                    Else
                                        'It's 'just' a value
                                        'But let's call a routine to get the description just in case...

                                        m_lReturn = GetDescription(vValue:=vValue, vDescription:=vDescription, _
                                                                   v_vSpecialsType:=m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp), _
                                                                   v_vSpecialsTypeReference:=m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                                        Dim vArray2(1) As Object
                                        vArray2(0) = vValue
                                        vArray2(1) = vDescription
                                        r_vScreenValues(lTemp) = vArray2
                                    End If
                                Else
                                    'It's 'just' a value
                                    'But let's call a routine to get the description just in case...
                                    m_lReturn = GetDescription(vValue:=vValue, vDescription:=vDescription, _
                                                               v_vSpecialsType:=m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp), _
                                                               v_vSpecialsTypeReference:=m_vScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp))

                                    Dim vArray2(1) As Object
                                    vArray2(0) = vValue
                                    vArray2(1) = vDescription
                                    r_vScreenValues(lTemp) = vArray2
                                End If
                        End Select
                    End If
                Else
                    ' not a property

                    Dim auxVar_2 As Object = m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)


                    'Developer Guide No. 149
                    If Not (Convert.IsDBNull(auxVar_2) Or IsNothing(auxVar_2)) Or Convert.ToString(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)).ToLower() = "work_claim_peril" Then

                        ' This is a list view entry

                        'Developer Guide No 270. 
                        If Convert.ToString(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)).ToLower() = "work_claim_peril" Then
                            sThisChildOIKey = ""
                        End If

                        ' I dont think that this is really doing enything useful but
                        'I did not have time to prove it so I left well alone
                        If sThisChildOIKey <> "" Then
                            sOIKey = sThisChildOIKey
                            sObjectName = sThisChildObjectName
                        Else
                            sOIKey = m_sMyOIKey
                            sObjectName = m_sMyObjectName
                        End If


                        vArray = Nothing

                        sStoreParentObjectName = ""
                        sStoreChildObjectName = ""
                        sStoreParentOIKey = ""
                        sStoreChildOIKey = ""

                        'If we're top level...
                        If Not SubScreen Then


                            'Developer Guide No.101
                            m_lReturn = GetParentForChild(sChildObjectName:=m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp), sParentObjectName:=sStoreParentObjectName, sParentOIKey:=sStoreParentOIKey, _
                                                          lObjectType:=m_vScreenDetails(PBDatabaseConsts.ACDExtraObjectType, lTemp))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                If bBusinessObjectCreatedHere Then
                                    m_lReturn = RemoveBusinessObject()
                                End If
                                Return result
                            End If

                            ' PW110804 - store correct details to retrieve child data
                            sOIKey = m_sParentOIKey
                            sObjectName = m_sParentObjectName
                        Else
                            ' This is a subscreen so the parent to the child list view objects is my object ID
                            sStoreParentObjectName = m_sMyObjectName
                            sStoreParentOIKey = m_sMyOIKey
                        End If

                        ' get child OI keys

                        m_lReturn = m_oGIS.GetChildOIKey(v_sParentObjectName:=sObjectName, v_sParentOIKey:=sOIKey, v_sChildObjectName:=CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)), r_vChildOIKeyArray:=vChildOIKeyArray)

                        sStoreChildObjectName = CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp))

                        'Count how many fields there are on the child screen
                        lCount = 0

                        'This is sufficient - if the screen has nothing, there's no way
                        ' that they could have entered any data.
                        If Information.IsArray(m_vChildScreenDetails) Then
                            For lTemp3 As Integer = m_vChildScreenDetails.GetLowerBound(1) To m_vChildScreenDetails.GetUpperBound(1)

                                If m_vChildScreenDetails(PBDatabaseConsts.ACDGISScreenId, lTemp3).Equals(m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)) Then
                                    lCount += 1
                                End If
                            Next lTemp3
                        End If

                        ' If there are some
                        If Not Information.IsArray(vChildOIKeyArray) Then
                            'Leave space for the child and parent objects and keys
                            ReDim vArray(0, lCount + 3)

                            vArray(0, vArray.GetUpperBound(1) - 3) = sStoreParentObjectName
                            vArray(0, vArray.GetUpperBound(1) - 2) = sStoreChildObjectName
                            vArray(0, vArray.GetUpperBound(1) - 1) = sStoreParentOIKey
                            vArray(0, vArray.GetUpperBound(1)) = sStoreChildOIKey


                        Else
                            'Leave space for the child and parent objects and keys

                            ReDim vArray(vChildOIKeyArray.GetUpperBound(0) + 1, lCount + 3)

                            'vArray(0, vArray.GetUpperBound(1) - 3) = sStoreParentObjectName
                            'vArray(0, vArray.GetUpperBound(1) - 2) = sStoreChildObjectName
                            'vArray(0, vArray.GetUpperBound(1) - 1) = sStoreParentOIKey
                            'vArray(0, vArray.GetUpperBound(1)) = sStoreChildOIKey

                            vArray(0, vArray.GetUpperBound(1) - 3) = sStoreParentObjectName
                            vArray(0, vArray.GetUpperBound(1) - 2) = sStoreChildObjectName
                            vArray(0, vArray.GetUpperBound(1) - 1) = sStoreParentOIKey
                            vArray(0, vArray.GetUpperBound(1)) = sStoreChildOIKey


                            For lTemp2 As Integer = vChildOIKeyArray.GetLowerBound(0) To vChildOIKeyArray.GetUpperBound(0)


                                sChildOIKey = CStr(vChildOIKeyArray(lTemp2))
                                lCount = -1
                                If Information.IsArray(m_vChildScreenDetails) Then 'some specials have no child screend details
                                    For lTemp3 As Integer = m_vChildScreenDetails.GetLowerBound(1) To m_vChildScreenDetails.GetUpperBound(1)


                                        If m_vChildScreenDetails(PBDatabaseConsts.ACDGISScreenId, lTemp3).Equals(m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lTemp)) Then

                                            lCount += 1
                                            ' Get the property - No need to worry too
                                            'much here, as only real value things can
                                            'be selected for the listview

                                            If Not (Convert.IsDBNull(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp3)) Or IsNothing(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp3))) Then

                                                m_lReturn = m_oGIS.GetPropertyValue(v_sObjectName:=CStr(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp3)), v_sPropertyName:=CStr(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraGISPropertyName, lTemp3)), v_sOIKey:=sChildOIKey, r_vPropertyValue:=vValue)
                                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                                    vValue = Nothing
                                                End If

                                                'Oops, not totally true - if we're
                                                'using a PM lookup or user-defined
                                                'we've only got the id. We need the
                                                'description instead...
                                                'Let's call a routine to do this...
                                                m_lReturn = GetDescription(vValue:=vValue, vDescription:=vDescription, v_vSpecialsType:=CInt(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraSpecialsType, lTemp3)), v_vSpecialsTypeReference:=CStr(m_vChildScreenDetails(PBDatabaseConsts.ACDExtraSpecialsTypeReference, lTemp3)))


                                                'vArray(lTemp2 + 1, lCount) = vDescription
                                                vArray(lTemp2 + 1, lCount) = vDescription
                                            Else
                                                vValue = ""
                                            End If
                                        End If

                                    Next lTemp3
                                End If
                                'Store the objects and keys so we can drill down more easily
                                Dim auxVar As Object = m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp)


                                If Convert.IsDBNull(auxVar) Or IsNothing(auxVar) Then
                                    sStoreChildObjectName = ""
                                Else

                                    sStoreChildObjectName = CStr(m_vScreenDetails(PBDatabaseConsts.ACDExtraGISObjectName, lTemp))
                                End If

                                sStoreChildOIKey = sChildOIKey



                                vArray(lTemp2 + 1, vArray.GetUpperBound(1) - 3) = sStoreParentObjectName
                                vArray(lTemp2 + 1, vArray.GetUpperBound(1) - 2) = sStoreChildObjectName
                                vArray(lTemp2 + 1, vArray.GetUpperBound(1) - 1) = sStoreParentOIKey
                                vArray(lTemp2 + 1, vArray.GetUpperBound(1)) = sStoreChildOIKey

                            Next lTemp2

                        End If



                        r_vScreenValues(lTemp) = vArray
                    Else

                        r_vScreenValues(lTemp) = ""
                    End If
                End If
            Next lTemp

            If bBusinessObjectCreatedHere Then
                m_lReturn = RemoveBusinessObject()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            If bBusinessObjectCreatedHere Then
                m_lReturn = RemoveBusinessObject()
            End If
            Return result

        End Try
    End Function

    '*******************************************************************************
    ' Name:         SaveOnLoadRisk
    ' Description:  Calls the business-side function that encapsulates all the
    '               database updates that take place on the loading of a risk.
    ' History:      RAW110804 - created (resilience)
    '*******************************************************************************
    Private Function SaveOnLoadRisk(ByVal v_iTask As Integer, ByRef r_vScreenValues As Object, ByRef r_vRiskDetails As Object, ByRef r_vRiskTypeDetails As Object, ByVal v_lTransactionType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SaveOnLoadRisk"

        Dim oBusinessStateless As Object
        Dim TheInput As XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskIn = XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskIn.CreateInstance()
        Dim TheOutput As XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskOut = XMLTransRiskScreenLoadRisk.RiskScreenLoadRiskOut.CreateInstance()
        Dim sInputXML, sOutputXML, sMsg, sGISXMLDataset As String


        result = gPMConstants.PMEReturnCode.PMTrue

        m_bChildAddStatus = False
        m_bRiskAdd = False
        m_bRiskCopied = False
        m_sReferReasons = ""
        m_sDeclineReasons = ""
        m_sMessages = ""
        m_sQuoteType = ""

        ' Get the business object

        m_lReturn = GetStatelessBusinessObject(r_oObject:=oBusinessStateless) ' RAW 18/08/2004 : Resilience : moved code to a separate function
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get instance of bSIRRiskScreen.Stateless", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_SaveOnLoadRisk ' RAW 23/08/2004 : Resilience : replaced call to exit function
        End If

        ' extract the XML from GIS as a string
        m_lReturn = m_oGIS.ReturnAsXML(r_vXMLDataSet:=sGISXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get XML string from GIS", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_SaveOnLoadRisk ' RAW 23/08/2004 : Resilience : replaced call to exit function
        End If

        'if not launched from Swift Black Box or test harness, or a sub-screen
        If m_lInsuranceFolderCnt <> -1 Or m_bSubScreen Then
            'Populate the XML for the call to the stateless object
            With TheInput
                .iTask = v_iTask
                .iSourceID = m_iSourceId
                .lNavigate = m_lNavigate
                .lProcessMode = m_lProcessMode
                .sTransactionType = m_sTransactionType
                .dtEffectiveDate = m_dtEffectiveDate
                .bSubScreen = m_bSubScreen
                .lScreenId = m_lScreenId
                .lRiskId = m_lRiskId
                .lRiskTypeId = m_lRiskTypeId
                .sGisDataModelCode = m_sGISDataModel
                .lGISDataModelType = m_lGISDataModelType
                .lObjectType = m_lObjectType
                .sGISXMLDataset = sGISXMLDataset
                .sMyOIKey = m_sMyOIKey
                .sMyObjectName = m_sMyObjectName
                .sParentOIKey = m_sParentOIKey
                .sParentObjectName = m_sParentObjectName
                .lPolicyLinkId = m_lPolicyLinkId
                .lInsuranceFolderCnt = m_lInsuranceFolderCnt
                .lInsuranceFileCnt = m_lInsuranceFileCnt


                .vScreenDetailsArray = m_vScreenDetails


                .vScreenValuesArray = r_vScreenValues


                .vRiskDetailsArray = r_vRiskDetails


                .vRiskTypeDetailsArray = r_vRiskTypeDetails
                .lTransactionType = v_lTransactionType
                .lProductId = m_lProductId
                .lPartyCnt = m_lPartyCnt
                .lClaimID = m_lClaimId
                .bCopyRisk = m_bCopyRisk
                .lCaseID = m_lCaseID
            End With

            'Serialize the input
            sInputXML = SerializeRiskScreenLoadRiskIn(oTypeIn:=TheInput)

            ' ensure that byref variables that have been passed into here are passed
            ' on down the process tree. Also pass risk ID by ref, so we get the new
            ' one back.
            ' pass original screen values and claim versioning flag, and the m_bRiskAdd

            sOutputXML = oBusinessStateless.RiskScreenLoadRisk(v_sInput:=sInputXML)

            ' process the results
            TheOutput = DeserializeRiskScreenLoadRiskOut(sOutputXML)

            If TheOutput.HasErrors Then
                sMsg = ComposeErrorString(TheOutput.Errors)
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to save to database - " & sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Exit_SaveOnLoadRisk 'RAW 23/08/2004 : Resilience : replaced call to exit function

            ElseIf TheOutput.HasWarnings Then
                sMsg = ComposeWarningString(TheOutput.Warnings)
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Warning - " & sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                'Nothing too serious so continue
            End If

            'Get the values from the stateless object output
            With TheOutput
                m_lRiskId = .lRiskId
                sGISXMLDataset = .sGISXMLDataset
                m_sMyOIKey = .sMyOIKey
                m_sMyObjectName = .sMyObjectName
                m_sParentOIKey = .sParentOIKey
                m_sParentObjectName = .sParentObjectName
                m_lPolicyLinkId = .lPolicyLinkId


                r_vScreenValues = .vScreenValuesArray


                r_vRiskDetails = .vRiskDetailsArray


                r_vRiskTypeDetails = .vRiskTypeDetailsArray
                m_bChildAddStatus = .bChildAddStatus
                m_bRiskAdd = .bRiskAdded
                m_bRiskCopied = .bRiskCopied
                m_sReferReasons = .sReferReasons ' RAW 20/09/2004 : CQ6832 : added
                m_sDeclineReasons = .sDeclineReasons ' RAW 20/09/2004 : CQ6832 : added
                m_sMessages = .sMessages ' RAW 20/09/2004 : CQ6832 : added
                m_sQuoteType = PBQuoteTypeEncode.GetQuoteTypeDesc(.lQuoteType) ' RAW 20/09/2004 : CQ6832 : added
            End With

        End If

        If m_bSubScreen Then
            'here m_sChildOIKey represents the main object that this screen is handling
            If m_sChildOIKey = "" Then m_sChildOIKey = m_sMyOIKey
            If m_sChildObjectName = "" Then m_sChildObjectName = m_sMyObjectName
        Else
            'here m_sChildOIKey represents the last child object that has been loaded
            If m_sParentOIKey = "" Then m_sParentOIKey = m_sMyOIKey
            If m_sParentObjectName = "" Then m_sParentObjectName = m_sMyObjectName
        End If

        ' now update GIS with the modified XML
        m_lReturn = m_oGIS.LoadFromXML(v_sGisDataModelCode:=m_sGISDataModel, v_sXMLDataSet:=sGISXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to load XML string into GIS", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_SaveOnLoadRisk
        End If

        GoTo Exit_SaveOnLoadRisk

Exit_SaveOnLoadRisk:
        ' terminate the business object.
        If Not (oBusinessStateless Is Nothing) Then

            oBusinessStateless.Dispose()
            oBusinessStateless = Nothing
        End If
        Return result

    End Function

    '*******************************************************************************
    ' Name:         SaveOnOKClick
    ' Description:  Calls the business-side function that encapsulates all the
    '               database updates that take place on an OK click.
    ' History:      RAW110804 - created (resilience)
    '*******************************************************************************
    Private Function SaveOnOKClick(ByVal v_iTask As Integer, ByRef r_vScreenValues As Object, ByVal v_lTransactionType As Integer, ByVal v_bPostQuote As Boolean, ByVal v_dtCoverStartDate As Date) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SaveOnOKClick"

        Dim oBusinessStateless As Object
        Dim TheInput As XMLTransRiskScreenOKClick.RiskScreenOKClickIn = XMLTransRiskScreenOKClick.RiskScreenOKClickIn.CreateInstance()
        Dim TheOutput As XMLTransRiskScreenOKClick.RiskScreenOKClickOut = XMLTransRiskScreenOKClick.RiskScreenOKClickOut.CreateInstance()
        Dim sInputXML, sOutputXML, sMsg, sGISXMLDataset As String


        result = gPMConstants.PMEReturnCode.PMTrue

        m_sReferReasons = ""
        m_sDeclineReasons = ""
        m_sMessages = ""
        m_sQuoteType = ""

        ' Get the business object

        m_lReturn = GetStatelessBusinessObject(r_oObject:=oBusinessStateless)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get instance of bSIRRiskScreen.Stateless", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_SaveOnOKClick
        End If

        ' extract the XML from GIS as a string
        m_lReturn = m_oGIS.ReturnAsXML(r_vXMLDataSet:=sGISXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get XML string from GIS", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_SaveOnOKClick
        End If

        'if launched from Swift Black Box or test harness
        If m_lInsuranceFolderCnt = -1 Then
            'get XML and exit
            m_vXMLDataSet = sGISXMLDataset
            Return result
        End If

        'Populate the XML for the call to the stateless object
        With TheInput
            .iTask = v_iTask
            .iSourceID = g_iSourceID
            .lNavigate = m_lNavigate
            .lProcessMode = m_lProcessMode
            .sTransactionType = m_sTransactionType
            .dtEffectiveDate = m_dtEffectiveDate
            .bSubScreen = m_bSubScreen
            .lScreenId = m_lScreenId
            .lRiskId = m_lRiskId
            .lRiskTypeId = m_lRiskTypeId
            .sGisDataModelCode = m_sGISDataModel
            .lGISDataModelType = m_lGISDataModelType
            .lObjectType = m_lObjectType
            .sGISXMLDataset = sGISXMLDataset
            .sMyOIKey = m_sMyOIKey
            .sMyObjectName = m_sMyObjectName
            .sParentOIKey = m_sParentOIKey
            .sParentObjectName = m_sParentObjectName
            .lPolicyLinkId = m_lPolicyLinkId
            .lInsuranceFileCnt = m_lInsuranceFileCnt


            .vScreenDetailsArray = m_vScreenDetails


            .vScreenValuesArray = r_vScreenValues


            .vRiskDetailsArray = m_vRiskDetails
            .lTransactionType = v_lTransactionType
            .bPostQuote = v_bPostQuote
            .dtCoverStartDate = v_dtCoverStartDate

            .lPartyCnt = m_lPartyCnt
            .dtPolicyStartDate = m_dtPolicyStartDate
            .dtPolicyEndDate = m_dtPolicyEndDate
            .lAgentCnt = m_lAgentCnt
            .lRiskCodeId = m_lRiskCodeId
            .lRiskGroupId = m_lRiskGroupId
            .lCountryId = m_lCountryId
        End With

        'Serialize the input
        sInputXML = SerializeRiskScreenOKClickIn(oTypeIn:=TheInput)

        'Perform the Save!

        sOutputXML = oBusinessStateless.RiskScreenOkClick(v_sInput:=sInputXML)

        ' process the results
        TheOutput = DeserializeRiskScreenOKClickOut(sOutputXML)
        If TheOutput.HasErrors Then
            sMsg = ComposeErrorString(TheOutput.Errors)
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to save to database - " & sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Exit_SaveOnOKClick

        ElseIf TheOutput.HasWarnings Then
            sMsg = ComposeWarningString(TheOutput.Warnings)
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Warning - " & sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            'Nothing too serious so continue
        End If

        'Get the values from the stateless object output
        With TheOutput
            sGISXMLDataset = .sGISXMLDataset


            r_vScreenValues = .vScreenValuesArray


            m_vRiskDetails = .vRiskDetailsArray
            m_sReferReasons = .sReferReasons
            m_sDeclineReasons = .sDeclineReasons
            m_sMessages = .sMessages
            m_sQuoteType = PBQuoteTypeEncode.GetQuoteTypeDesc(.lQuoteType)
        End With

        ' all is ok - now update GIS with the modified XML
        m_lReturn = m_oGIS.LoadFromXML(v_sGisDataModelCode:=m_sGISDataModel, v_sXMLDataSet:=sGISXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to load XML string into GIS", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            GoTo Exit_SaveOnOKClick
        End If

        GoTo Exit_SaveOnOKClick

Exit_SaveOnOKClick:
        ' terminate the business object.
        If Not (oBusinessStateless Is Nothing) Then

            oBusinessStateless.Dispose()
            oBusinessStateless = Nothing
        End If
        Return result

    End Function

    '******************************************************************************
    ' Name:         SaveOnCancel
    ' Description:  Calls the business-side function that encapsulates all the
    '               database updates that take place on cancelling the risk
    '               screen.
    ' History:      RAW110804 - created (resilience)
    '******************************************************************************
    Public Function SaveOnCancel(ByVal v_iTask As Integer, ByVal v_bRevertBackRisk As Boolean) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SaveOnCancel"

        Dim oBusinessStateless As Object
        Dim TheInput As XMLTransRiskScreenCancel.RiskScreenCancelIn = XMLTransRiskScreenCancel.RiskScreenCancelIn.CreateInstance()
        Dim TheOutput As XMLTransRiskScreenCancel.RiskScreenCancelOut = XMLTransRiskScreenCancel.RiskScreenCancelOut.CreateInstance()
        Dim sInputXML, sOutputXML, sMsg, sGISXMLDataset As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the business object

            m_lReturn = GetStatelessBusinessObject(r_oObject:=oBusinessStateless)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get instance of bSIRRiskScreen.Stateless", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                Return result
            End If

            ' extract the XML from GIS as a string
            m_lReturn = m_oGIS.ReturnAsXML(r_vXMLDataSet:=sGISXMLDataset)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to get XML string from GIS", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                Return result
            End If

            'Populate the XML for the call to the stateless object
            With TheInput
                .iTask = v_iTask
                .iSourceID = g_iSourceID
                .lNavigate = m_lNavigate
                .lProcessMode = m_lProcessMode
                .sTransactionType = m_sTransactionType
                .dtEffectiveDate = m_dtEffectiveDate
                .bSubScreen = m_bSubScreen
                .lScreenId = m_lScreenId
                .lRiskId = m_lRiskId
                .sGisDataModelCode = m_sGISDataModel
                .lGISDataModelType = m_lGISDataModelType
                .lObjectType = m_lObjectType
                .sGISXMLDataset = sGISXMLDataset
                .sMyOIKey = m_sMyOIKey
                .sMyObjectName = m_sMyObjectName
                .sParentOIKey = m_sParentOIKey
                .sParentObjectName = m_sParentObjectName
                .lPolicyLinkId = m_lPolicyLinkId
                .bRevertBackRisk = v_bRevertBackRisk
                .lInsuranceFileCnt = m_lInsuranceFileCnt
                .bRiskAdded = m_bRiskAdd
                .bRiskCopied = m_bRiskCopied
            End With

            'Serialize the input
            sInputXML = SerializeRiskScreenCancelIn(oTypeIn:=TheInput)

            'Perfomr the save

            sOutputXML = oBusinessStateless.RiskScreenCancel(v_sInput:=sInputXML)

            ' process the results
            TheOutput = DeserializeRiskScreenCancelOut(sOutputXML)

            If TheOutput.HasErrors Then
                sMsg = ComposeErrorString(TheOutput.Errors)
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to save to database - " & sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            ElseIf TheOutput.HasWarnings Then
                sMsg = ComposeWarningString(TheOutput.Warnings)
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Warning - " & sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
            End If

            With TheOutput
                sGISXMLDataset = .sGISXMLDataset
            End With

            ' now update GIS with the modified XML
            m_lReturn = m_oGIS.LoadFromXML(v_sGisDataModelCode:=m_sGISDataModel, v_sXMLDataSet:=sGISXMLDataset)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(sUsername:=g_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:=" Failed to load XML string into GIS", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)
                Return result
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save on cancel", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


        Finally
            ' terminate the business object.
            If Not (oBusinessStateless Is Nothing) Then

                oBusinessStateless.Dispose()
                oBusinessStateless = Nothing
            End If


        End Try
        Return result
    End Function



    'NOT IN 1.8.6
    '******************************************************************************
    ' Name:         DoUpdate
    ' Description:  contains the original contents of function Update
    ' History:
    ' RAW08092003   CQ2377 created
    '******************************************************************************
    Private Function DoUpdate(ByRef r_vScreenValues As Object) As Integer

        Dim result As Integer = 0

        Dim lTransactionType As Integer
        Dim bPostQuote As Boolean
        Dim dtCoverStartDate As Date



        result = gPMConstants.PMEReturnCode.PMTrue

        ' CTAF 20020809 - Merged in from CNIC - start
        m_lReturn = IsScreenPostQuote(r_bIsPostQuote:=bPostQuote)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = GetBusinessObject()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = RemoveGISInterfaceObject()
            Return result
        End If


        lTransactionType = m_oBusiness.TransactionType

        ' Moved Gis Code to a separate function so that it can be called
        'independently from this update procedure

        If LoadGisFromScreenValues(r_vScreenValues:=r_vScreenValues) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Now save to the database
        m_lReturn = SaveOnOKClick(v_iTask:=Task, r_vScreenValues:=r_vScreenValues, v_lTransactionType:=lTransactionType, v_bPostQuote:=bPostQuote, v_dtCoverStartDate:=dtCoverStartDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = RemoveBusinessObject()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    '******************************************************************************
    ' Name:     GetStatelessBusinessObject
    ' Description:
    ' History:
    '  RAW 18/08/2004 : Resilience : created - based on GetBusinessObject
    '******************************************************************************
    Private Function GetStatelessBusinessObject(ByRef r_oObject As Object) As Integer
        Dim result As Integer = 0
        'Dim bSIRRiskScreen As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (r_oObject Is Nothing) Then
            Return result
        End If

        ' Get an instance of the object via the public object manager.
        Dim temp_r_oObject As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_r_oObject, "bSIRRiskScreen.Stateless", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        r_oObject = temp_r_oObject
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to get an instance of the business object.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create " & "stateless business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatelessBusinessObject")
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name:         DeleteQuote
    ' Description:  Deletes the specified quote from the database (not only
    '               quote output but the policy version too). Note that this
    '               has been written initially to remove a new what-if quote
    '               that has not been saved (where the risk screen has been
    '               cancelled so the quote screen has not even been shown).
    ' History:
    ' 08/03/2005 CJB - Created (as part of PN19313).
    '
    ' ***************************************************************** '
    Public Function DeleteQuote(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vKeyArray As Object
        Dim sPolicyBinderName As String = ""
        Const ACTablePolicyBinder As String = "_Policy_Binder"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' First we'll delete the data model specific quote output
            '--------------------------------------------------------

            ' Construct the policy binder name and flag the instance to be deleted (note that
            ' this will delete all children in the dataset too).
            sPolicyBinderName = m_sGISDataModel & ACTablePolicyBinder

            ' Get the key of the binder as we need it to deleteit
            m_lReturn = m_oGIS.GetAllOIKey(sPolicyBinderName, vKeyArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (Not Information.IsArray(vKeyArray)) Then
                gPMFunctions.RaiseError("m_oGIS.GetAllOIKey", "Error trying to get policy binder keyarray in order to delete a quote.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Flag deletion of the binder and all its children

            m_lReturn = m_oGIS.DelObjectInstance(v_sObjectName:=sPolicyBinderName, v_sOIKey:=CStr(vKeyArray(0)))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGIS.DelObjectInstance", "Error trying to delete a quote.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Actually do the delete
            m_lReturn = m_oGIS.SaveToDB()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGIS.SaveToDB", "Error trying to delete a quote.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Now we need to delete the policy version
            '-----------------------------------------
            m_lReturn = GetBusinessObject()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetBusinessObject", "Error trying to create business object.", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oBusiness.DeletePolicyVersion(v_lInsuranceFileCnt:=m_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.DeletePolicyVersion", "Error trying to delete a quote.", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteQuote", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
End Class

