Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: Interface
    '
    ' Date: 11th July 1997
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' SP090299 - Fix to previous bug fix
    '
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Interface"


    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    'developer guide no. 50
    Dim frmInterface As frmInterface
    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' {* USER DEFINED CODE (Begin) *}
    Private m_iDocumentID As Integer
    Private m_dtAccountingDate As Date
    '
    '' CF280998 - Added following properties
    Private m_bReversingDocument As Boolean
    Private m_iReversingDocumentID As Integer
    Private m_dtReverseDate As Date
    Private m_bRecurringDocument As Boolean
    Private m_iOccurances As Integer
    Private m_vRecurringDocumentIDs As Object
    Private m_dtRecurringDocumentDates As Date
    Private m_sDocumentRef As String
    '
    '
    '' CF210699 - Added following keys
    'Private m_lClientAccountID As Long
    'Private m_lBankAccountID As Long

    ' {* USER DEFINED CODE (End) *}

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer

    Private m_lPMAuthorityLevel As Integer

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get

            ' Standard Property.

            ' Return the interface exit status.
            Return m_lStatus

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            ' Standard Property.

            ' Return the task.
            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            ' Standard Property.

            ' Return the navigate flag.
            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ' Standard Property.

            ' Return the process mode.
            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            ' Standard Property.

            ' Return the type of business.
            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            ' Standard Property.

            ' Return the effective date.
            Return m_dtEffectiveDate

        End Get
    End Property

    Public WriteOnly Property DocumentID() As Integer
        Set(ByVal Value As Integer)

            m_iDocumentID = Value

        End Set
    End Property

    Public WriteOnly Property AccountingDate() As Date
        Set(ByVal Value As Date)

            m_dtAccountingDate = Value

        End Set
    End Property

    Public Property ReversingDocument() As Boolean
        Get
            Return m_bReversingDocument

        End Get
        Set(ByVal Value As Boolean)

            m_bReversingDocument = Value

        End Set
    End Property

    Public Property ReversingDocumentID() As Integer
        Get
            Return m_iReversingDocumentID

        End Get
        Set(ByVal Value As Integer)

            m_iReversingDocumentID = Value

        End Set
    End Property
    Public Property ReverseDate() As Date
        Get
            Return m_dtReverseDate

        End Get
        Set(ByVal Value As Date)

            m_dtReverseDate = Value

        End Set
    End Property

    Public Property RecurringDocument() As Boolean
        Get
            Return m_bRecurringDocument

        End Get
        Set(ByVal Value As Boolean)

            m_bRecurringDocument = Value

        End Set
    End Property

    Public Property Occurances() As Integer
        Get
            Return m_iOccurances

        End Get
        Set(ByVal Value As Integer)

            m_iOccurances = Value

        End Set
    End Property
    Public Property RecurringDocumentIDs() As Object
        Get
            Return m_vRecurringDocumentIDs

        End Get
        Set(ByVal Value As Object)

            m_vRecurringDocumentIDs = Value

        End Set
    End Property

    Public Property RecurringDocumentDates() As Date
        Get
            Return m_dtRecurringDocumentDates

        End Get
        Set(ByVal Value As Date)

            m_dtRecurringDocumentDates = Value

        End Set
    End Property

    Public Property DocumentRef() As String
        Get

            Return m_sDocumentRef

        End Get
        Set(ByVal Value As String)

            m_sDocumentRef = Value

        End Set
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
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Dim sHelpFile As String
        Dim m_lReturn As gPMConstants.PMEReturnCode
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
                g_iCompanyID = .SourceID
                g_iUserID = .UserID
                g_sUserName = .UserName
                g_sPassword = .Password
                g_iCurrencyID = .CurrencyID
                g_iLogLevel = .LogLevel
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = g_sProductFamily
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            If sHelpFile <> "" Then
                'developer guide no. 39 (No Solution)
                'App.HelpFile = sHelpFile
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
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

                End If
                g_oObjectManager = Nothing
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
        Dim sTmp As String = ""


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

                '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
                ''            Case PMKeyNameName
                ''                m_sName$ = CStr(vKeyArray(PMKeyValue, lRow&))
                '            Case ACTKeyNameDocumentID
                '                m_lDocumentID& = CLng(vKeyArray(PMKeyValue, lRow&))
                '            'SP010299
                '            Case ACTKeyNameReversingDocumentID
                '                m_lReversingDocumentID& = CLng(vKeyArray(PMKeyValue, lRow&))
                '
                '            Case ACTKeyNameReversingDocumentDate
                '                If Trim(vKeyArray(PMKeyValue, lRow&)) <> "" Then
                '                    m_dtReverseDate = CLng(CDate(vKeyArray(PMKeyValue, lRow&)))
                '                End If
                '
                '            Case ACTKeyNameDocumentDate
                '                If Trim(vKeyArray(PMKeyValue, lRow&)) <> "" Then
                '                    m_dtAccountingDate = CLng(CDate(vKeyArray(PMKeyValue, lRow&)))
                '                End If
                '
                '            Case ACTKeyNameReversingDocument
                '                If Trim(vKeyArray(PMKeyValue, lRow&)) <> "" Then
                '                    m_bReversingDocument = CLng(CBool(vKeyArray(PMKeyValue, lRow&)))
                '                End If
                '
                '            Case ACTKeyNameRecurringDocumentDates
                '                sTmp = CStr(vKeyArray(PMKeyValue, lRow&))
                '                'Convert the string key back to an array
                '                m_lReturn& = ParseArray(vArray:=m_vRecurringDocumentDates, _
                ''                                        sString:=sTmp, _
                ''                                        bArrayToString:=False)
                '
                '                If (m_lReturn <> PMTrue) Then
                '                    LogMessage _
                ''                        iType:=PMLogOnError, _
                ''                        sMsg:="ParseArray Failed for Recurring Document Dates", _
                ''                        vApp:=ACApp, _
                ''                        vClass:=ACClass, _
                ''                        vMethod:="SetKeys", _
                ''                        vErrNo:=Err.Number, _
                ''                        vErrDesc:=Err.Description
                '                End If
                '
                '            Case ACTKeyNameRecurringDocumentIDs
                '                sTmp = CStr(vKeyArray(PMKeyValue, lRow&))
                '                'Convert the string key back to an array
                '                m_lReturn& = ParseArray(vArray:=m_vRecurringDocumentIDs, _
                ''                                        sString:=sTmp, _
                ''                                        bArrayToString:=False)
                '
                '                If (m_lReturn <> PMTrue) Then
                '                    LogMessage _
                ''                        iType:=PMLogOnError, _
                ''                        sMsg:="ParseArray Failed for Recurring Document IDs", _
                ''                        vApp:=ACApp, _
                ''                        vClass:=ACClass, _
                ''                        vMethod:="SetKeys", _
                ''                        vErrNo:=Err.Number, _
                ''                        vErrDesc:=Err.Description
                '                End If
                '
                '            Case ACTKeyNameOccurrences
                '                m_iOccurances = CLng(vKeyArray(PMKeyValue, lRow&))
                '
                '            Case ACTKeyNameRecurringDocument
                '                If Trim(vKeyArray(PMKeyValue, lRow&)) <> "" Then
                '                    m_bRecurringDocument = CLng(CBool(vKeyArray(PMKeyValue, lRow&)))
                '                End If
                '
                '            ' CF 210699
                '            Case ACTKeyNameAccountID
                '                m_lClientAccountID& = CLng(vKeyArray(PMKeyValue, lRow&))
                '
                '            Case "bank_account_id"
                '                m_lBankAccountID& = CLng(vKeyArray(PMKeyValue, lRow&))
                ''eck110500
                '            Case ACTKeyNameBranchID
                '                g_iCompanyID = CInt(vKeyArray(PMKeyValue, lRow&))
                '
                '        End Select

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
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vKeyArray(PMKeyName, 0) = PMKeyNameCnt
            '    vKeyArray(PMKeyValue, 0) = m_lNameCnt&

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
    Public Function GetSummary(ByRef vSummaryArray As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            Dim vKeyArray(1, 0) As Object

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$
            vSummaryArray = ""

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



            ' Error Section.

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
            m_lReturn = ProcessInterface()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


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

        'Modified,intialise the instance
        frmInterface = New frmInterface
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Call the List Form initialise method
        'Developer Guide no.9

        m_lReturn = frmInterface.Initialise()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmInterface = Nothing
            Return result
        End If

        '    'SP010299 - set these to empty if nothing in them
        '    'SP090299 - check if object before checking for nothing
        '    If (IsArray(m_vRecurringDocumentDates) = False) Then
        '        If (IsObject(m_vRecurringDocumentDates) = True) Then
        '            If (m_vRecurringDocumentDates Is Nothing) Then
        '                m_vRecurringDocumentDates = Empty
        '            End If
        '        End If
        '    End If
        '
        '    If (IsArray(m_vRecurringDocumentIDs) = False) Then
        '        If (IsObject(m_vRecurringDocumentIDs) = True) Then
        '            If (m_vRecurringDocumentIDs Is Nothing) Then
        '                m_vRecurringDocumentIDs = Empty
        '            End If
        '        End If
        '    End If

        ' Assign the parameters to the interface properties.

        With frmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Call the Load method to setup the List Form details
        'developer guide no. 68

        m_lReturn = frmInterface.Load_Renamed()
        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmInterface = Nothing
            Return result
        End If

        ' Call the ShowForm method to show the form, allow user input etc.
        m_lReturn = frmInterface.ShowForm(lDisplayState:=FormShowConstants.Modal)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            frmInterface = Nothing
            Return result
        End If

        ' Assign the property members from the interface parameters.
        With frmInterface
            m_lStatus = .Status
            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        frmInterface.Close()
        frmInterface = Nothing

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
        ' Error Section.
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

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class

