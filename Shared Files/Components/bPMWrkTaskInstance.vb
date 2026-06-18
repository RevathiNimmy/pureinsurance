Option Strict Off
Option Explicit On
Imports System
<System.Runtime.InteropServices.ProgId("bPMWrkTaskInstance_NET.bPMWrkTaskInstance")> _
Public Module bPMWrkTaskInstance

    Public Enum LockName As Long
        InsuranceFolderCnt = 1
        InsuranceFileCnt = 2
        PartyCnt = 3
        ClaimId = 4
        TaskInstanceCnt = 5
        UserGroupCnt = 6
        TaskGroupCnt = 7
        CoverNoteBookId = 8
        BGId = 9
        RenewalProcess = 10
        TransDetailKey = 11
        CashListItemID = 12
        ClaimPaymentCnt = 13
        RiskKey = 14
        PartyBankKey = 15
        CashDepositKey = 15
        NextCashDepositNumber = 16
        InvalidValue = 17 'Use it as an in valid value place holder. Do not add it in LockNameString
    End Enum


    Public ReadOnly Property LockNameString(ByVal LockName As LockName) As String
        Get
            Select Case LockName
                Case LockName.InsuranceFileCnt
                    Return "insurance_file_cnt"
                Case LockName.InsuranceFolderCnt
                    Return "insurance_folder_cnt"
                Case LockName.PartyCnt
                    Return "party_cnt"
                Case LockName.ClaimId
                    Return "claim_id"
                Case LockName.TaskInstanceCnt
                    Return "pmwrk_task_instance_cnt"

                Case LockName.UserGroupCnt
                    Return "pmuser_group_id"

                Case LockName.TaskGroupCnt
                    Return "pmwrk_task_group_id"

                Case LockName.CoverNoteBookId
                    Return "cover_note_book_id"

                Case LockName.BGId
                    Return "bg_id"

                Case LockName.RenewalProcess
                    Return "renewal_status_cnt"

                Case LockName.TransDetailKey
                    Return "transdetail_id"

                Case LockName.CashListItemID
                    Return "cashlistitem_id"

                Case LockName.ClaimPaymentCnt
                    Return "claim_payment_id"

                Case LockName.RiskKey
                    Return "risk_cnt"

                Case LockName.CashDepositKey
                    Return "cashdeposit_id"

                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property


    Public ReadOnly Property LockNameStringToEnum(ByVal LockNameString As String) As LockName
        Get
            Select Case LockNameString
                Case "insurance_file_cnt"
                    Return LockName.InsuranceFileCnt

                Case "insurance_folder_cnt"
                    Return LockName.InsuranceFolderCnt

                Case "party_cnt"
                    Return LockName.PartyCnt

                Case "claim_id"
                    Return LockName.ClaimId

                Case "pmwrk_task_instance_cnt"
                    Return LockName.TaskInstanceCnt

                Case "pmuser_group_id"
                    Return LockName.UserGroupCnt

                Case "pmwrk_task_group_id"
                    Return LockName.TaskGroupCnt

                Case "cover_note_book_id"
                    Return LockName.CoverNoteBookId

                Case "bg_id"
                    Return LockName.BGId

                Case "renewal_status_cnt"
                    Return LockName.RenewalProcess

                Case "transdetail_id"
                    Return LockName.TransDetailKey

                Case "cashlistitem_id"
                    Return LockName.CashListItemID

                Case "claim_payment_id"
                    Return LockName.ClaimPaymentCnt

                Case "risk_cnt"
                    Return LockName.RiskKey

                Case "cashdeposit_id"
                    Return LockName.CashDepositKey

                Case Else
                    Return LockName.InvalidValue
            End Select
        End Get
    End Property



End Module