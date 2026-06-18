Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
Imports System.Text
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed
    Implements IDisposable
    Implements SSP.S4I.Interfaces.ILocalInterface
    Implements aPMNav.NavigatorV2
    ' ***************************************************************** '
    ' Class Name: Wrapper
    '
    ' Date: 16/09/1998
    '
    ' Description: Main public class of the Wrapper.
    '
    ' Edit History:
    ' ***************************************************************** '


    'This class implements Navigator Version 2

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "Initialise"


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

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' {* USER DEFINED CODE (Begin) *}
    Private m_sServerListFilePath As String = ""
    Private m_sServerListVersion As String = ""
    Private m_sServerListPrefVersion As String = ""
    Private m_bServerListFileCompressed As Boolean

    Private m_sClientListFilePath As String = ""
    Private m_sClientListFilePathIdx As String = ""
    Private m_sClientListFilePathDat As String = ""

    Private m_sServerListFilePathIdx As String = ""
    Private m_sServerListFilePathDat As String = ""

    Private m_sClientListVersion As String = ""
    Private m_sClientListPrefVersion As String = ""
    Private m_sAppPath As String = ""

    Private m_iRLDFFile As Integer
    Private m_iRLDFIndex As Integer
    Private m_bRLDFOpen As Boolean
    Private m_cIndex As Collection

    ' {* USER DEFINED CODE (End) *}

    ' Stores the exit status of the Wrapper.
    Private m_lStatus As Integer

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Interface object
    Private m_oInterface As Object

    Private m_oBusiness As bGEMListManager.Form
    Private m_oZipper As bPMZipper.Business

    ' Key variables
    Private m_vKeyArray As Object
    Private m_lPolicyFunction As Integer

    'BusinessType variable

    Private m_iBusinessType As Integer



    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' ***************************************************************** '
    ' The following property is set by Navigator                        '
    ' ***************************************************************** '

    ' The Calling application or component name.
    Public WriteOnly Property NavigatorV2_CallingAppName() As String Implements aPMNav.NavigatorV2.CallingAppName
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property
    'End Property

    ' ***************************************************************** '
    ' The following properties are set by Navigator via the             '
    ' SetProcessModes method and tell the component what                '
    ' mode of operation it is in.                                       '
    ' ***************************************************************** '

    ' The Task that the form is to perform
    ' i.e. Add, Edit, View, Delete
    Public ReadOnly Property NavigatorV2_Task() As Integer Implements aPMNav.NavigatorV2.Task
        Get
            Return m_iTask
        End Get
    End Property

    ' The status of the Navigator button on the form.
    ' i.e. Not Required, Enabled, Disabled
    Public ReadOnly Property NavigatorV2_Navigate() As Integer Implements aPMNav.NavigatorV2.Navigate
        Get
            Return m_lNavigate
        End Get
    End Property

    ' The type of process that is being performed
    ' i.e. Generic, Enquiry, Quotation, Make Live
    Public ReadOnly Property NavigatorV2_ProcessMode() As Integer Implements aPMNav.NavigatorV2.ProcessMode
        Get
            Return m_lProcessMode
        End Get
    End Property

    ' The type of transaction that is being performed
    ' i.e. Quotation, New Business, Renewal, MTA
    Public ReadOnly Property NavigatorV2_TransactionType() As String Implements aPMNav.NavigatorV2.TransactionType
        Get
            Return m_sTransactionType
        End Get
    End Property

    ' The effective date that we are working to
    Public ReadOnly Property NavigatorV2_EffectiveDate() As Date Implements aPMNav.NavigatorV2.EffectiveDate
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    ' ***************************************************************** '
    ' The following properties need to be set by the                    '
    ' component so that Navigator can tell what happened.               '
    ' ***************************************************************** '

    ' The user status of the form on exit
    ' i.e. PMOK, PMCancel or PMNavigate.
    ' Business objects will normally just set this to PMOK,
    ' unless they want the ability to adjust the route through a process.
    Public ReadOnly Property NavigatorV2_Status() As Integer Implements aPMNav.NavigatorV2.Status
        Get
            Return m_lStatus
        End Get
    End Property

    ' The Completion Status of the Step
    ' i.e.Complete , Incomplete, Inactive
    Public ReadOnly Property NavigatorV2_StepStatus() As String Implements aPMNav.NavigatorV2.StepStatus
        Get
            Return m_sStepStatus.Value
        End Get
    End Property


    Public WriteOnly Property BusinessType() As Integer
        Set(ByVal Value As Integer)

            m_iBusinessType = Value

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

            Dim sTitle, sMessage As String

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call the initialise method.
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the object manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                g_oObjectManager = Nothing
                Return result
            End If

            ' Store the language ID from the object manager
            ' to the public variables, to enable us to use
            ' them throughout the object.
            With g_oObjectManager
                g_iLanguageID = .LanguageID
                g_iSourceID = .SourceID
            End With

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bGEMListManager.Form", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Display error stating the problem.
                sTitle = "Unable to Create List Update Business"
                sMessage = "iGEMListManager"
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return result
            End If

            ' File compresser
            m_oZipper = New bPMZipper.Business()

            If m_oZipper Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            ' m_sTransactionType$ = PMTypeOfBusinessGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

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
    ' Name: Start (Standard Method)
    '
    ' Description: Entry point for the object to start its processing.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Starts the Wrapper processing.
            'm_lReturn& = WrapperProcess()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the Wrapper.
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
                If m_oBusiness IsNot Nothing Then
                    m_oBusiness.Dispose()
                    m_oBusiness = Nothing
                End If

                If Not (m_oZipper Is Nothing) Then
                    'm_lReturn = m_oListUser.Terminate
                    m_oZipper = Nothing
                End If

                '******************************************************************
                ' Close the RLDF
                '******************************************************************
                FileSystem.FileClose(m_iRLDFFile)
                FileSystem.FileClose(m_iRLDFIndex)
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' The following methods are called by Navigator BEFORE              '
    ' the component is told to Start its job.                           '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: SetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The array will contain the key values required by the
    '              component to do its job.
    '
    ' ***************************************************************** '
    ''developer guide no.  changes to be checked at run time
    Public Function NavigatorV2_SetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.SetKeys

        Dim result As Integer = 0
        Dim sKey, sCode As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save the settings to the registry
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)


                sKey = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                If sKey <> PMKeyNameComponentManager Then

                    sCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)).Trim()

                    If sKey.Trim() <> "" And sCode.Trim() <> "" Then
                        m_lReturn = CType(gPMFunctions.SaveRegSettings(sSetting:=sCode, sAppName:="Gemini", sSection:="Keys", sKey:=sKey), gPMConstants.PMEReturnCode)

                    End If
                End If


                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim() = PMKeyNamePolicyFunction Then


                    m_lPolicyFunction = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End If

            Next lRow

            ' Step through the key array.
            '    For lRow& = LBound(vKeyArray, 2) To UBound(vKeyArray, 2)
            '        ' Assign the parameter member with the
            '        ' correct key array item.
            '
            '        ' {* USER DEFINED CODE (Begin) *}
            '
            '        Select Case Trim$(CStr(vKeyArray(PMKeyName, lRow&)))
            '            Case VOYKeyNameCoverId
            '                m_lCoverID = CLng(vKeyArray(PMKeyValue, lRow&))?
            '
            '        End Select
            '
            '        ' {* USER DEFINED CODE (End) *}
            '    Next lRow&

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Navigator Standard Method)
    '
    ' Description: Sets the mode of operation for the Component.
    '              The properties are described individually above.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer Implements aPMNav.NavigatorV2.SetProcessModes

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Navigator Standard Method)
    '
    ' Description: Set the Process, Map and Step Completion Status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function NavigatorV2_SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer Implements aPMNav.NavigatorV2.SetStatus

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Navigator Standard Method)
    '
    ' Description: Tells the Component to Start its job.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_Start() As Integer Implements aPMNav.NavigatorV2.Start
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the Wrapper.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the object", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' The following methods are called by Navigator AFTER               '
    ' the component has done its job.                                   '
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetKeys (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format KeyName, KeyValue.
    '              The component populates the array with
    '              key values. i.e. If the component is
    '              FindParty it will return the PartyCnt of the Party
    '              selected by the user.
    '
    ' ***************************************************************** '
    Public Function NavigatorV2_GetKeys(ByRef vKeyArray(,) As Object) As Integer Implements aPMNav.NavigatorV2.GetKeys

        Dim result As Integer = 0

        Try


            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            '    ReDim vKeyArray(1, ?)

            ' Assign the key array with the parameter members.
            'BB171297
            '    vKeyArray(PMKeyName, 0) = VOYKeyNameCoverId?
            '    vKeyArray(PMKeyValue, 0) = m_lCoverID?
            '    vKeyArray(PMKeyName, 1) = VOYKeyNameTradeName?
            '    vKeyArray(PMKeyValue, 1) = m_sTradingName?
            '
            ' {* USER DEFINED CODE (End) *}

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary (Navigator Standard Method)
    '
    ' Description: Accepts an Array in the format Summary Level, Summary
    '              Heading, Summary Value.
    '
    '              The component populates the array with any
    '              summary information it wants to return to Navigator.
    '
    '              There are three levels of Summary, Process,
    '              Map Instance and Map.
    ' ***************************************************************** '
    ''developer guide no.  changes to be checked at run time
    Public Function NavigatorV2_GetSummary(ByRef vSummaryArray As Object) As Integer Implements aPMNav.NavigatorV2.GetSummary

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' {* USER DEFINED CODE (Begin) *}

            ' Initialise the summary array with the number of
            ' items needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vSummaryArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vSummaryArray(PMKeyName, 0) = PMKeyNameNavigatorTitle1
            '    vSummaryArray(PMKeyValue, 0) = m_sNavigatorTitle$
            '
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorV2_GetSummary", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' CL090699 BEGIN-->
    Public Function GetListAndCodes(ByVal v_sPropertyId As String, ByRef r_vListData() As Object, ByRef r_vListDataCode() As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        Return GetLists(v_sPropertyId, r_vListData, v_vSearchString, r_vListDataCode)

    End Function
    ' <-- END CL090699

    ' CL090699 BEGIN-->
    ' This wrapper avoids binary imcompatibility
    Public Function GetList(ByVal v_sPropertyId As String, ByRef r_vListData() As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        Return GetLists(v_sPropertyId, r_vListData, v_vSearchString)

    End Function
    ' <-- END CL090699

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: GetLists (Standard Method)
    '
    ' Description: Returns a list in a variant array for a given
    '              property id.
    '
    ' ***************************************************************** '
    Private Function GetLists(ByVal v_sPropertyId As String, ByRef r_vListData() As Object, Optional ByVal v_vSearchString As Object = Nothing, Optional ByRef r_vListDataCode() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordNumber As Integer
        Dim uDetailRecord As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()
        Dim i, lPos As Integer
        Dim sSearchString, sRecord As String
        Dim bEOF, bFirst, bCodeListWanted As Boolean ' CL090699
        Dim sABICode, sPropID, sDesc As String
        Dim sFullDesc As New StringBuilder
        Dim iPosPlusPlusPlus As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        bCodeListWanted = Not Information.IsNothing(r_vListDataCode)

        i = 0
        ReDim r_vListData(i)

        If bCodeListWanted Then ReDim r_vListDataCode(i) ' CL090699

        ' *****************************************************************
        ' Get the record number from the index
        ' *****************************************************************
        m_lReturn = CType(GetListIndex(v_sPropertyId:=v_sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            MessageBox.Show("Unable to load list for " & v_sPropertyId, "ListManager", MessageBoxButtons.OK)
            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        ' *****************************************************************
        ' Assemble the list
        ' *****************************************************************
        '    Get #m_iRLDFFile, lRecordNumber + 1, uDetailRecord
        '
        '    If IsMissing(v_vSearchString) = True Then
        '
        '        While v_sPropertyId = Trim(uDetailRecord.PropertyId) _
        ''        And EOF(m_iRLDFFile) <> True _
        ''        And i < GEMMaxListItems
        '
        '            ReDim Preserve r_vListData(i)
        '            r_vListData(i) = Trim(uDetailRecord.Description)
        '            i = i + 1
        '            Get #m_iRLDFFile, , uDetailRecord
        '
        '        Wend
        '
        '    Else
        '
        '        sSearchString = CStr(v_vSearchString)
        '
        '        While v_sPropertyId = Trim(uDetailRecord.PropertyId) _
        ''        And EOF(m_iRLDFFile) <> True _
        ''        And i < GEMMaxListItems
        '
        '            sRecord = Trim(uDetailRecord.Description)
        '            lPos = InStr(1, sRecord, sSearchString, vbTextCompare)
        '
        '            If lPos > 0 Then
        '
        '                ReDim Preserve r_vListData(i)
        '                r_vListData(i) = sRecord
        '                i = i + 1
        '            End If
        '
        '            Get #m_iRLDFFile, , uDetailRecord
        '
        '        Wend
        '
        '    End If
        'sj 18/05/99 - start
        ' Pick up last record of last list

        bEOF = False
        bFirst = True


        If Information.IsNothing(v_vSearchString) Then

            While Not bEOF And i < GEMMaxListItems

                If bFirst Then

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                    bFirst = False
                Else

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                End If
                'developer guide no. 130(Guide)
                sPropID = uDetailRecord.PropertyId.Trim()
                sDesc = uDetailRecord.Description.Trim()
                sABICode = uDetailRecord.ABICode.Trim()

                ' CL160699 BEGIN-->

                ' Look for the +++ char sequence. This means concatenate with
                ' the following record (and so on...). This gives us
                ' access to very long list item descriptions.
                sFullDesc = New StringBuilder("")

                iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                Do While (iPosPlusPlusPlus <> 0)
                    sFullDesc.Append(sDesc.Substring(0, iPosPlusPlusPlus - 1))


                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                    ' keep on truckin' mama
                    'developer guide no. 130(Guide)
                    sDesc = uDetailRecord.Description.Trim
                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                Loop


                sFullDesc.Append(sDesc)

                ' <-- END CL160699

                If v_sPropertyId <> sPropID Then
                    bEOF = True
                Else
                    ReDim Preserve r_vListData(i)

                    r_vListData(i) = sFullDesc.ToString()

                    ' CL090699 BEGIN-->
                    If bCodeListWanted Then
                        ReDim Preserve r_vListDataCode(i)

                        r_vListDataCode(i) = sABICode
                    End If
                    ' <-- END CL090699

                    i += 1
                    bEOF = FileSystem.EOF(m_iRLDFFile)

                End If


            End While

        Else


            sSearchString = CStr(v_vSearchString)

            While Not bEOF And i < GEMMaxListItems

                If bFirst Then

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                    bFirst = False
                Else

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                End If
                'developer guide no. 130(Guide)
                sPropID = uDetailRecord.PropertyId.Trim()
                sDesc = uDetailRecord.Description.Trim()
                sABICode = uDetailRecord.ABICode.Trim()

                ' CL160699 BEGIN-->

                ' Look for the +++ char sequence. This means concatenate with
                ' the following record (and so on...). This gives us
                ' access to very long list item descriptions.
                sFullDesc = New StringBuilder("")

                iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                Do While (iPosPlusPlusPlus <> 0)
                    sFullDesc.Append(sDesc.Substring(0, iPosPlusPlusPlus - 1))


                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                    ' keep on truckin' mama
                    'developer guide no. 130(Guide)
                    sDesc = uDetailRecord.Description.Trim()
                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                Loop


                sFullDesc.Append(sDesc)

                ' <-- END CL160699


                If v_sPropertyId <> sPropID Then
                    bEOF = True
                Else

                    sRecord = sFullDesc.ToString()
                    lPos = (sRecord.IndexOf(sSearchString, StringComparison.CurrentCultureIgnoreCase) + 1)

                    If lPos > 0 Then
                        ReDim Preserve r_vListData(i)

                        r_vListData(i) = sFullDesc.ToString()

                        ' CL090699 BEGIN-->
                        If bCodeListWanted Then
                            ReDim Preserve r_vListDataCode(i)

                            r_vListDataCode(i) = sABICode
                        End If
                        ' <-- END CL090699

                        i += 1
                    End If

                    bEOF = FileSystem.EOF(m_iRLDFFile)
                End If


            End While
        End If

        'sj 18/05/99 - end
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: PopulateListControl (Standard Method)
    '
    ' Description: Returns a list in a variant array for a given
    '              property id.
    '
    ' ***************************************************************** '
    Public Function PopulateListControl(ByVal v_sPropertyId As String, ByRef r_oControl As Object, Optional ByVal v_vSearchString As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordNumber As Integer
        Dim uDetailRecord As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()
        Dim lPos As Integer
        Dim sSearchString, sRecord As String
        Dim i As Integer
        Dim bEOF, bFirst As Boolean
        Dim sABICode, sPropID, sDesc, sFullDesc As String
        Dim iPosPlusPlusPlus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            i = 0


            'todolist_alkesh
            'r_oControl.Clear()
            'condition added to check the type of control
            If (r_oControl.GetType.FullName = "System.Windows.Forms.ComboBox") Then

                r_oControl.items.clear()

            End If



            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=v_sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Unable to load list for " & v_sPropertyId, "ListManager", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' *****************************************************************
            ' Assemble the list
            ' *****************************************************************
            '    Get #m_iRLDFFile, lRecordNumber + 1, uDetailRecord

            '    If IsMissing(v_vSearchString) = True Then
            '
            '        While v_sPropertyId = Trim(uDetailRecord.PropertyId) _
            ''        And EOF(m_iRLDFFile) <> True _
            ''        And i < GEMMaxListItems
            '
            '            r_oControl.AddItem Trim(uDetailRecord.Description)
            '            Get #m_iRLDFFile, , uDetailRecord
            '            i = i + 1
            '
            '        Wend
            '
            '    Else
            '
            '        sSearchString = CStr(v_vSearchString)
            '
            '        While v_sPropertyId = Trim(uDetailRecord.PropertyId) _
            ''        And EOF(m_iRLDFFile) <> True _
            ''        And i < GEMMaxListItems
            '
            '            sRecord = Trim(uDetailRecord.Description)
            '            lPos = InStr(1, sRecord, sSearchString, vbTextCompare)
            '
            '            If lPos > 0 Then
            '
            '                r_oControl.AddItem sRecord
            '            End If
            '
            '            Get #m_iRLDFFile, , uDetailRecord
            '            i = i + 1
            '
            '        Wend
            '
            '    End If
            'sj 18/05/99 - start
            ' Pick up last record of last list

            bEOF = False
            bFirst = True


            If Information.IsNothing(v_vSearchString) Then

                While Not bEOF And i < GEMMaxListItems

                    If bFirst Then

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                        bFirst = False
                    Else

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                    End If

                    'developer guide no. 130(Guide)
                    sPropID = uDetailRecord.PropertyId.Trim()
                    sDesc = uDetailRecord.Description.Trim()
                    sABICode = uDetailRecord.ABICode.Trim()

                    ' CL160699 BEGIN-->

                    ' Look for the +++ char sequence. This means concatenate with
                    ' the following record (and so on...). This gives us
                    ' access to very long list item descriptions.
                    sFullDesc = ""

                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                    Do While (iPosPlusPlusPlus <> 0)
                        sFullDesc = sFullDesc & sDesc.Substring(0, iPosPlusPlusPlus - 1)


                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                        ' keep on truckin' mama

                        'developer guide no. 130(Guide)
                        sDesc = uDetailRecord.Description.Trim()
                        iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                    Loop


                    sFullDesc = sFullDesc & sDesc

                    ' <-- END CL160699

                    If v_sPropertyId <> sPropID Then
                        bEOF = True
                    Else

                        'condition added to check the type of control
                        If (r_oControl.GetType.FullName = "System.Windows.Forms.ComboBox") Then
                            r_oControl.items.add(sFullDesc)
                        Else
                            r_oControl.AddItem(sFullDesc)
                        End If
                        i += 1
                        bEOF = FileSystem.EOF(m_iRLDFFile)
                    End If


                End While

            Else


                sSearchString = CStr(v_vSearchString)

                While Not bEOF And i < GEMMaxListItems

                    If bFirst Then

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                        bFirst = False
                    Else

                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                    End If
                    'developer guide no. 130(Guide)
                    sPropID = uDetailRecord.PropertyId.Trim()
                    sDesc = uDetailRecord.Description.Trim()
                    sABICode = uDetailRecord.ABICode.Trim()

                    ' CL160699 BEGIN-->

                    ' Look for the +++ char sequence. This means concatenate with
                    ' the following record (and so on...). This gives us
                    ' access to very long list item descriptions.
                    sFullDesc = ""

                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                    Do While (iPosPlusPlusPlus <> 0)
                        sFullDesc = sFullDesc & sDesc.Substring(0, iPosPlusPlusPlus - 1)


                        FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                        ' keep on truckin' mama
                        'developer guide no. 130(Guide)
                        sDesc = uDetailRecord.Description.Trim()
                        iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                    Loop


                    sFullDesc = sFullDesc & sDesc

                    ' <-- END CL160699

                    If v_sPropertyId <> sPropID Then
                        bEOF = True
                    Else

                        sRecord = sFullDesc
                        lPos = (sRecord.IndexOf(sSearchString, StringComparison.CurrentCultureIgnoreCase) + 1)

                        If lPos > 0 Then

                            'condition added to check the type of control
                            If (r_oControl.GetType.FullName = "System.Windows.Forms.ComboBox") Then
                                r_oControl.items.add(sRecord)
                            Else
                                r_oControl.AddItem(sRecord)
                            End If
                            i += 1
                        End If

                        bEOF = FileSystem.EOF(m_iRLDFFile)
                    End If


                End While
            End If

            'sj 18/05/99 - end


            'condition added to check the type of control
            If (r_oControl.GetType.FullName = "System.Windows.Forms.ComboBox") Then
                If r_oControl.items.Count > 0 Then

                    r_oControl.selectedindex = 0
                End If
            Else
                If r_oControl.ListCount > 0 Then

                    r_oControl.ListIndex = 0
                End If
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Populate List Control", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateListControl", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckListVersions (Standard Method)
    '
    ' Description: Check the client list versions against the server
    '              values. If they are different copy the RLDF
    '              from the server and update the versions in the client
    '              registry
    ' ***************************************************************** '
    Public Function CheckListVersions() As Integer

        Dim result As Integer = 0
        Dim sExistingClientRLDF As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Pass across the business type first
            'MN300699

            m_oBusiness.BusinessType = m_iBusinessType

            ' *****************************************************************
            ' Get the registry settings
            ' *****************************************************************
            m_lReturn = CType(GetServerSettings(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting server registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_sClientListFilePathDat = m_sServerListFilePath & ".dat"

            ' *****************************************************************
            ' Open the RLDF files
            ' *****************************************************************

            'RDT130599
            ' Changed the four lines below to use the Freefile function
            ' and prevent file locking

            m_iRLDFFile = FileSystem.FreeFile()
            'developer guide no. 132(Guide)
            FileSystem.FileOpen(m_iRLDFFile, m_sServerListFilePathDat, OpenMode.Random, OpenAccess.Read, OpenShare.Shared)
            m_iRLDFIndex = FileSystem.FreeFile()
            'developer guide no. 132(Guide)
            FileSystem.FileOpen(m_iRLDFIndex, m_sServerListFilePathIdx, OpenMode.Random, OpenAccess.Read, OpenShare.Shared)

            ' *****************************************************************
            ' Open the RLDF files
            ' *****************************************************************
            m_lReturn = CType(BuildIndex(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error building index", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersions", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            m_bRLDFOpen = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckListVersions failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckListVersions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetServerSettings (Standard Method)
    '
    ' Description: Uses the ListManager business object to get registry
    '              values from the server.
    '
    ' ***************************************************************** '
    Private Function GetServerSettings() As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oBusiness.SetServerSettings(r_sServerListFilePath:=m_sServerListFilePath, r_sServerListVersion:=m_sServerListVersion, r_sServerListPrefVersion:=m_sServerListPrefVersion, r_bServerListFileCompressed:=m_bServerListFileCompressed, r_sAppPath:=m_sAppPath)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sServerListFilePathIdx = m_sServerListFilePath & ".idx"
        m_sServerListFilePathDat = m_sServerListFilePath & ".dat"

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: BuildIndex (Standard Method)
    '
    ' Description: Build the index for the RLDF data
    '
    ' ***************************************************************** '
    Private Function BuildIndex() As Integer

        Dim result As Integer = 0
        Dim uIndexRecord As MainModule.RLDFIndexRecord = MainModule.RLDFIndexRecord.CreateInstance()
        Dim bEOF As Boolean

        m_cIndex = New Collection()



        result = gPMConstants.PMEReturnCode.PMTrue

        '    Get #m_iRLDFIndex, , uIndexRecord
        '    While EOF(m_iRLDFIndex) <> True
        '
        '        m_cIndex.Add _
        ''            uIndexRecord.RecordNumber, _
        ''            Key:=Trim(uIndexRecord.PropertyId)
        '        Get #m_iRLDFIndex, , uIndexRecord
        '    Wend

        'sj 18/05/99 - start
        ' For random files EOF seems to be set on the last record NOT!! after it
        bEOF = False

        While Not bEOF


            FileSystem.FileGet(m_iRLDFIndex, uIndexRecord, -1)
            'developer guide no. 130(Guide)
            m_cIndex.Add(uIndexRecord.RecordNumber, uIndexRecord.PropertyId.Trim())

            bEOF = FileSystem.EOF(m_iRLDFIndex)

        End While
        'sj 18/05/88 - end

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetListIndex (Standard Method)
    '
    ' Description: Gets the record number from the collection
    '
    ' ***************************************************************** '
    Private Function GetListIndex(ByVal v_sPropertyId As String, ByRef r_lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            r_lRecordNumber = CInt(m_cIndex(v_sPropertyId))

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the Wrapper entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub



    ' ***************************************************************** '
    ' Name: GetDescription (Standard Method)
    '
    ' Description: Returns a desc for a given property idand ABI code.
    '
    ' CL090699
    '
    ' ***************************************************************** '
    Public Function GetDescription(ByVal sPropertyId As String, ByVal sABICodeTarget As String, ByRef sDescription As String) As Integer

        Dim result As Integer = 0
        Dim lRecordNumber As Integer
        Dim uDetailRecord As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()
        Dim bEOF, bFirst As Boolean
        Dim sABICode, sPropID, sDesc As String
        Dim sFullDesc As New StringBuilder
        Dim iPosPlusPlusPlus As Integer

        Try

            sDescription = ""

            ' *****************************************************************
            ' Get the record number from the index
            ' *****************************************************************
            m_lReturn = CType(GetListIndex(v_sPropertyId:=sPropertyId, r_lRecordNumber:=lRecordNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Unable to load list for " & sPropertyId, "ListManager", MessageBoxButtons.OK)
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            bEOF = False
            bFirst = True


            While Not bEOF

                If bFirst Then

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, lRecordNumber + 1)
                    bFirst = False
                Else

                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)
                End If
                'developer guide no. 130(Guide)

                sPropID = uDetailRecord.PropertyId.Trim()
                sDesc = uDetailRecord.Description.Trim()
                sABICode = uDetailRecord.ABICode.Trim()

                ' CL160699 BEGIN-->

                ' Look for the +++ char sequence. This means concatenate with
                ' the following record (and so on...). This gives us
                ' access to very long list item descriptions.
                sFullDesc = New StringBuilder("")

                iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)

                Do While (iPosPlusPlusPlus <> 0)
                    sFullDesc.Append(sDesc.Substring(0, iPosPlusPlusPlus - 1))


                    FileSystem.FileGet(m_iRLDFFile, uDetailRecord, -1)

                    ' keep on truckin' mama
                    'developer guide no. 130(Guide)
                    sDesc = uDetailRecord.Description.Trim()
                    iPosPlusPlusPlus = (sDesc.IndexOf("+++", StringComparison.CurrentCultureIgnoreCase) + 1)
                Loop


                sFullDesc.Append(sDesc)

                ' <-- END CL160699


                If sPropertyId <> sPropID Then
                    bEOF = True
                Else
                    If sABICodeTarget = sABICode Then
                        sDescription = sFullDesc.ToString()
                        ' found it... exit!
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If

                End If

            End While

            ' If we've reached here then we haven't found a description


            Return gPMConstants.PMEReturnCode.PMError

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDescription failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDescription", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

