Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Text
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
    ' Date: 08/10/1998
    '
    ' Description: Main public class to accompany the interface form.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify

    Private Const ACClass As String = "EMail"

    ' PRIVATE Data Members (Begin)

    ' Object parameter members.
    Private m_sCallingAppName As String = ""

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_sNavigatorTitle As String = ""

    'User Detfined Parameters

    Private m_lPartyCnt As Integer
    Private m_sEmailAddress As String = ""
    Private m_sEmailSubject As String = ""
    Private m_sEmailText As String = ""
    Private m_sEmailAttachment As String = ""
    Private m_sClientCode As String = ""
    Private m_sPolicyNumber As String = ""
    Private m_sClaimNumber As String = ""

    Private m_lClaimCnt As Integer
    Private m_lInsFileCnt As Integer
    Private m_lInsFolderCnt As Integer

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Stores the exit status of the Wrapper.
    Private m_lStatus As Integer


    Private m_oBusiness As bSIREmail.Business

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As gPMConstants.PMEReturnCode


    Dim m_ofrmInterface As frmInterface

    ' PUBLIC Property Procedures (Begin)
    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the calling application name.
            m_lPartyCnt = Value

        End Set
    End Property
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Standard Property.

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property
    ' PUBLIC Methods(Begin)
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer Implements SSP.S4I.Interfaces.ILocalInterface.Initialise
        Dim result As Integer = 0
        Dim sHelpFile As String = ""
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
            End With

            ' Initialise the process modes.
            m_iTask = gPMConstants.PMEComponentAction.PMView
            m_lNavigate = gPMConstants.PMENavigateButtonStatus.PMNavigateNotRequired
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegsettinglevel:=eRegSettingLevel, v_sSettingName:="HelpFile", r_sSettingValue:=sHelpFile), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to retrive Helpfile", Application.ProductName)
                Return result
            End If
            If sHelpFile <> "" Then
                'TODO :
                'App.HelpFile = sHelpFile
            End If
            ' Get an instance of the business object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIREmail.Business", vInstanceManager:="ClientManager")
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to instance the business object
                result = gPMConstants.PMEReturnCode.PMFalse
                m_oBusiness = Nothing
                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of bSIREmail", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return result
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
                    Case PMNavKeyConst.PMKeyNamePartyCnt

                        m_lPartyCnt = CInt(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClientCode

                        m_sClientCode = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsFileCnt

                        m_lInsFileCnt = CInt(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNamePolicyNumber

                        m_sPolicyNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.PMKeyNameInsFolderCnt

                        m_lInsFolderCnt = CInt(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClaimCnt

                        m_lClaimCnt = CInt(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow)))
                    Case PMNavKeyConst.PMKeyNameClaimNumber

                        m_sClaimNumber = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case gSIRLibrary.SIRKeyEmailAddress

                        m_sEmailAddress = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case gSIRLibrary.SIRKeyEmailSubject

                        m_sEmailSubject = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case gSIRLibrary.SIRKeyEmailText

                        m_sEmailText = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case gSIRLibrary.SIRKeyEmailAttachment

                        m_sEmailAttachment = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
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

            ' {* USER DEFINED CODE (Begin) *
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = gSIRLibrary.SIRKeyEmailAddress

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_sEmailAddress
            ' {* USER DEFINED CODE (End) *}

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods(End)
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
        Dim oOutlook As Outlook
        Dim vContacts, vUserProp, vAttachments As Object
        Dim sDatabaseName, sBranchCode As String
        Dim asEmailAddress(,) As String
        Dim lEmailAddressCount As Integer
        Dim fSelectAddress As frmSelectAddress
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sEmailSubject As String = ""
        Dim sEmailTo As New StringBuilder



        result = gPMConstants.PMEReturnCode.PMTrue

        oOutlook = New Outlook()

        'Developer Guide No 9
        m_lReturn = oOutlook.Initialise()

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


            m_lReturn = m_oBusiness.GetAddressContactDetails(vPartyCnt:=m_lPartyCnt, vContacts:=vContacts)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And Information.IsArray(vContacts) Then

                For lLoop As Integer = vContacts.GetLowerBound(1) To vContacts.GetUpperBound(1)

                    If CStr(vContacts(5, lLoop)).Trim() = gSIRLibrary.SIRMainAddressABICode And CStr(vContacts(4, lLoop)).Trim() = gSIRLibrary.SIREmailContactCode Then

                        If lEmailAddressCount = 0 Then
                            asEmailAddress = Array.CreateInstance(GetType(String), New Integer() {ARRAY_EMAIL_UPPER - ARRAY_EMAIL_LOWER + 1, lEmailAddressCount + 1}, New Integer() {ARRAY_EMAIL_LOWER, 0})
                        Else
                            asEmailAddress = ArraysHelper.RedimPreserve(Of String(,))(asEmailAddress, New Integer() {ARRAY_EMAIL_UPPER - ARRAY_EMAIL_LOWER + 1, lEmailAddressCount + 1}, New Integer() {ARRAY_EMAIL_LOWER, 0})
                        End If


                        asEmailAddress(ARRAY_EMAIL_EMAILADDRESS, lEmailAddressCount) = CStr(vContacts(2, lLoop)).Trim()

                        asEmailAddress(ARRAY_EMAIL_DESCRIPTION, lEmailAddressCount) = CStr(vContacts(6, lLoop)).Trim()

                        lEmailAddressCount += 1

                    End If
                Next lLoop
            End If

            m_sEmailAddress = ""

            Select Case lEmailAddressCount
                Case 0
                    'Do nothing
                Case 1
                    sEmailTo = New StringBuilder(asEmailAddress(ARRAY_EMAIL_EMAILADDRESS, 0))
                Case Else

                    fSelectAddress = New frmSelectAddress()
                    fSelectAddress.EmailAddress = VB6.CopyArray(asEmailAddress)

                    fSelectAddress.ShowDialog()
                    lReturn = fSelectAddress.State

                    If lReturn = gPMConstants.PMEReturnCode.PMOk Then
                        asEmailAddress = VB6.CopyArray(fSelectAddress.EmailAddress)
                        If IsInitialisedArray(asEmailAddress) Then
                            For lLoop As Integer = asEmailAddress.GetLowerBound(1) To asEmailAddress.GetUpperBound(1)
                                sEmailTo.Append(asEmailAddress(ARRAY_EMAIL_EMAILADDRESS, lLoop) & "; ")
                            Next lLoop
                            If sEmailTo.ToString().Length >= 2 Then
                                sEmailTo = New StringBuilder(sEmailTo.ToString().Substring(0, sEmailTo.ToString().Length - 2))
                            End If
                        End If
                    Else
                        fSelectAddress.Close()
                        Return result
                    End If

                    fSelectAddress.Close()

            End Select


            m_lReturn = m_oBusiness.GetBranchCode(v_iSourceID:=g_iSourceID, r_sBranchCode:=sBranchCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessInterface", "Failed to get the branch code from the business object", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = m_oBusiness.GetDatabaseName(r_sDatabaseName:=sDatabaseName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("ProcessInterface", "Failed to get the database name from the business object", gPMConstants.PMELogLevel.PMLogError)
            End If

            ReDim vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3)

            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = USER_PROPERTY_DATABASE

            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = sDatabaseName


            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = USER_PROPERTY_BRANCH_CODE

            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = sBranchCode


            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = USER_PROPERTY_CLIENT_KEY

            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_lPartyCnt


            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = USER_PROPERTY_ORIGIN

            vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = "true"

            If m_lClaimCnt > 0 Then
                ReDim Preserve vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4)

                vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = USER_PROPERTY_CLAIM_KEY

                vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lClaimCnt

                If m_sClaimNumber.Trim() <> "" Then
                    sEmailSubject = "Regarding claim " & m_sClaimNumber
                End If

            ElseIf m_lInsFileCnt > 0 Then
                ReDim Preserve vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5)

                vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = USER_PROPERTY_INSURANCEFILE_KEY

                vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = m_lInsFileCnt


                vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = USER_PROPERTY_INSURANCEFOLDER_KEY

                vUserProp(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = m_lInsFolderCnt

                If m_sPolicyNumber.Trim() <> "" Then
                    sEmailSubject = "Regarding policy " & m_sPolicyNumber
                End If

            End If

            If m_sEmailAttachment <> "" Then
                ReDim vAttachments(0)

                vAttachments(0) = m_sEmailAttachment
            End If



            m_lReturn = CType(oOutlook.NewEmail(sEmailTo.ToString(), sEmailSubject, , vAttachments, vUserProp, "", False), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oOutlook.Dispose()
            oOutlook = Nothing

        Else

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

        'Developer Guide No 50
        m_ofrmInterface = New frmInterface
        ' Assign the parameters to the interface properties.
        ' developer guide no. 69
        With m_ofrmInterface
            .CallingAppName = m_sCallingAppName
            .Task = m_iTask
            .Navigate = m_lNavigate
            .ProcessMode = m_lProcessMode
            .TransactionType = m_sTransactionType
            .EffectiveDate = m_dtEffectiveDate

            ' {* USER DEFINED CODE (Begin) *}


            'developer guide no. 24
            .Business = m_oBusiness
            .PartyCnt = m_lPartyCnt
            .EmailAddress = m_sEmailAddress
            .EmailSubject = m_sEmailSubject
            .EmailText = m_sEmailText
            .EmailAttachment = m_sEmailAttachment

            ' {* USER DEFINED CODE (End) *}
        End With

        ' Load the instance of the interface into memory.
        'developer guide no. 69
        Dim tempLoadForm As frmInterface = m_ofrmInterface

        ' Check if we have had an error so far.
        'developer guide no. 69
        If m_ofrmInterface.ErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST return the error.
            'developer guide no. 69
            result = m_ofrmInterface.ErrorNumber
        End If

        ' Set the status in the interface.
        'developer guide no. 69
        m_lReturn = CType(m_ofrmInterface.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value), gPMConstants.PMEReturnCode)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to set the status.
            result = gPMConstants.PMEReturnCode.PMFalse
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
        'developer guide no. 69
        With m_ofrmInterface
            m_lStatus = .Status
            m_sStepStatus.Value = .StepStatus

            ' {* USER DEFINED CODE (Begin) *}
            m_sEmailAddress = .EmailAddress
            ' {* USER DEFINED CODE (End) *}
        End With

        ' Unload and destroy the instance of the interface
        ' from memory.
        ' developer guide no. 69
        m_ofrmInterface.Close()
        'developer guide no. 69
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
        'developer guide no. 69
        VB6.ShowForm(m_ofrmInterface, lDisplayState)

        If lDisplayState = FormShowConstants.Modal Then
            ' Check for any form errors.
            'developer guide no. 69
            If m_ofrmInterface.ErrorNumber <> 0 Then
                'developer guide no. 69
                result = m_ofrmInterface.ErrorNumber
            End If
        End If

        Return result

    End Function
    'PRIVATE Methods (End)
    Private Function IsInitialisedArray(ByRef oArray(,) As Object) As Boolean

        Dim lLower As Integer

        If Information.IsArray(oArray) Then
            On Error Resume Next
            lLower = oArray.GetLowerBound(0)
            Return Information.Err().Number = 0
        Else
            Return False
        End If

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
