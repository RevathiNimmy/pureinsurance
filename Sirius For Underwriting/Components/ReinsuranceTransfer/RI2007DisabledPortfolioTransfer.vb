Option Strict On
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmRI2007DisabledPortfolioTransfer
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmRI2007DisabledPortfolioTransfer
    '
    ' Date: 06/07/04
    '
    ' Description: Interface for RI portfolio transfer.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmRI2007DisabledPortfolioTransfer"

    ' Object parameter members.
    Private m_nStatus As PMEReturnCode
    Private m_nErrorNumber As Integer

    Private Enum DeferredRIField
        edInsuranceFileCnt = 0
        edInsuranceRef = 1
        edClientCode = 2
        edClientName = 3
        edTransferDate = 4
        edInsuranceFileTypeId = 5
    End Enum

    Private m_oPolicies(,) As Object

    ' Stores the return value for the a function call.
    Private m_nReturn As Integer
    Private m_nItemsFound As Integer

    Private m_nProgressValue As Integer
    Private m_sStatusBarText As String = ""

    Private m_oBusiness As bSIRRIPortfolioTransfer.RI2007DisabledBusiness

    Public Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_nStatus
        End Get
        Set(ByVal Value As Integer)
            ' set the interface exit status.
            m_nStatus = CType(Value, PMEReturnCode)
        End Set
    End Property

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        m_nStatus = PMEReturnCode.PMCancel
        Me.Hide()
    End Sub

    Private Sub cmdTransfer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTransfer.Click

        Dim sMessage As String = ""

        Try

            ' Get policies to be processed
            m_nReturn = GetBusiness()

            ' Display message box according to number of policies returned
            If m_nItemsFound < 1 Then
                ' No policy matches criteria, warn user and do nothing
                sMessage = "No policies were found that match your search criteria."
                '   CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirm1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=Resources.ResourceManager))
                MessageBox.Show(sMessage, "RI Portfolio Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' Policies found, ask user confirmation before processing
                sMessage = "policie(s) have been found, proceed with transfer?"
                ' sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACConfirm2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                If MessageBox.Show(CStr(m_nItemsFound) & " " & sMessage, "RI Portfolio Transfer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    ' Process those policies
                    m_nReturn = TransferPolicies()
                End If
            End If

            ' If an error's occurred, it should have been handled already
            ' Set up the interface again
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            cmdCancel.Text = "&Close"
            cmdCancel.Enabled = True

        Catch excep As System.Exception

            m_nErrorNumber = PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error occurred whilst processing the policy.", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="cmdTransfer", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim sMessage As String
        Dim sTitle As String

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_nErrorNumber = PMEReturnCode.PMTrue

            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting
            ' the interface.
            m_nStatus = PMEReturnCode.PMCancel

            ' Create business  object
            m_oBusiness = New bSIRRIPortfolioTransfer.RI2007DisabledBusiness

            m_nReturn = CInt(m_oBusiness.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=RI2007DisabledMainModule.ACApp))

            If m_nReturn <> PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_nErrorNumber = PMEReturnCode.PMFalse

                ' Display error stating the problem.

                sTitle = My.Resources.Resources.str1302
                '   CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                sMessage = My.Resources.Resources.str1303
                'CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception
            m_nErrorNumber = PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub
    ''' <summary>
    ''' frmRIPortfolioTransfer Load
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmRIPortfolioTransfer_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            m_nReturn = SetInterfaceDefaults()
            If m_nReturn <> PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred setting interface defaults", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=CObj(Information.Err().Number), vErrDesc:=CObj(Information.Err().Description))
            End If

        Catch excep As System.Exception

            m_nErrorNumber = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the interface object", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ''' <summary>
    ''' transfer all selected policies to new RI model
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function TransferPolicies() As Integer

        Dim nResult As Integer = 0
        Dim nCurrInsFileCnt As Integer
        Dim sMessage As String = ""
        Dim iCount As Integer ' E007
        Dim dtTransferDate As Date
        Dim nRenewalStatus As Integer
        Dim bMsg As Integer
        Dim msgbx As Integer
        Dim nCntUnderRenewal As Integer
        Dim nCurrInsuranceFileTypeId As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            m_nReturn = PMEReturnCode.PMTrue
            ' Disable the buttons
            cmdCancel.Enabled = False
            dtTransferDate = txtDate.Value
            nCntUnderRenewal = 0

            m_nReturn = GetUnderRenewalPoliciesCount(nCntUnderRenewal)

            If nCntUnderRenewal > 0 Then
                msgbx = MsgBox("WARNING : " & nCntUnderRenewal & " policy versions found which are being Portfolio transferred and are under renewal." & vbCrLf & _
                                 "These will automatically be deleted from the renewal cycle." & vbCrLf & _
                                 "User will need to reselect these after transfer, in order to get correct reinsurance split on renewal version." & vbCrLf & _
                                 "Do You want to proceed?", vbYesNo, "WARNING - Policies Under Renewal!")
                If msgbx <> vbYes Then
                    Return m_nReturn
                End If
            End If
            Dim bSkipProcessing As Boolean = False
            For iLoopy As Integer = 0 To (m_nItemsFound - 1)
                bSkipProcessing = False
                nRenewalStatus = -1
                ' Update the interface
                txtPolicyNumber.Text = gPMFunctions.NullToString(m_oPolicies(DeferredRIField.edInsuranceRef, iLoopy)).Trim()
                txtPolicyNumber.Refresh()
                txtClientCode.Text = gPMFunctions.NullToString(m_oPolicies(DeferredRIField.edClientCode, iLoopy)).Trim()
                txtClientCode.Refresh()
                txtClientName.Text = gPMFunctions.NullToString(m_oPolicies(DeferredRIField.edClientName, iLoopy)).Trim()
                txtClientName.Refresh()
                nCurrInsFileCnt = gPMFunctions.NullToLong(m_oPolicies(DeferredRIField.edInsuranceFileCnt, iLoopy))
                _sbrStatus_Panel1.Text = "Processing Policy..."
                sbrStatus.Refresh()

                m_nReturn = CheckInRenewal(v_nInsuranceFileCnt:=nCurrInsFileCnt, r_nRenewalStatus:=nRenewalStatus)

                If m_nReturn <> PMEReturnCode.PMTrue Then
                    MessageBox.Show("CheckInRenewal failed for insurance_file_cnt = " + CStr(nCurrInsFileCnt), "RI PortFolio", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    bSkipProcessing = True
                End If

                If (nRenewalStatus <> -1) Then

                    m_nReturn = DeletePolicyFromRenewal(v_nInsuranceFileCnt:=nCurrInsFileCnt)

                    If m_nReturn <> PMEReturnCode.PMTrue OrElse m_nReturn = PMEReturnCode.PMNotFound Then
                        MessageBox.Show("DeletePolicyFromRenewal failed for insurance_file_cnt = " + CStr(nCurrInsFileCnt), "RI Portfolio", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        bSkipProcessing = True
                    End If
                    'Do not process under renewal version as it is already deleted
                    nCurrInsuranceFileTypeId = gPMFunctions.NullToLong(m_oPolicies(DeferredRIField.edInsuranceFileTypeId, iLoopy))
                    If m_nReturn = PMEReturnCode.PMTrue AndAlso nCurrInsuranceFileTypeId = 3 Then
                        bSkipProcessing = True
                    End If
                End If
                If Not bSkipProcessing Then
                    Dim nNewInsurancefileCnt As Integer = 0
                    Dim bPendingRI As Boolean = False
                    ' Process policy
                    m_nReturn = m_oBusiness.ProcessSinglePolicy(v_nInsuranceFileCnt:=nCurrInsFileCnt, v_dtTransferDate:=CDate(m_oPolicies(DeferredRIField.edTransferDate, iLoopy)), r_sMessage:=sMessage,
                                                                v_nNewInsuranceFileCnt:=nNewInsurancefileCnt, v_bIgnoreClaims:=chkIgnoreClaims.Checked, r_bPendingRI:=bPendingRI)

                    If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue OrElse bPendingRI Then
                        ' we need to process it manually, insert a record into Insurance_File_Deferred_RI_Usage


                        m_nReturn = CInt(m_oBusiness.InsertInsFilePTRIUsage(v_lInsFileCnt:=nCurrInsFileCnt,
                                                               v_dtTransferDate:=CDate(m_oPolicies(DeferredRIField.edTransferDate, iLoopy))))

                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            iPMFunc.LogMessage(
                                    iType:=PMConst.PMLogOnError,
                                    sMsg:="An error has occurred whilst setting the policy for manual processing.",
                                    vApp:=RI2007DisabledMainModule.ACApp,
                                    vClass:=ACClass,
                                    vMethod:="TransferPolicies",
                                    vErrNo:=CObj(Err.Number),
                                    vErrDesc:=CObj(Err.Description))

                            iPMFunc.SetMousePointer(PMConst.PMMouseNormal)
                            Return m_nReturn
                        End If


                        iCount = iCount + 1
                    Else
                        'Create Accumulations
                        m_nReturn = GetAccumulations(nNewInsurancefileCnt)
                        If (m_nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                            Return m_nReturn
                        End If
                    End If

                    If m_nReturn <> PMEReturnCode.PMTrue Then
                        iPMFunc.LogMessage( _
                                iType:=PMConst.PMLogOnError, _
                                sMsg:="An error has occurred whilst insurancefile key in log file", _
                                vApp:=RI2007DisabledMainModule.ACApp, _
                                vClass:=ACClass, _
                                vMethod:="TransferPolicies", _
                                vErrNo:=CObj(Err.Number), _
                                vErrDesc:=CObj(Err.Description))

                        iPMFunc.SetMousePointer(PMConst.PMMouseNormal)
                        Return m_nReturn
                    End If
                End If
            Next iLoopy

            ' Finished!
            _sbrStatus_Panel1.Text = "Processing complete."
            sbrStatus.Refresh()
            ' E007
            If iCount = 0 Then
                MessageBox.Show("Processing of policies is complete.", "Processing Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                sMessage = "Processing of policies is complete." + CStr(iCount) + " Policies needed to be manually processed."
                MsgBox(sMessage, CType(MessageBoxButtons.OK, MsgBoxStyle), "Processing Complete")
            End If
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransferPolicies Failed", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="TransferPolicies", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function

    Private Function GetAccumulations(ByVal v_nNewInsuranceFileCnt As Integer) As Integer

        Dim oKeys(2, 1) As Object

        Try
            m_nReturn = PMEReturnCode.PMTrue
            Dim oObject As iPMUAccumulationValues.Interface_Renamed = New iPMUAccumulationValues.Interface_Renamed

            m_nReturn = oObject.Initialise()

            If (m_nReturn <> PMEReturnCode.PMTrue) Then
                MsgBox("error init")
                Return m_nReturn
            End If

            m_nReturn = oObject.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)

            oKeys(0, 0) = "insurance_file_cnt"
            oKeys(1, 0) = v_nNewInsuranceFileCnt
            m_nReturn = oObject.SetKeys(vKeyArray:=oKeys)

            m_nReturn = oObject.Start()

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetAccumulations", "Failed GetAllRiskStatus ", gPMConstants.PMELogLevel.PMLogError)
            End If

            oObject.Dispose()

            If (m_nReturn <> PMEReturnCode.PMTrue) Then
                MsgBox("error terminate")
                Return m_nReturn
            End If

            Return m_nReturn
        Catch excep As System.Exception

            m_nReturn = gPMConstants.PMEReturnCode.PMError
         
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccumulations Failed", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="TransferPolicies", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)
            Return m_nReturn
        End Try

    End Function
    ''' <summary>
    ''' Get all policies from DB which are using a replaced RI model
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetBusiness() As PMEReturnCode

        Dim nResult As Integer
        Dim nProductID As Integer
        Dim dtTransferDate As Date

        Try

            nResult = PMEReturnCode.PMTrue

            ' Display a searching message
            DisplayStatusSearching()

            ' Get selection criteria from interface
            nProductID = cboProducts.ItemId

            dtTransferDate = txtDate.Value

            ' Get matching policies

            m_nReturn = m_oBusiness.GetPoliciesPortfolioTransfer(v_nProductID:=nProductID, v_dtTransferDate:=dtTransferDate, r_oPolicyArray:=m_oPolicies)

            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' Update the module level variable that holds the number of policies we're dealing with
            If Information.IsArray(m_oPolicies) Then
                m_nItemsFound = m_oPolicies.GetUpperBound(1) + 1
            Else
                m_nItemsFound = 0
            End If

            ' Display a searching message
            DisplayStatusFound()

            ' Set the mouse pointer to normal
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Return CType(nResult, PMEReturnCode)

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBusiness failed", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return CType(nResult, PMEReturnCode)

        End Try
    End Function

    ''' <summary>
    ''' Sets all of the interface default values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = PMEReturnCode.PMTrue

            ' Default some fields
            txtPolicyNumber.Text = ""
            txtClientCode.Text = ""
            txtClientName.Text = ""
            cboProducts.FirstItem = "All"
            txtDate.Value = DateTime.Today

            ' Display all language specific captions.
            m_nReturn = DisplayCaptions()
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Display the status searching message.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayStatusSearching()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then
                '   sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _sbrStatus_Panel1.Text = sMessage
            sbrStatus.Refresh()

        Catch excep As System.Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="DisplayStatusSearching", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    ''' <summary>
    ''' Display the status found message.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub DisplayStatusFound()

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.
            If sMessage = "" Then
                '    sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _sbrStatus_Panel1.Text = CStr(m_nItemsFound) & " " & sMessage
            sbrStatus.Refresh()

        Catch excep As System.Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="DisplayStatusFound", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)
        End Try
    End Sub

    Private Sub frmRI2007DisabledPortfolioTransfer_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        If Not (m_oBusiness Is Nothing) Then

            ' Terminate the business object
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing

        End If

    End Sub

    Private Function DisplayCaptions() As Integer

        Dim nResult As Integer = 0
        Try

            nResult = PMEReturnCode.PMTrue

            ' Display all language specific captions.

            lblProduct.Text = "Product:" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblTransferDate.Text = "Transfer Date:" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblPolicyNumber.Text = "Policy Number:" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientCode.Text = "Client Code:" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lblClientName.Text = "Client Name:" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            fmeSelectPolicy.Text = "Select Policies For Batch Transfer" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            fmeCurrentPolicy.Text = "Current Policy" ', iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' CheckInRenewal
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_nRenewalStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckInRenewal(ByVal v_nInsuranceFileCnt As Integer, _
                               ByRef r_nRenewalStatus As Integer) As Integer

        Try

            m_nReturn = PMEReturnCode.PMTrue

            m_nReturn = m_oBusiness.CheckInRenewal(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, _
                                                   r_nRenewalStatus:=r_nRenewalStatus)

            If (m_nReturn <> PMEReturnCode.PMTrue) Then
                Return m_nReturn
            End If
        Catch excep As Exception

            m_nReturn = PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="CheckInRenewal Failed", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="CheckInRenewal", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)

        End Try
        Return m_nReturn
    End Function

    ''' <summary>
    ''' remove renewal version of policy, renewal_status and all associate records
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeletePolicyFromRenewal(ByVal v_nInsuranceFileCnt As Integer) As Integer

        Dim oRenewal As bSIRRenewal.Business = Nothing

        Try

            m_nReturn = PMEReturnCode.PMTrue
            oRenewal = New bSIRRenewal.Business
            m_nReturn = CInt(oRenewal.Initialise(sUsername:="", sPassword:="", iUserID:=1, iSourceID:=1, iLanguageID:=1, iCurrencyID:=26, iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=RI2007DisabledMainModule.ACApp))

            If m_nReturn <> PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            m_nReturn = oRenewal.DeletePolicyFromRenewal(v_lInsuranceFileCnt:=v_nInsuranceFileCnt)

        Catch excep As Exception
            m_nReturn = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="DeletePolicyFromRenewal", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)
        Finally

            oRenewal.dispose()
        End Try
        Return m_nReturn
    End Function

    ''' <summary>
    ''' Get all under renewal policies which are coming in portfolio cycle with this criteria
    ''' </summary>
    ''' <param name="nUnderRenewalPoliciesCount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetUnderRenewalPoliciesCount(ByRef nUnderRenewalPoliciesCount As Integer) As Integer

        Dim nProductID As Integer
        Dim dtTransferDate As Date
        Dim oRenPoliciesCnt(,) As Object

        Try
            m_nReturn = PMEReturnCode.PMTrue
            nUnderRenewalPoliciesCount = 0

            ' Get selection criteria from interface
            nProductID = cboProducts.ItemId
            dtTransferDate = txtDate.Value

            ' Get matching policies
            m_nReturn = m_oBusiness.GetUnderRenewalPoliciesCount(v_nProductID:=nProductID, _
                    v_dtTransferDate:=dtTransferDate, _
                    r_oPolicyArray:=oRenPoliciesCnt)

            If (m_nReturn <> PMEReturnCode.PMTrue) Then
                Return m_nReturn
            End If

            ' Update the module level variable that holds the number of policies we're dealing with
            If IsArray(oRenPoliciesCnt) Then
                nUnderRenewalPoliciesCount = CInt(oRenPoliciesCnt(0, 0))
            Else
                nUnderRenewalPoliciesCount = 0
            End If
            Return m_nReturn
        Catch excep As Exception

            m_nReturn = PMEReturnCode.PMError
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to GetUnderRenewalPoliciesCount", vApp:=RI2007DisabledMainModule.ACApp, vClass:=ACClass, vMethod:="GetUnderRenewalPoliciesCount", vErrNo:=CObj(Information.Err().Number), vErrDesc:=excep.Message, excep:=excep)
        End Try

    End Function

End Class
