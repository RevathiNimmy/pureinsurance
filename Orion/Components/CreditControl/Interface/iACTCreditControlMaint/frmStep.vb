Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmStep
    Inherits Windows.Forms.Form

    '====================================================================
    '   Class/Module: frmStep
    '   Description :
    '
    '====================================================================
    '   Maintenance History
    '
    '    07 January 2003    S Dissanayake    Created.
    '
    '====================================================================

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmStep"

    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)
    Public Status As Integer
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Stores the return value for the a function call.

    ' Form control
    Private m_oFormfields As Object

    Private m_lReturn As Integer
    Private m_vDocTemplateList As Object
    Private m_sBusinessType As String = ""

    Private m_lStepID As Integer

    Private m_vTaskGroupUserGroups(,) As Object
    Private m_vTaskGroupTask(,) As Object
    Private m_vLookupDetails(,) As Object
    Private m_vLookupTables(,) As Object
    Private m_sPrevTaskGroup As String = ""

    Private m_lTaskGroupId As Integer
    Private m_lTaskId As Integer
    Private m_lUserGroupId As Integer
    Private m_sStepDescription As String = ""
    Private m_sAgencyOrUnderwriting As String = ""
    Private m_bCaptionRenWTGUpdate As Boolean

    Private m_vInstalmentFailureCount As Object
    Private m_lNextAvailableInstalmentFailureCount As Object

    Private m_lAutoCancelDocumentTemplate1 As Integer
    Private m_lAutoCancelDocumentTemplate2 As Integer

    Private m_vAutoCancelDocumentTemplate1TriggerAmount As Object
    Private m_vAutoCancelDocumentTemplate2TriggerAmount As Object
    Private m_vWriteOffToleranceAmount As Object
    Private m_lWriteOffReasonId As Integer

    Private m_nJumpToNextStepBroker As Integer
    Private m_nJumpToNextStepBrokerSingleInst As Integer
    Private m_nBrokerDaysSingleInst As Integer
    Private m_nAccountAmtSingleInst As Integer
    Private m_nBrokerLetterIdSingleInst As Integer

