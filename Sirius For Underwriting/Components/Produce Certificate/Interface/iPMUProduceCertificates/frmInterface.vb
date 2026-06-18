Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form


    ' This componenet is designed to produce the following reports
    '1) Credential Certificate
    '2) NonCredential Certificate
    '3) Bulk Certificate




    'Pending Tasks
    ' Check for Error Handlers - Done
    '



    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the general interface object.
    'Private m_oGeneral As iPMUProduceCertificates.General

    'Type of Certicates that can be produced
    '-----------------------------------------------
    Private Const ACCredentialCertificate As String = "Credential Certificate"
    Private Const ACNonCredentialCertificate As String = "Non Credential Certificate"
    Private Const ACBulkCertificate As String = "Bulk Certificate"
    '-----------------------------------------------


    '-----------------------------------------------
    Private Const ACBulkCertFindGClient As Integer = 1
    Private Const ACBulkCertProduceCertificate As Integer = 2
    '-----------------------------------------------

    '-----------------------------------------------
    Private Const ACCredentialFindClient As Integer = 1
    Private Const ACCredentialFindGClient As Integer = 2
    Private Const ACCredentialFindPolicy As Integer = 3
    Private Const ACCredentialInsertComment As Integer = 4
    Private Const ACCredentialProduceCertificate As Integer = 5
    '-----------------------------------------------

    '-----------------------------------------------
    Private Const ACNonCredentialFindClient As Integer = 1
    Private Const ACNonCredentialFindGClient As Integer = 2
    Private Const ACNonCredentialFindPolicy As Integer = 3
    Private Const ACNonCredentialIsNonScheduled As Integer = 4
    Private Const ACNonCredentialInsertComment As Integer = 5
    Private Const ACNonCredentialProduceCertificate As Integer = 6
    '-----------------------------------------------



    '-----------------------------------------------
    'Private Const ACBulkCertStepLabelFindGCClient As String = "Find Certificate Holder"
    'Private Const ACBulkCertStepLabelProduceCertificate As String = "Produce Certificate"
    ''-----------------------------------------------
    '
    ''-----------------------------------------------
    'Private Const ACCredentialStepLabelFindClient As String = "Find Member"
    'Private Const ACCredentialStepLabelFindGCClient As String = "Find Certificate Holder"
    'Private Const ACCredentialStepLabelShowPolicies As String = "Show Policies"
    'Private Const ACCredentialStepLabelInsertComments As String = "Insert Comments"
    'Private Const ACCredentialStepLabelProduceCertificate As String = "Produce Certificate"
    ''-----------------------------------------------
    '
    ''-----------------------------------------------
    'Private Const ACNonCredentialStepLabelFindClient As String = "Find Member"
    'Private Const ACNonCredentialStepLabelFindGCClient As String = "Find Certificate Holder"
    'Private Const ACNonCredentialStepLabelShowPolicies As String = "Show Policies"
    'Private Const ACNonCredentialStepLabelIsAncillary As String = "Non Scheduled Ancillary                                      Personal"
    'Private Const ACNonCredentialStepLabelInsertComments As String = "Insert Comments"
    'Private Const ACNonCredentialStepLabelProducecertificate As String = "Produce Certificate"
    '-----------------------------------------------

    Private Const ACGisDataModelRiskIndex As Integer = 1

    Private m_vPolicies As Object
    '
    Private m_lSelGCPartyCnt As Integer
    Private m_lSelPartyCnt As Integer
    Private m_lInsuranceFileCnt As Integer
    Private m_sProcessType As String = ""
    Private m_bIsExistAssociation As Boolean
    Private m_lCurrentStepNumber As Integer
    Private m_lSelectedProcessIndex As Integer
    Private m_bIsNonScheduledAncillary As Boolean
    Private m_sNonScheduledAncillaryName As String = ""
    Private m_sComment As String = ""
    Private m_bBlankCertificate As Boolean
    Private m_lPolicySelPartyCnt As Integer
    Private m_lSelNumberOfYear As Integer

    'Private sPolicyListCaption(ACPColumnMax) As String

    Private oProcessDetails() As ProcessDetails = Nothing
    'Private m_lReturn As Long
    Private Structure ProcessDetails
        Dim CertificateType As String
        Dim MaxSteps As Integer
        Dim StepCaption() As String
        Dim StepLabel() As String
        Public Shared Function CreateInstance() As ProcessDetails
            Dim result As New ProcessDetails
            result.CertificateType = String.Empty
            Return result
        End Function
    End Structure

    Public WriteOnly Property SelGCPartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lSelGCPartyCnt = Value
        End Set
    End Property

    Public WriteOnly Property SelPartyCnt() As Integer
        Set(ByVal Value As Integer)
            m_lSelPartyCnt = Value
        End Set
    End Property

    Public WriteOnly Property IsExistAssociation() As Boolean
        Set(ByVal Value As Boolean)
            m_bIsExistAssociation = Value
        End Set
    End Property

    Public WriteOnly Property CurrentStepNumber() As Integer
        Set(ByVal Value As Integer)
            m_lCurrentStepNumber = Value
        End Set
    End Property

    Public WriteOnly Property SelectedProcessIndex() As Integer
        Set(ByVal Value As Integer)
            m_lSelectedProcessIndex = Value
        End Set
    End Property

    Public WriteOnly Property NonScheduledAncillaryName() As String
        Set(ByVal Value As String)
            m_sNonScheduledAncillaryName = Value
        End Set
    End Property

    Public WriteOnly Property Comment() As String
        Set(ByVal Value As String)
            m_sComment = Value
        End Set
    End Property

    Public WriteOnly Property BlankCertificate() As Boolean
        Set(ByVal Value As Boolean)
            m_bBlankCertificate = Value
        End Set
    End Property

    Private Sub chkNonScheduledAncillary_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkNonScheduledAncillary.CheckStateChanged
        If chkNonScheduledAncillary.CheckState = CheckState.Checked Then
            txtAncillaryName.ReadOnly = False
            txtAncillaryName.Focus()
        Else
            txtAncillaryName.Text = ""
            txtAncillaryName.ReadOnly = True
        End If
    End Sub

    Private Sub cmdBrowseCertHolder_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowseCertHolder.Click
        Const kMethodName As String = "cmdBrowseCertHolder_Click"
        Dim lReturn As Integer
        Try





            lReturn = BrowseCertHolder()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frmInterface.BrowseCertHolder Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                If gPMFunctions.ToSafeLong(m_lSelGCPartyCnt) <> 0 Then 'And ToSafeInteger(m_lSelPartyCnt) <> 0
                    lReturn = CreateAssociation(lSelGCParty_cnt:=m_lSelGCPartyCnt, lSelParty_cnt:=m_lSelPartyCnt)
                End If
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frmInterface.CreateAssociation Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    Private Function BrowseCertHolder() As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const kMethodName As String = "cmdBrowseClient_Click"

        Try



            Dim lReturn As Integer
            Dim sPartyName, lPartyCnt As String

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = FindClient("GC", m_lSelGCPartyCnt, sPartyName)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frnmInterface.BrowseCertHolder Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                txtCertHolderName.Text = sPartyName
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=cmdBrowseCertHolder.Focused, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Private Sub cmdBrowseClient_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowseClient.Click
        Const kMethodName As String = "BrowseClient"
        Dim lReturn As Integer
        Try





            lReturn = BrowseClient()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            Else
                If gPMFunctions.ToSafeLong(m_lSelGCPartyCnt) <> 0 Then
                    lReturn = CreateAssociation(lSelGCParty_cnt:=m_lSelGCPartyCnt, lSelParty_cnt:=m_lSelPartyCnt)
                End If
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Exit Sub
    End Sub

    Private Function BrowseClient() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "BrowseClient"
        Try


            Dim sPartyName As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(FindClient("NONGC", m_lSelPartyCnt, sPartyName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.BrowseClient Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                txtClientName.Text = sPartyName
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Sub cmdBrowsePolicy_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBrowsePolicy.Click
        Const kMethodName As String = "BrowsePolicy"
        Dim lReturn As Integer
        Try





            optPolicy.Checked = True

            lReturn = BrowsePolicy()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            Else
                If gPMFunctions.ToSafeInteger(m_lSelGCPartyCnt) <> 0 Then
                    lReturn = CreateAssociation(lSelGCParty_cnt:=m_lSelGCPartyCnt, lSelParty_cnt:=m_lSelPartyCnt)
                End If
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Exit Sub
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click
        Const kMethodName As String = "cmdExit_Click"
        Dim lReturn As Integer
        Try



            Me.Close()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdLossRunPrint_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdLossRunPrint.Click
        Const kMethodName As String = "cmdLossRunPrint_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try




            lReturn = CType(PrintLossHistoryLetter(v_bIsLossHistoryOnly:=True), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.PrintLossHistoryLetter Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub

    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNext.Click

        Const kMethodName As String = "cmdNext_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try





            lReturn = CType(ValidateStep(oProcessDetails(m_lSelectedProcessIndex).CertificateType, m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Exit Sub
            End If

            If oProcessDetails(m_lSelectedProcessIndex).CertificateType = ACNonCredentialCertificate And optPolicy.Checked Then
                If m_lCurrentStepNumber = ACCredentialFindGClient Then
                    m_lCurrentStepNumber += 1
                End If
            End If

            m_lCurrentStepNumber += 1
            If m_lCurrentStepNumber > 0 Then
                Select Case oProcessDetails(m_lSelectedProcessIndex).CertificateType
                    Case ACCredentialCertificate
                        lReturn = CType(ProcessNextStepCredential(m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.ProcessNextStepCredential Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Case ACNonCredentialCertificate
                        lReturn = CType(ProcessNextStepNonCredential(m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.ACNonCredentialCertificate Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Case ACBulkCertificate
                        m_lInsuranceFileCnt = 0
                        lReturn = CType(ProcessNextStepBulk(m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.ACNonCredentialCertificate Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                End Select
            End If

            ' SetupScreenControl Function is called only if the Current Step is successfully performed
            ' else skip this function from execution and display user the proper error message
            lReturn = CType(SetupScreenControls(m_lCurrentStepNumber, oProcessDetails(m_lSelectedProcessIndex).CertificateType), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frmInterface.SetupScreenControls Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdBack_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBack.Click
        Const kMethodName As String = "cmdBack_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try




            If oProcessDetails(m_lSelectedProcessIndex).CertificateType = ACNonCredentialCertificate And optPolicy.Checked Then
                If m_lCurrentStepNumber = ACCredentialInsertComment Then
                    m_lCurrentStepNumber -= 1
                End If
            End If

            m_lCurrentStepNumber -= 1
            If m_lCurrentStepNumber > 1 Then
                Select Case oProcessDetails(m_lSelectedProcessIndex).CertificateType
                    Case ACCredentialCertificate
                        lReturn = CType(ProcessNextStepCredential(CurrentStepNumber:=m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.ProcessNextStepCredential Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case ACNonCredentialCertificate
                        lReturn = CType(ProcessNextStepNonCredential(CurrentStepNumber:=m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.ProcessNextStepNonCredential Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case ACBulkCertificate
                        lReturn = CType(ProcessNextStepBulk(CurrentStepNumber:=m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.ProcessNextStepBulk Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                End Select
            End If

            ' SetupScreenControl Function is called only if the Current Step is successfully performed
            ' else skip this function from execution and display user the proper error message
            lReturn = CType(SetupScreenControls(StepNumber:=m_lCurrentStepNumber, ProcessType:=oProcessDetails(m_lSelectedProcessIndex).CertificateType), gPMConstants.PMEReturnCode)


        Catch ex As Exception

            ' DO Not Call any functions before herek or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub

    Private Sub cmdFinish_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFinish.Click
        Const kMethodName As String = "CmdFinish_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try




            Select Case oProcessDetails(m_lSelectedProcessIndex).CertificateType
                Case ACCredentialCertificate
                    lReturn = CType(PrintCertificate(v_bIsCertificateOnly:=False), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.PrintCertificate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    '            lReturn = PrintLossHistoryLetter()
                    '            If lReturn <> PMTrue Then
                    '                RaiseError kMethodName, "iPMUProduceCertificates.frmInterface.PrintLossHistoryLetter Failed", PMLogError
                    '            End If

                Case ACNonCredentialCertificate
                    lReturn = CType(PrintCertificate(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.PrintCertificate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Case ACBulkCertificate
                    lReturn = CType(PrintCertificate(v_bIsBulkCertificate:=True), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.PrintCertificate Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
            End Select


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally



        End Try
        Exit Sub
    End Sub


    Private Function ProcessNextStepBulk(ByVal CurrentStepNumber As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessNextStepBulk"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            Select Case CurrentStepNumber
                Case ACBulkCertFindGClient

                Case ACBulkCertProduceCertificate
                    lReturn = CType(FindPolicies(v_lSelParty_cnt:=m_lSelPartyCnt), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.FindPolicies Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
            End Select

            '
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally






        End Try
        Return result
    End Function

    Private Function ProcessNextStepCredential(ByRef CurrentStepNumber As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessNextStepCredential"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case CurrentStepNumber
                Case ACCredentialFindPolicy
                    ' Produce Policies as soon as the step is sucessfully performed

                    ' It will check whether Party Cnt already selected is
                    ' changed by user or not and will populate Policy list only if it
                    ' is changed.
                    If m_lPolicySelPartyCnt <> m_lSelPartyCnt Then
                        lReturn = CType(FindPolicies(v_lSelParty_cnt:=m_lSelPartyCnt), gPMConstants.PMEReturnCode)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        ' Save Party Cnt on which policies list is produced.
                        m_lPolicySelPartyCnt = m_lSelPartyCnt

                    End If

                    If Information.IsArray(m_vPolicies) Then
                        lReturn = CType(SetupPolicyList(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        m_bBlankCertificate = False
                    Else

                        lvwPolicies.Items.Clear()
                        m_bBlankCertificate = True
                    End If

                Case ACCredentialProduceCertificate
                    m_sComment = txtComment.Text
                Case ACCredentialInsertComment
                    If lvwPolicies.Items.Count > 0 Then
                        m_lInsuranceFileCnt = CInt(ListViewHelper.GetListViewSubItem(lvwPolicies.Items.Item(lvwPolicies.SelectedItems(0).Index), ACPColumnInsuranceFileCnt).Text)
                    End If
            End Select



            '
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    Private Function ProcessNextStepNonCredential(ByRef CurrentStepNumber As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessNextStepNonCredential"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            Dim lReturn As gPMConstants.PMEReturnCode
            Select Case CurrentStepNumber
                Case ACNonCredentialFindPolicy
                    ' Produce Policies as soon as the step is sucessfully performed

                    ' It will check whether Party Cnt already selected is
                    ' changed by user or not and will populate Policy list only if it
                    ' is changed.
                    If optClient.Checked Then
                        If m_lPolicySelPartyCnt <> m_lSelPartyCnt Then
                            lReturn = CType(FindPolicies(v_lSelParty_cnt:=m_lSelPartyCnt), gPMConstants.PMEReturnCode)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            ' Save Party Cnt on which policies list is produced.
                            m_lPolicySelPartyCnt = m_lSelPartyCnt

                            If Information.IsArray(m_vPolicies) Then
                                lReturn = CType(SetupPolicyList(), gPMConstants.PMEReturnCode)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                                End If

                                m_bBlankCertificate = False
                            Else
                                lvwPolicies.Items.Clear()
                                m_bBlankCertificate = True
                            End If
                        End If
                    ElseIf optPolicy.Checked Then
                        lvwPolicies.Items.Clear()
                    End If

                Case ACNonCredentialInsertComment
                    If chkNonScheduledAncillary.CheckState Then
                        m_bIsNonScheduledAncillary = True
                        m_sNonScheduledAncillaryName = gPMFunctions.ToSafeString(txtAncillaryName.Text.Trim())
                    Else
                        m_bIsNonScheduledAncillary = False
                        m_sNonScheduledAncillaryName = ""
                    End If
                    If lvwPolicies.Items.Count > 0 Then
                        m_lInsuranceFileCnt = CInt(ListViewHelper.GetListViewSubItem(lvwPolicies.Items.Item(lvwPolicies.SelectedItems(0).Index), ACPColumnInsuranceFileCnt).Text)
                    End If
                Case ACNonCredentialProduceCertificate
                    m_sComment = txtComment.Text
            End Select



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    Private Function ValidateStep(ByVal ProcessType As String, ByVal StepNumber As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateStep"
        Try


            result = gPMConstants.PMEReturnCode.PMFalse

            Select Case ProcessType
                Case ACCredentialCertificate, ACNonCredentialCertificate
                    Select Case StepNumber
                        Case 1
                            If optClient.Checked Then
                                If txtClientName.Text = "" Then
                                    If Not (txtPolicyRef.Text.Trim().Length > 0) Then
                                        result = gPMConstants.PMEReturnCode.PMFalse
                                        MessageBox.Show("Please Select a Valid Personal or Corporate Client.", "Produce Loss History Letter and Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                        txtClientName.Focus()
                                        Return result
                                    End If
                                End If
                            ElseIf optPolicy.Checked Then
                                If Not (txtPolicyRef.Text.Trim().Length > 0) Then
                                    result = gPMConstants.PMEReturnCode.PMFalse
                                    MessageBox.Show("Please Select a Policy No.", "Produce Loss History Letter and Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                    txtPolicyRef.Focus()
                                    Return result
                                End If
                            End If
                        Case 2
                            If txtCertHolderName.Text = "" Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                MessageBox.Show("Please Select a Valid Group Client.", "Produce Loss History Letter and Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtCertHolderName.Focus()
                                Return result
                            End If
                    End Select

                Case ACBulkCertificate
                    Select Case StepNumber
                        Case 1
                            If txtCertHolderName.Text = "" Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                MessageBox.Show("Please Select a Valid Group Client.", "Produce Loss History Letter and Certificate", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                txtCertHolderName.Focus()
                                Return result
                            End If

                    End Select
            End Select

            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    Private Function SetupScreenControls(ByVal StepNumber As Integer, Optional ByVal ProcessType As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupScreenControls"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            FrameCertificates.Visible = False
            FrameCertHolder.Visible = False
            FrameClient.Visible = False
            lvwPolicies.Visible = False
            FrameAncillary.Visible = False
            frameComment.Visible = False

            If StepNumber <> 0 Then

                Select Case ProcessType
                    Case ACCredentialCertificate
                        Select Case StepNumber
                            Case ACCredentialFindClient
                                FrameClient.Visible = True
                                optPolicy.Visible = False
                                txtPolicyRef.Visible = False
                                cmdBrowsePolicy.Visible = False
                                optClient.Checked = True
                            Case ACCredentialFindGClient
                                FrameCertHolder.Visible = True
                            Case ACCredentialFindPolicy
                                lvwPolicies.Visible = True
                            Case ACCredentialInsertComment
                                frameComment.Visible = True
                            Case ACCredentialProduceCertificate
                                cmdFinish.Text = "&Cert/Loss Run Print "
                                cmdLossRunPrint.Enabled = True

                        End Select

                    Case ACNonCredentialCertificate

                        Select Case StepNumber
                            Case ACNonCredentialFindClient
                                FrameClient.Visible = True
                                optPolicy.Visible = True
                                txtPolicyRef.Visible = True
                                cmdBrowsePolicy.Visible = True
                                If Not optPolicy.Checked Then
                                    optClient.Checked = True
                                End If
                            Case ACNonCredentialFindGClient
                                FrameCertHolder.Visible = True
                            Case ACNonCredentialFindPolicy
                                lvwPolicies.Visible = True
                            Case ACNonCredentialIsNonScheduled
                                FrameAncillary.Visible = True
                            Case ACNonCredentialInsertComment
                                frameComment.Visible = True
                            Case ACNonCredentialProduceCertificate
                                frameComment.Visible = False
                        End Select
                    Case ACBulkCertificate
                        Select Case StepNumber
                            Case ACBulkCertFindGClient
                                FrameCertHolder.Visible = True
                                'ACBulkCertProduceCertificate
                            Case ACBulkCertProduceCertificate
                                '  FrameCertHolder.Visible = False
                        End Select

                End Select
                cmdBack.Enabled = True

                If StepNumber = oProcessDetails(m_lSelectedProcessIndex).MaxSteps Then
                    cmdFinish.Enabled = True
                    cmdNext.Enabled = False
                Else
                    cmdNext.Enabled = True
                    cmdFinish.Enabled = False
                    cmdLossRunPrint.Enabled = False
                End If

                lblStepChild(StepNumber - 1).ForeColor = Color.Black
                imgTickChild(StepNumber - 1).Image = ImageListTick.Images.Item(0)

                If StepNumber > 1 Then
                    lblStepChild(StepNumber - 2).Font = VB6.FontChangeBold(lblStepChild(StepNumber - 2).Font, False)
                    'lblTickChild(StepNumber - 2).FontBold = False
                    imgTickChild(StepNumber - 2).Image = ImageListTick.Images.Item(0)

                End If '

                If StepNumber < oProcessDetails(m_lSelectedProcessIndex).MaxSteps Then
                    lblStepChild(StepNumber).Font = VB6.FontChangeBold(lblStepChild(StepNumber).Font, False)
                    'lblTickChild(StepNumber).FontBold = False
                    imgTickChild(StepNumber).Image = ImageListTick.Images.Item(0)

                    lblStepChild(StepNumber).ForeColor = SystemColors.ControlDark
                    'lblTickChild(StepNumber).ForeColor = &H80000010
                    imgTickChild(StepNumber).Image = ImageListTick.Images.Item(1)

                    FrameStepOptions.Visible = True
                Else
                    FrameStepOptions.Visible = False
                End If

                lblStepChild(StepNumber - 1).Font = VB6.FontChangeBold(lblStepChild(StepNumber - 1).Font, True)
                'lblTickChild(StepNumber - 1).FontBold = True
                imgTickChild(StepNumber - 1).Image = ImageListTick.Images.Item(2)

                lblStepLabel.Text = gPMFunctions.ToSafeString(oProcessDetails(m_lSelectedProcessIndex).StepLabel(StepNumber - 1))

            ElseIf StepNumber = 0 Then
                lReturn = CType(DeallocateLabels(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = CType(GenerateLabels(m_lSelectedProcessIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

                FrameStepOptions.Visible = True
                FrameCertificates.Visible = True
                cmdBack.Enabled = False
                cmdLossRunPrint.Enabled = False
                cmdFinish.Enabled = False
                cmdNext.Enabled = True

                '        FrameClient.Visible = False
                '        FrameCertHolder.Visible = False
                '        lvwPolicies.Visible = False
                '        FrameAncillary.Visible = False

                Select Case m_lSelectedProcessIndex
                    Case 0
                        cmdFinish.Text = "&Cert/Loss Run Print "
                    Case Else
                        cmdFinish.Text = "&Print"
                End Select

                lblStepLabel.Text = "This step will allow you to configure the " &
                                    "wizard to produce Loss History Letter and Certificate, " &
                                    "Bulk Certificate or Certificate only." &
                                    " Just select from the options specified below."
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




            ' Call SetupList to Populate the array in the ListView


        End Try
        Return result
    End Function


    Private Function GenerateLabels(ByRef SelectedProcessIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GenerateLabels"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            'This Function will take process type as input Parameter
            'and will load dynamically the step labels on the basis of process type

            lblStepParent(0).Text = "Produce Loss History Letter"
            lblStepParent(0).Visible = True
            'lblTickParent(0).Visible = True
            imgTickParent(0).Visible = True
            imgTickParent(0).Image = ImageListTick.Images.Item(0)

            lblStepChild(0).Font = VB6.FontChangeBold(lblStepChild(0).Font, False)
            'lblTickChild(0).FontBold = False
            'lblTickChild(0).Visible = True
            imgTickChild(0).Image = ImageListTick.Images.Item(1)
            imgTickChild(0).Visible = True

            lblStepChild(0).Text = oProcessDetails(SelectedProcessIndex).StepCaption(0)
            lblStepChild(0).Visible = True
            lblStepChild(0).ForeColor = SystemColors.ControlDark
            'lblTickChild(0).ForeColor = &H80000010

            For lCount As Integer = 1 To oProcessDetails(SelectedProcessIndex).MaxSteps - 1
                ' Write a code to load and display the labels dynamically
                ContainerHelper.LoadControl(Me, "lblStepChild", lCount)
                lblStepChild(lCount).Text = oProcessDetails(SelectedProcessIndex).StepCaption(lCount)
                lblStepChild(lCount).Left = lblStepChild(lCount - 1).Left
                lblStepChild(lCount).Top = lblStepChild(lCount - 1).Top + VB6.TwipsToPixelsY(270)
                lblStepChild(lCount).ForeColor = SystemColors.ControlDark
                lblStepChild(lCount).Font = VB6.FontChangeBold(lblStepChild(lCount).Font, False)
                lblStepChild(lCount).Visible = True

                '        Load lblTickChild(lCount)
                '        lblTickChild(lCount).Left = lblTickChild(lCount - 1).Left
                '        lblTickChild(lCount).Top = lblTickChild(lCount - 1).Top + 270
                '        lblTickChild(lCount).ForeColor = &H80000010
                '        lblTickChild(lCount).FontBold = False
                '        lblTickChild(lCount).Visible = True

                ContainerHelper.LoadControl(Me, "imgTickChild", lCount)
                imgTickChild(lCount).Left = imgTickChild(lCount - 1).Left
                imgTickChild(lCount).Top = imgTickChild(lCount - 1).Top + VB6.TwipsToPixelsY(270)
                imgTickChild(lCount).Image = ImageListTick.Images.Item(1)
                'imgTickChild(lCount).ForeColor = &H80000010
                'imgTickChild(lCount).FontBold = False
                imgTickChild(lCount).Visible = True

            Next




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



            ' Call SetupList to Populate the array in the ListView



        End Try
        Return result
    End Function

    Private Function DeallocateLabels() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DeallocateLabels"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'This Function will take process type as input Parameter
            'and will load dynamically the step labels on the basis of process type
            lblStepParent(0).Visible = False
            'lblTickParent(0).Visible = False
            'lblTickChild(0).Visible = False
            lblStepChild(0).Visible = False

            imgTickParent(0).Visible = False
            imgTickChild(0).Visible = False
            For lCount As Integer = 1 To lblStepChild.Length - 1 'oProcessDetails(SelectedProcessIndex).MaxSteps - 1
                If Not IsNothing(Me.lblStepChild(lCount)) Then
                    ' Write a code to load and display the labels dynamically
                    ContainerHelper.UnloadControl(Me, "lblStepChild", lCount)
                    'Unload lblTickChild(lCount)
                    ContainerHelper.UnloadControl(Me, "imgTickChild", lCount)
                End If
            Next



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



            ' Call SetupList to Populate the array in the ListView


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: FindPolicies
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function FindPolicies(Optional ByVal v_lSelParty_cnt As Integer = 0, Optional ByVal v_lPolicyCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim bPMUProduceCertificates, bSIRFindInsurance As Object


        ' Since we need to design the functionality which is bit similar with searching policies on         the basis of Risk Index. So we follow the same path and will make little amendments         in SP spu_gis_search_property_find_two to make it work also in accordance to our            scenario. Changes to SPU are specified in the section XX
        ' Instantiate bSirFindInsurance
        ' Call FindLikeIndex(SelParty_Cnt, ,m_vPolicies) Function

        Const kMethodName As String = "FindPolicies"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oFindInsurance As bSIRFindInsurance.Form

        Dim oProduceCertificates As bPMUProduceCertificates.Business

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)


            If Not (Convert.IsDBNull(v_lPolicyCnt) Or IsNothing(v_lPolicyCnt) Or False) Then

                If v_lPolicyCnt > 0 Then

                    If oFindInsurance Is Nothing Then
                        Dim temp_oFindInsurance As Object
                        lReturn = g_oObjectManager.GetInstance(temp_oFindInsurance, "bSIRFindInsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                        oFindInsurance = temp_oFindInsurance
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of bSIRFindInsurance.Form", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If

                    'oFindInsurance.PartyCnt = m_lSelPartyCnt

                    m_vPolicies = Nothing

                    lReturn = oFindInsurance.FindLikeIndex(sIndex:=gPMFunctions.ToSafeString(CStr(v_lPolicyCnt)), lNumberOfRecords:=500, vResultArray:=m_vPolicies, lSpecificDataModelIndex:=ACGisDataModelRiskIndex, sSearchType:="=", lSpecificFieldType:=3)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
                        gPMFunctions.RaiseError(kMethodName, "oFindInsurance.FindLikeIndex Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    oFindInsurance.Dispose()

                ElseIf Not (Convert.IsDBNull(v_lSelParty_cnt) Or IsNothing(v_lSelParty_cnt) Or False) Then
                    If v_lSelParty_cnt > 0 Then
                        If oProduceCertificates Is Nothing Then
                            ' Get an instance of the business object via
                            ' the public object manager.
                            Dim temp_oProduceCertificates As Object
                            lReturn = g_oObjectManager.GetInstance(temp_oProduceCertificates, "bPMUProduceCertificates.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                            oProduceCertificates = temp_oProduceCertificates

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of bPMUProduceCertificates.Business", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            'oFindInsurance.PartyCnt = m_lSelPartyCnt

                            m_vPolicies = Nothing


                            lReturn = oProduceCertificates.FindPolicies(v_lPartyCnt:=m_lSelPartyCnt, r_vResultArray:=m_vPolicies)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue And lReturn <> gPMConstants.PMEReturnCode.PMFalse Then
                                gPMFunctions.RaiseError(kMethodName, "bPMUProduceCertificates.Business.FindPolicies Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If


                            oProduceCertificates.Dispose()

                        End If
                    End If
                End If
            End If

            If Information.IsArray(m_vPolicies) Then
                lReturn = CType(RemoveDuplicates(m_vPolicies), gPMConstants.PMEReturnCode)
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oFindInsurance = Nothing
            oProduceCertificates = Nothing

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)




            ' Call SetupList to Populate the array in the ListView

        End Try
        Return result
    End Function

    Public Function RemoveDuplicates(ByRef m_vArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "RemoveDuplicates"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lCurrFolderCnt As Integer
            Dim bCheckIfRemoved As Boolean
            Dim lMergeIndex As Integer
            Dim vResultArray, vCheckedFiles(,) As Object
            Dim sCurrFileRef As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode


            lCurrFolderCnt = CInt(m_vArray(12, 0))

            sCurrFileRef = gPMFunctions.ToSafeString(CStr(m_vArray(3, 0)))


            For lCount As Integer = 0 To m_vArray.GetUpperBound(1)

                lReturn = CType(CheckIfRemoved(vCheckedFiles:=vCheckedFiles, lFolderCnt:=m_vArray(12, lCount), sFileRef:=m_vArray(3, lCount), bCheckIfRemoved:=bCheckIfRemoved), gPMConstants.PMEReturnCode)
                If lReturn = gPMConstants.PMEReturnCode.PMTrue And Not bCheckIfRemoved Then


                    lReturn = CType(GetMaxRow(m_vArray(12, lCount), m_vArray(3, lCount), lMergeIndex), gPMConstants.PMEReturnCode)
                    If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        lReturn = CType(MergeItem(vResultArray, lMergeIndex), gPMConstants.PMEReturnCode)
                        If Information.IsArray(vCheckedFiles) Then

                            ReDim Preserve vCheckedFiles(1, vCheckedFiles.GetUpperBound(1) + 1)
                        Else
                            ReDim vCheckedFiles(1, 0) ' second for Insurance Ref
                        End If



                        vCheckedFiles(0, vCheckedFiles.GetUpperBound(1)) = m_vArray(12, lCount)



                        vCheckedFiles(1, vCheckedFiles.GetUpperBound(1)) = m_vArray(3, lCount) ' File Ref
                    End If

                End If
            Next


            m_vPolicies = vResultArray


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Private Function GetMaxRow(ByVal lFolderCnt As Integer, ByVal sFileRef As String, ByRef lIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetMaxRow"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lMaxInsuranceCnt As Integer


            For lCount As Integer = 0 To m_vPolicies.GetUpperBound(1)


                If CDbl(m_vPolicies(12, lCount)) = lFolderCnt And CStr(m_vPolicies(3, lCount)) = sFileRef Then

                    If CDbl(m_vPolicies(2, lCount)) > lMaxInsuranceCnt Then

                        lMaxInsuranceCnt = CInt(m_vPolicies(2, lCount))
                        lIndex = lCount
                    End If
                End If
            Next



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Private Function MergeItem(ByRef vArray(,) As Object, ByVal lMergeArray As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "MergeItem"

        Dim lReturn, lUpperBound As Integer
        'ReDim Preserve vArray(UBound(vArray, 2) + 1)

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(vArray) Then

                ReDim Preserve vArray(27, vArray.GetUpperBound(1) + 1)
            Else
                ReDim vArray(27, 0)
            End If


            lUpperBound = vArray.GetUpperBound(1)

            For lCount As Integer = 0 To vArray.GetUpperBound(0)


                vArray(lCount, lUpperBound) = m_vPolicies(lCount, lMergeArray)
            Next



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Private Function CheckIfRemoved(ByVal vCheckedFiles(,) As Object, ByVal sFileRef As String, ByVal lFolderCnt As Integer, ByRef bCheckIfRemoved As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CheckIfRemoved"

        Dim lReturn As Integer
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(vCheckedFiles) Then
                For lCount As Integer = 0 To vCheckedFiles.GetUpperBound(1)


                    If lFolderCnt = CDbl(vCheckedFiles(0, lCount)) And gPMFunctions.ToSafeString(sFileRef) = gPMFunctions.ToSafeString(CStr(vCheckedFiles(1, lCount))) Then
                        bCheckIfRemoved = True
                        Return result
                    End If
                Next
            End If
            bCheckIfRemoved = False



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CheckAssociates
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateAssociation(ByVal lSelGCParty_cnt As Integer, ByVal lSelParty_cnt As Integer) As Integer
        Dim result As Integer = 0
        Dim bSIRParty As Object


        Const kMethodName As String = "CreateAssociation"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim oAssociates As Object
        Dim vAssociates(,) As Object
        Dim vAddAssociates(,) As Object
        Dim lUpperBound As Integer
        Dim sShortName, sResolvedName As String
        Dim UBoundFirstLeaf As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' if we havent got an instance of find party
            If oAssociates Is Nothing Then

                ' get a new instance of find party
                Dim temp_oAssociates As Object
                lReturn = g_oObjectManager.GetInstance(temp_oAssociates, "bSIRParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oAssociates = temp_oAssociates

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed iPMUProduceCertificates.frmInterface.CheckAssociates Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


            lReturn = oAssociates.GetAssociates(vPartyCnt:=lSelGCParty_cnt, vAssociates:=vAssociates)

            If Information.IsArray(vAssociates) Then

                For lCount As Integer = vAssociates.GetLowerBound(1) To vAssociates.GetUpperBound(1)

                    If gPMFunctions.ToSafeLong(CInt(vAssociates(0, lCount))) = m_lSelPartyCnt Then
                        m_bIsExistAssociation = True
                        Exit For
                    End If
                Next
            End If

            If Not m_bIsExistAssociation Then
                If Information.IsArray(vAssociates) Then

                    lUpperBound = vAssociates.GetUpperBound(0)

                    UBoundFirstLeaf = vAssociates.GetUpperBound(1) + 1
                Else
                    lUpperBound = 5
                    UBoundFirstLeaf = 0
                End If

                ReDim vAddAssociates(lUpperBound, UBoundFirstLeaf)

                If Information.IsArray(vAssociates) Then


                    lReturn = CType(MergeArray(vArrayTobeMerge:=vAddAssociates, vArrayMergedWith:=vAssociates), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed iPMUProduceCertificates.frmInterface.CheckAssociates Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                lReturn = oAssociates.GetOtherDetails(vAgentCnt:=m_lSelPartyCnt, vAgentRef:=sShortName, vAgentName:=sResolvedName)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed iPMUProduceCertificates.frmInterface.CheckAssociates Failed", gPMConstants.PMELogLevel.PMLogError)
                End If




                vAddAssociates(0, vAddAssociates.GetUpperBound(1)) = gPMFunctions.ToSafeLong(m_lSelPartyCnt)


                vAddAssociates(1, vAddAssociates.GetUpperBound(1)) = gPMFunctions.ToSafeString(sShortName)


                vAddAssociates(2, vAddAssociates.GetUpperBound(1)) = gPMFunctions.ToSafeString(sResolvedName)


                vAddAssociates(3, vAddAssociates.GetUpperBound(1)) = 1


                vAddAssociates(4, vAddAssociates.GetUpperBound(1)) = "Associate"


                vAddAssociates(5, vAddAssociates.GetUpperBound(1)) = False


                lReturn = oAssociates.UpdateAssociates(vPartyCnt:=m_lSelGCPartyCnt, vAssociates:=vAddAssociates, lPartyRelationshipGroupId:=1)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed iPMUProduceCertificates.frmInterface.CheckAssociates Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: MergeArray
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function MergeArray(ByRef vArrayTobeMerge(,) As Object, ByVal vArrayMergedWith(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "MergeArray"

        Dim lReturn, lInnercount As Integer
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            For lCount As Integer = vArrayMergedWith.GetLowerBound(1) To vArrayMergedWith.GetUpperBound(1) 'm_oXA.LowerBound(1) To m_oXA.UpperBound(1)
                For lInnercount = vArrayMergedWith.GetLowerBound(0) To vArrayMergedWith.GetUpperBound(0)


                    vArrayTobeMerge(lInnercount, lCount) = vArrayMergedWith(lInnercount, lCount)
                Next
                lInnercount = 0
            Next





        Catch excep As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PrintCertificate
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function PrintCertificate(Optional ByVal v_bIsBulkCertificate As Boolean = False, Optional ByVal v_bIsCertificateOnly As Boolean = True) As Integer
        Dim result As Integer = 0
        Dim iPMBReportPrint As Object

        Const kMethodName As String = "PrintCertificate"

        Dim lReturn As Integer

        Dim oReport As iPMBReportPrint.Interface_Renamed
        Dim vKeyArray(1, 21) As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Instance of Reportprint component
            Dim temp_oReport As Object
            lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            With oReport
                'Send Report & Parameters into Report via Setkeys


                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName

                vKeyArray(1, 0) = "PLICO_certificate"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY


                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name

                vKeyArray(1, 2) = "GC_PartyCNT"


                vKeyArray(0, 3) = "GC_PartyCNT"

                vKeyArray(1, 3) = m_lSelGCPartyCnt


                vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameParam2Name

                vKeyArray(1, 4) = "PartyCNT"


                vKeyArray(0, 5) = "PartyCNT"

                vKeyArray(1, 5) = m_lSelPartyCnt


                vKeyArray(0, 6) = PMNavKeyConst.PMKeyNameParam3Name

                vKeyArray(1, 6) = "Comments"


                vKeyArray(0, 7) = "Comments"

                vKeyArray(1, 7) = m_sComment


                vKeyArray(0, 8) = PMNavKeyConst.PMKeyNameParam4Name

                vKeyArray(1, 8) = "AncillaryName"


                vKeyArray(0, 9) = "AncillaryName"

                vKeyArray(1, 9) = m_sNonScheduledAncillaryName


                vKeyArray(0, 10) = PMNavKeyConst.PMKeyNameParam5Name

                vKeyArray(1, 10) = "InsuranceFileCNT"


                vKeyArray(0, 11) = "InsuranceFileCNT"

                vKeyArray(1, 11) = m_lInsuranceFileCnt


                vKeyArray(0, 12) = PMNavKeyConst.PMKeyNameParam6Name

                vKeyArray(1, 12) = "IsBulkCertificates"


                vKeyArray(0, 13) = "IsBulkCertificates"

                vKeyArray(1, 13) = v_bIsBulkCertificate


                vKeyArray(0, 14) = PMNavKeyConst.PMKeyNameParam7Name

                vKeyArray(1, 14) = "IsCertificateOnly"


                vKeyArray(0, 15) = "IsCertificateOnly"

                vKeyArray(1, 15) = v_bIsCertificateOnly

                vKeyArray(0, 16) = PMNavKeyConst.PMKeyNameParam8Name

                vKeyArray(1, 16) = "IsLossHistoryOnlyC"


                vKeyArray(0, 17) = "IsLossHistoryOnlyC"

                vKeyArray(1, 17) = False

                vKeyArray(0, 18) = PMNavKeyConst.PMKeyNameParam9Name

                vKeyArray(1, 18) = "IsLossHistoryOnly"


                vKeyArray(0, 19) = "IsLossHistoryOnly"

                vKeyArray(1, 19) = False

                vKeyArray(0, 20) = PMNavKeyConst.PMKeyNameParam10Name
                vKeyArray(1, 20) = "NumOFYears"

                vKeyArray(0, 21) = "NumOFYears"
                vKeyArray(1, 21) = m_lSelNumberOfYear

                ' Add all the parameters in the similar fashion to the KeyArray


                lReturn = .SetKeys(vKeyArray:=vKeyArray)

                'If m_iIsReportOpen = 1 Then

                lReturn = .Start()


                ' Close Report Component

                .Dispose()

                oReport = Nothing

            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: PrintCertificate
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function PrintLossHistoryLetter(Optional ByVal v_bIsLossHistoryOnly As Boolean = True) As Integer
        Dim result As Integer = 0
        Dim iPMBReportPrint As Object

        Const kMethodName As String = "PrintLossHistoryLetter"


        Dim oReport As iPMBReportPrint.Interface_Renamed
        Dim vKeyArray(1, 13) As Object
        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get Instance of Reportprint component
            Dim temp_oReport As Object
            lReturn = g_oObjectManager.GetInstance(temp_oReport, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oReport = temp_oReport

            With oReport
                'Send Report & Parameters into Report via Setkeys


                vKeyArray(0, 0) = PMNavKeyConst.PMKeyNameReportName

                vKeyArray(1, 0) = "PLICO_LossHistoryLetter"


                vKeyArray(0, 1) = PMNavKeyConst.PMKeyNamePrintReport

                vKeyArray(1, 1) = PMNavKeyConst.AC_VIEW_ONLY


                vKeyArray(0, 2) = PMNavKeyConst.PMKeyNameParam1Name

                vKeyArray(1, 2) = "GC_PartyCNT"


                vKeyArray(0, 3) = "GC_PartyCNT"

                vKeyArray(1, 3) = m_lSelGCPartyCnt


                vKeyArray(0, 4) = PMNavKeyConst.PMKeyNameParam2Name

                vKeyArray(1, 4) = "PartyCNT"


                vKeyArray(0, 5) = "PartyCNT"

                vKeyArray(1, 5) = m_lSelPartyCnt


                vKeyArray(0, 6) = PMNavKeyConst.PMKeyNameParam3Name

                vKeyArray(1, 6) = "Comments"


                vKeyArray(0, 7) = "Comments"

                vKeyArray(1, 7) = ""


                vKeyArray(0, 8) = PMNavKeyConst.PMKeyNameParam4Name

                vKeyArray(1, 8) = "InsuranceFileCNT"


                vKeyArray(0, 9) = "InsuranceFileCNT"

                vKeyArray(1, 9) = m_lInsuranceFileCnt


                vKeyArray(0, 10) = PMNavKeyConst.PMKeyNameParam5Name

                vKeyArray(1, 10) = "IsLossHistoryOnly"


                vKeyArray(0, 11) = "IsLossHistoryOnly"

                vKeyArray(1, 11) = v_bIsLossHistoryOnly


                vKeyArray(0, 12) = PMNavKeyConst.PMKeyNameParam6Name
                vKeyArray(1, 12) = "NumOFYears"

                vKeyArray(0, 13) = "NumOFYears"
                vKeyArray(1, 13) = m_lSelNumberOfYear

                ' Add all the parameters in the similar fashion to the KeyArray


                lReturn = .SetKeys(vKeyArray:=vKeyArray)

                'If m_iIsReportOpen = 1 Then

                lReturn = .Start()


                ' Close Report Component

                .Dispose()
                oReport = Nothing
            End With



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Private Sub Form_Initialize_Renamed()
        ' Forms initialise event.
        Try

            iPMFunc.ShowFormInTaskBar_Attach()


            ' Set the mouse pointer to busy.

        Catch excep As System.Exception


            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


        End Try


    End Sub

    ' ***************************************************************** '
    ' Name: FindClient
    '
    ' Description:
    '
    ' ***************************************************************** '

    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oFindParty As Object
        Dim oAllowedparties As Object

        Try

            iPMFunc.ShowFormInTaskBar_Detach()




            ReDim Preserve oProcessDetails(2)
            oProcessDetails(0).CertificateType = ACCredentialCertificate
            oProcessDetails(0).MaxSteps = 5
            ReDim Preserve oProcessDetails(0).StepCaption(4)
            oProcessDetails(0).StepCaption(0) = "Find Member"
            oProcessDetails(0).StepCaption(1) = "Find Certificate Holder"
            oProcessDetails(0).StepCaption(2) = "Show Policies"
            oProcessDetails(0).StepCaption(3) = "Insert Comments"
            oProcessDetails(0).StepCaption(4) = "Produce Certificate"

            ReDim Preserve oProcessDetails(0).StepLabel(4)
            oProcessDetails(0).StepLabel(0) = "Select member from the Find Client screen. " &
                                              "You can only work " &
                                              "with Personal or Corporate Clients."
            oProcessDetails(0).StepLabel(1) = "Select Certificate Holder from the Find " &
                                              "Client screen, You can" &
                                              " only select Group Clients."
            oProcessDetails(0).StepLabel(2) = "Below is the List of Policies on " &
                                              "which Client is attached to the Risk."
            oProcessDetails(0).StepLabel(3) = "Comments (If any)."
            oProcessDetails(0).StepLabel(4) = "Click 'Loss Run Print' for Loss History only. " &
                                              "Click 'Cert/Loss Run Print' to produce the Certificate and the Loss History Report."




            oProcessDetails(1).CertificateType = ACNonCredentialCertificate
            oProcessDetails(1).MaxSteps = 6
            ReDim Preserve oProcessDetails(1).StepCaption(5)
            oProcessDetails(1).StepCaption(0) = "Find Member"
            oProcessDetails(1).StepCaption(1) = "Find Certificate Holder"
            oProcessDetails(1).StepCaption(2) = "Show Policies"
            oProcessDetails(1).StepCaption(3) = "Non Scheduled Ancillary Personal"
            oProcessDetails(1).StepCaption(4) = "Insert Comments"
            oProcessDetails(1).StepCaption(5) = "Produce Certificate"

            ReDim Preserve oProcessDetails(1).StepLabel(5)
            oProcessDetails(1).StepLabel(0) = oProcessDetails(0).StepLabel(0)
            oProcessDetails(1).StepLabel(1) = oProcessDetails(0).StepLabel(1)
            oProcessDetails(1).StepLabel(2) = oProcessDetails(0).StepLabel(2)
            oProcessDetails(1).StepLabel(3) = "Tick the box below if this is for Ancillary Personal."
            oProcessDetails(1).StepLabel(4) = oProcessDetails(0).StepLabel(3)
            oProcessDetails(1).StepLabel(5) = oProcessDetails(0).StepLabel(4)

            oProcessDetails(2).CertificateType = ACBulkCertificate
            oProcessDetails(2).MaxSteps = 2
            ReDim Preserve oProcessDetails(2).StepCaption(1)
            oProcessDetails(2).StepCaption(0) = "Find Certificate Holder"
            oProcessDetails(2).StepCaption(1) = "Produce Certificate"

            ReDim Preserve oProcessDetails(2).StepLabel(1)
            oProcessDetails(2).StepLabel(0) = oProcessDetails(0).StepLabel(1)
            oProcessDetails(2).StepLabel(1) = oProcessDetails(0).StepLabel(4)


            CurrentStepNumber = 0
            lReturn = CType(SetupScreenControls(m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
            SelectedProcessIndex = 0
            Me.optCredentialCertificate.Checked = True
            lReturn = SetInterfaceDefaults()



        Catch ex As Exception
            'm_lErrorNumber = PMError

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
    End Sub

    Public Sub mnuExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuExit.Click

        Const kMethodName As String = "mnuExit_Click"
        Dim lReturn As Integer
        Try

            Me.Close()


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)
        Finally



        End Try
    End Sub

    Public Sub mnuRestart_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _mnuRestart_1.Click
        optCredentialCertificate.Checked = False
        optCredentialCertificate.Checked = True
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optClient_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optClient.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            txtPolicyRef.Text = ""
            txtPolicyRef.Enabled = False
            cmdBrowsePolicy.Enabled = False
            txtClientName.Enabled = True
            cmdBrowseClient.Enabled = True
            m_lInsuranceFileCnt = 0
        End If
    End Sub

    Private Sub optCredentialCertificate_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optCredentialCertificate.CheckedChanged
        Const kMethodName As String = "optCredentialCertificate_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try
            If eventSender.Checked Then
                If isInitializingComponent Then
                    Exit Sub
                End If


                lReturn = InitialiseWizard()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frmInterface.InitialiseWizard Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                m_lSelectedProcessIndex = 0
                CboNumberOfYear.Visible = True
                LblLoss.Visible = True

                lReturn = CType(SetupScreenControls(StepNumber:=m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frmInterface.SetupScreenControls Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally




        End Try
        Exit Sub
    End Sub

    Private Sub optNonCredentialCertificate_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optNonCredentialCertificate.CheckedChanged
        Const kMethodName As String = "optNonCredentialCertificate_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try
            If eventSender.Checked Then
                If isInitializingComponent Then
                    Exit Sub
                End If



                lReturn = InitialiseWizard()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.InitialiseWizard Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lSelectedProcessIndex = 1
                CboNumberOfYear.Visible = False
                LblLoss.Visible = False

                lReturn = CType(SetupScreenControls(StepNumber:=m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.InitialiseWizard Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally




        End Try
        Exit Sub
    End Sub

    Private Sub optBulkCertificate_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optBulkCertificate.CheckedChanged
        Const kMethodName As String = "optBulkCertificate_Click"
        Dim lReturn As gPMConstants.PMEReturnCode
        Try
            If eventSender.Checked Then
                If isInitializingComponent Then
                    Exit Sub
                End If


                lReturn = InitialiseWizard()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "InitialiseWizard Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lSelectedProcessIndex = 2
                lReturn = CType(SetupScreenControls(StepNumber:=m_lCurrentStepNumber), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupScreenControls Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally




        End Try
        Exit Sub
    End Sub

    ' ***************************************************************** '
    ' Name: FindClient
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function FindClient(ByVal sPartyType As String, ByRef lPartyCnt As Integer, ByRef sPartyName As String) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object
        Const kMethodName As String = "FindClient"

        Dim lReturn As Integer

        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim oAllowedparties As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' if we havent got an instance of find party
            If oFindParty Is Nothing Then

                ' get a new instance of find party
                Dim temp_oFindParty As Object
                lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                oFindParty = temp_oFindParty

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Failed to GetInstnace of iPMBFindParty.Interface", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If

            ' Set component properties and start interface

            oFindParty.CallingAppName = ACApp

            oFindParty.IgnoreDriversAndWitnesses = True

            oFindParty.NotEditable = gPMConstants.PMEReturnCode.PMTrue

            If sPartyType = "GC" Then

                oFindParty.SpecialParty = "GC"
            ElseIf sPartyType = "NONGC" Then
                ReDim oAllowedparties(1)

                oAllowedparties(0) = "Personal Client"

                oAllowedparties(1) = "Corporate Client"


                oFindParty.ValidPartyTypesArray = oAllowedparties
            End If


            ' start the find part component

            lReturn = oFindParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMuProduceCertificates.frmInterface.FindClient Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the returned partycnt

            If oFindParty.PartyCnt > 0 Then

                lPartyCnt = oFindParty.PartyCnt

                sPartyName = oFindParty.LongName
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oFindParty = Nothing





        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetpupPolicyList
    '
    ' Description: Updates all interface details from the business
    '              object.
    '
    ' ***************************************************************** '
    Public Function SetupPolicyList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupPolicyList"
        Dim oListItem As ListViewItem
        Dim lStart, lEnd, lCount As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            lvwPolicies.Items.Clear()
            If Information.IsArray(m_vPolicies) Then

                lStart = m_vPolicies.GetLowerBound(1)

                lEnd = m_vPolicies.GetUpperBound(1)

                lCount = 0

                For lRow As Integer = lStart To lEnd
                    ''If Not (m_vPolicies(ACPInsuranceType, lRow) = 1 Or m_vPolicies(ACPInsuranceType, lRow) = 4 Or m_vPolicies(ACPInsuranceType, lRow) = 7 Or m_vPolicies(ACPInsuranceType, lRow) = 10) Then

                    oListItem = lvwPolicies.Items.Insert(lCount, gPMFunctions.ToSafeString(CStr(m_vPolicies(ACPPolicyNumber, lRow))))

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnPolicyType).Text = m_vPolicies(ACPPolicyType, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnProductName).Text = m_vPolicies(ACPProductName, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnRegarding).Text = m_vPolicies(ACPRegarding, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnRenewalDate).Text = m_vPolicies(ACPRenewalDate, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnAgent).Text = m_vPolicies(ACPAgent, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnPremium).Text = m_vPolicies(ACPPremium, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnPolicyStatus).Text = m_vPolicies(ACPPolicyStatus, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnRiskTypeDescription).Text = m_vPolicies(ACPRiskTypeDescription, lRow).Trim()

                    ListViewHelper.GetListViewSubItem(oListItem, ACPColumnInsuranceFileCnt).Text = m_vPolicies(ACPInsuranceFileCnt, lRow).Trim()

                    lCount += 1
                    ''End If
                Next lRow
                'Set first item selected by default.
                lvwPolicies.Items(0).Selected = True
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception
            'Developer Guide No. 178
            ListViewFunc.ListViewBatchEnd()

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally

            '        Return result
            '        Resume


            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetupPolicyList"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            For lCount As Integer = 1 To ACPColumnMax + 1
                lvwPolicies.Columns.Add("", 94)
            Next

            lvwPolicies.Columns.Item(ACPColumnPolicyNumber).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwPolicies.Columns.Item(ACPColumnPolicyType).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwPolicies.Columns.Item(ACPColumnProductName).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwPolicies.Columns.Item(ACPColumnRegarding).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwPolicies.Columns.Item(ACPColumnRenewalDate).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwPolicies.Columns.Item(ACPColumnAgent).Width = CInt(VB6.TwipsToPixelsX(1800))
            lvwPolicies.Columns.Item(ACPColumnPremium).Width = CInt(VB6.TwipsToPixelsX(1200))
            lvwPolicies.Columns.Item(ACPColumnPolicyStatus).Width = CInt(VB6.TwipsToPixelsX(1300))
            lvwPolicies.Columns.Item(ACPColumnRiskTypeDescription).Width = CInt(VB6.TwipsToPixelsX(2000))
            lvwPolicies.Columns.Item(ACPColumnInsuranceFileCnt).Width = CInt(0)

            lvwPolicies.Columns.Item(ACPColumnPolicyNumber).Text = "Policy Number"
            lvwPolicies.Columns.Item(ACPColumnPolicyType).Text = "Policy Type"
            lvwPolicies.Columns.Item(ACPColumnProductName).Text = "Product Name"
            lvwPolicies.Columns.Item(ACPColumnRegarding).Text = "Regarding"
            lvwPolicies.Columns.Item(ACPColumnRenewalDate).Text = "Renewal Date"
            lvwPolicies.Columns.Item(ACPColumnAgent).Text = "Agent"
            lvwPolicies.Columns.Item(ACPColumnPremium).Text = "Premium"
            lvwPolicies.Columns.Item(ACPColumnPolicyStatus).Text = "Policy Status"
            lvwPolicies.Columns.Item(ACPColumnRiskTypeDescription).Text = "Risk Type Description"
            lvwPolicies.Columns.Item(ACPColumnInsuranceFileCnt).Text = "InsuranceFileCnt"

            CboNumberOfYear.SelectedIndex = 1

        Catch ex As Exception
            'Developer Guide No. 178
            ListViewFunc.ListViewBatchEnd()

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    Private Function InitialiseWizard() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "InitialiseWizard"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Default values of all the controls
            txtAncillaryName.Text = ""
            txtCertHolderName.Text = ""
            txtClientName.Text = ""
            optClient.Checked = True
            txtComment.Text = ""
            chkNonScheduledAncillary.CheckState = CheckState.Unchecked
            lvwPolicies.Items.Clear()

            ' Reset All the properties
            m_lSelGCPartyCnt = 0
            m_lSelPartyCnt = 0
            m_bIsExistAssociation = False
            m_lCurrentStepNumber = 0
            m_lSelectedProcessIndex = 0
            m_bIsNonScheduledAncillary = False
            m_sNonScheduledAncillaryName = ""
            m_sComment = ""
            m_bBlankCertificate = False
            m_lPolicySelPartyCnt = 0
            m_lInsuranceFileCnt = 0

            FrameAncillary.Visible = False
            FrameCertHolder.Visible = False
            FrameClient.Visible = False
            frameComment.Visible = False

            cmdLossRunPrint.Enabled = False
            cmdFinish.Enabled = False


        Catch ex As Exception
            'Developer Guide No. 178
            ListViewFunc.ListViewBatchEnd()

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    Public Function BrowsePolicy() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "BrowsePolicy"
        Try


            Dim sPolicyRef As String = ""
            Dim lReturn As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(FindPolicy(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMUProduceCertificates.frmInterface.BrowsePolicy Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: FindPolicy
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function FindPolicy() As Integer
        Dim result As Integer = 0


        Dim oFindPolicy As iPMBFindInsurance.Interface_Renamed

        Dim lReturn As Integer
        Dim sInsuranceRef As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create Find Insurance object
            Dim temp_oFindPolicy As Object
            lReturn = g_oObjectManager.GetInstance(temp_oFindPolicy, sClassName:="iPMBFindInsurance.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindPolicy = temp_oFindPolicy

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            ' Set component properties and start interface

            oFindPolicy.CallingAppName = ACApp

            oFindPolicy.InsReference = txtPolicyRef.Text

            oFindPolicy.FindMode = 1

            oFindPolicy.InsFileType = gSIRLibrary.SIRInsFileTypePolicy

            lReturn = oFindPolicy.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process object 'iPMBFindInsurance.Interface'.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If

            'Retrieve InsuranceRef and set as PolicyRef

            If oFindPolicy.Status <> gPMConstants.PMEReturnCode.PMCancel Then

                sInsuranceRef = oFindPolicy.InsReference

                m_lInsuranceFileCnt = oFindPolicy.InsFileCnt

                txtClientName.Text = ""
                'Display Policy Reference on form
                txtPolicyRef.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=sInsuranceRef.Trim())

                ' Destroy Find Insurance object

                oFindPolicy.Dispose()
                oFindPolicy = Nothing

                ' Do search
                ' Call cmdFindNow_Click
            Else
                ' Destroy Find Insurance object

                oFindPolicy.Dispose()
                oFindPolicy = Nothing
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindPolicyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindPolicy", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Private Sub optPolicy_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optPolicy.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            txtClientName.Text = ""
            txtClientName.Enabled = False
            cmdBrowseClient.Enabled = False
            txtPolicyRef.Enabled = True
            cmdBrowsePolicy.Enabled = True
            m_lSelPartyCnt = 0
        End If
    End Sub

    Private Sub CboNumberOfYear_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CboNumberOfYear.SelectedIndexChanged
        If CboNumberOfYear.SelectedIndex = 0 Then
            m_lSelNumberOfYear = 0
        Else
            m_lSelNumberOfYear = CboNumberOfYear.SelectedItem
        End If

    End Sub
End Class