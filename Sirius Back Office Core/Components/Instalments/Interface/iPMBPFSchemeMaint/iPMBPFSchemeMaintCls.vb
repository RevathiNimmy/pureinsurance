Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 17/02/1997
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' SP011298 - changes to support new business roadmap
    ' SP130199 - Change for Tim
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lSchemeNo As Integer
    Private m_lSchemeVersion As Integer
    Private m_sSchemeName As String = ""

    Private m_lPartyCnt As Integer
    Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""

    ' Member to hold instance of form interface
    Private m_ofrmInterface As iPMBPFSchemeMaint.frmInterface

    Private m_bDeleteMode As Boolean
    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.1)
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.1)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            m_lPMAuthorityLevel = CStr(Value)

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property
    ' {* USER DEFINED CODE (Begin) *}
    Public Property SchemeNo() As Integer
        Get

            Return m_lSchemeNo

        End Get
        Set(ByVal Value As Integer)

            m_lSchemeNo = Value

        End Set
    End Property

    Public Property SchemeVersion() As Integer
        Get

            Return m_lSchemeVersion

        End Get
        Set(ByVal Value As Integer)

            m_lSchemeVersion = Value

        End Set
    End Property

    Public ReadOnly Property SchemeName() As String
        Get

            Return m_sSchemeName

        End Get
    End Property

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property PartyCode() As String
        Get

            Return m_sPartyCode

        End Get
        Set(ByVal Value As String)

            m_sPartyCode = Value

        End Set
    End Property

    Public ReadOnly Property PartyName() As String
        Get

            Return m_sPartyName

        End Get
    End Property
    ' {* USER DEFINED CODE (End) *}
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
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Dim sMessage, sTitle As String
        Dim sHelpFile As String = String.Empty
        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Set the object manager to nothing.
                g_oObjectManager = Nothing

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                'eck220500
                g_iUserID = .UserID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRPremiumFinance.Maintenance", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                'Developer Guide No 243
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                'Developer Guide No 243
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = PMProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            'Find out from the registry where the Help File is
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrieve Helpfile", Application.ProductName)
                Return result
            End If

            If sHelpFile <> "" Then
                'refer Developer Guide No. 38(No Solutions)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
                If g_oBusiness IsNot Nothing Then
                    g_oBusiness.Dispose()
                    g_oBusiness = Nothing
                End If
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a valid array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}

                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    Case PMNavKeyConst.PMKeyNameSchemeNo

                        m_lSchemeNo = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameSchemeVersion

                        m_lSchemeVersion = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameShortName

                        m_sPartyCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.

            ReDim vKeyArray(1, 11)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameSchemeNo

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lSchemeNo

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameSchemeVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lSchemeVersion

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.PMKeyNamePartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPartyCnt

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.PMKeyNameShortName

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_sPartyCode

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Standard Method)
    '
    ' Description: Stores all of the summary array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            'SP011298 - Changes to support roadmap (if nowt to do return vSummarryArray="")
            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Scheme Name"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sSchemeName

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Scheme Version"

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_lSchemeVersion

            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Set the process modes for the business object.
            If Not (g_oBusiness Is Nothing) Then

                m_lReturn = g_oBusiness.SetProcessModes(vTask:=vTask, vNavigate:=vNavigate, vProcessMode:=vProcessMode, vTransactionType:=vTransactionType, vEffectiveDate:=vEffectiveDate)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.

                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes")

                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer
        'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.2)
        Dim result As Integer = 0
        Dim sValue As String = ""
        Const kMethodName As String = "Start"
        'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.2)

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.2)

            ' Get System Option for Disable Wildcard Search
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionDisableWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for DisableWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bDisableWildcardSearchOption = (sValue = "1")

            ' Get System Option for m_bEnablePartialWildcardSearchOption
            m_lReturn = CType(iPMFunc.GetSystemOption(kSystemOptionEnablePartialWildcardSearch, sValue), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption for EnablePartialWildcardSearch Failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If
            m_bEnablePartialWildcardSearchOption = (sValue = "1")
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.2)




            ' Check if the SchemeNo is greater than zero. If so,
            ' there is no need to proceed with the interface. We
            ' can therefore return straight back out.
            If m_lSchemeNo > 0 Then
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
            Else
                ' Starts the interface processing.
                m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Failed to process the interface.
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetailsFromPMBArray
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetDetailsFromPMBArray(ByVal v_lInvariantKey As Integer, Optional ByRef r_sClientCode As String = "", Optional ByRef r_sClientName As String = "", Optional ByRef r_sAddress1 As String = "", Optional ByRef r_sAddress2 As String = "", Optional ByRef r_sAddress3 As String = "", Optional ByRef r_sAddress4 As String = "", Optional ByRef r_sPostCode As String = "", Optional ByRef r_sPortfolio As String = "", Optional ByRef r_sCustomerId As String = "", Optional ByRef r_sStatus As String = "", Optional ByRef r_sTelNo As String = "", Optional ByRef r_sAltTelNo As String = "", Optional ByRef r_sDOB As String = "", Optional ByRef r_sSex As String = "", Optional ByRef r_sMarried As String = "", Optional ByRef r_sChildren As String = "") As Integer

        Dim result As Integer = 0

        Try



            Return g_oBusiness.GetDetailsFromPMBArray(v_lInvariantKey:=v_lInvariantKey, r_sClientCode:=r_sClientCode, r_sClientName:=r_sClientName, r_sAddress1:=r_sAddress1, r_sAddress2:=r_sAddress2, r_sAddress3:=r_sAddress3, r_sAddress4:=r_sAddress4, r_sPostCode:=r_sPostCode, r_sPortfolio:=r_sPortfolio, r_sCustomerId:=r_sCustomerId, r_sStatus:=r_sStatus, r_sTelNo:=r_sTelNo, r_sAltTelNo:=r_sAltTelNo, r_sDOB:=r_sDOB, r_sSex:=r_sSex, r_sMarried:=r_sMarried, r_sChildren:=r_sChildren)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetailsFromPMBArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsFromPMBArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetID (Standard Method)
    '
    ' Description: Gets the ID for the search parameter from the
    '              business object.
    '
    ' ***************************************************************** '
    Public Function GetID(ByRef lId As Integer, Optional ByRef vShortName As Object = Nothing, Optional ByRef vName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the ID from the busines object.

            m_lReturn = g_oBusiness.GetID(lId:=lId, vShortName:=vShortName, vName:=vName)

            ' Check for errors (PMNotFound=OK)
            If (m_lReturn = gPMConstants.PMEReturnCode.PMFalse) Or (m_lReturn = gPMConstants.PMEReturnCode.PMError) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID")
                Return result
            End If

            ' Return the value.

            Return m_lReturn

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the ID from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = CType(UnLoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to unload the interface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: LoadInterface (Standard Method)
    '
    ' Description: Loads the instance of the interface into memory and
    '              passes the parameters in.
    '
    ' ***************************************************************** '
    Private Function LoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_ofrmInterface = New iPMBPFSchemeMaint.frmInterface()

        ' Assign the parameters to the interface properties.
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate
            ' {* USER DEFINED CODE (Begin) *}

            .SchemeNo = m_lSchemeNo
            .SchemeVersion = m_lSchemeVersion

            .PartyCnt = m_lPartyCnt
            .PartyCode = m_sPartyCode
            ' {* USER DEFINED CODE (End) *}
            'Start (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.3)
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
            'End (Girija chokkalingam) - (Tech Spec - NEM - Wild Card Search.doc) - (5.7.1.3)
        End With


        'refer Developer Guide No. 68(No solutions)

        ' Check if we have had an error so far.
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = m_ofrmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With m_ofrmInterface
            m_lStatus = .Status

            ' {* USER DEFINED CODE (Begin) *}

            m_lSchemeNo = .SchemeNo
            m_lSchemeVersion = .SchemeVersion
            m_sSchemeName = .SchemeName

            m_lPartyCnt = .PartyCnt
            m_sPartyCode = .PartyCode
            m_sPartyName = .PartyName
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        m_ofrmInterface.Close()
        m_ofrmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display the interface.
        VB6.ShowForm(m_ofrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If m_ofrmInterface.ErrorNumber <> 0 Then
                result = m_ofrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Private Shared _DefaultInstance As Interface_Renamed = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Interface_Renamed
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Interface_Renamed
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class

