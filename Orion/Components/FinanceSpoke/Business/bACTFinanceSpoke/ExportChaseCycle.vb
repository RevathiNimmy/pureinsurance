Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Friend NotInheritable Class ExportChaseCycle
    '************************************************************************
    ' Class/Module: ExportChaseCycle
    '
    ' Description : Prints Chase Cycle letters and reports for all Chase Cycle Items for a branch
    '
    '************************************************************************


    ' ************************************************
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_iBatchID As Integer
    Private m_nTotalRecords As Integer = 0
    Protected Const kBatchStatusComplete As String = "C"
    Protected Const kBatchStatusFailed As String = "F"
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportChaseCycle"

    ' Private variables
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database

    ' Constants for the HeaderData array
    Private Const kbHDBranch As Byte = 9
    Private Const kbHDAsOfDate As Byte = 10
    Private Const kbHDSpoolDoc As Byte = 11
    Private Const kbHDArchiveDoc As Byte = 12

    ' Result Array columns for Chase Cycle Item
    Private Const kbCCItemID As Byte = 0
    Private Const kbCCReason As Byte = 1
    Private Const kbCCInsuranceFolderCnt As Byte = 2
    Private Const kbCCInsuranceFileCnt As Byte = 3
    Private Const kbCCCanAutoCancel As Byte = 4
    Private Const kbCCWillAutoCancel As Byte = 5
    Private Const kbCCStepID As Byte = 6
    Private Const kbCCCreatedDate As Byte = 7
    Private Const kbCCDueDate As Byte = 8
    Private Const kbCCLetterSent As Byte = 9
    Private Const kbCCAutoCancelPolicy As Byte = 10
    Private Const kbCCCheckAutoCancel As Byte = 11
    Private Const kbCCPMUserGroupID As Byte = 12
    Private Const kbCCPMWrkTaskID As Byte = 13
    Private Const kbCCNumberOfDays As Byte = 14
    Private Const kbCCNextStepID As Byte = 15
    Private Const kbCCpmuser_group_id As Byte = 17
    Private Const kbCCpmuser_id As Byte = 18
    Private Const kbCCis_deleted As Byte = 19
    Private Const kbCCItemUpdated As Byte = 20
    Private Const kbCCDocumentTemplateCode As Integer = 21
    Private Const kbCCDocumentTemplateId As Integer = 22
    Private Const kbCCStepPMWrkTaskGroupId As Integer = 23
    Private Const kbCCStepDescription As Integer = 24
    Private Const kbCCCustomerName As Byte = 25


    ' Component Object variables
    Private m_oChaseCycle As bSIRChaseCycle.Business
    Private m_oChaseCycleItem As bSIRChaseCycle.ChaseCycleItem
    Private m_oAccount As Object

    Private m_oRenewal As Object

    Private m_dAsOfDate As Date
    Private m_bSpoolDoc As Boolean
    Private m_bArchiveDoc As Boolean
    Private m_sUnderwritingOrAgency As String = ""

    ' Stored procedures
    Private Const ksSPExportChaseCycleSQL As String = "spu_ACT_Spoke_ExportChaseCycle"
    Private Const ksSPExportChaseCycleName As String = "GetExportChaseCycle"
    Private Const ksSPExportChaseCycleStored As Boolean = True


    Friend WriteOnly Property Business() As Business
        Set(ByVal Value As Business)

            m_oBusiness = Value

        End Set
    End Property
    Friend WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    Friend Function PassThroughLogin(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PassThroughLogin
        ' PURPOSE: Pass through the module level login information to the Class.
        ' This is for COM+. Normally a business class will not require this but the Spoke
        ' design means that Classes are instantiated by the Business class and can
        ' no longer rely on global variables.
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PassThroughLogin", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select
        End Try

        Return result


    End Function


    '*************************************************************************
    ' Name: GetChaseCycleItems
    '
    ' Description: This function loops through all Chase Cycle items
    '              for the passed Branch and processes each, and produces
    '              Chase Cycle reports at the end. Unlike other Export
    '              classes, this one does not return any records, so a local
    '              variant array is declared to store the returned records
    '              from the initial select stored procedure.
    '*************************************************************************

    Private Function GetChaseCycleItems(ByVal v_sBranchCode As String) As Integer

        Dim result As Integer = 0
        Const k_sFunctionName As String = "GetChaseCycleItems"

        Dim lClientItemsArray() As Object, lDeleteItemsArray() As Object, lAutoCancelItemsArray() As Object
        Dim lClientItems As Integer
        Dim lDeleteItems As Integer
        Dim vResultArray, vValue As Object
        Dim lAutoCancelItems As Integer
        Dim bDeletePermanent As Boolean

        Dim bProcessItem As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResult1, vResult2, vResult3 As Object
        '**************



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add branch_code as an input param
        If m_oDatabase.Parameters.Add(sName:="branch_code", vValue:=v_sBranchCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Execute SQL Statement to get Chase Cycle Items
        'spu_ACT_Spoke_ExportChaseCycle
        If m_oDatabase.SQLSelect(sSQL:=ksSPExportChaseCycleSQL, sSQLName:=ksSPExportChaseCycleName, bStoredProcedure:=ksSPExportChaseCycleStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        ' Process only if something returned
        If Information.IsArray(vResultArray) Then

            ' Create an instance of the Chase Cycle object
            m_oChaseCycle = New bSIRChaseCycle.Business
            If m_oChaseCycle.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Create an instance of the Chase Cycle Item object
            m_oChaseCycleItem = New bSIRChaseCycle.ChaseCycleItem
            If m_oChaseCycleItem.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="getUnderwritingOrAgency call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleItems")
                Return result
            End If

            ' Loop through all Chase Cycle Items found
            For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                If ProcessChaseCycleItem(r_vItemArray:=vResultArray, i:=i, r_vClientItemsArray:=lClientItemsArray, r_lClientItems:=lClientItems, r_vAutoCancelItemsArray:=lAutoCancelItemsArray, r_lAutoCancelItems:=lAutoCancelItems, r_vDeleteItemsArray:=lDeleteItemsArray, r_lDeleteItems:=lDeleteItems) <> gPMConstants.PMEReturnCode.PMTrue Then

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="ProcessDebt call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleItems")
                    Return result

                End If
            Next

            ' Produce client letters, if necessary
            If lClientItems > 0 Then

                If m_oChaseCycle.ProduceClientLetters(v_vChaseCycleItems:=lClientItemsArray, v_bSpoolDocuments:=m_bSpoolDoc, v_bArchiveDocuments:=m_bArchiveDoc) <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oChaseCycle.ProduceClientLetters call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleItems")

                    Return result
                End If
            End If

            ' Delete items in the delete array
            For i As Integer = 0 To lDeleteItems - 1
                bDeletePermanent = False

                If m_oChaseCycleItem.DirectDelete(v_lChaseCycleItemID:=Conversion.Val(CStr(lDeleteItemsArray(i))), v_bDeletePermanent:=bDeletePermanent) <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oChaseCycleItem.DirectDelete call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleItems")
                    Return result
                End If
            Next i

            'Now let update the database if updated the array
            ' Loop through all Chase Cycle Items found

            For i As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)

                If gPMFunctions.ToSafeDouble(vResultArray(kbCCItemUpdated, i)) = 1 Then

                    If m_oChaseCycleItem.DirectEdit(v_vChaseCycleItemID:=Conversion.Val(CStr(vResultArray(kbCCItemID, i))), v_vChaseCycleReason:=IIf(CStr(vResultArray(kbCCReason, i)) <> "", vResultArray(kbCCReason, i), DBNull.Value), v_vInsuranceFolderCnt:=IIf(CStr(vResultArray(kbCCInsuranceFolderCnt, i)) <> "", vResultArray(kbCCInsuranceFolderCnt, i), DBNull.Value), v_vInsuranceFileCnt:=IIf(CStr(vResultArray(kbCCInsuranceFileCnt, i)) <> "", vResultArray(kbCCInsuranceFileCnt, i), DBNull.Value), v_vCanAutoCancel:=IIf(CStr(vResultArray(kbCCCanAutoCancel, i)) <> "", vResultArray(kbCCCanAutoCancel, i), DBNull.Value), v_vWillAutoCancel:=IIf(CStr(vResultArray(kbCCWillAutoCancel, i)) <> "", vResultArray(kbCCWillAutoCancel, i), DBNull.Value), v_vChaseCycleStepID:=IIf(CStr(vResultArray(kbCCStepID, i)) <> "", vResultArray(kbCCStepID, i), DBNull.Value), v_vCreatedDate:=IIf(CStr(vResultArray(kbCCCreatedDate, i)) <> "", vResultArray(kbCCCreatedDate, i), DBNull.Value), v_vDueDate:=IIf(CStr(vResultArray(kbCCDueDate, i)) <> "", vResultArray(kbCCDueDate, i), DBNull.Value), v_vLetterSent:=IIf(CStr(vResultArray(kbCCLetterSent, i)) <> "", vResultArray(kbCCLetterSent, i), DBNull.Value), v_vPMUserGroupId:=IIf(CStr(vResultArray(kbCCpmuser_group_id, i)) <> "", vResultArray(kbCCpmuser_group_id, i), DBNull.Value), v_vPMUserId:=IIf(CStr(vResultArray(kbCCpmuser_id, i)) <> "", vResultArray(kbCCpmuser_id, i), DBNull.Value), v_vIsDeleted:=IIf(CStr(vResultArray(kbCCis_deleted, i)) <> "", vResultArray(kbCCis_deleted, i), DBNull.Value)) <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Chase Cycle Item ID=" & CStr(vResultArray(kbCCItemID, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If
                End If
            Next

            ' Kill the instance of the Chase Cycle object

            m_oChaseCycle.Dispose()
            m_oChaseCycle = Nothing

            ' Kill the instance of the Chase Cycle Item object

            m_oChaseCycleItem.Dispose()
            m_oChaseCycleItem = Nothing


            result = gPMConstants.PMEReturnCode.PMTrue
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        m_oChaseCycle = Nothing
        m_oChaseCycleItem = Nothing
        Return result

    End Function

    '*********************************************************************
    ' Name: Start
    '
    ' Description: Start process for use case
    '
    '*********************************************************************
    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData As Object) As Integer

        Dim result As Integer = 0
        Dim sBranchCode As String = ""
        Dim vResults As Object
        Dim luBound As Integer
        Dim bDBTransStarted As Boolean

        Const k_Header_Values As Integer = 1

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            'We need valid database and business objects
            If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Business and Database object are not set")
            End If

            'OK do the Export processing...

            ' Assign the values

            sBranchCode = CStr(r_vHeaderData(k_Header_Values)(kbHDBranch))

            If r_vHeaderData(k_Header_Values).GetUpperBound(0) = kbHDArchiveDoc Then
                'We have the 12th element set the module level variable

                m_dAsOfDate = CDate(r_vHeaderData(k_Header_Values)(kbHDAsOfDate))

                m_bSpoolDoc = CBool(r_vHeaderData(k_Header_Values)(kbHDSpoolDoc))

                m_bArchiveDoc = CBool(r_vHeaderData(k_Header_Values)(kbHDArchiveDoc))
            Else
                'set it to default Value
                m_dAsOfDate = CDate("1/1/1900")
                m_bSpoolDoc = False
                m_bArchiveDoc = False
            End If
            CreateBatch()
            'Begin a transaction
            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to begin transaction")
            End If

            bDBTransStarted = True

            ' Get the Chase Cycle items from the database based on the
            ' passed criteria
            m_lReturn = CType(GetChaseCycleItems(v_sBranchCode:=sBranchCode), gPMConstants.PMEReturnCode)

            Select Case m_lReturn
                Case gPMConstants.PMEReturnCode.PMTrue

                    result = gPMConstants.PMEReturnCode.PMTrue
                    'commit transaction
                    If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to commit transaction")
                    End If
                    UpdateBatchTask(kBatchStatusComplete, m_iBatchID, m_nTotalRecords, 0)
                    bDBTransStarted = False

                Case gPMConstants.PMEReturnCode.PMNotFound

                    result = gPMConstants.PMEReturnCode.PMNotFound
                    bDBTransStarted = False ' set this before doing rollback to avoid it being called again by error handler if it fails
                    'rollback transaction
                    If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to rollback transaction")
                    End If
                    UpdateBatchTask(kBatchStatusComplete, m_iBatchID, 0, 0)
                Case Else

                    result = gPMConstants.PMEReturnCode.PMFalse
                    
                    bDBTransStarted = False ' set this before doing rollback to avoid it being called again by error handler if it fails
                    'rollback transaction
                    If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to rollback transaction")
                    End If
                    UpdateBatchTask(kBatchStatusFailed, m_iBatchID, 0, 0)
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Call to GetChaseCycleItems failed")
            End Select

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE

            Return result

        Catch excep As System.Exception
            
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If bDBTransStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If

            UpdateBatchTask(kBatchStatusFailed, m_iBatchID, 0, 0)
            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ProcessChaseCycleItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function ProcessChaseCycleItem(ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByRef r_vClientItemsArray() As Object, ByRef r_lClientItems As Integer, ByRef r_vAutoCancelItemsArray() As Object, ByRef r_lAutoCancelItems As Integer, ByRef r_vDeleteItemsArray() As Object, ByRef r_lDeleteItems As Integer) As Integer


        Dim result As Integer = 0
        Const kMethodName As String = "ProcessChaseCycleItem"

        Dim lChaseCycleItemID As Integer
        Dim dDueDate As Date
        Dim iRecurringDays As Integer
        Dim bItemUpdated, bCanAutoCancel, bDirectPolicy As Boolean
        Dim dCompareDate As Date
        Dim bProcessThis, bFirstProcessedItemForPolicy, bFirstProcessedItemForVersion As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lNextStepId, lPMWrkTaskInstanceCnt As Integer

        Static vPreviousItemArrayIndex As Object
        Static vPreviousProcessedItemArrayIndex As Object

        Dim bGenerateDocument As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Store oft-used details
            bItemUpdated = False

            dDueDate = CDate(r_vItemArray(kbCCDueDate, i))

            lChaseCycleItemID = CInt(Conversion.Val(CStr(r_vItemArray(kbCCItemID, i))))

            bGenerateDocument = True

            If m_dAsOfDate > CDate("1/1/1900") Then
                dCompareDate = m_dAsOfDate
            Else
                dCompareDate = DateTime.Today
            End If

            ' Check if an action is due to be taken
            If dDueDate <= dCompareDate Then
                ' this item is now due
                bProcessThis = True
            End If


            ' if the Chase Cycle item is to be processed
            If bProcessThis Then

                ' Is this the fist item to be processed for a policy or policy version
                bFirstProcessedItemForPolicy = True
                bFirstProcessedItemForVersion = True
                m_nTotalRecords += 1

                '***********************************************************
                ' POLICY LEVEL ACTIONS

                ' if this is not the first item processed against this policy
                If Not bFirstProcessedItemForPolicy Then

                    ' inherit the auto cancel values from the previous processed item for this policy
                    ' NB - any items set to auto cancel will have been sorted first
                    ' so that subsequent items will use these same values

                    bCanAutoCancel = CBool(r_vItemArray(kbCCWillAutoCancel, vPreviousProcessedItemArrayIndex))



                    r_vItemArray(kbCCAutoCancelPolicy, i) = r_vItemArray(kbCCAutoCancelPolicy, vPreviousProcessedItemArrayIndex)

                Else
                    ' This the first item for this policy to be processed
                    ' NB: the following actions will only be done once per policy

                    ' Check if the policy can be auto cancelled



                    If (CStr(r_vItemArray(kbCCAutoCancelPolicy, i)) = "1" Or CStr(r_vItemArray(kbCCCheckAutoCancel, i)) = "1") Or CStr(r_vItemArray(kbCCCanAutoCancel, i)) = "1" Then

                        ' Check the Auto Cancel rules

                        lReturn = m_oChaseCycle.AutoCancel(v_lChaseCycleItemId:=lChaseCycleItemID, v_bCheckRulesOnly:=True, r_bAutoCancelResult:=bCanAutoCancel, v_bArchiveDoc:=m_bArchiveDoc, v_bSpoolDoc:=m_bSpoolDoc)

                        ' if no outstanding transactions have been found
                        ' against the account associated with the current Chase Cycle item
                        If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            ' set the is_deleted flag against the Chase Cycle item
                            ' as there is no longer any debt to pursue. The policy has
                            ' been fully paid.

                            r_vItemArray(kbCCis_deleted, i) = 1

                            ' indicate the item has been updated and needs updating on the db

                            r_vItemArray(kbCCItemUpdated, i) = 1

                            ' leave the function via the finally label to ensure all clean up actions take place.
                            Return lReturn

                        ElseIf lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "m_oChaseCycle.AutoCancel call failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    ' reset the bCanAutoCancel indicator if a live MTA is on the policy
                    ' NB: auto-cancel is not allowed if a live MTA is on the Policy

                    ' if the policy can be autocancelled
                    If (Conversion.Val(CStr(r_vItemArray(kbCCPMWrkTaskID, i))) > 0 And Conversion.Val(CStr(r_vItemArray(kbCCPMUserGroupID, i))) > 0) AndAlso r_vItemArray(kbCCWillAutoCancel, i) = "0" Then

                        ' create the task defined against the Chase Cycle step
                        If Conversion.Val(CStr(r_vItemArray(kbCCLetterSent, i))) = 0 Then

                            lReturn = m_oChaseCycle.CreateTask(v_lPMWrkTaskID:=Conversion.Val(CStr(r_vItemArray(kbCCPMWrkTaskID, i))), v_lPMWrkTaskGroupID:=IIf(CStr(r_vItemArray(kbCCStepPMWrkTaskGroupId, i)) = "", 1, r_vItemArray(kbCCStepPMWrkTaskGroupId, i)), v_sCustomer:=CStr(r_vItemArray(kbCCCustomerName, i)).Trim(), v_dtTaskDueDate:=DateTime.Now, v_lPMUserGroupID:=Conversion.Val(CStr(r_vItemArray(kbCCPMUserGroupID, i))), v_sDescription:=r_vItemArray(kbCCStepDescription, i), v_iTaskStatus:=0, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=lPMWrkTaskInstanceCnt)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "CreateTask Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If

                    If bCanAutoCancel Then

                        ' Update the Item with will_auto_cancel = 1

                        r_vItemArray(kbCCWillAutoCancel, i) = "1"

                        ' indicate the item has been updated and needs updating on the db
                        bItemUpdated = True

                        '************************************
                        ' Add the Item to the auto cancel array
                        r_lAutoCancelItems += 1
                        ReDim Preserve r_vAutoCancelItemsArray(r_lAutoCancelItems - 1)

                        r_vAutoCancelItemsArray(r_lAutoCancelItems - 1) = lChaseCycleItemID
                        '************************************
                    End If
                End If

                ' END OF POLICY LEVEL ACTIONS
                '***********************************************************

                'Run_Auto_Cancel_Rules checked in Chase Cycle
                If r_vItemArray(kbCCAutoCancelPolicy, i) = 1 And bCanAutoCancel = False Then
                    bGenerateDocument = False
                End If

                '***********************************************************
                ' POLICY VERSION LEVEL ACTIONS

                ' if this is the first item to be processed for this policy_version (insurance_file_cnt)
                ' or previous item was balance instalment
                If bFirstProcessedItemForVersion And bGenerateDocument = True Then

                    ' the following actions are only done once per policy_version

                    ' Check if a client letter is due to be sent
                    ' NB: This process does not actually take into account whether a valid template id
                    ' has been specified on the Chase Cycle step,
                    ' the logic to do this is in the produceclientletters function called
                    ' later in the process



                    If Conversion.Val(CStr(r_vItemArray(kbCCLetterSent, i))) = 0 Then

                        r_lClientItems += 1
                        ReDim Preserve r_vClientItemsArray(r_lClientItems - 1)

                        r_vClientItemsArray(r_lClientItems - 1) = lChaseCycleItemID

                        ' Set letter sent
                        r_vItemArray(kbCCLetterSent, i) = "1"
                        bItemUpdated = True
                    End If

                End If

                ' END OF POLICY VERSION LEVEL ACTIONS
                '***********************************************************

                ' We are no longer processing just the first item for a policy

                ' store next step indicators

                lNextStepId = CInt(Conversion.Val(CStr(r_vItemArray(kbCCNextStepID, i))))

                ' If applicable, auto cancel the policy

                If bCanAutoCancel And CStr(r_vItemArray(kbCCAutoCancelPolicy, i)) = "1" Then

                    ' if this is not the first processed item for the policy
                    If Not bFirstProcessedItemForPolicy Then
                        ' do nothing - as the policy will have already been cancelled by an earlier CCI
                    Else
                        ' else this is the first Chase Cycle item for policy
                        ' and so the auto cancel needs to be done here

                        lReturn = m_oChaseCycle.AutoCancel(v_lChaseCycleItemID:=lChaseCycleItemID, v_bCheckRulesOnly:=False, r_bAutoCancelResult:=bCanAutoCancel, v_bArchiveDoc:=m_bArchiveDoc, v_bSpoolDoc:=m_bSpoolDoc)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "m_oChaseCycle.AutoCancel call failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    ' Add the Chase Cycle Item to the delete array
                    ' so that this item can be deleted later in the processing.
                    r_lDeleteItems += 1
                    ReDim Preserve r_vDeleteItemsArray(r_lDeleteItems - 1)

                    'r_vDeleteItemsArray(r_lDeleteItems) = lChaseCycleItemID
                    r_vDeleteItemsArray(r_lDeleteItems - 1) = lChaseCycleItemID

                    ' indicate the item has been updated and needs updating on the db
                    bItemUpdated = False
                ElseIf lNextStepId <> 0 Then
                    ' update the step id to the next step id

                    r_vItemArray(kbCCStepID, i) = lNextStepId
                    r_vItemArray(kbCCDueDate, i) = dDueDate.AddDays(Conversion.Val(CStr(r_vItemArray(kbCCNumberOfDays, i))))

                    ' reset the Letter Sent Indicator to 0 when moving to next step
                    r_vItemArray(kbCCLetterSent, i) = "0"

                    ' indicate the item has been updated and needs updating on the db
                    bItemUpdated = True

                    ' this is the last step in the Chase Cycle package
                End If

            End If

            'Set the value of ItemUpdated in the array

            r_vItemArray(kbCCItemUpdated, i) = Math.Abs(CInt(bItemUpdated))

            ' store the current index for use when processing the next debt
            vPreviousItemArrayIndex = i

            If bProcessThis Then
                vPreviousProcessedItemArrayIndex = i
            End If


        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)
            Return result
            ' If you want to rollback a transaction or something, do it here
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: UpdateChaseCycleItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    Public Function UpdateChaseCycleItem(ByRef r_vItemArray(,) As Object, ByVal i As Integer, ByVal v_cAmountOwing As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateChaseCycleItem"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oChaseCycleItem.DirectEdit(v_vChaseCycleItemID:=Conversion.Val(CStr(r_vItemArray(kbCCItemID, i))), v_vChaseCycleReason:=IIf(CStr(r_vItemArray(kbCCReason, i)) <> "", r_vItemArray(kbCCReason, i), DBNull.Value), v_vInsuranceFolderCnt:=IIf(CStr(r_vItemArray(kbCCInsuranceFolderCnt, i)) <> "", r_vItemArray(kbCCInsuranceFolderCnt, i), DBNull.Value), v_vInsuranceFileCnt:=IIf(CStr(r_vItemArray(kbCCInsuranceFileCnt, i)) <> "", r_vItemArray(kbCCInsuranceFileCnt, i), DBNull.Value), v_vCanAutoCancel:=IIf(CStr(r_vItemArray(kbCCCanAutoCancel, i)) <> "", r_vItemArray(kbCCCanAutoCancel, i), DBNull.Value), v_vWillAutoCancel:=IIf(CStr(r_vItemArray(kbCCWillAutoCancel, i)) <> "", r_vItemArray(kbCCWillAutoCancel, i), DBNull.Value), v_vChaseCycleStepID:=IIf(CStr(r_vItemArray(kbCCStepID, i)) <> "", r_vItemArray(kbCCStepID, i), DBNull.Value), v_vCreatedDate:=IIf(CStr(r_vItemArray(kbCCCreatedDate, i)) <> "", r_vItemArray(kbCCCreatedDate, i), DBNull.Value), v_vDueDate:=IIf(CStr(r_vItemArray(kbCCDueDate, i)) <> "", r_vItemArray(kbCCDueDate, i), DBNull.Value), v_vLetterSent:=IIf(CStr(r_vItemArray(kbCCLetterSent, i)) <> "", r_vItemArray(kbCCLetterSent, i), DBNull.Value), v_vPMUserGroupId:=IIf(CStr(r_vItemArray(kbCCpmuser_group_id, i)) <> "", r_vItemArray(kbCCpmuser_group_id, i), DBNull.Value), v_vPMUserId:=IIf(CStr(r_vItemArray(kbCCpmuser_id, i)) <> "", r_vItemArray(kbCCpmuser_id, i), DBNull.Value), v_vIsDeleted:=IIf(CStr(r_vItemArray(kbCCis_deleted, i)) <> "", r_vItemArray(kbCCis_deleted, i), DBNull.Value))

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' We will log the update failure but continue to loop

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update the Chase Cycle Item ID=" & CStr(r_vItemArray(kbCCItemID, i)), vApp:=ACApp, vClass:=ACClass, vMethod:="GetChaseCycleItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            End If

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)
            Return result
            ' If you want to rollback a transaction or something, do it here

        End Try
        Return result

    End Function
    ''' <summary>
    ''' Create Chase Cycle Batch
    ''' </summary>
    Private Sub CreateBatch()
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_Create_Chase_Cycle_Batch", "Create Chase Cycle Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_Create_Chase_Cycle_Batch'")
            End If
            ' Get batch id
            m_iBatchID = m_oDatabase.Parameters.Item("batch_id").Value
    End Sub
    ''' <summary>
    ''' Update Batch Task
    ''' </summary>
    ''' <param name="sBatchStatusCode"></param>
    ''' <param name="nBatchId"></param>
    ''' <param name="nTotal_Transactions"></param>
    ''' <param name="nReject_transactions"></param>
    Public Sub UpdateBatchTask(ByVal sBatchStatusCode As String, ByVal nBatchId As Integer, ByVal nTotal_Transactions As Integer, ByVal nReject_transactions As Integer)
        Dim nReturnValue As Integer
        Try
            AddParameterLite(m_oDatabase, "Batch_Id", nBatchId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "FileName", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "BatchStatusCode", sBatchStatusCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "Total_Transactions", nTotal_Transactions, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "Reject_Transactions", nReject_transactions, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            nReturnValue = m_oDatabase.SQLSelect("spu_Update_BatchTask", "Update Batch in Batch", True)
            If nReturnValue <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_Update_BatchTask'")
            End If
        Catch ex As Exception
            Throw New Exception("Unable to update entry in Batch", ex)
        End Try
    End Sub
End Class

