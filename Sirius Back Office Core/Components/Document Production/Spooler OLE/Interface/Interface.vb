Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '

    Dim frmInterface As frmInterface
    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    Private m_lPMAuthorityLevel As Integer

    Private m_sStepStatus As String = ""

    ' {* USER DEFINED CODE (Begin) *}
    Private m_lDocNumber As Integer
    Private m_sFileName As String = ""
    Private m_lMode As Integer
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sDescription As String = ""
    'DC180401 as requested by Dave Newson (Documaster)
    Private m_lInsuranceFolderCnt As Integer
    'DC180401

    'DC110203 -ISS1460 -cater for archiving of claims documents
    Private m_lClaimCnt As Integer

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property DocNumber() As Integer
        Get
            Return m_lDocNumber
        End Get
        Set(ByVal Value As Integer)
            m_lDocNumber = Value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return m_sFileName
        End Get
        Set(ByVal Value As String)
            m_sFileName = Value
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

    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
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

    'DC180401 as requested by Dave Newson (Documaster)
    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property
    'DC180401
    'DC110203 -ISS1460 -start -cater for archiving claims documents
    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property
    'DC110203 -end

    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
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

    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property

    ' {* USER DEFINED CODE (Begin) *}
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
    Public Function Initialise() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
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
                LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

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
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}

                'UPGRADE_WARNING: (1068) vKeyArray() of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    '            Case PMKeyNameDocumentTypeId
                    '                m_lDocumentTypeId = CLng(vKeyArray(PMKeyValue, lRow&))
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
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 1)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameID
            '    vKeyArray(PMKeyValue, 0) = m_iNameID%

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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.
            'UPGRADE_WARNING: (1037) Couldn't resolve default property of object vSummaryArray(PMKeyName, 0). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameNavigatorTitle1
            'UPGRADE_WARNING: (1037) Couldn't resolve default property of object vSummaryArray(PMKeyValue, 0). More Information: http://www.vbtonet.com/ewis/ewi1037.aspx
            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sNavigatorTitle

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
    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object, ByRef vProcessMode_optional As Object, ByRef vTransactionType_optional As Object, ByRef vEffectiveDate_optional As Object) As gPMConstants.PMEReturnCode
        Dim vTask As Object = Nothing
        If vTask_optional Is Nothing Or Not vTask_optional.Equals(Type.Missing) Then vTask = TryCast(vTask_optional, Object)
        Dim vNavigate As Object = Nothing
        If vNavigate_optional Is Nothing Or Not vNavigate_optional.Equals(Type.Missing) Then vNavigate = TryCast(vNavigate_optional, Object)
        Dim vProcessMode As Object = Nothing
        If vProcessMode_optional Is Nothing Or Not vProcessMode_optional.Equals(Type.Missing) Then vProcessMode = TryCast(vProcessMode_optional, Object)
        Dim vTransactionType As Object = Nothing
        If vTransactionType_optional Is Nothing Or Not vTransactionType_optional.Equals(Type.Missing) Then vTransactionType = TryCast(vTransactionType_optional, Object)
        Dim vEffectiveDate As Object = Nothing
        If vEffectiveDate_optional Is Nothing Or Not vEffectiveDate_optional.Equals(Type.Missing) Then vEffectiveDate = TryCast(vEffectiveDate_optional, Object)
        Try

            Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
            Try

                result = gPMConstants.PMEReturnCode.PMTrue

                ' Assign the process modes to the property members.

                If Not Not (vTask_optional Is Nothing) AndAlso vTask_optional.Equals(Type.Missing) Then
                    'UPGRADE_WARNING: (1068) vTask of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    m_iTask = CInt(vTask)
                End If

                If Not Not (vNavigate_optional Is Nothing) AndAlso vNavigate_optional.Equals(Type.Missing) Then
                    'UPGRADE_WARNING: (1068) vNavigate of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    m_lNavigate = CInt(vNavigate)
                End If

                If Not Not (vProcessMode_optional Is Nothing) AndAlso vProcessMode_optional.Equals(Type.Missing) Then
                    'UPGRADE_WARNING: (1068) vProcessMode of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    m_lProcessMode = CInt(vProcessMode)
                End If

                If Not Not (vTransactionType_optional Is Nothing) AndAlso vTransactionType_optional.Equals(Type.Missing) Then
                    'UPGRADE_WARNING: (1068) vTransactionType of type Variant is being forced to String. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    m_sTransactionType = CStr(vTransactionType)
                End If

                If Not Not (vEffectiveDate_optional Is Nothing) AndAlso vEffectiveDate_optional.Equals(Type.Missing) Then
                    'UPGRADE_WARNING: (1068) vEffectiveDate of type Variant is being forced to Date. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                    m_dtEffectiveDate = CDate(vEffectiveDate)
                End If

                Return result

            Catch excep As System.Exception




                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Return result

            End Try
        Finally
            vTask_optional = vTask
            vNavigate_optional = vNavigate
            vProcessMode_optional = vProcessMode
            vTransactionType_optional = vTransactionType
            vEffectiveDate_optional = vEffectiveDate
        End Try
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object, ByRef vProcessMode_optional As Object, ByRef vTransactionType_optional As Object) As gPMConstants.PMEReturnCode
        Dim tempRefParam As Object = Type.Missing
        Return SetProcessModes(vTask_optional, vNavigate_optional, vProcessMode_optional, vTransactionType_optional, tempRefParam)
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object, ByRef vProcessMode_optional As Object) As gPMConstants.PMEReturnCode
        Dim tempRefParam2 As Object = Type.Missing
        Dim tempRefParam3 As Object = Type.Missing
        Return SetProcessModes(vTask_optional, vNavigate_optional, vProcessMode_optional, tempRefParam2, tempRefParam3)
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object, ByRef vNavigate_optional As Object) As gPMConstants.PMEReturnCode
        Dim tempRefParam4 As Object = Type.Missing
        Dim tempRefParam5 As Object = Type.Missing
        Dim tempRefParam6 As Object = Type.Missing
        Return SetProcessModes(vTask_optional, vNavigate_optional, tempRefParam4, tempRefParam5, tempRefParam6)
    End Function

    Public Function SetProcessModes(ByRef vTask_optional As Object) As gPMConstants.PMEReturnCode
        Dim tempRefParam7 As Object = Type.Missing
        Dim tempRefParam8 As Object = Type.Missing
        Dim tempRefParam9 As Object = Type.Missing
        Dim tempRefParam10 As Object = Type.Missing
        Return SetProcessModes(vTask_optional, tempRefParam7, tempRefParam8, tempRefParam9, tempRefParam10)
    End Function

    Public Function SetProcessModes() As gPMConstants.PMEReturnCode
        Dim tempRefParam11 As Object = Type.Missing
        Dim tempRefParam12 As Object = Type.Missing
        Dim tempRefParam13 As Object = Type.Missing
        Dim tempRefParam14 As Object = Type.Missing
        Dim tempRefParam15 As Object = Type.Missing
        Return SetProcessModes(tempRefParam11, tempRefParam12, tempRefParam13, tempRefParam14, tempRefParam15)
    End Function

    ' ***************************************************************** '
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the interface processing.
            m_lReturn = ProcessInterface()

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
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: ProcessInterface (Standard Method)
    '
    ' Description: Calls the appropriate methods to process the
    '              interface.
    '
    ' ***************************************************************** '
    Private Function ProcessInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = LoadInterface()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Display the interface.
        m_lReturn = ShowInterface(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to display the inteface.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Destroy the interface from memory.
        m_lReturn = UnLoadInterface()

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
    Private Function LoadInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        frmInterface = New frmInterface

        ' Assign the parameters to the interface properties.
        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            .FileName = m_sFileName
            .Mode = m_lMode
            .PartyCnt = m_lPartyCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            'DN 09/01/02 - Pass the FolderCnt as this is now used for archiving
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .Description = m_sDescription
            'DC110203 -ISS1460 -cater for claims documents
            .ClaimCnt = m_lClaimCnt

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        Dim tempLoadForm As frmInterface = frmInterface

        ' Check if we have had an error so far.
        If frmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            result = frmInterface.ErrorNumber
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnLoadInterface (Standard Method)
    '
    ' Description: Unloads the instance of the interface from memory.
    '
    ' ***************************************************************** '
    Private Function UnLoadInterface() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus
            m_lDocNumber = .DocNumber

            ' {* USER DEFINED CODE (Begin) *}

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ShowInterface (Standard Method)
    '
    ' Description: Displays the instance of the interface using the
    '              display state.
    '
    ' ***************************************************************** '
    Private Function ShowInterface(ByRef lDisplayState As Integer) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = frmInterface.ProcessForm()

        ' Display the interface.
        VB6.ShowForm(frmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If frmInterface.ErrorNumber <> 0 Then
                result = frmInterface.ErrorNumber
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
        'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

