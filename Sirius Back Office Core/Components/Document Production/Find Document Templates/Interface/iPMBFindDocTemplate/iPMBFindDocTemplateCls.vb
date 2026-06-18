Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface

    Private Const ACClass As String = "Interface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lDocumentTemplateId As Integer
    Private m_lDocumentTypeId As Integer
    Private m_sDocumentCode As String = ""
    Private m_sDocumentTypeCode As String = ""
    Private m_sDocumentTemplateDescription As String = ""

    Private m_vDocumentSplit(,) As Object

    Private m_lSourceId As Integer

    Private m_lUserID As Integer 'MKW190903 PN6943

    Private m_lMode As Integer

    Private m_lRiskTypeId As Integer

    Private m_lProductId As Integer

    Private m_lGISPropertyID As Integer
    Private m_lGISObjectID As Integer


    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Private m_lInsuranceFileCnt As Integer
    Private m_lProcessType As Integer

    Private m_sDocumentTypeDescription As String = "" 'MKW 281003 PN7287 1.8.5 to 1.8.6 catchup

    Private m_vSourceArray As Object 'MKW190903 PN6943
    Private m_lKeepOnTop As Integer
    Private m_bDisableWildcardSearchOption As Boolean
    Private m_bEnablePartialWildcardSearchOption As Boolean
    Private Const kSystemOptionDisableWildcardSearch As Integer = 5065
    Private Const kSystemOptionEnablePartialWildcardSearch As Integer = 5066
    Dim objfrmInteface As frmInterface
    Public Property GISPropertyID() As Integer
        Get
            Return m_lGISPropertyID
        End Get
        Set(ByVal Value As Integer)
            m_lGISPropertyID = Value
        End Set
    End Property

    Public Property GISObjectID() As Integer
        Get
            Return m_lGISObjectID
        End Get
        Set(ByVal Value As Integer)
            m_lGISObjectID = Value
        End Set
    End Property


    Public Property DocumentTypeDescription() As String
        Get
            Return m_sDocumentTypeDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTypeDescription = Value
        End Set
    End Property

    Public Property ProcessType() As Integer
        Get
            Return m_lProcessType
        End Get
        Set(ByVal Value As Integer)
            m_lProcessType = Value
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

    Public WriteOnly Property RiskTypeId() As Integer
        Set(ByVal Value As Integer)
            m_lRiskTypeId = Value
        End Set
    End Property

    Public WriteOnly Property ProductId() As Integer
        Set(ByVal Value As Integer)
            m_lProductId = Value
        End Set
    End Property

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

            m_lPMAuthorityLevel = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            Return m_lStatus

        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
    Public ReadOnly Property DocumentSplit() As Object
        Get

            Return VB6.CopyArray(m_vDocumentSplit)

        End Get
    End Property

    Public Property DocumentTemplateId() As Integer
        Get

            Return m_lDocumentTemplateId

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentTemplateId = Value

        End Set
    End Property

    Public Property DocumentTemplateDescription() As String
        Get
            Return m_sDocumentTemplateDescription
        End Get
        Set(ByVal Value As String)
            m_sDocumentTemplateDescription = Value
        End Set
    End Property


    Public Property DocumentTypeId() As Integer
        Get

            Return m_lDocumentTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lDocumentTypeId = Value

        End Set
    End Property

    Public Property DocumentCode() As String
        Get

            Return m_sDocumentCode

        End Get
        Set(ByVal Value As String)

            m_sDocumentCode = Value

        End Set
    End Property

    Public Property DocumentTypeCode() As String
        Get

            Return m_sDocumentTypeCode

        End Get
        Set(ByVal Value As String)

            m_sDocumentTypeCode = Value

        End Set
    End Property

    Public Property Mode() As Integer
        Get
            Return m_lMode
        End Get
        Set(ByVal Value As Integer)
            m_lMode = Value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get

            Return m_lSourceId

        End Get
        Set(ByVal Value As Integer)

            m_lSourceId = Value

        End Set
    End Property 'MKW190903 PN6943

    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)

            m_dtEffectiveDate = CDate(Value)
        End Set
    End Property

    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sMessage, sTitle As String

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
                m_lUserID = .UserID 'MKW190903 PN6943
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_g_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_g_oBusiness, "bSIRFindDocTemplate.Form", vInstanceManager:="ClientManager")
            g_oBusiness = temp_g_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return result
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
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
                    ' CTAF 160600
                    Case PMNavKeyConst.PMKeyNameDocumentTypeId

                        m_lDocumentTypeId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'RWH(26/09/2000) RSAIB Process 28.
                    Case PMNavKeyConst.PMKeyNameInsuranceFileCnt

                        m_lInsuranceFileCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                        'RWH(26/09/2000) RSAIB Process 28.
                    Case PMNavKeyConst.PMKeyNameProcessType

                        m_lProcessType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameDocTemplateMode 'PN15032

                        m_lMode = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))

                    Case PMNavKeyConst.PMKeyNameKeepWindowOnTop

                        m_lKeepOnTop = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

                ' {* USER DEFINED CODE (End) *}
            Next lRow

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
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

            ReDim vKeyArray(1, 1)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameDocumentTemplateId
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lDocumentTemplateId
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.PMKeyNameDocumentTypeId
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_lDocumentTypeId

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


            ReDim vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0)

            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummHeading, 0) = "Document Template"
            vSummaryArray(gPMConstants.PMENavSummaryArrayColPosition.PMNavSummValue, 0) = m_sDocumentCode

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
            If g_oBusiness Is Nothing Then


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
        Dim result As Integer = 0
        Dim sValue As String = ""
        Const kMethodName As String = "Start"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            m_lReturn = CType(GetValidSources(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Starts the interface processing.
            m_lReturn = CType(ProcessInterface(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
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
    Public Function GetID() As Integer
        Return gPMConstants.PMEReturnCode.PMTrue
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


        'RWH(26/09/2000) RSAIB Process 28. Auto doc prod.
        If m_lMode = ACInvisibleMergeMode Then

            m_lReturn = EstablishRequiredTemplate()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        End If

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'RWH(27/09/2000)
        If m_lMode <> ACInvisibleMergeMode Then
            If m_lKeepOnTop = 1 Then

                m_lReturn = CType(iPMFunc.SetWindowPlacement(objfrmInteface.Handle.ToInt32(), True), gPMConstants.PMEReturnCode)
            End If

            ' Display the interface.
            m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'RWH(27/09/2000) This in done in the 'UnloadInterface' routine below.
        '    m_lDocumentTemplateId = frmInterface.DocumentTemplateId
        '    m_sDocumentCode = frmInterface.DocumentCode
        '    m_lDocumentTypeId = frmInterface.DocumentTypeId
        '    m_sDocumentTypeCode = frmInterface.DocumentTypeCode

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
        objfrmInteface = New frmInterface


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.


        With objfrmInteface
            .CallingAppName = m_sCallingAppName
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}

            .DocumentCode = m_sDocumentCode
            .DocumentTypeId = m_lDocumentTypeId
            .Mode = m_lMode
            .SourceId = m_lSourceId





            .SourceArray = m_vSourceArray 'MKW190903 PN6943

            'RWH(26/09/2000) RSAIB Process 28.
            If m_lMode = ACInvisibleMergeMode Then
                .InsuranceFileCnt = m_lInsuranceFileCnt
                .ProcessType = m_lProcessType
            End If

            .RiskTypeId = m_lRiskTypeId
            .ProductId = m_lProductId
            .GISPropertyID = m_lGISPropertyID
            .GISObjectID = m_lGISObjectID
            .DisableWildcardSearchOption = m_bDisableWildcardSearchOption
            .EnablePartialWildcardSearchOption = m_bEnablePartialWildcardSearchOption
        End With

        ' Load the instance of the interface into memory.


        ' Check if we have had an error so far.

        If objfrmInteface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = objfrmInteface.ErrorNumber
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


        With objfrmInteface
            m_lStatus = .Status

            If m_lStatus <> gPMConstants.PMEReturnCode.PMCancel Then
                ' {* USER DEFINED CODE (Begin) *}
                m_lDocumentTemplateId = .DocumentTemplateId
                m_sDocumentCode = .DocumentCode
                m_lDocumentTypeId = .DocumentTypeId
                m_sDocumentTypeCode = .DocumentTypeCode
                m_sDocumentTemplateDescription = .DocumentTemplateDescription
                m_sDocumentTypeDescription = .DocumentTypeDescription 'MKW 281003 PN7287 1.8.5 to 1.8.6 catchup
                ' {* USER DEFINED CODE (End) *}
            End If
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.

        objfrmInteface.Close()
        objfrmInteface = Nothing

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

        VB6.ShowForm(objfrmInteface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.

            If objfrmInteface.ErrorNumber <> 0 Then
                result = objfrmInteface.ErrorNumber
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


    ' ***************************************************************** '
    '
    ' Name: Establish RequiredTemplate
    '
    ' Description:
    '
    ' History: 26/09/2000 RWH - Created.
    '        : PW160702 - change to use Process Types Docs lookup table
    '                     instead of constants
    ' ***************************************************************** '
    Private Function EstablishRequiredTemplate() As Integer

        Dim result As Integer = 0
        Dim sTemplateCode As String = ""
        Dim lReportPointer As Integer
        Dim sBusinessType As String = ""
        Dim bDocumentSplit As Boolean

        ' PW160702
        Dim vResultArray(,) As Object
        Dim vTabArray(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue
        '
        ' PW160702 - use the new lookups table to get Process Type Code
        '


        vResultArray = Nothing
        ReDim vTabArray(3, 0)


        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "process_types_docs"

        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = m_lProcessType

        ' PW160702 - Get the Lookup item for current process type

        m_lReturn = g_oBusiness.GetProcessTypesLookupValues(iLookupType:=gPMConstants.PMELookupType.PMLookupSingle, vTableArray:=vTabArray, iLanguageID:=g_iLanguageID, vResultArray:=vResultArray)

        ' PW160702 - Assigned returned lookup type

        sTemplateCode = CStr(vResultArray(2, 0)).Trim()

        ' PW160702 - If cancellation...
        If sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancel Then


            m_lReturn = g_oBusiness.GetBusinessType(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_sBusinessType:=sBusinessType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case sBusinessType
                Case "DIRECT"
                    sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelClient
                Case Else
                    sTemplateCode = gSIRLibrary.SIRDocTypeCodeCancelAgent
            End Select

        End If

        sTemplateCode = sTemplateCode & m_sTransactionType.Trim()


        m_lReturn = g_oBusiness.GetReportPointer(m_lInsuranceFileCnt, lReportPointer)

        '    If (m_lReturn <> PMTrue) Then
        '        'Log error.
        '        EstablishRequiredTemplate = PMFalse
        '     ' Log Error Message
        '        LogMessage _
        ''            iType:=PMLogOnError, _
        ''            sMsg:="Failed to get report_pointer", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="EstablishRequiredTemplate"
        '        Exit Function
        '    End If

        If lReportPointer <> 0 Then
            sTemplateCode = sTemplateCode & CStr(lReportPointer)
        End If

        'Check Process_Types_Docs table and check allow_split_documents flag.


        m_lReturn = g_oBusiness.GetProcessTypesDocsSplitStatus(gPMFunctions.ToSafeLong(CStr(vResultArray(0, 0))), bDocumentSplit)

        If bDocumentSplit Then
            ReDim m_vDocumentSplit(1, 2)


            m_lReturn = g_oBusiness.GetAvailableTemplate(v_sTemplateCode:="1" & sTemplateCode, r_lTemplateId:=m_lDocumentTemplateId, r_lTemplateTypeId:=m_lDocumentTypeId, r_sDocDescription:=m_sDocumentTemplateDescription, v_dtEffectiveDate:=m_dtEffectiveDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                m_vDocumentSplit(0, 0) = 0
                m_vDocumentSplit(1, 0) = 0
            Else
                m_vDocumentSplit(0, 0) = m_lDocumentTemplateId
                m_vDocumentSplit(1, 0) = m_lDocumentTypeId
            End If


            m_lReturn = g_oBusiness.GetAvailableTemplate(v_sTemplateCode:="2" & sTemplateCode, r_lTemplateId:=m_lDocumentTemplateId, r_lTemplateTypeId:=m_lDocumentTypeId, r_sDocDescription:=m_sDocumentTemplateDescription, v_dtEffectiveDate:=m_dtEffectiveDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                m_vDocumentSplit(0, 1) = 0
                m_vDocumentSplit(1, 1) = 0
            Else
                m_vDocumentSplit(0, 1) = m_lDocumentTemplateId
                m_vDocumentSplit(1, 1) = m_lDocumentTypeId
            End If


            m_lReturn = g_oBusiness.GetAvailableTemplate(v_sTemplateCode:="3" & sTemplateCode, r_lTemplateId:=m_lDocumentTemplateId, r_lTemplateTypeId:=m_lDocumentTypeId, r_sDocDescription:=m_sDocumentTemplateDescription, v_dtEffectiveDate:=m_dtEffectiveDate)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                m_vDocumentSplit(0, 2) = 0
                m_vDocumentSplit(1, 2) = 0
            Else
                m_vDocumentSplit(0, 2) = m_lDocumentTemplateId
                m_vDocumentSplit(1, 2) = m_lDocumentTypeId
            End If

            If (CDbl(m_vDocumentSplit(0, 0)) = 0) And (CDbl(m_vDocumentSplit(0, 1)) = 0) And (CDbl(m_vDocumentSplit(0, 2)) = 0) Then
                bDocumentSplit = False

                m_vDocumentSplit = Nothing
            End If
        End If

        If Not (bDocumentSplit) Then
            'Ensure template exists. If not, apply rules until  suitable template is found.

            m_lReturn = g_oBusiness.GetAvailableTemplate(v_sTemplateCode:=sTemplateCode, r_lTemplateId:=m_lDocumentTemplateId, r_lTemplateTypeId:=m_lDocumentTypeId, r_sDocDescription:=m_sDocumentTemplateDescription, v_dtEffectiveDate:=m_dtEffectiveDate)


            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    'That's OK.
                Case gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Template not found with code '" & sTemplateCode & "'.", "Find Document Template", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    'RKS PN13490
                    Return gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    'Log error.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get available template", vApp:=ACApp, vClass:=ACClass, vMethod:="EstablishRequiredTemplate")
                    Return result
            End Select
        End If

        Return result

    End Function

    'MKW190903 PN6943
    Private Function GetValidSources() As Integer
        Dim result As Integer = 0

        Dim oPMUser As bPMUser.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim temp_oPMUser As Object
        m_lReturn = g_oObjectManager.GetInstance(temp_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oPMUser = temp_oPMUser

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error.
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If


        m_lReturn = oPMUser.GetUserSources(r_vSourceArray:=m_vSourceArray, v_vUserID:=m_lUserID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get valid sources", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSources", excep:=New Exception(Information.Err().Description))

            Return result
        End If


        '    ' Remove instance of PMUser
        If Not (oPMUser Is Nothing) Then

            oPMUser.Dispose()
            oPMUser = Nothing
        End If

        Return result

    End Function
End Class

