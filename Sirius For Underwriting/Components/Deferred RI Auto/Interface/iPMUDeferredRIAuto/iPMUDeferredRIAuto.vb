Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmDeferredRIAuto
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmDeferredRIAuto
    '
    ' Date: 26/09/00
    '
    ' Description: Interface for Renewals processing.
    '
    ' Edit History: AMB 18-Sep-03: 1.8.6 Deferred Reinsurance development
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmDeferredRIAuto"

    ' Object parameter members.
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As Integer

    Private Enum DeferredRIField
        edInsuranceFileCnt = 0
        'edInsuranceFolderCnt = 2
        edInsuranceRef = 1
        edClientCode = 8
        edClientName = 9
        edInsStatus = 12
        edInsType = 13
    End Enum

    Private m_vDefRIRiskData(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As Integer

    ' AMB 16-Sep-03: 1.8.6 Deferred Reinsurance development
    Private m_lProgressValue As Integer
    Private m_sStatusBarText As String = ""

    ' AMB 18-Sep-03: 1.8.6 Deferred Reinsurance development

    Private m_oBusiness As bSIRDeferredRIAuto.Business
    'Private oRenewal As New bSIRRenSelection.Business

    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Return m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()

    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdStart.Click
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: cmdStart_Click
        ' AUTHOR: Andrew Bibby
        ' DATE: 16-Sep-03, 14:50
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        ' process those policies
        m_lReturn = ProcessDefRIPolicies()


        ' if an error's occurred, it should have been handled already

        ' set up the interface again
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        cmdStart.Enabled = False
        cmdCancel.Text = "&Close"
        cmdCancel.Enabled = True

        Exit Sub



        m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occurred whilst processing the policy.", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdStart", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    End Sub

    ' ***************************************************************** '
    '
    ' Name: GetAccumulations
    '
    ' 13/08/2014
    '
    ' ***************************************************************** '
    Private Function GetAccumulations(ByVal v_lNewInsuranceFileCnt As Long) As Integer

        Dim oAccumulations As Object
        Dim aKeys(2, 1) As Object
        Dim nResult As Integer
        Dim oAccumulationValues As iPMUAccumulationValues.Interface_Renamed

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


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise iPMUAccumulationValues.Interface_Renamed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientCode", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse


            End If
            m_lReturn = g_oObjectManager.GetInstance(oAccumulations, sClassName:="iPMUAccumulationValues.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oAccumulationValues = oAccumulations

            m_lReturn = oAccumulationValues.Initialise()
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                MsgBox("error init")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            nResult = oAccumulationValues.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
            aKeys(0, 0) = "insurance_file_cnt"
            aKeys(1, 0) = v_lNewInsuranceFileCnt
            nResult = oAccumulationValues.SetKeys(vKeyArray:=aKeys)
            nResult = oAccumulationValues.Start()

            If (nResult <> gPMConstants.PMEReturnCode.PMTrue) Then
                MsgBox("error start")
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            '    m_lStatus = oObject.Status




            Return nResult


        Catch excep As System.Exception
            ' Log Error Message
            nResult = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccumulations Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccumulations", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        Finally
            oAccumulationValues = Nothing


        End Try
        Return nResult
    End Function

    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.
        Dim sMessage, sTitle As String

        Try

            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Create business  object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRDeferredRIAuto.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmDeferredRIAuto_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            iPMFunc.ShowFormInTaskBar_Detach()

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If


            m_lReturn = ShowDeferredRIPolicies()

        Catch excep As System.Exception




            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ShowDeferredRIPolicies
    '
    ' Description:
    ' ***************************************************************** '
    Private Function ShowDeferredRIPolicies() As Integer

        Dim result As Integer = 0
        Try

            'Display a searching message.
            DisplayStatusSearching()

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred whilst obtaining the deferred reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDeferredRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result

            End If

            EnableDisableButtons()

            'Display a searching message.
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get details.
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDeferredRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: ProcessDefRIPolicies
    '
    ' Description:
    '
    ' AMB 09-Sep-03: 1.8.6 Deferred Reinsurance development - amended from frmRenewal version
    ' 03/03/2004 - move main works to business object and the way it process
    ' ***************************************************************** '
    Public Function ProcessDefRIPolicies() As Integer

        Dim result As Integer = 0
        Dim lCurrInsFileCnt As Integer
        Dim nInsFile As Integer
        Dim sInsRef As String
        Dim nInsStatus As Integer
        Dim nNewInsurancefileCnt As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            sInsRef = ""

            'disable the buttons
            cmdStart.Enabled = False
            cmdCancel.Enabled = False

            For lLoopy As Integer = 0 To (m_lItemsFound - 1)

                'PM033638 only go for policy version not for risk types. Risks are handled furthur so avoid looping here

                'm_lReturn = DeletePolicyFromRenewal(v_lInsuranceFolderCnt:=m_vDefRIRiskData(DeferredRIField.edInsuranceFolderCnt, lLoopy))
                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete in-renewal version of policy", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDefRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                '    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                '    ProcessDefRIPolicies = gPMConstants.PMEReturnCode.PMFalse
                '    Exit Function
                'End If

                If nInsFile <> Trim$(gPMFunctions.NullToInteger(m_vDefRIRiskData(DeferredRIField.edInsuranceFileCnt, lLoopy))) Then

                    ' update the interface
                    txtPolicyNumber.Text = gPMFunctions.NullToString(m_vDefRIRiskData(DeferredRIField.edInsuranceRef, lLoopy)).Trim()
                    Application.DoEvents()
                    txtClientCode.Text = gPMFunctions.NullToString(m_vDefRIRiskData(DeferredRIField.edClientCode, lLoopy)).Trim()
                    Application.DoEvents()
                    txtClientName.Text = gPMFunctions.NullToString(m_vDefRIRiskData(DeferredRIField.edClientName, lLoopy)).Trim()
                    Application.DoEvents()
                    lCurrInsFileCnt = gPMFunctions.NullToLong(m_vDefRIRiskData(DeferredRIField.edInsuranceFileCnt, lLoopy))

                    Dim sInsuranceFileType As String = m_vDefRIRiskData(DeferredRIField.edInsType, lLoopy)
                    nInsStatus = m_vDefRIRiskData(DeferredRIField.edInsStatus, lLoopy)

                    'Developer Guide No 168
                    _sbrStatus_Panel1.Text = "processing Policy..."
                    sbrStatus.Refresh()


                    m_lReturn = m_oBusiness.ProcessSingleDefRIPolicy(nInsuranceFileCnt:=lCurrInsFileCnt, nInsStatus:=nInsStatus, nNewInsurancefileCount:=nNewInsurancefileCnt, sInsuranceFileType:=sInsuranceFileType.Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' we need to process it manually, insert a record into Insurance_File_Deferred_RI_Usage


                        m_lReturn = m_oBusiness.InsertInsFileDefRIUsage(v_lInsFileCnt:=lCurrInsFileCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred whilst setting the policy for manual processing.", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDefRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Else
                        'Create Accumulations
                        If nNewInsurancefileCnt <> lCurrInsFileCnt Then
                            m_lReturn = GetAccumulations(nNewInsurancefileCnt)
                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                                Return m_lReturn
                            End If
                        End If
                    End If
                End If
                nInsFile = Trim$(NullToLong(m_vDefRIRiskData(DeferredRIField.edInsuranceFileCnt, lLoopy)))
                sInsRef = Trim$(NullToString(m_vDefRIRiskData(DeferredRIField.edInsuranceRef, lLoopy)))

            Next lLoopy

            ' run the GetBusiness again, as the completed risks are now marked as 'Quoted'
            ' so won't be returned - only risks that have failed auto-processing will be returned
            ' as they will still be have a risk_status of 'RIDEFERRED'
            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred whilst obtaining the deferred reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDefRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return result
            End If

            ' we're all done, but let the user know how many risks need to be done manually, if any
            If m_lItemsFound Then
                'Developer Guide No 168
                _sbrStatus_Panel1.Text = "Manual processing of " & m_lItemsFound & " risks is required."
                sbrStatus.Refresh()

                MessageBox.Show("Processing of risks with deferred reinsurance is complete." & _
                                Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) & _
                                "To manually process the " & CStr(m_lItemsFound) & _
                                " risks that could not be done automatically, " & _
                                Strings.Chr(13) & Strings.Chr(10) & _
                                "run the 'Deferred Reinsurance Amendment' task.", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Else

                'Developer Guide No 168
                _sbrStatus_Panel1.Text = "Processing complete."
                sbrStatus.Refresh()

                MessageBox.Show("Processing of risks with deferred reinsurance is complete.", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the deferred reinsurance on the policy", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessDefRIPolicies", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name:GetBusiness
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetDeferredRIPolicies(r_vDeferredRIArray:=m_vDefRIRiskData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' update the module level variable that holds the number of risks we're dealing with
            If Information.IsArray(m_vDefRIRiskData) Then
                m_lItemsFound = m_vDefRIRiskData.GetUpperBound(1) + 1
            Else
                m_lItemsFound = 0
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            txtPolicyNumber.Text = ""
            txtClientCode.Text = ""
            txtClientName.Text = ""

            ' disable the start button till there's something to do
            cmdStart.Enabled = False

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'Developer Guide No 168
            _sbrStatus_Panel1.Text = sMessage
            sbrStatus.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then

                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            'Developer Guide No 168
            _sbrStatus_Panel1.Text = CStr(m_lItemsFound) & " " & sMessage
            sbrStatus.Refresh()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub frmDeferredRIAuto_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        If Not (m_oBusiness Is Nothing) Then

            ' Terminate the business object

            m_oBusiness.Dispose()
            ' Destroy the instance of the renewals business object
            ' from memory.
            m_oBusiness = Nothing

        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: DisplayMessage
    '
    ' Description: Displays message to report failure of child process.
    '
    ' History: 14/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DisplayMessage) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DisplayMessage(ByVal v_sComponentName As String) As Integer
    'Dim result As Integer = 0
    'Dim sTitle, sMessage As String
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get description from the resource file.

    'sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '

    'sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
    '
    ' Display message.
    'MessageBox.Show(sMessage & v_sComponentName, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
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
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DisplayMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: EnableDisableButtons
    '
    ' Description:
    '
    ' AMB 08-Sep-03: 1.8.6 Deferred Reinsurance development
    '
    ' ***************************************************************** '
    Private Sub EnableDisableButtons()

        Try

            cmdStart.Enabled = Not (m_lItemsFound = 0)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EnableDisableButtons Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EnableDisableButtons", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Function DeletePolicyFromRenewal(ByVal v_lInsuranceFolderCnt As Long) As Long

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim bRenewalsExist As Boolean
        Dim oRenewal As Object

        Try

            m_lReturn = g_oObjectManager.GetInstance(oObject:=oRenewal, sClassName:="bSirRenSelection.Business", vInstanceManager:=PMGetViaClientManager)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oRenewal.GetRenewalsForPolicy(v_lInsFolderCnt:=v_lInsuranceFolderCnt, r_bRenewalsExist:=bRenewalsExist)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            If (bRenewalsExist = True) Then
                m_lReturn = oRenewal.DeleteRenewalsForPolicy()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyFromRenewal", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
       

        Finally
            oRenewal = Nothing
        End Try
        Return nResult
    End Function

End Class