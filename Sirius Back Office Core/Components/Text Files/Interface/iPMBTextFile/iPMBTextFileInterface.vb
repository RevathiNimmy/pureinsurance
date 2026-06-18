Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Imports System.IO

<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 10/05/1999
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


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
    Private m_lPartyCnt As Integer
    Private m_lInsuranceFolderCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_lClaimCnt As Integer

    Private m_lRiskCodeId As Integer
    Private m_lRiskGroupId As Integer

    Private m_lDocumentTypeId As Integer
    Private m_lDocumentTemplateId As Integer

    Private m_lSlotNumber As Integer
    Private m_lFileNumber As Integer

    Private m_lMode As Integer

    Private m_lSourceId As Integer

    Private m_sWordVersion As String = ""

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the interface.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sDocumentTemplateDescription As String = ""
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    'Developer Guide No. 50
    Dim objfrmInteface As frmInterface

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

    Public Property InsuranceFolderCnt() As Integer
        Get
            Return m_lInsuranceFolderCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFolderCnt = Value
        End Set
    End Property

    Public Property ClaimCnt() As Integer
        Get
            Return m_lClaimCnt
        End Get
        Set(ByVal Value As Integer)
            m_lClaimCnt = Value
        End Set
    End Property

    Public Property RiskCodeId() As Integer
        Get
            Return m_lRiskCodeId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskCodeId = Value
        End Set
    End Property

    Public Property RiskGroupId() As Integer
        Get
            Return m_lRiskGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lRiskGroupId = Value
        End Set
    End Property

    Public Property SlotNumber() As Integer
        Get
            Return m_lDocumentTemplateId
        End Get
        Set(ByVal Value As Integer)
            m_lSlotNumber = Value
        End Set
    End Property

    Public Property FileNumber() As Integer
        Get
            Return m_lFileNumber
        End Get
        Set(ByVal Value As Integer)
            m_lFileNumber = Value
        End Set
    End Property

    Public Property SourceId() As Integer
        Get
            Return m_lSourceId
        End Get
        Set(ByVal Value As Integer)
            m_lSourceId = Value
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

    Public WriteOnly Property DocumentTemplateDescription() As String
        Set(ByVal Value As String)
            m_sDocumentTemplateDescription = Value
        End Set
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
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

            'SJ 27/07/2004 - start
            bPMDocFunctions.Username = g_oObjectManager.UserName
            'SJ 27/07/2004 - end

            ' CTAF 021100 - Get instance of bPMZipper
            g_oZipper = New bPMZipper.Business()

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
                g_sUserName = .UserName 'MKW150703 PN5359
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

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
                If g_oObjectManager IsNot Nothing Then
                    g_oObjectManager.Dispose()
                    g_oObjectManager = Nothing
                End If
                If Not (g_oZipper Is Nothing) Then

                    g_oZipper = Nothing

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

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.

                ' {* USER DEFINED CODE (Begin) *}


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    '            Case PMKeyNameID
                    '                m_iNameID% = CInt(vKeyArray(PMKeyValue, lRow&))
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
    Public Function GetSummary(ByRef vSummaryArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.

            vSummaryArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.PMKeyNameNavigatorTitle1

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

        'Developer Guide No. 50
        objfrmInteface = New frmInterface


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load the interface into memory.
        m_lReturn = CType(LoadInterface(), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to load the interface.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'We're printing...

        If m_iTask = 20 Then

            'Developer Guide No. 50
            m_lReturn = objfrmInteface.PrintDocument()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Developer Guide No. 50
            m_lReturn = objfrmInteface.DeleteClient()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Else

            ' Display the interface.
            m_lReturn = CType(ShowInterface(lDisplayState:=FormShowConstants.Modal), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to display the inteface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
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
        Dim sFilePath As String = ""
        'Developer Guide No. 50


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Assign the parameters to the interface properties.

        'Developer Guide No. 50



        With objfrmInteface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            .Mode = m_lMode
            .PartyCnt = m_lPartyCnt
            .InsuranceFileCnt = m_lInsuranceFileCnt
            .InsuranceFolderCnt = m_lInsuranceFolderCnt
            .ClaimCnt = m_lClaimCnt
            .RiskCodeId = m_lRiskCodeId
            .RiskGroupId = m_lRiskGroupId
            .SlotNumber = m_lSlotNumber
            .FileNumber = m_lFileNumber
            .SourceId = m_lSourceId 'DJM 08/05/2002 : Changed to use the correct Source ID (not g_iSourceID)
            .WordVersion = m_sWordVersion
            .DocumentTemplateDescription = m_sDocumentTemplateDescription
            ' {* USER DEFINED CODE (End) *}
        End With

        'Developer Guide No. 50
        m_lReturn = CType(objfrmInteface.GetFilePath(sPath:=sFilePath), gPMConstants.PMEReturnCode)
        If FileSystem.Dir(sFilePath, FileAttribute.Normal) = "" Then
            MessageBox.Show("The document requested does not exist. " & Strings.Chr(13) & Strings.Chr(10) & _
                            " Path : " & sFilePath, "Client Manager - Text Files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Developer Guide No. 50
        objfrmInteface.FileNumber = m_lFileNumber


        ' Load the instance of the interface into memory.
        objfrmInteface.frmInterfaceLoad()
        ' Check if we have had an error so far.
        'Developer Guide No. 50
        If objfrmInteface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            'Developer Guide No. 50
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
        'Developer Guide No. 50

        With objfrmInteface
            m_lStatus = .Status
            m_sStepStatus = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}
            'HERE!!!!
            m_lSlotNumber = .SlotNumber
            m_lFileNumber = .FileNumber

            ' {* USER DEFINED CODE (End) *}

        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        'Developer Guide No. 50

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
        'Developer Guide No. 50
        VB6.ShowForm(objfrmInteface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            If objfrmInteface.ErrorNumber <> 0 Then
                result = objfrmInteface.ErrorNumber
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: PrintDocument
    '
    ' Description: Calls the print document function in the interface
    '
    ' History: 13/06/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PrintDocument) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PrintDocument() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Silent or verbose?
    '
    'Select Case m_lMode
    'Case gSIRLibrary.ACPrintMode
    ' Call PrintDocument on the form
    'm_lReturn = CType(frmInterface.PrintDocument(), gPMConstants.PMEReturnCode)
    '
    'Case gSIRLibrary.ACPrintSilentMode
    ' Call PrintDocument on the form
    'm_lReturn = CType(frmInterface.PrintDocumentSilent(), gPMConstants.PMEReturnCode)
    '
    'Case Else
    '
    'End Select
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PrintDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PrintDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


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

    Public Sub CopyDLLFile()

        Dim sKeyString As String = "SOFTWARE\Microsoft\Office\"
        sKeyString = sKeyString & m_sWordVersion & "\Word\InstallRoot"

        Dim sPathValue As String = gPMFunctions.QueryKeyValue(gPMConstants.HKEY_LOCAL_MACHINE, sKeyString, "Path")
        If Not File.Exists(sPathValue & "Artinsoft.VB6.dll") Then
            File.Copy(Application.StartupPath & "\Artinsoft.VB6.dll", sPathValue & "Artinsoft.VB6.dll", True)
        End If
        If Not File.Exists(sPathValue & "bClientManager.dll") Then
            File.Copy(Application.StartupPath & "\bClientManager.dll", sPathValue & "bClientManager.dll", True)
        End If
        If Not File.Exists(sPathValue & "bPMPropertyManager.dll") Then
            File.Copy(Application.StartupPath & "\bPMPropertyManager.dll", sPathValue & "bPMPropertyManager.dll", True)
        End If
        If Not File.Exists(sPathValue & "bPMUserGroup.dll") Then
            File.Copy(Application.StartupPath & "\bPMUserGroup.dll", sPathValue & "bPMUserGroup.dll", True)
        End If
        If Not File.Exists(sPathValue & "bSIRFieldManager.dll") Then
            File.Copy(Application.StartupPath & "\bSIRFieldManager.dll", sPathValue & "bSIRFieldManager.dll", True)
        End If
        If Not File.Exists(sPathValue & "bSIRProductOptions.dll") Then
            File.Copy(Application.StartupPath & "\bSIRProductOptions.dll", sPathValue & "bSIRProductOptions.dll", True)
        End If
        If Not File.Exists(sPathValue & "dPMDAO.dll") Then
            File.Copy(Application.StartupPath & "\dPMDAO.dll", sPathValue & "dPMDAO.dll", True)
        End If
        If Not File.Exists(sPathValue & "iPMMessage.dll") Then
            File.Copy(Application.StartupPath & "\iPMMessage.dll", sPathValue & "iPMMessage.dll", True)
        End If
        If Not File.Exists(sPathValue & "SharedFiles.dll") Then
            File.Copy(Application.StartupPath & "\SharedFiles.dll", sPathValue & "SharedFiles.dll", True)
        End If
        If Not File.Exists(sPathValue & "SSP.S4I.Interfaces.dll") Then
            File.Copy(Application.StartupPath & "\SSP.S4I.Interfaces.dll", sPathValue & "SSP.S4I.Interfaces.dll", True)
        End If

    End Sub
End Class