#Region "Public Properties"
    ''' <summary>
    ''' JumpToNextStepBroker  - CC Steps for Broker
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Property JumpToNextStepBroker() As Integer
    '    Get
    '        Return chkJumptonextstepBrokerSingleInst.CheckState
    '    End Get

    '    Set(ByVal Value As Integer)
    '        If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
    '            chkJumptonextstepBrokerSingleInst.CheckState = CheckState.Unchecked
    '        Else
    '            chkJumptonextstepBrokerSingleInst.CheckState = Value
    '        End If
    '    End Set
    'End Property
    Public Property JumpToNextStepBroker() As Integer
        Get
            Return chkJumptonextstepBroker.CheckState
        End Get

        Set(ByVal Value As Integer)
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkJumptonextstepBroker.CheckState = CheckState.Unchecked
            Else
                chkJumptonextstepBroker.CheckState = Value
            End If
        End Set
    End Property
    ''' <summary>
    ''' JumpToNextStepBrokerSingleInst -  - CC Steps for Single Instalment Broker
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Property JumpToNextStepBrokerSingleInst() As Integer
    '    Get
    '        Return m_nJumpToNextStepBrokerSingleInst
    '    End Get
    '    Set(ByVal Value As Integer)
    '        m_nJumpToNextStepBrokerSingleInst = Value
    '    End Set
    'End Property
    Public Property JumpToNextStepBrokerSingleInst() As Integer
        Get
            Return chkJumptonextstepBrokerSingleInst.CheckState
        End Get
        Set(ByVal Value As Integer)
            chkJumptonextstepBrokerSingleInst.CheckState = Value
        End Set
    End Property
    ''' <summary>
    ''' Broker days for Single Instalment Plan
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BrokerDaysSingleInst() As Integer
        Get
            If txtBrokerDaysSingleInst.Text.Trim() = "" Then
                Return Nothing
            Else
                Return CInt(txtBrokerDaysSingleInst.Text)
            End If
        End Get
        Set(ByVal Value As Integer)
            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtBrokerDaysSingleInst.Text = ""
            Else
                txtBrokerDaysSingleInst.Text = Value
            End If
        End Set
    End Property
    ''' <summary>
    ''' AccountAmtSingleInst
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountAmtSingleInst() As Integer
        Get
            If txtAccountAmtSingleInst.Text.Trim() = "" Then

                Return Nothing
            Else
                Return CDec(txtAccountAmtSingleInst.Text)
            End If
        End Get
        Set(ByVal Value As Integer)
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtAccountAmtSingleInst.Text = ""
            Else
                txtAccountAmtSingleInst.Text = CDec(Value)
            End If
        End Set
    End Property
    ''' <summary>
    ''' Letter to be produced for Single Instalment Plan
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BrokerLetterIdSingleInst() As Integer
        Get
            Dim nId As Integer
            If cboBrokerLetterSingleInst.SelectedIndex < 0 Then
                nId = 0
            Else
                nId = m_vDocTemplateList.GetValue(1, cboBrokerLetterSingleInst.SelectedIndex)
            End If

            If nId = 0 Then

                Return Nothing
            Else
                Return nId
            End If
        End Get

        Set(ByVal Value As Integer)

            Dim nListIndex As Integer

            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboBrokerLetterSingleInst.SelectedIndex = -1
            Else

                m_lReturn = GetComboDocuMatch(v_lDocID:=Value, r_lListindex:=nListIndex)
                If m_lReturn = PMEReturnCode.PMTrue Then
                    If cboBrokerLetterSingleInst.Items.Count > 0 Then
                        cboBrokerLetterSingleInst.SelectedIndex = nListIndex
                    End If
                Else
                    cboBrokerLetterSingleInst.SelectedIndex = -1
                End If
            End If

        End Set


    End Property

    Public Property WriteOffReasonId() As Integer
        Get
            Return m_lWriteOffReasonId
        End Get
        Set(ByVal Value As Integer)
            m_lWriteOffReasonId = Value
        End Set
    End Property

    'developer guide no.101
    'Public Property WriteOffToleranceAmount() As String
    Public Property WriteOffToleranceAmount() As Object
        Get
            Return m_vWriteOffToleranceAmount
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            'm_vWriteOffToleranceAmount = CStr(Value)
            m_vWriteOffToleranceAmount = Value
        End Set
    End Property


    Public Property AutoCancelDocumentTemplate1() As Integer
        Get
            Return m_lAutoCancelDocumentTemplate1
        End Get
        Set(ByVal Value As Integer)
            m_lAutoCancelDocumentTemplate1 = Value
        End Set
    End Property


    Public Property AutoCancelDocumentTemplate2() As Integer
        Get
            Return m_lAutoCancelDocumentTemplate2
        End Get
        Set(ByVal Value As Integer)
            m_lAutoCancelDocumentTemplate2 = Value
        End Set
    End Property

    'developer guide no.101
    'Public Property AutoCancelDocumentTemplate1TriggerAmount() As String
    Public Property AutoCancelDocumentTemplate1TriggerAmount() As Object
        Get
            Return m_vAutoCancelDocumentTemplate1TriggerAmount
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            'm_vAutoCancelDocumentTemplate1TriggerAmount = CStr(Value)
            m_vAutoCancelDocumentTemplate1TriggerAmount = Value
        End Set
    End Property

    'developer guide no.101
    'Public Property AutoCancelDocumentTemplate2TriggerAmount() As String
    Public Property AutoCancelDocumentTemplate2TriggerAmount() As Object
        Get
            Return m_vAutoCancelDocumentTemplate2TriggerAmount
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            'm_vAutoCancelDocumentTemplate2TriggerAmount = CStr(Value)
            m_vAutoCancelDocumentTemplate2TriggerAmount = Value
        End Set
    End Property


    'Private m_oBusiness As Object
    '
    'Public Property Set Business(ByVal v_oBusiness As Object)
    '    Set m_oBusiness = v_oBusiness
    'End Property

    Public WriteOnly Property NextAvailableInstalmentFailureCount() As Integer
        Set(ByVal Value As Integer)
            m_lNextAvailableInstalmentFailureCount = Value
        End Set
    End Property

    'developer guide no.101
    'Public Property InstalmentFailureCount() As Integer
    Public Property InstalmentFailureCount() As Object
        Get
            Return m_vInstalmentFailureCount
        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)

            'm_vInstalmentFailureCount = CInt(Value)
            m_vInstalmentFailureCount = Value
        End Set
    End Property

    Public Property TaskId() As Integer
        Get
            Return m_lTaskId
        End Get
        Set(ByVal Value As Integer)
            m_lTaskId = Value
        End Set
    End Property
    Public Property TaskGroupId() As Integer
        Get
            Return m_lTaskGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lTaskGroupId = Value
        End Set
    End Property

    Public Property UserGroupId() As Integer
        Get
            Return m_lUserGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lUserGroupId = Value
        End Set
    End Property
    Public Property StepDescription() As String
        Get
            Return m_sStepDescription
        End Get
        Set(ByVal Value As String)
            m_sStepDescription = Value
        End Set
    End Property
    'DEVELOPER GUIDE NO 33
    Public WriteOnly Property LookupTables() As Object(,)
        Set(ByVal Value As Object(,))
            m_vLookupTables = Value
        End Set
    End Property
    'DEVELOPER GUIDE NO 33
    Public WriteOnly Property LookupDetails() As Object(,)
        Set(ByVal Value As Object(,))
            m_vLookupDetails = Value
        End Set
    End Property
    'DEVELOPER GUIDE NO 33
    Public WriteOnly Property TaskGroupTasks() As Object(,)
        Set(ByVal Value As Object(,))
            m_vTaskGroupTask = Value
        End Set
    End Property
    'DEVELOPER GUIDE NO 33
    Public WriteOnly Property TaskGroupUsers() As Object(,)
        Set(ByVal Value As Object(,))
            m_vTaskGroupUserGroups = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)


    Public Property StepId() As Integer
        Get
            Return m_lStepID
        End Get
        Set(ByVal Value As Integer)
            m_lStepID = Value
        End Set
    End Property

    Public WriteOnly Property BusinessType() As String
        Set(ByVal Value As String)
            m_sBusinessType = Value
        End Set
    End Property

    Public WriteOnly Property DocumentList() As Object(,)
        'dEVELOPER Guide No. 33
        Set(ByVal Value As Object(,))

            'automatocally set document list array with first item being blank
            Dim vNewArray As Object

            'what if no documents yet created
            If Not IsArray(Value) Then

                m_vDocTemplateList = 0
                Exit Property
            End If

            'Parse the array and get values out into our modular array. The combo listindex will
            'match the elements of our array
            ReDim vNewArray(2, (Value.GetUpperBound(1) + 1))
            'Add first item manually

            vNewArray(0, 0) = 0

            vNewArray(1, 0) = 0

            vNewArray(2, 0) = "(None)"

            For lLoop As Integer = 0 To Value.GetUpperBound(1)

                vNewArray(0, lLoop + 1) = lLoop + 1


                vNewArray(1, lLoop + 1) = CInt(Value(0, lLoop))


                vNewArray(2, lLoop + 1) = Value(1, lLoop)
            Next lLoop



            m_vDocTemplateList = vNewArray

        End Set
    End Property

    'developer guide no.101
    Public Property StepNumber() As Object
        Get
            If txtStep.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtStep.Text))
                Return CInt(txtStep.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtStep.Text = ""
            Else

                'txtStep.Text = CStr(Value)
                txtStep.Text = Value
            End If
        End Set
    End Property

    'developer guide no.1001
    Public Property NextStep() As Object
        Get
            If txtNextStep.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtNextStep.Text))
                Return CInt(txtNextStep.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtNextStep.Text = ""
            Else

                'txtNextStep.Text = CStr(Value)
                txtNextStep.Text = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property PreviousStep() As String
    Public Property PreviousStep() As Object
        Get
            If txtPreviousStep.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtPreviousStep.Text))
                Return CInt(txtPreviousStep.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtPreviousStep.Text = ""
            Else

                'txtPreviousStep.Text = CStr(Value)
                txtPreviousStep.Text = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property OffHoldStep() As String
    Public Property OffHoldStep() As Object
        Get
            If txtOffHoldStep.Text.Trim() = "" Then

                Return Nothing
            Else
                Return txtOffHoldStep.Text.Trim()
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtOffHoldStep.Text = ""
            Else

                'txtOffHoldStep.Text = CStr(Value)
                txtOffHoldStep.Text = Value
            End If
        End Set
    End Property
    'developer guide no.
    'Public Property ElapsedDays() As String
    Public Property ElapsedDays() As Object
        Get
            If txtElapsedDays.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtElapsedDays.Text))
                Return CInt(txtElapsedDays.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtElapsedDays.Text = ""
            Else
                'txtElapsedDays.Text = CStr(Value)
                txtElapsedDays.Text = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property BrokerDays() As String
    Public Property BrokerDays() As Object
        Get
            If txtBrokerDays.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtBrokerDays.Text))
                Return CInt(txtBrokerDays.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtBrokerDays.Text = ""
            Else

                'txtBrokerDays.Text = CStr(Value)
                txtBrokerDays.Text = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property PolicyAmt() As String
    Public Property PolicyAmt() As Object
        Get
            If txtPolicyAmt.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CDec(txtPolicyAmt.Text))
                Return CDec(txtPolicyAmt.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtPolicyAmt.Text = ""
            Else

                'txtPolicyAmt.Text = CStr(CDec(Value))
                txtPolicyAmt.Text = CDec(Value)
            End If
        End Set
    End Property

    Public Property AccountAmt() As Object
        Get
            If txtAccountAmt.Text.Trim() = "" Then

                Return Nothing
            Else
                Return CDec(txtAccountAmt.Text)
            End If
        End Get
        Set(ByVal Value As Object)
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtAccountAmt.Text = ""
            Else
                txtAccountAmt.Text = CDec(Value)
            End If
        End Set
    End Property


    Public Property JumpToNextStep() As Object
        Get

            Return chkJumpToNextStep.CheckState
        End Get

        Set(ByVal Value As Object)


            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkJumpToNextStep.CheckState = CheckState.Unchecked
            Else


                'chkJumpToNextStep.CheckState = CInt(Value)
                chkJumpToNextStep.CheckState = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property CheckAutoCancelRules() As String
    Public Property CheckAutoCancelRules() As Object
        Get
            'Return CStr(chkCheckAutoCancel.CheckState)
            Return chkCheckAutoCancel.CheckState
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkCheckAutoCancel.CheckState = CheckState.Unchecked
            Else


                'chkCheckAutoCancel.CheckState = CInt(Value)
                chkCheckAutoCancel.CheckState = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property RunAutoCancelRules() As String
    Public Property RunAutoCancelRules() As Object
        Get
            'Return CStr(chkRunAutoCancel.CheckState)
            Return chkRunAutoCancel.CheckState
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkRunAutoCancel.CheckState = CheckState.Unchecked
            Else


                'chkRunAutoCancel.CheckState = CInt(Value)
                chkRunAutoCancel.CheckState = Value
            End If
        End Set
    End Property


    'developer guide no.101
    'Public Property CheckAutoLapseRenewal() As String
    Public Property CheckAutoLapseRenewal() As Object
        Get
            'Return CStr(chkAutoLapseRenewal.CheckState)
            Return chkAutoLapseRenewal.CheckState
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkAutoLapseRenewal.CheckState = CheckState.Unchecked
            Else


                'chkAutoLapseRenewal.CheckState = CInt(Value)
                chkAutoLapseRenewal.CheckState = Value
            End If
        End Set
    End Property


    'developer guide no.101
    'Public Property RecurringDays() As String
    Public Property RecurringDays() As Object
        Get
            If txtRecurringDays.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtRecurringDays.Text))
                Return CInt(txtRecurringDays.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            'If Convert.IsDBNull(Value) Or IsNothing(Value) Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtRecurringDays.Text = ""
            Else

                'txtRecurringDays.Text = CStr(Value)
                txtRecurringDays.Text = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property Reprint() As String
    Public Property Reprint() As Object
        Get
            'Return CStr(chkReprint.CheckState)
            Return chkReprint.CheckState
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkReprint.CheckState = CheckState.Unchecked
            Else


                'chkReprint.CheckState = CInt(Value)
                chkReprint.CheckState = Value
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property ClientLetterId() As Integer
    Public Property ClientLetterId() As Object
        Get
            Dim lId As Integer

            If cboClientLetter.SelectedIndex < 0 Then
                lId = 0
            Else

                'lId = CInt(m_vDocTemplateList(1, cboClientLetter.SelectedIndex))
                lId = m_vDocTemplateList(1, cboClientLetter.SelectedIndex)
            End If

            If lId = 0 Then

                Return Nothing
            Else
                Return lId
            End If
        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)
            Dim lListIndex As Integer



            'If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboClientLetter.SelectedIndex = -1
            Else

                'm_lReturn = GetComboDocuMatch(v_lDocID:=CInt(Value), r_lListindex:=lListIndex)
                m_lReturn = GetComboDocuMatch(v_lDocID:=Value, r_lListindex:=lListIndex)
                If m_lReturn = PMEReturnCode.PMTrue Then
                    If cboClientLetter.Items.Count > 0 Then
                        cboClientLetter.SelectedIndex = lListIndex
                    End If
                Else
                    cboClientLetter.SelectedIndex = -1
                End If
            End If

        End Set
    End Property


    'developer guide no.101
    'Public Property ClientLetterId2() As Integer
    Public Property ClientLetterId2() As Object
        Get
            Dim lId As Integer

            If cboClientLetter2.SelectedIndex < 0 Then
                lId = 0
            Else

                'lId = CInt(m_vDocTemplateList(1, cboClientLetter2.SelectedIndex))
                lId = m_vDocTemplateList(1, cboClientLetter2.SelectedIndex)
            End If

            If lId = 0 Then

                Return Nothing
            Else
                Return lId
            End If
        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)
            Dim lListIndex As Integer




            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboClientLetter2.SelectedIndex = -1
            Else

                m_lReturn = GetComboDocuMatch(v_lDocID:=CInt(Value), r_lListindex:=lListIndex)
                If m_lReturn = PMEReturnCode.PMTrue Then
                    If cboClientLetter2.Items.Count > 0 Then
                        cboClientLetter2.SelectedIndex = lListIndex
                    End If
                Else
                    cboClientLetter2.SelectedIndex = -1
                End If
            End If

        End Set
    End Property
    'Developer Guide No. 101
    'Public Property OIPLetterId() As Integer
    Public Property OIPLetterId() As Object
        Get
            Dim lId As Integer

            If cboOIPLetter.SelectedIndex < 0 Then
                lId = 0
            Else

                lId = CInt(m_vDocTemplateList(1, cboOIPLetter.SelectedIndex))
            End If

            If lId = 0 Then

                Return Nothing
            Else
                Return lId
            End If
        End Get
        Set(ByVal Value As Object)
            Dim lListIndex As Integer



            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboOIPLetter.SelectedIndex = -1
            Else

                m_lReturn = GetComboDocuMatch(v_lDocID:=CInt(Value), r_lListindex:=lListIndex)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If cboOIPLetter.Items.Count > 0 Then
                        cboOIPLetter.SelectedIndex = lListIndex
                    End If
                Else
                    cboOIPLetter.SelectedIndex = -1
                End If
            End If

        End Set
    End Property

    'developer guide no.101
    'Public Property OIPLetterId2() As Integer
    Public Property OIPLetterId2() As Object
        Get
            Dim lId As Integer

            If cboOIPLetter2.SelectedIndex < 0 Then
                lId = 0
            Else

                'lId = CInt(m_vDocTemplateList(1, cboOIPLetter2.SelectedIndex))
                lId = m_vDocTemplateList(1, cboOIPLetter2.SelectedIndex)
            End If

            If lId = 0 Then

                Return Nothing
            Else
                Return lId
            End If
        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)
            Dim lListIndex As Integer



            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboOIPLetter2.SelectedIndex = -1
            Else

                m_lReturn = GetComboDocuMatch(v_lDocID:=CInt(Value), r_lListindex:=lListIndex)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If cboOIPLetter2.Items.Count > 0 Then
                        cboOIPLetter2.SelectedIndex = lListIndex
                    End If
                Else
                    cboOIPLetter2.SelectedIndex = -1
                End If
            End If

        End Set
    End Property
    'developer guide no.101
    'Public Property BrokerReportId() As String
    Public Property BrokerReportId() As Object
        Get
            If cboPMLookupBrokerReport.ListIndex < 1 Then

                Return Nothing
            Else
                'Return CStr(cboPMLookupBrokerReport.ItemId)
                Return cboPMLookupBrokerReport.ItemId
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then

                cboPMLookupBrokerReport.ListIndex = 0
            Else

                cboPMLookupBrokerReport.ItemId = CInt(Value)
            End If
        End Set
    End Property

    'developer guide no.101
    'Public Property WrkManagerTaskId() As Integer
    Public Property WrkManagerTaskId() As Object
        Get
            If cboPMLookupWrkTask.ListIndex < 1 Then

                Return Nothing
            Else
                Return cboPMLookupWrkTask.ItemId
            End If
        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboPMLookupWrkTask.ListIndex = 0
            Else

                'cboPMLookupWrkTask.ItemId = CInt(Value)
                cboPMLookupWrkTask.ItemId = Value
            End If
        End Set
    End Property


    Public Property WrkManagerTaskId2() As Object
        Get
            If cboPMLookupWrkTask2.ListIndex < 1 Then

                Return Nothing
            Else
                Return cboPMLookupWrkTask2.ItemId
            End If
        End Get
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboPMLookupWrkTask2.ListIndex = 0
            Else

                cboPMLookupWrkTask2.ItemId = Value
            End If
        End Set
    End Property


    Public Property BrokerLetterId() As Object
        Get
            Dim lId As Integer
            If cboBrokerLetter.SelectedIndex < 0 Then
                lId = 0
            Else

                lId = m_vDocTemplateList(1, cboBrokerLetter.SelectedIndex)
            End If

            If lId = 0 Then

                Return Nothing
            Else
                Return lId
            End If
        End Get

        Set(ByVal Value As Object)

            Dim lListIndex As Integer



            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboBrokerLetter.SelectedIndex = -1
            Else

                'm_lReturn = GetComboDocuMatch(v_lDocID:=CInt(Value), r_lListindex:=lListIndex)
                m_lReturn = GetComboDocuMatch(v_lDocID:=Value, r_lListindex:=lListIndex)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If cboBrokerLetter.Items.Count > 0 Then
                        cboBrokerLetter.SelectedIndex = lListIndex
                    End If
                Else
                    cboBrokerLetter.SelectedIndex = -1
                End If
            End If

        End Set
    End Property

    'Public Property Get UserGroup() As String
    '    UserGroup = uctPMLookupUserGroup.ItemCaption
    'End Property

    'Public Property Get UserGroupId() As Variant
    '    If cboPMLookupUserGroup.ListIndex < 1 Then
    '        UserGroupId = Null
    '    Else
    '        UserGroupId = cboPMLookupUserGroup.ItemId
    '    End If
    'End Property
    '
    'Public Property Let UserGroupId(ByRef vInput As Variant)
    '    If IsNull(vInput) Or Trim(vInput) = "" Then
    '        cboPMLookupUserGroup.ListIndex = 0
    '    Else
    '        cboPMLookupUserGroup.ItemId = vInput
    '    End If
    'End Property
    '
    'Public Property Get UserGroupId2() As Variant
    '    If cboPMLookupUserGroup2.ListIndex < 1 Then
    '        UserGroupId2 = Null
    '    Else
    '        UserGroupId2 = cboPMLookupUserGroup2.ItemId
    '    End If
    'End Property
    '
    'Public Property Let UserGroupId2(ByRef vInput As Variant)
    '    If IsNull(vInput) Or Trim(vInput) = "" Then
    '        cboPMLookupUserGroup2.ListIndex = 0
    '    Else
    '        cboPMLookupUserGroup2.ItemId = vInput
    '    End If
    'End Property

    'Public Property Get ActionType() As Variant
    '    If cboPMLookupActionType.ListIndex < 1 Then
    '        ActionType = Null
    '    Else
    '        ActionType = cboPMLookupActionType.ItemId
    '    End If
    'End Property
    '
    'Public Property Let ActionType(ByRef vInput As Variant)
    '    If IsNull(vInput) Or Trim(vInput) = "" Then
    '        cboPMLookupActionType.ListIndex = 0
    '    Else
    '        cboPMLookupActionType.ItemId = vInput
    '    End If
    'End Property
    '
    'Public Property Get ActionType2() As Variant
    '    If cboPMLookupActionType2.ListIndex < 1 Then
    '        ActionType2 = Null
    '    Else
    '        ActionType2 = cboPMLookupActionType2.ItemId
    '    End If
    'End Property
    '
    'Public Property Let ActionType2(ByRef vInput As Variant)
    '    If IsNull(vInput) Or Trim(vInput) = "" Then
    '        cboPMLookupActionType2.ListIndex = 0
    '    Else
    '        cboPMLookupActionType2.ItemId = vInput
    '    End If
    'End Property


    Public Property TolerancePercentage1() As Object
        Get
            If txtTolPercent1.Text.Trim() = "" Then

                Return Nothing
            Else
                Return StringsHelper.Format(txtTolPercent1.Text, "##.0000")
            End If
        End Get
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtTolPercent1.Text = ""
            Else
                txtTolPercent1.Text = Value
            End If
        End Set
    End Property


    Public Property TolerancePercentage2() As Object
        Get
            If txtTolPercent2.Text.Trim() = "" Then

                Return Nothing
            Else
                Return StringsHelper.Format(txtTolPercent2.Text, "##.0000")
            End If
        End Get
        Set(ByVal Value As Object)

            'If Convert.IsDBNull(Value) Or IsNothing(Value) Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtTolPercent2.Text = ""
            Else

                txtTolPercent2.Text = Value
            End If
        End Set
    End Property

    Public WriteOnly Property CaptionRenWTGUpdate() As Object
        Set(ByVal Value As Object)

            If Information.IsDBNull(Value) OrElse IsNothing(Value) OrElse Value = False Then
                m_bCaptionRenWTGUpdate = False
            Else
                m_bCaptionRenWTGUpdate = True
            End If
        End Set
    End Property



    Public Property StopAccount() As Object
        Get
            Return chkStopAccount.CheckState
        End Get
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkStopAccount.CheckState = CheckState.Unchecked
            Else


                chkStopAccount.CheckState = Value
            End If
        End Set
    End Property
#End Region
    ' IsOptionalNum - test value for numeric or blank
    '
    ' Kevin Grandison
    ' 24/06/2003
    '
    Private Function IsOptionalNum(ByVal v_vValue As TextBox, ByVal v_sLabel As String) As Boolean

        Dim result As Boolean = False
        Const sMsgBoxTitle As String = "Credit Control Rule Item"
        Try


            result = True

            Dim dbNumericTemp As Double
            If Not Double.TryParse(v_vValue.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And v_vValue.Text <> "" Then
                MessageBox.Show(v_sLabel & " must be numeric.", sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate " & sMsgBoxTitle & " '" & v_sLabel & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="IsOptionalNum", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboTaskGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskGroup.SelectedIndexChanged
        SetupTaskCombos()

        ' store the currenct task group
        m_sPrevTaskGroup = cboTaskGroup.Text

    End Sub
    ''' <summary>
    ''' Event for Check Auto Cancel
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub chkCheckAutoCancel_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCheckAutoCancel.CheckStateChanged
        If m_sBusinessType = ACBusTypeINS OrElse m_sBusinessType = AcBusTypeINSC OrElse m_sBusinessType = AcBusTypeINSH Then
            If chkCheckAutoCancel.CheckState = CheckState.Checked Then
                'cboClientLetter.SelectedIndex = 0
                'cboClientLetter.Enabled = False   As per Sarah mail Work itrm # 2615
                cboBrokerLetter.SelectedIndex = 0
                cboBrokerLetter.Enabled = False
                If cboBrokerLetterSingleInst.Items.Count > 0 Then
                    cboBrokerLetterSingleInst.SelectedIndex = 0
                End If
                cboBrokerLetterSingleInst.Enabled = False
            ElseIf chkRunAutoCancel.CheckState = CheckState.Unchecked Then
                cboClientLetter.Enabled = True
                cboBrokerLetter.Enabled = True
                cboBrokerLetterSingleInst.Enabled = True

            End If
        End If
    End Sub
    ''' <summary>
    ''' Check State Changed for RunAuto Cancel
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub chkRunAutoCancel_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRunAutoCancel.CheckStateChanged
        If m_sBusinessType = ACBusTypeINS Or m_sBusinessType = AcBusTypeINSC Or m_sBusinessType = AcBusTypeINSH Then
            If chkRunAutoCancel.CheckState = CheckState.Checked Then
                cboClientLetter.SelectedIndex = 0
                cboClientLetter.Enabled = False
                cboBrokerLetter.SelectedIndex = 0
                cboBrokerLetter.Enabled = False
                If cboBrokerLetterSingleInst.Items.Count > 0 Then
                    cboBrokerLetterSingleInst.SelectedIndex = 0
                End If
                cboBrokerLetterSingleInst.Enabled = False
            ElseIf chkRunAutoCancel.CheckState = CheckState.Unchecked Then
                cboClientLetter.Enabled = True
                cboBrokerLetter.Enabled = True
                cboBrokerLetterSingleInst.Enabled = True
            End If
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub
    ''' <summary>
    ''' OK Click
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Dim nStepVal As Integer
        Try

            'Check mandatory controls have been entered into.

            m_lReturn = m_oFormfields.CheckMandatoryControls()
            ' Check for errors
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Do some basic datatype validation (this should be automated in future)

            ' Validate fields
            If Not IsOptionalNum(txtStep, "Step") Then Exit Sub
            If Not IsOptionalNum(txtNextStep, "Next Step") Then Exit Sub
            If Not IsOptionalNum(txtPreviousStep, "Previous Step") Then Exit Sub
            If Not IsOptionalNum(txtOffHoldStep, "Off Hold Step ") Then Exit Sub
            '<Pankaj PN:39534>
            'developer guide no.248
            'start
            nStepVal = gPMFunctions.ToSafeInteger(txtStep.Text)
            If _
                gPMFunctions.ToSafeInteger(txtNextStep.Text) = nStepVal AndAlso _
                gPMFunctions.ToSafeInteger(txtPreviousStep.Text) = nStepVal AndAlso _
                gPMFunctions.ToSafeInteger(txtOffHoldStep.Text) = nStepVal Then
                'end
                MessageBox.Show("Step's order can't be same", "Credit Control Rule Item", MessageBoxButtons.OK, _
                                MessageBoxIcon.Error)
                txtStep.Focus()
                Exit Sub
            End If
            '</Pankaj PN:39534>
            If Not IsOptionalNum(txtElapsedDays, "Elapsed Days ") Then Exit Sub
            If Not IsOptionalNum(txtBrokerDays, "Broker Elapsed Days") Then Exit Sub
            If Not IsOptionalNum(txtBrokerDaysSingleInst, "Broker Elapsed Days(Single Instalment)") Then Exit Sub
            If Not IsOptionalNum(txtPolicyAmt, "Policy Amount") Then Exit Sub
            If Not IsOptionalNum(txtAccountAmt, "Account Amount") Then Exit Sub
            If Not IsOptionalNum(txtAccountAmtSingleInst, "Account Amount(Single Instalment)") Then Exit Sub
            If Not IsOptionalNum(txtRecurringDays, "Recurring Days") Then Exit Sub

            If _
                (m_sBusinessType = ACBusTypeINS) Or (m_sBusinessType = AcBusTypeINSC) Or _
                (m_sBusinessType = AcBusTypeINSH) Then

                If Not IsOptionalNum(txtAutoCancelDoc1Trigger, "Auto Cancellation Document Trigger 1") Then
                    txtAutoCancelDoc1Trigger.Focus()
                    Exit Sub
                End If

                If Not IsOptionalNum(txtAutoCancelDoc2Trigger, "Auto Cancellation Document Trigger 2") Then
                    txtAutoCancelDoc2Trigger.Focus()
                    Exit Sub
                End If

                If _
                    Not _
                    IsOptionalNum(txtOutstandingBalanceWriteOffToleranceAmount, _
                                  "Auto Cancellation Write Off Tolerance Amount") Then
                    txtOutstandingBalanceWriteOffToleranceAmount.Focus()
                    Exit Sub
                End If

                If txtAutoCancelDoc1Trigger.Text = "" Then
                    If cboAutoCancellationDocument1.SelectedIndex > 0 Then
                        MessageBox.Show( _
                            "An Auto Cancellation Document Template cannot be specified unless a valid trigger amount has been provided", _
                            "Auto Cancellation Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        cboAutoCancellationDocument1.Focus()
                        Exit Sub
                    End If
                End If

                If txtAutoCancelDoc2Trigger.Text = "" Then
                    If cboAutoCancellationDocument2.SelectedIndex > 0 Then
                        MessageBox.Show( _
                            "An Auto Cancellation Document Template cannot be specified unless a valid trigger amount has been provided", _
                            "Auto Cancellation Document", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        cboAutoCancellationDocument2.Focus()
                        Exit Sub
                    End If
                End If

                If txtOutstandingBalanceWriteOffToleranceAmount.Text <> "" Then
                    If cboWriteOffReason.ListIndex = 0 Then
                        MessageBox.Show("A write off reason other than (None) must be selected", "Write Off Reason", _
                                        MessageBoxButtons.OK, MessageBoxIcon.Information)
                        cboWriteOffReason.Focus()
                        Exit Sub
                    End If
                End If

            End If

            ' Validate fields

            'Make sure step number not duplicate
            If Not IsValidStepNumber(CInt(txtStep.Text.Trim())) Then
                DisplayMessage(r_lTitleId:=ACInvalidStepNumberTitle, r_lMessageId:=ACInvalidStepNumberDetails, _
                               r_lOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            '<Pankaj PN: 39979 - Make sure if TaskGroup is selected then Task and UserGroup can not be blanked>
            If cboTaskGroup.SelectedIndex > 0 Then
                If cboTask.SelectedIndex = -1 Then
                    MessageBox.Show("A 'Task Group' has been selected, please choose a 'Task'", _
                                    "Credit Control Rule Item", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboTask.Focus()
                    Exit Sub
                End If
            End If
            If cboTaskGroup.SelectedIndex > 0 Then
                If cboUserGroup.SelectedIndex = -1 Then
                    MessageBox.Show("A 'Task Group' has been selected, please choose a 'User Group'", _
                                    "Credit Control Rule Item", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboUserGroup.Focus()
                    Exit Sub
                End If
            End If
            '</Pankaj>

            If cboTask.SelectedIndex >= 0 Then
                m_lTaskId = VB6.GetItemData(cboTask, cboTask.SelectedIndex)
            Else
                m_lTaskId = 0
            End If

            If cboUserGroup.SelectedIndex >= 0 Then
                m_lUserGroupId = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)
            Else
                m_lUserGroupId = 0
            End If

            If cboTaskGroup.SelectedIndex >= 0 Then
                m_lTaskGroupId = VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex)
            Else
                m_lTaskGroupId = 0
            End If

            m_sStepDescription = txtStepDescription.Text

            If _
                (m_sBusinessType = ACBusTypeINS) Or (m_sBusinessType = AcBusTypeINSC) Or _
                (m_sBusinessType = AcBusTypeINSH) Then

                If cboInstalmentFailureCount.SelectedIndex <= 0 Then

                    Me.InstalmentFailureCount = Nothing
                Else
                    Me.InstalmentFailureCount = CInt(cboInstalmentFailureCount.Text)
                End If

                If txtAutoCancelDoc1Trigger.Text <> "" Then
                    Dim dbNumericTemp As Double
                    If _
                        Not _
                        Double.TryParse(txtAutoCancelDoc1Trigger.Text, NumberStyles.Number, _
                                        CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    End If
                End If

                Me.AutoCancelDocumentTemplate1TriggerAmount = gPMFunctions.BlankToNull(txtAutoCancelDoc1Trigger.Text)
                Me.AutoCancelDocumentTemplate2TriggerAmount = gPMFunctions.BlankToNull(txtAutoCancelDoc2Trigger.Text)
                Me.AutoCancelDocumentTemplate1 = VB6.GetItemData(cboAutoCancellationDocument1, _
                                                                 cboAutoCancellationDocument1.SelectedIndex)
                Me.AutoCancelDocumentTemplate2 = VB6.GetItemData(cboAutoCancellationDocument2, _
                                                                 cboAutoCancellationDocument2.SelectedIndex)
                Me.WriteOffToleranceAmount = gPMFunctions.BlankToNull(txtOutstandingBalanceWriteOffToleranceAmount.Text)
                Me.WriteOffReasonId = cboWriteOffReason.ItemData(cboWriteOffReason.ListIndex)
                Me.JumpToNextStepBroker = chkJumptonextstepBroker.CheckState
                Me.JumpToNextStepBrokerSingleInst = chkJumptonextstepBrokerSingleInst.CheckState

            Else


                Me.InstalmentFailureCount = Nothing
                Me.AutoCancelDocumentTemplate1 = 0
                Me.AutoCancelDocumentTemplate2 = 0

                Me.AutoCancelDocumentTemplate1TriggerAmount = Nothing

                Me.AutoCancelDocumentTemplate2TriggerAmount = Nothing

                Me.WriteOffToleranceAmount = Nothing
                Me.WriteOffReasonId = 0
                Me.JumpToNextStepBroker = Nothing
            End If

            ' Set the interface status.
            Status = gPMConstants.PMEReturnCode.PMOK

            Me.Hide()

        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try
    End Sub
    ''' <summary>
    ''' frmStep_Activated
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmStep_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Try

                'Enable either "days" or "Instalments" frame dependant on Business Type

                fraTriggers.Visible = False

                fraAutoCancellationActions.Height = fraWriteOffTolerance.Top

                cboAutoCancellationDocument1.Visible = False
                cboAutoCancellationDocument2.Visible = False
                txtAutoCancelDoc1Trigger.Visible = False
                txtAutoCancelDoc2Trigger.Visible = False
                ssTabBroker.TabPages(1).Visible = False
                Select Case m_sBusinessType
                    Case ACBusTypeINS, AcBusTypeINSH, AcBusTypeINSC

                        cboAutoCancellationDocument1.Visible = True
                        cboAutoCancellationDocument2.Visible = True
                        txtAutoCancelDoc1Trigger.Visible = True
                        txtAutoCancelDoc2Trigger.Visible = True

                        fraTriggers.Visible = True
                        ' fraBrokerBusiness.Visible = True
                        fraTriggers.Left = fraStepOrder.Left
                        fraTriggers.Width = fraStepOrder.Width
                        fraTriggers.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(fraStepOrder.Top) + VB6.PixelsToTwipsY(fraStepOrder.Height) + 75)
                        fraDirectCustomers.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(fraTriggers.Top) + VB6.PixelsToTwipsY(fraTriggers.Height) + 75)
                        ssTabBroker.Top = fraDirectCustomers.Top + fraDirectCustomers.Height - 10
                        fraAutoCancellationActions.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(ssTabBroker.Top) + VB6.PixelsToTwipsY(ssTabBroker.Height) + 140)
                        ' fraAutoCancellationActions.Height = VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraAutoCancellationDocuments.Top) + VB6.PixelsToTwipsY(fraAutoCancellationDocuments.Height) + VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraWriteOffTolerance.Top) + VB6.PixelsToTwipsY(fraWriteOffTolerance.Height)))
                        fraAutoCancellationActions.Height = _
                            VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraAutoCancellationDocuments.Top) + _
                                               VB6.PixelsToTwipsY(fraAutoCancellationDocuments.Height) + 75)
                        fraAutoCancellationDocuments.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(fraWriteOffTolerance.Top) + _
                                VB6.PixelsToTwipsY(fraWriteOffTolerance.Height))
                        fraOther.Top = fraAutoCancellationActions.Top + fraAutoCancellationActions.Height
                        ssTabBroker.TabPages(1).Visible = True

                        fraDirectCustomers.Visible = True
                        'fraBrokerBusiness.Visible = False
                        chkJumpToNextStep.Visible = True
                        fraTolerance.Visible = False

                        fraPrint2.Visible = False
                        fraOther2.Visible = False
                        fraTolerancePercentages.Visible = False

                        'Resize the form accordingly
                        fraDirectCustomers.Height = VB6.TwipsToPixelsY(1100)


                        fraRecurring.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(fraOther.Top) + VB6.PixelsToTwipsY(fraOther.Height) + 75)
                        cmdOK.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(fraRecurring.Top) + VB6.PixelsToTwipsY(fraRecurring.Height) + 100)
                        cmdCancel.Top = cmdOK.Top

                        chkJumpToNextStep.Visible = True
                        lblClientLetter.Left = chkJumpToNextStep.Left
                        lblElapsedDays.Left = VB6.TwipsToPixelsX(2970)
                        lblElapsedDays.Top = chkJumpToNextStep.Top + VB6.TwipsToPixelsY(50)

                        'Developer Guide No. 243
                        'lblElapsedDays.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=530, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                        lblElapsedDays.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=530, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        cboClientLetter.Left = VB6.TwipsToPixelsX(1890)
                        txtElapsedDays.Left = VB6.TwipsToPixelsX(5250)
                        txtElapsedDays.Top = chkJumpToNextStep.Top

                        Me.Height = _
                            VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(cmdOK.Top) + VB6.PixelsToTwipsY(cmdOK.Height) + 550)

                        iPMFunc.CenterForm(Me)

                        'set caption
                        If Status = gPMConstants.PMEComponentAction.PMAdd Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        ElseIf Status = gPMConstants.PMEComponentAction.PMEdit Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        End If

                    Case ACBusTypeCLF, ACBusTypeCLD

                        fraDirectCustomers.Visible = True
                        '  fraBrokerBusiness.Visible = True
                        chkJumpToNextStep.Visible = False
                        ssTabBroker.Visible = True
                        fraTolerance.Visible = False
                        fraPrint2.Visible = False
                        fraOther2.Visible = False
                        fraTolerancePercentages.Visible = False

                        lblBroketrDays.Left = chkJumptonextstepBroker.Left
                        txtBrokerDays.Left = lblBroketrDays.Width + 300
                        lblBrokerDaysSingleInst.Left = chkJumptonextstepBrokerSingleInst.Left
                        txtBrokerDaysSingleInst.Left = lblBrokerDaysSingleInst.Width + 300

                        chkJumptonextstepBroker.Visible = False
                        chkJumptonextstepBrokerSingleInst.Visible = False

                        'set caption
                        If Status = gPMConstants.PMEComponentAction.PMAdd Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        ElseIf Status = gPMConstants.PMEComponentAction.PMEdit Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        End If

                    Case ACBusTypeCIN, ACBusTypeCIA

                        fraDirectCustomers.Visible = True
                        'fraBrokerBusiness.Visible = True

                        fraTolerance.Visible = False
                        fraPrint2.Visible = True
                        fraOther2.Visible = True
                        fraTolerancePercentages.Visible = True
                        ssTabBroker.Visible = True

                        lblBroketrDays.Left = chkJumptonextstepBroker.Left
                        txtBrokerDays.Left = lblBroketrDays.Width + 300
                        chkJumptonextstepBroker.Visible = False

                        lblBrokerDaysSingleInst.Left = chkJumptonextstepBrokerSingleInst.Left
                        txtBrokerDaysSingleInst.Left = lblBrokerDaysSingleInst.Width + 300
                        chkJumptonextstepBrokerSingleInst.Visible = False

                        'set caption
                        If Status = gPMConstants.PMEComponentAction.PMAdd Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        ElseIf Status = gPMConstants.PMEComponentAction.PMEdit Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        End If
                    Case ACBusTypeTRANS

                        fraDirectCustomers.Visible = True
                        'fraBrokerBusines.Visible = True
                        fraTolerance.Visible = True

                        fraPrint2.Visible = False
                        fraOther2.Visible = False
                        fraTolerancePercentages.Visible = False
                        ssTabBroker.Visible = True
                        lblElapsedDays.Tag = "CAP;509"
                        chkJumpToNextStep.Visible = False
                        chkRunAutoCancel.Enabled = False
                        chkCheckAutoCancel.Enabled = False

                        chkRunAutoCancel.CheckState = CheckState.Unchecked
                        chkCheckAutoCancel.CheckState = CheckState.Unchecked
                        lblBroketrDays.Left = chkJumptonextstepBroker.Left
                        txtBrokerDays.Left = lblBroketrDays.Width + 300
                        chkJumptonextstepBroker.Visible = False
                        lblBrokerDaysSingleInst.Left = chkJumptonextstepBrokerSingleInst.Left
                        txtBrokerDaysSingleInst.Left = lblBrokerDaysSingleInst.Width + 300
                        chkJumptonextstepBrokerSingleInst.Visible = False

                        'set caption
                        If Status = gPMConstants.PMEComponentAction.PMAdd Then

                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        ElseIf Status = gPMConstants.PMEComponentAction.PMEdit Then

                            If m_bCaptionRenWTGUpdate Then

                                'Developer Guide No. 243
                                'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditRENWTGUpdateTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditRENWTGUpdateTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            Else

                                'Developer Guide No. 243
                                'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            End If
                        End If

                    Case Else

                        fraDirectCustomers.Visible = True
                        'fraBrokerBusines.Visible = True
                        fraTolerance.Visible = True
                        ssTabBroker.Visible = True
                        fraPrint2.Visible = False
                        fraOther2.Visible = False
                        fraTolerancePercentages.Visible = False

                        lblElapsedDays.Tag = "CAP;509"
                        chkJumpToNextStep.Visible = False
                        lblBroketrDays.Left = chkJumptonextstepBroker.Left
                        txtBrokerDays.Left = lblBroketrDays.Left + lblBroketrDays.Width
                        chkJumptonextstepBroker.Visible = False

                        lblBrokerDaysSingleInst.Left = chkJumptonextstepBrokerSingleInst.Left
                        txtBrokerDaysSingleInst.Left = lblBrokerDaysSingleInst.Left + lblBrokerDaysSingleInst.Width
                        chkJumptonextstepBrokerSingleInst.Visible = False

                        fraAutoCancellationActions.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(ssTabBroker.Top) + VB6.PixelsToTwipsY(ssTabBroker.Height))
                        fraOther.Top = _
                            VB6.TwipsToPixelsY( _
                                VB6.PixelsToTwipsY(fraAutoCancellationActions.Top) + _
                                VB6.PixelsToTwipsY(fraAutoCancellationActions.Height))
                        fraRecurring.Top = _
                            VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(fraOther.Top) + VB6.PixelsToTwipsY(fraOther.Height))
                        'set caption
                        If Status = gPMConstants.PMEComponentAction.PMAdd Then

                            'Developer Guide No. 243
                            'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                        ElseIf Status = gPMConstants.PMEComponentAction.PMEdit Then

                            If m_bCaptionRenWTGUpdate Then

                                'Developer Guide No. 243
                                'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditRENWTGUpdateTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACEditRENWTGUpdateTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            Else

                                'Developer Guide No. 243
                                'Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString,bResFile:= My.Resources.ResourceManager))
                                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lID:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                            End If
                        End If

                End Select


                m_lReturn = SetInterfaceDefaults()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                Exit Sub

            Catch excep As System.Exception


                ' Error Section.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Try
            cboClientLetter.SelectedIndex = -1
            Status = gPMConstants.PMEReturnCode.PMCancel



        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub


    Private Sub frmStep_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sAgencyOrUnderwriting)

            ' Display all language specific captions.
            'developer guide no.243
            m_lReturn = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If





            '    If ToSafeInteger(m_vInstalmentFailureCount, -1) = -1 Then
            '        cboInstalmentFailureCount.Text = ""
            '    Else
            '        cboInstalmentFailureCount.Text = m_vInstalmentFailureCount
            '    End If


            'Set formfields object

            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)



            'Set and display the document dropdowns
            'Commented the code as it is not required here.
            'm_lReturn = ShowDocumentLists()


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            cboWriteOffReason.FirstItem = ""
        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmStep_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        'Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        m_oFormfields.Dispose()
        m_oFormfields = Nothing

        eventArgs.Cancel = Cancel <> 0
    End Sub
    ''' <summary>
    ''' Get list of Document_Template records
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    
    Public Function ShowDocumentLists() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ShowDocumentLists
        ' PURPOSE: Get list of Document_Template records
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim vArray, vNewArray As Object
        'Dim lClientDocId As Long
        Dim lClientListIndex As Integer
        'Dim lOIPDocId As Long
        Dim lOIPListIndex As Integer


        Try

        result = gPMConstants.PMEReturnCode.PMTrue
            'what if no documents yet populated
                If Not Information.IsArray(m_vDocTemplateList) Then
                    cboOIPLetter.Items.Clear()
                    cboClientLetter.Items.Clear()
                    cboOIPLetter2.Items.Clear()
                    cboClientLetter2.Items.Clear()
                    cboBrokerLetter.Items.Clear()
                    cboBrokerLetterSingleInst.Items.Clear()
                    Return result
                End If

                'Populate the dropdowns
                cboClientLetter.Items.Clear()
                cboOIPLetter.Items.Clear()
                cboClientLetter2.Items.Clear()
                cboOIPLetter2.Items.Clear()
                cboBrokerLetter.Items.Clear()
                cboAutoCancellationDocument1.Items.Clear()
                cboAutoCancellationDocument2.Items.Clear()
                cboBrokerLetterSingleInst.Items.Clear()

                For lLoop As Integer = 0 To m_vDocTemplateList.GetUpperBound(1)

                    cboClientLetter.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))

                    cboOIPLetter.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))

                    cboClientLetter2.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))
                    cboBrokerLetter.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))
                    cboOIPLetter2.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))



                cboAutoCancellationDocument1.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))

                cboAutoCancellationDocument2.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))

                    VB6.SetItemData(cboAutoCancellationDocument1, cboAutoCancellationDocument1.Items.Count - 1, CInt(m_vDocTemplateList(1, lLoop)))

                    VB6.SetItemData(cboAutoCancellationDocument2, cboAutoCancellationDocument2.Items.Count - 1, CInt(m_vDocTemplateList(1, lLoop)))

            Next lLoop
        Catch ex As Exception
            ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumentLists", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

        Finally

        End Try

        Return result


    End Function


    Private Function IsValidStepNumber(ByVal v_iStepNumber As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: IsValidStepNumber
        ' PURPOSE: Ensure that a Step Number is not duplicated
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lUserGroupId, lCompanyId As Integer


        Try

            result = True

            'If no items in listview - then no conflict
            If ofrmDetails.lvwSteps.Items.Count = 0 Then
                Return result
            End If

            'If the StepID is missing, then it is a new step being added
            If m_lStepID = 0 And Me.Status <> gPMConstants.PMEComponentAction.PMEdit Then

                'Loop through the listview
                For Each oListItem As ListViewItem In ofrmDetails.lvwSteps.Items

                    'Test for a match on step number only
                    With oListItem

                        If Conversion.Val(.Text) = v_iStepNumber Then

                            'If adding an item - error


                            'if editing an item - no error


                            'There is a match so an entry already exists
                            'therefore this selection is invalid
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                Next oListItem

            Else
                'there should only be one row, which is the currently selected item
                'Loop through the listview

                For Each oListItem As ListViewItem In ofrmDetails.lvwSteps.Items
                    'Test for a match on UserGroupId and CompanyId
                    With oListItem
                        If Conversion.Val(.Text) = v_iStepNumber And Conversion.Val(ListViewHelper.GetListViewSubItem(oListItem, 7).Text) <> m_lStepID Then

                            'There is a match so an entry already exists
                            'thereofre this selection is invalid
                            Return gPMConstants.PMEReturnCode.PMFalse

                        End If
                    End With
                Next oListItem

            End If



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValidStepNumber", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    Private Function GetComboDocuMatch(ByVal v_lDocID As Integer, ByRef r_lListindex As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetComboDocuMatch
        ' PURPOSE: Return the position of a match for a document ID from
        '          m_vDocTemplateList array
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim lUserGroupId, lCompanyId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If no items in listview - then nothing to do
            If Not IsArray(m_vDocTemplateList) Then
                r_lListindex = 0
                Return result
            End If

            'Go through the array and find a match for the supplied ID

            For lLoop As Integer = 0 To m_vDocTemplateList.GetUpperBound(1)

                If CInt(m_vDocTemplateList(1, lLoop)) = v_lDocID Then

                    r_lListindex = CInt(m_vDocTemplateList(0, lLoop))
                    Return result
                End If
            Next lLoop

            'We should have a match by now - this code should not execute

            result = False
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find supplied document Id in dropdown of available documents", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDocumentMatch")



            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------



        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDocuMatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse


            End Select

        Finally


        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PopulateTaskCbo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function PopulateTaskCbo(ByVal v_lTaskGroupId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateTaskCbo"

        Dim llBound, lUBound, lTaskId, lTaskGroupId As Integer
        Dim sTaskDescription As String = ""
        Dim lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' initialisation
            cboTask.Items.Clear()

            lIndex = 0

            ' if we have a selected task group
            If cboTaskGroup.Text <> "" Then

                ' if we have an array of task group tasks
                If IsArray(m_vTaskGroupTask) Then

                    ' get array boundaries
                    llBound = m_vTaskGroupTask.GetLowerBound(1)
                    lUBound = m_vTaskGroupTask.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = llBound To lUBound

                        ' get item details
                        lTaskGroupId = CInt(m_vTaskGroupTask(ACTaskGroupTaskGroupId, lItem))
                        lTaskId = CInt(m_vTaskGroupTask(ACTaskgroupTaskId, lItem))
                        sTaskDescription = CStr(m_vTaskGroupTask(ACTaskGroupTaskDescription, lItem))

                        ' if task group matches the selected task group
                        If lTaskGroupId = v_lTaskGroupId Then

                            ' add task to combo
                            cboTask.Items.Insert(lIndex, sTaskDescription)
                            VB6.SetItemData(cboTask, lIndex, lTaskId)

                            lIndex += 1

                        End If

                    Next lItem

                End If

            End If

            cboTask.Enabled = Not (lIndex = 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PopulateUserGroupscbo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function PopulateUserGroupscbo(ByVal v_lTaskGroupId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateUserGroupscbo"

        Dim lIndex, llBound, lUBound, lTaskGroupId, lUserGroupId As Integer
        Dim sUserGroup As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboUserGroup.Items.Clear()

            lIndex = 0

            ' if we have a selected task group
            If cboTaskGroup.Text <> "" Then

                ' if we have an array
                If IsArray(m_vTaskGroupUserGroups) Then

                    ' get array boundaries
                    llBound = m_vTaskGroupUserGroups.GetLowerBound(1)
                    lUBound = m_vTaskGroupUserGroups.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = llBound To lUBound

                        ' get item details
                        lTaskGroupId = CInt(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_TaskGroupId, lItem))
                        lUserGroupId = CInt(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupId, lItem))
                        sUserGroup = CStr(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupDescription, lItem))

                        ' if task group matches the selected task group
                        If lTaskGroupId = v_lTaskGroupId Then

                            ' add user group to combo
                            cboUserGroup.Items.Insert(lIndex, sUserGroup)
                            VB6.SetItemData(cboUserGroup, lIndex, lUserGroupId)
                            lIndex += 1

                        End If

                    Next lItem

                End If

            End If

            cboUserGroup.Enabled = Not (lIndex = 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '               Selects the specified row
    ' ***************************************************************** '

    Private Function GetLookupDetails(ByVal v_sLookupTable As String, ByRef r_octlLookup As ComboBox, Optional ByVal v_lSelectedItemId As Integer = 0, Optional ByVal v_bAddBlankEntry As Boolean = False) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookupDetails"

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        ' Const ACValueID As Integer = 1
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim lItemIndex, lItemFoundIndex, lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            r_octlLookup.Items.Clear()

            bFoundMatch = False

            For lRow = m_vLookupTables.GetLowerBound(1) To m_vLookupTables.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupTables(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & v_sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            lItemIndex = -1
            lItemFoundIndex = -1

            If v_bAddBlankEntry Then
                r_octlLookup.Items.Insert(0, "")
                lIndex = 1
            Else
                lIndex = 0
            End If

            For lCntr As Integer = CInt(m_vLookupTables(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupTables(ACValueStartPos, lRow)) + CDbl(m_vLookupTables(ACValueNumber, lRow))) - 1)

                r_octlLookup.Items.Add(CStr(m_vLookupDetails(ACDetailDesc, lCntr)))
                VB6.SetItemData(r_octlLookup, lIndex, CInt(m_vLookupDetails(ACDetailKey, lCntr)))


                If CDbl(m_vLookupDetails(ACDetailKey, lCntr)) = v_lSelectedItemId Then
                    lItemFoundIndex = lItemIndex
                End If

                Debug.WriteLine(VB6.GetItemString(r_octlLookup, lIndex) & ":" & CStr(lIndex))

                lIndex += 1
                lItemIndex += 1

            Next lCntr

            ' set the item we want to display in the list
            If lItemFoundIndex <> -1 Then
                r_octlLookup.SelectedIndex = lItemFoundIndex
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedItemId", v_lSelectedItemId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************
            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-06-2003 : workflow
    ' ***************************************************************** '
    Private Function PopulateLookups() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateLookups"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Lookup Values

            ' Task Group
            m_lReturn = GetLookupDetails(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_octlLookup:=cboTaskGroup, v_bAddBlankEntry:=True)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupItem
    '
    ' Parameters: n/a
    '
    ' Description: Returns the code for a specifed item description
    '                  in a specified lookup table..
    '
    ' History:
    '           Created : MEvans : 06-06-2003 : 223
    ' ***************************************************************** '
    Private Function GetLookupItem(ByVal v_sLookupTable As String, ByVal r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookupItem"

        Dim lRow As Integer
        Dim bFoundMatch As Boolean


        Dim llBound, lUBound As Integer
        Dim v_vLookupItem As String = ""
        Dim lLookupItem As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lookup value contants.
            Const ACValueTableName As Integer = 0
            '  Const ACValueID As Integer = 1
            Const ACValueStartPos As Integer = 2
            Const ACValueNumber As Integer = 3

            Const ACDetailKey As Integer = 0
            Const ACDetailDesc As Integer = 1
            Const ACDetailCode As Integer = 2

            ' Initilisation
            bFoundMatch = False

            For lRow = m_vLookupTables.GetLowerBound(1) To m_vLookupTables.GetUpperBound(1)

                ' Check for a match of the table name.
                If CStr(m_vLookupTables(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
                    bFoundMatch = True
                    Exit For
                End If

            Next lRow

            If bFoundMatch Then
                ' get array boundaries for specified table
                llBound = CInt(m_vLookupTables(ACValueStartPos, lRow))

                lUBound = CInt((CDbl(m_vLookupTables(ACValueStartPos, lRow)) + CDbl(m_vLookupTables(ACValueNumber, lRow))) - 1)


                ' set lookup properties
                If r_lItemId <> 0 Then
                    v_vLookupItem = CStr(r_lItemId)
                    lLookupItem = 0

                ElseIf r_sItemDesc <> "" Then
                    v_vLookupItem = r_sItemDesc
                    lLookupItem = 1

                ElseIf r_sItemCode <> "" Then
                    v_vLookupItem = r_sItemCode
                    lLookupItem = 2
                End If

                ' loop around the available items for the specified table
                For lCntr As Integer = llBound To lUBound

                    ' get the code for the specified lookup items key
                    If CStr(m_vLookupDetails(lLookupItem, lCntr)).Trim() = v_vLookupItem Then

                        ' return the requested code, id, description
                        r_sItemDesc = CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim()
                        r_sItemCode = CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim()
                        r_lItemId = CInt(CStr(m_vLookupDetails(ACDetailKey, lCntr)).Trim())

                        Exit For
                    End If

                Next lCntr

            End If

            ' if we dont find the code then log an error
            If r_sItemCode = "" Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_sItemCode", r_sItemCode)
                oDict.Add("r_lItemId", r_lItemId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find code for lookuptable:" & v_sLookupTable & "and lookup Item:" & v_vLookupItem, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
                '*******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_sItemCode", r_sItemCode)
            oDict.Add("r_lItemId", r_lItemId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupTaskCombos
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-07-2003 : workflow
    ' ***************************************************************** '
    Private Function SetupTaskCombos() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetupTaskCombos"

        Dim lTaskGroupId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get task group id
            If cboTaskGroup.Text <> "" Then
                m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_sItemDesc:=cboTaskGroup.Text, r_sItemCode:="", r_lItemId:=lTaskGroupId)
            End If

            ' if a new item has been selected or onload no item has yet been selected
            If m_sPrevTaskGroup <> cboTaskGroup.Text Or m_sPrevTaskGroup = "" Then

                If cboTaskGroup.Text <> "" Then

                    ' populate task
                    If PopulateTaskCbo(v_lTaskGroupId:=lTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' populate user groups
                    If PopulateUserGroupscbo(v_lTaskGroupId:=lTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else

                    ' clear down any selected task and user group

                    ' user group clear
                    cboUserGroup.Items.Clear()
                    cboUserGroup.Enabled = False

                    ' task clear
                    cboTask.Items.Clear()
                    cboTask.Enabled = False

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 14-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Public Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lListIndex As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If m_sBusinessType = ACBusTypeRENUPD Then

                With chkCheckAutoCancel
                    .Visible = False
                    .CheckState = CheckState.Unchecked
                End With

                With chkRunAutoCancel
                    .Visible = False
                    .CheckState = CheckState.Unchecked
                End With

                chkAutoLapseRenewal.Visible = True

            Else

                chkCheckAutoCancel.Visible = True
                chkRunAutoCancel.Visible = True
                chkAutoLapseRenewal.Visible = False

            End If

            If (m_sBusinessType = ACBusTypeINS) Or (m_sBusinessType = AcBusTypeINSC) Or (m_sBusinessType = AcBusTypeINSH) Then

                ' populate the available values for instalment failure count
                PopulateInstalmentFailureCountCombo()

            End If

            If Status = gPMConstants.PMEComponentAction.PMAdd Then

                m_sPrevTaskGroup = ""

                ' set interface defaults..
                txtStepDescription.Text = "Manual Debt Follow Up"

                ' populate lookup combos
                lReturn = PopulateLookups()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = SetupTaskCombos()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupTaskCombos Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                txtOutstandingBalanceWriteOffToleranceAmount.Text = ""
                If cboAutoCancellationDocument1.Items.Count > 0 Then
                    cboAutoCancellationDocument1.SelectedIndex = 0
                    cboAutoCancellationDocument2.SelectedIndex = 0
                End If
                txtAutoCancelDoc1Trigger.Text = ""
                txtAutoCancelDoc2Trigger.Text = ""

            Else

                ' populate lookup combos
                m_lReturn = PopulateLookups()

                ' set interface defaults..
                txtStepDescription.Text = m_sStepDescription

                ' populate task lookup cbo's
                lReturn = CType(SelectcboItem(cboTaskGroup, m_lTaskGroupId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboTaskGroup", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' populate task lookup cbo's
                If gPMFunctions.ToSafeLong(m_vInstalmentFailureCount, 0) <> 0 Then
                    lReturn = CType(SelectcboItem(cboInstalmentFailureCount, m_vInstalmentFailureCount), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboInstalmentFailureCount", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                ' verify that any saved data has been correctly transposed to the screen
                ' if not raise a message box stating that the individual item is no longer
                ' available....
                If m_lTaskGroupId And cboTaskGroup.SelectedIndex = -1 Then
                    MessageBox.Show("The Task Group originally specified for the step is no longer available. TaskGroupId=" & m_lTaskGroupId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_lTaskGroupId = 0
                Else
                    If m_lTaskGroupId <> VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex) Then
                        MessageBox.Show("The Task Group originally specified for the step is no longer available. Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        m_lTaskGroupId = 0
                    End If
                End If

                If m_lTaskGroupId <> 0 Then

                    lReturn = CType(SelectcboItem(cboTask, m_lTaskId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboTask", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    lReturn = CType(SelectcboItem(cboUserGroup, m_lUserGroupId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboUserGroup", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If cboTask.SelectedIndex = -1 And m_lTaskId > 0 Then
                        MessageBox.Show("The Task originally specified for the step is no longer available. TaskId=" & m_lTaskId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Else
                        If m_lTaskId <> VB6.GetItemData(cboTask, cboTask.SelectedIndex) Then
                            MessageBox.Show("The Task originally specified for the step is no longer available. TaskId=" & m_lTaskId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If

                    If cboUserGroup.SelectedIndex = -1 And m_lUserGroupId > 0 Then
                        MessageBox.Show("The User Group originally specified for the step is no longer available. UserGroupId=" & m_lUserGroupId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    ElseIf cboUserGroup.SelectedIndex >= 0 Then
                        If m_lUserGroupId <> VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex) Then
                            MessageBox.Show("The User Group originally specified for the step is no longer available. UserGroupId=" & m_lUserGroupId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        End If
                    End If

                End If


                If (m_sBusinessType = ACBusTypeINS) Or (m_sBusinessType = AcBusTypeINSC) Or (m_sBusinessType = AcBusTypeINSH) Then

                    txtAutoCancelDoc1Trigger.Text = m_vAutoCancelDocumentTemplate1TriggerAmount
                    txtAutoCancelDoc2Trigger.Text = m_vAutoCancelDocumentTemplate2TriggerAmount
                    txtOutstandingBalanceWriteOffToleranceAmount.Text = m_vWriteOffToleranceAmount

                    If m_lAutoCancelDocumentTemplate1 <> 0 Then
                        GetComboDocuMatch(m_lAutoCancelDocumentTemplate1, lListIndex)
                    Else
                        lListIndex = 0
                    End If

                    cboAutoCancellationDocument1.SelectedIndex = lListIndex

                    If m_lAutoCancelDocumentTemplate2 <> 0 Then
                        GetComboDocuMatch(m_lAutoCancelDocumentTemplate2, lListIndex)
                    Else
                        lListIndex = 0
                    End If

                    cboAutoCancellationDocument2.SelectedIndex = lListIndex

                    cboWriteOffReason.ItemId = m_lWriteOffReasonId

                End If

            End If


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

    ' ***************************************************************** '
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 03-10-2003 : 229
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count - 1
                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

            End If

            If bItemNotFound Then

                ' log that we havent found the specified item
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSelectedId", v_lSelectedId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedId", v_lSelectedId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PopulateInstalmentFailureCountCombo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2007 : ADDACS Phase II
    ' ***************************************************************** '
    Public Sub PopulateInstalmentFailureCountCombo()

        Const kMethodName As String = "PopulateInstalmentFailureCountCombo"

        Dim lReturn, lSubValue, lInstalmentFailureCount As Integer

        Try



            ' get the current instalment failure count
            lInstalmentFailureCount = gPMFunctions.ToSafeInteger(m_vInstalmentFailureCount, 0)

            cboInstalmentFailureCount.Items.Clear()

            ' populate instalment failure reasons
            cboInstalmentFailureCount.Items.Add("N/A")
            VB6.SetItemData(cboInstalmentFailureCount, cboInstalmentFailureCount.Items.Count - 1, -1)

            ' add the next available item
            If lInstalmentFailureCount <> 0 Then

                If m_lNextAvailableInstalmentFailureCount < lInstalmentFailureCount Then
                    cboInstalmentFailureCount.Items.Add(CStr(m_lNextAvailableInstalmentFailureCount))
                    VB6.SetItemData(cboInstalmentFailureCount, cboInstalmentFailureCount.Items.Count - 1, m_lNextAvailableInstalmentFailureCount)
                End If

                cboInstalmentFailureCount.Items.Add(CStr(lInstalmentFailureCount))
                VB6.SetItemData(cboInstalmentFailureCount, cboInstalmentFailureCount.Items.Count - 1, lInstalmentFailureCount)

            Else

                cboInstalmentFailureCount.Items.Add(CStr(m_lNextAvailableInstalmentFailureCount))
                VB6.SetItemData(cboInstalmentFailureCount, cboInstalmentFailureCount.Items.Count - 1, m_lNextAvailableInstalmentFailureCount)

            End If

            cboInstalmentFailureCount.SelectedIndex = 0


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lSubValue, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (Text1_Change) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Text1_Change()
    '
    'End Sub

    Private Sub txtAccountAmt_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtAccountAmt.Leave

        Dim cAccountAmount As Decimal
        Try
            ' If the textbox has a value
            If txtAccountAmt.Text.Trim() <> "" Then


                ' If the value is non numeric show an appropriate message
                Dim dbNumericTemp As Double
                If Not Double.TryParse(txtAccountAmt.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    MessageBox.Show("Only numeric value is allowed in this field. Please re-enter.", "Tolerance Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    txtAccountAmt.Focus()
                    Exit Sub
                End If

                ' Try to convert the user entered value to currency data type
                cAccountAmount = CDec(txtAccountAmt.Text)
            End If
        Catch ex As Exception
            ' Show message if value cannot be converted to currency.
            MessageBox.Show("The value in this field is not a valid currency amount. Please re-enter.", "Tolerance Amount", MessageBoxButtons.OK, MessageBoxIcon.Error)
            txtAccountAmt.Focus()

        Finally


        End Try
    End Sub
End Class